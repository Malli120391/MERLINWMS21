<%@ Page Title="" Language="C#" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="WarehouseOccupancyReport.aspx.cs" Inherits="MRLWMSC21.mReports.WarehouseOccupancyReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
 
    <script src="../mInventory/Scripts/angular.min.js"></script>
    <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />
     <script src="../mInventory/Scripts/dirPagination.js"></script>

    <script src="WarehouseOccupancyReport.js"></script>

    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
   
    <div class="dashed"></div>
<div class="pagewidth">

     <div ng-app="myApp" ng-controller="WarehouseOccupancyReport">
         <div class="divlineheight"></div>
         <div style="padding:10px;text-align:center;">
            <table class="Headertablewidth" style="width: 99%;">
                <tr>
                    <td align="right">
                       <input type="text" id="txtWH" placeholder="Warehouse"   ng-click="getWarehouse()"   class="TextboxInventoryAuto" />&nbsp;&nbsp;
                        <input type="text" id="txtDate" placeholder="Date" class="TextboxOutbound" datepicker />&nbsp;&nbsp;
                       
                             <button type="button" ng-click="Getdetails()"  style="width: 100px !important; background-color: #455b7c;" class="addbuttonOutbound">View Report <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                            </td>
                         
                </tr>
                
            </table>   
                
             <p></p>
             <table class="mytableOutbound" id="tbldatas" >
                    <thead>
                        <tr class="mytableReportItemsHeaderTR">
                            <th  colspan="15" class="thalign">
                                <table class="Headertablewidth">
                                     <tr>
                                        <td>
                                            <div class="buttonrowstyle">
                                                <button type="button" id="btnPdf" class="button button3" ng-click="exportPdf()">PDF &nbsp;<i class="fa fa-file-pdf-o" aria-hidden="true"></i></button>
                                                <button type="button" id="btnExcel" class="button button1" ng-click="exportExcel()">Excel &nbsp;<i class="fa fa-file-excel-o" aria-hidden="true"></i></button>
                                                <button type="button" id="btnTxt" class="button button4" ng-click="exportTxt()">Txt &nbsp;<i class="fa fa-file-text-o" aria-hidden="true"></i></button>
                                                <button type="button" id="btnWord" class="button button2" ng-click="exportWord()">Word &nbsp;<i class="fa fa-file-word-o" aria-hidden="true"></i></button>
                                                <button type="button" id="btnCsv" class="button button1" ng-click="exportCsv()">CSV &nbsp;<i class="fa fa-file-excel-o" aria-hidden="true"></i></button>
                                                <button type="button" id="btnXML" class="button button6" ng-click="exportXml()">XML &nbsp;<i class="fa fa-file-text-o" aria-hidden="true"></i></button>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </th>
                        </tr>
                        <tr class="mytableReportHeaderTR">
                                 <th>S No.</th>
                            <th ng-click="sort('Tenant')">Tenant Name<span class="glyphicon sort-icon" ng-show="sortKey=='TenantName'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('AvailableQty')">Available Qty.<span class="glyphicon sort-icon" ng-show="sortKey=='AvailableQty'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('TotalVolume')">Total Volume<span class="glyphicon sort-icon" ng-show="sortKey=='TotalVolume'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('Occupancy')">Occupancy<span class="glyphicon sort-icon" ng-show="sortKey=='Occupancy'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                         
                          
                        </tr>
                    </thead>
                    <tbody class="mytableReportBodyTR">
                        <tr dir-paginate="WH in WHOccupReport|orderBy:sortKey:reverse|filter:search|itemsPerPage:10">
                            
                            <td style="text-align:right;width:20px;">{{$index + 1}} </td>
                            <td style="text-align:left;width:60px;">{{WH.Tenant}}</td>
                            <td style="text-align:right;width:40px;">{{WH.AvailableQty}}</td>
                            <td style="text-align:right;width:40px;">{{WH.TotalVolume}}</td>
                            <td style="text-align:right;width:40px;">{{WH.Occupancy}}</td>
                        
                          
                        </tr>
                                               <tr>
                            <td colspan="2">Total :</td>
        <td style="text-align:right;width:40px;"> {{TotalAvailableQty}}</td>
        <td style="text-align:right;width:40px;"> {{TTTotalVolume}}</td>
        <td style="text-align:right;width:40px;"> {{TotalOccupancy}}</td>
    </tr>
                    </tbody>
                    <tfoot>
                        <tr class="mytableReportFooterTR">
                            <td colspan="15">
                                <div class="divpaginationstyle">
                                    <dir-pagination-controls direction-links="true" boundary-links="true"> </dir-pagination-controls>
                                </div>
                            </td>
                        </tr>
                    </tfoot>

                </table>
        
          <p></p>

             <table class="mytableOutbound" id="tbldatas1" style="width:80% !important;text-align:center !important;">
                    <thead>
                 
                        <tr class="mytableReportHeaderTR" >
                              
                            <th ng-click="sort('Tenant')">Total warehouse Volume(CBM)<span class="glyphicon sort-icon" ng-show="sortKey=='TenantName'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('AvailableQty')">Total Occupied Volume(CBM)<span class="glyphicon sort-icon" ng-show="sortKey=='AvailableQty'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('TotalVolume')">Total Available Volume(CBM)<span class="glyphicon sort-icon" ng-show="sortKey=='TotalVolume'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                          
                          
                        </tr>
                    </thead>
                    <tbody class="mytableReportBodyTR">
                        <tr ng-repeat="TL in TotalsList">
                                                 
                            <td style="text-align:right;width:40px;">{{TL.TotalWarehouseVolume}}</td>
                            <td style="text-align:right;width:40px;">{{TL.TotalOccupiedVolume}}</td>
                            <td style="text-align:right;width:40px;">{{TL.TotalAvailableVolume}}</td>
                        
                          
                        </tr>
 
                    </tbody>
          <%--          <tfoot>
                        <tr class="mytableReportFooterTR">
                            <td colspan="15">
                                <div class="divpaginationstyle">
                                    <dir-pagination-controls direction-links="true" boundary-links="true"> </dir-pagination-controls>
                                </div>
                            </td>
                        </tr>
                    </tfoot>--%>

                </table>         
             

                <table id="tbldata"></table>
              </div>
          </div>  
    </div>
</asp:Content>
