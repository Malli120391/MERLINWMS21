
var myApp = angular.module('MyApp', ['angularUtils.directives.dirPagination']);
myApp.controller('Customerreturns', function ($scope, $http) {
    var Teantid = 0;
    var OutboundID = 0;
    var InvoiceId = 0;
    var WareHouseID = 0;


    var textfieldname = $("#txtTenant");
    $scope.hide = false;
    DropdownFunction(textfieldname);
    $("#txtTenant").autocomplete({
        source: function (request, response) {

            debugger;
            if ($("#txtTenant").val() == '') {
                Teantid = 0;
                OutboundID = 0;
                InvoiceId = 0;
              //  WareHouseID = 0;
                $("#txtobdnumber").val('');
                $("#atcInvoice").val('');
               // $("#atcStoreNo").val('');
               // $scope.clearData();
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
            OutboundID = 0;
            InvoiceId = 0;
           // WareHouseID = 0;
            //$scope.clearData();
            $("#txtobdnumber").val('');
            $("#atcInvoice").val('');
          //  $("#atcStoreNo").val('');
            if (Teantid != undefined && Teantid != "" && Teantid != 0) {
                $scope.OBDNumber();
            }
            debugger;
        },
        minLength: 0
    });


    $scope.OBDNumber = function () {
        debugger;
        var textfieldname1 = $("#txtobdnumber");
        DropdownFunction(textfieldname1);
        $("#txtobdnumber").autocomplete({
            source: function (request, response) {
                if ($("#txtobdnumber").val() == '') {
                   
                    InvoiceId = 0;
                   // WareHouseID = 0;
                    $scope.clearData();
                    $("#atcInvoice").val('');
                   // $("#atcStoreNo").val('');
                    OutboundID = 0;
                }
                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/LoadReturnOBDNumbersTenant',
                    data: "{ 'prefix': '" + request.term + "', 'TenantID':'" + Teantid+"'}",
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
                OutboundID = i.item.val;
                InvoiceId = 0;
               // WareHouseID = 0;

                $("#atcInvoice").val('');
               // $("#atcStoreNo").val('');
                if (OutboundID != undefined && OutboundID != "" && OutboundID != 0) {
                    $scope.Invoice();
                   // $scope.WareHouse();
                    $scope.clearData();
                }
                debugger;
            },
            minLength: 0
        });
    }
    $scope.clearData = function () {
        $scope.Cutomerreturn = [];
        $scope.$apply();
        
    }
    $scope.Invoice = function () {
        var textfieldname1 = $("#atcInvoice");
        DropdownFunction(textfieldname1);
        $("#atcInvoice").autocomplete({
            source: function (request, response) {
                if ($("#atcInvoice").val() == '') {
                    InvoiceId = 0;
                    $scope.clearData();
                  //  WareHouseID = 0;
                    //$("#atcStoreNo").val('');
                }
                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/LoadReturnInvoices',
                    data: "{ 'prefix': '" + request.term + "', 'OutboundID':'" + OutboundID + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data.d, function (item) {
                            return {
                                label: item.split(',')[1],
                                val: item.split(',')[0]
                            }
                        }))
                    }
                });
            },
            select: function (e, i) {
                InvoiceId = i.item.val;
               // WareHouseID = 0;
               // $("#atcStoreNo").val('');
                //$scope.clearData();
                debugger;
            },
            minLength: 0
        });
    }



  //  $scope.WareHouse = function () {
        
        var textfieldname1 = $("#atcStoreNo");
        DropdownFunction(textfieldname1);
        $("#atcStoreNo").autocomplete({
            source: function (request, response) {

                debugger;
                if ($("#atcStoreNo").val() == '') {
                    WareHouseID = 0;
                    $("#txtobdnumber").val('');
                    $("#atcInvoice").val('');
                    $scope.clearData();
                    
                }
                $.ajax({
                  //  url: '../mWebServices/FalconWebService.asmx/LoadReturnStoreNumber',
                  //  data: "{ 'OutboundID': '" + OutboundID + "'}",
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
                $('#txtobdnumber').val("");
                $('#atcInvoice').val("");
               
               // $scope.clearData();
                debugger;
            },
            minLength: 0
        });
   // }

    $scope.GetDetails = function () {
        if (WareHouseID == undefined || WareHouseID == "" || WareHouseID == 0 || WareHouseID == null) {
            showStickyToast(false, 'Please select  store');
            return false;
        }
         if (Teantid == undefined || Teantid == "" || Teantid == 0 || Teantid == null) {
            showStickyToast(false, 'Please select  Tenant');
            return false;
         }
         if (OutboundID == undefined || OutboundID == "" || OutboundID == 0 || OutboundID == null) {
             showStickyToast(false, 'Please select  Outbound');
             return false;
         }
        if (InvoiceId == undefined || InvoiceId == "" || InvoiceId == 0 || InvoiceId == null) {
            InvoiceId == 0;
             //showStickyToast(false, 'Please select  Invoice');
             //return false;
         }
         
        $scope.blockUI = true;
         var httpreqtenant = {
             method: 'POST',
             url: '../mInventory/CustomerReturns.aspx/GetCutreturnlist',
             headers: {
                 'Content-Type': 'application/json; charset=utf-8',
                 'dataType': 'json'
             },
             data: { 'OutboundID': OutboundID, 'CustomerPOID': InvoiceId, 'WareHouseID': WareHouseID}
         }
         $http(httpreqtenant).success(function (response) {
             debugger;
             $scope.blockUI = false;
             $scope.Cutomerreturn = response.d;
             if ($scope.Cutomerreturn.length != 0) {
                 $scope.hide = true;
             }
             else {
                 $scope.hide = false;
                 showStickyToast(false, 'No data found for given search criteria');
             }
             
         });

    }

    $scope.Transfer = function () {

        if (Teantid == undefined || Teantid == "" || Teantid == 0 || Teantid == null) {
            showStickyToast(false, 'Please select  Teant');
            return false;
        }
        if (OutboundID == undefined || OutboundID == "" || OutboundID == 0 || OutboundID == null) {
            showStickyToast(false, 'Please select  Outbound');
            return false;
        }
        if (InvoiceId == undefined || InvoiceId == "" || InvoiceId == 0 || InvoiceId == null) {
            InvoiceId == 0;
            //showStickyToast(false, 'Please select  Invoice');
            //return false;
        }
        if (WareHouseID == undefined || WareHouseID == "" || WareHouseID == 0 || WareHouseID == null) {
            showStickyToast(false, 'Please select  store');
            return false;
        }

        var list = $.grep($scope.Cutomerreturn, function (values, i) {
            return values.Isselected == true;
        });


        if (list.length == 0) {
            showStickyToast(false, 'Please select  atleast one line item for returns');
            return false;
        }

        var checkqty = $.grep(list, function (values, i) {
            return values.ReturnQty != 0;
        });
        if (checkqty.length == 0) {
            showStickyToast(false, 'Please enter valid Return Qty.');
            return false;
        }

        $scope.blockUI = true;
        var httpreqtenant = {
            method: 'POST',
            url: '../mInventory/CustomerReturns.aspx/Transfer',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'lstCutrt': list, 'WareHouseID': WareHouseID, 'TenantID': Teantid,'OutboundID': OutboundID }
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
                showStickyToast(true, 'Return Order created successfully with Inbound Ref. :' + $scope.Result );
            }
        });

    }

    $scope.checkreturnqty = function (obj) {
        debugger
        if (obj.ReturnQty > obj.PendingReturnQty) {
            showStickyToast(false, 'Return Qty. does not exceed Pending Returned Qty.');
            obj.ReturnQty = obj.PendingReturnQty
        }
    }
    
    

   



});