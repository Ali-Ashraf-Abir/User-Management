namespace Task4.Models;

using System.ComponentModel.DataAnnotations;
using task4.Enums;

public class User
{
    public int Id { get; set; }
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    [MaxLength(150)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    public AccountStatus AccountStatus { get; set; }
        = AccountStatus.Unverified;
    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    public DateTime? LastLoginTime { get; set; }

    public DateTime RegistrationTime { get; set; }

    public string VerificationToken { get; set; } = string.Empty;


}