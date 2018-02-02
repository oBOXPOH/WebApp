using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication.Models;
using WebApplication.ViewModels;

namespace WebApplication.Controllers
{
    [Authorize(Roles = "Администратор")]
    public class UsersController : Controller
    {
        ApplicationContext applicationContext;
        UserManager<User> userManager;
        RoleManager<IdentityRole> roleManager;

        public UsersController(UserManager<User> usrManager, RoleManager<IdentityRole> rleManager, ApplicationContext appContext)
        {
            applicationContext = appContext;
            userManager = usrManager;
            roleManager = rleManager;
        }

        public IActionResult Index()
        {
            return View(userManager.Users.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            List<SelectListItem> roles = new List<SelectListItem>();

            bool isSelected;

            foreach (var item in roleManager.Roles.ToList())
            {
                isSelected = false;
                if (item.Name == "Пользователь")
                    isSelected = true;

                roles.Add(new SelectListItem { Value = item.Name, Text = item.Name, Selected = isSelected });
            }

            ViewBag.Roles = roles;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { UserName = model.Login, Email = model.EMail, DOB = model.DOB, UserRole = model.UserRole };
                
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    Event evnt = new Event
                    {
                        Date = DateTime.Now,
                        Description = $"{User.Identity.Name} создал {user.UserName} как {user.UserRole}"
                    };

                    await applicationContext.Events.AddAsync(evnt);

                    await userManager.AddToRoleAsync(user, user.UserRole);

                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(string id)
        {
            User user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                await userManager.DeleteAsync(user);

                Event evnt = new Event
                {
                    Date = DateTime.Now,
                    Description = $"{User.Identity.Name} удалил {user.UserName}, который был {user.UserRole}"
                };

                await applicationContext.Events.AddAsync(evnt);
            }

            return RedirectToAction("Index");
        }
    }
}
