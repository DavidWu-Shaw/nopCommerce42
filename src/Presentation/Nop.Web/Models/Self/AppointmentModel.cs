using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Web.Models.Self
{
    public class AppointmentModel
    {
        public string id { get; set; }
        public string text { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string resource { get; set; }
        public TagModel tags { get; set; }
    }

    public class TagModel
    {
        public string status { get; set; }
        public string doctor { get; set; }
    }
}
