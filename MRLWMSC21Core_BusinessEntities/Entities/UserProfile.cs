using System;
using System.Collections.Generic;
using System.Text;

namespace MRLWMSC21Core.Entities
{
    public class UserProfile
    {
        private string _FirstName;
        private string _MiddleName;
        private string _LastName;

        private string _eMailID;
        private string _Password;

        private string _UserGUID;
        private int _UserID;

        private string _AccountName;
        private int _AccountID;

        private string _TenantName;
        private int _TenantID;

        private int _UserTypeID;
        private string _UserType;

        private string _SessionIdentifier;
        private string _CookieIdentifier;
        private string _ClientIP;
        private string _ClientMAC;

        private string _LoginTimeStamp;
        private string _LastRequestTimestamp;

        private List<UserRoles> _UserRoles;
        private List<Warehouse> _Warehouses;

        private string _UserRoleID;

        private int _SSOUserID;

        private bool _IsLoggedIn;

        public string FirstName { get => _FirstName; set => _FirstName = value; }
        public string MiddleName { get => _MiddleName; set => _MiddleName = value; }
        public string LastName { get => _LastName; set => _LastName = value; }
        public string EMailID { get => _eMailID; set => _eMailID = value; }
        public string Password { get => _Password; set => _Password = value; }
        public string UserGUID { get => _UserGUID; set => _UserGUID = value; }
        public int UserID { get => _UserID; set => _UserID = value; }
        public string AccountName { get => _AccountName; set => _AccountName = value; }
        public int AccountID { get => _AccountID; set => _AccountID = value; }
        public string TenantName { get => _TenantName; set => _TenantName = value; }
        public int TenantID { get => _TenantID; set => _TenantID = value; }
        public int UserTypeID { get => _UserTypeID; set => _UserTypeID = value; }
        public string UserType { get => _UserType; set => _UserType = value; }
        public string SessionIdentifier { get => _SessionIdentifier; set => _SessionIdentifier = value; }
        public string CookieIdentifier { get => _CookieIdentifier; set => _CookieIdentifier = value; }
        public string ClientIP { get => _ClientIP; set => _ClientIP = value; }
        public List<UserRoles> UsersRoles { get => _UserRoles; set => _UserRoles = value; }
        public List<Warehouse> Warehouses { get => _Warehouses; set => _Warehouses = value; }
        public int SSOUserID { get => _SSOUserID; set => _SSOUserID = value; }
        public string LoginTimeStamp { get => _LoginTimeStamp; set => _LoginTimeStamp = value; }
        public string LastRequestTimestamp { get => _LastRequestTimestamp; set => _LastRequestTimestamp = value; }
        public string ClientMAC { get => _ClientMAC; set => _ClientMAC = value; }
        public bool IsLoggedIn { get => _IsLoggedIn; set => _IsLoggedIn = value; }
        public string UserRoleID { get => _UserRoleID; set => _UserRoleID = value; }
    }
}
