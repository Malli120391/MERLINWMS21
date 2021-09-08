using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MRLWMSC21Common;
using System.Text;

namespace MRLWMSC21WCF.BL
{
    public class OutboundHanlder
    {
        public UserDataTable GetOBDnos(BO.Outbound outbound)
        {
            UserDataTable usertable = new UserDataTable();
            DataSet ds = DB.GetDS("EXEC [dbo].[Get_OBDNoList] @AccountId=" + outbound.AccountId, false);
            DataTable dt = ds.Tables[0];
            usertable.Table = dt;
            return usertable;

        }
        public UserDataTable GetSONos(BO.Outbound outbound)
        {
            UserDataTable usertable = new UserDataTable();
            DataSet ds = DB.GetDS("EXEC [dbo].[sp_INB_SOInvoiceNos_HHT] @OBDNO=" + DB.SQuote(outbound.Obdno), false);
            DataTable dt = null;
            if (ds != null)
                dt = ds.Tables[0];
            usertable.Table = dt;
            return usertable;

        }
        public BO.Outbound GetAvailbleQtyList(BO.Outbound outbound)
        {

            DataSet dataset;


            dataset = DB.GetDS("EXEC [dbo].[sp_INV_GetAvailableQty_HHT]  @Mcode=" + DB.SQuote(outbound.MCode) + ", @Location=" + DB.SQuote(outbound.Location) + ",@CartonCode=" + DB.SQuote(outbound.CartonNo) + ",@MfgDate=" + (outbound.MfgDate != "" ? DB.SQuote(outbound.MfgDate) : "''") + ",@ExpDate=" + (outbound.ExpDate != "" ? DB.SQuote(outbound.ExpDate) : "''") + ",@SerialNo=" + (outbound.SerialNo != "" ? DB.SQuote(outbound.SerialNo) : "''") + ",@BatchNo = " + (outbound.BatchNo != "" ? DB.SQuote(outbound.BatchNo) : "''") + ",@ProjectRefNo = " + (outbound.ProjectNo != "" ? DB.SQuote(outbound.ProjectNo) : "''"), false);
            if (dataset != null && dataset.Tables[0] != null && dataset.Tables[0].Rows.Count != 0)
            {
                outbound.AvailQty = dataset.Tables[0].Rows[0]["AvailableQuantity"].ToString();
            }
            return outbound;

        }
        public BO.Outbound GetOBDNo(BO.Outbound outbound)
        {
            try
            {
                outbound = GetOBDID(outbound);
                outbound = GetOBDQty(outbound);
                return outbound;
            }
            catch (Exception ex)
            {
                return outbound;
            }
        }
        public static BO.Outbound GetOBDID(BO.Outbound outbound)
        {
            int id = 0;
            id = DB.GetSqlN("select OutboundID AS N from OBD_Outbound where OBDNumber=" + DB.SQuote(outbound.Obdno));
            outbound.OutboundId = id.ToString();
            return outbound;
        }

        public BO.Outbound GetOBDQty(BO.Outbound outbound)
        {
            DataSet ds = DB.GetDS("EXEC [dbo].[Get_OutboundQty_HHT] @OutboundNo=" + DB.SQuote(outbound.Obdno), false);
            if (ds.Tables[0].Rows.Count != 0)
            {
                outbound.OBDQty = Convert.ToDecimal(ds.Tables[0].Rows[0]["SOQuantity"].ToString());
            }
            return outbound;
        }

