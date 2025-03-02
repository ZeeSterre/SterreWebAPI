using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

public class RegisterRequest
{
    [Required]
    public string UserName { get; set; }

    [Required]
    [MinLength(10, ErrorMessage = "Wachtwoord moet minimaal 10 tekens lang zijn.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{10,}$",
        ErrorMessage = "Let op: wachtwoord moet minstens één hoofdletter, kleine letter, cijfer en speciaal teken bevatten.")]
    public string Password { get; set; }
}

public class AppUser : IdentityUser
{
    public override string? Email { get => null; set { } }
    public override string? NormalizedEmail { get => null; set { } }
}
public class LoginRequest
{
    [Required]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }
}

public class LoginResponse
{
    public string? Message { get; set; }
    public string? Token { get; set; }
}