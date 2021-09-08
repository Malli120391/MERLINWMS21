using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21Common;
using Newtonsoft.Json;
using System.Web.Script.Services;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Reflection;
using ClosedXML.Excel;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Threading;
using System.Globalization;

namespace MRLWMSC21.mMaterialManagement
{
    public partial class CustomerList : System.Web.UI.Page
    {
        public static CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        //======================= Added By M.D.Prasad For View Only Condition ======================//
        public static CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
        public static String[] UserRole;
        public static String UserRoledat = "";
        public static int UserTypeID;
        private object ddlLanguage;

        //======================= Added By M.D.Prasad For View Only Condition ======================//
        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Orders";//_sit";
            cp1 = HttpContext.Current.User as CustomPrincipal;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Customer");
                //======================= Added By M.D.Prasad For View Only Condition ======================//
                UserRole = cp1.Roles;
                UserRoledat = "";
                for (int i = 0; i < UserRole.Length; i++)
                {
                    UserRoledat = UserRoledat + UserRole[i].ToString() + ",";
                }
                UserTypeID = cp1.UserTypeID;
                //======================= Added By M.D.Prasad For View Only Condition ======================//
                //            if (Session["SelectedLang"] != null)

                //            {

                //                Langchange.SelectedValue = Session["SelectedLang"].ToString();

                //                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture
                //(Langchange.SelectedValue);

                //                Thread.CurrentThread.CurrentUICulture = new CultureInfo
                //(Langchange.SelectedValue);

                //            }

                //            else

                //            {

                //                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture
                //(Langchange.SelectedValue);

                //                Thread.CurrentThread.CurrentUICulture = new CultureInfo
                //(Langchange.SelectedValue);

