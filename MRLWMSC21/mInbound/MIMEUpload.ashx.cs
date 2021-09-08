using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace MRLWMSC21.mInbound
{
    /// <summary>
    /// Summary description for FileUpload
    /// </summary>
    public class MIMEUpload : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
            {
                string UploadFilesUrlPath = "";
                //List<string> UploadFilesUrlPath = new List<string>();
                if (context.Request.Files.Count > 0)
                {
                    HttpFileCollection SelectedFiles = context.Request.Files;
                    for (int i = 0; i < SelectedFiles.Count; i++)
                    {
                        HttpPostedFile postedFile = SelectedFiles[i];

                        string savepath = HttpContext.Current.Server.MapPath("~/GateEntryDocuments/");

                        var extension = Path.GetExtension(postedFile.FileName);

                        string img = DateTime.Now.ToString("ddMMyyyyhhmmss") + extension;

                        //var filesize = postedFile.ContentLength;
                        //string FileName = context.Server.MapPath("~/UploadedFiles/" + postedFile.FileName);
                        decimal filesize = Math.Round(((decimal)postedFile.ContentLength / (decimal)1024), 2);
                        var fileLocation = string.Format("{0}/{1}", savepath, img, filesize);

                        var filedataurl = fileLocation.Split('/')[1];
                        // UploadFilesUrlPath.Add(filedataurl.ToString()+",");

                        UploadFilesUrlPath = UploadFilesUrlPath + filedataurl.ToString() + ",";
                        Thread.Sleep(1000);
                        postedFile.SaveAs(fileLocation);

                    }
                }

                else
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("Please Select Files");
                }

                context.Response.ContentType = "text/plain";
                context.Response.Write(UploadFilesUrlPath);
                context.Response.StatusCode = 200;
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