using System;

namespace DecoratorPatternExample
{
    // --- STEP 2: DEFINE COMPONENT INTERFACE ---
    public interface INotifier
    {
        void Send(string message);
    }


    // --- STEP 3: IMPLEMENT CONCRETE COMPONENT ---
    // This is the core functionality that we will wrap things around.
    public class EmailNotifier : INotifier
    {
        public void Send(string message)
        {
            Console.WriteLine($"[Email] Sending: {message}");
        }
    }


    // --- STEP 4: IMPLEMENT DECORATOR CLASSES ---

    // Abstract Decorator class holding a reference to an INotifier object
    public abstract class NotifierDecorator : INotifier
    {
        protected readonly INotifier _wrappedNotifier;

        protected NotifierDecorator(INotifier notifier)
        {
            _wrappedNotifier = notifier;
        }

        // Virtual so that concrete decorators can override or add behavior
        public virtual void Send(string message)
        {
            _wrappedNotifier.Send(message);
        }
    }

    // Concrete Decorator A: Adds SMS capability
    public class SMSNotifierDecorator : NotifierDecorator
    {
        public SMSNotifierDecorator(INotifier notifier) : base(notifier) { }

        public override void Send(string message)
        {
            // First, let the previous wrappers or component do their job
            base.Send(message); 
            
            // Then, execute this decorator's specific job
            SendSMS(message);
        }

        private void SendSMS(string message)
        {
            Console.WriteLine($"[SMS] Sending Text Alert: {message}");
        }
    }

    // Concrete Decorator B: Adds Slack capability
    public class SlackNotifierDecorator : NotifierDecorator
    {
        public SlackNotifierDecorator(INotifier notifier) : base(notifier) { }

        public override void Send(string message)
        {
            base.Send(message);
            SendSlackMessage(message);
        }

        private void SendSlackMessage(string message)
        {
            Console.WriteLine($"[Slack Channel] Posting Update: {message}");
        }
    }


    // --- STEP 5: TEST THE DECORATOR IMPLEMENTATION ---
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- System Notification Demo ---\n");

            string alertMessage = "CRITICAL: Server CPU usage exceeded 95%!";

            // Scenario 1: standard setup, just email the admin
            Console.WriteLine("Configuring: Standard Email Only");
            INotifier standardChannel = new EmailNotifier();
            standardChannel.Send(alertMessage);
            Console.WriteLine(new string('-', 40));

            // Scenario 2: Emergency! Email + SMS are needed
            Console.WriteLine("Configuring: Email + SMS Alert");
            INotifier criticalChannel = new SMSNotifierDecorator(new EmailNotifier());
            criticalChannel.Send(alertMessage);
            Console.WriteLine(new string('-', 40));

            // Scenario 3: Complete alert stack! Email + SMS + Slack message
            Console.WriteLine("Configuring: Full Stack (Email + SMS + Slack)");
            
            // Note how we nest the objects like Russian nesting dolls
            INotifier omniChannel = new SlackNotifierDecorator(
                                        new SMSNotifierDecorator(
                                            new EmailNotifier()
                                        )
                                    );
                                    
            omniChannel.Send(alertMessage);
            Console.WriteLine(new string('-', 40));
        }
    }
}