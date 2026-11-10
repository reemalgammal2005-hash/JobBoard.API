
using JobBoard.Common.Enums;

namespace JobBoard.Common.DTOs.Jobs;

public class UpdateJobDto
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Requirements { get; set; } = string.Empty;

    public string? Responsibilities { get; set; }

    public string Location { get; set; } = string.Empty;

    public int CategoryId { get; set; }

    public EmploymentType EmploymentType { get; set; }

    public WorkMode WorkMode { get; set; }

    public decimal? MinSalary { get; set; }

    public decimal? MaxSalary { get; set; }

    public string Currency { get; set; } = "EGP";

    public DateTime? ApplicationDeadline { get; set; }

    public JobStatus Status { get; set; }
}
