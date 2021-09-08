using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MRLWMSC21Common;
using System.Web.UI.HtmlControls;

namespace MRLWMSC21.mManufacturingProcess
{
    public partial class JobRoutePlan : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = "Inbound";

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DesignLogic.SetInnerPageSubHeading(this.Page, "JOB/ROUTE PLAN");
            }
            string poid=CommonLogic.QueryString("poid");
            if (poid !="")
            {
                string sqlforJrpHeader = "exec sp_MFG_GetProductinOrderJRPHeader @ProductionOrderHeaderID="+ poid;
                IDataReader objJrpHeaderData = DB.GetRS(sqlforJrpHeader);
                while (objJrpHeaderData.Read())
                {
                    
                    ltOEMPartNo.Text = objJrpHeaderData.GetString(0);
                    ltJrpType.Text = objJrpHeaderData.GetString(8);
                    ltRtPartNumber.Text = objJrpHeaderData.GetString(1);
                    ltReleaseDate.Text = objJrpHeaderData.GetString(9);
                    ltPartDescription.Text = objJrpHeaderData.GetString(2);
                  //  ltKitCode.Text = objJrpHeaderData.GetString(10);
                    ltProjectCode.Text = objJrpHeaderData.GetString(4);
                    ltMfgdate.Text = objJrpHeaderData.GetString(11);
                    ltWorkOrder.Text = objJrpHeaderData.GetString(3);
                    ltJobType.Text = objJrpHeaderData.GetString(5);
                    string vRefDocumnets=objJrpHeaderData.GetString(7);
                    string[] vDocumentsSplit = vRefDocumnets.Split(',');
                    ltReferenceDocumnets.Text = "";

                    ltReferenceDocumnets.Text += "<table border='1' width='100%' style=\"border-collapse:collapse;\">";
                    if (vDocumentsSplit[0] != "")
                    {
                        for (int index = 0; index < vDocumentsSplit.Length; index++)
                        {
                            ltReferenceDocumnets.Text += "<tr><td>";

                            string[] vDocumnetValuesSplit = vDocumentsSplit[index].Split('@');
                            ltReferenceDocumnets.Text += vDocumnetValuesSplit[0] + "</td>";
                            ltReferenceDocumnets.Text += "<td>" + vDocumnetValuesSplit[1] + "</td></tr>";

                        }
                        ltReferenceDocumnets.Text += "</table>";
                    }
                 


                    //ltReferenceDocumnets
                
                }

                string sqlforJrpDetails = "exec [dbo].[sp_MFG_GetProductinOrderJRPDetails]@ProductionOrderHeaderID=" + poid;
                IDataReader objJrpDetaild = DB.GetRS(sqlforJrpDetails);
                HtmlTableRow tr;
                HtmlTableCell td;
                while (objJrpDetaild.Read())
                {
                    tr = new HtmlTableRow();
                    td = new HtmlTableCell();
                    td.InnerHtml = objJrpDetaild.GetString(0);
                    tr.Controls.Add(td);
                    td = new HtmlTableCell();
                    td.InnerHtml = objJrpDetaild.GetString(1);
                    tr.Controls.Add(td);
                    td = new HtmlTableCell();
                    td.InnerHtml = objJrpDetaild.GetString(2);
                    tr.Controls.Add(td);
                    td = new HtmlTableCell();
                    td.InnerHtml = objJrpDetaild.GetString(3);
                    tr.Controls.Add(td);
                    td = new HtmlTableCell();
                    td.InnerHtml = objJrpDetaild.GetString(4);
                    tr.Controls.Add(td);
                    td = new HtmlTableCell();
                    td.InnerHtml = objJrpDetaild.GetString(5);
                    td.Width = "130";
                    tr.Controls.Add(td);
                    td = new HtmlTableCell();
                    td.InnerHtml = objJrpDetaild.GetString(6);
                    td.Width = "130";
                    tr.Controls.Add(td);
                    td = new HtmlTableCell();
                    td.InnerHtml = objJrpDetaild.GetString(7);
                    td.Width = "130";
                    tr.Controls.Add(td);
                    td = new HtmlTableCell();
                    td.InnerHtml = objJrpDetaild.GetString(8);
                    tr.Controls.Add(td);
                    tblJRP.Controls.Add(tr);

                  
                }


            }
            

        }
    }
}