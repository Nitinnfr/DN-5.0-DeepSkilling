using System.ComponentModel.DataAnnotations;

namespace RetailInventory
{
    // Lab 2 & Lab 11: Category Entity
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        // Navigation property
        public virtual List<Product> Products { get; set; } = new();
    }

    // Lab 2, 8, 11, 15: Product Entity
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        
        // Lab 8: New column tracking inventory
        public int StockQuantity { get; set; }

        // Navigation properties
        public virtual Category? Category { get; set; }
        
        // Lab 11: One-to-One
        public virtual ProductDetail? ProductDetail { get; set; }
        
        // Lab 11: Many-to-Many
        public virtual List<Tag> Tags { get; set; } = new();

        // Lab 15: Concurrency row check tokens
        [Timestamp]
        public byte[]? RowVersion { get; set; }
    }

    // Lab 11: One-to-One Related Model
    public class ProductDetail
    {
        public int ProductDetailId { get; set; }
        public string WarrantyInfo { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }
    }

    // Lab 11: Many-to-Many Related Model
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public virtual List<Product> Products { get; set; } = new();
    }

    // Lab 7 & Lab 12: Data Transfer Object
    public class ProductDTO
    {
        public string Name { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}