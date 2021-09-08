using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRLWMSC21Core.Entities
{
    public class APIInventory
    {

        private string _TenantCode;
        private string _WareHouseCode;
        private string _MaterialCode;
        private string _ExpDate;
        private string _BatchNo;
        private string _ProjectNo;
        private string _SerialNo;
        private string _MfgDate;
        private string _MaterialDescription;
        private string _UoMQty;
        private decimal _AvailableQuantity;
        private string _ProjectRefNo;
        private decimal _OnHandQty;
        private string _MRP;
        private string _BatchNumber;
        private decimal _AllocatedQty;
        private decimal _DamagedQty;
        private string _TotalRecords;

        private string _StartDate;
        private string _EndDate;
        private string _PaginationID;
        private string _PageSize;

        private string _UpdatedDate;

        public string TenantCode { get => _TenantCode; set => _TenantCode = value; }
        public string WareHouseCode { get => _WareHouseCode; set => _WareHouseCode = value; }
        public string MaterialCode { get => _MaterialCode; set => _MaterialCode = value; }
        public string ExpDate { get => _ExpDate; set => _ExpDate = value; }
        public string BatchNo { get => _BatchNo; set => _BatchNo = value; }
        public string ProjectNo { get => _ProjectNo; set => _ProjectNo = value; }
        public string SerialNo { get => _SerialNo; set => _SerialNo = value; }
        public string MfgDate { get => _MfgDate; set => _MfgDate = value; }
        public string MaterialDescription { get => _MaterialDescription; set => _MaterialDescription = value; }
        public string UoMQty { get => _UoMQty; set => _UoMQty = value; }
        public decimal AvailableQuantity { get => _AvailableQuantity; set => _AvailableQuantity = value; }
        public string ProjectRefNo { get => _ProjectRefNo; set => _ProjectRefNo = value; }
        public decimal OnHandQty { get => _OnHandQty; set => _OnHandQty = value; }
        public string MRP { get => _MRP; set => _MRP = value; }
        public string StartDate { get => _StartDate; set => _StartDate = value; }
        public string EndDate { get => _EndDate; set => _EndDate = value; }
        public string PaginationID { get => _PaginationID; set => _PaginationID = value; }
        public string PageSize { get => _PageSize; set => _PageSize = value; }
        public string BatchNumber { get => _BatchNumber; set => _BatchNumber = value; }
        public decimal AllocatedQty { get => _AllocatedQty; set => _AllocatedQty = value; }
        public decimal DamagedQty { get => _DamagedQty; set => _DamagedQty = value; }
        public string TotalRecords { get => _TotalRecords; set => _TotalRecords = value; }
        public string UpdatedDate { get => _UpdatedDate; set => _UpdatedDate = value; }
    }
}
