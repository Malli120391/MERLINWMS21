using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using MRLWMSC21Common;
using System.Data;
using System.Globalization;

namespace MRLWMSC21.mReports
{
    public partial class LostAndFoundReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Audit / Lost And Found Report");
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)] //, string StartDate, string EndDate, string TenantID, string SupplierID, string MaterialID
        public static List<LostAndFoundReports> GetLostFoundReportList(string WarehouseID, DateTime StartDate, DateTime EndDate, string TenantID, string SupplierID, string MaterialID)
        {
            List<LostAndFoundReports> GetLFReport = new List<LostAndFoundReports>();
            GetLFReport = new LostAndFoundReport().GetLFRPT(GetLFReport, WarehouseID, StartDate, EndDate, TenantID, SupplierID, MaterialID);
            return GetLFReport;
        }

        private List<LostAndFoundReports> GetLFRPT(List<LostAndFoundReports> sList,string WarehouseID, DateTime StartDate, DateTime EndDate, string TenantID, string SupplierID, string MaterialID)
        {
            LostAndFoundReports lst = new LostAndFoundReports();
            string formattedstartDate = StartDate.ToString("dd-MMM-yyyy");
            var format = "NULL";
            string formattedendDate = EndDate == DateTime.MinValue ? format : DB.SQuote(EndDate.ToString("dd-MMM-yyyy"));
            var mmid = MaterialID == "" ? "0" : MaterialID;
            var tenantid = TenantID == "" ? "0" : TenantID;
            var supid = SupplierID == "" ? "0" : SupplierID;
            string Query = " EXEC [dbo].[USP_GET_RPT_LostandFoundStorageLocationLF]  @StartDt =" + DB.SQuote(formattedstartDate) + ",  @EndDt = " + formattedendDate + ", @WarehouseID =" + WarehouseID + ", @MaterialMasterID =" + mmid + ", @TenantID =" + tenantid + ", @SupplierID =" + supid;
            DataSet DS = DB.GetDS(Query, false);
            

            if (DS != null && DS.Tables != null)
            {
                if (DS.Tables[0].Rows.Count != 0)
                {
                    List<OBTable> OBT = new List<OBTable>();                    
                    //{
                    for (int i = 0; i < DS.Tables[0].Rows.Count; i++)
                    {
                        OBTable IFR = new OBTable();
                        IFR.LostORFound = DS.Tables[0].Rows[i]["LostORFound"].ToString();
                        IFR.StartDt = DS.Tables[0].Rows[i]["StartDt"].ToString();
                        OBT.Add(IFR);// = IFR;
                    }
                    //IFR.LostORFound = row["LostORFound"].ToString();
                    
                    lst.objOBTable = OBT;          //}; 
                }

                if (DS.Tables[1].Rows.Count != 0)
                {
                    List<MainTable> OMT = new List<MainTable>();                   
                    //{
                    for (int i = 0; i < DS.Tables[1].Rows.Count; i++)
                    {
                        MainTable IFR1 = new MainTable();
                        IFR1.Date = DS.Tables[1].Rows[i]["Date"].ToString();
                        IFR1.UserName = DS.Tables[1].Rows[i]["UserName"].ToString();
                        IFR1.AccountName = DS.Tables[1].Rows[i]["AccountName"].ToString();
                        IFR1.Type = DS.Tables[1].Rows[i]["Type"].ToString();
                        IFR1.MCode = DS.Tables[1].Rows[i]["MCode"].ToString();
                        IFR1.Location = DS.Tables[1].Rows[i]["Location"].ToString();
                        IFR1.LOST = DS.Tables[1].Rows[i]["LOST"].ToString();
                        IFR1.FOUND = DS.Tables[1].Rows[i]["FOUND"].ToString();
                        IFR1.Total = DS.Tables[1].Rows[i]["Total"].ToString();
                        IFR1.NetPosition = DS.Tables[1].Rows[i]["NetPosition"].ToString();
                        OMT.Add(IFR1);
                    }
                    //};
                    //lst.objMainTable = IFR1;
                    
                    lst.objMainTable = OMT;
                }

                if (DS.Tables[2].Rows.Count != 0)
                {
                    List<CBTable> CBT = new List<CBTable>();                    
                    //{
                    for (int i = 0; i < DS.Tables[2].Rows.Count; i++)
                    {
                        CBTable IFR2 = new CBTable();
                        //IFR.LostORFound = row["LostORFound"].ToString();
                        IFR2.CB = DS.Tables[2].Rows[i]["CB"].ToString();
                        IFR2.CBEnd = DS.Tables[2].Rows[i]["CBEnd"].ToString();
                        CBT.Add(IFR2);
                    }
                    //};
                    //lst.objCBTable = IFR2;
                    
                    lst.objCBTable = CBT;
                }
                sList.Add(lst);
            }
            return sList;
        }



        public class LostAndFoundReports
        {
            public List<OBTable> objOBTable { get; set; }
            public List<MainTable> objMainTable { get; set; }
            public List<CBTable> objCBTable { get; set; }
        }
        public class MainTable
        {
            public string Date { get; set; }
            public string UserName { get; set; }
            public string AccountName { get; set; }
            public string Type { get; set; }
            public string MCode { get; set; }
            public string Location { get; set; }
            public string LOST { get; set; }
            public string FOUND { get; set; }
            public string Total { get; set; }
            public string NetPosition { get; set; }
        }
        public class OBTable
        {
            public string LostORFound { get; set; }
            public string StartDt { get; set; }
        }
        public class CBTable
        {
            public string CB { get; set; }
            public string CBEnd { get; set; }
        }
    }
}