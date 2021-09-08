using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21Common;
using System.Data;
using MRLWMSC21.ServiceReference1;

namespace MRLWMSC21
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lnkSubmit_Click(object sender, EventArgs e)
        {
            if(txtEmailID.Text.Trim()=="")
            {
                lblError.Text = "Please Enter Email";
                //resetError("Please Enter Email",true);
                return;
            }
            //if (txtCaptcha.Text.Trim() == "")
            //{
            //    lblError.Text = "Please Enter Captcha Code";
            //    return;
            //}

            int userid = 0;

            string query = "select * from GEN_User where Email="+DB.SQuote(txtEmailID.Text.Trim());
            DataSet ds = DB.GetDS(query,false);
            if (ds.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    userid = Convert.ToInt32(row["SSOUserID"].ToString());
                }
            }
            else
            {
                lblError.Text = "Please Enter Valid Email";
                return;
            }

            //if (txtCaptcha.Text.ToLower() == Session["CaptchaVerify"].ToString())
            //{
                //lblCaptchaMessage.Text = "Captcha Correct";
                //lblCaptchaMessage.ForeColor = System.Drawing.Color.Green;
                ServiceReference1.SingleSignOnDBSinkClient obj = new ServiceReference1.SingleSignOnDBSinkClient();
                string result = obj.ForgetPassword(userid.ToString(), txtEmailID.Text.Trim());
                if (result == "")
                {
                    lblError.Text = "Please Check your Mail for further Process";
                    lblError.ForeColor = System.Drawing.Color.Green;
                    txtEmailID.Text = "";
                    txtCaptcha.Text = "";
                }
            //}
            //else
            //{
            //    lblError.Text = "Please enter correct captcha";
                
            //}
        }

        protected void resetError(string error, bool isError)
        {
            

            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);


        }
    }
}