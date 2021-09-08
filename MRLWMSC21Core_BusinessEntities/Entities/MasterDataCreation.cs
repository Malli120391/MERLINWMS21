using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMSCore_BusinessEntities.Entities
{
   public  class MasterDataCreation
    {
           
        private string tenantCode;
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

        public string TenantCode { get => tenantCode; set => tenantCode = value; }
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


    }
}
