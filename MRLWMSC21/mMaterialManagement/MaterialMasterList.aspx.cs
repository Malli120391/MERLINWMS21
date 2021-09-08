using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using MRLWMSC21Common;
using System.Threading;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Data.OleDb;
using System.Collections;
using ClosedXML.Excel;
using System.Web.UI.HtmlControls;
using System.Web.Services;


// Module Name : Material Management
// Usecase Ref.: Material master list_UC_003
// DevelopedBy : Naresh P
// Created On  : 01/10/2013 
// Modified On : 24/03/2015



namespace MRLWMSC21.mMaterialManagement


{
    public partial class MaterialMasterList : System.Web.UI.Page
    {
        public DataTable dt1 = null;
        private String SOCheck = "";
        private String POCheck = "";
        CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        private string selectMMList = "";
        private int TenantID = 0;
        private int CreatedBy = 0;
        private int RequestedBy = 0;
        private string TenantRootDir = "";
        private string MMTPath = "";
        private string MMTExcelPath = "";

        private int _TotalRowCount;
        OleDbConnection Econ;
        SqlConnection con;

        string constr, Query, sqlconn;

        // Set page theme
        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "MasterData";
        }


        // This method fires when page loading
        protected void Page_Load(object sender, EventArgs e)
        {
            string s = "there,";
            string[] words = s.Split(',');
            foreach (string word in words)
            {
                string wor = word;
            }
            cp = HttpContext.Current.User as CustomPrincipal;
            Page.Validate();

            TenantID = cp.TenantID;
            ViewState["TenantID"] = cp.TenantID;
            CreatedBy = cp.UserID;

            RequestedBy = cp.UserID;

            //Get Tenant Root Directory
            TenantRootDir = DB.GetSqlS("select DefaultValue AS S from SYS_SystemConfiguration SYS_C left join SYS_SysConfigKey SYS_K on SYS_K.SysConfigKeyID=SYS_C.SysConfigKeyID where SysConfigKey='TenantContentPath'");

            // Get MaterialMaster Path
            MMTPath = DB.GetSqlS(" select DefaultValue AS S from SYS_SystemConfiguration SYS_C left join SYS_SysConfigKey SYS_K on SYS_K.SysConfigKeyID=SYS_C.SysConfigKeyID where SysConfigKey='MaterialManagementPath'");

            MMTExcelPath = DB.GetSqlS(" select DefaultValue AS S from SYS_SystemConfiguration SYS_C left join SYS_SysConfigKey SYS_K on SYS_K.SysConfigKeyID=SYS_C.SysConfigKeyID where SysConfigKey='MaterialManagementImportExcelPath'");


            // Roles Allowed: 1,2,6,10,11,12,13,18
            // MMAdmin, Sales Coordinator, Purchase Coordinator, System Admin, Super Admin, Accounts Staff
            //if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("Item Master List"))))
            //{
            //    Response.Redirect("../Login.aspx?eid=6");
            //}

            if (!IsPostBack)
            {

                Page.Validate();

                DesignLogic.SetInnerPageSubHeading(this.Page, "Item Master List");

                ViewState["AtchAuotOpen"] = "false";

                //Get Tenant Root Directory
                TenantRootDir = DB.GetSqlS("select DefaultValue AS S from SYS_SystemConfiguration SYS_C left join SYS_SysConfigKey SYS_K on SYS_K.SysConfigKeyID=SYS_C.SysConfigKeyID where SysConfigKey='TenantContentPath'");

                // Get MaterialMaster Path
                MMTPath = DB.GetSqlS(" select DefaultValue AS S from SYS_SystemConfiguration SYS_C left join SYS_SysConfigKey SYS_K on SYS_K.SysConfigKeyID=SYS_C.SysConfigKeyID where SysConfigKey='MaterialManagementPath'");

                //CommonLogic.LoadDay(ddlMfgDay, "0");
                //CommonLogic.LoadMonth(ddlMfgMonth, "0");
                //CommonLogic.LoadYear(ddlMfgYear, 2000, DateTime.Now.Year, "0");

                //CommonLogic.LoadDay(ddlExpDay, "0");
                //CommonLogic.LoadMonth(ddlExpMonth, "0");
                //CommonLogic.LoadYear(ddlExpYear, 2005, 2025, "0");

                //LoadWarehousePrinter(ddlWarehousePrinter); // Load warehouse printers
                CommonLogic.LoadPrinters(ddlWarehousePrinter);
                LoadMMAdminApprovalList(ddlMMAdminApproved); // Load MMAdminApproved dropdown list
                CommonLogic.LoadLabelSizes(ddlLabelSize);
                // LoadDropDown(ddlMTypeID, "Select MTypeID, MType + ' - ' + Description as MTypeDesc from MMT_MType Where IsActive =1", "MTypeDesc", "MTypeID", "All");

                //selectMMList = "EXEC [dbo].[sp_MMT_MaterialMasterList]   @OEMPartNumber='', @SupplierID=0 , @Supplier='' , @MCode='' , @IsMMAdminApproved=" + ddlMMAdminApproved.SelectedValue + ",@MTypeID=" + ddlMTypeID.SelectedValue;

                hifTenant.Value = cp.TenantID.ToString();
                txtTenant.Enabled = CommonLogic.CheckSuperAdmin(txtTenant, cp, hifTenant);

                if (hifTenant.Value != "0")
                {
                    flupldImportExcel.Visible = false;
                    lnkflupldImportExcel.Visible = false;
                }

                if (cp.TenantID != 0)
                {
                    gvMMList.Columns[4].Visible = false;
                }

                string text = txtTenant.Text.Trim();
                if (text == "Search Tenant...")
                    text = "";

                //selectMMList = "EXEC [dbo].[sp_TPL_MaterialMasterList]   @OEMPartNumber='', @SupplierID=0 , @Supplier='' , @MCode='' , @IsMMAdminApproved=" + ddlMMAdminApproved.SelectedValue + ",@MTypeID=" + ddlMTypeID.SelectedValue + ",@TenantName=" + DB.SQuote(text) + ",@AccountID_New=" + cp.AccountID + ",@TenantID_New=" + cp.TenantID; 
                //Added By Shravya as MType is changed to autocomplete dropdown based on Tenant Selected
                selectMMList = "EXEC [dbo].[sp_TPL_MaterialMasterList]   @OEMPartNumber='', @SupplierID=0 , @Supplier='' , @MCode='' ,@Description='', @IsMMAdminApproved=" + ddlMMAdminApproved.SelectedValue + ",@MTypeID=0 ,@TenantName=" + DB.SQuote(text) + ",@AccountID_New=" + cp.AccountID + ",@TenantID_New=" + cp.TenantID;

                ViewState["MMListSQL"] = this.selectMMList;
                ViewState["MMListSort"] = "MaterialMasterID";
                ViewState["MMListSortOrder"] = "DESC";
                ViewState["IsSorting"] = "false";
                hfpageId.Value = "1";

                // generate grid data
                this.MMList_buildGridData(this.MMList_buildGridData(hfpageId.Value, drppagesize.SelectedValue), hfpageId.Value, drppagesize.SelectedValue);
                if (CommonLogic.QueryString("statid") == "success")
                {
                    resetError("Successfully Updated", false);
                }


            }



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


        // This method popup if any errors occured
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


        // This method fires when user clicks search button
        protected void lnkMMListSearch_Click(object sender, EventArgs e)
        {
            ViewState["IsSorting"] = "false";
            String vDesc = txtDescription.Text;
            String vSupplierString = txtSupplier.Text.Split('-')[0];
            String vMaterialCode = txtMMListItemCode.Text.Trim();
            String vOEMNumber = txtOEMSearch.Text.Trim();
            String vTenant = txtTenant.Text.Split('-')[0];
            String vMType = txtMType.Text.Split('-')[0];
            if (hifMTypeID.Value == "0" && vMType != "")
            {
                resetError("Invalid Material Type", true);
                return;
            }



            //if (vSupplierString == "Search Supplier...")
            //    vSupplierString = "";


            //if (vMaterialCode == "Search Part Number ...")
            //    vMaterialCode = "";

            //if (vOEMNumber == "Search OEM # ...")
            //    vOEMNumber = "";

            //if (vTenant == "Search Tenant...")
            //    vTenant = "";

            //if (vMType == "Search Material Type ...")
            //    vMType = "";

            //selectMMList = "EXEC [dbo].[sp_MMT_MaterialMasterList]   @OEMPartNumber=" + DB.SQuote(vOEMNumber) + ", @Supplier=" + DB.SQuote(vSupplierString) + ",@SupplierID=0,@MCode=" + DB.SQuote(vMaterialCode) + ",@IsMMAdminApproved=" + ddlMMAdminApproved.SelectedValue + ",@MTypeID=" + ddlMTypeID.SelectedValue;

            selectMMList = "EXEC [dbo].[sp_TPL_MaterialMasterList]   @OEMPartNumber=" + DB.SQuote(vOEMNumber) + ", @Supplier=" + DB.SQuote(vSupplierString) + ",@SupplierID=0,@MCode=" + DB.SQuote(vMaterialCode) + ",@Description=" + DB.SQuote(vDesc) + ",@IsMMAdminApproved=" + ddlMMAdminApproved.SelectedValue + ",@MTypeID=" + hifMTypeID.Value + " ,@TenantName=" + DB.SQuote(vTenant) + ",@AccountID_New=" + cp.AccountID + ",@TenantID_New=" + cp.TenantID;

            ViewState["MMListSQL"] = selectMMList;
            ViewState["MMListSort"] = "MCode";
            ViewState["MMListSortOrder"] = "ASC";

            // Generate grid based on user selection criteria
            this.MMList_buildGridData(this.MMList_buildGridData(hfpageId.Value, drppagesize.SelectedValue), hfpageId.Value, drppagesize.SelectedValue);


            //if (txtSupplier.Text.Trim() == "")
            //    txtSupplier.Text = "Search Supplier...";


            //if (txtMMListItemCode.Text.Trim() == "")
            //    txtMMListItemCode.Text = "Search Part # ...";

            //if (txtMType.Text.Trim() == "")
            //    txtMType.Text = "Search Material Type ...";
        }

        //This method  popups print dialog box
        public string GetPrintDialog(String MCode, string MCodeAlt, string OEMPartNo, string ItemDesc)
        {
            String PrintLink = "<a  style='text-decoration:none;' " + String.Format("href=\"javascript:openDialogAndBlock(\'Print Barcode Label\',\'{0}|{1}|{2}|{3}\')", MCode, MCodeAlt, OEMPartNo, ItemDesc.Replace("\"", "")) + ";\"> Print </a>";
            return PrintLink;
        }



        // This method displays the alternative MCodes and MCode
        public string DisplayMCodes(String MCode, string MCodeAlt, string MCodeAlt2)
        {
            String resString = "";

            if (MCode != "")
                //resString += MCode;

                if (MCodeAlt != "")
                    resString += "</br> Alt. Part# 1: " + MCodeAlt;

            if (MCodeAlt2 != "")
                resString += "</br> Alt. Part# 2:" + MCodeAlt2;

            return resString;

        }

        // This methos is used for check box list
        public string GetCheckValue(String IsChecked)
        {
            String resultValue = "";

            if (IsChecked != "")
            {
                if (Convert.ToBoolean(Convert.ToInt32(IsChecked)))
                    resultValue = "<img src='/Images/plain_checked.gif' border='0'/>";
            }

            return resultValue;
        }

        // This method return the boolean value
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

        // This method loads MMAdmin approved dropdown list
        public void LoadMMAdminApprovalList(DropDownList ddlMMAAList)
        {
            ddlMMAAList.Items.Clear();
            ddlMMAAList.Items.Add(new ListItem("Select All", "2"));
            ddlMMAAList.Items.Add(new ListItem("MMAdmin. Approved", "1"));
            ddlMMAAList.Items.Add(new ListItem("MMAdmin. Appr.Pending", "0"));
        }

        // This method loads warehouse printers
        public void LoadWarehousePrinter(DropDownList ddlWarehousePrinter)
        {
            CommonLogic.LoadPrinters(ddlWarehousePrinter);
        }


        #region -------------------------MMList Grid Data-----------------------------

        // This method returns MMList data
        protected DataTable MMList_buildGridData(String PageNumber, String PageSize)
        {
            ltSITRecordCount.Text = "";
            StringBuilder sql = new StringBuilder();
            sql.Append(ViewState["MMListSQL"].ToString());

            sql.Append(",@PageIndex=" + (ViewState["IsSorting"].ToString().Equals("true") ? "0" : PageNumber) + ", @PageSize=" + PageSize);
            DataSet ds = DB.GetDS(sql.ToString(), false);
            _TotalRowCount = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            ltSITRecordCount.Text = "[" + _TotalRowCount + "]";
            if (_TotalRowCount == 0)
            {
                //  resetError("No material is available", true);
                imgbtngvMMList.Visible = false;
                lblstringperpage.Visible = false;
                drppagesize.Visible = false;
            }
            else
            {
                imgbtngvMMList.Visible = true;
                lblstringperpage.Visible = true;
                drppagesize.Visible = true;
            }
            ds.Tables[0].Dispose();
            generatePager(_TotalRowCount, Convert.ToInt16(drppagesize.SelectedValue), Convert.ToInt16(hfpageId.Value));
            return ds.Tables[1];
        }

        // This method generate grid data
        protected void MMList_buildGridData(DataTable dt, String SPageNumber, String SPageSize)
        {
            dt1 = dt;
            int PageNumber = Convert.ToInt16(SPageNumber);
            int PageSize = Convert.ToInt16(SPageSize);
            if (ViewState["IsSorting"].ToString().Equals("true"))
            {
                dt.DefaultView.Sort = ViewState["SortString"].ToString() + " " + ViewState["SortingOrder"].ToString();
                if (PageSize * (PageNumber - 1) > _TotalRowCount)
                    PageNumber = (int)Math.Ceiling((decimal)_TotalRowCount / PageSize);
                gvMMList.DataSource = dt.DefaultView.ToTable().Rows.Cast<System.Data.DataRow>().Skip((PageNumber - 1) * PageSize).Take(PageSize).CopyToDataTable();
            }
            else
                gvMMList.DataSource = dt;

            gvMMList.DataBind();

            //for (int i = 0; i < gvMMList.Rows.Count; i++)
            //{

            //    String mid = ((Literal)gvMMList.Rows[i].FindControl("ltMMItemID")).Text.Trim();
            //    String Mcode = ((Literal)gvMMList.Rows[i].FindControl("ltMCode1")).Text.Trim();
            //    String TenantID = ((Literal)gvMMList.Rows[i].FindControl("ltTenantID")).Text.Trim();

            //    //String loacalfolder = TenantRootDir + DB.GetSqlS("select UniqueID AS S from GEN_Tenant where TenantID=" + TenantID) + MMTPath + mid + "/";
            //    String loacalfolder = DB.GetSqlS("select DefaultValue AS S from SYS_SystemConfiguration SYS_C left join SYS_SysConfigKey SYS_K on SYS_K.SysConfigKeyID=SYS_C.SysConfigKeyID where SysConfigKey='TenantContentPath'") + TenantID + MMTPath + mid;
            //    //String loacalfolder = DB.GetSqlS("select DefaultValue AS S from SYS_SystemConfiguration SYS_C left join SYS_SysConfigKey SYS_K on SYS_K.SysConfigKeyID=SYS_C.SysConfigKeyID where SysConfigKey='TenantContentPath'") + MMTPath + mid;
            //    String midSavepath = System.Web.HttpContext.Current.Server.MapPath("~\\" + loacalfolder); ;
            //    String sidpath = "";

            //    //IDataReader rsGetSupplierList = DB.GetRS("select mmsup.SupplierID,sup.SupplierName from MMT_MaterialMaster_Supplier mmsup join MMT_Supplier sup on sup.SupplierID=mmsup.SupplierID  where MaterialMasterID=" + mid + " and sup.isactive=1 and sup.isdeleted=0");
            //    IDataReader rsGetSupplierList = DB.GetRS("select mmsup.SupplierID,sup.SupplierName from MMT_MaterialMaster_Supplier mmsup join MMT_Supplier sup on sup.SupplierID=mmsup.SupplierID  where MaterialMasterID=" + mid + " and sup.isactive=1 and sup.isdeleted=0  and mmsup.IsDeleted=0");

            //    while (rsGetSupplierList.Read())
            //    {                        
            //        sidpath = midSavepath + "//" + DB.RSFieldInt(rsGetSupplierList, "SupplierID");
            //        if (Directory.Exists(sidpath))
            //        {
            //            string[] Attachmentlist = Directory.GetFiles(sidpath);
            //            LinkButton styleSheet = ((LinkButton)gvMMList.Rows[i].FindControl("viewattachment"));
            //            //styleSheet.ForeColor = System.Drawing.Color.Blue; 
            //            styleSheet.Attributes.Add("class", "btntxtclr");
            //        }                       
            //    }                
            //}

            dt.Dispose();
        }

        // This method sorts the grid data
        protected void gvMMList_Sorting(object sender, GridViewSortEventArgs e)
        {
            ViewState["MMListIsInsert"] = false;
            gvMMList.EditIndex = -1;
            ViewState["MMListSort"] = e.SortExpression.ToString();
            ViewState["MMListSortOrder"] = (ViewState["MMListSortOrder"].ToString() == "ASC" ? "DESC" : "ASC");

            this.MMList_buildGridData(this.MMList_buildGridData(hfpageId.Value, drppagesize.SelectedValue), hfpageId.Value, drppagesize.SelectedValue);
        }

        protected void gvMMList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //======================= Added By M.D.Prasad For View Only Condition ======================//
            string[] hh = cp.Roles;
            for (int i = 0; i < hh.Length; i++)
            {
                if (cp.UserTypeID == 3 && Convert.ToInt32(hh[i]) == 4)
                {
                    btnSampleTemplate.Visible = false;
                    flupldImportExcel.Visible = false;
                    lnkflupldImportExcel.Visible = false;
                    imgbtngvMMList.Visible = false;
                    lnkAddMaterial.Visible = false;
                }
            }
            //======================= Added By M.D.Prasad For View Only Condition ======================//
            //if (cp.TenantID != 0)
            //{
            //    if (e.Row.RowType == DataControlRowType.DataRow && ((e.Row.RowState == DataControlRowState.Normal) || (e.Row.RowState == DataControlRowState.Alternate)))
            //    {
            //        //DataRow row = ((DataRowView)e.Row.DataItem).Row;
            //        //String InvoiceNos = row["InvoiceIDs"].ToString();
            //        //Boolean Check = Convert.ToBoolean(row["IsKitParent"]);
            //        Literal mmid = (Literal)e.Row.FindControl("ltMMItemID");
            //        HtmlGenericControl msgDiv = (HtmlGenericControl)e.Row.FindControl("msgDiv");
            //        DataSet ds = DB.GetDS("SELECT ATT.MM_MST_Material_ID,ATT.MM_MST_Attribute_ID,ATT.MM_MST_AttributeLookup_ID,CNF.MM_CNF_IndustryMaterialAttribute_ID,dbo.UDF_ParseAndReturnLocaleString(CNF.UILabelText,'en') as UILabelText,IND.IndustryName,dbo.UDF_ParseAndReturnLocaleString(ATT.AttributeValue,'en') AS AttributeValue,dbo.UDF_ParseAndReturnLocaleString(LK.LookupText,'en') AS LookupText "
            //                              + " FROM MM_TRN_MaterialAttributes ATT JOIN MM_CNF_IndustryMaterialAttributes CNF ON  ATT.MM_MST_Attribute_ID = CNF.MM_MST_Attribute_ID AND ATT.isactive = 1 AND ATT.isdeleted = 0 " +
            //                                 " JOIN GEN_Industry IND ON CNF.GEN_MST_Industry_ID = IND.IndustryID AND IND.isactive = 1 AND IND.isdeleted = 0 AND CNF.isactive = 1 AND CNF.isdeleted = 0 LEFT JOIN MM_MST_AttributeLookup LK ON LK.MM_MST_Attribute_ID = ATT.MM_MST_Attribute_ID AND ATT.MM_MST_AttributeLookup_ID = LK.MM_MST_AttributeLookup_ID AND LK.isactive = 1 AND LK.isdeleted = 0 WHERE ATT.MM_MST_Material_ID=" + mmid.Text + "", false);
            //        if (ds.Tables[0].Rows.Count > 0)
            //        {
            //            string industry = ds.Tables[0].Rows[0]["IndustryName"].ToString();
            //            msgDiv.InnerHtml = "<div id='divPreferenceHeader' style='color:#4480e2;font-size:7.5px;font-weight:bold'><b>" + industry + "</b></div>";
            //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //            {

            //                if (ds.Tables[0].Rows[i]["MM_MST_AttributeLookup_ID"].ToString() != "0")
            //                {
            //                    //msgDiv.InnerHtml += "<div id='divPreferenceHeader1' style='color:#4480e2;font-size:7.5px'>" + ds.Tables[0].Rows[i]["UILabelText"].ToString() + "&nbsp;" + ds.Tables[0].Rows[i]["LookupText"].ToString() + "  </div>";
            //                    msgDiv.InnerHtml += "<div id='divPreferenceHeader1' style='color:#4480e2;font-size:7.5px'> &nbsp;" + ds.Tables[0].Rows[i]["LookupText"].ToString() + "  </div>";
            //                }
            //                else
            //                {
            //                    if (ds.Tables[0].Rows[i]["AttributeValue"].ToString() != "")
            //                    {
            //                        //msgDiv.InnerHtml += "<div id='divPreferenceHeader1' style='color:#4480e2;font-size:7.5px'>" + ds.Tables[0].Rows[i]["LookupText"].ToString() + " &nbsp;" + ds.Tables[0].Rows[i]["UILabelText"].ToString() + " &nbsp;" + ds.Tables[0].Rows[i]["AttributeValue"].ToString() + " </div>";
            //                        msgDiv.InnerHtml += "<div id='divPreferenceHeader1' style='color:#4480e2;font-size:7.5px'>" + ds.Tables[0].Rows[i]["LookupText"].ToString() + " |" + ds.Tables[0].Rows[i]["AttributeValue"].ToString() + " </div>";
            //                    }
            //                    else
            //                    {
            //                        msgDiv.InnerHtml += "<div id='divPreferenceHeader1' style='color:#4480e2;font-size:7.5px'></div>";

            //                    }
            //                }
            //            }
            //        }
            //        gvMMList.Columns[7].Visible = false;
            //        gvMMList.Columns[8].Visible = false;

            //        //======================= Added By M.D.Prasad For View Only Condition ======================//
            //        for (int i = 0; i < hh.Length; i++)
            //        {
            //            if (cp.UserTypeID == 3 && Convert.ToInt32(hh[i]) == 4)
            //            {
            //                gvMMList.Columns[7].Visible = false;
            //                gvMMList.Columns[8].Visible = false;
            //                gvMMList.Columns[9].Visible = false;
            //            }
            //        }
            //        //======================= Added By M.D.Prasad For View Only Condition ======================//
            //    }
            //}
            //else
            //{
            //    if (e.Row.RowType == DataControlRowType.DataRow )
            //    {
            //        //DataRow row = ((DataRowView)e.Row.DataItem).Row;
            //        //String InvoiceNos = row["InvoiceIDs"].ToString();
            //        //Boolean Check = Convert.ToBoolean(row["IsKitParent"]);


            //        StringBuilder sb = new StringBuilder();
            //        sb.Append("<div id ='divPreferenceHeader1' style ='color:#4480e2;font-size:7.5px'>");

            //           Literal mmid = (Literal)e.Row.FindControl("ltMMItemID");
            //        HtmlGenericControl msgDiv = (HtmlGenericControl)e.Row.FindControl("msgDiv");
            //        DataSet ds = DB.GetDS("SELECT ATT.MM_MST_Material_ID,ATT.MM_MST_Attribute_ID,ATT.MM_MST_AttributeLookup_ID,CNF.MM_CNF_IndustryMaterialAttribute_ID,dbo.UDF_ParseAndReturnLocaleString(CNF.UILabelText,'en') as UILabelText,IND.IndustryName,dbo.UDF_ParseAndReturnLocaleString(ATT.AttributeValue,'en') AS AttributeValue,dbo.UDF_ParseAndReturnLocaleString(LK.LookupText,'en') AS LookupText "
            //                              + " FROM MM_TRN_MaterialAttributes ATT JOIN MM_CNF_IndustryMaterialAttributes CNF ON  ATT.MM_MST_Attribute_ID = CNF.MM_MST_Attribute_ID AND ATT.isactive = 1 AND ATT.isdeleted = 0 " +
            //                                 " JOIN GEN_Industry IND ON CNF.GEN_MST_Industry_ID = IND.IndustryID AND IND.isactive = 1 AND IND.isdeleted = 0 AND CNF.isactive = 1 AND CNF.isdeleted = 0 LEFT JOIN MM_MST_AttributeLookup LK ON LK.MM_MST_Attribute_ID = ATT.MM_MST_Attribute_ID AND ATT.MM_MST_AttributeLookup_ID = LK.MM_MST_AttributeLookup_ID AND LK.isactive = 1 AND LK.isdeleted = 0 WHERE ATT.MM_MST_Material_ID=" + mmid.Text + "", false);
            //        if (ds.Tables[0].Rows.Count > 0)
            //        {
            //            string industry = ds.Tables[0].Rows[0]["IndustryName"].ToString();
            //            //msgDiv.InnerHtml = "<div id='divPreferenceHeader' style='color:#4480e2;font-size:7.5px;font-weight:bold'>" + industry + "</div>";
            //            sb.Append(industry);
            //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++) {

            //                //if (ds.Tables[0].Rows[i]["MM_MST_AttributeLookup_ID"].ToString() != "0")
            //                //{
            //                //    msgDiv.InnerHtml += "<div id='divPreferenceHeader1' style='color:#4480e2;font-size:7.5px'>" + ds.Tables[0].Rows[i]["UILabelText"].ToString() + "&nbsp; :" + ds.Tables[0].Rows[i]["LookupText"].ToString() + "  </div>";
            //                //}
            //                //else
            //                //{
            //                //    if (ds.Tables[0].Rows[i]["AttributeValue"].ToString() != "")
            //                //    {
            //                //        msgDiv.InnerHtml += "<div id='divPreferenceHeader1' style='color:#4480e2;font-size:7.5px'>" + ds.Tables[0].Rows[i]["LookupText"].ToString() + " &nbsp;" + ds.Tables[0].Rows[i]["UILabelText"].ToString() + " &nbsp; :" + ds.Tables[0].Rows[i]["AttributeValue"].ToString() + " </div>";
            //                //    }
            //                //    else
            //                //    {
            //                //        msgDiv.InnerHtml += "<div id='divPreferenceHeader1' style='color:#4480e2;font-size:7.5px'></div>";

            //                //    }
            //                //}
            //                if (ds.Tables[0].Rows[i]["MM_MST_AttributeLookup_ID"].ToString() != "0")
            //                {
            //                    //msgDiv.InnerHtml += "<div id='divPreferenceHeader1' style='color:#4480e2;font-size:7.5px'>" + ds.Tables[0].Rows[i]["UILabelText"].ToString() + "&nbsp;" + ds.Tables[0].Rows[i]["LookupText"].ToString() + "  </div>";
            //                    sb.Append( " | &nbsp;" + ds.Tables[0].Rows[i]["LookupText"].ToString());
            //                }
            //                else
            //                {
            //                    if (ds.Tables[0].Rows[i]["AttributeValue"].ToString() != "")
            //                    {
            //                        //msgDiv.InnerHtml += "<div id='divPreferenceHeader1' style='color:#4480e2;font-size:7.5px'>" + ds.Tables[0].Rows[i]["LookupText"].ToString() + " &nbsp;" + ds.Tables[0].Rows[i]["UILabelText"].ToString() + " &nbsp;" + ds.Tables[0].Rows[i]["AttributeValue"].ToString() + " </div>";
            //                        sb.Append(ds.Tables[0].Rows[i]["LookupText"].ToString() + " | " + ds.Tables[0].Rows[i]["AttributeValue"].ToString());
            //                    }
            //                    else
            //                    {
            //                       // sb.Append(ds.Tables[0].Rows[i]["LookupText"].ToString() + " | " + ds.Tables[0].Rows[i]["AttributeValue"].ToString());

            //                        //msgDiv.InnerHtml += "<div id='divPreferenceHeader1' style='color:#4480e2;font-size:7.5px'></div>";

            //                    }
            //                }
            //            }
            //            sb.Append("</ div > ");
            //            msgDiv.InnerHtml += sb.ToString();


            //        }

            //        //======================= Added By M.D.Prasad For View Only Condition ======================//
            //        for (int i = 0; i < hh.Length; i++)
            //        {
            //            if (cp.UserTypeID == 3 && Convert.ToInt32(hh[i]) == 4)
            //            {
            //                gvMMList.Columns[7].Visible = false;
            //                gvMMList.Columns[8].Visible = false;
            //                gvMMList.Columns[9].Visible = false;
            //            }
            //        }
            //        //======================= Added By M.D.Prasad For View Only Condition ======================//
            //    }
            //}
        }

