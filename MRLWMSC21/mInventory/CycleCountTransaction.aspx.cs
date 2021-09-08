using MRLWMSC21Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRLWMSC21.mInventory
{
    public partial class CycleCountTransaction : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        protected void page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Inventory";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // DesignLogic.SetInnerPageSubHeading(this.Page, "Cycle Count Transaction");
            }
        }


        //========================= Setter Methods =======================//


        //========================= MasterDetailsSet ===========================//
        [WebMethod]
        public static string MasterDetailsSet(string Sp_Set, string JSON)
        {
            Sp_Set = "[dbo].[USP_SET_CCM_TRN_CycleCounts]";
            Dictionary<dynamic, dynamic> values = JsonConvert.DeserializeObject<Dictionary<dynamic, dynamic>>(JsonConvert.DeserializeObject(JSON).ToString());

            StringBuilder sb = new StringBuilder();
            int count = 0;
            foreach (KeyValuePair<dynamic, dynamic> pair in values)
            {
                if (count != 0)
                    sb.Append(",");
                sb.Append(pair.Key + "=" + DB.SQuote(pair.Value));
                count++;
            }
            // sb.ToString();
            DB.ExecuteSQL("EXEC " + Sp_Set + " " + sb.ToString());

            return "";
        }
        //========================= MasterDetailsSet ===========================//

        //========================= MasterDetailsSet_NEW ===========================//
        [WebMethod]
        public static string MasterDetailsSet_NEW(string Sp_Set, string JSON)
        {
            Sp_Set = "[dbo].[USP_SET_CCM_TRN_CycleCountInventory_NEW]";
            Dictionary<dynamic, dynamic> values = JsonConvert.DeserializeObject<Dictionary<dynamic, dynamic>>(JsonConvert.DeserializeObject(JSON).ToString());

            StringBuilder sb = new StringBuilder();
            int count = 0;
            foreach (KeyValuePair<dynamic, dynamic> pair in values)
            {
                if (count != 0)
                    sb.Append(",");
                sb.Append(pair.Key + "=" + DB.SQuote(pair.Value));
                count++;
            }
            // sb.ToString();
            DB.ExecuteSQL("EXEC " + Sp_Set + " " + sb.ToString());

            return "";
        }
        //========================= MasterDetailsSet_NEW ===========================//

        //========================= MasterDetailsSetIsBlock ===========================//
        [WebMethod]
        public static string MasterDetailsSetIsBlock(string Sp_Set, string JSON)
        {
            Sp_Set = "[dbo].[USP_UPSERT_StorageLocation_IsBlock]";
            Dictionary<dynamic, dynamic> values = JsonConvert.DeserializeObject<Dictionary<dynamic, dynamic>>(JsonConvert.DeserializeObject(JSON).ToString());

            StringBuilder sb = new StringBuilder();
            int count = 0;
            foreach (KeyValuePair<dynamic, dynamic> pair in values)
            {
                if (count != 0)
                    sb.Append(",");
                sb.Append(pair.Key + "=" + DB.SQuote(pair.Value));
                count++;
            }
            // sb.ToString();
            DB.ExecuteSQL("EXEC " + Sp_Set + " " + sb.ToString());

            return "";
        }
        //========================= MasterDetailsSetIsBlock ===========================//


        //========================= MasterDetailsSetLF ===========================//
        [WebMethod]
        public static string MasterDetailsSetLF(string Sp_Set, string JSON)
        {
            Sp_Set = "[dbo].[USP_SET_CCM_TRN_CycleCounts_LostandFoundQty]";
            Dictionary<dynamic, dynamic> values = JsonConvert.DeserializeObject<Dictionary<dynamic, dynamic>>(JsonConvert.DeserializeObject(JSON).ToString());

            StringBuilder sb = new StringBuilder();
            int count = 0;
            foreach (KeyValuePair<dynamic, dynamic> pair in values)
            {
                if (count != 0)
                    sb.Append(",");
                sb.Append(pair.Key + "=" + DB.SQuote(pair.Value));
                count++;
            }
            // sb.ToString();
            DB.ExecuteSQL("EXEC " + Sp_Set + " " + sb.ToString());

            return "";
        }
        //========================= MasterDetailsSetLF ===========================//

        //========================= MasterDetailsSetLFO ===========================//
        [WebMethod]
        public static string MasterDetailsSetLFO(string Sp_Set, string JSON)
        {
            Sp_Set = "[dbo].[USP_CCM_LostAndFoundOperations]";
            Dictionary<dynamic, dynamic> values = JsonConvert.DeserializeObject<Dictionary<dynamic, dynamic>>(JsonConvert.DeserializeObject(JSON).ToString());

            StringBuilder sb = new StringBuilder();
            int count = 0;
            foreach (KeyValuePair<dynamic, dynamic> pair in values)
            {
                if (count != 0)
                    sb.Append(",");
                sb.Append(pair.Key + "=" + DB.SQuote(pair.Value));
                count++;
            }
            // sb.ToString();
            DB.ExecuteSQL("EXEC " + Sp_Set + " " + sb.ToString());

            return "";
        }
        //========================= MasterDetailsSetLFO ===========================//

        //========================= MasterDetailsSetCompletion ===========================//
        [WebMethod]
        public static string MasterDetailsSetCompletion(string Sp_Set, string JSON)
        {
            Sp_Set = "[dbo].[USP_Update_CCM_TRN_CycleCounts_CompletionTime]";
            Dictionary<dynamic, dynamic> values = JsonConvert.DeserializeObject<Dictionary<dynamic, dynamic>>(JsonConvert.DeserializeObject(JSON).ToString());

            StringBuilder sb = new StringBuilder();
            int count = 0;
            foreach (KeyValuePair<dynamic, dynamic> pair in values)
            {
                if (count != 0)
                    sb.Append(",");
                sb.Append(pair.Key + "=" + DB.SQuote(pair.Value));
                count++;
            }
            // sb.ToString();
            DB.ExecuteSQL("EXEC " + Sp_Set + " " + sb.ToString());

            return "";
        }
        //========================= MasterDetailsSetCompletion ===========================//


        //========================= MasterDetailsSetNewCount ===========================//
        [WebMethod]
        public static string MasterDetailsSetNewCount(string Sp_Set, string JSON)
        {
            Sp_Set = "[dbo].[USP_SET_CCM_TRN_CycleCounts_NewCount]";
            Dictionary<dynamic, dynamic> values = JsonConvert.DeserializeObject<Dictionary<dynamic, dynamic>>(JsonConvert.DeserializeObject(JSON).ToString());

            StringBuilder sb = new StringBuilder();
            int count = 0;
            foreach (KeyValuePair<dynamic, dynamic> pair in values)
            {
                if (count != 0)
                    sb.Append(",");
                sb.Append(pair.Key + "=" + DB.SQuote(pair.Value));
                count++;
            }
            // sb.ToString();
            DB.ExecuteSQL("EXEC " + Sp_Set + " " + sb.ToString());

            return "";
        }
        //========================= MasterDetailsSetNewCount ===========================//

        //========================= Setter Methods =======================//

        //========================= Getter Methods =======================//

        [WebMethod]
        public static string GetCycleCountTransactionList(int AccountID)
        {
            CustomPrincipal cp2 = HttpContext.Current.User as CustomPrincipal;
            DataSet ds = DB.GetDS("EXEC [dbo].[USP_GET_CCM_TRN_CycleCounts] @AccountID=" + AccountID+",@UserId="+cp2.UserID, false);
            return JsonConvert.SerializeObject(ds);
        }

        [WebMethod]
        public static string GetCycleCountTransactionListByStatus(int AccountID)
        {
            CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
            DataSet ds = DB.GetDS("EXEC [dbo].[USP_GET_CCM_TRN_CycleCounts_Ascending] @AccountID=" + AccountID+", @UserId="+ cp1.UserID, false);
            return JsonConvert.SerializeObject(ds);
        }


        //========================= GetSuccessInfoLocationFilter ===========================//
        [WebMethod]
        public static string GetSuccessInfoLocationFilter(string SP_Name, string JSON)
        {
            SP_Name = "[dbo].[USP_GET_CCM_TRN_LocationFilterQueryWithActiveStock]";
            Dictionary<dynamic, dynamic> values = JsonConvert.DeserializeObject<Dictionary<dynamic, dynamic>>((JsonConvert.DeserializeObject(JSON)).ToString());


            StringBuilder sb = new StringBuilder();
            int count = 0;
            foreach (KeyValuePair<dynamic, dynamic> pair in values)
            {
                if (count != 0)
                    sb.Append(",");
                sb.Append(pair.Key + "=" + DB.SQuote(pair.Value));
                count++;
            }
            //sb.ToString();
            DataSet dsInfo = DB.GetDS("EXEC " + SP_Name + " " + sb.ToString(), false);
            return Newtonsoft.Json.JsonConvert.SerializeObject(dsInfo);
        }
        //========================= GetSuccessInfoLocationFilter ===========================//


        //========================= GetSuccessInfoCapture ===========================//
        [WebMethod]
        public static string GetSuccessInfoCapture(string SP_Name, string JSON)
        {
            SP_Name = "[dbo].[USP_GET_CCM_TRN_CycleCounts_Capture]";
            Dictionary<dynamic, dynamic> values = JsonConvert.DeserializeObject<Dictionary<dynamic, dynamic>>((JsonConvert.DeserializeObject(JSON)).ToString());


            StringBuilder sb = new StringBuilder();
            int count = 0;
            foreach (KeyValuePair<dynamic, dynamic> pair in values)
            {
                if (count != 0)
                    sb.Append(",");
                sb.Append(pair.Key + "=" + DB.SQuote(pair.Value));
                count++;
            }
            //sb.ToString();
            DataSet dsInfo = DB.GetDS("EXEC " + SP_Name + " " + sb.ToString(), false);
            return Newtonsoft.Json.JsonConvert.SerializeObject(dsInfo);
        }
        //========================= GetSuccessInfoCapture ===========================//

        //========================= GetSuccessInfoFilter ===========================//
        [WebMethod]
        public static string GetSuccessInfoFilter(string SP_Name, string JSON)
        {
            SP_Name = "[dbo].[USP_GET_CCM_TRN_LocationFilterQuery]";
            Dictionary<dynamic, dynamic> values = JsonConvert.DeserializeObject<Dictionary<dynamic, dynamic>>((JsonConvert.DeserializeObject(JSON)).ToString());


            StringBuilder sb = new StringBuilder();
            int count = 0;
            foreach (KeyValuePair<dynamic, dynamic> pair in values)
            {
                if (count != 0)
                    sb.Append(",");
                sb.Append(pair.Key + "=" + DB.SQuote(pair.Value));
                count++;
            }
            //sb.ToString();
            DataSet dsInfo = DB.GetDS("EXEC " + SP_Name + " " + sb.ToString(), false);
            return Newtonsoft.Json.JsonConvert.SerializeObject(dsInfo);
        }
        //========================= GetSuccessInfoFilter ===========================//

        //========================= GetSuccessInfoContainers ===========================//
        [WebMethod]
        public static string GetSuccessInfoContainers(string SP_Name, string JSON)
        {
            SP_Name = "[dbo].[USP_GetContainers_Capture_CycleCount]";
            Dictionary<dynamic, dynamic> values = JsonConvert.DeserializeObject<Dictionary<dynamic, dynamic>>((JsonConvert.DeserializeObject(JSON)).ToString());


            StringBuilder sb = new StringBuilder();
            int count = 0;
            foreach (KeyValuePair<dynamic, dynamic> pair in values)
            {
                if (count != 0)
                    sb.Append(",");
                sb.Append(pair.Key + "=" + DB.SQuote(pair.Value));
                count++;
            }
            //sb.ToString();
            DataSet dsInfo = DB.GetDS("EXEC " + SP_Name + " " + sb.ToString(), false);
            return Newtonsoft.Json.JsonConvert.SerializeObject(dsInfo);
        }
        //========================= GetSuccessInfoContainers ===========================//

        //========================= GetSuccessInfoLocations_Capture ===========================//
        [WebMethod]
        public static string GetSuccessInfoLocations_Capture(string SP_Name, string JSON)
        {
            SP_Name = "[dbo].[USP_GetLocations_Capture_CycleCount]";
            Dictionary<dynamic, dynamic> values = JsonConvert.DeserializeObject<Dictionary<dynamic, dynamic>>((JsonConvert.DeserializeObject(JSON)).ToString());


            StringBuilder sb = new StringBuilder();
            int count = 0;
            foreach (KeyValuePair<dynamic, dynamic> pair in values)
            {
                if (count != 0)
                    sb.Append(",");
                sb.Append(pair.Key + "=" + DB.SQuote(pair.Value));
                count++;
            }
            //sb.ToString();
            DataSet dsInfo = DB.GetDS("EXEC " + SP_Name + " " + sb.ToString(), false);
            return Newtonsoft.Json.JsonConvert.SerializeObject(dsInfo);
        }
        //========================= GetSuccessInfoLocations_Capture ===========================//

        //========================= GetSuccessInfoCounts ===========================//
        [WebMethod]
        public static string GetSuccessInfoCounts(string SP_Name, string JSON)
        {
            SP_Name = "[dbo].[USP_GET_CCM_TRN_CycleCounts]";
            Dictionary<dynamic, dynamic> values = JsonConvert.DeserializeObject<Dictionary<dynamic, dynamic>>((JsonConvert.DeserializeObject(JSON)).ToString());


            StringBuilder sb = new StringBuilder();
            int count = 0;
            foreach (KeyValuePair<dynamic, dynamic> pair in values)
            {
                if (count != 0)
                    sb.Append(",");
                sb.Append(pair.Key + "=" + DB.SQuote(pair.Value));
                count++;
            }
            //sb.ToString();
            DataSet dsInfo = DB.GetDS("EXEC " + SP_Name + " " + sb.ToString(), false);
            return Newtonsoft.Json.JsonConvert.SerializeObject(dsInfo);
        }
        //========================= GetSuccessInfoCounts ===========================//

        //========================= Getter Methods =======================//
    }
}