using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using MRLWMSC21Common;
using System.Globalization;
using System.IO;
using System.Threading;



namespace MRLWMSC21.mInventory
{
    //Author Name:  Gvd Prasad
    //Created On:   15-Dec-2013    
    //Use case ID:  Active Stock_UC_019

    public partial class ActiveStock : System.Web.UI.Page
    {
        bool isexcel = false;
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        int _TotalRowCount,pageIndex=1;

        protected void page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Inventory";
        }

        protected void page_Init(object sender, EventArgs e)
        {
                AddSearchMSps();
               // AddGridHeaderList();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("Active Stock"))))
            {
                //test case ID (TC_007)
                //Only authorized user can access active stock page

                Response.Redirect("../Login.aspx?eid=6");
            }

            if (!IsPostBack)
            {
                // txtTenant.Enabled = CommonLogic.CheckSuperAdmin(txtTenant, cp, hifTenant);

                //Get Tenant Root Directory
                ViewState["TenantRootDir"] = DB.GetSqlS("select DefaultValue AS S from SYS_SystemConfiguration SYS_C left join SYS_SysConfigKey SYS_K on SYS_K.SysConfigKeyID=SYS_C.SysConfigKeyID where SysConfigKey='TenantContentPath'");

                // Get MaterialMaster Path
                ViewState["MMTPath"] = DB.GetSqlS(" select DefaultValue AS S from SYS_SystemConfiguration SYS_C left join SYS_SysConfigKey SYS_K on SYS_K.SysConfigKeyID=SYS_C.SysConfigKeyID where SysConfigKey='MaterialManagementPath'");

                //Get UniqueID
                ViewState["UniqueID"] = DB.GetSqlS("select UniqueID AS S from TPL_Tenant where TenantID=" + cp.TenantID);

                DesignLogic.SetInnerPageSubHeading(this.Page, "Active Stock");

                LoadDropDown(ddlMaterialType, "Select MTypeID, MType + ' - ' + Description as MTypeDesc from MMT_MType Where IsActive =1", "MTypeDesc", "MTypeID", "All");
                LoadDropDown(ddlWarehouse, "select WarehouseID,WHCode from GEN_Warehouse where IsDeleted=0 and IsActive=1", "WHCode", "WarehouseID", "All");
                BuildSiteDropDowns(ddlSite, "SELECT LocationZoneCode FROM INV_LocationZone where IsDeleted=0 and IsActive=1", "LocationZoneCode");
                BuildNumberDropDowns(ddlAisle);
                BuildCharDropDowns(ddlBay);
                BuildNumberDropDowns(ddlBeam);
                CommonLogic.LoadPrinters(ddlNetworkPrinter);
                GetStorageLocations();
                if (cp.TenantID != 0)
                {
                    gvActiveStock.Columns[1].Visible = false;
                }

                //ddlReplenishments.Items.Add(new ListItem("All Items","0"));
                //ddlReplenishments.Items.Add(new ListItem("Replenishments","1"));
                ViewState["SearchDetails"] = " DECLARE @Count Int exec [dbo].[sp_INV_GetActiveStockList_New]  @MaterialMasterID=0,@NonConformity=0 ,@Damaged=0 ,@Discepancy=0 ,@Location =null ,@IsToolType=0 ,@Tenant=" + hifTenant.Value + " ,@GRNNumber='' ,@Replenishment=0 ,@MaterialStorageParameters= '' ,@WarehouseId=0,@MaterialStorageParameterValues ='' ";
                bindGrid(pageIndex);
            }
            //set Detault to GetData
            Page.Form.DefaultButton = lnkGetData.UniqueID;//

        }

