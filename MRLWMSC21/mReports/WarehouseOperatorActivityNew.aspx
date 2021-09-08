<%@ Page Title="Warehouse Operator Activity" Language="C#" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="WarehouseOperatorActivityNew.aspx.cs" Inherits="MRLWMSC21.mReports.WarehouseOperatorActivityNew" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#txtFromdate").datepicker({
                dateFormat: "dd/mm/yy",
                maxDate: new Date(),
                onSelect: function (selected) {
                    $(this).focus();
                    $("#txttodate").datepicker("option", "minDate", selected, { dateFormate: "dd/mm/yy" })
                }
            });
            $("#txttodate").datepicker({
                dateFormat: "dd/mm/yy",
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
    <link href="Scripts/Custom.css" rel="stylesheet" />
    <script src="WarehouseOperatorActivityNew.js"></script>
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>

    <div ng-app="myApp" ng-controller="WarehouseOperatorActivityNew" class="container">
        <div class="divlineheight"></div>
        <div class="row">
            <div class="">
                <div class="">
                    <div class="col m2">
                        <div class="flex">
                            <input type="text" id="txtWarehouse" required="" />
                            <label><%= GetGlobalResourceObject("Resource", "WareHouse")%></label>
                            <span class="errorMsg"></span>
                        </div>
                    </div>


                    <div class="col m2">
                        <div class="flex">
                            <input type="text" id="txtTenant" required="" />
                            <label><%= GetGlobalResourceObject("Resource", "Tenant")%></label>
                           <%-- <span class="errorMsg"></span>--%>
                        </div>
                    </div>
                     
                            <div class="col m2">
                        <div class="flex">
                            <input type="text" id="txtoperator" required="" />
                            <label><%= GetGlobalResourceObject("Resource", "Operator")%></label>
                        </div>
                    </div>
                    <div class="col m2">
                        <div class="flex">
                            <input type="text" required="" ng-model="fromdate" id="txtFromdate" />
                            <label><%= GetGlobalResourceObject("Resource", "FromDate")%></label>
                        </div>
                    </div>
                    <div class="col m2">
                        <div class="flex">
                            <input type="text" required="" ng-model="todate" id="txttodate" />
                            <label><%= GetGlobalResourceObject("Resource", "ToDate")%> </label>
                        </div>
                    </div>
                    <div class="col m12">
                        <gap5></gap5>
                        <flex end><button type="button" ng-click="Getgedetails()" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Search")%> <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                        <a class="btn btn-primary" ng-click="exportExcel()"><%= GetGlobalResourceObject("Resource", "Export")%> <i class="fa fa-file-excel-o" aria-hidden="true"></i></a></flex>
                    </div>
                </div>
            </div>
        </div>
        <div class="row" style="margin: 0;">
            <div class="col-sm-6 col-lg-6" style="margin: 0; padding: 0;">
                <div class="divmainwidth">
                    <table class="table-striped" id="tbldatas">
                        <thead>
                            <tr class="mytableReportHeaderTR">
                                <th ng-click="sort('Operator')"> <%= GetGlobalResourceObject("Resource", "Operator")%><span class="glyphicon sort-icon" ng-show="sortKey=='Operator'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('RoleAssigned')"><%= GetGlobalResourceObject("Resource", "RoleAssigned")%><span class="glyphicon sort-icon" ng-show="sortKey=='RoleAssigned'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('Warehouse')"><%= GetGlobalResourceObject("Resource", "Warehouse")%> <span class="glyphicon sort-icon" ng-show="sortKey=='Warehouse'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('TotalLineItems')"><%= GetGlobalResourceObject("Resource", "TotalLineItems")%> <span class="glyphicon sort-icon" ng-show="sortKey=='TotalLineItems'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th  ng-click="sort('TotalQuantity')"><%= GetGlobalResourceObject("Resource", "TotalQuantity")%> <span class="glyphicon sort-icon" ng-show="sortKey=='TotalQuantity'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('GoodsInLineItems')"> <%= GetGlobalResourceObject("Resource", "GoodsInLineItems")%><span class="glyphicon sort-icon" ng-show="sortKey=='GoodsInLineItems'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th  ng-click="sort('GoodsInQuantity')"><%= GetGlobalResourceObject("Resource", "GoodsInQuantity")%> <span class="glyphicon sort-icon" ng-show="sortKey=='GoodsInQuantity'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('GoodsOutLineItems')"><%= GetGlobalResourceObject("Resource", "GoodsOutLineItems")%>  <span class="glyphicon sort-icon" ng-show="sortKey=='GoodsOutLineItems'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('GoodsOutQuantity')"><%= GetGlobalResourceObject("Resource", "GoodsOutQuantity")%>  <span class="glyphicon sort-icon" ng-show="sortKey=='GoodsOutQuantity'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            </tr>
                        </thead>
                        <tbody class="mytableReportBodyTR">
                            <tr dir-paginate="BLR in BIllingReport|orderBy:sortKey:reverse|filter:search|itemsPerPage:25">
                                <td>{{BLR.Operator}}</td>
                                <td>{{BLR.RoleAssigned}}</td>
                                <td>{{BLR.Warehouse}}</td>
                                <td>{{BLR.TotalLineItems}}</td>
                                <td>{{BLR.TotalQuantity}}</td>
                                <td>{{BLR.GoodsInLineItems}}</td>
                                <td>{{BLR.GoodsInQuantity}}</td>
                                <td>{{BLR.GoodsOutLineItems}}</td>
                                <td>{{BLR.GoodsOutQuantity}}</td>
                                <%--<td  align="right">{{BLR.TotalCostAfterDiscWithOutTax}}</td>
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
