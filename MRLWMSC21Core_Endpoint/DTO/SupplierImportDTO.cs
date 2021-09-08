using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21_Endpoint.DTO
{
    public class SupplierImportDTO
    {
      
        private string _SupplierName;
        private string _SupplierCode;
        private string _Phone1;
        private string _Address1;
        private string _CountryName;
        private string _EmailAddress;
        private string _PCPName;
        private string _PCPTitle;
        private string _PCPContactNumber;
        private string _PCPEmail;
        private string _SupplierStatus;

       
        public string SupplierName { get => _SupplierName; set => _SupplierName = value; }
        public string SupplierCode { get => _SupplierCode; set => _SupplierCode = value; }
        public string Phone1 { get => _Phone1; set => _Phone1 = value; }
        public string Address1 { get => _Address1; set => _Address1 = value; }
        public string CountryName { get => _CountryName; set => _CountryName = value; }
        public string EmailAddress { get => _EmailAddress; set => _EmailAddress = value; }
        public string PCPName { get => _PCPName; set => _PCPName = value; }
        public string PCPTitle { get => _PCPTitle; set => _PCPTitle = value; }
        public string PCPContactNumber { get => _PCPContactNumber; set => _PCPContactNumber = value; }
        public string PCPEmail { get => _PCPEmail; set => _PCPEmail = value; }
        public string SupplierStatus { get => _SupplierStatus; set => _SupplierStatus = value; }

    }
}