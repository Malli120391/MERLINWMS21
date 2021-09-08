using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MRLWMSC21Common;
using System.Data;
using System.Text;

namespace MRLWMSC21WCF.BL
{
    public class VLPDHandler
    {
        public string GetVLPDNo(BO.VLPD VLPD)
        {
            try
            {
                return GetVLPDID(VLPD).ToString();
            }
            catch (Exception ex)
            {
                return "0";
            }
        }
        public static int GetVLPDID(BO.VLPD VLPD)
        {
            return DB.GetSqlN("select id as N from obd_vlpd where VLPDNumber=" + DB.SQuote(VLPD.VLPDNo) + " AND IsDeleted=0 " + " AND StatusId=3");
        }
        public BO.VLPD GetItemToPick(BO.VLPD VLPD)
        {

            if (VLPD.TransferRequestId != 0)
            {

                string query = "EXEC [dbo].[GEN_PICK_INTERNALTRANSFER_HHT] @TransferRequestID=" + VLPD.TransferRequestId;
                try
                {
                    DataSet ds = DB.GetDS(query, false);

                    if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count != 0)
                    {
                        VLPD.TransferRequestId = Convert.ToInt32(ds.Tables[0].Rows[0]["TransferRequestID"].ToString());
                        VLPD.VLPDNo = ds.Tables[0].Rows[0]["VLPDNumber"].ToString();
                        VLPD.Assignedid = Convert.ToInt32(ds.Tables[0].Rows[0]["AssignedID"].ToString());
                        VLPD.MaterialMasterId = Convert.ToInt32(ds.Tables[0].Rows[0]["MaterialMasterID"].ToString());
                        VLPD.MCode = ds.Tables[0].Rows[0]["MCode"].ToString();
                        VLPD.MDescription = ds.Tables[0].Rows[0]["MDescription"].ToString();
                        VLPD.FromCartonCode = ds.Tables[0].Rows[0]["CartonCode"].ToString();
                        VLPD.FromCartonID = Convert.ToInt32(ds.Tables[0].Rows[0]["CartonID"].ToString());
                        VLPD.Location = ds.Tables[0].Rows[0]["Location"].ToString();
                        VLPD.LocationID = Convert.ToInt32(ds.Tables[0].Rows[0]["LocationID"].ToString());
                        VLPD.MfgDate = ds.Tables[0].Rows[0]["MfgDate"].ToString();
                        VLPD.ExpDate = ds.Tables[0].Rows[0]["ExpDate"].ToString();
                        VLPD.SerialNo = ds.Tables[0].Rows[0]["SerialNo"].ToString();
                        VLPD.BatchNo = ds.Tables[0].Rows[0]["BatchNo"].ToString();
                        VLPD.ProjectRefNo = ds.Tables[0].Rows[0]["ProjectRefNo"].ToString();
                        VLPD.AssignedQuantity = ds.Tables[0].Rows[0]["AssignedQuantity"].ToString();
                        VLPD.PickedQty = ds.Tables[0].Rows[0]["PickedQty"].ToString();
                        VLPD.PendingQty = ds.Tables[0].Rows[0]["PendingQty"].ToString();
                        VLPD.OutboundID = ds.Tables[0].Rows[0]["OutboundID"].ToString();
                        VLPD.SODetailsID = ds.Tables[0].Rows[0]["SODetailsID"].ToString();
                        VLPD.StorageLocationID = ds.Tables[0].Rows[0]["StorageLocationID"].ToString();
                        VLPD.StorageLocation = ds.Tables[0].Rows[0]["SLOC"].ToString();
                        VLPD.GoodsmomentDeatilsId = Convert.ToInt32(ds.Tables[0].Rows[0]["GoodsMovementDetailsID"].ToString());
                        VLPD.TransferRequestDetailsId = Convert.ToInt32(ds.Tables[0].Rows[0]["TransferRequestDetailsID"].ToString());
                        VLPD.MRP = ds.Tables[0].Rows[0]["MRP"].ToString();
                    }
                    else
                    {
                        VLPD.PendingQty = "";
                    }

                    return VLPD;

                }
                catch (Exception ex)
                {
                    throw ex;

                }
            }


