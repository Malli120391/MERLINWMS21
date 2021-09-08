using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MRLWMSC21Common;
using System.Data;
using GenCode128;
using System.IO;
using System.Drawing;

using System.Web.UI.WebControls.WebParts;

namespace MRLWMSC21.mManufacturingProcess
{
    public partial class JobCard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string vPooductionID = "0";
            if (CommonLogic.QueryString("proid") != null)
            {
                vPooductionID = CommonLogic.QueryString("proid");
            }
            try
            {
                IDataReader reader = DB.GetRS("EXEC dbo.sp_MFG_GetWorkCenterWiseJobCard @ProductionOrderHeaderID=" + vPooductionID);
                while (reader.Read())
                {
                    string prorefNumber = DB.RSField(reader, "PRORefNo");
                    string mcode = DB.RSField(reader, "MCode");
                    string workcenterFinishedRawMateraildata = DB.RSField(reader, "WorkCenterFinichedMaterial");
                    string[] workcenterFinishedRawMateraildatasplit = workcenterFinishedRawMateraildata.Split('|');
                    string workcentermcode = workcenterFinishedRawMateraildatasplit[0];
                    string workcenterQty = workcenterFinishedRawMateraildatasplit[1];
                    string WorkCenterRefNo = DB.RSField(reader, "WorkCenterRefNo");
                    string jobordnumber = prorefNumber + "-" + WorkCenterRefNo;
                    string operationname = DB.RSField(reader, "Name");
                    joborder.InnerHtml += "<table border='1' width='100%' cellpadding='1' cellspacing='1' style=\"border:1px solid black;border-collapse:collapse;\"><tr><td colspan=\"2\"><b><center>" + operationname + "</center></b></td></tr><tr><td><b>Job OrderNumber:</b><br/>" + "<img border=0 src=\"..\\mInbound\\Code39Handler.ashx?code=" + jobordnumber + "\" /><br/>";
                    joborder.InnerHtml += "<div class=\"divstyle\">" + jobordnumber + "</div></td>";
                    string PRORefNo = DB.RSField(reader, "PRORefNo");
                    string StartDate = DB.RSFieldDateTime(reader, "StartDate").ToString("dd MMM yyyyy");
                    joborder.InnerHtml += "<td><b>Production Order Num:</b><br/><img border=0 src=\"..\\mInbound\\Code39Handler.ashx?code=" + PRORefNo + "\" /><br/><div class=\"divstyle\">" + jobordnumber + "</div></td></tr>";
                    joborder.InnerHtml += "<tr><td><b>Start Date:</b>  " + StartDate + "</td>";
                    string duedate = DB.RSFieldDateTime(reader, "DueDate").ToString("dd MMM yyyy");
                    joborder.InnerHtml += "<td><b>Due Date:</b>  " + duedate + "</td></tr>";
                    joborder.InnerHtml += "<br/><br/>";
                    string bomrefNum = DB.RSField(reader, "BOMRefNumber");
                    joborder.InnerHtml += "<tr><td><b>BoM Ref. No#</b>" + bomrefNum + "</td>";
                    string routingref = DB.RSField(reader, "RoutingRefNo");
                    joborder.InnerHtml += "<td><b>Routing Ref. No:</b>" + routingref + "</td></tr></table><br/><br/>";
                    string Qty = DB.RSFieldDecimal(reader, "ProductionQuantity").ToString();
                    joborder.InnerHtml += "<b>Finished Product</b>";
                    joborder.InnerHtml += "<table border='1' width='100%' cellpadding='1' cellspacing='1' style=\"border:1px solid black;border-collapse:collapse;\"><th>Material Code</th><th>Quantity</th><tr><td></b></br><img  src=\"..\\mInbound\\Code39Handler.ashx?code=" + mcode + "\" />";
                    joborder.InnerHtml += "<div class=\"divstyle\">" + workcentermcode + "</div></td>";
                    joborder.InnerHtml += "<td>" + workcenterQty + "</td></tr></table><br/>";
                    joborder.InnerHtml += "<b>Raw Material:</b>";
                    joborder.InnerHtml += "<table table border='1' width='100%' cellpadding='1' cellspacing='1' style=\"border:1px solid black;border-collapse:collapse;\"><tr><th>Line No:</th><th>MCode:</th><th>UOM:</th><th>Qty:</th></tr>";
                    string rawMaterial = DB.RSField(reader, "WorkCenterRawMaterial");
                    string[] splitrawmat = rawMaterial.Split(',');
                    for (int i = 0; i < splitrawmat.Length; i++)
                    {
                        joborder.InnerHtml += "<tr>";
                        string[] splitmatdetails = splitrawmat[i].Split('|');
                        joborder.InnerHtml += "<td >" + splitmatdetails[5] + "</td>";
                        joborder.InnerHtml += "<td ><br/><img border=0 src=\"..\\mInbound\\Code39Handler.ashx?code=" + splitmatdetails[2] + "\" /></br><center>" + splitmatdetails[2] + "</center></td>";
                        joborder.InnerHtml += "<td>" + splitmatdetails[4] + "</td>";
                        joborder.InnerHtml += "<td >" + splitmatdetails[3] + "</td>";

                        joborder.InnerHtml += "</tr>";
                    }
                    joborder.InnerHtml += "</table>";
                    //string[] splitmatdetails=

                    joborder.InnerHtml += "<br/><br/><hr><br/><br/>";

                }
                
                //ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript: printReport(); ", true);

            }
            catch
            {
                lblError.Text = "No Data Found";
            }
          
        }
    
    }
}