var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('GroupOutbound', function ($scope, $http) {
    $scope.searchdata = new GroupOutboundSearchData(undefined, 0, '', 0, '', '');
    $scope.onlyNumbers = /^\d+$/;
    var httpreqtenant = {
        method: 'POST',
        url: '../mOutbound/GroupOBD.aspx/getTenantData',
        headers: {
            'Content-Type': 'application/json; charset=utf-8',
            'dataType': 'json'
        },
        data: {}
    }
    $http(httpreqtenant).success(function (response) {
        debugger;
        $scope.tenants = response.d; 
       
    });

    //var httpWH = {
    //    method: 'POST',
    //    url: '../mOutbound/GroupOBD.aspx/getWareHouseData',
    //    headers: {
    //        'Content-Type': 'application/json; charset=utf-8',
    //        'dataType': 'json'
    //    },
    //    data: {
    //        'TenantID':$scope.searchdata.TenantId}
    //}
    //$http(httpWH).success(function (response) {
    //    $scope.WareHouseData = response.d;
    //});
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
    


    $scope.LoadDock = function () {
        var whid = $scope.searchdata.WareHouseId;
        if (whid != 0 && whid != undefined && whid != "" && whid != null) {
            var httpdc = {
                method: 'POST',
                url: '../mOutbound/GroupOBD.aspx/getDocks',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'dataType': 'json'
                },
                data: { 'WareHouseID': whid}
            }
            $http(httpdc).success(function (response) {
                $scope.Docks = response.d;
            });
        }
       
    }

    $scope.gettenant = function () {
        debugger;
        $scope.searchdata.CustomerName = "";
        $("#txtcust").val("");
    }

    $scope.getCustomerData = function () {

        debugger;

        var data1;
        if ($scope.searchdata.CustomerName == "" || $scope.searchdata.CustomerName == null || $scope.searchdata.CustomerName == undefined) {
            data1 = "";
        }
        else {
            data1 = $scope.searchdata.CustomerName;
        }
        if ($scope.searchdata.TenantId == undefined || $scope.searchdata.TenantId == 0) {
            $scope.customerdata = null;

            return false;

        }
        var httpWH = {
            method: 'POST',
            url: '../mOutbound/GroupOBD.aspx/getWareHouseData',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: {
                'TenantID': $scope.searchdata.TenantId
            }
        }
        $http(httpWH).success(function (response) {
            $scope.WareHouseData = response.d;
        });

        var httpreq = {
            method: 'POST',
            url: '../mOutbound/GroupOBD.aspx/getCustomerData',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'prefix': data1, 'tenantid': $scope.searchdata.TenantId }
        }


        $http(httpreq).success(function (response) {

            $scope.customerdata = response.d;

        }).error(function (response) {

        })

    }

  
    $scope.GetOBDList = function () {
        debugger;
$('.loader-block').css('display','block');
var data = $scope.searchdata;
        $scope.obddata = null;
        if ($scope.searchdata.TenantId == undefined || $scope.searchdata.TenantId == 0 || $scope.searchdata.TenantId == null) {
            showStickyToast(false, 'Please select Tenant ');
            return false;
        }
        if ($scope.searchdata.WareHouseId == undefined || $scope.searchdata.WareHouseId == 0 || $scope.searchdata.WareHouseId == null) {
            showStickyToast(false, 'Please select WareHouse ');
            return false;
        }
        if ($scope.searchdata.FromDate == undefined) {
            $scope.searchdata.FromDate == "";
        }
        if ($scope.searchdata.ToDate == undefined) {
            $scope.searchdata.ToDate == "";
        }
        //if ($scope.searchdata.CustomerName == undefined || $scope.searchdata.CustomerName == '' ) {
        //    showStickyToast(false, 'Please Enter Customer ');
        //    return false;
        //}
        var httpreq = {
            method: 'POST',
            url: '../mOutbound/GroupOBD.aspx/getoUTBOUNDDataForGroupOBD',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'obj': $scope.searchdata }
        }


        $http(httpreq).success(function (response) {
            if(response.d=='' || response.d==null || response.d==undefined || response.d.length==0)
            {
                showStickyToast(false, 'NO OBD data found for given search criteria ');
                 return false;
            }

            $scope.obddata = response.d;

        }).error(function (response) {

        })
    }

    $scope.CreateGroupOBD = function () {
        
        list = $.grep($scope.obddata, function (values, i) {
            return values.Isselected == true;
        });
        if (list.length == 0) {
            showStickyToast(false, 'Please select at least one Outbound', false);
            return false;
        }
        if ($scope.DockId == undefined || $scope.DockId == 0 || $scope.DockId == null) {
            showStickyToast(false, 'Please select Dock ');
            return false;
        }
        if ($scope.VehicleTypeId == undefined || $scope.VehicleTypeId == 0 || $scope.VehicleTypeId == null) {
            showStickyToast(false, 'Please select Vehicle Type ');
            return false;
        }
        if ($scope.Vehicleno == undefined || $scope.Vehicleno == "" || $scope.Vehicleno == null) {
            showStickyToast(false, 'Please enter Vehicle no. ');
            return false;
        } 
        if ($scope.DriverName == undefined || $scope.DriverName == "" || $scope.DriverName == null) {
            showStickyToast(false, 'Please enter Driver Name');
            return false;
        }
        if ($scope.Mobile == undefined || $scope.Mobile == "" || $scope.Mobile == null) {
            showStickyToast(false, 'Please enter Mobile No. ');
            return false;
        }
        var OutboundID = '';
        for (var i = 0; list.length > i; i++) {
            if (list[i].Isselected) {
                OutboundID += list[i].OutboundID + ",";
            }
        }
        var totalOutboundIDs = OutboundID.substring(0, OutboundID.length - 1);
        $scope.blockUI=true;

        var httpreq = {
            method: 'POST',
            url: '../mOutbound/GroupOBD.aspx/CreateGroupOBD',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'outboundids': totalOutboundIDs, 'obj': $scope.searchdata, 'VehicletypeId': $scope.VehicleTypeId, 'VehicleNo': $scope.Vehicleno, 'Driver': $scope.DriverName, 'Mobile': $scope.Mobile, 'DockID': $scope.DockId }
        }
        $http(httpreq).success(function (response) {
            $scope.blockUI=false;

            if (response.d != undefined && response.d != '') {
               
                $scope.obddata = null;
                $scope.DockId = undefined;
                $scope.VehicleTypeId = undefined;
                $scope.Vehicleno = '';
                $scope.DriverName = '';
                $scope.Mobile = '';
                var httpreq = {
                    method: 'POST',
                    url: '../mOutbound/GroupOBD.aspx/getoUTBOUNDDataForGroupOBD',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'obj': $scope.searchdata }
                }


                $http(httpreq).success(function (response) {            

                    $scope.obddata = response.d;
                });
                showStickyToast(true, 'Successfully Created Group OBD with ' + response.d, false);
            }
            else {
            }

        }).error(function (response) {
            $scope.blockUI=false;
        })
    }
    $scope.calcualtedimension = function () {
        debugger;
        $scope.MainVolume = 0;
        $scope.MainWeight = 0;
        var volume = 0;
        var weight = 0;
        var items = $.grep($scope.obddata, function (strn) {
            return strn.Isselected == true;
        });

        for (var i = 0; i < items.length; i++) {

            {
                
                volume = volume+ items[i].Volume;
                weight = weight + items[i].Weight;


            }

        }
        $scope.MainVolume = volume.toFixed(5);
        $scope.MainWeight = weight.toFixed(5);

    }

    $scope.selectAll = function () {
        debugger;

        console.log($("#allselect").is(":checked"));

        if ($("#allselect").prop("checked") == true) {
            $(".allsel").prop("checked", true);
            for (var i = 0; i < $scope.obddata.length; i++) {

                $scope.obddata[i].Isselected = true;
            }
            $scope.calcualtedimension();
        }
        else {
            $(".allsel").prop("checked", false);
            for (var i = 0; i < $scope.obddata.length; i++) {
                $scope.obddata[i].Isselected = false;

            }
            $scope.calcualtedimension();
        }

    }
    
});

function GroupOutboundSearchData(tenantid, warehouseid, customername,customerid, fromdate,todate) {
    this.TenantId = tenantid;
    this.WareHouseId = warehouseid;
    this.CustomerName = customername;
    this.FromDate = fromdate;
    this.ToDate = todate;
}
function isNumber(evt) {
    var iKeyCode = (evt.which) ? evt.which : evt.keyCode
    if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
        return false;
    return true;
}
function blockSpecialChar(e) {
    var k = e.keyCode;
    return ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || (k >= 48 && k <= 57));
}