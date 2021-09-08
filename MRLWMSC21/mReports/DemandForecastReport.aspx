<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="DemandForecastReport.aspx.cs" Inherits="MRLWMSC21.mReports.DemandForecastReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=B03F5F7F11D50A3A" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:ScriptManager ID="mySManager" runat="server" EnablePartialRendering="true" />    
   

    <script>
        $(document).ready(function () {
            var tdReportParent = document.getElementById('tdRePortParent');
            var tdChildTables = tdReportParent.getElementsByTagName('table');
            for (index = 0; index < tdChildTables.length; index++) {
                tdChildTables[index].setAttribute("align", "center");
            }
        });
    </script>
    <div class="dashed"></div>
<div class="pagewidth">

    <table border="0" cellpadding="2" cellspacing="0" align="left" width="100%">
        <tr>
            <td class="ModuleHeader" align="center" width="155px">
                <nobr>Demand Forecast Report</nobr>
            </td>
            <td class="parmArea" colspan="2" align="right">

                <table border="0" cellpadding="3" cellspacing="1">

                    <tr>
                        <%--<td>
                            <label style="font-size: 15px">Transaction Year : </label>
                            <asp:TextBox runat="server" ID="txtYear" Width="120" SkinID="txt_Req" Font-Size="14px"></asp:TextBox>
                            <asp:HiddenField runat="server" ID="hifMCode" />
                        </td>--%>
                        <td>
                            <asp:Literal ID="ltStatus" runat="server"> </asp:Literal>
                            <asp:LinkButton ID="lnkGetReport" CssClass="ui-btn ui-button-large" runat="server" Text="View Report" OnClick="lnkGetReport_Click" /> </td>
                    </tr>
                    <tr>

                  <%--      <td align="left" class="FormLabels" colspan="4">
                            <label style="font-size: 15px">Select Printer : </label>
                            <br />

                            <asp:DropDownList ID="ddlPrinters" SkinID="ddlRequired" Width="150" runat="server"></asp:DropDownList>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkPrint" CssClass="ButMSGo" runat="server" Text="Print" OnClick="lnkPrint_Click" />
                        </td>--%>

                    </tr>
                </table>

            </td>

        </tr>
        <tr>

            <td id="tdRePortParent" align="center" colspan="2" height="400px"> 

                <rsweb:ReportViewer ID="rvDemandForecastReport" runat="server" Visible="false" Width="99.9%" Height="800" CssClass="rptClass" ZoomMode="Percent" ShowZoomControl="true"
                    ShowParameterPrompts="false" ProcessingMode="Remote" BorderColor="Teal" ShowPrintButton="true"
                    InternalBorderColor="Teal" InternalBorderStyle="Double" InternalBorderWidth="1">
                </rsweb:ReportViewer>

            </td>

        </tr>
    </table>
</div>
</asp:Content>


