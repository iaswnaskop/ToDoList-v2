using System.ComponentModel.DataAnnotations;
using LoginPage.Entities;

namespace LoginPage.Models;

public class UserRegister
{
    [Required(ErrorMessage = "Username is required")]
    [Display(Name = "Username")]
    public string Name { get; set; } 
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    [Display(Name = "Email")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    [StringLength(15, ErrorMessage = "The password should be at least 8 characters long.", MinimumLength = 8)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,15}$", 
        ErrorMessage = "-Include at least one lowercase letter, " +
                       "-Include at least one uppercase letter, " +
                       "-Include at least one number" +
                       "-Include at least one special character.")]
    [Display(Name = "Password")]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "Please select a role")]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a valid role")]
    
    [Display(Name = "Role")]
    public int  RoleId { get; set; }
    public IEnumerable<Role> AvailableRoles { get; set; } = new List<Role>();
}