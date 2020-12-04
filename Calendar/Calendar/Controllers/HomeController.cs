using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Calendar.Models;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace Calendar.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly dbContext _context;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = new dbContext();
        }

        /* Main Index */
        public IActionResult Index(DateTime? datetime)
        {
            /* If none passed, default to today */
            DateTime date = datetime ?? DateTime.Now;

            /* Configure session id */
            string sessionId = getSessionId();

            /* Load objects for view */
            Bundle bundle = new Bundle()
            {
                /* Take 7 days from week */
                week = BuildWeek(sessionId, date)
            };

            return View(bundle);
        }

        public IActionResult PublishEvent(string eventname, string eventdesc, DateTime eventstart, DateTime eventend)
        {
            /* Configure session id */
            string sessionId = getSessionId();

            List<int> takenHours = _context.getTakenHoursDay(sessionId, eventstart);

            if(takenHours.Contains(eventstart.Hour) | takenHours.Contains(eventend.Hour))
            {
                return RedirectToAction("Index", "Home");
            }

            _context.addEvent(new Event()
            {
                eventName = eventname,
                eventDesc = eventdesc,
                startHour = eventstart,
                endHour = eventend
            }, sessionId);

            return RedirectToAction("Index", new { datetime = eventstart });

        }

        /* Edit Event */
        public IActionResult EditEvent(int thisevent)
        {
            return View();
        }

        /* Add Event */
        public IActionResult AddEvent(DateTime datetime, int hour)
        {

            /* Load objects for view */
            Bundle bundle = new Bundle()
            {
                datetime = datetime.AddHours(hour)
            };


            return View(bundle);
        }


        /* Go back a week */
        public IActionResult BackWeek(DateTime datetime)
        {
            return RedirectToAction("Index", new { datetime = datetime.AddDays(-7) });
        }

        /*Go forward a week  */
        public IActionResult ForwardWeek(DateTime datetime)
        {
            return RedirectToAction("Index", new { datetime = datetime.AddDays(7) });
        }

        /* Build a week object from session and datetime */
        public Week BuildWeek(string sessionId, DateTime dateTime)
        {
            /* Load stored events */
            Week thisWeek = new Week(dateTime);
            List<Event> events = _context.getEvents(sessionId, thisWeek.daysOfWeek[0].thisDate, thisWeek.daysOfWeek[6].thisDate);
            thisWeek.addEvents(events);

            return thisWeek;
        }

        /* Set a session cookie */
        public void SetCookie(string value)
        {
            /* New cookie, expires in a year */
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddYears(1);

            Response.Cookies.Append("sessionId", value, option);
        }

        /* Read session cookie */
        public string ReadCookie()
        {
            return Request.Cookies["sessionId"];
        }

        /* Configures session id */
        public string getSessionId()
        {
            string sessionValue = ReadCookie();

            if(sessionValue == null)
            {
                /* Set new session id if none exist */
                string newGuid = Guid.NewGuid().ToString();
                SetCookie(newGuid);
                return newGuid;
            }
            else
            {
                return sessionValue;
            }
        }

    }
}
