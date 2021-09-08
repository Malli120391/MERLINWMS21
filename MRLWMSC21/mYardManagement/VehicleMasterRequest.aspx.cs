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

namespace MRLWMSC21.mYardManagement
{
    public partial class VehicleMasterRequest : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        public String vid = CommonLogic.QueryString("vid");
        protected void Page_PreInit(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Vehicle Master Request");
                if ((CommonLogic.QueryString("vid") != "") && (CommonLogic.QueryString("vid") != "null"))
                {
                    GetVehicles(Convert.ToInt32(vid));
                }
            }
        }

        protected void lnkBasicVehicleSave_Click(object sender, EventArgs e)
        {
            //if(Account.Value == "" || txtAccount.Value.Trim() == "")
            //{
            //    resetError("Please Select Account", true);
            //    return;
            //}
            //if (Freight.Value == "" || txtFreight.Value.Trim() == "")
            //{
            //    resetError("Please Select Freight Company", true);
            //    return;
            //}
            //if (VehicleType.Value == "" || txtVehicleType.Value.Trim() == "")
            //{
            //    resetError("Please Select Vehicle Type", true);
            //    return;
            //}
            //if (RegdNumber.Value.Trim() == "")
            //{
            //    resetError("Please Enter Registration Number", true);
            //    return;
            //}
            //if (OwnerName.Value.Trim() == "")
            //{
            //    resetError("Please Enter Owner Name", true);
            //    return;
            //}
            //if (OContactNumber.Value.Trim() == "")
            //{
            //    resetError("Please Enter Owner Contact Number", true);
            //    return;
            //}
            ////if (Convert.ToDecimal(VWidth.Value.Trim()) < Convert.ToDecimal(SWidth.Value.Trim()))
            ////{
            ////    resetError("Vehicle width Should be greater than Storage Width ", true);
            ////    return;
            ////}
            //if (SUoM.Value == "" || txtSUoM.Value == "" || SLength.Value.Trim() == "" || SWidth.Value.Trim() == "" || SHeight.Value.Trim() == "")
            //{
            //    resetError("Please Enter All Mandatory Fields in Storage Dimensions", true);
            //    return;
            //}
            //if (VUoM.Value == "" || txtVUoM.Value == "" || VLength.Value.Trim() == "" || VWidth.Value.Trim() == "" || VHeight.Value.Trim() == "")
            //{
            //    resetError("Please Enter All Mandatory Fields in Volume Dimensions", true);
            //    return;
            //}

            //if (WUoM.Value == "" || txtWUoM.Value == "" || WUoM.Value.Trim() == "")
            //{
            //    resetError("Please  Select the  Weight Dimensions",true);
            //    return;
            //}
            if (SLength.Value.Trim() == "0" || SWidth.Value.Trim() == "0" || SHeight.Value.Trim() == "0")
            {
                resetError("Storage Dimensions values should not be zero ", true);
                return;
            }
            if (VLength.Value.Trim() == "0" || VWidth.Value.Trim() == "0" || VHeight.Value.Trim() == "0")
            {
                resetError("Vehicle Dimensions values should not be zero ", true);
                return;
            }
            if (WStorage.Value.Trim() == "0" || WTare.Value.Trim() == "0" || WStorage.Value.Trim() == "0.000" || WTare.Value.Trim() == "0.000")
            {
                resetError("Weight Dimensions values should not be zero ", true);
                return;
            }


            string Vehicleid = vid == "" ? "0" : vid;
            int AccountID = Convert.ToInt32(Account.Value);
            int TenantID = Convert.ToInt32(tenant.Value);
            int VehicleTypeID = Convert.ToInt32(VehicleType.Value);
            string RegdNum = RegdNumber.Value.ToString();
            RegdNum = RegdNum.Replace(" ", String.Empty);
            string Ownername = OwnerName.Value.ToString();
            string OwnerContact = OContactNumber.Value.ToString();
            int FreightID = Convert.ToInt32(Freight.Value);
            int StorageUoM = SUoM.Value==""? 0:Convert.ToInt32(SUoM.Value);
            string SLengthv = SLength.Value;
            Decimal SLengthv1 = Convert.ToDecimal(SLength.Value);
            string SWidthv = SWidth.Value;
            Decimal SWidthv1 = Convert.ToDecimal(SWidthv);
            string SHeightv = SHeight.Value;
            Decimal SHeightv1 = Convert.ToDecimal(SHeight.Value);
            int VehicleUoM = Convert.ToInt32(VUoM.Value);
            string VLengthv = VLength.Value;
            Decimal VLengthv1 = Convert.ToDecimal(VLength.Value);
            string VWidthv = VWidth.Value;
            Decimal VWidthv1 = Convert.ToDecimal(VWidthv);
            string VHeightv = VHeight.Value;
            Decimal VHeightv1 = Convert.ToDecimal(VHeight.Value);
            int WeightUoM = WUoM.Value== ""? 0: Convert.ToInt32(WUoM.Value);
            string TareW = WTare.Value;
            string StorageW = WStorage.Value;
            string TotalW = WTotal.Value;
            if (VWidthv1 < SWidthv1)
            {
                //if (Convert.ToDecimal(VWidth.Value.Trim()) < Convert.ToDecimal(SWidth.Value.Trim()))
                //{
                resetError("Vehicle width Should be greater than Storage Width ", true);
                return;
                //}
            }
            if (VLengthv1 < SLengthv1)
            {
               
                resetError("Vehicle length Should be greater than Storage length ", true);
                return;
              
            }
            if (VHeightv1 < SHeightv1)
            {

                resetError("Vehicle height Should be greater than Storage height ", true);
                return;

            }
            int s = DB.GetSqlN("select count(RegistrationNumber) as N from YM_MST_Vehicles where RegistrationNumber='" + RegdNumber.Value.Trim() + "' and AccountID='"+cp.AccountID+ "' and YM_MST_Vehicle_ID!="+ Vehicleid + "  ");
            if (s >=1)
            {
                resetError("Vehicle Registration Number already Exists", true);
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("<root><data><YM_MST_Vehicle_ID>" + Vehicleid + "</YM_MST_Vehicle_ID>") ;
            sb.Append("<AccountID>" + AccountID + "</AccountID><TenantID>" + TenantID + "</TenantID><YM_MST_VehicleType_ID>" + VehicleTypeID + "</YM_MST_VehicleType_ID>");
            sb.Append("<RegistrationNumber>" + RegdNum.Trim() + "</RegistrationNumber><OwnerName>" + Ownername + "</OwnerName><OwnerContact>" + OwnerContact + "</OwnerContact>");
            sb.Append("<FreightCompanyID>" + FreightID + "</FreightCompanyID>");
            sb.Append("<DimensionUoM>" + StorageUoM + "</DimensionUoM><StorageLength> " + SLengthv + " </StorageLength><StorageWidth>" + SWidthv + "</StorageWidth><StorageHeight>" + SHeightv + "</StorageHeight>");
            sb.Append("<VehicleDimensionUoM>" + VehicleUoM + "</VehicleDimensionUoM><VehicleLength> " + VLengthv + " </VehicleLength><VehicleWidth>" + VWidthv + "</VehicleWidth><VechicleHeight>" + VHeightv + "</VechicleHeight>");
            sb.Append("<WeightUoM>" + WeightUoM + "</WeightUoM><TareWeight> " + TareW + " </TareWeight><MaxStorageWeight>" + StorageW + "</MaxStorageWeight><MaxTotalWeight>" + TotalW + "</MaxTotalWeight>");
            sb.Append("</data></root> ");

            DataSet dsInfo = DB.GetDS("EXEC [dbo].[USP_UPSERT_YM_MST_Vehicles] @DataXml='" +sb + "', @LoggedInUserID=" + cp.UserID, false);
            vid = dsInfo.Tables[0].Rows[0]["VehicleID"].ToString();            
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test11", "Success('" + vid + "');", true);
        }

        public void GetVehicles(int Vehicleid)
        {
            DataSet dsInfo = DB.GetDS("EXEC [dbo].[USP_Get_YM_MST_VehiclesWithUoMs] @AccountID_New="+cp.AccountID+",@YM_MST_Vehicle_ID=" + Vehicleid, false);

            if (dsInfo.Tables[0].Rows.Count > 0)
            {
                Account.Value = dsInfo.Tables[0].Rows[0]["AccountID"].ToString();
                tenant.Value = dsInfo.Tables[0].Rows[0]["TenantID"].ToString();
                txtTenant.Value = dsInfo.Tables[0].Rows[0]["TenantName"].ToString();
                txtAccount.Value = dsInfo.Tables[0].Rows[0]["Account"].ToString();
                Freight.Value = dsInfo.Tables[0].Rows[0]["FreightCompanyID"].ToString();
                txtFreight.Value = dsInfo.Tables[0].Rows[0]["FreightCompany"].ToString();
                VehicleType.Value = dsInfo.Tables[0].Rows[0]["YM_MST_VehicleType_ID"].ToString();
                txtVehicleType.Value = dsInfo.Tables[0].Rows[0]["VehicleType"].ToString();
                RegdNumber.Value = dsInfo.Tables[0].Rows[0]["RegistrationNumber"].ToString();
                OwnerName.Value = dsInfo.Tables[0].Rows[0]["OwnerName"].ToString();
                OContactNumber.Value = dsInfo.Tables[0].Rows[0]["OwnerContact"].ToString();
                SUoM.Value = dsInfo.Tables[0].Rows[0]["DYM_MST_UoM_ID"].ToString();
                txtSUoM.Value = dsInfo.Tables[0].Rows[0]["UoMCode"].ToString();
                SLength.Value = dsInfo.Tables[0].Rows[0]["StorageLength"].ToString();
                SWidth.Value = dsInfo.Tables[0].Rows[0]["StorageWidth"].ToString();
                SHeight.Value = dsInfo.Tables[0].Rows[0]["StorageHeight"].ToString();
                VUoM.Value = dsInfo.Tables[0].Rows[0]["DYM_MST_UoM_ID"].ToString();
                txtVUoM.Value = dsInfo.Tables[0].Rows[0]["UoMCode"].ToString();
                VLength.Value = dsInfo.Tables[0].Rows[0]["VehicleLength"].ToString();
                VWidth.Value = dsInfo.Tables[0].Rows[0]["VehicleWidth"].ToString();
                VHeight.Value = dsInfo.Tables[0].Rows[0]["VechicleHeight"].ToString();
                WUoM.Value = dsInfo.Tables[0].Rows[0]["WYM_MST_UoM_ID"].ToString();
                txtWUoM.Value = dsInfo.Tables[0].Rows[0]["WUOM"].ToString();
                WTare.Value = dsInfo.Tables[0].Rows[0]["TareWeight"].ToString();
                WStorage.Value = dsInfo.Tables[0].Rows[0]["MaxStorageWeight"].ToString();
                WTotal.Value = dsInfo.Tables[0].Rows[0]["MaxTotalWeight"].ToString();
            }
            else
            {
                resetError("No Data Available", true);
            }
        }

        protected void resetError(string error, bool isError)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);
        }
    }
}