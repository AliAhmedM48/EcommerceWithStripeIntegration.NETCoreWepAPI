using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Repository.Identity;
public class StoreIdentityDbContext : IdentityDbContext<AppUser>
{
    public StoreIdentityDbContext(DbContextOptions<StoreIdentityDbContext> options) : base(options)
    {

    }
}
