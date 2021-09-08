using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using MRLWMSC21Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;

namespace MRLWMSC21.mInbound
{
    public partial class InbImports : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        public static CustomPrincipal CustomPrincipal = HttpContext.Current.User as CustomPrincipal;
        protected void PagePre_Init(object sender, EventArgs e)
        {
            Page.Theme = "Inbound";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Inward Import");
                LoadExcelColumns();

            }

        }
        private void LoadExcelColumns()
        {
            //List<string> _list = new List<string>(){"SNo", "TenantCode", "SupplierCode", "InvoiceNo", "InvoiceDate", "InvoiceUoM", "InvoiceValue",
            //    "InvoiceQuantity",  "PONumber", "PODate", "POValue", "POQuantity", "ShipmentDate","WHCode","ItemCode","ItemValue", 
            //    "BatchNo", "ExpDate", "MfgDate", "QtyPerUoM", "Qty"}; // Need to make it dynamic. i.e, have to get it from DB
            DataSet ds = DB.GetDS("SELECT * FROM GEN_importExcel_INB_Parameters WHERE Category='INB' AND IsActive=1", false);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cbListColumns.Items.Clear();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ListItem cbItem = new ListItem();
                    cbItem.Text = ds.Tables[0].Rows[i]["Param_Name"].ToString();
                    if (ds.Tables[0].Rows[i]["IsMandatory"].ToString() == "False")
                    {
                        cbItem.Enabled = true;
                    }
                    else
                        cbItem.Enabled = false;
                    cbItem.Selected = true;
                    cbListColumns.Items.Add(cbItem);
                }
            }

        }

        string ColumnData;
        protected void btnGetTemplate_Click(object sender, EventArgs e)
        {
            // List<string> _lstSelected = new List<string>();
            ColumnData = "";
            StringBuilder _sb = new StringBuilder();
            //_sb.Append("<table><tr>");
            foreach (ListItem item in cbListColumns.Items)
            {
                if (item.Selected)
                {
                    // _sb.Append("<th>"+item.ToString()+"</th>");
                    // _lstSelected.Add(item.ToString());   
                    if ((item.ToString().ToLower()).Contains("date"))
                    { item.Value += "(dd/MM/yyyy)"; }
                    ColumnData = ColumnData + "," + item.Value;
                }
            }
            // _sb.Append("</tr></table>");
            // ViewState["ColumnData"] = ColumnData.Remove(0,1);
            //DownloadTemplate(_sb.ToString());
            DownloadTemplate(ColumnData.ToString().Substring(1));

        }

        private void DownloadTemplate(string _sColumns)
        {
            string FileName = "Template_INB_" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss_tt");
            //HttpResponse response = HttpContext.Current.Response;
            //response.Clear();
            //response.ClearHeaders();
            //response.ClearContent();
            //response.Charset = Encoding.UTF8.WebName;
            //response.AddHeader("content-disposition", "attachment; filename=" + FileName + ".xls");
            //response.AddHeader("Content-Type", "application/Excel");
            //response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";// "application/vnd.xlsx";
            //response.Write(_sColumns);            
            //response.End();


            using (XLWorkbook wb = new XLWorkbook())
            {
                DataTable dt = new DataTable();

                string[] colData = _sColumns.ToString().Split(',');

                for (int i = 0; i < colData.Length; i++)
                {
                    dt.Columns.Add(colData[i].ToString());
                }
                var WS = wb.Worksheets.Add(dt, "InwardImport");


                wb.Worksheet(1).Rows(1, 1).Style.Fill.BackgroundColor = XLColor.FromHtml("219,229,241");
                wb.Worksheet(1).Rows(1, 1).Style.Font.FontColor = XLColor.RichBlack;
                wb.Worksheet(1).Cells("A1").Style.Font.FontColor = XLColor.Red;
                wb.Worksheet(1).Cells("B1").Style.Font.FontColor = XLColor.Red;
                //wb.Worksheet(1).Cells("C1").Style.Font.FontColor = XLColor.Red;
                //wb.Worksheet(1).Cells("D1").Style.Font.FontColor = XLColor.Red;
                //wb.Worksheet(1).Cells("D1").Value = "PODate (dd-mm-yyyy)";
                wb.Worksheet(1).Cells("E1").Style.Font.FontColor = XLColor.Red;
                wb.Worksheet(1).Cells("F1").Style.Font.FontColor = XLColor.Red;
                wb.Worksheet(1).Cells("G1").Style.Font.FontColor = XLColor.Red;
                wb.Worksheet(1).Cells("H1").Style.Font.FontColor = XLColor.Red;
                wb.Worksheet(1).Cells("N1").Style.Font.FontColor = XLColor.Red;
                wb.Worksheet(1).Cells("O1").Style.Font.FontColor = XLColor.Red;
                wb.Worksheet(1).Cells("P1").Style.Font.FontColor = XLColor.Red;
                wb.Worksheet(1).Cells("Q1").Style.Font.FontColor = XLColor.Red;
                //wb.Worksheet(1).Cells("R1").Style.Font.FontColor = XLColor.Red;
                wb.Worksheet(1).Columns("D").Style.NumberFormat.Format = "@";
                wb.Worksheet(1).Columns("I").Style.NumberFormat.Format = "@";
                wb.Worksheet(1).Columns("J").Style.NumberFormat.Format = "@";
                wb.Worksheet(1).Columns("O").Style.NumberFormat.Format = "@";
                wb.Worksheet(1).Columns("O").Style.NumberFormat.Format = "@";


                // wb.Worksheet(1).Columns[4].num


                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }



            /* Microsoft.Office.Interop.Excel.ApplicationClass XcelApp = new Microsoft.Office.Interop.Excel.ApplicationClass();*/
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                string saveFolder = Server.MapPath("Excels"); //Pick a folder on your machine to store the uploaded files
                DeleteExistingFiles(saveFolder);
                string filePath = Path.Combine(saveFolder, fuExcel.FileName);
                fuExcel.SaveAs(filePath);
                DataTable DtExcel = GetDataTableFromExcel(filePath, Path.GetExtension(fuExcel.FileName));
                ImportDataFromExcel(DtExcel, Path.GetExtension(fuExcel.FileName));
            }
            catch (Exception ex)
            {

            }
        }

        private DataTable GetDataTableFromExcel(string FilePath, string Extension)
        {
            DataTable dtExcelRecords = null;
            try
            {
                string conStr = "";
                switch (Extension)
                {
                    case ".xls": //Excel 97-03
                        conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FilePath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                        break;
                    case ".xlsx": //Excel 07
                        conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FilePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                        break;
                }
                conStr = String.Format(conStr, FilePath, 1);

                System.Data.OleDb.OleDbConnection connExcel = new System.Data.OleDb.OleDbConnection(conStr);
                System.Data.OleDb.OleDbCommand cmdExcel = new System.Data.OleDb.OleDbCommand();
                System.Data.OleDb.OleDbDataAdapter oda = new System.Data.OleDb.OleDbDataAdapter();
                dtExcelRecords = new DataTable();
                cmdExcel.Connection = connExcel;
                connExcel.Open();
                DataTable dtExcelSchema;
                dtExcelSchema = connExcel.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
                string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                if (SheetName.Contains("FilterDatabase"))
                {
                    SheetName = dtExcelSchema.Rows[2]["TABLE_NAME"].ToString();
                }

                cmdExcel.CommandText = "SELECT * From [Sheet1$]"; //cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
                oda.SelectCommand = cmdExcel;
                oda.Fill(dtExcelRecords);
                connExcel.Close();
            }
            catch (Exception ex)
            {
                string error = "Please attach valid excel sheet";
                string isError = "true";
                DisplayError(error, isError);


            }
            return dtExcelRecords;
        }


        private void DeleteExistingFiles(string _sFilePath)
        {
            if ((System.IO.File.Exists(_sFilePath)))
            {
                System.IO.File.Delete(_sFilePath);
            }
        }

        public void ImportDataFromExcel(DataTable dtExcelData, string excelFilePath)
        {

            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                // string sexcelconnectionstring;
                // string ssqlconnectionstring = ConfigurationManager.AppSettings["DBConn"].ToString();
                // sexcelconnectionstring = @"provider=microsoft.jet.oledb.4.0;data source=" + excelFilePath +
                //";extended properties=" + "\"excel 8.0;hdr=yes;\"";


                // //if (fileExtension == ".xls")
                // //{
                //        // sexcelconnectionstring = "provider=Microsoft.ACE.OLEDB.12.0;Data Source="
                //        // + excelFilePath + ";Extended Properties = \"Excel 8.0;HDR=YES\"";
                //// }
                // //else if (fileExtension == ".xlsx")
                //// {
                // //    sexcelconnectionstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                //   //    fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                //// }



                // System.Data.OleDb.OleDbConnection MyConnection;
                // System.Data.DataSet DtSet;
                // System.Data.OleDb.OleDbDataAdapter MyAdapter;

                // MyConnection = new System.Data.OleDb.OleDbConnection(sexcelconnectionstring);

                // MyConnection.Open();
                // DataTable Sheets = MyConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);


                // MyAdapter = new System.Data.OleDb.OleDbDataAdapter("select * from ["+Sheets.Rows[0]["TABLE_NAME"].ToString()+"]", MyConnection);
                // MyAdapter.TableMappings.Add("Table", "TestTable");
                // DtSet = new System.Data.DataSet();
                // MyAdapter.Fill(DtSet);               
                // MyConnection.Close();




                if (dtExcelData != null && dtExcelData.Rows.Count > 0)
                {
                    StringBuilder _sb = new StringBuilder();
                    string _sColumns = GetColumns(dtExcelData);
                    _sb.Append(" INSERT INTO  TMP_Inb_Imports(" + _sColumns + ") ");
                    for (int i = 0; i < dtExcelData.Rows.Count; i++)
                    {
                        //_sb.Append("INSERT INTO  TMP_Inb_Imports(" + _sColumns + ") VALUES(" + GetRow(DtSet.Tables[0], i) + ");");
                        if (i == 0)
                            _sb.Append(" SELECT " + GetRow(dtExcelData, i));
                        else
                            _sb.Append(" UNION SELECT " + GetRow(dtExcelData, i));
                    }
                    int _iResult = Convert.ToInt32(DB.GetDS(_sb.ToString() + " SELECT @@ROWCOUNT", false).Tables[0].Rows[0][0].ToString());
                    if (_iResult > 0)
                    {
                        int res = DB.GetSqlN("EXEC [dbo].[sp_INB_BulkInsert_Inward]");
                        string error;
                        string isError;
                        if (res == 1)
                        {
                            error = "Excel uploaded successfully. <br>" + _iResult + " Record(s) updated";
                            isError = "false";
                        }
                        else
                        {
                            error = "Excel uploading failed. <br> 0 Record(s) updated";
                            isError = "true";
                            DB.GetSqlS("UPDATE TMP_Inb_Imports SET IsActive=0 WHERE User_ID=" + cp.UserID);
                        }
                        DisplayError(error, isError);
                    }
                    else
                    {
                        string error = "Excel uploading failed. <br> 0 Record(s) updated";
                        string isError = "true";
                        DisplayError(error, isError);
                    }
                }
                else
                {
                    string error = "Excel uploading failed. <br> 0 Record(s) updated";
                    string isError = "true";
                    DisplayError(error, isError);
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message.ToString();
                //string _sexMsg = ex.Message.ToString().Remove(ex.Message.Length > 145 ? 145 : ex.Message.Length - 1);
                string error = "Excel uploading failed. <br> 0 Record(s) updated";
                string isError = "true";
                DisplayError(error, isError);
            }
            finally
            {
                DeleteInsertedRecords();
            }
        }

        private string GetColumns(DataTable dt)
        {
            string TableColumns = "[User_ID]";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                TableColumns = TableColumns + "," + dt.Columns[i];
            }
            return TableColumns + ", IsActive";
        }
        private string GetRow(DataTable dt, int index)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            string _sRow = cp.UserID.ToString();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                _sRow = _sRow + ",'" + dt.Rows[index][i].ToString() + "'";
            }
            return _sRow + ", 1";
        }

        private void DeleteInsertedRecords()
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            DB.GetSqlS("UPDATE TMP_Inb_Imports SET IsActive=0 WHERE User_ID=" + cp.UserID);
        }

        private void DisplayError(string error, string isError)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);
        }

        //public void LoadExcelData()
        //{
        //    DataSet ds = DB.GetDS("select * from TMP_Inb_Imports", false);
        //    gvExcelData.DataSource = ds;
        //    gvExcelData.DataBind();
        //}

        //protected void gvExcelData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    gvExcelData.PageIndex = e.NewPageIndex;
        //    LoadExcelData();
        //}


        public class InboundImport
        {
            private string _TenantCode { get; set; }
            [XmlElement(IsNullable = true)]
            public string TenantCode
            {
                get { return string.IsNullOrEmpty(this._TenantCode) ? string.Empty : this._TenantCode; }
                set
                {
                    if (this._TenantCode != value)
                    {
                        this._TenantCode = value;
                    }
                }
            }
            private string _SupplierCode { get; set; }
            [XmlElement(IsNullable = true)]
            public string SupplierCode
            {
                get { return string.IsNullOrEmpty(this._SupplierCode) ? string.Empty : this._SupplierCode; }
                set
                {
                    if (this._SupplierCode != value)
                    {
                        this._SupplierCode = value;
                    }
                }
            }

            [XmlElement(IsNullable = true)]
            public string PONumber
            {
                get { return string.IsNullOrEmpty(this.PONO) ? string.Empty : this.PONO; }
                set
                {
                    if (this.PONO != value)
                    {
                        this.PONO = value;
                    }
                }
            }
            private string PONO;
            //  public string PONumber { get; set; }
            private string _PODate { get; set; }
            [XmlElement(IsNullable = true)]
            public string PODate
            {
                get { return string.IsNullOrEmpty(this._PODate) ? string.Empty : this._PODate; }
                set
                {
                    if (this._PODate != value)
                    {
                        this._PODate = value;
                    }
                }
            }
            public string POTypeCode { get; set; }



            [XmlElement(IsNullable = true)]
            public string LineNumber
            {
                get { return string.IsNullOrEmpty(this._LineNumber) ? string.Empty : this._LineNumber; }
                set
                {
                    if (this._LineNumber != value)
                    {
                        this._LineNumber = value;
                    }
                }
            }
            private string _LineNumber { get; set; }


            // public string ItemCode { get; set; }
            [XmlElement(IsNullable = true)]
            public string PartNo
            {
                get { return string.IsNullOrEmpty(this._PartNo) ? string.Empty : this._PartNo; }
                set
                {
                    if (this._PartNo != value)
                    {
                        this._PartNo = value;
                    }
                }
            }
            private string _PartNo { get; set; }
            [XmlElement(IsNullable = true)]
            public string UoM
            {
                get { return string.IsNullOrEmpty(this._UoM) ? string.Empty : this._UoM; }
                set
                {
                    if (this._UoM != value)
                    {
                        this._UoM = value;
                    }
                }
            }
            private string _UoM { get; set; }
            [XmlElement(IsNullable = true)]
            public string UOMQty
            {
                get { return string.IsNullOrEmpty(this._UoMQty) ? string.Empty : this._UoMQty; }
                set
                {
                    if (this._UoMQty != value)
                    {
                        this._UoMQty = value;
                    }
                }
            }
            private string _UoMQty { get; set; }

            private string _MfgDate { get; set; }
            [XmlElement(IsNullable = true)]
            public string MfgDate
            {
                get { return string.IsNullOrEmpty(this._MfgDate) ? string.Empty : this._MfgDate; }
                set
                {
                    if (this._MfgDate != value)
                    {
                        this._MfgDate = value;
                    }
                }
            }
            private string _ExpDate { get; set; }
            [XmlElement(IsNullable = true)]
            public string ExpDate
            {
                get { return string.IsNullOrEmpty(this._ExpDate) ? string.Empty : this._ExpDate; }
                set
                {
                    if (this._ExpDate != value)
                    {
                        this._ExpDate = value;
                    }
                }
            }
            //  public string MfgDate { get; set; }
          //  public string ExpDate { get; set; }
            public string BatchNo { get; set; }
            public string SerialNo { get; set; }
            public string ProjRefNo { get; set; }
            private string _InvoiceNo { get; set; }
            [XmlElement(IsNullable = true)]
            public string InvoiceNo
            {
                get { return string.IsNullOrEmpty(this._InvoiceNo) ? string.Empty : this._InvoiceNo; }
                set
                {
                    if (this._InvoiceNo != value)
                    {
                        this._InvoiceNo = value;
                    }
                }
            }
            private string _InvoiceDate { get; set; }
            [XmlElement(IsNullable = true)]
            public string InvoiceDate
            {
                get { return string.IsNullOrEmpty(this._InvoiceDate) ? string.Empty : this._InvoiceDate; }
                set
                {
                    if (this._InvoiceDate != value)
                    {
                        this._InvoiceDate = value;
                    }
                }
            }
            private string _InvQty { get; set; }
            public string InvoiceQuantity
            {
                get { return string.IsNullOrEmpty(this._InvQty) ? string.Empty : this._InvQty; }
                set
                {
                    if (this._InvQty != value)
                    {
                        this._InvQty = value;
                    }
                }
            }
            private string WHCode { get; set; }
            public string WarehouseCode
            {
                get { return string.IsNullOrEmpty(this.WHCode) ? string.Empty : this.WHCode; }
                set
                {
                    if (this.WHCode != value)
                    {
                        this.WHCode = value;
                    }
                }
            }
            private string _POQty { get; set; }
            public string POQty
            {
                get { return string.IsNullOrEmpty(this._POQty) ? string.Empty : this._POQty; }
                set
                {
                    if (this._POQty != value)
                    {
                        this._POQty = value;
                    }
                }
            }
            public string MRP { get; set; }

            public string AccountId { get; set; }


            public int HUSize { get; set; }
        }

        [WebMethod]
        public static string getPONumber(string TenantCode)
        {
            DataSet ds = DB.GetDS("EXEC [dbo].[SP_GetNewPO_ByTenantCode] @TenantCode ='" + TenantCode +"'", false);
            return JsonConvert.SerializeObject(ds);
        }

        [WebMethod]
        public static ExcelResult GetInwardImport(List<InboundImport> inblst)
        {
            CustomPrincipal = HttpContext.Current.User as CustomPrincipal;
            List<InboundImport> chklst = new List<InboundImport>();
            ExcelResult result = new ExcelResult();
            result.Status = -1;
            chklst = inblst.FindAll(item => (item.POQty.Trim() == "" || item.UOMQty.Trim() == "" || item.WarehouseCode.Trim() == "" || item.InvoiceDate.Trim() == "" || item.InvoiceNo.Trim() == "" || item.InvoiceQuantity.Trim() == "" || item.UoM.Trim() == "" || item.PartNo.Trim() == "" || item.LineNumber.Trim() == "" || item.PODate.Trim() == "" || item.PONumber.Trim() == "" || item.TenantCode.Trim() == "" || item.SupplierCode.Trim() == "")).ToList();
            
            if (inblst.Count == 0)
            {
                result.Result = "Error : Please import valid excel ";

                return result;
            }
            else if (chklst.Count > 0)
            {
                result.Result = "Error : Few mandatory Fields are not entered ";

                return result;
            }
            chklst = null;
            try
            {
                chklst = inblst.FindAll(item => (Convert.ToInt32(item.InvoiceQuantity) <= 0)).ToList();
            }
            catch (Exception e)
            {

                result.Result = "Error : Enter valid Invoice Qty. in Excel ";
                return result;
            }
            if (chklst.Count > 0)
            {

                result.Result = "Error :  Invoice Qty. should not contain 0’s and Negatives";
                return result;

            }
            chklst = null;
            try
            {
                chklst = inblst.FindAll(item => (Convert.ToInt32(item.UOMQty) <= 0)).ToList();
            }
            catch (Exception e)
            {

                result.Result = "Error : Enter valid UOM Qty. in Excel ";
                return result;
            }
            if (chklst.Count > 0)
            {

                result.Result = "Error :  UOM Qty. should not contain 0’s and Negatives";
                return result;

            }

            chklst = null;
            try
            {
                chklst = inblst.FindAll(item => (Convert.ToInt32(item.POQty) <= 0)).ToList();
            }
            catch (Exception e)
            {

                result.Result = "Error : Enter valid PO Qty. in Excel ";
                return result;
            }
            if (chklst.Count > 0)
            {

                result.Result = "Error :  PO Qty. should not contain 0’s and Negatives";
                return result;

            }

            chklst = null;
            try
            {
                chklst = inblst.FindAll(item => (Convert.ToInt32(item.LineNumber) <= 0)).ToList();
            }
            catch (Exception e)
            {

                result.Result = "Error : Enter valid Line Number  in Excel ";
                return result;
            }
            if (chklst.Count > 0)
            {

                result.Result = "Error :  Line Number should not contain 0’s and Negatives";
                return result;

            }

            //chklst = null;
            //chklst = inblst.FindAll(item => (item.PONumber.Length != 9)).ToList();
            //if (chklst.Count > 0)
            //{
            //    result.Result = "Error :   PO No. should contain 9 digits";

            //    return result;
            //}


            //chklst = null;
            //try
            //{
            //    chklst = inblst.FindAll(item => (Convert.ToInt32(item.POQty) <= Convert.ToInt32(item.InvoiceQuantity))).ToList();
            //}
            //catch (Exception e)
            //{

            //    result.Result = "Error : Enter valid data in Excel ";
            //    return result;
            //}
            //if (chklst.Count > 0)
            //{

            //    result.Result = "Error :  Invoice Qty. should not greater than PO Qty. ";
            //    return result;

            //}

            string error = "";
            string BatchNo = "";
            string MRP = "";
            string SerialNo = "";
            string ProjectRefNo = "";
            string MfgDate = "";
            string ExpDate = "";
            string itemcode = "";
            string mmid = "";
            string WarehouseCode = "";
            string TenantCode = "";
            string SupplierCOde = "";
            string invoicedate = "";
            string podate = "";
            ListtoDataTableConverter converter = new ListtoDataTableConverter();
            int rownumber = 0;
            DataTable dt = converter.ToDataTable(inblst);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                BatchNo = dt.Rows[i]["BatchNo"].ToString().Trim() != "" ? dt.Rows[i]["BatchNo"].ToString().Trim() : null;
                SerialNo = dt.Rows[i]["SerialNo"].ToString().Trim() != "" ? dt.Rows[i]["SerialNo"].ToString() : null;
                ProjectRefNo = dt.Rows[i]["ProjRefNo"].ToString().Trim() != "" ? dt.Rows[i]["ProjRefNo"].ToString().Trim() : null;
                MfgDate = dt.Rows[i]["MfgDate"].ToString().Trim() != "" ? dt.Rows[i]["MfgDate"].ToString().Trim() : null;
                ExpDate = dt.Rows[i]["ExpDate"].ToString().Trim() != "" ? dt.Rows[i]["ExpDate"].ToString().Trim() : null;
                itemcode = dt.Rows[i]["PartNo"].ToString().Trim();
                TenantCode = dt.Rows[i]["TenantCode"].ToString().Trim();
                SupplierCOde = dt.Rows[i]["SupplierCode"].ToString().Trim();
                WarehouseCode = dt.Rows[i]["WarehouseCode"].ToString().Trim();
                invoicedate = dt.Rows[i]["InvoiceDate"].ToString();
                podate = dt.Rows[i]["PODate"].ToString();
                MRP = dt.Rows[i]["MRP"].ToString();
                try
                {
                    if (MRP.Trim() != "")
                    {
                        int MRPValue = Convert.ToInt32(MRP);
                        if (Convert.ToInt32(MRP) <= 0)
                        {
                            result.Result = "Error : Please Enter Valid MRP at row " + rownumber;
                            return result;
                        }
                    }


                }
                catch (Exception e)
                {

                    result.Result = "Error : Please Enter Valid MRP at row " + rownumber;
                    return result;
                }
                rownumber = i + 1;
                try
                {
                    // invoicedate = DateTime.ParseExact(invoicedate.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                    //DateTime date;
                    //bool success = DateTime.TryParse(invoicedate, out date);
                    //invoicedate = date.ToString("MM/dd/yyyy");
                    invoicedate = CommonLogic.Formateddate(invoicedate);
                }
                catch (Exception ex)
                {
                    if (ex.Message == "Year, Month, and Day parameters describe an un-representable DateTime.")
                    {
                        result.Result = "Invalid date is provided at Invoicedate";
                        return result;
                    }
                    result.Status = -1;
                    result.Result = "Please provide Invoice Date format as dd/MM/yyyy";
                    return result;
                }
                try
                {

                    //  podate = DateTime.ParseExact(podate.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                    //DateTime date;
                    //bool success = DateTime.TryParse(podate,out date);
                    //podate=date.ToString("MM/dd/yyyy");

                    podate = CommonLogic.Formateddate(podate);
                }
                catch (Exception ex)
                {
                    if (ex.Message == "Year, Month, and Day parameters describe an un-representable DateTime.")
                    {
                        result.Result = "Invalid date is provided at POdate";
                        return result;
                    }
                    result.Status = -1;
                    result.Result = "Please provide PO Date format as dd/MM/yyyy";
                    return result;
                }
                inblst[i].InvoiceDate = invoicedate;
                inblst[i].PODate = podate;
                string query = "SELECT MaterialMasterID as N FROM MMT_MaterialMaster WHERE MCode in(" + DB.SQuote(itemcode.Trim()) + ")";
                int MaterialMasterid = DB.GetSqlN(query);
                if (MaterialMasterid == 0)
                {
                    result.Result = "Error : Please Enter Mandatory Item Code at row " + rownumber;
                    return result;
                }
                String mfgdate = null;
                if (dt.Rows[i]["MfgDate"].ToString() != "")
                {
                    try
                    {
                        mfgdate = dt.Rows[i]["MfgDate"].ToString();
                        // mfgdate = DateTime.ParseExact((dt.Rows[i]["MfgDate"]).ToString().Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                        //DateTime date;
                        //bool success = DateTime.TryParse(dt.Rows[i]["MfgDate"].ToString(), out date);
                        //mfgdate = date.ToString("MM/dd/yyyy");
                        mfgdate = CommonLogic.Formateddate(mfgdate);
                    }
                    catch (Exception ex)
                    {

                        if (ex.Message == "Year, Month, and Day parameters describe an un-representable DateTime.")
                        {
                            result.Result = "Invalid date is provided at mfgdate";
                            return result;
                        }
                        result.Result = "Please provide Mfg Date format as dd/MM/yyyy at row " + rownumber;
                        return result;
                    }

                }
                string Expdate = null;
                if (dt.Rows[i]["ExpDate"].ToString() != "")
                {
                    try
                    {
                        Expdate = dt.Rows[i]["ExpDate"].ToString();
                        //  Expdate = DateTime.ParseExact((dt.Rows[i]["ExpDate"]).ToString().Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                        //DateTime date;
                        //bool success = DateTime.TryParse(dt.Rows[i]["ExpDate"].ToString(), out date);
                        //Expdate = date.ToString("MM/dd/yyyy");
                        Expdate = CommonLogic.Formateddate(Expdate);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message == "Year, Month, and Day parameters describe an un-representable DateTime.")
                        {
                            result.Result = "Invalid date is provided at Expdate";
                            return result;
                        }
                        result.Result = "Please provide  Exp Date format as dd/MM/yyyy at row" + rownumber;
                        return result;
                    }

                }
                string Isrequired = "0";
                string Parametername = "";
                string Query = "select mm.MaterialMaster_MaterialStorageParameterID,mm.MaterialMasterID,mm.IsRequired,mm.MaterialStorageParameterID,msp.ParameterName from MMT_MaterialMaster_MSP mm join MMT_MSP msp on mm.MaterialStorageParameterID = msp.MaterialStorageParameterID where mm.MaterialMasterID in (" + MaterialMasterid + ")";
                DataSet ds = DB.GetDS(Query, false);
                if (ds.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        Isrequired = row["IsRequired"].ToString();
                        Parametername = row["ParameterName"].ToString();

                        if (ds.Tables != null)
                        {
                            if (Parametername == "MfgDate" && Isrequired == "True")
                            {
                                if (MfgDate == "" || MfgDate == null)
                                {
                                    result.Result = "Error : Please Enter Mandatory Mfg. Date at row " + rownumber;
                                    return result;
                                }

                            }
                            else if (Parametername == "ExpDate" && Isrequired == "True")
                            {
                                if (ExpDate == "" || ExpDate == null)
                                {

                                    result.Result = "Error : Please Enter Mandatory Exp. Date at row " + rownumber;
                                    return result;
                                }

                            }


                            else if (Parametername == "BatchNo" && Isrequired == "True")
                            {
                                if (BatchNo == "" || BatchNo == null)
                                {

                                    result.Result = "Error : Please Enter Mandatory Batch No. at row " + rownumber;
                                    return result;
                                }

                            }
                            else if (Parametername == "MRP" && Isrequired == "True")
                            {
                                if (MRP == "" || MRP == null)
                                {

                                    result.Result = "Error : Please Enter Mandatory MRP at row " + rownumber;
                                    return result;
                                }

                            }

                            else if (Parametername == "ProjectRefNo" && Isrequired == "True")
                            {
                                if (ProjectRefNo == "" || ProjectRefNo == null)
                                {

                                    result.Result = "Error : Please Enter Mandatory Project Ref. No. at row " + rownumber;
                                    return result;
                                }

                            }
                            // added by lalitha on 13/02/2019 for checking validation
                            else if (Parametername == "SerialNo" && Isrequired == "True")
                            {
                                if (SerialNo == "" || SerialNo == null)
                                {

                                    result.Result = "Error : Please Enter Mandatory SerialNo at row " + rownumber;
                                    return result;
                                }

                            }


                        }
                    }
                }
                inblst[i].MfgDate = mfgdate;
                inblst[i].ExpDate = Expdate;

                inblst[i].AccountId = CustomPrincipal.AccountID.ToString();  // added by lalitha on 13/03/2019

                if (SerialNo != "" && SerialNo != null)
                {
                    //<!------------------Procedure Conversion------------------------>
                    // int serialnocount = DB.GetSqlN("select COUNT(SerialNo) as N from ORD_SupplierInvoiceDetails where PODetailsID="+ hifPODetailsID.Value+" and isactive=1 and isdeleted=0 and SerialNo='"+txtSerialNo.Text.Trim()+ "' and SupplierInvoiceDetailsID !="+ ltSupplierInvoiceDetailsID.Text);
                    //int serialnocount = DB.GetSqlN("Exec [dbo].[USP_SupplierInvoiceSerialNOCount]  @PODetailsID=" + hifPODetailsID.Value + ", @SupplierInvoiceDetailsID=" + ltSupplierInvoiceDetailsID.Text + ", @SerialNo='" + txtSerialNo.Text.Trim() + "'");
                    //if (serialnocount > 0)
                    //{
                    //    result.Result = "Serial No. already Exists";
                    //    return result;
                    //}
                    int count = DB.GetSqlN("Exec [dbo].[sp_GET_SERIAL_COUNT] @SerialNo='" + inblst[i].SerialNo + "',@MCode='" + inblst[i].PartNo + "'");
                    if (count > 0)
                    {
                        result.Result = "Serial No. already Exists";
                        return result;
                    }
                    inblst[i].InvoiceQuantity = "1";
                }
            }

            string xml = null;
            string s = string.Join("", inblst);
            var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            emptyNamepsaces.Add("", "");
            var settings = new XmlWriterSettings();
            using (StringWriter sw = new StringWriter())
            {
                XmlSerializer serialiser = new XmlSerializer(typeof(List<InboundImport>));
                serialiser.Serialize(sw, inblst, emptyNamepsaces);
                try
                {
                    xml = sw.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", " ");

                    string res = DB.GetSqlS("EXEC  [dbo].[sp_INB_Excel_BulkInsert_InwardImport] " + DB.SQuote(xml) + ",@UpdatedBy=" + CustomPrincipal.UserID.ToString());


                    if (res == "Succesfully Added")
                    {
                        result.Status = 1;
                        result.Result = "Succesfully Added";
                        return result;

                    }
                    else
                    {

                        result.Result = res;
                        return result;
                    }

                }
                catch (Exception e)
                {
                    result.Status = -2;
                    result.Result = e.Message;
                    return result;

                }
            }




        }
        public class ListtoDataTableConverter

        {

            public DataTable ToDataTable<T>(List<T> items)

            {

                DataTable dataTable = new DataTable(typeof(T).Name);

                //Get all the properties

                PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (PropertyInfo prop in Props)

                {

                    //Setting column names as Property names

                    dataTable.Columns.Add(prop.Name);

                }

                foreach (T item in items)

                {

                    var values = new object[Props.Length];

                    for (int i = 0; i < Props.Length; i++)

                    {

                        //inserting property values to datatable rows

                        values[i] = Props[i].GetValue(item, null);

                    }

                    dataTable.Rows.Add(values);

                }

                //put a breakpoint here and check datatable

                return dataTable;

            }

        }

        protected void resetError(string error, bool isError)
        {


            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);


        }
        public class ExcelResult
        {
            public int Status { get; set; }
            public string Result { get; set; }
        }
    }
}