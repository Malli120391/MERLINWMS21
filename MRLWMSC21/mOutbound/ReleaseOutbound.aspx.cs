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
using System.Collections.Generic;
using System.Linq;
using MRLWMSC21.mOutbound.BL;
using Newtonsoft.Json;
using System.IO;
using MRLWMSC21Common;

namespace MRLWMSC21.mOutbound
{
    public partial class ReleaseOutbound : System.Web.UI.Page
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
                DesignLogic.SetInnerPageSubHeading(this.Page, "Release Outbound");
            }
        }
       
        [WebMethod]
        public static List<ReleaseOBD> GetOBDReleaseList(string OBDNumber, string TenantID,string PageIndex,string PageSize)
        {
            try
            {
                OutBoundBL OBL = new OutBoundBL();
                return OBL.GetOBDReleaseList(OBDNumber, TenantID, PageIndex, PageSize);
            }
            catch
            {
                return null;
            }

        }

        [WebMethod]
        public static List<ReleaseIndividualOBD> GetOBDWiseitem(string OBDNumber)
        {
            try
            {
                OutBoundBL OBL = new OutBoundBL();
                return OBL.GetOBDwiseItem(OBDNumber);
            }
            catch
            {
                return null;
            }

        }
        [WebMethod]
        public static string RevertLineItems(string outboundid,string SODetailsID,string quantity)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                OutBoundBL OBL = new OutBoundBL();
                 return OBL.RevertOBDLineItem(0,outboundid, SODetailsID, quantity,cp.UserID);
            }
            catch
            {
                return "";
            }

        }

        //Release OBD ITem
        [WebMethod]
        public static string ReleaseOBDItem(int OBDID)
        //public static string SavePickQty(SavePickNoteInfo1 Pickdata, int Qty, int VLPDID)
        {
            try
            {

                OutBoundBL objbl = new OutBoundBL();
                return objbl.UpsertReleaseItem(OBDID,1);               

            }
            catch
            {
                return "";
            }

        }


        [WebMethod]
        public static ReleaseoutboundResult saveBulkReleaseItems(List<ReleaseIndividualOBD> Items,int VLPDPicking )
        {
            try
            {
                List<ReleaseIndividualOBD> glist = new List<ReleaseIndividualOBD>();
                OutBoundBL mp = new OutBoundBL();
               return mp.UpsertReleaseItem(Items, VLPDPicking);
                
            }
            catch (Exception e)
            {
             
                 return null;
            }



        }

        [WebMethod]
        public static string GetContainers()
        {
            string Json = "";
            string selectCCList;
            try
            {
                selectCCList = "Select * from GEN_Vehicle where isactive=1 and isdeleted=0";               
                DataSet ds = DB.GetDS(selectCCList, false);
                Json = JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                Json = "Failed";
            }
            return Json;
        }


    }
}
