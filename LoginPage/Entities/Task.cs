using System.ComponentModel.DataAnnotations;

namespace LoginPage.Entities;

public class Task
{
    public int Id { get; set; }
    
    public required string Title { get; set; }
    public string? Description { get; set; }
    
    public required int StatusId { get; set; }
    public Status Status { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? UntilAt { get; set; }
    public ICollection<UserTask> UserTasks { get; set; }
    //public Guid UserId { get; set; }
}