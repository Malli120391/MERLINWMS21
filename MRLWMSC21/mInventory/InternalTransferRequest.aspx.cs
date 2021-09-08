using MRLWMSC21Common;
using MRLWMSC21.mOutbound.BL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRLWMSC21.mInventory
{
    public partial class InternalTransferRequest : System.Web.UI.Page
    {
        public static CustomPrincipal cp;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            Page.Theme = "Orders";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "");
            }
        }

        [WebMethod]
        public static List<DropDownListData> getTenantData()
        {
            try
            {
                OutBoundBL objbl = new OutBoundBL();
                return objbl.getTenantData();
            }
            catch
            {
                return null;
            }
        }

        // added by lalitha on 14/02/2019 for getting warehouse data
        [WebMethod]
        public static List<DropDownListDataFilter> GetWarehouse()
        {
            try
            {
                OutBoundBL objbl = new OutBoundBL();
                return objbl.getWareHouseDataData();
            }
            catch
            {
                return null;
            }
        }

        [WebMethod]
        public static int UpsertTrenaferRequest(int TenantID,int WarehouseID, int TransfertypeID, string remarks, int IsSuggestedReg)
        {
            try
            {

                int transferID = new InternalTransferRequest().UpsertTrasfer(TenantID, WarehouseID,TransfertypeID, remarks, IsSuggestedReg);
                return transferID;
            }
            catch
            {
                return 0;
            }
        }

        [WebMethod]
        public static int ItemWiseTransferInfo(int MaterialMasterID, int TRID, int TRDID, int ToLocationId, int FromLocID, decimal Qty, string BatchNo, int CartonID)
        {
            int result = 0;
            try
            {
                string Query = "EXEC [dbo].[INTERNALSTOCK_TRANSFER_ITEM] @TransferRequestID = "+TRID+ ", @TransferRequestDetailsID="+TRDID+ ", @MaterialMasterID="+MaterialMasterID+ ", @ToLocationID="+ToLocationId+ ", @FromLocationID="+FromLocID+ ", @Quantity="+Qty+ ", @BatchNo="+DB.SQuote(BatchNo)+ ", @CreatedBy="+cp.UserID+ ", @CartonID = "+CartonID+"";
                result = DB.GetSqlN(Query);
            }
            catch(Exception EX)
            {
                result = 0;
            }
            return result;
        }
        [WebMethod]
        public static int UpsertTransferRequestDetails (int TransferRequestID,int MaterialMasterID, int LocationID, string BatchNo, int cartonID, string Quantity, int FromSLID = 0, int ToSL = 0, int ToLocationID=0)
        {
            CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
            string Query = "EXEC [dbo].[UPSERT_TRANSFER_REQUEST_DETAILS] @TransferID = "+ TransferRequestID + ", @CartonID  ="+ cartonID + " , @LocationID = "+ LocationID + " ,@MaterialMaterID ="+ MaterialMasterID + " , @Quantity = "+ Quantity + ",@BatchNo = "+ DB.SQuote(BatchNo) + " ,@FromSL ="+ FromSLID + "  , @ToSL = "+ ToSL + ", @ToLocationID = " + ToLocationID + "";
            int result = DB.GetSqlN(Query);
            return result;
        }

        [WebMethod]
        public static string GetAvailableQty(string TENANTID,  string MaterialMasterID, string LocationID, string BatchNo, string cartonID,  string StorageLocationID)
        {
            CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
            if (cartonID == "" || cartonID == null)
            {
                cartonID = "0";
            }
            if(StorageLocationID== "" || StorageLocationID == null)
            {
                StorageLocationID = "0";
            }
            string Query = "EXEC [dbo].[SP_TRS_MATERIALQTY] @TENANTID = " + Convert.ToInt32(TENANTID) + ", @CARTIONID  =" +Convert.ToInt32(cartonID) + " , @LOCATIONID = " + LocationID + " ,@MATERIALMASTERID =" + MaterialMasterID + " ,@BATCHNO = " + DB.SQuote(BatchNo) + " , @STORAGELOCATIONID = " + StorageLocationID + "";
            DataSet ds= DB.GetDS(Query, false);

            string Qty = ds.Tables[0].Rows[0][0].ToString();
            return Qty;
            //return DB.GetSqlN(Query);
        }


        [WebMethod]
        public static List<TransferRequest> GetTransferInfo(int TransferID)
        {
            return new InternalTransferRequest().GetTRHeaderInfo(TransferID);
        }

        [WebMethod]
        public static List<TransferRequestDetails> GetTransferRequestDetails(int TransferID)
        {
            return new InternalTransferRequest().GetTRDetailsInfo(TransferID);
        }
        [WebMethod]
        public static List<DropDownList> GetItem(int tenatID, string Prefix)
        {
            return new InternalTransferRequest().GetItemList(tenatID, Prefix);
        }
        [WebMethod]
        public static List<DropDownList> Getlocation(int mid, string Prefix, int TenantID, int WHID)
        {
            return new InternalTransferRequest().Getlocationinfo(mid, Prefix, TenantID, WHID);
        }

        [WebMethod]
        public static List<DropDownList> GetCarton(int mid,string Prefix)
        {
            return new InternalTransferRequest().GetCartonInfo(mid,Prefix);
        }

        [WebMethod]
        public static List<DropDownList> Transfertype()
        {
            return new InternalTransferRequest().GetTransferType();
        }
        [WebMethod]
        public static List<DropDownList> Storagelocation()
        {
            return new InternalTransferRequest().GetStorageLocation();
        }

        [WebMethod]
        public static List<DropDownList> Toloccationlist(string Prefix, string TenantID, int WareHouseId)
        {
            return new InternalTransferRequest().GetToLocInfo(Prefix, TenantID, WareHouseId);
        }

        [WebMethod]
        public static List<DropDownList> Cartoninfo(string Prefix,string Location , string TenantID = "0")
        {
            return new InternalTransferRequest().GetCartonList(Prefix,Location, TenantID);
        }

        public int UpsertTrasfer(int TenantID,int WarehouseID, int TransfertypeID, string remarks, int IsSuggestedReg)
        {
            try
            {
                int TransferRequestID = DB.GetSqlN("EXEC [dbo].[UPSERT_TRANSFERREQUEST] @TenatID = "+TenantID+ ",@WarehouseId="+ WarehouseID + ", @TransferType = " + TransfertypeID+ ", @Remarks = "+DB.SQuote(remarks)+ ", @UserID = "+cp.UserID+ ", @IsSuggestedReg = "+IsSuggestedReg+"");
                return TransferRequestID;
            }
            catch(Exception ex)
            {
                return 0;
            }
        }

        public List<TransferRequest> GetTRHeaderInfo(int TransferID)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            List<TransferRequest> lst = new List<TransferRequest>();
            string Query = "EXEC [dbo].[GET_TRANSFERREQUEST_HEADER_INFO] @TransferReqID = "+ TransferID + "";
            DataSet DS = DB.GetDS(Query, false);

            if(DS.Tables[0].Rows.Count != 0)
            {
                foreach(DataRow row in DS.Tables[0].Rows)
                {
                    TransferRequest TR = new TransferRequest();
                    TR.TransferID = Convert.ToInt32(row["TransferRequestID"]);
                    TR.TransferNo = row["TransferRequestNumber"].ToString();
                    TR.TenantID = row["TenantID"].ToString() == "" ? 0 : Convert.ToInt32(row["TenantID"]);//Convert.ToInt32(row["TenantID"]);
                    TR.TenantName = row["TenantName"].ToString();
                    TR.Remarks  = row["Remarks"].ToString();
                    TR.TransferTypeID = Convert.ToInt32(row["TransferTypeID"]);
                    TR.IsSuggestedReq = Convert.ToUInt16(row["IsSuggestedReg"]);
                    TR.status = row["FulfillStatus"].ToString();
                    TR.WarehouseId = Convert.ToInt32(row["WarehouseId"]);
                    TR.StatusId = Convert.ToInt32(row["FulfillStatusID"]);
                    TR.WHcode = row["WHCode"].ToString();
                    lst.Add(TR);
                }
            }
            return lst;
        }
        public List<TransferRequestDetails> GetTRDetailsInfo(int TransferID)
        {
            List<TransferRequestDetails> lst = new List<TransferRequestDetails>();
            string Query = "EXEC [dbo].[GET_TRANSFER_REQUEST_DETAILS] @TransferRequestID = " + TransferID + "";
            DataSet DS = DB.GetDS(Query, false);
            cp = HttpContext.Current.User as CustomPrincipal;
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    TransferRequestDetails TR = new TransferRequestDetails();
                    TR.TransferID = Convert.ToInt32(row["TransferRequestID"]);
                    TR.TransferRequestDetailsID = Convert.ToInt32(row["TransferRequestDetailsID"]);
                    TR.MaterialMasterID = Convert.ToInt32(row["MaterialMasterID"].ToString());
                    TR.Mcode = (row["MCode"].ToString());
                    TR.BatchNo = row["BatchNo"].ToString();
                    TR.Quantiy = Convert.ToDecimal(row["Quantity"].ToString());
                    TR.LocationID = Convert.ToInt32(row["FromLocationID"]);
                    TR.Location = row["FromLoction"].ToString();
                    TR.FromSL = row["FromSL"].ToString() != "" ? row["FromSL"].ToString() : null;  
                    TR.FromSLID = row["FromStorageLocationID"].ToString().Trim() == "" ? 0 : Convert.ToInt32(row["FromStorageLocationID"]);  
                    TR.ToSL = row["TOSL"].ToString()!=""? row["TOSL"].ToString():null;
                    TR.ToSLID = row["ToStorageLocationID"].ToString().Trim()==""?0:Convert.ToInt32(row["ToStorageLocationID"]);
                    TR.ToLocationID = row["ToLocationID"].ToString().Trim() == "" ? 0 : Convert.ToInt32(row["ToLocationID"]);
                    TR.Tolocation = row["ToLocation"].ToString();
                    TR.IsTransferDone = Convert.ToInt32(row["IsTransfer"]);
                    lst.Add(TR);
                }
            }
            return lst;
        }

        public List<DropDownList> GetTransferType()
        {
            List<DropDownList> lst = new List<DropDownList>();

            string Query = "SELECT TransferType, TransferTypeID FROM INV_TransferType WHERE TransferTypeID IN (5)";
            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    DropDownList DDl = new DropDownList();
                    DDl.Id = Convert.ToInt32(row["TransferTypeID"]);
                    DDl.Name = row["TransferType"].ToString();
                    lst.Add(DDl);
                }

            }
            return lst;

        }

        public List<DropDownList> GetStorageLocation()
        {
            List<DropDownList> lst = new List<DropDownList>();

            string Query = "SELECT Code, Id FROM StorageLocation WHERE ID NOT IN (1,2,7)";
            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    DropDownList DDl = new DropDownList();
                    DDl.Id = Convert.ToInt32(row["Id"]);
                    DDl.Name = row["Code"].ToString();
                    lst.Add(DDl);
                }

            }
            return lst;

        }


        public List<DropDownList> GetItemList(int tenatID, string Prefix)
        {
            List<DropDownList> lst = new List<DropDownList>();

            //string Query = "SELECT  MCode, MM.MaterialMasterID FROM MMT_MaterialMaster MM JOIN MMT_MaterialMaster_Supplier MMS ON MMS.MaterialMasterID =  MM.MaterialMasterID JOIN TPL_Tenant_Supplier TS ON TS.SupplierID =  MMS.SupplierID WHERE TS.TenantID =  " + tenatID + " AND ( (MCode LIKE '%"+ Prefix + "%' OR MCode LIKE '" + Prefix + "%' OR MCode LIKE '%" + Prefix + "'))";
            string Query = "SELECT DISTINCT TOP 15 MM.MCode, MM.MaterialMasterID FROM vAvailableStock vAS JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID = vAS.MaterialMasterID JOIN TPL_Tenant_MaterialMaster TM ON TM.MaterialMasterID =  MM.MaterialMasterID WHERE VAS.StorageLocationID = 3 AND TM.TenantID = " + tenatID + " AND ( (MM.MCode LIKE '%" + Prefix + "%' OR MM.MCode LIKE '" + Prefix + "%' OR MM.MCode LIKE '%" + Prefix + "'))";
            DataSet DS = DB.GetDS(Query, false);
            if(DS.Tables[0].Rows.Count != 0)
            {
                foreach(DataRow row in DS.Tables[0].Rows)
                {
                    DropDownList DDl = new DropDownList();
                    DDl.Id = Convert.ToInt32(row["MaterialMasterID"]);
                    DDl.Name = row["MCode"].ToString();
                    lst.Add(DDl);
                }
               
            }
            return lst;
            
        }
        public List<DropDownList> Getlocationinfo(int mmid, string Prefix, int TenantID, int WHID)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            List<DropDownList> lst = new List<DropDownList>();
            int accountid =  DB.GetSqlN("Exec[dbo].[USP_MST_GetAccount_Tenant] @TenantID = " + TenantID);

            //  string Query1 = "SELECT TOP 20 LOC.LocationID, LOC.Location FROM INV_Location LOC JOIN INV_LocationZone LOCZ ON LOCZ.LocationZoneID =  LOC.ZoneId JOIN GEN_Warehouse WH ON WH.WarehouseID =  LOCZ.WarehouseID WHERE WH.AccountID = "+cp.AccountID+" AND LOC.IsActive =  1 AND LOC.IsDeleted = 0 AND LOC.Location LIKE '%"+ Prefix + "%'";

            //From Miscellaneous and available stock
            //  string Query = "SELECT DISTINCT VAS.LocationID,LOC.Location, VAS.CartonID, CN.CartonCode FROM vAvailableStock VAS JOIN INV_Location LOC ON LOC.LocationID = VAS.LocationID JOIN INV_Carton CN ON CN.CartonID = VAS.CartonID WHERE VAS.StorageLocationID = 3  AND MaterialMasterId = "+mmid+" AND LOC.Location LIKE '%" + Prefix + "%'";
            string Query = "SELECT DISTINCT VAS.LocationID,LOC.DisplayLocationCode Location  FROM vAvailableStock VAS JOIN INV_Location LOC ON LOC.LocationID = VAS.LocationID  LEFT JOIN INV_Carton CN ON CN.CartonID = VAS.CartonID  LEFT JOIN INV_LocationZone LOCZ ON LOCZ.LocationZoneID =  LOC.ZoneId LEFT JOIN  TPL_Tenant_Contract TPL on TPL.WarehouseID=LOCZ.warehouseid LEFT JOIN GEN_Warehouse GEN on GEN.WarehouseID=TPL.WarehouseID"
                              + " where accountid = " + accountid + " and TPL.tenantID = " + TenantID + " and GEN.WarehouseID=" + WHID + " and TPl.IsActive = 1 and TPL.IsDeleted = 0 and LOC.IsActive = 1 AND LOC.IsDeleted = 0 and VAS.StorageLocationID = 3 AND IsBlockedForCycleCount !=1 AND MaterialMasterId = " + mmid + " AND LOC.Location LIKE '%%'";
            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    DropDownList DDl = new DropDownList();
                    DDl.Id = Convert.ToInt32(row["LocationID"]);
                    DDl.Name = row["Location"].ToString();
                    lst.Add(DDl);
                }

            }
            return lst;
        }

        public List<DropDownList> GetCartonInfo(int mmid,string Prefix)
        {
            List<DropDownList> lst = new List<DropDownList>();

            //string Query = "SELECT TOP 20 CartonID, CartonCode  FROM INV_Carton WHERE CartonCode LIKE '%"+ Prefix + "%'";
            string Query = "SELECT DISTINCT VAS.LocationID,LOC.Location, VAS.CartonID, CN.CartonCode FROM vAvailableStock VAS JOIN INV_Location LOC ON LOC.LocationID = VAS.LocationID JOIN INV_Carton CN ON CN.CartonID = VAS.CartonID WHERE VAS.StorageLocationID = 3  AND MaterialMasterId = " + mmid + "AND LOC.Location = " + DB.SQuote(Prefix);
            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    DropDownList DDl = new DropDownList();
                    DDl.Id = Convert.ToInt32(row["CartonID"]);
                    DDl.Name = row["CartonCode"].ToString();
                    lst.Add(DDl);
                }
            }
            return lst;
        }

        public List<DropDownList> GetToLocInfo(string Prefix, string TenantID = "0", int WarehouseId = 0)
        {
            List<DropDownList> lst = new List<DropDownList>();

            //string Query = "SELECT TOP 20 CartonID, CartonCode  FROM INV_Carton WHERE CartonCode LIKE '%"+ Prefix + "%'";
            // string Query = "SELECT TOP 15 Location, LocationID FROM INV_Location WHERE Location LIKE '%"+ Prefix + "%' AND IsActive = 1";
            string Query = "EXEC [dbo].[GET_TENANTWISELOCATION] @TenantID = " + TenantID + ", @Prefix = '" + Prefix + "', @WarehouseId=" + WarehouseId + "";
            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    DropDownList DDl = new DropDownList();
                    DDl.Id = Convert.ToInt32(row["LocationID"]);
                    DDl.Name = row["Location"].ToString();
                    lst.Add(DDl);
                }
            }
            return lst;
        }

        public List<DropDownList> GetCartonList(string Prefix, string Location, string TenantID ="0")
        {
            List<DropDownList> lst = new List<DropDownList>();

            string Query = "EXEC [dbo].[sp_GET_CartonList_Internal] @Location = '"+ Location + "', @TenantID ="+ TenantID + "";
           // string Query = "SELECT TOP 15 CartonID, CartonCode FROM INV_Carton WHERE CartonCode LIKE '%" + Prefix + "%' AND IsActive = 1 AND IsDeleted =0 AND WareHouseID IN(SELECT DISTINCT WarehouseID FROM TPL_Tenant_Contract WHERE TenantID = "+ TenantID + " AND IsActive = 1 AND IsDeleted  =0)";
            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    DropDownList DDl = new DropDownList();
                    DDl.Id = Convert.ToInt32(row["CartonID"]);
                    DDl.Name = row["CartonCode"].ToString();
                    lst.Add(DDl);
                }
            }
            return lst;
        }


        // added by lalitha on 14/02/2019
        //public List<DropDownList> GetWarehouse()
        //{
        //    List<DropDownList> lst = new List<DropDownList>();

        //    string Query = "Exec[dbo].[USP_MST_DropWH] @AccountID = " + cp.AccountID.ToString() +"";
        //    DataSet DS = DB.GetDS(Query, false);
        //    if (DS.Tables[0].Rows.Count != 0)
        //    {
        //        foreach (DataRow row in DS.Tables[0].Rows)
        //        {
        //            DropDownList DDl = new DropDownList();
        //            DDl.Id = Convert.ToInt32(row["WarehouseID"]);
        //            DDl.Name = row["WHCode"].ToString();
        //            lst.Add(DDl);
        //        }

        //    }
        //    return lst;

        //}


        public class DropDownList
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }           

        public class TransferRequest
        {
            public int TransferID { get; set; }
            public string TransferNo { get; set; }
            public int TransferTypeID { get; set; }
            public string Remarks { get; set; }
            public int TenantID { get; set; }
            public string TenantName { get; set; }
            public int IsSuggestedReq { get; set; }
            public string status { get; set; }

            public int WarehouseId { get; set; }
            public string WHcode { get; set; }

            public int StatusId { get; set; }

        }

        public class TransferRequestDetails
        {
            public int TransferID { get; set; }
            public int TransferRequestDetailsID { get; set; }
            public int MaterialMasterID { get; set; }
            public string Mcode { get; set; }
            public string BatchNo { get; set; }
            public decimal Quantiy { get; set; }
            public int LocationID { get; set; }
            public string Location { get; set; }
            public string FromSL { get; set; }
            public int FromSLID { get; set; }
            public int ToSLID { get; set; }
            public string ToSL { get; set; }
            public string Tolocation { get; set; }
            public int ToLocationID { get; set; }
            public int IsTransferDone { get; set; }
            public string ToCarton { get; set; }
            
        }


    }
}