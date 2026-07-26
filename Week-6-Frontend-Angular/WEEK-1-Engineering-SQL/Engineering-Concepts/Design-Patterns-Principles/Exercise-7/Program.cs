using System;
using System.Collections.Generic;

namespace ObserverPatternExample
{
    // --- STEP 4: DEFINE OBSERVER INTERFACE ---
    // Represents the subscribers waiting for updates.
    public interface IObserver
    {
        void Update(string stockSymbol, double price);
    }


    // --- STEP 2: DEFINE SUBJECT INTERFACE ---
    // Represents the publisher that holds the state and manages subscribers.
    public interface IStock
    {
        void RegisterObserver(IObserver observer);
        void DeregisterObserver(IObserver observer);
        void NotifyObservers();
    }


    // --- STEP 3: IMPLEMENT CONCRETE SUBJECT ---
    public class StockMarket : IStock
    {
        // Internal list to keep track of all registered observers
        private readonly List<IObserver> _observers = new List<IObserver>();
        
        private string _stockSymbol;
        private double _price;

        // Method to simulate state changes (Stock Price movements)
        public void SetStockPrice(string stockSymbol, double price)
        {
            _stockSymbol = stockSymbol;
            _price = price;
            Console.WriteLine($"[Market Data] {stockSymbol} ticker updated to ${price:F2}");
            
            // Trigger the notification system automatically whenever a price changes
            NotifyObservers();
        }

        public void RegisterObserver(IObserver observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

        public void DeregisterObserver(IObserver observer)
        {
            if (_observers.Contains(observer))
            {
                _observers.Remove(observer);
            }
        }

        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                // Push the updated stock metrics to every observer
                observer.Update(_stockSymbol, _price);
            }
        }
    }


    // --- STEP 5: IMPLEMENT CONCRETE OBSERVERS ---

    // Concrete Observer 1: Mobile App Display
    public class MobileApp : IObserver
    {
        private readonly string _username;

        public MobileApp(string username)
        {
            _username = username;
        }

        public void Update(string stockSymbol, double price)
        {
            Console.WriteLine($" -> [Mobile Push Notification for {_username}]: {stockSymbol} is now ${price:F2}");
        }
    }

    // Concrete Observer 2: Web Dashboard Display
    public class WebApp : IObserver
    {
        private readonly string _dashboardName;

        public WebApp(string dashboardName)
        {
            _dashboardName = dashboardName;
        }

        public void Update(string stockSymbol, double price)
        {
            Console.WriteLine($" -> [Web Dashboard '{_dashboardName}']: Rendering live ticker chart for {stockSymbol} @ ${price:F2}");
        }
    }


    // --- STEP 6: TEST THE OBSERVER IMPLEMENTATION ---
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Stock Market Notification Engine ---\n");

            // Instantiate our concrete Subject (The Publisher)
            StockMarket nasdaq = new StockMarket();

            // Create some Observers (The Subscribers)
            MobileApp aliceMobile = new MobileApp("Alice");
            MobileApp bobMobile = new MobileApp("Bob");
            WebApp tradingTerminal = new WebApp("WallStreet Pro");

            // Register them to listen to the market
            Console.WriteLine("Registering subscribers...");
            nasdaq.RegisterObserver(aliceMobile);
            nasdaq.RegisterObserver(bobMobile);
            nasdaq.RegisterObserver(tradingTerminal);
            Console.WriteLine(new string('-', 60) + "\n");

            // Simulate stock changes
            nasdaq.SetStockPrice("AAPL (Apple Inc.)", 175.50);
            Console.WriteLine();
            
            nasdaq.SetStockPrice("TSLA (Tesla)", 220.15);
            Console.WriteLine(new string('-', 60) + "\n");

            // Bob decides to close his mobile app (Deregistering)
            Console.WriteLine("Action: Bob closes his app (Deregisters)...\n");
            nasdaq.DeregisterObserver(bobMobile);

            // Simulate another stock change (Bob shouldn't receive this)
            nasdaq.SetStockPrice("MSFT (Microsoft)", 415.80);
            Console.WriteLine(new string('-', 60));
        }
    }
}