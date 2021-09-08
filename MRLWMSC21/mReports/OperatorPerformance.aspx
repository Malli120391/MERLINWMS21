<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OperatorPerformance.aspx.cs" Inherits="MRLWMSC21.mReports.OperatorPerformance" %>

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

    <script>
        $(document).ready(function () {
            var textfieldname = $('#<%=txtOperator.ClientID%>');
            DropdownFunction(textfieldname);
            $('#<%=txtOperator.ClientID%>').autocomplete({
                source: function (request, response) {

                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadUsersDataForOperatorPerformanceReport") %>',
                        data: "{ 'prefix': '" + request.term + "'}",
                        dataType: "json",
                        type: "POST",
                        async: true,
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {

                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split(',')[1],
                                    val: item.split(',')[0]
                                }
                            }))

                        }

                    });
                },
                select: function (e, i) {

                    $("#<%=this.hifOperator.ClientID%>").val(i.item.val);
                },
                minLength: 0
            });
        });
    </script>

    <script>
        $(document).ready(function () {
            var textfieldname = $('#<%=txtRole.ClientID%>');
            DropdownFunction(textfieldname);
            $('#<%=txtRole.ClientID%>').autocomplete({
                source: function (request, response) {

                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadUserRolesForOperatorPerformanceReport") %>',
                        data: "{ 'prefix': '" + request.term + "'}",
                        dataType: "json",
                        type: "POST",
                        async: true,
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {

                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split(',')[1],
                                    val: item.split(',')[0]
                                }
                            }))

                        }

                    });
                },
                select: function (e, i) {

                    $("#<%=this.hifRole.ClientID%>").val(i.item.val);
                },
                minLength: 0
            });
        });
    </script>
<div class="dashed"></div>
<div class="pagewidth">

    <table border="0" cellpadding="2" cellspacing="0" align="center" width="100%">
        <tr>
            <td class="ModuleHeader" style="width: 30px">
                <nobr>Operator Performance Report</nobr>
            </td>
            <td class="parmArea" colspan="2" align="right">

                <table border="0" cellpadding="3" cellspacing="1">

                    <tr>
                        <td align="left">
                            <label style="font-size: 15px">Operator : </label>
                            <br />
                            <asp:TextBox runat="server" ID="txtOperator" Width="135" Font-Size="14px" SkinID="txt_Req"></asp:TextBox>
                            <asp:HiddenField runat="server" ID="hifOperator" />
                        </td>

                        <td align="left">
                            <label style="font-size: 15px">Role : </label>
                            <br />
                            <asp:TextBox runat="server" ID="txtRole" Width="135" Font-Size="14px" SkinID="txt_Req"></asp:TextBox>
                            <asp:HiddenField runat="server" ID="hifRole" />
                        </td>
                        <td>
                            <br />
                            <asp:LinkButton ID="lnkGetReport" CssClass="ui-btn ui-button-large" runat="server" Text="View Report" OnClick="lnkGetReport_Click" /></td>
                    </tr>

                    <tr>
                        <!--  <td   align="left" class="FormLabels" colspan="4" >            
                        Select Printer :<br />            
                        <asp:DropDownList ID="ddlPrinters" SkinID="ddlRequired" Width="150" runat="server"></asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton  ID="lnkPrint" CssClass="ButMSGo"  runat="server" Text="Print" OnClick="lnkPrint_Click"  />
                        </td> -->
                    </tr>

                </table>
            </td>
        </tr>
        <tr>
            <td id="tdRePortParent" align="center" colspan="2" height="400px">

                <rsweb:ReportViewer ID="rvOperatorPerformanceReport" runat="server" Visible="false" Width="99.9%" Height="600" CssClass="rptClass" ZoomMode="Percent"
                    ShowParameterPrompts="false" ProcessingMode="Remote" BorderColor="Teal" ShowPrintButton="false"
                    InternalBorderColor="Teal" InternalBorderStyle="Solid" InternalBorderWidth="1">
                </rsweb:ReportViewer>

            </td>

        </tr>

    </table>
</div>
</asp:Content>
