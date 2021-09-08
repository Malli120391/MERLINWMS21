using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using MRLWMSC21Common;

namespace MRLWMSC21.TPL
{
    public partial class DisplayPDF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

            int TenantID = 0; 
            String filename = CommonLogic.QueryString("filename");
            String path="";
            String TenantRootDir = "";
            String MMTPath="";
            TenantID = cp.TenantID;
            
            try
            {

                if (filename != null || filename != "")
                {
                    //path = Server.MapPath("~/");
                    //TenantRootDir = DB.GetSqlS("select DefaultValue AS S from SYS_SystemConfiguration SYS_C left join SYS_SysConfigKey SYS_K on SYS_K.SysConfigKeyID=SYS_C.SysConfigKeyID where TenantID=" + TenantID + " and SysConfigKey='TenantContentPath'");
                    //MMTPath = DB.GetSqlS(" select DefaultValue AS S from SYS_SystemConfiguration SYS_C left join SYS_SysConfigKey SYS_K on SYS_K.SysConfigKeyID=SYS_C.SysConfigKeyID where TenantID=" + TenantID + " and SysConfigKey='MaterialManagementPath'");
                    //path =path+TenantRootDir + DB.GetSqlS("select UniqueID AS S from GEN_Tenant where TenantID=" + TenantID) + MMTPath + mid + "/" + sid + "/";
                    path = Server.MapPath(@"~/TPL/InvoicePdf/");
                    path += "/" + filename;
                    WebClient client = new WebClient();
                    Byte[] buffer = client.DownloadData(path);
                    if(buffer!=null)
                    {
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-length", buffer.Length.ToString());

                        Response.BinaryWrite(buffer);
                    }
                }
            }
            catch (Exception ex)
            {
                ltmsg.Text = "<font color=red>Note*</font>: ";
                if (!(File.Exists(path)))
                {
                    ltmsg.Text += "Please Upload PDF File ";
                }
                else
                ltmsg.Text += ex.Message;


            }


        }
    }
}