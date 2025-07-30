namespace Tasker.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string userName { get; set; }
    public string password { get; set; }
    public DateTime CreatedAt { get; set; }
}
