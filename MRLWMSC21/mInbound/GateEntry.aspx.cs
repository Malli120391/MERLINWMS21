using MRLWMSC21Common;
using MRLWMSC21.mMaterialManagement;
using MRLWMSC21.mOutbound.BL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
//using System.Net.Http;

namespace MRLWMSC21.mInbound
{
    public partial class GateEntry : System.Web.UI.Page
    {
        public static CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            // Page.Theme = "Inventory";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Add Gate Entry");
            }
        }
        [WebMethod]
        public static List<DropDownData> GetAccounts()
        {
            List<DropDownData> olst = new List<DropDownData>();
            DataSet ds = DB.GetDS("select accountid,account from GEN_Account where 0=" + cp1.AccountID + " or accountid=" + cp1.AccountID, false);
            try
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    olst.Add(new DropDownData()
                    {
                        ID = Convert.ToInt32(row["accountid"]),
                        Value = (row["account"]).ToString()

                    });
                }
            }
            catch (Exception e)
            {
            }
            return olst;
        }
        [WebMethod]
        public static List<DropDownListDataFilter> getWareHouseData(string TenantId)
        {
            try
            {
                OutBoundBL objbl = new OutBoundBL();
                return objbl.getWareHouseDataData(TenantId);
            }
            catch
            {
                return null;
            }

        }
        [WebMethod]
        public static List<DropDownListDataFilter> GetInboundDataForGateEntry(string prefix,string TenantId)
        {
            try
            {
                OutBoundBL objbl = new OutBoundBL();
                return objbl.GetInboundDataForGateEntry(prefix,TenantId);
            }
            catch
            {
                return null;
            }

        }
        [WebMethod]
        public static List<DropDownListDataFilter> GetDocumentTypes(string ShipemtTypeID)
        {
            try
            {
                OutBoundBL objbl = new OutBoundBL();
                return objbl.GetDocumenttypesForGateEntry(Convert.ToInt32(ShipemtTypeID));
            }
            catch
            {
                return null;
            }

        }
        [WebMethod]
        public static List<DropDownListDataFilter> GetMIMETypes()
        {
            try
            {
                OutBoundBL objbl = new OutBoundBL();
                return objbl.GetMIMETypesForGateEntry();
            }
            catch
            {
                return null;
            }

        }

        [WebMethod]
        public static int GetCurrentAccount()
         {
            return cp1.AccountID;
        }
        [WebMethod]
        public static int UpsertGateEntryHeader(GateEntryHeader obj,int Status)
        {

            try
            {
                //if(obj.EstIBDockAssignTime!="")
                //{
                //    obj.EstIBDockAssignTime = DateTime.ParseExact(obj.EstIBDockAssignTime, "dd-MMM-yyyy HH:mm", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm");
                //}
                //if (obj.EstIBOperationTime != "")
                //{
                //    obj.EstIBOperationTime = DateTime.ParseExact(obj.EstIBOperationTime, "dd-MMM-yyyy HH:mm", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm");
                //}
                //if (obj.UnloadingTime != "")
                //{
                //    obj.UnloadingTime = DateTime.ParseExact(obj.UnloadingTime, "dd-MMM-yyyy HH:mm", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm");
                //}
                int? unloadedqty = 0;
                if(obj.UnloadedQty!="")
                {
                    unloadedqty =0;
                    //Convert.ToInt32(obj.UnloadedQty)
                }
                if (obj.FrieghtCompanyid == "")
                {
                    obj.FrieghtCompanyid = "0";
                }
                if (obj.ArrivingFromId == "")
                {
                    obj.ArrivingFromId = "0";
                }
                if(obj.ArrivingCityId == "" || obj.ArrivingCityId==null)
                {
                    obj.ArrivingCityId = "0";
                }
                if (obj.ArrivingStateId == "" || obj.ArrivingStateId == null)
                {
                    obj.ArrivingStateId = "0";
                }


                int res =     DB.GetSqlN("exec [SP_UpsertGateEntryDetails] @GateEnryId=" + obj.GateEntryId + ",@YM_MST_Vehicle_ID=" + obj.VehicleId + ",@WarehouseID=" + obj.WareHouseId +
                       ",@TenantId=" + obj.TenantId + ",@TransporterCompanyId=" + obj.FrieghtCompanyid + ",@InDriverName=" +DB.SQuote(obj.InDriverName) + ",@InDriverContactNumber=" +DB.SQuote(obj.InDriverNo)+
                       ",@GateEntryTypeId="+obj.GateEntryType+ ",@ContainerSeal="+DB.SQuote(obj.ContainerSeal)+ 
                       ",@ArrivingFrom_CountryMasterID="+obj.ArrivingFromId+ ",@GateStatusId="+ Status + ",@ArrivingFrom_stateMasterID=" + obj.ArrivingStateId+
                       ",@ArrivingFrom_CityMasterID=" + obj.ArrivingCityId + ",@unloadedqty="+ unloadedqty + ",@IshaveReturnLoad=" + Convert.ToInt32(obj.IsRetutnLoad));
                return res;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        [WebMethod]
        public static GateEntryHeader getGateEntryDetails(int gateid)
        {
            GateEntryHeader obj = new GateEntryHeader();
            DataSet ds = DB.GetDS("[SP_GetGateEntryDetails] @GateId="+ gateid, false);
            DataTable dtPrimaryInfo = ds.Tables[0];
            try
            {
                if (dtPrimaryInfo.Rows.Count > 0)
                {
                    obj.GateEntryId=Convert.ToInt32(dtPrimaryInfo.Rows[0]["YM_TRN_VehicleYardAvailability_ID"].ToString());
                    obj.StatusId = Convert.ToInt32(dtPrimaryInfo.Rows[0]["StatusId"].ToString());
                    obj.Status = dtPrimaryInfo.Rows[0]["Status"].ToString();
                    obj.VehicleId = dtPrimaryInfo.Rows[0]["YM_MST_Vehicle_ID"].ToString();
                    obj.Vehicle = dtPrimaryInfo.Rows[0]["RegistrationNumber"].ToString();
                    obj.PermitInfo = dtPrimaryInfo.Rows[0]["storagevol"].ToString();
                    obj.CapacityInfo = dtPrimaryInfo.Rows[0]["MaxStorageWeight"].ToString();
                    //obj.TenantId = dtPrimaryInfo.Rows[0]["joborderrefno"].ToString();
                    obj.TenantId = dtPrimaryInfo.Rows[0]["TenantID"].ToString();
                    obj.Tenant = dtPrimaryInfo.Rows[0]["TenantCode"].ToString();
                    obj.WareHouseId = dtPrimaryInfo.Rows[0]["WarehouseID"].ToString();
                    obj.WareHouse = dtPrimaryInfo.Rows[0]["WHName"].ToString();
                    obj.FrieghtCompanyid = dtPrimaryInfo.Rows[0]["FreightCompanyId"].ToString();
                    obj.FrieghtCompany = dtPrimaryInfo.Rows[0]["FreightCompany"].ToString();
                    obj.InDriverName = dtPrimaryInfo.Rows[0]["DriverName"].ToString();
                    obj.InDriverNo = dtPrimaryInfo.Rows[0]["DriverContactNumber"].ToString();
                    obj.GateEntryType = dtPrimaryInfo.Rows[0]["YM_MST_ShipmentType_ID"].ToString();
                    //obj.InboundId = dtPrimaryInfo.Rows[0]["InboundID"].ToString();
                    //obj.InboundNo = dtPrimaryInfo.Rows[0]["StoreRefNo"].ToString();
                    obj.ContainerSeal = dtPrimaryInfo.Rows[0]["ContainerSeal"].ToString();
                    //obj.LRNO = dtPrimaryInfo.Rows[0]["LR"].ToString();
                   // obj.BOXQty = dtPrimaryInfo.Rows[0]["BOXQty"].ToString();
                   // obj.SKUQty = dtPrimaryInfo.Rows[0]["SKUQty"].ToString();
                    obj.ArrivingFromId = dtPrimaryInfo.Rows[0]["ShipmentCountryMasterID"].ToString();
                    obj.CountryArrivingFrom = dtPrimaryInfo.Rows[0]["CountryName"].ToString();
                    obj.ArrivingStateId = dtPrimaryInfo.Rows[0]["StateMasterID"].ToString();
                    obj.ArrivingState = dtPrimaryInfo.Rows[0]["StateName"].ToString();
                    obj.ArrivingCityId = dtPrimaryInfo.Rows[0]["CityMasterID"].ToString();
                    obj.ArrivingCity = dtPrimaryInfo.Rows[0]["CityName"].ToString();
                    //obj.IsPreferedLocationId = dtPrimaryInfo.Rows[0]["PreferredDestination_CityMasterID"].ToString();
                   // obj.PreferedCity = dtPrimaryInfo.Rows[0]["CityName"].ToString();
                   // obj.Dock = dtPrimaryInfo.Rows[0]["dockname"].ToString();
                   // obj.DockID = dtPrimaryInfo.Rows[0]["DockID"].ToString();
                    //obj.EstIBDockAssignTime = dtPrimaryInfo.Rows[0]["EstIBDockAssignTimestamp"].ToString();
                    //obj.EstIBOperationTime = dtPrimaryInfo.Rows[0]["EstIBOpsTime"].ToString();
                    obj.IsRetutnLoad =Convert.ToBoolean( dtPrimaryInfo.Rows[0]["IsHaveReturnLoad"]);
                    obj.UnloadedQty= dtPrimaryInfo.Rows[0]["UnloadedQty"].ToString();
                    obj.OutdateTime= dtPrimaryInfo.Rows[0]["GateOutTime"].ToString();
                    obj.IscreatedFromInbound =Convert.ToInt32(dtPrimaryInfo.Rows[0]["IscreatedFromInbound"]);

                }
            }
            catch (Exception e)
            {
            }
            return obj;
        }
        [WebMethod]
        public static int UpsertGateEntryPreferreddLocation(PreferedLocation obj, string GID)
        {
            try
            {
                int PreferredId= DB.GetSqlN("[dbo].[SP_UpsertLocationPreferenceDetails] @PreferredLocId="+ obj.PreferedLocationId+ ",@CountryId=" + obj.CountryID+ ",@StateId="+ obj.StateID+ ",@LocationId="+obj.CityID+ ",@GateId="+GID+ ",@Userid="+cp1.UserID);
                return PreferredId;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        [WebMethod]
        public static List<PreferedLocation> GetGateEntryPreferreddLocation( string GID)
        {
            List<PreferedLocation> lst = new List<PreferedLocation>();
         
            try
            {
                DataSet ds = DB.GetDS("[SP_GetPreferredLocation] @GateId=" + GID, false);


                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    lst.Add(new PreferedLocation()
                    {
                        PreferedLocationId= (row["YM_TRN_VehicleDestPreference_ID"]).ToString(),
                        GateId = (row["YM_TRN_VEhicleYardAvailability_ID"]).ToString(),
                        State = (row["state"]).ToString(),
                        StateID = (row["PreferredState_ID"]).ToString(),
                        Country = (row["Country"]).ToString(),
                        CountryID = (row["PreferredCountry_ID"]).ToString(),
                        City = (row["city"]).ToString(),
                        CityID = (row["PreferredCity_ID"]).ToString()

                    });
                }

            }
            catch (Exception e)
            {
                return null;
            }
            return lst;
        }
        [WebMethod]
        public static List<DocumentType> GetGateEntryDocuments(string GID)
        {
            List<DocumentType> lst = new List<DocumentType>();

            try
            {
                DataSet ds = DB.GetDS("[SP_GetDocumentDataForGateEntry] @GateId=" + GID, false);


                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    lst.Add(new DocumentType()
                    {
                        ShipmentDocumentID =Convert.ToInt32(row["YM_TRN_ShipmentDocument_ID"]),
                        DocumentTypeID =Convert.ToInt32(row["YM_MST_DocumentType_ID"]),
                        
                        DocumentName = (row["DocumentName"]).ToString(),
                        DocumentURI = (row["DocumentURI"]).ToString(),
                        Document_Type= (row["DocumentType"]).ToString(),


                    });
                }

            }
            catch (Exception e)
            {
                return null;
            }
            return lst;
        }

        [WebMethod]
        public static int DeleteGateEntryPreferreddLocation(PreferedLocation obj,string GID)
        {
            try
            {
                 DB.GetSqlN("delete from YM_TRN_VehicleDestPreferences where YM_TRN_VehicleDestPreference_ID="+ obj.PreferedLocationId);
                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        [WebMethod]
        public static int UpsertGateOutInfo(string outtime, string GID)
        {
            try
            {
                if (outtime != "")
                {
                    outtime = DateTime.ParseExact(outtime, "dd-MMM-yyyy HH:mm", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm");
                }
                DB.GetSqlN("update YM_TRN_VehicleYardAvailability set YM_TRN_VehicleYardAvailability_statusId=8,GateOutTime="+DB.SQuote(outtime )+ " where YM_TRN_VehicleYardAvailability_ID=" + GID);
                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        [WebMethod]
        public static string CalculateExpectedOutTime(string time,int minutes)
        {
            try
            {
                DateTime outtime = DateTime.ParseExact(time, "dd-MMM-yyyy HH:mm", CultureInfo.InvariantCulture);
                DateTime x30MinsLater = outtime.AddMinutes(minutes);
                return x30MinsLater.ToString();
            }
            catch (Exception e)
            {
                return "";
            }
        }
        [WebMethod]
        public static int DeleteGateEntryDocument(DocumentType obj, string GID)
        {
            try
            {
                DB.GetSqlN("[SP_DeleteDocumentDataFromGateentry] @DocumentShipmentid=" + obj.ShipmentDocumentID + ",@gateId=" + GID);
                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        [WebMethod]
        public static int UploadDocuments(DocumentType obj, string GID)
        {
            try
            {
                
               int result= DB.GetSqlN("[SP_UpsertDocumentDetails] @YM_TRN_ShipmentDocument_ID="+obj.ShipmentDocumentID+",@GateId=" + GID+ ",@YM_MST_DocumentType_ID="+obj.DocumentTypeID+ ",@DocumentName="+DB.SQuote(obj.DocumentName)+ ",@DocumentURI="+DB.SQuote(obj.DocumentURI.Split(',')[0])+ ",@userid="+cp1.UserID);
                return result;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        [WebMethod]
        public static int DeleteGateEntryDock(GateEntryDocks obj, string GID)
        {
            try
            {
                DB.GetSqlN("delete from YM_TRN_VehicleDocking where YM_TRN_VehicleDocking_ID=" + obj.VehicleDocking_ID);
                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        [WebMethod]
        public static int UpsertGateEntryDock(GateEntryDocks obj, string GID)
        {
            try
            {
                if (obj.ESTDockInTime != "")
                {
                    obj.ESTDockInTime = DateTime.ParseExact(obj.ESTDockInTime, "dd-MMM-yyyy HH:mm", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm");
                }
                if(obj.SKUQty=="" || obj.SKUQty==null)
                {
                    obj.SKUQty = "0";
                }
                if (obj.BOXQty == "" || obj.BOXQty == null)
                {
                    obj.BOXQty = "0";
                }
                 if (obj.ShipmentId == "" || obj.ShipmentId == null)
                {
                    obj.ShipmentId = "0";
                }
               
                return DB.GetSqlN("[dbo].[SP_UpsertDocDetailsForGateEntry] @YM_TRN_VehicleDestPreference_ID=" + obj.VehicleDocking_ID + ",@DockId=" + obj.DockID + ",@GateID=" + GID +
               ", @ShipmentId=" + obj.ShipmentId + ",@LRNumber=" + DB.SQuote(obj.LR) + ",@EstDockintime=" + DB.SQuote(obj.ESTDockInTime) + ",@EstOperationtime=" + obj.ESTOperationTime +
               ",@BoxQty=" + obj.BOXQty + ",@SKUQty=" + obj.SKUQty + ",@userid=" + cp1.UserID);
            }
            catch (Exception e)
            {
                return 0;
            }

           

        }
        [WebMethod]
        public static List<GateEntryDocks> GetGateEntryDocks(string GID)
        {
            List<GateEntryDocks> lst = new List<GateEntryDocks>();

            try
            {
                DataSet ds = DB.GetDS("[SP_GettingDockingDetailsForGateEntry] @GateId=" + GID, false);


                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    lst.Add(new GateEntryDocks()
                    {
                        VehicleDocking_ID = (row["YM_TRN_VehicleDocking_ID"]).ToString(),
                        GateId = (row["YM_TRN_VehicleYardAvailability_ID"]).ToString(),
                        DockID = (row["DockId"]).ToString(),
                        Dock = (row["DockName"]).ToString(),
                        LR = (row["LRNumber"]).ToString(),
                        ESTDockInTime = (row["EstDockInTime"]).ToString(),
                        ESTOperationTime = (row["EstOpsTime"]).ToString(),
                        ESTDockoutTime = (row["EstDockOutTime"]).ToString(),
                        SKUQty = Convert.ToInt32(row["SKUQty"])==0?"": (row["SKUQty"]).ToString(),
                        BOXQty =Convert.ToInt32(row["BOXQty"])==0? "" : (row["BOXQty"]).ToString(),
                        Actualdockintime = (row["ActualDockInTime"]).ToString(),
                        Actualoperationtime = (row["ActualOpsTime"]).ToString(),
                        Actualdockouttime = (row["ActualDockOutTime"]).ToString(),
                        ShipmentId = (row["shipmentid"]).ToString(),
                        ShipmentNo = (row["shipmentNo"]).ToString()

                    });
                }

            }
            catch (Exception e)
            {
                return null;
            }
            return lst;
        }

        [WebMethod]
        public static int UpsertUnloadedData(string GID,string qty)
        {
            try
            {
                //DB.ExecuteSQL("update YM_TRN_VehicleYardAvailability set UnloadedQty="+ qty + ",YM_TRN_VehicleYardAvailability_statusId=5 where YM_TRN_VehicleYardAvailability_ID="+ GID);
                DB.ExecuteSQL("exec [SP_UpdateUnloadingDetails] @GateId=" + GID + ",@UnloadedQty=" + qty+ ",@userid="+cp1.UserID);
                return 1;
            }
            catch(Exception ex)
            {
                return 0;

            }
        }
        public class GateEntryHeader
        {
            public string OutdateTime { get; set; }
            public int StatusId { get; set; }
            public string Status { get; set; }
            public int SNO { get; set; }
            public string ArrivingCityId { get; set; }
            public string ArrivingCity { get; set; }
            public string ArrivingStateId { get; set; }
            public string ArrivingState { get; set; }
            public string UnloadedQty { get; set; }
            public string DockID { get; set; }
            public string Dock { get; set; }
            public int GateEntryId { get; set; }
            public string PreferedCity { get; set; }
            public string IsPreferedLocationId { get; set; }
            public Boolean IsRetutnLoad { get; set; }
            public string ArrivingFromId { get; set; }
            public string BOXQty { get; set; }
            public string SKUQty { get; set; }
            public string LRNO { get; set; }
            public string ContainerSeal { get; set; }
            public string CapacityInfo { get; set; }
            public string PermitInfo { get; set; }
            public string FrieghtCompanyid { get; set; }
            public string FrieghtCompany { get; set; }
            public string AccountId { get; set; }
            public string TenantId { get; set; }
            public string GateEntryType { get; set; }
            public string Vehicle { get; set; }
            public string Tenant { get; set; }
            public string VehicleId { get; set; }
            public string WareHouseId {get;set;}
            public string WareHouse { get; set; }
            public string InTime { get; set; }
            public string EstmatedInTime { get; set; }
            public string InDriverName { get; set; }
            public string InDriverNo { get; set; }
            public string InboundNo { get; set; }
            public string InboundId { get; set; }
            public string EstIBDockAssignTime { get; set; }
            public string CountryArrivingFrom { get; set; }
            public string CityArrivingFrom { get; set; }
            public string EstIBOperationTime { get; set; }
            public string IBDockAssignTime { get; set; }
            public string OBOperationTime { get; set; }
            public string IBDockOperationTime { get; set; }
            public string PermittedWaitTime { get; set; }
            public string OutDriver { get; set; }
            public string OutBoundNo { get; set; }
            public string OutBoundId { get; set; }
            public string EstOBDockAssignTime { get; set; }
            public string OBDockAssignTime { get; set; }
            public string LeavingToCountry { get; set; }
            public string LeavingToCity { get; set; }
            public string OBDockOperationTime { get; set; }
            public string OutTimeStamp { get; set; }
            public string PreferedDestinationCountry { get; set; }
            public string UnloadingTime { get; set; }
            public string ReservedForOutbound { get; set; }
            public string GateEntryCheck { get; set; }
            public string CreatedDate { get; set; }
            public int IscreatedFromInbound { get; set; }


        }
        public class PreferedLocation
        {
            public string PreferedLocationId { get; set; }
            public string GateId { get; set; }
            public string StateID { get; set; }
            public string State { get; set; }
            public string CountryID { get; set; }
            public string Country { get; set; }
            public string CityID { get; set; }
            public string City { get; set; }
        }
        public class DocumentType
        {
            public int ShipmentDocumentID { get; set; }
            public string GateId { get; set; }
            public int MIMEID { get; set; }
            public int DocumentTypeID { get; set; }
            public string DocumentName { get; set; }
            public string DocumentURI { get; set; }
            public string Document_Type { get; set; }



        }
        public class GateEntryDocks
        {
            public string VehicleDocking_ID { get; set; }
            public string GateId { get; set; }
            public string DockID { get; set; }
            public string Dock { get; set; }
            public string LR { get; set; }
            public string ESTDockInTime { get; set; }
            public string ESTOperationTime { get; set; }
            public string ESTDockoutTime { get; set; }

            public string SKUQty { get; set; }
            public string BOXQty { get; set; }
            public string Actualdockintime { get; set; }

            public string Actualoperationtime { get; set; }
            public string Actualdockouttime { get; set; }
            public string ShipmentId { get; set; }
             public string ShipmentNo { get; set; }
            public string SupplierInvoiceId { get; set; }
            public string InvoiceNumber { get; set; }
            public string SupplierId { get; set; }
            public string Supplier { get; set; }
        }
            
    }
}