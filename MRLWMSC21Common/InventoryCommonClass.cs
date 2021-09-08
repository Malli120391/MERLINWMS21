using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using MRLWMSC21Common;


namespace MRLWMSC21Common
{
    public class InventoryCommonClass
    {

        public static DataTable GetPOQuantityListForStoreRef(String InboundID, String MaterialMasterID, String POLineNumber, String POHeaderID, String SupplierInvoiceID, string SuggestPutawayID = "")
        {

            //int VMMID = DB.GetSqlN("Select MaterialMasterID AS N from MMT_MaterialMaster where IsActive=1 and IsDeleted=0 and MCode="+DB.SQuote(MaterialCode));

            string sCmdPOQuantity = "dbo.sp_INV_GetPOQuantityListForStoreRef @InboundID=" + InboundID + ",@MaterialMasterID=" + MaterialMasterID + ",@POLineNumber=" + POLineNumber + ",@POHeaderID=" + POHeaderID + ",@SupplierInvoiceID=" + SupplierInvoiceID + ", @SuggestPutawayID =" + SuggestPutawayID;
            DataSet dsPOQuantityList = DB.GetDS(sCmdPOQuantity, false);
            return dsPOQuantityList.Tables[0];
        }

        public static int UpdateGoodsMovementDetails(String GoodsMovementDetailsID, String GoodsMovementTypeID, String Location, String MaterialMaster_IUoMID, String DocQty, String Quantity, String GoodsTransactionID, String SupplierInvoiceID, String LineNumber, String mmid, String IsDamaged, String hasDiscrepancy, String POSOHeaderID, String CreatedBy, String MaterialStorageParameterIDs, String MaterialStorageParameterValues, String Conversionfactor, String remarks, String LastModifiedBy, String IsActive, String KitplannerID, String IsPositiveRecall, String IsNonConformity, String AsIs, String Carton = "0", string StorageLocationID = "0", String MfgDate = "", String ExpDate = "", String SerialNo = "0", String BatchNo = "0", String ProjectRefNo = "0", string SPID = "0")
        {
            // Modified by swamy on 22/May/2020
            return -1;

            //StringBuilder sCmdUpsertPOQuantity = new StringBuilder();
            //sCmdUpsertPOQuantity.Append("DECLARE @NewResult int  ");
            //sCmdUpsertPOQuantity.Append("Exec sp_INV_UpsertGoodsMovementDetails ");
            //sCmdUpsertPOQuantity.Append("@GoodsMovementDetailsID=" + GoodsMovementDetailsID);
            //sCmdUpsertPOQuantity.Append(",@GoodsMovementTypeID=" + GoodsMovementTypeID);
            //sCmdUpsertPOQuantity.Append(" ,@Location=" + (Location != "" && Location != "0" ? DB.SQuote(Location) : "STAGING"));
            //sCmdUpsertPOQuantity.Append(" ,@CartonID =" + Carton);
            //sCmdUpsertPOQuantity.Append(",@Quantity=" + (Quantity != "" ? Quantity : "NULL"));
            //sCmdUpsertPOQuantity.Append(",@TransactionDocID=" + GoodsTransactionID);
            //sCmdUpsertPOQuantity.Append(",@SupplierInvoiceID="+SupplierInvoiceID);
            //sCmdUpsertPOQuantity.Append(",@LineNumber=" + LineNumber);
            //sCmdUpsertPOQuantity.Append(",@IsPositiveRecall="+IsPositiveRecall);
            //sCmdUpsertPOQuantity.Append(",@MaterialMasterID=" + mmid);
            //sCmdUpsertPOQuantity.Append(",@IsDamaged=" + IsDamaged);
            //sCmdUpsertPOQuantity.Append(",@IsNonconformity="+IsNonConformity);
            //sCmdUpsertPOQuantity.Append(",@AsIs="+AsIs);
            //sCmdUpsertPOQuantity.Append(",@POSOHeaderID=" + POSOHeaderID);
            //sCmdUpsertPOQuantity.Append(",@DocQty=" + DocQty);
            //sCmdUpsertPOQuantity.Append(",@MaterialMaster_IUoMID=" + MaterialMaster_IUoMID);
            //sCmdUpsertPOQuantity.Append(",@CreatedBy=" + CreatedBy);
            //sCmdUpsertPOQuantity.Append(",@KitPlannerID=" + (KitplannerID != "0" ? KitplannerID : "NULL"));
            //sCmdUpsertPOQuantity.Append(",@LastModifiedBy=" + LastModifiedBy);
            //sCmdUpsertPOQuantity.Append(",@HasDiscrepancy=" + hasDiscrepancy);
            //sCmdUpsertPOQuantity.Append(",@IsActive=" + IsActive);
            //sCmdUpsertPOQuantity.Append(",@Remarks=" + (remarks != "" ? DB.SQuote(remarks) : "NULL"));
            //sCmdUpsertPOQuantity.Append(",@ConversionFactor=" + DB.SQuote(Conversionfactor));
            //sCmdUpsertPOQuantity.Append(",@MaterialStorageParameterIDs=" + DB.SQuote(MaterialStorageParameterIDs));
            //sCmdUpsertPOQuantity.Append(",@MaterialStorageParameterValues=" + DB.SQuote(MaterialStorageParameterValues));
            //sCmdUpsertPOQuantity.Append(",@StorageLocationID=" + DB.SQuote(StorageLocationID));
            //sCmdUpsertPOQuantity.Append(",@MfgDate=" + DB.DateQuote( MfgDate));
            //sCmdUpsertPOQuantity.Append(",@ExpDate=" + DB.DateQuote( ExpDate));
            //sCmdUpsertPOQuantity.Append(",@SerialNo=" + DB.SQuote(SerialNo));
            //sCmdUpsertPOQuantity.Append(",@BatchNo=" + DB.SQuote(BatchNo));
            //sCmdUpsertPOQuantity.Append(",@ProjectRefNo=" + DB.SQuote(ProjectRefNo));
            //sCmdUpsertPOQuantity.Append(",@SuggestedPutawayID =" + SPID);
            //sCmdUpsertPOQuantity.Append(",@Result=@NewResult output select @NewResult as N");
            //int Result = DB.GetSqlN(sCmdUpsertPOQuantity.ToString());
            //return Result;

        }
        public static IDataReader GetPickItemDetailsForOBDNumber(String Outbound, String MMID, String LineNumber, String LocationID, String kitID, String SOHeaderID)
        {
            String sCmdPickitemDetails = "EXEC dbo.sp_INV_GetSOQuantityListForOBDNumber @OuboundID=" + Outbound + ",@MaterialMasterID=" + MMID + ",@LineNumber=" + LineNumber + ",@LocationID=" + LocationID + ",@kitPlannerID=" + kitID + ",@SOHeaderID=" + SOHeaderID;
            IDataReader drPickItemdetails = DB.GetRS(sCmdPickitemDetails);
            return drPickItemdetails;
        }
        public static int UpdateReturnGoodsMovementDetails(String GoodsMovementDetailsID, String Quantiity, String DocQty, String TransactionDocID, String POSODetialsID, String ConvertionFactor, String MaterialMaster_UoMID, String CreatedBy, String Carton)
        {
            // Modified by swamy on 22/May/2020
            return -1;

            //String sCmdUpdateReturnDetails = "DECLARE @Status int  EXEC  dbo.sp_INV_UpsertRetunGoodsOutDetails @GoodsMovementDetailsID=" + GoodsMovementDetailsID + ",@Quantity=" + Quantiity + ",@DocQty=" + DocQty + ",@TransactionDocID=" + TransactionDocID + ",@POSODetailsID=" + POSODetialsID + ",@ConversionFactor=" + DB.SQuote(ConvertionFactor) + ",@MaterialMaster_IUoMID=" + MaterialMaster_UoMID + ",@CreatedBy=" + CreatedBy + ",@CartonID=" + Carton + ",@Result=@Status output select @Status as N";
            //int Result = DB.GetSqlN(sCmdUpdateReturnDetails);
            //return Result;
        }

