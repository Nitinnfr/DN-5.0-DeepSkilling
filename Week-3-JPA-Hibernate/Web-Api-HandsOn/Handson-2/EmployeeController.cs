using Microsoft.AspNetCore.Mvc;

namespace RetailWebApi.Controllers
{
    // Lab Objective: Custom Route naming (tested as "api/Employee" or "api/Emp")
    [ApiController]
    [Route("api/[controller]")] // Change to [Route("api/Emp")] when performing the route modification lab step
    public class EmployeeController : ControllerBase
    {
        // Simple inner model definition for simulation
        public class Employee
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Role { get; set; } = string.Empty;
        }

        private static readonly List<Employee> _employees = new()
        {
            new Employee { Id = 101, Name = "Nitin Singh", Role = "Software Engineer Intern" },
            new Employee { Id = 102, Name = "Jane Smith", Role = "DBA Specialist" }
        };

        /// <summary>
        /// GET: api/Employee
        /// Demonstrates explicitly stating response visibility types
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Employee>))] // Documents return type
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ActionName("GetAllEmployees")] // Informative name assignment for documentation clarity
        public IActionResult Get()
        {
            return Ok(_employees); // Status 200 OK with tracking body list payload
        }
    }
}