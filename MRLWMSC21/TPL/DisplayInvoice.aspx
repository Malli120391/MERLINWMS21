<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DisplayInvoice.aspx.cs" Inherits="MRLWMSC21.TPL.DisplayInvoice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <table class="internalData" width="100%" cellpadding="5" cellspacing="5">  
        <tr>
            <td>
                <iframe id="iframe" runat="server" style="width:100%; height:77vh;"></iframe>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:LinkButton ID="lnksaveInvoice" runat="server" CssClass="btn btn-primary" OnClick="lnksaveInvoice_Click" >Send<i class="material-icons vl">send</i></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                <asp:LinkButton ID="lnkCancel" runat="server" CssClass="btn btn-primary" OnClick="lnkCancel_Click">Cancel<%=MRLWMSC21Common.CommonLogic.btnfaClear%></asp:LinkButton>
            </td>
        </tr>
    </table>
    

</asp:Content>
