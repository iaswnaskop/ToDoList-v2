using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LoginPage.Context;
using LoginPage.Entities;
using LoginPage.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Status = LoginPage.Models.Status;
using Task = LoginPage.Entities.Task;

namespace LoginPage.Services.Tasks;

public class TasksService(DataContext context,IConfiguration _configuration ) : ITasksService
{
    public async Task<List<Entities.Status>> GetStatus()
    {
        try
        {
            var status = context.Status.ToList();
            if (status.Count == 0)
                return null;
           
            return status;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<List<Task>> GetTasksByUserId(Guid id)
    {
        try
        {
            var tasks = context.Tasks
                .Include(s => s.Status)
                .Include(u => u.UserTasks)
                .Where(u => u.UserTasks.Any(ut => ut.UserId == id))
                .ToList();
            if (tasks.Count == 0)
                return null;
           
            return tasks;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<Task> GetTasksById(int id)
    {
        try
        {
            var task = await context.Tasks
                .Include(s => s.Status)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (task == null)
                return null;
           
            return task;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
    
    public async Task<Task> CreateTaskByUser(Models.CreateTaskModel task)
    {
        try
        {
            if (task == null)
                return null;
            
            var newTask = new Entities.Task
            {
                Title = task.Task.Title,
                Description = task.Task.Description,
                StatusId = 1, // Default status ID on create
                CreatedAt = DateTime.UtcNow,
                UntilAt = task.Task.UntilAt
               
            };

            context.Tasks.Add(newTask);
            await context.SaveChangesAsync();
            List<Guid> userIds = task.UserId;
            foreach (var userId in userIds)
            {
                var userTask = new Entities.UserTask
                {
                    TaskId = newTask.Id,
                    UserId = userId
                };
                context.UsersTasks.Add(userTask);
            }
            await context.SaveChangesAsync();
            return newTask;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Task> UpdateTask(int id, Models.CreateTaskModel task)
    {
        try
        {
            var existingTask = await context.Tasks
                .Include(s => s.Status)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (existingTask == null)
                return null;
            existingTask.Title = task.Task.Title;
            existingTask.Description = task.Task.Description;
            existingTask.StatusId = task.Task.StatusId;
            existingTask.UpdatedAt = DateTime.UtcNow;
            
            await context.SaveChangesAsync();
            return existingTask;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
    public async Task<bool> DeleteTask(int id)
    {
        try
        {
            var task = await context.Tasks.FindAsync(id);
            if (task == null)
                return false;
            
            context.Tasks.Remove(task);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}