using System;
using System.Web;
using System.Web.Security;
using System.Data;
using System.Text;
using System.Collections;
using System.Globalization;

namespace MRLWMSC21Common
{
    /// <summary>
    /// User object info is normally rebuilt off of session["UserID"]!!!! This is created in AppLogic.SessionStart or other places
    /// </summary>
    [Serializable]
    public class User
    {


        #region Properties
        
              

        private int m_UserID;
        private Guid m_UserGUID;
        private bool m_HasUserRecord;
        
        private String m_UserLevelName;
        private String m_LocaleSetting;
        
        private String m_EMail;
        
        private String m_WarehouseIDs;
        private String m_UserTypeIDs;
        private String m_SiteCodes;

        private int m_DepartmentID;

        private bool m_OKToEMail;
        
        private String m_Password;
        private bool m_IsAnon;
        
        private String m_FirstName;
        private String m_LastName;
        
        private String m_LastIPAddress;
        private bool m_SuppressCookies;
        
        private String m_Roles;
        private int m_LocationID;
        private String m_MobilePhone;

      

        public int UserID
        {
            get
            {
                return m_UserID;
            }
        }
       
        

        public Guid UserGUID
        {
            get{ return m_UserGUID;}
        }

        

        public String UserLevelName
        {
            get
            {
                return m_UserLevelName;
            }
        }

        public String EMail
        {
            get
            {
                return m_EMail;
            }
            set
            {
                m_EMail = value;
            }
        }

        public String FirstName
        {
            get{ return m_FirstName;}
            set{ m_FirstName = value.Trim();}
        }

        public String LastName
        {
            get{ return m_LastName;}
            set{ m_LastName = value;}
        }

        public String FullName()
        {
            return (m_FirstName + " " + m_LastName).Trim();
        }

        public int DepartmentID
        {
            get { return m_DepartmentID; }
            set { m_DepartmentID = value; }
        }

        public String WarehosueIDs
        {
            get { return m_WarehouseIDs ; }
            set { m_WarehouseIDs = value.Trim(); }
        }

        public String UserTypeIDs
        {
            get { return m_UserTypeIDs; }
            set { m_UserTypeIDs = value.Trim(); }
        }

        public String SiteCodes
        {
            get { return m_SiteCodes ; }
            set { m_SiteCodes = value.Trim(); }
        }
       
        public String LastIPAddress
        {
            get
            {
                return m_LastIPAddress;
            }
        }

        public bool OKToEMail
        {
            get
            {
                return m_OKToEMail;
            }
            set
            {
                m_OKToEMail = value;
            }
        }

        public String Password
        {
            get
            {
                return m_Password;
            }
            set
            {
                m_Password = value;
            }
        }

       
       

        public String LocaleSetting
        {
            get
            {
                return m_LocaleSetting;
            }
        }


      

        public String MobilePhone
        {
            get { return m_MobilePhone; }
            set { m_MobilePhone = value.Trim(); }
        }

        

            

        public bool IsAnon
        {
            get
            {
                return m_IsAnon;
            }
        }

        public bool HasUserRecord
        {
            get
            {
                return m_HasUserRecord;
            }
        }

       


       

      
      
       

        //private String m_PaymentMethod; // last payment method chosen by User, not guaranteed to be accurate or current!, but should be in a linear shopping experience during checkout!
     
        #endregion


        #region Constructors

        public User()
            : this(CommonLogic.SessionUSInt("UserID"), false)
        { }

        public User(bool SuppressCookies)
            : this(CommonLogic.SessionUSInt("UserID"), SuppressCookies)
        { }

        public User(int UserID)
            : this(UserID, false)
        { }

        public User(int UserID, bool SuppressCookies)
        {
            m_SuppressCookies = SuppressCookies;
            Init(UserID);
        }

       

