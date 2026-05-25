namespace ClashOfCodes.Shared.Models;

public class MatchPlayer
{
    public int Id { get; set; }     //primary key
    public int CrownsEarned { get; set; }
    public int FinalBE { get; set; }
    public int RpChange { get; set; }
    public string SpellsUsedJson { get; set; } = "[]";

    // --- RELATIONSHIP: User & Match (Join Table) ---
    // Can one User play in many Matches? Yes.
    // Can one Match have many Users? Yes.
    // Result: Many-to-Many (User <-> Match).
    // Golden Rule: Create a Join Table (MatchPlayer) that has Foreign Keys for BOTH sides.
    public int UserId { get; set; }
    public User? User { get; set; }

    public int MatchId { get; set; }
    public Match? Match { get; set; }
}