function isNumber(evt) {
    var iKeyCode = (evt.which) ? evt.which : evt.keyCode
    if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
        return false;
    return true;
}

var Tenantid = 0;
var mmid = 0;

$(document).ready(function () {
    $('#txtTenant').val("");
    var textfieldname = $("#txtTenant");
    DropdownFunction(textfieldname);
    $("#txtTenant").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/GetTenantList',
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
            Tenantid = i.item.val;

        },
        minLength: 0
    });



    $('#txtPartnumber').val("");
    var textfieldname = $("#txtPartnumber");
    DropdownFunction(textfieldname);
    $("#txtPartnumber").autocomplete({
        source: function (request, response) {
            if (Tenantid == 0 || Tenantid == "0" || Tenantid == undefined || Tenantid == null) {
                showStickyToast(false, 'Please select Tenant');
                return false;
            }
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/LoadMaterialsForCurrentStock',
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
            mmid = i.item.val;
        },
        minLength: 0
    });
});

var myApp = angular.module('OBDSkipApp', ['angularUtils.directives.dirPagination']);
myApp.controller('SkipItem', function ($scope, $http) {
    $scope.Skipdata = [];

    $scope.GetSkipData = function () {

        if (Tenantid == 0 || Tenantid == undefined) {
            showStickyToast(false,"Please Select the Tenant");
            return;
        }
        if ($scope.type == undefined || $scope.type == null) {
            showStickyToast(false,"Please Select the Report type");
            return;
        }
        $scope.blockUI = true;
        var skip = {
            method: 'POST',
            url: 'SkipLog.aspx/getSkipData',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'TenantID': Tenantid, 'Type': $scope.type, 'Material': mmid, 'Fromdate': $("#txtFromDate").val(), 'Todate': $("#txtToDate").val()}
        }
        $http(skip).success(function (response) {
            debugger;
            var data = JSON.parse(response.d);
            data.Table.forEach(function (value) {
                value.Isselected = "";
            });
            $scope.Skipdata = data.Table;
            $scope.blockUI = false;

        });
    }

    //$scope.GetSkipData();
 
    $scope.downloadExcel = function () {
        if (Tenantid == 0 || Tenantid == undefined) {
            showStickyToast(false, "Please Select the Tenant");
            return;
        }
        if ($scope.type == undefined || $scope.type == null) {
            showStickyToast(false, "Please Select the Report type");
            return;
        }
        $scope.blockUI = true;
        var skip = {
            method: 'POST',
            url: 'SkipLog.aspx/DownloadExcelForLog',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'TenantID': Tenantid, 'Type': $scope.type, 'Material': mmid, 'Fromdate': $("#txtFromDate").val(), 'Todate': $("#txtToDate").val() }
        }
        $http(skip).success(function (response) {
            debugger;
            if (response.d == "0") {
                showStickyToast(false, "Error while downloading");
            }
            else {
                window.open('../ExcelData/' + response.d + '.xlsx');
                
            }
            $scope.blockUI = false;
        });
    }


    $scope.ClearList = function () {
        
        $scope.Skipdata = [];
    }

    $scope.ChangeSelectALL = function () {
        debugger;
        for (var i = 0; i < $scope.Skipdata.length; i++) {
            $scope.Skipdata[i].Isselected = $scope.SelectALL;
        }

    }

    $scope.UpdateSkipItemLog = function () {
        debugger;
        var temp = [];
        temp = $scope.Skipdata.filter(function (value) {
            return value.Isselected == true;
        });
        if ($scope.type == undefined || $scope.type == null) {
            showStickyToast(false, "Please Select the Report type");
            return;
        }
        if (temp.length == 0) {
            showStickyToast(false, "Please select atleast one Item ");
            return;
        }

        var obj=[] ;
        temp.forEach(function (value) {
            var t = [{ MaterialID: 0, LocationId: 0, SkipQuantity: 0 }];
            t[0].MaterialID = value.MaterialMasterId;
            t[0].LocationId = value.LocationId;
            t[0].SkipQuantity = value.SkipQuantity;
            obj = obj.concat(t);
        });

        var clear = {
            method: 'POST',
            url: 'SkipLog.aspx/ClearLog',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'Type': $scope.type, 'SkipClear':obj }
        }
        $http(clear).success(function (response) {
            if (response.d == "1") {
                showStickyToast(true, "Successfully Cleared from Skip Log");
                $scope.GetSkipData();
            }
            else {
                showStickyToast(false, "Error while clearing Skip Log");
            }
        });

    }
});