        private void Init(int UserID)
        {


            m_UserID = UserID;
            m_LocaleSetting = String.Empty;
            

          
            if (m_LocaleSetting.Length == 0 || AppLogic.IsAdminSite)
            {
                m_LocaleSetting = Localization.GetWebConfigLocale();
            }
            

           
            

            m_LastIPAddress = CommonLogic.ServerVariables("REMOTE_ADDR");
            m_DepartmentID  = 0;
            m_UserLevelName = String.Empty;
            m_HasUserRecord = false;
            m_MobilePhone = String.Empty;
            m_EMail = String.Empty;
            m_Password = String.Empty;
            m_IsAnon = true;
          
            m_Roles = String.Empty;
            m_FirstName = String.Empty;
            m_LastName = String.Empty;
            m_WarehouseIDs  = String.Empty;
            m_UserTypeIDs  = String.Empty;
            
            m_LocationID = 0;
           
            if (m_UserID != 0)
            {
                // we have a session, so rebuild: User info from it:
               // String sql = "select c.*,Address.LandPhone, Address.MobilePhone,Address.CountryID, Address.LocationID,Address.Suite,Address.State,Address.Address1,Address.Zip, isnull(cl.LevelDiscountPercent, 0) LevelDiscountPercent, isnull(cl.LevelDiscountsApplyToExtendedPrices, 0) LevelDiscountsApplyToExtendedPrices from User c " + DB.GetNoLock() + " JOIN Address on c.UserID = Address.UserID left join UserLevel cl " + DB.GetNoLock() + " on c.UserLevelID = cl.UserLevelID where  Address.AddressID = c.BillingAddressID AND c.Deleted=0 and c.UserID=" + m_UserID.ToString();
                String sql = "select * from User Where UserID=" + m_UserID.ToString();
                
                IDataReader rs = DB.GetRS(sql);
                if (rs.Read())
                {
                    m_HasUserRecord = true;
                    m_MobilePhone = DB.RSField(rs, "Mobile");
                    m_EMail = DB.RSField(rs, "EMail");
                    m_Password = DB.RSField(rs, "Password");
                    if (AppLogic.AppConfigBool("EncryptPassword"))
                    {
                        m_Password = AppLogic.UnmungeString(m_Password);
                        if (m_Password.StartsWith("Error")) //bad decryption may be in clear
                        {
                            m_Password = DB.RSField(rs, "Password");
                        }
                    }
                    m_IsAnon = (m_EMail.Trim().Length == 0 || m_EMail.StartsWith("Anon_"));
                    m_FirstName = DB.RSField(rs, "FirstName");
                    m_LastName = DB.RSField(rs, "LastName");
                    m_LocationID = DB.RSFieldInt(rs, "LocationID");
                    m_DepartmentID = DB.RSFieldInt(rs, "DepartmentID");
                    m_WarehouseIDs  = DB.RSField(rs, "WarehosueID");
                    m_UserTypeIDs = DB.RSField(rs, "UserTypeID");
                    m_SiteCodes  = DB.RSField(rs, "UserTypeID");

                    
                 
                    
                    m_UserGUID = DB.RSFieldGUID(rs, "UserGUID");
                    if (CommonLogic.ServerVariables("QUERY_STRING").ToUpperInvariant().IndexOf("LOCALESETTING=") == -1 && DB.RSField(rs, "LocaleSetting").Length != 0)
                    {
                        m_LocaleSetting = Localization.CheckLocaleSettingForProperCase(DB.RSField(rs, "LocaleSetting"));
                    }
                    
                   // m_UserLevelName = GetUserLevelName(m_UserLevelID, m_LocaleSetting);
                    if (!m_IsAnon)
                    {
                        // Check that the password is encrypted
                        string PWD = AppLogic.UnmungeString(DB.RSField(rs, "Password"));
                        bool isClear = (PWD.StartsWith("Error."));
                        if (AppLogic.AppConfigBool("EncryptPassword"))
                        {
                            if (isClear)
                            {
                                PWD = AppLogic.MungeString(DB.RSField(rs, "Password"));
                                DB.ExecuteSQL(String.Format("update User set [Password]={0} where UserID={1}", DB.SQuote(PWD), m_UserID));
                            }
                        }
                        else
                        {
                            if (!isClear)
                            {
                                DB.ExecuteSQL(String.Format("update User set [Password]={0} where UserID={1}", DB.SQuote(DB.RSField(rs, "Password")), m_UserID));
                            }
                        }
                    }
                }
                if (!m_SuppressCookies)
                {
                   
                   
                        DB.ExecuteSQL("update User set LastIPAddress=" + DB.SQuote(m_LastIPAddress) + " where UserID=" + m_UserID.ToString());
                   
                }
                rs.Close();

               
            }
            if (m_LocaleSetting.Length == 0 || AppLogic.IsAdminSite)
            {
                m_LocaleSetting = Localization.GetWebConfigLocale();
            }
           
            m_LocaleSetting = Localization.CheckLocaleSettingForProperCase(m_LocaleSetting);
           

        }

        #endregion


        #region Other Functions

       
       
