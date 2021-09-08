using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21Common;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;
using System.Web.Services;
using System.Web.Script.Services;
using System.Globalization;
using Newtonsoft.Json;

namespace MRLWMSC21.mMaterialManagement
{
    public partial class ItemMasterRequest : System.Web.UI.Page
    {
        public static CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
        public  CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        public String mid = CommonLogic.QueryString("mid");
        string TenantRootDir = "";
        string MMTPath = "";
        string PartNum = "";
        //String _MCode = "";
        //string TenantIDLS = "";
        //int TenantIDL = 0;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            cp1 = HttpContext.Current.User as CustomPrincipal;

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Item Master Request");
                if ((CommonLogic.QueryString("mid") != "") || (CommonLogic.QueryString("mid") == "null"))
                {
                    //DataSet mt = DB.GetDS("select MCode, TenantID from MMT_MaterialMaster where MaterialMasterID=" + mid, false);
                    //_MCode = mt.Tables[0].Rows[0]["MCode"].ToString() ;
                    //TenantIDLS = mt.Tables[0].Rows[0]["TenantID"].ToString();
                    //TenantIDL = Convert.ToInt32(TenantIDLS);
                    middailogbox();
                   LoadMMItemData();
                }
            }
            
        }

        [WebMethod]
        public static string GetItemMasterLoad(int TenantID, int AccountID)
        {
            string Json = "";
            string IMLList;
            try
            {
                IMLList = "Exec USP_MMT_MasterDataList @TenantID=" + TenantID + ",@AccountID=" + AccountID;
                DataSet ds = DB.GetDS(IMLList, false);
                Json = JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                Json = "Failed";
            }
            return Json;

        }
       
        [WebMethod]
        public static string GetItemMasterDetails(int MMID)
        {
            string Json = "";
            string IMDList;
            try
            {
                IMDList = "Exec USP_MMT_GETMaterailParametersInfo @MaterialMasterID=" + MMID + ", @AccountID_New=" + cp1.AccountID;
                DataSet ds = DB.GetDS(IMDList, false);
                Json = JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                Json = "Failed";
            }
            return Json;

        }

        [WebMethod]
        public static string UpsertBasicDetails(string BasicData,int AccountID, int TenantID, int LoggedInUserID, int MaterialMasterID , string Mcode)
        {
            //if(MaterialMasterID != 0)
            //{
            //    string POCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckPO] @MaterialMasterID=" + MaterialMasterID + ",@TenantID=" + TenantID);
            //    if(POCheck != "")
            //    {
            //        string POMap = "mappedpo";
            //        return POMap;
            //    }
            //}

            if (MaterialMasterID == 0)
            {
                int itemcount = DB.GetSqlN("select COUNT(*) AS N from  MMT_MaterialMaster MM INNER JOIN TPL_Tenant TNT ON TNT.TenantID = " + TenantID + " AND TNT.IsActive = 1 AND TNT.IsDeleted = 0 AND TNT.AccountID = " + AccountID + " INNER JOIN TPL_Tenant_MaterialMaster TMM ON TMM.MaterialMasterID = MM.MaterialMasterID AND MM.IsActive = 1 AND MM.IsDeleted = 0 AND TNT.TenantID = TMM.TenantID where MM.Mcode=" + DB.SQuote(Mcode) + "");

                if (itemcount != 0)
                {
                    string status = "Failed";
                    return status;
                }
            }
            var UpsertBasic = JsonConvert.DeserializeXmlNode(BasicData, "root").OuterXml;
            DataSet dsInfo = DB.GetDS("EXEC [USP_UPSERT_MMTMasterXMLData] @DataXml='" + UpsertBasic + "',@LoggedInUserID=" + LoggedInUserID + ",@TenantID=" + TenantID + ",@AccountID=" + AccountID, false);
            //PartNum = dsInfo.Tables
            return Newtonsoft.Json.JsonConvert.SerializeObject(dsInfo);
            
           
        }

        [WebMethod]
        public static string PartCheck(int TenantID, string MCode)
        {
            string PartCheck = "";
            int PCheck = DB.GetSqlN("SELECT count(*) AS N FROM MMT_MaterialMaster WHERE IsActive=1 AND IsDeleted=0 and TenantID=" + TenantID + "and MCode=" + DB.SQuote(MCode));
            if(PCheck != 0)
            {
                PartCheck = "Exists";
            }
            else
            {
                PartCheck = "NotExists";
            }
            return PartCheck;
        }

        [WebMethod]
        public static string UpsertCopyBasicDetails(string BasicData, int AccountID, int TenantID, int LoggedInUserID, int MaterialMasterID, int prevMaterialMasterID, string Mcode)
        {

            int itemcount = DB.GetSqlN("select COUNT(*) AS N from  MMT_MaterialMaster MM INNER JOIN TPL_Tenant TNT ON TNT.TenantID = " + TenantID + " AND TNT.IsActive = 1 AND TNT.IsDeleted = 0 AND TNT.AccountID = " + AccountID + " INNER JOIN TPL_Tenant_MaterialMaster TMM ON TMM.MaterialMasterID = MM.MaterialMasterID AND MM.IsActive = 1 AND MM.IsDeleted = 0 AND TNT.TenantID = TMM.TenantID where MM.Mcode=" + DB.SQuote(Mcode) + "");
            if (itemcount == 0)
            {                       
                var UpsertBasic = JsonConvert.DeserializeXmlNode(BasicData, "root").OuterXml;
                DataSet dsInfo = DB.GetDS("EXEC [USP_SET_MaterialCopy] @DataXml='" + UpsertBasic + "',@LoggedInUserID=" + LoggedInUserID + ",@TenantID=" + TenantID + ",@AccountID=" + AccountID + ",@OldMID=" + prevMaterialMasterID, false);
                //PartNum = dsInfo.Tables
                return Newtonsoft.Json.JsonConvert.SerializeObject(dsInfo);
            }
            else
            {
                string status = "Failed";
                return status;
            }
        }

        [WebMethod]
        public static string UpsertAddlnInfo(string AddlnData, int TenantID, int LoggedInUserID)
        {
            var UpsertAddln = JsonConvert.DeserializeXmlNode(AddlnData, "root").OuterXml;
            StringBuilder sb = new StringBuilder();
            sb.Append("EXEC [USP_UPSERT_MMTMasterXMLData] @DataXml='" + UpsertAddln + "',@LoggedInUserID=" + LoggedInUserID + ",@TenantID=" + TenantID);
            DB.ExecuteSQL(sb.ToString());
            return "";
        }

        [WebMethod]
        public static string UpsertSupDetailsInfo(string ltMMT_SupID, string MaterialMasterID, string vSUPID, int TenantID, string vUnitCost, string vDeliveryTime, int CurrencyID, string txtSupplierPartNumber, string vInitialOrderQty, int CreatedBy)
        {
            string POCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckPOForDelete] @MaterialMasterID=" + MaterialMasterID + ",@SupplierIDs=" + vSUPID);
            if (POCheck != "")
            {
                return POCheck;
            }
            StringBuilder sqlSupDetails = new StringBuilder(2500);
            sqlSupDetails.Append("EXEC [dbo].[sp_MMT_UpsertMaterialMaster_Supplier]   ");
            sqlSupDetails.Append("@MaterialMaster_SupplierID=" + ltMMT_SupID);
            sqlSupDetails.Append(",@MaterialMasterID=" + MaterialMasterID);
            sqlSupDetails.Append(",@SupplierID=" + vSUPID);
            sqlSupDetails.Append(",@TenantID=" + TenantID);
            sqlSupDetails.Append(",@ExpectedUnitCost=" + vUnitCost);
            sqlSupDetails.Append(",@PlannedDeliveryTime=" + vDeliveryTime);
            sqlSupDetails.Append(",@InitialOrderQuantity=" + vInitialOrderQty);
            sqlSupDetails.Append(",@CurrencyID=" + (CurrencyID == 0 ? "NULL" : CurrencyID.ToString()));
            sqlSupDetails.Append(",@CreatedBy=" + CreatedBy);
            sqlSupDetails.Append(",@SupplierPartNo=" + (txtSupplierPartNumber == "" ? "Null" : DB.SQuote(txtSupplierPartNumber)));
            DB.ExecuteSQL(sqlSupDetails.ToString());
            return "";
        }

        [WebMethod]
        public static string UpsertUoMInfo(string ltMMT_GUoMID, string MaterialMasterID, string UoMTypeID, int TenantID, int UoMID, string UoMQty, int CreatedBy)
        {
            int UoMCheck = DB.GetSqlN("EXEC [dbo].[sp_MMT_CheckIsUoMConfigured] @MaterialMasterUoMID=" + ltMMT_GUoMID);
            if (UoMCheck != 0)
            {
                var CheckUoM = UoMCheck.ToString();
                return CheckUoM;
            }
            StringBuilder sqlUoMDetails = new StringBuilder(2500);
            sqlUoMDetails.Append("EXEC  [dbo].[sp_MMT_UpsertMaterialMaster_GEN_UoM]    @MaterialMaster_UoMID=" + ltMMT_GUoMID + ",@TenantID=" + TenantID + ",@MaterialMasterID=" + MaterialMasterID + ",@UoMTypeID=" + UoMTypeID + ",@UoMID=" + UoMID + ",@UoMQty=" + UoMQty + ",@CreatedBy=" + CreatedBy);
            DB.ExecuteSQL(sqlUoMDetails.ToString());
            return "";
        }
        
        [WebMethod]
        public static string UpsertQCInfo(string vMMT_QualityParameterID, string MaterialMasterID, string vQualityParameterID, int TenantID, int vIsRequired, string vtxtMinValue, string vtxtMaxValue, int CreatedBy)
        {
            if(vMMT_QualityParameterID != "0")
            {
                string POChecks = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckPO] @MaterialMasterID=" + MaterialMasterID + ",@TenantID=" + TenantID);
                if (POChecks != "")
                {
                    return POChecks;
                }
            }            
            StringBuilder sql = new StringBuilder(2500);
            sql.Append("EXEC  [dbo].[sp_MMT_UpsertMaterialMaster_QualityParameters]   ");
            sql.Append("@MaterialMaster_QualityParameterID=" + vMMT_QualityParameterID);
            sql.Append(",@MaterialMasterID=" + MaterialMasterID);
            sql.Append(",@QualityParameterID=" + vQualityParameterID);
            sql.Append(",@IsRequired=" + vIsRequired);
            sql.Append(",@Cratedby=" + CreatedBy);
            sql.Append(",@MinTolerance=" + (vtxtMinValue != "0" ? DB.SQuote(vtxtMinValue) : "0.00"));
            sql.Append(",@MaxTolerance=" + (vtxtMaxValue != "0" ? DB.SQuote(vtxtMaxValue) : "0.00"));
            DB.ExecuteSQL(sql.ToString());
            return "";
        }

        [WebMethod]
        public static string UpsertRepInfo(string RepData, int TenantID, int LoggedInUserID)
        {
            var UpsertAddln = JsonConvert.DeserializeXmlNode(RepData, "root").OuterXml;
            StringBuilder sb = new StringBuilder();
            sb.Append("EXEC [USP_UPSERT_MMTMasterXMLData] @DataXml='" + UpsertAddln + "',@LoggedInUserID=" + LoggedInUserID + ",@TenantID=" + TenantID);
            DB.ExecuteSQL(sb.ToString());
            return "";
        }

        [WebMethod]
        public static string GetBinVolume(int LocationID)
        {
            string Json = "";
            string IMDList;
            try
            {
                IMDList = "SELECT DISTINCT LocationID, Location, [Length], [Width], [Height], [MaxWeight] FROM INV_Location	WHERE IsActive = 1 AND IsDeleted = 0 AND IsFastMoving=1 AND LocationID=" + LocationID;
                DataSet ds = DB.GetDS(IMDList, false);
                Json = JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                Json = "Failed";
            }
            return Json;

        }


        protected void resetError(string error, bool isError)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);
        }

        [WebMethod]
        public static string DeleteSupDetails(int MaterialMasterID, int rfidIDs, int TenantID, int SupplierIDs)
        {
            StringBuilder sqlDeleteString = new StringBuilder();
            string POCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckPOForDelete] @MaterialMasterID="+ MaterialMasterID + ",@SupplierIDs="+ SupplierIDs);
            if (POCheck != "")
            {
                return POCheck;
            }
            sqlDeleteString.Append("UPDATE MMT_MaterialMaster_Supplier SET IsDeleted=1 , IsActive=0 WHERE MaterialMaster_SupplierID =" + rfidIDs);
            DB.ExecuteSQL(sqlDeleteString.ToString());
            return "";
        }

        [WebMethod]
        public static string DeleteUoMDetails(int rfidIDs)
        {
            StringBuilder sqlDeleteString = new StringBuilder();
            int UoMCheck = DB.GetSqlN("EXEC [dbo].[sp_MMT_CheckIsUoMConfigured] @MaterialMasterUoMID=" + rfidIDs);
            if(UoMCheck != 0)
            {
                var CheckUoM = UoMCheck.ToString();
                return CheckUoM;
            }
            sqlDeleteString.Append(" Update MMT_MaterialMaster_GEN_UoM set IsDeleted=1, IsActive=0 Where MaterialMaster_UoMID=" + rfidIDs);
            DB.ExecuteSQL(sqlDeleteString.ToString());
            return "";
        }

        [WebMethod]
        public static string DeleteQCDetails(int MaterialMasterID,int rfidIDs, int TenantID)
        {
            StringBuilder sqlDeleteString = new StringBuilder();
            string POChecks = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckPO] @MaterialMasterID=" + MaterialMasterID + ",@TenantID=" + TenantID);
            if (POChecks != "")
            {
                return POChecks;
            }
            sqlDeleteString.Append(" UPDATE MMT_MaterialMaster_QualityParameters SET IsDeleted=1 , IsActive=0 WHERE MaterialMaster_QualityParameterID=" + rfidIDs);
            DB.ExecuteSQL(sqlDeleteString.ToString());
            return "";
        }

        protected void ItemPicSave_Click(object sender, EventArgs e)
        {
            if (fuItemPicture.HasFile)
            {
                String FileExtenion = Path.GetExtension(fuItemPicture.FileName);
                string TenantRootDir = DB.GetSqlS("select DefaultValue AS S from SYS_SystemConfiguration SYS_C left join SYS_SysConfigKey SYS_K on SYS_K.SysConfigKeyID=SYS_C.SysConfigKeyID where SysConfigKey='TenantContentPath'");

                // Get MaterialMaster Path
                string MMTPath = DB.GetSqlS(" select DefaultValue AS S from SYS_SystemConfiguration SYS_C left join SYS_SysConfigKey SYS_K on SYS_K.SysConfigKeyID=SYS_C.SysConfigKeyID where SysConfigKey='MaterialManagementPath'");

                DataSet mt = DB.GetDS("select MCode, TenantID from MMT_MaterialMaster where MaterialMasterID=" + mid, false);
                string _MCode = mt.Tables[0].Rows[0]["MCode"].ToString();
                string TenantIDLS = mt.Tables[0].Rows[0]["TenantID"].ToString();
                int TenantIDL = Convert.ToInt32(TenantIDLS);

                //Catalog Number
                string MCatlogNumber = _MCode;

                if (FileExtenion == ".jpeg" || FileExtenion == ".JPEG" || FileExtenion == ".jpg" || FileExtenion == ".JPG" || FileExtenion == ".gif" || FileExtenion == ".GIF" || FileExtenion == ".png" || FileExtenion == ".PNG")
                {
                    UploadItemFile(TenantRootDir + TenantIDL + MMTPath, fuItemPicture, MCatlogNumber + Path.GetExtension(fuItemPicture.FileName));
                    //CommonLogic.UploadFile(TenantRootDir + TenantIDL + MMTPath, fuItemPicture, MCatlogNumber + fuItemPicture.FileName);
                }
                else
                {
                    resetError("Please upload a valid file  <br/>", true);
                    return;
                }
            }
            //resetError("Successfully Uploaded Item Pictures <br/>", false); 
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test11", "successitemsave();", true);
            //Response.Redirect("ItemMasterRequest.aspx?mid=" + mid);s
            //LoadMMItemData();

        }


        // This method fires when user selects any from measurement
        [WebMethod]
        public static string Build_FromMeasurement(string MeasurementID)
        {
            string Json = "";
            string MeasureList;
            try
            {
                MeasureList = "select convert(nvarchar,isnull(MPM.MetricPrifixID,0))+','+convert(nvarchar,MTM.MeasurementID)MeasurementID,IIF(MPM.MetricPrifixID IS NOT NULL, MPM.MetricPrifixName+' '+MTM.MeasurementName,MTM.MeasurementName) Measurement from MES_MeasurementMaster MTM LEFT JOIN MES_MatricPrefixMaster MPM ON MTM.ConversionTypeID=1 where MeasurementTypeID=" + MeasurementID + " ORDER BY Measurement";
                DataSet ds = DB.GetDS(MeasureList, false);
                Json = JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                Json = "Failed";
            }
            return Json;
        }

        [WebMethod]
        public static string UoMConversion(string MeasurementTypeID, string FromMeasurements0, string FromMeasurements1, string ToMeasurements0, string ToMeasurements1)
        {
            String UOMData = "";
            String UoMConvert = "select ISNULL(MTD.ConvesionValue,1)*IIF(ISNULL(MPM1.PowerOf,0)-ISNULL(MPM.PowerOf,0)<0,1/POWER(convert(float,10),ISNULL(MPM.PowerOf,0)-ISNULL(MPM1.PowerOf,0)),POWER(convert(float,10),ISNULL(MPM1.PowerOf,0)-ISNULL(MPM.PowerOf,0))) N "
                                        + "from MES_MeasurementMaster MTM "
                                        + "LEFT JOIN MES_MatricPrefixMaster MPM ON MTM.ConversionTypeID=1 and MPM.MetricPrifixID=" + ToMeasurements0
                                        + "left join MES_MatricPrefixMaster MPM1 ON MPM1.MetricPrifixID=" + FromMeasurements0
                                        + "LEFT join MES_MeasurementDetails MTD on MTD.MeasurementID=" + FromMeasurements1 + " AND MTD.ToMesurementID=MTM.MeasurementID "
                                        + "where MTM.MeasurementTypeID=" + MeasurementTypeID + " and MTM.MeasurementID=" + ToMeasurements1;
            IDataReader rsGetConversion = DB.GetRS(UoMConvert);
            if (rsGetConversion.Read())
            {
                UOMData = "Conversion : " + DB.RSFieldDouble(rsGetConversion, "N");
            }
            rsGetConversion.Close();
            return UOMData;            
        }
        //public void Build_FromMeasurement(String MeasurementID)
        //{
        //    ddlFromMeasurement.Items.Clear();
        //    ddlToMeasurement.Items.Clear();
        //    IDataReader rsGetFromMeasurement = DB.GetRS("select convert(nvarchar,isnull(MPM.MetricPrifixID,0))+','+convert(nvarchar,MTM.MeasurementID)MeasurementID,IIF(MPM.MetricPrifixID IS NOT NULL, MPM.MetricPrifixName+' '+MTM.MeasurementName,MTM.MeasurementName) Measurement from MES_MeasurementMaster MTM LEFT JOIN MES_MatricPrefixMaster MPM ON MTM.ConversionTypeID=1 where MeasurementTypeID=" + MeasurementID + " ORDER BY Measurement");
        //    ddlFromMeasurement.Items.Add(new ListItem("Select", "0"));
        //    ddlToMeasurement.Items.Add(new ListItem("Select", "0"));
        //    while (rsGetFromMeasurement.Read())
        //    {
        //        ddlFromMeasurement.Items.Add(new ListItem(DB.RSField(rsGetFromMeasurement, "Measurement"), DB.RSField(rsGetFromMeasurement, "MeasurementID")));
        //        ddlToMeasurement.Items.Add(new ListItem(DB.RSField(rsGetFromMeasurement, "Measurement"), DB.RSField(rsGetFromMeasurement, "MeasurementID")));
        //    }
        //    rsGetFromMeasurement.Close();
        //}

        //Measurement OnSelectedIndex
        //protected void ddlMeadureType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Build_FromMeasurement(ddlMeadureType.SelectedValue);

        //    if (ddlToMeasurement.SelectedValue == "0" && ddlFromMeasurement.SelectedValue == "0")
        //    {
        //        lbConversion.Text = "";
        //    }
        //    Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "fnScroll();", true);
        //   // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "fnScroll();", true);

        //}

        // This method toggles the uom conversion are
        //protected void lnkUoMConversion_Click(object sender, EventArgs e)
        //{
        //    if (trMeasurements.Visible == true || ddlMeadureType.SelectedValue == "0")
        //    {
        //        tdConversion.Visible = false;
        //        trMeasurements.Visible = false;
        //    }
        //    else
        //    {
        //        tdConversion.Visible = true;
        //        trMeasurements.Visible = true;
        //    }
        //}

        // This method fires when user selects from measurement
        //protected void ddlFromMeasurement_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    ddlToMeasurement.SelectedIndex = 0;
        //    lbConversion.Text = "";
        //}

        // This method fires when user selects to measurement
        //protected void ddlToMeasurement_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    lbConversion.Text = "";

        //    String[] FromMeasurements = ddlFromMeasurement.SelectedValue.Split(',');
        //    String[] ToMeasurements = ddlToMeasurement.SelectedValue.Split(',');

        //    if (FromMeasurements.Length == 2 && ToMeasurements.Length == 2)
        //    {
        //        String CmdConversion = "select ISNULL(MTD.ConvesionValue,1)*IIF(ISNULL(MPM1.PowerOf,0)-ISNULL(MPM.PowerOf,0)<0,1/POWER(convert(float,10),ISNULL(MPM.PowerOf,0)-ISNULL(MPM1.PowerOf,0)),POWER(convert(float,10),ISNULL(MPM1.PowerOf,0)-ISNULL(MPM.PowerOf,0))) N "
        //                                + "from MES_MeasurementMaster MTM "
        //                                + "LEFT JOIN MES_MatricPrefixMaster MPM ON MTM.ConversionTypeID=1 and MPM.MetricPrifixID=" + ToMeasurements[0]
        //                                + "left join MES_MatricPrefixMaster MPM1 ON MPM1.MetricPrifixID=" + FromMeasurements[0]
        //                                + "LEFT join MES_MeasurementDetails MTD on MTD.MeasurementID=" + FromMeasurements[1] + " AND MTD.ToMesurementID=MTM.MeasurementID "
        //                                + "where MTM.MeasurementTypeID=" + ddlMeadureType.SelectedValue + " and MTM.MeasurementID=" + ToMeasurements[1];
        //        IDataReader rsGetConversion = DB.GetRS(CmdConversion);
        //        if (rsGetConversion.Read())
        //        {
        //            lbConversion.Text = " Conversion : " + DB.RSFieldDouble(rsGetConversion, "N");
        //        }
        //        rsGetConversion.Close();
        //    }
        //    else
        //    {
        //        lbConversion.Text = "";
        //    }
        //}

        [WebMethod]
        public static string GetIndustries(int MaterialMasterID,string AccountID , int TenantId)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            string Json = "";
            try
            {
                DataSet ds = DB.GetDS("[dbo].[GEN_MST_Get_Material_Industries] @MaterialMasterID=" + MaterialMasterID + ",@AccountID=" + Convert.ToInt32(AccountID)+ ", @TenantId= "+Convert.ToInt32(TenantId) , false);
                Json = JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                Json = "Failed";
            }
            return Json;
        }

        [WebMethod]
        public static string SETIndustries(int UserID, string Inxml, int MM_MST_Material_ID, int TenantiD)
        {
            string Status = "";
            try
            {
                string POCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckPO] @MaterialMasterID=" + MM_MST_Material_ID + ",@TenantID=" + TenantiD);
                if (POCheck != "")
                {
                    Status = "POMapped";
                    return Status;
                }
                StringBuilder sb = new StringBuilder();
                sb.Append(" Exec [dbo].[USP_SET_Industry_Attributes] ");
                sb.Append(" @AinputDataXml='" + Inxml + "'");
                sb.Append(" ,@LoggedUserID=" + UserID);
                //sb.Append(" ,@LanguageType='en'");
                sb.Append(" ,@MM_MST_Material_ID=" + MM_MST_Material_ID);
                DB.ExecuteSQL(sb.ToString());
                Status = "success";
                //ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:UpsertIndustries(" + vResult + "); ", true);
            }
            catch (Exception ex)
            {
                Status = "Failed";
            }
            return Status;
        }

        [WebMethod]
        public static string SETIndustryID(int UserID, int IndustryID, int MM_MST_Material_ID ,int TenantiD)
        {
            string Status = "";
            try
            {
                string POCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckPO] @MaterialMasterID=" + MM_MST_Material_ID + ",@TenantID=" + TenantiD);
                if (POCheck != "")
                {
                    Status = "POMapped";
                    return Status;
                }
                StringBuilder sb = new StringBuilder();
                sb.Append("Update MMT_MaterialMaster SET IndustryID=" + IndustryID + ",UpdatedBy="+ UserID +" WHERE MaterialMasterID=" + MM_MST_Material_ID);                
                DB.ExecuteSQL(sb.ToString());
                Status = "success";
                //ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:UpsertIndustries(" + vResult + "); ", true);
            }
            catch (Exception ex)
            {
                Status = "Failed";
            }
            return Status;
        }

        [WebMethod]
        public static string GetPreferences(int MaterialMasterID)
        {
            string Json = "";
            try
            {
                DataSet ds = DB.GetDS("[dbo].[GEN_MST_Get_PreferenceGroups] @MaterialMasterID=" + MaterialMasterID, false);
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
            string Status = "";
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" Exec [dbo].[USP_SET_GEN_TRN_Preferences] ");
                sb.Append(" @inputDataXml='" + Inxml + "'");
                sb.Append(" ,@LoggedUserID=" + UserID);
                //sb.ToString();
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
        public static string SETMsps(int UserID, string Inxml)
        {
            string Status = "";
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" Exec [dbo].[USP_UPSERT_MMTMSPXMLData] ");
                sb.Append(" @DataXml='" + Inxml + "'");
                sb.Append(" ,@LoggedInUserID=" + UserID);
                //sb.ToString();
                DB.ExecuteSQL(sb.ToString());
                Status = "success";
            }
            catch (Exception ex)
            {
                Status = "Failed";
            }
            return Status;
        }

        protected void middailogbox()
        {
            string TenantIDLS = "0";
            string _MCode = "";
            if (mid != null && mid != "")
            {
                //Get Tenant Root Directory
                TenantRootDir = DB.GetSqlS("select DefaultValue AS S from SYS_SystemConfiguration SYS_C left join SYS_SysConfigKey SYS_K on SYS_K.SysConfigKeyID=SYS_C.SysConfigKeyID where SysConfigKey='TenantContentPath'");

                // Get MaterialMaster Path
                MMTPath = DB.GetSqlS(" select DefaultValue AS S from SYS_SystemConfiguration SYS_C left join SYS_SysConfigKey SYS_K on SYS_K.SysConfigKeyID=SYS_C.SysConfigKeyID where SysConfigKey='MaterialManagementPath'");

                //_MCode = DB.GetSqlS("select MCode AS S from MMT_MaterialMaster where MaterialMasterID=" + mid);

                DataSet mt = DB.GetDS("select MCode, TenantID from MMT_MaterialMaster  where isactive=1 and isdeleted=0 and  MaterialMasterID=" + mid, false);
                if (mt.Tables[0].Rows.Count > 0) { 
                //string _MCode = mt.Tables[0].Rows[0]["MCode"].ToString();
                //string TenantIDLS = mt.Tables[0].Rows[0]["TenantID"].ToString();
                  TenantIDLS = mt.Tables[0].Rows[0]["TenantID"].ToString();
                  _MCode = mt.Tables[0].Rows[0]["MCode"].ToString();
                }
                int TenantIDL = Convert.ToInt32(TenantIDLS);

                lblvieweattachment.Text = "<a style=\"text-decoration:none;\" href=\"javascript:openDialog(\'View Attachment Files\')\">   View Attachments  <span  class=\"space fa fa-external-link\"></span> </a>";
                //String loacalfolder = TenantRootDir + DB.GetSqlS("select UniqueID AS S from GEN_Tenant where TenantID=" + TenantID) + MMTPath + mid;
                String loacalfolder = TenantRootDir + TenantIDL + MMTPath + mid;
                //String loacalfolder = TenantRootDir + MMTPath + mid;

                //String midSavepath = System.Web.HttpContext.Current.Server.MapPath("~/") + loacalfolder;
                String midSavepath = System.Web.HttpContext.Current.Server.MapPath("~\\" + loacalfolder); 
                String sidpath = "";

                trvmaterialattachment.Nodes[0].ChildNodes.Clear();

                TreeNode trnInternalOrder;
                TreeNode trnsubInternalOrder;
                trvmaterialattachment.Nodes[0].Text = _MCode;
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
                            trnsubInternalOrder.NavigateUrl = Attachmentname.EndsWith(".pdf") ? String.Format("DisplayPDF.aspx?sid={0}&mid={1}&filename={2}&tid={3}", DB.RSFieldInt(rsGetSupplierList, "SupplierID"), mid, Attachmentname, TenantIDL) : "../" + loacalfolder + "/" + DB.RSFieldInt(rsGetSupplierList, "SupplierID") + "/" + Attachmentname;
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

        public void LoadMMItemData()
        {
            string TenantIDLS = "0";
            string _MCode = "";
            //Get Attatchment file
            //String sFileName = CommonLogic._GetAttatchmentFile(TenantRootDir + DB.GetSqlS("select UniqueID AS S from GEN_Tenant where TenantID=" + TenantID) + MMTPath, _MCode);
            DataSet mt = DB.GetDS("select MCode, TenantID from MMT_MaterialMaster where isactive=1 and isdeleted=0 and MaterialMasterID=" + mid, false);
            if (mt.Tables[0].Rows.Count > 0)
            {
                 _MCode = mt.Tables[0].Rows[0]["MCode"].ToString();
                 TenantIDLS = mt.Tables[0].Rows[0]["TenantID"].ToString();
            }
            int TenantIDL = Convert.ToInt32(TenantIDLS);

            String sFileName = CommonLogic._GetAttatchmentFile(TenantRootDir + TenantIDL + MMTPath, _MCode);

                if (sFileName != "")
                {
                    ltPicHolder.Text = "<img border='0' height='140' width='180' src='../" + sFileName + "'/>";
                }
                else
                {
                    ltPicHolder.Text = "";
                }   

        }

        public static bool UploadItemFile(String PathName, FileUpload FileControl, String FileName)
        {
            bool status = false;

            if (FileControl.HasFile)
            {
                try
                {
                    var _Path = System.Web.HttpContext.Current.Server.MapPath("~/" + PathName);

                    var _Directory = new DirectoryInfo(_Path);

                    if (_Directory.Exists == false)
                    {
                        _Directory.Create();
                    }

                    var file = Path.Combine(_Path, FileName);

                    //if (File.Exists(file))
                    //    File.Delete(file);
                    //Array.ForEach(Directory.GetFiles(_Path), File.Delete);
                    foreach (FileInfo allfiles in _Directory.GetFiles())
                    {
                        allfiles.Delete();
                    }

                    FileControl.SaveAs(file);

                    status = true;

                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            return status;
        }
    }
}