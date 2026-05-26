using System.ComponentModel.DataAnnotations;

namespace ClashOfCodes.Shared.Models;

public class RegisterModel
{
    [Required(ErrorMessage = "You MUST type a username!")]
    public string Username { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "You MUST enter a valid email address!")]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "You MUST type a password!")]
    public string Password { get; set; } = string.Empty;
}

public class LoginModel
{
    [Required(ErrorMessage = "You MUST type a username!")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "You MUST type a password!")]
    public string Password { get; set; } = string.Empty;
}