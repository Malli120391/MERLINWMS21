using MRLWMSC21.TPL.Invoice;

using MRLWMSC21_Library.WMS_DBCommon;
using MRLWMSC21_Library.WMS_ServiceControl;
using MRLWMSC21_Library.WMS_ServiceObjects;
using MRLWMSC21_Library.WMS_Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRLWMSC21.TPL
{
    public partial class InvoiceGenerationTestPage : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
               
            //TPLBilling bill = new TPLBilling(1019);
            //if (bill.getInvoice(1019, Convert.ToDateTime("2015-07-08"), Convert.ToDateTime("2015-07-11"))) 
            //{
            //    bill.saveInvoice(1019, Convert.ToDateTime("2015-07-08"), Convert.ToDateTime("2015-07-11"));
            //}

            //suggested putaway testing 
            CommonCriteria criteria = new CommonCriteria();
            criteria.INBOUNDID = 5074;

            object DBResult = DAOController.GetDataFromStoredProcedure(DRLExecuteCode.INVOICE_LIST_BY_INBOUND, new DBCriteria().INVOICE_LIST_BY_INBOUND(criteria));
            if (DBResult != null)
            {
                Object ServiceResult = WMSController.Process(ServiceCall.INB_SUGGESTED_PUTAWAY, DBResult);
            }

            
            //suggested picknote testing 
            //CommonCriteria criteria = new CommonCriteria();
            //criteria.OUTBOUNDID = 1031;
            //criteria.MATERIALCODE = "";
            //Object DBResult = DAOController.GetDataFromStoredProcedure(DRLExecuteCode.DELIVERY_PICK_NOTE, new DBCriteria().DELIVERY_PICK_NOTE(criteria));
            //AvailableStockData availStock = (AvailableStockData)DBResult;
            //Object ServiceResult = WMSController.Process(ServiceCall.OBD_SUGGESTED_PICKNOTE, availStock);
            
        }

    }

}