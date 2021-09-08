using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Services;
using System.Web.Services;
using System.Reflection;
using MRLWMSC21Common;
using System.Data;
using System.Globalization;

namespace MRLWMSC21.mReports
{
    public partial class NonConfirmityReportNew : System.Web.UI.Page
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
                DesignLogic.SetInnerPageSubHeading(this.Page, "Inbound / Inward QC Report");
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<NonConfirmityReport> GetBillingReportList(string POHeaderID)
        {
            POHeaderID = POHeaderID == "" ? "null" : POHeaderID;

            List<NonConfirmityReport> GetBillingReport = new List<NonConfirmityReport>();
            GetBillingReport = new NonConfirmityReportNew().GetInwardNCReport(POHeaderID);
            return GetBillingReport;
        }

        private List<NonConfirmityReport> GetInwardNCReport(string POHeaderID)
        {
            
            List<NonConfirmityReport> list = new List<NonConfirmityReport>();
            string Query = "EXEC [sp_RPT_GetNCGoodsInReportForPOHeader] @POHeaderID= " + POHeaderID;
            DataSet ds = DB.GetDS(Query, false);
            if (ds.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    NonConfirmityReport IR = new NonConfirmityReport();
                    IR.LineNo = row["LineNumber"].ToString();
                    IR.PartNumber = row["MCode"].ToString();
                    IR.Quantity = row["Count"].ToString();
                    IR.Status = row["Status"].ToString();
                    IR.QCParameters = row["QCParameters"].ToString();
                    IR.ActualQCParameters = row["ActualQcParameters"].ToString();
                    list.Add(IR);

                }
            }

            
            return list;
            

        }
        public class NonConfirmityReport
        {
            public string LineNo { get; set; }
            public string PartNumber { get; set; }
            public string Quantity { get; set; }
            public string Status { get; set; }
            public string QCParameters { get; set; }
            public string ActualQCParameters { get; set; }



        }
        private void resetError(string error, bool isError)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);

        }
    }
}