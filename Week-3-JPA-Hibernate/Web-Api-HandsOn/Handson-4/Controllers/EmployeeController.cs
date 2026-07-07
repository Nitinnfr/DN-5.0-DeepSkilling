using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RetailWebApi.Filters;
using RetailWebApi.Models;
using System;
using System.Collections.Generic;

namespace RetailWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [CustomAuthFilter] // Applies the token verification filter to all endpoints in this controller
    public class EmployeeController : ControllerBase
    {
        private static List<Employee> _employeeList = new();

        // Constructor initializes standard mock database entries
        public EmployeeController()
        {
            if (_employeeList.Count == 0)
            {
                _employeeList = GetStandardEmployeeList();
            }
        }

        // Private method defining initial data state setup
        private List<Employee> GetStandardEmployeeList()
        {
            return new List<Employee>
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
                }
            };
        }

        /// <summary>
        /// GET: api/Employee
        /// Bypasses token requirements via [AllowAnonymous] and returns standard array state
        /// </summary>
        [HttpGet]
        [AllowAnonymous] // Overrides controller-level CustomAuthFilter for this specific method
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Employee>))]
        public ActionResult<List<Employee>> Get()
        {
            return Ok(_employeeList);
        }

        /// <summary>
        /// POST: api/Employee
        /// Demonstrates reading objects using [FromBody] from the raw JSON payload body
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create([FromBody] Employee newEmployee)
        {
            if (newEmployee == null) return BadRequest("Invalid payload model input context.");
            _employeeList.Add(newEmployee);
            return Ok("Employee successfully indexed into list state tracking.");
        }

        /// <summary>
        /// GET: api/Employee/simulate-crash
        /// Expressly triggers your CustomExceptionFilter configuration to test logging actions
        /// </summary>
        [HttpGet("simulate-crash")]
        [TypeFilter(typeof(CustomExceptionFilter))] // Attaches exception capture to this endpoint
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult SimulateCrash()
        {
            throw new InvalidOperationException("Simulated database constraint failure exception triggered for lab logging verification.");
        }
    }
}