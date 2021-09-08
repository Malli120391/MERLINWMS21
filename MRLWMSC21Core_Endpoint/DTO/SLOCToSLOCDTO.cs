using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21_Endpoint.DTO
{
    public class SLOCToSLOCDTO
    {
        private string _WareHouseCode;

        private string _PaginationId;
        private string _Pagesize;

        private string _StartDate;
        private string _EndDate;

        public string WareHouseCode { get => _WareHouseCode; set => _WareHouseCode = value; }
        public string PaginationId { get => _PaginationId; set => _PaginationId = value; }
        public string Pagesize { get => _Pagesize; set => _Pagesize = value; }
        public string StartDate { get => _StartDate; set => _StartDate = value; }
        public string EndDate { get => _EndDate; set => _EndDate = value; }
    }
}