<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SalesOrder.aspx.cs" Inherits="MRLWMSC21.mReports.SalesOrder" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=B03F5F7F11D50A3A" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:ScriptManager ID="mySManager" runat="server" EnablePartialRendering="true" />


    <script>
        $(document).ready(function () {
            var tdReportParent = document.getElementById('tdRePortParent');
            var tdChildTables = tdReportParent.getElementsByTagName('table');
            for (index = 0; index < tdChildTables.length; index++) {
                tdChildTables[index].setAttribute("align", "center");
                tdChildTables[index].setAttribute("align", "center");
            }
        });
    </script>


    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%= this.atcFromDate.ClientID %>").datepicker({
                dateFormat: "dd/mm/yy",
                maxDate: new Date(),
                onSelect: function (selected) {
                    $("#<%= this.atcToDate.ClientID %>").datepicker("option", "minDate", selected, { dateformate: "dd/mm/yy" })
                }
            });
            $("#<%= this.atcToDate.ClientID %>").datepicker({
                dateFormat: "dd/mm/yy",
                maxDate: new Date(),
            });
            $("#<%= this.atcSODate.ClientID %>").datepicker({
                dateFormat: "dd/mm/yy",
                maxDate: new Date(),
            });

            var textfieldname = $('#<%=this.atcSONumber.ClientID%>');
            DropdownFunction(textfieldname);
            $('#<%=this.atcSONumber.ClientID%>').autocomplete({

                source: function (request, response) {
                    // alert('ffffffff');
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadSONumbersForSalesOrderReport") %>',
                        data: "{ 'prefix': '" + request.term + "','CustomerID':'0','TenentID':'" +<%=cp.TenantID%> +"'}",
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
                    $("#<%=hifSONumber.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });


            var textfieldname = $('#<%=this.atcSOType.ClientID%>');
            DropdownFunction(textfieldname);
            $("#<%= this.atcSOType.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadSOTypes") %>',
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
                    $("#<%=hifSOType.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });

            var textfieldname = $('#<%=this.atcSOStatus.ClientID%>');
            DropdownFunction(textfieldname);
            $("#<%= this.atcSOStatus.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadSOStatus") %>',
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
                    $("#<%=hifSOStatus.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });
        })

    </script>
<div class="dashed"></div>
<div class="pagewidth">

    <table border="0" cellpadding="2" cellspacing="0" align="center" width="100%">
        <tr>
            <td class="ModuleHeader" style="width: 30px">
                <nobr>Sales Order Report</nobr>
            </td>
            <td class="parmArea" colspan="2" align="right">

                <table border="0" cellpadding="3" cellspacing="1"> 

                    <tr>
                        <td align="left">
                            <label style="font-size: 15px">SO Number : </label>
                            <br />
                            <asp:TextBox runat="server" ID="atcSONumber" Width="130" Font-Size="14px" SkinID="txt_Req"></asp:TextBox>
                            <asp:HiddenField runat="server" ID="hifSONumber" />
                        </td>
                        <td align="left">
                            <label style="font-size: 15px">SO Date : </label>
                            <br />
                            <asp:TextBox runat="server" ID="atcSODate" Width="150" Font-Size="14px"></asp:TextBox>
                        </td>

                        <td align="left">
                            <label style="font-size: 15px">SO Status : </label>
                            <br />
                            <asp:TextBox runat="server" ID="atcSOStatus" Width="130" Font-Size="14px" SkinID="txt_Req"></asp:TextBox>
                            <asp:HiddenField runat="server" ID="hifSOStatus" />

                        </td>
                        <td align="left">
                            <label style="font-size: 15px">SO Type  : </label>
                            <br />
                            <asp:TextBox runat="server" ID="atcSOType" Width="130" Font-Size="14px" SkinID="txt_Req"></asp:TextBox>
                            <asp:HiddenField runat="server" ID="hifSOType" />

                        </td>


                        <tr>

                            <td align="left">
                                <label style="font-size: 15px">From Date : </label>
                                <br />
                                <asp:TextBox runat="server" ID="atcFromDate" Width="150" Font-Size="14px"></asp:TextBox>
                            </td>

                            <td align="left">
                                <label style="font-size: 15px">To Date : </label>
                                <br />
                                <asp:TextBox runat="server" ID="atcToDate" Width="150" Font-Size="14px"></asp:TextBox>
                            </td>
                            <td>
                                <%--<asp:Literal ID="ltStatus" runat="server" ></asp:Literal>
                         <br />--%>
                                <br />
                                <asp:LinkButton ID="lnkGetReport" CssClass="ui-btn ui-button-large" runat="server" Text="View Report" OnClick="lnkGetReport_Click" /></td>
                        </tr>
                    <tr>

                        <!--   <td   align="left" class="FormLabels" colspan="4" >
            
            Select Printer :<br />
            
            <asp:DropDownList ID="ddlPrinters" SkinID="ddlRequired" Width="150" runat="server"></asp:DropDownList>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:LinkButton  ID="lnkPrint" CssClass="ButMSGo"  runat="server" Text="Print" OnClick="lnkPrint_Click"  /></td> -->

                    </tr>
                </table>

            </td>

        </tr>
        <tr>
            <td id="tdRePortParent" align="center" colspan="2" height="400">

                <rsweb:ReportViewer ID="rvSOReport" runat="server" Visible="false" Width="99.9%" Height="800" CssClass="rptClass" ZoomMode="Percent"
                    ShowParameterPrompts="false" ProcessingMode="Remote" BorderColor="Teal" ShowPrintButton="true"
                    InternalBorderColor="Teal" InternalBorderStyle="Solid" InternalBorderWidth="1">
                </rsweb:ReportViewer>

            </td>
        </tr>
    </table>
    </div>
</asp:Content>
