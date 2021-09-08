using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21_Library.WMS_DBCommon
{
    public interface DBServiceController
    {
        Object processDBCall(string DB_CALL_CODE, Object input, int IN_MSG_TYPE);
    }
}
