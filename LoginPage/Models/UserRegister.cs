using System.ComponentModel.DataAnnotations;
using LoginPage.Entities;

namespace LoginPage.Models;

public class UserRegister
{
    
    public string Name { get; set; } 
    public string Email { get; set; }
    public string Password { get; set; }
    
    [Display(Name = "Role")]
    public int  RoleId { get; set; }
    public IEnumerable<Role> AvailableRoles { get; set; } = new List<Role>();
}