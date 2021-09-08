var app = angular.module('MyApp', ['angularUtils.directives.dirPagination']);


app.controller('NewWarehouse', function ($scope, $http, $timeout) {
app.directive('ngConfirmClick', [
        function () {
            return {
                link: function (scope, element, attr) {
                    var msg = attr.ngConfirmClick || "Are you sure?";
                    var clickAction = attr.confirmedClick;
                    element.bind('click', function (event) {
                        if (window.confirm(msg)) {
                            scope.$eval(clickAction)
                        }
                    });
                }
            };
        }])

    var accountid = 0;
    var WHID = 0;
    $scope.warehousedata = new WareHouse(0, '', '', 0, '', 0, 0, '', '', '', '', '',
        '', 0, 0, 0, '', '', '', '', '', '', '', '', 0, 0, 0, '', '', 0, 0, 0, 0, parseInt(accountid));
    $scope.Warehouseobj;
    if (window.location.href.indexOf("accountid") > -1) {
        var obj = location.href.split('?')[1].split('&');
        if (obj.length == 1) {
            debugger;
            accountid = obj[0].split('=')[1];
            $scope.warehousedata.AccountId = parseInt(accountid);
        }
        if (obj.length > 1) {
            debugger;

            accountid = obj[0].split('=')[1];
            $scope.warehousedata.AccountId = parseInt(accountid);
            WHID = obj[2].split('=')[1];
            //if (WHID > 0) {
            //    showStickyToast(true, "Saved Successfully ", false);
            //}
            //  alert(WHID);
        }

    }
    // alert(accountid);
    // accountid = new URL(window.location.href).searchParams.get("accountid");
    // WHID = new URL(window.location.href).searchParams.get("WareHouseID");

    //
    //if (location.href.split('?accountid=')[1] > 0) {
    //    $scope.fromAccount = true;
    //    accountid = location.href.split('?accountid=')[1];
    //    //disableaccount = true;
    //}
    //else {
    //    disableaccount = false;
    //}




    $scope.GetWareHouseList = function () {
        debugger;
        var states = {
            method: 'POST',
            url: '../TPL/NewAccount.aspx/GetWareHouseList',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'AccountId': accountid },
            async: false,
        }
        $http(states).success(function (response) {
            debugger;
            $scope.WarehouseListdata = JSON.parse(response.d);
           
            var WH = parseInt(WHID);
            if (WH == 0 || WH == null || WH == "" || WH == undefined) {
                debugger; 
                $('#Chkisactive').prop('checked', true);
            }
            //alert(WH);
            debugger;
            if (WH != 0) {
                $scope.Warehouseobj = $.grep($scope.WarehouseListdata, function (a) { return a.WarehouseID == WHID });

                $scope.whlist = false;
                $scope.DisplayNewWareHouse = true;
                var data = $scope.Warehouseobj[0];
                $scope.getCurrency(data.CountryMasterID);
                $scope.getCities(data.StateID);
                $scope.getZipCode(data.CityID);
                $scope.warehouse = data.WarehouseID
                $scope.warehousename = data.WHCode + '-' + data.WHName


                $scope.warehousedata = new WareHouse(data.WarehouseID, data.WHName, data.WHCode, data.WarehouseGroupID, data.Location, data.RackingTypeID,
                    data.WarehouseTypeID, '', data.AddressLine1, data.FloorSpace, '', 0, '',
                    data.CountryMasterID, data.CurrencyID, data.InOutID, data.PCP_Name, data.PCP_Mobile, data.PCP_Email,
                    data.PCP_Address, data.SCP_Name, data.SCP_Mobile, data.SCP_Email, data.SCP_Address, data.StateID, data.CityID, data.ZipCodeID, data.Latitude
                    , data.Longitude, data.Lenght, data.Width, data.Height, data.GEN_MST_PreferenceOption_ID, data.AccountID, data.isdeleted);//added accountid
                $scope.warehousedata.Currency = data.CurrencyID;
                $scope.warehousedata.ZipCodeId = data.ZipCodeID;
              //  $scope.
                //$('#ChkConfigure' + i).prop('checked', true);
                //$("#Chkisactive").prop('checked').val(data.Isactive);


                if (data.IsActive == 0) {
                    $('#Chkisactive').prop('checked', false);
                    $('#btncreatedc').prop("disabled", true);
                    $('#btncreatezn').prop("disabled", true);
                }
                else { $('#Chkisactive').prop('checked', true);}

                //$("#Chkisactive").prop(data.Isactive == true ? 'checked' : 'unchecked');
                if (data.WarehouseID != 0) {
                    $('#btncreatewh').html('Save Warehouse');
                }
            }
        });
    }

    $scope.GetWareHouseList();

    $scope.displaylist = function () {
        $scope.whlist = true;
        $scope.DisplayNewWareHouse = false;
    }
    $scope.PDFDownload = function (obj) {

        if (obj == 0) {
            showStickyToast(false, "Unable to Download PDF ");
            return false;
        }

        var pdf = {
            method: 'POST',
            url: '../TPL/NewWarehouse.aspx/getBarCode',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'Docid': obj }
        }
        $http(pdf).success(function (response) {

            if (response.d != null) {
                var obj = response.d;
                var link = document.createElement('a');
                link.href = 'BarcodePdf/' + obj;
                link.download = obj;
                link.dispatchEvent(new MouseEvent('click'));
                showStickyToast(true, "PDF Generated Successfully ");
                return false;

            }
            else {
                showStickyToast(false, "Error while generating PDF ");
                return false;
            }
        });
    }


