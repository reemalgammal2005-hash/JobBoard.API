
using JobBoard.Common.DTOs.Applications;

namespace JobBoard.BLL.Interfaces;

public interface IApplicationService
{
    Task<ApplicationResponseDto> ApplyAsync(
        int jobId, ApplyJobDto dto, int userId);

    Task<List<ApplicationResponseDto>> GetMyApplicationsAsync(
        int userId);

    Task<List<ApplicationResponseDto>> GetJobApplicantsAsync(
        int jobId, int userId);

    Task<bool> UpdateStatusAsync(
        int applicationId,
        UpdateApplicationStatusDto dto,
        int userId);
}
