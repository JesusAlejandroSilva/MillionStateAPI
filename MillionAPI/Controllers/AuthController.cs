using Microsoft.AspNetCore.Mvc;
using Million.Application.Ports.Interfaces;

namespace Million.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtTokenService _jwt;

        public AuthController(IJwtTokenService jwt) => _jwt = jwt;

        /// <summary>Genera un JWT de prueba para usar en Swagger</summary>
        [HttpPost("token")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public IActionResult CreateToken([FromBody] LoginRequest request)
        {
            if (request.Username is null || request.Password is null)
                return BadRequest("username/password requeridos");

            var roles = new[] { "Admin" };
            var token = _jwt.GenerateToken(request.Username, roles);
            return Ok(new { token });
        }

        public record LoginRequest(string Username, string Password);
    }
}
