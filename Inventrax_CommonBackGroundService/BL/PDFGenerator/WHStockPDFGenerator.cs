using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using MRLWMSC21Common;
using iTextSharp.text.pdf;
using System.Configuration;

namespace Inventrax_CommonBackGroundService.BL.PDFGenerator
{
    class WHStockPDFGenerator : IPDFGenerator
    {
        public void Create(DataTable dtResult, string tenantName, string warehouseName, string fileName)
        {
            int defaultSize = 2000;

            //int dynamicSize = dtResult.Rows[0].Table.Columns.Count * 100;

            //if (dynamicSize > defaultSize)
            //{
            //    defaultSize = dynamicSize;

            //}

            var pgSize = new iTextSharp.text.Rectangle(2000, 842);
            Document pdfDoc = new iTextSharp.text.Document(pgSize, 9f, 9f, 9f, 0f);
            MemoryStream memoryStream = new MemoryStream();
            FileStream fileStream = new FileStream(fileName, FileMode.Create);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, fileStream);
            pdfDoc.Open();


            #region Heading section start
            PdfPTable tableHead = new PdfPTable(2);
            tableHead.TotalWidth = 2000;
            string folderPath = ConfigurationManager.AppSettings["ExcellFolderPath"];
            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(folderPath + @"\shipperlogo.png");
            
            PdfPCell cell = new PdfPCell(jpg);
            cell.Border = 0;
            tableHead.AddCell(cell);
            iTextSharp.text.Font fdefault = FontFactory.GetFont("Arial", 50, BaseColor.RED);
            cell = new PdfPCell(new Phrase("Warehouse Stock summary Report", fdefault));
            cell.Border = 0;
            tableHead.AddCell(cell);

            pdfDoc.Add(tableHead);
            #endregion

            #region Tenat & Warehouse Details section start

            iTextSharp.text.Font tenantFont = FontFactory.GetFont("Arial", 20, BaseColor.BLACK);
            PdfPTable whDetailstable = new PdfPTable(1);
            whDetailstable.TotalWidth = 2000;

            cell = new PdfPCell(new Phrase("Tenant :  " + tenantName, tenantFont));
            cell.Border = 0;
            whDetailstable.AddCell(cell);

            cell = new PdfPCell(new Phrase("Warehouse : " + warehouseName, tenantFont));
            cell.Border = 0;
            whDetailstable.AddCell(cell);

            cell = new PdfPCell(new Phrase("    ", tenantFont));
            cell.Border = 0;
            whDetailstable.AddCell(cell);



            pdfDoc.Add(whDetailstable);

            #endregion

            #region SKu Details Section

            PdfPTable table = new PdfPTable(dtResult.Rows[0].Table.Columns.Count);
            table.HeaderRows = 1;
            table.TotalWidth = 2000;

            iTextSharp.text.Font tableHeadingFont = FontFactory.GetFont("Arial", 15, 1, BaseColor.BLACK);
            foreach (DataColumn dc in dtResult.Rows[0].Table.Columns)
            {


                cell = new PdfPCell(new Phrase(dc.ToString(), tableHeadingFont));

                table.AddCell(cell);

            }

            foreach (DataRow dr in dtResult.Rows)
            {

                foreach (DataColumn dc in dr.Table.Columns)
                {
                    table.AddCell(dr[dc.ToString()].ToString());
                }
            }


            pdfDoc.Add(table);

            #endregion


            #region footer section

            iTextSharp.text.Font footerFont = FontFactory.GetFont("Arial", 15, BaseColor.BLACK);
            PdfPTable footertable = new PdfPTable(1);
            footertable.TotalWidth = 2000;

            cell = new PdfPCell(new Phrase("", footerFont));
            cell.Border = 0;
            footertable.AddCell(cell);


            cell = new PdfPCell(new Phrase("This is computer generated report no signature required.", footerFont));
            cell.Border = 0;
            footertable.AddCell(cell);

            cell = new PdfPCell(new Phrase("", footerFont));
            cell.Border = 0;
            footertable.AddCell(cell);

            pdfDoc.Add(footertable);

            #endregion


            pdfDoc.Close();
            fileStream.Close();
        }
    }
}
