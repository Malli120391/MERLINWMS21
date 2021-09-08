using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21Common;
using System.Data;
using System.Web.Services;
using Newtonsoft.Json;
using System.Text;

namespace MRLWMSC21.mMaterialManagement
{
    public partial class LocationTree : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "MasterData";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DesignLogic.SetInnerPageSubHeading(this.Page, "Location Management");
            if (!IsPostBack)
            {
                CommonLogic.LoadStores(ddlWarehouse, "", 0, false);
                CommonLogic.LoadPrinters(ddlPrinter);
               
                //LoadAisles();
            }
            //DataTable table=null;
            //CommonLogic.SendPrintJob_Location_2x1p5("location",table,


            ViewState["TenantID"] = cp.TenantID;

            if (cp.TenantID != 0)
            {
                tbltenant.Visible = false;
                selSearchCategory.SelectedIndex = 2;
                selSearchCategory.Enabled = false;
            }

        }

        public void LoadAisles()
        {
            //DataSet ds= DB.GetDS("SELECT ID, Aisle FROM INV_LocationAisle WHERE IsActive=1 AND IsDeleted=0", false);
            //ddlAisle.DataSource = ds;
            //ddlAisle.DataTextField = "Aisle";
            //ddlAisle.DataValueField = "ID";
            //ddlAisle.DataBind(); 

            //ddlAislePrint.DataSource = ds;
            //ddlAislePrint.DataTextField = "Aisle";
            //ddlAislePrint.DataValueField = "ID";
            //ddlAislePrint.DataBind();
        }

        [WebMethod]
        public static string GetRacks(int ZoneID)
        {
            DataSet ds = DB.GetDS("SELECT DISTINCT Rack FROM INV_Location WHERE  ZoneID=" + ZoneID + " AND  IsActive=1 AND IsDeleted=0 ORDER BY Rack DESC", false);

            return JsonConvert.SerializeObject(ds.Tables[0]);
        }

        
             [WebMethod]
        public static string GetColumnAndLevel(string rackCode, int ZoneID)
        {
            DataSet ds = null;

            ds = DB.GetDS("select DISTINCT bay AS [Column] FROM   INV_Location WHERE ZoneID="+ZoneID+" AND Rack = '" + rackCode + "' AND IsActive = 1 AND IsDeleted = 0 AND LEN(bay)> 1  ORDER BY bay ASC;     select DISTINCT [Level] AS Level  FROM   INV_Location WHERE ZoneID="+ZoneID+" AND Rack='" + rackCode + "' AND IsActive=1 AND IsDeleted=0 AND ASCII([Level])>=65  ORDER BY [Level] ASC", false);
            
            return JsonConvert.SerializeObject(ds);
        }

        [WebMethod]
        public static string GetBins(string rackCode, string ColCode, string LevCode, int ZoneID)
        {
           // return "";
            DataTable dt = null;
            dt = DB.GetDS("DECLARE  @Col NVARCHAR(10)= '" + ColCode + "', @Lev NVARCHAR(1)= '" + LevCode + "'; select DISTINCT RackLocation AS Bin FROM   INV_Location WHERE ZoneID="+ZoneID+" AND Rack = '" + rackCode + "' AND (@Col='0' OR  bay = '" + ColCode + "') AND (@Lev='0' OR [Level]='" + LevCode + "') AND IsActive = 1 AND IsDeleted = 0 AND LEN(bay)> 1  ORDER BY RackLocation ASC;   ", false).Tables[0];

            return JsonConvert.SerializeObject(dt);
        }

        [WebMethod]
        public static string AddingLocations(string rack, string column, string level, string bin, string zone, string Tenant, string Supplier, string IsFastMoving,
                                            string length, string width, string height, string maxWeight, int ZoneID, int LocType)
        {
            CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
            string reqRack = rack;
            int reqColumn = int.Parse(column==""?"0":column);
            int reqLevel = int.Parse(level==""?"0":level);
            int reqBin = int.Parse(bin);
            string Zone = zone;
            string TenantName = Tenant;
            string SupplierName = Supplier;
            int _iIsFastMoving = Convert.ToInt32(IsFastMoving);
            int reqLength = int.Parse(length);
            int reqWidth = int.Parse(width);
            int reqHeight = int.Parse(height);
            int reqMaxWeight = int.Parse(maxWeight);
            



            string _sLastLoc = ""; int _iLastAisle = 0; string _iLastRack = ""; char _cLastLevel = '0'; int _iLastColumn = 0;

            DataSet ds = DB.GetDS("SELECT TOP 1 Location, Aisle, Level, Bay FROM INV_Location  WHERE ZoneID="+ZoneID+" AND  Rack=" + reqRack + " AND  IsActive=1 AND IsDeleted=0 ORDER BY LocationID DESC", false);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                _sLastLoc = ds.Tables[0].Rows[0][0].ToString();
                _iLastAisle = Convert.ToInt32(ds.Tables[0].Rows[0][1].ToString());
                _cLastLevel = Convert.ToChar(ds.Tables[0].Rows[0][2].ToString());
                _iLastColumn = Convert.ToInt32(ds.Tables[0].Rows[0][3].ToString());
            }

            if (_sLastLoc != "")
            {
                // _iLastAisle++;
                //  _iLastRack = Convert.ToInt32(_sLastLoc.Substring(2, 2));

                _iLastRack = _sLastLoc.Substring(2, 2);
                _cLastLevel = Convert.ToChar(_sLastLoc.Substring(4, 1));
                _iLastColumn = Convert.ToInt32(_sLastLoc.Substring(5, 2));
            }





            char vLevelFrom = Convert.ToChar(Convert.ToInt32(_cLastLevel) + 1);
            char vLevelTo = Convert.ToChar(Convert.ToInt32(vLevelFrom) + reqLevel - 1);

            int vColFrom = _iLastColumn + 1;
            int vColTo = vColFrom + reqColumn - 1;

            int vBinFrom = 1;
            int vBinTo = reqBin;

            string sRack;
            string sBin;
            string sCol;


            StringBuilder sbXML = new StringBuilder();
            sbXML.Append("<LocationHeader>");
            string location = "";



            int _iAisle = _iLastAisle;


            if (reqColumn == 0)
            {
                
                //select DISTINCT[Level]  FROM INV_Location WHERE Rack = '01' AND IsActive = 1 AND IsDeleted = 0 AND bay is NOT NULL ORDER BY[Level] ASC
                DataTable dt = DB.GetDS("select DISTINCT bay AS COL FROM   INV_Location WHERE ZoneID="+ZoneID+" AND  Rack = '"+reqRack+ "' AND IsActive = 1 AND IsDeleted = 0 AND LEN(bay)> 1  ORDER BY bay ASC", false).Tables[0];
                for (int lev = vLevelFrom; lev <= vLevelTo; lev++)
                {
                    for (int col = 0; col < dt.Rows.Count; col++)
                    {
                        for (int loc = vBinFrom; loc <= vBinTo; loc++)
                        {
                            sRack = reqRack;
                            sCol = dt.Rows[col][0].ToString();
                            sBin = loc <= 9 ? ("0" + loc) : loc.ToString();

                            location = Zone + sRack + Convert.ToChar(lev) + sCol + sBin;

                            sbXML.Append("<LocationInfo>");
                            sbXML.Append("<Aisle>" + _iAisle + "</Aisle>");
                            sbXML.Append("<Rack>" + sRack + "</Rack>");
                            sbXML.Append("<Level>" + Convert.ToChar(lev) + "</Level>");
                            sbXML.Append("<Column>" + sCol + "</Column>");
                            sbXML.Append("<Bin>" + sBin + "</Bin>");
                            sbXML.Append("<Location>" + location + "</Location>");
                            sbXML.Append("</LocationInfo>");

                        }
                    }
                }
                sbXML.Append("</LocationHeader>");

            }

            else if (reqLevel == 0)
            {
                DataTable dt = DB.GetDS("select DISTINCT [Level]  FROM   INV_Location WHERE ZoneID="+ZoneID+" AND Rack='"+reqRack+ "'  AND IsActive=1 AND IsDeleted=0 AND ASCII([Level])>=65  ORDER BY [Level] ASC", false).Tables[0];
                for (int lev = 0; lev < dt.Rows.Count; lev++)
                {
                    for (int col = vColFrom; col <= vColTo; col++)
                    {
                        for (int loc = vBinFrom; loc <= vBinTo; loc++)
                        {
                            sRack = reqRack;
                            sCol = col <= 9 ? ("0" + col) : col.ToString();
                            sBin = loc <= 9 ? ("0" + loc) : loc.ToString();

                            location = Zone + sRack + Convert.ToChar(dt.Rows[lev][0].ToString()) + sCol + sBin;

                            sbXML.Append("<LocationInfo>");
                            sbXML.Append("<Aisle>" + _iAisle + "</Aisle>");
                            sbXML.Append("<Rack>" + sRack + "</Rack>");
                            sbXML.Append("<Level>" + Convert.ToChar(dt.Rows[lev][0].ToString()) + "</Level>");
                            sbXML.Append("<Column>" + sCol + "</Column>");
                            sbXML.Append("<Bin>" + sBin + "</Bin>");
                            sbXML.Append("<Location>" + location + "</Location>");
                            sbXML.Append("</LocationInfo>");

                        }
                    }
                }
                sbXML.Append("</LocationHeader>");

            }






            String sql = "[dbo].[sp_INV_InsertLocation] @Locations=" + DB.SQuote(sbXML.ToString()) + ",@Tenant=" + CommonLogic.IIF(TenantName != "", DB.SQuote(TenantName), "NULL") + ",@CreatedBy=" + cp.UserID + ",@Supplier=" + DB.SQuote(Supplier) + ",@Zone=" + Zone + ",@IsFastMoving=" + _iIsFastMoving + ",@Length=" + reqLength + ",@Width=" + reqWidth + ",@Height=" + reqHeight + ",@MaxWeight=" + reqMaxWeight + ",@ZoneID=" + ZoneID + ",@LocationTypeID=" + LocType + ",@AccountID=" + cp.AccountID;
            try
            {
                DB.ExecuteSQL(sql);
                InsertLocationHandler obj = new InsertLocationHandler();
                obj.UpdateMaxCounts(ZoneID);
                obj.UpdateCoordinates(ZoneID);

                return "1";
            }
            catch (Exception ex)
            {
                DB.ExecuteSQL("INSERT INTO SEC_ErrorLog(Description, ErrorDate, Location) VALUES(" + DB.SQuote(ex.Message) + ", GETDATE(), 'Coordinates')");
                return ex.Message;
            }            
        }
    }
}