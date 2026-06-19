-- ====================================================================
-- LAB: EMPLOYEE MANAGEMENT SYSTEM (VIEWS & COMPUTED COLUMNS)
-- ====================================================================

-- --------------------------------------------------------------------
-- SYSTEM RESET & TABLES SETUP
-- --------------------------------------------------------------------
DROP VIEW IF EXISTS vw_EmployeeReport;
DROP VIEW IF EXISTS vw_EmployeeAnnualSalary;
DROP VIEW IF EXISTS vw_EmployeeFullName;
DROP VIEW IF EXISTS vw_EmployeeBasicInfo;
DROP TABLE IF EXISTS Employees;
DROP TABLE IF EXISTS Departments;

CREATE TABLE Departments (
    DepartmentID INT PRIMARY KEY,
    DepartmentName VARCHAR(100)
);

CREATE TABLE Employees (
    EmployeeID INT PRIMARY KEY,
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    DepartmentID INT,
    Salary DECIMAL(10, 2),
    JoinDate DATE,
    FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID)
);

-- --------------------------------------------------------------------
-- DATA SEEDING
-- --------------------------------------------------------------------
INSERT INTO Departments (DepartmentID, DepartmentName) VALUES
(1, 'Engineering'),
(2, 'Human Resources'),
(3, 'Marketing');

INSERT INTO Employees (EmployeeID, FirstName, LastName, DepartmentID, Salary, JoinDate) VALUES
(101, 'John', 'Doe', 1, 6000.00, '2025-01-10'),
(102, 'Jane', 'Smith', 1, 7500.00, '2025-03-15'),
(103, 'Michael', 'Brown', 2, 5000.00, '2026-02-01'),
(104, 'Emily', 'Davis', 3, 5500.00, '2026-05-20');


-- ====================================================================
-- Exercise 1: Create a Simple View
-- Goal: Abstract a standard JOIN operation into a reusable virtual table
-- ====================================================================

CREATE VIEW vw_EmployeeBasicInfo AS
SELECT 
    e.EmployeeID, 
    e.FirstName, 
    e.LastName, 
    d.DepartmentName
FROM Employees e
JOIN Departments d ON e.DepartmentID = d.DepartmentID;

-- --- Verify Exercise 1 ---
SELECT * FROM vw_EmployeeBasicInfo;


-- ====================================================================
-- Exercise 2: Add Computed Column - Full Name
-- Goal: Use string concatenation to cleanly format name dimensions
-- ====================================================================

CREATE VIEW vw_EmployeeFullName AS
SELECT 
    EmployeeID,
    FirstName || ' ' || LastName AS FullName,
    Salary
FROM Employees;

-- --- Verify Exercise 2 ---
SELECT * FROM vw_EmployeeFullName;


-- ====================================================================
-- Exercise 3: Add Computed Column - Annual Salary
-- Goal: Introduce automated inline financial multiplier logic
-- ====================================================================

CREATE VIEW vw_EmployeeAnnualSalary AS
SELECT 
    EmployeeID,
    FirstName,
    LastName,
    Salary,
    (Salary * 12) AS AnnualSalary
FROM Employees;

-- --- Verify Exercise 3 ---
SELECT * FROM vw_EmployeeAnnualSalary;


-- ====================================================================
-- Exercise 4: Add Multiple Computed Columns
-- Goal: Construct a unified, calculated reporting matrix
-- ====================================================================

CREATE VIEW vw_EmployeeReport AS
SELECT 
    e.EmployeeID,
    e.FirstName || ' ' || e.LastName AS FullName,
    d.DepartmentName,
    (e.Salary * 12) AS AnnualSalary,
    (e.Salary * 12) * 0.10 AS Bonus
FROM Employees e
JOIN Departments d ON e.DepartmentID = d.DepartmentID;

-- --- Verify Exercise 4 ---
SELECT * FROM vw_EmployeeReport;