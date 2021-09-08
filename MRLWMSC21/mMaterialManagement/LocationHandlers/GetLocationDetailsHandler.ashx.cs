using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MRLWMSC21Common;
using System.Data;
using System.Text;

namespace MRLWMSC21.mMaterialManagement.LocationHandlers
{
    /// <summary>
    /// Summary description for GetLocationDetailsHandler
    /// </summary>
    public class GetLocationDetailsHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string location = context.Request.Params["location"];
            string json = LoadBinDetails(location);
            context.Response.ContentType = "application/json";
            context.Response.Write(json);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        private string LoadBinDetails(String strLocation)
        {
            string sCmd = "EXEC [sp_Inv_GetLocationDetailByLoc] @Location=" + DB.SQuote(strLocation);
            StringBuilder locationData = new StringBuilder();
            string json = "";
            locationData.Append("[");

            IDataReader rsBinDetails = DB.GetRS(sCmd);

            while (rsBinDetails.Read())
            {
                string width = DB.RSFieldInt(rsBinDetails, "Width").ToString();
                locationData.Append("{\"width\":\"" + width + "\"},"); //0
               
                string height = DB.RSFieldInt(rsBinDetails, "Height").ToString();
                locationData.Append("{\"height\":\"" + height + "\"},"); //1
               
                string length = DB.RSFieldInt(rsBinDetails, "Length").ToString();
                locationData.Append("{\"length\":\"" + length + "\"},"); //2
                
                string maxWeight = DB.RSFieldDecimal(rsBinDetails, "MaxWeight").ToString();
                locationData.Append("{\"maxweight\":\"" + maxWeight + "\"},"); //3
                
                string isMixedMaterialok = DB.RSFieldTinyInt(rsBinDetails, "IsMixedMaterialOK").ToString();
                locationData.Append("{\"isMMok\":\"" + isMixedMaterialok + "\"},"); //4
                
                string fixedMcode = DB.RSField(rsBinDetails, "MCode").ToString();
                locationData.Append("{\"fixedMcode\":\"" + fixedMcode + "\"},"); //5
                
                string isActive = DB.RSFieldTinyInt(rsBinDetails, "IsActive").ToString();
                locationData.Append("{\"isactive\":\"" + isActive + "\"},"); //6

                string IsQuarantine = DB.RSFieldTinyInt(rsBinDetails, "IsQuarantine").ToString();
                locationData.Append("{\"isQuarantine\":\"" + IsQuarantine + "\"},"); //7

                string Tenant = DB.RSField(rsBinDetails, "TenantName").ToString();
                locationData.Append("{\"Tenant\":\"" + Tenant + "\"},"); //8

                string Supplier = DB.RSField(rsBinDetails, "SupplierName").ToString();
                locationData.Append("{\"Supplier\":\"" + Supplier + "\"},"); //9

                string TenantID = DB.RSFieldInt(rsBinDetails, "TenantID").ToString();
                locationData.Append("{\"TenantID\":\"" + TenantID + "\"},"); //10

                string SupplierID = DB.RSField(rsBinDetails, "SupplierID").ToString();
                locationData.Append("{\"SupplierID\":\"" + SupplierID + "\"},"); //11

                string FixedMaterialMasterID = DB.RSField(rsBinDetails, "FixedMaterialMasterID").ToString();
                locationData.Append("{\"FixedMaterialMasterID\":\"" + FixedMaterialMasterID + "\"},"); //12

                string cbm = Convert.ToDecimal((Convert.ToDecimal(width) * Convert.ToDecimal(height) * Convert.ToDecimal(length)) / 1000000).ToString();
                locationData.Append("{\"cbm\":\"" + cbm + "\"},"); //13

                string IsFastMoving = DB.RSFieldBool(rsBinDetails, "IsFastMoving") == true ? "1" : "0";
                locationData.Append("{\"isFastMoving\":\"" + IsFastMoving + "\"},"); //14

                string LocType = DB.RSFieldInt(rsBinDetails, "LocationTypeID").ToString();
                locationData.Append("{\"LocType\":\"" + LocType + "\"},"); //15
                string Location = DB.RSField(rsBinDetails, "Location").ToString();
                locationData.Append("{\"Location\":\"" + Location + "\"}"); //15



            }
            rsBinDetails.Close();
            locationData.Append("]");
            json = locationData.ToString();
            return json;
        }
    }
}