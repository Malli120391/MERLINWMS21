using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MRLWMSC21Common;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using Neodynamic.SDK.Printing;
using GenCode128;
using KeepAutomation.Barcode.Bean;
using QRCoder;
using ZXing;


// Module Name : Generate BarCode Under Inbound
// Description : 
// DevelopedBy : Naresh P
// CreatedOn   : 11/11/2013
// Lsst Modified Date : 20/11/2013



namespace MRLWMSC21.mInbound
{
    /// <summary>
    /// Summary description for Code39Handler
    /// </summary>
    public class Code39Handler : IHttpHandler
    {

        string code = ""; 
        public void ProcessRequest(HttpContext context)
        {
            code = context.Request["code"];
            if (code != null)
            {
                GenBarCode128(code, context);
            }

            {
                //naresh
            }
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

        private byte[] GetImageBytes(Image image)
        {
            ImageCodecInfo codec = null;

            foreach (ImageCodecInfo e in ImageCodecInfo.GetImageEncoders())
            {
                if (e.MimeType == "image/png")
                {
                    codec = e;
                    break;
                }
            }

            using (EncoderParameters ep = new EncoderParameters())
            {
                ep.Param[0] = new EncoderParameter(Encoder.Quality, 20L);

                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, codec, ep);
                    return ms.ToArray();
                }
            }
        }



        private void GenBarCode128(String Code, HttpContext context)
        {

            string code1 = code.ToString();

            if (code1.Contains("DeliveryNote") == false)
            {

            Image myimg = Code128Rendering.MakeBarcodeImage(Code, 2, true);
            //pictBarcode.Image = myimg;

            // Then we send the Graphics with the actual barcode
            context.Response.ContentType = "image/jpeg";
            myimg.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);

            }
            else {

                for (int i = 0; i < 1; i++)
                {
                    //=================== Commneted By M.D.Prasad On 10-Apr-2020 ================//
                    /*
                    BarCode qrcode = new BarCode();
                qrcode.Symbology = KeepAutomation.Barcode.Symbology.QRCode;
                qrcode.CodeToEncode = Code.Replace("DeliveryNote","");
                qrcode.X = 4;
                qrcode.Y = 4;
                qrcode.BarCodeHeight = 20;
                qrcode.BarCodeWidth = 20;
                context.Response.ContentType = "image/jpeg";
                Bitmap img = qrcode.generateBarcodeToBitmap();
                img.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Bmp);
                //// qrcode.generateBarcodeToImageFile("C://qrcode-csharp.png");*/

                    //========================= Added By M.D.Prasad On 10-Apr-2020 For QR Code Using QR Coder ==============================//
                    var QCwriter = new BarcodeWriter();
                    QCwriter.Format = BarcodeFormat.QR_CODE;
                    QCwriter.Options.Height = 400;
                    QCwriter.Options.Width = 400;
                    var result = QCwriter.Write(Code.Replace("DeliveryNote", ""));
                    //string path = "D://qrcode-csharp.png";
                    var barcodeBitmap = new Bitmap(result);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        barcodeBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        byte[] byteImage = ms.ToArray();
                        //imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                        context.Response.ContentType = "image/png";
                        ms.WriteTo(context.Response.OutputStream);
                    }
                    //======================= Added By M.D.Prasad On 10-Apr-2020 For QR Code Using QR Coder ==========================//
                }
            }
        }



    }
}