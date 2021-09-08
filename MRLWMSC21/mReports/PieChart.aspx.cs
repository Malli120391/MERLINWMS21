using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using MRLWMSC21Common;
using System.Globalization;
using System.Configuration;
using System.Web.UI.DataVisualization.Charting;

namespace MRLWMSC21.mReports
{
    public partial class PieChart : System.Web.UI.Page
    {
        CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        protected void Page_Load(object sender, EventArgs e)
        {

            string[] rolesAllowedInThisPage = CommonLogic.GetRolesAllowed("All");

            if (!cp.IsInAnyRoles(rolesAllowedInThisPage))
            {
                Response.Redirect("Login.aspx?eid=6");
            }

            string[] AllowedRoleIDs = { "1", "2", "3", "4", "5", "7", "17", "19" };
            if (cp.IsInAnyRoles(AllowedRoleIDs))
            {
                
                LoadChartdata();

            }
           
        }
        private void LoadChartdata()
        {

            DataSet ds;
            //DataView dv;

            // Inbound and Outbound Activities
            ds = DB.GetDS("with test as( SELECT	sum(case when ProductionOrderStatusID=1 THEN 1 ELSE 0  END) AS [Initiated], Sum(case when ProductionOrderStatusID=2 THEN 1 ELSE 0  END) AS [InProcess], sum(case when ProductionOrderStatusID=3 THEN 1 ELSE 0  END) AS [Completed],sum(case when ProductionOrderStatusID=4 THEN 1 ELSE 0  END) AS [OnHold],sum(case when ProductionOrderStatusID=5 THEN 1 ELSE 0  END) AS [Closed],(select count(*) from MFG_ProductionOrderHeader) as TotalProductionOrder FROM MFG_ProductionOrderHeader  ) select name,value from (select Initiated,InProcess,Completed,OnHold,Closed from test)p UNPIVOT (value FOR name IN  (Initiated,InProcess,Completed,OnHold,Closed) )AS unpvt; ", false);
            //dv = ds.Tables[0].DefaultView;
            DataTable dt = ds.Tables[0];
            string[] x = new string[dt.Rows.Count];
            int[] y = new int[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                x[i] = dt.Rows[i][0].ToString();
                y[i] = Convert.ToInt32(dt.Rows[i][1]);
            }
            Chart1.Series[0].Points.DataBindXY(x, y);
            Chart1.Series[0].ChartType = SeriesChartType.Pie;
            Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            Chart1.Legends[0].Enabled = true;
        }
    }
}