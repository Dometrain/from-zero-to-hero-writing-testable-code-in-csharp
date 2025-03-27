using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OrderManagement.Api.Controllers;


// DEMO ONLY: This is a simple auth controller for testing purposes.
// In a real application, you would use a more robust authentication system
// such as OAuth or OpenID Connect.
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        switch (request.Email)
        {
            // This is just for DEMO - DON'T DO THIS in a real app
            case "admin@example.com" when request.Password == "admin123":
            {
                var token = GenerateJwtToken(request.Email, "Admin", "1");
                return Ok(new { token, role = "Admin" });
            }
            case "user@example.com" when request.Password == "user123":
            {
                var token = GenerateJwtToken(request.Email, "User", "2");
                return Ok(new { token, role = "User" });
            }
            default:
                return Unauthorized();
        }
    }

    private string GenerateJwtToken(string email, string role, string userId)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
} 