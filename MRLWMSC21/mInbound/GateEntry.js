var app = angular.module('MyApp', ['angularUtils.directives.dirPagination']);


app.controller('GateEntry', function ($scope, $http, $timeout) {

    debugger;
    $scope.Types = [{ 'Id': 1, 'Name': 'InBound' }, { 'Id': 2, 'Name': 'OutBound' }]
    $scope.GateEntryObj = new GateEntryHeader(0,'New', 0, 0, 0, '', 0, '', 0, '', '', false, '', '', '', '', '', '', '', 0, 0, 0, '', 0, 0, '', '', '', '', '', 0, '', '', '',
        '', '', '', '', '', '', '', '', 0, '', '', ''
        , '', '', '', '', '', 0, '');
    $scope.PreLoactiondata = new PreferedLocation(0, 0, 0, '', 0, '', 0, '');
    $scope.DocksData = new GateEntryDocks(0, 0, 0, '', '', '', '', '', '', '', '', '', '', 0, '');
    $scope.ShipmentDocuments = new DocumentType(0, 0, 0, '', '');

    //
    var accounts = {
        method: 'POST',
        url: 'GateEntry.aspx/GetAccounts',
        headers: {
            'Content-Type': 'application/json; charset=utf-8',
            'dataType': 'json'
        },
        data: {}
    }
    $http(accounts).success(function (response) {
        $scope.AccountData = response.d;

    });
    var accounts = {
        method: 'POST',
        url: 'GateEntry.aspx/GetCurrentAccount',
        headers: {
            'Content-Type': 'application/json; charset=utf-8',
            'dataType': 'json'
        },
        data: {}
    }
    $http(accounts).success(function (response) {
        debugger;

        $scope.GateEntryObj.AccountId = response.d;

    });

    //------------------------------ getting tenant data----------------------------//
    var textfieldname = $("#txtTenant");
    DropdownFunction(textfieldname);
    $("#txtTenant").autocomplete({
        source: function (request, response) {
            if ($scope.GateEntryObj.Tenant == '' || $scope.GateEntryObj.Tenant == undefined) {
                $scope.GateEntryObj.TenantId = 0;
            }

            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/GetTenantList',
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
            $scope.GateEntryObj.TenantId = i.item.val;
            $scope.getwarehouseData();

        },
        minLength: 0
    });

    //------------------- getting Freight Company data ------------------------------//
    var textfieldname = $("#txtFreightCmpny");
    DropdownFunction(textfieldname);
    $("#txtFreightCmpny").autocomplete({
        source: function (request, response) {
            if ($('#txtFreightCmpny').val() == '' || $('#txtFreightCmpny').val() == undefined) {
                $scope.GateEntryObj.FrieghtCompanyid = 0;
            }
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/GetFrieghtCompanyData',
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
            $scope.GateEntryObj.FrieghtCompanyid = i.item.val;


        },
        minLength: 0
    });

    //------------------- getting Arriving country data ------------------------------//
    var textfieldname = $("#txtArrivingFrom");
    DropdownFunction(textfieldname);
    $("#txtArrivingFrom").autocomplete({
        source: function (request, response) {
            debugger;
            if ($('#txtArrivingFrom').val() == '' || $('#txtArrivingFrom').val() == undefined) {
                $scope.GateEntryObj.ArrivingFromId = 0;
            }

            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/GetCountryForGateEntry',
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
            $scope.GateEntryObj.ArrivingFromId = i.item.val;


        },
        minLength: 0
    });
    $scope.PreferedCountryId = 0;
    //------------------- getting Preferred country data ------------------------------//
    var textfieldname = $("#txtPreCountry");
    DropdownFunction(textfieldname);
    $("#txtPreCountry").autocomplete({
        source: function (request, response) {
            if ($('#txtPreCountry').val() == '' || $('#txtPreCountry').val() == undefined) {
                $scope.PreLoactiondata.CountryID = 0;
            }

            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/GetCountryForGateEntry',
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
            $scope.PreLoactiondata.CountryID = i.item.val;


        },
        minLength: 0
    });












    //------------------- getting Arriving Sate data ------------------------------//
    var textfieldname = $("#txtArrivingSate");
    DropdownFunction(textfieldname);
    $("#txtArrivingSate").autocomplete({
        source: function (request, response) {
            if ($('#txtArrivingFrom').val() == '' || $('#txtArrivingFrom').val() == undefined) {
                return false;
            }
            if ($("#txtArrivingSate").val() == "") {
                $scope.GateEntryObj.ArrivingStateId = "0";
            }
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/GetStateForGateEntry',
                data: "{ 'prefix': '" + request.term + "','countryid': '" + $scope.GateEntryObj.ArrivingFromId + "'}",
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
            $scope.GateEntryObj.ArrivingStateId = i.item.val;


        },
        minLength: 0
    });


    //------------------- getting Preferred Sate data ------------------------------//
    $scope.GateEntryObj.PreferredStateID = "0";
    var textfieldname = $("#txtPreSate");
    DropdownFunction(textfieldname);
    $("#txtPreSate").autocomplete({
        source: function (request, response) {
            if ($('#txtPreCountry').val() == '' || $('#txtPreCountry').val() == undefined) {
                return false;
            }
            if ($("#txtPreSate").val() == "") {
                $scope.PreLoactiondata.StateID = "0";
            }
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/GetStateForGateEntry',
                data: "{ 'prefix': '" + request.term + "','countryid': '" + $scope.PreLoactiondata.CountryID + "'}",
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
            $scope.PreLoactiondata.StateID = i.item.val;


        },
        minLength: 0
    });




    //------------------- getting city data ------------------------------//
    var textfieldname = $("#txtArrivingCity");
    DropdownFunction(textfieldname);
    $("#txtArrivingCity").autocomplete({
        source: function (request, response) {
            debugger;
            if ($('#txtArrivingCity').val() == '' || $('#txtArrivingCity').val() == undefined) {
                $scope.GateEntryObj.ArrivingCityId = 0;
            }
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/GetArriveCityForGateEntry',
                data: "{ 'prefix': '" + request.term + "','stateid': '" + $scope.GateEntryObj.ArrivingStateId + "'}",
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
            $scope.GateEntryObj.ArrivingCityId = i.item.val;


        },
        minLength: 0
    });


    //------------------- getting Preferred city data ------------------------------//
    $scope.GateEntryObj.PreferredCityid = 0;
    var textfieldname = $("#txtPreCity");
    var gateid = 0;


    DropdownFunction(textfieldname);
    $("#txtPreCity").autocomplete({
        source: function (request, response) {
            debugger;
            if ($('#txtPreSate').val() == '' || $('#txtPreSate').val() == undefined) {
                return false;
            }
            if ($('#txtPreCity').val() == '' || $('#txtPreCity').val() == undefined) {
                $scope.PreLoactiondata.CityID = 0;
            }
            if (location.href.indexOf('?GID=') > 0) {
                gateid = location.href.split('?GID=')[1];
            }
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/GetCityForGateEntry',
                data: "{ 'prefix': '" + request.term + "','stateid': '" + $scope.PreLoactiondata.StateID + "','gateid': '" + gateid + "'}",
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
            $scope.PreLoactiondata.CityID = i.item.val;


        },
        minLength: 0
    });

    //--------------------------------- getting Document Types ----------------------------//
    $scope.gettingDocumentType = function () {
        var accounts = {
            method: 'POST',
            url: 'GateEntry.aspx/GetDocumentTypes',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'ShipemtTypeID': $scope.GateEntryObj.GateEntryType }
        }
        $http(accounts).success(function (response) {
            $scope.DocumentTypes = response.d;

        });
    }
    //--------------------------------- getting Document Types ----------------------------//
    $scope.gettingMimeTypes = function () {
        var accounts = {
            method: 'POST',
            url: 'GateEntry.aspx/GetMIMETypes',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: {}
        }
        $http(accounts).success(function (response) {
            $scope.MimeTypes = response.d;

        });
    }

    $scope.getwarehouseData = function () {
        debugger;
        //if ($scope.GateEntryObj.TenantId == undefined || $scope.GateEntryObj.TenantId == "" || $scope.GateEntryObj.TenantId == 0) {
        //    return false;
        //}
        var httpWH = {
            method: 'POST',
            url: 'GateEntry.aspx/getWareHouseData',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'TenantId': $scope.GateEntryObj.TenantId }
        }
        $http(httpWH).success(function (response) {
            $scope.WareHouseData = response.d;
        });
    }
    $scope.getwarehouseData();
    var storageVolume = '';
    var vehiclevolume = '';
    var storageweight = '';
    var totalweight = '';
    //----------------------------------- getting Vehicle Data -----------------------------------//
    var textfieldname = $("#txtvehicle");
    DropdownFunction(textfieldname);
    $("#txtvehicle").autocomplete({
        source: function (request, response) {
            debugger;
            //if ($scope.GateEntryObj.TenantId == '0' || $scope.GateEntryObj.TenantId  == undefined) {
            //   return false;
            //}
            if ($("#txtvehicle").val() == '' || $("#txtvehicle").val() == undefined) {
                $scope.GateEntryObj.VehicleId = 0;
                $('#txtPermitInfo').val('');
                $('#txtCapacityInfo').val('');
            }
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/getVehicleForGateEntry',
                data: "{ 'prefix': '" + request.term + "','TenantId': '" + $scope.GateEntryObj.TenantId + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.split(',')[0],
                            val: item.split(',')[1],
                            storageVolume: item.split(',')[2],
                            vehiclevolume: item.split(',')[3],
                            storageweight: item.split(',')[4],
                            totalweight: item.split(',')[5]
                        }
                    }))
                }
            });
        },
        select: function (e, i) {
            $('#txtPermitInfo').val(i.item.storageVolume);
            $('#txtCapacityInfo').val(i.item.storageweight);
            $scope.GateEntryObj.VehicleId = i.item.val;
            $scope.GateEntryObj.PermitInfo = i.item.storageVolume;
            $scope.GateEntryObj.CapacityInfo = i.item.storageweight;

        },
        minLength: 0
    });

    //----------------------------------- getting Docks  Data -----------------------------------//
    var textfieldname = $("#txtDock");
    DropdownFunction(textfieldname);
    $("#txtDock").autocomplete({
        source: function (request, response) {
            debugger;
            if ($scope.GateEntryObj.GateEntryType == 0 || $scope.GateEntryObj.GateEntryType == '0' || $scope.GateEntryObj.GateEntryType == null) {
                return false;
            }
            if ($('#txtlinepartno').val() == '') {
                $scope.GateEntryObj.DockID = 0;
            }

            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/getDocksForGateEntry ',
                data: "{ 'prefix': '" + request.term + "','warehouseid': '" + $scope.GateEntryObj.WareHouseId + "','shipmentid': '" + $scope.GateEntryObj.GateEntryType + "'}",
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
            $scope.DocksData.DockID = i.item.val;

        },
        minLength: 0
    });

    //----------------------------------- getting Shipment  Data -----------------------------------//
    var textfieldname = $("#txtInboundId");
    DropdownFunction(textfieldname);
    $("#txtInboundId").autocomplete({
        source: function (request, response) {
            debugger;
            if ($scope.GateEntryObj.TenantId == '0' || $scope.GateEntryObj.TenantId == undefined) {
                showStickyToast(false, "Please select Tenant ");
                return false;
            }
            else if ($scope.GateEntryObj.GateEntryType == '0' || $scope.GateEntryObj.GateEntryType == undefined) {
                showStickyToast(false, "Please select Reporting For ");
                return false;
            }
            if ($('#txtInboundId').val() == "") {
                $scope.DocksData.ShipmentId = 0;

            }
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/GetInboundDataForGateEntry ',
                data: "{ 'prefix': '" + request.term + "','TenantId': '" + $scope.GateEntryObj.TenantId + "','ShipmentType': '" + $scope.GateEntryObj.GateEntryType + "'}",
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
            $scope.DocksData.ShipmentId = i.item.val;

        },
        minLength: 0
    });


    $scope.SaveGateEntry = function (status) {
        debugger;
        if (status == 1) {
            if ($scope.GateEntryObj.AccountId == undefined || $scope.GateEntryObj.AccountId == 0) {
                showStickyToast(false, "Please select Account ");
                return false;
            }
            else if ($scope.GateEntryObj.AccountId == undefined || $scope.GateEntryObj.AccountId == "" || $scope.GateEntryObj.AccountId == "0" || $('#txtTenant').val() == '') {
                showStickyToast(false, "Please select Tenant ");
                return false;
            }
            else if ($scope.GateEntryObj.WareHouseId == undefined || $scope.GateEntryObj.WareHouseId == 0 || $scope.GateEntryObj.WareHouseId == "0") {
                showStickyToast(false, "Please select Warehouse ");
                return false;
            }
            else if ($scope.GateEntryObj.VehicleId == undefined || $scope.GateEntryObj.VehicleId == 0 || $scope.GateEntryObj.VehicleId == "0" || $('#txtvehicle').val() == '') {
                showStickyToast(false, "Please select vehicle ");
                return false;
            }
            else if ($scope.GateEntryObj.InDriverName == undefined || $scope.GateEntryObj.InDriverName == '') {
                showStickyToast(false, "Please Enter Driver Name ");
                return false;
            }
            else if ($scope.GateEntryObj.InDriverNo == undefined || $scope.GateEntryObj.InDriverNo == '') {
                showStickyToast(false, "Please Enter Driver No. ");
                return false;
            }
        }
        if (status == 2) {
            if ($scope.IscreatedFromInbound == 1) {

                showStickyToast(false, "Cannot update, as return load is enabled for respective vehicle ");
                return false;

            }
            if ($scope.GateEntryObj.GateEntryType == undefined || $scope.GateEntryObj.GateEntryType == 0 || $scope.GateEntryObj.GateEntryType == "0") {
                showStickyToast(false, "Please select Reporting For ");
                return false;
            }
           
            else if ($scope.GateEntryObj.ArrivingFromId == undefined || $scope.GateEntryObj.ArrivingFromId == 0 || $scope.GateEntryObj.ArrivingFromId == "0" || $('#txtArrivingFrom').val() == '') {
                if ($scope.GateEntryObj.GateEntryType == 1) {
                    showStickyToast(false, "Please select 'Arriving From Country' ");
                    return false;
                }
                else {
                    $scope.GateEntryObj.ArrivingFromId = 0;
                }
               
            }
            else if ($scope.GateEntryObj.IsRetutnLoad == undefined || $scope.GateEntryObj.IsRetutnLoad==false)
            {
                if ($scope.PreferedLocationdata != undefined || $scope.PreferedLocationdata != null)
                {
                    if ($scope.PreferedLocationdata.length > 0 && $scope.GateEntryObj.GateEntryType==1) {
                        showStickyToast(false, "Cannot uncheck 'Return load', Once Prefered Location Added ");
                        return false;
                    }
                }
            }
            if ($scope.DockData != undefined) {
                if ($scope.DockData.length != 0) {
                    showStickyToast(false, "Cannot modify data, Once dock added ");
                    return false;
                }
            }
        }

        //var status = 1;
        var accounts = {
            method: 'POST',
            url: 'GateEntry.aspx/UpsertGateEntryHeader',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'obj': $scope.GateEntryObj, 'Status': status }
        }
        $http(accounts).success(function (response) {
            $scope.getgateentryDetails();
            if (response.d > 0) {
                
                if (status == 1) {
                    showStickyToast(true, "Successfully Added ");
                    $timeout(function () { window.location.href = '../mInbound/gateentry.aspx?GID=' + response.d; }, 1500);
                }
                else {
                    showStickyToast(true, "Shipment Info  Added Successfully");
                }
                
            }
            else {
                showStickyToast(true, "Error while saving ");
            }

        });
    }
    $scope.IscreatedFromInbound = 0;
    $scope.getgateentryDetails = function () {
        if (location.href.indexOf('?GID=') > 0) {
            var accounts = {
                method: 'POST',
                url: 'GateEntry.aspx/getGateEntryDetails',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'dataType': 'json'
                },
                data: { 'gateid': location.href.split('?GID=')[1] }
            }
            $http(accounts).success(function (response) {
                debugger;
                var obj = response.d;
                $scope.GateEntryObj = new GateEntryHeader(obj.OutdateTime,obj.Status, obj.StatusId, obj.ArrivingCityId, obj.ArrivingStateId, obj.UnloadedQty, obj.DockID, '', obj.GateEntryId, obj.PreferedCity, parseInt(obj.IsPreferedLocationId), obj.IsRetutnLoad, parseInt(obj.ArrivingFromId),
                    obj.SKUQty, obj.BOXQty,
                    obj.LRNO, obj.ContainerSeal, obj.CapacityInfo, obj.PermitInfo, parseInt(obj.FrieghtCompanyid), parseInt(obj.AccountId), parseInt(obj.TenantId),
                    obj.Tenant,
                    obj.VehicleId, parseInt(obj.WareHouseId), '', '', obj.InDriverName, obj.InDriverNo, obj.InboundNo, obj.InboundId, obj.EstIBDockAssignTime, obj.CountryArrivingFrom, '',
                    obj.EstIBOperationTime, '', '', '', '', '', '', '', 0, '', '', ''
                    , '', '', '', '', '', parseInt(obj.GateEntryType), obj.Vehicle);
                $scope.IscreatedFromInbound = obj.IscreatedFromInbound;
                if (obj.StatusId > 0) {
                    $scope.ShimentInfo = true;
                }
                if (obj.StatusId > 1) {
                    $scope.DockInfo = true;
                }
                if (obj.StatusId >= 5) {
                    $scope.IsUnload = true;
                    $scope.gettingDocumentType();
                    $scope.gettingMimeTypes();
                    $scope.GetDocumentTypes(0);
                }
                if (obj.StatusId > 6) {
                    $scope.showgateoutinfo = true;
                }
                $('#txtTenant').val(obj.Tenant);
                $('#txtFreightCmpny').val(obj.FrieghtCompany);
                $('#txtvehicle').val(obj.Vehicle);
                $('#txtArrivingFrom').val(obj.CountryArrivingFrom);
                $('#txtIsPreferedLocation').val(obj.PreferedCity);
                $('#txtDock').val(obj.Dock);
                $('#txtArrivingSate').val(obj.ArrivingState);
                $('#txtArrivingCity').val(obj.ArrivingCity);
            });

        }

    }
    //---------------------------- get preferred location ------------------------//
    $scope.GetPreferedLocation = function () {
        debugger;
        var accounts = {
            method: 'POST',
            url: 'GateEntry.aspx/GetGateEntryPreferreddLocation',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'GID': location.href.split('?GID=')[1] }
        }
        $http(accounts).success(function (response) {
            $scope.PreferedLocationdata = response.d;


        });
    }
    $scope.clearShipmentInfo = function () {
        $('#txtArrivingSate').val('');
        $('#txtArrivingCity').val('');
        $('#txtArrivingFrom').val('');
        $scope.GateEntryObj.ArrivingFromId = 0;
        $scope.GateEntryObj.ArrivingStateId = 0;
        $scope.GateEntryObj.ArrivingCityId = 0;
    }
    //---------------------------- save preferred location ------------------------//
    $scope.savePreferedLocation = function () {
        debugger;
        if ($scope.PreLoactiondata.CountryID == '0' || $scope.PreLoactiondata.CountryID == 0 || $scope.PreLoactiondata.CountryID == null || $('#txtPreCountry').val() == "") {
            showStickyToast(false, "Please select Preffered Country ");
            return false;
        }
        //if ($scope.PreLoactiondata.StateID == '0' || $scope.PreLoactiondata.StateID == 0 || $scope.PreLoactiondata.StateID == null || $('#txtPreSate').val() == "") {
        //    showStickyToast(false, "Please select Preffered State ");
        //    return false;
        //}
        //if ($scope.PreLoactiondata.CityID == '0' || $scope.PreLoactiondata.CityID == 0 || $scope.PreLoactiondata.CityID == null || $('#txtPreCity').val() == "") {
        //    showStickyToast(false, "Please select Preffered City ");
        //    return false;
        //}
        var accounts = {
            method: 'POST',
            url: 'GateEntry.aspx/UpsertGateEntryPreferreddLocation',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'obj': $scope.PreLoactiondata, 'GID': location.href.split('?GID=')[1] }
        }
        $http(accounts).success(function (response) {
            debugger;

            if (response.d > 0) {
                showStickyToast(true, "Successfully added ");
                $("#PreferdLocation").modal('hide');
            }
            else {
                showStickyToast(false, "Error while adding ");
            }
            $scope.GetPreferedLocation();

        });
    }

    $scope.GetPreferedLocation();
    $scope.getgateentryDetails();
    $scope.cleardataWhenOpen = function () {
        $("#SupModal").modal({
            show: 'true'
        });
    }


    $scope.cleardataWhenOpenPreLocation = function (id) {
        if (id == 1) {
            if ($scope.GateEntryObj.StatusId == 1) {
                showStickyToast(false, 'Please save reporting Information', false);
                return false;
            }
        }
        $scope.PreferredCityid = '0';
        $scope.PreferredStateID = '0';
        $scope.PreferedCountryId = '0';
        $('#txtPreCountry').val('');
        $('#txtPreSate').val('');
        $('#txtPreCity').val('');
        $("#PreferdLocation").modal({
            show: 'true'
        });
    }
    $scope.cleardataWhenOpenPreDock = function (val) {
        debugger;
        if (val == 1) {
            if ($scope.DockData != null && $scope.DockData != undefined && $scope.DockData != '' && $scope.DockData.length != 0) {
                var docks = $.grep($scope.DockData, function (strn) {
                    return strn.Actualdockouttime == '';
                });
                if (docks != undefined || docks != null) {
                    if (docks.length != 0) {
                        showStickyToast(false, 'Unable to add Docks, Please Dock Out alreaded Added Docks', false);
                        return false;
                    }

                }
            }
        }
        

        $scope.DocksData = new GateEntryDocks(0, 0, 0, '', '', '', '', '', '', '', '', '', '', 0, '');
        $('#txtInboundId').val('');
        $('#txtPreSate').val('');
        $('#txtDock').val('');
        $("#DockModal").modal({
            show: 'true'
        });
    }

    //////////---------------------- updating Preferred Location details ----------------------///////////
    $scope.updatePreferedLOCDetails = function (obj) {
        debugger;
        $scope.PreLoactiondata = new PreferedLocation(obj.PreferedLocationId, obj.GateId, obj.StateID, '', obj.CountryID, '', obj.CityID, '');
        $("#PreferdLocation").modal({
            show: 'true'
        });
        $('#txtPreCountry').val(obj.Country);
        $('#txtPreSate').val(obj.State);
        $('#txtPreCity').val(obj.City);

    }
    //////////---------------------- updating Docks Data ----------------------///////////
    $scope.updateDock = function (obj) {
        debugger;
        $scope.DocksData = new GateEntryDocks(obj.VehicleDocking_ID, obj.GateId, obj.DockID, obj.Dock,
            obj.LR, obj.ESTDockInTime, obj.ESTOperationTime, obj.ESTDockoutTime, obj.SKUQty, obj.BOXQty, obj.Actualdockintime,
            obj.Actualoperationtime, obj.Actualdockouttime, obj.ShipmentId);
        $("#DockModal").modal({
            show: 'true'
        });
        $('#txtInboundId').val(obj.ShipmentNo);
        $('#txtDock').val(obj.Dock);


    }
    //---------------------------- get Dock details--------------- ------------------------//
    $scope.GetDockDetails = function () {
        debugger;
        var accounts = {
            method: 'POST',
            url: 'GateEntry.aspx/GetGateEntryDocks',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'GID': location.href.split('?GID=')[1] }
        }
        $http(accounts).success(function (response) {
            $scope.DockData = response.d;


        });
    }
    $scope.GetDockDetails();

    //---------------------- deleting Preferred Location --------------------------//
    $scope.DeletePreferredLocDetails = function (obj) {
        var accounts = {
            method: 'POST',
            url: 'GateEntry.aspx/DeleteGateEntryPreferreddLocation',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'obj': obj, 'GID': location.href.split('?GID=')[1] }
        }
        $http(accounts).success(function (response) {
            if (response.d > 0) {
                showStickyToast(true, "Successfully Deleted ");

            }
            else {
                showStickyToast(false, "Error while Deleting ");
            }
            $scope.GetPreferedLocation();
        });
    }
    //---------------------- deleting Docks Data--------------------------//
    $scope.DeleteDock = function (obj) {
        var accounts = {
            method: 'POST',
            url: 'GateEntry.aspx/DeleteGateEntryDock',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'obj': obj, 'GID': location.href.split('?GID=')[1] }
        }
        $http(accounts).success(function (response) {
            if (response.d > 0) {
                showStickyToast(true, "Successfully Deleted ");

            }
            else {
                showStickyToast(false, "Error while Deleting ");
            }
            $scope.GetDockDetails();
        });
    }






    //-------------------------- saving Docking Details -------------------------------//
    $scope.saveDockDetails = function () {
        debugger;
        if ($scope.DocksData.Actualdockintime != '') {
            showStickyToast(false, "Unable to update, Once vehicle is docked ");
            return false;
        }
        if ($scope.DocksData.ShipmentId == '0' || $scope.DocksData.ShipmentId == 0 || $scope.DocksData.ShipmentId == null || $('#txtInboundId').val() == "") {
            showStickyToast(false, "Please select Shipment No. ");
            return false;
        }
        if ($scope.DocksData.DockID == '0' || $scope.DocksData.DockID == 0 || $scope.DocksData.DockID == null || $('#txtDock').val() == "") {
            showStickyToast(false, "Please select Dock ");
            return false;
        }
        else if ($scope.DocksData.LR == '' || $scope.DocksData.LR == undefined) {
            showStickyToast(false, "Please enter LR# ");
            return false;
        }
        else if ($scope.DocksData.ESTDockInTime == '' || $scope.DocksData.ESTDockInTime == undefined) {
            showStickyToast(false, "Please enter Est. Dock In Time ");
            return false;
        }
        else if ($scope.DocksData.ESTOperationTime == '' || $scope.DocksData.ESTOperationTime == undefined) {
            showStickyToast(false, "Please enter Est. Opr. Time ");
            return false;
        }
        else if ($scope.DocksData.SKUQty == '' || $scope.DocksData.SKUQty == undefined) {
            if ($scope.DocksData.BOXQty == '' || $scope.DocksData.BOXQty == undefined) {
                showStickyToast(false, "Please enter Box/SKU Qty ");
                return false;
            }
            else if ($scope.DocksData.BOXQty == '0' || $scope.DocksData.BOXQty == 0 || parseInt($scope.DocksData.BOXQty)==0) {
                showStickyToast(false, "Please enter valid Box Qty ");
                return false;
            }
            
        }
        else if ($scope.DocksData.BOXQty == '' || $scope.DocksData.BOXQty == undefined)
        {
            if ($scope.DocksData.SKUQty == '' || $scope.DocksData.SKUQty == undefined){
                showStickyToast(false, "Please enter Box/SKU Qty ");
                return false;
            }
            else if ($scope.DocksData.SKUQty == '0' || $scope.DocksData.SKUQty == 0 || parseInt($scope.DocksData.SKUQty) == 0) {
                showStickyToast(false, "Please enter valid SKU Qty ");
                return false;
            }
          
        }
        if (parseInt($scope.DocksData.SKUQty) == 0 || parseInt($scope.DocksData.BOXQty) == 0) {
            showStickyToast(false, "Please enter valid Box/SKU Qty ");
            return false;
        }
        var accounts = {
            method: 'POST',
            url: 'GateEntry.aspx/UpsertGateEntryDock',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'obj': $scope.DocksData, 'GID': location.href.split('?GID=')[1] }
        }
        $http(accounts).success(function (response) {
            $scope.GetDockDetails();
            if (response.d > 0) {
                showStickyToast(true, "Successfully Added ");
                $("#DockModal").modal('hide');

            }
            else {
                showStickyToast(false, "Error while Adding ");
            }

        });
    }

    //------------------------------ getting Items------------------------------//
    var textfieldname = $("#txtlinepartno");
    DropdownFunction(textfieldname);
    $("#txtlinepartno").autocomplete({

        source: function (request, response) {
            //if ($scope.BomDetails.MCode == '' || $scope.BomDetails.MCode == undefined) {
            //    $scope.BomDetails.MMID = '0';
            //}

            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/GetMaterialsForGateEntry',
                data: "{ 'prefix': '" + request.term + "','tenantid':'" + $scope.GateEntryObj.TenantId + "','inboundID':'" + $scope.GateEntryObj.InboundId + "','Outboundid':'" + $scope.GateEntryObj.OutBoundId + "','gateid':'" + $scope.GateEntryObj.OutBoundId + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.split(',')[0],
                            val: item.split(',')[1],

                        }
                    }))
                }
            });
        },
        select: function (e, i) {
            $scope.BomDetails.MMID = '0';
            $scope.BomDetails.MMID = i.item.val;

        },
        minLength: 0
    });


    //--------------------------- save unloading Data -------------------------------//
    $scope.SaveUnloadVehicleData = function () {
        if ($scope.GateEntryObj.UnloadedQty == '' || $scope.GateEntryObj.UnloadedQty == undefined) {
            showStickyToast(false, "Please enter Unloaded Qty ");
            return false;
        }
        else if ($scope.GateEntryObj.UnloadedQty == '0') {
            showStickyToast(false, "Please enter Valid Unloaded Qty ");
            return false;
        }
        var accounts = {
            method: 'POST',
            url: 'GateEntry.aspx/UpsertUnloadedData',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: {
                'GID': location.href.split('?GID=')[1], 'qty': $scope.GateEntryObj.UnloadedQty
            }
        }
        $http(accounts).success(function (response) {
            $scope.getgateentryDetails();
            if (response.d > 0) {
                if ($scope.GateEntryObj.GateEntryType == 1) {
                    showStickyToast(true, "Unloaded Details Added Successfully ");
                }
                else {
                    showStickyToast(true, "Loading Details Added Successfully ");
                }
                
            }
            else {
                showStickyToast(false, "Error while updating ");
            }
        });
    }
    $scope.checkFileExtension = function (elem) {
        debugger;
        var filePath = elem.name;

        if (filePath.indexOf('.') == -1)
            return;

        var validExtensions = new Array();
        var ext = filePath.substring(filePath.lastIndexOf('.') + 1).toLowerCase();

        validExtensions[0] = 'pdf';
       

        for (var i = 0; i < validExtensions.length; i++) {
            if (ext == validExtensions[i])
                return;
        }

        elem.value = "";
        alert('The file extension ' + ext.toUpperCase() + ' is not allowed!');
        return;
    }
    $scope.ShipmentDocuments.DocumentURI = '';
    $scope.uploadFile = function () {
        debugger;

        if ($scope.ShipmentDocuments.DocumentTypeID == undefined || $scope.ShipmentDocuments.DocumentTypeID == '' || $scope.ShipmentDocuments.DocumentTypeID == 0 || $scope.ShipmentDocuments.DocumentTypeID == null) {
            showStickyToast(false, "Please select Document Type");
            return false;
        }
        else if ($scope.ShipmentDocuments.MIMEID == undefined || $scope.ShipmentDocuments.MIMEID == '' || $scope.ShipmentDocuments.MIMEID == 0 || $scope.ShipmentDocuments.MIMEID == null) {
            showStickyToast(false, "Please select MIME Type");
            return false;
        }
        else if ($scope.ShipmentDocuments.DocumentName == undefined || $scope.ShipmentDocuments.DocumentName == '' || $scope.ShipmentDocuments.MIMEID == null) {
            showStickyToast(false, "Please Enter Document Name");
            return false;
        }
        //else if ($scope.ShipmentDocuments.DocumentURI == undefined || $scope.ShipmentDocuments.DocumentURI == '' || $scope.ShipmentDocuments.DocumentURI == null) {
        //    showStickyToast(false, "Please select file");
        //    return false;
        //}
        $scope.ShipmentDocuments.DocumentURI = '';
        var fileData = $('#Mimefile');
        fileData = fileData[0].files[0];
        var formData = new FormData();
        formData.append("file", fileData);
        if (fileData == undefined || fileData == '') {
            showStickyToast(false, "Please select file");
            return false;
        }
        var filePath = fileData.name;
        var ext = filePath.substring(filePath.lastIndexOf('.') + 1).toLowerCase();
        if ($scope.ShipmentDocuments.MIMEID == 1) {
            if (ext.toLowerCase() != 'pdf'.toLowerCase()) {
                showStickyToast(false, "Please Upload Files with extension based on selected MIME type");
                return false;
            }
        }
        else if ($scope.ShipmentDocuments.MIMEID == 2) {
            if (ext.toLowerCase() != 'JPEG'.toLowerCase()) {
                showStickyToast(false, "Please Upload Files with extension based on selected MIME type");
                return false;
            }
        }
        if ($scope.ShipmentDocuments.MIMEID == 3) {
            if (ext.toLowerCase() != 'PNG'.toLowerCase()) {
                showStickyToast(false, "Please Upload Files with extension based on selected MIME type");
                return false;
            }
        }
        
       
        $.ajax({
            url: "../ImageUpload.ashx",
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            async: false,
            success: function (result) {
                debugger;
                //alert("success.");
                $scope.ShipmentDocuments.DocumentURI = '../GateEntryDocuments/' + result;
                $scope.uploadfiledata();
            },
            error: function (errorData) {
                debugger;
                console.log(errorData);
                // alert("Error in uploading media, please try later.");
                showStickyToast(false, "Error in uploading media, please try later.");
            }
        });
    }
    $scope.GetDocumentTypes = function (val) {
        debugger;
        var accounts = {
            method: 'POST',
            url: 'GateEntry.aspx/GetGateEntryDocuments',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: {
                'GID': location.href.split('?GID=')[1]
            }
        }
        $http(accounts).success(function (response) {
            if (val == 1) {
                $scope.getgateentryDetails();
            }
          
            $scope.Documnetdata = response.d;
        });
    }
    //---------------------- get added Document ---------------------------//
    $scope.getSampleTemplate = function (url) {
        debugger;
        window.open(url);
    }
    //---------------------- delete document ------------------//
    $scope.DeleteDocument = function (obj) {
        var accounts = {
            method: 'POST',
            url: 'GateEntry.aspx/DeleteGateEntryDocument',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'obj': obj, 'GID': location.href.split('?GID=')[1] }
        }
        $http(accounts).success(function (response) {
            $scope.getgateentryDetails();
            if (response.d > 0) {
                showStickyToast(true, "Successfully Deleted ");

            }
            else {
                showStickyToast(false, "Error while Deleting ");
            }
            $scope.GetDocumentTypes(1);
        });
    }


    $scope.uploadfiledata = function () {
        debugger;
        var accounts = {
            method: 'POST',
            url: 'GateEntry.aspx/UploadDocuments',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: {
                'obj': $scope.ShipmentDocuments,'GID': location.href.split('?GID=')[1]
            }
        }
        $http(accounts).success(function (response) {
            $scope.GetDocumentTypes(1);
            $scope.ShipmentDocuments = new DocumentType(0, 0, 0, '', '');
            $('#Mimefile').val("");
            if (response.d > 0) {
                showStickyToast(true, "Successfully uploaded");
                $("#SupModal").modal('hide');
            }
            else {
                showStickyToast(false, "Error while uploading");
            }
        });
    }

    $scope.SaveGateOutTime=function()
    {
        if($scope.GateEntryObj.OutdateTime=='' || $scope.GateEntryObj.OutdateTime==undefined || $scope.GateEntryObj.OutdateTime==null)
        {
                showStickyToast(false, "Please enter Out Time");
                return false;
        }
             var accounts = {
                        method: 'POST',
                        url: 'GateEntry.aspx/UpsertGateOutInfo',
                        headers: {
                            'Content-Type': 'application/json; charset=utf-8',
                            'dataType': 'json'
                        },
                        data: {
                            'outtime': $scope.GateEntryObj.OutdateTime,'GID': location.href.split('?GID=')[1]
                        }
                    }
                    $http(accounts).success(function (response) {
                        $scope.GetDocumentTypes(0);
                        $scope.getgateentryDetails();
                                if (response.d > 0) {
                                    showStickyToast(true, "Successfully Updated");
                                    $("#SupModal").modal('hide');
                                }
                                else {
                                    showStickyToast(false, "Error while adding");
                                }
                    });
    }

    $scope.checkIsreturnload = function () {
        debugger;
        if ($scope.PreferedLocationdata != undefined) {
            if ($scope.PreferedLocationdata.length != 0) {
                if ($scope.GateEntryObj.IsRetutnLoad == undefined) {
                    showStickyToast(false, "unable to change once preferred Location added");
                    $scope.$scope.GateEntryObj.IsRetutnLoad = true;
                }
            }
        }
    }

