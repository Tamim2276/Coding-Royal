using System.Text.Json.Serialization;

namespace ClashOfCodes.Shared.Models;

public class Problem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Difficulty { get; set; }      // 1=Easy, 2=Medium, 3=Hard
    public string Topic { get; set; } = string.Empty;
    public string TestCasesJson { get; set; } = "[]";
    public string HiddenTestCasesJson { get; set; } = "[]";

    // JsonIgnore prevents infinite loop when API serializes this to JSON.
    // The Blazor client doesn't need the MCQ questions inside the problem —
    // it fetches them separately via GET /api/mcq/{problemId}
    [JsonIgnore]
    public List<McqQuestion> McqQuestions { get; set; } = new();
}