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

namespace MRLWMSC21.mMaterialManagement
{
    public partial class KitPlannerRequest : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        public static CustomPrincipal cp1 = HttpContext.Current.User as CustomPrincipal;
        public static string Headerid ="";
        protected void Page_Load(object sender, EventArgs e)
        {
            txtLength.Enabled = false;
            txtWidth.Enabled = false;
            txtHeight.Enabled = false;
            txtWeight.Enabled = false;
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "Kit Planner Request");
                LoadDropDown(ddlKitType, "select KitTypeID,KitType from MMT_KitType where IsActive=1 and IsDeleted=0", "KitType", "KitTypeID", "Select Kit Type");
                hdnkpid.Value = "0";
                
               
            }
            if (CommonLogic.QueryString("kpid") != "")
            {
                hdnkpid.Value = CommonLogic.QueryString("kpid").ToString();

                lnkCreatekit.Text = "Update" + CommonLogic.btnfaUpdate;
                FormData();
                txtPartNo.Enabled = false;
                txtKitTenant.Enabled = false;
                ddlKitType.Enabled = false;
                Headerid = CommonLogic.QueryString("kpid").ToString();
                if (ddlKitType.SelectedValue == "2")
                {
                    //txtPartNo.Enabled = false;
                    //txtLength.Enabled = true;
                    //txtWidth.Enabled = true;
                    //txtHeight.Enabled = true;
                    //txtWeight.Enabled = true;
                    txtPartNo.Enabled = true;
                    txtLength.Enabled = false;
                    txtWidth.Enabled = false;
                    txtHeight.Enabled = false;
                    txtWeight.Enabled = false;
                    lnkCreatekit.Visible = true;
                    //phuom.Visible = true;
                   // phqty.Visible = true;
                    //txtSupplier.Enabled = false;
                }
                else
                {
                    txtPartNo.Enabled = true;
                    txtLength.Enabled = false;
                    txtWidth.Enabled = false;
                    txtHeight.Enabled = false;
                    txtWeight.Enabled = false;
                    lnkCreatekit.Visible = false;
                   // phuom.Visible = false;
                    //phqty.Visible = false;
                    //txtSupplier.Enabled = true;
                }

            }
            else
            {
                Headerid = "0";
            }



            }
        private void FormData()
        {
            DataSet ds = DB.GetDS("EXEC sp_MMT_GetKitHeaderDetails @KitPlannerId=" + CommonLogic.QueryString("kpid"), false);
            DataTable dtPrimaryInfo = ds.Tables[0];

            if (dtPrimaryInfo.Rows.Count > 0)
            {
                txtKitCode.Text= dtPrimaryInfo.Rows[0]["KitCode"].ToString();
                txtSupplier.Text = dtPrimaryInfo.Rows[0]["supplier"].ToString();
                hifTenant.Value= dtPrimaryInfo.Rows[0]["TenantID"].ToString();
                txtKitTenant.Text= dtPrimaryInfo.Rows[0]["TenantName"].ToString();
                txtPartNo.Text= dtPrimaryInfo.Rows[0]["mcode"].ToString();
                hifMMID.Value= dtPrimaryInfo.Rows[0]["ParentMaterialMasterID"].ToString();
                ddlKitType.SelectedValue= dtPrimaryInfo.Rows[0]["KitTypeID"].ToString();
                txtLength.Text = dtPrimaryInfo.Rows[0]["MLength"].ToString();
                txtWeight.Text = dtPrimaryInfo.Rows[0]["MWeight"].ToString();
                txtWidth.Text = dtPrimaryInfo.Rows[0]["MWidth"].ToString();
                txtHeight.Text = dtPrimaryInfo.Rows[0]["MHeight"].ToString();

            }
            }
       
        protected void lnkCreatekit_Click(object sender, EventArgs e)
        {
            cp = HttpContext.Current.User as CustomPrincipal;
            int KitPlannerID = 0;
            if (CommonLogic.QueryString("kpid") != "")
            {
                KitPlannerID = Convert.ToInt32(Request.QueryString["kpid"].ToString());
            }
            if(hifTenant.Value.ToString()=="0")
            {
                resetError("Please select Tenant", true);
                return;
            }
            if (txtKitTenant.Text.ToString().Trim() == "")
            {
                resetError("Please select Tenant", true);
                return;
            }
            if(ddlKitType.SelectedValue.ToString()=="0")
            {
                resetError("Please select Kit Type", true);
                return;
            }

            if(Convert.ToInt32(ddlKitType.SelectedValue.ToString())==1)
            {
                if (hifMMID.Value.ToString() == "0")
                {
                    resetError("Please select Part No.", true);
                    return;
                }
                if (txtPartNo.Text.ToString().Trim() == "")
                {
                    resetError("Please select Part No.", true);
                    return;
                }
            }
            //if (hifsupplier.Value.ToString() == "0")
            //{
            //    resetError("Please select Supplier", true);
            //    return;
            //}
            //if (txtSupplier.Text.ToString().Trim() == "")
            //{
            //    resetError("Please select Supplier", true);
            //    return;
            //}
            //else if (Convert.ToInt32(ddlKitType.SelectedValue.ToString()) == 2)
            //{
            //    if (txtLength.Text.ToString().Trim() == "")
            //    {
            //        resetError("Please Enter Length", true);
            //        return;
            //    }
            //    if (txtLength.Text.ToString().Trim() == "0")
            //    {
            //        resetError("Please Enter valid Length", true);
            //        return;
            //    }
            //    if (txtWeight.Text.ToString().Trim() == "")
            //    {
            //        resetError("Please Enter weight", true);
            //        return;
            //    }
            //    if (txtWeight.Text.ToString().Trim() == "0")
            //    {
            //        resetError("Please Enter valid weight", true);
            //        return;
            //    }
            //    if (txtHeight.Text.ToString().Trim() == "")
            //    {
            //        resetError("Please Enter Height", true);
            //        return;
            //    }
            //    if (txtHeight.Text.ToString().Trim() == "0")
            //    {
            //        resetError("Please Enter valid Height", true);
            //        return;
            //    }
            //    if (txtWidth.Text.ToString().Trim() == "")
            //    {
            //        resetError("Please Enter Width", true);
            //        return;
            //    }
            //    if (txtWidth.Text.ToString().Trim() == "0")
            //    {
            //        resetError("Please Enter valid Width", true);
            //        return;
            //    }
            //}
            StringBuilder Query = new StringBuilder();
            Query.Append(" Declare @KpId int ");
            Query.Append ( "EXEC [sp_MMT_UpsertKitPlanner]");
            Query.Append("@KitPlannerID="+KitPlannerID);
            Query.Append(",@ParentMaterialMasterID="+hifMMID.Value);
            Query.Append(",@SupplierId=" + hifsupplier.Value.ToString());
            Query.Append(",@TenantID="+hifTenant.Value);
            Query.Append(",@CreatedBy="+cp.UserID.ToString());
            Query.Append(",@KitTypeID="+ddlKitType.SelectedValue);
            Query.Append(" ,@MLength  =" + (txtLength.Text == "" ? "NULL" : DB.SQuote(txtLength.Text)));
            Query.Append(" ,@MHeight  =" + (txtHeight.Text == "" ? "NULL" : DB.SQuote(txtHeight.Text)));
            Query.Append(" ,@MWidth  =" + (txtWidth.Text == "" ? "NULL" : DB.SQuote(txtWidth.Text)));
            Query.Append(" ,@MWeight  =" + (txtWeight.Text == "" ? "NULL" : DB.SQuote(txtWeight.Text)));
            Query.Append(",@AccountID="+cp.AccountID.ToString());
            //Query.Append(" @KpId=@KpId output select @KpId n");


            try
            {
                int kpid = 0;
                kpid = DB.GetSqlN(Query.ToString());
                if (lnkCreatekit.Text == "Update" + CommonLogic.btnfaUpdate)
                {
                    Response.Redirect("KitPlannerRequest.aspx?kpid=" +kpid + "&status=Updatesuccess", false);
                }
                else
                {
                    Response.Redirect("KitPlannerRequest.aspx?kpid=" + kpid + "&status=Createsuccess", false);
                   
                }

            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

        public void LoadDropDown(DropDownList ddllist, String SqlQry, String strListName, String strListValue, string defaultValue)
        {
            // Initially clear all DropDownList Items
            ddllist.Items.Clear();
            ddllist.Items.Add(new ListItem(defaultValue, "0")); // Add Default value to the dropdown

            IDataReader rsList = DB.GetRS(SqlQry);

            while (rsList.Read())
            {

                ddllist.Items.Add(new ListItem(rsList[strListName].ToString(), rsList[strListValue].ToString()));

            }

            rsList.Close();
        }

        protected void ddlKitType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlKitType.SelectedValue == "2")
            {
                //txtPartNo.Enabled = false;
                //txtLength.Enabled = true;
                //txtWidth.Enabled = true;
                //txtHeight.Enabled = true;
                //txtWeight.Enabled = true;
                txtPartNo.Enabled = true;
                txtLength.Enabled = false;
                txtWidth.Enabled = false;
                txtHeight.Enabled = false;
                txtWeight.Enabled = false;
                //phuom.Visible = true;
                //phqty.Visible = true;
                txtPartNo.Text = "";
                hifMMID.Value = "0";
                //txtSupplier.Enabled = false;
            }
            else
            {
                txtPartNo.Enabled = true;
                txtLength.Enabled = false;
                txtWidth.Enabled = false;
                txtHeight.Enabled = false;
                txtWeight.Enabled = false;
                //phuom.Visible = false;
                //phqty.Visible = false;
                txtPartNo.Text = "";
                hifMMID.Value = "0";
                //txtSupplier.Enabled = true;
            }
        }
        


        [WebMethod]
        public static string GetKitDetailsList()
        {
            if(Headerid!="")
            {
                DataTable dt = DB.GetDS("EXEC [dbo].[sp_MMT_GetKitPlannerChildList] @KitPlannerID=" + Headerid, false).Tables[0];

                return JsonConvert.SerializeObject(dt);
            }
            else
            {
                return "";
            }
           
        }

        [WebMethod]
        public static int SetKitDetails(int kpid, int DetailID, int MatID, Decimal Qty)
        {
            try
            {
                DB.ExecuteSQL("EXEC [dbo].[sp_MMT_SetKitPlannerChild] @KitPlannerID=" + kpid + ",@DetailID=" + DetailID + ", @MatID=" + MatID + ",@Quantity=" + Qty);
                return 1;
            }
            catch {
                return 0;
            }
            
        }
        [WebMethod]
        public static int UpsertKitData(string KitPartNo, string Quantity,string PUOM,string Duom,string detailsid)
        {
            DataSet ds=DB.GetDS("select count(*) as N from ORD_PODetails where isdeleted=0 and isactive=1 and KitPlannerID=" + Headerid + ";select count(*) as N from ORD_SODetails where isdeleted=0 and isactive=1 and KitPlannerID=" + Headerid ,false);
            if(Convert.ToInt32(ds.Tables[0].Rows[0]["N"])>1)
            {
                return -3;
            }
            if (Convert.ToInt32(ds.Tables[1].Rows[0]["N"]) > 1)
            {
                return -4;
            }
            int vaule= DB.GetSqlN("exec [dbo].[sp_MMT_SetKitPlannerChild] @KitPlannerID="+Headerid+",@DetailID="+ detailsid + ",@MatID=" + KitPartNo + ",@Quantity=" + Quantity + ",@UoMID=" + PUOM + ",@UserID=" + cp1.UserID);
            if(detailsid=="0" )
            {
                if(vaule>0)
                {
                    return 1;
                }
                else {
                    return -1;
                }
            }
            else
            {
                if (vaule > 0)
                {
                    return 2;
                }
                else
                {
                    return -1;
                }
            }
        }
        [WebMethod]
        public static int DeleteChildItems(string kpid)
        {
            try
            {
                DataSet ds = DB.GetDS("select count(*) as N from ORD_PODetails where isdeleted=0 and isactive=1 and KitPlannerID=" + Headerid + ";select count(*) as N from ORD_SODetails where isdeleted=0 and isactive=1 and KitPlannerID=" + Headerid, false);
                if (Convert.ToInt32(ds.Tables[0].Rows[0]["N"]) > 1)
                {
                    return -3;
                }
                if (Convert.ToInt32(ds.Tables[1].Rows[0]["N"]) > 1)
                {
                    return -4;
                }
                DB.ExecuteSQL("delete from    MMT_KitPlannerDetail where KitPlannerDetailID=" + kpid);
                return 1;
            }
            catch(Exception e)
            {
                return -1;
            }
            
            
        }
            protected void resetError(string error, bool isError)
        {

            /*string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
            if (isError)
                str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

            if (error.Length > 0)
                str += error + "";
            else
                str = "";


            lblStatus.Text = str;*/
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);

        }


    }
}