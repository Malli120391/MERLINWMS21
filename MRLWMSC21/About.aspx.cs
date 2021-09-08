using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using MRLWMSC21Common;

namespace MRLWMSC21
{
    public partial class About : Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            
    

          
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            /*
            lblServiceDetails.Text = objService.myTestService();

            DataSet ds = (DataSet)XmlCommon.DeSerializeAnObject(objService.getTestService("select top 10 * from [FinchWMS].[dbo].[MMT_MaterialMaster]").ToString(), new DataSet());

            IDataReader rsList = ds.Tables[0].CreateDataReader();

            while (rsList.Read())
            {

                lblServiceDetails.Text = DB.RSFieldInt(rsList, "MaterialMasterID").ToString();
                break;

            }
            */
        }

        public string LinkButton1_Click1()
        {
            string customerName = Request.Form[txtTestSearch.UniqueID];
            string customerId = Request.Form[hftxtSearch.UniqueID];
            lblTest.Text = customerId;
            return "";
        }
    }
}