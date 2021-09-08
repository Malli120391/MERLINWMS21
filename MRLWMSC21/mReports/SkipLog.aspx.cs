using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;
using ClosedXML.Excel;
using MRLWMSC21Common;


namespace MRLWMSC21.mReports
{
    public partial class OBDSkipLog : System.Web.UI.Page
    {
        public static CustomPrincipal cp = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            cp = (CustomPrincipal)HttpContext.Current.User;
        }

        [WebMethod]
        public static string getSkipData(string TenantID,string Type,string Material ,string Fromdate,string Todate)
        {
            string sql;
            if (Type == "obd")
            {
               sql= "EXEC OBD_RPT_SkipLog @TenantID="+ TenantID + ",@MaterialID="+ Material + ",@From='"+ Fromdate + "',@Todate='"+ Todate + "'";
            }
            else
            {
                sql= "EXEC INB_RPT_SkipLog @TenantID=" + TenantID + ",@MaterialID=" + Material + ",@From='" + Fromdate + "',@Todate='" + Todate + "'";
            }
             
            DataSet ds = DB.GetDS(sql, false);
            return Newtonsoft.Json.JsonConvert.SerializeObject(ds);
        }

        [WebMethod]
        public static string ClearLog(string Type, List<SkipClearData> SkipClear)
        {
            try
            {
               
                var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                emptyNamepsaces.Add("", "");
                var settings = new XmlWriterSettings();
                string xml = null;
                using (StringWriter sw = new StringWriter())
                {
                    XmlSerializer serialiser = new XmlSerializer(typeof(List<SkipClearData>));
                    serialiser.Serialize(sw, SkipClear, emptyNamepsaces);
                  
                        xml = sw.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", " ");
   
                }
                  return DB.GetSqlS("Exec [SP_UpdateSkipItemLog] @XML=" + DB.SQuote(xml)+ ",@Type="+ DB.SQuote(Type));
                
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

           
        }

        [WebMethod]
        public static string DownloadExcelForLog(string TenantID, string Type, string Material, string Fromdate, string Todate)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            string strUploadPath = System.Web.HttpContext.Current.Server.MapPath("..\\ExcelData");
            string fileName = "";
            try
            {
                string sql;
                // fileName = "IBSummaryReport_" + DateTime.Now.Day + "" + DateTime.Now.Month + "" + DateTime.Now.Year + "_" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second;
                fileName = "SkipLogReport";
                if (Type == "obd")
                {
                    sql = "EXEC OBD_RPT_SkipLog @TenantID=" + TenantID + ",@MaterialID=" + Material + ",@From='" + Fromdate + "',@Todate='" + Todate + "',@Isexcel=1";
                }
                else
                {
                    sql = "EXEC INB_RPT_SkipLog @TenantID=" + TenantID + ",@MaterialID=" + Material + ",@From='" + Fromdate + "',@Todate='" + Todate + "',@Isexcel=1";
                }

                DataSet ds = DB.GetDS(sql, false);
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(ds);
                    wb.SaveAs(strUploadPath + "\\" + fileName + ".xlsx");
                }

            }
            catch (Exception ex)
            {
                return "0";
            }
            return fileName;

        }

        public class SkipClearData
        {
            public string MaterialID { get; set; }
            public string LocationId { get; set; }
            public string SkipQuantity { get; set; }
        }
    }
}