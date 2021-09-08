using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ServiceModel;
namespace MRLWMSC21WCF.BO
{
    [DataContract]
    public class Outbound
    {


        [DataMember]
        public string Obdno { set; get; }
        [DataMember]
        public string Lineno { set; get; }
        [DataMember]
        public string GoodsMovementDetailsID { set; get; }
        [DataMember]
        public string GoodsMovementTypeID { set; get; }
        [DataMember]
        public string CartonNo { set; get; }
        [DataMember]
        public decimal Qty { set; get; }
        [DataMember]
        public string MCode { set; get; }
        [DataMember]
        public string InvoiceNumber { set; get; }
        [DataMember]
        public string SONumber { set; get; }
        [DataMember]
        public string POSOHeaderId { set; get; }
        [DataMember]
        public decimal CF { set; get; }
        [DataMember]
        public string MaterialMaster_IUoMID { set; get; }
        [DataMember]
        public string SupplierInvoiceID { set; get; }
        [DataMember]
        public string OutboundId { set; get; }
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
        public string InvoiceQty { set; get; }
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
        public string ProjectNo { set; get; }
        [DataMember]
        public string Result { set; get; }
        [DataMember]
        public int Deliverystatus { set; get; }
        [DataMember]
        public Boolean CycleCountOn { set; get; }
        [DataMember]
        public string IsPostiveRecall { set; get; }
        [DataMember]
        public string Location { set; get; }
        [DataMember]
        public string AvailQty { set; get; }
        [DataMember]
        public decimal QtyinBUoM { set; get; }

        [DataMember]
         public string Msps { set; get; }
        [DataMember]
        public decimal OBDQty { set; get; }
        [DataMember]
        public int LocationId { set; get; }
        [DataMember]
        public string AssignedQuantity { set; get; }
        [DataMember]
        public string PickedQty { set; get; }
        [DataMember]
        public string SODetailsID { set; get; }
        [DataMember]
        public int GoodsmomentDeatilsId { set; get; }
        
        [DataMember]
        public string PendingQty { set; get; }
        [DataMember]
        public int AccountId { set; get; }
        [DataMember]
        public decimal TotalPickedQty { set; get; }
        [DataMember]
        public string SkipReason { set; get; }
        [DataMember]
        public decimal SkipQty { set; get; }
        [DataMember]
        public int Flag { set; get; }
        [DataMember]
        public int Assignedid { set; get; }
        [DataMember]
        public string ToCartonNo { set; get; }
        [DataMember]
        public string Erocode { set; get; }

        [DataMember]
        public string MRP { set; get; }

        [DataMember]
        public int HUSize { set; get; }

        [DataMember]
        public int HUNo { set; get; }
    }
}