using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MRLWMSC21Common;
using System.Data;
using Newtonsoft.Json;
using System.Globalization;

namespace MRLWMSC21WCF.BL
{
    public class LoginHandler
    {
        public FalconHHTSData.LoginUserData GetUserDetails(String UserName, String Password, String LoginIPAddress, BO.ValidateUserLogin objValidateUserLogin)
        {
            FalconHHTSData.LoginUserData thisUserDetails = new FalconHHTSData.LoginUserData();
            if(CommonLogic.Application("AppAuth")=="SSO")
            {
                DataSet ds = DB.GetDS("select * from gen_user where Email=" + DB.SQuote(UserName), false);
                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count != 0)
                {

                    SessionValidator.SingleSignOnDBSinkClient ssvalidator = new SessionValidator.SingleSignOnDBSinkClient();
                    objValidateUserLogin.LastRequestTimestamp = DateTime.Now.ToString();
                    string json = JsonConvert.SerializeObject(objValidateUserLogin);
                    thisUserDetails.Result = ssvalidator.ValidateUserLogin(json);
                    if (thisUserDetails.Result != null)
                    {
                        BO.ValidateUserLogin login = JsonConvert.DeserializeObject<BO.ValidateUserLogin>(thisUserDetails.Result.Substring(1, thisUserDetails.Result.Length - 2));


                        if (login.ErroCode != null)
                            switch (login.ErroCode.ToString())
                            {
                                case "-1":
                                    thisUserDetails.Result = "The email id you have entered is invalid.";

                                    break;
                                case "-2":
                                    thisUserDetails.Result = "You have exceeded the concurrent user limit as per your subscription plan.";

                                    break;
                                case "-3":
                                    string dt = DateTime.Parse(login.SubscriptionEndDate).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
                                    thisUserDetails.Result = "Your account subscription has expired on ";
                                    thisUserDetails.Result = thisUserDetails.Result + dt;
                                    break;
                                case "-4":
                                    thisUserDetails.Result = "The Password you have entered is Invalid.";
                                    break;
                            }
                        thisUserDetails.SessionIdetifier = login.SessionIdentifier;
                        thisUserDetails.CleintIdetifier = login.ClientCookieIdentifier;

                        string _sAPPS_MST_User_ID = login.Apps_MST_User_ID.ToString().Trim();
                        if (thisUserDetails.Login(_sAPPS_MST_User_ID, EnryptString(Password), LoginIPAddress))
                        {
                            Audit.LogInAudit(thisUserDetails.UserID.ToString(), LoginIPAddress, thisUserDetails.TenantID);
                            thisUserDetails.Result = "0";
                        }
                    }

                }
                else
                {
                    thisUserDetails.Result = "Not a valid credentails";
                }
            }
            else
            {
                if (thisUserDetails.Login(UserName, EnryptString(Password), LoginIPAddress))
                {
                    Audit.LogInAudit(thisUserDetails.UserID.ToString(), LoginIPAddress, thisUserDetails.TenantID);
                    thisUserDetails.Result = "0";
                }
                else
                {
                    thisUserDetails.Result = "Not a valid credentails";
                }
            }
          
            return thisUserDetails;
        }
        public string EnryptString(string strEncrypted)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);
            string encrypted = Convert.ToBase64String(b);
            return encrypted;
        }
    }
}