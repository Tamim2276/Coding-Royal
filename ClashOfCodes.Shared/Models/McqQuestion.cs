namespace ClashOfCodes.Shared.Models;

public class McqQuestion
{
    public int Id { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string OptionsJson { get; set; } = "[]";
    public int CorrectOptionIndex { get; set; }
    // --- RELATIONSHIP: Problem & McqQuestion ---
    // Can one Problem have many MCQ Questions? Yes. 
    // Can one MCQ Question belong to many Problems? No.
    // Result: One-to-Many (1 Problem -> Many MCQs).
    // Golden Rule: The "Many" side (McqQuestion) gets the Foreign Key.
    public int ProblemId { get; set; }
    public Problem? Problem { get; set; }
}