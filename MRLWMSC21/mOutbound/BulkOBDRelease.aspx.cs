using MRLWMSC21Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRLWMSC21.mOutbound
{
    public partial class BulkOBDRelease : System.Web.UI.Page
    {

        public static CustomPrincipal cp;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            Page.Theme = "Outbound";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Bulk OBD Release");
            }
        }
        [WebMethod]
        public static string GETOBData(string FromDate, string ToDate, string TenantID, string WHID, string OBDID, string priority, string DueDate, string SOHID, string CustomerID, string PaginationId, string PageSize,string StatusID)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                string query = "EXEC [dbo].[USP_OBD_GETOBDDataForBulkRelease] @FromDate=" + DB.SQuote(FromDate) + ",@ToDate=" + DB.SQuote(ToDate) + ",@TenantID=" + TenantID + ",@WHID=" + WHID + ",@OBDID=" + OBDID + ",@Priority=" + DB.SQuote(priority) + ",@DueDate=" + DB.SQuote(DueDate) + ",@SOHID=" + SOHID + ",@CustomerID=" + CustomerID + ",@RowNumber=" + PaginationId + ",@NofRecordsPerPage=" + PageSize + ",@StatusID=" + StatusID + ",@AccountID=" + cp.AccountID;
                DataSet ds = DB.GetDS(query, false);
                return JsonConvert.SerializeObject(ds);



            }
            catch (Exception)
            {

                throw;
            }
        }

        [WebMethod]
        public static string BulkRelease(List<OBDData> oBDDatas,string DockId)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            
            try
            {
                string fileName = "";

                fileName = "OBDRelease" + DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second;
                DataSet ResultDs = new DataSet();

                DataTable dtSuccess = new DataTable();
                DataColumn SucessStatus = new DataColumn("Status", typeof(string));
                DataColumn SuccessOBDNumber = new DataColumn("OBDNumber", typeof(string));
                DataColumn SuccessSONumber = new DataColumn("SONumber", typeof(string));

                dtSuccess.TableName = "Success";
                dtSuccess.Columns.Add(SucessStatus);
                dtSuccess.Columns.Add(SuccessOBDNumber);
                dtSuccess.Columns.Add(SuccessSONumber);

                DataTable dtfailure = new DataTable();
                dtfailure.TableName = "Failure";



                DataColumn FailureStatus = new DataColumn("Status", typeof(string));
                DataColumn FailureOBDNumber = new DataColumn("OBDNumber", typeof(string));
                DataColumn SONumber = new DataColumn("SONumber", typeof(string));
                DataColumn MCode = new DataColumn("MCode", typeof(string));
                DataColumn MDescription = new DataColumn("MDescription", typeof(string));
                DataColumn PickQuantity = new DataColumn("PickQuantity", typeof(string));
                DataColumn BatchNumber = new DataColumn("BatchNumber", typeof(string));
                DataColumn SerialNumber = new DataColumn("SerialNumber", typeof(string));
                DataColumn ExpDate = new DataColumn("ExpDate", typeof(string));
                DataColumn MfgDate = new DataColumn("MfgDate", typeof(string));


                
                dtfailure.Columns.Add(FailureStatus);
                dtfailure.Columns.Add(FailureOBDNumber);
                dtfailure.Columns.Add(SONumber);
                dtfailure.Columns.Add(MCode);
                dtfailure.Columns.Add(MDescription);
                dtfailure.Columns.Add(PickQuantity);
                dtfailure.Columns.Add(BatchNumber);
                dtfailure.Columns.Add(SerialNumber);
                dtfailure.Columns.Add(ExpDate);
                dtfailure.Columns.Add(MfgDate);


                for (int i = 0; i < oBDDatas.Count; i++)
                {

                    string query = "EXEC [dbo].[Sp_OBD_ReserveStockForPicking] @OutboundID=" + oBDDatas[i].OutboundID + ",@UserID=" + cp.UserID + ",@DockID=" + DockId;
                    DataSet ds = DB.GetDS(query, false);

                    if (ds.Tables.Count == 1)
                    {
                        string Query_HU = "[dbo].[SP_HU_Upsert_Stock_Reservation] @OutBoundId = " + oBDDatas[i].OutboundID + ", @UserId = " + cp.UserID + "";
                        DB.ExecuteSQL(Query_HU);

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DataRow drSucess = dtSuccess.NewRow();
                            drSucess["Status"] = "Success";
                            drSucess["OBDNumber"] = oBDDatas[i].OBDNumber;
                            drSucess["SONumber"] = oBDDatas[i].SONumber;
                            dtSuccess.Rows.Add(drSucess);
                        }
                       
                        
                    }
                    else
                    {
                        
                       
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            DataRow drFailure = dtfailure.NewRow();
                            drFailure["Status"] = "Failure";
                            drFailure["OBDNumber"] = oBDDatas[i].OBDNumber;
                            drFailure["SONumber"] = dr["SONumber"].ToString();
                            drFailure["MCode"] = dr["MCode"].ToString();
                            drFailure["MDescription"] = dr["MDescription"].ToString();
                            drFailure["PickQuantity"] = dr["PickQuantity"].ToString();
                            drFailure["BatchNumber"] = dr["BatchNumber"].ToString();
                            drFailure["SerialNumber"] = dr["SerialNumber"].ToString();
                            drFailure["ExpDate"] = dr["ExpDate"].ToString();
                            drFailure["MfgDate"] = dr["MfgDate"].ToString();
                            dtfailure.Rows.Add(drFailure);
                        }
                        

                    }

                    
                }

                ResultDs.Tables.Add(dtSuccess);
                ResultDs.Tables.Add(dtfailure);

                MRLWMSC21Common.CommonLogic.ExportLoadSheetInfo(ResultDs, fileName, new List<int>());
                
                return fileName;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public class OBDData
        {
            public string OBDNumber { get; set; }
            public int OutboundID { get; set; }
            public string SONumber { get; set; }
        }


    }
}