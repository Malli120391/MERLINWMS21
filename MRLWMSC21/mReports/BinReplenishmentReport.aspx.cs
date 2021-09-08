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

namespace MRLWMSC21.mReports
{
    public partial class BinReplenishmentReport : System.Web.UI.Page
    {
        string abc = "";
        public static CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        string bcd = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Inventory / Bin Replenishment Report");
            }
        }

        [WebMethod]
        public static List<BinList> GetBinReportList(string ItemNumber, int TenantId, int WareHouseId)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            List<BinList> bdlst = new List<BinList>();
            string Data = "EXEC [dbo].[sp_BinReplenishmentReport] @MaterialMasterID =" + ItemNumber + ",@AccountID_New=" + cp.AccountID + ",@TenantID_New=" + TenantId+ ",@WareHouseId = "+WareHouseId+""; 
            DataSet bds = DB.GetDS(Data, false);
            
            if (bds.Tables[1].Rows.Count != 0)
            {
                foreach (DataRow row in bds.Tables[1].Rows)
                {
                    bdlst.Add(new BinList
                    {
                        MaterialmasterId = Convert.ToInt32(row["MaterialMasterID"]),
                        ItemNo = row["MCode"].ToString(),
                        oBinDetailsListlst = GetBinReportDataList(Convert.ToInt32(row["MaterialMasterID"]), bds.Tables[0])
                    });
                    
                }
            }
            return bdlst;
        }
        public static List<BinDetailsList> GetBinReportDataList(int materialmasterid, DataTable dt)
        {
              List<BinDetailsList> bdlst = new List<BinDetailsList>();
           
                   foreach (DataRow row in dt.Rows)
                   {
                        if (Convert.ToInt32(row["MaterialMasterID"]) == materialmasterid)
                        {
                            bdlst.Add(new BinDetailsList
                            {
                                MinQty = row["MinimumStockLevel"].ToString(),
                                ItemNo = row["MCode"].ToString(),
                                MaxQty = row["MaximumStockLevel"].ToString(),
                                BinRepLoc = row["AvailableBin"].ToString(),
                                BinRepQty = row["AvailableQty"].ToString(),
                                SuggLoc = row["SuggestedRepBin"].ToString(),
                                SuggQty = row["SuggestedRepQty"].ToString()
                                //BinRepQty=
                            });
                        }
                        
                   }
                   return bdlst;
                   
                    
        }

        [WebMethod]
        public static List<BinPopUpList> GetBinPopUpList(int MID)
        {
            List<BinPopUpList> bplst = new List<BinPopUpList>();
            string Data = "EXEC [dbo].[sp_INV_ActiveStock_BinReplinishment] @MaterialMasterID =" + MID;
            DataSet bds = DB.GetDS(Data, false);

            if (bds.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in bds.Tables[0].Rows)
                {
                    bplst.Add(new BinPopUpList
                    {
                        StorageLocation = row["StorageLocation"].ToString(),
                        ItemNo = row["Item"].ToString(),
                        Location = row["Location"].ToString(),
                        Quantity = row["Quantity"].ToString(),
                        UoM = row["UoM/UoMQTY"].ToString()
                    });

                }
            }
            return bplst;
        }
        public class BinList
        {
            public int MaterialmasterId { get; set; }
            public string ItemNo { get; set; }
            public List<BinDetailsList> oBinDetailsListlst { get; set; }
        }

        public class BinPopUpList
        {
            public string StorageLocation { get; set; }
            public string ItemNo { get; set; }
            public string Location { get; set; }
            public string Quantity { get; set; }
            public string UoM { get; set; }
            
        }

        public class BinDetailsList
        {
            public string ItemNo { get; set; }
            public string MinQty { get; set; }
            public string MaxQty { get; set; }
            public string BinRepLoc { get; set; }
            public string BinRepQty { get; set; }
            public string SuggLoc { get; set; }
            public string SuggQty { get; set; }
        }
    }
}