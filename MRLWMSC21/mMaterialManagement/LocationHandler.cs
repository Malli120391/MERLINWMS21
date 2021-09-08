using MRLWMSC21Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace MRLWMSC21.mMaterialManagement
{
    public class LocationHandler
    {

        public static List<DropDownData> GetAisleList()
        {
            string query = " SELECT ID, Aisle AS VALUE FROM INV_LOCATIONAisle  WHERE IsActive=1 AND IsDeleted=0 ";
            try
            {
                List<DropDownData> lstAisle = new List<DropDownData>();
                lstAisle.Add(new DropDownData() { ID = 0, Value = "-Select-" });
                DataSet ds = DB.GetDS(query, false);
                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        lstAisle.Add(new DropDownData() { ID = Convert.ToInt16(row["ID"]), Value = row["VALUE"].ToString() });
                    }
                }
                return lstAisle;

            }
            catch (Exception ex)
            {
                throw new Exception("Error while loading Aisle List" + ex.Message);
            }
        }

        public static List<DropDownData> GetPhaseList()
        {
            string query = " SELECT LOCATIONZONEID ID,LOCATIONZONECODE VALUE FROM INV_LOCATIONZONE ";
            try
            {
                List<DropDownData> lstPhase = new List<DropDownData>();
                lstPhase.Add(new DropDownData() { ID = 0, Value = "-Select-" });
                DataSet ds = DB.GetDS(query, false);
                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        lstPhase.Add(new DropDownData() { ID = Convert.ToInt16(row["ID"]), Value = row["VALUE"].ToString() });
                    }
                }
                return lstPhase;

            }
            catch (Exception ex)
            {
                throw new Exception("Error while loading Phase List" + ex.Message);
            }

        }
        public static List<DropDownData> GetRacList()
        {
            string query = " SELECT LocationRackID ID,LocationRack Value FROM INV_LOCATIONRACK WHERE IsActive = 1 AND IsDeleted = 0 order by LocationRackID asc ";
            try
            {
                List<DropDownData> lstRack = new List<DropDownData>();
                lstRack.Add(new DropDownData() { ID = 0, Value = "-Select-" });
                DataSet ds = DB.GetDS(query, false);
                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        lstRack.Add(new DropDownData() { ID = Convert.ToInt16(row["ID"]), Value = row["Value"].ToString() });
                    }
                }
                return lstRack;

            }
            catch (Exception ex)
            {
                throw new Exception("Error while loading Rack List" + ex.Message);
            }
        }
        public static List<DropDownData> GetColumnList()
        {
            string query = " SELECT LocationColumnID ID,LOCATIONCOLUMN VALUE FROM INV_LOCATIONCOLUMN  WHERE IsActive = 1 AND IsDeleted = 0 order by LocationColumnID asc ";
            try
            {
                List<DropDownData> lstColumn = new List<DropDownData>();
                lstColumn.Add(new DropDownData() { ID = 0, Value = "-Select-" });
                DataSet ds = DB.GetDS(query, false);
                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        lstColumn.Add(new DropDownData() { ID = Convert.ToInt16(row["ID"]), Value = row["VALUE"].ToString().Trim() });
                    }
                }
                return lstColumn;

            }
            catch (Exception ex)
            {
                throw new Exception("Error while loading Column List" + ex.Message);
            }
        }
        public static List<DropDownData> GetBinList()
        {
            string query = " SELECT ID ID,binNumber Value FROM Inv_LocationBin  WHERE IsActive = 1 AND IsDeleted = 0 order by id asc ";
            try
            {
                List<DropDownData> lstBin = new List<DropDownData>();
                lstBin.Add(new DropDownData() { ID = 0, Value = "-Select-" });
                DataSet ds = DB.GetDS(query, false);
                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        lstBin.Add(new DropDownData() { ID = Convert.ToInt16(row["ID"]), Value = row["Value"].ToString() });
                    }
                }
                return lstBin;

            }
            catch (Exception ex)
            {
                throw new Exception("Error while loading Bin List" + ex.Message);
            }
        }
        public static List<DropDownData> GetBayList()
        {
            string query = " SELECT LocationLevelID  AS ID,LocationLevel as Value FROM INV_LocationLevel  WHERE IsActive=1 AND IsDeleted=0 order by LocationLevelID asc ";
            try
            {
                List<DropDownData> lstBin = new List<DropDownData>();
                lstBin.Add(new DropDownData() { ID = 0, Value = "-Select-" });
                DataSet ds = DB.GetDS(query, false);
                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        lstBin.Add(new DropDownData() { ID = Convert.ToInt16(row["ID"]), Value = row["Value"].ToString() });
                    }
                }
                return lstBin;

            }
            catch (Exception ex)
            {
                throw new Exception("Error while loading Bin List" + ex.Message);
            }
        }
        public static List<DropDownData> GetLevelList()
        {
            string query = " SELECT LocationLevelID  AS ID,LocationLevel as Value FROM INV_LocationLevel  WHERE IsActive=1 AND IsDeleted=0 order by LocationLevelID asc ";
            try
            {
                List<DropDownData> lstBin = new List<DropDownData>();
                lstBin.Add(new DropDownData() { ID = 0, Value = "-Select-" });
                DataSet ds = DB.GetDS(query, false);
                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        lstBin.Add(new DropDownData() { ID = Convert.ToInt16(row["ID"]), Value = row["Value"].ToString() });
                    }
                }
                return lstBin;

            }
            catch (Exception ex)
            {
                throw new Exception("Error while loading Bin List" + ex.Message);
            }
        }
        public static List<DropDownData> GetTenantList()
        {
            string query = " SELECT TenantID ID, TenantName Value FROM TPL_Tenant WHERE IsActive=1 AND IsDeleted=0";
            try
            {
                List<DropDownData> lstTenant = new List<DropDownData>();
                lstTenant.Add(new DropDownData() { ID = 0, Value = "-Select-" });
                DataSet ds = DB.GetDS(query, false);
                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        lstTenant.Add(new DropDownData() { ID = Convert.ToInt16(row["ID"]), Value = row["Value"].ToString() });
                    }
                }
                return lstTenant;

            }
            catch (Exception ex)
            {
                throw new Exception("Error while loading Bin List" + ex.Message);
            }
        }


        public static List<DropDownData> GetSuppliersList(int TenantID)
        {
            string query = @"SELECT TS.SupplierID ID, SUP.SupplierName Value FROM TPL_Tenant_Supplier TS
                        JOIN MMT_Supplier SUP ON TS.SupplierID = SUP.SupplierID AND SUP.IsActive = 1 AND SUP.IsDeleted = 0
                        WHERE TS.TenantID = "+TenantID+" AND TS.SupplierID > 0 AND TS.IsActive = 1 AND TS.IsDeleted = 0";
            try
            {
                List<DropDownData> lstSup = new List<DropDownData>();
                lstSup.Add(new DropDownData() { ID = 0, Value = "-Select-" });
                DataSet ds = DB.GetDS(query, true);
                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        lstSup.Add(new DropDownData() { ID = Convert.ToInt16(row["ID"]), Value = row["Value"].ToString() });
                    }
                }
                return lstSup;

            }
            catch (Exception ex)
            {
                throw new Exception("Error while loading Supplier List" + ex.Message);
            }
        }


        public static string CreateLocations(LocationCreateRequest request,CustomPrincipal cp)
        {
            string locationPattern = MRLWMSC21Common.CommonLogic.Application("LocationPattern");
            //List<DropDownData> phaseList = GetPhaseList();
            //List<DropDownData> aisleList = GetAisleList();
            //List<DropDownData> rackList = GetRacList();
            //List<DropDownData> columnList = GetColumnList();
            //List<DropDownData> levelList = GetBayList();
            //List<DropDownData> binList = GetBinList();
            //List<DropDownData> tenantList = GetTenantList();

            //int aisleFrom = Convert.ToInt32(request.AisleName);
           
            int rackFrom = Convert.ToInt32(request.RackFrom);
            int rackTo = Convert.ToInt32(request.RackTo);

            int columnFrom = Convert.ToInt32(request.ColumnFrom);
            int columnTo = Convert.ToInt32(request.ColumnTo);

            int levelFrom = Convert.ToInt32(request.LevelFrom);
            int levelTo = Convert.ToInt32(request.LevelTo);

            int binFrom = Convert.ToInt32(request.BinFrom);
            int binTo = Convert.ToInt32(request.BinTo);
            int fastMoving = Convert.ToInt32(request.IsFastMoving);
          
            int tenantID = Convert.ToInt32(request.TenantID!=""?request.TenantID:"0");
            string supList = request.SupList.ToString();


            string rackLable = string.Empty;
            string columnLable = string.Empty;
            string levelLable = string.Empty;
            string binLable = string.Empty;
            string locationLable = string.Empty;
            string LableDisplayCode = string.Empty;

            List<string> lstLocation = new List<string>();
            StringBuilder sbXML = new StringBuilder();
            sbXML.Append("<LocationHeader>");
            for (int rackindex = rackFrom; rackindex <= rackTo; rackindex++)
            {
                for (int colIndex = columnFrom; colIndex <= columnTo; colIndex++)
                {
                    for (int levelIndex = levelFrom; levelIndex <= levelTo; levelIndex++)
                    {
                        for (int binIndex = binFrom; binIndex <= binTo; binIndex++)
                        {
                          

                            //Rack lable
                            rackLable = request.RackList.Find(a => a.ID == rackindex).Value; //01 03 04 06
                            //Column lable
                            columnLable = request.ColumnList.Find(a => a.ID == colIndex).Value;
                            //Level lable
                            levelLable = request.LevelList.Find(a => a.ID == levelIndex).Value;
                            //Level lable
                            binLable = request.BinList.Find(a => a.ID == binIndex).Value;


                            //locationLable = request.PhaseName + columnLable + rackLable + levelLable + binLable;
                            if (locationPattern == "WH-ZONE-RACK-COL-LEVEL-BIN")
                            {
                                locationLable = request.WhCode + "" + request.PhaseName + "" + rackLable + "" + columnLable + "" + levelLable + "" + binLable;
                                LableDisplayCode= request.WhCode + "-" + request.PhaseName + "-" + rackLable + "-" + columnLable + "-" + levelLable + "-" + binLable;
                            }
                            else if (locationPattern == "WH-ZONE-RACK-LEVEL-COL-BIN")
                            {
                                locationLable = request.WhCode + "" + request.PhaseName + "" + rackLable + "" + levelLable + "" + columnLable + "" + binLable;
                                LableDisplayCode = request.WhCode + "-" + request.PhaseName + "-" + rackLable + "-" + levelLable + "-" + columnLable + "-" + binLable;
                            }
                            else if (locationPattern == "WH-ZONE-RACK-LEVEL-BIN")
                            {
                                locationLable = request.WhCode + "" + request.PhaseName + "" + rackLable + "" + levelLable + "" + binLable;
                                LableDisplayCode = request.WhCode + "-" + request.PhaseName + "-" + rackLable + "-" + levelLable + "-" + binLable;
                            }
                            else if (locationPattern == "ZONE-RACK-COL-LEVEL-BIN")
                            {
                                locationLable = request.PhaseName + "" + rackLable + "" + columnLable + levelLable + "" + binLable;
                                LableDisplayCode = request.PhaseName + "-" + rackLable + "-" + columnLable + levelLable + "-" + binLable;

                            }
                            else if (locationPattern == "ZONE-RACK-LEVEL-COL-BIN")
                            {
                                locationLable = request.PhaseName + "" + rackLable + "" + levelLable + "" + columnLable + "" + binLable;
                                LableDisplayCode = request.PhaseName + "-" + rackLable + "-" + levelLable + "-" + columnLable + "-" + binLable;

                            }
                            else if (locationPattern == "WH-ZONE-RACK-BIN-LEVEL")
                            {
                                locationLable = request.WhCode + "" + request.PhaseName + "" + rackLable + "" + binLable + "" + levelLable;
                                LableDisplayCode = request.WhCode + "-" + request.PhaseName + "-" + rackLable + "-" + binLable + "-" + levelLable;
                            }
                            else if (locationPattern == "WH-ZONE-RACK-BIN-LEVEL-COL")
                            {
                                locationLable = request.WhCode + "" + request.PhaseName + "" + rackLable + "" + binLable + "" + levelLable + "" + columnLable;
                                LableDisplayCode = request.WhCode + "-" + request.PhaseName + "-" + rackLable + "-" + binLable + "-" + levelLable+"-"+columnLable;

                            }
                            else if (locationPattern == "RACK-COL-LEVEL-BIN")
                            {
                                locationLable = rackLable + "" + columnLable + "" + levelLable;
                                LableDisplayCode = rackLable + "-" + columnLable + "-" + levelLable;

                            }

                            else
                            {
                                return "Bin Configuration not available";
                            }



                     

                            sbXML.Append("<LocationInfo>");
                            sbXML.Append("<Aisle>0</Aisle>");
                            sbXML.Append("<Rack>" + rackLable + "</Rack>");
                            sbXML.Append("<Level>" + levelLable + "</Level>");
                            sbXML.Append("<Column>" + columnLable + "</Column>");
                            sbXML.Append("<Bin>" + binLable + "</Bin>");
                            sbXML.Append("<Location>" + locationLable + "</Location>");
                            sbXML.Append("<LableDisplayCode>" + LableDisplayCode + "</LableDisplayCode>");
                            sbXML.Append("</LocationInfo>");

                            //sbXML.Append("<LocationInfo>");
                            //sbXML.Append("<PHASE>" + request.PhaseName + "</PHASE>");
                            //sbXML.Append("<Aisle>" + request.AisleName + "</Aisle>");
                            //sbXML.Append("<Rack>" + rackLable + "</Rack>");
                            //sbXML.Append("<Column>" + columnLable + "</Column>"); //Bay
                            //sbXML.Append("<Level>" + levelLable + "</Level>");
                            //sbXML.Append("<Bin>" + binLable + "</Bin>");
                            //sbXML.Append("<Location>" + locationLable + "</Location>");
                            //sbXML.Append("<IsFastMoving>" + fastMoving + "</IsFastMoving>");
                            //sbXML.Append("<TenantID>" + tenantID + "</TenantID>");
                            //sbXML.Append("<SupList>" + supList + "</SupList>");
                            //sbXML.Append("</LocationInfo>");

                            //String sql = "[dbo].[sp_INV_InsertLocation] @Locations=" + DB.SQuote(sbXML.ToString()) +
                            //    ",@Tenant=" + CommonLogic.IIF(Tenant != "", DB.SQuote(Tenant), "NULL") + ",@CreatedBy=" + cp.UserID +
                            //    ",@Supplier=" + DB.SQuote(Supplier) + ",@Zone='" + Zone + "',@IsFastMoving=" + IsFastMoving + 
                            //    ",@Length=" + reqLength + ",@Width=" + reqWidth + ",@Height=" + reqHeight +
                            //    ",@MaxWeight=" + reqMaxWeight + ",@ZoneID=" + ZoneID + 
                            //    ",@LocationTypeID=" + LocType + ",@AccountID=" + cp.AccountID;
                        }
                    }
                }

            }
            sbXML.Append("</LocationHeader>");

            string locationData = sbXML.ToString();
            try
            {

                //DB.ExecuteSQL("EXEC [dbo].[sp_Create_Bulk_Locations] @XML_Data=" + DB.SQuote(locationData));

                String sql = "[dbo].[sp_INV_InsertLocation] @Locations=" + DB.SQuote(sbXML.ToString()) +
                    ",@Tenant=" + CommonLogic.IIF(request.TenantName != "", DB.SQuote(request.TenantName), "NULL") + ",@CreatedBy=" + cp.UserID +
                    ",@Supplier=" + DB.SQuote(request.SupList) + ",@Zone='" + request.PhaseName + "',@IsFastMoving=" + request.IsFastMoving +
                    ",@Length=" + request.Length + ",@Width=" + request.Width + ",@Height=" + request.Height +
                    ",@MaxWeight=" + request.Length + ",@ZoneID=" +request.ZoneID +
                    ",@LocationTypeID=" + request.LocationType + ",@AccountID=" + cp.AccountID;

            
                    DB.ExecuteSQL(sql);

                    return "1";
            }
            catch (Exception ex)
            {
                DB.ExecuteSQL("INSERT INTO SEC_ErrorLog(Description, ErrorDate, Location) VALUES(" + DB.SQuote(ex.Message) + ", GETDATE(), 'Coordinates')");
                return ex.Message;
            }


        }



        public static List<Rack> GetLocations(string zoneID)
        {
            List<Rack> rackList = new List<Rack>();

           // string strLocQuery = " SELECT Zone, RACK, Bay RackColumn, Level RackLevel, RackLocation, Location FROM INV_Location WHERE IsActive=1 AND IsDeleted=0 AND ZONE = " + DB.SQuote(phaseName);
           // strLocQuery += " ORDER BY Rack, Bay ,Level, RackLocation ";

            string strLocQuery = "EXEC [sp_TPL_GetTenantLocDataForZone] @ZoneID = '"+ zoneID + "',@TenantID = 0";


            try
            {

                DataSet dsLocations = DB.GetDS(strLocQuery, false);

                foreach (DataRow dr in dsLocations.Tables[0].Rows)
                {
                    // Find rack from rack list
                    Rack rack = rackList.FirstOrDefault(r => r.RackName == dr["RACK"].ToString());
                    if (rack == null)
                    {
                        // Find rack from dataset
                        rack = new Rack();
                        rack.RackName = dr["RACK"].ToString();
                        rackList.Add(rack);
                    }

                    // Find column list from Rack
                    List<Column> columnList = rack.ColumnList;
                    if (columnList == null)
                    {
                        // create new column list and add to rack
                        columnList = new List<Column>();
                        columnList.Add(new Column() { ColumnName = dr["RackColumn"].ToString() });
                        rack.ColumnList = columnList;
                    }


                    // Find column from column list
                    Column column = columnList.FirstOrDefault(a => a.ColumnName == dr["RackColumn"].ToString());
                    if (column == null)
                    {

                        // Create new column from data set
                        column = new Column();
                        column.ColumnName = dr["RackColumn"].ToString();

                        // Add column to column list
                        columnList.Add(column);
                    }


                    // Find Level List from Column
                    List<Level> levelList = column.LevelList;

                    if (levelList == null)
                    {
                        // Create new  Level List from dataset
                        levelList = new List<Level>();
                        levelList.Add(new Level() { LevelName = dr["RackLevel"].ToString() });

                        // add new level list to column
                        column.LevelList = levelList;
                    }


                    // Find level from level list
                    Level level = levelList.FirstOrDefault(a => a.LevelName == dr["RackLevel"].ToString());
                    if (level == null)
                    {
                        // Create new level from Data set
                        level = new Level();
                        level.LevelName = dr["RackLevel"].ToString();

                        //add level to level list
                        levelList.Add(level);
                    }


                    // Find bin list from level
                    List<Bin> binList = level.binList;

                    if (binList == null)
                    {
                        // Create new bin list from dataset
                        binList = new List<Bin>();
                        binList.Add(new Bin() { BinName = dr["RackLocation"].ToString(), FullLocation = dr["Location"].ToString(), bindata = dr["LocCont"].ToString(), binRepdata=dr["BinRepl"].ToString(), Tenant= dr["Tenant"].ToString(), Account= dr["Account"].ToString(), TenantID = dr["TenantID"].ToString(), LocationID = dr["LocationID"].ToString(), MCode = dr["MCode"].ToString(), IsActive = Convert.ToInt16(dr["IsActive"].ToString()), IsQuarantine = Convert.ToInt16(dr["IsQuarantine"].ToString()), IsMixedMaterialOK = Convert.ToInt16(dr["IsMixedMaterialOK"].ToString()), IsFastMoving = Convert.ToBoolean(dr["IsFastMoving"].ToString()), Suppliers= dr["Suppliers"].ToString() });

                        // add bin list to level
                        level.binList = binList;

                    }
                    else
                    {
                        // add bin to binlist
                        binList.Add(new Bin() { BinName = dr["RackLocation"].ToString(), FullLocation = dr["Location"].ToString(), bindata= dr["LocCont"].ToString(), binRepdata = dr["BinRepl"].ToString(), Tenant = dr["Tenant"].ToString(), Account = dr["Account"].ToString(), TenantID = dr["TenantID"].ToString(), LocationID = dr["LocationID"].ToString(), MCode = dr["MCode"].ToString(), IsActive = Convert.ToInt16(dr["IsActive"].ToString()), IsQuarantine = Convert.ToInt16(dr["IsQuarantine"].ToString()), IsMixedMaterialOK = Convert.ToInt16(dr["IsMixedMaterialOK"].ToString()), IsFastMoving = Convert.ToBoolean(dr["IsFastMoving"].ToString()), Suppliers = dr["Suppliers"].ToString() });
                    }

                }


            }
            catch (Exception ex)
            {
            }

            //var newList = people.OrderBy(x => x.LastName).ToList();
            for (int i = 0; i < rackList.Count; i++)
            {
                for (int j = 0; j < rackList[i].ColumnList.Count; j++)
                {
                    rackList[i].ColumnList[j].LevelList = rackList[i].ColumnList[j].LevelList.OrderByDescending(x => x.LevelName).ToList();
                }                
            }
            //List<Rack> SortedList = rackList[].OrderBy(o => o.OrderDate).ToList();
            return rackList;
        }
        

    }
}