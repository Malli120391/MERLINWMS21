<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MaterialAgeingReport.aspx.cs" Inherits="MRLWMSC21.mReports.MaterialAgeingReport" %>

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
    </script>
<div class="dashed"></div>
<div class="pagewidth">

    <table border="0" cellpadding="2" cellspacing="0" align="left" width="100%">
        <tr>
            <td class="ModuleHeader" style="width: 30px">
                <nobr>Material Ageing Report</nobr>
            </td>
            <td class="parmArea" colspan="2" align="right">

                <table border="0" cellpadding="3" cellspacing="1">

                    <tr>

                         <td align="left">
                            <label style="font-size: 15px">Part Number : </label>
                            <br />
                            <asp:TextBox runat="server" ID="txtMCode" Width="140" SkinID="txt_Req" Font-Size="14px"></asp:TextBox>
                            <asp:HiddenField runat="server" ID="hifMCode" />

                        </td>

                        <td width="30%">
                            <asp:Literal ID="ltSite" runat="server" Text="Zone:" /><br />
                            <asp:DropDownList ID="ddlSite" runat="server" Width="90" />
                        </td >


                        <td>
                            <label style="font-size: 15px; vertical-align: top">Age in Days: </label>
                            <br />
                            <asp:DropDownList ID="drpAgeing" runat="server" Width="150" Font-Size="16px">
                                <asp:ListItem Text="All" Value="0" Selected="False"></asp:ListItem>
                                <asp:ListItem Text="0-30" Value="1" Selected="False"></asp:ListItem>
                                <asp:ListItem Text="31-90" Value="2" Selected="False"></asp:ListItem>
                                <asp:ListItem Text="91-180" Value="3" Selected="False"></asp:ListItem>
                                <asp:ListItem Text="181-365" Value="4" Selected="False"></asp:ListItem>
                                <asp:ListItem Text="Above 365" Value="5" Selected="False"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <br />
                            <asp:LinkButton ID="lnkGetReport" CssClass="ui-btn ui-button-large" runat="server" Text="View Report" OnClick="lnkGetReport_Click" /></td>
                    </tr>
                    <tr>

                        <!-- <td   align="left" class="FormLabels" colspan="4" >
            
                                <label style="font-size:15px"> Select Printer : </label> <br />
            
                                <asp:DropDownList ID="ddlPrinters" SkinID="ddlRequired" Width="150" runat="server"></asp:DropDownList>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton  ID="lnkPrint" CssClass="ButMSGo"  runat="server" Text="Print" OnClick="lnkPrint_Click"/>
                                </td> -->

                    </tr>
                </table>

            </td>

        </tr>
        <tr>
            <td id="tdRePortParent" align="center" colspan="2" height="400px">

                <rsweb:ReportViewer ID="rvMaterialAgeing" runat="server" Visible="false" Width="99.9%" Height="800" CssClass="rptClass" ZoomMode="Percent"
                    ShowParameterPrompts="false" ProcessingMode="Remote" BorderColor="Teal" ShowPrintButton="false"
                    InternalBorderColor="Teal" InternalBorderStyle="Solid" InternalBorderWidth="1">
                </rsweb:ReportViewer>

            </td>

        </tr>

    </table>
</div>
</asp:Content>
