using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Core.Services.Contracts;

public interface ITokenService
{
    Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager);
}
