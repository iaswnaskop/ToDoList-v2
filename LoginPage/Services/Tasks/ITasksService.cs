using LoginPage.Models;
using Status = LoginPage.Entities.Status;
using Task = LoginPage.Entities.Task;

namespace LoginPage.Services.Tasks;

public interface ITasksService
{
    public Task<List<Status>> GetStatus();
    public Task<List<Task>> GetTasksByUserId(Guid id);
    
    //public Task<Task> CreateTaskByAdmin(Models.CreateTaskModel task);
    public Task<Task> CreateTaskByUser(Models.CreateTaskModel task);
    public Task<Task> GetTasksById(int id);
    public Task<Task> UpdateTask(int id, Models.CreateTaskModel task);
    public Task<bool> DeleteTask(int id);
}