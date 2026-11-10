
namespace JobBoard.Common.Entities;

public class CandidateSkill
{
    public int CandidateProfileId { get; set; }

    public int SkillId { get; set; }

    public string? ProficiencyLevel { get; set; }

    public int? YearsOfExperience { get; set; }

    // Navigation Properties
    public CandidateProfile CandidateProfile { get; set; } = null!;

    public Skill Skill { get; set; } = null!;
}
