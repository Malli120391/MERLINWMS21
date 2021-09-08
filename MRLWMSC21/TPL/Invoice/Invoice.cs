using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21.TPL.Invoice
{
    interface Invoice
    {


        bool saveInvoice(int tenantID, DateTime fromDate, DateTime toDate);
        bool cancelInvoice(int tenantID, DateTime fromDate, DateTime toDate);
        bool getInvoice(int tenantID, DateTime fromDate, DateTime toDate);

    }


}
