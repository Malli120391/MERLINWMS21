using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21_Endpoint.DTO
{
    public class InboundDataDTO
    {

       
        private string supplierCode;
        private string warehouseCode;
        private string lineNumber;
        private string pONumber;
        private string itemCode;
        private string uoM;
        private string uoMQty;
        private string invoiceQuantity;
        private string invoiceNo;
        private string invoiceDate;

        private string _PODate;
        private string _MFGDate;
        private string _EXPDate;
        private string _BatchNO;
        private string _ProjectRef;
        private string _MRP;
        private string _POQty;

        private string _NoPackages;
        private string _GrossWeights;
        private string _CBM;
        private string _PrioritySetLevel;
        private string _PriorityReceivedDate;
        private string _PriorityReceivedTime;
        private string _Remarks;





        public string SupplierCode { get => supplierCode; set => supplierCode = value; }
        public string WarehouseCode { get => warehouseCode; set => warehouseCode = value; }
        public string LineNumber { get => lineNumber; set => lineNumber = value; }
        public string PONumber { get => pONumber; set => pONumber = value; }
        public string ItemCode { get => itemCode; set => itemCode = value; }
        public string UoM { get => uoM; set => uoM = value; }
        public string UoMQty { get => uoMQty; set => uoMQty = value; }
        public string InvoiceQuantity { get => invoiceQuantity; set => invoiceQuantity = value; }
        public string InvoiceNo { get => invoiceNo; set => invoiceNo = value; }
        public string InvoiceDate { get => invoiceDate; set => invoiceDate = value; }
        public string PODate { get => _PODate; set => _PODate = value; }
        public string MFGDate { get => _MFGDate; set => _MFGDate = value; }
        public string EXPDate { get => _EXPDate; set => _EXPDate = value; }
        public string BatchNO { get => _BatchNO; set => _BatchNO = value; }
        public string ProjectRef { get => _ProjectRef; set => _ProjectRef = value; }
        public string MRP { get => _MRP; set => _MRP = value; }
        public string POQty { get => _POQty; set => _POQty = value; }
        public string NoPackages { get => _NoPackages; set => _NoPackages = value; }
        public string GrossWeights { get => _GrossWeights; set => _GrossWeights = value; }
        public string CBM { get => _CBM; set => _CBM = value; }
        public string PrioritySetLevel { get => _PrioritySetLevel; set => _PrioritySetLevel = value; }
        public string PriorityReceivedDate { get => _PriorityReceivedDate; set => _PriorityReceivedDate = value; }
        public string PriorityReceivedTime { get => _PriorityReceivedTime; set => _PriorityReceivedTime = value; }
        public string Remarks { get => _Remarks; set => _Remarks = value; }
    }
}