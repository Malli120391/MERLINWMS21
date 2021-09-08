using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using MRLWMSC21Common;
using System.Data;
using System.Collections;
using System.ServiceModel.Web;

namespace MRLWMSC21WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService2" in both code and config file together.

    [ServiceContract, XmlSerializerFormat]
    public interface IFalconMFG
    {
        [OperationContract]
        string DoWork();

        [OperationContract]
        List<THHTWSData.KeyValueStruct> GetRoleID(String empcode, int WorkCenterID);
    
        [OperationContract]
        string GetIORefNumber();

        [OperationContract]
        UserDataTable GetIOBOMDetails(string ProductionOrderID, string RoutingDetailsID, int WorkCenterID);

        [OperationContract]
        List<THHTWSData.KeyValueStruct> CheckUserLogin(string UserID, string Password, string LoginIPAddress, string WorkcenterName);

        [OperationContract]
        UserDataTable GetProductionOrderList(int WorkCenterID);

        [OperationContract]
        UserDataTable GetOperations(String ProductionOrderHeaderID, int WorkCenterID);

        [OperationContract]
        UserDataTable GetWorkCenterWiseHeaderData(String ProductionOrderHeaderID, int WorkCenterID);

        [OperationContract]
        bool UpsertRoutingDetailsActivityPassorFail(String RouteActivityID, String EmpCode, int PassOrFail, String vProductionOrderID, String vRemarks, string vCheckListResultData, int vUserRoleID, string vReasonForDelay, int DelayTime, string vDispotionText, int vIsReworkRequired,
            string vActualCondition, string vShouldbeCondition, string ImmediateActionTargetDate, string CorrectiveActionReferrence, string RCAteamFormation, string TargetDateForCAPA, string CommunicationDate, string NonCorrectiveActionRemarks, int CorrectiveActionRequired, int vDispositionID, int IsReworkProcedureAmmendment, string ParentNCReference);

        [OperationContract]
        string UpsertGoodsMovementDetails(string EmployeeCode, string ProductionOrderHeaderID, string RoutingDetailsActivityID, string MCode, decimal Qty, string WorkCenterID, int GoodsMovementTypeID, int IsDamaged, int IsLocallyProduced, string Remarks, string ScrapCode, string reworkroutingID, string SerialNumber, string BatchNumber, int IsPositiveRecall, string DeliveryDocNumber);

        [OperationContract]
        UserDataTable GetMaterialReceivedList(String ProductionOrderHeaderID, String WorkCenterID);

        [OperationContract]
        bool UpdatePositiveRecall(int WorkCenterID, string ProductionOrderID, string remarks, String EmployeeCode, int vstatus, int vRoutingDetailsID);

        [OperationContract]
        List<THHTWSData.KeyValueStruct> GetQCParameters(int RoutingDetailsActivityID, String vProductionOrderHeaderID);

        [OperationContract]
        UserDataTable GetActivityLevelComponents(int RoutingDetailsActivityID, int ProductionOrderHeaderID, int WorkCenterID);

        [OperationContract]
        UserDataTable GetOperationActivities(int UserID, String OperationNumber, int ProductionOrderHeaderID, int WorkCenterID);


        [OperationContract]
        string UpsertRoutingDetailsActivityCapture(int RoutingdetailsActivityID, String ProductionOrderHeaderID, String UserCode, int StartTime, string Remarks, int RoleID);

        [OperationContract]
        UserDataTable GetOperatorActivityResuls(int vRoutingDetailsActivityID,int ProductionOrderID);

        [OperationContract]
        int IsPassOrFailCaptured(int vRouteID, String EmpCode, String vProductionOrderID, int roleID);
        [OperationContract]
        int UpsertToolCapture(String ItemCode, String SerialNumber, int vRouteID, String EmployeeCode, String productionOrderId);

        [OperationContract]
        int CreateNewNCReqest(String ProductionOrderHeaderID, String WorkcenterID, String ActivityID, String ReasonForRequest, String RequestBY, String Remarks, String NCMaterialsList, String NCQuantityList, String ScrabQuantityList, String BatchNumberlist, String RoutingDetailsCaptureIDsList, string vActualCondition, string vShouldbeCondition, string NCReferenceNumber);
         [OperationContract]
         List<THHTWSData.KeyValueStruct> GetDropDownData(String SqlString, String DefaultValue);
          [OperationContract]
         UserDataTable GetOperationstatus(String ProductionOrderHeaderID);
          [OperationContract]
          UserDataTable GetConsumedMatListForPositiveRecall(string vProductionID, int vRouteID);
          [OperationContract]
          int UpsertPositiveRecallDetails(string vProductionOrderID, int vRouteID, string EmployeeCode, string vData, int vWorkCenterID);
          [OperationContract]
          string UpsertGoodsMovementDetails_Kitting(int UserID,int ProductionOrderHeaderID,string MCode,Decimal Qty, int WorkCenterID,int GoodsMovementTypeID,string Remarks,string SerialNumber,string BatchNumber,string DelDocRefNo,string KItCode);
          [OperationContract]
          int LogOutUser(int UserID);
        [OperationContract]
        UserDataTable GetDataTable(string exeQuery);
        [OperationContract]
        string UpdateGoodsoutForKitting(int ProductionOrderHeaderID, int UserID);
        [OperationContract]
        string CheckForOperationCompletion(int ProductionOrderHeaderID);
        [OperationContract]
        bool CloseJobOrder(int ProductionOrderHeaderID, int RoutingType, string EmployeeCode);
        [OperationContract]
        string ScrapActivity(int RoutingActivityID, int ProductionOrderHeaderID, string EmployeeCode, string Remarks);
        [OperationContract]
        string UpdateReworkProcedureCapture(int RouteID, int NCRefrenceNumber, string SerialNumbers, string ReworkProcedureText, string employeeCode, string ProductionOrderID, string UserUpdatedProcedure,string ReworkProcedureIDs);
        [OperationContract]
        string UpsertTERCapture(string ProductionOrderID, int RoutingDetailsActivityID, string RouteTERIDList, string InsertPreTestValueList, string EmployeeCode, string UpdatePreTestValueList, string UpdateTERCaptureIDList, int TERTypeID);
        [OperationContract]
        UserDataTable GetReworkProcedureData(string dataSource);
        [OperationContract]
        TableData GetOperationsData(String ProductionOrderHeaderID, int WorkCenterID);
        [OperationContract]
        int PauseActivity(int vPauseType, int vRoutingDetaisActivityCaptureID, string Remarks, int vRoutingDetailsActivityCapture_PauseActivityID, string Employeecode);
        [OperationContract]
        string UploadFile(ArrayList list, string fileName, string NCRefNumber, string FolderName);
        [OperationContract]
        string GetReleaseVersion();
        [OperationContract]
        bool GetSQRConfirmation(string vEmployeecode, string vPassword);
        [OperationContract]
        /*[WebInvoke(UriTemplate = "/Data", Method = "GET", ResponseFormat = WebMessageFormat.p)]*/
        string GetTestData();
        [OperationContract]
        string RejectReworkProcedure(string NCReferenceNumber);

        [OperationContract]
        string CaptureNCCustomerMRBDispositionDetails(string vRoutingDetailsActivityCaptureID, string vCFTnum, string vCustomerNCNo, string vMRBDisposition);
        [OperationContract]
        List<THHTWSData.KeyValueStruct> GetReworkProcedureNew(string NCReferenceNumber, string ProductionOrderHeaderID, int RoutingDetailsActivityID);
        [OperationContract]
        bool MEApprovalForReworkProcedure(int MEUserID, int RouteID, string NCReference, string ProductionOrderID, int MEPassORFail);
        [OperationContract]
        int CaptureReworkProcedureResults(string Employeecode, string QCEmployeecode, int IsOperatorAccepted, string OperatorRemarks, int IsQCAccepted, string QCRemarks, int ReworkProcedureCaptureID, string NCReferenceNumber, int RoutingdetailsActivityID, int ProductionOrderID);

        [OperationContract]
        int DeleteToolData(string RoutingDetailsActivity_ToolsID);
     
    }
   
    [DataContract]
    public class UserDataTable
    {
        [DataMember]
        public DataTable Table
        {
            get;
            set;
        }
    }
   
    public class TableData
    {

        [DataMember]
        public DataTable OperationsData
        {
            get;
            set;
        }
        [DataMember]
        public DataTable ActivitiesData
        {
            get;
            set;
        }
        [DataMember]
        public DataTable ActivityMaterialData
        {
            get;
            set;
        }
        [DataMember]
        public DataTable ActivityResultsData
        {
            get;
            set;
        }


    }
}
