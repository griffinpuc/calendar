using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calendar.Models
{

    /*
     * Class Event:
     * This defines what parameters an event may have
     */

    public class Event
    {

        public int eventId { get; set; }
        public string eventName { get; set; }
        public string eventDesc { get; set; }
        public DateTime startHour { get; set; }
        public DateTime endHour { get; set; }
        public int totalHours { get; set; }

    }
}
