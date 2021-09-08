using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21Common;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Globalization;
using System.Resources;
using ClosedXML.Excel;
using System.Text.RegularExpressions;

namespace MRLWMSC21.mInventory
{
    //Author    :   Gvd Prasad
    //Created On:   19-Sep-2013

    public partial class SupplierList : System.Web.UI.Page
    {

        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        protected string selectMMList = "";
        OleDbConnection Econ;
        SqlConnection con;
        private int TenantID = 0;
        private string TenantRootDir = "";
        private string MMTPath = "";
        private string MMTExcelPath = "";
        string constr, Query, sqlconn;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "MasterData";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            TenantID = cp.TenantID;
            ViewState["TenantID"] = cp.TenantID;


            //Get Tenant Root Directory
            TenantRootDir = DB.GetSqlS("select DefaultValue AS S from SYS_SystemConfiguration SYS_C left join SYS_SysConfigKey SYS_K on SYS_K.SysConfigKeyID=SYS_C.SysConfigKeyID where SysConfigKey='TenantContentPath'");

            // Get MaterialMaster Path
            MMTPath = DB.GetSqlS(" select DefaultValue AS S from SYS_SystemConfiguration SYS_C left join SYS_SysConfigKey SYS_K on SYS_K.SysConfigKeyID=SYS_C.SysConfigKeyID where SysConfigKey='MaterialManagementPath'");

            MMTExcelPath = DB.GetSqlS(" select DefaultValue AS S from SYS_SystemConfiguration SYS_C left join SYS_SysConfigKey SYS_K on SYS_K.SysConfigKeyID=SYS_C.SysConfigKeyID where SysConfigKey='MaterialManagementImportExcelPath'");

            //if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("Supplier List"))))
            //{
            //    Response.Redirect("../Login.aspx?eid=6");
            //}
            if (!IsPostBack)
            {
                hifTenant.Value = cp.TenantID.ToString();
                txtTenant.Enabled = CommonLogic.CheckSuperAdmin(txtTenant, cp, hifTenant);

                DesignLogic.SetInnerPageSubHeading(this.Page, "Supplier");
                if (CommonLogic.QueryString("statid") == "success")
                    resetBlkLableError("Successfully Saved", false);
                else if (CommonLogic.QueryString("statid") == "codesuccess")
                    resetBlkLableError("Successfully Saved", false);

                // txtSearchSuplier.Attributes["onclick"] = "javascript:this.focus();";
                // txtSearchSuplier.Attributes["onfocus"] = "javascript:this.select();";

                ViewState["MMListSort"] = "SupplierID";
                ViewState["MMListSortOrder"] = "DESC";
                //selectMMList = "Exec [dbo].[sp_MMT_SupplierList] @SupplierCode='" + (txtSupplierCode.Text.Trim() != "Supplier Code..." ? txtSupplierCode.Text.Trim() : "") + "',@SupplierName='" + (txtSupplier.Text.Trim() != "Supplier Name..." ? txtSupplier.Text.Trim() : "") + "',@UserRoleID=null";

                string Supplier = txtSupplier.Text.Split('-')[0];
                if (Supplier == "Supplier Name...")
                    Supplier = "";

                string TenantName = txtTenant.Text.Split('-')[0];
                if (TenantName == "Tenant...")
                    TenantName = "";

                selectMMList = "Exec [dbo].[sp_TPL_GetSupplierList] @SupplierCode='',@TenantName=" + DB.SQuote(TenantName) + ",@SupplierName=" + DB.SQuote(Supplier) + ",@UserRoleID=null, @TenantID=" + (hifTenant.Value);

                //selectMMList = selectMMList + ",@AccountID="+cp.AccountID.ToString() + ",@UserTypeID="+cp.UserTypeID.ToString(); 

                selectMMList = selectMMList + ",@AccountID_New =" + cp.AccountID.ToString() + ",@UserTypeID=" + cp.UserTypeID.ToString() + ",@TenantID_New = " + cp.TenantID.ToString();

                ViewState["MMListSQL"] = this.selectMMList;

                if (cp.TenantID != 0)
                {
                    gvManList.Columns[2].Visible = false;
                }


                this.MMList_buildGridData(this.MMList_buildGridData());
            }
        }

