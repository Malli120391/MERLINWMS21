using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using MRLWMSC21Common;

namespace MRLWMSC21
{
    public partial class NestedGridView : System.Web.UI.Page
    {

        string gvUniqueID = String.Empty;
        int gvNewPageIndex = 0;
        int gvEditIndex = -1;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                ViewState["RoutingDetailsList"] = "EXEC [dbo].[sp_MFG_GetRoutingDetails] @RoutingHeaderID="+CommonLogic.QueryString("routid");
                this.gvParent_buildGridData(gvParent_buildGridData());
            }
        }




        #region  ----------- Parent GridView  -------------------



        protected void lnkAddNewLineItem_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["RoutingDetailsList"] = "EXEC [dbo].[sp_MFG_GetRoutingDetails] @RoutingHeaderID=" + CommonLogic.QueryString("routid");
                DataSet dsRoutingDetailsList = gvParent_buildGridData();
                DataRow newRow = dsRoutingDetailsList.Tables[0].NewRow();
                int sequenceNumber = DB.GetSqlN("select top 1 SequenceNumber as N from MFG_RoutingDetails where IsDeleted=0 and RoutingHeaderID=" + CommonLogic.QueryString("routid") + " order by SequenceNumber desc");
                newRow["RoutingDetailsID"] = 0;
                newRow["Name"] = "";
                newRow["IsSerialNoRequired"] = 0;
                newRow["SequenceNumber"] = ++sequenceNumber;
                dsRoutingDetailsList.Tables[0].Rows.InsertAt(newRow, 0);
                gvParent.EditIndex = 0;
                gvParent.PageIndex = 0;
                gvParent_buildGridData(dsRoutingDetailsList);

            }
            catch (Exception ex)
            {
               
            }
        }

        protected DataSet gvParent_buildGridData()
        {
            string sql = ViewState["RoutingDetailsList"].ToString();
            
            DataSet ds = DB.GetDS(sql, false);
           
            return ds;

        }

        protected void gvParent_buildGridData(DataSet ds)
        {
            gvParent.DataSource = ds;
            gvParent.DataBind();
            ds.Dispose();
        }

        protected void gvParent_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem == null)
                return;

            GridViewRow row = e.Row;
            //Find Child GridView control
            GridView gv = new GridView();
            gv = (GridView)row.FindControl("gvChild");

            if (gv.UniqueID == gvUniqueID)
            {
                gv.PageIndex = gvNewPageIndex;
                gv.EditIndex = gvEditIndex;
                //Check if Sorting used
               
                //Expand the Child grid
                ClientScript.RegisterStartupScript(GetType(), "Expand", "<SCRIPT LANGUAGE='javascript'>expandcollapse('div" + ((DataRowView)e.Row.DataItem)["RoutingDetailsID"].ToString() + "','one');</script>");
            }


             Literal ltRoutingDetailsID = (Literal)row.FindControl("ltRoutingDetailsID");

             ViewState["RoutScrapCodesListSQL"] = "EXEC [sp_MFG_GetRoutingScrapCodeDetails]  @RoutingDetailsID=" + ltRoutingDetailsID.Text;
            
            //Prepare the query for Child GridView by passing the Customer ID of the parent row
            gv.DataSource = this.gvChild_buildGridData();
            gv.DataBind();

        }

        protected void gvParent_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow row = ((Control)e.CommandSource).NamingContainer as GridViewRow;



        }

        protected void gvParent_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

        }

        protected void gvParent_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

        }

        protected void gvParent_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvParent.EditIndex = e.NewEditIndex;
                gvParent_buildGridData(gvParent_buildGridData());
            }
            catch (Exception ex)
            {
               
            }
        }

        protected void gvParent_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            try
            {
                gvParent.PageIndex = e.NewPageIndex;
                gvParent_buildGridData(gvParent_buildGridData());
            }
            catch (Exception ex)
            {
               
            }

        }

        protected void lnkIsDeleted_Click(object sender, EventArgs e)
        {

        }

        #endregion  ----------- Parent GridView  -------------------




        #region  ----------- Child GridView  -------------------



        protected DataSet gvChild_buildGridData()
        {
            string sql = ViewState["RoutScrapCodesListSQL"].ToString();

            DataSet ds = DB.GetDS(sql, false);

            return ds;

        }

        protected void gvChild_buildGridData(DataSet ds)
        {
            gvParent.DataSource = ds;
            gvParent.DataBind();
            ds.Dispose();
        }


        protected void gvChild_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView gvTemp = (GridView)sender;
            gvUniqueID = gvTemp.UniqueID;
            gvNewPageIndex = e.NewPageIndex;
            gvParent.DataBind();
        }

        protected void gvChild_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

        }

        protected void gvChild_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gvChild_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView gvTemp = (GridView)sender;
            gvUniqueID = gvTemp.UniqueID;
            gvEditIndex = e.NewEditIndex;
            this.gvParent_buildGridData(gvParent_buildGridData());
        }

        protected void gvChild_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView gvTemp = (GridView)sender;
            gvUniqueID = gvTemp.UniqueID;
            gvEditIndex = -1;
            gvParent.DataBind();
        }

        protected void gvChild_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void lnkRoutSCRDelete_Click(object sender, EventArgs e)
        {

        }

        #endregion  ----------- Child GridView  -------------------

        

       

       
    }
}