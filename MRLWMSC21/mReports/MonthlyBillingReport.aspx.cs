using MRLWMSC21Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;


namespace MRLWMSC21.mReports
{
    public partial class MonthlyBillingReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "3PL/ Billing Report");
            }
        }
        [WebMethod]
        public static String getReport(string FromDate, string ToDate, string Tenant, string Warehouse)
        {
            try
            {
               

                string sql = "EXEC [dbo].[SP_RPT_TPL_MontlyBillingReport] @TenantID=" + Tenant + ",@StartDate='" + FromDate + "' ,@EndDate='" + ToDate + "',@WarehouseID=" + Warehouse;

                
                DataSet ds = new DataSet();
                ds = DB.GetDS(sql, false);
                return Newtonsoft.Json.JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [WebMethod]
        public static string generatePDFData(string FromDate, string ToDate, string Tenant, string Warehouse)
        {
            try
            {
                string sql = "EXEC [dbo].[SP_RPT_TPL_MontlyBillingReport] @TenantID=" + Tenant + ",@StartDate='" + FromDate + "' ,@EndDate='" + ToDate + "',@WarehouseID=" + Warehouse;

                CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
                DataSet _dsResult = DB.GetDS(sql, false);
                List<mOutbound.ITextSharp.CommonPDF.RateDetails> lstInboundRate = new List<mOutbound.ITextSharp.CommonPDF.RateDetails>();
                List<mOutbound.ITextSharp.CommonPDF.RateDetails> lstOutboundRate = new List<mOutbound.ITextSharp.CommonPDF.RateDetails>();
                List<mOutbound.ITextSharp.CommonPDF.RateDetails> lstInventory = new List<mOutbound.ITextSharp.CommonPDF.RateDetails>();
                List<mOutbound.ITextSharp.CommonPDF.RateDetails> lstOtherCharges = new List<mOutbound.ITextSharp.CommonPDF.RateDetails>();

                List<StorageBilingData> _storageData = new List<StorageBilingData>();

                List<BillingTo> _billingTo = new List<BillingTo>();
                mOutbound.ITextSharp.CommonPDF.Address _billingFromData = new mOutbound.ITextSharp.CommonPDF.Address();

                mOutbound.ITextSharp.CommonPDF.Address _billingToData = new mOutbound.ITextSharp.CommonPDF.Address();
                if (_dsResult.Tables.Count == 6)
                {

                    foreach (DataRow _drinbUn in _dsResult.Tables[0].Rows)
                    {
                        mOutbound.ITextSharp.CommonPDF.RateDetails _inbunlData = new mOutbound.ITextSharp.CommonPDF.RateDetails
                        {
                            ActivityNAme = _drinbUn["RateName"].ToString(),
                            Vaue =float.Parse(_drinbUn["Rate"].ToString())
                          
                        };
                        lstInboundRate.Add(_inbunlData);
                    }
                    foreach (DataRow _drinbUn in _dsResult.Tables[1].Rows)
                    {
                        mOutbound.ITextSharp.CommonPDF.RateDetails _inbunlData = new mOutbound.ITextSharp.CommonPDF.RateDetails
                        {
                            ActivityNAme = _drinbUn["RateName"].ToString(),
                            Vaue = float.Parse(_drinbUn["Rate"].ToString())

                        };
                        lstOutboundRate.Add(_inbunlData);
                    }

                    foreach (DataRow _drinbUn in _dsResult.Tables[2].Rows)
                    {
                        mOutbound.ITextSharp.CommonPDF.RateDetails _inbunlData = new mOutbound.ITextSharp.CommonPDF.RateDetails
                        {
                            ActivityNAme = _drinbUn["RateName"].ToString(),
                            Vaue = float.Parse(_drinbUn["Rate"].ToString())

                        };
                        lstInventory.Add(_inbunlData);
                    }
                    foreach (DataRow _drinbUn in _dsResult.Tables[3].Rows)
                    {
                        mOutbound.ITextSharp.CommonPDF.RateDetails _inbunlData = new mOutbound.ITextSharp.CommonPDF.RateDetails
                        {
                            ActivityNAme = _drinbUn["RateName"].ToString(),
                            Vaue = float.Parse(_drinbUn["Rate"].ToString())

                        };
                        lstOtherCharges.Add(_inbunlData);
                    }

                    
                    //from address
                  
                    foreach (DataRow _drBillingData in _dsResult.Tables[4].Rows)
                    {
                        _billingFromData.TenantName = _drBillingData["TenantName"].ToString();
                        _billingFromData.Address1 = _drBillingData["Address1"].ToString();
                        _billingFromData.Address2 = _drBillingData["Address2"].ToString();
                        _billingFromData.City = _drBillingData["City"].ToString();
                        _billingFromData.State = _drBillingData["State"].ToString();
                        _billingFromData.ZIP = _drBillingData["ZIP"].ToString();


                    }


                    //to address

                    foreach (DataRow _drBillingData in _dsResult.Tables[5].Rows)
                    {
                        _billingToData.TenantName = _drBillingData["TenantName"].ToString();
                        _billingToData.Address1 = _drBillingData["Address1"].ToString();
                        _billingToData.Address2 = _drBillingData["Address2"].ToString();
                        _billingToData.City = _drBillingData["City"].ToString();
                        _billingToData.State = _drBillingData["State"].ToString();
                        _billingToData.ZIP = _drBillingData["ZIP"].ToString();


                    }
                }

                mOutbound.UnloadPDFGenerator pdfGenerator = null;// new mOutbound.UnloadPDFGenerator(_inbUnload, _inbReceive, _outPicking, _outLoading, _storageData, _billingTo);
                string logo = DB.GetSqlS("SELECT LogoPath AS S FROM GEN_Account WHERE AccountID=" + cp1.AccountID);
                // String filePath = pdfGenerator.GenerateBillingPDFData(mOutbound.UnloadTemplate.Billing_SHEET, logo);
                string fileName = Tenant+""+ DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day +
                     DateTime.Now.Hour + "" + DateTime.Now.Minute +
                     DateTime.Now.Second;
                mOutbound.ITextSharp.CommonPDF comPDF = new mOutbound.ITextSharp.CommonPDF();
                
               string logopath= HttpContext.Current.Request.PhysicalApplicationPath+ @"TPL\AccountLogos\" + logo;
    
                string filePath = HttpContext.Current.Request.PhysicalApplicationPath + @"mOutbound\PackingSlip\" + fileName + ".pdf";
                //MRLWMSC21Common.CommonLogic.SafeMapPath(@"mOutbound\PackingSlip\" + fileName + ".pdf");
                comPDF.GenerateMonthlyBillingPDF(logopath,"", filePath, _billingFromData, _billingToData, lstInboundRate, lstOutboundRate, lstInventory, lstOtherCharges,FromDate,ToDate);
                return fileName+".pdf";
            }
            catch (Exception ex)
            {
                return "Error : " + ex;
            }
        }

        public class BillingTo
        {
            public string TenantName { set; get; }
            public string Address1 { set; get; }
            public string Address2 { set; get; }
            public string City { set; get; }
            public string State { set; get; }
            public string ZIP { set; get; }
        }
        public class BillingInboundUnload
        {
            public string StoreRefNo { set; get; }
            public string SupplierName { set; get; }
            public string PONumber { set; get; }
            public string Receipt { set; get; }
            public string ServiceMaterial { set; get; }
            public string UoM { set; get; }
            public string Quantity { set; get; }
            public string UnitCost { set; get; }
            public string Tax { set; get; }
            public string TotalCost { set; get; }
        }

        public class BillingInboundReceiving
        {
            public string StoreRefNo { set; get; }
            public string SupplierName { set; get; }
            public string PONumber { set; get; }
            public string Receipt { set; get; }
            public string ServiceMaterial { set; get; }
            public string UoM { set; get; }
            public string Quantity { set; get; }
            public string UnitCost { set; get; }
            public string Tax { set; get; }
            public string TotalCost { set; get; }
        }

        public class BillingOutboundPicking
        {
            public string OBDNumber { set; get; }
            public string CustomerName { set; get; }
            public string SONumber { set; get; }
            public string Delivery { set; get; }
            public string ServiceMaterial { set; get; }
            public string UoM { set; get; }
            public string Quantity { set; get; }
            public string UnitCost { set; get; }
            public string Tax { set; get; }
            public string TotalCost { set; get; }
        }

        public class BillingOutboundLoading
        {
            public string OBDNumber { set; get; }
            public string CustomerName { set; get; }
            public string SONumber { set; get; }
            public string Delivery { set; get; }
            public string ServiceMaterial { set; get; }
            public string UoM { set; get; }
            public string Quantity { set; get; }
            public string UnitCost { set; get; }
            public string Tax { set; get; }
            public string TotalCost { set; get; }
        }

        public class StorageBilingData
        {
            public string MCode { set; get; }
            public string MDescription { set; get; }
            public string UoM { set; get; }
            public string date { set; get; }
            public string AvailableQty { set; get; }
            public string UnitCost { set; get; }
            public string TotalCost { set; get; }
        }

        public class ConsolidatedData
        {
            public string InboundUnloadTotal { get; set; }
            public string InboundReceiveTotal { get; set; }
            public string OutboundPickTotal { get; set; }
            public string OutboundloadTotal { get; set; }
            public string StorageBillingTotal { get; set; }
        }
    }
}