<%@ Page Language="C#"  MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="DemandForecastReportNew.aspx.cs" Inherits="MRLWMSC21.mReports.DemandForecastReportNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
<script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
    <script src="DemandForecastReportNew.js"></script>
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>

    <link href="Scripts/Custom.css" rel="stylesheet" />
    <style>
        .mytableReportHeaderTR {
            color: #000 !important;
            background-color: #fff !important;
            text-align: justify;
        }

        .module_Green {
            border-right: none !important;
            border-left: none !important;
        }
    </style>
     <div ng-app="myApp" ng-controller="DemandForecastReportNew" class="container" >

         <div class="row">
             <flex end><a class="btn btn-primary" ng-click="exportExcel()">Export To<i class="fa fa-file-excel-o" aria-hidden="true"></i></a></flex>
         </div>
        
          <div class="row" style="margin: 0;">
            <div class="col m12 s12" >
                <div class="" >
                <table class=" table-striped" id="tbldatas" >
                    <thead>
                        <tr class="mytableReportHeaderTR">
                          <%--  <th colspan="3">Inventory</th>--%>
                              <th colspan="3"> <%= GetGlobalResourceObject("Resource", "Inventory")%></th>
                         <%--   <th colspan="2">Replenishment</th>--%>
                               <th colspan="2"> <%= GetGlobalResourceObject("Resource", "Replenishment")%></th>
                         <%--   <th colspan="4">Demand Forecast</th>--%>
                               <th colspan="4"> <%= GetGlobalResourceObject("Resource", "DemandForecast")%> </th>
                        </tr>
                        <tr class="mytableReportHeaderTR">
                            <th ng-click="sort('Client')"> <%= GetGlobalResourceObject("Resource", "SrNo")%> <span class="glyphicon sort-icon" ng-show="sortKey=='Client'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('OBDNumber')"><%= GetGlobalResourceObject("Resource", "MaterialCode")%> <span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th number ng-click="sort('VLPDNumber')"><%= GetGlobalResourceObject("Resource", "AvailableQty")%> <span class="glyphicon sort-icon" ng-show="sortKey=='VLPDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('MType')"><%= GetGlobalResourceObject("Resource", "ReorderPoint")%> <span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('MType')"><%= GetGlobalResourceObject("Resource", "PlannedDeliveryTime")%> <span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('MType')"> {{Month1}}  <span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('MType')"> {{Month2}} <span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('ItemVolume')"> {{Month3}} <span class="glyphicon sort-icon" ng-show="sortKey=='ItemVolume'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('ItemWeight')"> {{Month4}}  <span class="glyphicon sort-icon" ng-show="sortKey=='ItemWeight'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                        </tr>
                    </thead>
                    <tbody class="mytableReportBodyTR">
                        <tr dir-paginate="BLR in BIllingReport|orderBy:sortKey:reverse|filter:search|itemsPerPage:25">
                            <td align="center">{{$index +1}}</td>
                            <td >{{BLR.MaterialCode}}</td>
                            <td number>{{BLR.AvailableQty}}</td>
                            <td >{{BLR.ReorderPoint}}</td>
                            <td >{{BLR.PlannedDeliveryTime}}</td>
                            <td >{{BLR.M1}}</td>
                            <td >{{BLR.M2}}</td>
                            <td>{{BLR.M3}}</td>
                            <td>{{BLR.M4}}</td>
                            
                          
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

                </table></div>

                <table id="tbldata"></table>
                <div class="divlineheight"></div>
            </div>
        </div>     
     </div>



</asp:Content>
