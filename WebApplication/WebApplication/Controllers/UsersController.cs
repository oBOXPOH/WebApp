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
            IndexUserViewModel model = new IndexUserViewModel { Users = userManager.Users.ToList() };

            model.Roles = new List<string>();
            model.AmountOfRoles = new List<int>();

            foreach (var item in roleManager.Roles.ToList())
            {
                model.Roles.Add(item.Name);
                model.AmountOfRoles.Add(0);
            }

            foreach (var user in model.Users)
            {
                for (int i = 0; i < model.Roles.Count; i++)
                {
                    if (model.Roles[i] == user.UserRole)
                    {
                        model.AmountOfRoles[i]++;
                        break;
                    }
                }
            }

            return View(model);
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
                    await userManager.AddToRoleAsync(await userManager.FindByEmailAsync(user.Email), user.UserRole);

                    Event evnt = new Event
                    {
                        Date = DateTime.Now,
                        Description = $"«{User.Identity.Name}» создал пользователя «{user.UserName}» с правами «{user.UserRole}»"
                    };

                    await applicationContext.Events.AddAsync(evnt);
                    await applicationContext.SaveChangesAsync();

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

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            User user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                List<SelectListItem> roles = new List<SelectListItem>();

                bool isSelected;

                foreach (var item in roleManager.Roles.ToList())
                {
                    isSelected = false;
                    if (item.Name == user.UserRole)
                        isSelected = true;

                    roles.Add(new SelectListItem { Value = item.Name, Text = item.Name, Selected = isSelected });
                }

                ViewBag.Roles = roles;

                return View(new EditUserViewModel { Id = user.Id, Login = user.UserName, EMail = user.Email, DOB = user.DOB, UserRole = user.UserRole });
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await userManager.FindByIdAsync(model.Id);

                if (user != null)
                {
                    user.UserName = model.Login;
                    user.Email = model.EMail;
                    user.DOB = model.DOB;
                    user.UserRole = model.UserRole;

                    var result = await userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        string role = (await userManager.GetRolesAsync(user) as List<string>).ElementAt(0);
                        await userManager.RemoveFromRoleAsync(user, role);
                        await userManager.AddToRoleAsync(user, user.UserRole);

                        Event evnt = new Event
                        {
                            Date = DateTime.Now,
                            Description = $"«{User.Identity.Name}» редактировал пользователя «{user.UserName}» с правами «{user.UserRole}»"
                        };

                        await applicationContext.Events.AddAsync(evnt);
                        await applicationContext.SaveChangesAsync();

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
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(string id)
        {
            User user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                Event evnt = new Event
                {
                    Date = DateTime.Now,
                    Description = $"«{User.Identity.Name}» удалил пользователя «{user.UserName}» с правами «{user.UserRole}»"
                };

                await applicationContext.Events.AddAsync(evnt);
                await applicationContext.SaveChangesAsync();

                await userManager.DeleteAsync(user);
            }

            return RedirectToAction("Index");
        }
    }
}