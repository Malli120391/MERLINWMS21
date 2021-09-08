using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ServiceModel;
namespace MRLWMSC21WCF.BO
{
    [DataContract]
    public class VLPD
    {
        [DataMember]
        public string VLPDNo { set; get; }
        [DataMember]
        public int VLPDId { set; get; }
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
        public string FromCartonCode { set; get; }
        [DataMember]
        public int FromCartonID { set; get; }
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
        public string AssignedQuantity { set; get; }
        [DataMember]
        public string PickedQty { set; get; }
        [DataMember]
        public string PendingQty { set; get; }
        [DataMember]
        public string OutboundID { set; get; }

        [DataMember]
        public string SODetailsID { set; get; }
        [DataMember]
        public string StorageLocationID { set; get; }
        [DataMember]
        public string StorageLocation { set; get; }
        [DataMember]
        public string Result { set; get; }
        [DataMember]
        public int VLPDStatusID { set; get; }

        [DataMember]
        public int TransferRequestId { set; get; }

        [DataMember]
        public string POSOHeaderId { set; get; }
        [DataMember]
        public decimal CF { set; get; }
        [DataMember]
        public string MaterialMaster_IUoMID { set; get; }
        [DataMember]
        public string KitId { set; get; }
        [DataMember]
        public string Lineno { set; get; }
        [DataMember]
        public decimal QtyinBUoM { set; get; }

        [DataMember]
        public int GoodsmomentDeatilsId { set; get; }
        [DataMember]
        public decimal SkipQty { set; get; }
        [DataMember]
        public string SkipReason { set; get; }
        [DataMember]
        public int TransferRequestDetailsId { set; get; }
        [DataMember]
        public decimal TotalPickedQty { set; get; }
        [DataMember]
        public int Flag { set; get; }
        [DataMember]
        public int AccountId { set; get; }
        [DataMember]
        public string ToCartonCode { set; get; }
        [DataMember]
        public int PickedId { set; get; }
        [DataMember]
        public string Erocode { set; get; }

        [DataMember]
        public string MRP { set; get; }
        [DataMember]
        public string Vehicle { set; get; }
        [DataMember]
        public string OBDNumber { set; get; }
        [DataMember]
        public string DriverName { set; get; }
        [DataMember]
        public string DriverNo { set; get; }
        [DataMember]
        public string LRnumber { set; get; }
        [DataMember]
        public string TenantId { set; get; }
    }
}