using MRLWMSC21Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRLWMSC21.mInventory
{
    public partial class ContainerTransfer : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected void page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Inventory";
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public DataSet LoadCantainerList(string ContainerCode)
        {
            DataSet DS = new DataSet();
            DS = DB.GetDS("EXEC sp_INV_GetAvailableQty_Container @CantinerCode = "+DB.SQuote(ContainerCode)+" ", false);
            return DS;
        }

        public void LoadContaimerList(DataSet DS)
        {
            gvcartonlist.DataSource = DS;
            gvcartonlist.DataBind();
            DS.Dispose();
        }

        protected void lnkGet_Click(object sender, EventArgs e)
        {
            if (txtCarton.Text == "" || txtCarton.Text == null)
            {
                resetError("Please select Cantainer", true);
                return;
            }
            this.LoadContaimerList(this.LoadCantainerList(txtCarton.Text));
        }
        private void resetError(string error, bool isError)
        {

            /*string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
            if (isError)
                str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

            if (error.Length > 0)
                str += error + "";
            else
                str = "";


            ltStatus.Text = str;*/
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "closeAsynchronus(); showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);

        }

        protected void lnkTransfer_Click(object sender, EventArgs e)
        {
            if (txtLocation.Text == "" || txtLocation.Text == null)
            {
                resetError("Please select Location", true);
                return;
            }
            decimal INOH = 0;
            decimal OBOH = 0;
            decimal AvaiableQty = 0;
            string Location = "";
            string TOLocation = txtLocation.Text;
            string Mcode = "";
            Literal FromLocation = new Literal();
            //foreach (GridViewRow row in gvcartonlist.Rows)
            //{
            //    Location = Convert.ToString((Literal)row.FindControl("ltLocation"));
            //    INOH = Convert.ToDecimal((Literal)row.FindControl("ltINOH"));
            //    OBOH = Convert.ToDecimal((Literal)row.FindControl("ltOBOH"));
            //    AvaiableQty = Convert.ToDecimal((Literal)row.FindControl("ltAvailableQty"));
            //    Mcode = Convert.ToString((Literal)row.FindControl("ltMcode"));
            //    //if (Location == TOLocation)
            //    //{
            //    //    resetError("From Location and To Location is same", true);
            //    //    return;
            //    //}
            //    //if (INOH != 0)
            //    //{
            //    //    resetError(""+Mcode+" is INOH Qty", true);
            //    //    return;
            //    //}
            //    //if (OBOH != 0)
            //    //{
            //    //    resetError("" + Mcode + " is OBOH Qty", true);
            //    //    return;
            //    //}
            //    //if (AvaiableQty != 0)
            //    //{
            //    //    resetError("There is no avaiable qty for "+Mcode+"", true);
            //    //    return;
            //    //}
            //}

            int Result = DB.GetSqlN("EXEC SP_UPSERT_CONTAINERINTERNALTRANSFER @ContainerCode = " + DB.SQuote(txtCarton.Text) + " , @UserID = " + cp.UserID + ", @Location = "+DB.SQuote(txtLocation.Text)+"");
            if (Result == 0)
            {
                resetError("Sucessfully transfer", false);
                this.LoadContaimerList(this.LoadCantainerList(txtCarton.Text));
                return;
            }
            else if (Result == -1)
            {
                resetError("Items in INOH Qty", true);
                return;
            }
            else if(Result == -2)
            {
                resetError("Items in INOH Qty", true);
                return;
            }
            else if (Result == -3)
            {
                resetError("Items in OBOH Qty", true);
                return;
            }
            else if (Result == -4)
            {
                resetError("Quantity not avaiable", true);
                return;
            }
        }

    }
}