using ClashOfCodes.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ClashOfCodes.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Problem> Problems { get; set; }

    public DbSet<McqQuestion> McqQuestions { get; set; }
    public DbSet<MatchPlayer> MatchPlayers { get; set; }
    public DbSet<Room> Rooms { get; set; }

}
