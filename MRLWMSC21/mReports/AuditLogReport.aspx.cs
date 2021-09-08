using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21Common;
using System.Data;
using System.Globalization;
using System.Web.Script.Services;
using System.Web.Services;

namespace MRLWMSC21.mReports
{
    public partial class AuditLogReport : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Outbound";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Audit Log Report");
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public static List<Current_Stock_Report> GetBillingReportList(string Tenantid, string Mcode, string Mtypeid, string BatchNo, string Location, string kitId)
        public static List<Audit_Log_report> GetBillingReportList(string ObjectID)
        {
            //Tenantid = Tenantid == "" ? "0" : Tenantid;
            //Mcode = Mcode == "" ? "0" : Mcode;
            //Mtypeid = Mtypeid == "" ? "0" : Mtypeid;
            //BatchNo = BatchNo == "" ? "''" : BatchNo;
            //Location = Location == "" ? "0" : Location;
            //kitId = kitId == "" ? "0" : kitId;

            List<Audit_Log_report> GetBillingReport = new List<Audit_Log_report>();
            //GetBillingReport = GetBillngRPT(Tenantid, Mcode, Mtypeid, BatchNo, Location, kitId);
            GetBillingReport = new AuditLogReport().GetBillngRPT(ObjectID);
            return GetBillingReport;
        }

        
       // public static List<Current_Stock_Report> GetBillngRPT(string Tenantid, string Mcode, string Mtypeid, string BatchNo, string Location, string kitId)
        private List<Audit_Log_report> GetBillngRPT(string ObjectID)
        {


            List<Audit_Log_report> lst = new List<Audit_Log_report>();
            //string Query = " EXEC [dbo].[sp_RPT_GetActiveStock_New] @MaterialMasterID=" + MCode + ", @LocationID = " + Location + ", @MTypeID = " + MaterialType + "";
           // string Query = "EXEC [sp_INV_GetCurrentStockReport] @TenantID=" + Tenantid + ",@MaterialMasterID=" + Mcode + ",@MTypeID=" + Mtypeid + ",@BatchNo=" + BatchNo + ",@LocationID=" + Location + ",@KitID=" + kitId + "";
            string Query = "select tab.name as Tablename, col.name as ColName,case when HH.IsDataInserted = 1 then 'Yes' else 'No' end as DataInserted,case when HH.IsDataUpdated = 1 then 'Yes' else 'No' end as DataUpdated,case when HH.IsDataDeleted = 1 then 'Yes' else 'No' end as DataDeleted , * from HIST_TRN_HistoryDetails AS HH INNER JOIN SYS.Objects AS Tab ON Tab.object_id = hh.Object_ID inner join sys.columns as col on col.column_id = hh.Column_ID and tab.object_id = col.object_id where hh.object_id= "+DB.SQuote(ObjectID);
            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    Audit_Log_report OR = new Audit_Log_report();
                    OR.TransactionId = row["HIST_TRN_HistoryHeader_ID"].ToString();
                    OR.EntityId = row["Object_ID"].ToString();
                    OR.TableName = row["Tablename"].ToString();
                    OR.ColumnName = row["ColName"].ToString();
                    OR.DataInserted = row["DataInserted"].ToString();
                    OR.DataUpdated = row["DataUpdated"].ToString();
                    OR.DataDeletd = row["DataDeleted"].ToString();
                    OR.OldValue = row["OldValue"].ToString();
                    OR.NewValue = row["NewValue"].ToString();
                    OR.ModifiedBy = row["ActivityBy"].ToString();
                    OR.ModifiedOn = row["modify_date"].ToString();
                    

                    lst.Add(OR);
                }
            }
            return lst;
        }

        public class Audit_Log_report
        {
            public string TransactionId { get; set; }
            public string EntityId { get; set; }
            public string TableName { get; set; }
            public string ColumnName { get; set; }
            public string DataInserted { get; set; }
            public string DataUpdated { get; set; }
            public string DataDeletd { get; set; }
            public string OldValue { get; set; }
            public string NewValue { get; set; }
            public string ModifiedBy { get; set; }
            public string ModifiedOn { get; set; }
            


        }
    }
}