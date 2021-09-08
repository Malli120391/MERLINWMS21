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
    public partial class InspectionCheckList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string poid = CommonLogic.QueryString("poid");
            if (poid != "")
            {
                string sqlForReferDocumnets = "EXEC [dbo].[sp_MFG_GetProductinOrderJRPHeader] @ProductionOrderHeaderID="+poid;
                IDataReader objRefferedDodumentslist = DB.GetRS(sqlForReferDocumnets);
                while (objRefferedDodumentslist.Read())
                {
                    string vRefDocumnets = objRefferedDodumentslist.GetString(7);
                    string[] vDocumentsSplit = vRefDocumnets.Split(',');

                    ltDocuments.Text += "<table border='1' width='100%' style=\"border-collapse:collapse;\">";
                    if (vDocumentsSplit[0] != "")
                    {
                        for (int index = 0; index < vDocumentsSplit.Length; index++)
                        {
                            ltDocuments.Text += "<tr><td>";

                            string[] vDocumnetValuesSplit = vDocumentsSplit[index].Split('@');
                            ltDocuments.Text += vDocumnetValuesSplit[0] + "</td>";
                            ltDocuments.Text += "<td>" + vDocumnetValuesSplit[1] + "</td></tr>";

                        }
                        ltDocuments.Text += "</table>";
                    }
                }

                string sqlforJrpDetails = " EXEC [dbo].[sp_MFG_GetInspectionCheckList]"+poid;
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
                    td.Width = "1000";
                    td.Style.Value = "border-top-style:none;border-bottom-style:none;";

                    string data = objJrpDetaild.GetString(1);

                    string[] checkPointData = data.Split('!');
                    string inner = "";
                    inner += "<table  width='100%' border=\"1\" style=\"border-collapse:collapse;\">";
                    for (int i = 0; i < checkPointData.Length; i++)
                    {
                        //For Removing bottom border for last row
                        if (i == checkPointData.Length - 1)
                        {
                            string[] innersplit = checkPointData[i].Split('^');
                            inner += "<tr>";
                            for (int index = 0; index < innersplit.Length; index++)
                            {

                                if (index == 0)
                                {
                                    inner += "<td width=\"400\" style=\"border-left-style:hidden;border-bottom-style:hidden;\">";
                                    inner += innersplit[index];
                                    inner += "</td>";
                                }
                                else if (index == 1 || index == 2)
                                {

                                    inner += "<td width=\"100\" style=\"border-bottom-style:hidden;\">";
                                    inner += innersplit[index];
                                    inner += "</td>";
                                }
                                else if (index == 3)
                                {
                                    //For Splitting Pass/Failed Data with UserNames
                                    string[] userData = innersplit[index].Split('-');
                                    inner += "<td width=\"300\" style=\"border-bottom-style:hidden;\">";
                                    inner += "<table width=\"300\" border=\"0\" height=\"40\" style=\"border-collapse:collapse;border-top-style:none;border-bottom-style:none;\">";
                                    inner += "<tr>";
                                    inner += "<td width=\"100\" style=\"border-top-style:none;border-bottom-style:none;border-left-style:hidden;\">";
                                    inner += userData[0];
                                    inner += "</td>";

                                    inner += "<td width=\"100\" style=\"border-top-style:none;border-bottom-style:none;\">";
                                    inner += userData[1];
                                    inner += "</td>";

                                    inner += "<td width=\"100\" style=\"border-top-style:none;border-bottom-style:none;border-right-style:hidden\">";
                                    inner += userData[2];
                                    inner += "</td>";
                                    inner += "</tr>";
                                    inner += "</table>";
                                    inner += "</td>";

                                }
                                else
                                {
                                    inner += "<td width=\"100\" style=\"border-bottom-style:hidden;\">";
                                    inner += innersplit[index];
                                    inner += "</td>";
                                }

                            }
                            inner += "</tr>";
                        }
                        else
                        {
                            string[] innersplit = checkPointData[i].Split('^');
                            inner += "<tr>";
                            for (int index = 0; index < innersplit.Length; index++)
                            {

                                if (index == 0)
                                {
                                    inner += "<td width=\"400\" style=;border-left-style:hidden;\">";
                                    inner += innersplit[index];
                                    inner += "</td>";
                                }
                                else if (index == 1 || index == 2)
                                {

                                    inner += "<td width=\"100\">";
                                    inner += innersplit[index];
                                    inner += "</td>";
                                }
                                else if (index == 3)
                                {
                                    //For Splitting Pass/Failed Data with UserNames
                                    string[] userData = innersplit[index].Split('-');
                                    inner += "<td width=\"300\">";
                                    inner += "<table width=\"300\" border=\"0\" height=\"40\" style=\"border-collapse:collapse;border-top-style:none;border-bottom-style:none;\">";
                                    inner += "<tr>";
                                    inner += "<td width=\"100\" style=\"border-top-style:none;border-bottom-style:none;border-left-style:hidden;\">";
                                    inner += userData[0];
                                    inner += "</td>";

                                    inner += "<td width=\"100\" style=\"border-top-style:none;border-bottom-style:none;\">";
                                    inner += userData[1];
                                    inner += "</td>";

                                    inner += "<td width=\"100\" style=\"border-top-style:none;border-bottom-style:none;border-right-style:hidden\">";
                                    inner += userData[2];
                                    inner += "</td>";
                                    inner += "</tr>";
                                    inner += "</table>";
                                    inner += "</td>";

                                }
                                else
                                {
                                    inner += "<td width=\"100\">";
                                    inner += innersplit[index];
                                    inner += "</td>";
                                }

                            }
                            inner += "</tr>";

                        }


                    }
                    inner += "</table>";
                    td.InnerHtml = inner;
                    tr.Controls.Add(td);
                    tblCheckList.Controls.Add(tr);


                }
                objJrpDetaild.Close();
            }
           

        }
    }
}