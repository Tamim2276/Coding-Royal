
using ClashOfCodes.API.Data;
using ClashOfCodes.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ClashOfCodes.API.Data;

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
                if (!appDb.McqQuestions.Any())
                {
                    appDb.McqQuestions.AddRange(
                        new McqQuestion { QuestionText = "What is 2 + 2?", OptionsJson = "[\"3\", \"4\", \"5\"]", CorrectOptionIndex = 1, ProblemId = 1 },
                        new McqQuestion { QuestionText = "What is the capital of France?", OptionsJson = "[\"Berlin\", \"Madrid\", \"Paris\"]", CorrectOptionIndex = 2, ProblemId = 2 }
                    );
                    appDb.SaveChanges();
                }
                if (!appDb.MatchPlayers.Any())
                {
                    appDb.MatchPlayers.AddRange(
                        new MatchPlayer { MatchId = 1, UserId = 1 },
                        new MatchPlayer { MatchId = 1, UserId = 2 },
                        new MatchPlayer { MatchId = 2, UserId = 1 }
                    );
                    appDb.SaveChanges();
                }
                if (!appDb.Rooms.Any())
                {
                    appDb.Rooms.AddRange(
                        new Room { Code = "ABCD", HostUserId = 1, ConfigurationJson = "[]", Status = "Waiting" },
                        new Room { Code = "EFGH", HostUserId = 2, ConfigurationJson = "[]", Status = "Waiting" }
                    );
                    appDb.SaveChanges();
                }
            })
        );
    }


}