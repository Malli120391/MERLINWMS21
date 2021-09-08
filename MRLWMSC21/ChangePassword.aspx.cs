using MRLWMSC21Common;
using MRLWMSC21.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace MRLWMSC21
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            
            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            //Uri myUri = new Uri("http://www.example.com?param1=good¶m2=bad");
            Uri myUri = new Uri(url);
            string UserId = HttpUtility.ParseQueryString(myUri.Query).Get("UserID");
            string EmailId = HttpUtility.ParseQueryString(myUri.Query).Get("emailID");

            ltEmailId.Text = EmailId;
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {

            if (txtNewPassword.Text.Trim() == "")
            {
                lblError.Text = "Please Enter New Password";
                return;
            }
            if (txtConfirmPassword.Text.Trim() == "")
            {
                lblError.Text = "Please Enter Confirm Password";
                return;
            }
            if (txtNewPassword.Text.Trim() != txtConfirmPassword.Text.Trim())
            {
                lblError.Text = "Password Mismatches Please Enter Correct Password";
                return;
            }
            else
            {
            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            
            Uri myUri = new Uri(url);
            string UserId = HttpUtility.ParseQueryString(myUri.Query).Get("UserID");
            string EmailId = HttpUtility.ParseQueryString(myUri.Query).Get("emailID");

                //Commented by MD.Prasad // string EncNewPassword = Encrypt.EncryptData(CommonLogic.Application("EncryptKey"), txtConfirmPassword.Text.Trim());
                string EncNewPassword = CommonLogic.EncryptString(txtConfirmPassword.Text.Trim());
                ServiceReference1.SingleSignOnDBSinkClient obj = new ServiceReference1.SingleSignOnDBSinkClient();
                string result = obj.ChangePassword(EmailId, "", EncNewPassword, UserId, "2");

                if (result == "Updated Password")
                {
                    lblError.Text = "Password Updated Successfully";
                    lblError.ForeColor = System.Drawing.Color.Green;
                    txtNewPassword.Text = "";
                    txtConfirmPassword.Text = "";
                    return;

                }
                else
                {
                    lblError.Text = "Error While Updating Password";
                    return;
                }
            }
        }

       
    }
}