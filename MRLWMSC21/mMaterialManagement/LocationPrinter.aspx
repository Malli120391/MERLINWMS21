<%@ Page Title=" .: Location Printer :. " Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LocationPrinter.aspx.cs" Inherits="MRLWMSC21.LocationPrinter" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="dashed"></div>
    <div class="pagewidth">
      <table border="0" cellPadding="0" cellSpacing="0" align="center" > 
                    <tr>
                        <td colspan="2" class="SubHeading3"><br /> Location Printing :  <br /><br /></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblStatus" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td >
                           
                            Zone: 
                             <asp:DropDownList ID="ddlPrintlabel" runat="server">
                                    <asp:ListItem Text="Store" Value="S1"/>
                                    <asp:ListItem Text="QC" Value="Q1"/>
                                    <asp:ListItem Text="Finished Goods" Value="F1"/>
                                    <asp:ListItem Text="Receiving" Value="R1"/>
                                    <asp:ListItem Text="Production" Value="P1"/>
                                    <asp:ListItem Text="Cut & Code " Value="C1"/>
                                    <asp:ListItem Text="Potting" Value="P2"/>
                                    <asp:ListItem Text="Braiding" Value="B1"/>
                             </asp:DropDownList>
                       
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            Rack :
                            <asp:DropDownList ID="ddlRack" runat="server">
                                <asp:ListItem Text="01" Value="S101"/>
                                <asp:ListItem Text="02" Value="S102"/>
                                <asp:ListItem Text="03" Value="S103"/>
                                <asp:ListItem Text="04" Value="S104"/>
                                <asp:ListItem Text="05" Value="S105"/>
                                <asp:ListItem Text="06" Value="S106"/>
                                <asp:ListItem Text="07" Value="S107"/>
                                <asp:ListItem Text="08" Value="S108"/>
                                <asp:ListItem Text="09" Value="S109"/>
                                <asp:ListItem Text="10" Value="S110"/>
                                <asp:ListItem Text="11" Value="S111"/>
                                <asp:ListItem Text="12" Value="S112"/>
                                <asp:ListItem Text="13" Value="S113"/>
                                <asp:ListItem Text="14" Value="S114"/>
                                <asp:ListItem Text="15" Value="S115"/>
                                <asp:ListItem Text="16" Value="S116"/>
                                <asp:ListItem Text="17" Value="S117"/>
                                <asp:ListItem Text="18" Value="S118"/>
                                <asp:ListItem Text="19" Value="S119"/>
                                <asp:ListItem Text="20" Value="S120"/>
                                <asp:ListItem Text="21" Value="S121"/>
                                <asp:ListItem Text="22" Value="S122"/>
                                <asp:ListItem Text="23" Value="S123"/>
                                <asp:ListItem Text="24" Value="S124"/>
                                <asp:ListItem Text="25" Value="S125"/>
                                <asp:ListItem Text="26" Value="S126"/>
                                <asp:ListItem Text="27" Value="S127"/>
                                <asp:ListItem Text="28" Value="S128"/>
                                <asp:ListItem Text="29" Value="S129"/>
                                <asp:ListItem Text="30" Value="S130"/>
                                <asp:ListItem Text="31" Value="S131"/>
                                <asp:ListItem Text="32" Value="S132"/>
                                <asp:ListItem Text="33" Value="S133"/>
                                <asp:ListItem Text="34" Value="S134"/>
                                <asp:ListItem Text="35" Value="S135"/>
                                
                             </asp:DropDownList>

                              <asp:LinkButton ID="lnkPrintLabel" SkinID="lnkButEmpty" Text="Print Label" OnClick="lnkPrintLabel_Click" runat="server" CssClass="FormLabels"></asp:LinkButton></td>
                       
                    </tr>
          </table>
</div>
</asp:Content>
