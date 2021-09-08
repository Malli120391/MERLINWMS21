using MRLWMSC21Common;
using MRLWMSC21.mOutbound;
using MRLWMSC21.mOutbounds;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLWMSC21.mOutbound.ITextSharp
{
    public class CommonPDF
    {
        float[] widths = null;
        Font HeaderFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 15f);
        Font miniHeader = new Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 10f);
        Font dataFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 8f);


        public string GenerateMonthlyBillingPDF(string logo,string folderPath, string fileName, Address fromAdd, Address toAdd, List<RateDetails> lstinboudRate,
            List<RateDetails> lstobdRate, List<RateDetails> lststorateRate, List<RateDetails> lstothRate,string fromDate,string toDate)
        {

            Document doc = new Document();
            doc.SetPageSize(PageSize.A4);
            doc.SetMargins(10, 10, 50, 10);
            //Create PDF Table  


            //Create a PDF file in specific path  
            PdfWriter.GetInstance(doc, new FileStream(fileName, FileMode.Create));


            //Open the PDF document  
            doc.Open();

            PdfPTable mainTable = new PdfPTable(2);
            widths = new float[] { 20f, 80f };
            mainTable.TotalWidth = 100f;
            mainTable.SetWidths(widths);


            // Adding Logo at left side
            PdfPCell cellLogo = new PdfPCell(GetPDFLogo(logo), false);
            cellLogo.PaddingLeft = 1;
            cellLogo.PaddingRight = 1;
            cellLogo.PaddingTop = 1;
            cellLogo.PaddingBottom = 1;
            cellLogo.MinimumHeight = 50f;

            cellLogo.Border = Rectangle.NO_BORDER;
            var FontColour = new BaseColor(153, 204, 255);
            cellLogo.BackgroundColor = FontColour;

            // Adding top row cell 1
            mainTable.AddCell(cellLogo);



            PdfPTable tabRightHeder = new PdfPTable(3);

            widths = new float[] { 40f, 30f, 30f };
            tabRightHeder.TotalWidth = 100f;
            tabRightHeder.SetWidths(widths);
            PdfPCell topCell = new PdfPCell(new Paragraph("        Billing Report", HeaderFont));
            topCell.Border = Rectangle.NO_BORDER;
            topCell.BackgroundColor = FontColour;
            tabRightHeder.AddCell(topCell);

            topCell = new PdfPCell(new Paragraph("", dataFont));
            topCell.Border = Rectangle.NO_BORDER;
            topCell.BackgroundColor = FontColour;
            tabRightHeder.AddCell(topCell);

            topCell = new PdfPCell(new Paragraph(" ", dataFont));
            topCell.Border = Rectangle.NO_BORDER;
            topCell.BackgroundColor = FontColour;
            tabRightHeder.AddCell(topCell);


            topCell = new PdfPCell(new Paragraph(" ", dataFont));
            topCell.Border = Rectangle.NO_BORDER;
            topCell.BackgroundColor = FontColour;
            tabRightHeder.AddCell(topCell);

            topCell = new PdfPCell(new Paragraph("From Date: "+ fromDate, dataFont));
            topCell.Border = Rectangle.NO_BORDER;
            topCell.BackgroundColor = FontColour;
            tabRightHeder.AddCell(topCell);

            topCell = new PdfPCell(new Paragraph("To Date:  "+toDate, dataFont));
            topCell.Border = Rectangle.NO_BORDER;
            topCell.BackgroundColor = FontColour;
            tabRightHeder.AddCell(topCell);


            PdfPCell billingRightCell = new PdfPCell(tabRightHeder);
            billingRightCell.Border = Rectangle.NO_BORDER;


            // Adding top row cell 2
            mainTable.AddCell(billingRightCell);




            //Add Content to PDF  
            doc.Add(mainTable);

            AddEmptyTable(doc);




            var billBoxBackColor = new BaseColor(226, 230, 225);
            PdfPTable table2 = new PdfPTable(2);
            widths = new float[] { 50f, 50f };
            table2.TotalWidth = 100f;
            table2.SetWidths(widths);

            string fromAddData = "";
            fromAddData += fromAdd.TenantName;
            fromAddData += "\n" + fromAdd.Address1;
            fromAddData += "\n" + fromAdd.Address2;
            fromAddData += "\n" + fromAdd.City;
            fromAddData += "\n" + fromAdd.State;
            fromAddData += "\n" + fromAdd.ZIP;

            table2.AddCell(new PdfPCell(new Phrase("Billing To : ", miniHeader)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 15, BackgroundColor = billBoxBackColor, Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER });
            table2.AddCell(new PdfPCell(new Phrase("Billing From : ", miniHeader)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 15, BackgroundColor = billBoxBackColor, Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER });




            PdfPCell addressLeftCell = new PdfPCell(new Paragraph(fromAddData, dataFont)) { BackgroundColor = billBoxBackColor, Border = Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER };
            table2.AddCell(addressLeftCell);



            string toAddressData = "";
            toAddressData += toAdd.TenantName;
            toAddressData += "\n" + toAdd.Address1;
            toAddressData += "\n" + toAdd.Address2;
            toAddressData += "\n" + toAdd.City;
            toAddressData += "\n" + toAdd.State;
            toAddressData += "\n" + toAdd.ZIP;
            PdfPCell addressRightCell = new PdfPCell(new Paragraph(toAddressData, dataFont)) { BackgroundColor = billBoxBackColor, Border = Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER };
            table2.AddCell(addressRightCell);





            doc.Add(table2);


            AddEmptyTable(doc);






            AddBillingTable("Inbound", lstinboudRate, doc);
            AddEmptyTable(doc);
            AddBillingTable("Outbound", lstobdRate, doc);
            AddEmptyTable(doc);
            AddBillingTable("Storage", lststorateRate, doc);

            AddEmptyTable(doc);
            AddBillingTable("Others", lstothRate, doc);

            AddEmptyTable(doc);
            AddEmptyTable(doc);

            decimal totalValue =(decimal) (lstinboudRate.Sum(R => R.Vaue) + lstobdRate.Sum(R => R.Vaue) + lststorateRate.Sum(R => R.Vaue) + lstothRate.Sum(R => R.Vaue));
       
            AddGrandTotalTable(doc, totalValue.ToString("F3"));







            // Closing the document  
            doc.Close();


            return "";
        }

        public void AddEmptyTable(Document doc)
        {
            PdfPTable emptyTable = new PdfPTable(1);
            PdfPCell cell = new PdfPCell(new Paragraph("\n", HeaderFont));
            cell.Border = Rectangle.NO_BORDER;
            emptyTable.AddCell(cell);
            doc.Add(emptyTable);
        }

        public void AddBillingTable(string BillingType, List<RateDetails> billingDetails, Document doc)
        {
            var headerBoxBackColor = new BaseColor(211, 219, 212);

            PdfPTable RateHeaderTable = new PdfPTable(3);
            widths = new float[] { 40f, 20f, 40f };
            RateHeaderTable.HorizontalAlignment = iTextSharp.text.Image.ALIGN_CENTER;
            RateHeaderTable.SetWidths(widths);
            RateHeaderTable.WidthPercentage = 75f;



            PdfPCell cell = new PdfPCell(new Paragraph("", HeaderFont));
            cell.Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER;
            cell.BackgroundColor = headerBoxBackColor;
            RateHeaderTable.AddCell(cell);

            cell = new PdfPCell(new Paragraph(BillingType, HeaderFont));
            cell.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;
            cell.BackgroundColor = headerBoxBackColor;
            RateHeaderTable.AddCell(cell);

            cell = new PdfPCell(new Paragraph("", HeaderFont));
            cell.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
            cell.BackgroundColor = headerBoxBackColor;
            RateHeaderTable.AddCell(cell);

            doc.Add(RateHeaderTable);


            PdfPTable RateDetails = new PdfPTable(2);

            RateDetails.HorizontalAlignment = iTextSharp.text.Image.ALIGN_CENTER;
            widths = new float[] { 50f, 20f };
            RateDetails.SetWidths(widths);
            RateDetails.WidthPercentage = 75f;

            foreach (RateDetails rate in billingDetails)
            {
                PdfPCell rateCell = new PdfPCell(new Paragraph(rate.ActivityNAme, dataFont));
                RateDetails.AddCell(rateCell);
                rateCell = new PdfPCell(new Paragraph(rate.Vaue.ToString("F3"), dataFont));
                rateCell.HorizontalAlignment = 2;
                RateDetails.AddCell(rateCell);
            }

            doc.Add(RateDetails);


            PdfPTable rateTotalTable = new PdfPTable(3);
            widths = new float[] { 40f, 20f, 10f};
            rateTotalTable.HorizontalAlignment = iTextSharp.text.Image.ALIGN_CENTER;
            rateTotalTable.SetWidths(widths);
            rateTotalTable.WidthPercentage = 75f;



            cell = new PdfPCell(new Paragraph("", dataFont));
            cell.Border = Rectangle.NO_BORDER;
            rateTotalTable.AddCell(cell);


        


            cell = new PdfPCell(new Paragraph("Total in KWD", dataFont) );
            rateTotalTable.AddCell(cell);
            cell = new PdfPCell(new Paragraph(billingDetails.Sum(r => r.Vaue).ToString("F3"), dataFont));
            cell.HorizontalAlignment = 2;
            rateTotalTable.AddCell(cell);


            doc.Add(rateTotalTable);



        }


        public void AddGrandTotalTable(Document doc,string totalValue)
        {


            var headerBoxBackColor = new BaseColor(153, 204, 255);


            PdfPTable table = new PdfPTable(2);

            table.HorizontalAlignment = iTextSharp.text.Image.ALIGN_CENTER;
            widths = new float[] { 50f, 20f };
            table.SetWidths(widths);
            table.WidthPercentage = 75f;

            PdfPCell rateCell = new PdfPCell(new Paragraph("Grand Total in KWD", HeaderFont));
            rateCell.BackgroundColor = headerBoxBackColor;
            rateCell.HorizontalAlignment = 1;
            table.AddCell(rateCell);
            rateCell = new PdfPCell(new Paragraph(totalValue, HeaderFont));
            rateCell.BackgroundColor = headerBoxBackColor;
            rateCell.HorizontalAlignment = 2;
            table.AddCell(rateCell);




            doc.Add(table);

        }


            private iTextSharp.text.Image GetPDFLogo(string picLocation)
        {

            try
            {
                System.Drawing.Image image = System.Drawing.Image.FromFile(picLocation);
                iTextSharp.text.Image logo = null;

                MemoryStream imageStream = new MemoryStream();
                image.Save(imageStream, System.Drawing.Imaging.ImageFormat.Png);
                logo = iTextSharp.text.Image.GetInstance(imageStream.ToArray());

                logo.ScaleToFit(150, 30);
                logo.Alignment = iTextSharp.text.Image.ALIGN_RIGHT;
                image.Dispose();
                return logo;
            }
            catch (Exception ex)
            {
                throw new Exception("exception while writing logo in pdf:" + ex.Message);
            }

        }

        public class Address
        {
            public string TenantName { set; get; }
            public string Address1 { set; get; }
            public string Address2 { set; get; }
            public string City { set; get; }
            public string State { set; get; }
            public string ZIP { set; get; }
        }
        public class RateDetails
        {
            public string ActivityNAme { set; get; }
            public float Vaue { set; get; }

        }
    }
}