        protected void gvMMList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["MMListIsInsert"] = false;
            gvMMList.PageIndex = e.NewPageIndex;
            gvMMList.EditIndex = -1;
            this.MMList_buildGridData(this.MMList_buildGridData(hfpageId.Value, drppagesize.SelectedValue), hfpageId.Value, drppagesize.SelectedValue);
        }

        protected void btnShipInTranEE_Click(object sender, EventArgs e)
        { }

        // This method updates the checked list  grid data
        protected void lnkUpdateIsActive_Click(object sender, EventArgs e)
        {
            string gvIDs = "";
            string gvUnChkIDs = "";
            bool chkBox = false;

            //'Navigate through each row in the GridView for checkbox items
            foreach (GridViewRow gv in gvMMList.Rows)
            {

                // Check if material is configured in PO/SO
                //SOCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckSO] @MaterialMasterID=" + ((Literal)gv.FindControl("ltMMItemID")).Text.ToString() + ",@TenantID=" + TenantID);
                //POCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckPO] @MaterialMasterID=" + ((Literal)gv.FindControl("ltMMItemID")).Text.ToString() + ",@TenantID=" + TenantID);

                SOCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckSO] @MaterialMasterID=" + ((Literal)gv.FindControl("ltMMItemID")).Text.ToString());
                POCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckPO] @MaterialMasterID=" + ((Literal)gv.FindControl("ltMMItemID")).Text.ToString());

                // If material is configured in PO/SO user not allowed to modify any details
                if (SOCheck != "" || POCheck != "")
                {
                    chkBox = false;
                    resetError("cannot change as this material is configured in" + " WO Numbers:" + SOCheck + " " + CommonLogic.IIF(POCheck != "", " PO Numbers :", "") + POCheck, true);

                    this.MMList_buildGridData(this.MMList_buildGridData(hfpageId.Value, drppagesize.SelectedValue), hfpageId.Value, drppagesize.SelectedValue);
                    return;

                }
                else
                {

                    CheckBox isApprovedChkBxItem = (CheckBox)gv.FindControl("chkIsActive");
                    if (isApprovedChkBxItem.Checked)
                    {
                        chkBox = true;
                        // Concatenate GridView items with comma for SQL Delete
                        gvIDs += ((Literal)gv.FindControl("ltMMItemID")).Text.ToString() + ",";

                    }
                    else
                    {
                        gvUnChkIDs += ((Literal)gv.FindControl("ltMMItemID")).Text.ToString() + ",";
                    }
                }
            }

            // Execute SQL Query only if checkboxes are checked to avoid any error with initial null string
            try
            {
                string updateSQL = "";
                if (chkBox)
                {

                    updateSQL = "Update MMT_MaterialMaster SET IsActive=1 WHERE MaterialMasterID IN (" + gvIDs.Substring(0, gvIDs.LastIndexOf(",")) + ")";
                    DB.ExecuteSQL(updateSQL);
                    updateSQL = "Update MMT_MaterialMaster SET IsActive=0 WHERE MaterialMasterID IN (" + gvUnChkIDs.Substring(0, gvUnChkIDs.LastIndexOf(",")) + ")";
                    DB.ExecuteSQL(updateSQL);
                }
                else
                {
                    updateSQL = "Update MMT_MaterialMaster SET IsActive=0 WHERE MaterialMasterID IN (" + gvUnChkIDs.Substring(0, gvUnChkIDs.LastIndexOf(",")) + ")";
                    DB.ExecuteSQL(updateSQL);
                }
                resetError("Successfully Updated", false);

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error Updating", true);

            }

            //this.MMList_buildGridData(this.MMList_buildGridData());
            this.MMList_buildGridData(this.MMList_buildGridData(hfpageId.Value, drppagesize.SelectedValue), hfpageId.Value, drppagesize.SelectedValue);
        }

        // This method updates the IsApproved checked list  grid data
        protected void lnkUpdateIsApproved_Click(object sender, EventArgs e)
        {

            string gvIDs = "";
            string gvUnChkIDs = "";
            bool chkBox = false;

            //'Navigate through each row in the GridView for checkbox items
            foreach (GridViewRow gv in gvMMList.Rows)
            {
                // Check if material is configured in PO/SO

                //SOCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckSO] @MaterialMasterID=" + ((Literal)gv.FindControl("ltMMItemID")).Text.ToString() + ",@TenantID=" + TenantID);
                //POCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckPO] @MaterialMasterID=" + ((Literal)gv.FindControl("ltMMItemID")).Text.ToString() + ",@TenantID=" + TenantID);

                SOCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckSO] @MaterialMasterID=" + ((Literal)gv.FindControl("ltMMItemID")).Text.ToString());
                POCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckPO] @MaterialMasterID=" + ((Literal)gv.FindControl("ltMMItemID")).Text.ToString());

                // If material is configured in PO/SO user not allowed to modify any details
                if (SOCheck != "" || POCheck != "")
                {
                    chkBox = false;
                    resetError("Cannot change , as  this material is configured in" + " WO Numbers:" + SOCheck + " " + CommonLogic.IIF(POCheck != "", " PO Numbers :", "") + POCheck, true);

                    this.MMList_buildGridData(this.MMList_buildGridData(hfpageId.Value, drppagesize.SelectedValue), hfpageId.Value, drppagesize.SelectedValue);
                    return;

                }
                else
                {

                    CheckBox isApprovedChkBxItem = (CheckBox)gv.FindControl("chkIsApproved");
                    if (isApprovedChkBxItem.Checked)
                    {
                        chkBox = true;
                        // Concatenate GridView items with comma for SQL Delete
                        gvIDs += ((Literal)gv.FindControl("ltMMItemID")).Text.ToString() + ",";

                    }
                    else
                    {
                        gvUnChkIDs += ((Literal)gv.FindControl("ltMMItemID")).Text.ToString() + ",";
                    }
                }
            }

            // Execute SQL Query only if checkboxes are checked to avoid any error with initial null string
            try
            {
                string updateSQL = "";
                if (chkBox)
                {
                    updateSQL = "Update MMT_MaterialMaster SET IsAproved=1 WHERE MaterialMasterID IN (" + gvIDs.Substring(0, gvIDs.LastIndexOf(",")) + ")";
                    DB.ExecuteSQL(updateSQL);
                    updateSQL = "Update MMT_MaterialMaster SET IsAproved=0 WHERE MaterialMasterID IN (" + gvUnChkIDs.Substring(0, gvUnChkIDs.LastIndexOf(",")) + ")";
                    DB.ExecuteSQL(updateSQL);
                }
                else
                {
                    updateSQL = "Update MMT_MaterialMaster SET IsAproved=0 WHERE MaterialMasterID IN (" + gvUnChkIDs.Substring(0, gvUnChkIDs.LastIndexOf(",")) + ")";
                    DB.ExecuteSQL(updateSQL);
                }

                resetError("Successfully Updated", false);

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while Updating", true);

            }

            this.MMList_buildGridData(this.MMList_buildGridData(hfpageId.Value, drppagesize.SelectedValue), hfpageId.Value, drppagesize.SelectedValue);

        }

        #endregion -------------------------MMList Grid Data-----------------------------


        #region ----------------Print Individual Label----------------------------------

        private void ThermalLabelWorker(object thisMLabel2)
        {
            MRLWMSC21Common.TracklineMLabel thisMLabel = (MRLWMSC21Common.TracklineMLabel)thisMLabel2;

            String vresult = "";

            //if (rdlLabelSize.SelectedValue.ToLower() == "big")
            //    CommonLogic.SendPrintJob_Big_7p6x5(thisMLabel.MCode, thisMLabel.AltMCode, thisMLabel.OEMPartNo, thisMLabel.Description, thisMLabel.BatchNo, thisMLabel.SerialNo, thisMLabel.KitPlannerID.ToString(), thisMLabel.KitChildrenCount, thisMLabel.ParentMCode, thisMLabel.InvQty, thisMLabel.MfgDate, thisMLabel.ExpDate, thisMLabel.PrinterType, thisMLabel.PrinterIP, thisMLabel.StrRefNo, thisMLabel.OBDNumber, thisMLabel.KitCode, thisMLabel.ReqNo, thisMLabel.IsBoxLabelReq, false,thisMLabel.PrintQty, out vresult);
            //else
            //    CommonLogic.SendPrintJob_Small_5x2p5(thisMLabel.MCode, thisMLabel.AltMCode, thisMLabel.Description, thisMLabel.BatchNo, thisMLabel.SerialNo, thisMLabel.KitPlannerID.ToString(), thisMLabel.KitChildrenCount, thisMLabel.ParentMCode, thisMLabel.InvQty, thisMLabel.MfgDate, thisMLabel.ExpDate, thisMLabel.PrinterType, thisMLabel.PrinterIP, thisMLabel.StrRefNo, thisMLabel.ReqNo, thisMLabel.IsBoxLabelReq, out vresult);

            //thisMLabel.Result = vresult;
        }

        // This method fires when user clicks print button
        protected void btnPrintLabel_Click(object sender, EventArgs e)
        {
            if (txtMCode.Text == "")
            {
                resetError("Please Enter Part Number", true);
                return;
            }
            if (txtItemDesc.Text == "")
            {
                resetError("Please Enter Item Description", true);
                return;
            }
            if (txtQuantity.Text == "0" || txtQuantity.Text == "")
            {
                resetError("Please Enter valid Qty", true);
                return;
            }
            if (ddlWarehousePrinter.SelectedValue == "0")
            {
                resetError("Please select  printer", true);
                return;
            }


            if (ddlLabelSize.SelectedValue == "0")
            {
                resetError("Please select Label", true);
                return;
            }
            if (txtQuantity.Text != "" && txtMCode.Text.Trim() != "" && txtItemDesc.Text != "")
            {

                string vExpDate = string.Empty;
                string vMfgDate = string.Empty;


                //vExpDate = ddlExpDay.SelectedItem.Text.PadLeft(2, '0') + "-" + ddlExpMonth.SelectedItem.Text + "-" + ddlExpYear.SelectedItem.Text;

                //vMfgDate = ddlMfgDay.SelectedItem.Text.PadLeft(2, '0') + "-" + ddlMfgMonth.SelectedItem.Text + "-" + ddlMfgYear.SelectedItem.Text;

                if (vExpDate.IndexOf("DD") != -1 || vExpDate.IndexOf("MMM") != -1 || vExpDate.IndexOf("YYYY") != -1)
                {
                    vExpDate = string.Empty;
                }

                if (vMfgDate.IndexOf("DD") != -1 || vMfgDate.IndexOf("MMM") != -1 || vMfgDate.IndexOf("YYYY") != -1)
                {
                    vMfgDate = string.Empty;
                }






                // btnPrintLabel.Enabled = false;
                MRLWMSC21Common.TracklineMLabel thisMLabel = new MRLWMSC21Common.TracklineMLabel();
                string ddl = ddlLabelSize.SelectedValue;

                string length = "";
                string width = "";
                string LabelType = "";
                string query = "select * from TPL_Tenant_BarcodeType where IsActive=1 and IsDeleted=0 and TenantBarcodeTypeID=" + ddl;
                DataSet DS = DB.GetDS(query, false);
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    length = row["Length"].ToString();
                    width = row["Width"].ToString();
                    LabelType = row["LabelType"].ToString();
                }
                thisMLabel.LabelType = LabelType;

                thisMLabel.MCode = txtMCode.Text.Trim();
                thisMLabel.AltMCode = "";//txtAltMCode.Text.Trim();  
                thisMLabel.OEMPartNo = "";//txtOEMNo.Text.Trim();
                thisMLabel.Description = txtItemDesc.Text.Trim();
                thisMLabel.BatchNo = txtBatchNo.Text.Trim();
                thisMLabel.SerialNo = txtSerialNo.Text.Trim();
                thisMLabel.KitPlannerID = 0;
                thisMLabel.KitChildrenCount = CommonLogic.GetChildrenCount(thisMLabel.KitPlannerID.ToString());
                thisMLabel.ParentMCode = CommonLogic.GetKitParentCode(thisMLabel.KitPlannerID.ToString());
                thisMLabel.PrintQty = txtQuantity.Text.Trim();
                thisMLabel.MfgDate = vMfgDate;
                thisMLabel.ExpDate = vExpDate;
                thisMLabel.PrinterType = "IP";
                thisMLabel.OBDNumber = "";
                thisMLabel.KitCode = "";
                if (chkNeedBoxLabel.Checked == true)
                {
                    thisMLabel.IsBoxLabelReq = true;
                }
                //thisMLabel.PrinterIP = ddlWarehousePrinter.SelectedValue.Trim();

                thisMLabel.PrinterIP = ddlWarehousePrinter.SelectedValue;
                thisMLabel.IsBoxLabelReq = chkNeedBoxLabel.Checked;
                thisMLabel.Length = length;
                thisMLabel.Width = width;
                int dpi = 0;
                dpi = 203;//CommonLogic.GETDPI(thisMLabel.PrinterIP);
                thisMLabel.Dpi = dpi;

                thisMLabel.StrRefNo = txtStrRefNo.Text.Trim();
                thisMLabel.ReqNo = txtMRP.Text.Trim();
                thisMLabel.Mrp = txtMRP.Text.Trim();
                //For Printing Label Using ZPL Added by Prasanna 
                MRLWMSC21Common.PrintCommon.CommonPrint print = new MRLWMSC21Common.PrintCommon.CommonPrint();

                print.PrintBarcodeLabel(thisMLabel);
                //Commented ON 01-JAN-2019 BY Prasad //
                /*string ZPL = "";
                ZPL = print.PrintBarcodeLabel(thisMLabel);
                bool z = ZPL.Contains("\r\n");
                if (z == true)
                {
                    ZPL = ZPL.Replace("\r\n", "XYZ786XYZ");
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "PrintRTR_ZPL('" + ZPL + "');", true);*/
                Clearfields();

                //print.PrintLable(thisMLabel);
                // CommonLogic.PrintLabel(thisMLabel);



                //Commented  by Prasanna on 21st august 2017
                //Thread worker = new Thread(this.ThermalLabelWorker);
                //Thread worker = new Thread(new ParameterizedThreadStart(this.ThermalLabelWorker));
                //worker.SetApartmentState(ApartmentState.STA);
                //worker.Name = "ThermalLabelWorker";
                //worker.Start(thisMLabel);
                //worker.Join();

                ////CommonLogic.SendPrintJob(DB.RSField(rsLineItem, "MCode"), DB.RSField(rsLineItem, "AlternativeMCode"), DB.RSField(rsLineItem, "MDescription"), DB.RSField(rsLineItem, "BatchNo"),"",DB.RSFieldInt(rsLineItem, "KitPlannerID").ToString(), Convert.ToInt32(DB.RSFieldDecimal(rsLineItem, "InvoiceQuantity")) + 1, DateTime.MinValue, DB.RSFieldDateTime(rsLineItem, "ExpiryDate"), "IP", ddlNetworkPrinter.SelectedValue.Trim(), false, out result);
                //if (thisMLabel.Result != "Success")
                //{
                //    resetError(thisMLabel.Result, false);
                //    //resetError("Error printing. Please contact Trackline admin.", false);
                //}
                //else
                //{
                //    resetError("Successfully printed selected items", false);
                //}

                //String result = "";
                //CommonLogic.SendPrintJob(txtMCode.Text.Trim(), txtAltMCode.Text.Trim(), txtItemDesc.Text.Trim(), txtBatchNo.Text.Trim(), "", "", Convert.ToInt32(txtQuantity.Text.Trim()), vMfgDate, vExpDate, rdlPrinterType.SelectedValue, ddlWarehousePrinter.SelectedValue, false, out result);

            }
            else
            {
                lblPrintStatus.Text = "Please enter mandatory fields";
                return;
            }

            HideEditCustomer();
            RegisterStartupScript("jsUnblockDialog", "unblockDialog();");
            resetError("Successfully printed selected items", false);

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Clearfields();
        }
        public void Clearfields()
        {
            //    ddlMfgDay.SelectedIndex = 0;
            //    ddlMfgMonth.SelectedIndex = 0;
            //    ddlMfgYear.SelectedIndex = 0;
            //    ddlExpDay.SelectedIndex = 0;
            //    ddlExpMonth.SelectedIndex = 0;
            //    ddlExpYear.SelectedIndex = 0;
            txtBatchNo.Text = "";
            txtSerialNo.Text = "";
            txtStrRefNo.Text = "";
            txtMRP.Text = "";
            txtQuantity.Text = "";
            ddlWarehousePrinter.SelectedIndex = 0;
            ddlLabelSize.SelectedIndex = 0;
        }

        private void RegisterStartupScript(string key, string script)
        {
            ScriptManager.RegisterStartupScript(phrJsRunner, phrJsRunner.GetType(), key, script, true);
        }

        private void HideEditCustomer()
        {
            RegisterStartupScript("jsblockDialg", "blockDialog();");
            ClearEditCustomerForm();
            RegisterStartupScript("jsCloseDialg", "closeDialog();");    //Close dialog on client side
        }

        private void ClearEditCustomerForm()
        {
            //Empty out text boxes
            var textBoxes = new List<Control>();
            FindControlsOfType(this.phrUpdateReason, typeof(TextBox), textBoxes);

            foreach (TextBox textBox in textBoxes)
                textBox.Text = "";

            //Clear validators
            var validators = new List<Control>();
            FindControlsOfType(this.phrUpdateReason, typeof(BaseValidator), validators);

            foreach (BaseValidator validator in validators)
                validator.IsValid = true;
        }

        static public void FindControlsOfType(Control root, Type controlType, List<Control> list)
        {
            if (root.GetType() == controlType || root.GetType().IsSubclassOf(controlType))
            {
                list.Add(root);
            }
            //Skip input controls
            if (!root.HasControls())
                return;

            foreach (Control control in root.Controls)
            {
                FindControlsOfType(control, controlType, list);
            }
        }

        #endregion ----------------Print Individual Label----------------------------------

        // This method fires when user clicks export to excel button
        //protected void imgbtngvMMList_Click(object sender, ImageClickEventArgs e)
        protected void imgbtngvMMList_Click(object sender, EventArgs e)
        {
            String[] hiddencolumns = { "Print", "Edit", "Copy" };
            gvMMList.AllowPaging = false;
            //MMList_buildGridData(MMList_buildGridData());
            this.MMList_buildGridData(this.MMList_buildGridData("0", drppagesize.SelectedValue), hfpageId.Value, drppagesize.SelectedValue);
            CommonLogic.ExporttoExcel(gvMMList, "ItemMasterList", hiddencolumns);
        }


        // This is For Export tot Excel rendering Control
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }


        #region -------------- Attachments ----------------

        // Load supplier attachments based on material masterid
        public string GetAttachmentsDialog(String MCode, String mid)
        {
            String PrintLink = "";

            if (mid != null && mid != "")
            {
                PrintLink = "<a style=\"text-decoration:none;\" href=\"javascript:openDialog1(\'" + MCode + "\')\"> " + MCode + " </a>";
                //String loacalfolder = TenantRootDir + DB.GetSqlS("select UniqueID AS S from GEN_Tenant where TenantID=" + TenantID) + MMTPath + mid + "/";
                //String loacalfolder = TenantRootDir + DB.GetSqlS("select UniqueID AS S from TPL_Tenant where TenantID=" + TenantID) + MMTPath + mid + "/";
                //String midSavepath = System.Web.HttpContext.Current.Server.MapPath("~/") + loacalfolder;
                String loacalfolder = DB.GetSqlS("select DefaultValue AS S from SYS_SystemConfiguration SYS_C left join SYS_SysConfigKey SYS_K on SYS_K.SysConfigKeyID=SYS_C.SysConfigKeyID where SysConfigKey='TenantContentPath'") + hifTenant.Value + MMTPath + mid;
                String midSavepath = System.Web.HttpContext.Current.Server.MapPath("~\\" + loacalfolder); ;
                String sidpath = "";

                trvmaterialattachment.Nodes[0].ChildNodes.Clear();

                TreeNode trnInternalOrder;
                TreeNode trnsubInternalOrder;
                trvmaterialattachment.Nodes[0].Text = txtMCode.Text;
                //DB.GetSqlS("select Mcode as S from MMT_MaterialMaster where MaterialMasterID = " + mid + " and isactive=1 and isdeleted=0");
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
                            trnsubInternalOrder.NavigateUrl = Attachmentname.EndsWith(".pdf") ? String.Format("DisplayPDF.aspx?sid={0}&mid={1}&filename={2}&tid={3}", DB.RSFieldInt(rsGetSupplierList, "SupplierID"), mid, Attachmentname, hifTenant.Value) : "../" + loacalfolder + "/" + DB.RSFieldInt(rsGetSupplierList, "SupplierID") + "/" + Attachmentname;
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

                /*if (Directory.Exists(midSavepath))
                {
                    string[] supplieridlist = Directory.GetDirectories(midSavepath);
                    
                    String suppliername = "";
                    foreach (string supplierid in supplieridlist)
                    {
                        suppliername = new DirectoryInfo(supplierid).Name;
                        suppliername = DB.GetSqlS("select distinct(suppliername) as S from MMT_Supplier ms join MMT_MaterialMaster_Supplier mms on ms.SupplierID=mms.SupplierID where ms.IsActive=1 and ms.IsDeleted=0 and mS.SupplierID=" + suppliername);
                        trnInternalOrder = new TreeNode(suppliername);
                        trvmid.Nodes[0].ChildNodes.Add(trnInternalOrder);
                        string[] Attachmentlist = Directory.GetFiles(supplierid);
                        string attachmentfilename = "";
                        foreach (string attachmentfile in Attachmentlist)
                        {
                            attachmentfilename = Path.GetFileName(attachmentfile);
                            trnsubInternalOrder = new TreeNode(attachmentfilename);
                            
                        }


                        
                    }
                    
                }
                else
                {
                    lblfileslist.Text = "";
                }*/
            }
            else
                PrintLink = "";



            return PrintLink;
        }

        protected void middailogbox()
        {


        }


        #endregion  -------------------- Attachments -------------


        protected void gvMMList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            if (e.CommandName == "viewpics")
            {
                ViewState["AtchAuotOpen"] = "true";
                String[] parms = e.CommandArgument.ToString().Split(',');
                String mid = parms[0];
                String Mcode = parms[1];
                String TenantID = parms[2];

                //String loacalfolder = TenantRootDir + DB.GetSqlS("select UniqueID AS S from GEN_Tenant where TenantID=" + TenantID) + MMTPath + mid + "/";
                String loacalfolder = DB.GetSqlS("select DefaultValue AS S from SYS_SystemConfiguration SYS_C left join SYS_SysConfigKey SYS_K on SYS_K.SysConfigKeyID=SYS_C.SysConfigKeyID where SysConfigKey='TenantContentPath'") + TenantID + MMTPath + mid;
                //String loacalfolder = DB.GetSqlS("select DefaultValue AS S from SYS_SystemConfiguration SYS_C left join SYS_SysConfigKey SYS_K on SYS_K.SysConfigKeyID=SYS_C.SysConfigKeyID where SysConfigKey='TenantContentPath'") + MMTPath + mid;
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
                            trnsubInternalOrder.NavigateUrl = Attachmentname.EndsWith(".pdf") ? String.Format("DisplayPDF.aspx?sid={0}&mid={1}&filename={2}&tid={3}", DB.RSFieldInt(rsGetSupplierList, "SupplierID"), mid, Attachmentname, TenantID) : "../" + loacalfolder + "/" + DB.RSFieldInt(rsGetSupplierList, "SupplierID") + "/" + Attachmentname; trnsubInternalOrder.Expanded = false;
                            trnInternalOrder.ChildNodes.Add(trnsubInternalOrder);

                        }
                    }
                    else
                    {
                        //Literal styleSheet = (Literal)this.Master.FindControl("ltMCode1");
                        trnsubInternalOrder = new TreeNode();
                        trnsubInternalOrder.Expanded = false;
                        trnsubInternalOrder.Text = "Empty";
                        trnInternalOrder.ChildNodes.Add(trnsubInternalOrder);
                    }
                    trvmaterialattachment.Nodes[0].ChildNodes.Add(trnInternalOrder);
                }

                //  ScriptManager.RegisterStartupScript(phrJsRunner, phrJsRunner.GetType(), "unblockAycDialog", "unblockAycDialog()", true);
                RegisterStartupScript("jsUnblockDialog", "closeProcessing();");
                //RegisterStartupScript("jsUnblockDialog", "OpenAttachment(\"" + Mcode + "  Attachmens\");");

            }
        }

        // Test mail purpose
        protected void lnkMail_Click(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            MailMessage mail = new MailMessage();
            mail.Subject = "Test Mail from Falcon 22 ";
            mail.To.Add("Ronnie.Mathew@rosselltechsys.com");
            mail.To.Add("sureshy@inventrax.com");


            mail.Body = "This is a test mail . Pls Ignore";
            try
            {
                AppLogic.SendMailInWebMail(mail);
                resetError("success", false);
            }
            catch (Exception ex)
            {
                //resetError(ex.ToString(),true); 
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);

            }


            //AppLogic.SendMail("Test Mail from Falcon", "This is a test mail . Pls Ignore", true);



        }

        // This method fires when attachment dialog box is closed
        protected void lnkattchemntclose_Click(object sender, EventArgs e)
        {
            //ViewState["AtchAuotOpen"] = "false";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "closeAttachment();", true);

        }

        protected void drppagesize_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.MMList_buildGridData(this.MMList_buildGridData(hfpageId.Value, drppagesize.SelectedValue), hfpageId.Value, drppagesize.SelectedValue);
        }


        protected void dlPagerupper_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "PageNo")
            {
                hfpageId.Value = e.CommandArgument.ToString();
                this.MMList_buildGridData(this.MMList_buildGridData(hfpageId.Value, drppagesize.SelectedValue), hfpageId.Value, drppagesize.SelectedValue);
            }

        }

        // This method generates pager row
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
                pageLinkContainer.Add(new ListItem("&lt;&lt;", "01", currentPage != 1));
            for (int i = startPageLink; i <= lastPageLink; i++)
            {
                pageLinkContainer.Add(new ListItem(i.ToString("00"), i.ToString(), currentPage != i));
            }
            if (lastPageLink != totalPageCount)
                pageLinkContainer.Add(new ListItem("&gt;&gt;", totalPageCount.ToString("00"), currentPage != totalPageCount));

            dlPagerupper.DataSource = pageLinkContainer;
            dlPagerupper.DataBind();
            dlPager.DataSource = pageLinkContainer;
            dlPager.DataBind();
        }


        protected void btnSampleTemplate_Click(object sender, EventArgs e)
        {
            if ((hifTenant.Value != "" || hifTenant.Value != String.Empty) && (txtTenant.Text != string.Empty || txtTenant.Text != ""))
            {


                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                #region existing template
                dt.Columns.Add("PartNumber " + Environment.NewLine + " (Max Length - 20)", typeof(string));
                dt.Columns.Add("OEMPartNumber", typeof(string));
                dt.Columns.Add("MaterialType " + Environment.NewLine + "(Max Length - 5)", typeof(string));
                dt.Columns.Add("ProductCategory", typeof(string));
                dt.Columns.Add("StorageCondition " + Environment.NewLine + "(Max Length - 5)", typeof(string));
                dt.Columns.Add("ItemDescriptionShort" + Environment.NewLine + "(Max Length - 50)", typeof(string));
                dt.Columns.Add("ItemDescriptionLong", typeof(string));
                dt.Columns.Add("BUoM " + Environment.NewLine + "(Max Length - 6)", typeof(string));
                dt.Columns.Add("BUoM_Quantity" + Environment.NewLine + "(Max Length - 20)", typeof(decimal));
                dt.Columns.Add("MUoM " + Environment.NewLine + "(Max Length - 20)", typeof(string));
                dt.Columns.Add("MUoM_Quantity " + Environment.NewLine + "(Max Length - 20)", typeof(decimal));
                dt.Columns.Add("SupplierCode " + Environment.NewLine + "(Max Length - 10)", typeof(string));
                dt.Columns.Add("TenantCode " + Environment.NewLine + "(Max Length - 10)", typeof(string));
                dt.Columns.Add("Mlength (cm)", typeof(decimal));
                dt.Columns.Add("Mwidth (cm) ", typeof(decimal));
                dt.Columns.Add("Mheight (cm)", typeof(decimal));
                dt.Columns.Add("Mweight (gm)", typeof(decimal));
                dt.Columns.Add("Material GroupCode " + Environment.NewLine + "(Max Length - 12)", typeof(string));
                dt.Columns.Add("CapacityPerBin", typeof(string));
                dt.Columns.Add("TenantName", typeof(string));
                dt.Columns.Add("SupplierName", typeof(string));
                dt.Columns.Add("MCodeAlternative1", typeof(string));
                dt.Columns.Add("MCodeAlternative2", typeof(string));
                #endregion existing template
                ds.Tables.Add(dt.Copy());

                DataSet dsnew = DB.GetDS("EXEC GET_DYNAMIC_MSPS @TenantID=" + hifTenant.Value, false);

                for (int i = 0; i < dsnew.Tables[0].Rows.Count; i++)
                {
                    ds.Tables[0].Columns.Add(dsnew.Tables[0].Rows[i][0].ToString(), dsnew.Tables[0].Columns[0].GetType());

                }

                downLoadExcel(ds, "MMT_ItemMasterFromExcel", 1);
            }
            else
            {

                resetError("Please select tenant", true);
            }
        }



        #region -------------- Import Excel -----------------------------------

        protected void lnkflupldImportExcel_Click(object sender, EventArgs e)
        {

            if ((hifTenant.Value != "" || hifTenant.Value != String.Empty) && (txtTenant.Text != string.Empty || txtTenant.Text != ""))
            {

                try
                {

                    if (flupldImportExcel.HasFile)
                    {

                        CommonLogic.UploadFile(TenantRootDir + DB.GetSqlS("select UniqueID AS S from tpl_Tenant where TenantID=" + TenantID) + MMTExcelPath, flupldImportExcel, "ItemMasterList" + Path.GetExtension(flupldImportExcel.FileName));

                        DataTable dtExcelRecords;

                        String sFileName = CommonLogic._GetAttatchmentFile(TenantRootDir + DB.GetSqlS("select UniqueID AS S from tpl_Tenant where TenantID=" + TenantID) + MMTExcelPath, "ItemMasterList");

                        if (sFileName != "")
                        {

                            string mslinepath = HttpContext.Current.Server.MapPath(sFileName);

                            dtExcelRecords = CommonLogic.ImportDataFromExcel(mslinepath.Replace("\\mMaterialManagement", ""), Path.GetExtension(flupldImportExcel.FileName));

                            var dt = dtExcelRecords.AsEnumerable().Select(x => x.Field<string>("PartNumber")).Distinct();

                            getConnection();

                            con.Open();


                            //creating object of SqlBulkCopy    
                            SqlBulkCopy objbulk = new SqlBulkCopy(con);
                            //assigning Destination table name    
                            objbulk.DestinationTableName = "MMT_ItemMasterFromExcel";
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
                            //objbulk.ColumnMappings.Add(11, 11);
                            //objbulk.ColumnMappings.Add(12, 12);
                            objbulk.ColumnMappings.Add(11, 11);
                            objbulk.ColumnMappings.Add(12, 12);
                            objbulk.ColumnMappings.Add(13, 13);
                            objbulk.ColumnMappings.Add(14, 14);
                            objbulk.ColumnMappings.Add(15, 15);
                            objbulk.ColumnMappings.Add(16, 16);
                            objbulk.ColumnMappings.Add(17, 17);
                            objbulk.ColumnMappings.Add(18, 18);

                            objbulk.ColumnMappings.Add(21, 21);
                            objbulk.ColumnMappings.Add(22, 22);
                            //inserting Datatable Records to DataBase    

                            //dtExcelRecords = RemoveNullColumnFromDataTable(dtExcelRecords);
                            dtExcelRecords = RemoveEmptyRows(dtExcelRecords);

                            //if(dtExcelRecords.)

                            // added by lalitha on 22/04/2019
                            if (dtExcelRecords.Rows.Count == 0)
                            {
                                resetError("Please fill the item master excel sheet", true);
                            }
                            objbulk.BulkCopyTimeout = 500;
                            objbulk.WriteToServer(dtExcelRecords);
                            con.Close();
                            CommonLogic._DeleteAttatchment(TenantRootDir + DB.GetSqlS("select UniqueID AS S from tpl_Tenant where TenantID=" + TenantID) + MMTExcelPath, "ItemMasterList" + Path.GetExtension(flupldImportExcel.FileName));

                            //  DB.ExecuteSQL("EXEC [dbo].[sp_MMT_UpsertMaterialMasterItemFromExcel] @CreatedBy=" + cp.UserID + ",@TenantID=" + cp.TenantID);
                            #region to generate xml with parent child nodes
                            DataSet dsmsps = DB.GetDS("EXEC GET_DYNAMIC_MSPS @TenantID=" + hifTenant.Value, false);
                            DataTable dtmsps = new DataTable("MSP");
                            dtmsps = dsmsps.Tables[0];

                            StringBuilder sb = new StringBuilder();
                            int imspscount = dtmsps.Rows.Count;
                            int ilimit = (dtExcelRecords.Columns.Count) - imspscount;

                            int z = 0;
                            for (int i = ilimit; i < dtExcelRecords.Columns.Count; i++)
                            {

                                if (dtmsps.Rows[z][0].ToString() != dtExcelRecords.Columns[i].ToString())
                                {

                                    resetError("Please check the uploaded template, Attributes doesnt match for uploaded template and selected teanat", true);
                                    return;
                                }
                                z++;


                            }


                            sb.Append("<root>");
                            for (int i = 0; i < dtExcelRecords.Rows.Count; i++)
                            {
                                sb.Append("<data>");

                                //int TenantID = DB.GetSql("select UniqueID AS S from tpl_Tenant where TenantID=" + TenantID);

                                sb.Append("<MM_MST_Material_ID>" + dtExcelRecords.Rows[i][0].ToString() + "</MM_MST_Material_ID>");
                                for (int j = 0; j < dtExcelRecords.Columns.Count; j++)
                                {

                                    if (j < ilimit)
                                    {

                                    }
                                    else
                                    {
                                        sb.Append("<MSPS>");
                                        for (int k = 0; k < dtmsps.Rows.Count; k++)
                                        {
                                            if (Convert.ToString(dtExcelRecords.Rows[i][j]) == "" || Convert.ToString(dtExcelRecords.Rows[i][j]) == string.Empty)
                                            {

                                            }
                                            else
                                            {

                                                sb.Append("<MSP>");
                                                sb.Append("<MM_MST_Attribute_ID>" + dtmsps.Rows[k][1].ToString() + "</MM_MST_Attribute_ID><ATTRIBUTEVALUE>" + dtExcelRecords.Rows[i][j].ToString() + "</ATTRIBUTEVALUE>");
                                                sb.Append("</MSP>");
                                            }
                                            j++;
                                        }
                                        sb.Append("</MSPS>");
                                    }
                                }
                                sb.Append("</data>");

                            }
                            sb.Append("</root>");

                            string strxml = sb.ToString();
                            #endregion to generate xml with parent child nodes





                            DataSet DS = DB.GetDS("[dbo].[Usp_MMT_Bulk_UpsertMaterialMasterItemFromExcel_ATTRIBUTES] @UserID = " + cp.UserID + ",@AccountId=" + cp.AccountID + ",@XML=" + "'" + strxml + "'", false);
                            this.MMList_buildGridData(this.MMList_buildGridData(hfpageId.Value, drppagesize.SelectedValue), hfpageId.Value, drppagesize.SelectedValue);

                            if (DS.Tables[0].Rows.Count != 0)
                            {
                                downLoadExcel(DS, "FailedItems");
                            }


                            resetError("Successfully Uploaded", false);
                        }

                    }
                    else
                    {
                        resetError("Please upload item master excel sheet", true);
                        return;

                    }


                }
                catch (Exception ex)
                {
                    flupldImportExcel.Attributes.Clear();
                    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);

                    DB.ExecuteSQL("DELETE FROM MMT_ItemMasterFromExcel");

                    if (ex.Message.Contains("IX_MMT_ItemMasterFromExcel"))
                    {
                        resetError("Duplicate partnumber exists, Please check once and try again  : " + ex.Message.Substring(ex.Message.IndexOf("(") + 1).Replace(").\r\nThe statement has been terminated.", ""), true);
                        return;
                    }

                    resetError("Error while uploading item master. Error is " + ex.Message, true);

                    return;

                }
            }
            else
            {
                resetError("Please select tenant", true);
            }

        }

        //public static void RemoveNullColumnFromDataTable(DataTable dt)
        //{
        //    for (int i = dt.Rows.Count - 1; i >= 0; i--)
        //    {
        //        if (dt.Rows[i][1] == DBNull.Value)
        //            dt.Rows[i].Delete();
        //    }
        //    dt.AcceptChanges();
        //}

        private static DataTable RemoveEmptyRows(DataTable dt)
        {
            DataTable dt1 = dt.Clone(); //copy the structure 
            for (int i = 0; i <= dt.Rows.Count - 1; i++) //iterate through the rows of the source
            {
                DataRow currentRow = dt.Rows[i];  //copy the current row 
                foreach (var colValue in currentRow.ItemArray)//move along the columns 
                {
                    if (!string.IsNullOrEmpty(colValue.ToString())) // if there is a value in a column, copy the row and finish
                    {
                        dt1.ImportRow(currentRow);
                        break; //break and get a new row                        
                    }
                }
            }
            return dt1;
        }

        private void downLoadExcel(DataSet ds, string Name, int ismandatory = 0)
        {

            try
            {
                string FileName = Name;
                using (XLWorkbook wb = new XLWorkbook())
                {

                    IXLWorksheet sheet;
                    sheet = (wb.AddWorksheet(ds.Tables[0]));
                    sheet.Table("Table1").ShowAutoFilter = false;
                    wb.Worksheet(1).Rows(1, 1).Style.Fill.BackgroundColor = XLColor.FromHtml("219,229,241");
                    wb.Worksheet(1).Rows(1, 1).Style.Font.Bold = true;
                    wb.Worksheet(1).Rows(1, 1).Style.Font.FontName = "Calibri";
                    wb.Worksheet(1).Rows(1, 1).Style.Font.FontSize = 12;
                    wb.Worksheet(1).Rows(1, 1).Style.Font.FontColor = XLColor.RichBlack;
                    //wb.Worksheet(1).Rows(1, 1).Style.Alignment.WrapText = true;


                    if (ismandatory == 1)
                    {
                        //Making Partnumber as Not Mandatory 
                        //string[] strmandatoryfields = { "A1", "C1", "E1", "F1", "H1", "I1", "J1", "K1", "L1", "M1", "R1" ,"S1"};
                        string[] strmandatoryfields = { "C1", "E1", "F1", "H1", "I1", "J1", "K1", "L1", "M1", "R1", "S1" };

                        for (int i = 0; i < strmandatoryfields.Length; i++)
                        {
                            wb.Worksheet(1).Cells(strmandatoryfields[i]).Style.Font.FontColor = XLColor.Red;
                        }
                    }

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

                        flupldImportExcel.Attributes.Clear();
                        Response.End();
                    }

                }
            }
            catch (Exception ex)
            {
                flupldImportExcel.Attributes.Clear();
                this.MMList_buildGridData(this.MMList_buildGridData(hfpageId.Value, drppagesize.SelectedValue), hfpageId.Value, drppagesize.SelectedValue);
            }


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


        #endregion ---------------- Import Excel -------------------------------


        //===================== Added by M.D.Prasad on 22-03-2019============================//

        [WebMethod]
        public static string GetPrint(string MCode, string MDesc, string SerialNo, string BatchNo, string MfgDate, string ExpDate, string ProjectRefNo, string LabelID, string PrintQty, string MRP, string HUSize, string HUNo)
        {

            string ZPL = "";
            TracklineMLabel Mlabel = new TracklineMLabel();
            Mlabel.MCode = MCode;
            Mlabel.Description = MDesc;
            Mlabel.SerialNo = SerialNo;
            Mlabel.BatchNo = BatchNo;
            Mlabel.MfgDate = MfgDate;
            Mlabel.ExpDate = ExpDate;
            Mlabel.ProjectNo = ProjectRefNo;
            Mlabel.Mrp = MRP;

            string length = "";
            string width = "";
            string LabelType = "";
            string query = "select * from TPL_Tenant_BarcodeType where IsActive=1 and IsDeleted=0 and TenantBarcodeTypeID=" + LabelID;
            DataSet DS = DB.GetDS(query, false);
            foreach (DataRow row in DS.Tables[0].Rows)
            {
                length = row["Length"].ToString();
                width = row["Width"].ToString();
                LabelType = row["LabelType"].ToString();
            }
            Mlabel.KitCode = "";
            Mlabel.KitChildrenCount = CommonLogic.GetChildrenCount(Mlabel.KitPlannerID.ToString());
            Mlabel.ParentMCode = CommonLogic.GetKitParentCode(Mlabel.KitPlannerID.ToString());
            Mlabel.PrinterIP = "0";
            Mlabel.IsBoxLabelReq = false;
            Mlabel.Length = length;
            Mlabel.Width = width;
            Mlabel.HUSize = HUSize;
            Mlabel.HUNo = HUNo;
            int dpi = 0;
            dpi = CommonLogic.GETDPI(Mlabel.PrinterIP);
            Mlabel.Dpi = 203; //dpi;
            Mlabel.PrintQty = PrintQty;
            Mlabel.LabelType = LabelType;
            MRLWMSC21Common.PrintCommon.CommonPrint print = new MRLWMSC21Common.PrintCommon.CommonPrint();
            ZPL = print.PrintBarcodeLabel(Mlabel);
            return ZPL;
        }
        //===================== Added by M.D.Prasad on 22-03-2019============================//
    }
}