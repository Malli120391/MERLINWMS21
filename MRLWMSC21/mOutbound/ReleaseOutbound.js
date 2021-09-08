
var myApp = angular.module('Myapp', ['angularUtils.directives.dirPagination']);
myApp.controller('ReleaseOutbound', function ($scope, $http) {
    $scope.IsOBDPicking = false;
    var RefTenant = '';
    $scope.Containers=[];
    //----------For Popup Open-----------------//
    $scope.OBDRefNo = '';
    $('#spanClose').click(function (event) {
        $('#divContainer').hide();
    });

    

    $scope.closepopup = function () {
        $('#divContainer').hide();
    }

    $scope.closeData = function () {
        $('#divContainer').hide();
    }
    $scope.openDialog = function (title, OBDNumber, statusid) {
        debugger;
        $scope.MainVolume='';
        $scope.MainWeight='';
        $scope.AvailableContainer = '';
        if (statusid > 1) {
            $scope.DisplayReleaseData = false;
        }
        else {
            $scope.DisplayReleaseData = true;
        }
        var httpitemInfo =
            {
                method: 'POST',
                url: '../mOutbound/ReleaseOutbound.aspx/GetOBDWiseitem',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'dataType': 'json'
                },
                data: { 'OBDNumber': OBDNumber }
            }
        $http(httpitemInfo).success(function (response) {
            
            $scope.OBDwiseItems = response.d;
        });
     
        $('#divContainer').show();
    }
    $scope.getOBD = function () {

        //alert('11');  
        var Refobd = '';
        var tenantid = RefTenant;
        
        $('#txtOBDNumber').val("");
        var textfieldname = $("#txtOBDNumber");
        DropdownFunction(textfieldname);
        $("#txtOBDNumber").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/LoadOBDNumbers',
                    data: "{ 'prefix': '" + request.term + "','TenantId':"+tenantid+"}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        debugger;
                        response($.map(data.d, function (item) {
                            return {
                                label: item.split(',')[0],
                                val: item.split(',')[1]
                            }
                        }))

                        if (data.d.length == 0) {
                            showStickyToast(false, "There are no OBD's for the selected tenant", false);
                            return false;
                        }
                    }
                });
            },
            select: function (e, i) {
                Refobd = i.item.val;
                // alert(Refnumber);
                //$scope.ngtenant = i.item.val;
            },
            minLength: 0
        });
    }
    //Tenant Dropdown   
    var textfieldname = $("#txtTenant");
    DropdownFunction(textfieldname);
    $("#txtTenant").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/LoadTenantDataByUserWH',
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
            RefTenant = i.item.val;
            // alert(Refnumber);
            //$scope.ngtenant = i.item.val;
        },
        minLength: 0
    });

    $scope.GetOBDInfo = function (pageIndex) {
        
        var OBDNumber = "";
        var Tenantid = "";
        if ($("#txtTenant").text == "Tenant")
            Tenantid = "";
        else
            Tenantid = RefTenant;

        OBDNumber = $("#txtOBDNumber").val();
        $scope.blockUI = true;
            var httpDeliveryPickNoteMaterialInfo =
                {
                    method: 'POST',
                    url: '../mOutbound/ReleaseOutbound.aspx/GetOBDReleaseList',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'OBDNumber': OBDNumber, 'TenantID': Tenantid, 'PageIndex': pageIndex, 'PageSize': 10 }
                }
            $http(httpDeliveryPickNoteMaterialInfo).success(function (response) {
                console.log(response.d);
                $scope.Releaseinfo = response.d;               
                $scope.blockUI = false;
                $scope.TotalRecords = response.d[0].TotalRecords;
            });
    }

        //----------For Popup Close-----------------//
    $scope.GetOBDInfo(1);
    $scope.ReleaseItem = function (oBDID) {
        
        var obdid = oBDID;
        var httpreqsft = {
            method: 'POST',
            url: '../mOutbound/ReleaseOutbound.aspx/ReleaseOBDItem',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'OBDID': obdid},

        }
        $http(httpreqsft).success(function (result) {
            
            if (result = "1") {
               
                showStickyToast(true, 'Released Successfully', false);
               
                $scope.GetOBDInfo(1);
            }
            else {
                showStickyToast(true, 'Error While Releasing', false);
            }
        });
    }

    $scope.GetOBDdetails = function () {
        debugger
        var OBDNumber = "";
        var Tenantid = "";
        if ($("#txtTenant").val() == "") {
            Tenantid = 0;
        }
        else {
            Tenantid = RefTenant;
        }
        OBDNumber = $("#txtOBDNumber").val();
        $scope.blockUI = true;
        var httpOBDlist =
            {
                method: 'POST',
                url: '../mOutbound/ReleaseOutbound.aspx/GetOBDReleaseList',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'dataType': 'json'
                },
                data: { 'OBDNumber': OBDNumber, 'TenantID': Tenantid, 'PageIndex': 1, 'PageSize': 10}
            }
        $http(httpOBDlist).success(function (response) {
            
            $scope.Releaseinfo = response.d;

            if ($scope.Releaseinfo.length == 0 || $scope.Releaseinfo.length == null || $scope.Releaseinfo == null || $scope.Releaseinfo == undefined) {

                showStickyToast(false, "No Data Found", false);
                $scope.blockUI = false;
                return true;
            }
            $scope.TotalRecords = response.d[0].TotalRecords;
            $scope.blockUI = false;
        });
    }

    $scope.checkRevertQty = function (item) {
    }
    $scope.checkQty = function (item) {
    
    if (item.DeliveryQty > item.PendingQty) {
            showStickyToast(false, 'Delivery Qty does not exceed PendingQty', false);
            item.DeliveryQty = item.PendingQty;
           

            return false;
        }
    else {
        item.TotalVolume = item.DeliveryQty * item.VolumeinCBM;
        item.TotalWeight = item.DeliveryQty * item.MWeight;
        $scope.MainVolume = 0;
        $scope.MainWeight = 0;
        var items = $.grep($scope.OBDwiseItems, function (strn) {
            return (strn.IsSelected == true);
        });

        for (var i = 0; i < items.length; i++) {

            {

                $scope.MainVolume += items[i].TotalVolume;
                $scope.MainWeight = $scope.MainWeight + items[i].TotalWeight;


            }

        }

        }
        
    }
  


    //For Release 
    $scope.saveBulkReleaseItems = function () {
        
        debugger;
        var items = $.grep($scope.OBDwiseItems,function(strn) {
            return strn.IsSelected == true;
        });
        var data = $.grep($scope.OBDwiseItems, function (strn) {
            return (strn.IsSelected == true && strrn.PendingQty < strn.DeliveryQty);
        });

        if (data != undefined || data != "" || data != null || data.length != 0) {
            showStickyToast(false, 'Quantity Exceeded ', false);
            return false;
        }

        if (items.length == 0) {
            showStickyToast(false, 'Please select atleast one SO Number ', false);
            return false;
        }
        var VLPDPicking = 0;
        if ($scope.IsOBDPicking == true) {
            VLPDPicking = 0;
        }
        else {
            VLPDPicking = 1;
        }
       
        //var httpreq = {
        //    method: 'POST',
        //    url: 'ReleaseOutbound.aspx/saveBulkReleaseItems',
        //    headers: {
        //        'Content-Type': 'application/json; charset=utf-8',
        //        'dataType': 'json'
        //    },
        //    //data: { 'OutboundID': ID }
        //    data: {
        //        'Items': items, 'VLPDPicking': VLPDPicking},
        //    async: false
        //}
        //$http(httpreq).success(function (response) {
        //    debugger;
        //    var resultdata = response.d;
        //    if (resultdata == null) {
        //        showStickyToast(false, 'Error while Releasing', false);
        //        return false;
        //    }
        //    else if (resultdata.Status == -1) {
        //        showStickyToast(false, 'Problem with Assigning', false);
        //        $scope.resultantassigndata = JSON.parse(response.d.ResultData);
        //        $scope.GetOBDInfo();
              
        //    }
        //    else if (response.d == 1) {
        //        $scope.resultantassigndata = JSON.parse(response.d.ResultData);
        //        $scope.GetOBDInfo();
        //        $('#divContainer').hide();
        //        showStickyToast(true, 'Successfully Updated', false);
        //    }
           

        //})


    }
    $scope.calcualtedimension = function () {
        debugger;
       
        $scope.MainVolume = 0;
        $scope.MainWeight = 0;
var containers='';
        var items = $.grep($scope.OBDwiseItems, function (strn) {
            return strn.IsSelected == true;
        });
        
        for (var i = 0; i < items.length; i++) 
            {
                $scope.MainVolume += items[i].TotalVolume;
                $scope.MainWeight = $scope.MainWeight+items[i].TotalWeight;
            }

   var AjaxCall =
             {
                 method: 'Post',
                 datatype: 'json',
                 url: 'ReleaseOutbound.aspx/GetContainers',
                 data: {},
                 async:false,
                 headers: 'application-json; charset=utf-8'
             }
            $http(AjaxCall).then(function (response) {
               
                var data = JSON.parse(response.data.d);
               $scope.Containers = data.Table;
               containers = $.grep($scope.Containers, function (strn) {
               return strn.MaxWeightInKG>=$scope.MainWeight;
        });
        $scope.min=0;
        $scope.max=0;
if(containers.length!=0)
{
for (var index in containers)
    {
        
     
        var item=containers[index];
        if($scope.min==0 && $scope.max==0){
            // set first default element
            $scope.min=item.MaxWeightInKG;
            $scope.max=item.MaxWeightInKG;
        }
        else{
            if($scope.min>item.MaxWeightInKG)
                $scope.min=item.MaxWeightInKG;
            else if($scope.max<item.MaxWeightInKG)
                $scope.max=item.MaxWeightInKG;
        }
    }
//alert($scope.min);

var data1 = $.grep(containers,function(a){return a.MaxWeightInKG == $scope.min});
$scope.AvailableContainer=data1[0].Vehicle+ " Is Available";
//alert(data);
}
else
{
$scope.AvailableContainer="No Container is Available";
}



//for(var j=0;j<containers.length;j++)
//{
//$scope.min = Math.min.apply(Math,$scope.containers.map(function(item){return item.MaxWeightInKG}));

//var dt =

////alert($scope.min);
//}

            });

//for (var j=0;j<$scope.Containers.length;j++)
//{

//}
    }


});
function isNumber(evt) {
    var iKeyCode = (evt.which) ? evt.which : evt.keyCode
    if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
        return false;
    return true;
}
