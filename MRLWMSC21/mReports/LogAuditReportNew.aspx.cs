using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using MRLWMSC21Common;
using System.Data;
using System.Globalization;

namespace MRLWMSC21.mReports
{
    public partial class LogAuditReportNew : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
            protected void Page_PreInit(object sender, EventArgs e)
            {
                Page.Theme = "Outbound";
            }
            protected void Page_Load(object sender, EventArgs e)
            {
                if (!IsPostBack)
                {
                    DesignLogic.SetInnerPageSubHeading(this.Page, "Audit / User Audit Report");
                }
            }
            [WebMethod]
            [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
            public static List<LogAuditReport> GetBillingReportList(int UserID, string FromDate, string ToDate)
            {
                List<LogAuditReport> GetBillingReport = new List<LogAuditReport>();
                GetBillingReport = new LogAuditReportNew().GetBillngRPT(UserID, FromDate, ToDate);
                return GetBillingReport;
            }

            private List<LogAuditReport> GetBillngRPT(int UserID, string FromDate, string ToDate)
            {
            cp = HttpContext.Current.User as CustomPrincipal;
            if (FromDate != "")
                {
                    FromDate = DateTime.ParseExact(FromDate.Replace("-", "/"), "dd/MMM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }
                if (ToDate != "")
                {
                    ToDate = DateTime.ParseExact(ToDate.Replace("-", "/"), "dd/MMM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }


                List<LogAuditReport> lst = new List<LogAuditReport>();
                string Query = " EXEC [sp_RPT_GetLogAuditData] @UserID= " + UserID + ", @FromDate = " + DB.SQuote(FromDate) + ", @ToDate = " + DB.SQuote(ToDate) + ",@AccountID_New=" + cp.AccountID + ",@TenantID_New=" + cp.TenantID;
            
                DataSet DS = DB.GetDS(Query, false);
                if (DS.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        LogAuditReport BR = new LogAuditReport();
                        BR.EmployeeCode = row["EmployeeCode"].ToString();
                        BR.AccountName = row["Account"].ToString();
                        BR.EmployeeName = row["UserName"].ToString();
                        BR.IPAddress = row["IPAddress"].ToString();
                        BR.LoginTime = row["LogInTime"].ToString();
                        BR.LogoutTime = row["LogOutTime"].ToString();
                        BR.SessionTime = row["SessionTime"].ToString();
                    
                        lst.Add(BR);
                    }
                }
                return lst;
            }


            public class LogAuditReport
            {
                public string EmployeeCode { get; set; }
                public string EmployeeName { get; set; }
                public string IPAddress { get; set; }
                public string LoginTime { get; set; }
                public string LogoutTime { get; set; }
                public string SessionTime { get; set; }
                public string AccountName { get; set; }


            }
    }
}