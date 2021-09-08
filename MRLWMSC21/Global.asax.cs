using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using MRLWMSC21;
using MRLWMSC21Common;
using System.Text;
using System.Collections;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.Protocols;
using System.Configuration.Provider;
using System.Threading;
using System.Security.Principal;
using System.Diagnostics;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MRLWMSC21.Generic_Class.Class;
using Newtonsoft.Json;

namespace MRLWMSC21
{
    [Serializable()]
    public class Global : HttpApplication
    {

        private THHTWSData.LoginUserData thisUser;

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup


            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterOpenAuth();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //RouteTable.Routes.MapPageRoute(
            //   "Login",
            //   "Login",
            //   "~/Login.aspx");
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

            try
            {
                /*
                string sKey = (string)Session["user"];

                Session.Remove(sKey);
                HttpContext.Current.Cache.Remove(sKey);*/
            }
            catch (Exception ex)
            {
                Response.Redirect("Login.aspx");
            }

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
            CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
            Exception ex = Server.GetLastError();

            if (cp == null)
            {
                Response.Redirect("~/Login.aspx");
            }

            if (ex is HttpRequestValidationException)
            {
                Response.Clear();
                Response.StatusCode = 200;
                Response.Write(@"[html]");
                Response.End();

                Response.Redirect("Login.aspx");
            }

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            //  LoadWindowsUserCredentials();
            // Extract the forms authentication cookie
            //string cookieName = FormsAuthentication.FormsCookieName;
            //HttpCookie authCookie = Context.Request.Cookies[cookieName];
            HttpCookie authCookie = null;
            HttpCookie authCookieValue = null;
            if (Request.Cookies["WMSCustomPrinciple"] != null)
            { 
                string UserCookie = Request.Cookies["WMSCustomPrinciple"].Value.Replace("%0d%0a", "");
                ValidateUser objValidateUser = JsonConvert.DeserializeObject<ValidateUser>(UserCookie);
                if(objValidateUser!=null)
                { 
                authCookie = new HttpCookie("WMSCustomPrinciple", Newtonsoft.Json.JsonConvert.SerializeObject(objValidateUser.UserCookie.Name));
                authCookieValue = new HttpCookie("WMSCustomPrinciple", Newtonsoft.Json.JsonConvert.SerializeObject(objValidateUser.UserCookie.Value));
                }
            }
            if (null == authCookie)
            {
                // There is no authentication cookie.
                return;
            }

            FormsAuthenticationTicket authTicket = null;
            try
            {
                string key = authCookieValue.Value.Trim(new char[] { '"' });
                authTicket = FormsAuthentication.Decrypt(key);
            }
            catch (Exception ex)
            {
                // Log exception details (omitted for simplicity)
                return;
            }

            if (null == authTicket)
            {
                // Cookie failed to decrypt.
                return;
            }
            // When the ticket was created, the UserData property was assigned a
            // comma delimited string of role names.


            try
            {

                thisUser = (THHTWSData.LoginUserData)XmlCommon.DeSerializeAnObject(authTicket.UserData);

                //string[] roles = authTicket.UserData.Split(',');
                string[] roles = CommonLogic.FilterSpacesInArrElements(thisUser.Roles.Split(','));
                string[] warehouses = CommonLogic.FilterSpacesInArrElements(thisUser.Warehouses.Split(','));
                //string[] sitecodes =  thisUser.SiteCodes != "" ? thisUser.SiteCodes.Split(',') : null ;
                string[] sitecodes = CommonLogic.FilterSpacesInArrElements(thisUser.SiteCodes.Split(','));


                // Create an Identity object
                FormsIdentity id = new FormsIdentity(authTicket);

                // This principal will flow throughout the request.
                CustomPrincipal principal = new CustomPrincipal(id, thisUser.TenantID, thisUser.TenantName, thisUser.UserGUID, thisUser.UserID, thisUser.FirstName, thisUser.LastName, thisUser.Email, roles, warehouses, sitecodes, thisUser.AccountID, thisUser.UserTypeID, thisUser.Account);
                // Attach the new principal object to the current HttpContext object
                Context.User = principal;
            }
            catch (Exception ex)
            {

                Response.Redirect("Login.aspx");
            }

        }


        /* protected void Application_PreRequestHandlerExecute(Object sender, EventArgs e)
         {
             if (HttpContext.Current.Session != null)
             {
                 if (Session["username"] != null)
                 {
                     string cacheKey = Session["username"].ToString();
                     if ((string)HttpContext.Current.Cache[cacheKey] != Session.SessionID)
                     {
                         FormsAuthentication.SignOut();
                         Session.Abandon();
                         Response.Redirect("~/Login.aspx");
                     }

                     string user = (string)HttpContext.Current.Cache[cacheKey];
                 }
             }
         }*/


        protected void Application_PreRequestHandlerExecute(Object sender, EventArgs e)
        {
            /*
            if (HttpContext.Current.Session != null)
            {
                if (Session["user"] != null) // e.g. this is after an initial logon
                {
                    string sKey = (string)Session["user"];
                    string sUser = (string)HttpContext.Current.Cache[sKey];
                }
            }*/


        }

