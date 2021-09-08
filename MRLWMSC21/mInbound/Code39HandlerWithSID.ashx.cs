using GenCode128;
using KeepAutomation.Barcode.Bean;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using ZXing;

namespace MRLWMSC21.mInbound
{
    /// <summary>
    /// Summary description for Code39HandlerWithSID
    /// </summary>
    public class Code39HandlerWithSID : IHttpHandler
    {

        string code = "", supInvDetailsID = "";
        public void ProcessRequest(HttpContext context)
        {
            code = context.Request["code"];
            supInvDetailsID = context.Request["SIDETID"];
            if (code != null)
            {
                GenBarCode128(code, supInvDetailsID, context);
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

        private void GenBarCode128(String Code,String supInvDetailsID, HttpContext context)
        {
            //================== Barcode ==================//
            /*Image myimg = Code128Rendering.MakeBarcodeImage(Code + "|" + supInvDetailsID, 2, true);
            context.Response.ContentType = "image/jpeg";
            myimg.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);*/

            //================== QR Code =====================//
            /*BarCode qrcode = new BarCode();
            qrcode.Symbology = KeepAutomation.Barcode.Symbology.QRCode;
            qrcode.CodeToEncode = Code + "|" + supInvDetailsID;
            qrcode.X = 4;
            qrcode.Y = 4;
            qrcode.BarCodeHeight = 20;
            qrcode.BarCodeWidth = 20;
            context.Response.ContentType = "image/jpeg";
            Bitmap img = qrcode.generateBarcodeToBitmap();
            img.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Bmp);*/

            //======================= QR Code ==========================//

            //======================= Added By M.D.Prasad On 10-Apr-2020 For QR Code Using QR Coder ==========================//
            var QCwriter = new BarcodeWriter();
            QCwriter.Format = BarcodeFormat.QR_CODE;
            QCwriter.Options.Height = 400;
            QCwriter.Options.Width = 400;
            string qrCodeData = supInvDetailsID == "0" ? Code : Code + "|" + supInvDetailsID;
            var result = QCwriter.Write(qrCodeData);
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
            //========================== Added By M.D.Prasad On 10-Apr-2020 For QR Code Using QR Coder =============================//
        }
    }
}