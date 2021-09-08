using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21.TPL.Invoice
{
    public class TenantInfo
    {
        public int TenantID { get; set; }
        public int billTypeID { get; set; }

        public int currencyID { get; set; }
        public string currencyCode { get; set; }

        public int spaceChargeID { get; set; }
        public int spaceUtilizationID { get; set; }

        public string primaryEmail { get; set; }
        public string tenantName { get; set; }
        public string companyName { get; set; }
        public string InvoiceID { get; set; }
        public int previousInvoice { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public DateTime fromDateTime { get; set; }
        public DateTime toDateTime { get; set; }

        public TenantAddress address { get; set; }
        public BankData bankData { get; set; }
        public InvoiceMail mailInfo { get; set; }
    }

    public class TenantAddress
    {

        public string countryName { get; set; }
        public int countryID { get; set; }
        public string city { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string email { get; set; }
        public int addressTypeID { get; set; }
        public string zip { get; set; }

    }

    public class BankData
    {

        public string accountNo { get; set; }
        public string IBAN { get; set; }
        public string BIC { get; set; }
        public string bankName { get; set; }
        public string holderName { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string city { get; set; }
        public string pinCode { get; set; }
    }

    public class InvoiceMail
    {

        public string FromAddress { get; set; }
        public IList<string> ToAddress { get; set; }

        public string PdfLocation { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public string CCAddress { get; set; }

    }


}