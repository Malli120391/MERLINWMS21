using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Inventrax_CommonBackGroundService.BL.ExcellGenerator
{

    public interface IExcellGenerator
    {
        void Create(DataTable dt,string tenantName,string warehouseName,string fileName);
    }

}
