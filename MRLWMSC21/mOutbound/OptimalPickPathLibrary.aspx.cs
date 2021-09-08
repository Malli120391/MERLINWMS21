using MRLWMSC21Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using MRLWMSC21Core.Library;
using MRLWMSC21_Library.WMS_DBCommon;
using MRLWMSC21_Library.WMS_ServiceObjects;
using MRLWMSC21_Library.WMS_Services;
using MRLWMSC21_Library.WMS_ServiceControl;

using System.Collections;
using MRLWMSC21.mOutbound.OptimalPickNote;

namespace MRLWMSC21.mOutbound
{
    public partial class OptimalPickPathLibrary : System.Web.UI.Page
    {
        CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        protected String OBDNumber;
        protected OBDTrack thisOOBD;
        public IDictionary<int, SOLineItemData> soInfo;
        public IDictionary<int, string> soMSPs;
        public IDictionary<int, MSP> MSP;
        StringBuilder RowDataHeader = new StringBuilder();
        bool isFirstTime = true;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Outbound";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                String OBDNumber = DB.GetSqlS("select top 1 OBDNumber AS S from OBD_Outbound where IsActive=1 and IsDeleted=0 AND OutboundID=" + CommonLogic.QueryStringUSInt("obdid"));

                if (OBDNumber.Length != 0)
                {
                    thisOOBD = new OBDTrack(Convert.ToInt16(CommonLogic.QueryStringUSInt("obdid")));
                }

                ltDelvDocNo.Text = "<span class='FormLabels'><b>   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  Delivery Doc. No. :" + thisOOBD.OBDNumber + "</b><span><br/><br/><img src=\" ../mInbound/Code39Handler.ashx?code=" + thisOOBD.OBDNumber + "\" />";
                ltDelvDocDetails.Text = "<table cellpadding='2' cellspacing='1' border='0' >";
                ltDelvDocDetails.Text += "<tr><td class='FormLabels' align='left' >Customer :</td><td class='FormLabels' align='left'>" + CommonLogic.GetCustomerName(thisOOBD.CustomerID.ToString()) + "</td></tr>";
                ltDelvDocDetails.Text += "<tr><td class='FormLabels' align='left'>Requested By  :</td><td class='FormLabels' align='left'>" + CommonLogic.GetUserName(thisOOBD.RequestedByID.ToString()) + "</td></tr>";
                ltDelvDocDetails.Text += "<tr><td class='FormLabels' align='left'>Delv. Doc. Date :</td><td class='FormLabels' align='left'>" + thisOOBD.OBDDate + "</td></tr>";
                ltDelvDocDetails.Text += "</table>";


                DesignLogic.SetInnerPageSubHeading(this.Page, "Delivery Pick Note");
                //string[] rolesAllowedInThisPage = CommonLogic.GetRolesAllowed("1,2,4,5,7,17");

