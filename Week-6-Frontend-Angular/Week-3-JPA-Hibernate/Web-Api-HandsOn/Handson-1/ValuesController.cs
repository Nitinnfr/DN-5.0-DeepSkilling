using Microsoft.AspNetCore.Mvc;

namespace RetailWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Routes to: api/values
    public class ValuesController : ControllerBase
    {
        // Thread-safe Mock Database State Storage
        private static readonly List<string> _mockStorage = new() { "Value1", "Value2", "Value3" };

        // 1. GET: api/values (Read All Actions)
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_mockStorage); // Status: 200 OK
        }

        // 2. GET: api/values/{id} (Read Single Action)
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            if (id < 0 || id >= _mockStorage.Count)
            {
                return NotFound($"Item at position {id} does not exist."); // Status: 404 Not Found
            }
            return Ok(_mockStorage[id]);
        }

        // 3. POST: api/values (Write/Create Action)
        [HttpPost]
        public IActionResult Create([FromBody] string newValue)
        {
            if (string.IsNullOrWhiteSpace(newValue))
            {
                return BadRequest("Payload string cannot be empty."); // Status: 400 Bad Request
            }
            _mockStorage.Add(newValue);
            int itemIndex = _mockStorage.Count - 1;
            
            return CreatedAtAction(nameof(GetById), new { id = itemIndex }, newValue); // Status: 201 Created
        }

        // 4. PUT: api/values/{id} (Write/Update Action)
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] string updatedValue)
        {
            if (id < 0 || id >= _mockStorage.Count) return NotFound("Resource index out of bounds.");
            if (string.IsNullOrWhiteSpace(updatedValue)) return BadRequest("Invalid update body.");

            _mockStorage[id] = updatedValue;
            return NoContent(); // Status: 204 No Content
        }

        // 5. DELETE: api/values/{id} (Delete Action)
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            if (id < 0 || id >= _mockStorage.Count) return NotFound("Target element not found.");
            
            _mockStorage.RemoveAt(id);
            return NoContent(); // Status: 204 No Content
        }
    }
}