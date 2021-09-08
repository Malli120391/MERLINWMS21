using MRLWMSC21Common;
using MRLWMSC21.mMaterialManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using static MRLWMSC21.mInbound.GateEntry;

namespace MRLWMSC21.mInbound
{
    public partial class GateEntryList : System.Web.UI.Page
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
                DesignLogic.SetInnerPageSubHeading(this.Page, "Gate Entry List");
            }
        }
        [WebMethod]
        public static int GetCurrentAccount()
        {
            return cp1.AccountID;
        }
        [WebMethod]
        public static List<GateEntryHeader> getGateEntryListData(GateSearch obj)
        {
           List<GateEntryHeader> lst = new List<GateEntryHeader>();
            DataSet ds = DB.GetDS("[SP_GetGateEntryDetails] @GateId=" + 0+ ",@AccountId="+ cp1.AccountID+ ",@StatusId="+obj.StatusId+ ",@VehicleID="+obj.VehicleId, false);


            try
            {
                int SNO = 0;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    lst.Add(new GateEntryHeader()
                    {
                                    SNO=++SNO,
                                    GateEntryId = Convert.ToInt32(row["YM_TRN_VehicleYardAvailability_ID"].ToString()),
                                    VehicleId = row["YM_MST_Vehicle_ID"].ToString(),
                                    Vehicle = row["RegistrationNumber"].ToString(),
                                    //TenantId = row["joborderrefno"].ToString(),
                                    TenantId = row["TenantID"].ToString(),
                                    Tenant = row["TenantCode"].ToString(),
                                    WareHouseId = row["WarehouseID"].ToString(),
                                    WareHouse = row["WHName"].ToString(),
                                    FrieghtCompanyid = row["FreightCompanyId"].ToString(),
                                    FrieghtCompany = row["FreightCompany"].ToString(),
                                    InDriverName = row["DriverName"].ToString(),
                                    InDriverNo = row["DriverContactNumber"].ToString(),
                                    GateEntryType = row["YM_MST_ShipmentType_ID"].ToString(),
                                    //InboundId = row["InboundID"].ToString(),
                                    //InboundNo = row["StoreRefNo"].ToString(),
                                    ContainerSeal = row["ContainerSeal"].ToString(),
                                    //LRNO = row["LR"].ToString(),
                                    // BOXQty = row["BOXQty"].ToString(),
                                    // SKUQty = row["SKUQty"].ToString(),
                                    ArrivingFromId = row["ShipmentCountryMasterID"].ToString(),
                                    CountryArrivingFrom = row["CountryName"].ToString(),
                                    ArrivingStateId = row["StateMasterID"].ToString(),
                                    ArrivingState = row["StateName"].ToString(),
                                    ArrivingCityId = row["CityMasterID"].ToString(),
                                    ArrivingCity = row["CityName"].ToString(),
                                    //IsPreferedLocationId = row["PreferredDestination_CityMasterID"].ToString(),
                                    // PreferedCity = row["CityName"].ToString(),
                                    // Dock = row["dockname"].ToString(),
                                    // DockID = row["DockID"].ToString(),
                                    //EstIBDockAssignTime = row["EstIBDockAssignTimestamp"].ToString(),
                                    //EstIBOperationTime = row["EstIBOpsTime"].ToString(),
                                    IsRetutnLoad = Convert.ToBoolean(row["IsHaveReturnLoad"]),
                                    GateEntryCheck= row["GateCheck"].ToString(),
                                    Status= row["Status"].ToString(),
                                     CreatedDate = row["createddate"].ToString()

                    });
                 }
            }
            catch (Exception e)
            {
            }
            return lst;
        }
        [WebMethod]
        public static int UpdateGateEntryData(int GateId, int IsgateIn)
        {
            try
            {
                DB.ExecuteSQL("[SP_Dock_InOut_Time] @GateId=" + GateId + ",@IsDockIn=" + IsgateIn+ ",@userid="+cp1.UserID);
                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }
           
        }
        [WebMethod]
        public static List<DropDownData> GetGateEntryStatus()
        {
            List<DropDownData> otenantlst = new List<DropDownData>();
            DataSet DS = DB.GetDS("select YM_MST_VehicleYardStatus_ID,status from YM_MST_VehicleYardStatus where Isdeleted=0 and IsActive=1 ", false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    otenantlst.Add(new DropDownData()
                    {
                        ID = Convert.ToInt32(row["YM_MST_VehicleYardStatus_ID"]),
                        Value = row["status"].ToString(),

                    });

                }
            }
            return otenantlst;
        }
     }
    public class GateSearch
    {
        public int StatusId { set; get; }
        public string VehicleId { set; get; }
    }
}