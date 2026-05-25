using ClashOfCodes.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClashOfCodes.API.Data;

public class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    // We don't need to define a DbSet for User because IdentityDbContext already includes it as "Users".
    // public DbSet<User> Users { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Problem> Problems { get; set; }

    public DbSet<McqQuestion> McqQuestions { get; set; }
    public DbSet<MatchPlayer> MatchPlayers { get; set; }
    public DbSet<Room> Rooms { get; set; }

}
