
using JobBoard.Common.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.DAL.Data;

public class ApplicationDbContext
    : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSets
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<CandidateProfile> CandidateProfiles => Set<CandidateProfile>();
    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Application> Applications => Set<Application>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<CandidateSkill> CandidateSkills => Set<CandidateSkill>();
    public DbSet<JobSkill> JobSkills => Set<JobSkill>();
    public DbSet<SavedJob> SavedJobs => Set<SavedJob>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // =========================
        // ApplicationUser
        // =========================

        builder.Entity<ApplicationUser>()
            .Property(u => u.FullName)
            .HasMaxLength(150)
            .IsRequired();

        // =========================
        // Company - ApplicationUser (One to One)
        // =========================

        builder.Entity<Company>()
            .HasOne(c => c.User)
            .WithOne(u => u.Company)
            .HasForeignKey<Company>(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Company>()
            .HasIndex(c => c.UserId)
            .IsUnique();

        builder.Entity<Company>()
            .Property(c => c.CompanyName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Entity<Company>()
            .Property(c => c.Description)
            .HasMaxLength(3000)
            .IsRequired();

        builder.Entity<Company>()
            .Property(c => c.Website)
            .HasMaxLength(2048);

        builder.Entity<Company>()
            .Property(c => c.Location)
            .HasMaxLength(200);

        builder.Entity<Company>()
            .Property(c => c.LogoUrl)
            .HasMaxLength(2048);

        // =========================
        // CandidateProfile - ApplicationUser (One to One)
        // =========================

        builder.Entity<CandidateProfile>()
            .HasOne(c => c.User)
            .WithOne(u => u.CandidateProfile)
            .HasForeignKey<CandidateProfile>(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<CandidateProfile>()
            .HasIndex(c => c.UserId)
            .IsUnique();

        builder.Entity<CandidateProfile>()
            .Property(c => c.Headline)
            .HasMaxLength(200);

        builder.Entity<CandidateProfile>()
            .Property(c => c.Summary)
            .HasMaxLength(3000);

        builder.Entity<CandidateProfile>()
            .Property(c => c.ResumeUrl)
            .HasMaxLength(2048);

        builder.Entity<CandidateProfile>()
            .Property(c => c.PhoneNumber)
            .HasMaxLength(20);

        builder.Entity<CandidateProfile>()
            .Property(c => c.Location)
            .HasMaxLength(200);

        builder.Entity<CandidateProfile>()
            .Property(c => c.Education)
            .HasMaxLength(300);

        builder.Entity<CandidateProfile>()
            .Property(c => c.LinkedInUrl)
            .HasMaxLength(2048);

        // =========================
        // Category
        // =========================

        builder.Entity<Category>()
            .Property(c => c.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Entity<Category>()
            .Property(c => c.Description)
            .HasMaxLength(500);

        builder.Entity<Category>()
            .HasIndex(c => c.Name)
            .IsUnique();

        // =========================
        // Skill
        // =========================

        builder.Entity<Skill>()
            .Property(s => s.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Entity<Skill>()
            .Property(s => s.Description)
            .HasMaxLength(500);

        builder.Entity<Skill>()
            .HasIndex(s => s.Name)
            .IsUnique();

        // =========================
        // Company - Jobs (One to Many)
        // =========================

        builder.Entity<Job>()
            .HasOne(j => j.Company)
            .WithMany(c => c.Jobs)
            .HasForeignKey(j => j.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        // =========================
        // Category - Jobs (One to Many)
        // =========================

        builder.Entity<Job>()
            .HasOne(j => j.Category)
            .WithMany(c => c.Jobs)
            .HasForeignKey(j => j.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // =========================
        // Job Properties
        // =========================

        builder.Entity<Job>()
            .Property(j => j.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Entity<Job>()
            .Property(j => j.Location)
            .HasMaxLength(200)
            .IsRequired();

        builder.Entity<Job>()
            .Property(j => j.Currency)
            .HasMaxLength(3)
            .IsRequired();

        builder.Entity<Job>()
            .Property(j => j.MinSalary)
            .HasPrecision(18, 2);

        builder.Entity<Job>()
            .Property(j => j.MaxSalary)
            .HasPrecision(18, 2);

        // Job indexes
        builder.Entity<Job>()
            .HasIndex(j => j.Status);

        builder.Entity<Job>()
            .HasIndex(j => j.CategoryId);

        builder.Entity<Job>()
            .HasIndex(j => j.CreatedAt);

        builder.Entity<Job>()
            .HasIndex(j => j.CompanyId);

        // Job check constraints
        builder.Entity<Job>()
            .ToTable(table =>
            {
                table.HasCheckConstraint(
                    "CK_Jobs_MinSalary_NonNegative",
                    "[MinSalary] IS NULL OR [MinSalary] >= 0");

                table.HasCheckConstraint(
                    "CK_Jobs_MaxSalary_NonNegative",
                    "[MaxSalary] IS NULL OR [MaxSalary] >= 0");

                table.HasCheckConstraint(
                    "CK_Jobs_SalaryRange",
                    "[MinSalary] IS NULL OR [MaxSalary] IS NULL OR [MinSalary] <= [MaxSalary]");
            });

        // =========================
        // Job - Applications
        // =========================

        builder.Entity<Application>()
            .HasOne(a => a.Job)
            .WithMany(j => j.Applications)
            .HasForeignKey(a => a.JobId)
            .OnDelete(DeleteBehavior.Restrict);

        // =========================
        // Candidate - Applications
        // =========================

        builder.Entity<Application>()
            .HasOne(a => a.Candidate)
            .WithMany(c => c.Applications)
            .HasForeignKey(a => a.CandidateId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Application>()
            .Property(a => a.CoverLetter)
            .HasMaxLength(5000)
            .IsRequired();

        builder.Entity<Application>()
            .Property(a => a.ResumeUrl)
            .HasMaxLength(2048);

        builder.Entity<Application>()
            .Property(a => a.RecruiterNotes)
            .HasMaxLength(2000);

        // Prevent duplicate applications
        builder.Entity<Application>()
            .HasIndex(a => new { a.JobId, a.CandidateId })
            .IsUnique();

        // =========================
        // SavedJob (Many to Many)
        // =========================

        builder.Entity<SavedJob>()
            .HasKey(s => new
            {
                s.CandidateProfileId,
                s.JobId
            });

        builder.Entity<SavedJob>()
            .HasOne(s => s.CandidateProfile)
            .WithMany(c => c.SavedJobs)
            .HasForeignKey(s => s.CandidateProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<SavedJob>()
            .HasOne(s => s.Job)
            .WithMany(j => j.SavedJobs)
            .HasForeignKey(s => s.JobId)
            .OnDelete(DeleteBehavior.Cascade);

        // =========================
        // CandidateSkill
        // =========================

        builder.Entity<CandidateSkill>()
            .HasKey(cs => new
            {
                cs.CandidateProfileId,
                cs.SkillId
            });

        builder.Entity<CandidateSkill>()
            .HasOne(cs => cs.CandidateProfile)
            .WithMany(cp => cp.CandidateSkills)
            .HasForeignKey(cs => cs.CandidateProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<CandidateSkill>()
            .HasOne(cs => cs.Skill)
            .WithMany(s => s.CandidateSkills)
            .HasForeignKey(cs => cs.SkillId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<CandidateSkill>()
            .Property(cs => cs.ProficiencyLevel)
            .HasMaxLength(50);

        builder.Entity<CandidateSkill>()
            .ToTable(table =>
            {
                table.HasCheckConstraint(
                    "CK_CandidateSkills_YearsOfExperience",
                    "[YearsOfExperience] IS NULL OR [YearsOfExperience] >= 0");
            });

        // =========================
        // JobSkill
        // =========================

        builder.Entity<JobSkill>()
            .HasKey(js => new
            {
                js.JobId,
                js.SkillId
            });

        builder.Entity<JobSkill>()
            .HasOne(js => js.Job)
            .WithMany(j => j.JobSkills)
            .HasForeignKey(js => js.JobId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<JobSkill>()
            .HasOne(js => js.Skill)
            .WithMany(s => s.JobSkills)
            .HasForeignKey(js => js.SkillId)
            .OnDelete(DeleteBehavior.Cascade);

        // =========================
        // CandidateProfile Check Constraints
        // =========================

        builder.Entity<CandidateProfile>()
            .ToTable(table =>
            {
                table.HasCheckConstraint(
                    "CK_CandidateProfiles_YearsOfExperience",
                    "[YearsOfExperience] IS NULL OR [YearsOfExperience] >= 0");
            });
    }
}
