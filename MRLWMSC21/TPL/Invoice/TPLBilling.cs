using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MRLWMSC21.TPL;
using MRLWMSC21.TPL.ActivityRate;
using MRLWMSC21.TPL.InvoiceWriter;
using System.IO;
using System.Net;
using System.Data;
using System.Text;
using MRLWMSC21Common;

/*
Author: Manikanta Challa.
Date: 17th June 2015
Invoice generation depends on allocated rates, transactions and its activity charges.
*/


namespace MRLWMSC21.TPL.Invoice
{

    public class TPLBilling : Invoice
    {

        public IDictionary<int, RateGroup> rateGroups;
        public IList<string> ErrorList;
        public IDictionary<string, TransactionCharge> transactionAccessorial;
        public InvoiceInfo invoiceInfo;

        public TPLBillHandler billHandler;
        public TenantInfo tenantDetails;

        public int TenantID;
        public IList<CalculatedStorageCharge> calculatedCharges;
        public IDictionary<int, IDictionary<int, StorageCharge>> storeCharges;

        public IDictionary<int, RateInfo> defaultRates;
        public SummaryPage summaryPage;
        public IDictionary<int, TaxationInfo> taxationInfo;

        public IList<RateInfo> activeRateList;
        public DateTime _FromDate;
        public DateTime _ToDate;
        public bool IsAccurateInvoice;
        public IList<RateInfo> tenantRateList;
        public ActivityRateHandler rateHandler;


        private CustomPrincipal customprinciple = HttpContext.Current.User as CustomPrincipal;

        public TPLBilling(int TenantID)
        {
            this.TenantID = TenantID;
            ErrorList = new List<string>();
            this.IsAccurateInvoice = true;
        }



        public TPLBilling()
        {
            this.billHandler = new TPLBillHandler();
            ErrorList = new List<string>();
            this.IsAccurateInvoice = true;
        }


        public void invoiceObjectsNullify()
        {
            storeCharges = null;
            calculatedCharges = null;
            transactionAccessorial = null;
            invoiceInfo = null;
            tenantDetails = null;
            activeRateList = null;
            billHandler = null;
            summaryPage = null;
            defaultRates = null;
            tenantRateList = null;
            taxationInfo = null;
            tenantRateList = new List<RateInfo>();
            this.billHandler = new TPLBillHandler();
        }

