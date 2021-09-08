using SAPIntegration.BusinessObjects;
using SAPIntegration.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRLWMSC21Common;

namespace SAPIntegration.INOUT
{
    public class ItemManager
    {

        private IList<InvItem> listItem;
        private bool isDelete;
        private bool isCreate;
        private bool isUpdate;

        public ItemManager(object item) 
        {
            try
            {
                this.listItem = (IList<InvItem>)item;
                isDelete = false;
                isUpdate = false;
                isCreate = true;
            }
            catch(Exception ex)
            {
                throw new InvParserException(ex.Message,4);
            }
        }

        public bool saveOrUpdate() 
        {

            bool status = false;

            int MMID = 0;

            try
            {

                if (listItem == null && listItem.Count == 0)
                {
                    return status;
                }


                InvItem item = listItem[0];

                StringBuilder SqlString = new StringBuilder();


                MMID = DB.GetSqlN("select Materialmasterid AS N from MMT_MaterialMaster where isactive=1 AND isdeleted=0 and MCode='" + item.partNumber + "'");
                int MTypeID = 3;

                String SOCheck, POCheck;

                SOCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckSO] @MaterialMasterID=" + MMID + ",@TenantID=" + 1);
                POCheck = DB.GetSqlS("EXEC [dbo].[sp_GEN_CheckPO] @MaterialMasterID=" + MMID + ",@TenantID=" + 1);


                // Check if material is configured in po or so
                if (SOCheck != "" || POCheck != "")
                {
                    //ApplicationCommon.WriteLog("Admin", "WMS JDE Service", "RTIMOUT", "Cannot update, as this material " + DetailNode["szProductItem"].InnerText + " is configured in  " + CommonLogic.IIF(SOCheck != "", "SO Numbers : ", "") + SOCheck + " " + CommonLogic.IIF(POCheck != "", "PO Numbers : ", "") + POCheck, " RTIMOUT");
                    SqlString = null;
                    return false;
                }

                // Check if material is configured in routing
                if (DB.GetSqlN("select MaterialMasterID AS N from MFG_RoutingHeader_Revision MFG_RV JOIN MMT_MaterialMaster_Revision MMT_MR ON MMT_MR.MaterialMasterRevisionID=MFG_RV.MaterialMasterRevisionID AND MMT_MR.IsActive=1 AND MMT_MR.IsDeleted=0 where MMT_MR.MaterialMasterID=" + MMID + " AND MMT_MR.IsActive=1 AND MMT_MR.IsDeleted=0") != 0)
                {
                    // ApplicationCommon.WriteLog("Admin", "WMS JDE Service", "RTIMOUT", "Cannot update, as this material " + DetailNode["szProductItem"].InnerText + "  is in use", " RTIMOUT");
                    SqlString = null;
                    return false;
                }



                SqlString.Append(" DECLARE @UpdateMaterialMasterID int ;");

                SqlString.Append(" EXEC [sp_MMT_UpsertMaterialMasterItem]  ");

                SqlString.Append("  @MaterialMasterID=" + MMID);
                SqlString.Append(" ,@MCode=" + ( item.partNumber != null && item.partNumber!=""? DB.SQuote(item.partNumber) :"NULL" ));

                SqlString.Append(" ,@MCodeAlternative1=''");
                SqlString.Append(" ,@MCodeAlternative2=''");

                SqlString.Append(" ,@MDescription='" + item.description + "'");
                SqlString.Append(" ,@MDescriptionLong=''");

                SqlString.Append(" ,@CustomerPartNumber=''");
                SqlString.Append(" ,@OEMPartNumber='" + item.OEMCode + "'");


                SqlString.Append(" ,@MPlantID=1");
                SqlString.Append(" ,@StorageConditionID=1");

                SqlString.Append(" ,@Remarks=''");
                SqlString.Append(" ,@IsAproved=1");
                SqlString.Append(" ,@RequestedBy=1");
                SqlString.Append(" ,@IsActive=1");

                SqlString.Append(" ,@MGroupID=NULL");
                SqlString.Append(" ,@ProductCategoryID=1");

                SqlString.Append(" ,@MLength=NULL");
                SqlString.Append(" ,@MHeight=NULL");
                SqlString.Append(" ,@MWidth=NULL");
                SqlString.Append(" ,@MWeight=NULL");

                SqlString.Append(" ,@IsFirstEdit=1");
                SqlString.Append(" ,@LastEditedByID=NULL");
                SqlString.Append(" ,@TenantID=1");
                SqlString.Append(" ,@CreatedBy=1");

                SqlString.Append(" ,@StandardPrice=NULL");
                SqlString.Append(" ,@UoMID=NULL");


                SqlString.Append(" ,@TotalShelfLifeinDays=0");

                SqlString.Append(" ,@MinShelfLifeinDays=0");




                SqlString.Append(" ,@MTypeID=" + MTypeID);


                SqlString.Append(" ,@PUoMID=NULL");
                SqlString.Append(" ,@PUoMQty=NULL");
                SqlString.Append(" ,@CurrencyID=NULL");
                SqlString.Append(" ,@POrderText=''");

                SqlString.Append(" ,@ReorderPoint=0");
                SqlString.Append(" ,@ReorderQtyMax=0");
                SqlString.Append(" ,@ReorderQtyMin=0");
                SqlString.Append(" ,@MaximumStockLevel=NULL");
                SqlString.Append(" ,@MinimumStockLevel=NULL");

                SqlString.Append(" ,@SUoMID=NULL");
                SqlString.Append(" ,@SUoMQty=NULL");

                SqlString.Append(" ,@PrUoMID=NULL");
                SqlString.Append(" ,@PrUoMQty=NULL");




                SqlString.Append(" , @Result = @UpdateMaterialMasterID OUTPUT ;");

                SqlString.Append(" Select @UpdateMaterialMasterID AS N ");




                if (DB.GetSqlN(SqlString.ToString()) == -3)
                {
                    CommonLogic.createErrorNode("SAPUSER", "Item Manager", "Error while updating item master details", "Error while updating item master details", "Error while updating item master details");
                }
                else
                {

                    SqlString.Clear();


                    MMID = DB.GetSqlN("select Materialmasterid AS N from MMT_MaterialMaster where isactive=1 AND isdeleted=0 and MCode='" + item.partNumber + "'");

                    List<BusinessUOM> uoms = (List<BusinessUOM>)item.UOMS;

                    String UoMCode = "";

                    foreach (BusinessUOM uom in uoms)
                    {
                        if (uom.UOMTypeCode.Equals("Base"))
                        {
                            UoMCode = uom.UOMCode;
                        }
                    }




                    int UoMID = CommonDAO.GetUoMID(UoMCode);

                    if (UoMID != 0)
                    {


                        // Check if UoM is configured or not
                        if (DB.GetSqlN("select count( * ) AS N from MMT_MaterialMaster_GEN_UoM where  isactive=1 AND isdeleted =0 AND materialmasterid=" + MMID) > 2)
                        {
                            //ApplicationCommon.WriteLog("Admin", "WMS JDE Service", "RTIMOUT", DetailNode["szProductItem"].InnerText + " UoM's already updated", " RTIMOUT");
                            SqlString = null;
                        }


                        SqlString.Append(" EXEC  [dbo].[sp_MMT_UpsertMaterialMaster_GEN_UoM]  ");
                        SqlString.Append("   @MaterialMaster_UoMID=" + DB.GetSqlN("select MaterialMaster_UoMID AS N from MMT_MaterialMaster_GEN_UoM where IsActive=1 AND IsDeleted=0 AND UoMTypeID=1  AND MaterialMasterID=" + MMID));
                        SqlString.Append(",@TenantID=1");
                        SqlString.Append(",@MaterialMasterID=" + MMID);
                        SqlString.Append(",@UoMTypeID=1");
                        SqlString.Append(",@UoMID=" + (UoMID == 0 ? "NULL" : UoMID.ToString()));
                        SqlString.Append(",@UoMQty=1");
                        SqlString.Append(",@CreatedBy=1");

                        DB.ExecuteSQL(SqlString.ToString());

                        SqlString.Clear();

                        SqlString.Append(" EXEC  [dbo].[sp_MMT_UpsertMaterialMaster_GEN_UoM]  ");
                        SqlString.Append("   @MaterialMaster_UoMID=" + DB.GetSqlN("select MaterialMaster_UoMID AS N from MMT_MaterialMaster_GEN_UoM where IsActive=1 AND IsDeleted=0 AND UoMTypeID=2  AND MaterialMasterID=" + MMID));
                        SqlString.Append(",@TenantID=1");
                        SqlString.Append(",@MaterialMasterID=" + MMID);
                        SqlString.Append(",@UoMTypeID=2");
                        SqlString.Append(",@UoMID=" + (UoMID == 0 ? "NULL" : UoMID.ToString()));
                        SqlString.Append(",@UoMQty=1");
                        SqlString.Append(",@CreatedBy=1");

                        DB.ExecuteSQL(SqlString.ToString());
                    }

                }



                status = true;
            }
            catch (Exception ex)
            {

                CommonLogic.createErrorNode("SAPUSER", "Item Manager", ex.Source, ex.Message, ex.StackTrace);
            }



            return status;
        }

        public bool deleteItem() 
        {
            return true;
        }

        public bool saveItem() 
        {
            return true;
        }

    }
}
