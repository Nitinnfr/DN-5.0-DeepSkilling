-- ====================================================================
-- ADVANCED SQL: STORED PROCEDURE EXECUTION & DATA RETRIEVAL
-- Dialect: Microsoft SQL Server (T-SQL)
-- ====================================================================

-- --------------------------------------------------------------------
-- DEPENDENCY CHECK: Ensure the base setup exists from previous labs
-- --------------------------------------------------------------------
IF OBJECT_ID('dbo.sp_GetEmployeesByDepartment', 'P') IS NULL
BEGIN
    EXEC('
    CREATE PROCEDURE sp_GetEmployeesByDepartment @DepartmentID INT AS 
    BEGIN 
        SELECT EmployeeID, FirstName, LastName, Salary FROM Employees WHERE DepartmentID = @DepartmentID; 
    END');
END;
GO


-- ====================================================================
-- Block 4 - Exercise 4: Execute a Stored Procedure
-- Goal: Call the procedure with a parameter and review the results
-- ====================================================================

-- Executing the procedure to find employees in the IT Department (ID = 2)
EXEC sp_GetEmployeesByDepartment @DepartmentID = 2;
GO


-- ====================================================================
-- Block 4 - Exercise 5: Return Data from a Stored Procedure
-- Goal: Create a procedure that returns the total count of employees 
--       in a specific department using an OUTPUT parameter.
-- ====================================================================
CREATE PROCEDURE sp_GetDepartmentEmployeeCount
    @DepartmentID INT,
    @EmployeeCount INT OUTPUT -- This parameter passes data back to the caller
AS
BEGIN
    SET NOCOUNT ON;

    SELECT @EmployeeCount = COUNT(*)
    FROM Employees
    WHERE DepartmentID = @DepartmentID;
END;
GO

-- --- Verification & Execution for Exercise 5 ---
-- To capture and print data from an output parameter, we use local variables:
DECLARE @TotalCount INT;

-- Execute the procedure, matching our variable to the OUTPUT parameter
EXEC sp_GetDepartmentEmployeeCount 
    @DepartmentID = 2, 
    @EmployeeCount = @TotalCount OUTPUT;

-- Display the returned value
SELECT @TotalCount AS [Total Employees in Department 2];
GO