                //            }

            }

        }




        [WebMethod]
        public static string GetCustomerList(string Search , int TenantID)
        {
            string Json = "";
            string selectMMList;
            try
            {

                CustomPrincipal customp = HttpContext.Current.User as CustomPrincipal;
                selectMMList = "Exec USP_GEN_GetCustomerInfo ";
                //selectMMList = selectMMList + "@AccountID_New = " + customp.AccountID.ToString() + ",@UserTypeID_New = " + customp.UserTypeID.ToString() + ",@TenantID_New = " + customp.TenantID.ToString() + ",@UserID_New=" + customp.UserID.ToString() + ",@SearchText=" + DB.SQuote(Search);
                selectMMList = selectMMList + "@AccountID_New = " + customp.AccountID.ToString() + ",@UserTypeID_New = " + customp.UserTypeID.ToString() + ",@TenantID_New = " +TenantID + ",@UserID_New=" + customp.UserID.ToString() + ",@SearchText=" + DB.SQuote(Search);

                DataSet ds = DB.GetDS(selectMMList, false);
                Json = JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                Json = "Failed";
            }
            return Json;
        }

        [WebMethod]
        public static string DeleteCustomer(string StrId)
        {

            CustomPrincipal customprinciple = HttpContext.Current.User as CustomPrincipal;

            string Status;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" Exec [dbo].[GEN_DELETE_Customer] ");
                sb.Append("@PK=" + StrId + ",");
                sb.Append("@LoggedInUserID=" + cp.UserID);


                DB.ExecuteSQL(sb.ToString());
                Status = "success";
            }
            catch (Exception ex)
            {
                Status = "Failed";
            }

            return Status;
        }

        public class CustomerImport
        {
            [XmlElement(IsNullable = true)]
            public string TenantCode
            {
                get { return string.IsNullOrEmpty(this.tenantCode) ? string.Empty : this.tenantCode; }
                set
                {
                    if (this.tenantCode != value)
                    {
                        this.tenantCode = value;
                    }
                }
            }
            private string tenantCode;

            [XmlElement(IsNullable = true)]
            public string CustomerName
            {
                get { return string.IsNullOrEmpty(this.customerName) ? string.Empty : this.customerName; }
                set
                {
                    if (this.customerName != value)
                    {
                        this.customerName = value;
                    }
                }
            }
            private string customerName;

            [XmlElement(IsNullable = true)]
            public string CustomerCode
            {
                get { return string.IsNullOrEmpty(this.customerCode) ? string.Empty : this.customerCode; }
                set
                {
                    if (this.customerCode != value)
                    {
                        this.customerCode = value;
                    }
                }
            }
            private string customerCode;

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
            public string Mobile
            {
                get { return string.IsNullOrEmpty(this.mobile) ? string.Empty : this.mobile; }
                set
                {
                    if (this.mobile != value)
                    {
                        this.mobile = value;
                    }
                }
            }
            private string mobile;

            [XmlElement(IsNullable = true)]
            public string AccountCode
            {
                get { return string.IsNullOrEmpty(this.accountCode) ? string.Empty : this.accountCode; }
                set
                {
                    if (this.accountCode != value)
                    {
                        this.accountCode = value;
                    }
                }
            }
            private string accountCode;
        }

        //
        [WebMethod]
        public static string ImportExcelCustomerData(List<CustomerImport> inblst)
        {
            CustomPrincipal customprinciple = HttpContext.Current.User as CustomPrincipal;
            List<CustomerImport> lst = new List<CustomerImport>();
            List<CustomerImport> chklst = new List<CustomerImport>();
            if (inblst.Count == 0)
            {
                return "-8";
                
            }
            chklst = inblst.FindAll(item => (item.AccountCode.Trim() == "" || item.TenantCode.Trim() == "" || item.CustomerName.Trim() == "" || item.CustomerCode.Trim() == ""));
            if (chklst.Count > 0)
            {
                return "-8";
               
            }
            lst = inblst;
            ListtoDataTableConverter converter = new ListtoDataTableConverter();
            int rownumber = 0;
            DataTable dt = converter.ToDataTable(lst);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                inblst[i].CustomerName  = dt.Rows[i]["CustomerName"].ToString().Trim() != "" ? dt.Rows[i]["CustomerName"].ToString() : null;
                //SerialNo = dt.Rows[i]["SerialNo"].ToString().Trim() != "" ? dt.Rows[i]["SerialNo"].ToString() : null;
                inblst[i].CustomerCode  = dt.Rows[i]["CustomerCode"].ToString().Trim() != "" ? dt.Rows[i]["CustomerCode"].ToString() : null;
                inblst[i].TenantCode  = dt.Rows[i]["TenantCode"].ToString().Trim() != "" ? dt.Rows[i]["TenantCode"].ToString() : null;
                inblst[i].Email  = dt.Rows[i]["Email"].ToString().Trim() != "" ? dt.Rows[i]["Email"].ToString() : null;
                inblst[i].Mobile  = dt.Rows[i]["Mobile"].ToString().Trim() != "" ? dt.Rows[i]["Mobile"].ToString() : null;
                inblst[i].AccountCode  = dt.Rows[i]["AccountCode"].ToString().Trim() != "" ? dt.Rows[i]["AccountCode"].ToString() : null;
                rownumber = i + 1;
            }
            string xml = null;
            string s = string.Join("", inblst);
            var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            emptyNamepsaces.Add("", "");
            var settings = new XmlWriterSettings();
            CustomerList cs = new CustomerList();
            string FileName = "FailedItems";
            using (StringWriter sw = new StringWriter())
            {
                XmlSerializer serialiser = new XmlSerializer(typeof(List<CustomerImport>));
                serialiser.Serialize(sw, inblst, emptyNamepsaces);
                try
                {
                    xml = sw.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", " ");
                    //int res = DB.GetSqlN("EXEC  [dbo].[sp_INB_Excel_BulkInsert_Inward] "+DB.SQuote(xml));
                    DataSet ds = DB.GetDS("EXEC  [dbo].[Usp_Excel_BulkInsert_CustomerImport] @XMLDATA=" + DB.SQuote(xml) + ",@UpdatedBy=" + customprinciple.UserID.ToString(), false);
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        return "1";
                        //successfully Imported
                    }

                    else if (ds.Tables[0].Rows.Count == inblst.Count)
                    {
                        //DataSet p_dsSrc, string fileName, List<int> notReqiredCoulumnIndex
                        // all records include errors
                        ExportDataSetToExcel(ds, FileName, new List<int>());
                        return "-1";
                    }
                    else
                    {

                        ExportDataSetToExcel(ds, FileName, new List<int>());
                        return "-2";
                       
                    }

                }
                catch (Exception e)
                {
                   return "-6";

                   
                }


            }
        }




        //download method
        public static void ExportDataSetToExcel(DataSet p_dsSrc, string fileName, List<int> notReqiredCoulumnIndex)
        {
            string strOperationNumber = string.Empty;
            ExcelPackage objExcelPackage = new ExcelPackage();
            for (int tableindex = 0; tableindex < p_dsSrc.Tables.Count; tableindex++)
            {

                DataTable dtSrc = p_dsSrc.Tables[tableindex];
                string sheetName = fileName;
               
                //Create the worksheet    
                ExcelWorksheet objWorksheet = objExcelPackage.Workbook.Worksheets.Add(sheetName);
                //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1    
                //Rows
                //Adding Headers

                for (int index = 0; index < dtSrc.Rows.Count; index++)
                {
                    //Columns
                    for (int colindex = 0; colindex < dtSrc.Columns.Count; colindex++)
                    {
                        // Checking for row index and wring header data
                        if (index == 0)
                        {

                            if (notReqiredCoulumnIndex.IndexOf(colindex) == -1)
                            {

                                objWorksheet.Cells[index + 1, colindex + 1].RichText.Text = dtSrc.Columns[colindex].Caption;
                                objWorksheet.Cells[index + 2, colindex + 1].RichText.Text = dtSrc.Rows[index][colindex].ToString();

                            }

                        }
                        else
                        {

                            if (notReqiredCoulumnIndex.IndexOf(colindex) == -1)
                            {

                                objWorksheet.Cells[index + 2, colindex + 1].RichText.Text = dtSrc.Rows[index][colindex].ToString();

                            }
                        }

                    }
                }
                objWorksheet.Cells.Style.Font.SetFromFont(new System.Drawing.Font("Calibri", 12));
                objWorksheet.Cells.AutoFitColumns();
                

                //Format the header    
                using (ExcelRange objRange = objWorksheet.Cells["A1:XFD1"])
                {
                    objRange.Style.Font.Bold = true;
                    objRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    objRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    


                }
            }


            try
            {
                string strUploadPath = System.Web.HttpContext.Current.Server.MapPath("..\\ExcelData");
                objExcelPackage.SaveAs(new System.IO.FileInfo(strUploadPath + "\\" + fileName + ".xlsx"));
                // System.Diagnostics.Process.Start("Data.xlsx");

            }
            catch (Exception ex)
            {


            }
        }
        private static void downLoadExcel(DataSet ds)
        {
            try
            {
                string FileName = "FailedItems";
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(ds.Tables[0]);

                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".xlsx");
                    //Response.Redirect("MaterialMasterList.aspx", false);
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
                        HttpContext.Current.Response.Flush();
                        // Response.WriteFile(Response.OutputStream);

                        //FUCUSImportExcel.Attributes.Clear();
                        HttpContext.Current.Response.End();
                    }

                }
            }
            catch (Exception ex)
            {
                //FUSupImportExcel.Attributes.Clear();
                // GetCustomerList();
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

        //protected void Langchange_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (Langchange.SelectedValue == "en-Us")

        //    {

        //        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-Us");

        //        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-Us");

        //        Session["SelectedLang"] = Langchange.SelectedValue;
        //        string s = Session["SelectedLang"].ToString();
        //        // Server.Transfer(Request.Path);
        //        Server.Transfer(Request.Path);


        //    }

        //    else if (Langchange.SelectedValue == "hi-IN")

        //    {

        //        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("hi-IN");

        //        Thread.CurrentThread.CurrentUICulture = new CultureInfo("hi-IN");

        //        Session["SelectedLang"] = Langchange.SelectedValue;
        //        string s = Session["SelectedLang"].ToString();
        //        // Server.Transfer("~/Test.aspx");
        //        // Response.Redirect("Test.aspx");
        //        Server.Transfer(Request.Path);

        //    }

        //    else

        //    {

        //        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX");

        //        Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-MX");

        //        Session["SelectedLang"] = Langchange.SelectedValue;
        //        string s = Session["SelectedLang"].ToString();
        //        //Response.Redirect("Test.aspx");
        //        Server.Transfer(Request.Path);



        //}



        public void resetError(string error, bool isError)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);

        }
    }
}
    
