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
            List<Event> events = applicationContext.Events.ToList();

            if (events.Count != 0)
                return View(events.OrderByDescending(p => p.Date).ToList());
            else
                return View(events);
        }
    }
}
