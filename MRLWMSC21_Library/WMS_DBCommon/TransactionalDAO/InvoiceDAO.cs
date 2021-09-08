using MRLWMSC21Common;
using MRLWMSC21_Library.WMS_ServiceObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21_Library.WMS_DBCommon.TransactionalDAO
{
    
    
    internal class InvoiceDAO : DBServiceController
    {
        delegate Object InvoiceDataAccess(Object input, int DB_ACTION);

        public Object processDBCall(string DB_CALL_CODE, Object input, int IN_MSG_TYPE)
        {
            try
            {
                
                if (DBServicePool.DBServiceImplementRef.ContainsKey(DB_CALL_CODE.Trim()) || DBServicePool.DBServiceImplementRef[DB_CALL_CODE].Trim() != "")
                {
                    MethodInfo methodInfo = typeof(InvoiceDAO).GetMethod(DBServicePool.DBServiceImplementRef[DB_CALL_CODE]);
                    InvoiceDataAccess dataAccess = (InvoiceDataAccess)Delegate.CreateDelegate(typeof(InvoiceDataAccess), null, methodInfo);
                    return dataAccess(input, IN_MSG_TYPE);
                }
                else
                {
                    throw new NotImplementedException("the requested DB execution call not implemented under InvoiceDAO. [" + DB_CALL_CODE + "]");
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException("the requested DB Execution Call not implemented yet. [" + DB_CALL_CODE + "] " + ex.Message);
            }
        }

        public Inbound GetInvoiceListByInboud(Object input, int DB_ACTION = 2)
        {
            //CRITERIAL IN THE FORM OF DICTIONARY
            Inbound inbound = null;
            IDictionary<int, SupplierInvoice> SupplierInvoice = null;
            
            if (input != null && DB_ACTION == 2)
            {
                IDictionary<string, object> criteria_input = (IDictionary<string, object>)input;
                inbound = new Inbound();
                inbound.InboundId = criteria_input["INBOUNDID"].ToString();
                inbound.InboundSysId = Convert.ToInt32(criteria_input["INBOUNDID"]);

                DataSet InboundInvoice = DB.GetDS("[dbo].[SP_SUGGESTED_PUTAWAY_GRN_STATUS] @INBOUNDID=" + Convert.ToInt32(criteria_input["INBOUNDID"]), false);
                if (InboundInvoice != null && InboundInvoice.Tables.Count > 0 && InboundInvoice.Tables[0] != null)
                {

                    SupplierInvoice = new Dictionary<int, SupplierInvoice>();
                    DataTable INB_INVOICE = InboundInvoice.Tables[0];
                    
                    foreach (DataRow row in INB_INVOICE.Rows)
                    {
                        SupplierInvoice invoice = null;
                        LineItem InvLineItem = new LineItem();

                        if (inbound.InboundStoreRef == null)
                            inbound.InboundStoreRef = row["StoreRefNo"].ToString();
                                             


                        InvLineItem.Base_UOM = row["BASE_UOM"].ToString();
                        InvLineItem.Base_UOMID = Convert.ToInt16(row["BASE_UOMID"]);
                        InvLineItem.Base_UOM_QTY = Convert.ToDecimal(row["BASE_UOM_QTY"]);

                        InvLineItem.GRN_STATUS = (Convert.ToInt16(row["GRN_STATUS"]) == 1) ;
                        
                        InvLineItem.INV_QTY = Convert.ToDecimal(row["INV_QTY"]);
                        InvLineItem.INV_UOM = row["INV_UOM"].ToString();
                        InvLineItem.INV_UOMID = Convert.ToInt16(row["INV_UOMID"]);
                        InvLineItem.INV_UOM_QTY = Convert.ToDecimal(row["INV_UOM_QTY"]);

                        InvLineItem.LineNo = (row["LineNumber"]).ToString();
                        InvLineItem.LineSeqId = Convert.ToInt16(row["LineNumber"]);
                        InvLineItem.MaterialCode = row["MCode"].ToString();
                        InvLineItem.MaterialID = Convert.ToInt32(row["MaterialMasterID"]);
                        
                        InvLineItem.PO_QTY = Convert.ToDecimal(row["PO_QTY"]);
                        InvLineItem.PO_UOM = row["PO_UOM"].ToString();
                        InvLineItem.PO_UOMID = Convert.ToInt16(row["PO_UOMID"]);
                        InvLineItem.PO_UOM_QTY = Convert.ToDecimal(row["PO_UOM_QTY"]);

                        InvLineItem.processedDocQty = Convert.ToDecimal(row["DOC_QTY"]);
                        InvLineItem.processedQty = Convert.ToDecimal(row["RECEIVED_QTY"]);
                        InvLineItem.QTY = Convert.ToDecimal(row["INV_QTY"]);

                        InvLineItem.InvoiceSysLineId = Convert.ToInt32(row["SupplierInvoiceDetailsID"]);
                        InvLineItem.MDesc = row["MDesc"].ToString();

                        InvLineItem.dimensions = new MaterialDimension() 
                        { MVOLUME = Convert.ToDecimal(row["MVOLUME"]), MWEIGHT = Convert.ToDecimal(row["MWEIGHT"]) };
                        InvLineItem.IsPalletized = (Convert.ToInt16(row["IsPalletized"].ToString()) == 1);

                        
                        if (!SupplierInvoice.ContainsKey(Convert.ToInt32(row["SupplierInvoiceID"])))
                        {
                            invoice = new SupplierInvoice();
                            if (row["InvoiceDate"] != null && row["InvoiceDate"].ToString() != "")
                            {
                                invoice.ProcessDate = Convert.ToDateTime(row["InvoiceDate"]);
                                invoice.ValueDate = Convert.ToDateTime(row["InvoiceDate"]);
                            }
                            
                            invoice.supplierInvCode = row["InvoiceNumber"].ToString();
                            invoice.supplierInvID = Convert.ToInt16(row["SupplierInvoiceID"]);
                            invoice.SupplierSysId = Convert.ToInt16(row["SupplierID"]);
                            invoice.SupplierCode = row["SupplierCode"].ToString();
                            invoice.VatCode = row["VAT_CODE"].ToString();
                            
                            invoice.POID = Convert.ToInt16(row["POHeaderID"]);
                            invoice.POCode = row["PONumber"].ToString();
                            invoice.TenantID = Convert.ToInt16(row["TenantID"]);
                            invoice.StoreRefId = row["StoreRefNo"].ToString();
                            invoice.InvoiceLineItems = new List<LineItem>();
                            invoice.InboundId = inbound.InboundSysId;

                            inbound.SupplierSysId = Convert.ToInt16(row["SupplierID"]);
                            inbound.TenantId = Convert.ToInt16(row["TenantID"]); 
                            SupplierInvoice.Add(Convert.ToInt32(row["SupplierInvoiceID"]), invoice);
                        }
                        else
                        {
                            invoice = SupplierInvoice[Convert.ToInt32(row["SupplierInvoiceID"])];
                        }

                        invoice.InvoiceLineItems.Add(InvLineItem);
                        SupplierInvoice[Convert.ToInt32(row["SupplierInvoiceID"])] = invoice;

                    }

                }

            }
            else
            {
                int InboundID = (int)input;
                //NEED TO WRITE SOME NATIVE QUERY
            }
            inbound.invoices = SupplierInvoice;
            return inbound;
        }

        public SupplierInvoice GetInvoiceById(Object input, int DB_ACTION = 2)
        {
            return null;
        }

        public IList<SupplierInvoice> GetInvoiceListByTenant(Object input, int DB_ACTION = 2)
        {
            return null;
        }

        public IList<SupplierInvoice> GetInvoiceListBySupplier(Object input, int DB_ACTION = 2)
        {
            return null;
        }

        public IList<SupplierInvoice> GetInvoiceListByPO(Object input, int DB_ACTION = 2)
        {
            return null;
        }
    }
}
