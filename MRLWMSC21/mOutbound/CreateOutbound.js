var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination', 'xlsx-model']);
myApp.controller('createoutbound', function ($scope, $http) {
    $scope.hide1 = true;
    $scope.AvailableQtySOList = undefined;
    $scope.UNAvailableQtySOList = undefined;
    $scope.searchdata = new OutboundSearchData(undefined, 0, 1, '');
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

    var httpWH = {
        method: 'POST',
        url: '../mOutbound/CreateOutbound.aspx/getWareHouseData',
        headers: {
            'Content-Type': 'application/json; charset=utf-8',
            'dataType': 'json'
        },
        data: {}
    }
    $http(httpWH).success(function (response) {
        $scope.WareHouseData = response.d;
    });

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
        debugger;
        $scope.OBDTypesData = response.d;
        $scope.searchdata.DeliveryTypeId = 1;
    });
    $scope.checkdata = function () {
        alert('hi');
    }
    
    $scope.ImportData = function (data) {
        debugger;
        $scope.AvailableQtySOList = null;
        $scope.ResultOutinfon = null;
        if ($scope.searchdata.TenantId == undefined || $scope.searchdata.TenantId == "") {
            showStickyToast(false, 'Please select Tenant ');
            return false;
        }
        if ($scope.searchdata.WareHouseId == undefined || $scope.searchdata.WareHouseId == "") {
            showStickyToast(false, 'Please select Warehouse ');
            return false;
        }
        if ($scope.searchdata.DeliveryTypeId == undefined || $scope.searchdata.DeliveryTypeId == "") {
            showStickyToast(false, 'Please select Delivery Type ');
            return false;
        } 
        
        if (data == undefined) {
            showStickyToast(false, 'Please Import Excel ');
            return false;
        }
        var filename = JSON.stringify(Object.keys(data));
        var excelname = JSON.parse(filename.replace(/(\{|,)\s*(.+?)\s*:/g, '$1 "$2":'));

        var SONUMBERS = '';
        var Sonumberdata = data[excelname].SONumbers;
        if (Sonumberdata == undefined || Sonumberdata == null || Sonumberdata == '') {
            showStickyToast(false, 'Please Import Excel with Valid Data ');
            return false;
        }
        else if (Sonumberdata.length == 0) {
            showStickyToast(false, 'Please Enter atleast one so number in Excel ');
            return false;
        }
        if (Sonumberdata.length != 0) {
            for (var i = 0; Sonumberdata.length > i; i++) {
                SONUMBERS += Sonumberdata[i].SONumbers + ',';

            }

        }     
       
        $scope.searchdata.SoNumbers = SONUMBERS.substring(0, SONUMBERS.length - 1);
        if ($scope.searchdata.SoNumbers == '') {
            showStickyToast(false, 'Please Import Excel with Proper Data ');
            return false;
        }


        var httpWH = {
            method: 'POST',
            url: '../mOutbound/CreateOutbound.aspx/CreateOBD',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'obj': $scope.searchdata }
        }
        $http(httpWH).success(function (response) {
            debugger;
            if (response.d.status == -1) {
                showStickyToast(false, 'No Pending Quantity for given Imported So Numbers');
                $("#filetype").val('');
                return;
            }
            else if (response.d.status == -2) {
                showStickyToast(false, 'Error while creation');
                $("#filetype").val('');
                return;
            }
            else {
                $scope.ResultOutinfon = response.d.oOBDlst;
                showStickyToast(true, 'OBDS are Successfully Created');
                $scope.searchdata.WareHouseId = "";
                $scope.searchdata.TenantId = "";
                $scope.searchdata.DeliveryTypeId = 1;
                //$scope.excel = "";
                $("#filetype").val('');
            }
            //$scope.ResultOutinfon = response.d;
            //if ($scope.ResultOutinfon == undefined || $scope.ResultOutinfon.length == 0) {
            //    showStickyToast(false, 'Error while creation');
            //    return;
            //}
            //else {
            //    showStickyToast(true, 'OBDS are Successfully Created');
            //    $scope.searchdata.WareHouseId = "";
            //    $scope.searchdata.TenantId = "";
            //    $scope.searchdata.DeliveryTypeId = "";
            //    //$scope.excel = "";
            //    $("#filetype").val('');
            //}
           
        });

        //var httpWH = {
        //    method: 'POST',
        //    url: '../mOutbound/CreateOutbound.aspx/getSOListForCreateOutbound',
        //    headers: {
        //        'Content-Type': 'application/json; charset=utf-8',
        //        'dataType': 'json'
        //    },
        //    data: { 'obj': $scope.searchdata }
        //}
        //$http(httpWH).success(function (response) {
        //    
          
        //    $scope.SoList = JSON.parse(response.d);
        //    var list = $scope.SoList;
        //    for (var i = 0; i < $scope.SoList.length; i++) {
        //        $scope.SoList[i].Selected = true;
        //    }
            
        //    if ($scope.SoList != undefined && $scope.SoList != null) {
        //        if ($scope.SoList.length != 0) {
        //            $scope.AvailableQtySOList = $.grep($scope.SoList, function (Items) {
        //                return Items.DeliverQty != 0;
        //            });
        //            $scope.UNAvailableQtySOList = $.grep(list, function (Items) {
        //                return Items.DeliverQty == 0;
        //            });

        //        }
        //    }
            

           
        //});

       
        }

    $scope.makeTable = function (mydata) {
        
        var table = $('<table border=1>');
        var tblHeader = "<tr>";
        for (var k in mydata[0]) tblHeader += "<th>" + k + "</th>";
        tblHeader += "</tr>";
        $(tblHeader).appendTo(table);
        $.each(mydata, function (index, value) {
            var TableRow = "<tr>";
            $.each(value, function (key, val) {
                TableRow += "<td>" + val + "</td>";
            });
            TableRow += "</tr>";
            $(table).append(TableRow);
        });
        return ($(table));
    };
    $scope.CreateOutbound = function () {
        

        if ($scope.searchdata.TenantId == undefined || $scope.searchdata.TenantId == "") {
            showStickyToast(false, 'Please select Tenant ');
            return false;
        }
        if ($scope.searchdata.WareHouseId == undefined || $scope.searchdata.WareHouseId == "") {
            showStickyToast(false, 'Please select Warehouse ');
            return false;
        }
        if ($scope.searchdata.DeliveryTypeId == undefined || $scope.searchdata.DeliveryTypeId == "") {
            showStickyToast(false, 'Please select Delivery Type ');
            return false;
        }

        var selectedsodata = $.grep($scope.GETSOList, function (po) {
            return (po.Isselected == true);
        });
        if (selectedsodata.length == 0) {
            showStickyToast(false, 'Please select at least one SO Number');
            return false;
        }
        var myJsonString = JSON.stringify(selectedsodata);
       

        var httpWH = {
            method: 'POST',
            url: '../mOutbound/CreateOutbound.aspx/CreateOutboundForSelectedSonumbers',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'obj': myJsonString, 'tenantid': $scope.searchdata.TenantId, 'warehouseid': $scope.searchdata.WareHouseId, 'deliverytypeid': $scope.searchdata.DeliveryTypeId }
        }
        $http(httpWH).success(function (response) {
            if (response.d == 1) {
                showStickyToast(true, 'Successfully Created ');
                return false;
            }
            else {
                showStickyToast(true, 'Error while Creating ');
                return false;
            }
        });
    }
    $scope.cleardata = function () {
        debugger;
        $("#btnimport").val(null);
        $scope.excel = null;
        $scope.searchdata.TenantId = undefined;
        $scope.searchdata.WareHouseId = undefined;
        $scope.searchdata.DeliveryTypeId = 1;
    }
});

function OutboundSearchData(tenantid, warehouseid, deliveryTypeId, sonumbers) {
    this.TenantId = tenantid;
    this.WareHouseId = warehouseid;
    this.DeliveryTypeId = deliveryTypeId;
    this.SoNumbers = sonumbers;
}