                //if (!cp.IsInAnyRoles(rolesAllowedInThisPage))
                //{
                //    Response.Redirect("Login.aspx?eid=6");
                //}
                LoadData();

            }
        }

        private void LoadData(string mcode = "", bool isrestriction = false)
        {

            if (!IsPostBack)
            {
                LoadAssemblyData();
            }
        }


        private void LoadAssemblyData()
        {
            try
            {
                CommonCriteria criteria = new CommonCriteria();
                criteria.OUTBOUNDID = Convert.ToInt32(CommonLogic.QueryString("obdid"));

                if (txtMCode.Text == "" || txtMCode.Text == "Search Part Number...")
                    criteria.MATERIALCODE = "";
                else
                    criteria.MATERIALCODE = txtMCode.Text;

                object DBResult = DAOController.GetDataFromStoredProcedure(DRLExecuteCode.DELIVERY_PICK_NOTE, new DBCriteria().DELIVERY_PICK_NOTE(criteria));
                AvailableStockData availStock = (AvailableStockData)DBResult;
                AvailableStockData serviceResult = null;

                if (chkRestickQty.Checked)
                    serviceResult = (AvailableStockData)WMSController.Process(ServiceCall.OBD_RESTRICTED_PICKNOTE, availStock);
                else
                    serviceResult = (AvailableStockData)WMSController.Process(ServiceCall.OBD_SUGGESTED_PICKNOTE, availStock);

                MSP = serviceResult.MSP;
                if (!IsPostBack)
                    LoadgridHeader();
                gvOptimalPickPath.DataSource = serviceResult.suggestedStockData;
                gvOptimalPickPath.DataBind();

            }
            catch (ServiceException se)
            {
                resetError(se.getStackTraceInfo(), true);
            }

        }


        protected void resetError(string error, bool isError)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);
        }

        protected void lnkMCodeSearch_Click(object sender, EventArgs e)
        {
            LoadAssemblyData();
        }

        protected void gvOptimalPickPath_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
            }
            else
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    SuggestedStockInfo suggestedStockInfo = (SuggestedStockInfo)e.Row.DataItem;
                    Literal ltDynamicMsps = (Literal)e.Row.FindControl("ltDynamicMsps");

                    ltDynamicMsps.Text = DynamicMsps(suggestedStockInfo.MSPS, suggestedStockInfo.IsMatchedMSP);
                    if (!suggestedStockInfo.IsMatchedMSP)
                    {
                        ((Label)e.Row.FindControl("ltLocation")).CssClass = "Strikeoff";
                        ((Label)e.Row.FindControl("ltAvailbleQty")).CssClass = "Strikeoff";

                        //HyperLink hlPicklink=(HyperLink)e.Row.FindControl("hlPicklink");
                        //hlPicklink.Text = "";
                        //hlPicklink.CssClass = "";
                    }
                }
            if (e.Row.RowType == DataControlRowType.Footer)
            {

            }
        }


        private String DynamicMsps(IList<MSP> MSPS, bool suggestedStockInfo)
        {
            StringBuilder tableMsps = new StringBuilder();
            foreach (KeyValuePair<int, MSP> msp in MSP)
            {
                bool Itemfound = false;
                if (MSPS != null)
                    foreach (MSP listmsp in MSPS)
                    {
                        if (listmsp.MSPSysId == msp.Value.MSPSysId)
                        {
                            Itemfound = true;
                            if (!suggestedStockInfo)
                                tableMsps.Append("<td class='Strikeoff'>" + listmsp.NativeValue + "</td>");
                            else
                                tableMsps.Append("<td >" + listmsp.NativeValue + "</td>");
                            break;
                        }
                    }
                if (!Itemfound)
                    tableMsps.Append("<td></td>");
            }
            return tableMsps.ToString();
        }

        public string LoadHeader()
        {
            RowDataHeader.Append("<table border='1'cellspacing=\"1\" cellpadding=\"1\"  width='100%'><tr>");
            RowDataHeader.Append("<th width=\"50px\">Line No.</th>");
            RowDataHeader.Append("<th width=\"324px\">Part Number</th>");
            RowDataHeader.Append("<th width=\"90px\">SUoM / Qty.</th>");
            RowDataHeader.Append("<th width=\"90px\">Del. Doc. Qty.</th>");
            RowDataHeader.Append("<th width=\"50px\">Kit ID</th>");
            RowDataHeader.Append(GetDynamicHeader());// Dynamic Msps
            RowDataHeader.Append("<th width=\"80px\">Picked Qty.</th>");
            RowDataHeader.Append("<th width=\"80px\">Avl.Qty.</th>");

            if (chkRestickQty.Checked)
                RowDataHeader.Append("<th width=\"100px\">Pickable Qty.</th>");

            RowDataHeader.Append("<th width=\"60px\">Dam.</th>");
            RowDataHeader.Append("<th width=\"60px\">Dis.</th>");
            RowDataHeader.Append("<th width=\"60px\"></th>");
            RowDataHeader.Append("</tr></table>");
            return RowDataHeader.ToString();
        }



        private string GetDynamicHeader()
        {
            StringBuilder DynamicMsps = new StringBuilder();
            foreach (KeyValuePair<int, string> EachMsp in soMSPs)
            {
                DynamicMsps.Append("<th width=\"130px\">" + EachMsp.Value + "</th>");
            }
            return DynamicMsps.ToString();
        }



        private string GetDynamicMSPValues(string materialMSPs, ref bool isExpire, bool isStrikeoff = false)
        {
            if (soMSPs == null)
                return "";
            StringBuilder DynamicMsps = new StringBuilder();
            IDictionary<int, string> materialmsplist = new Dictionary<int, string>();
            foreach (string eachmsp in materialMSPs.Split(',').Where(x => x.Length != 0))
            {
                string[] mspIDValues = eachmsp.Split('|');
                materialmsplist.Add(Convert.ToInt32("0" + mspIDValues[0]), mspIDValues[1]);
            }
            foreach (KeyValuePair<int, string> EachMsp in soMSPs)
            {

                DynamicMsps.Append("<td width=\"130px\">");
                if (materialmsplist.ContainsKey(EachMsp.Key))
                {
                    if (isStrikeoff)
                    {
                        DynamicMsps.Append("<span class=\"Strikeoff\">" + materialmsplist[EachMsp.Key] + "</span>");
                    }
                    else
                    {
                        DynamicMsps.Append(materialmsplist[EachMsp.Key]);
                    }
                }
                DynamicMsps.Append("</td>");
            }
            if (materialmsplist != null && materialmsplist.ContainsKey(2)) //2 is for Expire Date
            {
                string ExpireDate = materialmsplist[2];
                if (DateTime.ParseExact(ExpireDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).CompareTo(DateTime.Now) < 0)
                {
                    isExpire = false;
                }
            }
            return DynamicMsps.ToString();
        }



        protected void chkRestickQty_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
                gvOptimalPickPath.Columns[11].Visible = true;
            else
                gvOptimalPickPath.Columns[11].Visible = false;
            LoadAssemblyData();
        }



        private void LoadgridHeader()
        {
            StringBuilder gridHeader = new StringBuilder();
            StringBuilder gridFooter = new StringBuilder();
            foreach (KeyValuePair<int, MSP> msp in MSP)
            {
                gridHeader.Append("<th>" + msp.Value.DisplayName + "</th>");
                gridFooter.Append("<th></th>");
            }
            gvOptimalPickPath.Columns[6].HeaderText += gridHeader.ToString();
            gvOptimalPickPath.Columns[6].FooterText += gridFooter.ToString();
        }
        
    }
}