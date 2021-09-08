<%@ Page Title="Inbound List" Language="C#" AutoEventWireup="true"  MasterPageFile="~/Site.Master" CodeBehind="InboundList.aspx.cs" Inherits="MRLWMSC21.mInbound.InboundList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
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

        var myApp = angular.module('InboundList',['angularUtils.directives.dirPagination']);
        myApp.controller('InboundListController', function ($scope, $http,$timeout) {
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

            
            //********** Store Ref No**********//

            debugger;
            $('#txtStoreRefNo').val("");
            var textfieldname = $("#txtStoreRefNo");
            DropdownFunction(textfieldname);
            $("#txtStoreRefNo").autocomplete({
                source: function (request, response) {                   
                    if (WHID == 0 || WHID == "0" || WHID == undefined || WHID == null) {
                        showStickyToast(false, 'Please select WareHouse');
                        return false;
                    }
                    $.ajax({
                        url: '../mWebServices/FalconWebService.asmx/LoadStoreRefNos',
                        data: "{ 'prefix': '" + request.term + "'}",
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
                    RefStoreID = i.item.val;
                    RefStoreNo = i.item.label;

                },
                minLength: 0
            });


            //**********   FROM AND TO DATE  **********//

            debugger;
            $("#txtFromDate").datepicker({
                dateFormat: "dd-M-yy",
                maxDate: new Date(),
                onSelect: function (selected) {
                    $("#txtToDate").datepicker("option", "minDate", selected, { dateFormat: "dd-M-yy" })
                }
            });
            $("#txtToDate").datepicker({
                dateFormat: "dd-M-yy",
                maxDate: new Date()
            });
            $("#txtDueDate").datepicker({
                dateFormat: "dd-M-yy",
                maxDate: new Date(),
                onSelect: function (selected) {
                    $("#txtToDate").datepicker("option", "minDate", selected, { dateFormat: "dd-M-yy" })
                }
            });


            //**********   To get data of INBOUND List   **********//


            $scope.getSearchData = function (PaginationId) {

                debugger;
              

                if ($("#txtWarehouse").val() == undefined || $("#txtWarehouse").val() == null || $("#txtWarehouse").val() == "" || $("#txtWarehouse").val() == 0) {

                    showStickyToast(false, 'Please Select WareHouse', false);
                    return;
                }

                if ($("#txttenant").val() == undefined || $("#txttenant").val() == null || $("#txttenant").val() == "" || $("#txttenant").val() == 0) {

                    RefTenant = 0;
                }

                if ($("#txtStoreRefNo").val() == undefined || $("#txtStoreRefNo").val() == null || $("#txtStoreRefNo").val() == "" || $("#txtStoreRefNo").val() == 0) {

                    RefStoreID = 0;
                }

                if (PaginationId == undefined || PaginationId == "") {
                    PaginationId = 1;
                }
                var pagesize = $scope.noofrecords;

                $scope.blockUI = true;
                var accounts = {
                    method: 'POST',
                    url: 'InboundList.aspx/getInboundData',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: {  'TenantID': RefTenant,'WareHouseID': WHID, 'StoreRefNo':RefStoreID ,'PaginationId': PaginationId, 'PageSize': pagesize }
                }
                $http(accounts).success(function (response) {
                    debugger;
                    var dt = JSON.parse(response.d);
                    $scope.InboundList = dt.Table1;
                    if ($scope.InboundList == undefined || $scope.InboundList == null || $scope.InboundList.length == 0) {
                        $scope.blockUI = false;
                        showStickyToast(false, "No Data found for given search crieteria", false);
                        return false;
                    }
                    if ($scope.InboundList.length != 0) {
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
            $scope.Edit = function (InboundID) {
                debugger

                window.location.href = "DispatchList.aspx?inbid=" + InboundID;

            };
        });
    </script>

    <div ng-app="InboundList" ng-controller="InboundListController" class="container">

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
                            <input type="text" id="txtStoreRefNo" required="" />
                            <label>Store Ref No</label>
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
                                
                                
                            <a href="InboundPalletReceipt.aspx">
                                <button type="button" class="btn btn-primary">Add <%=MRLWMSC21Common.CommonLogic.btnfaNew %></button>
                            </a>                     
                        </div>
                    </div>
                </div>
            </div>
            <div ng-if="InboundList!=undefined && InboundList!=null && InboundList.length!=0">
                <table class="table-striped">
                    <thead>
                        <tr>
                            <th>S.No</th>
                            <th>Store Ref No.</th>
                            <th>Tenant</th>
                            <th>warehouse</th>                            
                            <th>Document Recv Date</th>                          
                            <th>Action</th>
                        </tr>
                    </thead>
                    <tbody>
                      
                        <tr dir-paginate="Obj in InboundList|itemsPerPage:25" pagination-id="main" total-items="Totalrecords"> 
                            <td>{{$index+1}}</td>
                            <td>{{Obj.}}</td>
                            <td>{{Obj.}}</td>                           
                            <td>{{Obj.}}</td>
                            <td>{{Obj. | date:'dd-MMM-yyyy'}}</td>                          
                            <td><a target="_blank" ng-click="Edit(Obj.InboundID)"><i class="material-icons ss Edit">mode_edit</i><em class="sugg-tooltis">Edit</em></a></td>
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
