using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPIntegration.BusinessObjects
{
    public class InvVendor
    {

        public IList<AddressBook> postalAddress { get; set; }
        public IList<AddressBook> contactAddress { get; set; }
        
        public IList<string> phone { get; set; }
        public IList<string> email { get; set; }
        public IList<string> mobile { get; set; }
        
        public IList<string> contactNames { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public string mname { get; set; }

        public string vendorCode { get; set; }
        public int vendorID { get; set; }
        public int vendorTypeID { get; set; }
        public int TenantID { get; set; }
        public string countryCode { get; set; }
        public int countryID { get; set; }
        public string fax { get; set; }
        public int langID { get; set; }
        public string vatCode { get; set; }
        public string organization { get; set; }
        public string partyNo { get; set; }
        public string DBA { get; set; }
        public bool isBlocked { get; set; }
        public string vendorGroup { get; set; }

        public OrganizationName organizationInfo { get; set; }
        public bool isOrganization { get; set; }
        public IList<InvBankData> bankData { get; set; }
        public IList<InvItem> itemInfo { get; set; }
        public InvCurrency currency { get; set; }

        public string PCP { get; set; }
        public string PCPEmail { get; set; }
        public string PCPTitle { get; set; }
        public string PCPContactNo { get; set; }
        public int createdBy { get; set; }
        public int requestedBy { get; set; }
        public string createdOn { get; set; }
        public string createdUser { get; set; }
        public IList<string> searchText { get; set; }
    }

    public class InvBankData 
    {
        public string BankName { get; set; }
        public string BankCode { get; set; }
        public string IBAN { get; set; }
        public string SwiftIBAN { get; set; }
        public string BIC { get; set; }
        public string HolderName { get; set; }
        public string AccountNumber { get; set; }
        public int BankCountryID { get; set; }
        public AddressBook address { get; set; }

    }

    public class InvCurrency
    {
        public string currencyName { get; set; }
        public int currencyID { get; set; }
        public string currencyCode { get; set; }
        public string pdfSymbol { get; set; }
    }


}
