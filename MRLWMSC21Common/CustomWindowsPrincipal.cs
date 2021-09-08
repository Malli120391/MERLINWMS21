using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.Protocols;
using System.Configuration.Provider;
using System.Threading;
using System.Security.Principal;
using System.Diagnostics;
using System.Configuration;
using System.DirectoryServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;


namespace MRLWMSC21Common
{
    public sealed class CustomWindowsPrincipal:IPrincipal
    {

        // Private vars
        private WindowsIdentity identity;

        private int _tenantID;

        private Guid _userGUID;
        private int _UserID;

        private string _FirstName;
        private string _LastName;
        private string _eMail;

        private string[] _roles;
        private string[] _warehouses;
        private string[] _sitecodes;

      

        public CustomWindowsPrincipal(IIdentity identity, int TenantID, Guid UserGUID, int UserID, string FirstName, string LastName, string Email, string[] roles, string[] warehouses, string[] sitecodes)
        {
            _UserID = UserID;
           
            this.identity = (WindowsIdentity)identity;
            _tenantID = TenantID;
            _userGUID = UserGUID;
            _FirstName = FirstName;
            _LastName = LastName;
            _eMail = Email;
           
            _roles = new string[roles.Length];
            _warehouses = new string[warehouses.Length];
            _sitecodes = new string[sitecodes.Length];
            
            roles.CopyTo(_roles, 0);
            warehouses.CopyTo(_warehouses, 0);
            sitecodes.CopyTo(_sitecodes, 0);

            Array.Sort(_roles);
            Array.Sort(_warehouses);
            Array.Sort(_sitecodes);


        }




        //Authenticate user with specified name and password
        public static bool AuthenticateUser(String UserName,String Password){

            bool status = false;

            try
            {
                // get login user name
                String WinLoginUserName = WindowsPrincipal.Current.Identity.Name;

                // establish domain context
                PrincipalContext userDomain = new PrincipalContext(ContextType.Domain);

                // find your user
                UserPrincipal user = UserPrincipal.FindByIdentity(userDomain, WinLoginUserName);

                DirectoryEntry entry = new DirectoryEntry(GetCurrentDomainPath(), WinLoginUserName, Password);

                DirectorySearcher search = new DirectorySearcher(entry)
                {
                    Filter = "(SAMAccountName=" + user.SamAccountName + ")"
                };

                search.PropertiesToLoad.Add("cn");

                SearchResult result = search.FindOne();

                if (null != result)
                {

                    status= true;

                }


            }
            catch (Exception ex) {
                status = false;
            }

            return status;

        }

        //Get all users for specified group 
        public static List<String> GetGroupUsers(String GroupName)
        {
            List<String> listUsers = null;

            try
            {
                listUsers = new List<String>();

                PrincipalContext principalContext = new PrincipalContext(ContextType.Domain);

                GroupPrincipal group = GroupPrincipal.FindByIdentity(principalContext, GroupName);

                foreach (Principal principal in group.Members)
                {
                    listUsers.Add((String)principal.Name);
                }
            }
            catch (Exception ex) { 

            }
            return listUsers;
        }


        // Check User in specified group or not
        public static bool IsInGroup(String UserName, String GroupName) {

            bool status = false;

            List<String> GroupUsers = null;

            try
            {
                
                GroupUsers = GetGroupUsers(GroupName);

                if (GroupUsers.Contains(UserName)) {
                    status = true;
                }

            }
            catch (Exception ex) {

                status = false;

            }

            return status;
        }


        //Get Current windows UserName
        public static string GetCurrentWinUser()
        {

            String UserName = "";

            try
            {
                PrincipalContext userDomain = new PrincipalContext(ContextType.Domain);

                // get login user name
                String WinLoginUserName = WindowsPrincipal.Current.Identity.Name;

                // find your user
                UserPrincipal user = UserPrincipal.FindByIdentity(userDomain, WinLoginUserName);

                UserName = user.Name;
            }
            catch (Exception ex) { 

            }

            return UserName;
        }


        //Get  Current domain LDAP path
        public static string GetCurrentDomainPath()
        {
            DirectoryEntry de =
               new DirectoryEntry("LDAP://RootDSE");

            return "LDAP://" +
               de.Properties["defaultNamingContext"][0].
                   ToString(); 
        }


        public bool IsInRole(string Role)
        {
            bool status = false;

            List<String> GroupUsers = null;

            try
            {

                GroupUsers = GetGroupUsers(Role);

                if (GroupUsers.Contains(GetCurrentWinUser()))
                {
                    status = true;
                }

            }
            catch (Exception ex)
            {

                status = false;

            }

            return status;
        }


        public bool IsInWarehouse(string warehouse)
        {
            return Array.BinarySearch(_warehouses, warehouse) >= 0 ? true : false;
        }


        public bool IsInSiteCode(string sitecode)
        {
            return Array.BinarySearch(_sitecodes, sitecode) >= 0 ? true : false;
        }

        // Identity property
        public IIdentity Identity
        {
            get { return identity; }
        }

        //TenantID
        public int TenantID
        {
            get
            {
                return _tenantID;
            }
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
        public String FirstName
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
                return _warehouses;
            }
        }

        // Checks whether a principal is in all of the specified set of   roles
        public bool IsInAllRoles(params string[] roles)
        {
            foreach (string searchrole in roles)
            {
                if (Array.BinarySearch(_roles, searchrole) < 0)
                    return false;
            }
            return true;
        }


        // Checks whether a principal is in any of the specified set of roles
        public bool IsInAnyRoles(params string[] roles)
        {
            foreach (string searchrole in roles)
            {
                if (Array.BinarySearch(_roles, searchrole) >= 0)
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
                if (Array.BinarySearch(_sitecodes, searchSC) <= 0)
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
