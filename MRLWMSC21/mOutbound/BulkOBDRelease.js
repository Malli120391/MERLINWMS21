var app = angular.module('MyApp', ['angularUtils.directives.dirPagination']);
app.controller('FormCtrl', function ($scope, $http) {


    var RefTenant = '';
    var WarehouseID = '';
    var SOHID = '';
    var OBDID = '';
    var CustID = '';
    var DeliveryStatusID = '';
    var DockID = '';

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

    var textfieldname = $("#txtSO");
    DropdownFunction(textfieldname);
    $("#txtSO").autocomplete({
        source: function (request, response) {
          
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/LoadSONOS',
                data: JSON.stringify({ 'prefix': request.term, 'warehouseid': + WarehouseID  }),
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
            SOHID = i.item.val
        },
        minLength: 0
    });


    var textfieldname = $("#txtOBD");
    DropdownFunction(textfieldname);
    $("#txtOBD").autocomplete({
        source: function (request, response) {

            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/LoadOBDNOS',
               // data: JSON.stringify({ 'prefix': request.term }),
                data: "{ 'prefix': '" + request.term + "','TenantId':'" + RefTenant + "' }",
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
            OBDID = i.item.val
        },
        minLength: 0
    });


    var textfieldname = $("#txtCustomer");
    DropdownFunction(textfieldname);
    $("#txtCustomer").autocomplete({
        source: function (request, response) {

            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/LoadCustomers',
                data: JSON.stringify({ 'prefix': request.term }),
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
            CustID = i.item.val
        },
        minLength: 0
    });

    var textfieldname = $("#txtPriority");
    DropdownFunction(textfieldname);
    $("#txtPriority").autocomplete({
        source: function (request, response) {

            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/LoadPriority',
                data: JSON.stringify({ 'prefix': request.term }),
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {

                    response($.map(data.d, function (item) {
                        return {
                            label: item.split(',')[0],
                            
                        }
                    }))
                }
            });
        },
        select: function (e, i) {
            OBDID = i.item.label
        },
        minLength: 0
    });

    var textfieldname = $("#txtDeliveryStatus");
    DropdownFunction(textfieldname);
    $("#txtDeliveryStatus").autocomplete({
        source: function (request, response) {

            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/LoadOBDStatus',
                data: JSON.stringify({ 'prefix': request.term }),
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
            DeliveryStatusID = i.item.val
        },
        minLength: 0
    });

    var textfieldname = $("#txtdock");
    DropdownFunction(textfieldname);
    $("#txtdock").autocomplete({
        source: function (request, response) {

            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/LoadOutboundDocks',
                data: JSON.stringify({ 'prefix': WarehouseID }),
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
            DockID = i.item.val
        },
        minLength: 0
    });


    

    $scope.SelectAll = function (obj) {

        debugger;
        if (document.getElementById("parent").checked) {

            var List = $scope.BillingData;
            for (var index = 0; index < $scope.BillingData.length; index++) {
                $scope.BillingData[index].IsSelected = true;
                //$scope.List[index].IsSelected = true;

            }
        } else {
            var List = $scope.BillingData;

            for (var index = 0; index < $scope.BillingData.length; index++) {

                $scope.BillingData[index].IsSelected = false;

            }
        }

    }



    $scope.Getgedetails = function (PaginationId) {
        debugger
        var fromdate = $('#txtFromDate').val();
        var Todate = $('#txtToDate').val();
        var Duedate = $('#txtDueDate').val();
        var priority = $('#txtPriority').val();

        if ($("#txtTenant").val() == '') {
            RefTenant = 0;
        }
        if ($("#txtWarehouse").val() == '') {
            showStickyToast(false, "Please select Warehouse");
            return false;
        }
        if ($("#txtOBD").val() == '') {
            OBDID = 0;
        }
        if ($("#txtSO").val() == '') {
            SOHID = 0;
        }
        if ($("#txtCustomer").val() == '') {
            CustID = 0;
        }
        if ($("#txtDeliveryStatus").val() == '') {
            showStickyToast(false, "Please select Delivery Status");
            return false;
        }


        //document.getElementById('loader').style.display = 'block';
        var pagesize = $scope.noofrecords;
        $scope.blockUI = true;
        $('inv-preloader').show();
        var billing = {
            method: 'POST',
            url: 'BulkOBDRelease.aspx/GETOBData',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'FromDate': fromdate, 'ToDate': Todate, 'TenantID': RefTenant, 'WHID': WarehouseID, 'OBDID': OBDID, 'priority': priority, 'DueDate': Duedate, 'SOHID': SOHID, 'CustomerID': CustID, 'PaginationId': PaginationId, 'PageSize': pagesize, 'StatusID': DeliveryStatusID }
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

            if ($("#txtDeliveryStatus").val() == 'Sent to Store') {
                $scope.DisplayReleaseData = false;
            }
            else {
                $scope.DisplayReleaseData = true;
            }

           
        

        });
    }
    //$scope.Getgedetails(1);

    

    $scope.Dock = function () {
        debugger;
        if ($("#txtDeliveryStatus").val() == 'Sent to Store') {
            $scope.DisplayReleaseData = true;
        }
        else {
            $scope.DisplayReleaseData = false;
        }
    }

    $scope.Dock();

    $scope.BulkRelease = function () {
        debugger;
        
        var list = $.grep($scope.BillingData, function (BillingData) {
            return (BillingData.IsSelected == true);
        });
        if (list.length == 0) {
            showStickyToast(false, 'Please Select atleast one Outbound', false);
            $scope.blockUI = false;
            return;
        }

        
        

        if ($("#txtdock").val() == '' && $("#txtDeliveryStatus").val() == 'Sent to Store') {
            showStickyToast(false, "Please select Dock");
            return false;
        }
        if ($("#txtdock").val() == '' && $("#txtDeliveryStatus").val() == 'On Hold') {
            DockID = 0;
        }
        
        $scope.blockUI = true;
        $('inv-preloader').show();
        var billing = {
            method: 'POST',
            url: 'BulkOBDRelease.aspx/BulkRelease',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'oBDDatas': list, 'DockId': DockID }
        }
        $http(billing).success(function (response) {
            debugger;
            window.open('../ExcelData/' + response.d + '.xlsx');
            $scope.blockUI = false;
            $('inv-preloader').hide();
            $scope.Getgedetails(1);
        }).catch(function (reason) {
            debugger;
            showStickyToast(false, 'Unexpected error, please contact support');
            console.log(reason);
            $scope.blockUI = false;
            $('inv-preloader').hide();
            $scope.Getgedetails(1);
        });
    }


   
});