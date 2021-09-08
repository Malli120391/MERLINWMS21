using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRLWMSC21.mReports
{
    public partial class InfoGraphics : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static MRLWMSC21.mReports.Models.ChartsModel.TotalWHData GETAllBinsData()
        {
            try
            {
                MRLWMSC21.mReports.Models.ChartsModel.TotalWHData lstData = new Models.ChartsModel.TotalWHData();
                MRLWMSC21.mReports.BL.ChartsBL TBins = new MRLWMSC21.mReports.BL.ChartsBL();
                lstData = TBins.GET_AllInventoryBinsData();
                return lstData;
            }
            catch (Exception ex)
            {
                //LogExceptionsInDB("BOMList", (MethodBase)MethodBase.GetCurrentMethod(), (MethodInfo)MethodInfo.GetCurrentMethod(), ex, "");
                return null;
            }
        }
    }
}