using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class EventsController : Controller
    {
        ApplicationContext applicationContext;

        public EventsController(ApplicationContext appContext)
        {
            applicationContext = appContext;
        }

        public IActionResult Index()
        {
            return View(applicationContext.Events.ToList().OrderByDescending(p => p.Date));
        }
    }
}
