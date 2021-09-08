using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21_Endpoint.DTO
{
    public class SupplierCreationDTO
    {
        private string _SupplierCode;
        private string _SupplierName;

        public string SupplierCode { get => _SupplierCode; set => _SupplierCode = value; }
        public string SupplierName { get => _SupplierName; set => _SupplierName = value; }
    }
}