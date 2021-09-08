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
using Newtonsoft.Json;

namespace MRLWMSC21.mReports
{
    public partial class MaterialAgeingReportNew : System.Web.UI.Page
    {
        protected CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        static CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Outbound";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Inventory / Material Ageing Report");
            }
        }

        [WebMethod]
        public static string getMaterialAgeingReport(string AgeinDays, string ZoneCode, string Mcode, string TenantID, string WarehouseID, string IsExport, string NoofRecords, string PageNo)
        {
            try
            {
                CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
                AgeinDays = AgeinDays == "" ? "0" : AgeinDays;
                ZoneCode = ZoneCode == "Select Zone" ? "''" : DB.SQuote(ZoneCode);
                Mcode = Mcode == "" ? "0" : Mcode;
                string Query = "EXEC [sp_RPT_GetMaterialAgeingReport]  @AgeIndays= " + AgeinDays + ",  @ZoneCode= " + ZoneCode + ",  @MaterialMasterID=" + Mcode + ",@AccountID_New=" + cp1.AccountID + ",@WarehouseID=" + WarehouseID + ",@TenantID_New=" + TenantID + ",@IsExport=" + 0 + ",@NofRecordsPerPage=" + NoofRecords + ",@Rownumber=" + PageNo;
                DataSet ds = DB.GetDS(Query, false);
                return JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                return "Error : " + ex;
            }
        }

        [WebMethod]
        public static string getMaterialAgeingReport_Export(string AgeinDays, string ZoneCode, string Mcode, string TenantID, string WarehouseID, string IsExport, string NoofRecords, string PageNo)
        {
            try
            {
                CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
                AgeinDays = AgeinDays == "" ? "0" : AgeinDays;
                ZoneCode = ZoneCode == "Select Zone" ? "''" : DB.SQuote(ZoneCode);
                Mcode = Mcode == "" ? "0" : Mcode;
                string fileName = "MaterialAgeingReport" + DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second;
                string Query = "EXEC [sp_RPT_GetMaterialAgeingReport]  @AgeIndays= " + AgeinDays + ",  @ZoneCode= " + ZoneCode + ",  @MaterialMasterID=" + Mcode + ",@AccountID_New=" + cp1.AccountID + ",@WarehouseID=" + WarehouseID + ",@TenantID_New=" + TenantID + ",@IsExport=" + 1 + ",@NofRecordsPerPage=" + NoofRecords + ",@Rownumber=" + PageNo;
                DataSet ds = DB.GetDS(Query, false);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    MRLWMSC21Common.CommonLogic.ExportExcelDataForReports(ds.Tables[0], fileName, new List<int>(), "Material Ageing Report");
                    return fileName;
                }
                else
                {
                    return "No Data Found";
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public static List<MaterialAgeingReport> GetBillingReportList(string AgeinDays, string ZoneCode,string Mcode, string TenantID, string WarehouseID)
        //{
        //    AgeinDays = AgeinDays == "" ? "0" : AgeinDays;
        //    ZoneCode = ZoneCode == "Select Zone" ? "''" : DB.SQuote(ZoneCode);
        //    Mcode = Mcode == "" ? "0" : Mcode;

        //    List<MaterialAgeingReport> GetBillingReport = new List<MaterialAgeingReport>();
        //    GetBillingReport = new MaterialAgeingReportNew().GetBillngRPT( AgeinDays, ZoneCode,Mcode, TenantID, WarehouseID);
        //    return GetBillingReport;
        //}

        //private List<MaterialAgeingReport> GetBillngRPT(string AgeinDays, string ZoneCode,string Mcode,string TenantID,string WarehouseID)
        //{

        //    List<MaterialAgeingReport> lst = new List<MaterialAgeingReport>();
        //    string Query = "  EXEC [sp_RPT_GetMaterialAgeingReport]  @AgeIndays= " +AgeinDays + ",  @ZoneCode= " + ZoneCode + ",  @MaterialMasterID=" +Mcode+ ",@AccountID_New=" + cp.AccountID + ",@WarehouseID="+ WarehouseID + ",@TenantID_New=" + TenantID;
        //    DataSet DS = DB.GetDS(Query, false);
        //    if (DS.Tables[0].Rows.Count != 0)
        //    {
        //        foreach (DataRow row in DS.Tables[0].Rows)
        //        {
        //            MaterialAgeingReport BR = new MaterialAgeingReport();
        //            BR.PartNumber = row["PartNo"].ToString();
        //            BR.PartDescription = row["MDescription"].ToString();
        //            BR.Tenant = row["TenantName"].ToString();
        //            BR.Location = row["Location"].ToString();
        //            BR.UnitPrice = row["UnitPrice"].ToString();
        //            BR.UoM = row["UoM/Qty"].ToString();
        //            BR.AvailableQty = row["AvailableQTY"].ToString();
        //            BR.ReceivedDate = row["ReceivedDate"].ToString();
        //            BR.AgeinDays = row["Age"].ToString();
        //            lst.Add(BR);
        //        }
        //    }
        //    return lst;
        //}


        public class MaterialAgeingReport
        {
            public string PartNumber { get; set; }
            public string PartDescription { get; set; }
            public string Tenant { get; set; }
            public string Location { get; set; }
            public string UnitPrice { get; set; }
            public string UoM { get; set; }
            public string AvailableQty { get; set; }
            public string ReceivedDate { get; set; }
            public string AgeinDays { get; set; }

        }


        //[WebMethod]
        //public static List<ListItem> GetZones()
        //{
        //    string query = "SELECT LocationZoneID,LocationZoneCode FROM INV_LocationZone where IsDeleted=0 and IsActive=1";
        //    string constr = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
        //    using (SqlConnection con = new SqlConnection(constr))
        //    {
        //        using (SqlCommand cmd = new SqlCommand(query))
        //        {
        //            List<ListItem> Zones = new List<ListItem>();
        //            cmd.CommandType = CommandType.Text;
        //            cmd.Connection = con;
        //            con.Open();
        //            using (SqlDataReader sdr = cmd.ExecuteReader())
        //            {
        //                while (sdr.Read())
        //                {
        //                    Zones.Add(new ListItem
        //                    {
        //                        Value = sdr["LocationZoneID"].ToString(),
        //                        Text = sdr["LocationZoneCode"].ToString()
        //                    });
        //                }
        //            }
        //            con.Close();
        //            return Zones;
        //        }
        //    }
        //}

        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public static List<MRLWMSC21.mReports.GetDataListModel.Zones> GetZones()
        //{
        //    try
        //    {
        //        List<MRLWMSC21.mReports.GetDataListModel.Zones> ltsZones = new List<MRLWMSC21.mReports.GetDataListModel.Zones>();
        //        GetDataListBL get = new GetDataListBL();
        //        ltsZones = get.GetZonesList();
        //        return ltsZones;
        //    }
        //    catch (Exception ex)
        //    {

        //        return null;
        //    }

        //}


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<MRLWMSC21.mReports.GetDataListModel.Zones> GetZones(int WareHouseId)
        {
            List<MRLWMSC21.mReports.GetDataListModel.Zones> lstZones = new List<MRLWMSC21.mReports.GetDataListModel.Zones>();
            //string Query = "SELECT LocationZoneID,LocationZoneCode FROM INV_LocationZone where IsDeleted=0 and IsActive=1";
            string Query = "SELECT DISTINCT LocationZoneID, LocationZoneCode FROM INV_LocationZone ILZ JOIN GEN_Warehouse GW ON GW.WarehouseID = ILZ.WarehouseID JOIN TPL_Tenant TNT ON TNT.AccountID = GW.AccountID where ILZ.IsDeleted = 0 and ILZ.IsActive = 1 AND  ILZ.WarehouseID="+ WareHouseId + " AND GW.AccountID = case when 0 = " + cp1.AccountID.ToString() + " then TNT.AccountID else " + cp1.AccountID.ToString() + " end";
            DataSet DS = DB.GetDS(Query, false);
            foreach (DataRow row in DS.Tables[0].Rows)
            {
                lstZones.Add(new MRLWMSC21.mReports.GetDataListModel.Zones()
                {
                    LocationZoneID = Convert.ToInt32(row["LocationZoneID"].ToString()),
                    LocationZoneCode = (row["LocationZoneCode"].ToString()),
                });
            }
            return lstZones;

        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<MRLWMSC21.mReports.GetDataListModel.Warehouse> GetWarehouse()
        {
            try
            {
                List<MRLWMSC21.mReports.GetDataListModel.Warehouse> ltsWareHouse = new List<MRLWMSC21.mReports.GetDataListModel.Warehouse>();
                GetDataListBL get = new GetDataListBL();
                ltsWareHouse = get.Getwarehouse();
                return ltsWareHouse;
            }
            catch (Exception ex)
            {

                return null;
            }

        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<MRLWMSC21.mReports.GetDataListModel.Warehouse> GetWarehouse1(string TenantID)
        {
            try
            {
                List<MRLWMSC21.mReports.GetDataListModel.Warehouse> ltsWareHouse = new List<MRLWMSC21.mReports.GetDataListModel.Warehouse>();
                GetDataListBL get = new GetDataListBL();
                ltsWareHouse = get.Getwarehouse1(TenantID);
                return ltsWareHouse;
            }
            catch (Exception ex)
            {

                return null;
            }

        }


    }
}