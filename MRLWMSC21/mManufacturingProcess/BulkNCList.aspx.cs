using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Security.Cryptography;
using MRLWMSC21Common;


// Module Name : Manufacturing
// Usecase Ref.: Bulk NC list_ UC_ 013
// DevelopedBy : Naresh P
// CreatedOn   : 05/05/2014
// Modified On : 24/03/2015


namespace MRLWMSC21.mManufacturingProcess
{
    public partial class BulkNCList : System.Web.UI.Page
    {



        public CustomPrincipal cp = HttpContext.Current.User as CustomPrincipal;
        public String PrHID = "";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!cp.IsInAnyRoles(CommonLogic.GetRolesAllowed(CommonLogic.GetRolesAllowedForthisPage("Bulk NC List"))))
            {
                Response.Redirect("../Login.aspx?eid=6");
            }

            if (!IsPostBack) {

                DesignLogic.SetInnerPageSubHeading(this.Page, "Bulk NC List");

            }
        }

        public void PopulateRootNode() {

            try
            {

                IDataReader drRD = DB.GetRS("select distinct RD.RoutingDetailsID,RD.OperationNumber,RD.Name,RD.DisplayNumber from MFG_ProductionOrderHeader PROH JOIN MFG_RoutingHeader_Revision RHRv ON RHRv.RoutingHeaderRevisionID=PROH.RoutingHeaderRevisionID AND RHRv.IsActive=1 AND RHRv.IsDeleted=0 JOIN MFG_RoutingDetails RD ON RD.RoutingHeaderID=RHRv.RoutingHeaderID AND RD.IsActive=1 AND RD.IsDeleted=0 WHERE PROH.IsActive=1 AND PROH.IsDeleted=0 AND PROH.ProductionOrderStatusID=2 AND PROH.ProductionOrderHeaderID=" + PrHID + " order by RD.DisplayNumber ASC");

                while (drRD.Read())
                {
                    TreeNode tnRoutDet = new TreeNode();
                    tnRoutDet.Text = "<span style='color:#FF8000;font-family:Calibri;font-weight:bold;font-size:18pt;'> " + DB.RSField(drRD, "OperationNumber") + "</span> <span style='font-weight:bold;color:#0B4C5F;font-family:Calibri;font-size:15pt;'>" + DB.RSField(drRD, "Name") + "</span>";
                    tnRoutDet.Value = DB.RSFieldInt(drRD, "RoutingDetailsID").ToString();
                    tnRoutDet.SelectAction = TreeNodeSelectAction.None;

                    tvBulkNCList.Nodes.Add(tnRoutDet);


                    PopulateActivityNode(tnRoutDet, DB.RSFieldInt(drRD, "RoutingDetailsID"));
                }

                drRD.Close();

            }
            catch (Exception ex) {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }

        }

        public void PopulateActivityNode(TreeNode tn,int RoutingDetailID) {
            try
            {

                IDataReader drAC = DB.GetRS("select distinct RDA.RoutingDetailsActivityID,RDA.ActivityCode,RDA.Description,RDA.DisplayOrder from MFG_ProductionOrderHeader PROH JOIN MFG_RoutingHeader_Revision RHRv ON RHRv.RoutingHeaderRevisionID=PROH.RoutingHeaderRevisionID AND  RHRv.isActive=1 AND RHRv.IsDeleted=0 JOIN MFG_RoutingDetails RD ON RD.RoutingHeaderID=RHRv.RoutingHeaderID AND  RD.isActive=1 AND RD.IsDeleted=0 JOIN MFG_RoutingDetailsActivity RDA ON RDA.RoutingDetailsID=RD.RoutingDetailsID AND  RDA.isActive=1 AND RDA.IsDeleted=0 WHERE PROH.IsActive=1 AND PROH.IsDeleted=0 AND PROH.ProductionOrderStatusID=2 AND  RDA.RoutingDetailsID=" + RoutingDetailID + "  AND  PROH.ProductionOrderHeaderID=" + PrHID + " order by RDA.DisplayOrder ASC");

                while (drAC.Read())
                {

                    TreeNode tnActivityDet = new TreeNode();
                    tnActivityDet.Text = "<span style='color:#86B404;font-family:Calibri;font-weight:bold;font-size:15pt;'> " + DB.RSField(drAC, "ActivityCode") + "</span> <span style='font-weight:bold;color:#424242;font-family:Calibri;font-size:13pt;'>" + DB.RSField(drAC, "Description") + "</span>";
                    tnActivityDet.Value = DB.RSFieldInt(drAC, "RoutingDetailsActivityID").ToString();
                    tnActivityDet.SelectAction = TreeNodeSelectAction.None;

                    tn.ChildNodes.Add(tnActivityDet);


                    PopulateActivityMaterials(tnActivityDet, DB.RSFieldInt(drAC, "RoutingDetailsActivityID"));
                }

                drAC.Close();
            }
            catch (Exception ex) {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
        }

        public void PopulateActivityMaterials(TreeNode tn,int RoutindDetailsActivityID) {

            try
            {

                IDataReader drMM = DB.GetRS("[dbo].[sp_MFG_GetBulkNCUpdateMaterialDetails] @ProductionOrderHeaderID=" + PrHID + ",@RoutingDetailsActivityID=" + RoutindDetailsActivityID);

                Random rnd = new Random();

                String randValue = "";

                int i = 1;
                while (drMM.Read())
                {
                    TreeNode tnActivityMM = new TreeNode();

                    randValue = Get8DigitNumber(); 

                    tnActivityMM.Text = "<span style='font-family:Calibri;font-size:13pt;'> " + DB.RSField(drMM, "MCode") + DB.RSField(drMM, "Oempartno") + DB.RSField(drMM, "BatchNo1") + " | Qty: " + DB.RSFieldDecimal(drMM, "Quantity") + "</span>  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  <input onBlur=\"CheckGMDQty(this," + DB.RSFieldDecimal(drMM, "Quantity") + ")\"   onKeyPress=\"return checkDec(this,event)\"  value=\"" + DB.RSFieldDecimal(drMM, "Quantity") + "\"  id=\"txtField\" type=text width=\"" + "30px" + "\" name=\"" + randValue + DB.RSFieldInt(drMM, "MaterialMasterID").ToString() + i + "\" runat=\"server\" /> ";

                  //  tnActivityMM.Text += "  &nbsp; | NC Qty: <input onBlur=\"CheckGMDQty(this," + DB.RSFieldDecimal(drMM, "Quantity") + ")\"   onKeyPress=\"return checkDec(this,event)\"  value=\"" + DB.RSFieldDecimal(drMM, "Quantity") + "\"  id=\"txtField\" type=text width=\"" + "30px" + "\" name=\"" + randValue + DB.RSFieldInt(drMM, "MaterialMasterID").ToString() + i + "\" runat=\"server\" /> ";

                  //  tnActivityMM.Value = DB.RSFieldInt(drMM, "RoutingDetailsActivityCaptureID").ToString() + " ~ " + DB.RSFieldInt(drMM, "MaterialMasterID").ToString() + " ~ " + DB.RSField(drMM, "BatchNo").ToString() + " ~ " + DB.RSFieldInt(drMM, "WorkCenterID").ToString(); 
                    tnActivityMM.Value = DB.RSFieldInt(drMM, "RoutingDetailsActivityID").ToString() + " ~ " + DB.RSFieldInt(drMM, "MaterialMasterID").ToString() + " ~ " + DB.RSField(drMM, "BatchNo").ToString() + " ~ " + DB.RSFieldInt(drMM, "WorkCenterID").ToString(); 
                    tnActivityMM.SelectAction = TreeNodeSelectAction.None;
                    tnActivityMM.NavigateUrl = randValue + DB.RSFieldInt(drMM, "MaterialMasterID").ToString() + i;
                    //tnActivityMM.Target = DB.RSFieldInt(drMM, "RoutingDetailsActivityCaptureID").ToString();
                    tnActivityMM.Target = DB.RSFieldInt(drMM, "RoutingDetailsActivityID").ToString();
                    tn.ChildNodes.Add(tnActivityMM);

                    i++;
                }

                drMM.Close();
            }
            catch (Exception ex) {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }

        }

        protected void lnkGetDetails_Click(object sender, EventArgs e)
        {

            try
            {

                if (txtKitCode.Text=="" || txtJobOrderRefNo.Text=="")
                {
                    resetError("Please check for mandatory fields", true);
                    return;
                }


                tvBulkNCList.Nodes.Clear();
                PrHID = hifJobRefNoNumber.Value;

                ViewState["ProhID"] = hifJobRefNoNumber.Value;
                hifActivityID.Value = "";

                if (PrHID != "")
                {
                    PopulateRootNode();
                }
                tvBulkNCList.ExpandAll();
            }
            catch (Exception ex) {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while loading", true);
            }
        }

        protected void lnkTvSubmit_Click(object sender, EventArgs e)
        {

            try
            {

                Page.Validate("UpdateRev");

                if (!Page.IsValid)
                {
                    resetError("Please check for mandatory fields",true);
                    return;
                }


                if (hifActivityID.Value == "") {
                    resetError("Please select activity no which you want to scrap", true);
                    return;
                }

                String sysName=System.Environment.MachineName;

                StringBuilder sbTreeNodeCollection = new StringBuilder();
                StringBuilder sbTreeActivityNodeIDCollection = new StringBuilder();
                StringBuilder sbTreeActivityNodeCaptureIDCollection = new StringBuilder(); 

                TreeNodeCollection tnColl = tvBulkNCList.Nodes;


                foreach (TreeNode tnOpr in tnColl) {
                    foreach (TreeNode tnAct in tnOpr.ChildNodes)
                    {
                        if (tnAct.Checked)
                        {
                            sbTreeActivityNodeIDCollection.Append(tnAct.Value + ",");
                           
                            foreach (TreeNode tnActMM in tnAct.ChildNodes)
                            {

                                if (tnActMM.Checked)
                                {

                                    sbTreeActivityNodeCaptureIDCollection.Append(tnActMM.Target + ",");

                                    if (Convert.ToDecimal(Page.Request.Form[tnActMM.NavigateUrl].ToString()) == 0)
                                    {
                                        resetError("Qantity cannot be 0 or empty", true);
                                        return;
                                    }
                                    
                                    sbTreeNodeCollection.Append(tnActMM.Value + " ~ " + Page.Request.Form[tnActMM.NavigateUrl].ToString() + ",");

                                    
                                }

                            }
                        }
                    }

                }


                String OBDNumber = GetOBDNumber();

                String SONumber = GetSONumber();

                String CUSPONumber = GetCUSPONumber();

                String IORefNumber = GetIORefNo();

                StringBuilder SQLUpdate = new StringBuilder();

                if ( sbTreeActivityNodeIDCollection.ToString() == "") {
                    resetError("Please select activity or material which you want to scrap", true);
                    return;
                }


                if (OBDNumber != "" && SONumber != "" && CUSPONumber != "" && IORefNumber != "" && hifWorkCenter.Value != "" && hifJobRefNoNumber.Value != "" && hifActivityID.Value != "")
                {

                    SQLUpdate.Append("DECLARE @NewUpdateOutboundID int; EXEC   [dbo].[sp_MFG_BulkNCUpdate] @ProductionOrderHeaderID=" + ViewState["ProhID"] + ",  @ActivityIds=" + DB.SQuote(sbTreeActivityNodeIDCollection.ToString()) + " , @ActivityCaptureIds=" + DB.SQuote(sbTreeActivityNodeCaptureIDCollection.ToString()) + ", 	 @GoodsMovementData=" + DB.SQuote(sbTreeNodeCollection.ToString()) + ", @ActivityID=" + hifActivityID.Value + " , @Remarks=" + DB.SQuote(txtRemarks.Text) + ", @UserID=" + cp.UserID + ", @WorkcenterID=" + hifWorkCenter.Value + " ,@IORefNo=" + DB.SQuote(IORefNumber) + ", @IN_SONumber=" + DB.SQuote(SONumber) + ", @IN_OBDNumber=" + DB.SQuote(OBDNumber) + " ,@IN_CusPONumber=" + DB.SQuote(CUSPONumber) + " ,@IN_Tenant=" + cp.TenantID + " ,@IN_NewOutboundID=@NewUpdateOutboundID OUTPUT; select @NewUpdateOutboundID AS N");

                    //int UpdateStatus = 0;
                    int UpdateStatus =  DB.GetSqlN(SQLUpdate.ToString());



                    switch (UpdateStatus)
                    {
                        case -1:
                            resetError("This 'NC Order' has material deficiency", true);
                            return;
                        case 0:
                            resetError("Error while updating", true);
                            return;
                        case -3:
                            resetError("The selected activities are successfully scraped", false);
                            return;
                        

                    }

                    if (UpdateStatus > 0)
                    {
                        resetError("NC order is released with OBDNumber:" + OBDNumber, false);

                    }

                }
                else
                {
                    resetError("Please check for mandatory fields", true);
                    return;
                }




                tvBulkNCList.Nodes.Clear();
                PrHID = hifJobRefNoNumber.Value;

                ViewState["ProhID"] = hifJobRefNoNumber.Value;
                hifActivityID.Value = "";

                if (PrHID != "")
                {
                    PopulateRootNode();
                }
                tvBulkNCList.ExpandAll();

            }
            catch (Exception ex) {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while updating", true);
                return;
            }


        }

        protected void resetError(string error, bool isError)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", "showStickyToast(" + (isError.ToString().ToLower() == "true" ? "false" : "true") + ",\"" + error + "\");", true);
        }

        public String GetIORefNo() {


            try
            {
                String NewIONumber = DB.GetSqlS("EXEC sp_SYS_GetSystemConfigValue @SysConfigKey=N'internalorder.aspx.cs.IO_Prefix',@TenantID=" + cp.TenantID);

                int length = Convert.ToInt32(DB.GetSqlS("EXEC sp_SYS_GetSystemConfigValue @SysConfigKey=N'internalorder.aspx.cs.IO_Length',@TenantID=" + cp.TenantID));


                //string HeaderID = ViewState["HeaderID"].ToString();
                String OldIONumber = DB.GetSqlS("select top 1 IORefNo as S from MFG_InternalOrderHeader  order by IORefNo desc");

                NewIONumber += "" + (Convert.ToInt16(DateTime.Now.Year) % 100);           //add year code to ponumber

                int power = (Int32)Math.Pow((double)10, (double)(length - 1));          //getting minvalue of prifix length

                String newvalue = "";
                if (OldIONumber != "" && NewIONumber.Equals(OldIONumber.Substring(0, NewIONumber.Length)))        //if ponumber is existed and same year ponumber  enter
                {
                    String temp = OldIONumber.Substring(NewIONumber.Length, length);                            //getting number of last prifix
                    Int32 number = Convert.ToInt32(temp);
                    number++;


                    while (power > 1)                                                                           //add '0' to number at left side for get 
                    {
                        if (number / power > 0)
                        {
                            break;
                        }
                        newvalue += "0";
                        power /= 10;
                    }
                    newvalue += number;
                }
                else
                {                                                                                           //other wise generate first number 
                    for (int i = 0; i < length - 1; i++)
                        newvalue += "0";
                    newvalue += "1";
                }

                return  NewIONumber += newvalue;
               
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
                resetError("Error while generating IONumber", true);

                return "";
            }

        }

        private String GetOBDNumber()
        {
            String OBDNumber = "";
            try
            {

                int length = Convert.ToInt32(DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='OutboundDetails.aspx' ,@TenantID=" + cp.TenantID));



                String NewOBDNumber = DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='dummyoutboundforproduction_prefix' ,@TenantID=" + cp.TenantID) + (Convert.ToInt16(DateTime.Now.Year) % 100);           //add year code to ponumber

                String OldOBDNumber = DB.GetSqlS("select top 1 OBDNumber AS S from OBD_Outbound where OBDNumber like '" + NewOBDNumber + "%' order by OBDNumber desc");

                int power = (Int32)Math.Pow((double)10, (double)(length - 1));          //getting minvalue of prifix length

                String newvalue = "";
                if (OldOBDNumber != "" && NewOBDNumber.Equals(OldOBDNumber.Substring(0, NewOBDNumber.Length)))        //if ponumber is existed and same year ponumber  enter
                {
                    String temp = OldOBDNumber.Substring(NewOBDNumber.Length, length);                            //getting number of last prifix
                    Int32 number = Convert.ToInt32(temp);
                    number++;


                    while (power > 1)                                                                           //add '0' to number at left side for get 
                    {
                        if (number / power > 0)
                        {
                            break;
                        }
                        newvalue += "0";
                        power /= 10;
                    }
                    newvalue += number;
                }
                else
                {                                                                                           //other wise generate first number 
                    for (int i = 0; i < length - 1; i++)
                        newvalue += "0";
                    newvalue += "1";
                }

                NewOBDNumber += newvalue;
                OBDNumber = NewOBDNumber;
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
            return OBDNumber;
        }

        private String GetSONumber()
        {
            String SONumber = "";
            try
            {
                String NewSONumber = DB.GetSqlS("EXEC [sp_SYS_GetSystemConfigValue]   @SysConfigKey='dummysoheaderforinternalorder_prefix' ,@TenantID=" + cp.TenantID);
                NewSONumber += "" + (Convert.ToInt16(DateTime.Now.Year) % 100);

                int length = Convert.ToInt32(DB.GetSqlS("select SysConfigValue as S from SYS_SystemConfiguration as N where TenantID=" + cp.TenantID + " and SysConfigKeyID= (select SysConfigKeyID from SYS_SysConfigKey where SysConfigKey=N'salesorder.aspx.cs.SO_Length') "));

                //String OldSONumber = DB.GetSqlS("select TOP 1 SONumber as S from ORD_SOHeader where SOTypeID =6 and TenantID=" + cp.TenantID + "  ORDER BY SONumber desc ");
                String OldSONumber = DB.GetSqlS("select TOP 1 SONumber as S from ORD_SOHeader where SONumber like '" + NewSONumber + "%' ORDER BY SONumber desc ");

                //add year code to ponumber

                int power = (Int32)Math.Pow((double)10, (double)(length - 1));          //getting minvalue of prifix length

                String newvalue = "";
                if (OldSONumber != "" && NewSONumber.Equals(OldSONumber.Substring(0, NewSONumber.Length)))        //if ponumber is existed and same year ponumber  enter
                {
                    String temp = OldSONumber.Substring(NewSONumber.Length, length);                            //getting number of last prifix
                    Int32 number = Convert.ToInt32(temp);
                    number++;


                    while (power > 1)                                                                           //add '0' to number at left side for get 
                    {
                        if (number / power > 0)
                        {
                            break;
                        }
                        newvalue += "0";
                        power /= 10;
                    }
                    newvalue += number;
                }
                else
                {                                                                                           //other wise generate first number 
                    for (int i = 0; i < length - 1; i++)
                        newvalue += "0";
                    newvalue += "1";
                }

                NewSONumber += newvalue;
                SONumber = NewSONumber;
            }
            catch (Exception ex)
            {
                CommonLogic.createErrorNode(cp.UserID + " / " + cp.FirstName, this.Page.ToString(), ex.Source, ex.Message, ex.StackTrace);

            }
            return SONumber;
        }

        private String GetCUSPONumber()
        {
            Random generateNo = new Random();
            return "DCUSPO" + generateNo.Next(1000, 10000);
        }

        public string Get8DigitNumber()
        {
            var bytes = new byte[4];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            uint random = BitConverter.ToUInt32(bytes, 0) % 100000000;
            return String.Format("{0:D8}", random);
        }


    }
}