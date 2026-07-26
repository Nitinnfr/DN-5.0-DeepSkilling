using System;

namespace EmployeeManagementSystem
{
    // --- STEP 2: DEFINE EMPLOYEE CLASS ---
    public class Employee
    {
        public string EmployeeId { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public double Salary { get; set; }

        public Employee(string employeeId, string name, string position, double salary)
        {
            EmployeeId = employeeId;
            Name = name;
            Position = position;
            Salary = salary;
        }

        public override string ToString()
        {
            return $"ID: {EmployeeId,-6} | Name: {Name,-15} | Position: {Position,-15} | Salary: ${Salary:N2}";
        }
    }

    // --- STEP 3: IMPLEMENT ARRAY MANAGEMENT OPERATIONS ---
    public class EmployeeManager
    {
        private readonly Employee[] _employees;
        private int _currentCount; // Tracks the actual number of active records

        public EmployeeManager(int capacity)
        {
            // Initializing a fixed-size array
            _employees = new Employee[capacity];
            _currentCount = 0;
        }

        // 1. ADD EMPLOYEE
        public bool AddEmployee(Employee emp)
        {
            if (_currentCount >= _employees.Length)
            {
                Console.WriteLine("[Error] Cannot add employee. The system array capacity is completely full!");
                return false;
            }

            _employees[_currentCount] = emp;
            _currentCount++;
            Console.WriteLine($"[Success] Added employee: {emp.Name}");
            return true;
        }

        // 2. SEARCH EMPLOYEE (By ID)
        public int SearchEmployeeById(string employeeId)
        {
            for (int i = 0; i < _currentCount; i++)
            {
                if (_employees[i].EmployeeId.Equals(employeeId, StringComparison.OrdinalIgnoreCase))
                {
                    return i; // Found! Return the index position in memory
                }
            }
            return -1; // Not found
        }

        // 3. TRAVERSE & DISPLAY ALL RECORDS
        public void TraverseEmployees()
        {
            Console.WriteLine("\n================ ACTIVE EMPLOYEE REGISTER ================");
            if (_currentCount == 0)
            {
                Console.WriteLine(" No active employee records found.");
                Console.WriteLine("==========================================================\n");
                return;
            }

            for (int i = 0; i < _currentCount; i++)
            {
                Console.WriteLine($" [{i}] {_employees[i]}");
            }
            Console.WriteLine("==========================================================\n");
        }

        // 4. DELETE EMPLOYEE
        public bool DeleteEmployee(string employeeId)
        {
            int indexToDelete = SearchEmployeeById(employeeId);

            if (indexToDelete == -1)
            {
                Console.WriteLine($"[Error] Delete failed. Employee ID '{employeeId}' not found.");
                return false;
            }

            string savedName = _employees[indexToDelete].Name;

            // Shift all subsequent elements left to close the gap in contiguous memory
            for (int i = indexToDelete; i < _currentCount - 1; i++)
            {
                _employees[i] = _employees[i + 1];
            }

            // Clear the last duplicated reference and decrement counter
            _employees[_currentCount - 1] = null;
            _currentCount--;

            Console.WriteLine($"[Success] Deleted '{savedName}' and re-aligned memory bounds.");
            return true;
        }
    }

    // --- TEST DRIVER ---
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Initializing Corporate HR Core Matrix ---\n");

            // Instantiating a manager with a fixed maximum capacity of 5 slots
            EmployeeManager hrSystem = new EmployeeManager(5);

            // Populate system records
            hrSystem.AddEmployee(new Employee("E001", "Alice Vance", "CTO", 165000));
            hrSystem.AddEmployee(new Employee("E002", "Bob Miller", "DevOps Lead", 115000));
            hrSystem.AddEmployee(new Employee("E003", "Charlie Day", "QA Analyst", 78000));

            // View initial records
            hrSystem.TraverseEmployees();

            // Search validation
            Console.WriteLine("Action: Searching for Employee ID 'E002'...");
            int foundIndex = hrSystem.SearchEmployeeById("E002");
            Console.WriteLine($" -> Query Result: Found at internal index array slot [{foundIndex}]\n");

            // Delete validation (Triggers memory shifting shifts)
            Console.WriteLine("Action: Terminating record 'E002'...");
            hrSystem.DeleteEmployee("E002");

            // Re-verify traversal to see shifted elements
            hrSystem.TraverseEmployees();
        }
    }
}