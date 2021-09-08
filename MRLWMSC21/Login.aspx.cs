using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Security;
using MRLWMSC21Common;
using System.Text;
using System.Collections;
using System.Configuration.Provider;
using System.Threading;
using System.Security.Principal;
using System.Diagnostics;
using System.Net;
using MRLWMSC21.Generic_Class.Class;
using System.Net.NetworkInformation;
using MRLWMSC21.Generic_Class;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

using MRLWMSC21.ServiceReference1;
using Newtonsoft.Json;
using System.Globalization;

namespace MRLWMSC21
{
    public partial class Login : System.Web.UI.Page
    {
        ServiceReference1.SingleSignOnDBSinkClient objServiceClient = new ServiceReference1.SingleSignOnDBSinkClient();

        string ssoTargetURL = System.Configuration.ConfigurationManager.AppSettings["ssoTagetURL"].ToString();
        string merlinWMSAppID = System.Configuration.ConfigurationManager.AppSettings["merlinWMSAppID"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            string dt = CommonLogic.DecryptString("ODg5Mg==");
            if (!IsPostBack)
            {
                bool status = Request.QueryString.AllKeys.Contains("XYZPLXYZ000");
                if (status == true)
                {

                    string dp = CommonLogic.DecryptString(Request.QueryString["XYZPLXYZ000"].ToString());
                    DataSet ds = DB.GetDS("SELECT * FROM GEN_User WHERE SSOUserID=" + Convert.ToInt32(dp), false);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        //verifyLogin(ds.Tables[0].Rows[0]["Email"].ToString(), "123");
                        verifyLogin(ds.Tables[0].Rows[0]["Email"].ToString(), ds.Tables[0].Rows[0]["Password"].ToString());
                    }
                }
            }

            //if (!IsPostBack)
            //{
            //    if (Request.QueryString["message"] == "failure")
            //    {
            //        //Response.Write("Please validate your credentials!!");
            //        string data = "Please validate your credentials!!";
            //        //Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "showAlert('" + data + "');", true);
            //        lblError.Text = data;
            //    }
            //    else if (Request.QueryString["message"] == "success")
            //    {

            //        string data = "SSO Validated." + "Welcome!! " + Request.QueryString["username"].Split(',')[0];
            //        lblError.Text = data;
            //        //Thread.Sleep(500);

            //        //Commented By Suresh as per new DurgaPrasad INv_SSO Validation is already done on 01 Dec 2020
            //        verifyLogin(Request.QueryString["username"].Split(',')[0], Request.QueryString["username"].Split(',')[1]);
            //        //if (!IsPostBack)
            //        //{

            //    }
            //    else
            //    {
            //        Response.Redirect(ssoTargetURL + merlinWMSAppID);
            //    }
                
            //}

      
            if (!IsPostBack)
            {

                if (CommonLogic.QueryString("FP") != null && CommonLogic.QueryString("FP") != "")
                {

                    if (CommonLogic.QueryString("SE") != null)
                    {
                        lblError.Text = "Sorry! Your session has expired. Please login once again.";
                        return;
                    }
                    else
                        lblError.Text = "";

                    if (CommonLogic.QueryString("eid") != null)
                    {
                        switch (CommonLogic.QueryString("eid"))
                        {

                            case "2":
                                ResetError("Please login to access this functionality.", true);
                                return;

                            case "3":
                                ResetError("Sorry, you do not have access to this functionality.Please login with correct credentials", true);
                                return;

                            case "5":
                                ResetError("Your new password is updated.Please use your new password to login.", true);
                                return;

                            case "6":

                                ResetError("Sorry, you do not have access to this functionality.Please contact the  support center to assign a role to this credential.", true);
                                return;
                        }
                    }

                    if (Request.QueryString["FL"] == null)
                        validateUserSession();

                        try
                        {
                            txtEmailID.Attributes.Add("autocomplete", "off");
                            txtPassword.Attributes.Add("autocomplete", "off");

                            if (Request.UrlReferrer != null)
                                Session["ReferedURL"] = Request.UrlReferrer.ToString();

                            if (Request.Browser.Cookies)
                            {
                                if (Request.Cookies["InvWMSCookie"] != null)
                                {
                                    HttpCookie getCookie = Request.Cookies.Get("InvWMSCookie");
                                    txtEmailID.Text = getCookie.Values["UserName"].ToString();
                                    txtPassword.Text = DB.GetSqlS("select Password AS S from GEN_User where EnPassword='" + getCookie.Values["Password"].ToString() + "'");
                                    // txtPassword.Attributes.Add("value", txtPassword.Text);
                                    chkRemember.Checked = true;
                                }
                            }

                            txtEmailID.Attributes["onclick"] = "javascript:this.focus();";
                            txtEmailID.Attributes["onfocus"] = "javascript:this.select();";
                            txtPassword.Attributes["onclick"] = "javascript:this.focus();";
                            txtPassword.Attributes["onfocus"] = "javascript:this.select();";
                        }
                        catch (Exception ex)
                        {
                           // WriteToFile("Exception2 : end" + ex.ToString());
                            // ResetError("Error while loading user information", true);
                            return;
                        }
                }
            }
            if (CommonLogic.QueryString("AccountID") != null && CommonLogic.QueryString("AccountID") != "")
            {
                ServiceReference1.SingleSignOnDBSinkClient obj = new SingleSignOnDBSinkClient();
                string AccountiD = CommonLogic.QueryString("AccountID");
                //string ActivationLogin = "http://192.168.1.29/WMSService/Service/SingleSignOnDBSink.svc/LoginActivation/" + CommonLogic.QueryString("AccountId");
                string result = obj.LoginActivation(AccountiD);
            }


        }



