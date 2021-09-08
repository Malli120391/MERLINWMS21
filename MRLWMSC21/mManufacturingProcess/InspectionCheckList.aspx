<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InspectionCheckList.aspx.cs" Inherits="MRLWMSC21.mManufacturingProcess.InspectionCheckList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
      <script type="text/javascript" src="Scripts/ben_Print.js"></script>
    <link href="../PrintStyle.css" type="text/css" rel="stylesheet" media="print" />
    <style type="text/css"></style>
    <style type="text/css">
        #printJrp {
           
            padding-left:10PX;
            padding-right:10PX;
            font-family:Calibri;
        }
       .headertable
         {
            background-color:#CCFFFF;  
        }
    </style>
    <script type="text/javascript">

        function printJRP() {
            $("#printJrp").print();
        }
    </script>
    <div id="printbutton">
         <asp:ImageButton ID="lmbWorkOrderReport" ImageUrl="~/Images/Print 20X20..png" OnClientClick="printJRP();" ToolTip="Print JRP"  runat="server"></asp:ImageButton>
    </div>
   
    <div id="printJrp">
      <img border="0" src="../Images/RT_LOGO.png"  align="right" class="OnlyForPrint" >
        <br /><br />
    
        <b>List of reference documents:</b>
        <asp:Literal ID="ltDocuments" runat="server"></asp:Literal>
        <center><b>STAGE INSPECTION PROCEDURE SHEET FOR WIRE ASSEMBLY</b></center>
    <table id="tblCheckList" runat="server" width="100%" border="1" style="border-collapse:collapse;">
        <tr class="headertable">
            <td><b>Opn. No.</b></td>
            <td style="border-top-style:none;border-bottom-style:none;">
                <table  width="1000" border="1" height="50" style="border-collapse:collapse;border-bottom-style:none;border-top-style:none;">
                    <tr>
                     <td width="400" style="border-collapse:collapse;border-bottom-style:none;border-left-style:hidden"><b>Check Point</b></td>
                    <td width="100" style="border-collapse:collapse;border-bottom-style:none;"><b>Inspection Type</b></td>
                    <td width="100" style="border-collapse:collapse;border-bottom-style:none;"><b>Tool History</b></td>
                     <td width="300" style="border-collapse:collapse;border-bottom-style:none;">
                         <table>
                             <tr>
                                <td width="100"><b>Operator</b></td>
                                <td width="100"><b>Supervisor</b></td>
                                 <td width="100"><b>QC</b></td>
                             </tr>
                             
                         </table>
                     </td>
                     <td width="100" style="border-collapse:collapse;border-bottom-style:none;"><b>Reference Doc</b></td>
                    </tr>
                    
                </table>
            </td>
            
        </tr>
    </table>
        </div>
</asp:Content>
