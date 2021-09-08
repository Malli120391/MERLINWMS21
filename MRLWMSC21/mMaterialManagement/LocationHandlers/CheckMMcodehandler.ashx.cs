using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MRLWMSC21Common;

namespace MRLWMSC21.mMaterialManagement
{
    /// <summary>
    /// Summary description for CheckMMcodehandler
    /// </summary>
    public class CheckMMcodehandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string vMMID =String.Empty;

            string mmCode = context.Request.QueryString["mmcode"];
            string queryCommand = "Select MaterialMasterID as N from MMT_MaterialMaster Where MCode=" + DB.SQuote(mmCode);
            string result = DB.GetSqlN(queryCommand).ToString();

            if (result == "0")
            {
                vMMID = "0";
            }
            else
            {
                vMMID = result;
            }
            context.Response.Write(result);
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