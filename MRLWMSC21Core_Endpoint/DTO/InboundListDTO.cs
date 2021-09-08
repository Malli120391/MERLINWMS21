using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21_Endpoint.DTO
{
    public class InboundListDTO
    {
        //Inbound Number
        private string _StoreRefNo;

        //Type
        private string _ShipmentType;

        private string _SupplierName;

        //Date
        private string _DocReceivedDate;

        //Status
        private string _InboundStatus;

        private int _LineCount;


        private string _UserID;

        private string _WarehouseIDs;

        private string _InboundStatusID;

        private string _SupplierID;

        private string _StatusName;

        private string _ShipmentTypeID;

        private string _Limit;
        private string _Date;



        //Wh Code
        private string _ReferedStores;

        public string StoreRefNo { get => _StoreRefNo; set => _StoreRefNo = value; }
        public string ShipmentType { get => _ShipmentType; set => _ShipmentType = value; }
        public string SupplierName { get => _SupplierName; set => _SupplierName = value; }
        public string DocReceivedDate { get => _DocReceivedDate; set => _DocReceivedDate = value; }
        public string InboundStatus { get => _InboundStatus; set => _InboundStatus = value; }
        public int LineCount { get => _LineCount; set => _LineCount = value; }
        public string UserID { get => _UserID; set => _UserID = value; }
        public string ReferedStores { get => _ReferedStores; set => _ReferedStores = value; }
        public string WarehouseIDs { get => _WarehouseIDs; set => _WarehouseIDs = value; }
        public string InboundStatusID { get => _InboundStatusID; set => _InboundStatusID = value; }
        public string SupplierID { get => _SupplierID; set => _SupplierID = value; }
        public string StatusName { get => _StatusName; set => _StatusName = value; }
        public string ShipmentTypeID { get => _ShipmentTypeID; set => _ShipmentTypeID = value; }
        public string Limit { get => _Limit; set => _Limit = value; }
        public string Date { get => _Date; set => _Date = value; }
    }

    public class POListDTO
    {
        private string _POHeaderID;

        //Type
        private string _POType;

        //Status
        private string _StatusName;

        private string _StoreRefNos;

        private string _LineItemCount;
        private string _DocReceivedDate;
        //WH COde
        private string _WHCode;

        //Order ID
        private string _PONumber;

        private string _UserID;

        public string POHeaderID { get => _POHeaderID; set => _POHeaderID = value; }
        public string POType { get => _POType; set => _POType = value; }
        public string StatusName { get => _StatusName; set => _StatusName = value; }
        public string StoreRefNos { get => _StoreRefNos; set => _StoreRefNos = value; }
        public string LineItemCount { get => _LineItemCount; set => _LineItemCount = value; }
        public string WHCode { get => _WHCode; set => _WHCode = value; }
        public string PONumber { get => _PONumber; set => _PONumber = value; }
        public string UserID { get => _UserID; set => _UserID = value; }
        public string DocReceivedDate { get => _DocReceivedDate; set => _DocReceivedDate = value; }
    }
}