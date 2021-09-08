using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using MRLWMSC21Common;
using System.Data;
using System.Globalization;
using System.Web.Script.Services;
using System.Web.Services;
using Newtonsoft.Json;
using System.Text;

namespace MRLWMSC21.mReports
{
    public partial class CurrentStockReport : System.Web.UI.Page
    {
        public CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
        public static CustomPrincipal cp;
        public static string App_Path = "";

        protected void Page_PreInit(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            Page.Theme = "Outbound";
             // App_Path=System.Web.HttpContext.Current.Server.MapPath.ToString();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Current Stock");
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<MRLWMSC21.mReports.GetDataListModel.Printers> GetPrinters()
        {
            try
            {
                List<MRLWMSC21.mReports.GetDataListModel.Printers> lstprinter = new List<GetDataListModel.Printers>();
                GetDataListBL data = new GetDataListBL();
                lstprinter = data.GetPrinter();
                return lstprinter;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<MRLWMSC21.mReports.GetDataListModel.LabelSize> GetLabels()
        {
            try
            {
                List<MRLWMSC21.mReports.GetDataListModel.LabelSize> lstlabel = new List<GetDataListModel.LabelSize>();
                GetDataListBL data = new GetDataListBL();
                lstlabel = data.Getlabel();
                return lstlabel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public class Current_Stock_Report
        {
            public string Location { get; set; }
            public string CartonCode { get; set; }
            public string GoodsInQty { get; set; }
            public string GoodsOutQty { get; set; }
            public string AvailableQty { get; set; }
            public string GRNDate { get; set; }
            public string GRNNumber { get; set; }
            public string MfgDate { get; set; }
            public string ExpDate { get; set; }
            public string SerialNo { get; set; }
            public string BatchNo { get; set; }
            public string ProjectRefNo { get; set; }
            public string TenantName { get; set; }
            public string TenantCode { get; set; }
            public string MCode { get; set; }
            public string Code { get; set; }
            public string WHCode { get; set; }
            public string WHName { get; set; }
            
            public int MaterialMasterID { get; set; }
            public int TenantID { get; set; }
            public string KitCode { get; set; }
            public string KitQuantity { get; set; }
            public string NestedQty { get; set; }
            public string DeNestedQty { get; set; }
            public int HasDiscrepancy { get; set; }
            public string NoOfCopies { get; set; }

            public string MRP { get; set; }
            public string LineNo { get; set; }
            public string HUNo { get; set; }
            public string HUSize { get; set; }

        }

        //=================================== Added By M.D.Prasad =========================================//
        [WebMethod]
        public static string GetCurrentStockDynamicData(string typeid, string typetext, string Tenantid, string Mcode, string Mtypeid, string BatchNo, string Location, string kitId, string JSON, int IndustryID, int Warehouseid, string OEMNum , int PageId, int PageSize,string containerID)
        {
            try
            {
                CustomPrincipal cp3 = HttpContext.Current.User as CustomPrincipal;
                DataSet ds = null;
                Tenantid = Tenantid == "" ? "0" : Tenantid;
                Mcode = Mcode == "" ? "0" : Mcode;
                Mtypeid = Mtypeid == "" ? "0" : Mtypeid;
                BatchNo = BatchNo == "" ? "''" : DB.SQuote(BatchNo);
                Location = Location == "" ? "0" : Location;
                kitId = kitId == "" ? "0" : kitId;
                containerID = containerID == "" ? "0" : containerID;
                if (OEMNum == "") { OEMNum = ""; } else { OEMNum = OEMNum; }
                if (cp3 != null)
                {

                    //Page page = new Page();
                    //page.Response.Redirect("..Login.aspx?FP=1&SE=1");
                    ds = DB.GetDS("EXEC [sp_INV_GetCurrentStockReportDynamic] @SearchType=" + typeid + ",@searchText=" + DB.SQuote(typetext) + ",@TenantID=" + Tenantid + ",@MaterialMasterID=" + Mcode + ",@MTypeID=" + Mtypeid + ",@BatchNo=" + BatchNo + ",@LocationID=" + Location + ",@KitID=" + kitId + ",@AccountID_New=" + cp3.AccountID + ",@TenantID_New=" + Tenantid + ",@UserID_New=" + cp3.UserID + ",@UserTypeID_New=" + cp3.UserTypeID + ",@IndustryXML='" + JSON + "',@GEN_MST_Industry_ID=" + IndustryID + ",@WarehouseID=" + Warehouseid + ",@OEMPartNo='" + OEMNum + "', @MenuID=7245, @Rownumber=" + PageId + ", @NofRecordsPerPage=" + PageSize + ",@CartonID=" + containerID, false);
                }
                else
                {
                    return "0";
                }
                return JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                return "0";
            }
        }

        //=================================== Added By M.D.Prasad =========================================//

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetPrint(List<Current_Stock_Report> printobj, string Printerid, string LabelID)
        {
            string ZPL = "";
            List<Current_Stock_Report> lst = new List<Current_Stock_Report>();
            lst = printobj;
            TracklineMLabel Mlabel = new TracklineMLabel();         
                Mlabel.MCode = printobj[0].MCode;
                Mlabel.SerialNo = printobj[0].SerialNo;
                Mlabel.BatchNo = printobj[0].BatchNo;
                //Mlabel.MfgDate = printobj[0].MfgDate;
                //Mlabel.ExpDate = printobj[0].ExpDate;
                Mlabel.ProjectNo = printobj[0].ProjectRefNo;
            //Mlabel.Mrp = printobj[0].MRP;
            //Mlabel.MfgDate = Mlabel.MfgDate == null ? "" : printobj[0].MfgDate;
            //Mlabel.ExpDate = Mlabel.ExpDate == null ? "" : printobj[0].ExpDate;
            //Mlabel.Mrp = Mlabel.Mrp == null ? "" : printobj[0].MRP;
            //Mlabel.KitCode = Mlabel.KitCode == null ? "" : printobj[0].KitCode;
            //Mlabel.Lineno = Mlabel.Lineno == null ? "0" : printobj[0].LineNo;
            Mlabel.MfgDate = printobj[0].MfgDate == null || printobj[0].MfgDate == "" ? "" : printobj[0].MfgDate;
            Mlabel.ExpDate = printobj[0].ExpDate == null || printobj[0].ExpDate == "" ? "" : printobj[0].ExpDate;
            Mlabel.Mrp = printobj[0].MRP == null || printobj[0].MRP == "" ? "" : printobj[0].MRP;
            Mlabel.KitCode = printobj[0].KitCode == null || printobj[0].KitCode == "" ? "" : printobj[0].KitCode;
            Mlabel.Lineno = printobj[0].LineNo == null || printobj[0].LineNo == "" ? "0" : printobj[0].LineNo;
            Mlabel.HUNo= printobj[0].HUNo == null || printobj[0].HUNo == "" ? "1" : printobj[0].HUNo;
            Mlabel.HUSize = printobj[0].HUSize == null || printobj[0].HUSize == "" ? "1" : printobj[0].HUSize;


            string length = "";
                string width = "";
                string LabelType = "";
                string query = "select * from TPL_Tenant_BarcodeType where IsActive=1 and IsDeleted=0 and TenantBarcodeTypeID=" + LabelID;
                DataSet DS = DB.GetDS(query, false);
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    length = row["Length"].ToString();
                    width = row["Width"].ToString();
                    LabelType = row["LabelType"].ToString();
                }
                Mlabel.PrinterIP = Printerid;
                Mlabel.IsBoxLabelReq = false;
                Mlabel.Length = length;
                Mlabel.Width = width;
                int dpi = 0;
                dpi = CommonLogic.GETDPI(Mlabel.PrinterIP);
                Mlabel.Dpi = 203; //dpi;
                Mlabel.PrintQty = printobj[0].NoOfCopies;
                Mlabel.LabelType = LabelType;
                MRLWMSC21Common.PrintCommon.CommonPrint print = new MRLWMSC21Common.PrintCommon.CommonPrint();
            //Commented ON 01-JAN-2019 BY Prasad // print.PrintBarcodeLabel(Mlabel);
            ZPL = print.PrintBarcodeLabel(Mlabel);
            lst.Clear();
            return ZPL;// "Printed Successfully";
        }
        

        //=====================================  Added By M.D.Prasad ======================================//

        [WebMethod]
        public static string GetColumnsWithMenuID(int MenuID)
        {
            CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
            DataSet ds = DB.GetDS("EXEC [dbo].[USP_GetConfigurationColmuns] @AccountID=" + cp1.AccountID + ",@TenantID=" + cp1.TenantID + ",@UserID=" + cp1.UserID + ", @MenuID =" + MenuID, false);
            return JsonConvert.SerializeObject(ds);
        }

        [WebMethod]
        public static string MasterDetailsSet(string Sp_Set, string JSON)
        {
            Dictionary<dynamic, dynamic> values = JsonConvert.DeserializeObject<Dictionary<dynamic, dynamic>>(JsonConvert.DeserializeObject(JSON).ToString());

            StringBuilder sb = new StringBuilder();
            int count = 0;
            foreach (KeyValuePair<dynamic, dynamic> pair in values)
            {
                if (count != 0)
                    sb.Append(",");
                sb.Append(pair.Key + "=" + DB.SQuote(pair.Value));
                count++;
            }
            //sb.ToString();
            DB.ExecuteSQL("EXEC " + Sp_Set + " " + sb.ToString());

            return "";
        }


        [WebMethod]
        public static string SET_CurrentStockColumnOptionData(int MenuID, string DynamicXML)
        {
            CustomPrincipal cpl = HttpContext.Current.User as CustomPrincipal;
            string Status = "";
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" Exec [dbo].[Usp_UpsertGEN_CNF_DynamicColumns] ");
                sb.Append(" @DynamicXML='" + DynamicXML + "'");
                sb.Append(" ,@MenuID=" + MenuID);
                sb.Append(" ,@AccountID=" + cpl.AccountID);
                sb.Append(" ,@UserID=" + cpl.UserID);
                sb.Append(" ,@TenantID=" + cpl.TenantID);
                //sb.ToString();
                DB.ExecuteSQL(sb.ToString());
                Status = "success";
            }
            catch (Exception ex)
            {
                Status = "Failed";
            }
            return Status;
        }

        [WebMethod]
        public static string GetMspItems(int MaterialMasterID)
        {
            try
            {
                DataSet ds = DB.GetDS("EXEC [dbo].[USP_GET_CurrentStock_MSP_Items] @MaterialMasterID=" + MaterialMasterID, false);
                return JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                return "Error : " + ex;
            }
        }

        //=====================================  Added By M.D.Prasad ======================================//


        [WebMethod]
        public static string GetExcelData(string typeid, string typetext, string Tenantid, string Mcode, string Mtypeid, string BatchNo, string Location, string kitId, string JSON, int IndustryID, int Warehouseid, string OEMNum,string PageId,string PageSize, string containerID)
        {
            try
            {
                CustomPrincipal cp2 = HttpContext.Current.User as CustomPrincipal;
                string FileName= "CurrentStockReport" + DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second;
                Tenantid = Tenantid == "" ? "0" : Tenantid;
                Mcode = Mcode == "" ? "0" : Mcode;
                Mtypeid = Mtypeid == "" ? "0" : Mtypeid;
                BatchNo = BatchNo == "" ? "''" : DB.SQuote(BatchNo);
                Location = Location == "" ? "0" : Location;
                kitId = kitId == "" ? "0" : kitId;
                if (OEMNum == "") { OEMNum = ""; } else { OEMNum = OEMNum; }
                //DataSet ds = DB.GetDS("EXEC [sp_INV_GetCurrentStockReportDynamic] @SearchType=" + typeid + ",@searchText=" + DB.SQuote(typetext) + ",@TenantID=" + Tenantid + ",@MaterialMasterID=" + Mcode + ",@MTypeID=" + Mtypeid + ",@BatchNo=" + BatchNo + ",@LocationID=" + Location + ",@KitID=" + kitId + ",@AccountID_New=" + cp.AccountID + ",@TenantID_New=" + Tenantid + ",@UserID_New=" + cp.UserID + ",@UserTypeID_New=" + cp.UserTypeID + ",@IndustryXML='" + JSON + "',@GEN_MST_Industry_ID=" + IndustryID + ",@WarehouseID=" + Warehouseid + ",@OEMPartNo='" + OEMNum + "', @MenuID=7245, @IsExcel=1 ", false);
                DataSet ds = DB.GetDS("EXEC [sp_INV_GetCurrentStockReportDynamic] @SearchType=" + typeid + ",@searchText=" + DB.SQuote(typetext) + ",@TenantID=" + Tenantid + ",@MaterialMasterID=" + Mcode + ",@MTypeID=" + Mtypeid + ",@BatchNo=" + BatchNo + ",@LocationID=" + Location + ",@KitID=" + kitId + ",@AccountID_New=" + cp2.AccountID + ",@TenantID_New=" + Tenantid + ",@UserID_New=" + cp2.UserID + ",@UserTypeID_New=" + cp2.UserTypeID + ",@IndustryXML='" + JSON + "',@GEN_MST_Industry_ID=" + IndustryID + ",@WarehouseID=" + Warehouseid + ",@OEMPartNo='" + OEMNum + "', @MenuID=7245, @Rownumber=" + PageId + ", @NofRecordsPerPage=" + PageSize + ",@CartonID=" + containerID, false);

                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                dt.Columns.Remove("RID");
                dt.Columns.Remove("TenantID");
                dt.Columns.Remove("MaterialMasterID");

                CommonLogic.ExportExcelDataForReports(dt, FileName, new List<int>(),"Current Stock Report");
                return FileName;
            }
            catch (Exception ex)
            {
                return "Error : " + ex;
            }
        }
    }
}