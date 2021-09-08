using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21Common;


using System.Data;
using MRLWMSC21_Library.WMS_ServiceObjects;
using MRLWMSC21_Library.WMS_DBCommon;
using MRLWMSC21_Library.WMS_ServiceControl;
using MRLWMSC21_Library.WMS_Services;

namespace MRLWMSC21.mInbound
{
    public partial class SuggestedLocation : System.Web.UI.Page
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



                //IList<MRLWMSC21_Library.WMS_ServiceObjects.AddressRecord> tmp =null;//= new List<MRLWMSC21_Library.WMS_ServiceObjects.AddressRecord>();
                //if (tmp == null)
                //{
                //    resetError("is null", true);
                //}
                //else
                //    resetError("is not null", false);

                DesignLogic.SetInnerPageSubHeading(this,"Suggested Location");
                CommonCriteria criteria = new CommonCriteria();
                criteria.INBOUNDID = Convert.ToInt32(CommonLogic.QueryString("ibdid"));

                try
                {
                    object DBResult = DAOController.GetDataFromStoredProcedure(DRLExecuteCode.INVOICE_LIST_BY_INBOUND, new DBCriteria().INVOICE_LIST_BY_INBOUND(criteria));
                    if (DBResult != null)
                    {
                        Object ServiceResult = WMSController.Process(ServiceCall.INB_SUGGESTED_PUTAWAY, DBResult);
                        invDetails = (Inbound)ServiceResult;
                        lblStoreRefNo.Text = "Store Ref. No. : " + invDetails.InboundStoreRef;
                        lblBarStoreRefNo.Text = String.Format("<img width=\"250px\" src=\"Code39Handler.ashx?code={0} \"", invDetails.InboundStoreRef);
                        gvInvoiceList.DataSource = invDetails.invoices.Values.ToList();
                        gvInvoiceList.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    //there no locations to suggest
                    if (ex is ServiceException)
                    {
                        ServiceException se = (ServiceException)ex;
                        if (se.getErrorNo() == 15)
                            resetError("There is no location to suggest. Please map locations to the supplier in location manager ", true);
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

        protected void gvInvoiceList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                invoice = (SupplierInvoice)e.Row.DataItem;
                GridView gvLineItems = (GridView)e.Row.FindControl("gvLineItems");
                gvLineItems.DataSource = invoice.InvoiceLineItems;
                gvLineItems.DataBind();
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                invDetails = null;
            }
            
        }

        protected void gvLineItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                lineItem = (LineItem)e.Row.DataItem;
                GridView gvSuggestLocation = (GridView)e.Row.FindControl("gvSuggestLocation");

                if (lineItem.SuggestedLocations == null || lineItem.SuggestedLocations.Count<1)
                {
                    List<MRLWMSC21_Library.WMS_ServiceObjects.SuggestedLocation> suggestedLocations = new List<MRLWMSC21_Library.WMS_ServiceObjects.SuggestedLocation>();
                    suggestedLocations.Add(new MRLWMSC21_Library.WMS_ServiceObjects.SuggestedLocation());
                    gvSuggestLocation.DataSource = suggestedLocations;
                }
                else
                {
                    gvSuggestLocation.DataSource = lineItem.SuggestedLocations;
                }
                
                gvSuggestLocation.DataBind();
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                invoice = null;
            }
        }

        protected void gvSuggestLocation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                MRLWMSC21_Library.WMS_ServiceObjects.SuggestedLocation suggestedLoc = (MRLWMSC21_Library.WMS_ServiceObjects.SuggestedLocation)e.Row.DataItem;
                HyperLink hplPick = (HyperLink)e.Row.FindControl("hplPick");
                if (invDetails!=null &&  invoice !=null )
                {
                    hplPick.NavigateUrl = "../mInventory/StockIn.aspx?ibdno=" + invDetails.InboundId + "&mmid=" + lineItem.MaterialID + "&lno=" + lineItem.LineNo + "&poid=" + invoice.POID + "&invid=" + invoice.supplierInvID + "&Loc=" + suggestedLoc.LocationCode;
                }

                if (suggestedLoc.QTY == 0)
                {
                    hplPick.Text= "";
                    hplPick.CssClass = "";
                }
                
            }
            else if(e.Row.RowType==DataControlRowType.Footer)
            {
                lineItem = null;
            }
        }

        protected void lnkSearchMaterial_Click(object sender, EventArgs e)
        {

        }
    }
}