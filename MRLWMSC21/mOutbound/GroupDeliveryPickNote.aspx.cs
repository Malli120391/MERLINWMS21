using MRLWMSC21Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21.mOutbound.BL;
using System.Reflection;
using System.Data;

namespace MRLWMSC21.mOutbound
{
    public partial class GroupDeliveryPickNote : System.Web.UI.Page
    {
        // public static CustomPrincipal cp;
        public static CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            Page.Theme = "Outbound";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Delivery Pick Note");
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<MRLWMSC21.mOutbound.BL.DeliveryPickNote> GET_DeliveryData(int VLPDID, int TransferRequestID)
        {
            try
            {
                List<MRLWMSC21.mOutbound.BL.DeliveryPickNote> ltsku = new List<MRLWMSC21.mOutbound.BL.DeliveryPickNote>();
                OutBoundBL data = new OutBoundBL();
                ltsku = data.GetVLPDItems(VLPDID, TransferRequestID);
                return ltsku;
            }
            catch (Exception ex)
            {
                return null;
            }


        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static int DeletePickedItems(int PickedId)
        {
            string Query = "EXEC [dbo].[DELETE_PICKEDITEMS_FOR_VLPD_OUTBOUND] @VLPDPickedID = "+ PickedId + ", @CreatedBy = "+cp.UserID+"";
            int restult = DB.GetSqlN(Query);
            return restult;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<MRLWMSC21.mOutbound.BL.DeliveryPickNote> GetPickedItems (int MMID, int LoctionID, int CartonID, int Vlpdid , string BatchNo, string serialNo , string ProjectRefNo, string ExpDate, string MfgDate, int VLPDAssingID,string MRP)
        {
            List<MRLWMSC21.mOutbound.BL.DeliveryPickNote> lst = new List<BL.DeliveryPickNote>();
          string Query = "EXEC [dbo].[GEN_VLPD_ITEMSWISEPICKEDLIST] @MaterialMasterID = "+MMID+ ", @CartonID = "+CartonID+ ", @VLPDID = "+Vlpdid+ ", @LocationID = "+LoctionID+ ",   "+
                            " @BatchNo = "+DB.SQuote(BatchNo)+ ", @SerialNo = "+DB.SQuote(serialNo)+ ", @ProjectRefNo = "+ DB.SQuote(ProjectRefNo) + ", @ExpDate = "+ DB.SQuote(ExpDate) + ", @MfgDate = "+DB.SQuote(MfgDate)+ " , @MRP=" +DB.SQuote(MRP) + ", @VLPDAssignId=" + VLPDAssingID + "";

            DataSet DS = DB.GetDS(Query, false);
            if(DS.Tables[0].Rows.Count != 0)
            {
                foreach(DataRow row in DS.Tables[0].Rows)
                {
                    MRLWMSC21.mOutbound.BL.DeliveryPickNote DPN = new BL.DeliveryPickNote();
                    DPN.MCode = row["MCode"].ToString();
                    DPN.Location = row["Location"].ToString();
                    DPN.CartonCode = row["CartonCode"].ToString();
                    DPN.PickedQty = Convert.ToDecimal(row["PickedQty"].ToString());
                    DPN.AssignID = Convert.ToInt32(row["PickedID"].ToString());
                    lst.Add(DPN);
                }
            }
            return lst;
        }

       [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<MRLWMSC21.mOutbound.BL.PickNoteInfo> GET_MaterialData(int VLPDID, int MaterialID)
        {
            try
            {

                OutBoundBL data = new OutBoundBL();
                return data.GetDeliveryPickItems(VLPDID, MaterialID);

            }
            catch (Exception ex)
            {
                return null;
            }


        }



        [WebMethod]
        public static string SavePickQty(BL.DeliveryPickNote Pickdata, int Qty, int VLPDID, int TransferRequestID,string containerid)
        //public static string SavePickQty(SavePickNoteInfo1 Pickdata, int Qty, int VLPDID)
        {
            try
            {

                OutBoundBL objbl = new OutBoundBL();


                return objbl.UpsertPickItem(Pickdata, cp.UserID, Qty, VLPDID, containerid,TransferRequestID);
                //   return "";

            }
            catch
            {
                return "";
            }

        }


    }

    
    public class SavePickNoteInfo1
    {
        private int id { get; set; }

        //public int Id { get => id; set => id = value; }
    }
}