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
    public partial class FunctionalConfiguration : System.Web.UI.Page
    {
        CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "MasterData";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Functional Strategy");
                LoadFunctionalStrategyGridData(LoadFunctionalStrategyGridData());
                ViewState["value"] = 0;
                ViewState["Error"] = "";
            }
        }

        private DataSet LoadFunctionalStrategyGridData(string SearchData="")
        {
            string Query = "SELECT cf.ConfigurableFunctionID,cf.FuncExecCode,CFS.FunctionalStrategyID,CFS.[StrategyCode],CFS.[Description],CFS.[DataType],CFS.[Value] "+
                             "FROM [GEN_Configurable_Functional_Strategy] CFS "+
                             "join [GEN_Configurable_Functionality] CF on CFS.ConfigurableFunctionID=cf.ConfigurableFunctionID and CF.IsActive=1 and CF.IsDeleted=0 "+
                             "where CFS.[IsActive]=1 and CFS.[IsDeleted] =0 ";
            if (SearchData!="")
                Query += " and cf.FuncExecCode like '" + SearchData + "%'";
            return DB.GetDS(Query,false);
        }

        public string displayValue(String value, String DataType)
        {
            if (value == "1" && DataType == "BOOLEAN")
                return "Yes";
            else if (value == "0" && DataType == "BOOLEAN")
                return "No";
            else
                return value;
        }

        private void LoadFunctionalStrategyGridData(DataSet Ds)
        {
            gvFunctionalStrategy.DataSource = Ds;
            gvFunctionalStrategy.DataBind();
        }

        protected void gvFunctionalStrategy_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvFunctionalStrategy.EditIndex = e.NewEditIndex;
            LoadFunctionalStrategyGridData(LoadFunctionalStrategyGridData(txtFunctionalExecutiveCode.Text));
            string dataType=((Literal)gvFunctionalStrategy.Rows[e.NewEditIndex].FindControl("ltDataType")).Text;
            GridViewRow gr = gvFunctionalStrategy.Rows[e.NewEditIndex];
            TextBox txtValue = (TextBox)gr.FindControl("txtValue");
            CheckBox chkValue = (CheckBox)gr.FindControl("chkValue");
            if (dataType.ToUpper().Equals("BOOLEAN"))
            {
                hifDataType.Value = "0";
                chkValue.Visible = true;
                txtValue.Visible = false;
                ViewState["value"] = 2;
                ViewState["Error"] = "The value is either 0 or 1 ";
                if (txtValue.Text == "1")
                    chkValue.Checked = true;
            }
            else if (dataType.ToUpper().Equals("DECIMAL"))
            {
                hifDataType.Value = "1";
                chkValue.Visible = false;
                txtValue.Visible = true;
                ViewState["value"] = 100;
                ViewState["Error"] = "The value lessthen 100 percent";
            }

            else if (dataType.ToUpper().Equals("INTEGER"))
            {
                hifDataType.Value = "0";
                chkValue.Visible = false;
                txtValue.Visible = true;
                //ViewState["value"] = 100;
                //ViewState["Error"] = "The value lessthen 100 percent";
            }
        }

        protected void gvFunctionalStrategy_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvFunctionalStrategy.EditIndex = -1;
            LoadFunctionalStrategyGridData(LoadFunctionalStrategyGridData(txtFunctionalExecutiveCode.Text));
        }

        protected void gvFunctionalStrategy_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Page.Validate("vgFunctionalStrategy");
            if(!Page.IsValid)
            { return; }
            try
            {
                GridViewRow gr = gvFunctionalStrategy.Rows[e.RowIndex];
                string hifFunctionalStrategyID = ((HiddenField)gr.FindControl("hifFunctionalStrategyID")).Value;
                string value ="";
                TextBox txtValue = (TextBox)gr.FindControl("txtValue");
                CheckBox chkValue = (CheckBox)gr.FindControl("chkValue");
                if (txtValue.Visible)
                    value = txtValue.Text;
                else
                {
                    if (chkValue.Checked)
                        value = "1";
                    else
                        value = "0";
                }
                String Query = "update [GEN_Configurable_Functional_Strategy] set Value=" + DB.SQuote(value) + " where FunctionalStrategyID="+hifFunctionalStrategyID;
                DB.ExecuteSQL(Query);
                gvFunctionalStrategy.EditIndex = -1;
                LoadFunctionalStrategyGridData(LoadFunctionalStrategyGridData(txtFunctionalExecutiveCode.Text.Trim()));
                resetError("Successfully updated",false);
            }
            catch (Exception ex)
            {
                resetError("Error while updating data", true);
            }
        }

        protected void gvFunctionalStrategy_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvFunctionalStrategy.PageIndex = e.NewPageIndex;
            LoadFunctionalStrategyGridData(LoadFunctionalStrategyGridData());
        }

        protected void resetError(string error, bool isError)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);
        }

        protected void lnkSearch_Click(object sender, EventArgs e)
        {
            //if (txtFunctionalExecutiveCode.Text != "")
            LoadFunctionalStrategyGridData(LoadFunctionalStrategyGridData(txtFunctionalExecutiveCode.Text));
        }
    }
}