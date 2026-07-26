-- ====================================================================
-- EXERCISE 1: RANKING AND WINDOW FUNCTIONS
-- SCENARIO: Find the top 3 most expensive products per category
-- ====================================================================

WITH RankedProducts AS (
    SELECT 
        product_id,
        product_name,
        category,
        price,
        
        -- 1. Unique row numbers assigned sequentially
        ROW_NUMBER() OVER (
            PARTITION BY category 
            ORDER BY price DESC
        ) AS RowNum,
        
        -- 2. Rank with gaps skipped on duplicate prices
        RANK() OVER (
            PARTITION BY category 
            ORDER BY price DESC
        ) AS StandardRank,
        
        -- 3. Dense rank with contiguous sequence numbers
        DENSE_RANK() OVER (
            PARTITION BY category 
            ORDER BY price DESC
        ) AS DenseRank

    FROM products
)
SELECT 
    category,
    product_name,
    price,
    RowNum,
    StandardRank,
    DenseRank
FROM RankedProducts
-- Filtering out any items outside the top 3 spots using DenseRank 
-- (Ensures we catch all items tied for 3rd place securely)
WHERE DenseRank <= 3
ORDER BY category, DenseRank, product_name;