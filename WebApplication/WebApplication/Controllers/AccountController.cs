using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Models;
using WebApplication.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace WebApplication.Controllers
{
    public class AccountController : Controller
    {
        readonly UserManager<User> userManager;
        readonly SignInManager<User> signInManager;

        public AccountController (UserManager<User> usrManager, SignInManager<User> sgnInManager)
        {
            userManager = usrManager;
            signInManager = sgnInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    UserName = model.Login,
                    Email = model.EMail,
                    DOB = model.DOB
                };

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
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
    }
}
