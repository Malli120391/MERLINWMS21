using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MRLWMSC21Common;

namespace MRLWMSC21.mMaterialManagement
{
    /// <summary>
    /// Summary description for DeleteLocationHandler1
    /// </summary>
    public class DeleteLocationHandler : IHttpHandler
    {

        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string deletedLocations = context.Request.Params["dellocations"];
                int Output = DB.GetSqlN("[dbo].[sp_INV_DeleteLocation] @Locations='" + deletedLocations + "',@UpdatedBy=" + cp.UserID.ToString());
                if (Output == 1)
                {
                    context.Response.Write("success");
                }
                else
                {
                    context.Response.Write("failure");
                }
            }
            catch (Exception)
            {
                context.Response.Write("error");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}