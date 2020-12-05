using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calendar.Models
{

    /*
     * Class Bundle:
     * Contains all parameters that may be passed to the views
     */

    public class Bundle
    {

        public Week week { get; set; }
        public DateTime datetime { get; set; }
        public Event someEvent { get; set; }
        public int hour { get; set; }
        public int message { get; set; }

    }
}
