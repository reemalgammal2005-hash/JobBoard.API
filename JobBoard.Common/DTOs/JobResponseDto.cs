
using JobBoard.Common.Enums;

namespace JobBoard.Common.DTOs.Jobs;

public class JobResponseDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Requirements { get; set; } = string.Empty;

    public string? Responsibilities { get; set; }

    public string Location { get; set; } = string.Empty;

    public string CompanyName { get; set; } = string.Empty;

    public string CategoryName { get; set; } = string.Empty;

    public EmploymentType EmploymentType { get; set; }

    public WorkMode WorkMode { get; set; }

    public decimal? MinSalary { get; set; }

    public decimal? MaxSalary { get; set; }

    public string Currency { get; set; } = "EGP";

    public JobStatus Status { get; set; }

    public DateTime? ApplicationDeadline { get; set; }

    public DateTime CreatedAt { get; set; }
}