        protected void LoadWindowsUserCredentials()
        {
            try
            {

                bool ClearLogin = false;

                String EMailField = "";
                String PasswordField = "";

                IDataReader rs;

                String SqlStr = "";

                if (EMailField == "")
                {

                    // get login user name
                    String WinLoginUserName = WindowsPrincipal.Current.Identity.Name;

                    // establish domain context
                    PrincipalContext userDomain = new PrincipalContext(ContextType.Domain);

                    // find your user
                    UserPrincipal user = UserPrincipal.FindByIdentity(userDomain, WinLoginUserName);

                    EMailField = user.EmailAddress;

                    SqlStr = String.Format("dbo.[sp_GEN_CheckUserLogin] @Email='{1}', @Password='{2}'", DB.GetNoLock(), EMailField, CommonLogic.EncryptString(PasswordField));//Encrypt.EncryptData(CommonLogic.Application("EncryptKey"), PasswordField)); //Commented by MD.Prasad //

                }
                else
                {
                    SqlStr = String.Format("dbo.[sp_GEN_CheckUserLogin] @Email='{1}', @Password='{2}'", DB.GetNoLock(), EMailField, CommonLogic.EncryptString(PasswordField));//Encrypt.EncryptData(CommonLogic.Application("EncryptKey"), PasswordField)); //Commented by MD.Prasad //
                }

                rs = DB.GetRS(SqlStr);

                ClearLogin = rs.Read();

                if (!ClearLogin)
                {
                    rs.Close();

                }
                else
                {

                    // string strHostName = System.Net.Dns.GetHostName();
                    //string clientIPAddress = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();

                    String IP = CommonLogic.ServerVariables("HTTP_X_FORWARDED_FOR");
                    if (IP == String.Empty)
                        IP = CommonLogic.ServerVariables("REMOTE_ADDR");
                    else
                        IP = CommonLogic.ServerVariables("REMOTE_HOST");


                    THHTWSData.LoginUserData thisUser = new THHTWSData.LoginUserData(DB.RSFieldGUID(rs, "UserGUID")
                                                                                       , DB.RSFieldInt(rs, "UserID")
                                                                                       , DB.RSFieldInt(rs, "TenantID")
                                                                                       , DB.RSField(rs, "TenantName")
                                                                                       , DB.RSField(rs, "FirstName")
                                                                                       , DB.RSField(rs, "LastName")
                                                                                       , DB.RSField(rs, "Email")
                                                                                       , DB.RSField(rs, "Roles")
                                                                                       , DB.RSField(rs, "Warehouses")
                                                                                       , DB.RSField(rs, "Zones")
                                                                                       , DB.RSField(rs, "Departments")
                                                                                       , DB.SQuote(IP));

                    rs.Close();

                    // Create the authentication ticket

                    String udata = XmlCommon.SerializeObject(thisUser);

                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1
                                                                                          , thisUser.Email
                                                                                          , DateTime.Now
                                                                                          , DateTime.Now.AddMinutes(60)
                                                                                          , false
                                                                                          , udata
                                                                                          );

                    // Now encrypt the ticket.
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    // Create a cookie and add the encrypted ticket to the
                    // cookie as data.
                    HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    // Add the cookie to the outgoing cookies collection.
                    Response.Cookies.Add(authCookie);


                    // Redirect the user to the originally requested page
                    if (CheckIfUserLoggedIn(thisUser.UserGUID))
                    // if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        // Response.Redirect(FormsAuthentication.GetRedirectUrl(thisUser.FirstName, false));
                        // lblStatus.Text = "User Already logged";
                        Response.Redirect("Default.aspx?logstatus=logged", false);
                        //Response.Redirect("mReports/Default.aspx?logstatus=logged", false);

                    }
                    else
                    {
                        Response.Redirect(FormsAuthentication.GetRedirectUrl(thisUser.FirstName, false), false);
                    }



                }
            }
            catch (Exception ex)
            {

            }

        }


        private Boolean CheckIfUserLoggedIn(Guid UserGUID)
        {
            //validate your user here (Forms Auth or Database, for example)
            // this could be a new "illegal" logon, so we need to check
            // if these credentials are already in the Cache 
            string sKey = UserGUID.ToString();
            string sUser = "";// Convert.ToString(Cache[sKey]);
            if (sUser == null || sUser == String.Empty)
            {
                // No Cache item, so sesion is either expired or user is new sign-on
                // Set the cache item and Session hit-test for this user---
                TimeSpan SessTimeOut = new TimeSpan(0, 0, HttpContext.Current.Session.Timeout, 0, 0);
                HttpContext.Current.Cache.Insert(sKey, sKey, null, DateTime.MaxValue, SessTimeOut, System.Web.Caching.CacheItemPriority.NotRemovable, null);
                //Session["user"] = TextBox1.Text + TextBox2.Text;
                // Let them in - redirect to main page, etc.
                //Label1.Text = "<Marquee><h1>Welcome!</h1></marquee>";
                return true;

            }
            else
            {
                // cache item exists, so too bad... 
                //Label1.Text = "<Marquee><h1><font color=red>ILLEGAL LOGIN ATTEMPT!!!</font></h1></marquee>";
                return true;
            }
        }


    }
}
