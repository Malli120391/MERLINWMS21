using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using MRLWMSC21Common;
using System.Web.UI.HtmlControls;
using System.Globalization;

namespace MRLWMSC21.mManufacturingProcess
{
    public partial class PhantomGoodsIn : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
      //  public  int Count = 0;

        protected void page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Inventory";
        }

        protected void page_Init(object sender, EventArgs e)
        {

            if (CommonLogic.QueryString("ProId") != "" && CommonLogic.QueryString("lno") != "")
            {

                String ProdId = CommonLogic.QueryString("ProId");

                String LineNumber = CommonLogic.QueryString("lno");

                AddLineitems(ProdId, LineNumber);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            DesignLogic.SetInnerPageSubHeading(this.Page, "PHANTOM JOB ORDER FOR INBOUND");
            if (!IsPostBack)
            {
                Page.Validate();
                if (CommonLogic.QueryString("ProId") != "" && CommonLogic.QueryString("lno") != "")
                {
                    BuildPOQuantityDetails();
                    IDataReader rsposoCount = DB.GetRS("[sp_MFG_GetPOSOCountForPhantom]  @ProductionOrderHeaderID=" + CommonLogic.QueryString("Proid"));

                    rsposoCount.Read();

                    int POCount = DB.RSFieldInt(rsposoCount, "POCount");
                    int SOCount = DB.RSFieldInt(rsposoCount, "SOCount");
                    if (POCount == SOCount)
                    {
                        lnkpo.Enabled = false;

                    }
                    rsposoCount.Close();
                }
                else
                {
                    tbjoborder_FormDetails.Visible = false;
                }


            }
        }

        private void BuildPOQuantityDetails()
        {
            tbjoborder_FormDetails.Visible = false;

            try
            {

                String prodid = CommonLogic.QueryString("ProId");

                String LineNumber = CommonLogic.QueryString("lno");


                String KitCod = DB.GetSqlS("select KitCode  as S from MFG_ProductionOrderHeader  poh join MMT_MaterialMaster_Revision mr on mr.MaterialMasterRevisionID=poh.MaterialMasterRevisionID  join MMT_MaterialMaster mm on mm.MaterialMasterID=mr.MaterialMasterID where ProductionOrderHeaderID=" + prodid);

                txtin_kitcode.Text = KitCod;

                hifkitcode.Value = prodid;
                txtin_poitemline.Text = LineNumber;

                DataTable dsJoborder = GetDataforjob(prodid, LineNumber);

                if(dsJoborder.Rows.Count==0)
                {
                    tbjoborder_FormDetails.Visible = false;
                    pnljoborderList.Visible = false;
                    dsJoborder.Dispose();
                    resetError("No data bound for this material",true);
                    return;
                }
                
               if (DB.GetSqlN("SELECT TOP 1 DeliveryStatusID AS N FROM OBD_Outbound OBD JOIN OBD_Outbound_ORD_CustomerPO C_PO ON C_PO.OutboundID=OBD.OutboundID JOIN ORD_SODetails SOD ON SOD.SOHeaderID=C_PO.SOHeaderID JOIN MFG_SOPO_ProductionOrder SOPO ON SOPO.SOPOHeaderID=SOD.SOHeaderID and SOPOTypeID=2 WHERE ProductionOrderHeaderID=" + CommonLogic.QueryString("ProId")) == 1)
                {
                    resetError("Goods out is not done for this material",true);
                    pnljoborderList.Visible = false;
                    tbjoborder_FormDetails.Visible = false;
                   return;

                }
              
                  else
               {
               
                tbjoborder_FormDetails.Visible = true;

                pnljoborderList.Visible = true;
                 }

                Page.Validate();
            }
            catch(Exception ex)
            {
                resetError("Error while build data", true);
            }
        }

        public static DataTable GetDataforjob(String prod, String Lno)

        {
            String scmdjoborder = "EXEC [dbo].[sp_MFG_GetDataForPhantomGoodsIn] @ProductionOrderHeaderID=" + prod + ",@LineNumber=" + Lno;
            DataSet dsjoborderList = DB.GetDS(scmdjoborder, false);
            
            return dsjoborderList.Tables[0];
            
        }
        private void AddLineitems(string ProdId, string LineNumber)
        {

            try
            {

                IDataReader rsJoborder = DB.GetRS("EXEC [dbo].[sp_MFG_GetDataForPhantomGoodsIn] @ProductionOrderHeaderID=" + ProdId + ",@LineNumber=" + LineNumber);
                HtmlTableRow htRow = null;
                HtmlTableCell htCell;

                TextBox txtbatchno;
                TextBox txtQuantity;
                Label ltliteral;
                Label lbl;

               
                    while (rsJoborder.Read())
                    {
                       
                        htRow = new HtmlTableRow();
                        htCell = new HtmlTableCell();

                        htCell.Align = "left";

                        ltliteral = new Label();
                        lbl = new Label();
                        ltliteral.ID = "ltM" + DB.RSFieldInt(rsJoborder, "SODetailsID") + DB.RSField(rsJoborder, "MCode");

                        ltliteral.Text = "<nobr> " + DB.RSField(rsJoborder, "MCode") + " </nobr> <br/>";
                        ltliteral.ForeColor = System.Drawing.Color.DarkBlue;

                        lbl.Text = " <font color=DarkOrange > [" + DB.RSField(rsJoborder, "OEMPartNo") + " ]  </font>   " + " <b> <font color=black >| </font></b> <font color=#009999 >    " + DB.RSField(rsJoborder, "UoMQty") + "</font >";

                        lbl.Font.Size = 10;
                        
                        htCell.Controls.Add(ltliteral);
                        htCell.Controls.Add(lbl);
                        htCell.Controls.Add(new LiteralControl("&nbsp;"));
                        htRow.Cells.Add(htCell);

                        htCell = new HtmlTableCell();
                        txtbatchno = new TextBox();

                        txtbatchno.ID = "txtB" + DB.RSFieldInt(rsJoborder, "SODetailsID") + DB.RSField(rsJoborder, "BatchNo");

                        txtbatchno.Text = DB.RSField(rsJoborder, "BatchNo");

                        txtbatchno.Enabled = false;
                        
                        //  txtbatchno.Attributes.Add("onblur", "javascript:return focuslostforbtc(" + txtbatchno.Text + ",this);");

                        txtbatchno.EnableTheming = false;

                        htCell.Controls.Add(txtbatchno);

                        htRow.Cells.Add(htCell);

                        htCell = new HtmlTableCell();
                        txtQuantity = new TextBox();

                        txtQuantity.ID = "txtQ" + DB.RSFieldInt(rsJoborder, "SODetailsID") + DB.RSFieldDecimal(rsJoborder, "SOQuantity");

                        txtQuantity.Text = DB.RSFieldDecimal(rsJoborder, "SOQuantity").ToString();
                        txtQuantity.Width = 80;

                        //  txtQuantity.Attributes.Add("onblur", "javascript:return focuslostforqty(" + txtQuantity.Text + ",this);");
                        txtQuantity.EnableTheming = false;

                        txtQuantity.Enabled = false;
                        htCell.Controls.Add(txtQuantity);

                        htRow.Cells.Add(htCell);
                        tbljoborder.Rows.Add(htRow);

                        IDataReader rsQty = DB.GetRS("select top 1 (SOQuantity/BOMQuantity) as Qty,MCode from MFG_ProductionOrderHeader PROH JOIN MMT_MaterialMaster_Revision MMV ON MMV.MaterialMasterRevisionID=PROH.MaterialMasterRevisionID JOIN MFG_SOPO_ProductionOrder SOPO ON SOPO.ProductionOrderHeaderID=PROH.ProductionOrderHeaderID AND SOPO.SOPOTypeID=2 JOIN ORD_SODetails SOD ON SOD.SOHeaderID=SOPO.SOPOHeaderID JOIN MFG_BOMDetails BOD ON BOD.BOMMaterialMasterID=SOD.MaterialMasterID AND BOD.IsDeleted=0 join MMT_MaterialMaster mm on mm.MaterialMasterID=MMV.MaterialMasterID where PROH.ProductionOrderHeaderID=" + CommonLogic.QueryString("Proid") + " AND SOD.LineNumber=" + LineNumber);
                        rsQty.Read();
                        lbldata.Text = "<br/>";

                        lbldata.Text += " Finished Good Part No.: <b> <font color=DarkGreen size='4.999'>   " + DB.RSField(rsQty, "MCode") + "</font> </b>   Quantity:  <b> <font color=DarkGreen size='4.999'>" + DB.RSFieldDecimal(rsQty, "Qty").ToString("0.00") + "</font> </b>";
                        rsQty.Close();

                    }

                    rsJoborder.Close();

                }
            


            catch (Exception ex)
            {
                resetError("Error while build the data", true);
            }
            
        }

      

        private void resetError(string error, bool isError)
        {
           
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);

        }

        protected void lnkin_GetDetails_Click(object sender, EventArgs e)
        {
             Page.Validate("getDetails");
             if (IsValid)
             {
                 try
                 {
                    
                     Response.Redirect("PhantomGoodsIn.aspx?ProId=" + hifkitcode.Value + "&lno=" + txtin_poitemline.Text.Trim());


                 }
                 catch (Exception ex)
                 {
                     resetError("Error try again", true);
                 }
             }    
        }


        protected void lnkpo_Click(object sender, EventArgs e)
        {
          
            String Kitcode = txtin_kitcode.Text.Trim();

            String LineNumber = txtin_poitemline.Text.Trim();

            String PONumber = "";
            String InvoiceNumber = "";
            String StoreRefNo = "";

            IDataReader rsCount = DB.GetRS("[sp_MFG_GetPOSOCountForPhantom]  @ProductionOrderHeaderID=" + CommonLogic.QueryString("Proid"));
           
            rsCount.Read();
            
                 int POCount=DB.RSFieldInt(rsCount,"POCount");
                 int SOCount=DB.RSFieldInt(rsCount,"SOCount");


                 if (POCount == 0)
                 {
                     PONumber = GetDummyPONumber();
                     InvoiceNumber = GetDummyInvoiceNo();

                 }

                 if (POCount == SOCount-1)
                 {
                    
                     StoreRefNo = GetDummyStoreRefNo();
                 }

            try
            {
                StringBuilder cMdUpdatePodetails = new StringBuilder();

                cMdUpdatePodetails.Append("DECLARE @NewResult int EXEC sp_MFG_UpdatePhantomGoodsIn");
                cMdUpdatePodetails.Append("  @ProductionOrderHeaderID=" + CommonLogic.QueryString("Proid"));
                cMdUpdatePodetails.Append(",@PONumber=" + (PONumber != "" ? DB.SQuote(PONumber) : "NULL"));
                cMdUpdatePodetails.Append(",@InvoiceNumber=" + (InvoiceNumber != "" ? DB.SQuote(InvoiceNumber) : "NULL"));
                cMdUpdatePodetails.Append(",@StoreRefNo=" + (StoreRefNo != "" ? DB.SQuote(StoreRefNo) : "NULL"));
                cMdUpdatePodetails.Append(",@LineNumber=" + LineNumber);
                cMdUpdatePodetails.Append(",@CreatedBy=" + cp.UserID);

                cMdUpdatePodetails.Append(",@Result=@NewResult output select @NewResult as N");

                int Result = DB.GetSqlN(cMdUpdatePodetails.ToString());
                if (Result == 1 || Result == 2)

                    resetError( "Line number is already updated ",false);

                else if (Result == 3)
                {
                    resetError("New line item is added",false);
                   
                }
                else if (Result == 4)
                {
                    resetError(" Successfully updated with Store ref. no. :"+StoreRefNo, false);
                    
                }
                else

                    resetError("not updated ", true);
               
            }

            catch (Exception ex)
            {
                resetError("Error while updating", true);
            }
            rsCount.Close();
        }

        private String GetDummyPONumber()
        {

                String NewPONumber = "";
                try
                {

                    NewPONumber = DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='dummypoheaderforphantom_prefix' ,@TenantID=" + cp.TenantID);//"DP" + DB.GetSqlS("select SysConfigValue as S from SYS_SystemConfiguration where TenantID=" + cp.TenantID + " and SysConfigKeyID= (select SysConfigKeyID from SYS_SysConfigKey where SysConfigKey=N'purchaseorder.aspx.cs.PO_Prefix') ");
                    NewPONumber += "" + (Convert.ToInt16(DateTime.Now.Year) % 100);           

                    int length = Convert.ToInt16(DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='purchaseorder.aspx.cs.PO_Length' ,@TenantID=" + cp.TenantID)); //Convert.ToInt32(DB.GetSqlS("select SysConfigValue as S from SYS_SystemConfiguration as N where TenantID=" + cp.TenantID + " and SysConfigKeyID= (select SysConfigKeyID from SYS_SysConfigKey where SysConfigKey=N'purchaseorder.aspx.cs.PO_Length') "));

                    String OldPONumber = DB.GetSqlS("select TOP 1 PONumber as S from ORD_POHeader where PONumber like '" + NewPONumber + "%' and TenantID=" + cp.TenantID + "  ORDER BY PONumber desc ");


                    int power = (Int32)Math.Pow((double)10, (double)(length - 1));          

                    String newvalue = "";

                   

                        if (OldPONumber != "" && NewPONumber.Equals(OldPONumber.Substring(0, NewPONumber.Length)))

                      
                        {
                            String temp = OldPONumber.Substring(NewPONumber.Length, length);                            
                            Int32 number = Convert.ToInt32(temp);
                            number++;


                            while (power > 1)                                                                           
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
                        {                                                                                           
                            for (int i = 0; i < length - 1; i++)
                                newvalue += "0";
                            newvalue += "1";
                        }

                        NewPONumber += newvalue;
                        
                    
                }
                catch (Exception ex)
                {
                    NewPONumber = "";
                    
                }
            
                return NewPONumber;
            }
        

        private String GetDummyInvoiceNo()
        {
            Random generateNo = new Random();
            return "DINV" + generateNo.Next(1000, 10000);
        }

      

        private String GetDummyStoreRefNo()
        {

            string WarehouseID = "1";

            String vNewStoreRefNUmber = null;


            try
            {
                String StoreRefNoLength = DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='inbounddetails.aspx.cs.StoreRefNo_Length' ,@TenantID=" + cp.TenantID);

                vNewStoreRefNUmber = DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='dummyinboundforphantom_prefix' ,@TenantID=" + cp.TenantID) + DB.GetSqlS("select WarehouseGroupCode as S from GEN_Warehouse GEN_W  left join GEN_WarehouseGroup GEN_WG on GEN_WG.WarehouseID=GEN_W.WarehouseID where  GEN_WG.WarehouseID=" + WarehouseID);

                String OldStoreRefNumber = DB.GetSqlS("select top 1  StoreRefNo AS S from INB_Inbound where  TenantID=" + cp.TenantID + " and StoreRefNo like '" + vNewStoreRefNUmber + "%' order by InboundID DESC "); // Get Previous StoreRefNo


                vNewStoreRefNUmber += (Convert.ToInt16(DateTime.Now.Year) % 100);           
               
                int length = Convert.ToInt32(StoreRefNoLength);                   
                int power = (Int32)Math.Pow((double)10, (double)(length - 1));          


                if (OldStoreRefNumber != "" && vNewStoreRefNUmber.Equals(OldStoreRefNumber.Substring(0, vNewStoreRefNUmber.Length)))       
                {
                    String temp = OldStoreRefNumber.Substring(vNewStoreRefNUmber.Length, length);                            
                    Int32 number = Convert.ToInt32(temp);
                    number++;


                    while (power > 1)                                                                           
                    {
                        if (number / power > 0)
                        {
                           
                            break;
                        }
                        vNewStoreRefNUmber += "0";
                        power /= 10;
                    }
                    vNewStoreRefNUmber += "" + number;
                }
                else
                {                                                                                           
                    for (int i = 0; i < length - 1; i++)
                        vNewStoreRefNUmber += "0";
                    vNewStoreRefNUmber += "1";
                }

               
            }
            catch (Exception)
            {
                vNewStoreRefNUmber = "";
               
            }

            return vNewStoreRefNUmber;
        }

        protected void lnkin_cancel_Click1(object sender, EventArgs e)
        {
            try
            {

                Response.Redirect("PhantomGoodsIn.aspx");

            }
            catch (Exception ex)
            {
                resetError("Error try again", true);
            }
        }

        
      
    }
}