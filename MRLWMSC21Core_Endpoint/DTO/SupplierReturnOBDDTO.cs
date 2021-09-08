using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21_Endpoint.DTO
{
    public class SupplierReturnOBDDTO
    {

        private string _StartDate;
        private string _EndDate;
        private string _PaginationID;
        private string _PageSize;
        private string _WareHouseCode;

        public string StartDate { get => _StartDate; set => _StartDate = value; }
        public string EndDate { get => _EndDate; set => _EndDate = value; }
        public string PaginationID { get => _PaginationID; set => _PaginationID = value; }
        public string PageSize { get => _PageSize; set => _PageSize = value; }
        public string WareHouseCode { get => _WareHouseCode; set => _WareHouseCode = value; }
    }
}