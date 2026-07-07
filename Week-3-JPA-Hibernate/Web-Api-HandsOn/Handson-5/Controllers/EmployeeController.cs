using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RetailWebApi.Models;

namespace RetailWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // Lab Step 4 Check: Test access restrictions by changing these roles configurations
    [Authorize(Roles = "Admin,POC")] // Allows users matching either Admin OR POC access profiles
    public class EmployeeController : ControllerBase
    {
        private static readonly List<Employee> _employeeList = new()
        {
            new Employee
            {
                Id = 1,
                Name = "Nitin Singh",
                Salary = 85000,
                Permanent = true,
                DateOfBirth = new DateTime(2001, 05, 15)
            }
        };

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Employee>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Get()
        {
            return Ok(_employeeList);
        }
    }
}