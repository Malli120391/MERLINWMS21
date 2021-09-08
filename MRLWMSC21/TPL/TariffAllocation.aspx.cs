using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRLWMSC21Common;
using System.Text;
using System.Globalization;

namespace FalconAdmin._3PLBilling
{
    public partial class TariffAllocation : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        protected void Page_Load(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            if (!IsPostBack)
            {
                ViewState["Tenant"] = "EXEC [sp_TPL_GetTariffAllocation] @TenantId=0,@AccountId=" + cp.AccountID;
                //int accid = cp.AccountID;
                LoadTariffData(LoadTariffData());
            }
        }

        private DataSet LoadTariffData()
        {
            string sql = ViewState["Tenant"].ToString();
            DataSet ds = DB.GetDS(sql, false);
            return ds;
        }

        private void LoadTariffData(DataSet ds)
        {
            gvTariffAllocation.DataSource = ds;
            gvTariffAllocation.DataBind();
            ds.Dispose();
        }

        protected void lnkAdd_Click(object sender, EventArgs e)
        {
            gvTariffAllocation.EditIndex = 0;
            gvTariffAllocation.PageIndex = 0;
            DataSet ds = LoadTariffData();
            DataRow row = ds.Tables[0].NewRow();
            row["TenantActivityRateID"] = 0;
            row["TenantDiscountRateID"] = 0;
            row["OnRate"] = 0;
            
            //row["TenantDiscountRateGroupID"] = 0;
            ds.Tables[0].Rows.InsertAt(row,0);
            LoadTariffData(ds);
        }

