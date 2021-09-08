using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21_Library.WMS_ServiceObjects
{
    public class ResultObject
    {
        public string ID {get;set;}
        public object Result { get; set; }
        public bool FUNC_EXEC_STATUS { get; set; }
    }

}