        public bool getInvoice(int tenantID, DateTime fromDate, DateTime toDate)
        {

            this.TenantID = tenantID;
            this._FromDate = fromDate;
            this._ToDate = toDate;
            bool functionalFlow = true;
            invoiceObjectsNullify();

            try
            {
                Object temp = null;
                temp = this.billHandler.getTenantDetails(this.TenantID);
                if (temp != null)
                {
                    this.tenantDetails = (TenantInfo)temp;
                }
            }
            catch (MRLWMSC21.TPL.INVTPLException ex)
            {
                functionalFlow = false;
                ErrorList.Add("error while getting tenant primary information");
                ex.writeExceptionLog(ex.StackTrace);
            }


            try
            {
                Object temp = null;
                temp = this.billHandler.getTaxationInfo("AP", fromDate, toDate);
                if (temp != null)
                {
                    this.taxationInfo = (IDictionary<int, TaxationInfo>)temp;
                }
            }
            catch (MRLWMSC21.TPL.INVTPLException ex)
            {
                functionalFlow = false;
                ErrorList.Add("error while getting service tax information");
                ex.writeExceptionLog(ex.StackTrace);
            }


            if (functionalFlow)
            {
                //getting allocated rates
                try
                {
                    Object temp = null;
                    temp = this.billHandler.getActiveRateListNew(this.TenantID, fromDate, toDate);
                    if (temp != null)
                    {
                        this.activeRateList = (IList<RateInfo>)temp;
                    }

                }
                catch (MRLWMSC21.TPL.INVTPLException ex)
                {
                    functionalFlow = false;
                    ErrorList.Add("error while getting active rates assigned to tenant");
                    ex.writeExceptionLog(ex.StackTrace);
                }

            }

            //Only Assigned Rates
            if (functionalFlow)
            {
                //getting allocated rates
                try
                {
                    Object temp = null;
                    temp = this.billHandler.getActiveRateListNew(this.TenantID, fromDate, toDate);
                    if (temp != null)
                    {
                        this.tenantRateList = (IList<RateInfo>)temp;
                    }

                }
                catch (MRLWMSC21.TPL.INVTPLException ex)
                {
                    functionalFlow = false;
                    ErrorList.Add("error while getting active rates assigned to tenant");
                    ex.writeExceptionLog(ex.StackTrace);
                }

            }

            //Stop Invoice Generation without assigned rates
            if (functionalFlow)
            {
                if (this.activeRateList.Count < 1)
                {
                    functionalFlow = false;
                }
            }


            if (functionalFlow)
            {
                Object temp = null;
                try
                {
                    temp = this.billHandler.getDefaultRateInformation(fromDate, toDate);
                    if (temp != null)
                    {
                        this.defaultRates = (IDictionary<int, RateInfo>)temp;
                    }
                }
                catch (MRLWMSC21.TPL.INVTPLException ex)
                {
                    functionalFlow = false;
                    ErrorList.Add("problem with getting default rates");
                    ex.writeExceptionLog(ex.StackTrace);
                }
            }


            //getting accessorial charges
            if (functionalFlow)
            {
                Object temp = null;
                try
                {
                    temp = this.billHandler.getListOfTransactions(tenantID, fromDate, toDate);
                    if (temp != null)
                    {
                        this.transactionAccessorial = (IDictionary<string, TransactionCharge>)temp;
                    }
                }
                catch (MRLWMSC21.TPL.INVTPLException ex)
                {
                    functionalFlow = false;
                    ErrorList.Add("problem while getting list of Inbound & Outbound transactions");
                    ex.writeExceptionLog(ex.StackTrace);
                }
            }


            //getting storage details
            if (functionalFlow)
            {

                Object temp = null;
                try
                {
                    temp = this.billHandler.getStoredItemsCharges(tenantID, fromDate, toDate);
                    if (temp != null)
                    {
                        this.storeCharges = (IDictionary<int, IDictionary<int, StorageCharge>>)temp;
                    }
                }
                catch (MRLWMSC21.TPL.INVTPLException ex)
                {
                    functionalFlow = false;
                    ErrorList.Add("problem while getting stored items list");
                    ex.writeExceptionLog(ex.StackTrace);
                }

            }



            if (functionalFlow)
            {

                try
                {
                    Object temp2 = null;
                    temp2 = this.billHandler.updateRatesWithDefaultRates(this.activeRateList, this.defaultRates);
                    if (temp2 != null)
                    {
                        this.activeRateList = (IList<RateInfo>)temp2;
                    }
                }
                catch (MRLWMSC21.TPL.INVTPLException ex)
                {
                    functionalFlow = false;
                    ErrorList.Add("problem while updating assigned rates with default rates");
                    ex.writeExceptionLog(ex.StackTrace);
                }
            }


            if (functionalFlow)
            {
                try
                {

                    if (this.storeCharges != null && this.activeRateList != null)
                    {
                        this.storeCharges = this.billHandler.applyChargesOnInhouseItems(this.activeRateList, this.storeCharges);
                        this.calculatedCharges = this.billHandler.actualStorageCharges;
                    }

                }
                catch (MRLWMSC21.TPL.INVTPLException ex)
                {
                    functionalFlow = false;
                    ErrorList.Add("problem while calculating storage charges");
                    ex.writeExceptionLog(ex.StackTrace);
                }

            }

            //apply default rates to transactions..
            if (functionalFlow)
            {
                Object temp = null;
                try
                {
                    temp = this.billHandler.getUpdatableTransactionsWithDefaultRates(this.transactionAccessorial, this.tenantDetails, this.defaultRates);
                    if (temp != null)
                    {
                        this.transactionAccessorial = (IDictionary<string, TransactionCharge>)temp;
                    }
                }
                catch (MRLWMSC21.TPL.INVTPLException ex)
                {
                    functionalFlow = false;
                    ErrorList.Add("problem while getting transactional charges");
                    ex.writeExceptionLog(ex.StackTrace);
                }
            }
            

            //summary page preparation
            if (functionalFlow)
            {
                try
                {
                    Object temp = null;
                    temp = this.billHandler.prepareSummaryPage(this.calculatedCharges, this.transactionAccessorial, this.tenantDetails, this.defaultRates);
                    if (temp != null)
                    {
                        this.summaryPage = (SummaryPage)temp;
                    }
                }
                catch (MRLWMSC21.TPL.INVTPLException ex)
                {
                    functionalFlow = false;
                    ErrorList.Add("problem while preparing summary page");
                    ex.writeExceptionLog(ex.StackTrace);
                }
            }


            try
            {
                //update with Miscellaneous Rates
                Object temp = null;
                temp = this.billHandler.getUpdatedSummaryWithMiscellaneousRates(this.summaryPage, this.activeRateList,this.tenantDetails.previousInvoice);
                if (temp != null)
                {
                    this.summaryPage = (SummaryPage)temp;
                }
            }
            catch (MRLWMSC21.TPL.INVTPLException ex)
            {
                functionalFlow = false;
                ErrorList.Add("problem while applying other charges");
                ex.writeExceptionLog(ex.StackTrace);
            }


            try
            {
                //update taxation information
                Object temp = null;
                if (this.taxationInfo != null)
                {
                    temp = this.billHandler.prepareInvoiceRatesWithVAT(this.summaryPage, this.taxationInfo);
                    if (temp != null)
                    {
                        this.summaryPage = (SummaryPage)temp;
                    }
                }
            }
            catch (MRLWMSC21.TPL.INVTPLException ex)
            {
                functionalFlow = false;
                ErrorList.Add("problem while applying other charges");
                ex.writeExceptionLog(ex.StackTrace);
            }


            //Invoice writing...
            if (functionalFlow)
            {
                try
                {
                    invoiceInfo = new InvoiceInfo();
                    invoiceInfo.fromDate = CommonFunctions.dateTimetoString(this._FromDate, "dd MMM yyyy");
                    invoiceInfo.toDate = CommonFunctions.dateTimetoString(this._ToDate, "dd MMM yyyy");
                    invoiceInfo.GeneratedDate = CommonFunctions.dateTimetoString(DateTime.Now, "dd MMM yyyy");

                    invoiceInfo.TenantID = TenantID;
                    if (this.calculatedCharges != null)
                    {
                        invoiceInfo.calculatedStoreCharges = this.calculatedCharges;
                    }

                    if (this.summaryPage != null)
                    {
                        invoiceInfo.summary = this.summaryPage;
                    }

                    if (this.storeCharges != null)
                    {
                        invoiceInfo.storeCharges = this.storeCharges;
                    }

                    if (this.tenantDetails != null)
                    {
                        invoiceInfo.tenantData = this.tenantDetails;
                    }

                    if (this.transactionAccessorial != null)
                    {
                        invoiceInfo.transactionAccessorial = this.transactionAccessorial;
                    }

                    if (this.billHandler.InvoiceRates != null)
                    {
                        invoiceInfo.InvoiceRates = this.billHandler.InvoiceRates;
                    }

                    TPLInvoiceLandscape invoiceWriter = new TPLInvoiceLandscape();
                    invoiceWriter.fromDateTime = this._FromDate;
                    invoiceWriter.toDateTime = this._ToDate;
                    invoiceWriter.generationDateTime = DateTime.Now;
                    invoiceWriter.invoicePDFGeneration(invoiceInfo);

                }

                catch (MRLWMSC21.TPL.INVTPLException ex)
                {
                    functionalFlow = false;
                    ErrorList.Add("problem while generating invoice with summary page");
                    ex.writeExceptionLog(ex.StackTrace);
                }

            }

            if (!functionalFlow)
            {
                this.IsAccurateInvoice = false;
                return false;
            }
            else
            {
                if (!this.billHandler.functionalFlow)
                {
                    updateErrorList(this.billHandler.ErrorList);
                    this.IsAccurateInvoice = false;
                }
                return true;
            }

        }

