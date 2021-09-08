using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using MRLWMSC21Common;
using System.Web.Services;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text.pdf.parser;
using GenCode128;
using System.Web.Script.Services;
using MRLWMSC21Common.PrintCommon;

namespace MRLWMSC21.mOutbound
{
    public partial class DeliveryPackSlip : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected void page_Init(object sender, EventArgs e)
        {
            AddHeaderList();
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Outbound";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("Search Outbound"))))
            {
                Response.Redirect("../Login.aspx?eid=6");
            }

            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Delivery Pack Slip");
                Build_HeaderInformation();
                Build_DeliveryPackSlip(Build_DeliveryPackSlip());
            }
        }

        private void Build_HeaderInformation()
        {
            StringBuilder cMdGetOutboountHeaderInformation = new StringBuilder();
            cMdGetOutboountHeaderInformation.Append("select OBDNumber,USR.FirstName+ISNULL(' '+usr.LastName,'') as RequestedBy,format(OBDDate,'dd-MMM-yyyy') as OBDDate,CUS.CustomerName from OBD_Outbound OBD ");
            cMdGetOutboountHeaderInformation.Append("JOIN GEN_User USR ON USR.UserID=OBD.RequestedBy ");
            cMdGetOutboountHeaderInformation.Append("JOIN GEN_Customer CUS ON CUS.CustomerID=OBD.CustomerID ");
            cMdGetOutboountHeaderInformation.Append("where OutboundID="+CommonLogic.QueryString("obdid"));
            IDataReader rsHeaderinformation = DB.GetRS(cMdGetOutboountHeaderInformation.ToString());
            rsHeaderinformation.Read();
            ltOBDNumber.Text =  "<span class='FormLabels'>   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  Delivery Doc. No. :" + DB.RSField(rsHeaderinformation, "OBDNumber") + "<span><br/><img src=\" ../mInbound/Code39Handler.ashx?code=" + DB.RSField(rsHeaderinformation, "OBDNumber") + "\" />";
            ltCustomer.Text = "&nbsp; : "+DB.RSField(rsHeaderinformation, "CustomerName");
            ltRequestedBy.Text = "&nbsp; : " + DB.RSField(rsHeaderinformation, "RequestedBy");
            ltDelvDocDate.Text = " &nbsp;: " + DB.RSField(rsHeaderinformation, "OBDDate");

        }

        private void AddHeaderList()
        {
            StringBuilder cMdGetMaterialStorageParameters = new StringBuilder();
            cMdGetMaterialStorageParameters.Append("select DISTINCT MSP.MaterialStorageParameterID,DisplayName from OBD_Outbound_ORD_CustomerPO obd ");
            cMdGetMaterialStorageParameters.Append("join ORD_SODetails SOD ON  SOD.SOHeaderID=OBD.SOHeaderID AND SOD.IsDeleted=0 ");
            cMdGetMaterialStorageParameters.Append("JOIN MMT_MaterialMaster_MMT_MaterialStorageParameter MMMSP ON MMMSP.MaterialMasterID=SOD.MaterialMasterID AND MMMSP.IsDeleted=0 ");
            cMdGetMaterialStorageParameters.Append("JOIN MMT_MaterialStorageParameter MSP ON MSP.MaterialStorageParameterID=MMMSP.MaterialStorageParameterID ");
            cMdGetMaterialStorageParameters.Append("WHERE OutboundID="+CommonLogic.QueryString("obdid")+" ORDER BY MSP.MaterialStorageParameterID");

            StringBuilder CreateHeaderTable = new StringBuilder();
            CreateHeaderTable.Append("<table border=\"1\" align=\"center\"width=\"" + 100 + "%\"  style=\"border-collapse:collapse;border: 1px solid black;\"><tr>");

            IDataReader rsGetMaterialStorageParameter = DB.GetRS(cMdGetMaterialStorageParameters.ToString());
            while (rsGetMaterialStorageParameter.Read())
            {
                CreateHeaderTable.Append("<td width=\"90\">" + DB.RSField(rsGetMaterialStorageParameter, "DisplayName") + "</td>");
            }
            CreateHeaderTable.Append("<td width=\"90\">Picked Qty.</td>");
            CreateHeaderTable.Append("</table></tr>");
            gvDeliveryPackSlip.Columns[4].HeaderText = CreateHeaderTable.ToString();
        }

        protected void gvDeliveryPackSlip_DataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && !(e.Row.RowState == DataControlRowState.Edit))
            {
                String SplitLocation = ((Literal)e.Row.FindControl("ltSplitMaterialStorage")).Text;
                ((Literal)e.Row.FindControl("ltSplitMaterialStorage")).Text=Build_SplitLocation(SplitLocation);
            }
        }

        private String Build_SplitLocation(String SplitData)
        {
            StringBuilder GenerateTable = new StringBuilder();
            String[] Datacells;
            GenerateTable.Append("<table border=\"1\" align=\"center\"width=\"" + 100 + "%\"  style=\"border-collapse:collapse;border: 1px solid black;\">");
            String []DataRows = SplitData.Split(',');
            for (int RowIndex = 0; RowIndex < DataRows.Length; RowIndex++)
            {
                GenerateTable.Append("<tr>");
                Datacells = DataRows[RowIndex].Split('|');
                for (int CellIndex = 0; CellIndex < Datacells.Length; CellIndex++)
                {
                    GenerateTable.Append("<td width=\"90px\">"+Datacells[CellIndex]+" </td>");
                }
                GenerateTable.Append("</tr>");
            }
                GenerateTable.Append("</table>");
                return GenerateTable.ToString();
        }

        private DataSet Build_DeliveryPackSlip()
        {
            String cMdGetDeliveryPackSlip = "dbo.sp_OBD_GetDeliveryPackSlip "+CommonLogic.QueryString("obdid");
            DataSet dsGetDeliveryPackSlip = DB.GetDS(cMdGetDeliveryPackSlip,false);
            return dsGetDeliveryPackSlip;
        }

        private void Build_DeliveryPackSlip(DataSet dsGetDeliveryPackSlip)
        {
            gvDeliveryPackSlip.DataSource = dsGetDeliveryPackSlip;
            gvDeliveryPackSlip.DataBind();
            dsGetDeliveryPackSlip.Dispose();
        }



        [WebMethod]
        public static string btnPrintPackingData(string OBDId)
        {
            
            DataSet ds = DB.GetDS("[dbo].[Get_OBD_DeliveryPackSlip] @OutboundId="+OBDId, false);
            if (ds.Tables[0].Rows.Count > 0)
            {
                MRLWMSC21Common.PrintCommon.PackingSlipGenerator PSG = new MRLWMSC21Common.PrintCommon.PackingSlipGenerator();

                MRLWMSC21Common.PrintCommon.PackingData objOBDData = new MRLWMSC21Common.PrintCommon.PackingData();
                objOBDData = PSG.GeneratePackingSlip(OBDId);
                if (objOBDData.OBDPackingData.Count > 0)
                {
                    PreparePDF(objOBDData);
                    //ExportPdfToExcel();
                    // DB.ExecuteSQL("UPDATE OBD_Outbound SET IsBoxPackingCompleted=1 WHERE OutboundID=" + OBDId);
                    return "1";
                }
                else
                    return "0";
                
            }
            else
            {
                return PreapreTable_ReqToFillBoxDetails(ds.Tables[0]);
                //MRLWMSC21Common.PrintCommon.PackingSlipGenerator PSG = new MRLWMSC21Common.PrintCommon.PackingSlipGenerator();

                //MRLWMSC21Common.PrintCommon.PackingData objOBDData = new MRLWMSC21Common.PrintCommon.PackingData();
                //objOBDData = PSG.GeneratePackingSlip(OBDId);
                //if (objOBDData.OBDPackingData.Count > 0)
                //{
                //    PreparePDF(objOBDData);
                //    //ExportPdfToExcel();
                //   // DB.ExecuteSQL("UPDATE OBD_Outbound SET IsBoxPackingCompleted=1 WHERE OutboundID=" + OBDId);
                //    return "1";
                //}
                //else
                //    return "0";
                // System.Diagnostics.Process.Start(HttpContext.Current.Server.MapPath("PackingSlip.pdf"));
            }
        }

        private static void PreparePDF(MRLWMSC21Common.PrintCommon.PackingData obj)
        {
            if (obj != null && obj.OBDPackingData.Count > 0)
            {
                string filename = HttpContext.Current.Server.MapPath("PackingSlip.pdf");


                FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);

                Document doc = new Document(PageSize.A4, 20f, 20f, 20f, 20f);

                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, fs);
                PdfContentByte cb = new PdfContentByte(writer);
                doc.Open();

                for (int j = 0; j < obj.OBDPackingData.Count; j++)
                {

                    // doc.NewPage();
                    int _iFont = 11;


                    float curY = writer.GetVerticalPosition(true);

                    PdfMeasures oMes = new PdfMeasures();
                    oMes = NewPageRequired(obj.OBDPackingData[j], curY);
                    if (oMes.NewPageReq == true)
                    {
                        doc.NewPage();
                        doc.AddHeader("Delivery Pack Slip","");
                    }
                    _iFont = oMes.FontSize;



                    PdfPCell cell = null;
                    PdfPTable tabItems = null;
                    float[] widths = new float[] { 2f, 10f, 3f };
                    tabItems = new PdfPTable(widths);

                    if (j == 0)
                    {
                        doc.Add(SetTableHeader("Delivery Pack Slip", obj.OBDPackingData[j].CustomerName + "-" + obj.OBDPackingData[j].SiteCode.TrimStart('0'), obj.OBDPackingData[j].OBDNumber, obj.OBDPackingData[j].VehicleNo, obj.OBDPackingData[j].DriverName, obj.OBDPackingData.Count.ToString(), cb, obj.OBDPackingData[j].userName));


                        
                        cell = new PdfPCell(new Phrase("S. No.", FontFactory.GetFont("Arial", _iFont, Font.BOLD, BaseColor.BLACK)));
                        cell.MinimumHeight = 20;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        tabItems.AddCell(cell);
                        cell = new PdfPCell(new Phrase("SKU CODE", FontFactory.GetFont("Arial", _iFont, Font.BOLD, BaseColor.BLACK)));
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.MinimumHeight = 20;
                        tabItems.AddCell(cell);
                        cell = new PdfPCell(new Phrase("Quantity", FontFactory.GetFont("Arial", _iFont, Font.BOLD, BaseColor.BLACK)));
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.MinimumHeight = 20;
                        tabItems.AddCell(cell);
                    }


                    MRLWMSC21Common.PrintCommon.PackingSlip oSlip = new MRLWMSC21Common.PrintCommon.PackingSlip();
                    oSlip.BoxItemDetails = obj.OBDPackingData[j].BoxItemDetails;

                    for (int i = 0; i < oSlip.BoxItemDetails.Count; i++)
                    {
                        cell = new PdfPCell(new Phrase((j + 1).ToString(), FontFactory.GetFont("Arial", _iFont, Font.NORMAL, BaseColor.BLACK)));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        tabItems.AddCell(cell);
                        cell = new PdfPCell(new Phrase(oSlip.BoxItemDetails[i].SKUCode, FontFactory.GetFont("Arial", _iFont, Font.NORMAL, BaseColor.BLACK)));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        tabItems.AddCell(cell);
                        cell = new PdfPCell(new Phrase(oSlip.BoxItemDetails[i].Quantity.ToString(), FontFactory.GetFont("Arial", _iFont, Font.NORMAL, BaseColor.BLACK)));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        tabItems.AddCell(cell);
                    }

                    //tabItems.SpacingAfter = 20;
                    doc.Add(tabItems);
                }

                doc.Close();
                //ShowPdf(filename);
            }
        }



        public static void ShowPdf(string filename)
        {
            //Clears all content output from Buffer Stream
            HttpContext.Current.Response.ClearContent();
            //Clears all headers from Buffer Stream
            HttpContext.Current.Response.ClearHeaders();
            //Adds an HTTP header to the output stream
            HttpContext.Current.Response.AddHeader("Content-Disposition", "inline;filename=" + filename);
            //Gets or Sets the HTTP MIME type of the output stream
            HttpContext.Current.Response.ContentType = "application/pdf";
            //Writes the content of the specified file directory to an HTTP response output stream as a file block
            HttpContext.Current.Response.WriteFile(filename);
            //sends all currently buffered output to the client
            HttpContext.Current.Response.Flush();
            //Clears all content output from Buffer Stream
            HttpContext.Current.Response.Clear();
        }

        public static PdfPTable SetTableHeader(string HeaderName,string StoreName, string OBDNumber, string BoxNumber,string DriverName, string TotalNoOfBoxes, PdfContentByte cb, string userName)
        {
            int _iMinHeight = 40;
            Font fontHead = new Font(FontFactory.GetFont("Arial", 15, Font.BOLD, BaseColor.BLACK));


            PdfPTable table = new PdfPTable(3);

            PdfPCell cell = new PdfPCell(new Paragraph("Delivery Pack Slip", fontHead));
            cell.MinimumHeight = _iMinHeight;
            //table.AddCell(cell);
            cell = new PdfPCell(new Paragraph(HeaderName,FontFactory.GetFont("Arial", 20, Font.BOLD, BaseColor.BLACK)));
            cell.Colspan = 5;
            cell.MinimumHeight = _iMinHeight;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);


            cell = new PdfPCell(new Phrase("CUSTOMER NAME", fontHead));
            cell.MinimumHeight = _iMinHeight;

            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(StoreName, FontFactory.GetFont("Arial", 20, Font.BOLD, BaseColor.BLACK)));
            cell.Colspan = 2;
            cell.MinimumHeight = _iMinHeight;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);


            cell = new PdfPCell(new Phrase("OBD No.", fontHead));
            cell.MinimumHeight = _iMinHeight;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);


            cell = new PdfPCell(new Phrase("", fontHead));
            cell.Colspan = 2;
            cell.MinimumHeight = _iMinHeight;
            cell.PaddingLeft = 100;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;


            iTextSharp.text.Image img = CreateBarCode(OBDNumber , cb);
            img.ScaleAbsolute(100, 1);
            cell.AddElement(img);


            table.AddCell(cell);



                
            cell = new PdfPCell(new Phrase("VEHICLE No.", fontHead));
            cell.MinimumHeight = _iMinHeight;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);
            //cell = new PdfPCell(new Phrase(userName  + BoxNumber + " of " + TotalNoOfBoxes, fontHead));
            cell = new PdfPCell(new Phrase(userName + BoxNumber, fontHead));
            cell.MinimumHeight = _iMinHeight;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Colspan = 2;
            table.AddCell(cell);


            cell = new PdfPCell(new Phrase("DRIVER NAME", fontHead));
            cell.MinimumHeight = _iMinHeight;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);
            //cell = new PdfPCell(new Phrase(userName  + BoxNumber + " of " + TotalNoOfBoxes, fontHead));
            cell = new PdfPCell(new Phrase(userName + DriverName, fontHead));
            cell.MinimumHeight = _iMinHeight;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Colspan = 2;
            table.AddCell(cell);

            return table;

        }

        public static iTextSharp.text.Image CreateBarCode(string BarcodeString, PdfContentByte cb)
        {



            Barcode128 code128 = new Barcode128();
            code128.CodeType = Barcode.EAN13;
            code128.ChecksumText = false;
            code128.GenerateChecksum = false;
            code128.BarHeight = 15;
            code128.Code = BarcodeString;

            iTextSharp.text.Image image128 = code128.CreateImageWithBarcode(cb, null, null);

            image128.ScaleToFit(10, 10);
            return image128;
        }

        public static PdfMeasures NewPageRequired(MRLWMSC21Common.PrintCommon.PackingSlip obj, float curY)
        {
            PdfMeasures oMes = new PdfMeasures();
            float pageHeight = 822;
            float TabHeaderHeight = 220.34f;
            float lineItemHeight = 16.66f;
            int lineItemsCount = obj.BoxItemDetails.Count;

            float remainSpace = pageHeight - (pageHeight - curY);
            float reqSpace = TabHeaderHeight + (lineItemsCount * lineItemHeight);
            float diff = remainSpace - reqSpace;

            decimal remainPerc = Convert.ToDecimal((remainSpace / pageHeight) * 100);
            decimal reqPerc = Convert.ToDecimal((reqSpace / pageHeight) * 100);

            decimal diffPerc = reqPerc - remainPerc;


            if (diffPerc < 0 || diffPerc > 20)
            {
                oMes.FontSize = 11;
                oMes.NewPageReq = false;
                return oMes;
            }

            if (diffPerc < 10)
            {
                oMes.FontSize = 9;
                oMes.NewPageReq = false;
                return oMes;
            }

            else if (diffPerc < 20)
            {
                oMes.FontSize = 8;
                oMes.NewPageReq = false;
                return oMes;
            }

            else
            {
                oMes.FontSize = 11;
                oMes.NewPageReq = true;
                return oMes;
            }

        }

        //public static void ExportPdfToExcel()
        //{
        //    string fileName = HttpContext.Current.Server.MapPath("PackingSlip.pdf");
        //    StringBuilder text = new StringBuilder();
        //    PdfReader pdfReader = new PdfReader(fileName);
        //    for (int page = 1; page <= pdfReader.NumberOfPages; page++)
        //    {
        //        ITextExtractionStrategy strategy = new LocationTextExtractionStrategy();
        //        string currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);
        //        currentText = Encoding.UTF8.GetString(Encoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.UTF8.GetBytes(currentText)));
        //        text.Append(currentText);

        //    }
        //    pdfReader.Close();
        //    HttpContext.Current.Response.Clear();
        //    HttpContext.Current.Response.Buffer = true;
        //    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=ReceiptExport.xls");
        //    HttpContext.Current.Response.Charset = "";
        //    HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
        //    HttpContext.Current.Response.Write(text);
        //    HttpContext.Current.Response.Flush();
        //    HttpContext.Current.Response.End();
        //}

        public static string PreapreTable_ReqToFillBoxDetails(DataTable dt)
        {
            StringBuilder html = new StringBuilder();
           // dt.Columns.RemoveAt(2);
            //Table start.
            html.Append("<div style='background-color:orange;padding:10px;color:white;font-weight:700;'>BOX PENDING DETAILS  <span id='spanClose' class='spanClose fa fa-close' onclick='ClosePopUp()' style='color:white;font-size:15pt;font-weight:500;cursor:pointer;float:right;'></span></div>");
            html.Append("<table border = '1' cellPadding='5' cellSpacing='1' style='width:100%;text-align:left;border-collapse:collapse;border:1px solid #E7E7E7;'>");
            //Building the Header row.
            html.Append("<tr>");
            foreach (DataColumn column in dt.Columns)
            {
                html.Append("<th>");
                html.Append(column.ColumnName);
                html.Append("</th>");
            }
            html.Append("</tr>");

            //Building the Data rows.
            foreach (DataRow row in dt.Rows)
            {
                html.Append("<tr>");
                foreach (DataColumn column in dt.Columns)
                {
                    html.Append("<td>");
                    html.Append(row[column.ColumnName]);
                    html.Append("</td>");
                }
                html.Append("</tr>");
            }

            //Table end.
            html.Append("</table>");

            //Append the HTML string to Placeholder.
            return html.ToString();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<Delivery_Pack_Slip> GetPackList(string obdid)
        //public static List<Current_Stock_Report> GetBillingReportList()
        {

            
            List<Delivery_Pack_Slip> GetBillingReport = new List<Delivery_Pack_Slip>();
            GetBillingReport = GetPack(obdid);
            //GetBillingReport = new CurrentStockReport().GetBillngRPT();
            return GetBillingReport;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<Delivery_Pack_Slip> GetPack(string obdid)
        //private List<Current_Stock_Report> GetBillngRPT()
        {
           // string obdid = CommonLogic.QueryString("obdid");

            List<Delivery_Pack_Slip> lst = new List<Delivery_Pack_Slip>();
            string Vlpd = "0";
            string _query = "select ISNULL(IsVLPDPicking,0)AS IsVLPDPicking from OBD_Outbound where OutboundID=" + obdid;
            DataSet _Ds = DB.GetDS(_query, false);
            if(_Ds.Tables[0].Rows.Count!=0)
            {
                foreach(DataRow row in _Ds.Tables[0].Rows)
                {
                    Vlpd = row["IsVLPDPicking"].ToString();
                }
            }



            string Query = " [dbo].[Get_OBD_DeliveryPackSlip] @OutboundId=" + obdid + ",@VlpdPicking=" + Vlpd;
            DataSet DS = DB.GetDS(Query, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    Delivery_Pack_Slip OR = new Delivery_Pack_Slip();
                    OR.MCode = row["MCode"].ToString();
                    OR.OEMPartNo = row["OEMPartNo"].ToString();
                    OR.MDescription = row["MDescription"].ToString();
                    //OR.CartonCode = row["CartonCode"].ToString();
                    //OR.Location = row["Location"].ToString();
                    //OR.MfgDate = row["MfgDate"].ToString();
                    //OR.ExpDate = row["ExpDate"].ToString();
                    //OR.BatchNo = row["BatchNo"].ToString();
                    //OR.SerialNo = row["SerialNo"].ToString();
                    //OR.ProjectRefNo = row["ProjectRefNo"].ToString();
                    OR.PickedQty = row["PickedQty"].ToString();
                    OR.DockName = row["DockName"].ToString();
                    OR.VehicleNo = row["VehicleNo"].ToString();
                    OR.VehicleType = row["VehicleType"].ToString();
                    OR.DriverName = row["DriverName"].ToString();
                    OR.DriverMobileNo = row["DriverMobileNo"].ToString();


                    lst.Add(OR);
                }
            }
            return lst;
        }

        #region
        //================Added by Priya========================//

        [WebMethod]
        public static string btnPrintPackingSlipInfo(string PSNHeaderID)
        {
            DataSet _dsResult = DB.GetDS("EXEC [dbo].[Get_OBD_DeliveryPackSlip_Material_Info] " + PSNHeaderID, false);

            List<BoxItemDetails1> _picklist = new List<BoxItemDetails1>();
            List<PackingSlip1> _pHeader = new List<PackingSlip1>();

            //List<PickMaterilsList1> _picklist = new List<PickMaterilsList1>();
            //List<PickListHeader1> _pHeader = new List<PickListHeader1>();

            if (_dsResult.Tables.Count > 0)
            {

                foreach (DataRow _drPick in _dsResult.Tables[1].Rows)
                {
                    BoxItemDetails1 _pickMaterial = new BoxItemDetails1
                    {
                        PackingSlipNo = _drPick["PackingSlipNo"].ToString(),
                        MCode = _drPick["MCode"].ToString(),
                        ItemDesc = _drPick["ItemDesc"].ToString(),
                        HandlingRemarks = _drPick["Remarks"].ToString(),
                        PickedQty = _drPick["PickedQuantity"].ToString(),
                        UOM = _drPick["UOM"].ToString(),
                        PackedQty = _drPick["PackedQty"].ToString(),
                        Weight = _drPick["ItemWeight"].ToString(),
                        Volume = _drPick["ItemVolume"].ToString()
                    };
                    _picklist.Add(_pickMaterial);
                }

                if (_dsResult.Tables[0].Rows != null)
                {

                    DataRow row = _dsResult.Tables[0].Rows[0];
                    //DataRow drow = _dsResult.Tables[2].Rows[0];
                    PackingSlip1 _pickHeader = new PackingSlip1
                    {
                        CustomerName = row["CustomerName"].ToString(),
                        CustPONumber = row["CustPONumber"].ToString(),
                        ShipmentDate = row["ShipmentDate"].ToString(),
                        AddressLine1 = row["AddressLine1"].ToString(),
                        Address2 = row["Address2"].ToString(),
                        PSNumber = row["PackingSlipNo"].ToString(),
                        AccountName = row["Account"].ToString(),
                        //TotPickedQty = drow["PickedQuantity"].ToString()
                    };
                    _pHeader.Add(_pickHeader);
                }
            }
           

            UnloadPDFGenerator pdfGenerator = new UnloadPDFGenerator(_picklist, _pHeader);
            //UnloadPDFGenerator pcklistHeader = new UnloadPDFGenerator(_pHeader);

            String filePath = pdfGenerator.GeneratePDF(UnloadTemplate.PACKLIST_SHEET);

            return filePath;
        }
        //================Added by Priya========================//
#endregion
        public class Delivery_Pack_Slip
        {
            public string MCode { get; set; }
            public string OEMPartNo { get; set; }
            public string MDescription { get; set; }
            public string CartonCode { get; set; }
            public string Location { get; set; }
            public string MfgDate { get; set; }
            public string ExpDate { get; set; }
            public string BatchNo { get; set; }
            public string SerialNo { get; set; }
            public string ProjectRefNo { get; set; }
            public string PickedQty { get; set; }
            public string VehicleNo { get; set; }
            public string VehicleType { get; set; }
            public string DriverName { get; set;  }
            public string DockName { get; set; }
            public string DriverMobileNo { get; set; }

        }

    }



    public class PdfMeasures
    {
        public int FontSize { set; get; }
        public bool NewPageReq { get; set; }
    }




   

    
}