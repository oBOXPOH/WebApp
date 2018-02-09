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
            string adminName = "Admin";
            string adminPassword = "k11111";

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

            if (await userManager.FindByNameAsync(adminName) == null)
            {
                User admin = new User { Email = "admin@mail.ru", UserName = adminName, UserRole = "Администратор", EmailConfirmed = true };

                var result = await userManager.CreateAsync(admin, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, admin.UserRole);
                }
            }

            User user;

            for (int i = 0; i < 10; i++)
            {
                user = new User { Email = $"user{i}@mail.ru", UserName = $"User{i}", UserRole = "Пользователь", EmailConfirmed = true };

                var result = await userManager.CreateAsync(user, "k11111");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, user.UserRole);
                }
            }
        }
    }
}
