using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MRLWMSC21Common;

namespace MRLWMSC21
{
    public partial class LocationPrinter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lnkPrintLabel_Click(object sender, EventArgs e)
        {
            String result="";
            //DataSet ds = DB.GetDS("Select Location from INV_Location WHERE LEFT(Location,2) ='" + ddlPrintlabel.SelectedValue + "'", false);
            DataSet ds = null;
            if (ddlPrintlabel.SelectedValue != "S1")
            {
                 ds = DB.GetDS("Select Location from INV_Location WHERE LEFT(Location,2) ='" + ddlPrintlabel.SelectedValue + "'", false);
            }
            else
            {
                 ds = DB.GetDS("Select Location from INV_Location WHERE LEFT(Location,4) ='" + ddlRack.SelectedValue + "'", false);
            }
           // DataSet ds = DB.GetDS("Select Location from INV_Location WHERE LEFT(Location,4) ='S101'", false);
            DataTable dt=ds.Tables[0];

            DataTable mDT = new DataTable();

            DataColumn DC = new DataColumn("EmployeeCode",typeof(string));
            
            mDT.Columns.Add(DC);

            mDT.Rows.Add("sss");


            

            MRLWMSC21Common.CommonLogic.SendPrintJob_Location_2x1p5("", dt,"172.18.0.127" ,out result);
          lblStatus.Text = "";
          lblStatus.Text = result;
        }
    }
}