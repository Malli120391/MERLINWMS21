using MRLWMSC21Common;
using SAPIntegration.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPIntegration.INOUT
{
    public class PurchaseOrderManager
    {
        private InvPurchaseOrderInfo InvPO;
        private bool isUpdate;
        private bool isCreate;
        private bool isDelete;


        public PurchaseOrderManager(object poObj) 
        {
            InvPO = (InvPurchaseOrderInfo)poObj;
            isUpdate = false;
            isDelete = false;
            isCreate = true;
        }

        public bool saveOrUpdate() 
        {
            bool status = false;

            if (InvPO == null)
                return status;

            try
            {
                int CurrencyID = 0;
                int POHeaderID = DB.GetSqlN("select POHeaderID AS N from ORD_POHeader where  PONumber='" + InvPO.POCode + "'");

                //String OrderAction = DetailNode["cOrderAction"].InnerText;


                //if (OrderAction.Equals("3") && POHeaderID != 0)
                //{
                //    if (DB.GetSqlN("select top 1 POHeaderID AS N from INB_Inbound_ORD_SupplierInvoice where IsDeleted=0 AND POHeaderID=" + POHeaderID) != 0)
                //    {
                //        //ApplicationCommon.WriteLog("Admin", "WMS JDE Service", "RTPOOUT", "Cannot update, as this " + DetailNode["mnOrderNumber"].InnerText + " is configured in some transactions ", "RTPOOUT");
                //        return status;
                //    }
                //    else
                //    {
                //        DB.ExecuteSQL("update ord_poheader set IsActive=0 , isdeleted=1  where  IsDeleted=0 AND POHeaderID=" + POHeaderID);
                //    }
                //}


                StringBuilder SqlString = new StringBuilder();

                int SupplierID = DB.GetSqlN("select SupplierID AS N from MMT_Supplier where  SupplierCode='" + InvPO.vendor.vendorCode + "'");

                if (SupplierID == 0)
                {
                    //ApplicationCommon.WriteLog("Admin", "WMS JDE Service", "RTPOOUT", "Cannot update, as this " + DetailNode["mnSupplierAddressNumber"].InnerText + " is not configured in supplier ", "RTPOOUT");
                    return status;
                }


                CurrencyID = CommonDAO.GetCurrencyID(InvPO.vendorInvoices[0].currency.currencyCode);

                SqlString.Append("DECLARE @UpdatePOHeaderID int ;   ");

                SqlString.Append("EXEC [dbo].[sp_ORD_UpsertPOHeader]    ");

                SqlString.Append("   @POHeaderID=" + POHeaderID);
                SqlString.Append(",@PONumber='" + InvPO.POCode + "'");
                SqlString.Append(",@PODate='" + InvPO.PODate + "'");
                SqlString.Append(",@POStatusID=1");
                SqlString.Append(",@POTypeID=1");
                SqlString.Append(",@SupplierID=" + (SupplierID == 0 ? "NULL" : SupplierID.ToString()));
                SqlString.Append(",@DateRequested='" + InvPO.PODate + "'");
                SqlString.Append(",@DateDue=NULL");
                SqlString.Append(",@DepartmentID=NULL");
                SqlString.Append(",@DivisionID=NULL");
                SqlString.Append(",@RequestedBy=1");
                SqlString.Append(",@TotalValue=NULL");
                SqlString.Append(",@CurrencyID=" + (CurrencyID == 0 ? "NULL" : CurrencyID.ToString()));
                SqlString.Append(",@ExchangeRate=NULL");
                SqlString.Append(",@POTax=NULL");
                SqlString.Append(",@Instructions=NULL");
                SqlString.Append(",@Remarks=NULL");
                SqlString.Append(",@CreatedBy=1");
                SqlString.Append(",@LastModifiedBy=1");
                SqlString.Append(",@TenantID=1");
                SqlString.Append(",@IsActive=1");
                SqlString.Append(",@IsDeleted=0");
                SqlString.Append(",@NewPOHeaderID = @UpdatePOHeaderID OUTPUT ;  Select @UpdatePOHeaderID AS N");


                POHeaderID = DB.GetSqlN(SqlString.ToString());

                if (POHeaderID == 0)
                {
                    //ApplicationCommon.WriteLog("Admin", "WMS JDE Service", "RTPOOUT", " Error while updating PO details  ", "RTPOOUT");

                    return status;
                }
                else
                {

                    SqlString.Clear();

                    try
                    {

                        foreach (InvInvoice invoice in InvPO.vendorInvoices)
                        {

                            foreach (LineItem lineItem in invoice.lineItems)
                            {

                                int MMID = CommonDAO.GetMMID(lineItem.item.partNumber);



                                int MM_UoMID = CommonDAO.GetMM_UoMID(MMID, lineItem.item.UOMS[0].UOMCode, "1");


                                SqlString.Append("      EXEC [sp_ORD_UpsertPODetails]  ");
                                SqlString.Append("  @PODetailsID= " + DB.GetSqlN("select PODetailsID AS N from ORD_PODetails where IsActive=1 AND IsDeleted=0 AND  POHeaderID=" + POHeaderID + "  AND  LineNumber=" + Convert.ToInt32(lineItem.LineNo)));
                                SqlString.Append(",@POHeaderID=" + POHeaderID);
                                SqlString.Append(",@LineNumber=" + Convert.ToInt32(lineItem.LineNo));
                                SqlString.Append(",@RequirementNumber=''");
                                SqlString.Append(",@MaterialMasterID=" + (MMID == 0 ? "NULL" : MMID.ToString()));
                                SqlString.Append(",@KitPlannerID=NULL");
                                SqlString.Append(",@MaterialMaster_PUoMID=" + (MM_UoMID == 0 ? "NULL" : MM_UoMID.ToString()));
                                SqlString.Append(",@POQuantity=" + lineItem.purchQty);
                                SqlString.Append(",@CreatedBy=1");
                                SqlString.Append(",@StoreParameterIDS=''");
                                SqlString.Append(",@StoreParameterValues=''  ; ");

                            }
                        }


                        DB.ExecuteSQL(SqlString.ToString());
                    }
                    catch (Exception ex)
                    {
                        CommonLogic.createErrorNode("SAPUSER", "PO Manager", ex.Source, ex.Message, ex.StackTrace);
                        return status;
                    }
                }


                status = true;

            }


            catch (Exception ex)
            {
                CommonLogic.createErrorNode("SAPUSER", "PO Manager", ex.Source, ex.Message, ex.StackTrace);

                return status;
            }


            return status;

        }

        public bool deletePO() 
        {
            return false;
        }


    }
}