        //public static int UpdateGoodsMovementDetails_new(  String Location, String DocQty, String Quantity, String InboundID, String LineNumber, String mmid, String IsDamaged, String hasDiscrepancy, String CreatedBy, String remarks, String Carton, string StorageLocationID,  String UserID ,String MfgDate = "", String ExpDate = "", String SerialNo = "0", String BatchNo = "0", String ProjectRefNo = "0", string SPID = "0", String PoheaderID = "0")
        //{
        //    StringBuilder sCmdUpsertPOQuantity = new StringBuilder();
        //    sCmdUpsertPOQuantity.Append("Exec sp_INV_UpsertGoodsMovementDetails_NEW ");
        //    sCmdUpsertPOQuantity.Append(" @TransactionID="+InboundID+"  ");
        //    sCmdUpsertPOQuantity.Append(" ,@MaterialMasterID=" + mmid + "  ");
        //    sCmdUpsertPOQuantity.Append(" ,@LineNumber=" + LineNumber + "  ");
        //    sCmdUpsertPOQuantity.Append(" ,@POHeaderID=" + PoheaderID + "  ");
        //    sCmdUpsertPOQuantity.Append(" ,@Quantity=" + Quantity + "  ");
        //    sCmdUpsertPOQuantity.Append(" ,@DocQty=" + Quantity + "  ");
        //    sCmdUpsertPOQuantity.Append(" ,@BatchNo=" + (BatchNo != "" ? DB.SQuote(BatchNo) : "NULL"));
        //    sCmdUpsertPOQuantity.Append(" ,@SerialNo="  +(SerialNo != "" ? DB.SQuote(SerialNo) : "NULL"));
        //    sCmdUpsertPOQuantity.Append(" ,@MfgDate=" + (MfgDate != "" ? DB.SQuote(MfgDate) : "NULL"));
        //    sCmdUpsertPOQuantity.Append(" ,@ExpDate=" + (ExpDate != "" ? DB.SQuote(ExpDate) : "NULL"));
        //    sCmdUpsertPOQuantity.Append(" ,@ProjectRefNo=" + (ProjectRefNo != "" ? DB.SQuote(ProjectRefNo) : "NULL"));
        //    sCmdUpsertPOQuantity.Append(" ,@SuggestedPutawayID=" + SPID + "  ");
        //    sCmdUpsertPOQuantity.Append(" ,@Loction=" + DB.SQuote(Location) + "  ");
        //    sCmdUpsertPOQuantity.Append(" ,@CartonCode=" + DB.SQuote(Carton) + "  ");
        //    sCmdUpsertPOQuantity.Append(" ,@StorageLocationID=" + StorageLocationID + "  ");
        //    sCmdUpsertPOQuantity.Append(" ,@IsDamaged=" + IsDamaged + "  ");
        //    sCmdUpsertPOQuantity.Append(" ,@HasDiscrepancy=" + hasDiscrepancy + "  ");
        //    sCmdUpsertPOQuantity.Append(" ,@Remarks=" + (remarks != "" ? DB.SQuote(remarks) : "NULL"));
        //    sCmdUpsertPOQuantity.Append(" ,@GoodsMovementTypeID=1 ");
        //    sCmdUpsertPOQuantity.Append(" ,@CreatedBy=" +UserID + "  ");
        //    int Result = DB.GetSqlN(sCmdUpsertPOQuantity.ToString());
        //    return Result;
        //}



