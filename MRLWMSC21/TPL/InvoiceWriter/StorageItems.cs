using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MRLWMSC21.TPL.InvoiceWriter
{
    public static class StorageItems
    {
        public static DataSet GetWeekTables(DataSet ds)
        {          

            DataSet dsFinal = new DataSet();
            DataTable dt = ds.Tables[0];
            DataTable dtCodes = dt.DefaultView.ToTable(true, "MCode");

            Dictionary<string, string> dic = new Dictionary<string, string>();

            

            DataTable dtTemp = null;
            int dayCount = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dayCount == 0)
                {
                    dtTemp = new DataTable();
                    dtTemp.Columns.Add("MCode");
                }
                string date = dt.Rows[i]["MDate"].ToString();
                if (dic.Keys.Contains(date))
                {
                    continue;
                }
                else {
                    dayCount++;
                    dic.Add(date, date);
                    if (dayCount == 7)
                    {
                        dtTemp.Columns.Add(date);
                        dtTemp.Columns.Add("UnitCost");
                        dtTemp.Columns.Add("CapacityPerBin");
                        dtTemp.Columns.Add("Price");
                        dtTemp = AddMaterialCodes(dtTemp, dtCodes, dt);

                        dsFinal.Tables.Add(dtTemp);
                        dayCount = 0;
                    }
                    else {
                        dtTemp.Columns.Add(date);
                    }                    
                }

                
            }

            if (dayCount != 6 && dayCount != 0)
            {
                // dtTemp.Columns.Add(date);
                dtTemp.Columns.Add("UnitCost");
                dtTemp.Columns.Add("CapacityPerBin");
                dtTemp.Columns.Add("Price");
                dtTemp = AddMaterialCodes(dtTemp, dtCodes, dt);

                dsFinal.Tables.Add(dtTemp);
                dayCount = 0;
            }

            return dsFinal;
            
        }


        public static DataTable AddMaterialCodes(DataTable dtTemp, DataTable dtCodes, DataTable dt)        
        {
            for (int i = 0; i < dtCodes.Rows.Count; i++)
            {
                dtTemp.Rows.Add(dtCodes.Rows[i][0]);
                for (int j = 0; j < dtTemp.Rows.Count; j++)
                {
                    decimal WeekTotalAval = 0;
                    for (int k = 1; k < dtTemp.Columns.Count; k++)
                    {
                        if (dtTemp.Columns[k].ColumnName != "UnitCost" && dtTemp.Columns[k].ColumnName != "CapacityPerBin" && dtTemp.Columns[k].ColumnName != "Price")
                        {
                            string rowInfo = GetValue(dtTemp.Rows[j][0].ToString(), dtTemp.Columns[k].ColumnName, dt);
                            if (rowInfo != "" && rowInfo.Contains('|'))
                            {
                                dtTemp.Rows[j][k] = rowInfo.Split('|')[0];
                                WeekTotalAval += Convert.ToDecimal(rowInfo.Split('|')[0]);

                                dtTemp.Rows[j]["UnitCost"] = rowInfo.Split('|')[1];
                                dtTemp.Rows[j]["CapacityPerBin"] = rowInfo.Split('|')[2];
                                dtTemp.Rows[j]["Price"] = ((WeekTotalAval / Convert.ToDecimal(rowInfo.Split('|')[2].Split(' ')[0])) * Convert.ToDecimal(rowInfo.Split('|')[1].Split(' ')[0])).ToString("##");
                            }
                        }
                    } 
                }

            }
            return dtTemp;
        }


        public static string GetValue(string MCode, string date, DataTable dt)
        {
            string retValue = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                if (dt.Rows[i]["MCode"].ToString() == MCode && dt.Rows[i]["MDate"].ToString() == date)
                {
                    decimal avlQty = Convert.ToDecimal(dt.Rows[i]["AvailableQty"]);
                    string UnitCost = dt.Rows[i]["UnitCost"].ToString();
                    string capBin = dt.Rows[i]["CapacityPerBin"].ToString();
                    retValue = avlQty.ToString() +"|"+ UnitCost +"|"+ capBin;

                    //string Price = "0";
                    //if (UnitCost != "" && UnitCost.IndexOf(' ') > -1)
                    //{
                    //    Price = (Convert.ToDecimal(UnitCost.Split(' ')[0]) * avlQty).ToString("##");
                    //}
                    //retValue = retValue + "|" + Price;
                    break;
                }
               
            }
            return retValue;
        }
    }


   
}