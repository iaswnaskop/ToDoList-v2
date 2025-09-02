namespace LoginPage.Models;

public class CreateTaskModel
{
    public TaskModel Task { get; set; }
    public List<Guid> UserId { get; set; }
}

public class TaskModel
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateTime? UntilAt { get; set; }
    public int StatusId { get; set; }
}