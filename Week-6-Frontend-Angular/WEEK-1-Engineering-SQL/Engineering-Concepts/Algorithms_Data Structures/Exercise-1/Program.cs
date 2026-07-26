using System;
using System.Collections.Generic;

namespace InventoryManagementSystem
{
    // --- THE PRODUCT ENTITY ---
    public class Product
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

        public Product(string productId, string productName, int quantity, double price)
        {
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            Price = price;
        }

        public override string ToString()
        {
            return $"ID: {ProductId} | Name: {ProductName,-15} | Stock: {Quantity,-5} | Price: ${Price:F2}";
        }
    }

    // --- THE INVENTORY MANAGEMENT ENGINE ---
    public class Inventory
    {
        // Using a Dictionary (Hash Table) for O(1) lookups via Product ID
        private readonly Dictionary<string, Product> _products = new Dictionary<string, Product>();

        // 1. ADD PRODUCT
        public bool AddProduct(Product product)
        {
            if (product == null || string.IsNullOrWhiteSpace(product.ProductId)) return false;

            // Prevent duplicate product IDs
            if (_products.ContainsKey(product.ProductId))
            {
                Console.WriteLine($"[Error] Cannot add. Product ID '{product.ProductId}' already exists.");
                return false;
            }

            _products.Add(product.ProductId, product);
            Console.WriteLine($"[Success] Added: {product.ProductName}");
            return true;
        }

        // 2. UPDATE PRODUCT STOCK / PRICE
        public bool UpdateProduct(string productId, int newQuantity, double newPrice)
        {
            // Fast lookup check
            if (!_products.TryGetValue(productId, out Product product))
            {
                Console.WriteLine($"[Error] Update failed. Product ID '{productId}' not found.");
                return false;
            }

            product.Quantity = newQuantity;
            product.Price = newPrice;
            Console.WriteLine($"[Success] Updated ID '{productId}': New Stock = {newQuantity}, New Price = ${newPrice:F2}");
            return true;
        }

        // 3. DELETE PRODUCT
        public bool DeleteProduct(string productId)
        {
            if (!_products.ContainsKey(productId))
            {
                Console.WriteLine($"[Error] Delete failed. Product ID '{productId}' not found.");
                return false;
            }

            _products.Remove(productId);
            Console.WriteLine($"[Success] Product ID '{productId}' removed from inventory.");
            return true;
        }

        // 4. DISPLAY ALL INVENTORY
        public void DisplayInventory()
        {
            Console.WriteLine("\n--- Current Warehouse Inventory Stock ---");
            if (_products.Count == 0)
            {
                Console.WriteLine("The warehouse is currently empty.");
                return;
            }

            foreach (var product in _products.Values)
            {
                Console.WriteLine(product);
            }
            Console.WriteLine(new string('-', 50));
        }
    }

    // --- TEST DRIVER ---
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Initializing Warehouse Management System ---\n");
            Inventory warehouse = new Inventory();

            // Test Adding Products
            warehouse.AddProduct(new Product("P101", "Laptop", 50, 999.99));
            warehouse.AddProduct(new Product("P102", "Smartphone", 120, 599.49));
            warehouse.AddProduct(new Product("P103", "Wireless Mouse", 300, 24.99));
            
            // Try adding a duplicate ID
            warehouse.AddProduct(new Product("P101", "Tablet", 10, 350.00));
            
            warehouse.DisplayInventory();

            // Test Updating a Product
            Console.WriteLine("\nAction: Updating stock for Smartphone due to a new shipment...");
            warehouse.UpdateProduct("P102", 145, 579.99);

            // Test Deleting a Product
            Console.WriteLine("\nAction: Discontinuing Wireless Mouse...");
            warehouse.DeleteProduct("P103");

            // Display Final State
            warehouse.DisplayInventory();
        }
    }
}