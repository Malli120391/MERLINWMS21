using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMSCore_BusinessEntities.Entities
{
    public class ScannedItem
    {
        private string _scanInput;
        private string _warehouseID;
        private bool _scanResult;
        private string _message;
        private string _tenantID;
        private string _skuCode;
        private string _batch;
        private string _serialNumber;
        private string _expDate;
        private string _mfgDate;
        private string _prjRef;
        private string _kitID;
        private string _lineNumber;
        private String _inboundID;
        private String _mrp;
        private string _obdNumber;
        private string _vlpdNumber;
        private string _SupplierInvoiceDetailsId;
        private int _AccountID;
        private int _IsCycleCount;
        private int _UserID;
        private int _HUSize;
        private int _HUNo;
        public int HUNo { get => _HUNo; set => _HUNo = value; }
        public int HUSize { get => _HUSize; set => _HUSize = value; }
        public string SupplierInvoiceDetailsId { get => _SupplierInvoiceDetailsId; set => _SupplierInvoiceDetailsId = value; }
        public string ScanInput { get => _scanInput; set => _scanInput = value; }
        public bool ScanResult { get => _scanResult; set => _scanResult = value; }
        public string Message { get => _message; set => _message = value; }
        public string WarehouseID { get => _warehouseID; set => _warehouseID = value; }
        public string TenantID { get => _tenantID; set => _tenantID = value; }
        public string SkuCode { get => _skuCode; set => _skuCode = value; }
        public string Batch { get => _batch; set => _batch = value; }
        public string SerialNumber { get => _serialNumber; set => _serialNumber = value; }
        public string ExpDate { get => _expDate; set => _expDate = value; }
        public string MfgDate { get => _mfgDate; set => _mfgDate = value; }
        public string PrjRef { get => _prjRef; set => _prjRef = value; }
        public string KitID { get => _kitID; set => _kitID = value; }
        public string LineNumber { get => _lineNumber; set => _lineNumber = value; }
        public string InboundID { get => _inboundID; set => _inboundID = value; }
        public string Mrp { get => _mrp; set => _mrp = value; }
        public string ObdNumber { get => _obdNumber; set => _obdNumber = value; }
        public string VlpdNumber { get => _vlpdNumber; set => _vlpdNumber = value; }
        public int AccountID { get => _AccountID; set => _AccountID = value; }
        public int IsCycleCount { get => _IsCycleCount; set => _IsCycleCount = value; }
        public int UserID { get => _UserID; set => _UserID = value; }
    }
}