        public void updateErrorList(IList<string> errList)
        {
            foreach (string err in errList)
            {
                this.ErrorList.Add(err);
            }
        }

        public bool saveInvoice(int tenantID, DateTime fromDate, DateTime toDate)
        {
            bool invoiceSentStatus = true;
            try
            {
                if (getInvoice(tenantID, fromDate, toDate))
                {

                    this.tenantDetails.fromDateTime = fromDate;
                    this.tenantDetails.toDateTime = toDate;

                    if (this.IsAccurateInvoice)
                    {
                        string sourceFile = CommonFunctions.dateTimetoString(fromDate, "dd-MM-yyyy") + "_" + CommonFunctions.dateTimetoString(toDate, "dd-MM-yyyy") + "_" + DateTime.Now.ToString("dd-MM-yyyy") + "_" + tenantID + ".pdf";
                        string destinationFile = Convert.ToString(tenantID).PadLeft(4, '0') + "-" + this.tenantDetails.InvoiceID + ".pdf";
                        this.tenantDetails.InvoiceID = destinationFile;

                        if (copyInvoice("/TPL/InvoicePdf/", sourceFile, destinationFile))
                        {

                            Object temp = null;
                            temp = getInvoiceMailObj(destinationFile);

                            if (temp != null)
                            {
                                this.tenantDetails.mailInfo = (InvoiceMail)temp;
                                if (this.billHandler.holdInvoiceRates(this.tenantDetails, this.summaryPage, this.invoiceInfo.InvoiceRates))
                                {
                                    if (this.billHandler.sendInvoiceToTenant(this.tenantDetails, new NetworkCredential("nareshp@inventrax.com", "Pa$$w0rd")))
                                    {

                                        if (updateTenantActivityTariff(tenantID))
                                        {
                                            invoiceSentStatus = true;
                                        }
                                        else
                                        {
                                            ErrorList.Add("Problem while updating tenant activity rates");
                                            invoiceSentStatus = false;
                                        }
                                    }
                                }
                                else
                                {
                                    ErrorList.Add("Null Invoice or problem while holding invoice rates in DB");
                                    invoiceSentStatus = false;
                                }

                            }

                        }
                        else
                        {
                            invoiceSentStatus = false;
                        }
                    }
                    else
                    {
                        invoiceSentStatus = false;
                    }
                }
            }
            catch (INVTPLException ex)
            {
                invoiceSentStatus = false;
                ex.writeExceptionLog("exception while sending invoice to tenant:" + ex.StackTrace);
            }
            return invoiceSentStatus;
        }

