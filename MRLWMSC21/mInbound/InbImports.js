
var MyApp = angular.module('MyApp', ['xlsx-model']);
MyApp.controller('createinbound', function ($scope, $http) {
    //alert('hi');

    $scope.ImportData = function (data) {
        //
        debugger;
        if (data == undefined || data == '' || data == null) {
            showStickyToast(false, "Please Upload valid File");
        }

        var filename = JSON.stringify(Object.keys(data));
        var excelname = JSON.parse(filename.replace(/(\{|,)\s*(.+?)\s*:/g, '$1 "$2":'));
        //var InwardImport = '';
        $scope.inbdata = data[excelname].InwardImport;

        if ($scope.inbdata == undefined || $scope.inbdata == '' || $scope.inbdata == null) {
            showStickyToast(false, "Please Upload valid File");
        }
        //This loop is for getting PO Number,PO Quantity and Supplier Code if not exists in excel Sheet
        $.each($scope.inbdata, function (index) {
            debugger;
            if ($scope.inbdata[index].POQty == undefined || $scope.inbdata[index].POQty == null || $scope.inbdata[index].POQty == '') {
                $scope.inbdata[index].POQty = $scope.inbdata[index].InvoiceQuantity;
            }
            if ($scope.inbdata[index].SupplierCode == undefined || $scope.inbdata[index].SupplierCode == null || $scope.inbdata[index].SupplierCode == '') {
                $scope.inbdata[index].SupplierCode = "";//$scope.inbdata[index].TenantCode;
            }
            if ($scope.inbdata[index].PODate == undefined || $scope.inbdata[index].PODate == null || $scope.inbdata[index].PODate == '') {
                $scope.inbdata[index]['PODate(dd/MM/yyyy)'] = $scope.inbdata[index]['InvoiceDate(dd/MM/yyyy)'];
            }
            //call procedure SP_GetNewPO_ByTenantCode for getting New PO Number
            if ($scope.inbdata[index].PONumber == undefined || $scope.inbdata[index].PONumber == null || $scope.inbdata[index].PONumber == '') {
                $scope.blockUI = true;
                var http = {
                    method: 'POST',
                    url: '../mInbound/InbImports.aspx/getPONumber',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'TenantCode': $scope.inbdata[index].TenantCode }
                }
                $http(http).success(function (response) {
                    $scope.blockUI = false;
                    var obj = JSON.parse(response.d);
                    var table = obj.Table[0];
                    if (response.d.length != 0 && response.d != undefined) {
                        $scope.inbdata[index].PONumber = table.PONumber;
                        //$scope.inbdata[index].PODate = $scope.inbdata[i]['InvoiceDate(dd/MM/yyyy)'];table.PODate;
                        $scope.inbdata[index]['PODate(dd/MM/yyyy)'] = table.PODate;
                    }
                });
            }
        })
    }

    $scope.CreateInbound = function () {
        debugger;
        if ($scope.inbdata == undefined || $scope.inbdata == "" || $scope.inbdata == null) {
            showStickyToast(false, "Please Upload Excel Sheet");
            return false;
        }
        for (var i = 0; i < $scope.inbdata.length; i++) {
            $scope.inbdata[i].PODate = $scope.inbdata[i]['PODate(dd/MM/yyyy)'];
            $scope.inbdata[i].InvoiceDate = $scope.inbdata[i]['InvoiceDate(dd/MM/yyyy)'];
            $scope.inbdata[i].MfgDate = $scope.inbdata[i]['MfgDate(dd/MM/yyyy)'];
            $scope.inbdata[i].ExpDate = $scope.inbdata[i]['ExpDate(dd/MM/yyyy)'];
        }

        $scope.blockUI = true;
        var http = {
            method: 'POST',
            url: '../mInbound/InbImports.aspx/GetInwardImport',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'inblst': $scope.inbdata }
        }
        $http(http).success(function (response) {
            $scope.blockUI = false;
            $scope.inbdata = "";
            $('#filetype').val("");

            if (response.d == "" || response.d == undefined || response.d == undefined == null) {
                showStickyToast(false, 'Error While Importing , Try again ');
                return false;
            }
            else {
                var obj = response.d;
                if (obj.Status == 1) {
                    showStickyToast(true, "Inbound Created successfully.");
                }
                else if (obj.Status == -1) {

                    showStickyToast(false, obj.Result);
                }
                else {
                    showStickyToast(false, obj.Result);
                }
            }

        });
    }

});