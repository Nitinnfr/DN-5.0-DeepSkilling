using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RetailWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RetailWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous] // Allows testing directly via Swagger/Postman without token restrictions
    public class EmployeeController : ControllerBase
    {
        // Hardcoded simulation list tracking state across HTTP operations
        private static readonly List<Employee> _employeeList = new()
        {
            new Employee
            {
                Id = 1,
                Name = "Nitin Singh",
                Salary = 85000,
                Permanent = true,
                DateOfBirth = new DateTime(2001, 05, 15),
                Department = new Department { Id = 10, DeptName = "Engineering" },
                Skills = new List<Skill> { new Skill { Id = 1, SkillName = "C# .NET Core" } }
            },
            new Employee
            {
                Id = 2,
                Name = "Jane Smith",
                Salary = 92000,
                Permanent = true,
                DateOfBirth = new DateTime(1999, 08, 22),
                Department = new Department { Id = 20, DeptName = "IT Security" },
                Skills = new List<Skill> { new Skill { Id = 2, SkillName = "Penetration Testing" } }
            }
        };

        /// <summary>
        /// GET: api/Employee
        /// </summary>
        [HttpGet]
        public ActionResult<List<Employee>> Get()
        {
            return Ok(_employeeList);
        }

        /// <summary>
        /// PUT: api/Employee/{id}
        /// Lab Objective: Updates an Employee object based on user input validation rules.
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Employee))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Employee> UpdateEmployee(int id, [FromBody] Employee updatedData)
        {
            // 1. Check if the id value is lesser than or equal to 0
            if (id <= 0)
            {
                return BadRequest("Invalid employee id");
            }

            // 2. Find the existing record inside the hardcoded collection state
            var existingEmployee = _employeeList.FirstOrDefault(e => e.Id == id);

            // 3. If ID is greater than 0 but not present in our collection list, throw BadRequest
            if (existingEmployee == null)
            {
                return BadRequest("Invalid employee id");
            }

            // 4. Update fields using JSON data extracted via the [FromBody] attribute
            existingEmployee.Name = updatedData.Name;
            existingEmployee.Salary = updatedData.Salary;
            existingEmployee.Permanent = updatedData.Permanent;
            existingEmployee.DateOfBirth = updatedData.DateOfBirth;
            
            if (updatedData.Department != null)
            {
                existingEmployee.Department = updatedData.Department;
            }
            
            if (updatedData.Skills != null && updatedData.Skills.Count > 0)
            {
                existingEmployee.Skills = updatedData.Skills;
            }

            // 5. Filter the employee list data for the input id and return that as the output
            return Ok(existingEmployee);
        }
    }
}