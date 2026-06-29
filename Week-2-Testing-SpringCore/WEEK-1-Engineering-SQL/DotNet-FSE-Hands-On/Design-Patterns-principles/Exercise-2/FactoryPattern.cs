using System;

namespace DesignPatterns
{
    // ====================================================================
    // 1. THE PRODUCT INTERFACE
    // Defines the contract that all concrete products must fulfill.
    // ====================================================================
    public interface INotification
    {
        void SendMessage(string message);
    }

    // ====================================================================
    // 2. CONCRETE PRODUCTS
    // Distinct variations of the product interface.
    // ====================================================================
    public class EmailNotification : INotification
    {
        public void SendMessage(string message)
        {
            Console.WriteLine($"[SMTP Email Server] Dispatching secure email: '{message}'");
        }
    }

    public class SmsNotification : INotification
    {
        public void SendMessage(string message)
        {
            Console.WriteLine($"[SMS Cellular Gateway] Broadcasting SMS text alert: '{message}'");
        }
    }

    // ====================================================================
    // 3. THE CREATOR (FACTORY) CLASS
    // Contains the core Factory Method responsible for object instantiation.
    // ====================================================================
    public abstract class NotificationFactory
    {
        // This is the explicit Factory Method
        public abstract INotification CreateNotifier();

        // The factory can also contain internal business logic that operates on products
        public void DeliverNotification(string message)
        {
            // Call the factory method to manufacture a product instance dynamically
            INotification notifier = CreateNotifier();
            notifier.SendMessage(message);
        }
    }

    // ====================================================================
    // 4. CONCRETE CREATORS
    // Overrides the factory method to return specific concrete product instances.
    // ====================================================================
    public class EmailNotificationFactory : NotificationFactory
    {
        public override INotification CreateNotifier()
        {
            return new EmailNotification();
        }
    }

    public class SmsNotificationFactory : NotificationFactory
    {
        public override INotification CreateNotifier()
        {
            return new SmsNotification();
        }
    }

    // ====================================================================
    // --- VERIFICATION RUNNER ---
    // ====================================================================
    class FactoryPatternProgram
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== INITIALIZING FACTORY DELIVERY RIGS ===\n");

            // Imagine this setting comes dynamically from a configuration file or database setting
            string configuredChannel = "SMS"; 
            NotificationFactory activeFactory;

            // Resolve the correct concrete factory on the fly
            if (configuredChannel == "Email")
            {
                activeFactory = new EmailNotificationFactory();
            }
            else
            {
                activeFactory = new SmsNotificationFactory();
            }

            // The client code interacts completely with the abstract interface!
            // It has zero direct knowledge of EmailNotification or SmsNotification.
            activeFactory.DeliverNotification("Your order security verification code is: 4092");

            Console.WriteLine("\n=== SWITCHING CHANNELS TO EMAIL ===");
            activeFactory = new EmailNotificationFactory();
            activeFactory.DeliverNotification("Monthly payroll summary statement report is now downloadable.");
        }
    }
}