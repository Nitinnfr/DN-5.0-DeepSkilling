-- ====================================================================
-- EXERCISE 4 SCHEMA: MONTHLY TRANSACTION MATRIX
-- ====================================================================

-- Drop table if it already exists for a clean reset
DROP TABLE IF EXISTS monthly_sales_staging;

-- Create transaction table tracking sales timeline
CREATE TABLE monthly_sales_staging (
    product_name VARCHAR(100),
    sale_month VARCHAR(10), -- 'Jan', 'Feb', 'Mar'
    quantity_sold INT
);

-- Seed with data across multiple months for cross-tabulation analysis
INSERT INTO monthly_sales_staging (product_name, sale_month, quantity_sold) VALUES
('Pro Book 15', 'Jan', 10),
('Pro Book 15', 'Feb', 15),
('Pro Book 15', 'Mar', 8),
('Ultra Book 14', 'Jan', 5),
('Ultra Book 14', 'Mar', 12),  -- Note: No sales in Feb!
('Gaming Rig X', 'Feb', 20),
('Gaming Rig X', 'Mar', 22);