<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="StockMovementReport.aspx.cs" Inherits="MRLWMSC21.mReports.StockMovementReport" %>

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


    <%--<script>
        $(document).ready(function () {
            var texfieldname = $('#<%=txtMCode.ClientID%>');
            DropdownFunction(texfieldname);
            $('#<%=txtMCode.ClientID%>').autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetMCode") %>',
                        data: "{ 'prefix': '" + request.term + "','TenantID':'" +<%= this.cp.TenantID %> +"'}",
                        dataType: "json",
                        type: "POST",
                        async: true,
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {

                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split(',')[0],
                                    val: item.split(',')[1]
                                }
                            }))

                        }

                    });
                },
                select: function (e, i) {

                    $("#<%=this.hifMCode.ClientID%>").val(i.item.val);

                },
                minLength: 0
            });
        });
    </script>--%>


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
                <nobr>Stock Movement Report</nobr>
            </td>

            <td class="parmArea" colspan="2" align="right">

                <table border="0" cellpadding="3" cellspacing="1">

                    <tr>                        

                        <%--  <td align="left">
                            <label style="font-size: 15px">Part No. : </label>
                            <br />
                            <asp:TextBox runat="server" ID="txtMCode" Width="130" Font-Size="14px" SkinID="txt_Req"></asp:TextBox>
                            <asp:HiddenField runat="server" ID="hifMCode" />
                        </td>--%>

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

            <td id="tdRePortParent" align="center" colspan="2" height="350px">

                <rsweb:ReportViewer ID="rvStockMovementReport" runat="server" Visible="false" Width="99.9%" Height="800" CssClass="rptClass" ZoomMode="Percent" ShowZoomControl="true"
                    ShowParameterPrompts="false" ProcessingMode="Remote" BorderColor="Teal" ShowPrintButton="true"
                    InternalBorderColor="Teal" InternalBorderStyle="Double" InternalBorderWidth="1">
                </rsweb:ReportViewer>

            </td>

        </tr>
    </table>
    </div>
</asp:Content>

