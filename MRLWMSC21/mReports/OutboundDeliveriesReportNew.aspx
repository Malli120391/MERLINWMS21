<%@ Page Language="C#" Title="Outbound Deliveries Report" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="OutboundDeliveriesReportNew.aspx.cs" Inherits="MRLWMSC21.mReports.OutboundDeliveriesReportNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#txtFromdate").datepicker({
                dateFormat: "dd-M-yy",
                maxDate: new Date(),
                onSelect: function (selected) {
                    $(this).focus();
                    $("#txttodate").datepicker("option", "minDate", selected, { dateFormate: "dd-M-yy" })
                }
            });
            $("#txttodate").datepicker({
                dateFormat: "dd-M-yy",
                maxDate: new Date(),
                onSelect: function (selected) {
                    $(this).focus();
                }
            });

            $('#txtFromdate, #txttodate').keypress(function () {
                return false;
            });
        });

    </script>

    <style>
      
    </style>


    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
    <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
    <link href="Scripts/Custom.css" rel="stylesheet" />
    <script src="OutboundDeliveriesReportNew.js"></script>
  
    <div ng-app="myApp" ng-controller="OutboundDeliveriesReportNew" class="pagewidth" >
        <div class="divlineheight"></div>
        <div class="">

            <div class="">
                <div class="row">
                    <div class="col m3">
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
                    <div class="col m3 s3">
                        <div class="flex">
                            <select required="required" ng-model="ddlDocumentType" ng-options="doc.DocumentTypeID as doc.Documenttype for doc in documents">
                            </select>
                            <label><%= GetGlobalResourceObject("Resource", "DocumentType")%></label>
                        </div>
                    </div>
                    <div class="col m3 s3">
                        <div class="flex">
                            <select ng-model="ddlDeliveryStatus" ng-options="delivery.DeliveryStatusID as delivery.Deliverystatus for delivery in deliveries">

                                <option value=""><%= GetGlobalResourceObject("Resource", "DeliveryStatus")%></option>
                            </select>
                        </div>
                    </div>
                    <div class="col m3 s3">
                        <div class="flex">
                            <input type="text" ng-model="fromdate" id="txtFromdate" required="" />
                            <label><%= GetGlobalResourceObject("Resource", "FromDate")%> </label>
                        </div>
                    </div>
                    <div class="col m3 s3">
                        <div class="flex">
                            <input type="text" ng-model="todate" id="txttodate" required="" />
                            <label><%= GetGlobalResourceObject("Resource", "ToDate")%> </label>
                        </div>
                    </div>
                    <div class="col m12 s12">
                       <flex end> <button type="button" ng-click="Getgedetails()" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "ViewReport")%> <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>                 
                        <a class="btn btn-primary" ng-click="exportExcel()"><%= GetGlobalResourceObject("Resource", "ExportTo")%><i class="fa fa-file-excel-o" aria-hidden="true"></i></a>
                           </flex>
                    </div>


                </div>



            </div>
        </div>
        <div class="row" style="margin: 0;">
            <div class="col-sm-6 col-lg-6" style="margin: 0; padding: 0;">
                <div class="divmainwidth">
                    <table class="table-striped" id="tbldatas">
                        <thead>
                            <tr class="">
                                <th ng-click="sort('Client')"><%= GetGlobalResourceObject("Resource", "DeliveryDocNo")%><span class="glyphicon sort-icon" ng-show="sortKey=='Client'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('OBDNumber')"><%= GetGlobalResourceObject("Resource", "DocumentType")%>  <span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('VLPDNumber')"><%= GetGlobalResourceObject("Resource", "DeliveryDocDate")%>  <span class="glyphicon sort-icon" ng-show="sortKey=='VLPDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('MType')"><%= GetGlobalResourceObject("Resource", "Customer")%> <span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('MType')"><%= GetGlobalResourceObject("Resource", "Stores")%><span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('MType')"><%= GetGlobalResourceObject("Resource", "PGIDateDoneBy")%> <span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('MType')"><%= GetGlobalResourceObject("Resource", "DeliveryDate")%> <span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('ItemVolume')"><%= GetGlobalResourceObject("Resource", "Status")%> <span class="glyphicon sort-icon" ng-show="sortKey=='ItemVolume'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('ItemWeight')"><%= GetGlobalResourceObject("Resource", "LineItems")%>  <span class="glyphicon sort-icon" ng-show="sortKey=='ItemWeight'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            </tr>
                        </thead>
                        <tbody class="">
                            <tr dir-paginate="BLR in BIllingReport|orderBy:sortKey:reverse|filter:search|itemsPerPage:25">
                                <td>{{BLR.DeliveryDocNo}}</td>
                                <td>{{BLR.DocumentType}}</td>
                                <td>{{BLR.DeliveryDocDate}}</td>
                                <td>{{BLR.Customer}}</td>
                                <td>{{BLR.Stores}}</td>
                                <td>{{BLR.PGI}}</td>
                                <td>{{BLR.DeliveryDate}}</td>
                                <td>{{BLR.Status}}</td>
                                <td>{{BLR.LineItems}}</td>
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
                </div>

                <table id="tbldata"></table>
                <div class="divlineheight"></div>
            </div>
        </div>
</asp:Content>
