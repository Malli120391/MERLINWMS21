using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;


namespace MRLWMSC21Common
{
    public class CustomPrincipal : IPrincipal
    {

        private IIdentity _identity;

        private int _tenantID;
        
        private Guid _userGUID;
        private int _UserID;

        private string _FirstName;
        private string _LastName;
        private string _eMail;
        private string _TenantName;

        

        private string[] _roles;
        private string[] _warehouses;
        private string[] _sitecodes;

        private int _AccountID;
        private int _UserTypeID;

        private string _Account;


        public CustomPrincipal(IIdentity identity, int TenantID, string TenantName, Guid UserGUID, int UserID, string FirstName, string LastName, string Email, string[] roles, string[] warehouses, string[] sitecodes,int AccountID = 0, int UserTypeID = 0,string account="")
        {
            _UserID = UserID;
            _identity = identity;
            _tenantID = TenantID;
            _TenantName = TenantName;
            _userGUID = UserGUID;
            _FirstName = FirstName;
            _LastName = LastName;
            _eMail= Email;
            _roles = new string[roles.Length];
            _warehouses = new string[warehouses.Length];
            _sitecodes = new string[sitecodes.Length];
            roles.CopyTo(_roles, 0);
            warehouses.CopyTo(_warehouses, 0);
            sitecodes.CopyTo(_sitecodes, 0);
            _AccountID = AccountID;
            _UserTypeID = UserTypeID;
            _Account = account;
            //_Account = acc;


            Array.Sort(_roles);
            Array.Sort(_warehouses);
            Array.Sort(_sitecodes);
        }

        // IPrincipal Implementation
        public bool IsInRole(string role)
        {
            return Array.BinarySearch(_roles, role) >= 0 ? true : false;
        }

        public bool IsInWarehouse(string warehouse)
        {
            return Array.BinarySearch(_warehouses, warehouse) >= 0 ? true : false;
        }

        public bool IsInSiteCode(string sitecode)
        {
            return Array.BinarySearch(_sitecodes, sitecode) >= 0 ? true : false;
        }

        /*
        public bool IsInWarehouse(string[] AllowedWarehouses)
        {

            bool isInWH = false;
            foreach (string userWH in AllowedWarehouses)
            {
                if (_warehouses.Contains(userWH))
                {
                    isInWH = true;
                    break;
                }
            }
            return isInWH;

        }

        public bool IsInSiteCode(string[] AllowedSiteCodes)
        {

            bool isInSC = false;
            foreach (string userSC in AllowedSiteCodes)
            {
                if (_sitecodes.Contains(userSC))
                {
                    isInSC = true;
                    break;
                }
            }
            return isInSC;

        }

       */ 

        public IIdentity Identity
        {
            get
            {
                return _identity;
            }
        }

        //TenantID
        public int TenantID
        {
            get
            {
                return _tenantID ;
            }
        }

        public int AccountID
        {
            get { return _AccountID; }
            
        }

        

        public int UserTypeID
        {
            get { return _UserTypeID; }
           
        }

        //Tenant Name
        public string TenantName
        {
            get { return _TenantName; }
            set { _TenantName = value; }
        }

        //Account
        public string Account
        {
            get { return _Account; }
            set { _Account = value; }

        }

        //UserGUID
        public Guid UserGUID
        {
            get
            {
                return _userGUID;
            }
        }

        public int UserID
        {
            get
            {
                return _UserID;
            }
        }

        //FirstName
        public String  FirstName
        {
            get
            {
                return _FirstName; 
            }
        }

        //LastName
        public String LastName
        {
            get
            {
                return _LastName;
            }
        }

        //Email
        public String Email
        {
            get
            {
                return _eMail;
            }
        }
        
        //Roles
        public String[] Roles
        {
            get
            {
                return _roles;
            }
        }

        //Warehosue
        public String[] Warehouses
        {
            get
            {
                return _warehouses ;
            }
        }

         // Checks whether a principal is in all of the specified set of   roles
        public bool IsInAllRoles( params string [] roles )
        {
          foreach (string searchrole in roles )
          {
            if (Array.BinarySearch(_roles, searchrole) < 0 )
              return false;
          }
          return true;
        }


        // Checks whether a principal is in any of the specified set of roles
        public bool IsInAnyRoles( params string [] roles )
        {
          foreach (string searchrole in roles )
          {
            if (Array.BinarySearch(_roles, searchrole ) >= 0 )
              return true;
          }
          return false;
        }


        // Checks whether a principal is in all of the specified set of warehouses
        public bool IsInAllWarehouses(params string[] warehouses)
        {
            foreach (string searchWH in warehouses)
            {
                if (Array.BinarySearch(_warehouses, searchWH) <= 0)
                    return false;
            }
            return true;
        }


        // Checks whether a principal is in any of the specified set of   warehouses
        public bool IsInAnyWarehouses(params string[] warehouses)
        {
            foreach (string searchWH in warehouses)
            {
                if (Array.BinarySearch(_warehouses, searchWH) >= 0)
                    return true;
            }
            return false;
        }

        // Checks whether a principal is in all of the specified set of   sitecodes
        public bool IsInAllSitecodes(params string[] sitecodes)
        {
            foreach (string searchSC in sitecodes)
            {
                if (Array.BinarySearch(_sitecodes , searchSC) <= 0)
                    return false;
            }
            return true;
        }


        // Checks whether a principal is in any of the specified set of   sitecodes
        public bool IsInAnySitecodes(params string[] sitecodes)
        {
            foreach (string searchSC in sitecodes)
            {
                if (Array.BinarySearch(_sitecodes, searchSC) >= 0)
                    return true;
            }
            return false;
        }


    }
}
