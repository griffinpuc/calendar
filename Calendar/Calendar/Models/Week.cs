using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calendar.Models
{
    public class Week
    {
        /* Every week has days */
        public List<Day> daysOfWeek = new List<Day>();

        /* Construct a week */
        public Week(DateTime dt)
        {
            /* From passed datetime, get all dates in that week, starting with the first */
            foreach(DateTime date in getWeekDates(dt))
            {
                Day newDay = new Day() { thisDate = date };
                daysOfWeek.Add(newDay);
            }
        }

        /* Add events to week */
        public void addEvents(List<Event> weeksEvents)
        {
            /* From passed datetime, get all dates in that week, starting with the first */
            foreach (Day day in daysOfWeek)
            {
                DateTime dayStart = day.thisDate;
                DateTime dayEnd = day.thisDate.AddHours(24);

                /* For every hour of day */
                while (dayStart != dayEnd)
                {

                    /* Linq statement, if any events have an event on this hour */
                    if (weeksEvents.Any(i => i.startHour.Hour == dayStart.Hour && i.startHour.DayOfWeek == day.thisDate.DayOfWeek))
                    {
                        /* If there are, add all of them to this days events */
                        var events = weeksEvents.Where(i => i.startHour.Hour == dayStart.Hour && i.startHour.DayOfWeek == day.thisDate.DayOfWeek);
                        day.events.Add(events.First());

                    }
                    else if (weeksEvents.Any(i => i.endHour.Hour == dayStart.Hour && i.startHour.DayOfWeek == day.thisDate.DayOfWeek))
                    {
                        var events = weeksEvents.Where(i => i.endHour.Hour == dayStart.Hour && i.startHour.DayOfWeek == day.thisDate.DayOfWeek);
                        day.events.Add(events.First());

                    }
                    else
                    {
                        day.events.Add(null);
                    }

                    dayStart = dayStart.AddHours(1);
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
            DateTime day = getStartOfWeek(dt);

            for(int i = 0; i < 7; i++)
            {
                retval[i] = new DateTime(day.Year, day.Month, day.Day, 0, 0, 0);
                day = day.AddDays(1);
            }

            return retval;
        }

    }

    public class Day
    {
        public DateTime thisDate;
        public List<Event> events = new List<Event>();
    }
}
