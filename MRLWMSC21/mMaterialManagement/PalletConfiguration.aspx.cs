using MRLWMSC21Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRLWMSC21.mMaterialManagement
{
    public partial class PalletConfiguration : System.Web.UI.Page
    {
      public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this, "Pallet Configuration");
                LoadPalletConfigurationData(LoadPalletConfigurationData());
            }
        }

        public DataSet LoadPalletConfigurationData()
        {
            string Query = "select PalletID,PalletCode,IsReusable,Length,Width,Height,WH.WarehouseID,WH.WHCode,zone.LocationZoneID,zone.LocationZoneCode from INV_Pallet Pal " +
                                "join GEN_Warehouse WH on Pal.WarehouseID=WH.WarehouseID and Pal.IsActive=1 "+
                                "join INV_LocationZone Zone on pal.ZoneID=Zone.LocationZoneID and Zone.IsActive=1 and Zone.IsDeleted=0 "+
                                "where Pal.IsActive=1 and WH.accountid =  case when 0 = "+ cp.AccountID.ToString() + " then AccountID else " + cp.AccountID.ToString() + " end order by PalletID desc";
            return DB.GetDS(Query, false);
        }

        public void LoadPalletConfigurationData(DataSet ds)
        {
            gvPalletAllocation.DataSource = ds;
            gvPalletAllocation.DataBind();
            ds.Dispose();
        }
        protected void lnkAddNewPallet_Click(object sender, EventArgs e)
        {
            DataSet ds = LoadPalletConfigurationData();
            DataRow dr=ds.Tables[0].NewRow();
            dr["PalletID"] = 0;
            dr["WarehouseID"] = 0;
            dr["LocationZoneID"] = 0;
            ds.Tables[0].Rows.InsertAt(dr, 0);
            gvPalletAllocation.EditIndex = 0;
            LoadPalletConfigurationData(ds);
        }

        protected void gvPalletAllocation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (gvPalletAllocation.EditIndex>=0 && e.Row.RowIndex == gvPalletAllocation.EditIndex)
            {
                
                //DropDownList ddlWHCode = (DropDownList)e.Row.FindControl("ddlWHCode");
                //HiddenField hifWarehouseID=(HiddenField)e.Row.FindControl("hifWarehouseID");
                //LoadDropDown(ddlWHCode, "select WHCode,WarehouseID from GEN_Warehouse where IsActive=1 and IsDeleted=0", "WHCode", "WarehouseID", "Select Warehouse");
                
                //ddlWHCode.SelectedValue = hifWarehouseID.Value;
                //DropDownList ddlLocationZoneCode = (DropDownList)e.Row.FindControl("ddlLocationZoneCode");
                //HiddenField hifLocationZoneID = (HiddenField)e.Row.FindControl("hifLocationZoneID");
                //LoadDropDown(ddlLocationZoneCode, "select LocationZoneID,LocationZoneCode from INV_LocationZone where IsActive=1 and IsDeleted=0", "LocationZoneCode", "LocationZoneID", "Select Zone");
                //ddlLocationZoneCode.SelectedValue = hifLocationZoneID.Value;
            }
        }

        public void LoadDropDown(DropDownList ddllist, String SqlQry, String strListName, String strListValue, string defaultValue)
        {
            // Initially clear all DropDownList Items
            ddllist.Items.Clear();
            ddllist.Items.Add(new ListItem(defaultValue, "0")); // Add Default value to the dropdown

            IDataReader rsList = DB.GetRS(SqlQry);

            while (rsList.Read())
            {

                ddllist.Items.Add(new ListItem(rsList[strListName].ToString(), rsList[strListValue].ToString()));

            }

            rsList.Close();
        }
        protected void gvPalletAllocation_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvPalletAllocation.EditIndex = e.NewEditIndex;
            LoadPalletConfigurationData(LoadPalletConfigurationData());
        }

        protected void gvPalletAllocation_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvPalletAllocation.EditIndex = -1;
            LoadPalletConfigurationData(LoadPalletConfigurationData());
        }

        protected void gvPalletAllocation_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Page.Validate("vgPallet");
            if (!IsValid)
            {
                return;
            }
            GridViewRow gr = gvPalletAllocation.Rows[e.RowIndex];
            string palletID = ((HiddenField)gr.FindControl("hifPalletID")).Value;
            string palletCode = ((TextBox)gr.FindControl("txtPalletCode")).Text;
            bool isReusable = ((CheckBox)gr.FindControl("chkIsReusable")).Checked;
            string length = ((TextBox)gr.FindControl("txtLength")).Text;
            string width = ((TextBox)gr.FindControl("txtWidth")).Text;
            string height = ((TextBox)gr.FindControl("txtHeight")).Text;
            //string warehouseID=((DropDownList)gr.FindControl("ddlWHCode")).SelectedValue;
            //string locationZoneID=((DropDownList)gr.FindControl("ddlLocationZoneCode")).SelectedValue;
            string warehouseID = ((HiddenField)gr.FindControl("hifWarehouseID")).Value;
            string locationZoneID = ((HiddenField)gr.FindControl("hifLocationZoneID")).Value;

            StringBuilder Query = new StringBuilder();
            Query.Append(" EXEC [sp_INB_Upsert_Pallet] ");
            Query.Append(" @PalletID=" + (palletID == "" ?"0":palletID));
            Query.Append(" ,@PalletCode=" + palletCode);
            Query.Append(" ,@IsReusable=" + (isReusable ? "1" : "0"));
            Query.Append(" ,@Length=" + length);
            Query.Append(" ,@Width=" + width);
            Query.Append(" ,@Height=" + height);
            Query.Append(" ,@WarehouseID="+warehouseID);
            Query.Append(" ,@LocationZoneID=" + locationZoneID);
            Query.Append(" ,@CreatedBy="+cp.UserID);
            gvPalletAllocation.EditIndex = -1;
            try
            {
                DB.ExecuteSQL(Query.ToString());
                LoadPalletConfigurationData(LoadPalletConfigurationData());
                resetError("Successfully updated", false);
            }
            catch (Exception ex)
            {
                resetError("Error while updating data", true);
            }
        }
        protected void resetError(string error, bool isError)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);
        }
        protected void gvPalletAllocation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPalletAllocation.PageIndex = e.NewPageIndex;
            LoadPalletConfigurationData(LoadPalletConfigurationData());
        }

       
    }
}