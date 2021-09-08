var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination', 'xlsx-model']);
myApp.controller('outboundcreate', function ($scope, $http) {
  //  alert('hhh');
    $scope.searchdata = new OutboundSearchData(undefined, 0, 1, '',0);
    $scope.hide = false;
    var Warehouseid=0
    var textfieldname = $("#txtWarehouse");
    //debugger;

    DropdownFunction(textfieldname);
    $("#txtWarehouse").autocomplete({
        source: function (request, response) {
            debugger;
            $.ajax({
                //url: '../mWebServices/FalconWebService.asmx/LoadWHForWHList',
                //data: "{ 'prefix': '" + request.term + "','TenantID':'" + Tenantid + "'  }",
                url: '../mWebServices/FalconWebService.asmx/LoaDWHListBasedonUser',
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
            $('#txtTenant').val("");
            $scope.searchdata.TenantId = "";
            Warehouseid = i.item.val;
          //  $scope.searchdata.WareHouseId = i.item.val;
        },
        minLength: 0
    });
        //ending of warehouse dropdown

    $('#txtTenant').val("");
    var textfieldname = $("#txtTenant");
    DropdownFunction(textfieldname);
    $("#txtTenant").autocomplete({ 
        source: function (request, response) {
            if (Warehouseid == "" || Warehouseid == 0 || Warehouseid == undefined) {
                showStickyToast(false, "Please select Warehouse", false);
                return;
            }
            $.ajax({
                //url: '../mWebServices/FalconWebService.asmx/GetTenantList',
                //data: "{ 'prefix': '" + request.term + "'}",
                url: '../mWebServices/FalconWebService.asmx/GetWarehouseTenant',
                data: "{ 'prefix': '" + request.term + "','WHID':'" + Warehouseid + "'}",
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
            $scope.searchdata.TenantId = i.item.val;
        },
        minLength: 0
    });



    var httpreqtenant = {
        method: 'POST',
        url: '../mOutbound/CreateOutbound.aspx/getTenantData',
        headers: {
            'Content-Type': 'application/json; charset=utf-8',
            'dataType': 'json'
        },
        data: {}
    }
    $http(httpreqtenant).success(function (response) {
        $scope.tenants = response.d;
    });
    $scope.getwarehouseData = function () {
        debugger;
        var httpWH = {
            method: 'POST',
            url: '../mOutbound/OutboundCreation.aspx/getWareHouseData',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'TenantId': $scope.searchdata.TenantId }
        }
        $http(httpWH).success(function (response) {
            $scope.WareHouseData = response.d;
        });
    }

    

    var OBDtype = {
        method: 'POST',
        url: '../mOutbound/CreateOutbound.aspx/getOBDTypesData',
        headers: {
            'Content-Type': 'application/json; charset=utf-8',
            'dataType': 'json'
        },
        data: {}
    }
    $http(OBDtype).success(function (response) {
        $scope.OBDTypesData = response.d;
    });


    $scope.GetSOList = function () {
        $scope.ResultOutinfon = null;
        $scope.ResultOutinfon = null;
        var TenantID = $scope.searchdata.TenantId;

        if (TenantID == undefined || TenantID == null || TenantID == "") {
            showStickyToast(false, 'Please select Tenant ');
            return false;
        }
        var SOList = {
            method: 'POST',
            url: '../mOutbound/OutboundCreation.aspx/getSOListForCreateOutbound',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'TenantID': TenantID }
        }
        $http(SOList).success(function (response) {
            $scope.SOInfo = response.d;
            if ($scope.SOInfo == undefined || $scope.SOInfo == null || $scope.SOInfo == "" || $scope.SOInfo.length == 0) {
                showStickyToast(false, 'No data found for given search criteria ');
                return false;
            }
            debugger;
            if ($scope.SOInfo.length > 0) {
                $scope.show = true;
                $scope.tenantedit = false;
                $scope.warehouseedit = false;
                $scope.obdtypeedit = false;
            }
            else {
                $scope.show = false;
                $scope.tenantedit = false;
                $scope.warehouseedit = false;
                $scope.obdtypeedit = false;
            }


        });
    }

    $scope.GetData = function () {
        $scope.GetSOList();
        $scope.getwarehouseData();
    }

    $scope.Create = function ()
    {
        debugger;
        var sonumber = "";
        var list = $scope.SOInfo;
        var TenantID = $scope.searchdata.TenantId;
        $scope.searchdata.WareHouseId = Warehouseid;
        var WareHouseID = $scope.searchdata.WareHouseId;
        var OBDTYpe = $scope.searchdata.DeliveryTypeId;
        $scope.searchdata.PriorityTypeID = $('#dropPriority').val();
        if (TenantID == undefined || TenantID == null || TenantID == "") {
            showStickyToast(false, 'Please select Tenant ');
            return false;
        }
        if (Warehouseid == undefined || Warehouseid == null || Warehouseid == "") {
            showStickyToast(false, 'Please select Warehouse ');
            return false;
        }
        if (OBDTYpe == undefined || OBDTYpe == null || OBDTYpe == "") {
            showStickyToast(false, 'Please select OBDType ');
            return false;
        }
        if ($scope.searchdata.PriorityTypeID == undefined || $scope.searchdata.PriorityTypeID == null || $scope.searchdata.PriorityTypeID == "") {
            showStickyToast(false, 'Please select OBD Priority.. ');
            return false;
        }
        if (list.length > 0) {
            for (var i = 0; list.length > i; i++) {
                if (list[i].IsSelected == true) {
                    sonumber += list[i].SOnumber + ',';
                }
            }
        }
       

        $scope.searchdata.SoNumbers = sonumber.substring(0, sonumber.length - 1);
        if ($scope.searchdata.SoNumbers == '') {
            showStickyToast(false, 'Please select atleast one SO number for create outbound ');
            return false;
        }

        $scope.blockUI = true;
        var OutboundCreate = {
            method: 'POST',
            url: '../mOutbound/CreateOutbound.aspx/CreateOBD',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'obj': $scope.searchdata }
        }
        $http(OutboundCreate).success(function (response) {
            if (response.d.status == -1) {
                showStickyToast(false, 'No Pending Quantity for given Imported So Numbers');
                $scope.blockUI = false;
                return false;
            }
            else if (response.d.status == -2) {
                showStickyToast(false, 'Error while creation');
                $scope.blockUI = false;
                return false;
            }
            else {
                $scope.ResultOutinfon = response.d.oOBDlst;
                if ($scope.ResultOutinfon.length > 0) {
                    $scope.hide = true;
                }
                else {
                    $scope.hide = false;
                }
                $scope.SOInfo = null;
                showStickyToast(true, 'OBDS are Successfully Created');
                $scope.searchdata.WareHouseId = "";
                $scope.searchdata.TenantId = "";
                $scope.searchdata.DeliveryTypeId = 1;
                $scope.blockUI = false;
            }
          
        });
    }
    $scope.cleardata = function () {
        $scope.searchdata.TenantId = undefined;
        $scope.searchdata.WareHouseId = undefined;
        $scope.searchdata.DeliveryTypeId = 1;
        $scope.SOInfo = null;

    }
    $scope.SelectAll = function (data) {
        debugger;

        if (data == true) {
            for (var i = 0; $scope.SOInfo.length > i; i++) {
                $scope.SOInfo[i].IsSelected = true;
            }
        }
        else {
            for (var i = 0; $scope.SOInfo.length > i; i++) {
                $scope.SOInfo[i].IsSelected = false;
            }

        }
        //alert('Hiii');
    }

    $scope.uncheckedparent = function () {
        debugger;
       // $("#allchecked").checked = true;
        $('#allchecked').removeAttr('checked');
        
       // $scope.v = false;
    }

});
function OutboundSearchData(tenantid, warehouseid, deliveryTypeId, sonumbers, PriorityTypeID) {
    this.TenantId = tenantid;
    this.WareHouseId = warehouseid;
    this.DeliveryTypeId = deliveryTypeId;
    this.SoNumbers = sonumbers;
    this.PriorityTypeID = PriorityTypeID;
}