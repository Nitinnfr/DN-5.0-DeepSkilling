using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicesJwt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecureController : ControllerBase
    {
        [HttpGet("data")]
        [Authorize] // Requires any valid JWT token
        public IActionResult GetSecureData()
        {
            return Ok("This is protected data accessible by any authenticated user.");
        }
    }
}