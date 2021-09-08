using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using MRLWMSC21Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
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

namespace MRLWMSC21.mInventory
{
    public partial class StockImport : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        protected void PagePre_Init(object sender, EventArgs e)
        {
            Page.Theme = "Inventory";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Stock Import");
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
                    ColumnData = ColumnData + "," + item.ToString();
                }
            }
            // _sb.Append("</tr></table>");
            // ViewState["ColumnData"] = ColumnData.Remove(0,1);
            //DownloadTemplate(_sb.ToString());
            DownloadTemplate(ColumnData.ToString().Substring(1));

        }
        private void DownloadTemplate(string _sColumns)
        {
            string FileName = "Template_StockTake";
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
                wb.Worksheets.Add(dt, "Sheet1");

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
            string _sRow = cp.UserID.ToString();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                _sRow = _sRow + ",'" + dt.Rows[index][i].ToString() + "'";
            }
            return _sRow + ", 1";
        }

        private void DeleteInsertedRecords()
        {
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


      
        public class StockImport1
        {
            public string TenantCode { get; set; }

            public string MaterialCode { get; set; }
            public string UoM { get; set; }
            public string Quantity { get; set; }
            public string MfgDate { get; set; }
            public string ExpDate { get; set; }
            public string BatchNo { get; set; }
            public string Bin { get; set; }


        }
    
        [WebMethod]
        public static string GetStockImport(List<StockImport1> Stklst)
        {
            CustomPrincipal customprinciple = HttpContext.Current.User as CustomPrincipal;
            List<StockImport1> lst = new List<StockImport1>();
            lst = Stklst;

            string error = "";
            string BatchNo = "";
            string MfgDate = "";
            string ExpDate = "";
            string itemcode = "";
            string mmid = "";
            string tenantcode = "";

            string ItemCode = "";
            string UoM = "";
            string Qty = "";

            string TenantCode = "";

            string Tid = "";
            string TenId = "";

            if (lst.Count == 0)
            {
                error = "-8";
                return error;
            }
            //DataTable ds=
            //for(int i=0;i<lst.Count)

            tenantcode = lst[0].TenantCode;
            ItemCode = lst[0].MaterialCode;
            UoM = lst[0].UoM;
            Qty = lst[0].Quantity;




            if (tenantcode == "" || tenantcode == null)
            {
                error = "TCode";
                return error;
            }

            if (ItemCode == "" || itemcode == null)
            {
                error = "MCode";
                return error;
            }
            if (UoM == "" || UoM == null)
            {
                error = "uom";
                return error;
            }
            if (Qty == "" || Qty == null)
            {
                error = "qty";
                return error;
            }

            ListtoDataTableConverter converter = new ListtoDataTableConverter();
            int rownumber = 0;
            DataTable dt = converter.ToDataTable(lst);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                BatchNo = dt.Rows[i]["BatchNo"].ToString().Trim() != "" ? dt.Rows[i]["BatchNo"].ToString() : null;
                //SerialNo = dt.Rows[i]["SerialNo"].ToString().Trim() != "" ? dt.Rows[i]["SerialNo"].ToString() : null;
                MfgDate = dt.Rows[i]["MfgDate"].ToString().Trim() != "" ? dt.Rows[i]["MfgDate"].ToString() : null;
                ExpDate = dt.Rows[i]["ExpDate"].ToString().Trim() != "" ? dt.Rows[i]["ExpDate"].ToString() : null;
                itemcode = dt.Rows[i]["MaterialCode"].ToString();
                TenantCode = dt.Rows[i]["TenantCode"].ToString();
                rownumber = i + 1;
            }

            //string query = "SELECT * FROM MMT_MaterialMaster WHERE MCode in(" + DB.SQuote(itemcode) + ")";
            //DataSet DS = DB.GetDS(query, false);
            //if (DS.Tables[0].Rows.Count != 0)
            //{
            //    foreach (DataRow row in DS.Tables[0].Rows)
            //    {
            //        mmid = row["MaterialMasterID"].ToString();
            //    }
            //}

            int accid = 0;
            string xml = null;
            string s = string.Join("", Stklst);
            var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            emptyNamepsaces.Add("", "");
            var settings = new XmlWriterSettings();

            string isError;
            using (StringWriter sw = new StringWriter())
            {
                XmlSerializer serialiser = new XmlSerializer(typeof(List<StockImport1>));
                serialiser.Serialize(sw, Stklst, emptyNamepsaces);

                string Isrequired = "0";
                string Parametername = "";
                //if (mmid == "")
                //{
                //    error = "-7";
                //    return error;
                //}
                //string Query = "select mm.MaterialMaster_MaterialStorageParameterID,mm.MaterialMasterID,mm.IsRequired,mm.MaterialStorageParameterID,msp.ParameterName from MMT_MaterialMaster_MSP mm join MMT_MSP msp on mm.MaterialStorageParameterID = msp.MaterialStorageParameterID where mm.MaterialMasterID in (" + mmid + ")";
                //DataSet ds = DB.GetDS(Query, false);
                //if (ds.Tables[0].Rows.Count != 0)
                //{
                //    foreach (DataRow row in ds.Tables[0].Rows)
                //    {
                //        Isrequired = row["IsRequired"].ToString();
                //        Parametername = row["ParameterName"].ToString();

                //        if (ds.Tables != null)
                //        {
                //            if (Parametername == "MfgDate" && Isrequired == "True")
                //            {
                //                if (MfgDate == "" || MfgDate == null)
                //                {
                //                    //  resetError("Please Enter Mandatory Mfg. Date", true);

                //                    //error = "Please Enter Mandatory Mfg. Date";
                //                    error = "-1," + rownumber;
                //                    isError = "true";

                //                    return error;
                //                }

                //            }
                //            else if (Parametername == "ExpDate" && Isrequired == "True")
                //            {
                //                if (ExpDate == "" || ExpDate == null)
                //                {
                //                    //resetError("Please Enter Mandatory Exp. Date", true);

                //                    // error = "Please Enter Mandatory Exp. Date";
                //                    error = "-2," + rownumber;
                //                    isError = "true";
                //                    return error;
                //                }

                //            }



                //            else if (Parametername == "BatchNo" && Isrequired == "True")
                //            {
                //                if (BatchNo == "" || BatchNo == null)
                //                {
                //                    //resetError("Please Enter Mandatory Batch No", true);
                //                    //error = "Please Enter Mandatory Batch No";
                //                    error = "-4," + rownumber;
                //                    isError = "true";


                //                    return error;
                //                }

                //            }


                //        }
                //    }
                //}

                try
                {
                    xml = sw.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", " ");
                    //int res = DB.GetSqlN("EXEC  [dbo].[sp_INB_Excel_BulkInsert_Inward] "+DB.SQuote(xml));



                    //string _query = "select * from TPL_Tenant where TenantCode in (" + DB.SQuote(TenantCode) + ")";
                    //DataSet _ds = DB.GetDS(_query, false);
                    //if (_ds.Tables[0].Rows.Count != 0)
                    //{
                    //    foreach (DataRow row in _ds.Tables[0].Rows)
                    //    {
                    //        Tid = row["TenantID"].ToString();
                    //    }
                    //}


                    //string _Query = "select * from MMT_Supplier where SupplierCode in (" + DB.SQuote(SupplierCOde) + ")";
                    //DataSet _Ds = DB.GetDS(_Query, false);
                    //if (_Ds.Tables[0].Rows.Count != 0)
                    //{
                    //    foreach (DataRow row in _Ds.Tables[0].Rows)
                    //    {
                    //        TenId = row["TenantID"].ToString();
                    //    }
                    //}


                    //if (Tid == TenId)
                    //{



                    int res = DB.GetSqlN("EXEC  [dbo].[SP_INV_Stock_BulkUpsert] " + DB.SQuote(xml) + ",@AccountID=" + customprinciple.AccountID.ToString());
                    if (res == 1)
                    {
                        DataSet ds1 = DB.GetDS("Exec sp_INV_Stock_BulkInsert", false);
                        return ds1.Tables[0].Rows[0]["Column1"].ToString();
                        //return false;
                    }


                    //if (res == 1)
                    //{

                    //    //error = "Excel uploaded successfully.";
                    //    error = "1";
                    //    isError = "false";

                    //}
                    //else
                    //{
                    //    //error = "ERROR WHILE UPLOADING ";
                    //    error = "0";
                    //    isError = "true";
                    //}
                    //DisplayError(error, isError);
                    //}
                    //else
                    //{
                    //    error = "InvalidTenantCode";
                    //}
                }
                catch (Exception e)
                {
                    error = "-6";

                    // throw (new Exception("0"));
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

        protected void resetError(string error, bool isError)
        {


            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);


        }
    }
}