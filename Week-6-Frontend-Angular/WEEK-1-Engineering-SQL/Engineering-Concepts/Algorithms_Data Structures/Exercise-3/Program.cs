using System;
using System.Collections.Generic;

namespace OrderSortingSystem
{
    // --- STEP 2: DEFINE ORDER CLASS ---
    public class Order
    {
        public string OrderId { get; set; }
        public string CustomerName { get; set; }
        public double TotalPrice { get; set; }

        public Order(string orderId, string customerName, double totalPrice)
        {
            OrderId = orderId;
            CustomerName = customerName;
            TotalPrice = totalPrice;
        }

        public override string ToString()
        {
            return $"ID: {OrderId,-6} | Customer: {CustomerName,-12} | Total: ${TotalPrice:F2}";
        }
    }

    // --- STEP 3: IMPLEMENT SORTING ALGORITHMS ---
    public class SortingEngine
    {
        // 1. BUBBLE SORT
        public static void BubbleSort(Order[] orders)
        {
            int n = orders.Length;
            for (int i = 0; i < n - 1; i++)
            {
                // Track if a swap happened to optimize slightly for already-sorted arrays
                bool swapped = false; 
                
                for (int j = 0; j < n - i - 1; j++)
                {
                    // Sort in descending order to prioritize high-value orders
                    if (orders[j].TotalPrice < orders[j + 1].TotalPrice)
                    {
                        // Swap elements
                        Order temp = orders[j];
                        orders[j] = orders[j + 1];
                        orders[j + 1] = temp;
                        swapped = true;
                    }
                }
                // If no elements were swapped in the inner loop, the array is already sorted
                if (!swapped) break;
            }
        }

        // 2. QUICK SORT (Entry point)
        public static void QuickSort(Order[] orders, int low, int high)
        {
            if (low < high)
            {
                // Partitioning index
                int pivotIndex = Partition(orders, low, high);

                // Recursively sort elements before and after partition
                QuickSort(orders, low, pivotIndex - 1);
                QuickSort(orders, pivotIndex + 1, high);
            }
        }

        private static int Partition(Order[] orders, int low, int high)
        {
            // Selecting the last element's price as the pivot value
            double pivot = orders[high].TotalPrice;
            int i = (low - 1); 

            for (int j = low; j < high; j++)
            {
                // Sort descending: change to '>' to prioritize higher values early
                if (orders[j].TotalPrice > pivot)
                {
                    i++;
                    // Swap orders[i] and orders[j]
                    Order temp1 = orders[i];
                    orders[i] = orders[j];
                    orders[j] = temp1;
                }
            }

            // Swap orders[i+1] and orders[high] (or pivot)
            Order temp2 = orders[i + 1];
            orders[i + 1] = orders[high];
            orders[high] = temp2;

            return i + 1;
        }
    }

    // --- TEST DRIVER ---
    class Program
    {
        static void Main(string[] args)
        {
            // Setup an unsorted list of e-commerce customer orders
            Order[] ordersForBubbleSort = GetMockOrders();
            Order[] ordersForQuickSort = GetMockOrders();

            Console.WriteLine("=== Original Unsorted Orders ===");
            PrintOrders(ordersForBubbleSort);

            // --- Test Bubble Sort ---
            Console.WriteLine("\nRunning Bubble Sort (Descending)...");
            SortingEngine.BubbleSort(ordersForBubbleSort);
            PrintOrders(ordersForBubbleSort);

            // --- Test Quick Sort ---
            Console.WriteLine("\nRunning Quick Sort (Descending)...");
            SortingEngine.QuickSort(ordersForQuickSort, 0, ordersForQuickSort.Length - 1);
            PrintOrders(ordersForQuickSort);
        }

        private static Order[] GetMockOrders()
        {
            return new Order[]
            {
                new Order("O101", "Alice", 250.75),
                new Order("O102", "Bob", 45.00),
                new Order("O103", "Charlie", 1200.50),
                new Order("O104", "David", 89.99),
                new Order("O105", "Emma", 450.00)
            };
        }

        private static void PrintOrders(Order[] orders)
        {
            foreach (var order in orders)
            {
                Console.WriteLine($" {order}");
            }
        }
    }
}