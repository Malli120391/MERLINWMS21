using MRLWMSC21Common;
using MRLWMSC21.mOutbound.BL;
using MRLWMSC21.mOutbound;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

namespace MRLWMSC21.mOutbound
{
    public partial class OutboundSearch_New : System.Web.UI.Page
    {
        public CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
        public static CustomPrincipal cp;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Search Outbound");
            }
        }
        [WebMethod]
        public static string GetOBSearchData(OutboundSearch obj, int PageSize, int PageIndex)
        {
            CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

            try
            {


                StringBuilder sCmdupdatelineitem = new StringBuilder();
                sCmdupdatelineitem.Append(" EXEC dbo.sp_OBD_SearchOutbound_New");
                sCmdupdatelineitem.Append(" @StartDate=" + (obj.FromDate == "" ? "NULL" : DB.SQuote(obj.FromDate)));
                sCmdupdatelineitem.Append(",@EndDate=" + (obj.ToDate == "" ? "NULL" : DB.SQuote(obj.ToDate)));
                sCmdupdatelineitem.Append(",@StoreID=" + obj.Warehouseid);
                sCmdupdatelineitem.Append(", @DocumentTypeID=" + obj.DocumentTypeID);
                sCmdupdatelineitem.Append(", @DivisionIDs=NULL");
                sCmdupdatelineitem.Append(",@DeliveryStatusID='" + obj.DeliveryStatusID + "'");
                sCmdupdatelineitem.Append(",@WarehouseID=" + obj.Warehouseid);
                sCmdupdatelineitem.Append(",@SearchText=" + (obj.SearchText == "" ? "NULL" : DB.SQuote(obj.SearchText)));
                sCmdupdatelineitem.Append(", @SearchField=" + obj.CategoryID);
                sCmdupdatelineitem.Append(",@PageIndex=" + PageIndex);
                sCmdupdatelineitem.Append(",@PageSize=" + PageSize);
                sCmdupdatelineitem.Append(",@TenantID=" + obj.tenantid);
                sCmdupdatelineitem.Append(",@AccountID_New='" + cp.AccountID + "'");
                sCmdupdatelineitem.Append(",@UserTypeID_New=" + cp.UserTypeID);
                sCmdupdatelineitem.Append(",@TenantID_New=" + cp.TenantID);
                sCmdupdatelineitem.Append(",@UserID_New='" + cp.UserID + "'");
                sCmdupdatelineitem.Append(",@AWBNo=" + (obj.AWBNo == "" ? "NULL" : DB.SQuote(obj.AWBNo)));
                sCmdupdatelineitem.Append(",@DueDate=" + (obj.DueDate == "" ? "NULL" : DB.SQuote(obj.DueDate)));


                DataSet ds = DB.GetDS(sCmdupdatelineitem.ToString(), false);
                return JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        [WebMethod]
        public static string DownloadExcelForLog(OutboundSearch obj, string PageSize, string PageIndex)
        {
            CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                PageIndex = PageIndex == "1" || PageIndex == null ? "null" : DB.SQuote(PageIndex);
                PageSize = PageSize == "1" || PageSize == null ? "null" : DB.SQuote(PageSize);

                string fileName = "OutboundSearch" + DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second;
                //Added by prdeep p tp restrict tenant user 02-10-2020
                //if (cp2.UserTypeID == 3)
                StringBuilder sCmdupdatelineitem = new StringBuilder();
                sCmdupdatelineitem.Append(" EXEC dbo.sp_OBD_SearchOutbound_New ");
                sCmdupdatelineitem.Append("@StartDate=" + (obj.FromDate == "" ? "NULL" : DB.SQuote(obj.FromDate)));
                sCmdupdatelineitem.Append(",@EndDate=" + (obj.ToDate == "" ? "NULL" : DB.SQuote(obj.ToDate)));
                sCmdupdatelineitem.Append(",@StoreID=" + obj.Warehouseid);
                sCmdupdatelineitem.Append(", @DocumentTypeID=" + obj.DocumentTypeID);
                sCmdupdatelineitem.Append(", @DivisionIDs=NULL");

                sCmdupdatelineitem.Append(",@DeliveryStatusID='" + obj.DeliveryStatusID + "'");
                sCmdupdatelineitem.Append(",@WarehouseID=" + DB.SQuote(String.Join(",", cp.Warehouses)));
                sCmdupdatelineitem.Append(",@SearchText=" + (obj.SearchText == "" ? "NULL" : DB.SQuote(obj.SearchText)));
                sCmdupdatelineitem.Append(", @SearchField=" + obj.CategoryID);
                sCmdupdatelineitem.Append(",@PageIndex=" + PageIndex);
                sCmdupdatelineitem.Append(",@PageSize=" + PageSize);
                sCmdupdatelineitem.Append(",@TenantID=" + obj.tenantid);
                sCmdupdatelineitem.Append(",@AccountID_New='" + cp.AccountID + "'");
                sCmdupdatelineitem.Append(",@UserTypeID_New=" + cp.UserTypeID);
                sCmdupdatelineitem.Append(",@TenantID_New=" + cp.TenantID);
                sCmdupdatelineitem.Append(",@UserID_New='" + cp.UserID + "'");
                sCmdupdatelineitem.Append(",@AWBNo=" + (obj.AWBNo == "" ? "NULL" : DB.SQuote(obj.AWBNo)));
                sCmdupdatelineitem.Append(",@DueDate=" + (obj.DueDate == "" ? "NULL" : DB.SQuote(obj.DueDate)));


                DataSet ds = DB.GetDS(sCmdupdatelineitem.ToString(), false);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    MRLWMSC21Common.CommonLogic.ExportExcelDataForReports(ds.Tables[1], fileName, new List<int>(), "Outbound Search");
                    return fileName;
                }
                else
                {
                    return "No Data Found";
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [WebMethod]
        public static string searchOutboundData(string TenantID, string WarehouseID, string FromDate, string ToDate, string StatusID, string SearchCategory, string SearchText)
        {
            try
            {
                FromDate = FromDate == "" ? "null" : DB.SQuote(FromDate);
                ToDate = ToDate == "" ? "null" : DB.SQuote(ToDate);
                SearchText = SearchText == "" ? "null" : DB.SQuote(SearchText);
                string query = "EXEC [dbo].[sp_OBD_GetPackingSlipData] @StartDate=" + FromDate + ",@EndDate=" + ToDate + ",@DeliveryStatusID=" + StatusID + ",@WarehouseID=" + WarehouseID + ",@SearchText=" + SearchText + ",@SearchField=" + SearchCategory + ",@TenantID=" + TenantID;
                DataSet ds = DB.GetDS(query, false);
                return JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                return "Error : " + ex;
            }

        }
        [WebMethod]
        public static string generatePckingSlip(List<PackingPDFData> obj)
        {
            string path = "";
            try
            {
                //string strUploadPath = System.Web.HttpContext.Current.Server.MapPath("..\\ExcelData");
                //  OutBoundBL _obdData = new OutBoundBL();
                path = generatePDFData(obj);
                //return path;
            }
            catch (Exception ex)
            {
                path = "Error";
            }
            return path;
        }
        public static string generatePDFData(List<PackingPDFData> _data)
        {
            string FileName = "";
            try
            {
                string siteURL = System.Web.Configuration.WebConfigurationManager.AppSettings["siteURL"].ToString();
                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                    {
                        string pdfFile;
                        string subDirectory = "";
                        //Document pdfDoc = new Document(PageSize.A7, 10f, 10f, 10f, 0f);
                        var pgSize = new iTextSharp.text.Rectangle(230, 285);
                        Document pdfDoc = new iTextSharp.text.Document(pgSize, 9f, 9f, 9f, 0f);
                        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                        //PdfWriter writer = PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
                        FileName = "ShippingLabel.pdf";//_" + DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second + ".pdf";
                        string fileLocation = MRLWMSC21Common.CommonLogic.SafeMapPath("~/mOutbound/PackingSlip/");

                        if (File.Exists(fileLocation + FileName))
                        {
                            try
                            {
                                File.Delete(fileLocation + FileName);
                            }
                            catch (Exception ex)
                            {
                                //Do something
                            }
                        }

                        CreateDirectory(fileLocation);
                        fileLocation += subDirectory;
                        CreateDirectory(fileLocation);

                        pdfFile = fileLocation + FileName;

                        MemoryStream memoryStream = new MemoryStream();
                        FileStream fileStream = new FileStream(pdfFile, FileMode.Create);
                        PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, fileStream);
                        pdfDoc.Open();
                        if (_data.Count > 0)
                        {
                            foreach (var dt in _data)
                            {
                                DataSet ds = DB.GetDS("SELECT * FROM OBD_Outbound WHERE IsActive=1 AND IsDeleted=0 AND OutboundID =" + dt.OutboundID, false);
                                if (ds.Tables != null)
                                {
                                    string htmlData = "";
                                    string SONUmber = ""; string Address = ""; string SODate = ""; string Notes = ""; string CusName = ""; string MobileNo = ""; string ZipCode = "";
                                    int SOHeaderID = DB.GetSqlN("SELECT TOP 1 SOHeaderID AS N FROM OBD_Outbound_ORD_CustomerPO WHERE IsActive=1 AND IsDeleted=0 AND OutboundID=" + dt.OutboundID);
                                    if (SOHeaderID != 0)
                                    {
                                        DataSet soData = DB.GetDS("SELECT SONumber,FORMAT(SODate ,'dd-MMM-yyyy') SODate FROM ORD_SOHeader WHERE IsActive=1 AND IsDeleted=0 AND SOHeaderID =" + SOHeaderID, false);
                                        if (soData != null)
                                        {
                                            if (soData.Tables[0].Rows.Count > 0)
                                            {
                                                SONUmber = soData.Tables[0].Rows[0]["SONumber"].ToString();
                                                SODate = soData.Tables[0].Rows[0]["SODate"].ToString();
                                            }
                                        }
                                        int CustomerID = DB.GetSqlN("SELECT CustomerID AS N FROM ORD_SOHeader WHERE IsActive=1 AND IsDeleted=0 AND SOHeaderID =" + SOHeaderID);
                                        DataSet data = DB.GetDS("EXEC [sp_OBD_GetDeliveryNote_Header] @OutboundID=" + dt.OutboundID, false);
                                        if (data.Tables != null)
                                        {
                                            CusName = data.Tables[0].Rows[0]["CustomerName"].ToString() == "" || data.Tables[0].Rows[0]["CustomerName"].ToString() == null ? "" : data.Tables[0].Rows[0]["CustomerName"].ToString() + " - ";
                                            Address = data.Tables[0].Rows[0]["Address1"].ToString() == "" || data.Tables[0].Rows[0]["Address1"].ToString() == null ? "" : data.Tables[0].Rows[0]["Address1"].ToString();
                                            MobileNo = data.Tables[0].Rows[0]["Phone1"].ToString() == "" || data.Tables[0].Rows[0]["Phone1"].ToString() == null ? "" : data.Tables[0].Rows[0]["Phone1"].ToString();
                                            ZipCode = data.Tables[0].Rows[0]["Zip"].ToString() == "" || data.Tables[0].Rows[0]["Zip"].ToString() == null ? "" : "<b>Kode Pos :</b> " + data.Tables[0].Rows[0]["Zip"].ToString();
                                            Address = Address.Substring(0, Math.Min(200, Address.Length));
                                        }
                                    }
                                    string AWBNo = ""; string Date = ""; string Barcode = ""; string Courier = ""; string Qrcode = "";
                                    AWBNo = ds.Tables[0].Rows[0]["AWBNo"].ToString() == "" || ds.Tables[0].Rows[0]["AWBNo"].ToString() == null || ds.Tables[0].Rows[0]["AWBNo"].ToString() == "0" ? "" : ds.Tables[0].Rows[0]["AWBNo"].ToString();
                                    Notes = ds.Tables[0].Rows[0]["Notes"].ToString() == "" || ds.Tables[0].Rows[0]["Notes"].ToString() == null ? "" : " <b>Note :</b> " + ds.Tables[0].Rows[0]["Notes"].ToString();
                                    Date = SODate == "" || SODate == null ? "" : SODate; //ds.Tables[0].Rows[0]["DueDate"].ToString() == "" || ds.Tables[0].Rows[0]["DueDate"].ToString() == null ? "" : Convert.ToDateTime(ds.Tables[0].Rows[0]["DueDate"].ToString().Trim()).ToString("dd-MMM-yyyy");                                    
                                    Barcode = AWBNo == "" || AWBNo == null ? "" : "<img width='165px' height='30px' style='text-align:right;'src='" + siteURL + "/mInbound/Code39Handler.ashx?code=" + AWBNo + "'/> ";
                                    Courier = ds.Tables[0].Rows[0]["Courier"].ToString() == "" || ds.Tables[0].Rows[0]["Courier"].ToString() == null ? "" : ds.Tables[0].Rows[0]["Courier"].ToString();
                                    Qrcode = SONUmber == "" || SONUmber == null ? "" : "<img height='65px' src='" + siteURL + "/mInbound/Code39Handler.ashx?code=" + SONUmber.ToString() + "DeliveryNote'/>";

                                    Notes = Notes.Substring(0, Math.Min(100, Notes.Length)); CusName = CusName.Substring(0, Math.Min(53, CusName.Length));
                                    htmlData += "<table style='font-size:7px;color:#000000;' cellspacing='0' cellpadding='2'>";
                                    htmlData += "<tr><td style='text-align:center;font-size:13px;font-weight:bold;' bgcolor='#FFF' color='#000' border='1'>" + dt.TenantCode + "</td><td colspan='3' style='text-align:right;font-size:8px;' width='100%'>" + Barcode + "<b>AWB No. : </b>" + AWBNo + "</td></td></tr>";
                                    htmlData += "<tr><td style='text-align:right;font-size:8px;' colspan='4'>" + Date + " <br/> <b>" + Courier + "</b><p style='text-align:left;vertical-align:top;font-size:9px;'><b>Order : </b>" + SONUmber + "</p></td></tr>";
                                    //htmlData += "<tr><td style='text-align:left;vertical-align:top;font-size:9px;' colspan='4'><b>Order : </b>" + SONUmber + "</td></tr>";
                                    htmlData += "<tr><td style='text-align:left;' colspan='4'><b>Ship To :  </b> <br/> " + CusName + MobileNo + "<br/>" + Address + "<br/>" + ZipCode + "<br/>" + Notes + "</td></tr>";
                                    htmlData += "<tr><td style='text-align:left;' colspan='2'>" + Qrcode + "</td><td style='text-align:right !important;' align='right' colspan='2'><span style='color:#1a40ba !important;font-size:10px;'><b>Fulfilled By : </b></span/><p></p><br/><img width='85px' height='20px' src='" + siteURL + "/Images/inventrax.jpg'/></td></tr>";
                                    htmlData += "</table>";
                                    pdfDoc.NewPage();
                                    StringReader sr = new StringReader(htmlData.ToString());
                                    htmlparser.Parse(sr);
                                }
                            }
                        }
                        else
                        {
                            FileName = "No Data Found";
                        }
                        pdfDoc.Close();
                        fileStream.Close();

                        //HttpContext.Current.Response.ContentType = "application/pdf";
                        //HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".pdf");
                        //HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        //HttpContext.Current.Response.Write(pdfDoc);
                        //HttpContext.Current.Response.End();
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error : " + ex;
            }

            return FileName;
        }
        private static bool CreateDirectory(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            return true;
        }

        public class PackingPDFData
        {
            public string AWBNo { get; set; }
            public string Courier { get; set; }
            public string Priority { get; set; }
            public string DueDate { get; set; }
            public string Notes { get; set; }
            public string Address { get; set; }
            public int OutboundID { get; set; }
            public string TenantCode { get; set; }
        }

        public class OutboundSearch
        {

            public string FromDate { get; set; }
            public string ToDate { get; set; }
            public string tenantid { get; set; }
            public int Warehouseid { get; set; }
            public int DocumentTypeID { get; set; }
            public int DeliveryStatusID { get; set; }
            public string AWBNo { get; set; }

            public string SearchText { get; set; }
            public int CategoryID { get; set; }


            public string DueDate { get; set; }



        }
    }
}