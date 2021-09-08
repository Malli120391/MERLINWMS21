using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRLWMSC21_Endpoint.DTO
{

    public class ProfileDTO
    {
        private string _UserName;

        private string _UserID;
        private int _SSOUserID;
        private int _UserTypeID;
        private string _UserType;

        private string _SessionIdentifier;
        private string _CookieIdentifier;
        private string _ClientIP;
        private string _ClientMAC;
        private string _EMail;

        private string _LoginTimeStamp;
        private string _LastRequestTimestamp;

        private int _UserRoleID;
        private string _UserRole;

        private int _WarehouseID;

        private bool _IsLoggedIn;

        private string _FirstName;
        private string _LastName;
        private string _Password;
        private string _SiteCodes;
        private string _DepartmentIDs;
        private string _MachineIPAddress;
        private int _TenantID;
        private int _SsoId;
        private string _AccountId;
        public string UserName { get => _UserName; set => _UserName = value; }
        public string UserID { get => _UserID; set => _UserID = value; }
        public int UserTypeID { get => _UserTypeID; set => _UserTypeID = value; }
        public string UserType { get => _UserType; set => _UserType = value; }
        public string SessionIdentifier { get => _SessionIdentifier; set => _SessionIdentifier = value; }
        public string CookieIdentifier { get => _CookieIdentifier; set => _CookieIdentifier = value; }
        public string ClientIP { get => _ClientIP; set => _ClientIP = value; }
        public string ClientMAC { get => _ClientMAC; set => _ClientMAC = value; }
        public string LoginTimeStamp { get => _LoginTimeStamp; set => _LoginTimeStamp = value; }
        public string LastRequestTimestamp { get => _LastRequestTimestamp; set => _LastRequestTimestamp = value; }
        public int UserRoleID { get => _UserRoleID; set => _UserRoleID = value; }
        public string UserRole { get => _UserRole; set => _UserRole = value; }
        public int WarehouseID { get => _WarehouseID; set => _WarehouseID = value; }
        public bool IsLoggedIn { get => _IsLoggedIn; set => _IsLoggedIn = value; }
        public string EMail { get => _EMail; set => _EMail = value; }
        public int SSOUserID { get => _SSOUserID; set => _SSOUserID = value; }
        public string FirstName { get => _FirstName; set => _FirstName = value; }
        public string LastName { get => _LastName; set => _LastName = value; }
        public string Password { get => _Password; set => _Password = value; }
        public string SiteCodes { get => _SiteCodes; set => _SiteCodes = value; }
        public string DepartmentIDs { get => _DepartmentIDs; set => _DepartmentIDs = value; }
        public string MachineIPAddress { get => _MachineIPAddress; set => _MachineIPAddress = value; }
        public int TenantID { get => _TenantID; set => _TenantID = value; }
        public int SsoId { get => _SsoId; set => _SsoId = value; }
        public string AccountId { get => _AccountId; set => _AccountId = value; }
    }

}