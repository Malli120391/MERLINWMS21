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
using System.Web.UI.DataVisualization.Charting;

namespace MRLWMSC21.mReports
{
    public partial class ChartControl : System.Web.UI.Page
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
                pnlCharts.Visible = true;
                LoadChartdata();                
                
            }
           
        }

        private void LoadChartdata()
        {

            DataSet ds;
            DataView dv;

            // Inbound and Outbound Activities
            ds = DB.GetDS("EXEC sp_CHT_GetInOutActivities", false);
            dv = ds.Tables[0].DefaultView;

            ChtInOutActivities.Series["Inbound"].Points.DataBindXY(dv, "WHCode", dv, "Inbound");
            ChtInOutActivities.Series["Outbound"].Points.DataBindXY(dv, "WHCode", dv, "Outbound");


            // Inbound activities
            ds = DB.GetDS("EXEC sp_CHT_GetInboundStatistics", false);
            dv = ds.Tables[0].DefaultView;

            chtInboundActivities.Series["Received"].Points.DataBindXY(dv, "ShipmentExpectedDate", dv, "Received");
            chtInboundActivities.Series["Verified"].Points.DataBindXY(dv, "ShipmentExpectedDate", dv, "Verified");
            chtInboundActivities.Series["GRNDone"].Points.DataBindXY(dv, "ShipmentExpectedDate", dv, "GRNDone");

            // Outbound activities
            ds = DB.GetDS("EXEC sp_CHT_GetOutboundStatistics", false);
            dv = ds.Tables[0].DefaultView;

            chtOutboundAcivities.Series["Orders"].Points.DataBindXY(dv, "OrderDate", dv, "Orders");
            chtOutboundAcivities.Series["PGIDone"].Points.DataBindXY(dv, "OrderDate", dv, "PGIDone");
            chtOutboundAcivities.Series["Delivered with POD"].Points.DataBindXY(dv, "OrderDate", dv, "Delivered with POD");


            // Received and consumed quantity by user
            ds = DB.GetDS("EXEC sp_CHT_GetReceivedAndConsumedQtybyUser", false);
            dv = ds.Tables[0].DefaultView;

            chtRcvdAndConQtybyUser.Series["ReceivedQuantity"].Points.DataBindXY(dv, "UserName", dv, "ReceivedQuantity");
            chtRcvdAndConQtybyUser.Series["ConsumedQuantity"].Points.DataBindXY(dv, "UserName", dv, "ConsumedQuantity");
                

            

            // Production order status
            ds = DB.GetDS("EXEC sp_CHT_GetProductionorderStatus", false);
           
            DataTable dt = ds.Tables[0];
            string[] x = new string[dt.Rows.Count];
            int[] y = new int[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                x[i] = dt.Rows[i][0] + " (" + dt.Rows[i][1].ToString() + ")".ToString();
                y[i] = Convert.ToInt32(dt.Rows[i][1]);
            }

            chtProductionOrderStatus.Series[0].Points.DataBindXY(x, y);
            chtProductionOrderStatus.Series[0].ChartType = SeriesChartType.Pie;
            chtProductionOrderStatus.ChartAreas["MainChartArea"].Area3DStyle.Enable3D = true;
            chtProductionOrderStatus.Legends[0].Enabled = true;
            //chtProductionOrderStatus.Series["Default"]["PieLabelStyle"] = "Disabled";
            chtProductionOrderStatus.Series["Default"].Points[0].Color = System.Drawing.Color.LightSeaGreen;
            chtProductionOrderStatus.Series["Default"].Points[1].Color = System.Drawing.Color.SteelBlue;
            chtProductionOrderStatus.Series["Default"].Points[2].Color = System.Drawing.Color.Thistle;
            chtProductionOrderStatus.Series["Default"].Points[3].Color = System.Drawing.Color.Gray;
            chtProductionOrderStatus.Series["Default"].Points[4].Color = System.Drawing.Color.LightSteelBlue;

            //chtProductionOrderStatus.Series["Default"].Label =  " + "#PERCENT{P1}" + "  ";

            chtProductionOrderStatus.Series["Default"].Label = "" + "#PERCENT{P1}" + "";
            chtProductionOrderStatus.Series["Default"].LegendText = "#VALX";             
           
        }
        
    }
}
