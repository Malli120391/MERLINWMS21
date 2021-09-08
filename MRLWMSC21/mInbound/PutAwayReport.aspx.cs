using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using MRLWMSC21Common;

namespace MRLWMSC21.mInbound
{
    public partial class PutAwayReport : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Inbound";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this, "Put Away Statistics");
                LoadUsers();
            }
        }

        public void LoadUsers()
        {
            DataSet ds = DB.GetDS("SELECT * FROM GEN_User WHERE IsActive=1 and AccountID="+ cp.AccountID, false);
            ddlUser.DataSource = ds;
            ddlUser.DataTextField = "FirstName";
            ddlUser.DataValueField = "UserID";
            ddlUser.DataBind();

            int CurYear = DateTime.Now.Year; 
            for (int i = 0; i < 5; i++)
            {
                ddlYear.Items.Insert(i, (CurYear - i).ToString());
            }
        }

        [WebMethod]
        public static string GetPutAwayData(string year, string user)
        {
            DataSet ds = DB.GetDS("[sp_INB_GET_PutAwayStatistics] @Year=" + year + ", @User=" + user, false);

            return JsonConvert.SerializeObject(ds);

            //return JsonConvert.SerializeObject(GetSampleData());
        }


        public static DataSet GetSampleData()
        {
           

            DataTable dt1 = new DataTable();
            dt1.Columns.Add("Date");
            dt1.Columns.Add("Corrects");
            dt1.Columns.Add("Wrongs");
            dt1.Rows.Add("Jan 2017", "20", "40");
            dt1.Rows.Add("Feb 2017", "20", "20");
            dt1.Rows.Add("Mar 2017", "120", "40");
            dt1.Rows.Add("Apr 2017", "150", "40");
            dt1.Rows.Add("May 2017", "190", "40");
            dt1.Rows.Add("Jun 2017", "90", "40");
            dt1.Rows.Add("Jul 2017", "70", "30");
            dt1.Rows.Add("Aug 2017", "50", "40");
            dt1.Rows.Add("Sep 2017", "100", "70");
            dt1.Rows.Add("Oct 2017", "80", "40");
            dt1.Rows.Add("Nov 2017", "40", "20");
            dt1.Rows.Add("Dec 2017", "20", "10");
           

            DataSet ds = new DataSet();         
            ds.Tables.Add(dt1);

            return ds;
        }
    }
}