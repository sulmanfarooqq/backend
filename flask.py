# -------------------- IMPORTS --------------------

# Flask core objects:
# Flask -> main application object
# request -> incoming HTTP request data (JSON, headers, params)
# jsonify -> convert Python dicts to JSON HTTP responses
# g -> global request-level storage (used for auth, middleware)
from flask import Flask, request, jsonify, g

# SQLAlchemy -> ORM for database interaction
from flask_sqlalchemy import SQLAlchemy

# wraps -> preserves function metadata when using decorators
from functools import wraps

# os -> environment variables (used in real apps for secrets)
import os

# datetime -> timestamps, token expiry
import datetime

# jwt -> JSON Web Tokens for authentication
import jwt


# -------------------- APP CONFIGURATION --------------------

# Create Flask application instance
# This represents your backend server
app = Flask(__name__)

# Secret key used for signing tokens and sessions
# In real backend apps, this MUST come from environment variables
app.config["SECRET_KEY"] = "super-secret-key"

# Database connection string
# sqlite used here for simplicity (PostgreSQL in real production)
app.config["SQLALCHEMY_DATABASE_URI"] = "sqlite:///app.db"

# Disable unnecessary overhead
app.config["SQLALCHEMY_TRACK_MODIFICATIONS"] = False

# Initialize ORM with Flask app
# This allows Python classes to map to database tables
db = SQLAlchemy(app)


# -------------------- DATABASE MODELS --------------------

# User table
# Each class = one database table
class User(db.Model):
    # Primary key column (unique user id)
    id = db.Column(db.Integer, primary_key=True)

    # Username field (must be unique)
    username = db.Column(db.String(80), unique=True, nullable=False)

    # Password field (plain text here ONLY for learning)
    # Real backend uses hashed passwords
    password = db.Column(db.String(120), nullable=False)


# Post table (example resource)
class Post(db.Model):
    id = db.Column(db.Integer, primary_key=True)

    # Post title
    title = db.Column(db.String(120))

    # Post content
    content = db.Column(db.Text)

    # Foreign key linking post → user
    user_id = db.Column(db.Integer, db.ForeignKey("user.id"))


# -------------------- MIDDLEWARE --------------------

# Runs BEFORE every request
# Used for logging, auth checks, timing, etc.
@app.before_request
def before_request():
    # Store request start time
    g.start_time = datetime.datetime.utcnow()


# Runs AFTER every request
# Used for modifying response, adding headers, logging
@app.after_request
def after_request(response):
    return response


# -------------------- AUTH UTILITIES --------------------

# Decorator to protect routes (authentication middleware)
def token_required(f):
    @wraps(f)
    def decorated(*args, **kwargs):
        # Read Authorization header
        token = request.headers.get("Authorization")

        # If no token → user is unauthorized
        if not token:
            return jsonify({"error": "Token missing"}), 401

        try:
            # Decode token using secret key
            data = jwt.decode(
                token,
                app.config["SECRET_KEY"],
                algorithms=["HS256"]
            )

            # Store user_id in request context
            g.user_id = data["user_id"]

        except:
            # Token invalid or expired
            return jsonify({"error": "Invalid token"}), 401

        # Proceed to actual route
        return f(*args, **kwargs)

    return decorated


# -------------------- AUTH ROUTES --------------------

# Register a new user
@app.route("/register", methods=["POST"])
def register():
    # Parse JSON body
    data = request.json

    # Create new user object
    user = User(
        username=data["username"],
        password=data["password"]
    )

    # Save to database
    db.session.add(user)
    db.session.commit()

    return jsonify({"message": "User created"}), 201


# Login route
@app.route("/login", methods=["POST"])
def login():
    data = request.json

    # Find user by username
    user = User.query.filter_by(username=data["username"]).first()

    # Validate credentials
    if not user or user.password != data["password"]:
        return jsonify({"error": "Invalid credentials"}), 401

    # Create JWT token
    token = jwt.encode(
        {
            "user_id": user.id,
            "exp": datetime.datetime.utcnow() + datetime.timedelta(hours=1)
        },
        app.config["SECRET_KEY"],
        algorithm="HS256"
    )

    return jsonify({"token": token})


# -------------------- CRUD API ROUTES --------------------

# READ posts (pagination example)
@app.route("/posts", methods=["GET"])
def get_posts():
    # Read query param ?page=1
    page = request.args.get("page", 1, type=int)

    # Items per page
    per_page = 5

    # Paginated query
    posts = Post.query.paginate(page=page, per_page=per_page)

    # Convert ORM objects to JSON
    result = [
        {
            "id": p.id,
            "title": p.title,
            "content": p.content
        }
        for p in posts.items
    ]

    return jsonify(result)


# CREATE post (protected route)
@app.route("/posts", methods=["POST"])
@token_required
def create_post():
    data = request.json

    post = Post(
        title=data["title"],
        content=data["content"],
        user_id=g.user_id  # from token
    )

    db.session.add(post)
    db.session.commit()

    return jsonify({"message": "Post created"}), 201


# UPDATE post
@app.route("/posts/<int:id>", methods=["PUT"])
@token_required
def update_post(id):
    # Fetch post or return 404
    post = Post.query.get_or_404(id)

    data = request.json

    # Update only provided fields
    post.title = data.get("title", post.title)
    post.content = data.get("content", post.content)

    db.session.commit()

    return jsonify({"message": "Post updated"})


# DELETE post
@app.route("/posts/<int:id>", methods=["DELETE"])
@token_required
def delete_post(id):
    post = Post.query.get_or_404(id)

    db.session.delete(post)
    db.session.commit()

    return jsonify({"message": "Post deleted"})


# -------------------- ERROR HANDLING --------------------

# Custom 404 response
@app.errorhandler(404)
def not_found(e):
    return jsonify({"error": "Not found"}), 404


# Custom 500 response
@app.errorhandler(500)
def server_error(e):
    return jsonify({"error": "Server error"}), 500


# -------------------- HEALTH CHECK --------------------

# Used by load balancers / monitoring tools
@app.route("/health")
def health():
    return jsonify({"status": "ok"})


# -------------------- APPLICATION ENTRY --------------------

# Run only when executed directly
if __name__ == "__main__":
    # Create tables if they don't exist
    with app.app_context():
        db.create_all()

    # Start development server
    app.run(debug=True)
