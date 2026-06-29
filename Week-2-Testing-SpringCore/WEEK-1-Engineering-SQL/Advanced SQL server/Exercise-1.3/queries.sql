-- ====================================================================
-- FULL CONSOLIDATED PRODUCTION QUERY ANALYSIS SUITE
-- Includes: Exercise 1 (Windows), Exercise 2 (Cubes), Exercise 3 (CTEs/Upserts)
-- ====================================================================

-- ====================================================================
-- EXERCISE 1: WINDOW FUNCTIONS (RANKING ANALYSIS)
-- ====================================================================
-- Identifies price-point rankings partitioned by individual product categories
SELECT 
    product_id,
    product_name,
    category,
    price,
    ROW_NUMBER() OVER (PARTITION BY category ORDER BY price DESC) AS Row_Num,
    RANK()       OVER (PARTITION BY category ORDER BY price DESC) AS Rank_Val,
    DENSE_RANK() OVER (PARTITION BY category ORDER BY price DESC) AS Dense_Rank_Val
FROM products;


-- ====================================================================
-- EXERCISE 2: ADVANCED MULTI-DIMENSIONAL AGGREGATIONS
-- ====================================================================
-- Calculates multi-level subtotals and grand totals across sales dimensions

-- 1. Deepest Level: Subtotals by Region AND Category
SELECT 
    c.region,
    p.category,
    SUM(od.quantity) AS TotalQuantitySold
FROM orders o
JOIN order_details od ON o.order_id = od.order_id
JOIN customers c     ON o.customer_id = c.customer_id
JOIN products p      ON od.product_id = p.product_id
GROUP BY c.region, p.category

UNION ALL

-- 2. Mid Level: Totals by Region only (Category falls back to NULL)
SELECT 
    c.region,
    NULL AS category,
    SUM(od.quantity) AS TotalQuantitySold
FROM orders o
JOIN order_details od ON o.order_id = od.order_id
JOIN customers c     ON o.customer_id = c.customer_id
GROUP BY c.region

UNION ALL

-- 3. Mid Level: Totals by Category only (Region falls back to NULL)
SELECT 
    NULL AS region,
    p.category,
    SUM(od.quantity) AS TotalQuantitySold
FROM orders o
JOIN order_details od ON o.order_id = od.order_id
JOIN products p      ON od.product_id = p.product_id
GROUP BY p.category

UNION ALL

-- 4. High Level: Absolute Company-Wide Grand Total (Both fields are NULL)
SELECT 
    NULL AS region,
    NULL AS category,
    SUM(od.quantity) AS TotalQuantitySold
FROM order_details od;


-- ====================================================================
-- EXERCISE 3: PART A - RECURSIVE CTE (CALENDAR GENERATOR)
-- ====================================================================
-- Synthesizes 31 continuous date dimensions dynamically in memory
WITH RECURSIVE calendar(calendar_date) AS (
    -- Anchor Member: Starting date line
    SELECT '2025-01-01'
    
    UNION ALL
    
    -- Recursive Member: Loop modifier (+1 day step function)
    SELECT date(calendar_date, '+1 day')
    FROM calendar
    -- Termination condition bounds
    WHERE calendar_date < '2025-01-31'
)
SELECT calendar_date FROM calendar;


-- ====================================================================
-- EXERCISE 3: PART B - THE UPSERT PATTERN (STAGE SYNCHRONIZATION)
-- ====================================================================
-- Updates existing target row pricing matrices or appends novel line items

INSERT INTO products (product_id, product_name, category, price)
SELECT product_id, product_name, category, price 
FROM staging_products
ON CONFLICT(product_id) DO UPDATE SET
    product_name = excluded.product_name,
    category = excluded.category,
    price = excluded.price;

-- --- VERIFICATION VERDICT ---
-- Evaluates execution updates and final row insertions (IDs 11 & 12)
SELECT * FROM products;