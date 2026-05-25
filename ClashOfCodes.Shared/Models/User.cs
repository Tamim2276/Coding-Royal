using Microsoft.AspNetCore.Identity;
namespace ClashOfCodes.Shared.Models;

public class User : IdentityUser<int>
{

    // REMOVED Id, Username, Email, and PasswordHash from here. 
    // IdentityUser provides UserName, Email, and PasswordHash automatically!
    // public int Id { get; set; }    //primary key

    // public string Username { get; set; } = string.Empty;
    // public string Email { get; set; } = string.Empty;
    // public string PasswordHash { get; set; } = string.Empty;

    //game stats
    public int RankPoints { get; set; } = 0;
    public int CurrentArena { get; set; } = 1;
    public int Wins { get; set; } = 0;
    public int Losses { get; set; } = 0;

    //spells as json string
    public string SelectedSpells { get; set; } = "[]";

    // --- RELATIONSHIP: User & Match ---
    // Can one User play in many Matches? Yes.
    // Can one Match have many Users? Yes.
    // Result: Many-to-Many. Requires the "MatchPlayer" join table!
    public List<MatchPlayer> MatchPlayers { get; set; } = new List<MatchPlayer>();

    // --- RELATIONSHIP: User & Room ---
    // 1 User -> Many Rooms. User is the "One" side, so it gets a List.
    public List<Room> CreatedRooms { get; set; } = new List<Room>();
}
