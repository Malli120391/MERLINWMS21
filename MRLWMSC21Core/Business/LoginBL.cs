using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRLWMSC21Core.Entities;
using MRLWMSC21Core.SSOService;
using MRLWMSC21Core.Library;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MRLWMSC21Core.DataAccess;

namespace MRLWMSC21Core.Business
{
    public class LoginBL : Interfaces.ILoginBL
    {
        private string _ClassCode = "WMSCore_BL_001_";

        private UserDAL oUserDAL = null;
        private SingleSignOnDBSinkClient _objServiceClient = null;
        private Constants.ApplicationAuthentication _AuthMechanism;

        public LoginBL(Constants.ApplicationAuthentication AuthMechanism, int LoginUser, string ConnectionString)
        {
            _AuthMechanism = AuthMechanism;

            if (AuthMechanism == Constants.ApplicationAuthentication.InventraxSSO)
                _objServiceClient = new SingleSignOnDBSinkClient();
            else
                oUserDAL = new UserDAL(LoginUser, ConnectionString);

            _ClassCode = ExceptionHandling.GetClassExceptionCode(ExceptionHandling.ExcpConstants_API_BusinessLayer.LoginBL);
        }

        public UserProfile UserLogin(UserProfile oUser)
        {

            try
            {
                int _sAPPS_MST_User_ID = 0;


                if (_AuthMechanism == Constants.ApplicationAuthentication.InventraxSSO)
                {
                    ValidateUserLogin objValidateUserLogin = new ValidateUserLogin
                    {
                        Apps_MST_Application_ID = 2,
                        ClientMAC = oUser.ClientMAC,
                        ClientCookieIdentifier = oUser.CookieIdentifier,
                        EMail = oUser.EMailID,
                        SessionIdentifier = oUser.SessionIdentifier,
                        Password = oUser.Password
                    };

                    string json = JsonConvert.SerializeObject(objValidateUserLogin);
                    string UserResult = _objServiceClient.ValidateUserLogin(json);

                    UserResult = "{\"ValidateLoginResponse\": " + UserResult.Remove(0, 1).Replace("]", "") + "}";
                    ValidateLoginResponse _Response = new ValidateLoginResponse(UserResult);

                    string _sErrorMessage = string.Empty;

                    _sAPPS_MST_User_ID = ConversionUtility.ConvertToInt(_Response.Apps_MST_User_ID.ToString().Trim());//_Response.UserProfiler.SSOUserID.ToString();
                }
                UserProfile _oUserProfile = new UserProfile
                {
                    SSOUserID = _sAPPS_MST_User_ID,
                    CookieIdentifier = oUser.CookieIdentifier,
                    SessionIdentifier = oUser.SessionIdentifier,
                    ClientIP = oUser.ClientIP,
                    ClientMAC = oUser.ClientMAC,
                    EMailID = oUser.EMailID
                };

                if (_AuthMechanism == Constants.ApplicationAuthentication.DBAuthhentication)
                    _oUserProfile.Password = oUser.Password;

                _oUserProfile = oUserDAL.LoginUser(_oUserProfile);
                return _oUserProfile;

            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception ex)
            {
                ExceptionHandling.LogException(ex, _ClassCode + "001");

                return null;
            }
        }
        
        public UserProfile ValidateUserSession(UserProfile oUser)
        {

            try
            {

                _objServiceClient = new SingleSignOnDBSinkClient();

                ValidateUserLogin objValidateUserLogin = new ValidateUserLogin
                {
                    Apps_MST_Application_ID = 2,
                    Apps_MST_User_ID = oUser.SSOUserID,
                    ClientCookieIdentifier = oUser.CookieIdentifier,
                    ClientMAC = oUser.ClientMAC,
                    LastRequestTimestamp = oUser.LastRequestTimestamp.ToString(),
                    SessionIdentifier = oUser.SessionIdentifier
                };

                string json = JsonConvert.SerializeObject(objValidateUserLogin);


                SingleSignOnDBSinkClient obj = new SingleSignOnDBSinkClient();
                int output = obj.ValidateUserSession(json);

                if (output <= 0)
                    oUser.IsLoggedIn = false;

                oUser.LastRequestTimestamp = DateTime.UtcNow.ToString();
                oUser.IsLoggedIn = true;
                return oUser;

            }
            catch (Exception ex)
            {
                ExceptionHandling.LogException(ex, _ClassCode + "002");

                return null;
            }
        }

        public bool UserLogout(UserProfile oUser)
        {

            try
            {
                SSOService.SingleSignOnDBSinkClient obj = new SingleSignOnDBSinkClient();
                obj.EndUserSession(oUser.CookieIdentifier, oUser.SessionIdentifier);
                
                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandling.LogException(ex, _ClassCode + "003");

                return false;
            }
        }


    }


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
            ErrorCode = (string)jUser["ErrorCode"];
        }

    }
}
