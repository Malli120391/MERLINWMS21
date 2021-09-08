using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace MRLWMSC21WCF.BO
{
    [DataContract]
    public class TransferBO
    {

        [DataMember]
        public string TransferOrderNo { set; get; }
        [DataMember]
        public int TransferOrderId { set; get; }
        [DataMember]
        public int UserId { set; get; }
        [DataMember]
        public string Result { set; get; }
        [DataMember]
        public int AccountId { set; get; }
        [DataMember]
        public string MCode { set; get; }
        [DataMember]
        public string FromSLoc { set; get; }
        [DataMember]
        public string ToSLoc { set; get; }
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
        public string FromCartonNo { set; get; }
        [DataMember]
        public string FromLocation { set; get; }
        [DataMember]
        public string AvailQty { set; get; }
        [DataMember]
        public string TransferQty { set; get; }
        [DataMember]
        public string ToLocation { set; get; }
        [DataMember]
        public string ToCartonNo { set; get; }
        [DataMember]
        public string TenantID { set; get; }
        [DataMember]
        public string  WarehouseID { set; get; }

        public string MRP { get; set; }

    }
}