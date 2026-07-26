-- ====================================================================
-- FULL CONSOLIDATED DATABASE SCHEMA SETUP
-- Includes structures and sample datasets for Exercises 1 & 2
-- ====================================================================

-- --------------------------------------------------------------------
-- STEP 1: CLEANUP / RESET ENTITIES
-- (Dropped in reverse order of dependencies to avoid Foreign Key errors)
-- --------------------------------------------------------------------
DROP TABLE IF EXISTS order_details;
DROP TABLE IF EXISTS orders;
DROP TABLE IF EXISTS customers;
DROP TABLE IF EXISTS products;


-- --------------------------------------------------------------------
-- STEP 2: CREATE CORE DATA TABLES
-- --------------------------------------------------------------------

-- Table 1: Product Inventory Matrix (Exercise 1 Core)
CREATE TABLE products (
    product_id INT PRIMARY KEY,
    product_name VARCHAR(100),
    category VARCHAR(50),
    price DECIMAL(10, 2)
);

-- Table 2: Customer Segments
CREATE TABLE customers (
    customer_id INT PRIMARY KEY,
    customer_name VARCHAR(100),
    region VARCHAR(50)
);

-- Table 3: Order Header
CREATE TABLE orders (
    order_id INT PRIMARY KEY,
    customer_id INT,
    order_date DATE,
    FOREIGN KEY(customer_id) REFERENCES customers(customer_id)
);

-- Table 4: Order Line Items (Relational Bridge)
CREATE TABLE order_details (
    order_detail_id INT PRIMARY KEY,
    order_id INT,
    product_id INT,
    quantity INT,
    FOREIGN KEY(order_id) REFERENCES orders(order_id),
    FOREIGN KEY(product_id) REFERENCES products(product_id)
);


-- --------------------------------------------------------------------
-- STEP 3: SEED SYSTEM WITH TEST DATA
-- --------------------------------------------------------------------

-- Seed Products (Contains precise price duplicate ties for Exercise 1)
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

-- Seed Customers
INSERT INTO customers (customer_id, customer_name, region) VALUES 
(1, 'TechCorp', 'North'), 
(2, 'BioMed', 'South'), 
(3, 'RetailGiant', 'North');

-- Seed Orders
INSERT INTO orders (order_id, customer_id, order_date) VALUES 
(101, 1, '2026-01-15'), 
(102, 2, '2026-01-16'), 
(103, 3, '2026-01-17');

-- Seed Order Details (Transactional breakdown for Exercise 2 multi-aggregations)
INSERT INTO order_details (order_detail_id, order_id, product_id, quantity) VALUES 
(1, 101, 1, 5),   -- North region bought 5 Pro Books (Electronics)
(2, 101, 7, 10),  -- North region bought 10 Ergonomic Chairs (Furniture)
(3, 102, 2, 3),   -- South region bought 3 Ultra Books (Electronics)
(4, 103, 6, 2);   -- North region bought 2 Leather Couches (Furniture)