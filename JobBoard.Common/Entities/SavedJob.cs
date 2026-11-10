
namespace JobBoard.Common.Entities;

public class SavedJob
{
    public int CandidateProfileId { get; set; }

    public int JobId { get; set; }

    public DateTime SavedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public CandidateProfile CandidateProfile { get; set; } = null!;

    public Job Job { get; set; } = null!;
}
