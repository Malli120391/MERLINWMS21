using MRLWMSC21Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21.mOutbound.OptimalPickNote;
using System.Text;

namespace MRLWMSC21.mOutbound
{
    public partial class OptimalPickPath : System.Web.UI.Page
    {
        CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        protected String OBDNumber;
        protected OBDTrack thisOOBD;
        public IDictionary<int, SOLineItemData> soInfo;
        public IDictionary<int, string> soMSPs;
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
                string[] rolesAllowedInThisPage = CommonLogic.GetRolesAllowed("1,2,4,5,7,17");

                if (!cp.IsInAnyRoles(rolesAllowedInThisPage))
                {
                    Response.Redirect("Login.aspx?eid=6");
                }
                LoadData();

            }
        }

        private void LoadData(string mcode="",bool isrestriction=false)
        {
            OptimalPickNote.OptimalPickNote optimalPickNote = new OptimalPickNote.OptimalPickNote(Convert.ToInt16(CommonLogic.QueryStringUSInt("obdid")), isrestriction);
            IDictionary<string, IList<RawLiveStock>> dataList = optimalPickNote.getOptimalPickList(mcode);
            soInfo = optimalPickNote.getSOInfo();
            soMSPs = optimalPickNote.getSOMSPs();
            //var dat=dataList.Values.ToList();
            gvOptimalPickPath.Columns[1].HeaderText = LoadHeader();
            gvOptimalPickPath.DataSource = dataList;
            gvOptimalPickPath.DataBind();
            tdDDRPrintArea.Width = (soMSPs.Count*120)+1300+"px";
            
        }

        protected void lnkMCodeSearch_Click(object sender, EventArgs e)
        {
            String MCode = (txtMCode.Text != "Search Part Number..." ? txtMCode.Text : "");
            LoadData(MCode,chkRestickQty.Checked);
        }

        protected void gvOptimalPickPath_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
            }
            else
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                //ItemDetails pendingDetails = (ItemDetails)e.Row.DataItem;
                KeyValuePair<string, IList<RawLiveStock>> pendingDetails = (KeyValuePair<string, IList<RawLiveStock>>)e.Row.DataItem;
                ((Literal)e.Row.FindControl("ltLocation")).Text = pendingDetails.Key;
                
                GridView gvRowliveStock = (GridView)e.Row.FindControl("gvRowliveStock");
                int columnsCount = gvRowliveStock.Columns.Count;
                if(chkRestickQty.Checked)
                    gvRowliveStock.Columns[columnsCount - 4].Visible = true;
                else
                    gvRowliveStock.Columns[columnsCount - 4].Visible = false;
                gvRowliveStock.Columns[4].HeaderText+=GetDynamicHeader();
                gvRowliveStock.DataSource = pendingDetails.Value;
                gvRowliveStock.DataBind();
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {

            }
        }
        public string LoadHeader()
        {
            RowDataHeader.Append("<table border='1'cellspacing=\"1\" cellpadding=\"1\"  width='100%'><tr>");
            RowDataHeader.Append("<th width=\"50px\">Line No.</th>");
            RowDataHeader.Append("<th width=\"324px\">Part Number</th>");
            RowDataHeader.Append("<th width=\"90px\">SUoM / Qty.</th>");
            RowDataHeader.Append("<th width=\"90px\">Del. Doc. Qty.</th>");
            RowDataHeader.Append("<th width=\"50px\">Kit ID</th>");
            //RowDataHeader.Append("<th width=\"40px\">HU</th>");           
            RowDataHeader.Append(GetDynamicHeader());// Dynamic Msps
            RowDataHeader.Append("<th width=\"80px\">Picked Qty.</th>");
            RowDataHeader.Append("<th width=\"80px\">Avl. Qty.</th>");            
            if(chkRestickQty.Checked)
            RowDataHeader.Append("<th width=\"100px\">Pickable Qty.</th>");
            
            RowDataHeader.Append("<th width=\"60px\">Dam.</th>");
            RowDataHeader.Append("<th width=\"60px\">Dis.</th>");
            RowDataHeader.Append("<th width=\"60px\"></th>");
            RowDataHeader.Append("</tr></table>");
            return RowDataHeader.ToString();

        }

        protected void gvRowliveStock_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                RawLiveStock RawLiveStock = (RawLiveStock)e.Row.DataItem;

                bool isStrikeoff=false;
                if (!RawLiveStock.isMatchedMSP || RawLiveStock.IsDamaged == 1 && RawLiveStock.HasDiscrepency == 1)
                {
                    isStrikeoff = true;
                }

                ((Literal)e.Row.FindControl("ltMcode")).Text = RawLiveStock.Mcode + " <span style=\"font-size:10pt; color:#888888\">" + RawLiveStock.MDesc + "</span>" + "<br/><img height='30px' width=\"300px\" src=\"../mInbound/Code39Handler.ashx?code=" + RawLiveStock.Mcode + "\"/><br />";
                bool isExpire = true;
                if (soInfo.ContainsKey(RawLiveStock.SODetailsID))
                {
                    SOLineItemData soLineItemData = soInfo[RawLiveStock.SODetailsID];
                    ((Literal)e.Row.FindControl("ltSuomQty")).Text = soLineItemData.SO_UOM + " / " + soLineItemData.SO_UOM_QTY;
                    ((Literal)e.Row.FindControl("ltDelDocQty")).Text = soLineItemData.SO_QTY.ToString();
                    Literal ltDynamicMsp = (Literal)e.Row.FindControl("ltDynamicMsp");
                    ((Label)e.Row.FindControl("ltLineno")).Text = soLineItemData.LineNo.ToString();
                    
                    ltDynamicMsp.Text = GetDynamicMSPValues(RawLiveStock.MSPValues, ref isExpire, isStrikeoff);
                }
                //Label ltLineno = (Label)e.Row.FindControl("ltLineno");
                //Label ltHandlingUnit = (Label)e.Row.FindControl("ltHandlingUnit");
                Label ltAvailbleQty=(Label)e.Row.FindControl("ltAvailbleQty");
                Label lbKitID = (Label)e.Row.FindControl("lbKitID");
                Label ltPickedQty=(Label)e.Row.FindControl("ltPickedQty");
                Label ltPickableQty = (Label)e.Row.FindControl("ltPickableQty");
                HyperLink hlPicklink = (HyperLink)e.Row.FindControl("hlPicklink");
                ((Literal)e.Row.FindControl("ltDamage")).Text = (RawLiveStock.IsDamaged == 0 ? "" : "<img src=\"../Images/blue_menu_icons/check_mark.png\"/>");
                ((Literal)e.Row.FindControl("ltDisc")).Text = (RawLiveStock.HasDiscrepency == 0 ? "" : "<img src=\"../Images/blue_menu_icons/check_mark.png\"/>");
                if (isStrikeoff)
                {
                    //ltHandlingUnit.CssClass = "Strikeoff";
                    ltAvailbleQty.CssClass = "Strikeoff";
                    ltPickedQty.CssClass = "Strikeoff";
                    ltPickableQty.CssClass = "Strikeoff";
                    lbKitID.CssClass = "Strikeoff";
                    hlPicklink.CssClass = "";
                }
                else 
                {
                    if (RawLiveStock.IsDamaged == 0 && RawLiveStock.HasDiscrepency == 0 && isExpire)
                   {
                        //hlPicklink.NavigateUrl = "../mInventory/PickItem.aspx?so=" + RawLiveStock.SODetailsID + "&gd=" + RawLiveStock.GoodsMovementID + "&hd=" + RawLiveStock.HUBox + "&mm=" + RawLiveStock.MaterialID + "&loc=" + RawLiveStock.LocationID;
                       hlPicklink.NavigateUrl = "../mInventory/PickItem.aspx?soid=" + soInfo[RawLiveStock.SODetailsID].SOID + "&mmid=" + RawLiveStock.MaterialID + "&obdid=" + CommonLogic.QueryString("obdid") + "&LineNum=" + soInfo[RawLiveStock.SODetailsID].LineNo + "&locid=" + RawLiveStock.LocationID + "&kitid=" + (RawLiveStock.KitPlannerID == 0 ? "NULL" : RawLiveStock.KitPlannerID.ToString());
                        hlPicklink.Text = "Pick";

                   } else
                       hlPicklink.CssClass = "";
                }
                //ltHandlingUnit.Text = (RawLiveStock.NoOfUnits==1?"":RawLiveStock.HUBox + " / " + RawLiveStock.NoOfUnits);
                
                //((Literal)e.Row.FindControl("ltSerial")).Text = RawLiveStock.serialNo;
                //((Literal)e.Row.FindControl("ltBatch")).Text = RawLiveStock.BatchNo;
                
                ltPickedQty.Text = RawLiveStock.PickedQty.ToString("0.00");
                ltPickableQty.Text = RawLiveStock.pickableQty.ToString("0.00");
                ltAvailbleQty.Text = RawLiveStock.AvailQty.ToString("0.00");//Quantity
                lbKitID.Text = (RawLiveStock.KitPlannerID==0?"":RawLiveStock.KitPlannerID.ToString());
            }
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
            foreach(string eachmsp in materialMSPs.Split(',').Where(x=>x.Length!=0))
            {
                string []mspIDValues = eachmsp.Split('|');
                materialmsplist.Add(Convert.ToInt32("0"+mspIDValues[0]), mspIDValues[1]);
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
            if (materialmsplist!=null && materialmsplist.ContainsKey(2)) //2 is for Expire Date
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
            String MCode = (txtMCode.Text != "Search Part Number..." ? txtMCode.Text : "");
            LoadData(MCode, chkRestickQty.Checked);
            
        }
    }
}