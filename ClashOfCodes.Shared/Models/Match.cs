namespace ClashOfCodes.Shared.Models;

public class Match
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public string RoomCode { get; set; } = string.Empty;
    public bool IsRanked { get; set; }
    public int WinnerId { get; set; }
    // --- RELATIONSHIP: User & Match ---
    // Can one Match have many Users? Yes.
    // Can one User play in many Matches? Yes.
    // Result: Many-to-Many. Requires the "MatchPlayer" join table!
    public List<MatchPlayer> MatchPlayers { get; set; } = new List<MatchPlayer>();

}