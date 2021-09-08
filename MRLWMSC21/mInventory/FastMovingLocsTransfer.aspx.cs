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
    public partial class FastMovingLocsTransfer : System.Web.UI.Page
    {
        public static CustomPrincipal cp;
        protected void page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Orders";
            cp = HttpContext.Current.User as CustomPrincipal;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //  DesignLogic.SetInnerPageSubHeading(this.Page, "Material Transfer");
                LoadDropdowns();
            }
        }

        private void LoadDropdowns()
        {
            DataSet ds = DB.GetDS("SELECT TransferTypeID, TransferType FROM INV_TransferType WHERE IsActive=1 AND IsDeleted=0 AND TransferTypeID IN (5); SELECT FulfillStatusID, FulfillStatus FROM INV_FastMovementLocsFulfillOrderStatus WHERE IsActive=1 AND IsDeleted=0", true);
            ddlTransferType.DataSource = ds.Tables[0];
            ddlTransferType.DataValueField = "TransferTypeID";
            ddlTransferType.DataTextField = "TransferType";

            ddlTransferType.DataBind();
            ddlTransferType.Items.Insert(0, new ListItem("--Please Select--", "0"));
            ddlStatus.DataSource = ds.Tables[1];
            ddlStatus.DataValueField = "FulfillStatusID";
            ddlStatus.DataTextField = "FulfillStatus";
            ddlStatus.DataBind();
            ddlStatus.Items.Insert(0, new ListItem("--Please Select--", "0"));
        }

        [WebMethod]
        public static List<FastMovingLocsTransferOrder> GetList(int TransferTypeId, int StatusId, int WarehouseID)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            DataSet ds = DB.GetDS("EXEC [dbo].[sp_INV_GetFastMovementLocsFulfillOrders] @TransferTypeID=" + TransferTypeId + ", @AccountID=" + cp.AccountID + ",@FulfillStatusID =" + StatusId + " , @UserID = " + cp.UserID + " , @WareHouseID=" + WarehouseID, false);
            List<FastMovingLocsTransferOrder> List = new List<FastMovingLocsTransferOrder>();
            List = FillObject(ds.Tables[0]);
            return List;

        }

        [WebMethod]
        public static int InitiateToInProcess(int TransferRequestID)
        {
            StringBuilder sb = new StringBuilder();
            cp = HttpContext.Current.User as CustomPrincipal;
            sb.Append(" DECLARE @Count INT = (SELECT COUNT(*) FROM INV_TransferRequestDetails WHERE TransferRequestID=" + TransferRequestID + " AND IsActive=1 AND IsDeleted=0);  ");
            sb.Append(" UPDATE INV_TransferRequest SET StatusID=(CASE WHEN @Count=0 THEN 1 ELSE 2 END)  WHERE TransferRequestID=" + TransferRequestID + "; ");
            sb.Append(" SELECT @Count N ");

            int result = DB.GetSqlN("EXEC [dbo].[INTERNALSTOCK_RESERVE] @TransferRequestedID = " + TransferRequestID + " , @CreatedBy = " + cp.UserID + "");
            //  int res = DB.GetSqlN(sb.ToString());
            return result;
        }

        [WebMethod]
        public static int InprocessToCloss(int TransferRequestID)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            string Query = "EXEC [dbo].[INTERNALSTOCK_VERIFICATION] @TransferRequestID = " + TransferRequestID + "";
            int result = DB.GetSqlN(Query);
            //  int res = DB.GetSqlN(sb.ToString());
            return result;
        }

        [WebMethod]
        public static int DeleteOrder(int TransferRequestID)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            try
            {
                DB.ExecuteSQL(" Update INV_TransferRequest SET IsActive=0, UpdatedBy=1, UpdatedOn=GETUTCDATE() WHERE TransferRequestID=" + TransferRequestID + "; UPDATE INV_TransferRequestDetails SET IsActive=0, UpdatedBy=1, UpdatedOn=GETUTCDATE() WHERE TransferRequestID=" + TransferRequestID + ";  ");
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        [WebMethod]
        public static int DeleteItemsById(string SP_Del, string JSON)
        {
            //MasterModel objMasterModel = null;
            //objMasterModel = new MasterModel();
            //return objMasterModel.DeleteItemsById(SP_Del, JSON);
            return 0;

        }

        public static List<FastMovingLocsTransferOrder> FillObject(DataTable dt)
        {
            List<FastMovingLocsTransferOrder> List = new List<FastMovingLocsTransferOrder>();

            HashSet<int> hs = new HashSet<int>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int HeaderID = Convert.ToInt32(dt.Rows[i]["TransferRequestID"]);
                if (!hs.Contains(HeaderID))
                {
                    hs.Add(HeaderID);
                    FastMovingLocsTransferOrder oHeader = new FastMovingLocsTransferOrder();
                    oHeader.TransferRequestID = HeaderID;
                    oHeader.TransferRequestNumber = dt.Rows[i]["TransferRequestNumber"].ToString();
                    oHeader.TenantName = dt.Rows[i]["TenantName"].ToString();
                    oHeader.TransferType = dt.Rows[i]["TransferType"].ToString();
                    oHeader.CreatedBy = dt.Rows[i]["CreatedBy"].ToString();
                    oHeader.CreatedOn = Convert.ToDateTime(dt.Rows[i]["CreatedOn"].ToString()).ToString("dd-MMM-yyyy");
                    oHeader.FulfillStatus = dt.Rows[i]["FulfillStatus"].ToString();
                    // oHeader.Details = GetDetailsList(dt, HeaderID);
                    oHeader.FulfillStatusID = dt.Rows[i]["StatusID"].ToString();
                    oHeader.WareHouse = dt.Rows[i]["WareHouse"].ToString();
                    List.Add(oHeader);
                }
                //else
                //{

                //}
            }

            return List;
        }

        public static List<FastMovingLocsTransferOrderDetails> GetDetailsList(DataTable dt, int HeaderID)
        {
            List<FastMovingLocsTransferOrderDetails> dList = new List<FastMovingLocsTransferOrderDetails>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (Convert.ToInt32(dt.Rows[i]["TransferRequestID"]) == HeaderID)
                {
                    FastMovingLocsTransferOrderDetails oDetails = new FastMovingLocsTransferOrderDetails();
                    oDetails.TransferRequestDetailsID = Convert.ToInt32(dt.Rows[i]["TransferRequestDetailsID"]);
                    oDetails.MCode = dt.Rows[i]["MCode"].ToString();
                    oDetails.Quantity = Convert.ToDecimal(dt.Rows[i]["Quantity"]);
                    oDetails.ToLocation = dt.Rows[i]["ToLocation"].ToString();
                    oDetails.FromLocation = dt.Rows[i]["FromLocation"].ToString();
                    oDetails.BatchNo = dt.Rows[i]["BatchNo"].ToString();
                    oDetails.DestBatchNo = dt.Rows[i]["DestBatchNo"].ToString();
                    dList.Add(oDetails);
                }
            }

            return dList;
        }
    }


    public class FastMovingLocsTransferOrder
    {
        public int TransferRequestID { get; set; }
        public string TransferRequestNumber { get; set; }
        public String TenantName { get; set; }
        public String TransferType { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string FulfillStatusID { get; set; }
        public string FulfillStatus { get; set; }

        public List<FastMovingLocsTransferOrderDetails> Details { get; set; }

        public string WareHouse { get; set; }

    }

    public class FastMovingLocsTransferOrderDetails
    {
        public int TransferRequestDetailsID { get; set; }
        public string MCode { get; set; }
        public Decimal Quantity { get; set; }
        public string BatchNo { get; set; }
        public string DestBatchNo { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
    }
}