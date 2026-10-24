using Microsoft.AspNetCore.Mvc;
using lazy_pj.Server.Interfaces;
using lazy_pj.Server.Models;

namespace lazy_pj.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _authService;

        public AuthController(IAuth authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserModel user)
        {
            try
            {
                var result = await _authService.RegisterUserAsync(user);
                if (result)
                    return Ok(new { message = "Registration successful!" });

                return BadRequest(new { message = "Registration failed" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserModel user)
        {
            try
            {
                var result = await _authService.LoginUserAsync(user);
                if (result)
                    return Ok(new { message = "Login successful!" });

                return Unauthorized(new { message = "Invalid credentials" });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Edit([FromBody] UserModel user)
        {
            try
            {
                var result = await _authService.EditUserAsync(user);
                if (result)
                    return Ok(new { message = "User updated successfully!" });

                return BadRequest(new { message = "Update failed" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _authService.DeleteUserAsync(id);
                if (result)
                    return Ok(new { message = "User deleted successfully!" });

                return BadRequest(new { message = "Delete failed" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}