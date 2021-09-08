using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21Common;



using MRLWMSC21_Library.WMS_ServiceObjects;
using MRLWMSC21_Library.WMS_DBCommon;
using MRLWMSC21_Library.WMS_ServiceControl;
using MRLWMSC21_Library.WMS_Services;

using System.Data;


namespace MRLWMSC21.mInbound
{

    public partial class PutawaySuggestion : System.Web.UI.Page
    {
        Inbound invDetails = null;
        SupplierInvoice invoice = null;
        LineItem lineItem = null;

        // Set page theme
        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Inbound";
        }


        protected void resetError(string error, bool isError)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);
        }


        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                DesignLogic.SetInnerPageSubHeading(this, "Suggested Location");
                CommonCriteria criteria = new CommonCriteria();
                criteria.INBOUNDID = Convert.ToInt32(CommonLogic.QueryString("ibdid"));

                object DBResult = DAOController.GetDataFromStoredProcedure(DRLExecuteCode.INVOICE_LIST_BY_INBOUND, new DBCriteria().INVOICE_LIST_BY_INBOUND(criteria));

                if (DBResult != null)
                {

                    try
                    {
                        Object ServiceResult = WMSController.Process(ServiceCall.INB_SUGGESTED_PUTAWAY, DBResult);
                        invDetails = (Inbound)ServiceResult;
                        lblStoreRefNo.Text = "Store Ref. No. : " + invDetails.InboundStoreRef;
                        lblBarStoreRefNo.Text = String.Format("<img width=\"250px\" src=\"Code39Handler.ashx?code={0} \"", invDetails.InboundStoreRef);
                       
                        gvInvoiceList.DataSource = invDetails.invoices.Values.ToList();
                        gvInvoiceList.DataBind();

                    }
                    catch (Exception ex)
                    {
                        //there no locations to suggest
                        if (ex is ServiceException)
                        {
                            ServiceException se = (ServiceException)ex;
                            if (se.getErrorNo() == 15)
                                resetError(se.getSuggetedMessage() + " [there is no locations to suggest] ErrorNo:" + se.getErrorNo() + " /" + se.getMsg(), true);
                            else
                                resetError(se.getMsg() + "ErrorNo: " + se.getErrorNo() + " [technical problem]", true);
                        }
                        else
                        {
                            resetError(ex.Message + " [technical problem]", true);
                        }
                    }
                }
            }
        }



        protected void gvInvoiceList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                
                
                invoice = (SupplierInvoice)e.Row.DataItem;
                GridView gvLineItems = (GridView)e.Row.FindControl("gvLineItems");
                
                //gvLineItems.DataSource = invoice.InvoiceLineItems;
                               
                DataTable dt = getInnerGridRows(invoice.InvoiceLineItems);

                gvLineItems.DataSource = dt;
                gvLineItems.DataBind();
                
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                invDetails = null;
            }

        }


        protected DataTable getInnerGridRows(IList<LineItem> lineItems) 
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn("Line No.", typeof(string)));
            dt.Columns.Add(new DataColumn("Material Code", typeof(string)));
            dt.Columns.Add(new DataColumn("Mdesc", typeof(string)));
            dt.Columns.Add(new DataColumn("BUoM / Qty.", typeof(string)));
            dt.Columns.Add(new DataColumn("PUoM / Qty.", typeof(string)));
            dt.Columns.Add(new DataColumn("INV. UOM / Qty.", typeof(string)));
            dt.Columns.Add(new DataColumn("PO Qty.", typeof(string)));
            dt.Columns.Add(new DataColumn("StockInLink", typeof(string)));
            dt.Columns.Add(new DataColumn("Inv. Qty.", typeof(string)));
            dt.Columns.Add(new DataColumn("Initiated Qty.", typeof(string)));
            dt.Columns.Add(new DataColumn("Location", typeof(string)));
            dt.Columns.Add(new DataColumn("Sug. Qty.", typeof(string)));

            foreach(LineItem li in lineItems)
            {

                if (li.SuggestedLocations != null && li.SuggestedLocations.Count > 0)
                {
                    foreach (MRLWMSC21_Library.WMS_ServiceObjects.SuggestedLocation sc in li.SuggestedLocations)
                    {
                        DataRow dr = dt.NewRow();
                        dr["Location"] = sc.LocationCode;
                        dr["Sug. Qty."] = Math.Round( (sc.QTY * 100.00m)/100,2);
                        dr["Material Code"] = li.MaterialCode + Environment.NewLine;
                        dr["Mdesc"] = li.MDesc + Environment.NewLine;
                        dr["Line No."] = li.LineNo;
                        dr["BUoM / Qty."] = li.Base_UOM + "/" + li.Base_UOM_QTY;
                        dr["PUoM / Qty."] = li.PO_UOM + "/" + li.PO_UOM_QTY;
                        dr["INV. UOM / Qty."] = li.INV_UOM + "/" + li.INV_UOM_QTY;

                        dr["PO Qty."] = li.PO_QTY;
                        dr["Inv. Qty."] = li.INV_QTY;
                        dr["Initiated Qty."] = li.processedDocQty;

                        
                        if (invDetails != null && invoice != null)
                        {
                            dr["StockInLink"] = "../mInventory/StockIn.aspx?ibdno=" + invDetails.InboundId + "&mmid=" + li.MaterialID + "&lno=" + li.LineNo + "&poid=" + invoice.POID + "&invid=" + invoice.supplierInvID + "&Loc=" + sc.LocationCode;
                        }

                        dt.Rows.Add(dr);
                    }
                }
                else 
                {
                    DataRow dr = dt.NewRow();
                    dr["Location"] = "";
                    dr["Sug. Qty."] = "";

                    dr["Material Code"] = li.MaterialCode + Environment.NewLine;
                    dr["Mdesc"] = li.MDesc + Environment.NewLine;
                    dr["Line No."] = li.LineNo.ToString();
                    dr["BUoM / Qty."] = li.Base_UOM + "/" + li.Base_UOM_QTY;
                    dr["PUoM / Qty."] = li.PO_UOM + "/" + li.PO_UOM_QTY;
                    dr["INV. UOM / Qty."] = li.INV_UOM + "/" + li.INV_UOM_QTY;

                    dr["PO Qty."] = li.PO_QTY;
                    dr["Inv. Qty."] = li.INV_QTY;
                    dr["Initiated Qty."] = li.processedDocQty;
                    dr["StockInLink"] = "";       
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }
        
        
        protected void gvLineItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    lineItem = (LineItem)e.Row.DataItem;
            //    GridView gvSuggestLocation = (GridView)e.Row.FindControl("gvSuggestLocation");

            //    if (lineItem.SuggestedLocations == null || lineItem.SuggestedLocations.Count < 1)
            //    {
            //        List<MRLWMSC21_Library.WMS_ServiceObjects.SuggestedLocation> suggestedLocations = new List<MRLWMSC21_Library.WMS_ServiceObjects.SuggestedLocation>();
            //        suggestedLocations.Add(new MRLWMSC21_Library.WMS_ServiceObjects.SuggestedLocation());
            //        gvSuggestLocation.DataSource = suggestedLocations;
            //    }
            //    else
            //    {
            //        gvSuggestLocation.DataSource = lineItem.SuggestedLocations;
            //    }

            //    gvSuggestLocation.DataBind();
            //}
            //else if (e.Row.RowType == DataControlRowType.Footer)
            //{
            //    invoice = null;
            //}

            //HyperLink hplPick = (HyperLink)e.Row.FindControl("hplPick");
            //if (invDetails != null && invoice != null)
            //{
            //    hplPick.NavigateUrl = "../mInventory/StockIn.aspx?ibdno=" + invDetails.InboundId + "&mmid=" + lineItem.MaterialID + "&lno=" + lineItem.LineNo + "&poid=" + invoice.POID + "&invid=" + invoice.supplierInvID + "&Loc=";
            //}
                
        }

    }
}