namespace ClashOfCodes.Shared.Models;

public class Problem
{
    public int Id { get; set; }     //primary key
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Difficulty { get; set; }
    public string Topic { get; set; } = string.Empty;
    public string TestCaseJson { get; set; } = "[]";
    public string HiddenTestCaseJson { get; set; } = "[]";

    // --- RELATIONSHIP: Problem & McqQuestion ---
    // Can one Problem have many MCQ Questions? Yes.
    // Can one MCQ Question belong to many Problems? No.
    // Result: One-to-Many (1 Problem -> Many MCQs).
    // Problem is the "One" side, so it gets a List of the "Many".
    public List<McqQuestion> McqQuestions { get; set; } = new List<McqQuestion>();

    // Store test cases as JSON: [{"Input":"5","ExpectedOutput":"25"}]
    public string TestCasesJson { get; set; } = "[]";
}
