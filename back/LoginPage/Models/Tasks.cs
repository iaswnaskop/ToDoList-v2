using LoginPage.Entities;

namespace LoginPage.Models;

public class Tasks
{
    public int Id { get; set; }
    public List<string> UserId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int StatusId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? UntilAt { get; set; }
    public string StatusName { get; set; }
    
}