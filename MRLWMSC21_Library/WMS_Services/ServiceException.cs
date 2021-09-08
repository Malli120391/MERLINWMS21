using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21_Library.WMS_Services
{
    public class ServiceException : Exception
    {
        private int ErrorCode;
        private string message;

        public ServiceException(int code, string msg)
        {
            ErrorCode = code;
            message = msg;
        }

        public string getSuggetedMessage()
        {
            if (ResourceSuggestion.ErrorCode.ContainsKey(ErrorCode))
            {
                return ResourceSuggestion.ErrorCode[ErrorCode];
            }
            else
            {
                return "there is no resource suggestion for this error #code";
            }
        }

        public int getErrorNo() 
        {
            return ErrorCode;
        }

        public string getStackTraceInfo()
        {
            return message;
        }

        public string getMsg() 
        {
            return message;
        }
    }
}
