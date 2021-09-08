using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21Common;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.Text;
namespace MRLWMSC21.mInventory
{

    public partial class CaptureCycleCountDetails : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        string ParamNames = string.Empty;
        string MSpIDs = string.Empty;
        string IsRequiredIDs = string.Empty;
        string ParamValues = string.Empty;


        protected void page_PreInit(object sender, EventArgs e)
        {

            Page.Theme = "Inventory";



        }
        protected void page_Init(object sender, EventArgs e)
        {

            if (CommonLogic.QueryString("mmid") != "")
            {
                String MMID = CommonLogic.QueryString("mmid");
                AddMspTextBoxs(MMID);
            }

        }
        private void AddMspTextBoxs(string MMID)
        {

            if (Session["mcode"] != null)
            {
                txtmmID.Text = Session["mcode"].ToString();
                txtccID.Text = Session["ccid"].ToString();
                txtlocID.Text = Session["location"].ToString();
                hifMCode.Value = Session["mmid"].ToString();
                hifLocID.Value = Session["locID"].ToString();
            }
            IDataReader rsConfigureMSP = DB.GetRS("[sp_ORD_GetMaterialStorageParameters] @MaterialMasterID=" + MMID + ",@TenantID=" + cp.TenantID);
            int count = 0;
            int x = 10;
            pnlMsps.Visible = true;
            HtmlTableRow tr;
            HtmlTableCell td;
            while (rsConfigureMSP.Read())
            {
                string paramName = DB.RSField(rsConfigureMSP, "ParameterName");
                string displayName = DB.RSField(rsConfigureMSP, "DisplayName");
                string ControlType = DB.RSField(rsConfigureMSP, "ControlType");
                string Datasource = DB.RSField(rsConfigureMSP, "DataSource");
                string ParameterDataType = DB.RSField(rsConfigureMSP, "ParameterDataType");
                string IsRequired = DB.RSFieldTinyInt(rsConfigureMSP, "IsRequired").ToString();
                string MspID = DB.RSFieldInt(rsConfigureMSP, "MaterialStorageParameterID").ToString();

                ParamNames += paramName + ",";
                MSpIDs += MspID + ",";
                IsRequiredIDs += IsRequired + ",";

                Label lblParam = new Label();
                lblParam.Text = displayName;

                tr = new HtmlTableRow();
                td = new HtmlTableCell();

                td.Controls.Add(lblParam);
                tr.Controls.Add(td);


                td = new HtmlTableCell();
                if (ControlType == "TextBox")
                {
                    if (ParameterDataType == "Nvarchar")
                    {
                        TextBox txt = new TextBox();

                        td.Controls.Add(txt);
                        tr.Controls.Add(td);
                        if (IsRequired == "1")
                        {
                            txt.ID = "Reqtext" + paramName;
                            txt.Attributes.Add("onblur", "testIsRequired(this);");
                        }
                        else
                        {
                            txt.ID = "text" + paramName;
                        }
                    }
                    else if (ParameterDataType == "DateTime")
                    {
                        TextBox txt = new TextBox();

                        txt.CssClass = "DateBoxCSS_small";

                        if (IsRequired == "1")
                        {
                            txt.ID = "ReqDate" + paramName;
                        }
                        else
                        {
                            txt.ID = "date" + paramName;
                        }

                        txt.EnableTheming = false;

                        td.Controls.Add(txt);
                        tr.Controls.Add(td);
                    }

                }
                else if (ControlType == "DropDownList")
                {
                    DropDownList ddlDropDownList = new DropDownList();
                    ddlDropDownList.Width = 200;
                    BuildDynamicDropDown(ddlDropDownList, Datasource, paramName);
                    td.Controls.Add(ddlDropDownList);
                    tr.Controls.Add(td);
                    if (IsRequired == "1")
                    {
                        ddlDropDownList.ID = "Req" + paramName;
                    }
                    else
                    {
                        ddlDropDownList.ID = "Not" + paramName;
                    }

                }
                x += 20;
                tblMSPs.Controls.Add(tr);
            }
        }
        private void BuildDynamicDropDown(DropDownList dropdown, String sql, String HeaderName)
        {
            dropdown.Items.Clear();

            dropdown.Items.Add(new ListItem());
            try
            {
                dropdown.Items.Clear();

                IDataReader dropdownReader = DB.GetRS(sql);

                dropdown.Items.Add(new ListItem("Select " + HeaderName, "0"));
                while (dropdownReader.Read())
                {
                    dropdown.Items.Add(new ListItem(dropdownReader[0].ToString(), dropdownReader[1].ToString()));
                }
                dropdownReader.Close();
            }
            catch (Exception)
            {
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Capture Material details");
                if (Session["mmid"] != null)
                {
                    string MMID = Session["mmid"].ToString();
                    string Query = "select CONVERT(NVARCHAR,UOM.UoM)+'/'+CONVERT(NVARCHAR,MMTUOM.UoMQty)+'  '+UT.UoMType AS UOM,MMTUOM.MaterialMaster_UoMID from MMT_MaterialMaster_GEN_UoM MMTUOM " +
                                    "JOIN GEN_UoM UOM ON UOM.UoMID=MMTUOM.UoMID AND UOM.IsActive=1 AND UOM.IsDeleted=0 " +
                                    "JOIN GEN_UoMType UT ON UT.UoMTypeID=MMTUOM.UoMTypeID AND UT.IsActive=1 AND UT.IsDeleted=0 " +
                                    "where MaterialMasterID=" + MMID;
                    BuildDynamicDropDown(ddlUOM, Query, "UOM");

                }
            }

        }

