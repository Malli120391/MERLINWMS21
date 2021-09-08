using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Globalization;
using MRLWMSC21Common;
using System.Web.Services;
using System.IO;
using Newtonsoft.Json;
using MRLWMSC21.TPL;
using static MRLWMSC21.TPL.NewAccount;
using System.Text.RegularExpressions;

namespace FalconAdmin.General
{
    public partial class TenantRegistration : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        public String tid1 = CommonLogic.QueryString("tid");
        string MMTPath = "";
        string ddlCountryVal = "";
        string ddlBillCountryVal = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            string selectMMList;
            if (!IsPostBack)
            {
                //txtlatitude.Enabled = false;
                DesignLogic.SetInnerPageSubHeading(this.Page, "New Tenant");
                //selectMMList = "SELECT Account,AccountID FROM GEN_Account where IsActive=1 and IsDeleted=0";
                //selectMMList = selectMMList + "and AccountID = case when 0 = " + cp.AccountID.ToString() + " then AccountID else " + cp.AccountID.ToString() + " end";
                selectMMList = "Exec [dbo].[USP_MST_GetAccountDrop] @LogAccountID=" + cp.AccountID.ToString();



                //trContractInformation.Visible=(CommonLogic.QueryString("tid") != "");
                //LoadDropDown(ddlIndustry, "select IndustryID,IndustryType from GEN_Industry where IsActive=1 and IsDeleted=0", "IndustryType", "IndustryID", "Select Industry");
                LoadDropDown(ddlBusinessType, "Exec [dbo].[USP_MST_GetBusinessType]", "BusinessType", "BusinessTypeID", "Select Business Type");
              //  LoadDropDown(ddlCountry, "select CountryMasterID,CountryName from GEN_CountryMaster where IsActive=1 and IsDeleted=0 order by CountryName", "CountryName", "CountryMasterID", "Select Country");
                LoadDropDown(ddlCountry, "Exec  [dbo].[USP_LoadCountryDropDown]", "CountryName", "CountryMasterID", "Select Country");
               

                //LoadDropDown(ddlCurrency, "select CurrencyID,Currency from GEN_Currency where IsActive=1 and IsDeleted=0 order by Currency", "Currency", "CurrencyID", "Select Currency");
               //LoadDropDown(ddlBankCountry, "select CountryMasterID,CountryName from GEN_CountryMaster where IsActive=1 and IsDeleted=0 order by CountryName", "CountryName", "CountryMasterID", "Select Country");
              //LoadDropDown(ddlBankCurrency, "select CurrencyID,Currency from GEN_Currency where IsActive=1 and IsDeleted=0 order by Currency", "Currency", "CurrencyID", "Select Currency");
               LoadDropDown(ddlBilltype, "SELECT BillName,BillTypeID FROM TPL_BillType where IsActive=1 and IsDeleted=0", "BillName", "BillTypeID", "Select Bill Type");
                //LoadDropDown(ddlaccount, "SELECT Account,AccountID FROM GEN_Account where IsActive=1 and IsDeleted=0", "Account", "AccountID", "Select Account");

                LoadDropDown(ddlaccount, selectMMList, "Account", "AccountID", "Select Account");

                


                //LoadDropDown(ddldocumenttype, "select FileTypeID,FileType from MMT_FileType where isdeleted=0 and isactive=1", "FileType", "FileTypeID", "Select Attachment Type");
                ////LoadDropDown(ddlSpaceutilization, "select SpaceUtilizationID,SpaceUtilization from TPL_SpaceUtilization where IsActive=1 and IsDeleted=0", "SpaceUtilization", "SpaceUtilizationID","Select Space Utilization Type");
                MMTPath = DB.GetSqlS(" select DefaultValue AS S from SYS_SystemConfiguration SYS_C left join SYS_SysConfigKey SYS_K on SYS_K.SysConfigKeyID=SYS_C.SysConfigKeyID where SysConfigKey='MaterialManagementPath'");

                //billing Information
                LoadDropDown(ddlBillingCountry, "Exec  [dbo].[USP_LoadCountryDropDown]", "CountryName", "CountryMasterID", "Select Country");
                LoadDropDown(ddlBillingCurrency, "Exec [dbo].[USP_MST_GetCurrencyByCountry]", "Currency", "CurrencyID", "Select Currency");
              
                if (CommonLogic.QueryString("tid") != "")
                {
                    ViewState["tid"] = CommonLogic.QueryString("tid");
                    //int accountid = DB.GetSqlN("select AccountID as N from TPL_Tenant where TenantID=" + ViewState["tid"].ToString());
                    int accountid = DB.GetSqlN("Exec [dbo].[USP_MST_GetAccount_Tenant] @TenantID=" + ViewState["tid"].ToString());
                    string userurl = "http://localhost/MRLWMSC21_3PL/Admin/General/UsersList.aspx?ACID=" + accountid + "&TID=" + ViewState["tid"] + "&UID=" + cp.UserID;
                    //userFrame.Attributes.Add("src", userurl);
                    FillFOrmData();
                    gvContract.Visible = true;
                    lnkSavePrimaryInfo.Text = "Update" + CommonLogic.btnfaUpdate;
                    LoadContract(LoadContract());
                    if (gvContract.Rows.Count <= 0)
                    {

                        lblvieweattachment.Visible = false;
                    }
                    middailogbox();
                    trContractInformation.Visible = true;


                   // trUserInformation.Visible = true;
                    //LoadMspCheckBoxList();
                    if (CommonLogic.QueryString("status") == "Createsuccess")
                    {
                        resetError("Successfully created", false);

                    }
                    else if(CommonLogic.QueryString("status") == "Updatesuccess")
                    {
                        resetError("Successfully updated", false);
                    }
                }
                else
                {
                    ViewState["tid"] = "0";
                    lnkSavePrimaryInfo.Text = "Save" + CommonLogic.btnfaSave;
                   // lnkAddContract_Click(sender, e);
                    //lnkAddContract.Enabled = false;
                    gvContract.Columns[5].Visible = false;
                }

                Page.Validate("vgTenantRegistration");
                Page.Validate("vgContract");

            }
            else {
                if (ViewState["tid"].ToString() != "0")
                {
                    //lnkAddContract.Enabled = true;

                }
                else
                {
                    //lnkAddContract.Enabled = true;
                }

            }

