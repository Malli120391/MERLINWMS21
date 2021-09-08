using MRLWMSC21Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRLWMSC21.mMaterialManagement
{
    public partial class KitOrdersList : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Orders";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            String sql = "[SP_GetJobOrderHeaderList]";
            ViewState["joblistsql"] = sql;
            setGridData();
        }
        private void setGridData()
        {
            try
            {
                String sql = ViewState["joblistsql"].ToString();
                DataSet dsPOList = DB.GetDS(sql, false);
                KitList.DataSource = dsPOList;
                KitList.DataBind();
                dsPOList.Dispose();
                if (KitList.Rows.Count == 0)
                {
                    resetBlkLableError("No Jobs is available", true);
                }
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }
        protected void resetBlkLableError(string error, bool isError)
        {

          
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);

        }
        protected void JobList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            KitList.PageIndex = e.NewPageIndex;

            //String sql = "[dbo].[sp_ORD_POHeaderList] @PONumber='" + txtPONumber.Text.Trim() + "',@POStatusID=" + ddlSelectStatus.SelectedValue + ",@TenantID=" + hifTenant.Value;
            //Session["listqueue"] = sql;
            setGridData();


        }
        protected void JobListList_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataSet dsGetPOList = null;
            try
            {
                dsGetPOList = KitList.DataSource as DataSet;
            }
            catch (Exception ex)
            {
                resetBlkLableError("Error while loading", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }

            KitList.DataSource = dsGetPOList;
            KitList.DataBind();
            dsGetPOList.Dispose();
        }
    }
}