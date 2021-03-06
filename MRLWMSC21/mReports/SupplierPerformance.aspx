<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SupplierPerformance.aspx.cs" Inherits="MRLWMSC21.mReports.SupplierPerformance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=B03F5F7F11D50A3A" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

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
             var textfieldname = $('#<%=txtSupplier.ClientID%>');
                DropdownFunction(textfieldname);
                $('#<%=txtSupplier.ClientID%>').autocomplete({
                source: function (request, response) {

                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadSupplierForSupplierPerformanceReport") %>',
                        data: "{ 'prefix': '" + request.term + "'}",
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

                    $("#<%=this.hifSupplier.ClientID%>").val(i.item.val);
                },
                minLength: 0
            });
         });

                </script>

     <script>
         $(document).ready(function () {
             var textfieldname = $('#<%=txtMCode.ClientID%>');
             DropdownFunction(textfieldname);
             $('#<%=txtMCode.ClientID%>').autocomplete({
                    source: function (request, response) {

                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetMCode") %>',
                            data: "{ 'prefix': '" + request.term + "','TenantID':'1'}",
                        dataType: "json",
                        type: "POST",
                        async: true,
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            if (data.d == '')
                            { alert('No material available') };
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
                <nobr>Supplier Performance Report</nobr>
            </td>

            <td class="parmArea" colspan="2" align="right">

                <table border="0" cellpadding="3" cellspacing="1">

                    <tr>
                        <td align="left">
                            <label style="font-size: 15px">Supplier : </label>
                            <br />
                            <asp:TextBox runat="server" ID="txtSupplier"  Width="200"  SkinID="txt_Req"></asp:TextBox>
                            <asp:HiddenField runat="server" ID="hifSupplier" />
                        </td>
                        <td align="left" >
                            <label style="font-size: 15px">Part Number : </label>
                            <br />
                            <asp:TextBox runat="server" ID="txtMCode"  SkinID="txt_Req"></asp:TextBox>
                            <asp:HiddenField runat="server" ID="hifMCode" />
                        </td>


                        <td align="right" >
                            <br />
                            <asp:LinkButton ID="lnkGetReport" CssClass="ui-btn ui-button-large" runat="server" Text="View Report" OnClick="lnkGetReport_Click" />

                        </td>

                        <!--  <td   align="left" class="FormLabels" colspan="4" >
            
            Select Printer :<br />
            
            <asp:DropDownList ID="ddlPrinters" SkinID="ddlRequired" Width="150" runat="server"></asp:DropDownList>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:LinkButton  ID="lnkPrint" CssClass="ButMSGo"  runat="server" Text="Print" OnClick="lnkPrint_Click"  /></td> -->


                    </tr>
                    </table>
                </td>

                    <tr>
                        <td id="tdRePortParent" align="center" colspan="2" height="400">

                            <rsweb:ReportViewer ID="rvSUPPerformanceReport" Visible="false" runat="server" Width="99.9%" Height="800" CssClass="rptClass" ZoomMode="Percent"
                                ShowParameterPrompts="false" ProcessingMode="Remote" BorderColor="Teal" ShowPrintButton="false"
                                InternalBorderColor="Teal" InternalBorderStyle="Solid" InternalBorderWidth="1">
                            </rsweb:ReportViewer>

                        </td>
                    </tr>

                </table>

    </div>
</asp:Content>
