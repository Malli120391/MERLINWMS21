using MRLWMSC21Common;
using MRLWMSC21.mOutbound.BL;
using MRLWMSC21.mReports;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRLWMSC21.mInbound
{
    public partial class RTReport : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        protected String POQuantityListSQL = "";
        InboundTrack IBTrack;
        int prmInboundTrackingID = 0;
        private GridViewHelper helper;


        protected void PagePre_Init(object sender, EventArgs e)
        {

            Page.Theme = "Inbound";

        }

        protected void Page_Load(object sender, EventArgs e)
        {

            string data = HttpContext.Current.Request.Url.AbsolutePath;
            prmInboundTrackingID = DB.GetSqlN("Select top 1 InboundID AS N from INB_Inbound where IsActive=1 and IsDeleted=0 AND InboundID=" + CommonLogic.QueryStringUSInt("ibdid"));

            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Receiving Tally Sheet");



                if (prmInboundTrackingID != 0)
                {
                    ViewState["InboundID"] = prmInboundTrackingID;
                    IBTrack = new InboundTrack(prmInboundTrackingID);

                    if (IBTrack.ShipmentTypeID == 4)
                        lblHeader.Text = "Returns Receiving Tally Report";

                    ltbarStoreRefNo.Text = IBTrack.StoreRefNumber + "<br/><br/><img border=0 src=\"Code39Handler.ashx?code=" + IBTrack.StoreRefNumber + "\" />";
                    ltbarStoreRefNo1.Text = IBTrack.StoreRefNumber;

                   // ltrGRNNo.Text = IBTrack.Account;


                    if (IBTrack.ReferedStoreIDs != "" && IBTrack.ReferedStoreIDs != null)
                    {
                        ltbarWarehouse.Text = CommonLogic.GetStoreNames(IBTrack.ReferedStoreIDs, ",");
                    }
                    else
                    {
                        ltbarWarehouse.Text = "";
                    }
                    ltSupplier.Text = IBTrack.SupplierName;
                    //ltAWBBLNo.Text = IBTrack.ConsignmentNoteTypeValue;
                    ltDockLocation.Text = IBTrack.DockLocation;
                    ltDockBarcode.Text = "<img border=0 src=\"Code39Handler.ashx?code=" + IBTrack.DockLocation + "\" />"; //================= Added By M.D.Prasad For Generate Barcode Image for Dock Location On 31-Dec-2019 =================//
                    //ltNoofPackages.Text = IBTrack.NoofPackagesInDocument.ToString();
                    //  ltWeight.Text = IBTrack.GrossWeight;

                    ltDocDate.Text = IBTrack.DocReceivedDate;// DateTime.ParseExact(IBTrack.DocReceivedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
                    ltTenant.Text = CommonLogic.QueryString("TN");
                }
            }

            if (!IsPostBack)
            {
                String IconPath = "";
                if (cp != null)
                {
                    //cp = HttpContext.Current.User as CustomPrincipal;
                    //cp = Session["UserProfile"] as CustomPrincipal;
                    string fileName = DB.GetSqlS("SELECT DISTINCT LogoPath AS S FROM GEN_User USR JOIN GEN_Account ACC ON ACC.AccountID = USR.AccountID AND ACC.IsActive = 1 AND ACC.IsDeleted = 0 WHERE USR.IsActive = 1 AND USR.IsDeleted = 0 AND USR.UserID = " + cp.UserID);
                    if (fileName == "" || fileName == null || cp.AccountID == 0)
                    {
                        fileName = "inventrax.png";
                        IconPath = "Images/";
                    }
                    else
                    {
                        fileName = fileName;
                        IconPath = "TPL/AccountLogos/";
                    }

                    if (cp.UserID == 1)
                    {
                        fileName = "inventrax.png";
                        IconPath = "Images/";
                    }

                    imgLogo.ImageUrl = Page.ResolveUrl("~") + IconPath + fileName;
                    //imgLogo.Attributes.Add("width", "166");
                    //imgLogo.Attributes.Add("height", "50");
                }
            }
        }

        [WebMethod]
        public static List<MRLWMSC21.mReports.GetDataListModel.LabelSize> GetLabels()
        {
            try
            {
                List<MRLWMSC21.mReports.GetDataListModel.LabelSize> lstlabel = new List<GetDataListModel.LabelSize>();
                GetDataListBL data = new GetDataListBL();
                lstlabel = data.Getlabel();
                return lstlabel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [WebMethod]
        public static string getReceivingDetails(string InbID, string MID)
        {
            DataSet ds = DB.GetDS("EXEC[sp_INB_ReceivingTallyReport] @InboundID = " + InbID + ", @MCode='" + MID + "'", false);
            return JsonConvert.SerializeObject(ds);
        }

        public class RTR_Report
        {
            public string MfgDate { get; set; }
            public string ExpDate { get; set; }
            public string SerialNo { get; set; }
            public string BatchNo { get; set; }
            public string ProjectRefNo { get; set; }
            public string MCode { get; set; }
            public string MDescription { get; set; }
            public string MDescriptionLong { get; set; }

            public string LineNo { get; set; }
            public string MRP { get; set; }
            public string KitCode { get; set; }
            public string PrintQty { get; set; }
            public string LabelType { get; set; }
            public string HUNo { get; set; }
            public string HUSize { get; set; }
            public string GRNNumber { get; set; }
        }

        [WebMethod]
        public static string GetPrint(List<RTR_Report> printobj, string Printerid, string LabelID)
        {
            OutBoundBL objData = new OutBoundBL();
            string path = "";
            if (Printerid == "2")
            {
                //string labelType = DB.GetSqlS("SELECT LabelType S FROM TPL_Tenant_BarcodeType WHERE IsActive=1 and IsDeleted=0 and TenantBarcodeTypeID=" + LabelID);
                //path = objData.generatePrintLabeltoPDF(printobj, labelType);
                return "200";
            }
            else
            {
                string ZPL = "";
                List<RTR_Report> lst = new List<RTR_Report>();
                lst = printobj;
                TracklineMLabel Mlabel = new TracklineMLabel();
                for (var i = 0; i < printobj.Count; i++)
                {

                    int HUSize = Convert.ToInt32(printobj[i].HUSize == "" ? "0" : printobj[i].HUSize);

                    if (HUSize == 0)
                    {
                        Mlabel.MCode = printobj[i].MCode;
                        Mlabel.MDescription = printobj[i].MDescription;
                        Mlabel.MDescriptionLong = printobj[i].MDescriptionLong;
                        //string MDescription = DB.GetSqlS("SELECT MDescription AS S FROM MMT_MaterialMaster WHERE IsActive=1 AND IsDeleted=0 AND MCode='" + printobj[i].MCode + "'");
                       // Mlabel.Description = MDescription;
                        Mlabel.SerialNo = printobj[i].SerialNo;
                        Mlabel.BatchNo = printobj[i].BatchNo;
                        Mlabel.MfgDate = printobj[i].MfgDate;
                        Mlabel.ExpDate = printobj[i].ExpDate;
                        Mlabel.ProjectNo = printobj[i].ProjectRefNo;
                        Mlabel.Lineno = printobj[i].LineNo;
                        Mlabel.Mrp = printobj[i].MRP;
                        Mlabel.KitCode = printobj[i].KitCode;
                        //int HUNo = j + 1;
                        Mlabel.HUNo = "";
                        Mlabel.HUSize = "";

                        string length = "";
                        string width = "";
                        string LabelType = "";
                        string query = "select  top 1 * from TPL_Tenant_BarcodeType where IsActive=1 and IsDeleted=0 and TenantBarcodeTypeID=" + LabelID;
                        DataSet DS = DB.GetDS(query, false);
                        foreach (DataRow row in DS.Tables[0].Rows)
                        {
                            length = row["Length"].ToString();
                            width = row["Width"].ToString();
                            LabelType = row["LabelType"].ToString();
                        }
                        Mlabel.PrinterIP = Printerid;
                        Mlabel.IsBoxLabelReq = false;
                        Mlabel.Length = length;
                        Mlabel.Width = width;
                        int dpi = 0;
                        dpi = CommonLogic.GETDPI(Mlabel.PrinterIP);
                        Mlabel.Dpi = 203; //dpi;
                        Mlabel.PrintQty = printobj[i].PrintQty;
                        Mlabel.LabelType = LabelType;
                        MRLWMSC21Common.PrintCommon.CommonPrint print = new MRLWMSC21Common.PrintCommon.CommonPrint();
                        //Commented ON 01-JAN-2019 BY Prasad // print.PrintBarcodeLabel(Mlabel);
                        ZPL += print.PrintBarcodeLabel(Mlabel);
                    }
                    else
                    {
                        for (var j = 0; j < HUSize; j++)
                        {
                            Mlabel.MCode = printobj[i].MCode;
                            Mlabel.MDescription = printobj[i].MDescription;
                            Mlabel.MDescriptionLong = printobj[i].MDescriptionLong;

                            // string MDescription = DB.GetSqlS("SELECT MDescription AS S FROM MMT_MaterialMaster WHERE IsActive=1 AND IsDeleted=0 AND MCode='" + printobj[i].MCode + "'");
                            //Mlabel.Description = MDescription;
                            Mlabel.SerialNo = printobj[i].SerialNo;
                            Mlabel.BatchNo = printobj[i].BatchNo;
                            Mlabel.MfgDate = printobj[i].MfgDate;
                            Mlabel.ExpDate = printobj[i].ExpDate;
                            Mlabel.ProjectNo = printobj[i].ProjectRefNo;
                            Mlabel.Lineno = printobj[i].LineNo;
                            Mlabel.Mrp = printobj[i].MRP;
                            Mlabel.KitCode = printobj[i].KitCode;
                            int HUNo = j + 1;
                            Mlabel.HUNo = Convert.ToString(HUNo);
                            Mlabel.HUSize = printobj[i].HUSize;

                            string length = "";
                            string width = "";
                            string LabelType = "";
                            string query = "select top 1 * from TPL_Tenant_BarcodeType where IsActive=1 and IsDeleted=0 and TenantBarcodeTypeID=" + LabelID;
                            DataSet DS = DB.GetDS(query, false);
                            foreach (DataRow row in DS.Tables[0].Rows)
                            {
                                length = row["Length"].ToString();
                                width = row["Width"].ToString();
                                LabelType = row["LabelType"].ToString();
                            }
                            Mlabel.PrinterIP = Printerid;
                            Mlabel.IsBoxLabelReq = false;
                            Mlabel.Length = length;
                            Mlabel.Width = width;
                            int dpi = 0;
                            dpi = CommonLogic.GETDPI(Mlabel.PrinterIP);
                            Mlabel.Dpi = 203; //dpi;
                            Mlabel.PrintQty = printobj[i].PrintQty;
                            Mlabel.LabelType = LabelType;
                            MRLWMSC21Common.PrintCommon.CommonPrint print = new MRLWMSC21Common.PrintCommon.CommonPrint();
                            //Commented ON 01-JAN-2019 BY Prasad // print.PrintBarcodeLabel(Mlabel);
                            ZPL += print.PrintBarcodeLabel(Mlabel);
                        }
                    }
                }
                lst.Clear();
                return ZPL;// "Printed Successfully";  
            }
        }



        public static void printPDF(string zplString, int Length, int Width)
        {
            string dimensions = Width + "x" + Length;
            byte[] zpl = Encoding.UTF8.GetBytes(zplString);
            var request = (HttpWebRequest)WebRequest.Create("http://api.labelary.com/v1/printers/8dpmm/labels/" + dimensions + "/1/");
            request.Method = "POST";
            request.Accept = "application/pdf"; // omit this line to get PNG images back
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = zpl.Length;

            var requestStream = request.GetRequestStream();
            requestStream.Write(zpl, 0, zpl.Length);
            requestStream.Close();

            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                var responseStream = response.GetResponseStream();
                var fileStream = File.Create("label.pdf"); // change file name for PNG images
                responseStream.CopyTo(fileStream);
                responseStream.Close();
                fileStream.Close();
            }
            catch (WebException ex)
            {
                Console.WriteLine("Error: {0}", ex.Status);
            }
        }
    }

}