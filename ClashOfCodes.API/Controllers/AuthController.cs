using ClashOfCodes.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
    
}
