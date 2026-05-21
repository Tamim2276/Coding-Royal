namespace ClashOfCodes.Models;

public class User
{
    public int Id { get; set; }    //primary key

    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    //game stats
    public int RankPoints { get; set; } = 0;
    public int CurrentArena { get; set; } = 1;
    public int Wins { get; set; } = 0;
    public int Losses { get; set; } = 0;

    //spells as json string
    public string SelectedSpells { get; set; } = "[]";
}
