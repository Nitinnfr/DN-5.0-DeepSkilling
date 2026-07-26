-- ====================================================================
-- MASTER SQL SYSTEM LAB: USER-DEFINED FUNCTIONS (UDFs)
-- Dialect: Microsoft SQL Server (T-SQL)
-- ====================================================================

-- --------------------------------------------------------------------
-- SYSTEM RESET & CORE DATA ENGINE SCHEMA SETUP
-- --------------------------------------------------------------------
IF OBJECT_ID('dbo.Employees', 'U') IS NOT NULL DROP TABLE dbo.Employees;
IF OBJECT_ID('dbo.Departments', 'U') IS NOT NULL DROP TABLE dbo.Departments;

CREATE TABLE Departments (
    DepartmentID INT PRIMARY KEY,
    DepartmentName VARCHAR(100)
);

CREATE TABLE Employees (
    EmployeeID INT PRIMARY KEY,
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    DepartmentID INT,
    Salary DECIMAL(10,2),
    JoinDate DATE,
    FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID)
);

-- --- SEED TRANSACTIONS DATASET ---
INSERT INTO Departments (DepartmentID, DepartmentName) VALUES
(1, 'HR'), (2, 'IT'), (3, 'Finance');

INSERT INTO Employees (EmployeeID, FirstName, LastName, DepartmentID, Salary, JoinDate) VALUES
(1, 'John', 'Doe', 1, 5000.00, '2020-01-15'),
(2, 'Jane', 'Smith', 2, 6000.00, '2019-03-22'),
(3, 'Bob', 'Johnson', 3, 5500.00, '2021-07-01');
GO


-- ====================================================================
-- Exercise 1 & 6: Create & Execute a Scalar Function
-- Goal: Calculate standard annual baseline salaries horizontally
-- ====================================================================
CREATE FUNCTION fn_CalculateAnnualSalary (
    @Salary DECIMAL(10,2)
)
RETURNS DECIMAL(10,2)
AS
BEGIN
    RETURN (@Salary * 12);
END;
GO

-- --- Verification & Execution (Exercise 6) ---
SELECT 
    EmployeeID, 
    FirstName, 
    Salary AS MonthlySalary,
    dbo.fn_CalculateAnnualSalary(Salary) AS AnnualSalary
FROM Employees;
GO


-- ====================================================================
-- Exercise 2 & 8: Table-Valued Function (TVF) Manipulation
-- Goal: Filter and yield an entire nested virtual table dynamically
-- ====================================================================
CREATE FUNCTION fn_GetEmployeesByDepartment (
    @DeptID INT
)
RETURNS TABLE
AS
RETURN (
    SELECT EmployeeID, FirstName, LastName, DepartmentID, Salary 
    FROM Employees
    WHERE DepartmentID = @DeptID
);
GO

-- --- Verification Test (Exercise 2 - IT Department [ID: 2]) ---
SELECT * FROM dbo.fn_GetEmployeesByDepartment(2);

-- --- Verification Test (Exercise 8 - Finance Department [ID: 3]) ---
SELECT * FROM dbo.fn_GetEmployeesByDepartment(3);
GO


-- ====================================================================
-- Exercise 3: User-Defined Functional Bonus Logic (10% Tier)
-- Goal: Isolate custom financial baseline percentages
-- ====================================================================
CREATE FUNCTION fn_CalculateBonus (
    @Salary DECIMAL(10,2)
)
RETURNS DECIMAL(10,2)
AS
BEGIN
    RETURN (@Salary * 0.10);
END;
GO

-- --- Verification Test ---
SELECT EmployeeID, FirstName, Salary, dbo.fn_CalculateBonus(Salary) AS BonusAmount FROM Employees;
GO


-- ====================================================================
-- Exercise 4: Altering/Modifying functions (Upgrade to 15% Tier)
-- Goal: Re-engineer existing internal algebraic parameters safely
-- ====================================================================
ALTER FUNCTION fn_CalculateBonus (
    @Salary DECIMAL(10,2)
)
RETURNS DECIMAL(10,2)
AS
BEGIN
    RETURN (@Salary * 0.15);
END;
GO

-- --- Verification Test ---
SELECT EmployeeID, FirstName, Salary, dbo.fn_CalculateBonus(Salary) AS UpgradedBonus FROM Employees;
GO


-- ====================================================================
-- Exercise 7: Querying Scalar Data Blocks targets
-- Goal: Run single row calculations for individual entity matches
-- ====================================================================
SELECT dbo.fn_CalculateAnnualSalary(Salary) AS SingleEmployeeAnnualSalary 
FROM Employees 
WHERE EmployeeID = 1;
GO


-- ====================================================================
-- Exercise 9: Deeply Nesting Multiple User-Defined Functions
-- Goal: Nest scalar operations inside a parent calculator module
-- ====================================================================
CREATE FUNCTION fn_CalculateTotalCompensation (
    @Salary DECIMAL(10,2)
)
RETURNS DECIMAL(10,2)
AS
BEGIN
    RETURN (dbo.fn_CalculateAnnualSalary(@Salary) + dbo.fn_CalculateBonus(@Salary));
END;
GO

-- --- Verification Test ---
SELECT 
    EmployeeID, 
    FirstName, 
    Salary,
    dbo.fn_CalculateTotalCompensation(Salary) AS TotalCompensation
FROM Employees;
GO


-- ====================================================================
-- Exercise 10: Modifying Nested Framework Systems
-- Goal: Force updates down across compound framework structures
-- ====================================================================
ALTER FUNCTION fn_CalculateTotalCompensation (
    @Salary DECIMAL(10,2)
)
RETURNS DECIMAL(10,2)
AS
BEGIN
    -- Automatically inherits the updated 15% bonus multiplier from Exercise 4
    RETURN (dbo.fn_CalculateAnnualSalary(@Salary) + dbo.fn_CalculateBonus(@Salary));
END;
GO

-- --- Final Verification Test ---
SELECT 
    EmployeeID, 
    FirstName, 
    dbo.fn_CalculateAnnualSalary(Salary) AS BaseAnnual,
    dbo.fn_CalculateBonus(Salary) AS CurrentBonus,
    dbo.fn_CalculateTotalCompensation(Salary) AS FinalTakeHomePay
FROM Employees;
GO


-- ====================================================================
-- Exercise 5: Dropping and Cleaning System Functions
-- Goal: Deprecate structures securely from structural schema trees
-- ====================================================================
-- (Uncomment lines below if you explicitly need to erase it at the end)
-- DROP FUNCTION dbo.fn_CalculateBonus;