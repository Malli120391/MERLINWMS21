using MRLWMSC21Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Management;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRLWMSC21.mMaterialManagement
{
    public partial class LocationManager : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Inbound";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            // LoadPrinters();
            DesignLogic.SetInnerPageSubHeading(this.Page, "Location Manager");
            if (!IsPostBack)
            {
                CommonLogic.LoadStores(ddlWarehouse, "", 0, false);
                CommonLogic.LoadPrinters(ddlPrinter);
                LoadLocationTypes();
            }

            ViewState["TenantID"] = cp.TenantID;
            hdnTenantID.Value = cp.TenantID.ToString();

            if (cp.TenantID != 0)
            {
                tbltenant.Visible = false;
                selSearchCategory.SelectedIndex = 2;
                selSearchCategory.Enabled = false;
            }
            hdnRoles.Value = string.Join(",", cp.Roles);

            if (cp.IsInRole("3") || cp.IsInRole("5") || cp.IsInRole("2") || cp.IsInRole("42") || cp.IsInRole("45"))
            {
                hdnRoles.Value = "1";
            }
            else {
                hdnRoles.Value = "0";
            }
        }

        private void LoadLocationTypes()
        {
            DataSet ds = DB.GetDS("select ID, LocationType FROM Inv_LocationType WHERE IsActive=1 AND IsDeleted=0 AND ID IN(12,11)  ORDER BY ID DESC", false);
            ddlLocType.DataSource = ds.Tables[0];
            ddlLocType.DataTextField = "LocationType";
            ddlLocType.DataValueField = "ID";
            ddlLocType.DataBind();

            ddlLocTypeBulk.DataSource = ds.Tables[0];
            ddlLocTypeBulk.DataTextField = "LocationType";
            ddlLocTypeBulk.DataValueField = "ID";
            ddlLocTypeBulk.DataBind();

            ddlLocTypeBulkCreate.DataSource = ds.Tables[0];
            ddlLocTypeBulkCreate.DataTextField = "LocationType";
            ddlLocTypeBulkCreate.DataValueField = "ID";
            ddlLocTypeBulkCreate.DataBind();

            ddlLocTypeAdd.DataSource = ds.Tables[0];
            ddlLocTypeAdd.DataTextField = "LocationType";
            ddlLocTypeAdd.DataValueField = "ID";
            ddlLocTypeAdd.DataBind();

        }

        //public void LoadPrinters() {
        //    var searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Printer");
        //    var results = searcher.Get();

        //    IList<ManagementBaseObject> printers = new List<ManagementBaseObject>();

        //    foreach (var printer in results)
        //    {
        //        if ((bool)printer["Network"])
        //        {
        //            printers.Add(printer);
        //        }
        //    }
        //}


       
        protected void btnLoadMap_Click(object sender, EventArgs e)
        {
            //$('#divMap').empty();
            // selectedBins = [];

            int ZoneID = Convert.ToInt32(hdnSelectedZone.Value);
            List<Rack> oRackList = GetLocations(ZoneID.ToString());


            if (oRackList.Count <= 0)
            {
                resetError("Please select Zone", true);
                // showStickyToast(false, "No Location Found", false);
                // UnBlockDialog();
                return;
            }
            StringBuilder rack = new StringBuilder();
            var bindataClass = "";
            int divMapWidth = 1000, rackHeight = 100, rackWidth = 25, columnWidth = 100, levelHeight = 20;
            for (var i = 0; i < oRackList.Count; i++)
            {
                rackHeight = GetRackHeight_Div(oRackList[i]);
                rackWidth = GetRackWidth_Div(oRackList[i]);
                divMapWidth = divMapWidth < rackWidth ? rackWidth : divMapWidth;               
                divMap1.Style.Add("width", divMapWidth + "px");
                divMap1.Style.Add("overflow-x","auto");
                rack.Append("<div class='divRackContainer' style='width:" + rackWidth + "px;'>");
                rack.Append("<div class='divRackName' style='height:" + rackHeight + "px;line-height:" + rackHeight + "px;'>" + oRackList[i].RackName + "</div><div class='divColumnContainer'>");

                for (var j = 0; j < oRackList[i].ColumnList.Count; j++)
                {
                    rack.Append("<div class='divColumn' >");

                    for (var k = 0; k < oRackList[i].ColumnList[j].LevelList.Count; k++)
                    {
                        rack.Append("<div class='divLevelBox'");
                        rack.Append("<div> ");
                        for (var l = 0; l < oRackList[i].ColumnList[j].LevelList[k].binList.Count; l++)
                        {
                            var bindata = oRackList[i].ColumnList[j].LevelList[k].binList[l].bindata;
                            if (bindata != "")
                                bindataClass = "bindataClass";
                            else
                                bindataClass = "";

                            rack.Append("<span class='spanBin " + bindataClass + "' data-fulllocation='" + oRackList[i].ColumnList[j].LevelList[k].binList[l].FullLocation + "' data-Tenant='" + oRackList[i].ColumnList[j].LevelList[k].binList[l].Tenant + "' data-TenantID='" + oRackList[i].ColumnList[j].LevelList[k].binList[l].TenantID + "'   data-bindata='" + bindata + "' data-binrepdata='" + oRackList[i].ColumnList[j].LevelList[k].binList[l].binRepdata + "' data-suppliers='" + oRackList[i].ColumnList[j].LevelList[k].binList[l].Suppliers + "'   data-isQ='" + oRackList[i].ColumnList[j].LevelList[k].binList[l].IsQuarantine + "' data-isF='" + oRackList[i].ColumnList[j].LevelList[k].binList[l].IsFastMoving + "' data-isM='" + oRackList[i].ColumnList[j].LevelList[k].binList[l].IsMixedMaterialOK + "'>" + oRackList[i].ColumnList[j].LevelList[k].binList[l].BinName + "</span>");
                        }
                        rack.Append("</div> ");
                        rack.Append("<div class='divLevel'  style='height:" + levelHeight + "px;'>" + oRackList[i].ColumnList[j].ColumnName + "-" + oRackList[i].ColumnList[j].LevelList[k].LevelName + "</div>");
                    }
                    rack.Append(" </div>");
                }
                rack.Append("</div>");

                rack.Append("</div>");
               
                //$('#divMap').append(rack);
            }
            divMap1.InnerHtml = rack.ToString();
            LoadHandlers();

            ddlLocationCode.SelectedValue = ZoneID.ToString();

        }

    private int GetRackHeight_Div(Rack oRack)
    {
        var rackHeight = 0;
        for (var i = 0; i < oRack.ColumnList.Count; i++)
        {
            if (oRack.ColumnList[i].LevelList.Count > rackHeight)
                rackHeight = oRack.ColumnList[i].LevelList.Count;
        }
        return rackHeight * 50;
    }

    private int GetRackWidth_Div(Rack oRack)
    {
        return oRack.ColumnList.Count * 150;
    }

        protected void resetError(string error, bool isError)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);
        }

        protected void LoadHandlers()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "LoadHandlers();", true);
        }

        [WebMethod]
        public static List<DropDownData> GetAisleList()
        {
            List<DropDownData> lstAisle = LocationHandler.GetAisleList();

            return lstAisle;
        }

        [WebMethod]
        public static List<DropDownData> GetZoneList()
        {
            List<DropDownData> lstPhase = LocationHandler.GetPhaseList();

            return lstPhase;
        }

        [WebMethod]
        public static List<DropDownData> GetRackList()
        {
            List<DropDownData> lstRack = LocationHandler.GetRacList();

            return lstRack;
        }


        [WebMethod]
        public static List<DropDownData> GetColumnList()
        {
            List<DropDownData> lstColumn = LocationHandler.GetColumnList();

            return lstColumn;
        }


        [WebMethod]
        public static List<DropDownData> GetBinList()
        {
            List<DropDownData> lstColumn = LocationHandler.GetBinList();

            return lstColumn;
        }
        [WebMethod]
        public static List<DropDownData> GetBayList()
        {
            List<DropDownData> lstColumn = LocationHandler.GetBayList();

            return lstColumn;
        }


        [WebMethod]
        public static List<DropDownData> GetLevelList()
        {
            List<DropDownData> lstColumn = LocationHandler.GetLevelList();

            return lstColumn;
        }

        [WebMethod]
        public static List<DropDownData> GetTenantList()
        {
            return LocationHandler.GetTenantList();

        }

        [WebMethod]
        public static List<DropDownData> GetSuppliersList(int TenantID)
        {
            return LocationHandler.GetSuppliersList(TenantID);

        }


        [WebMethod]
        public static string CreateLocation(LocationCreateRequest data)
        {

            CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
            string responce = LocationHandler.CreateLocations(data, cp);
            return responce;
        }

        [WebMethod]
        public static List<Rack> GetLocations(string zoneID)
        {

            return LocationHandler.GetLocations(zoneID);

        }

       

       
    }


    public class DropDownData
    {
        public int ID { set; get; }
        public string Value { set; get; }
    }


    //public class Aisle
    //{
    //    public string AisleName { set; get; }
    //    public List<Column> RackList { set; get; }
    //}
    public class Rack
    {
        public string RackName { set; get; }
        public List<Column> ColumnList { set; get; }
    }

    public class Column
    {
        public string ColumnName { set; get; }
        public List<Level> LevelList { set; get; }

    }

    public class Level
    {
        public string LevelName { set; get; }
        public List<Bin> binList { set; get; }
    }

    public class Bin
    {
        public string BinName { set; get; }
        public string FullLocation { set; get; }
        public int Status { set; get; }
        public string Tenant { set; get; }
        public string Account { set; get; }
        public string bindata { set; get; }
        public string binRepdata { set; get; }
        public string MCode { set; get; }
        public string LocationID { get; set; }
        public string TenantID { get; set; }
        public string Zone { set; get; }
        public int IsActive { get; set; }
        public int IsQuarantine { get; set; }
        public int IsMixedMaterialOK { get; set; }
        public bool IsFastMoving { get; set; }
        public string Suppliers { get; set; }
        

    }

    public class Tenant
    {
        public string TenantName { set; get; }
        public int TenantID { set; get; }

    }

    public class LocationCreateRequest
    {
        public string PhaseName { set; get; }
        public string AisleName { set; get; }       
        public string RackFrom { set; get; }
        public string RackTo { set; get; }
        public string ColumnFrom { set; get; }
        public string ColumnTo { set; get; }
        public string LevelFrom { set; get; }
        public string LevelTo { set; get; }
        public string BinFrom { set; get; }
        public string BinTo { set; get; }
        public string IsFastMoving { get; set; }
        public string TenantID { get; set; }
        public string SupList { get; set; }
        public string WhCode { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string Length { get; set; }
        public string LocationType { get; set; }
        
        public string ZoneID { get; set; }
        public string TenantName { get; set; }

        public List<DropDownData> RackList { get; set; }
        public List<DropDownData> ColumnList { get; set; }
        public List<DropDownData> LevelList { get; set; }
        public List<DropDownData> BinList { get; set; }

    }

    //public class LocationCreateRequestDummy
    //{
    //    public string PhaseName { set; get; }
    //    public string AisleName { set; get; }
    //    public string RackFrom { set; get; }
    //    public string RackTo { set; get; }
    //    public string ColumnFrom { set; get; }
    //    public string ColumnTo { set; get; }
    //    public string LevelFrom { set; get; }
    //    public string LevelTo { set; get; }
    //    public string BinFrom { set; get; }
    //    public string BinTo { set; get; }
    //    public string IsFastMoving { get; set; }
    //    public int TenantID { get; set; }
    //    public string SupList { get; set; }
    //    public string WhCode { get; set; }
    //    public string Width { get; set; }
    //    public string Height { get; set; }
    //    public string Weight { get; set; }
    //    public string Length { get; set; }
    //    public string LoctionType { get; set; }
    //    public string ZoneID { get; set; }
    //    public string TenantName { get; set; }

    //}

}