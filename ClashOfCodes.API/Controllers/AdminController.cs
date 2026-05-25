using ClashOfCodes.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClashOfCodes.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly AppDbContext _context;
    public AdminController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("all-data")]
    public async Task<IActionResult> GetAllData()
    {
        // with navigation properties may cause System.Text.Json object cycle errors 
        // unless ReferenceHandler.IgnoreCycles is configured in Program.cs.
        var data = new
        {
            Users = await _context.Users.AsNoTracking().ToListAsync(),
            Matches = await _context.Matches.AsNoTracking().ToListAsync(),
            Problems = await _context.Problems.AsNoTracking().ToListAsync(),
            McqQuestions = await _context.McqQuestions.AsNoTracking().ToListAsync(),
            MatchPlayers = await _context.MatchPlayers.AsNoTracking().ToListAsync(),
            Rooms = await _context.Rooms.AsNoTracking().ToListAsync()
        };

        return Ok(data);
    }
}