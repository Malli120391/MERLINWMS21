<%@ Page Title=" Help :." Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Help.aspx.cs" Inherits="MRLWMSC21.Help.Help" MaintainScrollPositionOnPostback="true" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <asp:ScriptManager ID="smHelp" runat="server" EnablePartialRendering="true" SupportsPartialRendering="true"></asp:ScriptManager>

    <style type="text/css">

        .heading5 {

            color:#0094ff;
            font-weight:bold;
            font-family:Calibri;

        }

    </style>
    
    <br />

    <table border="0" cellpadding="5px" cellspacing="5px" align="center">

        <tr>
            <td>
                <h2 class="heading5">Operational Manuals</h2>
            </td>

        </tr>

        </table>

        <table border="0"  align="center">

        <tr>

            <td>
                <asp:HyperLink ID="HyperLink4" Target="_blank" NavigateUrl="~/Help/ADMINISTRATION.pdf" runat="server" ImageUrl="~/Images/Admin.png" ></asp:HyperLink>
            </td>

        </tr>



        <tr>

            <td>
                <asp:HyperLink Target="_blank" NavigateUrl="~/Help/MRLWMSC21-OperationalManual.pdf" runat="server" ImageUrl="~/Images/MRLWMSC21.png"></asp:HyperLink>
            </td>

        </tr>

        <tr>

            <td>
                <asp:HyperLink ID="HyperLink1" Target="_blank" NavigateUrl="~/Help/FalconSFT-Configuration.pdf"  runat="server" ImageUrl="~/Images/FalconMFG.png"></asp:HyperLink>
            </td>

        </tr>


        <tr>

            <td>
                <asp:HyperLink ID="HyperLink3" Target="_blank" NavigateUrl="~/Help/FalconSFT-OperationalManual.pdf"  ImageUrl="~/Images/FalconSFT.png" runat="server"></asp:HyperLink>
            </td>

        </tr>


        <tr>

            <td>
                <asp:HyperLink ID="HyperLink2" Target="_blank" NavigateUrl="~/Help/Handheld-OperationaManual.pdf" ImageUrl="~/Images/Handheld.png" runat="server"></asp:HyperLink>
            </td>

        </tr>

    </table>

    <br />
    <br />
    <br />

</asp:Content>
