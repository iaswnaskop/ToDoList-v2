using LoginPage.Entities;

namespace LoginPage.Models;

public class UserModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public Role? Role { get; set; }
    public int? RoleId { get; set; }
    public string? RoleName { get; set; }
}

public class UserViewModel
{
    public UserModel CurrentUser { get; set; }
    public List<UserModel>? AllUsers { get; set; }
    public List<Tasks> Tasks { get; set; } = new List<Tasks>();
    public UpdateTaskModel UpdateTask { get; set; }
    public List<Status> StatusList { get; set; } = new List<Status>();
}