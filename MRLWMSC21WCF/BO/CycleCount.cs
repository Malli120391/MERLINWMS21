using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace MRLWMSC21WCF.BO
{
    [DataContract]
    public class CycleCount
    {
        [DataMember]
        public string Result { set; get; }
        [DataMember]
        public string Location { set; get; }
        [DataMember]
        public int UserId { set; get; }
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
        public string MRP { set; get; }
        [DataMember]
        public string ProjectRefNo { set; get; }
        [DataMember]
        public string CCName { set; get; }
        [DataMember]
        public string SKU { set; get; }
        [DataMember]
        public decimal CCQty { set; get; }
        [DataMember]
        public string Count { set; get; }
        [DataMember]
        public int CycleCountID { set; get; }
        [DataMember]
        public int AccountCycleCountID { set; get; }
        [DataMember]
        public int MSTCycleCountID { set; get; }
        [DataMember]
        public int CycleCountEntityID { set; get; }
        [DataMember]
        public int EntityID { set; get; }
        [DataMember]
        public string Container { set; get; }
        [DataMember]
        public int AccountId { set; get; }
        [DataMember]
        public int TenantID { set; get; }
        [DataMember]
        public int WarehouseID { set; get; }
        public string StorageLocation { set; get; }
    }
}