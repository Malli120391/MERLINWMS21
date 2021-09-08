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
    public partial class BillingReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "3PL/ Billing Report");
            }
        }
        [WebMethod]
        public static String getReport(string FromDate ,string ToDate,string Tenant,string Warehouse)
        {
            try {
                string sql = "EXEC sp_RPT_3PLBillingReportS @TenantID=" + Tenant + ",@StartDate='" + FromDate + "' ,@EndDate='" + ToDate + "',@WarehouseID="+ Warehouse;

                //sql += "   select te.TenantName,ta.Address1,ta.Address2,ta.City,ta.State,ta.ZIP from TPL_Tenant_AddressBook ta join TPL_Tenant te on ta.TenantID = te.TenantID where AddressBookTypeID = 2 and te.TenantID =" + Tenant;  commented by lalitha on 20/02/2019
                DataSet ds = new DataSet();
                ds = DB.GetDS(sql, false);
                return Newtonsoft.Json.JsonConvert.SerializeObject(ds);
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        [WebMethod]
        public static string generatePDFData(string FromDate, string ToDate, string Tenant, string Warehouse)
        {
            try
            {
                string sql = "EXEC sp_RPT_3PLBillingReportS @TenantID=" + Tenant + ",@StartDate='" + FromDate + "' ,@EndDate='" + ToDate + "',@WarehouseID=" + Warehouse;
                CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
                DataSet _dsResult = DB.GetDS(sql, false);
                List<BillingInboundUnload> _inbUnload = new List<BillingInboundUnload>();
                List<BillingInboundReceiving> _inbReceive = new List<BillingInboundReceiving>();

                List<BillingOutboundPicking> _outPicking = new List<BillingOutboundPicking>();
                List<BillingOutboundLoading> _outLoading = new List<BillingOutboundLoading>();

                List<StorageBilingData> _storageData = new List<StorageBilingData>();

                List<BillingTo> _billingTo = new List<BillingTo>();

                if (_dsResult.Tables.Count > 0)
                {
                    
                    foreach (DataRow _drinbUn in _dsResult.Tables[0].Rows)
                    {
                        BillingInboundUnload _inbunlData = new BillingInboundUnload
                        {
                            StoreRefNo = _drinbUn["StoreRefNo"].ToString(),
                            SupplierName = _drinbUn["SupplierName"].ToString(),
                            PONumber = _drinbUn["PONumber"].ToString(),
                            Receipt = _drinbUn["Receipt"].ToString(),
                            ServiceMaterial = _drinbUn["ServiceMaterial"].ToString(),
                            Quantity = _drinbUn["Quantity"].ToString(),
                            UoM = _drinbUn["UoM"].ToString(),
                            UnitCost = _drinbUn["UnitCost"].ToString(),
                            Tax = _drinbUn["Tax"].ToString(),
                            TotalCost = _drinbUn["TotalCost"].ToString(),
                           
                        };
                        _inbUnload.Add(_inbunlData);
                    }
                    foreach (DataRow _drinbRec in _dsResult.Tables[1].Rows)
                    {
                        BillingInboundReceiving _inbRecData = new BillingInboundReceiving
                        {
                            StoreRefNo = _drinbRec["StoreRefNo"].ToString(),
                            SupplierName = _drinbRec["SupplierName"].ToString(),
                            PONumber = _drinbRec["PONumber"].ToString(),
                            Receipt = _drinbRec["Receipt"].ToString(),
                            ServiceMaterial = _drinbRec["ServiceMaterial"].ToString(),
                            Quantity = _drinbRec["Quantity"].ToString(),
                            UoM = _drinbRec["UoM"].ToString(),
                            UnitCost = _drinbRec["UnitCost"].ToString(),
                            Tax = _drinbRec["Tax"].ToString(),
                            TotalCost = _drinbRec["TotalCost"].ToString(),
                        };
                        _inbReceive.Add(_inbRecData);
                    }

                    foreach (DataRow _drOutPick in _dsResult.Tables[2].Rows)
                    {
                        BillingOutboundPicking _outPickData = new BillingOutboundPicking
                        {
                            OBDNumber = _drOutPick["OBDNumber"].ToString(),
                            CustomerName = _drOutPick["CustomerName"].ToString(),
                            SONumber = _drOutPick["SONumber"].ToString(),
                            Delivery = _drOutPick["Delivery"].ToString(),
                            ServiceMaterial = _drOutPick["ServiceMaterial"].ToString(),
                            Quantity = _drOutPick["Quantity"].ToString(),
                            UoM = _drOutPick["UoM"].ToString(),
                            UnitCost = _drOutPick["UnitCost"].ToString(),
                            Tax = _drOutPick["Tax"].ToString(),
                            TotalCost = _drOutPick["TotalCost"].ToString(),
                        };
                        _outPicking.Add(_outPickData);
                    }

                    foreach (DataRow _droutLoad in _dsResult.Tables[3].Rows)
                    {
                        BillingOutboundLoading _outLoadData = new BillingOutboundLoading
                        {
                            OBDNumber = _droutLoad["OBDNumber"].ToString(),
                            CustomerName = _droutLoad["CustomerName"].ToString(),
                            SONumber = _droutLoad["SONumber"].ToString(),
                            Delivery = _droutLoad["Delivery"].ToString(),
                            ServiceMaterial = _droutLoad["ServiceMaterial"].ToString(),
                            Quantity = _droutLoad["Quantity"].ToString(),
                            UoM = _droutLoad["UoM"].ToString(),
                            UnitCost = _droutLoad["UnitCost"].ToString(),
                            Tax = _droutLoad["Tax"].ToString(),
                            TotalCost = _droutLoad["TotalCost"].ToString(),
                        };
                        _outLoading.Add(_outLoadData);
                    }

                    foreach (DataRow _drstrData in _dsResult.Tables[4].Rows)
                    {
                        StorageBilingData _strData = new StorageBilingData
                        {
                            MCode = _drstrData["MCode"].ToString(),
                            MDescription = _drstrData["MDescription"].ToString(),
                            UoM = _drstrData["UoM"].ToString(),
                            date = _drstrData["date"].ToString(),
                            AvailableQty = _drstrData["AvailableQty"].ToString(),
                            UnitCost = _drstrData["UnitCost"].ToString(),
                            TotalCost = _drstrData["TotalCost"].ToString(),
                        };
                        _storageData.Add(_strData);
                    }
                    foreach (DataRow _drBillingData in _dsResult.Tables[5].Rows)
                    {
                        BillingTo _billingData = new BillingTo
                        {
                            TenantName = _drBillingData["TenantName"].ToString(),
                            Address1 = _drBillingData["Address1"].ToString(),
                            Address2 = _drBillingData["Address2"].ToString(),
                            City = _drBillingData["City"].ToString(),
                            State = _drBillingData["State"].ToString(),
                            ZIP = _drBillingData["ZIP"].ToString(),
                        };
                        _billingTo.Add(_billingData);
                    }
                }

                mOutbound.UnloadPDFGenerator pdfGenerator = new mOutbound.UnloadPDFGenerator(_inbUnload, _inbReceive, _outPicking, _outLoading, _storageData, _billingTo);
                string logo = DB.GetSqlS("SELECT LogoPath AS S FROM GEN_Account WHERE AccountID=" + cp1.AccountID);
                String filePath = pdfGenerator.GenerateBillingPDFData(mOutbound.UnloadTemplate.Billing_SHEET, logo);
                return filePath;
            }
            catch (Exception ex)
            {
                return "Error : " + ex;
            }
        }

        public class BillingTo {
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

        public class StorageBilingData {
            public string MCode { set; get; }
            public string MDescription { set; get; }
            public string UoM { set; get; }
            public string date { set; get; }
            public string AvailableQty { set; get; }
            public string UnitCost { set; get; }
            public string TotalCost { set; get; }
        }

        public class ConsolidatedData {
            public string InboundUnloadTotal { get; set; }
            public string InboundReceiveTotal { get; set; }
            public string OutboundPickTotal { get; set; }
            public string OutboundloadTotal { get; set; }
            public string StorageBillingTotal { get; set; }
        }
    }
}