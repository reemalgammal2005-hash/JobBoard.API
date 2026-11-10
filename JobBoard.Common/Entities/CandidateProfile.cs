
namespace JobBoard.Common.Entities;

public class CandidateProfile
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? Headline { get; set; }

    public string? Summary { get; set; }

    public string? ResumeUrl { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Location { get; set; }

    public int? YearsOfExperience { get; set; }

    public string? Education { get; set; }

    public string? LinkedInUrl { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public ApplicationUser User { get; set; } = null!;

    public ICollection<Application> Applications { get; set; }
        = new List<Application>();

    public ICollection<CandidateSkill> CandidateSkills { get; set; }
        = new List<CandidateSkill>();

    public ICollection<SavedJob> SavedJobs { get; set; }
        = new List<SavedJob>();
}
