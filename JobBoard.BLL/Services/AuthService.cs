
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JobBoard.BLL.Interfaces;
using JobBoard.Common.DTOs.Auth;
using JobBoard.Common.Entities;
using JobBoard.DAL.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JobBoard.BLL.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager;
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<int>> roleManager,
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
        _configuration = configuration;
    }

    // =========================
    // Register
    // =========================

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        if (dto.Role != "Candidate" && dto.Role != "Company")
            throw new InvalidOperationException("Invalid account role.");

        var existingUser = await _userManager.FindByEmailAsync(dto.Email);

        if (existingUser != null)
            throw new InvalidOperationException("Email is already registered.");

        // Create role if it doesn't exist
        if (!await _roleManager.RoleExistsAsync(dto.Role))
        {
            var roleResult = await _roleManager.CreateAsync(
                new IdentityRole<int>(dto.Role));

            if (!roleResult.Succeeded)
                throw new InvalidOperationException(
                    string.Join(", ", roleResult.Errors.Select(e => e.Description)));
        }

        // Create user
        var user = new ApplicationUser
        {
            FullName = dto.FullName,
            UserName = dto.Email,
            Email = dto.Email,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException(
                string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        try
        {
            // Assign role
            var roleResult = await _userManager.AddToRoleAsync(user, dto.Role);

            if (!roleResult.Succeeded)
                throw new InvalidOperationException(
                    string.Join(", ", roleResult.Errors.Select(e => e.Description)));

            // Create profile
            if (dto.Role == "Company")
            {
                var company = new Company
                {
                    UserId = user.Id,
                    CompanyName = dto.FullName,
                    Description = string.Empty
                };

                _context.Companies.Add(company);
            }
            else
            {
                var candidate = new CandidateProfile
                {
                    UserId = user.Id
                };

                _context.CandidateProfiles.Add(candidate);
            }

            await _context.SaveChangesAsync();
        }
        catch
        {
            await _userManager.DeleteAsync(user);
            throw;
        }

        return await GenerateAuthResponseAsync(user, dto.Role);
    }

    // =========================
    // Login
    // =========================

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null)
            throw new InvalidOperationException("Invalid email or password.");

        if (!user.IsActive)
            throw new InvalidOperationException("Your account is inactive.");

        var passwordValid = await _userManager.CheckPasswordAsync(
            user, dto.Password);

        if (!passwordValid)
            throw new InvalidOperationException("Invalid email or password.");

        var roles = await _userManager.GetRolesAsync(user);

        if (roles.Count == 0)
            throw new InvalidOperationException("User has no assigned role.");

        return await GenerateAuthResponseAsync(user, roles[0]);
    }

    // =========================
    // Generate JWT
    // =========================

    private async Task<AuthResponseDto> GenerateAuthResponseAsync(
        ApplicationUser user,
        string role)
    {
        var key = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("JWT Key is missing.");

        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];

        var duration = int.Parse(
            _configuration["Jwt:DurationInMinutes"] ?? "60");

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(key));

        var credentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256);

        var expiresAt = DateTime.UtcNow.AddMinutes(duration);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return new AuthResponseDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email!,
            Role = role,
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = expiresAt
        };
    }
}
