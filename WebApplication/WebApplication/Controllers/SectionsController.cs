using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.Models;
using WebApplication.ViewModels.Sections;

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
                FirstLevelSection section = await applicationContext.FirstLevelSections.FirstOrDefaultAsync(p => p.Title == model.Title);

                if (section == null)
                {
                    section = new FirstLevelSection { Title = model.Title, PublishDate = DateTime.Now, Slug = model.Title.GenerateSlug() };

                    await applicationContext.FirstLevelSections.AddAsync(section);

                    Event evnt = new Event
                    {
                        Date = DateTime.Now,
                        Description = $"«{User.Identity.Name}» создал раздел с названием «{section.Title}»"
                    };

                    await applicationContext.Events.AddAsync(evnt);

                    await applicationContext.SaveChangesAsync();

                    return RedirectToAction("Index", "Sections");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Раздел с таким названием уже существует");
                }

            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id, string slug)
        {
            FirstLevelSection section = await applicationContext.FirstLevelSections.FirstOrDefaultAsync(p => p.Id == id);

            if (section != null)
            {
                List<SecondLevelSection> subSections = (from item in applicationContext.SecondLevelSections.ToList()
                                                        where item.Id == id
                                                        select item).ToList();

                return View(new DetailsFirstLevelSectionViewModel { Title = section.Title, EditDate = section.EditDate != null ? section.EditDate : section.PublishDate, SecondLevelSections = subSections });
            }

            return NotFound();
        }
    }
}
