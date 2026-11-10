
using JobBoard.Common.Enums;

namespace JobBoard.Common.DTOs.Applications;

public class ApplicationResponseDto
{
    public int Id { get; set; }

    public int JobId { get; set; }

    public string JobTitle { get; set; } = string.Empty;

    public int CandidateId { get; set; }

    public string CandidateName { get; set; } = string.Empty;

    public string CoverLetter { get; set; } = string.Empty;

    public string? ResumeUrl { get; set; }

    public ApplicationStatus Status { get; set; }

    public string? RecruiterNotes { get; set; }

    public DateTime AppliedAt { get; set; }
}
