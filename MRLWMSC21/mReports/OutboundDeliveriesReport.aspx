<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OutboundDeliveriesReport.aspx.cs" Inherits="MRLWMSC21.mReports.OutboundDeliveriesReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=B03F5F7F11D50A3A" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:ScriptManager ID="mySManager" runat="server" EnablePartialRendering="true" />

    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%= this.ObdFromDate.ClientID %>").datepicker({
                dateFormat: "dd/mm/yy",
                maxDate: new Date(),
                onSelect: function (selected) {
                    $("#<%= this.ObdToDate.ClientID %>").datepicker("option", "minDate", selected, { dateformate: "dd/mm/yy" })
                }
            });
            $("#<%= this.ObdToDate.ClientID %>").datepicker({
                dateFormat: "dd/mm/yy",
                maxDate: new Date(),
            });
        })
    </script>

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
                <nobr>Outbound Deliveries Report</nobr>
            </td>

            <td class="parmArea" colspan="2" align="right">

                <table border="0" cellpadding="3" cellspacing="1">

                    <tr>

                        <td align="left">
                            <label style="font-size: 15px">Document Type : </label>
                            <br />
                            <asp:DropDownList ID="ddlDocumentType" CssClass="txt_slim" runat="server" Width="160" Height="22" Font-Size="12.5" />
                        </td>

                        <td align="left">
                            <label style="font-size: 15px">Delivery Status : </label>
                            <br />
                            <asp:DropDownList ID="ddlDeliveryStatus" CssClass="txt_slim" runat="server" Width="160" Height="22" Font-Size="12.5" />
                        </td>

                        <td align="left">
                            <label style="font-size: 15px">From Date : </label>
                            <br />
                            <asp:TextBox runat="server" ID="ObdFromDate" Width="120" Font-Size="14px"></asp:TextBox>
                        </td>

                        <td align="left">
                            <label style="font-size: 15px">To Date : </label>
                            <br />
                            <asp:TextBox runat="server" ID="ObdToDate" Width="120" Font-Size="14px"></asp:TextBox>
                        </td>

                        <td>
                            <br />
                            <asp:LinkButton ID="lnkGetReport" CssClass="ui-btn ui-button-large" runat="server" Text="View Report" OnClick="lnkGetReport_Click" />
                        </td>
                    </tr>
                    <tr>
                    </tr>
                </table>

            </td>

        </tr>

        <tr>

            <td id="tdRePortParent" align="center" colspan="2" height="400px">

                <rsweb:ReportViewer ID="rvOutboundDeliveriesReport" runat="server" Visible="false" Width="99.9%" Height="800" CssClass="rptClass" ZoomMode="Percent" ShowZoomControl="true"
                    ShowParameterPrompts="false" ProcessingMode="Remote" BorderColor="Teal" ShowPrintButton="true"
                    InternalBorderColor="Teal" InternalBorderStyle="Double" InternalBorderWidth="1">
                </rsweb:ReportViewer>

            </td>

        </tr>
    </table>
    </div>
</asp:Content>
