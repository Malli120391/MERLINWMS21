
var MyApp = angular.module('MyApp', ['xlsx-model']);
MyApp.controller('Stock', function ($scope, $http) {
    //alert('hi');
    $scope.ImportStockData = function (Data) {
        debugger;
        if (Data == undefined) {
            showStickyToast(false, "Please Import Valid File");
            return false;
        }
        var filename = JSON.stringify(Object.keys(Data));
        var excelname = JSON.parse(filename.replace(/(\{|,)\s*(.+?)\s*:/g, '$1 "$2":'));
        var InwardImport = '';
        // var inbdata = data[excelname].InwardImport;
        // $scope.inbdata = data[excelname].InwardImport;
        $scope.stockdata = Data[excelname].Sheet1;
    }
  
    $scope.CreateStock = function () {
        debugger;
        if ($scope.stockdata == undefined) {
            showStickyToast(false, "Please Upload Excel Sheet");
        }
        var http = {
            method: 'POST',
            url: 'StockImport.aspx/GetStockImport',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'Stklst': $scope.stockdata }
        }
        $http(http).success(function (response) {
            //
            debugger;
            var result = response.d;
            if (response.d == "1") {
                showStickyToast(true, "Successfully Imported");
                angular.element("input[type='file']").val(null);
                $scope.stockdata = [];
                return;
            }
            if (response.d == "0") {
                showStickyToast(false, "UnExpected Error");
                angular.element("input[type='file']").val(null);
                $scope.stockdata = [];
                return;
            }
            if (response.d == "-6") {
                showStickyToast(false, "Error While Inserting");
                angular.element("input[type='file']").val(null);
                $scope.stockdata = [];
                return;
            }
            if (response.d == "-8") {
                showStickyToast(false, "Please Enter Valid data");
                angular.element("input[type='file']").val(null);
                $scope.stockdata = [];
                return;
            }
            else {

                showStickyToast(false, response.d);
                angular.element("input[type='file']").val(null);
                $scope.stockdata = [];
                return;
            }


          

        });
  
    }



});