using MRLWMSC21Common;
using MRLWMSC21.mOutbound.BL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRLWMSC21.mOutbound
{
    public partial class ReleaseOBDItems : System.Web.UI.Page
    {
        static List<int> lst = new List<int>();
        public static CustomPrincipal cp;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            Page.Theme = "Outbound";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Release Outbound Items");
            }
        }
        [WebMethod]
        public static List<ReleaseIndividualOBD> GetOBDWiseitem(string OBDid)
        {
            try
            {
                OutBoundBL OBL = new OutBoundBL();
                return OBL.GetOBDwiseItem(OBDid);
            }
            catch
            {
                return null;
            }

        }
        [WebMethod]
        public static int getOBDStatus(string OBDid)
        {
            try
            {
                return DB.GetSqlN("select DeliveryStatusID as N from obd_outbound where OutboundID="+ OBDid);
            }
            catch
            {
                return 0;
            }

        }
        [WebMethod]
        public static ReleaseoutboundResult saveBulkReleaseItems(List<ReleaseIndividualOBD> Items, int VLPDPicking)
        {
            string OutBoundId = Items[0].OutboundID.ToString();
            int wareHouseid=GetWarehouseIDByOutbound(OutBoundId);
            ReleaseoutboundResult Responce = new ReleaseoutboundResult();
            if (!lst.Contains(wareHouseid))
            {
                lst.Add(wareHouseid);
                try
                {
                    List<ReleaseIndividualOBD> glist = new List<ReleaseIndividualOBD>();
                    OutBoundBL mp = new OutBoundBL();
                    Responce =  mp.UpsertReleaseItem(Items, VLPDPicking);

                }
                catch (Exception e)
                {

                    Responce.Status = -1; 
                }

                lst.Remove(wareHouseid);
                
            }
            else
            {
                Responce.Status = -6;
            }
            return Responce;


        }
        [WebMethod]
        public static List<ErrorcodeParent> ErrorCodes(string OutboundID)
        {
            OutBoundBL bl = new OutBoundBL();
            return bl.GetErrrodes(Convert.ToInt32(OutboundID));
        }

        [WebMethod]
        public static List<DropDownListDataFilter> getDocks(string outboundid)
        {
            int WareHouseID = DB.GetSqlN("select WarehouseID as N from OBD_RefWarehouse_Details where outboundid=" + outboundid + " and IsDeleted=0 and IsActive=1");
            try
            {
                OutBoundBL objbl = new OutBoundBL();
                return objbl.getDocks(WareHouseID.ToString());
            }
            catch
            {
                return null;
            }

        }

        // ============================= Added By M.D.Prasad ON 22-02-2019 ==========================//
        [WebMethod]
        public static string saveBulkReleaseItemsForOBD(string outboundid, string xmlData,string DockID)
        {
            CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
            string Result = "";
            string OutBoundId = outboundid;
            int wareHouseid = GetWarehouseIDByOutbound(OutBoundId);
            if (!lst.Contains(wareHouseid))
            {
                lst.Add(wareHouseid);
                try
                {
                    string query = "Exec[dbo].[Sp_OBD_UpsertPickingSuggestions] @OutboundID = " + outboundid + ", @UserID = " + cp1.UserID + ",@DataXML='" + xmlData + "'" + ",@DockID=" + DockID;


                    DataSet ds = DB.GetDS(query, false);
                    Result =  JsonConvert.SerializeObject(ds);

                    int AssgnResult = Convert.ToInt32(ds.Tables[0].Rows[0]["Status"].ToString());

                    if (AssgnResult == 1)
                    {
                        string Query_HU = "[dbo].[SP_HU_Upsert_Stock_Reservation] @OutBoundId = "+ outboundid + ", @UserId = "+ cp1.UserID + "";
                        DB.ExecuteSQL(Query_HU);
                    }
                }
                catch (Exception e)
                {
                    Result = e.Message;
                }
                lst.Remove(wareHouseid);
            }
            else
            {
                Result = "This OBD is already in Queue";
            }

            return Result;
        }
        // ============================= Added By M.D.Prasad ON 22-02-2019 ==========================//


        //[WebMethod]
        //public static ReleaseoutboundResult saveBulkReleaseItemsForOBD(List<ReleaseIndividualOBD> Items, int VLPDPicking,string DockId,string vehicleTypeid,string vehiclenumber, string drivername, string mobilenumber, string outboundid)
        //    //,string mobilenumber,string outboundid)
        //{
        //    try
        //    {
        //        List<ReleaseIndividualOBD> glist = new List<ReleaseIndividualOBD>();
        //        OutBoundBL mp = new OutBoundBL();

        //        ReleaseoutboundResult obj = mp.UpsertReleaseItem(Items, VLPDPicking);
        //        int statusid = DB.GetSqlN("select DeliveryStatusID as N from OBD_Outbound where OutboundID="+ outboundid);
        //        if (statusid > 1)
        //        {
        //            DB.ExecuteSQL("update OBD_Outbound set VehicleTypeID = " + vehicleTypeid + ", DockID = " + DockId + ", VehicleNo = " + DB.SQuote(vehiclenumber) + ", DriverName =" + DB.SQuote(drivername) + ", DriverMobileNo = " + DB.SQuote(mobilenumber) + " where OutboundID=" + outboundid);
        //        }
        //        return obj;
        //    }
        //    catch (Exception e)
        //    {

        //        return null;
        //    }



        //}
        [WebMethod]
        public static string RevertLineItems(string IsKitParent,string outboundid, string SODetailsID, string quantity)
        {
            try
            {
                CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
                OutBoundBL OBL = new OutBoundBL();
                return OBL.RevertOBDLineItem(Convert.ToInt32(IsKitParent),outboundid, SODetailsID, quantity, cp.UserID);
            }
            catch(Exception e)
            {
                return "Error :Error while Revert";
            }

        }


        private static int GetWarehouseIDByOutbound(string outboundID)
        {
            try
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(" select REF_WH.WarehouseID AS N from OBD_Outbound OBD ");
                sb.Append(" JOIN OBD_RefWarehouse_Details REF_WH ON REF_WH.OutboundID = OBD.OutboundID ");
                sb.Append(" WHERE OBD.OutboundID ="+ outboundID);
                return DB.GetSqlN(sb.ToString());

            }
            catch(Exception ex)
            {
                return 0;
            }
            

        }
    }
}