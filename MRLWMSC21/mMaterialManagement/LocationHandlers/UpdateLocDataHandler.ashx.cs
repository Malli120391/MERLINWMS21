using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MRLWMSC21Common;
using System.Text;
using System.Data;

namespace MRLWMSC21.mMaterialManagement
{
    /// <summary>
    /// Summary description for UpdateLocDataHandler
    /// </summary>
    public class UpdateLocDataHandler : IHttpHandler
    {
        CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal; 

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //string Supplier=context.Request.QueryString["Supplier"];

            string _sLocs = context.Request.QueryString["locid"];
            if (_sLocs == "")
            {
                string ZoneID = context.Request.QueryString["ZoneID"];
                string zone = context.Request.QueryString["zone"];
                string rack = context.Request.QueryString["rack"];
                string col = context.Request.QueryString["col"];
                string lev = context.Request.QueryString["lev"];
               

                StringBuilder query = new StringBuilder();
                query.Append("DECLARE @Rack NVARCHAR(2)='" + rack + "', @Col NVARCHAR(2)='" + col + "', @Lev CHAR='" + lev + "' ");
                //query.Append(" SELECT Location FROM INV_Location ");
                query.Append(" SELECT LocationID FROM INV_Location ");
                query.Append(" WHERE ZoneId = '" + ZoneID + "' ");
                query.Append(" AND (@Col='0' OR Bay = @Col) ");
                query.Append(" AND (@Lev='0' OR Level = @Lev) ");
                query.Append(" AND (@Rack='0' OR Rack = @Rack) ");
                query.Append(" AND IsActive = 1 AND IsDeleted = 0 ");

                DataTable dt = DB.GetDS(query.ToString(), false).Tables[0];

                _sLocs = dt.AsEnumerable()
                    //.Select(row => row["Location"].ToString())
                    .Select(row => row["LocationID"].ToString())
                    .Aggregate((s1, s2) => String.Concat(s1 + "," + s2));

            }




            string sqlCommand = "EXEC [dbo].[sp_INV_UpdateLocation] @Location=" + DB.SQuote(_sLocs) + ",@Length=" + context.Request.QueryString["length"] + ",@Width=" + context.Request.QueryString["width"] + ",@Height=" + context.Request.QueryString["height"] + ",@MaxWeight=" + context.Request.QueryString["maxweight"] + ",@IsMixedMaterialOK=" + context.Request.QueryString["ismmok"]+ ",@IsFastMoving=" + context.Request.QueryString["isFM"]+ ",@MCode=" + DB.SQuote(context.Request.QueryString["MCode"]) + ",@IsActive=" + context.Request.QueryString["isactive"] + ",@IsQuarantine=" + context.Request.QueryString["isQuarantine"] + ",@Tenant=" + DB.SQuote(context.Request.QueryString["Tenant"]) + ",@Supplier=" + DB.SQuote(context.Request.QueryString["Supplier"]) + ",@CreatedBy=" + cp.UserID + ",@ZoneID=" + context.Request.QueryString["ZoneID"] + ",@LocationTypeID=" + context.Request.QueryString["LocType"] + ",@AccountID=" + cp.AccountID;

            //string sqlCommand = "exec [dbo].[sp_INV_UpdateLocation] @Location=" + DB.SQuote(context.Request.QueryString["locid"]) + ",@Length=" + context.Request.QueryString["length"] + ",@Width=" + context.Request.QueryString["width"] + ",@Height=" + context.Request.QueryString["height"] + ",@MaxWeight=" + context.Request.QueryString["maxweight"] + ",@IsMixedMaterialOK=" + context.Request.QueryString["ismmok"] + ",@FixedMaterialMasterID=" + context.Request.QueryString["mmid"] + ",@IsActive=" + context.Request.QueryString["isactive"] + ",@IsQuarantine=" + context.Request.QueryString["isQuarantine"];
            try
            {
                DB.ExecuteSQL(sqlCommand);
                context.Response.Write("Successfully Updated");
            }
            catch (Exception)
            {
                context.Response.Write("error occured");
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