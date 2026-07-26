-- ====================================================================
-- ADVANCED SQL: INDEX-DRIVEN PERFORMANCE EXERCISES
-- Dialect: SQLite & SQL Server (T-SQL) Compatible
-- ====================================================================

-- --------------------------------------------------------------------
-- SYSTEM RESET & TARGET SCHEMA INITIALIZATION
-- --------------------------------------------------------------------
DROP TABLE IF EXISTS OptimizationEmployees;
DROP TABLE IF EXISTS OptimizationDepartments;

CREATE TABLE OptimizationDepartments (
    DepartmentID INT PRIMARY KEY, -- Automatically generates a Clustered Index baseline
    DepartmentName VARCHAR(100) NOT NULL
);

CREATE TABLE OptimizationEmployees (
    EmployeeID INT PRIMARY KEY,
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    Email VARCHAR(100),
    Salary DECIMAL(10,2),
    DepartmentID INT,
    JoinDate DATE,
    FOREIGN KEY (DepartmentID) REFERENCES OptimizationDepartments(DepartmentID)
);

-- --- SEED MASS PERFORMANCE DATASET ---
INSERT INTO OptimizationDepartments (DepartmentID, DepartmentName) VALUES 
(1, 'HR'), (2, 'IT'), (3, 'Finance'), (4, 'Marketing');

INSERT INTO OptimizationEmployees (EmployeeID, FirstName, LastName, Email, Salary, DepartmentID, JoinDate) VALUES
(1, 'John', 'Doe', 'john.doe@corp.com', 5000.00, 1, '2022-01-15'),
(2, 'Jane', 'Smith', 'jane.smith@corp.com', 8500.00, 2, '2021-03-22'),
(3, 'Michael', 'Johnson', 'mike.j@corp.com', 7000.00, 2, '2020-07-30'),
(4, 'Emily', 'Davis', 'emily.d@corp.com', 5500.00, 3, '2019-11-05'),
(5, 'David', 'Miller', 'david.m@corp.com', 9200.00, 2, '2023-05-12'),
(6, 'Sarah', 'Wilson', 'sarah.w@corp.com', 4800.00, 4, '2024-02-18');


-- ====================================================================
-- HANDS-ON TASKS: EXECUTING QUERY OPTIMIZATIONS
-- ====================================================================

-- --- TASK 1: NON-CLUSTERED INDEX (Single Column Optimization) ---
-- Scenario: The application frequently runs text queries looking up employees by their email address.
-- Execution Before Indexing: Full Table Scan (Slow)
SELECT * FROM OptimizationEmployees WHERE Email = 'david.m@corp.com';

-- Creation Action:
CREATE INDEX idx_emp_email ON OptimizationEmployees(Email);

-- Execution After Indexing: Index Seek (Instantaneous)
SELECT * FROM OptimizationEmployees WHERE Email = 'david.m@corp.com';


-- --- TASK 2: COMPOSITE INDEX (Multi-Column Optimization) ---
-- Scenario: Executive dashboard widgets frequently pull records matching specific department scopes AND filter by high-salary brackets.
-- Execution Before Indexing: Scan/Filter operation on individual parts
SELECT * FROM OptimizationEmployees WHERE DepartmentID = 2 AND Salary > 6000.00;

-- Creation Action:
CREATE INDEX idx_emp_dept_salary ON OptimizationEmployees(DepartmentID, Salary);

-- Execution After Indexing: High-efficiency compound tree traversal lookup
SELECT * FROM OptimizationEmployees WHERE DepartmentID = 2 AND Salary > 6000.00;