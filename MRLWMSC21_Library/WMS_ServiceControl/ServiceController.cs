using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21_Library.WMS_ServiceControl
{
    interface ServiceController
    {
        Object RequestDispatcher(string FUNC_EXEC_CODE,Object input);
    }
}
