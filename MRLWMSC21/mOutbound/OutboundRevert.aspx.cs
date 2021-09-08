using MRLWMSC21Common;
using MRLWMSC21.mOutbound.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRLWMSC21.mOutbound
{
    public partial class OutboundRevert : System.Web.UI.Page
    {
        public static CustomPrincipal cp;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            Page.Theme = "Outbound";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DesignLogic.SetInnerPageSubHeading(this.Page, "Revert Outbound");
        }
        [WebMethod]
        public static List<DropDownListDataFilter> getOBDNumbers(string prefix)
        {
            try
            {
                OutBoundBL objbl = new OutBoundBL();
                return objbl.getOBDNumbersForOBDRevert(prefix, cp.AccountID,5);
            }
            catch
            {
                return null;
            }

        }
        [WebMethod]
        public static List<DropDownListDataFilter> getOBDNumbersForPGIrevert(string prefix)
        {
            try
            {
                OutBoundBL objbl = new OutBoundBL();
                return objbl.getOBDNumbersForOBDRevert(prefix, cp.AccountID, 6);
            }
            catch
            {
                return null;
            }

        }
        [WebMethod]
        public static List<DropDownListDataFilter> getItems(string prefix)
        {
            try
            {
                OutBoundBL objbl = new OutBoundBL();
                return objbl.getItemsForOBDRevert(prefix, cp.AccountID);
            }
            catch
            {
                return null;
            }

        }
        [WebMethod]
        public static List<RevertOutboundData> GetOutBoundDetailsDataForRevert(RevertSearch obj)
        {
            try
            {
                OutBoundBL objbl = new OutBoundBL();
                return objbl.GetOutBoundDetailsDataForRevert(obj,cp.AccountID);
            }
            catch
            {
                return null;
            }

        }
        [WebMethod]
        public static string RevertPGI(int outboundid)
        {
            try
            {
                OutBoundBL objbl = new OutBoundBL();
                return objbl.RevertPGI(outboundid);
            }
            catch(Exception e)
            {
                return "Error : Error while reverting";
            }

        }
        [WebMethod]
        public static string RevertOutbounDetails(string outboundid, string SodetailsId, string Qty,string MMID)
        {
            try
            {
                return DB.GetSqlS("EXEC [SP_OBD_revertOBDORLineItemWise] @MMID="+ MMID + ",@OutboundID=" + outboundid + ",@SodetailsID=" + SodetailsId + ",@ITEMREVERTQTY=" + Qty + ",@Createdby=" + cp.UserID);
                
            }
            catch (Exception e)
            {
                return e.Message;
            }




        }
        [WebMethod]
        public static string RevertOutbound(string OBDnumber)
        {
            try
            {
                int outboundid = DB.GetSqlN("select isnull((select OutboundID from OBD_Outbound where OBDNumber="+DB.SQuote(OBDnumber )+ "),0) as N");
                if(outboundid==0)
                {
                    return "-7";
                }
                return DB.GetSqlS("EXEC [SP_OBD_revertOBDORLineItemWise] @OutboundID=" + outboundid + ",@SodetailsID=" + 0 + ",@ITEMREVERTQTY=" + 0 + ",@Createdby=" + cp.UserID);
               
            }
            catch (Exception e)
            {
                return e.Message;
            }




        }
    }
}