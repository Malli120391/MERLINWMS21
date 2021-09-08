using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPIntegration.Parser
{
    public class InvParserException : Exception 
    {

        public InvParserException(string msg, int ErrorCode) 
        {
            //log write required
            //setters
        }

        public string getMessageInfo() 
        {
            return null;
        }

    }
}
