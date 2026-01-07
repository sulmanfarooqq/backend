using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetLearningDemo
{
    // ====================== CLASS AND STRUCT DEFINITION ======================
    // Classes are reference types that 
    // contain properties, methods, events, etc.
    public class User
    {
        // Properties - public fields that can be accessed from outside
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        
        // Constructor - initializes object when created
        public User(int id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }
        
        // Methods - functions that belong to the class
        public void DisplayInfo()
        {
            Console.WriteLine($"User: {Name}, Email: {Email}");
        }
    }

    // Structs are value types, typically used for small data structures
    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public double DistanceFromOrigin()
        {
            return Math.Sqrt(X * X + Y * Y);
        }
    }

    // Interface - defines contracts that classes can implement
    public interface IShape
    {
        double CalculateArea();
        void Draw();
    }

    // Enum - defines a set of named constants
    public enum Status
    {
        Active = 1,
        Inactive = 2,
        Pending = 3
    }

    // ====================== MAIN APPLICATION CLASS ======================
    class Program
    {
        // Entry point of the application
        static void Main(string[] args)
        {
            Console.WriteLine("=== C# Application Concepts Demo ===\n");

            // ====================== VARIABLES AND DATA TYPES ======================
            Console.WriteLine("--- Variables and Data Types ---");
            
            // Basic data types
            int integerNumber = 42;           // 32-bit integer
            long longNumber = 1234567890L;    // 64-bit integer
            float floatNumber = 3.14f;        // Single precision floating point
            double doubleNumber = 3.14159;    // Double precision floating point
            decimal decimalNumber = 123.45m;  // Decimal type for financial calculations
            bool booleanValue = true;         // Boolean (true/false)
            char character = 'A';             // Single character
            string text = "Hello, World!";    // String of characters
            
            Console.WriteLine($"Integer: {integerNumber}");
            Console.WriteLine($"Double: {doubleNumber}");
            Console.WriteLine($"Boolean: {booleanValue}");
            Console.WriteLine($"String: {text}\n");

            // ====================== ARRAYS AND COLLECTIONS ======================
            Console.WriteLine("--- Arrays and Collections ---");
            
            // Array - fixed size collection of same type
            int[] numbers = { 1, 2, 3, 4, 5 };
            
            // List - dynamic array that can grow/shrink
            List<string> names = new List<string> { "Alice", "Bob", "Charlie" };
            
            // Dictionary - key-value pairs
            Dictionary<string, int> ages = new Dictionary<string, int>
            {
                { "Alice", 25 },
                { "Bob", 30 },
                { "Charlie", 35 }
            };
            
            Console.WriteLine($"Array length: {numbers.Length}");
            Console.WriteLine($"List count: {names.Count}");
            Console.WriteLine($"Dictionary count: {ages.Count}\n");

            // ====================== OPERATORS ======================
            Console.WriteLine("--- Operators ---");
            
            int a = 10, b = 3;
            Console.WriteLine($"Addition: {a} + {b} = {a + b}");
            Console.WriteLine($"Subtraction: {a} - {b} = {a - b}");
            Console.WriteLine($"Multiplication: {a} * {b} = {a * b}");
            Console.WriteLine($"Division: {a} / {b} = {a / b}");
            Console.WriteLine($"Modulus: {a} % {b} = {a % b}");
            Console.WriteLine($"Increment: {a}++ = {a++} (then {a})");
            Console.WriteLine($"Logical AND: true && false = {true && false}");
            Console.WriteLine($"Logical OR: true || false = {true || false}\n");

            // ====================== CONDITIONAL STATEMENTS ======================
            Console.WriteLine("--- Conditional Statements ---");
            
            // If-else statement
            if (integerNumber > 40)
            {
                Console.WriteLine($"{integerNumber} is greater than 40");
            }
            else if (integerNumber == 40)
            {
                Console.WriteLine($"{integerNumber} equals 40");
            }
            else
            {
                Console.WriteLine($"{integerNumber} is less than 40");
            }
            
            // Switch statement
            Status userStatus = Status.Active;
            switch (userStatus)
            {
                case Status.Active:
                    Console.WriteLine("User is active");
                    break;
                case Status.Inactive:
                    Console.WriteLine("User is inactive");
                    break;
                case Status.Pending:
                    Console.WriteLine("User is pending");
                    break;
                default:
                    Console.WriteLine("Unknown status");
                    break;
            }
            
            // Ternary operator
            string accessLevel = userStatus == Status.Active ? "Full Access" : "Limited Access";
            Console.WriteLine($"Access Level: {accessLevel}\n");

            // ====================== LOOPS ======================
            Console.WriteLine("--- Loops ---");
            
            // For loop
            Console.Write("For loop: ");
            for (int i = 0; i < 5; i++)
            {
                Console.Write($"{i} ");
            }
            Console.WriteLine();
            
            // While loop
            Console.Write("While loop: ");
            int counter = 0;
            while (counter < 5)
            {
                Console.Write($"{counter} ");
                counter++;
            }
            Console.WriteLine();
            
            // Foreach loop
            Console.Write("Foreach loop: ");
            foreach (string name in names)
            {
                Console.Write($"{name} ");
            }
            Console.WriteLine("\n");

            // ====================== OBJECT-ORIENTED PROGRAMMING ======================
            Console.WriteLine("--- Object-Oriented Programming ---");
            
            // Create objects using classes
            User user1 = new User(1, "John Doe", "john@example.com");
            User user2 = new User(2, "Jane Smith", "jane@example.com");
            
            user1.DisplayInfo();
            user2.DisplayInfo();
            
            // Using structs
            Point point = new Point(10, 20);
            Console.WriteLine($"Point coordinates: ({point.X}, {point.Y})");
            Console.WriteLine($"Distance from origin: {point.DistanceFromOrigin():F2}\n");

            // ====================== EXCEPTION HANDLING ======================
            Console.WriteLine("--- Exception Handling ---");
            
            try
            {
                int divisionResult = 10 / 0; // This will throw an exception
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine($"Exception caught: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Finally block always executes");
            }
            Console.WriteLine();

            // ====================== LAMBDA EXPRESSIONS AND LINQ ======================
            Console.WriteLine("--- Lambda Expressions and LINQ ---");
            
            // Lambda expression
            Func<int, int, int> add = (x, y) => x + y;
            Console.WriteLine($"Lambda result: {add(5, 3)}");
            
            // LINQ - Language Integrated Query
            var evenNumbers = numbers.Where(n => n % 2 == 0).ToList();
            Console.Write("Even numbers from array: ");
            evenNumbers.ForEach(n => Console.Write($"{n} "));
            Console.WriteLine("\n");

            // ====================== ASYNCHRONOUS PROGRAMMING ======================
            Console.WriteLine("--- Asynchronous Programming ---");
            
            // Async method call
            AsyncExample().Wait(); // Wait for async operation to complete
            
            // ====================== EVENTS AND DELEGATES ======================
            Console.WriteLine("--- Events and Delegates ---");
            
            // Create an instance of the event publisher
            Button button = new Button();
            
            // Subscribe to the event
            button.Click += Button_Clicked;
            
            // Trigger the event
            button.Press();
            Console.WriteLine();

            // ====================== GENERICS ======================
            Console.WriteLine("--- Generics ---");
            
            // Generic classes allow type-safe operations on different types
            GenericContainer<string> stringContainer = new GenericContainer<string>("Hello");
            GenericContainer<int> intContainer = new GenericContainer<int>(42);
            
            Console.WriteLine($"String container: {stringContainer.Value}");
            Console.WriteLine($"Int container: {intContainer.Value}\n");

            // ====================== DISPOSABLE PATTERN ======================
            Console.WriteLine("--- Disposable Pattern ---");
            
            // Using 'using' statement automatically calls Dispose()
            using (DisposableExample disposable = new DisposableExample())
            {
                Console.WriteLine("Inside using block");
            }
            Console.WriteLine("Outside using block - Dispose was automatically called\n");

            // ====================== ENUMERABLES AND ENUMERATORS ======================
            Console.WriteLine("--- Enumerables and Enumerators ---");
            
            // Custom enumerable class
            NumberSequence sequence = new NumberSequence(5);
            foreach (int num in sequence)
            {
                Console.Write($"{num} ");
            }
            Console.WriteLine("\n");

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        // Async method example
        static async Task AsyncExample()
        {
            Console.WriteLine("Starting async operation...");
            
            // Simulate async work
            await Task.Delay(1000);
            
            Console.WriteLine("Async operation completed!");
        }

        // Event handler
        static void Button_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("Button was clicked!");
        }
    }

    // ====================== EVENT PUBLISHER AND SUBSCRIBER ======================
    // Delegate - type-safe function pointer
    public delegate void ClickEventHandler(object sender, EventArgs e);

    // Event publisher class
    public class Button
    {
        // Event - allows other classes to register methods to be called when event occurs
        public event ClickEventHandler Click;

        // Method to trigger the event
        public void Press()
        {
            Console.WriteLine("Button pressed");
            // Check if any subscribers and trigger the event
            Click?.Invoke(this, EventArgs.Empty);
        }
    }

    // ====================== GENERIC CLASS ======================
    // Generic class - allows type-safe operations on different types
    public class GenericContainer<T>
    {
        public T Value { get; set; }

        public GenericContainer(T value)
        {
            Value = value;
        }
    }

    // ====================== DISPOSABLE CLASS ======================
    // Implements IDisposable to clean up resources
    public class DisposableExample : IDisposable
    {
        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                // Clean up managed resources here
                Console.WriteLine("Disposing resources...");
                disposed = true;
            }
        }

        ~DisposableExample()
        {
            Dispose(false);
        }
    }

    // ====================== CUSTOM ENUMERABLE ======================
    public class NumberSequence : IEnumerable<int>
    {
        private int count;

        public NumberSequence(int count)
        {
            this.count = count;
        }

        public IEnumerator<int> GetEnumerator()
        {
            for (int i = 0; i < count; i++)
            {
                yield return i;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}