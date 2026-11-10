
using System.ComponentModel.DataAnnotations;
using JobBoard.Common.Enums;

namespace JobBoard.Common.DTOs.Jobs;

public class CreateJobDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public string Requirements { get; set; } = string.Empty;

    public string? Responsibilities { get; set; }

    [Required]
    public string Location { get; set; } = string.Empty;

    public int CategoryId { get; set; }

    public EmploymentType EmploymentType { get; set; }

    public WorkMode WorkMode { get; set; }

    public decimal? MinSalary { get; set; }

    public decimal? MaxSalary { get; set; }

    public string Currency { get; set; } = "EGP";

    public DateTime? ApplicationDeadline { get; set; }
}
