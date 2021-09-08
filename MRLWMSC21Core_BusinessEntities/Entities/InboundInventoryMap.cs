using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRLWMSC21Core.Entities
{
    public class InboundInventoryMap
    {
        private int _InboundID;
        private int _POHeaderID;
        private int _PODetailsID;
        private int _SupplierInvoiceID;
        private int _SupplierInvoiceDetailsID;
        private int _MaterialMasterID;
        private int _VehicleID;
        private int _OrderLineNumber;
        private int _IUoMID;
        private decimal _MRP;
        private int _MOP;
        private int _ColorID;

        private int _IsExcessPO;

        private int _GenericSKUID;

        private string _IUoM;
        private string _PONumber;
        private string _InvoiceNumber;
        private string _VehicleNumber;
        private string _MaterialCode;
        private string _LRNumber;

        private decimal _IUoMConversionQty;
        private decimal _ShipmentQuantity;
        private decimal _ReceivedQuantity;

        private decimal _GRNQuantity;

        public int InboundID { get => _InboundID; set => _InboundID = value; }
        public int POHeaderID { get => _POHeaderID; set => _POHeaderID = value; }
        public int PODetailsID { get => _PODetailsID; set => _PODetailsID = value; }
        public int SupplierInvoiceID { get => _SupplierInvoiceID; set => _SupplierInvoiceID = value; }
        public int SupplierInvoiceDetailsID { get => _SupplierInvoiceDetailsID; set => _SupplierInvoiceDetailsID = value; }
        public int MaterialMasterID { get => _MaterialMasterID; set => _MaterialMasterID = value; }
        public int VehicleID { get => _VehicleID; set => _VehicleID = value; }
        public int OrderLineNumber { get => _OrderLineNumber; set => _OrderLineNumber = value; }
        public int IUoMID { get => _IUoMID; set => _IUoMID = value; }
        public string IUoM { get => _IUoM; set => _IUoM = value; }
        public string PONumber { get => _PONumber; set => _PONumber = value; }
        public string InvoiceNumber { get => _InvoiceNumber; set => _InvoiceNumber = value; }
        public string VehicleNumber { get => _VehicleNumber; set => _VehicleNumber = value; }
        public string MaterialCode { get => _MaterialCode; set => _MaterialCode = value; }
        public decimal IUoMConversionQty { get => _IUoMConversionQty; set => _IUoMConversionQty = value; }
        public decimal ShipmentQuantity { get => _ShipmentQuantity; set => _ShipmentQuantity = value; }
        public string LRNumber { get => _LRNumber; set => _LRNumber = value; }
        public decimal ReceivedQuantity { get => _ReceivedQuantity; set => _ReceivedQuantity = value; }
        public decimal GRNQuantity { get => _GRNQuantity; set => _GRNQuantity = value; }
        public decimal MRP { get => _MRP; set => _MRP = value; }
        public int MOP { get => _MOP; set => _MOP = value; }
        public int ColorID { get => _ColorID; set => _ColorID = value; }
        public int GenericSKUID { get => _GenericSKUID; set => _GenericSKUID = value; }
        public int IsExcessPO { get => _IsExcessPO; set => _IsExcessPO = value; }
    }
}
