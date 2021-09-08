<%@ Page Title="Outbound Summary" Language="C#" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="OutboundSummary.aspx.cs" Inherits="MRLWMSC21.mReports.OutboundSummary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="../Scripts/timeentry/jquery.timeentry.js"></script>

    <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
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

            $("#txtFromTime").timeEntry();
            $("#txtToTime").timeEntry();
        });


    </script>
    <style>
        .flex .timeEntry_wrap ~ label {
            top: -7px !important;
            font-size: 11px !important;
            font-weight: 500;
        }
        .flex .errorMsg {
            width:20px !important;
        }
    </style>
    <script>
        "use strict"
        const app = angular.module('OBDApp', ['angularUtils.directives.dirPagination'])
            .controller('OBDCtrl', function ($scope, $http, $compile, $filter) {
                $scope.CustomerID = 0;
                $scope.OutboundID = 0;
                var tenantid = 0;
                var Warehouseid = 0;
                var LoadsheetHID = 0; var SOHeaderID = 0;
                var fromDate = "", toDate = "", fromTime = "", toTime = "";
                $("#txtTenant").val("");

                var textfieldname = $("#txtTenant");
                debugger;
                DropdownFunction(textfieldname);
                $("#txtTenant").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                           <%-- url: '../mWebServices/FalconWebService.asmx/LoadTenantsDataFor3PL',
                            data: "{ 'prefix': '" + request.term + "','Accountid': <%= cp.AccountID%>  }",--%>
                        url: '../mWebServices/FalconWebService.asmx/LoadTenantsByWH',
                        data: "{ 'prefix': '" + request.term + "','whid':'"+Warehouseid+"' }",
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
                $("#txtCustomer").val("");
                $("#txtOutbound").val("");
                $('#txtLoadSheetNo').val("");
                $('#txtSONo').val("");
                //$("#txtWarehouse").val("");
                $scope.CustomerID = 0;
                $scope.OutboundID = 0;
                SOHeaderID = 0;
                LoadsheetHID = 0;
                //Warehouseid = 0;
            },
            minLength: 0
        });

        var textfieldname = $("#txtCustomer");
        DropdownFunction(textfieldname);
        $("#txtCustomer").autocomplete({
            source: function (request, response) {
                debugger;
                $.ajax({
                        <%--//url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/CustomerBasedonAccount") %>', --%>                       
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/CustomerBasedonTenant") %>',
                            data: JSON.stringify({ 'prefix': request.term, 'TenantId': tenantid }),
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                if (data.d == "" || data.d == ",") {
                                    alert("No customer is available");

                                    return;
                                }
                                response($.map(data.d, function (item) {
                                    return {
                                        label: item.split(',')[0],
                                        val: item.split(',')[1]
                                    }
                                }))
                            },
                            error: function (response) {

                            },
                            failure: function (response) {

                            }
                        });
                    },
                    select: function (e, i) {
                        //  $("#hidCustomer").val(i.item.val);
                        $scope.CustomerID = i.item.val;
                        $("#txtOutbound").val("");
                        $('#txtSONo').val("");
                        $scope.OutboundID = 0;
                        SOHeaderID = 0;
                    },
                    minLength: 0
                });


                $('#txtOutbound').val("");
                var textfieldname = $("#txtOutbound");
                DropdownFunction(textfieldname);
                $("#txtOutbound").autocomplete({
                    source: function (request, response) {
                        debugger;
                        $.ajax({
                            url: '../mWebServices/FalconWebService.asmx/Get_OBDList',
                            data: JSON.stringify({ 'prefix': request.term, 'TenantId': tenantid, 'WarehouseId': Warehouseid, 'CustomerId': $scope.CustomerID }),
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
                                    showStickyToast(false, "There is no OBD's for given search criteria", false);
                                    return false;
                                }
                            }
                        });
                    },
                    select: function (e, i) {
                        $scope.OutboundID = i.item.val;
                        $('#txtSONo').val("");
                        SOHeaderID = 0;
                    },
                    minLength: 0
                });

                $("#txtWarehouse").val("");
                var textfieldname = $("#txtWarehouse");
                debugger;
                DropdownFunction(textfieldname);
                $("#txtWarehouse").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            //url: '../mWebServices/FalconWebService.asmx/LoadWHForWHListWithUserIDForOBDSummary',
                            //data: "{ 'prefix': '" + request.term + "','TenantID':'" + tenantid + "'  }",
                            url: '../mWebServices/FalconWebService.asmx/LoaDWHListBasedonUser',
                            data: "{ 'prefix': '" + request.term + "','whid':'"+Warehouseid+"' }",
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
                        Warehouseid = i.item.val;
                        //$("#txtWarehouse").val("");
                        $("#txtCustomer").val("");
                        $scope.CustomerID = 0;
                        $scope.OutboundID = 0;
                        $("#txtOutbound").val("");
                        $("#txtTenant").val("");
                        $('#txtLoadSheetNo').val("");
                        $('#txtSONo').val("");
                        tenantid = 0;
                        SOHeaderID = 0;
                        LoadsheetHID = 0;
                    },
                    minLength: 0
                });

                var textfieldname = $('#txtLoadSheetNo');
            DropdownFunction(textfieldname);
            $('#txtLoadSheetNo').autocomplete({
                source: function (request, response) {
                    debugger;
                    if (tenantid == "" || tenantid == null || tenantid == undefined) {
                        //tenantid = 0;
                        showStickyToast(false, "Please select Tenant", false);
                        return false;
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
                                showStickyToast(false, "No Load Sheet Number Found", false);
                            }
                        }

                    });
                },
                select: function (e, i) {
                    //$('#hdnLoadSheetNo').val(i.item.val);
                    LoadsheetHID = i.item.val;
                },
                minLength: 0
            });


            var textfieldname = $('#txtSONo');
            DropdownFunction(textfieldname);
            $('#txtSONo').autocomplete({
                source: function (request, response) {
                    debugger;
                    if ($scope.OutboundID == "" || $scope.OutboundID == null || $scope.OutboundID == undefined) {
                        //tenantid = 0;
                        showStickyToast(false, "Please select Outbound", false);
                        return false;
                    }
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetSOListUnderOBD") %>',
                        data: "{ 'prefix': '" + request.term + "', 'OBDID': '" + $scope.OutboundID + "'}",
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
                                showStickyToast(false, "No SO Number Found", false);
                            }
                        }

                    });
                },
                select: function (e, i) {
                    //$('#hdnLoadSheetNo').val(i.item.val);
                    SOHeaderID = i.item.val;
                },
                minLength: 0
            });



                $scope.getData = function (pageindex) {
                    debugger;
                    if (tenantid == "0" || tenantid == null || tenantid == undefined || $("#txtTenant").val().trim() == "" || $("#txtTenant").val() == null || $("#txtTenant").val() == undefined) {
                        //showStickyToast(false, "Please select Tenant");
                        //return false;
                        tenantid = "0";
                    }

                    if (Warehouseid == "0" || Warehouseid == null || Warehouseid == undefined || $("#txtWarehouse").val() == "0" || $("#txtWarehouse").val() == "" || $("#txtWarehouse").val() == null || $("#txtWarehouse").val() == undefined) {
                        showStickyToast(false, "Please select Warehouse");
                        return false;
                    }

                    $scope.TotalResult = '';
                    if ($('#txtCustomer').val() == '') { $scope.CustomerID = 0; }
                    if ($('#txtOutbound').val() == '') { $scope.OutboundID = 0; }

                    if ($('#txtLoadSheetNo').val() == '') { LoadsheetHID = 0; }
                    if ($('#txtSONo').val() == '') { SOHeaderID = 0; }

                    if ($("#txtFromDate").val() == "") {
                        fromDate = "";
                        showStickyToast(false, "Please select From Date", false); return false;
                    } else { fromDate = $("#txtFromDate").val(); }
                    if ($("#txtFromTime").val() == "") {
                        fromTime = "";
                        showStickyToast(false, "Please enterFrom Time", false); return false;
                    } else { fromTime = $("#txtFromTime").val(); }
                    if ($("#txtToDate").val() == "") {
                        toDate = "";
                        showStickyToast(false, "Please select To Date", false); return false;
                    } else { toDate = $("#txtToDate").val(); }
                    if ($("#txtToTime").val() == "") {
                        toTime = "";
                        showStickyToast(false, "Please enter To Time", false); return false;
                    } else { toTime = $("#txtToTime").val(); }
                    $scope.blockUI = true;
                    var req = {
                        method: 'POST',
                        url: 'OutboundSummary.aspx/getOutBoundSummary',
                        headers: {
                            'Content-Type': 'application/json; charset=utf-8',
                            'dataType': 'json'
                        },
                        //data: { 'TenantID':tenantid,'WarehouseID':Warehouseid,'FromDate': $('#txtFromDate').val(),'Customerid': $scope.CustomerID ,'Outboundid':$scope.OutboundID,'PageSize':25,'PageIndex':pageindex },
                        data: { 'FromDate': fromDate, 'ToDate': toDate, 'PageSize': 25, 'PageIndex': pageindex, 'Customerid': $scope.CustomerID, 'Outboundid': $scope.OutboundID, 'TenantID': tenantid, 'WarehouseID': Warehouseid, 'FromTime': fromTime, 'ToTime': toTime, 'LoadSHID': LoadsheetHID, 'SOHID': SOHeaderID },
                    }
                    $http(req).success(function (response) {
                        debugger
                        if (response.d == "0") {
                            $scope.blockUI = false;
                            showStickyToast(false, "Dates should not be more than One Week", false); return false;
                        }
                        var dt = JSON.parse(response.d);
                        $scope.SummaryData = dt.Table;
                        $scope.blockUI = false;
                        if ($scope.SummaryData == undefined) {
                            showStickyToast(false, "No Data found", false); return false;
                        }
                        else {
                            $scope.TotalResult = dt.Table[0];
                            $scope.TotalRecords = dt.Table[0].TotalRecords;
                        }
                        
                    });
                };
                // $scope.getData(1);

                $scope.clearData = function () {
                    debugger;
                    $("#PickListRefNo").val(""); $("#Customer").val(""); $("#txtstatus").val(""); $("#txtOrderType").val("");
                    $scope.PLHeaderID = 0;
                    $scope.CustomerID = 0;
                    $scope.StatusID = 0;
                    $scope.OrderTypeId = 0;
                    $scope.Fromdate = "";
                    $scope.Todate = "";
                    $('#txtFromDate').val('');
                };

                $scope.downloadExcel = function () {
                    debugger;
                    if ($scope.SummaryData == undefined || $scope.SummaryData == null || $scope.SummaryData.length == 0) {
                        showStickyToast(false, "No Data found to Download Excel ");
                        return;
                    }
                    if (tenantid == "0" || tenantid == null || tenantid == undefined || $("#txtTenant").val().trim() == "" || $("#txtTenant").val() == null || $("#txtTenant").val() == undefined) {
                        tenantid = "0";
                    }
                    if (Warehouseid == "0" || Warehouseid == null || Warehouseid == undefined || $("#txtWarehouse").val() == "0" || $("#txtWarehouse").val() == "" || $("#txtWarehouse").val() == null || $("#txtWarehouse").val() == undefined) {
                        showStickyToast(false, "Please select Warehouse");
                        return false;
                    }
                    if ($('#txtCustomer').val() == '') { $scope.CustomerID = 0; }
                    if ($('#txtOutbound').val() == '') { $scope.OutboundID = 0; }
                    if ($('#txtLoadSheetNo').val() == '') { LoadsheetHID = 0; }
                    if ($('#txtSONo').val() == '') { SOHeaderID = 0; }
                    if ($("#txtFromDate").val() == "") {
                        fromDate = "";
                        showStickyToast(false, "Please select From Date", false); return false;
                    } else { fromDate = $("#txtFromDate").val(); }
                    if ($("#txtFromTime").val() == "") {
                        fromTime = "";
                        showStickyToast(false, "Please enterFrom Time", false); return false;
                    } else { fromTime = $("#txtFromTime").val(); }
                    if ($("#txtToDate").val() == "") {
                        toDate = "";
                        showStickyToast(false, "Please select To Date", false); return false;
                    } else { toDate = $("#txtToDate").val(); }
                    if ($("#txtToTime").val() == "") {
                        toTime = "";
                        showStickyToast(false, "Please enter To Time", false); return false;
                    } else { toTime = $("#txtToTime").val(); }
                    $scope.blockUI = true;
                    var accounts = {
                        method: 'POST',
                        url: 'OutboundSummary.aspx/DownLoadExcelDataForItems',
                        headers: {
                            'Content-Type': 'application/json; charset=utf-8',
                            'dataType': 'json'
                        },                        
                        // data: { 'TenantID': tenantid, 'WarehouseID': Warehouseid, 'FromDate': $('#txtFromDate').val(), 'Customerid': $scope.CustomerID, 'Outboundid': $scope.OutboundID },
                        data: { 'FromDate': fromDate, 'ToDate': toDate, 'Customerid': $scope.CustomerID, 'Outboundid': $scope.OutboundID, 'TenantID': tenantid, 'WarehouseID': Warehouseid,'FromTime': fromTime, 'ToTime': toTime , 'LoadSHID': LoadsheetHID, 'SOHID': SOHeaderID},
                    }
                    $http(accounts).success(function (response) {
                        $scope.blockUI = false;
                        if (response.d == "0") {                            
                            showStickyToast(false, "Dates should not be more than One Week", false); return false;
                        }
                        window.open('../ExcelData/' + response.d + '.xlsx');
                    });

                }
            });

    </script>
    <div ng-app="OBDApp" ng-controller="OBDCtrl" class="container">
        <div ng-show="blockUI">
            <div style="width: 100%; height: 100vh; z-index: 999; position: fixed; top: 0; left: 0; right: 0; bottom: 0; align-items: center; display: flex; justify-content: center; background: rgba(255, 255, 255, 0.24); background: hsla(0, 0%, 100%, 0.72);">

                <div style="align-self: center;">
                    <img width="60" src="../Images/preloader.svg" />

                </div>

            </div>

        </div>
        <div class="row">
            <div class="col m3">
                <div class="flex">
                    <input type="text" id="txtWarehouse" required />
                    <label>WareHouse</label>
                    <span class="errorMsg"></span>
                </div>
            </div>
            <div class="col m3">
                 <div class="flex">
                    <input type="text" id="txtTenant" required />
                    <label>Tenant</label>                   
                    <input type="hidden" id="hdnTenant" />
                </div>               
            </div>
            <div class="col m3">
                <div class="flex">
                    <input type="text" id="txtCustomer" required />
                    <input type="hidden" id="hidCustomer" />
                    <label>Customer</label>
                </div>
            </div>

            <div class="col m3">
                 <div class="flex">
                    <input type="text" id="txtLoadSheetNo" required />
                    <label>Load Sheet No.</label>                   
                </div>               
            </div>

            <div class="col m3">
                <div class="flex">
                    <input type="text" id="txtOutbound" required />
                    <label>Outbound Number</label>
                </div>
            </div>

            <div class="col m3">
                 <div class="flex">
                    <input type="text" id="txtSONo" required />
                    <label>SO Number</label>                   
                </div>               
            </div>            

            <div class="col m2">
                <div class="flex">
                    <input type="text" id="txtFromDate" required />
                    <label>From Date</label>
                     <span class="errorMsg"></span>
                </div>
            </div>
            <div class="col m1">
                <div class="flex">
                    <input type="text" id="txtFromTime" required />
                    <label>From Time</label>
                     <span class="errorMsg"></span>
                </div>
            </div>
            <div class="col m2">
                <div class="flex">
                    <input type="text" id="txtToDate" required />
                    <label>To Date</label>
                     <span class="errorMsg"></span>
                </div>
            </div>
            <div class="col m1">
                <div class="flex">
                    <input type="text" id="txtToTime" required />
                    <label>To Time</label>
                     <span class="errorMsg"></span>
                </div>
            </div>
            <div class="col m12">
                <gap5></gap5>
                <flex end>
                    <button type="button" ng-click="getData(1)" class="btn btn-primary obd">Search <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>&nbsp;&nbsp;   
                    <button type='button' class="btn btn-primary" ng-click="downloadExcel()">Export <i class="fa fa-file-excel-o"></i></button>
               </flex>
            </div>
        </div>
        <div class="row">
            <div class="col m12" style="overflow: auto;">

                <table class="table-striped" style="width:2700px !important">
                    <thead>
                        <tr>
                            <th number>S. No</th>
                            <th>WH Code</th>
                            <th>Tenant Code</th>
                            <th>Load Sheet#</th>
                            <th>Customer</th>
                            <th>SO#</th>
                            <th>OBD#</th>
                            <th>OBD Status</th>
                            <th>Shipment Type</th>
                            <th>AWB#</th>
                            <th>Courier</th>
                            <th>MCode</th>
                            <th>Description</th>
                            <th center>Mfg. Date</th>
                            <th center>Exp. Date</th>
                            <th>Serial#</th>
                            <th>Batch#</th>
                            <th>Project Ref.#</th>
                            <th>MRP</th>
                            <th number>Inv. Qty.</th>
                            <th number>Assigned Qty.</th>
                            <th number>Picked Qty.</th>
                            <th number>Packed Qty.</th>
                            <th number>Loaded Qty.</th>
                            <th number>PGI Qty.</th>
                            <th number>Volume(CBM)</th>
                            <th center>Deliv. Doc. Timestamp</th>
                            <th center>PGI Updated Timestamp</th>
                            <th center>Delivered Timestamp</th>                            
                        </tr>
                    </thead>
                    <tbody>
                        <tr dir-paginate="dt in SummaryData|itemsPerPage:25 " total-items="TotalRecords">
                            <td number>{{$index+1}}</td>
                            <td>{{dt.WHCode}}</td>
                            <td>{{dt.TenantCode}}</td>
                            <td>{{dt.LoadsheetNo}}</td>
                            <td>{{dt.CustomerName}}</td>
                            <td>{{dt.SONumber}}</td>
                            <td>{{dt.OBDNumber}}</td>
                            <td>{{dt.Status}}</td>
                            <td>{{dt.ShipmentType}}</td>
                            <td>{{dt.AWB}}</td>
                            <td>{{dt.Courier}}</td>
                            <td>{{dt.MCode}}</td>
                            <td>{{dt.Description}}</td>
                            <td style="text-align:center;">{{dt.MfgDate}}</td>
                            <td style="text-align:center;">{{dt.ExpDate}}</td>
                            <td>{{dt.SerialNo}}</td>
                            <td>{{dt.BatchNo}}</td>
                            <td>{{dt.ProjectRefNo}}</td>
                            <td>{{dt.MRP}}</td>
                            <td number>{{dt.InvoiceQty}}</td>
                            <td number>{{dt.AssignedQty}}</td>
                            <td number>{{dt.PickedQty}}</td>
                            <td number>{{dt.PackedQty}}</td>
                            <td number>{{dt.LoadQty}}</td>
                            <td number>{{dt.PGIQty}}</td>
                            <td number>{{dt.VolumeCBM}}</td>
                            <td style="text-align:center;">{{dt.DelivDocTimestamp}}</td>
                            <td style="text-align:center;">{{dt.PGIUpdatedTimestamp}}</td>
                            <td style="text-align:center;">{{dt.DeliveredTimestamp}}</td>                            
                        </tr>
                    </tbody>
                    <tfoot style="display:none;">
                        <tr>
                            <td><b>Total</b></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td number><b>{{TotalResult.SuggestedQuantity}}</b></td>
                            <td number><b>{{TotalResult.PickedQuantity}}</b></td>
                            <%-- <td number><b>{{TotalResult.LoadedQuantity}}</b></td>
                            <td number><b>{{TotalResult.InvoiceQuantity}}</b></td>
                             <td number><b>{{TotalResult.TotalSuggestedCFT}}</b></td>
                             <td number><b>{{TotalResult.TotalPickedCFT}}</b></td>
                            <td number><b>{{TotalResult.TotalLoadedCFT}}</b></td>--%>
                        </tr>
                    </tfoot>
                </table>

                
            </div>
        </div>
        <div class="row">
            <div class="col m12">
                <div flex end>
                    <dir-pagination-controls direction-links="true" on-page-change="getData(newPageNumber)" boundary-links="true"> </dir-pagination-controls>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MMContent" runat="server">
</asp:Content>
