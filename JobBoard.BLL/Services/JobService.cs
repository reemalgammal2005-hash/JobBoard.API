
using JobBoard.BLL.Interfaces;
using JobBoard.Common.DTOs.Jobs;
using JobBoard.Common.Entities;
using JobBoard.Common.Enums;
using JobBoard.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.BLL.Services;

public class JobService : IJobService
{
    private readonly ApplicationDbContext _context;

    public JobService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Get all published jobs
    public async Task<List<JobResponseDto>> GetAllAsync()
    {
        return await _context.Jobs
            .AsNoTracking()
            .Where(j => j.Status == JobStatus.Published)
            .Select(j => new JobResponseDto
            {
                Id = j.Id,
                Title = j.Title,
                Description = j.Description,
                Requirements = j.Requirements,
                Responsibilities = j.Responsibilities,
                Location = j.Location,
                CompanyName = j.Company.CompanyName,
                CategoryName = j.Category.Name,
                EmploymentType = j.EmploymentType,
                WorkMode = j.WorkMode,
                MinSalary = j.MinSalary,
                MaxSalary = j.MaxSalary,
                Currency = j.Currency,
                Status = j.Status,
                ApplicationDeadline = j.ApplicationDeadline,
                CreatedAt = j.CreatedAt
            })
            .ToListAsync();
    }

    // Get job by ID
    public async Task<JobResponseDto?> GetByIdAsync(int id)
    {
        return await _context.Jobs
            .AsNoTracking()
            .Where(j => j.Id == id && j.Status == JobStatus.Published)
            .Select(j => new JobResponseDto
            {
                Id = j.Id,
                Title = j.Title,
                Description = j.Description,
                Requirements = j.Requirements,
                Responsibilities = j.Responsibilities,
                Location = j.Location,
                CompanyName = j.Company.CompanyName,
                CategoryName = j.Category.Name,
                EmploymentType = j.EmploymentType,
                WorkMode = j.WorkMode,
                MinSalary = j.MinSalary,
                MaxSalary = j.MaxSalary,
                Currency = j.Currency,
                Status = j.Status,
                ApplicationDeadline = j.ApplicationDeadline,
                CreatedAt = j.CreatedAt
            })
            .FirstOrDefaultAsync();
    }

    // Create job
    public async Task<JobResponseDto> CreateAsync(
        CreateJobDto dto, int userId)
    {
        var company = await _context.Companies
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (company == null)
            throw new InvalidOperationException("Company profile not found.");

        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == dto.CategoryId && c.IsActive);

        if (category == null)
            throw new InvalidOperationException("Category not found or inactive.");

        if (dto.MinSalary < 0 || dto.MaxSalary < 0 ||
            (dto.MinSalary.HasValue && dto.MaxSalary.HasValue &&
             dto.MinSalary > dto.MaxSalary))
        {
            throw new InvalidOperationException("Invalid salary range.");
        }

        var job = new Job
        {
            CompanyId = company.Id,
            CategoryId = dto.CategoryId,
            Title = dto.Title,
            Description = dto.Description,
            Requirements = dto.Requirements,
            Responsibilities = dto.Responsibilities,
            Location = dto.Location,
            EmploymentType = dto.EmploymentType,
            WorkMode = dto.WorkMode,
            MinSalary = dto.MinSalary,
            MaxSalary = dto.MaxSalary,
            Currency = dto.Currency,
            Status = JobStatus.Draft,
            ApplicationDeadline = dto.ApplicationDeadline,
            CreatedAt = DateTime.UtcNow
        };

        _context.Jobs.Add(job);
        await _context.SaveChangesAsync();

        return (await GetCompanyJobResponseAsync(job.Id))!;
    }

    // Update job
    public async Task<JobResponseDto?> UpdateAsync(
        int id, UpdateJobDto dto, int userId)
    {
        var job = await _context.Jobs
            .Include(j => j.Company)
            .FirstOrDefaultAsync(j => j.Id == id && j.Company.UserId == userId);

        if (job == null)
            return null;

        var categoryExists = await _context.Categories
            .AnyAsync(c => c.Id == dto.CategoryId && c.IsActive);

        if (!categoryExists)
            throw new InvalidOperationException("Category not found or inactive.");

        if (dto.MinSalary < 0 || dto.MaxSalary < 0 ||
            (dto.MinSalary.HasValue && dto.MaxSalary.HasValue &&
             dto.MinSalary > dto.MaxSalary))
        {
            throw new InvalidOperationException("Invalid salary range.");
        }

        job.Title = dto.Title;
        job.Description = dto.Description;
        job.Requirements = dto.Requirements;
        job.Responsibilities = dto.Responsibilities;
        job.Location = dto.Location;
        job.CategoryId = dto.CategoryId;
        job.EmploymentType = dto.EmploymentType;
        job.WorkMode = dto.WorkMode;
        job.MinSalary = dto.MinSalary;
        job.MaxSalary = dto.MaxSalary;
        job.Currency = dto.Currency;
        job.Status = dto.Status;
        job.ApplicationDeadline = dto.ApplicationDeadline;
        job.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return await GetCompanyJobResponseAsync(job.Id);
    }

    // Close job instead of physically deleting it
    public async Task<bool> DeleteAsync(int id, int userId)
    {
        var job = await _context.Jobs
            .FirstOrDefaultAsync(j => j.Id == id &&
                                      j.Company.UserId == userId);

        if (job == null)
            return false;

        job.Status = JobStatus.Closed;
        job.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    // Get company job including draft and closed jobs
    private async Task<JobResponseDto?> GetCompanyJobResponseAsync(int id)
    {
        return await _context.Jobs
            .AsNoTracking()
            .Where(j => j.Id == id)
            .Select(j => new JobResponseDto
            {
                Id = j.Id,
                Title = j.Title,
                Description = j.Description,
                Requirements = j.Requirements,
                Responsibilities = j.Responsibilities,
                Location = j.Location,
                CompanyName = j.Company.CompanyName,
                CategoryName = j.Category.Name,
                EmploymentType = j.EmploymentType,
                WorkMode = j.WorkMode,
                MinSalary = j.MinSalary,
                MaxSalary = j.MaxSalary,
                Currency = j.Currency,
                Status = j.Status,
                ApplicationDeadline = j.ApplicationDeadline,
                CreatedAt = j.CreatedAt
            })
            .FirstOrDefaultAsync();
    }
}
