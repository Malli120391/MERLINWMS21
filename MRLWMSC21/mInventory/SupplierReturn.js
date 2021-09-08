
var myApp = angular.module('MyApp', ['angularUtils.directives.dirPagination']);
myApp.controller('Sutomerretun', function ($scope, $http) {
    var Teantid = 0;
    var POHeaderID = 0;
    var InvoiceId = 0;
    var WareHouseID = 0;
    var DookID = 0;
    var VehicleTypeID = 0

    var textfieldname = $("#txtTenant");
    $scope.hide = false;
    DropdownFunction(textfieldname);
    $("#txtTenant").autocomplete({
        source: function (request, response) {
            debugger;
            if ($("#txtTenant").val() == '') {
                Teantid = 0;
                POHeaderID = 0;
                InvoiceId = 0;
               // WareHouseID = 0;
               // $("#atcStore").val('');
                $("#atcSupplierNumber").val('');
                $("#atcPONumber").val('');
                $scope.clearData();

            }
            $.ajax({
                //url: '../mWebServices/FalconWebService.asmx/LoadTenantDataFor3PL',
                //data: "{ 'prefix': '" + request.term + "'}",

                url: '../mWebServices/FalconWebService.asmx/LoadTenantsByWH',
                data: "{ 'prefix': '" + request.term + "','whid':'" + WareHouseID + "' }",
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
            Teantid = i.item.val;
            POHeaderID = 0;
            InvoiceId = 0;
           // WareHouseID = 0;
           // $("#atcStore").val('');
            $("#atcSupplierNumber").val('');
            $("#atcPONumber").val('');
            if (Teantid != undefined && Teantid != "" && Teantid != 0) {
                $scope.PONumber();
                $scope.clearData();
            }
            debugger;
        },
        minLength: 0
    });


    $scope.PONumber = function () {
        var textfieldname1 = $("#atcPONumber");
        DropdownFunction(textfieldname1);
        $("#atcPONumber").autocomplete({
            source: function (request, response) {
                if ($("#atcPONumber").val() == '') {
                    POHeaderID = 0;
                   
                    InvoiceId = 0;
                    //WareHouseID = 0;
                   // $("#atcStore").val('');
                    $("#atcSupplierNumber").val('');
                    $scope.clearData();
                }
                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/GetPOHeaderListTenant',
                    data: "{ 'prefix': '" + request.term + "', 'TenantID':'" + Teantid + "'}",
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
                POHeaderID = i.item.val;
                InvoiceId = 0;
               // WareHouseID = 0;
               // $("#atcStore").val('');
                $("#atcSupplierNumber").val('');
                if (POHeaderID != undefined && POHeaderID != "" && POHeaderID != 0) {
                    $scope.Invoice();
                    $scope.clearData();
                }
                debugger;
            },
            minLength: 0
        });
    }
    $scope.Invoice = function () {
        var textfieldname1 = $("#atcSupplierNumber");
        DropdownFunction(textfieldname1);
        $("#atcSupplierNumber").autocomplete({
            source: function (request, response) {
                if ($("#atcSupplierNumber").val() == '') {
                    InvoiceId = 0;
                    $scope.clearData();
                   // WareHouseID = 0;
                    //$("#atcStore").val('');
                  
                }
                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/GetInvoiceListForPONumber',
                    data: "{ 'POHeaderID': '" + POHeaderID + "'}",
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
                InvoiceId = i.item.val;
               // WareHouseID = 0;
               // $("#atcStore").val('');
                if (InvoiceId != undefined && InvoiceId != "" && InvoiceId != 0) {
                  //  $scope.WareHouse();
                    $scope.clearData();
                }
                
                debugger;
            },
            minLength: 0
        });
    }

   // $scope.WareHouse = function () {
        var textfieldname1 = $("#atcStore");
        DropdownFunction(textfieldname1);
        $("#atcStore").autocomplete({
            source: function (request, response) {
                if ($("#atcStore").val() == '') {
                    WareHouseID = 0;
                    $scope.clearData();
                }
                $.ajax({
                  //  url: '../mWebServices/FalconWebService.asmx/GetStoreForPONumber',
                   // data: "{ 'POHeaderID': '" + POHeaderID + "','InvoiceNumber':'" + InvoiceId + "'}",
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
                WareHouseID = i.item.val;
                $('#txtTenant').val("");
                $('#atcPONumber').val("");
                $("#atcSupplierNumber").val("");
               // $scope.clearData();
            },
            minLength: 0
        });
    //}
    $scope.clearData = function () {
        $scope.Supplierreturns = [];
        $scope.$apply();

    }
    $scope.GetDetails = function () {


        debugger;
        if (Teantid == undefined || Teantid == "" || Teantid == 0 || Teantid == null) {
            showStickyToast(false, 'Please select  Tenant', false);
            return false;
        }
        if (POHeaderID == undefined || POHeaderID == "" || POHeaderID == 0 || POHeaderID == null) {
            showStickyToast(false, 'Please select  PO Number', false);
            return false;
        }
        if (InvoiceId == undefined || InvoiceId == "" || InvoiceId == 0 || InvoiceId == null) {
            showStickyToast(false, 'Please select  Invoice', false);
            return false;
        }
        if (WareHouseID == undefined || WareHouseID == "" || WareHouseID == 0 || WareHouseID == null) {
            showStickyToast(false, 'Please select  Store', false);
            return false;
        }
        $scope.blockUI = true;
        var httpreqtenant = {
            method: 'POST',
            url: '../mInventory/SupplierReturn.aspx/SupplierReturnlist',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'POHeaderId': POHeaderID, 'SupplierInvoiceId': InvoiceId, 'WareHouseID': WareHouseID }
        }
        $http(httpreqtenant).success(function (response) {
            debugger;
            $scope.blockUI = false;
            $scope.Supplierreturns = response.d;
            if ($scope.Supplierreturns.length != 0) {
                $scope.hide = true;
                $scope.LoadDock();
            }
            else {
                $scope.hide = false;
                showStickyToast(false, 'No data found for given search criteria', false);
            }

        });

    }


    $scope.LoadDock = function () {

        var httpdc = {
            method: 'POST',
            url: '../mInventory/SupplierReturn.aspx/getDocks',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'WareHouseID': WareHouseID }
        }
        $http(httpdc).success(function (response) {
            $scope.Docks = response.d;
        });


    }

    var httpVT = {
        method: 'POST',
        url: '../mOutbound/GroupOBD.aspx/getVehicleTypeData',
        headers: {
            'Content-Type': 'application/json; charset=utf-8',
            'dataType': 'json'
        },
        data: {}
    }
    $http(httpVT).success(function (response) {
        $scope.VehicleTypes = response.d;
    });


    $scope.checkreturnqty = function (obj) {
        debugger
        if (obj.ReturnQty > obj.PendingReturnQty) {
            showStickyToast(false, 'Return Qty does not exceed PendingReturnQty');
            obj.ReturnQty = obj.PendingReturnQty
        }
    }

    $scope.Transfer = function () {

        if (Teantid == undefined || Teantid == "" || Teantid == 0 || Teantid == null) {
            showStickyToast(false, 'Please select  Tenant');
            return false;
        }
        if (POHeaderID == undefined || POHeaderID == "" || POHeaderID == 0 || POHeaderID == null) {
            showStickyToast(false, 'Please select  PO Number');
            return false;
        }
        if (InvoiceId == undefined || InvoiceId == "" || InvoiceId == 0 || InvoiceId == null) {
            showStickyToast(false, 'Please select  Invoice');
            return false;
        }
        if (WareHouseID == undefined || WareHouseID == "" || WareHouseID == 0 || WareHouseID == null) {
            showStickyToast(false, 'Please select  Warehouse');
            return false;
        }

        var list = $.grep($scope.Supplierreturns, function (values, i) {
            return values.Isselected == true;
        });


        if (list.length == 0) {
            showStickyToast(false, 'Please select  atleast one line item for returns');
            return false;
        }
        var checkqty = $.grep(list, function (values, i) {
            return  values.ReturnQty != 0;
        });
        if (checkqty.length == 0) {
            showStickyToast(false, 'Please enter valid Return Qty.');
            return false;
        }
        debugger;
       // DookID = 0;
       // var VehicleTypeID = 0
        if ($("#dockid").val() == '' || $("#dockid").val() == undefined) {
            showStickyToast(false, 'Please select Dock ');
            return false;
        }
        else {
            DookID= $("#dockid").val();
            var arr = DookID.split(':');
            DookID = arr[1];
        }
        if ($("#vehicletypeid").val() == '' || $("#vehicletypeid").val() == undefined) {
            showStickyToast(false, 'Please select Vehicle Type ');
            return false;
        }
        else {
            VehicleTypeID = $("#vehicletypeid").val();
            var arr1 = VehicleTypeID.split(':');
            VehicleTypeID = arr1[1];
        }
        if($("#txtVehicleno").val() == '' || $("#txtVehicleno").val() == undefined) {
            showStickyToast(false, 'Please enter Vehicle no. ');
            return false;
        }
        if($("#txtDriver").val() == '' || $("#txtDriver").val() == undefined) {
            showStickyToast(false, 'Please enter Driver Name');
            return false;
        }
        if ($("#txtMobile").val() == '' || $("#txtMobile").val() == undefined || $("#txtMobile").val() == '0') {
            showStickyToast(false, 'Please enter Mobile No. ');
            return false;
        }
        var mobile = $("#txtMobile").val();

        if (mobile.length != 10) {
            showStickyToast(false, 'Please enter valid Mobile No. ');
            return false;
        }
      


        $scope.blockUI = true;

        var httpreqtenant = {
            method: 'POST',
            url: '../mInventory/SupplierReturn.aspx/TransferSupplierReturns',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'POHeaderId': POHeaderID, 'TenantID': Teantid, 'WareHouseID': WareHouseID, 'SupplierInvoiceId': InvoiceId, 'lis': list, 'DockId': DookID, 'VehicletypeId': VehicleTypeID, 'DriverName': $("#txtDriver").val(), 'MobileNum': $("#txtMobile").val(), 'VehileNumber': $("#txtVehicleno").val() }
        }
        $http(httpreqtenant).success(function (response) {
            debugger;
            $scope.blockUI = false;
            $scope.Result = response.d;
            if ($scope.Result == "Error") {
                showStickyToast(false, 'Error while returns creations');
                return false;
            }
            else {
                $scope.GetDetails();
                $scope.hide = false;
               showStickyToast(true, $scope.Result);    }
        });

    }

   
});