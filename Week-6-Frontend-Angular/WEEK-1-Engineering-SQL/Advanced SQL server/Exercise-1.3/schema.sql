-- ====================================================================
-- EXERCISE 3 SCHEMA: STAGING SETUP
-- ====================================================================

-- Drop table if it already exists to allow for clean resets
DROP TABLE IF EXISTS staging_products;

-- Create the staging area table structure
CREATE TABLE staging_products (
    product_id INT PRIMARY KEY,
    product_name VARCHAR(100),
    category VARCHAR(50),
    price DECIMAL(10, 2)
);

-- Insert operational staging data (Mixture of price changes and new items)
INSERT INTO staging_products (product_id, product_name, category, price) VALUES
(1, 'Pro Book 15', 'Electronics', 1150.00),    -- Price Drop (Was 1200.00)
(5, 'Budget Tablet', 'Electronics', 550.00),   -- Price Increase (Was 500.00)
(11, 'Wireless Mouse', 'Electronics', 45.00),  -- BRAND NEW PRODUCT
(12, 'Desk Lamp', 'Furniture', 60.00);         -- BRAND NEW PRODUCT