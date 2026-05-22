namespace ClashOfCodes.Models;

public class Room
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string ConfigurationJson { get; set; } = "{}";
    public string Status { get; set; } = "Waiting"; // Waiting, Active, Closed

    // --- RELATIONSHIP: User & Room ---
    // Can one User host many Rooms? Yes. A player might create a room on Monday, and another room on Tuesday.
    // Can one Room have many Host Users? No. A room is created by exactly one owner.
    // Result: One-to-Many (1 User -> Many Rooms). 
    // Golden Rule: The "Many" side (Room) gets the Foreign Key.
    public int HostUserId { get; set; }
    public User? HostUser { get; set; }

}