using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRLWMSC21Core.Entities
{
    public class Picklist
    {
        private int _PicklistHeaderID;

        private string _PickListRefNo;
        
        private int _OutboundID;
        private int _TransferRequestID;
        private int _JobOrderHeaderID;
        private int _CustomerID;
        private string _CustomerCode;

        private int _OBD_PickListTypeID;
        
        private int _Dock_LocationID;
        private string _DockLocationCode;

        private List<PickListInventory> _InventoryList;
        private string _PalletCode;

        private decimal _TotalPicklistQuantity;
        private decimal _TotalPickedQuantity;

        public int PicklistHeaderID { get => _PicklistHeaderID; set => _PicklistHeaderID = value; }
        public string PickListRefNo { get => _PickListRefNo; set => _PickListRefNo = value; }
        public int OutboundID { get => _OutboundID; set => _OutboundID = value; }
        public int TransferRequestID { get => _TransferRequestID; set => _TransferRequestID = value; }
        public int JobOrderHeaderID { get => _JobOrderHeaderID; set => _JobOrderHeaderID = value; }
        public int CustomerID { get => _CustomerID; set => _CustomerID = value; }
        public int OBD_PickListTypeID { get => _OBD_PickListTypeID; set => _OBD_PickListTypeID = value; }
        public int Dock_LocationID { get => _Dock_LocationID; set => _Dock_LocationID = value; }
        public string DockLocationCode { get => _DockLocationCode; set => _DockLocationCode = value; }
        public List<PickListInventory> InventoryList { get => _InventoryList; set => _InventoryList = value; }
        public string PalletCode { get => _PalletCode; set => _PalletCode = value; }
        public decimal TotalPicklistQuantity { get => _TotalPicklistQuantity; set => _TotalPicklistQuantity = value; }
        public decimal TotalPickedQuantity { get => _TotalPickedQuantity; set => _TotalPickedQuantity = value; }
        public string CustomerCode { get => _CustomerCode; set => _CustomerCode = value; }
    }

}
