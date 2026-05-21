namespace ClashOfCodes.Models;

public class Match
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public string RoomCode { get; set; } = string.Empty;
    public bool IsRanked { get; set; }
    public int WinnerId { get; set; }
}