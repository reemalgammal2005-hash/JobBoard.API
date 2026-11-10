
namespace JobBoard.Common.Entities;

public class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation Property
    public ICollection<Job> Jobs { get; set; }
    = new List<Job>();
}
