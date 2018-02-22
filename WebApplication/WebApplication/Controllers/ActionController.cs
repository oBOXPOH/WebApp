using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.Models;
using WebApplication.ViewModels.Sections;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication.Controllers
{
    public class ActionController : Controller
    {
        ApplicationContext applicationContext;

        public ActionController(ApplicationContext appContext)
        {
            applicationContext = appContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Sections/{fstLevelSection}")]
        public async Task<IActionResult> ViewSection(string fstLevelSection)
        {
            FirstLevelSection firstLevelSection = await applicationContext.FirstLevelSections.FirstOrDefaultAsync(p => p.Slug == fstLevelSection);

            if (firstLevelSection != null)
            {
                List<SecondLevelSection> sections = (from item in applicationContext.SecondLevelSections
                                                    where item.FirstLevelTitleId == firstLevelSection.Id
                                                    select item).ToList();

                return View("~/Views/Sections/Details.cshtml", new DetailsFirstLevelSectionViewModel
                {
                    Title = firstLevelSection.Title,
                    EditDate = firstLevelSection.EditDate != null ? firstLevelSection.EditDate : firstLevelSection.PublishDate,
                    SecondLevelSections = sections
                });
            }

            return NotFound();
        }

        [HttpGet]
        public IActionResult Create(ActionType actionType, string parentId = null)
        {
            switch (actionType)
            {
                case ActionType.FirstLevelSection:
                    return View("~/Views/Sections/Create");

                case ActionType.SecondLevelSection:
                    return View("CreateSecondLevelSection");
                default:
                    return NotFound();
            }
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

            return View("CreateFirstLevelSection", model);
        }
    }
}
