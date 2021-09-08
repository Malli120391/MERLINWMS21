<%@ Page Title=" Customer Request :." Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerRequest.aspx.cs" Inherits="MRLWMSC21.mMaterialManagement.CustomerRequest" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="dashed"></div>
    <div class="pagewidth">
<table>
    <tr>
        <td colspan="3">
            <asp:RequiredFieldValidator ID="rfvCustomerName" Text="*" ForeColor="Red" runat="server" ControlToValidate="txtCustomerName" />
            <asp:Literal ID="ltCustomerName" runat="server" Text="CustomerName" />
            <asp:TextBox ID="txtCustomerName" runat="server" Width="160"/>
            
            
        </td>
       
    </tr>
    <tr>
        <td>
            <asp:RequiredFieldValidator ID="rfvPhone1" Text="*" ForeColor="Red" runat="server" ControlToValidate="txtPhone1" />
            <asp:Literal ID="ltPhone1" runat="server" Text="Phone No. 1" />
            <asp:TextBox ID="txtPhone1" runat="server" Width="160" />
        </td>
        <td>
            <asp:RequiredFieldValidator ID="rfvphone2" Text="*" ForeColor="Red" runat="server" ControlToValidate="txtPhone2" />
            <asp:Literal ID="lthone2" runat="server" Text="Phone No. 2" />
            <asp:TextBox ID="txtPhone2" runat="server" Width="160" />
        </td>
        <td>
            <asp:RequiredFieldValidator ID="rfvMobile" Text="*" ForeColor="Red" runat="server" ControlToValidate="txtmobile" />
            <asp:Literal ID="ltMobile" runat="server" Text="Mobile No." />
            <asp:TextBox ID="txtmobile" runat="server" Width="160" />
        </td>
    </tr>
    <tr>
        <td>
             <asp:RequiredFieldValidator ID="rfvFax" Text="*" ForeColor="Red" runat="server" ControlToValidate="txtFax" />
            <asp:Literal ID="ltFax" runat="server" Text="Fax" />
            <asp:TextBox ID="txtFax" runat="server" Width="160" />
        </td>
        <td>
             <asp:RequiredFieldValidator ID="rfvEmail" Text="*" ForeColor="Red" runat="server" ControlToValidate="txtEmail" />
            <asp:Literal ID="ltEmail" runat="server" Text="Fax" />
            <asp:TextBox ID="txtEmail" runat="server" Width="160" />
        </td>
       
    </tr>
    <tr>
        <td>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Text="*" ForeColor="Red" runat="server" ControlToValidate="txtEmail" />
            <asp:Literal ID="Literal1" runat="server" Text="Fax" />
            <asp:TextBox ID="TextBox1" runat="server" Width="160" />
        </td>
    </tr>
</table>
        </div>
</asp:Content>
