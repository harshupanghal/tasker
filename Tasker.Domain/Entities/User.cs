namespace Tasker.Domain.Entities;

// -- user model --

/* we are storing : 
 *  - username
 *  - password (plain, as it is) - later we can add encryption.
 *  - date of creation 
 */
public class User
{
    public int Id { get; set; }
    public string userName { get; set; }
    public string password { get; set; }
    public DateTime CreatedAt { get; set; }
}
