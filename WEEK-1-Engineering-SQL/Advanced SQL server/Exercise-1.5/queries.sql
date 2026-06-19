-- ====================================================================
-- EXERCISE 5: SIMPLIFYING WITH COMMON TABLE EXPRESSIONS (CTEs)
-- ====================================================================

-- Step 1: Define the isolated counting mechanism layer
WITH CustomerOrderCounts AS (
    SELECT
        customer_id,
        COUNT(order_id) AS OrderCount
    FROM orders
    GROUP BY customer_id
)
-- Step 2: Query the CTE layer and join user profile details
SELECT
    c.customer_id,
    c.customer_name,
    coc.OrderCount
FROM CustomerOrderCounts coc
JOIN customers c ON c.customer_id = coc.customer_id
WHERE coc.OrderCount > 3;