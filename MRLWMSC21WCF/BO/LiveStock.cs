using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ServiceModel;
namespace MRLWMSC21WCF.BO
{
    [DataContract]
    public class LiveStock
    {
        [DataMember]
        public string Mcode { set; get; }
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
        public string CartonNo { set; get; }
        [DataMember]
        public string Location { set; get; }
        [DataMember]
        public string TenantCode { set; get; }
        [DataMember]
        public int AccountId { set; get; }

        [DataMember]
        public int WarehouseID { set; get; }

        [DataMember]
        public int UserId { set; get; }
        [DataMember]
        public int TenantID { set; get; }
        public string MRP { set; get; }
    }
}