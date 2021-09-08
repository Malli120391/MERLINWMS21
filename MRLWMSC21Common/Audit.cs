using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRLWMSC21Common
{
    public sealed class Audit
    {

        // Records the User Login Details
        public static bool LogInAudit(String UserID,String LoginIP,int TenantID)
        {
            try
            {
                //INOUT Value = 1 for Logout process
                //DB.ExecuteSQL("INSERT into LogAudit(UserID, INOUT, IPAddress) Values(" + UserID + ",1," + DB.SQuote(LoginIP) + ")");
                DB.ExecuteSQL("exec sp_SEC_InsertLogAudit @UserID=" + UserID + ",@Inout=1,@LogTime='"+DateTime.Now+"',@IPAddress =" + DB.SQuote(LoginIP)+",@TenantID="+TenantID+",@CreatedBy="+UserID);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Records the User Logout Details
        public static bool LogOutAudit(String UserID, String LoginIP, int TenantID)
        {
            try
            {
                //INOUT Value = 0 for Logout process
               // DB.ExecuteSQL("INSERT into LogAudit(UserID, IPAddress) Values(" + UserID + ",0," + DB.SQuote(LoginIP) + ")");
                DB.ExecuteSQL("exec sp_SEC_InsertLogAudit @UserID=" + UserID + ",@Inout=2,@LogTime='" + DateTime.Now + "',@IPAddress =" + DB.SQuote(LoginIP) + ",@TenantID=" + TenantID + ",@CreatedBy=" + UserID);
                return true;
            }
            catch
            {
                return false;
            }
        }


        // Records the User Transaction Details
        public static void LogTranscationAudit(String UserID, int InOutID,int AuditActivityID,int RecordRefID,int DeliveryStatusID)
        {
            try
            {
                //InOutID = 1 for Inbound and InOutID = 2 for Outbound
                DB.ExecuteSQL("sp_InsertTransactionLog @UserID=" + UserID + ",@InOutID=" + InOutID.ToString() + ",@AuditActivityID=" + AuditActivityID.ToString() + ",@RecordRefID=" + RecordRefID.ToString() + ",@DeliveryStatusID=" + DeliveryStatusID);
               
            }
            catch(Exception ex)
            {
               
            }
        }

    }
}