            if (cp.AccountID != 0)
            {
                ddlaccount.SelectedValue = cp.AccountID.ToString();
                ddlaccount.Enabled = false;
            }
        }
        protected void LoadMspCheckBoxList()
        {

            // Get data from MaterialStorageParameter
            IDataReader rsChkList = DB.GetRS("select WarehouseID,WHCode from  GEN_Warehouse where IsActive=1 AND IsDeleted=0");

            while (rsChkList.Read())
            {
                WHChkBoxList.Items.Add(new ListItem(rsChkList["WHCode"].ToString(), rsChkList["WarehouseID"].ToString()));               
            }
            rsChkList.Close();

        }
        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlCountryVal = ddlCountry.SelectedItem.Value;
            //LoadDropDown(ddlCurrency, "select CurrencyID,Currency from GEN_Currency where IsActive=1 and IsDeleted=0 and CountryID = " + ddlCountryVal + " order by Currency", "Currency", "CurrencyID", "Select Currency");
            LoadDropDown(ddlCurrency, "Exec [dbo].[USP_MST_GetCurrencyByCountry] @CountryID = " + hdnCountry.Value + "", "Currency", "CurrencyID", "Select Currency");
        }

        protected void ddlBillingCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlBillCountryVal = ddlBillingCountry.SelectedItem.Value;
           // LoadDropDown(ddlBillingCurrency, "select CurrencyID,Currency from GEN_Currency where IsActive=1 and IsDeleted=0 and CountryID = " + ddlBillCountryVal + " order by Currency", "Currency", "CurrencyID", "Select Currency");
            LoadDropDown(ddlBillingCurrency, "Exec [dbo].[USP_MST_GetCurrencyByCountry] @CountryID = " + ddlBillCountryVal + " order by Currency", "Currency", "CurrencyID", "Select Currency");

        }

        private void FillFOrmData()
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            DataSet ds = DB.GetDS("EXEC sp_TPL_GetTenantRegistrationDetails @TenantID="+CommonLogic.QueryString("tid")+", @AccountID_New="+cp.AccountID, false);
            DataTable dtPrimaryInfo=ds.Tables[0];
            DataTable dtBillingInfo=ds.Tables[1];
            DataTable dtBankInfo=ds.Tables[2];
            DataTable dtContractInfo=ds.Tables[3];
            DataTable dtSpaceutilization = ds.Tables[4];
            DataTable dtInvoice = ds.Tables[5];
            if (dtPrimaryInfo.Rows.Count > 0)
            {
                hifTenantID.Value = CommonLogic.QueryString("tid");
                txtCompanyName.Text = dtPrimaryInfo.Rows[0]["TenantName"].ToString();
                txtCompanyDBA.Text = dtPrimaryInfo.Rows[0]["CompanyDBA"].ToString();
                ddlBusinessType.SelectedValue = dtPrimaryInfo.Rows[0]["BusinessTypeID"].ToString();
                txtCompanyRegistration.Text = dtPrimaryInfo.Rows[0]["TenantRegistrationNo"].ToString();
                txtFirstName.Text = dtPrimaryInfo.Rows[0]["PCPFirstName"].ToString();
                txtLastName.Text = dtPrimaryInfo.Rows[0]["PCPLastName"].ToString();
                //Query.Append(" ,@PCPContactNo  =NULL");
                txtEmail.Text = dtPrimaryInfo.Rows[0]["PCPEmail"].ToString();
                txtWebsite.Text = dtPrimaryInfo.Rows[0]["Website"].ToString();
                if (dtPrimaryInfo.Rows[0]["IsInsurance"].ToString() == "1")
                    cbxIsInsurence.Checked = true;
                else
                    cbxIsInsurence.Checked = false;

                if (dtPrimaryInfo.Rows[0]["IsTaxApplicable"].ToString() == "1")
                    cbtaxapplicable.Checked = true;
                else
                    cbtaxapplicable.Checked = false;
                txtGstNumber.Text = dtPrimaryInfo.Rows[0]["GSTNumber"].ToString();
                txtAddressLine1.Text = dtPrimaryInfo.Rows[0]["Address1"].ToString();
                txtAddressLine2.Text = dtPrimaryInfo.Rows[0]["Address2"].ToString();
                txtCity.Text = dtPrimaryInfo.Rows[0]["City"].ToString();
                txtState.Text = dtPrimaryInfo.Rows[0]["State"].ToString();
                hdnStateId.Value = dtPrimaryInfo.Rows[0]["StateMasterID"].ToString();
                hdnCity.Value = dtPrimaryInfo.Rows[0]["CityMasterID"].ToString();
                hdnZip.Value = dtPrimaryInfo.Rows[0]["ZipCodeID"].ToString();
                txtZip.Text = dtPrimaryInfo.Rows[0]["ZIP"].ToString();
                ddlaccount.SelectedValue= dtPrimaryInfo.Rows[0]["AccountID"].ToString();
                //ddlCountry.SelectedValue = dtPrimaryInfo.Rows[0]["CountryMasterID"].ToString();
                hdnCountry.Value= dtPrimaryInfo.Rows[0]["CountryMasterID"].ToString();
                txtCountry.Text = dtPrimaryInfo.Rows[0]["CountryName"].ToString();
                //LoadDropDown(ddlCurrency, "select CurrencyID,Currency from GEN_Currency where IsActive=1 and IsDeleted=0 and CountryID = " + hdnCountry.Value + " order by Currency", "Currency", "CurrencyID", "Select Currency");
                hdnCUrrencyId.Value= dtPrimaryInfo.Rows[0]["CurrencyID"].ToString();
                txtCurrency.Text= dtPrimaryInfo.Rows[0]["Currency"].ToString();
                ddlCurrency.SelectedValue = dtPrimaryInfo.Rows[0]["CurrencyID"].ToString();
                txtPhoneno1.Text = dtPrimaryInfo.Rows[0]["Phone1"].ToString();
                txtPhoneno2.Text = dtPrimaryInfo.Rows[0]["Phone2"].ToString();
                txtMobile.Text = dtPrimaryInfo.Rows[0]["Mobile"].ToString();
                //txtFax.Text = dtPrimaryInfo.Rows[0]["Fax"].ToString();
                txtCI_email.Text = dtPrimaryInfo.Rows[0]["EMail"].ToString();
                chkIsActive.Checked = (dtPrimaryInfo.Rows[0]["IsActive"].ToString() == "1");
                txtlatitude.Text= dtPrimaryInfo.Rows[0]["Latitude"].ToString();
                txtlongitude.Text = dtPrimaryInfo.Rows[0]["Longitude"].ToString();
                cbxSameAddress.Checked = (dtBillingInfo.Rows[0]["IsSameAddress"].ToString() == "True");


            }
            // Billing Address Information
            if (cbxSameAddress.Checked == true)
            {
                cbxSameAddress.Checked = true;
                ddlBilltype.SelectedValue = dtBillingInfo.Rows[0]["BillTypeID"].ToString();
                txtBillingAddressLine1.Text = txtAddressLine1.Text;
                txtBillingAddressLine2.Text = txtAddressLine2.Text;
                txtBillingCity.Text = txtCity.Text;
                txtBillingState.Text = txtState.Text;
                hdnBillingZip.Value = hdnZip.Value;
                txtBillingZip.Text = txtZip.Text;
                //ddlBillingCountry.SelectedValue = dtBillingInfo.Rows[0]["CountryMasterID"].ToString();
                hdnBillingCountry.Value = hdnCountry.Value;
                txtBillingCountry.Text = txtCountry.Text;
                ddlBillingCurrency.SelectedValue = ddlCurrency.SelectedValue;
                txtBillingCurrency.Text = txtCurrency.Text;
                hdnBillingState.Value = hdnStateId.Value;
                hdnBillingCity.Value = hdnCity.Value;
                hdnBillingCurrency.Value = hdnCUrrencyId.Value;
                txtBillPhoneno1.Text = txtPhoneno1.Text;
                txtBillPhoneno2.Text = txtPhoneno2.Text;
                txtBillingMobile.Text = txtMobile.Text;
                //txtBillingFax.Text = dtBillingInfo.Rows[0]["Fax"].ToString();
                txtBillEmail.Text = txtCI_email.Text;
                txtblatitude.Text = txtlatitude.Text;
                txtblongitude.Text = txtlongitude.Text;
                cbxSameAddress.Checked = (dtBillingInfo.Rows[0]["IsSameAddress"].ToString() == "True");
            }
            else
            {
            if (dtBillingInfo.Rows.Count > 0)
            {
                ddlBilltype.SelectedValue = dtBillingInfo.Rows[0]["BillTypeID"].ToString();
                if (dtBillingInfo.Rows[0]["IsLocked"].ToString() == "1")
                    cbxIsLock.Checked = true;
                else
                    cbxIsLock.Checked = false;
                txtBillingAddressLine1.Text = dtBillingInfo.Rows[0]["Address1"].ToString();
                txtBillingAddressLine2.Text = dtBillingInfo.Rows[0]["Address2"].ToString();
                txtBillingCity.Text = dtBillingInfo.Rows[0]["City"].ToString();
                txtBillingState.Text = dtBillingInfo.Rows[0]["State"].ToString();
                hdnBillingZip.Value = dtBillingInfo.Rows[0]["ZipCodeID"].ToString();
                txtBillingZip.Text = dtBillingInfo.Rows[0]["ZIP"].ToString();
                //ddlBillingCountry.SelectedValue = dtBillingInfo.Rows[0]["CountryMasterID"].ToString();
                hdnBillingCountry.Value = dtBillingInfo.Rows[0]["CountryMasterID"].ToString();
                txtBillingCountry.Text = dtBillingInfo.Rows[0]["BillingCountry"].ToString();
                hdnBillingCurrency.Value = dtBillingInfo.Rows[0]["CurrencyID"].ToString();
                ddlBillingCurrency.SelectedValue = dtBillingInfo.Rows[0]["CurrencyID"].ToString();
                txtBillingCurrency.Text= dtBillingInfo.Rows[0]["Currency"].ToString();
                hdnBillingState.Value = dtBillingInfo.Rows[0]["StateMasterID"].ToString();
                hdnBillingCity.Value = dtBillingInfo.Rows[0]["CityMasterID"].ToString();
                txtBillPhoneno1.Text = dtBillingInfo.Rows[0]["Phone1"].ToString();
                txtBillPhoneno2.Text = dtBillingInfo.Rows[0]["Phone2"].ToString();
                txtBillingMobile.Text = dtBillingInfo.Rows[0]["Mobile"].ToString();
                //txtBillingFax.Text = dtBillingInfo.Rows[0]["Fax"].ToString();
                txtBillEmail.Text = dtBillingInfo.Rows[0]["EMail"].ToString();
                txtblatitude.Text = dtBillingInfo.Rows[0]["Latitude_BI"].ToString();
                txtblongitude.Text = dtBillingInfo.Rows[0]["Longitude_BI"].ToString();
                cbxSameAddress.Checked = (dtBillingInfo.Rows[0]["IsSameAddress"].ToString() == "True");

                }
            }

            // Bank Deatils
            //if (dtBankInfo.Rows.Count > 0)
            //{
            //    txtBankName.Text = dtBankInfo.Rows[0]["BankName"].ToString();
            //    txtAccountNo.Text = dtBankInfo.Rows[0]["AccountNo"].ToString();
            //    txtBankAddress.Text = dtBankInfo.Rows[0]["BankAddress"].ToString();
            //    txtBankCity.Text = dtBankInfo.Rows[0]["City"].ToString();
            //    txtIBAN.Text = dtBankInfo.Rows[0]["IBAN"].ToString();
            //    ddlBankCurrency.SelectedValue = dtBankInfo.Rows[0]["CurrencyID"].ToString();
            //    ddlBankCountry.SelectedValue = dtBankInfo.Rows[0]["CountryMasterID"].ToString();
            //    txtBIC.Text = dtBankInfo.Rows[0]["BIC"].ToString();
            //    if (dtBankInfo.Rows[0]["IsDirectDebit"].ToString() == "1")
            //        cbxDirectDebit.Checked = true;
            //    else
            //        cbxDirectDebit.Checked = false;

            //}

            // Inovice
            if (dtInvoice.Rows.Count > 0)
            {
                cbxIsLock.Checked = (dtInvoice.Rows[0]["IsDeleted"].ToString() == "0");
                hifTenantActivityRateID.Value = dtInvoice.Rows[0]["TenantActivityRateID"].ToString();
            }
            else
                cbxIsLock.Checked = false;

            //// Contract Details
            //if (dtContractInfo.Rows.Count > 0)
            //{

            //    txtContract.Text = dtContractInfo.Rows[0]["TenantContract"].ToString();
            //    txtContractRemarks.Text = dtContractInfo.Rows[0]["Remarks"].ToString();
            //    txtContractFromDate.Text = dtContractInfo.Rows[0]["EffectiveFrom"].ToString();
            //    txtContractToDate.Text = dtContractInfo.Rows[0]["EffectiveTo"].ToString();
            //}

            //if (dtSpaceutilization.Rows.Count > 0)
            //{
            //    hifTenantSpaceUtilizationID.Value = dtSpaceutilization.Rows[0]["TenantSpaceUtilizationID"].ToString();
            //    if (dtSpaceutilization.Rows[0]["IsDeleted"].ToString() == "0")
            //    {
            //        ddlSpaceutilization.SelectedValue = dtSpaceutilization.Rows[0]["SpaceUtilizationID"].ToString();
            //    }
            //    else
            //        ddlSpaceutilization.SelectedValue = "-1";
            //}

            //}
            //else {
            //    resetError("Error in Data", true);
            //}
            ds.Dispose();


        }

        public void LoadDropDown(DropDownList ddllist, String SqlQry, String strListName, String strListValue, string defaultValue)
        {
            // Initially clear all DropDownList Items
            ddllist.Items.Clear();
            ddllist.Items.Add(new ListItem(defaultValue, "-1")); // Add Default value to the dropdown

            IDataReader rsList = DB.GetRS(SqlQry);

            while (rsList.Read())
            {

                ddllist.Items.Add(new ListItem(rsList[strListName].ToString(), rsList[strListValue].ToString()));

            }

            rsList.Close();
        }
        [WebMethod]
        public static string SetFile(int Tenantid, string filepath,int filetypeid)
        {

            string Status = "";
            int TenantID = 0;
            CustomPrincipal _cp = HttpContext.Current.User as CustomPrincipal;

            try
            {
                StringBuilder sb = new StringBuilder();
             
                sb.Append(" Exec [dbo].[sp_TPL_UpsertTenantFileAttachment] ");
                sb.Append(" ,@TenantID  =" + TenantID);
                sb.Append(" ,@FileTypeID  =" + filetypeid);
                sb.Append(" ,@ResourcePath  =" + TenantID);
                sb.Append(" ,@CreatedBy  =" + _cp.UserID.ToString());

                DB.ExecuteSQL(sb.ToString());
                Status = "success";
            }
            catch (Exception ex)
            {
                Status = "Failed";
            }
            return Status;
        }
        protected void lnkSavePrimaryInfo_Click(object sender, EventArgs e)
        {
            string val= HttpContext.Current.Request.Form[txtlatitude.UniqueID.Replace("_", "$")];
            cp = HttpContext.Current.User as CustomPrincipal;
            //string val = latitude.Text;
            //string val = txtlatitude.Text;
            //string val1 = va

            //if (ddlaccount.SelectedValue == "0" || txtCompanyName.Text == "" || txtCompanyRegistration.Text == "" || txtCompanyDBA.Text == "" || txtWebsite.Text == "" || txtEmail.Text == "" || ddlBusinessType.SelectedValue == "0" || ddlCountry.SelectedValue == "0" || ddlCurrency.SelectedValue == "0" || txtAddressLine1.Text == "" || txtAddressLine2.Text == "" || txtZip.Text == "" || ddlBilltype.SelectedValue == "0" || ddlCountry.SelectedValue == "0" || ddlCurrency.SelectedValue == "0" || txtBillEmail.Text == "" || txtBillingMobile.Text == "" || txtBillingAddressLine1.Text == "" || txtBillingAddressLine2.Text == "" || txtBillingZip.Text == "" || txtBankName.Text == "" || txtAccountNo.Text == "" || txtIBAN.Text == "" || txtBIC.Text == "" || txtBankAddress.Text == "" || txtBankCity.Text == "" || ddlBankCountry.SelectedValue == "0" || ddlCurrency.SelectedValue == "0")
            //{
            //    resetError("Please select Mandatory Fields", true);
            //    return;

            //}
            if (ddlaccount.SelectedValue == "-1" || txtCompanyName.Text.Trim() == "" || txtCompanyRegistration.Text.Trim() == "" || txtCompanyDBA.Text.Trim() == "" || ddlBusinessType.SelectedValue == "-1" ||  hdnCUrrencyId.Value == "-1" || txtAddressLine1.Text.Trim() == "" || ddlBilltype.SelectedValue == "-1" ||   txtBillEmail.Text.Trim() == "" || txtBillingMobile.Text.Trim() == "" || txtBillingAddressLine1.Text.Trim() == "" )
            {
                resetError("Please select mandatory fields", true);
                return;

            }
            if (txtCountry.Text.Trim() == "" || txtCurrency.Text.Trim() == "" || txtState.Text.Trim() == "" || txtCity.Text.Trim() == "" || txtZip.Text.Trim() == "" || txtBillingCountry.Text.Trim() == "" || txtBillingState.Text.Trim() == "" || txtBillingCity.Text.Trim() == "" || txtBillingZip.Text.Trim() == "") 
            {
                resetError("Please select mandatory fields", true);
                return;
            }
            string email = txtCI_email.Text;
           
            string Billemail = txtBillEmail.Text;
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);           
            Match match2 = regex.Match(Billemail);
            if (match.Success== false || match2.Success==false  )
            {
                resetError("Please enter valid Email fields", true);
                return;
            }

            string Pcpemail = txtEmail.Text;
            if (Pcpemail.Trim() != "" && Pcpemail.Trim() != null)
            {
               
                Match match1 = regex.Match(Pcpemail);
                if (match1.Success == false)
                {
                    resetError("Please enter valid Email fields", true);
                    return;
                }
            }

            //if (txtMobile.Text.Length < 10 || txtBillingMobile.Text.Length < 10)
            //{
            //    resetError("Please Enter Valid Mobile No", true);
            //    return;
            //}
            /*
            if (txtZip.Text.Length < 6 || txtBillingZip.Text.Length < 6)
            {
                resetError("Please Enter valid Zip", true);
                return;
            }
            */
            Page.Validate("vgTenantRegistration");
            Page.Validate("vgContract");
            //if (!Page.IsValid)
            //{
            //    return;
            //}
            string Website = txtWebsite.Text.Trim();
            if (Website!= "" && Website !="")
            {
               Page.Validate("validWebsite");
                if (!Page.IsValid)
                {
                    resetError("Please enter valid WebSite", true);
                    return;
                }
            }

            int TenantID = 0;
            TenantID = Convert.ToInt32(ViewState["tid"].ToString());
            //string tenantid1;

            //if (CommonLogic.QueryString("tid") != "")
            //{

            //    tenantid1 = CommonLogic.QueryString("tid");
            //}
            //else
            //{
            //    tenantid1 = null;
            //}
            

            int Applicable = 0;
            if (cbtaxapplicable.Checked == true)
            {
                Applicable = 1;
            }


            //int countOfExistCode = DB.GetSqlN("SELECT COUNT(*) N FROM TPL_Tenant WHERE AccountID="+ ddlaccount.SelectedValue + " AND TenantID!="+ TenantID + " AND TenantCode="+ DB.SQuote(txtCompanyDBA.Text) + " AND IsActive=1 AND IsDeleted=0");
            int countOfExistCode = DB.GetSqlN("[dbo].[USP_MST_CheckExistTenantCode] @AccountID=" + ddlaccount.SelectedValue + ",@TenantID=" + TenantID + ",@TenantCode=" + DB.SQuote(txtCompanyDBA.Text) + "");
            if (countOfExistCode > 0)
            {
                resetError("Tenant Code already exists under this Account", true);
                return;
            }
            StringBuilder Query = new StringBuilder();
            Query.Append(" Declare @TTid int ");
            Query.Append(" EXEC sp_TPL_UpsertTenantRegistration");
            Query.Append(" @AccountID  =" + ddlaccount.SelectedValue);
            Query.Append(" ,@TenantID  =" + TenantID);
            Query.Append(" ,@TenantName  =" + DB.SQuote(txtCompanyName.Text));
            Query.Append(" ,@CompanyDBA  =" + (txtCompanyDBA.Text == "" ? "NULL" : DB.SQuote(txtCompanyDBA.Text)));
            Query.Append(" ,@BusinessTypeID  =" + ddlBusinessType.SelectedValue);
            Query.Append(" ,@TenantRegistrationNo  =" + DB.SQuote(txtCompanyRegistration.Text));
            Query.Append(" ,@PCPFirstName  =" + (txtFirstName.Text == "" ? "NULL" : DB.SQuote(txtFirstName.Text)));
            Query.Append(" ,@PCPLastName  =" + (txtLastName.Text == "" ? "NULL" : DB.SQuote(txtLastName.Text)));
            Query.Append(" ,@PCPContactNo  =NULL");
            Query.Append(" ,@PCPEmail  =" + DB.SQuote(txtEmail.Text));
            Query.Append(" ,@Website  =" + DB.SQuote(txtWebsite.Text));
            Query.Append(" ,@IsInsurance  =" + (cbxIsInsurence.Checked ? "1" : "0"));
            Query.Append(" ,@CreatedBy  =" + cp.UserID);
            //Query.Append(" ,@AddressBookTypeID_CI  =1");
            Query.Append(" ,@Address1_CI  =" + DB.SQuote(txtAddressLine1.Text));
            Query.Append(" ,@Address2_CI  =" + DB.SQuote(txtAddressLine2.Text));
            Query.Append(" ,@City_CI  =" + (txtCity.Text == "" ? "NULL" : DB.SQuote(txtCity.Text)));
            Query.Append(" ,@State_CI  =" + (txtState.Text == "" ? "NULL" : DB.SQuote(txtState.Text)));
            Query.Append(" ,@ZIP_CI  =" + DB.SQuote(txtZip.Text));
            //Query.Append(" ,@CountryMasterID_CI  =   " + ddlCountry.SelectedValue);
            Query.Append(" ,@CountryMasterID_CI  =   " + hdnCountry.Value);
            //Query.Append(" ,@CurrencyID_CI  =" + ddlCurrency.SelectedValue);
            Query.Append(" ,@CurrencyID_CI  =" + hdnCUrrencyId.Value);
            Query.Append(" ,@Phone1_CI  =" + (txtPhoneno1.Text == "" ? "NULL" : DB.SQuote(txtPhoneno1.Text)));
            Query.Append(" ,@Phone2_CI  =" + (txtPhoneno2.Text == "" ? "NULL" : DB.SQuote(txtPhoneno2.Text)));
            Query.Append(" ,@Mobile_CI  =" + DB.SQuote(txtMobile.Text));
            Query.Append(" ,@Fax_CI  =NULL");//+DB.SQuote(txtFax.Text));
            Query.Append(" ,@EMail_CI  =" + DB.SQuote(txtCI_email.Text));
            Query.Append(", @IsTaxApplicable = " + Applicable);
            Query.Append(", @GSTNumber = " + DB.SQuote(txtGstNumber.Text));
            Query.Append(", @Latitude_CI = " + (txtlatitude.Text == "" ? "NULL" : DB.SQuote(txtlatitude.Text)));
            Query.Append(", @Longitude_CI = " + (txtlongitude.Text == "" ? "NULL" : DB.SQuote(txtlongitude.Text)));
            Query.Append(" ,@ZipCodeId_CI  =   " + hdnZip.Value);
            Query.Append(" ,@IsSameAddress_CI  =   " + (cbxSameAddress.Checked ? "1" : "0"));
            // Page.


            // Billing Address Information

            if (cbxSameAddress.Checked == true)
            {
                txtBillingAddressLine1.Text = txtAddressLine1.Text;
                txtBillingAddressLine2.Text = txtAddressLine2.Text;
                txtBillingCity.Text = txtCity.Text;
                txtBillingState.Text = txtState.Text;
                hdnBillingZip.Value = hdnZip.Value;
                txtBillingZip.Text = txtZip.Text;
                //ddlBillingCountry.SelectedValue = dtBillingInfo.Rows[0]["CountryMasterID"].ToString();
                hdnBillingCountry.Value = hdnCountry.Value;
                txtBillingCountry.Text = txtCountry.Text;
                //ddlBillingCurrency.SelectedValue = ddlCurrency.SelectedValue;
                txtBillingCurrency.Text = txtCurrency.Text;
                hdnBillingState.Value = hdnStateId.Value;
                hdnBillingCurrency.Value = hdnCUrrencyId.Value;
                hdnBillingCity.Value = hdnCity.Value;
                txtBillPhoneno1.Text = txtPhoneno1.Text;
                txtBillPhoneno2.Text = txtPhoneno2.Text;
                txtBillingMobile.Text = txtMobile.Text;
                //txtBillingFax.Text = dtBillingInfo.Rows[0]["Fax"].ToString();
                txtBillEmail.Text = txtCI_email.Text;
                txtblatitude.Text = txtlatitude.Text;
                txtblongitude.Text = txtlongitude.Text;
                Query.Append(" ,@IsSameAddress_BI  =   " + (cbxSameAddress.Checked ? "1" : "0"));
            }
            else
            {
                txtBillingAddressLine1.Text = txtBillingAddressLine1.Text;
                txtBillingAddressLine2.Text = txtBillingAddressLine2.Text;
                txtBillingCity.Text = txtBillingCity.Text;
                txtBillingState.Text = txtBillingState.Text;
                hdnBillingZip.Value = hdnBillingZip.Value;
                txtBillingZip.Text = txtBillingZip.Text;
                //ddlBillingCountry.SelectedValue = dtBillingInfo.Rows[0]["CountryMasterID"].ToString();
                hdnBillingCountry.Value = hdnBillingCountry.Value;
                txtBillingCountry.Text = txtBillingCountry.Text;
                ddlBillingCurrency.SelectedValue = ddlBillingCurrency.SelectedValue;
                hdnBillingCurrency.Value = hdnBillingCurrency.Value;
                txtBillingCurrency.Text = txtBillingCurrency.Text;
                hdnBillingState.Value = hdnBillingState.Value;
                hdnBillingCity.Value = hdnBillingCity.Value;
                txtBillPhoneno1.Text = txtBillPhoneno1.Text;
                txtBillPhoneno2.Text = txtBillPhoneno2.Text;
                txtBillingMobile.Text = txtBillingMobile.Text;
                //txtBillingFax.Text = dtBillingInfo.Rows[0]["Fax"].ToString();
                txtBillEmail.Text = txtBillEmail.Text;
                txtblatitude.Text = txtblatitude.Text;
                txtblongitude.Text = txtblongitude.Text;
                Query.Append(" ,@IsSameAddress_BI  =   " + (cbxSameAddress.Checked ? "1" : "0"));
            }


            //Query.Append(" ,@AddressBookTypeID_BI  =2");
            Query.Append(" ,@Address1_BI  =" + DB.SQuote(txtBillingAddressLine1.Text));
            //Query.Append(" ,@TenantSpaceUtilizationID  =" + DB.SQuote(hifTenantSpaceUtilizationID.Value));
            //Query.Append(" ,@SpaceUtilizationID =" + DB.SQuote(ddlSpaceutilization.SelectedValue));
            Query.Append(" ,@Address2_BI  =" + DB.SQuote(txtBillingAddressLine2.Text));
            Query.Append(" ,@City_BI  =" + (txtBillingCity.Text == "" ? "NULL" : DB.SQuote(txtBillingCity.Text)));
            Query.Append(" ,@State_BI  =" + (txtBillingState.Text == "" ? "NULL" : DB.SQuote(txtBillingState.Text)));
            Query.Append(" ,@ZIP_BI  =" + DB.SQuote(txtBillingZip.Text));
            //Query.Append(" ,@CountryMasterID_BI  =" + ddlBillingCountry.SelectedValue);
            Query.Append(" ,@CountryMasterID_BI  =" + hdnBillingCountry.Value);
            //Query.Append(" ,@CurrencyID_BI  =" + ddlBillingCurrency.SelectedValue);
            Query.Append(" ,@CurrencyID_BI  =" + hdnBillingCurrency.Value);
            Query.Append(" ,@Phone1_BI  =" + (txtBillPhoneno1.Text == "" ? "NULL" : DB.SQuote(txtBillPhoneno1.Text)));
            Query.Append(" ,@Phone2_BI  =" + (txtBillPhoneno2.Text == "" ? "NULL" : DB.SQuote(txtBillPhoneno2.Text)));
            Query.Append(" ,@Mobile_BI  =" + DB.SQuote(txtBillingMobile.Text));
            Query.Append(" ,@Fax_BI  =NULL");// + DB.SQuote(txtBillingFax.Text));
            Query.Append(" ,@EMail_BI  =" + DB.SQuote(txtBillEmail.Text));
            Query.Append(" ,@Latitude_BI  =" + (txtblatitude.Text == "" ? "NULL" : DB.SQuote(txtblatitude.Text)));
            Query.Append(" ,@Longitude_BI  =" + (txtblongitude.Text == "" ? "NULL" : DB.SQuote(txtblongitude.Text)));
            Query.Append(" ,@ZipCodeId_BI  =" + DB.SQuote(hdnBillingZip.Value));


            // Bank Deatils

            //Query.Append(" ,@BankName  =" + DB.SQuote(txtBankName.Text));
            //Query.Append(" ,@AccountNo  =" + DB.SQuote(txtAccountNo.Text));
            //Query.Append(" ,@BankAddress  =" + DB.SQuote(txtBankAddress.Text));
            //Query.Append(" ,@City  =" + DB.SQuote(txtBankCity.Text));
            //Query.Append(" ,@IBAN  =" + DB.SQuote(txtIBAN.Text));
            //Query.Append(" ,@BIC  =" + DB.SQuote(txtBIC.Text));

            ////Query.Append(" ,@BIC  ="+);
            //Query.Append(" ,@CurrencyID  =" + ddlBankCurrency.SelectedValue);
            //Query.Append(" ,@CountryID  =" + ddlBankCountry.SelectedValue);
            //Query.Append(" ,@IsDirectDebit  =" + (cbxDirectDebit.Checked ? "1" : "0"));


            //// Contract Details
            //string hifTenantContractID, ContractName, FromDate, ToDate, Remarks, SquareUnits = "NULL";

            //if (gvContract.Rows.Count > 0 && CommonLogic.QueryString("tid") == "")
            //{
            //    GridViewRow gvRow = gvContract.Rows[0];
            //    hifTenantContractID = ((HiddenField)gvRow.FindControl("hifTenantContractID")).Value;
            //    ContractName = ((TextBox)gvRow.FindControl("txtContractName")).Text;
            //    FromDate = ((TextBox)gvRow.FindControl("txtEffectiveFrom")).Text;
            //    ToDate = ((TextBox)gvRow.FindControl("txtEffectiveTo")).Text;
            //    Remarks = ((TextBox)gvRow.FindControl("txtRemarks")).Text;
            //    if (((CheckBox)gvRow.FindControl("cbSpaceRental")).Checked)
            //    {

            //        SquareUnits = ((TextBox)gvRow.FindControl("txtSquareUnits")).Text == "" ? "0" : SquareUnits;

            //    }



            //}
            //else
            //{
            //    hifTenantContractID = "";
            //    ContractName = "";
            //    FromDate = "01/01/1991";
            //    ToDate = "01/01/1991";
            //    Remarks = "";
            //}
            //string imgpath = null;
            //if (FUAttachment.HasFile)
            //{
            //    string path = string.Concat((Server.MapPath("~/TenantFiles/" + FUAttachment.FileName)));
            //    FUAttachment.PostedFile.SaveAs(path);
            //    string ext = Path.GetExtension(path);
            //    imgpath = Path.GetFileName(path);

            //    string[] pathArr = path.Split('\\');
            //    string[] fileArr = pathArr.Last().Split('.');
            //    string fileName = fileArr.Last().ToString();
            //}
            //else
            //{
            //    imgpath = txtimage.Text;
            //}
            //if (ContractName != null)
            //{
            //    Query.Append(" ,@TenantContract =" + DB.SQuote(ContractName));
            //    Query.Append(" ,@Remarks =" + DB.SQuote(Remarks));
            //    Query.Append(" ,@EffectiveFrom =" + DB.SQuote(DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy")));
            //    Query.Append(" ,@EffectiveTo =" + DB.SQuote(DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy")));
            //    Query.Append(" ,@SquareUnits=" + SquareUnits);
            // Invoice Setting Details

            Query.Append(" ,@BillTypeID =" + ddlBilltype.SelectedValue);
                //Query.Append(" ,@FileTypeID =" + ddldocumenttype.SelectedValue);
                //Query.Append(" ,@ResourcePath =" + txtimage.Text);
                //Query.Append("--@BillTypeDescription =");
                //Query.Append(" ,@IsLocked =" + (cbxIsLock.Checked?"1":"0"));
                Query.Append(",@Invoice=" + (cbxIsLock.Checked ? 1 : 0) + ",");
                Query.Append("@TenantActivityRateID =" + hifTenantActivityRateID.Value + ",");
                Query.Append("@InvoiceFrom =" + DB.SQuote(DateTime.Now.ToString("MM/dd/yyyy")) + ",");
                Query.Append("@InvoiceTo =" + DB.SQuote(CaliculateToDate(ddlBilltype.SelectedValue).ToString("MM/dd/yyyy")) + ",");
                Query.Append("@Active=" + (chkIsActive.Checked ? "1" : "0") + ",");
                Query.Append(" @Tid=@TTid output select @TTid n");
            //}
            
            try
            {
                ViewState["tid"] = DB.GetSqlN(Query.ToString());
                if (lnkSavePrimaryInfo.Text == "Update" + CommonLogic.btnfaUpdate)
                {
                    //  Response.Redirect("TenantRegistration.aspx?tid=" + ViewState["tid"] + "&status=Updatesuccess", false);
                    Response.Redirect("TenantList.aspx", false);

                }
                else
                {
                    Response.Redirect("TenantRegistration.aspx?tid=" + ViewState["tid"] + "&status=Createsuccess", false);
                }
                gvContract.Columns[5].Visible = true;
                lnkAddContract.Enabled = true;
                gvContract.EditIndex = -1;
                LoadContract(LoadContract());
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Violation of UNIQUE KEY constraint 'IX_TPL_TenantName'. Cannot insert duplicate key in object 'dbo.TPL_Tenant"))
                {
                    resetError("Tenant name already exists", true);
                }
                else
                {
                    resetError("Error while updating "+ex.Message, true);
                }
                CommonLogic.createErrorNode(cp.UserID.ToString(), this.Page.ToString(), ex.Source, ex.Message,ex.StackTrace);
            }
        }

        protected void resetError(string error, bool isError)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showStickyToast", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);
        }
        protected void resetErrorContract(string error, bool isError)
        {

            string str = "<font class=\"noticeMsg\">NOTICE:</font>";
            if (isError)
                str = "<font class=\"errorMsg\">ERROR:</font>";

            if (error.Length > 0)
                str += error + "";
            else
                str = "";

            ltContracterrorMsg.Text = str + "</font>";

        }
        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("TenantList.aspx");
        }

        protected void lnkAddContract_Click(object sender, EventArgs e)
        {
           
            gvContract.EditIndex = 0;
            gvContract.PageIndex = 0;
            DataSet ds = LoadContract();
            DataRow row = ds.Tables[0].NewRow();
            row["TenantContractID"] = 0;
            ds.Tables[0].Rows.InsertAt(row, 0);
            LoadContract(ds);
            gvContract.Visible = true;                          
        }
        protected void lnksave_Click(object sender, EventArgs e)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            string selectedValue = "";
            foreach (ListItem item in WHChkBoxList.Items)
            {
                if (item.Selected)
                {
                    selectedValue += item.Value + ",";
                }
            }
            try
            {
                StringBuilder cMdUpdateProductionReturns = new StringBuilder();
                cMdUpdateProductionReturns.Append("DECLARE @Result int   ");
                cMdUpdateProductionReturns.Append("EXEC [dbo].[sp_TPL_UpsertTenantWarehouses]  ");
                cMdUpdateProductionReturns.Append("@ProductionOrderHeaderID=" + ViewState["HeaderID"]);
                cMdUpdateProductionReturns.Append(",@TenantID=" + CommonLogic.QueryString("tid1"));
                cMdUpdateProductionReturns.Append(",@productionorderIDs=" + DB.SQuote(selectedValue.ToString()));
                cMdUpdateProductionReturns.Append(",@CreatedBy=" + cp.UserID);
                cMdUpdateProductionReturns.Append(",@TenantWarehouseID=1");
                //cMdUpdateProductionReturns.Append(",@MaterialMasterUoMID=" + MaterialUomID);


            }
            catch(Exception ex)
            {
            }
            }

        protected void gvContract_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;

            if ((e.Row.RowType == DataControlRowType.DataRow) && (e.Row.RowIndex == gvContract.EditIndex))
                {
                    if(gvContract.EditIndex ==0)
                    {
                        ((CheckBox)e.Row.FindControl("cbkactive")).Checked = true;
                    }
                    //if (gvContract.EditIndex == -1)
                    //{
                    //    gvContract.ShowFooter = false;
                    //    gvContract.Columns[8].Visible = false;
                    //    ((CheckBox)e.Row.FindControl("cbkactive")).Enabled = true;
                    //}
                    string tcid = ((HiddenField)e.Row.FindControl("hifTenantContractID")).Value;
                if (tcid != "0")
                {
                    int isactive = MRLWMSC21Common.DB.GetSqlN("select convert(int, IsActive) as N from TPL_Tenant_Contract where TenantContractID=" + tcid + "");
                    if (isactive == 0)
                    {
                        ((CheckBox)e.Row.FindControl("cbkactive")).Checked = false;
                    }
                    else
                    {
                        ((CheckBox)e.Row.FindControl("cbkactive")).Checked = true;
                    }

                    ((CheckBox)e.Row.FindControl("cbkactive")).Enabled = true;
                }
                else
                {
                    ((CheckBox)e.Row.FindControl("cbkactive")).Enabled = false;
                }
                    if (gvContract.Rows.Count <= 0)
                    {
                        gvContract.ShowFooter = false;
                        gvContract.Columns[8].Visible = false;                   

                    }
                    else
                    {
                        gvContract.ShowFooter = true;
                        gvContract.Columns[8].Visible = true;
                    }
                    DataRow dr = ((DataRowView)e.Row.DataItem).Row;

                    if (dr["Value"].ToString() != "")
                    {
                        ((Label)e.Row.FindControl("lblsquareunits")).Visible = true;
                        ((TextBox)e.Row.FindControl("txtSquareUnits")).Visible = true;
                        ((CheckBox)e.Row.FindControl("cbSpaceRental")).Checked = true;
                        ((CheckBox)e.Row.FindControl("cbkactive")).Checked = true;
                    

                    }
                }
            

            }

        protected void gvContract_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvContract.EditIndex = e.NewEditIndex;
            LoadContract(LoadContract());
        }

        protected void gvContract_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvContract.EditIndex = -1;
            LoadContract(LoadContract());
            //lnkAddContract.Enabled = true;
            //Response.Redirect("TenantRegistration.aspx");
        }

        protected void gvContract_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            Page.Validate("vgContract");
          //  Page.Validate("UpdatePODetails");
            if (!IsValid)
            {
                resetError("Please enter all mandatory fields", true);
                  return;
            }
            //    if (!Page.IsValid)
            //{
            //    resetError("Please enter all mandatory fields", true);
            //    return;
            //}
            string uploadedFile = null;
            


            string file = null;
            GridViewRow gvRow = gvContract.Rows[e.RowIndex];
            string hifTenantContractID=((HiddenField)gvRow.FindControl("hifTenantContractID")).Value;
            string ContractName = ((TextBox)gvRow.FindControl("txtContractName")).Text;
            string FromDate = ((TextBox)gvRow.FindControl("txtEffectiveFrom")).Text;
            string ToDate = ((TextBox)gvRow.FindControl("txtEffectiveTo")).Text;
            string Remarks = ((TextBox)gvRow.FindControl("txtRemarks")).Text;
            string SpaceRental = (((CheckBox)gvRow.FindControl("cbSpaceRental")).Checked?"1":"0");
            string SquareUnits = ((TextBox)gvRow.FindControl("txtSquareUnits")).Text;
            string path = ((Literal)gvRow.FindControl("ltpath1")).Text;
            HiddenField hifwarehouseID = (HiddenField)gvRow.FindControl("hifWarehouseID");
            string chactive = (((CheckBox)gvRow.FindControl("cbkactive")).Checked ? "1" : "0");       
           
            FileUpload fuCOA = (FileUpload)gvRow.FindControl("FUattachment");
            string name = Path.GetFileNameWithoutExtension(fuCOA.FileName.ToString());
            
            //int tenantcontid = DB.GetSqlN("SELECT MAX(TenantContractID) AS N FROM TPL_Tenant_Contract where TenantContract='"+ContractName+"' and TenantID= "+ ViewState["tid"]+" and IsActive=1 and isdeleted=0 and TenantContractID!="+Convert.ToInt32(hifTenantContractID));
            int tenantcontid = DB.GetSqlN(" [dbo].[USP_MST_GetTenantContractID] @TenantContractID=" + Convert.ToInt32(hifTenantContractID) + ", @TenantID=" + ViewState["tid"] + ",@TenantContract = '" + ContractName + "'");


           
            if (tenantcontid != 0)
            {
                resetError("Tenant Contract already exists", true);
                return;
            }
            //DateTime df = Convert.ToDateTime(FromDate.ToString());
            //DateTime dt = Convert.ToDateTime(ToDate.ToString());

            //if (df >= dt)
            //{
            //    resetError("Effective ToDate is Greater than Effective FromDate", true);
            //    return;
            //}

             int contractid = 0;
            if (hifTenantContractID != null && hifTenantContractID != "0")
            {
                contractid = Convert.ToInt32(hifTenantContractID);
                file = path;


            }
            else {
                int Count = 0;
                Count = DB.GetSqlN("SELECT MAX(TenantContractID) AS N FROM TPL_Tenant_Contract");
                Count++;
                contractid = Count;
            }

            if (fuCOA.HasFile)
            {
                bool upload = true;
                file = name + DateTime.Now.ToString("yyyyMMddTHHmmss") + Path.GetExtension(fuCOA.FileName.ToString());
                string fleUpload = Path.GetExtension(fuCOA.FileName.ToString());
                String Savepath = System.Web.HttpContext.Current.Server.MapPath("~/") + "TenantFiles" + "/" + tid1 + "/" + contractid;

                if (fuCOA.PostedFile.ContentLength > 5000000)
                {
                    resetError("File Should not Exceed 5MB", true);
                    return;

                }
                    var directory = new DirectoryInfo(Savepath);
                if (Directory.Exists(Savepath))
                {
                    string[] Attachmentlist=Directory.GetFiles(Savepath);

                    for (int i = 0; i<Attachmentlist.Length; i++)
                    {
                        if (System.IO.File.Exists(Attachmentlist[i]))
                        {
                            System.IO.File.Delete(Attachmentlist[i]); // DELETE THE FILE BEFORE CREATING A NEW ONE.
                        }

                    }
                    }
                    if (!Directory.Exists(Savepath))
                    Directory.CreateDirectory(Savepath);

             
                fuCOA.SaveAs(Savepath + @"\" + file);


                //fuCOA.SaveAs(Server.MapPath("~/TenantFiles/" + fuCOA.FileName.ToString()));
                //    uploadedFile = (Server.MapPath("~/TenantFiles/" + fuCOA.FileName.ToString()));
                //file = fuCOA.FileName.ToString();
                }
            if (Convert.ToDateTime(FromDate) > Convert.ToDateTime(ToDate))
            {
                resetError("Effective From Should not exceed Effective To", true);
                //lblSerialNo.Style["display"] = "block";
                return;
            }
            FromDate = DB.SQuote(CommonLogic.GetConfigValue(Convert.ToInt16(ViewState["tid"]), "DateFormat") == "dd-M-yyyy" ? DateTime.ParseExact(FromDate, "dd-M-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : FromDate);
            ToDate = DB.SQuote(CommonLogic.GetConfigValue(Convert.ToInt16(ViewState["tid"]), "DateFormat") == "dd-M-yyyy" ? DateTime.ParseExact(ToDate, "dd-M-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : ToDate);
            //if (dt.Rows[i]["MfgDate"].ToString() != "" && dt.Rows[i]["ExpDate"].ToString() != "")
            //{
            

            //}
            if (ViewState["tid"].ToString() != "0")
            {
                StringBuilder query = new StringBuilder();
                query.Append("EXEC [dbo].[sp_TPL_UpsertTenantContract]  ");
                query.Append(" @TenantContractID=" + hifTenantContractID + ",");
                query.Append(" @TenantContract= " + DB.SQuote(ContractName)+ " ,");
                //query.Append(" @EffectiveFrom =" + DB.SQuote(DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy")) + ",");
                //query.Append(" @EffectiveTo =" + DB.SQuote(DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy")) + ",");

                query.Append(" @EffectiveFrom =" + FromDate + ",");
                //query.Append(" @EffectiveTo =" + DB.SQuote(DateTime.ParseExact(ToDate, "dd-M-yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy")) + ",");
                query.Append(" @EffectiveTo =" + ToDate + ",");

                query.Append(" @TenantID =" + ViewState["tid"] + ", ");
                query.Append(" @ResourcePath='"+file+"',");
                query.Append(" @Remarks=" + CommonLogic.IIF(Remarks == "", "NULL", DB.SQuote(Remarks)) + ",");
                query.Append("@WarehouseID=" + hifwarehouseID.Value+",");
                query.Append(" @SpaceRental=" + SpaceRental+",");
                query.Append(" @IsActive=" + chactive + ",");
                query.Append(" @SquareUnits=" + Convert.ToDecimal(ConvertNumberIfEmpty(SquareUnits))+",");
                query.Append(" @CreatedBy=" + cp.UserID);
                try
                {
                    DB.ExecuteSQL(query.ToString());
                    gvContract.EditIndex = -1;
                    gvContract.PageIndex = 0;
                    LoadContract(LoadContract());
                    middailogbox();
                    resetError("Successfully updated", false);
                    lblvieweattachment.Visible = true;

                }
                catch (Exception ex)
                {
                    resetError("Error while updating "+ex.Message, true);
                }
            }
            else
            { 

            }
        }
      
        private string ConvertNumberIfEmpty(string value)
        {
            return "0" + value;
        }

        protected void gvContract_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvContract.PageIndex = e.NewPageIndex;
            LoadContract(LoadContract());
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            string DeletedRecords = "";
            int count = 0;
            foreach (GridViewRow row in gvContract.Rows)
            {

                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox c = (CheckBox)row.FindControl("cbkTenantContractID");
                    var c1 = (CheckBox)row.FindControl("cbkTenantContractID");
                    if (c.Checked)
                    {
                        count++;
                    }
                }
                    if ((CheckBox)row.FindControl("cbkTenantContractID")!=null  && ((CheckBox)row.FindControl("cbkTenantContractID")).Checked)
                {
                    DeletedRecords += ((HiddenField)row.FindControl("hifDeleteTenantContractID")).Value + ",";
                }
             
            }
            if (count == 0)
            {
                resetError("Please select check box", true);
                return;
            }
            if (DeletedRecords != "")
            {
                StringBuilder query = new StringBuilder();
                query.Append("update TPL_Tenant_Contract set IsDeleted=1 where TenantContractID in (" + DeletedRecords.Substring(0, DeletedRecords.LastIndexOf(',')) + ")");//,DeletedOn=getdate()
                try
                {
                    DB.ExecuteSQL(query.ToString());
                    LoadContract(LoadContract());
                    middailogbox();
                    resetError("Successfully Deleted", false);
                }
                catch (Exception ex)
                {
                    resetError("Error while deleting", true);
                }
            }
        }

        private DataSet LoadContract()
        {
            DataSet ds = DB.GetDS(" EXEC [dbo].[sp_TPL_GetTenantContract] @TenantID=" + ViewState["tid"].ToString(), false);
            return ds;
        }
       
        private void LoadContract(DataSet ds)
        {
            gvContract.DataSource = ds;
            gvContract.DataBind();
            if (gvContract.Rows.Count > 0)
            {
               
                gvContract.ShowFooter = true;
                gvContract.Columns[8].Visible = true;
               
            }

        }


        private DateTime CaliculateToDate(string id) {
            //DateTime today = new DateTime();
            switch (id)
            {
                case "1":
                    return DateTime.Now.AddDays(15);

                case "2":
                    return DateTime.Now.AddDays(30);

                case "3":
                    return DateTime.Now.AddMonths(3);

                case "4":
                    return DateTime.Now.AddMonths(6);
                    
                case "5":
                    return DateTime.Now.AddYears(1);
            }
            return DateTime.Now;
        }

        protected void cbSpaceRental_CheckedChanged(object sender, EventArgs e)
        {
            //((CheckBox)sender).Parent.Controls[3].Controls[1].Visible = true;
            if (((CheckBox)sender).Checked)
            {
                
                int rowindex = gvContract.EditIndex;
                gvContract.Rows[rowindex].FindControl("lblsquareunits").Visible = true;
                gvContract.Rows[rowindex].FindControl("txtSquareUnits").Visible = true;
                //rfvsquareft.Visible = true;
               
            }
            else
            {
                int rowindex = gvContract.EditIndex;
                gvContract.Rows[rowindex].FindControl("lblsquareunits").Visible = false;
                gvContract.Rows[rowindex].FindControl("txtSquareUnits").Visible = false;
                //rfvsquareft.Visible = false;
            }
        }


        protected void btnupload_Click1(object sender, EventArgs e)
        {
            string imgpath = null;
            cp = HttpContext.Current.User as CustomPrincipal;
            if (FUAttachment.HasFile)
            {
                string path = string.Concat((Server.MapPath("~/TenantFiles/" + FUAttachment.FileName)));
                FUAttachment.PostedFile.SaveAs(path);
                string ext = Path.GetExtension(path);
                imgpath = Path.GetFileName(path);

                string[] pathArr = path.Split('\\');
                string[] fileArr = pathArr.Last().Split('.');
                string fileName = fileArr.Last().ToString();
            }
            else
            {
                imgpath = txtimage.Text;
            }
            if (ViewState["tid"].ToString() != "0")
            {
                StringBuilder query = new StringBuilder();
                query.Append("EXEC [dbo].[sp_MMT_UpsertTenantFileAttachment]  ");
                query.Append(" @ResourcePath=" + DB.SQuote(imgpath) + ",");
                query.Append(" @TenantID =" + ViewState["tid"] + ", ");
                query.Append(" @CreatedBy =" + cp.UserID.ToString() + ", ");

                

                //query.Append(" @CreatedBy=" + cp.UserID);
                try
                {
                    DB.ExecuteSQL(query.ToString());
                    gvContract.EditIndex = -1;
                    gvContract.PageIndex = 0;
                    LoadContract(LoadContract());
                    resetError("Successfully updated", false);
                }
                catch (Exception ex)
                {
                    resetError("Error while updating " + ex.Message, true);
                }
            }
            else
            {

            }

        }
        protected void middailogbox()
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            if (tid1 != null && tid1 != "")
            {
                lblvieweattachment.Text = "<a style=\"text-decoration:none;\" href=\"javascript:openDialog(\'View Attachment Files\')\">   View Attachments  <span  class=\"space fa fa-external-link\"></span> </a>";
                //String loacalfolder = TenantRootDir + DB.GetSqlS("select UniqueID AS S from GEN_Tenant where TenantID=" + TenantID) + MMTPath + mid;
                String loacalfolder = "TenantFiles";


                //String midSavepath = System.Web.HttpContext.Current.Server.MapPath("~/") + loacalfolder;
                String midSavepath = System.Web.HttpContext.Current.Server.MapPath("~\\" + loacalfolder); ;
                String sidpath = "";

                trvmaterialattachment.Nodes[0].ChildNodes.Clear();

                TreeNode trnInternalOrder;
                TreeNode trnsubInternalOrder;
               trvmaterialattachment.Nodes[0].Text = txtCompanyName.Text;
                //DB.GetSqlS("select Mcode as S from MMT_MaterialMaster where MaterialMasterID = " + mid + " and isactive=1 and isdeleted=0");
                IDataReader rsGetSupplierList = DB.GetRS("select tenct.TenantID,tenct.TenantContract,tenct.ResourcePath,tenct.TenantContractID from TPL_Tenant_Contract tenct where TenantID=" + tid1 + " and tenct.isactive=1 and tenct.isdeleted=0");

                while (rsGetSupplierList.Read())
                {
                    trnInternalOrder = new TreeNode();
                    trnInternalOrder.Expanded = false;
                    trnInternalOrder.Text = DB.RSField(rsGetSupplierList, "TenantContract");
                    string path= DB.RSField(rsGetSupplierList, "ResourcePath");
                    int contractid = DB.RSFieldInt(rsGetSupplierList, "TenantContractID");
                    sidpath = midSavepath + "\\" +tid1+"\\"+ DB.RSFieldInt(rsGetSupplierList, "TenantContractID")+"\\"+ path;
                  //  sidpath = midSavepath + "\\" + tid1 + "\\" + DB.RSFieldInt(rsGetSupplierList, "TenantContractID") + "\\" ;
                    //  sidpath = midSavepath + "\\" + path;
                    String Attachmentname = "";
                    string directoryName = Path.GetDirectoryName(sidpath);
                    //sidpath = "D:\\MRLWMSC21_3PL_SL\\MRLWMSC21\\TenantFiles\\2088\\3092\\Jellyfish20180102T152223.jpg";
                    if (Directory.Exists(directoryName))
                    {                      

                    string[] Attachmentlist = Directory.GetFiles(directoryName);

                    foreach (String Attachment in Attachmentlist)
                        {
                            Attachmentname = Path.GetFileName(Attachment);
                            trnsubInternalOrder = new TreeNode();
                            trnsubInternalOrder.Text = Attachmentname;
                            // trnsubInternalOrder.NavigateUrl = Attachmentname.EndsWith(".pdf") ? String.Format("DisplayPDF.aspx?sid={0}&filename={1}&tid={2}", DB.RSFieldInt(rsGetSupplierList, "TenantID"),Attachmentname,tid1) : "../" + loacalfolder + "/" + DB.RSFieldInt(rsGetSupplierList, "TenantID") + "/" + Attachmentname;
                           // trnsubInternalOrder.NavigateUrl = "../" + loacalfolder + "/" + Attachmentname;
                            trnsubInternalOrder.NavigateUrl = "../" + loacalfolder + "/" +tid1+"/"+contractid+"/"+Attachmentname;
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
                
            }
            else
                lblvieweattachment.Text = "";
        }
        [WebMethod]
        public static string SetFile(int tentid, string Resources)
        {
            string Status = "";
            cp1 = HttpContext.Current.User as CustomPrincipal;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" Exec [dbo].[sp_MMT_UpsertTenantFileAttachment] ");
                sb.Append(" @ResourcePath='" + Resources + "'");
                sb.Append(" ,@TenantID=" + tentid);
                sb.Append(" ,@CreatedBy=1");
                DB.ExecuteSQL(sb.ToString());
                Status = "success";
            }
            catch (Exception ex)
            {
                Status = "Failed";
            }
            return Status;
        }

        [WebMethod]
        public static string GetWarehouse()
        {
            string Json = "";
            try
            {
                DataSet ds = DB.GetDS("Exec [dbo].[TPL_Get_TenantWarehouses] @TenantID=0", false);
                Json = JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                Json = "Failed";
            }
            return Json;
        }
        [WebMethod]
        public static string SETWarehouses(int UserID, string Inxml)
        {
            string Status = "";
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" Exec [dbo].[USP_SET_TPL_TenantWarehouses] ");
                sb.Append(" @inputDataXml='" + Inxml + "'");
               // sb.Append(" ,@LoggedUserID=" + UserID);
                sb.Append(" ,@CreatedBy=" + UserID);
                            
                DB.ExecuteSQL(sb.ToString());
                Status = "success";
            }
            catch (Exception ex)
            {
                Status = "Failed";
            }
            return Status;
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
        
        [WebMethod]
        public static string GetUserList(int Accountid, int tenantid)
        {


            try
            {


                DataSet ds = DB.GetDS("[dbo].[sp_GetUserList] @TenantId=" + tenantid, false);
                //foreach (DataRow row in ds.Tables[0].Rows)
                //{
                //    userdata.Add(new User()
                //    {
                //        UserID = Convert.ToInt32(row["UserID"]),
                //        UserTypeID = Convert.ToInt32(row["UserTypeID"]),
                //        AccountID=Convert.ToInt32(row["AccountID"]),
                //        TenantID= Convert.ToInt32(row["TenantID"]),
                //        FirstName= row["UserTypeID"].ToString(),
                //        LastNane = row["UserTypeID"].ToString(),
                //        MiddleName= row["UserTypeID"].ToString(),
                //        FullName= row["UserTypeID"].ToString(),
                //        EMPCode = row["UserTypeID"].ToString(),
                //        Gender= Convert.ToInt32(row["UserID"]),
                //        Email= row["UserTypeID"].ToString(),
                //        AltEmail1= row["UserTypeID"].ToString(),
                //        AltEmail2= row["UserTypeID"].ToString(),
                //        Password= row["UserTypeID"].ToString(),
                //        Mobile = row["UserTypeID"].ToString(),
                //        Isactive= Convert.ToBoolean(row["UserTypeID"]),
                //        UserType= row["UserTypeID"].ToString(),
                //        AccountName= row["UserTypeID"].ToString(),
                //        tenantName= row["UserTypeID"].ToString(),
                //        rolesinformation= row["UserTypeID"].ToString(),



                //    });
                //}

                return DataTableToJSONWithJSONNet(ds.Tables[0]);
            }
            catch (Exception e)
            {
                return null;
            }

        }
        [WebMethod]
        public static List<DropDownData> GetUserTypes()
        {
            List<DropDownData> olst = new List<DropDownData>();
            DataSet ds = DB.GetDS("select UserTypeID,UserType from GEN_UserType where IsActive=1 and IsDeleted=0 and UserTypeID=3", false);
            try
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    olst.Add(new DropDownData()
                    {
                        ID = Convert.ToInt32(row["UserTypeID"]),
                        Name = (row["UserType"]).ToString()

                    });
                }
            }
            catch (Exception e)
            {
            }
            return olst;
        }
        [WebMethod]
        public static List<DropDownData> GetAccount(int tenantid)
        {
          
            List<DropDownData> olst = new List<DropDownData>();
            DataSet ds = DB.GetDS("select AccountID,Account+' - '+AccountCode account from GEN_Account where AccountID=( select AccountID from TPL_Tenant where TenantID=" + tenantid+")", false);
            try
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    olst.Add(new DropDownData()
                    {
                        ID = Convert.ToInt32(row["AccountID"]),
                        Name = (row["account"]).ToString()

                    });
                }
            }
            catch (Exception e)
            {
            }
            return olst;
        }
        [WebMethod]
        public static List<DropDownData> GetTenants(int tenantid)
        {
            List<DropDownData> olst = new List<DropDownData>();
            DataSet ds = DB.GetDS("select TenantID,TenantName+' - '+TenantCode tenant from TPL_Tenant where IsDeleted=0 and IsActive=1 and tenantid=" + tenantid, false);
            try
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    olst.Add(new DropDownData()
                    {
                        ID = Convert.ToInt32(row["TenantID"]),
                        Name = (row["tenant"]).ToString()

                    });
                }
            }
            catch (Exception e)
            {
            }
            return olst;
        }
        [WebMethod]
        public static List<DropDownData> GetRoleData(int usertypeid)
        {
            List<DropDownData> olst = new List<DropDownData>();
            DataSet ds = DB.GetDS("select UserRoleID,UserRole from GEN_UserRole where IsDeleted=0 and IsActive=1 and (0=" + usertypeid + " or UserRoleTypeID=" + usertypeid + ")", false);
            try
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    olst.Add(new DropDownData()
                    {
                        ID = Convert.ToInt32(row["UserRoleID"]),
                        Name = (row["UserRole"]).ToString()

                    });
                }
            }
            catch (Exception e)
            {
            }
            return olst;
        }
        [WebMethod]
        public static List<DropDownData> GetWareHouseData(int tenantid)
        {
            List<DropDownData> olst = new List<DropDownData>();
            DataSet ds = DB.GetDS("select WarehouseID,WHName+' - '+WHCode warehouse from GEN_Warehouse where AccountID=(select AccountID from TPL_Tenant where TenantID="+ tenantid + ")", false);
            try
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    olst.Add(new DropDownData()
                    {
                        ID = Convert.ToInt32(row["WarehouseID"]),
                        Name = (row["warehouse"]).ToString()

                    });
                }
            }
            catch (Exception e)
            {
            }
            return olst;
        }
        [WebMethod]
        public static int UpsertUserData(MRLWMSC21.TPL.NewAccount.User obj)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            int isactive = obj.Isactive == true ? 1 : 0;
            string encriptpwd = DB.SQuote(Encrypt.EncryptData(CommonLogic.Application("EncryptKey"), obj.Password.Trim()));
            int userid = DB.GetSqlN("DECLARE @UpdateUserID int; Exec [dbo].[sp_GEN_UpsertUsers] @UserID=" + obj.UserID + ",@TenantID=" + obj.TenantID + ",@FirstName=" + DB.SQuote(obj.FirstName) + ",@LastName=" + DB.SQuote(obj.LastNane) + ",@MiddleName=" + DB.SQuote(obj.MiddleName) + ",@Email=" + DB.SQuote(obj.Email) +
                 ",@AlternateEmail1=" + DB.SQuote(obj.AltEmail1) + ",@AlternateEmail2=" + DB.SQuote(obj.AltEmail2) + ",@Password=" + DB.SQuote(obj.Password) + ",@UserRoleIDs=" + DB.SQuote(obj.Roles) + ",@WarehouseIDs=" + DB.SQuote(obj.warehouses) + ",@IsActive=" + isactive +
                 ",@EnPassword=" + encriptpwd + ",@Mobile=" + DB.SQuote(obj.Mobile) + ",@CreatedBy=" + cp1.UserID + ",@EmployeeCode=" + DB.SQuote(obj.EMPCode) + ",@AccountID=" + obj.AccountID + ",@UserTypeID=" + obj.UserTypeID + ",@NewUserID = @UpdateUserID OUTPUT Select @UpdateUserID as N");
            return userid;
            //return 1;

        }
        [WebMethod]
        public static List<string> setUserData(string obj)
        {
            List<string> lst = new List<string>();

            DataSet roles = DB.GetDS("select value from dbo.udf_Split(" + DB.SQuote(obj) + ", ',')", false);
            foreach (DataRow row in roles.Tables[0].Rows)
            {
                lst.Add((row["value"]).ToString());
            }
            return lst;
        }

        protected void cbxSameAddress_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxSameAddress.Checked == true)
            {
                txtBillingAddressLine1.Text = txtAddressLine1.Text;
                txtBillingAddressLine2.Text = txtAddressLine2.Text;
                txtBillingCity.Text = txtCity.Text;
                txtBillingState.Text = txtState.Text;
                hdnBillingZip.Value = hdnZip.Value;
                txtBillingZip.Text = txtZip.Text;
                //ddlBillingCountry.SelectedValue = dtBillingInfo.Rows[0]["CountryMasterID"].ToString();
                hdnBillingCountry.Value = hdnCountry.Value;
                txtBillingCountry.Text = txtCountry.Text;
                //ddlBillingCurrency.SelectedValue = ddlCurrency.SelectedValue;
                txtBillingCurrency.Text = txtCurrency.Text;
                hdnBillingState.Value = hdnStateId.Value;
                hdnBillingCity.Value = hdnCity.Value;
                txtBillPhoneno1.Text = txtPhoneno1.Text;
                txtBillPhoneno2.Text = txtPhoneno2.Text;
                txtBillingMobile.Text = txtMobile.Text;
                //txtBillingFax.Text = dtBillingInfo.Rows[0]["Fax"].ToString();
                txtBillEmail.Text = txtCI_email.Text;
               txtblatitude.Text = txtlatitude.Text;
                txtblongitude.Text = txtlongitude.Text;
            }
            else
            {
                txtBillingAddressLine1.Text = txtBillingAddressLine1.Text;
                txtBillingAddressLine2.Text = txtBillingAddressLine2.Text;
                txtBillingCity.Text = txtBillingCity.Text;
                txtBillingState.Text = txtBillingState.Text;
                hdnBillingZip.Value = hdnBillingZip.Value;
                txtBillingZip.Text = txtBillingZip.Text;
                //ddlBillingCountry.SelectedValue = dtBillingInfo.Rows[0]["CountryMasterID"].ToString();
                hdnBillingCountry.Value = hdnBillingCountry.Value;
                txtBillingCountry.Text = txtBillingCountry.Text;
                ddlBillingCurrency.SelectedValue = ddlBillingCurrency.SelectedValue;
                txtBillingCurrency.Text = txtBillingCurrency.Text;
                hdnBillingState.Value = hdnBillingState.Value;
                hdnBillingCity.Value = hdnBillingCity.Value;
                txtBillPhoneno1.Text = txtBillPhoneno1.Text;
                txtBillPhoneno2.Text = txtBillPhoneno2.Text;
                txtBillingMobile.Text = txtBillingMobile.Text;
                //txtBillingFax.Text = dtBillingInfo.Rows[0]["Fax"].ToString();
                txtBillEmail.Text = txtBillEmail.Text;
                txtblatitude.Text = txtblatitude.Text;
                txtblongitude.Text = txtblongitude.Text;
            }
        }
    }
}