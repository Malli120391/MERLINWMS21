<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InwardNCReport.aspx.cs" Inherits="MRLWMSC21.mReports.InwardNCReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=B03F5F7F11D50A3A" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
  
     <asp:ScriptManager ID="mySManager" runat="server" EnablePartialRendering="true" />

    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%=this.txtPONumber.ClientID%>').autocomplete({
                source: function (request, response) {

                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadPONumbers") %>',
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
        });
    </script> 
<div class="dashed"></div>
<div class="pagewidth">
    <table border="0" cellpadding="2" cellspacing="0" align="left"  width="100%" >
        <tr>
            <td class="ModuleHeader" style="width:30px"><nobr>Inward NC Report</nobr></td>
            <td class="parmArea" colspan="2"  align="right"> 
                
            <table border="0" cellpadding="3" cellspacing="1"  >

                <tr>       
                

                <tr>                     
                                 
                    <td >
                        <label style="font-size:15px"> PO Number : </label> <br />
                         <asp:TextBox runat="server" ID="txtPONumber" Width="150" Height="14" Font-Size="14px"></asp:TextBox>
                           <asp:HiddenField runat="server" ID="hifPONumber" />
                    </td>
                     <td >
                         <asp:Literal ID="ltStatus" runat="server" ></asp:Literal>
                         <br />
                         <asp:LinkButton  ID="lnkGetReport" CssClass="ui-btn ui-button-large"  runat="server" Text="View Report" OnClick="lnkGetReport_Click" /></td>
                </tr>
                    <tr>

       <!-- <td   align="left" class="FormLabels" colspan="4" >
            
            <label style="font-size:15px"> Select Printer : </label> <br />
            
            <asp:DropDownList ID="ddlPrinters" SkinID="ddlRequired" Width="150" runat="server"></asp:DropDownList>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:LinkButton  ID="lnkPrint" CssClass="ButMSGo"  runat="server" Text="Print" OnClick="lnkPrint_Click"  /></td> -->

        </tr>
        </table>
            
                    </td>

        </tr>
        <tr>
        <td  align="center" colspan="2" height="400px" >
            
            <rsweb:ReportViewer ID="rvNCReport"  runat="server" Width="74%" Height="800" CssClass="rptClass"  ZoomMode="Percent" 
                 ShowParameterPrompts="false"  ProcessingMode="Remote" BorderColor="Teal" ShowPrintButton="true"
                InternalBorderColor="Teal" InternalBorderStyle="Solid" >
            </rsweb:ReportViewer>

        </td>

        </tr>

    </table>
    </div>
</asp:Content>
