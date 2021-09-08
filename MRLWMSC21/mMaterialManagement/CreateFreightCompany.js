var app = angular.module('MyApp', ['angularUtils.directives.dirPagination']);
app.controller('FrmCtrl', function ($scope, $http) {
    $scope.showList = true;
    $scope.showForm = false;
    var accountid = 0;
    //Account filters
    var textfieldname = $("#txtAccount");
    DropdownFunction(textfieldname);
    $("#txtAccount").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/LoadAccountDataFor3PL',
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
            accountid = i.item.val;
        },
        minLength: 0
    });

    var roleid = {

        method: 'POST',
        url: 'CreateFreightCompany.aspx/getlist',
        headers: {
            'Content-Type': 'application/json; charset=utf-8',
            'dataType': 'json'
        },
        data: { 'objcom': $scope.data }
    }
    $http(roleid).success(function (response) {
        debugger;
        $scope.Listdata = response.d;


    });
    $scope.Clear = function () {

        $scope.data = null;
        $scope.showList = true;
        $scope.showForm = false;
        $("#txtAccount").val("");
    }
    $scope.Edit = function (obj) {
        debugger;
        $("#Container").modal({
            show: 'true'
        });
        $scope.data = new dbparameters(obj.FreightCompanyCode, obj.FreightCompany, obj.FreightCompanyID);
       // $scope.showList = false;
       // $scope.showForm = true;
        $("#btnCreate").html("Update");
        $('#stID').val(obj.FreightCompanyID);
        $("#txtAccount").val(obj.AccountCode);
        accountid = obj.AccountID;

    }

    $scope.Delete = function (obj) {
        debugger;
        if (confirm("Are you sure do you want to delete ?")) {
            var roleid = {
                method: 'POST',
                url: 'CreateFreightCompany.aspx/delete',
                headers: {
                    'content-Type': 'application/json; charset=utf-8',
                    'dataType':'json'
                },
                data: { 'obj': obj }
            }
            $http(roleid).success(function (response) {
                if (response.d == "Exist") {
                    showStickyToast(false, 'Could not delete as Vehicle is mapped with this freight company.', false);
                    return;
                }
                showStickyToast(true, 'Deleted Successfully', false);
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
        $("#Container").modal({
            show: 'true'
        });
       // $scope.showList = false;
       // $scope.showForm = true;
        $("#btnCreate").html("Create");
    }

    $scope.data = new dbparameters('', '',0);
    $scope.Submit = function (obj) {
        
        debugger;
        $scope.data = obj;
        if ($("#txtAccount").val() == "") {

            showStickyToast(false, 'Please Select Account', false)
            return false;
        }
        if ($("#txtcode").val() == "")
        {

            showStickyToast(false, 'Please Enter Freight CompanyCode', false)
            return false;
        }
        if ($("#txtcompany").val() =="") {
         
            showStickyToast(false, 'Please Enter FreightCompany Name', false)
            return false;
        }
        if ($("#txtcode").val().trim() != "") {
            debugger;
            item = $scope.Listdata; var count = 0;

            if ($('#stID').val() != 0) {

                item = $.grep($scope.Listdata, function (data) {
                    return data['FreightCompanyID'] != $('#stID').val();
                });
            }

            Count = $.grep(item, function (data) {

                return data['FreightCompanyCode'].toLowerCase() == $('#txtcode').val().toLowerCase().trim();
            });

            if (Count.length != 0) {
                IsValid = false;
                showStickyToast(false, 'Freight CompanyCode already exists.', false);
                return false;
            }

        }
        var roleid = {

            method: 'POST',
            url: 'CreateFreightCompany.aspx/Insert',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'objcom': $scope.data, 'AccountID': accountid }
        }
        $http(roleid).success(function (response) {
            if (response.d == "Exist") {
                showStickyToast(false, 'Could not update as Vehicle is mapped with this.', false);
                return;
            }
            if ($('#btnCreate').text() == "Update") {
                showStickyToast(true, '	Freight Company Updated Successfully', false);
                $scope.showForm = false;
                $scope.showList = true;

                setTimeout(function () {
                    location.reload();
                }, 2000);
            }
            else {
                showStickyToast(true, '	Freight Company Created Successfully', false);
                $scope.showForm = false;
                $scope.showList = true;

                setTimeout(function () {
                    location.reload();
                }, 2000);
            }
        });
    }

  });
function dbparameters(code, company, ID) {
    
    this.FreightCompanyCode = code;
    this.FreightCompany = company;
    this.FreightCompanyID = ID;
}