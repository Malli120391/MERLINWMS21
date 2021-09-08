using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21_Endpoint.DTO
{
    public class OutboundListDTO
    {
        private int _OutboundID;

        private int _RowNum;

        //Order Id
        private string _OBDNumber;

        //Date
        private string _OBDDate;

        private string _CustomerName;

        //Type
        private string _DocumentType;

        //WH Code
        private string _ReferedStores;

        private int _LineCount;

        private int _TodayCount;

        //Delivery Status
        private string _DeliveryStatus;


        private string _UserID;

        private string _WarehouseIDs;

        private string _InboundStatusID;

        private string _SupplierID;

        private string _StatusName;

        private string _ShipmentTypeID;

        private string _Limit;



        public int OutboundID { get => _OutboundID; set => _OutboundID = value; }
        public int RowNum { get => _RowNum; set => _RowNum = value; }
        public string OBDNumber { get => _OBDNumber; set => _OBDNumber = value; }
        public string OBDDate { get => _OBDDate; set => _OBDDate = value; }
        public string CustomerName { get => _CustomerName; set => _CustomerName = value; }
        public string DocumentType { get => _DocumentType; set => _DocumentType = value; }
        public string ReferedStores { get => _ReferedStores; set => _ReferedStores = value; }
        public int LineCount { get => _LineCount; set => _LineCount = value; }
        public int TodayCount { get => _TodayCount; set => _TodayCount = value; }
        public string DeliveryStatus { get => _DeliveryStatus; set => _DeliveryStatus = value; }
        public string UserID { get => _UserID; set => _UserID = value; }
        public string WarehouseIDs { get => _WarehouseIDs; set => _WarehouseIDs = value; }
        public string InboundStatusID { get => _InboundStatusID; set => _InboundStatusID = value; }
        public string SupplierID { get => _SupplierID; set => _SupplierID = value; }
        public string StatusName { get => _StatusName; set => _StatusName = value; }
        public string ShipmentTypeID { get => _ShipmentTypeID; set => _ShipmentTypeID = value; }
        public string Limit { get => _Limit; set => _Limit = value; }
    }

    public class SOListDTO
    {
        private string _SOHeaderID;

        //Order ID
        private string _SONumber;
        private string _DocReceivedDate;
        //status
        private string _StatusName;

        //WareHouse
        private string _WHCode;

        private string _UserID;

        //Type
        private string _SOType;

        private string _Limit;

        private string _Date;

        public string SOHeaderID { get => _SOHeaderID; set => _SOHeaderID = value; }
        public string SONumber { get => _SONumber; set => _SONumber = value; }
        public string StatusName { get => _StatusName; set => _StatusName = value; }
        public string WHCode { get => _WHCode; set => _WHCode = value; }
        public string UserID { get => _UserID; set => _UserID = value; }
        public string SOType { get => _SOType; set => _SOType = value; }
        public string Limit { get => _Limit; set => _Limit = value; }
        public string Date { get => _Date; set => _Date = value; }
        public string DocReceivedDate { get => _DocReceivedDate; set => _DocReceivedDate = value; }
    }
}