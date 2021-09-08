using MRLWMSC21.TPL.Invoice;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using MRLWMSC21Common;

namespace MRLWMSC21.TPL.InvoiceWriter
{
    public class TPLInvoiceLandscape
    {
        public int height { get; set; }
        public Document document { get; set; }
        public MemoryStream memoryStream { get; set; }
        public PdfWriter pdfWriter { get; set; }
        public FileStream fileStream { get; set; }
        public PdfContentByte pdfByteContent { get; set; }
        public PdfPTable StoreTable { get; set; }
        public BaseFont addressFont { get; set; }
        public BaseFont headerFont { get; set; }
        public int pageNo { get; set; }
        public iTextSharp.text.Image addLogo { get; set; }
        public IDictionary<int, IList<string>> weeklyStorageList { get; set; }
        
        public string footerInfo { get; set; }
        public string pdfFile;
        public string fileLocation = @"../InvoicePdf/";
        public string imageFile = "";

        public string fromDate;
        public string toDate;
        public string generationDate;
        public int InvoiceDays;

        public DateTime fromDateTime;
        public DateTime toDateTime;
        public DateTime generationDateTime;


        public void initInvoiceDays()
        {
            this.InvoiceDays = Convert.ToInt16((Convert.ToDateTime(this.toDate) - Convert.ToDateTime(this.fromDate)).TotalDays) + 1;
            int _temp = 0;
            this.weeklyStorageList = new Dictionary<int, IList<string>>();

            
            while (_temp < InvoiceDays)
            {
                int weekNo = (_temp / 7 == 0) ? (1) : (_temp / 7) + 1;
                string day = Convert.ToDateTime(this.fromDate).AddDays(_temp).ToString("dd/MM/yyyy");
                if (this.weeklyStorageList.ContainsKey(weekNo))
                {
                    IList<string> days = this.weeklyStorageList[weekNo];
                    days.Add(day);
                    this.weeklyStorageList[weekNo] = days;
                }
                else
                {
                    IList<string> days = new List<string>();
                    days.Add(day);
                    this.weeklyStorageList.Add(weekNo, days);
                }
                _temp++;
            }
        }

        public void newPageRequire(int height, bool forceDocCreation)
        {

            if (height <= (LandscapeDimension.yStart)+15 || forceDocCreation)
            {
                this.pdfByteContent.EndText();
                this.document.NewPage();
                this.height = LandscapeDimension.yEnd;
                logoAdd(imageFile);
                this.document.Add(addLogo);
                initializeBaseFont();
                addFooterIno(this.footerInfo);
            }

        }

