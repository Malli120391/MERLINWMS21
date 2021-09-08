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
using System.Web.UI.HtmlControls;


// Module Name : Manufacturing
// Usecase Ref.: Routing Details_UC_008
// DevelopedBy : Naresh P
// CreatedOn   : 05/05/2014
// Modified On : 24/03/2015


namespace MRLWMSC21.mManufacturingProcess
{
    public partial class Routing : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        public static string ParentRoutingDetailsID = "";

        string gvUniqueID = String.Empty;
        int gvNewPageIndex = 0;
        int gvEditIndex = -1;
        string gvSortExpr = String.Empty;
        



        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Inbound";

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Validate();

            if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("Routing"))))
            {
                Response.Redirect("../Login.aspx?eid=6");
            }

            if (CommonLogic.QueryString("status") == "success")
            {
                lblStatus.Text = "New revision successfully created";
            }

            if (CommonLogic.QueryString("upstatus") == "success")
            {
                lblStatus.Text = "Successfully Updated";
            }


            if (!IsPostBack)
            {


                LoadoprCheckBoxList();

                 DesignLogic.SetInnerPageSubHeading(this.Page, "Routing");

                ViewState["TenantID"] = cp.TenantID.ToString();

                ViewState["isEditMode"] = false;
                lnkUpdate.Text = "Create Routing" + CommonLogic.btnfaSave;
                ViewState["HeaderID"] = 0;
                if (CommonLogic.QueryString("routid") != "")
                {
                    hifRoutingHeaderID.Value = CommonLogic.QueryString("routid");
                    build_FormData();

                    ViewState["RotDocRefSqlList"] = "EXEC [dbo].[sp_MFG_GetRoutingDocRefDetails] @RoutingHeaderID=" + CommonLogic.QueryString("routid");
                    gvRoutDocRef_buildGridData(gvRoutDocRef_buildGridData());

                    txtRoutingRefNo.Enabled = false;
                    txtRevNo.Enabled = false;

                    txtBOMrefNo.Enabled = false;
                    txtFinishedMaterial.Enabled = false;
                    txtRoutingDocumentType.Enabled = false;
                    lnkCopyRotDetails.Visible = true;
                    lnkClear.Visible = false;

                    LoadRoutDefciencyList();

                    if (DB.GetSqlN("select top 1 MFG_RC.OldRoutingDetailsActivityID AS N from MFG_RoutingClone MFG_RC JOIN MFG_RoutingDetailsActivity RDA ON RDA.RoutingDetailsActivityID=MFG_RC.OldRoutingDetailsActivityID AND RDA.IsActive=1 AND RDA.IsDeleted=0 JOIN MFG_RoutingDetails RD ON RD.RoutingDetailsID=RDA.RoutingDetailsID AND  RD.IsActive=1 AND RD.IsDeleted=0 where  MFG_RC.IsActive=1 AND MFG_RC.IsDeleted=0 AND RD.RoutingHeaderID=" + CommonLogic.QueryString("routid")) != 0)
                    {
                        lnkCopyRotDetails.Visible = false;
                    }

                    lnkAddTERCheckPoints.OnClientClick = "openTERDialog1();";
                    lnkAddTERCheckPoints.Visible = true;

                    ViewState["TERCheckPointsSQL"] = "EXEC [dbo].[sp_MFG_GetRoutingDetailsActivity_TERCheckPoints]  @RoutingHeaderID=" + CommonLogic.QueryString("routid");

                    gvTERCheckPoints_buildGridData(gvTERCheckPoints_buildGridData());
                }
                else
                {
                    tdDocRefDt.Visible = false;
                    lnkAddDocRef.Visible = false;
                    tdlineitems.Visible = false;
                    chkIsActive.Checked = true;
                    lnkAddNewLineItem.Visible = false;
                    lnkCopyRotDetails.Visible = false;
                    tdRotBOMDefc.Visible = false;
                }


            }
        }

        #region -------- Basic Data  --------------------

        private void build_FormData()
        {
            try
            {
                IDataReader rsRoutingHeaderData = DB.GetRS("EXEC [dbo].[sp_MFG_GetRoutingHeader] @RoutingHeaderID=" + CommonLogic.QueryString("routid"));
                if (rsRoutingHeaderData.Read() == true)
                {
                    // txtRoutingRefNo.Text = DB.RSField(rsRoutingHeaderData, "MCode");
                    atcSupplier.Text = DB.RSField(rsRoutingHeaderData, "SupplierName");
                    hifSupplier.Value = DB.RSFieldInt(rsRoutingHeaderData, "SupplierID").ToString();
                    txtRoutingName.Text = DB.RSField(rsRoutingHeaderData, "RoutingName");
                    chkIsActive.Checked = Convert.ToBoolean(DB.RSFieldTinyInt(rsRoutingHeaderData, "IsActive"));
                    chkIsDeleted.Checked = Convert.ToBoolean(DB.RSFieldTinyInt(rsRoutingHeaderData, "IsDeleted"));
                    txtInspectionCheckListVer.Text = DB.RSField(rsRoutingHeaderData, "InspectionCheckListVersion");
                    txtRoutingDocumentType.Text = DB.RSField(rsRoutingHeaderData, "RoutingDocumentType");
                    txtRoutRemarks.Text = DB.RSField(rsRoutingHeaderData,"Remarks");
                    lblRoutType.Text = "<nobr>" + DB.RSField(rsRoutingHeaderData, "RoutingDocumentType") + "<nobr/>";

                    lnkAddTERCheckPoints.Text = "TER Check Points [ " + DB.RSFieldInt(rsRoutingHeaderData, "TERCheckPointsCount") +" ]";

                    txtJAPReferenceNo.Text = DB.RSField(rsRoutingHeaderData, "JAPReferenceNo");
                    ViewState["HeaderID"] = CommonLogic.QueryString("routid");
                    lnkUpdate.Text = "Update"+CommonLogic.btnfaUpdate;
                    ViewState["RoutingDetailsList"] = "EXEC [dbo].[sp_MFG_GetRoutingDetails] @RoutingHeaderID=" + ViewState["HeaderID"];
                    txtRoutingRefNo.Enabled = false;

                    lblEffectiveDate.Text = DB.RSFieldDateTime(rsRoutingHeaderData, "EffectiveDate").ToString("dd/MM/yyyy");

                    txtRevNo.Text = DB.GetSqlS("select Revision AS S from MFG_RoutingHeader_Revision where IsActive=1 AND IsDeleted=0 AND RoutingHeaderID=" + CommonLogic.QueryString("routid"));

                    int MMRvID = DB.GetSqlN("select MaterialMasterRevisionID AS N from MFG_RoutingHeader_Revision where IsActive=1 AND IsDeleted=0 AND RoutingHeaderID=" + CommonLogic.QueryString("routid"));

                    hifMMRvID.Value = MMRvID.ToString();

                    txtRoutingRefNo.Text = DB.GetSqlS("select MM.MCode+'-'+MMR.Revision AS S  from MMT_MaterialMaster_Revision MMR JOIN MMT_MaterialMaster MM ON MMR.MaterialMasterID=MM.MaterialMasterID  AND MM.IsActive=1 AND MM.IsDeleted=0 where  MMR.MaterialMasterRevisionID=" + MMRvID);

                    hifRoutingDocTypeID.Value = DB.RSFieldInt(rsRoutingHeaderData, "RoutingDocumentTypeID").ToString();

                    if (DB.RSFieldInt(rsRoutingHeaderData, "RoutingDocumentTypeID") == 1 || DB.RSFieldInt(rsRoutingHeaderData, "RoutingDocumentTypeID") == 3)
                        tdCheckPointFormat.Visible=true;
                    else
                        tdCheckPointFormat.Visible=false;

                    hifBoMHeaderRevID.Value = DB.RSFieldInt(rsRoutingHeaderData, "BOMHeaderRevisionID").ToString();

                    txtBOMrefNo.Text = DB.RSField(rsRoutingHeaderData, "BOMRevision");

                    hifFinishedMRvID.Value = DB.RSFieldInt(rsRoutingHeaderData, "FinishedMaterialMasterID").ToString();

                    txtFinishedMaterial.Text = DB.RSField(rsRoutingHeaderData, "FinishedMaterial");

                    hifBoMHeaderID.Value = DB.RSFieldInt(rsRoutingHeaderData, "BOMHeaderID").ToString();



                    txtApprovedBy.Text = DB.RSField(rsRoutingHeaderData, "ApprovedBy");
                    txtJRPReleseDate.Text = DB.RSFieldDateTime(rsRoutingHeaderData, "ApprovedByDate").ToString("dd/MM/yyyy") == DateTime.MinValue.ToString("dd/MM/yyyy") ? "" : DB.RSFieldDateTime(rsRoutingHeaderData, "ApprovedByDate").ToString("dd/MM/yyyy");
                    txtJAPLink.Text = DB.RSField(rsRoutingHeaderData, "Change");
                    //txtCheckedByDate.Text = DB.RSFieldDateTime(rsRoutingHeaderData, "CheckedByDate").ToString("dd/MM/yyyy") == "01/01/0001" ? DateTime.Now.ToString("dd/MM/yyyy") : DB.RSFieldDateTime(rsRoutingHeaderData, "CheckedByDate").ToString("dd/MM/yyyy");
                    
                    txtJAPRev.Text = DB.RSField(rsRoutingHeaderData, "CusdesignDocRefNo");
                    txtBTPRev.Text = DB.RSField(rsRoutingHeaderData, "CustomerPartListRefNo");
                    txtCAGRev.Text = DB.RSField(rsRoutingHeaderData, "DesignDocumentRefNo");

                    txtECNDate.Text = (DB.RSFieldDateTime(rsRoutingHeaderData, "ECNDate").ToString("dd/MM/yyyy") == DateTime.MinValue.ToString("dd/MM/yyyy") ? "" : DB.RSFieldDateTime(rsRoutingHeaderData, "ECNDate").ToString("dd/MM/yyyy"));
                    txtECNNo.Text = DB.RSField(rsRoutingHeaderData, "ECNNumber");
                  //  txtDocName.Text = DB.RSField(rsRoutingHeaderData, "DocumentName");
                    txtPreparedBy.Text = DB.RSField(rsRoutingHeaderData, "PreparedBy");
                   // txtSpecificationDocumentRefNo.Text = DB.RSField(rsRoutingHeaderData, "SpecificationDocumentRefNo");
                   // txtStandardRef.Text = DB.RSField(rsRoutingHeaderData, "StandardRef");
                    txtPreviousVer.Text = DB.RSField(rsRoutingHeaderData, "PreviousVersion");
                    txtICReleaseDate.Text = DB.RSFieldDateTime(rsRoutingHeaderData, "PreparedByDate").ToString("dd/MM/yyyy") == DateTime.MinValue.ToString("dd/MM/yyyy") ? "" : DB.RSFieldDateTime(rsRoutingHeaderData, "PreparedByDate").ToString("dd/MM/yyyy");
                    txtCheckedBy.Text = DB.RSField(rsRoutingHeaderData, "CheckedBy");



                    txtCGMDrawingRefNo.Text = DB.RSField(rsRoutingHeaderData, "CGMDrawingRefNo");
                    txtBTPDocumentRefNo.Text = DB.RSField(rsRoutingHeaderData, "BTPDocumentRefNo");
                   
                    txtOtherReferences.Text = DB.RSField(rsRoutingHeaderData, "OtherReferences");

                    txtJRPApprovedBy.Text = DB.RSField(rsRoutingHeaderData, "JRPApprovedBy");
                    txtJRPReviewedBy.Text = DB.RSField(rsRoutingHeaderData, "JRPReviewedBy");
                    txtJRPPreparedBy.Text = DB.RSField(rsRoutingHeaderData, "JRPPreparedBy");
                    //txtJRPQCApprovedBy.Text = DB.RSField(rsRoutingHeaderData, "JRPQCApprovedBy");


                    hifJRPApprovedByID.Value = DB.RSFieldInt(rsRoutingHeaderData, "JRPApprovedByID").ToString();
                    hifJRPReviewedByID.Value = DB.RSFieldInt(rsRoutingHeaderData, "JRPReviewedByID").ToString();
                    hifJRPPreparedByID.Value = DB.RSFieldInt(rsRoutingHeaderData, "JRPPreparedByID").ToString();
                    hifJRPQCApprovedByID.Value = DB.RSFieldInt(rsRoutingHeaderData, "JRPQCApprovedByID").ToString();





                    hifApprovedBy.Value = DB.RSFieldInt(rsRoutingHeaderData, "ApprovedByID").ToString();
                    hifCheckedBy.Value = DB.RSFieldInt(rsRoutingHeaderData, "CheckedByID").ToString();
                    hifPreparedBy.Value = DB.RSFieldInt(rsRoutingHeaderData, "PreparedByID").ToString();








                    Build_RoutingLineItems(Build_RoutingLineItems());


                }
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while loading routing details", true);
            }
        }

        protected void lnkClear_Click(object sender, EventArgs e)
        {
            //if (lnkUpdate.Text == "Update")
            //{
            //    Response.Redirect("Routing.aspx?bomid=" + ViewState["HeaderID"].ToString());

            //}
            //else
            //{
                Response.Redirect("RoutingList.aspx");
            //}
        }

        protected void lnkUpdate_Click(object sender, EventArgs e)
        {

            
            //Page.Validate("UpdateRouting");

            //if (!Page.IsValid)
            //{
            //    resetError("Please check mandatory fields", true);
            //    Page.Validate();
            //    return;
            //}


            //if (DB.GetSqlN("select top 1 MFG_RDAC.RoutingDetailsActivityCaptureID AS N from MFG_ProductionOrderHeader MFG_POH JOIN MFG_RoutingHeader_Revision MFG_RHRv ON MFG_RHRv.RoutingHeaderRevisionID=MFG_POH.RoutingHeaderRevisionID AND  MFG_RHRv.IsActive=1 AND MFG_RHRv.IsDeleted=0  JOIN MFG_BOMHeader_Revision MFG_BHRv ON MFG_BHRv.BOMHeaderRevisionID=MFG_RHRv.BOMHeaderRevisionID AND MFG_BHRv.IsActive=1 AND MFG_BHRv.IsDeleted=0 JOIN MFG_RoutingDetailsActivityCapture MFG_RDAC ON MFG_RDAC.ProductionOrderHeaderID=MFG_POH.ProductionOrderHeaderID AND MFG_RDAC.isDeleted=0   where  MFG_POH.ProductionOrderStatusID IN (2,3,4,5)   AND   MFG_POH.IsActive=1 AND MFG_POH.IsDeleted=0  AND  MFG_RHRv.RoutingHeaderID=" + (CommonLogic.QueryString("routid") == "" ? "0" : CommonLogic.QueryString("routid"))) != 0)
            //{
            //    resetError("Production process is started, cannot modify routing details", true);
            //    return;
            //}

            if ((CommonLogic.QueryString("routid") == "" ? "0" : CommonLogic.QueryString("routid"))!="0")
            {
            if (DB.GetSqlN("EXEC [dbo].[sp_MFG_CheckForBOMRoutingEdit]  @RoutingHeaderID=" + (CommonLogic.QueryString("routid") == "" ? "0" : CommonLogic.QueryString("routid"))) != 0)
            {
                //resetError("Production process is started, cannot modify routing details", true);
                resetError("Cannot modify routing details, as routing is mapped to job order", true);
                return;
            }
            }


            int RoutingDocumentTypeID = DB.GetSqlN("select RoutingDocumentTypeID AS N from MFG_RoutingDocumentType where IsActive=1 AND IsDeleted=0 AND RoutingDocumentType=" + DB.SQuote(txtRoutingDocumentType.Text));

            if (RoutingDocumentTypeID == 0)
            {
                resetError("Routing document type does not exist", true);
                return;
            }

            String vMMRvID = Request.Form[hifMMRvID.UniqueID].ToString(); //DB.GetSqlN("select MaterialMasterID AS N from MMT_MaterialMaster where IsActive=1 AND IsDeleted=0 AND MCode="+DB.SQuote(txtRoutingRefNo.Text));

            if (vMMRvID == "")
            {
                resetError("Invalid Part #", true);
                return;
            }



            if (DB.GetSqlN("select MM.MTypeID AS N from MMT_MaterialMaster_Revision MMR JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=MMR.MaterialMasterID where MMR.MaterialMasterRevisionID=" + vMMRvID) == 8)
            {
                if (RoutingDocumentTypeID != 1006) {
                    resetError("Routing document type for sub-assembly is sets/phantoms", true);
                    return;
                }
            }
            else
            {
                if (RoutingDocumentTypeID == 1006)
                {
                    resetError("Invalid routing document type", true);
                    return;
                }
            }

            String vBOMMMrevID = DB.GetSqlN("select MaterialMasterRevisionID AS N from MFG_BOMHeader_Revision where IsActive=1 AND IsDeleted=0 AND BOMHeaderRevisionID =" + CommonLogic.IIF(hifBoMHeaderRevID.Value != "", hifBoMHeaderRevID.Value, "0")).ToString();

            if (vBOMMMrevID == "" || vBOMMMrevID == "0")
            {
                resetError("Invalid BOM Ref.#", true);
                return;
            }


            if (vMMRvID != vBOMMMrevID)
            {
                resetError("Routing Ref.# and  BOM Ref.# should be same", true);
                return;
            }


            if (DB.GetSqlN("select top 1 BD.BOMHeaderID AS N from MFG_BOMDetails BD JOIN MFG_BOMHeader_Revision BHR ON BHR.BOMHeaderID=BD.BOMHeaderID AND BHR.IsActive=1 AND BHR.IsDeleted=0 where BD.IsActive=1 AND BD.IsDeleted=0 AND BHR.BOMHeaderRevisionID=" + CommonLogic.IIF(hifBoMHeaderRevID.Value != "", hifBoMHeaderRevID.Value, "0")) == 0) {

                resetError("Add atleast one BOM line item", true);
                return;
            }


            if (txtRevNo.Text.Trim() == "")
            {
                resetError("Revision cannot be empty", true);
                return;
            }

            if ( txtFinishedMaterial.Text == "" ||  hifFinishedMRvID.Value == "")
            {
                resetError("Finished material cannot be empty", true);
                return;
            }

            int FinishedMaterialTypeID = DB.GetSqlN("select MTypeID AS N from MMT_MaterialMaster where MaterialMasterID IN ( select MaterialMasterID from MMT_MaterialMaster_Revision where materialmasterrevisionid=" + CommonLogic.IIF(hifFinishedMRvID.Value != "", hifFinishedMRvID.Value, "0") + ")" );

            if (FinishedMaterialTypeID == 0 || (RoutingDocumentTypeID != 2 && FinishedMaterialTypeID == 9))
            {
                resetError("Invalid routing document type for the selected finished material" , true);
                return;
            }
            


            StringBuilder cmdUpdateRoutingHeaderDetails = new StringBuilder();
            cmdUpdateRoutingHeaderDetails.Append("DECLARE @NewHeaderID int ");
            cmdUpdateRoutingHeaderDetails.Append("Exec [dbo].[sp_MFG_UpsertRoutingHeader] ");
            cmdUpdateRoutingHeaderDetails.Append("@RoutingHeaderID=" + ViewState["HeaderID"]);
            cmdUpdateRoutingHeaderDetails.Append(",@MaterialMasterRevisionID=" + vMMRvID);
            cmdUpdateRoutingHeaderDetails.Append(",@RoutingName=" + DB.SQuote(txtRoutingName.Text.Trim()));
            cmdUpdateRoutingHeaderDetails.Append(",@Revision=" + DB.SQuote(txtRevNo.Text.Trim()));

            cmdUpdateRoutingHeaderDetails.Append(",@ProjectCode=NULL");
            cmdUpdateRoutingHeaderDetails.Append(",@RoutingDocumentTypeID=" + RoutingDocumentTypeID);
            cmdUpdateRoutingHeaderDetails.Append(",@JAPReferenceNo=" + DB.SQuote(txtJAPReferenceNo.Text));
            cmdUpdateRoutingHeaderDetails.Append(",@InspectionCheckListVersion=" + DB.SQuote(txtInspectionCheckListVer.Text));

            cmdUpdateRoutingHeaderDetails.Append(",@SupplierID=" + CommonLogic.IIF(hifSupplier.Value != "", hifSupplier.Value, "NULL"));

            cmdUpdateRoutingHeaderDetails.Append(",@BOMHeaderRevisionID=" + CommonLogic.IIF(hifBoMHeaderRevID.Value != "", hifBoMHeaderRevID.Value, "NULL"));
            cmdUpdateRoutingHeaderDetails.Append(",@FinishedMaterialMasterID=" + CommonLogic.IIF(hifFinishedMRvID.Value != "", hifFinishedMRvID.Value, "NULL"));

            cmdUpdateRoutingHeaderDetails.Append(",@Remarks=" + DB.SQuote(txtRoutRemarks.Text));
            cmdUpdateRoutingHeaderDetails.Append(",@CreatedBy=" + cp.UserID);
            cmdUpdateRoutingHeaderDetails.Append(",@LastModifiedBy=" + cp.UserID);
            cmdUpdateRoutingHeaderDetails.Append(",@IsActive=" + Convert.ToInt16(chkIsActive.Checked));
            cmdUpdateRoutingHeaderDetails.Append(",@IsDeleted=" + Convert.ToInt16(chkIsDeleted.Checked));

            
            String ApprovedByDate = "";
                if (txtJRPReleseDate.Text.Trim() != "")
                    ApprovedByDate = DB.SQuote(CommonLogic.GetConfigValue(cp.TenantID, "DateFormat") == "dd/MM/yyyy" ? DateTime.ParseExact(txtJRPReleseDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : txtJRPReleseDate.Text.Trim());
                else
                    ApprovedByDate = "NULL";

            String PreparedByDate = "";
                if (txtICReleaseDate.Text.Trim() != "")
                    PreparedByDate = DB.SQuote(CommonLogic.GetConfigValue(cp.TenantID, "DateFormat") == "dd/MM/yyyy" ? DateTime.ParseExact(txtICReleaseDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : txtICReleaseDate.Text.Trim());
                else
                    PreparedByDate = "NULL";
            /*
            String CheckedByDate = "";
                if (txtCheckedByDate.Text.Trim() != "")
                    CheckedByDate = DB.SQuote(CommonLogic.GetConfigValue(cp.TenantID, "DateFormat") == "dd/MM/yyyy" ? DateTime.ParseExact(txtCheckedByDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : txtCheckedByDate.Text.Trim());
                else
                    CheckedByDate = "NULL";*/

            String ECNDate = "";
                if (txtECNDate.Text.Trim() != "")
                    ECNDate = DB.SQuote(CommonLogic.GetConfigValue(cp.TenantID, "DateFormat") == "dd/MM/yyyy" ? DateTime.ParseExact(txtECNDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : txtECNDate.Text.Trim());
                else
                    ECNDate = "NULL";



            cmdUpdateRoutingHeaderDetails.Append(",@DocumentName=null");
            cmdUpdateRoutingHeaderDetails.Append(",@ApprovedBy=" + ( (hifApprovedBy.Value != "0" && hifApprovedBy.Value != "")? hifApprovedBy.Value: "NULL"));
            cmdUpdateRoutingHeaderDetails.Append(",@ApprovedByDate="+ApprovedByDate);
            cmdUpdateRoutingHeaderDetails.Append(",@PreparedBy=" + ((hifPreparedBy.Value != "0" && hifPreparedBy.Value != "") ? hifPreparedBy.Value: "NULL"));
            cmdUpdateRoutingHeaderDetails.Append(",@PreparedByDate="+PreparedByDate);
            cmdUpdateRoutingHeaderDetails.Append(",@CheckedBy=" + (( hifCheckedBy.Value != "0" && hifCheckedBy.Value != "" )?hifCheckedBy.Value: "NULL"));
            cmdUpdateRoutingHeaderDetails.Append(",@CheckedByDate=null");
            cmdUpdateRoutingHeaderDetails.Append(",@PreviousVersion=" +DB.SQuote(txtPreviousVer.Text));
            cmdUpdateRoutingHeaderDetails.Append(",@ECNNumber=" +DB.SQuote(txtECNNo.Text));
            cmdUpdateRoutingHeaderDetails.Append(",@ECNDate=" + ECNDate);
            cmdUpdateRoutingHeaderDetails.Append(",@Change="+DB.SQuote(txtJAPLink.Text));

            cmdUpdateRoutingHeaderDetails.Append(",@CusdesignDocRefNo="+DB.SQuote(txtJAPRev.Text));

            cmdUpdateRoutingHeaderDetails.Append(",@CustomerPartListRefNo="+DB.SQuote(txtBTPRev.Text));
            cmdUpdateRoutingHeaderDetails.Append(",@SpecificationDocumentRefNo=null");
            cmdUpdateRoutingHeaderDetails.Append(",@DesignDocumentRefNo="+DB.SQuote(txtCAGRev.Text));
            cmdUpdateRoutingHeaderDetails.Append(",@StandardRef=null");


            cmdUpdateRoutingHeaderDetails.Append(",@CGMDrawingRefNo=" + DB.SQuote(txtCGMDrawingRefNo.Text));
            cmdUpdateRoutingHeaderDetails.Append(",@BTPDocumentRefNo=" + DB.SQuote(txtBTPDocumentRefNo.Text));
            cmdUpdateRoutingHeaderDetails.Append(",@OtherReferences=" + DB.SQuote(txtOtherReferences.Text));
            cmdUpdateRoutingHeaderDetails.Append(",@JRPApprovedBy=" + ((hifJRPApprovedByID.Value != "0" && hifJRPApprovedByID.Value != "") ? hifJRPApprovedByID.Value : "NULL"));
            cmdUpdateRoutingHeaderDetails.Append(",@JRPReviewedBy=" + ((hifJRPReviewedByID.Value != "0" && hifJRPReviewedByID.Value != "") ? hifJRPReviewedByID.Value : "NULL"));
            cmdUpdateRoutingHeaderDetails.Append(",@JRPPreparedBy=" + ((hifJRPPreparedByID.Value != "0" && hifJRPPreparedByID.Value != "") ? hifJRPPreparedByID.Value : "NULL"));
            cmdUpdateRoutingHeaderDetails.Append(",@JRPQCApprovedBy=" + ((hifJRPQCApprovedByID.Value != "0" && hifJRPQCApprovedByID.Value != "") ? hifJRPQCApprovedByID.Value : "NULL"));

            



            cmdUpdateRoutingHeaderDetails.Append(",@NewRoutingHeaderID=@NewHeaderID OUTPUT select @NewHeaderID as N");
            Boolean IsDeleted = chkIsDeleted.Checked;
            try
            {
                ViewState["HeaderID"] = DB.GetSqlN(cmdUpdateRoutingHeaderDetails.ToString());

                if (IsDeleted == true)
                {
                    Response.Redirect("RoutingList.aspx");
                }
                lnkUpdate.Text = "Update"+CommonLogic.btnfaUpdate;
                tdlineitems.Visible = true;
                lnkAddNewLineItem.Visible = true;
                resetError("Successfully Updated", false);

                Response.Redirect("Routing.aspx?routid=" + ViewState["HeaderID"].ToString() + "&upstatus=success");

            }
            catch (SqlException sqlex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), sqlex.Source, sqlex.Message, sqlex.StackTrace);
                if (sqlex.ErrorCode == -2146232060)
                {
                    resetError("Routing Ref.#  is already updated", true);
                    return;
                }
                resetError("Error while updating routing details", true);
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while updating routing details", true);
            }
        }

        #endregion -------- Basic Data  --------------------


        #region ------- Routing Details  -----------------------

        public String Getimage(String IsChecked)
        {


            if (IsChecked != "")
            {
                if (Convert.ToBoolean(Convert.ToInt32(IsChecked)))
                    return "<img src=\"../Images/blue_menu_icons/check_mark.png\" />";
                else
                    return "";
            }
            else
            {
                return "";
            }


        }

        protected void lnkAddNewLineItem_Click(object sender, EventArgs e)
        {
            try
            {

                if (DB.GetSqlN("EXEC [dbo].[sp_MFG_CheckForBOMRoutingEdit]  @RoutingHeaderID=" + (CommonLogic.QueryString("routid") == "" ? "0" : CommonLogic.QueryString("routid"))) != 0)
                {
                    //resetError("Production process is started, cannot modify routing details", true);
                    resetError("Cannot modify routing details, as routing is mapped to job order", true);
                    return;
                }

                ViewState["RoutingDetailsList"] = "EXEC [dbo].[sp_MFG_GetRoutingDetails] @RoutingHeaderID=" + ViewState["HeaderID"];
                DataSet dsRoutingDetailsList = Build_RoutingLineItems();
                DataRow newRow = dsRoutingDetailsList.Tables[0].NewRow();

                newRow["RoutingDetailsID"] = 0;
                newRow["Name"] = "";

                dsRoutingDetailsList.Tables[0].Rows.InsertAt(newRow, 0);
                gvRoutingDetailsList.EditIndex = 0;
                gvRoutingDetailsList.PageIndex = 0;
                Build_RoutingLineItems(dsRoutingDetailsList);

            //    resetError("New Operation Sequence added",false);

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while inserting new record", true);
            }
        }

        protected void lnkGet_Click(object sender, EventArgs e)
        {

        }

        private DataSet Build_RoutingLineItems()
        {
            String cmdRoutingDetailsList = ViewState["RoutingDetailsList"].ToString();
            DataSet dsRoutingDetailsList = DB.GetDS(cmdRoutingDetailsList, false);
            return dsRoutingDetailsList;
        }

        private void Build_RoutingLineItems(DataSet dsRoutingDetailsList)
        {
            gvRoutingDetailsList.DataSource = dsRoutingDetailsList;
            gvRoutingDetailsList.DataBind();
            dsRoutingDetailsList.Dispose();



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

        protected void gvRoutingDetailsList_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.DataItem == null)
                return;

            
                for (int i = 0; i < gvRoutingDetailsList.Columns.Count; i++)
                {
                    if(i==3)
                        e.Row.Cells[3].ToolTip = "Display Order";
                    else
                        e.Row.Cells[i].ToolTip = gvRoutingDetailsList.Columns[i].HeaderText;
                }

                

            LinkButton lnkChildItems = (LinkButton)e.Row.FindControl("lnkAddActivityDetails");
            lnkChildItems.OnClientClick = "openActivityDialogAndBlock('Add Activity Details', '" + lnkChildItems.ClientID + "');";
            

            /*
            LinkButton lnkQCItems = (LinkButton)e.Row.FindControl("lnkAddQCParameters");
            lnkQCItems.OnClientClick = "openDialogAndBlock1('Add QC Parametrs', '" + lnkQCItems.ClientID + "');";
            */


            GridViewRow row = e.Row;

            //Find Child GridView control
            GridView gv = new GridView();
            gv = (GridView)row.FindControl("gvRoutingChildActivity");


           


            if (gv.UniqueID == gvUniqueID)
            {
                gv.PageIndex = gvNewPageIndex;
                gv.EditIndex = gvEditIndex;
                //Check if Sorting used

                //Expand the Child grid
                ClientScript.RegisterStartupScript(GetType(), "Expand", "<SCRIPT LANGUAGE='javascript'>expandcollapse('div" + ((DataRowView)e.Row.DataItem)["RoutingDetailsID"].ToString() + "','one');</script>");
            }


            Literal ltRoutingDetailsID = (Literal)row.FindControl("ltRoutingDetailsID");


            ViewState["RoutChildActivityListSQL"] = "EXEC [dbo].[sp_MFG_GetRoutingDetailsActivity]  @RoutingDetailsID=" + ltRoutingDetailsID.Text;

            //Prepare the query for Child GridView by passing the Customer ID of the parent row
            gv.DataSource = this.gvRoutingChildActivity_buildGridData();
            gv.DataBind();



        }

        protected void gvRoutingDetailsList_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvRoutingDetailsList.EditIndex = e.NewEditIndex;
                Build_RoutingLineItems(Build_RoutingLineItems());
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while editing", true);
            }
        }

        protected void gvRoutingDetailsList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            /*
            Page.Validate("UpdateGridItems");
            if (!IsValid)
            {
                return;
            }*/

            //if (DB.GetSqlN("select top 1 MFG_RDAC.RoutingDetailsActivityCaptureID AS N from MFG_ProductionOrderHeader MFG_POH JOIN MFG_RoutingHeader_Revision MFG_RHRv ON MFG_RHRv.RoutingHeaderRevisionID=MFG_POH.RoutingHeaderRevisionID AND  MFG_RHRv.IsActive=1 AND MFG_RHRv.IsDeleted=0  JOIN MFG_BOMHeader_Revision MFG_BHRv ON MFG_BHRv.BOMHeaderRevisionID=MFG_RHRv.BOMHeaderRevisionID AND MFG_BHRv.IsActive=1 AND MFG_BHRv.IsDeleted=0 JOIN MFG_RoutingDetailsActivityCapture MFG_RDAC ON MFG_RDAC.ProductionOrderHeaderID=MFG_POH.ProductionOrderHeaderID AND MFG_RDAC.isDeleted=0 where  MFG_POH.ProductionOrderStatusID IN (2,3,4,5)   AND  MFG_POH.IsActive=1 AND MFG_POH.IsDeleted=0  AND   MFG_RHRv.RoutingHeaderID=" + (CommonLogic.QueryString("routid") == "" ? "0" : CommonLogic.QueryString("routid"))) != 0)
            //{
            //    resetError("Production process is started, cannot modify 'Routing' details", true);
            //    return;
            //}

            if (DB.GetSqlN("EXEC [dbo].[sp_MFG_CheckForBOMRoutingEdit]  @RoutingHeaderID=" + (CommonLogic.QueryString("routid") == "" ? "0" : CommonLogic.QueryString("routid"))) != 0)
            {
                //resetError("Production process is started, cannot modify routing details", true);
                resetError("Cannot modify routing details, as routing is mapped to job order", true);
                return;
            }



            GridViewRow gridRow = gvRoutingDetailsList.Rows[e.RowIndex];
            Literal ltRoutingDetailsID = (Literal)gridRow.FindControl("ltRoutingDetailsID");
            TextBox ltSequenceNumber = (TextBox)gridRow.FindControl("ltSequenceNumber");
            TextBox txtName = (TextBox)gridRow.FindControl("txtName");
            TextBox txtDisplayNumber = (TextBox)gridRow.FindControl("txtDisplayNumber");

          

            
            // TextBox atcWorkCenterGroup = (TextBox)gridRow.FindControl("atcWorkCenterGroup");
            //  HiddenField hifWorkCenterGroup = (HiddenField)gridRow.FindControl("hifWorkCenterGroup");
            //  TextBox txtNumberofCycles = (TextBox)gridRow.FindControl("txtNumberofCycles");
            //  TextBox txtNumberofHours = (TextBox)gridRow.FindControl("txtNumberofHours");
            //   TextBox txtDescription = (TextBox)gridRow.FindControl("txtDescription");
            //  CheckBox chkIsSerialNumberRequire = (CheckBox)gridRow.FindControl("chkIsSerialNumberRequire");


            if (ltSequenceNumber.Text == "")
            {
                resetError("Operation number cannot be empty", true);
                return;
            }
            if (txtName.Text == "")
            {
                resetError("Operation name cannot be empty", true);
                return;
            }

            if (txtDisplayNumber.Text == "" || txtDisplayNumber.Text == "0")
            {
                resetError("Display order cannot be empty or  0", true);
                return;
            }


            if (DB.GetSqlN("select DisplayNumber AS N from MFG_RoutingDetails where IsDeleted=0 AND IsActive=1 AND  RoutingHeaderID=" + ViewState["HeaderID"] + " AND RoutingDetailsID<>" + ltRoutingDetailsID.Text + " AND DisplayNumber=" + txtDisplayNumber.Text) != 0)
            {
                resetError("This display order is already updated", true);
                return;
            }


            StringBuilder cmdUpdateLineItems = new StringBuilder();
            cmdUpdateLineItems.Append("EXEC [dbo].[sp_MFG_UpsertRoutingDetails] ");
            cmdUpdateLineItems.Append("@RoutingDetailsID=" + ltRoutingDetailsID.Text);
            cmdUpdateLineItems.Append(",@RoutingHeaderID=" + ViewState["HeaderID"]);
            cmdUpdateLineItems.Append(",@Name=" + DB.SQuote(txtName.Text.Trim()));
            cmdUpdateLineItems.Append(",@IsSerialNoRequired=NULL");
            cmdUpdateLineItems.Append(",@OperationNumber=" + DB.SQuote(ltSequenceNumber.Text));
            cmdUpdateLineItems.Append(",@WorkCenterGroupID=NULL");
            cmdUpdateLineItems.Append(",@NumberofCycles=NULL");
            cmdUpdateLineItems.Append(",@NumberofHours=NULL");
            cmdUpdateLineItems.Append(",@DisplayNumber=" + (txtDisplayNumber.Text == "" ? "0" : txtDisplayNumber.Text));
            
            //cmdUpdateLineItems.Append(",@Description="+(txtDescription.Text.Trim()!=""?DB.SQuote( txtDescription.Text.Trim()):"NULL"));
            cmdUpdateLineItems.Append(",@Description=NULL");
            cmdUpdateLineItems.Append(",@CreatedBy=" + cp.UserID);
            try
            {
                DB.ExecuteSQL(cmdUpdateLineItems.ToString());
                gvRoutingDetailsList.EditIndex = -1;
                Build_RoutingLineItems(Build_RoutingLineItems());
                resetError("Successfully Updated", false);
            }
            catch (SqlException sqlex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), sqlex.Source, sqlex.Message, sqlex.StackTrace);

                if (sqlex.ErrorCode == -2146232060)
                {
                    this.resetError("Operation number already exists", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);

                resetError("Error while updating routing operation line item", true);
                return;
            }

        }

        protected void gvRoutingDetailsList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvRoutingDetailsList.EditIndex = -1;
                Build_RoutingLineItems(Build_RoutingLineItems());
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while canceling", true);
            }
        }

        protected void gvRoutingDetailsList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvRoutingDetailsList.PageIndex = e.NewPageIndex;
                Build_RoutingLineItems(Build_RoutingLineItems());
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while changing index", true);
            }
        }

        protected void gvRoutingDetailsList_RowCommand(object sender, GridViewCommandEventArgs e)
        {


            GridViewRow row = ((Control)e.CommandSource).NamingContainer as GridViewRow;

            if (row != null)
            {

                if (e.CommandName == "EditChildItems")
                {
                    ClearActivityDetails();

                    ViewState["isEditMode"] = false;

                    lnkRoutDelete.Visible = false;

                    int RoutDetID = Localization.ParseNativeInt(e.CommandArgument.ToString());

                    ViewState["RoutDetID"] = RoutDetID.ToString();

                    hifRoutingDetailsID.Value = RoutDetID.ToString();

                    ParentRoutingDetailsID = RoutDetID.ToString();

                    hifDisplayNumber.Value = DB.GetSqlN("select DisplayNumber AS N from MFG_RoutingDetails where RoutingDetailsID=" + RoutDetID.ToString()).ToString();

                    hifSourceActivityRoutingDetailsActivityID.Value = Convert.ToString(RoutDetID);

                    hifDisplayOrder.Value = "0";

                    hifSActivityID.Value = "0";
                }
            }


        }

        protected void lnkIsDeleted_Click(object sender, EventArgs e)
        {

            if (DB.GetSqlN("select top 1 MFG_RDAC.RoutingDetailsActivityCaptureID AS N from MFG_ProductionOrderHeader MFG_POH JOIN MFG_RoutingHeader_Revision MFG_RHRv ON MFG_RHRv.RoutingHeaderRevisionID=MFG_POH.RoutingHeaderRevisionID AND  MFG_RHRv.IsActive=1 AND MFG_RHRv.IsDeleted=0  JOIN MFG_BOMHeader_Revision MFG_BHRv ON MFG_BHRv.BOMHeaderRevisionID=MFG_RHRv.BOMHeaderRevisionID AND MFG_BHRv.IsActive=1 AND MFG_BHRv.IsDeleted=0 JOIN MFG_RoutingDetailsActivityCapture MFG_RDAC ON MFG_RDAC.ProductionOrderHeaderID=MFG_POH.ProductionOrderHeaderID AND MFG_RDAC.isDeleted=0  where MFG_POH.ProductionOrderStatusID=2  AND  MFG_POH.IsActive=1 AND MFG_POH.IsDeleted=0  AND   MFG_RHRv.RoutingHeaderID=" + (CommonLogic.QueryString("routid") == "" ? "0" : CommonLogic.QueryString("routid"))) != 0)
            {
                resetError("Production process is started, cannot modify 'Routing' details", true);
                return;
            }


            int rowCount = gvRoutingDetailsList.Rows.Count;
            GridViewRow row;
            String RoutingDetailsIDs = "";
            for (int rowNo = 0; rowNo < rowCount; rowNo++)
            {
                row = gvRoutingDetailsList.Rows[rowNo];
                if (gvRoutingDetailsList.EditIndex != rowNo)
                {
                    if (((CheckBox)row.FindControl("chkisdeleted")).Checked)
                    {
                        RoutingDetailsIDs += ((Literal)row.FindControl("ltRoutingDetailsID")).Text + ",";

                    }
                }
            }

            try
            {
                if (RoutingDetailsIDs != "")
                {
                    DB.ExecuteSQL("Exec [dbo].[sp_MFG_DelteRoutingDetails] @RoutingDetailsIDs=" + DB.SQuote(RoutingDetailsIDs)+ ",@UpdatedBy="+cp.UserID.ToString());
                    gvRoutingDetailsList.EditIndex = -1;
                    Build_RoutingLineItems(Build_RoutingLineItems());
                    if (gvRoutingDetailsList.Rows.Count == 0)
                        atcSupplier.Enabled = true;
                    resetError("Successfully deleted the selected line items", false);
                }
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while deleting selected line items", true);
            }

        }

        #endregion ------- Routing Details  -----------------------


        #region ---------  Routing Details Activity  ----------------------



        protected void lnkRoutActivityUpdate_Click(object sender, EventArgs e)
        {
            try
            {


                if (DB.GetSqlN("select top 1 MFG_RDAC.RoutingDetailsActivityCaptureID AS N from MFG_ProductionOrderHeader MFG_POH JOIN MFG_RoutingHeader_Revision MFG_RHRv ON MFG_RHRv.RoutingHeaderRevisionID=MFG_POH.RoutingHeaderRevisionID AND  MFG_RHRv.IsActive=1 AND MFG_RHRv.IsDeleted=0  JOIN MFG_BOMHeader_Revision MFG_BHRv ON MFG_BHRv.BOMHeaderRevisionID=MFG_RHRv.BOMHeaderRevisionID AND MFG_BHRv.IsActive=1 AND MFG_BHRv.IsDeleted=0 JOIN MFG_RoutingDetailsActivityCapture MFG_RDAC ON MFG_RDAC.ProductionOrderHeaderID=MFG_POH.ProductionOrderHeaderID AND MFG_RDAC.isDeleted=0  where MFG_POH.ProductionOrderStatusID IN (2,3,4,5)   AND  MFG_POH.IsActive=1 AND MFG_POH.IsDeleted=0 AND MFG_RDAC.RoutingDetailsActivityID=" + (ViewState["mRoutingDetailsActivityID"].ToString() == "" ? "0" : ViewState["mRoutingDetailsActivityID"].ToString()) + "  AND   MFG_RHRv.RoutingHeaderID=" + (CommonLogic.QueryString("routid") == "" ? "0" : CommonLogic.QueryString("routid"))) != 0)
                {
                    resetError("Production process is started, cannot modify 'Routing' details", true);
                    return;
                }
                

               // string vtxtActivityName = txtActivityName.Text;

                string vtxtActivityCode = txtActivityCode.Text;



                // string vtxtActivityType = txtActivityType.Text;

                string vtxtWorkCenterGroup = txtWorkCenterGroup.Text;

                string vDescription = txtDescription.Text;

                string vCheckListType = txtCheckListType.Text;

                string vTERType = txtTERCaptureType.Text;

                StringBuilder sql = new StringBuilder(2500);

                if (vtxtActivityCode == "" || txtDisplayOrder.Text=="" || txtDisplayOrder.Text=="0" )
                {
                    resetError("'Activity Number' or 'Display Order' cannot be empty or  0", true);
                    return;

                }


                if (DB.GetSqlN("select DisplayOrder AS N from MFG_RoutingDetailsActivity where IsDeleted=0 AND IsActive=1 AND  RoutingDetailsID=" + ViewState["RoutDetID"].ToString() + " AND RoutingDetailsActivityID<>" + ViewState["mRoutingDetailsActivityID"].ToString() + " AND DisplayOrder=" + txtDisplayOrder.Text) != 0)
                {
                    resetError("This display order is already updated", true);
                    return;
                }


                if (txtSourceActivity.Text == "")
                    hifSourceActivityID.Value = "";

                /*
                    int ActivityTypeID = DB.GetSqlN("select ActivityTypeID AS N  from  MFG_ActivityType where IsActive=1 AND IsDeleted=0 AND ActivityType=" + DB.SQuote( vtxtActivityType));

                    if (ActivityTypeID == 0)
                    {
                        this.resetError("Invalid Activity Type", true);
                        return;

                    }
                   
                int ParentActivityID=0;
                    if (vtxtParentActivity != "")
                    {
                        ParentActivityID = DB.GetSqlN("select RoutingDetailsActivityID AS N from MFG_RoutingDetailsActivity where ActivityTypeID=1 AND IsActive=1 AND IsDeleted=0 AND RoutingDetailsID=" + ViewState["RoutDetID"].ToString() + " AND ActivityCode=" + DB.SQuote(vtxtParentActivity));

                        if (ParentActivityID == 0)
                        {
                            this.resetError("Invalid Parent Activity", true);
                            return;
                        }
                    }
                */
                int WorkCenterGroupID = DB.GetSqlN("select WorkCenterGroupID AS N  from  MFG_WorkCenterGroup where IsActive=1 AND IsDeleted=0 AND WorkCenterGroup=" + DB.SQuote(vtxtWorkCenterGroup));

                if (WorkCenterGroupID == 0)
                {
                    this.resetError("Invalid workstation type", true);
                    return;

                }

                int CheckListTypeID = DB.GetSqlN("select QCCheckListConfigurationID AS N from MFG_QCCheckListConfiguration where IsActive=1 AND IsDeleted=0 AND CheckListName="+DB.SQuote(vCheckListType));

                /*
                if (CheckListTypeID == 0)
                {
                    this.resetError("CheckList Type does not exist", true);
                    return;

                }*/
                int TERTypeID = 0;

                if (vTERType != "")
                {

                    TERTypeID = DB.GetSqlN("select TERCaptureTypeID AS N from MFG_TERCaptureType where IsActive=1 AND IsDeleted=0 AND TERCaptureType='" + vTERType + "'");

                    if (TERTypeID == 0) {

                        resetError("TER capture type does not exist",true);
                        return;
                    }


                    if (DB.GetSqlN("select top 1 RD.RoutingDetailsID AS N from MFG_RoutingDetails RD JOIN MFG_RoutingDetailsActivity RDA ON RDA.RoutingDetailsID=RD.RoutingDetailsID AND RDA.IsActive=1 AND RDA.IsDeleted=0 where RD.IsActive=1 AND RD.IsDeleted=0  AND RD.RoutingHeaderID=" + (CommonLogic.QueryString("routid") == "" ? "0" : CommonLogic.QueryString("routid")) + " AND RDA.TERCaptureTypeID=" + TERTypeID + "  AND    RDA.RoutingDetailsActivityID<>" + (ViewState["mRoutingDetailsActivityID"].ToString() == "" ? "0" : ViewState["mRoutingDetailsActivityID"].ToString())) != 0)
                    {
                        resetError("This 'TER Capture Type' is already updated to another activity", true);
                        return;
                    }

                }
                

                //int QCParmtrCount = DB.GetSqlN("select COUNT(*) AS N from MFG_RoutingDetailsActivity_QualityParameter where RoutingDetailsActivityID=" + (ViewState["mRoutingDetailsActivityID"].ToString() == "" ? "0" : ViewState["mRoutingDetailsActivityID"].ToString()));

                
                //if (Convert.ToBoolean(ViewState["isEditMode"]) == true)
                //{
                //    if (QCParmtrCount != 0)
                //    {

                //        resetError("Cannot modify activity details, as check points are configured, ", true);
                //            return;
                       
                //    }
                //} As Ronnies requirement i removed this check 16/03/2015 11:38

                /*
                int QCTERCount = DB.GetSqlN("select COUNT(*) AS N from MFG_RoutingDetailsActivity_TERCheckPoints where RoutingDetailsActivityID=" + (ViewState["mRoutingDetailsActivityID"].ToString() == "" ? "0" : ViewState["mRoutingDetailsActivityID"].ToString()));


                if (Convert.ToBoolean(ViewState["isEditMode"]) == true)
                {
                    if (QCTERCount != 0)
                    {
                       
                            resetError("Once the check points are configured,You cannot modify activity details", true);
                            return;
                       
                    }
                }*/


                int CycleTimeInMins =( Convert.ToInt32((txtCycleTimeInHours.Text == "" ? "0" : txtCycleTimeInHours.Text)) * 60 + Convert.ToInt32((txtCycleTimeInMins.Text == "" ? "0" : txtCycleTimeInMins.Text)) );

                

                sql.Append("DECLARE @UpdatedWorkCenterID int   ");
                sql.Append("EXEC  [dbo].[sp_MFG_UpsertRoutingDetailsActivity]   ");
                sql.Append("@RoutingDetailsctivityID=" + ViewState["mRoutingDetailsActivityID"].ToString());
                sql.Append(",@ParentActivityID=NULL");
                sql.Append(",@RoutingDetailsID=" + ViewState["RoutDetID"].ToString());
                sql.Append(",@ActivityCode=" + DB.SQuote(vtxtActivityCode));
                //sql.Append(",@ActivityName=NULL" + DB.SQuote(vtxtActivityName));

                sql.Append(",@QCCheckListConfigurationID=" + (CheckListTypeID.ToString()=="0"?"NULL":CheckListTypeID.ToString()) );
                sql.Append(",@ActivityName=NULL");
                sql.Append(",@WorkCenterGroupID=" + WorkCenterGroupID);
                sql.Append(",@ActivityTypeID=NULL");
                sql.Append(",@Description=" + DB.SQuote(vDescription));
                sql.Append(",@CreatedBy=" + cp.TenantID);
                sql.Append(",@TERCaptureTypeID=" + (TERTypeID == 0 ? "NULL" : TERTypeID.ToString()));
                sql.Append(",@DisplayOrder=" + (txtDisplayOrder.Text == "" ? "0" : txtDisplayOrder.Text));

                sql.Append(",@CycleTimeInHours=" + CycleTimeInMins);

                sql.Append(",@SourceActivityID=" + ( hifSourceActivityID.Value == "" ? "NULL" : hifSourceActivityID.Value ));
                
                sql.Append(",@NewRoutingDetailsctivityID = @UpdatedWorkCenterID output select @UpdatedWorkCenterID as N");

                UpdateOprCheckList(DB.GetSqlN(sql.ToString()).ToString());

                resetError("Successfully Updated", false);

                this.Build_RoutingLineItems(Build_RoutingLineItems());



                ClearActivityDetails();

                if (Convert.ToBoolean(ViewState["isEditMode"]) == true)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "dialog", "closeActivityDialog()", true);

                    Build_RoutingLineItems(Build_RoutingLineItems());
                }

            }
            catch (SqlException sqlex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), sqlex.Source, sqlex.Message, sqlex.StackTrace);

                if (sqlex.ErrorCode == -2146232060)
                {
                    this.resetError("Activity number already exists", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while updating activity details", true);
                return;
            }
        }




        public void ClearActivityDetails()
        {

            txtActivityCode.Text = "";
          //  txtActivityName.Text = "";

            txtWorkCenterGroup.Text = "";
            txtDescription.Text = "";
            txtCheckListType.Text = "";
            txtCycleTimeInHours.Text = "";
            txtTERCaptureType.Text = "";
            txtDisplayOrder.Text = "";
            txtSourceActivity.Text = "";

            ViewState["mRoutingDetailsActivityID"] = "0";

            int count = oprChkBoxList.Items.Count;
            for (int i = 0; i < count; i++)
            {
                oprChkBoxList.Items[i].Selected = false; //Disable checked list item
                oprChkBoxList.Items[i].Enabled = true;  // Enable unchecked list item
            }

        }


        public void LoadActivityDetails(int RoutingDetailsActivityID)
        {

            try
            {
                // hifSourceActivityRoutingDetailsActivityID.Value = Convert.ToString(RoutingDetailsActivityID);

                Session["hifSourceActivityRoutingDetailsActivityID"] = Convert.ToString(RoutingDetailsActivityID);
                IDataReader drActivity = DB.GetRS("EXEC [dbo].[sp_MFG_GetRoutingDetailsActivity]  @RoutingDetailsActivityID=" + RoutingDetailsActivityID);

                if (drActivity.Read())
                {
                    txtActivityCode.Text = DB.RSField(drActivity, "ActivityCode");
                    //txtActivityName.Text = DB.RSField(drActivity, "ActivityName");

                    txtWorkCenterGroup.Text = DB.RSField(drActivity, "WorkCenterGroup");
                    txtDescription.Text = DB.RSField(drActivity, "Description");
                    txtTERCaptureType.Text = DB.RSField(drActivity, "TERCaptureType");
                    hifTERCaptureTypeID.Value = DB.RSFieldInt(drActivity, "TERCaptureTypeID").ToString();
                    txtCheckListType.Text = DB.RSField(drActivity, "CheckListName");
                    hifCheckListTypeID.Value = DB.RSFieldInt(drActivity, "QCCheckListConfigurationID").ToString();

                    txtCycleTimeInHours.Text = ( DB.RSFieldInt(drActivity, "CycleTimeInHours") / 60 ).ToString() ;

                    txtCycleTimeInMins.Text = (DB.RSFieldInt(drActivity, "CycleTimeInHours") % 60).ToString();

                    txtDisplayOrder.Text = DB.RSFieldInt(drActivity, "DisplayOrder").ToString();
                    txtSourceActivity.Text = DB.RSField(drActivity, "SourceActivity");
                    hifSourceActivityID.Value = DB.RSFieldInt(drActivity, "SourceActivityID").ToString();

                    ViewState["mRoutingDetailsActivityID"] = RoutingDetailsActivityID.ToString();


                    int QCParmtrCount = DB.GetSqlN("select COUNT(*) AS N from MFG_RoutingDetailsActivity_QualityParameter where RoutingDetailsActivityID=" + (ViewState["mRoutingDetailsActivityID"].ToString() == "" ? "0" : ViewState["mRoutingDetailsActivityID"].ToString()));

                    if (Convert.ToBoolean(ViewState["isEditMode"]) == true)
                    {
                        if (QCParmtrCount != 0)
                        {

                            txtWorkCenterGroup.Enabled = false;
                            txtCheckListType.Enabled = false;

                        }
                    }
                }

                drActivity.Close();

                LoadConfiguredOprList(RoutingDetailsActivityID.ToString());

            }
            catch (Exception ex)
            {

                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);

            }
        }


        protected void lnkRoutCancel_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "dialog", "closeActivityDialog()", true);

            Build_RoutingLineItems(Build_RoutingLineItems());
        }




        protected void LoadoprCheckBoxList()
        {


            IDataReader rsChkList = DB.GetRS("select ActivityCode,ActivityTypeID from MFG_ActivityType where IsActive=1 AND IsDeleted=0");

            while (rsChkList.Read())
            {
                oprChkBoxList.Items.Add(new ListItem(rsChkList["ActivityCode"].ToString(), rsChkList["ActivityTypeID"].ToString()));

            }
            rsChkList.Close();

        }



        
        protected void LoadConfiguredOprList(string RoutingDetailsActivityID)
        {

            IDataReader mspReader = DB.GetRS("select MFG_RO.ActivityTypeID from MFG_RoutingDetailsActivity_Operation MFG_RO  where MFG_RO.isactive=1 AND MFG_RO.isdeleted=0 AND MFG_RO.RoutingDetailsActivityID=" + RoutingDetailsActivityID);
            int count = oprChkBoxList.Items.Count;
            for (int i = 0; i < count; i++)
            {
                oprChkBoxList.Items[i].Selected = false; //Disable checked list item
                oprChkBoxList.Items[i].Enabled = true;  // Enable unchecked list item
            }
            if (mspReader.Read())
            {
                do
                {
                    oprChkBoxList.Items[DB.RSFieldInt(mspReader, "ActivityTypeID") - 1].Selected = true; // Enable unchecked list item

                } while (mspReader.Read());


            }

            mspReader.Close();

        }


       
        protected void UpdateOprCheckList(string RoutingDetailsActivityID)
        {

            string OprIDs = "";
            foreach (ListItem listItem in oprChkBoxList.Items) // Get data from CheckBox List
            {

                if (listItem.Selected)
                {
                    OprIDs += listItem.Value + ",";
                }

            }

            
            string[] OperIDs = OprIDs.Split(',');
            // When Item is Cheked
            try
            {
                StringBuilder sqlMspUpsert = new StringBuilder();

                sqlMspUpsert.Append("EXEC [dbo].[sp_MFG_UpsertRoutingDetailsActivity_Operation]     ");
                sqlMspUpsert.Append("@RoutingDetailsActivityID=" + RoutingDetailsActivityID);

                //As per the swamy reqirement i removed these conditions on 18/08/2014
                /*if (OperIDs.Contains("1") && OperIDs.Contains("2") && OperIDs.Contains("3"))
                {
                    sqlMspUpsert.Append(",@ActivityTypeIDs=" + DB.SQuote("1,2,3"));
                }
                else if (OperIDs.Contains("3"))
                {
                    sqlMspUpsert.Append(",@ActivityTypeIDs=" + DB.SQuote("1,2,3"));
                }
                else
                    if (OperIDs.Contains("1") && OperIDs.Contains("3"))
                        sqlMspUpsert.Append(",@ActivityTypeIDs=" + DB.SQuote("1,2,3"));
                else
                if (OperIDs.Contains("1") || OperIDs.Contains("2"))
                    sqlMspUpsert.Append(",@ActivityTypeIDs=" + DB.SQuote("1,2"));
                else
                {
                    sqlMspUpsert.Append(",@ActivityTypeIDs=" + DB.SQuote(""));
                }*/


                sqlMspUpsert.Append(",@ActivityTypeIDs=" + DB.SQuote(OprIDs));
                sqlMspUpsert.Append(",@CreatedBy=" + cp.UserID);

                DB.ExecuteSQL(sqlMspUpsert.ToString()); // Execute SP

                resetError("Successfully Updated", false);

                sqlMspUpsert.Clear(); // Clear Data in sqlMspUpsert

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while updating operation details", true);
            }
        }



        protected void lnkRoutDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToBoolean(ViewState["isEditMode"]) == true)
                {

                    if (DB.GetSqlN("select top 1 MFG_RDAC.RoutingDetailsActivityCaptureID AS N from MFG_ProductionOrderHeader MFG_POH JOIN MFG_RoutingHeader_Revision MFG_RHRv ON MFG_RHRv.RoutingHeaderRevisionID=MFG_POH.RoutingHeaderRevisionID AND  MFG_RHRv.IsActive=1 AND MFG_RHRv.IsDeleted=0  JOIN MFG_BOMHeader_Revision MFG_BHRv ON MFG_BHRv.BOMHeaderRevisionID=MFG_RHRv.BOMHeaderRevisionID AND MFG_BHRv.IsActive=1 AND MFG_BHRv.IsDeleted=0 JOIN MFG_RoutingDetailsActivityCapture MFG_RDAC ON MFG_RDAC.ProductionOrderHeaderID=MFG_POH.ProductionOrderHeaderID AND MFG_RDAC.isDeleted=0  where MFG_POH.ProductionOrderStatusID IN (2,3,4,5)   AND  MFG_POH.IsActive=1 AND MFG_POH.IsDeleted=0 AND MFG_RDAC.RoutingDetailsActivityID=" + (ViewState["mRoutingDetailsActivityID"].ToString() == "" ? "0" : ViewState["mRoutingDetailsActivityID"].ToString()) + "  AND   MFG_RHRv.RoutingHeaderID=" + (CommonLogic.QueryString("routid") == "" ? "0" : CommonLogic.QueryString("routid"))) != 0)
                    {
                        resetError("Production process is started, cannot modify 'Routing' details", true);
                        return;
                    }

                    if (DB.GetSqlN("select Count(*) AS N from MFG_RoutingDetailsActivity RDA JOIN MFG_RoutingDetails RD ON RD.RoutingDetailsID=RDA.RoutingDetailsID AND RD.IsActive=1 AND RD.IsDeleted=0 where RD.RoutingHeaderID=" + (CommonLogic.QueryString("routid") == "" ? "0" : CommonLogic.QueryString("routid")) + " AND RDA.IsActive=1 AND RDA.IsDeleted=0 AND RDA.SourceActivityID=" + (ViewState["mRoutingDetailsActivityID"].ToString() == "" ? "0" : ViewState["mRoutingDetailsActivityID"].ToString())) != 0)
                    {
                        resetError("Cannot delete, as this activity configured as source activity", true);
                        return;
                    }



                    DB.ExecuteSQL("EXEC [dbo].[sp_MFG_DelteRoutingDetailsActivity] @RoutingDetailsActivityID=" + (ViewState["mRoutingDetailsActivityID"].ToString() == "" ? "0" : ViewState["mRoutingDetailsActivityID"].ToString())+ ",@UpdatedBy="+cp.UserID.ToString());



                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "dialog", "closeActivityDialog()", true);

                    Build_RoutingLineItems(Build_RoutingLineItems());

                    resetError("Successfully Deleted", false);

                }
            }
            catch (Exception ex) {

                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while deleting activity details", false);
                return;
            }

        }



        /*
        protected void lnkAddNewActivityDetails_Click(object sender, EventArgs e)
        {


            ViewState["RoutActivityIsInsert"] = false;
            StringBuilder sql = new StringBuilder(2500);

            try
            {
                gvRoutingActivity.EditIndex = 0;
                DataSet dsParent = this.gvRoutingActivity_buildGridData();
                DataRow row = dsParent.Tables[0].NewRow();
                row["RoutingDetailsActivityID"] = 0;
                dsParent.Tables[0].Rows.InsertAt(row, 0);
                this.gvRoutingActivity_buildGridData(dsParent);
                this.resetError("Add New Line Item.", false);
                ViewState["RoutActivityIsInsert"] = true;

            }
            catch (Exception ex)
            {
                this.resetError("Error updating data, check for mandatory fields <br/>" + ex.ToString(), true);
            }


        }

        protected DataSet gvRoutingActivity_buildGridData()
        {
            string sql = ViewState["RoutActivityListSQL"].ToString();
            DataSet ds = DB.GetDS(sql, false);
            
            return ds;

        }

        protected void gvRoutingActivity_buildGridData(DataSet ds)
        {
            gvRoutingActivity.DataSource = ds;
            gvRoutingActivity.DataBind();
            ds.Dispose();
        }

        protected void gvRoutingActivity_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["RoutActivityIsInsert"] = false;
            this.resetError("", false);
            gvRoutingActivity.PageIndex = e.NewPageIndex;
            gvRoutingActivity.EditIndex = -1;
            this.gvRoutingActivity_buildGridData(this.gvRoutingActivity_buildGridData());


        }

        protected void gvRoutingActivity_RowEditing(object sender, GridViewEditEventArgs e)
        {
            ViewState["RoutActivityIsInsert"] = false;
            gvRoutingActivity .EditIndex = e.NewEditIndex;
            this.gvRoutingActivity_buildGridData(this.gvRoutingActivity_buildGridData());
        }

        protected void gvRoutingActivity_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            ViewState["RoutActivityIsInsert"] = false;

            GridViewRow row = gvRoutingActivity.Rows[e.RowIndex];
            
            if (row != null)
            {

                    string txtActivityName = ((TextBox)row.FindControl("txtActivityName")).Text;

                    string txtActivityCode = ((TextBox)row.FindControl("txtActivityCode")).Text;

                    string txtActivityType = ((TextBox)row.FindControl("txtActivityType")).Text;

                    string txtWorkCenterGroup = ((TextBox)row.FindControl("txtWorkCenterGroup")).Text;

                    string ltHidRoutingDetailsActivityID = ((Literal)row.FindControl("ltHidRoutingDetailsActivityID")).Text;
                    
                    string vDescription = ((TextBox)row.FindControl("txtDescription")).Text;

                    StringBuilder sql = new StringBuilder(2500);

                    int ActivityTypeID = DB.GetSqlN("select ActivityTypeID AS N  from  MFG_ActivityType where IsActive=1 AND IsDeleted=0 AND ActivityType=" + DB.SQuote( txtActivityType));

                    if (ActivityTypeID == 0)
                    {
                        this.resetError("Invalid Activity Type", true);
                        return;

                    }

                    int WorkCenterGroupID = DB.GetSqlN("select WorkCenterGroupID AS N  from  MFG_WorkCenterGroup where IsActive=1 AND IsDeleted=0 AND WorkCenterGroup=" + DB.SQuote( txtWorkCenterGroup));

                    if (WorkCenterGroupID == 0)
                    {
                        this.resetError("Invalid Work Station Type", true);
                        return;

                    }




                    sql.Append("EXEC  [dbo].[sp_MFG_UpsertRoutingDetailsActivity]   ");
                    sql.Append("@RoutingDetailsctivityID=" + ltHidRoutingDetailsActivityID);
                    sql.Append(",@RoutingDetailsID=" + ViewState["RoutDetID"].ToString());
                    sql.Append(",@ActivityCode=" + DB.SQuote(txtActivityCode));
                    sql.Append(",@ActivityName=" + DB.SQuote(txtActivityName));
                    sql.Append(",@WorkCenterGroupID=" + WorkCenterGroupID);
                    sql.Append(",@ActivityTypeID=" + ActivityTypeID);
                    sql.Append(",@Description=" + DB.SQuote(vDescription));
                    sql.Append(",@CreatedBy=" + cp.TenantID);
                    try
                    {
                        DB.ExecuteSQL(sql.ToString());

                        gvRoutingActivity.EditIndex = -1;

                        this.gvRoutingActivity_buildGridData(this.gvRoutingActivity_buildGridData());

                        this.resetError("Update success", false);


                    }
                    catch (SqlException sqlex)
                    {
                        if (sqlex.ErrorCode == -2146232060)
                        {
                            this.resetError("Duplicate entries not allowed", true);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {

                        this.resetError("Cannot update data,  check for mandatory fields", true);
                    }

                }

            
        }

        protected void gvRoutingActivity_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

            if (Localization.ParseBoolean(ViewState["RoutActivityIsInsert"].ToString()))
            {
                GridViewRow row = gvRoutingActivity.Rows[e.RowIndex];
                if (row != null)
                {
                    int iden = 0;
                    if (((Literal)row.FindControl("ltHidRoutingDetails_ScrapCodeID")).Text != "")
                    {
                        iden = Convert.ToInt32(((Literal)row.FindControl("ltHidRoutingDetailsActivityID")).Text);
                        this.deleteActivityRow(iden);
                    }

                    resetError("New Item canceled", false);

                }

            }

            gvRoutingActivity.EditIndex = -1;

            this.gvRoutingActivity_buildGridData(this.gvRoutingActivity_buildGridData());

        }

        protected void gvRoutingActivity_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gvRoutingActivity_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void resetError(string error, bool isError)
        {

            string str = "<font class=\"noticeMsg\">NOTICE:</font>&nbsp;&nbsp;&nbsp;";
            if (isError)
                str = "<font class=\"errorMsg\">ERROR:</font>&nbsp;&nbsp;&nbsp;";

            if (error.Length > 0)
                str += error + "";
            else
                str = "";


            lblRoutActivityStatus.Text = str;


        }

        protected void lnkRoutActivityCancel_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "dialog", "closeActivityDialog()", true);

            this.gvRoutingActivity_buildGridData(gvRoutingActivity_buildGridData());

            Build_RoutingLineItems(Build_RoutingLineItems());

        }

        protected void lnkRoutActDelete_Click(object sender, EventArgs e)
        {

            string gvIDs = "";

            bool chkBox = false;
            //'Navigate through each row in the GridView for checkbox items
            foreach (GridViewRow gv in gvRoutingActivity.Rows)
            {
                CheckBox isDelete = (CheckBox)gv.FindControl("chkChildIsDelete");

                if (isDelete == null)
                    return;

                if (isDelete.Checked)
                {
                    chkBox = true;
                    // Concatenate GridView items with comma for SQL Delete
                    gvIDs += ((Literal)gv.FindControl("ltHidRoutingDetailsActivityID")).Text.ToString() + ",";



                }
            }

            // Execute SQL Query only if checkboxes are checked to avoid any error with initial null string
            try
            {
                string DeleteSQL = "";
                if (chkBox)
                {
                    DeleteSQL = "DELETE MFG_RoutingDetailsActivity WHERE RoutingDetailsActivityID IN (" + gvIDs.Substring(0, gvIDs.LastIndexOf(",")) + ")";  // //
                    DB.ExecuteSQL(DeleteSQL);

                }

                resetError("Successfully deleted the selected items", false);

            }
            catch (Exception ex)
            {
                resetError("Error updating data" + ex.ToString(), true);
            }

            this.gvRoutingActivity_buildGridData(this.gvRoutingActivity_buildGridData());
        }

        protected void deleteActivityRow(int iden)
        {

            try
            {
                DB.ExecuteSQL("DELETE from MFG_RoutingDetailsActivity WHERE RoutingDetailsActivityID =" + iden);
            }
            catch
            {
                resetError("Error cancelling delete row", true);
            }

        }*/

        #endregion ---------  Routing Details Activity  ----------------------


        #region     ---------------   Routing Scrap Code Details ---------------------


        protected DataSet gvRoutScrapCodes_buildGridData()
        {
            string sql = ViewState["RoutScrapCodesListSQL"].ToString();
            DataSet ds = DB.GetDS(sql, false);
            //   ltSITRecordCount.Text = "[" + ds.Tables[0].Rows.Count.ToString() + "]";
            /*
            if (ltSITRecordCount.Text == "[0]")
            {
                resetParentListError("No kit item is available for the given material code", true);
            }*/

            return ds;

        }

        protected void gvRoutScrapCodes_buildGridData(DataSet ds)
        {
            gvRoutScrapCodes.DataSource = ds;
            gvRoutScrapCodes.DataBind();
            ds.Dispose();
        }

        protected void lnkAddNewItem_Click(object sender, EventArgs e)
        {

            ViewState["RoutScrapCodesIsInsert"] = false;
            StringBuilder sql = new StringBuilder(2500);

            try
            {
                gvRoutScrapCodes.EditIndex = 0;
                gvRoutScrapCodes.PageIndex = 0;
                DataSet dsParent = this.gvRoutScrapCodes_buildGridData();
                DataRow row = dsParent.Tables[0].NewRow();
                row["RoutingDetailsActivity_NonConfirmityID"] = 0;
                dsParent.Tables[0].Rows.InsertAt(row, 0);
                this.gvRoutScrapCodes_buildGridData(dsParent);
             //   this.resetError("New Scrap line item added", false);
                ViewState["RoutScrapCodesIsInsert"] = true;

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                this.resetError("Error while inserting new scrap line item", true);
            }


        }

        protected void gvRoutScrapCodes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["RoutScrapCodesIsInsert"] = false;

            gvRoutScrapCodes.PageIndex = e.NewPageIndex;
            gvRoutScrapCodes.EditIndex = -1;
            this.gvRoutScrapCodes_buildGridData(this.gvRoutScrapCodes_buildGridData());
        }

        protected void gvRoutScrapCodes_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gvRoutScrapCodes_RowEditing(object sender, GridViewEditEventArgs e)
        {
            ViewState["RoutScrapCodesIsInsert"] = false;
            gvRoutScrapCodes.EditIndex = e.NewEditIndex;
            this.gvRoutScrapCodes_buildGridData(this.gvRoutScrapCodes_buildGridData());
        }

        protected void gvRoutScrapCodes_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {


            ViewState["RoutScrapCodesIsInsert"] = false;
            GridViewRow row = gvRoutScrapCodes.Rows[e.RowIndex];

            if (row != null)
            {
                string vScrapCode = ((TextBox)row.FindControl("txtScrapCode")).Text;
                string vRoutingDetails_ScrapCodeID = ((Literal)row.FindControl("ltHidRoutingDetails_ScrapCodeID")).Text;
                string vDescription = ((TextBox)row.FindControl("txtDescription")).Text;

                StringBuilder sql = new StringBuilder(2500);

                int vScrapCodeID = DB.GetSqlN("select NonConfirmityCodeID AS N  from  MFG_NonConfirmityCode where IsActive=1 AND IsDeleted=0 AND NonConfirmityCode=" + DB.SQuote(vScrapCode.Split('-')[0]));

                if (vScrapCodeID == 0)
                {
                    this.resetError("Invalid Non Confirmity Code", true);
                    return;

                }

                sql.Append("EXEC  [dbo].[sp_MFG_UpsertRoutingDetailsActivity_NonConfirmityCode]   ");
                sql.Append("@RoutingDetails_NonConfirmityID=" + vRoutingDetails_ScrapCodeID);
                sql.Append(",@RoutingDetailsctivityID=" + ViewState["vRoutingDetailsActivityID"]);
                sql.Append(",@NonConfirmityCodeID=" + vScrapCodeID);
                sql.Append(",@Description=" + DB.SQuote(vDescription));
                sql.Append(",@CreatedBy=" + cp.TenantID);
                try
                {
                    DB.ExecuteSQL(sql.ToString());

                    gvRoutScrapCodes.EditIndex = -1;

                    this.gvRoutScrapCodes_buildGridData(this.gvRoutScrapCodes_buildGridData());

                    this.resetError("Successfully Updated", false);


                }
                catch (SqlException sqlex)
                {
                    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), sqlex.Source, sqlex.Message, sqlex.StackTrace);

                    if (sqlex.ErrorCode == -2146232060)
                    {
                        this.resetError("NC Code already exists", true);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                    this.resetError("Error while updating scrap line item", true);
                }

            }

        }

        protected void gvRoutScrapCodes_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            /*
            if (Localization.ParseBoolean(ViewState["RoutScrapCodesIsInsert"].ToString()))
            {
                GridViewRow row = gvRoutScrapCodes.Rows[e.RowIndex];
                if (row != null)
                {
                    int iden = 0;
                    if (((Literal)row.FindControl("ltHidRoutingDetails_ScrapCodeID")).Text != "")
                    {
                        iden = Convert.ToInt32(((Literal)row.FindControl("ltHidRoutingDetails_ScrapCodeID")).Text);
                        this.deleteRowPerm(iden);
                    }

                    resetError("New Item canceled", false);
                    
                }

            }*/

            gvRoutScrapCodes.EditIndex = -1;
            this.gvRoutScrapCodes_buildGridData(this.gvRoutScrapCodes_buildGridData());
        }

        protected void gvRoutScrapCodes_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "dialog", "closeDialog()", true);


            this.gvRoutScrapCodes_buildGridData(gvRoutScrapCodes_buildGridData());
            Build_RoutingLineItems(Build_RoutingLineItems());
        }

        protected void deleteRowPerm(int iden)
        {

            try
            {
                DB.ExecuteSQL("DELETE from MFG_RoutingDetailsActivity_NonConfirmityCode WHERE RoutingDetailsActivity_NonConfirmityID =" + iden);
            }
            catch(Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error cancelling delete row", true);
            }

        }

        protected void lnkRoutSCRDelete_Click(object sender, EventArgs e)
        {


            string gvIDs = "";

            bool chkBox = false;
            //'Navigate through each row in the GridView for checkbox items
            foreach (GridViewRow gv in gvRoutScrapCodes.Rows)
            {
                CheckBox isDelete = (CheckBox)gv.FindControl("chkChildIsDelete");

                if (isDelete == null)
                    return;

                if (isDelete.Checked)
                {
                    chkBox = true;
                    // Concatenate GridView items with comma for SQL Delete
                    gvIDs += ((Literal)gv.FindControl("ltHidRoutingDetails_ScrapCodeID")).Text.ToString() + ",";



                }
            }

            // Execute SQL Query only if checkboxes are checked to avoid any error with initial null string
            try
            {
                string DeleteSQL = "";
                if (chkBox)
                {
                    DeleteSQL = "DELETE from MFG_RoutingDetailsActivity_NonConfirmityCode WHERE RoutingDetailsActivity_NonConfirmityID IN (" + gvIDs.Substring(0, gvIDs.LastIndexOf(",")) + ")";  // //
                    DB.ExecuteSQL(DeleteSQL);
                    resetError("Successfully deleted the selected line items", false);
                }



            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while deleting scrap line items", true);
            }

            this.gvRoutScrapCodes_buildGridData(this.gvRoutScrapCodes_buildGridData());


        }




        #endregion   ---------------   Routing Scrap Code Details ---------------------


        #region   ---------------   Routing QC Parameters Details ---------------------


        public String GetIsRequiredimage(String IsChecked)
        {


            if (IsChecked != "")
            {
                if (Convert.ToBoolean(Convert.ToInt32(IsChecked)))
                    return "<img src=\"../Images/blue_menu_icons/check_mark.png\" />";
                else
                    return "";
            }
            else
            {
                return "";
            }


        }

        protected void lnkAddQualityParameter_Click(object sender, EventArgs e)
        {

            if (!cp.IsInRole("-3"))
            {
                resetError("Only 'QC Operator' can configure checkpoints", true);
                return;
            }

            ViewState["RoutQCIsInsert"] = false;
            StringBuilder sql = new StringBuilder(2500);

            try
            {

                if (DB.GetSqlN("select top 1 MFG_RDAC.RoutingDetailsActivityCaptureID AS N from MFG_ProductionOrderHeader MFG_POH JOIN MFG_RoutingHeader_Revision MFG_RHRv ON MFG_RHRv.RoutingHeaderRevisionID=MFG_POH.RoutingHeaderRevisionID AND  MFG_RHRv.IsActive=1 AND MFG_RHRv.IsDeleted=0  JOIN MFG_BOMHeader_Revision MFG_BHRv ON MFG_BHRv.BOMHeaderRevisionID=MFG_RHRv.BOMHeaderRevisionID AND MFG_BHRv.IsActive=1 AND MFG_BHRv.IsDeleted=0 JOIN MFG_RoutingDetailsActivityCapture MFG_RDAC ON MFG_RDAC.ProductionOrderHeaderID=MFG_POH.ProductionOrderHeaderID AND MFG_RDAC.isDeleted=0  where MFG_POH.ProductionOrderStatusID IN (2,3,4,5)   AND  MFG_POH.IsActive=1 AND MFG_POH.IsDeleted=0 AND MFG_RDAC.RoutingDetailsActivityID=" + (ViewState["vRoutingDetailsActivityID1"].ToString() == "" ? "0" : ViewState["vRoutingDetailsActivityID1"].ToString()) + "  AND   MFG_RHRv.RoutingHeaderID=" + (CommonLogic.QueryString("routid") == "" ? "0" : CommonLogic.QueryString("routid"))) != 0)
                {
                    resetError("Production process is started, cannot modify 'Routing' details", true);
                    return;
                }

                int CheckListTypeID = DB.GetSqlN("select QCCheckListConfigurationID AS N from MFG_RoutingDetailsActivity where RoutingDetailsActivityID=" + ViewState["vRoutingDetailsActivityID1"]);

                if (CheckListTypeID == 0)
                {
                    resetError("Please update check list type", true);
                    return;
                }


                gvQualityParameters.EditIndex = 0;
                gvQualityParameters.PageIndex = 0;
                DataSet dsParent = this.gvQualityParameters_buildGridData();
                DataRow row = dsParent.Tables[0].NewRow();
                row["RoutingDetailsActivity_QualityParameterID"] = 0;
                row["VisualType"] = 0;
                row["MeasureType"] = 0;
                row["RecordedType"] = 0;

                dsParent.Tables[0].Rows.InsertAt(row, 0);
                this.gvQualityParameters_buildGridData(dsParent);
               // this.resetError("New QC line item added", false);
                ViewState["RoutQCIsInsert"] = true;

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                this.resetError("Error while inserting new checkpoint", true);
            }
        }

        protected DataSet gvQualityParameters_buildGridData()
        {
            string sql = ViewState["RoutQCParametersSQL"].ToString();
            DataSet ds = DB.GetDS(sql, false);

            return ds;

        }

        protected void gvQualityParameters_buildGridData(DataSet ds)
        {

            gvQualityParameters.DataSource = ds;
            gvQualityParameters.DataBind();
            ds.Dispose();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "unblockQualityDialog", "unblockQualityDialog();", true);

        }


        protected void gvQualityParameters_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ViewState["RoutQCIsInsert"] = false;

            gvQualityParameters.PageIndex = e.NewPageIndex;
            gvQualityParameters.EditIndex = -1;
            this.gvQualityParameters_buildGridData(this.gvQualityParameters_buildGridData());
        }

        protected void gvQualityParameters_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.DataItem == null)
                return;
            try
            {

                
                if ((e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
                {

                    TextBox txtCheckPointDescription = (TextBox)e.Row.FindControl("txtCheckPointDescription");
                    txtCheckPointDescription.Focus();

                    CheckBox chkVisualType = (CheckBox)e.Row.FindControl("chkVisualType");

                    chkVisualType.Checked = true;

                    String vID = ((Literal)e.Row.FindControl("lthidRoutingDetails_QualityParameterID")).Text;

                    if (vID != "0")
                    {

                        /*
                        CheckBox chk = (CheckBox)e.Row.FindControl("chkRecordedType");
                        TextBox txtMinValue = (TextBox)e.Row.FindControl("txtMinValue");
                        TextBox txtMaxValue = (TextBox)e.Row.FindControl("txtMaxValue");
                        TextBox txtParameterDataType = (TextBox)e.Row.FindControl("txtParameterDataType");

                        if (chk.Checked)
                        {
                            txtMinValue.Visible = true;
                            txtMaxValue.Visible = true;
                            txtParameterDataType.Visible = true;
                        }
                        else
                        {
                            txtMinValue.Visible = false;
                            txtMaxValue.Visible = false;
                            txtParameterDataType.Visible = false;
                        }

                        */

                    }
                }
            }
            catch (Exception ex) {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while loading QC Parameters ",true);
            }

        }

        protected void gvQualityParameters_RowEditing(object sender, GridViewEditEventArgs e)
        {
            ViewState["RoutQCIsInsert"] = false;
            gvQualityParameters.EditIndex = e.NewEditIndex;
            this.gvQualityParameters_buildGridData(this.gvQualityParameters_buildGridData());
        }

        protected void gvQualityParameters_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            if (!cp.IsInRole("-3"))
            {
                resetError("Only 'QC Operator' can update checkpoints", true);
                return;
            }

            if (DB.GetSqlN("select top 1 MFG_RDAC.RoutingDetailsActivityCaptureID AS N from MFG_ProductionOrderHeader MFG_POH JOIN MFG_RoutingHeader_Revision MFG_RHRv ON MFG_RHRv.RoutingHeaderRevisionID=MFG_POH.RoutingHeaderRevisionID AND  MFG_RHRv.IsActive=1 AND MFG_RHRv.IsDeleted=0  JOIN MFG_BOMHeader_Revision MFG_BHRv ON MFG_BHRv.BOMHeaderRevisionID=MFG_RHRv.BOMHeaderRevisionID AND MFG_BHRv.IsActive=1 AND MFG_BHRv.IsDeleted=0 JOIN MFG_RoutingDetailsActivityCapture MFG_RDAC ON MFG_RDAC.ProductionOrderHeaderID=MFG_POH.ProductionOrderHeaderID AND MFG_RDAC.isDeleted=0  where MFG_POH.ProductionOrderStatusID IN (2,3,4,5)   AND  MFG_POH.IsActive=1 AND MFG_POH.IsDeleted=0 AND MFG_RDAC.RoutingDetailsActivityID=" + (ViewState["vRoutingDetailsActivityID1"].ToString() == "" ? "0" : ViewState["vRoutingDetailsActivityID1"].ToString()) + "  AND   MFG_RHRv.RoutingHeaderID=" + (CommonLogic.QueryString("routid") == "" ? "0" : CommonLogic.QueryString("routid"))) != 0)
            {
                resetError("Production process is started, cannot modify 'Routing' details", true);
                return;
            }
                


            ViewState["RoutQCIsInsert"] = false;
            GridViewRow row = gvQualityParameters.Rows[e.RowIndex];

            if (row != null)
            {
                
                string vRoutingDetails_QualityParameterID = ((Literal)row.FindControl("lthidRoutingDetails_QualityParameterID")).Text;
                string vtxtDisplayOrder = ((TextBox)row.FindControl("txtDisplayOrder")).Text;
                string vchkVisualType = ((CheckBox)row.FindControl("chkVisualType")).Checked == true ? "1" : "0";
                string vchkMeasureType = ((CheckBox)row.FindControl("chkMeasureType")).Checked == true ? "1" : "0";
                string vchkRecordedType = ((CheckBox)row.FindControl("chkRecordedType")).Checked == true ? "1" : "0";
                string vtxtParameterDataType = ((TextBox)row.FindControl("txtParameterDataType")).Text;

                string vtxtMinValue = ((TextBox)row.FindControl("txtMinValue")).Text == "" ? "0" : ((TextBox)row.FindControl("txtMinValue")).Text;
                string vtxtMaxValue = ((TextBox)row.FindControl("txtMaxValue")).Text == "" ? "0" : ((TextBox)row.FindControl("txtMaxValue")).Text;


                string vtxtCheckPointDescription = ((TextBox)row.FindControl("txtCheckPointDescription")).Text;
                string vtxtJAPReferenceText = ((TextBox)row.FindControl("txtJAPReferenceText")).Text;



                StringBuilder sql = new StringBuilder(2500);

                if (vtxtCheckPointDescription == "")
                {
                    this.resetError("Check point cannot be empty", true);
                    return;
                }


                
                int CheckListTypeID = DB.GetSqlN("select QCCheckListConfigurationID AS N from MFG_RoutingDetailsActivity where RoutingDetailsActivityID=" + ViewState["vRoutingDetailsActivityID1"]);

                /*
                if (CheckListTypeID != 0 && CheckListTypeID != 3)
                {
                    int SeperatorCount = DB.GetSqlN("select SeparatorCount AS N from MFG_QCCheckListConfiguration where QCCheckListConfigurationID=" + CheckListTypeID);

                    string[] CheckPointArray = vtxtCheckPointDescription.Split('|');

                    if (CheckPointArray.Length != SeperatorCount) {

                        this.resetError("Invalid check point", true);
                        return;
                    }

                }*/

                if (CheckListTypeID == 0)
                {
                    this.resetError("Please update check list type", true);
                    return;
                }
                


                String vtxtParameterDataTypeID = "0";
                if (vchkRecordedType != "0")
                {

                    vtxtParameterDataTypeID = DB.GetSqlN("select top 1 ParameterDataTypeID AS N from  GEN_ParameterDataType where IsActive=1 AND IsDeleted=0 AND ParameterDataType=" + DB.SQuote(vtxtParameterDataType)).ToString();

                    if (vtxtParameterDataTypeID == "0")
                    {
                        this.resetError("Invalid parameter type", true);
                        return;

                    }

                }

                sql.Append("EXEC  [dbo].[sp_MFG_UpsertRoutingDetailsActivity_QualityParameter]   ");
                sql.Append("@RoutingDetails_QualityParameterID=" + vRoutingDetails_QualityParameterID);
                sql.Append(",@RoutingDetailsActivityID=" + ViewState["vRoutingDetailsActivityID1"]);
                sql.Append(",@DisplayOrder=" + vtxtDisplayOrder);
                sql.Append(",@VisualType=1");
                sql.Append(",@MeasureType=" + vchkMeasureType);
                sql.Append(",@RecordedType=" + vchkRecordedType);
                if (vchkRecordedType == "1")
                {
                    if (vtxtParameterDataTypeID == "2")
                    {

                        sql.Append(",@ParameterDataTypeID=" + (vtxtParameterDataTypeID == "0" ? "NULL" : vtxtParameterDataTypeID));

                        if (vtxtParameterDataTypeID == "2" || vtxtParameterDataTypeID == "1")
                        {
                            sql.Append(",@MinValue=" + (vtxtMinValue != "0" ? DB.SQuote(vtxtMinValue) : "0"));
                        }
                        else
                        {
                            sql.Append(",@MinValue=" + (vtxtMinValue != "0" ? DB.SQuote(vtxtMinValue) : "NULL"));
                        }

                        sql.Append(",@MaxValue=" + (vtxtMaxValue != "0" ? DB.SQuote(vtxtMaxValue) : "0.00"));
                    }
                    else {
                        sql.Append(",@ParameterDataTypeID=" + (vtxtParameterDataTypeID == "0" ? "NULL" : vtxtParameterDataTypeID));
                        sql.Append(",@MinValue=NULL");
                        sql.Append(",@MaxValue=NULL");
                    }

                    
                }
                else {
                    sql.Append(",@ParameterDataTypeID=NULL" );
                    sql.Append(",@MinValue=NULL");
                    sql.Append(",@MaxValue=NULL" );
                }
                

                sql.Append(",@CheckPointDescription=" + DB.SQuote(vtxtCheckPointDescription));
                sql.Append(",@JAPReferenceText=" + DB.SQuote(vtxtJAPReferenceText));
                sql.Append(",@CreatedBy=" + cp.UserID);


                decimal vMinTollerance, vMaxTollerance;
                try
                {
                    vMinTollerance = Convert.ToDecimal(vtxtMinValue);
                    vMaxTollerance = Convert.ToDecimal(vtxtMaxValue);
                }
                catch (Exception ex) {
                    this.resetError("Please enter a valid entry", true);
                    return;
                }

                if (vtxtParameterDataTypeID == "2" || vtxtParameterDataTypeID == "1")
                {
                    //if (vMaxTollerance == 0 || vMaxTollerance == 0)
                    //{
                    //    this.resetError("The Tolerance value cannot be 0 or empty", true);
                    //    return;
                    //}

                    if (vMaxTollerance < vMinTollerance)
                    {
                        this.resetError(" Max. tolerance value should be greater than min. tolerance value", true);
                        return;
                    }

                }
                try
                {
                    DB.ExecuteSQL(sql.ToString());

                    gvQualityParameters.EditIndex = -1;

                    this.gvQualityParameters_buildGridData(this.gvQualityParameters_buildGridData());

                    this.resetError("Successfully Updated", false);


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

                    this.resetError("Error while updating QC parameters", true);

                    return;
                }

            }



        }

        protected void gvQualityParameters_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            /*
            if (Localization.ParseBoolean(ViewState["RoutQCIsInsert"].ToString()))
            {
                GridViewRow row = gvQualityParameters.Rows[e.RowIndex];
                if (row != null)
                {
                    int iden = 0;
                    if (((Literal)row.FindControl("lthidRoutingDetails_QualityParameterID")).Text != "")
                    {
                        iden = Convert.ToInt32(((Literal)row.FindControl("lthidRoutingDetails_QualityParameterID")).Text);
                        this.deleteRowPerm1(iden);
                    }

                    resetError("New Item canceled", false);

                }

            }*/

            gvQualityParameters.EditIndex = -1;
            this.gvQualityParameters_buildGridData(this.gvQualityParameters_buildGridData());
        }

        protected void gvQualityParameters_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void lnkQCDelete_Click(object sender, EventArgs e)
        {

            if (!cp.IsInRole("-3"))
            {
                resetError("Only 'QC Operator' can modify these details", true);
                return;
            }

            if (DB.GetSqlN("select top 1 MFG_RDAC.RoutingDetailsActivityCaptureID AS N from MFG_ProductionOrderHeader MFG_POH JOIN MFG_RoutingHeader_Revision MFG_RHRv ON MFG_RHRv.RoutingHeaderRevisionID=MFG_POH.RoutingHeaderRevisionID AND  MFG_RHRv.IsActive=1 AND MFG_RHRv.IsDeleted=0  JOIN MFG_BOMHeader_Revision MFG_BHRv ON MFG_BHRv.BOMHeaderRevisionID=MFG_RHRv.BOMHeaderRevisionID AND MFG_BHRv.IsActive=1 AND MFG_BHRv.IsDeleted=0 JOIN MFG_RoutingDetailsActivityCapture MFG_RDAC ON MFG_RDAC.ProductionOrderHeaderID=MFG_POH.ProductionOrderHeaderID AND MFG_RDAC.isDeleted=0 where MFG_POH.ProductionOrderStatusID IN (2,3,4,5)   AND  MFG_POH.IsActive=1 AND MFG_POH.IsDeleted=0 AND MFG_RDAC.RoutingDetailsActivityID=" + (ViewState["vRoutingDetailsActivityID1"].ToString() == "" ? "0" : ViewState["vRoutingDetailsActivityID1"].ToString()) + "  AND   MFG_RHRv.RoutingHeaderID=" + (CommonLogic.QueryString("routid") == "" ? "0" : CommonLogic.QueryString("routid"))) != 0)
            {
                resetError("Production process is started, cannot modify 'Routing' details", true);
                return;
            }


            string gvIDs = "";

            bool chkBox = false;
            //'Navigate through each row in the GridView for checkbox items
            foreach (GridViewRow gv in gvQualityParameters.Rows)
            {
                CheckBox isDelete = (CheckBox)gv.FindControl("chkChildIsDelete");

                if (isDelete == null)
                    return;

                if (isDelete.Checked)
                {

                    chkBox = true;
                    // Concatenate GridView items with comma for SQL Delete
                    gvIDs += ((Literal)gv.FindControl("lthidRoutingDetails_QualityParameterID")).Text.ToString() + ",";



                }
            }

            // Execute SQL Query only if checkboxes are checked to avoid any error with initial null string
            try
            {
                string DeleteSQL = "";
                if (chkBox)
                {
                    DeleteSQL = "DELETE MFG_RoutingDetailsActivity_QualityParameter WHERE RoutingDetailsActivity_QualityParameterID IN (" + gvIDs.Substring(0, gvIDs.LastIndexOf(",")) + ")";  // //
                    DB.ExecuteSQL(DeleteSQL);
                    resetError("Successfully deleted the selected line items", false);
                }



            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while deleting line items", true);
            }

            this.gvQualityParameters_buildGridData(this.gvQualityParameters_buildGridData());
        }

        protected void deleteRowPerm1(int iden)
        {

            try
            {
                DB.ExecuteSQL("DELETE MFG_RoutingDetailsActivity_QualityParameter WHERE RoutingDetailsActivity_QualityParameterID =" + iden); // //
            }
            catch(Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while canceling", true);
            }

        }

        protected void lnkQtCancel_Click(object sender, EventArgs e)
        {

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "dialog", "closeDialog1()", true);

            this.gvQualityParameters_buildGridData(gvQualityParameters_buildGridData());
            Build_RoutingLineItems(Build_RoutingLineItems());
        }




        #endregion ---------------  Routing QC Parameters Details ---------------------


        #region ---------- Routing Child Activity Details----------------------

        protected DataSet gvRoutingChildActivity_buildGridData()
        {
            string sql = ViewState["RoutChildActivityListSQL"].ToString();
            DataSet ds = DB.GetDS(sql, false);

            return ds;

        }

        protected void gvRoutingChildActivity_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView gvTemp = (GridView)sender;
            gvUniqueID = gvTemp.UniqueID;
            gvNewPageIndex = e.NewPageIndex;
            gvRoutingDetailsList.DataBind();
        }

        protected void gvRoutingChildActivity_RowCommand(object sender, GridViewCommandEventArgs e)
        {



            if (e.CommandName == "EditScrapItems")
            {

                int RoutDetID = Localization.ParseNativeInt(e.CommandArgument.ToString());

                ViewState["vRoutingDetailsActivityID"] = RoutDetID.ToString();


                ViewState["RoutScrapCodesListSQL"] = "EXEC [sp_MFG_GetRoutingNCDetails]  @RoutingDetailsActivityID=" + RoutDetID;

                this.gvRoutScrapCodes_buildGridData(this.gvRoutScrapCodes_buildGridData());


            }

            if (e.CommandName == "EditQCItems")
            {

                int RoutDetID = Localization.ParseNativeInt(e.CommandArgument.ToString());

                ViewState["vRoutingDetailsActivityID1"] = RoutDetID.ToString();

                ViewState["RoutQCParametersSQL"] = "EXEC [sp_MFG_GetRoutingQCParametersDetails]  @RoutingDetailsActivityID=" + RoutDetID;

                gvQualityParameters_buildGridData(gvQualityParameters_buildGridData());

            }

            /*
            if (e.CommandName == "EditTERItems")
            {

                int RoutDetID = Localization.ParseNativeInt(e.CommandArgument.ToString());

                ViewState["vRoutingDetailsActivityID1"] = RoutDetID.ToString();

                ViewState["TERCheckPointsSQL"] = "EXEC [dbo].[sp_MFG_GetRoutingDetailsActivity_TERCheckPoints]  @RoutingDetailsActivityID=" + RoutDetID;

                gvTERCheckPoints_buildGridData(gvTERCheckPoints_buildGridData());

            }*/



            if (e.CommandName == "AddComponents")
            {
                int vRoutingActivityID = Localization.ParseNativeInt(e.CommandArgument.ToString());

                //hifRoutingDetailsActivityIDNew.Value = vRoutingActivityID.ToString();

                ViewState["vRoutingActivityID"] = vRoutingActivityID.ToString();

                ViewState["vRoutingDetailsActivity_Components"] = "EXEC [dbo].[sp_MFG_GetRoutingDetailsActivity_MaterilaMaster] @RoutingDetailsActivityID=" + vRoutingActivityID + ",@BOMHeaderID=" + CommonLogic.IIF(hifBoMHeaderID.Value != "", hifBoMHeaderID.Value, "0");

                gvActivityMcode_buildGridData(gvActivityMcode_buildGridData());
            }

            if (e.CommandName == "EditActivityItems")
            {

                ViewState["isEditMode"] = true;

                lnkRoutDelete.Visible = true;

                int RoutDetID = Localization.ParseNativeInt(e.CommandArgument.ToString().Split(',')[1]);

                int RoutActivityID = Localization.ParseNativeInt(e.CommandArgument.ToString().Split(',')[0]);

                hifDisplayOrder.Value = e.CommandArgument.ToString().Split(',')[2].ToString();

                ViewState["RoutDetID"] = RoutDetID.ToString();

                Session["RoutDetactivityID"] = RoutActivityID.ToString();
                // gvRoutingActivity.EditIndex = 0;
                // ViewState["RoutActivityListSQL"] = "EXEC [dbo].[sp_MFG_GetRoutingDetailsActivity]  @RoutingDetailsActivityID=" + RoutActivityID;

                LoadActivityDetails(RoutActivityID);

                //this.gvRoutingActivity_buildGridData(this.gvRoutingActivity_buildGridData());

                this.Build_RoutingLineItems(Build_RoutingLineItems());

                hifRoutingDetailsctivityID.Value = RoutActivityID.ToString();

                hifSActivityID.Value = RoutActivityID.ToString();

                hifDisplayNumber.Value = DB.GetSqlN("select DisplayNumber AS N from MFG_RoutingDetails where RoutingDetailsID=" + RoutDetID.ToString()).ToString();

                hifSourceActivityRoutingDetailsActivityID.Value = Convert.ToString(RoutDetID);

            }







        }

        protected void gvRoutingChildActivity_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.DataItem == null)
                return;


            GridView gvTemp = (GridView)sender;

            for (int i = 0; i < gvTemp.Columns.Count; i++)
            {
                e.Row.Cells[i].ToolTip = gvTemp.Columns[i].HeaderText;
            }

            /*
            LinkButton lnkAddScrapCodes = (LinkButton)e.Row.FindControl("lnkAddScrapCodes");
            lnkAddScrapCodes.OnClientClick = "openDialogAndBlock('Add Non Confirmity Details', '" + lnkAddScrapCodes.ClientID + "');";
            */

           // LinkButton lblQCLineItemCount = (LinkButton)e.Row.FindControl("lblQCLineItemCount");
            //LinkButton lblTERLineItemCount = (LinkButton)e.Row.FindControl("lblTERLineItemCount");

            Literal ltQCCheckListConfigurationID = (Literal)e.Row.FindControl("ltQCCheckListConfigurationID");


            if (ltQCCheckListConfigurationID.Text != "0" && ltQCCheckListConfigurationID.Text != "")
            {
                /*
                if (ltQCCheckListConfigurationID.Text == "5")
                {
                    LinkButton lnkTERCheckPoints = (LinkButton)e.Row.FindControl("lnkAddTERCheckPoints");

                    lnkTERCheckPoints.OnClientClick = "openTERDialog1();";
                    lnkTERCheckPoints.Visible = true;
                }
                else*/
                if (ltQCCheckListConfigurationID.Text != "5")
                {
                    LinkButton lnkAddQCParameters = (LinkButton)e.Row.FindControl("lnkAddQCParameters");
                    lnkAddQCParameters.OnClientClick = "openDialogAndBlock1('Add Check Point Details', '" + lnkAddQCParameters.ClientID + "');";

                    lnkAddQCParameters.Visible = true;
                }
            }
            
            LinkButton lnkEditActivityItems = (LinkButton)e.Row.FindControl("lnkEditActivityItems");
            lnkEditActivityItems.OnClientClick = "openActivityDialogAndBlock('Add Activity Details', '" + lnkEditActivityItems.ClientID + "');";

            LinkButton lnkAddMcodes = (LinkButton)e.Row.FindControl("lnkAddComponents");
            lnkAddMcodes.OnClientClick = "openMcodeDialog('Component Details')";


        }

        protected void gvRoutingChildActivity_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void gvRoutingChildActivity_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

        }

        protected void gvRoutingChildActivity_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

        }

        #endregion ---------- Routing Child Activity Details----------------------


        #region ..................... Routing Document Ref. Details ........................


        protected DataSet gvRoutDocRef_buildGridData()
        {
            string sql = ViewState["RotDocRefSqlList"].ToString();
            DataSet ds = DB.GetDS(sql, false);

            return ds;

        }

        protected void gvRoutDocRef_buildGridData(DataSet ds)
        {

            gvRoutDocRef.DataSource = ds;
            gvRoutDocRef.DataBind();
            ds.Dispose();

        }


        protected void lnkAddDocRef_Click(object sender, EventArgs e)
        {
            ViewState["RoutDocRefIsInsert"] = false;
            StringBuilder sql = new StringBuilder(2500);

            try
            {
                gvRoutDocRef.EditIndex = 0;
                gvRoutDocRef.PageIndex = 0;
                DataSet dsParent = this.gvRoutDocRef_buildGridData();
                DataRow row = dsParent.Tables[0].NewRow();
                row["RoutingDocumentReferenceID"] = 0;
                dsParent.Tables[0].Rows.InsertAt(row, 0);
                this.gvRoutDocRef_buildGridData(dsParent);
             //   this.resetError("New Doc. Ref. line item added", false);
                ViewState["RoutDocRefIsInsert"] = true;

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                this.resetError("Error while inserting new doc. ref. line item", true);
            }
        }

        protected void gvRoutDocRef_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            ViewState["RoutDocRefIsInsert"] = false;

            gvRoutDocRef.PageIndex = e.NewPageIndex;
            gvRoutDocRef.EditIndex = -1;
            this.gvRoutDocRef_buildGridData(this.gvRoutDocRef_buildGridData());

        }

        protected void gvRoutDocRef_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gvRoutDocRef_RowEditing(object sender, GridViewEditEventArgs e)
        {
            ViewState["RoutDocRefIsInsert"] = false;
            gvRoutDocRef.EditIndex = e.NewEditIndex;
            this.gvRoutDocRef_buildGridData(this.gvRoutDocRef_buildGridData());
        }

        protected void gvRoutDocRef_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            /*
            Page.Validate("UpdateRoutDocRef");

            if (!Page.IsValid)
                return;*/
            /*
            if (DB.GetSqlN("select top 1 MFG_RDAC.RoutingDetailsActivityCaptureID AS N from MFG_ProductionOrderHeader MFG_POH JOIN MFG_RoutingHeader_Revision MFG_RHRv ON MFG_RHRv.RoutingHeaderRevisionID=MFG_POH.RoutingHeaderRevisionID AND  MFG_RHRv.IsActive=1 AND MFG_RHRv.IsDeleted=0  JOIN MFG_BOMHeader_Revision MFG_BHRv ON MFG_BHRv.BOMHeaderRevisionID=MFG_RHRv.BOMHeaderRevisionID AND MFG_BHRv.IsActive=1 AND MFG_BHRv.IsDeleted=0 JOIN MFG_RoutingDetailsActivityCapture MFG_RDAC ON MFG_RDAC.ProductionOrderHeaderID=MFG_POH.ProductionOrderHeaderID AND MFG_RDAC.isDeleted=0 where MFG_POH.ProductionOrderStatusID=2  AND   MFG_POH.IsActive=1 AND MFG_POH.IsDeleted=0  AND   MFG_RHRv.RoutingHeaderID=" + (CommonLogic.QueryString("routid") == "" ? "0" : CommonLogic.QueryString("routid"))) != 0)
            {
                resetError("Once the production process starts cannot modify 'Routing' details", true);
                return;
            }*/


            ViewState["RoutDocRefIsInsert"] = false;
            GridViewRow row = gvRoutDocRef.Rows[e.RowIndex];

            if (row != null)
            {
                string txtDocumentLabel = ((TextBox)row.FindControl("txtDocumentLabel")).Text;
                string ltHidRoutingDocumentReferenceID = ((Literal)row.FindControl("ltHidRoutingDocumentReferenceID")).Text;
                string txtDocumentName = ((TextBox)row.FindControl("txtDocumentName")).Text;
                string txtFileLocation = ((TextBox)row.FindControl("txtFileLocation")).Text;

                StringBuilder sql = new StringBuilder(2500);

                if (txtDocumentLabel == "")
                {
                    this.resetError("Document Label cannot be empty", true);
                    return;
                }

                sql.Append("EXEC  [dbo].[sp_MFG_UpsertRoutingDocRef]  ");
                sql.Append("@RoutingDocumentReferenceID=" + ltHidRoutingDocumentReferenceID);
                sql.Append(",@RoutingHeaderID=" + CommonLogic.QueryString("routid"));
                sql.Append(",@FileLocation=" + DB.SQuote(txtFileLocation));

                sql.Append(",@DocumentLabel=" + DB.SQuote(txtDocumentLabel));
                sql.Append(",@DocumentName=" + DB.SQuote(txtDocumentName));
                sql.Append(",@CreatedBy=" + cp.UserID);



                try
                {
                    DB.ExecuteSQL(sql.ToString());

                    gvRoutDocRef.EditIndex = -1;

                    this.gvRoutDocRef_buildGridData(this.gvRoutDocRef_buildGridData());

                    this.resetError("Successfully Updated", false);


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
                    this.resetError("Error while updating doc. ref. details", true);

                    return;
                }

            }
        }

        protected void gvRoutDocRef_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            /*
            if (Localization.ParseBoolean(ViewState["RoutDocRefIsInsert"].ToString()))
            {
                GridViewRow row = gvRoutDocRef.Rows[e.RowIndex];
                if (row != null)
                {
                    int iden = 0;
                    if (((Literal)row.FindControl("ltHidRoutingDocumentReferenceID")).Text != "")
                    {
                        iden = Convert.ToInt32(((Literal)row.FindControl("ltHidRoutingDocumentReferenceID")).Text);
                        this.deleteRotDocRefRow(iden);
                    }

                    resetError("New Item canceled", false);

                }

            }*/

            gvRoutDocRef.EditIndex = -1;
            this.gvRoutDocRef_buildGridData(this.gvRoutDocRef_buildGridData());
        }

        protected void lnkRotDocRedDelete_Click(object sender, EventArgs e)
        {
            /*
            if (DB.GetSqlN("select top 1 MFG_RDAC.RoutingDetailsActivityCaptureID AS N from MFG_ProductionOrderHeader MFG_POH JOIN MFG_RoutingHeader_Revision MFG_RHRv ON MFG_RHRv.RoutingHeaderRevisionID=MFG_POH.RoutingHeaderRevisionID AND  MFG_RHRv.IsActive=1 AND MFG_RHRv.IsDeleted=0  JOIN MFG_BOMHeader_Revision MFG_BHRv ON MFG_BHRv.BOMHeaderRevisionID=MFG_RHRv.BOMHeaderRevisionID AND MFG_BHRv.IsActive=1 AND MFG_BHRv.IsDeleted=0 JOIN MFG_RoutingDetailsActivityCapture MFG_RDAC ON MFG_RDAC.ProductionOrderHeaderID=MFG_POH.ProductionOrderHeaderID  where MFG_POH.ProductionOrderStatusID=2  AND  MFG_POH.IsActive=1 AND MFG_POH.IsDeleted=0  AND  MFG_RHRv.RoutingHeaderID=" + (CommonLogic.QueryString("routid") == "" ? "0" : CommonLogic.QueryString("routid"))) != 0)
            {
                resetError("Once the production process starts cannot modify Routing details", true);
                return;
            }*/


            string gvIDs = "";

            bool chkBox = false;
            //'Navigate through each row in the GridView for checkbox items
            foreach (GridViewRow gv in gvRoutDocRef.Rows)
            {
                CheckBox isDelete = (CheckBox)gv.FindControl("chkChildIsDelete");

                if (isDelete == null)
                    return;

                if (isDelete.Checked)
                {

                    chkBox = true;
                    // Concatenate GridView items with comma for SQL Delete
                    gvIDs += ((Literal)gv.FindControl("ltHidRoutingDocumentReferenceID")).Text.ToString() + ",";



                }
            }

            // Execute SQL Query only if checkboxes are checked to avoid any error with initial null string
            try
            {
                string DeleteSQL = "";
                if (chkBox)
                {
                    DeleteSQL = " delete from MFG_RoutingDocumentReference where RoutingDocumentReferenceID  IN (" + gvIDs.Substring(0, gvIDs.LastIndexOf(",")) + ")";  // //
                    DB.ExecuteSQL(DeleteSQL);
                    resetError("Successfully deleted the selected line items", false);
                }



            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while deleting selected line items", true);
            }

            this.gvRoutDocRef_buildGridData(this.gvRoutDocRef_buildGridData());


        }

        protected void deleteRotDocRefRow(int iden)
        {

            try
            {
                DB.ExecuteSQL("delete from MFG_RoutingDocumentReference where RoutingDocumentReferenceID =" + iden); // //

                this.gvRoutDocRef_buildGridData(this.gvRoutDocRef_buildGridData());
            }
            catch(Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while deleting  row", true);
            }

        }



        #endregion ..................... Routing Document Ref. Details ........................


        #region ------------ Activity level MCodes  ---------------------

        protected DataSet gvActivityMcode_buildGridData()
        {
            try
            {
                
                string sql = ViewState["vRoutingDetailsActivity_Components"].ToString();

                DataSet ds = DB.GetDS(sql, false);
                return ds;
            }
            catch (Exception ex) {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while loading components" , true);
                return null;
            }

        }
        protected void gvActivityMcode_buildGridData(DataSet ds)
        {
            gvActivityMcode.DataSource = ds;
            gvActivityMcode.DataBind();
            ds.Dispose();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "unblockMcodeDialog", "unblockMcodeDialog();", true);
        }

        protected void lnkAddComponents_Click(object sender, EventArgs e)
        {
            //ViewState["vRoutingDetailsActivity_Components"] = false;
            StringBuilder sql = new StringBuilder(2500);

            try
            {
                //ViewState["vRoutingActivityID"] = hifRoutingDetailsActivityIDNew.Value;

                if (ViewState["vRoutingActivityID"] != null)
                {

                    if (DB.GetSqlN("select top 1 MFG_RDAC.RoutingDetailsActivityCaptureID AS N from MFG_ProductionOrderHeader MFG_POH JOIN MFG_RoutingHeader_Revision MFG_RHRv ON MFG_RHRv.RoutingHeaderRevisionID=MFG_POH.RoutingHeaderRevisionID AND  MFG_RHRv.IsActive=1 AND MFG_RHRv.IsDeleted=0  JOIN MFG_BOMHeader_Revision MFG_BHRv ON MFG_BHRv.BOMHeaderRevisionID=MFG_RHRv.BOMHeaderRevisionID AND MFG_BHRv.IsActive=1 AND MFG_BHRv.IsDeleted=0 JOIN MFG_RoutingDetailsActivityCapture MFG_RDAC ON MFG_RDAC.ProductionOrderHeaderID=MFG_POH.ProductionOrderHeaderID AND MFG_RDAC.isDeleted=0  where MFG_POH.ProductionOrderStatusID IN (2,3,4,5)  AND  MFG_POH.IsActive=1 AND MFG_POH.IsDeleted=0 AND MFG_RDAC.RoutingDetailsActivityID=" + (ViewState["vRoutingActivityID"].ToString() == "" ? "0" : ViewState["vRoutingActivityID"].ToString()) + "  AND   MFG_RHRv.RoutingHeaderID=" + (ViewState["HeaderID"].ToString() == "" ? "0" : ViewState["HeaderID"].ToString())) != 0)
                    {
                        resetError("Production process is started, cannot modify 'Routing' details", true);
                        return;
                    }
                }

                gvActivityMcode.EditIndex = 0;
                gvActivityMcode.PageIndex = 0;
                DataSet dsParent = this.gvActivityMcode_buildGridData();
                DataRow row = dsParent.Tables[0].NewRow();
                row["RoutingDetailsActivity_MaterilaMasterID"] = 0;
                dsParent.Tables[0].Rows.InsertAt(row, 0);
                this.gvActivityMcode_buildGridData(dsParent);
                //this.resetError("New Component line item added", false);
                //ViewState["vRoutingDetailsActivity_Components"] = true;

            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                this.resetError("Error while inserting new component" + ex.Message , true);
            }
        }


        private string GetQueryStringValueFromRawUrl(string queryStringKey)
        {
            var currentUri = new Uri(HttpContext.Current.Request.Url.Scheme + "://" +
                HttpContext.Current.Request.Url.Authority +
                HttpContext.Current.Request.RawUrl);
            var queryStringCollection = HttpUtility.ParseQueryString((currentUri).Query);
            return queryStringCollection.Get(queryStringKey);
        }



        protected void lnkMcodeDelete_Click(object sender, EventArgs e)
        {

            if (DB.GetSqlN("select top 1 MFG_RDAC.RoutingDetailsActivityCaptureID AS N from MFG_ProductionOrderHeader MFG_POH JOIN MFG_RoutingHeader_Revision MFG_RHRv ON MFG_RHRv.RoutingHeaderRevisionID=MFG_POH.RoutingHeaderRevisionID AND  MFG_RHRv.IsActive=1 AND MFG_RHRv.IsDeleted=0  JOIN MFG_BOMHeader_Revision MFG_BHRv ON MFG_BHRv.BOMHeaderRevisionID=MFG_RHRv.BOMHeaderRevisionID AND MFG_BHRv.IsActive=1 AND MFG_BHRv.IsDeleted=0 JOIN MFG_RoutingDetailsActivityCapture MFG_RDAC ON MFG_RDAC.ProductionOrderHeaderID=MFG_POH.ProductionOrderHeaderID AND MFG_RDAC.isDeleted=0  where MFG_POH.ProductionOrderStatusID IN (2,3,4,5)   AND  MFG_POH.IsActive=1 AND MFG_POH.IsDeleted=0 AND MFG_RDAC.RoutingDetailsActivityID=" + (ViewState["vRoutingActivityID"].ToString() == "" ? "0" : ViewState["vRoutingActivityID"].ToString()) + "  AND   MFG_RHRv.RoutingHeaderID=" + (CommonLogic.QueryString("routid") == "" ? "0" : CommonLogic.QueryString("routid"))) != 0)
            {
                resetError("Production process is started, cannot modify 'Routing' details", true);
                return;
            }


            string gvIDs = "";

            bool chkBox = false;
            //'Navigate through each row in the GridView for checkbox items
            foreach (GridViewRow gv in gvActivityMcode.Rows)
            {
                CheckBox isDelete = (CheckBox)gv.FindControl("chkChildIsDelete");

                if (isDelete == null)
                    return;

                if (isDelete.Checked)
                {

                    chkBox = true;
                    // Concatenate GridView items with comma for SQL Delete
                    gvIDs += ((Literal)gv.FindControl("lthid_RoutingDetailsActivity_MaterilaMasterID")).Text.ToString() + ",";



                }
            }

            //Execute SQL Query only if checkboxes are checked to avoid any error with initial null string
            try
            {
                string DeleteSQL = "";
                if (chkBox)
                {
                    DeleteSQL = "DELETE MFG_RoutingDetailsActivity_MaterilaMaster where RoutingDetailsActivity_MaterilaMasterID in (" + gvIDs.Substring(0, gvIDs.LastIndexOf(",")) + ")";  // //
                    DB.ExecuteSQL(DeleteSQL);
                    LoadRoutDefciencyList();
                    resetError("Successfully deleted the selected line items", false);
                }



            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while deleting selected line items", true);
            }

            this.gvActivityMcode_buildGridData(this.gvActivityMcode_buildGridData());

        }

        protected void lnkMcodeClose_Click(object sender, EventArgs e)
        {
            this.gvActivityMcode_buildGridData(this.gvActivityMcode_buildGridData());
          
            Build_RoutingLineItems(Build_RoutingLineItems());

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "dialog", "CloseMcodeDialog()", true);

            this.gvActivityMcode_buildGridData(this.gvActivityMcode_buildGridData());

            Build_RoutingLineItems(Build_RoutingLineItems());

            LoadRoutDefciencyList();

        }

        protected void gvActivityMcode_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gvActivityMcode_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            GridViewRow row = gvActivityMcode.Rows[e.RowIndex];

            if (row != null)
            {

                if (DB.GetSqlN("select top 1 MFG_RDAC.RoutingDetailsActivityCaptureID AS N from MFG_ProductionOrderHeader MFG_POH JOIN MFG_RoutingHeader_Revision MFG_RHRv ON MFG_RHRv.RoutingHeaderRevisionID=MFG_POH.RoutingHeaderRevisionID AND  MFG_RHRv.IsActive=1 AND MFG_RHRv.IsDeleted=0  JOIN MFG_BOMHeader_Revision MFG_BHRv ON MFG_BHRv.BOMHeaderRevisionID=MFG_RHRv.BOMHeaderRevisionID AND MFG_BHRv.IsActive=1 AND MFG_BHRv.IsDeleted=0 JOIN MFG_RoutingDetailsActivityCapture MFG_RDAC ON MFG_RDAC.ProductionOrderHeaderID=MFG_POH.ProductionOrderHeaderID  AND MFG_RDAC.isDeleted=0 where MFG_POH.ProductionOrderStatusID IN (2,3,4,5)   AND  MFG_POH.IsActive=1 AND MFG_POH.IsDeleted=0 AND MFG_RDAC.RoutingDetailsActivityID=" + (ViewState["vRoutingActivityID"].ToString() == "" ? "0" : ViewState["vRoutingActivityID"].ToString()) + "  AND   MFG_RHRv.RoutingHeaderID=" + (CommonLogic.QueryString("routid") == "" ? "0" : CommonLogic.QueryString("routid"))) != 0)
                {
                    resetError("Production process is started, cannot modify 'Routing' details", true);
                    return;
                }


                string vRoutingDetailsActivity_MaterilaMasterID = ((Literal)row.FindControl("lthid_RoutingDetailsActivity_MaterilaMasterID")).Text;


                string vMcode = ((TextBox)row.FindControl("txtMcode")).Text;
                string vQuantity = ((TextBox)row.FindControl("txtQty")).Text;

                StringBuilder sql = new StringBuilder(2500);

                int vMMID = DB.GetSqlN("select MaterialMasterID as N from MMT_MaterialMaster where isactive=1 AND isdeleted=0 AND MCode=" + DB.SQuote(vMcode.Trim()));

                if (vMMID == 0)
                {
                    this.resetError("Invalid material", true);
                    return;

                }

                Decimal RQty = Convert.ToDecimal(vQuantity==""?"0":vQuantity);

                /*
                if (vQuantity == "0" || vQuantity == "0.0" || vQuantity == "0.00" || vQuantity == "")
                {
                    resetError("Invalid quantity", true);
                    return;
                }*/

                if (RQty<=0)
                {
                    resetError("Invalid quantity", true);
                    return;
                }

                int BOMMMID = DB.GetSqlN("select MM.MaterialMasterID AS N from MFG_BOMHeader MFG_BH JOIN MFG_BOMDetails MFG_BD ON MFG_BD.BOMHeaderID=MFG_BH.BOMHeaderID AND MFG_BD.IsActive=1 AND MFG_BD.IsDeleted=0 JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=MFG_BD.BOMMaterialMasterID AND MM.IsActive=1 AND MM.IsDeleted=0 where MFG_BH.IsActive=1 AND MFG_BH.IsDeleted=0 AND MFG_BH.BOMHeaderID=" + CommonLogic.IIF(hifBoMHeaderID.Value != "", hifBoMHeaderID.Value, "0") + "  AND MM.MCode=" + DB.SQuote(vMcode.Split('[')[0].Trim()));

                if (BOMMMID == 0)
                {
                    this.resetError("This material is not configured in 'BOM'", true);
                    return;
                }

                decimal BoMQty = 0;

                if (hifRoutingDocTypeID.Value != "2")
                {

                    BoMQty = DB.GetSqlNDecimal("select SUM(BOMQuantity) AS N from MFG_BOMDetails BD  where  BD.ParentMaterialMasterID  IN (select BOMMaterialMasterID from MFG_BOMDetails where BOMHeaderID=" + CommonLogic.IIF(hifBoMHeaderID.Value != "", hifBoMHeaderID.Value, "0") + " AND MaterialMasterRevisionID=" + hifFinishedMRvID.Value + ") AND BOMMaterialMasterID=" + vMMID + "  AND BOMHeaderID=" + CommonLogic.IIF(hifBoMHeaderID.Value != "", hifBoMHeaderID.Value, "0"));
                }
                else
                {
                    BoMQty = DB.GetSqlNDecimal("select SUM(BOMQuantity) AS N from MFG_BOMDetails BD JOIN MMT_MaterialMaster MM ON MM.MaterialMasterID=BD.BOMMaterialMasterID  AND  MM.IsActive=1 AND MM.IsDeleted=0 where BD.BOMHeaderID=" + CommonLogic.IIF(hifBoMHeaderID.Value != "", hifBoMHeaderID.Value, "0") + " AND BD.IsActive=1 AND BD.IsDeleted=0 AND MaterialMasterRevisionID is null AND BOMMaterialMasterID=" + vMMID);
                }

                if (RQty > BoMQty)
                {
                    this.resetError("Requested qty. is greater than available qty.  ( " + BoMQty +" )", true);
                    return;
                }
                if (DB.GetSqlNDecimal("select SUM(Quantity) AS N from MFG_RoutingDetails MFG_RD JOIN MFG_RoutingDetailsActivity MFG_RDA ON MFG_RD.RoutingDetailsID=MFG_RDA.RoutingDetailsID AND MFG_RDA.IsActive=1 AND MFG_RDA.IsDeleted=0 LEFT JOIN MFG_RoutingDetailsActivity_MaterilaMaster MFG_RDAM  ON  MFG_RDA.RoutingDetailsActivityID=MFG_RDAM.RoutingDetailsActivityID  AND MFG_RDAM.IsDeleted=0 and MFG_RDAM.RoutingDetailsActivity_MaterilaMasterID!=" + vRoutingDetailsActivity_MaterilaMasterID + " where MFG_RDAM.IsActive=1 AND MFG_RDAM.MaterialMasterID=" + vMMID + " AND MFG_RD.IsActive=1 AND MFG_RD.IsDeleted=0  AND MFG_RD.RoutingHeaderID=" + CommonLogic.QueryString("routid")) + Convert.ToDecimal(vQuantity) > BoMQty)
                {

                     this.resetError("Requested quantity is greater than available 'BOM' quantity", true);
                     return;
                }


 
                sql.Append("EXEC  [dbo].[sp_MFG_UpsertRoutingDetailsActivity_MaterilaMaster]  ");
                sql.Append("@RoutingDetailsActivity_MaterilaMasterID=" + vRoutingDetailsActivity_MaterilaMasterID);
                sql.Append(",@RoutingDetailsActivityID=" + ViewState["vRoutingActivityID"]);
                sql.Append(",@MaterialMasterID=" + vMMID);
                sql.Append(",@Quantity=" + RQty);
                sql.Append(",@CreatedBy=" + cp.UserID);
                try
                {
                    DB.ExecuteSQL(sql.ToString());

                    gvActivityMcode.EditIndex = -1;

                    this.gvActivityMcode_buildGridData(this.gvActivityMcode_buildGridData());

                    Build_RoutingLineItems(Build_RoutingLineItems());

                    LoadRoutDefciencyList();

                    this.resetError("Successfully Updated", false);


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
                    this.resetError("Error while updating 'Components'", true);
                }

            }

        }

        protected void gvActivityMcode_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //SSSSSSSS

            gvActivityMcode.EditIndex = e.NewEditIndex;
            this.gvActivityMcode_buildGridData(this.gvActivityMcode_buildGridData());

        }

        protected void gvActivityMcode_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

            gvActivityMcode.EditIndex = -1;
            this.gvActivityMcode_buildGridData(this.gvActivityMcode_buildGridData());

        }


        #endregion ------------ Activity level MCodes  ---------------------


        #region -----------------  Revision History -----------------------

        protected void lnkCreateNewRevision_Click(object sender, EventArgs e)
        {
            int NewRoutID = 0;
            try
            {

                if (DB.GetSqlN("select top 1 MFG_RC.OldRoutingDetailsActivityID AS N from MFG_RoutingClone MFG_RC JOIN MFG_RoutingDetailsActivity RDA ON RDA.RoutingDetailsActivityID=MFG_RC.OldRoutingDetailsActivityID AND RDA.IsActive=1 AND RDA.IsDeleted=0 JOIN MFG_RoutingDetails RD ON RD.RoutingDetailsID=RDA.RoutingDetailsID AND  RD.IsActive=1 AND RD.IsDeleted=0 where  MFG_RC.IsActive=1 AND MFG_RC.IsDeleted=0 AND RD.RoutingHeaderID=" + ViewState["HeaderID"]) != 0)
                {
                    resetError("This routing is already cloned", true);
                    return;
                }
                
                if (txtNewRevision.Text == "" || txtEffectedDate.Text=="" || txtRevRemarks.Text=="")
                {
                    resetError("Please check for mandatory fields", true);
                    return;
                }

                String OldRevision = DB.GetSqlS("select Revision S from MFG_RoutingHeader_Revision where RoutingDocumentTypeID=" + (hifRoutingDocTypeID.Value == "" ? "0" : hifRoutingDocTypeID.Value) + "  AND MaterialMasterRevisionID=" + hifMMRvID.Value + " AND IsActive=1 AND IsDeleted=0 order by Revision  desc");

                int compareValue = 0;

                compareValue = String.Compare(txtNewRevision.Text, OldRevision, StringComparison.Ordinal);

                if (compareValue <= 0)
                {
                    resetError("'New Revision' must be greater than the previous revision. Previous revision:"+OldRevision, true);
                    return;
                }

                if (!DateGreaterOrEqual(DateTime.ParseExact(txtEffectedDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture), DateTime.Now))
                {
                    resetError("Effective date should be greater than or equal to system date", true);
                    return;
                }

                if (OldRevision != "")
                {
                    if (!DateGreaterOrEqual(DateTime.ParseExact(txtEffectedDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture), DateTime.ParseExact(DB.GetSqlS("select convert(nvarchar(50),EffectiveDate,103) S from MFG_RoutingHeader_Revision where MaterialMasterRevisionID=" + hifMMRvID.Value + " AND IsActive=1 AND IsDeleted=0 order by Revision  desc"), "dd/MM/yyyy", CultureInfo.InvariantCulture)))
                    {
                        resetError("Effective date should be greater than previous revision effective date", true);
                        return;
                    }
                }



                if (DB.GetSqlN("select ROUV.RoutingHeaderRevisionID AS N  from MFG_RoutingHeader_Revision ROUV  WHERE  ROUV.RoutingDocumentTypeID=" + (hifRoutingDocTypeID.Value == "" ? "0" : hifRoutingDocTypeID.Value) + " AND ROUV.RoutingHeaderID=" + ViewState["HeaderID"] + "   AND  ROUV.Revision='" + txtNewRevision.Text + "'") != 0)
                {
                    resetError("Revision already exists", true);
                    return;
                }


                StringBuilder cmdUpdateRoutingHeaderDetails = new StringBuilder();
                cmdUpdateRoutingHeaderDetails.Append("DECLARE @NewHeaderID int ");
                cmdUpdateRoutingHeaderDetails.Append("Exec [dbo].[sp_MFG_CopyRoutingDetails] ");
                cmdUpdateRoutingHeaderDetails.Append("@RoutingHeaderID=" + ViewState["HeaderID"]);
                cmdUpdateRoutingHeaderDetails.Append(",@Revision='" + txtNewRevision.Text + "'");
                cmdUpdateRoutingHeaderDetails.Append(",@EffectiveDate='" + (DateTime.ParseExact(txtEffectedDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture)).Date.ToString("yyyy/MM/dd") + "'");
                cmdUpdateRoutingHeaderDetails.Append(",@Remarks='" + txtRevRemarks.Text + "'");
                cmdUpdateRoutingHeaderDetails.Append(",@CreatedBy=" + cp.UserID);
                cmdUpdateRoutingHeaderDetails.Append(",@NewRoutingHeaderID=@NewHeaderID OUTPUT select @NewHeaderID as N");

              
                NewRoutID = DB.GetSqlN(cmdUpdateRoutingHeaderDetails.ToString());

              
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while creating new revision", true);
                return;
            }

            Response.Redirect("Routing.aspx?routid=" + NewRoutID + "&status=success");
        }



        private bool DateGreaterOrEqual(DateTime dt1, DateTime dt2)
        {
            return DateTime.Compare(dt1.Date, dt2.Date) >= 0;
        }



        #endregion -----------------  Revision History -----------------------


        #region ----  Routing BOM  Deficiency List Dialog  --------


        protected DataSet gvRoutingBOMDefcList_buildGridData()
        {
            string sql = "EXEC [dbo].[sp_MFG_CheckRoutingBOMDeficiency] @RoutingHeaderID="+CommonLogic.QueryString("routid")+",@BOMHeaderID=" + CommonLogic.IIF(hifBoMHeaderID.Value != "", hifBoMHeaderID.Value, "0") + ",";
            if (hifRoutingDocTypeID.Value == "2")
            {
                sql += "@ParentMMID=0";
            }
            else
            {
                sql += "@ParentMMID=" + DB.GetSqlN("select MaterialMasterID AS N from MMT_MaterialMaster_Revision where MaterialMasterRevisionID=" + CommonLogic.IIF(hifFinishedMRvID.Value != "", hifFinishedMRvID.Value, "0"));
            }
            DataSet ds = DB.GetDS(sql, false);

            return ds;
        }

        protected void gvRoutingBOMDefcList_buildGridData(DataSet ds)
        {
            gvRoutingBOMDefcList.DataSource = ds.Tables[0];
            gvRoutingBOMDefcList.DataBind();
            ds.Dispose();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "unblockProdDialog", "unblockProdDialog();", true);

        }

        protected void gvRoutingBOMDefcList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem == null)
                return;

        }

        protected void gvRoutingBOMDefcList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {


            gvRoutingBOMDefcList.PageIndex = e.NewPageIndex;
            gvRoutingBOMDefcList.EditIndex = -1;
            this.gvRoutingBOMDefcList_buildGridData(this.gvRoutingBOMDefcList_buildGridData());

        }


        protected void lnkVewRoutingBOMDefc_Click(object sender, EventArgs e)
        {
            gvRoutingBOMDefcList_buildGridData(gvRoutingBOMDefcList_buildGridData());
        }

        #endregion ----  Routing BOM  Deficiency List Dialog  --------


        public void LoadRoutDefciencyList() {

            try
            {

                DataSet dsRoutBOMDefc = gvRoutingBOMDefcList_buildGridData();

                if (dsRoutBOMDefc.Tables[0].Rows.Count != 0)
                {
                    gvRoutingBOMDefcList_buildGridData(gvRoutingBOMDefcList_buildGridData());
                    lnkVewRoutingBOMDefc.Visible = true;
                    //tdRotBOMDefc.Visible = true;
                    lblRoutDefciency.Visible = true;
                    lnkVewRoutingBOMDefc.OnClientClick = "ProdOpenDialog();";
                }
                else
                {
                   // tdRotBOMDefc.Visible = false;
                    lnkVewRoutingBOMDefc.Visible = false;
                    lblRoutDefciency.Visible = false;
                }
            }
            catch (Exception ex) {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while loading routing deficiency list",true);
                return;
            }

        }


        #region ------------- TER Check Points --------------------


        protected DataSet gvTERCheckPoints_buildGridData()
        {
            string sql = ViewState["TERCheckPointsSQL"].ToString();
            DataSet ds = DB.GetDS(sql, false);

            return ds;

        }

        protected void gvTERCheckPoints_buildGridData(DataSet ds)
        {

            gvTERCheckPoints.DataSource = ds;
            gvTERCheckPoints.DataBind();
            ds.Dispose();


            ScriptManager.RegisterStartupScript(this, this.GetType(), "unblockTERDialog", "unblockTERDialog();", true);
        }




        protected void lnkAddTERCheckPoint_Click(object sender, EventArgs e)
        {


            if (!cp.IsInRole("-3"))
            {
                resetError("Only 'QC Operator' can configure checkpoints", true);
                return;
            }

            StringBuilder sql = new StringBuilder();

            try
            {
                /*
                if (DB.GetSqlN("select top 1 MFG_RDAC.RoutingDetailsActivityCaptureID AS N from MFG_ProductionOrderHeader MFG_POH JOIN MFG_RoutingHeader_Revision MFG_RHRv ON MFG_RHRv.RoutingHeaderRevisionID=MFG_POH.RoutingHeaderRevisionID AND  MFG_RHRv.IsActive=1 AND MFG_RHRv.IsDeleted=0  JOIN MFG_BOMHeader_Revision MFG_BHRv ON MFG_BHRv.BOMHeaderRevisionID=MFG_RHRv.BOMHeaderRevisionID AND MFG_BHRv.IsActive=1 AND MFG_BHRv.IsDeleted=0 JOIN MFG_RoutingDetailsActivityCapture MFG_RDAC ON MFG_RDAC.ProductionOrderHeaderID=MFG_POH.ProductionOrderHeaderID AND MFG_RDAC.isDeleted=0  where MFG_POH.ProductionOrderStatusID=2  AND  MFG_POH.IsActive=1 AND MFG_POH.IsDeleted=0 AND MFG_RDAC.RoutingDetailsActivityID=" + (ViewState["vRoutingDetailsActivityID1"].ToString() == "" ? "0" : ViewState["vRoutingDetailsActivityID1"].ToString()) + "  AND   MFG_RHRv.RoutingHeaderID=" + (CommonLogic.QueryString("routid") == "" ? "0" : CommonLogic.QueryString("routid"))) != 0)
                {
                    resetError("Once the production process starts cannot modify 'Routing' details", true);
                    return;
                }*/

                /*
                int CheckListTypeID = DB.GetSqlN("select QCCheckListConfigurationID AS N from MFG_RoutingDetailsActivity where RoutingDetailsActivityID=" + ViewState["vRoutingDetailsActivityID1"]);

                if (CheckListTypeID == 0 || CheckListTypeID != 5)
                {
                    resetError("Please update check list type or  invalid check list type", true);
                    return;
                }*/


                if (DB.GetSqlN("select top 1 TER_C.RoutingDetailsActivity_TERID AS N from MFG_TERCapture TER_C JOIN MFG_RoutingDetailsActivity_TERCheckPoints TER_Chp ON TER_Chp.RoutingDetailsActivity_TERID=TER_C.RoutingDetailsActivity_TERID AND TER_Chp.IsDeleted=0 AND TER_Chp.IsActive=1 JOIN MFG_RoutingHeader_Revision RHRv ON RHRv.RoutingHeaderID=TER_Chp.RoutingHeaderID AND RHRv.IsDeleted=0 AND RHRv.IsActive=1 JOIN MFG_ProductionOrderHeader POH ON POH.RoutingHeaderRevisionID=RHRv.RoutingHeaderRevisionID AND POH.ProductionOrderStatusID IN (2,3,4,5)   AND POH.IsDeleted=0 AND POH.IsActive=1 where TER_C.IsActive=1 AND TER_C.IsDeleted=0 AND RHRv.RoutingHeaderID=" + (CommonLogic.QueryString("routid") == "" ? "0" : CommonLogic.QueryString("routid"))) != 0)
                {
                    resetError("Production process is started, cannot modify 'Routing' details", true);
                    return;
                }


                gvTERCheckPoints.EditIndex = 0;
                gvTERCheckPoints.PageIndex = 0;
                DataSet dsParent = this.gvTERCheckPoints_buildGridData();
                DataRow row = dsParent.Tables[0].NewRow();
                row["RoutingDetailsActivity_TERID"] = 0;
              
                dsParent.Tables[0].Rows.InsertAt(row, 0);
                this.gvTERCheckPoints_buildGridData(dsParent);
                
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                this.resetError("Error while inserting new check point", true);
            }




        }

        protected void gvTERCheckPoints_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTERCheckPoints.PageIndex = e.NewPageIndex;
            gvTERCheckPoints.EditIndex = -1;
            this.gvTERCheckPoints_buildGridData(this.gvTERCheckPoints_buildGridData());
        }

        protected void gvTERCheckPoints_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem == null)
                return;
        }

        protected void gvTERCheckPoints_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvTERCheckPoints.EditIndex = e.NewEditIndex;
            this.gvTERCheckPoints_buildGridData(this.gvTERCheckPoints_buildGridData());


          
        }

        protected void gvTERCheckPoints_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {


            if (!cp.IsInRole("-3"))
            {
                resetError("Only 'QC Operator' can update checkpoints", true);
                return;
            }
            
            //Page.Validate("UpdateTERCheckPoints");
            /*
            if (!Page.IsValid)
            {
                resetError("Please check for mandatory fields", true);
                return;
            }*/
            /*
            if (DB.GetSqlN("select top 1 MFG_RDAC.RoutingDetailsActivityCaptureID AS N from MFG_ProductionOrderHeader MFG_POH JOIN MFG_RoutingHeader_Revision MFG_RHRv ON MFG_RHRv.RoutingHeaderRevisionID=MFG_POH.RoutingHeaderRevisionID AND  MFG_RHRv.IsActive=1 AND MFG_RHRv.IsDeleted=0  JOIN MFG_BOMHeader_Revision MFG_BHRv ON MFG_BHRv.BOMHeaderRevisionID=MFG_RHRv.BOMHeaderRevisionID AND MFG_BHRv.IsActive=1 AND MFG_BHRv.IsDeleted=0 JOIN MFG_RoutingDetailsActivityCapture MFG_RDAC ON MFG_RDAC.ProductionOrderHeaderID=MFG_POH.ProductionOrderHeaderID AND MFG_RDAC.isDeleted=0  where MFG_POH.ProductionOrderStatusID=2  AND  MFG_POH.IsActive=1 AND MFG_POH.IsDeleted=0 AND MFG_RDAC.RoutingDetailsActivityID=" + (ViewState["vRoutingDetailsActivityID1"].ToString() == "" ? "0" : ViewState["vRoutingDetailsActivityID1"].ToString()) + "  AND   MFG_RHRv.RoutingHeaderID=" + (CommonLogic.QueryString("routid") == "" ? "0" : CommonLogic.QueryString("routid"))) != 0)
            {
                resetError("Once the production process starts cannot modify 'Routing' details", true);
                return;
            }*/

            GridViewRow row = gvTERCheckPoints.Rows[e.RowIndex];

            if (row != null)
            {

                string ltRoutingDetailsActivity_TERID = ((Literal)row.FindControl("ltRoutingDetailsActivity_TERID")).Text;
                string txtWireID = ((TextBox)row.FindControl("txtWireID")).Text;
                string txtCoreID = ((TextBox)row.FindControl("txtCoreID")).Text;
                string txtCircleNo = ((TextBox)row.FindControl("txtCircleNo")).Text;
                string txtDesignator1 = ((TextBox)row.FindControl("txtDesignator1")).Text;
                string txtPinNo1 = ((TextBox)row.FindControl("txtPinNo1")).Text;
                string txtDesignator2 = ((TextBox)row.FindControl("txtDesignator2")).Text;
                string txtPinNo2 = ((TextBox)row.FindControl("txtPinNo2")).Text;

                if (txtWireID == "" || txtCoreID == "" || txtCircleNo == "" || txtDesignator1 == "" || txtPinNo1 == "" || txtDesignator2 == "" || txtPinNo2 == "")
                {
                    resetError("Please check for mandatory fields", true);
                    return;
                }


                if (DB.GetSqlN("select top 1 TER_C.RoutingDetailsActivity_TERID AS N from MFG_TERCapture TER_C JOIN MFG_RoutingDetailsActivity_TERCheckPoints TER_Chp ON TER_Chp.RoutingDetailsActivity_TERID=TER_C.RoutingDetailsActivity_TERID AND TER_Chp.IsDeleted=0 AND TER_Chp.IsActive=1 JOIN MFG_RoutingHeader_Revision RHRv ON RHRv.RoutingHeaderID=TER_Chp.RoutingHeaderID AND RHRv.IsDeleted=0 AND RHRv.IsActive=1 JOIN MFG_ProductionOrderHeader POH ON POH.RoutingHeaderRevisionID=RHRv.RoutingHeaderRevisionID AND POH.ProductionOrderStatusID IN (2,3,4,5)   AND POH.IsDeleted=0 AND POH.IsActive=1 where TER_C.IsActive=1 AND TER_C.IsDeleted=0 AND RHRv.RoutingHeaderID=" + (CommonLogic.QueryString("routid") == "" ? "0" : CommonLogic.QueryString("routid")) + "  AND  TER_C.RoutingDetailsActivity_TERID=" + ltRoutingDetailsActivity_TERID) != 0)
                {
                    resetError("Production process is started, cannot modify 'Routing' details", true);
                    return;
                }

                
                string vtxtParameterDataType = ((TextBox)row.FindControl("txtParameterDataType")).Text;

                string vtxtMinValue = ((TextBox)row.FindControl("txtMinValue")).Text == "" ? "0" : ((TextBox)row.FindControl("txtMinValue")).Text;
                string vtxtMaxValue = ((TextBox)row.FindControl("txtMaxValue")).Text == "" ? "0" : ((TextBox)row.FindControl("txtMaxValue")).Text;
                

                StringBuilder sql = new StringBuilder(2500);

              
                //int CheckListTypeID = DB.GetSqlN("select QCCheckListConfigurationID AS N from MFG_RoutingDetailsActivity where RoutingDetailsActivityID=" + ViewState["vRoutingDetailsActivityID1"]);

                /*
                if (CheckListTypeID != 0 && CheckListTypeID != 3)
                {
                    int SeperatorCount = DB.GetSqlN("select SeparatorCount AS N from MFG_QCCheckListConfiguration where QCCheckListConfigurationID=" + CheckListTypeID);

                    string[] CheckPointArray = vtxtCheckPointDescription.Split('|');

                    if (CheckPointArray.Length != SeperatorCount) {

                        this.resetError("Invalid check point", true);
                        return;
                    }

                }*/
                /*
                if (CheckListTypeID == 0 || CheckListTypeID != 5)
                {
                    this.resetError("Please update check list type or  invalid check list type", true);
                    return;
                }*/



                String vtxtParameterDataTypeID = "0";
                if (vtxtParameterDataType != "")
                {

                    vtxtParameterDataTypeID = DB.GetSqlN("select top 1 ParameterDataTypeID AS N from  GEN_ParameterDataType where IsActive=1 AND IsDeleted=0 AND ParameterDataType=" + DB.SQuote(vtxtParameterDataType)).ToString();

                    if (vtxtParameterDataTypeID == "0")
                    {
                        this.resetError("Invalid parameter type", true);
                        return;
                    }
                }


                sql.Append("EXEC [dbo].[sp_MFG_UpsertRoutingDetailsActivity_TERCheckPoints] ");
                sql.Append("@RoutingDetailsActivity_TERID=" + ltRoutingDetailsActivity_TERID);
                sql.Append(",@RoutingHeaderID=" + (CommonLogic.QueryString("routid") == "" ? "0" : CommonLogic.QueryString("routid")));
                sql.Append(",@WireID=" + DB.SQuote( txtWireID));
                sql.Append(",@CoreID="+ DB.SQuote(txtCoreID));
                sql.Append(",@CircleNo=" + DB.SQuote(txtCircleNo));
                sql.Append(",@Designator1=" + DB.SQuote(txtDesignator1));
                sql.Append(",@PinNo1=" + DB.SQuote(txtPinNo1));
                sql.Append(",@Designator2="+ DB.SQuote(txtDesignator2));
                sql.Append(",@PinNo2=" + DB.SQuote( txtPinNo2));
                sql.Append(",@ParameterDataTypeID="+(vtxtParameterDataTypeID=="0"?"NULL":vtxtParameterDataTypeID));
                             
                sql.Append(",@CreatedBy=" + cp.UserID);


                decimal vMinTollerance = Convert.ToDecimal(vtxtMinValue);
                decimal vMaxTollerance = Convert.ToDecimal(vtxtMaxValue);

                if (vtxtParameterDataTypeID == "2" || vtxtParameterDataTypeID == "1")
                {
                    if (vMaxTollerance == 0 || vMaxTollerance == 0)
                    {
                        this.resetError("Tolerance value cannot be 0 or empty", true);
                        return;
                    }

                    if (vMaxTollerance < vMinTollerance)
                    {
                        this.resetError(" Max. tolerance value should be greater than Min. tolerance value", true);
                        return;
                    }


                    sql.Append(",@MinValue=" + vMinTollerance);
                    sql.Append(",@MaxValue=" + vMaxTollerance);

                }
                else
                {
                    sql.Append(",@MinValue=NULL");
                    sql.Append(",@MaxValue=NULL");
                }



                try
                {
                    DB.ExecuteSQL(sql.ToString());

                    gvTERCheckPoints.EditIndex = -1;

                    this.gvTERCheckPoints_buildGridData(this.gvTERCheckPoints_buildGridData());
                    build_FormData();
                    this.resetError("Successfully Updated", false);


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
                    this.resetError("Error while updating check points", true);

                    return;
                }

            }




            


        }

        protected void gvTERCheckPoints_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvTERCheckPoints.EditIndex = -1;
            this.gvTERCheckPoints_buildGridData(this.gvTERCheckPoints_buildGridData());
        }

        protected void gvTERCheckPoints_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void lnkTERCancel_Click(object sender, EventArgs e)
        {

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "dialog", "closeTERDialog1()", true);

            this.gvTERCheckPoints_buildGridData(gvTERCheckPoints_buildGridData());
            Build_RoutingLineItems(Build_RoutingLineItems());
            build_FormData();
        }

        protected void lnkTERDelete_Click(object sender, EventArgs e)
        {

            if (!cp.IsInRole("-3"))
            {
                resetError("Only 'QC Operator' can modify these details", true);
                return;
            }
            /*
            if (DB.GetSqlN("select top 1 MFG_RDAC.RoutingDetailsActivityCaptureID AS N from MFG_ProductionOrderHeader MFG_POH JOIN MFG_RoutingHeader_Revision MFG_RHRv ON MFG_RHRv.RoutingHeaderRevisionID=MFG_POH.RoutingHeaderRevisionID AND  MFG_RHRv.IsActive=1 AND MFG_RHRv.IsDeleted=0  JOIN MFG_BOMHeader_Revision MFG_BHRv ON MFG_BHRv.BOMHeaderRevisionID=MFG_RHRv.BOMHeaderRevisionID AND MFG_BHRv.IsActive=1 AND MFG_BHRv.IsDeleted=0 JOIN MFG_RoutingDetailsActivityCapture MFG_RDAC ON MFG_RDAC.ProductionOrderHeaderID=MFG_POH.ProductionOrderHeaderID AND MFG_RDAC.isDeleted=0 where MFG_POH.ProductionOrderStatusID=2  AND  MFG_POH.IsActive=1 AND MFG_POH.IsDeleted=0 AND MFG_RDAC.RoutingDetailsActivityID=" + (ViewState["vRoutingDetailsActivityID1"].ToString() == "" ? "0" : ViewState["vRoutingDetailsActivityID1"].ToString()) + "  AND   MFG_RHRv.RoutingHeaderID=" + (CommonLogic.QueryString("routid") == "" ? "0" : CommonLogic.QueryString("routid"))) != 0)
            {
                resetError("Once the production process starts cannot modify 'Routing' details", true);
                return;
            }
            */

            string gvIDs = "";

            bool chkBox = false;
            //'Navigate through each row in the GridView for checkbox items
            foreach (GridViewRow gv in gvTERCheckPoints.Rows)
            {
                CheckBox isDelete = (CheckBox)gv.FindControl("chkTERChildIsDelete");

                if (isDelete == null)
                    return;

                if (isDelete.Checked)
                {

                    chkBox = true;
                    // Concatenate GridView items with comma for SQL Delete
                    gvIDs += ((Literal)gv.FindControl("ltRoutingDetailsActivity_TERID")).Text.ToString() + ",";

                    if (DB.GetSqlN("select top 1 TER_C.RoutingDetailsActivity_TERID AS N from MFG_TERCapture TER_C JOIN MFG_RoutingDetailsActivity_TERCheckPoints TER_Chp ON TER_Chp.RoutingDetailsActivity_TERID=TER_C.RoutingDetailsActivity_TERID AND TER_Chp.IsDeleted=0 AND TER_Chp.IsActive=1 JOIN MFG_RoutingHeader_Revision RHRv ON RHRv.RoutingHeaderID=TER_Chp.RoutingHeaderID AND RHRv.IsDeleted=0 AND RHRv.IsActive=1 JOIN MFG_ProductionOrderHeader POH ON POH.RoutingHeaderRevisionID=RHRv.RoutingHeaderRevisionID AND POH.ProductionOrderStatusID IN (2,3,4,5)   AND POH.IsDeleted=0 AND POH.IsActive=1 where TER_C.IsActive=1 AND TER_C.IsDeleted=0 AND RHRv.RoutingHeaderID=" + (CommonLogic.QueryString("routid") == "" ? "0" : CommonLogic.QueryString("routid")) + "  AND  TER_C.RoutingDetailsActivity_TERID=" + ((Literal)gv.FindControl("ltRoutingDetailsActivity_TERID")).Text.ToString()) != 0)
                    {
                        resetError("Production process is started, cannot modify 'Routing' details", true);
                        return;
                    }

                    
                }
            }

            // Execute SQL Query only if checkboxes are checked to avoid any error with initial null string
            try
            {
                string DeleteSQL = "";
                if (chkBox)
                {
                    DeleteSQL = "DELETE MFG_RoutingDetailsActivity_TERCheckPoints WHERE RoutingDetailsActivity_TERID IN (" + gvIDs.Substring(0, gvIDs.LastIndexOf(",")) + ")";  // //
                    DB.ExecuteSQL(DeleteSQL);

                    resetError("Successfully deleted the selected line items", false);
                }



            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while deleting line items", true);
            }

            this.gvTERCheckPoints_buildGridData(this.gvTERCheckPoints_buildGridData());
            build_FormData();
        }


        #endregion ------------- TER Check Points --------------------

        protected void lnkTestButton_Click(object sender, EventArgs e)
        {
            resetError("Error while loading routing deficiency list", true);
        }



    }
}
