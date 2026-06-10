
using System.ComponentModel.DataAnnotations;

class User
{
    public int Id {get;set;}
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string FullName { get; set; } = string.Empty;
    public bool AccountStatus{get;set;}

  
}