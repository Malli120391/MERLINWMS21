using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21Common;
using Newtonsoft.Json;
using System.Web.Script.Services;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Globalization;

namespace MRLWMSC21.mReports
{
    public partial class OrderProcessreport : System.Web.UI.Page
    {

        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Outbound";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Performance / Order Process Report");
            }
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public static List<Current_Stock_Report> GetBillingReportList(string Tenantid, string Mcode, string Mtypeid, string BatchNo, string Location, string kitId)
        public static List<Order_Process> GetBillingReportList(string FromDate,string ToDate, int TenantId, int WareHouse)
        {
            FromDate = FromDate == "" ? "null" : FromDate;
            ToDate = ToDate == "" ? "null" : ToDate;

            List<Order_Process> GetBillingReport = new List<Order_Process>();
            GetBillingReport = new OrderProcessreport().GetBillngRPT(FromDate,ToDate, TenantId, WareHouse);
            return GetBillingReport;
        }


        // public static List<Current_Stock_Report> GetBillngRPT(string Tenantid, string Mcode, string Mtypeid, string BatchNo, string Location, string kitId)
        private List<Order_Process> GetBillngRPT(string FromDate, string ToDate, int TenantId, int WareHouse)
        {
            if (FromDate != "" && FromDate != "null")
            {
               // FromDate = DateTime.ParseExact(FromDate.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                FromDate = DateTime.ParseExact(FromDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            }
            if (ToDate != "" && ToDate != "null")
            {
               // ToDate = DateTime.ParseExact(ToDate.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                ToDate = DateTime.ParseExact(ToDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            }

            FromDate = FromDate == "null" ? FromDate : DB.SQuote(FromDate);
            ToDate = ToDate == "null" ? ToDate : DB.SQuote(ToDate);           
            List<Order_Process> lst = new List<Order_Process>();
            string Query = "EXEC [dbo].[USP_orderProcessReport] @WindowStart="+FromDate+ ",@WindowEnd="+ToDate+ ",@TenantId = "+TenantId+ ", @WareHouseId="+WareHouse+"";
            DataSet DS = DB.GetDS(Query, false);
            //if (DS.Tables[0].Rows.Count == 1)
            //{
            //    foreach (DataRow row in DS.Tables[0].Rows)
            //    {
            //        Order_Process OR = new Order_Process();
            //        OR.TotalInward = row["TotalInward"].ToString();
            //        if(OR.TotalInward=="0")
            //        s++;
            //        OR.WorkItemsCompleted = row["WorkItemsCompleted"].ToString();
            //        if (OR.WorkItemsCompleted == "0")
            //            s++;
            //        OR.CompletedPercent = row["CompletedPercent"].ToString();
            //        if (OR.CompletedPercent == "0.00")
            //            s++;
            //        OR.ReceiptsToDo = row["ReceiptsToDo"].ToString();
            //        if (OR.ReceiptsToDo == "0")
            //            s++;
            //        OR.ToDoPercent = row["ToDoPercent"].ToString();
            //        if (OR.ToDoPercent == "0.00")
            //            s++;
            //         lst.Add(OR);
            //        if (s == 5)
            //        {
            //            return null;
            //        }
            //        else
            //        {
            //            return lst;
            //        }
            //    }
            //}
                if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    var s = 0;
                    Order_Process OR = new Order_Process();
                    OR.TotalInward = row["TotalInward"].ToString();
                    if (OR.TotalInward == "0")
                        s++;
                    OR.WorkItemsCompleted = row["WorkItemsCompleted"].ToString();
                    if (OR.WorkItemsCompleted == "0")
                        s++;
                    OR.CompletedPercent = row["CompletedPercent"].ToString();
                    if (OR.CompletedPercent == "0.00")
                        s++;
                    OR.ReceiptsToDo = row["ReceiptsToDo"].ToString();
                    if (OR.ReceiptsToDo == "0")
                        s++;
                    OR.ToDoPercent = row["ToDoPercent"].ToString();
                    if (OR.ToDoPercent == "0.00")
                        s++;
                    lst.Add(OR);
                    if (s == 5)
                    {
                        return null;
                    }
                    else
                    {
                        return lst;
                    }

                }
                
                
               
            }
            
            return lst;
        }

        public class Order_Process
        {
            public string TotalInward { get; set; }
            public string WorkItemsCompleted { get; set; }
            public string CompletedPercent { get; set; }
            public string ReceiptsToDo { get; set; }
            public string ToDoPercent { get; set; }
           

        }
    }
}