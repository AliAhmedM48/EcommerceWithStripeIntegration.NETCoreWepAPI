using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Repository.Identity;
public static class StoreIdentityDbContextSeeder
{
    public async static Task SeedAppUserAsync(UserManager<AppUser> _userManager)
    {
        if (_userManager.Users.Count() == 0)
        {
            var user = new AppUser()
            {
                Email = "AliAhmed@gmail.com",
                DisplayName = "Ali Ahmed",
                UserName = "AliAhmed",
                PhoneNumber = "01022033579",
                Address = new Address()
                {
                    FName = "Ali",
                    LName = "Ahmed",
                    City = "Suez",
                    Country = "Egypt",
                    Street = "Elshabab"
                }

            };

            //await _userManager.CreateAsync(user, password: "P@ssW0rd");
            await _userManager.CreateAsync(user, password: "Ali@123");
        }


    }
}
