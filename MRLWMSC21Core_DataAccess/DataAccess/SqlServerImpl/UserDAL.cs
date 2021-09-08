using System;
using System.Collections.Generic;
using System.Text;
using MRLWMSC21Core.Library;
using MRLWMSC21Core.Entities;
using System.Data.SqlClient;
using System.Data;

namespace MRLWMSC21Core.DataAccess
{
    public class UserDAL : BaseDAL, Interfaces.IUserDAL
    {
        private string _ClassCode = "WMSCore_DA_002_";

        public UserDAL(int LoginUser, string ConnectionString) : base(LoginUser, ConnectionString)
        {

            _ClassCode = ExceptionHandling.GetClassExceptionCode(ExceptionHandling.ExcpConstants_API_DataLayer.UserDAL);
        }

        public bool SaveUser(User oUser)
        {
            try
            {

                // Method block to Save Entity Data to Database.
                StringBuilder _sbCreateUser = new StringBuilder();
                
                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandling.LogException(ex, _ClassCode + "001");

                return false;
            }
        }


        public List<User> GetAllUsersUnderAccount(int iAccountID)
        {
            try
            {

                // Method block to Fetch the Entity List from Database.
                List<User> _lUsers = new List<User>();

                return _lUsers;
            }
            catch (Exception ex)
            {
                ExceptionHandling.LogException(ex, "002");

                return null;
            }
        }


        public List<User> GetAllUsersUnderAccount(Account oAccount)
        {
            try
            {

                // Method block to Fetch the Entity List from Database.
                List<User> _lUsers = new List<User>();

                return _lUsers;
            }
            catch (Exception ex)
            {
                ExceptionHandling.LogException(ex, "003");

                return null;
            }
        }

        public List<User> GetOnlyAccountUsers(Account oAccount)
        {

            try
            {

                // Method block to Fetch the Entity List from Database.
                List<User> _lUsers = new List<User>();

                return _lUsers;
            }
            catch (Exception ex)
            {
                ExceptionHandling.LogException(ex, "004");

                return null;
            }
        }

        public List<User> GetOnlyAccountUsers(int iAccountID)
        {

            try
            {

                // Method block to Fetch the Entity List from Database.
                List<User> _lUsers = new List<User>();
                
                return _lUsers;
            }
            catch (Exception ex)
            {
                ExceptionHandling.LogException(ex, "005");

                return null;
            }
        }

        public List<User> GetAllUsersUnderTenant(Tenant oTenant)
        {

            try
            {

                // Method block to Fetch the Entity List from Database.
                List<User> _lUsers = new List<User>();

                return _lUsers;
            }
            catch (Exception ex)
            {
                ExceptionHandling.LogException(ex, "006");

                return null;
            }
        }

        public List<User> GetAllUsersUnderTenant(int iTenant)
        {

            try
            {

                // Method block to Fetch the Entity List from Database.
                List<User> _lUsers = new List<User>();

                return _lUsers;
            }
            catch (Exception ex)
            {
                ExceptionHandling.LogException(ex, "007");

                return null;
            }
        }

        public User GetUserByID(int UserID)
        {

            try
            {

                // Method block to Fetch the Entity List from Database.
                User _oUsers = new User();

                return _oUsers;
            }
            catch (Exception ex)
            {
                ExceptionHandling.LogException(ex, _ClassCode + "008");

                return null;
            }
        }



        public List<User> GetUsersByID(string sUserIDs, string Delimiter)
        {
            try
            {

                // Method block to Fetch the Entity List from Database.
                List<User> _lUsers = new List<User>();

                return _lUsers;
            }
            catch (Exception ex)
            {
                ExceptionHandling.LogException(ex, "007");

                return null;
            }
        }


        public UserProfile LoginUser(UserProfile oUserProfile)
        {
            try
            {
               
                // Method block to Fetch the Entity List from Database.

                string _sSQL = "EXEC dbo.[sp_GEN_CheckUserLogin] @Email = '" + oUserProfile.EMailID+"'"+ ",@Password= '"+oUserProfile.Password+"'";

                DataSet _dsResults = base.FillDataSet(_sSQL);
                UserProfile _oUserProfile = null;

                if (_dsResults != null)
                    if (_dsResults.Tables.Count > 0)
                        if (_dsResults.Tables[0].Rows.Count > 0)
                        {
                            _oUserProfile = new UserProfile();

                            _oUserProfile.SSOUserID = ConversionUtility.ConvertToInt(_dsResults.Tables[0].Rows[0]["SSOUserID"].ToString());
                            _oUserProfile.UserID = ConversionUtility.ConvertToInt(_dsResults.Tables[0].Rows[0]["UserID"].ToString());
                            _oUserProfile.TenantID = ConversionUtility.ConvertToInt(_dsResults.Tables[0].Rows[0]["TenantID"].ToString());
                            _oUserProfile.AccountID = ConversionUtility.ConvertToInt(_dsResults.Tables[0].Rows[0]["AccountID"].ToString());
                            _oUserProfile.UserTypeID = ConversionUtility.ConvertToInt(_dsResults.Tables[0].Rows[0]["UserTypeID"].ToString());

                             
                            _oUserProfile.UserGUID = _dsResults.Tables[0].Rows[0]["UserGUID"].ToString();
                            _oUserProfile.ClientMAC = oUserProfile.ClientMAC;
                            _oUserProfile.ClientIP = oUserProfile.ClientIP;
                            _oUserProfile.CookieIdentifier = oUserProfile.CookieIdentifier;
                            _oUserProfile.SessionIdentifier = oUserProfile.SessionIdentifier;
                            _oUserProfile.FirstName = _dsResults.Tables[0].Rows[0]["FirstName"].ToString();
                            _oUserProfile.LastName = _dsResults.Tables[0].Rows[0]["LastName"].ToString();
                            _oUserProfile.EMailID = _dsResults.Tables[0].Rows[0]["Email"].ToString();
                            _oUserProfile.AccountName = _dsResults.Tables[0].Rows[0]["Account"].ToString();
                            _oUserProfile.TenantName = _dsResults.Tables[0].Rows[0]["TenantName"].ToString();

                            _oUserProfile.LoginTimeStamp = DateTime.UtcNow.ToString();
                            _oUserProfile.LastRequestTimestamp = DateTime.UtcNow.ToString();
                          
                            _oUserProfile.IsLoggedIn = true;

                            _oUserProfile.UserRoleID = _dsResults.Tables[0].Rows[0]["Roles"].ToString();


                          }
                      else
                        {
                            throw new WMSExceptionMessage() { WMSExceptionCode = "EMC_User_DAL_0001", WMSMessage = ErrorMessages.EMC_User_DAL_0001, ShowAsError = true };
                        }

                return _oUserProfile;

            }
            catch (WMSExceptionMessage excp)
            {
                throw excp;
            }
            catch (Exception ex)
            {
                ExceptionHandling.LogException(ex, "008");

                return null;
            }
            
        }
        
    }
}
