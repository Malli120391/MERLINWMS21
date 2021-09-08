<%@ Page Title="" Language="C#" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="LoadSheetReport.aspx.cs" Inherits="FalconWMS.mReports.LoadSheetReport" %>

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
     <style>
        
        .mytablechildOutbound tr td {
                padding: 9px 15px 9px 0px !important;
        }
        .setwidth select,input
        {
            width:100% !important;
        }

        .table-striped{
            white-space:nowrap;
        }

        .table-striped td input{
            width:30px !important;
        }
    </style>
    <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
    <link href="../mInventory/Scripts/bootstrap.min.css" rel="stylesheet" />
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>
    <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />
    <script>
        var app = angular.module('IBApp', ['angularUtils.directives.dirPagination']);
        app.controller('LoadSheet', function ($scope, $http, $timeout, $window, $filter) {
            debugger;
            $scope.search = new LoadSheetSearch(0, 0, 0, 0);
            var textfieldname = $('#txtOutboundNo');
            DropdownFunction(textfieldname);
            $('#txtOutboundNo').autocomplete({
                source: function (request, response) {
                    debugger;
                    if (tenantid == "" || tenantid == null || tenantid == undefined) {
                        tenantid = 0;
                    }
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadOBDNumbers") %>',
                        data: "{ 'prefix': '" + request.term + "', 'TenantId': '" + tenantid + "'}",
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
                                showStickyToast(false, "No Outbound found ")
                            }

                        }

                    });
                },
                select: function (e, i) {

                    $scope.search.outboundID = i.item.val;

                },
                minLength: 0
            });

            var textfieldname = $('#txtLoadSheetNumber');
            DropdownFunction(textfieldname);
            $('#txtLoadSheetNumber').autocomplete({
                source: function (request, response) {
                    debugger;
                    if (tenantid == "" || tenantid == null || tenantid == undefined) {
                        tenantid = 0;
                    }
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadLoadSheetNumbers") %>',
                        data: "{ 'prefix': '" + request.term + "', 'TenantId': '" + tenantid + "'}",
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
                                showStickyToast(false, "No Load Sheet Number Found")
                            }

                        }

                    });
                },
                select: function (e, i) {

                    $scope.search.LoadSheetHeaderID = i.item.val;

                },
                minLength: 0
            });

            var tenantid;


             var textfieldname = $("#txtWarehouse");
    //debugger;
    
    DropdownFunction(textfieldname);
    $("#txtWarehouse").autocomplete({
        source: function (request, response) {
            debugger;
            $.ajax({
                //url: '../mWebServices/FalconWebService.asmx/LoadWHForWHList',
                //data: "{ 'prefix': '" + request.term + "','TenantID':'" + Tenantid + "'  }",
                url: '../mWebServices/FalconWebService.asmx/LoaDWHListBasedonUser',
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
            Warehouseid = i.item.val;
        },
        minLength: 0
    });
   
            //Tenant drop down

            $("#txtTenant").val("");

            var textfieldname = $("#txtTenant");
            debugger;
            DropdownFunction(textfieldname);
            $("#txtTenant").autocomplete({
                source: function (request, response) {
                    $.ajax({
                      <%--  url: '../mWebServices/FalconWebService.asmx/LoadTenantsDataFor3PL',
                        data: "{ 'prefix': '" + request.term + "','Accountid': <%= cp.AccountID%>  }",--%>
                        //  url: '../mWebServices/FalconWebService.asmx/LoadTenantDataByUserWH',
                        //data: "{ 'prefix': '" + request.term + "' }",
                          url: '../mWebServices/FalconWebService.asmx/GetWarehouseTenant',
                          data: "{ 'prefix': '" + request.term + "','WHID':'" + Warehouseid +"'}",
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
                    tenantid = i.item.val;
                    $("#txtTenant").val("");
                    $scope.search.tenantid = tenantid;
                    $('#txtOutboundNo').val("");
                    $('#txtLoadSheetNumber').val("");

                },
                minLength: 0
            });


            //ending 


            $scope.GetLoadSheetSummaryData = function (pageindex) {
                debugger;
                if (tenantid == "0" || tenantid == null || tenantid == undefined || $("#txtTenant").val().trim() == "" || $("#txtTenant").val() == null || $("#txtTenant").val() == undefined) {

                    showStickyToast(false, "Please select Tenant");
                    return false;
                }

                $scope.search.StatusID = $('#txtprpo option:selected').toArray().map(item => item.value).join();

                if ($('#txtLoadSheetNumber').val() == '') { $scope.search.LoadSheetHeaderID = 0; }
                if ($('#txtOutboundNo').val() == '') { $scope.search.outboundID = 0; }
                if ($('#txtStatus').val() == '') { $scope.search.StatusID = 0; } else { $scope.search.StatusID }
                $scope.blockUI = true;

                debugger
                var httpreq = {
                    method: 'POST',
                    url: 'LoadSheetReport.aspx/GetLoadSheetSummaryData',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'obj': $scope.search, 'TenantID': tenantid, 'OuboundID': $scope.search.ouboundID, 'LoadSheetHeaderID': $scope.search.LoadSheetHeaderID, 'PageSize': 25, 'PageIndex': pageindex },
                    async: false
                }
                $http(httpreq).success(function (response) {
                    debugger;
                    $scope.blockUI = false;

                    if (response.d != undefined && response.d != null) {
                        $scope.IBLOG = JSON.parse(response.d).Table;
                    }
                    else {
                        showStickyToast(false, "No Data found ");
                    }
                    if ($scope.IBLOG.length == 0) {
                        showStickyToast(false, "No Data found ");
                    }
                    $scope.TotalData = JSON.parse(response.d).Table[0];
                    $scope.TotalRecords = JSON.parse(response.d).Table[0].TotalRecords;
                });
            }



            //ending 
            $scope.OpenPOPUP = function (LoadHeaderID) {
                debugger

                $("#DivAddUser").modal({
                    show: 'true'

                });

                $scope.ViewPickedRSNData(LoadHeaderID);
            }
            var LoadsheetID;
            $scope.ViewPickedRSNData = function (LID) {
                debugger;
                LoadsheetID = LID;
                var accounts = {
                    method: 'POST',
                    url: 'LoadSheetReport.aspx/GetLoadSheetData',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: {
                        'LoadSheetID': LID
                    }
                }
                $http(accounts).success(function (response) {
                    debugger;
                    $scope.blockUI = false;
                    $scope.PickedRSNData = JSON.parse(response.d).Table;
                   // $scope.PickedRSNData = JSON.parse(response.d);
                    //$scope.PickedRSNData = JSON.parse(response.d).Table[0];
                    //$("#ViewRSNData").modal("show");
                    $("#ViewRSNData").modal({
                        show: 'true'
                    });
                });
            }

            // $scope.GetLoadSheetSummaryData();
            $scope.downloadExcel = function () {
                debugger;
                if ($scope.PickedRSNData == undefined || $scope.PickedRSNData == null || $scope.PickedRSNData.length == 0) {
                    showStickyToast(false, "No Data found to Download Excel ");
                    return;
                }

                $scope.blockUI = true;


                 var httpreq = {
                    method: 'POST',
                    async: false,
                    url: 'LoadSheetReport.aspx/DownloadExcelForLog',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: {
                        'LoadSheetID': LoadsheetID
                     }

                }
                $http(httpreq).success(function (response) {
                    debugger;
                    $scope.blockUI = false;
                    window.open('../ExcelData/' + response.d + '.xlsx');

                });
            }
        });

        function LoadSheetSearch(tenantid, LoadSheetHeaderID, ouboundID, StatusID) {
            this.tenantid = tenantid;
            this.LoadSheetHeaderID = LoadSheetHeaderID;
            this.ouboundID = ouboundID;
            this.StatusID = StatusID;

        }

    </script>

    <div ng-app="IBApp" ng-controller="LoadSheet" class="pagewidth">
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
                    <input type="text" id="txtWarehouse" required="" />
                    <label>Warehouse</label>
                     <span class="errorMsg"></span>
                </div>
            </div>
                <div class="col s2 m2">
                    <div class="flex">
                        <div>

                            <input type="text" id="txtTenant" required="" />
                            <label>Tenant</label>
                            <span class="errorMsg"></span>

                        </div>
                    </div>
                </div>
                <div class="col s2 m2">
                    <div class="flex">
                        <div>

                            <input type="text" id="txtLoadSheetNumber" required="" />
                            <label>Load Sheet No.</label>

                        </div>
                    </div>
                </div>
                <div class="col s2 m2">
                    <div class="flex">
                        <div>

                            <input type="text" id="txtOutboundNo" required="" />
                            <label>Outbound No.</label>

                        </div>
                    </div>
                </div>
                <div class="col s2 m2">
                    <div class="flex">
                        <div>

                            <select id="txtprpo" name="txtprpo" class="form-control">
                                <option value="0" selected></option>
                                <option value="1">Initiated</option>
                                <option value="2">In-Process</option>
                                <option value="3">Completed</option>
                            </select>
                            <label>Status</label>

                        </div>
                    </div>
                </div>
                <div class="col s2 m2">
                    <gap></gap>
                    <div class="">

                        <div class="flex__ end">
                            <button type="button" ng-click="GetLoadSheetSummaryData(1)" class="btn btn-primary obd">Search <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>&nbsp;&nbsp;
                            

                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row" style="margin: 0;">
            <div class="col-sm-12 col-lg-12" style="margin: 0; padding: 0;">
                <div class="divmainwidth" style="width: 100%; overflow: auto;">
                    <table class="table-striped" id="" style="white-space: nowrap;">
                        <thead>
                            <tr>
                                <th>S. No. </th>
                                <%-- <th>Load Sheet ID.</th>--%>
                                <th>Load Sheet No.</th>
                                <th>Driver Name</th>
                                <th>Driver No.</th>
                                <th>Vehicle No.</th>
                                <th>LR No. </th>
                                <th>Status</th>
                                <th>View</th>

                            </tr>
                        </thead>
                        <tbody>
                            <tr dir-paginate="IB in IBLOG | itemsPerPage : 25" pagination-id="cdata">
                                <td>{{$index + 1}}</td>
                                <%-- <td>{{IB.LoadSheetId}}</td>--%>
                                <td>{{IB.LoadSheetNo }}</td>
                                <td>{{IB.DriverName}}</td>
                                <td>{{IB.DriverNo }}</td>
                                <td>{{IB.VehileNo }}</td>
                                <td>{{IB.LRNumber }}</td>
                                <td>{{IB.Status}}</td>
                                <%--<td><a   ng-click="ViewPickedRSNData(10)" title="View Load Sheet List"><i class="material-icons">remove_red_eye</i></a></td>--%>
                                <%--<td> <button type="button" id="btncomplete"  class="btn btn-primary" ng-click="ViewPickedRSNData(10)">View</button></td>--%>
                                <td><a ng-click="ViewPickedRSNData(IB.LoadSheetId)"><i class="material-icons">remove_red_eye</i><em class="sugg-tooltis" style="width: 90px; left: -100px;">Load Sheet</em></a></td>
                                <%--<td><a  ng-click="OpenPOPUP(IB.LoadSheetId)"><i class="material-icons">remove_red_eye</i><em class="sugg-tooltis" style="width: 90px;left: -100px;">Load Sheet</em></a></td>--%>
                            </tr>
                        </tbody>
                    </table>
                    <br />


                </div>
                <div class="divlineheight"></div>
            </div>
        </div>
        <br />
       <div class="divpaginationstyle" flex end>
                                    <dir-pagination-controls direction-links="true" boundary-links="true" pagination-id="cdata"></dir-pagination-controls>
     </div>
     <div class="modal inmodal" id="ViewRSNData" tabindex="-1" role="dialog" aria-hidden="true">
            <div class="modal-dialog" style="width: 60% !important;">
                <div class="modal-content animated fadeIn">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                        <h4 class="modal-title">Load Sheet Details</h4>
                    </div>
                    <div class="modal-body" style="height: 450px; overflow: auto;">
                        <div style="padding-left:700px">
                             <button type='button' class="btn btn-primary" ng-click="downloadExcel()">Export <i class="material-icons">cloud_download</i></button>
                        </div>
                        <br />
                        <div id="divEntityDetails" class="form-horizontal">
                            <form role="form">

                                <table id="tblList" class="table-striped">
                                    <thead>
                                        <tr>
                                            <th>S. No.</th>
                                            <th>Outbound No.</th>
                                            <%--<th>Sales Order No.</th>--%>
                                            <th>Line No.</th>
                                            <th>SKU</th>
                                            <th>SKU Description</th>
                                            <th>Picked Qty.</th>
                                            <th>Mfg. Date</th>
                                            <th>Exp. Date</th>
                                            <th>MRP</th>
                                            <th>Batch No.</th>
                                            <th>Customer Name</th>
                                            <th>Customer Address</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr dir-paginate="dt in PickedRSNData | itemsPerPage : 10" pagination-id="childdata">
                                            <td>{{$index + 1}}</td>
                                            <td>{{dt.OBDNumber}}</td>
                                            <%--  <td>{{dt.SONumber}}</td>--%>
                                            <td>{{dt.LineNumber}}</td>
                                            <td>{{dt.MCode}}</td>
                                            <td>{{dt.MDescription}}</td>
                                            <td>{{dt.PickedQty}}</td>
                                            <td>{{dt.MfgDate}}</td>
                                            <td>{{dt.ExpDate}}</td>
                                            <td>{{dt.MRP}}</td>
                                            <td>{{dt.BatchNo}}</td>
                                            <td>{{dt.CustomerName}}</td>
                                            <td>{{dt.CustomerAddress}}</td>
                                        </tr>
                                    </tbody>
                                </table>
                                
                                <div class="divpaginationstyle" flex end>
                                    <dir-pagination-controls direction-links="true" boundary-links="true" pagination-id="childdata"></dir-pagination-controls>
                                </div>
                            </form>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-white" data-dismiss="modal" style="color: white !important;">Close</button>
                    </div>
                </div>
            </div>
        </div>

</div>

   


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MMContent" runat="server">
</asp:Content>
