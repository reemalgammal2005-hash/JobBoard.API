
namespace JobBoard.Common.Entities;

public class Skill
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<CandidateSkill> CandidateSkills { get; set; }
        = new List<CandidateSkill>();

    public ICollection<JobSkill> JobSkills { get; set; }
        = new List<JobSkill>();
}
