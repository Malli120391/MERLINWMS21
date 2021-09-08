
using System;
using System.Collections;
using System.Data;
using System.Globalization;
using MRLWMSC21Common.Interfaces;
using System.Security;
namespace MRLWMSC21Common
{
    /// <summary>
    /// Summary description for CustomerSession.
    /// </summary>
    public sealed class CustomerSession : ICustomerSession
    {
        public int m_CustomerID;
        private Hashtable SessionParms;

        public CustomerSession(int CustomerID)
        {
            m_CustomerID = CustomerID;
            Age();
            LoadFromDB();
        }

        public void Age()
        {

            DB.ExecuteSQL("delete from CustomerSession where CustomerID=" + m_CustomerID.ToString() + " and ExpiresOn<=getdate()");
        }

        public static void StaticAge(int CustomerID)
        {

            DB.ExecuteSQL("delete from CustomerSession where CustomerID=" + CustomerID.ToString() + " and ExpiresOn<=getdate()");
        }

        public static void StaticAgeAllCustomers()
        {
          const string query = "delete from CustomerSession where(SessionValue IS NULL or cast(SessionValue as nvarchar(4000))= '') and ExpiresOn<= getdate()";
          DB.ExecuteSQL(query);
        }

        public void Clear()
        {
            DB.ExecuteSQL("delete from CustomerSession where CustomerID=" + m_CustomerID.ToString());
            LoadFromDB();
        }

        public void ClearVal(String SessionName)
        {
            DB.ExecuteSQL("delete from CustomerSession where SessionName=" + DB.SQuote(SessionName) + " and CustomerID=" + m_CustomerID.ToString());
            LoadFromDB();
        }

        public static void StaticClear(int CustomerID)
        {
            DB.ExecuteSQL("delete from CustomerSession where CustomerID=" + CustomerID.ToString());
        }

        public static void StaticClearAllCustomers()
        {
            const string query = "delete from CustomerSession";
            DB.ExecuteSQL(query);
        }

        public void ResyncToDB()
        {
            Age();
            LoadFromDB();
        }

        private void LoadFromDB()
        {

            IDataReader rs;
            using (rs = DB.GetRS("Select * from CustomerSession " + DB.GetNoLock() + " where CustomerID=" + m_CustomerID.ToString()))
            {
                SessionParms = new Hashtable();
                while (rs.Read())
                {
                    SessionParms.Add(DB.RSField(rs, "SessionName").ToLowerInvariant().Trim(), DB.RSField(rs, "SessionValue"));
                }
                rs.Close();
            }
        }

        public void SetVal(String SessionName, String SessionValue, System.DateTime ExpiresOn)
        {
            SessionName = SessionName.ToLowerInvariant().Trim();
            if (DB.GetSqlN("select count(SessionName) as N from CustomerSession " + DB.GetNoLock() + " where CustomerID=" + m_CustomerID.ToString() + " and SessionName=" + DB.SQuote(SessionName)) == 0)
            {
                // new parm, add it:
                DB.ExecuteSQL("insert into CustomerSession(CustomerID, SessionName,SessionValue,ExpiresOn) values(" + m_CustomerID.ToString() + "," + DB.SQuote(SessionName) + "," + DB.SQuote(SessionValue) + "," + DB.DateQuote(Localization.ToDBDateTimeString(ExpiresOn)) + ")");
                SessionParms.Add(SessionName, SessionValue);
            }
            else
            {
                DB.ExecuteSQL("update CustomerSession set SessionValue=" + DB.SQuote(SessionValue) + ", ExpiresOn=" + DB.DateQuote(Localization.ToDBDateTimeString(ExpiresOn)) + " where SessionName=" + DB.SQuote(SessionName) + " and CustomerID=" + m_CustomerID.ToString());
                SessionParms[SessionName] = SessionValue;
            }
        }

