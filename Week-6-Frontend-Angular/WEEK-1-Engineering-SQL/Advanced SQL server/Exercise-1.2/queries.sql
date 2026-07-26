-- ====================================================================
-- EXERCISE 2: ADVANCED AGGREGATIONS (SQLite Compatible)
-- SCENARIO: Analyze sales volume across multi-dimensional groupings
-- ====================================================================

-- --- EQUIVALENT TO: GROUPING SETS / CUBE / ROLLUP ---
-- Stitches individual breakdown layers together using UNION ALL

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

-- 4. High Level: Absolute Grand Total across everything (Both are NULL)
SELECT 
    NULL AS region,
    NULL AS category,
    SUM(od.quantity) AS TotalQuantitySold
FROM order_details od;