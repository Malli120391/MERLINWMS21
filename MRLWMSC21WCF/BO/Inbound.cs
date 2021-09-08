using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace MRLWMSC21WCF.BO
{
    [DataContract]
    public class Inbound
    {
        [DataMember]
        public string Storerefno { set; get; }
        [DataMember]
        public string Lineno { set; get; }
        [DataMember]
        public string GoodsMovementDetailsID { set; get; }
        [DataMember]
        public string GoodsMovementTypeID { set; get; }
        [DataMember]
        public string CartonNo { set; get; }
        [DataMember]
        public string Qty { set; get; }
        [DataMember]
        public string MCode { set; get; }
        [DataMember]
        public string InvoiceNumber { set; get; }
        [DataMember]
        public string PONumber { set; get; }
        [DataMember]
        public string POSOHeaderId { set; get; }
        [DataMember]
        public string CF { set; get; }
        [DataMember]
        public string MaterialMaster_IUoMID { set; get; }
        [DataMember]
        public string SupplierInvoiceID { set; get; }
        [DataMember]
        public string InboundId { set; get; }
        [DataMember]
        public string CartonId { set; get; }
        [DataMember]
        public string MaterialMasterId { set; get; }

        [DataMember]
        public string OEMPartNo { set; get; }
        [DataMember]
        public string MDescription { set; get; }
       
        [DataMember]
        public string HasDisc { set; get; }
        [DataMember]
        public string IsDam { set; get; }

        [DataMember]
        public string CreatedBy { set; get; }
        [DataMember]
        public string MaterialStorageParameterIDs { set; get; }
        [DataMember]
        public string MaterialStorageParameterValues { set; get; }
        [DataMember]
        public string KitId { set; get; }
        [DataMember]
        public string KitNo { set; get; }
        [DataMember]
        public decimal InvoiceQty { set; get; }
        [DataMember]
        public string SLoc { set; get; }
        [DataMember]
        public string SLocId { set; get; }
        [DataMember]
        public string SerialNo { set; get; }
        [DataMember]
        public string MfgDate { set; get; }
        [DataMember]
        public string ExpDate { set; get; }
        [DataMember]
        public string BatchNo { set; get; }
        [DataMember]
        public string HUSize { set; get; }
        [DataMember]
        public string HUNo { set; get; }

        [DataMember]
        public string ProjectNo { set; get; }
        [DataMember]
        public string MRP { set; get; }
        [DataMember]
        public string Result { set; get; }
        [DataMember]
        public int Deliverystatus { set; get; }
        [DataMember]
        public int CycleCountOn { set; get; }
        [DataMember]
        public string IsPostiveRecall { set; get; }

        [DataMember]
        public string Msps { set; get; }
        [DataMember]
        public decimal ReceivedQty { set; get; }
        [DataMember]
        public decimal ItemPendingQty { set; get; }
        [DataMember]
        public int AccountId { set; get; }
        [DataMember]
        public int UserId { set; get; }
        [DataMember]
        public int TenantId { set; get; }
        [DataMember]
        public string Dock { set; get; }
        [DataMember]
        public string VehicleNo { set; get; }
        [DataMember]
        public string SupplierInvoiceDetailsId { set; get; }


    }

    [DataContract]
    public class MSP
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Value { get; set; }
    }
}