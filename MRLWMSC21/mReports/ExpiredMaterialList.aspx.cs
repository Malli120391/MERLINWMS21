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
    public partial class ExpiredMaterialList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lnkGetReport_Click(object sender, EventArgs e)
        {

            rvExpiredMaterialList.Visible = true;
            rvExpiredMaterialList.ServerReport.ReportServerUrl = new Uri(CommonLogic.Application("ReportServerUrl"));
            rvExpiredMaterialList.ServerReport.ReportPath = CommonLogic.Application("ReportPath") + "ExpiredMaterialList";
            rvExpiredMaterialList.ShowParameterPrompts = false;
            rvExpiredMaterialList.ShowPrintButton = true;

            rvExpiredMaterialList.ServerReport.Refresh();           
           
        }
    }
}