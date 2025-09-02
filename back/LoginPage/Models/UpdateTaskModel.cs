namespace LoginPage.Models;

public class UpdateTaskModel
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateTime? UntilAt { get; set; }
    public int StatusId { get; set; }
    public string StatusName { get; set; } = string.Empty;
}