using MRLWMSC21.mOutbound.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21Common;

namespace MRLWMSC21.mOutbound
{
    public partial class GroupOBDList : System.Web.UI.Page
    {
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
                DesignLogic.SetInnerPageSubHeading(this.Page, "Group OBD List");
            }

        }
        //------------------------ added by durga for getting customers on 15/11/2017----------------------//
        [WebMethod]
        public static List<MRLWMSC21.mOutbound.BL.GroupOBD> getGroupOUTBOUNDDataForGroupOBD(int VLPDID,int TenantID,int WHID)
        {
            try
            {
                OutBoundBL objbl = new OutBoundBL();
                return objbl.getGroupOUTBOUNDDataForGroupOBD(VLPDID, TenantID, WHID);
            }
            catch(Exception e)
            {
                return null;
            }

        }
        //Verified List
        [WebMethod]
        public static List<MRLWMSC21.mOutbound.BL.GroupOBDList> getOBDList(int VLPDID)
        {
            try
            {
                OutBoundBL objbl = new OutBoundBL();
                return objbl.getOBDList(VLPDID);
            }
            catch
            {
                return null;
            }

        }
        //Initiate Pickup
        [WebMethod]
        public static string InitiatePickUp(int VLPDID)
        //public static string SavePickQty(SavePickNoteInfo1 Pickdata, int Qty, int VLPDID)
        {
            try
            {

                OutBoundBL objbl = new OutBoundBL();
                return objbl.UpsertPickUpInitiate(VLPDID);

            }
            catch
            {
                return "";
            }

        }
        //Verify
        [WebMethod]
        public static string UpsertVerification(int VLPDID)
        //public static string SavePickQty(SavePickNoteInfo1 Pickdata, int Qty, int VLPDID)
        {
            try
            {

                OutBoundBL objbl = new OutBoundBL();
                return objbl.VerifyVLPD(VLPDID,cp.UserID);

            }
            catch(Exception e)
            {
                return "";
            }

        }

    }
}