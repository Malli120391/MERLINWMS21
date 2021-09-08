<%@ Page Title=" .: Job Rout Plan :." Language="C#" MasterPageFile="~/mManufacturingProcess/Manufacturing.master" AutoEventWireup="true" CodeBehind="JobRoutePlan.aspx.cs" Inherits="MRLWMSC21.mManufacturingProcess.JobRoutePlan" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ManfContent" runat="server">
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

        function printJRP()
        {
            $("#printJrp").print();
        }
    </script>
    <div id="printbutton" align="right">
        <br />
         <asp:ImageButton ID="lmbWorkOrderReport" ImageUrl="~/Images/Print 20X20..png" OnClientClick="printJRP();" ToolTip="Print JRP"  runat="server"></asp:ImageButton>
    </div>
   
    <div id="printJrp">
      
        
        <center><b>JOB/ROUTE PLAN</b></center>
        <table border="1" width="100%" align="center" style="border-collapse:collapse;" class="headertable">
    <tr>
        <td>
            <b>OEM Part No:</b>&nbsp&nbsp&nbsp<asp:Literal ID="ltOEMPartNo" runat="server"></asp:Literal>
        </td>
        <td>
            <b>JRP Type:</b>&nbsp&nbsp&nbsp<asp:Literal ID="ltJrpType" runat="server"></asp:Literal>
        </td>
    </tr>
    <tr>
        <td>
            <b>RT Part No:</b>&nbsp&nbsp&nbsp<asp:Literal ID="ltRtPartNumber" runat="server"></asp:Literal>
        </td>
        <td>
            <b>RELEASE DATE:</b>&nbsp&nbsp&nbsp<asp:Literal ID="ltReleaseDate" runat="server"></asp:Literal>
        </td>
    </tr>
     <tr>
        <td>
            <b>PART DESCRIPTION : </b>&nbsp&nbsp&nbsp<asp:Literal ID="ltPartDescription" runat="server"></asp:Literal>
        </td>
        <td>
            <b>KIT CODE/ASSEMBLY SERIAL NO:</b>&nbsp&nbsp&nbsp<asp:Literal ID="ltKitCode" runat="server"></asp:Literal>
        </td>
    </tr>
     <tr>
        <td>
            <b>PROJECT CODE:  </b>&nbsp&nbsp&nbsp<asp:Literal ID="ltProjectCode" runat="server"></asp:Literal>
        </td>
        <td>
            <b>MONTH OF MANUFACTURE :</b>&nbsp&nbsp&nbsp<asp:Literal ID="ltMfgdate" runat="server"></asp:Literal>
        </td>
    </tr>
    <tr>
        <td>
            <b>WORK ORDER NO:  </b>&nbsp&nbsp&nbsp<asp:Literal ID="ltWorkOrder" runat="server"></asp:Literal>
        </td>
        <td>
            <b>NEXT ASSEMBLY No/ STAGE No: </b>&nbsp&nbsp&nbsp<asp:Literal ID="ltNextAssembly" runat="server"></asp:Literal>
        </td>
    </tr>
      <tr>
        <td colspan="2">
            <b>JOB TYPE: </b>&nbsp&nbsp&nbsp<asp:Literal ID="ltJobType" runat="server"></asp:Literal>
        </td>
        
    </tr>
          <tr>
        <td colspan="2">
            <b>APPROVER'S NAME:</b>&nbsp&nbsp&nbsp<asp:Literal ID="ltAproverName" runat="server"></asp:Literal>
        </td>
       
    </tr>
    <tr>
        <td colspan="2">
            <b>APPROVER'S SIGN & DATE:</b>&nbsp&nbsp&nbsp<asp:Literal ID="ltAppSign" runat="server"></asp:Literal>
        </td>
        
    </tr>
      <tr>
        <td colspan="2">
            <b>QC  APPROVER'S NAME:</b>&nbsp&nbsp&nbsp<asp:Literal ID="ltQcname" runat="server"></asp:Literal>
        </td>
        
    </tr>
          <tr>
        <td colspan="2">
            <b>QC  APPROVER'S SIGN & DATE:</b>&nbsp&nbsp&nbsp<asp:Literal ID="ltQcSign" runat="server"></asp:Literal>
        </td>
        
    </tr>
    <tr>
        <td colspan="2" align="center"><b> Reference Documents</b></td>
    </tr>
    <tr>
        <td  colspan="2"><asp:Literal ID="ltReferenceDocumnets" runat="server"></asp:Literal></td>
    </tr>
    
</table>  
    <center><b>Activity Details</b></center>
    <table id="tblJRP" runat="server" border="1" style="border-collapse:collapse;">
        <tr class="headertable">
            <td><b>Opn. No.</b></td>
            <td><b>Activity. No</b></td>
            <td><b>Operation details</b></td>
            <td><b>Work Station No</b></td>
            <td><b>Components</b></td>
            <td><b>Operator</b></td>
            <td><b>Sup. Sign</b></td>
            <td><b>Ins. Sign  & stamp</b></td>
            <td><b>Remarks </b></td>
        </tr>
    </table>
        <br />
        <br />
         <img border="0" src="../Images/inventrax_logo_100x41.gif"  align="right" class="OnlyForPrint" >
        </div>
   

</asp:Content>
