
using System.ComponentModel.DataAnnotations;

namespace JobBoard.Common.DTOs.Applications;

public class ApplyJobDto
{
    [Required]
    public string CoverLetter { get; set; } = string.Empty;

    public string? ResumeUrl { get; set; }
}

