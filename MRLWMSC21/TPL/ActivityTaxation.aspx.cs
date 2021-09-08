using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using MRLWMSC21Common;

namespace FalconAdmin._3PLBilling
{
    public partial class ActivityTaxation : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                ViewState["Taxation"] = "select TAT.ActivityTaxationID,TAT.TaxationID,TAT.HSNCode,TAT.ActivityRateTypeID,TT.TaxCode,TAR.ActivityRateType from TPL_ActivityTaxation TAT left join TPL_Taxation TT on TT.TaxationID=TAT.TaxationID and TT.IsActive=1 and TT.IsDeleted=0 left join TPL_Activity_RateType TAR on TAR.ActivityRateTypeID=TAT.ActivityRateTypeID and TAR.IsActive=1 and TAR.IsDeleted=0  where TAT.IsActive=1 and TAT.IsDeleted=0 ORDER BY TAT.CreatedOn DESC ";
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
            row["ActivityTaxationID"] = 0;
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
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowIndex == gvActivityTaxaction.EditIndex))
            {

                DataRow row = ((DataRowView)e.Row.DataItem).Row;
                ((HiddenField)e.Row.FindControl("hifTaxationID")).Value = row["TaxationID"].ToString();
                ((HiddenField)e.Row.FindControl("hifActivityTaxationID")).Value = row["ActivityTaxationID"].ToString();
                ((HiddenField)e.Row.FindControl("hifActivityRateTypeID")).Value = row["ActivityRateTypeID"].ToString();
            }
        }

        protected void gvActivityTaxaction_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvActivityTaxaction.EditIndex = -1;
            LoadActivityTaxation(LoadActivityTaxation());
        }

        protected void gvActivityTaxaction_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //resetError("", false);
            //Page.Validate("vgTaxaction");
            //if (!Page.IsValid)
            //{
            //    return;
            //}
            cp = HttpContext.Current.User as CustomPrincipal;
            GridViewRow gvRow = gvActivityTaxaction.Rows[e.RowIndex];
            string hifTaxcode = ((HiddenField)gvRow.FindControl("hifTaxationID")).Value;
            string ActivityRateTypeID = ((HiddenField)gvRow.FindControl("hifActivityRateTypeID")).Value;
            if(hifTaxcode=="" || hifTaxcode == null)
            {
                resetError("Please Select TaxCode", true);
                return;
            }
            if(ActivityRateTypeID=="" || ActivityRateTypeID == null)
            {
                resetError("Please Select Traiff Sub-Group",true);
                return;
            }

            StringBuilder query = new StringBuilder();
           
            HiddenField hifActivityTaxationID = (HiddenField)gvRow.FindControl("hifActivityTaxationID");
            HiddenField hifTaxationID = (HiddenField)gvRow.FindControl("hifTaxationID");
            HiddenField hifActivityRateTypeID = (HiddenField)gvRow.FindControl("hifActivityRateTypeID");
            TextBox txtHSNCode= (TextBox)gvRow.FindControl("txtHSNCode");
            query.Append("EXEC [sp_TPL_Upsert_ActivityTaxation] ");
            query.Append(" @ActivityTaxationID=" + hifActivityTaxationID.Value + ",");
            query.Append(" @HSNCode=" + DB.SQuote(txtHSNCode.Text )+",");
            query.Append(" @TaxationID=" + hifTaxationID.Value + ",");
            query.Append(" @ActivityRateTypeID=" + hifActivityRateTypeID.Value + ",");
            query.Append(" @CreatedBy=" + cp.UserID);

            try {
                DB.ExecuteSQL(query.ToString());
                gvActivityTaxaction.EditIndex = -1;
                LoadActivityTaxation(LoadActivityTaxation());
                ScriptManager.RegisterStartupScript(this, this.GetType(), "test11", "Succes();", true);
                //resetError("Successfully updated",false);
            }
            catch (Exception ex)
            {
                resetError("Error while updating",true);
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
                        DeletedRecords += ((HiddenField)row.FindControl("hifItemTenantActivityRateID")).Value + ",";
                    }
                }
                catch (Exception ex)
                {

                }
                
            }
            if (DeletedRecords != "")
            {
                string Query = "UPdate TPL_ActivityTaxation  set IsDeleted=1 where ActivityTaxationID in (" + DeletedRecords.Substring(0, DeletedRecords.LastIndexOf(',')) + ") ;";
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showStickyToast", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);
        }

        //protected void resetError(string error, bool isError)
        //{

        //    string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
        //    if (isError)
        //        str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

        //    if (error.Length > 0)
        //        str += error + "";
        //    else
        //        str = "";


        //    lblStatus.Text = str;


        //}
    }
}