using System;

class Program
{
    static void Main(string[] args)
    {
        Logger logger1 = Logger.GetInstance();
        Logger logger2 = Logger.GetInstance();

        logger1.Log("Application Started");
        logger2.Log("User Logged In");

        // Check if both references point to the same object
        if (logger1 == logger2)
        {
            Console.WriteLine("\nOnly one Logger instance exists.");
        }
        else
        {
            Console.WriteLine("\nMultiple Logger instances exist.");
        }

        Console.ReadKey();
    }
}