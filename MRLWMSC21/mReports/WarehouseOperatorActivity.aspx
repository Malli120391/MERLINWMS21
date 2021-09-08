<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WarehouseOperatorActivity.aspx.cs" Inherits="MRLWMSC21.mReports.WarehouseOperatorActivity" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=B03F5F7F11D50A3A" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="mySManager" runat="server" EnablePartialRendering="true" />

    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%= this.txtFromDate.ClientID %>").datepicker({
                dateFormat: "dd/mm/yy",
                maxDate: new Date(),
                onSelect: function (selected) {
                    $("#<%=this.txtToDate.ClientID%>").datepicker("option", "minDate", selected, { dateFormat: "dd/mm/yy" })
                }
            });
            $("#<%= this.txtToDate.ClientID %>").datepicker({
                dateFormat: "dd/mm/yy",
                maxDate: new Date()
            });
        });

    </script>

    <%--<script>
         $(document).ready(function () {
             var textfieldname = $('#<%=txtDepartment.ClientID%>');
             DropdownFunction(textfieldname);
             $('#<%=txtDepartment.ClientID%>').autocomplete({
                 source: function (request, response) {
                     $.ajax({
                         url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadDepartment") %>',
                         data: "{ 'prefix': '" + request.term + "','TenentID':'1'}",
                            dataType: "json",
                            type: "POST",
                            async: true,
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                if (data.d == '')
                                { alert('No Dept. available') };
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

                     $("#<%=this.hifDepartment.ClientID%>").val(i.item.val);
                    },
                    minLength: 0
             });
         });

                </script>--%>

    <%--<script>
        $(document).ready(function () {
            var textfieldname = $('#<%=txtDivision.ClientID%>');
             DropdownFunction(textfieldname);
             $('#<%=txtDivision.ClientID%>').autocomplete({
                 
                 source: function (request, response) {
                     $.ajax({ 
                         url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadDivision") %>',
                         data: "{ 'prefix': '" + request.term + "', 'TenantID':'1', 'DeptID':'" + $("#<%=this.hifDepartment.ClientID%>").value + "' }",
                         dataType: "json",
                         type: "POST",
                         async: true,
                         contentType: "application/json; charset=utf-8",
                         success: function (data) {
                             if (data.d == '')
                             { alert('No Dept. available') };
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

                     $("#<%=this.hifDivision.ClientID%>").val(i.item.val);
                 },
                 minLength: 0
             });
         });

                </script>--%>


    <script>
        $(document).ready(function ()  {
            var tdReportParent = document.getElementById('tdRePortParent');
            var tdChildTables = tdReportParent.getElementsByTagName('table');
            for (index = 0; index < tdChildTables.length; index++) {
                tdChildTables[index].setAttribute("align", "center");                
            }
        });
    </script>
<div class="dashed"></div>
<div class="pagewidth">
    <table border="0" cellpadding="2" cellspacing="0" align="center" width="100%">
        <tr>
            <td class="ModuleHeader" style="width: 30px">
                <nobr> Operator Activity Report </nobr>
            </td>
            <td class="parmArea" colspan="2" align="right">

                <table border="0" cellpadding="3" cellspacing="1">

                    <tr>
                        <td align="left">
                            <label style="font-size: 15px">From Date : </label>
                            <br />
                            <asp:TextBox runat="server" ID="txtFromDate" Width="120" Font-Size="14px" ></asp:TextBox>
                        </td>

                        <td align="left">
                            <label style="font-size: 15px">To Date : </label>
                            <br />
                            <asp:TextBox runat="server" ID="txtToDate" Width="120" Font-Size="14px" ></asp:TextBox>
                        </td>
                        <%--<td align="left">
                        <label style="font-size:15px"> Department : </label> <br />
                         <asp:TextBox runat="server" ID="txtDepartment" Width="150" Font-Size="14px" Height="14"></asp:TextBox>
                         <asp:HiddenField runat="server" ID="hifDepartment" />                        
                    </td>
                    <td align="left">
                        <label style="font-size:15px"> Division : </label> <br />
                         <asp:TextBox runat="server" ID="txtDivision" Width="150" Font-Size="14px" Height="14"></asp:TextBox>
                        <asp:HiddenField runat="server" ID="hifDivision" Value="0" />                          
                    </td>--%>
                        <td>
                            <br />
                            <asp:LinkButton ID="lnkGetReport" CssClass="ui-btn ui-button-large" runat="server" Text="View Report" OnClick="lnkGetReport_Click" /></td>
                    </tr>



                </table>
            </td>
        </tr>
        <tr>
            <td id="tdRePortParent" align="center" colspan="2" height="400px">

                <rsweb:ReportViewer ID="rvWarehouseOperatorActivity" runat="server" Visible="false" Width="99.9%" Height="600" CssClass="rptClass" ZoomMode="Percent"
                    ShowParameterPrompts="false" ProcessingMode="Remote" BorderColor="Teal" ShowPrintButton="false"
                    InternalBorderColor="Teal" InternalBorderStyle="Solid" InternalBorderWidth="1">
                </rsweb:ReportViewer>


            </td>

        </tr>

    </table>
    </div>
</asp:Content>
