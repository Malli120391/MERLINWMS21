using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventrax_CommonBackGroundService.BL.PDFGenerator
{
    interface IPDFGenerator
    {
        void Create(DataTable dt, string tenantName, string warehouseName, string fileName);
    }
}
