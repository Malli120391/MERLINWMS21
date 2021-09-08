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
    public partial class InwardInspectonReportNew : System.Web.UI.Page
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
                DesignLogic.SetInnerPageSubHeading(this.Page, "INWARD INSPECTION REPORT");
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<InwardInspectonReport> GetBillingReportList(string storerefno)
        {
            storerefno = storerefno == "" ? "null" : storerefno;
            //MaterialType = MaterialType == "" ? "null" : MaterialType;
            //Location = Location == "" ? "null" : Location;

            List<InwardInspectonReport> GetBillingReport = new List<InwardInspectonReport>();
            GetBillingReport = new InwardInspectonReportNew().GetBillngRPT(storerefno);
            return GetBillingReport;
        }

        private List<InwardInspectonReport> GetBillngRPT(string storerefno)
        {



            List<InwardInspectonReport> lst = new List<InwardInspectonReport>();
            //string Query = " EXEC [sp_RPT_GetLogAuditData] @UserID= " + PartNO + ", @FromDate = " + DB.SQuote(MaterialType) + ", @ToDate = " + DB.SQuote(ToDate) + "";
            string Query = " Exec [sp_RPT_GetInwardInspectionReport] @InboundID=" + storerefno + ",@AccountID_New=" + cp.AccountID + ",@TenantID_New=" + cp.TenantID;

            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    InwardInspectonReport BR = new InwardInspectonReport();

                    BR.PartNo = row["MCode"].ToString();
                    BR.Description = row["MDescription"].ToString();
                    BR.ReceivedQty = row["ReceivedQty"].ToString();
                    BR.AcceptedQty = row["AcceptedQty"].ToString();
                    BR.RejectedQty = row["RejectedQty"].ToString();
                    //BR.SampleSize = row[""].ToString();
                    //BR.Parameter = row[""].ToString();
                    //BR.Test = row[""].ToString();
                    BR.UoM = row["UoM"].ToString();
                    //BR.Specification = row[""].ToString();
                    //BR.Observation = row[""].ToString();
                    BR.Remarks = row["Status"].ToString();
                    //BR.SerialNO = row["SerialNo"].ToString();
                    //BR.StockType = row["StockType"].ToString();

                    lst.Add(BR);
                }
            }
            
            return lst;
        }


        public class InwardInspectonReport
        {
            public string PartNo { get; set; }
            public string Description { get; set; }
            public string ReceivedQty { get; set; }
            public string AcceptedQty { get; set; }
            public string RejectedQty { get; set; }
            //public string SampleSize { get; set; }
            //public string Parameter { get; set; }
            //public string Test { get; set; }
            public string UoM { get; set; }
            //public string Specification { get; set; }
            //public string Observation { get; set; }
            public string Remarks { get; set; }
            //public string SerialNO { get; set; }
            //public string StockType { get; set; }

        }
        private void resetError(string error, bool isError)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);

        }
    }
}