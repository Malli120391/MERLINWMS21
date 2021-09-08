using MRLWMSC21Common;
using MRLWMSC21.TPL.Invoice;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRLWMSC21.TPL
{
    public partial class GSTInvoiceGeneration : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        public string fileLocation = @"../InvoicePdf/";
        public FileStream fileStream { get; set; }
        public int TenantId;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (CommonLogic.QueryString("msg") != "")
                    resetError(CommonLogic.QueryString("msg"), false);
                BuidGridData(BuidGridData());
                TenantId = cp.TenantID;
            }

        }
        private DataSet BuidGridData()
        {
            //DataSet ds = DB.GetDS(" EXEC [dbo].[sp_TPL_GetInvoiceData] ", false);
            DataSet ds = DB.GetDS(" EXEC [dbo].[sp_TPL_GetInvoiceQueueList] @AccountID_New=" + cp.AccountID + ",@TenantID_New=" + cp.TenantID, false);
            return ds;
        }

        private void BuidGridData(DataSet ds)
        {
            gvInvoice.DataSource = ds;
            gvInvoice.DataBind();
            ds.Dispose();
        }

        protected void gvInvoice_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvInvoice.EditIndex = -1;
            gvInvoice.PageIndex = e.NewPageIndex;

           // ViewState["PODetailsList"] = "EXEC dbo.sp_ORD_PODetailsList  @POHeaderID=" + ViewState["HeaderID"].ToString() + " ,@MCode=''";
            BuidGridData(BuidGridData());
        }

        protected void gvInvoice_RowCommand(object sender, GridViewCommandEventArgs e)
        {
             
            string repuee = "₹";
            int TanentID = 0;
            string FromDate = "";
            string Todatte = "";
            if (e.CommandName == "ViewBill")
            {
                string[] arguments = e.CommandArgument.ToString().Split('`');
                TanentID = Convert.ToInt16(arguments[0]);
                FromDate = (arguments[1]);
                Todatte = (arguments[2]);

                //Dummy data for Invoice (Bill).

                string Query = "[dbo].[sp_TPL_GST_GETINVOICEDETAILS] @TENANTID = " + TanentID + " , @FromDate = " + DB.SQuote(FromDate) + ", @Todtae=" + DB.SQuote(Todatte) + "";

                DataSet ds = DB.GetDS(Query, false);
                string companyName = "ASPSnippets";

                string sqlStatement = "EXEC [dbo].[sp_TPL_GetTenantPrimaryInfo] @TenantID = " + TanentID;
                DataSet DS_Tenant_Info = DB.GetDS(sqlStatement, false);
                DataTable T1 = DS_Tenant_Info.Tables[0];
                string TanentName = "";
                string TenantRegistraction = "";
                string Add1 = "";
                string Add2 = "";
                string EmailID = "";
                string Country = "";
                string Zip = "";
                string GSTNumber = "";
                string CountryMasterId = "";
                string TenantStateId = "";

                foreach (DataRow row in T1.Rows)
                {
                    TanentName = row["TenantName"].ToString();
                    TenantRegistraction = row["TenantRegistrationNo"].ToString();
                    Add1 = row["Address1"].ToString();
                    Add2 = row["Address2"].ToString();
                    EmailID = row["EMail"].ToString();
                    Country = row["CountryName"].ToString();
                    Zip = row["ZIP"].ToString();
                    GSTNumber = row["GSTNumber"].ToString();
                    CountryMasterId = row["CountryMasterID"].ToString();
                    TenantStateId = row["StateMasterID"].ToString();
                }


                this.fileLocation = System.Web.HttpContext.Current.Server.MapPath("~/" + "/TPL/InvoicePdf/");
                string filename = filename = "Invoice_" + FromDate + "to" + Todatte + "to" + TanentName + ".pdf";
                string URL = fileLocation + filename;
                string ExisitingInvoice = DB.GetSqlS("SELECT InvoiceNumber AS S FROM GEN_GSTInvoiceInfo WHERE TenantID = " + TanentID + " AND FromDate = '" + FromDate + "'AND ToDate = '" + Todatte + "'");
                string InvoiceNum = "";
                if (ExisitingInvoice == null || ExisitingInvoice == "")
                {

                    InvoiceNum = DB.GetSqlS("EXEC [dbo].[sp_TPL_GST_GETINVOICENUMBER]");
                    DB.ExecuteSQL("EXEC [dbo].[sp_TPL_GST_UPSERT_GST_INVOICE_INFO] @InvoiceNumber = '" + InvoiceNum + "',@TenantID = " + TanentID + " , @FromDate = '" + FromDate + "', @ToDate = '" + Todatte + "', @FileURL = '" + filename + "', @UserID = " + cp.UserID + "");
                }
                else
                {
                    InvoiceNum = ExisitingInvoice;
                }
                DataTable dt = new DataTable();
                dt = ds.Tables[0];


                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter hw = new HtmlTextWriter(sw))

                    {
                        if (CountryMasterId == "5")
                        {


                            string TaxComponent = "";
                            string GstQuery = "EXEC [dbo].[Get_RPT_Inovice_Taxation] @CountryID= " + CountryMasterId + ",@BuyerStateID=1" + ",@SellerStateID= " + TenantStateId + ",@CommodityID= 100101" + "";
                            DataSet gst = DB.GetDS(GstQuery, false);
                            if (gst.Tables[0].Rows.Count != 0)
                            {
                                foreach (DataRow row in gst.Tables[0].Rows)
                                {
                                    TaxComponent = row["ComponentName"].ToString();
                                }
                            }

                            if (TaxComponent == "SGST")
                            {
                                StringBuilder sb = new StringBuilder();

                                //Generate Invoice (Bill) Header.



                                sb.Append("<table style='font-weight: bold; font-size: 9%' border='0' width='100%' cellspacing='0' cellpadding='0'");
                                sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan = '2'><b>Invoice : " + InvoiceNum + "</b></td></tr>");
                                sb.Append("<tr><td align='right' style='background-color: #18B5F0' colspan = '2'><img src=" + Server.MapPath("Images/inventrax.png") + " /> <br/></td></tr>");
                                sb.Append("<tr><td align='right' style='background-color: #18B5F0' colspan = '2'>     Avya Inventrax Private Limited,<br/># ODC-4, First Floor, Phase 2,<br/>Tech Mahindra IT Park, Satyam Jn,<br/>Visakhapatnam, Andhra Pradesh,<br/>India- 530013</td></tr>");
                                sb.Append("<tr><td align='left' style='background-color: #18B5F0' colspan = '2'>" + TanentName + " <br/> " + Add1 + " " + Add2 + " <br/> GST Number :- " + GSTNumber + " <br/> " + EmailID + " <br/> " + Country + " " + Zip + " </td></tr>");
                                sb.Append("<tr><td colspan = '2'></td></tr>");

                                sb.Append("</td><td align = 'right'><b>Date: </b>");
                                sb.Append(DateTime.Now);
                                sb.Append(" </td></tr>");

                                sb.Append("</td></tr>");
                                sb.Append("</table>");
                                sb.Append("<br />");


                                sb.Append("<table border = '1' style='width:100%'>");
                                sb.Append("<tr style='font-weight: bold; font-size: 9%'>");
                                sb.Append("<th style='width=25%; font-size: 9%'> Item</th ><th style='width=7%; font-size: 9%'> HSN Code.</th ><th style='width=7%; font-size: 9%'> Qty.</th ><th style='width=7%; font-size: 9%'> UnitCost</th ><th style='width=7%;'>Rent</th><th style='width=7%;'>CGST(%)</th><th style='width=7%;'>CGST</th ><th style='width=7%;'>SGST(%)</th> <th style='width=7%;'>SGST</th> <th style='width=7%;'>Cess(%)</th><th style='width=7%;'>Cess</th><th style='width=7%;'>Total Rent</th>");

                                if (TaxComponent == "IGST")
                                {
                                    dt.Columns.Remove("CGST");
                                    dt.Columns.Remove("CGST_Amt");
                                    dt.Columns.Remove("SGST");
                                    dt.Columns.Remove("SGST_Amt");
                                    dt.Columns.Remove("UGST");
                                    dt.Columns.Remove("UGST_Amt");
                                    dt.Columns.Remove("TotalQty");
                                    dt.Columns.Remove("totalS");
                                    dt.Columns.Remove("totalU");

                                }

                                else if (TaxComponent == "SGST")
                                {
                                    dt.Columns.Remove("IGST");
                                    dt.Columns.Remove("IGST_Amt");
                                    dt.Columns.Remove("UGST");
                                    dt.Columns.Remove("UGST_Amt");
                                    dt.Columns.Remove("TotalQty");
                                    dt.Columns.Remove("totalI");
                                    dt.Columns.Remove("totalU");
                                    dt.Columns.Remove("VAT");
                                    dt.Columns.Remove("VAT_Amt");
                                    dt.Columns.Remove("totalV");


                                }

                                else if (TaxComponent == "UGST")
                                {
                                    dt.Columns.Remove("CGST");
                                    dt.Columns.Remove("CGST_Amt");
                                    dt.Columns.Remove("SGST");
                                    dt.Columns.Remove("SGST_Amt");
                                    dt.Columns.Remove("IGST");
                                    dt.Columns.Remove("IGST_Amt");
                                }
                                sb.Append("</tr>");
                                foreach (DataRow row in dt.Rows)
                                {
                                    sb.Append("<tr>");
                                    foreach (DataColumn column in dt.Columns)
                                    {
                                        sb.Append("<td style='font-size: 9%'>");
                                        sb.Append(row[column]);
                                        sb.Append("</td>");
                                    }
                                    sb.Append("</tr>");
                                }
                                sb.Append("<tr>");
                                sb.Append("<td style='font-size: 9%;font-weight: bold;'>Total</td> " +
                                    " <td style='font-size: 11%'></td>" +
                                    " <td style='font-size: 11%'></td>  " +
                                    "<td style='font-size: 9%'></td>" +
                                    " <td style='font-size: 9%'> " + dt.Compute("sum(Qty)", "") + "</td> " +
                                    "<td style='font-size: 9%'></td>" +
                                    " <td style='font-size: 9%'> " + dt.Compute("sum(CGST_Amt)", "") + "</td> " +
                                    "<td style='font-size: 9%'></td>" +
                                    " <td style='font-size: 9%'> " + dt.Compute("sum(SGST_Amt)", "") + "</td> " +
                                    " <td style='font-size: 9%'></td>" +
                                    //"<td style='font-size: 9%'></td> " +
                                    " <td style='font-size: 9%'> " + dt.Compute("sum(CESS_Amt)", "") + "</td> " +
                                    "<td style='font-size: 9%'> " + dt.Compute("sum(totalS)", "") + "</td>");
                                sb.Append("</tr>");
                                sb.Append("</table>");
                                string Query_InboundTra = "EXEC [dbo].[sp_TPL_GST_GETINVOICEDETAILS_INBOUND] @TENANTID =  " + TanentID + ", @FromDate = " + DB.SQuote(FromDate) + ", @Todtae = " + DB.SQuote(Todatte) + "";
                                DataSet InboundTra = DB.GetDS(Query_InboundTra, false);
                                DataTable InboundTable = InboundTra.Tables[0];
                                StringBuilder sb1 = new StringBuilder();
                                if (InboundTra.Tables[0].Rows.Count != 0 && InboundTra.Tables[0] != null)
                                {
                                    sb1.Append("<table border = '0' style='font-weight: bold; font-size: 9%;width:100%' >");
                                    sb1.Append("<tr><td style='background-color: #18B5F0' colspan = '2'> INBOUND TRANSACTIONS</td></tr>");
                                    sb1.Append("</table>");
                                    sb1.Append("<br />");
                                    sb1.Append("<table border = '1' style='width:100%'>");
                                    sb1.Append("<tr style='font-weight: bold; font-size: 9%'>");
                                    sb1.Append("<th style='width=25%; font-size: 7%;'> ACTIVITY NAME</th ><th style='width=7%;'> ACTIVITY DATE</th ><th style='width=7%;'> PRICE PER UNIT</th ><th style='width=7%;'>UOM</th><th style='width=7%;'>QUANTITY</th><th style='width=7%;'>PRICE</th>");
                                    sb1.Append("</tr>");
                                    foreach (DataRow row in InboundTable.Rows)
                                    {
                                        sb1.Append("<tr>");
                                        foreach (DataColumn column in InboundTable.Columns)
                                        {
                                            sb1.Append("<td style='font-size: 7%'>");
                                            sb1.Append(row[column]);
                                            sb1.Append("</td>");
                                        }
                                        sb1.Append("</tr>");
                                    }
                                    sb1.Append("</table>");
                                }
                                if (InboundTra.Tables[1].Rows.Count != 0 && InboundTra.Tables[1] != null)
                                {
                                    sb1.Append("<table border = '0' style='font-weight: bold; font-size: 9%;width:100%' >");
                                    sb1.Append("<tr><td style='background-color: #18B5F0' colspan = '2'> OUTBOUND TRANSACTIONS</td></tr>");
                                    sb1.Append("</table>");
                                    sb1.Append("<br />");
                                    sb1.Append("<table border = '1' style='width:100%'>");
                                    sb1.Append("<tr>");
                                    sb1.Append("<th style='width=25%; font-size: 7%;'> ACTIVITY NAME</th ><th style='width=7%;'> ACTIVITY DATE</th ><th style='width=7%;'> PRICE PER UNIT</th ><th style='width=7%;'>UOM</th><th style='width=7%;'>QUANTITY</th><th style='width=7%;'>PRICE</th>");
                                    sb1.Append("</tr>");
                                    foreach (DataRow row in InboundTable.Rows)
                                    {
                                        sb1.Append("<tr>");
                                        foreach (DataColumn column in InboundTable.Columns)
                                        {
                                            sb1.Append("<td style='font-size: 7%'>");
                                            sb1.Append(row[column]);
                                            sb1.Append("</td>");
                                        }
                                        sb1.Append("</tr>");
                                    }
                                    sb1.Append("</table>");
                                }

                                StringBuilder s2 = new StringBuilder();
                                s2.Append("<table border = '0' style='width:100%'>");
                                s2.Append("<tr><td style='background-color: #18B5F0' colspan = '2'> Material Storage Information </td></tr>");
                                s2.Append("</table>");

                                DataSet DS = DB.GetDS("EXEC [dbo].[sp_TPL_GetStoredItemsDetails_Weekly] @TenantID = " + TanentID + ", @StartDate = '" + FromDate + "',@EndDate = '" + Todatte + "'", false);

                                DataSet dsWeekTables = TPL.InvoiceWriter.StorageItems.GetWeekTables(DS);
                                int week = 1;
                                for (int i = 0; dsWeekTables.Tables.Count > i; i++)
                                {

                                    s2.Append("<table border = '0' style='font-weight: bold; font-size: 7%;width:100%' >");
                                    s2.Append("<tr>");
                                    s2.Append("<td >Week : " + week + "</td><br/>");
                                    s2.Append("</tr>");
                                    s2.Append("</table>");
                                    DataTable WeeKtable = null;
                                    WeeKtable = dsWeekTables.Tables[i];
                                    s2.Append("<table border = '0'>");
                                    s2.Append("<tr>");
                                    foreach (DataColumn coloum in WeeKtable.Columns)
                                    {

                                        s2.Append("<th style='font-weight: bold; font-size: 7%;'>");
                                        string colName = coloum.ColumnName.Contains("12:00") ? coloum.ColumnName.Split(new string[] { "12:00" }, StringSplitOptions.None)[0] : coloum.ColumnName;
                                        s2.Append(colName);
                                        s2.Append("</th>");
                                    }
                                    s2.Append("</tr>");
                                    foreach (DataRow row in WeeKtable.Rows)
                                    {
                                        s2.Append("<tr>");
                                        foreach (DataColumn column in WeeKtable.Columns)
                                        {
                                            s2.Append("<td style='font-size: 7%'>");
                                            s2.Append(row[column]);
                                            s2.Append("</td>");
                                        }
                                        s2.Append("</tr>");
                                    }
                                    week += 1;
                                    s2.Append("</td>");
                                    s2.Append("</tr></table>");

                                }

                                var header = iTextSharp.text.Image.GetInstance(Server.MapPath("Images/inventrax.png"));
                                header.SetAbsolutePosition(0, 730); // X and Y Accroding to need
                                                                    // var footer = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/letter-head-bottom.jpg"));
                                Document pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 0f);

                                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                                PdfWriter.GetInstance(pdfDoc, new FileStream(URL, FileMode.Create));
                                for (int a = 0; a < 3; a++)
                                {
                                    if (a == 1)
                                    {
                                        sb = null;
                                        sb = sb1;
                                    }
                                    else if (a == 2)
                                    {
                                        sb = null;
                                        sb = s2;
                                    }
                                    StringReader sr = new StringReader(sb.ToString());
                                    pdfDoc.Open();
                                    //pdfDoc.Add(header);
                                    htmlparser.Parse(sr);
                                    pdfDoc.NewPage();
                                }
                                pdfDoc.Close();
                                Response.ContentType = "application/pdf";
                                Response.AddHeader("content-disposition", "attachment;filename=Invoice" + FromDate + "to" + Todatte + "to" + TanentName + ".pdf");
                                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                Response.Write(pdfDoc);
                                Response.Redirect("DisplayInvoice.aspx?fid=" + filename + "");
                                Response.End();
                            }

                            else if (TaxComponent == "IGST")
                            {
                                StringBuilder sb = new StringBuilder();

                                //Generate Invoice (Bill) Header.



                                sb.Append("<table style='font-weight: bold; font-size: 9%' border='0' width='100%' cellspacing='0' cellpadding='0'");
                                sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan = '2'><b>Invoice : " + InvoiceNum + "</b></td></tr>");
                                sb.Append("<tr><td align='right' style='background-color: #18B5F0' colspan = '2'><img src=" + Server.MapPath("Images/inventrax.png") + " /> <br/></td></tr>");
                                sb.Append("<tr><td align='right' style='background-color: #18B5F0' colspan = '2'>     Avya Inventrax Private Limited,<br/># ODC-4, First Floor, Phase 2,<br/>Tech Mahindra IT Park, Satyam Jn,<br/>Visakhapatnam, Andhra Pradesh,<br/>India- 530013</td></tr>");
                                sb.Append("<tr><td align='left' style='background-color: #18B5F0' colspan = '2'>" + TanentName + " <br/> " + Add1 + " " + Add2 + " <br/> GST Number :- " + GSTNumber + " <br/> " + EmailID + " <br/> " + Country + " " + Zip + " </td></tr>");
                                sb.Append("<tr><td colspan = '2'></td></tr>");

                                sb.Append("</td><td align = 'right'><b>Date: </b>");
                                sb.Append(DateTime.Now);
                                sb.Append(" </td></tr>");

                                sb.Append("</td></tr>");
                                sb.Append("</table>");
                                sb.Append("<br />");


                                sb.Append("<table border = '1' style='width:100%'>");
                                sb.Append("<tr style='font-weight: bold; font-size: 9%'>");
                                sb.Append
                                    ("<th style='width=25%; font-size: 9%'> Item</th >" +
                                    "<th style='width=7%; font-size: 9%'> HSN Code.</th >" +
                                    "<th style='width=7%; font-size: 9%'> Qty.</th >" +
                                    "<th style='width=7%; font-size: 9%'> Unit Rent</th >" +
                                    "<th style='width=7%;'>Gross Value</th>" +
                                    "<th style='width=7%;'>IGST(%)</th>" +
                                    " <th style='width=7%;'>IGST</th> " +
                                    "<th style='width=7%;'>Cess(%)</th>" +
                                    "<th style='width=7%;'>Cess</th>" +
                                    "<th style='width=7%;'>Net Value</th>");

                                sb.Append("</tr>");
                                if (TaxComponent == "IGST")
                                {
                                    dt.Columns.Remove("CGST");
                                    dt.Columns.Remove("CGST_Amt");
                                    dt.Columns.Remove("SGST");
                                    dt.Columns.Remove("SGST_Amt");
                                    dt.Columns.Remove("UGST");
                                    dt.Columns.Remove("UGST_Amt");
                                    dt.Columns.Remove("TotalQty");
                                    dt.Columns.Remove("totalS");
                                    dt.Columns.Remove("totalU");
                                    dt.Columns.Remove("VAT");
                                    dt.Columns.Remove("VAT_Amt");
                                    dt.Columns.Remove("totalV");
                                }

                                else if (TaxComponent == "SGST")
                                {
                                    dt.Columns.Remove("IGST");
                                    dt.Columns.Remove("IGST_Amt");

                                }

                                else if (TaxComponent == "UGST")
                                {
                                    dt.Columns.Remove("CGST");
                                    dt.Columns.Remove("CGST_Amt");
                                    dt.Columns.Remove("SGST");
                                    dt.Columns.Remove("SGST_Amt");
                                    dt.Columns.Remove("IGST");
                                    dt.Columns.Remove("IGST_Amt");
                                }

                                foreach (DataRow row in dt.Rows)
                                {
                                    sb.Append("<tr>");
                                    foreach (DataColumn column in dt.Columns)
                                    {


                                        sb.Append("<td style='font-size: 9%'>");
                                        sb.Append(row[column]);
                                        sb.Append("</td>");
                                    }
                                    sb.Append("</tr>");
                                }
                                sb.Append("<tr>");
                                sb.Append
                                    ("<td style='font-size: 9%;font-weight: bold;'>Total</td>  " +
                                    "<td style='font-size: 11%'></td> " +
                                    "<td style='font-size: 11%'></td>  " +
                                    "<td style='font-size: 9%'></td>" +
                                    " <td style='font-size: 9%'> " + dt.Compute("sum(Qty)", "") + "</td> " +
                                    "<td style='font-size: 9%'></td>" +
                                    " <td style='font-size: 9%'> " + dt.Compute("sum(IGST_Amt)", "") + "</td> " +
                                    " <td style='font-size: 9%'></td>" +
                                    //"<td style='font-size: 9%'></td> " +
                                    " <td style='font-size: 9%'> " + dt.Compute("sum(CESS_Amt)", "") + "</td> " +
                                    "<td style='font-size: 9%'> " + dt.Compute("sum(totalI)", "") + "</td>");
                                sb.Append("</tr>");
                                sb.Append("</table>");
                                string Query_InboundTra = "EXEC [dbo].[sp_TPL_GST_GETINVOICEDETAILS_INBOUND] @TENANTID =  " + TanentID + ", @FromDate = " + DB.SQuote(FromDate) + ", @Todtae = " + DB.SQuote(Todatte) + "";
                                DataSet InboundTra = DB.GetDS(Query_InboundTra, false);
                                DataTable InboundTable = InboundTra.Tables[0];
                                StringBuilder sb1 = new StringBuilder();
                                if (InboundTra.Tables[0].Rows.Count != 0 && InboundTra.Tables[0] != null)
                                {
                                    sb1.Append("<table border = '0' style='font-weight: bold; font-size: 9%;width:100%' >");
                                    sb1.Append("<tr><td style='background-color: #18B5F0' colspan = '2'> INBOUND TRANSACTIONS</td></tr>");
                                    sb1.Append("</table>");
                                    sb1.Append("<br />");
                                    sb1.Append("<table border = '1' style='width:100%'>");
                                    sb1.Append("<tr style='font-weight: bold; font-size: 9%'>");
                                    sb1.Append("<th style='width=25%; font-size: 7%;'> ACTIVITY NAME</th ><th style='width=7%;'> ACTIVITY DATE</th ><th style='width=7%;'> PRICE PER UNIT</th ><th style='width=7%;'>UOM</th><th style='width=7%;'>QUANTITY</th><th style='width=7%;'>Gross Value</th>");
                                    sb1.Append("</tr>");
                                    foreach (DataRow row in InboundTable.Rows)
                                    {
                                        sb1.Append("<tr>");
                                        foreach (DataColumn column in InboundTable.Columns)
                                        {
                                            sb1.Append("<td style='font-size: 7%'>");
                                            sb1.Append(row[column]);
                                            sb1.Append("</td>");
                                        }
                                        sb1.Append("</tr>");
                                    }
                                    sb1.Append("</table>");
                                }
                                if (InboundTra.Tables[1].Rows.Count != 0 && InboundTra.Tables[1] != null)
                                {
                                    sb1.Append("<table border = '0' style='font-weight: bold; font-size: 9%;width:100%' >");
                                    sb1.Append("<tr><td style='background-color: #18B5F0' colspan = '2'> OUTBOUND TRANSACTIONS</td></tr>");
                                    sb1.Append("</table>");
                                    sb1.Append("<br />");
                                    sb1.Append("<table border = '1' style='width:100%'>");
                                    sb1.Append("<tr>");
                                    sb1.Append("<th style='width=25%; font-size: 7%;'> ACTIVITY NAME</th ><th style='width=7%;'> ACTIVITY DATE</th ><th style='width=7%;'> PRICE PER UNIT</th ><th style='width=7%;'>UOM</th><th style='width=7%;'>QUANTITY</th><th style='width=7%;'>Gross Value</th>");
                                    sb1.Append("</tr>");
                                    foreach (DataRow row in InboundTable.Rows)
                                    {
                                        sb1.Append("<tr>");
                                        foreach (DataColumn column in InboundTable.Columns)
                                        {
                                            sb1.Append("<td style='font-size: 7%'>");
                                            sb1.Append(row[column]);
                                            sb1.Append("</td>");
                                        }
                                        sb1.Append("</tr>");
                                    }
                                    sb1.Append("</table>");
                                }

                                StringBuilder s2 = new StringBuilder();
                                s2.Append("<table border = '0' style='width:100%'>");
                                s2.Append("<tr><td style='background-color: #18B5F0' colspan = '2'> Material Storage Information </td></tr>");
                                s2.Append("</table>");

                                DataSet DS = DB.GetDS("EXEC [dbo].[sp_TPL_GetStoredItemsDetails_Weekly] @TenantID = " + TanentID + ", @StartDate = '" + FromDate + "',@EndDate = '" + Todatte + "'", false);

                                if (DS != null && DS.Tables.Count > 0)
                                {
                                    DataSet dsWeekTables = TPL.InvoiceWriter.StorageItems.GetWeekTables(DS);
                                    int week = 1;
                                    for (int i = 0; dsWeekTables.Tables.Count > i; i++)
                                    {

                                        s2.Append("<table border = '0' style='font-weight: bold; font-size: 7%;width:100%' >");
                                        s2.Append("<tr>");
                                        s2.Append("<td >Week : " + week + "</td><br/>");
                                        s2.Append("</tr>");
                                        s2.Append("</table>");
                                        DataTable WeeKtable = null;
                                        WeeKtable = dsWeekTables.Tables[i];
                                        s2.Append("<table border = '0'>");
                                        s2.Append("<tr>");
                                        foreach (DataColumn coloum in WeeKtable.Columns)
                                        {

                                            s2.Append("<th style='font-weight: bold; font-size: 7%;'>");
                                            string colName = coloum.ColumnName.Contains("12:00") ? coloum.ColumnName.Split(new string[] { "12:00" }, StringSplitOptions.None)[0] : coloum.ColumnName;
                                            s2.Append(colName);
                                            s2.Append("</th>");
                                        }
                                        s2.Append("</tr>");
                                        foreach (DataRow row in WeeKtable.Rows)
                                        {
                                            s2.Append("<tr>");
                                            foreach (DataColumn column in WeeKtable.Columns)
                                            {
                                                s2.Append("<td style='font-size: 7%'>");
                                                s2.Append(row[column]);
                                                s2.Append("</td>");
                                            }
                                            s2.Append("</tr>");
                                        }
                                        week += 1;
                                        s2.Append("</td>");
                                        s2.Append("</tr></table>");

                                    }
                                }
                                var header = iTextSharp.text.Image.GetInstance(Server.MapPath("Images/inventrax.png"));
                                header.SetAbsolutePosition(0, 730); // X and Y Accroding to need
                                                                    // var footer = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/letter-head-bottom.jpg"));
                                Document pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 0f);

                                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                                PdfWriter.GetInstance(pdfDoc, new FileStream(URL, FileMode.Create));
                                for (int a = 0; a < 3; a++)
                                {
                                    if (a == 1)
                                    {
                                        sb = null;
                                        sb = sb1;
                                    }
                                    else if (a == 2)
                                    {
                                        sb = null;
                                        sb = s2;
                                    }
                                    StringReader sr = new StringReader(sb.ToString());
                                    pdfDoc.Open();
                                    //pdfDoc.Add(header);
                                    htmlparser.Parse(sr);
                                    pdfDoc.NewPage();
                                }
                                pdfDoc.Close();
                                Response.ContentType = "application/pdf";
                                Response.AddHeader("content-disposition", "attachment;filename=Invoice" + FromDate + "to" + Todatte + "to" + TanentName + ".pdf");
                                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                Response.Write(pdfDoc);
                                Response.Redirect("DisplayInvoice.aspx?fid=" + filename + "");
                                Response.End();
                            }

                            else if (TaxComponent == "UGST")
                            {
                                StringBuilder sb = new StringBuilder();

                                //Generate Invoice (Bill) Header.



                                sb.Append("<table style='font-weight: bold; font-size: 9%' border='0' width='100%' cellspacing='0' cellpadding='0'");
                                sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan = '2'><b>Invoice : " + InvoiceNum + "</b></td></tr>");
                                sb.Append("<tr><td align='right' style='background-color: #18B5F0' colspan = '2'><img src=" + Server.MapPath("Images/inventrax.png") + " /> <br/></td></tr>");
                                sb.Append("<tr><td align='right' style='background-color: #18B5F0' colspan = '2'>     Avya Inventrax Private Limited,<br/># ODC-4, First Floor, Phase 2,<br/>Tech Mahindra IT Park, Satyam Jn,<br/>Visakhapatnam, Andhra Pradesh,<br/>India- 530013</td></tr>");
                                sb.Append("<tr><td align='left' style='background-color: #18B5F0' colspan = '2'>" + TanentName + " <br/> " + Add1 + " " + Add2 + " <br/> GST Number :- " + GSTNumber + " <br/> " + EmailID + " <br/> " + Country + " " + Zip + " </td></tr>");
                                sb.Append("<tr><td colspan = '2'></td></tr>");

                                sb.Append("</td><td align = 'right'><b>Date: </b>");
                                sb.Append(DateTime.Now);
                                sb.Append(" </td></tr>");

                                sb.Append("</td></tr>");
                                sb.Append("</table>");
                                sb.Append("<br />");


                                sb.Append("<table border = '1' style='width:100%'>");
                                sb.Append("<tr style='font-weight: bold; font-size: 9%'>");
                                sb.Append
                                    ("<th style='width=25%; font-size: 9%'> Item</th >" +
                                    "<th style='width=7%; font-size: 9%'> HSN Code.</th >" +
                                    "<th style='width=7%; font-size: 9%'> Qty.</th >" +
                                    "<th style='width=7%; font-size: 9%'> UnitCost</th >" +
                                    "<th style='width=7%;'>Rent</th>" +
                                    "<th style='width=7%;'>UGST(%)</th>" +
                                    " <th style='width=7%;'>UGST</th> " +
                                    "<th style='width=7%;'>Cess(%)</th>" +
                                    "<th style='width=7%;'>Cess</th>" +
                                    "<th style='width=7%;'>Total Rent</th>");

                                sb.Append("</tr>");
                                if (TaxComponent == "IGST")
                                {
                                    dt.Columns.Remove("CGST");
                                    dt.Columns.Remove("CGST_Amt");
                                    dt.Columns.Remove("SGST");
                                    dt.Columns.Remove("SGST_Amt");
                                }

                                else if (TaxComponent == "SGST")
                                {
                                    dt.Columns.Remove("IGST");
                                    dt.Columns.Remove("IGST_Amt");

                                }

                                else if (TaxComponent == "UGST")
                                {
                                    dt.Columns.Remove("CGST");
                                    dt.Columns.Remove("CGST_Amt");
                                    dt.Columns.Remove("SGST");
                                    dt.Columns.Remove("SGST_Amt");
                                    dt.Columns.Remove("IGST");
                                    dt.Columns.Remove("IGST_Amt");
                                    dt.Columns.Remove("TotalQty");
                                    dt.Columns.Remove("totalS");
                                    dt.Columns.Remove("totalI");
                                    dt.Columns.Remove("VAT");
                                    dt.Columns.Remove("VAT_Amt");
                                    dt.Columns.Remove("totalV");

                                }

                                foreach (DataRow row in dt.Rows)
                                {
                                    sb.Append("<tr>");
                                    foreach (DataColumn column in dt.Columns)
                                    {


                                        sb.Append("<td style='font-size: 9%'>");
                                        sb.Append(row[column]);
                                        sb.Append("</td>");
                                    }
                                    sb.Append("</tr>");
                                }
                                sb.Append("<tr>");
                                sb.Append
                                    ("<td style='font-size: 9%;font-weight: bold;'>Total</td>  " +
                                    "<td style='font-size: 11%'></td> " +
                                    "<td style='font-size: 11%'></td>  " +
                                    "<td style='font-size: 9%'></td>" +
                                    " <td style='font-size: 9%'> " + dt.Compute("sum(Qty)", "") + "</td> " +
                                    "<td style='font-size: 9%'></td>" +
                                    " <td style='font-size: 9%'> " + dt.Compute("sum(UGST_Amt)", "") + "</td> " +
                                    " <td style='font-size: 9%'></td>" +
                                    //"<td style='font-size: 9%'></td> " +
                                    " <td style='font-size: 9%'> " + dt.Compute("sum(CESS_Amt)", "") + "</td> ");
                                //"<td style='font-size: 9%'> " + dt.Compute("sum(TotalQty)", "") + "</td>");
                                sb.Append("</tr>");
                                sb.Append("</table>");
                                string Query_InboundTra = "EXEC [dbo].[sp_TPL_GST_GETINVOICEDETAILS_INBOUND] @TENANTID =  " + TanentID + ", @FromDate = " + DB.SQuote(FromDate) + ", @Todtae = " + DB.SQuote(Todatte) + "";
                                DataSet InboundTra = DB.GetDS(Query_InboundTra, false);
                                DataTable InboundTable = InboundTra.Tables[0];
                                StringBuilder sb1 = new StringBuilder();
                                if (InboundTra.Tables[0].Rows.Count != 0 && InboundTra.Tables[0] != null)
                                {
                                    sb1.Append("<table border = '0' style='font-weight: bold; font-size: 9%;width:100%' >");
                                    sb1.Append("<tr><td style='background-color: #18B5F0' colspan = '2'> INBOUND TRANSACTIONS</td></tr>");
                                    sb1.Append("</table>");
                                    sb1.Append("<br />");
                                    sb1.Append("<table border = '1' style='width:100%'>");
                                    sb1.Append("<tr style='font-weight: bold; font-size: 9%'>");
                                    sb1.Append("<th style='width=25%; font-size: 7%;'> ACTIVITY NAME</th ><th style='width=7%;'> ACTIVITY DATE</th ><th style='width=7%;'> PRICE PER UNIT</th ><th style='width=7%;'>UOM</th><th style='width=7%;'>QUANTITY</th><th style='width=7%;'>PRICE</th>");
                                    sb1.Append("</tr>");
                                    foreach (DataRow row in InboundTable.Rows)
                                    {
                                        sb1.Append("<tr>");
                                        foreach (DataColumn column in InboundTable.Columns)
                                        {
                                            sb1.Append("<td style='font-size: 7%'>");
                                            sb1.Append(row[column]);
                                            sb1.Append("</td>");
                                        }
                                        sb1.Append("</tr>");
                                    }
                                    sb1.Append("</table>");
                                }
                                if (InboundTra.Tables[1].Rows.Count != 0 && InboundTra.Tables[1] != null)
                                {
                                    sb1.Append("<table border = '0' style='font-weight: bold; font-size: 9%;width:100%' >");
                                    sb1.Append("<tr><td style='background-color: #18B5F0' colspan = '2'> OUTBOUND TRANSACTIONS</td></tr>");
                                    sb1.Append("</table>");
                                    sb1.Append("<br />");
                                    sb1.Append("<table border = '1' style='width:100%'>");
                                    sb1.Append("<tr>");
                                    sb1.Append("<th style='width=25%; font-size: 7%;'> ACTIVITY NAME</th ><th style='width=7%;'> ACTIVITY DATE</th ><th style='width=7%;'> PRICE PER UNIT</th ><th style='width=7%;'>UOM</th><th style='width=7%;'>QUANTITY</th><th style='width=7%;'>PRICE</th>");
                                    sb1.Append("</tr>");
                                    foreach (DataRow row in InboundTable.Rows)
                                    {
                                        sb1.Append("<tr>");
                                        foreach (DataColumn column in InboundTable.Columns)
                                        {
                                            sb1.Append("<td style='font-size: 7%'>");
                                            sb1.Append(row[column]);
                                            sb1.Append("</td>");
                                        }
                                        sb1.Append("</tr>");
                                    }
                                    sb1.Append("</table>");
                                }

                                StringBuilder s2 = new StringBuilder();
                                s2.Append("<table border = '0' style='width:100%'>");
                                s2.Append("<tr><td style='background-color: #18B5F0' colspan = '2'> Material Storage Information </td></tr>");
                                s2.Append("</table>");

                                DataSet DS = DB.GetDS("EXEC [dbo].[sp_TPL_GetStoredItemsDetails_Weekly] @TenantID = " + TanentID + ", @StartDate = '" + FromDate + "',@EndDate = '" + Todatte + "'", false);

                                DataSet dsWeekTables = TPL.InvoiceWriter.StorageItems.GetWeekTables(DS);
                                int week = 1;
                                for (int i = 0; dsWeekTables.Tables.Count > i; i++)
                                {

                                    s2.Append("<table border = '0' style='font-weight: bold; font-size: 7%;width:100%' >");
                                    s2.Append("<tr>");
                                    s2.Append("<td >Week : " + week + "</td><br/>");
                                    s2.Append("</tr>");
                                    s2.Append("</table>");
                                    DataTable WeeKtable = null;
                                    WeeKtable = dsWeekTables.Tables[i];
                                    s2.Append("<table border = '0'>");
                                    s2.Append("<tr>");
                                    foreach (DataColumn coloum in WeeKtable.Columns)
                                    {

                                        s2.Append("<th style='font-weight: bold; font-size: 7%;'>");
                                        string colName = coloum.ColumnName.Contains("12:00") ? coloum.ColumnName.Split(new string[] { "12:00" }, StringSplitOptions.None)[0] : coloum.ColumnName;
                                        s2.Append(colName);
                                        s2.Append("</th>");
                                    }
                                    s2.Append("</tr>");
                                    foreach (DataRow row in WeeKtable.Rows)
                                    {
                                        s2.Append("<tr>");
                                        foreach (DataColumn column in WeeKtable.Columns)
                                        {
                                            s2.Append("<td style='font-size: 7%'>");
                                            s2.Append(row[column]);
                                            s2.Append("</td>");
                                        }
                                        s2.Append("</tr>");
                                    }
                                    week += 1;
                                    s2.Append("</td>");
                                    s2.Append("</tr></table>");

                                }

                                var header = iTextSharp.text.Image.GetInstance(Server.MapPath("Images/inventrax.png"));
                                header.SetAbsolutePosition(0, 730); // X and Y Accroding to need
                                                                    // var footer = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/letter-head-bottom.jpg"));
                                Document pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 0f);

                                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                                PdfWriter.GetInstance(pdfDoc, new FileStream(URL, FileMode.Create));
                                for (int a = 0; a < 3; a++)
                                {
                                    if (a == 1)
                                    {
                                        sb = null;
                                        sb = sb1;
                                    }
                                    else if (a == 2)
                                    {
                                        sb = null;
                                        sb = s2;
                                    }
                                    StringReader sr = new StringReader(sb.ToString());
                                    pdfDoc.Open();
                                    //pdfDoc.Add(header);
                                    htmlparser.Parse(sr);
                                    pdfDoc.NewPage();
                                }
                                pdfDoc.Close();
                                Response.ContentType = "application/pdf";
                                Response.AddHeader("content-disposition", "attachment;filename=Invoice" + FromDate + "to" + Todatte + "to" + TanentName + ".pdf");
                                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                Response.Write(pdfDoc);
                                Response.Redirect("DisplayInvoice.aspx?fid=" + filename + "");
                                Response.End();
                            }
                        }

                        else
                        {
                            StringBuilder sb = new StringBuilder();

                            //Generate Invoice (Bill) Header.



                            sb.Append("<table style='font-weight: bold; font-size: 9%' border='0' width='100%' cellspacing='0' cellpadding='0'");
                            sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan = '2'><b>Invoice : " + InvoiceNum + "</b></td></tr>");
                            sb.Append("<tr><td align='right' style='background-color: #18B5F0' colspan = '2'><img src=" + Server.MapPath("Images/inventrax.png") + " /> <br/></td></tr>");
                            sb.Append("<tr><td align='right' style='background-color: #18B5F0' colspan = '2'>     Avya Inventrax Private Limited,<br/># ODC-4, First Floor, Phase 2,<br/>Tech Mahindra IT Park, Satyam Jn,<br/>Visakhapatnam, Andhra Pradesh,<br/>India- 530013</td></tr>");
                            sb.Append("<tr><td align='left' style='background-color: #18B5F0' colspan = '2'>" + TanentName + " <br/> " + Add1 + " " + Add2 + " <br/> GST Number :- " + GSTNumber + " <br/> " + EmailID + " <br/> " + Country + " " + Zip + " </td></tr>");
                            sb.Append("<tr><td colspan = '2'></td></tr>");

                            sb.Append("</td><td align = 'right'><b>Date: </b>");
                            sb.Append(DateTime.Now);
                            sb.Append(" </td></tr>");

                            sb.Append("</td></tr>");
                            sb.Append("</table>");
                            sb.Append("<br />");


                            sb.Append("<table border = '1' style='width:100%'>");
                            sb.Append("<tr style='font-weight: bold; font-size: 9%'>");
                            sb.Append("<th style='width=25%; font-size: 9%'> Item</th >" +
                                "<th style='width=7%; font-size: 9%'> HSN Code.</th >" +
                                "<th style='width=7%; font-size: 9%'> Qty.</th >" +
                                "<th style='width=7%; font-size: 9%'> UnitCost</th >" +
                                "<th style='width=7%;'>Rent</th>" +
                                "<th style='width=7%;'>VAT(%)</th>" +
                                "<th style='width=7%;'>VAT</th>" +
                                "<th style='width=7%;'>Total Rent</th>");

                            sb.Append("</tr>");

                            dt.Columns.Remove("CGST");
                            dt.Columns.Remove("CGST_Amt");
                            dt.Columns.Remove("SGST");
                            dt.Columns.Remove("SGST_Amt");
                            dt.Columns.Remove("IGST");
                            dt.Columns.Remove("IGST_Amt");
                            dt.Columns.Remove("Cess");
                            dt.Columns.Remove("CESS_Amt");
                            dt.Columns.Remove("UGST");
                            dt.Columns.Remove("UGST_Amt");
                            dt.Columns.Remove("totalI");
                            dt.Columns.Remove("totalS");
                            dt.Columns.Remove("totalU");
                            dt.Columns.Remove("TotalQty");




                            foreach (DataRow row in dt.Rows)
                            {
                                sb.Append("<tr>");
                                foreach (DataColumn column in dt.Columns)
                                {
                                    sb.Append("<td style='font-size: 9%'>");
                                    sb.Append(row[column]);
                                    sb.Append("</td>");
                                }
                                sb.Append("</tr>");
                            }
                            sb.Append("<tr>");
                            sb.Append("<td style='font-size: 9%;font-weight: bold;'>Total</td>  " +
                                "<td style='font-size: 11%'></td> " +
                                "<td style='font-size: 11%'></td>  " +
                                "<td style='font-size: 9%'></td> " +
                                "<td style='font-size: 9%'> " + dt.Compute("sum(Qty)", "") + "</td> " +
                                "<td style='font-size: 9%'></td>" +
                                " <td style='font-size: 9%'> " + dt.Compute("sum(VAT_Amt)", "") + "</td> " +
                                //"<td style='font-size: 9%'></td>" +
                                //" <td style='font-size: 9%'> " + dt.Compute("sum(SGST_Amt)", "") + "</td> " +
                                //" <td style='font-size: 9%'></td> " +
                                //"<td style='font-size: 9%'> " + dt.Compute("sum(IGST_Amt)", "") + "</td> " +
                                //"<td style='font-size: 9%'></td> " +
                                //" <td style='font-size: 9%'> " + dt.Compute("sum(CESS_Amt)", "") + "</td> " +
                                "<td style='font-size: 9%'> " + dt.Compute("sum(totalV)", "") + "</td>");
                            sb.Append("</tr>");
                            sb.Append("</table>");
                            string Query_InboundTra = "EXEC [dbo].[sp_TPL_GST_GETINVOICEDETAILS_INBOUND] @TENANTID =  " + TanentID + ", @FromDate = " + DB.SQuote(FromDate) + ", @Todtae = " + DB.SQuote(Todatte) + "";
                            DataSet InboundTra = DB.GetDS(Query_InboundTra, false);
                            DataTable InboundTable = InboundTra.Tables[0];
                            DataTable OutboundTable = InboundTra.Tables[1];
                            StringBuilder sb1 = new StringBuilder();
                            if (InboundTra.Tables[0].Rows.Count != 0 && InboundTra.Tables[0] != null)
                            {
                                sb1.Append("<table border = '0' style='font-weight: bold; font-size: 9%;width:100%' >");
                                sb1.Append("<tr><td style='background-color: #18B5F0' colspan = '2'> INBOUND TRANSACTIONS</td></tr>");
                                sb1.Append("</table>");
                                sb1.Append("<br />");
                                sb1.Append("<table border = '1' style='width:100%'>");
                                sb1.Append("<tr style='font-weight: bold; font-size: 9%'>");
                                sb1.Append("<th style='width=25%; font-size: 7%;'> ACTIVITY NAME</th ><th style='width=7%;'> ACTIVITY DATE</th ><th style='width=7%;'> PRICE PER UNIT</th ><th style='width=7%;'>UOM</th><th style='width=7%;'>QUANTITY</th><th style='width=7%;'>PRICE</th>");
                                sb1.Append("</tr>");
                                foreach (DataRow row in InboundTable.Rows)
                                {
                                    sb1.Append("<tr>");
                                    foreach (DataColumn column in InboundTable.Columns)
                                    {
                                        sb1.Append("<td style='font-size: 7%'>");
                                        sb1.Append(row[column]);
                                        sb1.Append("</td>");
                                    }
                                    sb1.Append("</tr>");
                                }
                                sb1.Append("</table>");
                            }
                            if (InboundTra.Tables[1].Rows.Count != 0 && InboundTra.Tables[1] != null)
                            {
                                sb1.Append("<table border = '0' style='font-weight: bold; font-size: 9%;width:100%' >");
                                sb1.Append("<tr><td style='background-color: #18B5F0' colspan = '2'> OUTBOUND TRANSACTIONS</td></tr>");
                                sb1.Append("</table>");
                                sb1.Append("<br />");
                                sb1.Append("<table border = '1' style='width:100%'>");
                                sb1.Append("<tr>");
                                sb1.Append("<th style='width=25%; font-size: 7%;'> ACTIVITY NAME</th ><th style='width=7%;'> ACTIVITY DATE</th ><th style='width=7%;'> PRICE PER UNIT</th ><th style='width=7%;'>UOM</th><th style='width=7%;'>QUANTITY</th><th style='width=7%;'>PRICE</th>");
                                sb1.Append("</tr>");
                                foreach (DataRow row in OutboundTable.Rows)
                                {
                                    sb1.Append("<tr>");
                                    foreach (DataColumn column in OutboundTable.Columns)
                                    {
                                        sb1.Append("<td style='font-size: 7%'>");
                                        sb1.Append(row[column]);
                                        sb1.Append("</td>");
                                    }
                                    sb1.Append("</tr>");
                                }
                                sb1.Append("</table>");
                            }

                            StringBuilder s2 = new StringBuilder();
                            s2.Append("<table border = '0' style='width:100%'>");
                            s2.Append("<tr><td style='background-color: #18B5F0' colspan = '2'> Material Storage Information </td></tr>");
                            s2.Append("</table>");

                            DataSet DS = DB.GetDS("EXEC [dbo].[sp_TPL_GetStoredItemsDetails_Weekly] @TenantID = " + TanentID + ", @StartDate = '" + FromDate + "',@EndDate = '" + Todatte + "'", false);

                            DataSet dsWeekTables = TPL.InvoiceWriter.StorageItems.GetWeekTables(DS);
                            int week = 1;
                            for (int i = 0; dsWeekTables.Tables.Count > i; i++)
                            {

                                s2.Append("<table border = '0' style='font-weight: bold; font-size: 7%;width:100%' >");
                                s2.Append("<tr>");
                                s2.Append("<td >Week : " + week + "</td><br/>");
                                s2.Append("</tr>");
                                s2.Append("</table>");
                                DataTable WeeKtable = null;
                                WeeKtable = dsWeekTables.Tables[i];
                                s2.Append("<table border = '0'>");
                                s2.Append("<tr>");
                                foreach (DataColumn coloum in WeeKtable.Columns)
                                {

                                    s2.Append("<th style='font-weight: bold; font-size: 7%;'>");
                                    string colName = coloum.ColumnName.Contains("12:00") ? coloum.ColumnName.Split(new string[] { "12:00" }, StringSplitOptions.None)[0] : coloum.ColumnName;
                                    s2.Append(colName);
                                    s2.Append("</th>");
                                }
                                s2.Append("</tr>");
                                foreach (DataRow row in WeeKtable.Rows)
                                {
                                    s2.Append("<tr>");
                                    foreach (DataColumn column in WeeKtable.Columns)
                                    {
                                        s2.Append("<td style='font-size: 7%'>");
                                        s2.Append(row[column]);
                                        s2.Append("</td>");
                                    }
                                    s2.Append("</tr>");
                                }
                                week += 1;
                                s2.Append("</td>");
                                s2.Append("</tr></table>");

                            }

                            var header = iTextSharp.text.Image.GetInstance(Server.MapPath("Images/inventrax.png"));
                            header.SetAbsolutePosition(0, 730); // X and Y Accroding to need
                                                                // var footer = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/letter-head-bottom.jpg"));
                            Document pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 0f);

                            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                            PdfWriter.GetInstance(pdfDoc, new FileStream(URL, FileMode.Create));
                            for (int a = 0; a < 3; a++)
                            {
                                if (a == 1)
                                {
                                    sb = null;
                                    sb = sb1;
                                }
                                else if (a == 2)
                                {
                                    sb = null;
                                    sb = s2;
                                }
                                StringReader sr = new StringReader(sb.ToString());
                                pdfDoc.Open();
                                //pdfDoc.Add(header);
                                htmlparser.Parse(sr);
                                pdfDoc.NewPage();
                            }
                            pdfDoc.Close();
                            Response.ContentType = "application/pdf";
                            Response.AddHeader("content-disposition", "attachment;filename=Invoice" + FromDate + "to" + Todatte + "to" + TanentName + ".pdf");
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Response.Write(pdfDoc);
                            Response.Redirect("DisplayInvoice.aspx?fid=" + filename + "");
                            Response.End();
                        }
                        //  StringBuilder sb = new StringBuilder();

                        //  //Generate Invoice (Bill) Header.



                        //  sb.Append("<table style='font-weight: bold; font-size: 9%' border='0' width='100%' cellspacing='0' cellpadding='0'");
                        //  sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan = '2'><b>Invoice : " + InvoiceNum + "</b></td></tr>");
                        //  sb.Append("<tr><td align='right' style='background-color: #18B5F0' colspan = '2'><img src=" + Server.MapPath("Images/inventrax.png") + " /> <br/></td></tr>");
                        //  sb.Append("<tr><td align='right' style='background-color: #18B5F0' colspan = '2'>     Avya Inventrax Private Limited,<br/># ODC-4, First Floor, Phase 2,<br/>Tech Mahindra IT Park, Satyam Jn,<br/>Visakhapatnam, Andhra Pradesh,<br/>India- 530013</td></tr>");
                        //  sb.Append("<tr><td align='left' style='background-color: #18B5F0' colspan = '2'>"+ TanentName + " <br/> " + Add1 + " " + Add2 + " <br/> GST Number :- "+GSTNumber+" <br/> " + EmailID + " <br/> "+Country+" "+Zip+" </td></tr>");
                        //  sb.Append("<tr><td colspan = '2'></td></tr>");

                        //  sb.Append("</td><td align = 'right'><b>Date: </b>");
                        //  sb.Append(DateTime.Now);
                        //  sb.Append(" </td></tr>");

                        //  sb.Append("</td></tr>");
                        //  sb.Append("</table>");
                        //  sb.Append("<br />");


                        //  sb.Append("<table border = '1' style='width:100%'>");
                        //  sb.Append("<tr style='font-weight: bold; font-size: 9%'>");
                        //  sb.Append("<th style='width=25%; font-size: 9%'> Item</th ><th style='width=7%; font-size: 9%'> HSN Code.</th ><th style='width=7%; font-size: 9%'> Qty.</th ><th style='width=7%; font-size: 9%'> UnitCost</th ><th style='width=7%;'>Rent</th><th style='width=7%;'>CGST(%)</th><th style='width=7%;'>CGST</th ><th style='width=7%;'>SGST(%)</th> <th style='width=7%;'>SGST</th> <th style='width=7%;'>IGST(%)</th> <th style='width=7%;'>IGST</th><th style='width=7%;'>Cess(%)</th><th style='width=7%;'>Cess</th><th style='width=7%;'>Total Rent</th>");

                        //  sb.Append("</tr>");
                        //  foreach (DataRow row in dt.Rows)
                        //  {
                        //      sb.Append("<tr>");
                        //      foreach (DataColumn column in dt.Columns)
                        //      {
                        //          sb.Append("<td style='font-size: 9%'>");
                        //          sb.Append(row[column]);
                        //          sb.Append("</td>");
                        //      }
                        //      sb.Append("</tr>");
                        //  }
                        //  sb.Append("<tr>");
                        //  sb.Append("<td style='font-size: 9%;font-weight: bold;'>Total</td>  <td style='font-size: 11%'></td> <td style='font-size: 11%'></td>  " +
                        //      "<td style='font-size: 9%'></td> <td style='font-size: 9%'> " + dt.Compute("sum(Qty)", "") + "</td> <td style='font-size: 9%'></td>" +
                        //      " <td style='font-size: 9%'> " + dt.Compute("sum(CGST_Amt)", "") + "</td> <td style='font-size: 9%'></td> <td style='font-size: 9%'> " + dt.Compute("sum(SGST_Amt)", "") + "</td> " +
                        //      " <td style='font-size: 9%'></td> <td style='font-size: 9%'> " + dt.Compute("sum(IGST_Amt)", "") + "</td> <td style='font-size: 9%'></td> " +
                        //      " <td style='font-size: 9%'> " + dt.Compute("sum(CESS_Amt)", "") + "</td> <td style='font-size: 9%'> " + dt.Compute("sum(TotalQty)", "") + "</td>"); 
                        //  sb.Append("</tr>");
                        //  sb.Append("</table>");
                        //  string Query_InboundTra = "EXEC [dbo].[sp_TPL_GST_GETINVOICEDETAILS_INBOUND] @TENANTID =  " + TanentID + ", @FromDate = " + DB.SQuote(FromDate) + ", @Todtae = " + DB.SQuote(Todatte) + "";
                        //  DataSet InboundTra = DB.GetDS(Query_InboundTra, false);
                        //  DataTable InboundTable = InboundTra.Tables[0];
                        //  StringBuilder sb1 = new StringBuilder();
                        //  if (InboundTra.Tables[0].Rows.Count != 0 && InboundTra.Tables[0] != null)
                        //  {
                        //      sb1.Append("<table border = '0' style='font-weight: bold; font-size: 9%;width:100%' >");
                        //      sb1.Append("<tr><td style='background-color: #18B5F0' colspan = '2'> INBOUND TRANSACTIONS</td></tr>");
                        //      sb1.Append("</table>");
                        //      sb1.Append("<br />");
                        //      sb1.Append("<table border = '1' style='width:100%'>");
                        //      sb1.Append("<tr style='font-weight: bold; font-size: 9%'>");
                        //      sb1.Append("<th style='width=25%; font-size: 7%;'> ACTIVITY NAME</th ><th style='width=7%;'> ACTIVITY DATE</th ><th style='width=7%;'> PRICE PER UNIT</th ><th style='width=7%;'>UOM</th><th style='width=7%;'>QUANTITY</th><th style='width=7%;'>PRICE</th>");
                        //      sb1.Append("</tr>");
                        //      foreach (DataRow row in InboundTable.Rows)
                        //      {
                        //          sb1.Append("<tr>");
                        //          foreach (DataColumn column in InboundTable.Columns)
                        //          {
                        //              sb1.Append("<td style='font-size: 7%'>");
                        //              sb1.Append(row[column]);
                        //              sb1.Append("</td>");
                        //          }
                        //          sb1.Append("</tr>");
                        //      }
                        //      sb1.Append("</table>");
                        //  }
                        //  if (InboundTra.Tables[1].Rows.Count != 0 && InboundTra.Tables[1] != null)
                        //  {
                        //      sb1.Append("<table border = '0' style='font-weight: bold; font-size: 9%;width:100%' >");
                        //      sb1.Append("<tr><td style='background-color: #18B5F0' colspan = '2'> OUTBOUND TRANSACTIONS</td></tr>");
                        //      sb1.Append("</table>");
                        //      sb1.Append("<br />");
                        //      sb1.Append("<table border = '1' style='width:100%'>");
                        //      sb1.Append("<tr>");
                        //      sb1.Append("<th style='width=25%; font-size: 7%;'> ACTIVITY NAME</th ><th style='width=7%;'> ACTIVITY DATE</th ><th style='width=7%;'> PRICE PER UNIT</th ><th style='width=7%;'>UOM</th><th style='width=7%;'>QUANTITY</th><th style='width=7%;'>PRICE</th>");
                        //      sb1.Append("</tr>");
                        //      foreach (DataRow row in InboundTable.Rows)
                        //      {
                        //          sb1.Append("<tr>");
                        //          foreach (DataColumn column in InboundTable.Columns)
                        //          {
                        //              sb1.Append("<td style='font-size: 7%'>");
                        //              sb1.Append(row[column]);
                        //              sb1.Append("</td>");
                        //          }
                        //          sb1.Append("</tr>");
                        //      }
                        //      sb1.Append("</table>");
                        //  }

                        //  StringBuilder s2 = new StringBuilder();
                        //  s2.Append("<table border = '0' style='width:100%'>");
                        //  s2.Append("<tr><td style='background-color: #18B5F0' colspan = '2'> Material Storage Information </td></tr>");
                        //  s2.Append("</table>");

                        //  DataSet DS = DB.GetDS("EXEC [dbo].[sp_TPL_GetStoredItemsDetails_Weekly] @TenantID = " + TanentID + ", @StartDate = '" + FromDate + "',@EndDate = '" + Todatte + "'", false);

                        // DataSet dsWeekTables =  TPL.InvoiceWriter.StorageItems.GetWeekTables(DS);
                        // int week = 1;   
                        // for (int i = 0; dsWeekTables.Tables.Count > i; i++)
                        // {

                        //     s2.Append("<table border = '0' style='font-weight: bold; font-size: 7%;width:100%' >");
                        //     s2.Append("<tr>");
                        //     s2.Append("<td >Week : "+week+"</td><br/>");
                        //     s2.Append("</tr>");
                        //     s2.Append("</table>");
                        //     DataTable WeeKtable = null;
                        //     WeeKtable = dsWeekTables.Tables[i];
                        //     s2.Append("<table border = '0'>");
                        //     s2.Append("<tr>");
                        //     foreach (DataColumn coloum in WeeKtable.Columns)
                        //     {

                        //         s2.Append("<th style='font-weight: bold; font-size: 7%;'>");
                        //         string colName = coloum.ColumnName.Contains("12:00") ? coloum.ColumnName.Split(new string[] { "12:00" }, StringSplitOptions.None)[0] : coloum.ColumnName;
                        //         s2.Append(colName);
                        //         s2.Append("</th>");
                        //     }
                        //     s2.Append("</tr>");
                        //     foreach (DataRow row in WeeKtable.Rows)
                        //     {
                        //         s2.Append("<tr>");
                        //         foreach (DataColumn column in WeeKtable.Columns)
                        //         {
                        //             s2.Append("<td style='font-size: 7%'>");
                        //             s2.Append(row[column]);
                        //             s2.Append("</td>");
                        //         }
                        //         s2.Append("</tr>");
                        //     }
                        //     week += 1;
                        //     s2.Append("</td>");
                        //     s2.Append("</tr></table>");

                        // }

                        // var header = iTextSharp.text.Image.GetInstance(Server.MapPath("Images/inventrax.png"));
                        // header.SetAbsolutePosition(0, 730); // X and Y Accroding to need
                        //// var footer = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/letter-head-bottom.jpg"));
                        //  Document pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 0f);

                        //  HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                        //  PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                        //  PdfWriter.GetInstance(pdfDoc, new FileStream(URL, FileMode.Create));
                        //  for (int a = 0; a < 3; a++)
                        //  {
                        //      if (a == 1)
                        //      {
                        //          sb = null;
                        //          sb = sb1;
                        //      }
                        //      else if (a == 2)
                        //      {
                        //          sb = null;
                        //          sb = s2;
                        //      }
                        //      StringReader sr = new StringReader(sb.ToString());
                        //      pdfDoc.Open();
                        //      //pdfDoc.Add(header);
                        //      htmlparser.Parse(sr);
                        //      pdfDoc.NewPage();
                        //  }
                        //  pdfDoc.Close();
                        //  Response.ContentType = "application/pdf";
                        //  Response.AddHeader("content-disposition", "attachment;filename=Invoice" + FromDate + "to"+Todatte+"to"+TanentName+".pdf");
                        //  Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        //  Response.Write(pdfDoc);
                        //  Response.Redirect("DisplayInvoice.aspx?fid=" + filename + "");
                        //  Response.End();

                    }
                }
            }
        }
      
        public string CommonFileSave(HttpPostedFileBase postedFile, string filePath)
        {
            string resultResponse = "sccuess";

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
                postedFile.SaveAs(filePath + postedFile.FileName);
            }
            else
            {
                filePath = filePath + postedFile.FileName;
                if (!System.IO.File.Exists(filePath))
                {
                    postedFile.SaveAs(filePath);
                }
            }
            return resultResponse;
        }


        protected void resetError(string error, bool isError)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);
        }
    }
}