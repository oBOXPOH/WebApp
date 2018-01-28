using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Models;
using WebApplication.ViewModels;

namespace WebApplication.Controllers
{
    public class UsersController : Controller
    {
        UserManager<User> userManager;

        public UsersController(UserManager<User> usrManager)
        {
            userManager = usrManager;
        }

        public IActionResult Index()
        {
            return View(userManager.Users.ToList());
        }
    }
}
