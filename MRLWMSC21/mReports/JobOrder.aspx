<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="JobOrder.aspx.cs" Inherits="MRLWMSC21.mReports.JobOrder" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

     <%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=B03F5F7F11D50A3A" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

     <asp:ScriptManager ID="mySManager" runat="server" EnablePartialRendering="true" />


    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%= this.txtFromDate.ClientID %>").datepicker({ dateFormat: "dd/mm/yy" });
            $("#<%= this.txtToDate.ClientID %>").datepicker({ dateFormat: "dd/mm/yy" });
        });

     </script>
<div class="dashed"></div>
<div class="pagewidth">

     <table border="0" cellpadding="2" cellspacing="0" align="left"  width="100%" >
        <tr>
            
            <td class="parmArea" colspan="2"  align="right"> 
                
            <table border="0" cellpadding="3" cellspacing="1"  >

                <tr>                   
                

                <tr>
                   
                    <td>
                        From Date : <br />
                         <asp:TextBox runat="server" ID="txtFromDate" Width="150" ></asp:TextBox>
                    </td>
              
                    <td >
                        To Date : <br />
                         <asp:TextBox runat="server" ID="txtToDate" Width="150" ></asp:TextBox>
                    </td>
                     <td >
                         <asp:Literal ID="ltStatus" runat="server" ></asp:Literal>
                         <br />
                         <asp:LinkButton  ID="lnkGetReport" CssClass="ui-btn ui-button-large"  runat="server" Text="View Report" OnClick="lnkGetReport_Click" /></td>
                </tr>
                    <tr>

        <td   align="left" class="FormLabels" colspan="4" >
            
            Select Printer :<br />
            
            <asp:DropDownList ID="ddlPrinters" SkinID="ddlRequired" Width="150" runat="server"></asp:DropDownList>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:LinkButton  ID="lnkPrint" CssClass="ButMSGo"  runat="server" Text="Print" OnClick="lnkPrint_Click"  /></td>

        </tr>
        </table>
            
                    </td>

        </tr>
        <tr>
        <td  align="center" colspan="2">
            
            <rsweb:ReportViewer ID="rvJobOrder" runat="server" Width="100%" Height="800" CssClass="rptClass"  ZoomMode="Percent" 
                 ShowParameterPrompts="false"  ProcessingMode="Remote" BorderColor="YellowGreen"  ShowPrintButton="false"
                InternalBorderColor="YellowGreen" InternalBorderStyle="Solid" InternalBorderWidth="3"   >
                
            </rsweb:ReportViewer>

        </td>


        </tr>

    </table>
    </div>
</asp:Content>
