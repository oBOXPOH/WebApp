﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Models;
using WebApplication.ViewModels;

namespace WebApplication.Controllers
{
    public class ArticlesController : Controller
    {
        ApplicationContext applicationContext;
        UserManager<User> userManager;

        public ArticlesController(ApplicationContext appContext, UserManager<User> usrManager)
        {
            applicationContext = appContext;
            userManager = usrManager;
        }

        public IActionResult Index()
        {
            List<Article> articles = applicationContext.Articles.ToList();

            if (articles.Count != 0)
                return View(articles.OrderByDescending(p => p.PostDate).ToList());
            else
                return View(articles);

        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateArticleViewModel model)
        {
            if (ModelState.IsValid)
            {
                Article article = new Article { UserId = (await userManager.FindByNameAsync(User.Identity.Name)).Id, Title = model.Title, ShortDescription = model.ShortDescription,
                    FullDescription = model.FullDescription, PostDate = DateTime.Now };

                await applicationContext.Articles.AddAsync(article);

                await applicationContext.SaveChangesAsync();

                Event evnt = new Event
                {
                    Date = DateTime.Now,
                    Description = $"«{User.Identity.Name}» создал статью с названием «{article.Title}»"
                };

                await applicationContext.Events.AddAsync(evnt);

                return RedirectToAction("Index", "Articles");
            }

            return View();
        }
    }
}