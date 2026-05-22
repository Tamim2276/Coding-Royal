using ClashOfCodes.Models;
using Microsoft.EntityFrameworkCore;

namespace ClashOfCodes.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Match> Matches { get; set; }

    public DbSet<Problem> Problems { get; set; }

}
