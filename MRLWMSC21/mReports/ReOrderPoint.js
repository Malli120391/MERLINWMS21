var app = angular.module('MyApp', ['angularUtils.directives.dirPagination']);
app.controller('FormCtrl', function ($scope, $http) {

    var RefTenant = '';
    var WarehouseID = '';
    var Refpartno = 0;
    var MTypeId = 0;
    var Mgroup = '';

    $scope.noofrecords = 50;


    $('#txtTenant').val("");
    var textfieldname = $("#txtTenant");
    DropdownFunction(textfieldname);
    $("#txtTenant").autocomplete({
        source: function (request, response) {
            $.ajax({
                //url: '../mWebServices/FalconWebService.asmx/GetTenantList',
                // data: "{ 'prefix': '" + request.term + "'}",
                url: '../mWebServices/FalconWebService.asmx/LoadTenantsByWH',
                data: "{ 'prefix': '" + request.term + "','whid':'" + WarehouseID + "' }",
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
            //Tenantid = i.item.val;
            RefTenant = i.item.val;
        },
        minLength: 0
    });


    var textfieldname = $("#txtWarehouse");
    DropdownFunction(textfieldname);
    $("#txtWarehouse").autocomplete({
        source: function (request, response) {
            //if ($("#txtTenant").val() == '') {
            //   showStickyToast(false, "Please select Tenant");
            // return false;
            //}
            $.ajax({
                // url: '../mWebServices/FalconWebService.asmx/LoadWarehousesBasedonTenant',
                //  data: JSON.stringify({ 'prefix': request.term, 'tenantID': RefTenant }),
                url: '../mWebServices/FalconWebService.asmx/LoaDWHListBasedonUser',
                data: "{ 'prefix': '" + request.term + "'  }",
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
            $('#txtTenant').val("");
        },
        minLength: 0
    });


    $('#txtMcode').val("");
    var textfieldname = $("#txtMcode");
    DropdownFunction(textfieldname);
    $("#txtMcode").autocomplete({
        source: function (request, response) {
            debugger;
            if (RefTenant == 0 || RefTenant == "0" || RefTenant == undefined || RefTenant == null) {
                showStickyToast(false, 'Please select Tenant');
                return false;
            }
            if ($("#txtMcode").val() == '') {
                Refpartno = 0;
            }
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/LoadMaterialsForCurrentStock',
                data: "{ 'prefix': '" + request.term + "','TenantID':'" + RefTenant + "'}",
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
            Refpartno = i.item.val;
        },
        minLength: 0
    });


    $("#txtMType").val("");
    var textfieldname = $("#txtMType");
    DropdownFunction(textfieldname);
    $("#txtMType").autocomplete({
        source: function (request, response) {
            if (RefTenant == 0 || RefTenant == "0" || RefTenant == undefined || RefTenant == null) {
                showStickyToast(false, 'Please select Tenant');
                return false;
            }
            if ($("#txtMType").val() == '') {
                Refmtype = 0;
            }
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/LoadMaterialTypesForCurrentStock',
                data: "{ 'prefix': '" + request.term + "','TenantID':'" + RefTenant + "'}",
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
            MTypeId = i.item.val;
        },
        minLength: 0
    });

    $("#txtMaterialGroup").val("");
    var textfieldname = $("#txtMaterialGroup");
    DropdownFunction(textfieldname);
    $("#txtMaterialGroup").autocomplete({
        source: function (request, response) {
            if (RefTenant == 0 || RefTenant == "0" || RefTenant == undefined || RefTenant == null) {
                showStickyToast(false, 'Please select Tenant');
                return false;
            }
            if ($("#txtMaterialGroup").val() == '') {
                Refmtype = 0;
            }
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/LoadMaterialGroupDataItem',
                data: "{ 'prefix': '" + request.term + "','TenantID':'" + RefTenant + "'}",
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
            Mgroup = i.item.val;
        },
        minLength: 0
    });


    $scope.Getgedetails = function (PaginationId) {
       
        if ($("#txtWarehouse").val() == '') {
            showStickyToast(false, "Please select Warehouse");
            return false;
        }

        if ($("#txtTenant").val() == '') {
            showStickyToast(false, "Please select Tenant");
            return false;
        }


        if ($("#txtMaterialGroup").val() == '') {
            Mgroup = 0;
        }

        if ($("#txtMType").val() == '') {
            MTypeId = 0;
        }

        if ($("#txtMcode").val() == '') {
            Refpartno = 0;
        }


        var pagesize = $scope.noofrecords;
        $scope.blockUI = true;
        $('inv-preloader').show();
        var billing = {
            method: 'POST',
            url: 'ReOrderPoint.aspx/GETOBData',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'TenantID': RefTenant, 'WHID': WarehouseID, 'MtypeID': MTypeId, 'MgroupId': Mgroup, 'MMId': Refpartno, 'PaginationId': PaginationId, 'PageSize': pagesize }
        }
        $http(billing).success(function (response) {
            debugger;
            //document.getElementById('loader').style.display = 'none';
            $scope.blockUI = false;
            $('inv-preloader').hide();
            var dt = JSON.parse(response.d);
            $scope.BillingData = dt.Table;
            if ($scope.BillingData.length != 0) {
                $scope.Totalrecords = dt.Table[0].TotalRecords;
            }
            else {
                $scope.Totalrecords = 0;
                showStickyToast(false, 'No Data Found', false);
            }

            
        });
    }

    $scope.exportExcel = function () {
        debugger;
        $scope.blockUI = true;
        $('inv-preloader').show();
        var billing = {
            method: 'POST',
            url: 'ReOrderPoint.aspx/ExportExcel',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'TenantID': RefTenant, 'WHID': WarehouseID, 'MtypeID': MTypeId, 'MgroupId': Mgroup, 'MMId': Refpartno }
        }
        $http(billing).success(function (response) {
            debugger;
            //document.getElementById('loader').style.display = 'none';
            window.open('../ExcelData/' + response.d + '.xlsx');
            $scope.blockUI = false;
            $('inv-preloader').hide();


        });
    }

});