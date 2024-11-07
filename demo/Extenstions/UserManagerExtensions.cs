using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace demo.Extenstions;

public static class UserManagerExtensions
{
    public static async Task<AppUser?> FindByEmailWithAddressAsync(this UserManager<AppUser> userManager, ClaimsPrincipal User)
    {
        if (userManager is null) throw new ArgumentNullException(nameof(userManager));

        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        if (userEmail == null) throw new KeyNotFoundException("User email claim not found.");

        var user = await userManager.Users.Include(p => p.Address).FirstOrDefaultAsync(u => u.Email == userEmail);
        if (user is null) return null;

        return user;
    }
}
