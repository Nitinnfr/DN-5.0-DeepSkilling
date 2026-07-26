using Microsoft.AspNetCore.Mvc;

namespace RetailWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Root routing attribute: api/values
    public class ValuesController : ControllerBase // In .NET Core, controllers inherit from ControllerBase instead of ApiController
    {
        // Thread-safe in-memory data store for demonstration
        private static readonly List<string> _mockData = new() { "Value1", "Value2", "Value3" };

        /// <summary>
        /// GET: api/values (Retrieves all items)
        /// Status Codes: 200 OK
        /// </summary>
        [HttpGet]
        public IActionResult GetAll()
        {
            // Returns 200 OK along with the collection payload (Auto-serialized to JSON)
            return Ok(_mockData);
        }

        /// <summary>
        /// GET: api/values/{id} (Retrieves a single item by its index identity)
        /// Status Codes: 200 OK, 404 NotFound, 400 BadRequest
        /// </summary>
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            if (id < 0)
            {
                return BadRequest("ID cannot be negative."); // 400 Bad Request
            }

            if (id >= _mockData.Count)
            {
                return NotFound($"Item at index {id} was not found."); // 404 Not Found
            }

            return Ok(_mockData[id]);
        }

        /// <summary>
        /// POST: api/values (Creates a new item record)
        /// Status Codes: 201 Created, 400 BadRequest
        /// </summary>
        [HttpPost]
        public IActionResult Create([FromBody] string newValue)
        {
            if (string.IsNullOrWhiteSpace(newValue))
            {
                return BadRequest("Value cannot be empty.");
            }

            _mockData.Add(newValue);
            int newIndex = _mockData.Count - 1;

            // 201 Created response indicating exactly where the new resource lives
            return CreatedAtAction(nameof(GetById), new { id = newIndex }, newValue);
        }

        /// <summary>
        /// PUT: api/values/{id} (Updates an existing record completely)
        /// Status Codes: 204 NoContent, 400 BadRequest, 404 NotFound
        /// </summary>
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] string updatedValue)
        {
            if (id < 0 || id >= _mockData.Count)
            {
                return NotFound("Target index not found.");
            }

            if (string.IsNullOrWhiteSpace(updatedValue))
            {
                return BadRequest("Update payload cannot be empty.");
            }

            _mockData[id] = updatedValue;
            return NoContent(); // 204 No Content is standard for successful updates with no return body
        }

        /// <summary>
        /// DELETE: api/values/{id} (Removes a target record item)
        /// Status Codes: 204 NoContent, 404 NotFound
        /// </summary>
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            if (id < 0 || id >= _mockData.Count)
            {
                return NotFound("Target index doesn't exist.");
            }

            _mockData.RemoveAt(id);
            return NoContent(); // 204 No Content
        }

        /// <summary>
        /// GET: api/values/simulate-error (Demonstrates 500 Internal Server Error)
        /// </summary>
        [HttpGet("simulate-error")]
        public IActionResult SimulateError()
        {
            try
            {
                throw new Exception("Simulated database failure.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error occurred: {ex.Message}");
            }
        }
    }
}