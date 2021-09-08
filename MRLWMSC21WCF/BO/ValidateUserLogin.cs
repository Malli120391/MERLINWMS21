using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ServiceModel;


namespace MRLWMSC21WCF.BO
{
    [DataContract]
    public class ValidateUserLogin
    {
        [DataMember]
        public int Apps_MST_Application_ID { get; set; }
        [DataMember]
        public int Apps_MST_User_ID { get; set; }
        [DataMember]
        public string EMail { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string ClientCookieIdentifier { get; set; }
        [DataMember]
        public string ClientMAC { get; set; }
        [DataMember]
        public string SessionIdentifier { get; set; }
        [DataMember]
        public string LastRequestTimestamp { get; set; }
        [DataMember]
        public string ErroCode { get; set; }
        [DataMember]
        public string SubscriptionEndDate { get; set; }
        [DataMember]
        public int AccountId { get; set; }

    }
}