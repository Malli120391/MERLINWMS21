using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21.TPL.Invoice
{
    public class InvoiceInfo
    {
        public TenantInfo tenantData { get; set; }
        public SummaryPage summary { get; set; }
        public IDictionary<string, TransactionCharge> transactionAccessorial { get; set; }
        public IDictionary<int,IDictionary<int, StorageCharge>> storeCharges { get; set; }
        public IList<CalculatedStorageCharge> calculatedStoreCharges { get; set; }
        public IDictionary<int, InvoiceRate> InvoiceRates { get; set; }

        public int TenantID { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string invoiceID { get; set; }
        public string GeneratedDate { get; set; }
        
        
    }

    
    
    public class InvoiceRate 
    {
        public int rateID { get; set; }
        public decimal price { get; set; }
        public int currencyID { get; set; }
        public decimal discount { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string currencyCode { get; set; }
        public string rateName { get; set; }
        public int activityID { get; set; }
        public decimal unitPrice { get; set; }
        public decimal QTY { get; set; }
        public TaxationInfo taxInfo { get; set; }

   }

    public class TaxationInfo 
    {
        public string stateCode { get; set; }
        public decimal percentage { get; set; }
        public decimal price  { get; set; }
        public string taxCode { get; set; }
        public int taxationID { get; set; }
    }

}