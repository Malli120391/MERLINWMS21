using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21Common;
using Microsoft.Reporting.WebForms;

namespace MRLWMSC21.mReports
{
    public partial class SalesOrder : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected void Page_Load(object sender, EventArgs e)
        {
            ReportCommon.LoadDropDownfromXML(ddlPrinters, "PrinterIPSettings.xml");
           /* if (!IsPostBack)
            {

                ReportCommon.LoadDropDownfromXML(ddlPrinters, "PrinterIPSettings.xml");

                if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed("1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20")))
                {
                    Response.Redirect("Login.aspx?eid=6");
                }



                rvSOReport.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
                rvSOReport.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "SalesOrder";

                rvSOReport.ShowParameterPrompts = false;
                rvSOReport.ShowPrintButton = true;


                rvSOReport.ServerReport.Refresh();
            } */
        }

        protected void lnkGetReport_Click(object sender, EventArgs e)
        {
            rvSOReport.Visible = true;
            rvSOReport.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
            rvSOReport.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "SalesOrder";
            rvSOReport.ShowParameterPrompts = false;
            rvSOReport.ShowPrintButton = true;

            
            List<ReportParameter> _reportParameterCollection = new List<ReportParameter>();

            ReportParameter paramSONumber = new ReportParameter("SONumber");
            if (hifSONumber.Value != "" && atcSONumber.Text != "")
                paramSONumber.Values.Add(atcSONumber.Text);
            else
                paramSONumber.Values.Add(null);
            _reportParameterCollection.Add(paramSONumber);

            ReportParameter paramSODate = new ReportParameter("SODate");
            if (atcSODate.Text.ToString() != "")
                paramSODate.Values.Add(ReportCommon.FormatDate(atcSODate.Text, '/'));
            else
                paramSODate.Values.Add(null);
            _reportParameterCollection.Add(paramSODate);

            ReportParameter paramStatusID = new ReportParameter("SOStatusID");
            if (atcSOStatus.Text != "" && hifSOStatus.Value != "")
                paramStatusID.Values.Add(hifSOStatus.Value);
            else
                paramStatusID.Values.Add(null);
            _reportParameterCollection.Add(paramStatusID);

            ReportParameter paramSOTypeID = new ReportParameter("SOTypeID");
            if (atcSOType.Text != "" && hifSOType.Value != "")
                paramSOTypeID.Values.Add(hifSOType.Value);
            else
                paramSOTypeID.Values.Add(null);
            _reportParameterCollection.Add(paramSOTypeID);

           /* ReportParameter paramSupplierID = new ReportParameter("SupplierID");
            if (atcSupplier.Text != "" && hifSupplier.Value != "")
                paramSupplierID.Values.Add(hifSupplier.Value);
            else
                paramSupplierID.Values.Add(null);
            _reportParameterCollection.Add(paramSupplierID);  */

            ReportParameter paramFromDate = new ReportParameter("FromDate");
            if (atcFromDate.Text.ToString() != "")
                paramFromDate.Values.Add(ReportCommon.FormatDate(atcFromDate.Text, '/'));
            else
                paramFromDate.Values.Add(null);
            _reportParameterCollection.Add(paramFromDate);

            ReportParameter paramToDate = new ReportParameter("ToDate");
            if (atcToDate.Text.ToString() != "")
                paramToDate.Values.Add(ReportCommon.FormatDate(atcToDate.Text, '/'));
            else
                paramToDate.Values.Add(null);
            _reportParameterCollection.Add(paramToDate);

            rvSOReport.ServerReport.SetParameters(_reportParameterCollection);
            rvSOReport.ServerReport.Refresh();
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {

        }



    }
}