        public static void StaticSetVal(int CustomerID, String SessionName, String SessionValue, System.DateTime ExpiresOn)
        {
            SessionName = SessionName.ToLowerInvariant().Trim();
            if (DB.GetSqlN("select count(SessionName) as N from CustomerSession " + DB.GetNoLock() + " where CustomerID=" + CustomerID.ToString() + " and SessionName=" + DB.SQuote(SessionName)) == 0)
            {
                // new parm, add it:
                DB.ExecuteSQL("insert into CustomerSession(CustomerID, SessionName,SessionValue,ExpiresOn) values(" + CustomerID.ToString() + "," + DB.SQuote(SessionName) + "," + DB.SQuote(SessionValue) + "," + DB.DateQuote(Localization.ToDBDateTimeString(ExpiresOn)) + ")");
            }
            else
            {
                DB.ExecuteSQL("update CustomerSession set SessionValue=" + DB.SQuote(SessionValue) + ", ExpiresOn=" + DB.DateQuote(Localization.ToDBDateTimeString(ExpiresOn)) + " where SessionName=" + DB.SQuote(SessionName) + " and CustomerID=" + CustomerID.ToString());
            }
        }

        static public String StaticGetVal(String SessionName, int CustomerID)
        {
            StaticAge(CustomerID); // make sure we don't get expired crap back
            String tmpS = String.Empty;
            using (IDataReader rs = DB.GetRS("select SessionValue from CustomerSession " + DB.GetNoLock() + " where CustomerID=" + CustomerID.ToString() + " and SessionName=" + DB.SQuote(SessionName)))
            {
                if (rs.Read())
                {
                    tmpS = DB.RSField(rs, "SessionValue");
                }
                rs.Close();
            }
            return tmpS;
        }

        static public void StaticClearVal(String SessionName, int CustomerID)
        {
            DB.ExecuteSQL("delete from CustomerSession where CustomerID=" + CustomerID.ToString() + " and SessionName=" + DB.SQuote(SessionName));
        }

        public void SetVal(String SessionName, int NewValue, System.DateTime ExpiresOn)
        {
            SetVal(SessionName, NewValue.ToString(), ExpiresOn);
        }

        public void SetVal(String SessionName, Single NewValue, System.DateTime ExpiresOn)
        {
            SetVal(SessionName, NewValue.ToString(), ExpiresOn);
        }

        public void SetVal(String SessionName, decimal NewValue, System.DateTime ExpiresOn)
        {
            SetVal(SessionName, NewValue.ToString(), ExpiresOn);
        }

        public void SetVal(String SessionName, System.DateTime NewValue, System.DateTime ExpiresOn)
        {
            SetVal(SessionName, NewValue.ToString(), ExpiresOn);
        }

        public String Session(String SessionName)
        {
            String tmpS = String.Empty;
            try
            {
                tmpS = SessionParms[SessionName.ToLowerInvariant().Trim()].ToString();
            }
            catch
            {
                tmpS = String.Empty;
            }
            return tmpS;
        }

        public bool SessionBool(String paramName)
        {
            String tmpS = CommonLogic.Session(paramName).ToUpperInvariant();
            if (tmpS == "TRUE" || tmpS == "YES" || tmpS == "1")
            {
                return true;
            }
            return false;
        }

        public int SessionUSInt(String paramName)
        {
            String tmpS = Session(paramName);
            return Localization.ParseUSInt(tmpS);
        }

        public long SessionUSLong(String paramName)
        {
            String tmpS = Session(paramName);
            return Localization.ParseUSLong(tmpS);
        }

        public Single SessionUSSingle(String paramName)
        {
            String tmpS = Session(paramName);
            return Localization.ParseUSSingle(tmpS);
        }

        public Decimal SessionUSDecimal(String paramName)
        {
            String tmpS = Session(paramName);
            return Localization.ParseUSDecimal(tmpS);
        }

        public DateTime SessionUSDateTime(String paramName)
        {
            String tmpS = Session(paramName);
            return Localization.ParseUSDateTime(tmpS);
        }

        public int SessionNativeInt(String paramName)
        {
            String tmpS = Session(paramName);
            return Localization.ParseNativeInt(tmpS);
        }

        public long SessionNativeLong(String paramName)
        {
            String tmpS = Session(paramName);
            return Localization.ParseNativeLong(tmpS);
        }

        public Single SessionNativeSingle(String paramName)
        {
            String tmpS = Session(paramName);
            return Localization.ParseNativeSingle(tmpS);
        }

        public Decimal SessionNativeDecimal(String paramName)
        {
            String tmpS = Session(paramName);
            return Localization.ParseNativeDecimal(tmpS);
        }

        public DateTime SessionNativeDateTime(String paramName)
        {
            String tmpS = Session(paramName);
            return Localization.ParseNativeDateTime(tmpS);
        }

    }
}
