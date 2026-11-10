
using JobBoard.BLL.Interfaces;
using JobBoard.Common.DTOs.Applications;
using JobBoard.Common.Entities;
using JobBoard.Common.Enums;
using JobBoard.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.BLL.Services;

public class ApplicationService : IApplicationService
{
    private readonly ApplicationDbContext _context;

    public ApplicationService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Candidate applies for a job
    public async Task<ApplicationResponseDto> ApplyAsync(
        int jobId, ApplyJobDto dto, int userId)
    {
        var candidate = await _context.CandidateProfiles
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (candidate == null)
            throw new InvalidOperationException("Candidate profile not found.");

        var job = await _context.Jobs
            .FirstOrDefaultAsync(j =>
                j.Id == jobId &&
                j.Status == JobStatus.Published);

        if (job == null)
            throw new InvalidOperationException("Job not found or not published.");

        if (job.ApplicationDeadline.HasValue &&
            job.ApplicationDeadline.Value < DateTime.UtcNow)
        {
            throw new InvalidOperationException("Application deadline has passed.");
        }

        var alreadyApplied = await _context.Applications
            .AnyAsync(a =>
                a.JobId == jobId &&
                a.CandidateId == candidate.Id);

        if (alreadyApplied)
            throw new InvalidOperationException("You already applied for this job.");

        var application = new JobBoard.Common.Entities.Application
        {
            JobId = jobId,
            CandidateId = candidate.Id,
            CoverLetter = dto.CoverLetter,
            ResumeUrl = dto.ResumeUrl,
            Status = ApplicationStatus.Pending,
            AppliedAt = DateTime.UtcNow
        };

        _context.Applications.Add(application);
        await _context.SaveChangesAsync();

        return await GetApplicationResponseAsync(application.Id)
            ?? throw new InvalidOperationException("Application could not be loaded.");
    }

    // Candidate views their applications
    public async Task<List<ApplicationResponseDto>> GetMyApplicationsAsync(
        int userId)
    {
        var candidate = await _context.CandidateProfiles
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (candidate == null)
            throw new InvalidOperationException("Candidate profile not found.");

        return await _context.Applications
            .AsNoTracking()
            .Where(a => a.CandidateId == candidate.Id)
            .OrderByDescending(a => a.AppliedAt)
            .Select(a => new ApplicationResponseDto
            {
                Id = a.Id,
                JobId = a.JobId,
                JobTitle = a.Job.Title,
                CandidateId = a.CandidateId,
                CandidateName = a.Candidate.User.FullName,
                CoverLetter = a.CoverLetter,
                ResumeUrl = a.ResumeUrl,
                Status = a.Status,
                RecruiterNotes = a.RecruiterNotes,
                AppliedAt = a.AppliedAt
            })
            .ToListAsync();
    }

    // Company views applicants for its own job
    public async Task<List<ApplicationResponseDto>> GetJobApplicantsAsync(
        int jobId, int userId)
    {
        var ownsJob = await _context.Jobs
            .AnyAsync(j => j.Id == jobId && j.Company.UserId == userId);

        if (!ownsJob)
            throw new InvalidOperationException(
                "Job not found or you do not own this job.");

        return await _context.Applications
            .AsNoTracking()
            .Where(a => a.JobId == jobId)
            .OrderByDescending(a => a.AppliedAt)
            .Select(a => new ApplicationResponseDto
            {
                Id = a.Id,
                JobId = a.JobId,
                JobTitle = a.Job.Title,
                CandidateId = a.CandidateId,
                CandidateName = a.Candidate.User.FullName,
                CoverLetter = a.CoverLetter,
                ResumeUrl = a.ResumeUrl,
                Status = a.Status,
                RecruiterNotes = a.RecruiterNotes,
                AppliedAt = a.AppliedAt
            })
            .ToListAsync();
    }

    // Company updates application status
    public async Task<bool> UpdateStatusAsync(
        int applicationId,
        UpdateApplicationStatusDto dto,
        int userId)
    {
        var application = await _context.Applications
            .Include(a => a.Job)
                .ThenInclude(j => j.Company)
            .FirstOrDefaultAsync(a =>
                a.Id == applicationId &&
                a.Job.Company.UserId == userId);

        if (application == null)
            return false;

        if (!Enum.IsDefined(typeof(ApplicationStatus), dto.Status))
            throw new InvalidOperationException("Invalid application status.");

        application.Status = dto.Status;
        application.RecruiterNotes = dto.RecruiterNotes;
        application.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    private async Task<ApplicationResponseDto?> GetApplicationResponseAsync(
        int applicationId)
    {
        return await _context.Applications
            .AsNoTracking()
            .Where(a => a.Id == applicationId)
            .Select(a => new ApplicationResponseDto
            {
                Id = a.Id,
                JobId = a.JobId,
                JobTitle = a.Job.Title,
                CandidateId = a.CandidateId,
                CandidateName = a.Candidate.User.FullName,
                CoverLetter = a.CoverLetter,
                ResumeUrl = a.ResumeUrl,
                Status = a.Status,
                RecruiterNotes = a.RecruiterNotes,
                AppliedAt = a.AppliedAt
            })
            .FirstOrDefaultAsync();
    }
}
