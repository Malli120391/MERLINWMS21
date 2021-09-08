using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPIntegration.BusinessObjects
{
    public class InvSalesOrderInfo
    {
        public InvCustomer customer { get; set; }
        public IList<InvInvoice> custInvoices { get; set; }
        public IList<InvCustomerPO> CustPO { get; set; }
        public string SODate { get; set; }
        public InvDeliveryInfo deliveryInfo { get; set; }
        public IList<AddressBook> partnerAddress { get; set; }
        public string SOID { get; set; }
        public string SOCode { get; set; }
        public int createdBy { get; set; }
        public int requestedBy { get; set; }
        public string SOType { get; set; }
        public string SOStatus { get; set; }
        public string currencyCode { get; set; }
        public string SOName { get; set; }
        public string DocStatus { get; set; }
        public DateTime shippingDateReq { get; set; }
    }

    public class InvCustomerPO
    {
        public string CustPONo { get; set; }
        public int CustPOID { get; set; }
        public DateTime CustPODate { get; set; }
        public IList<LineItem> reqItems { get; set; }
    }

    public class InvDeliveryInfo
    {
        public string DeliveryName { get; set; }
        public string DeliveryStatus { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime SpecifiedDate { get; set; }
        public IList<string> address { get; set; }
    }
}
