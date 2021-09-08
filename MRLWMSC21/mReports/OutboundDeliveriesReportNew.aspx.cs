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
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services;

namespace MRLWMSC21.mReports
{
    public partial class OutboundDeliveriesReportNew : System.Web.UI.Page
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
                DesignLogic.SetInnerPageSubHeading(this.Page, "Outbound / Outbound Deliveries Report");
            }
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<OutboundDeliveriesReport> GetBillingReportList(string DocumenttypeID, string DeliverystatusID, string FromDate, string ToDate, int TenantId, int Warehouseid)
        {
            DocumenttypeID =DocumenttypeID== "" ? "0" : DocumenttypeID;
            DeliverystatusID =DeliverystatusID== "" ? "0" : DeliverystatusID;
            FromDate = FromDate == "" ? "null" : FromDate;
            ToDate = ToDate == "" ? "null" : ToDate;

            List<OutboundDeliveriesReport> GetBillingReport = new List<OutboundDeliveriesReport>();
            GetBillingReport = new OutboundDeliveriesReportNew().GetBillngRPT(DocumenttypeID,DeliverystatusID,FromDate,ToDate, TenantId,Warehouseid);
            return GetBillingReport;
        }

        private List<OutboundDeliveriesReport> GetBillngRPT(string DocumenttypeID, string DeliverystatusID, string FromDate, string ToDate, int TenantId, int Warehouseid)
        {
            if (FromDate != "" && FromDate!="null")
            {
                //FromDate = DateTime.ParseExact(FromDate.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                //Lines added by meena 
                DateTime str1 = Convert.ToDateTime(FromDate);
                FromDate = str1.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            if (ToDate != "" && ToDate !="null")
            {
                //ToDate = DateTime.ParseExact(ToDate.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                //Lines added by meena 
                DateTime str2 = Convert.ToDateTime(ToDate);
                ToDate = str2.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            }

            FromDate = FromDate == "null" ? FromDate : DB.SQuote(FromDate);
            ToDate = ToDate == "null" ? ToDate : DB.SQuote(ToDate);

            List<OutboundDeliveriesReport> lst = new List<OutboundDeliveriesReport>();
            string Query = "EXEC [sp_RPT_GetOutboundDeliveries] @DocumentTypeID = " + DocumenttypeID + ", @DeliveryStatusID="+DeliverystatusID + ", @FromDate = " + FromDate + ", @ToDate = " +ToDate + ",@AccountID_New="+ cp.AccountID + ",@TenantID_New=" + TenantId+ ", @WareHouseId = " + Warehouseid+"";
            //EXEC [sp_RPT_GetOutboundDeliveries] @DocumentTypeID=null, @DeliveryStatusID=5, @FromDate=null, @ToDate=null
            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    OutboundDeliveriesReport BR = new OutboundDeliveriesReport();
                    BR.DeliveryDocNo = row["OBDNumber"].ToString();
                    BR.DocumentType = row["DocumentType"].ToString();
                    BR.DeliveryDocDate = row["OBDDate"].ToString();
                    BR.Customer = row["CustomerName"].ToString();
                    BR.Stores = row["ReferedStores"].ToString();
                    BR.PGI = row["PGI"].ToString();
                    BR.DeliveryDate = row["DeliveryDate"].ToString();
                    BR.Status = row["DeliveryStatus"].ToString();
                    BR.LineItems = row["LineItems"].ToString();
                   
                    lst.Add(BR);
                }
            }
            return lst;
        }

        public class OutboundDeliveriesReport
        {
            public string DeliveryDocNo { get; set; }
            public string DocumentType { get; set; }
            public string DeliveryDocDate { get; set; }
            public string Customer { get; set; }
            public string Stores { get; set; }
            public string PGI { get; set; }
            public string DeliveryDate { get; set; }
            public string Status { get; set; }
            public string LineItems { get; set; }

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<MRLWMSC21.mReports.GetDataListModel.DocumentType> GetDocuments()
        {
            try
            {
                List<MRLWMSC21.mReports.GetDataListModel.DocumentType> ltsdocuments = new List<MRLWMSC21.mReports.GetDataListModel.DocumentType>();
                GetDataListBL data = new GetDataListBL();
                ltsdocuments = data.GetDocumentType();
                return ltsdocuments;
            }
            catch (Exception ex)
            {

                return null;
            }

        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<MRLWMSC21.mReports.GetDataListModel.DeliveryStatus> GetDelivery()
        {
            try
            {
                List<MRLWMSC21.mReports.GetDataListModel.DeliveryStatus> ltsdeliveries = new List<MRLWMSC21.mReports.GetDataListModel.DeliveryStatus>();
                GetDataListBL data = new GetDataListBL();
                ltsdeliveries = data.GetDeliverystatus();
                return ltsdeliveries;
            }
            catch (Exception ex)
            {

                return null;
            }

        }
    }
}