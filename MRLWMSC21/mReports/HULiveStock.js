var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('HULiveStock', function ($scope, $http) {

    var Tenantid = 0;
    var Warehouseid = 0;
    var MMID = 0;
    $scope.StockSummary = [];

    $('#txtTenant').val("");
    var textfieldname = $("#txtTenant");
    DropdownFunction(textfieldname);
    $("#txtTenant").autocomplete({
        source: function (request, response) {
            debugger;
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/GetWarehouseTenant',
                data: "{ 'prefix': '" + request.term + "','WHID':'" + Warehouseid + "'}",
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
            Tenantid = i.item.val;
        },
        minLength: 0
    });

    $("#txtWarehouse").val("");
    var textfieldname = $("#txtWarehouse");
    DropdownFunction(textfieldname);
    $("#txtWarehouse").autocomplete({
        source: function (request, response) {
            debugger;
            $.ajax({
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


    var textfieldname = $("#txtPartNo");
    DropdownFunction(textfieldname);
    $("#txtPartNo").autocomplete({
        source: function (request, response) {
            debugger;
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/GetReplenishedMaterialCode',
                data: "{ 'prefix': '" + request.term + "','TenantID':'" + Tenantid + "'}",
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
            MMID = i.item.val;
        },
        minLength: 0
    });

    $scope.getDetails = function (pageNo) {
        debugger;     

        if ($("#txtWarehouse").val().trim() == "" || $("#txtWarehouse").val().trim() == undefined || $("#txtWarehouse").val().trim() == null) {
            Warehouseid = 0;
            showStickyToast(false, "Please select Warehouse");
            return false;
        }
        else { Warehouseid = Warehouseid; }

        if ($("#txtTenant").val().trim() == "" || $("#txtTenant").val().trim() == undefined || $("#txtTenant").val().trim() == null) {
            Tenantid = 0;
            showStickyToast(false, "Please select Tenant");
            return false;
        }
        else { Tenantid = Tenantid; }

        if ($("#txtPartNo").val().trim() == "" || $("#txtPartNo").val().trim() == undefined || $("#txtPartNo").val().trim() == null) {
            MMID = 0;
        }
        else { MMID = MMID; }


        var batchNo = $("#txtBatchNo").val().trim();
        var serialNo = $("#txtSerialNo").val().trim();
        var mfgDate = $("#txtMfgDate").val().trim();
        var expDate = $("#txtExpDate").val().trim();
        var ProjectRefNo = $("#txtProjectRefNo").val().trim();
        var MRP = $("#txtMRP").val().trim();
        $scope.ReportType = $("#selReportType").val();

        $scope.blockUI = true;
        var httpreq = {
            method: 'POST',
            url: 'HULiveStock.aspx/getHUStockDetails',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: {
                'WHID': Warehouseid, 'TenantID': Tenantid, 'MID': MMID, 'BatchNo': batchNo, 'SerialNo': serialNo, 'MfgDate': mfgDate, 'ExpDate': expDate, 'ProjectRefNo': ProjectRefNo, 'MRP': MRP, 'ReportType': $("#selReportType").val(), 'PageNo': pageNo, 'PageSize': 25
            },
            async: false
        }
        $http(httpreq).success(function (response) {
            debugger
            var dt = JSON.parse(response.d).Table;
            if (dt == undefined || dt == null || dt.length == 0) {
                showStickyToast(false, "No Data Found", false);
                $scope.HUStock = null;
                $scope.Totalrecords = 0;
                $scope.blockUI = false;
                return false;
            }
            $scope.HUStock = dt;
            $scope.Totalrecords = $scope.HUStock[0].TotalRecords;
            $scope.blockUI = false;
        });
    }


    $scope.getDetailsExport = function () {
        debugger;
        if ($scope.HUStock == null || $scope.HUStock == undefined) {
            showStickyToast(false, "No Data Found", false);
            return false;
        }
        if ($("#txtWarehouse").val().trim() == "" || $("#txtWarehouse").val().trim() == undefined || $("#txtWarehouse").val().trim() == null) {
            Warehouseid = 0;
            showStickyToast(false, "Please select Warehouse");
            return false;
        }
        else { Warehouseid = Warehouseid; }

        if ($("#txtTenant").val().trim() == "" || $("#txtTenant").val().trim() == undefined || $("#txtTenant").val().trim() == null) {
            Tenantid = 0;
            showStickyToast(false, "Please select Tenant");
            return false;
        }
        else { Tenantid = Tenantid; }

        if ($("#txtPartNo").val().trim() == "" || $("#txtPartNo").val().trim() == undefined || $("#txtPartNo").val().trim() == null) {
            MMID = 0;
        }
        else { MMID = MMID; }


        var batchNo = $("#txtBatchNo").val().trim();
        var serialNo = $("#txtSerialNo").val().trim();
        var mfgDate = $("#txtMfgDate").val().trim();
        var expDate = $("#txtExpDate").val().trim();
        var ProjectRefNo = $("#txtProjectRefNo").val().trim();
        var MRP = $("#txtMRP").val().trim();
        $scope.ReportType = $("#selReportType").val();

        $scope.blockUI = true;
        var httpreq = {
            method: 'POST',
            url: 'HULiveStock.aspx/getHUStockDetails_Export',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: {
                'WHID': Warehouseid, 'TenantID': Tenantid, 'MID': MMID, 'BatchNo': batchNo, 'SerialNo': serialNo, 'MfgDate': mfgDate, 'ExpDate': expDate, 'ProjectRefNo': ProjectRefNo, 'MRP': MRP, 'ReportType': $("#selReportType").val(), 'PageNo': 1, 'PageSize': 250000
            },
            async: false
        }
        $http(httpreq).success(function (response) {
            debugger
            if (response.d == "No Data Found") {
                showStickyToast(false, "No Data Found", false);
                $scope.blockUI = false;
                return false;
            }
            else {
                window.open('../ExcelData/' + response.d + ".xlsx");
            }
            $scope.blockUI = false;
        });
    }
});