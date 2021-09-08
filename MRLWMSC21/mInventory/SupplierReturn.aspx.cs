using MRLWMSC21Common;
using MRLWMSC21.mOutbound.BL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;

namespace MRLWMSC21.mInventory
{

    public partial class SupplierReturn : System.Web.UI.Page
    {
        public static CustomPrincipal cp = null;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            Page.Theme = "Inventory";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Orders / Supplier Returns");
            }
        }
        [WebMethod]
        public static List<SupplierRt> SupplierReturnlist(string POHeaderId, string SupplierInvoiceId, string WareHouseID)
        {

            return new SupplierReturn().GetSupplierList(POHeaderId, SupplierInvoiceId, WareHouseID);
        }
        [WebMethod]
        public static List<DropDownListDataFilter> getDocks( string WareHouseID)
        {

            return new SupplierReturn().getDock( WareHouseID);
        }
        [WebMethod]
        public static string TransferSupplierReturns(int POHeaderId, int TenantID, int WareHouseID, int SupplierInvoiceId, List<SupplierRt> lis, int DockId, int VehicletypeId, string DriverName, string MobileNum, string VehileNumber)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            string validation = "";
            try
            {
                string xml = null;
                var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                string s = string.Join("", lis);
                var settings = new XmlWriterSettings();
                using (StringWriter sw = new StringWriter())
                {
                    try
                    {
                        XmlSerializer serialiser = new XmlSerializer(typeof(List<SupplierRt>));
                        serialiser.Serialize(sw, lis, emptyNamepsaces);
                        xml = sw.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", " ");
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }

                string OBDNumber = new SupplierReturn().GetOBDNumber();
                string CustomerPONum = new SupplierReturn().GetCustomerPONo();
                string SONumber = new SupplierReturn().GetSONumber();

                string Query = "EXEC [dbo].[DummyOutbound_SupplierReturns] @OBDNUmber = "+DB.SQuote(OBDNumber) + ", @SONumber = "+DB.SQuote(SONumber) + ", @CustomerPO = "+DB.SQuote(CustomerPONum)+ ", @WareHouseID = "+WareHouseID+" "+
                                ",@POHeaderID = "+POHeaderId+ ", @SupplierInvoiceID = "+SupplierInvoiceId+ ", @DockID = "+DockId+ ", @VehicleTypeID = "+VehicletypeId+ ", @VehicleNo = "+DB.SQuote(VehileNumber)+ " , @DriverName= "+DB.SQuote(DriverName)+" "+
                                ", @MobileNo = "+DB.SQuote(MobileNum)+ ", @CreatedBy = "+cp.UserID+ ",@AccountID="+cp.AccountID+", @TenantID= "+TenantID+ ", @XML="+DB.SQuote(xml)+"";


                int Outboundid = DB.GetSqlN(Query);
                if(Outboundid != -1)
                {
                    validation = "Return Order created successfully with Outbound Ref.:" + OBDNumber;
                }
                else
                {
                    validation = "Error";
                }
                //if (Outboundid != -1)
                //{
                //    string Qe = "EXEC [dbo].[TRN_AllocateAndReserveInventory_SupplierReturs] @SupplierInvoiceID = " + SupplierInvoiceId + ", @WarehouseID = " + WareHouseID + ", @OutboundID= " + Outboundid + ", @CreatedBy =" + cp.UserID + "";
                //    int result = DB.GetSqlN(Qe);
                //    if (result == 1)
                //    {
                //        validation = "Return Order created successfully with Outbound Ref.:" + OBDNumber;
                //    }
                //    else
                //    {
                //        validation = "Outbound Created but stock allocation is not at done. Outbound Ref.: " + OBDNumber;
                //    }

                //}
                //else
                //{
                //    validation = "Error";
                //}
                //if(Outboundid != -1)
                //{
                //    string Qe = "EXEC [dbo].[TRN_AllocateAndReserveInventory_SupplierReturs] @SupplierInvoiceID = "+SupplierInvoiceId+", @WarehouseID = "+WareHouseID+", @OutboundID= "+ Outboundid + ", @CreatedBy ="+cp.UserID+"";
                //    int result = DB.GetSqlN(Qe);
                //    if(result == 1)
                //    {
                //        validation = "Return Order created successfully with Outbound Ref.:" + OBDNumber;
                //    }
                //    else
                //    {
                //        validation = "Outbound Created but stock allocation is not at done. Outbound Ref.: " + OBDNumber;
                //    }

                //}
                //else
                //{
                //    validation = "Error";
                //}
            }
            catch (Exception ex)
            {
                validation = "Error";
            }
            return validation;
        }
        public List<SupplierRt> GetSupplierList(string POHeaderId, string SupplierInvoiceId, string WareHouseID)
        {
            List<SupplierRt> Lst = new List<SupplierRt>();

            string Query = "EXEC [dbo].[sp_INV_GetSupplierReturnMaterial_NEW] @POHeaderID = " + POHeaderId + ", @SupplierInvoiceID = " + SupplierInvoiceId + ",@WarehouseID = " + WareHouseID + "";
            DataSet DS = DB.GetDS(Query, false);

            if (DS.Tables.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    SupplierRt SRT = new SupplierRt();
                    SRT.MaterialMasterID = Convert.ToInt32(row["MaterialMasterID"]);
                    SRT.MaterialMasterIDUomUID = Convert.ToInt32(row["MaterialMaster_InvUoMID"]);
                    SRT.MCode = row["MCode"].ToString();
                    SRT.Line = row["LineNumber"].ToString();
                    SRT.BatchNo = row["BatchNo"].ToString();
                    SRT.MfgDate = row["MfgDate"].ToString();
                    SRT.ExpDate = row["ExpDate"].ToString();
                    SRT.SerialNo = row["SerialNo"].ToString();
                    SRT.ProjectRefNo = row["ProjectRefNo"].ToString();
                    SRT.MRP = row["MRP"].ToString();
                    SRT.PickedQty = Convert.ToDecimal(row["AvailableQty"]);
                    SRT.PendingReturnQty = Convert.ToDecimal(row["PendingReturnQty"]);
                    //SRT.ReturnQty = Convert.ToDecimal(row["PendingReturnQty"]);
                    SRT.KitplannerID= row["KitPlannerID"].ToString();
                    SRT.Isselected = false;
                    SRT.IsKitParent= Convert.ToInt32(row["IsKitParent"]);
                    SRT.Location = row["Location"].ToString();
                    SRT.CartonCode = row["CartonCode"].ToString();
                    SRT.StorageLocation = row["StorageLocation"].ToString();
                    SRT.GRNUpdateID = Convert.ToInt32(row["GRNHeaderID"]);
                    Lst.Add(SRT);
                }
            }

            return Lst;
        }

        public List<DropDownListDataFilter> getDock(string WareHouseID = "0")
        {
            List<DropDownListDataFilter> doclst = new List<DropDownListDataFilter>();
            //DataSet DS = DB.GetDS("SELECT DockID, DockNo FROM GEN_Dock DOC WHERE WarehouseID = " + WareHouseID + " AND  IsDeleted=0 and IsActive=1 and DockTypeID=2 and  DOCKID NOT IN (SELECT  DISTINCT  DockID FROM OBD_VLPD WHERE  StatusId >= 2 AND 4 < StatusId) ", false);
            DataSet DS = DB.GetDS("exec [SP_GetDocks]@IsOutbound=1,@Warehouseid=" + WareHouseID, false);
            if (DS.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    doclst.Add(new DropDownListDataFilter()
                    {
                        Id = Convert.ToInt32(row["DockID"]),
                        Name = row["DockNo"].ToString(),

                    });

                }
            }
            return doclst;
        }

        private String GetOBDNumber()
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            String OBDNumber = "";
            try
            {

                int length = Convert.ToInt32(DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='OutboundDetails.aspx' ,@TenantID=" + cp.TenantID));



                String NewOBDNumber = DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='dummyoutboundforsupplierreturn_prefix' ,@TenantID=" + cp.TenantID) + (Convert.ToInt16(DateTime.Now.Year) % 100);           //add year code to ponumber

                String OldOBDNumber = DB.GetSqlS("select top 1 OBDNumber as S from OBD_Outbound where AccountID="+cp.AccountID+" AND OBDNumber like '" + NewOBDNumber + "%' order by OBDNumber desc");

                int power = (Int32)Math.Pow((double)10, (double)(length - 1));          //getting minvalue of prifix length

                String newvalue = "";
                if (OldOBDNumber != "" && NewOBDNumber.Equals(OldOBDNumber.Substring(0, NewOBDNumber.Length)))        //if ponumber is existed and same year ponumber  enter
                {
                    String temp = OldOBDNumber.Substring(NewOBDNumber.Length, length);                            //getting number of last prifix
                    Int32 number = Convert.ToInt32(temp);
                    number++;


                    while (power > 1)                                                                           //add '0' to number at left side for get 
                    {
                        if (number / power > 0)
                        {
                            break;
                        }
                        newvalue += "0";
                        power /= 10;
                    }
                    newvalue += number;
                }
                else
                {                                                                                           //other wise generate first number 
                    for (int i = 0; i < length - 1; i++)
                        newvalue += "0";
                    newvalue += "1";
                }

                NewOBDNumber += newvalue;
                OBDNumber = NewOBDNumber;
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
            return OBDNumber;
        }
        private String GetCustomerPONo()
        {
            Random generateNo = new Random();
            return "DCUSPO" + generateNo.Next(1000, 10000);
        }
        private String GetSONumber()
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            String SONumber = "";
            try
            {
                String NewSONumber = DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='dummysoheaderforsupplierreturn_prefix' ,@TenantID=" + cp.TenantID); //DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='inbounddetails.aspx.cs.StoreRefNo_Length' ,@TenantID=" + cp.TenantID); //"D" + DB.GetSqlS("select SysConfigValue as S from SYS_SystemConfiguration where TenantID=" + cp.TenantID + " and SysConfigKeyID= (select SysConfigKeyID from SYS_SysConfigKey where SysConfigKey=N'salesorder.aspx.cs.SO_Prefix') ");
                NewSONumber += "" + (Convert.ToInt16(DateTime.Now.Year) % 100);           //add year code to ponumber

                int length = Convert.ToInt16(DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='salesorder.aspx.cs.SO_Length' ,@TenantID=" + cp.TenantID)); //Convert.ToInt32(DB.GetSqlS("select SysConfigValue as S from SYS_SystemConfiguration as N where TenantID=" + cp.TenantID + " and SysConfigKeyID= (select SysConfigKeyID from SYS_SysConfigKey where SysConfigKey=N'salesorder.aspx.cs.SO_Length') "));

                //String OldSONumber = DB.GetSqlS("select TOP 1 SONumber as S from ORD_SOHeader where SOTypeID =7 and TenantID=" + cp.TenantID + "  ORDER BY SONumber desc ");
                String OldSONumber = DB.GetSqlS("select TOP 1 SONumber as S from ORD_SOHeader where AccountID="+cp.AccountID+" AND SONumber like '" + NewSONumber + "%' ORDER BY SONumber desc ");



                int power = (Int32)Math.Pow((double)10, (double)(length - 1));          //getting minvalue of prifix length

                String newvalue = "";
                if (OldSONumber != "" && NewSONumber.Equals(OldSONumber.Substring(0, NewSONumber.Length)))        //if ponumber is existed and same year ponumber  enter
                {
                    String temp = OldSONumber.Substring(NewSONumber.Length, length);                            //getting number of last prifix
                    Int32 number = Convert.ToInt32(temp);
                    number++;


                    while (power > 1)                                                                           //add '0' to number at left side for get 
                    {
                        if (number / power > 0)
                        {
                            break;
                        }
                        newvalue += "0";
                        power /= 10;
                    }
                    newvalue += number;
                }
                else
                {                                                                                           //other wise generate first number 
                    for (int i = 0; i < length - 1; i++)
                        newvalue += "0";
                    newvalue += "1";
                }

                NewSONumber += newvalue;
                SONumber = NewSONumber;
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
            return SONumber;
        }
        public class SupplierRt
        {
            public string Line { get; set; }
            public string MCode { get; set; }
            public int MaterialMasterID { get; set; }
            public string MfgDate { get; set; }
            public string ExpDate { get; set; }
            public string SerialNo { get; set; }
            public int MaterialMasterIDUomUID { get; set; }
            public string BatchNo { get; set; }
            public string ProjectRefNo { get; set; }
            public string MRP { get; set; }
            public decimal PickedQty { get; set; }
            public decimal PendingReturnQty { get; set; }
            public decimal ReturnQty { get; set; }
            public string KitplannerID { get; set; }
            public Boolean Isselected { get; set; }
            public int IsKitParent { get; set; }
            public string Location { get; set; }
            public string CartonCode { get; set; }
            public string StorageLocation { get; set; }
            public int GRNUpdateID { get; set; }


        }
    }
}