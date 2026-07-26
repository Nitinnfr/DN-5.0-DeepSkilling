-- ====================================================================
-- MASTER SQL SYSTEM LAB: TRANSACT-SQL (T-SQL) TRIGGERS SUITE
-- Dialect: Microsoft SQL Server (T-SQL)
-- ====================================================================

-- --------------------------------------------------------------------
-- SYSTEM RESET & CORE SCHEMA CONFIGURATION
-- --------------------------------------------------------------------
IF OBJECT_ID('dbo.EmployeeChanges', 'U') IS NOT NULL DROP TABLE dbo.EmployeeChanges;
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
    AnnualSalary DECIMAL(10,2) NULL, -- Added explicitly for Exercise 6 configuration
    FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID)
);

-- --- SEED TRANS-DIMENSIONAL DATASET ---
INSERT INTO Departments (DepartmentID, DepartmentName) VALUES
(1, 'HR'), (2, 'Finance'), (3, 'IT'), (4, 'Marketing');

INSERT INTO Employees (EmployeeID, FirstName, LastName, DepartmentID, Salary, JoinDate) VALUES
(1, 'John', 'Doe', 1, 5000.00, '2022-01-15'),
(2, 'Jane', 'Smith', 2, 6000.00, '2021-03-22'),
(3, 'Michael', 'Johnson', 3, 7000.00, '2020-07-30'),
(4, 'Emily', 'Davis', 4, 5500.00, '2019-11-05');
GO


-- ====================================================================
-- Exercise 1: Create an AFTER TRIGGER (Audit Logging)
-- Goal: Capture old vs new records when employee compensation changes
-- ====================================================================

-- 1. Create the Audit Log Table
CREATE TABLE EmployeeChanges (
    LogID INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeID INT,
    OldSalary DECIMAL(10,2),
    NewSalary DECIMAL(10,2),
    ChangedDate DATETIME DEFAULT GETDATE(),
    ChangedBy NVARCHAR(100) DEFAULT ORIGINAL_LOGIN()
);
GO

-- 2. Build the AFTER UPDATE Engine
CREATE TRIGGER trg_AfterEmployeeSalaryUpdate
ON Employees
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Check if the Salary column was actually modified
    IF UPDATE(Salary)
    BEGIN
        INSERT INTO EmployeeChanges (EmployeeID, OldSalary, NewSalary)
        SELECT 
            i.EmployeeID,
            d.Salary AS OldSalary,
            i.Salary AS NewSalary
        FROM inserted i
        JOIN deleted d ON i.EmployeeID = d.EmployeeID;
    END
END;
GO

-- --- Verification Test for Exercise 1 ---
-- UPDATE Employees SET Salary = 5300.00 WHERE EmployeeID = 1;
-- SELECT * FROM EmployeeChanges;
GO


-- ====================================================================
-- Exercise 2: Create an INSTEAD OF Trigger (Security Boundary)
-- Goal: Formally intercept and block hard deletions of employee profiles
-- ====================================================================
CREATE TRIGGER trg_InsteadOfEmployeeDelete
ON Employees
INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;
    -- Raise an explicit validation exception and stop processing
    RAISERROR ('CRITICAL ALERT: Physical deletion of employee profiles is prohibited. Deactivation aborted.', 16, 1);
END;
GO

-- --- Verification Test for Exercise 2 ---
-- DELETE FROM Employees WHERE EmployeeID = 4; -- This will trigger the custom error!
GO


-- ====================================================================
-- Exercise 3: Create a LOGON Trigger (Server Maintenance Locking)
-- Goal: Secure server instances from client logons during off-hours
-- Note: Logon triggers apply to ALL SERVER connections.
-- ====================================================================
CREATE TRIGGER trg_LogonMaintenanceRestriction
ON ALL SERVER 
FOR LOGON
AS
BEGIN
    -- Evaluate if current system hour is between 2:00 AM and 2:59 AM
    IF (DATEPART(HOUR, GETDATE()) = 2)
    BEGIN
        -- Allow system administrators (sa) to bypass maintenance lockdowns if necessary
        IF ORIGINAL_LOGIN() <> 'sa'
        BEGIN
            ROLLBACK; -- Hard terminates the incoming connection lifecycle
        END
    END
END;
GO


-- ====================================================================
-- Exercise 4 & 5: Modifying and Dropping Triggers via DDL SQL
-- Goal: Documentation on how to alter or drop triggers via scripts
-- ====================================================================

-- To Modify a trigger logic programmatically (Exercise 4 equivalent):
-- ALTER TRIGGER trg_InsteadOfEmployeeDelete ON Employees INSTEAD OF DELETE AS ...

-- To Delete/Drop a trigger from your schema completely (Exercise 5):
-- DROP TRIGGER trg_LogonMaintenanceRestriction ON ALL SERVER;
-- DROP TRIGGER trg_InsteadOfEmployeeDelete;
GO


-- ====================================================================
-- Exercise 6: Create a Trigger to Update a Computed Column
-- Goal: Calculate macro-financial attributes automatically on data changes
-- ====================================================================
CREATE TRIGGER trg_CalculateAnnualSalary
ON Employees
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Avoid infinite loops by verifying that the salary data itself changed
    IF UPDATE(Salary)
    BEGIN
        UPDATE e
        SET e.AnnualSalary = i.Salary * 12
        FROM Employees e
        JOIN inserted i ON e.EmployeeID = i.EmployeeID;
    END
END;
GO

-- --- Final Verification Test for Exercise 6 ---
-- Let's update an individual employee salary and check if AnnualSalary automatically updates
UPDATE Employees SET Salary = 8000.00 WHERE EmployeeID = 3;
SELECT EmployeeID, FirstName, Salary AS MonthlySalary, AnnualSalary FROM Employees WHERE EmployeeID = 3;
GO