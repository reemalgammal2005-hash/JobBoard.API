
using JobBoard.Common.Enums;

namespace JobBoard.Common.Entities;

public class Job
{
    public int Id { get; set; }

    public int CompanyId { get; set; }

    public int CategoryId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Requirements { get; set; } = string.Empty;

    public string? Responsibilities { get; set; }

    public string Location { get; set; } = string.Empty;

    public EmploymentType EmploymentType { get; set; }
        = EmploymentType.FullTime;

    public WorkMode WorkMode { get; set; }
        = WorkMode.OnSite;

    public decimal? MinSalary { get; set; }

    public decimal? MaxSalary { get; set; }

    public string Currency { get; set; } = "EGP";

    public JobStatus Status { get; set; } = JobStatus.Draft;

    public DateTime? ApplicationDeadline { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public Company Company { get; set; } = null!;

    public Category Category { get; set; } = null!;

    public ICollection<Application> Applications { get; set; }
        = new List<Application>();

    public ICollection<JobSkill> JobSkills { get; set; }
        = new List<JobSkill>();

    public ICollection<SavedJob> SavedJobs { get; set; }
        = new List<SavedJob>();
}
