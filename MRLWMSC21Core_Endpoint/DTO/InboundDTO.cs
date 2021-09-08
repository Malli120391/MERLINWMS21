using System;
using System.Collections.Generic;
using System.Linq;    
using System.Web;

namespace MRLWMSC21_Endpoint.DTO
{
    public class InboundDTO
    {
        private string _InboundID;
        private string _StoreRefNo;
      
        private string _PalletNo;
      
        private string _UserId;
    
        private string _AccountID;
        private string _StorageLocation;
        private string _Result;
        private string _Mcode;
        private string _ReceivedQty;
        private string _ItemPendingQty;
        private string _BatchNo;
        private string _SerialNo;
        private string _MfgDate;
        private string _ExpDate;
        private string _ProjectRefno;
        private string _Qty;
        private string _LineNo;
        private string _IsDam;
        private string _HasDisc;
        private string _CreatedBy;
        private string _CartonNo;
        private string _SkipType;
        private string _SkipReason;
        private string _InvoiceQty;
        private string _IsOutbound;
        private string _MRP;
        private string _Dock;
        private string _VehicleNo;
        private List<EntryDTO> _Entry;
        private string _SupplierInvoiceDetailsID;
        private string _HUNo;
        private string _HUSize;

     
        public string SupplierInvoiceDetailsID { get => _SupplierInvoiceDetailsID; set => _SupplierInvoiceDetailsID = value; }
        public string UserId { get => _UserId; set => _UserId = value;}
      
        public string InboundID { get => _InboundID; set => _InboundID = value; }
        public string StoreRefNo { get => _StoreRefNo; set => _StoreRefNo = value; }
   
        public string PalletNo { get => _PalletNo; set => _PalletNo = value; }
    
        public string AccountID { get => _AccountID; set => _AccountID = value; }
        public string StorageLocation { get => _StorageLocation; set => _StorageLocation = value; }
        public string Result { get => _Result; set => _Result = value; }
        public string Mcode { get => _Mcode; set => _Mcode = value; }
        public string ReceivedQty { get => _ReceivedQty; set => _ReceivedQty = value; }
        public string ItemPendingQty { get => _ItemPendingQty; set => _ItemPendingQty = value; }
        public string BatchNo { get => _BatchNo; set => _BatchNo = value; }
        public string SerialNo { get => _SerialNo; set => _SerialNo = value; }
        public string MfgDate { get => _MfgDate; set => _MfgDate = value; }
        public string ExpDate { get => _ExpDate; set => _ExpDate = value; }
        public string ProjectRefno { get => _ProjectRefno; set => _ProjectRefno = value; }
        public string Qty { get => _Qty; set => _Qty = value; }
        public string LineNo { get => _LineNo; set => _LineNo = value; }
        public string HasDisc { get => _HasDisc; set => _HasDisc = value; }
        public string CartonNo { get => _CartonNo; set => _CartonNo = value; }
        public string CreatedBy { get => _CreatedBy; set => _CreatedBy = value; }
        public string IsDam { get => _IsDam; set => _IsDam = value; }
        public string SkipType { get => _SkipType; set => _SkipType = value; }
        public string SkipReason { get => _SkipReason; set => _SkipReason = value; }
        public string InvoiceQty { get => _InvoiceQty; set => _InvoiceQty = value; }
        public string IsOutbound { get => _IsOutbound; set => _IsOutbound = value; }
        public string MRP { get => _MRP; set => _MRP = value; }
        public string Dock { get => _Dock; set => _Dock = value; }
        public List<EntryDTO> Entry { get => _Entry; set => _Entry = value; }
        public string VehicleNo { get => _VehicleNo; set => _VehicleNo = value; }
        public string HUNo { get => _HUNo; set => _HUNo = value; }
        public string HUSize { get => _HUSize; set => _HUSize = value; }
    }
}