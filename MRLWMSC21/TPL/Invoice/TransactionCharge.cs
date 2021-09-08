using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21.TPL.Invoice
{
    public class TransactionCharge
    {
        public int transactionID { get; set; }
        public int transactionType { get; set; }
        public string transactionRefNumber { get; set; }

        public string date { get; set; }
        public string transactionName { get; set; }
        public IList<AccessorialCharge> charges { get; set; }
    }

    public class AccessorialCharge 
    {
        
        public int UoMID { get; set; }
        public string UOM { get; set; }
        public decimal unitCost { get; set; }
        public string activityName { get; set; }
        public int activityID { get; set; }
        public decimal disc { get; set; }
        public decimal quantity { get; set; }
        public string currency { get; set; }
        public int currencyID { get; set; }
        public int rateID { get; set; }
        public decimal priceWithoutDisc { get; set; }

    }

}