using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using MRLWMSC21Common;
using System.Globalization;

namespace FalconAdmin._3PLBilling
{
    public partial class Taxation : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["Taxation"] = "SELECT [TaxationID],[TaxCode],[Description],FORMAT([FromDate],'dd-MMM-yyyy') FromDate,FORMAT([ToDate],'dd-MMM-yyyy') ToDate,[Percentage],[StateCode]  FROM [TPL_Taxation] where isactive=1 and isdeleted=0 ORDER BY CreatedOn DESC";
                LoadActivityTaxation(LoadActivityTaxation());
            }
        } 


        private DataSet LoadActivityTaxation()
        {
            string sql = ViewState["Taxation"].ToString();
            DataSet ds = DB.GetDS(sql, false);
            return ds;
        }

        private void LoadActivityTaxation(DataSet ds)
        {
            gvActivityTaxaction.DataSource = ds;
            gvActivityTaxaction.DataBind();
            ds.Dispose();
        }

        protected void lnkAddNew_Click(object sender, EventArgs e)
        {
            gvActivityTaxaction.EditIndex = 0;
            gvActivityTaxaction.PageIndex = 0;
            DataSet ds = LoadActivityTaxation();
            DataRow row = ds.Tables[0].NewRow();
            row["TaxationID"] = 0;
            ds.Tables[0].Rows.InsertAt(row, 0);
            LoadActivityTaxation(ds);
        }

        protected void gvActivityTaxaction_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvActivityTaxaction.EditIndex = e.NewEditIndex;
            LoadActivityTaxation(LoadActivityTaxation());
        }

        protected void gvActivityTaxaction_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            //if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowIndex == gvActivityTaxaction.EditIndex))
            //{

            //    DataRow row = ((DataRowView)e.Row.DataItem).Row;
            //    ((HiddenField)e.Row.FindControl("hifTaxationID")).Value = row["TaxationID"].ToString();
            //    ((HiddenField)e.Row.FindControl("hifActivityTaxationID")).Value = row["ActivityTaxationID"].ToString();
            //    ((HiddenField)e.Row.FindControl("hifActivityRateTypeID")).Value = row["ActivityRateTypeID"].ToString();
            //}
        }

        protected void gvActivityTaxaction_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvActivityTaxaction.EditIndex = -1;
            LoadActivityTaxation(LoadActivityTaxation());
        }

        protected void gvActivityTaxaction_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            resetError("", false);
            Page.Validate("vgTaxaction");
            if (!Page.IsValid)
            {
                return;
            }
            cp = HttpContext.Current.User as CustomPrincipal;
            StringBuilder query = new StringBuilder();
            GridViewRow gvRow = gvActivityTaxaction.Rows[e.RowIndex];
            HiddenField hifTaxationID = (HiddenField)gvRow.FindControl("hifTaxationID");
            TextBox txtTaxCode = (TextBox)gvRow.FindControl("txtTaxCode");
            TextBox txtDescription = (TextBox)gvRow.FindControl("txtDescription");
            TextBox txtFromDate = (TextBox)gvRow.FindControl("txtFromDate");
            TextBox txtToDate = (TextBox)gvRow.FindControl("txtToDate");
            TextBox txtPercentage = (TextBox)gvRow.FindControl("txtPercentage");
            TextBox txtStateCode = (TextBox)gvRow.FindControl("txtStateCode");

            if(txtPercentage.Text.Trim()=="")
            {
                resetError("Please enter Percentage", true);
                return;
                txtPercentage.Text = "0";
            }
            query.Append("EXEC [sp_TPL_UpsertTaxation] ");
            query.Append(" @TaxationID=" + hifTaxationID.Value + ",");
            query.Append(" @TaxCode=" + DB.SQuote(txtTaxCode.Text) + ",");
            query.Append(" @Description=" + DB.SQuote(txtDescription.Text) + ",");
            query.Append(" @Percentage=" + DB.SQuote(txtPercentage.Text) + ",");
            query.Append(" @StateCode=" + DB.SQuote(txtStateCode.Text) + ",");
            //string STRDATE = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString();
            query.Append(" @FromDate=" + DB.DateQuote(DateTime.ParseExact(txtFromDate.Text.Trim().Replace("-", "/"), "dd/MMM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy")) + ",");
            query.Append(" @ToDate=" + DB.DateQuote(DateTime.ParseExact(txtToDate.Text.Trim().Replace("-", "/"), "dd/MMM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy")) + ",");
            query.Append(" @IsActive=1" + ",");
            query.Append(" @IsDeleted=0" + ",");
            query.Append(" @CreatedBy=" + cp.UserID);
            int taxcode = 0;
            if (hifTaxationID.Value == "0")
            {
                taxcode = DB.GetSqlN("select COUNT(*) N from TPL_Taxation where IsActive=1 and IsDeleted=0 and TaxCode=" + DB.SQuote(txtTaxCode.Text));
            }
            else
            {
                taxcode = DB.GetSqlN("select COUNT(*) N from TPL_Taxation where IsActive=1 and IsDeleted=0 and TaxCode=" + DB.SQuote(txtTaxCode.Text) + " and TaxationID not in (select TaxationID from TPL_Taxation where TaxationID =" + DB.SQuote(hifTaxationID.Value) + ")");
            }

            if (taxcode == 0)
            {
                try
                {
                    DB.ExecuteSQL(query.ToString());
                    gvActivityTaxaction.EditIndex = -1;
                    LoadActivityTaxation(LoadActivityTaxation());
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "test11", "Succes();", true);
                    //resetError("Successfully updated",false);
                }
                catch (Exception ex)
                {
                    resetError("Error while updating", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "test11", "Exists();", true);
                //resetError("Tax Code already exists",true);
            }
        }

        protected void gvActivityTaxaction_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvActivityTaxaction.PageIndex = e.NewPageIndex;
            LoadActivityTaxation(LoadActivityTaxation());
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            string DeletedRecords = "";
          
            foreach (GridViewRow row in gvActivityTaxaction.Rows)
            {
                try
                {
                    if (((CheckBox)row.FindControl("cbkTaxation")).Checked)
                    {
                        DeletedRecords += ((HiddenField)row.FindControl("hifTaxationID")).Value + ",";
                    }
                }
                catch(Exception ex)
                {

                }
                
            }
            if (DeletedRecords != "")
            {
                string Query = "UPdate [TPL_Taxation]  set IsDeleted=1 where [TaxationID] in (" + DeletedRecords.Substring(0, DeletedRecords.LastIndexOf(',')) + ") ;";
                try
                {
                    DB.ExecuteSQL(Query);
                    LoadActivityTaxation(LoadActivityTaxation());
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "test11", "SuccesDel();", true);
                    //resetError("Successfully Deleted", false);
                }
                catch (Exception ex)
                {
                    resetError("Error while deleting", true);
                }
            }
        }
        protected void resetError(string error, bool isError)
        {

            string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
            if (isError)
                str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

            if (error.Length > 0)
                str += error + "";
            else
                str = "";


            lblStatus.Text = str;


        }
    }
}