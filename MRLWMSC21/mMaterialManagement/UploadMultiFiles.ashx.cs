using System;
using MRLWMSC21Common;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace MRLWMSC21.mMaterialManagement
{
    /// <summary>
    /// Summary description for UploadMultiFiles
    /// </summary>
    public class UploadMultiFiles : IHttpHandler
    {

       
        public void ProcessRequest(HttpContext context)
        {
            CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                /*String DocType = CommonLogic.QueryString("DocType");
                String mid = CommonLogic.QueryString("mid");
                String sidname = CommonLogic.QueryString("sid");
                String ddldocumentvalue = CommonLogic.QueryString("ddldocumentvalue");*/
                //String DocType =context.Request.Form.Get("DocType");

                String DoctypeID = context.Request.Form["Doctype"];
                String DocType = DB.GetSqlS("select FileName as S from MMT_FileType where FileTypeID=" + DoctypeID + "and IsActive=1 and IsDeleted=0");
                String mid = context.Request.Form["mid"];
                String sidname = context.Request.Form["sid"];
                String TenantID = context.Request.Form["Tid"];
                
                HttpPostedFile postedFile = context.Request.Files["FileNames"];
                int filecount = DB.GetSqlN("Declare @count int EXEC [sp_MMT_GetFileCount]	@MaterialMasterID=" + mid + ",@SupplierID=" + sidname + ",@FileTypeID=" + DoctypeID + ",@FileCouont=@count output Select @count as N");
                filecount++;
                String filename = DocType + "_" + mid + "_" + sidname + "_" + filecount + Path.GetExtension(postedFile.FileName);

                //String TenantID = cp.TenantID.ToString();
                //String MMTPath = DB.GetSqlS(" select DefaultValue AS S from SYS_SystemConfiguration SYS_C left join SYS_SysConfigKey SYS_K on SYS_K.SysConfigKeyID=SYS_C.SysConfigKeyID where TenantID=" + TenantID + " and SysConfigKey='MaterialManagementPath'");
                String MMTPath = DB.GetSqlS(" select DefaultValue AS S from SYS_SystemConfiguration SYS_C left join SYS_SysConfigKey SYS_K on SYS_K.SysConfigKeyID=SYS_C.SysConfigKeyID where SysConfigKey='MaterialManagementPath'");
                String TenantRootDir = DB.GetSqlS("select DefaultValue AS S from SYS_SystemConfiguration SYS_C left join SYS_SysConfigKey SYS_K on SYS_K.SysConfigKeyID=SYS_C.SysConfigKeyID where SysConfigKey='TenantContentPath'");


                String Savepath = System.Web.HttpContext.Current.Server.MapPath("~/") + TenantRootDir+"/" + TenantID + "/" + MMTPath + mid + "/" + sidname + "/";
                var directory = new DirectoryInfo(Savepath);
                if (!Directory.Exists(Savepath))
                    Directory.CreateDirectory(Savepath);
                postedFile.SaveAs(Savepath + @"\" + filename);
                
                DB.ExecuteSQL("EXEC [sp_MMT_UpsertMaterialMasterFileAttachment]  @MaterialMasterID= " + mid + ",@SupplierID=" + sidname + ",@FileTypeID=" + DoctypeID + ",@FileCount=" + filecount + ",@CreatedBy='" + cp.UserID + "'");
            }
            catch (Exception ex)
            {
                context.Response.Write("Error: " + ex.Message);

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