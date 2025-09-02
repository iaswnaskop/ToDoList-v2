using LoginPage.Entities;
using LoginPage.Models;

namespace LoginPage.Services.Login;

public interface ILoginService
{
    Task<(string? Token, string? ErrorMessage)> Login(UserLogin request);
    Task<User> Register(UserRegister request);
    Task<UserModel> GetUserFromToken(string token);
    Task<List<Role>> GetRoles();
    Task<List<UserModel>> GetAllUsers();
    Task<UserModel> GetUserById(Guid id);
}