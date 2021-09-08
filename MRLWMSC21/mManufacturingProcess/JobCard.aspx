<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="JobCard.aspx.cs" Inherits="MRLWMSC21.mManufacturingProcess.JobCard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="Scripts/ben_Print.js"></script>
    <style>
      
        .printReport {
            position:relative;
            left:50px;
        }
        .divstyle {
             position:relative;
            left:200px;
        }
        hr {
          position:relative;
          left:-50px;
          
        }
      
    </style>  
    <script type="text/javascript">
        function printReport() {
            $(".printReport").print();
        }
    </script>
    <asp:Label runat="server" ID="lblError"></asp:Label>
      <center><input type="button" value="Print" onclick="printReport()" /></center> 
      <center><h1>Consolidated Job Card</h1></center> 
    <div id="joborder" runat="server" class="printReport">
    </div>
  


 
 
</asp:Content>
