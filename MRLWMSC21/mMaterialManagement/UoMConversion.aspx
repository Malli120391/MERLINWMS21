<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UoMConversion.aspx.cs" Inherits="MRLWMSC21.mMaterialManagement.UoMConversion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script>
        function ShowConversionValue(ToDropDown) {            
            document.getElementById('lbConversion').innerText = ToDropDown.value;
        }
    </script>

    <div>
        <table>
            <tr>
                <td colspan="3">
                    <asp:DropDownList ID="ddlMeadureType" OnSelectedIndexChanged="ddlMeadureType_SelectedIndexChanged" runat="server" AutoPostBack="true"> 
                    <asp:ListItem Text="Length" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Area" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Volume" Value="3"></asp:ListItem>
                    <asp:ListItem Text="Weight" Value="4"></asp:ListItem>
                    <asp:ListItem Text="Liquid Measurements" Value="5"></asp:ListItem>
                    <asp:ListItem Text="Termperture" Value="6"></asp:ListItem>

        </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    From Measurement:<br/>
                    <asp:DropDownList  ID="ddlFromMeasurement" runat="server" OnSelectedIndexChanged="ddlFromMeasurement_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                </td>
                <td>
                    To Measurement:<br/>
                    <asp:DropDownList ID="ddlToMeasurement" runat="server" onchange="ShowConversionValue(this);"></asp:DropDownList>
                </td>
                <td>
                    <label id="lbConversion" style="font-size:20px;"></label>
                </td>
            </tr>
        </table>
        
    </div>

</asp:Content>