        public void SetLocale(String LocaleSetting)
        {
            LocaleSetting = Localization.CheckLocaleSettingForProperCase(LocaleSetting);
            if (LocaleSetting.Length == 0)
            {
                LocaleSetting = Localization.GetWebConfigLocale();
            }
            m_LocaleSetting = LocaleSetting;
        }

    
        static public String GetName(int UserID)
        {
            String tmpS = String.Empty;
            IDataReader rs = DB.GetRS("Select firstname,lastname from User  " + DB.GetNoLock() + " where UserID=" + UserID.ToString());
            if (rs.Read())
            {
                tmpS = (DB.RSField(rs, "FirstName") + " " + DB.RSField(rs, "LastName")).Trim();
            }
            rs.Close();
            return tmpS;
        }

       
      

       
       

        

        static public int GetIDFromEMail(String EMail)
        {
            int tmpS = 0;
            IDataReader rs = DB.GetRS("Select * from User  " + DB.GetNoLock() + " where deleted=0 and EMail=" + DB.SQuote(EMail.ToLower()));
            if (rs.Read())
            {
                tmpS = DB.RSFieldInt(rs, "UserID");
            }
            rs.Close();
            return tmpS;
        }

        static public Guid GetGUID(int UserID)
        {
            Guid tmpS = Guid.Empty;
            IDataReader rs = DB.GetRS("Select UserGUID from User  " + DB.GetNoLock() + " where Userid=" + UserID.ToString());
            if (rs.Read())
            {
                tmpS = DB.RSFieldGUID(rs, "UserGUID");
            }
            rs.Close();
            return tmpS;
        }

      

      

       

        public static String GetUsername(int UserID)
        {
            IDataReader rs = DB.GetRS("Select * from User where Userid=" + UserID.ToString());
            String uname = String.Empty;
            if (rs.Read())
            {
                uname = (DB.RSField(rs, "FirstName") + "" + DB.RSField(rs, "LastName")).Trim();
            }
            rs.Close();
            return uname;
        }

     
        public void RequireUserRecord()
        {
            if (!m_HasUserRecord)
            {
               
                HttpContext.Current.Session["UserID"] = m_UserID.ToString();
                //HttpContext.Current.Session["UserLevelID"] = m_UserLevelID.ToString();
                HttpContext.Current.Session["UserGUID"] = m_UserGUID;

                // try to write them a new Cookie (may fail, but it's OK), so when they visit next time, we will remember who they are:
                FormsAuthentication.SetAuthCookie(m_UserGUID.ToString(), true);
                HttpCookie Cookie = HttpContext.Current.Response.Cookies[FormsAuthentication.FormsCookieName];
                Cookie.Expires = DateTime.Now.Add(new TimeSpan(1000, 0, 0, 0));

                Init(m_UserID); // must reload User data now that we have a record for it
            }
        }

      
        public bool IsLicensedUser()
        {
            if (IsAnon)
            {
                return false;
            }
            int N = DB.GetSqlN("select count(*) as N from Orders " + DB.GetNoLock() + " where TransactionState=" + DB.SQuote(AppLogic.ro_TXStateCaptured) + " and UserID=" + m_UserID.ToString());
            return (N != 0);
        }

        static public String GetUserLevelName(int UserLevelID, String LocaleSetting)
        {
            String tmpS = String.Empty;
            if (UserLevelID != 0)
            {
                IDataReader rs = DB.GetRS("Select Name from UserLevel " + DB.GetNoLock() + " where UserLevelID=" + UserLevelID.ToString());
                if (rs.Read())
                {
                    tmpS = DB.RSFieldByLocale(rs, "Name", LocaleSetting);
                }
                rs.Close();
            }
            return tmpS;
        }

    
    

       
       

#endregion

        #region Update - Delete Methods

        /// <summary>
        /// Creates an array of User sql parameters that can be used by String.Format to build SQL statements.
        /// </summary>
        /// <returns>object[]</returns>
        private object[] UserValues
        {
            get
            {
          
                object[] values = new object[] 
		          {
			          m_UserID,                      //{0}
			          DB.SQuote(m_EMail),                 //{2}
			          DB.SQuote(m_FirstName),            //{3}
			          DB.SQuote(m_LastName),             //{4}
			          DB.SQuote(m_LastIPAddress),         //{6}
                      DB.SQuote(m_WarehouseIDs),    //{9}
                      DB.SQuote(m_UserTypeIDs),    //{9}
                      DB.SQuote(m_SiteCodes),    //{9}
                      m_DepartmentID, 
                      DB.SQuote(m_Password),                //{12}

                      
                    
		          };
                return values;
            }
        }


       

        #endregion
    }






}
