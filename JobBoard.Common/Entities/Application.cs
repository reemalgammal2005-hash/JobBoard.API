
using JobBoard.Common.Enums;

namespace JobBoard.Common.Entities;

public class Application
{
    public int Id { get; set; }

    public int JobId { get; set; }

    public int CandidateId { get; set; }

    public string CoverLetter { get; set; } = string.Empty;

    public ApplicationStatus Status { get; set; }
        = ApplicationStatus.Pending;

    public string? ResumeUrl { get; set; }

    public string? RecruiterNotes { get; set; }

    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public Job Job { get; set; } = null!;

    public CandidateProfile Candidate { get; set; } = null!;
}
