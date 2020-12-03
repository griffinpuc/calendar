using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calendar.Models
{
    public class Week
    {
        /* Every week has days */
        public List<Day> daysOfWeek;

        /* Construct a week */
        public Week(DateTime dt, List<Event> weeksEvents)
        {
            /* From passed datetime, get all dates in that week, starting with the first */
            foreach(DateTime date in getWeekDates(dt))
            {
                Day newDay = new Day() { thisDate = date };

                /* Linq statement, if any events have an event on this day of the week */
                if(weeksEvents.Any(i => i.startHour.DayOfWeek == date.DayOfWeek))
                {
                    /* If there are, add all of them to this days events */
                    var events = weeksEvents.Where(i => i.startHour.DayOfWeek == date.DayOfWeek);
                    foreach(Event thisevent in events)
                    {
                        newDay.events.Add(thisevent);
                    }
                    
                }
            }
        }

        /* Returns starting date of the week given a date */
        public DateTime getStartOfWeek(DateTime dt)
        {
            return dt.AddDays(((int)(dt.DayOfWeek) * -1) + 1);
        }

        /* Returns all weekdates in a week */
        public DateTime[] getWeekDates(DateTime dt)
        {
            DateTime[] retval = new DateTime[7];
            DateTime startOfWeek = getStartOfWeek(dt);

            for(int i = 0; i < 7; i++)
            {
                retval[i] = startOfWeek;
                startOfWeek.AddDays(1);
            }

            return retval;
        }

    }

    public class Day
    {
        public DateTime thisDate;
        public List<Event> events;
    }
}
