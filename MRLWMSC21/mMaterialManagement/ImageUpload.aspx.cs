using MRLWMSC21Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRLWMSC21.mMaterialManagement
{
    public partial class ImageUpload : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "MasterData";
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            DesignLogic.SetInnerPageSubHeading(this.Page, "Logo Upload");
        }
        protected void lnkflupldImage_Click(object sender, EventArgs e)
        {
            try
            {
                string filename = "Logo_Header_falcon2.png";
                var filePath = Server.MapPath("~/Images/" + filename);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                if (flupldImage.HasFile)
                {
                    string fileName = Path.GetFileName(flupldImage.PostedFile.FileName);
                    flupldImage.PostedFile.SaveAs(Server.MapPath("~/Images/") + fileName);
                    // Response.Redirect(Request.Url.AbsoluteUri);
                }
            }
            catch (Exception ex)
            {
            }


        }
    }
}