        public bool cancelInvoice(int tenantID, DateTime fromDate, DateTime toDate)
        {
            return false;
        }

        public InvoiceMail getInvoiceMailObj(string destinationFile)
        {
            try
            {

                string selectSQL = "select TOP 1 Subject,Body from TPL_MailTemplate where IsActive=1";
                IDataReader mailBodyObj = DB.GetRS(selectSQL);
                string body = "";
                string subject = "";

                while (mailBodyObj.Read())
                {
                    body = DB.RSField(mailBodyObj, "Body").ToString();
                    subject = DB.RSField(mailBodyObj, "Subject").ToString();
                }
                mailBodyObj.Close();

                this.tenantDetails.FromDate = this._FromDate.ToString("dd MMM yyyy");
                this.tenantDetails.ToDate = this._ToDate.ToString("dd MMM yyyy");

                InvoiceMail mailInfo = new InvoiceMail();
                mailInfo.Subject = subject.Replace("<ID>", this.tenantDetails.InvoiceID);
                body = body.Replace("<TenantName>", this.tenantDetails.tenantName.Trim()).Replace("<From>", this.tenantDetails.FromDate).Replace("<To>", this.tenantDetails.ToDate);
                                
                mailInfo.ToAddress = new List<string>();
                mailInfo.ToAddress.Add("sureshy@inventrax.com");
                mailInfo.ToAddress.Add("manikanta.challa@inventrax.com");

                mailInfo.FromAddress = "mani.challa1990@gmail.com";
                mailInfo.Body = body;
                mailInfo.PdfLocation = System.Web.HttpContext.Current.Server.MapPath("~/" + "/TPL/InvoicePdf/") + destinationFile;
                return mailInfo;
            }
            catch (Exception e)
            {
                throw new INVTPLException("exception while initializing mail object: " + e.StackTrace);
            }

        }


        public bool copyInvoice(string relativeLocation, string sourceFile, string destinationFile)
        {
            try
            {
                string prefix = System.Web.HttpContext.Current.Server.MapPath("~/" + relativeLocation);
                File.Copy(prefix + sourceFile, prefix + destinationFile, true);
                File.Delete(prefix + sourceFile);
            }
            catch (IOException ex)
            {
                throw new INVTPLException("problem while copy the source" + ex.StackTrace);
            }
            return true;
        }

        public bool updateTenantActivityTariff(int TenantID)
        {
            string AllocationIDs = "";

            foreach (RateInfo info in tenantRateList)
            {
                AllocationIDs = AllocationIDs + info.rateAllocationID.ToString() + ",";
            }

            StringBuilder sbUpsertTenantActiveRates = new StringBuilder(2500);
            sbUpsertTenantActiveRates.Append("DECLARE @NewUpdateTenantActivityRateID int;   ");
            sbUpsertTenantActiveRates.Append("EXEC sp_TPL_UpsertTenantActiviteRates ");
            sbUpsertTenantActiveRates.Append("@AllocationIDs=" + DB.SQuote(AllocationIDs.Substring(0, AllocationIDs.Length - 1)));

            sbUpsertTenantActiveRates.Append(",@TenantID=" + TenantID);
            sbUpsertTenantActiveRates.Append(",@UpdatedBy=" + customprinciple.UserID.ToString());
            sbUpsertTenantActiveRates.Append(",@TenantActivityRateID=@NewUpdateTenantActivityRateID OUTPUT ;");
            sbUpsertTenantActiveRates.Append("select @NewUpdateTenantActivityRateID AS N");

            try
            {
                int Result = DB.GetSqlN(sbUpsertTenantActiveRates.ToString());
                if (Result == 1)
                {
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                ErrorList.Add("Problem while updating tenant activity rates");
                return false;
            }

        }

    }

}