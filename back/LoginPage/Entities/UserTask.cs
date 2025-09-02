namespace LoginPage.Entities;

public class UserTask
{
    public Guid UserId { get; set; }
    public User User { get; set; }

    public int TaskId { get; set; }
    public Task Task { get; set; }
}