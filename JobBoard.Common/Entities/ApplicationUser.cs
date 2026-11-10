
using Microsoft.AspNetCore.Identity;

namespace JobBoard.Common.Entities;

public class ApplicationUser : IdentityUser<int>
{
    public string FullName { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public Company? Company { get; set; }

    public CandidateProfile? CandidateProfile { get; set; }
}
