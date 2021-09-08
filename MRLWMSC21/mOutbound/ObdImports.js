var MyApp = angular.module('MyApp', ['xlsx-model']);
MyApp.controller('createoutbound', function ($scope, $http) {
    //alert('hi');

    $scope.ImportData = function (data) {
        //
        if (data == undefined || data == null || data == '') {
            showStickyToast(false, 'Please enter valid file');
            return;
        }
        $("#fuExcel").val(null);
        $("#filetype").val(null);
        $scope.excel = null;
        debugger;
        var filename = JSON.stringify(Object.keys(data));
        var excelname = JSON.parse(filename.replace(/(\{|,)\s*(.+?)\s*:/g, '$1 "$2":'));
        var OutwardImport = '';
        // var inbdata = data[excelname].InwardImport;
        $scope.obdbdata = data[excelname].OutwardImport;
        if ($scope.obdbdata == undefined || $scope.obdbdata == null || $scope.obdbdata == '') {
            showStickyToast(false, 'Please enter valid file');
            return;
        }

        //$http(httpWH).success(function (response) {
        //    
        //    $scope.GETINBList = response.d;



        //}); 
    }

    $scope.CreateOutbound = function () {
        debugger;
        if ($scope.obdbdata == undefined) {
            showStickyToast(false, "Please upload Excel Sheet");
            return false;
        }
        var http = {
            method: 'POST',
            url: '../mOutbound/ObdImports.aspx/GetOutwardImport',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'obdlst': $scope.obdbdata }
        }
        $http(http).success(function (response) {
            //
            debugger;
            if (response.d.indexOf('Error') != -1) {
                showStickyToast(false, response.d);

                return false;
            }
            else {
                showStickyToast(true, response.d);
                $scope.obdbdata = null;
                $scope.excel = null;
                $("#fuExcel").val(null);
                $("#filetype").val(null);
                $scope.excel = null;
                return false;
            }

            //$scope.inbdata = response.d;

            //if (response.d == "1") {
            //    showStickyToast(true, "Outbound Created successfully.");
            //    setTimeout(function () {    
            //        location.reload();
            //    }, 2000);
            //}
            //else if (response.d == "0") {
            //    showStickyToast(false, "ERROR WHILE UPLOADING");
            //    setTimeout(function () {
            //        location.reload();
            //    }, 2000);
            //}
            //else if (response.d == "-1") {
            //    showStickyToast(false, "Please Enter Mandatory Mfg. Date");
            //    setTimeout(function () {
            //        location.reload();
            //    }, 2000);
            //}
            //else if (response.d == "-2") {
            //    showStickyToast(false, "Please Enter Mandatory Exp. Date");
            //    setTimeout(function () {
            //        location.reload();
            //    }, 2000);
            //}
            //else if (response.d == "-3") {
            //    showStickyToast(false, "Please Enter Mandatory Batch No");
            //    setTimeout(function () {
            //        location.reload();
            //    }, 2000);
            //}

        });
        //$http(http).error(function (response) {
        //    alert(response.text);
        //});
    }

});