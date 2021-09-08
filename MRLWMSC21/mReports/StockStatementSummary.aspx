<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="StockStatementSummary.aspx.cs" Inherits="MRLWMSC21.mReports.Stock_Statement_Summary" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=B03F5F7F11D50A3A" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:ScriptManager ID="mySManager" runat="server" EnablePartialRendering="true" />

    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%= this.txtFromDate.ClientID %>").datepicker({ 
                dateFormat: "dd/mm/yy",
                maxDate: new Date(),
                onSelect: function (selected) {
                    $("#<%= this.txtToDate.ClientID %>").datepicker("option", "minDate", selected, { dateformate: "dd/mm/yy" }) 
                }
            });
            $("#<%= this.txtToDate.ClientID %>").datepicker({ 
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
            <td class="ModuleHeader" align="center" width="155px">
                <nobr>Stock Statement Summary</nobr>
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


                        <td>
                            <label style="font-size: 15px; vertical-align: top">Material Type: </label>
                            <br />
                            <asp:DropDownList ID="drpMType" runat="server" Width="175" Font-Size="16px">
                                <asp:ListItem Text="ALL" Value="0" Selected="False"></asp:ListItem>
                                <asp:ListItem Text="RM - Raw Material" Value="7" Selected="False"></asp:ListItem>
                                <asp:ListItem Text="SA - Semi Finished Goods" Value="8" Selected="False"></asp:ListItem>
                                <asp:ListItem Text="FG - Finished Goods" Value="9" Selected="False"></asp:ListItem>
                                <asp:ListItem Text="CS - Consumables" Value="11" Selected="False"></asp:ListItem>
                                <asp:ListItem Text="TL - Tools" Value="1011" Selected="False"></asp:ListItem>
                            </asp:DropDownList>
                        </td>


                        <td align="left">
                            <label style="font-size: 15px">Opening Date : </label>
                            <br />
                            <asp:TextBox runat="server" ID="txtFromDate" Width="120" Font-Size="14px"></asp:TextBox>
                        </td>

                        <td align="left">
                            <label style="font-size: 15px">Closing Date : </label>
                            <br />
                            <asp:TextBox runat="server" ID="txtToDate" Width="120" Font-Size="14px"></asp:TextBox>
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
