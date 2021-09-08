using MRLWMSC21Common;
using MRLWMSC21.mOutbound;
using MRLWMSC21.mOutbounds;
using MRLWMSC21.TPL;
//using FalconSFALibrary.BusinessObject;
//using FalconSFALibrary.Util;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MRLWMSC21.mReports.BillingReport;
using static MRLWMSC21.mReports.ReceiptConfirmationReport;
using static MRLWMSC21.mReports.WHOccupancyReport;
using static MRLWMSC21.mReports.WHStockInfoReport;

namespace MRLWMSC21.mOutbound
{

    public enum UnloadTemplate
    {

        PACKLIST_SHEET,
        BAR_CODE,
        WHOCC_SHEET, RCR_SHEET, Billing_SHEET,WHStock_SHEET //========================== Added New Method for Generate Warehouse Occupancy Report By M.D.Prasad ON 13-Nov-2019 ======================//

    }
 

    public class UnloadPDFGenerator
    {

        private int height { get; set; }
        private Document document { get; set; }
        private Document document1 { get; set; }
        private MemoryStream memoryStream { get; set; }
        private PdfWriter pdfWriter { get; set; }
        private FileStream fileStream { get; set; }
        private PdfContentByte pdfByteContent { get; set; }
        private BaseFont addressFont { get; set; }
        private BaseFont headerFont { get; set; }

        private Boolean IsKiwiInvoiceTemplate { get; set; }
        private Boolean IsMPSND { get; set; }
        private NewWarehouse.Barcode _barCode;
        private int pageNo { get; set; }
        private iTextSharp.text.Image addLogo { get; set; }
        private iTextSharp.text.Image addLogo1 { get; set; }
        private string footerInfo { get; set; }
        private string pdfFile;
        private string fileLocation = "";
        private string imageFile = "~/Images/inventrax.jpg";
        private string imageFile1 = "~/Images/logo-marker.png";
        private string subDirectory = "";
        // private List<Unload> unloads;
        public List<MRLWMSC21Common.PrintCommon.BoxItemDetails1> _picklist;
        public List<MRLWMSC21Common.PrintCommon.PackingSlip1> _pickHeader;

        private List<WHFirstTable> _fTable;
        private List<WHSecondTable> _sTable;

        private List<WHStockHTable> _whshTable;
        private List<WHStockMTable> _whsmTable;

        private List<RCRHeader> _hTable;
        private List<RCRData> _cData;

        private List<BillingInboundUnload> _inbUnload;
        private List<BillingInboundReceiving> _inbReceive;
        private List<BillingOutboundPicking> _outPicking;
        private List<BillingOutboundLoading> _outLoading;
        private List<StorageBilingData> _storageData;
        private List<BillingTo> _billingToData;



        public UnloadPDFGenerator(List<MRLWMSC21Common.PrintCommon.BoxItemDetails1> picklist, List<MRLWMSC21Common.PrintCommon.PackingSlip1> pickHeader)
        {
            _picklist = picklist;
            _pickHeader = pickHeader;
        }
        public UnloadPDFGenerator(NewWarehouse.Barcode barCode)
        {
            _barCode = barCode;
        }
        private bool CreateDirectory(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            return true;
        }

        private void PBPLPickListTemplate()
        {
            fileLocation = MRLWMSC21Common.CommonLogic.SafeMapPath("~/mOutbound/PackingSlip/");
            imageFile = MRLWMSC21Common.CommonLogic.SafeMapPath("~/Images/inventrax.jpg");
            imageFile1 = MRLWMSC21Common.CommonLogic.SafeMapPath("~/Images/logo-marker.png");
        }


        private void PBPLWHOCCTemplate(string imageLogo)
        {
            string img = "";
            if (imageLogo == "" || imageLogo == null)
            {
                img = "inventrax.jpg";
            }
            else
            {
                img = imageLogo;
            }
            fileLocation = MRLWMSC21Common.CommonLogic.SafeMapPath("~/mOutbound/PackingSlip/");
            //imageFile = MRLWMSC21Common.CommonLogic.SafeMapPath("~/Images/" + img);
            imageFile = MRLWMSC21Common.CommonLogic.SafeMapPath("~/Images/inventrax.jpg");
            imageFile1 = MRLWMSC21Common.CommonLogic.SafeMapPath("~/Images/logo-marker.png");
        }
        public UnloadPDFGenerator(List<WHFirstTable> fTable, List<WHSecondTable> sTable)
        {
            _fTable = fTable;
            _sTable = sTable;
        }

        public UnloadPDFGenerator(List<WHStockHTable> whshTable, List<WHStockMTable> whsmTable)
        {
            _whshTable = whshTable;
            _whsmTable = whsmTable;
        }


        public UnloadPDFGenerator(List<RCRHeader> hTable, List<RCRData> cData)
        {
            _hTable = hTable;
            _cData = cData;
        }

        public UnloadPDFGenerator(List<BillingInboundUnload> inbUnload, List<BillingInboundReceiving> inbReceive, List<BillingOutboundPicking> outPicking, List<BillingOutboundLoading> outLoading, List<StorageBilingData> storageData, List<BillingTo> billingToData)
        {
            _inbUnload = inbUnload;
            _inbReceive = inbReceive;
            _outPicking = outPicking;
            _outLoading = outLoading;
            _storageData = storageData;
            _billingToData = billingToData;
        }

        public string GenerateWHOCCPDFData(UnloadTemplate TEMPLATE, string imageLogo, string WHID)
        {
            switch (TEMPLATE.ToString())
            {

                case "PACKLIST_SHEET":
                    return GeneratePickListLikeKiwiPDF();
                case "WHOCC_SHEET":
                    return GenerateWHOCCPDF(imageLogo, WHID);
            }
            return null;
        }

        public string GenerateRCRPDFData(UnloadTemplate TEMPLATE, string imageLogo, string TenantID)
        {
            switch (TEMPLATE.ToString())
            {

                case "RCR_SHEET":
                    return GenerateRCRPDF(imageLogo, TenantID);
            }
            return null;
        }

        public string GenerateWHStockPDFData(UnloadTemplate TEMPLATE, string imageLogo, string TenantID,string WHID)
        {
            switch (TEMPLATE.ToString())
            {
                case "WHStock_SHEET":
                    return GenerateWHStockPDF(imageLogo, TenantID,WHID);
            }
            return null;
        }

        public string GenerateBillingPDFData(UnloadTemplate TEMPLATE, string imageLogo)
        {
            switch (TEMPLATE.ToString())
            {

                case "Billing_SHEET":
                    return GenerateBillingPDF(imageLogo);
            }
            return null;
        }

        public string GeneratePDF(UnloadTemplate TEMPLATE)
        {
            switch (TEMPLATE.ToString())
            {



                case "PACKLIST_SHEET":
                    return GeneratePickListLikeKiwiPDF();

                   // for BarCorde generation
                    case "BAR_CODE":
                    return GenerateBarCode();



            }
            return null;
        }

     
        //PickLIST PDF Generation

        public string GeneratePickListLikeKiwiPDF()
        {
            try
            {
                //Route code wise directory then files to place in that directory..
                PBPLPickListTemplate();
                CreateDirectory(fileLocation);
                fileLocation += subDirectory;
                CreateDirectory(fileLocation);

                string _PickListID = "PSNInfo";

                string _FileName = _PickListID != null ? _PickListID + "(" + DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss").Replace(" ", "").Replace(":", "") + ")" + ".pdf" : DateTime.Now.ToString("yyyyMMdd HH:mm:ss").Replace(" ", "").Replace(":", "") + ".pdf";

                //string _FileName = "PackSlipMaterialInfo.pdf";
                pageNo = 0;
                height = Dimension.YEND;
                height -= Dimension.LINE_GAP;

                InitializeKiwiPDFObject(_FileName);
                //LogoAdd(imageFile);
                //FalconLogoAdd(imageFile1);
                //document.Add(addLogo);
                //document.Add(addLogo1);

                WritePickListLikeKiwiTemplate(_pickHeader, _picklist);

                document.Close();
                fileStream.Close();
                return subDirectory + _FileName;
            }

            catch (Exception e)
            {
                if (document != null)
                {
                    if (pdfByteContent != null)
                        pdfByteContent.EndText();
                    document.Close();
                }

                if (fileStream != null) { fileStream.Close(); }
                throw new Exception("PickList Generation problem:" + e.StackTrace);
            }
        }

        //========================== Added New Method for Generate Warehouse Occupancy Report By M.D.Prasad ON 13-Nov-2019 ======================//
        public string GenerateWHOCCPDF(string imageLogo, string WHID)
        {
            try
            {
                //Route code wise directory then files to place in that directory..
                PBPLWHOCCTemplate(imageLogo);
                CreateDirectory(fileLocation);
                fileLocation += subDirectory;
                CreateDirectory(fileLocation);

                string _PickListID = "WHOCCReport";

                string _FileName = _PickListID != null ? _PickListID + "(" + DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss").Replace(" ", "").Replace(":", "") + ")" + ".pdf" : DateTime.Now.ToString("yyyyMMdd HH:mm:ss").Replace(" ", "").Replace(":", "") + ".pdf";

                //string _FileName = "PackSlipMaterialInfo.pdf";
                pageNo = 0;
                height = Dimension.YEND;
                height -= Dimension.LINE_GAP;

                InitializeKiwiPDFObject(_FileName);
                //LogoAdd(imageFile);
                //FalconLogoAdd(imageFile1);
                //document.Add(addLogo);
                //document.Add(addLogo1);

                WriteWHOCCTemplate(_fTable, _sTable, WHID);

                document.Close();
                fileStream.Close();
                return subDirectory + _FileName;
            }