//});
    //Get Dock List
    $scope.GetDockList = function () {

        debugger;
        var states = {
            method: 'POST',
            url: '../TPL/NewWarehouse.aspx/GetDocList',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'AccountId': accountid, 'WarehouseID': WHID }
        }
        $http(states).success(function (response) {
            debugger;
            $scope.DockListdata = response.d;
        });
    }
    $scope.GetDockList();

    //Get Zone List
    $scope.GetZoneList = function () {
        debugger;
        var states = {
            method: 'POST',
            url: '../TPL/NewWarehouse.aspx/GetZoneList',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'AccountId': accountid, 'WarehouseID': WHID }
        }
        $http(states).success(function (response) {
            debugger;
            $scope.ZoneListdata = response.d;
        });
    }
    $scope.GetZoneList();

    //Get Dock Data 
    $scope.getDocData = function (info) {
        debugger;
        $('#imgloader').hide();
        $('#btnsavedck').css('display', 'block');
        $("#DockModal").modal({
            show: 'true'
        });
        if (info != null) {
            $scope.warehousename = info.WarehouseName;
            $scope.warehouse = info.WarehouseID;
            $scope.docnumber = info.DockNumber;
            $scope.docname = info.DockName;
            $scope.doctype = info.DockTypeID;
            $scope.dockids = info.DockID;
        }
        else {
            $scope.docnumber = '';
            $scope.docname = '';
            $scope.doctype = '';
            $scope.dockids = '';
        }
    }

    //Get Zone Data
    $scope.getZoneData = function (info) {
        debugger;
        // var dockcheck = $.grep(info, function (a) { return a.IsDockZone == "True" });
        if (info != null) {
            if (info.IsDockZone == "True") {
                // $('#dveditzone').prop("disabled", true);
                //$('#dveditzone').parent('.tool').prepend('<span style="display:inline-block;padding: 14px 58px;" data-tooltip="Additional warehouses cannot be created as the Warehouse Limit is reached."></span>');
                $("#ZoneModal").modal({
                    show: 'true'
                });

            }
            else {
                $("#ZoneModal").modal({
                    show: 'true'
                });
            }
        }

        else {
            $("#ZoneModal").modal({
                show: 'true'
            });
        }
        if (info != null) {
            $scope.warehousename = info.WarehouseName;
            $scope.warehouse = info.WarehouseID;
            $scope.zonecode = info.ZoneCode;
            $scope.zonedesc = info.ZoneDesc;
            $scope.zoneids = info.ZoneID;

            var Editzones = {
                method: 'POST',
                url: '../TPL/NewWarehouse.aspx/EditZone',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'dataType': 'json'
                },
                data: {
                    'ZoneId': $scope.zoneids
                }
            }
            $http(Editzones).success(function (response) {
                debugger;
                //$scope.delres = response.d;
                if (response.d == 0) {
                    //showStickyToast(true, "Successfully Deleted");
                    $('#zncode').prop("disabled", false);
                    $('#zndesc').prop("disabled", false);
                }
                else {
                    //showStickyToast(false, "Could not Delete since ISDockzone/location is mapped");
                    $('#zncode').prop("disabled", true);
                    $('#zncode').parent('.tool').prepend('<span style="display:inline-block;padding: 14px 58px;" data-tooltip="Cannot Edit the Zone Code as Locations are mapped to this Zone"></span>');
                    $('#zndesc').prop("disabled", true);
                    $('#zndesc').parent('.tool').prepend('<span style="display:inline-block;padding: 14px 58px;" data-tooltip="Cannot Edit the Zone Description as Locations are mapped to this Zone"></span>');
                    return false;
                }

            });
        }
        else {
            $scope.zonecode = '';
            $scope.zonedesc = '';
            $scope.zoneids = '';
            $('#zncode').prop("disabled", false);
            $('#zndesc').prop("disabled", false);
        }
    }
    $scope.deleteZoneData = function (data) {
        if (confirm("Are you sure do you want to delete?")) {
            // todo code for deletion
       
        debugger;
        var Zid = data.ZoneID;
        var zones = {
            method: 'POST',
            url: '../TPL/NewWarehouse.aspx/DeleteZone',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: {
                'ZoneId': Zid, 'WHID': WHID
            }
        }
        $http(zones).success(function (response) {
           debugger;
            //$scope.delres = response.d;
            if (response.d == 0) {

                //$scope.GetWareHouseList();
                //$scope.GetSubscriptionListList();
                //$scope.GetUserList();
                $scope.GetZoneList();
                showStickyToast(true, "Successfully Deleted");
                
            }
            else {
                showStickyToast(false, "Could not Delete since ISDockzone/location is mapped");
                return false;
            }

            });
        }
        else {
            return false;
        }
    }
    //Add location by meena
    $scope.AddLoc = function (info) {
        debugger;
        location.href = "../mMaterialManagement/LocationManager.aspx?ACCID=" + accountid + "&&WHId=" + WHID + "&&ZNId=" + info.ZoneID;

    }

    //Back To List 
    $scope.displaylist = function () {
        location.href = "../TPL/NewAccount.aspx?accountid=" + accountid;
    }

    //Zone Clear
    $scope.myZoneclear = function () {
        $("#zncode, #zndesc, #ZoneID").val("");
    }

    //Dock Clear
    $scope.myDockclear = function () {

        $("#docknum, #UnitCost, #dockname, #doctype, #DockID").val("");
    }

    //Account
    var accounts = {
        
        method: 'POST',
        url: '../TPL/NewAccount.aspx/GetAccount',
        headers: {
            'Content-Type': 'application/json; charset=utf-8',
            'dataType': 'json'
        },
        data: { 'Accountid': accountid }
    }
    $http(accounts).success(function (response) {
        debugger;
        $scope.AccountData = response.d;
        if (location.href.split('?accountid=')[1] > 0) {
            $scope.warehousedata.AccountId = parseInt(accountid);
        }
        // $scope.warehousedata.AccountId = accountid;

    });
    //Rack Types
    var racktypes = {
        method: 'POST',
        url: '../TPL/NewAccount.aspx/GetRackTypes',
        headers: {
            'Content-Type': 'application/json; charset=utf-8',
            'dataType': 'json'
        },
        data: {}
    }
    $http(racktypes).success(function (response) {
        $scope.racktypes = response.d;
    });
    //Warehouse Types
    var whtypes = {
        method: 'POST',
        url: '../TPL/NewAccount.aspx/GetWareHouseTypes',
        headers: {
            'Content-Type': 'application/json; charset=utf-8',
            'dataType': 'json'
        },
        data: {}
    }
    $http(whtypes).success(function (response) {
        $scope.whtypes = response.d;
    });
    //In-Out's
    var Inouts = {
        method: 'POST',
        url: '../TPL/NewAccount.aspx/GetInouts',
        headers: {
            'Content-Type': 'application/json; charset=utf-8',
            'dataType': 'json'
        },
        data: {}
    }
    $http(Inouts).success(function (response) {
        ////
        $scope.inouts = response.d;
    });
    //Get Country Names
    var countrynames = {
        method: 'POST',
        url: '../TPL/NewAccount.aspx/GetCountryNames',
        headers: {
            'Content-Type': 'application/json; charset=utf-8',
            'dataType': 'json'
        },
        data: {}
    }
    $http(countrynames).success(function (response) {
        $scope.countrynames = response.d;
    });
    //Get Currency On Select Country
    $scope.getCurrency = function (id) {
        ////
        if (id == 0) {
            if ($scope.warehousedata.Country == undefined || $scope.warehousedata.Country == 0) {
                $scope.currencynames = null;
                showStickyToast(false, "Please Select Country");
                return false;
            }
            id = $scope.warehousedata.Country;
        }
        $scope.GetStates(id);
        var Currency = {
            method: 'POST',
            url: '../TPL/NewAccount.aspx/GetCurrency',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'CountryId': id }
        }
        $http(Currency).success(function (response) {

            $scope.currencynames = response.d;
        });
    }
    //Get States
    $scope.GetStates = function (id) {
        var states = {
            method: 'POST',
            url: '../TPL/NewAccount.aspx/GetStates',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'Countryid': id }
        }
        $http(states).success(function (response) {

            $scope.Statesdata = response.d;
        });
    }
    //Get Cities On State Select
    $scope.getCities = function (id) {
        if (id == 0) {
            if ($scope.warehousedata.StateId == undefined || $scope.warehousedata.StateId == 0) {
                $scope.CityData = null;
                showStickyToast(false, "Please Select State");
                return false;
            }
            id = $scope.warehousedata.StateId;
        }
        var states = {
            method: 'POST',
            url: '../TPL/NewAccount.aspx/GetCities',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'stateid': id }
        }
        $http(states).success(function (response) {
            ////
            $scope.CityData = response.d;
        });
    }
    //Get Zip On Select City
    $scope.getZipCode = function (id) {

        if (id == 0) {
            if ($scope.warehousedata.CityId == undefined || $scope.warehousedata.CityId == 0) {
                $scope.ZipCodeData = null;
                showStickyToast(false, "Please Select City");
                return false;
            }
            id = $scope.warehousedata.CityId;
        }
        var zipcode = {
            method: 'POST',
            url: '../TPL/NewAccount.aspx/GetZipcode',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'cityid': id }
        }
        $http(zipcode).success(function (response) {
            ////
            $scope.ZipCodeData = response.d;
        });
    }
    //Get Latitude Longitude On Select Zip
    $scope.getlatitudelongitude = function () {
        ////
        var address = '';
        var geocoder = new google.maps.Geocoder();
        var latitude, longitude;
        if ($scope.warehousedata.ZipCodeId != undefined && $scope.warehousedata.ZipCodeId != 0) {
            address = $scope.invoicenumbers = $.grep($scope.ZipCodeData, function (strn) {
                return strn.ID == $scope.warehousedata.ZipCodeId;
            });
        }
        else {
            $('#txtlatitude').val('');
            $('#txtlangitude').val('');
            $scope.warehousedata.Latitude = '';
            $scope.warehousedata.Langitude = '';
        }
        geocoder.geocode({ 'address': address[0].Name }, function (results, status) {

            if (status == google.maps.GeocoderStatus.OK) {
                latitude = results[0].geometry.location.lat();
                longitude = results[0].geometry.location.lng();
                //alert("Latitude: " + latitude + "\nLongitude: " + longitude);
                $('#txtlatitude').val(latitude);
                $('#txtlangitude').val(longitude);


                $scope.warehousedata.Latitude = results[0].geometry.location.lat();
                $scope.warehousedata.Langitude = results[0].geometry.location.lng();
            }
        });
    }
    //Get Time Preference
    var time = {
        method: 'POST',
        url: '../TPL/NewAccount.aspx/GetTimePreferences',
        headers: {
            'Content-Type': 'application/json; charset=utf-8',
            'dataType': 'json'
        },
        data: {}
    }
    $http(time).success(function (response) {
        $scope.timepreference = response.d;
    });

    $("#zncode, #Text18").keypress(function (e) {
        var regex = new RegExp("^[a-zA-Z0-9]+$");
        var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
        if (regex.test(str)) {
            return true;
        }

        e.preventDefault();
        return false;
    });

    //Get Warehouse Dropdown for Dock Creation
    var WarehouseList = {
        method: 'POST',
        url: '../TPL/NewWarehouse.aspx/GetWarehouse',
        headers: {
            'Content-Type': 'application/json;charset=utf-8',
            'dataType': 'json'
        },
        data: { 'accountid': accountid }
    }
    $http(WarehouseList).success(function (response) {

        $scope.warehouselist = response.d;
    })
    //Get DocType Dropdown for Dock Creation
    var DocTypeList = {
        method: 'POST',
        url: '../TPL/NewWarehouse.aspx/GetDocType',
        headers: {
            'Content-Type': 'application/json;charset=utf-8',
            'dataType': 'json'
        },
        data: {}
    }
    $http(DocTypeList).success(function (response) {

        $scope.doctypelist = response.d;
    })
    //Create Dock
    $scope.CreateNewDock = function () {
        debugger;

        //var whtype = $("#warehouseid").val();
        var whtype = $scope.warehouse;
        var docknum = $("#docknum").val().trim();
        //var dockname = $("#dockname").val().trim();
        //var docktype = $("#doctype").val();
        var docktype = $scope.doctype;
        var docid = $scope.dockids;
        docid = $scope.dockids == "" ? 0 : $scope.dockids;
        if (whtype == null || whtype == '') {
            showStickyToast(false, "Please select Warehouse ");
            return false;
        }
        if (docknum == null || docknum == "") {
            showStickyToast(false, "Please check all mandatory fields ");
            //showStickyToast(false, "Please enter Dock Number ");
            return false;
        }
        //if (dockname == null || dockname == "") {
        //    showStickyToast(false, "Please check all mandatory fields ");
        //   // showStickyToast(false, "Please enter Dock Name ");
        //    return false;
        //}
        if (docktype == null || docktype == "") {
            showStickyToast(false, "Please check all mandatory fields ");
           // showStickyToast(false, "Please select Dock Type ");
            return false;
        }
        if (docknum.length < 6) {
            showStickyToast(false, "Please enter 6 Digits for Dock");
            return false;
        }


        
        


        if (docid != 0) {
            var checkdocnum = $.grep($scope.DockListdata, function (a) { return (a.DockNumber.toLowerCase()).indexOf(docknum.toLowerCase()) > -1 && a.WarehouseID == whtype && a.DockID != docid });
            if (checkdocnum == 0) {
                $('#btnsavedck').hide();
                $('#imgloader').css('display', 'block');
                var Docks = {
                    method: 'POST',
                    url: '../TPL/NewWarehouse.aspx/UpsertNewDock',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'WarehouseID': whtype, 'DockID': docid, 'DockNum': docknum, 'DockName': docknum, 'DocType': docktype }
                }
                $http(Docks).success(function (response) {
                    if (response.d == '-1') {
                        $('#imgloader').hide();
                        $('#btnsavedck').css('display', 'block');
                        showStickyToast(false, "Please enter Alpha Numeric characters for Dock");
                        return false;
                    }
                    else {
                        $('#imgloader').hide();
                        $('#btnsavedck').css('display', 'block');
                        $scope.GetDockList();
                        showStickyToast(true, "Successfully Saved ");
                        $("#DockModal").modal('hide');
                        //myDockclear();
                    }
                })
            }
            else {
                $('#btnsavedck').hide();
                $('#imgloader').css('display', 'block');
                var checkdocnum1 = $.grep($scope.DockListdata, function (a) { return (a.DockNumber.toLowerCase()).indexOf(docknum.toLowerCase()) > -1 && a.WarehouseID == whtype });
                if (checkdocnum1 != 0) {
                    showStickyToast(false, "Dock No. already exists !");
                    return false;
                }
                else {

                    var Docks1 = {
                        method: 'POST',
                        url: '../TPL/NewWarehouse.aspx/UpsertNewDock',
                        headers: {
                            'Content-Type': 'application/json; charset=utf-8',
                            'dataType': 'json'
                        },
                        data: { 'WarehouseID': whtype, 'DockID': docid, 'DockNum': docknum, 'DockName': docknum, 'DocType': docktype }
                    }
                    $http(Docks1).success(function (response) {
                        if (response.d == '-1') {
                            $('#imgloader').hide();
                            $('#btnsavedck').css('display', 'block');
                            showStickyToast(false, "Please enter Alpha Numeric characters for Dock");
                            return false;
                        }
                        else {
                            $('#imgloader').hide();
                            $('#btnsavedck').css('display', 'block');
                            $scope.GetDockList();
                            showStickyToast(true, "Successfully Saved ");
                            $("#DockModal").modal('hide');
                            //myDockclear();
                        }
                    })
                }
            }
        }
        else {
            var checkdocnum2 = $.grep($scope.DockListdata, function (a) { return (a.DockNumber.toLowerCase()).indexOf(docknum.toLowerCase()) > -1 });
            if (checkdocnum2 != 0) {
                showStickyToast(false, "Dock No. already exists !");
                return false;
            }
            else {
                $('#btnsavedck').hide();
                $('#imgloader').css('display', 'block');
                var Docks2 = {
                    method: 'POST',
                    url: '../TPL/NewWarehouse.aspx/UpsertNewDock',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'WarehouseID': whtype, 'DockID': docid, 'DockNum': docknum, 'DockName': docknum, 'DocType': docktype }
                }
                $http(Docks2).success(function (response) {
                    if (response.d == '-1') {
                        $('#imgloader').hide();
                        $('#btnsavedck').css('display', 'block');
                        showStickyToast(false, "Please enter Alpha Numeric characters for Dock");
                        return false;
                    }
                    else {
                        $('#imgloader').hide();
                        $('#btnsavedck').css('display', 'block');
                        $scope.GetDockList();
                        showStickyToast(true, "Successfully Saved ");
                        $("#DockModal").modal('hide');
                    //myDockclear();
                    }
                  
                })
            }
        }

    }

    var validator = function (value) {
        if (/^[a-zA-Z0-9]*$/.test(value)) {
            ngModel.$setValidity('alphanumeric', true);
            return value;
        } else {
            ngModel.$setValidity('alphanumeric', false);
            return undefined;
        }
    };

    //Create Zone
    $scope.CreateNewZone = function () {

        var warehse = $scope.warehouse;
        var zonecode = $("#zncode").val().trim();
        var zondedesc = $("#zndesc").val().trim();
        var zoneid = $scope.zoneids;
        zoneid = $scope.zoneids == "" ? 0 : $scope.zoneids;
        if (zonecode == null || zonecode == '') {
            showStickyToast(false, "Please check all mandatory fields");
            return false;
        }
        if (zondedesc == null || zondedesc == '') {
            showStickyToast(false, "Please check all mandatory fields");
            return false;
        }
        if (zonecode.length < 2) {
            showStickyToast(false, "Please Enter valid 2 Character Zone Code");
            return false;
        }
        if (zoneid != 0) {
            var checkzonecode = $.grep($scope.ZoneListdata, function (a) { return (a.ZoneCode.toLowerCase()).indexOf(zonecode.toLowerCase()) > -1 && a.WarehouseID == warehse && a.ZoneID != zoneid });
            if (checkzonecode == 0) {
                var Zones = {
                    method: 'POST',
                    url: '../TPL/NewWarehouse.aspx/UpsertNewZone',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'WarehouseID': warehse, 'ZoneID': zoneid, 'ZoneCode': zonecode, 'ZoneDesc': zondedesc }
                }
                $http(Zones).success(function (response) {
                    $scope.GetZoneList();
                    showStickyToast(true, "Successfully saved ");
                    $("#ZoneModal").modal('hide');
                    //myZoneclear();
                })
            }
            else {
                var checkzonecode1 = $.grep($scope.ZoneListdata, function (a) { return (a.ZoneCode.toLowerCase()).indexOf(zonecode.toLowerCase()) > -1 && a.WarehouseID == warehse });
                if (checkzonecode1 != 0) {
                    showStickyToast(false, "Zone Code already exists !");
                    return false;
                }
                else {
                    var Zones1 = {
                        method: 'POST',
                        url: '../TPL/NewWarehouse.aspx/UpsertNewZone',
                        headers: {
                            'Content-Type': 'application/json; charset=utf-8',
                            'dataType': 'json'
                        },
                        data: { 'WarehouseID': warehse, 'ZoneID': zoneid, 'ZoneCode': zonecode, 'ZoneDesc': zondedesc }
                    }
                    $http(Zones1).success(function (response) {
                        $scope.GetZoneList();
                        showStickyToast(true, "Successfully Saved ");
                        $("#ZoneModal").modal('hide');
                        //myZoneclear();
                    })
                }
            }
        }
        else {
            var checkzonecode2 = $.grep($scope.ZoneListdata, function (a) { return (a.ZoneCode.toLowerCase()).indexOf(zonecode.toLowerCase()) > -1 });
            if (checkzonecode2 != 0) {
                showStickyToast(false, "Zone Code already exists !");
                return false;
            }
            else {
                var Zones2 = {
                    method: 'POST',
                    url: '../TPL/NewWarehouse.aspx/UpsertNewZone',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'WarehouseID': warehse, 'ZoneID': zoneid, 'ZoneCode': zonecode, 'ZoneDesc': zondedesc }
                }
                $http(Zones2).success(function (response) {
                    $scope.GetZoneList();
                    showStickyToast(true, "Successfully Saved ");
                    $("#ZoneModal").modal('hide');
                    //myZoneclear();
                })
            }
        }
    }
    $scope.getchkvalue = function () {
        debugger;
        var atr = $("#Chkisactive").prop('checked');
        if (atr == true) {
            $scope.warehousedata.Isactive = 1;
        }
        else {
            $scope.warehousedata.Isactive = 0;
        }
    }
    //Create Warehouse
    $scope.CreateNewWareHouse = function () {


        debugger;

        if ($scope.warehousedata.AccountId == undefined || $scope.warehousedata.AccountId == '' || $scope.warehousedata.AccountId == null) {
            $scope.warehousedata.AccountId = 0;
            showStickyToast(false, "Please check all mandatory fields ");
            return false;
        }
        if ($scope.warehousedata.WHName == undefined || $scope.warehousedata.WHName == '') {
            $scope.warehousedata.WHGroupcode = 0;
            showStickyToast(false, "Please check all mandatory fields ");
            
            return false;
        }
        if ($scope.warehousedata.WHCode == undefined || $scope.warehousedata.WHCode == '') {
            $scope.warehousedata.WHGroupcode = 0;
            showStickyToast(false, "Please check all mandatory fields ");
            return false;
        }
        var WhCOde = $scope.warehousedata.WHCode;
        $scope.warehousedata.IsDeleted = 0;
        var atr = $("#Chkisactive").prop('checked');
        if (atr == true) {
            $scope.warehousedata.Isactive = 1;
        }
        else {
            $scope.warehousedata.Isactive = 0;
        }
        var whcode = WhCOde.length;
        if (whcode < 3) {
            showStickyToast(false, "Please Enter valid 3 Character Warehouse Code");
            return false;
        }
        //if ($scope.warehousedata.WHGroupcode == undefined || $scope.warehousedata.WHGroupcode == '' || $scope.warehousedata.WHGroupcode == null) {
        //    $scope.warehousedata.WHGroupcode = 0;
        //    showStickyToast(false, "Please select warehouse group code ");
        //    return false;
        //}
        if ($scope.warehousedata.RackingRType == undefined || $scope.warehousedata.RackingRType == '' || $scope.warehousedata.RackingRType == null) {
            $scope.warehousedata.RackingRType = 0;
            //showStickyToast(false, "Please Enter First Name ");
            // return false;
        }
        if ($scope.warehousedata.WHtype == undefined || $scope.warehousedata.WHtype == '' || $scope.warehousedata.WHtype == null) {
            $scope.warehousedata.WHtype = 0;
            //showStickyToast(false, "Please Enter First Name ");
            // return false;
        }
        //if ($scope.warehousedata.Inout == undefined || $scope.warehousedata.Inout == '' || $scope.warehousedata.Inout == null) {
        //    $scope.warehousedata.Inout = 0;
        //    showStickyToast(false, "Please select Inout ");
        //    return false;
        //}
        if ($scope.warehousedata.Country == undefined || $scope.warehousedata.Country == '' || $scope.warehousedata.Country == null) {
            $scope.warehousedata.Country = 0;
            showStickyToast(false, "Please check all mandatory fields ");
            return false;
        }
        if ($scope.warehousedata.StateId == undefined || $scope.warehousedata.StateId == '' || $scope.warehousedata.StateId == null) {
            $scope.warehousedata.StateId = 0;
            showStickyToast(false, "Please check all mandatory fields ");
            return false;
        }
        if ($scope.warehousedata.CityId == undefined || $scope.warehousedata.CityId == '' || $scope.warehousedata.CityId == null) {
            $scope.warehousedata.CityId = 0;
            showStickyToast(false, "Please check all mandatory fields");
            return false;
        }
        if ($scope.warehousedata.ZipCodeId == undefined || $scope.warehousedata.ZipCodeId == '' || $scope.warehousedata.ZipCodeId == null) {
            $scope.warehousedata.ZipCodeId = 0;
            showStickyToast(false, "Please check all mandatory fields");
            return false;
        }
        if ($scope.warehousedata.Currency == undefined || $scope.warehousedata.Currency == '' || $scope.warehousedata.Currency == null) {
            $scope.warehousedata.Currency = 0;
            showStickyToast(false, "Please check all mandatory fields");
            return false;
        }
        if ($scope.warehousedata.Time == undefined || $scope.warehousedata.Time == '' || $scope.warehousedata.Time == null) {
            $scope.warehousedata.Time = 0;
            showStickyToast(false, "Please check all mandatory fields");
            return false;
        }
        if ($scope.warehousedata.Pmobile == undefined) {
            $scope.warehousedata.Pmobile = '';
        }


        if ($scope.warehousedata.Pmobile != '' || $scope.warehousedata.Pmobile != "" || $scope.warehousedata.Pmobile == undefined) {
            ////
            var PmobileLength = $scope.warehousedata.Pmobile;
            var PLenght = PmobileLength.length;
            if (PLenght < 10) {
                showStickyToast(false, "Please Enter valid Primary Mobile No. ");
                return false;
            }
        }
        if ($scope.warehousedata.SMobile != '' || $scope.warehousedata.SMobile != "" || $scope.warehousedata.SMobile == undefined) {
            var SmobileLength = $scope.warehousedata.SMobile;
            var SLenght = SmobileLength.length;
            if (SLenght < 10) {
                showStickyToast(false, "Please Enter valid Secondary Mobile No. ");
                return false;
            }
        }
        if ($scope.warehousedata.SMobile == undefined) {
            $scope.warehousedata.SMobile = '';
        }
        $scope.warehousedata.Length = $("#txtlength").val();
        if ($scope.warehousedata.Length == undefined) {
            $scope.warehousedata.Length = 0;
        }
        $scope.warehousedata.Width = $("#txtwidth").val();
        if ($scope.warehousedata.Width == undefined) {
            $scope.warehousedata.Width = 0;
        }
        $scope.warehousedata.Height = $("#txtheight").val();
        if ($scope.warehousedata.Height == undefined) {
            $scope.warehousedata.Height = 0;
        }
        $scope.warehousedata.FloorSpace = $("#txtfloorspace").val();
        if ($scope.warehousedata.FloorSpace == undefined) {
            $scope.warehousedata.FloorSpace = '';
        }
        if ($scope.warehousedata.Inout == undefined || $scope.warehousedata.Inout == '' || $scope.warehousedata.Inout == null) {
            $scope.warehousedata.Inout = 3;
          
            //showStickyToast(false, "Please Enter First Name ");
            // return false;
        }
        var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        var isemail;
        if ($scope.warehousedata.pEmail != '' && $scope.warehousedata.pEmail != undefined) {
            isemail = regex.test($scope.warehousedata.pEmail);
            if (!isemail) {
                showStickyToast(false, "Please Enter valid Primary Email ");
                return false;
            }
        }
        if ($scope.warehousedata.SEmail != '' && $scope.warehousedata.SEmail != undefined) {
            isemail = regex.test($scope.warehousedata.SEmail);
            if (!isemail) {
                showStickyToast(false, "Please Enter valid Secondary Email ");
                return false;
            }
        }
        var Currency = {
            method: 'POST',
            url: '../TPL/NewWarehouse.aspx/UpsertNewWareHouse',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'obj': $scope.warehousedata, 'accountid': parseInt(accountid) }
        }
        $http(Currency).success(function (response) {
            debugger;
            $scope.GetWarehouseid = response.d;
            if (response.d > 0) {
                if ($scope.warehousedata.WareHouseID != 0) {
                    showStickyToast(true, "Successfully Saved ");
                }
                else {
                    setTimeout(function () {
                    showStickyToast(true, "Successfully Created ");
                    location.href = "../TPL/NewWarehouse.aspx?accountid=" + accountid + "&&WareHouseID=" + $scope.GetWarehouseid;
                    }, 2500);
                    }
                $scope.GetDockList();
                $scope.getWarehoses();
                


                $scope.GetWareHouseList();
                $scope.warehousedata = new WareHouse(0, '', '', 0, '', 0, 0, '', '', '', '', '',
                    '', 0, 0, 0, '', '', '', '', '', '', '', '', 0, 0, 0, '', '', 0, 0, 0, 0, undefined);//added accountid
                $scope.whlist = true;
                $scope.DisplayNewWareHouse = false;
                return false;
            }
            else if (response.d == -2) {
                showStickyToast(false, "WH Code Already Exist for Selected Account");
                return false;
            }
            else {
                showStickyToast(false, "WH Code already exists");
                return false;
            }
        });

    }
    //$(function () {
    //    $("#DockModal").on('show.bs.modal', function () {
    //        
    //        myDockclear();
    //    });
    //    $("#ZoneModal").on('show.bs.modal', function () {
    //        
    //        myZoneclear();
    //    });
    //});

});
function WareHouse(warehouseid, whname, whcode, whgroupcode, location, rackingtype, whtype, wharea, whaddress, floorspace, measurements, pin,
    geolocation, country, currency, Inout, pName, pMobile, pEmail, paddress, sname, smobile, semail, saddress,
    stateid, cityid, zipcode, latitude, langitude, length, width, height, Time, accountid,IsDeleted) {
    this.WareHouseID = warehouseid;
    this.WHName = whname;
    this.WHCode = whcode;
    this.WHGroupcode = whgroupcode;
    this.Location = location;
    this.RackingRType = rackingtype;
    this.WHtype = whtype;
    this.WHarea = wharea;
    this.WHAddress = whaddress;
    this.FloorSpace = floorspace;
    this.Measurements = measurements;
    this.PIN = pin;
    this.GeoLocation = geolocation;
    this.Country = country;
    this.Currency = currency;
    (Inout == 0 ? "null" : Inout);
    this.Inout = Inout;
    this.pName = pName;
    this.Pmobile = pMobile;
    this.pEmail = pEmail;
    this.PAddress = paddress;
    this.sname = sname;
    this.SMobile = smobile;
    this.SEmail = semail;
    this.SAddress = saddress;
    this.StateId = stateid;
    this.CityId = cityid;
    this.ZipCodeId = zipcode;
    this.Latitude = latitude;
    this.Langitude = langitude;
    this.Length = length;
    this.Width = width;
    this.Height = height;
    this.Time = Time;
    this.AccountId = accountid;
    this.IsDeleted = IsDeleted;

}
//function myDockclear() {
//    $("#docknum, #UnitCost, #dockname, #doctype, #DockID").val("");
//}
//function myZoneclear() {
//    $("#zncode, #zndesc, #ZoneID").val("");

//}

