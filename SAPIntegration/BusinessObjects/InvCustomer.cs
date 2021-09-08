using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPIntegration.BusinessObjects
{
    public class InvCustomer
    {

        public IList<AddressBook> postalAddress { get; set; }
        public IList<AddressBook> contactAddress { get; set; }

        public IList<string> phone { get; set; }
        public IList<string> email { get; set; }
        public IList<string> mobile { get; set; }

        public OrganizationName organizationInfo { get; set; }
        public bool isOrganization { get; set; }
        public int custID { get; set; }
        public string custCode { get; set; }

        public string fname { get; set; }
        public string lname { get; set; }
        public string mname { get; set; }
        
        public int customerTypeID { get; set; }
        public int TenantID { get; set; }
        public string countryCode { get; set; }
        public int countryID { get; set; }
        public string city { get; set; }

        public string fax { get; set; }
        public int langID { get; set; }
        public string vatCode { get; set; }
        public string custGroup { get; set; }
        public string langCode { get; set; }
        public string partyNumber { get; set; }
        public string DBAName { get; set; }
        public bool isBlocked { get; set; }

        public IList<InvBankData> bankData { get; set; }
        public InvCurrency currency { get; set; }
        public IList<InvSalesOrderInfo> salesOrders { get; set; }

        public int createdBy { get; set; }
        public int requestedBy { get; set; }
        
    }

    public class OrganizationName 
    {
        public string organizationName { get; set; }
        public string nameAlias { get; set; }
        public string organizationNumber { get; set; }
        public string langCode { get; set; }

    }

    public class AddressBook 
    {
        public string city { get; set; }
        public string street1 { get; set; }
        public string street2 { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        public int addressType { get; set; }
        public string adressLine1 { get; set; }
        public string adressLine2 { get; set; }

    }
}
