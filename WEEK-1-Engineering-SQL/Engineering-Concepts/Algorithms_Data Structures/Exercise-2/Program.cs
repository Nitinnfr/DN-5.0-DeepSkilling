using System;
using System.Collections.Generic;

namespace ECommerceSearch
{
    // --- STEP 2: DEFINE PRODUCT CLASS ---
    public class Product : IComparable<Product>
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }

        public Product(string productId, string productName, string category)
        {
            ProductId = productId;
            ProductName = productName;
            Category = category;
        }

        // Necessary for Binary Search sorting mechanics
        public int CompareTo(Product other)
        {
            if (other == null) return 1;
            // Sorting based alphabetically on ProductName
            return string.Compare(this.ProductName, other.ProductName, StringComparison.OrdinalIgnoreCase);
        }

        public override string ToString()
        {
            return $"[{ProductId}] {ProductName} ({Category})";
        }
    }

    // --- STEP 3: IMPLEMENT SEARCH ALGORITHMS ---
    public class SearchEngine
    {
        // 1. LINEAR SEARCH
        // Traverses the collection one by one from start to finish
        public static int LinearSearch(Product[] catalog, string targetName)
        {
            for (int i = 0; i < catalog.Length; i++)
            {
                if (string.Equals(catalog[i].ProductName, targetName, StringComparison.OrdinalIgnoreCase))
                {
                    return i; // Item found, return index position
                }
            }
            return -1; // Item not found
        }

        // 2. BINARY SEARCH
        // Divide-and-conquer approach. Requires a pre-sorted array!
        public static int BinarySearch(Product[] sortedCatalog, string targetName)
        {
            int low = 0;
            int high = sortedCatalog.Length - 1;

            while (low <= high)
            {
                int mid = low + (high - low) / 2; // Prevents potential integer overflow
                int comparison = string.Compare(sortedCatalog[mid].ProductName, targetName, StringComparison.OrdinalIgnoreCase);

                if (comparison == 0)
                {
                    return mid; // Item found!
                }
                else if (comparison < 0)
                {
                    low = mid + 1; // Target is in the upper half
                }
                else
                {
                    high = mid - 1; // Target is in the lower half
                }
            }
            return -1; // Item not found
        }
    }

    // --- TEST CLIENT ---
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Building E-Commerce Catalog Systems ---\n");

            // Setup mock platform catalog array
            Product[] catalog = new Product[]
            {
                new Product("E10", "Wireless Headphones", "Electronics"),
                new Product("B25", "Leather Boots", "Footwear"),
                new Product("A01", "Coffee Maker", "Appliances"),
                new Product("D42", "Gaming Keyboard", "Electronics"),
                new Product("C11", "Denim Jacket", "Apparel")
            };

            string searchTarget = "Gaming Keyboard";

            // --- 1. Linear Search Test ---
            Console.WriteLine($"[Linear Search] Querying for '{searchTarget}'...");
            int linearResult = SearchEngine.LinearSearch(catalog, searchTarget);
            DisplayResult(linearResult, catalog);


            // --- 2. Binary Search Test ---
            // CRITICAL STEP: Binary search breaks completely if the data is not sorted first.
            Console.WriteLine("\nSorting catalog alphabetically for Binary Search...");
            Array.Sort(catalog); 
            
            Console.WriteLine("\nSorted Catalog State:");
            foreach (var prod in catalog) Console.WriteLine($" - {prod.ProductName}");

            Console.WriteLine($"\n[Binary Search] Querying for '{searchTarget}'...");
            int binaryResult = SearchEngine.BinarySearch(catalog, searchTarget);
            DisplayResult(binaryResult, catalog);
        }

        private static void DisplayResult(int index, Product[] catalog)
        {
            if (index != -1)
            {
                Console.WriteLine($" -> Match Found! Item details: {catalog[index]} at Index [{index}]");
            }
            else
            {
                Console.WriteLine(" -> Product not found in catalog listing.");
            }
        }
    }
}