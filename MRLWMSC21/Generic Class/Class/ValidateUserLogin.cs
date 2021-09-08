using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21.Generic_Class.Class
{

    [Serializable()]
    public class ValidateUserLogin
    {
        public int Apps_MST_Application_ID { get; set; }
        public int Apps_MST_User_ID { get; set; }
        public string EMail { get; set; }
        public string Password { get; set; }
        public string ClientCookieIdentifier { get; set; }
        public string ClientMAC { get; set; }
        public string SessionIdentifier { get; set; }
        public string LastRequestTimestamp { get; set; }

    }

    [Serializable()]
    public class ValidateLoginResponse
    {

        public int Apps_MST_Application_ID { get; set; }

        public int Apps_MST_User_ID { get; set; }

        public int ConcurrentUserLimit { get; set; }

        
        public int WarehouseLimit { get; set; }

        public string ClientCookieIdentifier { get; set; }

        public string SessionIdentifier { get; set; }

        public int SessionTimeoutTime { get; set; }
        public int IsAuthenticated { get; set; }
        public string SubscriptionEndDate { get; set; }
        public string ErrorCode { get; set; }
        public string Zoho_Status { get; set; }
        public ValidateLoginResponse(string json)
        {
            JObject jObject = JObject.Parse(json);
            JToken jUser = jObject["ValidateLoginResponse"];
            Apps_MST_Application_ID = (int)jUser["Apps_MST_Application_ID"];
            Apps_MST_User_ID = (int)jUser["Apps_MST_User_ID"];
            ConcurrentUserLimit = (int)jUser["ConcurrentUserLimit"];
            WarehouseLimit = (int)jUser["WarehouseLimit"];
            ClientCookieIdentifier = (string)jUser["ClientCookieIdentifier"];
            SessionIdentifier = (string)jUser["SessionIdentifier"];
            SessionTimeoutTime = (int)jUser["SessionTimeoutTime"];
            IsAuthenticated = (int)jUser["IsAuthenticated"];
            SubscriptionEndDate = (string)jUser["SubscriptionEndDate"];
            ErrorCode = (string)jUser["ZOHO_Status"] == "Canceled" ? "-99" : (string)jUser["ErrorCode"];
            Zoho_Status = (string)jUser["ZOHO_Status"];
        }

    }

}