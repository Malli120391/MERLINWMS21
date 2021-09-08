using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace MRLWMSC21.mWebServices
{
    /// <summary>
    /// Summary description for PODUpload
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class PODUpload : System.Web.Services.WebService
    {

        [WebMethod]
        public bool UploadFile(String PathName, Byte[] FileControl, String FileName)
        {
            bool status = false;


            try
            {
                //String inputFilePath = @"C:\demo.jpg";
                //byte[] arr = File.ReadAllBytes(inputFilePath);
                ////
                var _Path = System.Web.HttpContext.Current.Server.MapPath("~/" + PathName);
                //var file = Path.Combine(_Path, FileName);
                //MemoryStream inputStream = new MemoryStream(FileControl);
                //Bitmap bmp = new Bitmap(inputStream);
                //List<Bitmap> images = new List<Bitmap>();
                //images.Add(bmp);
                //PDFDocument doc = new PDFDocument(images.ToArray());
                //MemoryStream outputStream = new MemoryStream();
                //doc.Save(outputStream);
                //MemoryStream ms = new MemoryStream(FileControl);

                //var _Path = System.Web.HttpContext.Current.Server.MapPath(WCFURL);
                //var _Path =Uri. WCFURL;
                //Uri uri = new Uri(WCFURL);
                //var _Path = System.Web.HttpContext.Current.Server.MapPath("~/" + PathName);
                //var _Path = uri.ToString();
                var _Directory = new DirectoryInfo(_Path.ToString());

                if (_Directory.Exists == false)
                {
                    _Directory.Create();
                }
                MemoryStream ms = new MemoryStream(FileControl, 0, FileControl.Length);
                ms.Write(FileControl, 0, FileControl.Length);
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                var file = Path.Combine(_Path, FileName);
                image.Save(file, System.Drawing.Imaging.ImageFormat.Png);
                //ConvertImageToPdf("Image.png", "SamplePDF.pdf");
                // instance a filestream pointing to the
                // storage folder, use the original file name
                // to name the resulting file
                //FileStream fs = new FileStream
                //    (_Path, FileMode.Create);

                // write the memory stream containing the original
                // file as a byte array to the filestream
                //ms.WriteTo(fs);

                // clean up
                //ms.Close();
                //fs.Close();
                //fs.Dispose();

                status = true;

            }
            catch (Exception ex)
            {
                return false;
            }


            return status;


        }

        public static void ConvertImageToPdf(string srcFilename, string dstFilename)
        {
            iTextSharp.text.Rectangle pageSize = null;

            using (var srcImage = new Bitmap(System.Web.HttpContext.Current.Server.MapPath(srcFilename)))
            {
                pageSize = new iTextSharp.text.Rectangle(0, 0, srcImage.Width, srcImage.Height);
            }
            using (var ms = new MemoryStream())
            {
                var document = new iTextSharp.text.Document(pageSize, 0, 0, 0, 0);
                iTextSharp.text.pdf.PdfWriter.GetInstance(document, ms).SetFullCompression();
                document.Open();
                var image = iTextSharp.text.Image.GetInstance(System.Web.HttpContext.Current.Server.MapPath(srcFilename));
                document.Add(image);
                document.Close();

                File.WriteAllBytes(System.Web.HttpContext.Current.Server.MapPath(dstFilename), ms.ToArray());
            }
        }
    }
}