        public BO.Outbound GetOBDItemToPick(BO.Outbound outbound)
        {

            string query = " EXEC [dbo].[GEN_OBDWISEDEATILS_HHT] @OBDID=" + outbound.OutboundId;
            try
            {
                DataSet ds = DB.GetDS(query, false);

                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count != 0)
                {
                    outbound.Assignedid = Convert.ToInt32(ds.Tables[0].Rows[0]["AssignedID"].ToString());
                    outbound.MaterialMasterId = ds.Tables[0].Rows[0]["MaterialMasterID"].ToString();
                    outbound.MCode = ds.Tables[0].Rows[0]["MCode"].ToString();
                    outbound.MDescription = ds.Tables[0].Rows[0]["MDescription"].ToString();
                    outbound.CartonNo = ds.Tables[0].Rows[0]["CartonCode"].ToString();
                    outbound.CartonId = ds.Tables[0].Rows[0]["CartonID"].ToString();
                    outbound.Location = ds.Tables[0].Rows[0]["Location"].ToString();
                    outbound.LocationId = Convert.ToInt32(ds.Tables[0].Rows[0]["LocationID"].ToString());
                    outbound.MfgDate = ds.Tables[0].Rows[0]["MfgDate"].ToString();
                    outbound.ExpDate = ds.Tables[0].Rows[0]["ExpDate"].ToString();
                    outbound.SerialNo = ds.Tables[0].Rows[0]["SerialNo"].ToString();
                    outbound.BatchNo = ds.Tables[0].Rows[0]["BatchNo"].ToString();
                    outbound.ProjectNo = ds.Tables[0].Rows[0]["ProjectRefNo"].ToString();
                    outbound.AssignedQuantity = ds.Tables[0].Rows[0]["AssignedQuantity"].ToString();
                    outbound.PickedQty = ds.Tables[0].Rows[0]["PickedQty"].ToString();
                    outbound.OutboundId = ds.Tables[0].Rows[0]["OutboundID"].ToString();
                    outbound.SODetailsID = ds.Tables[0].Rows[0]["SODetailsID"].ToString();
                    outbound.SLocId = ds.Tables[0].Rows[0]["StorageLocationID"].ToString();
                    outbound.SLoc = ds.Tables[0].Rows[0]["SLOC"].ToString();
                    outbound.GoodsmomentDeatilsId = Convert.ToInt32(ds.Tables[0].Rows[0]["GoodsMovementDetailsID"].ToString());
                    outbound.Lineno = ds.Tables[0].Rows[0]["LineNumber"].ToString();
                    outbound.MaterialMaster_IUoMID = ds.Tables[0].Rows[0]["MaterialMaster_SUoMID"].ToString();
                    outbound.CF = Convert.ToDecimal(ds.Tables[0].Rows[0]["CF"].ToString());
                    outbound.POSOHeaderId = ds.Tables[0].Rows[0]["SOHeaderID"].ToString();
                    outbound.PendingQty = ds.Tables[0].Rows[0]["PendingQty"].ToString();
                    outbound.MRP = ds.Tables[0].Rows[0]["MRP"].ToString();
                    outbound.HUNo= Convert.ToInt32(ds.Tables[0].Rows[0]["HUNo"].ToString());
                    outbound.HUSize = Convert.ToInt32(ds.Tables[0].Rows[0]["HUSize"].ToString());

                }
                else
                {
                    outbound.PendingQty = "0";
                }
                return outbound;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        public void CheckItem(BO.Outbound outbound)
        {
            DB.GetSqlN("EXEC [dbo].[SP_ChangeGateEntryStatus] @ShipmentId =" + outbound.OutboundId + ",@IsForInbound =0");
        }
        public BO.Outbound RegeneratePickItemsuggestionForOBD(BO.Outbound outbound)
        {
            string query = " EXEC [dbo].[Sp_OBD_UpsertPickingSuggestions] @OutboundID=" + Convert.ToInt32(outbound.OutboundId) + ",@UserID=" + outbound.CreatedBy;
            DataSet ds = DB.GetDS(query, false);

            //if (ds.Tables[0].Rows[0]["N"].ToString() == "-1")
            //{
            //    outbound.Erocode = "-1";

            //}
            //else
            //{
            //    outbound.Erocode = "1";
            //}
            //outbound.Result = (ds.Tables[1].Rows[0]["ErrorMessage"].ToString());

            return outbound;
        }


        //if (ds != null && ds.Tables[1] != null && ds.Tables[1].Rows.Count != 0)
        //{

        //    string result = ds.Tables[1].Rows[0]["ErrorCodes"].ToString() == "" ? "0" : ds.Tables[1].Rows[0]["ErrorCodes"].ToString();
        //    switch (result)
        //    {
        //        case "-1":
        //            outbound.Result = "No Qty for Release Outbound";
        //            break;
        //        case ",1,":
        //            outbound.Result = "No materials in pick list";
        //            break;
        //        case ",2,":
        //            outbound.Result = "No Active stock is available for this shipment.";
        //            break;
        //        case ",3,":
        //            outbound.Result = " Partial fulfilment of inventory";
        //            break;
        //        case "-10":
        //            outbound.Result = "Material is not available in OK Location";
        //            break;
        //        case "-11":
        //            outbound.Result = " Material is available at Location which is blocked for cycle count";
        //            break;
        //        case "-12":
        //            outbound.Result = " Expired Material";
        //            break;
        //        case "-13":
        //            outbound.Result = "Material with Shelf life expiry";
        //            break;
        //        case "-14":
        //            outbound.Result = "Material which is already allocated & skipped";
        //            break;
        //        case "0":
        //            outbound.Result = "";
        //            break;
        //    }
        //}
        //else
        //{
        //    outbound.Result = "";

        //}
    
          
        
        public BO.Outbound OBDSkipItem(BO.Outbound outbound)
        {
            //string drlStatement = "EXEC [dbo].[USP_SET_OBD_SKIP]  @VLPDAssignID=" + Convert.ToInt32(outbound.Assignedid) + ",@MCode=" + DB.SQuote(outbound.MCode) + ",@MfgDate=" + (outbound.MfgDate != "" ? DB.SQuote(outbound.MfgDate) : "''") + ",@ExpDate=" + (outbound.ExpDate != "" ? DB.SQuote(outbound.ExpDate) : "''") + ",@SerialNo=" + (outbound.SerialNo != "" ? DB.SQuote(outbound.SerialNo) : "''") + ",@BatchNo = " + (outbound.BatchNo != "" ? DB.SQuote(outbound.BatchNo) : "''") + ",@ProjectRefNo = " + (outbound.ProjectNo != "" ? DB.SQuote(outbound.ProjectNo) : "''") + ",@SkipQty = " + (outbound.SkipQty) + ", @CreatedBy=" + Convert.ToInt32(outbound.CreatedBy) + ",@CartonCode=" + DB.SQuote(outbound.CartonNo) + ",@Location=" + DB.SQuote(outbound.Location) + ",@Reason=" + DB.SQuote(outbound.SkipReason) + ",@VLPDID=" + outbound.OutboundId;
            string drlStatement = "EXEC [dbo].[USP_SET_OBD_SKIP]  @VLPDAssignID=" + Convert.ToInt32(outbound.Assignedid) + ",@Reason=" + DB.SQuote(outbound.SkipReason) + ",@CreatedBy=" + outbound.CreatedBy;

            string result = DB.GetSqlS(drlStatement);
            if(result=="1")
            {
                outbound.Result = "Success";
            }
            else
            {
                outbound.Result = result;
            }
        
            if (outbound.Flag == 1)
            {
               RegeneratePickItemsuggestionForOBD(outbound);
                return outbound;
            }

            return outbound;
        }
        public UserDataTable GetOBDPickedList(BO.Outbound outbound ,out string Result)
        {

            UserDataTable usertable = new UserDataTable();
            string sql = "[dbo].[sp_GET_VLPD_PickedInfoList_HHT] @OutboundID=" + outbound.OutboundId + ",@MCode=" + DB.SQuote(outbound.MCode) + ",@MFGDate=" + DB.SQuote(outbound.MfgDate) + ",@EXPDate=" + DB.SQuote(outbound.ExpDate);
            sql += ",@Batch=" + DB.SQuote(outbound.BatchNo) + ",@SerialNo=" + DB.SQuote(outbound.SerialNo) + ",@ProjectRef=" + DB.SQuote(outbound.ProjectNo) + ",@MRP=" + DB.SQuote(outbound.MRP);
            DataSet ds = DB.GetDS(sql, false);
            DataTable dt = ds.Tables[0];
            usertable.Table = dt;
            Result = ds.Tables[1].Rows[0][0].ToString();
            return usertable;

        }

        public BO.Outbound UpdatePickItem(BO.Outbound outbound)
        {
            try
            {
                StringBuilder sCmdUpsertPOQuantity = new StringBuilder();


                if (outbound.HUNo > 1)
                {

                    outbound.GoodsMovementDetailsID = "0";
                    outbound.GoodsMovementTypeID = "2";
                    outbound.IsPostiveRecall = "0";
                    outbound.IsDam = "0";
                    outbound.HasDisc = "0";

                    outbound.QtyinBUoM = Convert.ToDecimal(outbound.Qty * outbound.CF);
                   
                    sCmdUpsertPOQuantity.Append("Exec [dbo].[SP_Upsert_HU_OBD_Items_Picking] ");
                    sCmdUpsertPOQuantity.Append("@OBDNumber=" + DB.SQuote(outbound.Obdno));
                    sCmdUpsertPOQuantity.Append(",@LineNumber=" + outbound.Lineno);
                    sCmdUpsertPOQuantity.Append(",@SOHeaderID=" + outbound.POSOHeaderId);
                    sCmdUpsertPOQuantity.Append(",@MCode=" + DB.SQuote(outbound.MCode));
                    sCmdUpsertPOQuantity.Append(",@Location=" + DB.SQuote(outbound.Location));
                    sCmdUpsertPOQuantity.Append(",@Quantity=" + outbound.Qty);
                    sCmdUpsertPOQuantity.Append(",@IsDamaged=" + outbound.IsDam);
                    sCmdUpsertPOQuantity.Append(",@HasDiscrepancy=" + outbound.HasDisc);
                    sCmdUpsertPOQuantity.Append(",@CreatedBy=" + outbound.CreatedBy);
                    sCmdUpsertPOQuantity.Append(",@MfgDate=" + DB.DateQuote(outbound.MfgDate));
                    sCmdUpsertPOQuantity.Append(",@ExpDate=" + DB.DateQuote(outbound.ExpDate));
                    sCmdUpsertPOQuantity.Append(",@SerialNo=" + DB.SQuote(outbound.SerialNo));
                    sCmdUpsertPOQuantity.Append(",@BatchNo=" + DB.SQuote(outbound.BatchNo));
                    sCmdUpsertPOQuantity.Append(",@Projrefno=" + DB.SQuote(outbound.ProjectNo));
                    sCmdUpsertPOQuantity.Append(",@CartonCode =" + DB.SQuote(outbound.CartonNo));
                    sCmdUpsertPOQuantity.Append(",@ToCartonCode =" + DB.SQuote(outbound.ToCartonNo));
                    sCmdUpsertPOQuantity.Append(",@AssignedId =" + outbound.Assignedid);
                    sCmdUpsertPOQuantity.Append(",@SoDetailsIdnew=" + outbound.SODetailsID);
                    sCmdUpsertPOQuantity.Append(",@MRP=" + DB.SQuote(outbound.MRP));
                    sCmdUpsertPOQuantity.Append(",@AccountID=" + outbound.AccountId);
                    sCmdUpsertPOQuantity.Append(",@HUSize=" + outbound.HUSize);
                    sCmdUpsertPOQuantity.Append(",@HUNo=" + outbound.HUNo);

                }

                else
                {

                    outbound.GoodsMovementDetailsID = "0";
                    outbound.GoodsMovementTypeID = "2";
                    outbound.IsPostiveRecall = "0";
                    outbound.IsDam = "0";
                    outbound.HasDisc = "0";

                    outbound.QtyinBUoM = Convert.ToDecimal(outbound.Qty * outbound.CF);

                    

                    sCmdUpsertPOQuantity.Append("Exec [dbo].[sp_INV_PickItemFromBin] ");
                    sCmdUpsertPOQuantity.Append("@OBDNumber=" + DB.SQuote(outbound.Obdno));
                    sCmdUpsertPOQuantity.Append(",@LineNumber=" + outbound.Lineno);
                    sCmdUpsertPOQuantity.Append(",@SOHeaderID=" + outbound.POSOHeaderId);
                    sCmdUpsertPOQuantity.Append(",@MCode=" + DB.SQuote(outbound.MCode));
                    sCmdUpsertPOQuantity.Append(",@Location=" + DB.SQuote(outbound.Location));
                    sCmdUpsertPOQuantity.Append(",@Quantity=" + outbound.Qty);
                    sCmdUpsertPOQuantity.Append(",@IsDamaged=" + outbound.IsDam);
                    sCmdUpsertPOQuantity.Append(",@HasDiscrepancy=" + outbound.HasDisc);
                    sCmdUpsertPOQuantity.Append(",@CreatedBy=" + outbound.CreatedBy);
                    sCmdUpsertPOQuantity.Append(",@MfgDate=" + DB.DateQuote(outbound.MfgDate));
                    sCmdUpsertPOQuantity.Append(",@ExpDate=" + DB.DateQuote(outbound.ExpDate));
                    sCmdUpsertPOQuantity.Append(",@SerialNo=" + DB.SQuote(outbound.SerialNo));
                    sCmdUpsertPOQuantity.Append(",@BatchNo=" + DB.SQuote(outbound.BatchNo));
                    sCmdUpsertPOQuantity.Append(",@Projrefno=" + DB.SQuote(outbound.ProjectNo));
                    sCmdUpsertPOQuantity.Append(",@CartonCode =" + DB.SQuote(outbound.CartonNo));
                    sCmdUpsertPOQuantity.Append(",@ToCartonCode =" + DB.SQuote(outbound.ToCartonNo));
                    sCmdUpsertPOQuantity.Append(",@AssignedId =" + outbound.Assignedid);
                    sCmdUpsertPOQuantity.Append(",@SoDetailsIdnew=" + outbound.SODetailsID);
                    sCmdUpsertPOQuantity.Append(",@MRP=" + DB.SQuote(outbound.MRP));
                    sCmdUpsertPOQuantity.Append(",@AccountID=" + outbound.AccountId);
                    sCmdUpsertPOQuantity.Append(",@HUSize=" + outbound.HUSize);
                    sCmdUpsertPOQuantity.Append(",@HUNo=" + outbound.HUNo);                    
                }

                int Result = DB.GetSqlN(sCmdUpsertPOQuantity.ToString());

                if (Result == -999)
                {

                    outbound.Result = "Qty Exceeded";


                }

                else if (Result == 1)
                {
                    outbound.Result = "Success";
                    CheckItem(outbound);
                    GetOBDItemToPick(outbound);

                }
                else if (Result == -444)
                {
                    outbound.Result = Result.ToString();
                }
                else if (Result == -333)
                {
                    outbound.Result = Result.ToString();
                }
                else if (Result == 2)
                {
                    outbound.Result = "Stock not Availble";
                }
                else
                {
                    outbound.Result = "Process failed,Please Contact Support team";

                }
                return outbound;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            }

       

        public string CheckContainerOBD(string CartonNo, string OutboundID)
        {
            string resultvalue;
            string sql = "select CartonID AS N from INV_Carton where CartonCode=" + DB.SQuote(CartonNo) + " and IsActive=1 and WareHouseID=(select WarehouseID from OBD_RefWarehouse_Details where OutboundID=" + OutboundID + ")";
            int result = DB.GetSqlN(sql);
            if (result != 0)
            {
                resultvalue = "";
            }
            else
            {
                resultvalue = "Not a valid Container.";
            }
            return resultvalue;
        }
    }
}