        public Boolean GetBool(String IsChecked)
        {


            if (IsChecked != "")
            {
                if (Convert.ToBoolean(Convert.ToInt32(IsChecked)))
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }


        }

        protected void lnkClearData_Click(object sender, EventArgs e)
        {


        }

        protected void lnkGetItemDetails_Click(object sender, EventArgs e)
        {



        }

        protected void resetError(string error, bool isError)
        {

            /*string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
            if (isError)
                str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

            if (error.Length > 0)
                str += error + "";
            else
                str = "";


            lblStatus.Text = str;*/
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);

        }

        protected void resetBlkLableError(string error, bool isError)
        {

            /*string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
            if (isError)
                str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

            if (error.Length > 0)
                str += error + "";
            else
                str = "";


            lblGroupLabelStatus.Text = str;*/
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);

        }

        protected void lnkGetData_Click(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            //selectMMList = "Exec [dbo].[sp_MMT_SupplierList] @SupplierCode=" + (txtSupplierCode.Text.Trim() != "Supplier Code..." ? DB.SQuote(txtSupplierCode.Text.Trim()) : "''") + ",@SupplierName=" + (txtSupplier.Text.Trim() != "Supplier Name..." ? DB.SQuote(txtSupplier.Text.Trim()) : "''") + ",@UserRoleID=Null";

            string text = txtSupplier.Text.Split('-')[0];
            if (text == "Supplier Name...")
                text = "";

            string TenantName = txtTenant.Text.Split('-')[0];
            if (TenantName == "Tenant...")
                TenantName = "";

            selectMMList = "Exec [dbo].[sp_TPL_GetSupplierList] @SupplierCode='',@SupplierName=" + DB.SQuote(text) + ",@UserRoleID=Null, @TenantName=" + (DB.SQuote(TenantName));

            selectMMList = selectMMList + ",@AccountID=" + hifAccount.Value + ",@AccountID_New =" + cp.AccountID.ToString() + ",@UserTypeID=" + cp.UserTypeID.ToString() + ", @TenantID_New = " + cp.TenantID.ToString();

            ViewState["MMListSQL"] = this.selectMMList;
            this.MMList_buildGridData(this.MMList_buildGridData());
        }

        protected void btnLiveStockEE_Click(object sender, EventArgs e)
        {
            Int16[] HiddenColumns = { 4, 5, 6 };
            //CommonLogic.PrepareGridViewForExport(gvManList, HiddenColumns);
            gvManList.AllowPaging = false;
            this.MMList_buildGridData(this.MMList_buildGridData());
            CommonLogic.PrepareGridViewForExport(gvManList, HiddenColumns);
            String ExcelFileName = "SupplierList";
            CommonLogic.ExportGridView(gvManList, ExcelFileName);
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        public string GetUoMDetails(String BUoM1, String BQty1, String BUoM2, String BQty2, String BUoM3, String BQty3)
        {
            String result = "";

            if (BUoM1 != "")
                result = BUoM1 + "/" + BQty1;

            if (BUoM2 != "")
                result += "<br/>" + BUoM2 + "/" + BQty2;

            if (BUoM3 != "")
                result += "<br/>" + BUoM3 + "/" + BQty3;

            return result;
        }

        public string GetCheckValue(String IsChecked)
        {
            String resultValue = "";

            if (IsChecked != "")
            {
                if (Convert.ToBoolean(Convert.ToInt32(IsChecked)))
                    resultValue = "<img src='images/plain_checked.gif' border='0'/>";
            }

            return resultValue;
        }

        #region -------------------------MMList Grid Data-----------------------------

        protected DataSet MMList_buildGridData()
        {
            DataSet dsGetSupplierList = null;
            String sql = ViewState["MMListSQL"].ToString();
            try
            {

                dsGetSupplierList = DB.GetDS(sql, false);
            }
            catch (Exception ex)
            {
                resetError("Error while loading", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
            return dsGetSupplierList;
        }

        protected void gvManList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int index = e.RowIndex;
            GridViewRow ro = gvManList.Rows[index];
        }

        protected void MMList_buildGridData(DataSet dsGetSupplierList)
        {
            gvManList.DataSource = dsGetSupplierList;
            gvManList.DataBind();
            dsGetSupplierList.Dispose();

        }

        protected void gvManList_Sorting(object sender, GridViewSortEventArgs e)
        {
            ViewState["ShimpentPendingIsInsert"] = false;
            gvManList.EditIndex = -1;
            ViewState["MMListSort"] = e.SortExpression.ToString();
            ViewState["MMListSortOrder"] = (ViewState["MMListSortOrder"].ToString() == "ASC" ? "DESC" : "ASC");
            this.MMList_buildGridData(this.MMList_buildGridData());
        }

        protected void gvManList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //======================= Added By M.D.Prasad For View Only Condition ======================//
            string[] hh = cp.Roles;
            for (int i = 0; i < hh.Length; i++)
            {
                if (cp.UserTypeID == 3 && Convert.ToInt32(hh[i]) == 4)
                {
                    FUSupImportExcel.Visible = false;
                    lnkflupldImportExcel.Visible = false;
                    lnkAdd.Visible = false;
                    btnSample.Visible = false;
                    btnLiveStockE.Visible = false;
                }
            }
            //======================= Added By M.D.Prasad For View Only Condition ======================//
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if ((e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowState == DataControlRowState.Alternate))
                    {

                        Boolean vIsApproved = ((CheckBox)e.Row.Cells[4].FindControl("chkIsApproved")).Checked;
                        LinkButton vlnkEditItem = (LinkButton)e.Row.Cells[5].FindControl("lnkEditItem");

                        //======================= Added By M.D.Prasad For View Only Condition ======================//
                        for (int i = 0; i < hh.Length; i++)
                        {
                            if (cp.UserTypeID == 3 && Convert.ToInt32(hh[i]) == 4)
                            {
                                gvManList.Columns[6].Visible = false;
                            }
                        }
                        //======================= Added By M.D.Prasad For View Only Condition ======================//

                        // String vRequestedBy = ((Literal)e.Row.Cells[5].FindControl("lthidRequestedBy")).Text;
                    }
                }
            }

            if (cp.TenantID != 0)
            {
                if (e.Row.RowType == DataControlRowType.DataRow && ((e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowState == DataControlRowState.Alternate)))
                {
                    gvManList.Columns[6].Visible = false;
                    //======================= Added By M.D.Prasad For View Only Condition ======================//
                    for (int i = 0; i < hh.Length; i++)
                    {
                        if (cp.UserTypeID == 3 && Convert.ToInt32(hh[i]) == 4)
                        {
                            gvManList.Columns[6].Visible = false;
                        }
                    }
                    //======================= Added By M.D.Prasad For View Only Condition ======================//
                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    LinkButton lnkUpdateIsIsActive = (LinkButton)e.Row.FindControl("lnkUpdateIsIsActive");
                    LinkButton lnkUpdateIsApproved = (LinkButton)e.Row.FindControl("lnkUpdateIsApproved");

                    lnkUpdateIsIsActive.Visible = false;
                    lnkUpdateIsApproved.Visible = false;
                }
            }


        }

        protected void gvManList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["MMListIsInsert"] = false;

            gvManList.PageIndex = e.NewPageIndex;
            gvManList.EditIndex = -1;
            this.MMList_buildGridData(this.MMList_buildGridData());
        }

        protected void btnShipInTranEE_Click(object sender, EventArgs e)
        {
            Int16[] HiddenColumns = { 0, 8 };
            CommonLogic.PrepareGridViewForExport(gvManList, HiddenColumns);
            this.MMList_buildGridData(this.MMList_buildGridData());
            CommonLogic.ExportGridView(gvManList, "MMItemsList");
        }

        //have to modify
        protected void lnkUpdateIsApproved_Click(object sender, EventArgs e)
        {


            string gvchkIDs = "";
            string gvUnChkIDs = "";

            bool approvechkBox = false;

            //'Navigate through each row in the GridView for checkbox items
            foreach (GridViewRow gv in gvManList.Rows)
            {
                CheckBox isApprovedChkBxItem = (CheckBox)gv.FindControl("chkIsApproved");

                if (isApprovedChkBxItem.Checked)
                {
                    approvechkBox = true;
                    // Concatenate GridView items with comma for SQL Delete
                    gvchkIDs += ((Literal)gv.FindControl("ltSupplierID")).Text.ToString() + ",";

                }
                else
                {
                    gvUnChkIDs += ((Literal)gv.FindControl("ltSupplierID")).Text.ToString() + ",";
                }

            }



            // Execute SQL Query only if checkboxes are checked to avoid any error with initial null string
            try
            {
                string updateSQL = "";
                if (approvechkBox)
                {
                    updateSQL = "EXEC dbo.sp_MMT_UpsertSupplierStatus @IsApprovedSupplierIDs='" + gvchkIDs + "', @UpdatedBy= " + cp.UserID.ToString();
                    DB.ExecuteSQL(updateSQL);

                    updateSQL = "EXEC dbo.sp_MMT_UpsertSupplierStatus @IsNotApprovedSupplierIDs='" + gvUnChkIDs + "', @UpdatedBy= " + cp.UserID.ToString();

                    DB.ExecuteSQL(updateSQL);
                    resetBlkLableError("Successfully Saved", false);
                }
                else
                {
                    updateSQL = "EXEC dbo.sp_MMT_UpsertSupplierStatus @IsNotApprovedSupplierIDs='" + gvUnChkIDs + "', @UpdatedBy= " + cp.UserID.ToString();

                    DB.ExecuteSQL(updateSQL);
                    resetBlkLableError("Successfully Saved", false);
                }
            }
            catch (Exception ex)
            {
                resetBlkLableError("Error while updating data", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);

            }
            this.MMList_buildGridData(this.MMList_buildGridData());
        }

        public string GetUserName(String UserID)
        {
            if (UserID == "")
                return "";
            else
            {
                return CommonLogic.GetUserName(UserID);
            }
        }

        public void edit_click(object sender, EventArgs e)
        {

            Response.Redirect("SupplierRequest.aspx?mfgid=");
        }

        #endregion

        protected void lnkMMListSearch_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void lnkUpdateIsIsActive_Click(object sender, EventArgs e)
        {
            string gvactIds = "";
            string gvunactIds = "";
            cp = HttpContext.Current.User as CustomPrincipal;
            bool actchkbox = false;
            //'Navigate through each row in the GridView for checkbox items
            foreach (GridViewRow gv in gvManList.Rows)
            {

                CheckBox isDeletedchkbxItem = (CheckBox)gv.FindControl("chkIsActive");

                if (isDeletedchkbxItem.Checked)
                {
                    actchkbox = true;

                    gvactIds += ((Literal)gv.FindControl("ltSupplierID")).Text.ToString() + ",";
                }
                else
                {
                    gvunactIds += ((Literal)gv.FindControl("ltSupplierID")).Text.ToString() + ",";
                }

            }
            // Execute SQL Query only if checkboxes are checked to avoid any error with initial null string
            try
            {
                string updateSQL = "";

                if (actchkbox)
                {
                    updateSQL = "EXEC dbo.sp_MMT_UpsertSupplierStatus @IsActiveSupplierIDs='" + gvactIds + "', @UpdatedBy= " + cp.UserID.ToString();

                    DB.ExecuteSQL(updateSQL);
                    updateSQL = "EXEC dbo.sp_MMT_UpsertSupplierStatus @IsNotActiveSupplierIDs='" + gvunactIds + "', @UpdatedBy= " + cp.UserID.ToString();

                    DB.ExecuteSQL(updateSQL);
                    resetBlkLableError("Successfully Saved", false);
                }
                else
                {
                    updateSQL = "EXEC dbo.sp_MMT_UpsertSupplierStatus @IsNotActiveSupplierIDs='" + gvunactIds + "', @UpdatedBy= " + cp.UserID.ToString();

                    DB.ExecuteSQL(updateSQL);
                    resetBlkLableError("Successfully Saved", false);
                }
            }
            catch (Exception ex)
            {
                resetBlkLableError("Error while updating data", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);

            }
            //Session["MMListSQL"] = this.selectMMList;
            this.MMList_buildGridData(this.MMList_buildGridData());
        }
        public void upsertSupplierDetails(string Accountcode, string Tenant, string SupplierName, string SupplierCode, string Telephone, string Email, string address, string currency, string country, string ContactPerson, string title, string contactno, string PCPEmail)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            StringBuilder sqlInsert = new StringBuilder();
            sqlInsert.Append("EXEC [dbo].[sp_MMT_importSupplier] ");
            sqlInsert.Append("@SupplierName=" + DB.SQuote(SupplierName));
            sqlInsert.Append(",@SupplierCode =" + DB.SQuote(SupplierCode));
            sqlInsert.Append(",@AccountName =" + DB.SQuote(Accountcode));
            sqlInsert.Append(",@Tenant=" + DB.SQuote(Tenant));
            sqlInsert.Append(",@Phone1 =" + DB.SQuote(Telephone));
            sqlInsert.Append(",@Address1 =" + DB.SQuote(address));
            sqlInsert.Append(",@CountryName  =" + DB.SQuote(country));
            sqlInsert.Append(",@EmailAddress =" + DB.SQuote(Email));
            sqlInsert.Append(",@PCP =" + DB.SQuote(ContactPerson));
            sqlInsert.Append(",@PCPTitle =" + DB.SQuote(title));
            sqlInsert.Append(",@PCPContactNumber =" + DB.SQuote(contactno));
            sqlInsert.Append(",@PCPEmail =" + DB.SQuote(PCPEmail));
            sqlInsert.Append(",@Currency=" + DB.SQuote(currency));
            sqlInsert.Append(",@CreatedBy = " + cp.UserID.ToString());

            DB.ExecuteSQL(sqlInsert.ToString());

            //Response.Redirect("SupplierList.aspx?statid=success", false);
            //try
            //{
            //    int ResMfgRequest = DB.GetSqlN(sqlInsert.ToString());
            //    if (ResMfgRequest == -1)  //to Check Supplier Name duplication while in Edit Mode OR New Record
            //    {
            //        resetError("This Supplier Name is already existing.", false);
            //        return;
            //    }
            //    else if (ResMfgRequest > 0) //If insertion or updation is successful, send an E-Mail
            //    {
            //        resetError("Successfully Saved", false);
            //        Response.Redirect("SupplierList.aspx?statid=success", false);
            //    }

            //}
            //catch (SqlException ex)
            //{
            //    if (ex.Message.IndexOf(" Cannot insert duplicate key in object 'dbo.MMT_Supplier'.") > -1)
            //    {
            //        resetError("Supplier code already exists under this Tenant", true);
            //    }
            //    else
            //    {
            //        resetError("Error while submitting the data", true);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    resetError("Error while submitting the data", true);
            //}
        }
        private void getConnection()
        {
            try
            {
                sqlconn = CommonLogic.Application("DBConn");
                con = new SqlConnection(sqlconn);
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                return;
            }

        }
        protected void lnkflupldImportExcel_Click(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                string completePath = Server.MapPath("~/ExcelData/" + FUSupImportExcel.FileName);
                string actualFile = Server.MapPath("~/mMaterialManagement/SampleTemplateForMaterial/SupplierFile.xlsx");
                if (FUSupImportExcel.HasFile)
                {
                    //CommonLogic.UploadFile(TenantRootDir + DB.GetSqlS("select UniqueID AS S from GEN_Tenant where TenantID=" + TenantID) + MMTExcelPath, FUSupImportExcel, "SupplierList" + Path.GetExtension(FUSupImportExcel.FileName));
                    //String sFileName = CommonLogic._GetAttatchmentFile(TenantRootDir + DB.GetSqlS("select UniqueID AS S from GEN_Tenant where TenantID=" + TenantID) + MMTExcelPath, "SupplierList");
                    //if (sFileName != "")
                    //{

                    //    string mslinepath = HttpContext.Current.Server.MapPath(sFileName);

                    //    DataTable dtExcelSupplier = CommonLogic.ImportDataFromExcel(mslinepath.Replace("\\mMaterialManagement", ""), Path.GetExtension(FUSupImportExcel.FileName));
                    if (FUSupImportExcel.FileName != "")
                    {
                        string path = string.Concat((Server.MapPath("~/ExcelData/" + FUSupImportExcel.FileName)));
                        FUSupImportExcel.PostedFile.SaveAs(path);
                        string ext = Path.GetExtension(path);
                        DataTable dt = CommonLogic.ImportDataFromExcel(path, ext);

                        string actualFileext = Path.GetExtension(actualFile);
                        DataTable actualFileDT = CommonLogic.ImportDataFromExcel(actualFile, actualFileext);
                        if (dt.Columns.Count != actualFileDT.Columns.Count)
                        {
                            resetError("Please upload proper excel", true);
                            return;

                        }

                        for (int i = 0; i < actualFileDT.Columns.Count; i++)
                        {
                            if (actualFileDT.Columns[i].ColumnName.Trim() != dt.Columns[i].ColumnName.Trim())
                            {
                                resetError("Please upload proper excel", true);
                                return;
                            }
                        }


                        if (dt.Rows.Count == 0)
                        {
                            resetError("Please fill the data in excel sheet", true);
                            return;

                        }
                        //else
                        //{
                        //    resetError("Please fill the data in excel sheet", true);
                        //    return;
                        //}

                        // Phone no. & Email Validation
                        foreach(DataRow dr in dt.Rows)
                        {
                            //if (!phoneNoValidate(dr[4].ToString()))
                            //{
                            //    resetError("Enter valid Phone No.", true);
                            //    return;
                            //}
                            if (!emailValidate(dr[7].ToString()))
                            {
                                resetError("Enter valid Email Address", true);
                                return;
                            }

                            //if (!phoneNoValidate(dr[10].ToString()))
                            //{
                            //    resetError("Enter valid PCP Phone No.", true);
                            //    return;
                            //}
                            if (!emailValidate(dr[11].ToString()))
                            {
                                resetError("Enter valid PCP Email Address", true);
                                return;
                            }
                        } 

                        if (FUSupImportExcel.FileName != "")
                        {
                            //string mslinepath = HttpContext.Current.Server.MapPath(path);
                            DataTable dtExcelSupplier = CommonLogic.ImportDataFromExcel(path, ext);

                            var dt1 = dtExcelSupplier.AsEnumerable().Select(x => x.Field<string>("SupplierName")).Distinct();

                            getConnection();

                            con.Open();


                            //creating object of SqlBulkCopy    
                            SqlBulkCopy objbulk = new SqlBulkCopy(con);
                            //assigning Destination table name    
                            objbulk.DestinationTableName = "MMT_SupplierFromExcel";
                            //Mapping Table column   
                            objbulk.ColumnMappings.Add(0, 0);
                            objbulk.ColumnMappings.Add(1, 1);
                            objbulk.ColumnMappings.Add(2, 2);
                            objbulk.ColumnMappings.Add(3, 3);
                            objbulk.ColumnMappings.Add(4, 4);
                            objbulk.ColumnMappings.Add(5, 5);
                            objbulk.ColumnMappings.Add(6, 6);
                            objbulk.ColumnMappings.Add(7, 7);
                            objbulk.ColumnMappings.Add(8, 8);
                            objbulk.ColumnMappings.Add(9, 9);
                            objbulk.ColumnMappings.Add(10, 10);
                            objbulk.ColumnMappings.Add(11, 11);

                            //inserting Datatable Records to DataBase    

                            objbulk.WriteToServer(dtExcelSupplier);
                            con.Close();

                        }

                        if (System.IO.File.Exists(completePath))
                        {
                            System.IO.File.Delete(completePath);
                        }
                        //  DB.ExecuteSQL("EXEC [dbo].[sp_MMT_UpsertMaterialMasterItemFromExcel] @CreatedBy=" + cp.UserID + ",@TenantID=" + cp.TenantID);


                        DataSet DS = DB.GetDS("[dbo].[sp_MMT_importSupplierNew] @UserID = " + cp.UserID + "", false);
                        //this.MMList_buildGridData(this.MMList_buildGridData(hfpageId.Value, drppagesize.SelectedValue), hfpageId.Value, drppagesize.SelectedValue);

                        if (DS.Tables[0].Rows.Count != 0)
                        {
                            downLoadExcel(DS);
                        }

                        this.MMList_buildGridData(this.MMList_buildGridData());
                        resetError("Successfully Uploaded", false);


                    }
                    else
                    {
                        resetError("Please select item master excel sheet", true);
                        return;

                    }
                }
                //string completePath = Server.MapPath("~/ExcelData/" + FUSupImportExcel.FileName);
                //if (System.IO.File.Exists(completePath))
                //{
                //    System.IO.File.Delete(completePath);
                //}
                //try
                //{
                //    if (FUSupImportExcel.HasFile)
                //    {
                //        string path = string.Concat((Server.MapPath("~/ExcelData/" + FUSupImportExcel.FileName)));
                //        FUSupImportExcel.PostedFile.SaveAs(path);
                //        string ext = Path.GetExtension(path);
                //        DataTable dt = CommonLogic.ImportDataFromExcel(path, ext);
                //        for (int i = 0; i < dt.Rows.Count; i++)
                //        {
                //             string Account = dt.Rows[i]["AccountCode"].ToString();
                //            string Tenant = dt.Rows[i]["TenantCode"].ToString();
                //            string SupplierName = dt.Rows[i]["SupplierName"].ToString();
                //            string SupplierCode = dt.Rows[i]["SupplierCode"].ToString();
                //            string Telephone1 = dt.Rows[i]["Telephone1"].ToString().Trim() != "" ? dt.Rows[i]["Telephone1"].ToString() : "Null";
                //            string Address = dt.Rows[i]["Address"].ToString().Trim() != "" ? dt.Rows[i]["Address"].ToString() : "Null";
                //            string country = dt.Rows[i]["Country"].ToString().Trim() != "" ? dt.Rows[i]["Country"].ToString() : "Null";
                //            string Currency = dt.Rows[i]["Address"].ToString().Trim() != "" ? dt.Rows[i]["Address"].ToString() : "Null";
                //            string Email = dt.Rows[i]["Email"].ToString().Trim() != "" ? dt.Rows[i]["Email"].ToString() : "Null";
                //            string ContactPersonName = dt.Rows[i]["ContactPerson Name"].ToString().Trim() != "" ? dt.Rows[i]["ContactPerson Name"].ToString() : "Null";
                //            string Title = dt.Rows[i]["Title"].ToString().Trim() != "" ? dt.Rows[i]["Title"].ToString() : "Null";
                //            string ContactNo = dt.Rows[i]["Address"].ToString().Trim() != "" ? dt.Rows[i]["Address"].ToString() : "Null";
                //            string ContactEmail = dt.Rows[i]["ContactEmail"].ToString().Trim() != "" ? dt.Rows[i]["ContactEmail"].ToString() : "Null";
                //        //  string   Account = "0";
                //            if (Account == "")
                //            {
                //                resetError("Please Select Account", true);
                //                return;
                //            }
                //            if (Tenant == "")
                //            {
                //                resetError("Please Select Tenant", true);
                //                return;
                //            }
                //            if (SupplierName == "")
                //            {
                //                resetError("Please Enter Supplier Name", true);
                //                return;
                //            }
                //            if (SupplierCode == "")
                //            {
                //                resetError("Please Enter Supplier Code", true);
                //                return;
                //            }
                //            if (Telephone1 == "")
                //            {
                //                resetError("Please Enter TelePhoneNo.1", true);
                //                return;
                //            }
                //            if (Telephone1 != "" && (Telephone1.Trim().Length < 10 || Telephone1.Trim().Length > 14))
                //            {
                //                resetError("Please Enter Valid TelePhoneNo.1", true);
                //                return;
                //            }

                //            if (Address == "")
                //            {
                //                resetError("Please Enter Address.", true);
                //                return;
                //            }
                //            if (country == "")
                //            {
                //                resetError("Please Enter Country", true);
                //                return;
                //            }
                //            if (Currency == "")
                //            {
                //                resetError("Please Enter Currency", true);
                //                return;
                //            }
                //            if (Email != "")
                //            {
                //                //string email = txtEmailAddress.Text;
                //                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                //                Match match = regex.Match(Email);
                //                if (!match.Success)
                //                {
                //                    resetError("Please Enter Valid Email", true);
                //                    return;
                //                }
                //            }
                //            if (ContactEmail != "")
                //            {
                //                //string email = txtEmailAddress.Text;
                //                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                //                Match match = regex.Match(ContactEmail);
                //                if (!match.Success)
                //                {
                //                    resetError("Please Enter Valid Email", true);
                //                    return;
                //                }
                //            }


                //            upsertSupplierDetails(Account, Tenant, SupplierName, SupplierCode, Telephone1, Email, Address,Currency, country, ContactPersonName, Title, ContactNo, ContactEmail);
                //            resetError("Excel Imported successfully", false);
                //            this.MMList_buildGridData(this.MMList_buildGridData());
                //        }


                //    }
                //}
                //catch (Exception ex)
                //{
                //    resetBlkLableError(ex.Message, true);
                //}
                else
                {
                    resetError("Please Upload Excel Sheet", true);
                }
            }
            catch (Exception ex)
            {
                FUSupImportExcel.Attributes.Clear();
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);

                if (ex.Message.Contains("IX_MMT_SupplierFromExcel"))
                {
                    resetError("Duplicate partnumber exists, Please check once and try again  : " + ex.Message.Substring(ex.Message.IndexOf("(") + 1).Replace(").\r\nThe statement has been terminated.", ""), true);
                    return;
                }

                resetError("Error while uploading Supplier. Error is " + ex.Message, true);

                return;

            }
        }


        private void downLoadExcel(DataSet ds)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                string FileName = "FailedItems";
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(ds.Tables[0]);

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".xlsx");
                    //Response.Redirect("MaterialMasterList.aspx", false);
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        // Response.WriteFile(Response.OutputStream);

                        FUSupImportExcel.Attributes.Clear();
                        Response.End();
                    }

                }
            }
            catch (Exception ex)
            {
                FUSupImportExcel.Attributes.Clear();
                this.MMList_buildGridData(this.MMList_buildGridData());
            }


        }

        private bool phoneNoValidate(string number)
        {
            foreach(Char ch in number)
            {
                if(ch<48 || ch>57)
                {
                    return false;
                }
            }
            if (number.Length != 10)
            {
                return false;
            }
            return true;
        }
        private bool emailValidate(string email)
        {
            Regex regex = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            bool isValid = regex.IsMatch(email);
            return isValid;
        }
    }
}

