<%@ Page Language="C#" Title="Material Traceability Report" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="MaterialTrackingReportNew.aspx.cs" Inherits="MRLWMSC21.mReports.MaterialTrackingReportNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
<script src="../Scripts/angular.min.js"></script>
    <script src="../mInventory/Scripts/dirPagination.js"></script>
    <script src="MaterialTrackingReportNew.js"></script>
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
    <link href="Scripts/Custom.css" rel="stylesheet" />

     <div ng-app="myApp" ng-controller="MaterialTrackingReportNew" class="container">
         <div class="divlineheight"></div>
         <div class="row">

              <div class="col m3">
                 <div class="flex">
                      <asp:TextBox runat="server" ID="txtWarehouse" ClientIDMode="Static" required=""></asp:TextBox>
                     <label>Warehouse</label>
                      <asp:HiddenField runat="server" ID="hdnWarehouse" ClientIDMode="Static" Value="0" />
                       <span class="errormsg"></span>
                 </div>
             </div>


             <div class="col m3">
                 <div class="flex">
                      <asp:TextBox runat="server" ID="txtTenant" ClientIDMode="Static" required=""></asp:TextBox>
                     <label>Tenant</label>
                     <asp:HiddenField runat="server" ID="hdnTenant" ClientIDMode="Static" Value="0" />
                       <span class="errormsg"></span>
                 </div>
             </div>

             

              <div class="col m3">
                 <div class="flex">
                      <asp:TextBox runat="server" ID="txtMaterial" ClientIDMode="Static" required=""></asp:TextBox>
                     <label>Material</label>
                     <asp:HiddenField runat="server" ID="hdnMaterial" ClientIDMode="Static" Value="0" />
                     <span class="errormsg"></span>
                 </div>
             </div>
             <div class="col m3">
                   <gap5></gap5>
                         <button type="button" ng-click="Getgedetails()" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Search")%> <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                                 <asp:LinkButton runat="server" ID="lnkExportData" CssClass="btn btn-primary" OnClick="lnkExportData_Click">Export <i class="fa fa-file-excel-o" aria-hidden="true"></i></asp:LinkButton>
             </div>

<%--              <div class="col m3">
                 <div class="flex">
                      <asp:TextBox runat="server" ID="txtserailno" ClientIDMode="Static" required=""></asp:TextBox>
                     <label>Serial No.</label>
                 </div>
             </div>--%>
              
             <%--<div class="col m3">
                 <div class="flex">
                     <input type="text" id="txtTenant" required="" />
                     <label><%= GetGlobalResourceObject("Resource", "Tenant")%></label>
                 </div>
             </div>
             <div class="col m3">
                 <div class="flex">
                     <input type="text" id="txtWarehouse" required="" />
                     <label><%= GetGlobalResourceObject("Resource", "WareHouse")%></label>
                 </div>
             </div>

              <div class="col m3">
                 <div class="flex">
                     <input type="text" id="txtMaterial" required="" />
                     <label>Material Code</label>
                 </div>
             </div>

             <div class="col m3">
                 <div class="flex">
                     <input type="text" id="txtserailno" required="" />
                     <label>Serial No.</label> <span class="errormsg"></span>
                 </div>
             </div>--%>
           
         </div>
         <div class="row" style="display:none;">
              <div class="col m3">
                 <div class="flex">
                      <asp:TextBox runat="server" ID="txtbatchno" ClientIDMode="Static" required=""></asp:TextBox>
                     <label>Batch No.</label> 
                 </div>
             </div>
             <div class="col m9">
                       
             </div>
         </div>
         <div class="row">
             <div class="col m12">

                 <%------------------- Removed Unnecessary Code from table by durga on 16/05/2018 ------------------------------------%>
                  <%------------------- Modified by M.D.Prasad on 16/12/2019 ------------------------------------%>

                     <div ng-if="BIllingReportIn!=undefined && BIllingReportIn!=null ">
                           <h3>Inbound Receipt</h3>
                         <table class="table-striped" id="tbldatas">
                             <thead>
                                 <tr class="mytableReportHeaderTR">
                                     <th>PO Number</th>
                                     <th>PO Date</th>
                                     <th>Invoice Number</th>
                                     <th>Invoice Date</th>
                                     <th>Tenant</th>
                                     <th>Supplier</th>
                                     <th>Store Ref. No.</th>
                                     <th>Part No.</th>
                                     <th>Received Qty. </th>
<%--                                     <th>S. Loc.</th>
                                     <th>Location</th>--%>
                                 </tr>
                             </thead>
                             <tbody class="mytableReportBodyTR">                                 
                                 <tr dir-paginate="BLR in BIllingReportIn|itemsPerPage:25" pagination-id="main1">

                                     <td>{{BLR.PONumber}}</td>
                                     <td>{{BLR.PODate}}</td>
                                     <td>{{BLR.InvoiceNumber}}</td>
                                     <td>{{BLR.InvoiceDate}}</td>
                                     <td>{{BLR.Tenant}}</td>
                                     <td>{{BLR.Supplier}}</td>
                                     <td>{{BLR.StoreRefNo}}</td>
                                     <td>{{BLR.PartNo}}</td>
                                     <td>{{BLR.ReceivedQty}}</td>
<%--                                     <td>{{BLR.sloc}}</td>
                                     <td>{{BLR.location}}</td>--%>
                                 </tr>
                                 <tr ng-show="BIllingReportIn.length==0">
                                     <td colspan="9">
                                         <div style="font-size: 13px !important; text-align: center;">No data Found. </div>
                                     </td>
                                 </tr>
                             </tbody>
                         </table>
                         <div style="float: right !important; font-family: Arial; font-size: small; margin-right: 1%;">
                             <dir-pagination-controls direction-links="true" pagination-id="main1" boundary-links="true"> </dir-pagination-controls>
                         </div>
                     </div>
                     <div ng-if="BIllingReportOut!=undefined && BIllingReportOut!=null ">

                            <h3>Outbound Delivery</h3>
                         <table class="table-striped">
                             <thead>
                                 <tr class="mytableReportHeaderTR">
                                     <th>SO Number</th>                                    
                                     <th>SO Date</th>                                    
                                     <th>Customer PO No.</th>                                    
                                     <th>Customer</th>   
                                     <th>OBD No.</th>
                                     <th> Part No.</th>                                   
                                     <th >Picked Qty. </th>                                    
                                 </tr>
                             </thead>
                             <tbody class="mytableReportBodyTR">
                                 <tr dir-paginate="BLR in BIllingReportOut|itemsPerPage:25" pagination-id="main">

                                     <td>{{BLR.SONumber}}</td>
                                     <td>{{BLR.SODate}}</td>
                                     <td>{{BLR.CustomerPoNo}}</td>
                                     <td>{{BLR.Customer}}</td>
                                     <td>{{BLR.OutboundNumber}}</td>
                                     <td>{{BLR.PartNo}}</td>
                                     <td>{{BLR.PickedQty}}</td>
                                 </tr>
                                 <tr ng-show="BIllingReportOut.length==0">
                                     <td colspan="7">
                                         <div style="font-size: 13px !important; text-align: center;">No data Found. </div>
                                     </td>
                                 </tr>
                             </tbody>

                         </table>
                         <div style="float: right !important; font-family: Arial; font-size: small; margin-right: 1%;">
                             <dir-pagination-controls direction-links="true" pagination-id="main" boundary-links="true"> </dir-pagination-controls>
                         </div>
                     </div>


                 <table id="tbldataIn" class="table"></table>
                 <div class="divlineheight"></div>
                 <table id="tbldataOut"></table>
             </div>
         </div>
     </div>



</asp:Content>