            catch (Exception e)
            {
                if (document != null)
                {
                    if (pdfByteContent != null)
                        pdfByteContent.EndText();
                    document.Close();
                }

                if (fileStream != null) { fileStream.Close(); }
                throw new Exception("PickList Generation problem:" + e.StackTrace);
            }
        }



        private void WriteWHOCCTemplate(List<WHFirstTable> _lPlist, List<WHSecondTable> _lPlistHeader, string WHID)
        {
            if (_lPlist != null && _lPlist.Count > 0)
            {
                int NO_PAGES_REQUIRED = _lPlist.Count;
                int picklistCnt = 0;
                int index = 0;
                // document.SetPageSize(PageSize.A4.Rotate());


                if (_lPlist != null && _lPlist.Count > 0)
                {
                    //WriteHeaderForKiwi_PickList(_lPlist);
                    WriteHeaderForKiwi_PickList(WHID);

                    PdfPTable picklistTable = null;
                    float[] widths = null;


                    picklistTable = new PdfPTable(4);
                    //widths = new float[] { 1.25f, 3f, 3f, 6f, 8f, 3f, 3f, 5f };
                    widths = new float[] { 1.25f, 6f, 3f, 5f };
                    picklistTable.TotalWidth = 600f;
                    picklistTable.HorizontalAlignment = Element.ALIGN_CENTER;
                    picklistTable.SpacingBefore = 10f;
                    picklistTable.LockedWidth = true;


                    picklistTable.SetWidths(widths);

                    Font headerFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                    Font normalFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                    Font fontHighlight = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);



                    //string _PickRefNo = _picklist1[0] != null && _picklist1[0].PickListHeaderID != null ? _picklist1[0].PickListHeaderID : "";

                    picklistTable.AddCell(new PdfPCell(new Phrase("Sr.No", headerFont)) { Colspan = 1, Rowspan = 1 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Company", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    //picklistTable.AddCell(new PdfPCell(new Phrase("Commodity", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    //picklistTable.AddCell(new PdfPCell(new Phrase("Available Qty.", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Total Volume (CBM)", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Occupied Percentage", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

                    Font dataFont = normalFont;
                    foreach (WHFirstTable piclistItem in _lPlist)
                    {
                        if (piclistItem.TenantName == "TOTAL")
                        {
                            picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, MinimumHeight = 20 });
                        }
                        else
                        {
                            picklistTable.AddCell(new PdfPCell(new Phrase((++index).ToString(), dataFont)) { Colspan = 1, Rowspan = 1, MinimumHeight = 20 });
                        }                        
                        picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.TenantName, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        // picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.AvailableQty, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        //picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.Commodity, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.TotalVolume, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_RIGHT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.Occupancy, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_RIGHT, MinimumHeight = 20 });
                        //picklistTable.AddCell(new PdfPCell(new Phrase(" ", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    }
                    document.Add(picklistTable);
                    WriteHeaderForKiwi_PickListFirst(_lPlistHeader);
                    if (++picklistCnt < NO_PAGES_REQUIRED)
                        NewKiwiA4Required(height, true);

                }



            }

        }


        private void WriteHeaderForKiwi_PickList(string wareHouseID)
        {
            try
            {
                PdfPTable PicklistHeaderTable = new PdfPTable(4);
                PicklistHeaderTable.HorizontalAlignment = Element.ALIGN_CENTER;
                PicklistHeaderTable.TotalWidth = 600f;
                PicklistHeaderTable.SpacingBefore = 0f;
                PicklistHeaderTable.LockedWidth = true;
                //PicklistHeaderTable.SetWidths(new float[] { 0.75f, 2f, 0.5f, 0.75f, 1.5f });
                PicklistHeaderTable.SetWidths(new float[] { 0.75f, 2f, 0.5f, 0.75f });

                Font HeaderFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1257, BaseFont.NOT_EMBEDDED), 16f);

                Font headerFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                Font headerFont1 = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 10f);
                Font dataFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                Font tinyDataFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                Font fontHighlight = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);

                LogoAdd1(imageFile);
                //FalconLogoAdd(imageFile1);


                PdfPCell cell = new PdfPCell(new Paragraph("", dataFont));
                cell.Colspan = 2;
                cell.Border = 0;

                cell.AddElement(addLogo);

                PdfPCell cell1 = new PdfPCell(new Paragraph("", dataFont));
                cell1.Colspan = 3;
                cell1.Border = 0;

                cell1.AddElement(addLogo1);

                PicklistHeaderTable.AddCell(cell);
                PicklistHeaderTable.AddCell(cell1);


                Phrase p1 = new Phrase();
                string Data = "";

                Data += "Transcrate International Logistics";
                if (wareHouseID == "1")
                {
                    Data += "\n" + "3PL Warehouse-01, Amghara";
                }
                else if (wareHouseID == "4027")
                {
                    Data += "\n" + "3PL Warehouse-01, Sharq";
                }
                else
                {
                    Data += "\n" + "3PL Warehouse-02, Amghara";
                }

                PicklistHeaderTable.AddCell(new PdfPCell(new Phrase(Dimension.Kiwi_WHOCCData["HEADDING"], HeaderFont)) { Colspan = 5, Rowspan = 1, Border = 0, HorizontalAlignment = PdfPCell.ALIGN_CENTER, MinimumHeight = 30 });
                PicklistHeaderTable.AddCell(new PdfPCell(new Phrase(Data, headerFont1)) { Colspan = 5, Rowspan = 1, Border = 0, HorizontalAlignment = PdfPCell.ALIGN_CENTER, MinimumHeight = 30 });
                document.Add(PicklistHeaderTable);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void WriteHeaderForKiwi_PickListFirst(List<WHSecondTable> _firstTable)
        {
            try
            {
                if (_firstTable != null && _firstTable.Count > 0)
                {
                    //int NO_PAGES_REQUIRED = _firstTable.Count;
                    //int picklistCnt = 0;
                    int index = 0;
                    PdfPTable picklistTable = null;
                    float[] widths = null;


                    picklistTable = new PdfPTable(3);
                    //widths = new float[] { 1.25f, 3f, 3f, 6f, 8f, 3f, 3f, 5f };
                    widths = new float[] {3f, 3f, 6f };
                    picklistTable.TotalWidth = 600f;
                    picklistTable.HorizontalAlignment = Element.ALIGN_CENTER;
                    picklistTable.SpacingBefore = 10f;
                    picklistTable.LockedWidth = true;


                    picklistTable.SetWidths(widths);

                    Font headerFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                    Font normalFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                    Font fontHighlight = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);

                    //picklistTable.AddCell(new PdfPCell(new Phrase("Sr.No", headerFont)) { Colspan = 1, Rowspan = 1 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Total Warehouse Volume (CBM)", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Total Occupied Volume (CBM)", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Total Available Volume (CBM)", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

                    Font dataFont = normalFont;
                    foreach (WHSecondTable piclistItem in _firstTable)
                    {
                        //picklistTable.AddCell(new PdfPCell(new Phrase((++index).ToString(), dataFont)) { Colspan = 1, Rowspan = 1, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.WarehouseVolume, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.OccupiedVolume, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.AvailableVolume, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER, MinimumHeight = 20 });
                    }

                    document.Add(picklistTable);

                    //if (++picklistCnt < NO_PAGES_REQUIRED)
                    //    NewKiwiA4Required(height, true);
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //========================== Added New Method for Generate Warehouse Occupancy Report By M.D.Prasad ON 13-Nov-2019 END ======================//


        //===================== Added New PDF For WH Stock Information Report By M.D.Prasad ====================//
        public string GenerateWHStockPDF(string imageLogo, string TenantID,string WHID)
        {
            try
            {
                //Route code wise directory then files to place in that directory..
                PBPLWHOCCTemplate(imageLogo);
                CreateDirectory(fileLocation);
                fileLocation += subDirectory;
                CreateDirectory(fileLocation);

                string _PickListID = "WHStockInfoReport";

                string _FileName = _PickListID != null ? _PickListID + "(" + DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss").Replace(" ", "").Replace(":", "") + ")" + ".pdf" : DateTime.Now.ToString("yyyyMMdd HH:mm:ss").Replace(" ", "").Replace(":", "") + ".pdf";

                pageNo = 0;
                height = Dimension.YEND;
                height -= Dimension.LINE_GAP;

                InitializeKiwiPDFObject_RCR_Lanscape(_FileName);

                WriteWHStockTemplate(_whsmTable, _whshTable, imageLogo, TenantID, WHID);

                document.Close();
                fileStream.Close();
                return subDirectory + _FileName;
            }

            catch (Exception e)
            {
                if (document != null)
                {
                    if (pdfByteContent != null)
                        pdfByteContent.EndText();
                    document.Close();
                }

                if (fileStream != null) { fileStream.Close(); }
                throw new Exception("PickList Generation problem:" + e.StackTrace);
            }
        }

        private void WriteWHStockTemplate(List<WHStockMTable> _lPlist, List<WHStockHTable> _lPlistHeader,string Accountlogo, string TenantID, string WHID)
        {
            if (_lPlist != null && _lPlist.Count > 0)
            {
                int NO_PAGES_REQUIRED = _lPlist.Count;
                int picklistCnt = 0;
                int index = 0;
                if (_lPlist != null && _lPlist.Count > 0)
                {
                    WriteHeaderForWHStock(_lPlistHeader,Accountlogo, WHID);

                    PdfPTable picklistTable = null;
                    float[] widths = null;
                    if (TenantID == "58" || TenantID == "52" || TenantID == "80")
                    {
                        picklistTable = new PdfPTable(12);
                        widths = new float[] {2f, 5f, 5f, 5f, 5f, 3f, 5f, 5f, 4f, 4f, 4f, 4f };
                    }
                    else
                    {
                        picklistTable = new PdfPTable(13);
                        widths = new float[] {2f, 5f, 5f, 4f, 3f, 4f, 4f, 5f, 4f, 4f, 4f, 4f, 5f };
                    }
                    picklistTable.TotalWidth = 790f;
                    picklistTable.HorizontalAlignment = Element.ALIGN_CENTER;
                    picklistTable.SpacingBefore = 10f;
                    picklistTable.LockedWidth = true;
                    picklistTable.SetWidths(widths);

                    Font headerFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                    Font normalFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                    Font fontHighlight = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);

                    picklistTable.AddCell(new PdfPCell(new Phrase("Sr.No", headerFont)) { Colspan = 1, Rowspan = 1 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Item Code", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

                    if (TenantID == "52" || TenantID == "39")
                    {
                        picklistTable.AddCell(new PdfPCell(new Phrase("PO Number", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                        picklistTable.AddCell(new PdfPCell(new Phrase("Airway Bill No.", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                        picklistTable.AddCell(new PdfPCell(new Phrase("Serial No.", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    }
                    else if (TenantID == "58")
                    {
                        picklistTable.AddCell(new PdfPCell(new Phrase("Collection", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                        picklistTable.AddCell(new PdfPCell(new Phrase("Cus-Design & Color", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                        picklistTable.AddCell(new PdfPCell(new Phrase("Size", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });                      
                    }
                    else if (TenantID == "80")
                    {
                        picklistTable.AddCell(new PdfPCell(new Phrase("Brand", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                        picklistTable.AddCell(new PdfPCell(new Phrase("Description", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                        picklistTable.AddCell(new PdfPCell(new Phrase("Model", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    }
                    else
                    {
                        picklistTable.AddCell(new PdfPCell(new Phrase("Description", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    }

                    picklistTable.AddCell(new PdfPCell(new Phrase("Category", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("UoM/Qty.", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

                    if (TenantID != "58" && TenantID != "52" && TenantID != "39" && TenantID != "80")
                    {
                        picklistTable.AddCell(new PdfPCell(new Phrase("Mfg. Date", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                        picklistTable.AddCell(new PdfPCell(new Phrase("Exp. Date", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                        picklistTable.AddCell(new PdfPCell(new Phrase("Batch No.", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    }

                    picklistTable.AddCell(new PdfPCell(new Phrase("Onhand Qty.", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Allocated Qty.", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Good Qty.", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

                    picklistTable.AddCell(new PdfPCell(new Phrase("Damaged Qty.", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Volume(CBM)", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

                    Font dataFont = normalFont;
                    foreach (WHStockMTable piclistItem in _lPlist)
                    {
                        if (piclistItem.ItemCode == "Total")
                        {
                            picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, MinimumHeight = 20 });
                        }
                        else {
                            picklistTable.AddCell(new PdfPCell(new Phrase((++index).ToString(), dataFont)) { Colspan = 1, Rowspan = 1, MinimumHeight = 20 });
                        }
                        picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.ItemCode, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });

                        if (TenantID == "58" || TenantID == "52" || TenantID == "39" || TenantID == "80")
                        {
                            picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.MCodeAlternative2, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                            picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.Description, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                            picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.MCodeAlternative1, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });                            
                        }
                        else
                        {
                            picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.Description, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        }
                        picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.Category, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.UoMQty, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });

                        if (TenantID != "58" && TenantID != "52" && TenantID != "39" && TenantID != "80")
                        {
                            picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.MfgDate, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                            picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.ExpDate, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                            picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.BatchNo, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        }

                        picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.OnhandQty, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.AllocatedQty, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.GoodQty, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });

                        picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.DamagedQty, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        //picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.ExcessQty, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        //picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.ExcessOrShortQty, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.Volume, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    }

                    document.Add(picklistTable);
                    if (++picklistCnt < NO_PAGES_REQUIRED)
                        NewKiwiA4Required(height, true);

                }
            }
        }

        private void WriteHeaderForWHStock(List<WHStockHTable> _lPlistHeader,string Accountlogo, string WHID)
        {
            try
            {
                PdfPTable PicklistHeaderTable = new PdfPTable(6);
                PicklistHeaderTable.HorizontalAlignment = Element.ALIGN_CENTER;
                PicklistHeaderTable.TotalWidth = 790f;
                PicklistHeaderTable.SpacingBefore = 0f;
                PicklistHeaderTable.LockedWidth = true;
                PicklistHeaderTable.SetWidths(new float[] { 2.5f, 5f, 4f, 4f, 4f, 4f });

                Font HeaderFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1257, BaseFont.NOT_EMBEDDED), 16f);

                Font headerFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                Font headerFont1 = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED),9.5f);
                Font headerFont2 = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 10f);
                Font dataFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                Font tinyDataFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                Font fontHighlight = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                string AccountFile = MRLWMSC21Common.CommonLogic.SafeMapPath("~/TPL/AccountLogos/" + Accountlogo);
                LogoAdd1(AccountFile);
              //  LogoAdd1(imageFile);

                PdfPCell cell = new PdfPCell(new Paragraph("", dataFont));
                cell.Colspan = 3;
                cell.Border = 0;

                cell.AddElement(addLogo);

                PdfPCell cell1 = new PdfPCell(new Paragraph("", dataFont));
                cell1.Colspan = 3;
                cell1.Border = 0;

                cell1.AddElement(addLogo1);

                PicklistHeaderTable.AddCell(cell);
                PicklistHeaderTable.AddCell(cell1);

                string Data = "";
                /*if (WHID == "1")
                {
                    Data += "3PL Warehouse-01, Amghara";
                }
                else if (WHID == "4027")
                {
                    Data += "3PL Warehouse-01, Sharq";
                }
                else
                {
                    Data += "3PL Warehouse-02, Amghara";
                }*/

                Data += "3PL Warehouse";

                PicklistHeaderTable.AddCell(new PdfPCell(new Phrase(Dimension.WHStockHeaderFormatData["HEADDING"], HeaderFont)) { Colspan = 6, Rowspan = 1, Border = 0, HorizontalAlignment = PdfPCell.ALIGN_CENTER, MinimumHeight = 30 });
                PicklistHeaderTable.AddCell(new PdfPCell(new Phrase(Data, headerFont2)) { Colspan = 6, Rowspan = 1, Border = 0, HorizontalAlignment = PdfPCell.ALIGN_CENTER, MinimumHeight = 30 });

                foreach (WHStockHTable _pHeader in _lPlistHeader)
                {
                    string _TenantName = _pHeader != null && _pHeader.TenantName != null ? _pHeader.TenantName : "";
                    string _Address = _pHeader != null && _pHeader.Address != null ? _pHeader.Address : "";
                    string _ContactPerson = _pHeader != null && _pHeader.ContactPerson != null ? _pHeader.ContactPerson : "";
                    string _EmailID = _pHeader != null && _pHeader.EmailID != null ? _pHeader.EmailID : "";
                    string _DocDate = _pHeader != null && _pHeader.DocDate != null ? _pHeader.DocDate : "";

                    //Information Getting form DB

                    PicklistHeaderTable.AddCell(GetCellWithAlign(Dimension.WHStockHeaderFormatData["TenantName"], headerFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER));
                    PicklistHeaderTable.AddCell(GetCellWithAlign(": " + _TenantName, dataFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER, 6));

                    PicklistHeaderTable.AddCell(GetCellWithAlign(Dimension.WHStockHeaderFormatData["Address1"], headerFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER));
                    PicklistHeaderTable.AddCell(GetCellWithAlign(": " + _Address, dataFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER, 6));

                    PicklistHeaderTable.AddCell(GetCellWithAlign(Dimension.WHStockHeaderFormatData["ContactPerson"], headerFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER));
                    PicklistHeaderTable.AddCell(GetCellWithAlign(": " + _ContactPerson, dataFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER, 6));

                    PicklistHeaderTable.AddCell(GetCellWithAlign(Dimension.WHStockHeaderFormatData["EmailID"], headerFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER));
                    PicklistHeaderTable.AddCell(GetCellWithAlign(": " + _EmailID, dataFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER, 6));

                    PicklistHeaderTable.AddCell(GetCellWithAlign(Dimension.WHStockHeaderFormatData["DocDate"], headerFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER));
                    PicklistHeaderTable.AddCell(GetCellWithAlign(": " + _DocDate, dataFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER, 6));
                    document.Add(PicklistHeaderTable);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //======================================= END ========================================//
        //========================= RCR ====================//

        public string GenerateRCRPDF(string imageLogo, string TenantID)
        {
            try
            {
                //Route code wise directory then files to place in that directory..
                PBPLWHOCCTemplate(imageLogo);
                CreateDirectory(fileLocation);
                fileLocation += subDirectory;
                CreateDirectory(fileLocation);

                string _RCR = "ReceiptConfirmationReport";

                string _FileName = _RCR != null ? _RCR + "(" + DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss").Replace(" ", "").Replace(":", "") + ")" + ".pdf" : DateTime.Now.ToString("yyyyMMdd HH:mm:ss").Replace(" ", "").Replace(":", "") + ".pdf";

                pageNo = 0;
                height = Dimension.YEND;
                height -= Dimension.LINE_GAP;

                InitializeKiwiPDFObject_RCR_Lanscape(_FileName);
                WriteRCRTemplate(_cData, imageLogo, _hTable, TenantID);

                document.Close();
                fileStream.Close();
                return subDirectory + _FileName;
            }

            catch (Exception e)
            {
                if (document != null)
                {
                    if (pdfByteContent != null)
                        pdfByteContent.EndText();
                    document.Close();
                }

                if (fileStream != null) { fileStream.Close(); }
                throw new Exception("PickList Generation problem:" + e.StackTrace);
            }
        }
          
        private void WriteRCRTemplate(List<RCRData> _lPlist,string AccountLogo, List<RCRHeader> _lPlistHeader, string TenantID)
        {
            if (_lPlist != null && _lPlist.Count > 0)
            {
                int NO_PAGES_REQUIRED = _lPlist.Count;
                int picklistCnt = 0;
                int index = 0;
                if (_lPlist != null && _lPlist.Count > 0)
                {
                    WriteHeaderForRCR(_lPlistHeader, AccountLogo);

                    PdfPTable picklistTable = null;
                    float[] widths = null;
                    if (TenantID == "58" || TenantID == "52")
                    {
                        picklistTable = new PdfPTable(12);
                        widths = new float[] { 6f, 5f, 5f, 5f, 3f, 5f, 5f, 4f, 4f, 4f, 4f, 5f };
                    }
                    else {
                        picklistTable = new PdfPTable(12);
                        widths = new float[] { 6f, 5f, 3f, 5f, 5f, 3f, 5f, 4f, 4f, 4f, 4f, 5f };
                    }
                    picklistTable.TotalWidth = 790f;
                    picklistTable.HorizontalAlignment = Element.ALIGN_CENTER;
                    picklistTable.SpacingBefore = 10f;
                    picklistTable.LockedWidth = true;
                    picklistTable.SetWidths(widths);

                    Font headerFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                    Font normalFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                    Font fontHighlight = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);

                    //picklistTable.AddCell(new PdfPCell(new Phrase("Sr.No", headerFont)) { Colspan = 1, Rowspan = 1 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Item Code", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

                    if (TenantID == "52")
                    {
                        picklistTable.AddCell(new PdfPCell(new Phrase("Serial No.", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                        picklistTable.AddCell(new PdfPCell(new Phrase("PO Number", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                        picklistTable.AddCell(new PdfPCell(new Phrase("Airway Bill No.", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    }
                    else if (TenantID == "58")
                    {
                        picklistTable.AddCell(new PdfPCell(new Phrase("Size", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                        picklistTable.AddCell(new PdfPCell(new Phrase("Collection", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                        picklistTable.AddCell(new PdfPCell(new Phrase("Description", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    }
                    else {
                        picklistTable.AddCell(new PdfPCell(new Phrase("Description", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    }

                    picklistTable.AddCell(new PdfPCell(new Phrase("UoM", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

                    if (TenantID != "58" && TenantID != "52")
                    {
                        picklistTable.AddCell(new PdfPCell(new Phrase("Mfg. Date", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                        picklistTable.AddCell(new PdfPCell(new Phrase("Exp. Date", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    }

                    picklistTable.AddCell(new PdfPCell(new Phrase("Expected Qty.", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Received Qty.", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Good Qty.", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

                    picklistTable.AddCell(new PdfPCell(new Phrase("Damaged Qty.", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Excess Qty.", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Short Qty.", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Volume(CBM)", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

                    Font dataFont = normalFont;
                    foreach (RCRData piclistItem in _lPlist)
                    {
                        //picklistTable.AddCell(new PdfPCell(new Phrase((++index).ToString(), dataFont)) { Colspan = 1, Rowspan = 1, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.MCode, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });

                        if (TenantID == "58" || TenantID == "52")
                        {
                            picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.MCodeAlternative1, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                            picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.MCodeAlternative2, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                            picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.MDescription, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        }
                        else
                        {
                            picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.MDescription, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        }

                        picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.BUoM, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });

                        if (TenantID != "58" && TenantID != "52")
                        {
                            picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.MfgDate, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                            picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.ExpDate, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        }

                        picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.InvoiceQuantity, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.ReceivedQty, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.GoodQty, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });

                        picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.DamagedQty, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.ExcessQty, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.ExcessOrShortQty, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.MVolume, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    }

                    document.Add(picklistTable);

                    WriteFoterForRCR();

                    if (++picklistCnt < NO_PAGES_REQUIRED)
                        NewKiwiA4Required(height, true);

                }
            }
        }


        private void WriteHeaderForRCR(List<RCRHeader> _lPlistHeader,string AccountLogo)
        {
            try
            {
                PdfPTable PicklistHeaderTable = new PdfPTable(6);
                PicklistHeaderTable.HorizontalAlignment = Element.ALIGN_CENTER;
                PicklistHeaderTable.TotalWidth = 790f;
                PicklistHeaderTable.SpacingBefore = 0f;
                PicklistHeaderTable.LockedWidth = true;
                PicklistHeaderTable.SetWidths(new float[] { 1.5f, 4f, 0.5f, 2f, 2f, 2f });

                Font HeaderFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1257, BaseFont.NOT_EMBEDDED), 16f);

                Font headerFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                Font dataFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                Font tinyDataFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                Font fontHighlight = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);

            //    string file = DB.GetSqlS("SELECT LogoPath AS S FROM GEN_Account WHERE AccountID=" + );
                string AccountFile = MRLWMSC21Common.CommonLogic.SafeMapPath("~/TPL/AccountLogos/" + AccountLogo);
                LogoAdd1(AccountFile);

                PdfPCell cell = new PdfPCell(new Paragraph("", dataFont));
                cell.Colspan = 3;
                cell.Border = 0;

                cell.AddElement(addLogo);

                PdfPCell cell1 = new PdfPCell(new Paragraph("", dataFont));
                cell1.Colspan = 3;
                cell1.Border = 0;

                cell1.AddElement(addLogo1);

                PicklistHeaderTable.AddCell(cell);
                PicklistHeaderTable.AddCell(cell1);


                PicklistHeaderTable.AddCell(new PdfPCell(new Phrase(Dimension.RCRHeaderFormatData["HEADDING"], HeaderFont)) { Colspan = 6, Rowspan = 1, Border = 0, HorizontalAlignment = PdfPCell.ALIGN_CENTER, MinimumHeight = 30 });
                foreach (RCRHeader _pHeader in _lPlistHeader)
                {
                    string _TenantName = _pHeader != null && _pHeader.TenantName != null ? _pHeader.TenantName : "";
                    string _Address1 = _pHeader != null && _pHeader.Address1 != null ? _pHeader.Address1 : "";
                    string _ShipmentType = _pHeader != null && _pHeader.ShipmentType != null ? _pHeader.ShipmentType : "";
                    string _VehicleRegistrationNo = _pHeader != null && _pHeader.VehicleRegistrationNo != null ? _pHeader.VehicleRegistrationNo : "";
                    string _StoreRefNo = _pHeader != null && _pHeader.StoreRefNo != null ? _pHeader.StoreRefNo : "";
                    string _ShipmentReceivedOn = _pHeader != null && _pHeader.ShipmentReceivedOn != null ? _pHeader.ShipmentReceivedOn : "";

                    //Information Getting form DB

                    PicklistHeaderTable.AddCell(GetCellWithAlign(Dimension.RCRHeaderFormatData["StoreRefNo"], headerFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER));
                    PicklistHeaderTable.AddCell(GetCellWithAlign(": " + _StoreRefNo, dataFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER, 2));

                    PicklistHeaderTable.AddCell(GetCellWithAlign(Dimension.RCRHeaderFormatData["ShipmentReceivedOn"], headerFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER));
                    PicklistHeaderTable.AddCell(GetCellWithAlign(": " + _ShipmentReceivedOn, dataFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER, 2));

                    PicklistHeaderTable.AddCell(GetCellWithAlign(Dimension.RCRHeaderFormatData["TenantName"], headerFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER));
                    PicklistHeaderTable.AddCell(GetCellWithAlign(": " + _TenantName, dataFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER, 2));

                    PicklistHeaderTable.AddCell(GetCellWithAlign(Dimension.RCRHeaderFormatData["Address1"], headerFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER));
                    PicklistHeaderTable.AddCell(GetCellWithAlign(": " + _Address1, dataFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER, 2));

                    PicklistHeaderTable.AddCell(GetCellWithAlign(Dimension.RCRHeaderFormatData["ShipmentType"], headerFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER));
                    PicklistHeaderTable.AddCell(GetCellWithAlign(": " + _ShipmentType, dataFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER, 2));

                    PicklistHeaderTable.AddCell(GetCellWithAlign(Dimension.RCRHeaderFormatData["VehicleRegistrationNo"], headerFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER));
                    PicklistHeaderTable.AddCell(GetCellWithAlign(": " + _VehicleRegistrationNo, dataFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER, 2));

                    document.Add(PicklistHeaderTable);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void WriteFoterForRCR()
        {
            try
            {
                PdfPTable PicklistHeaderTable = new PdfPTable(6);
                PicklistHeaderTable.HorizontalAlignment = Element.ALIGN_CENTER;
                PicklistHeaderTable.TotalWidth = 790f;
                PicklistHeaderTable.SpacingBefore = 0f;
                PicklistHeaderTable.LockedWidth = true;
                PicklistHeaderTable.SetWidths(new float[] { 5f, 2f, 5f, 2f, 5f, 2f });

                Font HeaderFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1257, BaseFont.NOT_EMBEDDED), 16f);

                Font headerFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                Font dataFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                Font tinyDataFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                Font fontHighlight = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);

                LogoAdd1(imageFile);

                PdfPCell cell = new PdfPCell(new Paragraph("", dataFont));
                cell.Colspan = 3;
                cell.Border = 0;

                PdfPCell cell1 = new PdfPCell(new Paragraph("", dataFont));
                cell1.Colspan = 3;
                cell1.Border = 0;

                PicklistHeaderTable.AddCell(new PdfPCell(new Phrase("", HeaderFont)) { Colspan = 6, Rowspan = 1, Border = 0, HorizontalAlignment = PdfPCell.ALIGN_CENTER, MinimumHeight = 30 });

                PicklistHeaderTable.AddCell(GetCellWithAlign("DATE", headerFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER));
                PicklistHeaderTable.AddCell(GetCellWithAlign(" ", dataFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER, 1));

                PicklistHeaderTable.AddCell(GetCellWithAlign("", headerFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER));
                PicklistHeaderTable.AddCell(GetCellWithAlign(" ", dataFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER, 1));

                PicklistHeaderTable.AddCell(GetCellWithAlign("", headerFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER));
                PicklistHeaderTable.AddCell(GetCellWithAlign(" ", dataFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER, 1));

                PicklistHeaderTable.AddCell(new PdfPCell(new Phrase("", HeaderFont)) { Colspan = 6, Rowspan = 1, Border = 0, HorizontalAlignment = PdfPCell.ALIGN_CENTER, MinimumHeight = 10 });

                PicklistHeaderTable.AddCell(GetCellWithAlign("SIGNATURE", headerFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER));
                PicklistHeaderTable.AddCell(GetCellWithAlign(" ", dataFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER, 1));

                PicklistHeaderTable.AddCell(GetCellWithAlign("SIGNATURE", headerFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER));
                PicklistHeaderTable.AddCell(GetCellWithAlign(" ", dataFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER, 1));

                PicklistHeaderTable.AddCell(GetCellWithAlign("SIGNATURE", headerFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER));
                PicklistHeaderTable.AddCell(GetCellWithAlign(" ", dataFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER, 1));

                PicklistHeaderTable.AddCell(new PdfPCell(new Phrase("", HeaderFont)) { Colspan = 6, Rowspan = 1, Border = 0, HorizontalAlignment = PdfPCell.ALIGN_CENTER, MinimumHeight = 10 });

                PicklistHeaderTable.AddCell(GetCellWithAlign("WAREHOUSE MANAGER", headerFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER));
                PicklistHeaderTable.AddCell(GetCellWithAlign(" ", dataFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER, 1));

                PicklistHeaderTable.AddCell(GetCellWithAlign("OPERATION SUPERVISOR", headerFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER));
                PicklistHeaderTable.AddCell(GetCellWithAlign(" ", dataFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER, 1));

                PicklistHeaderTable.AddCell(GetCellWithAlign("WMS SUPERVISOR", headerFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER));
                PicklistHeaderTable.AddCell(GetCellWithAlign(" ", dataFont, Element.ALIGN_LEFT, Rectangle.NO_BORDER, 1));

                document.Add(PicklistHeaderTable);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //========================= END ====================//


        //======================= Billing Report ========================//

        public string GenerateBillingPDF(string imageLogo)
        {
            try
            {
                //Route code wise directory then files to place in that directory..
                PBPLWHOCCTemplate(imageLogo);
                CreateDirectory(fileLocation);
                fileLocation += subDirectory;
                CreateDirectory(fileLocation);

                string _PickListID = "BillingReport";

                string _FileName = _PickListID != null ? _PickListID + "(" + DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss").Replace(" ", "").Replace(":", "") + ")" + ".pdf" : DateTime.Now.ToString("yyyyMMdd HH:mm:ss").Replace(" ", "").Replace(":", "") + ".pdf";

                //string _FileName = "PackSlipMaterialInfo.pdf";
                pageNo = 0;
                height = Dimension.YEND;
                height -= Dimension.LINE_GAP;

                InitializeKiwiPDFObject(_FileName);
                //LogoAdd(imageFile);
                //FalconLogoAdd(imageFile1);
                //document.Add(addLogo);
                //document.Add(addLogo1);

                WriteBillingTemplate(_inbUnload, _inbReceive, _outPicking, _outLoading, _storageData, _billingToData);

                document.Close();
                fileStream.Close();
                return subDirectory + _FileName;
            }

            catch (Exception e)
            {
                if (document != null)
                {
                    if (pdfByteContent != null)
                        pdfByteContent.EndText();
                    document.Close();
                }

                if (fileStream != null) { fileStream.Close(); }
                throw new Exception("PickList Generation problem:" + e.StackTrace);
            }
        }

        decimal BillingUnloadTotal = 0;
        decimal BillingReceiveTotal = 0;
        decimal BillingPickingTotal = 0;
        decimal BillingLoadTotal = 0;
        decimal BillingStorageTotal = 0;

        private void WriteBillingTemplate(List<BillingInboundUnload> _inbUnloadList, List<BillingInboundReceiving> _inbReceiveList, List<BillingOutboundPicking> _outPickingList, List<BillingOutboundLoading> _outLoadingList, List<StorageBilingData> _storageBillingList, List<BillingTo> _billingToList)
        {
            if (_inbUnloadList != null && _inbUnloadList.Count > 0)
            {
                int NO_PAGES_REQUIRED = _inbUnloadList.Count;
                int picklistCnt = 0;
                int index = 0;
                if (_inbUnloadList != null && _inbUnloadList.Count > 0)
                {
                    WriteHeaderForBilling();

                    WriteBillingTo(_billingToList);

                    PdfPTable picklistTable = null;
                    float[] widths = null;


                    picklistTable = new PdfPTable(11);
                    widths = new float[] { 2f, 4f, 6f, 4f, 4f, 5f, 2f, 3f, 3f, 3f, 4f };
                    picklistTable.TotalWidth = 600f;
                    picklistTable.HorizontalAlignment = Element.ALIGN_CENTER;
                    picklistTable.SpacingBefore = 10f;
                    picklistTable.LockedWidth = true;

                    picklistTable.SetWidths(widths);

                    Font headerFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                    Font normalFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                    Font fontHighlight = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);

                    //PdfPCell cell = new PdfPCell(new Phrase("Inbound : Unloading & Handling Charges",headerFont)) { Colspan = 11, Rowspan = 1, Border = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER, MinimumHeight = 10 };
                    //cell.Colspan = 11;

                    //cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    picklistTable.AddCell(new PdfPCell(new Phrase("Inbound : Unloading & Handling Charges", headerFont)) { Colspan = 11, Rowspan = 1, Border = 0, HorizontalAlignment = PdfPCell.ALIGN_CENTER, MinimumHeight = 20 });

                    picklistTable.AddCell(new PdfPCell(new Phrase("Sr.No", headerFont)) { Colspan = 1, Rowspan = 1 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Store Ref. No.", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Supplier", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("PO Number", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Receipt Date", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

                    picklistTable.AddCell(new PdfPCell(new Phrase("Service/Material", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("UoM", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Quantity", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Unit Cost", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

                    picklistTable.AddCell(new PdfPCell(new Phrase("TAX", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Total Cost", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

                    Font dataFont = normalFont;
                    decimal inbUnLoad = 0;
                    foreach (BillingInboundUnload inbUnItem in _inbUnloadList)
                    {
                        picklistTable.AddCell(new PdfPCell(new Phrase((++index).ToString(), dataFont)) { Colspan = 1, Rowspan = 1, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(inbUnItem.StoreRefNo, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(inbUnItem.SupplierName, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(inbUnItem.PONumber, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(inbUnItem.Receipt, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });

                        picklistTable.AddCell(new PdfPCell(new Phrase(inbUnItem.ServiceMaterial, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(inbUnItem.UoM, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(inbUnItem.Quantity, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(inbUnItem.UnitCost, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });

                        picklistTable.AddCell(new PdfPCell(new Phrase(inbUnItem.Tax, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(inbUnItem.TotalCost, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        inbUnLoad = inbUnLoad + Convert.ToDecimal(inbUnItem.TotalCost.ToString());
                    }
                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });

                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });

                    picklistTable.AddCell(new PdfPCell(new Phrase("Total", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });

                    picklistTable.AddCell(new PdfPCell(new Phrase(inbUnLoad.ToString(), dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    BillingUnloadTotal = inbUnLoad;
                    document.Add(picklistTable);
                    WriteInboundReceive(_inbReceiveList);
                    WriteOutboundPicking(_outPickingList);
                    WriteOutboundLoading(_outLoadingList);
                    WriteStorageBillingData(_storageBillingList);
                    WriteConsolidatedData();
                    if (++picklistCnt < NO_PAGES_REQUIRED)
                        NewKiwiA4Required(height, true);
                }
            }
        }

        private void WriteHeaderForBilling()
        {
            try
            {
                PdfPTable PicklistHeaderTable = new PdfPTable(3);
                PicklistHeaderTable.HorizontalAlignment = Element.ALIGN_CENTER;
                PicklistHeaderTable.TotalWidth = 600f;
                PicklistHeaderTable.SpacingBefore = 0f;
                PicklistHeaderTable.LockedWidth = true;
                PicklistHeaderTable.SetWidths(new float[] { 0.75f, 2f, 0.5f });

                Font HeaderFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1257, BaseFont.NOT_EMBEDDED), 16f);

                Font headerFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                Font dataFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                Font tinyDataFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                Font fontHighlight = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);

                LogoAdd1(imageFile);

                PdfPCell cell = new PdfPCell(new Paragraph("", dataFont));
                cell.Colspan = 2;
                cell.Border = 0;

                cell.AddElement(addLogo);

                PdfPCell cell1 = new PdfPCell(new Paragraph("", dataFont));
                cell1.Colspan = 3;
                cell1.Border = 0;

                cell1.AddElement(addLogo1);

                PicklistHeaderTable.AddCell(cell);
                PicklistHeaderTable.AddCell(cell1);

                PicklistHeaderTable.AddCell(new PdfPCell(new Phrase(Dimension.Kiwi_BillingData["HEADDING"], HeaderFont)) { Colspan = 5, Rowspan = 1, Border = 0, HorizontalAlignment = PdfPCell.ALIGN_CENTER, MinimumHeight = 30 });
                document.Add(PicklistHeaderTable);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private void WriteBillingTo(List<BillingTo> _billingData)
        {
            try
            {
                if (_billingData != null && _billingData.Count > 0)
                {
                    //int NO_PAGES_REQUIRED = _firstTable.Count;
                    //int picklistCnt = 0;
                    int index = 0;
                    PdfPTable picklistTable = null;
                    float[] widths = null;


                    picklistTable = new PdfPTable(2);
                    widths = new float[] { 6f, 6f };
                    picklistTable.TotalWidth = 600f;
                    picklistTable.HorizontalAlignment = Element.ALIGN_CENTER;
                    picklistTable.SpacingBefore = 10f;
                    picklistTable.LockedWidth = true;


                    picklistTable.SetWidths(widths);

                    Font headerFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                    Font normalFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                    Font fontHighlight = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);


                    picklistTable.AddCell(new PdfPCell(new Phrase("Billing To : ", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 15 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Billing From : ", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 15 });

                    Phrase p1 = new Phrase();
                    string Data = "";
                    foreach (BillingTo billingItem in _billingData)
                    {
                        Data += billingItem.TenantName;
                        Data += "\n" + billingItem.Address1;
                        Data += "\n" + billingItem.Address2;
                        Data += "\n" + billingItem.City;
                        Data += "\n" + billingItem.State;
                        Data += "\n" + billingItem.ZIP;
                    }
                    Font dataFont = normalFont;
                    picklistTable.AddCell(new PdfPCell(new Phrase(Data, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase(Data, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });

                    document.Add(picklistTable);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private void WriteInboundReceive(List<BillingInboundReceiving> _inbReceData)
        {
            try
            {
                int index = 0;
                if (_inbReceData != null && _inbReceData.Count > 0)
                {
                    PdfPTable picklistTable = null;
                    float[] widths = null;
                    picklistTable = new PdfPTable(11);
                    widths = new float[] { 2f, 4f, 6f, 4f, 4f, 5f, 2f, 3f, 3f, 3f, 4f };
                    picklistTable.TotalWidth = 600f;
                    picklistTable.HorizontalAlignment = Element.ALIGN_CENTER;
                    picklistTable.SpacingBefore = 10f;
                    picklistTable.LockedWidth = true;

                    picklistTable.SetWidths(widths);

                    Font headerFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                    Font normalFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                    Font fontHighlight = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);

                    picklistTable.AddCell(new PdfPCell(new Phrase("Inbound : Receiving & Putaway Charges", headerFont)) { Colspan = 11, Rowspan = 1, Border = 0, HorizontalAlignment = PdfPCell.ALIGN_CENTER, MinimumHeight = 20 });


                    picklistTable.AddCell(new PdfPCell(new Phrase("Sr.No", headerFont)) { Colspan = 1, Rowspan = 1 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Store Ref. No.", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Supplier", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("PO Number", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Receipt Date", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

                    picklistTable.AddCell(new PdfPCell(new Phrase("Service/Material", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("UoM", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Quantity", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Unit Cost", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

                    picklistTable.AddCell(new PdfPCell(new Phrase("TAX", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Total Cost", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });


                    Font dataFont = normalFont;
                    decimal inbReceive = 0;
                    foreach (BillingInboundReceiving inbReceiveItem in _inbReceData)
                    {
                        picklistTable.AddCell(new PdfPCell(new Phrase((++index).ToString(), dataFont)) { Colspan = 1, Rowspan = 1, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(inbReceiveItem.StoreRefNo, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(inbReceiveItem.SupplierName, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(inbReceiveItem.PONumber, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(inbReceiveItem.Receipt, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });

                        picklistTable.AddCell(new PdfPCell(new Phrase(inbReceiveItem.ServiceMaterial, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(inbReceiveItem.UoM, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(inbReceiveItem.Quantity, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(inbReceiveItem.UnitCost, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });

                        picklistTable.AddCell(new PdfPCell(new Phrase(inbReceiveItem.Tax, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(inbReceiveItem.TotalCost, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });

                        inbReceive = inbReceive + Convert.ToDecimal(inbReceiveItem.TotalCost.ToString());
                    }
                    BillingReceiveTotal = inbReceive;
                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });

                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });

                    picklistTable.AddCell(new PdfPCell(new Phrase("Total", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase(inbReceive.ToString(), dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    document.Add(picklistTable);
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void WriteOutboundPicking(List<BillingOutboundPicking> _obdPickData)
        {
            try
            {
                int index = 0;
                if (_obdPickData != null && _obdPickData.Count > 0)
                {
                    PdfPTable picklistTable = null;
                    float[] widths = null;
                    picklistTable = new PdfPTable(11);
                    widths = new float[] { 2f, 4f, 6f, 4f, 4f, 5f, 2f, 3f, 3f, 3f, 4f };
                    picklistTable.TotalWidth = 600f;
                    picklistTable.HorizontalAlignment = Element.ALIGN_CENTER;
                    picklistTable.SpacingBefore = 10f;
                    picklistTable.LockedWidth = true;

                    picklistTable.SetWidths(widths);

                    Font headerFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                    Font normalFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                    Font fontHighlight = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);

                    picklistTable.AddCell(new PdfPCell(new Phrase("Outbound : Picking & Packing Charges", headerFont)) { Colspan = 11, Rowspan = 1, Border = 0, HorizontalAlignment = PdfPCell.ALIGN_CENTER, MinimumHeight = 20 });

                    picklistTable.AddCell(new PdfPCell(new Phrase("Sr.No", headerFont)) { Colspan = 1, Rowspan = 1 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("OBD Number", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Customer", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("SO Number", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Delivery Date", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

                    picklistTable.AddCell(new PdfPCell(new Phrase("Service/Material", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("UoM", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Quantity", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Unit Cost", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

                    picklistTable.AddCell(new PdfPCell(new Phrase("TAX", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Total Cost", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });


                    Font dataFont = normalFont;
                    decimal obdPick = 0;
                    foreach (BillingOutboundPicking obdPickItem in _obdPickData)
                    {
                        picklistTable.AddCell(new PdfPCell(new Phrase((++index).ToString(), dataFont)) { Colspan = 1, Rowspan = 1, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(obdPickItem.OBDNumber, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(obdPickItem.CustomerName, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(obdPickItem.SONumber, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(obdPickItem.Delivery, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });

                        picklistTable.AddCell(new PdfPCell(new Phrase(obdPickItem.ServiceMaterial, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(obdPickItem.UoM, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(obdPickItem.Quantity, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(obdPickItem.UnitCost, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });

                        picklistTable.AddCell(new PdfPCell(new Phrase(obdPickItem.Tax, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(obdPickItem.TotalCost, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });

                        obdPick = obdPick + Convert.ToDecimal(obdPickItem.TotalCost.ToString());
                    }

                    BillingPickingTotal = obdPick;

                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });

                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });

                    picklistTable.AddCell(new PdfPCell(new Phrase("Total", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase(obdPick.ToString(), dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    document.Add(picklistTable);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void WriteOutboundLoading(List<BillingOutboundLoading> _obdLoadData)
        {
            try
            {
                int index = 0;
                if (_obdLoadData != null && _obdLoadData.Count > 0)
                {
                    PdfPTable picklistTable = null;
                    float[] widths = null;
                    picklistTable = new PdfPTable(11);
                    widths = new float[] { 2f, 4f, 6f, 4f, 4f, 5f, 2f, 3f, 3f, 3f, 4f };
                    picklistTable.TotalWidth = 600f;
                    picklistTable.HorizontalAlignment = Element.ALIGN_CENTER;
                    picklistTable.SpacingBefore = 10f;
                    picklistTable.LockedWidth = true;

                    picklistTable.SetWidths(widths);

                    Font headerFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                    Font normalFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                    Font fontHighlight = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);

                    picklistTable.AddCell(new PdfPCell(new Phrase("Outbound : Loading Charges", headerFont)) { Colspan = 11, Rowspan = 1, Border = 0, HorizontalAlignment = PdfPCell.ALIGN_CENTER, MinimumHeight = 20 });

                    picklistTable.AddCell(new PdfPCell(new Phrase("Sr.No", headerFont)) { Colspan = 1, Rowspan = 1 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("OBD Number", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Customer", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("SO Number", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Delivery Date", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

                    picklistTable.AddCell(new PdfPCell(new Phrase("Service/Material", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("UoM", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Quantity", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Unit Cost", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

                    picklistTable.AddCell(new PdfPCell(new Phrase("TAX", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Total Cost", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });


                    Font dataFont = normalFont;
                    decimal obdLoad = 0;
                    foreach (BillingOutboundLoading obdLoadItem in _obdLoadData)
                    {
                        picklistTable.AddCell(new PdfPCell(new Phrase((++index).ToString(), dataFont)) { Colspan = 1, Rowspan = 1, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(obdLoadItem.OBDNumber, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(obdLoadItem.CustomerName, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(obdLoadItem.SONumber, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(obdLoadItem.Delivery, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });

                        picklistTable.AddCell(new PdfPCell(new Phrase(obdLoadItem.ServiceMaterial, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(obdLoadItem.UoM, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(obdLoadItem.Quantity, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(obdLoadItem.UnitCost, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });

                        picklistTable.AddCell(new PdfPCell(new Phrase(obdLoadItem.Tax, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(obdLoadItem.TotalCost, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });

                        obdLoad = obdLoad + Convert.ToDecimal(obdLoadItem.TotalCost.ToString());
                    }

                    BillingLoadTotal = obdLoad;

                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });

                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });

                    picklistTable.AddCell(new PdfPCell(new Phrase("Total", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase(obdLoad.ToString(), dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    document.Add(picklistTable);
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void WriteStorageBillingData(List<StorageBilingData> _strBillingData)
        {
            try
            {
                int index = 0;
                if (_strBillingData != null && _strBillingData.Count > 0)
                {
                    PdfPTable picklistTable = null;
                    float[] widths = null;
                    picklistTable = new PdfPTable(8);
                    widths = new float[] { 2f, 4f, 6f, 3f, 4f, 3f, 5f, 3f };
                    picklistTable.TotalWidth = 600f;
                    picklistTable.HorizontalAlignment = Element.ALIGN_CENTER;
                    picklistTable.SpacingBefore = 10f;
                    picklistTable.LockedWidth = true;

                    picklistTable.SetWidths(widths);

                    Font headerFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                    Font normalFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                    Font fontHighlight = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);

                    picklistTable.AddCell(new PdfPCell(new Phrase("Storage Billing Report", headerFont)) { Colspan = 11, Rowspan = 1, Border = 0, HorizontalAlignment = PdfPCell.ALIGN_CENTER, MinimumHeight = 20 });

                    picklistTable.AddCell(new PdfPCell(new Phrase("Sr.No", headerFont)) { Colspan = 1, Rowspan = 1 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Part No.", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Description", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("UoM", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Date", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

                    picklistTable.AddCell(new PdfPCell(new Phrase("Avaliable Qty.", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Unit Cost", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Total Cost", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

                    Font dataFont = normalFont;
                    decimal strData = 0;
                    foreach (StorageBilingData strBilligItem in _strBillingData)
                    {
                        picklistTable.AddCell(new PdfPCell(new Phrase((++index).ToString(), dataFont)) { Colspan = 1, Rowspan = 1, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(strBilligItem.MCode, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(strBilligItem.MDescription, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(strBilligItem.UoM, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(strBilligItem.date, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });

                        picklistTable.AddCell(new PdfPCell(new Phrase(strBilligItem.AvailableQty, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(strBilligItem.UnitCost, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(strBilligItem.TotalCost, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });

                        strData = strData + Convert.ToDecimal(strBilligItem.TotalCost.ToString());
                    }
                    BillingStorageTotal = strData;
                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });

                    picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });

                    picklistTable.AddCell(new PdfPCell(new Phrase("Total", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase(strData.ToString(), dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    document.Add(picklistTable);
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void WriteConsolidatedData()
        {
            try
            {
                PdfPTable picklistTable = null;
                float[] widths = null;
                picklistTable = new PdfPTable(3);
                widths = new float[] { 2f, 4f, 6f };
                picklistTable.TotalWidth = 600f;
                picklistTable.HorizontalAlignment = Element.ALIGN_CENTER;
                picklistTable.SpacingBefore = 10f;
                picklistTable.LockedWidth = true;

                picklistTable.SetWidths(widths);

                Font headerFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                Font normalFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                Font fontHighlight = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);

                Font dataFont = normalFont;
                picklistTable.AddCell(new PdfPCell(new Phrase("Consolidated Data", headerFont)) { Colspan = 11, Rowspan = 1, Border = 0, HorizontalAlignment = PdfPCell.ALIGN_CENTER, MinimumHeight = 20 });

                picklistTable.AddCell(new PdfPCell(new Phrase("Inbound Charges", headerFont)) { Colspan = 1, Rowspan = 2, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                picklistTable.AddCell(new PdfPCell(new Phrase("Unloading & Handling Charges", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                picklistTable.AddCell(new PdfPCell(new Phrase(BillingUnloadTotal.ToString(), dataFont)) { Colspan = 1, Rowspan = 1, MinimumHeight = 20, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });

                picklistTable.AddCell(new PdfPCell(new Phrase("Receiving and Putaway Charges", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                picklistTable.AddCell(new PdfPCell(new Phrase(BillingReceiveTotal.ToString(), dataFont)) { Colspan = 1, Rowspan = 1, MinimumHeight = 20, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });

                picklistTable.AddCell(new PdfPCell(new Phrase("Outbound Charges", headerFont)) { Colspan = 1, Rowspan = 2, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                picklistTable.AddCell(new PdfPCell(new Phrase("Picking and Packing Charges", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                picklistTable.AddCell(new PdfPCell(new Phrase(BillingPickingTotal.ToString(), dataFont)) { Colspan = 1, Rowspan = 1, MinimumHeight = 20, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });

                picklistTable.AddCell(new PdfPCell(new Phrase("Loading Charges", headerFont)) { Colspan = 1, Rowspan = 1, MinimumHeight = 20, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                picklistTable.AddCell(new PdfPCell(new Phrase(BillingLoadTotal.ToString(), dataFont)) { Colspan = 1, Rowspan = 1, MinimumHeight = 20, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });

                picklistTable.AddCell(new PdfPCell(new Phrase("Storage Charges", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, MinimumHeight = 20 });
                picklistTable.AddCell(new PdfPCell(new Phrase(BillingStorageTotal.ToString(), dataFont)) { Colspan = 1, Rowspan = 1, MinimumHeight = 20, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
                decimal Total = BillingUnloadTotal + BillingReceiveTotal + BillingPickingTotal + BillingLoadTotal + BillingStorageTotal;
                picklistTable.AddCell(new PdfPCell(new Phrase("Total", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                picklistTable.AddCell(new PdfPCell(new Phrase("", headerFont)) { Colspan = 1, Rowspan = 1 });
                picklistTable.AddCell(new PdfPCell(new Phrase(Total.ToString(), dataFont)) { Colspan = 1, Rowspan = 1, MinimumHeight = 20, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
                document.Add(picklistTable);

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //======================= END ===================//






        private void WritePickListLikeKiwiTemplate(List<MRLWMSC21Common.PrintCommon.PackingSlip1> _lPlist, List<MRLWMSC21Common.PrintCommon.BoxItemDetails1> _lPPacklist)
        {
            if (_lPlist != null && _lPlist.Count > 0)
            {
                int NO_PAGES_REQUIRED = _lPlist.Count;
                int picklistCnt = 0;
                int index = 0;
                // document.SetPageSize(PageSize.A4.Rotate());


                if (_lPlist != null && _lPlist.Count > 0)
                {
                    WriteHeaderForKiwi_PickList(_picklist, _pickHeader);
                    PdfPTable picklistTable = null;
                    float[] widths = null;


                    picklistTable = new PdfPTable(6);
                    widths = new float[] { 2f, 3f, 10f, 10f, 2f, 2f };
                    picklistTable.TotalWidth = 600f;
                    picklistTable.HorizontalAlignment = Element.ALIGN_CENTER;
                    picklistTable.SpacingBefore = 10f;
                    picklistTable.LockedWidth = true;


                    picklistTable.SetWidths(widths);

                    Font headerFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                    Font normalFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                    Font fontHighlight = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);

                    string _PickRefNo = _pickHeader[0] != null && _pickHeader[0].CustomerName != null ? _pickHeader[0].CustomerName : "";

                    picklistTable.AddCell(new PdfPCell(new Phrase("Line.", headerFont)) { Colspan = 1, Rowspan = 1 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("ItemCode", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Item Description", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Special Handling Instructions", headerFont)) { Colspan = 2, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    //picklistTable.AddCell(new PdfPCell(new Phrase("Ordered", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Shipped", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    //picklistTable.AddCell(new PdfPCell(new Phrase("Required Qty", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    //picklistTable.AddCell(new PdfPCell(new Phrase("Pick List Qty", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    //picklistTable.AddCell(new PdfPCell(new Phrase("Remarks", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

                    Font dataFont = normalFont;
                    decimal totalPickedqty = 0;
                    decimal totalPackedqty = 0;
                    //decimal totalReqQty = 0;
                    //MRLWMSC21Common.PrintCommon.PackingData1 obj;
                    //MRLWMSC21Common.PrintCommon.PackingSlip1 oSlip = new MRLWMSC21Common.PrintCommon.PackingSlip1();
                    foreach (MRLWMSC21Common.PrintCommon.BoxItemDetails1 piclistItem in _picklist)
                    {
                        picklistTable.AddCell(new PdfPCell(new Phrase((++index).ToString(), dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.MCode, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.ItemDesc, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.HandlingRemarks, dataFont)) { Colspan = 2, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        // picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.PickedQty, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER, MinimumHeight = 20 });
                        picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.PackedQty, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER, MinimumHeight = 20 });
                        //picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.ReqQty, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        //picklistTable.AddCell(new PdfPCell(new Phrase(piclistItem.Qty, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                        //totalPickedqty = totalPickedqty + Convert.ToDecimal(piclistItem.PickedQty);
                        totalPackedqty = totalPackedqty + Convert.ToDecimal(piclistItem.PackedQty);
                        //if (piclistItem.MCode)
                        //{
                        //totalPickedqty = totalPickedqty + Convert.ToDecimal(piclistItem.PickedQty);
                        //_pickHeader[0].TotPickedQty
                        //}
                        //totalReqQty = totalReqQty + Convert.ToDecimal(piclistItem.ReqQty);
                        //picklistTable.AddCell(new PdfPCell(new Phrase(" ", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    }

                    picklistTable.AddCell(new PdfPCell(new Phrase("SHIPPING AND LOADING INSTRUCTIONS:", headerFont)) { Colspan = 3, Rowspan = 8, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    //picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    //picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                    //picklistTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("Totals For Order:", headerFont)) { Colspan = 2, Rowspan = 8, HorizontalAlignment = PdfPCell.ALIGN_CENTER, MinimumHeight = 20 });
                    //picklistTable.AddCell(new PdfPCell(new Phrase(""+ totalPickedqty, dataFont)) { Colspan = 1, Rowspan = 8, HorizontalAlignment = PdfPCell.ALIGN_CENTER, MinimumHeight = 20 });
                    picklistTable.AddCell(new PdfPCell(new Phrase("" + totalPackedqty, dataFont)) { Colspan = 1, Rowspan = 8, HorizontalAlignment = PdfPCell.ALIGN_CENTER, MinimumHeight = 20 });

                    /*****************************Bottom Totals**************************************/
                    //picklistTable.AddCell(GetCellWithAlign(" ", headerFont, Element.ALIGN_LEFT, Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER, colspan: 1));
                    //picklistTable.AddCell(GetCellWithAlign(" ", headerFont, Element.ALIGN_LEFT, Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER, colspan: 1));
                    //picklistTable.AddCell(GetCellWithAlign(" ", headerFont, Element.ALIGN_CENTER, Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER, colspan: 1));
                    //picklistTable.AddCell(GetCellWithAlign(" ", headerFont, Element.ALIGN_CENTER, Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER, colspan: 1));
                    //picklistTable.AddCell(GetCellWithAlign(" ", headerFont, Element.ALIGN_CENTER, Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER, colspan: 1));
                    //picklistTable.AddCell(GetCellWithAlign("Totay Qty", headerFont, Element.ALIGN_JUSTIFIED, Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER, colspan: 1));
                    //// picklistTable.AddCell(GetCellWithAlign("" + totalReqQty, headerFont, Element.ALIGN_LEFT, Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER, colspan: 1));
                    //picklistTable.AddCell(GetCellWithAlign("" + totalqty, headerFont, Element.ALIGN_LEFT, Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER, colspan: 1));
                    //picklistTable.AddCell(GetCellWithAlign(" ", headerFont, Element.ALIGN_LEFT, Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER, colspan: 1));

                    document.Add(picklistTable);

                    PdfPTable packlistWeightTable = null;
                    packlistWeightTable = new PdfPTable(4);
                    widths = new float[] { 5f, 5f, 5f, 5f };
                    packlistWeightTable.TotalWidth = 600f;
                    packlistWeightTable.HorizontalAlignment = Element.ALIGN_CENTER;
                    packlistWeightTable.SpacingBefore = 10f;
                    packlistWeightTable.LockedWidth = true;

                    packlistWeightTable.AddCell(new PdfPCell(new Phrase("Gross Weight:", headerFont)) { Colspan = 1, Border = 0, Rowspan = 3, HorizontalAlignment = PdfPCell.ALIGN_RIGHT, MinimumHeight = 40 });
                    packlistWeightTable.AddCell(new PdfPCell(new Phrase(" ", dataFont)) { Colspan = 1, Rowspan = 3, Border = 0, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 40 });
                    packlistWeightTable.AddCell(new PdfPCell(new Phrase("Net Weight:", headerFont)) { Colspan = 1, Border = 0, Rowspan = 3, HorizontalAlignment = PdfPCell.ALIGN_RIGHT, MinimumHeight = 40 });
                    packlistWeightTable.AddCell(new PdfPCell(new Phrase("", dataFont)) { Colspan = 1, Rowspan = 3, Border = 0, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 40 });
                    document.Add(packlistWeightTable);

                    if (++picklistCnt < NO_PAGES_REQUIRED)
                        NewKiwiA4Required(height, true);

                }



            }

        }

        private void WriteHeaderForKiwi_PickList(List<MRLWMSC21Common.PrintCommon.BoxItemDetails1> _pickList, List<MRLWMSC21Common.PrintCommon.PackingSlip1> _pickHeader)
        {
            try
            {
                PdfPTable PicklistHeaderTable = new PdfPTable(7);
                PicklistHeaderTable.HorizontalAlignment = Element.ALIGN_CENTER;
                PicklistHeaderTable.TotalWidth = 600f;
                PicklistHeaderTable.SpacingBefore = 10f;
                PicklistHeaderTable.LockedWidth = true;
                PicklistHeaderTable.SetWidths(new float[] { 0.75f, 1.25f, 0.5f, 0.5f, 0.5f, 1f, 1f });

                Font HeaderFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1257, BaseFont.NOT_EMBEDDED), 16f);
                Font MainHeaderFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1257, BaseFont.NOT_EMBEDDED), 14f);
                Font headerFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                Font dataFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                Font tinyDataFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);
                Font fontHighlight = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 9f);

                LogoAdd1(imageFile);
                //FalconLogoAdd(imageFile1);


                PdfPCell cell = new PdfPCell(new Paragraph("", dataFont));
                cell.Colspan = 3;
                cell.Border = 0;

                cell.AddElement(addLogo);

                PdfPCell cell1 = new PdfPCell(new Paragraph("", dataFont));
                cell1.Colspan = 4;
                cell1.Border = 0;

                cell1.AddElement(addLogo1);

                PicklistHeaderTable.AddCell(cell);
                PicklistHeaderTable.AddCell(cell1);

                //PicklistHeaderTable.AddCell(new PdfPCell(new Phrase(Dimension.Kiwi_PickList_Headers_PickLIst["HEAD1"], MainHeaderFont)) { Colspan = 7, Rowspan = 1, Border = 0, HorizontalAlignment = PdfPCell.ALIGN_CENTER, MinimumHeight = 30 });
                //PicklistHeaderTable.AddCell(new PdfPCell(new Phrase(Dimension.Kiwi_PickList_Headers_PickLIst["HEAD2"], HeaderFont)) { Colspan = 7, Rowspan = 1, Border = 0, HorizontalAlignment = PdfPCell.ALIGN_CENTER, MinimumHeight = 30 });

                String _AccName = _pickHeader != null && _pickHeader[0].AccountName != null ? _pickHeader[0].AccountName : "";
                PicklistHeaderTable.AddCell(new PdfPCell(new Phrase(_AccName, MainHeaderFont)) { Colspan = 7, Rowspan = 1, Border = 0, HorizontalAlignment = PdfPCell.ALIGN_CENTER, MinimumHeight = 30 });

                String _CustName = _pickHeader != null && _pickHeader[0].CustomerName != null ? _pickHeader[0].CustomerName : "";
                PicklistHeaderTable.AddCell(new PdfPCell(new Phrase(_CustName, MainHeaderFont)) { Colspan = 7, Rowspan = 1, Border = 0, HorizontalAlignment = PdfPCell.ALIGN_CENTER, MinimumHeight = 30 });

                PicklistHeaderTable.AddCell(new PdfPCell(new Phrase(Dimension.Kiwi_PickList_Headers_PickLIst["HEADDING"], HeaderFont)) { Colspan = 7, Rowspan = 1, Border = 0, HorizontalAlignment = PdfPCell.ALIGN_CENTER, MinimumHeight = 30 });
                String _CustPONumber = _pickHeader != null && _pickHeader[0].CustPONumber != null ? _pickHeader[0].CustPONumber : "";
                String _PSNumber = _pickHeader != null && _pickHeader[0].PSNumber != null ? _pickHeader[0].PSNumber : "";


                PicklistHeaderTable.AddCell(new PdfPCell(new Phrase("ORDER NO.", headerFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, Border = Rectangle.RIGHT_BORDER | PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER });
                PicklistHeaderTable.AddCell(new PdfPCell(new Phrase(" " + _PSNumber, dataFont)) { Colspan = 1, Rowspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20, Border = Rectangle.RIGHT_BORDER | PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER });

                PdfContentByte cb = pdfWriter.DirectContent;
                PdfPCell cell2 = new PdfPCell(new Paragraph("", dataFont));
                iTextSharp.text.Image img = CreateBarCode(_PSNumber, cb);
                img.ScaleAbsolute(100f, 1f);
                cell2.Colspan = 3;
                cell2.AddElement(img);
                cell2.HorizontalAlignment = Rectangle.ALIGN_RIGHT;
                cell2.Border = Rectangle.RIGHT_BORDER | PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER;


                PicklistHeaderTable.AddCell(new PdfPCell(cell2));
                //PicklistHeaderTable.AddCell(GetCellWithAlign("", headerFont, Rectangle.TOP_BORDER, Element.ALIGN_LEFT, 3));
                PicklistHeaderTable.AddCell(new PdfPCell(new Phrase("SHIPPING DATE", headerFont)) { Colspan = 1, Rowspan = 4, HorizontalAlignment = PdfPCell.ALIGN_CENTER, Border = Rectangle.RIGHT_BORDER | PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER });
                PicklistHeaderTable.AddCell(new PdfPCell(new Phrase(" " + DateTime.Now, dataFont)) { Colspan = 1, Rowspan = 4, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20, Border = Rectangle.RIGHT_BORDER | PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER });

                //PicklistHeaderTable.AddCell(GetCellWithAlign(Dimension.Kiwi_PickList_Headers_PickLIst["CustPONumber"], headerFont, Rectangle.TOP_BORDER, Element.ALIGN_LEFT, 1));
                //PicklistHeaderTable.AddCell(GetCellWithAlign(":" + _CustPONumber, headerFont, Rectangle.TOP_BORDER, Element.ALIGN_LEFT, 1));
                //PicklistHeaderTable.AddCell(GetCellWithAlign("", headerFont, Rectangle.TOP_BORDER, Element.ALIGN_LEFT, 3));
                //PicklistHeaderTable.AddCell(GetCellWithAlign(Dimension.Kiwi_PickList_Headers_PickLIst["Shippingdate"], headerFont, Rectangle.TOP_BORDER, Element.ALIGN_LEFT, 1));
                //PicklistHeaderTable.AddCell(GetCellWithAlign(":" + DateTime.Now, headerFont, Rectangle.TOP_BORDER, Element.ALIGN_LEFT, 1));
                document.Add(PicklistHeaderTable);

                PdfPTable AddressTable = null;
                float[] widths = null;
                AddressTable = new PdfPTable(7);
                widths = new float[] { 0.5f, 1.25f, 0.5f, 0.5f, 0.5f, 1f, 1f };
                AddressTable.TotalWidth = 600f;
                AddressTable.HorizontalAlignment = Element.ALIGN_CENTER;
                AddressTable.SpacingBefore = 10f;
                AddressTable.SpacingAfter = 10f;
                AddressTable.LockedWidth = true;
                AddressTable.SetWidths(widths);
                AddressTable.DefaultCell.Border = 0;
                String _Address1 = _pickHeader != null && _pickHeader[0].AddressLine1 != null ? _pickHeader[0].AddressLine1 : "";
                String _Address2 = _pickHeader != null && _pickHeader[0].Address2 != null ? _pickHeader[0].Address2 : "";
                //AddressTable.AddCell(new PdfPCell(new Phrase(Dimension.Kiwi_PickList_Headers_PickLIst["HEAD1"], HeaderFont)) { Colspan = 2, Rowspan = 1, Border = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER, MinimumHeight = 30 });
                //AddressTable.AddCell(new PdfPCell(new Phrase(Dimension.Kiwi_PickList_Headers_PickLIst["HEAD1"], HeaderFont)) { Colspan = 2, Rowspan = 1, Border = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER, MinimumHeight = 30 });

                AddressTable.AddCell(new PdfPCell(new Phrase(Dimension.Kiwi_PickList_Headers_PickLIst["Address1"], headerFont)) { Colspan = 1, Rowspan = 8, Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 30 });
                AddressTable.AddCell(new PdfPCell(new Phrase(" " + _Address1, dataFont)) { Colspan = 3, Rowspan = 8, Border = Rectangle.RIGHT_BORDER | PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER, HorizontalAlignment = PdfPCell.ALIGN_CENTER, MinimumHeight = 20 });
                //AddressTable.AddCell(new PdfPCell(new Phrase(" ", dataFont)) { Colspan = 1, Rowspan = 8, Border = 4, HorizontalAlignment = PdfPCell.ALIGN_CENTER, MinimumHeight = 20 });
                AddressTable.AddCell(new PdfPCell(new Phrase(Dimension.Kiwi_PickList_Headers_PickLIst["Address2"], headerFont)) { Colspan = 1, Rowspan = 8, Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER, HorizontalAlignment = PdfPCell.ALIGN_CENTER, MinimumHeight = 30 });
                AddressTable.AddCell(new PdfPCell(new Phrase(" " + _Address1, dataFont)) { Colspan = 2, Rowspan = 8, Border = Rectangle.RIGHT_BORDER | PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER, HorizontalAlignment = PdfPCell.ALIGN_CENTER, MinimumHeight = 20 });
                //AddressTable.AddCell(new PdfPCell(new Phrase(" ", dataFont)) { Colspan = 1, Rowspan = 8, Border = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER, MinimumHeight = 20 });

                document.Add(AddressTable);

                PdfPTable POTable = null;
                POTable = new PdfPTable(7);
                widths = new float[] { 0.75f, 1.25f, 0.5f, 0.5f, 0.5f, 1f, 1f };
                POTable.TotalWidth = 600f;
                POTable.HorizontalAlignment = Element.ALIGN_CENTER;
                POTable.SpacingBefore = 10f;
                POTable.SpacingAfter = 10f;
                POTable.LockedWidth = true;
                POTable.SetWidths(widths);

                PdfPCell cell3 = new PdfPCell(new Paragraph("", dataFont));
                iTextSharp.text.Image img1 = CreateBarCode(_CustPONumber, cb);
                img1.ScaleAbsolute(100f, 1f);
                cell3.Colspan = 5;
                cell3.Rowspan = 1;
                cell3.AddElement(img1);
                cell3.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                cell3.Border = Rectangle.RIGHT_BORDER | PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER; ;
                POTable.AddCell(new PdfPCell(new Phrase(Dimension.Kiwi_PickList_Headers_PickLIst["CustPONumber"], headerFont)) { Colspan = 1, Rowspan = 1, Border = Rectangle.RIGHT_BORDER | PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 30 });

                POTable.AddCell(new PdfPCell(new Phrase(" " + _CustPONumber, dataFont)) { Colspan = 1, Rowspan = 1, Border = Rectangle.RIGHT_BORDER | PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 30 });
                //POTable.AddCell(new PdfPCell(new Phrase(" ", dataFont)) { Colspan = 1, Rowspan = 8, Border = Rectangle.RIGHT_BORDER | PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 30 });
                POTable.AddCell(new PdfPCell(cell3));

                //POTable.AddCell(new PdfPCell(new Phrase(Dimension.Kiwi_PickList_Headers_PickLIst["Address2"], headerFont)) { Colspan = 1, Rowspan = 1, Border = 1, HorizontalAlignment = PdfPCell.ALIGN_CENTER, MinimumHeight = 30 });
                //POTable.AddCell(new PdfPCell(new Phrase(":" + _Address2, dataFont)) { Colspan = 1, Rowspan = 1, Border = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT, MinimumHeight = 20 });
                document.Add(POTable);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /*********************************** Creation of Vehicle Gate Pass ****************************/

        private PdfPCell GetCellWithAlign(String str, Font font, int align = 1, int border = 4, int colspan = 1)
        {
            PdfPCell cell = new PdfPCell(new Phrase(str, font));
            cell.HorizontalAlignment = align;
            cell.Border = border;
            cell.Colspan = colspan;
            return cell;
        }

        public static string NumberToWords(decimal DecimalValue)
        {
            // int number = Convert.ToInt32(DecimalValue);
            int number = Convert.ToInt32(Math.Truncate(DecimalValue));
            int DecimalPart = Convert.ToInt32((DecimalValue - number) * 100);

            int Number = Math.Abs(number);

            if (DecimalPart < 0)
            {
                DecimalPart = Convert.ToInt32((100 + DecimalPart));
            }
            else
                DecimalPart = Convert.ToInt32((DecimalValue - number) * 100);
            if (number == 0)
                return "zero";

            if (Number < 0)
                return "minus " + NumberToWords(Math.Abs(Number));

            string words = "";

            if ((Number / 1000000) > 0)
            {
                words += NumberToWords(Number / 100000) + " Lakh ";
                Number %= 100000;
            }

            if ((Number / 1000) > 0)
            {
                words += NumberToWords(Number / 1000) + " Thousand ";
                Number %= 1000;
            }

            if ((Number / 100) > 0)
            {
                words += NumberToWords(Number / 100) + " Hundred ";
                Number %= 100;
            }

            if (Number > 0)
            {
                if (words != "")
                    words += "";

                var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

                if (Number < 20)
                    words += unitsMap[Number];
                else
                {
                    words += tensMap[Number / 10];
                    if ((Number % 10) > 0)
                        words += "-" + unitsMap[Number % 10];
                }
            }

            string _sDecimalString = string.Empty;

            if (DecimalPart > 0)
                _sDecimalString = NumberToWords(DecimalPart);

            words = words + (_sDecimalString.Length > 0 ? " and " + _sDecimalString + " paise" : "");

            return words;
        }

        public static iTextSharp.text.Image CreateBarCode(string BarcodeString, PdfContentByte cb)
        {

            Barcode128 code128 = new Barcode128();
            code128.CodeType = Barcode.EAN13;
            code128.ChecksumText = false;
            code128.GenerateChecksum = true;
            code128.BarHeight = 20;
            code128.Code = BarcodeString;
            code128.Font = null;

            iTextSharp.text.Image image128 = code128.CreateImageWithBarcode(cb, null, null);



            image128.ScaleToFit(20, 20);
            return image128;
        }

        public static iTextSharp.text.Image CreateBarCodeWarehouse(string BarcodeString, PdfContentByte cb)
        {

            Barcode128 code128 = new Barcode128();
            code128.CodeType = Barcode.CODE128;
            code128.ChecksumText = true;
            code128.GenerateChecksum = true;
            code128.BarHeight = 35;
            code128.Code = BarcodeString;

            iTextSharp.text.Image image128 = code128.CreateImageWithBarcode(cb, null, null);
            image128.SetAbsolutePosition(10, 300);
            image128.ScalePercent(70);

            //image128.ScaleToFit(10, 10);
            return image128;
        }

        private void InitAddressFonts()
        {
            BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            pdfByteContent.SetColorFill(BaseColor.BLACK);
            pdfByteContent.SetFontAndSize(baseFont, 8);
            pdfByteContent.BeginText();
        }

        private void InitHeaderFonts()
        {
            BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            pdfByteContent.SetColorFill(BaseColor.BLACK);
            pdfByteContent.SetFontAndSize(baseFont, 9);
            pdfByteContent.BeginText();
        }

        private void InitializePDFObject(string fileName)
        {

            pdfFile = fileLocation + fileName;
            document = new Document(new RectangleReadOnly(600, 800), 5, 5, 5, 5);
            memoryStream = new MemoryStream();
            fileStream = new FileStream(pdfFile, FileMode.Create);
            pdfWriter = PdfWriter.GetInstance(document, fileStream);
            // document.SetPageSize(PageSize.A4.Rotate());
            document.Open();
            pdfByteContent = pdfWriter.DirectContent;
        }

        private void InitializeKiwiPDFObject(string fileName)
        {
            pdfFile = fileLocation + fileName;
            document = new Document(new RectangleReadOnly(660, 900));
            // document.SetPageSize(PageSize.A4.Rotate());
            memoryStream = new MemoryStream();
            fileStream = new FileStream(pdfFile, FileMode.Create);
            pdfWriter = PdfWriter.GetInstance(document, fileStream);
            document.Open();
        }

        private void InitializeKiwiPDFObject_RCR_Lanscape(string fileName)
        {
            pdfFile = fileLocation + fileName;
            document = new Document(PageSize.A4_LANDSCAPE);
            document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            memoryStream = new MemoryStream();
            fileStream = new FileStream(pdfFile, FileMode.Create);
            pdfWriter = PdfWriter.GetInstance(document, fileStream);
            document.Open();
        }
        private void BarcodePDFObject(string fileName)
        {
            pdfFile = fileLocation + fileName;
            document = new Document(new RectangleReadOnly(900, 600));
            // document.SetPageSize(PageSize.A4.Rotate());
            memoryStream = new MemoryStream();
            fileStream = new FileStream(pdfFile, FileMode.Create);
            pdfWriter = PdfWriter.GetInstance(document, fileStream);
            document.Open();
        }

        private void InitDirectContentWriter()
        {
            height = Dimension.YEND;
            document.NewPage();
            LogoAdd(imageFile);
            document.Add(addLogo);
            pdfByteContent = pdfWriter.DirectContent;
            //InitializeBaseFont();
        }

        private void LogoAdd(string picLocation)
        {

            try
            {
                System.Drawing.Image image = System.Drawing.Image.FromFile(picLocation);
                addLogo = default(iTextSharp.text.Image);

                MemoryStream imageStream = new MemoryStream();
                image.Save(imageStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                addLogo = iTextSharp.text.Image.GetInstance(imageStream.ToArray());

                addLogo.ScaleToFit(100, 30);
                addLogo.Alignment = iTextSharp.text.Image.ALIGN_LEFT;

            }
            catch (Exception ex)
            {
                throw new Exception("exception while writing logo in pdf:" + ex.Message);
            }

        }

        /// <summary>
        /// Pick List Customized Size Logo Adding
        /// </summary>
        private void LogoAdd1(string picLocation)
        {

            try
            {
                System.Drawing.Image image = System.Drawing.Image.FromFile(picLocation);
                addLogo = default(iTextSharp.text.Image);

                MemoryStream imageStream = new MemoryStream();
                image.Save(imageStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                addLogo = iTextSharp.text.Image.GetInstance(imageStream.ToArray());

                addLogo.ScaleToFit(100, 45);
                addLogo.Alignment = iTextSharp.text.Image.ALIGN_LEFT;

            }
            catch (Exception ex)
            {
                throw new Exception("exception while writing logo in pdf:" + ex.Message);
            }

        }
        private void FalconLogoAdd(string picLocation)
        {

            try
            {
                System.Drawing.Image image = System.Drawing.Image.FromFile(picLocation);
                addLogo1 = default(iTextSharp.text.Image);

                MemoryStream imageStream = new MemoryStream();
                image.Save(imageStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                addLogo1 = iTextSharp.text.Image.GetInstance(imageStream.ToArray());

                addLogo1.ScaleToFit(100, 45);
                addLogo1.Alignment = iTextSharp.text.Image.ALIGN_RIGHT;

            }
            catch (Exception ex)
            {
                throw new Exception("exception while writing logo in pdf:" + ex.Message);
            }

        }

        private void NewPageRequire(int height, bool forceDocCreation)
        {
            if (height <= (Dimension.YSTART) + 15 || forceDocCreation)
            {
                pdfByteContent.EndText();
                document.NewPage();
                this.height = Dimension.YEND;
                LogoAdd(imageFile);
                document.Add(addLogo);
                InitializeBaseFont();
            }
        }

        private void NewKiwiA4Require(int height, bool forceDocCreation)
        {
            if (forceDocCreation || height <= (Dimension.YSTART) + 20)
            {
                document.NewPage();
                this.height = Dimension.YEND;
                LogoAdd(imageFile);
                document.Add(addLogo);
            }
        }

        private void NewKiwiA4Required(int height, bool forceDocCreation)
        {
            if (forceDocCreation || height <= (Dimension.YSTART) + 20)
            {
                document.NewPage();
                this.height = Dimension.YEND1;
                //LogoAdd(imageFile);
                //document.Add(addLogo);
            }
        }

        private void InitializeBaseFont()
        {
            BaseFont baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            pdfByteContent.SetColorFill(BaseColor.BLACK);
            pdfByteContent.SetFontAndSize(baseFont, 9);
            pdfByteContent.BeginText();
        }
        public string GenerateBarCode()
        {
            try
            {
                PBPLBarcodeTemplate();
                CreateDirectory(fileLocation);
                fileLocation += subDirectory;
                CreateDirectory(fileLocation);
                string _FileName = "Doc" + "(" + DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss").Replace(" ", "").Replace(":", "") + ")" + ".pdf";
                pageNo = 0;
                height = Dimension.YEND;
                height -= Dimension.LINE_GAP;


                BarcodePDFObject(_FileName);
                write_BarCode_Lable_Print(_barCode);

                document.Close();
                fileStream.Close();
                return subDirectory + _FileName;
            }
            catch (Exception e)
            {
                if (document != null)
                {
                    if (pdfByteContent != null)
                        pdfByteContent.EndText();
                    document.Close();
                }

                if (fileStream != null) { fileStream.Close(); }
                throw new Exception("BarCode Generation problem:" + e.StackTrace);
            }

        }
        private void PBPLBarcodeTemplate()
        {
            fileLocation = MRLWMSC21Common.CommonLogic.SafeMapPath("~/TPL/BarcodePdf/");

        }
        private void write_BarCode_Lable_Print(NewWarehouse.Barcode _barCode)
        {
            try
            {
                document.SetPageSize(PageSize.A4.Rotate());
                PdfPTable Barcode = new PdfPTable(2);
                Barcode.HorizontalAlignment = Element.ALIGN_CENTER;
                Barcode.TotalWidth = 800f;
                //Barcode.TotalHeight = 800f;
                Barcode.SpacingBefore = 0f;
                Barcode.LockedWidth = true;
                Barcode.SetWidths(new float[] { 10f, 0.1f });





                Font headerFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 60f);
                Font dataFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 10f);
                Font tinyDataFont = new Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 8f);
                Font fontHighlight = new Font(BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 8f);



                PdfContentByte cb = pdfWriter.DirectContent;

                PdfPCell cell2 = new PdfPCell(new Paragraph("", dataFont));
                iTextSharp.text.Image img = CreateBarCodeWarehouse(_barCode.LocationCode, cb);
                //img.ScaleAbsolute(10, 500);

                //cell2.Colspan = 1;
                cell2.AddElement(img);
                cell2.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                cell2.Border = 0;
                cell2.FixedHeight = 80;
                //cell.Width = 500;

                Barcode.AddCell(new PdfPCell(new Phrase("", headerFont)) { Colspan = 2, Rowspan = 10, HorizontalAlignment = PdfPCell.ALIGN_CENTER, Border = 0 });
                Barcode.AddCell(new PdfPCell(new Phrase("", headerFont)) { Colspan = 2, HorizontalAlignment = PdfPCell.ALIGN_CENTER, Border = 0 });
                Barcode.AddCell(new PdfPCell(new Phrase("", headerFont)) { Colspan = 2, HorizontalAlignment = PdfPCell.ALIGN_CENTER, Border = 0 });
                Barcode.AddCell(new PdfPCell(new Phrase("", headerFont)) { Colspan = 2, HorizontalAlignment = PdfPCell.ALIGN_CENTER, Border = 0 });
                Barcode.AddCell(new PdfPCell(new Phrase("", headerFont)) { Colspan = 2, HorizontalAlignment = PdfPCell.ALIGN_CENTER, Border = 0 });
                Barcode.AddCell(new PdfPCell(new Phrase("", headerFont)) { Colspan = 2, HorizontalAlignment = PdfPCell.ALIGN_CENTER, Border = 0 });
                Barcode.AddCell(new PdfPCell(new Phrase("", headerFont)) { Colspan = 2, HorizontalAlignment = PdfPCell.ALIGN_CENTER, Border = 0 });
                Barcode.AddCell(new PdfPCell(new Phrase("", headerFont)) { Colspan = 2, HorizontalAlignment = PdfPCell.ALIGN_CENTER, Border = 0 });
                Barcode.AddCell(new PdfPCell(new Phrase("", headerFont)) { Colspan = 2, HorizontalAlignment = PdfPCell.ALIGN_CENTER, Border = 0 });
                Barcode.AddCell(new PdfPCell(new Phrase("", headerFont)) { Colspan = 2, HorizontalAlignment = PdfPCell.ALIGN_CENTER, Border = 0 });


                Barcode.AddCell(img);
                //Barcode.AddCell(_barCode.DockName);
                Barcode.AddCell(GetCellWithAlign("", headerFont, Element.ALIGN_CENTER, Rectangle.NO_BORDER, colspan: 1));
                Barcode.AddCell(GetCellWithAlign("" + _barCode.DockName, headerFont, Element.ALIGN_CENTER, Rectangle.NO_BORDER, colspan: 2));
                document.Add(Barcode);


            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private void AddFooterInfo(string text)
        {

            pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, text, 60, 20, 0);
            pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "PAGE " + ++pageNo, Dimension.SUMMARY_COLUMNS[5], 20, 0);
        }

    }

}
