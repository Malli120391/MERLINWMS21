/////////////////////////////////////////////////////
// Author     : Gopi Vurukuti 
// Created On : 12 Dec 2017  
// Description: To Create Storage Locations.

//---------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21Common;
using Newtonsoft.Json;
using System.Web.Script.Services;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace MRLWMSC21.mInventory
{
    public partial class StorageLocations : System.Web.UI.Page
    {

        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Inventory";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Storage Locations");
            }
        }
        [WebMethod]
        public static string  GetStorageList()
        {
            string Json = "";
            try
            {
                DataSet ds = DB.GetDS("Exec SP_INV_GET_STORAGELOCATION ", false);
                 Json = JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                Json = "Failed";
            }
            return Json;
        }
        [WebMethod]
        public static string SetStorageLocations(int StrId,string Inxml)
        {
            string Status;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" Exec SP_INV_SET_STORAGELOCATION ");
                sb.Append(" @inputDataXml='" + Inxml +"'");
                sb.Append(" ,@StorageLocationId=" + StrId);
                DB.ExecuteSQL(sb.ToString());
                Status = "success";
            }
           catch (Exception ex)
            {
                Status = "Failed";
            }
            return Status;
        }
        [WebMethod]
        public static void DeleteStorageLocation(string StrId)
        {
            string Status;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" Exec SP_INV_Delete_STORAGELOCATION ");
                sb.Append("@StorageLocationId=" + StrId);
                DB.ExecuteSQL(sb.ToString());
                Status = "success";
            }
            catch (Exception ex)
            {
                Status = "Failed";
            }
        }
    }
}