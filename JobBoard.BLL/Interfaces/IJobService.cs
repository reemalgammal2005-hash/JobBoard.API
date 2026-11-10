
using JobBoard.Common.DTOs.Jobs;

namespace JobBoard.BLL.Interfaces;

public interface IJobService
{
    Task<List<JobResponseDto>> GetAllAsync();

    Task<JobResponseDto?> GetByIdAsync(int id);

    Task<JobResponseDto> CreateAsync(CreateJobDto dto, int userId);

    Task<JobResponseDto?> UpdateAsync(int id, UpdateJobDto dto, int userId);

    Task<bool> DeleteAsync(int id, int userId);
}
