using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace MRLWMSC21WCF.BO
{
    [DataContract]
    public class PutAway
    {
        [DataMember]
        public int Id { set; get; }
        [DataMember]
        public string TrnasferNo { set; get; }
        [DataMember]
        public int TransferId { set; get; }
        [DataMember]
        public int UserId { set; get; }
        [DataMember]
        public int Assignedid { set; get; }
        [DataMember]
        public int MaterialMasterId { set; get; }
        [DataMember]
        public string MCode { set; get; }
        [DataMember]
        public string MDescription { set; get; }
        [DataMember]
        public string CartonCode { set; get; }
        [DataMember]
        public int CartonID { set; get; }
        [DataMember]
        public string Location { set; get; }
        [DataMember]
        public int LocationID { set; get; }
        [DataMember]
        public string ExpDate { set; get; }
        [DataMember]
        public string MfgDate { set; get; }
        [DataMember]
        public string SerialNo { set; get; }
        [DataMember]
        public string BatchNo { set; get; }
        [DataMember]
        public string ProjectRefNo { set; get; }
        [DataMember]
        public string MRP { set; get; }
        [DataMember]
        public string AssignedQuantity { set; get; }
        [DataMember]
        public string PutAwayQty { set; get; }
        [DataMember]
        public string PendingPutAwayQty { set; get; }

        [DataMember]
        public string StorageLocationID { set; get; }
        [DataMember]
        public string StorageLocation { set; get; }
        [DataMember]
        public string Result { set; get; }
    
        [DataMember]
        public int TransferRequestId { set; get; }
        [DataMember]
        public string Lineno { set; get; }
        [DataMember]
        public string CF { set; get; }
        [DataMember]
        public string MaterialMaster_IUoMID { set; get; }
        [DataMember]
        public string SupplierInvoiceID { set; get; }
        [DataMember]
        public string InboundId { set; get; }
        [DataMember]
        public string POSOHeaderId { set; get; }
        [DataMember]
        public string KitId { set; get; }
        [DataMember]
        public decimal GMDRemainingQty { set; get; }
        [DataMember]
        public string GoodsMovementDetailsID { set; get; }
        [DataMember]
        public string GoodsMovementTypeID { set; get; }
        [DataMember]
        public string IsPostiveRecall { set; get; }

        [DataMember]
        public string HasDisc { set; get; }
        [DataMember]
        public string IsDam { set; get; }
        [DataMember]
        public string CartonId { set; get; }
        [DataMember]
        public string SLocId { set; get; }
        [DataMember]
        public string StorerefNo { set; get; }
        [DataMember]
        public int SuggestedPutawayID { set; get; }
        [DataMember]
        public decimal SuggestedRemainingQty { set; get; }
        [DataMember]
        public decimal SuggestedReceivedQty { set; get; }
        [DataMember]
        public decimal SuggestedQty { set; get; }
        [DataMember]
        public decimal TotalQuantity { set; get; }
        [DataMember]
        public string Skipreason { set; get; }
        [DataMember]
        public decimal SkipQty{ set; get; }
        [DataMember]
        public int Flag { set; get; }
        [DataMember]
        public int TransferRequestDetailsId { set; get; }
        [DataMember]
        public int PickedLocationID { set; get; }
        [DataMember]
        public decimal ReceivedQuantity { set; get; }

        [DataMember]
        public string Dock { set; get; }

        [DataMember]
        public string ScnnedLocation { set; get; }


    }
}