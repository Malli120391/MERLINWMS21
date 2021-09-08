using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRLWMSC21Core.Entities
{
   public class Outbound
    {
        private int _MaterialMasterID;
        private string _Mcode;
        private decimal _SOQty;
        private decimal _PackedQty;
        private int _OutboundID;
        private string _CustomerName;
        private int _SoDetailsID;
        private string _MFGDate;
        private string _EXPDate;
        private string _SerialNo;
        private string _ProjectRefNo;
        private string _BatchNo;
        private string _MRP;
        private string _BusinessType;
        private int _PSNID;
        private decimal _PickedQty;
        private string _CartonSerialNo;
        private int _PSNDetailsID;
        private string _OBDNumber;
        private string _PackType;
        private int _SOHeaderID;
        private string _PackComplete;
        private string _SONumber;
        private string _DriverName;
        private string _DriverNo;
        private string _LRnumber;
        private int _UserId;
        private string _TenantId;
        private string _Vehicle;
        private List<string> _lstSalesOrders;
        private string _LoadRefNo;
        private int _TotalSOCount;
        private int _AccountID;
        private int _ScannedSOCount;
        private string _CustomerCode;
        private string _CustomerAddress;
        private string _ErroMessage;
        private string _WareHouseID;
        private string _Status;
        private string _HUNo;
        private string _HUSize;


        public string Status { get => _Status; set => _Status = value; }
        public int MaterialMasterID { get => _MaterialMasterID; set => _MaterialMasterID = value; }
        public string Mcode { get => _Mcode; set => _Mcode = value; }
        public decimal SOQty { get => _SOQty; set => _SOQty = value; }
        public decimal PackedQty { get => _PackedQty; set => _PackedQty = value; }
        public int OutboundID { get => _OutboundID; set => _OutboundID = value; }
        public string CustomerName { get => _CustomerName; set => _CustomerName = value; }
        public int SoDetailsID { get => _SoDetailsID; set => _SoDetailsID = value; }
        public string MFGDate { get => _MFGDate; set => _MFGDate = value; }
        public string EXPDate { get => _EXPDate; set => _EXPDate = value; }
        public string SerialNo { get => _SerialNo; set => _SerialNo = value; }
        public string ProjectRefNo { get => _ProjectRefNo; set => _ProjectRefNo = value; }
        public string MRP { get => _MRP; set => _MRP = value; }
        public string BatchNo { get => _BatchNo; set => _BatchNo = value; }
        public string BusinessType { get => _BusinessType; set => _BusinessType = value; }
        public int PSNID { get => _PSNID; set => _PSNID = value; }
        public decimal PickedQty { get => _PickedQty; set => _PickedQty = value; }
        public string CartonSerialNo { get => _CartonSerialNo; set => _CartonSerialNo = value; }
        public int PSNDetailsID { get => _PSNDetailsID; set => _PSNDetailsID = value; }
        public string OBDNumber { get => _OBDNumber; set => _OBDNumber = value; }
        public string PackType { get => _PackType; set => _PackType = value; }
        public int SOHeaderID { get => _SOHeaderID; set => _SOHeaderID = value; }
        public string PackComplete { get => _PackComplete; set => _PackComplete = value; }
        public string SONumber { get => _SONumber; set => _SONumber = value; }
        public string DriverName { get => _DriverName; set => _DriverName = value; }
        public string DriverNo { get => _DriverNo; set => _DriverNo = value; }
        public string LRnumber { get => _LRnumber; set => _LRnumber = value; }
        public int UserId { get => _UserId; set => _UserId = value; }
        public string TenantId { get => _TenantId; set => _TenantId = value; }
        public string Vehicle { get => _Vehicle; set => _Vehicle = value; }
        public List<string> LstSalesOrders { get => _lstSalesOrders; set => _lstSalesOrders = value; }
        public string LoadRefNo { get => _LoadRefNo; set => _LoadRefNo = value; }
        public int TotalSOCount { get => _TotalSOCount; set => _TotalSOCount = value; }
        public int ScannedSOCount { get => _ScannedSOCount; set => _ScannedSOCount = value; }
        public string CustomerCode { get => _CustomerCode; set => _CustomerCode = value; }
        public string CustomerAddress { get => _CustomerAddress; set => _CustomerAddress = value; }
        public string WareHouseID { get => _WareHouseID; set => _WareHouseID = value; }
        public int AccountID { get => _AccountID; set => _AccountID = value; }
        public string HUNo { get => _HUNo; set => _HUNo = value; }
        public string HUSize { get => _HUSize; set => _HUSize = value; }
    }
}
