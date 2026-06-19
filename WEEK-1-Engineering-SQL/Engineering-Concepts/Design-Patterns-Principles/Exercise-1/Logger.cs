using System;

public sealed class Logger
{
    // Single instance of Logger
    private static Logger instance = null;

    // Lock object for thread safety
    private static readonly object lockObject = new object();

    // Private constructor
    private Logger()
    {
        Console.WriteLine("Logger Instance Created");
    }

    // Public method to get the single instance
    public static Logger GetInstance()
    {
        if (instance == null)
        {
            lock (lockObject)
            {
                if (instance == null)
                {
                    instance = new Logger();
                }
            }
        }

        return instance;
    }

    // Logging method
    public void Log(string message)
    {
        Console.WriteLine($"Log: {message}");
    }
}