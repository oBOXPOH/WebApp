using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class SectionsController : Controller
    {
        ApplicationContext applicationContext;

        public SectionsController(ApplicationContext appContext)
        {
            applicationContext = appContext;
        }

        public IActionResult Index()
        {
            return View(applicationContext.FirstLevelSections.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateFirstLevelSectionViewModel model)
        {
            if (ModelState.IsValid)
            {

            }

            return View(model);
        }
    }
}
