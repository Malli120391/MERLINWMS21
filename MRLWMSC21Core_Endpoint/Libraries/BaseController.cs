using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using MRLWMSC21_Endpoint.Security;
using MRLWMSC21Core.Library;
using Newtonsoft.Json;

namespace MRLWMSC21_Endpoint.Controllers
{
    public class BaseController : ApiController
    {
        private string _ConnectionString;
        private int _LoggedInUserID;
        public string ConnectionString { get => _ConnectionString; }
        public int LoggedInUserID { get => _LoggedInUserID; set => _LoggedInUserID = value; }

        public BaseController()
        {
            _ConnectionString = FrameworkUtilities.ReadApplicationKey("dbConnectionString");

        }


        public void LoadControllerDefaults(WMSCoreMessage oMessage, bool IsOverrideAuth = true)
        {
            _LoggedInUserID = Convert.ToInt32(oMessage.AuthToken.UserID);

            _ConnectionString = FrameworkUtilities.ReadApplicationKey("dbConnectionString");

            if (!WMSCoreEndpointSecurity.ValidateRequest(oMessage.AuthToken))
            {
                throw new WMSExceptionMessage() { WMSExceptionCode = "WMC_GEN_CTRLR_001", WMSMessage = ErrorMessages.WMC_GEN_CTRLR_001, ShowAsCriticalError = true };
            }
        }

        public static List<T> ConvertListJson<T>(object obj)
        {
            string output = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<List<T>>(output);

        }

        public static T ConvertJson<T>(object obj)
        {
            string output = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<T>(output);

        }





    }
}