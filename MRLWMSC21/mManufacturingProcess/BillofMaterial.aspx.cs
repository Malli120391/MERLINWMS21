using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using MRLWMSC21Common;
using System.Globalization;
using System.Configuration;



// Module Name : Manufacturing
// Usecase Ref.: BOM Details_UC_006
// DevelopedBy : Naresh P
// CreatedOn   : 05/05/2014
// Modified On : 24/03/2015



namespace MRLWMSC21.mManufacturingProcess
{
    public partial class BillofMaterial : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        

        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Inbound";

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("Bill of Material"))))
            {
                Response.Redirect("../Login.aspx?eid=6");
            }

            if (CommonLogic.QueryString("status") == "success")
              {

                 resetMainError( "New revision successfully created",false);
              }

            if (CommonLogic.QueryString("bomid") != "" && CommonLogic.QueryString("upStatus") == "success")
              {

                  resetMainError("Successfully Updated", false);
              }



            



            

            if (!IsPostBack)
            {
                

                DesignLogic.SetInnerPageSubHeading(this.Page, "Bill of Material");

                ViewState["HeaderID"] = 0;



                if (CommonLogic.QueryString("bomid") != "")
                {
                    hifBOMHeaderID.Value = CommonLogic.QueryString("bomid");
                    lnkCopyBoMDetails.Visible = true;
                    imgbtngvBoMlineItems.Visible = true;
                    Build_FormData();
                    txtbomrefno.Enabled = false;
                    txtRevision.Enabled = false;
                    LoadTreeViewData();
                    pnlBOMTreeView.Visible = true;
                    imgbtngvBOMList.Visible = true;
                    lblTotalItems.Visible = true;
                    lnkClear.Visible = false;
                    lnkUpdate.Text = "Update" + CommonLogic.btnfaUpdate;
                    
                }
                else
                {
                    imgbtngvBoMlineItems.Visible = false;
                    Build_CreationFrom();
                    pnlBOMTreeView.Visible = false;
                    imgbtngvBOMList.Visible = false;

                    lblTotalItems.Visible = false;

                    lnkUpdate.Text = "Create BOM" + CommonLogic.btnfaSave;
                }


            }




        }

        private void Build_CreationFrom()
        {

           chkIsActive.Checked = true;
            lnkAddNewLineItem.Visible = false;
            
           // tdlineitems.Visible = false;
            
           
        }

        private void Build_FormData()
        {
            String BoMHeaderID= CommonLogic.QueryString("bomid").ToString();
            String cMdHeaderDetials = "EXEC [dbo].[sp_MFG_GetBoMHeaderData]  @BOMHeaderID=" + BoMHeaderID;
            IDataReader rsHeaderDetials = DB.GetRS(cMdHeaderDetials);
            if (rsHeaderDetials.Read())
            {
                ViewState["HeaderID"] = DB.RSFieldInt(rsHeaderDetials, "BOMHeaderID");
                //txtbomrefno.Text = DB.RSField(rsHeaderDetials, "BOMRefNumber");
                hifbomtype.Value= DB.RSFieldInt(rsHeaderDetials, "BOMTypeID").ToString();
                atcBOMType.Text = DB.RSField(rsHeaderDetials, "BOMType");
              
                // hifMaterialMaster.Value = DB.RSFieldInt(rsHeaderDetials, "MaterialMasterID").ToString();
                //atcMaterialMaster.Text= DB.RSField(rsHeaderDetials, "MCode");


                int MMRvID = DB.GetSqlN("select MaterialMasterRevisionID AS N from MFG_BoMHeader_Revision where IsActive=1 AND IsDeleted=0 AND BOMHeaderID=" + CommonLogic.QueryString("bomid"));

                txtRevision.Text = DB.GetSqlS("select Revision AS S from MFG_BoMHeader_Revision where IsActive=1 AND IsDeleted=0 AND BOMHeaderID=" + CommonLogic.QueryString("bomid") + "AND MaterialMasterRevisionID=" + MMRvID );

                

                hidBomRef.Value = MMRvID.ToString();

                txtbomrefno.Text = DB.GetSqlS("select MM.MCode+' - '+MMR.Revision   AS S  from MMT_MaterialMaster_Revision MMR JOIN MMT_MaterialMaster MM ON MMR.MaterialMasterID=MM.MaterialMasterID  AND MM.IsActive=1 AND MM.IsDeleted=0 JOIN MMT_MType MT ON MT.MTypeID=MM.MTypeID where  MMR.MaterialMasterRevisionID=" + MMRvID);

              //  lblAssemblyName.Text = txtbomrefno.Text + "/" + txtRevision.Text;

               

               // hifRoutingHeader.Value = DB.RSFieldInt(rsHeaderDetials, "RoutingHeaderID").ToString();
               // atcRoutingHeader.Text= DB.RSField(rsHeaderDetials, "RoutingRefNo");
                txtProductName.Text = DB.RSField(rsHeaderDetials, "ProductName");
                lblEffectiveDate.Text = DB.RSFieldDateTime(rsHeaderDetials, "EffectiveDate").ToString("dd/MM/yyyy");

                txtBOMRevRemarks.Text = DB.RSField(rsHeaderDetials, "Remarks");
             //   txtProductQuantity.Text = DB.RSFieldDecimal(rsHeaderDetials, "ProductQuantity").ToString();
                hifPROUoM_qty.Value = DB.RSFieldInt(rsHeaderDetials, "MaterialMaster_PROUoMID").ToString();
                atcPROUoM_qty.Text = DB.RSField(rsHeaderDetials, "PROUoM") + "/" + DB.RSFieldDecimal(rsHeaderDetials, "PROUoMQty").ToString();
                chkIsActive.Checked = Convert.ToBoolean(DB.RSFieldTinyInt(rsHeaderDetials, "IsActive"));
                chkIsDeleted.Checked = Convert.ToBoolean(DB.RSFieldTinyInt(rsHeaderDetials, "IsDeleted"));
                ViewState["cMdBoMlist"] = "[dbo].[sp_MFG_GetBoMDetailsList] @BOMHeaderID=" + ViewState["HeaderID"];
                Buildgrid_BoMLinenumber(Buildgrid_BoMLinenumber());
                
                lnkUpdate.Text = "Update";
                //IbutNewPRONumer.Enabled = false;


                //check job order release

                /*
                if (DB.GetSqlN("select ProductionOrderHeaderID AS N from MFG_ProductionOrderHeader where IsActive=1 AND IsDeleted=0 AND BOMHeaderRevisionID=" + DB.GetSqlN("select BOMHeaderRevisionID AS N from MFG_BoMHeader_Revision where IsActive=1 AND IsDeleted=0 AND BOMHeaderID=" + CommonLogic.QueryString("bomid"))) != 0)
                {
                    chkIsActive.Enabled = false;
                    chkIsDeleted.Enabled = false;
                }*/



            }
            rsHeaderDetials.Close();
        }

        protected void lnkClear_Click(object sender, EventArgs e)
        {
            if (lnkUpdate.Text == "Update")
            {
                Response.Redirect("BillofMaterial.aspx?bomid=" + ViewState["HeaderID"].ToString());

            }
            else
            {
                Response.Redirect("BillofMaterialList.aspx");
            }
        }

        protected void lnkUpdate_Click(object sender, EventArgs e)
        {
            
            Page.Validate("save");
            
            
            if (!Page.IsValid)
            {
                resetError("Please check for mandatory fields", true);
                return;
            }

            // Previous Query  select top 1 MFG_RDAC.RoutingDetailsActivityCaptureID AS N from MFG_ProductionOrderHeader MFG_POH JOIN MFG_RoutingHeader_Revision MFG_RHRv ON MFG_RHRv.RoutingHeaderRevisionID=MFG_POH.RoutingHeaderRevisionID AND  MFG_RHRv.IsActive=1 AND MFG_RHRv.IsDeleted=0  JOIN MFG_BOMHeader_Revision MFG_BHRv ON MFG_BHRv.BOMHeaderRevisionID=MFG_RHRv.BOMHeaderRevisionID AND MFG_BHRv.IsActive=1 AND MFG_BHRv.IsDeleted=0 JOIN MFG_RoutingDetailsActivityCapture MFG_RDAC ON MFG_RDAC.ProductionOrderHeaderID=MFG_POH.ProductionOrderHeaderID AND MFG_RDAC.isDeleted=0  where MFG_POH.ProductionOrderStatusID IN (2,3,4,5)  AND  MFG_POH.IsActive=1 AND MFG_POH.IsDeleted=0  AND   MFG_BHRv.BOMHeaderID=
            if((CommonLogic.QueryString("bomid") == "" ? "0" : CommonLogic.QueryString("bomid"))!="0")
            {
                if (DB.GetSqlN("EXEC [dbo].[sp_MFG_CheckForBOMRoutingEdit]  @BOMHeaderID=" + (CommonLogic.QueryString("bomid") == "" ? "0" : CommonLogic.QueryString("bomid"))) != 0)
                {
                    //resetError("Production process is started, cannot add/modify BOM details", true);
                    resetError("Cannot add/modify BOM details, as 'BOM' is mapped to job order", true);
                    return;
                }
            }

            
            /*
            Page.Validate();
            if (!Page.IsValid)
            {
                resetError("Please check mandatory fields", true);
                return;
            }*/

            String vMMRvID = Request.Form[hidBomRef.UniqueID].ToString(); //DB.GetSqlN("select MaterialMasterID AS N from MMT_MaterialMaster where IsActive=1 AND IsDeleted=0 AND MCode="+DB.SQuote(txtRoutingRefNo.Text));


            if (CommonLogic.QueryString("bomid") != "") {

                if (chkIsDeleted.Checked || !chkIsActive.Checked) {

                    if (DB.GetSqlN("select RHR.BOMHeaderRevisionID AS N from MFG_RoutingHeader_Revision RHR JOIN MFG_BOMHeader_Revision BHR ON BHR.BOMHeaderRevisionID=RHR.BOMHeaderRevisionID AND BHR.IsActive=1 AND BHR.IsDeleted=0 where RHR.IsActive=1 AND RHR.IsDeleted=0 AND BHR.BOMHeaderID="+CommonLogic.QueryString("bomid")) != 0)
                    {
                        resetError("Production process is started, cannot delete/modify BOM details",true);
                        return;
                    }

                }

            }

            if (vMMRvID == "")
            {
                resetError("Invalid Part #", true);
                return;
            }


            if (DB.GetSqlN("select MM.MTypeID AS N from MMT_MaterialMaster_Revision MMR JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=MMR.MaterialMasterID where MMR.MaterialMasterRevisionID=" + vMMRvID) == 8)
            {
                if (hifbomtype.Value != "7")
                {
                    resetError("BOM type for sub-assembly should be 'Sets/Phantoms'", true);
                    return;
                }
            }
            else {
                if (hifbomtype.Value == "7")
                {
                    resetError("Invalid BOM type", true);
                    return;
                }
            }
            

            if (DB.GetSqlN("select top 1 MaterialMaster_UoMID AS N from MMT_MaterialMaster_GEN_UoM where MaterialMasterID=" + DB.GetSqlN("select MaterialMasterID AS N from MMT_MaterialMaster_Revision where IsActive=1 AND IsDeleted=0 AND MaterialMasterRevisionID=" + vMMRvID) + " AND MaterialMaster_UoMID =" + (hifPROUoM_qty.Value == "" ? "0" : hifPROUoM_qty.Value)) == 0)
            {
                
                this.resetError("UoM does not exist for the material", true);
                return;
            }


            StringBuilder cMdUpdatePROHeaderDetials = new StringBuilder();
            cMdUpdatePROHeaderDetials.Append("Declare @NewHeaderID int  ");
            cMdUpdatePROHeaderDetials.Append("EXEC [dbo].[sp_MFG_UpsertBoMHeader] ");
            cMdUpdatePROHeaderDetials.Append("@BOMHeaderID=" + ViewState["HeaderID"].ToString());
            cMdUpdatePROHeaderDetials.Append(",@BOMRefNumber=NULL");
            cMdUpdatePROHeaderDetials.Append(",@BOMTypeID=" + hifbomtype.Value);
            cMdUpdatePROHeaderDetials.Append(",@MaterialMasterID=NULL");

            cMdUpdatePROHeaderDetials.Append(",@MaterialMasterRevisionID="+vMMRvID);
            cMdUpdatePROHeaderDetials.Append(",@Revision=" + DB.SQuote(txtRevision.Text));

            cMdUpdatePROHeaderDetials.Append(",@ProductName=" + DB.SQuote(txtProductName.Text.Trim()));
            cMdUpdatePROHeaderDetials.Append(",@Remarks=" + DB.SQuote(txtBOMRevRemarks.Text.Trim()));
            //cMdUpdatePROHeaderDetials.Append(",@ProductQuantity=" + txtProductQuantity.Text.Trim());
            cMdUpdatePROHeaderDetials.Append(",@ProductQuantity=NULL");
            cMdUpdatePROHeaderDetials.Append(",@MaterialMaster_PROUoMID=" + hifPROUoM_qty.Value);
            cMdUpdatePROHeaderDetials.Append(",@TenantID=" + cp.TenantID);
            cMdUpdatePROHeaderDetials.Append(",@CreatedBy=" + cp.UserID);
            cMdUpdatePROHeaderDetials.Append(",@LastModifiedBy=" + cp.UserID);
            cMdUpdatePROHeaderDetials.Append(",@IsActive="+Convert.ToInt16(chkIsActive.Checked));
            cMdUpdatePROHeaderDetials.Append(",@IsDeleted="+Convert.ToInt16(chkIsDeleted.Checked));
            cMdUpdatePROHeaderDetials.Append(",@NewBOMHeaderID=@NewHeaderID OUTPUT select @NewHeaderID as N");
            Boolean IsDeleted = chkIsDeleted.Checked;


            try
            {
                ViewState["HeaderID"] = DB.GetSqlN(cMdUpdatePROHeaderDetials.ToString());

                lnkUpdate.Text = "Update";
                


                if (IsDeleted==true)
                {
                    Response.Redirect("BillofMaterialList.aspx");
                }
                lnkAddNewLineItem.Visible = true;
               // tdlineitems.Visible = true;
                //IbutNewPRONumer.Enabled = false;
                

                Response.Redirect("BillofMaterial.aspx?bomid=" + ViewState["HeaderID"].ToString() + "&upStatus=success");
                resetError("Successfully Updated", false);
            }
            catch (SqlException sqlex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), sqlex.Source, sqlex.Message, sqlex.StackTrace);
                if (sqlex.ErrorCode == -2146232060)
                {
                    resetError("BOM Ref.# already exists", true);
                    return;
                }
                resetError("Error while updating BOM details", true);
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while updating BOM details",true);
                return;
            }
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


        protected void resetMainError(string error, bool isError)
        {
            
            string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
            if (isError)
                str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

            if (error.Length > 0)
                str += error + "";
            else
                str = "";
            lblStatus.Text = str;

          //  ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);


        }


        #region ---------- BOM Line Items ----------

        private DataSet Buildgrid_BoMLinenumber()
        {
            String cMdBoMlist= ViewState["cMdBoMlist"].ToString();
            DataSet dsBoMlist = DB.GetDS(cMdBoMlist, false);
            return dsBoMlist;
        }

        private void Buildgrid_BoMLinenumber(DataSet dsBoMlist)
        {
            gvBoMlineItems.DataSource = dsBoMlist;
            gvBoMlineItems.DataBind();
            dsBoMlist.Dispose();
        }

        protected void lnkAddNewLineItem_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["cMdBoMlist"] = "[dbo].[sp_MFG_GetBoMDetailsList] @BOMHeaderID=" + ViewState["HeaderID"];
                DataSet dsBoMlist = Buildgrid_BoMLinenumber();
                DataRow NewRow = dsBoMlist.Tables[0].NewRow();

                int recentlinenumber = DB.GetSqlN("select top 1 BOMLineNumber as N from MFG_BOMDetails where IsDeleted=0 and BOMHeaderID="+ViewState["HeaderID"]+" order by BOMLineNumber desc");
                NewRow["BOMDetailsID"] = 0;
                NewRow["BOMLineNumber"] = ++recentlinenumber;
                dsBoMlist.Tables[0].Rows.InsertAt(NewRow,0);
                gvBoMlineItems.EditIndex = 0;
                gvBoMlineItems.PageIndex = 0;
                Buildgrid_BoMLinenumber(dsBoMlist);
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }

        protected void gvBoMlineItems_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvBoMlineItems.EditIndex = e.NewEditIndex;
            Buildgrid_BoMLinenumber(Buildgrid_BoMLinenumber());
        }

        protected void gvBoMlineItems_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Page.Validate("UpdateGridItems");
            if (!IsValid)
                return;
            GridViewRow editRow= gvBoMlineItems.Rows[e.RowIndex];
            Literal ltbomDetailsID = (Literal)editRow.FindControl("ltbomDetailsID");
            Literal ltBoMLineNumber = (Literal)editRow.FindControl("ltBoMLineNumber");
            TextBox atcMaterialCode = (TextBox)editRow.FindControl("atcMaterialCode");
            HiddenField hifMaterialCode = (HiddenField)editRow.FindControl("hifMaterialCode");
            TextBox atcBOMUoM_Qty = (TextBox)editRow.FindControl("atcBOMUoM_Qty");
            HiddenField hifBOMUoM_Qty = (HiddenField)editRow.FindControl("hifBOMUoM_Qty");
            TextBox atcWorkCenterGroup = (TextBox)editRow.FindControl("atcWorkCenterGroup");
            HiddenField hifWorkCenterGroup = (HiddenField)editRow.FindControl("hifWorkCenterGroup");
            TextBox txtbomQuantity = (TextBox)editRow.FindControl("txtbomQuantity");
            StringBuilder cmdUpdateBoMDetails = new StringBuilder();


            int vMMID = DB.GetSqlN("select MaterialMasterID as N from MMT_MaterialMaster where isactive=1 AND isdeleted=0 AND MCode=" + DB.SQuote(atcMaterialCode.Text));

            if (vMMID == 0)
            {
                this.resetError("This material does not exist", true);
                return;

            }


            if (DB.GetSqlN("select top 1 MaterialMaster_UoMID AS N from MMT_MaterialMaster_GEN_UoM where IsActive=1 AND IsDeleted=0 AND MaterialMasterID=" + vMMID + " AND MaterialMaster_UoMID =" + (hifBOMUoM_Qty.Value == "" ? "0" : hifBOMUoM_Qty.Value)) == 0)
            {
                this.resetError("UoM does not exist for the material", true);
                return;
            }



            string vQuantity = txtbomQuantity.Text;

            if (vQuantity == "0" || vQuantity == "0.0" || vQuantity == "0.00" || vQuantity == "")
            {
                resetError("Invalid quantity", true);
                return;
            }

            cmdUpdateBoMDetails.Append("EXEC [dbo].[sp_MFG_UpsertBoMDetails] ");
            cmdUpdateBoMDetails.Append("@BOMDetailsID="+ltbomDetailsID.Text);
            cmdUpdateBoMDetails.Append(",@BOMHeaderID=" +ViewState["HeaderID"] );
            cmdUpdateBoMDetails.Append(",@BOMLineNumber=" +ltBoMLineNumber.Text );
            cmdUpdateBoMDetails.Append(",@BOMMaterialMasterID=" + vMMID);
            cmdUpdateBoMDetails.Append(",@MaterialMaster_BOMUoMID=" +hifBOMUoM_Qty.Value );
            cmdUpdateBoMDetails.Append(",@BOMQuantity=" +txtbomQuantity.Text );
            cmdUpdateBoMDetails.Append(",@CreatedBy=" +cp.UserID );
            try
            {
                DB.ExecuteSQL(cmdUpdateBoMDetails.ToString());
                gvBoMlineItems.EditIndex = -1;
                Buildgrid_BoMLinenumber(Buildgrid_BoMLinenumber());
                resetError("Successfully Updated", false);
            }
            catch (SqlException sqlex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), sqlex.Source, sqlex.Message, sqlex.StackTrace);

                if (sqlex.ErrorCode == -2146232060)
                {
                    this.resetError("This line item is already updated", true);
                    return;
                }
            }
            catch(Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while updating BOM line items",true);
            }


           
        }

        protected void gvBoMlineItems_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvBoMlineItems.EditIndex =-1;
            Buildgrid_BoMLinenumber(Buildgrid_BoMLinenumber());
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            int rowCount = gvBoMlineItems.Rows.Count;
            GridViewRow row;
            String BoMDetailsIDs = "";
            for (int rowNo = 0; rowNo < rowCount; rowNo++)
            {
                row = gvBoMlineItems.Rows[rowNo];
                if (gvBoMlineItems.EditIndex != rowNo)
                {
                    if (((CheckBox)row.FindControl("chkisdeleted")).Checked)
                    {
                        BoMDetailsIDs += ((Literal)row.FindControl("ltbomDetailsID")).Text + ",";
                        String MMID=((Literal)row.FindControl("ltMMID")).Text ;

                        if (DB.GetSqlN("select RHRv.RoutingHeaderRevisionID AS N from MFG_RoutingHeader_Revision RHRv JOIN MFG_BOMHeader_Revision BHRv ON BHRv.BOMHeaderRevisionID=RHRv.BOMHeaderRevisionID AND BHRv.IsActive=1 AND BHRv.IsDeleted=0 JOIN MFG_RoutingDetails RD ON RD.RoutingHeaderID=RHRv.RoutingHeaderID AND RD.IsActive=1 AND RD.IsDeleted=0 JOIN MFG_RoutingDetailsActivity RDA ON RDA.RoutingDetailsID=RD.RoutingDetailsID AND RDA.IsActive=1 AND RDA.IsDeleted=0 JOIN MFG_RoutingDetailsActivity_MaterilaMaster RDAM ON RDAM.RoutingDetailsActivityID=RDA.RoutingDetailsActivityID AND RDAM.IsActive=1 AND RDAM.IsDeleted=0 where RHRv.IsActive=1 AND RHRv.IsDeleted=0 AND BHRv.BOMHeaderID=" + CommonLogic.QueryString("bomid") + " AND RDAM.MaterialMasterID=" + MMID) != 0)
                        {
                            resetError("Cannot modify/delete, as material is configured in routing",true);
                            return;
                        }


                    }
                }
            }

            try
            {
                DB.ExecuteSQL("Exec [dbo].[sp_MFG_DeleteBOMDetails] @BOMDetailsIDs=" + DB.SQuote(BoMDetailsIDs));
                gvBoMlineItems.EditIndex = -1;
                Buildgrid_BoMLinenumber(Buildgrid_BoMLinenumber());
                resetError("Successfully deleted the selected line items", false);
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while deleting selected line items", true);
            }
        }

        #endregion ---------- BOM Line Items ----------

        #region ----------  New Revision -------------

        protected void lnkCreateNewRevision_Click(object sender, EventArgs e)
        {

            int NewBoMID = 0;
            try
            {


                



                if (txtNewRevision.Text == "" || txtRevRemarks.Text == "" || txtEffectedDate.Text=="")
                {
                    resetError("Please check for mandatory fields",true);
                    return;
                }




                String OldRevision = DB.GetSqlS("select Revision S from MFG_BOMHeader_Revision where MaterialMasterRevisionID=" + hidBomRef.Value + " AND IsActive=1 AND IsDeleted=0 order by Revision  desc");


                int compareValue = 0;

                compareValue = String.Compare(txtNewRevision.Text, OldRevision, StringComparison.Ordinal);

                if (compareValue <= 0)
                {
                    resetError("'New Revision' must be greater than the previous revision. Previous revision:" + OldRevision, true);
                    return;
                }

                string CurrDate = DateTime.Now.ToString("dd/MM/yyyy");

                //DateTime.ParseExact(txtEffectiveDate.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture), DateTime.ParseExact(CurrDate, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                //DateTime.ParseExact(txtEffectedDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture),DateTime.Now
                if( !DateGreaterOrEqual(DateTime.ParseExact(txtEffectedDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture), DateTime.ParseExact(CurrDate, "dd/MM/yyyy", CultureInfo.InvariantCulture))){
                    resetError("Effective date should be greater than or equal to system date", true);
                    return;
                }
                

                if (OldRevision != "")
                {
                    //!DateGreaterOrEqual(DateTime.ParseExact(txtEffectiveDate.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture), DateTime.ParseExact(DB.GetSqlS("select top 1  convert(nvarchar(50),EffectiveDate,103)  AS S from MMT_MaterialMaster_Revision where IsActive=1 AND IsDeleted=0 AND MaterialMasterID=" + CommonLogic.QueryString("mid") + " order by Revision desc"), "dd/MM/yyyy", CultureInfo.InvariantCulture))

                    if (!DateGreaterOrEqual(DateTime.ParseExact(txtEffectedDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture), DateTime.ParseExact(DB.GetSqlS("select convert(nvarchar(50),EffectiveDate,103) S from MFG_BOMHeader_Revision where MaterialMasterRevisionID=" + hidBomRef.Value + " AND IsActive=1 AND IsDeleted=0 order by Revision  desc"), "dd/MM/yyyy", CultureInfo.InvariantCulture)))
                    {
                        resetError("Effective date should be greater than previous revision effective date", true);
                        return;
                    }
                }



                if (DB.GetSqlN("select top 1 BOML.MaterialMasterRevisionID AS N from MMT_MaterialMaster_Revision MMRv JOIN MFG_BOMHeader_Revision BHRv ON BHRv.MaterialMasterRevisionID=MMRv.MaterialMasterRevisionID AND BHRv.IsActive=1 AND BHRv.IsDeleted=0 join MMT_MaterialMaster_Revision MMRL ON MMRL.MaterialMasterID=MMRv.MaterialMasterID AND MMRL.IsDeleted=0 and MMRL.IsActive=1 left join MFG_BOMHeader_Revision BOML ON BOML.MaterialMasterRevisionID=MMRL.MaterialMasterRevisionID where MMRv.IsActive=1 AND MMRv.IsDeleted=0 AND BHRv.BOMHeaderID=" + ViewState["HeaderID"] + "  order by MMRL.Revision desc") != 0)
                {
                    resetError("Create 'New Revision for this Finished Good' to clone BOM",  true);
                    return;
                }

                StringBuilder cmdUpdateRoutingHeaderDetails = new StringBuilder();
                cmdUpdateRoutingHeaderDetails.Append("DECLARE @NewHeaderID int ");
                cmdUpdateRoutingHeaderDetails.Append("Exec [dbo].[sp_MFG_CopyBOMDetails] ");
                cmdUpdateRoutingHeaderDetails.Append("@BOMHeaderID=" + ViewState["HeaderID"]);
                cmdUpdateRoutingHeaderDetails.Append(",@Revision='" + txtNewRevision.Text + "'");
                //cmdUpdateRoutingHeaderDetails.Append(",@EffectiveDate='" + (DateTime.ParseExact(txtEffectedDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture)).Date + "'");
                cmdUpdateRoutingHeaderDetails.Append(",@EffectiveDate=" + (txtEffectedDate.Text.Trim() != "" ? DB.SQuote(DateTime.ParseExact(txtEffectedDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy")) : "NULL"));
                

                cmdUpdateRoutingHeaderDetails.Append(",@Remarks='" + txtRevRemarks.Text + "'");
                cmdUpdateRoutingHeaderDetails.Append(",@CreatedBy=" + cp.UserID);
                cmdUpdateRoutingHeaderDetails.Append(",@NewBOMHeaderID=@NewHeaderID OUTPUT select @NewHeaderID as N");

                if (DB.GetSqlN("select BOMV.BOMHeaderRevisionID AS N from MFG_BOMHeader_Revision BOMV  join MMT_MaterialMaster_Revision MMV on MMV.MaterialMasterRevisionID=BOMV.MaterialMasterRevisionID and MMV.IsActive=1 and MMV.IsDeleted=0 join MMT_MaterialMaster MM ON MM.MaterialMasterID=MMV.MaterialMasterID where BOMV.IsDeleted=0 AND BOMV.BOMHeaderID=" + ViewState["HeaderID"] + " AND  BOMV.Revision='" + txtNewRevision.Text + "'") != 0)
                {
                    resetError("Revision already exists", true);
                }

                 NewBoMID = DB.GetSqlN(cmdUpdateRoutingHeaderDetails.ToString());

                
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while creating new revision", true);
            }

            if (NewBoMID != 0)
            {

                Response.Redirect("BillofMaterial.aspx?bomid=" + NewBoMID + "&status=success");
            }

        }


        private bool DateGreaterOrEqual(DateTime dt1, DateTime dt2)
        {
            return DateTime.Compare(dt1.Date, dt2.Date) >= 0;
        }


        #endregion ----------  New Revision -------------

        protected void imgbtngvBoMlineItems_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                gvBoMlineItems.AllowPaging = false;
                String[] hiddencolumns = { "Delete","" };
                Buildgrid_BoMLinenumber(Buildgrid_BoMLinenumber());
                CommonLogic.ExporttoExcel(gvBoMlineItems, "BOM Line Items", hiddencolumns);
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while exporting data", true);
            }

        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

      

        #region ---------- Structured BOM Details ----------


        protected void lnkAddChildItem_Click(object sender, EventArgs e)
        {

        }

        protected void lnkButCancel_Click(object sender, EventArgs e)
        {

            txtMType.Text = "";
            txtChildMMCodeRev.Text = "";
            txtChildMMCode.Text = "";
            txtParentMMCode.Text = "";
            txtChildMMCodeRev.Text = "";
            txtQuantity.Text = "";
            txtRTPartNumber.Text = "";
            txtUoM.Text = "";
            txtMMRevUoM.Text = "";
            txtRTUoM.Text = "";

            chkBDDelete.Checked = false;

        }

        protected void lnkButUpdate_Click(object sender, EventArgs e)
        {
            StringBuilder cmdUpdateBoMDetails = new StringBuilder();

            Page.Validate("updateBOMItems");

            if (!Page.IsValid)
            {
                resetError("Please check for mandatory fields", true);
                return;
            }


            


            try
            {

                //if (DB.GetSqlN("select top 1 MFG_RDAC.RoutingDetailsActivityCaptureID AS N from MFG_ProductionOrderHeader MFG_POH JOIN MFG_RoutingHeader_Revision MFG_RHRv ON MFG_RHRv.RoutingHeaderRevisionID=MFG_POH.RoutingHeaderRevisionID AND  MFG_RHRv.IsActive=1 AND MFG_RHRv.IsDeleted=0  JOIN MFG_BOMHeader_Revision MFG_BHRv ON MFG_BHRv.BOMHeaderRevisionID=MFG_RHRv.BOMHeaderRevisionID AND MFG_BHRv.IsActive=1 AND MFG_BHRv.IsDeleted=0 JOIN MFG_RoutingDetailsActivityCapture MFG_RDAC ON MFG_RDAC.ProductionOrderHeaderID=MFG_POH.ProductionOrderHeaderID AND MFG_RDAC.isDeleted=0  where MFG_POH.ProductionOrderStatusID IN (2,3,4,5)  AND  MFG_POH.IsActive=1 AND MFG_POH.IsDeleted=0  AND   MFG_BHRv.BOMHeaderID=" + (CommonLogic.QueryString("bomid") == "" ? "0" : CommonLogic.QueryString("bomid"))) != 0)
                //{
                //    resetError("Production process is started, cannot add/modify BOM details", true);
                //    return;
                //}

                
                if (DB.GetSqlN("EXEC [dbo].[sp_MFG_CheckForBOMRoutingEdit]  @BOMHeaderID=" + (CommonLogic.QueryString("bomid") == "" ? "0" : CommonLogic.QueryString("bomid"))) != 0)
                {
                    resetError("Cannot add/modify BOM details, as BOM is mapped to job order", true);
                    return;
                }



                String MType = DB.GetSqlN("select MTypeID AS N from MMT_MType where IsActive=1 AND IsDeleted=0 AND MType=left('" + txtMType.Text + "',2)").ToString();

                int MTypeID = Convert.ToInt32(MType == "0" ? "0" : MType);
                if (MTypeID == 0)
                {
                    resetError("Please select MType", true);
                    return;
                }


                if (DB.GetSqlN("select RHRv.RoutingHeaderRevisionID AS N from MFG_RoutingHeader_Revision RHRv JOIN MFG_BOMHeader_Revision BHRv ON BHRv.BOMHeaderRevisionID=RHRv.BOMHeaderRevisionID AND BHRv.IsActive=1 AND BHRv.IsDeleted=0 JOIN MFG_RoutingDetails RD ON RD.RoutingHeaderID=RHRv.RoutingHeaderID AND RD.IsActive=1 AND RD.IsDeleted=0 JOIN MFG_RoutingDetailsActivity RDA ON RDA.RoutingDetailsID=RD.RoutingDetailsID AND RDA.IsActive=1 AND RDA.IsDeleted=0 JOIN MFG_RoutingDetailsActivity_MaterilaMaster RDAM ON RDAM.RoutingDetailsActivityID=RDA.RoutingDetailsActivityID AND RDAM.IsActive=1 AND RDAM.IsDeleted=0 where RHRv.IsActive=1 AND RHRv.IsDeleted=0 AND BHRv.BOMHeaderID=" + CommonLogic.QueryString("bomid") + " AND RDAM.MaterialMasterID=" +  (ViewState["BOMMMID"] ==null? "0": ViewState["BOMMMID"].ToString())) != 0)
                {
                    resetError("Cannot modify, as this material is configured in routing", true);
                    return;
                }
                

                if (chkBDDelete.Checked) {


                    CheckifBOMItemsinRouting(MTypeID);

                    LoadTreeViewData();

                 

                    return;
                }

                String BOMDetailsID = hifBOMDetailsID.Value;

                if (MTypeID == 9) {
                    resetError("Sorry only one Finished Good for BOM", true);
                    return;
                }
                
                int vMMID = 0;

                int ParentUOMID = 0;

                if (MTypeID != 7)
                {

                    vMMID = DB.GetSqlN("select MM_R.MaterialMasterID AS N from MMT_MaterialMaster MM JOIN MMT_MaterialMaster_Revision MM_R ON MM_R.MaterialMasterID=MM.MaterialMasterID AND MM_R.IsActive=1 AND MM_R.IsDeleted=0 where MM.IsActive=1 AND MM.isdeleted=0   AND MM.MTypeID IN (8,9) AND   MM.MCode +' - '+ MM_R.Revision='" + txtParentMMCode.Text + "'");
                    
                    int sMTypeID= DB.GetSqlN("select MTypeID AS N from MMT_MaterialMaster where IsActive=1 AND IsDeleted=0 AND MaterialMasterID=" + vMMID);

                    if ( sMTypeID == 0 || sMTypeID == 7 )
                    {
                        this.resetError("Invalid parent part number", true);
                        return;
                    }


                    if (vMMID == 0)
                    {
                        this.resetError("Material does not exist", true);
                        return;

                    }

                    ParentUOMID = DB.GetSqlN("select MaterialMaster_UoMID AS N from MMT_MaterialMaster_GEN_UoM where IsActive=1 AND IsDeleted=0 AND MaterialMasterID=" + vMMID + "  AND UoMTypeID=1");

                    if (ParentUOMID == 0)
                    {
                        this.resetError("UoM does not exist for this material", true);
                        return;
                    }
                }
                else {

                    //vMMID = DB.GetSqlN("select MaterialMasterID as N from MMT_MaterialMaster where isactive=1 AND isdeleted=0 AND MCode=" + DB.SQuote(txtChildMMCode.Text));

                    vMMID = DB.GetSqlN("select MaterialMasterID as N from MMT_MaterialMaster where isactive=1 AND isdeleted=0 AND MCode=" + DB.SQuote(txtRTPartNumber.Text));


                    if (vMMID == 0)
                    {
                        this.resetError("This Material does not exist", true);
                        return;

                    }


                    if (DB.GetSqlN("select top 1 MaterialMaster_UoMID AS N from MMT_MaterialMaster_GEN_UoM where IsActive=1 AND IsDeleted=0 AND MaterialMasterID=" + vMMID + " AND MaterialMaster_UoMID =" + (hifRTUoMID.Value == "" ? "0" : hifRTUoMID.Value)) == 0)         //(hifTvBOMUoM.Value == "" ? "0" : hifTvBOMUoM.Value)) == 0)
                    {
                        this.resetError("UoM does not exist for this material", true);
                        return;
                    }

                }


                decimal vQuantity = Convert.ToDecimal(txtQuantity.Text == "" ? "0" : txtQuantity.Text);

                if (vQuantity <=0)
                {
                    resetError("Invalid quantity", true);
                    return;
                }

                


                cmdUpdateBoMDetails.Append("EXEC [dbo].[sp_MFG_UpsertBoMDetailsForPhantomKit] ");
                
                cmdUpdateBoMDetails.Append("@BOMHeaderID=" + ViewState["HeaderID"]);
                cmdUpdateBoMDetails.Append(",@BOMLineNumber=NULL" );

                if(hifBOMDetailsID.Value=="")
                    cmdUpdateBoMDetails.Append(",@BOMDetailsID=" + 0);
                else
                    cmdUpdateBoMDetails.Append(",@BOMDetailsID=" + (hifBOMDetailsID.Value == "" ? "NULL" : hifBOMDetailsID.Value));

                String ParentBMMID = DB.GetSqlN("select MM_R.MaterialMasterID AS N from MMT_MaterialMaster MM JOIN MMT_MaterialMaster_Revision MM_R ON MM_R.MaterialMasterID=MM.MaterialMasterID AND MM_R.IsActive=1 AND MM_R.IsDeleted=0 where MM.IsActive=1 AND MM.isdeleted=0   AND MM.MTypeID IN (8,9) AND   MM.MCode +' - '+ MM_R.Revision='" + txtParentMMCode.Text + "'").ToString();

                if ( ParentBMMID == "0" || DB.GetSqlN("select MTypeID AS N from MMT_MaterialMaster where IsActive=1 AND IsDeleted=0 AND MaterialMasterID=" + ParentBMMID) == 7 )
                {
                    this.resetError("Invalid parent part number", true);
                    return;
                }



                if (MTypeID != 7)
                {

                        String BMMID = DB.GetSqlN("select MM_R.MaterialMasterID AS N from MMT_MaterialMaster MM JOIN MMT_MaterialMaster_Revision MM_R ON MM_R.MaterialMasterID=MM.MaterialMasterID AND MM_R.IsActive=1 AND MM_R.IsDeleted=0 where MM.IsActive=1 AND MM.isdeleted=0   AND MM.MTypeID IN (8,9) AND   MM.MCode +' - '+ MM_R.Revision='" + txtRTPartNumber.Text + "'").ToString();



                        String MMRevisionID = DB.GetSqlN("select MM_R.MaterialMasterRevisionID AS N from MMT_MaterialMaster MM JOIN MMT_MaterialMaster_Revision MM_R ON MM_R.MaterialMasterID=MM.MaterialMasterID AND MM_R.IsActive=1 AND MM_R.IsDeleted=0 where MM.IsActive=1 AND MM.isdeleted=0   AND MM.MTypeID IN (8,9) AND   MM.MCode +' - '+ MM_R.Revision='" + txtRTPartNumber.Text + "'").ToString();

                       // String ParentBMMID = DB.GetSqlN("select MaterialMasterID AS N from MMT_MaterialMaster_Revision where IsActive=1 AND IsDeleted=0 AND MaterialMasterRevisionID=" + (hifTvBOMParentPartNo.Value == "" ? "0" : hifTvBOMParentPartNo.Value)).ToString();
                        cmdUpdateBoMDetails.Append(",@BOMMaterialMasterID=" + (BMMID == "0" ? "NULL" : BMMID));
                        cmdUpdateBoMDetails.Append(",@MaterialMaster_BOMUoMID=" + (ParentUOMID.ToString() == "0" ? "NULL" : ParentUOMID.ToString()));
                        cmdUpdateBoMDetails.Append(",@MaterialMasterRevisionID=" + (MMRevisionID == "0" ? "NULL" : MMRevisionID));
                        cmdUpdateBoMDetails.Append(",@ParentMaterialMasterRevisionID=" + (ParentBMMID == "0" ? "0" : ParentBMMID));

                }
                else {
                    
                        cmdUpdateBoMDetails.Append(",@BOMMaterialMasterID=" + vMMID);
                       // cmdUpdateBoMDetails.Append(",@MaterialMaster_BOMUoMID=" + (hifTvBOMUoM.Value == "" ? "NULL" : hifTvBOMUoM.Value));
                        cmdUpdateBoMDetails.Append(",@MaterialMaster_BOMUoMID=" + (hifRTUoMID.Value == "" ? "NULL" : hifRTUoMID.Value));
                        cmdUpdateBoMDetails.Append(",@MaterialMasterRevisionID=NULL");
                        cmdUpdateBoMDetails.Append(",@ParentMaterialMasterRevisionID=" + (ParentBMMID == "0" ? "0" : ParentBMMID));

                }

                cmdUpdateBoMDetails.Append(",@BOMQuantity=" + vQuantity);
                cmdUpdateBoMDetails.Append(",@CreatedBy=" + cp.UserID);

                DB.ExecuteSQL(cmdUpdateBoMDetails.ToString());

                LoadTreeViewData();

                resetError("Successfully Updated", false);

                LoadTreeViewData();
                
            }
            catch (SqlException sqlex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), sqlex.Source, sqlex.Message, sqlex.StackTrace);
                if (sqlex.ErrorCode == -2146232060)
                {
                    this.resetError("This line item is already updated", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while updating BOM line items", true);
                return;
            }

        }


        public void CheckifBOMItemsinRouting(int MTypeID)
        {
            try
            {
                if (ViewState["BOMDetailsID"] == null)
                {
                    return;
                }

                if (DB.GetSqlN("select RHRv.RoutingHeaderRevisionID AS N from MFG_RoutingHeader_Revision RHRv JOIN MFG_BOMHeader_Revision BHRv ON BHRv.BOMHeaderRevisionID=RHRv.BOMHeaderRevisionID AND BHRv.IsActive=1 AND BHRv.IsDeleted=0 JOIN MFG_RoutingDetails RD ON RD.RoutingHeaderID=RHRv.RoutingHeaderID AND RD.IsActive=1 AND RD.IsDeleted=0 JOIN MFG_RoutingDetailsActivity RDA ON RDA.RoutingDetailsID=RD.RoutingDetailsID AND RDA.IsActive=1 AND RDA.IsDeleted=0 JOIN MFG_RoutingDetailsActivity_MaterilaMaster RDAM ON RDAM.RoutingDetailsActivityID=RDA.RoutingDetailsActivityID AND RDAM.IsActive=1 AND RDAM.IsDeleted=0 where RHRv.IsActive=1 AND RHRv.IsDeleted=0 AND BHRv.BOMHeaderID=" + CommonLogic.QueryString("bomid") + " AND RDAM.MaterialMasterID=" + ViewState["BOMMMID"].ToString()) == 0)
                {

                    if (MTypeID == 8)
                    {
                        //String BMMID = DB.GetSqlN("select MM_R.MaterialMasterID AS N from MMT_MaterialMaster MM JOIN MMT_MaterialMaster_Revision MM_R ON MM_R.MaterialMasterID=MM.MaterialMasterID AND MM_R.IsActive=1 AND MM_R.IsDeleted=0 where MM.IsActive=1 AND MM.isdeleted=0   AND MM.MTypeID IN (8,9) AND   MM.MCode +' - '+ MM_R.Revision='" + txtChildMMCode.Text + "'").ToString();

                        DB.ExecuteSQL(" delete from MFG_BOMDetails where BOMHeaderID=" + CommonLogic.QueryString("bomid") + " AND ( ParentMaterialMasterID=" + ViewState["BOMMMID"].ToString() + " OR BOMMaterialMasterID=" + ViewState["BOMMMID"].ToString() + " ) AND ParentMaterialMasterID<>0");
                    }
                    else if (MTypeID == 9)
                    {
                        DB.ExecuteSQL(" delete from MFG_BOMDetails where BOMHeaderID=" + CommonLogic.QueryString("bomid") + " AND ParentMaterialMasterID<>0 ");
                    }
                    else
                    {

                        DB.ExecuteSQL("delete from MFG_BOMDetails where BOMDetailsID=" + (ViewState["BOMDetailsID"].ToString() == "" ? "0" : ViewState["BOMDetailsID"].ToString()));
                    }
                    resetError("The selected node is successfully deleted", true);
                }
                else
                {
                    resetError("Cannot delete, as material is configured in routing", true);
                    return;
                }

            }
            catch (Exception ex) {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while checking BOM line items in routing", true);
                return;
            }
        }


        #endregion ---------- Structured BOM Details ----------


        #region ----- TreeView Control --------


        public void LoadTreeViewData() {

            PopulateTreeview();

            ViewState["BOMDetailsID"] = "";

            ViewState["IsNodeSelected"] = "false";
         
        }


        private void PopulateTreeview()
        {

            //lblLineItemsCount.Text = "[ " + DB.GetSqlN("select  COUNT(*) AS N from MFG_BOMDetails BD JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=BD.BOMMaterialMasterID AND MM.MTypeID=7 where BD.IsActive=1 AND BD.IsDeleted=0 AND BOMHeaderID=" + CommonLogic.QueryString("bomid")).ToString() + " ]";

            lblLineItemsCount.Text = "[ " + DB.GetSqlN("SELECT COUNT(BOMHeaderID) AS N FROM MFG_BOMDetails where IsActive=1 AND IsDeleted=0  AND MaterialMasterRevisionID IS NULL AND ParentMaterialMasterID<>0 AND BOMHeaderID=" + CommonLogic.QueryString("bomid").ToString()) + " ]";

            this.tvHierarchyView.Nodes.Clear();

            tvHierarchyView.ShowLines = true;

            

            HierarchyTrees hierarchyTrees = new HierarchyTrees();
            HierarchyTrees.HTree objHTree = null;
             //(@"data source=INVENTRAX8\MSSQL2012;initial catalog=MRLWMSC21_RT;user id=DBAdmin;password=inv123;persist security info=True;packet size=4096;"))
            using (SqlConnection connection = new SqlConnection
            (CommonLogic.Application("DBConn")))
            {
                connection.Open();
                using (SqlCommand command =
                new SqlCommand("[sp_MFG_GetBoMTreeViewList]", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlParameter sqlPrm = new SqlParameter("@BOMHeaderID", CommonLogic.QueryString("bomid"));
                    command.Parameters.Add(sqlPrm);
                    SqlDataReader reader = command.ExecuteReader
                    (System.Data.CommandBehavior.CloseConnection);
                    while (reader.Read())
                    {
                        objHTree = new HierarchyTrees.HTree();

                        objHTree.LevelDepth = int.Parse(reader["LEVEL_DEPTH"].ToString());
                        objHTree.NodeID = int.Parse(reader["NODE_ID"].ToString());
                        objHTree.UnderParent = int.Parse(reader["UNDER_PARENT"].ToString());
                        objHTree.NodeDescription = reader["NODE_DESCRIPTION"].ToString();
                        objHTree.MDescription = reader["MDescription"].ToString();
                        hierarchyTrees.Add(objHTree);

                    }
                }
            }
            //Iterate through Collections.
            foreach (HierarchyTrees.HTree hTree in hierarchyTrees)
            {
                //Filter the collection HierarchyTrees based on 
                //Iteration as per object Htree Parent ID 
                HierarchyTrees.HTree parentNode = hierarchyTrees.Find
                (delegate(HierarchyTrees.HTree emp)
                { return emp.NodeID == hTree.UnderParent; });
                //If parent node has child then populate the leaf node.
                if (parentNode != null)
                {
                    foreach (TreeNode tn in tvHierarchyView.Nodes)
                    {
                        //If single child then match Node ID with Parent ID
                        if (tn.Value == parentNode.NodeID.ToString())
                        {
                            TreeNode tn2 = new TreeNode(hTree.NodeDescription.ToString(), hTree.NodeID.ToString());
                            tn2.ToolTip = hTree.MDescription.ToString();
                            tn.ChildNodes.Add(tn2);
                           
                        }

                        //If Node has multiple child ,
                        //recursively traverse through end child or leaf node.
                        if (tn.ChildNodes.Count > 0)
                        {
                            foreach (TreeNode ctn in tn.ChildNodes)
                            {
                                RecursiveChild(ctn, parentNode.NodeID.ToString(), hTree);

                              //  ctn.ToolTip = hTree.NodeDescription;
                            }
                        }
                    }
                }
                //Else add all Node at first level 
                else
                {
                    TreeNode tn = new TreeNode(hTree.NodeDescription, hTree.NodeID.ToString());
                    tn.ToolTip = hTree.MDescription;
                    tvHierarchyView.Nodes.Add(tn);
                  // tvHierarchyView.ToolTip = hTree.NodeDescription.ToString();
                }
            }
            tvHierarchyView.ExpandAll();
        }


        public void RecursiveChild(TreeNode tn, string searchValue, HierarchyTrees.HTree hTree)
        {
            if (tn.Value == searchValue)
            {
                TreeNode tn1 = new TreeNode(hTree.NodeDescription.ToString(), hTree.NodeID.ToString());
                tn1.ToolTip = hTree.MDescription.ToString();
                tn.ChildNodes.Add(tn1);
                //tn.ToolTip=hTree.NodeDescription.ToString();
            }
            if (tn.ChildNodes.Count > 0)
            {
                foreach (TreeNode ctn in tn.ChildNodes)
                {
                    RecursiveChild(ctn, searchValue, hTree);
                    //ctn.ToolTip = hTree.NodeDescription;
                }
            }
        }


        protected void tvHierarchyView_SelectedNodeChanged(object sender, System.EventArgs e)
        {
           // string str = tvHierarchyView.SelectedNode.Value + "/" + ( tvHierarchyView.SelectedNode.Parent == null ? "0" : tvHierarchyView.SelectedNode.Parent.Value );

          // resetError(str,true);

           String MMID = tvHierarchyView.SelectedNode.Value;

           ViewState["BOMMMID"] = MMID;

           String PrMMID = (tvHierarchyView.SelectedNode.Parent == null ? "0" : tvHierarchyView.SelectedNode.Parent.Value);

           String SQLStr = "EXEC [dbo].[sp_MFG_GetBoMTreeViewDetails]  @BOMHeaderID="+CommonLogic.QueryString("bomid")+",@MaterialMasterID=" + MMID + ",@ParentMaterialMasterID=" + PrMMID;


           LoadSelectedNodeDetails(SQLStr,MMID);

           ViewState["IsNodeSelected"] = "true";

            

           tvHierarchyView.SelectedNode.Selected = false;

            
        }


        private void LoadSelectedNodeDetails(String SQLStr,String vMMID) {

            try
            {
                chkBDDelete.Checked = false;

                IDataReader drSelectedNoteDet = DB.GetRS(SQLStr);
                drSelectedNoteDet.Read();
                hifMTypeID.Value = DB.RSFieldInt(drSelectedNoteDet, "MTypeID").ToString();
                txtUoM.Text = DB.RSField(drSelectedNoteDet,"UoM");
                
                txtRTUoM.Text = DB.RSField(drSelectedNoteDet, "UoM");
              
                txtQuantity.Text = DB.RSFieldDecimal(drSelectedNoteDet,"BOMQuantity").ToString();
                txtMType.Text = DB.RSField(drSelectedNoteDet, "MType");

                IDataReader drParent = DB.GetRS("select  PrMM.MCode + ISNULL(' - ' + PrMMRev.Revision,'') AS ParentPartNo , BD.MaterialMasterRevisionID from MFG_BOMDetails BD LEFT JOIN MMT_MaterialMaster PrMM on PrMM.MaterialMasterID=BD.BOMMaterialMasterID LEFT JOIN MMT_MaterialMaster_Revision PrMMRev ON PrMMRev.MaterialMasterID=PrMM.MaterialMasterID AND PrMMRev.MaterialMasterRevisionID=BD.MaterialMasterRevisionID where BD.BOMHeaderID=" + CommonLogic.QueryString("bomid") + " AND BD.BOMMaterialMasterID=" + DB.RSFieldInt(drSelectedNoteDet, "ParentMaterialMasterID"));

                drParent.Read();

                if (DB.RSField(drParent, "ParentPartNo") == "")
                {
                    txtParentMMCode.Text = DB.RSField(drSelectedNoteDet, "PartNo");
                    hifTvBOMParentPartNo.Value = DB.RSFieldInt(drSelectedNoteDet, "MaterialMasterRevisionID").ToString();

                    ViewState["hifTvBOMParentPartNo"] = DB.RSFieldInt(drSelectedNoteDet, "MaterialMasterRevisionID").ToString();
                }
                else
                {

                    txtParentMMCode.Text = DB.RSField(drParent, "ParentPartNo");
                    hifTvBOMParentPartNo.Value = DB.RSFieldInt(drParent, "MaterialMasterRevisionID").ToString();

                    ViewState["hifTvBOMParentPartNo"] = DB.RSFieldInt(drParent, "MaterialMasterRevisionID").ToString();
                }

                if (DB.RSFieldInt(drSelectedNoteDet, "MTypeID") != 7)
                {
                    txtChildMMCode.Text=DB.RSField(drSelectedNoteDet,"PartNo");
                   // hifChildRevMMID.Value = DB.RSFieldInt(drSelectedNoteDet, "MaterialMasterRevisionID").ToString();
                    hifRevUoMID.Value = DB.RSFieldInt(drSelectedNoteDet, "MaterialMaster_BOMUoMID").ToString();

                    //txtParentMMCode.Text= DB.RSField(drSelectedNoteDet,"PartNo");

                    hifRTPartNoID.Value = DB.RSFieldInt(drParent, "MaterialMasterRevisionID").ToString();
                    hifRTUoMID.Value = DB.RSFieldInt(drSelectedNoteDet, "MaterialMaster_BOMUoMID").ToString();

                    txtRTPartNumber.Text = DB.RSField(drSelectedNoteDet, "PartNo");
                   
                }
                else {
                    txtChildMMCode.Text = DB.RSField(drSelectedNoteDet, "PartNo");
                   // hifTvBOMParentPartNo.Value = DB.RSFieldInt(drSelectedNoteDet, "BOMMaterialMasterID").ToString();
                    hifTvBOMUoM.Value = DB.RSFieldInt(drSelectedNoteDet, "MaterialMaster_BOMUoMID").ToString();
                    
                    hifRTUoMID.Value = DB.RSFieldInt(drSelectedNoteDet, "MaterialMaster_BOMUoMID").ToString();
                    txtRTPartNumber.Text = DB.RSField(drSelectedNoteDet, "PartNo");
                    hifRTPartNoID.Value = vMMID;
                }


                ViewState["BOMDetailsID"] = DB.RSFieldInt(drSelectedNoteDet, "BOMDetailsID").ToString();


                hifBOMDetailsID.Value = DB.RSFieldInt(drSelectedNoteDet, "BOMDetailsID").ToString();

                drSelectedNoteDet.Close();
                drParent.Close();
            }
            catch (Exception ex) {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);

                resetError("Error while loading selected node details",true);
            }


        }


        public void ExportToExcel(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                string filename = txtbomrefno.Text+".xls";
                System.IO.StringWriter tw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                DataGrid dgGrid = new DataGrid();
                dgGrid.DataSource = dt;
                dgGrid.DataBind();
                //Get the HTML for the control.
                dgGrid.RenderControl(hw);
                //Write the HTML back to the browser.
                //Response.ContentType = application/vnd.ms-excel;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
                this.EnableViewState = false;
                Response.Write(tw.ToString());
                Response.End();
            }
        }


        #endregion --- Treview Control --------


        protected void imgbtngvBOMList_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (CommonLogic.QueryString("bomid") != "")
                {

                    DataSet dsBOMList = DB.GetDS("EXEC [dbo].[sp_MFG_GetBoMTreeViewList_ExportToExcel]  @BOMHeaderID=" + (CommonLogic.QueryString("bomid") == "" ? "0" : CommonLogic.QueryString("bomid")), false);

                    if(dsBOMList.Tables[0].Rows.Count!=0)
                        ExportToExcel(dsBOMList.Tables[0]);
                }
            }
            catch (Exception ex) {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while exporting data", true);
                return;
            }
        }

        


    }
}