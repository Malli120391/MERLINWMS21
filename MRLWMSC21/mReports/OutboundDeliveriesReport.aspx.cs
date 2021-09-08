using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;
using MRLWMSC21Common;

namespace MRLWMSC21.mReports
{
    public partial class OutboundDeliveriesReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDocType(ddlDocumentType);
                LoadDeliveryStatus(ddlDeliveryStatus);
            }
        }        

        private void LoadDocType(DropDownList ddlDocType)
        {
            ddlDocType.Items.Clear();
            ddlDocType.Items.Add(new ListItem("All", ""));
            IDataReader rsDocType = DB.GetRS("Select DocumentTypeID, DocumentType from GEN_DocumentType Where IsActive =1");

            while (rsDocType.Read())
            {
                ddlDocType.Items.Add(new ListItem(rsDocType["DocumentType"].ToString(), rsDocType["DocumentTypeID"].ToString()));

            }
            rsDocType.Close();
        }

        private void LoadDeliveryStatus(DropDownList ddlDelStatus)
        {
            ddlDelStatus.Items.Clear();
            ddlDelStatus.Items.Add(new ListItem("All", ""));
            IDataReader rsDelStatus = DB.GetRS("Select DeliveryStatusID, DeliveryStatus from OBD_DeliveryStatus where IsActive=1 AND DeliveryStatusID NOT IN (3,8,9,10,11)");

            while (rsDelStatus.Read())
            {
                ddlDelStatus.Items.Add(new ListItem(rsDelStatus["DeliveryStatus"].ToString(), rsDelStatus["DeliveryStatusID"].ToString()));
            
            }
            rsDelStatus.Close();
        }

        protected void lnkGetReport_Click(object sender, EventArgs e)
        {
            rvOutboundDeliveriesReport.Visible = true;
            rvOutboundDeliveriesReport.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
            rvOutboundDeliveriesReport.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "OutboundDeliveries";
            rvOutboundDeliveriesReport.ShowParameterPrompts = false;
            rvOutboundDeliveriesReport.ShowPrintButton = true;

            List<ReportParameter> _reportParameterCollection = new List<ReportParameter>();

            ReportParameter paramDocumentType = new ReportParameter("DocumentTypeID");
            if (ddlDocumentType.SelectedValue != "")
                paramDocumentType.Values.Add(ddlDocumentType.SelectedValue);
            else
                paramDocumentType.Values.Add(null);
            _reportParameterCollection.Add(paramDocumentType);


            ReportParameter paramDeliveryStatus = new ReportParameter("DeliveryStatusID");
            if (ddlDeliveryStatus.SelectedValue != "")
                paramDeliveryStatus.Values.Add(ddlDeliveryStatus.SelectedValue);
            else
                paramDeliveryStatus.Values.Add(null);
            _reportParameterCollection.Add(paramDeliveryStatus);


            ReportParameter paramFromDate = new ReportParameter("FromDate");
            if (ObdFromDate.Text.ToString() != "")
                paramFromDate.Values.Add(ReportCommon.FormatDate(ObdFromDate.Text, '/'));
            else
                paramFromDate.Values.Add(null);
            _reportParameterCollection.Add(paramFromDate);


            ReportParameter paramToDate = new ReportParameter("ToDate");
            if (ObdToDate.Text.ToString() != "")
                paramToDate.Values.Add(ReportCommon.FormatDate(ObdToDate.Text, '/'));
            else
                paramToDate.Values.Add(null);
            _reportParameterCollection.Add(paramToDate);

            rvOutboundDeliveriesReport.ServerReport.SetParameters(_reportParameterCollection);
            rvOutboundDeliveriesReport.ServerReport.Refresh();
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        { 

        }
        
    }
}