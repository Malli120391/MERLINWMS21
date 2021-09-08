var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('ContainerManagement', function ($scope, $http) {
    $scope.getData = function () {
        debugger;
        if ($("#txtWareHouse").val() === "")
        {
            $("#hifWareHouse").val("0");
            showStickyToast(false, "Please select Warehouse", false);
            return false;
        }
        if ($("#txtContainertype").val() === "") {
            $("#hifContainertype").val("0");
            showStickyToast(false, "Please select Container Type", false);
            return false;
        }
        $("#divLoading").show();
        var httpreq = {
            method: 'POST',
            url: 'ContainerManagement.aspx/getContainerData',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'WHID': $("#hifWareHouse").val(), 'ConTID': $("#hifContainertype").val() },
            async: false
        }
        $http(httpreq).success(function (response) {
            debugger;
            var dt = JSON.parse(response.d).Table;
            $scope.ContainerData = dt;
            $("#divLoading").hide();
            //if (dt === undefined || dt === null || dt.length === 0) {
            //    $scope.ContainerData = null;
            //   // showStickyToast(false, "No Data Found", false);
            //    return false;
            //}
            //else {
            //    $scope.ContainerData = dt;
            //}
        });
    };
    //$scope.getData();

    $scope.isChecked = 0;
    $scope.selectAllCheckBoxs = function () {
        $scope.isChecked = !$scope.isChecked;
        $('.checkselectall').attr('checked', $scope.isChecked);
    };


    $scope.getPrintObjects = function () {
        debugger;
        $scope.PrintData = [];

        $(".checkedone").each(function () {
            if ($(this).is(":checked") === true) {
                var dt = "";
                dt = JSON.parse(JSON.stringify($(this).attr("data-obj")));
                $scope.PrintData.push(JSON.parse(dt));
            }
        });

        if ($('#pid').val() == "3") {
            if ($("#netPrinterHost").val() == "") {
                showStickyToast(false, "Please enter Printer IP Address", false);
                return false;
            }
            if ($("#netPrinterPort").val() == "") {
                showStickyToast(false, "Please enter Printer Port", false);
                return false;
            }
        }

        var httpreq = {            
            method: 'POST',
            url: 'ContainerManagement.aspx/GetPrint',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'printobj': $scope.PrintData, 'PrinterIP': $("#netPrinterHost").val(), 'PortID': $("#netPrinterPort").val()},
            async: false,
        }
        $http(httpreq).success(function (response) {
            debugger;
            var dt = response.d;
            if (dt == "") {
                showStickyToast(false, "Error occured", false);
                return false;
            }
            else if (dt == "Success") {
                showStickyToast(true, "Successfully Printed", false);
            }
            else {
                $("#printerCommands").val("");
                $("#printerCommands").val(dt);
                javascript: doClientPrint();
                showStickyToast(true, "Successfully Printed", false);
            }
        });
    };

});