using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPIntegration.BusinessObjects
{
    public class InvPurchaseOrderInfo
    {
        public InvVendor vendor { get; set; }
        public IList<InvInvoice> vendorInvoices { get; set; }
        public string PODate { get; set; }
        public IList<AddressBook> partnerAddress { get; set; }
        public InvDeliveryInfo deliveryInfo { get; set; }
        public int POID { get; set; }
        public string POCode { get; set; }
        public int createdBy { get; set; }
        public int requestedBy { get; set; }

        public string orderID { get; set; }
        public string vendGroup { get; set; }
        public string PurchName { get; set; }
        public string POStatus { get; set; }
        public string serviceName { get; set; }
        public string POType { get; set; }

    }

    public class InvInvoice
    {
        public string invoiceID { get; set; }
        public string invoiceVatNo { get; set; }
        public decimal netAmt { get; set; }
        public decimal grossAmt { get; set; }

        public Vat vatInfo { get; set; }
        public DateTime invoiceDate { get; set; }
        public InvCurrency currency { get; set; }
        public InvInvoiceAddress address { get; set; }
        public IList<LineItem> lineItems { get; set; }
    }

    public class Vat
    {
        public decimal vatAmt { get; set; }
        public decimal percent { get; set; }
        public string vatOn { get; set; }
        public string vatCode { get; set; }
    }

    public class LineItem
    {
        public decimal LineNo { get; set; }
        public InvItem item { get; set; }
        public decimal qtyOrdered { get; set; }
        public int UOMID { get; set; }
        public string UOMCode { get; set; }
        public decimal LinePrice { get; set; }
        public decimal unitPrice { get; set; }
        public IList<InvItemDim> itemDim { get; set; }
        public decimal packingQty { get; set; }
        public string transID { get; set; }
        public string SalesLineID { get; set; }
        public decimal salesQty { get; set; }
        public string currencyCode { get; set; }
        public string status { get; set; }
        public string purchLineID { get; set; }
        public decimal purchQty { get; set; }

    }

    public class InvInvoiceAddress
    {
        public int countryID { get; set; }
        public string countryCode { get; set; }
        public string cityCode { get; set; }
        public IList<string> address { get; set; }
        public string zipCode { get; set; }
    }


    public class InvItemDim
    {
        public int MSPID { get; set; }
        public string MSPCode { get; set; }
        public string MSPType { get; set; }
        public string Value { get; set; }
        public decimal quantity { get; set; }
        public IList<string> otherInfo { get; set; }
    }

}
