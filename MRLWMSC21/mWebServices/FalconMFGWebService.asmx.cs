using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using MRLWMSC21Common;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace MRLWMSC21.mWebServices
{


    /// <summary>
    /// ssswwwww
    /// Summary description for FalconMFGWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    //[System.Web.Script.Services.ScriptService]
    public class FalconMFGWebService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public List<THHTWSData.KeyValueStruct> CheckUserLogin(String UserID, String Password)
        {
            List<THHTWSData.KeyValueStruct> sources = new List<THHTWSData.KeyValueStruct>();
            IDataReader objUserDetails = DB.GetRS("EXEC [dbo].[sp_MFG_GetUserDetailsIfLogin] @UserID='" + UserID + "',@Password='" + Password + "'");
            sources.Clear();
            while (objUserDetails.Read())
            {
                sources.Add(new THHTWSData.KeyValueStruct("UserID", DB.RSFieldInt(objUserDetails, "UserID").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("TenantID", DB.RSFieldInt(objUserDetails, "TenantID").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("Email", DB.RSField(objUserDetails, "Email").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("FirstName", DB.RSField(objUserDetails, "FirstName").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("LastName", DB.RSField(objUserDetails, "LastName").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("WorkCenterID", DB.RSFieldInt(objUserDetails, "WorkCenterID").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("WorkCenter", DB.RSField(objUserDetails, "WorkCenter").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("EmployeeCode", DB.RSField(objUserDetails, "EmployeeCode").ToString()));
            }
            return sources;

        }

        [WebMethod]
        public DataTable GetProductionOrderList(int WorkCenterID)
        {


            DataSet ds = DB.GetDS("[dbo].[sp_MFG_GetWorkCenterWiseProductionOrderList] @WorkCenterID=" + WorkCenterID, false);
            //DataSet ds = DB.GetDS("[sp_MFG_GetProductionOrderHeaderList] @PRORefNo='', @WorkCenterID=" + WorkCenterID, false);

            return ds.Tables[0];

        }
        [WebMethod]
        public DataTable GetOperatorActivityResuls(int vRoutingDetailsActivityID)
        {
            DataSet ds = DB.GetDS("	exec [dbo].[sp_MFG_GetActivityResults] @RoutingDetailsActivityID=" + vRoutingDetailsActivityID, false);
            return ds.Tables[0];
        }

        [WebMethod]
        public DataTable GetOperationActivities(int UserID, String OperationNumber, int ProductionOrderHeaderID)
        {


            DataSet ds = DB.GetDS("[dbo].[sp_MFG_GetActivities] @UserID=" + UserID + ",@OperationNumber=" + DB.SQuote(OperationNumber) + ",@ProductionOrderHeaderID=" + ProductionOrderHeaderID, false);

            return ds.Tables[0];

        }
        [WebMethod]
        public DataTable GetActivityLevelComponents(int RoutingDetailsActivityID, int ProductionOrderHeaderID)
        {


            DataSet ds = DB.GetDS("[dbo].[sp_MFG_GetActivityWiseComponents] @ProductionOrderHeaderID=" + ProductionOrderHeaderID + ",@ROutingDetailsActivityID=" + RoutingDetailsActivityID, false);

            return ds.Tables[0];

        }

        [WebMethod]
        public int GetUserRoleID(string empcode)
        {

            int roldeid = DB.GetSqlN("select GUR.UserRoleID as N from GEN_User GU JOIN GEN_User_UserRole GUU ON GUU.UserID=GU.UserID JOIN GEN_UserRole GUR ON GUR.UserRoleID=GUU.UserRoleID WHERE GU.EmployeeCode=" + DB.SQuote(empcode) + " AND (GUR.UserRoleID=22 OR GUR.UserRoleID=23 OR GUR.UserRoleID=24 OR GUR.UserRoleID=25)");
            return roldeid;
        }

        [WebMethod]
        public string UpsertGoodsMovementDetails(String EmployeeCode, String ProductionOrderHeaderID, String RoutingDetailsActivityCaptureID, String MCode, Decimal Qty, String WorkCenterID, int GoodsMovementTypeID, int IsDamaged, int IsLocallyProduced, String Remarks, String ScrapCode, String reworkroutingID, String SerialNumber, String BatchNumber)
        {
            try
            {
                int UserID = DB.GetSqlN("select UserID as N from GEN_User where EmployeeCode=" + DB.SQuote(EmployeeCode));

                int vMMID = CommonLogic.GetMMID(MCode);

                if (RoutingDetailsActivityCaptureID != "0")
                {
                    int RCID = DB.GetSqlN("select RoutingDetailsActivityCaptureID AS N from MFG_RoutingDetailsActivityCapture where RoutingDetailsActivityID=" + RoutingDetailsActivityCaptureID);
                    RoutingDetailsActivityCaptureID = RCID.ToString();
                }

                /*IDataReader dr = null;

              if (IsLocallyProduced == 0)
                {
                    dr = DB.GetRS("EXEC [sp_MFG_GetProductionOrderHeaderList] @PRORefNo='' ,@ProductionOrderHeaderID=" + ProductionOrderHeaderID + ",@MaterialMasterID=" + vMMID);
                }
                else
                {
                    dr = DB.GetRS("EXEC [dbo].[sp_MFG_GetLocallyProducedMaterialDetails]  @ProductionOrderHeaderID=" + ProductionOrderHeaderID + ",@MaterialMasterID=" + vMMID + ",@WorkCenterID=" + WorkCenterID);
                }
                dr.Read();
               
                string LocID = "0";
                if (LocationID != "0")
                {
                  // LocID = CommonLogic.GetMFGLocationID(LocationID, WorkCenterID).ToString();
                     if (LocID =="0")
                     {
                         return "Location is not configured can't receive";
                     }
                }*/
                StringBuilder sbSqlString = new StringBuilder();
                sbSqlString.Append("declare @NewResult int ");
                sbSqlString.Append(" EXEC  [dbo].[sp_MFG_UpsertGoodsMovementDetails] @GoodsMovementDetailsID=0");
                sbSqlString.Append(" ,@ProductionOrderHeaderID=" + ProductionOrderHeaderID);
                sbSqlString.Append(" ,@RoutingDetailsActivityCaptureID=" + (RoutingDetailsActivityCaptureID != "0" ? RoutingDetailsActivityCaptureID : "NULL"));
                sbSqlString.Append(" ,@OperationID=NULL");
                sbSqlString.Append(" ,@WorkCenterID=" + WorkCenterID);
                sbSqlString.Append(" ,@Quantity=" + Qty);
                sbSqlString.Append(" ,@MaterialMasterID=" + vMMID);
                sbSqlString.Append(" ,@IsDamaged=" + IsDamaged);
                sbSqlString.Append(" ,@SerialNumbers=" + (SerialNumber != "0" ? DB.SQuote(SerialNumber) : "NULL"));
                sbSqlString.Append(" ,@GoodsMovementTypeID=" + GoodsMovementTypeID);
                sbSqlString.Append(" ,@NonConfirmityCodeID=" + ((ScrapCode != "0" && ScrapCode != null) ? ScrapCode : "NULL"));
                sbSqlString.Append(" ,@ReworkRoutingID=" + (reworkroutingID != "0" ? reworkroutingID : "NULL"));
                sbSqlString.Append(" ,@Remarks=" + DB.SQuote(Remarks));
                sbSqlString.Append(" ,@IPAddress=NULL");
                sbSqlString.Append(" ,@BatchNo=" + DB.SQuote(BatchNumber));
                sbSqlString.Append(" ,@CreatedBy=" + UserID);
                sbSqlString.Append(" ,@IsLocallyProduced=" + IsLocallyProduced);
                sbSqlString.Append(",@Result=@NewResult OUTPUT ");
                sbSqlString.Append("SELECT @NewResult AS N");

                try
                {
                    string ResponceResult = "";
                    int result = DB.GetSqlN(sbSqlString.ToString());
                    if (result == 100)
                    {
                        ResponceResult = "This Materils Not Configured in BOM";
                    }
                    else if (result == 200)
                    {
                        ResponceResult = "Received Qty is more than BOM Qty ";
                    }
                    else if (result == 1)
                    {
                        ResponceResult = "1";
                    }
                    return ResponceResult;
                }
                catch
                {
                    return "Failed plese contact Suppout team";
                }
            }
            catch (Exception ex)
            {

                return "Failed plese contact Suppout team";
            }

        }

        [WebMethod]
        public int IsPassOrFailCaptured(int vRouteID, String EmpCode, String vProductionOrderID)
        {
            StringBuilder sbSqlString = new StringBuilder();
            sbSqlString.Append("declare @userid int;set @userid=(select UserID from GEN_User where EmployeeCode=" + DB.SQuote(EmpCode) + ");");
            sbSqlString.Append("SELECT ISNULL(IsFailed,2) AS TI FROM MFG_RoutingDetailsActivityCapture WHERE RoutingDetailsActivityID=" + vRouteID + " AND UserID= @userid AND ProductionOrderHeaderID=" + vProductionOrderID);
            /*sbSqlString.Append("declare @RouteCaptureID int;");
            sbSqlString.Append("set @RouteCaptureID=(select RoutingDetailsActivityCaptureID from  MFG_RoutingDetailsActivityCapture where UserID=@userid and ProductionOrderHeaderID="+vProductionOrderID+" and RoutingDetailsActivityID="+vRouteID+" and IsFailed=0);");
            sbSqlString.Append("select RoutingDetailsActivityCaptureID as N from MFG_RoutingDetailsActivityPassOrFail where RoutingDetailsActivityCaptureID=@RouteCaptureID");*/
            int result = DB.GetSqlTinyInt(sbSqlString.ToString());
            return result;
        }
        [WebMethod]
        public string UpsertRoutingDetailsActivityCapture(int RoutingdetailsActivityID, String ProductionOrderHeaderID, String UserCode, int StartTime, string Remarks)
        {
            int UserID = DB.GetSqlN("select UserID as N from GEN_User where EmployeeCode=" + DB.SQuote(UserCode));

            StringBuilder sbSqlString = new StringBuilder();
            sbSqlString.Append("declare @NewResult int");
            sbSqlString.Append(" EXEC  [dbo].[sp_MFG_UpsertRoutingDetailsActivityCapture]");
            sbSqlString.Append("@RoutingDetailsActivityID=" + RoutingdetailsActivityID);
            sbSqlString.Append(",@ProductionOrderHeaderID=" + ProductionOrderHeaderID);

            sbSqlString.Append(",@UserID=" + UserID);
            sbSqlString.Append(",@StartTime=" + (StartTime != 0 ? StartTime : 0));
            //sbSqlString.Append(",@PassOrFail=" + (PassOrFail));
            sbSqlString.Append(",@Remarks=" + DB.SQuote(Remarks));
            sbSqlString.Append(",@Result=@NewResult OUTPUT ");
            sbSqlString.Append("SELECT @NewResult AS N");
            try
            {
                int result = DB.GetSqlN(sbSqlString.ToString());
                return result.ToString();
            }
            catch
            {
                return "Failed plese contact Suppout team";
            }
        }
        [WebMethod]
        public bool UpsertRoutingDetailsActivityPassorFail(String RouteActivityID, String EmpCode, int PassOrFail, String vProductionOrderID, String vRemarks, string vCheckListResultData, int vUserRoleID)
        {

            if (vCheckListResultData != "0")
            {


                StringBuilder sbSqlStringforCheckList = new StringBuilder();
                sbSqlStringforCheckList.Append("EXEC sp_MFG_UpsertQualityParametersCapture @QualityParametersCaptureData=" + DB.SQuote(vCheckListResultData));
                sbSqlStringforCheckList.Append(",@RoutingDetailsActivityID=" + RouteActivityID);
                sbSqlStringforCheckList.Append(",@UserRoleID=" + vUserRoleID);

                DB.ExecuteSQL(sbSqlStringforCheckList.ToString());

            }
            StringBuilder sbSqlString = new StringBuilder();
            sbSqlString.Append("declare @userid int;set @userid=(select UserID from GEN_User where EmployeeCode=" + DB.SQuote(EmpCode) + ");");
            sbSqlString.Append("declare @RouteCaptureID int;");
            sbSqlString.Append("set @RouteCaptureID=(select RoutingDetailsActivityCaptureID from  MFG_RoutingDetailsActivityCapture where UserID=@userid and ProductionOrderHeaderID=" + vProductionOrderID + " and RoutingDetailsActivityID=" + RouteActivityID + " and IsFailed IS NULL);");
            sbSqlString.Append("select @RouteCaptureID as N");
            int vRoutingDetailsActivityCaptureID = DB.GetSqlN(sbSqlString.ToString());


            /*StringBuilder sbSqlStringFORUpsert = new StringBuilder();
            sbSqlStringFORUpsert.Append("EXEC [dbo].[sp_MFG_UpsertRoutingDetailsActivityPassOrFail] ");
            sbSqlStringFORUpsert.Append("@RoutingDetailsActivityCaptureID="+vRoutingDetailsActivityCaptureID);
            sbSqlStringFORUpsert.Append(",@RoutingDetailsActivityID=" + RouteActivityID);
            sbSqlStringFORUpsert.Append(",@IsPassed="+PassOrFail);
            sbSqlStringFORUpsert.Append(",@Remarks=" + DB.SQuote(vRemarks));*/
            StringBuilder sbSqlStringFORUpsert = new StringBuilder();
            if (PassOrFail == 1)
            {
                sbSqlStringFORUpsert.Append("UPDATE MFG_RoutingDetailsActivityCapture SET IsFailed=0,Remarks=" + DB.SQuote(vRemarks) + " WHERE RoutingDetailsActivityCaptureID=" + vRoutingDetailsActivityCaptureID);
            }
            else
            {
                sbSqlStringFORUpsert.Append("UPDATE MFG_RoutingDetailsActivityCapture SET IsFailed=1,EndTime=GETDATE(),Remarks=" + DB.SQuote(vRemarks) + " WHERE RoutingDetailsActivityCaptureID=" + vRoutingDetailsActivityCaptureID);
            }
            try
            {
                DB.ExecuteSQL(sbSqlStringFORUpsert.ToString());
                return true;
            }
            catch
            {
                return false;
            }

        }

        [WebMethod]
        public DataTable GetMaterialReceivedList(String ProductionOrderHeaderID, String WorkCenterID)
        {

            DataSet ds = null;

            try
            {

                ds = DB.GetDS("EXEC [sp_MFG_GetMaterialReceivedList]  @ProductionOrderHeaderID=" + ProductionOrderHeaderID + ",@WorkCenterID=" + WorkCenterID, false);

                return ds.Tables[0];

            }
            catch (Exception ex)
            {

                return null;
            }

        }


        [WebMethod]
        public DataTable GetMaterialConsumedList(String ProductionOrderHeaderID, String WorkCenterID)
        {

            DataSet ds = null;

            try
            {

                ds = DB.GetDS("EXEC [sp_MFG_GetMaterialConsumedList]  @ProductionOrderHeaderID=" + ProductionOrderHeaderID + ",@WorkCenterID=" + WorkCenterID, false);

                return ds.Tables[0];

            }
            catch (Exception ex)
            {

                return null;
            }

        }
        [WebMethod]
        public DataTable GetJobCardData(int WorkCenterID, int ProductionOrderID)
        {
            try
            {
                DataSet dataset = DB.GetDS("EXEC dbo.sp_MFG_GetWorkCenterWiseJobCard @ProductionOrderHeaderID=" + ProductionOrderID + ",@WorkCenterID=" + WorkCenterID, false);
                return dataset.Tables[0];

            }
            catch
            {
                return null;
            }

        }
        [WebMethod]
        public List<THHTWSData.KeyValueStruct> GetWorkCenterWiseFinishedProductList(String WorkCenterID, String ProductionOrderHeaderID)
        {
            //Modified By Swamy
            List<THHTWSData.KeyValueStruct> sources = new List<THHTWSData.KeyValueStruct>();
            IDataReader rsGetData = DB.GetRS("select SUM(Quantity) AS ConsumedQty,MMT.MCode from MFG_GoodsMovementDetails GMD JOIN MMT_MaterialMaster MMT ON MMT.MaterialMasterID=GMD.MaterialMasterID where ProductionOrderHeaderID=" + ProductionOrderHeaderID + " AND WorkCenterID=" + WorkCenterID + " AND GoodsMovementTypeID=2 AND IsLocallyProduced=0 GROUP BY MMT.MCode");
            sources.Clear();
            while (rsGetData.Read())
            {
                sources.Add(new THHTWSData.KeyValueStruct(DB.RSField(rsGetData, "MCode").ToString(), DB.RSFieldDecimal(rsGetData, "ConsumedQty").ToString()));

            }
            return sources;

        }


        [WebMethod]
        public DataTable GetWorkCenterWiseRejectedMatListForRework(String WorkCenterID, String ProductionOrderHeaderID)
        {

            DataSet ds = null;

            try
            {
                ds = DB.GetDS("EXEC [dbo].[sp_MFG_GetWorkCenterWiseRejectedMatList] @productionOrderHeaderID=" + ProductionOrderHeaderID + ",@WorkCenterID=" + WorkCenterID, false);
            }
            catch (Exception ex)
            {

            }

            return ds.Tables[0];
        }


        [WebMethod]
        public DataTable GetRawMaterialAssignedQty(String ProductionOrderHeaderID, String WorkCenterID, String MCode)
        {
            DataSet ds = null;

            try
            {

                ds = DB.GetDS("EXEC  [dbo].[sp_MFG_GetRawMaterialAssignedQty]  @ProductionOrderHeaderID=" + ProductionOrderHeaderID + ",@WorkCenterID=" + WorkCenterID + ",@MCode=" + DB.SQuote(MCode), false);

                return ds.Tables[0];

            }
            catch (Exception ex)
            {

                return null;
            }
        }


        [WebMethod]
        public DataTable GetWorkCenterFinishedGoodsINList(String ProductionOrderHeaderID, String WorkCenterID)
        {

            DataSet ds = null;

            try
            {
                ds = DB.GetDS("sp_MFG_GetSerialNumberForMaterial @ProductionOrderHeaderID=" + ProductionOrderHeaderID + ",@WorkCenterID=" + WorkCenterID, false);
                return ds.Tables[0];



            }
            catch (Exception ex)
            {


                return null;
            }

        }


        [WebMethod]
        public DataTable GetScrapCodeList()
        {

            DataSet ds = null;

            try
            {

                ds = DB.GetDS("select ScrapCode from MFG_ScrapCode where IsActive=1 AND IsDeleted=0", false);

                return ds.Tables[0];

            }
            catch (Exception ex)
            {

                return null;
            }

        }
        [WebMethod]
        public DataTable GetOperations(String ProductionOrderHeaderID, int WorkCenterID)
        {
            DataSet ds = null;
            try
            {

                ds = DB.GetDS("EXEC [dbo].[sp_MFG_GetOperationNumbers]@ProductionOrderHeaderID=" + ProductionOrderHeaderID + ",@WorkCenterID=" + WorkCenterID, false);

                return ds.Tables[0];

            }
            catch (Exception ex)
            {


                return null;
            }


        }

        [WebMethod]
        public DataTable GetUnconfirmedProductsList(String ProductionOrderHeaderID, String WorkCenterID, int Nonconfirmitytype)
        {

            DataSet ds = null;
            try
            {

                ds = DB.GetDS("EXEC [sp_MFG_GetWorkCenterUnconfirmedGoodsINList]  @ProductionOrderHeaderID=" + ProductionOrderHeaderID + ",@WorkCenterID=" + WorkCenterID + ",@NonConfirmityTypeID=" + Nonconfirmitytype, false);

                return ds.Tables[0];

            }
            catch (Exception ex)
            {


                return null;
            }

        }

        [WebMethod]
        public Decimal GetWorkCenterProducedQty(String ProductionOrderHeaderID, String WorkCenterID)
        {

            DataSet ds = null;
            Decimal qty = 0;
            try
            {
                ds = DB.GetDS("EXEC dbo.sp_MFG_GetProducedQuantityForWorkcenter @WorkcenterID=" + WorkCenterID + ",@ProductionOrderHeaderID=" + ProductionOrderHeaderID, false);

                IDataReader dr = ds.Tables[0].CreateDataReader();

                dr.Read();

                qty = DB.RSFieldDecimal(dr, "Quantity");

                dr.Close();
            }
            catch (Exception ex)
            {

            }

            return qty;
        }
        //Swamyp
        [WebMethod]
        public int GetWorkCentreFinishedQty(String ProductionOrderHeaderID, String WorkCenterID)
        {
            try
            {
                Decimal Qty = DB.GetSqlNDecimal("select sum(Quantity) as N from MFG_GoodsMovementDetails where ProductionOrderHeaderID=" + ProductionOrderHeaderID + "and WorkCenterID=" + WorkCenterID + " and GoodsMovementTypeID=1 and IsLocallyProduced=1 and NonConfirmityCodeID is null");
                int Quantity = Int32.Parse(Qty.ToString());
                return Quantity;
            }
            catch (Exception ex)
            {

            }

            return 0;
        }

        [WebMethod]
        public DataTable GetWorkCenterWiseHeaderData(String ProductionOrderHeaderID, int WorkCenterID)
        {

            DataSet ds = null;

            try
            {

                ds = DB.GetDS("EXEC [sp_MFG_GetWorkCenterwiseHeaderData]@ProductionOrderHeaderId=" + ProductionOrderHeaderID + ",@WorkCenterID=" + WorkCenterID, false);

                return ds.Tables[0];

            }
            catch (Exception ex)
            {
                return null;

            }


        }

        [WebMethod]
        public int GetLastSerialNumberForMaterial(String ProductionOrderHeaderID, String WorkCenterID, String MMID)
        {

            int LastSerialNo = 0;
            try
            {

                LastSerialNo = DB.GetSqlN("SELECT TOP 1 SerialNumber AS N FROM MFG_SerialNumberCapture WHERE ProductionOrderHeaderID=" + ProductionOrderHeaderID + " AND WorkCenterID=" + WorkCenterID + " AND MaterialMasterID=" + MMID + " ORDER BY SerialNumber DESC");
            }
            catch (Exception ex)
            {

            }

            return LastSerialNo;
        }


        [WebMethod]
        public Decimal GetWorkCenterWiseProducedQty(String ProductionOrderHeaderID, String WorkCenterID)
        {


            DataSet ds = null;
            Decimal qty = 0;
            try
            {
                ds = DB.GetDS("EXEC [sp_MFG_GetWorkCenterWiseProducedQty]  @WorkCenterGroupID=0,@WorkcenterID=" + WorkCenterID + ",@ProductionOrderHeaderID=" + ProductionOrderHeaderID, false);

                IDataReader dr = ds.Tables[0].CreateDataReader();

                dr.Read();

                qty = DB.RSFieldDecimal(dr, "ProducedQty");

                dr.Close();
            }
            catch (Exception ex)
            {

            }

            return qty;

        }

        [WebMethod]
        public DataTable GetSerialNumbersForWorkCenter(int WorkCenterID, int ProductionOrderID)
        {
            DataSet ds = null;
            try
            {
                ds = DB.GetDS("EXEC dbo.sp_MFG_GetSerialNumbersForWorkCenter @ProductionOrderHeaderID=" + ProductionOrderID + ",@WorkCenterID=" + WorkCenterID, false);
                return ds.Tables[0];
            }
            catch
            {
                return null;
            }

        }


        [WebMethod]
        public bool UpdatePositiveRecall(int WorkCenterID, string ProductionOrderID, string remarks, String EmployeeCode, int vstatus, int vRoutingDetailsID)
        {
            bool status = false;
            try
            {
                //Getting UserID Based On EmployeeCode
                int UserID = DB.GetSqlN("select UserID as N from GEN_User where EmployeeCode=" + DB.SQuote(EmployeeCode));
                //Check For PositiveRecall Insert Or Update
                int vPostitiveRecallID = 0;

                if (vstatus == 1)
                {
                    vPostitiveRecallID = DB.GetSqlN("select PositiveRecallID as N from MFG_PositiveRecall where IsActive=1 and RoutingDetailsActivityID=" + vRoutingDetailsID + " and ProductionOrderHeaderID=" + ProductionOrderID + " and IsDeleted=0");
                }

                DB.ExecuteSQL("EXEC [dbo].[sp_MFG_UpsertPositiveRecall]  @PositiveRecallID=" + vPostitiveRecallID + ",@RoutingDetailsActivityID=" + vRoutingDetailsID + ",@ProductionOrderHeaderID=" + ProductionOrderID + ",@WorkCenterID=" + WorkCenterID + ",@Remarks=" + DB.SQuote(remarks) + ",@CreatedBy=" + UserID);

                return true;
            }
            catch
            {
                return status;
            }

        }


        [WebMethod]
        public string GetPositiveRecallDetails(int vRoutingDetailsActivityID, string ProductionOrderID)
        {
            /* DataSet ds = null;
             try
             {
                 string result= DB.GetSqlS("select Remarks as S from MFG_PositiveRecall where IsActive=1 and RoutingDetailsActivityID=3099 and ProductionOrderHeaderID=1 and IsDeleted=0");
                 return ds.Tables[0];
             }
             catch
             {
                 return null;
             }
             */
            return "";
        }


        [WebMethod]
        [XmlInclude(typeof(THHTWSData.KeyValueStruct))]
        public List<THHTWSData.KeyValueStruct> GetQCParameters(int RoutingDetailsActivityID, String vProductionOrderHeaderID)
        {
            List<THHTWSData.KeyValueStruct> sources = new List<THHTWSData.KeyValueStruct>();


            IDataReader rsGetData = DB.GetRS("EXEC [dbo].[sp_MFG_GetWorkCenterWiseQCParameters]@RoutingDetailsActivityID=" + RoutingDetailsActivityID + ",@ProductionOrderHeaderID=" + vProductionOrderHeaderID);
            sources.Clear();
            while (rsGetData.Read())
            {
                sources.Add(new THHTWSData.KeyValueStruct("ParameterName", DB.RSField(rsGetData, "ParameterName").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("VisualType", DB.RSFieldTinyInt(rsGetData, "VisualType").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("MeasureType", DB.RSFieldTinyInt(rsGetData, "MeasureType").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("RecordedType", DB.RSFieldTinyInt(rsGetData, "RecordedType").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("DataType", DB.RSField(rsGetData, "DataType").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("MINVALUE", DB.RSFieldInt(rsGetData, "MINVALUE").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("MAXVALUE", DB.RSFieldInt(rsGetData, "MAXVALUE").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("ID", DB.RSFieldInt(rsGetData, "RoutingDetailsActivity_QualityParameterID").ToString()));

                sources.Add(new THHTWSData.KeyValueStruct("OPCVisualValue", DB.RSFieldTinyInt(rsGetData, "OPCVisualValue").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("OPCMeasureValue", DB.RSFieldTinyInt(rsGetData, "OPCMeasureValue").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("OPCRecordedValue", DB.RSField(rsGetData, "OPCRecordedValue").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("JAPReferenceText", DB.RSField(rsGetData, "JAPReferenceText").ToString()));

                sources.Add(new THHTWSData.KeyValueStruct("OPPassed", DB.RSFieldTinyInt(rsGetData, "OPPassed").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("SUPPassed", DB.RSFieldTinyInt(rsGetData, "SUPPassed").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("QCPassed", DB.RSFieldTinyInt(rsGetData, "QCPassed").ToString()));

                sources.Add(new THHTWSData.KeyValueStruct("OPRemarks", DB.RSField(rsGetData, "OPRemarks").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("SUPRemarks", DB.RSField(rsGetData, "SUPRemarks").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("QCRemarks", DB.RSField(rsGetData, "QCRemarks").ToString()));

                sources.Add(new THHTWSData.KeyValueStruct("SUPCVisualValue", DB.RSFieldTinyInt(rsGetData, "SUPCVisualValue").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("SUPCMeasureValue", DB.RSFieldTinyInt(rsGetData, "SUPCMeasureValue").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("SUPCRecordedValue", DB.RSField(rsGetData, "SUPCRecordedValue").ToString()));


                sources.Add(new THHTWSData.KeyValueStruct("QCCVisualValue", DB.RSFieldTinyInt(rsGetData, "QCCVisualValue").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("QCCMeasureValue", DB.RSFieldTinyInt(rsGetData, "QCCMeasureValue").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("QCCRecordedValue", DB.RSField(rsGetData, "QCCRecordedValue").ToString()));

            }
            rsGetData.Close();

            int IsPositiveRecall = DB.GetSqlN("select PositiveRecallID as N from MFG_PositiveRecall where IsActive=1 and RoutingDetailsActivityID=" + RoutingDetailsActivityID + " and ProductionOrderHeaderID=" + vProductionOrderHeaderID);
            sources.Add(new THHTWSData.KeyValueStruct("IsPositiveRecallActive", IsPositiveRecall.ToString()));
            return sources;
        }

        [WebMethod]
        public String getSampleCode()
        {
            return "nn";
        }
        //Written by swamy on 03/03/2014
        [WebMethod]
        [XmlInclude(typeof(THHTWSData.KeyValueStruct))]
        public List<THHTWSData.KeyValueStruct> GetDropDownData(String SqlString, String DefaultValue)
        {
            List<THHTWSData.KeyValueStruct> sources = new List<THHTWSData.KeyValueStruct>();

            IDataReader rsGetData = DB.GetRS(SqlString);
            sources.Clear();

            sources.Add(new THHTWSData.KeyValueStruct("0", DefaultValue));
            while (rsGetData.Read())
            {
                sources.Add(new THHTWSData.KeyValueStruct(rsGetData[1].ToString(), rsGetData[0].ToString()));
            }
            rsGetData.Close();




            return sources;

        }
        [WebMethod]
        [XmlInclude(typeof(THHTWSData.KeyValueStruct))]
        public List<THHTWSData.KeyValueStruct> TestData(int poheaderID, int WorkCenterID)
        {
            List<THHTWSData.KeyValueStruct> sources = new List<THHTWSData.KeyValueStruct>();
            IDataReader rsGetData = DB.GetRS("select SUM(Quantity) AS ConsumedQty,MMT.MCode from MFG_GoodsMovementDetails GMD JOIN MMT_MaterialMaster MMT ON MMT.MaterialMasterID=GMD.MaterialMasterID where ProductionOrderHeaderID=" + poheaderID + " AND WorkCenterID=" + WorkCenterID + " AND GoodsMovementTypeID=2 AND IsLocallyProduced=0 GROUP BY MMT.MCode");
            sources.Clear();
            while (rsGetData.Read())
            {
                sources.Add(new THHTWSData.KeyValueStruct(DB.RSField(rsGetData, "MCode").ToString(), DB.RSFieldDecimal(rsGetData, "ConsumedQty").ToString()));

            }
            return sources;
        }



        [WebMethod]
        public string GetIORefNumber()
        {

            string IORefNumber = "";

            try
            {
                String NewIONumber = DB.GetSqlS("EXEC sp_SYS_GetSystemConfigValue @SysConfigKey=N'internalorder.aspx.cs.IO_Prefix',@TenantID=1");

                int length = Convert.ToInt32(DB.GetSqlS("EXEC sp_SYS_GetSystemConfigValue @SysConfigKey=N'internalorder.aspx.cs.IO_Length',@TenantID=1"));


                //string HeaderID = ViewState["HeaderID"].ToString();
                String OldIONumber = DB.GetSqlS("select top 1 IORefNo as S from MFG_InternalOrderHeader  order by IORefNo desc");

                NewIONumber += "" + (Convert.ToInt16(DateTime.Now.Year) % 100);           //add year code to ponumber

                int power = (Int32)Math.Pow((double)10, (double)(length - 1));          //getting minvalue of prifix length

                String newvalue = "";
                if (OldIONumber != "" && NewIONumber.Equals(OldIONumber.Substring(0, NewIONumber.Length)))        //if ponumber is existed and same year ponumber  enter
                {
                    String temp = OldIONumber.Substring(NewIONumber.Length, length);                            //getting number of last prifix
                    Int32 number = Convert.ToInt32(temp);
                    number++;


                    while (power > 1)                                                                           //add '0' to number at left side for get 
                    {
                        if (number / power > 0)
                        {
                            break;
                        }
                        newvalue += "0";
                        power /= 10;
                    }
                    newvalue += number;
                }
                else
                {                                                                                           //other wise generate first number 
                    for (int i = 0; i < length - 1; i++)
                        newvalue += "0";
                    newvalue += "1";
                }

                NewIONumber += newvalue;
                IORefNumber = NewIONumber;

                return IORefNumber;
            }
            catch (Exception ex)
            {
                return "";
            }

        }




        [WebMethod]
        public DataTable GetIOBOMDetails(string ProductionOrderID)
        {
            DataSet ds = null;
            try
            {
                int BOMHeaderID = DB.GetSqlN("select BOMHeaderID AS N from MFG_BOMHeader_Revision where IsActive=1 AND IsDeleted=0 AND BOMHeaderRevisionID IN  ( select BOMHeaderRevisionID from MFG_ProductionOrderHeader where IsActive=1 AND IsDeleted=0 AND ProductionOrderHeaderID=" + ProductionOrderID + " )");

                ds = DB.GetDS("EXEC [dbo].[sp_MFG_GetIOBoMDetailsList] @BOMHeaderID=" + BOMHeaderID, false);
                return ds.Tables[0];
            }
            catch
            {
                return null;
            }

        }

        [WebMethod]
        public int UpsertToolCapture(String ItemCode, String SerialNumber, int vRouteID, String EmployeeCode)
        {
            StringBuilder sbSqlString = new StringBuilder();
            sbSqlString.Append("declare @NewResult int");
            sbSqlString.Append(" EXEC  [dbo].[sp_MFG_UpsertRoutingDetailsActivity_TOOLS]");
            sbSqlString.Append("@RoutingDetailsActivityID=" + vRouteID);
            sbSqlString.Append(",@Mcode=" + DB.SQuote(ItemCode));
            sbSqlString.Append(",@EmpCode=" + DB.SQuote(EmployeeCode));
            sbSqlString.Append(",@SerialNumber=" + DB.SQuote(SerialNumber));
            sbSqlString.Append(",@Result=@NewResult OUTPUT ");
            sbSqlString.Append("SELECT @NewResult AS N");
            try
            {
                int result = DB.GetSqlN(sbSqlString.ToString());
                return result;
            }
            catch
            {
                return -555;
            }

        }

        [WebMethod]
        public bool UploadFile(String PathName, Byte[] FileControl, String WCFURL)
        {
            bool status = false;


            try
            {
                MemoryStream ms = new MemoryStream(FileControl);

                //var _Path = System.Web.HttpContext.Current.Server.MapPath(WCFURL);
                //var _Path =Uri. WCFURL;
                //Uri uri = new Uri(WCFURL);
                var _Path = System.Web.HttpContext.Current.Server.MapPath("~/" + PathName);
                //var _Path = uri.ToString();
                var _Directory = new DirectoryInfo(_Path.ToString());

                if (_Directory.Exists == false)
                {
                    _Directory.Create();
                }
                // instance a filestream pointing to the
                // storage folder, use the original file name
                // to name the resulting file
                FileStream fs = new FileStream
                    (_Path, FileMode.Create);

                // write the memory stream containing the original
                // file as a byte array to the filestream
                ms.WriteTo(fs);

                // clean up
                ms.Close();
                fs.Close();
                fs.Dispose();

                status = true;

            }
            catch (Exception ex)
            {
                return false;
            }


            return status;


        }
    }
}
