
using Microsoft.AspNetCore.Identity;

namespace JobBoard.DAL.Data.Seed;

public static class RoleSeeder
{
    public static async Task SeedRolesAsync(
        RoleManager<IdentityRole<int>> roleManager)
    {
        string[] roles = { "Admin", "Company", "Candidate" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var result = await roleManager.CreateAsync(
                    new IdentityRole<int>(role));

                if (!result.Succeeded)
                {
                    throw new Exception(
                        $"Failed to create role {role}: " +
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
