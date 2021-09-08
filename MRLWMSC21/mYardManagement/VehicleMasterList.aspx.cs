using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21Common;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;
using System.Web.Services;
using System.Web.Script.Services;
using System.Globalization;
using Newtonsoft.Json;

namespace MRLWMSC21.mYardManagement
{
    public partial class VehicleMasterList : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        public string gvYList = String.Empty;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            gvYList = "EXEC [dbo].[Usp_GET_YM_MST_Vehicles] @AccountID = " + cp.AccountID + ", @FreightCompanyID = 0, @YM_MST_VehicleType_ID = 0";
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Vehicle Master List");                
                GetVehicleList(gvYList);                
            }
        }

        protected void lnkVehicleListSearch_Click(object sender, EventArgs e)
        {
            if (Account.Value == "" || txtAccount.Value == "")
            {
                resetError("Please Select Account", true);
                return;
            }
            if (Freight.Value == "" || txtFreight.Value == "")
            {
                resetError("Please select Freight Company", true);
                return;
            }
            if (VehicleType.Value == "" || txtVehicleType.Value == "")
            {
                resetError("Please Select Vehicle Type", true);
                return;
            }
            string gvYList1 = "EXEC [dbo].[Usp_GET_YM_MST_Vehicles] @AccountID = " + Convert.ToInt32(Account.Value.ToString()) + ", @FreightCompanyID =" + Convert.ToInt32(Freight.Value.ToString()) + ",@YM_MST_VehicleType_ID = " + Convert.ToInt32(VehicleType.Value.ToString());
            GetVehicleList(gvYList1);
        }

        protected void resetError(string error, bool isError)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);
        }

        protected void gvYardList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvYardList.PageIndex = e.NewPageIndex;
            gvYardList.EditIndex = -1;
            GetVehicleList(gvYList);
        }

        public void GetVehicleList(string VList)
        {
            DataSet ds = DB.GetDS(VList.ToString(), false);
            gvYardList.DataSource = ds.Tables[0];
            gvYardList.DataBind();
        }        
    }
}