        public static int UpdateGoodsMovementDetails_new(string Location, string DocQty, string Quantity, string InboundID, string LineNumber, string MCode, string IsDamaged, string hasDiscrepancy, string CreatedBy, string Carton, string StorageLocation, string UserID, string MfgDate = "", string ExpDate = "", string SerialNo = "0", string BatchNo = "0", string ProjectRefNo = "0", string SPID = "0", string PoheaderID = "0", string remarks = "", string SupplierInvoiceId = "0", string SkipReasonID = "0", string MRP = "0", string Vehicle = "", string SupplierInvoiceDetailsID = "0")
        {

            return -1;
            //StringBuilder sCmdUpsertPOQuantity = new StringBuilder();
            //sCmdUpsertPOQuantity.Append("Exec sp_INV_UpsertGoodsMovementDetails_NEW ");
            //sCmdUpsertPOQuantity.Append(" @TransactionID=" + InboundID + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@MCode=" + DB.SQuote(MCode) + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@LineNumber=" + ((LineNumber==string.Empty ||LineNumber=="" ||LineNumber==null) ? "''":LineNumber) + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@POHeaderID=" + PoheaderID + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@Quantity=" + Quantity + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@DocQty=" + Quantity + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@BatchNo=" + (BatchNo != "" ? DB.SQuote(BatchNo) : "''"));
            //sCmdUpsertPOQuantity.Append(" ,@SerialNo=" + (SerialNo != "" ? DB.SQuote(SerialNo) : "''"));
            //sCmdUpsertPOQuantity.Append(" ,@MfgDate=" + (MfgDate != "" ? DB.SQuote(MfgDate) : "''"));
            //sCmdUpsertPOQuantity.Append(" ,@ExpDate=" + (ExpDate != "" ? DB.SQuote(ExpDate) : "''"));
            //sCmdUpsertPOQuantity.Append(" ,@ProjectRefNo=" + (ProjectRefNo != "" ? DB.SQuote(ProjectRefNo) : "''"));
            //sCmdUpsertPOQuantity.Append(" ,@MRP=" + ( MRP!= "" ? DB.SQuote(MRP) : "''"));
            //sCmdUpsertPOQuantity.Append(" ,@SuggestedPutawayID=" + SPID + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@Loction=" + DB.SQuote(Location) + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@CartonCode=" + DB.SQuote(Carton) + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@StorageLocation=" + DB.SQuote(StorageLocation) + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@IsDamaged=" + IsDamaged + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@HasDiscrepancy=" + hasDiscrepancy + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@Remarks=" + (remarks != "" ? DB.SQuote(remarks) : "NULL"));
            //sCmdUpsertPOQuantity.Append(" ,@GoodsMovementTypeID=1 ");
            //sCmdUpsertPOQuantity.Append(" ,@CreatedBy=" + UserID + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@SupplierInvoiceID=" + SupplierInvoiceId + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@SkipReasonID=" + SkipReasonID + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@VehicleNumebr=" + DB.SQuote(Vehicle) + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@SupplierInvoiceDetailsID_NEW=" + SupplierInvoiceDetailsID + "  ");
            //int Result = DB.GetSqlN(sCmdUpsertPOQuantity.ToString());
            //return Result;

        }