        protected void txtmmID_TextChanged(object sender, EventArgs e)
        {

            string data = "ssss";
            resetError(hifMCode.Value.ToString(), true);
        }
        private void resetError(string error, bool isError)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);

        }

        protected void lnkin_GetDetails_Click(object sender, EventArgs e)
        {




            string mmid = hifMCode.Value;
            string ccid = txtccID.Text;
            string location = txtlocID.Text;
            if (mmid == "0" || ccid == "" || hifLocID.Value == "0")
            {
                resetError("Provide Mandatory Fields", true);
                return;
            }

            Session["mmid"] = mmid;
            Session["mcode"] = txtmmID.Text;
            Session["location"] = txtlocID.Text;
            Session["ccid"] = txtccID.Text;
            Session["locID"] = hifLocID.Value;

            Response.Redirect("CaptureCycleCountDetails.aspx?mmid=" + mmid);

        }
        protected void txtDynamic_TextChanged(object sender, EventArgs e)
        {

        }

        protected void lnkin_cancel_Click(object sender, EventArgs e)
        {

        }
        protected void TextBox_Validate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = false;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            if (ddlUOM.SelectedValue == "0")
            {
                resetError("Select UoM", true);
                ddlUOM.Focus();
                return;
            }
            if (txtCountQty.Text == "")
            {
                resetError("Provide Quantity", true);
                txtCountQty.Focus();
                return;
            }
            int vCountUOMID = int.Parse(ddlUOM.SelectedValue);
         
            string dataSource = "select " + txtCountQty.Text + "*dbo.GetConversionFactor("+vCountUOMID+") AS N";
            decimal ConvertTOBUOM = DB.GetSqlNDecimal(dataSource);

            int count = tblMSPs.Controls.Count;
            for (int index = 0; index < count; index++)
            {
                System.Web.UI.HtmlControls.HtmlTableRow row = (System.Web.UI.HtmlControls.HtmlTableRow)tblMSPs.Controls[index];
                int rowcount = row.Controls.Count;
                for (int i = 0; i < rowcount; i++)
                {

                    System.Web.UI.HtmlControls.HtmlTableCell cell = (System.Web.UI.HtmlControls.HtmlTableCell)row.Controls[i];
                    string control = cell.Controls[0].GetType().ToString();

                    if (control != "System.Web.UI.WebControls.Label")
                    {
                        if (control == "System.Web.UI.WebControls.TextBox")
                        {
                            string value = "";
                            TextBox txt = (TextBox)cell.Controls[0];
                            string data = txt.Text;
                            if (txt.ID.Substring(0, 7) == "Reqtext")
                            {

                                if (data.Trim() == "")
                                {
                                    txt.Focus();
                                    resetError("Provide Mandatory Fields", true);
                                    return;
                                }
                                else
                                {
                                    value = txt.Text;
                                }
                            }
                            else if (txt.ID.Substring(0, 7) == "ReqDate")
                            {
                                string date = txt.Text;
                                if (date.Trim() != "")
                                {
                                    try
                                    {
                                        string formatDateTime = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                                        value = date;
                                    }
                                    catch
                                    {
                                        resetError("Invalid Date", true);
                                        txt.Focus();
                                        return;
                                    }
                                }
                                else
                                {
                                    resetError("Provide Mandatory Fields", true);
                                    txt.Focus();
                                    return;
                                }

                            }
                            else if (txt.ID.Substring(0, 4) == "text")
                            {
                                value = txt.Text;
                            }
                            else if (txt.ID.Substring(0, 4) == "date")
                            {
                                if (txt.Text != "")
                                {
                                    try
                                    {
                                        string formatDateTime = DateTime.ParseExact(txt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                                        value = txt.Text;
                                    }
                                    catch
                                    {
                                        resetError("Invalid Date", true);
                                        txt.Focus();
                                        return;
                                    }
                                }
                                else
                                {
                                    value = txt.Text;
                                }
                            }
                            ParamValues += value + "Œ";

                        }
                        else if (control == "System.Web.UI.WebControls.DropDownList")
                        {
                            DropDownList ddl = (DropDownList)cell.Controls[0];
                            string id1 = ddl.SelectedValue;
                            if (ddl.ID.Substring(0, 3) == "Req")
                            {
                                if (ddl.SelectedValue == "0")
                                {
                                    resetError("Provide Mandatory Fields", true);
                                    ddl.Focus();
                                    return;

                                }
                            }
                            ParamValues += ddl.SelectedValue + "Œ";
                        }
                    }
                }




            }
            string finamMspValues = "";
            string finalMspIds = "";
            if (ParamValues.Length != 0)
            {
                ParamValues = ParamValues.Remove(ParamValues.Length - 1);
                MSpIDs = MSpIDs.Remove(MSpIDs.Length - 1);
                string[] finalmspvaluesSplit = ParamValues.Split('Œ');
                string[] finalmspIDSplit = MSpIDs.Split(',');
                ParamValues = "";
                MSpIDs = "";
                for (int index = 0; index < finalmspvaluesSplit.Length; index++)
                {
                    if (finalmspvaluesSplit[index] != "" || finalmspvaluesSplit[index] != "0")
                    {
                        finamMspValues += finalmspvaluesSplit[index] + "Œ";
                        finalMspIds += finalmspIDSplit[index] + ",";
                    }
                }

            }
            else
            {
                finamMspValues = "";
                finalMspIds = "";
            }
          
            //resetError(ParamValues + "@" + MSpIDs, true);
            if (finamMspValues.Length != 0)
            {
                MSpIDs = finalMspIds.Remove(finalMspIds.Length - 1);
                ParamValues = finamMspValues.Remove(finamMspValues.Length - 1);
            }
          
            string queryForGetPhysicalQuantity = "declare @ddd decimal(18,2); "+
                                " EXEC [sp_INV_GetStockInListQty]  @ST_MaterialMasterID="+hifMCode.Value+", @ST_LocationID="+hifLocID.Value+", "+
                                "@ST_MaterialStorageParameterIDs=" + DB.SQuote(MSpIDs) + ",@ST_MaterialStorageParameterValues=" + DB.SQuote(ParamValues) + ", " +
                                "@ST_IsDamaged=0 ";
            decimal vPhysicalQuantity = DB.GetSqlNDecimal(queryForGetPhysicalQuantity);

            int vCyclecountdetailsid = DB.GetSqlN(" select CycleCountDetailsID as N from QCC_CycleCountDetails where CycleCountID=" + txtccID.Text + " and MaterialMasterID=" + hifMCode.Value);
            if (vCyclecountdetailsid == 0)
            {
                resetError("Invalid Details", true);
                return;
            }

            StringBuilder sbSqlString = new StringBuilder();
            sbSqlString.Append("declare @NewResult int ");
            sbSqlString.Append(" EXEC  [dbo].[sp_CC_InsertCycleCountCapture] ");
            sbSqlString.Append(" @CycleCountDetailsID=" + vCyclecountdetailsid);
            sbSqlString.Append(" ,@KitPlannerID=NULL");
            sbSqlString.Append(" ,@LocationID=" + hifLocID.Value);
            sbSqlString.Append(" ,@CapturedQty=" + vPhysicalQuantity);
            sbSqlString.Append(" ,@CountUoMID=" + vCountUOMID);
            sbSqlString.Append(" ,@ConversionFactor=NULL");
            sbSqlString.Append(" ,@CountQty=" + txtCountQty.Text);
            sbSqlString.Append(" ,@Quantity=" + ConvertTOBUOM);
            sbSqlString.Append(" ,@CountedBy=" + cp.UserID);
            sbSqlString.Append(" ,@HandlerIP=" + DB.SQuote("1.0.2.54"));
            sbSqlString.Append(" ,@Remarks=" + DB.SQuote("haiiii"));
            sbSqlString.Append(" ,@CreatedBy=" + cp.UserID);
            sbSqlString.Append(" ,@MaterialStorageParameterIDs=" +DB.SQuote(MSpIDs));
            sbSqlString.Append(" ,@Values=" +DB.SQuote(ParamValues));
            sbSqlString.Append(" ,@IsDamaged=" +  Convert.ToInt16(chkIsdam.Checked));
            sbSqlString.Append(",@Result=@NewResult OUTPUT ");
            sbSqlString.Append("SELECT @NewResult AS N");

            try
            {
                DB.GetSqlN(sbSqlString.ToString());
                resetError("Successfully Captured", false);
                txtmmID.Text = "";
                txtlocID.Text = "";
                txtCountQty.Text = "";
                hifLocID.Value = "0";
                hifMCode.Value = "0";
                txtccID.Text = "";
                pnlMsps.Visible = false;
            }
            catch
            {
                resetError("Error while updating,please contack support team", true);
            }
            string vLocationID = hifLocID.Value;


        }

        protected void txtCountQty_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double.Parse(txtCountQty.Text);
            }
            catch (FormatException ex)
            {
                resetError("Invalid Quantity", true);
                txtCountQty.Focus();
                return;
            }
        }
    }
}