//---------------------------- calculate Est. Dock Out Time ----------------------------------------//

// $scope.SaveGateOutTime=function()
//{
//debugger;
//     if($scope.GateEntryObj.ESTDockInTime=='' || $scope.GateEntryObj.ESTDockInTime==undefined || $scope.GateEntryObj.ESTDockInTime==null)
//        {
                
//                return false;
//        }
//    if($scope.GateEntryObj.ESTOperationTime=='' || $scope.GateEntryObj.ESTOperationTime==undefined || $scope.GateEntryObj.ESTOperationTime==null)
//        {
                
//                return false;
//        }
//             var accounts = {
//                        method: 'POST',
//                        url: 'GateEntry.aspx/CalculateExpectedOutTime',
//                        headers: {
//                            'Content-Type': 'application/json; charset=utf-8',
//                            'dataType': 'json'
//                        },
//                        data: {
//                            'time': $scope.GateEntryObj.ESTDockInTime,'minutes':  $scope.GateEntryObj.ESTOperationTime
//                        }
//                    }
//                    $http(accounts).success(function (response) {
//                        $scope.GateEntryObj.ESTDockoutTime=response.d;
//                    });
//}
    


    
});
function GateEntryHeader(OutdateTime,status,statusid,arrivingcityid,arrivingState,unloadedQty,dockid,dock,gateid,city,ispreferedLocation,isRetutnLoad,arrivingFrom,SKUqty,boxqty,lrno,containerSeal,capacityInfo,permitInfo,Frieghtcompanyid,accountid, tenantid, tenant,vehicleId, warehouseid, intime, estimatedtime, indrivername, indriverno, inboundNo, InboundId, estIBDockAssignTime, countryarrivingfrom, cityarrivingfrom,
    estoperationtime, ibdockassigntime, oboperationtime, ibDockOperationTime, permittedWaitTime, outDriver, outDriverNo, outBoundNo, outboundId, estDockAssignTime, obdockAssignTime, leavingToCountry
    , leavingToCity, obDockOperationTime, outTimeStamp, preferedDestinationCountry, reservedforOutbound,gateEntryType,vehicle,unloadingTime) {
    debugger;
    this.OutdateTime = OutdateTime;
    this.Status = status;
    this.StatusId = statusid;
    this.ArrivingCityId = arrivingcityid;
    this.ArrivingStateId = arrivingState;
    this.UnloadedQty=unloadedQty;
    this.DockID=dockid;
    this.Dock=dock;
    this.GateEntryId=gateid;
    this.PreferedCity=city;
    this.IsPreferedLocationId = ispreferedLocation;
    this.IsRetutnLoad = isRetutnLoad;
    this.ArrivingFromId = arrivingFrom;
    this.SKUQty = SKUqty;
    this.BOXQty = boxqty;
    this.LRNO = lrno;
    this.ContainerSeal = containerSeal;
    this.CapacityInfo = capacityInfo;
    this.PermitInfo = permitInfo;
    this.FrieghtCompanyid = Frieghtcompanyid;
    this.AccountId = accountid;
    this.TenantId = tenantid;
    this.GateEntryType=gateEntryType;
    this.Vehicle=vehicle;
    this.Tenant = tenant;
    this.VehicleId = vehicleId;
    this.WareHouseId = warehouseid;
    this.EstmatedInTime = estimatedtime;
    this.InTime = intime;
    this.InDriverName = indrivername;
    this.InDriverNo = indriverno;
    this.InboundNo = inboundNo;//
    this.InboundId = InboundId;//
    this.EstIBDockAssignTime = estIBDockAssignTime;
    this.CountryArrivingFrom = countryarrivingfrom;
    this.CityArrivingFrom = cityarrivingfrom;
    this.EstIBOperationTime = estoperationtime;
    this.IBDockAssignTime = ibdockassigntime;
    this.OBOperationTime = oboperationtime;
    this.IBDockOperationTime = ibDockOperationTime;
    this.PermittedWaitTime = permittedWaitTime;
    this.OutDriver = outDriver;
    this.OutDriverNo = outDriverNo;
    this.OutBoundNo = outBoundNo;
    this.OutBoundId = outboundId;
    this.EstOBDockAssignTime = estDockAssignTime;
    this.OBDockAssignTime = obdockAssignTime;
    this.LeavingToCountry = leavingToCountry;
    this.OBDockOperationTime = obDockOperationTime;
    this.OutTimeStamp = outTimeStamp;
    this.PreferedDestinationCountry = preferedDestinationCountry;
    this.ReservedForOutbound = reservedforOutbound;
    this.UnloadingTime = unloadingTime;
   

}
function PreferedLocation(Id,gateid, stateid, state, countryid, country, cityid, city)
{
    this.PreferedLocationId = Id;
    this.GateId = gateid;
    this.StateID = stateid;
    this.State = state;
    this.CountryID = countryid;
    this.Country = country;
    this.CityID = cityid;
    this.City = city;
}
function GateEntryDocks(Id, gateid, Dockid, Dock, lr, ESTDockIntime, ESTOperationtime, ESTDockouttime, skuqty, boxqty, actualdockintime, actualoperationtime, actualdockouttime, shipmentid) {
    this.VehicleDocking_ID = Id;
    this.GateId = gateid;
    this.DockID = Dockid;
    this.Dock = Dock;
    this.LR = lr;
    this.ESTDockInTime = ESTDockIntime;
    this.ESTOperationTime = ESTOperationtime;
    this.ESTDockoutTime = ESTDockouttime;
    this.SKUQty = skuqty;
    this.BOXQty = boxqty;
    this.Actualdockintime = actualdockintime;
    this.Actualoperationtime = actualoperationtime;
    this.Actualdockouttime = actualdockouttime;
    this.ShipmentId = shipmentid;
   
}

function DocumentType(Id, GateID,Mimeid, DocumentTypeID, DocumentName, DocumentURI) {
    this.ShipmentDocumentID = Id;
    this.GateId = GateID;
    this.MIMEID = Mimeid;
    this.DocumentTypeID = DocumentTypeID;
    this.DocumentName = DocumentName;
    this.DocumentURI = DocumentURI;
   
}
function isNumber(evt) {
    var iKeyCode = (evt.which) ? evt.which : evt.keyCode
    if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
        return false;
    return true;
}








