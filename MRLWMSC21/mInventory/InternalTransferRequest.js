var myApp = angular.module('MyApp', ['angularUtils.directives.dirPagination']);
myApp.controller('InternalTransfer', function ($scope, $http) {
    debugger;
    $scope.trasferID = 0;
    $scope.hide = false;
    $scope.hidesavedetails = true;
    $scope.hidesavebtn = true;
    $scope.hidesave = true;
    $scope.IsheaderCreated = false;

    var httpreqtenant = {
        method: 'POST',
        url: '../mInventory/InternalTransferRequest.aspx/Transfertype',
        headers: {
            'Content-Type': 'application/json; charset=utf-8',
            'dataType': 'json'
        },
        data: {}
    }
    $http(httpreqtenant).success(function (response) {
        $scope.transfertype = response.d;

    });

    var httpreqtenant = {
        method: 'POST',
        url: '../mInventory/InternalTransferRequest.aspx/Storagelocation',
        headers: {
            'Content-Type': 'application/json; charset=utf-8',
            'dataType': 'json'
        },
        data: {}
    }
    $http(httpreqtenant).success(function (response) {
        $scope.slinfo = response.d;
    });


    var SKU = '';
    var mid = 0;


    $scope.GoToList = function () {
        window.location.href = "FastMovingLocsTransfer.aspx";
    }

    $scope.Carton = function () {
        debugger

    }

    $scope.GetSave = function () {
        debugger;
        //alert();
        var TenantID = $("#hifTenant").val();
        var WarehouseId = $("#hifWarehouseId").val();
        var IsReq = 0;
        if (TenantID == undefined || TenantID == null || TenantID == "" || TenantID == 0) {
            showStickyToast(false, 'Please select Tenant ');
            return false;
        }
        if (WarehouseId == undefined || WarehouseId == null || WarehouseId == "" || WarehouseId == 0) {
            showStickyToast(false, 'Please select WareHouse ');
            return false;
        }
        var TransferTypeID = $scope.ddtransfertype;
        if (TransferTypeID == undefined || TransferTypeID == null || TransferTypeID == "" || TransferTypeID == 0) {
            showStickyToast(false, 'Please select Transfer Type');
            return false;
        }

        if (TransferTypeID != 5) {
            var IsSuggestedarenot = document.getElementById("cnissuggestedreg").checked

            if (IsSuggestedarenot == false) {
                IsReq = 0;
            }
            else {
                IsReq = 1;
            }
        }
        else {
            IsReq = 0;
        }

        $scope.blockUI = true;
        var remarks = $("#txtremarksid").val();
        var httpreqtenant = {
            method: 'POST',
            url: '../mInventory/InternalTransferRequest.aspx/UpsertTrenaferRequest',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'TenantID': TenantID, 'WarehouseID': WarehouseId, 'TransfertypeID': TransferTypeID, 'remarks': remarks, 'IsSuggestedReg': IsReq }
        }
        $http(httpreqtenant).success(function (response) {
            debugger;
            $scope.blockUI = false;
            $scope.trasferID = response.d;

            if ($scope.trasferID == 0) {
                showStickyToast(false, 'Error while creation for Transfer');
                return false;
            }
            else {

                //alert($scope.trasferID);
                $scope.IsheaderCreated = true;
                //$scope.GetTranrefinf();
                //$scope.GetLocinfo();
                //$scope.Cartonlist("");
                showStickyToast(true, 'Transfer header successfully created');
                setTimeout(function () {
                    // window.open(`/mInventory/InternalTransferRequest.aspx?Id=${$scope.trasferID}`);

                    window.location.href = "InternalTransferRequest.aspx?Id=" + $scope.trasferID;
                }, 1000);


            }
        });


    }

    $scope.gettraninfo = function () {
        debugger;
        if ($scope.trasferID == 0 || $scope.trasferID == undefined || $scope.trasferID == null) {
            if (location.href.indexOf('?Id=') > 0) {
                $scope.IsheaderCreated = true;
                var Id = location.href.split('?Id=')[1];
                if (Id != 0 && Id != undefined) {
                    $scope.trasferID = Id;
                    var httpreqtenant = {
                        method: 'POST',
                        url: '../mInventory/InternalTransferRequest.aspx/GetTransferInfo',
                        headers: {
                            'Content-Type': 'application/json; charset=utf-8',
                            'dataType': 'json'
                        },
                        data: { 'TransferID': $scope.trasferID }

                    }
                    $http(httpreqtenant).success(function (response) {
                        debugger;
                        $scope.TransferInfo = response.d;
                        $scope.txtRefNumber = $scope.TransferInfo[0].TransferNo;
                        $scope.ddlTenantId = $scope.TransferInfo[0].TenantName;
                        $scope.ddlWHId = $scope.TransferInfo[0].WHcode;
                        $('#hifWarehouseId').val($scope.TransferInfo[0].WarehouseId);
                        $('#hifTenant').val($scope.TransferInfo[0].TenantID);
                        // $scope.GetLocinfo();
                        // $scope.Cartonlist("");
                        $scope.ddtransfertype = $scope.TransferInfo[0].TransferTypeID;
                        $scope.txtremarks = $scope.TransferInfo[0].Remarks;
                        $scope.TRNstatus = $scope.TransferInfo[0].status;
                        var IsSuggested = $scope.TransferInfo[0].IsSuggestedReq;
                        if (IsSuggested == 0) {
                            $scope.cbIsReg = false;
                        }
                        else {
                            $scope.cbIsReg = true;
                        }
                        var staus = $scope.TransferInfo[0].status;
                        if (staus == "In Process" || staus == "Closed") {
                            $scope.hidesavedetails = false;
                            $scope.hidesavebtn = false;
                        }
                        else {
                            $scope.hidesavedetails = true;
                            $scope.hidesavebtn = true;

                        }
                        //$scope.Getiteminfo();
                        //$scope.location();
                        // $scope.Carton();
                        $scope.GetTRDList();
                        $scope.hide = true;
                        $scope.hidesave = false;

                        if ($scope.TransferInfo[0].StatusId == "2") {

                            $("#btnsearch").hide();
                        }

                    });
                }
            }

        }
    }
    $scope.gettraninfo();


    $scope.GetTranrefinf = function () {
        debugger;
        var TranferID = $scope.trasferID;



        var httpreqtenant = {
            method: 'POST',
            url: '../mInventory/InternalTransferRequest.aspx/GetTransferInfo',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'TransferID': TranferID }
        }
        $http(httpreqtenant).success(function (response) {

            $scope.TransferInfo = response.d;
            $scope.txtRefNumber = $scope.TransferInfo[0].TransferNo;
            //  $scope.ddlTenantId = $scope.TransferInfo[0].TenantID;
            $("#hifTenant").val($scope.TransferInfo[0].TenantID);
            $("#hifWarehouseId").val($scope.TransferInfo[0].WarehouseId);
            $scope.ddtransfertype = $scope.TransferInfo[0].TransferTypeID;
            $scope.txtremarks = $scope.TransferInfo[0].Remarks;
            var IsSuggestedReq = $scope.TransferInfo[0].IsSuggestedReq;
            if (IsSuggestedReq == 0) {
                $scope.cbIsReg = false;
            }
            else {
                $scope.cbIsReg = true;
            }
            // $scope.Getiteminfo();
            //$scope.location();
            // $scope.Carton();
            $scope.GetTRDList();

            //showStickyToast(false, "No Data Found");
            $scope.hide = true;
            $scope.hidesave = false;
        });
    }


    $scope.SaveTRD = function () {

        debugger;


        var SKU = '';
        var LocId = '';
        var CartonId = '';
        var TRID = $scope.trasferID;

        if ($("#hifMaterialMaster").val() == null || $("#hifMaterialMaster").val() == "" || $("#hifMaterialMaster").val() == undefined || $("#hifMaterialMaster").val() == 0 || $("#txtPartNumber").val() == "" || $("#txtPartNumber").val() == null || $("#txtPartNumber").val() == undefined) {

            showStickyToast(false, 'Please select Part Number', false);
            return false;
        }

        if ($("#hdnLocation").val() == null || $("#hdnLocation").val() == "" || $("#hdnLocation").val() == undefined || $("#hdnLocation").val() == 0 || $("#txtLocation").val() == "" || $("#txtLocation").val() == null || $("#txtLocation").val() == undefined) {

            showStickyToast(false, 'Please select Location', false);
            return false;
        }

        if ($("#hdnLocation").val() == null || $("#hdnLocation").val() == "" || $("#hdnLocation").val() == undefined || $("#hdnLocation").val() == 0 || $("#txtLocation").val() == "" || $("#txtLocation").val() == null || $("#txtLocation").val() == undefined) {

            showStickyToast(false, 'Please select Location', false);
            return false;
        }

        var fromSlID = 0;
        var ToSLID = 0;
        if ($scope.ddtransfertype == 5) {

            if ($("#hdnFromSL").val() == null || $("#hdnFromSL").val() == "" || $("#hdnFromSL").val() == undefined || $("#hdnFromSL").val() == 0 || $("#txtfromsl").val() == "" || $("#txtfromsl").val() == null || $("#txtfromsl").val() == undefined) {

                showStickyToast(false, 'Please select From Storage Location', false);
                return false;
            }

            fromSlID = $("#hdnFromSL").val();
            if ($("#hdnToSL").val() == null || $("#hdnToSL").val() == "" || $("#hdnToSL").val() == undefined || $("#hdnToSL").val() == 0 || $("#txttosl").val() == "" || $("#txttosl").val() == null || $("#txttosl").val() == undefined) {

                showStickyToast(false, 'Please select To Storage Location', false);
                return false;
            }

            ToSLID = $("#hdnToSL").val();

            if ($("#txtfromsl").val() != "" && $("#txttosl").val() != "") {
                if ($("#txtfromsl").val() == $("#txttosl").val()) {
                    showStickyToast(false, 'From Storage Location and To Storage Location shoud not be same.', false);
                    return false;
                }
            }


        }
        else {
            fromSlID = 0;
            ToSLID = 0;
        }



        var ToLocationID = 0;

        if ($scope.cbIsReg == false) {
            debugger;
            if ($scope.ddtransfertype != 5) {
                if ($("#hdnToLoc").val() == null || $("#hdnToLoc").val() == "" || $("#hdnToLoc").val() == undefined || $("#hdnToLoc").val() == 0 || $("#txttoloca").val() == "" || $("#txttoloca").val() == null || $("#txttoloca").val() == undefined) {
                    showStickyToast(false, 'Please select To Location', false);
                    return false;
                }

            }

        }
        if ($("#hdnToLoc").val() != "" && $("#hdnToLoc").val() != null && $("#hdnToLoc").val() != undefined) {

            var ToLocationID = $("#hdnToLoc").val();
        }
        else {
            ToLocation = 0;
        }

        if ($("#hdnCarton").val() != "" && $("#hdnCarton").val() != null && $("#hdnCarton").val() != undefined) {

            var ContainerId = $("#hdnCarton").val();
        }
        else {
            ContainerId = 0;
        }

        var MMID = $("#hifMaterialMaster").val();
        var LocationID = $("#hdnLocation").val();
        var BatchNo = $("#txtBatchNo").val();
        var Quantiy = $("#txtQtyid").val();
        var AvlQty = $("#AvalQty1").val();
        if (Quantiy == 0 || Quantiy == undefined) {
            showStickyToast(false, 'Please enter Transfer Quantity', false);
            return false;
        }

        if (parseFloat(Quantiy) > AvlQty) {

            showStickyToast(false, 'Transfer Qty. shoud not be more than Available Qty.')
            return false;
        }

        var ToLocation = $("#txttoloca").val();
        var FromLocation = $("#txtLocation").val();

        if ($("#txtLocation").val() != "" && $("#txttoloca").val() != "") {
            if ($("#txtLocation").val() == $("#txttoloca").val()) {
                showStickyToast(false, 'From Location and To Location shoud not be same.', false);
                return false;
            }
        }

        $scope.blockUI = true;
        var httpreqtenant = {
            method: 'POST',
            url: '../mInventory/InternalTransferRequest.aspx/UpsertTransferRequestDetails',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'TransferRequestID': TRID, 'MaterialMasterID': MMID, 'LocationID': LocationID, 'BatchNo': BatchNo, 'cartonID': ContainerId, 'Quantity': Quantiy, 'FromSLID': fromSlID, 'ToSL': ToSLID, 'ToLocationID': ToLocationID }
        }
        $http(httpreqtenant).success(function (response) {
            debugger;
            $scope.blockUI = false;
            $scope.result = response.d;
            if ($scope.result == "1") {
                showStickyToast(true, "Saved successfully");


                //setTimeout(function () { }, 1000);

                $("#txtBatchNo").val("");
                $("#txtPartNumber").val("");
                $("#hifMaterialMaster").val(0);
                $("#txtLocation").val("");
                $("#txtfromsl").val("");
                $("#txttoloca").val("");
                $("#txtCartonId").val("");
                $("#txtQtyid").val("");
                $("#txttosl").val("");
                $("#hdnLocation").val("");
                $("#hdnToSL").val("");
                $("#hdnFromSL").val("");
                $("#AvalQty1").val(0);
                $scope.GetTRDList();
                // $("#hifMaterialMaster").val("");
            }
            else {
                showStickyToast(false, "Quantity is not available for select Part Number");

            }



        });
    }



    $scope.GetTRDList = function () {
        debugger;
        var tenantID = $scope.trasferID;
        var httpreqtenant = {
            method: 'POST',
            url: '../mInventory/InternalTransferRequest.aspx/GetTransferRequestDetails',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'TransferID': tenantID }
        }
        $http(httpreqtenant).success(function (response) {
            $scope.TRDList = response.d;

            if (response.d.length > 0) {
                $scope.hidesavebtn = false;
            }

        });
    }


    $scope.Transfer = function (data) {
        debugger
        var TolocationID = data.ToLocationID;
        var Tolocation = data.Tolocation;
        var FromLoc = data.Location;
        var FromLocID = data.LocationID;
        var TRDID = data.TransferRequestDetailsID;
        var TRID = data.TransferID;
        var Qty = data.Quantiy
        var BatchNo = data.BatchNo;
        var MaterialMasterID = data.MaterialMasterID;
        var cartonID = 0
        var tocarton = data.ToCarton;

        //if (tocarton == "" || tocarton == undefined) {
        //    showStickyToast(false, 'Please select To-Carton');
        //    return false;
        //}
        //else {
        //    var carton = $.grep($scope.cartonlist, function (refnum) {
        //        return refnum.Name == tocarton;
        //    });
        //    if (carton.length == 0) {
        //        showStickyToast(false, 'Please select To-Carton', false);
        //        return false;
        //    }
        //    else {
        //        cartonID = carton[0].Id;
        //    }
        //}

        var httpreqtenant = {
            method: 'POST',
            url: '../mInventory/InternalTransferRequest.aspx/ItemWiseTransferInfo',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'MaterialMasterID': MaterialMasterID, 'TRID': TRID, 'TRDID': TRDID, 'ToLocationId': TolocationID, 'FromLocID': FromLocID, 'Qty': Qty, 'BatchNo': BatchNo, 'CartonID': cartonID }
        }
        $http(httpreqtenant).success(function (response) {
            $scope.result = response.d;
            if ($scope.result == -1) {
                showStickyToast(false, 'Error while Transfer', false);
                return false;
            }
            else if ($scope.result == -2) {
                showStickyToast(false, 'To Location and Carton Location are different', false);
                return false;
            }
            else if ($scope.result == -3) {
                showStickyToast(false, 'Carton can config mutlipule Location', false);
                return false;
            }
            else if ($scope.result == 0) {
                showStickyToast(false, 'Error while transfer', false);
                return false;
            }
            else {
                showStickyToast(true, 'Material transferred successfully', false);
                $scope.GetTranrefinf();
            }
        });
    }

    try {

        var TextFieldName = $("#txtTenant");
        DropdownFunction(TextFieldName);
        $("#txtTenant").autocomplete({
            source: function (request, response) {
                debugger;
                if ($("#txtWH").val() == "" || $("#txtWH").val() == null || $("#txtWH").val() == undefined) {
                    showStickyToast(false, "Please select Warehouse", false);
                    return;
                }
                if ($("#txtTenant").val() == "" || $("#txtTenant").val() == null || $("#txtTenant").val() == undefined) {
                    $("#hifTenant").val(0);
                }
                $.ajax({
                    // url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadTenantDataFor3PL") %>',
                    //url: '../mWebServices/FalconWebService.asmx/LoadTenantDataFor3PL',
                    //data: "{ 'prefix': '" + request.term + "'}",//<=cp.TenantID%>
                    url: '../mWebServices/FalconWebService.asmx/GetWarehouseTenant',
                    data: "{ 'prefix': '" + request.term + "','WHID':'" + $("#hifWarehouseId").val() + "'}",

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
                debugger;
                $("#hifTenant").val(i.item.val);
                // $scope.ddlTenantId = i.item.val;

            },
            minLength: 0
        });


        var TextFieldName = $("#txtWH");
        DropdownFunction(TextFieldName);
        $("#txtWH").autocomplete({
            source: function (request, response) {
                debugger;
                if ($("#txtWH").val() == "" || $("#txtWH").val() == null || $("#txtWH").val() == undefined) {
                    $("#hifWarehouseId").val(0);
                }
                if ($("#txtTenant").val() == "" || $("#txtTenant").val() == null || $("#txtTenant").val() == undefined) {
                    $("#hifTenant").val(0);
                }

                $.ajax({
                    //  url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadWarehouseData") %>',
                    //url: '../mWebServices/FalconWebService.asmx/LoadWarehouseData',   commented by LALITHA ON 24 JULY 2020 display the warehouse based on User
                    // data: "{ 'prefix': '" + request.term + "', 'TenantID':'" + $("#hifTenant").val() + "'}",//<=cp.TenantID%>
                    url: '../mWebServices/FalconWebService.asmx/LoadWarehouseBasedonUser',
                    data: "{ 'prefix': '" + request.term + "'}",//<=cp.TenantID%>
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
                debugger;
                $("#txtTenant").val("");
                $("#hifTenant").val(0);
                $("#hifWarehouseId").val(i.item.val);
                // $scope.ddlWHId = i.item.val;
            },
            minLength: 0
        });


    } catch (ex) {

    }


    $scope.Getiteminfo = function () {

        var TenantId = $scope.ddlTenantId;
        debugger;
        var TextFieldName = $("#txtPartNumber");
        DropdownFunction(TextFieldName);
        $("#txtPartNumber").autocomplete({
            source: function (request, response) {
                debugger;
                if (($("#txtPartNumber").val() == "" || $("#txtPartNumber").val() == null || $("#txtPartNumber").val() == undefined) && $("#hifMaterialMaster").val() > 0) {
                    $("#hifMaterialMaster").val(0)
                    $scope.GetAvailableQty();

                }

                $.ajax({
                    //  url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetTenantActiveStockMCode") %>',
                    url: '../mWebServices/FalconWebService.asmx/GetTenantActiveStockMCode',
                    data: "{ 'prefix': '" + request.term + "','TenantID':'" + $("#hifTenant").val() + "'}",//<=cp.TenantID%>
                    dataType: "json",
                    type: "POST",
                    async: true,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        debugger;
                        response($.map(data.d, function (item) {
                            return {

                                label: item.split('~')[0].split('`')[0],
                                description: item.split('~')[0].split('`')[1] == undefined ? "" : " <font color='#086A87'  >" + item.split('~')[0].split('`')[1] + "</font>",
                                val: item.split('~')[1]
                            }
                        }))

                    }

                });
            },
            select: function (e, i) {
                debugger;
                $("#hifMaterialMaster").val(i.item.val);
                // $scope.txtmcode = i.item.val;
                $("#hdnLocation").val("");
                $("#txtLocation").val("");
                $("#hdnCarton").val("");
                $("#hdnBatch").val("");
                $("#hdnToLoc").val("");
                $("#txttoloca").val("");
                $("#txtCartonId").val("");
                $("#txtBatchNo").val("");
                $("#AvalQty").val(0);

            },
            minLength: 0
        });
    }


    $scope.GetLocinfo = function () {
        var TextFieldName = $("#txtLocation");
        DropdownFunction(TextFieldName);
        $("#txtLocation").autocomplete({
            source: function (request, response) {
                debugger;

                if (($("#txtLocation").val() == "" || $("#txtLocation").val() == null || $("#txtLocation").val() == undefined) && $("#hdnLocation").val() > 0) {
                    $("#hdnLocation").val(0);
                    $scope.GetAvailableQty();
                }
                $.ajax({
                   // url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetAllLocationsUnderWarehouse") %>',
                    //url: '../mWebServices/FalconWebService.asmx/GetAllLocationsUnderWarehouse',
                    //data: "{'Prefix': '" + request.term + "','ProductCategory':'" + document.getElementById('<%=this.hifMaterialCategory.ClientID%>').value + "','InboundID':'0'}",
                    //data: "{'Prefix': '" + request.term + "','MMID':'" + $("#hifMaterialMaster").val() + "', 'WHID':'" + $('#hifWarehouseId').val() + "'}",

                    url: '../mWebServices/FalconWebService.asmx/GetActiveStockLocations',
                    data: "{ 'prefix': '" + request.term + "','TenantID':'" + $("#hifTenant").val() + "', 'WHID' : '" + $('#hifWarehouseId').val() + "' ,'mmid':'" + $("#hifMaterialMaster").val() + "'}",//<=cp.TenantID%>
                    dataType: "json",
                    type: "POST",
                    async: true,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        debugger;
                        response($.map(data.d, function (item) {
                            return {
                                label: item.split('~')[0],
                                val: item.split('~')[1]
                                //label: item.split('~')[0].split('`')[0],
                                //description: item.split('~')[0].split('`')[1] == undefined ? "" : " <font color='#086A87'  >" + item.split('~')[0].split('`')[1] + "</font>",
                                //val: item.split('~')[1]
                            }
                        }))

                    }

                });
            },
            select: function (e, i) {
                debugger;
                $("#hdnLocation").val(i.item.val);
                $scope.GetAvailableQty();
                $("#hdnCarton").val("");
                $("#hdnBatch").val("");
                $("#hdnToLoc").val("");
                $("#txttoloca").val("");
                $("#txtCartonId").val("");
                $("#txtBatchNo").val("");
                $("#AvalQty").val(0);
            },
            minLength: 0
        });
    }


    $scope.Carton = function () {
        var TextFieldName = $("#txtCartonId");
        DropdownFunction(TextFieldName);
        $("#txtCartonId").autocomplete({
            source: function (request, response) {
                debugger;

                if ($("#txtCartonId").val() == "" || $("#txtCartonId").val() == null || $("#txtCartonId").val() == undefined) {
                    $("#hdnCarton").val(0);
                    $scope.GetAvailableQty();
                }
                $.ajax({
                    //  url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetTenantActiveStockMCode") %>',
                    url: '../mWebServices/FalconWebService.asmx/GetCartonsUnderLocations_Inter',
                    data: "{ 'prefix': '" + request.term + "','WHID' : '" + $('#hifWarehouseId').val() + "' ,'mmid':'" + $("#hifMaterialMaster").val() + "' , 'LocationId' : '" + $("#hdnLocation").val() + "'}",//<=cp.TenantID%>
                    dataType: "json",
                    type: "POST",
                    async: true,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        debugger;
                        response($.map(data.d, function (item) {
                            return {

                                label: item.split('~')[0].split('`')[0],
                                description: item.split('~')[0].split('`')[1] == undefined ? "" : " <font color='#086A87'  >" + item.split('~')[0].split('`')[1] + "</font>",
                                val: item.split('~')[1]
                            }
                        }))

                    }

                });
            },
            select: function (e, i) {
                debugger;
                $("#hdnCarton").val(i.item.val);
                $scope.GetAvailableQty();
            },
            minLength: 0
        });
    }


    $scope.GetAvailableQty = function () {
        debugger;
        if ($("#hdnFromSL").val() == "" || $("#hdnFromSL").val() == null || $("#hdnFromSL").val() == undefined) {

            var SLocId = "0";
        }
        else {

            SLocId = $("#hdnFromSL").val();
        }
        if ($("#hdnCarton").val() == "" || $("#hdnCarton").val() == null || $("#hdnCarton").val() == undefined) {

            var Carton1Id = "0";
        }
        else {

            Carton1Id = $("#hdnCarton").val();
        }
        if ($("#hdnBatch").val() == null || $("#hdnBatch").val() == undefined || $("#hdnBatch").val() == 0) {

            var BatchId = "";
        }
        else {

            BatchId = $("#hdnBatch").val();
        }
        var httpreqtenant = {
            method: 'POST',
            url: '../mInventory/InternalTransferRequest.aspx/GetAvailableQty',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'TENANTID': $("#hifTenant").val(), 'MaterialMasterID': $("#hifMaterialMaster").val(), 'LocationID': $("#hdnLocation").val(), 'BatchNo': BatchId, 'cartonID': Carton1Id, 'StorageLocationID': SLocId }
        }
        $http(httpreqtenant).success(function (response) {
            debugger;
            $scope.Qty = response.d;
            $("#AvalQty1").val(response.d);
            // $("#AvalQty1").text(response.d);

        });

    }

    $scope.getBatchNo = function () {

        if ($("#hdnCarton").val() == "" || $("#hdnCarton").val() == null || $("#hdnCarton").val() == undefined) {

            $("#hdnCarton").val(0);
        }

        var TextFieldName = $("#txtBatchNo");
        DropdownFunction(TextFieldName);
        $("#txtBatchNo").autocomplete({
            source: function (request, response) {
                debugger;

                if ($("#txtBatchNo").val() == "" || $("#txtBatchNo").val() == null || $("#txtBatchNo").val() == undefined) {
                    $("#hdnBatch").val(0);
                }
                $.ajax({
                    //  url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetTenantActiveStockMCode") %>',
                    url: '../mWebServices/FalconWebService.asmx/GetBatchnosForTransfer',
                    data: "{ 'prefix': '" + request.term + "', 'LocationId':'" + $("#hdnLocation").val() + "','mmid':'" + $("#hifMaterialMaster").val() + "','TenantId':'" + $("#hifTenant").val() + "', 'CartonId' : '" + $("#hdnCarton").val() + "'}",//<=cp.TenantID%>
                    dataType: "json",
                    type: "POST",
                    async: true,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        debugger;
                        response($.map(data.d, function (item) {
                            return {

                                label: item.split('~')[0].split('`')[0],
                                description: item.split('~')[0].split('`')[1] == undefined ? "" : " <font color='#086A87'  >" + item.split('~')[0].split('`')[1] + "</font>",
                                val: item.split('~')[1]
                            }
                        }))

                    }

                });
            },
            select: function (e, i) {
                debugger;
                $("#hdnBatch").val(i.item.value);
                $scope.GetAvailableQty();

            },
            minLength: 0
        });
    }


    $scope.getToLocation = function () {

        var TextFieldName = $("#txttoloca");
        DropdownFunction(TextFieldName);
        $("#txttoloca").autocomplete({
            source: function (request, response) {
                debugger;
                $.ajax({
                    //  url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetTenantActiveStockMCode") %>',
                    url: '../mWebServices/FalconWebService.asmx/GetAllLocationsUnderWarehouse',
                    data: "{ 'Prefix': '" + request.term + "', 'MMID':'" + $("#hifMaterialMaster").val() + "','WHID' : '" + $('#hifWarehouseId').val() + "' }",//<=cp.TenantID%>
                    dataType: "json",
                    type: "POST",
                    async: true,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        debugger;

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
                debugger;
                $("#hdnToLoc").val(i.item.val);


            },
            minLength: 0
        });
    }



    $scope.Cartonlist1 = function () {
        debugger;
        var TextFieldName = $("#txtcart");
        DropdownFunction(TextFieldName);
        $("#txtcart").autocomplete({
            source: function (request, response) {
                debugger;

                if ($("#txtcart").val() == "" || $("#txtcart").val() == null || $("#txtcart").val() == undefined) {
                    $("#hdnTocart").val(0);
                }
                $.ajax({
                    //  url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetTenantActiveStockMCode") %>',
                    url: '../mWebServices/FalconWebService.asmx/GetCartons',
                    data: "{ 'prefix': '" + request.term + "','WHID' : '" + $('#hifWarehouseId').val() + "' ,'mmid':'" + $("#hifMaterialMaster").val() + "' }",//<=cp.TenantID%>
                    dataType: "json",
                    type: "POST",
                    async: true,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        debugger;
                        response($.map(data.d, function (item) {
                            return {

                                label: item.split('~')[0].split('`')[0],
                                description: item.split('~')[0].split('`')[1] == undefined ? "" : " <font color='#086A87'  >" + item.split('~')[0].split('`')[1] + "</font>",
                                val: item.split('~')[1]
                            }
                        }))

                    }

                });
            },
            select: function (e, i) {
                debugger;
                $("#hdnTocart").val(i.item.val);

            },
            minLength: 0
        });
    }


    $scope.getToStorageData = function () {

        var TextFieldName = $("#txttosl");
        DropdownFunction(TextFieldName);
        $("#txttosl").autocomplete({
            source: function (request, response) {
                debugger;
                $.ajax({
                    //  url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetTenantActiveStockMCode") %>',
                    url: '../mWebServices/FalconWebService.asmx/GetToStorageLocation',
                    data: "{ 'prefix': '" + request.term + "' }",//<=cp.TenantID%>
                    dataType: "json",
                    type: "POST",
                    async: true,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        debugger;
                        response($.map(data.d, function (item) {
                            return {

                                label: item.split('~')[0].split('`')[0],
                                description: item.split('~')[0].split('`')[1] == undefined ? "" : " <font color='#086A87'  >" + item.split('~')[0].split('`')[1] + "</font>",
                                val: item.split('~')[1]
                            }
                        }))

                    }

                });
            },
            select: function (e, i) {
                debugger;
                $("#hdnToSL").val(i.item.val);

            },
            minLength: 0
        });

    }

    $scope.getFromStorageData = function () {

        var TextFieldName = $("#txtfromsl");
        DropdownFunction(TextFieldName);
        $("#txtfromsl").autocomplete({
            source: function (request, response) {
                debugger;
                $.ajax({
                    //  url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetTenantActiveStockMCode") %>',
                    url: '../mWebServices/FalconWebService.asmx/GetToStorageLocation',
                    data: "{ 'prefix': '" + request.term + "'}",//<=cp.TenantID%>
                    dataType: "json",
                    type: "POST",
                    async: true,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        debugger;
                        response($.map(data.d, function (item) {
                            return {

                                label: item.split('~')[0].split('`')[0],
                                description: item.split('~')[0].split('`')[1] == undefined ? "" : " <font color='#086A87'  >" + item.split('~')[0].split('`')[1] + "</font>",
                                val: item.split('~')[1]
                            }
                        }))

                    }

                });
            },
            select: function (e, i) {
                debugger;
                $("#hdnFromSL").val(i.item.val);
                $scope.GetAvailableQty();
            },
            minLength: 0
        });

    }
});