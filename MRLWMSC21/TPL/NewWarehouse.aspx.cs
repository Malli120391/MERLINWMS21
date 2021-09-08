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
using MRLWMSC21.mOutbound;
using iTextSharp.text.pdf;
using System.Text.RegularExpressions;

namespace MRLWMSC21.TPL
{
    public partial class NewWarehouse : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        public static CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;

        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void resetError(string error, bool isError)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showStickyToast", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);
        }

        //===================== added by durga =====================================//
        [WebMethod]
        public static List<DropDownData> GetWareHouseCode()
        {
            List<DropDownData> olst = new List<DropDownData>();
           // DataSet ds = DB.GetDS("select WarehouseGroupID,WarehouseGroupCode from GEN_WarehouseGroup where IsDeleted=0 and IsActive=1", false);
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
        public static string getBarCode(string Docid)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            DataSet _dsResult = DB.GetDS("SELECT DOC.DockName AS DocName, LOC.DisplayLocationCode AS LocationCode FROM INV_Location  LOC JOIN GEN_DOCk DOC ON LOC.DockID = DOC.DockID WHERE LOC.DockID = '" + Docid + "' AND DisplayLocationCode<>''", false);
            Barcode _barCode = new Barcode();
            if (_dsResult.Tables.Count > 0)
            {
                DataRow _drRow = _dsResult.Tables[0].Rows[0];
                _barCode.DockName = _drRow["DocName"].ToString();
                _barCode.LocationCode = _drRow["DocName"].ToString();
            }

            UnloadPDFGenerator barcode = new UnloadPDFGenerator(_barCode);

            String filePath = barcode.GeneratePDF(UnloadTemplate.BAR_CODE);

            return filePath;
        }

        [WebMethod]
        public static List<DropDownData> GetWareHouseTypes()
        {
            List<DropDownData> olst = new List<DropDownData>();
           // DataSet ds = DB.GetDS("select WarehouseTypeID,WarehouseType from GEN_WarehouseType where IsDeleted=0 and IsActive=1", false);
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
            List<DropDownData> olst = new List<DropDownData>();
            //  DataSet ds = DB.GetDS("select CountryName,CountryMasterID from GEN_CountryMaster where IsDeleted=0 and IsActive=1 ORDER BY CountryName ", false);
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
            List<DropDownData> olst = new List<DropDownData>();
            // DataSet ds = DB.GetDS("select UserTypeID,UserType from GEN_UserType where IsActive=1 and IsDeleted=0 and UserTypeID!=1", false);
            DataSet ds = DB.GetDS("Exec [dbo].[USP_MST_GetUserRoleByType] @UserTypeID=0,@flag=1",false);
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
        public static List<DropDownData> GetTenants(int accountid)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            List<DropDownData> olst = new List<DropDownData>();
           // DataSet ds = DB.GetDS("select TenantID,TenantName+' - '+TenantCode tenant from TPL_Tenant where IsDeleted=0 and IsActive=1 and AccountID=" + accountid, false);
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
        public static List<DropDownData> GetCurrency(int CountryId)
        {
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
        public static string GetUserList(int Accountid, int tenantid)
        {

            cp1 = HttpContext.Current.User as CustomPrincipal;
            try
            {


                DataSet ds = DB.GetDS("[dbo].[sp_GetUserList] @AccountID=" + Accountid + ",@TenantId=" + tenantid, false);
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
        public static List<DropDownData> GetRackTypes()
        {
            List<DropDownData> olst = new List<DropDownData>();
          //  DataSet ds = DB.GetDS("select RackingType,RackingTypeID from GEN_RackingType where IsDeleted=0 and IsActive=1", false);
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
            //DataSet ds = DB.GetDS("select StateMasterID,StateName+' - '+StateCode state from GEN_StateMaster where IsDeleted=0 and IsActive=1 and CountryMasterID=" + Countryid, false);
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
            List<DropDownData> olst = new List<DropDownData>();
           // DataSet ds = DB.GetDS("select CityMasterID,CityName+' - '+CityCode city from GEN_CityMaster where IsDeleted=0 and IsActive=1 and StateMasterID=" + stateid, false);
            DataSet ds = DB.GetDS("[dbo].[USP_MST_GetCityDrop] @StateID=" + stateid + ", @Flag=1", false);
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
            //DataSet ds = DB.GetDS("select GEN_MST_PreferenceOption_ID,dbo.UDF_ParseAndReturnLocaleString(OptionLabel,'en') preference from GEN_MST_PreferenceOptions where GEN_MST_Preference_ID=11 and IsDeleted=0 and IsActive=1", false);
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
            //DataSet ds = DB.GetDS("select ZipCodeID,ZipCode from GEN_ZipCode where IsDeleted=0 and IsActive=1 and CityMasterID=" + cityid, false);
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
            List<DropDownData> olst = new List<DropDownData>();
           // DataSet ds = DB.GetDS("select UserRoleID,UserRole from GEN_UserRole where IsDeleted=0 and IsActive=1 and (0=" + usertypeid + " or UserRoleTypeID=" + usertypeid + " OR ISNULL(UserRoleTypeID, 0) = 0)", false);
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
            List<DropDownData> olst = new List<DropDownData>();
           // DataSet ds = DB.GetDS("select ClientResourceID,ClientResourceName from GEN_ClientResource where IsDeleted=0 and IsActive=1", false);
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
            List<DropDownData> olst = new List<DropDownData>();
          //  DataSet ds = DB.GetDS("select WarehouseID,WHName+' - '+WHCode warehouse from GEN_Warehouse where IsDeleted=0 and IsActive=1 and AccountID=" + Accountid, false);
            DataSet ds = DB.GetDS("[dbo].[USP_MST_DropWH]  @Flag=1,@AccountID=" + Accountid, false);
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
            DataSet ds = DB.GetDS("Exec [dbo].[USP_MST_GetAccountDrop] @LogAccountID=" + Accountid, false);

            
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
        public static int UpsertNewWareHouse(WareHouse obj, int accountid)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;

            try
            {
                if (obj.Isactive == 1)
                {
                    obj.IsDeleted = 0;
                }

                return DB.GetSqlN("exec [dbo].[UpsertNewWareHouse] @Warehouseid=" + obj.WareHouseID + ",@WHName=" + DB.SQuote(obj.WHName) + ",@WHCode=" + DB.SQuote(obj.WHCode) + ",@WHGroupcode=" + obj.WHGroupcode +
               ",@Location=" + DB.SQuote(obj.Location) + ",@RackingRType=" + obj.RackingRType + ",@WHtype=" + obj.WHtype + ",@WHAddress=" + DB.SQuote(obj.WHAddress) + ",@FloorSpace=" + DB.SQuote(obj.FloorSpace) +
               ",@Measurements=" + DB.SQuote(obj.Measurements) + ",@PIN=" + obj.ZipCodeId + ",@GeoLocation=" + DB.SQuote(obj.GeoLocation) + ",@CountryId=" + obj.Country + ",@CurrencyId=" + obj.Currency +
               ",@InoutId=" + obj.Inout + ",@pName=" + DB.SQuote(obj.pName) + ",@Pmobile=" + DB.SQuote(obj.Pmobile) + ",@pEmail=" + DB.SQuote(obj.pEmail) +
               ",@PAddress=" + DB.SQuote(obj.PAddress) + ",@sname=" + DB.SQuote(obj.sname) + ",@SMobile=" + DB.SQuote(obj.SMobile) + ",@SEmail=" + DB.SQuote(obj.SEmail) +
               ",@SAddress=" + DB.SQuote(obj.SAddress) + ",@UserID=" + cp1.UserID + ",@AccountId=" + accountid + ",@length=" + obj.Length + ",@Width=" + obj.Width +
               ",@height=" + obj.Height + ",@Latitude=" + DB.SQuote(obj.Latitude) + ",@Langitude=" + DB.SQuote(obj.Langitude) + ",@stateid=" + obj.StateId + ",@cityid=" + obj.CityId + ",@IsActive="+obj.Isactive+ ", @IsDeleted="+obj.IsDeleted+",@TimeZoneId=" + obj.Time);
            }

            catch (Exception e)
            {
                return -1;
            }

        }


        [WebMethod]
        public static int DeleteZone(int ZoneId,int WHID)
        {
            // string Result = "";
            cp1 = HttpContext.Current.User as CustomPrincipal;
            try
            {
                return DB.GetSqlN("exec [dbo].[DeleteZone] @Warehouseid=" + WHID + ",@ZoneId=" + ZoneId);
            }
                //StringBuilder res = new StringBuilder();
                //int WHcount = DB.GetSqlN("select count(LocationZoneID) as N from INV_LocationZone where isactive=1 and isdeleted=0 and WarehouseID=" + WarehouseID);
                //int WHcountdock = DB.GetSqlN("select count(DockID) as N from GEN_Dock where isactive=1 and isdeleted=0 and WarehouseID=" + WarehouseID);
                //if (WHcount == 0 && WHcountdock == 0)
                //{
                //    res.Append("Update GEN_Warehouse SET IsActive=0 , IsDeleted=1 where WarehouseID=" + WarehouseID);
                //    DB.ExecuteSQL(res.ToString());
                //    Result = "success";
                //}
                //else
                //{
                //    Result = "Mapped";

                //}
         
            catch (Exception e)
            {
                return 0;
            }
            //return Result;
        }

        [WebMethod]
        public static int EditZone(int ZoneId)
        {
            // string Result = "";
            try
            {
                //string query = "select count(*) as N  from INV_Location where ZoneId=" + ZoneId;
                string query = "Exec [dbo].[USP_LoadLocations] @ZoneId=" + ZoneId;
                return DB.GetSqlN(query);
            }

            catch (Exception e)
            {
                return 0;
            }
            //return Result;
        }


        [WebMethod]
        public static string GetWareHouseList(int AccountId)
        {
            if (AccountId == 0)
            {
                AccountId = cp1.AccountID;
            }
            DataSet ds = DB.GetDS("exec [SP_GetWareHouseList] @Accountid=" + AccountId, false);

            return DataTableToJSONWithJSONNet(ds.Tables[0]);
        }

        [WebMethod]
        public static string GetsubscriptionList(int AccountId)
        {
            //NewAccount obj = new NewAccount();

            //obj.getsubslist();
            cp1 = HttpContext.Current.User as CustomPrincipal;
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

        [WebMethod]
        public static List<DropDownData> GetWarehouse(int accountid)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            List<DropDownData> wlist = new List<DropDownData>();
            DataSet ds = DB.GetDS("SELECT DISTINCT WarehouseID, WHCode + ' - ' + WHName as WarehouseLoc FROM GEN_Warehouse WHERE IsActive = 1 AND IsDeleted = 0 AND AccountID =" + accountid, false);
            try
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    wlist.Add(new DropDownData()
                    {
                        ID = Convert.ToInt32(row["WarehouseID"]),
                        Name = (row["WarehouseLoc"]).ToString()
                    });
                }
            }
            catch (Exception e)
            {

            }
            return wlist;
        }

        [WebMethod]
        public static List<DropDownData> GetDocType()
        {
            List<DropDownData> dlist = new List<DropDownData>();
            //DataSet ds = DB.GetDS("SELECT DISTINCT DockTypeID, DockType FROM GEN_DockType WHERE IsActive = 1 AND IsDeleted = 0", false);
            DataSet ds = DB.GetDS("Exec [dbo].[USP_LoadDockType]", false);            
            try
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    dlist.Add(new DropDownData()
                    {
                        ID = Convert.ToInt32(row["DockTypeID"]),
                        Name = (row["DockType"]).ToString()
                    });
                }
            }
            catch (Exception e)
            {

            }
            return dlist;
        }

        [WebMethod]
        public static string UpsertNewDock(int WarehouseID, int DockID, string DockNum, string DockName, int DocType)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            //bool status = ValidatePassword(DockNum);
            bool status = isAlphaNumeric(DockNum);
            if (status == false)
            {
             
                return "-1";
            }

            DB.ExecuteSQL("EXEC [USP_UpsertGEN_Dock] @DockNo=" + DB.SQuote(DockNum) + ",@DockName=" + DB.SQuote(DockName) + ",@DockTypeID= " + DocType + ",@WarehouseID=" + WarehouseID + ",@DockID=" + DockID);
            return "";
        }

        public static bool ValidatePassword(string password)
        {
             string patternPassword = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$";
            //string patternPassword = "^[a-zA-Z0-9]*$";
            if (!string.IsNullOrEmpty(password))
            {
                if (!Regex.IsMatch(password, patternPassword))
                {
                    return false;
                }
            }
            return true;
        }


        public static bool isAlphaNumeric(string N)
        {
            bool YesNumeric = false;
            bool YesAlpha = false;
            bool BothStatus = false;


            for (int i = 0; i < N.Length; i++)
            {
                if (char.IsLetter(N[i]))
                    YesAlpha = true;

                if (char.IsNumber(N[i]))
                    YesNumeric = true;
            }

            if (YesAlpha == true && YesNumeric == true)
            {
                BothStatus = true;
            }
            else
            {
                BothStatus = false;
            }
            return BothStatus;
        }

        [WebMethod]
        public static List<DockList> GetDocList(string AccountId , int WarehouseID)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            List<DockList> bdlst = new List<DockList>();

            //<!--------------Converting into Procedure---------------->
          //  string Data = "SELECT DOC.WarehouseID, DOC.DockNo, DOC.DockID, DOC.DockName, DOC.DockTypeID, DOCT.DockType, WH.WHCode + '-' + WH.WHName as WareHouseName FROM GEN_Account ACC JOIN GEN_Warehouse WH ON WH.AccountID = ACC.AccountID JOIN GEN_Dock DOC ON DOC.WarehouseID = WH.WarehouseID JOIN GEN_DockType DOCT ON DOCT.DockTypeID = DOC.DockTypeID WHERE ACC.AccountID =" + AccountId + "AND WH.WarehouseID = " + WarehouseID;

            string Data= "[dbo].[USP_GetDocListByWH] @AccountID="+ AccountId + ",@WarehouseID="+ WarehouseID + "";
            DataSet bds = DB.GetDS(Data, false);

            if (bds.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in bds.Tables[0].Rows)
                {
                    bdlst.Add(new DockList
                    {
                        WarehouseID = Convert.ToInt32(row["WarehouseID"]),
                        WarehouseName = row["WareHouseName"].ToString(),
                        DockNumber = row["DockNo"].ToString(),
                        DockName = row["DockName"].ToString(),
                        DockTypeID = Convert.ToInt32(row["DockTypeID"]),
                        DockType = row["DockType"].ToString(),
                        DockID = Convert.ToInt32(row["DockID"])
                    });

                }
            }
            return bdlst;
        }

        [WebMethod]
        public static string UpsertNewZone(int WarehouseID, int ZoneID, string ZoneCode, string ZoneDesc)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            DB.ExecuteSQL("EXEC [USP_UpsertGEN_Zone] @ZoneCode=" + DB.SQuote(ZoneCode) + ",@ZoneDesc=" + DB.SQuote(ZoneDesc) + ",@WarehouseID=" + WarehouseID + ",@ZoneID=" + ZoneID);
            return "";
        }

        [WebMethod]
        public static List<ZoneList> GetZoneList(string AccountId, int WarehouseID)
        {
            List<ZoneList> bdlst = new List<ZoneList>();
            string Data = "SELECT WH.WHCode + '-' + WH.WHName as WareHouseName, WH.WarehouseID, LZ.LocationZoneID, LZ.LocationZoneCode, LZ.[Description] as ZoneDesc,LZ.IsDockZone,LZ.IsActive FROM INV_LocationZone LZ JOIN GEN_Warehouse WH ON WH.WarehouseID = LZ.WarehouseID WHERE  LZ.IsDeleted=0 AND WH.AccountID =" + AccountId + "AND WH.WarehouseID =" + WarehouseID;
            DataSet bds = DB.GetDS(Data, false);

            if (bds.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in bds.Tables[0].Rows)
                {
                    bdlst.Add(new ZoneList
                    {
                        WarehouseID = Convert.ToInt32(row["WarehouseID"]),
                        WarehouseName = row["WareHouseName"].ToString(),
                        ZoneCode = row["LocationZoneCode"].ToString(),
                        ZoneDesc = row["ZoneDesc"].ToString(),
                        ZoneID = Convert.ToInt32(row["LocationZoneID"]),
                        IsDockZone = row["IsDockZone"].ToString(),
                        IsActive= Convert.ToInt32(row["IsActive"])
                    });

                }
            }
            return bdlst;
        }

        public static string DataTableToJSONWithJSONNet(DataTable table)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }

        public class DockList
        {
            public int WarehouseID { get; set; }
            public String WarehouseName { get; set; }
            public String DockNumber { get; set; }
            public String DockName { get; set; }
            public int DockTypeID { get; set; }
            public String DockType { get; set; }
            public int DockID { get; set; }
        }

        public class ZoneList
        {
            public int WarehouseID { get; set; }
            public String WarehouseName { get; set; }
            public String ZoneCode { get; set; }
            public String ZoneDesc { get; set; }
            public int ZoneID { get; set; }
            public string IsDockZone { get; set; }
            public int IsActive { get; set; }

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
            public int Isactive { get; set; }
            
            public int IsDeleted { get; set; }

        }
        public class User
        {
            public int UserTypeID { get; set; }
            public int AccountID { get; set; }
            public int TenantID { get; set; }
            public String FirstName { get; set; }
            public String LastNane { get; set; }
            public String MiddleName { get; set; }
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



        }
        // for BarCode
        public class Barcode
        {
            public string DockName { get; set; }
            public string LocationCode { get; set; }
        }
    }
}