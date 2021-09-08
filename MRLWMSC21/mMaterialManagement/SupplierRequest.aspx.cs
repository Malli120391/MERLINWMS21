using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21Common;
using System.Data.SqlClient;
using System.Data;
using System.Xml.Serialization;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace MRLWMSC21.mMaterialManagement
{
    //Author    :   Gvd Prasad
    //Created On:   19-Sep-2013

    public partial class SupplierRequest : System.Web.UI.Page
    {
         public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "MasterData";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("Supplier Request"))))
            //{
            //    Response.Redirect("../Login.aspx?eid=6");
            //}
            cp = HttpContext.Current.User as CustomPrincipal;
            if (!IsPostBack)
            {
                Page.Validate();
                DesignLogic.SetInnerPageSubHeading(this.Page, "New Supplier");
                if (CommonLogic.QueryString("mfgid") == "")
                {
                    lblsearchterm.Visible = false;
                    ltSearchTerm.Visible = false;
                    txtSearchTerm.Visible = false;
                   
                   // chkIsActive.Visible = true;
                    chkIsApproved.Visible = false;
                   // chkIsActive.Enabled = false;
                    chkIsActive.Checked = true;
                    //lblPageSubHeader.Text = "Vendor Abbreviation Creation Request";

                    // lnkSendRequest.Text = "Send Request" + CommonLogic.btnfaSave;
                    lnkSendRequest.Text = "Save" + CommonLogic.btnfaSave;

                    //lnkAddTenant.Visible = false;
                    pnlSupplierList.Visible = false;
                    if (cp.AccountID != 0)
                    {
                        string acc = "";
                        string accID = "-1";
                        //<!-----------Procedure Converting----------->
                       // string query = "SELECT TOP 10 AccountID,Account FROM GEN_Account WHERE IsActive=1 AND IsDeleted=0 AND AccountID = " + cp.AccountID.ToString() + "order by Account";
                        string query = "Exec [dbo].[USP_MST_GetAccounts] @LogAccountID=" + cp.AccountID.ToString();
                        DataSet ds = DB.GetDS(query,false);
                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                acc = row["Account"].ToString();
                                accID = row["AccountID"].ToString();
                            }
                        }
                        
                        txtAccount.Text = acc;
                        hifAccount.Value = accID;
                        txtAccount.Enabled = false;
                       
                    }
                    else
                    {
                        //txtAccount.Enabled = true;
                        //string query = "SELECT TOP 1 AccountID,Account FROM GEN_Account WHERE IsActive=1 AND IsDeleted=0 AND AccountID = " + cp.AccountID.ToString();
                        //DataSet ds = DB.GetDS(query, false);

                        //txtAccount.Text = ds.Tables[0].Rows[0]["Account"].ToString();
                        //hifAccount.Value= ds.Tables[0].Rows[0]["AccountID"].ToString();
                        
                    }
                    
                }
                else
                {
                    //chkIsActive.Visible = true;
                   // chkIsActive.Enabled = true;
                 
                    ltSupplierCode.Visible = true;
                    txtSupplierCode.Visible = true;
                    //ltSearchTerm.Visible = true;
                    ltSearchTerm.Visible = false;
                    //txtSearchTerm.Visible = true;
                    txtSearchTerm.Visible = false; ;

                    //lnkAddTenant.Visible = true;

                    //lnkSendRequest.Text = "Update" + CommonLogic.btnfaUpdate;
                    lnkSendRequest.Text = "Save" + CommonLogic.btnfaSave;
                    if (cp.AccountID != 0)
                    {
                        string acc = "";
                        string accID = "-1";
                        //<!----------------Procedure Converting------------->
                        // string query = "SELECT TOP 10 AccountID,Account FROM GEN_Account WHERE IsActive=1 AND IsDeleted=0 AND AccountID = " + cp.AccountID.ToString() + "order by Account";
                        string query = "Exec [dbo].[USP_MST_GetAccounts] @LogAccountID=" + cp.AccountID.ToString();
                        DataSet ds = DB.GetDS(query, false);
                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                acc = row["Account"].ToString();
                                accID = row["AccountID"].ToString();
                            }
                        }

                        txtAccount.Text = acc;
                        hifAccount.Value = accID;
                        txtAccount.Enabled = false;
                    }
                    else
                    {
                        txtAccount.Enabled = true;
                    }
                   // ValidateTenant();

                    //<!----Procedure Converting-------------->
                   // string _query = "SELECT * FROM ORD_POHeader WHERE POStatusID NOT IN (3,4) AND SupplierID=" + CommonLogic.QueryString("mfgid");
                    string _query = "Exec [dbo].[USP_MST_GetSupplierPOStatus] @SupplierID=" + CommonLogic.QueryString("mfgid");
                    DataSet _ds = DB.GetDS(_query, false);
                    if (_ds.Tables[0].Rows.Count != 0)
                    {
                        chkIsActive.Enabled = false;
                    }
                    else
                    {
                        chkIsActive.Enabled = true;
                    }

                }
               
                if (CommonLogic.QueryString("mfgid") != "")
                {
                   LoadManufacturerData(CommonLogic.QueryString("mfgid"));

                    ViewState["TPLSupplierList"] = "[sp_TPL_GetTenantSupplierData] @SupplierID=" + CommonLogic.QueryString("mfgid");
                    this.SupplierList_buildGridData(this.SupplierList_buildGridData());
                }
               
            }

        }


        private void ValidateTenant()
        {
            //<!--------------Procedure Converting------>
            // if (DB.GetSqlN("select COUNT(*) N from MMT_MaterialMaster_Supplier where IsDeleted=0 and SupplierID=" + CommonLogic.QueryString("mfgid")) > 0)
            cp = HttpContext.Current.User as CustomPrincipal;
            if (DB.GetSqlN("Exec [dbo].[USP_Check_MMTSupplier] @SupplierID=" + CommonLogic.QueryString("mfgid")) > 0)

            {
                txtTenant.Enabled = false;
                //txtSupplierCode.Enabled = false;
            }
            else
            {
                txtTenant.Enabled = true;
                txtSupplierCode.Enabled = true;
            }
        }

        protected void lnkSendRequest_Click(object sender, EventArgs e)
        {
            //Page.Validate();
            //if (!Page.IsValid)
            //{
            //    return;
            //}
            //if (txtAccount.Text.Trim() == "")
            //{
            //    resetError("Please Select Account", true);
            //    txtAccount.Focus();
            //    return;
            //}
            cp = HttpContext.Current.User as CustomPrincipal;
            if (txtTenant.Text.Trim() == "")
            {
                resetError("Please Select Tenant", true);
                txtTenant.Focus();
                return;
            }
            if (txtSupplierName.Text.Trim() == "")
            {
                resetError("Please enter Supplier Name", true);
                txtSupplierName.Focus();
                return;
            }
            if (txtSupplierCode.Text.Trim() == "")
            {
                resetError("Please enter Supplier Code", true);
                txtSupplierCode.Focus();
                return;
            }
            if (txtTelephoneNo1.Text.Trim() == "")
            {
                resetError("Please enter TelePhoneNo.1", true);
                txtTelephoneNo1.Focus();
                return;
            }
            //if (txtTelephoneNo1.Text.Trim() != "" && (txtTelephoneNo1.Text.Trim().Length < 10 || txtTelephoneNo1.Text.Trim().Length > 14))
            //{
            //    resetError("Please enter Valid TelePhoneNo.1", true);
            //    txtTelephoneNo1.Focus();
            //    return;
            //}
            //if (txtTelephoneNo2.Text.Trim() != "" )
            //{
            //    if ((txtTelephoneNo2.Text.Trim().Length < 10 || txtTelephoneNo2.Text.Trim().Length > 14))
            //    {
            //        resetError("Please enter Valid TelePhoneNo.2", true);
            //        txtTelephoneNo2.Focus();
            //        return;
            //    }
            //}
            //if (txtMobileNo.Text.Trim() != "")
            //{
            //    if ((txtMobileNo.Text.Trim().Length < 10 || txtMobileNo.Text.Trim().Length > 14))
            //    {
            //        resetError("Please enter Valid Mobile No.", true);
            //        txtMobileNo.Focus();
            //        return;
            //    }
            //}
                
            
          

            if (txtSupplierAddress1.Text.Trim() == "")
            {
                resetError("Please enter Address.1", true);
                txtSupplierAddress1.Focus();
                return;
            }
            if (atcCountry.Text.Trim() == "")
            {
                resetError("Please select Country", true);
                atcCountry.Focus();
                return;
            }
            if (txtEmailAddress.Text.Trim() == "")
            {
                resetError("Please enter Email", true);
                txtEmailAddress.Focus();
                return;
            }
            if (txtEmailAddress.Text.Trim() != "")
            {
                string email = txtEmailAddress.Text;
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(email);
                if (!match.Success)
                {
                    resetError("Please enter Valid Email", true);
                    txtEmailAddress.Focus();
                    return;
                }
            }

            if (txtContactPerson.Text.Trim() == "")
            {
                resetError("Please enter Name", true);
                txtContactPerson.Focus();
                return;
            }
            if (txtContactPersonTitle.Text.Trim() == "")
            {
                resetError("Please enter Title", true);
                txtContactPersonTitle.Focus();
                return;
            }
            if (txtContactPersonContactNo.Text.Trim() == "")
            {
                resetError("Please enter Contact No.", true);
                txtContactPersonContactNo.Focus();
                return;
            }

            //if (txtContactPersonContactNo.Text.Trim() != "" && (txtContactPersonContactNo.Text.Trim().Length < 10 || txtContactPersonContactNo.Text.Trim().Length > 14))
            //{
            //    resetError("Please enter Valid Contact No.", true);
            //    txtContactPersonContactNo.Focus();
            //    return;
            //}

            if (txtEmailAddressContactPerson.Text.Trim() == "")
            {
                resetError("Please enter Email", true);
                txtEmailAddressContactPerson.Focus();
                return;
            }
            if (txtEmailAddressContactPerson.Text.Trim() != "")
            {
                string email = txtEmailAddressContactPerson.Text;
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(email);
                if (!match.Success)
                {
                    resetError("Please enter Valid Email", true);
                    txtEmailAddressContactPerson.Focus();
                    return;
                }
            }





            StringBuilder sqlInsert = new StringBuilder();



            sqlInsert.Append("Declare @NewUpdateSupplierID int ");
            sqlInsert.Append("EXEC [dbo].[sp_MMT_UpsertSupplier] ");
            sqlInsert.Append("@SupplierName=" + DB.SQuote(txtSupplierName.Text));



            sqlInsert.Append(",@Address1 =" + CommonLogic.IIF(txtSupplierAddress1.Text.Trim() != "", DB.SQuote(txtSupplierAddress1.Text.Trim()), "null"));
            sqlInsert.Append(",@Address2 =" + CommonLogic.IIF(txtSupplierAddress2.Text.Trim() != "", DB.SQuote(txtSupplierAddress2.Text.Trim()), "null"));

            sqlInsert.Append(",@CountryMasterID=" + CommonLogic.IIF(atcCountry.Text.Trim() != "" && Request.Form[hifCountry.UniqueID] != "", Request.Form[hifCountry.UniqueID], "null"));
            sqlInsert.Append(",@Phone1 =" + CommonLogic.IIF(txtTelephoneNo1.Text.Trim() != "", DB.SQuote(txtTelephoneNo1.Text.Trim()), "null"));
            sqlInsert.Append(",@Phone2 =" + CommonLogic.IIF(txtTelephoneNo2.Text.Trim() != "", DB.SQuote(txtTelephoneNo2.Text.Trim()), "null"));
            sqlInsert.Append(",@Mobile  =" + CommonLogic.IIF(txtMobileNo.Text.Trim() != "", DB.SQuote(txtMobileNo.Text.Trim()), "null"));
            sqlInsert.Append(",@Fax =" + CommonLogic.IIF(txtFaxNo.Text.Trim() != "", DB.SQuote(txtFaxNo.Text.Trim()), "null"));
            sqlInsert.Append(",@EmailAddress =" + CommonLogic.IIF(txtEmailAddress.Text.Trim() != "", DB.SQuote(txtEmailAddress.Text.Trim()), "null"));
            sqlInsert.Append(",@PCP =" + CommonLogic.IIF(txtContactPerson.Text.Trim() != "", DB.SQuote(txtContactPerson.Text.Trim()), "null"));
            sqlInsert.Append(",@PCPTitle =" + CommonLogic.IIF(txtContactPersonTitle.Text.Trim() != "", DB.SQuote(txtContactPersonTitle.Text.Trim()), "null"));
            sqlInsert.Append(",@PCPContactNumber =" + CommonLogic.IIF(txtContactPersonContactNo.Text.Trim() != "", DB.SQuote(txtContactPersonContactNo.Text.Trim()), "null"));
            sqlInsert.Append(",@PCPEmail =" + CommonLogic.IIF(txtEmailAddressContactPerson.Text.Trim() != "", DB.SQuote(txtEmailAddressContactPerson.Text.Trim()), "null"));
            sqlInsert.Append(",@BankName=" + CommonLogic.IIF(txtBankName.Text.Trim() != "", DB.SQuote(txtBankName.Text.Trim()), "null"));
            sqlInsert.Append(",@BankAddress=" + CommonLogic.IIF(txtBankAddress.Text.Trim() != "", DB.SQuote(txtBankAddress.Text.Trim()), "null"));
            sqlInsert.Append(",@BankCountryID=" + CommonLogic.IIF(atcBankCountry.Text.Trim() != "" && Request.Form[hifBankCountry.UniqueID] != "", Request.Form[hifBankCountry.UniqueID], "null"));
            sqlInsert.Append(",@AccountNo=" + CommonLogic.IIF(txtAccountNo.Text.Trim() != "", DB.SQuote(txtAccountNo.Text.Trim()), "null"));
            sqlInsert.Append(",@SortCodeORBLZCode=" + CommonLogic.IIF(txtSortCode.Text.Trim() != "", DB.SQuote(txtSortCode.Text.Trim()), "null"));
            sqlInsert.Append(",@IBANNo=" + CommonLogic.IIF(txtIBANNo.Text.Trim() != "", DB.SQuote(txtIBANNo.Text.Trim()), "null"));
            sqlInsert.Append(",@SwiftCode=" + CommonLogic.IIF(txtSwiftCode.Text.Trim() != "", DB.SQuote(txtSwiftCode.Text.Trim()), "null"));
            sqlInsert.Append(",@CurrencyID=" + CommonLogic.IIF(atcBankCurrency.Text.Trim() != "" && Request.Form[hifBankCurrency.UniqueID] != "", Request.Form[hifBankCurrency.UniqueID], "null"));
            sqlInsert.Append(",@SupplierCode=" + CommonLogic.IIF(txtSupplierCode.Text != "", DB.SQuote(txtSupplierCode.Text), "null"));

            sqlInsert.Append(",@CreatedBy = " + cp.UserID.ToString());                                                                    //have to change
            //sqlInsert.Append(",@TenantID=1");
            sqlInsert.Append(",@TenantID="+hifTenant.Value);
            sqlInsert.Append(",@AccountId=" + hifAccount.Value);

            if (CommonLogic.QueryString("mfgid") == "")
            {
                sqlInsert.Append(",@SupplierID=0");
                sqlInsert.Append(",@RequestedBy=" + cp.UserID.ToString());
                sqlInsert.Append(",@SearchTerm=null");
               
                sqlInsert.Append(",@IsApproved=1");
                sqlInsert.Append(",@IsActive=" + (chkIsActive.Checked ? "1" : "0"));
                sqlInsert.Append(",@IsFirstEdit=0");
                sqlInsert.Append(",@LastEditedByID=0");
                sqlInsert.Append(",@SupplierCodeAprEditCount =0");
                

            }
            else
            {
                sqlInsert.Append(",@SupplierID=" + CommonLogic.QueryString("mfgid"));
                sqlInsert.Append(",@RequestedBy=1");
                sqlInsert.Append(",@SearchTerm=" + CommonLogic.IIF(txtSearchTerm.Text != "", DB.SQuote(txtSearchTerm.Text), "null"));
               
                sqlInsert.Append(",@IsApproved=" + Convert.ToInt16(chkIsApproved.Checked));
                sqlInsert.Append(",@IsActive=" + Convert.ToInt16((chkIsActive.Checked ? "1" : "0")));

                //to Check the Supplier Name duplication while User is in Edit Mode

                //to Check the Edit first time or not
                if ((ltIsFirstEdit.Text.Trim() == "0") || (ltIsFirstEdit.Text.Trim() == ""))
                {
                    ltIsFirstEdit.Text = "1";
                    sqlInsert.Append(",@IsFirstEdit=1");
                }
                else
                {
                    ltIsFirstEdit.Text = (Convert.ToInt32(ltIsFirstEdit.Text) + 1).ToString();
                    sqlInsert.Append(",@IsFirstEdit=" + ltIsFirstEdit.Text);
                }
                sqlInsert.Append(",@LastEditedByID= 1");                                    //HAVE TO CHANGE
                sqlInsert.Append(",@SupplierCodeAprEditCount =" + (Convert.ToInt32(ltSupplierCodeAprEditCount.Text)+1));
            }
            sqlInsert.Append(",@NewSupplierID = @NewUpdateSupplierID");
            sqlInsert.Append(" OUTPUT Select @NewUpdateSupplierID as N");
            try
            {
                int ResMfgRequest = DB.GetSqlN(sqlInsert.ToString());
                if (ResMfgRequest == -1)  //to Check Supplier Name duplication while in Edit Mode OR New Record
                {
                    resetError("This Supplier Name is already existing.", false);
                    return;
                }
                else if (ResMfgRequest > 0) //If insertion or updation is successful, send an E-Mail
                {
                    resetError("Successfully Saved", false);
                    Response.Redirect("SupplierList.aspx?statid=success",false);
                }
                
            }
            catch (SqlException ex)
            {
                if (ex.Message.IndexOf(" Cannot insert duplicate key in object 'dbo.MMT_Supplier'.")>-1)
                {
                    resetError("Supplier code already exists under this Tenant", true);
                }
                else
                {
                    resetError("Error while submitting the data", true);
                }
            }
            catch (Exception ex)
            {
                resetError("Error while submitting the data", true);
            }

    


        }

        public void LoadManufacturerData(String MMItemID)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            IDataReader rsMMItem = null;
            try
            {
                rsMMItem = DB.GetRS("EXEC sp_MMT_SupplierDetails @SupplierID =" + MMItemID + " ,@AccountID_New="+cp.AccountID+ ",@TenantID_New="+cp.TenantID+",@TenantID=" + cp.TenantID);
            }
            catch (Exception ex)
            {
                resetError("Error while loading",true);
                return;
            }
            //if (!rsMMItem.Read())
            //{
            //    // ltsearch.Visible = false;               
            //    resetError("No details available", true);
            //    rsMMItem.Close();
            //    return;
            //}
            
                while (rsMMItem.Read())
            {

                txtSupplierName.Text = DB.RSField(rsMMItem, "SupplierName");
                Session["SupplierName"] = txtSupplierName.Text.Trim();

                txtSupplierCode.Text = DB.RSField(rsMMItem, "SupplierCode");
                txtSearchTerm.Text = DB.RSField(rsMMItem, "SearchTerm");
             


                txtSupplierAddress1.Text = DB.RSField(rsMMItem, "Address1");
                txtSupplierAddress2.Text = DB.RSField(rsMMItem, "Address2");

                atcCountry.Text=DB.RSField(rsMMItem,"CountryName");
                hifCountry.Value = DB.RSFieldInt(rsMMItem, "CountryMasterID").ToString();
                txtTelephoneNo1.Text = DB.RSField(rsMMItem, "phone1");
                txtTelephoneNo2.Text = DB.RSField(rsMMItem, "phone2");
                txtMobileNo.Text = DB.RSField(rsMMItem, "Mobile");
                txtFaxNo.Text = DB.RSField(rsMMItem, "Fax");
                txtEmailAddress.Text = DB.RSField(rsMMItem, "EmailAddress");
                txtContactPerson.Text = DB.RSField(rsMMItem, "PCP");
                txtContactPersonTitle.Text = DB.RSField(rsMMItem, "PCPTitle");
                txtContactPersonContactNo.Text = DB.RSField(rsMMItem, "PCPContactNumber");
                txtEmailAddressContactPerson.Text = DB.RSField(rsMMItem, "PCPEmail");

                txtBankName.Text = DB.RSField(rsMMItem, "BankName");
                txtBankAddress.Text = DB.RSField(rsMMItem, "BankAddress");

                atcBankCountry.Text=DB.RSField(rsMMItem,"BankCountry");
                hifBankCountry.Value = DB.RSFieldInt(rsMMItem, "BankCountryID").ToString();
                txtAccountNo.Text = DB.RSField(rsMMItem, "AccountNo");

                txtSortCode.Text = DB.RSField(rsMMItem, "SortCodeORBLZCode");
                txtIBANNo.Text = DB.RSField(rsMMItem, "IBANNo");
                txtSwiftCode.Text = DB.RSField(rsMMItem, "SwiftCode");
                atcBankCurrency.Text=DB.RSField(rsMMItem,"Currency");
                hifBankCurrency.Value = DB.RSFieldInt(rsMMItem, "CurrencyID").ToString();

                chkIsApproved.Checked = Convert.ToBoolean(DB.RSFieldTinyInt(rsMMItem, "IsApproved"));
                chkIsActive.Checked = Convert.ToBoolean(DB.RSFieldTinyInt(rsMMItem, "IsActive"));

                ltIsFirstEdit.Text = DB.RSFieldInt(rsMMItem, "IsFirstEdit").ToString();
                ltSupplierCodeAprEditCount.Text = DB.RSFieldInt(rsMMItem, "SupplierCodeAprEditCount").ToString();

                txtTenant.Text = DB.RSField(rsMMItem, "TenantName");
                hifTenant.Value = DB.RSFieldInt(rsMMItem, "TenantID").ToString();
                hifAccount.Value= DB.RSFieldInt(rsMMItem, "AccountId").ToString();
                if (cp.AccountID == 0)
                {
                    txtAccount.Text = DB.RSField(rsMMItem, "Account");
                }
                else
                {
                    string acc = "";
                    //<!---Procedure converting---->
                    //string query = "SELECT TOP 10 AccountID,Account FROM GEN_Account WHERE IsActive=1 AND IsDeleted=0 AND AccountID = " + cp.AccountID.ToString() + "order by Account";
                    string query = "Exec [dbo].[USP_MST_GetAccounts] @LogAccountID=" + cp.AccountID.ToString();
                    DataSet ds = DB.GetDS(query, false);
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            acc = row["Account"].ToString();
                        }
                    }

                    txtAccount.Text = acc;
                }
            }
            rsMMItem.Close();
        }

        public void LoadDropDown(DropDownList ddllist, String SqlQry, String strListName, String strListValue, string defaultValue)
        {
            ddllist.Items.Clear();
            ddllist.Items.Add(new ListItem(defaultValue, ""));

            IDataReader rsList = DB.GetRS(SqlQry);
            while (rsList.Read())
            {
                if (ddllist.ID == "ddlSupplierID")
                    ddllist.Items.Add(new ListItem(rsList["SupplierCode"].ToString() + "-" + rsList[strListName].ToString(), rsList[strListValue].ToString()));
                else
                    ddllist.Items.Add(new ListItem(rsList[strListName].ToString(), rsList[strListValue].ToString()));

            }
            rsList.Close();
        }

        public void DisableAllItemsExceptMaterialCode()
        {
            txtSupplierName.Enabled = false;
            rfvtxtSupplierName.Enabled = false;
            txtSearchTerm.Enabled = false;
            txtSupplierAddress1.Enabled = false;
            rfvtxtSupplierAddress1.Enabled = false;
            txtSupplierAddress2.Enabled = false;
            txtMobileNo.Enabled = false;
            rfvddlCountry.Enabled = false;
            txtMobileNo.Enabled = false;
            txtFaxNo.Enabled = false;
            txtEmailAddress.Enabled = false;
            rfvtxtEmailAddress.Enabled = false;
            revEmailAddress.Enabled = false;
            txtContactPerson.Enabled = false;
            txtContactPersonTitle.Enabled = false;
            txtContactPersonContactNo.Enabled = false;
            txtEmailAddressContactPerson.Enabled = false;
            //revEmailContactPerson.Enabled = false;
            txtBankName.Enabled = false;
            txtBankAddress.Enabled = false;
            txtAccountNo.Enabled = false;
            txtSortCode.Enabled = false;
            txtIBANNo.Enabled = false;
            txtSwiftCode.Enabled = false;
         
        }

        protected void resetError(string error, bool isError)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "closeAsynchronus(); showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);
        }


        public string GetUserName(String UserID)
        {
            if (UserID == "")
                return "";
            else
            {
            
                return "   ";
            }
            
        }

        protected void lnkButCancel_Click(object sender, EventArgs e)
        {   
               Response.Redirect("SupplierList.aspx");
            
        }


        private DataSet SupplierList_buildGridData()
        {
            string SupplierListsql = ViewState["TPLSupplierList"].ToString();
            DataSet dsSupplierList = DB.GetDS(SupplierListsql, false);
            return dsSupplierList;
        }

        private void SupplierList_buildGridData(DataSet cMdSupplierList)
        {
            gvSupplierList.DataSource = cMdSupplierList;
            gvSupplierList.DataBind();
            cMdSupplierList.Dispose();
        }


        protected void gvSupplierList_Sorting(object sender, GridViewSortEventArgs e)
        {
            //ViewState["ShimpentPendingIsInsert"] = false;
            //gvSupplierList.EditIndex = -1;
            //ViewState["MMListSort"] = e.SortExpression.ToString();
            //ViewState["MMListSortOrder"] = (ViewState["MMListSortOrder"].ToString() == "ASC" ? "DESC" : "ASC");
            //this.SupplierList_buildGridData(this.SupplierList_buildGridData());
        }

        protected void gvSupplierList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSupplierList.PageIndex = e.NewPageIndex;
            SupplierList_buildGridData(SupplierList_buildGridData());
        }

        protected void gvSupplierList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            if (e.Row.DataItem == null)
                return;

            if ((e.Row.RowState & DataControlRowState.Edit) != DataControlRowState.Edit)
            {
                string ltTenantID = ((Literal)e.Row.FindControl("ltTenantID")).Text;
                string SupplierCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckSupplierForDelete] @SupplierID=" + CommonLogic.QueryString("mfgid") + ",@TenantIDs=" + DB.SQuote(ltTenantID.ToString()));
                if (SupplierCheck != "")
                {
                    e.Row.Controls[4].Controls[0].Visible = false;
                }
            }
        }

        protected void gvSupplierList_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvSupplierList.EditIndex = e.NewEditIndex;
            SupplierList_buildGridData(SupplierList_buildGridData());
        }

        protected void gvSupplierList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvSupplierList.EditIndex = -1;
            SupplierList_buildGridData(SupplierList_buildGridData());
        }

        protected void gvSupplierList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            Page.Validate("vRequiredCompanyName");
            if (!Page.IsValid)
            {
                return;
            }

            GridViewRow row = gvSupplierList.Rows[e.RowIndex];

            if (row != null)
            {

                string hifTenantID = ((HiddenField)row.FindControl("hifTenantID")).Value;
                string ltTenantSupplierID =((Literal)row.FindControl("ltTenantSupplierID")).Text;

                if (hifTenantID == "")
                {
                    resetErrorForSupplier("This Tenant doesn't exist", true);
                    return;
                }

                if (DB.GetSqlN("select Tenant_SupplierID N  from TPL_Tenant_Supplier where TenantID=" + hifTenantID + " and SupplierID=" + CommonLogic.QueryString("mfgid") + " and IsDeleted=0 and Tenant_SupplierID!=" + ltTenantSupplierID) != 0)
                {
                    resetErrorForSupplier("Tenant already exists", true);
                    return;
                }

                StringBuilder QueryForActivityGroup = new StringBuilder();
                QueryForActivityGroup.Append(" EXEC [dbo].[sp_TPL_UpsertTenantSupplier] ");
                QueryForActivityGroup.Append(" @TenantID=" + hifTenantID);
                QueryForActivityGroup.Append(",@CreatedBy=" + cp.UserID);
                QueryForActivityGroup.Append(",@SupplierID=" + CommonLogic.QueryString("mfgid"));
                QueryForActivityGroup.Append(",@Tenant_SupplierID=" + ltTenantSupplierID);
                try
                {
                    DB.ExecuteSQL(QueryForActivityGroup.ToString());
                    resetErrorForSupplier("Successfully Saved", false);
                }
                catch (Exception ex)
                {
                    resetErrorForSupplier("Error while updating", true);
                }

                gvSupplierList.EditIndex = -1;
                this.SupplierList_buildGridData(this.SupplierList_buildGridData());
                
            }
        }

        protected void resetErrorForSupplier(string error, bool isError)
        {

            ////string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
            ////if (isError)
            ////    str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

            ////if (error.Length > 0)
            ////    str += error + "";
            ////else
            ////    str = "";

            //lblSupplierStatus.Text = str + "</font>";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);
        }

        protected void lnkSupplierDelete_Click(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            string gvIDs = "";
            bool chkBox = false;
            string TenantIDs = "";

            StringBuilder sqlDeleteString = new StringBuilder();

            foreach (GridViewRow gv in gvSupplierList.Rows)
            {
                CheckBox isDelete = (CheckBox)gv.FindControl("chkIsDelete");
                if (isDelete.Checked)
                {
                    chkBox = true;
                    gvIDs += ((Literal)gv.FindControl("ltTenantSupplierID")).Text.ToString() + ",";
                    TenantIDs += ((Literal)gv.FindControl("ltTenantID")).Text.ToString() + ",";
                }
            }

            if (chkBox)
            {
                string SupplierCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckSupplierForDelete] @SupplierID=" + CommonLogic.QueryString("mfgid") + ",@TenantIDs=" + DB.SQuote(TenantIDs.ToString()));

                if(SupplierCheck!="")
                {
                    this.resetErrorForSupplier("Cannot delete, as this Supplier is configured in  " + CommonLogic.IIF(SupplierCheck != "", "Materials : ", "") + SupplierCheck, true);
                    return;
                }

                sqlDeleteString.Append("BEGIN TRAN ");

                sqlDeleteString.Append("UPDATE TPL_Tenant_Supplier SET IsDeleted=1 WHERE Tenant_SupplierID IN (" + gvIDs.Substring(0, gvIDs.LastIndexOf(",")) + ")");

                sqlDeleteString.Append(" COMMIT ");
            }
            try
            {

                if (chkBox)
                {
                    DB.ExecuteSQL(sqlDeleteString.ToString());
                }
                resetErrorForSupplier("Successfully deleted the selected items", false);

            }
            catch (Exception ex)
            {
                resetErrorForSupplier("Error Updating data" + ex.ToString(), true);
            }

                ViewState["MMItemLocationSQL"] = "[sp_TPL_GetTenantSupplierData] @SupplierID=" + CommonLogic.QueryString("mfgid");
                this.SupplierList_buildGridData(this.SupplierList_buildGridData());

               
        }

       
        protected void lnkAddTenant_Click(object sender, EventArgs e)
        {
            try
            {
                gvSupplierList.EditIndex = 0;
                gvSupplierList.PageIndex = 0;

                DataSet dsSupplierList = SupplierList_buildGridData();
                DataRow row = dsSupplierList.Tables[0].NewRow();
                row["Tenant_SupplierID"] = 0;
                dsSupplierList.Tables[0].Rows.InsertAt(row, 0);
                this.SupplierList_buildGridData(dsSupplierList);
            }
            catch (Exception ex)
            {
                this.resetErrorForSupplier("Error while inserting", true);
            }
        }

        protected void lnkUpdateTenant_Click(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            Page.Validate("UpdateTenant");
            if (!Page.IsValid)
            {
                return;
            }

            if (hifTenant.Value == "")
            {
                resetErrorForSupplier("This Tenant doesn't exist", true);
                return;
            }

            StringBuilder QueryForActivityGroup = new StringBuilder();
            QueryForActivityGroup.Append(" EXEC [dbo].[sp_TPL_UpsertTenantSupplier] ");
            QueryForActivityGroup.Append(" @TenantID=" + hifTenant);
            QueryForActivityGroup.Append(",@CreatedBy=" + cp.UserID);
            QueryForActivityGroup.Append(",@SupplierID=" + CommonLogic.QueryString("mfgid"));
            QueryForActivityGroup.Append(",@Tenant_SupplierID=0");
            try
            {
                DB.ExecuteSQL(QueryForActivityGroup.ToString());
                resetErrorForSupplier("Successfully Saved", false);
            }
            catch (Exception ex)
            {
                resetErrorForSupplier("Error while updating", true);
            }

            gvSupplierList.EditIndex = -1;
            this.SupplierList_buildGridData(this.SupplierList_buildGridData());
        }
    }
}