using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21.TPL.Invoice
{
    public class SummaryPage
    {
        public IList<TPLRate> rates { get; set; }
        public ServiceCharge serviceCharge { get; set; }
        public Tax totalTax { get; set; }
        public decimal totalRatesPriceTax { get; set; }

        public decimal price;
        public string currency;
        public int currencyID;
        public string rateCurrency;
        public int rateCurrencyID;
        public decimal _totalRatesPrice;

    }


    public class TPLRate 
    {

        public string rateName { get; set; }
        public string UoMID { get; set; }
        public decimal quantity { get; set; }
        public decimal price { get; set; }
        public decimal pricePerUnit { get; set; }
        public string currency { get; set; }
        public string rateID { get; set; }

    }

    public class ServiceCharge 
    {
        public decimal percentage { get; set; }
        public decimal netAmount { get; set; }
        public string country { get; set; }
    }

    public class Tax 
    {
        public decimal price { get; set; }
        public string currency { get; set; }
        public decimal convertedPrice { get; set; }
    }
}