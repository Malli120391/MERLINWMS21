using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Security.Principal;
using MRLWMSC21Common;
using MRLWMSC21.ServiceReference1;
using System.Text.RegularExpressions;

namespace MRLWMSC21
{
    public partial class MyProfile : System.Web.UI.Page
    {
        CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "blue_theme";
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed("1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20")))
            //{
            //    Response.Redirect("Login.aspx?eid=6");
            //}
            //String NameandEmail = DB.GetSqlS("Select ('Name:' + FirstName  + '<br/><br/> Email:' +  Email) as S from Users Where userid=" + cp.UserID.ToString());

            if (!IsPostBack)
            {
                IDataReader rsUserInfo = DB.GetRS("Select * from GEN_User Where UserGUID=" + DB.SQuoteNotUnicode(cp.UserGUID.ToString()));

                while (rsUserInfo.Read())
                {
                    lblInfo.Text = "Full Name :" + DB.RSField(rsUserInfo, "FirstName") + DB.RSField(rsUserInfo, "LastName") + "<br/><br/> Email :" + DB.RSField(rsUserInfo, "Email");
                    txtMobile.Text = DB.RSField(rsUserInfo, "Mobile");
                }
                rsUserInfo.Close();
            }
        }


        protected void lnkChangePassword_Click(object sender, EventArgs e)
        {



            //string Email = txtMobile.Text.Trim();
            //Commented by MD.Prasad //string EncOldPassword = Encrypt.EncryptData(CommonLogic.Application("EncryptKey"), txtOldPassword.Text.Trim());
            //string EncNewPassword = DB.SQuote(Encrypt.EncryptData(CommonLogic.Application("EncryptKey"), txtOldPassword.Text.Trim()));
            //Commented by MD.Prasad // string EncNewPassword = Encrypt.EncryptData(CommonLogic.Application("EncryptKey"), txtConfirmPassword.Text.Trim());

            /*string EncOldPassword = CommonLogic.EncryptString(txtOldPassword.Text.Trim());
            string EncNewPassword = CommonLogic.EncryptString(txtConfirmPassword.Text.Trim());
            string userid = cp.UserID.ToString();


            ServiceReference1.SingleSignOnDBSinkClient obj = new ServiceReference1.SingleSignOnDBSinkClient();
            string result = obj.ChangePassword(Email, EncOldPassword, EncNewPassword, userid, "1");

            if (result == "1")
            {
                resetError("Password Updated Succesfully", true);
                return;
            }
            else if (result == "-2")
            {
                resetError("Password Mismatches Please Enter Correct Old Password", true);
                return;
            }
            else
            {
                resetError("Error While updating Password", true);
                return;
            }*/

            this.Page.Validate("valLogin");
            if (Page.IsValid)
            {
                bool status = ValidatePassword(txtNewPassword.Text);
                if (status == false)
                {
                    resetError("New Password : contain minimum 8 characters atleast 1 Alphabet, 1 Number and 1 Special Character", false);
                    return;
                }
                bool status1 = ValidatePassword(txtConfirmPassword.Text);
                if (status1 == false)
                {
                    resetError("Re-enter New-Password : contain minimum 8 characters atleast 1 Alphabet, 1 Number and 1 Special Character", false);
                    return;
                }
                if (DB.GetSqlS("Select Enpassword as S from GEN_User Where UserGUID=" + DB.SQuoteNotUnicode(cp.UserGUID.ToString())) != CommonLogic.EncryptString(txtOldPassword.Text))
                {
                    resetError("Sorry, your current password does not match with the one in the database.Please change your current and try again", true);
                    return;
                }

                try
                {
                    if (txtNewPassword.Text.Trim() != "")
                    {
                        if (txtConfirmPassword.Text.Trim() == "" || txtConfirmPassword.Text.Trim() == null)
                        {
                            resetError("Please enter Re-enter New-Password.", false);
                            return;
                        }
                        if (txtNewPassword.Text.Trim() == txtConfirmPassword.Text.Trim())
                        {
                            try
                            {
                                DB.ExecuteSQL("Update GEN_User SET mobile =" + DB.SQuote(txtMobile.Text.Trim()) + ",Password='" + txtNewPassword.Text + "',Enpassword =" + DB.SQuote(CommonLogic.EncryptString(txtNewPassword.Text)) + " Where UserGUID=" + DB.SQuoteNotUnicode(cp.UserGUID.ToString()));
                                Session.Clear();
                                Session.Abandon();
                                FormsAuthentication.SignOut();
                                Response.Redirect("~/Login.aspx");
                            }
                            catch
                            {
                                resetError("Error while updaing profile", false);
                            }
                        }
                        else
                        {
                            resetError("Your passwords do not match up!", false);
                            return;
                        }
                    }
                    else
                    {
                        try
                        {
                            DB.ExecuteSQL("Update GEN_User SET mobile =" + DB.SQuote(txtMobile.Text.Trim()) + " Where UserGUID=" + DB.SQuoteNotUnicode(cp.UserGUID.ToString()));
                            resetError("Profile is updated successfully", false);
                        }
                        catch
                        {
                            resetError("Error while updaing profile", false);
                        }
                    }


                }
                catch
                {
                    resetError("Error updating the password in the database", false);
                }
            }
            else
            {
                //resetError("Please enter mandatory fields.", false);
                //return;
            }
        }

        public bool ValidatePassword(string password)
        {
            string patternPassword = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$";
            if (!string.IsNullOrEmpty(password))
            {
                if (!Regex.IsMatch(password, patternPassword))
                {
                    return false;
                }
            }
            return true;
        }


        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(FormsAuthentication.GetRedirectUrl(cp.FirstName, false), false);
        }

        protected void resetError(string error, bool isError)
        {

            string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
            if (isError)
                str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

            if (error.Length > 0)
                str += error + "";
            else
                str = "";


            ltStatus.Text = str;



        }

    }
}
