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
    public partial class CustomerReturns : System.Web.UI.Page
    {
        public static CustomPrincipal cp;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            Page.Theme = "Inventory";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Orders / Customer Returns");
            }
        }

        [WebMethod]
        public static List<CustomerRt> GetCutreturnlist(int OutboundID, int CustomerPOID, int WareHouseID)
        {

            return new CustomerReturns().GetCustomerList(OutboundID, CustomerPOID, WareHouseID);
        }

        [WebMethod]
        public static string Transfer(List<CustomerRt> lstCutrt, string WareHouseID, int TenantID, int OutboundID)
        {
            string validation = "";
            try
            {
                string xml = null;
                var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                string s = string.Join("", lstCutrt);
                var settings = new XmlWriterSettings();
                using (StringWriter sw = new StringWriter())
                {
                    try
                    {
                        XmlSerializer serialiser = new XmlSerializer(typeof(List<CustomerRt>));
                        serialiser.Serialize(sw, lstCutrt, emptyNamepsaces);
                        xml = sw.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", " ");
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                
                string InboundNo = new CustomerReturns().GetDummyStoreRefNo(WareHouseID, TenantID);
                string PoNumber = new CustomerReturns().GetDummyPONumber();
                string invoiceno = new CustomerReturns().GetDummyInvoiceNo();

               
                string Query = "EXEC [dbo].[sp_INV_DummyInboundForCustomerReturns_NEW] @PONumber =  " + DB.SQuote(PoNumber) + ", @INBOUNDNUMBER = " + DB.SQuote(InboundNo) + ", @InvoiceNumber = " + DB.SQuote(invoiceno) + ", @UserID=" + cp.UserID + ",@AccountID="+cp.AccountID+", @TenantID= " + TenantID + ", @OutboundID = " + OutboundID + ", @WareHouseID=" + WareHouseID + ", @XML=" + DB.SQuote(xml) + "";
                int result = DB.GetSqlN(Query);
                if(result == 1)
                {
                    validation = InboundNo;
                }
                else
                {
                    validation = "Error";
                }
               
            }
            catch(Exception ex)
            {
                validation = "Error";
            }
           
            return validation;
        }

        public List<CustomerRt> GetCustomerList (int OutboundID, int CustomerPOID , int WareHouseID)
        {
            List<CustomerRt> Lst = new List<CustomerRt>();

            string Query = "EXEC [dbo].[sp_INV_GetCustomerReturnsMaterial_NEW] @OutboundID = "+OutboundID+ ", @Outbound_CustomerPOID = "+ CustomerPOID + ", @WarehouseID = "+ WareHouseID + "";
            DataSet DS = DB.GetDS(Query, false);
            
            if(DS.Tables.Count != 0)
            {
                foreach(DataRow row in DS.Tables[0].Rows)
                {
                    CustomerRt CRT = new CustomerRt();
                    CRT.MaterialMasterID = Convert.ToInt32(row["MaterialMasterID"]);
                    CRT.MaterialMasterIDUomUID = Convert.ToInt32(row["MaterialMaster_IUoMID"]);
                    CRT.MCode = row["MCode"].ToString();
                    CRT.Line = row["LineNumber"].ToString();
                    CRT.BatchNo = row["BatchNo"].ToString();
                    CRT.MfgDate = row["MfgDate"].ToString();
                    CRT.ExpDate = row["ExpDate"].ToString();
                    CRT.SerialNo = row["SerialNo"].ToString();
                    CRT.MRP = row["MRP"].ToString();
                    CRT.ProjectRefNo = row["ProjectRefNo"].ToString();
                    CRT.PickedQty = Convert.ToDecimal(row["PickedQuantity"]);
                    CRT.PendingReturnQty = Convert.ToDecimal(row["Remaining"]);
                    CRT.ReturnQty = Convert.ToDecimal(row["Remaining"]);
                    CRT.Isselected = false;
                    Lst.Add(CRT);
                }
            }

            return Lst;
        }

        private String GetDummyStoreRefNo(string WareHouseID, int  tenantID)
        {

            string WarehouseID = WareHouseID;
            String vNewStoreRefNUmber = null;
            try
            {
                String StoreRefNoLength = DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='inbounddetails.aspx.cs.StoreRefNo_Length' ,@TenantID=" + tenantID);  //DB.GetSqlS("select SysConfigValue as S from SYS_SystemConfiguration SYS_SC left join SYS_SysConfigKey SYS_SCK on  SYS_SCK.SysConfigKeyID=SYS_SC.SysConfigKeyID where TenantID=" + cp.TenantID + " and SYS_SCK.SysConfigKey='inbounddetails.aspx.cs.StoreRefNo_Length' and SYS_SCK.IsActive=1");

                vNewStoreRefNUmber = DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='dummyinboundforcustomerreturn_prefix' ,@TenantID=" + tenantID) + DB.GetSqlS("select WarehouseGroupCode as S from GEN_Warehouse GEN_W  left join GEN_WarehouseGroup GEN_WG on GEN_WG.WarehouseGroupID=GEN_W.WarehouseGroupID where  GEN_W.WarehouseID=" + WarehouseID);

                //String OldStoreRefNumber = DB.GetSqlS("select top 1  StoreRefNo AS S from INB_Inbound  INB JOIN TPL_Tenant TL ON TL.TenantID =  INB.TenantID where INB.StoreRefNo like '" + vNewStoreRefNUmber + "%' and TL.AccountId=" + cp.AccountID + " order by InboundID DESC "); // Get Previous StoreRefNo

                String OldStoreRefNumber = DB.GetSqlS("select top 1  StoreRefNo AS S from INB_Inbound where AccountID="+cp.AccountID+" AND StoreRefNo like '" + vNewStoreRefNUmber + "%'  order by InboundID DESC "); // Get Previous StoreRefNo


                vNewStoreRefNUmber += (Convert.ToInt16(DateTime.Now.Year) % 100);           //add year code to StoreRefNUmber
                                                                                            //vNewStoreRefNUmber += "" + (2014 % 100);
                int length = Convert.ToInt32(StoreRefNoLength);                   //get prifix length
                int power = (Int32)Math.Pow((double)10, (double)(length - 1));          //get minvalue of prifix length


                if (OldStoreRefNumber != "" && vNewStoreRefNUmber.Equals(OldStoreRefNumber.Substring(0, vNewStoreRefNUmber.Length)))        //get same year StoreRefNo if StoreRefNo is exists
                {
                    String temp = OldStoreRefNumber.Substring(vNewStoreRefNUmber.Length, length);                            //get number of last prifix
                    Int32 number = Convert.ToInt32(temp);
                    number++;


                    while (power > 1)                                                                           //add '0' to number at left side for get 
                    {
                        if (number / power > 0)
                        {
                            //newvalue += number;
                            break;
                        }
                        vNewStoreRefNUmber += "0";
                        power /= 10;
                    }
                    vNewStoreRefNUmber += "" + number;
                }
                else
                {                                                                                           //other wise generate first number 
                    for (int i = 0; i < length - 1; i++)
                        vNewStoreRefNUmber += "0";
                    vNewStoreRefNUmber += "1";
                }
            }
            catch (Exception ex)
            {
                vNewStoreRefNUmber = "";
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                // resetError("Error while generating StoreRef#", true);
            }

            return vNewStoreRefNUmber;
        }

        private String GetDummyPONumber()
        {
            String NewPONumber = "";
            try
            {
                NewPONumber = DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='dummypoheaderforcustomerreturn_prefix' ,@TenantID=" + cp.TenantID); //"D"+DB.GetSqlS("select SysConfigValue as S from SYS_SystemConfiguration where TenantID=" +cp.TenantID + " and SysConfigKeyID= (select SysConfigKeyID from SYS_SysConfigKey where SysConfigKey=N'purchaseorder.aspx.cs.PO_Prefix') ");
                NewPONumber += "" + (Convert.ToInt16(DateTime.Now.Year) % 100);           //add year code to ponumber

                int length = Convert.ToInt16(DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='purchaseorder.aspx.cs.PO_Length' ,@TenantID=" + cp.TenantID)); //Convert.ToInt32(DB.GetSqlS("select SysConfigValue as S from SYS_SystemConfiguration as N where TenantID=" + cp.TenantID + " and SysConfigKeyID= (select SysConfigKeyID from SYS_SysConfigKey where SysConfigKey=N'purchaseorder.aspx.cs.PO_Length') "));


                //String OldPONumber = DB.GetSqlS("select TOP 1 PONumber as S from ORD_POHeader where POTypeID=6 and TenantID=" + cp.TenantID + "  ORDER BY PONumber desc ");
                String OldPONumber = DB.GetSqlS("select TOP 1 PONumber as S from ORD_POHeader where AccountID ="+cp.AccountID+" AND  PONumber like '" + NewPONumber + "%' ORDER BY PONumber desc ");


                int power = (Int32)Math.Pow((double)10, (double)(length - 1));          //getting minvalue of prifix length

                String newvalue = "";
                if (OldPONumber != "" && NewPONumber.Equals(OldPONumber.Substring(0, NewPONumber.Length)))        //if ponumber is existed and same year ponumber  enter
                {
                    String temp = OldPONumber.Substring(NewPONumber.Length, length);                            //getting number of last prifix
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

                NewPONumber += newvalue;
                // txtPONumber.Text = NewPONumber;
            }
            catch (Exception ex)
            {
                NewPONumber = "";
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                // resetError(ex.ToString(), true);
            }
            return NewPONumber;
        }

        private String GetDummyInvoiceNo()
        {
            Random generateNo = new Random();
            return "DINV" + generateNo.Next(1000, 10000);
        }

        public class CustomerRt
        {
            public string Line { get; set; }
            public string MCode { get; set; }
            public int MaterialMasterID { get; set; }
            public string MfgDate { get; set; }
            public string ExpDate { get; set; }
            public string SerialNo { get; set; }
            public string MRP { get; set; }
            public int MaterialMasterIDUomUID { get; set; }
            public string BatchNo { get; set; }
            public string ProjectRefNo { get; set; }
            public decimal PickedQty { get; set; }
            public decimal PendingReturnQty { get; set; }
            public decimal ReturnQty { get; set; }
            public Boolean Isselected { get; set; }


        }

    }
}