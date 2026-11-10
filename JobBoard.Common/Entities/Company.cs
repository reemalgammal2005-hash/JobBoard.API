
namespace JobBoard.Common.Entities;

public class Company
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string CompanyName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string? Website { get; set; }

    public string? Location { get; set; }

    public string? LogoUrl { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public ApplicationUser User { get; set; } = null!;

    public ICollection<Job> Jobs { get; set; }
        = new List<Job>();
}
