using MRLWMSC21Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MRLWMSC21.mReports
{
    public class GetDataListBL
    {
        CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        public List<MRLWMSC21.mReports.GetDataListModel.Zones> GetZonesList()
        {
            List<MRLWMSC21.mReports.GetDataListModel.Zones> lstZones = new List<MRLWMSC21.mReports.GetDataListModel.Zones>();
           //string Query = "SELECT LocationZoneID,LocationZoneCode FROM INV_LocationZone where IsDeleted=0 and IsActive=1";
            string Query = "SELECT DISTINCT LocationZoneID, LocationZoneCode FROM INV_LocationZone ILZ JOIN GEN_Warehouse GW ON GW.WarehouseID = ILZ.WarehouseID JOIN TPL_Tenant TNT ON TNT.AccountID = GW.AccountID where ILZ.IsDeleted = 0 and ILZ.IsActive = 1 AND GW.AccountID = case when 0 = " + cp.AccountID.ToString() + " then TNT.AccountID else " + cp.AccountID.ToString() + " end";
            DataSet DS = DB.GetDS(Query, false);
            foreach (DataRow row in DS.Tables[0].Rows)
            {
                lstZones.Add(new MRLWMSC21.mReports.GetDataListModel.Zones()
                {
                    LocationZoneID = Convert.ToInt32(row["LocationZoneID"].ToString()),
                    LocationZoneCode = (row["LocationZoneCode"].ToString()),
                });
            }
            return lstZones;

        }


        public List<MRLWMSC21.mReports.GetDataListModel.DocumentType> GetDocumentType()
        {
            List<MRLWMSC21.mReports.GetDataListModel.DocumentType> lstDocumenttype = new List<MRLWMSC21.mReports.GetDataListModel.DocumentType>();
            string Query = "Select DocumentTypeID, DocumentType from GEN_DocumentType Where IsActive =1";
            DataSet DS = DB.GetDS(Query, false);
            foreach (DataRow row in DS.Tables[0].Rows)
            {
                lstDocumenttype.Add(new MRLWMSC21.mReports.GetDataListModel.DocumentType()
                {
                    DocumentTypeID = Convert.ToInt32(row["DocumentTypeID"].ToString()),
                    Documenttype = (row["DocumentType"].ToString()),
                });
            }
            return lstDocumenttype;

        }


        public List<MRLWMSC21.mReports.GetDataListModel.DeliveryStatus> GetDeliverystatus()
        {
            List<MRLWMSC21.mReports.GetDataListModel.DeliveryStatus> lstDeliverystatus = new List<MRLWMSC21.mReports.GetDataListModel.DeliveryStatus>();
            // string Query = "Select DeliveryStatusID, DeliveryStatus from OBD_DeliveryStatus where IsActive=1 AND DeliveryStatusID NOT IN (3,8,9,10,11)";
            string Query = "Select DeliveryStatusID, DeliveryStatus from OBD_DeliveryStatus where IsActive=1 AND DeliveryStatusID NOT IN (3,10,11,8,9,6)";
            DataSet DS = DB.GetDS(Query, false);
            foreach (DataRow row in DS.Tables[0].Rows)
            {
                lstDeliverystatus.Add(new MRLWMSC21.mReports.GetDataListModel.DeliveryStatus()
                {
                    DeliveryStatusID = Convert.ToInt32(row["DeliveryStatusID"].ToString()),
                    Deliverystatus = (row["DeliveryStatus"].ToString()),
                });
            }
            return lstDeliverystatus;

        }



        public List<MRLWMSC21.mReports.GetDataListModel.Warehouse> Getwarehouse()
        {
            List<MRLWMSC21.mReports.GetDataListModel.Warehouse> lstDeliverystatus = new List<MRLWMSC21.mReports.GetDataListModel.Warehouse>();
            string Query = "SELECT case when isnull(Location,'')!='' then WHCode+'-'+Location else WHCode end  AS WHCode, WarehouseID  FROM GEN_Warehouse where IsDeleted=0 and IsActive=1 and accountid=" + cp.AccountID;
            DataSet DS = DB.GetDS(Query, false);
            foreach (DataRow row in DS.Tables[0].Rows)
            {
                lstDeliverystatus.Add(new MRLWMSC21.mReports.GetDataListModel.Warehouse()
                {
                    WarehouseID = Convert.ToInt32(row["WarehouseID"].ToString()),
                    WHCode = (row["WHCode"].ToString()),
                });
            }
            return lstDeliverystatus;

        }
        public List<MRLWMSC21.mReports.GetDataListModel.Warehouse> Getwarehouse1(string TenantID)
        {
            List<MRLWMSC21.mReports.GetDataListModel.Warehouse> lstDeliverystatus = new List<MRLWMSC21.mReports.GetDataListModel.Warehouse>();
            //  string Query = "SELECT case when isnull(Location,'')!='' then WHCode+'-'+Location else WHCode end  AS WHCode, WarehouseID  FROM GEN_Warehouse where IsDeleted=0 and IsActive=1 and accountid=" + cp.AccountID;
            string Query = "SELECT DISTINCT TC.WarehouseID,WH.WHCode,TC.TenantID FROM TPL_Tenant_Contract AS TC JOIN  GEN_Warehouse AS WH ON TC.WarehouseID = WH.WarehouseID AND TC.IsActive=1 AND TC.IsDeleted=0 AND WH.IsActive=1 AND WH.IsDeleted=0 AND FORMAT(GETDATE(),'yyyy-MM-dd') BETWEEN FORMAT(TC.EffectiveFrom,'yyyy-MM-dd') AND  FORMAT(TC.EffectiveTo,'yyyy-MM-dd') AND (0=" + TenantID+" OR TC.TenantID =" + TenantID+")";
            DataSet DS = DB.GetDS(Query, false);
            foreach (DataRow row in DS.Tables[0].Rows)
            {
                lstDeliverystatus.Add(new MRLWMSC21.mReports.GetDataListModel.Warehouse()
                {
                    WarehouseID = Convert.ToInt32(row["WarehouseID"].ToString()),
                    WHCode = (row["WHCode"].ToString()),
                });
            }
            return lstDeliverystatus;

        }
        public List<MRLWMSC21.mReports.GetDataListModel.Printers> GetPrinter()
        {
            List<MRLWMSC21.mReports.GetDataListModel.Printers> lstPrinter = new List<GetDataListModel.Printers>();
            string Query = "select CR.ClientResourceName,CR.DeviceIP from GEN_ClientResource CR JOIN  GEN_DeviceModel DM ON DM.DeviceModelID=CR.DeviceModelID AND DM.IsActive=1 AND DM.IsDeleted=0 JOIN GEN_DeviceType DT ON DT.DeviceTypeID=DM.DeviceTypeID AND DT.IsActive=1 AND DT.IsDeleted=0 where CR.IsActive=1 AND CR.IsDeleted=0 AND DM.DeviceTypeID=3";
            DataSet DS = DB.GetDS(Query, false);
            foreach (DataRow row in DS.Tables[0].Rows)
            {
                lstPrinter.Add(new MRLWMSC21.mReports.GetDataListModel.Printers()
                {
                    PrinterId=row["DeviceIP"].ToString(),
                    PrinterName=row["ClientResourceName"].ToString(),

                });
            }

            return lstPrinter;
        }
        
        public List<MRLWMSC21.mReports.GetDataListModel.LabelSize> Getlabel()
        {
            List<MRLWMSC21.mReports.GetDataListModel.LabelSize> lstLabel = new List<GetDataListModel.LabelSize>();
            string Query = "select TenantBarcodeTypeID,LabelType+'-'+convert(varchar(50),[Length])+' * '+convert(varchar(50),[Width]) LB from TPL_Tenant_BarcodeType where IsActive=1";
            DataSet ds = DB.GetDS(Query, false);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                lstLabel.Add(new MRLWMSC21.mReports.GetDataListModel.LabelSize()
                {
                    BarCodeId=Convert.ToInt32(row["TenantBarcodeTypeID"].ToString()),
                    BarCode=row["LB"].ToString(),
                });
            }
            return lstLabel;
        }

        public List<MRLWMSC21.mReports.GetDataListModel.Tenant> GetTenant(string TenantName)
        {
            List<MRLWMSC21.mReports.GetDataListModel.Tenant> lstLabel = new List<GetDataListModel.Tenant>();
            
            //string Query = "SELECT TOP 10 TenantID,TenantName FROM [TPL_Tenant] where  TenantID<>0 AND IsDeleted=0 and AccountID="+cp.AccountID+" AND IsActive=1 and TenantName like '" + TenantName + "%' order by TenantName";
            string Query = "SELECT TOP 10 TenantID,TenantName FROM TPL_Tenant WHERE IsActive = 1 AND IsDeleted = 0 AND TenantID<>0 AND TenantName like '%" + TenantName + "%' AND AccountID = case when 0 = " + cp.AccountID.ToString() + " then AccountID else " + cp.AccountID.ToString() + " end" + " AND TenantID = case when  " + cp.TenantID.ToString() + "=0  then TenantID else " + cp.TenantID.ToString() + " end  order by TenantName";
            DataSet ds = DB.GetDS(Query, false);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                lstLabel.Add(new MRLWMSC21.mReports.GetDataListModel.Tenant()
                {
                    TenantId = Convert.ToInt32(row["TenantID"].ToString()),
                    TenantName = row["TenantName"].ToString(),
                });
            }
            return lstLabel;
        }

        public List<MRLWMSC21.mReports.GetDataListModel.Mcodes> GetMcode(string Mcode ,string TenantId)
        {
            List<MRLWMSC21.mReports.GetDataListModel.Mcodes> lstLabel = new List<GetDataListModel.Mcodes>();

            if (TenantId == "")
                TenantId = "0";

            //Account and Tenant filteration


            //  string Query = "select TOP 20 MMT.MCode,MMT.MaterialMasterID from MMT_MaterialMaster MMT LEFT JOIN TPL_Tenant_MaterialMaster TMM ON MMT.MaterialMasterID = TMM.MaterialMasterID WHERE MMT.IsActive = 1 AND MMT.IsDeleted = 0   AND MCode like '" + Mcode + "%' AND  TMM.TenantID="+TenantId;
            string Query = "select TOP 20 MMT.MCode,MMT.MaterialMasterID from MMT_MaterialMaster MMT LEFT JOIN TPL_Tenant_MaterialMaster TMM ON MMT.MaterialMasterID = TMM.MaterialMasterID LEFT JOIN TPL_Tenant TNT ON TNT.TenantID=TMM.TenantID WHERE MMT.IsActive = 1 AND MMT.IsDeleted = 0   AND MCode like '" + Mcode + "%' AND  TMM.TenantID=" + TenantId + " AND TNT.AccountID = case when 0 =" + cp.AccountID.ToString() + " then TNT.AccountID else " + cp.AccountID.ToString() + " end ";
   
            DataSet ds = DB.GetDS(Query, false);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                lstLabel.Add(new MRLWMSC21.mReports.GetDataListModel.Mcodes()
                {
                    McodeId = Convert.ToInt32(row["MaterialMasterID"].ToString()),
                    Mcode = row["MCode"].ToString(),
                });
            }
            return lstLabel;
        }

        public List<MRLWMSC21.mReports.GetDataListModel.MType> GetMType(string Mtype)
        {
            List<MRLWMSC21.mReports.GetDataListModel.MType> lstLabel = new List<GetDataListModel.MType>();
            string Query = "select top 20 MTypeID,MType,Description from MMT_MType where IsActive=1   AND  MType like '" + Mtype + "%'";
            DataSet ds = DB.GetDS(Query, false);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                lstLabel.Add(new MRLWMSC21.mReports.GetDataListModel.MType()
                {
                    MtypeId = Convert.ToInt32(row["MTypeID"].ToString()),
                    Mtype = row["MType"].ToString(),
                });
            }
            return lstLabel;
        }

        public List<MRLWMSC21.mReports.GetDataListModel.Locations> GetLoc(string loc)
        {
            List<MRLWMSC21.mReports.GetDataListModel.Locations> lstLabel = new List<GetDataListModel.Locations>();
            // string Query = "SELECT  Location,LocationID FROM INV_Location WHERE IsActive=1 AND IsDeleted=0 AND Location LIKE '" + loc + "%'";
            //string Query = "SELECT  Location,LocationID FROM INV_Location WHERE IsActive=1 AND IsDeleted=0 AND Location LIKE '" + loc + "%' AND TenantID = case when 0 =" + cp.TenantID.ToString() + " then TenantID else " + cp.TenantID.ToString() + " end";
            string Query = "select DISTINCT  LOC.LocationID AS LocationID,LOC.Location AS Location FROM INV_Location LOC INNER JOIN INV_LocationZone ZONE ON LOC.ZoneId = ZONE.LocationZoneID INNER JOIN GEN_Warehouse WARE ON ZONE.WarehouseID = WARE.WarehouseID WHERE LOC.IsActive = 1 AND WARE.AccountID=" + cp.AccountID+" AND LOC.IsDeleted = 0 AND LOC.Location LIKE '" + loc + "%'" ;
            DataSet ds = DB.GetDS(Query, false);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                lstLabel.Add(new MRLWMSC21.mReports.GetDataListModel.Locations()
                {
                    LocId = Convert.ToInt32(row["LocationID"].ToString()),
                    LocationName = row["Location"].ToString(),
                });
            }
            return lstLabel;
        }

        public List<MRLWMSC21.mReports.GetDataListModel.KitPlanner> GetKit(string kit)
        {
            List<MRLWMSC21.mReports.GetDataListModel.KitPlanner> lstLabel = new List<GetDataListModel.KitPlanner>();
            string Query = "select * from MMT_KitPlanner KP JOIN TPL_Tenant TEN ON TEN.TenantID = KP.TenantID JOIN GEN_Account ACC ON ACC.AccountID = TEN.AccountID where KP.IsActive = 1 and KP.IsDeleted = 0  AND ACC.AccountID ="+cp.AccountID+" AND KitCode LIKE '" + kit + "%' AND KP.TenantID = case when 0 =" + cp.TenantID.ToString() + " then KP.TenantID else " + cp.TenantID.ToString() + " end";
            DataSet ds = DB.GetDS(Query, false);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                lstLabel.Add(new MRLWMSC21.mReports.GetDataListModel.KitPlanner()
                {
                    KitId=Convert.ToInt32(row["KitPlannerID"].ToString()),
                    KitCode = row["KitCode"].ToString(),
                   
                });
            }
            return lstLabel;
        }
    }
}