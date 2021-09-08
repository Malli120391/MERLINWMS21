using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventrax_CommonBackGroundService.BL.WebHooks
{
    public class Metadata
    {
        public string path { get; set; }
        public int status_code { get; set; }
        public string status { get; set; }
        public string message { get; set; }
        public DateTime timestamp { get; set; }
    }

    public class Root
    {
        public Metadata metadata { get; set; }
    }
}
