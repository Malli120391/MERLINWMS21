using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using MRLWMSC21Common;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.Threading;

namespace MRLWMSC21.mInventory
{
    //This class for add inventory 
    //written by GV Durga Prasad
    //created on 4/11/2013
    //last Modified on 6/11/2013
    //Use case ID:  Goods-IN_UC_014

    public partial class StockIn : System.Web.UI.Page
    {
        LinkButton button;

        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected void page_PreInit(object sender, EventArgs e)
        {

            Page.Theme = "Inventory";


        }

        protected void page_Init(object sender, EventArgs e)
        {
            if (CommonLogic.QueryString("ibdno") != "" && CommonLogic.QueryString("mmid") != "" && CommonLogic.QueryString("lno") != "")
            {
                String Outboundid = CommonLogic.QueryString("ibdno");
                String MMID = CommonLogic.QueryString("mmid");
                String LineNumber = CommonLogic.QueryString("lno");
                AddMspTextBoxs(Outboundid, MMID, LineNumber);
                AddQcParametersControls(Outboundid, MMID, LineNumber);
                AddDynamicColumns(MMID);
            }
            //String MCode= txtin_MaterialCode.Text.Trim();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //string parameter = Request["__EVENTARGUMENT"];
            //if (parameter == "CaptureQC")
            //    lnkin_GetDetails_Click(sender, e);

            txtTenant.Enabled = CommonLogic.CheckSuperAdmin(txtTenant, cp, hifTenant);
            DesignLogic.SetInnerPageSubHeading(this.Page, "Goods Receipt (IN)");
            if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("Goods Receipt (IN)"))))
            {
                //test case ID (TC_009)
                //Only authorized user can access goods-in page

                Response.Redirect("../Login.aspx?eid=6");
            }

            if (!IsPostBack)
            {
                Page.Validate();
                ViewState["autoOpen"] = "false";
                CommonLogic.LoadPrinters(ddlNetworkPrinter);
                CommonLogic.LoadLabelSizes(ddlLabelSize);

                if (CommonLogic.QueryString("ibdno") != "" && CommonLogic.QueryString("mmid") != "" && CommonLogic.QueryString("lno") != "" && CommonLogic.QueryString("SPID") != "")
                {
                   
                    int InboundStatus = DB.GetSqlN("select InboundStatusID as N from INB_Inbound where InboundID =" + CommonLogic.QueryString("ibdno"));
                    ViewState["InboundStatus"] = InboundStatus;
                    if (InboundStatus >= 6)
                    {
                        resetError("Shipment is verified, cannot receive ", false);
                        //gvPOQuantityList.Enabled = false;
                        
                        lnkReceive.Enabled = false;
                    }
                    BuildPOQuantityDetails();
                    ViewState["ContainerMapping"] = " select distinct Loc.Location,Loc.LocationID,CGMD.CartonID,cart.CartonCode from  INV_CartonGoodsMovementDetails CGMD left join INV_GoodsMovementDetails  GMD on GMD.GoodsMovementDetailsID=CGMD.GoodsMovementDetailsID and GMD.IsDeleted=0 join INV_Location Loc on Loc.LocationID=gmd.LocationID and loc.IsActive=1 and loc.IsDeleted=0 join INV_Carton cart on cart.CartonID=CGMD.CartonID and cart.IsActive=1 and cart.IsDeleted=0 join INB_Inbound IBT ON IBT.InboundID=GMD.TransactionDocID AND GMD.GoodsMovementTypeID=1 where IBT.StoreRefNo='" + txtin_Storefno.Text.Trim() + "'";//CGMD.GoodsmovementDetailsid,
                    LoadContainerMapping(LoadContainerMapping());
                    GetStorageLocations();
                }
                else
                {

                    atcPOnumber.Visible = false;
                    ltPONumber.Visible = false;
                    tbInBound_FormDetails.Visible = false;
                }
                if (gvPOQuantityList.Rows.Count == 0)
                {
                    pnlGoodsIndata.Visible = false;
                }
                string mmid = CommonLogic.QueryString("mmid") == "" ? "0" : CommonLogic.QueryString("mmid");


                string Isrequired = "0";
                string Parametername = "";

                string Query = "select mm.MaterialMaster_MaterialStorageParameterID,mm.MaterialMasterID,mm.IsRequired,mm.MaterialStorageParameterID,msp.ParameterName from MMT_MaterialMaster_MSP mm join MMT_MSP msp on mm.MaterialStorageParameterID = msp.MaterialStorageParameterID where mm.MaterialMasterID=" + mmid;
                DataSet ds = DB.GetDS(Query, false);
                if (ds.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        Isrequired = row["IsRequired"].ToString();
                        Parametername = row["ParameterName"].ToString();

                        if (ds.Tables != null)
                        {
                            if (Parametername == "MfgDate" && Isrequired == "True")
                            {
                                SpMfgDate.Visible = true;

                            }
                            else if(Parametername == "MfgDate" && Isrequired == "False")
                            {
                                SpMfgDate.Visible = false;

                            }
                            else if (Parametername == "ExpDate" && Isrequired == "True")
                            {

                                SpExpdate.Visible = true;

                            }
                            else if (Parametername == "ExpDate" && Isrequired == "False")
                            {

                                SpExpdate.Visible = false;

                            }
                            else if (Parametername == "SerialNo" && Isrequired == "True")
                            {
                                SpSerialNo.Visible = true;

                            }
                            else if (Parametername == "SerialNo" && Isrequired == "False")
                            {
                                SpSerialNo.Visible = false;

                            }
                            else if (Parametername == "BatchNo" && Isrequired == "True")
                            {
                                SpBatchNo.Visible = true;

                            }
                            else if (Parametername == "BatchNo" && Isrequired == "False")
                            {
                                SpBatchNo.Visible = false;

                            }
                            else if (Parametername == "ProjectRefNo" && Isrequired == "True")
                            {
                                SpProjRefNo.Visible = true;

                            }

                            else if (Parametername == "ProjectRefNo" && Isrequired == "False")
                            {
                                SpProjRefNo.Visible = false;

                            }
                            else
                            {
                                SpProjRefNo.Visible = false;
                                SpBatchNo.Visible = false;
                                SpSerialNo.Visible = false;
                                SpExpdate.Visible = false;
                                SpMfgDate.Visible = false;
                            }
                        }
                    }
                }
                else
                {
                    SpProjRefNo.Visible = false;
                    SpBatchNo.Visible = false;
                    SpSerialNo.Visible = false;
                    SpExpdate.Visible = false;
                    SpMfgDate.Visible = false;

                }
                //SpProjRefNo.Visible = false;
                //SpBatchNo.Visible = false;
                //SpSerialNo.Visible = false;
                //SpExpdate.Visible = false;
                //SpMfgDate.Visible = false;
            }
        }

        public void GetStorageLocations()
        {
            DataSet ds = null;
            try
            {
                ds = DB.GetDS("Exec SP_INV_GET_STORAGELOCATION ", false);

                ddlStorageLocationID.DataSource = ds.Tables[0];
                ddlStorageLocationID.DataTextField = "Code";
                ddlStorageLocationID.DataValueField = "Id";
                ddlStorageLocationID.DataBind();

            }
            catch (Exception ex)
            {

            }

        }

        private DataSet LoadContainerMapping()
        {
            string query = ViewState["ContainerMapping"].ToString();
            return DB.GetDS(query, false);
        }
        private void LoadContainerMapping(DataSet ds)
        {
            gvLocationMapping.DataSource = ds;
            gvLocationMapping.DataBind();

        }

        //this Method for build dropdown
        private void BuildDropDown(DropDownList dropdown, String sql, String ListName, String ListValue, String Defaultvalue)
        {
            dropdown.Items.Clear();

            dropdown.Items.Add(new ListItem());
            try
            {
                dropdown.Items.Clear();

                IDataReader dropdownReader = DB.GetRS(sql);

                dropdown.Items.Add(new ListItem(Defaultvalue, ""));
                while (dropdownReader.Read())
                {
                    dropdown.Items.Add(new ListItem(dropdownReader[ListName].ToString(), dropdownReader[ListValue].ToString()));
                }
                dropdownReader.Close();
            }
            catch (Exception ex)
            {
                resetError("Error while loading",true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }

        private void BuildPOQuantityDetails()
        {
            tbInBound_FormDetails.Visible = false;
            try
            {
                String inboundID = CommonLogic.QueryString("ibdno");
                String MMID = CommonLogic.QueryString("mmid");
                String LineNumber = CommonLogic.QueryString("lno");
                String SUggestedPutawayID = CommonLogic.QueryString("SPID");
                String POID = CommonLogic.QueryString("poid") != "" ? CommonLogic.QueryString("poid") : "0";
                String InvID = CommonLogic.QueryString("invid") != "" ? CommonLogic.QueryString("invid") : "0";

                int POHeaderID;
                DataSet dsresult = DB.GetDS("select II.StoreRefNo as S,TT.TenantName,II.TenantID from INB_Inbound II join TPL_Tenant TT on TT.TenantID=ii.TenantID where II.InboundID=" + inboundID + ";select MCode AS S from MMT_MaterialMaster where MaterialMasterID=" + MMID, false);
                //txtin_Storefno.Text = DB.GetSqlS("select StoreRefNo as S from INB_Inbound where InboundID=" + inboundID);
                //txtin_MaterialCode.Text = DB.GetSqlS("select MCode AS S from MMT_MaterialMaster where MaterialMasterID=" + MMID);

                txtin_Storefno.Text = dsresult.Tables[0].Rows[0][0].ToString();
                txtTenant.Text = dsresult.Tables[0].Rows[0][1].ToString();
                hifTenant.Value = dsresult.Tables[0].Rows[0][2].ToString();
                txtin_MaterialCode.Text = dsresult.Tables[1].Rows[0][0].ToString();
                txtin_poitemline.Text = LineNumber;
                dsresult.Dispose();


                //if PO Header ID is passed
                if (POID != "0")
                {
                    try
                    {
                        atcPOnumber.Text = DB.GetSqlS("select PONumber as S from ORD_POHeader Where POHeaderID=" + POID);
                        hifPONumber.Value = POID;
                        ltPONumber.Visible = true;
                        atcPOnumber.Visible = true;
                    }
                    catch (Exception ex)
                    {
                        resetError("Error while build data", true);
                        CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                    }
                }

                //IF Invoice ID is passed
                if (InvID != "0")
                {
                    try
                    {
                        atcInvoiceNumber.Text = DB.GetSqlS("SELECT InvoiceNumber AS S FROM ORD_SupplierInvoice WHERE SupplierInvoiceID=" + InvID);
                        hifInvoiceNumber.Value = InvID;
                        ltInvoiceNumber.Visible = true;
                        atcInvoiceNumber.Visible = true;
                    }
                    catch (Exception ex)
                    {
                        resetError("Error while build data", true);
                        CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                    }
                }
                DataTable dsPOQuantityList = MRLWMSC21Common.InventoryCommonClass.GetPOQuantityListForStoreRef(inboundID, MMID, LineNumber, POID, InvID, SUggestedPutawayID);

                
                if (dsPOQuantityList.Rows.Count > 1)
                {
                    //IF dublicate records are found

                    //Get dublicate record count
                    int totalRows = dsPOQuantityList.Rows.Count;
                    bool isMultyPOs = false;
                    for (int index = 0; index < totalRows; index++)
                    {
                        for (int innerindex = index + 1; innerindex < totalRows; innerindex++)
                        {

                            //check dublicat while more than two PO's are configure to material in inbound
                            if (DB.RowFieldInt(dsPOQuantityList.Rows[index], "POHeaderID") != DB.RowFieldInt(dsPOQuantityList.Rows[innerindex], "POHeaderID"))
                            {
                                isMultyPOs = true;
                                index = totalRows;
                                break;
                            }
                        }
                    }
                    //Multi PO's are 
                    if (isMultyPOs)
                    {
                        resetError("Multi Records are found, Select PONumber", true);
                        atcPOnumber.Visible = true;
                        ltPONumber.Visible = true;
                    }
                    //Multi Invoice are configure 
                    else
                    {
                        resetError("Multi records are found, select invoice no.", true);
                        atcInvoiceNumber.Visible = true;
                        ltInvoiceNumber.Visible = true;
                    }
                    //DataSet set = new DataSet();
                    // ViewState["gvPOQuantityList"] = "EXEC dbo.sp_INV_GetGoodsMovementDetailsList @MaterialMasterID=" + MMID + ",@InboundID=" + inboundID + ",@LineNumber=" + txtin_poitemline.Text.Trim() + ",@POHeaderID=" + hifPONumber.Value;
                    // DataSet dscleardaset = Build_gvPOQuantityilst();
                    // dscleardaset.Clear();
                    tbInBound_FormDetails.Visible = false;
                    pnlPOQuantityList.Visible = true;
                    //Build_gvPOQuantityilst(dscleardaset);
                    return;
                }

                //Get exact record 
                else if (dsPOQuantityList.Rows.Count == 1)
                {
                    ltKitplannerValue.Visible = true;
                    ltkitPlanner.Visible = true;
                    //tbInBound_FormDetails.Visible = true;
                    //tdGridview.Visible = true;
                    ViewState["IsReeturned"] = Convert.ToBoolean(DB.RowFieldInt(dsPOQuantityList.Rows[0], "IsReeturned"));
                    ltBaseUoMQtyValue.Text = DB.RowField(dsPOQuantityList.Rows[0], "BUoM") + "/" + DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "BUoMQty");
                    POHeaderID = DB.RowFieldInt(dsPOQuantityList.Rows[0], "POHeaderID");
                    ltPOUoMQtyValue.Text = DB.RowField(dsPOQuantityList.Rows[0], "PUoM") + "/" + DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "PUoMQty");
                    ltInvUoMQtyValue.Text = DB.RowField(dsPOQuantityList.Rows[0], "InvUoM") + "/" + DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "InvUoMQty");
                    hifUoMQty.Value = DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "MUoMQty").ToString();
                    hifBUoMQty.Value = DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "BUoMQty").ToString();
                    ltMUoMQty.Text = DB.RowField(dsPOQuantityList.Rows[0], "MUoM") + "/" + DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "MUoMQty");
                    
                    ltDescriptionvalue.Text = DB.RowField(dsPOQuantityList.Rows[0], "MDescription");
                    
                    ltKitplannerValue.Text = DB.RowFieldInt(dsPOQuantityList.Rows[0], "KitplannerID").ToString();
                    if (DB.RowFieldInt(dsPOQuantityList.Rows[0], "KitplannerID") == 0)
                    {
                        ltKitplannerValue.Visible = false;
                        ltkitPlanner.Visible = false;
                    }
                    //String ConvertionFactor;
                    txtContainerCode.Text = "";
                    txtreceiveqty.Text = "";
                    txtPutaway.Text = DB.RowField(dsPOQuantityList.Rows[0], "Location");
                    hifputwayloc.Value = DB.RowFieldInt(dsPOQuantityList.Rows[0], "SuggestedLocationID").ToString();
                    // Calculating Conversion factor value
                    hifMeasurementType.Value = DB.RowFieldInt(dsPOQuantityList.Rows[0], "MeasurementTypeID").ToString();
                    if (DB.RowFieldInt(dsPOQuantityList.Rows[0], "HasSerialNo") == 1)
                    {
                        ltInvqty.Text = "Quantity";
                        trSerialText.Visible = true;
                        ltInvqtyvalue.Text = Math.Round(DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "InvQty"), 2).ToString();

                        hifInvUoMQty.Value = DB.RowFieldInt(dsPOQuantityList.Rows[0], "MaterialMaster_UoMID").ToString();

                        ltreceiveqtyperuomValue.Text =Math.Round( DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "CF"),5).ToString();
                        hifConversion.Value = "1.00";
                        //hifconversionInMUoM.Value = DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "CFInMUoM").ToString();
                        ltTotalQuantityvalue.Text = Math.Round(DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "CF") * DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "InvQty"), 2).ToString();
                        ViewState["ConvertionFactor"] = "# Convertion:BUoMQty :1";
                    }
                    else
                    {
                        ltInvqtyvalue.Text = DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "InvQty").ToString();

                        hifInvUoMQty.Value = DB.RowFieldInt(dsPOQuantityList.Rows[0], "MaterialMaster_InvUoMID").ToString();

                        ltreceiveqtyperuomValue.Text = Math.Round(DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "CF"), 3).ToString();
                        hifConversion.Value = DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "CF").ToString();
                        //hifconversionInMUoM.Value = DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "CFInMUoM").ToString();
                        ltTotalQuantityvalue.Text = Math.Round(DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "CF") * DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "InvQty"), 2).ToString();


                        if (DB.RowFieldInt(dsPOQuantityList.Rows[0], "MeasurementTypeID") == 0)// Is Other measuremet or not
                        {
                            if (DB.RowField(dsPOQuantityList.Rows[0], "InvUoM") == DB.RowField(dsPOQuantityList.Rows[0], "MUoM") && DB.RowField(dsPOQuantityList.Rows[0], "MUoM") == DB.RowField(dsPOQuantityList.Rows[0], "BUoM"))
                            {
                                //ltreceiveqtyperuomValue.Text = Math.Round(DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "MUoMQty") / DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "BUoMQty"), 2).ToString();
                                //hifConversion.Value = ltreceiveqtyperuomValue.Text;
                                //hifconversionInMUoM.Value = ltreceiveqtyperuomValue.Text;
                                ViewState["ConvertionFactor"] = "# Convertion: MUoMQty/BUoMQty :" + ltreceiveqtyperuomValue.Text;
                                //ltTotalQuantityvalue.Text = Math.Round(DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "MUoMQty") / DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "BUoMQty") * DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "InvQty"), 2).ToString();
                            }
                            else if (DB.RowField(dsPOQuantityList.Rows[0], "InvUoM") == DB.RowField(dsPOQuantityList.Rows[0], "MUoM"))
                            {
                                //ltreceiveqtyperuomValue.Text = Math.Round(DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "MUoMQty") * (DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "InvUoMQty") / DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "MUoMQty")), 2).ToString();
                                //hifConversion.Value = ltreceiveqtyperuomValue.Text;
                                //hifconversionInMUoM.Value = DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "MUoMQty").ToString();
                                ViewState["ConvertionFactor"] = "# Convertion: MUoMQty*InvUoMQty/MUoMQty:" + ltreceiveqtyperuomValue.Text;
                                //ltTotalQuantityvalue.Text = Math.Round(DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "InvQty") * DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "InvUoMQty"), 2).ToString();
                            }
                            else if (DB.RowField(dsPOQuantityList.Rows[0], "MUoM") == DB.RowField(dsPOQuantityList.Rows[0], "BUoM"))
                            {
                                //ltreceiveqtyperuomValue.Text = Math.Round(DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "MUoMQty") / DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "BUoMQty") * DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "InvUoMQty"), 2).ToString();
                                //hifConversion.Value = ltreceiveqtyperuomValue.Text;
                                //hifconversionInMUoM.Value = Math.Round(DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "MUoMQty") / DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "BUoMQty"), 2).ToString();
                                ViewState["ConvertionFactor"] = "# Convertion: MUoMQty/BUoMQty*InvUoMQty:" + ltreceiveqtyperuomValue.Text;
                                //ltTotalQuantityvalue.Text = Math.Round(DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "InvQty") * (DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "MUoMQty") / DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "BUoMQty") * DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "InvUoMQty")), 2).ToString();
                            }
                            else
                            {
                                //ltreceiveqtyperuomValue.Text = Math.Round(DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "MUoMQty") * DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "InvUoMQty"), 2).ToString();
                                //hifConversion.Value = ltreceiveqtyperuomValue.Text;
                                //hifconversionInMUoM.Value = DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "MUoMQty").ToString();
                                ViewState["ConvertionFactor"] = "# Convertion: MUoMQty*InvUoMQty :" + ltreceiveqtyperuomValue.Text;
                                //ltTotalQuantityvalue.Text = Math.Round(DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "InvQty") * DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "MUoMQty") * DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "InvUoMQty"), 2).ToString();
                            }
                        }
                        else
                        {
                            //decimal conversionValue = DB.GetSqlNDecimal("select convert(decimal(18,2),ISNULL(MTD.ConvesionValue,1)*IIF(ISNULL(MPM1.PowerOf,0)-ISNULL(MPM.PowerOf,0)<0,1/POWER(convert(float,10),ISNULL(MPM.PowerOf,0)-ISNULL(MPM1.PowerOf,0)),POWER(convert(float,10),ISNULL(MPM1.PowerOf,0)-ISNULL(MPM.PowerOf,0)))) N "
                            //                                         + " from GEN_UoM BUOM "
                            //                                         + " JOIN GEN_UoM IUOM ON IUOM.UoMID= " + DB.RowFieldInt(dsPOQuantityList.Rows[0], "IUoMID")
                            //                                        + " join MES_MeasurementMaster MTM on MTM.MeasurementID=BUOM.MeasurementID "
                            //                                        + " LEFT JOIN MES_MatricPrefixMaster MPM ON MTM.ConversionTypeID=1 and MPM.MetricPrifixID=BUOM.MatricPrifixID "
                            //                                        + " left join MES_MatricPrefixMaster MPM1 ON MPM1.MetricPrifixID=IUOM.MatricPrifixID "
                            //                                        + " LEFT join MES_MeasurementDetails MTD on MTD.MeasurementID=IUOM.MeasurementID  AND MTD.ToMesurementID=MTM.MeasurementID "
                            //                                        + "WHERE BUOM.UoMID=" + DB.RowFieldInt(dsPOQuantityList.Rows[0], "BUoMID"));

                            //String cmdGetConversionValues = "select	convert(decimal(18,3),ISNULL(MTD.ConvesionValue,1)*IIF(ISNULL(IMPM.PowerOf,0)-ISNULL(MPM.PowerOf,0)<0,1/POWER(convert(float,10),ISNULL(MPM.PowerOf,0)-ISNULL(IMPM.PowerOf,0)),POWER(convert(float,10),ISNULL(IMPM.PowerOf,0)-ISNULL(MPM.PowerOf,0)))) TotalConversion,"

                            //                               + " convert(decimal(18,6),ISNULL(MMTD.ConvesionValue,1)*IIF(ISNULL(IMPM.PowerOf,0)-ISNULL(MMPM.PowerOf,0)<0,1/POWER(convert(float,10),ISNULL(MMPM.PowerOf,0)-ISNULL(IMPM.PowerOf,0)),POWER(convert(float,10),ISNULL(IMPM.PowerOf,0)-ISNULL(MMPM.PowerOf,0)))) MinPickConversion"
                            //                               + " from GEN_UoM BUOM "
                            //                               + " join MES_MeasurementMaster MTM on MTM.MeasurementID=BUOM.MeasurementID"
                            //                               + " LEFT JOIN MES_MatricPrefixMaster MPM ON MTM.ConversionTypeID=1 and MPM.MetricPrifixID=BUOM.MatricPrifixID"

                            //                               + " JOIN GEN_UoM IUOM ON IUOM.UoMID=" + DB.RowFieldInt(dsPOQuantityList.Rows[0], "IUoMID")
                            //                               + " left join MES_MatricPrefixMaster IMPM ON IMPM.MetricPrifixID=IUOM.MatricPrifixID "
                            //                               + " LEFT join MES_MeasurementDetails MTD on MTD.MeasurementID=IUOM.MeasurementID  AND MTD.ToMesurementID=MTM.MeasurementID "

                            //                               + " JOIN GEN_UoM MUOM ON MUOM.UoMID=" + DB.RowFieldInt(dsPOQuantityList.Rows[0], "MUoMID")
                            //                               + " left join MES_MatricPrefixMaster MMPM ON MMPM.MetricPrifixID=MUOM.MatricPrifixID "
                            //                               + " LEFT join MES_MeasurementDetails MMTD ON MMTD.MeasurementID=IUOM.MeasurementID  AND MMTD.ToMesurementID=MUOM.MeasurementID "

                            //                               + " WHERE BUOM.UoMID=" + DB.RowFieldInt(dsPOQuantityList.Rows[0], "BUoMID");

                            //IDataReader rsGetConversionValues = DB.GetRS(cmdGetConversionValues);
                            //if (rsGetConversionValues.Read())
                            //{

                            //    ltreceiveqtyperuomValue.Text = Math.Round(DB.RSFieldDecimal(rsGetConversionValues, "TotalConversion") * DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "InvUoMQty") / DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "BUoMQty"), 3).ToString();
                            //    hifConversion.Value = Math.Round(DB.RSFieldDecimal(rsGetConversionValues, "TotalConversion") * DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "InvUoMQty") / DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "BUoMQty"), 6).ToString();
                            //    hifconversionInMUoM.Value = (DB.RSFieldDecimal(rsGetConversionValues, "MinPickConversion") * DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "InvUoMQty") / DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "MUoMQty")).ToString();
                            ViewState["ConvertionFactor"] = "# Convertion: Measurement Conversion:" + ltreceiveqtyperuomValue.Text;
                            //    ltTotalQuantityvalue.Text = Math.Round(DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "InvQty") * DB.RSFieldDecimal(rsGetConversionValues, "TotalConversion") * DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "InvUoMQty") / DB.RowFieldDecimal(dsPOQuantityList.Rows[0], "BUoMQty"), 2).ToString();
                            //}
                            //rsGetConversionValues.Close();
                        }
                    }
                    IDataReader rsConfigureMSP = DB.GetRS("[sp_ORD_GetMaterialStorageParameters] @MaterialMasterID=" + MMID + ",@TenantID=" + cp.TenantID);
                    while (rsConfigureMSP.Read())
                    {
                        String ControlType = DB.RSField(rsConfigureMSP, "ControlType");
                        String ControlID = "";


                        ((HiddenField)tbMaterialStorageparameter.FindControl("hif" + DB.RSField(rsConfigureMSP, "ParameterName"))).Value = DB.RowField(dsPOQuantityList.Rows[0], DB.RSField(rsConfigureMSP, "ParameterName"));
                        if ((bool)ViewState["IsReeturned"])
                        {
                            if (ControlType == "DropDownList")
                            {
                                ControlID = "ddl" + DB.RSField(rsConfigureMSP, "ParameterName");
                                ((DropDownList)tbMaterialStorageparameter.FindControl(ControlID)).SelectedValue = DB.RowField(dsPOQuantityList.Rows[0], DB.RSField(rsConfigureMSP, "ParameterName"));
                                ((DropDownList)tbMaterialStorageparameter.FindControl(ControlID)).Enabled = false;
                            }
                            else
                            {
                                ControlID = "txt" + DB.RSField(rsConfigureMSP, "ParameterName");
                                ((TextBox)tbMaterialStorageparameter.FindControl(ControlID)).Text = DB.RowField(dsPOQuantityList.Rows[0], DB.RSField(rsConfigureMSP, "ParameterName"));
                                ((TextBox)tbMaterialStorageparameter.FindControl(ControlID)).Enabled = false;
                            }
                        }

                    }
                    rsConfigureMSP.Close();

                    tbInBound_FormDetails.Visible = true;
                    pnlPOQuantityList.Visible = true;
                    ViewState["gvPOQuantityList"] = "EXEC dbo.sp_INV_GetGoodsMovementDetailsList @MaterialMasterID=" + MMID + ",@InboundID=" + inboundID + ",@LineNumber=" + txtin_poitemline.Text.Trim() + ",@POHeaderID=" + hifPONumber.Value + ",@SupplierInvoiceID=" + hifInvoiceNumber.Value;
                    Build_gvPOQuantityilst(Build_gvPOQuantityilst());
                    Decimal QuantitySum = DB.GetSqlNDecimal("sp_INV_GetGoodsInQuantitySum @MaterialMasterID=" + MMID + ",@InboundID=" + inboundID + ",@LineNumber=" + txtin_poitemline.Text.Trim() + ",@POHeaderID=" + hifPONumber.Value + ", @GoodsMovementDetailsID = 0" + ",@SupplierInvoiceID=" + hifInvoiceNumber.Value);
                    if (gvPOQuantityList.Rows.Count != 0)
                        ((Literal)gvPOQuantityList.FooterRow.FindControl("ltQuantityCount")).Text = "  Sum : " + QuantitySum;
                    Page.Validate();
                }
                //zero records are found
                else
                {
                    tbInBound_FormDetails.Visible = false;
                    pnlPOQuantityList.Visible = false;
                    resetError("No data found for the given criteria", true);
                }
                dsPOQuantityList.Dispose();
            }
            catch (Exception ex)
            {
                resetError("Error while build data", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }

        //this method for get the details based on materials,storerefno,poheaderid,linenumber
        protected void lnkin_GetDetails_Click(object sender, EventArgs e)
        {
            //Based on details redirect the same page 

            //tbInBound_FormDetails.Visible = false;
            Page.Validate("getDetails");
            if (IsValid)
            {
                int MMID = 0;
                int InboundID = 0;

                try
                {

                    MMID = DB.GetSqlN("select MaterialMasterID AS N from MMT_MaterialMaster where MCode=" + DB.SQuote(txtin_MaterialCode.Text.Trim()));
                    InboundID = DB.GetSqlN("select InboundID as N from INB_Inbound where StoreRefNo=" + DB.SQuote(txtin_Storefno.Text.Trim()));

                    
                }
                catch (Exception ex)
                {
                    resetError("Error try again", true);
                    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                }
                if (MMID != 0 && InboundID != 0)
                {
                    if (atcPOnumber.Visible && atcInvoiceNumber.Visible)
                        Response.Redirect("StockIn.aspx?ibdno=" + InboundID + "&mmid=" + MMID + "&lno=" + txtin_poitemline.Text.Trim() + "&poid=" + hifPONumber.Value + "&invid=" + hifInvoiceNumber.Value);
                    else if (atcPOnumber.Visible)
                        Response.Redirect("StockIn.aspx?ibdno=" + InboundID + "&mmid=" + MMID + "&lno=" + txtin_poitemline.Text.Trim() + "&poid=" + hifPONumber.Value);
                    else if (atcInvoiceNumber.Visible)
                        Response.Redirect("StockIn.aspx?ibdno=" + InboundID + "&mmid=" + MMID + "&lno=" + txtin_poitemline.Text.Trim() + "&invid=" + hifInvoiceNumber.Value);
                    else
                    {
                        

                        Response.Redirect("StockIn.aspx?ibdno=" + InboundID + "&mmid=" + MMID + "&lno=" + txtin_poitemline.Text.Trim());
                    }
                }
            }
            //else

        }

        //Add dynamic MSP dropdown
        private void BuildDynamicDropDown(DropDownList dropdown, String sql, String HeaderName)
        {
            dropdown.Items.Clear();

            dropdown.Items.Add(new ListItem());
            try
            {
                dropdown.Items.Clear();

                IDataReader dropdownReader = DB.GetRS(sql);

                dropdown.Items.Add(new ListItem("Select " + HeaderName, ""));
                while (dropdownReader.Read())
                {
                    dropdown.Items.Add(new ListItem(dropdownReader[0].ToString(), dropdownReader[1].ToString()));
                }
                dropdownReader.Close();
            }
            catch (Exception ex)
            {
                resetError("Error while loading",true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }

        //Update details of receive contity
        protected void inkReceive_Click(object sender, EventArgs e)
        {

            if (txtreceiveqty.Text.Trim() == "")
            {
                resetError("Please Enter Receive Quantiy", true);
                txtreceiveqty.Focus();
                return;
            }


            String mmid1 = CommonLogic.QueryString("mmid");
            String SuggestPutawayID = CommonLogic.QueryString("SPID");
            string Isrequired = "0";
            string Parametername = "";

            string Query = "select mm.MaterialMaster_MaterialStorageParameterID,mm.MaterialMasterID,mm.IsRequired,mm.MaterialStorageParameterID,msp.ParameterName from MMT_MaterialMaster_MSP mm join MMT_MSP msp on mm.MaterialStorageParameterID = msp.MaterialStorageParameterID where mm.MaterialMasterID=" + mmid1;
            DataSet ds = DB.GetDS(Query, false);
            if (ds.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Isrequired = row["IsRequired"].ToString();
                    Parametername = row["ParameterName"].ToString();

                    if (ds.Tables != null)
                    {
                        if (Parametername == "MfgDate" && Isrequired == "True")
                        {
                            if (txtMfgDate.Text.Trim() == "")
                            {
                                resetError("Please Enter Mandatory Mfg. Date", true);
                                txtMfgDate.Focus();
                                SpMfgDate.Visible = true;
                                return;
                            }
                            //else
                            //{
                            //    SpMfgDate.Visible = false;
                            //}
                        }
                        else if (Parametername == "ExpDate" && Isrequired == "True")
                        {
                            if (txtExpDate.Text.Trim() == "")
                            {
                                resetError("Please Enter Mandatory Exp. Date", true);
                                txtExpDate.Focus();
                                SpExpdate.Visible = true;
                                return;
                            }
                            //else
                            //{
                            //    SpExpdate.Visible = false;
                            //}
                        }

                        else if (Parametername == "SerialNo" && Isrequired == "True")
                        {
                            if (txtSerialNo.Text.Trim() == "")
                            {
                                resetError("Please Enter Mandatory Serial No", true);
                                txtSerialNo.Focus();
                                SpSerialNo.Visible = true;
                                return;
                            }
                            //else
                            //{
                            //    SpSerialNo.Visible = false;
                            //}
                        }

                        else if (Parametername == "BatchNo" && Isrequired == "True")
                        {
                            if (txtBatchNo.Text.Trim() == "")
                            {
                                resetError("Please Enter Mandatory Batch No", true);
                                txtBatchNo.Focus();
                                SpBatchNo.Visible = true;
                                return;
                            }
                            //else
                            //{
                            //    SpBatchNo.Visible = false;
                            //}
                        }

                        else if (Parametername == "ProjectRefNo" && Isrequired == "True")
                        {
                            if (txtProjectRefNo.Text.Trim() == "")
                            {
                                resetError("Please Enter Mandatory Project Ref. No ", true);
                                txtProjectRefNo.Focus();
                                SpProjRefNo.Visible = true;
                                return;
                            }
                            //else
                            //{
                            //    SpProjRefNo.Visible = false;
                            //}
                        }
                    }
                }
            }

           

            if (txtContainerCode.Text.Trim() == "")
            {
                resetError("Please Select Container Code", true);
                txtContainerCode.Focus();
                return;
            }
            if (hifContainercode.Value == "" || hifContainercode.Value == "0")
            {
                resetError("Please Select Container Code", true);
                txtPutaway.Focus();
                return;
            }
            if (txtPutaway.Text.Trim() == "")
            {
                resetError("Please Select Location", true);
                txtPutaway.Focus();
                return;
            }
            else
            {
                if (txtPutaway.Text.Trim() == "STAGING")
                {
                    resetError("Please Select Location", true);
                    txtPutaway.Focus();
                    return;
                }
            }
            


            Page.Validate("receiveQuantity");
            if (IsValid)
            {
                
                
                if (ltInvqtyvalue.Text == "0" || ltInvUoMQtyValue.Text == "/0")
                {
                    resetError("Invoice not configure, cannot receive", true);
                    return;
                }

                if (txtMfgDate.Text.Trim() != null && txtMfgDate.Text.Trim() !="")
                {
                    DateTime expiredate = DateTime.ParseExact(txtMfgDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    int result = DateTime.Compare(expiredate, DateTime.Now);
                    if (result > 0)
                    {
                        //test case ID (TC_040)
                        //Mfg. date should not exceed system date, cannot receive

                        resetError("Mfg. date should not exceed system date, cannot receive", true);
                        
                        return;
                    }
                }

                if (txtExpDate.Text.Trim() != null && txtExpDate.Text.Trim() != "")
                {
                    DateTime expiredate = DateTime.ParseExact(txtExpDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    int result = DateTime.Compare(expiredate, DateTime.Now);
                    if (result < 0)
                    {
                        //test case ID (TC_039)
                        //Expired items cannot be received

                        resetError("Cannot receive expired items", true);
                        
                        return;
                    }
                }

                

                bool IsReturned = (bool)ViewState["IsReeturned"];
                bool CheckDiscrepancy = false;
                String MissMatchedMSPS = "";
                int CamaPosition = 0;
                String DocQuantity = txtreceiveqty.Text.Trim();

                if (txtSerialNo != null && txtSerialNo.Text != "")
                {

                    if (Convert.ToDecimal(DocQuantity) != 1)
                    {
                        //test case ID (TC_037)
                        //Qty. should be one while serial no. capturing

                        resetError("Quantity should be 1 for serial no., cannot receive", true);
                        return;
                    }
                    if ((!(bool)ViewState["IsReeturned"]) && DB.GetSqlN("select GMDMSP.GoodsMovementDetails_MaterialStorageParameterID as N from INV_GoodsMovementDetails GMD JOIN INV_GoodsMovementDetails_MMT_MaterialStorageParameter GMDMSP ON GMD.GoodsMovementDetailsID=GMDMSP.GoodsMovementDetailsID AND GMDMSP.MaterialStorageParameterID=3 AND GMDMSP.IsDeleted=0 WHERE GMD.GoodsMovementTypeID=1 AND GMD.IsDeleted=0 and GMD.MaterialMasterID=" + CommonLogic.QueryString("mmid") + "  AND CONVERT(NVARCHAR,GMDMSP.Value)=" + DB.SQuote(txtSerialNo.Text)) != 0)
                    {
                        //test case ID (TC_038)
                        //Serial no. already exists, cannot receive

                        resetError("Serial no. already exists, cannot receive", true);
                        return;
                    }
                }

                String Quantity = (Convert.ToDecimal(DocQuantity) * Convert.ToDecimal(ltreceiveqtyperuomValue.Text)).ToString();
                String IUoM_QtyID = hifInvUoMQty.Value;
                String Location = txtPutaway.Text.Trim();
                String reMarkes = txtRemarks.Text.Trim();
                int IsDamaged = Convert.ToInt16(chkIsDamaged.Checked);
                String StoreRefno = txtin_Storefno.Text.Trim();
                String mmid = CommonLogic.QueryString("mmid");
                String inbno = CommonLogic.QueryString("ibdno");
                String LineNumber = txtin_poitemline.Text.Trim();
                String MCode = txtin_MaterialCode.Text.Trim();
                String POHeaderID = hifPONumber.Value;
                int HasDiscrepancy = Convert.ToInt16(chkHasDiscrepancy.Checked);

                String MfgDate = "";
                if (txtMfgDate.Text.Trim() != "")
                    MfgDate = GetDateTime(txtMfgDate.Text.Trim());
                    //MfgDate = DB.DateQuote(CommonLogic.GetConfigValue(cp.TenantID, "DateFormat") == "dd/MM/yyyy" ? DateTime.ParseExact(txtMfgDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : txtMfgDate.Text.Trim());
                else
                    MfgDate = "";

                String ExpDate = "";
                if (txtExpDate.Text.Trim() != "")
                    //ExpDate = DB.DateQuote(CommonLogic.GetConfigValue(cp.TenantID, "DateFormat") == "dd/MM/yyyy" ? DateTime.ParseExact(txtExpDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : txtExpDate.Text.Trim());
                    ExpDate = GetDateTime(txtExpDate.Text.Trim());
                else
                    ExpDate = "";

                //string MfgDate = txtMfgDate.Text.Trim();
                //string ExpDate = txtExpDate.Text.Trim();
                string SerialNo = txtSerialNo.Text.Trim();
                string BatchNo = txtBatchNo.Text.Trim();
                string ProjectRefNo = txtProjectRefNo.Text.Trim();


                try
                {

                    //check shipment status
                    IDataReader rsGetReceivetime = DB.GetRS("select ShipmentReceivedOn from INB_InboundTracking_Warehouse inbw join INB_Inbound inb on inb.InboundID=inbw.InboundID and  inb.IsDeleted=0 and inb.IsActive=1 where inbw.IsDeleted=0 and inbw.IsActive=1 and  inb.InboundID=" + CommonLogic.QueryString("ibdno"));
                    if (!rsGetReceivetime.Read())
                    {
                        resetError("Shipment is not received, cannot receive items", true);
                        rsGetReceivetime.Close();
                        return;
                    }
                    rsGetReceivetime.Close();

                    //Quantity shuld be non zero value
                    if (Convert.ToDecimal(Quantity) <= 0)
                    {

                        //test case ID (TC_036)
                        //Qty. cannot be zero

                        resetError("Quantity should be greater than zero", true);
                        return;
                    }

                    //check shipment status
                    int InboundStatus = DB.GetSqlN("select InboundStatusID as N from INB_Inbound where StoreRefNo ='" + StoreRefno + "'");
                    if (InboundStatus >= 6)
                    {
                        resetError("Shipment is verified, cannot receive ", true);
                        return;
                    }

                    //check if Material is in cycle count
                    int CycleCountOn = DB.GetSqlN("select convert(int,max(qcc.IsOn)) as N from QCC_CycleCount qcc left join QCC_CycleCountDetails qccd on qccd.CycleCountID=qcc.CycleCountID  where qccd.MaterialMasterID=" + CommonLogic.QueryString("mmid"));
                    if (CycleCountOn == 1)
                    {
                        //test case ID (TC_043)
                        //Cannot receive, as material is in cycle count

                        resetError("Cannot receive, as this material is in 'Cycle Count' ", true);
                        return;
                    }

                    //check Enter Location is valid or not
                    int Locationid = DB.GetSqlN("SELECT LocationID AS N FROM INV_Location LOC JOIN INV_LocationZone WHZ ON WHZ.LocationZoneCode=LEFT(LOC.Location,2) AND WHZ.IsActive=1 AND WHZ.IsDeleted=0 JOIN INB_RefWarehouse_Details RFD ON RFD.WarehouseID=WHZ.WarehouseID AND RFD.IsActive=1 AND RFD.IsDeleted=0 WHERE LOC.IsDeleted=0 AND RFD.InboundID=" + CommonLogic.QueryString("ibdno") + " AND Location=" + DB.SQuote(txtPutaway.Text.Trim()));
                    if (Locationid == 0 && txtContainerCode.Text == "")
                    {
                        resetError("Please enter valid location", true);
                        return;
                    }
                    else if (Locationid == 0)
                    {
                        //Locationid = DB.GetSqlN("select top 1 gmd.LocationID as N from INV_GoodsMovementDetails GMD join INV_CartonGoodsMovementDetails CGMD on GMD.GoodsMovementDetailsID=CGMD.GoodsMovementDetailsID join INV_Carton Cart on cart.CartonID=cgmd.CartonID where GMD.LocationID is not null ");
                        Location = DB.GetSqlS("select top 1 loc.Location as S from INV_GoodsMovementDetails GMD  join INV_CartonGoodsMovementDetails CGMD on GMD.GoodsMovementDetailsID=CGMD.GoodsMovementDetailsID and CGMD.IsDeleted=0 join INV_Carton Cart on cart.CartonID=cgmd.CartonID and cart.IsActive=1 and cart.IsDeleted=0 join INV_Location loc on loc.LocationID=gmd.LocationID where GMD.IsDeleted=0 and GMD.LocationID is not null and  cart.CartonCode like '_______" + txtin_Storefno.Text + "%' and cart.CartonID=" + hifContainercode.Value);
                        if (Location == "")
                        {
                            Location = "0";
                        }
                    }


                    IDataReader rsConfigureMSP = DB.GetRS("[sp_ORD_GetMaterialStorageParameters] @MaterialMasterID=" + CommonLogic.QueryString("mmid") + ",@TenantID=" + cp.TenantID);
                    String ControlID = "";
                    String value = "";
                    String MSPIDs = "";
                    String MSPValues = "";
                    String HiddenValue = "";

                    while (rsConfigureMSP.Read())
                    {
                        String ControlType = DB.RSField(rsConfigureMSP, "ControlType");


                        if (ControlType == "DropDownList")
                        {
                            ControlID = "ddl" + DB.RSField(rsConfigureMSP, "ParameterName");
                            value = ((DropDownList)tbMaterialStorageparameter.FindControl(ControlID)).SelectedValue;

                        }
                        else if (ControlType == "TextBox")
                        {
                            ControlID = "txt" + DB.RSField(rsConfigureMSP, "ParameterName");
                            TextBox txtTextBox = (TextBox)tbMaterialStorageparameter.FindControl(ControlID);

                            if (!IsReturned && DB.RSField(rsConfigureMSP, "ParameterName") == "ExpDate")
                            {

                                if (txtTextBox != null && txtTextBox.Text != "")
                                {
                                    DateTime expiredate = DateTime.ParseExact(txtTextBox.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    int result = DateTime.Compare(expiredate, DateTime.Now);
                                    if (result < 0)
                                    {
                                        //test case ID (TC_039)
                                        //Expired items cannot be received

                                        resetError("Cannot receive expired items", true);
                                        rsConfigureMSP.Close();
                                        return;
                                    }
                                }

                            }
                            else if (DB.RSField(rsConfigureMSP, "ParameterName") == "MfgDate")
                            {

                                if (txtTextBox != null && txtTextBox.Text != "")
                                {
                                    DateTime expiredate = DateTime.ParseExact(txtTextBox.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    int result = DateTime.Compare(expiredate, DateTime.Now);
                                    if (result > 0)
                                    {
                                        //test case ID (TC_040)
                                        //Mfg. date should not exceed system date, cannot receive

                                        resetError("Mfg. date should not exceed system date, cannot receive", true);
                                        rsConfigureMSP.Close();
                                        return;
                                    }
                                }
                            }
                            else if (DB.RSField(rsConfigureMSP, "ParameterName") == "SerialNo")
                            {

                                if (txtTextBox != null && txtTextBox.Text != "")
                                {

                                    if (Convert.ToDecimal(DocQuantity) != 1)
                                    {
                                        //test case ID (TC_037)
                                        //Qty. should be one while serial no. capturing

                                        resetError("Quantity should be 1 for serial no., cannot receive", true);
                                        return;
                                    }
                                    if ((!(bool)ViewState["IsReeturned"]) && DB.GetSqlN("select GMDMSP.GoodsMovementDetails_MaterialStorageParameterID as N from INV_GoodsMovementDetails GMD JOIN INV_GoodsMovementDetails_MMT_MaterialStorageParameter GMDMSP ON GMD.GoodsMovementDetailsID=GMDMSP.GoodsMovementDetailsID AND GMDMSP.MaterialStorageParameterID=3 AND GMDMSP.IsDeleted=0 WHERE GMD.GoodsMovementTypeID=1 AND GMD.IsDeleted=0 and GMD.MaterialMasterID=" + CommonLogic.QueryString("mmid") + "  AND CONVERT(NVARCHAR,GMDMSP.Value)=" + DB.SQuote(txtTextBox.Text)) != 0)
                                    {
                                        //test case ID (TC_038)
                                        //Serial no. already exists, cannot receive

                                        resetError("Serial no. already exists, cannot receive", true);
                                        return;
                                    }
                                }
                            }
                            value = ((TextBox)tbMaterialStorageparameter.FindControl(ControlID)).Text.Trim();
                        }

                        HiddenValue = ((HiddenField)tbMaterialStorageparameter.FindControl("hif" + DB.RSField(rsConfigureMSP, "ParameterName"))).Value;
                        if (HiddenValue != "" && value != HiddenValue)
                        {
                            //resetError("Invalid "+DB.RSField(rsConfigureMSP, "ParameterName") , true);
                            // return;
                            MissMatchedMSPS += DB.RSField(rsConfigureMSP, "ParameterName") + ",";
                            CheckDiscrepancy = true;
                        }
                        if (value != "")
                        {

                            MSPIDs += DB.RSFieldInt(rsConfigureMSP, "MaterialStorageParameterID") + ",";
                            MSPValues += value + ",";
                        }
                    }
                    rsConfigureMSP.Close();
                    /*
                    sCmdUpsertPOQuantity.Append(",@MaterialStorageParameterIDs="+DB.SQuote(MSPIDs));
                    sCmdUpsertPOQuantity.Append(",@MaterialStorageParameterValues=" + DB.SQuote(MSPValues));
                     */

                    //List the mis matched material storage parameters
                    if (CheckDiscrepancy)
                    {
                        HasDiscrepancy = 1;
                        MissMatchedMSPS = MissMatchedMSPS.Substring(0, MissMatchedMSPS.Length - 1);


                        CamaPosition = MissMatchedMSPS.LastIndexOf(',');
                        if (CamaPosition != -1)
                        {
                            MissMatchedMSPS = MissMatchedMSPS.Remove(CamaPosition) + " and " + MissMatchedMSPS.Remove(0, CamaPosition + 1);
                        }

                    }

                    Decimal QuantitySum = DB.GetSqlNDecimal("sp_INV_GetGoodsInQuantitySum @MaterialMasterID=" + CommonLogic.QueryString("mmid") + ",@InboundID=" + CommonLogic.QueryString("ibdno") + ",@LineNumber=" + txtin_poitemline.Text.Trim() + ",@POHeaderID=" + hifPONumber.Value + ", @GoodsMovementDetailsID = 0" + ",@SupplierInvoiceID=" + hifInvoiceNumber.Value);
                    Decimal TotalQuantity = QuantitySum + Convert.ToDecimal(DocQuantity);

                    Decimal invoiceQuantity;        
                    if (ltInvqtyvalue.Text != "")
                    {
                        invoiceQuantity = Convert.ToDecimal(ltInvqtyvalue.Text);
                    }
                    else
                    {
                        invoiceQuantity = 0;
                    }
                    if (TotalQuantity > invoiceQuantity)
                    {
                        //test case ID (TC_032)
                        //Qty. received should be less than or equal to invoice qty.

                        resetError("Quantity received should be less than or equal to invoice qty.", true);
                        return;
                    }


                    if (chkIsPrintRequired.Checked)
                    {
                        if (ddlNetworkPrinter.SelectedValue == "0")
                        {
                            resetError("Please select printer", true);
                            return;
                        }

                        if (ddlLabelSize.SelectedValue == "0")
                        {
                            resetError("Please Select Label Size ", true);
                            return;
                        }
                    }

                    
                    string poh = "select POHeaderID from INB_Inbound_ORD_SupplierInvoice where InboundID="+inbno;

                    

                    DataSet DS = DB.GetDS(poh, false);
                    if (DS.Tables[0].Rows.Count != 0)
                    {
                        foreach (DataRow row in DS.Tables[0].Rows)
                        {
                            POHeaderID = row["POHeaderID"].ToString();
                        }
                    }

                    string supplierinvoiceid = "";
                    string poheader = "SELECT SupplierInvoiceID FROM ORD_SupplierInvoice WHERE POHeaderID=" + POHeaderID;
                    DataSet ds1 = DB.GetDS(poheader, false);
                    if (ds1.Tables[0].Rows.Count != 0)
                    {
                        foreach (DataRow row in ds1.Tables[0].Rows)
                        {
                            supplierinvoiceid = row["SupplierInvoiceID"].ToString();
                        }
                    }
                    string PoDetailsId = "";
                    string podetails = "SELECT PODetailsID FROM ORD_SupplierInvoiceDetails  WHERE SupplierInvoiceID="+supplierinvoiceid;
                    DataSet ds2 = DB.GetDS(podetails, false);
                    if (ds2.Tables[0].Rows.Count != 0)
                    {
                        foreach (DataRow row in ds2.Tables[0].Rows)
                        {
                            PoDetailsId = row["PODetailsID"].ToString();
                        }
                    }


                    

                    string Mfgdate = "";
                    string Expdate = "";
                    string serialno = "";
                    string batchNo = "";
                    string projrefno = "";


                    //string query = "select POHeaderID,FORMAT( MfgDate,'dd/MM/yyyy') AS MfgDate,FORMAT(ExpDate,'dd/MM/yyyy') AS ExpDate, SerialNo,BatchNo,ProjectRefNo from ORD_PODetails where PODetailsID=" + PoDetailsId + "AND IsActive=1 AND IsDeleted=0";
                    string query = "select FORMAT( MfgDate,'dd/MM/yyyy') AS MfgDate,FORMAT(ExpDate,'dd/MM/yyyy') AS ExpDate, SerialNo,BatchNo,ProjectRefNo from ORD_SupplierInvoiceDetails where PODetailsID=" + PoDetailsId + "AND IsActive=1 AND IsDeleted=0";
                    DataSet Ds = DB.GetDS(query, false);
                    if (Ds.Tables[0].Rows.Count != 0)
                    {
                        foreach (DataRow row in Ds.Tables[0].Rows)
                        {
                            Mfgdate = row["MfgDate"].ToString();
                            Expdate = row["ExpDate"].ToString();
                            //serialno = row["SerialNo"].ToString();
                            batchNo = row["BatchNo"].ToString();
                            projrefno = row["ProjectRefNo"].ToString();

                            if (Ds.Tables != null)
                            {
                                if (Mfgdate != null && Mfgdate!="")
                                { 
                                    if (txtMfgDate.Text.Trim() != Mfgdate)
                                    {
                                        resetError("Mfg.Date MisMatches Please Enter Correct Mfg. Date", true);
                                        txtMfgDate.Focus();
                                        return;
                                    }
                                }

                                if (Expdate != null && Expdate !="")
                                {
                                    if(txtExpDate.Text.Trim()!=Expdate)
                                    {
                                        resetError("Exp.Date MisMatches Please Enter Correct Exp. Date", true);
                                        txtExpDate.Focus();
                                        return;
                                    }
                                }

                                //if (serialno != null && serialno !="")
                                //{
                                //    if (txtSerialNo.Text.Trim() != serialno)
                                //    {
                                //        resetError("Serial No. MisMatches Please Enter Correct Serial No.", true);
                                //        txtSerialNo.Focus();
                                //        return;
                                //    }
                                //}
                                if (batchNo != null && batchNo !="")
                                {
                                    if (txtBatchNo.Text.Trim() != batchNo)
                                    {
                                        resetError("Batch No. MisMatches Please Enter Correct Batch No.", true);
                                        txtBatchNo.Focus();
                                        return;
                                    }
                                }
                                if (projrefno != null && projrefno !="")
                                {
                                    if (txtProjectRefNo.Text.Trim() != projrefno)
                                    {
                                        resetError("Project Ref.No. MisMatches Please Enter Correct Project Ref.No.", true);
                                        txtProjectRefNo.Focus();
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    

                    int Result = MRLWMSC21Common.InventoryCommonClass.UpdateGoodsMovementDetails("0", "1", Location, IUoM_QtyID, DocQuantity, Quantity, CommonLogic.QueryString("ibdno"), hifInvoiceNumber.Value, LineNumber, CommonLogic.QueryString("mmid"), IsDamaged.ToString(), HasDiscrepancy.ToString(), POHeaderID, cp.UserID.ToString(), MSPIDs, MSPValues, ViewState["ConvertionFactor"].ToString(), reMarkes, cp.UserID.ToString(), "0", ltKitplannerValue.Text, Convert.ToInt16(chkPositiveRecall.Checked).ToString(), "0", "0", hifContainercode.Value, ddlStorageLocationID.SelectedValue.ToString(),MfgDate,ExpDate,SerialNo,BatchNo,ProjectRefNo,SuggestPutawayID);
                 
                    // DB.ExecuteSQL(sCmdUpsertPOQuantity.ToString());
                    gvPOQuantityList.EditIndex = -1;
                    ViewState["gvPOQuantityList"] = "EXEC dbo.sp_INV_GetGoodsMovementDetailsList @MaterialMasterID=" + CommonLogic.QueryString("mmid") + ",@InboundID=" + CommonLogic.QueryString("ibdno") + ",@LineNumber=" + txtin_poitemline.Text.Trim() + ",@POHeaderID=" + hifPONumber.Value + ",@SupplierInvoiceID=" + hifInvoiceNumber.Value;
                    Build_gvPOQuantityilst(Build_gvPOQuantityilst());
                    LoadContainerMapping(LoadContainerMapping());
                    QuantitySum = DB.GetSqlNDecimal("sp_INV_GetGoodsInQuantitySum @MaterialMasterID=" + CommonLogic.QueryString("mmid") + ",@InboundID=" + CommonLogic.QueryString("ibdno") + ",@LineNumber=" + txtin_poitemline.Text.Trim() + ",@POHeaderID=" + hifPONumber.Value + ", @GoodsMovementDetailsID = 0" + ",@SupplierInvoiceID=" + hifInvoiceNumber.Value);
                    if (gvPOQuantityList.Rows.Count != 0)
                        ((Literal)gvPOQuantityList.FooterRow.FindControl("ltQuantityCount")).Text = "  Sum : " + QuantitySum;
                    if (Result == 1)
                    {
                        if (CheckDiscrepancy)
                        {
                            //test case Id (TC_041)
                            //MSPs are  mismatched case
                           // DB.ExecuteSQL("INSERT INTO INB_SuggestedPutaway_Receive(SuggestedPutawayID, SuggestedLocationID, SuggestedQty) VALUES(" + SuggestPutawayID + ", " + hifputwayloc.Value + ", " + DocQuantity + ")");
                            resetError("Successfully updated  but " + MissMatchedMSPS + (CamaPosition != -1 ? " are " : " is ") + "inappropriate", true);
                        }
                        else
                        {

                            resetError("Successfully Updated ", false);

                          //  DB.ExecuteSQL("INSERT INTO INB_SuggestedPutaway_Receive(SuggestedPutawayID, SuggestedLocationID, SuggestedQty) VALUES(" + SuggestPutawayID + ", " + hifputwayloc.Value + ", " + DocQuantity + ")");

                            txtreceiveqty.Text = "";
                            txtMfgDate.Text = "";
                            txtExpDate.Text = "";
                            txtBatchNo.Text = "";
                            txtSerialNo.Text = "";
                            txtProjectRefNo.Text = "";
                            txtContainerCode.Text = "";
                            txtRemarks.Text = "";

                            if (chkIsPrintRequired.Checked)
                            {
                                if (ddlNetworkPrinter.SelectedValue != "0")
                                    PrintLabel((chkIsDamaged.Checked == true ? txtin_MaterialCode.Text + "-D" : txtin_MaterialCode.Text), CommonLogic.QueryString("mmid"), ltDescriptionvalue.Text, StoreRefno, MSPIDs, MSPValues, DocQuantity, ddlNetworkPrinter.SelectedValue, MfgDate, ExpDate, serialno, BatchNo, projrefno, 0,txtPrintQty.Text);
                            }

                        }
                    }
                    else
                        resetError("Not updated", true);
                }
                catch (Exception ex)
                {
                    resetError("Error while updating", true);
                    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                }
            }
            if (gvPOQuantityList.Rows.Count != 0)
            {
                pnlGoodsIndata.Visible = true;
            }

            //if (gvPOQuantityList.Rows.Count != 0)
            //{
            //    if (txtreceiveqty.Text.Trim() == "")
            //    {
            //        resetError("Please Enter Receive Quantiy", true);
            //        txtreceiveqty.Focus();
            //        return;
            //    }

            //    if (txtContainerCode.Text.Trim() == "")
            //    {
            //        resetError("Please Select Container Code", true);
            //        txtContainerCode.Focus();
            //        return;
            //    }
            //    pnlGoodsIndata.Visible = true;
            //}

            //if (txtreceiveqty.Text.Trim() == "")
            //{
            //    resetError("Please Enter Receive Quantiy", true);
            //    txtreceiveqty.Focus();
            //    return;
            //}

            //if (txtContainerCode.Text.Trim() == "")
            //{
            //    resetError("Please Select Container Code", true);
            //    txtContainerCode.Focus();
            //    return;
            //}
        }

        public string GetDateTime(string intptime)
        {

            string opTime = "";

            string[] col = intptime.Split('/');

            opTime = col[1] + '/' + col[0] + '/' + col[2] ;

            return opTime;

        }

        #region ----Grid Data Functions----

        public Boolean GetBool(String IsChecked)
        {


            if (IsChecked != "")
            {
                if (Convert.ToBoolean(Convert.ToInt32(IsChecked)))
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }


        }

        public String Getimage(String IsChecked)
        {


            if (IsChecked != "")
            {
                if (IsChecked == "Yes")
                    //return "../Images/blue_menu_icons/check_mark.png";
                    return "<center> <img src=\"../Images/blue_menu_icons/check_mark.png\" ><cente/>";
                else
                    return null;
            }
            else
            {
                return null;
            }


        }
        #endregion

        #region----Add Dynamic MSPs----
        //Add dynamic MSP columns to grid
        private void AddDynamicColumns(String MMID)
        {

            TemplateField field;
            try
            {
                IDataReader rsMSPList = DB.GetRS("[sp_ORD_GetMaterialStorageParameters] @MaterialMasterID=" + MMID + ",@TenantID=" + cp.TenantID);

                while (rsMSPList.Read())
                {
                    field = new TemplateField();
                    field.HeaderText = DB.RSField(rsMSPList, "ParameterName");
                    field.HeaderTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Header, DB.RSField(rsMSPList, "DisplayName"), "Literal");
                    field.ItemStyle.Width = 150;
                    field.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item, DB.RSField(rsMSPList, "ParameterName"), "Literal");
                    Boolean requirevalidation = Convert.ToBoolean(DB.RSFieldTinyInt(rsMSPList, "IsRequired"));

                    field.EditItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.EditItem, DB.RSField(rsMSPList, "ParameterName"), DB.RSField(rsMSPList, "ControlType"), DB.RSField(rsMSPList, "ParameterDataType"), DB.RSField(rsMSPList, "DataSource"), DB.RSField(rsMSPList, "MinTolerance"), DB.RSField(rsMSPList, "MaxTolerance"), "gridrequirequantity", requirevalidation);

                    gvPOQuantityList.Columns.Add(field);


                }

                rsMSPList.Close();
                field = new TemplateField();
                field.HeaderTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Header, "QC Capture", "Literal");
                field.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item, "CaptureQc", "LinkButton");
                field.EditItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.EditItem, "", "Empty");
                gvPOQuantityList.Columns.Add(field);

                field = new TemplateField();
                field.HeaderTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Header, "Delete", "Literal");
                field.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item, "Delete", "CheckBox");
                field.EditItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.EditItem, "Delete", "Empty");
                button = new LinkButton();
                button.ID = "deleteButton";
                button.Text = "Delete";
                button.Font.Underline = false;
                button.ForeColor = System.Drawing.Color.Blue;
                button.OnClientClick = "return confirm('Are you sure want to delete the selected items?')";
                button.Click += new EventHandler(lnkDeleteItems_click);
                field.FooterTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Footer, button);


                gvPOQuantityList.Columns.Add(field);


                BoundField editfield;
                editfield = new BoundField();
                editfield.DataField = "EditName";
                editfield.ReadOnly = true;
                editfield.Visible = false;
                gvPOQuantityList.Columns.Add(editfield);

                CommandField cmdfield = new CommandField();

                cmdfield.ControlStyle.CssClass = "CammandField";
                cmdfield.CausesValidation = true;
                cmdfield.ButtonType = ButtonType.Link;
                cmdfield.CancelText = "Cancel";
                cmdfield.FooterStyle.Font.Underline = false;
                cmdfield.EditText = "<nobr> Edit <img src='../Images/redarrowright.gif' border='0' /></nobr>";
                cmdfield.ShowEditButton = true;
                cmdfield.ControlStyle.Font.Underline = false;

                cmdfield.ValidationGroup = "gridrequirequantity";
                cmdfield.UpdateImageUrl = "icons/update.gif";
                cmdfield.UpdateText = "Update";


                field = new TemplateField();
                field.HeaderTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Header, "Print", "Literal");
                field.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item, "Print", "LinkButton");
                field.EditItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.EditItem, "", "Empty");
                gvPOQuantityList.Columns.Add(field);


                // gvPOQuantityList.Columns.Add(cmdfield);
            }
            catch (Exception ex)
            {
                resetError("Error while add Material Storage Parameters", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }

        private void AddQcParametersControls(String outboundid, String MMID, String LineNumber)
        {
            tbQcParameters.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            try
            {
                IDataReader drMatrialMsp = DB.GetRS("dbo.sp_MFG_GetQcMaterialStorageParameterForMaterial @MaterialMasterID=" + MMID);

                int mspCount = 0;
                bool isRemarks = false;
                TextBox txtRemarks = null;
                RadioButtonList rblRadiosList = null;
                HtmlTableRow htRow = null;
                RequiredFieldValidator validator = null;
                CustomValidator Cvalidator = null;
                HtmlTableCell htCell;
                Panel pnlDecimalComponents;
                Panel pnlMinMaxValues;
                TextBox txtMinValue;
                TextBox txtMaxValue;
                CheckBox chkIsRanged;
                TextBox txttextbox;
                CheckBox chkCheckBox;
                Literal ltliteral;
                DropDownList ddlDropDownList;
                while (drMatrialMsp.Read())
                {
                    isRemarks = false;
                    pnlDecimalComponents = null;
                    txtMinValue = null;
                    txtMaxValue = null;
                    chkIsRanged = null;
                    pnlMinMaxValues = null;

                    htCell = new HtmlTableCell();
                    htCell.Align = "left";
                    htCell.Width = "54.5%";
                    ltliteral = new Literal();
                    ltliteral.ID = "ltqc" + DB.RSField(drMatrialMsp, "ParameterName");
                    ltliteral.Text = DB.RSField(drMatrialMsp, "ParameterName") + ":<br />";

                    String ControlType = DB.RSField(drMatrialMsp, "ControlType");
                    validator = new RequiredFieldValidator();
                    validator.ValidationGroup = "QcParametersGroup";
                    validator.ID = "rfvqc" + DB.RSField(drMatrialMsp, "ParameterName");
                    validator.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                    validator.Display = ValidatorDisplay.Dynamic;
                    validator.ErrorMessage = "*";
                    if (ControlType == "DropDownList")
                    {

                        ddlDropDownList = new DropDownList();

                        ddlDropDownList.ID = "ddlqc" + DB.RSField(drMatrialMsp, "ParameterName");
                        ddlDropDownList.Width = 120;
                        BuildDynamicDropDown(ddlDropDownList, DB.RSField(drMatrialMsp, "DataSource"), DB.RSField(drMatrialMsp, "ParameterName"));

                        validator.ControlToValidate = ddlDropDownList.ID;
                        if (DB.RSFieldTinyInt(drMatrialMsp, "IsRequired") == 1)
                            htCell.Controls.Add(validator);
                        htCell.Controls.Add(ltliteral);
                        htCell.Controls.Add(ddlDropDownList);
                    }
                    else if (ControlType == "TextBox")
                    {
                        if (DB.RSField(drMatrialMsp, "ParameterName") == "Remarks")
                        {
                            txtRemarks = new TextBox();
                            txtRemarks.ID = "txtqc" + DB.RSField(drMatrialMsp, "ParameterName");
                            txtRemarks.TextMode = TextBoxMode.MultiLine;
                            txtRemarks.Width = 300;
                            txtRemarks.Height = 80;
                            //mspCount--;
                            isRemarks = true;
                        }
                        else
                        {
                            txttextbox = new TextBox();
                            txttextbox.ID = "txtqc" + DB.RSField(drMatrialMsp, "ParameterName");
                            txttextbox.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                            txttextbox.Width = 120;
                            txttextbox.EnableTheming = false;
                            txttextbox.CssClass = DB.RSField(drMatrialMsp, "ParameterName");
                            if (DB.RSField(drMatrialMsp, "ParameterDataType") == "Nvarchar")
                            {
                                txttextbox.Attributes.Add("onKeypress", "return checkSpecialChar(event)");
                                txttextbox.Attributes.Add("onblur", "checknonConfirm(this)");
                            }
                            else if (DB.RSField(drMatrialMsp, "ParameterDataType") == "Integer")
                            {
                                txttextbox.Attributes.Add("onKeypress", "return checkNum(event)");
                                txttextbox.Attributes.Add("onblur", "checknonConfirm(this)");
                            }
                            else if (DB.RSField(drMatrialMsp, "ParameterDataType") == "Decimal")
                            {
                                ltliteral.Text = DB.RSField(drMatrialMsp, "ParameterName") + ":[" + DB.RSFieldDecimal(drMatrialMsp, "MinTolerance") + "-" + DB.RSFieldDecimal(drMatrialMsp, "MaxTolerance") + "]<br />";

                                Cvalidator = new CustomValidator();
                                Cvalidator.ID = "cfvqc" + DB.RSField(drMatrialMsp, "ParameterName");
                                Cvalidator.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                                Cvalidator.ValidationGroup = "QcParametersGroup";
                                Cvalidator.Display = ValidatorDisplay.Dynamic;
                                Cvalidator.ErrorMessage = "*";
                                Cvalidator.ForeColor = System.Drawing.Color.Red;
                                Cvalidator.ServerValidate += CValidator_ServerValidate;

                                txtMinValue = new TextBox();
                                txtMinValue.Width = 50;
                                txtMinValue.ID = "txtMin" + DB.RSField(drMatrialMsp, "ParameterName");
                                txtMinValue.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                                txtMinValue.Text = "MinValue";
                                //txtMinValue.Enabled = false;
                                txtMinValue.Attributes.Add("Name", "MinValue");
                                txtMinValue.Attributes.Add("onfocus", "ClearText(this,'MinValue');");
                                txtMinValue.Attributes.Add("onblur", "CheckClearText(this,'MinValue','div" + DB.RSField(drMatrialMsp, "ParameterName") + "');");
                                txtMinValue.Attributes.Add("onKeypress", "return checkDec(this,event);");

                                txtMaxValue = new TextBox();
                                txtMaxValue.Width = 52;
                                txtMaxValue.ID = "txtMax" + DB.RSField(drMatrialMsp, "ParameterName");
                                txtMaxValue.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                                txtMaxValue.Text = "MaxValue";
                                //txtMaxValue.Enabled = false;
                                //txtMaxValue.CssClass = "txt_Blue_Small";
                                txtMaxValue.Attributes.Add("Name", "MaxValue");
                                txtMaxValue.Attributes.Add("onfocus", "ClearText(this,'MaxValue');");
                                txtMaxValue.Attributes.Add("onblur", "CheckClearText(this,'MaxValue','div" + DB.RSField(drMatrialMsp, "ParameterName") + "');");
                                txtMaxValue.Attributes.Add("onKeypress", "return checkDec(this,event);");

                                chkIsRanged = new CheckBox();
                                chkIsRanged.Text = "Is Range";
                                chkIsRanged.ID = "chk" + DB.RSField(drMatrialMsp, "ParameterName");
                                chkIsRanged.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                                chkIsRanged.Attributes.Add("Name", "IsRanged");
                                chkIsRanged.Attributes.Add("onclick", "CheckDecimalTextBoxes(this,'div" + DB.RSField(drMatrialMsp, "ParameterName") + "');");

                                txttextbox.Attributes.Add("Name", "Value");
                                //txttextbox.Attributes.Add("onfocus", "ClearText(this,'div" + DB.RSField(drMatrialMsp, "ParameterName") + "')");
                                txttextbox.Attributes.Add("onKeypress", "return checkDec(this,event);");
                                txttextbox.Attributes.Add("onblur", "checknonConfirm(this);");
                                txttextbox.CssClass = "txt_Blue_Small";

                                pnlDecimalComponents = new Panel();
                                pnlDecimalComponents.CssClass = "QCParentdiv";
                                pnlDecimalComponents.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                                pnlMinMaxValues = new Panel();
                                pnlMinMaxValues.CssClass = "divMinMax";
                                //pnlMinMaxValues.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                                pnlDecimalComponents.ID = "div" + DB.RSField(drMatrialMsp, "ParameterName");
                                pnlDecimalComponents.Controls.Add(ltliteral);
                                pnlDecimalComponents.Controls.Add(chkIsRanged);
                                pnlDecimalComponents.Controls.Add(new LiteralControl("<br/><br/>"));
                                pnlMinMaxValues.Controls.Add(txtMinValue);
                                pnlMinMaxValues.Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;&nbsp;"));
                                pnlMinMaxValues.Controls.Add(txtMaxValue);
                                pnlDecimalComponents.Controls.Add(pnlMinMaxValues);
                                //pnlDecimalComponents.Controls.Add(new LiteralControl("<br/>"));
                                pnlDecimalComponents.Controls.Add(txttextbox);
                            }
                            else if (DB.RSField(drMatrialMsp, "ParameterDataType") == "DateTime")
                            {
                                txttextbox.CssClass = "DateBoxCSS_small";
                                txttextbox.Attributes.Add("onblur", "checknonConfirm(this)");
                            }


                            if (DB.RSField(drMatrialMsp, "ParameterDataType") == "Decimal")
                            {

                                if (DB.RSFieldTinyInt(drMatrialMsp, "IsRequired") == 1)
                                    htCell.Controls.Add(Cvalidator);
                                htCell.Controls.Add(pnlDecimalComponents);
                                //htCell.Controls.Add(txttextbox);
                            }
                            else
                            {
                                validator.ControlToValidate = txttextbox.ID;
                                if (DB.RSFieldTinyInt(drMatrialMsp, "IsRequired") == 1)
                                    htCell.Controls.Add(validator);
                                htCell.Controls.Add(ltliteral);
                                htCell.Controls.Add(txttextbox);
                            }
                        }
                    }
                    else if (ControlType == "CheckBox")
                    {
                        chkCheckBox = new CheckBox();
                        chkCheckBox.ID = "";
                    }
                    else if (ControlType == "RadioBox")
                    {
                        //ltliteral.Text = DB.RSField(drMatrialMsp, "ParameterName");
                        rblRadiosList = new RadioButtonList();
                        rblRadiosList.RepeatDirection = RepeatDirection.Horizontal;
                        rblRadiosList.RepeatLayout = RepeatLayout.Flow;
                        rblRadiosList.ID = "rblqc" + DB.RSField(drMatrialMsp, "ParameterName");
                        rblRadiosList.Items.Add(new ListItem("Yes", "1"));
                        rblRadiosList.Items.Add(new ListItem("No", "0"));

                        validator.ControlToValidate = rblRadiosList.ID;
                        if (DB.RSFieldTinyInt(drMatrialMsp, "IsRequired") == 1)
                            htCell.Controls.Add(validator);
                        htCell.Controls.Add(ltliteral);
                        htCell.Controls.Add(rblRadiosList);
                    }


                    //if (mspCount++ % 2 == 0)
                    if (mspCount++ % 2 == 0)
                    {
                        htRow = new HtmlTableRow();

                        if (!isRemarks)
                        {
                            htRow.Cells.Add(htCell);
                        }
                        //tbMaterialStorageparameter.Rows.Add(htRow);
                    }
                    else
                    {
                        if (!isRemarks)
                        {
                            htRow.Cells.Add(htCell);
                            tbQcParameters.Rows.Add(htRow);
                            htRow = new HtmlTableRow();
                            htCell = new HtmlTableCell();
                            htCell.Controls.Add(new LiteralControl("&nbsp;"));
                            htRow.Cells.Add(htCell);
                            tbQcParameters.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                            tbQcParameters.Rows.Add(htRow);
                        }
                        else
                        {
                            mspCount--;
                        }
                    }


                }

                if (mspCount % 2 == 1)
                {
                    tbQcParameters.Rows.Add(htRow);
                }
                drMatrialMsp.Close();

                if (txtRemarks != null)
                {
                    htRow = new HtmlTableRow();
                    htCell = new HtmlTableCell();
                    htCell.ColSpan = 2;
                    htCell.Controls.Add(new LiteralControl("Remarks:<br/>"));
                    htCell.Controls.Add(txtRemarks);
                    htRow.Controls.Add(htCell);
                    tbQcParameters.Controls.Add(htRow);
                }

                htRow = new HtmlTableRow();
                htCell = new HtmlTableCell();
                chkCheckBox = new CheckBox();
                chkCheckBox.ID = "chkqcIsnonconformity";
                chkCheckBox.Attributes.Add("onclick", "CheckNonconformity(this);");
                //chkCheckBox.Enabled = false;
                chkCheckBox.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                chkCheckBox.Text = "Is Non-Conformity";
                htCell.Controls.Add(chkCheckBox);
                htRow.Controls.Add(htCell);
                tbQcParameters.Controls.Add(htRow);


                //tbMaterialStorageparameter.Rows.Add(new HtmlTableRow());

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                //resetError(ex.ToString(), true);
            }
        }

        //Adddynamic textBoxes of MSP
        private void AddMspTextBoxs(String outboundid, String MMID, String LineNumber)
        {
            try
            {
                IDataReader drMatrialMsp = DB.GetRS("[sp_ORD_GetMaterialStorageParameters] @MaterialMasterID=" + MMID + ",@TenantID=" + cp.TenantID);

                int mspCount = 0;

                HtmlTableRow htRow = null;
                HtmlTableCell htCell;
                TextBox txttextbox;
                Literal ltliteral;
                DropDownList ddlDropDownList;
                while (drMatrialMsp.Read())
                {


                    if (mspCount++ % 2 == 0)
                    {
                        htRow = new HtmlTableRow();
                        htCell = new HtmlTableCell();
                        htCell.Align = "left";
                        htCell.Width = "54.5%";
                        ltliteral = new Literal();
                        ltliteral.ID = "lt" + DB.RSField(drMatrialMsp, "ParameterName");
                        ltliteral.Text = DB.RSField(drMatrialMsp, "DisplayName") + ":<br />";

                        String ControlType = DB.RSField(drMatrialMsp, "ControlType");
                        HiddenField hifHiddenfield = new HiddenField();
                        hifHiddenfield.ID = "hif" + DB.RSField(drMatrialMsp, "ParameterName");
                        htCell.Controls.Add(hifHiddenfield);


                        RequiredFieldValidator validator = new RequiredFieldValidator();
                        validator.ValidationGroup = "receiveQuantity";
                        validator.ID = "rfv" + DB.RSField(drMatrialMsp, "ParameterName");
                        validator.Display = ValidatorDisplay.Dynamic;
                        validator.ErrorMessage = "*";
                        if (ControlType == "DropDownList")
                        {

                            ddlDropDownList = new DropDownList();

                            ddlDropDownList.ID = "ddl" + DB.RSField(drMatrialMsp, "ParameterName");
                            ddlDropDownList.Width = 160;
                            BuildDynamicDropDown(ddlDropDownList, DB.RSField(drMatrialMsp, "DataSource"), DB.RSField(drMatrialMsp, "ParameterName"));

                            validator.ControlToValidate = ddlDropDownList.ID;
                            if (DB.RSFieldTinyInt(drMatrialMsp, "IsRequired") == 1)
                                htCell.Controls.Add(validator);
                            htCell.Controls.Add(ltliteral);
                            htCell.Controls.Add(ddlDropDownList);
                        }
                        else if (ControlType == "TextBox")
                        {

                            txttextbox = new TextBox();
                            txttextbox.ID = "txt" + DB.RSField(drMatrialMsp, "ParameterName");
                            txttextbox.Width = 160;
                            txttextbox.EnableTheming = false;
                            txttextbox.CssClass = "txt_Blue_Small";
                            if (DB.RSField(drMatrialMsp, "ParameterDataType") == "Nvarchar")
                                txttextbox.Attributes.Add("onKeypress", "return checkSpecialChar(event)");
                            else if (DB.RSField(drMatrialMsp, "ParameterDataType") == "Integer")
                                txttextbox.Attributes.Add("onKeypress", "return checkNum(event)");
                            else if (DB.RSField(drMatrialMsp, "ParameterDataType") == "Decimal")
                                txttextbox.Attributes.Add("onKeypress", "return checkDec(event)");
                            else if (DB.RSField(drMatrialMsp, "ParameterDataType") == "DateTime")
                                txttextbox.CssClass = "DateBoxCSS_small";
                            validator.ControlToValidate = txttextbox.ID;
                            if (DB.RSFieldTinyInt(drMatrialMsp, "IsRequired") == 1)
                                htCell.Controls.Add(validator);
                            htCell.Controls.Add(ltliteral);
                            htCell.Controls.Add(txttextbox);
                        }

                        htRow.Cells.Add(htCell);
                        //tbMaterialStorageparameter.Rows.Add(htRow);
                    }
                    else
                    {
                        //htRow = new HtmlTableRow();
                        htCell = new HtmlTableCell();
                        htCell.Align = "left";
                        htCell.Width = "100%";
                        // htCell.Style.Add("padding-left", "140px");
                        txttextbox = new TextBox();
                        ltliteral = new Literal();
                        ltliteral.ID = "lt" + DB.RSField(drMatrialMsp, "ParameterName");
                        ltliteral.Text = DB.RSField(drMatrialMsp, "DisplayName") + ":<br />";

                        String ControlType = DB.RSField(drMatrialMsp, "ControlType");
                        HiddenField hifHiddenfield = new HiddenField();
                        hifHiddenfield.ID = "hif" + DB.RSField(drMatrialMsp, "ParameterName");
                        htCell.Controls.Add(hifHiddenfield);
                        RequiredFieldValidator validator = new RequiredFieldValidator();
                        validator.ValidationGroup = "receiveQuantity";
                        validator.ID = "rfv" + DB.RSField(drMatrialMsp, "ParameterName");
                        validator.Display = ValidatorDisplay.Dynamic;
                        validator.ErrorMessage = "*";
                        if (ControlType == "DropDownList")
                        {
                            ddlDropDownList = new DropDownList();
                            ddlDropDownList.ID = "ddl" + DB.RSField(drMatrialMsp, "ParameterName");
                            ddlDropDownList.Width = 160;
                            BuildDynamicDropDown(ddlDropDownList, DB.RSField(drMatrialMsp, "DataSource"), DB.RSField(drMatrialMsp, "ParameterName"));
                            validator.ControlToValidate = ddlDropDownList.ID;
                            if (DB.RSFieldTinyInt(drMatrialMsp, "IsRequired") == 1)
                                htCell.Controls.Add(validator);
                            htCell.Controls.Add(ltliteral);

                            htCell.Controls.Add(ddlDropDownList);
                        }
                        else if (ControlType == "TextBox")
                        {
                            txttextbox = new TextBox();
                            txttextbox.ID = "txt" + DB.RSField(drMatrialMsp, "ParameterName");
                            txttextbox.Width = 160;
                            txttextbox.EnableTheming = false;
                            txttextbox.CssClass = "txt_Blue_Small";
                            if (DB.RSField(drMatrialMsp, "ParameterDataType") == "Nvarchar")
                                txttextbox.Attributes.Add("onKeypress", "return checkSpecialChar(event)");
                            else if (DB.RSField(drMatrialMsp, "ParameterDataType") == "Integer")
                                txttextbox.Attributes.Add("onKeypress", "return checkNum(event)");
                            else if (DB.RSField(drMatrialMsp, "ParameterDataType") == "Decimal")
                                txttextbox.Attributes.Add("onKeypress", "return checkDec(event)");
                            else if (DB.RSField(drMatrialMsp, "ParameterDataType") == "DateTime")
                                txttextbox.CssClass = "DateBoxCSS_small";
                            validator.ControlToValidate = txttextbox.ID;
                            if (DB.RSFieldTinyInt(drMatrialMsp, "IsRequired") == 1)
                                htCell.Controls.Add(validator);
                            htCell.Controls.Add(ltliteral);
                            htCell.Controls.Add(txttextbox);
                        }

                        htRow.Cells.Add(htCell);
                        tbMaterialStorageparameter.Rows.Add(htRow);
                        //htRow = new HtmlTableRow();
                        // htCell = new HtmlTableCell();
                        // htCell.Controls.Add(new LiteralControl("&nbsp;"));
                        // htRow.Cells.Add(htCell);
                        // tbMaterialStorageparameter.Rows.Add(htRow);
                    }


                }
                if (mspCount % 2 != 0)
                {
                    tbMaterialStorageparameter.Rows.Add(htRow);
                }
                drMatrialMsp.Close();
                //tbMaterialStorageparameter.Rows.Add(new HtmlTableRow());
            }
            catch (Exception ex)
            {
                resetError("Error while build Material Storage Parameters", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }

        #endregion

        #region----Grid Functions----

        //delete gridrows 
        private void lnkDeleteItems_click(object sender, EventArgs e)
        {

            //test case Id (TC_045)
            //delete selected line item

            string gvchkIDs = "";


            foreach (GridViewRow gvRow in gvPOQuantityList.Rows)
            {
                if (gvRow.RowState != DataControlRowState.Edit)
                {
                    CheckBox isDeletedchkbxItem = (CheckBox)gvRow.FindControl("Delete");

                    if (isDeletedchkbxItem.Checked)
                    {


                        gvchkIDs += ((Literal)gvRow.FindControl("ltGoodsMovementDetailsID")).Text.ToString() + ",";
                    }
                }

            }
            try
            {
                int InboundStatus = DB.GetSqlN("select InboundStatusID as N from INB_Inbound where StoreRefNo ='" + txtin_Storefno.Text.Trim() + "'");
                if (InboundStatus >= 6)
                {
                    resetError("Shipment is verified, cannot receive", true);
                    return;
                }
                int CycleCountOn = DB.GetSqlN("select qcc.IsOn as N from QCC_CycleCount qcc left join QCC_CycleCountDetails qccd on qccd.CycleCountID=qcc.CycleCountID  where qccd.MaterialMasterID=" + CommonLogic.QueryString("mmid"));
                if (CycleCountOn == 1)
                {
                    resetError("This material is in 'Cycle Count', cannot delete ", true);
                    return;
                }

                String sCmdDeleteRows = "EXEC dbo.sp_INV_DeleteGoodsMovementDetails @GoodsMovementDetailsIDs=" + DB.SQuote(gvchkIDs)+ ",@UpdatedBy="+cp.UserID.ToString();
                DB.ExecuteSQL(sCmdDeleteRows);
                gvPOQuantityList.EditIndex = -1;
                ViewState["gvPOQuantityList"] = "EXEC dbo.sp_INV_GetGoodsMovementDetailsList @MaterialMasterID=" + CommonLogic.QueryString("mmid") + ",@InboundID=" + CommonLogic.QueryString("ibdno") + ",@LineNumber=" + txtin_poitemline.Text.Trim() + ",@POHeaderID=" + hifPONumber.Value + ",@SupplierInvoiceID=" + hifInvoiceNumber.Value;
                Build_gvPOQuantityilst(Build_gvPOQuantityilst());
                Decimal QuantitySum = DB.GetSqlNDecimal("sp_INV_GetGoodsInQuantitySum @MaterialMasterID=" + CommonLogic.QueryString("mmid") + ",@InboundID=" + CommonLogic.QueryString("ibdno") + ",@LineNumber=" + txtin_poitemline.Text.Trim() + ",@POHeaderID=" + hifPONumber.Value + ", @GoodsMovementDetailsID = 0" + ",@SupplierInvoiceID=" + hifInvoiceNumber.Value);
                if (gvPOQuantityList.Rows.Count != 0)
                {
                    ((Literal)gvPOQuantityList.FooterRow.FindControl("ltQuantityCount")).Text = "  Sum : " + QuantitySum;
                    pnlGoodsIndata.Visible = true;
                }
                
                resetError("Successfully deleted the selected items", false);
            }
            catch (Exception ex)
            {
                resetError("Error while deleting items", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }



        }

        //cancel editing
        protected void gvPOQuantityList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvPOQuantityList.EditIndex = -1;
                ViewState["gvPOQuantityList"] = "EXEC dbo.sp_INV_GetGoodsMovementDetailsList @MaterialMasterID=" + CommonLogic.QueryString("mmid") + ",@InboundID=" + CommonLogic.QueryString("ibdno") + ",@LineNumber=" + txtin_poitemline.Text.Trim() + ",@POHeaderID=" + hifPONumber.Value;
                Build_gvPOQuantityilst(Build_gvPOQuantityilst());
                //ltInventoryStatus.Text = "";
                Decimal QuantitySum = DB.GetSqlNDecimal("sp_INV_GetGoodsInQuantitySum @MaterialMasterID=" + CommonLogic.QueryString("mmid") + ",@InboundID=" + CommonLogic.QueryString("ibdno") + ",@LineNumber=" + txtin_poitemline.Text.Trim() + ",@POHeaderID=" + hifPONumber.Value + ", @GoodsMovementDetailsID = 0");
                if (gvPOQuantityList.Rows.Count != 0)
                    ((Literal)gvPOQuantityList.FooterRow.FindControl("ltQuantityCount")).Text = "  Sum : " + QuantitySum;
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                //resetError(ex.ToString(),true);
            }

        }

        //set edit mode to particular row
        protected void gvPOQuantityList_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvPOQuantityList.EditIndex = e.NewEditIndex;

            ViewState["gvPOQuantityList"] = "EXEC dbo.sp_INV_GetGoodsMovementDetailsList @MaterialMasterID=" + CommonLogic.QueryString("mmid") + ",@InboundID=" + CommonLogic.QueryString("ibdno") + ",@LineNumber=" + txtin_poitemline.Text.Trim() + ",@POHeaderID=" + hifPONumber.Value;
            Build_gvPOQuantityilst(Build_gvPOQuantityilst());

        }

        //Updating changes of grid
        protected void gvPOQuantityList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Page.Validate("gridrequirequantity");
            GridViewRow gvrEditRow = gvPOQuantityList.Rows[e.RowIndex];
            if (gvrEditRow != null)
            {

                String GoodsMovementDetailsID = ((Literal)gvrEditRow.FindControl("ltGoodsMovementDetailsID_Edit")).Text.Trim();
                String Location = ((TextBox)gvrEditRow.FindControl("txtLocation")).Text.Trim();
                String DocQuantity = ((TextBox)gvrEditRow.FindControl("txtQuantity")).Text.Trim();
                String IUoM_QtyID = hifInvUoMQty.Value;
                String Quantity = (Convert.ToDecimal(DocQuantity) * Convert.ToDecimal(ltreceiveqtyperuomValue.Text)).ToString();
                String IsDamaged = Convert.ToInt16(((CheckBox)gvrEditRow.FindControl("chkDamaged")).Checked).ToString();
                String StoreRefno = txtin_Storefno.Text.Trim();
                String LineNumber = txtin_poitemline.Text.Trim();
                String MCode = txtin_MaterialCode.Text.Trim();
                String POHeaderID = hifPONumber.Value;
                String StorageparameterIDs = "";
                String StorageparameterValues = "";
                string MfgDate = txtMfgDate.Text.Trim();
                string ExpDate = txtExpDate.Text.Trim();
                string SerialNo = txtSerialNo.Text.Trim();
                string BatchNo = txtBatchNo.Text.Trim();
                string ProjectRefNo = txtProjectRefNo.Text.Trim();

                /* StringBuilder sCmdUpsertPOQuantity = new StringBuilder();
                 sCmdUpsertPOQuantity.Append("sp_INV_UpsertGoodsMovementDetails ");
                 sCmdUpsertPOQuantity.Append("@GoodsMovementDetailsID=" + GoodsMovementDetailsID);
                 sCmdUpsertPOQuantity.Append(",@GoodsMovementTypeID=1");
                 sCmdUpsertPOQuantity.Append(",@Location=" + DB.SQuote(Location));
                 sCmdUpsertPOQuantity.Append(",@Quantity=" + Quantity);
                 sCmdUpsertPOQuantity.Append(",@StoreRefNo=" + DB.SQuote(StoreRefno));
                 sCmdUpsertPOQuantity.Append(",@LineNumber=" + LineNumber);
                 sCmdUpsertPOQuantity.Append(",@MCode=" + DB.SQuote(MCode));
                 sCmdUpsertPOQuantity.Append(",@IsDamaged=" + IsDamaged);
                 sCmdUpsertPOQuantity.Append(",@POHeaderID=" + POHeaderID);
                 sCmdUpsertPOQuantity.Append(",@CreatedBy=1");
                 */


                try
                {

                    int CycleCountOn = DB.GetSqlN("select convert(int,qcc.IsOn) as N from QCC_CycleCount qcc left join QCC_CycleCountDetails qccd on qccd.CycleCountID=qcc.CycleCountID  where qccd.MaterialMasterID=" + CommonLogic.QueryString("mmid"));
                    if (CycleCountOn == 1)
                    {
                        resetError("This material is in 'Cycle Count', cannot receive ", true);
                        return;
                    }
                    int InboundStatus = DB.GetSqlN("select InboundStatusID as N from INB_Inbound where StoreRefNo ='" + StoreRefno + "'");
                    if (InboundStatus >= 7)
                    {
                        resetError("Shipment is verified, cannot receive ", true);
                        return;
                    }
                    int Locationid = DB.GetSqlN("SELECT LocationID AS N FROM INV_Location LOC JOIN INV_LocationZone WHZ ON WHZ.LocationZoneCode=LEFT(LOC.Location,2) AND WHZ.IsActive=1 AND WHZ.IsDeleted=0 JOIN INB_RefWarehouse_Details RFD ON RFD.WarehouseID=WHZ.WarehouseID AND RFD.IsActive=1 AND RFD.IsDeleted=0 WHERE RFD.InboundID=" + CommonLogic.QueryString("ibdno") + " AND Location=" + DB.SQuote(Location));
                    if (Locationid == 0)
                    {
                        resetError("Not a valid location", true);
                        return;
                    }


                    Decimal Requirequantity = Convert.ToDecimal(DocQuantity);
                    if (Requirequantity == 0)
                    {
                        resetError("Quantity should always be greater than zero", true);
                        return;
                    }
                    Decimal QuantitySum = DB.GetSqlNDecimal("sp_INV_GetGoodsInQuantitySum @MaterialMasterID=" + CommonLogic.QueryString("mmid") + ",@InboundID=" + CommonLogic.QueryString("ibdno") + ",@LineNumber=" + txtin_poitemline.Text.Trim() + ",@POHeaderID=" + hifPONumber.Value + ", @GoodsMovementDetailsID = " + GoodsMovementDetailsID);
                    Decimal TotalQuantity = QuantitySum + Requirequantity;




                    Decimal invoiceQuantity;
                    if (ltInvqtyvalue.Text != "")
                    {
                        invoiceQuantity = Convert.ToDecimal(ltInvqtyvalue.Text);
                    }
                    else
                    {
                        invoiceQuantity = 0;
                    }
                    if (TotalQuantity > invoiceQuantity)
                    {
                        resetError("Receive quantity is more than the invoice quantity", true);
                        return;
                    }


                    IDataReader drMatrialMsp = DB.GetRS("[sp_ORD_GetMaterialStorageParameters] @MaterialMasterID=" + CommonLogic.QueryString("mmid") + ",@TenantID=" + cp.TenantID);
                    String MSPIDs = "";
                    String MSPValues = "";
                    String HiddenValue = "";
                    while (drMatrialMsp.Read())
                    {
                        String ControlType = DB.RSField(drMatrialMsp, "ControlType");
                        String ControlID = "";
                        String value = "";
                        if (ControlType == "DropDownList")
                        {
                            ControlID = "ddl" + DB.RSField(drMatrialMsp, "ParameterName");
                            DropDownList ddlDropdown = ((DropDownList)gvrEditRow.FindControl(ControlID));
                            if (ddlDropdown.Visible)
                                value = ddlDropdown.SelectedValue;

                        }
                        else if (ControlType == "TextBox")
                        {
                            ControlID = "txt" + DB.RSField(drMatrialMsp, "ParameterName");
                            TextBox txtTextBox = ((TextBox)gvrEditRow.FindControl(ControlID));

                            if (DB.RSField(drMatrialMsp, "ParameterName") == "ExpDate")
                            {

                                if (txtTextBox != null && txtTextBox.Text != "")
                                {
                                    DateTime expiredate = DateTime.ParseExact(txtTextBox.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    int result = DateTime.Compare(expiredate, DateTime.Now);
                                    if (result < 0)
                                    {
                                        resetError("Cannot receive expired items", true);
                                        drMatrialMsp.Close();
                                        return;
                                    }
                                }

                            }
                            else if (DB.RSField(drMatrialMsp, "ParameterName") == "MfgDate")
                            {

                                if (txtTextBox != null && txtTextBox.Text != "")
                                {
                                    DateTime expiredate = DateTime.ParseExact(txtTextBox.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    int result = DateTime.Compare(expiredate, DateTime.Now);
                                    if (result > 0)
                                    {
                                        resetError("MfgDate not valid cannot receive", true);
                                        drMatrialMsp.Close();
                                        return;
                                    }
                                }
                            }
                            else if (DB.RSField(drMatrialMsp, "ParameterName") == "SerialNo")
                            {

                                if (txtTextBox != null && txtTextBox.Text != "")
                                {

                                    if (Convert.ToDecimal(DocQuantity) > 1)
                                    {
                                        resetError("Quantity cannot be exceed 1 when Serial No. is captured at Goods IN", true);
                                        drMatrialMsp.Close();
                                        return;
                                    }
                                }
                            }

                            if (txtTextBox.Visible)
                                value = txtTextBox.Text.Trim();
                        }
                        HiddenValue = ((HiddenField)tbMaterialStorageparameter.FindControl("hif" + DB.RSField(drMatrialMsp, "ParameterName"))).Value;
                        if (HiddenValue != "" && value != HiddenValue)
                        {
                            resetError("Invalid " + DB.RSField(drMatrialMsp, "ParameterName"), true);
                            drMatrialMsp.Close();
                            return;
                        }
                        if (value != "")
                        {
                            MSPIDs += DB.RSFieldInt(drMatrialMsp, "MaterialStorageParameterID") + ",";
                            MSPValues += value + ",";
                        }
                    }
                    drMatrialMsp.Close();
                    // sCmdUpsertPOQuantity.Append(",@MaterialStorageParameterIDs=" + DB.SQuote(MSPIDs));
                    // sCmdUpsertPOQuantity.Append(",@MaterialStorageParameterValues=" + DB.SQuote(MSPValues));


                    int Result = InventoryCommonClass.UpdateGoodsMovementDetails(GoodsMovementDetailsID, "1", Location != "" ? Location : "NULL", IUoM_QtyID, DocQuantity, Quantity != "" ? Quantity : "NULL", CommonLogic.QueryString("ibdno"), hifInvoiceNumber.Value, LineNumber, CommonLogic.QueryString("mmid"), IsDamaged.ToString(), "0", POHeaderID, "1", MSPIDs, MSPValues, "", "", cp.UserID.ToString(), "0", ltKitplannerValue.Text, Convert.ToInt16(chkPositiveRecall.Checked).ToString(), "0", "0", MfgDate, ExpDate, SerialNo, BatchNo, ProjectRefNo);
                   

                    // DB.ExecuteSQL(sCmdUpsertPOQuantity.ToString());
                    gvPOQuantityList.EditIndex = -1;
                    ViewState["gvPOQuantityList"] = "EXEC dbo.sp_INV_GetGoodsMovementDetailsList @MaterialMasterID=" + CommonLogic.QueryString("mmid") + ",@InboundID=" + CommonLogic.QueryString("ibdno") + ",@LineNumber=" + txtin_poitemline.Text.Trim() + ",@POHeaderID=" + hifPONumber.Value;
                    Build_gvPOQuantityilst(Build_gvPOQuantityilst());

                    QuantitySum = DB.GetSqlNDecimal("sp_INV_GetGoodsInQuantitySum @MaterialMasterID=" + CommonLogic.QueryString("mmid") + ",@InboundID=" + CommonLogic.QueryString("ibdno") + ",@LineNumber=" + txtin_poitemline.Text.Trim() + ",@POHeaderID=" + hifPONumber.Value + ", @GoodsMovementDetailsID = 0");
                    if (gvPOQuantityList.Rows.Count != 0)
                        ((Literal)gvPOQuantityList.FooterRow.FindControl("ltQuantityCount")).Text = "  Sum : " + QuantitySum;

                    if (Result == 1)
                        resetError("Successfully  Received", false);
                    else
                        resetError("Not Received", true);
                }
                catch (Exception ex)
                {
                    resetError("Error while Received", true);
                    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                }


            }


        }

        //build gridview
        private void Build_gvPOQuantityilst(DataSet dsPOQuantityList)
        {
            gvPOQuantityList.DataSource = dsPOQuantityList.Tables[0];
            gvPOQuantityList.DataBind();
            //dsPOQuantityList.Tables[0].Compute("sum(DocQty)", "").ToString();
            dsPOQuantityList.Dispose();
        }

        protected void gvPOQuantityList_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            try
            {
                //int index = e.NewSelectedIndex;
                //ViewState["index"] = index;

            }
            catch (Exception ex) {
                resetError("Error while change index", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }

        }

        protected void gvPOQuantityList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvPOQuantityList.PageIndex = e.NewPageIndex;
                gvPOQuantityList.EditIndex = -1;
                ViewState["gvPOQuantityList"] = "EXEC dbo.sp_INV_GetGoodsMovementDetailsList @MaterialMasterID=" + CommonLogic.QueryString("mmid") + ",@InboundID=" + CommonLogic.QueryString("ibdno") + ",@LineNumber=" + txtin_poitemline.Text.Trim() + ",@POHeaderID=" + hifPONumber.Value;
                Build_gvPOQuantityilst(Build_gvPOQuantityilst());
                //ltInventoryStatus.Text = "";
                Decimal QuantitySum = DB.GetSqlNDecimal("sp_INV_GetGoodsInQuantitySum @MaterialMasterID=" + CommonLogic.QueryString("mmid") + ",@InboundID=" + CommonLogic.QueryString("ibdno") + ",@LineNumber=" + txtin_poitemline.Text.Trim() + ",@POHeaderID=" + hifPONumber.Value + ", @GoodsMovementDetailsID =0 ");
                if (gvPOQuantityList.Rows.Count != 0)
                    ((Literal)gvPOQuantityList.FooterRow.FindControl("ltQuantityCount")).Text = "  Sum : " + QuantitySum;
            }
            catch (Exception ex)
            {
                resetError("Error while change pageIndex", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }

        protected void gvPOQuantityList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row == null)
            {
                return;
            }
            //if (e.Row.RowType == DataControlRowType.Header)
            //{
            //    Control fff= e.Row.Controls[e.Row.Controls.Count - 1];
            //    CheckBox chkbox=new CheckBox();
            //    chkbox.CssClass="HeadDelChk";
            //    fff.Controls.Add(chkbox);
            //}

            
            


            if ((e.Row.RowType == DataControlRowType.DataRow) && !(e.Row.RowState == DataControlRowState.Edit))
            {
                DataRow row = ((DataRowView)e.Row.DataItem).Row;
                String QcCapture = row["CapturingQc"].ToString();
                LinkButton lnkCaptureQc = ((LinkButton)e.Row.FindControl("lnkCaptureQc"));
                if (lnkCaptureQc != null)
                {

                    if (QcCapture == "1")
                    {
                        lnkCaptureQc.Text = " QC Captured ";
                        //lnkCaptureQc.CommandArgument = row["GoodsMovementDetailsID"].ToString();
                        lnkCaptureQc.ForeColor = System.Drawing.Color.Green;
                    }
                    else if (QcCapture == "0")
                    {
                        lnkCaptureQc.Text = "QC Pending ";
                        lnkCaptureQc.Enabled = true;
                        lnkCaptureQc.ForeColor = System.Drawing.Color.Red;
                        // lnkCaptureQc.CommandArgument = row["GoodsMovementDetailsID"].ToString();
                    }
                    else
                    {
                        lnkCaptureQc.Text = "QC Not Required";
                        lnkCaptureQc.Enabled = false;
                        lnkCaptureQc.ForeColor = System.Drawing.Color.Black;
                    }
                    lnkCaptureQc.CommandArgument = row["GoodsMovementDetailsID"].ToString() + ',' + row["DocQty"].ToString() + ',' + row["IsDamaged"].ToString();
                    lnkCaptureQc.CommandName = "CaptureQc";
                }
                //lnkCaptureQc.OnClientClick = "openQCParametersDialog();";

                int count = gvPOQuantityList.Columns.Count;
                if ((int)ViewState["InboundStatus"] >= 6)
                {
                    e.Row.Cells[count - 2].Enabled = false;
                    e.Row.Cells[count - 3].Enabled = false;
                    e.Row.Cells[count - 4].Enabled = false;
                }

            }
                       

            LinkButton lnk = (LinkButton)e.Row.FindControl("lnkPrint");

            if (lnk == null)
                return;
            lnk.Text = "Print";

            lnk.CommandName = "Print";
            lnk.CommandArgument = e.Row.RowIndex.ToString();
            

            lnk.OnClientClick = "openPrintDialog();return true;";


        }

        protected void gvPOQuantityList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CaptureQc")
            {
                lblDailogStatus.Text = "";
                String[] CommandArguments = e.CommandArgument.ToString().Split(',');
                hifGoodsMovementID.Value = CommandArguments[0];
                hifIsDamaged.Value = CommandArguments[2];
                Build_DialogHeader(CommandArguments[0].ToString(), true);
                txtQcQuantity.Text = "";
                Build_QCParameters("");
                ViewState["autoOpen"] = "true";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "openQCParametersDialog(); ShowDefaultTextBoxes();", true);
            }

            if (e.CommandName == "Print")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                ViewState["index"] = index;

            }
        }

        //get dataset for grid
        private DataSet Build_gvPOQuantityilst()
        {
            DataSet dsPOQuantity = null;
            String sCmdPOQuantitylist = ViewState["gvPOQuantityList"].ToString();
            try
            {
                dsPOQuantity = DB.GetDS(sCmdPOQuantitylist, false);
            }
            catch (Exception ex)
            {
                resetError("Error while loading",true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
            return dsPOQuantity;
        }

        #endregion

        private void resetError(string error, bool isError)
        {

            /* string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
             if (isError)
                 str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

             if (error.Length > 0)
                 str += error + "";
             else
                 str = "";


             ltInventoryStatus.Text = str;*/
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "closeAsynchronus(); showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);

        }

        protected void lnkin_cancel_Click(object sender, EventArgs e)
        {
            /* txtin_MaterialCode.Text = "";
             txtin_poitemline.Text = "";
             txtin_Storefno.Text = "";
             atcPOnumber.Text = "";
             hifPONumber.Value = "0";
             tbInBound_FormDetails.Visible = false;
             tdGridview.Visible = false;
             pnlPOQuantityList.Visible = false;
             atcPOnumber.Visible = false;
             ltPONumber.Visible = false;
             Page.Validate();*/
            Response.Redirect("StockIn.aspx");
        }

        #region----QC values----

        protected void CValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator Cvalidator = (CustomValidator)source;

            if (Cvalidator.ID == "rfvAsIsDetails")
            {
                if (chkasis.Checked && txtGMDQCRemarks.Text == "")
                {
                    args.IsValid = false;
                }
                else
                {
                    args.IsValid = true;
                }
            }
            else
            {
                String BaseID = Cvalidator.ID.Substring(5);
                TextBox txtMinValue;
                TextBox txtMaxValue;
                TextBox txtExactValue;


                if (((CheckBox)tbMaterialStorageparameter.FindControl("chk" + BaseID)).Checked)
                {
                    txtMinValue = (TextBox)tbMaterialStorageparameter.FindControl("txtMin" + BaseID);
                    txtMaxValue = (TextBox)tbMaterialStorageparameter.FindControl("txtMax" + BaseID);
                    if ((txtMinValue.Text != "" && txtMinValue.Text != "MinValue") && (txtMaxValue.Text != "" && txtMaxValue.Text != "MaxValue"))
                        args.IsValid = true;
                    else
                        args.IsValid = false;

                }
                else
                {
                    txtExactValue = (TextBox)tbMaterialStorageparameter.FindControl("txtqc" + BaseID);
                    if (txtExactValue.Text != "")
                        args.IsValid = true;
                    else
                        args.IsValid = false;
                }
            }
        }

        private void BuildHeaderDetails(String GoodsMovementDetailID, bool isFirstCall)
        {
            HtmlTableRow DetailsRow;
            HtmlTableCell DetailsNameCell;
            HtmlTableCell DetialsValuecell;
            Literal ltLiteralName;
            Literal ltLiteralValue;
            try
            {
                IDataReader rsGetGoodsMovementDetails = DB.GetRS("sp_INV_GetGoodsMovementDetailsForQcValues " + GoodsMovementDetailID);
                while (rsGetGoodsMovementDetails.Read())
                {
                    DetailsRow = new HtmlTableRow();
                    DetailsNameCell = new HtmlTableCell();
                    DetialsValuecell = new HtmlTableCell();
                    ltLiteralName = new Literal();
                    ltLiteralName.Text = DB.RSField(rsGetGoodsMovementDetails, "DisplayName");
                    ltLiteralValue = new Literal();
                    if (DB.RSField(rsGetGoodsMovementDetails, "DisplayName") == "Location")
                    {
                        ltLiteralValue.Text = ": " + DB.RSField(rsGetGoodsMovementDetails, "Value").Split('-')[0];
                        if (isFirstCall)
                        {
                            atcQCNonconformityLocation.Text = DB.RSField(rsGetGoodsMovementDetails, "Value").Split('-')[0];
                            hifQCNonConformityLocation.Value = DB.RSField(rsGetGoodsMovementDetails, "Value").Split('-')[1];
                        }
                    }
                    else
                        ltLiteralValue.Text = ": " + DB.RSField(rsGetGoodsMovementDetails, "Value");
                    DetailsNameCell.Controls.Add(ltLiteralName);
                    DetialsValuecell.Controls.Add(ltLiteralValue);
                    DetailsRow.Controls.Add(DetailsNameCell);
                    DetailsRow.Controls.Add(DetialsValuecell);
                    tbGoodsMovementDetails.Controls.Add(DetailsRow);

                }
                rsGetGoodsMovementDetails.Close();
            }
            catch (Exception ex)
            {
                resetError("Error while loading",true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }

        private void Build_DialogHeader(String GoodsMovementDetailsID, bool IsFirst)
        {
            Page.Validate();
            BuildHeaderDetails(GoodsMovementDetailsID, true);

            /*IDataReader rsGetGoodsMovementQCQuantityDetails = DB.GetRS("select  from INV_GoodsMovementDetails GMD WHERE GoodsMovementDetailsID=" + hifGoodsMovementID.Value);
            rsGetGoodsMovementQCQuantityDetails.Read();
            
            rsGetGoodsMovementQCQuantityDetails.Close();
            */
            try
            {
                IDataReader rsGetGoodsMovementDetails = DB.GetRS("select AsIs,convert(nvarchar,QCRemarks) as QCRemarks,left(Location,2) as Location,DocQty,ISNULL((SELECT SUM(Quantity) FROM (SELECT DISTINCT QCSerialNo,Quantity FROM INV_QualityParametersCapture WHERE GoodsMovementDetailsID=GMD.GoodsMovementDetailsID AND IsDeleted=0) as kkk),0)AS QCQuantity from INV_GoodsMovementDetails GMD JOIN INV_Location LOC ON LOC.LocationID=GMD.LocationID where GoodsMovementDetailsID=" + hifGoodsMovementID.Value);
                rsGetGoodsMovementDetails.Read();
                ltCompletedQty.Text = DB.RSFieldDecimal(rsGetGoodsMovementDetails, "QCQuantity").ToString();
                ltTotalQty.Text = DB.RSFieldDecimal(rsGetGoodsMovementDetails, "DocQty").ToString();
                chkasis.Checked = DB.RSFieldBool(rsGetGoodsMovementDetails, "AsIs");
                txtGMDQCRemarks.Text = DB.RSField(rsGetGoodsMovementDetails, "QCRemarks");
                hifCheckLocation.Value = DB.RSField(rsGetGoodsMovementDetails, "Location");

                rsGetGoodsMovementDetails.Close();
            }
            catch (Exception ex)
            {
                resetError("Error while loading",true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
            if (IsFirst)
                Build_QCDataials(Build_QCDataials());
            Build_QCParameters((ltQCSerialNo.Text != "" ? ltQCSerialNo.Text.Split('-')[1] : ""));
        }

        protected void lnkUpdateQcValues_Click(object sender, EventArgs e)
        {

            //Update QC Details for goods movement

            Page.Validate("QcParametersGroup");

            if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed("-3")))
            {
                BuildHeaderDetails(hifGoodsMovementID.Value, false);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "closeAsynchronus(); ShowDefaultTextBoxes(); showStickyToast(false,'Only QCI can capture QC');", true);
                return;
            }
            if (!IsValid)
            {
                BuildHeaderDetails(hifGoodsMovementID.Value, false);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "closeAsynchronus(); ShowDefaultTextBoxes();", true);
                return;
            }
            if (hifSelectedQty.Value == "")
            {
                BuildHeaderDetails(hifGoodsMovementID.Value, false);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "closeAsynchronus(); ShowDefaultTextBoxes();  showStickyToast(false,\"Select QC Quantity\");", true);
                return;
            }

            // if (chkBulkUpdate.Checked && (Convert.ToInt16(lbTotalQty.Text) - Convert.ToInt16(lbPosition.Text)) + 1 < Convert.ToInt16((txtQcQuantity.Text != "" ? txtQcQuantity.Text : "1")))
            if (Convert.ToDecimal(hifSelectedQty.Value) < Convert.ToDecimal(txtQcQuantity.Text != "" ? txtQcQuantity.Text : "1"))
            {
                //Build_DialogHeader(hifGoodsMovementID.Value, Convert.ToInt32(lbPosition.Text));

                BuildHeaderDetails(hifGoodsMovementID.Value, false);
                //atcQCNonconformityLocation.Text = "";
                //hifQCNonConformityLocation.Value = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "closeAsynchronus(); ShowDefaultTextBoxes();  showStickyToast(false,\"'Bulk QC Qty.' is more than pending QC Quantity'\");", true);
                //resetError("'Bulk QC Qty.' is more than pending QC Quantity", true);
                return;
            }
            IDataReader rsConfigureMSP = null;
            try
            {
                rsConfigureMSP = DB.GetRS("dbo.sp_MFG_GetQcMaterialStorageParameterForMaterial @MaterialMasterID=" + CommonLogic.QueryString("mmid"));
            }
            catch (Exception ex)
            {
                resetError("Error while transaction", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                    return;
            }

            String ControlID;
            String value = "";
            String Minvalue;
            String MaxValue;
            String HiddenValue;
            String MSPIDs = "";
            String MSPValues = "";
            Boolean isnonConfirm = false;

            while (rsConfigureMSP.Read())
            {


                String ControlType = DB.RSField(rsConfigureMSP, "ControlType");


                if (ControlType == "DropDownList")
                {
                    ControlID = "ddlqc" + DB.RSField(rsConfigureMSP, "ParameterName");
                    value = ((DropDownList)tbMaterialStorageparameter.FindControl(ControlID)).SelectedValue;

                }
                else if (ControlType == "TextBox")
                {
                    ControlID = "chk" + DB.RSField(rsConfigureMSP, "ParameterName");

                    if (DB.RSField(rsConfigureMSP, "ParameterDataType") == "Decimal" && ((CheckBox)tbMaterialStorageparameter.FindControl(ControlID)).Checked)
                    {
                        Minvalue = ((TextBox)tbMaterialStorageparameter.FindControl("txtMin" + DB.RSField(rsConfigureMSP, "ParameterName"))).Text;
                        MaxValue = ((TextBox)tbMaterialStorageparameter.FindControl("txtMax" + DB.RSField(rsConfigureMSP, "ParameterName"))).Text;
                        if (Minvalue != "" && MaxValue != "" && Minvalue != "MinValue" && MaxValue != "MaxValue")
                        {
                            if (Convert.ToDecimal(Minvalue) < DB.RSFieldDecimal(rsConfigureMSP, "MinTolerance") || Convert.ToDecimal(Minvalue) > DB.RSFieldDecimal(rsConfigureMSP, "MaxTolerance") || Convert.ToDecimal(MaxValue) < DB.RSFieldDecimal(rsConfigureMSP, "MinTolerance") || Convert.ToDecimal(MaxValue) > DB.RSFieldDecimal(rsConfigureMSP, "MaxTolerance"))
                            {
                                isnonConfirm = true;
                            }
                            value = Minvalue + "-" + MaxValue;
                        }
                    }
                    else
                    {
                        ControlID = "txtqc" + DB.RSField(rsConfigureMSP, "ParameterName");
                        value = ((TextBox)tbMaterialStorageparameter.FindControl(ControlID)).Text.Trim();
                        if (value != "" && (DB.RSField(rsConfigureMSP, "ParameterDataType") == "Decimal" || DB.RSField(rsConfigureMSP, "ParameterDataType") == "Integer") && (DB.RSFieldDecimal(rsConfigureMSP, "MinTolerance") > Convert.ToDecimal(value) || DB.RSFieldDecimal(rsConfigureMSP, "MaxTolerance") < Convert.ToDecimal(value)))
                        {
                            isnonConfirm = true;
                        }
                    }

                }
                else if (ControlType == "CheckBox")
                {
                    ControlID = "chkqc" + DB.RSField(rsConfigureMSP, "ParameterName");
                    value = Convert.ToInt16(((CheckBox)tbMaterialStorageparameter.FindControl(ControlID)).Checked).ToString();
                }
                else if (ControlType == "RadioBox")
                {
                    ControlID = "rblqc" + DB.RSField(rsConfigureMSP, "ParameterName");
                    value = ((RadioButtonList)tbMaterialStorageparameter.FindControl(ControlID)).SelectedValue;
                }

                // if (value != "")
                {
                    MSPIDs += DB.RSFieldInt(rsConfigureMSP, "QualityParameterID") + ",";
                    MSPValues += value + ",";
                }
            }
            rsConfigureMSP.Close();

            try
            {
                String QCSerialNo = "";
                if (ltQCSerialNo.Text != "")
                {
                    QCSerialNo = ltQCSerialNo.Text.Split('-')[1];
                }
                if ((!chkasis.Checked && (isnonConfirm || ((CheckBox)tbQcParameters.FindControl("chkqcIsnonconformity")).Checked)) || (hifCheckLocation.Value == "Q1" && !(isnonConfirm || ((CheckBox)tbQcParameters.FindControl("chkqcIsnonconformity")).Checked)))
                {
                    if (hifQCNonConformityLocation.Value == "" || atcQCNonconformityLocation.Text == "")
                    {
                        atcQCNonconformityLocation.Enabled = true;
                        if (isnonConfirm || ((CheckBox)tbQcParameters.FindControl("chkqcIsnonconformity")).Checked)
                            ((CheckBox)tbQcParameters.FindControl("chkqcIsnonconformity")).Checked = true;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "closeAsynchronus(); ShowDefaultTextBoxes(); showStickyToast(false,'Select location for item');", true);
                        BuildHeaderDetails(hifGoodsMovementID.Value, false);
                        return;
                    }
                }

                StringBuilder sCmdUpdateQCParameters = new StringBuilder();
                sCmdUpdateQCParameters.Append("DECLARE @NewResult int ");
                sCmdUpdateQCParameters.Append("EXEC [dbo].[sp_INV_UpsertGoodsMovementDetails_QualityParameter] ");
                sCmdUpdateQCParameters.Append("@GoodsMovementDetailsID=" + hifGoodsMovementID.Value);
                sCmdUpdateQCParameters.Append(",@QualityParameterIDs=" + DB.SQuote(MSPIDs));
                sCmdUpdateQCParameters.Append(",@AsIs=" + Convert.ToInt16(chkasis.Checked));
                sCmdUpdateQCParameters.Append(",@QCRemarks=" + (chkasis.Checked ? DB.SQuote(txtGMDQCRemarks.Text) : "''"));
                sCmdUpdateQCParameters.Append(",@QCQuantity=" + (txtQcQuantity.Text.Trim() != "" ? txtQcQuantity.Text.Trim() : "1"));
                sCmdUpdateQCParameters.Append(",@QCSerialNo=" + (QCSerialNo != "" ? QCSerialNo : "NULL"));
                sCmdUpdateQCParameters.Append(",@LocationID=" + (hifQCNonConformityLocation.Value != "" && atcQCNonconformityLocation.Text != "" ? hifQCNonConformityLocation.Value : "NULL"));
                sCmdUpdateQCParameters.Append(",@Values=" + DB.SQuote(MSPValues));
                sCmdUpdateQCParameters.Append(",@IsNonConfirmity=" + ((isnonConfirm || ((CheckBox)tbQcParameters.FindControl("chkqcIsnonconformity")).Checked) ? "1" : "0"));
                sCmdUpdateQCParameters.Append(",@CreatedBy=" + cp.UserID);
                sCmdUpdateQCParameters.Append(",@Result=@NewResult output select @NewResult as N");


                //test case ID (TC_052)
                //Update QC values for the goodsin materials

                int Result = DB.GetSqlN(sCmdUpdateQCParameters.ToString());

                Build_DialogHeader(hifGoodsMovementID.Value, true);
                hifSelectedQty.Value = "";
                Build_QCParameters((ltQCSerialNo.Text != "" ? ltQCSerialNo.Text.Split('-')[1] : ""));
                ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "closeAsynchronus(); ShowDefaultTextBoxes(); showStickyToast(true,'Successfully Updated');", true);

            }
            catch (Exception ex)
            {

                Build_DialogHeader(hifGoodsMovementID.Value, false);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "closeAsynchronus(); ShowDefaultTextBoxes(); showStickyToast(false,'Error while updating');", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }

        protected void chkSelect_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkSelectrow = (CheckBox)sender;
            GridViewRow checkRow = (GridViewRow)chkSelectrow.Parent.Parent;
            String QCSerialNo = ((HiddenField)checkRow.FindControl("hifSerialNumber")).Value;
            Build_DialogHeader(hifGoodsMovementID.Value, false);
            if (chkSelectrow.Checked)
            {
                txtQcQuantity.Text = ((Literal)checkRow.FindControl("ltQuantity")).Text;
                hifSelectedQty.Value = ((Literal)checkRow.FindControl("ltQuantity")).Text;
                ltQCSerialNo.Text = (((Literal)checkRow.FindControl("ltSerialNo")).Text != "Pending Quantity" ? ((Literal)checkRow.FindControl("ltSerialNo")).Text : "");
                if (QCSerialNo != "")
                {
                    Build_QCParameters(QCSerialNo);
                }

            }
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "openQCParametersDialog(); ShowDefaultTextBoxes();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "closeAsynchronus(); ShowDefaultTextBoxes();", true);
        }

        private DataSet Build_QCDataials()
        {
            DataSet dsGetQCGoodsINDetails = null;
            String cMdGetQCGoodsINDetails = "sp_INV_GetQCDetailsList " + hifGoodsMovementID.Value;
            try
            {
                dsGetQCGoodsINDetails = DB.GetDS(cMdGetQCGoodsINDetails, false);
            }
            catch (Exception ex)
            {
                resetError("Error while loading",true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
            return dsGetQCGoodsINDetails;
        }

        private void Build_QCDataials(DataSet dsGetQCGoodsINDetails)
        {
            gvQCUpdatedList.DataSource = dsGetQCGoodsINDetails;
            gvQCUpdatedList.DataBind();
            dsGetQCGoodsINDetails.Dispose();
            if (gvQCUpdatedList.Rows.Count < 5)
                pnlQCUpdateList.Height = gvQCUpdatedList.Height;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "QCParameters", "closeAsynchronus(); unblockQCParametersDialog();", true);

        }

        private void Build_QCParameters(String QCSerialNo)
        {
            try
            {
                IDataReader dsGoodsMovementQCParams = DB.GetRS("dbo.sp_INV_GetQCParameterValuesForGoodsMovement	@GoodsMovementDetailsID=" + hifGoodsMovementID.Value + ",@QCSerialNo=" + (QCSerialNo != "" ? QCSerialNo : "0"));
                while (dsGoodsMovementQCParams.Read())
                {
                    String ControlID = null;
                    /*
                    if (isfirst)
                    {
                        ltQCSerialNo.Text = DB.RSField(dsGoodsMovementQCParams, "QCSerialNo");
                        if (DB.RSField(dsGoodsMovementQCParams, "QCSerialNo") == "")
                        {
                            imgright.Enabled = false;
                            chkBulkUpdate.Checked = false;
                            chkBulkUpdate.Enabled = true;
                        }
                        else
                        {
                            chkBulkUpdate.Checked = false;
                            chkBulkUpdate.Enabled = false;
                        }
                        txtQcQuantity.Text = "";
                        isfirst = false;
                    }*/
                    if (DB.RSField(dsGoodsMovementQCParams, "ControlType") == "TextBox")
                    {

                        if (DB.RSField(dsGoodsMovementQCParams, "ParameterDataType") == "Decimal")
                        {

                            if (DB.RSField(dsGoodsMovementQCParams, "Value").Split('-').Length == 2)
                            {
                                ControlID = "txtMin" + DB.RSField(dsGoodsMovementQCParams, "ParameterName");
                                ((TextBox)tbMaterialStorageparameter.FindControl(ControlID)).Text = DB.RSField(dsGoodsMovementQCParams, "Value").Split('-')[0];
                                ControlID = "txtMax" + DB.RSField(dsGoodsMovementQCParams, "ParameterName");
                                ((TextBox)tbMaterialStorageparameter.FindControl(ControlID)).Text = DB.RSField(dsGoodsMovementQCParams, "Value").Split('-')[1];
                                ((CheckBox)tbMaterialStorageparameter.FindControl("chk" + DB.RSField(dsGoodsMovementQCParams, "ParameterName"))).Checked = true;
                                ControlID = "txtqc" + DB.RSField(dsGoodsMovementQCParams, "ParameterName");
                                ((TextBox)tbMaterialStorageparameter.FindControl(ControlID)).Text = "";
                            }
                            else
                            {
                                ControlID = "txtMin" + DB.RSField(dsGoodsMovementQCParams, "ParameterName");
                                ((TextBox)tbMaterialStorageparameter.FindControl(ControlID)).Text = "";
                                ControlID = "txtMax" + DB.RSField(dsGoodsMovementQCParams, "ParameterName");
                                ((TextBox)tbMaterialStorageparameter.FindControl(ControlID)).Text = "";
                                ((CheckBox)tbMaterialStorageparameter.FindControl("chk" + DB.RSField(dsGoodsMovementQCParams, "ParameterName"))).Checked = false;
                                ControlID = "txtqc" + DB.RSField(dsGoodsMovementQCParams, "ParameterName");
                                ((TextBox)tbMaterialStorageparameter.FindControl(ControlID)).Text = DB.RSField(dsGoodsMovementQCParams, "Value");
                            }
                        }
                        else
                        {
                            ControlID = "txtqc" + DB.RSField(dsGoodsMovementQCParams, "ParameterName");
                            //((TextBox)tbMaterialStorageparameter.FindControl(ControlID)).Text = "";
                            ((TextBox)tbMaterialStorageparameter.FindControl(ControlID)).Text = DB.RSField(dsGoodsMovementQCParams, "Value");
                        }
                    }
                    else if (DB.RSField(dsGoodsMovementQCParams, "ControlType") == "DropDownList")
                    {
                        ControlID = "ddlqc" + DB.RSField(dsGoodsMovementQCParams, "ParameterName");
                        ((DropDownList)tbMaterialStorageparameter.FindControl(ControlID)).SelectedValue = DB.RSField(dsGoodsMovementQCParams, "Value");
                    }
                    else if (DB.RSField(dsGoodsMovementQCParams, "ControlType") == "CheckBox")
                    {
                        ControlID = "chkqc" + DB.RSField(dsGoodsMovementQCParams, "ParameterName");
                        ((CheckBox)tbMaterialStorageparameter.FindControl(ControlID)).Checked = Convert.ToBoolean(DB.RSFieldTinyInt(dsGoodsMovementQCParams, "Value"));
                        atcQCNonconformityLocation.Enabled = true;
                        if (!((CheckBox)tbMaterialStorageparameter.FindControl(ControlID)).Checked)
                        {
                            atcQCNonconformityLocation.Text = "";
                            hifQCNonConformityLocation.Value = "";
                            atcQCNonconformityLocation.Enabled = false;
                        }
                    }
                    else if (DB.RSField(dsGoodsMovementQCParams, "ControlType") == "RadioBox")
                    {
                        ControlID = "rblqc" + DB.RSField(dsGoodsMovementQCParams, "ParameterName");
                        ((RadioButtonList)tbMaterialStorageparameter.FindControl(ControlID)).SelectedIndex = -1;
                        ((RadioButtonList)tbMaterialStorageparameter.FindControl(ControlID)).SelectedValue = DB.RSField(dsGoodsMovementQCParams, "Value");
                    }
                }
                dsGoodsMovementQCParams.Close();
            }
            catch (Exception ex)
            {
                resetError("Error while loading",true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            //test case ID (TC_059)
            //Select captured QC parameters to delete

            StringBuilder DeleteSerialNos = new StringBuilder();
            foreach (GridViewRow row in gvQCUpdatedList.Rows)
            {
                if (((CheckBox)row.FindControl("chkSelect")).Checked)
                {
                    DeleteSerialNos.Append(((HiddenField)row.FindControl("hifSerialNumber")).Value + ",");
                }
            }
            if (DeleteSerialNos.ToString() != "" || DeleteSerialNos.ToString() != ",")
            {
                try
                {
                    DB.ExecuteSQL("dbo.sp_INV_DeleteQCParameterValues @GoodsMovementDetailsID=" + hifGoodsMovementID.Value + ",@QCSerialNoS=" + DB.SQuote(DeleteSerialNos.ToString()));
                    Build_DialogHeader(hifGoodsMovementID.Value, true);
                    hifSelectedQty.Value = "";
                    Build_QCParameters("");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "closeAsynchronus(); ShowDefaultTextBoxes(); showStickyToast(true,'Successfully deleted the selected items');", true);
                }
                catch (Exception ex)
                {
                    resetError("Error while Deleting",true);
                    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                }
            }
            else
            {
                resetError("No item is select", true);
            }


        }

        #endregion

        private void resetDailogError(string error, bool isError)
        {

            /* string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
             if (isError)
                 str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

             if (error.Length > 0)
                 str += error + "";
             else
                 str = "";


             lblDailogStatus.Text = str;*/
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "closeAsynchronus(); showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);

        }
        
        protected void lnkClose_Click(object sender, EventArgs e)
        {
            ViewState["autoOpen"] = "false";
            gvPOQuantityList.EditIndex = -1;
            ViewState["gvPOQuantityList"] = "EXEC dbo.sp_INV_GetGoodsMovementDetailsList @MaterialMasterID=" + CommonLogic.QueryString("mmid") + ",@InboundID=" + CommonLogic.QueryString("ibdno") + ",@LineNumber=" + txtin_poitemline.Text.Trim() + ",@POHeaderID=" + hifPONumber.Value + ",@SupplierInvoiceID=" + hifInvoiceNumber.Value;
            Build_gvPOQuantityilst(Build_gvPOQuantityilst());
            decimal QuantitySum = 0;
            try
            {
                QuantitySum = DB.GetSqlNDecimal("sp_INV_GetGoodsInQuantitySum @MaterialMasterID=" + CommonLogic.QueryString("mmid") + ",@InboundID=" + CommonLogic.QueryString("ibdno") + ",@LineNumber=" + txtin_poitemline.Text.Trim() + ",@POHeaderID=" + hifPONumber.Value + ", @GoodsMovementDetailsID = 0" + ",@SupplierInvoiceID=" + hifInvoiceNumber.Value);
            }
            catch (Exception ex)
            {
                resetError("Error while loading",true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
            if (gvPOQuantityList.Rows.Count != 0)
                ((Literal)gvPOQuantityList.FooterRow.FindControl("ltQuantityCount")).Text = "  Sum : " + QuantitySum;
        }

        #region----Print function----

        public void LoadStoresWithIPs(DropDownList ddlWH)
        {

            ddlWH.Items.Clear();

            ddlWH.Items.Add(new ListItem("Select Printer", "0"));
            ddlWH.Items.Add(new ListItem("Data Max - LP", "172.18.0.127"));
            ddlWH.Items.Add(new ListItem("Zebraa - LP", "172.18.0.53"));

        }

        public void PrintLabel(String MCode, String MMID, String Description, String StoreRefNo, String MspIDs, String MspValues, String Qty, String PrinterIP, String MfgDate, String ExpDate, String SerNo, String BatchNo,String ProjectNo,int QtyToPrint,String PrintQty)
        {
            try
            {



                MRLWMSC21Common.TracklineMLabel thisMLabel = new MRLWMSC21Common.TracklineMLabel();

                thisMLabel.MCode = MCode; //DB.GetSqlS("select MCode AS S from MMT_MaterialMaster where IsActive=1 AND IsDeleted=0 AND MaterialMasterID=" + MMID);
                thisMLabel.OEMPartNo = DB.GetSqlS("select OEMPartNo AS S from MMT_MaterialMaster where IsActive=1 AND IsDeleted=0 AND MaterialMasterID=" + MMID);
                thisMLabel.Description = Description;
                thisMLabel.AltMCode = ltInvUoMQtyValue.Text;


                /*
                if (Qty != "")
                    thisMLabel.InvQty = Convert.ToDecimal(Convert.ToDecimal(Qty));
                else
                    Qty = Convert.ToDecimal("0.00").ToString();
                */

                if (QtyToPrint == 0)
                {
                    thisMLabel.InvQty = 1;
                    thisMLabel.IsQtyNeedToPrint = false;
                }
                else
                {
                    thisMLabel.InvQty = QtyToPrint;
                    thisMLabel.IsQtyNeedToPrint = true;
                }

                thisMLabel.PrintQty = QtyToPrint.ToString();

                string vltMfgDate = "";
                string vltExpiryDate = "";
                string vltBatchNo = "";
                string vltSerialNo = "";

                if (MfgDate != "" || ExpDate != "" || SerNo != "" || BatchNo != "")
                {
                    vltMfgDate = MfgDate;
                    vltExpiryDate = ExpDate;
                    vltSerialNo = SerNo;
                    vltBatchNo = BatchNo;
                }


                IDataReader drMsps = DB.GetRS("[dbo].[sp_INV_GetMspValues] @MaterialStorageParameterIDs='" + MspIDs + "',@MaterialStorageParameterValues='" + MspValues + "'");

                while (drMsps.Read())
                {

                    switch (DB.RSFieldInt(drMsps, "MSPID"))
                    {
                        case 1:
                            vltMfgDate = DB.RSField(drMsps, "Value");
                            break;

                        case 2:
                            vltExpiryDate = DB.RSField(drMsps, "Value");
                            break;

                        case 3:
                            vltSerialNo = DB.RSField(drMsps, "Value");
                            break;

                        case 4:
                            vltBatchNo = DB.RSField(drMsps, "Value");
                            break;
                    }
                }

                drMsps.Close();

                if (vltMfgDate != "")
                {
                    thisMLabel.MfgDate = vltMfgDate;
                }
                else
                {
                    thisMLabel.MfgDate = "";
                }
                if (vltExpiryDate != "")
                {
                    thisMLabel.ExpDate = vltExpiryDate;
                }
                else
                {
                    thisMLabel.ExpDate = "";
                }
               
                thisMLabel.BatchNo = vltBatchNo;
                thisMLabel.SerialNo = vltSerialNo;


                
                thisMLabel.PrinterType = "IP";
                thisMLabel.PrinterIP = ddlNetworkPrinter.SelectedValue.Trim();
                string ddl = ddlLabelSize.SelectedValue.Trim();
                string length = "";
                string width = "";
                string LabelType = "";
                string query = "select * from TPL_Tenant_BarcodeType where IsActive=1 and IsDeleted=0 and TenantBarcodeTypeID=" + ddl;
                DataSet DS = DB.GetDS(query, false);
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    length = row["Length"].ToString();
                    width = row["Width"].ToString();
                    LabelType = row["LabelType"].ToString();
                }
             
                thisMLabel.Length = length;
                thisMLabel.Width = width;
                int dpi = 0;
                dpi = CommonLogic.GETDPI(thisMLabel.PrinterIP);
                thisMLabel.Dpi = dpi;
                thisMLabel.LabelType = LabelType;
                thisMLabel.IsBoxLabelReq = false;
                thisMLabel.ReqNo = "";
                thisMLabel.StrRefNo = txtin_Storefno.Text.Trim(); // DB.GetSqlS("select StoreRefNo AS S from INB_Inbound where IsActive=1 AND IsDeleted=0 AND InboundID=" + StoreRefNo); 
                thisMLabel.KitCode = "";
                thisMLabel.OBDNumber = "";
                //For Printing Label Using ZPL Added by Prasanna 
                MRLWMSC21Common.PrintCommon.CommonPrint print = new MRLWMSC21Common.PrintCommon.CommonPrint();
                print.PrintBarcodeLabel(thisMLabel);




                // ParameterizedThreadStart tThermalLabelWorker = new ParameterizedThreadStart(this.ThermalLabelWorker);
                // Thread worker = new Thread(new ParameterizedThreadStart(this.ThermalLabelWorker));


                //Thread worker = new Thread(this.ThermalLabelWorker);
                //Thread worker = new Thread(new ParameterizedThreadStart(this.ThermalLabelWorker));
                //worker.SetApartmentState(ApartmentState.STA);
                //worker.Name = "ThermalLabelWorker";
                //worker.Start(thisMLabel);
                //worker.Join();

                //CommonLogic.SendPrintJob(DB.RSField(rsLineItem, "MCode"), DB.RSField(rsLineItem, "AlternativeMCode"), DB.RSField(rsLineItem, "MDescription"), DB.RSField(rsLineItem, "BatchNo"),"",DB.RSFieldInt(rsLineItem, "KitPlannerID").ToString(), Convert.ToInt32(DB.RSFieldDecimal(rsLineItem, "InvoiceQuantity")) + 1, DateTime.MinValue, DB.RSFieldDateTime(rsLineItem, "ExpiryDate"), "IP", ddlNetworkPrinter.SelectedValue.Trim(), false, out result);
              
                  resetError("Successfully printed selected line items", false);
                



            }
            catch (Exception ex)
            {
                resetError("Error while printing", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                return;
            }
        }

        private void ThermalLabelWorker(object thisMLabel2)
        {
            try
            {
                MRLWMSC21Common.TracklineMLabel thisMLabel = (MRLWMSC21Common.TracklineMLabel)thisMLabel2;
                String vresult = "";
                //CommonLogic.SendPrintJob_Big_7p6x5(thisMLabel.MCode, thisMLabel.AltMCode, thisMLabel.OEMPartNo, thisMLabel.Description, thisMLabel.BatchNo, thisMLabel.SerialNo, thisMLabel.KitPlannerID.ToString(), thisMLabel.KitChildrenCount, thisMLabel.ParentMCode, thisMLabel.InvQty, thisMLabel.MfgDate, thisMLabel.ExpDate, thisMLabel.PrinterType, thisMLabel.PrinterIP, thisMLabel.StrRefNo, thisMLabel.OBDNumber, thisMLabel.KitCode, thisMLabel.ReqNo, thisMLabel.IsBoxLabelReq, thisMLabel.IsQtyNeedToPrint,thisMLabel.PrintQty, out vresult);
                thisMLabel.Result = vresult;
            }
            catch (Exception ex)
            {
                resetError("Error while printing", true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                return;
            }
        }

        #endregion

        
        protected void lnkPrintSubmit_Click(object sender, EventArgs e)
        {

            try
            {
                if (chkIsPrintRequired.Checked)
                {
                    if (ddlNetworkPrinter.SelectedValue == "0")
                    {
                        resetError("Please select printer", true);
                        return;
                    }
                    else
                    {

                        String MfgDate = "", ExpDate = "", SerNo = "", BatchNo = "",
                            ProjectRefno = "", PrintQty="";

                        GridViewRow gvrEditRow = gvPOQuantityList.Rows[Convert.ToInt32(ViewState["index"])];

                        Control cMfgDate = gvrEditRow.FindControl("MfgDate");
                        if (cMfgDate != null)
                        {
                            MfgDate = ((Literal)cMfgDate).Text.Trim().ToString();
                        }

                        Control cExpDate = gvrEditRow.FindControl("ExpDate");
                        if (cExpDate != null)
                        {
                            ExpDate = ((Literal)cExpDate).Text.Trim().ToString();
                        }

                        Control cSerNo = gvrEditRow.FindControl("SerialNo");
                        if (cSerNo != null)
                        {
                            SerNo = ((Literal)cSerNo).Text.Trim().ToString();
                        }

                        Control cBatchNo = gvrEditRow.FindControl("BatchNo");
                        if (cBatchNo != null)
                        {
                            BatchNo = ((Literal)cBatchNo).Text.Trim().ToString();
                        }
                      

                        Control cPrintQty = gvrEditRow.FindControl("ltQuantity");
                        if (cPrintQty != null)
                        {
                            PrintQty = ((Literal)cPrintQty).Text.Trim().ToString();
                        }

                        String IsPositiveRecall = ((Literal)gvrEditRow.FindControl("ltImgpositiveRecall")).Text.Trim().ToString();

                        String pickQuantity = Convert.ToString(Convert.ToInt32(txtPrintQty.Text));  //((Literal)gvrEditRow.FindControl("ltQuantity")).Text.Trim().ToString();

                        PrintLabel((IsPositiveRecall == "1" ? txtin_MaterialCode.Text + "-P" : txtin_MaterialCode.Text), CommonLogic.QueryString("mmid"), ltDescriptionvalue.Text, CommonLogic.QueryString("ibdno"), "", "", pickQuantity, ddlNetworkPrinter.SelectedValue, MfgDate, ExpDate, SerNo, BatchNo, ProjectRefno,Convert.ToInt32(pickQuantity),PrintQty);
                    }
                }
                else
                {
                    resetError("Please check 'Is Print Required' checkbox", true);
                    return;
                }




                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "dialog", "closePrintDialog()", true);
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);

            }
        }
        protected void lnkGenerateNewContainer_Click(object sender, EventArgs e)
        {
            if (getNewCarton(txtin_Storefno.Text) != null)
                resetError("Successfully Created", false);
            else
                resetError("Error while generating new container", true);
        }

        public string getNewCarton(string storeRefNumber)
        {

            StringBuilder query = new StringBuilder();
            //query.Append("Declare @newCarton nvarchar(50) set @newCarton=(select 'CAR'+right('0000'+convert(varchar(4), count(*)+1), 4)+'" + storeRefNumber.Trim() + "'  as Carton from INV_Carton where CartonCode like '_______" + storeRefNumber.Trim() + "%' ) ");
            //query.Append(" insert into INV_Carton(CartonCode,CreatedBy) values(@newCarton," + cp.UserID + ") ");
            //query.Append(" select @newCarton as s ");

            //query.Append("Declare @newCarton nvarchar(50) set @newCarton=(select 'C'+right('00'+convert(varchar(4), count(*)+1), 4)+'" + storeRefNumber.Trim() + "'  as Carton from INV_Carton where CartonCode like '%" + storeRefNumber.Trim() + "%' ) ");
            //query.Append(" insert into INV_Carton(CartonCode,CreatedBy) values(@newCarton," + cp.UserID + ") ");
            //query.Append(" select @newCarton as s ");

            query.Append("DECLARE @NewUpdateCarton nvarchar(50);   ");
            query.Append("EXEC [dbo].[sp_INV_CartonCreation]    ");
            query.Append("@InboundID=" + CommonLogic.QueryString("ibdno").ToString() + ",");
            query.Append("@UserId=" + cp.UserID + ",");
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

        protected void lnkMapContainertoLocation_Click(object sender, EventArgs e)
        {
            DataSet ds = LoadContainerMapping();
            DataRow drNewRow = ds.Tables[0].NewRow();
            drNewRow["CartonID"] = 0;
            drNewRow["LocationID"] = 0;
            //drNewRow["hifPreviousLocID"] = 0;
            ds.Tables[0].Rows.InsertAt(drNewRow, 0);
            gvLocationMapping.EditIndex = 0;
            gvLocationMapping.DataSource = ds;
            gvLocationMapping.DataBind();
        }

        protected void gvLocationMapping_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvLocationMapping.EditIndex = e.NewEditIndex;
            LoadContainerMapping(LoadContainerMapping());
        }

        protected void gvLocationMapping_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Page.Validate("VgMapping");
            if (!IsValid)
            {
                return;
            }
            HiddenField hifPreviousCartonID = ((HiddenField)gvLocationMapping.Rows[e.RowIndex].FindControl("hifPreviousCartonID"));
            HiddenField hifCartonID = ((HiddenField)gvLocationMapping.Rows[e.RowIndex].FindControl("hifCartonID"));
            HiddenField hifLocationID = ((HiddenField)gvLocationMapping.Rows[e.RowIndex].FindControl("hifLocationID"));
            HiddenField hifPreviousLocationID = ((HiddenField)gvLocationMapping.Rows[e.RowIndex].FindControl("hifPreviousLocationID"));


            String query = "";
            if (hifPreviousCartonID.Value == "0")
                query = "update INV_GoodsMovementDetails set LocationID=" + hifLocationID.Value + " where GoodsMovementDetailsID in " +
                            "(select GoodsMovementDetailsID from INV_CartonGoodsMovementDetails where CartonID=" + hifCartonID.Value + ")";
            else
            {
                if (hifPreviousCartonID.Value != hifCartonID.Value)
                {

                    query = " update INV_GoodsMovementDetails set LocationID=NULL where GoodsMovementDetailsID in " +
                            "(select GoodsMovementDetailsID from INV_CartonGoodsMovementDetails where CartonID=" + hifPreviousCartonID.Value + ")";

                    //query = "update INV_CartonGoodsMovementDetails set CartonID=" + hifCartonID.Value + " where CartonID=" + hifPreviousCartonID.Value + "";
                    //query += "update INV_GoodsMovementDetails set LocationID=" + hifLocationID.Value + " where GoodsMovementDetailsID in (select GoodsMovementDetailsID from INV_CartonGoodsMovementDetails where CartonID =" + hifCartonID.Value + ")";
                }
                //if (hifPreviousLocationID.Value != hifLocationID.Value)
                query += " update INV_GoodsMovementDetails set LocationID=" + hifLocationID.Value + " where GoodsMovementDetailsID in (select GoodsMovementDetailsID from INV_CartonGoodsMovementDetails where CartonID =" + hifCartonID.Value + ")";


            }
            try
            {
                if (query != "")
                    DB.ExecuteSQL(query);
                gvLocationMapping.EditIndex = -1;
                LoadContainerMapping(LoadContainerMapping());
                Build_gvPOQuantityilst(Build_gvPOQuantityilst());//Update Location  for the recevie grid
                resetError("Successfully updated", false);
            }
            catch (Exception ex)
            {
                resetError("Error while updating", false);
            }

        }

        protected void gvLocationMapping_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvLocationMapping.EditIndex = -1;
            LoadContainerMapping(LoadContainerMapping());
        }

        protected void gvLocationMapping_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((e.Row.RowType == DataControlRowType.DataRow) && !(e.Row.RowState == DataControlRowState.Edit))
            {
                DataRow row = ((DataRowView)e.Row.DataItem).Row;
                int count = gvLocationMapping.Columns.Count;
                if ((int)ViewState["InboundStatus"] >= 4)
                {
                    e.Row.Cells[count - 1].Enabled = false;
                }

               

            }
             String Location = "";
            if ((e.Row.RowType == DataControlRowType.DataRow))
            {
                if ((e.Row.RowState != DataControlRowState.Edit))
                {
                    if (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate)
                    {
                        Location = ((Literal)e.Row.FindControl("ltLocation")).Text;
                    }
                    if (Location != "STAGING" && Location != "")
                    {
                        e.Row.Cells[2].Enabled = false;
                    }
                }
                e.Row.Cells[2].Enabled = false;

            }
        }


    }
}