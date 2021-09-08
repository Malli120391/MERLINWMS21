<%@ Page Title="IBSummary Report" Language="C#" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="IBSummaryReport.aspx.cs" Inherits="MRLWMSC21.mReports.IBSummaryReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <style>
        /* Absolute Center Spinner */
        .loading {
            position: fixed;
            z-index: 9999;
            height: 2em;
            width: 2em;
            overflow: show;
            margin: auto;
            top: 0;
            left: 0;
            bottom: 0;
            right: 0;
        }

            /* Transparent Overlay */
            .loading:before {
                content: '';
                display: block;
                position: fixed;
                top: 0;
                left: 0;
                width: 100%;
                height: 100%;
                background-color: rgba(0,0,0,0.3);
            }

            /* :not(:required) hides these rules from IE9 and below */
            .loading:not(:required) {
                /* hide "loading..." text */
                font: 0/0 a;
                color: transparent;
                text-shadow: none;
                background-color: transparent;
                border: 0;
            }

                .loading:not(:required):after {
                    content: '';
                    display: block;
                    font-size: 10px;
                    width: 1em;
                    height: 1em;
                    margin-top: -0.5em;
                    -webkit-animation: spinner 1500ms infinite linear;
                    -moz-animation: spinner 1500ms infinite linear;
                    -ms-animation: spinner 1500ms infinite linear;
                    -o-animation: spinner 1500ms infinite linear;
                    animation: spinner 1500ms infinite linear;
                    border-radius: 0.5em;
                    -webkit-box-shadow: rgba(0, 0, 0, 0.75) 1.5em 0 0 0, rgba(0, 0, 0, 0.75) 1.1em 1.1em 0 0, rgba(0, 0, 0, 0.75) 0 1.5em 0 0, rgba(0, 0, 0, 0.75) -1.1em 1.1em 0 0, rgba(0, 0, 0, 0.5) -1.5em 0 0 0, rgba(0, 0, 0, 0.5) -1.1em -1.1em 0 0, rgba(0, 0, 0, 0.75) 0 -1.5em 0 0, rgba(0, 0, 0, 0.75) 1.1em -1.1em 0 0;
                    box-shadow: rgba(0, 0, 0, 0.75) 1.5em 0 0 0, rgba(0, 0, 0, 0.75) 1.1em 1.1em 0 0, rgba(0, 0, 0, 0.75) 0 1.5em 0 0, rgba(0, 0, 0, 0.75) -1.1em 1.1em 0 0, rgba(0, 0, 0, 0.75) -1.5em 0 0 0, rgba(0, 0, 0, 0.75) -1.1em -1.1em 0 0, rgba(0, 0, 0, 0.75) 0 -1.5em 0 0, rgba(0, 0, 0, 0.75) 1.1em -1.1em 0 0;
                }

        /* Animation */

        @-webkit-keyframes spinner {
            0% {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        @-moz-keyframes spinner {
            0% {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        @-o-keyframes spinner {
            0% {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        @keyframes spinner {
            0% {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }
    </style>

         <script src="../Scripts/angular.min.js"></script>
         <script src="Scripts/dirPagination.js"></script>
 
        <script type="text/javascript">
            $(document).ready(function () {
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
        });

    </script>
    <script>
        var app = angular.module('IBApp', ['angularUtils.directives.dirPagination']);
        app.controller('IBSummary', function ($scope, $http, $timeout, $window, $filter) {
            debugger;
            $scope.search = new IBLOGSummarySearch(0, '', '', '',0,0,0);
             var textfieldname = $('#txtStoreRefNo');
            DropdownFunction(textfieldname);
            $('#txtStoreRefNo').autocomplete({
                source: function (request, response) {
                    debugger;
                    if (tenantid == "" || tenantid == null || tenantid == undefined) {
                        tenantid = 0;
                    }
                     if (Warehouseid == "" || Warehouseid == null || Warehouseid == undefined) {
                        Warehouseid = 0;
                    }

                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadStoreRefNumbers") %>',
                        data: "{ 'prefix': '" + request.term + "', 'TenantId': '"+tenantid+"' , 'WarehouseId': '"+Warehouseid+"'}",
                        dataType: "json",
                        type: "POST",
                        async: true,
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {

                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split(',')[0],
                                    val: item.split(',')[1]
                                }
                            }))
                            if (data.d.length == "0") {
                                showStickyToast(false, "No StoreRefNumbers found ")
                            }

                        }

                    });
                },
                select: function (e, i) {

                   $scope.search.inboundID= i.item.val;

                },
                minLength: 0
            });


            var tenantid; 
            var Warehouseid;
            //Tenant drop down
            
            $("#txtTenant").val("");
           
            var textfieldname = $("#txtTenant");
            debugger;
            DropdownFunction(textfieldname);
            $("#txtTenant").autocomplete({
                source: function (request, response) {
                    if ($("#txtTenant").val() || $("#txtTenant").val == null || $("#txtTenant").val() == undefined) {
                        $scope.search.tenantid = 0;
                        tenantid = 0;
                    }
                    $.ajax({
                     <%--   url: '../mWebServices/FalconWebService.asmx/LoadTenantsDataFor3PL',
                        data: "{ 'prefix': '" + request.term + "','Accountid': <%= cp.AccountID%>  }",--%>   
                        url: '../mWebServices/FalconWebService.asmx/LoadTenantsByWH',
                        data: "{ 'prefix': '" + request.term + "','whid':'"+$scope.search.Warehouseid+"' }",
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
                    debugger;
                    tenantid = i.item.val;
                    $("#txtTenant").val("");
                    $scope.search.tenantid = tenantid;
                    //  $("#txtWarehouse").val("");                    
                    $('#txtStoreRefNo').val("");
                    $('#txtFromDate').val("");
                    $('#txtToDate').val("");
                    
                },
                minLength: 0
            });


           //ending 

            //wareHouse Dropdown starting
               $("#txtWarehouse").val("");
           
            var textfieldname = $("#txtWarehouse");
            debugger;
            DropdownFunction(textfieldname);
            $("#txtWarehouse").autocomplete({
                source: function (request, response) {

                    if ($("#txtWarehouse").val() == "" || $("#txtWarehouse").val() == null || $("#txtWarehouse").val() == undefined) {
                         $scope.search.Warehouseid = 0;
                    }
                    $.ajax({
                       // url: '../mWebServices/FalconWebService.asmx/LoadWHForWHList',
                       //  data: "{ 'prefix': '" + request.term + "','TenantID':'"+ tenantid +"'  }",
                         url: '../mWebServices/FalconWebService.asmx/LoaDWHListBasedonUser',
                        data: "{ 'prefix': '" + request.term + "'  }",
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
                    debugger;
                    Warehouseid = i.item.val;
                    $("#txtWarehouse").val("");
                    $scope.search.Warehouseid = Warehouseid;
                    $("#txtTenant").val("");
                    tenantid = 0;
                    $('#txtStoreRefNo').val("");
                },
                minLength: 0
            });
            //ending of warehouse dropdown


            $scope.Fromdate = $filter('date')(new Date(), 'dd-MMM-yyyy');
            $('#txtFromDate').val($scope.Fromdate);
            $('#txtToDate').val($scope.Fromdate);
            $scope.GetIBLOGSummaryData = function (pageindex) {
                debugger;

                //if (tenantid == "0" || tenantid == null || tenantid == undefined|| $("#txtTenant").val().trim() == "" || $("#txtTenant").val() == null || $("#txtTenant").val() == undefined) {

                //    showStickyToast(false, "Please select Tenant");
                //    return false;
                //}

                if (Warehouseid == "0" || Warehouseid == null || Warehouseid == undefined || $("#txtWarehouse").val() == "0" || $("#txtWarehouse").val() == "" || $("#txtWarehouse").val() == null || $("#txtWarehouse").val() == undefined) {

                     showStickyToast(false, "Please select Warehouse");
                     return false;
                }
                if (tenantid == "" || tenantid == null || tenantid == undefined) {
                    tenantid = 0;
                }

                if ($('#txtStoreRefNo').val() == '') { $scope.search.inboundID = 0; }

                $scope.blockUI = true;
                if ($('#txtFromDate').val() == undefined || $('#txtFromDate').val() == '') {
                    $scope.search.FromDate = '';
                }
                else {
                    $scope.search.FromDate = $('#txtFromDate').val();
                }
                if ($('#txtFromDate').val() == undefined || $('#txtFromDate').val() == '') {
                    $scope.search.ToDate = '';
                }
                else {
                    $scope.search.ToDate = $('#txtToDate').val();
                }
                debugger
                var httpreq = {
                    method: 'POST',
                    url: 'IBSummaryReport.aspx/GetIBLOGSummaryData',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'obj': $scope.search,'TenantID':tenantid,'WarehouseID':Warehouseid,'FromDate':$scope.search.FromDate,'ToDate':$scope.search.ToDate,'StoreRefNo':$scope.search.inboundID,'PageSize':25,'PageIndex':pageindex },
                    async: false
                }
                $http(httpreq).success(function (response) {
                    debugger;
                    $scope.blockUI = false;

                    if (response.d != undefined && response.d != null) {
                        $scope.IBLOG = JSON.parse(response.d).Table1;
                    }
                    else {
                        showStickyToast(false, "No Data found ");
                    }
                    if ($scope.IBLOG.length == 0) {
                        showStickyToast(false, "No Data found ");
                    }
                    $scope.TotalData = JSON.parse(response.d).Table1[0];
                    $scope.TotalRecords=JSON.parse(response.d).Table3[0].Total;
                });
            }
           // $scope.GetIBLOGSummaryData();
            $scope.downloadExcel = function () {
                debugger;
                if ($('#txtFromDate').val() == undefined || $('#txtFromDate').val() == '') {
                    $scope.search.FromDate = '';
                }
                else {
                    $scope.search.FromDate = $('#txtFromDate').val();
                }
                if ($('#txtFromDate').val() == undefined || $('#txtFromDate').val() == '') {
                    $scope.search.ToDate = '';
                }
                else {
                    $scope.search.ToDate = $('#txtToDate').val();
                }
                if ($scope.IBLOG == undefined || $scope.IBLOG == null || $scope.IBLOG.length == 0) {
                    showStickyToast(false, "No Data found to Download Excel ");
                    return;
                }


                $scope.blockUI = true;

                var httpreq = {
                    method: 'POST',
                    url: 'IBSummaryReport.aspx/DownloadExcelForLog',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'obj': $scope.search },
                    async: false
                }
                $http(httpreq).success(function (response) {
                    debugger;
                    $scope.blockUI = false;
                    window.open('../ExcelData/' + response.d + '.xlsx');

                });
            }
        });




        function IBLOGSummarySearch(VehicleId, InvoiceNo, FromDate, ToDate,inboundID,Warehouseid,tenantid) {
            this.VehicleId = VehicleId;
            this.InvoiceNo = InvoiceNo;
            this.FromDate = FromDate;
            this.ToDate = ToDate;
            this.inboundID = inboundID;
            this.Warehouseid = Warehouseid;
            this.tenantid = tenantid;

        }

    </script>

     <div ng-app="IBApp" ng-controller="IBSummary" class="pagewidth">
        <div ng-show="blockUI">
            <div style="width: 100%; height: 100vh; z-index: 999; position: fixed; top: 0; left: 0; right: 0; bottom: 0; align-items: center; display: flex; justify-content: center; background: rgba(255, 255, 255, 0.24); background: hsla(0, 0%, 100%, 0.72);">

                <div style="align-self: center;">
                  <img width="60" src="../Images/preloader.svg" />

                </div>

            </div>

        </div>
        <div style="">
            <div class="row">
               
                  <div class="col s2 m2">
                    <div class="flex">
                        <div>

                            <input type="text" id="txtWarehouse" required="" />
                            <label>WareHouse</label>
                            <span class="errorMsg"></span>

                        </div>
                    </div>
                </div>

                 <div class="col s2 m2">
                    <div class="flex">
                        <div>

                            <input type="text" id="txtTenant" required="" />
                            <label>Tenant</label>
                            <%--<span class="errorMsg"></span>--%>

                        </div>
                    </div>
                </div>
                <div class="col s2 m2">
                    <div class="flex">
                        <div>

                            <input type="text" id="txtStoreRefNo" required="" />
                            <label>Store Reference No.</label>

                        </div>
                    </div>
                </div>
                <div class="col s2 m2">
                    <div class="flex">
                        <div>

                            <input type="text" id="txtFromDate" ng-model="search.FromDate" required="" />
                            <label>From Date</label>

                        </div>
                    </div>
                </div>
                <div class="col s2 m2">
                    <div class="flex">
                        <div>
                            <input type="text" id="txtToDate" ng-model="search.ToDate" required="" />
                            <label>To Date</label>
                        </div>
                    </div>
                </div>
                <div class="col s2 m2">
                    <gap></gap>
                    <div class="">

                        <div class="flex__ end">
                            <button type="button" ng-click="GetIBLOGSummaryData(1)" class="btn btn-primary obd">Search <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>&nbsp;&nbsp;
                             <button type='button' class="btn btn-primary" ng-click="downloadExcel()">Export <i class="material-icons">cloud_download</i></button>

                        </div>
                    </div>
                </div>
            </div>
        </div>

             <div class="row" style="margin: 0;">
            <div class="col-sm-6 col-lg-6" style="margin: 0; padding: 0;">
                <div class="divmainwidth" style="width: 100%; overflow: auto;">           
                    <table class="table-striped" id="" style="white-space: nowrap;">
                        <thead>
                            <tr>
                                <th number>S. No. </th>
                                <th>Tenant Code</th>
                                <th>Supplier</th>
                                 <th>Store Ref. No.</th>
                             <%--   <th>Gate Entry Ref# No. </th>   --%>                            
                                <th>Gate Entry Date</th>
                               <%-- <th>Vehicle No. </th>--%>
                               <%-- <th number>Unloaded Qty. </th>--%>
                                <th number>Pending GRN Qty. </th>
                                <th number>GRN Qty. </th>
                                <th number>Pending Putaway Qty.  </th>
                                <th number>Putaway Qty. </th>



                            </tr>
                        </thead>
                        <tbody>
                            <tr dir-paginate="IB in IBLOG|itemsPerPage:25" total-items="TotalRecords">
                                <td number>{{IB.SlNo}}</td>
                                <td>{{IB.TenantCode}}</td>
                                <td>{{IB.SupplierName}}</td>
                                 <td>{{IB.StoreRefNo }}</td>
                               <%-- <td>{{IB.GateEntryNumber }}</td>--%>
                                <td>{{IB.CreatedOn}}</td>
                                <%--<td>{{IB.VehicleNumber }}</td>--%>
                              <%--  <td number>{{IB.UnloadedQuantity }}</td>--%>
                                <td number>{{IB.PendingGRNQuantity }}</td>
                                <td number>{{IB.GRNQuantity }}</td>
                                <td number>{{IB.PendingPutawayQuantity }}</td>  
                                <td number>{{IB.PutawayQuantity }}</td>



                            </tr>
                        </tbody>
                        <tfoot ng-show="IBLOG.length != 0">
                            <tr ng-if="TotalData!=undefined && TotalData!=null && TotalData.length!=0">
                                <td number>
                                    <div>
                                    </div>
                                </td>
                                <td></td>
                           <%--     <td>
                                    <div Title="Grand Total across all pages">
                                        <b>Grand Total</b>
                                    </div>
                                </td>--%>
                              <%--  <td></td>--%>
                                <td></td>        
                                <td></td>        
                                <td number>
                                    <div Title="Sum of Pending GRN Qty. across all pages">
                                        <b>{{TotalData.PendingGRNQty}}</b>
                                    </div>
                                </td>
                                <td number>
                                    <div Title="Sum of GRN Qty across all pages">
                                        <b>{{TotalData.GRNQty}}</b>
                                    </div>
                                </td>
                                <td number>
                                    <div Title="Sum of Pending Putaway Qty across all pages">
                                        <b>{{TotalData.PendingPutawayQty}}</b>
                                    </div>
                                </td>
                                <td number>
                                    <div Title="Sum of Putaway Qty. across all pages">
                                        <b>{{TotalData.PutawayQty}}</b>
                                    </div>
                                </td>
                            </tr>
                        </tfoot>

                    </table>
                    <br />


                </div>




                <div class="divlineheight"></div>
            </div>
        </div>
        <br />
        <div flex end>
            <dir-pagination-controls direction-links="true" on-page-change="GetIBLOGSummaryData(newPageNumber)" boundary-links="true"> </dir-pagination-controls>
        </div>
    </div>
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MMContent" runat="server">
</asp:Content>
