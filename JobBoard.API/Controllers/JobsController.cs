
using System.Security.Claims;
using JobBoard.BLL.Interfaces;
using JobBoard.Common.DTOs.Jobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobsController : ControllerBase
{
    private readonly IJobService _jobService;

    public JobsController(IJobService jobService)
    {
        _jobService = jobService;
    }

    // GET: api/Jobs
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var jobs = await _jobService.GetAllAsync();
        return Ok(jobs);
    }

    // GET: api/Jobs/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var job = await _jobService.GetByIdAsync(id);

        if (job == null)
            return NotFound(new { message = "Job not found." });

        return Ok(job);
    }

    // POST: api/Jobs
    [Authorize(Roles = "Company")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateJobDto dto)
    {
        try
        {
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _jobService.CreateAsync(dto, userId);

            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT: api/Jobs/1
    [Authorize(Roles = "Company")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateJobDto dto)
    {
        try
        {
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _jobService.UpdateAsync(id, dto, userId);

            if (result == null)
                return NotFound(new { message = "Job not found." });

            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE: api/Jobs/1
    [Authorize(Roles = "Company")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var deleted = await _jobService.DeleteAsync(id, userId);

        if (!deleted)
            return NotFound(new { message = "Job not found." });

        return Ok(new { message = "Job closed successfully." });
    }
}