        public static int UpdateGoodsMovementDetails_New_Web(string Location, string DocQty, string Quantity, string InboundID, string LineNumber, string MCode, string IsDamaged, string hasDiscrepancy, string CreatedBy, string Carton, string StorageLocation, string UserID, string MfgDate = "", string ExpDate = "", string SerialNo = "0", string BatchNo = "0", string ProjectRefNo = "0", string SPID = "0", string PoheaderID = "0", string remarks = "", string SupplierInvoiceId = "0", string SkipReasonID = "0", string MRP = "0", string Vehicle = "", string SupplierInvoiceDetailsID = "0")
        {

            return -1;
            //StringBuilder sCmdUpsertPOQuantity = new StringBuilder();
            //sCmdUpsertPOQuantity.Append("Exec sp_INV_UpsertGoodsMovementDetails_NEW_Web ");
            //sCmdUpsertPOQuantity.Append(" @TransactionID=" + InboundID + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@MCode=" + DB.SQuote(MCode) + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@LineNumber=" + ((LineNumber == string.Empty || LineNumber == "" || LineNumber == null) ? "''" : LineNumber) + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@POHeaderID=" + PoheaderID + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@Quantity=" + Quantity + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@DocQty=" + Quantity + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@BatchNo=" + (BatchNo != "" ? DB.SQuote(BatchNo) : "''"));
            //sCmdUpsertPOQuantity.Append(" ,@SerialNo=" + (SerialNo != "" ? DB.SQuote(SerialNo) : "''"));
            //sCmdUpsertPOQuantity.Append(" ,@MfgDate=" + (MfgDate != "" ? DB.SQuote(MfgDate) : "''"));
            //sCmdUpsertPOQuantity.Append(" ,@ExpDate=" + (ExpDate != "" ? DB.SQuote(ExpDate) : "''"));
            //sCmdUpsertPOQuantity.Append(" ,@ProjectRefNo=" + (ProjectRefNo != "" ? DB.SQuote(ProjectRefNo) : "''"));
            //sCmdUpsertPOQuantity.Append(" ,@MRP=" + (MRP != "" ? DB.SQuote(MRP) : "''"));
            //sCmdUpsertPOQuantity.Append(" ,@SuggestedPutawayID=" + SPID + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@Loction=" + DB.SQuote(Location) + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@CartonCode=" + DB.SQuote(Carton) + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@StorageLocation=" + DB.SQuote(StorageLocation) + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@IsDamaged=" + IsDamaged + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@HasDiscrepancy=" + hasDiscrepancy + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@Remarks=" + (remarks != "" ? DB.SQuote(remarks) : "NULL"));
            //sCmdUpsertPOQuantity.Append(" ,@GoodsMovementTypeID=1 ");
            //sCmdUpsertPOQuantity.Append(" ,@CreatedBy=" + UserID + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@SupplierInvoiceID=" + SupplierInvoiceId + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@SkipReasonID=" + SkipReasonID + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@VehicleNumebr=" + DB.SQuote(Vehicle) + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@SupplierInvoiceDetailsID_NEW=" + SupplierInvoiceDetailsID + "  ");
            //int Result = DB.GetSqlN(sCmdUpsertPOQuantity.ToString());
            //return Result;
        }