        public void addFooterIno(string text) 
        {
            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, text, 60, 20, 0);
            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "PAGE " + ++pageNo, LandscapeDimension.rateDimension[6], 20, 0);
        }

        public void initializeBaseFont()
        {
            BaseFont baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            this.pdfByteContent.SetColorFill(BaseColor.BLACK);
            this.pdfByteContent.SetFontAndSize(baseFont, 9);
            this.pdfByteContent.BeginText();
        }

        
        public TPLInvoiceLandscape()
        {
            this.fileLocation = System.Web.HttpContext.Current.Server.MapPath("~/" + "/TPL/InvoicePdf/");
            this.imageFile = System.Web.HttpContext.Current.Server.MapPath("~/" + "/Images/inventrax_3pl.png");
        }


        public void initAddressFonts()
        {
            BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            this.pdfByteContent.SetColorFill(BaseColor.BLACK);
            this.pdfByteContent.SetFontAndSize(baseFont, 8);
            this.pdfByteContent.BeginText();
        }


        public void initHeaderFonts()
        {
            BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            this.pdfByteContent.SetColorFill(BaseColor.BLACK);
            this.pdfByteContent.SetFontAndSize(baseFont, 9);
            this.pdfByteContent.BeginText();
        }



        public TPLInvoiceLandscape(string fileLocation)
        {
            this.fileLocation = fileLocation;
        }



        public void initializePDFObject(string fileName)
        {

            this.pdfFile = this.fileLocation + fileName;
            this.document = new Document(new RectangleReadOnly(840, 640), 10, 10, 10, 10);
            this.memoryStream = new MemoryStream();

            this.fileStream = new FileStream(this.pdfFile, FileMode.Create);
            this.pdfWriter = PdfWriter.GetInstance(this.document, this.fileStream);
            this.document.Open();
            this.pdfByteContent = this.pdfWriter.DirectContent;

        }


        public void invoicePDFGeneration(MRLWMSC21.TPL.Invoice.InvoiceInfo invoiceInformation)
        {

            try
            {

                this.pageNo = 0;
                this.height = LandscapeDimension.yEnd; this.height -= LandscapeDimension.lineGap;
                string _FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_" + invoiceInformation.TenantID + ".pdf";
                _FileName = CommonFunctions.dateTimetoString(this.fromDateTime, "dd-MM-yyyy") + "_" + CommonFunctions.dateTimetoString(this.toDateTime, "dd-MM-yyyy") + "_" + DateTime.Now.ToString("dd-MM-yyyy") + "_" + invoiceInformation.TenantID + ".pdf";
                 
                this.fromDate = invoiceInformation.fromDate;
                this.toDate = invoiceInformation.toDate;
                this.generationDate = invoiceInformation.GeneratedDate;
                this.footerInfo = Convert.ToString(invoiceInformation.tenantData.TenantID).PadLeft(3, '0') + "-" + invoiceInformation.tenantData.InvoiceID + "[ "+this.fromDate+" to "+this.toDate+" ]";

                initInvoiceDays();
                initializePDFObject(_FileName);
                initializeBaseFont();

                logoAdd(imageFile);
                this.document.Add(addLogo);
                addFooterIno(this.footerInfo);

                if (invoiceInformation.tenantData != null)
                {
                    this.pdfByteContent.EndText();
                    initAddressFonts();
                    writeBillAuthor();
                    writeTenantInfo(invoiceInformation.tenantData);
                }

                //summary page generation...
                if (invoiceInformation.summary != null)
                {
                    //writeSummaryInfo(invoiceInformation.summary);
                    writeSummaryInfo(invoiceInformation.summary, invoiceInformation.InvoiceRates);
                }

                //transaction information
                if (invoiceInformation.transactionAccessorial != null && invoiceInformation.transactionAccessorial.Count >=1)
                {
                    newPageRequire(this.height, true);
                    writeTransactionInfo(invoiceInformation.transactionAccessorial);
                }

                //storage information
                if (invoiceInformation.storeCharges != null)
                {
                    newPageRequire(this.height, true);
                    writeStorageInfoInLandScape(invoiceInformation.storeCharges);
                    //writeStorageInfo(invoiceInformation.storeCharges);
                }
                this.pdfByteContent.EndText();
                this.document.Close();

            }
            catch (Exception ex)
            {
                  if (this.document != null) {
                    if (this.pdfByteContent != null) {
                        this.pdfByteContent.EndText();
                    }
                    this.document.Close(); }
                if (fileStream != null) { fileStream.Close(); }

                throw new INVTPLException("Invoice writing problem:" + ex.StackTrace);
            }

        }



        public void writeSummaryInfo(SummaryPage summary)
        {
            if (summary != null)
            {

                if (summary.rates != null)
                {

                    this.pdfByteContent.EndText();
                    initHeaderFonts();

                    this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, LandscapeDimension.rateLabels[0], LandscapeDimension.rateDimension[0], this.height, 0);
                    this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_CENTER, LandscapeDimension.rateLabels[1], LandscapeDimension.rateDimension[1], this.height, 0);
                    this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_CENTER, LandscapeDimension.rateLabels[2], LandscapeDimension.rateDimension[2], this.height, 0);
                    this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, LandscapeDimension.rateLabels[3], LandscapeDimension.rateDimension[3], this.height, 0);
                    this.height -= LandscapeDimension.bigBreak;

                    this.pdfByteContent.EndText();
                    initializeBaseFont();


                    foreach (TPLRate rate in summary.rates)
                    {
                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, rate.rateName, LandscapeDimension.rateDimension[0], this.height, 0);
                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Math.Round(rate.pricePerUnit, 2).ToString(), LandscapeDimension.rateDimension[1], this.height, 0);
                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Math.Round(rate.quantity, 2).ToString(), LandscapeDimension.rateDimension[2], this.height, 0);
                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Math.Round(rate.price, 2) + "  " + rate.currency, LandscapeDimension.rateDimension[3], this.height, 0);

                        this.height -= LandscapeDimension.lineGap;
                        newPageRequire(this.height, false);

                    }

                    this.height -= LandscapeDimension.lineGap;

                    if (summary.rateCurrency.Trim() == summary.currency.Trim())
                    {
                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "TOTAL ", LandscapeDimension.rateDimension[2], this.height, 0);
                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Math.Round(summary._totalRatesPrice, 2) + "  " + summary.rateCurrency, LandscapeDimension.rateDimension[3], this.height, 0);
                        this.height -= LandscapeDimension.lineGap;
                    }
                    else 
                    {
                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Math.Round(summary._totalRatesPrice, 2) + "  " + summary.rateCurrency, LandscapeDimension.rateDimension[3], this.height, 0);
                        this.height -= LandscapeDimension.lineGap;
                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "TOTAL ", LandscapeDimension.rateDimension[2], this.height, 0);
                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Math.Round(summary.price, 2) + "  " + summary.currency, LandscapeDimension.rateDimension[3], this.height, 0);
                        this.height -= LandscapeDimension.lineGap;
                        
                    }
                    

                    newPageRequire(this.height, false);
                }

            }

        }



        public void writeSummaryInfo(SummaryPage summary,IDictionary<int,InvoiceRate> invoiceRates)
        {
            
            if (summary != null)
            {

                if (summary.rates != null)
                {

                    this.pdfByteContent.EndText();
                    initHeaderFonts();

                    this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, LandscapeDimension.rateLabels[0], LandscapeDimension.rateDimension[0], this.height, 0);
                    this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_CENTER, LandscapeDimension.rateLabels[1], LandscapeDimension.rateDimension[1], this.height, 0);
                    this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, LandscapeDimension.rateLabels[2], LandscapeDimension.rateDimension[2], this.height, 0);
                    
                    //DISC
                    this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_CENTER, LandscapeDimension.rateLabels[3], LandscapeDimension.rateDimension[3], this.height, 0);

                    this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, LandscapeDimension.rateLabels[4], LandscapeDimension.rateDimension[4], this.height, 0);
                    this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, LandscapeDimension.rateLabels[5], LandscapeDimension.rateDimension[5], this.height, 0);
                    this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, LandscapeDimension.rateLabels[6], LandscapeDimension.rateDimension[6], this.height, 0);
                    this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, LandscapeDimension.rateLabels[7], LandscapeDimension.rateDimension[7], this.height, 0);

                    this.height -= LandscapeDimension.bigBreak;
                    this.pdfByteContent.EndText();
                    initializeBaseFont();
                    
                    for (int ind = 0; ind < invoiceRates.Count;ind++)
                    {
                    
                        var Item = invoiceRates.ElementAt(ind);
                        InvoiceRate rate = Item.Value;

                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, rate.rateName, LandscapeDimension.rateDimension[0], this.height, 0);
                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT,Math.Round(rate.unitPrice, 2).ToString(), LandscapeDimension.rateDimension[1], this.height, 0);
                        
                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, rate.QTY.ToString(), LandscapeDimension.rateDimension[2], this.height, 0);
                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Math.Round(rate.discount,2).ToString(), LandscapeDimension.rateDimension[3], this.height, 0);

                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Math.Round(rate.price, 2) + "  " + rate.currencyCode, LandscapeDimension.rateDimension[4], this.height, 0);


                        if (rate.taxInfo != null)
                        {
                            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, rate.taxInfo.taxCode + " " + Math.Round(rate.taxInfo.percentage, 2), LandscapeDimension.rateDimension[5], this.height, 0);
                            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Math.Round(rate.taxInfo.price, 2).ToString(), LandscapeDimension.rateDimension[6], this.height, 0);
                            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Math.Round(rate.taxInfo.price + rate.price, 2).ToString(), LandscapeDimension.rateDimension[7], this.height, 0);
                        }
                        else 
                        {
                            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Math.Round(rate.price, 2).ToString(), LandscapeDimension.rateDimension[7], this.height, 0);
                        }

                        this.height -= LandscapeDimension.lineGap;
                        newPageRequire(this.height, false);
                    }

                    this.height -= LandscapeDimension.lineGap;
                    if (summary.rateCurrency.Trim() == summary.currency.Trim())
                    {
                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "TOTAL ", LandscapeDimension.rateDimension[6], this.height, 0);
                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Math.Round(summary._totalRatesPrice, 2) + "  " + summary.rateCurrency, LandscapeDimension.rateDimension[7], this.height, 0);
                        this.height -= LandscapeDimension.lineGap;
                    }
                    else
                    {
                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Math.Round(summary._totalRatesPrice, 2) + "  " + summary.rateCurrency, LandscapeDimension.rateDimension[7], this.height, 0);
                        this.height -= LandscapeDimension.lineGap;
                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "TOTAL ", LandscapeDimension.rateDimension[6], this.height, 0);
                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Math.Round(summary.price, 2) + "  " + summary.currency, LandscapeDimension.rateDimension[7], this.height, 0);
                        this.height -= LandscapeDimension.lineGap;
                    }
                    
                    newPageRequire(this.height, false);
                }

            }

        }


        public void writeBillAuthor()
        {

            this.height -= LandscapeDimension.bigBreak;
            //this.height -= LandscapeDimension.bigBreak;
            //origin address
            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, LandscapeDimension.originAddress[0], LandscapeDimension.rateDimension[7], this.height, 0);
            this.height -= LandscapeDimension.lineGap;

            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, LandscapeDimension.originAddress[1], LandscapeDimension.rateDimension[7], this.height, 0);
            this.height -= LandscapeDimension.lineGap;

            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, LandscapeDimension.originAddress[2], LandscapeDimension.rateDimension[7], this.height, 0);
            this.height -= LandscapeDimension.lineGap;

            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, LandscapeDimension.originAddress[3], LandscapeDimension.rateDimension[7], this.height, 0);
            this.height -= LandscapeDimension.bigBreak;

            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Date: " + this.generationDate, LandscapeDimension.rateDimension[7], this.height, 0);
            this.height -= LandscapeDimension.bigBreak;
        }

        public void writeTenantInfo(TenantInfo tenantInfo)
        {

            if (tenantInfo != null)
            {

                TenantAddress address = tenantInfo.address;
                BankData bankInfo = tenantInfo.bankData;
                string InvoiceID = Convert.ToString(tenantInfo.TenantID).PadLeft(3,'0') + "-" + tenantInfo.InvoiceID;

                //tenant address
                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, tenantInfo.tenantName, LandscapeDimension.xStart, this.height, 0);
                this.height -= LandscapeDimension.lineGap;

                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, address.addressLine1, LandscapeDimension.xStart, this.height, 0);
                this.height -= LandscapeDimension.lineGap;

                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, address.addressLine2, LandscapeDimension.xStart, this.height, 0);
                this.height -= LandscapeDimension.lineGap;

                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, address.city + " " + address.zip, LandscapeDimension.xStart, this.height, 0);
                this.height -= LandscapeDimension.lineGap;

                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, address.countryName, LandscapeDimension.xStart, this.height, 0);
                this.height -= LandscapeDimension.lineGap;
                this.height -= LandscapeDimension.bigBreak;

                
                this.pdfByteContent.EndText();
                initHeaderFonts();

                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "INVOICE: " + InvoiceID, LandscapeDimension.xStart, this.height, 0);
                this.height -= LandscapeDimension.lineGap;

                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Period: " + this.fromDate + " - " + this.toDate, LandscapeDimension.xStart, this.height, 0);
                this.height -= LandscapeDimension.bigBreak;

                this.pdfByteContent.EndText();
                initializeBaseFont();

            }

        }


        public void initTableUnderDoc()
        {

            this.StoreTable = new PdfPTable(5);
            this.StoreTable.TotalWidth = 420;
            this.StoreTable.WidthPercentage = 65;
            this.StoreTable.LockedWidth = true;


            float[] widths = new float[] { 2f, 1f, 1f, 1f, 1f };
            this.StoreTable.SetWidths(widths);
            this.StoreTable.HorizontalAlignment = 1;

            this.StoreTable.SpacingBefore = 10f;
            this.StoreTable.SpacingAfter = 10f;

        }


                
        public void writeStorageInfoInLandScape(IDictionary<int,IDictionary<int, StorageCharge>> storageCharges)
        {

            if (storageCharges != null && storageCharges.Count >= 1)
            {

                this.pdfByteContent.EndText();
                initHeaderFonts();

                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Material Storage Information", LandscapeDimension._storedItemDimension[0], this.height, 0);
                this.height -= LandscapeDimension.lineGap;
                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, LandscapeDimension.DrawLine, LandscapeDimension._storedItemDimension[0], this.height, 0);


                this.pdfByteContent.EndText();
                initializeBaseFont();

                for (int week = 0; week < this.weeklyStorageList.Count; week++)
                {

                    newPageRequire(this.height, false);
                    this.pdfByteContent.EndText();
                    initHeaderFonts();

                    this.height -= LandscapeDimension.bigBreak;
                    this.height -= LandscapeDimension.lineGap;
                    
                    this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Week " + (week + 1), LandscapeDimension._storedItemDimension[0], this.height, 0);
                    
                    this.height -= LandscapeDimension.lineGap;
                    var _tempItem = this.weeklyStorageList.ElementAt(week);
                    IList<string> _dates = _tempItem.Value;
                    

                    for (int spaceUtilIndex = 0; spaceUtilIndex < storageCharges.Count; spaceUtilIndex++)
                    {

                        var Item2 = storageCharges.ElementAt(spaceUtilIndex);
                        int spaceUtilID = Item2.Key;
                        IDictionary<int, StorageCharge> _tempstorageCharges = Item2.Value;
                        int _tempX = 1;
                        {
                            this.pdfByteContent.EndText();
                            initHeaderFonts();

                            this.height -= LandscapeDimension.bigBreak;
                            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, LandscapeDimension._tempStorageLabels[spaceUtilID-1,0], LandscapeDimension._storedItemDimension[0], this.height, 0);

                            foreach (string _date in _dates)
                            {
                                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, _date, LandscapeDimension._storedItemDimension[_tempX], this.height, 0);
                                _tempX++;
                            }
                            
                            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, LandscapeDimension._tempStorageLabels[spaceUtilID-1,1], LandscapeDimension._storedItemDimension[8], this.height, 0);
                            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, LandscapeDimension._tempStorageLabels[spaceUtilID - 1, 2], LandscapeDimension._storedItemDimension[9], this.height, 0);
                            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, LandscapeDimension._tempStorageLabels[spaceUtilID - 1, 3], LandscapeDimension._storedItemDimension[10], this.height, 0);
                            
                            this.height -= LandscapeDimension.lineGap;

                            this.pdfByteContent.EndText();
                            initializeBaseFont();
                            newPageRequire(this.height, false);
                        }


                        for (int index = 0; index < _tempstorageCharges.Count; index++)
                        {
                            newPageRequire(this.height, false);
                            var item = _tempstorageCharges.ElementAt(index);
                            int MMID = item.Key;
                            StorageCharge SC = item.Value;
                            
                            if (SC != null && SC.charges != null)
                            {

                                this.height -= LandscapeDimension.lineGap;
                                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, SC.materialMasterCode.Trim(), LandscapeDimension._storedItemDimension[0], this.height, 0);
                                _tempX = 1;

                                decimal _MMWeekCharges = 0;
                                decimal _PricePerUnit = 0;
                                decimal unitMeasureValue = 0;
                                bool storeStatus = false;
               
                                switch (spaceUtilID)
                                {
                                    case 1: unitMeasureValue = SC.area; break;
                                    case 2: unitMeasureValue = SC.volume; break;
                                    case 3: unitMeasureValue = SC.weight; break;
                                    case 4: unitMeasureValue = SC.perBinQuantity; break;
                                    case 5: unitMeasureValue = SC.rentalConstantCharge; break;
                                    default: unitMeasureValue = 0.00m; break;
                                }

                                foreach (MaterialStorageCharge MSC in SC.materialStorageCharges)
                                {
                                    if (MSC.charges != null)
                                    {
                                        foreach (Charge c in MSC.charges)
                                        {

                                            if (_dates.Contains(MSC.materialAvailDate.Trim()) && !c.isUnitCost)
                                            {
                                                if (!storeStatus)
                                                {
                                                    this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, SC.materialMasterCode.Trim(), LandscapeDimension._storedItemDimension[0], this.height, 0);
                                                }

                                                _MMWeekCharges += c.costAfterDisc;
                                                _tempX = _dates.IndexOf(MSC.materialAvailDate.Trim()) + 1;
                                                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, MSC.quantity.ToString(), LandscapeDimension._storedItemDimension[_tempX], this.height, 0);
                                                _PricePerUnit = c.unitCost;
                                                storeStatus = true;
                                            }
                                        }
                                    }
                                }

                                if (storeStatus)
                                {
                                    this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Math.Round(_PricePerUnit, 2).ToString()+"  "+SC.rateUom, LandscapeDimension._storedItemDimension[8], this.height, 0);
                                    this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Math.Round(unitMeasureValue, 2).ToString()+"  "+SC.unitMeasure, LandscapeDimension._storedItemDimension[9], this.height, 0);
                                    this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Math.Round(_MMWeekCharges, 2).ToString(), LandscapeDimension._storedItemDimension[10], this.height, 0);
                                }
                                newPageRequire(this.height, false);
                            }

                        }

                    }

                }

            }

        }

        public void writeStorageInfo(IDictionary<int, StorageCharge> storageCharges)
        {

            if (storageCharges != null)
            {

                this.pdfByteContent.EndText();
                initHeaderFonts();

                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Material Storage Information", LandscapeDimension.storageColumnsDimension[0], this.height, 0);
                this.height -= LandscapeDimension.bigBreak;
                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, LandscapeDimension.DrawLine, LandscapeDimension.storageColumnsDimension[0], this.height, 0);

                this.pdfByteContent.EndText();
                initializeBaseFont();

                for (int index = 0; index < storageCharges.Count; index++)
                {
                    this.pdfByteContent.EndText();
                    initHeaderFonts();

                    var item = storageCharges.ElementAt(index);
                    int MMID = item.Key;
                    StorageCharge SC = item.Value;

                    this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, SC.materialName + "  #" + SC.materialMasterCode, LandscapeDimension.storageColumnsDimension[0], this.height, 0);
                    this.height -= LandscapeDimension.bigBreak;


                    if (SC != null && SC.charges != null)
                    {

                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, LandscapeDimension.storageLabels[0], LandscapeDimension.storageColumnsDimension[0], this.height, 0);
                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_CENTER, LandscapeDimension.storageLabels[1], LandscapeDimension.storageColumnsDimension[1], this.height, 0);
                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_CENTER, LandscapeDimension.storageLabels[2], LandscapeDimension.storageColumnsDimension[2], this.height, 0);
                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_CENTER, LandscapeDimension.storageLabels[3], LandscapeDimension.storageColumnsDimension[3], this.height, 0);
                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, LandscapeDimension.storageLabels[4], LandscapeDimension.storageColumnsDimension[4], this.height, 0);
                        this.height -= LandscapeDimension.lineGap;

                        this.pdfByteContent.EndText();
                        initializeBaseFont();


                        foreach (MaterialStorageCharge MSC in SC.materialStorageCharges)
                        {
                            if (MSC.charges != null)
                            {
                                foreach (Charge c in MSC.charges)
                                {
                                    if (c.isUnitCost)
                                    {
                                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "N/A", LandscapeDimension.storageColumnsDimension[1], this.height, 0);
                                    }
                                    else
                                    {
                                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, c.perBinCapacity.ToString(), LandscapeDimension.storageColumnsDimension[1], this.height, 0);
                                    }

                                    this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, MSC.materialAvailDate, LandscapeDimension.storageColumnsDimension[0], this.height, 0);
                                    this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Math.Round(c.unitCost, 2).ToString(), LandscapeDimension.storageColumnsDimension[2], this.height, 0);
                                    this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Math.Round(MSC.quantity, 2).ToString(), LandscapeDimension.storageColumnsDimension[3], this.height, 0);
                                    this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Math.Round(c.costAfterDisc, 2).ToString(), LandscapeDimension.storageColumnsDimension[4], this.height, 0);

                                    this.height -= LandscapeDimension.lineGap;
                                    newPageRequire(this.height, false);
                                }

                            }

                        }

                        this.pdfByteContent.EndText();
                        initHeaderFonts();


                        //label display
                        this.height -= LandscapeDimension.lineGap;
                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Rate Name", LandscapeDimension.storageColumnsDimension[0], this.height, 0);
                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Price", LandscapeDimension.storageColumnsDimension[2], this.height, 0);
                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Discount", LandscapeDimension.storageColumnsDimension[3], this.height, 0);
                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Total Price", LandscapeDimension.storageColumnsDimension[4], this.height, 0);
                        this.height -= LandscapeDimension.lineGap;

                        this.pdfByteContent.EndText();
                        initializeBaseFont();

                        //Material wise overview...
                        foreach (MaterialDetailedCharges MDC in SC.charges)
                        {
                            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, MDC.chargeName, LandscapeDimension.storageColumnsDimension[0], this.height, 0);
                            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Math.Round(MDC.price, 2).ToString(), LandscapeDimension.storageColumnsDimension[2], this.height, 0);
                            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, MDC.disc.ToString() + " %", LandscapeDimension.storageColumnsDimension[3], this.height, 0);
                            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Math.Round(MDC.discPrice, 2).ToString(), LandscapeDimension.storageColumnsDimension[4], this.height, 0);
                            this.height -= LandscapeDimension.bigBreak;
                            newPageRequire(this.height, false);
                        }

                    }

                }

            }

        }
        
        public void writeTransactionInfo(IDictionary<string, TransactionCharge> accessorialCharges)
        {
            if (accessorialCharges != null && accessorialCharges.Count >=1)
            {


                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "INBOUND TRANSACTIONS", LandscapeDimension.xStart, this.height, 0);
                this.height -= LandscapeDimension.bigBreak;
                this.height -= LandscapeDimension.lineGap;


                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, LandscapeDimension.TranLabels[0], LandscapeDimension.TranDimension[0], this.height, 0);
                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, LandscapeDimension.TranLabels[1], LandscapeDimension.TranDimension[1], this.height, 0);
                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_CENTER, LandscapeDimension.TranLabels[2], LandscapeDimension.TranDimension[2], this.height, 0);
                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, LandscapeDimension.TranLabels[3], LandscapeDimension.TranDimension[3], this.height, 0);
                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_CENTER, LandscapeDimension.TranLabels[4], LandscapeDimension.TranDimension[4], this.height, 0);
                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, LandscapeDimension.TranLabels[5], LandscapeDimension.TranDimension[5], this.height, 0);
                this.height -= LandscapeDimension.bigBreak;


                for (int index = 0; index < accessorialCharges.Count; index++)
                {
                    var item = accessorialCharges.ElementAt(index);
                    string transKey = item.Key;
                    string[] tran_key = transKey.Split('_');

                    if (Convert.ToInt16(tran_key[1]) != 1)
                    {
                        continue;
                    }

                    TransactionCharge tranCharge = item.Value;
                    this.height -= LandscapeDimension.lineGap;
                    String tranDate = tranCharge.date;
                    int tranID = tranCharge.transactionID;
                    string tranRefNo = tranCharge.transactionRefNumber;

                    if (tranCharge.charges != null)
                    {
                        foreach (AccessorialCharge ac in tranCharge.charges)
                        {
                            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, ac.activityName + " #" + tranRefNo, LandscapeDimension.TranDimension[0], this.height, 0);
                            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, tranDate, LandscapeDimension.TranDimension[1], this.height, 0);
                            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Math.Round(ac.unitCost, 2).ToString(), LandscapeDimension.TranDimension[2], this.height, 0);

                            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, ac.UOM, LandscapeDimension.TranDimension[3], this.height, 0);
                            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, ac.quantity.ToString(), LandscapeDimension.TranDimension[4], this.height, 0);

                            decimal temp = Math.Round((ac.quantity * ac.unitCost), 2);
                            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Math.Round(temp, 2).ToString() + " " + ac.currency, LandscapeDimension.TranDimension[5], this.height, 0);
                            this.height -= LandscapeDimension.lineGap;
                            newPageRequire(this.height, false);
                        }

                    }
                    this.height -= LandscapeDimension.bigBreak;

                }

                newPageRequire(this.height, false);
                this.height -= LandscapeDimension.bigBreak;
                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "OUTBOUND TRANSACTIONS", LandscapeDimension.xStart, this.height, 0);
                this.height -= LandscapeDimension.bigBreak;
                this.height -= LandscapeDimension.lineGap;

                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, LandscapeDimension.TranLabels[0], LandscapeDimension.TranDimension[0], this.height, 0);
                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, LandscapeDimension.TranLabels[1], LandscapeDimension.TranDimension[1], this.height, 0);
                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_CENTER, LandscapeDimension.TranLabels[2], LandscapeDimension.TranDimension[2], this.height, 0);
                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, LandscapeDimension.TranLabels[3], LandscapeDimension.TranDimension[3], this.height, 0);
                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_CENTER, LandscapeDimension.TranLabels[4], LandscapeDimension.TranDimension[4], this.height, 0);
                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, LandscapeDimension.TranLabels[5], LandscapeDimension.TranDimension[5], this.height, 0);
                this.height -= LandscapeDimension.bigBreak;

                //outbound transactions...
                for (int index = 0; index < accessorialCharges.Count; index++)
                {
                    var item = accessorialCharges.ElementAt(index);
                    string transKey = item.Key;
                    string[] tran_key = transKey.Split('_');

                    if (Convert.ToInt16(tran_key[1]) != 2)
                    {
                        continue;
                    }

                    TransactionCharge tranCharge = item.Value;
                    string tranDate = tranCharge.date;
                    this.height -= LandscapeDimension.lineGap;
                    int tranID = tranCharge.transactionID;
                    string tranRefNo = tranCharge.transactionRefNumber;

                    if (tranCharge.charges != null)
                    {
                        foreach (AccessorialCharge ac in tranCharge.charges)
                        {
                            this.height -= LandscapeDimension.lineGap;
                            newPageRequire(this.height, false);
                            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, ac.activityName + " #" + tranRefNo, LandscapeDimension.TranDimension[0], this.height, 0);
                            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, tranDate, LandscapeDimension.TranDimension[1], this.height, 0);
                            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Math.Round(ac.unitCost, 2).ToString(), LandscapeDimension.TranDimension[2], this.height, 0);

                            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, ac.UOM, LandscapeDimension.TranDimension[3], this.height, 0);
                            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, ac.quantity.ToString(), LandscapeDimension.TranDimension[4], this.height, 0);

                            decimal temp = ac.quantity * ac.unitCost;
                            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Math.Round(temp, 2).ToString() + " " + ac.currency, LandscapeDimension.TranDimension[5], this.height, 0);
                            
                        }

                    }
                    this.height -= LandscapeDimension.bigBreak;
                }

            }

        }


        public void logoAdd(string picLocation)
        {

            try
            {
                System.Drawing.Image image = System.Drawing.Image.FromFile(picLocation);
                addLogo = default(iTextSharp.text.Image);

                MemoryStream imageStream = new MemoryStream();
                image.Save(imageStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                addLogo = iTextSharp.text.Image.GetInstance(imageStream.ToArray());

                addLogo.ScaleToFit(100, 50);
                addLogo.Alignment = iTextSharp.text.Image.ALIGN_RIGHT;

            }
            catch (Exception ex)
            {
                throw new INVTPLException("exception while writing logo in pdf:" + ex.Message);
            }

        }

    }

    
    public class LandscapeDimension
    {

        public static int xStart = 60;
        public static int yStart = 60;
        public static int xEnd = 840;
        public static int yEnd = 580;
        public static int lineGap = 12;
        public static int bigBreak = 20;
        public static string DrawLine = "";

        public static string[] originAddress = { "Avya Inventrax Private Limited,", "# 10-50-60/4, Waltair Main Road,", "Visakhapatnam-530002,", "AP. India." };
        public static string[] storageLabels = { "Material Avail Date", "Bin Capacity", "Price Per Unit", "Quantity", "Price" };
        public static int[] storageColumnsDimension = { 60, 320, 450, 520, 600 };

        public static string[] rateLabels = { "TARIFF", "PRICE PER UNIT", "QTY.","DISC(%)", "PRICE","TAX TYPE(%)", "TAXATION INFO","TOTAL"};
        public static int[] rateDimension = { 60,330,405,435,520,600,690,770};


        public static string[] TranLabels = { "ACTIVITY NAME", "ACTIVITY DATE", "PRICE PER UNIT", "UOM", "QUANTITY", "PRICE" };
        public static int[] TranDimension = { 60, 310, 450, 520, 600, 700 };


        public static string[,] _tempStorageLabels = { 
                                                     
                                                     { "Part Number", "Unit Cost", "Material Area", "Price" },
                                                     { "Part Number", "Unit Cost", "Material Volume", "Price" },
                                                     { "Part Number", "Unit Cost", "Material Weight", "Price" },
                                                     { "Part Number", "Unit Cost", "Bin Capacity", "Price" },
                                                     { "Part Number", "Unit Cost", "Rental Charge", "Price" }

                                                     };

        public static int[] _storedItemDimension = { 50, 220, 280, 340, 400, 460, 520, 580, 640, 730, 790 };


    }



}