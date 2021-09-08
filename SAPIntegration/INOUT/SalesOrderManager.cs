using SAPIntegration.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRLWMSC21Common;


namespace SAPIntegration.INOUT
{
    class SalesOrderManager
    {
        private InvSalesOrderInfo InvSO;
        private bool isUpdate;
        private bool isCreate;
        private bool isDelete;

        public SalesOrderManager(object soObj) 
        {
            InvSO = (InvSalesOrderInfo)soObj;
            isUpdate = false;
            isDelete = false;
            isCreate = true;
        }

        public bool saveOrUpdate() 
        {
            bool status = false;

            try
            {

                if (InvSO == null)
                    return status;

                StringBuilder SqlString = new StringBuilder();
                int CurrencyID = 0; int CountryMasterID = 0;

                int SOHeaderID = DB.GetSqlN("select SOHeaderID AS N from ORD_SOHeader where SONumber='" + InvSO.SOCode + "'");

                //if (OrderAction.Equals("3") && SOHeaderID != 0)
                //{
                //    if (DB.GetSqlN("select top 1 CPO.SOHeaderID AS N from OBD_Outbound_ORD_CustomerPO CPO  where CPO.IsDeleted=0 AND CPO.SOHeaderID=" + SOHeaderID) != 0)
                //    {
                //        ApplicationCommon.WriteLog("Admin", "WMS JDE Service", "RTSOOUT", "Cannot update, as this " + DetailNode["mnOrderNumber"].InnerText + " is configured in some transactions ", "RTSOOUT");
                //        return;
                //    }
                //    else
                //    {
                //        DB.ExecuteSQL("update ord_soheader set IsActive=0 , isdeleted=1  where  IsDeleted=0 AND SOHeaderID=" + SOHeaderID);
                //    }
                //}


                CurrencyID = CommonDAO.GetCurrencyID(InvSO.currencyCode);
                //CountryMasterID = CommonDAO.GetCountryID(salesOrder.);


                int CustomerID = DB.GetSqlN("select CustomerID AS N from GEN_Customer  where CustomerName='" + InvSO.customer.DBAName + " ( " + InvSO.customer.custCode + " )" + "'");

                if (CustomerID == 0)
                {
                    SqlString = null;
                    return false;
                }


                SqlString.Append("DECLARE @UpdateSOHeaderID int ;");
                SqlString.Append("EXEC [sp_ORD_UpsertSOHeader]   ");

                SqlString.Append("  @SOHeaderID=" + SOHeaderID);
                SqlString.Append(",@SONumber='" + InvSO.SOCode + "'");
                SqlString.Append(",@TenantID=1");
                SqlString.Append(",@CustomerID=" + CustomerID); //  NEED Coustomer Master
                SqlString.Append(",@SOStatusID=1"); //  NEED SO Status
                SqlString.Append(",@SOTypeID=1");
                SqlString.Append(",@CurrencyID=" + (CurrencyID == 0 ? "NULL" : CurrencyID.ToString()));
                SqlString.Append(",@RequestedBy=1");
                SqlString.Append(",@UserID=1");
                SqlString.Append(",@ProjectCode=''");
                SqlString.Append(",@SODate='" + InvSO.SODate + "'");
                SqlString.Append(",@RequirementNumber=''");
                SqlString.Append(",@ShipmentAddress1=''");
                SqlString.Append(",@ShipmentAddress2=''");
                SqlString.Append(",@City=''");
                SqlString.Append(",@Province=''");
                SqlString.Append(",@Zip=''");
                SqlString.Append(",@CountryMasterID=" + (CountryMasterID == 0 ? "NULL" : CountryMasterID.ToString()));
                SqlString.Append(",@Mobile=''");
                SqlString.Append(",@ShipmentCharges=NULL");
                SqlString.Append(",@SOTax=NULL");
                SqlString.Append(",@NetValue=NULL");
                SqlString.Append(",@GrossValue=NULL");
                SqlString.Append(",@CreatedBy=1");
                SqlString.Append(",@LastModifiedBy=1");
                SqlString.Append(",@IsActive=1");
                SqlString.Append(",@IsDeleted=0");

                SqlString.Append(",@NewSOHeaderID=@UpdateSOHeaderID OUTPUT ;");

                SqlString.Append("select @UpdateSOHeaderID AS  N");


                SOHeaderID = DB.GetSqlN(SqlString.ToString());

                if (SOHeaderID == 0)
                {
                    CommonLogic.createErrorNode("Admin", "Sales Order Manager", "SalesOrderManager", "  Error while updating SO details   ", "SalesOrderManager");

                }
                else
                {
                    SqlString.Clear();


                    try
                    {

                        List<InvCustomerPO> custPOList = (List<InvCustomerPO>)InvSO.CustPO;

                        InvCustomerPO custPO = custPOList[0];


                        foreach (LineItem lineItem in custPO.reqItems)
                        {

                            int MMID = CommonDAO.GetMMID(lineItem.item.partNumber);

                            int MM_UoMID = CommonDAO.GetMM_UoMID(MMID, lineItem.UOMCode, "1");


                            SqlString.Append("    EXEC [sp_ORD_UpsertSODetails]  ");

                            SqlString.Append("   @SODetailsID=" + DB.GetSqlN("select SODetailsID AS N from ORD_SODetails where IsActive=1 AND IsDeleted=0 AND  SOHeaderID=" + SOHeaderID + "  AND LineNumber=" + Convert.ToInt32(lineItem.LineNo)));
                            SqlString.Append(",@SOHeaderID=" + SOHeaderID);
                            SqlString.Append(",@LineNumber=" + Convert.ToInt32(lineItem.LineNo));
                            SqlString.Append(",@MaterialMasterID=" + (MMID == 0 ? "NULL" : MMID.ToString()));
                            SqlString.Append(",@MaterialMaster_SUoMID=" + (MM_UoMID == 0 ? "NULL" : MM_UoMID.ToString()));
                            SqlString.Append(",@KitPlannerID=NULL");
                            SqlString.Append(",@SOQuantity=" + lineItem.salesQty);
                            SqlString.Append(",@UnitPrice=NULL");
                            SqlString.Append(",@MaterialMaster_CustPOUoMID=NULL"); // NEED Customer PO
                            SqlString.Append(",@CustomerPOID=NULL"); // NEED Customer PO
                            SqlString.Append(",@CustPOQuantity=NULL");  // NEED Customer PO
                            SqlString.Append(",@SODiscountInPercentage=NULL");
                            SqlString.Append(",@VATCode=''");
                            //SqlString.Append(",@CountryofOriginID=NULL");
                            SqlString.Append(",@CreatedBy=1");
                            SqlString.Append(",@StoreParameterIDS=''");
                            SqlString.Append(",@StoreParameterValues=''  ; ");


                        }

                        if (SqlString.ToString() != "")
                            DB.ExecuteSQL(SqlString.ToString());

                    }
                    catch (Exception ex)
                    {
                        CommonLogic.createErrorNode("Admin", "Sales Order Manager", "SalesOrderManager", "  Error while updating SO details   ", "SalesOrderManager");
                    }
                }

                status = true;

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode("Admin", "Sales Order Manager", "SalesOrderManager", "  Error while updating SO details   ", "SalesOrderManager");
            }

            return status;
        }

        public bool deleteSalesOrder() 
        {
            return false;
        }
    }
}
