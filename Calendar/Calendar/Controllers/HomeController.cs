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

    /*
     * Class HomeController:
     * Contains all controllers and logic for website
     */

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
        public IActionResult Index(DateTime? datetime, int? message)
        {
            /* If none passed, default to today */
            DateTime date = datetime ?? DateTime.Now;

            /* Configure session id */
            string sessionId = getSessionId();

            /* Load objects for view */
            Bundle bundle = new Bundle()
            {
                /* Take 7 days from week */
                week = BuildWeek(sessionId, date),
                message = message ?? 0
            };

            return View(bundle);
        }

        /* Add Event */
        public IActionResult AddEvent(DateTime datetime, int hour)
        {

            /* Load objects for view */
            Bundle bundle = new Bundle()
            {
                datetime = datetime.AddHours(hour),
                hour = hour
            };


            return View(bundle);
        }

        /* Publish Event */
        public IActionResult PublishEvent(string eventname, string eventdesc, DateTime eventstart, int eventhrs)
        {
            /* Configure session id */
            string sessionId = getSessionId();

            List<int> takenHours = _context.getTakenHoursDay(sessionId, eventstart);
            DateTime eventend = eventstart.AddHours(eventhrs-1);

            /* Check if it overlaps another event */
            if(takenHours.Contains(eventstart.Hour) | takenHours.Contains(eventend.Hour))
            {
                return RedirectToAction("Index", new { message = 1 });
            }

            /* Check if it goes into the next day */
            if(eventstart.Hour+eventhrs > 24)
            {
                return RedirectToAction("Index", new { message = 2 });
            }

            /* If it passes all checks, publish event */
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
        public IActionResult EditEvent(int eventId)
        {

            /* Load objects for view */
            Bundle bundle = new Bundle()
            {
                someEvent = _context.getEvent(eventId)
            };

            return View(bundle);
        }

        /* Update Event */
        public IActionResult UpdateEvent(int eventid, string eventname, string eventdesc)
        {

            Event thisevent = _context.getEvent(eventid);
            thisevent.eventName = eventname;
            thisevent.eventDesc = eventdesc;

            _context.updateEvent(thisevent);

            return RedirectToAction("Index", new { datetime = thisevent.startHour });
        }

        /* Remove Event */
        public IActionResult RemoveEvent(int eventid)
        {
            _context.removeEvent(eventid);

            return RedirectToAction("Index", "Home");
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
