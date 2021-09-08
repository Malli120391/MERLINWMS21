using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MRLWMSC21Common;
using System.Text;
using System.Data;

namespace MRLWMSC21.mMaterialManagement.LocationHandlers
{
    /// <summary>
    /// Printing bulk Location labels ---Added by Prasanna om 18/09/2017
    /// </summary>
    public class PrintLocationHandler : IHttpHandler
    {
        string result;
        CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;  
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            result = PrintBulkLocations(context);// PrintLocations(context);
            context.Response.Write(result);
        }
        public string PrintLocations(HttpContext context)
        {

            int ZoneID = int.Parse(context.Request.Params["ZoneID"]);
            int vAisleFrom = int.Parse(context.Request.Params["aislefrom"]);
            int vAisleTo = int.Parse(context.Request.Params["aisleto"]);

            int vBayFrom = int.Parse(context.Request.Params["bayfrom"]);
            int vBayTo = int.Parse(context.Request.Params["bayto"]);
            int vBeamFrom = int.Parse(context.Request.Params["beamfrom"]);
            int vBeamTo = int.Parse(context.Request.Params["beamto"]);
            int vLocationFrom = int.Parse(context.Request.Params["locfrom"]);
            int vLocationTo = int.Parse(context.Request.Params["locto"]);
            string vZone = context.Request.Params["zone"];
            string vBay;
            string vLocation;
            string PrinterIP = context.Request.Params["PrinterIP"];


           

            StringBuilder locationdata = new StringBuilder();
            string location = "";

            for (int bay = vBayFrom; bay <= vBayTo; bay++)
            {
                for (int beam = vBeamFrom; beam <= vBeamTo; beam++)
                {
                    for (int loc = vLocationFrom; loc <= vLocationTo; loc++)
                    {
                        if (bay <= 9)
                        {
                            vBay = "0" + bay;
                        }
                        else
                        {
                            vBay = "" + bay;
                        }
                        if (loc <= 9)
                        {
                            vLocation = "0" + loc;
                        }
                        else
                        {
                            vLocation = "" + loc;
                        }

                        location = vZone + vBay + Convert.ToChar(beam) + vLocation;
                        locationdata.Append(location + ",");

                    }
                }
            }
            string sLocationstring = locationdata.ToString();
            sLocationstring = sLocationstring.Remove(sLocationstring.Length - 1);
            string[] locationsplit = sLocationstring.Split(',');
            try
            {

                for (int i = 0; i < locationsplit.Length; i++)
                {
                    if (locationsplit[i] != "")
                    {
                        MRLWMSC21Common.PrintLocationLabel printlabel = new MRLWMSC21Common.PrintLocationLabel();
                        printlabel.PrintLable(locationsplit[i].ToString(), PrinterIP);
                        
                    }
                }

                string success = "1";
                return success;

            }
            catch (Exception ex)
            {

                
            }
            return "";

        }


