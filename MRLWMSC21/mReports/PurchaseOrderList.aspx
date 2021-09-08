<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PurchaseOrderList.aspx.cs" Inherits="MRLWMSC21.mReports.PurchageOrderList" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=B03F5F7F11D50A3A" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:ScriptManager ID="mySManager" runat="server" EnablePartialRendering="true" />

    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%= this.txtFromDate.ClientID %>").datepicker({
                dateFormat: "dd/mm/yy",
                maxDate: new Date(),
                onSelect: function (selected) {
                    $(this).focus();
                    $("#<%= this.txtToDate.ClientID %>").datepicker("option", "minDate", selected, { dateFormate: "dd/mm/yy" });
                 }
            });
            $("#<%= this.txtToDate.ClientID %>").datepicker({
                dateFormat: "dd/mm/yy",
                maxDate: new Date(),
            });
            $("#<%= this.txtPODate.ClientID %>").datepicker({
                dateFormat: "dd/mm/yy",
                onselect: function (selected) {
                    $(this).focus();
                }
            });

            var textfieldname = $('#<%=this.atcPONumber.ClientID%>');
            DropdownFunction(textfieldname);
            $('#<%=this.atcPONumber.ClientID%>').autocomplete({
                 source: function (request, response)  {

                     $.ajax({
                         url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadPONumbersForPurchaseOrderReport") %>',
                         data: "{ 'prefix': '" + request.term + "','SupplierID':'0','TenentID':'" +<%=cp.TenantID%> +"'}",
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

                     $("#<%=this.hifPONumber.ClientID%>").val(i.item.val);

                 },
                 minLength: 0
             });

            var textfieldname = $('#<%=this.atcPOType.ClientID%>');
            DropdownFunction(textfieldname);
            $("#<%= this.atcPOType.ClientID %>").autocomplete({
                 source: function (request, response) {
                     $.ajax({
                         url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadPOTypes") %>',
                         data: "{ 'prefix': '" + request.term + "'}",
                         dataType: "json",
                         type: "POST",
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

                     $("#<%=hifPOType.ClientID %>").val(i.item.val);
                 },
                 minLength: 0
             });

            var textfieldname = $('#<%=this.atcSupplier.ClientID%>');
            DropdownFunction(textfieldname);
            $("#<%= this.atcSupplier.ClientID %>").autocomplete({
                 source: function (request, response) {
                     $.ajax({
                         url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadSupplierData") %>',
                         data: "{ 'prefix': '" + request.term + "','TenantID':'" +<%=cp.TenantID%> +"'}",
                         dataType: "json",
                         type: "POST",
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

                     $("#<%=hifSupplier.ClientID %>").val(i.item.val);

                 },
                 minLength: 0
             });

            var textfieldname = $('#<%=this.atcPOStatus.ClientID%>');
            DropdownFunction(textfieldname);
            $("#<%= this.atcPOStatus.ClientID %>").autocomplete({
                 source: function (request, response) {
                     $.ajax({
                         url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadPOStatus") %>',
                         data: "{ 'prefix': '" + request.term + "'}",
                         dataType: "json",
                         type: "POST",
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

                     $("#<%=hifPOStatus.ClientID %>").val(i.item.val);

                 },
                 minLength: 0
             });

        });

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
            <td class="ModuleHeader">
                <nobr>Purchase Order Report</nobr>
            </td>
            <td class="parmArea" colspan="2" align="right">

                <table border="0" cellpadding="1" cellspacing="2">

                    <tr>
                        <td align="left">
                            <label style="font-size: 15px">PO Number : </label>
                            <br />
                            <asp:TextBox runat="server" ID="atcPONumber" Font-Size="14px" Width="130" SkinID="txt_Req"></asp:TextBox>
                            <asp:HiddenField runat="server" ID="hifPONumber" />
                        </td>
                        <td align="left">
                            <label style="font-size: 15px">PO Date : </label>
                            <br />
                            <asp:TextBox runat="server" ID="txtPODate" Font-Size="14px" Width="150"></asp:TextBox>

                        </td>

                        <td align="left">
                            <label style="font-size: 15px">PO Status : </label>
                            <br />
                            <asp:TextBox runat="server" ID="atcPOStatus" Width="130" Font-Size="14px" SkinID="txt_Req"></asp:TextBox>
                            <asp:HiddenField runat="server" ID="hifPOStatus" />
                        </td>
                        <td align="left">
                            <label style="font-size: 15px">PO Type  : </label>
                            <br />
                            <asp:TextBox runat="server" ID="atcPOType" Width="130" SkinID="txt_Req" Font-Size="14px"></asp:TextBox>
                            <asp:HiddenField ID="hifPOType" runat="server" />
                        </td>

                        <tr>
                            <td align="left">
                                <label style="font-size: 15px">Supplier : </label>
                                <br />
                                <asp:TextBox runat="server" ID="atcSupplier" Width="130" SkinID="txt_Req" Font-Size="14px"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hifSupplier" />
                            </td>
                            <td align="left">
                                <label style="font-size: 15px">From Date : </label>
                                <br />
                                <asp:TextBox runat="server" ID="txtFromDate" Width="150" Font-Size="14px"></asp:TextBox>
                            </td>

                            <td align="left">
                                <label style="font-size: 15px">To Date : </label>
                                <br />
                                <asp:TextBox runat="server" ID="txtToDate" Width="150" Font-Size="14px"></asp:TextBox>
                            </td>
                            <td align="right">
                                <br />
                                <asp:LinkButton ID="lnkGetReport" CssClass="ui-btn ui-button-large" runat="server" Text="View Report" OnClick="lnkGetReport_Click" /></td>
                        </tr>
                    <tr>

                        <!-- <td   align="left" class="FormLabels" colspan="4" >            
                        <label style="font-size:15px"> Select Printer : </label> <br />            
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

                <rsweb:ReportViewer ID="rvMRReport" runat="server" Visible="false" Width="99.9%" Height="800" CssClass="rptClass" ZoomMode="Percent"
                    ShowParameterPrompts="false" ProcessingMode="Remote" BorderColor="Teal" ShowPrintButton="true"
                    InternalBorderColor="Teal" InternalBorderStyle="Solid" InternalBorderWidth="1">
                </rsweb:ReportViewer>

            </td>

        </tr>

    </table>
    </div>
</asp:Content>
