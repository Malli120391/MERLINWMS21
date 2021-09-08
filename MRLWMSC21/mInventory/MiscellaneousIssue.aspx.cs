using MRLWMSC21Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace MRLWMSC21.mInventory
{
    public partial class MiscellaneousIssue : System.Web.UI.Page
    {
        public static CustomPrincipal cp;
        static string warehouseid;

        protected void page_PreInit(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            Page.Theme = "Inventory";
        }
        protected void page_Init(object sender, EventArgs e)
        {
            warehouseid = cp.Warehouses[0].ToString();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DesignLogic.SetInnerPageSubHeading(this.Page, "Miscellaneous Issue");
        }

        [WebMethod]
        public static string getMiscellaneousList(int materialid, int WareHouseID)
        {
            DataSet ds = new DataSet();
            string query = "select UM.UoM + ' / ' + CONVERT(nvarchar, MGU.UoMQty)[UoM],MDescription from MMT_MaterialMaster MM join MMT_MaterialMaster_GEN_UoM MGU on MM.MaterialMasterID = MGU.MaterialMasterID and MGU.UoMTypeID = 1 and MGU.IsActive = 1 and MGU.IsDeleted = 0 join GEN_UoM UM on MGU.UoMID = UM.UoMID and UM.IsActive = 1 and UM.IsDeleted = 0 where MM.MaterialMasterID = " + materialid + "  and MM.IsActive = 1 and MM.IsDeleted = 0";
            query += "  EXEC [dbo].[sp_INV_GetAvailbleDataForMisslleniousIssues] @MaterialMasterID=" + materialid + ",@WarehouseID="+WareHouseID;
            ds = DB.GetDS(query, false);

            return Newtonsoft.Json.JsonConvert.SerializeObject(ds);
        }

        [WebMethod]
        public static string PickingItem(search obj)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                StringBuilder query = new StringBuilder();
                query.Append(" DECLARE @NewResult int ");
                query.Append(" Exec [sp_INV_MisslleniousIssueUpdate] @MaterialMasterID=" + obj.MMID);
                query.Append(" ,@SOQunatity=" + obj.pickQuantity);
                query.Append(" ,@LOCATION =" + DB.SQuote(obj.Location));
                query.Append(" ,@ActiveStockDetailsID=" + obj.GMDID);
                query.Append(" ,@Remarks=" + DB.SQuote(obj.Remarks));
                query.Append(" ,@StorageLocationID =" + obj.SLOCID);
                query.Append(",@WarehouseID=" + Convert.ToInt32(obj.WarehouseID));
                query.Append(",@AccountID=" + cp.AccountID);
                query.Append(" ,@UpdatedBy =" + cp.UserID.ToString());
                query.Append(" ,@Result=@NewResult output select @NewResult as N");
                int result = DB.GetSqlN(query.ToString());
                if (result == 0)
                {
                    return "Successfully Picked";

                }
                else if (result == -1)
                {
                    return "Quantity is not available in store";
                }
                else if (result == -2)
                {
                    return "No customer code available";
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [WebMethod]
        public static string getStorageLocation()
        {
            try
            {
                DataSet ds = new DataSet();
                ds = DB.GetDS("Exec SP_INV_GET_STORAGELOCATION ", false);
                return Newtonsoft.Json.JsonConvert.SerializeObject(ds);

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public class search
        {
            public string TenatID { get; set; }
            public string MMID { get; set; }
            public string pickQuantity { get; set; }
            public string Location { get; set; }
            public string LocationID { get; set; }
            public string SLOC { get; set; }
            public string SLOCID { get; set; }
            public string Remarks { get; set; }
            public string GMDID { get; set; }
            public string WarehouseID { get; set; }
        }
    }
}