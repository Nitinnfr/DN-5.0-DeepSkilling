-- ====================================================================
-- MASTER SQL SYSTEM LAB: TRANSACT-SQL (T-SQL) CURSORS SUITE
-- Dialect: Microsoft SQL Server (T-SQL)
-- ====================================================================

-- --------------------------------------------------------------------
-- SYSTEM RESET & CORE SCHEMA CONFIGURATION
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

-- --- SEED TRANS-DIMENSIONAL DATASET ---
INSERT INTO Departments (DepartmentID, DepartmentName) VALUES
(1, 'HR'), (2, 'IT'), (3, 'Finance');

INSERT INTO Employees (EmployeeID, FirstName, LastName, DepartmentID, Salary, JoinDate) VALUES
(1, 'John', 'Doe', 1, 5000.00, '2020-01-15'),
(2, 'Jane', 'Smith', 2, 6000.00, '2019-03-22'),
(3, 'Bob', 'Johnson', 3, 5500.00, '2021-07-30');
GO


-- ====================================================================
-- Exercise 1: Create a Cursor (Row-by-Row Lifecycle)
-- Goal: Declare, Open, Fetch Loop, Print, and Deallocate a standard cursor
-- ====================================================================

-- 1. Declare local variables to temporarily house row data fields
DECLARE @EmpID INT;
DECLARE @FName VARCHAR(50);
DECLARE @LName VARCHAR(50);
DECLARE @EmpSalary DECIMAL(10,2);

-- 2. Step 1: Declare the Cursor structure mapping
DECLARE emp_cursor CURSOR FOR
SELECT EmployeeID, FirstName, LastName, Salary
FROM Employees;

-- 3. Step 2: Open the data channel pipeline
OPEN emp_cursor;

-- 4. Step 3: Fetch the initial prime line item row
FETCH NEXT FROM emp_cursor INTO @EmpID, @FName, @LName, @EmpSalary;

-- 5. Step 4: Loop systematically through data records while matches continue
-- (@@FETCH_STATUS = 0 means the row was successfully pulled out)
WHILE @@FETCH_STATUS = 0
BEGIN
    -- Output individual row configurations using custom print strings
    PRINT 'EMPLOYEE RECORD PROCESSING -> ID: ' + CAST(@EmpID AS VARCHAR(10)) + 
          ' | Name: ' + @FName + ' ' + @LName + 
          ' | Monthly Earnings: $' + CAST(@EmpSalary AS VARCHAR(20));

    -- Pull next subsequent row frame from the target table stack
    FETCH NEXT FROM emp_cursor INTO @EmpID, @FName, @LName, @EmpSalary;
END;

-- 6. Step 5 & Cleanup: Close pipeline operations and scrub server memory structures
CLOSE emp_cursor;
DEALLOCATE emp_cursor;
GO


-- ====================================================================
-- Exercise 2: Types of Cursors
-- Goal: Document and evaluate the syntax and behaviors of major cursor types
-- ====================================================================

-- --- TYPE 1: FORWARD-ONLY ---
-- Behavior: Default cursor type. Can only move forward row-by-row (`FETCH NEXT`). 
-- Very lightweight, fast, and does not support backward traversal.
DECLARE cur_forward_only CURSOR FORWARD_ONLY FOR
SELECT EmployeeID, FirstName, LastName FROM Employees;
GO

-- --- TYPE 2: STATIC ---
-- Behavior: Copies the entire dataset out into a temp storage structure (`tempdb`). 
-- Modifications made to the raw database table by other users *will not* be visible. 
-- Supports bidirectional fetching (`FETCH PRIOR`, `FETCH FIRST`, `FETCH LAST`).
DECLARE cur_static CURSOR STATIC FOR
SELECT EmployeeID, FirstName, LastName FROM Employees;
GO

-- --- TYPE 3: DYNAMIC ---
-- Behavior: Complete opposite of Static. Detects all changes, insertions, and deletions 
-- made by concurrent user connections in real-time as you scroll through the cursor loop.
DECLARE cur_dynamic CURSOR DYNAMIC FOR
SELECT EmployeeID, FirstName, LastName FROM Employees;
GO

-- --- TYPE 4: KEYSET-DRIVEN ---
-- Behavior: A hybrid compromise. The unique primary identifiers (keys) are stored 
-- statically in `tempdb`. If a separate query updates a non-key column, the change 
-- *is* visible, but new rows added after opening the cursor remain hidden.
DECLARE cur_keyset CURSOR KEYSET FOR
SELECT EmployeeID, FirstName, LastName FROM Employees;
GO


-- ====================================================================
-- COMPARATIVE ARCHITECTURAL ANALYSIS MATRIX
-- ====================================================================
/*
| Cursor Type   | Directional Scrolling | Reflection of Real-Time Changes | Performance Cost |
|---------------|-----------------------|----------------------------------|------------------|
| FORWARD-ONLY  | Next Only             | Yes (for future fetched rows)    | Very Low         |
| STATIC        | All Directions        | No (Isolated Snapshot)           | Medium           |
| KEYSET        | All Directions        | Partially (Updates yes, Inserts no)| High           |
| DYNAMIC       | All Directions        | Full Real-Time Sync              | Very High        |
*/