        /// <summary>
        ///   /// Author : P S K M Swamy
        /// Created On  : 22/MAY/2020
        /// Description : Receiving Item into Inventory
        /// </summary>
        /// <param name="Location"></param>
        /// <param name="DocQty"></param>
        /// <param name="Quantity"></param>
        /// <param name="InboundID"></param>
        /// <param name="LineNumber"></param>
        /// <param name="MCode"></param>
        /// <param name="CreatedBy"></param>
        /// <param name="Carton"></param>
        /// <param name="StorageLocation"></param>
        /// <param name="HUNo"></param>
        /// <param name="MfgDate"></param>
        /// <param name="ExpDate"></param>
        /// <param name="SerialNo"></param>
        /// <param name="BatchNo"></param>
        /// <param name="ProjectRefNo"></param>
        /// <param name="SPID"></param>
        /// <param name="PoheaderID"></param>
        /// <param name="remarks"></param>
        /// <param name="SupplierInvoiceId"></param>
        /// <param name="SkipReasonID"></param>
        /// <param name="MRP"></param>
        /// <param name="Vehicle"></param>
        /// <param name="SupplierInvoiceDetailsID"></param>
        /// <param name="IsRequestFromApp"></param>
        /// <returns></returns>
        /// ReceiveItem(string Location, string DocQty, string Quantity, string InboundID, string LineNumber, string MCode, string CreatedBy, string Carton, string StorageLocation, int HUNo, string MfgDate = "", string ExpDate = "", string SerialNo = "0", string BatchNo = "0", string ProjectRefNo = "0", string SPID = "0", string PoheaderID = "0", string remarks = "", string SupplierInvoiceId = "0", string SkipReasonID = "0", string MRP = "0", string Vehicle = "", string SupplierInvoiceDetailsID = "0",int IsRequestFromApp=0)
        public static string ReceiveItem(GoodsInDTO objGoodsin)

