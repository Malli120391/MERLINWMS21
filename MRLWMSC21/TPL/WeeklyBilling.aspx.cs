using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
//using System.Web.Script.Services;
using MRLWMSC21Common;
using System.IO;
using System.Text;
using System.Globalization;
//using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;


namespace MRLWMSC21.TPL
{
    public partial class WeeklyBilling : System.Web.UI.Page
    {
        CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //DesignLogic.SetInnerPageSubHeading(this.Page, "Weekly Billing");
                LoadTenants();
                LoadHeader();
            }
        }

        private void LoadHeader()
        {
            DataSet dsHeader = new DataSet();
            dsHeader = DB.GetDS("Exec sp_RPT_GetTenantDetails " + ddlTenant.Value, false);
            if (dsHeader != null && dsHeader.Tables.Count > 0 && dsHeader.Tables[0].Rows.Count > 0)
            {
                ltCustomerName.Text = dsHeader.Tables[0].Rows[0]["TenantName"].ToString();
                ltAddress.Text = dsHeader.Tables[0].Rows[0]["Address1"].ToString() + "<br>&nbsp;&nbsp;" + dsHeader.Tables[0].Rows[0]["Address2"].ToString();
            }
        }

        private void LoadTenants()
        {
            DataSet dsTenants = DB.GetDS("SELECT TenantCode +' ['+TenantName+'] ' AS TenantName ,TenantID  FROM TPL_Tenant WHERE TenantID > 0 AND IsActive=1 AND IsDeleted=0", false);
            ddlTenant.DataSource = dsTenants.Tables[0];
            ddlTenant.DataTextField = "TenantName";
            ddlTenant.DataValueField = "TenantID";
            ddlTenant.DataBind();

            //throw new NotImplementedException();
        }



        [WebMethod]
        public static string PrepareGrids(string _sTenantID, string _sDateFrom, string _sDateTo)
        {
            DataSet dsData = GetData(_sTenantID, _sDateFrom, _sDateTo);
            if (dsData != null && dsData.Tables.Count > 0)
            {
                PrepareColumns(dsData);
                return JsonConvert.SerializeObject(dsData);
            }
            else return null;
        }





        public static DataSet GetData(string _sTenantID, string _sDateFrom, string _sDateTo)
        {
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["DBConn"].ToString());
            try
            {
                SqlCommand command = new SqlCommand("[dbo].[SP_RPT_TPL_Weekly_BillingInfo_WeekCharge]", con); // SP_RPT_TPL_Weekly_BillingInfo
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@TenantId", SqlDbType.Int).Value = _sTenantID;
                command.Parameters.Add("@StartDate", SqlDbType.Date).Value = _sDateFrom == "" ? null : DateTime.ParseExact(_sDateFrom, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString();  // "06/21/2016";
                command.Parameters.Add("@EndDate", SqlDbType.Date).Value = _sDateTo == "" ? null : DateTime.ParseExact(_sDateTo, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString();  // "07/20/2016";

                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                con.Close();
                //for (int i = 0; i < ds.Tables.Count; i++)
                //{
                //    for (int j = 0; j < ds.Tables[i].Rows.Count; j++)
                //    {
                //        for (int k = 0; k < ds.Tables[i].Columns.Count; k++)
                //        {
                //            ds.Tables[i].Rows[j][k]= ds.Tables[i].Rows[j][k].ToString() == "" ? "-" : ds.Tables[i].Rows[j][k].ToString();                        
                //        }
                //    }
                //}
                return ds;
            }
            catch (Exception ex) { con.Close(); return null; }
        }

        public static DataSet PrepareColumns(DataSet dsData)
        {
            int tablesCount = dsData.Tables.Count;
            string colorCode = "White";
            string title = "Week";
            string width1 = "90";
            for (int j = 0; j < tablesCount; j++)
            {
                if (j == 0)
                {
                    colorCode = "rgba(0,100,255,0.2)";
                    title = "Week-1";
                }
                if (j == 1)
                {
                    colorCode = "rgba(0,100,255,0.3)";
                    title = "Week-2";
                    if (tablesCount == 2)
                    {
                        title = "Consolidated Totals";
                        width1 = "200";
                    }
                }
                if (j == 2)
                {
                    colorCode = "rgba(0,100,255,0.4)";
                    title = "Week-3";
                    if (tablesCount == 3)
                    {
                        title = "Consolidated Totals";
                        width1 = "200";
                    }
                }
                if (j == 3)
                {
                    colorCode = "rgba(0,100,255,0.5)";
                    title = "Week-4";
                    if (tablesCount == 4)
                    {
                        title = "Consolidated Totals";
                        width1 = "200";
                    }
                }
                if (j == 4)
                {
                    colorCode = "rgba(0,100,255,0.5)";
                    title = "Week-5";
                    if (tablesCount == 5)
                    {
                        title = "Consolidated Totals";
                        width1 = "200";
                    }
                }
                if (j == 5)
                {
                    width1 = "250";
                    colorCode = "rgba(0,100,255,0.5)";
                    title = "Consolidated Totals";
                }
                string str = "[";
                string str1 = "{  headerName: \"" + title + "\",    children: [";
                DataTable dt = dsData.Tables[j];
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string HeaderName = dt.Columns[i].ToString();
                    if (HeaderName == "DisplaySeq")
                    {
                        continue;
                    }
                    if (HeaderName == "RateName")
                        str = str + "{ headerName:\"Rate Name\", field:\"" + dt.Columns[i] + "\", width:180, pinned:true, cellStyle:{\"background-color\": \"" + colorCode + "\"}, cellRenderer: ModifyCellData },";
                    else if (HeaderName.Contains("Activity") || HeaderName.Contains("Week"))
                        str1 = str1 + "{ headerName:\"" + HeaderName + "\", field:\"" + dt.Columns[i] + "\", width:" + width1 + ", cellStyle:{'text-align':'left'}},";
                    else
                        str1 = str1 + "{ headerName:\"" + HeaderName + "\", field:\"" + dt.Columns[i] + "\", width:" + width1 + ", cellStyle:{\"text-align\": \"right\"}, cellRenderer: ModifyCellData},";

                }
                str1 = str1.Substring(0, str1.Length - 1) + "]}";
                str = str + str1 + "]";
                DataTable dtheader = new DataTable();
                dtheader.Columns.Add("header");
                dtheader.Rows.Add(str);
                dsData.Tables.Add(dtheader);
            }
            return dsData;
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {

            DataSet ds = GetData(ddlTenant.Value, hdnCalDateFrom.Value, hdnCalDateTo.Value);
            DTToExcel(ds);
        }

        public void DTToExcel(DataSet ds)
        {
            string FileName = "Weekly Billing Report _ " + ddlTenant.Items[ddlTenant.SelectedIndex].Text + "_" + hdnCalDateFrom.Value + " To " + hdnCalDateTo.Value;
            //string FileName = "Leaders and Laggers Employee Report_ " + ddlFilter.Items[ddlFilter.SelectedIndex].Text + "_" + hdnSelectedUserName.Value + "_" + CalDate.Value;
            FileInfo f = new FileInfo(Server.MapPath("Downloads") + string.Format("\\{0}.xlsx", FileName));
            if (f.Exists)
                f.Delete(); // delete the file if it already exist.

            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.ClearHeaders();
            response.ClearContent();
            response.Charset = Encoding.UTF8.WebName;
            response.AddHeader("content-disposition", "attachment; filename=" + FileName + ".xls");
            response.AddHeader("Content-Type", "application/Excel");
            response.ContentType = "application/vnd.xlsx";

            string file = DB.GetSqlS("SELECT LogoPath AS S FROM GEN_Account WHERE AccountID=" + cp.AccountID);
            string Heading = FileName;
            string headerTable = @"<Table><tr><td rowspan='5'><img width='14%'  src='" + HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + HttpContext.Current.Request.ApplicationPath + "/TPL/AccountLogos/" + file + "'/></td><td colspan='15' align='center' style='text-align:center;'><div style='color:Maroon;font-size:15pt;font-weight:700;width:100%;background-color:Lightgrey;'>" + Heading + "</div></td></tr><tr></tr></Table>";
            response.Write(headerTable);
            response.Write("&nbsp;");
            if (ds.Tables.Count > 0)
            {
                // create a string writer
                StringWriter sw1 = new StringWriter();
                HtmlTextWriter htw1 = new HtmlTextWriter(sw1);
                //instantiate a datagrid
                DataGrid dg1 = new DataGrid();
                dg1.DataSource = ds.Tables[0];
                dg1.DataBind();
                dg1.HeaderStyle.ForeColor = System.Drawing.Color.White;
                dg1.HeaderStyle.BackColor = System.Drawing.Color.SteelBlue;
                dg1.RenderControl(htw1);
                Heading = "Week-1";
                headerTable = @"<Table><tr><td colspan='10' align='center' style='text-align:center;'><div style='color:Maroon;font-size:15pt;font-weight:700;width:100%;'>" + Heading + "</div></td></Table>";
                response.Write(headerTable);
                response.Write(sw1.ToString());
                response.Write("&nbsp;");
                dg1.Dispose();
            }
            if (ds.Tables.Count > 1)
            {
                StringWriter sw2 = new StringWriter();
                HtmlTextWriter htw2 = new HtmlTextWriter(sw2);
                //instantiate a datagrid
                DataGrid dg2 = new DataGrid();
                dg2.DataSource = ds.Tables[1];
                dg2.DataBind();
                dg2.HeaderStyle.ForeColor = System.Drawing.Color.White;
                dg2.HeaderStyle.BackColor = System.Drawing.Color.SteelBlue;
                dg2.RenderControl(htw2);
                Heading = "Week-2";
                if (ds.Tables.Count == 2)
                    Heading = "Consolidated Totals";
                headerTable = @"<Table><tr><td colspan='10' align='center' style='text-align:center;'><div style='color:Maroon;font-size:15pt;font-weight:700;width:100%;'>" + Heading + "</div></td></Table>";
                response.Write(headerTable);
                response.Write(sw2.ToString());
                response.Write("&nbsp;");
                dg2.Dispose();
            }
            if (ds.Tables.Count > 2)
            {
                StringWriter sw3 = new StringWriter();
                HtmlTextWriter htw3 = new HtmlTextWriter(sw3);
                //instantiate a datagrid
                DataGrid dg3 = new DataGrid();
                dg3.DataSource = ds.Tables[2];
                dg3.DataBind();
                dg3.HeaderStyle.ForeColor = System.Drawing.Color.White;
                dg3.HeaderStyle.BackColor = System.Drawing.Color.SteelBlue;
                dg3.RenderControl(htw3);
                Heading = "Week-3";
                if (ds.Tables.Count == 3)
                    Heading = "Consolidated Totals";
                headerTable = @"<Table><tr><td colspan='10' align='center' style='text-align:center;'><div style='color:Maroon;font-size:15pt;font-weight:700;width:100%;'>" + Heading + "</div></td></Table>";
                response.Write(headerTable);
                response.Write(sw3.ToString());
                response.Write("&nbsp;");
                dg3.Dispose();
            }
            if (ds.Tables.Count > 3)
            {
                StringWriter sw4 = new StringWriter();
                HtmlTextWriter htw4 = new HtmlTextWriter(sw4);
                //instantiate a datagrid
                DataGrid dg4 = new DataGrid();
                dg4.DataSource = ds.Tables[3];
                dg4.DataBind();
                dg4.HeaderStyle.ForeColor = System.Drawing.Color.White;
                dg4.HeaderStyle.BackColor = System.Drawing.Color.SteelBlue;
                dg4.RenderControl(htw4);
                Heading = "Week-4";
                if (ds.Tables.Count == 4)
                    Heading = "Consolidated Totals";
                headerTable = @"<Table><tr><td colspan='10' align='center' style='text-align:center;'><div style='color:Maroon;font-size:15pt;font-weight:700;width:100%;'>" + Heading + "</div></td></Table>";
                response.Write(headerTable);
                response.Write(sw4.ToString());
                response.Write("&nbsp;");
                dg4.Dispose();
            }
            //if (ds.Tables.Count > 4)
            //{
            //    StringWriter sw5 = new StringWriter();
            //    HtmlTextWriter htw5 = new HtmlTextWriter(sw5);
            //    //instantiate a datagrid
            //    DataGrid dg5 = new DataGrid();
            //    dg5.DataSource = ds.Tables[4];
            //    dg5.DataBind();
            //    dg5.HeaderStyle.ForeColor = System.Drawing.Color.White;
            //    dg5.HeaderStyle.BackColor = System.Drawing.Color.SteelBlue;
            //    dg5.RenderControl(htw5);
            //    Heading = "Totals";
            //    headerTable = @"<Table><tr><td colspan='7' align='center' style='text-align:center;'><div style='color:Maroon;font-size:15pt;font-weight:700;width:100%;'>" + Heading + "</div></td></Table>";
            //    response.Write(headerTable);
            //    response.Write(sw5.ToString());
            //    dg5.Dispose();
            //}
            if (ds.Tables.Count > 4)
            {
                StringWriter sw5 = new StringWriter();
                HtmlTextWriter htw5 = new HtmlTextWriter(sw5);
                //instantiate a datagrid
                DataGrid dg5 = new DataGrid();
                dg5.DataSource = ds.Tables[4];
                dg5.DataBind();
                dg5.HeaderStyle.ForeColor = System.Drawing.Color.White;
                dg5.HeaderStyle.BackColor = System.Drawing.Color.SteelBlue;
                dg5.RenderControl(htw5);
                Heading = "Week-5";
                headerTable = @"<Table><tr><td colspan='10' align='center' style='text-align:center;'><div style='color:Maroon;font-size:15pt;font-weight:700;width:100%;'>" + Heading + "</div></td></Table>";
                response.Write(headerTable);
                response.Write(sw5.ToString());
                response.Write("&nbsp;");
                dg5.Dispose();
            }


            if (ds.Tables.Count > 5)
            {
                StringWriter sw6 = new StringWriter();
                HtmlTextWriter htw6 = new HtmlTextWriter(sw6);
                //instantiate a datagrid
                DataGrid dg6 = new DataGrid();
                dg6.DataSource = ds.Tables[5];
                dg6.DataBind();
                dg6.HeaderStyle.ForeColor = System.Drawing.Color.White;
                dg6.HeaderStyle.BackColor = System.Drawing.Color.SteelBlue;
                dg6.RenderControl(htw6);
                Heading = "Consolidated Totals";
                headerTable = @"<Table><tr><td colspan='2' align='center' style='text-align:center;'><div style='color:Maroon;font-size:15pt;font-weight:700;width:100%;'>" + Heading + "</div></td></Table>";
                response.Write(headerTable);
                response.Write(sw6.ToString());
                dg6.Dispose();
            }

            ds.Dispose();
            response.End();
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }




        protected void btnExportPdf_Click(object sender, EventArgs e)
        {
            string file = DB.GetSqlS("SELECT LogoPath AS S FROM GEN_Account WHERE AccountID=" + cp.AccountID);
            string FileName = "Weekly Billing Report _ " + ddlTenant.Items[ddlTenant.SelectedIndex].Text + "_" + hdnCalDateFrom.Value + " To " + hdnCalDateTo.Value;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition",
                "attachment;filename=" + FileName + ".pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 70f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
           string imgPath = Server.MapPath("../TPL/AccountLogos/" + file);
            //string imgPath = Server.MapPath("../Images/inventrax.jpg");
            iTextSharp.text.Image imghead = iTextSharp.text.Image.GetInstance(imgPath);
            //imghead.SetAbsolutePosition(0, 250);
            imghead.SetAbsolutePosition(10, 770);
            imghead.ScaleAbsolute(160f, 50f);
            pdfDoc.Add(imghead);
            Paragraph paraHeaderInfo = new Paragraph(ddlTenant.Items[ddlTenant.SelectedIndex].Text + "(" + hdnCalDateFrom.Value + " To " + hdnCalDateTo.Value + ")", new Font(Font.FontFamily.HELVETICA, 14, Font.BOLD));
            paraHeaderInfo.Alignment = Element.ALIGN_CENTER;
            paraHeaderInfo.SpacingAfter = 10;
            pdfDoc.Add(paraHeaderInfo);

            DataSet ds = GetData(ddlTenant.Value, hdnCalDateFrom.Value, hdnCalDateTo.Value);

            if (ds.Tables.Count > 0)
            {
                //Create a dummy GridView
                GridView GridView1 = new GridView();
                GridView1.ShowHeaderWhenEmpty = true;
                GridView1.HeaderStyle.Font.Bold = true;
                GridView1.Font.Size = 9;
                GridView1.AllowPaging = false;
                GridView1.DataSource = ds.Tables[0];
                GridView1.DataBind();
                StringWriter sw1 = new StringWriter();
                HtmlTextWriter hw1 = new HtmlTextWriter(sw1);
                GridView1.RenderControl(hw1);
                StringReader sr1 = new StringReader(sw1.ToString());

                Paragraph para1 = new Paragraph("Week-1", new Font(Font.FontFamily.HELVETICA, 12));
                para1.Alignment = Element.ALIGN_CENTER;
                para1.SpacingAfter = 5;
                para1.SpacingBefore = 5;
                pdfDoc.Add(para1);
                htmlparser.Parse(sr1);
            }

            if (ds.Tables.Count > 1)
            {
                GridView GridView2 = new GridView();
                GridView2.ShowHeaderWhenEmpty = true;
                GridView2.HeaderStyle.Font.Bold = true;
                GridView2.Font.Size = 9;
                GridView2.AllowPaging = false;
                GridView2.DataSource = ds.Tables[1];
                GridView2.DataBind();
                StringWriter sw2 = new StringWriter();
                HtmlTextWriter hw2 = new HtmlTextWriter(sw2);
                GridView2.RenderControl(hw2);
                StringReader sr2 = new StringReader(sw2.ToString());

                string _sTitle = "Week-2";
                if (ds.Tables.Count < 3)
                    _sTitle = "Consolidated Totals";
                Paragraph para2 = new Paragraph(_sTitle, new Font(Font.FontFamily.HELVETICA, 12));
                para2.Alignment = Element.ALIGN_CENTER;
                para2.SpacingAfter = 5;
                para2.SpacingBefore = 10;
                pdfDoc.Add(para2);
                htmlparser.Parse(sr2);
            }
            if (ds.Tables.Count > 2)
            {
                GridView GridView3 = new GridView();
                GridView3.ShowHeaderWhenEmpty = true;
                GridView3.HeaderStyle.Font.Bold = true;
                GridView3.Font.Size = 9;
                GridView3.AllowPaging = false;
                GridView3.DataSource = ds.Tables[2];
                GridView3.DataBind();
                StringWriter sw3 = new StringWriter();
                HtmlTextWriter hw3 = new HtmlTextWriter(sw3);
                GridView3.RenderControl(hw3);
                StringReader sr3 = new StringReader(sw3.ToString());

                string _sTitle = "Week-3";
                if (ds.Tables.Count < 4)
                    _sTitle = "Consolidated Totals";
                Paragraph para3 = new Paragraph(_sTitle, new Font(Font.FontFamily.HELVETICA, 12));
                para3.Alignment = Element.ALIGN_CENTER;
                para3.SpacingAfter = 5;
                para3.SpacingBefore = 10;
                pdfDoc.Add(para3);
                htmlparser.Parse(sr3);
            }
            if (ds.Tables.Count > 3)
            {
                GridView GridView4 = new GridView();
                GridView4.ShowHeaderWhenEmpty = true;
                GridView4.HeaderStyle.Font.Bold = true;
                GridView4.Font.Size = 9;
                GridView4.AllowPaging = false;
                GridView4.DataSource = ds.Tables[3];
                GridView4.DataBind();
                StringWriter sw4 = new StringWriter();
                HtmlTextWriter hw4 = new HtmlTextWriter(sw4);
                GridView4.RenderControl(hw4);
                StringReader sr4 = new StringReader(sw4.ToString());

                string _sTitle = "Week-4";
                if (ds.Tables.Count < 5)
                    _sTitle = "Consolidated Totals";
                Paragraph para4 = new Paragraph(_sTitle, new Font(Font.FontFamily.HELVETICA, 12));
                para4.Alignment = Element.ALIGN_CENTER;
                para4.SpacingAfter = 5;
                para4.SpacingBefore = 10;
                pdfDoc.Add(para4);
                htmlparser.Parse(sr4);
            }
            if (ds.Tables.Count > 4)
            {
                GridView GridView5 = new GridView();
                GridView5.ShowHeaderWhenEmpty = true;
                GridView5.HeaderStyle.Font.Bold = true;
                GridView5.Font.Size = 9;
                GridView5.AllowPaging = false;
                GridView5.DataSource = ds.Tables[4];
                GridView5.DataBind();
                StringWriter sw5 = new StringWriter();
                HtmlTextWriter hw5 = new HtmlTextWriter(sw5);
                GridView5.RenderControl(hw5);
                StringReader sr5 = new StringReader(sw5.ToString());

                string _sTitle = "Week-5";
                if (ds.Tables.Count < 6)
                    _sTitle = "Consolidated Totals";
                Paragraph para5 = new Paragraph(_sTitle, new Font(Font.FontFamily.HELVETICA, 12));
                para5.Alignment = Element.ALIGN_CENTER;
                para5.SpacingAfter = 5;
                para5.SpacingBefore = 10;
                pdfDoc.Add(para5);
                htmlparser.Parse(sr5);
            }
            if (ds.Tables.Count > 5)
            {
                GridView GridViewTotalsCons = new GridView();
                GridViewTotalsCons.ShowHeaderWhenEmpty = true;
                GridViewTotalsCons.HeaderStyle.Font.Bold = true;
                GridViewTotalsCons.Font.Size = 9;
                GridViewTotalsCons.AllowPaging = false;
                GridViewTotalsCons.DataSource = ds.Tables[5];
                GridViewTotalsCons.DataBind();
                StringWriter swTotalsCons = new StringWriter();
                HtmlTextWriter hwTotalsCons = new HtmlTextWriter(swTotalsCons);
                GridViewTotalsCons.RenderControl(hwTotalsCons);
                StringReader srTotalsCons = new StringReader(swTotalsCons.ToString());

                Paragraph paraTotalsCons = new Paragraph("Consolidated Totals", new Font(Font.FontFamily.HELVETICA, 12));
                paraTotalsCons.Alignment = Element.ALIGN_CENTER;
                paraTotalsCons.SpacingAfter = 5;
                paraTotalsCons.SpacingBefore = 10;
                pdfDoc.Add(paraTotalsCons);
                htmlparser.Parse(srTotalsCons);
            }
         
            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();
        }



        protected void lnkSend_Click(object sender, EventArgs e)
        {
            try
            {
                //TPLBilling bill = (TPLBilling)Session["bill"];
                TPL.Invoice.TPLBilling bill = new Invoice.TPLBilling();

                if (bill.saveInvoice(Convert.ToInt32(hdnTenantID.Value), DateTime.ParseExact(hdnCalDateFrom.Value, "dd-MM-yyyy", CultureInfo.InvariantCulture), DateTime.ParseExact(hdnCalDateTo.Value, "dd-MM-yyyy", CultureInfo.InvariantCulture)))
                {
                    Session["bill"] = null;
                    Response.Redirect("InvoiceGeneration.aspx?msg=Invoice successfully sent");
                }
                else
                {
                    resetError("Error while sending invoice", true);
                }
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode("0", "Display Invoice", ex.Source, ex.Message, ex.StackTrace);
            }
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("../TPL/InvoiceGeneration.aspx");
        }

        protected void resetError(string error, bool isError)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);


        }

    }
}