using System.ComponentModel.DataAnnotations;

namespace src.Models;

public sealed class LoginRequest
{
    [Required, EmailAddress]
    public string Email { get; set; } = "";

    [Required]
    public string Password { get; set; } = "";

    public bool RememberMe { get; set; }
}
