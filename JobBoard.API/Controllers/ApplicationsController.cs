
using System.Security.Claims;
using JobBoard.BLL.Interfaces;
using JobBoard.Common.DTOs.Applications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ApplicationsController : ControllerBase
{
    private readonly IApplicationService _applicationService;

    public ApplicationsController(IApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    private int GetUserId()
    {
        return int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    // POST: api/Applications/jobs/1
    [Authorize(Roles = "Candidate")]
    [HttpPost("jobs/{jobId}")]
    public async Task<IActionResult> Apply(
        int jobId, ApplyJobDto dto)
    {
        try
        {
            var result = await _applicationService.ApplyAsync(
                jobId, dto, GetUserId());

            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // GET: api/Applications/my-applications
    [Authorize(Roles = "Candidate")]
    [HttpGet("my-applications")]
    public async Task<IActionResult> GetMyApplications()
    {
        try
        {
            var result = await _applicationService
                .GetMyApplicationsAsync(GetUserId());

            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // GET: api/Applications/jobs/1/applicants
    [Authorize(Roles = "Company")]
    [HttpGet("jobs/{jobId}/applicants")]
    public async Task<IActionResult> GetJobApplicants(int jobId)
    {
        try
        {
            var result = await _applicationService
                .GetJobApplicantsAsync(jobId, GetUserId());

            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT: api/Applications/1/status
    [Authorize(Roles = "Company")]
    [HttpPut("{applicationId}/status")]
    public async Task<IActionResult> UpdateStatus(
        int applicationId, UpdateApplicationStatusDto dto)
    {
        try
        {
            var updated = await _applicationService.UpdateStatusAsync(
                applicationId, dto, GetUserId());

            if (!updated)
                return NotFound(new { message = "Application not found." });

            return Ok(new { message = "Application status updated successfully." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