        public string PrintBulkLocations(HttpContext context)
        {
            string ZoneID = context.Request.Params["ZoneID"];
            string Zone = context.Request.Params["Zone"];
            //int Rack = Convert.ToInt32(context.Request.Params["Rack"]);            
            
            int Column = Convert.ToInt32(context.Request.Params["Column"]);
            string Level = context.Request.Params["Level"];
            int Bin = Convert.ToInt32(context.Request.Params["Bin"]);

            //string RackCode = Rack <= "9" ? "0" + Rack.ToString() : Rack.ToString();
            string RackCode = context.Request.Params["Rack"].ToString();
            string ColCode = Column <= 9 ? "0" + Column.ToString() : Column.ToString();
            char levCode = Level == "0" ? '0' : Convert.ToChar(Level);
            string BinCode = Bin <= 9 ? "0" + Bin.ToString() : Bin.ToString();

            StringBuilder sb = new StringBuilder();
            sb.Append(" DECLARE @Rack NVARCHAR(2) = '"+RackCode+"', @Col NVARCHAR(10)= '"+ColCode+"', @Lev NVARCHAR(1)= '"+levCode+"', @Bin NVARCHAR(2)= '"+BinCode+"' ");
            sb.Append(" SELECT Location FROM INV_Location WHERE ZoneID = '"+ZoneID+"' AND Rack = @Rack AND (@Col = '00' OR Bay = @Col) AND (@Lev = '0' OR[Level] = @Lev) AND (@Bin = '00' OR RackLocation = @Bin) ");
            sb.Append("  AND IsActive = 1 AND IsDeleted = 0 ");

            DataTable dtLoc = DB.GetDS(sb.ToString(), false).Tables[0];








            //DataTable dtLoc = null;

            //if (Rack == 0)
            //{

            //}
            //else if (ColOrLev == 0)
            //{
            //    dtLoc = DB.GetDS(" select DISTINCT Location FROM   INV_Location WHERE Rack = '" + RackCode + "' AND zone='" + Zone + "' AND IsActive = 1 AND IsDeleted = 0 AND LEN(bay)> 1  ", false).Tables[0];
            //}
            //else {
            //    if (ColOrLev == 1) //Column
            //    {
            //        if (Column == 0)
            //        {

            //            dtLoc = DB.GetDS(" select DISTINCT Location FROM   INV_Location WHERE Rack = '" + RackCode + "' AND zone='" + Zone + "' AND IsActive = 1 AND IsDeleted = 0 AND LEN(bay)> 1  ", false).Tables[0];
            //        }
            //        else
            //        {
                        
            //            if (Bin == 0)
            //            {                            
            //                dtLoc = DB.GetDS(" select DISTINCT Location FROM   INV_Location WHERE Rack = '" + RackCode + "' AND zone='" + Zone + "' AND Bay='" + ColCode + "' AND IsActive = 1 AND IsDeleted = 0 AND LEN(bay)> 1  ", false).Tables[0];
            //            }
            //            else
            //            {
            //                dtLoc = DB.GetDS(" select DISTINCT Location FROM   INV_Location WHERE Rack = '" + RackCode + "' AND zone='" + Zone + "' AND Bay='" + ColCode + "' AND RackLocation='" + Bin + "' AND IsActive = 1 AND IsDeleted = 0 AND LEN(bay)> 1  ", false).Tables[0];
            //            }
            //        }

            //    }
            //    else if (ColOrLev == 2) //Level
            //    {
            //        if (Level == "0")
            //        {
            //            dtLoc = DB.GetDS(" select DISTINCT Location  FROM   INV_Location WHERE Rack = '"+RackCode+"' AND zone = '"+Zone+"' AND IsActive = 1 AND IsDeleted = 0 AND ASCII([Level])>= 65  ORDER BY[Level] ASC", false).Tables[0];

            //        }
            //        else 
            //        {
            //            char levCode = Convert.ToChar(Level);
            //            if (Bin == 0)
            //            {
            //                dtLoc = DB.GetDS(" select DISTINCT Location  FROM   INV_Location WHERE Rack = '" + RackCode + "' AND zone = '" + Zone + "' AND[Level] = '" + levCode + "' AND IsActive = 1 AND IsDeleted = 0 AND ASCII([Level])>= 65  ORDER BY[Level] ASC", false).Tables[0];
            //            }
            //            else {
            //                dtLoc = DB.GetDS(" select DISTINCT Location  FROM   INV_Location WHERE Rack = '" + RackCode + "' AND zone ='" + Zone + "' AND[Level] = '" + levCode + "' RackLocation='" + Bin + "' AND IsActive = 1 AND IsDeleted = 0 AND ASCII([Level])>= 65  ORDER BY[Level] ASC", false).Tables[0];
            //            }

            //        }
            //    }
            //}

            
            string PrinterIP = context.Request.Params["PrinterIP"];




            StringBuilder locationdata = new StringBuilder();
           
            for (int i=0; i < dtLoc.Rows.Count; i++)
            {
               
                locationdata.Append(dtLoc.Rows[i][0].ToString() + ",");

                    
                
            }
            string sLocationstring = locationdata.ToString();
            sLocationstring = sLocationstring.Remove(sLocationstring.Length - 1);
            string[] locationsplit = sLocationstring.Split(',');    
            try
            {
                string success = "";
                for (int i = 0; i < locationsplit.Length; i++)
                {
                    if (locationsplit[i] != "")
                    {
                        MRLWMSC21Common.PrintLocationLabel printlabel = new MRLWMSC21Common.PrintLocationLabel();
                        //Commeneted by M.D Prasad on 2-1-2019 //////success += printlabel.PrintLable(locationsplit[i].ToString(), PrinterIP);
                        //success += printlabel.PrintLable_ZPL(locationsplit[i].ToString(), PrinterIP);
                        //success += printlabel.PrintLable_ZPL_DisplayLocCode(locationsplit[i].ToString(), PrinterIP);
                        success += printlabel.PrintLable_ZPL_FromDB(locationsplit[i].ToString(), PrinterIP);
                    }
                }

                
                return success;

            }
            catch (Exception ex)
            {
                return "";
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