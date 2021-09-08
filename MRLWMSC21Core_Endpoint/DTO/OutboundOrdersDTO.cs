using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21_Endpoint.DTO
{
    public class OutboundOrdersDTO
    {
        private Constants.EndpointConstants.ScanType _ScanType;


        private string _TenantCode;
        private string _WareHouseCode;
        private string _DeliveryDocNumber;
        private string _InvoiceNo;
        private string _SONumber;
        private string _AWBNo;
        private string _Courier;
        private string _DeliveryDocType;
        private string _DeliveryDocDate;
        private string _PickingDate;
        private string _PackingDate;
        private string _LoadGeneratedDate;
        private string _PGIDate;
        private string _DeliveryDate;
        private string _DeliveryStatus;
        private string _TotalRecords;


        private string _UpdatedDate;

        private string _StartDate;
        private string _EndDate;
        private string _PaginationID;
        private string _PageSize;

        public string StartDate { get => _StartDate; set => _StartDate = value; }
        public string EndDate { get => _EndDate; set => _EndDate = value; }
        public string PaginationID { get => _PaginationID; set => _PaginationID = value; }
        public string PageSize { get => _PageSize; set => _PageSize = value; }
        public string TenantCode { get => _TenantCode; set => _TenantCode = value; }
        public string WareHouseCode { get => _WareHouseCode; set => _WareHouseCode = value; }
        public string DeliveryDocNumber { get => _DeliveryDocNumber; set => _DeliveryDocNumber = value; }
        public string InvoiceNo { get => _InvoiceNo; set => _InvoiceNo = value; }
        public string SONumber { get => _SONumber; set => _SONumber = value; }
        public string AWBNo { get => _AWBNo; set => _AWBNo = value; }
        public string Courier { get => _Courier; set => _Courier = value; }
        public string DeliveryDocType { get => _DeliveryDocType; set => _DeliveryDocType = value; }
        public string DeliveryDocDate { get => _DeliveryDocDate; set => _DeliveryDocDate = value; }
        public string PickingDate { get => _PickingDate; set => _PickingDate = value; }
        public string PackingDate { get => _PackingDate; set => _PackingDate = value; }
        public string LoadGeneratedDate { get => _LoadGeneratedDate; set => _LoadGeneratedDate = value; }
        public string PGIDate { get => _PGIDate; set => _PGIDate = value; }
        public string DeliveryDate { get => _DeliveryDate; set => _DeliveryDate = value; }
        public string DeliveryStatus { get => _DeliveryStatus; set => _DeliveryStatus = value; }
        public string TotalRecords { get => _TotalRecords; set => _TotalRecords = value; }
        public string UpdatedDate { get => _UpdatedDate; set => _UpdatedDate = value; }
    }
}