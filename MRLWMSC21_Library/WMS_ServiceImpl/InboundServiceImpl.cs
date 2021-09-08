using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21_Library.WMS_ServiceImpl
{
    class InboundServiceImpl:WMSService
    {
        public object Process(string FUNC_EXEC_CODE, object input)
        {
            switch (FUNC_EXEC_CODE.ToString().Trim())
            {
                case "INB_C":
                case "INB_U":
                case "INB_D":
                case "INB_S":
                    break;
                case "INB_LIST":
                    break;
                default:
                    throw new NotImplementedException("[" + FUNC_EXEC_CODE + "] this functionality not implemented under InboundServiceImpl");
            }

            throw new NotImplementedException();
        }

        public string GetReceiveStatus(string FUNC_CALL_TYPE, string MSG, int INPUT_TYPE)
        {
            return null;
        }
        
    }


}