        protected void gvTariffAllocation_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvTariffAllocation.EditIndex = e.NewEditIndex;
            LoadTariffData(LoadTariffData());
        }

        protected void gvTariffAllocation_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvTariffAllocation.EditIndex = -1;
            LoadTariffData(LoadTariffData());
        }

        protected void gvTariffAllocation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowIndex == gvTariffAllocation.EditIndex))
            {
               
                DataRow row = ((DataRowView)e.Row.DataItem).Row;
                ((HiddenField)e.Row.FindControl("hifTenantActivityRateID")).Value = row["TenantActivityRateID"].ToString();
                ((HiddenField)e.Row.FindControl("hifTenantID")).Value = row["TenantID"].ToString();
                ((HiddenField)e.Row.FindControl("hifActivityRateGroupID")).Value = row["ActivityRateGroupID"].ToString();
                ((HiddenField)e.Row.FindControl("hifActivityRateTypeID")).Value = row["ActivityRateTypeID"].ToString();
                ((HiddenField)e.Row.FindControl("hifActivityRateID")).Value = row["ActivityRateID"].ToString();
                ((HiddenField)e.Row.FindControl("hifTenantDiscountRateID")).Value = row["TenantDiscountRateID"].ToString();
                //((HiddenField)e.Row.FindControl("hifTenantDiscountRateGroupID")).Value = row["TenantDiscountRateGroupID"].ToString();

                ((HiddenField)e.Row.FindControl("hifEffectiveTo")).Value = (row["EffectiveTo"].ToString()==""?"": row["EffectiveTo"]).ToString();
                
                ((CheckBox)e.Row.FindControl("cbxIsOntimeRate")).Checked = (row["IsOnetimeRate"].ToString() == "1" ? true : false);
                TextBox txtDiscount = (TextBox)e.Row.FindControl("txtDiscount");
                
                //RadioButton rbOnRate = (RadioButton)e.Row.FindControl("rbOnRate");
                //RadioButton rbOnRateGroup = (RadioButton)e.Row.FindControl("rbOnRateGroup");
                ////string BillTypeID = row["BillTypeID"].ToString();

                double OnRate = Convert.ToDouble(CheckIsEmpty(row["OnRate"].ToString()));
                //double OnRateGroup = Convert.ToDouble(CheckIsEmpty(row["OnRateGroup"].ToString()));
                //if (OnRate > 0)
                //{
                //    rbOnRate.Checked = true;
                    txtDiscount.Text = OnRate.ToString();
                //    hifDiscountChangePrevious.Value = "1";
                //}
                //else if (OnRateGroup > 0)
                //{
                //    rbOnRateGroup.Checked = true;
                //    txtDiscount.Text = OnRateGroup.ToString();
                //    hifDiscountChangePrevious.Value = "2";
                //}
                //else
                //{
                //    rbOnRate.Checked = false;
                //    rbOnRateGroup.Checked = false;
                //    hifDiscountChangePrevious.Value = "0";
                //    txtDiscount.Text = "0";

                //}
                ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "setBilltype('" + row["BillTypeID"].ToString() + "');", true);
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                foreach (GridViewRow row in gvTariffAllocation.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                       // row.Cells[5].HorizontalAlign = HorizontalAlign.Left;
                        string s = row.Cells[5].Text;
                        row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                    }
                }
               
            }
            }

        protected void gvTariffAllocation_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //resetError("", false);
            //Page.Validate("vgTariff");
            //if (!Page.IsValid)
            //{
            //    return;
            //}
            cp = HttpContext.Current.User as CustomPrincipal;
            GridViewRow gvRow = gvTariffAllocation.Rows[e.RowIndex];
            string hifTenantActivityRateID = ((HiddenField)gvRow.FindControl("hifTenantActivityRateID")).Value;
            string hifTenantID =((HiddenField)gvRow.FindControl("hifTenantID")).Value;
            string hifActivityRateGroupID = ((HiddenField)gvRow.FindControl("hifActivityRateGroupID")).Value;
            string hifActivityRateTypeID = ((HiddenField)gvRow.FindControl("hifActivityRateTypeID")).Value;
            string hifActivityRateID = ((HiddenField)gvRow.FindControl("hifActivityRateID")).Value;

            string hifTenantDiscountRateID = ((HiddenField)gvRow.FindControl("hifTenantDiscountRateID")).Value;
            HiddenField hifTenantDiscountRateGroupID = (HiddenField)gvRow.FindControl("hifTenantDiscountRateGroupID");

            string txtDiscount = ((TextBox)gvRow.FindControl("txtDiscount")).Text;
            RadioButton rbOnRate = (RadioButton)gvRow.FindControl("rbOnRate");
            RadioButton rbOnRateGroup = (RadioButton)gvRow.FindControl("rbOnRateGroup");
            TextBox txtEffectiveFrom = ((TextBox)gvRow.FindControl("txtEffectiveFrom"));
            //Label lbEffectiveTo = (Label)gvRow.FindControl("lbEffectiveTo");
            string hifEffectiveTo = ((HiddenField)gvRow.FindControl("hifEffectiveTo")).Value;
            TextBox txteffictiveTo = ((TextBox)gvRow.FindControl("txtEffectiveTo"));
            CheckBox cbxIsOntimeRate = (CheckBox)gvRow.FindControl("cbxIsOntimeRate");
            decimal discount = Convert.ToDecimal(CheckIsEmpty(txtDiscount));

            if (hifTenantID.ToString()== "" || hifTenantID.ToString() == null)
            {
                resetError("Please Select Tenant", true);
                return;
            }
            if ( hifActivityRateGroupID == "" || hifActivityRateGroupID == null)
            {
                resetError("Please Select Tariff Group", true);
                return;
            }
            if ( hifActivityRateTypeID == "" || hifActivityRateTypeID == null)
            {
                resetError("Please Select Tariff Sub-Group", true);
                return;
            }
            if ( hifActivityRateID == "" || hifActivityRateID == null)
            {
                resetError("Please select Tariff", true);
                return;
            }
            if (txtEffectiveFrom.Text == "" || txtEffectiveFrom.Text == null)
            {
                resetError("please enter FromDate", true);
                return;
            }
            if (txteffictiveTo.Text == ""|| txteffictiveTo.Text == null)
            {
                resetError("Please enter ToDate", true);
                return;
            }

            //hifDiscountChangePrevious.Value = "0";
            //rbOnRate.Checked = true;
            //if (discount > 0)
            //{
            //    if (rbOnRate.Checked)
            //    {
            //        hifDiscountChangeAfter.Value = "1";

            //    }
            //    else if (rbOnRateGroup.Checked)
            //    {
            //        hifDiscountChangeAfter.Value = "2";
            //    }
            //}            
            //string GroupChange = "0";
            //if (hifDiscountChangePrevious.Value != "0")
            //{
            //    if (hifDiscountChangeAfter.Value != hifDiscountChangePrevious.Value)
            //    {
            //        GroupChange = "1";
            //    }
            //}

            //string rateid = "0";
            //if (discount > 0)
            //{
            //    if (rbOnRate.Checked)
            //    {
            //        if (GroupChange == "0")
            //        {
            //            if (hifTenantDiscountRateID.Value == "") // This is for First time Create Discount
            //            {
            //                GroupChange = "1";
            //                rateid = "0";
            //            }
            //            else
            //                rateid = hifTenantDiscountRateID.Value;
            //        }
            //        else
            //            rateid = hifTenantDiscountRateGroupID.Value;

            //    }
            //    else if (rbOnRateGroup.Checked) 
            //    {

            //        if (GroupChange == "0")
            //        {
            //            if (hifTenantDiscountRateGroupID.Value == "") // This is for First time Create Discount
            //            {
            //                GroupChange = "1";
            //                rateid = "1";
            //            }
            //            else
            //                rateid = hifTenantDiscountRateGroupID.Value;
            //        }
            //        else
            //            rateid = hifTenantDiscountRateID.Value;
            //    }
            //}

            try
            {
                StringBuilder query = new StringBuilder();
                query.Append("EXEC [sp_TPL_UpsertTariffAllocation] @TenantActivityRateID=" + hifTenantActivityRateID);
                query.Append(" ,@TenantID=" + hifTenantID);
                query.Append(" ,@ActivityRateID=" + hifActivityRateID);
                query.Append(" ,@ActivityRateGroupID= " + hifActivityRateGroupID);
                //query.Append(" ,@EffectiveFrom =" + DB.SQuote(Convert.ToDateTime(txtEffectiveFrom).ToString("MM/dd/yyyy")));
                //query.Append(" ,@EffectiveFrom =" + DB.DateQuote(Convert.ToDateTime(txtEffectiveFrom.Text)));
                query.Append(" ,@EffectiveFrom =" + DB.DateQuote(DateTime.ParseExact(txtEffectiveFrom.Text.Trim().Replace("-", "/"), "dd/MMM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy")));
                //DateTime.ParseExact(txtEffectiveFrom,"dd/MM/yyyy",CultureInfo.InvariantCulture).ToString("MM/dd/yyyy")));
                //query.Append(" ,@EffectiveTo =" + DB.SQuote(Convert.ToDateTime(txteffictiveTo).ToString("MM/dd/yyyy")));
                query.Append(" ,@EffectiveTo =" + DB.DateQuote(DateTime.ParseExact(txteffictiveTo.Text.Trim().Replace("-", "/"), "dd/MMM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy")));
                // query.Append(" ,@EffectiveTo =" + DB.DateQuote(Convert.ToDateTime(txteffictiveTo.Text)));
                //DB.SQuote(DateTime.ParseExact(txteffictiveTo, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy")));
                query.Append(" ,@CreatedBy =" + cp.UserID);
                query.Append(" ,@IsOnetimeRate =" + (cbxIsOntimeRate.Checked ? "1" : "0"));
                query.Append(" ,@discount =" + discount);
                query.Append(" ,@TenantDiscountRateID =" + (hifTenantDiscountRateID == "" ? "0" : hifTenantDiscountRateID));


                gvTariffAllocation.EditIndex = -1;
                gvTariffAllocation.PageIndex = 0;
                DB.ExecuteSQL(query.ToString());
                ScriptManager.RegisterStartupScript(this, this.GetType(), "test11", "Succes();", true);
                //resetError("Successfully updated", false);
            }
            catch (Exception ex)
            {
                resetError("Error while updating", false);
            }

            LoadTariffData(LoadTariffData());
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

        protected void resetError(string error, bool isError)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showStickyToast", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);
        }

        protected void gvTariffAllocation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTariffAllocation.PageIndex = e.NewPageIndex;
            LoadTariffData(LoadTariffData());
        }

        

        //public string setDiscount(string OnRate,string OnRateGroup)
        //{
        //    OnRate = CheckIsEmpty(OnRate);
        //    OnRateGroup = CheckIsEmpty(OnRateGroup);
            
        //    double onRate = Convert.ToDouble(OnRate);
        //    double onRateGroup = Convert.ToDouble(OnRateGroup);
        //    if (onRate > 0)
        //    {
        //        return onRate.ToString();
        //    }
        //    else if (onRateGroup > 0)
        //    {
        //        return onRateGroup.ToString();
        //    }
        //    else
        //    return "0";
        //}

        public string setDiscountOn(string OnRate, string OnRateGroup)
        {
            OnRate = CheckIsEmpty(OnRate);
            OnRateGroup = CheckIsEmpty(OnRateGroup);

            double onRate = Convert.ToDouble(OnRate);
            double onRateGroup = Convert.ToDouble(OnRateGroup);
            if (onRate > 0)
            {
                return "On Rate";
            }
            else if (onRateGroup > 0)
            {
                return "On Rate Group";
            }
            else
                return "";
        }

        private string CheckIsEmpty(string value)
        {
            //if (value == "" || value == null)
            //    value = "0";
            return "0"+value;
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            string DeletedRecords="";
            foreach (GridViewRow row in gvTariffAllocation.Rows)
            { 
            try
            {
                    if (((CheckBox)row.FindControl("cbkhifTenantActivityRateID")).Checked)
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
                string Query = "UPdate TPL_Tenant_Activity_Rate  set IsDeleted=1 where TenantActivityRateID in (" + DeletedRecords.Substring(0,DeletedRecords.LastIndexOf(',') )+ ") ;";
                Query += "UPdate TPL_TenantDiscount_OnRate  set IsDeleted=1 where TenantActivityRateID in (" + DeletedRecords.Substring(0, DeletedRecords.LastIndexOf(',')) + ") ;";
                Query += "UPdate TPL_TenantDiscount_OnRateGroup  set IsDeleted=1 where TenantActivityRateID in (" + DeletedRecords.Substring(0, DeletedRecords.LastIndexOf(',')) + ")";
                try
                {
                    DB.ExecuteSQL(Query);
                    LoadTariffData(LoadTariffData());
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "test11", "SuccesDel();", true);
                    //resetError("Successfully Deleted", false);
                }
                catch(Exception ex)
                {
                    resetError("Error while deleting", true);
                }
            }
        }

       
        protected void lnkSearchTenant_Click(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;

            string text = txtTenant.Text.Trim();
            
            int tentid = 0;
           if(text=="")
            {
                tentid = 0;
            }
           else
            {
                tentid = Convert.ToInt32(hidTenant.Value);
            }

            if (text == "Tenant...")
                text = "";

            ViewState["Tenant"] = "EXEC [sp_TPL_GetTariffAllocation] @TenantId=" + tentid + ",@AccountId=" + cp.AccountID;

            this.LoadTariffData(this.LoadTariffData());
        }

        protected void gvTariffAllocation_PreRender(object sender, EventArgs e)
        {
           
                foreach (GridViewRow row in gvTariffAllocation.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        row.Cells[5].HorizontalAlign = HorizontalAlign.Left;
                    }
                }
         
        }
        //private void getDays(int BillTypeID) 
        //{
        //    int Days;
        //    switch (BillTypeID)
        //    {
        //        case 1: Days = 15;
        //                break;
        //        case 2: Days = 15;
        //                break;
        //        case 3: Days = 15;
        //                break;
        //        case 4: Days = 15;
        //                break;
        //    }
        //}
    }
}
