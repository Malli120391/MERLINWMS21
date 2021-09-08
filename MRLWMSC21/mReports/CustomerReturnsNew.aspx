<%@ Page Title="Customer Returns Report" Language="C#" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="CustomerReturnsNew.aspx.cs" Inherits="MRLWMSC21.mReports.CustomerReturnsNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#txtFromdate").datepicker({
                dateFormat: "dd-M-yy" ,
                maxDate: new Date(),
                onSelect: function (selected) {
                    $(this).focus();
                    $("#txttodate").datepicker("option", "minDate", selected, { dateFormate: "dd/mm/yy" })
                }
            });
            $("#txttodate").datepicker({
                dateFormat: "dd-M-yy" ,
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
    <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
    <%--<script src="3PLBillingReportNew.js"></script>--%>
    <link href="Scripts/Custom.css" rel="stylesheet" />
    <script src="CustomerReturnsNew.js"></script>
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>

    <div ng-app="myApp" ng-controller="CustomerReturnsNew" class="container">

        <div class="row">
            <div class="">
                <div class="">
                     <div class="col m3">
                        <div class="flex">
                            <input type="text" id="txtWarehouse" required="" />
                            <label><%= GetGlobalResourceObject("Resource", "WareHouse")%></label>
                            <span class="errorMsg"></span>
                        </div>
                    </div>

                    <div class="col m3">
                        <div class="flex">
                            <input type="text" id="txtTenant" required="" />
                            <label><%= GetGlobalResourceObject("Resource", "Tenant")%></label>
                           <%-- <span class="errorMsg"></span>--%>
                        </div>
                    </div>
                    
                    <div class="col m3">
                        <div class="flex">
                            <input type="text" required="" ng-model="fromdate" id="txtFromdate" />
                            <label><%= GetGlobalResourceObject("Resource", "FromDate")%></label>
                        </div>
                    </div>
                    <div class="col m3">
                        <div class="flex">
                            <input type="text" required="" ng-model="todate" id="txttodate" />
                            <label><%= GetGlobalResourceObject("Resource", "ToDate")%> </label>
                        </div>
                    </div>
                    <div class="col m12">
                        <gap5></gap5>
                        <flex end><button type="button" ng-click="Getgedetails()" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Search")%><%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                        <button type="button" ng-click="exportExcel()" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Export")%><i class="fa fa-file-excel-o" aria-hidden="true"></i></button></flex>

                    </div>
                </div>
            </div>
        </div>
        <div class="row" style="margin: 0;">
            <div class="col-sm-6 col-lg-6" style="margin: 0; padding: 0;">
                <div class="divmainwidth">
                    <table class=" table-striped" id="tbldatas">
                        <thead>
                            <tr class="mytableReportHeaderTR">
                                <th ng-click="sort('Client')"> <%= GetGlobalResourceObject("Resource", "SNo")%><span class="glyphicon sort-icon" ng-show="sortKey=='Client'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('MType')"><%= GetGlobalResourceObject("Resource", "Customer")%><span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('MType')"><%= GetGlobalResourceObject("Resource", "PartNumber")%> <span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('ItemVolume')"><%= GetGlobalResourceObject("Resource", "SONumber")%> <span class="glyphicon sort-icon" ng-show="sortKey=='ItemVolume'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('ItemWeight')"><%= GetGlobalResourceObject("Resource", "ReturnDate")%> <span class="glyphicon sort-icon" ng-show="sortKey=='ItemWeight'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th number ng-click="sort('ItemWeight')"><%= GetGlobalResourceObject("Resource", "ReturnQty")%> <span class="glyphicon sort-icon" ng-show="sortKey=='ItemWeight'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th number ng-click="sort('ItemWeight')"><%= GetGlobalResourceObject("Resource", "UoMQty")%> <span class="glyphicon sort-icon" ng-show="sortKey=='ItemWeight'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            </tr>
                        </thead>
                        <tbody class="mytableReportBodyTR">
                            <tr dir-paginate="BLR in BIllingReport|orderBy:sortKey:reverse|filter:search|itemsPerPage:25">
                                <td align="center">{{$index +1}}</td>
                                <td>{{BLR.Customer}}</td>
                                <td>{{BLR.PartNumber}}</td>
                                <td>{{BLR.SONumber}}</td>
                                <td align="right">{{BLR.ReturnDate}}</td>
                                <td number align="right">{{BLR.ReturnQty}}</td>
                                <td number align="right">{{BLR.UoM}}</td>
                                <%-- <td  align="right">{{BLR.TotalCostAfterDisc}}</td>
                            <td  align="right">{{BLR.TotalCostAfterDiscWithOutTax}}</td>
                            <td  align="right">{{BLR.Tax}}</td>--%>
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
    </div>
</asp:Content>
