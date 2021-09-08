<%@ Page Title="Outbound List" Language="C#" AutoEventWireup="true" MasterPageFile="~/mOutbound/OutboundMaster.master" CodeBehind="OutboundList.aspx.cs" Inherits="MRLWMSC21.mOutbound.OutboundList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="OBContent" runat="server">
    <script type="text/javascript" src="Scripts/CommonScripts.js"></script>
    <script type="text/javascript" src="../Scripts/timeentry/jquery.timeentry.js"></script>

    <link href="../mInventory/Scripts/bootstrap.min.css" rel="stylesheet" />
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>

    <script src="../Scripts/angular.min.js"></script>
    <script src="../mReports/Scripts/dirPagination.js"></script>
    <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />
    <script src="NewInboundSearch.js"></script>


    <script src="../Scripts/xlsx.full.min.js"></script>
    <script src="../Scripts/xlsx-model.js"></script>
    <link href="../Scripts/mdtimepicker.css" rel="stylesheet" />
    <script src="../Scripts/mdtimepicker.js"></script>
    <script src="Scripts/InventraxAjax.js"></script>

    <script src="../mInventory/CycleCountScripts/jquery.dataTables.min.js"></script>
    <script src="../mInventory/CycleCountScripts/dataTables.bootstrap.min.js"></script>

    <script src="../Scripts/angular.min.js"></script>
    <style>
        .listItems div {
            width: 25px;
        }

        .listItems {
            display: flex;
        }
    </style>
    <script>
        $(document).ready(function () {
            debugger


            $('#txtFromDate').datepicker({
                dateFormat: "dd-M-yy",
                maxDate: new Date(),
                onSelect: function (selected) {
                    $("#txtFromDate").datepicker("option", "minDate", selected, { dateFormat: "dd-M-yy" })
                }
            });

            $('#txtToDate').datepicker({
                dateFormat: "dd-M-yy",
                maxDate: new Date(),
                onSelect: function (selected) {
                    $("#txtToDate").datepicker("option", "minDate", selected, { dateFormat: "dd-M-yy" })
                }
            });
         
        });
        var myApp = angular.module('OutboundList', ['angularUtils.directives.dirPagination']);
        myApp.controller('OutboundListController', function ($scope, $http, $timeout) {
            var WHID = '';
            $scope.noofrecords = 25;
            $scope.Totalrecords = 0;
            //********** WARE HOUSE **********//

            $("#txtWarehouse").val("");
            var textfieldname = $("#txtWarehouse");
            DropdownFunction(textfieldname);
            $("#txtWarehouse").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '../mWebServices/FalconWebService.asmx/LoaDWHListBasedonUser',
                        data: "{ 'prefix': '" + request.term + "'  }",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split(',')[0],
                                    val: item.split(',')[1]
                                }
                            }))
                        }
                    });
                },
                select: function (e, i) {
                    WHID = i.item.val;
                },
                minLength: 0
            });

            //********** TENANT**********//

            debugger;
            $('#txttenant').val("");
            var textfieldname = $("#txttenant");
            DropdownFunction(textfieldname);
            $("#txttenant").autocomplete({
                source: function (request, response) {
                    if ($("#txttenant").val() == '') {
                        RefTenant = 0;
                    }
                    if (WHID == 0 || WHID == "0" || WHID == undefined || WHID == null) {
                        showStickyToast(false, 'Please select WareHouse');
                        return false;
                    }
                    $.ajax({
                        url: '../mWebServices/FalconWebService.asmx/LoadTenantsByWH',
                        data: "{ 'prefix': '" + request.term + "','whid':'" + WHID + "' }",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split(',')[0],
                                    val: item.split(',')[1]
                                }
                            }))
                        }
                    });
                },
                select: function (e, i) {
                    debugger
                    RefTenant = i.item.val;

                },
                minLength: 0
            });

            //**********    DELV. DOC NO**********//

            $('#txtOBDNumber').val("");
            var textfieldname = $("#txtOBDNumber");
            DropdownFunction(textfieldname);
            $("#txtOBDNumber").autocomplete({
                source: function (request, response) {
                    if (RefTenant == 0 || RefTenant == "0" || RefTenant == undefined || RefTenant == null) {
                        showStickyToast(false, 'Please select Tenant');
                        return false;
                    }
                    $.ajax({
                        url: '../mWebServices/FalconWebService.asmx/LoadOBDNumbers',
                        data: "{ 'prefix': '" + request.term + "','TenantId':" + RefTenant + "}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            debugger;
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split(',')[0],
                                    val: item.split(',')[1]
                                }
                            }))

                            if (data.d.length == 0) {
                                showStickyToast(false, "There are no OBD's for the selected tenant", false);
                                return false;
                            }
                        }
                    });
                },
                select: function (e, i) {
                    Refobd = i.item.val;
                },
                minLength: 0
            });

            //********** INDENT NUMBER **********//

            $('#txtIndentNo').val("");
            var textfieldname = $("#txtIndentNo");
            DropdownFunction(textfieldname);
            $("#txtIndentNo").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '../mWebServices/FalconWebService.asmx/LoadIndentNumber',
                        data: "{ 'prefix': '" + request.term + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            debugger;
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split(',')[0],
                                    val: item.split(',')[1]
                                }
                            }))
                        }

                    });
                },
                select: function (e, i) {

                    IndentNo = i.item.label;
                },
                minLength: 0
            });


            //**********   To get data of Outbound List   **********//


            $scope.getSearchData = function (PaginationId) {

                debugger;


                if ($("#txtWarehouse").val() == undefined || $("#txtWarehouse").val() == null || $("#txtWarehouse").val() == "" || $("#txtWarehouse").val() == 0) {

                    showStickyToast(false, 'Please Select WareHouse', false);
                    return;
                }

                if ($("#txttenant").val() == undefined || $("#txttenant").val() == null || $("#txttenant").val() == "" || $("#txttenant").val() == 0) {

                    RefTenant = 0;
                }
                if ($("#txtIndentNo").val() == undefined || $("#txtIndentNo").val() == null || $("#txtIndentNo").val() == "" || $("#txtIndentNo").val() == 0) {

                    IndentNo = "";
                }
                if ($("#txtOBDNumber").val() == undefined || $("#txtOBDNumber").val() == null || $("#txtOBDNumber").val() == "" || $("#txtOBDNumber").val() == 0) {

                    Refobd = 0;
                }
                if (PaginationId == undefined || PaginationId == "") {
                    PaginationId = 1;
                }
                var pagesize = $scope.noofrecords;
                var Startate = $("#txtFromDate").val();
                var EndDate = $("#txtToDate").val();

                $scope.blockUI = true;
                var accounts = {
                    method: 'POST',
                    url: 'OutboundList.aspx/getOutboundData',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'StartDate': StartDate, 'EndDate': EndDate, 'OutboundID': Refobd, 'TenantID': RefTenant, 'CustomerPONumber': IndentNo, 'WareHouseID': WHID, 'PaginationId': PaginationId, 'PageSize': pagesize }
                }
                $http(accounts).success(function (response) {
                    debugger;
                    var dt = JSON.parse(response.d);
                    $scope.OutboundList = dt.Table1;
                    if ($scope.OutboundList == undefined || $scope.OutboundList == null || $scope.OutboundList.length == 0) {
                        $scope.blockUI = false;
                        showStickyToast(false, "No Data found for given search crieteria", false);
                        return false;
                    }
                    if ($scope.OutboundList.length != 0) {
                        $scope.Totalrecords = dt.Table[0].NoofColumns;
                        $scope.blockUI = false;

                    }
                    else {
                        $scope.Totalrecords = 0;
                        showStickyToast(false, 'No Data Found', false);
                        $scope.blockUI = false;
                    }
                });


            }
            $scope.Edit = function (OutboundID) {
                debugger

                window.location.href = "DispatchList.aspx?obdid=" + OutboundID;

            };
        });
    </script>

    <div ng-app="OutboundList" ng-controller="OutboundListController" class="container">

        <div id="loader" hidden>
            <div style="width: 100%; height: 100vh; z-index: 999; position: fixed; top: 0; left: 0; right: 0; bottom: 0; align-items: center; display: flex; justify-content: center;">
                <div style="align-self: center;">
                    <div class="spinner">
                        <div class="bounce1"></div>
                        <div class="bounce2"></div>
                        <div class="bounce3"></div>
                    </div>
                </div>
            </div>
        </div>

        <div class="">
            <div class="">
                <div class="row">
                    <div class="col m2">
                        <div class="flex">
                            <input type="text" id="txtWarehouse" required="" />
                            <label>Ware house</label>
                        </div>
                    </div>
                    <div class="col m2">
                        <div class="flex">
                            <input type="text" id="txttenant" required="" />
                            <label>tenant</label>
                        </div>
                    </div>
                    <div class="col m2">
                        <div class="flex">
                            <input type="text" id="txtOBDNumber" required="" />
                            <label>Delv.Doc No</label>
                        </div>
                    </div>


                    <div class="col m2">
                        <div class="flex">
                            <input type="text" id="txtIndentNo" required="" />
                            <label>Indent Number</label>
                        </div>
                    </div>

                    <div class="col m2">
                        <div class="flex">
                            <input type="text" id="txtFromDate" required="" />
                            <label>From Date</label>
                        </div>
                    </div>
                    <div class="col m2">
                        <div class="flex">
                            <input type="text" id="txtToDate" required="" />
                            <label>To Date</label>
                        </div>
                    </div>

                    <br />

                    <div class="col m12">
                        <div class="flex__ end">
                            <button type="button" ng-click="getSearchData(1)" class="btn btn-primary">Search <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>&nbsp;
                                
                            <a href="DispatchList.aspx">
                                <button type="button" class="btn btn-primary">Add <%=MRLWMSC21Common.CommonLogic.btnfaNew %></button>
                            </a>
                        </div>
                    </div>
                </div>
            </div>
            <div ng-if="OutboundList!=undefined && OutboundList!=null && OutboundList.length!=0">
                <table class="table-striped">
                    <thead>
                        <tr>
                            <th>S.No</th>
                            <th>Delv.Document No.</th>
                            <th>Tenant</th>
                            <th>warehouse</th>
                            <th>Indent Number</th>
                            <th>Delv.Document Date</th>
                            <th>Customer Name</th>
                            <th>Action</th>
                        </tr>
                    </thead>
                    <tbody>

                        <tr dir-paginate="Obj in OutboundList|itemsPerPage:25" pagination-id="main" total-items="Totalrecords">
                            <td>{{$index+1}}</td>
                            <td>{{Obj.DelDocNo}}</td>
                            <td>{{Obj.Tenant}}</td>
                            <td>{{Obj.WareHouse}}</td>
                            <td>{{Obj.IndenNumber}}</td>
                            <td>{{Obj.DelDocDate | date:'dd-MMM-yyyy'}}</td>
                            <td>{{Obj.CustomerName}}</td>
                            <td><a target="_blank" ng-click="Edit(Obj.OutboundID)"><i class="material-icons ss Edit">mode_edit</i><em class="sugg-tooltis">Edit</em></a></td>
                        </tr>
                    </tbody>
                </table>
                <div style="float: right !important; font-family: Arial; font-size: small; margin-right: 1%;">
                    <dir-pagination-controls direction-links="true" boundary-links="true" pagination-id="main" on-page-change="getSearchData(newPageNumber)"> </dir-pagination-controls>

                </div>
            </div>
        </div>
    </div>
</asp:Content>
