using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MRLWMSC21Common;
using System.Text;
using System.Data;
using System.Runtime.InteropServices;

namespace MRLWMSC21.mMaterialManagement
{
    /// <summary>
    /// Summary description for InsertLocationHandler
    /// </summary>
    public class InsertLocationHandler : IHttpHandler
    {

        string result;
        CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;  
        public void ProcessRequest(HttpContext context)
        {
           
            int count = context.Request.QueryString.Count;
            if (count == 5)
            {
                result = insertBeamLocations(context);

            }            
            else 
            {
                result = insertLocations(context);

            }

            context.Response.ContentType = "text/plain";
            context.Response.Write(result);
        }
        public string insertLocations(HttpContext context)
        {

            int reqAisle = int.Parse(context.Request.Params["aisle"]);
            int reqRack = int.Parse(context.Request.Params["rack"]);
            int reqColumn = int.Parse(context.Request.Params["column"]);
            int reqLevel = int.Parse(context.Request.Params["level"]);
            int reqBin = int.Parse(context.Request.Params["bin"]);
            string Zone = context.Request.Params["zone"];
            string Tenant = context.Request.Params["Tenant"];
            string Supplier = context.Request.Params["Supplier"];
            int IsFastMoving = Convert.ToInt32(context.Request.Params["IsFastMoving"]);
            int reqLength = int.Parse(context.Request.Params["length"]);
            int reqWidth = int.Parse(context.Request.Params["width"]);
            int reqHeight = int.Parse(context.Request.Params["height"]);
            int reqMaxWeight = int.Parse(context.Request.Params["maxWeight"]);
            int ZoneID = int.Parse(context.Request.Params["ZoneID"]);
            int LocType = int.Parse(context.Request.Params["LocType"]);



            int _iLastAisle = 0;  int _iLastRack = 0; char _cLastLevel = '0'; int _iLastColumn = 0;

            //DataSet ds = DB.GetDS("SELECT TOP 1 Location, Aisle FROM INV_Location  WHERE ZoneID=" + ZoneID + " AND  IsActive=1 AND IsDeleted=0 ORDER BY LocationID DESC", false);
            DataTable dt = DB.GetDS("SELECT ISNULL(MAX(Aisle),0) Aisle, ISNULL(Max(Rack),0) Rack FROM INV_Location WHERE ZoneID=" + ZoneID + " AND  IsActive=1 AND IsDeleted=0; ", false).Tables[0];
           
            if (dt.Rows[0]["Rack"].ToString() != "0")
            {
                _iLastAisle = Convert.ToInt32(dt.Rows[0]["Aisle"].ToString());
                _iLastRack = Convert.ToInt32(dt.Rows[0]["Rack"].ToString());
                //DataTable dt1 = DB.GetDS("SELECT Max(Level) Lev, MAX(Bay) Col FROM INV_Location WHERE ZoneId="+ZoneID+" AND Rack="+ _iLastRack + " AND IsActive=1 AND IsDeleted=0 ", false).Tables[0];
               
                //_cLastLevel = Convert.ToChar(dt1.Rows[0]["Lev"].ToString());
                //_iLastColumn = Convert.ToInt32(dt1.Rows[0]["Col"].ToString());
            }




            int vRackFrom = _iLastRack + 1;
            int vRackTo = (_iLastRack) + (reqRack * reqAisle); // no.of racks for each Aisle

            char vLevelFrom = 'A';// Convert.ToChar(Convert.ToInt32(_cLastLevel) + 1);
            char vLevelTo = Convert.ToChar(Convert.ToInt32(vLevelFrom) + reqLevel-1);

            int vColFrom = 1;
            int vColTo = reqColumn;

            int vBinFrom = 1;
            int vBinTo = reqBin;

            string sRack;
            string sBin;
            string sCol;
           
           
            StringBuilder sbXML = new StringBuilder();
            sbXML.Append("<LocationHeader>");
            string location = "";


            int count = 0;
            int _iAisle = _iLastAisle;
            for (int rack = vRackFrom; rack <= vRackTo; rack++)
            {
                if (count % reqRack == 0)
                {
                    count = 0; _iAisle = _iAisle + 1;
                }
                count++;
                for (int lev = vLevelFrom; lev <= vLevelTo; lev++)
                {
                    for (int col = vColFrom; col <= vColTo; col++)
                    {
                        for (int loc = vBinFrom; loc <= vBinTo; loc++)
                        {
                            sRack = rack <= 9 ? ("0" + rack) : rack.ToString();
                            sCol = col <= 9 ? ("0" + col) : col.ToString();
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
                
                
            }
            sbXML.Append("</LocationHeader>");

            String sql = "[dbo].[sp_INV_InsertLocation] @Locations=" + DB.SQuote(sbXML.ToString()) + ",@Tenant=" + CommonLogic.IIF(Tenant != "", DB.SQuote(Tenant), "NULL") + ",@CreatedBy=" + cp.UserID + ",@Supplier=" + DB.SQuote(Supplier) + ",@Zone='" + Zone + "',@IsFastMoving=" + IsFastMoving + ",@Length=" + reqLength + ",@Width=" + reqWidth + ",@Height=" + reqHeight + ",@MaxWeight=" + reqMaxWeight + ",@ZoneID=" + ZoneID + ",@LocationTypeID=" + LocType + ",@AccountID=" + cp.AccountID;
            try
            {
                DB.ExecuteSQL(sql);
                string success = "1";
                UpdateMaxCounts(ZoneID);
                UpdateCoordinates(ZoneID);
                
                return success;
            }
            catch (Exception ex)
            {
                DB.ExecuteSQL("INSERT INTO SEC_ErrorLog(Description, ErrorDate, Location) VALUES(" + DB.SQuote(ex.Message) + ", GETDATE(), 'Coordinates')");
                return ex.Message;
            }
        }

        public void UpdateCoordinates(int ZoneID) {
            try
            {
                string query = "select * from INV_Location where ZoneID=" + ZoneID;
                DataSet ds = DB.GetDS(query, false);
                query = "select * from INV_LocationZone where LocationZoneID = " + ZoneID;
                DataSet ds2 = DB.GetDS(query, false);
                int rack = 0, level = 0, bin = 0, column = 0;
                rack = Convert.ToInt32(ds2.Tables[0].Rows[0]["RackCount"]);
                level = Convert.ToInt32(ds2.Tables[0].Rows[0]["LevelCount"]);
                bin = Convert.ToInt32(ds2.Tables[0].Rows[0]["BinCount"]);
                column = Convert.ToInt32(ds2.Tables[0].Rows[0]["ColoumnCount"]);
                string objlocnodes = SetMasterNodes(rack, bin, (rack * bin), level, (bin*column), rack);
                string[] nodes;
                string[] locationCodinates = objlocnodes.Split(':');
                StringBuilder xml = new StringBuilder();
                int strt = 1, end = 1, xt = 0;
                int[] ct = new int[20000];

                for (int rck = 1; rck <= rack; rck++)
                {
                    end = bin * rck;
                    for (int lvl = 1; lvl <= level; lvl++)
                    {
                        for (int bn = strt; bn <= end; bn++)
                        {

                            ct[xt] = bn;
                            xt = xt + 1;

                        }
                    }
                    strt = end + 1;

                }

                xml.Append("<Location>");
                int totalLocs = ds.Tables[0].Rows.Count;
                for (int axies = 0; axies < locationCodinates.Length - 1; axies++)
                {
                    if (axies < totalLocs)
                    {
                        xml.Append("<LocationHeader>");
                        xml.Append("<LocationName>" + Convert.ToString(ds.Tables[0].Rows[axies][1]) + "</LocationName>");
                        nodes = locationCodinates[axies].Split(',');
                        xml.Append("<XCoordinate>" + nodes[0] + "</XCoordinate>");
                        xml.Append("<YCoordinate>" + nodes[1] + "</YCoordinate>");
                        xml.Append("<ZCoordinate>" + nodes[2] + "</ZCoordinate>");
                        xml.Append("<FloorLevelIdentity>" + ct[axies] + "</FloorLevelIdentity>");
                        xml.Append("</LocationHeader>");
                    }
                }
                xml.Append("</Location>");
                string xmlstring = xml.ToString();
                StringBuilder sb = new StringBuilder();
                sb.Append(" Exec [dbo].[UpdateLocationCoOrdinates]");
                sb.Append(" @ZoneID="+ZoneID+", @XML ='" + xml + "'");
                DB.ExecuteSQL(sb.ToString());
            }
            catch (Exception ex)
            {
                DB.ExecuteSQL("INSERT INTO SEC_ErrorLog(Description, ErrorDate, Location) VALUES(" + DB.SQuote(ex.Message) + ", GETDATE(), 'Coordinates')");
            }
        }

        [DllImport("alglib.dll", CallingConvention = CallingConvention.Cdecl)]

        public static extern string SetLocationNodes(Int32 X, Int32 Y, Int32 Count, Int32 NoOfLevels, Int32 NoOfBins, Int32 NoOfRacks);
        public string SetMasterNodes(Int32 x, Int32 y, Int32 count, Int32 noOfLevels, Int32 noOfBins, Int32 noOfRacks)
        {
            string res = SetLocationNodes(x, y, count, noOfLevels, noOfBins, noOfRacks);
            return res;

        }

        public void UpdateMaxCounts(int ZoneID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" DECLARE @ZoneID INT = " + ZoneID);
            sb.Append(" UPDATE INV_LocationZone SET RackCount = Counts.RackCount, LevelCount = Counts.LevCount, BinCount = Counts.BinCount,ColoumnCount = Counts.ColoumnCount ");
            sb.Append(" FROM( ");
            sb.Append(" SELECT  COUNT(DISTINCT Rack) RackCount, COUNT(DISTINCT Level) LevCount, COUNT(DISTINCT RackLocation) BinCount ,COUNT(DISTINCT Level) ColoumnCount");
            sb.Append(" FROM INV_Location where ZoneID = @ZoneID AND IsActive = 1 AND IsDeleted = 0 ");
            sb.Append(" ) AS Counts ");
            sb.Append(" WHERE INV_LocationZone.LocationZoneID = @ZoneID ");
            DB.ExecuteSQL(sb.ToString());
        }
        public string insertBeamLocations(HttpContext context)
        {
            StringBuilder objlocdata = new StringBuilder(20000);
            string vLocationData = "";
            string vBeamCode = context.Request.QueryString["beamcode"];
            int vLocationFrom = int.Parse(context.Request.QueryString["beamlocfrom"]);
            int vLocationTo = int.Parse(context.Request.QueryString["beamlocto"]);

            //int Tenant = int.Parse(context.Request.QueryString["Tenant"]);
            string Tenant = context.Request.QueryString["Tenant"];
            string Supplier = context.Request.QueryString["Supplier"];

            for (int loc = vLocationFrom; loc <= vLocationTo; loc++)
            {
                if (loc <= 9)
                {
                    vLocationData = vBeamCode + "0" + loc;
                }
                else
                {
                    vLocationData = vBeamCode + loc;
                }
                objlocdata.Append(vLocationData + ",");
            }
            vLocationData = objlocdata.ToString();
            vLocationData = vLocationData.Remove(vLocationData.Length - 1);
            try
            {
                String sql = "[dbo].[sp_INV_InsertLocation] @Locations=" + DB.SQuote(vLocationData) + ",@Tenant=" + CommonLogic.IIF(Tenant != "", DB.SQuote(Tenant),"NULL") + ",@CreatedBy=" + cp.UserID + ",@Supplier=" + DB.SQuote(Supplier.ToString());
                DB.ExecuteSQL(sql);
                string success = "1";
                return success;
            }
            catch (Exception)
            {
                string fail = "0";
                return fail;
            }

        }

       

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}