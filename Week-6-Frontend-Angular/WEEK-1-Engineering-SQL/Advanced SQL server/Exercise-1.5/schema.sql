-- ====================================================================
-- EXERCISE 5 SCHEMA: ADDITIONAL SALES ORDER DATA
-- ====================================================================

-- Adding more orders to TechCorp (customer_id: 1) so they clear the >3 threshold
INSERT INTO orders (order_id, customer_id, order_date) VALUES 
(104, 1, '2026-01-18'), 
(105, 1, '2026-01-19'), 
(106, 1, '2026-01-20');

-- Adding more order details to match the new orders
INSERT INTO order_details (order_detail_id, order_id, product_id, quantity) VALUES 
(5, 104, 3, 1),
(6, 105, 4, 2),
(7, 106, 1, 1);