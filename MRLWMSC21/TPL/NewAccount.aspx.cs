using MRLWMSC21Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Services;
using Newtonsoft.Json;
using System.Xml;
using System.Xml.Serialization;
using MRLWMSC21.ServiceReference1;


namespace MRLWMSC21.TPL
{
    public partial class NewAccount : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        public static  CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
        public static String[] UserRole;
        public static String UserRoledat;
        public static String UserRole1;
        public static int Logaccountid;
        public static int UserId;
        public static int UsertypeId;

        public static String WH;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
             cp = HttpContext.Current.User as CustomPrincipal;

    }
        protected void Page_Load(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "New Account");
            }
                if (CommonLogic.QueryString("accountid") != "")
            {
                ViewState["HeaderID"] = CommonLogic.QueryString("accountid");
                string userurl = "http://localhost/MRLWMSC21_3PL/Admin/General/UsersList.aspx?ACID="+ ViewState["HeaderID"] + "&TID="+cp.TenantID+ "&UID="+cp.UserID;
                //userFrame.Attributes.Add("src", userurl);
                //string tenanturl = "http://localhost/MRLWMSC21_3PL/Admin/General/UsersList.aspx?ACID=" + ViewState["HeaderID"] + "&TID=" + cp.TenantID + "&UID=" + cp.UserID;
                //tenantframe.Attributes.Add("src", tenanturl);

                //BuildFormData();
                //lnkUpdate.Text = "Update" + CommonLogic.btnfaUpdate;
                if (CommonLogic.QueryString("Status") == "Success")
                {
                    resetError("Successfully Updated", false);
                }
                //if (CommonLogic.QueryString("plid") != "")
                //{

                string[] hh = cp.Roles;
                int role = Convert.ToInt32(hh[0]);
                //if(role)
                lnkUpdate.Text = "Save Account" + CommonLogic.btnfaUpdate;
                //dvreport.Visible = true;

                if (!IsPostBack)
                {
                    DesignLogic.SetInnerPageSubHeading(this.Page, "New Account");
                    UserRole = cp1.Roles;
                    UserRole1 = cp.Roles[0].ToString();
                    UserId = cp1.UserID;
                    Logaccountid = cp.AccountID;
                    UsertypeId = cp1.UserTypeID;
                    string query = "Exec [dbo].[sp_GetWHS] @AccountID= " + cp1.AccountID+", @UserID="+ UserId+ ",@UserTypeID=" + UsertypeId;
                    DataSet ds = DB.GetDS(query, false);
                    WH = ds.Tables[0].Rows[0]["Warehouseids"].ToString();
                    WH = WH == "" ? "0" : WH;

                    // WH = cp1.Warehouses[0].ToString()==""?"0": cp1.Warehouses[0].ToString();

                    UserRoledat = "";
                    for (int i = 0; i < UserRole.Length; i++)
                    {
                        UserRoledat = UserRoledat + UserRole[i].ToString() + ",";
                    }
                    LoadAccountDetails();
                }

            }
            else
            {

                // txtJONumber.Text = DB.GetSqlS("EXEC [dbo].[sp_EAM_GetJORefNo]");
                lnkUpdate.Text = "Create Account" + CommonLogic.btnfaSave;
               
            }
        }
        public void LoadAccountDetails()
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            int maxid = 0;
            try
            {
                IDataReader drDetails = DB.GetRS("[dbo].[USP_GEN_GetAccountInfo] @UserID_New=" + cp1.UserID + ",@AccountID_New=" + cp1.AccountID + ",@AccountID=" + CommonLogic.QueryString("accountid"));
                // DataSet ds = DB.GetDS("[dbo].[SP_INAS_GetMSTPilotList] @PilotID=" + CommonLogic.QueryString("plid"), false);
            
                if (drDetails.Read())
                {
                    hifssoaccid.Value = drDetails.GetInt32(6).ToString();
                    txtAccountName.Text = DB.RSField(drDetails, "Account");
                    txtAccountCode.Text = DB.RSField(drDetails, "AccountCode");
                    txtCompanyLegalName.Text = DB.RSField(drDetails, "CompanyLegalName");
                    txtzohoacccode.Text = DB.RSField(drDetails, "ZohoAccountId");
                    lblMessage.Text = DB.RSField(drDetails, "LogoPath");
                    // hifssoaccid.Value = DB.RSField(drDetails, "SSOAccountId");
                    lblMessage.Visible = false;
                    string upFile = lblMessage.Text;
                    String IconPath = "TPL/AccountLogos/";
                    UpdateImage.ImageUrl = Page.ResolveUrl("~") + IconPath + upFile;
                    UpdateImage.Attributes.Add("width", "166");
                    UpdateImage.Attributes.Add("height", "50");
                }
                else
                {
                    resetError("No details available", true);
                    
                }
                drDetails.Close();
            }
            catch (Exception ex)
            {


            }


        }
        protected void resetError(string error, bool isError)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showStickyToast", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);
        }
        protected void lnkUpdate_Click(object sender, EventArgs e)
        {
            Page.Validate("save");
            cp = HttpContext.Current.User as CustomPrincipal;
            if (!Page.IsValid)
            {
                //span1.Visible = false;
                //span2.Visible = false;
                //span3.Visible = false;
                resetError("Please check all mandatory fields", true);
                return;
            }
            int accountid = 0;
            string fileName = "";
          
            if (txtAccountName!=null && txtAccountName.Text != "")
            {
                if (AccountLogo.HasFile)
                {
                    String FileExtension = Path.GetExtension(AccountLogo.FileName).ToLower();
                    if (FileExtension == ".jpg" || FileExtension == ".jpeg" || FileExtension == ".png")
                    {
                        DateTime dt = DateTime.Now;
                        fileName = DateTime.Now.ToString("ddMMyyyy_hhmmss") + "_" + AccountLogo.FileName;
                        var files = dt + "_" + AccountLogo.FileName + FileExtension;
                        AccountLogo.SaveAs(Server.MapPath("~/TPL/AccountLogos/" + fileName));
                    }
                    else
                    {
                        resetError("Only .jpg, .jpeg, .png File Formats are Allowed !", true);
                        return;
                    }

                    System.Drawing.Image img = System.Drawing.Image.FromStream(AccountLogo.PostedFile.InputStream);
                    //int height = img.Height;
                    //int width = img.PhysicalDimension.Width;
                    //if (img.PhysicalDimension.Width < 300 || img.PhysicalDimension.Height < 100)
                    //{

                    if (AccountLogo.PostedFile.ContentLength > 2000000)
                    {
                        resetError("File should not exceed 2MB", true);
                        return;

                    }
                    
                //}
                //else
                //{
                //    resetError("Image width and height should be less than or equal to 300 X 100 !", true);
                //    return;
                //}
            }
                else
                {
                    if (lblMessage.Text != "")
                    {
                        fileName = lblMessage.Text;
                    }
                    else
                    {
                        resetError("Please upload File !", true);
                        return;
                    }
                }
                int duplicate = 0;
                int duplicateacc = 0;
                if (CommonLogic.QueryString("accountid") != "")
                {
                    //  duplicate = DB.GetSqlN("select AccountID AS N from GEN_ACCount where isactive=1 and isdeleted=0 and Account ='" + txtAccountName.Text + "' and AccountID!=" + ViewState["HeaderID"] + "");
                        duplicate = DB.GetSqlN("[dbo].[USP_MST_CheckExistAccount] @AccountName = '" + txtAccountName.Text + "',@AccountID=" + ViewState["HeaderID"] + ",@flag=0");
                }
                else
                {
                   // duplicate = DB.GetSqlN("select AccountID AS N from GEN_ACCount where isactive=1 and isdeleted=0 and Account ='" + txtAccountName.Text + "'");
                    duplicate = DB.GetSqlN("[dbo].[USP_MST_CheckExistAccount] @AccountName = '" + txtAccountName.Text + "',@flag=1");

                }
                if (duplicate != 0)
                {
                    resetError("Account Name already exists", true);
                    return;
                }
                if (CommonLogic.QueryString("accountid") != "")
                {
                    //duplicateacc = DB.GetSqlN("select count(*) N from GEN_ACCount  where isactive=1 and isdeleted=0 and AccountCode='" + txtAccountCode.Text + "' and AccountID!=" + ViewState["HeaderID"] + "");
                    duplicateacc = DB.GetSqlN("[dbo].[USP_MST_CheckExistAccount] @AccountID=" + ViewState["HeaderID"] + ",@AccountCode= '" + txtAccountCode.Text + "',@flag=2");
                }
                else
                {
                    //duplicateacc = DB.GetSqlN("select count(*) N from GEN_ACCount  where isactive=1 and isdeleted=0 and AccountCode='" + txtAccountCode.Text + "'");
                    duplicateacc = DB.GetSqlN("[dbo].[USP_MST_CheckExistAccount] @AccountCode= '" + txtAccountCode.Text + "',@flag=3");
                }

                if (duplicateacc != 0)
                {
                    resetError("Account Code already exists", true);
                    return;
                }

                string data;

                if (CommonLogic.QueryString("accountid")!= "")
                {
                  
                    data = CommonLogic.QueryString("accountid");
                }
                else
                {
                    data = null;
                }
                StringBuilder sCmdPilotCr = new StringBuilder();
                sCmdPilotCr.Append("DECLARE @UpdateAccountID int EXEC ");
                sCmdPilotCr.Append("[dbo].[USP_GEN_UpsertAccount] ");
                sCmdPilotCr.Append("@AccountID='" + data + "',");
                sCmdPilotCr.Append("@Account=" + (DB.SQuote(txtAccountName.Text)) + ",");
                sCmdPilotCr.Append("@AccountCode=" + (DB.SQuote(txtAccountCode.Text)) + ",");
                sCmdPilotCr.Append("@CompanyLegalName=" + (DB.SQuote(txtCompanyLegalName.Text)) + ",");
                sCmdPilotCr.Append("@CreatedBy=" + cp.UserID.ToString() + ",");
                sCmdPilotCr.Append("@LogoPath=" + (DB.SQuote(fileName)) + ",");
                sCmdPilotCr.Append("@ZohoAccountId=" + (DB.SQuote(txtzohoacccode.Text)) + ",");
                sCmdPilotCr.Append("@SSOAccountID=" + (DB.SQuote(hifssoaccid.Value)) + ",");
                sCmdPilotCr.Append("@NewAccountID = @UpdateAccountID OUTPUT Select @UpdateAccountID as N");
                accountid = DB.GetSqlN(sCmdPilotCr.ToString());

                if (data != null)
                {
                    Response.Redirect("AccountList.aspx?statid=Updatesuccess");
                    resetError("Successfully updated", false);
                }
                else
                {
                    Response.Redirect("AccountList.aspx?statid=Createsuccess");
                    resetError("Successfully created", false);
                }
            
            }
            else
            {
                resetError("Error while updating Account", true);
                return;
            }
            if (accountid != 0)
            {
                Response.Redirect("AccountList.aspx?statid=success");
            }
            else
            {
                resetError("Error while updating Account", true);
                return;
            }
            
        }


        [WebMethod]
        public static string GetPreferences()
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            string Json = "";
            try
            {
                DataSet ds = DB.GetDS("[dbo].[GEN_MST_Get_PreferenceGroups] ", false);
                Json = JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                Json = "Failed";
            }
            return Json;
        }
        [WebMethod]
        public static string SETPreferences(int UserID, string Inxml)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            string Status = "";
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" Exec [dbo].[USP_SET_GEN_TRN_Preferences] ");
                sb.Append(" @inputDataXml='" + Inxml + "'");
                sb.Append(" ,@LoggedUserID=" + UserID);
                DB.ExecuteSQL(sb.ToString());
                Status = "success";
            }
            catch (Exception ex)
            {
                Status = "Failed";
            }
            return Status;
        }

        //===================== added by Shravya for Warehouse Deletion =====================================//
        [WebMethod]
        public static string DeleteWarehouse(int WarehouseID)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            string Result = "";
            try
            {
                StringBuilder res = new StringBuilder();
              //  int WHcount = DB.GetSqlN("select count(LocationZoneID) as N from INV_LocationZone where isactive=1 and isdeleted=0 and WarehouseID=" + WarehouseID + " and ISNULL(IsDockZone,0) <> 1");
                int WHcount = DB.GetSqlN("[dbo].[USP_MST_GetLocationORDockCount_WH] @WarehouseID =" + WarehouseID + "");

               // int WHcountdock = DB.GetSqlN("select count(DockID) as N from GEN_Dock where isactive=1 and isdeleted=0 and WarehouseID=" + WarehouseID);
                int WHcountdock = DB.GetSqlN("[dbo].[USP_MST_GetLocationORDockCount_WH] @WarehouseID =" + WarehouseID + ",@flag=1");

                if (WHcount == 0 && WHcountdock==0)
                {
                    // res.Append("Update GEN_Warehouse SET IsActive=0 , IsDeleted=1 where WarehouseID=" + WarehouseID);
                   // DB.ExecuteSQL(res.ToString());
                    string WarehouseSql = "Exec [dbo].[sp_Delete_Warehouse] @WarehouseID=" + WarehouseID + "";
                   int value= DB.GetSqlN(WarehouseSql);
                    if(value==0)
                    {
                        Result = "success";
                    }
                    else
                    {
                        Result = "-1";
                    }
                    
                }
                else
                {
                    Result = "Mapped";
                    
                }
            }
            catch(Exception ex)
            {
                Result = "Failed";
            }
            return Result;
        }

        //===================== added by durga =====================================//
        [WebMethod]
        public static List<DropDownData> GetWareHouseCode()
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            List<DropDownData> olst = new List<DropDownData>();
            //DataSet ds = DB.GetDS("select WarehouseGroupID,WarehouseGroupCode from GEN_WarehouseGroup where IsDeleted=0 and IsActive=1", false);
            DataSet ds = DB.GetDS("Exec [dbo].[sp_Android_GetWarehouses] @Flag=1", false);
            try
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    olst.Add(new DropDownData()
                    {
                        ID = Convert.ToInt32(row["WarehouseGroupID"]),
                        Name = (row["WarehouseGroupCode"]).ToString()

                    });
                }
            }
            catch (Exception e)
            {
            }
            return olst;
        }
        [WebMethod]
        public static List<DropDownData> GetWareHouseTypes()
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            List<DropDownData> olst = new List<DropDownData>();
            //DataSet ds = DB.GetDS("select WarehouseTypeID,WarehouseType from GEN_WarehouseType where IsDeleted=0 and IsActive=1", false);
            DataSet ds = DB.GetDS("Exec   [dbo].[sp_Android_GetWarehouses] @Flag=2", false);
            try
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    olst.Add(new DropDownData()
                    {
                        ID = Convert.ToInt32(row["WarehouseTypeID"]),
                        Name = (row["WarehouseType"]).ToString()

                    });
                }
            }
            catch (Exception e)
            {
            }
            return olst;
        }
         [WebMethod]
        public static List<DropDownData> GetCountryNames()
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            List<DropDownData> olst = new List<DropDownData>();
           // DataSet ds = DB.GetDS("select CountryName,CountryMasterID from GEN_CountryMaster where IsDeleted=0 and IsActive=1 ORDER BY CountryName ", false);
            DataSet ds = DB.GetDS("Exec  [dbo].[USP_LoadCountryDropDown]  ", false);           
            try
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    olst.Add(new DropDownData()
                    {
                        ID = Convert.ToInt32(row["CountryMasterID"]),
                        Name = (row["CountryName"]).ToString()

                    });
                }
            }
            catch (Exception e)
            {
            }
            return olst;
        }
        [WebMethod]
        public static List<DropDownData> GetUserTypes()
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            List<DropDownData> olst = new List<DropDownData>();
            // DataSet ds = DB.GetDS("select UserRoleTypeID, UserRoleType from GEN_UserRoleType where IsActive=1 and IsDeleted=0 and UserRoleTypeId <>1 ", false);
            DataSet ds = DB.GetDS("Exec [dbo].[USP_MST_GetUserRoleType]", false);
            try
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    olst.Add(new DropDownData()
                    {
                        ID = Convert.ToInt32(row["UserRoleTypeID"]),
                        Name = (row["UserRoleType"]).ToString()

                    });
                }
            }
            catch (Exception e)
            {
            }
            return olst;
        }
        [WebMethod]
        public static List<DropDownData> GetTenants(int accountid)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            List<DropDownData> olst = new List<DropDownData>();
            //DataSet ds = DB.GetDS("select TenantID,TenantName+' - '+TenantCode tenant from TPL_Tenant where IsDeleted=0 and IsActive=1 and AccountID="+accountid, false);
            DataSet ds = DB.GetDS(" [dbo].[USP_MST_DropTenantAccountWise] @Flag=1,@AccountID=" + accountid, false);
           
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
        public static List<DropDownData> GetAccounts(int accountid,int tenantid)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            List<DropDownData> olst = new List<DropDownData>();
            //DataSet ds = DB.GetDS("select TenantID,TenantName+' - '+TenantCode tenant from TPL_Tenant where IsDeleted=0 and IsActive=1 and AccountID="+accountid, false);
            DataSet ds = DB.GetDS("[dbo].[sp_GetUserList] @AccountID=" + accountid + ",@UserIDNew="+cp1.UserID+",@TenantId=" + tenantid, false);
            try
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    olst.Add(new DropDownData()
                    {
                        ID = Convert.ToInt32(row["UserID"]),
                        Name = (row["FullName"]).ToString()

                    });
                }
            }
            catch (Exception e)
            {
            }
            return olst;
        }
        [WebMethod]
        public static List<DropDownData> GetUsersDrop(int accountid, int tenantid)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            List<DropDownData> olst = new List<DropDownData>();
            //DataSet ds = DB.GetDS("select TenantID,TenantName+' - '+TenantCode tenant from TPL_Tenant where IsDeleted=0 and IsActive=1 and AccountID="+accountid, false);
            DataSet ds = DB.GetDS("[dbo].[sp_GetUserList] @AccountID=" + accountid + ",@UserIDNew=" + cp1.UserID + ",@TenantId=" + tenantid, false);
            try
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    olst.Add(new DropDownData()
                    {
                        ID = Convert.ToInt32(row["UserID"]),
                        Name = (row["FullName"]).ToString()

                    });
                }
            }
            catch (Exception e)
            {
            }
            return olst;
        }
        [WebMethod]
         public static List<DropDownData> GetCurrency(int CountryId)
         {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            List<DropDownData> olst = new List<DropDownData>();
          // DataSet ds = DB.GetDS("select CurrencyID,Currency+' - '+Code Currency from GEN_Currency where IsDeleted=0 and IsActive=1 and CountryID=" + CountryId, false);
            DataSet ds = DB.GetDS("Exec [dbo].[USP_MST_GetCurrencyByCountry] @CountryID=" + CountryId, false);
            
             try
             {
                 foreach (DataRow row in ds.Tables[0].Rows)
                 {
                     olst.Add(new DropDownData()
                     {
                         ID = Convert.ToInt32(row["CurrencyID"]),
                         Name = (row["Currency"]).ToString()

                     });
                 }
             }
             catch (Exception e)
             {
             }
             return olst;
         }
        [WebMethod]
        public static string GetUserList(int Accountid,int tenantid,string UserId)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            string Json = "";
            try
            {              

                DataSet ds = DB.GetDS("[dbo].[sp_GetUserList] @AccountID=" + Accountid + ",@UserIDNew=" + cp1.UserID + ", @UserID = '" + UserId+ "',@TenantId=" + tenantid, false);
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
                string query = "Exec [dbo].[sp_GetWHS] @AccountID= " + cp1.AccountID;
                DataSet ds1 = DB.GetDS(query, false);
                WH = ds1.Tables[0].Rows[0]["Warehouseids"].ToString();
                //return DataTableToJSONWithJSONNet(ds.Tables[0]);
                Json = JsonConvert.SerializeObject(ds.Tables[0]);
                return Json;

            }
            catch (Exception e)
            {
                return null;
            }
            
        }

        [WebMethod]
         public static List<DropDownData> GetRackTypes()
         {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            List<DropDownData> olst = new List<DropDownData>();
            // DataSet ds = DB.GetDS("select RackingType,RackingTypeID from GEN_RackingType where IsDeleted=0 and IsActive=1", false);
               DataSet ds = DB.GetDS("Exec [dbo].[USP_MST_GetRackDrop]", false);
      
             try
             {
                 foreach (DataRow row in ds.Tables[0].Rows)
                 {
                     olst.Add(new DropDownData()
                     {
                         ID = Convert.ToInt32(row["RackingTypeID"]),
                         Name = (row["RackingType"]).ToString()

                     });
                 }
             }
             catch (Exception e)
             {
             }
             return olst;
         }
         [WebMethod]
         public static List<DropDownData> GetInouts()
         {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            List<DropDownData> olst = new List<DropDownData>();
            // DataSet ds = DB.GetDS("select InOutID,InOutType from GEN_InOut where IsDeleted=0 and IsActive=1", false);
            DataSet ds = DB.GetDS("Exec [dbo].[USP_MST_GetInOutTypeDrop]", false);
            
             try
             {
                 foreach (DataRow row in ds.Tables[0].Rows)
                 {
                     olst.Add(new DropDownData()
                     {
                         ID = Convert.ToInt32(row["InOutID"]),
                         Name = (row["InOutType"]).ToString()

                     });
                 }
             }
             catch (Exception e)
             {
             }
             return olst;
         }
         [WebMethod]
         public static List<DropDownData> GetStates(int Countryid)
         {
             List<DropDownData> olst = new List<DropDownData>();
           //  DataSet ds = DB.GetDS("select StateMasterID,StateName+' - '+StateCode state from GEN_StateMaster where IsDeleted=0 and IsActive=1 and CountryMasterID=" + Countryid, false);
            DataSet ds = DB.GetDS(" [dbo].[USP_MST_GetStateByCountry] @Flag=1, @CountryID=" + Countryid, false);           
             try
             {
                 foreach (DataRow row in ds.Tables[0].Rows)
                 {
                     olst.Add(new DropDownData()
                     {
                         ID = Convert.ToInt32(row["StateMasterID"]),
                         Name = (row["state"]).ToString()

                     });
                 }
             }
             catch (Exception e)
             {
             }
             return olst;
         }
         [WebMethod]
         public static List<DropDownData> GetCities(int stateid)
         {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            List<DropDownData> olst = new List<DropDownData>();
           // DataSet ds = DB.GetDS("select CityMasterID,CityName+' - '+CityCode city from GEN_CityMaster where IsDeleted=0 and IsActive=1 and StateMasterID="+stateid, false);
            DataSet ds = DB.GetDS("[dbo].[USP_MST_GetCityDrop] @StateID=" + stateid +", @Flag=1", false);
            try
             {
                 foreach (DataRow row in ds.Tables[0].Rows)
                 {
                     olst.Add(new DropDownData()
                     {
                         ID = Convert.ToInt32(row["CityMasterID"]),
                         Name = (row["city"]).ToString()

                     });
                 }
             }
             catch (Exception e)
             {
             }
             return olst;
         }
         [WebMethod]
         public static List<DropDownData> GetTimePreferences()
         {
             List<DropDownData> olst = new List<DropDownData>();
          //   DataSet ds = DB.GetDS("select GEN_MST_PreferenceOption_ID,dbo.UDF_ParseAndReturnLocaleString(OptionLabel,'en') preference from GEN_MST_PreferenceOptions where GEN_MST_Preference_ID=11 and IsDeleted=0 and IsActive=1" , false);
            DataSet ds = DB.GetDS("[dbo].[USP_MST_GetPreferenceOptionDrop]", false);
                        
            try
             {
                 foreach (DataRow row in ds.Tables[0].Rows)
                 {
                     olst.Add(new DropDownData()
                     {
                         ID = Convert.ToInt32(row["GEN_MST_PreferenceOption_ID"]),
                         Name = (row["preference"]).ToString()

                     });
                 }
             }
             catch (Exception e)
             {
             }
             return olst;
         }
         [WebMethod]
         public static List<DropDownData> GetZipcode(int cityid)
         {
             List<DropDownData> olst = new List<DropDownData>();
            // DataSet ds = DB.GetDS("select ZipCodeID,ZipCode from GEN_ZipCode where IsDeleted=0 and IsActive=1 and CityMasterID=" + cityid, false);
             DataSet ds = DB.GetDS("[dbo].[USP_MST_GetZipCodes] @CityID =" + cityid, false);
            try
             {
                 foreach (DataRow row in ds.Tables[0].Rows)
                 {
                     olst.Add(new DropDownData()
                     {
                         ID = Convert.ToInt32(row["ZipCodeID"]),
                         Name = (row["ZipCode"]).ToString()

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
            cp1 = HttpContext.Current.User as CustomPrincipal;
            List<DropDownData> olst = new List<DropDownData>();
           // DataSet ds = DB.GetDS("select UserRoleID,UserRole from GEN_UserRole where IsDeleted=0 and IsActive=1 and (UserRoleTypeID="+ usertypeid + " OR UserRoleTypeID IS NULL)", false);
            DataSet ds = DB.GetDS("[dbo].[USP_MST_GetUserRoleByType] @UserTypeID = " + usertypeid, false);
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
        public static List<DropDownData> GetPrinterData()
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            List<DropDownData> olst = new List<DropDownData>();
            //DataSet ds = DB.GetDS("select ClientResourceID,ClientResourceName from GEN_ClientResource where IsDeleted=0 and IsActive=1", false);
            DataSet ds = DB.GetDS("Exec [dbo].[USP_MST_GetClientResourceDrop]", false);
            try
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    olst.Add(new DropDownData()
                    {
                        ID = Convert.ToInt32(row["ClientResourceID"]),
                        Name = (row["ClientResourceName"]).ToString()

                    });
                }
            }
            catch (Exception e)
            {
            }
            return olst;
        }
        [WebMethod]
        public static List<DropDownData> GetWareHouseData(int Accountid)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            List<DropDownData> olst = new List<DropDownData>();
            DataSet ds = DB.GetDS("select WarehouseID,WHName+' - '+WHCode warehouse from GEN_Warehouse where IsDeleted=0 and IsActive=1 and AccountID="+ Accountid, false);
          //  DataSet ds = DB.GetDS("[dbo].[USP_MST_DropWH]  @Flag=1,@UserID="+cp1.UserID+",@AccountID=" + Accountid, false);
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
         public static List<DropDownData> GetAccount(int Accountid)
         {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            if (Accountid == 0)
            {
                Accountid = cp1.AccountID;
            }
            List<DropDownData> olst = new List<DropDownData>();
           //  DataSet ds = DB.GetDS("select top 10 AccountID,Account from GEN_Account where 0=" + Accountid + " or AccountID=" + Accountid, false);
               DataSet ds = DB.GetDS("Exec [dbo].[USP_MST_GetAccountDrop] @LogAccountID = " + Accountid , false);
            try
             {
                 foreach (DataRow row in ds.Tables[0].Rows)
                 {
                     olst.Add(new DropDownData()
                     {
                         ID = Convert.ToInt32(row["AccountID"]),
                         Name = (row["Account"]).ToString()

                     });
                 }
             }
             catch (Exception e)
             {
             }
             return olst;
         }
         [WebMethod]
         public static int UpsertNewWareHouse(WareHouse obj,int accountid)
         {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            try
             {
                 return DB.GetSqlN("exec [dbo].[UpsertNewWareHouse] @Warehouseid="+obj.WareHouseID+",@WHName=" + DB.SQuote(obj.WHName) + ",@WHCode=" + DB.SQuote(obj.WHCode) + ",@WHGroupcode=" + obj.WHGroupcode +
                ",@Location=" + DB.SQuote(obj.Location) + ",@RackingRType=" + obj.RackingRType + ",@WHtype=" + obj.WHtype + ",@WHAddress=" + DB.SQuote(obj.WHAddress) + ",@FloorSpace=" + DB.SQuote(obj.FloorSpace) +
                ",@Measurements=" + DB.SQuote(obj.Measurements) + ",@PIN=" + obj.ZipCodeId + ",@GeoLocation=" + DB.SQuote(obj.GeoLocation) + ",@CountryId=" + obj.Country + ",@CurrencyId=" + obj.Currency +
                ",@InoutId=" + obj.Inout + ",@pName=" + DB.SQuote(obj.pName) + ",@Pmobile=" + DB.SQuote(obj.Pmobile) + ",@pEmail=" + DB.SQuote(obj.pEmail) +
                ",@PAddress=" + DB.SQuote(obj.PAddress) + ",@sname=" + DB.SQuote(obj.sname) + ",@SMobile=" + DB.SQuote(obj.SMobile) + ",@SEmail=" + DB.SQuote(obj.SEmail) +
                ",@SAddress=" + DB.SQuote(obj.SAddress) + ",@UserID=" + cp1.UserID + ",@AccountId=" + accountid+",@length="+obj.Length+",@Width="+obj.Width+
                ",@height=" + obj.Height + ",@Latitude=" + DB.SQuote(obj.Latitude) + ",@Langitude=" + DB.SQuote(obj.Langitude) + ",@stateid=" + obj.StateId + ",@cityid=" + obj.CityId + ",@TimeZoneId=" + obj.Time);
             }

             catch (Exception e)
             {
                 return -1;
             }
           
         }
         [WebMethod]
         public static string GetWareHouseList(int AccountId)
         {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            if (AccountId == 0)
            {
                AccountId = cp1.AccountID;
            }
            DataSet ds = DB.GetDS("exec [SP_GetWareHouseList] @UserID="+cp1.UserID+",@Accountid=" + AccountId, false);
         
             return DataTableToJSONWithJSONNet(ds.Tables[0]);
         }
        [WebMethod]
        public static string GetsubscriptionList(int AccountId)
        {
            //NewAccount obj = new NewAccount();
            cp1 = HttpContext.Current.User as CustomPrincipal;
            //obj.getsubslist();
            if (AccountId == 0)
            {
                AccountId = cp1.AccountID;
            }
            DataSet ds = DB.GetDS("exec [SP_GetSubscriptionData] @AccountId=" + AccountId, false);

            return DataTableToJSONWithJSONNet(ds.Tables[0]);

            //ServiceReference1.SingleSignOnDBSinkClient objServiceClient = new SingleSignOnDBSinkClient();
            //string cal = objServiceClient.GetSubscriptionByAccount(Convert.ToString( AccountId));
            //return cal;
        }

        public void getsubslist()
        {
            ServiceReference1.SingleSignOnDBSinkClient objServiceClient = new SingleSignOnDBSinkClient();

           
        }
        public static string DataTableToJSONWithJSONNet(DataTable table)
         {


             string JSONString = string.Empty;
             JSONString = JsonConvert.SerializeObject(table);
             return JSONString;
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

        [WebMethod]
        public static int UpsertUserData(User obj)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            int isactive = obj.Isactive == true ? 1 : 0;
            string encriptpwd = CommonLogic.EncryptString(obj.Password.Trim());//DB.SQuote(Encrypt.EncryptData(CommonLogic.Application("EncryptKey"), obj.Password.Trim()));

            ServiceReference1.SingleSignOnDBSinkClient ssvalidator = new ServiceReference1.SingleSignOnDBSinkClient();
            //ssvalidator.InsertUserInvWMS()
            string ssouserid = "0";
            if (obj.UserID == 0)
            {
                Generic_Class.Class.User objUser = new Generic_Class.Class.User();
                objUser.Apps_MST_Account_ID = obj.SSOAccountID;
                objUser.TM_MST_Tenant_ID = obj.TenantID;
                objUser.FirstName = obj.FirstName;
                objUser.LastName = obj.LastNane;
                objUser.MiddleName = obj.MiddleName;
                objUser.CompanyName = obj.CompanyName;
                objUser.eMailID = obj.Email;
                objUser.EmpCode = obj.EMPCode;
                objUser.Password = obj.Password;
                objUser.Mobile = obj.Mobile;
                objUser.CreatedBy = cp1.UserID;
                objUser.CreatedOn = DateTime.Now;
                objUser.UpdatedBy = cp1.UserID;
                objUser.UpdatedOn = DateTime.Now;
                objUser.IsActive = obj.Isactive;
                //objUser.Phone = Request.Form["phone"];
                //objUser.ZOHO_Customer_ID = objResponseContact.subscription.customer.customer_id.ToString();
                string Serializedxml = string.Empty;
                Serializedxml = XmlSerialization(objUser);
                string UserResult = ""; //ssvalidator.AddUser3PL(Serializedxml.ToString(), objUser.Apps_MST_User_ID.ToString(), encriptpwd);
               // string UserResult = ssvalidator.InsertUserInvSSO(Serializedxml.ToString(), objUser.Apps_MST_User_ID.ToString());
                ssouserid = UserResult;
            }
            else
            {
                Generic_Class.Class.User objUser = new Generic_Class.Class.User();
                //objUser.Apps_MST_User_ID = obj.UserID;
                objUser.Apps_MST_Account_ID = obj.SSOAccountID;
                objUser.TM_MST_Tenant_ID = obj.TenantID;
                objUser.FirstName = obj.FirstName;
                objUser.LastName = obj.LastNane;
                objUser.MiddleName = obj.MiddleName;
                objUser.CompanyName = obj.CompanyName;
                objUser.eMailID = obj.Email;
                objUser.EmpCode = obj.EMPCode;
                objUser.Password = obj.Password;
                objUser.Mobile = obj.Mobile;
                objUser.CreatedBy = cp1.UserID;
                objUser.CreatedOn = DateTime.Now;
                objUser.UpdatedBy = cp1.UserID;
                objUser.UpdatedOn = DateTime.Now;
                objUser.IsActive = obj.Isactive;
                //objUser.Phone = Request.Form["phone"];
                //objUser.ZOHO_Customer_ID = objResponseContact.subscription.customer.customer_id.ToString();
                string Serializedxml = string.Empty;
                Serializedxml = XmlSerialization(objUser);
                string UserResult = "";// ssvalidator.AddUser3PL(Serializedxml.ToString(), obj.SSOUserID.ToString() ,encriptpwd);
                //string UserResult = ssvalidator.InsertUserInvSSO(Serializedxml.ToString(), objUser.Apps_MST_User_ID.ToString());
                ssouserid =  UserResult;
            }

            if (ssouserid == "")
            {
                ssouserid = "50";
            }
            else
            {
                ssouserid = ssouserid;
            }


            // ssvalidator.InsertUserInvSSO(DB.SQuote(obj.FirstName));
            string Qurey = "DECLARE @UpdateUserID int; Exec [dbo].[sp_GEN_UpsertUsers] @UserID=" + obj.UserID + ",@TenantID=" + obj.TenantID + ",@FirstName=" + DB.SQuote(obj.FirstName) + ",@LastName=" + DB.SQuote(obj.LastNane) + ",@MiddleName=" + DB.SQuote(obj.MiddleName) + ",@Email=" + DB.SQuote(obj.Email) +
                ",@AlternateEmail1=" + DB.SQuote(obj.AltEmail1) + ",@AlternateEmail2=" + DB.SQuote(obj.AltEmail2) + ",@Password=" + DB.SQuote(obj.Password) + ",@UserRoleIDs=" + DB.SQuote(obj.Roles) + ",@WarehouseIDs=" + DB.SQuote(obj.warehouses) + ",@IsActive=" + isactive +
                ",@EnPassword=" + DB.SQuote(encriptpwd) + ",@Mobile='" + obj.Mobile + "',@CreatedBy=" + cp1.UserID + ",@EmployeeCode=" + DB.SQuote(obj.EMPCode) + ",@AccountID=" + obj.AccountID + ",@UserTypeID=" + obj.UserTypeID + ",@SSOUserID='" + ssouserid + "',@NewUserID = @UpdateUserID OUTPUT Select @UpdateUserID as N";
            int userid= DB.GetSqlN(Qurey);

            //  DB.get
            //  string[] val;

            //cp1.Warehouses = '4136';
            return userid;

        }
        public static string XmlSerialization(object ObjToSerialize)
        {
            string xml;

            var xmlnamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            var settings = new XmlWriterSettings();

            using (StringWriter sw = new StringWriter())
            {
                XmlSerializer ser = new XmlSerializer(ObjToSerialize.GetType());
                ser.Serialize(sw, ObjToSerialize, xmlnamespaces);
                xml = sw.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", " ");
            }

            return xml;
        }
        public class DropDownData
        {
            public int ID { get; set; }
            public String Name { get; set; }

        }
        public class WareHouse
        {
            public int WareHouseID { get; set; }
            public String WHName { get; set; }
            public String WHCode { get; set; }
            public int WHGroupcode { get; set; }
            public String Location { get; set; }
            public int RackingRType { get; set; }
            public int WHtype { get; set; }
            public String WHarea { get; set; }
            public String WHAddress { get; set; }
            public String FloorSpace { get; set; }
            public String Measurements { get; set; }
            public String PIN { get; set; }
            public String GeoLocation { get; set; }
            public int Country { get; set; }
            public int Currency { get; set; }
            public int Inout { get; set; }
            public String pName { get; set; }
            public String Pmobile { get; set; }
            public String pEmail { get; set; }
            public String PAddress { get; set; }
            public String sname { get; set; }
            public String SMobile { get; set; }
            public String SEmail { get; set; }
            public String SAddress { get; set; }

            public int StateId { get; set; }
            public int CityId { get; set; }
            public int ZipCodeId { get; set; }
            public String Latitude { get; set; }
            public String Langitude { get; set; }
            public int Length { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public int Time { get; set; }
            public int AccountId { get; set; }

          

        }
        public class User
        {
            public int UserTypeID { get; set; }
            public int AccountID { get; set; }
            public int TenantID { get; set; }
            public String FirstName { get; set; }
            public String LastNane { get; set; }
            public String  MiddleName { get; set; }
            public String EMPCode { get; set; }
            public int Gender { get; set; }
            public String Email { get; set; }
            public String AltEmail1 { get; set; }
            public String AltEmail2 { get; set; }
            public String Password { get; set; }
            public String Mobile { get; set; }
            public int UserID { get; set; }
            public bool Isactive { get; set; }
            public string Roles { get; set; }
            public string warehouses { get; set; }
            public string PrinterId { get; set; }
            //public string UserType { get; set; }
            //public string AccountName { get; set; }
            //public string tenantName { get; set; }
            //public string FullName { get; set; }
            public List<string> RolesList { get; set; }
            public List<string> WHLISt { get; set; }
            //public string rolesinformation { get; set; }
            //public string warehouseinformation { get; set; }
            public int SSOAccountID { get; set; }

            public string CompanyName { get; set; }

            public int SSOUserID { get; set; }



        }
    }
}