        protected void btnLogin_Click(object sender, EventArgs e)
        {
            // Methood modified by Aditya Srinivas on 22nd Feb 2018. To Facilitate Login with SSO & Forms.

            try
            {
                this.Page.Validate("valLogin");
                if (!IsValid)
                {
                    return;
                };

                verifyLogin(txtEmailID.Text, txtPassword.Text);
                //Response.Redirect(ssoTargetURL + merlinWMSAppID + "&username=" + CommonLogic.EncryptString(txtEmailID.Text) + "&pwd=" + CommonLogic.EncryptString(txtPassword.Text.Trim()), false);

            }
            catch (Exception ex)
            {
                throw ex;

            }


        }

        private void verifyLogin(string UserName , string Password)
        {
            try

            {               
                string _sAuth = System.Configuration.ConfigurationManager.AppSettings["AppAuth"];
                if (_sAuth != null) // Condition to check if Authentication key is with SSO, then SSO, otherwise Local DB Authentication. 
                    if (_sAuth.ToString().Equals("SSO"))
                    {
                        txtEmailID.Text = UserName;
                        txtPassword.Text = Password;
                        // SSO Authentication

                        FormAuthTicket objFormAuthTicket = new FormAuthTicket();

                        String IP = CommonLogic.ServerVariables("HTTP_X_FORWARDED_FOR");

                        String EMailField = txtEmailID.Text;
                        String PasswordField = txtPassword.Text;

                        string EncNewPassword = CommonLogic.EncryptString(PasswordField);//Encrypt.EncryptData(CommonLogic.Application("EncryptKey"), PasswordField); Commented by MD.Prasad//

                        ValidateUserLogin objValidateUserLogin = new ValidateUserLogin();
                        objValidateUserLogin.Apps_MST_Application_ID = 2;
                        objValidateUserLogin.ClientMAC = GetMac();
                        objValidateUserLogin.ClientCookieIdentifier = SetClientGuid("WMSClientID", false);
                        objValidateUserLogin.EMail = EMailField;
                        objValidateUserLogin.SessionIdentifier = SetClientGuid("WMSSessionID", false);
                        objValidateUserLogin.Password = EncNewPassword;
                        SetClientGuid("WMSLoginTimeStamp", true);
                        string json = JsonConvert.SerializeObject(objValidateUserLogin);
                        string UserResult = objServiceClient.ValidateUserLogin(json);
                        // UserResult = "ValidateLoginResponse:" + UserResult;

                        UserResult = "{\"ValidateLoginResponse\": " + UserResult.Remove(0, 1).Replace("]", "") + "}";
                        ValidateLoginResponse _Response = new ValidateLoginResponse(UserResult);

                        // JsonConvert.DeserializeObject<ValidateLoginResponse>(UserResult);

                        string _sErrorMessage = string.Empty;
                        bool _bHasError = false;

                        if (_Response.ErrorCode != null)
                            switch (_Response.ErrorCode.ToString())
                            {
                                case "-1":
                                    _sErrorMessage = "The email id you have entered is invalid.";
                                    _bHasError = true;
                                    break;
                                case "-2":
                                    _sErrorMessage = "You have exceeded the concurrent user limit as per your subscription plan.";
                                    _bHasError = true;
                                    break;
                                case "-3":
                                    string dt = DateTime.Parse(_Response.SubscriptionEndDate).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
                                    _sErrorMessage = "Your account subscription has expired on ";
                                    _sErrorMessage = _sErrorMessage + dt;
                                    _bHasError = true;
                                    break;
                                case "-4":
                                    _sErrorMessage = "The Password you have entered is Invalid.";
                                    _bHasError = true;
                                    break;
                                case "-99":
                                    _sErrorMessage = "Your account has been Cancelled.";
                                    _bHasError = true;
                                    break;
                            }

                        if (_bHasError)
                        {
                            //  ResetError(_sErrorMessage, true);

                            lblError.Text = _sErrorMessage;
                            return;
                        }

                        string _sAPPS_MST_User_ID = _Response.Apps_MST_User_ID.ToString().Trim();//_Response.UserProfiler.SSOUserID.ToString();
                        int _iSessionTimeout = Convert.ToInt32(_Response.SessionTimeoutTime.ToString());

                        SetClientSSOUserID("APPSUID", _sAPPS_MST_User_ID);

                        //ValidateUser objValidateUser = objFormAuthTicket.ValidateUserWithSSOIntegration(_sAPPS_MST_User_ID, chkRemember.Checked, null, null, null, _iSessionTimeout, EncNewPassword);

                        ValidateUser objValidateUser = objFormAuthTicket.ValidateUserWithSSOIntegration_WOForms(_sAPPS_MST_User_ID, chkRemember.Checked, null, null, null, _iSessionTimeout, EncNewPassword);



                        // =========== commented by M.D.Prasad =============
                        //if (objValidateUser.UserCookie == null)
                        //HttpCookie InvAuthCookie = objValidateUser.UserCookie;
                        //Response.Cookies.Add(InvAuthCookie);
                        ////Remember Me feature
                        //RememberMe();


                        string sData = Newtonsoft.Json.JsonConvert.SerializeObject(objValidateUser);

                        //string _EncryptedCookie = Encrypt.EncryptData(CommonLogic.Application("EncryptKey").ToString(), sData);

                        // HttpCookie InvAuthCookie = new HttpCookie("WMSCustomPrinciple", _EncryptedCookie/*Newtonsoft.Json.JsonConvert.SerializeObject(objValidateUser)*/);

                        HttpCookie InvAuthCookie = new HttpCookie("WMSCustomPrinciple", Newtonsoft.Json.JsonConvert.SerializeObject(objValidateUser));
                        Response.Cookies.Add(InvAuthCookie);

                        Session["UserProfile"] = objValidateUser;
                        // =========== commented by M.D.Prasad =============

                        if (objValidateUser == null)
                        {
                            //ResetError("Error logging in.Please check your UserID and Password.", true); // =========== commented by M.D.Prasad =============
                            lblError.Text = "Error logging in.Please check your UserID and Password.";
                            txtEmailID.Focus();
                        }
                        else
                        {
                            //HttpCookie InvAuthCookie = objValidateUser.UserCookie;
                            //Response.Cookies.Add(InvAuthCookie);
                            //Remember Me feature
                            //RememberMe();


                            Session["UserProfile"] = objValidateUser;


                            // Redirect the user to the originally requested page
                            if (CheckIfUserLoggedIn(objValidateUser.UserProfiler.UserGUID))
                            //if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                            {
                                // Response.Redirect(FormsAuthentication.GetRedirectUrl(thisUser.FirstName, false));
                                // lblStatus.Text = "User Already logged";
                                Response.Redirect("Default.aspx?logstatus=logged", false);
                                //Response.Redirect("mReports/Default.aspx?logstatus=logged", false);
                                //Audit.LogInAudit(Convert.ToString(objValidateUser.UserProfiler.UserID), objValidateUser.UserProfiler.MachineIPAddress, objValidateUser.UserProfiler.TenantID);
                            }
                            else
                            {
                                Response.Redirect("Login.aspx?FL=1", false);

                                //Response.Redirect(FormsAuthentication.GetRedirectUrl(objValidateUser.UserProfiler.FirstName, false), false);
                                //Audit.LogInAudit(Convert.ToString(objValidateUser.UserProfiler.UserID), objValidateUser.UserProfiler.MachineIPAddress, objValidateUser.UserProfiler.TenantID);
                            }




                        }

                    }
                    else
                    {

                        txtEmailID.Text = UserName;
                        txtPassword.Text = Password;
                        // Database Credential Validation, in case of no SSO availability.

                        FormAuthTicket objFormAuthTicket = new FormAuthTicket();
                        String EMailField = txtEmailID.Text;
                        String PasswordField = txtPassword.Text;
                        ValidateUser objValidateUser = objFormAuthTicket.ValidateUser(EMailField, PasswordField, chkRemember.Checked, null, HttpContext.Current.Session.SessionID.ToString(), null);


                        HttpCookie InvAuthCookie = new HttpCookie("WMSCustomPrinciple", Newtonsoft.Json.JsonConvert.SerializeObject(objValidateUser));
                        Response.Cookies.Add(InvAuthCookie);

                        // =========== commented by M.D.Prasad =============
                        //HttpCookie InvAuthCookie = objValidateUser.UserCookie;
                        //// InvAuthCookie.Name = "InvWMSAuthCookie";
                        //// Add the cookie to the outgoing cookies collection.
                        //Response.Cookies.Add(InvAuthCookie);
                        ////Remember Me feature
                        //RememberMe();
                        //if (objValidateUser.UserCookie == null)
                        // =========== commented by M.D.Prasad =============
                        if (objValidateUser == null)
                        {
                            //ResetError("Error logging in.Please check your UserID and Password.", true); =========== commented by M.D.Prasad =============
                            lblError.Text = "Error logging in.Please check your UserID and Password.";
                            txtEmailID.Focus();
                        }
                        else
                        {

                            //HttpCookie InvAuthCookie = new HttpCookie("WMSCustomPrinciple", Newtonsoft.Json.JsonConvert.SerializeObject(objValidateUser));
                            ////HttpCookie InvAuthCookie = new HttpCookie(objValidateUser.UserCookie.Name, objValidateUser.UserCookie.Value);
                            //// InvAuthCookie.Name = "InvWMSAuthCookie";
                            //// Add the cookie to the outgoing cookies collection.
                            //Response.Cookies.Add(InvAuthCookie);
                            //Remember Me feature
                            RememberMe();


                            // Redirect the user to the originally requested page
                            if (CheckIfUserLoggedIn(objValidateUser.UserProfiler.UserGUID))
                            //if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                            {
                                // Response.Redirect(FormsAuthentication.GetRedirectUrl(thisUser.FirstName, false));
                                // lblStatus.Text = "User Already logged";
                                Response.Redirect("Default.aspx?logstatus=logged", false);
                                //Response.Redirect("mReports/Default.aspx?logstatus=logged", false);
                               // Audit.LogInAudit(Convert.ToString(objValidateUser.UserProfiler.UserID), objValidateUser.UserProfiler.MachineIPAddress, objValidateUser.UserProfiler.TenantID);
                            }
                            else
                            {
                                Response.Redirect("Login.aspx?FL=1", false);
                                //Response.Redirect(FormsAuthentication.GetRedirectUrl(objValidateUser.UserProfiler.FirstName, false), false);
                                //Audit.LogInAudit(Convert.ToString(objValidateUser.UserProfiler.UserID), objValidateUser.UserProfiler.MachineIPAddress, objValidateUser.UserProfiler.TenantID);
                            }




                        }
                    }

            }
            catch (Exception ex)
            {


            }
        }






        //protected void btnLogin_Click(object sender, EventArgs e)
        //{

        //    //   setUserCredentials(sender);

        //    try
        //    {
        //        this.Page.Validate("valLogin");
        //        if (!IsValid)
        //        {

        //            return;
        //        };

        //        FormAuthTicket objFormAuthTicket = new FormAuthTicket();
        //        String EMailField = txtEmailID.Text;
        //        String PasswordField = txtPassword.Text;
        //        ValidateUser objValidateUser = objFormAuthTicket.ValidateUser(EMailField, PasswordField, chkRemember.Checked, null,null,null);

        //        if (objValidateUser.UserCookie == null)
        //        {
        //            ResetError("Error logging in.Please check your UserID and Password.", true);
        //            txtEmailID.Focus();
        //        }
        //        else
        //        {
        //            String IP = CommonLogic.ServerVariables("HTTP_X_FORWARDED_FOR");
        //            i = objValidateUser;


        //            HttpCookie InvAuthCookie = objValidateUser.UserCookie;
        //            // InvAuthCookie.Name = "InvWMSAuthCookie";
        //            // Add the cookie to the outgoing cookies collection.
        //            Response.Cookies.Add(InvAuthCookie);
        //            //Remember Me feature
        //            RememberMe();
        //            ValidateUserLogin objValidateUserLogin = new ValidateUserLogin();
        //            objValidateUserLogin.Apps_MST_Application_ID = 2;
        //            objValidateUserLogin.ClientMAC = GetMac();
        //            objValidateUserLogin.ClientCookieIdentifier = SetClientGuid("WMSClientID", false);
        //            objValidateUserLogin.EMail = EMailField;
        //            objValidateUserLogin.SessionIdentifier = SetClientGuid("WMSSessionID", false);
        //            objValidateUserLogin.Password = PasswordField;
        //            SetClientGuid("WMSLoginTimeStamp", true);
        //            string json = JsonConvert.SerializeObject(objValidateUserLogin);
        //            string UserResult = objServiceClient.ValidateUserLogin(json);

        //            // Redirect the user to the originally requested page
        //            if (CheckIfUserLoggedIn(objValidateUser.UserProfiler.UserGUID))
        //            //if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
        //            { 
        //                // Response.Redirect(FormsAuthentication.GetRedirectUrl(thisUser.FirstName, false));
        //                // lblStatus.Text = "User Already logged";
        //                Response.Redirect("Default.aspx?logstatus=logged", false);
        //                Audit.LogInAudit(Convert.ToString(objValidateUser.UserProfiler.UserID), objValidateUser.UserProfiler.MachineIPAddress, objValidateUser.UserProfiler.TenantID);
        //            }
        //            else
        //            {
        //                Response.Redirect(FormsAuthentication.GetRedirectUrl(objValidateUser.UserProfiler.FirstName, false), false);
        //                Audit.LogInAudit(Convert.ToString(objValidateUser.UserProfiler.UserID), objValidateUser.UserProfiler.MachineIPAddress, objValidateUser.UserProfiler.TenantID);
        //            }




        //        }
        //    }
        //    catch (Exception ex)
        //    {


        //    }


        //}


        [System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.LinkDemand, Name = "FullTrust")]
        private Boolean CheckIfUserLoggedIn(Guid UserGUID)
        {
            try
            {

                //validate your user here (Forms Auth or Database, for example)
                // this could be a new "illegal" logon, so we need to check
                // if these credentials are already in the Cache 
                //string sKey = UserGUID.ToString();
                //string sUser = Convert.ToString(Cache[sKey]);
                //if (sUser == null || sUser == String.Empty)
                //{
                //    // No Cache item, so sesion is either expired or user is new sign-on
                //    // Set the cache item and Session hit-test for this user---
                //    TimeSpan SessTimeOut = new TimeSpan(0, 0, HttpContext.Current.Session.Timeout, 0, 0);
                //    HttpContext.Current.Cache.Insert(sKey, sKey, null, DateTime.MaxValue, SessTimeOut, System.Web.Caching.CacheItemPriority.NotRemovable, null);
                //    //Session["user"] = TextBox1.Text + TextBox2.Text;
                //    // Let them in - redirect to main page, etc.
                //    //Label1.Text = "<Marquee><h1>Welcome!</h1></marquee>";
                //    return false;

                //}
                //else
                //{
                //    // cache item exists, so too bad... 
                //    //Label1.Text = "<Marquee><h1><font color=red>ILLEGAL LOGIN ATTEMPT!!!</font></h1></marquee>";
                //    return true;
                //}

                //[Changed on 11 Nov 2018]
                //Commented original code during change over from forms based authentication to storing sessions in sql server 
                //Modified code is as given below. Found that there is no significance in old code. 
                string sKey = UserGUID.ToString();
                string sUser = Convert.ToString(Cache[sKey]);
                if (sUser == null || sUser == String.Empty)
                {
                    // No Cache item, so sesion is either expired or user is new sign-on
                    // Set the cache item and Session hit-test for this user---
                    TimeSpan SessTimeOut = new TimeSpan(0, 0, HttpContext.Current.Session.Timeout, 0, 0);
                    HttpContext.Current.Cache.Insert(sKey, sKey, null, DateTime.MaxValue, SessTimeOut, System.Web.Caching.CacheItemPriority.NotRemovable, null);
                    return true;

                }
                else { return true; }

            }
            catch (Exception ex)
            {
                ResetError("Error while loading login information", true);
                return false;
            }
        }


        [System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.LinkDemand, Name = "FullTrust")]
        public bool setUserCredentials(Object sender)
        {
            bool logStatus = false;

            string userName = txtEmailID.Text;
            string passWord = txtPassword.Text;

            string sKey = txtEmailID.Text;
            string sUser = Convert.ToString(Cache[sKey]);
            if (sUser == null || sUser == String.Empty)
            {
                TimeSpan SessTimeOut = new TimeSpan(0, 0, HttpContext.Current.Session.Timeout, 0, 0);
                HttpContext.Current.Cache.Insert(sKey, sKey, null, DateTime.MaxValue, SessTimeOut, System.Web.Caching.CacheItemPriority.NotRemovable, null);
                Session["user"] = txtEmailID.Text.Trim();

                bool ClearLogin = false;

                IDataReader rs;

                String SqlStr = "";

                SqlStr = String.Format("dbo.[sp_GEN_CheckUserLogin] @Email='{1}', @Password='{2}'", DB.GetNoLock(), userName, CommonLogic.EncryptString(passWord)); //Encrypt.EncryptData(CommonLogic.Application("EncryptKey"), passWord));

                rs = DB.GetRS(SqlStr);

                ClearLogin = rs.Read();

                if (!ClearLogin)
                {
                    ResetError("Error logging in.Please check you UserID and Password.", true);
                    rs.Close();
                    txtEmailID.Focus();
                    return logStatus;
                }
                else
                {

                    if (userName.Equals(DB.RSField(rs, "Email")) && passWord.Equals(DB.RSField(rs, "Password")))
                    {
                        return logStatus;
                    }
                    else
                    {
                        return logStatus;
                    }

                }

            }
            else
            {

                logStatus = true;
                //ResetError("Invalid Login", true);
                return logStatus;
            }
        }




        protected void ResetError(string error, bool isError)
        {
            /*
            string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
            if (isError)
                str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

            if (error.Length > 0)
                str += error + "";
            else
                str = "";
            lblStatusMessage.Text = str;*/


            // ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);
            ClientScript.RegisterClientScriptBlock(this.GetType(), "ShowStickeyToastOnLogin", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");");

        }



        [System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.LinkDemand, Name = "FullTrust")]
        private void RememberMe()
        {

            if (chkRemember.Checked == true)
            {

                if (Request.Browser.Cookies)
                {
                    Response.Cookies.Remove("InvWMSCookie");
                    HttpCookie myCookie = new HttpCookie("InvWMSCookie")
                    {
                        Secure = true,
                        Expires = DateTime.Now.AddDays(30)
                    };
                    myCookie.HttpOnly = true;
                    Response.Cookies.Add(myCookie);
                    myCookie.Values.Add("UserName", txtEmailID.Text.ToString());
                    // myCookie.Values.Add("Password", DB.GetSqlS("select EnPassword AS S from GEN_User where Email='" + txtEmailID.Text.ToString() + "'"));
                    DateTime dtxpiry = DateTime.Now.AddDays(5);
                }
            }
            else
            {
                //Response.Cookies.Remove("InvWMSCookie");
                Response.Cookies["InvWMSCookie"].Expires = DateTime.Now.AddDays(-1);
            }
        }

        [System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.LinkDemand, Name = "FullTrust")]
        private void InvWMSCookie()
        {

            if (Request.Cookies["InvWMSCookie"] != null)
            {
                //HttpCookie getCookie = Request.Cookies.Get("InvWMSCookie");
                //txtEmailID.Text = getCookie.Values["UserName"].ToString();
                //txtPassword.Text = DB.GetSqlS("select Password AS S from GEN_User where EnPassword='" + getCookie.Values["Password"].ToString() + "'");
                //txtPassword.Attributes.Add("value", txtPassword.Text);
                //chkRemember.Checked = true;
            }
        }

        private string GetMac()
        {

            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            return sMacAddress;
        }
        private string SetClientGuid(string CookieName, bool TimeStamp)
        {
            if (TimeStamp == false)
            {
                Guid newGuid;
                newGuid = Guid.NewGuid();
                string name = CookieName;
                if (Request.Cookies[CookieName] != null)
                {
                    Response.Cookies[CookieName].Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies[CookieName].Value = Convert.ToString(newGuid);
                    Response.Cookies[CookieName].Expires = DateTime.Now.AddMinutes(20); // add expiry time
                }
                else
                {
                    Response.Cookies[CookieName].Value = Convert.ToString(newGuid);
                    Response.Cookies[CookieName].Expires = DateTime.Now.AddMinutes(20); // add expiry time
                }
                return Convert.ToString(newGuid);
            }
            else
            {
                if (Request.Cookies[CookieName] != null)
                {
                    Response.Cookies[CookieName].Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies[CookieName].Value = DateTime.UtcNow.ToString("yyyy-MM-dd HH':'mm':'ss");
                    Response.Cookies[CookieName].Expires = DateTime.Now.AddMinutes(60); // add expiry time
                }
                else
                {
                    Response.Cookies[CookieName].Value = DateTime.UtcNow.ToString("yyyy-MM-dd HH':'mm':'ss");
                    Response.Cookies[CookieName].Expires = DateTime.Now.AddMinutes(60); // add expiry time
                }
                return "";
            }


        }

        private void SetClientSSOUserID(string CookieName, string CookieValue)
        {
            Guid newGuid;
            newGuid = Guid.NewGuid();
            string name = CookieName;
            if (Request.Cookies[CookieName] != null)
            {
                Response.Cookies[CookieName].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies[CookieName].Value = CookieValue;
                Response.Cookies[CookieName].Expires = DateTime.Now.AddMinutes(60); // add expiry time
            }
            else
            {
                Response.Cookies[CookieName].Value = CookieValue;
                Response.Cookies[CookieName].Expires = DateTime.Now.AddMinutes(60); // add expiry time
            }

        }


        private void validateUserSession()
        {
            //if (Session["UserProfile"] != null)
            //{


            if ((Request.Cookies["WMSClientID"] != null && Request.Cookies["WMSSessionID"] != null
                    && Request.Cookies["WMSLoginTimeStamp"] != null && Request.Cookies["APPSUID"] != null))
            {
                string _sSSOUserID = string.Empty;
                //ValidateUser objValidateUser = (ValidateUser)Session["UserProfile"];
                string ClientIdentifier = null, SessionIdentifier = null, LastTimeStamp = null;
                if (Request.Cookies["WMSClientID"] != null)
                {
                    ClientIdentifier = Request.Cookies["WMSClientID"].Value;
                }
                if (Request.Cookies["WMSSessionID"] != null)
                {
                    SessionIdentifier = Request.Cookies["WMSSessionID"].Value;
                }
                if (Request.Cookies["APPSUID"] != null)
                {
                    _sSSOUserID = Request.Cookies["APPSUID"].Value;
                }
                if (Request.Cookies["WMSLoginTimeStamp"] != null)
                {
                    LastTimeStamp = Request.Cookies["WMSLoginTimeStamp"].Value;
                }
                ValidateUserLogin objValidateUserLogin = new ValidateUserLogin();
                objValidateUserLogin.Apps_MST_Application_ID = 2;
                objValidateUserLogin.Apps_MST_User_ID = Convert.ToInt32(_sSSOUserID.ToString()); //  objValidateUser.UserProfiler.SSOUserID;
                objValidateUserLogin.ClientCookieIdentifier = ClientIdentifier;
                objValidateUserLogin.ClientMAC = GetMac();
                objValidateUserLogin.LastRequestTimestamp = LastTimeStamp;
                objValidateUserLogin.SessionIdentifier = SessionIdentifier;
                string json = JsonConvert.SerializeObject(objValidateUserLogin);
                if (ClientIdentifier != null && SessionIdentifier != null)
                {
                    ServiceReference1.SingleSignOnDBSinkClient obj = new SingleSignOnDBSinkClient();
                    int output = obj.ValidateUserSession(json);
                    if (output <= 0)
                    //if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        // Response.Redirect("./Login.aspx");
                    }
                    else
                    {

                        //Response.Redirect("./mReports/Default.aspx?logstatus=logged", false);
                        Response.Redirect("Default.aspx?logstatus=logged", false);
                    }

                }
            }
           
        }

        protected void lnkForgotPassword_Click(object sender, EventArgs e)
        {
            // Server.Transfer("./ForgotPassword.aspx", false);
            Response.Redirect("./ForgotPassword.aspx", true);
        }

    }

}