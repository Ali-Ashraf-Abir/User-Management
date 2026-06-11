namespace task4.Dtos;

using task4.Enums;

public class UserDto
{   
    public int Id {get;set;}
    public string Email { set; get; } = string.Empty;
    public string FullName { set; get; } = string.Empty;
    public AccountStatus AccountStatus { get; set; }
        = AccountStatus.Unverified;

    public DateTime? LastLoginTime { get; set; }

    public DateTime RegistrationTime { get; set; }

    public bool IsBlocked {get;set;}


}