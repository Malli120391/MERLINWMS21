<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PieChart.aspx.cs" Inherits="MRLWMSC21.mReports.PieChart" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   <table border="0" cellpadding="0" cellspacing="0" align="center" width="98%" height="450px">
        <tr>
            <td>
    <asp:Chart ID="Chart1" runat="server" Height="300px" Width="400px" Visible = "true">
        
            <Titles>
                <asp:Title ShadowOffset="3" Name="Items" />
            </Titles>
            <Legends>
                <asp:Legend Alignment="Center" Docking="Bottom" IsTextAutoFit="False" Name="Default" LegendStyle="Row" />
            </Legends>
            <Series>
                <asp:Series ChartType="Pie" Name="Default" />
            </Series>
            <ChartAreas>
                <asp:ChartArea  Name="ChartArea1" BorderWidth="0" />
            </ChartAreas>
        </asp:Chart>
                </td>
            </tr>
        </Table>
</asp:Content>
