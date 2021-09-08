using MRLWMSC21Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using Microsoft.AnalysisServices.AdomdClient;

namespace MRLWMSC21.mReports
{
    public partial class DemandForecastingReport : System.Web.UI.Page
    {
        public static CustomPrincipal cp;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            Page.Theme = "Outbound";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Demand Forecast Report");
            }
        }
        [WebMethod]
        public static string GetTimeSeries(string slidervalue)
        {
            try
            {
                

                using (AdomdConnection conn = new AdomdConnection(@"Data Source=192.168.1.241\devanalysis;Initial Catalog=SalesDemo; MDX Compatibility=1;"))
                {
                    conn.Open();
                    var mdxQuery = new StringBuilder();

                    mdxQuery.Append("SELECT Flattened PredictTimeSeries([Sales Data Analysis].[QUANTITY], " + slidervalue + "),[Sales Data Analysis].[SKU] From [Sales Data Analysis]");
                   
                    using (AdomdCommand cmd = new AdomdCommand(mdxQuery.ToString(), conn))
                    {
                        DataSet ds = new DataSet();
                        ds.EnforceConstraints = false;
                        ds.Tables.Add();
                        DataTable dt = ds.Tables[0];
                        dt.Load(cmd.ExecuteReader());
                        conn.Close();
                        return JsonConvert.SerializeObject(dt);
                    }
                   
                }

            
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}