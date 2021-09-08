using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21Common;
using System.Data.SqlClient;
using System.Data;
using System.Text;

namespace MRLWMSC21.mManufacturingProcess
{
     //Author   :   Gvd Prasad
    //Created On:   12-Aug-2014
    //Use case ID:  ECO_UC_015

    public partial class EngineeringChangeOrders : System.Web.UI.Page
    {
        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;

        protected void Page_PreInit(object sender, EventArgs e)
        {
           
            Page.Theme = "Inbound";

        }

        protected void page_Init(object sender, EventArgs e)
        {
            if (CommonLogic.QueryString("mmid") != "" && CommonLogic.QueryString("rdt") != "")
            {
                Biuld_OrderChangeData(Biuld_OrderChangeData());
                
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("ECOM"))))
            {
                //test case ID (TC_005)
                //Only authorized user can access ECOM page

                Response.Redirect("~/Login.aspx?eid=6");
            }
            Page.Form.DefaultButton = lnkGetDetails.UniqueID;
            DesignLogic.SetInnerPageSubHeading(this.Page, "Engineering Change Order");
            if (!IsPostBack)
            {
                if (CommonLogic.QueryString("mmid") != "" && CommonLogic.QueryString("rdt") != "")
                {
                    ddlRoutingDocumentType.SelectedValue = CommonLogic.QueryString("rdt");
                    lbMessage.Visible = true;
                }
            }
            if (CommonLogic.QueryString("success") == "true")
                resetError("Transferred successfully", false);
        }                
        
        protected void lnkGetDetails_Click(object sender, EventArgs e)
        {
            //test case ID (TC_009)
            //show all Job order history page

            if (atcPartNumber.Text == "" || hifPartNumber.Value == ""||ddlRoutingDocumentType.SelectedIndex==0)
            {
                resetError("Select part no. and routing type", true);
                return;
            }
            Response.Redirect("EngineeringChangeOrders.aspx?mmid="+hifPartNumber.Value+"&rdt="+ddlRoutingDocumentType.SelectedValue);
            //Biuld_OrderChangeData(Biuld_OrderChangeData());
           
        }

        private DataSet Biuld_OrderChangeData()
        {
            DataSet dsGetConfigureVBoMandRoutng=null;
            try
            {
                dsGetConfigureVBoMandRoutng = DB.GetDS("[sp_MFG_GetConfigureBoMandRoutingDetails]  @MaterialMasterID=" + CommonLogic.QueryString("mmid") + ",@RoutingDocumentTypeID=" + CommonLogic.QueryString("rdt"), false);
            }
            catch (Exception ex)
            {
                resetError("Error while loading",true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
            return dsGetConfigureVBoMandRoutng;
        }

        private void Biuld_OrderChangeData(DataSet dsGetConfigureVBoMandRoutng)
        {
            try
            {
                atcPartNumber.Text = DB.GetSqlS("select MCode as S from MMT_MaterialMaster where MaterialMasterID=" + CommonLogic.QueryString("mmid"));
            }catch(Exception ex)
            {
                resetError("Error while loading",true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
            hifPartNumber.Value = CommonLogic.QueryString("mmid");
            gvECO.DataSource = dsGetConfigureVBoMandRoutng;
            gvECO.DataBind();
            
            dsGetConfigureVBoMandRoutng.Dispose();
        }
             
        protected void resetError(string error, bool isError)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "closeAsynchronus(); showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);
        }

        protected void gvECO_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && !(e.Row.RowState == DataControlRowState.Edit))
            {
                Literal kkk=((Literal)e.Row.FindControl("ltJobList"));
                String JobList = kkk.Text;
                Panel pnlJobList = (Panel)e.Row.FindControl("pnlJobList");
                 
                if (JobList != "")
                {
                    LinkButton lnkJobList = null;
                    String[] JobData = null;
                    String[] JobListdata = JobList.Split(',');
                    for (int index = 0; index < JobListdata.Length; index++)
                    {
                        JobData = JobListdata[index].Split('!');
                        lnkJobList = new LinkButton();
                        lnkJobList.CommandName = "JobDetails";
                        lnkJobList.CommandArgument = JobData[0];
                        lnkJobList.Text = JobData[1];
                        lnkJobList.Font.Underline = false;
                        
                        
                        if (JobData[2] == "0")
                        {
                            lnkJobList.Text = "Upgrade ( "+JobData[1]+" )";
                            lnkJobList.ForeColor = System.Drawing.Color.Green;
                            lnkJobList.ToolTip = "Pending Job Order for Upgrade New Routing Revision";
                            lnkJobList.OnClientClick = "return confirm('Are you sure, you want to upgrade to 'New Routing Revision' ?');showAsynchronus();";
                            
                            //lnkJobList.Click+=lnkJobList_Click;
                        }
                        else if (JobData[2] == "1")
                        {

                            lnkJobList.ForeColor = System.Drawing.Color.Gray;
                            lnkJobList.ToolTip = "Pending Job Order but Routing Revision not in process";
                            lnkJobList.Enabled = false;
                        }
                        else if (JobData[2] == "2")
                        {

                            lnkJobList.ForeColor = System.Drawing.Color.Black;
                            lnkJobList.ToolTip = "Pending Job Order but Closed";
                            lnkJobList.Enabled = false;
                        }
                        else
                        {
                            lnkJobList.ForeColor = System.Drawing.Color.Black;
                            lnkJobList.ToolTip = "Upgraded to new Routing Revision";
                            lnkJobList.Enabled = false;
                        }
                        pnlJobList.Controls.Add(lnkJobList);
                        pnlJobList.Controls.Add(new LiteralControl("<br/>"));
                    }
                }
            }
        }
  
        protected void gvECO_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //test case ID (TC_014) 
            //select job order transfer to clone version
           

            StringBuilder cMdCheckRoutingColne = new StringBuilder();
            cMdCheckRoutingColne.Append("select TOP 1 1 AS N from MFG_RoutingHeader_Revision ROUR ");
            cMdCheckRoutingColne.Append(" JOIN MFG_RoutingDetails ROUD ON ROUD.RoutingHeaderID=ROUR.RoutingHeaderID AND ROUD.IsDeleted=0 ");
            cMdCheckRoutingColne.Append(" JOIN MFG_RoutingDetailsActivity ROUDA ON ROUDA.RoutingDetailsID=ROUD.RoutingDetailsID AND ROUDA.IsDeleted=0 ");
            cMdCheckRoutingColne.Append(" JOIN MFG_RoutingClone ROUC ON ROUC.OldRoutingDetailsActivityID=ROUDA.RoutingDetailsActivityID ");
            cMdCheckRoutingColne.Append(" JOIN MFG_ProductionOrderHeader PROH ON PROH.RoutingHeaderRevisionID=ROUR.RoutingHeaderRevisionID ");
            cMdCheckRoutingColne.Append(" WHERE PROH.ProductionOrderHeaderID=" + e.CommandArgument.ToString());
            int NewRoutingHeaderID = 0;
            try
            {
                int CheckRevisionClone = DB.GetSqlN(cMdCheckRoutingColne.ToString());

                if (CheckRevisionClone == 0)
                {
                    //test case ID (TC_015)
                    //details are not available

                    resetError("No Clone Available for Current Routing", true);
                    return;
                }
                String cMdChangeNewVersion = "DECLARE @GetResult INT EXEC dbo.sp_MFG_ChangeNewRevisionToJobOrder @ProductionOrderHeaderID=" + e.CommandArgument.ToString()+ ",@UpdatedBy="+cp.UserID.ToString() + " ,@Result=@GetResult  OUTPUT Select @GetResult as N";
                NewRoutingHeaderID = DB.GetSqlN(cMdChangeNewVersion);
                
            }
            catch (Exception ex)
            {
                resetError("Error while updating",true);
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
            if (NewRoutingHeaderID != 0)
            {
                Biuld_OrderChangeData();
                resetError("Successfully Updated", false);
                Response.Redirect("EngineeringChangeOrders.aspx?mmid=" + hifPartNumber.Value + "&rdt="+ddlRoutingDocumentType.SelectedValue+"&success=true");
            }
            else
            {
                resetError("Error while updating", true);
            }
        }
    }
}