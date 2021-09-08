using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MRLWMSC21Common;
using System.Data;
using System.Text;

namespace MRLWMSC21WCF.BL
{
    public class PutAwayHandler
    {


        public BO.PutAway GetItemTOPutAway(BO.PutAway PutAway)
        {
            if (PutAway.TransferRequestId != 0)
            {
                PutAway.InboundId = "0";

                string query = " EXEC [dbo].[GEN_PUTAWAY_ITEMS_ALLOCATED_LIST_INBWISE_HHT] @INBOUNDID=" + Convert.ToInt32(PutAway.InboundId) + ",@TransferRequestID=" + PutAway.TransferRequestId;
                try
                {
                    DataSet ds = DB.GetDS(query, false);

                    if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count != 0)
                    {

                        PutAway.SuggestedPutawayID = Convert.ToInt32(ds.Tables[0].Rows[0]["SuggestedPutawayID"].ToString());
                        PutAway.MaterialMasterId = Convert.ToInt32(ds.Tables[0].Rows[0]["MaterialMasterID"].ToString());
                        PutAway.MCode = ds.Tables[0].Rows[0]["MCode"].ToString();
                        PutAway.MDescription = ds.Tables[0].Rows[0]["MDescription"].ToString();
                        PutAway.CartonCode = ds.Tables[0].Rows[0]["CartonCode"].ToString();
                        PutAway.CartonID = Convert.ToInt32(ds.Tables[0].Rows[0]["CartonID"].ToString());
                        PutAway.Location = ds.Tables[0].Rows[0]["Location"].ToString();
                        PutAway.LocationID = Convert.ToInt32(ds.Tables[0].Rows[0]["LocationID"].ToString());
                        PutAway.MfgDate = ds.Tables[0].Rows[0]["MfgDate"].ToString();
                        PutAway.ExpDate = ds.Tables[0].Rows[0]["ExpDate"].ToString();
                        PutAway.SerialNo = ds.Tables[0].Rows[0]["SerialNo"].ToString();
                        PutAway.BatchNo = ds.Tables[0].Rows[0]["BatchNo"].ToString();
                        PutAway.ProjectRefNo = ds.Tables[0].Rows[0]["ProjectRefNo"].ToString();
                        PutAway.AssignedQuantity = ds.Tables[0].Rows[0]["InvoiceQty"].ToString();
                        PutAway.SuggestedQty = Convert.ToDecimal(ds.Tables[0].Rows[0]["SuggestedQty"].ToString());
                        PutAway.SuggestedReceivedQty = Convert.ToDecimal(ds.Tables[0].Rows[0]["SuggestedReceivedQty"].ToString());
                        PutAway.SuggestedRemainingQty = Convert.ToDecimal(ds.Tables[0].Rows[0]["SuggestedRemainingQty"].ToString());
                        PutAway.TransferRequestDetailsId = Convert.ToInt32(ds.Tables[0].Rows[0]["TransferRequestDetailsID"].ToString());
                        PutAway.PickedLocationID = Convert.ToInt32(ds.Tables[0].Rows[0]["PickedLocationID"].ToString());
                        PutAway.GMDRemainingQty = Convert.ToDecimal(ds.Tables[0].Rows[0]["PickedQty"].ToString());
                        PutAway.Dock = ds.Tables[0].Rows[0]["DOCK"].ToString();
                        PutAway.StorageLocation = ds.Tables[0].Rows[0]["StorageCode"].ToString();
                        PutAway.PutAwayQty = "1";
                    }
                    else
                    {
                        PutAway.PutAwayQty = "0";

                    }
                    return PutAway;
                }
                catch (Exception ex)
                {
                    throw ex;

                }


            }
            else
            {
                string query = " EXEC [dbo].[GEN_PUTAWAY_ITEMS_ALLOCATED_LIST_INBWISE_HHT_NEW] @INBOUNDID=" + Convert.ToInt32(PutAway.InboundId);
                try
                {
                    DataSet ds = DB.GetDS(query, false);

                    if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count != 0)
                    {

                        PutAway.SuggestedPutawayID = Convert.ToInt32(ds.Tables[0].Rows[0]["SuggestedPutawayID"].ToString());
                        PutAway.MaterialMasterId = Convert.ToInt32(ds.Tables[0].Rows[0]["MaterialMasterID"].ToString());
                        PutAway.MCode = ds.Tables[0].Rows[0]["MCode"].ToString();
                        PutAway.CartonCode = "0";
                        PutAway.Location = ds.Tables[0].Rows[0]["Location"].ToString();
                        PutAway.LocationID = Convert.ToInt32(ds.Tables[0].Rows[0]["LocationID"].ToString());
                        PutAway.MfgDate = ds.Tables[0].Rows[0]["MfgDate"].ToString();
                        PutAway.ExpDate = ds.Tables[0].Rows[0]["ExpDate"].ToString();
                        PutAway.SerialNo = ds.Tables[0].Rows[0]["SerialNo"].ToString();
                        PutAway.BatchNo = ds.Tables[0].Rows[0]["BatchNo"].ToString();
                        PutAway.ProjectRefNo = ds.Tables[0].Rows[0]["ProjectRefNo"].ToString();
                        PutAway.MRP = ds.Tables[0].Rows[0]["MRP"].ToString();
                        PutAway.StorageLocation = ds.Tables[0].Rows[0]["StorageCode"].ToString();
                        PutAway.SuggestedQty = Convert.ToDecimal(ds.Tables[0].Rows[0]["SuggestedQty"].ToString());
                        PutAway.SuggestedRemainingQty= Convert.ToDecimal(ds.Tables[0].Rows[0]["SuggestedRemainingQty"].ToString());
                        PutAway.SuggestedReceivedQty = Convert.ToDecimal(ds.Tables[0].Rows[0]["SuggPutQTY"].ToString());
                        PutAway.ReceivedQuantity = Convert.ToDecimal(ds.Tables[0].Rows[0]["RemainingQTY"].ToString());
                        PutAway.GMDRemainingQty = Convert.ToDecimal(ds.Tables[0].Rows[0]["GMDQTY"].ToString());
                        PutAway.Dock = ds.Tables[0].Rows[0]["Dock"].ToString();
                        PutAway.PutAwayQty = "1";
                        PutAway.Result = "3";

                    }
                    else
                    {
                        // PutAway.PutAwayQty = "0";
                        string sql = "EXEC INB_Verify_Putaway @InboundID=" + PutAway.InboundId;
                        PutAway.Result = DB.GetSqlS(sql);

                    }
                    return PutAway;
                }
                catch (Exception ex)
                {
                    throw ex;

                }
            }

        }
        public BO.PutAway RegenerateSuggestedItem(BO.PutAway PutAway)
        {
            string query = " EXEC [dbo].[USP_TRN_GeneratePutawaySuggestions] @InboundID=" + Convert.ToInt32(PutAway.InboundId) + ",@TransferRequestID=" + PutAway.TransferRequestId + ",@SuggestionID=" + PutAway.SuggestedPutawayID + ",@SuggestionQtyFulfilled=" + Convert.ToDecimal(PutAway.SuggestedReceivedQty) + ",@ReasonID=" + 1;
            DB.ExecuteSQL(query);
            return PutAway;
        }


        public BO.PutAway SkipItem(BO.PutAway putAway)
        {
            string drlStatement = "EXEC [dbo].[UpsertSkipItem_HHT]  @SuggestedId=" + Convert.ToInt32(putAway.SuggestedPutawayID) + ",@MCode=" + DB.SQuote(putAway.MCode) + ",@MfgDate="  +(putAway.MfgDate != "" ? DB.SQuote(putAway.MfgDate) : "''") + ",@ExpDate="  +(putAway.ExpDate != "" ? DB.SQuote(putAway.ExpDate) : "''") + ",@SerialNo=" + (putAway.SerialNo != "" ? DB.SQuote(putAway.SerialNo) : "''") + ",@BatchNo = "  +(putAway.BatchNo != "" ? DB.SQuote(putAway.BatchNo) : "''") + ",@ProjectRefNo = "  +(putAway.ProjectRefNo != "" ? DB.SQuote(putAway.ProjectRefNo) : "''") + ",@Qty = " + (putAway.SkipQty) + ", @CreatedBy=" + Convert.ToInt32(putAway.UserId) + ",@StorageLocation=" + DB.SQuote(putAway.StorageLocation) + ",@CartonCode=" + DB.SQuote(putAway.CartonCode) + ",@Location=" + DB.SQuote(putAway.Location) + ",@Reason=" + DB.SQuote(putAway.Skipreason) + ",@InboundId="+putAway.InboundId+",@TypeId=" + 1;
            int result = DB.GetSqlN(drlStatement);
            putAway.Result = result.ToString();
            if (putAway.Flag == 1)
            {
                RegenerateSuggestedItem(putAway);
            }
            return putAway;
        }
        public BO.PutAway UpsertPutAwayItem(BO.PutAway PutAway)
        {
            try {
                decimal qty = Convert.ToDecimal(PutAway.PutAwayQty);
                if (qty > PutAway.TotalQuantity)
                {
                    PutAway.Result = "Qty. should be less than putaway qty.";
                    return PutAway;
                }

                string valid = CheckValidPutAwayItem(PutAway);
                if (valid == "1")
                {
                    int remainingqty = CheckPutAwayItemRemainQty(PutAway);
                    if (remainingqty > 0)
                    {
                        PutAway.Result = " putaway could not perform, there is " + remainingqty + " Qty. has been received";
                        return PutAway;
                    }
                    else if (remainingqty == -999)
                    {
                        PutAway.Result = " Putaway could not perform";
                        return PutAway;
                    }
                    if (PutAway.TransferRequestId != 0)
                    {
                        UpdateInternalTransferReceiveItem(PutAway);
                        PutAway.Result = "";
                        GetItemTOPutAway(PutAway);
                        return PutAway;
                    }
                    else
                    {
                        string drlStatement = "EXEC [dbo].[UpsertPutAway_HHT]  @INBOUNDID=" + Convert.ToInt32(PutAway.InboundId) + ",@SuggestedPutAwayID=" + PutAway.SuggestedPutawayID + ",@MCode=" + DB.SQuote(PutAway.MCode) + ",@Quantity=" + Convert.ToDecimal(PutAway.PutAwayQty) + ",@SuggesstedLocation=" + DB.SQuote(PutAway.Location) + ",@CartonCode=" + DB.SQuote(PutAway.CartonCode) + ",@CreatedBy=" + Convert.ToInt32(PutAway.UserId) + ",@StorageLocation=" + DB.SQuote(PutAway.StorageLocation);
                        drlStatement += ",@ScannedLocation="+DB.SQuote(PutAway.ScnnedLocation);
                        int result = DB.GetSqlN(drlStatement);
                        switch (result)
                        {
                            case -9999:
                                PutAway.Result = "Invalid carton scanned";
                             
                                return PutAway;
                            case -9998:
                                PutAway.Result = "This carton available in other location";
                              
                                return PutAway;
                            case -9997:
                                PutAway.Result = "Putaway quantity exceeded";
                                return PutAway;
                            case 1:
                                PutAway.Result = "";
                                GetItemTOPutAway(PutAway);
                                return PutAway;
                        }
                    }
                }
                else
                {
                    PutAway.Result = "Scan valid SKU";
                }
                return PutAway;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private string CheckValidPutAwayItem(BO.PutAway PutAway)
        {
            decimal RecieveQty=0;
            string res,sql;
            sql = "EXEC [dbo].[Get_ReceivedQty_HHT] @INBOUNDID=" + PutAway.InboundId + ",@Mcode=" + DB.SQuote(PutAway.MCode) + ", @LineNo = " + DB.SQuote(PutAway.Lineno) + ", @MfgDate = " + (PutAway.MfgDate != "" ? DB.SQuote(PutAway.MfgDate) : "''") + ", @ExpDate = " + (PutAway.ExpDate != "" ? DB.SQuote(PutAway.ExpDate) : "''") + ", @SerialNo = " + (PutAway.SerialNo != "" ? DB.SQuote(PutAway.SerialNo) : "''") + ", @BatchNo = " + (PutAway.BatchNo != "" ? DB.SQuote(PutAway.BatchNo) : "''") + ", @ProjectRefNo = " + (PutAway.ProjectRefNo != "" ? DB.SQuote(PutAway.ProjectRefNo) : "''") + ", @MRP = " + (PutAway.MRP != "" ? DB.SQuote(PutAway.MRP) : "''");
            DataSet ds = DB.GetDS(sql, false);
            if (ds.Tables[0].Rows.Count != 0)
            {
                RecieveQty =Convert.ToDecimal( ds.Tables[0].Rows[0]["ReceivedQty"]);
            }
            res =(RecieveQty> 0) ? "1" : "0";

            return res;
        }

        private int CheckPutAwayItemRemainQty(BO.PutAway PutAway)
        {
            int RemainingQty = 0;
            string  sql;
            sql = "EXEC [dbo].[INB_CheckPutawayQty] @INBOUNDID=" + PutAway.InboundId + ",@Mcode=" + DB.SQuote(PutAway.MCode) + ", @LineNo = " + DB.SQuote(PutAway.Lineno) + ", @MfgDate = " + (PutAway.MfgDate != "" ? DB.SQuote(PutAway.MfgDate) : "''") + ", @ExpDate = " + (PutAway.ExpDate != "" ? DB.SQuote(PutAway.ExpDate) : "''") + ", @SerialNo = " + (PutAway.SerialNo != "" ? DB.SQuote(PutAway.SerialNo) : "''") + ", @BatchNo = " + (PutAway.BatchNo != "" ? DB.SQuote(PutAway.BatchNo) : "''") + ", @ProjectRefNo = " + (PutAway.ProjectRefNo != "" ? DB.SQuote(PutAway.ProjectRefNo) : "''") + ", @MRP = " + (PutAway.MRP != "" ? DB.SQuote(PutAway.MRP) : "''");
            RemainingQty = DB.GetSqlN(sql);

            if (RemainingQty != 0)
            {
                    return Convert.ToInt32(PutAway.PutAwayQty) - RemainingQty;
            }
            else
            { return -999;
            }
           
     
        }


        public BO.PutAway UpdateInternalTransferReceiveItem(BO.PutAway PutAway)
        {
            string drlStatement = "EXEC [dbo].[INTERNALSTOCK_TRANSFER_ITEM]   @TransferRequestID=" + Convert.ToInt32(PutAway.TransferRequestId) + ",@TransferRequestDetailsID=" + PutAway.TransferRequestDetailsId + ",@FromLocationID=" + PutAway.PickedLocationID + ",@BatchNo=" + DB.SQuote(PutAway.BatchNo) + ",@CreatedBy=" + PutAway.UserId + ",@Carton=" + DB.SQuote(PutAway.CartonCode) + ",@MCODE=" + DB.SQuote(PutAway.MCode) + ",@TLoc=" + DB.SQuote(PutAway.Location) + ",@Quantity=" + Convert.ToDecimal(PutAway.PutAwayQty) + ",@FullFilledQty=" + PutAway.TotalQuantity;
            int result = DB.GetSqlN(drlStatement);
            return PutAway;

        }
        public BO.PutAway GetCurrentLocationForPutaway(BO.PutAway putAway)
        {
            string putawaylocation = DB.GetSqlS("EXEC [dbo].[SP_CHECK_CONTAINER_LOCATION_MAPPING]  @CONTAINER=" + DB.SQuote(putAway.CartonCode));
            if (putawaylocation != null && putawaylocation != "")
            {
                putAway.Location = putawaylocation;
            }
            return putAway;

        }

        public BO.PutAway CheckPutAwayItemQty(BO.PutAway PutAway)
        {

            string drlStatement = "EXEC [dbo].[Check_PutAwayItem] @INBOUNDID=" + Convert.ToInt32(PutAway.InboundId) + ",@MaterialMasterId=" + PutAway.MaterialMasterId + ",@PartialPutAwayQty=" + Convert.ToDecimal(PutAway.PutAwayQty);
            int result = DB.GetSqlN(drlStatement);
            switch (result)
            {
                case -1:
                    PutAway.Result = "PutAway Qty is greater than the Received Qty.";
                    return PutAway;
                case -2:
                    PutAway.Result = "This Item not yet received";
                    return PutAway;
                case 1:
                    PutAway.Result = "";
                    return PutAway;
            }  
            
        
               
            return PutAway;
        }
        public string ChekContainerLocation(string cartoncode,string WarehouseID)
        {
            
            string Location = DB.GetSqlS("EXEC [dbo].[SP_CHECK_CONTAINER_LOCATION_MAPPING]  @CONTAINER=" + DB.SQuote(cartoncode)+ ",@WarehouseID="+ WarehouseID);
           return Location;
        }
        public BO.PutAway GetTransferContainer(BO.PutAway putAway)
        {

            string Container = DB.GetSqlS("EXEC [dbo].[Check_PickedContainer]  @TRASFRREQUESID=" + putAway.TransferRequestId);
            if(Container.Equals(putAway.CartonCode))
            {
                 putAway.Result = "From, To and Putaway container should be different";
            }
            else
            {
                putAway.Result = "1";
            }

            return putAway;
        }

        public UserDataTable GetMSPsReceiving(BO.PutAway putAway)
        {
            UserDataTable usertable = new UserDataTable();
            string sql = "EXEC INB_GetMSP_HHT @IndoundID=" + putAway.InboundId + ",@MCode=" + DB.SQuote(putAway.MCode);
            DataSet ds = DB.GetDS(sql, false);
            usertable.Table = ds.Tables[0];
            return usertable;

        }
    }
}