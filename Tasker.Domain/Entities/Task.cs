namespace Tasker.Domain.Entities;

// here i have deifned all the schema or models of entities, 
// basically its a structure of our entities, what value they have etc.

// -- Task Model -- 

/* here we are storing :
 * - title
 * - description
 * - completion status
 * - userid (to keep track of which user have which tasks)
 * - creation time
 * - updation time
 */

// userid is stored as foreign key, that is refering to user table

public class Task
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; } 
    public bool IsCompleted { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
