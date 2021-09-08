using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MRLWMSC21Common;

namespace MRLWMSC21.mManufacturingProcess
{
    public partial class DashBoard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string WorkCenterGroupData = "";
            string PRORefNoDetails = "";
            try
            {

                IDataReader reader = DB.GetRS("[dbo].[sp_MFG_GetDashBoardDataforProduction]");
                while (reader.Read())
                {
                    PRORefNoDetails += DB.RSField(reader, "PRORefNo") + "^";
                    PRORefNoDetails += DB.RSFieldInt(reader, "ProductionQuantity") + "^";
                    PRORefNoDetails += DB.RSFieldDateTime(reader, "StartDate").ToString("dd/MMM/yy") +"^";
                    PRORefNoDetails += DB.RSFieldDateTime(reader, "DueDate").ToString("dd/MMM/yy") + ",";
                    WorkCenterGroupData +=DB.RSField(reader, "WorkCenterData") + "^";
                  
                }
                WorkCenterGroupData = WorkCenterGroupData.Remove(WorkCenterGroupData.Length - 1, 1);
                lbWorkCrenterdata.Text = WorkCenterGroupData;
                PRORefNoDetails = PRORefNoDetails.Remove(PRORefNoDetails.Length - 1, 1);
                lblProRefNumbers.Text = PRORefNoDetails;
            }
            catch
            {
            }
        }
    }
}