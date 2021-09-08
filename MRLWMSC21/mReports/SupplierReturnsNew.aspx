<%@ Page Title="Supplier Returns Report" Language="C#" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="SupplierReturnsNew.aspx.cs" Inherits="MRLWMSC21.mReports.SupplierReturnsNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#txtFromdate").datepicker({
                dateFormat: "dd-M-yy",
                maxDate: new Date(),
                onSelect: function (selected) {
                    $(this).focus();
                    $("#txttodate").datepicker("option", "minDate", selected, { dateFormate: "dd-mm-yy" })
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
    <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
    <%--<script src="3PLBillingReportNew.js"></script>--%>
    <link href="Scripts/Custom.css" rel="stylesheet" />
    <script src="SupplierReturnsNew.js"></script>
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
  
    <div ng-app="myApp" ng-controller="SupplierReturnsNew" class="container">
        <div class="divlineheight"></div>
        <div class="row">
      
            <div class="">
                <div class=" ">
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
                      <%--      <span class="errorMsg"></span>--%>
                        </div>
                    </div>
                    
                    <div class="col m3">
                        <div class="flex">
                            <input type="text" required="" onpaste="return false" ng-model="fromdate" id="txtFromdate" />
                              <label> <%= GetGlobalResourceObject("Resource", "FromDate")%></label>
                        </div>
                    </div>
                    <div class="col m3">
                        <div class="flex">
                            <input type="text" required="" onpaste="return false" ng-model="todate" id="txttodate" />
                            <label><%= GetGlobalResourceObject("Resource", "ToDate")%> </label>
                        </div>
                    </div>
                    <div class="col m12">
                        <gap5></gap5>
                        <flex end><button type="button" ng-click="Getgedetails()" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Search")%>  <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                        <abutton type="button" ng-click="exportExcel()" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Export")%> <i class="fa fa-file-excel-o" aria-hidden="true"></i></abutton></flex>
                    </div>
                           
                </div>
            </div>
        </div>
        <div class="row" style="margin: 0;">
            <div class="">
                <div class="">
                    <table class="table-striped" id="tbldatas">
                        <thead>
                            <tr class="">
                                <th sno ng-click="sort('Client')"><%= GetGlobalResourceObject("Resource", "SNo")%><span class="glyphicon sort-icon" ng-show="sortKey=='Client'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('MType')"><%= GetGlobalResourceObject("Resource", "Supplier")%><span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('MType')"><%= GetGlobalResourceObject("Resource", "Tenant")%><span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('MType')"><%= GetGlobalResourceObject("Resource", "PartNumber")%> <span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('ItemVolume')"><%= GetGlobalResourceObject("Resource", "InvoiceNo")%> <span class="glyphicon sort-icon" ng-show="sortKey=='ItemVolume'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('ItemWeight')"><%= GetGlobalResourceObject("Resource", "ReturnDate")%> <span class="glyphicon sort-icon" ng-show="sortKey=='ItemWeight'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th number ng-click="sort('ItemWeight')"><%= GetGlobalResourceObject("Resource", "ReturnQty")%> <span class="glyphicon sort-icon" ng-show="sortKey=='ItemWeight'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th number ng-click="sort('ItemWeight')"><%= GetGlobalResourceObject("Resource", "UoMQty")%><span class="glyphicon sort-icon" ng-show="sortKey=='ItemWeight'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            </tr>
                        </thead>
                        <tbody class="">
                            <tr dir-paginate="BLR in BIllingReport|orderBy:sortKey:reverse|filter:search|itemsPerPage:25">
                                <td sno>{{$index +1}}</td>
                                <td>{{BLR.Supplier}}</td>
                                <td>{{BLR.Tenant}}</td>
                                <td>{{BLR.PartNumber}}</td>
                                <td>{{BLR.InvoiceNo}}</td>
                                <td align="right">{{BLR.ReturnDate}}</td>
                                <td number align="right">{{BLR.ReturnQty}}</td>
                                <td number align="right">{{BLR.UoM}}</td>
                                <%-- <td  align="right">{{BLR.TotalCostAfterDisc}}</td>
                            <td  align="right">{{BLR.TotalCostAfterDiscWithOutTax}}</td>
                            <td  align="right">{{BLR.Tax}}</td>--%>
                            </tr>
                        </tbody>
                        <tfoot>
                            <tr class="">
                                <td colspan="15">
                                    <div class="">
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
