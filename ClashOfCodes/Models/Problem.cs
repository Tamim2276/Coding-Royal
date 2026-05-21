namespace ClashOfCodes.Models;

public class Problem
{
    public int Id { get; set; }     //primary key
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Difficulty { get; set; }
    public string Topic { get; set; } = string.Empty;
    public string TestCaseJson { get; set; } = "[]";
    public string HiddenTestCaseJson { get; set; } = "[]";
}