        {
            //StringBuilder sCmdUpsertPOQuantity = new StringBuilder();
            //sCmdUpsertPOQuantity.Append("Exec [dbo].[sp_INV_ReceiveItem] ");
            //sCmdUpsertPOQuantity.Append(" @TransactionID=" + objGoodsin.InboundID + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@MCode=" + DB.SQuote(objGoodsin.MCode) + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@LineNumber=" + ((objGoodsin.LineNumber == string.Empty || objGoodsin.LineNumber == "" || objGoodsin.LineNumber == null) ? "''" : objGoodsin.LineNumber) + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@POHeaderID=" +DB.SQuote( objGoodsin.PoHeaderID) + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@DocQty=" + objGoodsin.DocumentQuantity + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@Quantity=" + objGoodsin.ConvertedQuantity + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@BatchNo=" + (objGoodsin.BatchNumber != "" ? DB.SQuote(objGoodsin.BatchNumber) : "''"));
            //sCmdUpsertPOQuantity.Append(" ,@SerialNo=" + (objGoodsin.SerialNumber != "" ? DB.SQuote(objGoodsin.SerialNumber) : "''"));
            //sCmdUpsertPOQuantity.Append(" ,@MfgDate=" + (objGoodsin.MfgDate != "" ? DB.SQuote(objGoodsin.MfgDate) : "''"));
            //sCmdUpsertPOQuantity.Append(" ,@ExpDate=" + (objGoodsin.ExpDate != "" ? DB.SQuote(objGoodsin.ExpDate) : "''"));
            //sCmdUpsertPOQuantity.Append(" ,@ProjectRefNo=" + (objGoodsin.ProjectRefNo != "" ? DB.SQuote(objGoodsin.ProjectRefNo) : "''"));
            //sCmdUpsertPOQuantity.Append(" ,@MRP=" + (objGoodsin.MRP != "" ? DB.SQuote(objGoodsin.MRP) : "''"));
            //sCmdUpsertPOQuantity.Append(" ,@StorageLocation=" + DB.SQuote(objGoodsin.StorageLocation) + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@Loction=" + DB.SQuote(objGoodsin.Location) + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@CartonCode=" + DB.SQuote(objGoodsin.CartonCode) + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@GoodsMovementTypeID=1 ");
            //sCmdUpsertPOQuantity.Append(" ,@CreatedBy=" + objGoodsin.LoggedinUserID + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@SupplierInvoiceID=" +DB.SQuote( objGoodsin.SupplierInvoiceId) + "  ");
            //sCmdUpsertPOQuantity.Append(" ,@HUNo=1");        
            //sCmdUpsertPOQuantity.Append(" ,@IsRequestFromPC=" + objGoodsin.IsRequestFromPC + "  ");

            //DataSet dsResult= DB.GetDS(sCmdUpsertPOQuantity.ToString(),false);
            //if(dsResult!=null && dsResult.Tables[0]!=null && dsResult.Tables[0].Rows.Count==1 && dsResult.Tables[0].Columns.Count==2)
            //{
            //    //throw new Exception(dsResult.Tables[0].Rows[0][1].ToString());
            //    return dsResult.Tables[0].Rows[0][1].ToString();
            //}
            //else if(dsResult != null && dsResult.Tables[0] != null && dsResult.Tables[0].Rows.Count == 1 && dsResult.Tables[0].Rows[0][0].ToString()=="1")
            //{
            //    return "1";
            //}
            //else
            //{
            //    throw new Exception("Unexpected error, please contact support");
            //}

            StringBuilder sCmdUpsertPOQuantity = new StringBuilder();
            if (Convert.ToInt32(objGoodsin.HUNo) > 1)
            {
                sCmdUpsertPOQuantity.Append("Exec [dbo].[SP_Upsert_HU_GoodsMovementDetails] ");
                sCmdUpsertPOQuantity.Append(" @HUNo=" + objGoodsin.HUNo + "  ");
                sCmdUpsertPOQuantity.Append(" ,@HUSize=" + DB.SQuote(objGoodsin.HUsize) + "  ");
                sCmdUpsertPOQuantity.Append(" ,@LineNumber=" + DB.SQuote(objGoodsin.LineNumber) + "  ");
                sCmdUpsertPOQuantity.Append(" ,@InboundId=" + DB.SQuote(objGoodsin.InboundID) + "  ");
                sCmdUpsertPOQuantity.Append(" ,@Mcode=" + DB.SQuote(objGoodsin.MCode) + "  ");
                sCmdUpsertPOQuantity.Append(" ,@MfgDate=" + DB.SQuote(objGoodsin.MfgDate) + "  ");
                sCmdUpsertPOQuantity.Append(" ,@ExpDate=" + DB.SQuote(objGoodsin.ExpDate) + "  ");
                sCmdUpsertPOQuantity.Append(" ,@ProjectRef=" + DB.SQuote(objGoodsin.ProjectRefNo) + "  ");
                sCmdUpsertPOQuantity.Append(" ,@MRP=" + DB.SQuote(objGoodsin.MRP) + "  ");
                sCmdUpsertPOQuantity.Append(" ,@BatchNo=" + DB.SQuote(objGoodsin.BatchNumber) + "  ");
                sCmdUpsertPOQuantity.Append(" ,@POHeaderID=" + DB.SQuote(objGoodsin.PoHeaderID) + "  ");
                sCmdUpsertPOQuantity.Append(" ,@Location=" + DB.SQuote(objGoodsin.Location) + "  ");
                sCmdUpsertPOQuantity.Append(" ,@StorageLocation=" + DB.SQuote(objGoodsin.StorageLocation) + "  ");
                sCmdUpsertPOQuantity.Append(" ,@Quntity=" + DB.SQuote(objGoodsin.DocumentQuantity) + "  ");
                sCmdUpsertPOQuantity.Append(" ,@UserId=" + (objGoodsin.LoggedinUserID) + "  ");
                sCmdUpsertPOQuantity.Append(" ,@CartonCode=" + DB.SQuote(objGoodsin.CartonCode) + "  ");
                sCmdUpsertPOQuantity.Append(" ,@StoreRefNo=" + DB.SQuote(objGoodsin.Storerefno) + "  ");
                sCmdUpsertPOQuantity.Append(" ,@TransactionTypeId= 1");
            }
            else
            {
                sCmdUpsertPOQuantity.Append("Exec [dbo].[sp_INV_ReceiveItem] ");
                sCmdUpsertPOQuantity.Append(" @TransactionID=" + objGoodsin.InboundID + "  ");
                sCmdUpsertPOQuantity.Append(" ,@MCode=" + DB.SQuote(objGoodsin.MCode) + "  ");
                sCmdUpsertPOQuantity.Append(" ,@LineNumber=" + ((objGoodsin.LineNumber == string.Empty || objGoodsin.LineNumber == "" || objGoodsin.LineNumber == null) ? "''" : objGoodsin.LineNumber) + "  ");
                sCmdUpsertPOQuantity.Append(" ,@POHeaderID=" + DB.SQuote(objGoodsin.PoHeaderID) + "  ");
                sCmdUpsertPOQuantity.Append(" ,@DocQty=" + objGoodsin.DocumentQuantity + "  ");
                sCmdUpsertPOQuantity.Append(" ,@Quantity=" + objGoodsin.ConvertedQuantity + "  ");
                sCmdUpsertPOQuantity.Append(" ,@BatchNo=" + (objGoodsin.BatchNumber != "" ? DB.SQuote(objGoodsin.BatchNumber) : "''"));
                sCmdUpsertPOQuantity.Append(" ,@SerialNo=" + (objGoodsin.SerialNumber != "" ? DB.SQuote(objGoodsin.SerialNumber) : "''"));
                sCmdUpsertPOQuantity.Append(" ,@MfgDate=" + (objGoodsin.MfgDate != "" ? DB.SQuote(objGoodsin.MfgDate) : "''"));
                sCmdUpsertPOQuantity.Append(" ,@ExpDate=" + (objGoodsin.ExpDate != "" ? DB.SQuote(objGoodsin.ExpDate) : "''"));
                sCmdUpsertPOQuantity.Append(" ,@ProjectRefNo=" + (objGoodsin.ProjectRefNo != "" ? DB.SQuote(objGoodsin.ProjectRefNo) : "''"));
                sCmdUpsertPOQuantity.Append(" ,@MRP=" + (objGoodsin.MRP != "" ? DB.SQuote(objGoodsin.MRP) : "''"));
                sCmdUpsertPOQuantity.Append(" ,@StorageLocation=" + DB.SQuote(objGoodsin.StorageLocation) + "  ");
                sCmdUpsertPOQuantity.Append(" ,@Loction=" + DB.SQuote(objGoodsin.Location) + "  ");
                sCmdUpsertPOQuantity.Append(" ,@CartonCode=" + DB.SQuote(objGoodsin.CartonCode) + "  ");
                sCmdUpsertPOQuantity.Append(" ,@GoodsMovementTypeID=1 ");
                sCmdUpsertPOQuantity.Append(" ,@CreatedBy=" + objGoodsin.LoggedinUserID + "  ");
                sCmdUpsertPOQuantity.Append(" ,@SupplierInvoiceID=" + DB.SQuote(objGoodsin.SupplierInvoiceId) + "  ");
                sCmdUpsertPOQuantity.Append(" ,@HUNo=1");
                sCmdUpsertPOQuantity.Append(" ,@IsRequestFromPC=" + objGoodsin.IsRequestFromPC + "  ");
            }



            DataSet dsResult = DB.GetDS(sCmdUpsertPOQuantity.ToString(), false);

            if (Convert.ToInt32(objGoodsin.HUNo) > 1)
            {
                if (Convert.ToInt32(dsResult.Tables[0].Rows[0][0]) == -20)
                {
                    return "Invalid Carton Scanned";
                }
                else if (Convert.ToInt32(dsResult.Tables[0].Rows[0][0]) == 0)
                {
                    return "Their is no pending SKU";
                }
                else if (Convert.ToInt32(dsResult.Tables[0].Rows[0][0]) == 1)
                {
                    return "1";
                }
                else
                {
                    throw new Exception("Unexpected error, please contact support");
                }
            }
            else
            {
                if (dsResult != null && dsResult.Tables[0] != null && dsResult.Tables[0].Rows.Count == 1 && dsResult.Tables[0].Columns.Count == 2)
                {
                    //throw new Exception(dsResult.Tables[0].Rows[0][1].ToString());
                    return dsResult.Tables[0].Rows[0][1].ToString();
                }
                else if (dsResult != null && dsResult.Tables[0] != null && dsResult.Tables[0].Rows.Count == 1 && dsResult.Tables[0].Rows[0][0].ToString() == "1")
                {
                    return "1";
                }
                else
                {
                    throw new Exception("Unexpected error, please contact support");
                }
            }
        }

       
    }

    public class GoodsInDTO
    {
        public string InboundID { set; get; }
        public string Storerefno { set; get; }
        public string MCode { set; get; }
        public string Location { set; get; }
        public string CartonCode { set; get; }
        public string DocumentQuantity { set; get; }
        public string PoHeaderID { set; get; }
        public string StorageLocation { set; get; }
        public string HUNo { set; get; }
        public string HUsize { set; get; }
        public string LineNumber { set; get; }
        public string BatchNumber { set; get; }
        public string ExpDate { set; get; }
        public string MfgDate { set; get; }
        public string SerialNumber { set; get; }
        public string ProjectRefNo { set; get; }
        public string MRP { set; get; }
        public int LoggedinUserID { set; get; }
        public string SupplierInvoiceId { set; get; }

        public int IsRequestFromPC { set; get; }

        public string ConvertedQuantity { set; get; }

    }

    
}

