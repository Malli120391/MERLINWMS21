<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="MaterialActivityReport.aspx.cs" Inherits="MRLWMSC21.mReports.MaterialActivityReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=B03F5F7F11D50A3A" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:ScriptManager ID="mySManager" runat="server" EnablePartialRendering="true" />

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
    <table class="tbsty" style="table-layout:fixed">
        <tbody>
            <tr class="module_yellow">
               <td class="ModuleHeader fixed-width">
                    
  
                   <div><a href="../Default.aspx">Home</a> / Reports / <span class="breadcrumbd">Material Activity Report</span></div>
                </td>
             </tr>
        </tbody>
    </table>
<div class="pagewidth">



    <table border="0" cellpadding="2" cellspacing="0" align="left" width="100%">
        <tr>
            
            <td class="parmArea" colspan="2" align="right">

                <table border="0" cellpadding="3" cellspacing="1">

                    <tr>
                        <td>
                            <div class="flex__ end">
                                <div class="flex">
                                    <asp:TextBox runat="server" ID="txtMCode" SkinID="txt_Req" required=""></asp:TextBox>
                                    <asp:HiddenField runat="server" ID="hifMCode" />
                                    <label>Part Number</label>
                                </div>&nbsp;&nbsp;
                                <div>
                                    <br />
                                    <asp:LinkButton ID="lnkGetReport" CssClass="btn btn-primary" runat="server" Text="View Report" OnClick="lnkGetReport_Click" />
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>

            </td>

        </tr>
        <tr>

            <td id="tdRePortParent" align="center" colspan="2" height="400px">

                <rsweb:ReportViewer ID="rvMaterialActivityReport" runat="server" Visible="false" Width="99.9%" Height="800" CssClass="rptClass" ZoomMode="Percent" ShowZoomControl="true"
                    ShowParameterPrompts="false" ProcessingMode="Remote" BorderColor="Teal" ShowPrintButton="true"
                    InternalBorderColor="Teal" InternalBorderStyle="Double" InternalBorderWidth="1">
                </rsweb:ReportViewer>

            </td>

        </tr>
    </table>
</div>
</asp:Content>



