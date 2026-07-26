using System;

namespace AdapterPatternExample
{
    // --- STEP 2: DEFINE TARGET INTERFACE ---
    // This is the interface your system expects to work with.
    public interface IPaymentProcessor
    {
        void ProcessPayment(double amount);
    }


    // --- STEP 3: IMPLEMENT ADAPTEE CLASSES ---
    // These represent third-party SDKs with completely incompatible method structures.

    // Adaptee 1: PayPal SDK
    public class PayPalGateway
    {
        public void MakePayment(double totalAmount)
        {
            Console.WriteLine($"[PayPal SDK] Safely authorized and processed a payment of ${totalAmount:F2}");
        }
    }

    // Adaptee 2: Stripe SDK
    public class StripeGateway
    {
        public void ChargeCustomer(string currency, int amountInCents)
        {
            double dollars = amountInCents / 100.0;
            Console.WriteLine($"[Stripe SDK] Captured charge of {currency}{dollars:F2} via Stripe APIs.");
        }
    }


    // --- STEP 4: IMPLEMENT THE ADAPTER CLASSES ---
    // These classes implement the target interface and translate the calls to the Adaptees.

    // Adapter for PayPal
    public class PayPalAdapter : IPaymentProcessor
    {
        private readonly PayPalGateway _payPalGateway;

        public PayPalAdapter(PayPalGateway payPalGateway)
        {
            _payPalGateway = payPalGateway;
        }

        public void ProcessPayment(double amount)
        {
            // Direct translation
            _payPalGateway.MakePayment(amount);
        }
    }

    // Adapter for Stripe
    public class StripeAdapter : IPaymentProcessor
    {
        private readonly StripeGateway _stripeGateway;

        public StripeAdapter(StripeGateway stripeGateway)
        {
            _stripeGateway = stripeGateway;
        }

        public void ProcessPayment(double amount)
        {
            // Translation: Stripe expects amounts in cents, and a currency code string
            int amountInCents = (int)(amount * 100);
            _stripeGateway.ChargeCustomer("USD", amountInCents);
        }
    }


    // --- STEP 5: TEST THE ADAPTER IMPLEMENTATION ---
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- E-Commerce Checkout System ---\n");

            double cartTotal = 149.99;

            // Scenario A: Client wants to pay using PayPal
            PayPalGateway externalPayPalSdk = new PayPalGateway();
            IPaymentProcessor payPalProcessor = new PayPalAdapter(externalPayPalSdk);
            
            Console.WriteLine("Executing checkout via PayPal:");
            ExecuteCheckout(payPalProcessor, cartTotal);


            // Scenario B: Client wants to pay using Stripe
            StripeGateway externalStripeSdk = new StripeGateway();
            IPaymentProcessor stripeProcessor = new StripeAdapter(externalStripeSdk);

            Console.WriteLine("Executing checkout via Stripe:");
            ExecuteCheckout(stripeProcessor, cartTotal);
        }

        // This client method only understands the IPaymentProcessor interface.
        // It has no idea (and shouldn't care) that PayPal or Stripe are running behind the scenes.
        static void ExecuteCheckout(IPaymentProcessor processor, double amount)
        {
            processor.ProcessPayment(amount);
            Console.WriteLine("Checkout Status: SUCCESS\n");
        }
    }
}