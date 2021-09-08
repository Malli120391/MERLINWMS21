using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MRLWMSC21Common.Interfaces;
namespace MRLWMSC21Common
{
    public class FalconHHTSData
    {
        public FalconHHTSData() { }

       [Serializable]
        public sealed class LoginUserData:ILoginUserData
        {
           private int _UserID;
           private string _FirstName;
           private string _LastName;
           private string _Email;
           private string _Password;
           private string _roles;
           private string _WarehouseIDs;

           private string _SiteCodes;
           private string _DepartmentIDs;

           private string _MachineIPAddress;
           private int _TenantID;
           private string _Result;
           private string _Sessionidentifier;
           private string _Clientidentifier;
           private int _SsoId;
           private int _AccountId;
            public int AccountID
            {
                get { return _AccountId; }
                set { _AccountId = value; }
            }

            public int UserID
           {
               get { return _UserID; }
               set { _UserID = value; }
           }
            public int SSOID
            {
                get { return _SsoId; }
                set { _SsoId = value; }
            }

            public string Result
            {
                get { return _Result; }
                set { _Result = value; }
            }


            public string FirstName
           {
               get { return _FirstName; }
               set { _FirstName = value; }
           }

         
            public string SessionIdetifier
            {
                get { return _Sessionidentifier; }
                set { _Sessionidentifier = value; }
            }
            public string CleintIdetifier
            {
                get { return _Clientidentifier; }
                set { _Clientidentifier = value; }
            }




            public string LastName
           {
               get { return _LastName; }
               set { _LastName = value; }
           }

           public string Email
           {
               get { return _Email; }
               set { _Email = value; }
           }
           public string Password
           {
               get { return _Password; }
               set { _Password = value; }
           }

           public string Roles
           {
               get { return _roles; }
               set { _roles = value; }
           }

           public string Warehouses
           {
               get { return _WarehouseIDs; }
               set { _WarehouseIDs = value; }
           }

           public string SiteCodes
           {
               get { return _SiteCodes; }
               set { _SiteCodes = value; }
           }

           public string DepartmentIDs
           {
               get { return _DepartmentIDs; }
               set { _DepartmentIDs = value; }
           }

           public string MachineIPAddress
           {
               get { return _MachineIPAddress; }
               set { _MachineIPAddress = value; }
           }
           public int TenantID
           {
               get { return _TenantID; }
               set { _TenantID = value; }
           }


           public LoginUserData() { }


           public LoginUserData(int UserID, String FirstName, string LastName, string Email, string Password, string UserTypeIDs, string WarehouseIDs,
               string SiteCodes, string DepartmentIDs, string MachineIPAddress, int TenantID)
           {
           _UserID = UserID;
           _FirstName = FirstName;
           _LastName = LastName;
           _Email = Email;
           _Password = Password;
           _roles = UserTypeIDs;
           _WarehouseIDs = WarehouseIDs;
           _SiteCodes = SiteCodes;
           _DepartmentIDs = DepartmentIDs;
           _MachineIPAddress = MachineIPAddress;
           _TenantID = TenantID;
          
           }


           public Boolean  Login(String SSO_Apps_MST_User_ID, String Password, String LoginIPAddress)
           {

               LoginUserData resUserDetails = new LoginUserData();
               Boolean Result = false;
                String SqlStr = String.Empty;
                if (CommonLogic.Application("AppAuth") == "SSO")
                {
                    SqlStr = String.Format("dbo.[sp_GEN_CheckUserLogin] @SSO_Apps_MST_User_ID = " + SSO_Apps_MST_User_ID + ",@Password='"+Password+"'");
                }
                else
                {
                    SqlStr = String.Format("dbo.[sp_GEN_CheckUserLogin] @Email= '" + SSO_Apps_MST_User_ID + "',@Password='" + Password + "'");

                }

                  
               
                IDataReader rsUDetails = DB.GetRS(SqlStr);

               while (rsUDetails.Read())
               {

                   _UserID = DB.RSFieldInt(rsUDetails, "UserID");
                   _FirstName = DB.RSField(rsUDetails, "FirstName");
                   _LastName = DB.RSField(rsUDetails, "LastName");
                   _Email = Email;
                   _Password = Password;
                   _roles = DB.RSField(rsUDetails, "Roles");
                   _WarehouseIDs = DB.RSField(rsUDetails, "Warehouses");
                   _SiteCodes = DB.RSField(rsUDetails, "Zones");
                   _DepartmentIDs = DB.RSField(rsUDetails, "Departments");
                   _MachineIPAddress = LoginIPAddress;
                   _TenantID = DB.RSFieldInt(rsUDetails, "TenantID");
                   _SsoId = DB.RSFieldInt(rsUDetails, "SSOUserID");
                   _AccountId = DB.RSFieldInt(rsUDetails, "AccountID");
                    Result = true;
               }
               rsUDetails.Close();
               return Result;
           }

       }
    }
}
