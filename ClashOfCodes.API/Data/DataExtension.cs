
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
                        new User { UserName = "Alice", Email = "alice@example.com" },
                        new User { UserName = "Bob", Email = "bob@example.com" }
                    );
                    appDb.SaveChanges();
                }

                if (!appDb.Problems.Any())
                {
                    appDb.Problems.AddRange(
                        // Easy — Difficulty 1
                        new Problem
                        {
                            Title = "Square a Number",
                            Description = "Given an integer N, print N squared.\n\nExample:\nInput: 5\nOutput: 25",
                            Difficulty = 1,
                            Topic = "Math",
                            TestCasesJson = """
                            [
                                {"Input":"5","ExpectedOutput":"25"},
                                {"Input":"3","ExpectedOutput":"9"},
                                {"Input":"10","ExpectedOutput":"100"}
                            ]
                            """
                        },
                        // Medium — Difficulty 2
                        new Problem
                        {
                            Title = "Sum of Array",
                            Description = "Given N integers on separate lines, print their sum.\n\nExample:\nInput:\n3\n1\n2\n3\nOutput: 6",
                            Difficulty = 2,
                            Topic = "Arrays",
                            TestCasesJson = """
                            [
                                {"Input":"3\n1\n2\n3","ExpectedOutput":"6"},
                                {"Input":"4\n10\n20\n30\n40","ExpectedOutput":"100"},
                                {"Input":"2\n5\n5","ExpectedOutput":"10"}
                            ]
                            """
                        },
                        // Hard — Difficulty 3
                        new Problem
                        {
                            Title = "Reverse a String",
                            Description = "Given a string, print it reversed.\n\nExample:\nInput: hello\nOutput: olleh",
                            Difficulty = 3,
                            Topic = "Strings",
                            TestCasesJson = """
                            [
                                {"Input":"hello","ExpectedOutput":"olleh"},
                                {"Input":"world","ExpectedOutput":"dlrow"},
                                {"Input":"abcd","ExpectedOutput":"dcba"}
                            ]
                            """
                        }
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