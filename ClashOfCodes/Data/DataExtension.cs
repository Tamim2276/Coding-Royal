
using ClashOfCodes.Data;
using ClashOfCodes.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class DataExtension
{
    public static void MigrateDatabase(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.Migrate();
        }
    }

    public static void AddClashOfCodesDbContext(this WebApplicationBuilder builder)
    {

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? "Data Source=ClashOfCodes.db";

        builder.Services.AddSqlite<AppDbContext>(
            connectionString,
            optionsAction: options => options.UseSeeding((context, _) =>
            {
                var appDb = (AppDbContext)context;
                if (!appDb.Users.Any())
                {
                    appDb.Users.AddRange(
                        new User { Username = "Alice", Email = "alice@example.com" },
                        new User { Username = "Bob", Email = "bob@example.com" }
                    );
                    appDb.SaveChanges();
                }
                if (!appDb.Problems.Any())
                {
                    appDb.Problems.AddRange(
                        new Problem { Title = "Two Sum", Description = "Find two numbers that add up to a target." },
                        new Problem { Title = "Reverse String", Description = "Reverse a given string." }
                    );
                    appDb.SaveChanges();
                }
                if (!appDb.Matches.Any())
                {
                    appDb.Matches.AddRange(
                        new Match { RoomCode = "ABCD", IsRanked = true, WinnerId = 1 },
                        new Match { RoomCode = "EFGH", IsRanked = false, WinnerId = 2 }
                    );
                    appDb.SaveChanges();
                }
            })
        );
    }


}