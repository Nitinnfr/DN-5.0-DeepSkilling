-- ====================================================================
-- DATABASE SCHEMA SETUP (Run this first to create tables and data)
-- ====================================================================

-- Drop the table if it already exists so we can start fresh

DROP TABLE IF EXISTS products;

-- Create the products table structure
CREATE TABLE products (
    product_id INT PRIMARY KEY,
    product_name VARCHAR(100),
    category VARCHAR(50),
    price DECIMAL(10, 2)
);

-- Insert dummy data containing precise price overlaps (ties)
INSERT INTO products (product_id, product_name, category, price) VALUES
(1, 'Pro Book 15', 'Electronics', 1200.00),
(2, 'Ultra Book 14', 'Electronics', 1200.00), -- Tie for 1st
(3, 'Gaming Rig X', 'Electronics', 1500.00),  -- Absolute highest
(4, 'Slim Phone', 'Electronics', 800.00),
(5, 'Budget Tablet', 'Electronics', 500.00),
(6, 'Leather Couch', 'Furniture', 2500.00),
(7, 'Ergonomic Chair', 'Furniture', 450.00),
(8, 'Standing Desk', 'Furniture', 850.00),
(9, 'Oak Dining Table', 'Furniture', 850.00), -- Tie for 2nd
(10, 'Bedside Nightstand', 'Furniture', 150.00);