using MRLWMSC21.TPL.Invoice;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MRLWMSC21.TPL.InvoiceWriter
{
    public class TPLInvoiceWriter
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
        public iTextSharp.text.Image addLogo { get; set; }

        public string pdfFile;
        public string imageFile = @"D:\Inventrax369\Education\inventrax.png";
        public string fileLocation = @"D:\Inventrax369\Education\TPLInvoice\";

        public string fromDate;
        public string toDate;
        public string generationDate;


        public void newPageRequire(int height, bool forceDocCreation)
        {

            if (height <= TPLInvoiceDimension.yStart || forceDocCreation)
            {
                this.pdfByteContent.EndText();
                this.document.NewPage();
                this.height = TPLInvoiceDimension.yEnd;
                initializeBaseFont();
            }

        }

        
        public void initializeBaseFont()
        {
            BaseFont baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            this.pdfByteContent.SetColorFill(BaseColor.BLACK);
            this.pdfByteContent.SetFontAndSize(baseFont, 9);
            this.pdfByteContent.BeginText();
        }



        public TPLInvoiceWriter()
        {
            this.fileLocation = @"D:\Inventrax369\Education\TPLInvoice\";
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

        

        public TPLInvoiceWriter(string fileLocation)
        {
            this.fileLocation = fileLocation;
        }



        public void initializePDFObject(string fileName)
        {

            this.pdfFile = this.fileLocation + fileName;
            
            this.document = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 10, 10);
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

                this.height = TPLInvoiceDimension.yEnd; this.height -= TPLInvoiceDimension.lineGap;
                string _FileName = DateTime.Now.ToString("yyyy-MM-dd") + "_" + invoiceInformation.TenantID+".pdf";


                this.fromDate = invoiceInformation.fromDate;
                this.toDate = invoiceInformation.toDate;
                this.generationDate = invoiceInformation.GeneratedDate;

                initializePDFObject(_FileName);
                initializeBaseFont();
               
                logoAdd(imageFile);
                this.document.Add(addLogo);
                
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
                    writeSummaryInfo(invoiceInformation.summary);
                }

                //transaction information
                if (invoiceInformation.transactionAccessorial != null)
                {
                    newPageRequire(this.height, true);
                    writeTransactionInfo(invoiceInformation.transactionAccessorial);
                }

                //storage information
                if (invoiceInformation.storeCharges != null)
                {
                    newPageRequire(this.height, true);
                    //writeStorageInfo(invoiceInformation.storeCharges);
                }


                this.pdfByteContent.EndText();
                this.document.Close();

                //byte[] content = this.memoryStream.ToArray();
                //fileStream.Write(content, 0, (int)content.Length);
                //fileStream.Close();

            }
            catch (Exception ex)
            {
                //log writing required...
                if(this.document != null){this.document.Close();}
                if(fileStream != null){fileStream.Close(); }
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

                    this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, TPLInvoiceDimension.rateLabels[0], TPLInvoiceDimension.rateDimension[0], this.height, 0);
                    this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_CENTER, TPLInvoiceDimension.rateLabels[1], TPLInvoiceDimension.rateDimension[1], this.height, 0);
                    this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_CENTER, TPLInvoiceDimension.rateLabels[2], TPLInvoiceDimension.rateDimension[2], this.height, 0);
                    this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT,TPLInvoiceDimension.rateLabels[3], TPLInvoiceDimension.rateDimension[3], this.height, 0);
                    this.height -= TPLInvoiceDimension.bigBreak;
                    
                    this.pdfByteContent.EndText();
                    initializeBaseFont();


                    foreach (TPLRate rate in summary.rates)
                    {
                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, rate.rateName, TPLInvoiceDimension.rateDimension[0], this.height, 0);
                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Math.Round(rate.pricePerUnit,2).ToString(), TPLInvoiceDimension.rateDimension[1], this.height, 0);
                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Math.Round(rate.quantity,2).ToString(), TPLInvoiceDimension.rateDimension[2], this.height, 0);
                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Math.Round(rate.price,2) + "  " + rate.currency, TPLInvoiceDimension.rateDimension[3], this.height, 0);

                        this.height -= TPLInvoiceDimension.lineGap;
                        newPageRequire(this.height, false);

                    }

                    this.height -= TPLInvoiceDimension.lineGap;
                    this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Total ", TPLInvoiceDimension.rateDimension[2], this.height, 0);
                    this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Math.Round(summary.price,2) + "  " + summary.currency, TPLInvoiceDimension.rateDimension[3], this.height, 0);
                    this.height -= TPLInvoiceDimension.lineGap;
                    newPageRequire(this.height, false);
                }

            }

        }

        public void writeBillAuthor() 
        {

            this.height -= TPLInvoiceDimension.bigBreak;
            this.height -= TPLInvoiceDimension.bigBreak;

            //origin address
            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, TPLInvoiceDimension.originAddress[0], TPLInvoiceDimension.rateDimension[3], this.height, 0);
            this.height -= TPLInvoiceDimension.lineGap;

            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, TPLInvoiceDimension.originAddress[1], TPLInvoiceDimension.rateDimension[3], this.height, 0);
            this.height -= TPLInvoiceDimension.lineGap;

            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, TPLInvoiceDimension.originAddress[2], TPLInvoiceDimension.rateDimension[3], this.height, 0);
            this.height -= TPLInvoiceDimension.lineGap;

            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, TPLInvoiceDimension.originAddress[3], TPLInvoiceDimension.rateDimension[3], this.height, 0);
            this.height -= TPLInvoiceDimension.bigBreak;
        
            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Date: "+this.generationDate, TPLInvoiceDimension.rateDimension[3], this.height, 0);
            this.height -= TPLInvoiceDimension.bigBreak;
        }

        public void writeTenantInfo(TenantInfo tenantInfo)
        {

            if (tenantInfo != null)
            {

                TenantAddress address = tenantInfo.address;
                BankData bankInfo = tenantInfo.bankData;
                string InvoiceID = tenantInfo.TenantID + "-" + tenantInfo.InvoiceID;

                //tenant address
                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, tenantInfo.tenantName, TPLInvoiceDimension.xStart, this.height, 0);
                this.height -= TPLInvoiceDimension.lineGap;

                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, address.addressLine1, TPLInvoiceDimension.xStart, this.height, 0);
                this.height -= TPLInvoiceDimension.lineGap;

                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, address.addressLine2, TPLInvoiceDimension.xStart, this.height, 0);
                this.height -= TPLInvoiceDimension.lineGap;

                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, address.city + " " + address.zip, TPLInvoiceDimension.xStart, this.height, 0);
                this.height -= TPLInvoiceDimension.lineGap;

                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, address.countryName, TPLInvoiceDimension.xStart, this.height, 0);
                this.height -= TPLInvoiceDimension.lineGap;
                this.height -= TPLInvoiceDimension.bigBreak;


                
                this.pdfByteContent.EndText();
                initHeaderFonts();

                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "INVOICE: " + InvoiceID,TPLInvoiceDimension.xStart, this.height, 0);
                this.height -= TPLInvoiceDimension.lineGap;

                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Period: " + this.fromDate+" - "+this.toDate, TPLInvoiceDimension.xStart, this.height, 0);
                this.height -= TPLInvoiceDimension.bigBreak;

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



        public void writeStorageInfo(IDictionary<int, StorageCharge> storageCharges)
        {


            if (storageCharges != null)
            {

                this.pdfByteContent.EndText();
                initHeaderFonts();

                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Material Storage Information", TPLInvoiceDimension.storageColumnsDimension[0], this.height, 0);
                this.height -= TPLInvoiceDimension.bigBreak;
                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, TPLInvoiceDimension.DrawLine, TPLInvoiceDimension.storageColumnsDimension[0], this.height, 0);

                this.pdfByteContent.EndText();
                initializeBaseFont();

                for (int index = 0; index < storageCharges.Count; index++)
                {
                    this.pdfByteContent.EndText();
                    initHeaderFonts();

                    var item = storageCharges.ElementAt(index);
                    int MMID = item.Key;
                    StorageCharge SC = item.Value;

                    this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT,SC.materialName+"  #"+SC.materialMasterCode, TPLInvoiceDimension.storageColumnsDimension[0], this.height, 0);
                    this.height -= TPLInvoiceDimension.bigBreak;
                    
                    
                    if (SC != null && SC.charges != null)
                    {

                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, TPLInvoiceDimension.storageLabels[0], TPLInvoiceDimension.storageColumnsDimension[0], this.height, 0);
                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_CENTER, TPLInvoiceDimension.storageLabels[1], TPLInvoiceDimension.storageColumnsDimension[1], this.height, 0);
                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_CENTER, TPLInvoiceDimension.storageLabels[2], TPLInvoiceDimension.storageColumnsDimension[2], this.height, 0);
                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_CENTER, TPLInvoiceDimension.storageLabels[3], TPLInvoiceDimension.storageColumnsDimension[3], this.height, 0);
                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT,TPLInvoiceDimension.storageLabels[4], TPLInvoiceDimension.storageColumnsDimension[4], this.height, 0);
                        this.height -= TPLInvoiceDimension.lineGap;

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
                                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "N/A", TPLInvoiceDimension.storageColumnsDimension[1], this.height, 0);
                                    }
                                    else
                                    {
                                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, c.perBinCapacity.ToString(), TPLInvoiceDimension.storageColumnsDimension[1], this.height, 0);
                                    }

                                    this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, MSC.materialAvailDate, TPLInvoiceDimension.storageColumnsDimension[0], this.height, 0);
                                    this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Math.Round(c.unitCost,2).ToString(), TPLInvoiceDimension.storageColumnsDimension[2], this.height, 0);
                                    this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Math.Round(MSC.quantity,2).ToString(), TPLInvoiceDimension.storageColumnsDimension[3], this.height, 0);
                                    this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Math.Round(c.costAfterDisc,2).ToString(), TPLInvoiceDimension.storageColumnsDimension[4], this.height, 0);

                                    this.height -= TPLInvoiceDimension.lineGap;
                                    newPageRequire(this.height, false);
                                }

                            }

                        }
                        
                        this.pdfByteContent.EndText();
                        initHeaderFonts();


                        //label display
                        this.height -= TPLInvoiceDimension.lineGap;
                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Rate Name", TPLInvoiceDimension.storageColumnsDimension[0], this.height, 0);
                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Price", TPLInvoiceDimension.storageColumnsDimension[2], this.height, 0);
                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Discount", TPLInvoiceDimension.storageColumnsDimension[3], this.height, 0);
                        this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Total Price", TPLInvoiceDimension.storageColumnsDimension[4], this.height, 0);
                        this.height -= TPLInvoiceDimension.lineGap;

                        this.pdfByteContent.EndText();
                        initializeBaseFont();

                        //Material wise overview...
                        foreach (MaterialDetailedCharges MDC in SC.charges)
                        {
                            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, MDC.chargeName, TPLInvoiceDimension.storageColumnsDimension[0], this.height, 0);
                            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Math.Round(MDC.price,2).ToString(), TPLInvoiceDimension.storageColumnsDimension[2], this.height, 0);
                            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, MDC.disc.ToString() + " %", TPLInvoiceDimension.storageColumnsDimension[3], this.height, 0);
                            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Math.Round(MDC.discPrice,2).ToString(), TPLInvoiceDimension.storageColumnsDimension[4], this.height, 0);
                            this.height -= TPLInvoiceDimension.bigBreak;
                            newPageRequire(this.height, false);
                        }

                    }

                }

            }

        }


        public void writeTransactionInfo(IDictionary<string, TransactionCharge> accessorialCharges)
        {
            if (accessorialCharges != null)
            {

                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "INBOUND TRANSACTIONS", TPLInvoiceDimension.xStart, this.height, 0);
                this.height -= TPLInvoiceDimension.bigBreak;
                this.height -= TPLInvoiceDimension.lineGap;
                
                //inbound transactions..
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
                    this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "TRANSACTION: #" + tran_key[0] + " CHARGES", TPLInvoiceDimension.xStart, this.height, 0);
                    this.height -= TPLInvoiceDimension.lineGap;
                    

                    if (tranCharge.charges != null)
                    {
                        foreach (AccessorialCharge ac in tranCharge.charges)
                        {
                            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, ac.activityName, TPLInvoiceDimension.storageColumnsDimension[0], this.height, 0);
                            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT,Math.Round(ac.unitCost ,2).ToString(), TPLInvoiceDimension.storageColumnsDimension[2], this.height, 0);
                            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT,ac.UOM+"  "+Math.Round(ac.quantity,2).ToString(), TPLInvoiceDimension.storageColumnsDimension[3], this.height, 0);
                            
                            decimal temp = Math.Round(ac.quantity * ac.unitCost,2);
                            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, temp.ToString() + "  " + ac.currency, TPLInvoiceDimension.storageColumnsDimension[4], this.height, 0);
                            this.height -= TPLInvoiceDimension.lineGap;
                            newPageRequire(this.height, false);
                        }

                    }
                    this.height -= TPLInvoiceDimension.bigBreak;

                }

                this.height -= TPLInvoiceDimension.bigBreak;
                this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "OUTBOUND TRANSACTIONS", TPLInvoiceDimension.xStart, this.height, 0);
                this.height -= TPLInvoiceDimension.bigBreak;
                this.height -= TPLInvoiceDimension.lineGap;

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
                    this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "TRANSACTION: #" + tran_key[0] + " CHARGES", TPLInvoiceDimension.xStart, this.height, 0);
                    this.height -= TPLInvoiceDimension.lineGap;

                    if (tranCharge.charges != null)
                    {
                        foreach (AccessorialCharge ac in tranCharge.charges)
                        {
                            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, ac.activityName, TPLInvoiceDimension.storageColumnsDimension[0], this.height, 0);
                            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Math.Round(ac.unitCost,2).ToString(), TPLInvoiceDimension.storageColumnsDimension[2], this.height, 0);
                            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, ac.UOM+"  "+Math.Round(ac.quantity, 2).ToString(), TPLInvoiceDimension.storageColumnsDimension[3], this.height, 0);

                            decimal temp = ac.quantity * ac.unitCost;
                            this.pdfByteContent.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Math.Round(temp,2).ToString()+" "+ac.currency, TPLInvoiceDimension.storageColumnsDimension[4], this.height, 0);
                            this.height -= TPLInvoiceDimension.lineGap;
                            newPageRequire(this.height, false);
                        }

                    }
                    this.height -= TPLInvoiceDimension.bigBreak;
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

    public class TPLInvoiceDimension
    {

        public static int xStart = 60;
        public static int yStart = 100;
        public static int xEnd = 530;
        public static int yEnd = 750;
        public static int lineGap = 13;
        public static int bigBreak = 25;
        public static string DrawLine = "";


        public static string[] originAddress = { "Avya Inventrax Private Limited,", "# 10-50-60/4, Waltair Main Road,", "Visakhapatnam-530002,", "AP. India." };
        public static string[] storageLabels = { "Material Avail Date", "Bin Capacity", "Price Per Unit", "Quantity", "Price" };
        public static string[] rateLabels = { "Rate Name", "Price per unit", "Quantity", "Price" };

        public static int[] storageColumnsDimension = { 60,220, 350, 430, 530 };
        public static int[] rateDimension = { 60, 330, 430, 530 };

    }

}