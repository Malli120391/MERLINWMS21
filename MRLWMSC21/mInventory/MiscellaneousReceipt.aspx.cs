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
using System.Threading;

namespace MRLWMSC21.mInventory
{
    public partial class MiscellaneousReceipt : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        

        protected void page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Inventory";
        }
        
        protected void page_Init(object sender, EventArgs e)
        {
            if (CommonLogic.QueryString("mid")!="")
                AddMspTextBoxs(CommonLogic.QueryString("mid"));
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("Stock Adjustment(in)"))))
            //{
            //    Response.Redirect("../Login.aspx?eid=6");
            //}

            string parameter = Request["__EVENTARGUMENT"];
            string Redirectquery = Request["__EVENTTARGET"];
            if (parameter == "materialDetails")
                lnkGetMaterialDetails_Click(sender, e);
            
            if (!IsPostBack)
            {
                GetStorageLocations();
                DesignLogic.SetInnerPageSubHeading(this.Page, "Miscellaneous Receipt");
                if (CommonLogic.QueryString("mid") != "")
                {
                    hifMaterialId.Value = CommonLogic.QueryString("mid");
                    Load_MaterialDetails();
                    lnksave.Visible = true;
                }else
                    lnksave.Visible = false;

            }
            Page.Validate("receiveQuantity");
        }

        public void GetStorageLocations()
        {
            DataSet ds = null;
            try
            {
                ds = DB.GetDS("Exec [dbo].[SP_INV_GET_STORAGELOCATIONForReturn] ", false);
                ddlStorageLocationID.AppendDataBoundItems = true;
                ddlStorageLocationID.Items.Add(new ListItem("-- Please Select --", "0"));
                ddlStorageLocationID.DataSource = ds.Tables[0];
                ddlStorageLocationID.DataTextField = "Code";
                ddlStorageLocationID.DataValueField = "Id";
                ddlStorageLocationID.DataBind();

            }
            catch (Exception ex)
            {

            }

        }

        protected void lnkGetMaterialDetails_Click(object sender, EventArgs e)
        {
        

            // Response.Redirect("MiscellaneousReceipt.aspx?mid=" + hifMaterialId.Value + "&mcode="+atcMateialCode.Text);
            ClearData();
           Load_MaterialDetails();
        }

        private void Load_MaterialDetails()
        {
            if (txtWH.Text == "" || hifWarehouseId.Value == "0")
            {
                resetError("Please Check For Mandatory Fields", true);
                return;
            }
            if (hifMaterialId.Value != "")
            {
                DataSet dsMaterialDetails = DB.GetDS("select UM.UoM+' / '+CONVERT(nvarchar,MGU.UoMQty) [UoM],MDescription from MMT_MaterialMaster MM join MMT_MaterialMaster_GEN_UoM MGU on MM.MaterialMasterID=MGU.MaterialMasterID and MGU.UoMTypeID=1 and MGU.IsActive=1 and MGU.IsDeleted=0 join GEN_UoM UM on MGU.UoMID=UM.UoMID and UM.IsActive=1 and UM.IsDeleted=0 where MM.MaterialMasterID=" + hifMaterialId.Value + "  and MM.IsActive=1 and MM.IsDeleted=0", false);
                DataTable dtMaterialDetails = dsMaterialDetails.Tables[0];
                if (dtMaterialDetails.Rows.Count>=1)
                {
                    trDetails.Style.Remove("display");
                    ltBaseUOM.Text = dtMaterialDetails.Rows[0][0].ToString();
                    ltDescription.Text = dtMaterialDetails.Rows[0][1].ToString();
                    
                }
                ViewState["materialID"] = hifMaterialId.Value;
                dsMaterialDetails.Dispose();
                //AddMspTextBoxs(hifMaterialId.Value);
                LoadGridData();
                tbdetail.Visible = true;
                lnksave.Visible = true;
           
        }
        }
        protected void resetError(string error, bool isError)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);

        }

        private void AddMspTextBoxs(String MMID)
        {
            try
            {
                IDataReader drMatrialMsp = DB.GetRS("[sp_ORD_GetMaterialStorageParameters] @MaterialMasterID=" + MMID + ",@TenantID=" + cp.TenantID);

                int mspCount = 0;

                HtmlTableRow htRow = null;
                HtmlTableCell htCell;
                TextBox txttextbox;
                Literal ltliteral;
                DropDownList ddlDropDownList;
                while (drMatrialMsp.Read())
                {


                    if (mspCount++ % 2 == 0)
                    {
                        htRow = new HtmlTableRow();
                        htCell = new HtmlTableCell();
                        htCell.Align = "left";
                        htCell.Width = "54.5%";
                        ltliteral = new Literal();
                        ltliteral.ID = "lt" + DB.RSField(drMatrialMsp, "ParameterName");
                        ltliteral.Text = DB.RSField(drMatrialMsp, "DisplayName") + ":<br />";

                        String ControlType = DB.RSField(drMatrialMsp, "ControlType");
                        HiddenField hifHiddenfield = new HiddenField();
                        hifHiddenfield.ID = "hif" + DB.RSField(drMatrialMsp, "ParameterName");
                        htCell.Controls.Add(hifHiddenfield);
                        RequiredFieldValidator validator = new RequiredFieldValidator();
                        validator.ValidationGroup = "receiveQuantity";
                        validator.ID = "rfv" + DB.RSField(drMatrialMsp, "ParameterName");
                        validator.Display = ValidatorDisplay.Dynamic;
                        validator.ErrorMessage = "*";
                        if (ControlType == "DropDownList")
                        {

                            ddlDropDownList = new DropDownList();
                            ddlDropDownList.ID = "ddl" + DB.RSField(drMatrialMsp, "ParameterName");
                            ddlDropDownList.Width = 160;
                            BuildDynamicDropDown(ddlDropDownList, DB.RSField(drMatrialMsp, "DataSource"), DB.RSField(drMatrialMsp, "ParameterName"));
                            validator.ControlToValidate = ddlDropDownList.ID;
                            if (DB.RSFieldTinyInt(drMatrialMsp, "IsRequired") == 1)
                                htCell.Controls.Add(validator);
                            htCell.Controls.Add(ltliteral);
                            htCell.Controls.Add(ddlDropDownList);
                        }
                        else if (ControlType == "TextBox")
                        {

                            txttextbox = new TextBox();
                            txttextbox.ID = "txt" + DB.RSField(drMatrialMsp, "ParameterName");
                            txttextbox.Width = 160;
                            txttextbox.EnableTheming = false;
                            txttextbox.CssClass = "txt_Blue_Small";
                            if (DB.RSField(drMatrialMsp, "ParameterDataType") == "Nvarchar")
                                txttextbox.Attributes.Add("onKeypress", "return checkSpecialChar(event)");
                            else if (DB.RSField(drMatrialMsp, "ParameterDataType") == "Integer")
                                txttextbox.Attributes.Add("onKeypress", "return checkNum(event)");
                            else if (DB.RSField(drMatrialMsp, "ParameterDataType") == "Decimal")
                                txttextbox.Attributes.Add("onKeypress", "return checkDec(event)");
                            else if (DB.RSField(drMatrialMsp, "ParameterDataType") == "DateTime")
                                txttextbox.CssClass = "DateBoxCSS_small";
                            validator.ControlToValidate = txttextbox.ID;
                            if (DB.RSFieldTinyInt(drMatrialMsp, "IsRequired") == 1)
                                htCell.Controls.Add(validator);
                            htCell.Controls.Add(ltliteral);
                            htCell.Controls.Add(txttextbox);
                        }

                        htRow.Cells.Add(htCell);
                        //tbMaterialStorageparameter.Rows.Add(htRow);
                    }
                    else
                    {
                        //htRow = new HtmlTableRow();
                        htCell = new HtmlTableCell();
                        htCell.Align = "left";
                        htCell.Width = "100%";
                        // htCell.Style.Add("padding-left", "140px");
                        txttextbox = new TextBox();
                        ltliteral = new Literal();
                        ltliteral.ID = "lt" + DB.RSField(drMatrialMsp, "ParameterName");
                        ltliteral.Text = DB.RSField(drMatrialMsp, "DisplayName") + ":<br />";
                        String ControlType = DB.RSField(drMatrialMsp, "ControlType");
                        HiddenField hifHiddenfield = new HiddenField();
                        hifHiddenfield.ID = "hif" + DB.RSField(drMatrialMsp, "ParameterName");
                        htCell.Controls.Add(hifHiddenfield);
                        RequiredFieldValidator validator = new RequiredFieldValidator();
                        validator.ValidationGroup = "receiveQuantity";
                        validator.ID = "rfv" + DB.RSField(drMatrialMsp, "ParameterName");
                        validator.Display = ValidatorDisplay.Dynamic;
                        validator.ErrorMessage = "*";
                        if (ControlType == "DropDownList")
                        {
                            ddlDropDownList = new DropDownList();
                            ddlDropDownList.ID = "ddl" + DB.RSField(drMatrialMsp, "ParameterName");
                            ddlDropDownList.Width = 160;
                            BuildDynamicDropDown(ddlDropDownList, DB.RSField(drMatrialMsp, "DataSource"), DB.RSField(drMatrialMsp, "ParameterName"));
                            validator.ControlToValidate = ddlDropDownList.ID;
                            if (DB.RSFieldTinyInt(drMatrialMsp, "IsRequired") == 1)
                                htCell.Controls.Add(validator);
                            htCell.Controls.Add(ltliteral);

                            htCell.Controls.Add(ddlDropDownList);
                        }
                        else if (ControlType == "TextBox")
                        {
                            txttextbox = new TextBox();
                            txttextbox.ID = "txt" + DB.RSField(drMatrialMsp, "ParameterName");
                            txttextbox.Width = 160;
                            txttextbox.EnableTheming = false;
                            txttextbox.CssClass = "txt_Blue_Small";
                            if (DB.RSField(drMatrialMsp, "ParameterDataType") == "Nvarchar")
                                txttextbox.Attributes.Add("onKeypress", "return checkSpecialChar(event)");
                            else if (DB.RSField(drMatrialMsp, "ParameterDataType") == "Integer")
                                txttextbox.Attributes.Add("onKeypress", "return checkNum(event)");
                            else if (DB.RSField(drMatrialMsp, "ParameterDataType") == "Decimal")
                                txttextbox.Attributes.Add("onKeypress", "return checkDec(event)");
                            else if (DB.RSField(drMatrialMsp, "ParameterDataType") == "DateTime")
                                txttextbox.CssClass = "DateBoxCSS_small";
                            validator.ControlToValidate = txttextbox.ID;
                            if (DB.RSFieldTinyInt(drMatrialMsp, "IsRequired") == 1)
                                htCell.Controls.Add(validator);
                            htCell.Controls.Add(ltliteral);
                            htCell.Controls.Add(txttextbox);
                        }

                        htRow.Cells.Add(htCell);
                        tbMaterialStorageparameter.Rows.Add(htRow);
                        //htRow = new HtmlTableRow();
                        // htCell = new HtmlTableCell();
                        // htCell.Controls.Add(new LiteralControl("&nbsp;"));
                        // htRow.Cells.Add(htCell);
                        // tbMaterialStorageparameter.Rows.Add(htRow);
                    }


                }
                if (mspCount % 2 != 0)
                {
                    tbMaterialStorageparameter.Rows.Add(htRow);
                }
                drMatrialMsp.Close();
                //tbMaterialStorageparameter.Rows.Add(new HtmlTableRow());
                atcMateialCode.Text = CommonLogic.QueryString("mcode");

            }
            catch (Exception ex)
            {
                resetError("Error while build Material Storage Parameters", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }

        //Add dynamic MSP dropdown
        private void BuildDynamicDropDown(DropDownList dropdown, String sql, String HeaderName)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            dropdown.Items.Clear();

            dropdown.Items.Add(new ListItem());
            try
            {
                dropdown.Items.Clear();

                IDataReader dropdownReader = DB.GetRS(sql);

                dropdown.Items.Add(new ListItem("Select " + HeaderName, ""));
                while (dropdownReader.Read())
                {
                    dropdown.Items.Add(new ListItem(dropdownReader[0].ToString(), dropdownReader[1].ToString()));
                }
                dropdownReader.Close();
            }
            catch (Exception ex)
            {
                resetError("Error while loading", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }

        protected void lnksave_Click(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            Page.Validate("");
            if (!IsValid)
            {
                resetError("Please Check For Mandatory Fields", true);
                return;
            }
      
            try
            {
                String MissMatchedMSPS = "";
                String DocQuantity = txtQuantity.Text.Trim();
                
               

                string SerialNo = txtserialNo.Text;
                if (SerialNo != "")
                {


                    if (Convert.ToDecimal(DocQuantity) != 1)
                    {
                        resetError("Quantity should be 1 for serial no., cannot receive", true);
                        return;
                    }
                    string Query = "[dbo].[sp_GET_SERIAL_COUNT] @SerialNo = " + DB.SQuote(SerialNo) + ", @MCode= " + DB.SQuote(atcMateialCode.Text.Trim()) + "";
                    int Count = DB.GetSqlN(Query);
                    if (Count != 0)
                    {
                        resetError("Duplicate serial no", true);
                        return;
                    }

                }

                

                StringBuilder query = new StringBuilder();
                query.Append("EXEC  [dbo].[sp_INV_MisslleniousReceiptUpdate] @MaterialMasterID=" + hifMaterialId.Value );
                query.Append(" ,@POQunatity=" + txtQuantity.Text);
                query.Append(" ,@LOCATION=" + DB.SQuote(txtLocation.Text));
                query.Append(" ,@IsDamaged=" + (chkDamgae.Checked?"1":"0"));
                query.Append(" ,@HasDiscipency=" + (chkDiscrepancy.Checked?"1":"0"));
                query.Append(" ,@Remarks="+DB.SQuote(txtRemarks.Text));
                query.Append(" ,@StorageLocationID =" + ddlStorageLocationID.SelectedValue.ToString());
                query.Append(" ,@BatchNo = " + DB.SQuote(txtbatchno.Text));
                query.Append(" ,@SerialNo = " + DB.SQuote(txtserialNo.Text));
                query.Append(" ,@MfgDate = " + DB.SQuote(txtMfgDate.Text).ToString());
                query.Append(" ,@ExpDate = " + DB.SQuote(txtexpdate.Text));
                query.Append(" ,@ProjectRefNo = " + DB.SQuote(txtprojectref.Text));
                query.Append(" ,@MRP=" + DB.SQuote(txtMRP.Text));
                query.Append(" ,@UpdatedBy =" +cp.UserID.ToString());
                query.Append(" ,@CartonID =" + hifContainercode.Value);
                query.Append(",@WareHouseId=" + hifWarehouseId.Value);
                DB.ExecuteSQL(query.ToString());
                LoadGridData();
                Thread.Sleep(500);
                lnkGetMaterialDetails_Click(sender, e);
                ClearData();
                resetError("Successfully received", false);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "test11", "HideLoader();", true);

            }
            catch (Exception ex)
            {
                resetError(ex.Message, true);
            }
        }

        private void  LoadGridData()
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                DataSet ds = DB.GetDS("[dbo].[sp_INV_GetAvailbleDataForMisslleniousReceipt] @MaterialMasterID=" + hifMaterialId.Value, false);
                List<ActiveStockDetail> activeMaterials = new List<ActiveStockDetail>();
                List<string> HeaderList = new List<string>();

                DataTable dtmaterialList = ds.Tables[0];
                int columnCount = dtmaterialList.Columns.Count;

                int mspsStartingRange = 7;

                for (int columnIndex = mspsStartingRange; columnIndex < columnCount; columnIndex++)
                {
                        HeaderList.Add(dtmaterialList.Columns[columnIndex].ColumnName);
                }
                foreach (DataRow dr in dtmaterialList.Rows)
                { 
                    ActiveStockDetail activeMaterial=new ActiveStockDetail();

                    activeMaterial.location = dr["Location"].ToString();//Location
                    activeMaterial.GoodsMovementDetailsID = (int)dr["GoodsMovementDetailsID"];
                    activeMaterial.qty = Convert.ToDecimal(dr["AVAILABLE"].ToString());//Location
                    activeMaterial.isDamgae = dr["Is Dam."].ToString().Equals("1");
                    activeMaterial.hasDiscapency = dr["Has Desc."].ToString().Equals("1");

                    // storage Location
                    activeMaterial.StorageLocationID = dr["StorageLocationID"].ToString();
                    activeMaterial.Code = dr["Code"].ToString();
                    activeMaterial.BatchNo = dr["BatchNo"].ToString();
                    activeMaterial.ExpDate = dr["ExpDate"].ToString();
                    activeMaterial.MfgDate = dr["MfgDate"].ToString();
                    activeMaterial.SerialNo = dr["SerialNo"].ToString();
                    activeMaterial.ContainerCode = dr["CartonCode"].ToString();
                    activeMaterial.ProjectREf = dr["ProjectRefNo"].ToString();
                    activeMaterial.MRP = dr["MRP"].ToString();

                    //activeMaterial.msps = new List<string>();
                    //for (int columnIndex = mspsStartingRange; columnIndex < columnCount; columnIndex++)
                    //{
                    //    activeMaterial.msps.Add(dr[columnIndex].ToString());
                    //}
                    activeMaterials.Add(activeMaterial);
                }
                
                gvReceivedQty.DataSource = activeMaterials;
                gvReceivedQty.DataBind();
              //  LoadGridHeader(HeaderList);

            }
            catch (Exception ex)
            { 

            }
        }

        protected void gvReceivedQty_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvReceivedQty.PageIndex = e.NewPageIndex;
            LoadGridData();
        }

        protected void gvReceivedQty_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                ActiveStockDetail activeStockDetail = ((ActiveStockDetail)e.Row.DataItem);
                //((Literal)e.Row.FindControl("ltDynamicMsps")).Text = DynamicRows(activeStockDetail.msps);
            }
        }
        private String DynamicRows(List<string> msps)
        {
            StringBuilder query = new StringBuilder();
            foreach (string msp in msps)
            {
                query.Append("<td>"+msp+"</td>");
            }
            return query.ToString();
        }
        private void LoadGridHeader(List<string> mspsHeader)
        {
            StringBuilder header = new StringBuilder();
            StringBuilder footer = new StringBuilder();
            foreach (string mspheader in mspsHeader)
            {
                header.Append("<th>"+mspheader+"</th>");
                footer.Append("<th>" +"</th>");
            }
            gvReceivedQty.HeaderRow.Cells[3].Text += header.ToString();
            gvReceivedQty.FooterRow.Cells[3].Text = footer.ToString();
            
        }

        private void ClearData()
        {
            txtLocation.Text = "";
            txtQuantity.Text = "";
            txtContainerCode.Text = "";
            txtRemarks.Text = "";
            txtbatchno.Text = "";
            txtMfgDate.Text = "";
            txtexpdate.Text = "";
            txtserialNo.Text = "";
            txtprojectref.Text = "";
            ddlStorageLocationID.SelectedIndex = 0;

        }

    }
    class ActiveStockDetail
    {
        public int GoodsMovementDetailsID { get; set; }
        public string location{get;set;}
        public decimal qty { get; set; }
        public bool isDamgae { get; set; }
        public bool hasDiscapency { get; set; }
        public string StorageLocationID { get; set; }
        public string Code { get; set; }
        
        public string BatchNo { get; set; }
        public string SerialNo { get; set; }
        public string MfgDate { get; set; }
        public string ExpDate { get; set; }
        public string ProjectREf { get; set; }
        public string MRP { get; set; }
        public string ContainerCode { get; set; }
        public List<string> msps { get; set; }
    }
    
}