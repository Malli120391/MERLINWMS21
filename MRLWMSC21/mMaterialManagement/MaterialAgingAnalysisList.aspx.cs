using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using MRLWMSC21Common;


namespace MRLWMSC21.mMaterialManagement
{
    public partial class MaterialAgingAnalysisList : System.Web.UI.Page
    {

        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;


        protected void page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "MasterData";
        }



        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {

                /*
                if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("Bill of Material"))))
                {
                    Response.Redirect("../Login.aspx?eid=6");
                }*/

                DesignLogic.SetInnerPageSubHeading(this.Page, "Ageing Analysis");

                BuilExpDataDropDown(ddlAge);

                ViewState["AgingSqlList"] = "[dbo].[sp_INV_GetActiveStockListWithAging] @MaterialMasterID=0,@NonConformity=0,@Damaged=0,@Discepancy=0,@Location ='_______',@Tenant=1,@GRNNumber='',@Replenishment=0,@MaterialStorageParameters= '',@MaterialStorageParameterValues ='',@age="+DB.SQuote(DateTime.Now.ToString("dd/MM/yyyy"));

                gvMAgingList_buildGridData(gvMAgingList_buildGridData());

            }


        }


        private void BuilExpDataDropDown(DropDownList ddlExpDate)
        {
            ddlExpDate.Items.Add(new ListItem("All", ""));
            DateTime dtNow = DateTime.Now;
            ddlExpDate.Items.Add(new ListItem("In 0 Days", dtNow.ToString("dd/MM/yyyy")));
            DateTime dt30day = DateTime.Now.AddDays(30);
            ddlExpDate.Items.Add(new ListItem("In 30 Days", dt30day.ToString("dd/MM/yyyy")));
            DateTime dt60day = DateTime.Now.AddDays(60);
            ddlExpDate.Items.Add(new ListItem("In 60 Days", dt60day.ToString("dd/MM/yyyy")));
            DateTime dt90day = DateTime.Now.AddDays(90);
            ddlExpDate.Items.Add(new ListItem("In 90 Days", dt90day.ToString("dd/MM/yyyy")));
            DateTime dt6thmonth = DateTime.Now.AddMonths(6);
            ddlExpDate.Items.Add(new ListItem("In 6 Months", dt6thmonth.ToString("dd/MM/yyyy")));
            DateTime dt1year = DateTime.Now.AddYears(1);
            ddlExpDate.Items.Add(new ListItem("In 1 Year", dt1year.ToString("dd/MM/yyyy")));
        }




        protected DataSet gvMAgingList_buildGridData()
        {
            string sql = ViewState["AgingSqlList"].ToString();

            DataSet ds = DB.GetDS(sql, false);



            if (ds.Tables[0].Rows.Count == 0)
            {
                //lblDeliveryRecCount.InnerText = "";
            }
            else
            {
                // lblDeliveryRecCount.InnerHtml = "&nbsp;&nbsp;&nbsp;  [" + ds.Tables[0].Rows.Count.ToString() + "]";
            }

            return ds;

        }

        protected void gvMAgingList_buildGridData(DataSet ds)
        {
            gvMAgingList.DataSource = ds;
            gvMAgingList.DataBind();
            ds.Dispose();
        }



        protected void gvMAgingList_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gvMAgingList_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gvMAgingList_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void gvMAgingList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            gvMAgingList.PageIndex = e.NewPageIndex;
            gvMAgingList.EditIndex = -1;
            this.gvMAgingList_buildGridData(this.gvMAgingList_buildGridData());

        }



        protected void lnkIssueSearch_Click(object sender, EventArgs e)
        {

            if (atcEditMCode.Text == "Search Part# ...")
            {
                atcEditMCode.Text = "";
                hifMaterialCode.Value = "";
            }

            ViewState["AgingSqlList"] = "[dbo].[sp_INV_GetActiveStockListWithAging] @MaterialMasterID=" + (hifMaterialCode.Value == "" ? "0" : hifMaterialCode.Value) + ",@NonConformity=0,@Damaged=0,@Discepancy=0,@Location ='_______',@Tenant=1,@GRNNumber='',@Replenishment=0,@MaterialStorageParameters= '',@MaterialStorageParameterValues ='',@age=" + DB.SQuote(ddlAge.SelectedValue);

           

            DataSet ds = DB.GetDS(ViewState["AgingSqlList"].ToString(), false);

            if (ds.Tables[0].Rows.Count == 0)
            {
                resetError("No material is available", true);
            }


            atcEditMCode.Text = "Search Part# ...";
            hifMaterialCode.Value = "";

            this.gvMAgingList_buildGridData(this.gvMAgingList_buildGridData());
        }


        protected void resetError(string error, bool isError)
        {

            /* string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
             if (isError)
                 str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

             if (error.Length > 0)
                 str += error + "";
             else
                 str = "";


             lblGroupLabelStatus.Text = str;*/
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);

        }
    }
}