using ClosedXML.Excel;
using MRLWMSC21Common;
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

namespace MRLWMSC21.mOutbound
{
    public partial class OdbImports : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        public static CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
        protected void PagePre_Init(object sender, EventArgs e)
        {
            Page.Theme = "Inbound";
            cp1 = HttpContext.Current.User as CustomPrincipal;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Outward Import");
                LoadExcelColumns();
            }

        }
        private void LoadExcelColumns()
        {
            //List<string> _list = new List<string>(){"SNo", "TenantCode", "CustomerCode","SONumber","SOQuantity","SOType","CurrencyCode",              
            //    "SORequestedBy", "SODate","SOCreatedOn", "SOCreatedBy", "CustomerPONumber", "CustomerPODate","CustomerPOQuantity", 
            //    "CustomerPOValue", "ItemCode", "UoM","UoMQuantity","UnitPrice",   "DocumentType","OBDDate","OBDRequestedBy",
            //    "BatchNo", "ExpDate", "MfgDate", "InvoiceNo"  }; // Need to make it dynamic. i.e, have to get it from DB
            DataSet ds = DB.GetDS("SELECT * FROM GEN_importExcel_OBD_Parameters WHERE Category='OBD' AND IsActive=1", false);
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
            // StringBuilder _sb = new StringBuilder();
            // _sb.Append("<table><tr>");
            foreach (ListItem item in cbListColumns.Items)
            {
                if (item.Selected)
                {
                    //_sb.Append("<th>" + item.ToString() + "</th>");
                    // _lstSelected.Add(item.ToString());   
                    ColumnData = ColumnData + "," + item.ToString();
                }
            }
            // _sb.Append("</tr></table>");
            //ViewState["ColumnData"] = ColumnData.Remove(0, 1);
            DownloadTemplate(ColumnData.ToString().Substring(1));

        }

        private void DownloadTemplate(string _sColumns)
        {
            string FileName = "Template_OBD_" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss_tt");
            //HttpResponse response = HttpContext.Current.Response;
            //response.Clear();
            //response.ClearHeaders();
            //response.ClearContent();
            //response.Charset = Encoding.UTF8.WebName;
            //response.AddHeader("content-disposition", "attachment; filename=" + FileName + ".xls");
            //response.AddHeader("Content-Type", "application/Excel");
            //response.ContentType = "application/vnd.xlsx";
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
                wb.Worksheets.Add(dt, "OutwardImport");

                wb.Worksheet(1).Rows(1, 1).Style.Fill.BackgroundColor = XLColor.FromHtml("219,229,241");
                wb.Worksheet(1).Rows(1, 1).Style.Font.FontColor = XLColor.RichBlack;
                wb.Worksheet(1).Cells("A1").Style.Font.FontColor = XLColor.Red;
                wb.Worksheet(1).Cells("B1").Style.Font.FontColor = XLColor.Red;
                wb.Worksheet(1).Cells("C1").Style.Font.FontColor = XLColor.Red;
                wb.Worksheet(1).Cells("D1").Style.Font.FontColor = XLColor.Red;
                wb.Worksheet(1).Cells("E1").Style.Font.FontColor = XLColor.Red;

                wb.Worksheet(1).Cells("F1").Style.Font.FontColor = XLColor.Red;
                wb.Worksheet(1).Cells("G1").Style.Font.FontColor = XLColor.Red;
                //wb.Worksheet(1).Cells("H1").Style.Font.FontColor = XLColor.Red;
                wb.Worksheet(1).Cells("I1").Style.Font.FontColor = XLColor.Red;
                wb.Worksheet(1).Cells("J1").Style.Font.FontColor = XLColor.Red;
                wb.Worksheet(1).Cells("K1").Style.Font.FontColor = XLColor.Red;
                wb.Worksheet(1).Cells("L1").Style.Font.FontColor = XLColor.Red;
                wb.Worksheet(1).Cells("M1").Style.Font.FontColor = XLColor.Red;
                wb.Worksheet(1).Cells("N1").Style.Font.FontColor = XLColor.Red;
                wb.Worksheet(1).Cells("O1").Style.Font.FontColor = XLColor.Red;

                wb.Worksheet(1).Columns("P").Style.NumberFormat.Format = "@";
                wb.Worksheet(1).Columns("Q").Style.NumberFormat.Format = "@";
                wb.Worksheet(1).Columns("R").Style.NumberFormat.Format = "@";
                wb.Worksheet(1).Columns("S").Style.NumberFormat.Format = "@";
                wb.Worksheet(1).Columns("T").Style.NumberFormat.Format = "@";
                wb.Worksheet(1).Columns("U").Style.NumberFormat.Format = "@";

                wb.Worksheet(1).Columns("V").Style.NumberFormat.Format = "@";
                wb.Worksheet(1).Columns("W").Style.NumberFormat.Format = "@";
                wb.Worksheet(1).Columns("X").Style.NumberFormat.Format = "@";
                wb.Worksheet(1).Columns("Y").Style.NumberFormat.Format = "@";
                wb.Worksheet(1).Columns("Z").Style.NumberFormat.Format = "@";
                wb.Worksheet(1).Columns("E").Style.NumberFormat.Format = "@";

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
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {

                string saveFolder = Server.MapPath("Excels"); //Pick a folder on your machine to store the uploaded files
                DeleteExistingFiles(saveFolder);
                string filePath = Path.Combine(saveFolder, fuExcel.FileName);
                fuExcel.SaveAs(filePath);
                //ImportDataFromExcel(filePath);
                DataTable DtExcel = GetDataTableFromExcel(filePath, Path.GetExtension(fuExcel.FileName));
                ImportDataFromExcel(DtExcel, filePath);
            }
            catch (Exception ex)
            {
                string error = "Please attach valid excel sheet";
                string isError = "true";
                DisplayError(error, isError);
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
                    SheetName = dtExcelSchema.Rows[1]["TABLE_NAME"].ToString();
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

            //System.IO.DirectoryInfo di = new DirectoryInfo(_sFolderPath);

            //foreach (FileInfo file in di.GetFiles())
            //{
            //    file.Delete();
            //}
            //foreach (DirectoryInfo dir in di.GetDirectories())
            //{
            //    dir.Delete(true);
            //}
        }

        public void ImportDataFromExcel(DataTable dtExcelData, string excelFilePath)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {

                // string ssqlconnectionstring = ConfigurationManager.AppSettings["DBConn"].ToString();
                // string sexcelconnectionstring = @"provider=microsoft.jet.oledb.4.0;data source=" + excelFilePath +
                //";extended properties=" + "\"excel 8.0;hdr=yes;\"";

                // System.Data.OleDb.OleDbConnection MyConnection;
                // System.Data.DataSet DtSet;
                // System.Data.OleDb.OleDbDataAdapter MyAdapter;

                // MyConnection = new System.Data.OleDb.OleDbConnection(sexcelconnectionstring);
                // MyConnection.Open();
                // DataTable Sheets = MyConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                // MyAdapter = new System.Data.OleDb.OleDbDataAdapter("select * from [" + Sheets.Rows[0]["TABLE_NAME"].ToString() + "]", MyConnection);
                // MyAdapter.TableMappings.Add("Table", "TestTable");
                // DtSet = new System.Data.DataSet();
                // MyAdapter.Fill(DtSet);
                // MyConnection.Close();




                if (dtExcelData != null && dtExcelData.Rows.Count > 0)
                {
                    StringBuilder _sb = new StringBuilder();
                    string _sColumns = GetColumns(dtExcelData);
                    _sb.Append(" INSERT INTO  TMP_Obd_Imports(" + _sColumns + ") ");
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
                        int res = DB.GetSqlN("EXEC [dbo].[sp_OBD_BulkInsert_Outward]");
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

        private void DisplayError(string error, string isError)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);
        }
        private void DeleteInsertedRecords()
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            DB.GetSqlS("UPDATE TMP_Obd_Imports SET IsActive=0 WHERE User_ID=" + cp.UserID);
        }

        public class OutboundImport
        {

            //public string TenantCode { get; set; }
            [XmlElement(IsNullable = true)]
            public string TenantCode
            {
                get { return string.IsNullOrEmpty(this.Tenant) ? string.Empty : this.Tenant; }
                set
                {
                    if (this.Tenant != value)
                    {
                        this.Tenant = value;
                    }
                }
            }
            private string Tenant;
            // public string WHcode { get; set; }
            [XmlElement(IsNullable = true)]
            public string WHcode
            {
                get { return string.IsNullOrEmpty(this.WHCODE) ? string.Empty : this.WHCODE; }
                set
                {
                    if (this.WHCODE != value)
                    {
                        this.WHCODE = value;
                    }
                }
            }
            private string WHCODE;
            // public string CustomerCode { get; set; }
            [XmlElement(IsNullable = true)]
            public string CustomerCode
            {
                get { return string.IsNullOrEmpty(this.Customercode) ? string.Empty : this.Customercode; }
                set
                {
                    if (this.Customercode != value)
                    {
                        this.Customercode = value;
                    }
                }
            }
            private string Customercode;

            [XmlElement(IsNullable = true)]
            public string CustomerName
            {
                get { return string.IsNullOrEmpty(this.Customername) ? string.Empty : this.Customername; }
                set
                {
                    if (this.Customername != value)
                    {
                        this.Customername = value;
                    }
                }
            }
            private string Customername;
            [XmlElement(IsNullable = true)]
            public string Email
            {
                get { return string.IsNullOrEmpty(this.email) ? string.Empty : this.email; }
                set
                {
                    if (this.email != value)
                    {
                        this.email = value;
                    }
                }
            }
            private string email;
            [XmlElement(IsNullable = true)]
            public string Zip
            {
                get { return string.IsNullOrEmpty(this.zip) ? string.Empty : this.zip; }
                set
                {
                    if (this.zip != value)
                    {
                        this.zip = value;
                    }
                }
            }
            private string zip;
            [XmlElement(IsNullable = true)]
            public string CustomerPhoneno
            {
                get { return string.IsNullOrEmpty(this.Customerphoneno) ? string.Empty : this.Customerphoneno; }
                set
                {
                    if (this.Customerphoneno != value)
                    {
                        this.Customerphoneno = value;
                    }
                }
            }
            private string Customerphoneno;
            [XmlElement(IsNullable = true)]
            public string CustomerAddress
            {
                get { return string.IsNullOrEmpty(this.Customeraddress) ? string.Empty : this.Customeraddress; }
                set
                {
                    if (this.Customeraddress != value)
                    {
                        this.Customeraddress = value;
                    }
                }
            }
            private string Customeraddress;

            // public string InvoiceNo { get; set; }
            [XmlElement(IsNullable = true)]
            public string InvoiceNo
            {
                get { return string.IsNullOrEmpty(this.Invoice) ? string.Empty : this.Invoice; }
                set
                {
                    if (this.Invoice != value)
                    {
                        this.Invoice = value;
                    }
                }
            }
            private string Invoice;
            // public string SONumber { get; set; }
            [XmlElement(IsNullable = true)]
            public string SONumber
            {
                get { return string.IsNullOrEmpty(this.SONO) ? string.Empty : this.SONO; }
                set
                {
                    if (this.SONO != value)
                    {
                        this.SONO = value;
                    }
                }
            }
            private string SONO;
            //public string SODate { get; set; }
            [XmlElement(IsNullable = true)]
            public string SODate
            {
                get { return string.IsNullOrEmpty(this.SODATE) ? string.Empty : this.SODATE; }
                set
                {
                    if (this.SODATE != value)
                    {
                        this.SODATE = value;
                    }
                }
            }
            private string SODATE;
            // public string SOQuantity { get; set; }
            [XmlElement(IsNullable = true)]
            public string SOQuantity
            {
                get { return string.IsNullOrEmpty(this.SOQty) ? string.Empty : this.SOQty; }
                set
                {
                    if (this.SOQty != value)
                    {
                        this.SOQty = value;
                    }
                }
            }
            private string SOQty;
            //public string ItemCode { get; set; }
            //[XmlElement(IsNullable = true)]
            //public string ItemCode
            //{
            //    get { return string.IsNullOrEmpty(this.Item) ? string.Empty : this.Item; }
            //    set
            //    {
            //        if (this.Item != value)
            //        {
            //            this.Item = value;
            //        }
            //    }
            //}
            //private string Item;


            [XmlElement(IsNullable = true)]
            public string PartNo
            {
                get { return string.IsNullOrEmpty(this.PartNum) ? string.Empty : this.PartNum; }
                set
                {
                    if (this.PartNum != value)
                    {
                        this.PartNum = value;
                    }
                }
            }
            private string PartNum;


            //public string LineNumber { get; set; }
            [XmlElement(IsNullable = true)]
            public string LineNumber
            {
                get { return string.IsNullOrEmpty(this.LineNo) ? string.Empty : this.LineNo; }
                set
                {
                    if (this.LineNo != value)
                    {
                        this.LineNo = value;
                    }
                }
            }
            private string LineNo;
            // public string UoM { get; set; }
            [XmlElement(IsNullable = true)]
            public string UoM
            {
                get { return string.IsNullOrEmpty(this.UOM) ? string.Empty : this.UOM; }
                set
                {
                    if (this.UOM != value)
                    {
                        this.UOM = value;
                    }
                }
            }
            private string UOM;
            //public string UoMQuantity { get; set; }
            [XmlElement(IsNullable = true)]
            public string UoMQuantity
            {
                get { return string.IsNullOrEmpty(this.UOMQty) ? string.Empty : this.UOMQty; }
                set
                {
                    if (this.UOMQty != value)
                    {
                        this.UOMQty = value;
                    }
                }
            }
            private string UOMQty;
            //public string MfgDate { get; set; }
            [XmlElement(IsNullable = true)]
            public string MfgDate
            {
                get { return string.IsNullOrEmpty(this.MFGDate) ? string.Empty : this.MFGDate; }
                set
                {
                    if (this.MFGDate != value)
                    {
                        this.MFGDate = value;
                    }
                }
            }
            private string MFGDate;
            //public string ExpDate { get; set; }
            [XmlElement(IsNullable = true)]
            public string ExpDate
            {
                get { return string.IsNullOrEmpty(this.ExpDATE) ? string.Empty : this.ExpDATE; }
                set
                {
                    if (this.ExpDATE != value)
                    {
                        this.ExpDATE = value;
                    }
                }
            }
            private string ExpDATE;
            //public string BatchNo { get; set; }
            [XmlElement(IsNullable = true)]
            public string BatchNo
            {
                get { return string.IsNullOrEmpty(this.Batchno) ? string.Empty : this.Batchno; }
                set
                {
                    if (this.Batchno != value)
                    {
                        this.Batchno = value;
                    }
                }
            }
            private string Batchno;
            //public string ProjRefNo { get; set;
            [XmlElement(IsNullable = true)]
            public string ProjRefNo
            {
                get { return string.IsNullOrEmpty(this.ProjectRefno) ? string.Empty : this.ProjectRefno; }
                set
                {
                    if (this.ProjectRefno != value)
                    {
                        this.ProjectRefno = value;
                    }
                }
            }
            private string ProjectRefno;

            // ADDED BY LALITHA ON 14/02/2019  
            [XmlElement(IsNullable = true)]
            public string SerialNo
            {
                get { return string.IsNullOrEmpty(this.Serialno) ? string.Empty : this.Serialno; }
                set
                {
                    if (this.Serialno != value)
                    {
                        this.Serialno = value;
                    }
                }
            }
            private string Serialno;

            [XmlElement(IsNullable = true)]
            public string MRP
            {
                get { return string.IsNullOrEmpty(this.mrp) ? string.Empty : this.mrp; }
                set
                {
                    if (this.mrp != value)
                    {
                        this.mrp = value;
                    }
                }
            }
            private string mrp;

            [XmlElement(IsNullable = true)]
            public string AWBNo
            {
                get { return string.IsNullOrEmpty(this.awbno) ? string.Empty : this.awbno; }
                set
                {
                    if (this.awbno != value)
                    {
                        this.awbno = value;
                    }
                }
            }
            private string awbno;

            [XmlElement(IsNullable = true)]
            public string Courier
            {
                get { return string.IsNullOrEmpty(this.courier) ? string.Empty : this.courier; }
                set
                {
                    if (this.courier != value)
                    {
                        this.courier = value;
                    }
                }
            }
            private string courier;

            [XmlElement(IsNullable = true)]
            public string Priority
            {
                get { return string.IsNullOrEmpty(this.priority) ? string.Empty : this.priority; }
                set
                {
                    if (this.priority != value)
                    {
                        this.priority = value;
                    }
                }
            }
            private string priority;

            [XmlElement(IsNullable = true)]
            public string DueDate
            {
                get { return string.IsNullOrEmpty(this.dueDate) ? string.Empty : this.dueDate; }
                set
                {
                    if (this.dueDate != value)
                    {
                        this.dueDate = value;
                    }
                }
            }
            private string dueDate;

            [XmlElement(IsNullable = true)]
            public string Notes
            {
                get { return string.IsNullOrEmpty(this.notes) ? string.Empty : this.notes; }
                set
                {
                    if (this.notes != value)
                    {
                        this.notes = value;
                    }
                }
            }
            private string notes;

        }
        [WebMethod]
        public static string GetOutwardImport(List<OutboundImport> obdlst)
        {
            CustomPrincipal customprinciple = HttpContext.Current.User as CustomPrincipal;
            List<OutboundImport> lst = new List<OutboundImport>();

            List<OutboundImport> data = null;
            List<OutboundImport> Checkqty = null;
            List<OutboundImport> CheckSodate = null;
            List<OutboundImport> CheckMFGdate = null;
            List<OutboundImport> CheckExpdate = null;
            //data = obdlst.FindAll(po => (po.CustomerCode == "" || po.InvoiceNo == "" || po.PartNo == "" || po.LineNumber == "" || po.SODate == "" || po.SONumber == "" || po.SOQuantity == "" || po.TenantCode == "" || po.UoM == "" || po.UoMQuantity == "" || po.WHcode == "")).ToList();
            //if(data.Count>0)
            //{
            //    return "Error : Please Enter Valid Data in Excel";
            //}
            try
            {
                Checkqty = obdlst.FindAll(item => (Convert.ToInt32(item.SOQuantity) <= 0)).ToList();
            }
            catch (Exception e)
            {

                return "Error : Enter valid Quantity in Excel ";

            }
            if (Checkqty.Count > 0)
            {

                return "Error :  Quantity should not contain 0’s and Negatives";

            }

            ListtoDataTableConverter converter = new ListtoDataTableConverter();
            int rownumber = 0;
            DataTable dt = converter.ToDataTable(obdlst);
            string Sodate = "";
            string MRP = "";
            //string MfgDate = "";
            //string ExpDate = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Sodate = dt.Rows[i]["SODate"].ToString().Trim() != "" ? dt.Rows[i]["SODate"].ToString().Trim() : null;
                //MfgDate = dt.Rows[i]["MfgDate"].ToString().Trim() != "" ? dt.Rows[i]["MfgDate"].ToString().Trim() : null;
                // ExpDate = dt.Rows[i]["ExpDate"].ToString().Trim() != "" ? dt.Rows[i]["ExpDate"].ToString().Trim() : null;                              
                rownumber = i + 1;
                //if (Sodate.Contains("/") == true)
                //{
                //    if (Sodate.Split('/')[2].Length == 2)
                //    {
                //        string date = Convert.ToDateTime(Sodate).ToString("dd-MM-yyyy");
                //        Sodate = date.Replace("-", "/");
                //    }
                //}
                try
                {
                    // invoicedate = DateTime.ParseExact(invoicedate.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                    //DateTime date;
                    //bool success = DateTime.TryParse(Sodate, out date);
                    //Sodate = date.ToString("MM/dd/yyyy");
                    Sodate = CommonLogic.Formateddate(Sodate);
                }
                catch (Exception ex)
                {
                    return "provide Sodate Date format as dd/MM/yyyy";
                }
                try
                {
                    MRP = dt.Rows[i]["MRP"].ToString().Trim() != "" ? dt.Rows[i]["MRP"].ToString().Trim() : null;
                    if (MRP != "")
                    {
                        int MRPvalue = Convert.ToInt32(MRP);
                    }
                }
                catch (Exception ex)
                {
                    return "provide Sodate Date format as dd/MM/yyyy";
                }

                obdlst[i].SODate = Sodate;

                string MfgDate = "";
                if (dt.Rows[i]["MfgDate"].ToString() != "")
                {
                    try
                    {
                        // mfgdate = DateTime.ParseExact((dt.Rows[i]["MfgDate"]).ToString().Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                        MfgDate = dt.Rows[i]["MfgDate"].ToString().Trim() != "" ? dt.Rows[i]["MfgDate"].ToString().Trim() : null;
                        //DateTime date;
                        //bool success = DateTime.TryParse(MfgDate, out date);
                        //MfgDate = date.ToString("MM/dd/yyyy");
                        MfgDate = CommonLogic.Formateddate(MfgDate);
                    }
                    catch (Exception ex)
                    {
                        return "provide Invoice Mfg Date format as dd-MM-yyyy" + rownumber;
                    }
                    obdlst[i].MfgDate = MfgDate;
                }
                string Expdate = null;
                if (dt.Rows[i]["ExpDate"].ToString() != "")
                {
                    try
                    {
                        //  Expdate = DateTime.ParseExact((dt.Rows[i]["ExpDate"]).ToString().Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                        Expdate = dt.Rows[i]["ExpDate"].ToString().Trim() != "" ? dt.Rows[i]["ExpDate"].ToString().Trim() : null;
                        //DateTime date;
                        //bool success = DateTime.TryParse(Expdate, out date);
                        //Expdate = date.ToString("MM/dd/yyyy");
                        Expdate = CommonLogic.Formateddate(Expdate);
                    }
                    catch (Exception ex)
                    {
                        return "provide Invoice Exp Date format as dd-MM-yyyy" + rownumber;
                    }

                    obdlst[i].ExpDate = Expdate;
                }

                string DueDate = null;
                if (dt.Rows[i]["DueDate"].ToString() != "")
                {
                    try
                    {
                        //  Expdate = DateTime.ParseExact((dt.Rows[i]["ExpDate"]).ToString().Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                        DueDate = dt.Rows[i]["DueDate"].ToString().Trim() != "" ? dt.Rows[i]["DueDate"].ToString().Trim() : null;
                        //DateTime date;
                        //bool success = DateTime.TryParse(Expdate, out date);
                        //Expdate = date.ToString("MM/dd/yyyy");
                        DueDate = CommonLogic.Formateddate(DueDate);
                    }
                    catch (Exception ex)
                    {
                        return "provide Due Date format as dd/MM/yyyy" + rownumber;
                    }

                    obdlst[i].DueDate = DueDate;
                }

            }
            
            lst = obdlst;

            string batchno = "";
            string mfgdate = "";
            string expdate = "";
            string itemcode = "";
            string mmid = "";

            batchno = lst[0].BatchNo;
            mfgdate = lst[0].MfgDate;
            expdate = lst[0].ExpDate;
            itemcode = lst[0].PartNo;

            string xml = null;
            string s = string.Join("", obdlst);
            var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            emptyNamepsaces.Add("", "");
            var settings = new XmlWriterSettings();
            string error = "";
            string isError;
            using (StringWriter sw = new StringWriter())
            {
                XmlSerializer serialiser = new XmlSerializer(typeof(List<OutboundImport>));
                serialiser.Serialize(sw, obdlst, emptyNamepsaces);

                string Isrequired = "0";
                string Parametername = "";
                
                try
                {
                    xml = sw.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", " ");
                    return DB.GetSqlS("EXEC [dbo].[sp_OBD_Excel_BulkInsert_Outward] " + DB.SQuote(xml) + ",@UpdatedBy=" + customprinciple.UserID.ToString() + ",@AccountId=" + customprinciple.AccountID);
                }
                catch (Exception e)
                {
                    error = e.Message;
                    //throw e;

                }
            }



            return error;
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
    }
}