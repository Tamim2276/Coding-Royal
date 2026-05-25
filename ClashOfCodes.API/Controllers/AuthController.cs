using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ClashOfCodes.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ClashOfCodes.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager; // UserManager is a service provided by ASP.NET Core Identity to manage user accounts, including creating users, hashing passwords, etc.
    private readonly IConfiguration _configuration; // IConfiguration is used to access configuration settings, such as the JWT secret key, from appsettings.json or environment variables.

    public AuthController(UserManager<User> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        // create a User object
        var user = new User
        {
            UserName = model.Username,
            Email = model.Email
        };

        //save it and password hash in database using UserManager
        var result = await _userManager.CreateAsync(user, model.Password); // CreateAsync will hash the password and save the user to the database. It returns an IdentityResult indicating success or failure.

        if (result.Succeeded)
        {
            return Ok(new { message = "User registered successfully" });
        }
        else
        {
            return BadRequest(result.Errors);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        // find the user by username
        var user = await _userManager.FindByNameAsync(model.Username);

        // check if the user exits and password is correct
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            // create clams for JWT token
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.UserName!),// ClaimTypes.Name is a standard claim type for the username. user.UserName is the username of the authenticated user.
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),// ClaimTypes.NameIdentifier is a standard claim type for the user ID. user.Id is the unique identifier of the user in the database.
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()) // unique identifier for the token
            };

            //create the secret key and signing credentials for JWT token
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]!)); // Get the secret key from configuration and create a SymmetricSecurityKey for signing the JWT token.
            // Create the JWT token
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)// Use HMAC SHA256 algorithm for signing the token
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token); // Serialize the JWT token to a string
            return Ok(new { token = tokenString });// Return the token to the client
        }
        return Unauthorized(new { message = "Invalid username or password" });
    }

}
