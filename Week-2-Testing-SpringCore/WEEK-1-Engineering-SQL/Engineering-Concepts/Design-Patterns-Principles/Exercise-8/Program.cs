using System;

namespace StrategyPatternExample
{
    // --- STEP 2: DEFINE STRATEGY INTERFACE ---
    public interface IPaymentStrategy
    {
        void Pay(double amount);
    }


    // --- STEP 3: IMPLEMENT CONCRETE STRATEGIES ---

    // Concrete Strategy 1: Credit Card
    public class CreditCardPayment : IPaymentStrategy
    {
        private readonly string _name;
        private readonly string _cardNumber;
        private readonly string _cvv;
        private readonly string _dateOfExpiry;

        public CreditCardPayment(string name, string cardNumber, string cvv, string dateOfExpiry)
        {
            _name = name;
            _cardNumber = cardNumber;
            _cvv = cvv;
            _dateOfExpiry = dateOfExpiry;
        }

        public void Pay(double amount)
        {
            // Masking card number for secure display output
            string maskedCard = _cardNumber.Substring(_cardNumber.Length - 4);
            Console.WriteLine($"[Credit Card] Paid ${amount:F2} using Card ending in **{maskedCard} (Holder: {_name}).");
        }
    }

    // Concrete Strategy 2: PayPal
    public class PayPalPayment : IPaymentStrategy
    {
        private readonly string _emailId;
        private readonly string _password;

        public PayPalPayment(string emailId, string password)
        {
            _emailId = emailId;
            _password = password; // In real-world apps, use secure tokens instead of raw passwords!
        }

        public void Pay(double amount)
        {
            Console.WriteLine($"[PayPal] Paid ${amount:F2} successfully from account: {_emailId}.");
        }
    }


    // --- STEP 4: IMPLEMENT CONTEXT CLASS ---
    // The context interacts with the Strategy interface instead of concrete classes.
    public class PaymentContext
    {
        private IPaymentStrategy _paymentStrategy;

        // Allows setting or changing the strategy dynamically at runtime
        public void SetPaymentStrategy(IPaymentStrategy strategy)
        {
            _paymentStrategy = strategy;
        }

        // Executes the strategy
        public void ExecutePayment(double amount)
        {
            if (_paymentStrategy == null)
            {
                Console.WriteLine("[Error] No payment method selected!");
                return;
            }
            
            _paymentStrategy.Pay(amount);
        }
    }


    // --- STEP 5: TEST THE STRATEGY IMPLEMENTATION ---
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- E-Commerce Dynamic Checkout Hub ---\n");

            // Create the context (the shopping cart checkout engine)
            PaymentContext checkout = new PaymentContext();
            double cartAmount = 249.50;

            // Scenario A: User selects Credit Card at checkout
            Console.WriteLine("User selects: Credit Card Option");
            IPaymentStrategy creditCard = new CreditCardPayment("John Doe", "1234567890123456", "123", "12/28");
            checkout.SetPaymentStrategy(creditCard);
            
            // Execute business logic
            checkout.ExecutePayment(cartAmount);
            Console.WriteLine(new string('-', 55) + "\n");


            // Scenario B: User changes their mind or a different user selects PayPal
            Console.WriteLine("User switches to / selects: PayPal Option");
            IPaymentStrategy payPal = new PayPalPayment("john.doe@example.com", "securePassword123");
            checkout.SetPaymentStrategy(payPal);
            
            // Execute the exact same method, but notice how the behavior swaps instantly!
            checkout.ExecutePayment(cartAmount);
            Console.WriteLine(new string('-', 55));
        }
    }
}