var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('GateList', function ($scope, $http, $timeout) {
    $scope.search = new GateSearch(0, 0);
    var accounts = {
        method: 'POST',
        url: 'GateEntryList.aspx/GetCurrentAccount',
        headers: {
            'Content-Type': 'application/json; charset=utf-8',
            'dataType': 'json'
        },
        data: {}
    }
    $http(accounts).success(function (response) {
        debugger;

        $scope.AccountId = response.d;

    });
  
    var status = {
        method: 'POST',
        url: 'GateEntryList.aspx/GetGateEntryStatus',
        headers: {
            'Content-Type': 'application/json; charset=utf-8',
            'dataType': 'json'
        },
        data: {}
    }
    $http(status).success(function (response) {
        debugger;

        $scope.GateStatus = response.d;

    });

    $scope.getGateEntryListData = function () {
        debugger;
        $scope.blockUI = true;
        if ($scope.search.VehicleId == undefined) {
            $scope.search.VehicleId = 0;
        }

        if ($scope.search.StatusId == undefined) {
            $scope.search.StatusId = 0;
        }
        var data = $scope.search;
        var accounts = {
            method: 'POST',
            url: 'GateEntryList.aspx/getGateEntryListData',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'obj': $scope.search }
        }
        $http(accounts).success(function (response) {
            debugger;
            $scope.blockUI = false;
            $scope.GateList = response.d;
            console.log($scope.GateList);

        });

    }
    $scope.getGateEntryListData();
    $scope.UpdateGateDetails = function (obj) {
        debugger;
        var IsgateIn = 0;
        if (obj.GateEntryCheck == 'Gate In') {
            IsgateIn = 1;
        }
        var accounts = {
            method: 'POST',
            url: 'GateEntryList.aspx/UpdateGateEntryData',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: {'GateId': obj.GateEntryId, 'IsgateIn': IsgateIn}
        }
        $http(accounts).success(function (response) {
            $scope.getGateEntryListData();
            if (response.d == 1) {
                showStickyToast(true, 'Gate Details Added Successfully ');
            }
            else {
                showStickyToast(false, 'Error while adding');
            }
        });

    }
    //----------------------------------- getting Vehicle Data -----------------------------------//
    var textfieldname = $("#txtvehicle");
    DropdownFunction(textfieldname);
    $("#txtvehicle").autocomplete({
        source: function (request, response) {
            debugger;

            if ($("#txtvehicle").val() == '' || $("#txtvehicle").val() == undefined) {
                $scope.search.VehicleId = 0;

            }
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/getVehicleForGateEntry',
                data: "{ 'prefix': '" + request.term + "','TenantId': '" + $scope.AccountId + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.split(',')[0],
                            val: item.split(',')[1],

                        }
                    }))
                }
            });
        },
        select: function (e, i) {

            $scope.search.VehicleId = i.item.val;


        },
        minLength: 0
    });
});
function GateSearch(StatusId, VehicleId) {
    this.StatusId = StatusId;
    this.VehicleId = VehicleId;
}