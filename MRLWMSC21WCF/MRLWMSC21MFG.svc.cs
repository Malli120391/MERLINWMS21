using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using MRLWMSC21Common;
using System.ServiceModel.Activation;
using System.IO;
using System.Collections;
namespace MRLWMSC21WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service2" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service2.svc or Service2.svc.cs at the Solution Explorer and start debugging.
    
    public class FalconMFG : IFalconMFG
    {
        
        public string DoWork()
        {
            return "Swamy";
        }
        public FalconHHTSData.LoginUserData GetUserDetails(String UserName, String Password, String LoginIPAddress)
        {
            FalconHHTSData.LoginUserData thisUserDetails = new FalconHHTSData.LoginUserData();
            if (thisUserDetails.Login(UserName, Password, LoginIPAddress))
            {
                Audit.LogInAudit(thisUserDetails.UserID.ToString(), LoginIPAddress, thisUserDetails.TenantID);
            }

            return thisUserDetails;
        }
        public List<THHTWSData.KeyValueStruct> GetRoleID(String empcode,int WorkCenterID)
        {

           
            List<THHTWSData.KeyValueStruct> sources = new List<THHTWSData.KeyValueStruct>();
            IDataReader userWorkcenterCheck = DB.GetRS("SELECT * FROM MFG_WorkCenter_GEN_User WCGU join GEN_USER GU ON GU.UserID=WCGU.UserID WHERE EmployeeCode="+DB.SQuote(empcode)+" AND WorkCenterID="+WorkCenterID+" AND WCGU.IsDeleted=0 AND WCGU.IsActive=1");
            if (!userWorkcenterCheck.Read())
            {
                sources.Add(new THHTWSData.KeyValueStruct("NotMapped", "NotMapped"));
                return sources;
            }

            IDataReader objUserDetails = DB.GetRS("[dbo].[sp_MFG_GetRolesBasedOnEmployeeID]@EmployeeCode="+DB.SQuote(empcode));
            
            while(objUserDetails.Read())
            {
                sources.Add(new THHTWSData.KeyValueStruct("UserRoleID", DB.RSField(objUserDetails, "UserRoleID").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("FirstName", DB.RSField(objUserDetails, "FirstName").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("UserID", DB.RSFieldInt(objUserDetails, "UserID").ToString()));
            }
            if (sources.Count == 0)
            {
                sources.Add(new THHTWSData.KeyValueStruct("NoData", "NoData"));
            }
            objUserDetails.Close();
            return sources;
            
        }
        public int LogOutUser(int UserID)
        {
            try
            {
                DB.ExecuteSQL("update sec_loginstatus set isloggedin=0 where userid=" + UserID);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
           
        }
        public List<THHTWSData.KeyValueStruct> CheckUserLogin(string UserID, string Password, string LoginIPAddress,string WorkcenterName)
        {
            //string vPassword=Encrypt.EncryptData(CommonLogic.Application("EncryptKey"), Password);

            List<THHTWSData.KeyValueStruct> sources = new List<THHTWSData.KeyValueStruct>();

            IDataReader objUserDetails = DB.GetRS("EXEC [dbo].[sp_MFG_GetUserDetailsIfLogin] @UserID='" + UserID + "',@Password="+DB.SQuote(Password) +",@WorkCenterName=" + DB.SQuote(WorkcenterName));
            string userID = "0";
            int TenantID = 0;
            sources.Clear();
            while (objUserDetails.Read())
            {
                /*string query = "select isloggedin,ipaddress from sec_loginstatus where userid=" + DB.RSFieldInt(objUserDetails, "UserID");
                IDataReader logStatus = DB.GetRS(query);
                if (logStatus.Read())
                {
                    int IsLogggedIN = DB.RSFieldTinyInt(logStatus, "isloggedin");
                    if (IsLogggedIN == 0)
                    {
                        DB.ExecuteSQL("update sec_loginstatus set isloggedin=1,ipaddress=" + DB.SQuote(WorkcenterName) + " where userid=" + DB.RSFieldInt(objUserDetails, "UserID"));
                    }
                    else
                    {
                        if (DB.RSField(logStatus, "ipaddress").ToString().Trim() !=WorkcenterName)
                        {
                            sources.Add(new THHTWSData.KeyValueStruct("IPAddress", DB.RSField(logStatus, "ipaddress").ToString()));
                            logStatus.Close();
                            return sources;
                        }
                    }
                    logStatus.Close();
                }
                else
                {
                    DB.ExecuteSQL("insert into sec_loginstatus(UserID,IPAddress,ISLoggedIN) values(" + DB.RSFieldInt(objUserDetails, "UserID") + "," + DB.SQuote(LoginIPAddress.Substring(LoginIPAddress.IndexOf('@')+1)) + ",1)");
                }*/
                sources.Add(new THHTWSData.KeyValueStruct("UserID", DB.RSFieldInt(objUserDetails, "UserID").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("TenantID", DB.RSFieldInt(objUserDetails, "TenantID").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("Email", DB.RSField(objUserDetails, "Email").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("FirstName", DB.RSField(objUserDetails, "FirstName").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("LastName", DB.RSField(objUserDetails, "LastName").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("WorkCenterID", DB.RSFieldInt(objUserDetails, "WorkCenterID").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("WorkCenter", DB.RSField(objUserDetails, "WorkCenter").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("EmployeeCode", DB.RSField(objUserDetails, "EmployeeCode").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("WorkCenterGroupID", DB.RSFieldInt(objUserDetails, "WorkCenterGroupID").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("DelayTime,ISFlushRequired", DB.RSField(objUserDetails, "DelayTime").ToString()+","+DB.RSField(objUserDetails, "ISFlushRequired").ToString()));
                userID = DB.RSFieldInt(objUserDetails, "UserID").ToString();
                TenantID = DB.RSFieldInt(objUserDetails, "TenantID");
               

            }
            objUserDetails.Close();
            if (sources.Count==10)
            {
                Audit.LogInAudit(userID, LoginIPAddress,TenantID);
            }
            return sources;

        }

       
        public UserDataTable GetProductionOrderList(int WorkCenterID)
        {
            UserDataTable userTable = new UserDataTable();
            //DataSet ds = DB.GetDS("[sp_MFG_GetProductionOrderHeaderList] @PRORefNo='', @WorkCenterID=" + WorkCenterID, false);
            DataSet ds = DB.GetDS("[sp_MFG_GetWorkCenterWiseProductionOrderList]@WorkCenterID=" + WorkCenterID, false);
            DataTable dt= ds.Tables[0];
            userTable.Table = dt;

            ds.Dispose();
            dt.Dispose();

            return userTable;
          
        }
        public UserDataTable GetOperationstatus(String ProductionOrderHeaderID)
        {
            UserDataTable userTable = new UserDataTable();
            DataSet ds = null;
            try
            {

                ds = DB.GetDS("exec [dbo].[sp_MFG_GetOperationStatus]@ProductionOrderHeaderID="+ProductionOrderHeaderID, false);

                DataTable dt = ds.Tables[0];
                userTable.Table = dt;
                ds.Dispose();
                dt.Dispose();
                return userTable;


            }
            catch (Exception ex)
            {
                return null;
            }
        }
      
        public UserDataTable GetOperations(String ProductionOrderHeaderID, int WorkCenterID)
        {
        
            UserDataTable userTable = new UserDataTable();
            DataSet ds = null;
            try
            {

                ds = DB.GetDS("EXEC [dbo].[sp_MFG_GetOperationNumbers]@ProductionOrderHeaderID=" + ProductionOrderHeaderID + ",@WorkCenterID=" + WorkCenterID, false);

                DataTable dt = ds.Tables[0];
                userTable.Table = dt;
                ds.Dispose();
                dt.Dispose();
                return userTable;


            }
            catch (Exception ex)
            {


                return null;
            }


        }

        public UserDataTable GetWorkCenterWiseHeaderData(String ProductionOrderHeaderID, int WorkCenterID)
        {

            DataSet ds = null;
            UserDataTable userTable = new UserDataTable();

            try
            {

                ds = DB.GetDS("EXEC [sp_MFG_GetWorkCenterwiseHeaderData]@ProductionOrderHeaderId=" + ProductionOrderHeaderID + ",@WorkCenterID=" + WorkCenterID, false);

                DataTable dt = ds.Tables[0];
                userTable.Table = dt;

                ds.Dispose();
                dt.Dispose();

                return userTable;

            }
            catch (Exception ex)
            {
              
                return null;

            }


        }
        public int GetUserRoleID(string empcode)
        {

            int roldeid = DB.GetSqlN("select GUR.UserRoleID as N from GEN_User GU JOIN GEN_User_UserRole GUU ON GUU.UserID=GU.UserID JOIN GEN_UserRole GUR ON GUR.UserRoleID=GUU.UserRoleID WHERE GU.EmployeeCode=" + DB.SQuote(empcode) + " AND (GUR.UserRoleID=22 OR GUR.UserRoleID=23 OR GUR.UserRoleID=24 OR GUR.UserRoleID=25)");
            return roldeid;
        }

        public bool UpsertRoutingDetailsActivityPassorFail(String RouteActivityID, String EmpCode, int PassOrFail, String vProductionOrderID, String vRemarks, string vCheckListResultData, int vUserRoleID,string vReasonForDelay,int DelayTime,string vDispotionText,int vIsReworkRequired,
            string vActualCondition, string vShouldbeCondition, string ImmediateActionTargetDate, string CorrectiveActionReferrence, string RCAteamFormation, string TargetDateForCAPA, string CommunicationDate, string NonCorrectiveActionRemarks,int CorrectiveActionRequired,int vDispositionID,int IsReworkProcedureAmmendment,string ParentNCReference)
        {
            //If CheckPoints Are Configured to This Activity Save These CheckPoins Data Into MFG_QualityParaMeterCapture
            if (vCheckListResultData != "0")
            {


                StringBuilder sbSqlStringforCheckList = new StringBuilder();
                sbSqlStringforCheckList.Append("EXEC sp_MFG_UpsertQualityParametersCapture @QualityParametersCaptureData=" + DB.SQuote(vCheckListResultData));
                sbSqlStringforCheckList.Append(",@RoutingDetailsActivityID=" + RouteActivityID);
                sbSqlStringforCheckList.Append(",@UserRoleID=" + vUserRoleID);
                sbSqlStringforCheckList.Append(",@ProductionOrderHearedID=" + vProductionOrderID);
                DB.ExecuteSQL(sbSqlStringforCheckList.ToString());
            }

            //Getting RoutingDetailsActivityCaptureID Using The above Details
            StringBuilder sbSqlString = new StringBuilder();
            sbSqlString.Append("declare @userid int;set @userid=(select UserID from GEN_User where EmployeeCode=" + DB.SQuote(EmpCode) + ");");
            sbSqlString.Append("declare @RouteCaptureID int;");
            sbSqlString.Append("set @RouteCaptureID=(select RoutingDetailsActivityCaptureID from  MFG_RoutingDetailsActivityCapture where UserRoleID=" + vUserRoleID + " and ProductionOrderHeaderID=" + vProductionOrderID + " and RoutingDetailsActivityID=" + RouteActivityID + " and IsFailed IS NULL and IsActive=1 and IsDeleted=0);");
            sbSqlString.Append("select @RouteCaptureID as N");

            int vRoutingDetailsActivityCaptureID = DB.GetSqlN(sbSqlString.ToString());
            StringBuilder sbSqlStringFORUpsert = new StringBuilder();
            //If This Activity Is Passed Just Update The Remarks And Result Is Captured
            if (PassOrFail==1)
            {
                sbSqlStringFORUpsert.Append("UPDATE MFG_RoutingDetailsActivityCapture SET IsFailed=0,Remarks=" + DB.SQuote(vRemarks) + ",ReasonForDelay=" + (vReasonForDelay.Length != 0 ? DB.SQuote(vReasonForDelay) : "NULL") + ",DelayTime="+DelayTime+" WHERE RoutingDetailsActivityCaptureID=" + vRoutingDetailsActivityCaptureID);
            }//If This Activity Is Failed Update The Remarks And Capture Result as Failed And Update IsActive=0 For That Activity
            else
            {
                //If This Result Is Failed By Operator
                if (vUserRoleID ==-1)
                {
                    int count = DB.GetSqlN("select count(RoutingDetailsActivityID) as N from MFG_RoutingDetailsActivity_Operation where RoutingDetailsActivityID=" + RouteActivityID);
                    if (count ==0)
                    {
                        sbSqlStringFORUpsert.Append("UPDATE MFG_RoutingDetailsActivityCapture SET IsFailed=1,EndTime=GETDATE(),Remarks=" + DB.SQuote(vRemarks) + ",ReasonForDelay=NULL,DelayTime=" + DelayTime +" WHERE RoutingDetailsActivityCaptureID=" + vRoutingDetailsActivityCaptureID);
                        sbSqlStringFORUpsert.Append(" UPDATE MFG_RoutingDetailsActivityCapture SET IsActive=0 WHERE RoutingDetailsActivityID=" + RouteActivityID + " AND ProductionOrderHeaderID=" + vProductionOrderID);
                    }
                    else
                    {
                        sbSqlStringFORUpsert.Append("UPDATE MFG_RoutingDetailsActivityCapture SET IsFailed=1,EndTime=GETDATE(),Remarks=" + DB.SQuote(vRemarks) + ",ReasonForDelay=NULL,DelayTime=" + DelayTime +" WHERE RoutingDetailsActivityCaptureID=" + vRoutingDetailsActivityCaptureID);
                    }
                }//If This Result Is Failed By Supervisor/Qc/CQC
                else
                {
                    //int NCReferenceNumber = DB.GetSqlN("select max(isnull(NCReferenceNumber,51400000))+1 as N from MFG_RoutingDetailsActivityCapture");
                    int NCReferenceNumber = DB.GetSqlN("select isnull(max(NCReferenceNumber),'5'+SUBSTRING(convert(nvarchar,YEAR(getdate())),3,2)+'00000')+1 "+
                                                       "AS N from MFG_RoutingDetailsActivityCapture where "+
                                                       " SUBSTRING(convert(nvarchar,NCReferenceNumber),2,2)=SUBSTRING(convert(nvarchar,YEAR(getdate())),3,2)");

                    int USerID=DB.GetSqlN("SELECT UserID as N from GEN_User where EmployeeCode=" + DB.SQuote(EmpCode));
                    sbSqlStringFORUpsert.Append("UPDATE MFG_RoutingDetailsActivityCapture SET IsFailed=1,EndTime=GETDATE(),Remarks=" 
                        + DB.SQuote(vRemarks) + ",ReasonForDelay=" + (vReasonForDelay.Length != 0 ? DB.SQuote(vReasonForDelay) : "NULL") 
                        + ",DelayTime=" + DelayTime + ",DispositionRemarks=" + DB.SQuote(vDispotionText) + ",ActualCondition=" 
                        + DB.SQuote(vActualCondition) + ",ShouldBeCondition=" + DB.SQuote(vShouldbeCondition)
                        + ",ImmediateActionTargetDate=" + (ImmediateActionTargetDate.Length != 0 ? DB.SQuote(ImmediateActionTargetDate) : "NULL")
                        + ",CorrectiveActionReference=" + (CorrectiveActionReferrence.Length != 0 ? DB.SQuote(CorrectiveActionReferrence) : "NULL")
                        + ",RCATeamFormationwithTeamLead=" + (RCAteamFormation.Length != 0 ? DB.SQuote(RCAteamFormation) : "NULL")
                        + ",TargetDateforCAPA=" + (TargetDateForCAPA.Length != 0 ? DB.SQuote(TargetDateForCAPA) : "NULL")
                        + ",Communicationdate=" + (CommunicationDate.Length != 0 ? DB.SQuote(CommunicationDate) : "NULL")
                        + ",RemarksForNoCorrectiveAction=" + (NonCorrectiveActionRemarks.Length != 0 ? DB.SQuote(NonCorrectiveActionRemarks) : "NULL")
                        + ",CorrectiveActionRequired="+CorrectiveActionRequired
                        + ",DispositionID=" + vDispositionID
                        + ",NCReferenceNumber=" + NCReferenceNumber
                        + ",IsReworkProcedureAmmendment=" + IsReworkProcedureAmmendment
                        + ",ParentNCReferenceNumber=" + ParentNCReference
                        + " WHERE RoutingDetailsActivityCaptureID=" + vRoutingDetailsActivityCaptureID);
                    
                    sbSqlStringFORUpsert.Append(" UPDATE MFG_RoutingDetailsActivityCapture SET IsActive=0 WHERE RoutingDetailsActivityID=" + RouteActivityID + " AND ProductionOrderHeaderID="+vProductionOrderID);
                    if (vIsReworkRequired == 1)
                    {
                        sbSqlStringFORUpsert.Append("INSERT INTO MFG_ReworkProcedureCapture(RoutingDetailsActivityID,NCReferenceNO,ReworkProcedureText, " +
                        "SerilaNo,SupervisorRemarks,SupervisorID,ProductionOrderHeaderID) " +
                        "values(" + RouteActivityID + "," + NCReferenceNumber + ",'',1,''," + USerID + "," + vProductionOrderID + ")");
                    }
                    
                }

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
        public string UpsertGoodsMovementDetails(string EmployeeCode, string ProductionOrderHeaderID, string RoutingDetailsActivityID, string MCode, decimal Qty, string WorkCenterID, int GoodsMovementTypeID, int IsDamaged, int IsLocallyProduced, string Remarks, string ScrapCode, string reworkroutingID, string SerialNumber, string BatchNumber,int IsPositiveRecall,string DeliveryDocNumber)
        {
            try
            {
                int UserID = DB.GetSqlN("select UserID as N from GEN_User where EmployeeCode=" + DB.SQuote(EmployeeCode));

                int vMMID = CommonLogic.GetMMID(MCode);
                if (vMMID == 0)
                {
                    return "Invalid part number";
                }
                //DeliveryDocument Number is 0 when this request raised from FabricationWorkStation
                int OBDID=0;
                if (DeliveryDocNumber != "0")
                {
                    OBDID = CommonLogic.GetOutboundID(DeliveryDocNumber);
                    if (OBDID == 0)
                    {
                        return "Invalid del. doc. no.";
                    }
                  
                }

                /*if (RoutingDetailsActivityCaptureID != "0")
                {
                    int RCID = DB.GetSqlN("select RoutingDetailsActivityCaptureID AS N from MFG_RoutingDetailsActivityCapture where RoutingDetailsActivityID=" + RoutingDetailsActivityCaptureID + "AND IsActive=1 AND IsDeleted=0");
                    RoutingDetailsActivityCaptureID = RCID.ToString();
                }*/

                StringBuilder sbSqlString = new StringBuilder();
                sbSqlString.Append("declare @NewResult int ");
                sbSqlString.Append(" EXEC  [dbo].[sp_MFG_UpsertGoodsMovementDetails] @GoodsMovementDetailsID=0");
                sbSqlString.Append(" ,@ProductionOrderHeaderID=" + ProductionOrderHeaderID);
                sbSqlString.Append(" ,@RoutingDetailsActivityID=" + (RoutingDetailsActivityID != "0" ? RoutingDetailsActivityID : "NULL"));
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
                sbSqlString.Append(" ,@IsPositiveRecall=" + IsPositiveRecall);
                sbSqlString.Append(" ,@OutboundID="+OBDID);
                sbSqlString.Append(" ,@Result=@NewResult OUTPUT ");
                sbSqlString.Append(" SELECT @NewResult AS N");

                try
                {
                    string ResponceResult = String.Empty;
                    int result = DB.GetSqlN(sbSqlString.ToString());
                    if (result == 100)
                    {
                        ResponceResult = "This material is not configured in BOM";
                    }
                    else if (result == 200)
                    {
                        ResponceResult = "Received qty. is greater than BOM qty.";
                    }
                    else if (result == 300)
                    {
                        ResponceResult = "Material cannot be received at this WS, as is was not processed at the Kitting/Panstock WS";
                    }
                    else if (result == 400)
                    {
                        ResponceResult = "Receive quantity is greater than the transferred quantity from kitting station";
                    }
                    else if (result == 1000)
                    {
                        ResponceResult = "Material cannot be received, as PGI is not done";
                    }
                    else if (result == 2000)
                    {
                        ResponceResult = "Material cannot be received, as the entered data is not issued from store";
                    }
                    else if (result == 3000)
                    {
                        ResponceResult = "Material cannot be received, as qty. is greater than issued qty.";
                    }
                    else if (result == 4000)
                    {
                        ResponceResult = "Material cannot be consumed, as qty. is not availabe with these combination";
                    }
                    else if (result == 1)
                    {
                        ResponceResult = "1";
                    }
                    return ResponceResult;
                }
                catch
                {
                    return "Failed, please contact admin";
                }
            }
            catch (Exception ex)
            {

                return "Failed, please contact admin";
            }

        }
        public UserDataTable GetMaterialReceivedList(String ProductionOrderHeaderID, String WorkCenterID)
        {

            DataSet ds = null;
            UserDataTable userTable = new UserDataTable();

            try
            {

                ds = DB.GetDS("EXEC [sp_MFG_GetMaterialReceivedList]  @ProductionOrderHeaderID=" + ProductionOrderHeaderID + ",@WorkCenterID=" + WorkCenterID, false);

                DataTable dt = ds.Tables[0];
                userTable.Table = dt;
                return userTable;

            }
            catch (Exception ex)
            {

                return null;
            }

        }
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
                sources.Add(new THHTWSData.KeyValueStruct("MINVALUE", DB.RSFieldDecimal(rsGetData, "MINVALUE").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("MAXVALUE", DB.RSFieldDecimal(rsGetData, "MAXVALUE").ToString()));
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
              
                sources.Add(new THHTWSData.KeyValueStruct("SeparatorCount", DB.RSFieldInt(rsGetData, "SeparatorCount").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("ReportType", DB.RSFieldInt(rsGetData, "ReportType").ToString()));

                sources.Add(new THHTWSData.KeyValueStruct("OperatorResult", DB.RSFieldTinyInt(rsGetData, "OperatorResult").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("OperatorRemarks", DB.RSField(rsGetData, "OperatorRemarks").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("SupervisorResult", DB.RSFieldTinyInt(rsGetData, "SupervisorResult").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("SupervisorRemarks", DB.RSField(rsGetData, "SupervisorRemarks").ToString()));

                sources.Add(new THHTWSData.KeyValueStruct("OperatorException", DB.RSFieldTinyInt(rsGetData, "OperatorException").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("SupException", DB.RSFieldTinyInt(rsGetData, "SupException").ToString()));
                sources.Add(new THHTWSData.KeyValueStruct("QCException", DB.RSFieldTinyInt(rsGetData, "QCException").ToString()));



            }
            rsGetData.Close();

            int IsPositiveRecall = DB.GetSqlN("select PositiveRecallID as N from MFG_PositiveRecall where IsActive=1 and RoutingDetailsActivityID=" + RoutingDetailsActivityID + " and ProductionOrderHeaderID=" + vProductionOrderHeaderID);
            sources.Add(new THHTWSData.KeyValueStruct("IsPositiveRecallActive", IsPositiveRecall.ToString()));
            return sources;
        }
        public UserDataTable GetActivityLevelComponents(int RoutingDetailsActivityID, int ProductionOrderHeaderID,int WorkCenterID)
        {
            UserDataTable userTable = new UserDataTable();

            DataSet ds = DB.GetDS("[dbo].[sp_MFG_GetActivityWiseConsumedMaterial] @ProductionOrderHeaderID=" + ProductionOrderHeaderID + ",@ROutingDetailsActivityID=" + RoutingDetailsActivityID + ",@WorkCenterID=" + WorkCenterID, false);

            DataTable dt = ds.Tables[0];
            userTable.Table = dt;
            ds.Dispose();
            dt.Dispose();
            return userTable;
            //return ds.Tables[0];

        }
        public UserDataTable GetOperationActivities(int UserID, String OperationNumber, int ProductionOrderHeaderID,int WorkCenterID)
        {

            UserDataTable userTable = new UserDataTable();
            DataSet ds = DB.GetDS("[dbo].[sp_MFG_GetActivities] @UserID=" + UserID + ",@OperationNumber=" + OperationNumber + ",@ProductionOrderHeaderID=" + ProductionOrderHeaderID + ",@WorkcenterID=" + WorkCenterID, false);

            DataTable dt = ds.Tables[0];
            userTable.Table = dt;
            ds.Dispose();
            dt.Dispose();
            return userTable;

        }
        public TableData GetOperationsData(String ProductionOrderHeaderID, int WorkCenterID)
        {
            TableData tables = new TableData();
            DataSet ds = null;
            try
            {
                ds = DB.GetDS("EXEC [dbo].[sp_MFG_GetOperationNumbers]@ProductionOrderHeaderID=" + ProductionOrderHeaderID + ",@WorkCenterID=" + WorkCenterID, false);
                DataTable dt = ds.Tables[0];
                tables.OperationsData = dt;
              
                int RoutingdetailsID = int.Parse(dt.Rows[0][3].ToString());

                ds = DB.GetDS("[dbo].[sp_MFG_GetActivities] @UserID=0,@OperationNumber=" + RoutingdetailsID + ",@ProductionOrderHeaderID=" + ProductionOrderHeaderID + ",@WorkcenterID=" + WorkCenterID, false);
                dt = ds.Tables[0];
                tables.ActivitiesData = dt;
                return tables;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public string UpsertRoutingDetailsActivityCapture(int RoutingdetailsActivityID, String ProductionOrderHeaderID, String UserCode, int StartTime, string Remarks,int RoleID)
        {
            int UserID = DB.GetSqlN("select UserID as N from GEN_User where EmployeeCode=" + DB.SQuote(UserCode));
            StringBuilder sbSqlString = new StringBuilder();
            sbSqlString.Append("declare @NewResult int");
            sbSqlString.Append(" EXEC  [dbo].[sp_MFG_UpsertRoutingDetailsActivityCapture]");
            sbSqlString.Append("@RoutingDetailsActivityID=" + RoutingdetailsActivityID);
            sbSqlString.Append(",@ProductionOrderHeaderID=" + ProductionOrderHeaderID);
            sbSqlString.Append(",@UserID=" + UserID);
            sbSqlString.Append(",@StartTime=" + (StartTime != 0 ? StartTime : 0));
            sbSqlString.Append(",@UserRoleID=" + RoleID);
            sbSqlString.Append(",@Remarks=" + DB.SQuote(Remarks));
            sbSqlString.Append(",@Result=@NewResult OUTPUT ");
            sbSqlString.Append("SELECT @NewResult AS N");
            try
            {
                int result = DB.GetSqlN(sbSqlString.ToString());
                if (result == -1)
                {
                    return "Another instance of SFT is already started for this activity";
                }
                else
                {
                    return result.ToString();
                }
              
            }
            catch
            {
                return "Failed, please contact admin";
            }
        }

        public UserDataTable GetOperatorActivityResuls(int vRoutingDetailsActivityID,int ProductionOrderID)
        {
            UserDataTable userTable = new UserDataTable();
            DataSet ds = DB.GetDS("	exec [dbo].[sp_MFG_GetActivityResults] @RoutingDetailsActivityID=" + vRoutingDetailsActivityID + ",@ProductionOrderHeaderID="+ProductionOrderID, false);
            DataTable dt = ds.Tables[0];
            userTable.Table = dt;
            ds.Dispose();
            dt.Dispose();
            return userTable;
        }
        public int IsPassOrFailCaptured(int vRouteID, String EmpCode, String vProductionOrderID,int roleID)
        {
            StringBuilder sbSqlString = new StringBuilder();
            sbSqlString.Append("declare @userid int;set @userid=(select UserID from GEN_User where EmployeeCode=" + DB.SQuote(EmpCode) + ");");
            sbSqlString.Append("SELECT ISNULL(IsFailed,2) AS TI FROM MFG_RoutingDetailsActivityCapture WHERE RoutingDetailsActivityID=" + vRouteID + " AND UserID= @userid AND ProductionOrderHeaderID=" + vProductionOrderID + " AND ISACTIVE=1 AND IsDeleted=0 AND UserRoleID=" + roleID);
            int result = DB.GetSqlTinyInt(sbSqlString.ToString());
            return result;
        }
        public int UpsertToolCapture(String ItemCode, String SerialNumber, int vRouteID, String EmployeeCode,String productionOrderId)
        {
            StringBuilder sbSqlString = new StringBuilder();
            sbSqlString.Append("declare @NewResult int");
            sbSqlString.Append(" EXEC  [dbo].[sp_MFG_UpsertRoutingDetailsActivity_TOOLS]");
            sbSqlString.Append("@RoutingDetailsActivityID=" + vRouteID);
            sbSqlString.Append(",@Mcode=" + DB.SQuote(ItemCode));
            sbSqlString.Append(",@EmpCode=" + DB.SQuote(EmployeeCode));
            sbSqlString.Append(",@SerialNumber=" + DB.SQuote(SerialNumber));
            sbSqlString.Append(",@ProductionOrderHeaderId=" + productionOrderId);
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


        
        public string GetIORefNumber()
        {

            string IORefNumber = String.Empty;

            try
            {
                String NewIONumber = DB.GetSqlS("EXEC sp_SYS_GetSystemConfigValue @SysConfigKey=N'internalorder.aspx.cs.IO_Prefix',@TenantID=1");

                int length = Convert.ToInt32(DB.GetSqlS("EXEC sp_SYS_GetSystemConfigValue @SysConfigKey=N'internalorder.aspx.cs.IO_Length',@TenantID=1"));


                //string HeaderID = ViewState["HeaderID"].ToString();
                String OldIONumber = DB.GetSqlS("select top 1 IORefNo as S from MFG_InternalOrderHeader  order by IORefNo desc");

                NewIONumber += String.Empty + (Convert.ToInt16(DateTime.Now.Year) % 100);           //add year code to ponumber

                int power = (Int32)Math.Pow((double)10, (double)(length - 1));          //getting minvalue of prifix length

                String newvalue = String.Empty;
                if (OldIONumber.Length != 0 && NewIONumber.Equals(OldIONumber.Substring(0, NewIONumber.Length)))        //if ponumber is existed and same year ponumber  enter
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
            catch (Exception )
            {
                return String.Empty;
            }

        }
        
        public UserDataTable GetIOBOMDetails(string ProductionOrderID,string RoutingDetailsID,int WorkCenterID)
        {
            UserDataTable userTable = new UserDataTable();

            DataSet ds = DB.GetDS("[dbo].[sp_MFG_GetActivityWiseComponents] @ProductionOrderHeaderID=" + ProductionOrderID + ",@ROutingDetailsActivityID=" + RoutingDetailsID + ",@WorkCenterID="+WorkCenterID, false);

            DataTable dt = ds.Tables[0];
            userTable.Table = dt;
            ds.Dispose();
            dt.Dispose();
            return userTable;

          
        }

        public int CreateNewNCReqest(String ProductionOrderHeaderID, String WorkcenterID, String ActivityID, String ReasonForRequest, String RequestBY, String Remarks, String NCMaterialsList, String NCQuantityList, String ScrabQuantityList, String BatchNumberlist, String RoutingDetailsCaptureIDsList,string vActualCondition,string vShouldbeCondition,string NCReferenceNumber)
        { 
            //UseCaseReference
            //Shop Floor Tracker_ Production Process_UC_011_5_II_d_iv
            int RequestedBy=0;
            try
            {
                RequestedBy = DB.GetSqlN("SELECT TOP 1 UserID AS N  FROM GEN_USER WHERE EmployeeCode=" + DB.SQuote(RequestBY));
            }
            catch (Exception)
            {
                return -1;
            }

            StringBuilder cMdupdateNCRequestforscap = new StringBuilder();
            cMdupdateNCRequestforscap.Append("DECLARE @Result int  ");
            cMdupdateNCRequestforscap.Append("EXEC dbo.sp_MFG_CreateNCOrderForScrab ");
            cMdupdateNCRequestforscap.Append("@ProductionOrderHeaderID=" + ProductionOrderHeaderID);
            //cMdupdateNCRequestforscap.Append(",@IONumber=" + DB.SQuote(GetIONumber()));
            cMdupdateNCRequestforscap.Append(",@IONumber=" + DB.SQuote(NCReferenceNumber));
            cMdupdateNCRequestforscap.Append(",@WorkcenterID=" + WorkcenterID);
            cMdupdateNCRequestforscap.Append(",@Activity=" + ActivityID);
            cMdupdateNCRequestforscap.Append(",@ReasonForRequestID=" + ReasonForRequest);
            cMdupdateNCRequestforscap.Append(",@RequestedBy=" + RequestedBy);
            cMdupdateNCRequestforscap.Append(",@Remarks=" + DB.SQuote(Remarks));
            cMdupdateNCRequestforscap.Append(",@NCMaterialList=" + DB.SQuote(NCMaterialsList));
            cMdupdateNCRequestforscap.Append(",@ScrapQuantityList=" + DB.SQuote(ScrabQuantityList));
            cMdupdateNCRequestforscap.Append(",@QuantityList=" + DB.SQuote(NCQuantityList));
            cMdupdateNCRequestforscap.Append(",@BatchNoList=" + DB.SQuote(BatchNumberlist));
            cMdupdateNCRequestforscap.Append(",@RoutingDetailsActivityIDs=" + DB.SQuote(RoutingDetailsCaptureIDsList));
            cMdupdateNCRequestforscap.Append(",@IN_SONumber="+DB.SQuote(GetSONumber()));
            cMdupdateNCRequestforscap.Append(",@IN_OBDNumber="+DB.SQuote(GetOBDNumber()));
            cMdupdateNCRequestforscap.Append(",@IN_CusPONumber="+DB.SQuote(GetCUSPONumber()));
            cMdupdateNCRequestforscap.Append(",@IN_Tenant=1");
            cMdupdateNCRequestforscap.Append(",@ActualCondition="+DB.SQuote(vActualCondition));
            cMdupdateNCRequestforscap.Append(",@ShouldbeCondition=" + DB.SQuote(vShouldbeCondition));
            cMdupdateNCRequestforscap.Append(",@NewInternalOrderHeaderID=@Result OUTPUT SELECT @Result AS N");
            int OutboundID = -5;
            try
            {
                OutboundID = DB.GetSqlN(cMdupdateNCRequestforscap.ToString());
                if (OutboundID == -1)
                    return -4;
                else if (OutboundID == 0)
                    return -3;
            }
            catch (Exception)
            {
                return -2;
            }
                        
            return OutboundID;
        }

        private String GetOBDNumber()
        {
            String OBDNumber = String.Empty;
            try
            {

                int length = Convert.ToInt32(DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='OutboundDetails.aspx' ,@TenantID=" + 1));

                

                String NewOBDNumber = DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='dummyoutboundforproduction_prefix' ,@TenantID=" + 1) + (Convert.ToInt16(DateTime.Now.Year) % 100);           //add year code to ponumber

                String OldOBDNumber = DB.GetSqlS("select top 1 OBDNumber AS S from OBD_Outbound where OBDNumber LIKE '" + NewOBDNumber + "%' order by OBDNumber desc");

                int power = (Int32)Math.Pow((double)10, (double)(length - 1));          //getting minvalue of prifix length

                String newvalue = String.Empty;
                if (OldOBDNumber.Length != 0 && NewOBDNumber.Equals(OldOBDNumber.Substring(0, NewOBDNumber.Length)))        //if ponumber is existed and same year ponumber  enter
                {
                    String temp = OldOBDNumber.Substring(NewOBDNumber.Length, length);                            //getting number of last prifix
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

                NewOBDNumber += newvalue;
                OBDNumber = NewOBDNumber;
            }
            catch (Exception ex)
            {

            }
            return OBDNumber;
        }

        private String GetSONumber()
        {
            String SONumber = String.Empty;
            try
            {
                String NewSONumber = DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='dummysoheaderforinternalorder_prefix' ,@TenantID=" + 1);
                NewSONumber += String.Empty + (Convert.ToInt16(DateTime.Now.Year) % 100);

                int length = Convert.ToInt32(DB.GetSqlS("select SysConfigValue as S from SYS_SystemConfiguration as N where TenantID=" + 1 +" and SysConfigKeyID= (select SysConfigKeyID from SYS_SysConfigKey where SysConfigKey=N'salesorder.aspx.cs.SO_Length') "));

                //String OldSONumber = DB.GetSqlS("select TOP 1 SONumber as S from ORD_SOHeader where SOTypeID =6 and TenantID=" + cp.TenantID + "  ORDER BY SONumber desc ");
                String OldSONumber = DB.GetSqlS("select TOP 1 SONumber as S from ORD_SOHeader where SONumber like '" + NewSONumber + "%' and TenantID=" + 1 + "  ORDER BY SONumber desc ");

                //add year code to ponumber

                int power = (Int32)Math.Pow((double)10, (double)(length - 1));          //getting minvalue of prifix length

                String newvalue = String.Empty;
                if (OldSONumber.Length != 0 && NewSONumber.Equals(OldSONumber.Substring(0, NewSONumber.Length)))        //if ponumber is existed and same year ponumber  enter
                {
                    String temp = OldSONumber.Substring(NewSONumber.Length, length);                            //getting number of last prifix
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

                NewSONumber += newvalue;
                SONumber = NewSONumber;
            }
            catch (Exception ex)
            {

            }
            return SONumber;
        }

        private String GetCUSPONumber()
        {
            Random generateNo = new Random();
            return "DCUSPO" + generateNo.Next(1000, 10000);
        }

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

        private String GetIONumber()
        {
            String IONumber = String.Empty;
            try
            {
                String NewIONumber = DB.GetSqlS("EXEC sp_SYS_GetSystemConfigValue @SysConfigKey=N'internalorder.aspx.cs.IO_Prefix',@TenantID=1" );

                int length = Convert.ToInt32(DB.GetSqlS("EXEC sp_SYS_GetSystemConfigValue @SysConfigKey=N'internalorder.aspx.cs.IO_Length',@TenantID=1"));


                //string HeaderID = ViewState["HeaderID"].ToString();
                String OldIONumber = DB.GetSqlS("select top 1 IORefNo as S from MFG_InternalOrderHeader  order by IORefNo desc");

                NewIONumber += String.Empty + (Convert.ToInt16(DateTime.Now.Year) % 100);           //add year code to ponumber

                int power = (Int32)Math.Pow((double)10, (double)(length - 1));          //getting minvalue of prifix length

                String newvalue = String.Empty;
                if (OldIONumber.Length != 0 && NewIONumber.Equals(OldIONumber.Substring(0, NewIONumber.Length)))        //if ponumber is existed and same year ponumber  enter
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
                IONumber = NewIONumber;
            }
            catch (Exception ex)
            {
                return IONumber;
            }
            return IONumber;
        }
        public UserDataTable GetConsumedMatListForPositiveRecall(string vProductionID,int vRouteID)
        {
            UserDataTable table=new UserDataTable();
            DataSet ds = DB.GetDS("exec [dbo].[sp_MFG_GetPositiveRecallMatList]@RoutingDetailsActivityID="+vRouteID+",@ProductionOrderHeaderID="+vProductionID, false);
            DataTable dt = ds.Tables[0];
            table.Table = dt;
            ds.Dispose();
            dt.Dispose();
           
            return table;
        }
        public int UpsertPositiveRecallDetails(string vProductionOrderID,int vRouteID,string EmployeeCode,string vData,int vWorkCenterID)
        {
            StringBuilder sqlString = new StringBuilder();
            sqlString.Append("declare @ResultOUT int; ");
            sqlString.Append("EXEC [dbo].[sp_MFG_UpsertMaterialPositiveRecall]@ProductionOrderHeaderID="+vProductionOrderID);
            sqlString.Append(",@RoutingdetaisActivityID="+vRouteID);
            sqlString.Append(",@WorkCenterID="+vWorkCenterID);
            sqlString.Append(",@EmployeeCode="+DB.SQuote(EmployeeCode));
            sqlString.Append(",@Data="+DB.SQuote(vData));
            sqlString.Append(",@Result=@ResultOUT output");
            sqlString.Append(" select @ResultOUT as N");
            int Result = DB.GetSqlN(sqlString.ToString());
            return Result;
        }
        public UserDataTable GetDataTable(string exeQuery)
        {
            UserDataTable table = new UserDataTable();
            DataSet ds = DB.GetDS(exeQuery, false);
            DataTable dt = ds.Tables[0];
            table.Table = dt;
            ds.Dispose();
            dt.Dispose();
            return table;

        }
        public string UpsertGoodsMovementDetails_Kitting(int UserID,int ProductionOrderHeaderID,string MCode,Decimal Qty, int WorkCenterID,int GoodsMovementTypeID,string Remarks,string SerialNumber,string BatchNumber,string DelDocRefNo,string KItCode)
        {
            StringBuilder sbSqlString = new StringBuilder();
            sbSqlString.Append("declare @NewResult int ");
            sbSqlString.Append(" EXEC  [dbo].[sp_MFG_UpsertGoodsMovementDetails_Kitting]");
            sbSqlString.Append("@ProductionOrderHeaderID=" + ProductionOrderHeaderID);
            sbSqlString.Append(",@WorkCenterID=" + WorkCenterID);
            sbSqlString.Append(",@Mcode=" +DB.SQuote(MCode));
            sbSqlString.Append(",@BatchNo=" +DB.SQuote(BatchNumber));
            sbSqlString.Append(",@SerialNo=" + DB.SQuote(SerialNumber));
            sbSqlString.Append(",@GoodsMovementTypeID=" + GoodsMovementTypeID);
            sbSqlString.Append(",@Quantity=" +Qty);
            sbSqlString.Append(",@DelDocNumber=" +DB.SQuote(DelDocRefNo));
            sbSqlString.Append(",@KitCode=" +DB.SQuote(KItCode));
            sbSqlString.Append(",@CreatedBy=" + UserID);
            sbSqlString.Append(",@Result=@NewResult OUTPUT ");
            sbSqlString.Append("SELECT @NewResult AS N");
            
                try
                {
                    string ResponceResult = String.Empty;
                    int result = DB.GetSqlN(sbSqlString.ToString());
                   
                    if (result == 100)
                    {
                        ResponceResult = "Invalid del. doc. no.";
                    }
                    else if (result ==50)
                    {
                        ResponceResult = "Wire marking job order should be closed before receiving this material";
                    }
                    else if (result ==60)
                    {
                        ResponceResult = "Sleeve printing job order should be closed before receiving this material";
                    }
                    else if (result ==200)
                    {
                        ResponceResult = "Invalid part number";
                    }
                    else if (result == 300)
                    {
                        ResponceResult = "Invalid part number for given del. doc. qty.";
                    }
                    else if (result == 400)
                    {
                        ResponceResult = "Entered quantity is greater than del. doc. qty.";
                    }
                    else if (result == 1000)
                    {
                        ResponceResult = "Material cannot be received, as PGI is not done";
                    }
                    else if (result ==2000)
                    {
                        ResponceResult = " Material cannot be received, as the entered data is not issued from store";
                    }
                    else if (result ==3000)
                    {
                        ResponceResult = "Material cannot be received, as qty. is greater than issued qty.";
                    }
                    else if (result == 1)
                    {
                        ResponceResult = "1";
                    }
                    return ResponceResult;
                }
                catch
                {
                    return "Failed, please contact admin";
                }
        }
        public string UpdateGoodsoutForKitting(int ProductionOrderHeaderID,int UserID)
        {
            StringBuilder sbSqlString = new StringBuilder();
            sbSqlString.Append("declare @NewResult int ");
            sbSqlString.Append("EXEC  [dbo].[sp_MFG_GoodsOutKittingComponents]");
            sbSqlString.Append("@ProductionOrderHeaderID=" + ProductionOrderHeaderID);
            sbSqlString.Append(",@UserID=" + UserID);
            sbSqlString.Append(",@Result=@NewResult OUTPUT ");
            sbSqlString.Append("SELECT @NewResult AS N");
          
            try
            {
                string ResponceResult = String.Empty;
                int result = DB.GetSqlN(sbSqlString.ToString());
                if (result == -5)
                {
                    ResponceResult = "Failed, please contact admin";
                }
                else if(result == -1)
                {
                    ResponceResult = "-1";
                }else
                {
                    ResponceResult = "1";
                }
                return ResponceResult;
            }
            catch
            {
                return "Failed, please contact admin";
            }
       
        }
        public string CheckForOperationCompletion(int ProductionOrderHeaderID)
        {
            IDataReader reader = DB.GetRS("EXEC [dbo].[sp_MFG_CheckForCloseJObOrder]@PRODUCTIONORDERHEADERID="+ProductionOrderHeaderID);
            if (reader.Read())
            {
                reader.Close();
                return "1";

            }
            else
            {
                return "0";
            }
           
        }
        public bool CloseJobOrder(int ProductionOrderHeaderID,int RoutingType,string EmployeeCode)
        {
            if (RoutingType != 2)
            {
                try
                {
                    int UserID = DB.GetSqlN("SELECT UserID as N FROM GEN_User WHERE EmployeeCode=" + DB.SQuote(EmployeeCode));
                    DB.ExecuteSQL("UPDATE MFG_ProductionOrderHeader SET ProductionOrderStatusID=5,ClosedOn=GETDATE(),ClosedBy="+UserID+" WHERE ProductionOrderHeaderID=" + ProductionOrderHeaderID);
                    return true;
                }
                catch
                {
                    return false;
                }
                
            }
            else
            {
                int UserID = DB.GetSqlN("SELECT UserID as N FROM GEN_User WHERE EmployeeCode=" + DB.SQuote(EmployeeCode));
                string vStorerefNumber = GetDummyStoreRefNo();
              
                string vPoNumber = GetDummyPONumber();
                if (vStorerefNumber.Length == 0 || vPoNumber.Length==0)
                {
                    return false;
                }
                string vInvoiceNumber = GetDummyInvoiceNo();
                StringBuilder sbSqlString = new StringBuilder();
                sbSqlString.Append("EXEC  [dbo].[sp_MFG_DummyInboundForProductionOrder]");
                sbSqlString.Append("@ProductionOrderHeaderID=" + ProductionOrderHeaderID);
                sbSqlString.Append(",@PONumber=" + vPoNumber);
                sbSqlString.Append(",@StoreRefNo="+vStorerefNumber);
                sbSqlString.Append(",@InvNumber=" + vInvoiceNumber);
                sbSqlString.Append(",@CreatedBy=" + UserID);
                try
                {
                    DB.ExecuteSQL(sbSqlString.ToString());
                    DB.ExecuteSQL("UPDATE MFG_ProductionOrderHeader SET ProductionOrderStatusID=5,ClosedOn=GETDATE(),ClosedBy=" + UserID + " WHERE ProductionOrderHeaderID=" + ProductionOrderHeaderID);
                    return true;
                }
                catch
                {
                    return false;
                }
              
            }
            
        }
        private string GetDummyStoreRefNo()
        {


            string vNewStoreRefNUmber = null;

            try
            {
                string StoreRefNoLength = DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='dummyinboundforproduction_prefix_length' ,@TenantID=1");  //DB.GetSqlS("select SysConfigValue as S from SYS_SystemConfiguration SYS_SC left join SYS_SysConfigKey SYS_SCK on  SYS_SCK.SysConfigKeyID=SYS_SC.SysConfigKeyID where TenantID=" + cp.TenantID + " and SYS_SCK.SysConfigKey='inbounddetails.aspx.cs.StoreRefNo_Length' and SYS_SCK.IsActive=1");

                vNewStoreRefNUmber = DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='dummyinboundforproductionreturn_prefix' ,@TenantID=1") + DB.GetSqlS("select WarehouseGroupCode as S from GEN_Warehouse GEN_W  left join GEN_WarehouseGroup GEN_WG on GEN_WG.WarehouseID=GEN_W.WarehouseID where  GEN_WG.WarehouseID=1");

                string OldStoreRefNumber = DB.GetSqlS("select top 1  StoreRefNo AS S from INB_Inbound where  TenantID=1 and StoreRefNo like '" + vNewStoreRefNUmber + "%' order by InboundID DESC "); // Get Previous StoreRefNo


                vNewStoreRefNUmber += (Convert.ToInt16(DateTime.Now.Year) % 100);           //add year code to StoreRefNUmber
                //vNewStoreRefNUmber += "" + (2014 % 100);
                int length = Convert.ToInt32(StoreRefNoLength);                   //get prifix length
                int power = (Int32)Math.Pow((double)10, (double)(length - 1));          //get minvalue of prifix length


                if (OldStoreRefNumber.Length != 0 && vNewStoreRefNUmber.Equals(OldStoreRefNumber.Substring(0, vNewStoreRefNUmber.Length)))        //get same year StoreRefNo if StoreRefNo is exists
                {
                    string temp = OldStoreRefNumber.Substring(vNewStoreRefNUmber.Length, length);                            //get number of last prifix
                    Int32 number = Convert.ToInt32(temp);
                    number++;


                    while (power > 1)                                                                           //add '0' to number at left side for get 
                    {
                        if (number / power > 0)
                        {
                            //newvalue += number;
                            break;
                        }
                        vNewStoreRefNUmber += "0";
                        power /= 10;
                    }
                    vNewStoreRefNUmber += String.Empty + number;
                }
                else
                {                                                                                           //other wise generate first number 
                    for (int i = 0; i < length - 1; i++)
                        vNewStoreRefNUmber += "0";
                    vNewStoreRefNUmber += "1";
                }

                //vNewStoreRefNUmber += newvalue; // New StoreREfNumber

                // txtStrRefNo.Text = vNewStoreRefNUmber;
            }
            catch (Exception)
            {
                vNewStoreRefNUmber = String.Empty;
                // resetError("Error while generating StoreRef#", true);
            }

            return vNewStoreRefNUmber;
        }

        private string GetDummyPONumber()
        {
            string NewPONumber = String.Empty;
            try
            {
                NewPONumber = DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='dummypoheaderforproduction_prefix' ,@TenantID=1"); //"D"+DB.GetSqlS("select SysConfigValue as S from SYS_SystemConfiguration where TenantID=" +cp.TenantID + " and SysConfigKeyID= (select SysConfigKeyID from SYS_SysConfigKey where SysConfigKey=N'purchaseorder.aspx.cs.PO_Prefix') ");
                NewPONumber += String.Empty + (Convert.ToInt16(DateTime.Now.Year) % 100);           //add year code to ponumber

                int length = Convert.ToInt16(DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='dummypoheaderforproduction_prefix_length' ,@TenantID=1")); //Convert.ToInt32(DB.GetSqlS("select SysConfigValue as S from SYS_SystemConfiguration as N where TenantID=" + cp.TenantID + " and SysConfigKeyID= (select SysConfigKeyID from SYS_SysConfigKey where SysConfigKey=N'purchaseorder.aspx.cs.PO_Length') "));


                //String OldPONumber = DB.GetSqlS("select TOP 1 PONumber as S from ORD_POHeader where POTypeID=6 and TenantID=" + cp.TenantID + "  ORDER BY PONumber desc ");
                string OldPONumber = DB.GetSqlS("select TOP 1 PONumber as S from ORD_POHeader where PONumber like '" + NewPONumber + "%' and TenantID=1 ORDER BY PONumber desc ");


                int power = (Int32)Math.Pow((double)10, (double)(length - 1));          //getting minvalue of prifix length

                string newvalue = String.Empty;
                if (OldPONumber.Length != 0 && NewPONumber.Equals(OldPONumber.Substring(0, NewPONumber.Length)))        //if ponumber is existed and same year ponumber  enter
                {
                    string temp = OldPONumber.Substring(NewPONumber.Length, length);                            //getting number of last prifix
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

                NewPONumber += newvalue;
                // txtPONumber.Text = NewPONumber;
            }
            catch (Exception ex)
            {
                NewPONumber = String.Empty;
                // resetError(ex.ToString(), true);
            }
            return NewPONumber;
        }
        private string GetDummyInvoiceNo()
        {
            Random generateNo = new Random();
            return "INV" + generateNo.Next(1000, 10000);
        }
        public string UpdateReworkProcedureCapture(int RouteID, int NCRefrenceNumber, string SerialNumbers, string ReworkProcedureText, string employeeCode, string ProductionOrderID, string UserUpdatedProcedure,string ReworkProcedureIDs)
        {
            StringBuilder sbSqlString = new StringBuilder();
            sbSqlString.Append("EXEC  [dbo].[sp_MFG_UpsertReworkProcudureCapture]");
            sbSqlString.Append("@ReworkProcedureText=" +DB.SQuote(ReworkProcedureText));
            sbSqlString.Append(",@LineNumbers=" +DB.SQuote(SerialNumbers));
            sbSqlString.Append(",@EmployeeCode=" + DB.SQuote(employeeCode));
            sbSqlString.Append(",@NCReferenceNumber=" + NCRefrenceNumber);
            sbSqlString.Append(",@RoutingDetailsActivityID=" + RouteID);
            sbSqlString.Append(",@ProductionOrderHeaderID=" + ProductionOrderID);
            sbSqlString.Append(",@ReworkProcedureIDs=" + DB.SQuote(ReworkProcedureIDs));
            sbSqlString.Append(",@UserUpdatedReworkProcedure=" +DB.SQuote( UserUpdatedProcedure));
            
            try
            {
                DB.ExecuteSQL(sbSqlString.ToString());
                return "1";
            }
            catch
            {
                return "0";
            }
            

        }
        public string UpsertTERCapture(string ProductionOrderID, int RoutingDetailsActivityID, string RouteTERIDList, string InsertValues,string EmployeeCode, string UpdateValues,string UpdateTERCaptureIDList,int TERTypeID)
        {
            StringBuilder sbSqlString = new StringBuilder();
            sbSqlString.Append("EXEC  [dbo].[sp_MFG_UpsertTERCapture]");
            sbSqlString.Append("@RoutingDetailsActivityID=" + RoutingDetailsActivityID);
            sbSqlString.Append(",@ProductionOrderHeaderID=" + ProductionOrderID);
            sbSqlString.Append(",@RoutingDetailsActivity_TERID=" + DB.SQuote(RouteTERIDList));
            sbSqlString.Append(",@InsertingResult=" + DB.SQuote(InsertValues));
            sbSqlString.Append(",@TerTyPeID=" +TERTypeID);
            sbSqlString.Append(",@EmployeeCode=" + DB.SQuote(EmployeeCode));
            sbSqlString.Append(",@UpdateResult=" + DB.SQuote(UpdateValues));
            sbSqlString.Append(",@UpdatableTERCaptureIDList=" + DB.SQuote(UpdateTERCaptureIDList));
            
            try
            {
                DB.ExecuteSQL(sbSqlString.ToString());
                return "1";
            }
            catch
            {
                return "0";
            }
        }
        public string ScrapActivity(int RoutingActivityID, int ProductionOrderHeaderID, string EmployeeCode,string Remarks)
        {


            StringBuilder sbSqlString = new StringBuilder();
            sbSqlString.Append("declare @NewResult int ");
            sbSqlString.Append("EXEC  [dbo].[sp_MFG_FlushActivityDetailsWithMaterialConsumption]");
            sbSqlString.Append("@ProductionORderHeaderID=" + ProductionOrderHeaderID);
            sbSqlString.Append(",@RoutingDetailsActivityID=" + RoutingActivityID);
            sbSqlString.Append(",@EmployeeCode=" +DB.SQuote(EmployeeCode));
            sbSqlString.Append(",@Remarks=" + DB.SQuote(Remarks));
            sbSqlString.Append(",@Result=@NewResult OUTPUT ");
            sbSqlString.Append("SELECT @NewResult AS N");
            //int userID=DB.GetSqlN("select UserID as N from GEN_User where EmployeeCode="+EmployeeCode);
            //string sql = "update MFG_RoutingDetailsActivityCapture set IsDeleted=1,DeletedBy="+userID+",DeletedRemarks="+DB.SQuote(Remarks)+" where  ProductionOrderHeaderID="+ProductionOrderHeaderID+" and RoutingDetailsActivityID="+RoutingActivityID+" and IsActive=1";
            try
            {
                int Result=DB.GetSqlN(sbSqlString.ToString());
                if (Result == 1)
                {
                    return "1";
                }
                else
                {
                    return "Failed, please contact admin";
                }
                //DB.ExecuteSQL(sbSqlString.ToString());
              
            }
            catch
            {
                return "Failed, please contact admin";
            }
           
        }
        public UserDataTable GetReworkProcedureData(string dataSource)
        {
            UserDataTable userTable = new UserDataTable();
            DataSet ds = DB.GetDS(dataSource, false);
            DataTable dt = ds.Tables[0];
            userTable.Table = dt;

            ds.Dispose();
            dt.Dispose();

            return userTable;

        }
        public int PauseActivity(int vPauseType, int vRoutingDetaisActivityCaptureID, string Remarks, int vRoutingDetailsActivityCapture_PauseActivityID,string Employeecode)
        {
            StringBuilder sbSqlString = new StringBuilder();
            sbSqlString.Append("EXEC  [dbo].[sp_MFG_UpsertRoutingDetailsActivity_PauseActivity]");
            sbSqlString.Append("@PausedType=" + vPauseType);
            sbSqlString.Append(",@RoutingDetailsActivityCaptureID=" + vRoutingDetaisActivityCaptureID);
            sbSqlString.Append(",@Remarks=" + DB.SQuote(Remarks));
            sbSqlString.Append(",@EmployeeCode=" + DB.SQuote(Employeecode));
            sbSqlString.Append(",@RoutingDetailsActivityCapture_PauseActivityID=" + vRoutingDetailsActivityCapture_PauseActivityID);
            try
            {
                DB.ExecuteSQL(sbSqlString.ToString());
                return 1;
            }
            catch
            {
                return 0;
            }
         
        }
        public string UploadFile(ArrayList list, string fileName,string NCRefNumber,string FolderName)
        {
            // the byte array argument contains the content of the file
            // the string argument contains the name and extension
            // of the file passed in the byte array
            fileName = fileName.Remove(fileName.Length - 1, 1);
            string[] filenames = fileName.Split(',');
            for (int index = 0; index < list.Count; index++)
            {
                byte[] f = list[index] as byte[];
                try
                {
                    // instance a memory stream and pass the
                    // byte array to its constructor
                    MemoryStream ms = new MemoryStream(f);

                    // instance a filestream pointing to the 
                    // storage folder, use the original file name
                    // to name the resulting file
                  

                    // write the memory stream containing the original
                    // file as a byte array to the filestream

                    if (FolderName == "NCAnalysisSheet")
                    {
                        try
                        {
                            String Savepath = System.Web.HttpContext.Current.Server.MapPath("~/SFTResources/" + FolderName + "/");
                            if (!Directory.Exists(Savepath))
                                Directory.CreateDirectory(Savepath);
                            FileStream fs = new FileStream(Savepath + NCRefNumber + ".pdf", FileMode.Create);
                            try
                            {
                                DB.ExecuteSQL("UPDATE MFG_RoutingDetailsActivityCapture  SET IsNCAnalysisAttached=1 where NCReferenceNumber=" + NCRefNumber);
                            }
                            catch (Exception ex)
                            {
                                return ex.Message.ToString();
                            }
                            ms.WriteTo(fs);

                            //clean up
                            ms.Close();
                            fs.Close();
                            fs.Dispose();
                        }
                        catch (Exception ex)
                        {
                            return ex.Message.ToString();
                        }

                    }
                    else
                    {
                        String Savepath = System.Web.HttpContext.Current.Server.MapPath("~/SFTResources/" + FolderName + "/") + NCRefNumber + "/";
                        if (!Directory.Exists(Savepath))
                            Directory.CreateDirectory(Savepath);
                        FileStream fs = new FileStream(Savepath + filenames[index], FileMode.Create);
                        ms.WriteTo(fs);

                        //clean up
                        ms.Close();
                        fs.Close();
                        fs.Dispose();
                    }
                  


                }
                catch (Exception ex)
                {
                    // return the error message if the operation fails
                    return ex.Message.ToString();
                }
            }
          

            return "OK";
        }
        public string GetReleaseVersion()
        {
            String retResult = String.Empty;

            string sql = "select top 1 isnull(Version+'|'+convert(nvarchar,ReleaseDate,103)+'|'+Remarks,'') as S from SFTRelease order by version desc";
            retResult = DB.GetSqlS(sql);
            return retResult;

        }
        public bool GetSQRConfirmation(string vEmployeecode,string vPassword)
        {
            string Query = "Select UserID as N from GEN_User where BINARY_CHECKSUM(EmployeeCode)=BINARY_CHECKSUM(" + DB.SQuote(vEmployeecode) + ") " +
                        "and BINARY_CHECKSUM(Password)=BINARY_CHECKSUM(" + DB.SQuote(vPassword) + ")";
            try
            {
                if (DB.GetSqlN(Query)!= 0)
                {

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
           
        }
        public string GetTestData()
        {
            return "Swamy";
        }

        public string RejectReworkProcedure(string NCReferenceNumber)
        {
            string query = "UPDATE MFG_ReworkProcedureCapture set SupervisorAcceped=NULL,SupervisorID=NULL,QCAccepted=NULL,QCID=NULL where NCReferenceNO=" + NCReferenceNumber;
            try
            {
                DB.ExecuteSQL(query);
                return "Successfully updated";
            }
            catch (Exception ex)
            {
                return "Failed, please contact admin";
            }
        
        }
        public string CaptureNCCustomerMRBDispositionDetails(string vRoutingDetailsActivityCaptureID,string vCFTnum,string vCustomerNCNo,string vMRBDisposition)
        {
            string _updateQuery = "UPDATE MFG_RoutingDetailsActivityCapture set CFTNo=" + DB.SQuote(vCFTnum) + ",CustomerNCNo=" + DB.SQuote(vCustomerNCNo) + ",MRBDisposition=" + DB.SQuote(vMRBDisposition) + " where RoutingDetailsActivityCaptureID=" + vRoutingDetailsActivityCaptureID;
            try
            {
                DB.ExecuteSQL(_updateQuery);
                return "1";
            }
            catch
            {
                return "0";
            }
           
        }
        public List<THHTWSData.KeyValueStruct> GetReworkProcedureNew(string NCReferenceNumber, string ProductionOrderHeaderID,int RoutingDetailsActivityID)
        {
            List<THHTWSData.KeyValueStruct> sources = new List<THHTWSData.KeyValueStruct>();
            string dataSource = "select SerilaNo,ReworkProcedureText,OperatorRemarks,isnull(OperatorAccepted,3) as OperatorAccepted,QCRemarks, " +
                                "isnull(QCAccepted,3) as QCAccepted,ReworkProcedureCaptureID,ISNULL(IsMEApproved,0) IsMEApproved,CASE WHEN ISNULL(OperatorAccepted,3)=1 AND ISNULL(QCAccepted,3)=1 THEN 1 ELSE 0 END AS IsFinished  from MFG_ReworkProcedureCapture " +
                                "where NCReferenceNO=" + DB.SQuote(NCReferenceNumber) + "AND ProductionOrderHeaderID=" + ProductionOrderHeaderID + "AND RoutingDetailsActivityID="+RoutingDetailsActivityID;
            IDataReader rsGetData = DB.GetRS(dataSource);
             sources.Clear();
             while (rsGetData.Read())
             {
                 sources.Add(new THHTWSData.KeyValueStruct("SerilaNo", DB.RSFieldInt(rsGetData, "SerilaNo").ToString()));
                 sources.Add(new THHTWSData.KeyValueStruct("ReworkProcedureText", DB.RSField(rsGetData, "ReworkProcedureText").ToString()));
                 sources.Add(new THHTWSData.KeyValueStruct("OperatorRemarks", DB.RSField(rsGetData, "OperatorRemarks").ToString()));
                 sources.Add(new THHTWSData.KeyValueStruct("OperatorAccepted", DB.RSFieldTinyInt(rsGetData, "OperatorAccepted").ToString()));
                 sources.Add(new THHTWSData.KeyValueStruct("QCRemarks", DB.RSField(rsGetData, "QCRemarks").ToString()));
                 sources.Add(new THHTWSData.KeyValueStruct("QCAccepted", DB.RSFieldTinyInt(rsGetData, "QCAccepted").ToString()));
                 sources.Add(new THHTWSData.KeyValueStruct("ReworkProcedureCaptureID", DB.RSFieldInt(rsGetData, "ReworkProcedureCaptureID").ToString()));
                 sources.Add(new THHTWSData.KeyValueStruct("IsMEApproved", DB.RSFieldTinyInt(rsGetData, "IsMEApproved").ToString()));
                 sources.Add(new THHTWSData.KeyValueStruct("IsFinished", DB.RSFieldTinyInt(rsGetData, "IsFinished").ToString()));
             }
            return sources;
        }

        public string UpdateReworkProcedureTextNew(string vNCReferenceNumber,int ReworkProcedureCaptureID,int OperatorID,int OperatorResult,string OperatorRemarks,int QCID,int QCResult,string QCRemarks,int ProductionOrderHeaderID)
        {
            return String.Empty;
        }

        public bool MEApprovalForReworkProcedure(int MEUserID,int RouteID,string NCReference,string ProductionOrderID,int MEPassORFail)
        {
            string DataSource = "UPDATE MFG_ReworkProcedureCapture SET IsMEApproved="+MEPassORFail+",MEUserID="+MEUserID+" WHERE NCReferenceNO="+DB.SQuote(NCReference)+" AND RoutingDetailsActivityID="+RouteID+" AND ProductionOrderHeaderID="+ProductionOrderID;
            try
            {
                DB.ExecuteSQL(DataSource);
                return true;
            }
            catch
            {
                return false;
            }
           
        }

        public int CaptureReworkProcedureResults(string Employeecode,string QCEmployeecode,int IsOperatorAccepted,string OperatorRemarks,int IsQCAccepted,string QCRemarks,int ReworkProcedureCaptureID,string NCReferenceNumber,int RoutingdetailsActivityID,int ProductionOrderID)
        {

            StringBuilder sbSqlString = new StringBuilder();
            sbSqlString.Append("declare @NewResult int ");
            sbSqlString.Append("EXEC  [dbo].[sp_MFG_CaptureReworkProcudureResults]");
            sbSqlString.Append("@OperatorEmployeeCode=" +DB.SQuote(Employeecode));
            sbSqlString.Append(",@QCEmployeeCode=" + DB.SQuote(QCEmployeecode));
            sbSqlString.Append(",@IsOperatorAccepted=" +IsOperatorAccepted);
            sbSqlString.Append(",@OperatorRemarks=" + DB.SQuote(OperatorRemarks));
            sbSqlString.Append(",@IsQCAccepted=" + IsQCAccepted);
            sbSqlString.Append(",@QCRemarks=" + DB.SQuote(QCRemarks));
            sbSqlString.Append(",@ReworkProcedureCaptureID=" + ReworkProcedureCaptureID);
            sbSqlString.Append(",@NCReferenceNumber=" +DB.SQuote(NCReferenceNumber));
            sbSqlString.Append(",@RoutingDetaisActivityID="+RoutingdetailsActivityID);
            sbSqlString.Append(",@ProdutionOrderHeaderID=" + ProductionOrderID);
            sbSqlString.Append(",@Result=@NewResult OUTPUT ");
            sbSqlString.Append("SELECT @NewResult AS N");
            try
            {
                int result = DB.GetSqlN(sbSqlString.ToString());
                return result;
            }
            catch
            {
                return -1;
            }
        }
        public int DeleteToolData(string RoutingDetailsActivity_ToolsID)
        {
            string strDeleteQuery = "update MFG_RoutingDetailsActivity_Tools set IsDeleted=1 where RoutingDetailsActivity_ToolsID=" + RoutingDetailsActivity_ToolsID;
            try
            {
                DB.ExecuteSQL(strDeleteQuery);
                return 1;
            }
            catch
            {
                return 0;
            }
        }

    }

}
