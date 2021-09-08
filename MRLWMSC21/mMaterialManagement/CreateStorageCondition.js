
var app = angular.module('MyApp', ['angularUtils.directives.dirPagination']);

app.controller('FrmCtrl', function ($scope, $http) {
    $scope.showList = true;
    $scope.showForm = false;
    var tenantid = 0;
    $scope.Getlist = function () {
        var roleid = {
            method: 'POST',
            url: 'CreateStorageCondition.aspx/Getlistdata',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: {}
        }
        $http(roleid).success(function (response) {

            $scope.listdata = response.d;
        });
    }
    $scope.Getlist();
    $scope.storageobj = null;
    $scope.Edit = function (obj) {
        debugger;
        $("#btnCreate").html('Update')
        //$scope.showList = false;
        //$scope.showForm = true;        
        $("#STCModal").modal({
            show: 'true'
        });
        $scope.tenantdata = new storage(obj.TenantID, obj.StorageConditionID, obj.StorageConditionCode, obj.StorageCondition, obj.TenantName);
        tenantid = obj.TenantID;
        $('#stID').val(obj.StorageConditionID);
        accountid = obj.AccountID;
    }
    $scope.Delete = function (obj) {


        if (confirm("Are you sure you want to delete")) {
            var roleid = {
                method: 'POST',
                url: 'CreateStorageCondition.aspx/Delete',
                headers: {
                    'Content-Type': 'application/json; chaeset=utf-8',
                    'dataType': 'json'
                },
                data: { 'obj': obj }

            }
            $http(roleid).success(function (response) {
                if (response.d == "exist") {
                    showStickyToast(false, 'Cannot delete  Storage Condition as item is mapped to this. ', false);
                    return;

                }
                showStickyToast(true, 'Deleted Successfully', false);
                $scope.Getlist();
                setTimeout(function () {
                    location.reload();
                }, 2000);

            });
        }
        else {
            return false;
        }


    }

    $scope.event = function () {
        debugger;
        //$scope.showList = false;
        //$scope.showForm = true;
        $("#STCModal").modal({
            show: 'true'
        });
        $scope.Clear();
        $("#btnCreate").html("Create");
    }


    //$scope.showForm = true;
    var textfieldname = $("#txtTenant");
    DropdownFunction(textfieldname);
    $("#txtTenant").autocomplete({
        source: function (request, response) {
            $.ajax({
               // url: '../mWebServices/FalconWebService.asmx/LoadTenantDataFor3PL',
                url: '../mWebServices/FalconWebService.asmx/LoadTenantDataByUserWH',  // adde by Ganesh @Sep 28 2020 Tenant drop down should be displayed by UserWH
                data: "{'prefix': '" + request.term + "' }",
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
        },
        minLength: 0
    });
    $scope.Clear = function () {
        $scope.tenantdata = null;
        $scope.showList = true;
        $scope.showForm = false;
        tenantid = 0;
        $("#txtTenant").val('');

    }
    $scope.tenantdata = new storage(0, 0, '', '', '', )

    $scope.Submit = function (obj) {

        //$scope.tenantdata.TenantID = tenantid;
        debugger;

        if ($("#txtTenant").val() == "") {
            showStickyToast(false, 'Please select Tenant', false);
            return false;
        }
        if ($("#txtcode").val() == "") {
            showStickyToast(false, 'Please enter storage condition code', false);
            return false;
        }

        obj.TenantID = tenantid;


        if ($("#txtcode").val().trim() != "") {
            debugger;
            var item = $scope.listdata, Count = 0;
            var ID = $('#stID').val();

            if ($('#stID').val() != 0) {

                item = $.grep($scope.listdata, function (data) {
                    return data['StorageConditionID'] != $('#stID').val();
                });
                Count = $.grep(item, function (data) {

                    return data['StorageConditionCode'].toLowerCase() == $('#txtcode').val().toLowerCase().trim() && data['TenantID'] == obj.tenentId;
                });

                if (Count.length != 0) {
                    IsValid = false;
                    showStickyToast(false, 'Storage Condition Code already exists.', false);
                    return false;
                }
            }
            else {

                Count = $.grep($scope.listdata, function (data) {

                    return data['StorageConditionCode'].toLowerCase() == $('#txtcode').val().toLowerCase().trim() && data['TenantID'] == obj.TenantID;
                });

                if (Count.length != 0) {
                    IsValid = false;
                    showStickyToast(false, 'Storage Condition Code already exists.', false);
                    return false;
                }
            }
        }
        var roleid = {

            method: 'POST',
            url: 'CreateStorageCondition.aspx/insert',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'strgobj': $scope.tenantdata }
        }
        $http(roleid).success(function (response) {
            debugger;
            if (response.d == "exist") {
                showStickyToast(false, 'Can not update  as Storage Condition is mapped to Item. ', false);
                return;

            }

            if ($('#btnCreate').text() == "Update") {
                showStickyToast(true, '	Storage Condition  Details Saved Successfully', false);
                $scope.Getlist();
                $scope.showForm = false;
                $scope.showList = true;

                setTimeout(function () {
                    location.reload();
                }, 1000);
            }
            else {
                showStickyToast(true, '	Storage Condition  Details Saved Successfully', false);
                $scope.Getlist();
                setTimeout(function () {
                    location.reload();
                }, 1000);
            }
        });
    }

});
function storage(tntId, strgID, strgcode, strgcon, tntname) {
    this.TenantID = tntId;
    this.StorageConditionID = strgID;
    this.StorageConditionCode = strgcode;
    this.StorageCondition = strgcon;
    this.TenantName = tntname;

}