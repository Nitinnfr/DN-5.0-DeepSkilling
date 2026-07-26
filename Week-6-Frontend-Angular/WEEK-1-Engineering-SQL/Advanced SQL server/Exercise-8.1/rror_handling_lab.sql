-- ====================================================================
-- MASTER SQL SYSTEM LAB: ADVANCED T-SQL ERROR HANDLING SUITE
-- Dialect: Microsoft SQL Server (T-SQL)
-- ====================================================================

-- --------------------------------------------------------------------
-- SYSTEM RESET & CORE SCHEMA CONFIGURATION
-- --------------------------------------------------------------------
IF OBJECT_ID('dbo.AuditLog', 'U') IS NOT NULL DROP TABLE dbo.AuditLog;
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
    Email VARCHAR(100) UNIQUE,
    Salary DECIMAL(10, 2),
    DepartmentID INT,
    FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID)
);

CREATE TABLE AuditLog (
    LogID INT IDENTITY(1,1) PRIMARY KEY,
    Action VARCHAR(100),
    ErrorMessage VARCHAR(4000),
    ActionDate DATETIME DEFAULT GETDATE()
);

-- --- SEED DATA ---
INSERT INTO Departments (DepartmentID, DepartmentName) VALUES (1, 'HR'), (2, 'IT');
INSERT INTO Employees (EmployeeID, FirstName, LastName, Email, Salary, DepartmentID) 
VALUES (1, 'Alice', 'Smith', 'alice@company.com', 5000.00, 1);
GO


-- ====================================================================
-- Question 1, 2, 3 & 6: Unified AddEmployee Procedure
-- Consolidates business validation, custom errors, severity tiers, 
-- logging, and THROW propagation in one production-ready engine.
-- ====================================================================
CREATE PROCEDURE AddEmployee
    @EmployeeID INT,
    @FirstName VARCHAR(50),
    @LastName VARCHAR(50),
    @Email VARCHAR(100),
    @Salary DECIMAL(10,2),
    @DepartmentID INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Question 3 & 6: Business Rules Enforcement via Custom RAISERROR
    IF @Salary <= 0
    BEGIN
        -- Severity 16 indicates an execution error that stops processing
        RAISERROR('Salary must be greater than zero.', 16, 1);
        RETURN;
    END

    IF @Salary < 1000
    BEGIN
        -- Severity 10 is an informational warning/trace message; execution continues
        RAISERROR('Warning: Salary is unusually low (< 1000). Proceeding with insert.', 10, 1);
    END

    -- Main Try block to handle systemic issues (like duplicate unique keys)
    BEGIN TRY
        INSERT INTO Employees (EmployeeID, FirstName, LastName, Email, Salary, DepartmentID)
        VALUES (@EmployeeID, @FirstName, @LastName, @Email, @Salary, @DepartmentID);
    END TRY
    BEGIN CATCH
        -- Capture error data points cleanly
        DECLARE @CapturedMessage VARCHAR(4000) = ERROR_MESSAGE();

        -- Question 1: Log the issue silently into the database AuditLog table
        INSERT INTO AuditLog (Action, ErrorMessage)
        VALUES ('AddEmployee Failure', @CapturedMessage);

        -- Question 2: Re-raise/propagate the original error back up to client applications
        THROW;
    END CATCH
END;
GO

-- --- Execution Tests for AddEmployee ---
-- EXEC AddEmployee 2, 'Bob', 'Jones', 'alice@company.com', 4000.00, 1; -- Fails: Duplicate Email (Logs & Throws)
-- EXEC AddEmployee 3, 'Charlie', 'Brown', 'charlie@co.com', -50.00, 1; -- Fails: Custom Salary Check
-- SELECT * FROM AuditLog;
GO


-- ====================================================================
-- Question 4: Nested TRY...CATCH with RAISERROR
-- Goal: Showcase multi-level defensive boundary validation routing
-- ====================================================================
CREATE PROCEDURE TransferEmployee
    @EmployeeID INT,
    @NewDepartmentID INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Outer Try Block
    BEGIN TRY
        
        -- Inner Try Block to catch isolated sub-validation sequences
        BEGIN TRY
            IF NOT EXISTS (SELECT 1 FROM Departments WHERE DepartmentID = @NewDepartmentID)
            BEGIN
                -- Route a custom missing department exception
                RAISERROR('Target DepartmentID does not exist.', 16, 1);
            END

            UPDATE Employees
            SET DepartmentID = @NewDepartmentID
            WHERE EmployeeID = @EmployeeID;
        END TRY
        BEGIN CATCH
            -- Inner Catch processes the validation failure
            INSERT INTO AuditLog (Action, ErrorMessage)
            VALUES ('TransferEmployee Validation Error', ERROR_MESSAGE());
            
            -- Re-raise to pass the error out to the outer management layer
            RAISERROR('Validation Layer Failed. Terminating Outer Scope.', 16, 1);
        END CATCH

    END TRY
    BEGIN CATCH
        -- Outer Catch logs the final execution termination summary
        INSERT INTO AuditLog (Action, ErrorMessage)
        VALUES ('TransferEmployee Global Crash', ERROR_MESSAGE());
        
        THROW;
    END CATCH
END;
GO


-- ====================================================================
-- Question 5: Logging All Errors in a Transaction
-- Goal: Ensure absolute database consistency (all or nothing)
-- ====================================================================
CREATE PROCEDURE BatchInsertEmployees
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Initialize a concrete transaction barrier
        BEGIN TRANSACTION;

        -- Transaction pass 1 (Valid)
        INSERT INTO Employees (EmployeeID, FirstName, LastName, Email, Salary, DepartmentID)
        VALUES (10, 'John', 'Developer', 'john@dev.com', 6000.00, 2);

        -- Transaction pass 2 (CRASH: Intentionally triggering duplicate key on ID 1)
        INSERT INTO Employees (EmployeeID, FirstName, LastName, Email, Salary, DepartmentID)
        VALUES (1, 'Cloned', 'User', 'clone@dev.com', 4500.00, 1);

        -- If everything runs cleanly up to this point, save changes permanently
        COMMIT TRANSACTION;
        PRINT 'Batch complete successfully.';
    END TRY
    BEGIN CATCH
        -- If any step crashes, check for open transaction states and execute a clean roll-back
        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
            PRINT 'Batch failure encountered. All transaction updates rolled back safely.';
        END

        -- Log the transaction failure payload
        INSERT INTO AuditLog (Action, ErrorMessage)
        VALUES ('BatchInsertEmployees Transaction Aborted', ERROR_MESSAGE());
    END CATCH
END;
GO

-- --- Execution Test for Transaction Batch ---
-- EXEC BatchInsertEmployees;
-- SELECT * FROM Employees; -- Notice that John (ID 10) was NEVER saved!
-- SELECT * FROM AuditLog;
GO