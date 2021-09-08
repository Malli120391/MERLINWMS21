using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Data;
using MRLWMSC21Common;
using System.Text;

namespace MRLWMSC21.mMaterialManagement.LocationHandlers
{
    /// <summary>
    /// Summary description for GetZonewiseLocationsHandler
    /// </summary>
    public class GetZonewiseLocationsHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string zone = context.Request.QueryString[0];
            string sqlCommand;

            //sqlCommand = "EXEC [sp_INV_GetLocDataForZone] @Zone=" + zone;
            string TenantID = context.Request.QueryString["TenantID"];
            sqlCommand = "EXEC [sp_TPL_GetTenantLocDataForZone] @Zone=" + zone + ",@TenantID=" + TenantID;

            DataSet dataset = DB.GetDS(sqlCommand, false);
            DataTable datatable = dataset.Tables[0];
            StringBuilder locationData = new StringBuilder();
            string location = String.Empty;
            string Tenant = String.Empty;
            string TenantIDs = String.Empty;
            string bindata = String.Empty;
            string isactive=String.Empty;
            string Supplier = string.Empty;
            string MCode = string.Empty;

            foreach (DataRow datareader in datatable.Rows)
            {
                location = datareader["location"].ToString();
                Tenant = datareader["Tenant"].ToString();
                TenantIDs = datareader["TenantID"].ToString();
                isactive = datareader["IsActive"].ToString();
                MCode = datareader["MCode"].ToString();
                //Supplier = datareader["Supplier"].ToString();
                bindata = datareader["LocCont"].ToString();
                //Appending Location and Location information with '|'
                
                //locationData.Append(location + isactive + '!' + TenantIDs + '!' + Tenant + '|' + bindata + "Ñ");
                locationData.Append(location + isactive + '!' + TenantIDs + '!' + Tenant + '!' + MCode + '|' + bindata + "Ñ");

            }

            string result = locationData.ToString();
            if (result.Length > 0)
            {
                result = result.Remove(result.Length - 1);
                result += '@' + dataset.Tables[1].Rows[0][0].ToString();
            }
            context.Response.Write(result);
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