            else
            {
                string query = " EXEC [dbo].[GEN_VLPD_ITEMS_ALLOCATED_LIST_OBDWISE_HHT] @VLPDID=" + VLPD.VLPDId + ",@TransferRequestID=" + VLPD.TransferRequestId;
                try
                {
                    DataSet ds = DB.GetDS(query, false);

                    if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count != 0)
                    {
                        VLPD.TransferRequestId = Convert.ToInt32(ds.Tables[0].Rows[0]["TransferRequestID"].ToString());
                        VLPD.VLPDNo = ds.Tables[0].Rows[0]["VLPDNumber"].ToString();
                        VLPD.Assignedid = Convert.ToInt32(ds.Tables[0].Rows[0]["AssignedID"].ToString());
                        VLPD.MaterialMasterId = Convert.ToInt32(ds.Tables[0].Rows[0]["MaterialMasterID"].ToString());
                        VLPD.MCode = ds.Tables[0].Rows[0]["MCode"].ToString();
                        VLPD.MDescription = ds.Tables[0].Rows[0]["MDescription"].ToString();
                        VLPD.FromCartonCode = ds.Tables[0].Rows[0]["CartonCode"].ToString();
                        VLPD.FromCartonID = Convert.ToInt32(ds.Tables[0].Rows[0]["CartonID"].ToString());
                        VLPD.Location = ds.Tables[0].Rows[0]["Location"].ToString();
                        VLPD.LocationID = Convert.ToInt32(ds.Tables[0].Rows[0]["LocationID"].ToString());
                        VLPD.MfgDate = ds.Tables[0].Rows[0]["MfgDate"].ToString();
                        VLPD.ExpDate = ds.Tables[0].Rows[0]["ExpDate"].ToString();
                        VLPD.SerialNo = ds.Tables[0].Rows[0]["SerialNo"].ToString();
                        VLPD.BatchNo = ds.Tables[0].Rows[0]["BatchNo"].ToString();
                        VLPD.ProjectRefNo = ds.Tables[0].Rows[0]["ProjectRefNo"].ToString();
                        VLPD.AssignedQuantity = ds.Tables[0].Rows[0]["AssignedQuantity"].ToString();
                        VLPD.PickedQty = ds.Tables[0].Rows[0]["PickedQty"].ToString();
                        VLPD.PendingQty = ds.Tables[0].Rows[0]["PendingQty"].ToString();
                        VLPD.OutboundID = ds.Tables[0].Rows[0]["OutboundID"].ToString();
                        VLPD.SODetailsID = ds.Tables[0].Rows[0]["SODetailsID"].ToString();
                        VLPD.StorageLocationID = ds.Tables[0].Rows[0]["StorageLocationID"].ToString();
                        VLPD.StorageLocation = ds.Tables[0].Rows[0]["SLOC"].ToString();
                        VLPD.GoodsmomentDeatilsId = Convert.ToInt32(ds.Tables[0].Rows[0]["GoodsMovementDetailsID"].ToString());
                        VLPD.TransferRequestDetailsId = Convert.ToInt32(ds.Tables[0].Rows[0]["TransferRequestDetailsID"].ToString());
                        VLPD.MRP = ds.Tables[0].Rows[0]["MRP"].ToString();

                    }
                    else
                    {
                        VLPD.PendingQty = "0";
                    }

                    return VLPD;

                }
                catch (Exception ex)
                {
                    throw ex;

                }
            }

        }


        public BO.VLPD RegeneratePickItemsuggestion(BO.VLPD VLPD)
        {
            StringBuilder sbSqlQuery = new StringBuilder();
            sbSqlQuery.Append(" EXEC [dbo].[USP_SET_SuggestedGMDLostAndFound] ");
            sbSqlQuery.Append("@VLPDID=" + Convert.ToInt32(VLPD.VLPDId));
            sbSqlQuery.Append(",@Location=" +DB.SQuote(VLPD.Location));
            sbSqlQuery.Append(",@MCODE = " + DB.SQuote(VLPD.MCode));
            sbSqlQuery.Append(",@MfgDate=" + (VLPD.MfgDate != "" ? DB.SQuote(VLPD.MfgDate) : "''"));
            sbSqlQuery.Append(",@ExpDate=" + (VLPD.ExpDate != "" ? DB.SQuote(VLPD.ExpDate) : "''"));
            sbSqlQuery.Append(",@SerialNo=" + (VLPD.SerialNo != "" ? DB.SQuote(VLPD.SerialNo) : "''"));
            sbSqlQuery.Append(",@BatchNo = " + (VLPD.BatchNo != "" ? DB.SQuote(VLPD.BatchNo) : "''"));
            sbSqlQuery.Append(",@ProjectRefNo = " + (VLPD.ProjectRefNo != "" ? DB.SQuote(VLPD.ProjectRefNo) : "''"));
            sbSqlQuery.Append(",@UserLoggedId=" + VLPD.UserId );
            sbSqlQuery.Append(",@Quantity = " + (VLPD.SkipQty));
            sbSqlQuery.Append(",@SuggestedId = " + Convert.ToInt32(VLPD.Assignedid));
            sbSqlQuery.Append(",@FullfilledQuantity = " + (VLPD.TotalPickedQty));
            sbSqlQuery.Append(",@Reason=" + DB.SQuote(VLPD.SkipReason));
            sbSqlQuery.Append(",@IsVLPDPicking=" + 1);


            DataSet ds = DB.GetDS(sbSqlQuery.ToString(), false);

            if (ds.Tables[0].Rows[0]["N"].ToString() == "-1")
            {
                VLPD.Erocode = "-1";

            }
            else
            {
                VLPD.Erocode = "1";
            }
            VLPD.Result = (ds.Tables[1].Rows[0]["ErrorMessage"].ToString());

           

            //if(result==1)
            //{
            //    VLPD.Result = "No materials in pick list";
            //}
            //else if(result ==2)
            //{
            //    VLPD.Result = "No Active stock is available for this shipment.";
            //}
            //else if(result ==3)
            //{
            //    VLPD.Result = " Partial fulfilment of inventory";
            //}
            //else
            //{
            //    VLPD.Result = "";
            //}
            return VLPD;
        }
        public BO.VLPD VLPDSkipItem(BO.VLPD VLPD)
        {
            string drlStatement = "EXEC [dbo].[USP_SET_OBD_SKIP]  @VLPDAssignID=" + Convert.ToInt32(VLPD.Assignedid) + ",@MCode=" + DB.SQuote(VLPD.MCode) + ",@MfgDate=" + (VLPD.MfgDate != "" ? DB.SQuote(VLPD.MfgDate) : "''") + ",@ExpDate=" + (VLPD.ExpDate != "" ? DB.SQuote(VLPD.ExpDate) : "''") + ",@SerialNo=" + (VLPD.SerialNo != "" ? DB.SQuote(VLPD.SerialNo) : "''") + ",@BatchNo = " + (VLPD.BatchNo != "" ? DB.SQuote(VLPD.BatchNo) : "''") + ",@ProjectRefNo = " + (VLPD.ProjectRefNo != "" ? DB.SQuote(VLPD.ProjectRefNo) : "''") + ",@SkipQty = " + (VLPD.SkipQty) + ", @CreatedBy=" + Convert.ToInt32(VLPD.UserId) + ",@CartonCode=" + DB.SQuote(VLPD.FromCartonCode) + ",@Location=" + DB.SQuote(VLPD.Location) + ",@Reason=" + DB.SQuote(VLPD.SkipReason) + ",@VLPDID=" + VLPD.VLPDId;
            string result = DB.GetSqlS(drlStatement);

            VLPD.Result = result.ToString();
            
            if (VLPD.Flag == 1)
            {
                return VLPD;
            }
        
            return VLPD;
        }



        public BO.VLPD UpsertPickItem(BO.VLPD Pickdata)
        {
            if (Pickdata.ToCartonCode != "")
            {
                if ((DB.GetSqlN("select count(*) as N from OBD_VLPD_PickedItems where  IsDeleted = 0  AND IsActive = 1 AND  VLPDID=" + Pickdata.VLPDId) > 0))
                {
                    int IsCartonPicking = DB.GetSqlN("select count(*) as N from OBD_VLPD_PickedItems WHERE IsDeleted = 0  AND IsActive = 1  and ToCartonID!=0 and ToCartonID is not null AND VLPDID = " + Pickdata.VLPDId);
                    if (IsCartonPicking == 0 )
                    {
                        Pickdata.Result=  "-444";
                        return Pickdata;
                    }            

                }
            }
            try
            {

                StringBuilder dmlStatement = new StringBuilder();
                dmlStatement.Append("DECLARE @NewVLPDID_ID int");
                dmlStatement.Append(" EXEC  [dbo].[UPSERT_VLPD_PICKEDINFO_HHT] ");
                dmlStatement.Append("@MaterialMasterID =" + Pickdata.MaterialMasterId + ",");
                dmlStatement.Append("@AssignID=" + Pickdata.Assignedid + ",");
                dmlStatement.Append("@PickQty=" + Pickdata.PickedQty + ",");
                dmlStatement.Append("@LocationID=" + Pickdata.LocationID + ",");
                dmlStatement.Append("@CartonID=" + Pickdata.FromCartonID + ",");
                dmlStatement.Append("@MfgDate=" + (DB.DateQuote(Pickdata.MfgDate)) + ",");
                dmlStatement.Append("@ExpDate=" + (DB.DateQuote(Pickdata.ExpDate)) + ",");
                dmlStatement.Append("@SerialNo=" + (DB.SQuote(Pickdata.SerialNo)) + ",");
                dmlStatement.Append("@BatchNo=" + (DB.SQuote(Pickdata.BatchNo)) + ",");
                dmlStatement.Append("@VLPDID=" + Pickdata.VLPDId + ",");
                dmlStatement.Append("@ProjectRefNo=" + (DB.SQuote(Pickdata.ProjectRefNo)) + ",");
                dmlStatement.Append("@CreatedBy=" + Pickdata.UserId + ",");
                dmlStatement.Append("@OutboundID=" + (Pickdata.OutboundID != "" ? Pickdata.OutboundID : "NULL") + ",");
                dmlStatement.Append("@SODetailsID=" + (Pickdata.SODetailsID != "" ? Pickdata.SODetailsID : "NULL") + ",");
                dmlStatement.Append("@StorageLocationID=" + Pickdata.StorageLocationID + ",");
                dmlStatement.Append("@TransferRequestId=" + Pickdata.TransferRequestId + ",");
                dmlStatement.Append("@TransferRequestDetailsId=" + Pickdata.TransferRequestDetailsId + ",");
                dmlStatement.Append("@ToCartonCode=" + DB.SQuote(Pickdata.ToCartonCode) + ",");
                dmlStatement.Append("@MRP=" + DB.SQuote(Pickdata.MRP) + ",");
                dmlStatement.Append("@NewVLPDID=@NewVLPDID_ID OUTPUT select @NewVLPDID_ID AS N;");
                int Result = DB.GetSqlN(dmlStatement.ToString());
                Pickdata.Result = Result.ToString();
                if(Pickdata.Result!="0")
                {

                    //if(Pickdata.TransferRequestId!=0)
                    //{
                    //    UpdatePickItemForInternalTransfer(Pickdata);
                    //}
                    //else
                    //{
                    //    SetFastMovingLocation(Pickdata);
                    //}
                    Pickdata = GetPickedQty(Pickdata);
                    CheckItem(Pickdata);
                    GetItemToPick(Pickdata);
                }
                //  DB.ExecuteSQL(dmlStatement.ToString());
                return Pickdata;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void CheckItem(BO.VLPD vLPD)
        {
            DB.GetSqlN("EXEC [dbo].[SP_ChangeGateEntryStatus] @ShipmentId =" + vLPD.VLPDId + ",@IsForInbound =0");
        }

        //public BO.VLPD UpdatePickItemForInternalTransfer(BO.VLPD outbound)
        //{


        //    DataSet ds = DB.GetDS("EXEC [dbo].[sp_GetOutboundData] @MaterialMasterID=" + outbound.MaterialMasterId + ",@OuboundID=" + outbound.OutboundID + ",@LocationID=" + outbound.LocationID, false);
        //    if (ds.Tables[0].Rows.Count != 0)
        //    {
        //        outbound.MaterialMaster_IUoMID = ds.Tables[0].Rows[0]["MaterialMaster_SUoMID"].ToString();
        //        outbound.OutboundID = ds.Tables[0].Rows[0]["OutboundID"].ToString();
        //        outbound.MaterialMasterId = Convert.ToInt32(ds.Tables[0].Rows[0]["MaterialMasterID"].ToString());
        //        outbound.POSOHeaderId = ds.Tables[0].Rows[0]["SOHeaderID"].ToString();
        //        outbound.CF = Convert.ToDecimal(ds.Tables[0].Rows[0]["CF"].ToString());
        //        outbound.KitId = ds.Tables[0].Rows[0]["KitPlannerID"].ToString();
        //        outbound.Lineno =ds.Tables[0].Rows[0]["LineNumber"].ToString();
        //    }




        //    outbound.QtyinBUoM = Convert.ToDecimal(Convert.ToDecimal(outbound.PickedQty )* outbound.CF);
        //    int result = InventoryCommonClass.UpdateGoodsMovementDetails("0", "2", outbound.Location, outbound.MaterialMaster_IUoMID, outbound.PickedQty.ToString(), outbound.QtyinBUoM.ToString(), outbound.OutboundID, "0", outbound.Lineno, outbound.MaterialMasterId.ToString(),"","", outbound.POSOHeaderId, outbound.UserId.ToString(), "0", "0", outbound.CF.ToString(), "", outbound.UserId.ToString(), "0", outbound.KitId != "" ? outbound.KitId : "NULL", "0", "0", "0", outbound.CartonID.ToString(), outbound.StorageLocationID, outbound.MfgDate, outbound.ExpDate, outbound.SerialNo, outbound.BatchNo, outbound.ProjectRefNo);
        //    if (result == 1)
        //    {

        //        outbound.Result = "Success";
        //        return outbound;
        //    }
        //    else
        //    {
        //        outbound.Result = "Process failed,Please Contact Support team";
        //        return outbound;
        //    }

        //    return outbound;

        //}

        public BO.VLPD GetPickedQty(BO.VLPD VLPd)
        {
            DataSet ds = DB.GetDS("EXEC [dbo].[Get_PickedQty_HHT] @VLPDID=" + VLPd.VLPDId + ",@Mcode=" + DB.SQuote(VLPd.MCode) + ",@TransferrequestID=" + VLPd.TransferRequestId, false);
            if (ds.Tables[0].Rows.Count != 0)
            {
                VLPd.TotalPickedQty = Convert.ToDecimal(ds.Tables[0].Rows[0]["PickedQty"].ToString());
            }
           
            return VLPd;
        }
        public BO.VLPD UpdatePickItemForInternalTransfer(BO.VLPD VLPDResponse)
        {


            DataSet ds = DB.GetDS("select * from INV_GoodsMovementDetails where GoodsMovementDetailsID="+ VLPDResponse.GoodsmomentDeatilsId, false);
            if (ds.Tables[0].Rows.Count != 0)
            {
                VLPDResponse.MaterialMaster_IUoMID = ds.Tables[0].Rows[0]["MaterialMaster_IUoMID"].ToString();
                VLPDResponse.MaterialMasterId = Convert.ToInt32(ds.Tables[0].Rows[0]["MaterialMasterID"].ToString());
                VLPDResponse.CF = Convert.ToDecimal(ds.Tables[0].Rows[0]["ConversionFactor"].ToString());
                VLPDResponse.MfgDate = ds.Tables[0].Rows[0]["MfgDate"].ToString();
                VLPDResponse.ExpDate = ds.Tables[0].Rows[0]["ExpDate"].ToString();
                VLPDResponse.SerialNo = ds.Tables[0].Rows[0]["SerialNo"].ToString();
                VLPDResponse.BatchNo = ds.Tables[0].Rows[0]["BatchNo"].ToString();
                VLPDResponse.ProjectRefNo = ds.Tables[0].Rows[0]["ProjectRefNo"].ToString();
                VLPDResponse.KitId=ds.Tables[0].Rows[0]["KitPlannerID"].ToString();
            }
            StringBuilder dmlStatement = new StringBuilder();
            dmlStatement.Append(" EXEC  [dbo].[UpsertInternalTransferItems_HHT]  ");
            dmlStatement.Append("@GoodsMomentDeatilsID =" + VLPDResponse.GoodsmomentDeatilsId + ",");
            dmlStatement.Append("@GoodsMomentTypeID=" + 2 + ",");
            dmlStatement.Append("@LocationID=" + VLPDResponse.LocationID + ",");
            dmlStatement.Append("@CartonID=" + VLPDResponse.FromCartonID + ",");
            dmlStatement.Append("@Quantity=" + VLPDResponse.PickedQty + ",");
            dmlStatement.Append("@StorageLocationID=" + VLPDResponse.StorageLocationID + ",");
            dmlStatement.Append("@CreatedBy=" + VLPDResponse.UserId + ",");
            dmlStatement.Append("@TransferRequestID=" + VLPDResponse.LocationID);
            int result=DB.GetSqlN(dmlStatement.ToString());
            if (result == 0)
            {
                VLPDResponse.Result = "Error while inserting";
            }
            else 
            {
                VLPDResponse.Result = "Inserted Successfully.";
            }
            return VLPDResponse;

        }
        public void SetFastMovingLocation(BO.VLPD Pickdata)
        {
            StringBuilder dmlStatement = new StringBuilder();
            dmlStatement.Append(" EXEC  [dbo].[sp_INV_SetFastMovementLocsFulfillOrders] ");
            dmlStatement.Append("@MaterialMasterID =" + Pickdata.MaterialMasterId + ",");
            dmlStatement.Append("@LocationID=" + 0+ ",");
            dmlStatement.Append("@Location=" + DB.SQuote(Pickdata.Location));
            DB.GetSqlN(dmlStatement.ToString());
        }
        public UserDataTable GetOpenVLPDNos(BO.VLPD vLPD)
        {
            UserDataTable usertable = new UserDataTable();
            DataSet ds = DB.GetDS("EXEC [dbo].[Get_VLPDList_HHT] @AccountId="+vLPD.AccountId, false);
            DataTable dt = ds.Tables[0];
            usertable.Table = dt;
            return usertable;

        }

        public UserDataTable GetOpenLoadsheetList(String TenantID, string AccountID)
        {
            UserDataTable usertable = new UserDataTable();
            DataSet ds = DB.GetDS("EXEC SP_OpenLoadList @TenantID = " + TenantID + ",@AccountID=" + AccountID, false); //============== Added By M.D.Prasad ON 28 - 08 - 2020 For AccounID ===========
            DataTable dt = ds.Tables[0];
            usertable.Table = dt;
            return usertable;
        }

        public UserDataTable GetPendingOBDListForLoading(String TenantID, String AccountID)
        {
            UserDataTable usertable = new UserDataTable();
            DataSet ds = DB.GetDS("EXEC SP_Pending_Load_OBD @AcountId = "+AccountID+ ", @TenantID = "+TenantID+"", false);
            DataTable dt = ds.Tables[0];
            usertable.Table = dt;
            return usertable;
        }

        public string UpsertLoadCreated(BO.VLPD VLPD)
        {
            try
            {
                string Query = "EXEC SP_GET_Upsert_LoadSheet @TenantId = " + VLPD.TenantId + ", @AccountID = " + VLPD.AccountId + ", @VEHICLENO = " + DB.SQuote(VLPD.Vehicle) + ", @OBDSNUMBER = " + DB.SQuote(VLPD.OBDNumber) + ", @DRIVERNO = " + DB.SQuote(VLPD.DriverNo) + ", @DRIVERNAME = " + DB.SQuote(VLPD.DriverName) + ", @LRNumber = " + DB.SQuote(VLPD.LRnumber) + ", @USERID = " + VLPD.UserId + "";
                return DB.GetSqlS(Query);
            }
            catch(Exception Ex)
            {
                return Ex.Message;
            }
            
        }
        public string UpsertLoad(BO.VLPD VLPD)
        {
            try
            {
                string Query = "EXEC SP_OBD_Upsert_Loading @MCODE = " + DB.SQuote(VLPD.MCode) + ", @LOADNUMBER = " + DB.SQuote(VLPD.VLPDNo) + ", @MfgDate = " + DB.SQuote(VLPD.MfgDate) + ", @ExpDate = " + DB.SQuote(VLPD.ExpDate) + ", @BatchNo = " + DB.SQuote(VLPD.BatchNo) + ", @SerialNo = " + DB.SQuote(VLPD.SerialNo) + ", @ProjectRefNo = " + DB.SQuote(VLPD.ProjectRefNo) + ", @MRP = " + DB.SQuote(VLPD.MRP) + ", @LOADEDQTY = "+DB.SQuote(VLPD.PickedQty)+ ",@UserID = "+VLPD.UserId+"";
                return DB.GetSqlS(Query);
            }
            catch (Exception Ex)
            {
                return Ex.Message;
            }

        }
        public string LoadVerification(String LoadNumber, String UserId)
        {
            try
            {
                string Query = "EXEC [dbo].[SP_Load_CheckLoadingStatus] @LoadSheetNumber = " + DB.SQuote(LoadNumber)+ ", @UserId = "+UserId+"";
                string result= DB.GetSqlS(Query);
                if(result=="1")
                {
                    string strOBDList = "EXEC [dbo].[SP_Load_GetOBDNumbersByLoadSheet] @LoadSheetRef="+DB.SQuote(LoadNumber);

                    DataSet dsOBDList=DB.GetDS(strOBDList,false);
                  
                    if (dsOBDList != null && dsOBDList.Tables[0].Rows.Count != 0)
                    {
                        int totalOBDCOunt = dsOBDList.Tables[0].Rows.Count;
                        int successOBD = 0;
                        foreach (DataRow dataROw in dsOBDList.Tables[0].Rows)
                        {
                            string outboundID = dataROw["OutboundID"].ToString();

                            string strQueryForStockOut = "EXEC [dbo].[sp_OBD_MoveStockOutByPickingData] @OutboundID=" + outboundID + ",@CreatedBy=" + UserId;
                            DataSet dsPickingResult = DB.GetDS(strQueryForStockOut, false);
                            if (dsPickingResult != null && dsPickingResult.Tables.Count != 0)
                            {
                                if (dsPickingResult.Tables.Count == 2)
                                {
                                    if (dsPickingResult.Tables[0].Rows[0][0].ToString() == "-1")
                                    {
                                        result = "Picking is not completed";

                                    }
                                    else if (dsPickingResult.Tables[0].Rows[0][0].ToString() == "-2")
                                    {
                                        result = "Picked quantity not available at dock location";

                                    }
                                    else if (dsPickingResult.Tables[0].Rows[0][0].ToString() == "-3")
                                    {
                                        result = "There is an issue in while processing stock out";

                                    }
                                    else if (dsPickingResult.Tables[0].Rows[0][0].ToString() == "-4")
                                    {
                                        result = "Unexpected error  while processing stock out";

                                    }
                                    else if (dsPickingResult.Tables[0].Rows[0][0].ToString() == "-5")
                                    {
                                        result = "There is no pending quantity for stock out";

                                    }
                                }
                                else if (dsPickingResult.Tables.Count == 1)
                                {
                                    StringBuilder sbPGIQuery = new StringBuilder();
                                    sbPGIQuery.Append("DECLARE @NewUpdateOutboundID int;  ");
                                    sbPGIQuery.Append("EXEC[dbo].[sp_OBD_UpsertPGI]");
                                    sbPGIQuery.Append("@OutboundID = " + outboundID);
                                    sbPGIQuery.Append(",@PGIDoneBy="+UserId);                                   
                                    sbPGIQuery.Append(",@DocumentTypeID=" + 1);
                                    sbPGIQuery.Append(",@UserID="+ UserId);
                                    sbPGIQuery.Append(",@OB_RefWarehouse_DetailsID=0");
                                    sbPGIQuery.Append(",@IsRequestFromPDT=1");
                                    sbPGIQuery.Append(",@UpdatedBy="+ UserId);
                                    sbPGIQuery.Append(",@NewOutboundID=@NewUpdateOutboundID OUTPUT ;");
                                    sbPGIQuery.Append(" SELECT @NewUpdateOutboundID AS N ;");

                                    

                                    DB.GetSqlN(sbPGIQuery.ToString());

                                    string HUQuery = "EXEC [dbo].[SP_Upsert_HU_GoodsOut] @outboundID=" + outboundID + ",@UserID=" + UserId;
                                    DataSet dsHu = DB.GetDS(HUQuery, false);


                                    if (dsHu.Tables[0].Rows[0][0].ToString() == "Done")
                                    {
                                        successOBD = successOBD + 1;
                                    }
                                        
                                }
                            }
                            else
                            {
                                result = "Error while processing stock out transaction, please contact support ";
                                
                            }

                        }

                        if (successOBD == totalOBDCOunt)
                        {
                            DB.ExecuteSQL("UPDATE OBD_LoadSheet_Header SET StatusID = 3  WHERE LoadSheetNo =" + DB.SQuote(LoadNumber));

                            return "PGI Updated";
                        }
                        else
                        {
                            return "Unable to cloase loadsheet as some deliveries are not processed";
                        }
                            
                    }
                    {
                        return "Error while closing loadsheet";

                    }
                    
                }
                else
                {
                    return result;
                }

             
            }
            catch (Exception Ex)
            {
                return Ex.Message;
            }
        }

        public BO.VLPD DeleteVLPDPickedItems(BO.VLPD vLPD)
        {
          
            int value=DB.GetSqlN("[dbo].[DELETE_PICKEDITEMS_FOR_VLPD_OUTBOUND] @VLPDPickedID=" + vLPD.PickedId + ",@CreatedBy=" + vLPD.UserId);
            if(value==1)
            {
                vLPD.Result = "Deleted successfully";
            }
            else
            {
                vLPD.Result = "Error while deleting.";
            }
            return vLPD;
        }
        public UserDataTable GetVLPDPickedList(BO.VLPD vLPD, out string Result)
        {
            UserDataTable usertable = new UserDataTable();
            string sql = "[dbo].[sp_GET_VLPD_PickedInfoList_HHT] @VLPDID=" + vLPD.VLPDId + ",@MCode=" + DB.SQuote(vLPD.MCode) + ",@MFGDate=" + DB.SQuote(vLPD.MfgDate) + ",@EXPDate=" + DB.SQuote(vLPD.ExpDate);
            sql += ",@Batch=" + DB.SQuote(vLPD.BatchNo) + ",@SerialNo=" + DB.SQuote(vLPD.SerialNo) + ",@ProjectRef=" + DB.SQuote(vLPD.ProjectRefNo) + ",@MRP=" + DB.SQuote(vLPD.MRP);
            DataSet ds = DB.GetDS(sql, false);
            DataTable dt = ds.Tables[0];
            usertable.Table = dt;
            Result = ds.Tables[1].Rows[0][0].ToString();
            return usertable;

        }

        // This Method added by lalitha on 30/04/2019

        public UserDataTable GetOpenVLPDNosForSorting(BO.VLPD vLPD)
        {
            UserDataTable usertable = new UserDataTable();
            DataSet ds = DB.GetDS("EXEC [dbo].[SP_GET_VLPD_SortageVLPDList] ", false);
            DataTable dt = ds.Tables[0];
            usertable.Table = dt;
            return usertable;

        }


        public UserDataTable VLPDSorting(BO.VLPD vLPD)
        {
            UserDataTable usertable = new UserDataTable();
            string sql = "EXEC [dbo].[SP_GET_VLPD_SortageVLPDList] @VLPDNUMBER=" + vLPD.VLPDNo + ",@SKU=" + vLPD.MCode + ",@BATCHNO=" + vLPD.BatchNo + ",@PROJECTREF=" + vLPD.ProjectRefNo + ",@SERIALNO=" + vLPD.SerialNo + ",@MFGDATE=" + vLPD.MfgDate + ",@EXPDATE=" + vLPD.ExpDate + ",@MRP='',@QTY=" + vLPD.PickedQty;
            DataSet ds = DB.GetDS(sql, false);
            DataTable dt = ds.Tables[0];
            usertable.Table = dt;
            return usertable;

        }
    }
}