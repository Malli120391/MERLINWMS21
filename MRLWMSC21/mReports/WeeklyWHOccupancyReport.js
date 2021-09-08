var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('WeeklyWHOccupancyReport', function ($scope, $http) {

    var fromDate = ""; toDate = ""; WarehouseID = 0; TenantID = 0; RefTenant = 0;

    $('#txtTenant').val("");
    var textfieldname = $("#txtTenant");
    DropdownFunction(textfieldname);
    $("#txtTenant").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/LoadTenantsForReports',
                data: "{ 'prefix': '" + request.term + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.split(',')[1],
                            val: item.split(',')[0]
                        }
                    }))
                }
            });
        },
        select: function (e, i) {
            RefTenant = i.item.val;
        },
        minLength: 0
    });

    var textfieldname = $("#txtWarehouse");
    DropdownFunction(textfieldname);
    $("#txtWarehouse").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/LoadWarehousesBasedonTenant',
                data: JSON.stringify({ 'prefix': request.term, 'tenantID': RefTenant }),
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
            WarehouseID = i.item.val
        },
        minLength: 0
    });

    $scope.Getgedetails = function () {
        //debugger
        if ($("#txtTenant").val() == "") { TenantID = 0; $("#txtTenant").val(""); } else { TenantID = RefTenant; }
        if ($("#txtWarehouse").val() == "") { WarehouseID = 0; $("#txtWarehouse").val(""); } else { WarehouseID = WarehouseID; }

        if ($("#txtFromdate").val() == "") {
            showStickyToast(false, "Please select From Date", false);
            fromDate = "";
            return false;
        }
        if ($("#txttodate").val() == "") {
            showStickyToast(false, "Please select To Date", false);
            toDate = "";
            return false;
        }

        if ($("#txtFromdate").val() == "") { fromDate = ""; } else { fromDate = $("#txtFromdate").val(); }
        if ($("#txttodate").val() == "") { toDate = ""; } else { toDate = $("#txttodate").val(); }

        $scope.blockUI = true;
        var httpreq = {
            method: 'POST',
            url: 'WeeklyWHOccupancyReport.aspx/getWeeklyWHOccReport',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { FromDate: fromDate, ToDate: toDate, Tenant: TenantID, Warehouse: WarehouseID },
            async: false
        }
        $http(httpreq).success(function (response) {
            debugger;

            var dt = JSON.parse(response.d).Table;
            if (dt == undefined || dt == null || dt.length == 0) {
                showStickyToast(false, "No Data Found", false);
                $scope.WHData = null;
                $scope.TenantData = null;
                $scope.blockUI = false;
                return false;
            }
            $scope.WHData = dt;
            $scope.TenantData = JSON.parse(response.d).Table1;

            //$scope.WHData = JSON.parse(response.d).Table;
            //$scope.TenantData = JSON.parse(response.d).Table1;
            //$scope.Header = Object.keys($scope.WHData[0]);
            //$scope.tenantData = $scope.getUnique($scope.WHData, 'TenantID');
            //debugger;
            //$scope.HeaderData = $.grep($scope.WHData, function (a) { return a.week == "week" })
            $scope.blockUI = false;
        })
    };
    //$scope.Getgedetails();

    $scope.getData = function (weekType, Tenant, colType) {
        //debugger
        var item = "";
        var dt = $.grep($scope.WHData, function (a) { return a.Week == weekType && a.TenantID == Tenant });
        if (dt.length > 0) {
            item = dt[0][colType];
            if (item != null) {
                item = dt[0][colType];
            } else { item = "0";}
        }
        else {
            item = "0";
        }
        return item;
    };

    $scope.getOccPercentage = function (Tenant, colType) {
        //debugger
        var item = "";
        var dt = $.grep($scope.WHData, function (a) { return a.Week != "Tariffs" && a.TenantID == Tenant });
        if (dt.length > 0) {
            item = $scope.sum(dt, 'OccupiedCBMPercentage');
        }
        else {
            item = "0";
        }
        return item;
    };

    $scope.getTotalCharges = function (Tenant, colType) {
        //debugger
        var item = "";
        var dt = $.grep($scope.WHData, function (a) { return a.TenantID == Tenant });
        if (dt.length > 0) {
            item = $scope.sum(dt, 'Charge');
        }
        else {
            item = "0";
        }
        return item;
    };

    $scope.sum = function (items, prop) {
        return items.reduce(function (a, b) {
            return a + b[prop];
        }, 0);
    };


    $scope.getUnique = function (arr, comp) {
        const unique = arr.map(e => e[comp]).
            map((e, i, final) => final.indexOf(e) === i && i)
            .filter((e) => arr[e]).map(e => arr[e]);
        return unique
    };
});