        public void GetStorageLocations()
        {
            DataSet ds = null;
            try
            {
                ds = DB.GetDS("Exec SP_INV_GET_STORAGELOCATION ", false);
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

        private void AddGridHeaderList()
        {
            //set Header Msp Column Details Based on DataBase 

            IDataReader dsDynamicMSp = null;
            try
            {
                dsDynamicMSp = DB.GetRS("sp_GEN_GetAllMaterialStorageParameters @ParameterUsageTypeID=1,@TenantID=" + cp.TenantID);
            }
            catch (Exception ex)
            {
                resetError("Error while loading", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                return;
            }
            int mspCount = 0;
            StringBuilder HeaderText = new StringBuilder();



            while (dsDynamicMSp.Read())
            {
                mspCount++;
                HeaderText.Append("<td align=\"center\" width=\"70px\"> " + DB.RSField(dsDynamicMSp, "DisplayName") + "</td>");
            }
            dsDynamicMSp.Close();

            String HearTable = "<table border=\"1\" align=\"center\"width=\"" + 100 + "%\"  style=\"border-collapse:collapse;border: 1px solid black;\"><tr><td align=\"center\" width=\"120px\">Location</td><td align=\"center\" width=\"160px\" title=\"Container Code\">Container Code</td><td align=\"center\" width=\"40px\">Kit ID</td><td align=\"center\" width=\"50px\">In-OH</td><td align=\"center\" width=\"50px\">Ob-OH</td><td align=\"center\" width=\"70px\">Aval.</td><td align=\"center\" width=\"35px\">Dam.</td><td align=\"center\" width=\"35px\">Disc.</td><td align=\"center\" width=\"35px\">QC Non Conf.</td>";

            //<td align=\"center\" width=\"35px\">As Is</td><td align=\"center\" width=\"35px\">Pve Recall</td>

            if (!isexcel)
            {
                HeaderText.Append("<td align=\"center\" width=\"20px\">Print<br/><input type=\"checkbox\" id=\"FacDetailsCheckAll\" onclick=\"javascript:check_FacDetailsuncheck(this)\" /></td>");
            }
            HeaderText.Append("</tr></table>");
            gvActiveStock.Columns[8].HeaderText = HearTable + HeaderText.ToString();
        }

        private IDataReader getMSPsDataReaderBasedonMaterial(string MMID)
        {
            // Get MSP List Based on MaterilMaster

            String sCmdRequiredMSP = "select ParameterName,ControlType,DataSource from MMT_MaterialStorageParameter msp left join MMT_MaterialMaster_MMT_MaterialStorageParameter mm_msp on msp.MaterialStorageParameterID=mm_msp.MaterialStorageParameterID and IsRequired=1 where mm_msp.MaterialMasterID=" + MMID;
            IDataReader rsRequireMsp = DB.GetRS(sCmdRequiredMSP);
            return rsRequireMsp;
        }

        private IDataReader getMSPsDataReader()
        {
            //Get MSP List Based On Tenant Only

            String sCmdRequiredMSP = "select ParameterName,ControlType,DataSource from MMT_MaterialStorageParameter where TenantID=" + cp.TenantID;
            IDataReader rsRequireMsp = DB.GetRS(sCmdRequiredMSP);
            return rsRequireMsp;
        }

        private void AddSearchMSps()
        {
            //Arrange Search MSPs In Web Page 
            // This control is for Dynamic Control adding

            HtmlTable tbMSPTable = tbMaterialStorageParameter;
            HtmlTableRow MSPRow = null;
            HtmlTableCell MSPCell = null;
            TextBox txtTextBox;
            DropDownList ddlDropDown;
            int tdCount = 0;

            IDataReader rsGetMaterialStorageParameters = null;
            try
            {
                rsGetMaterialStorageParameters = DB.GetRS("sp_GEN_GetAllMaterialStorageParameters @ParameterUsageTypeID=1,@TenantID=" + cp.TenantID);
            }
            catch (Exception ex)
            {
                resetError("Error while loading", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                return;
            }
            while (rsGetMaterialStorageParameters.Read())
            {

                if (DB.RSFieldInt(rsGetMaterialStorageParameters, "MaterialStorageParameterID") == 7 || DB.RSFieldInt(rsGetMaterialStorageParameters, "MaterialStorageParameterID") == 1)
                {
                    //condition for excluding 1 for MfgDate ,7 for CalibrationDate

                    continue;
                }

                if (tdCount == 0)
                {
                    //arrange new row ofter add three table cells
                    MSPRow = new HtmlTableRow();
                    tbMSPTable.Controls.Add(MSPRow);
                }

                String ParameterType = DB.RSField(rsGetMaterialStorageParameters, "ControlType");
                MSPCell = new HtmlTableCell();
                if (tdCount != 2)
                    MSPCell.Width = "35.2%";
                Label lblable = new Label();
                lblable.ID = "lb" + DB.RSField(rsGetMaterialStorageParameters, "ParameterName");
                lblable.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                lblable.Text = DB.RSField(rsGetMaterialStorageParameters, "DisplayName") + ":<br/>";
                MSPCell.Controls.Add(lblable);

                if (ParameterType == "DropDownList")
                {
                    //Parameter type selection then

                    ddlDropDown = new DropDownList();
                    ddlDropDown.Width = 200;
                    ddlDropDown.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                    ddlDropDown.ID = DB.RSField(rsGetMaterialStorageParameters, "ParameterName");
                    BuildDynamicDropDowns(ddlDropDown, DB.RSField(rsGetMaterialStorageParameters, "DataSource"), DB.RSField(rsGetMaterialStorageParameters, "DisplayName"));
                    MSPCell.Controls.Add(ddlDropDown);
                }
                else if (ParameterType == "TextBox")
                {

                    if (DB.RSField(rsGetMaterialStorageParameters, "ParameterName") == "ExpDate" || DB.RSField(rsGetMaterialStorageParameters, "ParameterName") == "CalibrationDueDate")
                    {
                        //Exclucive for Exp Date and Calibration Due Date Change Controls to Selection

                        if (DB.RSField(rsGetMaterialStorageParameters, "ParameterName") == "ExpDate")
                        {
                            lblable.Text = "Expiry Due:<br/>";
                        }
                        else if (DB.RSField(rsGetMaterialStorageParameters, "ParameterName") == "CalibrationDueDate")
                        {
                            lblable.Text = "Calb. Due:<br/>";
                        }

                        ddlDropDown = new DropDownList();
                        ddlDropDown.Width = 200;

                        ddlDropDown.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                        ddlDropDown.ID = DB.RSField(rsGetMaterialStorageParameters, "ParameterName");
                        BuilExpDataDropDown(ddlDropDown);
                        MSPCell.Controls.Add(ddlDropDown);
                        MSPRow.Controls.Add(MSPCell);
                        tdCount = (tdCount + 1) % 3;
                        continue;
                    }

                    txtTextBox = new TextBox();
                    txtTextBox.Width = 200;
                    txtTextBox.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                    txtTextBox.ID = DB.RSField(rsGetMaterialStorageParameters, "ParameterName");
                    if (DB.RSField(rsGetMaterialStorageParameters, "ParameterDataType") == "DateTime")
                    {
                        //set Date Control
                        txtTextBox.EnableTheming = false;
                        txtTextBox.CssClass = "DateBoxCSS_small";
                    }
                    else if (DB.RSField(rsGetMaterialStorageParameters, "ParameterDataType") == "Decimal")
                    {
                        //Set Decimal Check
                        txtTextBox.Attributes.Add("onKeyPress", "return checkDec(event)");
                    }
                    else if (DB.RSField(rsGetMaterialStorageParameters, "ParameterDataType") == "Integer")
                    {
                        //set integer check
                        txtTextBox.Attributes.Add("onKeyPress", "return checkNum(event)");
                    }
                    else if (DB.RSField(rsGetMaterialStorageParameters, "ParameterDataType") == "Nvarchar")
                    {
                        //Exclude special characters
                        txtTextBox.Attributes.Add("onKeyPress", "return checkSpecialChar(event)");
                    }
                    MSPCell.Controls.Add(txtTextBox);

                }

                MSPRow.Controls.Add(MSPCell);

                //calculate cell count for the row
                tdCount = (tdCount + 1) % 3;
            }
            rsGetMaterialStorageParameters.Close();

        }

        private void BuilExpDataDropDown(DropDownList ddlExpDate)
        {

            // Add Drop down list items for Expiry control

            ddlExpDate.Items.Add(new ListItem("All", ""));
            DateTime dtNow = DateTime.Now;
            ddlExpDate.Items.Add(new ListItem("In 0 Days", dtNow.ToString("dd/MM/yyyy")));
            DateTime dt30day = DateTime.Now.AddDays(30);
            ddlExpDate.Items.Add(new ListItem("In 30 Days", dt30day.ToString("dd/MM/yyyy")));
            DateTime dt60day = DateTime.Now.AddDays(60);
            ddlExpDate.Items.Add(new ListItem("In 60 Days", dt60day.ToString("dd/MM/yyyy")));
            DateTime dt90day = DateTime.Now.AddDays(90);
            ddlExpDate.Items.Add(new ListItem("In 90 Days", dt90day.ToString("dd/MM/yyyy")));
            DateTime dt6thmonth = DateTime.Now.AddMonths(6);
            ddlExpDate.Items.Add(new ListItem("In 6 Months", dt6thmonth.ToString("dd/MM/yyyy")));
            DateTime dt1year = DateTime.Now.AddYears(1);
            ddlExpDate.Items.Add(new ListItem("In 1 Year", dt1year.ToString("dd/MM/yyyy")));
        }

        public void BuildDynamicDropDowns(DropDownList ddlDropdown, String sql, String FildName)
        {
            //Add drop down item based on datedase records

            IDataReader rsDropdown = DB.GetRS(sql);
            ddlDropdown.Items.Add(new ListItem("All", ""));
            while (rsDropdown.Read())
            {
                ddlDropdown.Items.Add(new ListItem(rsDropdown.GetString(0), rsDropdown.GetInt32(1).ToString()));
            }
            rsDropdown.Close();
        }

        public void BuildSiteDropDowns(DropDownList ddlDropdown, String sql, String FildName)
        {

            IDataReader rsDropdown = DB.GetRS(sql);
            ddlDropdown.Items.Add(new ListItem("ALL", "__"));
            while (rsDropdown.Read())
            {
                ddlDropdown.Items.Add(new ListItem(DB.RSField(rsDropdown, FildName), DB.RSField(rsDropdown, FildName)));
            }
            rsDropdown.Close();
        }

        public void BuildNumberDropDowns(DropDownList ddlDropdown)
        {

            //Add drop down items form 1 to 100
            ddlDropdown.Items.Add(new ListItem("ALL", "__"));

            for (int i = 1; i < 100; i++)
            {
                ddlDropdown.Items.Add(new ListItem(i.ToString("D2"), i.ToString("D2")));
            }
        }

        public void BuildCharDropDowns(DropDownList ddlDropdown)
        {

            //Add drop down items form A to Z
            ddlDropdown.Items.Add(new ListItem("ALL", "_"));

            for (char i = 'A'; i <= 'Z'; i++)
            {
                ddlDropdown.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
        }

        protected void lnkGetData_Click(object sender, EventArgs e)
        {
            //test case ID (TC_008)
            //select any search feild we get relevant data


            //Get Active stock based on search values

            StringBuilder sCmdGetDetails = new StringBuilder();
            sCmdGetDetails.Append(" DECLARE @Count Int ");
            sCmdGetDetails.Append("Exec [dbo].[sp_INV_GetActiveStockList_New] ");
            sCmdGetDetails.Append("@MaterialMasterID=" + (hifMaterialCode.Value != "" && atcMateialCode.Text.Trim() != "" ? hifMaterialCode.Value : "0"));
            sCmdGetDetails.Append(",@Damaged=" + Convert.ToInt16(chkDamaged.Checked));
            sCmdGetDetails.Append(",@Discepancy=" + Convert.ToInt16(chkDiscripancy.Checked));
            sCmdGetDetails.Append(",@NonConformity=" + Convert.ToInt16(chkNonConformity.Checked));
            sCmdGetDetails.Append(",@IsToolType=" + ddlMaterialType.SelectedValue);
            sCmdGetDetails.Append(",@WarehouseId=" + ddlWarehouse.SelectedValue);
            sCmdGetDetails.Append(",@Tenant=" + (txtTenant.Text.Trim() != "" ? hifTenant.Value : "0"));//cp.TenantID
            sCmdGetDetails.Append(",@GRNNumber=" + DB.SQuote(txtGRNNumber.Text.Trim()));
            sCmdGetDetails.Append(",@CartonID=" + (hdncarton.Value != "" && txtcarton.Text.Trim() != "" ? hdncarton.Value : "0"));
            sCmdGetDetails.Append(",@Replenishment=" + (chkReplenishment.Checked ? 1 : 0));//ddlReplenishments.SelectedValue);
            sCmdGetDetails.Append(",@StorageLocationID=" + ddlStorageLocationID.SelectedValue.ToString()); 
            sCmdGetDetails.Append(",@Location =" + (ddlSite.SelectedValue + "" + ddlAisle.SelectedValue + "" + ddlBay.SelectedValue + "" + ddlBeam.SelectedValue == "_______" ? "null" : DB.SQuote(ddlSite.SelectedValue + "" + ddlAisle.SelectedValue + "" + ddlBay.SelectedValue + "" + ddlBeam.SelectedValue)));

            String MSPNames = "";
            String MSPValues = "";

            //Get Dynamic MSP search Values

            int RowCount = tbMaterialStorageParameter.Controls.Count;
            for (int rowIndex = 0; rowIndex < RowCount; rowIndex++)
            {
                HtmlTableRow row = (HtmlTableRow)tbMaterialStorageParameter.Controls[rowIndex];
                int columncount = row.Controls.Count;
                for (int columnIndex = 0; columncount > columnIndex; columnIndex++)
                {
                    String value = "";
                    String ParameterName = "";
                    Control control = row.Controls[columnIndex].Controls[1];
                    if (control.GetType().ToString() == "System.Web.UI.WebControls.DropDownList" && control.Visible == true)
                    {
                        DropDownList ddldropdown = (DropDownList)control;
                        value = ddldropdown.SelectedValue;
                        ParameterName = ddldropdown.ID;
                    }
                    else if (control.GetType().ToString() == "System.Web.UI.WebControls.TextBox" && control.Visible == true)
                    {
                        TextBox txtTextBox = (TextBox)control;
                        bool ss = txtTextBox.Visible;
                        value = txtTextBox.Text;
                        ParameterName = txtTextBox.ID;
                    }

                    if (value != "")
                    {
                        MSPValues += value + ",";
                        MSPNames += ParameterName + ",";
                    }
                }
            }

            sCmdGetDetails.Append(",@MaterialStorageParameters=" + DB.SQuote(MSPNames));
            sCmdGetDetails.Append(",@MaterialStorageParameterValues =" + DB.SQuote(MSPValues));


            try
            {
                ViewState["SearchDetails"] = sCmdGetDetails.ToString();
                bindGrid(1);
                if (gvActiveStock.Rows.Count == 0)
                {
                    resetError("No material is available with the given criteria", true);
                }
                else
                {
                    lbMateialsStatus.Text = "";
                }

            }
            catch (Exception ex)
            {
                resetError("Erroe while loading", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }

        }

        protected void gvActiveStock_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && !(e.Row.RowState == DataControlRowState.Edit))
            {
                String Location = ((Literal)e.Row.FindControl("ltSplitLocation")).Text;
                String MCode = ((Literal)e.Row.FindControl("ltMaterialCode")).Text;
                String MDescription = ((Literal)e.Row.FindControl("ltMDescription")).Text;
                String OEMPartNumber = ((Literal)e.Row.FindControl("ltOEMPartNo")).Text;

                if (Location != null)
                {
                    StringBuilder SplitLocation = GenerateLocationTable(Location, MCode, OEMPartNumber, MDescription);
                    ((Literal)e.Row.FindControl("ltMSPs")).Text = SplitLocation.ToString();
                }
            }


        }


        private StringBuilder GenerateLocationTable(String SplitLocation, String MCode, String OEMNumber, String ItemDescription)
        {
            //Arrange MSP and available stock quantity details

            StringBuilder generateTable = new StringBuilder();
            if (SplitLocation != null && SplitLocation != "")
            {
                double[] tablecellWidth = { 120, 160, 40, 50, 50, 70, 60 };

                generateTable.Append("<table border=\"1\" align=\"center\"width=\"100%\"  style=\"border-collapse:collapse;border: 1px solid black;\" >");

                String[] Locations = SplitLocation.Split(',');

                for (int rowposition = 0; rowposition < Locations.Length; rowposition++)
                {
                    String[] msps = Locations[rowposition].Split('|');
                    if (msps[12] != "")
                    {
                        if (DateTime.ParseExact(msps[12], "dd/MM/yyyy", CultureInfo.InvariantCulture) < DateTime.Now)
                        {
                            generateTable.Append("<tr style=\"background-color:pink;\">");
                        }
                        else
                        {
                            generateTable.Append("<tr>");
                        }
                    }
                    else
                    {
                        generateTable.Append("<tr>");
                    }


                    for (int columnPOsition = 0; columnPOsition < msps.Length; columnPOsition++)
                    {

                        if (columnPOsition > 5 && columnPOsition <= 8)
                        {
                            //Replace value by image while check data (Dam.,Disc.,QC Non Conf.,As Is,Pve Recall)

                            generateTable.Append("<td align=\"center\" valign=\"center\" width=\"" + 35 + "px\" >");

                            if (msps[columnPOsition] == "1")
                            {
                                generateTable.Append("<img  src=\"../Images/blue_menu_icons/check_mark.png\"  /> ");
                            }

                        }

                        else if (columnPOsition < 2)
                        {
                            //align center of location and kit values

                            generateTable.Append("<td  title=\"" + msps[columnPOsition] + "\" align=\"left\" width=\" " + tablecellWidth[columnPOsition] + "px\" >");
                            generateTable.Append(msps[columnPOsition]);

                        }
                        else if (columnPOsition <= 5)
                        {
                            //arrage values of stock GoodsIn-on hold,Goodsout-On Hold and Available 

                            generateTable.Append("<td align=\"right\" width=\" " + tablecellWidth[columnPOsition] + "px\" >");
                            generateTable.Append(msps[columnPOsition]);

                        }
                        else if (columnPOsition > 10)
                        {
                            //arrage valiues of MSP

                            if (msps[columnPOsition].Length > 8)
                            {
                                //if MSP value is too lenght then display as tooltip also
                                generateTable.Append("<td title=\"" + msps[columnPOsition] + "\" width=\"" + 70 + "px\" >");
                            }
                            else
                            {
                                generateTable.Append("<td  width=\"" + 70 + "px\" >");
                            }

                            generateTable.Append(msps[columnPOsition]);

                        }
                        generateTable.Append("</td>");


                    }

                    if (!isexcel)
                    {
                        generateTable.Append("<td width=\"" + 20 + "px\"> <input type=checkbox class=\"print\" onclick=\"javascript:check_All()\" name=\"" + (MCode + "" + msps[0].Trim()) + "\"/> </td>");


                        if (Page.Request.Form[(MCode + "" + msps[0].Trim())] != null && Page.Request.Form[(MCode + "" + msps[0].Trim())] == "on")
                        {
                            PrintLabel((msps[5] == "1" ? MCode + "-D" : MCode), OEMNumber, ItemDescription, msps[10], msps[11], msps[12], msps[13]);

                            //Page.Request.Form[(MCode + "" + msps[0].Trim())] = null;
                        }
                    }

                    generateTable.Append("</tr>");


                }

                generateTable.Append("</table>");

            }
            return generateTable;
        }


        private DataTable BuildgvActiveStock(int CurrentPage, int PageSize)
        {
            AddGridHeaderList();
            String sCmdActiveStock = ViewState["SearchDetails"].ToString();
            sCmdActiveStock += ",@PageIndex=" + CurrentPage + " ,@PageSize=" + PageSize + " ,@RowCount=@Count output select @Count as n  ";

            DataSet dsActiveStock = DB.GetDS(sCmdActiveStock, false);

            _TotalRowCount = Convert.ToInt32(dsActiveStock.Tables[1].Rows[0][0]);
            lblRecordCount.Text = "Total Items [ " + _TotalRowCount + " ]";
            generatePager(_TotalRowCount, PageSize, CurrentPage);


            return dsActiveStock.Tables[0];
        }

        private void BuildgvActiveStock(DataTable dtActiveStock)
        {
            gvActiveStock.DataSource = dtActiveStock;
            gvActiveStock.DataBind();
            dtActiveStock.Dispose();
        }

        protected void gvActiveStock_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvActiveStock.PageIndex = e.NewPageIndex;
            //BuildgvActiveStock(BuildgvActiveStock(0));
        }

        protected void gvActiveStock_RowCommand(object sender, GridViewCommandEventArgs e)
        {


            if (e.CommandName == "viewpics")
            {
                String MMTPath = DB.GetSqlS(" select DefaultValue AS S from SYS_SystemConfiguration SYS_C left join SYS_SysConfigKey SYS_K on SYS_K.SysConfigKeyID=SYS_C.SysConfigKeyID where SysConfigKey='MaterialManagementPath'");
                String[] parms = e.CommandArgument.ToString().Split(',');
                String mid = parms[0];
                String Mcode = parms[1];
                String TenantID = parms[2];

                String loacalfolder = DB.GetSqlS("select DefaultValue AS S from SYS_SystemConfiguration SYS_C left join SYS_SysConfigKey SYS_K on SYS_K.SysConfigKeyID=SYS_C.SysConfigKeyID where SysConfigKey='TenantContentPath'") + TenantID + MMTPath + mid;
                String midSavepath = System.Web.HttpContext.Current.Server.MapPath("~\\" + loacalfolder); ;
                String sidpath = "";

                trvmaterialattachment.Nodes[0].ChildNodes.Clear();

                TreeNode trnInternalOrder;
                TreeNode trnsubInternalOrder;
                trvmaterialattachment.Nodes[0].Text = Mcode;
                //DB.GetSqlS("select Mcode as S from MMT_MaterialMaster where MaterialMasterID = " + mid + " and isactive=1 and isdeleted=0");
                //IDataReader rsGetSupplierList = DB.GetRS("select mmsup.SupplierID,sup.SupplierName from MMT_MaterialMaster_Supplier mmsup join MMT_Supplier sup on sup.SupplierID=mmsup.SupplierID  where MaterialMasterID=" + mid + " and sup.isactive=1 and sup.isdeleted=0");
                IDataReader rsGetSupplierList = DB.GetRS("select mmsup.SupplierID,sup.SupplierName from MMT_MaterialMaster_Supplier mmsup join MMT_Supplier sup on sup.SupplierID=mmsup.SupplierID  where MaterialMasterID=" + mid + " and sup.isactive=1 and sup.isdeleted=0  and mmsup.IsDeleted=0");

                while (rsGetSupplierList.Read())
                {
                    trnInternalOrder = new TreeNode();
                    trnInternalOrder.Expanded = false;
                    trnInternalOrder.Text = DB.RSField(rsGetSupplierList, "SupplierName");
                    sidpath = midSavepath + "//" + DB.RSFieldInt(rsGetSupplierList, "SupplierID");
                    String Attachmentname = "";
                    if (Directory.Exists(sidpath))
                    {
                        string[] Attachmentlist = Directory.GetFiles(sidpath);
                        foreach (String Attachment in Attachmentlist)
                        {
                            Attachmentname = Path.GetFileName(Attachment);
                            trnsubInternalOrder = new TreeNode();
                            trnsubInternalOrder.Text = Attachmentname;
                            trnsubInternalOrder.NavigateUrl = Attachmentname.EndsWith(".pdf") ? String.Format("../mMaterialManagement/DisplayPDF.aspx?sid={0}&mid={1}&filename={2}&tid={3}", DB.RSFieldInt(rsGetSupplierList, "SupplierID"), mid, Attachmentname, TenantID) : "../" + loacalfolder + "/" + DB.RSFieldInt(rsGetSupplierList, "SupplierID") + "/" + Attachmentname;
                            trnsubInternalOrder.Expanded = false;
                            trnInternalOrder.ChildNodes.Add(trnsubInternalOrder);

                        }
                    }
                    else
                    {

                        trnsubInternalOrder = new TreeNode();
                        trnsubInternalOrder.Expanded = false;
                        trnsubInternalOrder.Text = "Empty";
                        trnInternalOrder.ChildNodes.Add(trnsubInternalOrder);
                    }
                    trvmaterialattachment.Nodes[0].ChildNodes.Add(trnInternalOrder);
                }
                rsGetSupplierList.Close();
                //ScriptManager.RegisterStartupScript(phrJsRunner, phrJsRunner.GetType(), "unblockAycDialog", "unblockAycDialog()", true);
                //RegisterStartupScript("jsUnblockDialog", "OpenAttachment(\"" + Mcode + "  Attachments\");");
                StartupScript("jsUnblockDialog", "unblockPartNumberDialog();");


            }

        }

        private void StartupScript(string key, string script)
        {
            ScriptManager.RegisterStartupScript(phrJsRunner, phrJsRunner.GetType(), key, script, true);
        }

        // CommonLogic for Loading DropDownList
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

        protected void resetError(string error, bool isError)
        {
            /*
            string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
            if (isError)
                str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

            if (error.Length > 0)
                str += error + "";
            else
                str = "";
            lblStatusMessage.Text = str;*/

            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);


        }

        protected void btnASExprtExcel_Click(object sender, ImageClickEventArgs e)
        {

            try
            {
                gvActiveStock.AllowPaging = false;
                string[] hiddencolumns = { "In Act.", "Out Act.", };
                isexcel = true;
                AddGridHeaderList();
                //BuildgvActiveStock(BuildgvActiveStock());
                BuildgvActiveStock(BuildgvActiveStock(0, Convert.ToInt32(drppagesize.SelectedValue)));
                CommonLogic.ExporttoExcel(gvActiveStock, "ActiveStock", hiddencolumns);
            }
            catch (Exception ex)
            {
                resetError("Error While Exporting Data", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }

        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        #region ----------  Label Printing  ------------------

        protected void lnkPrint_Click(object sender, EventArgs e)
        {

            if (ddlNetworkPrinter.SelectedValue == "0")
            {
                resetError("Please select printer", true);
                return;
            }

            bindGrid(Convert.ToInt32("0" + hfpageId.Value));
        }

        public void LoadStoresWithIPs(DropDownList ddlWH)
        {

            ddlWH.Items.Clear();

            ddlWH.Items.Add(new ListItem("Select Printer", "0"));
            ddlWH.Items.Add(new ListItem("Data Max - LP", "172.18.0.127"));
            ddlWH.Items.Add(new ListItem("Zebraa - LP", "172.18.0.53"));

        }

        public void PrintLabel(String MCode, String OEMNumber, String Description, String MfgDate, String ExpDate, String SerialNo, String BatchNo)
        {
            try
            {
                MRLWMSC21Common.TracklineMLabel thisMLabel = new MRLWMSC21Common.TracklineMLabel();

                thisMLabel.MCode = MCode;
                thisMLabel.OEMPartNo = OEMNumber;
                thisMLabel.Description = Description;
                thisMLabel.AltMCode = "";

                thisMLabel.InvQty = 1;

                string vltMfgDate = MfgDate;
                string vltExpiryDate = ExpDate;
                string vltBatchNo = BatchNo;
                string vltSerialNo = SerialNo;



                string mfgdate = "";
                if (vltMfgDate != "")
                {
                    try
                    {
                        mfgdate = DateTime.ParseExact(vltMfgDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                    }
                    catch (Exception ex)
                    {
                        mfgdate = DateTime.MinValue.ToString("MM/dd/yyyy");
                        CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                    }
                }
                else
                {
                    mfgdate = DateTime.MinValue.ToString("MM/dd/yyyy");
                }


                string expdate = "";
                if (vltExpiryDate != "")
                {
                    try
                    {
                        expdate = DateTime.ParseExact(vltExpiryDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                    }
                    catch (Exception ex)
                    {
                        expdate = DateTime.MinValue.ToString("MM/dd/yyyy");
                        CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                    }
                }
                else
                {
                    expdate = DateTime.MinValue.ToString("MM/dd/yyyy");
                }

                thisMLabel.MfgDate = mfgdate;
                thisMLabel.ExpDate = expdate;

                thisMLabel.BatchNo = vltBatchNo;
                thisMLabel.SerialNo = vltSerialNo;



                thisMLabel.PrinterType = "IP";
                thisMLabel.PrinterIP = ddlNetworkPrinter.SelectedValue.Trim();
                thisMLabel.IsBoxLabelReq = false;
                thisMLabel.ReqNo = "";

                thisMLabel.StrRefNo = "";
                thisMLabel.KitCode = "";
                thisMLabel.OBDNumber = "";

                Thread worker = new Thread(new ParameterizedThreadStart(this.ThermalLabelWorker));
                worker.SetApartmentState(ApartmentState.STA);
                worker.Name = "ThermalLabelWorker";
                worker.Start(thisMLabel);
                worker.Join();

                if (thisMLabel.Result != "Success")
                {
                    resetError("Error while printing. Please contact admin<br/>" + thisMLabel.Result, false);
                }
                else
                {
                    resetError("Successfully printed selected line items", false);
                }



            }
            catch (Exception ex)
            {
                resetError("Error while printing", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                return;
            }
        }

        private void ThermalLabelWorker(object thisMLabel2)
        {
            try
            {
                MRLWMSC21Common.TracklineMLabel thisMLabel = (MRLWMSC21Common.TracklineMLabel)thisMLabel2;
                String vresult = "";
               // CommonLogic.SendPrintJob_Big_7p6x5(thisMLabel.MCode, thisMLabel.AltMCode, thisMLabel.OEMPartNo, thisMLabel.Description, thisMLabel.BatchNo, thisMLabel.SerialNo, thisMLabel.KitPlannerID.ToString(), thisMLabel.KitChildrenCount, thisMLabel.ParentMCode, thisMLabel.InvQty, thisMLabel.MfgDate, thisMLabel.ExpDate, thisMLabel.PrinterType, thisMLabel.PrinterIP, thisMLabel.StrRefNo, thisMLabel.OBDNumber, thisMLabel.KitCode, thisMLabel.ReqNo, thisMLabel.IsBoxLabelReq, false, thisMLabel.PrintQty, out vresult);
                thisMLabel.Result = vresult;
            }
            catch (Exception ex)
            {
                resetError("Error while printing", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                return;
            }
        }

        #endregion --------- Label Printing --------------------

        protected void dlPager_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "PageNo")
            {
                pageIndex = Convert.ToInt32(e.CommandArgument);
                hfpageId.Value = pageIndex.ToString();
                bindGrid(pageIndex);
            }
            StartupScript("jsUnblockDialog", "closeAsynchronus();");
        }

        public void generatePager(int totalRowCount, int pageSize, int currentPage)
        {
            //This is for when 
            if (pageSize * (currentPage - 1) > totalRowCount)
                currentPage = (int)Math.Ceiling((decimal)totalRowCount / pageSize);


            int totalLinkInPage = 10;
            int totalPageCount = (int)Math.Ceiling((decimal)totalRowCount / pageSize);

            int startPageLink = Math.Max(currentPage - (int)Math.Floor((decimal)totalLinkInPage / 2), 1);
            int lastPageLink = Math.Min(startPageLink + totalLinkInPage - 1, totalPageCount);

            if ((startPageLink + totalLinkInPage - 1) > totalPageCount)
            {
                lastPageLink = Math.Min(currentPage + (int)Math.Floor((decimal)totalLinkInPage / 2), totalPageCount);
                startPageLink = Math.Max(lastPageLink - totalLinkInPage + 1, 1);
            }

            List<ListItem> pageLinkContainer = new List<ListItem>();

            if (startPageLink != 1)
                pageLinkContainer.Add(new ListItem("&lt;&lt;FirstPage", "01", currentPage != 1));
            for (int i = startPageLink; i <= lastPageLink; i++)
            {
                pageLinkContainer.Add(new ListItem(i.ToString("00"), i.ToString(), currentPage != i));
            }
            if (lastPageLink != totalPageCount)
                pageLinkContainer.Add(new ListItem("LastPage&gt;&gt;", totalPageCount.ToString("00"), currentPage != totalPageCount));

            dlPagerupper.DataSource = pageLinkContainer;
            dlPagerupper.DataBind();
            dlPager.DataSource = pageLinkContainer;
            dlPager.DataBind();
        }

        public void bindGrid(int currentPage)
        {
            int pageSize = Convert.ToInt32(drppagesize.SelectedValue);
            //_TotalRowCount = 0;
            BuildgvActiveStock(BuildgvActiveStock(currentPage, pageSize));
        }

        protected void drppagesize_SelectedIndexChanged(object sender, EventArgs e)
        {
            //bindGrid(pageIndex);
            //String label=dlPager.SelectedValue.ToString();
            string val = gvActiveStock.PageIndex.ToString();
            BuildgvActiveStock(BuildgvActiveStock(Convert.ToInt32(hfpageId.Value), Convert.ToInt32(drppagesize.SelectedValue)));

        }

    }


}