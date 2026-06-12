using System;

namespace DesignPatterns
{
    /// <summary>
    /// Exercise 1: Implementing a Thread-Safe Singleton Database Connection Manager
    /// </summary>
    public sealed class DatabaseConnectionManager
    {
        private readonly string _connectionString;

        // 1. Use Lazy<T> for automatic thread-safe, lazy initialization.
        // This ensures the instance is only created when Value is first accessed.
        private static readonly Lazy<DatabaseConnectionManager> _lazyInstance =
            new Lazy<DatabaseConnectionManager>(() => new DatabaseConnectionManager());

        // 2. A private constructor prevents external instantiations via 'new'
        private DatabaseConnectionManager()
        {
            _connectionString = "Data Source=market_db.db;Version=3;";
            Console.WriteLine(">>> Core Database Connection Engine initialized successfully. <<<");
        }

        // 3. Public access point to retrieve the single instance
        public static DatabaseConnectionManager Instance => _lazyInstance.Value;

        // A sample operational method to show the singleton in action
        public void ExecuteQuery(string sql)
        {
            Console.WriteLine($"Executing on [{_connectionString}]: {sql}");
        }
    }

    // --- VERIFICATION RUNNER ---
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Requesting Connection Instance 1 ---");
            DatabaseConnectionManager connection1 = DatabaseConnectionManager.Instance;

            Console.WriteLine("\n--- Requesting Connection Instance 2 ---");
            DatabaseConnectionManager connection2 = DatabaseConnectionManager.Instance;

            Console.WriteLine("\n--- Verification Evaluation Matrix ---");
            
            // ReferenceEquals checks if both variables point to the exact same object instance in memory
            if (ReferenceEquals(connection1, connection2))
            {
                Console.WriteLine("SUCCESS: Both variables point to the identical instance location.");
                Console.WriteLine($"Memory Hash 1: {connection1.GetHashCode()}");
                Console.WriteLine($"Memory Hash 2: {connection2.GetHashCode()}");
            }
            else
            {
                Console.WriteLine("FAILURE: Multiple instances exist in memory!");
            }

            // Use the instance
            Console.WriteLine();
            connection1.ExecuteQuery("SELECT * FROM Products;");
        }
    }
}