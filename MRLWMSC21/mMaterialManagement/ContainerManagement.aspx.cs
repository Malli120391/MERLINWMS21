using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21Common;
using System.Data;
using System.Collections;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Threading;
using System.Web.Services;
using Newtonsoft.Json;

namespace MRLWMSC21.mMaterialManagement
{
    public partial class ContainerManagement : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        public static CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
        int WareHouseID = 0;
        int ContainerTypeID = 0;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;

        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                DesignLogic.SetInnerPageSubHeading(this.Page, "Container Management");

                //CommonLogic.LoadPrinters(ddlPrinters);
                GetContainers(WareHouseID, ContainerTypeID);
                GetWarehouse();
                GetContainerType();
            }


        }
        public void GetContainers(int WareHouseID, int ContainerTypeID)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            //DataSet ds = DB.GetDS("select CartonId,CartonCode from INV_Carton where IsActive=1", false);
            DataSet ds = DB.GetDS("EXEC [dbo].[Get_Containers]  @WareHouseID= " + WareHouseID + ",@ContainerTypeID=" + ContainerTypeID + ",@AccountID = " + cp.AccountID + "", false);
            gvPallet.DataSource = null;
            gvPallet.DataSource = ds;
            gvPallet.DataBind();
            ds.Dispose();
        }


        protected void lnkPrintBarCodeLabel_Click(object sender, EventArgs e)
        {


            //if (hifNetworkPrinter.Value == "0")
            //{
            //    resetError("Printer", true);
            //    return;
            //}

            String vMCode = "";
            cp = HttpContext.Current.User as CustomPrincipal;
            //Print Barcode  the selected Linte Items
            try
            {


                String vltcontainerno = "";

                bool chkBox = false;
                int count = 0;
                string ZPL = "";//================== ADDED BY M.D.PRASAD ON 25-08-18 ============
                foreach (GridViewRow gv in gvPallet.Rows)
                {
                    vltcontainerno = "";
                    CheckBox isPrint = (CheckBox)gv.Cells[3].FindControl("chkIsPrint");
                    if (isPrint.Checked)
                    {
                        chkBox = true;
                        vltcontainerno = ((Literal)gv.Cells[2].FindControl("ltcontainerno")).Text.ToString();
                        MRLWMSC21Common.PrintCommon.PrintContainerLabel containerlabel = new MRLWMSC21Common.PrintCommon.PrintContainerLabel();
                        MRLWMSC21Common.PrintCommon.PrintBO printBo = new MRLWMSC21Common.PrintCommon.PrintBO();
                        printBo.BarcodeString = vltcontainerno;
                        containerlabel.PrintLable_Container(printBo, hifNetworkPrinter.Value);
                        //Commneted By MD. Prasad on 2-1-2019 //ZPL += containerlabel.PrintLable_ZPL(printBo, "0");//================== ADDED BY M.D.PRASAD ON 25-08-18 ============
                        count++;
                    }
                }
                if (count == 0)
                {
                    resetError("Please Select Containers for Print", true);
                }

               // ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "PrintRTR_ZPL('" + ZPL + "');", true);

            }

            catch (Exception ex)
            {
                resetError("Error while Printing", true);
                return;
            }
        }

        //protected void btnDelete_Click(object sender, ImageClickEventArgs e)
        //{
        //    String dltcontainerno = "";
        //    foreach (GridViewRow gv in gvPallet.Rows)
        //    {
        //        dltcontainerno = ((Literal)gv.Cells[1].FindControl("ltcontainerno")).Text.ToString();
        //    }

        //    DataSet ds=DB.GetDS("exec [dbo].[sp_Check_ContainerAvailbleQuantity] @CartonCode='"+dltcontainerno+"'",false);
        //    decimal availablequantity = 0;
        //    decimal goodsinquantity = 0;
        //    decimal goodsoutquantity = 0;
        //    if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count != 0)
        //    {
        //        availablequantity = Convert.ToDecimal(ds.Tables[0].Rows[0]["AvailableQuantity"]);
        //        goodsinquantity = Convert.ToDecimal(ds.Tables[0].Rows[0]["GoodsInQuantity"]);
        //        goodsoutquantity = Convert.ToDecimal(ds.Tables[0].Rows[0]["GoodsOutQuantity"]);
        //        if (availablequantity == 0 && goodsinquantity == 0 && goodsoutquantity == 0)
        //        {
        //           deleteContainers(dltcontainerno);
        //            GetContainers();
        //        }
        //        else
        //        {
        //            resetError("This Container is having available quantity", true);
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        deleteContainers(dltcontainerno);
        //        GetContainers();
        //    }


        //}
        protected void resetError(string error, bool isError)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);
        }

        protected void gvPallet_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPallet.PageIndex = e.NewPageIndex;
            // GetContainers();
            GetContainers(Convert.ToInt32(hifWareHouse.Value), Convert.ToInt32(hifContainertype.Value));
        }
        public void deleteContainers(string ContainerCode)
        {
            if (ContainerCode != "")
            {
                int exist = DB.GetSqlN("select Count(CGMD.CartonID) as N from INV_CartonGoodsMovementDetails CGMD LEFT JOIN INV_Carton CON ON CON.CartonID = CGMD.CartonID where CON.CartonCode ='" + ContainerCode + "'");
                if (exist != 0)
                {
                    resetError("Could not Delete as the Container is configured", true);
                    return;
                }
            }
            DB.ExecuteSQL("update INV_Carton set IsDeleted=1,IsActive=0 where CartonCode='" + ContainerCode + "'");
            //GetContainers();
            GetContainers(Convert.ToInt32(hifWareHouse.Value), Convert.ToInt32(hifContainertype.Value));
        }


        public string getNewCarton(int noofCartons)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            StringBuilder query = new StringBuilder();

            query.Append("DECLARE @NewUpdateCarton nvarchar(50);   ");
            query.Append("EXEC [dbo].[sp_INV_NewContainerCreation]    ");
            //query.Append("@WarehouseCode=" + DB.SQuote(ddlwarehouse.SelectedItem.Text) + ",");
            query.Append("@WarehouseId=" + DB.SQuote(hifWareHouse.Value) + ",");
            query.Append("@UserId=" + cp.UserID + ",");
            query.Append("@NoofCartons=" + noofCartons + ",");
            query.Append("@ContainerTypeID=" + DB.SQuote(hifContainertype.Value) + ",");
            query.Append("@NewCarton=@NewUpdateCarton OUTPUT select @NewUpdateCarton AS S");

            try
            {
                return DB.GetSqlS(query.ToString());

            }
            catch (Exception e)
            {
                return null;
            }

        }

        protected void lnkGenerateNewContainer_Click1(object sender, EventArgs e)
        {
            if (hifWareHouse.Value == "0" || txtWareHouse.Text == "")
            {
                resetError("Please Select Warehouse", true);
                return;
            }
            if (hifContainertype.Value == "0" || txtContainertype.Text == "")
            {
                resetError("Please Select Container Type", true);
                return;
            }
            string cartonCode = getNewCarton(25);

            if (cartonCode != null)
            {
                GetContainers(Convert.ToInt32(hifWareHouse.Value), Convert.ToInt32(hifContainertype.Value));

                ScriptManager.RegisterStartupScript(this, this.GetType(), "test11", "Succes();", true);
            }
            else
            {
                resetError("Error while generating new container", true);
                return;
            }
        }
        public void GetWarehouse()
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            DataSet ds = DB.GetDS("select WareHouseID,WHCode from GEN_Warehouse where IsActive=1 and AccountID = CASE WHEN  " + cp.AccountID.ToString() + " = 0 THEN  AccountID ELSE " + cp.AccountID.ToString() + " END", false);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            //ddlwarehouse.DataSource = dt;
            //ddlwarehouse.DataTextField = "WHCode";
            //ddlwarehouse.DataValueField = "WareHouseID";
            //ddlwarehouse.DataBind();
            //ddlwarehouse.Items.Insert(0, new ListItem("Warehouse", "0"));
        }
        public void GetContainerType()
        {
            DataSet ds = DB.GetDS("SELECT ContainerTypeID,ContainerType FROM INV_ContainerType where IsActive=1 and IsDeleted=0", false);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            //ddlContainerType.DataSource = dt;
            //ddlContainerType.DataTextField = "ContainerType";
            //ddlContainerType.DataValueField = "ContainerTypeID";
            //ddlContainerType.DataBind();
            //ddlContainerType.Items.Insert(0, new ListItem(" Container Type ", "0"));
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {


            //Get the button that raised the event
            LinkButton btn = (LinkButton)sender;

            //Get the row that contains this button
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            String dltcontainernos = ((Literal)gvr.Cells[1].FindControl("ltcontainerno")).Text.ToString();
            //String dltcontainerno = "";
            //foreach (GridViewRow gv in gvPallet.Rows)
            //{
            //    dltcontainerno = ((Literal)gv.Cells[1].FindControl("ltcontainerno")).Text.ToString();
            //}

            DataSet ds = DB.GetDS("exec [dbo].[sp_Check_ContainerAvailbleQuantity] @CartonCode='" + dltcontainernos + "'", false);
            decimal availablequantity = 0;
            decimal goodsinquantity = 0;
            decimal goodsoutquantity = 0;
            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count != 0)
            {
                availablequantity = Convert.ToDecimal(ds.Tables[0].Rows[0]["AvailableQuantity"]);
                goodsinquantity = Convert.ToDecimal(ds.Tables[0].Rows[0]["GoodsInQuantity"]);
                goodsoutquantity = Convert.ToDecimal(ds.Tables[0].Rows[0]["GoodsOutQuantity"]);
                if (availablequantity == 0 && goodsinquantity == 0 && goodsoutquantity == 0)
                {
                    deleteContainers(dltcontainernos);
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "DelSuccess();", true);
                    GetContainers(Convert.ToInt32(hifWareHouse.Value), Convert.ToInt32(hifContainertype.Value));
                    //resetError("Deleted Successfully", false);

                }
                else
                {
                    resetError("This Container is having available quantity", true);
                    return;
                }
            }
            else
            {
                deleteContainers(dltcontainernos);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Keys", "DelSuccess();", true);
                GetContainers(Convert.ToInt32(hifWareHouse.Value), Convert.ToInt32(hifContainertype.Value));
                //resetError("Deleted Successfully", false);

            }
        }

        protected void lnkSearch_Click(object sender, EventArgs e)
        {
            if (hifWareHouse.Value == "0" || txtWareHouse.Text == "")
            {
                resetError("Please Select Warehouse", true);
                return;
            }
            if (hifContainertype.Value == "0" || txtContainertype.Text == "")
            {
                resetError("Please Select Container Type", true);
                return;
            }
            GetContainers(Convert.ToInt32(hifWareHouse.Value.ToString()), Convert.ToInt32(hifContainertype.Value.ToString()));
            //GetContainers(1, Convert.ToInt32(hifContainertype.Value.ToString()));
        }

        [WebMethod]
        public static string getContainerData(string WHID, string ConTID)
        {
            cp1 = HttpContext.Current.User as CustomPrincipal;
            try
            {
                DataSet ds = DB.GetDS("EXEC [dbo].[Get_Containers]  @WareHouseID=" + WHID + ",@ContainerTypeID=" + ConTID + ",@AccountID=" + cp1.AccountID, false);
                return JsonConvert.SerializeObject(ds);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public class Container_Data
        {
            public string ContainerCode { get; set; }

        }

        [WebMethod]
        public static string GetPrint(List<Container_Data> printobj)
        {        

            cp1 = HttpContext.Current.User as CustomPrincipal;
            string strAccountName = " select Account as S  from GEN_Account where AccountID in  ";
            strAccountName += " (select AccountID from GEN_User where UserID="+cp1.UserID+")";
            string ZPL = "";
            try
            {
               string  Result= DB.GetSqlS(strAccountName);
                List<Container_Data> lst = new List<Container_Data>();
                lst = printobj;
                TracklineMLabel Mlabel = new TracklineMLabel();
                for (var i = 0; i < printobj.Count; i++)
                {
                    MRLWMSC21Common.PrintCommon.PrintContainerLabel containerlabel = new MRLWMSC21Common.PrintCommon.PrintContainerLabel();
                    MRLWMSC21Common.PrintCommon.PrintBO printBo = new MRLWMSC21Common.PrintCommon.PrintBO();
                    printBo.AccountName = Result;
                    printBo.BarcodeString = printobj[i].ContainerCode;
                    printBo.PrinterDPI = 203;
                    //ZPL += containerlabel.PrintLable_ZPL(printBo, "0");
                    ZPL += containerlabel.PrintLable_ZPL_FromDB(printBo, "0");
                }
                lst.Clear();

            }
            catch(Exception ex)
            {

            }
           
            return ZPL;// "Printed Successfully";
        }

    }


}
