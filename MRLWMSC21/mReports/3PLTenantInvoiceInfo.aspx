<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="3PLTenantInvoiceInfo.aspx.cs" Inherits="MRLWMSC21.mReports._3PLTenantInvoiceInfo" %>

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
            var textfieldname = $('#<%=txtTenant.ClientID%>');
            DropdownFunction(textfieldname);
            $('#<%=txtTenant.ClientID%>').autocomplete({
                source: function (request, response) {

                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadTenantsForReports") %>',
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

                    $("#<%=this.hifTenant.ClientID%>").val(i.item.val);
                },
                minLength: 0
            });
        });
    </script>


    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%= this.txtFromDate.ClientID %>").datepicker({
                dateFormat: "dd/mm/yy",
                maxDate: new Date(),
                onSelect: function (selected) {
                    $(this).focus();
                    $("#<%= this.txtToDate.ClientID %>").datepicker("option", "minDate", selected, { dateFormate: "dd/mm/yy" })
                 }
            });
            $("#<%= this.txtToDate.ClientID %>").datepicker({
                dateFormat: "dd/mm/yy",
                maxDate: new Date(),
                onSelect: function (selected) {
                    $(this).focus();
                }
            });
        });

    </script>   

      <style>
        .gvLightBlueNew {
            border: 0px !important;
            border-radius: 0px !important;
            background-color: #b2bbc8 !important;
        }
        .gvLightBlueNew_headerGrid {
            background-color: #455b7c;
            text-align: left;
            color: #ffffff;
        }

        .internalData td, th {
            padding:7px
        }
          a.ButEmpty {
              font-size: 13pt;
              font-weight: bold;
              padding-left: 5px;
              padding-right: 10px;
              padding-top: 5px;
              padding-bottom: 5px;
              box-shadow: var(--z1);
              margin: 1px;
              display: inline-block;
              background: #fff;
              border-radius: 30px;
              margin: auto;
              font-size: 14px;
              text-decoration: none;
              padding: 8px;
              text-align: center;
          }
          .flex input[type="text"], input[type="number"], textarea
          {
              width:75% !important;
          }
    </style>  
       <table class="tbsty">
        <tbody>
            <tr class="module_yellow">
               <td class="ModuleHeader fixed-width">
                    Tenant Invoice Statistics
                </td>
             </tr>
        </tbody>
    </table>
    <div class="dashed"></div>
<div class="pagewidth">
    <table border="0" cellpadding="2" cellspacing="0" align="center" width="100%">
        <tr>
            <td class="parmArea" colspan="2" align="right">

                <table border="0" cellpadding="3" cellspacing="1" class="fit right">

                    <tr>
                        <td align="left">
                            <div class="flex">
                            <asp:TextBox runat="server" ID="txtTenant" SkinID="txt_Req" required=""></asp:TextBox>
                            <label>Tenant</label>
                            <span class="errorMsg"></span>
                            <asp:HiddenField runat="server" ID="hifTenant" />
                            </div>
                        </td>

                           <%-- <td align="left">
                            <label style="font-size: 15px">Role : </label>
                            <br />
                            <asp:TextBox runat="server" ID="txtRole" Width="135" Font-Size="14px" SkinID="txt_Req"></asp:TextBox>
                            <asp:HiddenField runat="server" ID="hifRole" />
                        </td>--%>

                         <td align="left">
                            <div class="flex">
                            <asp:TextBox runat="server" ID="txtFromDate" Width="130"  required=""></asp:TextBox>
                             <label>From Date </label>
                             <span class="errorMsg"></span>
                                </div>
                        </td>

                        <td align="left">
                            <div class="flex">
                            <asp:TextBox runat="server" ID="txtToDate" Width="130" required=""></asp:TextBox>
                            <label>To Date </label>
                            <span class="errorMsg"></span>
                                </div>
                        </td>
                        <td>
                            <asp:LinkButton ID="lnkGetReport" CssClass="btn btn-primary" runat="server" Text="View Report" OnClick="lnkGetReport_Click" /></td>
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
            <td id="tdRePortParent" align="center" colspan="2" height="600px">

                <rsweb:ReportViewer ID="rv3PLTenantInvoiceInfo" runat="server" Visible="false" Width="99.9%" Height="600" CssClass="rptClass" ZoomMode="Percent"
                    ShowParameterPrompts="false" ProcessingMode="Remote" BorderColor="Teal" ShowPrintButton="false"
                    InternalBorderColor="Teal" InternalBorderStyle="Solid" InternalBorderWidth="1">
                </rsweb:ReportViewer>

            </td>

        </tr>

    </table>
    </div>
</asp:Content>

