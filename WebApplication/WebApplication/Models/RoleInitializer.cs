using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "rc_38@mail.ru";
            string adminPassword = "_veryhardpassword_";

            foreach (var item in roleManager.Roles.ToList())
            {
                await roleManager.DeleteAsync(item);
            }

            if (await roleManager.FindByNameAsync("Администратор") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Администратор"));
            }

            if (await roleManager.FindByNameAsync("Модератор") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Модератор"));
            }

            if (await roleManager.FindByNameAsync("Пользователь") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Пользователь"));
            }

            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User { Email = adminEmail, UserName = adminEmail };

                var result = await userManager.CreateAsync(admin, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Администратор");
                }
            }
        }
    }
}
