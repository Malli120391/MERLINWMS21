using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21_Library.INOUT_MSG
{
    /* this enum represents how many types of inbound messages are accpeted */
    public enum MSGType
    {
        XML_MSG,
        JSON_MSG,
        PROTOCAL_BUFFER,
        OBJECT_MSG,
        TINY_OBJECT,
        CUSTOM_STR,
        EDI
    }
}
