-- ====================================================================
-- ADVANCED SQL: COMPREHENSIVE STORED PROCEDURES & SCALAR FUNCTIONS
-- Dialect: Microsoft SQL Server (T-SQL)
-- ====================================================================

-- --------------------------------------------------------------------
-- SYSTEM RESET & CORE SCHEMA CONFIGURATION
-- --------------------------------------------------------------------
IF OBJECT_ID('dbo.Employees', 'U') IS NOT NULL DROP TABLE dbo.Employees;
IF OBJECT_ID('dbo.Departments', 'U') IS NOT NULL DROP TABLE dbo.Departments;

CREATE TABLE Departments (
    DepartmentID INT PRIMARY KEY,
    DepartmentName VARCHAR(100) NOT NULL
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

-- --- SEED DATA ---
INSERT INTO Departments (DepartmentID, DepartmentName) VALUES (1, 'HR'), (2, 'IT');
INSERT INTO Employees (EmployeeID, FirstName, LastName, DepartmentID, Salary, JoinDate) VALUES
(1, 'John', 'Doe', 1, 5000.00, '2022-01-15'),
(2, 'Jane', 'Smith', 2, 6000.00, '2021-03-22');
GO


-- ====================================================================
-- Block 4 - Exercise 1: Create a Stored Procedure
-- Goal: Retrieve complete employee rosters filtered by DepartmentID
-- ====================================================================
CREATE PROCEDURE sp_GetEmployeesByDepartment
    @DepartmentID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        EmployeeID, 
        FirstName, 
        LastName, 
        Salary, 
        JoinDate
    FROM Employees
    WHERE DepartmentID = @DepartmentID;
END;
GO

-- --- Verification Execution ---
-- EXEC sp_GetEmployeesByDepartment @DepartmentID = 2;
GO


-- ====================================================================
-- Block 5 - Exercise 7: Return Data from a Scalar Function
-- Goal: Calculate annual compensation based on a single employee ID
-- ====================================================================
CREATE FUNCTION fn_CalculateAnnualSalaryById (
    @EmpID INT
)
RETURNS DECIMAL(10,2)
AS
BEGIN
    DECLARE @AnnualSalary DECIMAL(10,2);

    -- Compute the target record calculation directly into our local variable
    SELECT @AnnualSalary = Salary * 12 
    FROM Employees 
    WHERE EmployeeID = @EmpID;

    RETURN ISNULL(@AnnualSalary, 0);
END;
GO

-- --- Verification Execution ---
-- SELECT dbo.fn_CalculateAnnualSalaryById(1) AS Employee1AnnualSalary;
GO