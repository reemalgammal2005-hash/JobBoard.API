
using JobBoard.Common.Enums;

namespace JobBoard.Common.DTOs.Applications;

public class UpdateApplicationStatusDto
{
    public ApplicationStatus Status { get; set; }

    public string? RecruiterNotes { get; set; }
}
