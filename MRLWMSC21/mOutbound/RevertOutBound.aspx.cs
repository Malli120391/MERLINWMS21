using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21Common;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.Services;
using Newtonsoft.Json;

// Module Name : RevertOutbound Under Outbound
// Usecase Ref.: Revert Outbound_UC_017
// DevelopedBy : Naresh P
// CreatedOn   : 24/11/2013
// Modified On : 25/11/2015

namespace MRLWMSC21.mOutbound
{
    public partial class RevertOutbound : System.Web.UI.Page
    {

        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        public static CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
        protected string SQLSearch = "EXEC [sp_OBD_RevertOutbound]   ";
        string TenantRootDir = "";
        string OutboundPath = "";
        string OBD_DeliveryNotePath = "";
        string OBD_PickandCheckSheetPath = "";
        string OBD_PODPath = "";

        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Outbound";
        }


        protected void Page_Load(object sender, EventArgs e)
        {

            DesignLogic.SetInnerPageSubHeading(this.Page, "Revert Outbound");


            if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("Revert Outbound"))))
            {
                Response.Redirect("../Login.aspx?eid=6");
            }

            string query = "EXEC [dbo].[sp_TPL_GetTenantDirectoryInfo] @TypeID=2";

            DataSet dsPath = DB.GetDS(query.ToString(), false);

            TenantRootDir = dsPath.Tables[0].Rows[4][0].ToString();
            OutboundPath = dsPath.Tables[0].Rows[0][0].ToString();
            OBD_DeliveryNotePath = dsPath.Tables[0].Rows[1][0].ToString();
            OBD_PickandCheckSheetPath = dsPath.Tables[0].Rows[2][0].ToString();
            OBD_PODPath = dsPath.Tables[0].Rows[3][0].ToString();


            if (!IsPostBack)
            {
                this.SQLSearch += "@OutboundID=NULL,@Tenant=''";
               // this.SQLSearch =
                ViewState["DeliveryResultsSQLString"] = this.SQLSearch + ", @AccountID_New = " + cp.AccountID.ToString() + ",@UserTypeID_New = " + cp.UserTypeID.ToString() + ",@TenantID_New = " + cp.TenantID.ToString();


                this.DeliveryResults_buildGridData(this.DeliveryResults_buildGridData());

            }


        }

        protected void lnkSearchtext_Click(object sender, EventArgs e)
        {
            string OBDNumber = txtsearchText.Text.Trim();
            if (OBDNumber == "Delv. Doc.#...")
                OBDNumber = "";

            string Tenant = txtTenant.Text.Trim();
            if (Tenant == "Tenant..." || hifTenant.Value.Trim() == "0")
                Tenant = "";

            this.SQLSearch += " @OutboundID=" + CommonLogic.GetOutboundID(OBDNumber) + ",@Tenant=" + DB.SQuote(Tenant);

            //if (txtsearchText.Text.Trim() != "")
            //    this.SQLSearch += " @OutboundID=" + CommonLogic.GetOutboundID(txtsearchText.Text.Trim()) + ",@TenantID="+hifTenant.Value;
            //else
            //    this.SQLSearch += " @OutboundID=NULL,@TenantID=''";


            ViewState["DeliveryResultsSQLString"] = this.SQLSearch;

            this.DeliveryResults_buildGridData(this.DeliveryResults_buildGridData());
        }

        public string GetRevertPNC(String Sent2PGIDate)
        {
            if (Sent2PGIDate != "")
            {
                return "<br/><nobr><img src='../Images/redarrowleft.gif' border='0' /> Revert PNC Verif.. </nobr>";
            }
            else
                return "";
        }

        public string GetRevertDelivery(String DocumentTypeID, String Sent2PGIDate)
        {


            if (Sent2PGIDate != "")
            {
                return "<nobr><img src='../Images/redarrowleft.gif' border='0' /> Revert Delivery.<a class=\"helpWTitle\" title=\"Revert Delivery| Reverts the delivery to the previous stage (PGI Pending).However line items picked for this delivery will not be released back to their locations. This features is only a convinence provided to reverse the workflow of an outbound delivery\"> <img src=\"../Images/help_icon.gif\" border=\"0\"  class=\"hidImage\"/></a></nobr>";


            }
            else
                return "";
        }

        public string GetRevertandReleaseDelivery(String DocumentTypeID, String Sent2PGIDate)
        {
            if (Sent2PGIDate != "")
            {

                return "<nobr><img src='../Images/redarrowleft.gif' border='0' /> Revert and Release Items.<a class=\"helpWTitle\" title=\"Revert and Release Items| Reverts the delivery to the previous stage (PGI Pending) and releases its  picked line items back to their picked location. This is an irreversable process, hence a carefull use of thie feature is recommended.\"> <img src=\"../Images/help_icon.gif\" border=\"0\"  class=\"hidImage\"/></a></nobr>";
            }
            else
                return "";
        }

        public string GetRevertPGI(String PGIDone_DT)
        {
            if (PGIDone_DT != "")
            {
                return "<br/><nobr><img src='../Images/redarrowleft.gif' border='0' /> Revert PGI. </nobr>";
            }
            else
                return "";
        }

        public string GetRevertCloseCancel(String DeliveryStatusID)
        {
            if (DeliveryStatusID != "")
            {
                if (DeliveryStatusID == "3")
                    return "<br/><nobr><img src='../Images/redarrowleft.gif' border='0' /> Revert Cancelled. </nobr>";
                else if (DeliveryStatusID == "10")
                    return "<br/><nobr><img src='../Images/redarrowleft.gif' border='0' /> Revert Closed. </nobr>";
                else return "";
            }
            else
                return "";
        }


        public string GetStoreRefNoWithLink(String OBDNumber, String TenantID)
        {
            String sFileName = CommonLogic._GetAttatchmentFile(TenantRootDir + TenantID + OutboundPath, OBDNumber);

            String ResValue = "";
            if (sFileName != "")
            {
                String Path = "../ViewImage.aspx?path=" + sFileName;

                ResValue += "<a style=\"text-decoration:none;\" href=\"#\" onclick=\" OpenImage(' " + Path + " ')  \" > " + OBDNumber + " </a>";
                // ResValue += "<img src=\"../Images/redarrowright.gif\"   />";
            }
            else
            {
                ResValue = OBDNumber;
            }


            return ResValue;
        }



                                         #region --------- gvDeliveryResults --------------

        protected DataSet DeliveryResults_buildGridData()
        {
            string sql = ViewState["DeliveryResultsSQLString"].ToString();
           
            DataSet ds = DB.GetDS(sql, false);
           // ltSRRecordCount.Text = "[" + ds.Tables[0].Rows.Count.ToString() + "]";
            return ds;

        }

        protected void DeliveryResults_buildGridData(DataSet ds)
        {
            gvDeliveryResults.DataSource = ds;
            gvDeliveryResults.DataBind();
            ds.Dispose();
        }




        protected void gvDeliveryResults_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.DeliveryResults_buildGridData(this.DeliveryResults_buildGridData());
        }



        protected void gvDeliveryResults_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
           
            gvDeliveryResults.PageIndex = e.NewPageIndex;
            gvDeliveryResults.EditIndex = -1;
            this.DeliveryResults_buildGridData(this.DeliveryResults_buildGridData());
        }

        protected void resetError(string error, bool isError)
        {
            /*
            string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
            if (isError)
                str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

            if (error.Length > 0)
                str += error + "";
            else
                str = "";
            lblStatusMessage.Text = str;*/

            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);


        }


        protected void gvDeliveryResults_RowCommand(object sender, GridViewCommandEventArgs e)
        {
           

            StringBuilder sqlRevertTrans = new StringBuilder();

            ViewState["GDRIsInsert"] = false;
            gvDeliveryResults.EditIndex = -1;
            String vOBDNumber = "";
            String vOBDTrackingID = "";
            String vRefWHID = "0";
            int OBD_TWHID = 0;
            int WH_ID = 0;

            if (e.CommandName != "Page")
            {
                char[] seps = new char[] { '&' };
                String[] CmdArguments = e.CommandArgument.ToString().Split(seps);

                vOBDNumber = CmdArguments[0];
                vOBDTrackingID = CmdArguments[1];

                if (CmdArguments[2] != "")
                    vRefWHID = CmdArguments[2];


                OBD_TWHID = DB.GetSqlN("select OutboundTracking_WarehouseID AS N from OBD_OutboundTracking_Warehouse  where OutboundID=" + vOBDTrackingID + " and OB_RefWarehouse_DetailsID=" + vRefWHID);

                WH_ID = DB.GetSqlN("select  WarehouseID AS N  from OBD_RefWarehouse_Details where IsActive=1 and IsDeleted=0 and OB_RefWarehouse_DetailsID=" + vRefWHID);
            }

           

            switch (e.CommandName)
            {

                     

                case "RevertPNC1":

                    if (DB.GetSqlN("Select count(OutboundID) as N from OBD_Outbound Where PGIDoneOn Is NOT NULL AND OutboundID=" + vOBDTrackingID) > 0)
                    {
                        resetError("Sorry, you cannot revert this to Pink N Check untill you Revert the PGI", true);
                        return;
                    }

                    sqlRevertTrans.Append("BEGIN TRANSACTION ");

                    sqlRevertTrans.Append("; delete from INV_PO_GoodsOutLink where SOGoodsMovementDetailsID IN (Select GoodsMovementDetailsID from INV_GoodsMovementDetails Where GoodsMovementTypeID =2 AND  TransactionDocID=" + vOBDTrackingID + ")");
                    sqlRevertTrans.Append("; delete from INV_GoodsMovementDetails_MMT_MaterialStorageParameter where GoodsMovementDetailsID IN (Select GoodsMovementDetailsID from INV_GoodsMovementDetails Where GoodsMovementTypeID =2 AND  TransactionDocID=" + vOBDTrackingID + ")");
                    
                    sqlRevertTrans.Append("; delete from  INV_GoodsMovementDetails  WHERE  GoodsMovementDetailsID IN (Select GoodsMovementDetailsID from INV_GoodsMovementDetails Where GoodsMovementTypeID =2 AND  TransactionDocID=" + vOBDTrackingID +")");


                    sqlRevertTrans.Append("; UPDATE OBD_Outbound SET DeliveryStatusID=1 WHERE OutboundID=" + vOBDTrackingID);
                    sqlRevertTrans.Append("; delete from OBD_RequiredEquipment where OutboundTracking_WarehouseID=" + OBD_TWHID);
                    sqlRevertTrans.Append("; delete from OBD_UsedEquipment where OutboundTracking_WarehouseID=" + OBD_TWHID);
                    sqlRevertTrans.Append(" ;DELETE OBD_OutboundTracking_Warehouse WHERE OutboundID=" + vOBDTrackingID + " AND OB_RefWarehouse_DetailsID=" + vRefWHID);
                    //InOut =2, OBD_StoreRef_ID =vOBDTrackingID, RevertStatusID=6 (for Revert PNC),
                    //sqlRevertTrans.Append(" ;INSERT INTO RevertLog(InOut,OBD_StoreRef_ID,RevertStatusID,StoreID,RevertedBy) VALUES(2," + vOBDTrackingID + ",6," + vRefWHID + "," + cp.UserID.ToString() + ")");
                    sqlRevertTrans.Append("; COMMIT TRANSACTION ");

                    try
                    {
                        DB.ExecuteSQL(sqlRevertTrans.ToString());

                        CommonLogic._DeleteAttatchment(TenantRootDir + DB.GetSqlN("select TenantID N from OBD_Outbound where IsActive=1 AND IsDeleted=0 AND OutboundID=" + vOBDTrackingID) + OutboundPath + OBD_PickandCheckSheetPath, DB.GetSqlS("select OBDNumber AS S from  OBD_Outbound where IsActive=1 AND IsDeleted=0 AND OutboundID=" + vOBDTrackingID) + "_" + DB.GetSqlN("select WarehouseID AS N from OBD_RefWarehouse_Details where IsActive=1 AND IsDeleted=0  AND OB_RefWarehouse_DetailsID=" + vRefWHID) + ".pdf");

                        resetError("PNC details successfully reverted ", false);
                    }
                    catch (Exception ex)
                    {
                        CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                        resetError("Error while reverting  PNC details", true);

                    }

                    String varFileName = vRefWHID + "_" + vOBDNumber;
                    //CommonLogic.DeleteAttachmentFile("PNC", vOBDNumber);
                    break;



                case "RevertPGI":

                    if (DB.GetSqlN("Select count(OutboundID) as N from OBD_OutboundTracking_Warehouse Where DeliveryDate Is NOT NULL AND OutboundID=" + vOBDTrackingID) > 0)
                    {
                        resetError("Sorry, you cannot Revert this to PGI until you Revert the Delivery", true);
                        return;
                    }

                    sqlRevertTrans.Append("BEGIN TRANSACTION    UPDATE OBD_Outbound SET PGIDoneOn= NULL,PGIDoneBy=NULL,DeliveryStatusID=5 WHERE OutboundID=" + vOBDTrackingID);

                    sqlRevertTrans.Append(" ;UPDATE OBD_OutboundTracking_Warehouse set PackedOn=null,PackedBy=null,PackingRemarks=null  WHERE OutboundID=" + vOBDTrackingID + " AND OB_RefWarehouse_DetailsID=" + vRefWHID);
                  
                   
                //  sqlRevertTrans.Append(" AND LocationID IN (select LocationID from INV_Location WHERE LEFT(Location,2) IN(Select LocationCode  from INV_LocationCode Where WarehouseID =" + WH_ID + ")))");


                    //sqlRevertTrans.Append(";UPDATE  GoodsMovementDetails  SET IsActive=0 WHERE  GoodsMovementID IN (Select GoodsMovementID from GoodsMovement Where MMMovementTypeID =2 AND ATCRef_IB_OBD_ID=" + vOBDTrackingID + ")");
                    //InOut =2, OBD_StoreRef_ID =vOBDTrackingID, RevertStatusID=7 (for Revert PGI),
                    //sqlRevertTrans.Append("; INSERT INTO RevertLog(InOut,OBD_StoreRef_ID,RevertStatusID,StoreID,RevertedBy) VALUES(2," + vOBDTrackingID + ",7," + vRefWHID + "," + cp.UserID.ToString() + ")");
                    sqlRevertTrans.Append(" COMMIT  TRANSACTION");


                    try
                    {
                        DB.ExecuteSQL(sqlRevertTrans.ToString());


                        resetError("PGI details successfully reverted", false);
                    }
                    catch (Exception ex)
                    {
                        CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                        resetError("Error while reverting  PGI details", true);
                    }
                    break;


                case "RevertDelivery":


                    if (DB.GetSqlN("select top 1 OBD_CPO.SOHeaderID AS N from OBD_Outbound_ORD_CustomerPO OBD_CPO JOIN MFG_ProductionOrderHeader MFG_POH ON MFG_POH.SOHeaderID=OBD_CPO.SOHeaderID AND MFG_POH.IsActive=1 AND MFG_POH.IsDeleted=0 where OBD_CPO.IsActive=1 AND OBD_CPO.IsDeleted=0 AND  OBD_CPO.OutboundID=" + vOBDTrackingID + " AND MFG_POH.ProductionOrderStatusID<>1") != 0)
                    {
                        resetError("You can not revert this shipment,Production In Process", true);
                        return;
                    }


                    sqlRevertTrans.Append("BEGIN TRANSACTION ");

                    sqlRevertTrans.Append(" UPDATE OBD_Outbound SET DeliveryStatusID=6 WHERE OutboundID=" + vOBDTrackingID);
                    sqlRevertTrans.Append(" ; UPDATE OBD_OutboundTracking_Warehouse SET DeliveryDate=NULL,DocumentNumber=NULL,DriverName=NULL,ReceivedBy=NULL,IsPODReceived=0,DeliveredBy=NULL,PackedBy=NULL,PackedOn=NULL,PackingRemarks=NULL  WHERE OutboundID=" + vOBDTrackingID + " AND OB_RefWarehouse_DetailsID=" + vRefWHID);
                    sqlRevertTrans.Append("; delete from OBD_UsedEquipment where OutboundTracking_WarehouseID=" + OBD_TWHID);
                    /// Since  we do Inventory Confirmation after POD attachment ie. after Delivery 
                    /// We Revert the Inventory during the RevertDelivery Stage as it comprimised both the Delivery and POD attachement for this Store

                    /*
                    int GoodsMvmntID = DB.GetSqlN("select top 1 GoodsMovementDetailsID AS N from INV_GoodsMovementDetails where IsActive=1 and IsDeleted=0 and GoodsMovementDetailsID IN (Select GoodsMovementDetailsID from INV_GoodsMovementDetails Where IsActive=1 and IsDeleted=0 and GoodsMovementTypeID =2 AND  TransactionDocID="+vOBDTrackingID+")");
                    sqlRevertTrans.Append(";delete from INV_PO_GoodsOutLink where SOGoodsMovementDetailsID=" + GoodsMvmntID);
                    sqlRevertTrans.Append(";delete from INV_GoodsMovementDetails_MMT_MaterialStorageParameter where GoodsMovementDetailsID=" + GoodsMvmntID);
                    
                    sqlRevertTrans.Append(";delete from  INV_GoodsMovementDetails  WHERE  GoodsMovementDetailsID IN (Select GoodsMovementDetailsID from INV_GoodsMovementDetails Where GoodsMovementTypeID =2 AND  TransactionDocID=" + vOBDTrackingID);
                    sqlRevertTrans.Append(" AND LocationID IN (select LocationID from INV_Location WHERE LEFT(Location,2) IN(Select LocationCode  from INV_LocationCode Where WarehouseID =" + WH_ID + ")))");*/
                    
                    //InOut =2, OBD_StoreRef_ID =vOBDTrackingID, RevertStatusID=8 (for Revert Delivery),
                    //sqlRevertTrans.Append("; INSERT INTO RevertLog(InOut,OBD_StoreRef_ID,RevertStatusID,StoreID,RevertedBy) VALUES(2," + vOBDTrackingID + ",8," + vStoreID + "," + cp.UserID.ToString() + ")");



                    sqlRevertTrans.Append(" ;COMMIT TRANSACTION ");


                    try
                    {
                        DB.ExecuteSQL(sqlRevertTrans.ToString());

                        DB.ExecuteSQL("[dbo].[sp_INB_CloseSOStatus]  @OutboundID=" + vOBDTrackingID + ",@SOStatusID=1,@Status=1,@UpdatedBy="+cp.UserID.ToString());

                        CommonLogic._DeleteAttatchment(TenantRootDir + DB.GetSqlS("select UniqueID AS S from TPL_Tenant where TenantID=" + cp.TenantID) + OutboundPath + OBD_PODPath, DB.GetSqlS("select OBDNumber AS S from  OBD_Outbound where IsActive=1 AND IsDeleted=0 AND OutboundID=" + vOBDTrackingID) + "_" + DB.GetSqlN("select WarehouseID AS N from OBD_RefWarehouse_Details where IsActive=1 AND IsDeleted=0  AND OB_RefWarehouse_DetailsID=" + vRefWHID) + ".pdf");

                        resetError("Delivery details successfully reverted", false);
                    }
                    catch (Exception ex)
                    {
                        CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                        resetError("Error while reverting  Delivery details", true);
                    }

                   // varFileName = vStoreID + "_" + vOBDNumber;
                   // CommonLogic.DeleteAttachmentFile("DCR", vOBDNumber);
                    break;

                case "RevertandRelease":
                    /*
                    sqlRevertTrans.Append("BEGIN TRANSACTION ");
                    sqlRevertTrans.Append(" UPDATE OBDTracking SET DeliveryStatusID=7 WHERE OBDTrackingID=" + vOBDTrackingID);
                    sqlRevertTrans.Append(" ; UPDATE OBDTracking_WareHouse SET DeliveryDate=NULL,DocumentNumber=NULL,DriverName=NULL,ReceivedBy=NULL,IsDCRReceived=NULL,AOBVehicalTypeID_1=NULL,AOBVehicalQty_1=NULL,AOBVehicalTypeID_2=NULL,AOBVehicalQty_2=NULL,AOBVehicalTypeID_3=NULL,AOBVehicalQty_3=NULL  WHERE OBDTrackingID=" + vOBDTrackingID + " AND StoreID=" + vStoreID);

                    /// Since  we do Inventory Confirmation after POD attachment ie. after Delivery 
                    /// We Revert the Inventory during the RevertDelivery Stage as it comprimised both the Delivery and POD attachement for this Store

                    sqlRevertTrans.Append(" UPDATE  GoodsMovementDetails  SET IsActive=0, Deleted=1 ,LastEditedBy=" + cp.UserID.ToString() + ",LastEditedDateTime=" + DB.DateQuote(DateTime.Now) + ",LastEditedOperatorIP=" + DB.SQuote(CommonLogic.GetUserIP()) + " WHERE  GoodsMovementID IN (Select GoodsMovementID from GoodsMovement Where MMMovementTypeID =2 AND  ATCRef_IB_OBD_ID=" + vOBDTrackingID);
                    sqlRevertTrans.Append(" AND LocationID IN (select LocationID from Location WHERE LEFT(Location,2) IN(Select LocationCode  from LocationCode Where WarehouseID =" + vStoreID + ")))");


                    //InOut =2, OBD_StoreRef_ID =vOBDTrackingID, RevertStatusID=8 (for Revert Delivery),
                    sqlRevertTrans.Append("; INSERT INTO RevertLog(InOut,OBD_StoreRef_ID,RevertStatusID,StoreID,RevertedBy) VALUES(2," + vOBDTrackingID + ",8," + vStoreID + "," + cp.UserID.ToString() + ")");
                    sqlRevertTrans.Append(" ;COMMIT TRANSACTION ");


                    try
                    {
                        DB.ExecuteSQL(sqlRevertTrans.ToString());
                        lblStatusMessage.Text = "Successfully reverted";
                    }
                    catch (Exception ex)
                    {
                        lblStatusMessage.Text = ex.ToString();
                    }

                    varFileName = vStoreID + "_" + vOBDNumber;
                    CommonLogic.DeleteAttachmentFile("DCR", vOBDNumber);*/
                    break;



                case "RevertCloseCancel":
                    /*
                    sqlRevertTrans.Append("BEGIN TRANSACTION ");
                    sqlRevertTrans.Append(" UPDATE OBDTracking SET DeliveryStatusID=1 WHERE OBDTrackingID=" + vOBDTrackingID);
                    //InOut =2, OBD_StoreRef_ID =vOBDTrackingID, RevertStatusID=9 (for  Revert Close Cancel),
                    sqlRevertTrans.Append("; INSERT INTO RevertLog(InOut,OBD_StoreRef_ID,RevertStatusID,StoreID,RevertedBy) VALUES(2," + vOBDTrackingID + ",9," + vStoreID + "," + cp.UserID.ToString() + ")");
                    sqlRevertTrans.Append(" ;COMMIT TRANSACTION ");*/

                    try
                    {
                        DB.ExecuteSQL(sqlRevertTrans.ToString());
                        resetError("Successfully reverted", false);
                    }
                    catch (Exception ex)
                    {
                        CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                        resetError("Error occured while reverting", true);
                    }

                    break;


            }

            this.DeliveryResults_buildGridData(this.DeliveryResults_buildGridData());



        }

        [WebMethod]
        public static string getOBDDetailsForRevert(string outboundid)
        {

            DataTable dt = DB.GetDS("EXEC [dbo].[GET_OUTBOUND_PNC] @OutboundID=" + outboundid,false).Tables[0];

            return JsonConvert.SerializeObject(dt);
          
           

        }
        [WebMethod]
        public static int RevertOutbounDetails(string outboundid,string SodetailsId,string Qty)
        {
            try
            {
                int val = DB.GetSqlN("EXEC [dbo].[UPSERT_PNC_OUTBOUNDWISE] @OutboundID=" + outboundid + ",@SODETAILSID=" + SodetailsId + ",@REVERTQUANTIY=" + Qty + ",@CreatedBy=" + cp1.UserID);
                return val;
            }
            catch(Exception e)
            {
                return -2;
            }
          



        }
        [WebMethod]
        public static int RevertALLLineItemsForOutbound(string outboundid)
        {

            try
            {
                int val = DB.GetSqlN("EXEC [dbo].[UPSERT_PNC_OUTBOUNDWISE] @OutboundID=" + outboundid + ",@SODETAILSID=" + 0 + ",@REVERTQUANTIY=" + 0 + ",@CreatedBy=" + cp1.UserID);
                return val;
            }
            catch (Exception e)
            {
                return -2;
            }



        }

        #endregion --------- gvDeliveryResults --------------
    }
}