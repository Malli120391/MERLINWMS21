var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('GateList', function ($scope, $http, $timeout) {
    var WHID = '';
    var RefTenant = 0;
    var InboundStatusID = '';
    var ShipmentTypeID = 0
    $scope.noofrecords = 25;
    $scope.Totalrecords = 0;

    $("#txtWarehouse").val("");
    var textfieldname = $("#txtWarehouse");
    DropdownFunction(textfieldname);
    $("#txtWarehouse").autocomplete({
        source: function (request, response) {
            //if (RefTenant == 0 || RefTenant == "0" || RefTenant == undefined || RefTenant == null) {
            //    showStickyToast(false, 'Please select Tenant');
            //    return false;
            //}
            //if ($("#txtWarehouse").val() == '') {
            //    WHID = 0;
            //}
            $.ajax({
                // url: '../mWebServices/FalconWebService.asmx/LoadWHForWHList_CurrentStock',
                //data: "{ 'prefix': '" + request.term + "','TenantID':'" + RefTenant + "'}",
                // data: "{ 'prefix': '" + request.term + "','TenantID':'" + 0 + "'}",
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
            WHID = i.item.val;


        },
        minLength: 0
    });

    debugger;
    $('#txttenant').val("");
    var textfieldname = $("#txttenant");
    DropdownFunction(textfieldname);
    $("#txttenant").autocomplete({
        source: function (request, response) {
            if ($("#txttenant").val() == '') {
                RefTenant = 0;
            }
            if (WHID == 0 || WHID == "0" || WHID == undefined || WHID == null) {
                showStickyToast(false, 'Please select WareHouse');
                return false;
            }
            $.ajax({
                // url: '../mWebServices/FalconWebService.asmx/GetTenantList',                
                // data: "{ 'prefix': '" + request.term + "'}",
                url: '../mWebServices/FalconWebService.asmx/LoadTenantsByWH',
                data: "{ 'prefix': '" + request.term + "','whid':'" + WHID + "' }",
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
            debugger
            RefTenant = i.item.val;

        },
        minLength: 0
    });



    debugger
    $('#txtstatus').val("");
    var textfieldname = $("#txtstatus");
    DropdownFunction(textfieldname);
    $("#txtstatus").autocomplete({
        source: function (request, response) {
            if ($("#txtstatus").val() == '') {
                RefTenant = 0;
                InboundStatusID = 0;      // this line added By lalitha on 23-12-2020  to Clear the StatusID when user de select the drop down value
            }

            $.ajax({
                // url: '../mWebServices/FalconWebService.asmx/GetTenantList',                
                // data: "{ 'prefix': '" + request.term + "'}",
                url: '../mWebServices/FalconWebService.asmx/GetInboundStatus',
                data: "{ 'prefix': '" + request.term + "' }",
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
            InboundStatusID = i.item.val;

        },
        minLength: 0
    });



    debugger
    $('#txtShipment').val("");
    var textfieldname = $("#txtShipment");
    DropdownFunction(textfieldname);
    $("#txtShipment").autocomplete({
        source: function (request, response) {
            if ($("#txtShipment").val() == '') {
                RefTenant = 0;
                ShipmentTypeID = 0;                   // this line added By lalitha on 23-12-2020  to Clear the Shipment TypeID when the user de select the drop down value
            }

            $.ajax({
                // url: '../mWebServices/FalconWebService.asmx/GetTenantList',                
                // data: "{ 'prefix': '" + request.term + "'}",
                url: '../mWebServices/FalconWebService.asmx/GetShipmentTypeForInbound',
                data: "{ 'prefix': '" + request.term + "' }",
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
            ShipmentTypeID = i.item.val;

        },
        minLength: 0
    });



    $scope.getSearchData = function (PaginationId) {

        debugger;

        if ($("#txtWarehouse").val() == undefined || $("#txtWarehouse").val() == null || $("#txtWarehouse").val() == "" || $("#txtWarehouse").val() == 0) {

            showStickyToast(false, 'Please Select WareHouse', false);
            return;
        }


        if ($("#txttenant").val() == undefined || $("#txttenant").val() == null || $("#txttenant").val() == "" || $("#txttenant").val() == 0) {

            RefTenant = 0;
        }

        if (PaginationId == undefined || PaginationId == "") {
            PaginationId = 1;
        }


        var pagesize = $scope.noofrecords;
        var SearchField = $("#selCategory").val(); //$scope.userSelect;
        if (SearchField == '' || SearchField == null) {
            SearchField = 0;
        }

        var StartDate = $("#txtFromDate").val();
        var EndDate = $("#txtToDate").val();
        var MixedSearch = $("#txtSearch").val();
        if (MixedSearch == "") {

            MixedSearch = null;
        }
        //if (StartDate == "") {
        //    StartDate = null;

        //}

        //if (EndDate == "") {
        //    EndDate = null;
        //}



        var ClearenceCompanyID = 0;

        $scope.blockUI = true;
        var accounts = {
            method: 'POST',
            url: 'NewInboundSearch.aspx/getInboundData',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'StartDate': StartDate, 'EndDate': EndDate, 'StoreID': WHID, 'ShipmentTypeID': ShipmentTypeID, 'ClearenceCompanyID': ClearenceCompanyID, 'ShipmentStatusID': InboundStatusID, 'TenantID_New': RefTenant, 'SearchText': MixedSearch, 'SearchField': SearchField, 'PaginationId': PaginationId, 'PageSize': pagesize, 'TenantID': RefTenant }
        }
        $http(accounts).success(function (response) {
            debugger;
            var dt = JSON.parse(response.d);
            $scope.InboundList = dt.Table1;
            if ($scope.InboundList == undefined || $scope.InboundList == null || $scope.InboundList.length == 0) {
                $scope.blockUI = false;
                showStickyToast(false, "No Data found for given search crieteria", false);
                return false;
            }

            if ($scope.InboundList.length != 0) {
                $scope.Totalrecords = dt.Table[0].NoofColumns;
                $scope.blockUI = false;
            }
            else {
                $scope.Totalrecords = 0;
                showStickyToast(false, 'No Data Found', false);
                $scope.blockUI = false;
            }

        });

    }

    //$scope.getSearchData();

    //$scope.getAdvancedData = function () {

    //    $(".categorydata").show();
    //}

    $scope.Edit = function (id) {
        debugger
        window.location.href = "InboundDetails.aspx?ibdid=" + id;
    };

    $scope.getExportData = function () {
        debugger

        if ($("#txtWarehouse").val() == undefined || $("#txtWarehouse").val() == null || $("#txtWarehouse").val() == "" || $("#txtWarehouse").val() == 0) {

            showStickyToast(false, 'Please Select WareHouse', false);
            return;
        }


        var pagesize = $scope.noofrecords;
        var SearchField = $scope.userSelect;
        if (SearchField == '' || SearchField == null) {
            SearchField = "";
        }

        var StartDate = $("#txtFromDate").val();
        var EndDate = $("#txtToDate").val();
        var MixedSearch = $("#txtSearch").val();
        if (MixedSearch == "") {

            MixedSearch = null;
        }

        var ClearenceCompanyID = 0;
        PaginationId = 1;

        PageSize = 1;

        $scope.blockUI = true;
        var accounts = {
            method: 'POST',
            url: 'NewInboundSearch.aspx/ExcelExport',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'StartDate': $("#txtFromDate").val(), 'EndDate': $("#txtToDate").val(), 'StoreID': WHID, 'ShipmentTypeID': ShipmentTypeID, 'ClearenceCompanyID': ClearenceCompanyID, 'ShipmentStatusID': InboundStatusID, 'TenantID_New': RefTenant, 'SearchText': $("#txtSearch").val(), 'SearchField': SearchField, 'PaginationId': PaginationId, 'PageSize': PageSize }
        }
        $http(accounts).success(function (response) {
            debugger
            $scope.blockUI = false;
            window.open('../ExcelData/' + response.d + '.xlsx');
        });


    }


});