using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EFCore.BulkExtensions;

namespace RetailInventory
{
    class Program
    {
        // Lab 13: Pre-compiled Query
        private static readonly Func<AppDbContext, decimal, IAsyncEnumerable<Product>> _expensiveProducts =
            EF.CompileAsyncQuery((AppDbContext ctx, decimal price) =>
                ctx.Products.Where(p => p.Price > price));

        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Retail Inventory Laboratory Execution Block ===");

            using (var context = new AppDbContext())
            {
                Console.WriteLine("Initializing testing database layout...");
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
            }

            await RunLab4_InsertingData();
            await RunLab5_RetrievingData();
            await RunLab6_UpdatingAndDeleting();
            await RunLab7_LINQQueries();
            await RunLab10_LoadingStrategies();
            await RunLab11_Relationships();
            await RunLab12_CircularReferences();
            await RunLab13_QueryOptimizations();
            await RunLab14_BulkOperations();
            await RunLab15_ConcurrencyControl();
        }

        static async Task RunLab4_InsertingData()
        {
            Console.WriteLine("\n--- Lab 4: Data Entry Tasks ---");
            using var context = new AppDbContext();

            var electronics = new Category { Name = "Electronics" };
            var groceries = new Category { Name = "Groceries" };
            await context.Categories.AddRangeAsync(electronics, groceries);

            var product1 = new Product { Name = "Laptop", Price = 75000, Category = electronics, StockQuantity = 10 };
            var product2 = new Product { Name = "Rice Bag", Price = 1200, Category = groceries, StockQuantity = 30 };
            
            await context.Products.AddRangeAsync(product1, product2);
            await context.SaveChangesAsync();
            Console.WriteLine("Context records synchronized successfully.");
        }

        static async Task RunLab5_RetrievingData()
        {
            Console.WriteLine("\n--- Lab 5: DB Query Retrieval Operations ---");
            using var context = new AppDbContext();

            var products = await context.Products.ToListAsync();
            foreach (var p in products) Console.WriteLine($"Discovered: {p.Name} costing {p.Price}");

            var product = await context.Products.FindAsync(1);
            Console.WriteLine($"Primary Key Identity Locate (ID 1): {product?.Name}");

            var expensive = await context.Products.FirstOrDefaultAsync(p => p.Price > 50000);
            Console.WriteLine($"Conditional Evaluation Result: {expensive?.Name}");
        }

        static async Task RunLab6_UpdatingAndDeleting()
        {
            Console.WriteLine("\n--- Lab 6: Mutations and Row Removals ---");
            using var context = new AppDbContext();

            var product = await context.Products.FirstOrDefaultAsync(p => p.Name == "Laptop");
            if (product != null)
            {
                product.Price = 70000;
                await context.SaveChangesAsync();
                Console.WriteLine($"Price Modified targeting Laptop: {product.Price}");
            }

            var toDelete = await context.Products.FirstOrDefaultAsync(p => p.Name == "Rice Bag");
            if (toDelete != null)
            {
                context.Products.Remove(toDelete);
                await context.SaveChangesAsync();
                Console.WriteLine("Removed target entity entry from records tracking context.");
            }
        }

        static async Task RunLab7_LINQQueries()
        {
            Console.WriteLine("\n--- Lab 7: Language Integrated Query Projections ---");
            using var context = new AppDbContext();

            var filtered = await context.Products
                .Where(p => p.Price > 1000)
                .OrderByDescending(p => p.Price)
                .ToListAsync();

            foreach (var item in filtered) Console.WriteLine($"LINQ Output -> {item.Name}: {item.Price}");
        }

        static async Task RunLab10_LoadingStrategies()
        {
            Console.WriteLine("\n--- Lab 10: Relationships Materialization Paths ---");
            using var context = new AppDbContext();

            // 1. Eager
            var eagerProducts = await context.Products.Include(p => p.Category).ToListAsync();
            Console.WriteLine($"Eager loaded collection tracking path: {eagerProducts.FirstOrDefault()?.Category?.Name}");

            // 2. Explicit
            var clearCtx = new AppDbContext();
            var singleProduct = await clearCtx.Products.FirstAsync();
            await clearCtx.Entry(singleProduct).Reference(p => p.Category).LoadAsync();
            Console.WriteLine($"Explicit database reference verification: {singleProduct.Category?.Name}");

            // 3. Lazy
            var lazyCtx = new AppDbContext();
            var lazyProduct = await lazyCtx.Products.FirstAsync();
            Console.WriteLine($"Lazy proxy resolution trace outcome: {lazyProduct.Category?.Name}");
        }

        static async Task RunLab11_Relationships()
        {
            Console.WriteLine("\n--- Lab 11: Complex Cardinality Configuration ---");
            using var context = new AppDbContext();

            var product = await context.Products.FirstAsync();
            product.ProductDetail = new ProductDetail { WarrantyInfo = "Standard Store Terms Verified" };
            product.Tags.Add(new Tag { Name = "Seasonal Promo" });

            await context.SaveChangesAsync();
            Console.WriteLine("Relationships updated successfully.");
        }

        static async Task RunLab12_CircularReferences()
        {
            Console.WriteLine("\n--- Lab 12: Object Cycles Decoupling Projection ---");
            using var context = new AppDbContext();

            var productDTOs = await context.Products
                .Select(p => new ProductDTO
                {
                    Name = p.Name,
                    CategoryName = p.Category != null ? p.Category.Name : "Unassigned Category",
                    Price = p.Price
                }).ToListAsync();

            foreach (var dto in productDTOs) Console.WriteLine($"Safe DTO Record: {dto.Name} in {dto.CategoryName}");
        }

        static async Task RunLab13_QueryOptimizations()
        {
            Console.WriteLine("\n--- Lab 13: Operational Optimization Rules ---");
            using var context = new AppDbContext();

            var trackingDisabled = await context.Products.AsNoTracking().ToListAsync();

            Console.WriteLine("Executing compiled expression loop targets:");
            await foreach (var item in _expensiveProducts(context, 20000))
            {
                Console.WriteLine($"- Cached Compilation item hit: {item.Name}");
            }
        }

        static async Task RunLab14_BulkOperations()
        {
            Console.WriteLine("\n--- Lab 14: Bulk Processing Performance ---");
            using var context = new AppDbContext();

            var bulkUpdateList = await context.Products.ToListAsync();
            foreach (var p in bulkUpdateList) p.StockQuantity += 5;

            await context.BulkUpdateAsync(bulkUpdateList);
            Console.WriteLine($"Bulk batch execution operations finalized matching target count: {bulkUpdateList.Count}");
        }

        static async Task RunLab15_ConcurrencyControl()
        {
            Console.WriteLine("\n--- Lab 15: Concurrency Token Interception ---");
            
            using var user1Context = new AppDbContext();
            using var user2Context = new AppDbContext();

            var prodUser1 = await user1Context.Products.FirstAsync();
            var prodUser2 = await user2Context.Products.FirstAsync();

            prodUser1.StockQuantity = 500;
            await user1Context.SaveChangesAsync();

            prodUser2.StockQuantity = -50;
            try
            {
                await user2Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                Console.WriteLine("Verified: Outdated context transaction modification blocked safely.");
            }
        }
    }
}