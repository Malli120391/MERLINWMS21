var myApp = angular.module('Myapp', ['angularUtils.directives.dirPagination']);
myApp.controller('ReleaseOutboundItems', function ($scope, $http) {
    $scope.name = 'lakshmi sridurga';
    debugger; 
    var obdid = 0;
    obdid = parseInt(new URL(window.location.href).searchParams.get("id"));
    $scope.ErrorCode = function () {
        debugger;
        var outboundID = location.href.split('?id=')[1];
        var httpitemInfo =
            {
                method: 'POST',
                url: '../mOutbound/ReleaseOBDItems.aspx/ErrorCodes',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'dataType': 'json'
                },
                data: { 'OutboundID': outboundID}
            }
        $http(httpitemInfo).success(function (response) {

            $scope.resultantassigndata = response.d;

        });
    }
    $scope.GetOBDInfo = function () {
        
        if (location.href.indexOf('?id=') > 0) {
           // $scope.ErrorCode();

            var statusid =
                {
                    method: 'POST',
                    url: '../mOutbound/ReleaseOBDItems.aspx/getOBDStatus',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'OBDid': location.href.split('?id=')[1] }
                }
            $http(statusid).success(function (response)
            {
                debugger;
                if (response.d > 1) {
                    $scope.DisplayReleaseData = false;
                }
                else {
                    $scope.DisplayReleaseData = true;
                }
            });
            var httpitemInfo =
                {
                    method: 'POST',
                    url: '../mOutbound/ReleaseOBDItems.aspx/GetOBDWiseitem',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'OBDid': location.href.split('?id=')[1] }
                }
            $http(httpitemInfo).success(function (response) {

                $scope.OBDwiseItems = response.d;
                $scope.OBDNumber = $scope.OBDwiseItems[0].OBDNumber;
                console.log($scope.OBDwiseItems);
            });

        }

    }

    $scope.GetOBDInfo();

    
   

    $scope.calcualtedimension = function ()
    {
        debugger;
        $scope.resultantassigndata = null;
        $scope.MainVolume = 0;
        $scope.MainWeight = 0;
        var containers = '';
        var items = $.grep($scope.OBDwiseItems, function (strn)
        {
            return strn.IsSelected == true;
        });

        for (var i = 0; i < items.length; i++) {
            $scope.MainVolume += items[i].TotalVolume;
            $scope.MainWeight = $scope.MainWeight + items[i].TotalWeight;
        }

        var AjaxCall =
            {
                method: 'Post',
                datatype: 'json',
                url: 'ReleaseOutbound.aspx/GetContainers',
                data: {},
                async: false,
                headers: 'application-json; charset=utf-8'
            }
        $http(AjaxCall).then(function (response) {

            var data = JSON.parse(response.data.d);
            $scope.Containers = data.Table;
            containers = $.grep($scope.Containers, function (strn) {
                return strn.MaxWeightInKG >= $scope.MainWeight;
            });
            $scope.min = 0;
            $scope.max = 0;
            if (containers.length != 0) {
                for (var index in containers) {


                    var item = containers[index];
                    if ($scope.min == 0 && $scope.max == 0) {
                        // set first default element
                        $scope.min = item.MaxWeightInKG;
                        $scope.max = item.MaxWeightInKG;
                    }
                    else {
                        if ($scope.min > item.MaxWeightInKG)
                            $scope.min = item.MaxWeightInKG;
                        else if ($scope.max < item.MaxWeightInKG)
                            $scope.max = item.MaxWeightInKG;
                    }
                }
                //alert($scope.min);

                 data = $.grep(containers, function (a) { return a.MaxWeightInKG == $scope.min });
                $scope.AvailableContainer = data[0].Vehicle + " Is Available";
                //alert(data);
            }
            else {
                $scope.AvailableContainer = "No Container is Available";
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
    $scope.PickingData = function (isvlpdpicking) {
        var items = $.grep($scope.OBDwiseItems, function (strn) {
            return (strn.IsSelected == true);
        });
        if (items.length == 0) {
            showStickyToast(false, 'Please select atleast one SO Number ', false);
            return false;
        }
        $scope.checkdata = items;
        var itemsdata = $.grep($scope.checkdata, function (strn) {
            return (strn.DeliveryQty == "0" || strn.DeliveryQty == undefined || strn.DeliveryQty == 0);
        });
        if (itemsdata != undefined) {
            if (itemsdata.length > 0) {
                showStickyToast(false, 'Please enter valid Delivery Qty. ', false);
                return false;
            }
        }
        else {
            showStickyToast(false, 'Please enter valid Delivery Qty. ', false);
            return false;
        }

        if (isvlpdpicking == 1) {
            $scope.saveBulkReleaseItems(1);
        }
        //else {

        //}
    }

  


    $scope.saveBulkReleaseItems = function (isvlpd) {

        debugger;
        var items = $.grep($scope.OBDwiseItems, function (strn) {
            return (strn.IsSelected == true);
        });
        if (items.length == 0) {
            showStickyToast(false, 'Please select atleast one SO Number ', false);
            return false;
        }
        $scope.checkdata = items;
        var itemsdata = $.grep($scope.checkdata, function (strn) {
            return (strn.DeliveryQty == "0" || strn.DeliveryQty == undefined || strn.DeliveryQty == 0);
        });
        if (itemsdata != undefined) {
            if (itemsdata.length > 0) {
                showStickyToast(false, 'Please enter valid Delivery Qty. ', false);
                return false;
            }
        }
        else {
            showStickyToast(false, 'Please enter valid Delivery Qty. ', false);
            return false;
        }

        //var VLPDPicking = 0;  
        //if ($scope.IsOBDPicking == true) {
        //    VLPDPicking = 0;
        //}
        //else {
        //    VLPDPicking = 1;
        //}

        var httpreq = {
            method: 'POST',
            url: 'ReleaseOBDItems.aspx/saveBulkReleaseItems',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            //data: { 'OutboundID': ID }
            data: {
                'Items': items, 'VLPDPicking': isvlpd
            },
            async: false
        }
        $http(httpreq).success(function (response) {
            debugger;
            var resultdata = response.d;
            if (resultdata == null) {
                showStickyToast(false, 'Error while Releasing', false);
                return false;
            }
            else if (resultdata.Status == -1) {
                
                //$scope.resultantassigndata = JSON.parse(response.d.ResultData);
                //console.log(response.d.ResultData);
                $scope.ErrorCode();
                showStickyToast(false, 'Problem with Assigning', false);
                $scope.GetOBDInfo();
                return false;

            }
            else if (response.d.Status == 1) {
              
               // $scope.resultantassigndata = JSON.parse(response.d.ResultData);
                $scope.ErrorCode();
                $scope.GetOBDInfo();
                showStickyToast(true, 'Successfully Updated', false);
                $('#divContainer').hide();
                
            }


        })


    }


    //===================== added by durga to regenarate suggestions ======================//
    $scope.RegenerateSuggestions = function () {

        debugger;
        var items = $.grep($scope.OBDwiseItems, function (strn) {
            return (strn.IsSelected == true);
        });
        if (items.length == 0) {
            showStickyToast(false, 'Please select atleast one SO Number ', false);
            return false;
        }
        $scope.checkdata = items;
        var itemsdata = $.grep($scope.checkdata, function (strn) {
            return (strn.DeliveryQty == "0" || strn.DeliveryQty == undefined || strn.DeliveryQty == 0);
        });
        if (itemsdata != undefined) {
            if (itemsdata.length > 0) {
                showStickyToast(false, 'Please enter valid Delivery Qty. ', false);
                return false;
            }
        }
        else {
            showStickyToast(false, 'Please enter valid Delivery Qty. ', false);
            return false;
        }

        var data = $.grep($scope.OBDwiseItems, function (strn)
        {
            return (strn.IsSelected == true && strn.PendingQty < strn.DeliveryQty);
        });

        if (data != undefined && data != "" && data != null && data.length != 0) {
            showStickyToast(false, 'Quantity Exceeded For Selected OBD', false);
            return false;
        }
        var checkeditems = null;
        checkeditems = $.grep($scope.OBDwiseItems, function (strn) {
            return strn.PendingQty !=0;
        });
        if (checkeditems != null && checkeditems != undefined) {
            if (checkeditems.length != 0) {
                $scope.blockUI = true;
                var httpreq = {
                    method: 'POST',
                    url: 'ReleaseOBDItems.aspx/saveBulkReleaseItems',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    //data: { 'OutboundID': ID }
                    data: {
                        'Items': items, 'VLPDPicking': 0
                    },
                    async: false
                }
                $http(httpreq).success(function (response) {
                    debugger;
                    $scope.blockUI = false;
                    var resultdata = response.d;
                    if (resultdata == null) {
                        showStickyToast(false, 'Error while Releasing', false);
                        return false;
                    }
                    else if (resultdata.Status == -1) {

                        //$scope.resultantassigndata = JSON.parse(response.d.ResultData);
                        //console.log(response.d.ResultData);
                        $scope.ErrorCode();
                        showStickyToast(false, 'Problem with Assigning', false);
                        $scope.GetOBDInfo();
                        return false;

                    }
                    else if (response.d.Status == 1) {

                        // $scope.resultantassigndata = JSON.parse(response.d.ResultData);
                        $scope.ErrorCode();
                        $scope.GetOBDInfo();
                        showStickyToast(true, 'Successfully Updated', false);
                        $('#divContainer').hide();

                    }


                });
            }
            else {
                //============= write message here
                showStickyToast(true, 'No pending Qty. to regenerate suggestions', false);
                return false;
            }
        }
        else {
            //============= write message here
            showStickyToast(true, 'No pending Qty. to regenerate suggestions', false);
            return false;
        }
        
        

    }
    //-------------------- added by durga for Revert asign Line Itmes before picking---------------------------//
    $scope.revertLineItmes = function (OBD) {
        debugger;
        var IsKitParent = 0;
        if (OBD.SOQty - OBD.PendingQty == 0) {
            showStickyToast(false, 'Unable to Revert, Quantity not Assigned', false);
            return false;
        }
        if (OBD.RevertQty == undefined || OBD.RevertQty == 0) {
            showStickyToast(false, 'Please Enter Valid Revert qty.', false);
            return false;
        }
        if (OBD.RevertQty == 0) {
            showStickyToast(false, 'Please Enter Valid Revert qty.', false);
            return false;
        }
        if (OBD.RevertQty == 0) {
            showStickyToast(false, 'Please Enter Valid Revert qty.', false);
            return false;
        }
        var assignedqty = OBD.SOQty - OBD.PendingQty;
        if (assignedqty < OBD.RevertQty) {
            showStickyToast(false, 'Revert qty. Exceeded', false);
            return false;
        }
        if  (OBD.KitPlannerID>0) {
            IsKitParent = 1;
        }
        debugger;
        $scope.blockUI = true;
        var httpreq = {
            method: 'POST',
            url: '../mOutbound/ReleaseOBDItems.aspx/RevertLineItems',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            //data: { 'OutboundID': ID }
            data: {
                'IsKitParent': IsKitParent,'outboundid': OBD.OutboundID, 'SODetailsID': OBD.SODetailsID, 'quantity': OBD.RevertQty
            },
            async: false
        }
        $http(httpreq).success(function (response) {
            $scope.blockUI = false;
            if (response.d == "Succesfully Reverted") {
                showStickyToast(true, 'Successfully Reverted', false);
                 $scope.GetOBDInfo();
                return false;
            }
            else {
                showStickyToast(false, response.d, false);
                 $scope.GetOBDInfo();
            }
        });

    }
    $scope.LoadDock = function () {
     
            var httpdc = {
                method: 'POST',
                url: '../mOutbound/ReleaseOBDItems.aspx/getDocks',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'dataType': 'json'
                },
                data: { 'outboundid': location.href.split('?id=')[1]  }
            }
        $http(httpdc).success(function (response) {
            debugger;
            $scope.Docks = response.d;
            
        });
        

    }
    $scope.LoadDock();
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

    $scope.ReleaseOBDwithDock = function ()
    {

        debugger;

        var items = $scope.OBDwiseItems;

        if ($scope.DisplayReleaseData == true && $scope.DockId == undefined) {
            showStickyToast(false, 'Please select atleast one Dock', false);
            return false;
        }

        else {
            $scope.DockId = 0;
        }
        if (items.length == 0) {
            showStickyToast(false, 'Please select atleast one SO Number ', false);
            return false;
        }
        var data = $.grep($scope.OBDwiseItems, function (strn)
        {
            return (strn.IsSelected == true && strn.DeliveryQty > strn.PendingQty );
        });

     

        if (data != undefined && data != "" && data != null && data.length != 0)
        {
            showStickyToast(false, 'Quantity Exceeded For Selected OBD', false);
            return false;
        }


        var validData = $.grep($scope.OBDwiseItems, function (strn)
        {
            return (strn.IsSelected == true && strn.DeliveryQty <= strn.PendingQty);
        });

        var fieldData = '<root>';
        for (let i = 0; i < validData.length; i++)
        {
            var index = $(this).attr("data-index");
            fieldData += '<data>';
            fieldData += '<SODetailsID>' + validData[i].SODetailsID + '</SODetailsID>';
            fieldData += '<DeliveryQty>' + validData[i].DeliveryQty + '</DeliveryQty>';
            fieldData += '</data>';
        } fieldData += '</root>';



        //var fieldData = '<root>';
        //$(".cbkSelected").each(function ()
        //{
        //    var index = $(this).attr("data-index");
        //    fieldData += '<data>';
        //    fieldData += '<SODetailsID>' + $(this).attr("data-DetailID") + '</SODetailsID>';
        //    fieldData += '<DeliveryQty>' + $(".DeliveryQty" + index).val() + '</DeliveryQty>';
        //    fieldData += '</data>';
        //});
        //fieldData += '</root>';
        $scope.blockUI = true;
        var httpreq = {
            method: 'POST',
            url: 'ReleaseOBDItems.aspx/saveBulkReleaseItemsForOBD',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'outboundid': obdid, 'xmlData': fieldData, 'DockID': $scope.DockId },
            //data: {
            //    'Items': items, 'VLPDPicking': 0, 'DockId': 0, 'vehicleTypeid': 0, 'vehiclenumber': "", 'drivername': "", 'mobilenumber': "", 'outboundid': obdid
            //    //, 'DockId': $scope.DockId, 'vehicleTypeid': $scope.VehicleTypeId, 'vehiclenumber': $scope.Vehicleno, 'drivername': $scope.DriverName, 'Mobile': $scope.Mobile, 'outboundid': location.href.split('?id=')[1]
            //},
            async: false,
        }
        $http(httpreq).success(function (response) {
            debugger;
            $scope.blockUI = false;
           //response.d;
            if (response.d == "This OBD is already in Queue") {
                $scope.ErrorCode();
                showStickyToast(false, 'Another User is Processing Release Request', false);
                $scope.GetOBDInfo();
                return false;
            }
            else {
                var resultdata = JSON.parse(response.d);
                if (resultdata == null) {
                    showStickyToast(false, 'Error while Releasing', false);
                    return false;
                }
                else if (JSON.parse(response.d).Table[0].Status == -1) {

                    //$scope.resultantassigndata = JSON.parse(response.d.ResultData);
                    //console.log(response.d.ResultData);
                    $scope.ErrorCode();
                    showStickyToast(false, 'Problem with Assigning', false);
                    $scope.GetOBDInfo();
                    return false;
                }
                else if (JSON.parse(response.d).Table[0].Status == -5) {

                    //$scope.resultantassigndata = JSON.parse(response.d.ResultData);
                    //console.log(response.d.ResultData);
                    $scope.ErrorCode();
                    showStickyToast(false, 'Stock not available', false);
                    $scope.GetOBDInfo();
                    return false;

                }
                else if (JSON.parse(response.d).Table[0].Status == -6)
                {

                    //$scope.resultantassigndata = JSON.parse(response.d.ResultData);
                    //console.log(response.d.ResultData);
                    $scope.ErrorCode();
                    showStickyToast(false, 'Another User is Processing Release Request', false);
                    $scope.GetOBDInfo();
                    return false;

                }
                else if (JSON.parse(response.d).Table[0].Status == 1) {

                    // $scope.resultantassigndata = JSON.parse(response.d.ResultData);
                    $scope.ErrorCode();
                    $scope.GetOBDInfo();
                    showStickyToast(true, 'Successfully Updated', false);
                    $('#divContainer').hide();

                    setTimeout(function () {
                        location.reload();
                    }, 1500);
                }
            }
           

        });
    };

    /* ==================== Commented By M.D.Prasad ON 26-Dec-2019 =====================
    $scope.ReleaseOBDwithDock = function () {
        debugger;
        if ($scope.DockId == undefined || $scope.DockId == 0 || $scope.DockId == null) {
            showStickyToast(false, 'Please select Dock ');
            return false;
        }

        // added by lalitha on 11/03/2019 
        if ($scope.VehicleTypeId == undefined || $scope.VehicleTypeId == null|| $scope.VehicleTypeId == "" ) {
            $scope.VehicleTypeId = 0;
        }
        //if ($scope.VehicleTypeId == undefined || $scope.VehicleTypeId == 0 || $scope.VehicleTypeId == null) {
        //    showStickyToast(false, 'Please select Vehicle Type ');
        //    return false;
        //}
        //if ($scope.Vehicleno == undefined || $scope.Vehicleno == "" || $scope.Vehicleno == null) {
        //    showStickyToast(false, 'Please enter Vehicle no. ');
        //    return false;
        //}
        //if ($scope.DriverName == undefined || $scope.DriverName == "" || $scope.DriverName == null) {
        //    showStickyToast(false, 'Please enter Driver Name');
        //    return false;
        //}
        //if ($scope.Mobile == undefined || $scope.Mobile == "" || $scope.Mobile == null) {
        //    showStickyToast(false, 'Please enter Mobile No. ');
        //    return false;
        //}
        var items = $.grep($scope.OBDwiseItems, function (strn) {
            return (strn.IsSelected == true);
        });

        for (let i = 0; i < items.length; i++) {
            items[i].MDescription= "";
        }

        debugger;
        if (items.length == 0) {
            showStickyToast(false, 'Please select atleast one SO Number ', false);
            return false;
        }
        $scope.checkdata = items;
        var itemsdata = $.grep($scope.checkdata, function (strn) {
            return (strn.DeliveryQty == "0" || strn.DeliveryQty == undefined || strn.DeliveryQty == 0);
        });
        if (itemsdata != undefined) {
            if (itemsdata.length > 0) {
                showStickyToast(false, 'Please enter valid Delivery Qty. ', false);
                return false;
            }
        }
        else {
            showStickyToast(false, 'Please enter valid Delivery Qty. ', false);
            return false;
        }

        var data = $.grep($scope.OBDwiseItems, function (strn) {
            return (strn.IsSelected == true && strn.PendingQty < strn.DeliveryQty);
        });

        if (data != undefined && data != "" && data != null && data.length != 0) {
            showStickyToast(false, 'Quantity Exceeded For Selected OBD', false);
            return false;
        }
        $scope.blockUI = true;
        var httpreq = {
            method: 'POST',
            url: 'ReleaseOBDItems.aspx/saveBulkReleaseItemsForOBD',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            //data: { 'OutboundID': ID }
            data: {
                'Items': items, 'VLPDPicking': 0, 'DockId': $scope.DockId, 'vehicleTypeid': $scope.VehicleTypeId, 'vehiclenumber': $scope.Vehicleno, 'drivername': $scope.DriverName, 'mobilenumber': $scope.Mobile, 'outboundid': location.href.split('?id=')[1]
                //, 'DockId': $scope.DockId, 'vehicleTypeid': $scope.VehicleTypeId, 'vehiclenumber': $scope.Vehicleno, 'drivername': $scope.DriverName, 'Mobile': $scope.Mobile, 'outboundid': location.href.split('?id=')[1]
            },
            async: false
        }
        $http(httpreq).success(function (response) {
            debugger;
            //$('#showColOptions').css('display', 'none');
            $("#showColOptions").modal('hide');
            $scope.blockUI = false;
            var resultdata = response.d;
            if (resultdata == null)
            {
                showStickyToast(false, 'Error while Releasing', false);
                return false;
            }
            else if (resultdata.Status == -1) {
               
                $scope.ErrorCode();
                showStickyToast(false, 'Problem with Assigning', false);
                //$scope.resultantassigndata = JSON.parse(response.d.ResultData);
               // console.log(response.d.ResultData);
                $scope.GetOBDInfo();

            }
            else if (response.d.Status == 1) {
               
                //$scope.resultantassigndata = JSON.parse(response.d.ResultData);
                $scope.ErrorCode();
                showStickyToast(true, 'Successfully Updated', false);
                $scope.GetOBDInfo();
                $('#divContainer').hide();

            }


        })

    }*/

    $scope.saveBulkReleaseItems = function (isvlpd) {

        debugger;
        var items = $.grep($scope.OBDwiseItems, function (strn) {
            return (strn.IsSelected == true);
        });
        if (items.length == 0) {
            showStickyToast(false, 'Please select atleast one SO Number ', false);
            return false;
        }
        $scope.checkdata = items;
        var itemsdata = $.grep($scope.checkdata, function (strn) {
            return (strn.DeliveryQty == "0" || strn.DeliveryQty == undefined || strn.DeliveryQty == 0);
        });
        if (itemsdata != undefined) {
            if (itemsdata.length > 0) {
                showStickyToast(false, 'Please enter valid Delivery Qty. ', false);
                return false;
            }
        }
        else {
            showStickyToast(false, 'Please enter valid Delivery Qty. ', false);
            return false;
        }

        

        var data = $.grep($scope.OBDwiseItems, function (strn) {
            return (strn.IsSelected == true && strn.PendingQty < strn.DeliveryQty);
        });

        if (data != undefined && data != "" && data != null && data.length != 0) {
            showStickyToast(false, 'Quantity Exceeded For Selected OBD', false);
            return false;
        }
        //var VLPDPicking = 0;
        //if ($scope.IsOBDPicking == true) {
        //    VLPDPicking = 0;
        //}
        //else {
        //    VLPDPicking = 1;
        //}
        $scope.blockUI = true;

        var httpreq = {
            method: 'POST',
            url: 'ReleaseOBDItems.aspx/saveBulkReleaseItems',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            //data: { 'OutboundID': ID }
            data: {
                'Items': items, 'VLPDPicking': isvlpd
            },
            async: false
        }
        $http(httpreq).success(function (response) {
            debugger;
            $scope.blockUI = false;
            var resultdata = response.d;
            if (resultdata == null) {
                showStickyToast(false, 'Error while Releasing', false);
                return false;
            }
            else if (resultdata.Status == -1) {
                
                //$scope.resultantassigndata = JSON.parse(response.d.ResultData);
                //console.log(response.d.ResultData);
                $scope.ErrorCode();
                $scope.GetOBDInfo();
                showStickyToast(false, 'Problem with Assigning', false);

            }
            else if (response.d.Status == 1) {
               
               // $scope.resultantassigndata = JSON.parse(response.d.ResultData);
                $scope.ErrorCode();
                $scope.GetOBDInfo();
                showStickyToast(true, 'Successfully Updated', false);
                $('#divContainer').hide();

            }


        });
      

    }
    $scope.clearOBDpopup = function () {
        debugger;
      
       
        var items = $.grep($scope.OBDwiseItems, function (strn) {
            return (strn.IsSelected == true);
        });
        if (items.length == 0) {
            showStickyToast(false, 'Please select atleast one SO Number ', false);
            return false;
        }
        $scope.checkdata = items;
        var itemsdata = $.grep($scope.checkdata, function (strn) {
            return (strn.DeliveryQty == "0" || strn.DeliveryQty == undefined || strn.DeliveryQty == 0);
        });
        if (itemsdata != undefined) {
            if (itemsdata.length > 0) {
                showStickyToast(false, 'Please enter valid Delivery Qty. ', false);
                return false;
            }
        }
        else {
            showStickyToast(false, 'Please enter valid Delivery Qty. ', false);
            return false;
        }

        $("#showColOptions").modal({
            show: 'true'
        });
        $scope.DockId = undefined;
        $scope.VehicleTypeId = undefined;
        $scope.Vehicleno = '';
        $scope.DriverName = '';
        $scope.Mobile = '';
    }
    $scope.CheckPArentSelected = function (obj) {
        debugger;
        for (var i = 0; i < $scope.OBDwiseItems.length; i++) {
           
                if (obj.LineNumber == $scope.OBDwiseItems[i].LineNumber) {
                    if (obj.IsSelected == true) {
                        $scope.OBDwiseItems[i].IsSelected = true;
                    }
                    else {
                        $scope.OBDwiseItems[i].IsSelected = false;
                    }
                }
           
        }
    }
    //--------------- function to update child quantities when kit quantity is changed
    $scope.checkQuantity = function (obj) {
        debugger;
        for (var i = 0; i < $scope.OBDwiseItems.length; i++) {
            
            if (obj.LineNumber == $scope.OBDwiseItems[i].LineNumber && $scope.OBDwiseItems[i].Ischild == 1) {
                if (obj.DeliveryQty != undefined && obj.DeliveryQty != "0" && obj.DeliveryQty != 0 && obj.DeliveryQty != "") {
                    $scope.OBDwiseItems[i].DeliveryQty = $scope.OBDwiseItems[i].ChildKitQuantity * obj.DeliveryQty;
                }
                else {
                    $scope.OBDwiseItems[i].DeliveryQty = 0;
                }
                   
          }
          
        }
      
       
    }
    //--------------- function to update child Revert  quantities when kit quantity is changed
    $scope.checkRevertQuantity = function (obj) {
        debugger;
       
            for (var i = 0; i < $scope.OBDwiseItems.length; i++) {

                if (obj.LineNumber == $scope.OBDwiseItems[i].LineNumber && $scope.OBDwiseItems[i].Ischild == 1) {
                    if (obj.RevertQty != undefined && obj.RevertQty != "0" && obj.RevertQty != 0 && obj.RevertQty != "") {
                        $scope.OBDwiseItems[i].RevertQty = $scope.OBDwiseItems[i].ChildKitQuantity * obj.RevertQty;
                    }
                    else {
                        $scope.OBDwiseItems[i].RevertQty = 0;
                    }
                  
                }

            }
       

    }

     // This Method added by lalitha on 27/02/2019 for select all checkbox 
    $scope.selectAll = function () {
        debugger;

        console.log($("#allselect").is(":checked"));
       
        if ($("#allselect").prop("checked") == true) {
            $(".allsel").prop("checked", true);
            for (var i = 0; i < $scope.OBDwiseItems.length; i++) {

                $scope.OBDwiseItems[i].IsSelected = true;
            }
            $scope.calcualtedimension();
        }
        else {
            $(".allsel").prop("checked", false);
            for (var i = 0; i < $scope.OBDwiseItems.length; i++) {
                $scope.OBDwiseItems[i].IsSelected = false;

            }
            $scope.calcualtedimension();
        }

    }

    // added by lalitha on 27/02/2019 for select all checkbox 

});
function isNumber(evt) {
    var iKeyCode = (evt.which) ? evt.which : evt.keyCode
    if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
        return false;
    return true;
}
