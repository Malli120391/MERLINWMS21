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
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.Protocols;
using System.Configuration.Provider;
using System.Threading;
using System.Security.Principal;
using System.Diagnostics;
using System.Net;

using MRLWMSC21.Generic_Class.Interface;

namespace MRLWMSC21.Generic_Class.Class
{
    [Serializable()]
    [System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.LinkDemand, Name = "FullTrust")]
    public sealed class FormAuthTicket : IFormAuthTicket
    {

        public ValidateUser ValidateUserWithSSOIntegration(string Apps_MST_User_ID, bool Rememberme, string ClientIdentifier, string SessionIdentifier, string LoginTimeStamp, int SessionTimeout, string Password)
        {
            try
            {
                bool ClearLogin = false;
                IDataReader rs;
                String SqlStr = "";
                ValidateUser objValidateUser = new Class.ValidateUser();
                SqlStr = String.Format("dbo.[sp_GEN_CheckUserLogin] @SSO_Apps_MST_User_ID = " + Apps_MST_User_ID + ",@Password='" + Password + "'");
                rs = DB.GetRS(SqlStr);
                ClearLogin = rs.Read();
                if (!ClearLogin)
                {
                    objValidateUser = null;
                }
                else
                {

                    FormAuthTicket objFormAuthTicket = new FormAuthTicket();
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
                                                                                    , DB.SQuote(IP)
                                                                                    , DB.RSFieldInt(rs, "AccountID")
                                                                                    , DB.RSFieldInt(rs, "UserTypeID")
                                                                                    , DB.RSField(rs, "Account"));

                    String udata = XmlCommon.SerializeObject(thisUser);

                    objValidateUser.UserProfiler = new UserProfile
                    {
                        UserGUID = thisUser.UserGUID,
                        UserID = thisUser.UserID,
                        TenantID = thisUser.TenantID,
                        TenantName = thisUser.TenantName,
                        FirstName = thisUser.FirstName,
                        LastName = thisUser.LastName,
                        Email = thisUser.Email,
                        Roles = thisUser.Roles,
                        Warehouses = thisUser.Warehouses,
                        DepartmentIDs = thisUser.Warehouses,
                        MachineIPAddress = IP,
                        SSOUserID = DB.RSFieldInt(rs, "SSOUserID")
                    };

                    //objValidateUser.UserCookie = objFormAuthTicket.LoginFormTicket(1, thisUser.Email, SessionTimeout, udata);

                    rs.Close();

                }
                return objValidateUser;
            }
            catch (Exception ex) { return null; }


        }

        //========================================== New Method For SQL SERVER FOR State Management, Created By PriyaDarsini ================================//
        public ValidateUser ValidateUserWithSSOIntegration_WOForms(string Apps_MST_User_ID, bool Rememberme, string ClientIdentifier, string SessionIdentifier, string LoginTimeStamp, int SessionTimeout, string Password)
        {
            try               
            {
                bool ClearLogin = false;
                IDataReader rs;
                String SqlStr = "";
                ValidateUser objValidateUser = new Class.ValidateUser();
                SqlStr = String.Format("dbo.[sp_GEN_CheckUserLogin] @SSO_Apps_MST_User_ID = " + Apps_MST_User_ID + ",@Password='" + Password + "'");
                rs = DB.GetRS(SqlStr);
                ClearLogin = rs.Read();
                if (!ClearLogin)
                {
                    objValidateUser = null;
                }
                else
                {

                    FormAuthTicket objFormAuthTicket = new FormAuthTicket();
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
                                                                                    , DB.SQuote(IP)
                                                                                    , DB.RSFieldInt(rs, "AccountID")
                                                                                    , DB.RSFieldInt(rs, "UserTypeID")
                                                                                    , DB.RSField(rs, "Account"));

                    String udata = XmlCommon.SerializeObject(thisUser);

                    objValidateUser.UserProfiler = new UserProfile
                    {
                        UserGUID = thisUser.UserGUID,
                        UserID = thisUser.UserID,
                        TenantID = thisUser.TenantID,
                        TenantName = thisUser.TenantName,
                        FirstName = thisUser.FirstName,
                        LastName = thisUser.LastName,
                        Email = thisUser.Email,
                        Roles = thisUser.Roles,
                        Warehouses = thisUser.Warehouses,
                        DepartmentIDs = thisUser.Warehouses,
                        MachineIPAddress = IP,
                        SSOUserID = DB.RSFieldInt(rs, "SSOUserID")
                    };

                    objValidateUser.UserCookie = objFormAuthTicket.LoginFormTicket(1, thisUser.Email, 20, udata, DateTime.Now);

                    rs.Close();

                }
                return objValidateUser;
            }
            catch (Exception ex) { return null; }


        }

        public ValidateUser ValidateUser(string User, string Password, bool Rememberme, string ClientIdentifier, string SessionIdentifier, string LoginTimeStamp)
        {
            try
            {
                bool ClearLogin = false;
                IDataReader rs;
                String SqlStr = "";
                ValidateUser objValidateUser = new Class.ValidateUser();
                SqlStr = String.Format("dbo.[sp_GEN_CheckUserLogin] @Email='{1}', @Password='{2}'", DB.GetNoLock(), User, CommonLogic.EncryptString(Password)); //Encrypt.EncryptData(CommonLogic.Application("EncryptKey"), Password)); Commented by MD.Prasad//
                rs = DB.GetRS(SqlStr);
                ClearLogin = rs.Read();
                if (!ClearLogin)
                {
                    objValidateUser = null;
                }
                else
                {

                    FormAuthTicket objFormAuthTicket = new FormAuthTicket();
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
                                                                                    , DB.SQuote(IP)
                                                                                    , DB.RSFieldInt(rs, "AccountID")
                                                                                    , DB.RSFieldInt(rs, "UserTypeID")
                                                                                    , DB.RSField(rs, "Account"));


                    String udata = XmlCommon.SerializeObject(thisUser);

                    objValidateUser.UserProfiler = new UserProfile
                    {
                        UserGUID = thisUser.UserGUID,
                        UserID = thisUser.UserID,
                        TenantID = thisUser.TenantID,
                        TenantName = thisUser.TenantName,
                        FirstName = thisUser.FirstName,
                        LastName = thisUser.LastName,
                        Email = thisUser.Email,
                        Roles = thisUser.Roles,
                        Warehouses = thisUser.Warehouses,
                        DepartmentIDs = thisUser.Warehouses,
                        MachineIPAddress = IP,
                        SSOUserID = DB.RSFieldInt(rs, "SSOUserID")
                    };
                    objValidateUser.UserCookie = objFormAuthTicket.LoginFormTicket(1, thisUser.Email, 20, udata, DateTime.Now);

                    rs.Close();

                }
                return objValidateUser;
            }
            catch (Exception ex) { return null; }


        }


        private HttpCookie LoginFormTicket(int version, string user, int setTimeout, string udata)
        {
            // Success, create non-persistent authentication cookie.

            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1
                                                                                 , user
                                                                                 , DateTime.Now
                                                                                 , DateTime.Now.AddMinutes(setTimeout)
                                                                                 , false
                                                                                 , udata
                                                                                 );


            HttpCookie InvAuthCookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket))
            {
                Secure = false,
                HttpOnly = false
            };
            return InvAuthCookie;
        }


        private MyCookie LoginFormTicket(int version, string user, int setTimeout, string udata, DateTime TimeStamp)
        {
            // Success, create non-persistent authentication cookie.

            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1
                                                                                 , user
                                                                                 , TimeStamp
                                                                                 , DateTime.Now.AddMinutes(setTimeout)
                                                                                 , false
                                                                                 , udata
                                                                                 );

            MyCookie InvAuthCookie = new MyCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));

            //HttpCookie InvAuthCookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket))
            //{
            //    Secure = false,
            //    HttpOnly = false
            //};
            return InvAuthCookie;
        }



    }

    [Serializable()]
    public sealed class ValidateUser
    {
        public UserProfile UserProfiler
        {
            get; set;
        }

        public MyCookie UserCookie
        {
            get; set;
        }

    }


    [Serializable()]
    public sealed class UserProfile
    {

        public int TenantID
        {
            get; set;
        }

        public string TenantName
        {
            get; set;
        }

        public Guid UserGUID
        {
            get; set;
        }

        public int UserID
        {
            get; set;
        }
        public int SSOUserID
        {
            get; set;
        }
        public string FirstName
        {
            get; set;
        }

        public string LastName
        {
            get; set;
        }

        public string Email
        {
            get; set;
        }
        public string Password
        {
            get; set;
        }

        public string Roles
        {
            get; set;
        }

        public string Warehouses
        {
            get; set;
        }

        public string SiteCodes
        {
            get; set;
        }

        public string DepartmentIDs
        {
            get; set;
        }

        public string MachineIPAddress
        {
            get; set;
        }

        public string Zones
        {
            get; set;
        }
      

    }

    [Serializable()]
    public sealed class MyCookie
    {
        private string name;

        private string value;

        public string Name { get => name; set => name = value;}
        public string Value { get => value; set => this.value = value; }

        public MyCookie(string CookieName, string CookieValue)
        {
            name = CookieName;
            value = CookieValue;
        }
    }
}