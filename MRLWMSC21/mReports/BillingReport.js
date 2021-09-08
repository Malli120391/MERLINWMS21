var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('BillingReport', function ($scope, $http) {
    $scope.ViewReport = function () {
        //debugger;
        //$scope.CurrencyFunction();
        $scope.from = $("#txtFromdate").val();
        $scope.to = $("#txttodate").val();
        if ($scope.from == "" || $scope.to == "" || RefTenant == "" || WarehouseID == 0 || WarehouseID == null || WarehouseID == undefined) {
            showStickyToast(false,"Please select the Search fields");
            return false;
        }
        $scope.blockUI = true;
        var report = {
            method: 'POST',
            url: 'BillingReport.aspx/getReport',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { FromDate: $scope.from, ToDate: $scope.to, Tenant: RefTenant,Warehouse: WarehouseID}
        }
        $http(report).then(function (response) {
            debugger;
          
            $scope.Data = JSON.parse(response.data.d);
            if ($scope.Data != null || $scope.Data != undefined || $scope.Data != "") {
                $scope.billDetails = true;
                //console.log($scope.Data.Table["Service / Material"]);
                $scope.Total1 = $scope.calcualte($scope.Data.Table);
                $scope.Total2 = $scope.calcualte($scope.Data.Table1);
                $scope.Total3 = $scope.calcualte($scope.Data.Table2);
                $scope.Total4 = $scope.calcualte($scope.Data.Table3);
                $scope.Total5 = $scope.calcualte($scope.Data.Table4);
                $scope.Currency = $scope.Data.Table6[0];
                $scope.TotalCur = $scope.Data.Table7[0];
                $scope.UnitCur = $scope.Data.Table8[0];
                $scope.blockUI = false;
            }
            else {
                showStickyToast(false, "No data avaliable");
               // alert("nO dATA");
                $scope.billDetails = false;
                  $scope.blockUI = false;
            }
           
        });
    }

    $scope.calcualte = function (value) {
      //  debugger;
        var sum = 0
        for (i = 0; i < value.length; i++) {
            sum += parseFloat(value[i].TotalCost);
        }
        return sum;
    }


    $scope.generatePDF = function () {
        //debugger
        $scope.from = $("#txtFromdate").val();
        $scope.to = $("#txttodate").val();
        if ($scope.from == "" || $scope.to == "" || RefTenant == "" || WarehouseID == 0 || WarehouseID == null || WarehouseID == undefined) {
            showStickyToast(false, "Please select the Search fields");
            return false;
        }
        $scope.blockUI = true;
        var report = {
            method: 'POST',
            url: 'BillingReport.aspx/generatePDFData',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { FromDate: $scope.from, ToDate: $scope.to, Tenant: RefTenant, Warehouse: WarehouseID }
        }
        $http(report).then(function (response) {
            debugger;
            if (response.data.d != null) {
                var obj = response.data.d;
                showStickyToast(true, "PDF Generated Successfully ", false);
                window.open('../mOutbound/PackingSlip/' + obj);
                $("#divLoading").hide();
                $scope.blockUI = false;
                return false;

            }
            else {
                showStickyToast(false, "No Data Found ", false);
                $("#divLoading").hide();
                $scope.blockUI = false;
                return false;
            }
        });
    };



    //$scope.CurrencyFunction = function () {

    
    //    if (WarehouseID == 0 || WarehouseID == "" || WarehouseID == undefined) {
    //        showStickyToast(false, "Please Select Warehouse");
    //        return false;
    //    }
    //    debugger;
    //    var report = {
    //        method: 'POST',
    //        url: 'BillingReport.aspx/getCurrencyBasedonWarehouse',
    //        headers: {
    //            'Content-Type': 'application/json; charset=utf-8',
    //            'dataType': 'json'
    //        },
    //        data: { Warehouse: WarehouseID }
    //    }
    //    $http(report).then(function (response) {
    //        debugger;
    //        $scope.Currency = response.data.d;

    //    });
    //}


});