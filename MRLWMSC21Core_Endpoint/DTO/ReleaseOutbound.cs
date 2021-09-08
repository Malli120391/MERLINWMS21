using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21_Endpoint.DTO
{
    public class ReleaseOutbound
    {

        private string _StartDate;

        private string _EndDate;


        private string _WarehouseCode;

        private string _PaginationID;

        private string _PageSize;

     
        public string WarehouseCode { get => _WarehouseCode; set => _WarehouseCode = value; }
        public string PaginationID { get => _PaginationID; set => _PaginationID = value; }
        public string PageSize { get => _PageSize; set => _PageSize = value; }
        public string StartDate { get => _StartDate; set => _StartDate = value; }
        public string EndDate { get => _EndDate; set => _EndDate = value; }
    }
}