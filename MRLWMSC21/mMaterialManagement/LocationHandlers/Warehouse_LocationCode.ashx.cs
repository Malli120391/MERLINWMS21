using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using MRLWMSC21Common;

namespace MRLWMSC21.mMaterialManagement
{
    /// <summary>
    /// Summary description for Warehouse_LocationCode
    /// </summary>
    public class Warehouse_LocationCode : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            StringBuilder strKeyValuePair = new StringBuilder();
            string WarehouseID = context.Request.QueryString["whid"];
            strKeyValuePair.Append("[");
            GetLocationCodesForWarehouse(WarehouseID, strKeyValuePair);


            strKeyValuePair.Append("]");
            context.Response.ContentType = "application/json";
            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.Write(strKeyValuePair.ToString());
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        private void GetLocationCodesForWarehouse(String WarehouseID, StringBuilder strBldKeyVal)
        {

            //String sqlStr = "Select LocationZoneID,LocationZoneCode from INV_LocationZone Where WarehouseID="+WarehouseID;
            // Modified by devi on 03/03/2016 to get the Active Zones list only
            String sqlStr = "Select LocationZoneID,LocationZoneCode from INV_LocationZone Where WarehouseID=" + WarehouseID + "AND IsActive=1 AND IsDeleted=0 AND ISNULL(IsDockZone,0) =0";

            IDataReader datareader = DB.GetRS(sqlStr);
            while (datareader.Read())
            {

                strBldKeyVal.Append("{");
                strBldKeyVal.Append("\"LocationCode\":\"" + DB.RSField(datareader, "LocationZoneCode") + "\",");
                strBldKeyVal.Append("\"ID\":\"" + datareader["LocationZoneID"].ToString() + "\"");
                strBldKeyVal.Append("},");

            }

            strBldKeyVal.Remove(strBldKeyVal.Length - 1, 1);

            datareader.Close();
        }
    }
}