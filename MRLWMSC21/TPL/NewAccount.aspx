<%@ Page Title="Account Creation" Language="C#" MasterPageFile="~/mMaterialManagement/MaterialManagementMaster.master" AutoEventWireup="true" CodeBehind="NewAccount.aspx.cs" Inherits="MRLWMSC21.TPL.NewAccount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
    <asp:ScriptManager runat="server" EnablePartialRendering="true" ID="PODetailsScript" SupportsPartialRendering="true"></asp:ScriptManager>
    <script src="Scripts/jquery-ui-1.8.24.min.js"></script>
    <script type="text/javascript" src="Scripts/CommonScripts.js"></script>

    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
    <link href="Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="Scripts/toast/jquery.toastmessage.js"></script>
    <script src="../mInventory/Scripts/angular.min.js"></script>
  <%--  <script src="../Admin/Scripts/angular.min.js"></script>--%>
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>
    <link href="../mInventory/Scripts/bootstrap.min.css" rel="stylesheet" />
    <script src="../mInventory/Scripts/dirPagination.js"></script>
    <%--<script src="../Admin/Scripts/dirPagination.js"></script>--%>
    <%--<script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyB-amPYw4EvJGyYfY16HzhF2lqpw--FcHM&libraries=places"></script>--%>

    <script type="text/javascript">
        function filterDigits(eventInstance) {
            eventInstance = eventInstance || window.event;
            key = eventInstance.keyCode || eventInstance.which;
            if ((47 < key) && (key < 58) || key == 8)
            {
                return true;
            }
            else
            {
                if (eventInstance.preventDefault)
                    eventInstance.preventDefault();
                eventInstance.returnValue = false;
                return false;
            }
        }
    </script>
    <script>
        var app = angular.module('MyApp', ['angularUtils.directives.dirPagination']);
        app.directive('allowOnlyNumbers', function () {
            return {
                restrict: 'A',
                link: function (scope, elm, attrs, ctrl) {
                    elm.on('keydown', function (event) {
                        if (event.which == 64 || event.which == 16) {
                            // to allow numbers  
                            return false;
                        } else if (event.which >= 48 && event.which <= 57) {
                            // to allow numbers  
                            return true;
                        } else if (event.which >= 96 && event.which <= 105) {
                            // to allow numpad number  
                            return true;
                        } else if ([8, 13, 27, 37, 38, 39, 40].indexOf(event.which) > -1) {
                            // to allow backspace, enter, escape, arrows  
                            return true;
                        } else {
                            event.preventDefault();
                            // to stop others  
                            return false;
                        }
                    });
                }
            }
        });
        app.controller('NewUser', function ($scope, $http, $timeout, $window) {
            debugger;

            debugger;
            $scope.whdelete = true; 
          <%--  var Role = [];
            Role = '<%=this.cp.Roles %>';
            for (var i = 0; i < Role.length; i++)
            {
                var roleid;
            }--%>
            $scope.userWH = '';
           $scope.UserId1=<%=UserId%>
            var strname = '<%=UserRole1%>';
            //user Role ID
            var UserRoleId = '<%=UserRoledat%>';
            $scope.userWH = '<%=WH%>';
            //alert(UserId1);
            //alert(UserRoleId);
            debugger;
            $scope.UserRoleId = UserRoleId.split(',');
            $scope.testedit=true;
           // alert($scope.UserRoleId);
           // alert(jQuery.inArray("6", $scope.UserRoleId));

            $scope.role = jQuery.inArray("5", $scope.UserRoleId);
            //$scope.role1 = $scope.UserRoleId.includes("5");

            //It returns if exists true  else false
          //  alert($scope.UserRoleId.includes("5"));

        // jQuery.inArray("test", myarray) != -1
            //var s=$scope.UserId.indexOf("3");
           
            
       
            //function httpGet(theUrl) {
            //    var xmlHttp = new XMLHttpRequest();
            //    xmlHttp.open("GET", theUrl, false); // false for synchronous request
            //    xmlHttp.send(null);
            //    return xmlHttp.responseText;
            //}
            //var accountid = 0;
            //$scope.GetResponse = function () {

            //        //debugger;
            //        var suburl = {
            //            //method: 'GET',
            //            //url: 'http://192.168.1.20/SSOServices/Service/IInventraxSSO.svc/GetSubscriptionByAccount/54',
            //            //headers: {
            //            //    'Content-Type': 'application/json; charset=utf-8',
            //            //    'dataType': 'json'
            //            //},
            //            //data: '{ACCOUNTID:54}',
            //            //async:false,
            //        }
            //        $http.get('http://192.168.1.20/SSOServices/Service/IInventraxSSO.svc/GetSubscriptionByAccount/54').success(function (response) {
            //            //debugger;
            //            alert();
            //            //$scope.SubscriptionData = JSON.parse(response.d);
            //            //console.log($scope.SubscriptionData);

            //        });
            //    }
            //    $scope.GetResponse();
            $scope.ssouserid = 0;    // added by lalitha on 21/03/2019 

            $scope.name = 'lakshmi sridurga';
            $scope.Genderdata = [{ Name: 'Male', ID: 1 }, { Name: 'FeMale', ID: 2 }]
            $scope.DisplayNewWareHouse = false;
            $scope.whlist = true;
            $scope.disableaccount = false;
            $scope.UserList = true;
            $scope.Disabled = false;
            $scope.WarehouseListdata = [];
            $scope.warehousedata = new WareHouse(0, '', '', 0, '', 0, 0, '', '', '', '', '',
                '', 0, 0, 0, '', '', '', '', '', '', '', '', 0, 0, 0, '', '', 0, 0, 0, 0, undefined);//added accountid

            $scope.fromAccount = false;
            ////debugger;

            $scope.selectedValues = [];
            //$scope.btntext = "ADD";
            $scope.$watch('selectedcategories ', function (nowSelected) {
                ////debugger;
                $scope.selectedValues = [];
                if (!nowSelected) {
                    return;
                }
                angular.forEach(nowSelected, function (val) {
                    $scope.selectedValues.push(val.id.toString());
                });
            });
            var accountid = 0
            //debugger;
            if (location.href.split('?accountid=')[1] > 0) {
             
                $scope.fromAccount = true;
                accountid = location.href.split('?accountid=')[1];
                disableaccount = true;
            }
            else {
                disableaccount = false;
            }
            $scope.UserData = new User(0, accountid, 0, '', '', '', '', 0, '', '', '', '', '', false, '', '', 0, 0, 0);
            var accounts = {
                method: 'POST',
                async: false,
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
                    $scope.warehousedata.AccountId = accountid;
                }
                // $scope.warehousedata.AccountId = accountid;

            });


            $scope.getTenant = function () {
                //debugger;
                var tenant = {
                    method: 'POST',
                    url: '../TPL/NewAccount.aspx/GetTenants',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'accountid': accountid }
                }
                $http(tenant).success(function (response) {
                    $scope.TenantData = response.d;


                });

            }
                  $scope.getUsersDrop = function () {
                debugger;
                var tenant = {
                    method: 'POST',
                    url: '../TPL/NewAccount.aspx/GetUsersDrop',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'accountid': accountid, 'tenantid':0 }
                }
                      $http(tenant).success(function (response) {
                          debugger;
                    $scope.UsersData = response.d;


                });

            }
            $scope.getTenant();
            $scope.getUsersDrop();

            var Utypes = {
                method: 'POST',
                url: '../TPL/NewAccount.aspx/GetUserTypes',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'dataType': 'json'
                },
                data: {}
            }
            $http(Utypes).success(function (response) {
                $scope.usertypes = response.d;
            });



            var tablenames = {
                method: 'POST',
                url: '../TPL/NewAccount.aspx/GetWareHouseCode',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'dataType': 'json'
                },
                data: {}
            }
            $http(tablenames).success(function (response) {
                $scope.WareHouseCodes = response.d;
            });
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

            var printer = {
                method: 'POST',
                url: '../TPL/NewAccount.aspx/GetPrinterData',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'dataType': 'json'
                },
                data: {}
            }
            $http(printer).success(function (response) {
                $scope.PrinterData = response.d;
            });

            var roles = {
                method: 'POST',
                url: '../TPL/NewAccount.aspx/GetRoleData',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'dataType': 'json'
                },
                data: { "usertypeid": 0 }
            }
            $http(roles).success(function (response) {
                $scope.Roles = response.d;
            });
            $scope.getWarehoses = function () {
                debugger;
                var warehouse = {
                    method: 'POST',
                    url: '../TPL/NewAccount.aspx/GetWareHouseData',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'Accountid': accountid }
                }
                $http(warehouse).success(function (response) {
                    $scope.warehouses = response.d;
                });
            }

            $scope.getWarehoses();

            $scope.getCurrency = function (id) {
                ////debugger;
                if (id == 0) {
                    if ($scope.warehousedata.Country == undefined || $scope.warehousedata.Country == 0) {
                        $scope.currencynames = null;
                        showStickyToast(false, "Please select Country");
                       
                        return false;
                    }
                    id = $scope.warehousedata.Country;
                }

                $scope.getStates(id);
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
                    ////debugger;
                    $scope.currencynames = response.d;
                });
            }

            ////debugger;
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
                ////debugger;
                $scope.inouts = response.d;
            });
            $scope.getStates = function (id) {
                if (id == 0) {
                    if ($scope.warehousedata.Country == undefined || $scope.warehousedata.Country == 0) {
                        $scope.Statesdata = null;
                        showStickyToast(false, "Please select Country");
                        return false;
                    }
                    id = $scope.warehousedata.Country;
                }

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
                    ////debugger;
                    $scope.Statesdata = response.d;
                });
            }

            $scope.getCities = function (id) {
                if (id == 0) {
                    if ($scope.warehousedata.StateId == undefined || $scope.warehousedata.StateId == 0) {
                        $scope.CityData = null;
                        showStickyToast(false, "Please select State");
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
                    ////debugger;
                    $scope.CityData = response.d;
                });
            }
            $scope.getZipCode = function (id) {
                ////debugger;
                if (id == 0) {
                    if ($scope.warehousedata.CityId == undefined || $scope.warehousedata.CityId == 0) {
                        $scope.ZipCodeData = null;
                        showStickyToast(false, "Please select City");
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
                    ////debugger;
                    $scope.ZipCodeData = response.d;
                });
            }
            $scope.getlatitudelongitude = function () {
                ////debugger;
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


            $scope.CreateNewWareHouse = function () {
                debugger;
                if ($scope.warehousedata.AccountId == undefined || $scope.warehousedata.AccountId == '' || $scope.warehousedata.AccountId == null) {
                    $scope.warehousedata.AccountId = 0;
                    showStickyToast(false, "Please select Account ");
                    return false;
                }
                if ($scope.warehousedata.WHName == undefined || $scope.warehousedata.WHName == '') {
                    $scope.warehousedata.WHGroupcode = 0;
                    showStickyToast(false, "Please enter WH Name ");
                    return false;
                }
                if ($scope.warehousedata.WHCode == undefined || $scope.warehousedata.WHCode == '') {
                    $scope.warehousedata.WHGroupcode = 0;
                    showStickyToast(false, "Please enter WH Code ");
                    return false;
                }
                //debugger;
                var WhCOde = $scope.warehousedata.WHCode;
                var whcode = WhCOde.length;
                if (whcode < 3) {
                    showStickyToast(false, "Please enter valid 3 character Warehouse Code");
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
                if ($scope.warehousedata.Inout == undefined || $scope.warehousedata.Inout == '' || $scope.warehousedata.Inout == null) {
                    $scope.warehousedata.Inout = 0;
                    showStickyToast(false, "Please select Inout ");
                    return false;
                }
                if ($scope.warehousedata.Country == undefined || $scope.warehousedata.Country == '' || $scope.warehousedata.Country == null) {
                    $scope.warehousedata.Country = 0;
                    showStickyToast(false, "Please select Country ");
                    return false;
                }
                if ($scope.warehousedata.StateId == undefined || $scope.warehousedata.StateId == '' || $scope.warehousedata.StateId == null) {
                    $scope.warehousedata.StateId = 0;
                    showStickyToast(false, "Please select State ");
                    return false;
                }
                if ($scope.warehousedata.CityId == undefined || $scope.warehousedata.CityId == '' || $scope.warehousedata.CityId == null) {
                    $scope.warehousedata.CityId = 0;
                    showStickyToast(false, "Please select city ");
                    return false;
                }
                if ($scope.warehousedata.ZipCodeId == undefined || $scope.warehousedata.ZipCodeId == '' || $scope.warehousedata.ZipCodeId == null) {
                    $scope.warehousedata.ZipCodeId = 0;
                    showStickyToast(false, "Please select Zipcode ");
                    return false;
                }
                if ($scope.warehousedata.Currency == undefined || $scope.warehousedata.Currency == '' || $scope.warehousedata.Currency == null) {
                    $scope.warehousedata.Currency = 0;
                    showStickyToast(false, "Please select Currency ");
                    return false;
                }
                if ($scope.warehousedata.Time == undefined || $scope.warehousedata.Time == '' || $scope.warehousedata.Time == null) {
                    $scope.warehousedata.Time = 0;
                    //showStickyToast(false, "Please select Currency ");
                    //return false;
                }
                if ($scope.warehousedata.Pmobile == undefined) {
                    $scope.warehousedata.Pmobile = '';
                }


                if ($scope.warehousedata.Pmobile != '' || $scope.warehousedata.Pmobile != "" || $scope.warehousedata.Pmobile == undefined) {
                    ////debugger;
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
                if ($scope.warehousedata.Length == undefined) {
                    $scope.warehousedata.Length = '';
                }
                if ($scope.warehousedata.Width == undefined) {
                    $scope.warehousedata.Width = '';
                }
                if ($scope.warehousedata.Height == undefined) {
                    $scope.warehousedata.Height = '';
                }
                if ($scope.warehousedata.FloorSpace == undefined) {
                    $scope.warehousedata.FloorSpace = '';
                }
                if ($scope.warehousedata.Inout == undefined || $scope.warehousedata.Inout == '' || $scope.warehousedata.Inout == null) {
                    $scope.warehousedata.Inout = 0;
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
                    url: '../TPL/NewAccount.aspx/UpsertNewWareHouse',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'obj': $scope.warehousedata, 'accountid': location.href.split('?accountid=')[1] }
                }
                $http(Currency).success(function (response) {
                    //debugger;
                    if (response.d > 0) {
                        if ($scope.warehousedata.WareHouseID != 0) {
                            showStickyToast(true, "Successfully Saved ");
                        }
                        else {
                            showStickyToast(true, "Successfully Created ");
                        }
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
                        showStickyToast(false, "Error While Creating");
                    }
                });

            }

            $scope.GetWareHouseList = function () {
//debugger;
var warehouselist=0;
                var states = {
                    method: 'POST',
                    url: '../TPL/NewAccount.aspx/GetWareHouseList',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'AccountId': accountid }
                }
                $http(states).success(function (response) {
                   debugger;
                    $scope.WarehouseListdata = JSON.parse(response.d);
                   // console.log($scope.WarehouseListdata);
                     warehouselist=$.grep($scope.WarehouseListdata,function(a){return a.IsActive==1 });
                    //alert(warehouselist.length);
                     debugger;
                    if ($scope.WarehouseListdata != null && $scope.WarehouseListdata.length > 0) {
                     //   alert($scope.UserId);
                       
                         //if ($scope.UserId == "3" || $scope.UserId == "1") {
                        if ($scope.UserRoleId.indexOf("3") == 0 || $scope.UserRoleId.indexOf("1") == 0) {
                            $('#btncreatenew').prop("disabled", false);
                            $('#btnCreateUser').prop("disabled", false);

                        }
                        else {

                            $('#btncreatenew').prop("disabled", true);
                            $('#btncreatenew').attr("title", "Additional warehouses cannot be created as the Warehouse Limit is reached");
                            //if ($scope.UserId == "5") {
                            if ($scope.UserRoleId.indexOf("5") == 0) {

                                $('#btncreatenew').hide();
                                $scope.whdelete = false;                                                              
                                $scope.UserData.UserTypeID = "4";
                               
                                //  $scope.UserDisabled = true;
                            }
                            else {
                                $('#btnCreateUser').prop("disabled", true);
                                $('#btnCreateUser').attr("title", "Additional users cannot be created as the User Limit is reached");
                            }

                        }


                        if ($scope.WarehouseListdata.length != 0 || $scope.WarehouseListdata.length != undefined) {
                            if ($scope.SubscriptionData[0].NumberOfWarehouses == warehouselist.length) {
                                //$('#btncreatenew').css("display", "none");
                                //  $scope.CreateWareHouseShow = true;
                                $('#btncreatenew').prop("disabled", true);
                                $('#btncreatenew').attr("title", "Additional warehouses cannot be created as the Warehouse Limit is reached");
                                $('#btncreatenew').parent('.tool').prepend('<span style="display:inline-block;    padding: 14px 58px;" data-tooltip="Additional warehouses cannot be created as the Warehouse Limit is reached."></span>');
                                //$('.tool [data-toggle="tooltip"]').tooltip();
                                //$scope.CreateWareHouseShow = false;
                            }
                            else {
                                //$('#btncreatenew').css("display", "block");
                                //$scope.CreateWareHouseShow = true;
                                //  $("#btncreatenew").tooltip('hide');
                                $('#btncreatenew').removeAttr("title");
                            }
                        }
                        else {
                            //$('#btncreatenew').css("display", "block");
                            //  $scope.CreateWareHouseShow = true;
                            $('#btncreatenew').removeAttr("title");

                        }
                    }
                    else {
                     //   alert($scope.UserId);
                         //if ($scope.UserId == "3" || $scope.UserId == "1") {
                         if ($scope.UserRoleId.indexOf("3") == 0 || $scope.UserRoleId.indexOf("1") == 0) {
                             $('#btncreatenew').prop("disabled", false);
                              $('#btnCreateUser').prop("disabled", false);

                         }
                         else {

                             $('#btncreatenew').prop("disabled", true);
                             $('#btncreatenew').attr("title", "Additional warehouses cannot be created as the Warehouse Limit is reached");
                             //if ($scope.UserId == "5") {
                               if ($scope.UserRoleId.indexOf("5") == 0) {
                                 debugger;
                                 $('#btnCreateUser').prop("disabled", false);
                                 $scope.UserData.UserTypeID = 3;
                                 $scope.UserDisabled = true;
                             }
                             else {
                                   $('#btnCreateUser').prop("disabled", true);
                                   $('#btnCreateUser').attr("title", "Additional users cannot be created as the User Limit is reached");
                                    $scope.UserDisabled = false;
                             }
                                                              
                          }
                    }

                });

     
            }
          //  debugger;
            //To get data from this url we need to add "Allow-Control-Allow-Origin: * " extension from chrome web store //
            var ssoaccountid = $('#<%=this.hifssoaccid.ClientID%>').val();
            var Subscription = '<%=ConfigurationManager.AppSettings["ZohoSubsiptionURl"].ToString() %>';
            $scope.GetSubscriptionListList = function () {
               debugger;

                $.ajax({
                    type: 'get',
                    //url: 'http://192.168.1.90/SSOServices_QC/Service/IInventraxSSO.svc/GetSubscriptionByAccount/' + ssoaccountid,
                    //url: 'http://192.168.1.20/SSOServices/Service/IInventraxSSO.svc/GetSubscriptionByAccount/' + ssoaccountid,
                    url: Subscription + ssoaccountid,

                    dataType: 'json',
                    success: function (response) {
                        debugger;
                        var subscriptiondata = JSON.parse(response);
                        $scope.SubscriptionData = subscriptiondata;
                        debugger;
                        if ($scope.SubscriptionData[0].NumberOfWarehouses != 0) {
                            $scope.CreateWareHouseShow = true;
                            if ($scope.WarehouseListdata != null && $scope.WarehouseListdata.length > 0) {
                                if ($scope.WarehouseListdata.length != 0 || $scope.WarehouseListdata.length != undefined) {
                                    if ($scope.SubscriptionData[0].NumberOfWarehouses == warehouselist.length) {
                                        //$('#btncreatenew').css("display", "none");
                                        //$scope.CreateWareHouseShow = true;
                                        $('#btncreatenew').prop("disabled", true);
                                        $('#btncreatenew').attr("title", "Additional warehouses cannot be created as the Warehouse Limit is reached");
                                        // $('.tool [data-toggle="tooltip"]').tooltip();
                                        //$("#btncreatenew").tooltip();
                                        //$scope.CreateWareHouseShow = false;
                                    }
                                }
                                else {
                                    //  $('#btncreatenew').css("display", "block");
                                   // $scope.CreateWareHouseShow = true;
                                    //$("#btncreatenew").tooltip('hide');
                                    $('#btncreatenew').removeAttr("title");
                                }
                            }
                        }
                        else
                        {
                            //$('#btncreatenew').css("display", "block");
                          //  $scope.CreateWareHouseShow = true;
                            $('#btncreatenew').removeAttr("title");

                        }


                    }
                });

            }
             //To get data from this url we need to add "Allow-Control-Allow-Origin: * " extension from chrome web store //

           <%-- $scope.GetSubscriptionListList = function () {
                //debugger;
                accountid= $('#<%=this.hifssoaccid.ClientID%>').val();
                var states = {
                    method: 'POST',
                    url: '../TPL/NewAccount.aspx/GetsubscriptionList',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'AccountId': accountid }
                }
                $http(states).success(function (response) {
                    ////debugger;
                    $scope.SubscriptionData = JSON.parse(response.d);
                    

                });
            }--%>


            $scope.GetUserList = function () {
                //debugger;
                if ($scope.tenant == undefined && $scope.tenant == null)
                {
                    $scope.tenant = '';
                }
                debugger;
                var states = {
                    method: 'POST',
                    url: '../TPL/NewAccount.aspx/GetUserList',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: {
                       'Accountid': accountid, 'tenantid': 0,'UserId':$scope.tenant
                   }
                   // data: JSON.stringify({ Accountid: accountid, tenantid: 0, UserId:  $scope.tenant }),
                }
                $http(states).success(function (response) {
                   debugger;
                    $scope.testedit = response.d.includes("Result");
                    if ($scope.testedit == false) {
                        $scope.UsersList = JSON.parse(response.d);
                        if ($scope.SubscriptionData[0].NumberOfConcurrentUsers == $scope.UsersList.length) {
                            $('#btnCreateUser').prop("disabled", true);
                            $('#btnCreateUser').attr("title", "Additional users cannot be created as the User Limit is reached");
                            $('#btnCreateUser').parent('.tool').prepend('<span style="display:inline-block;    padding: 14px 58px;" data-tooltip="Additional users cannot be created as the User Limit is reached."></span>');
                        }
                        else
                        {
                            $('#btnCreateUser').prop("disabled", false);
                        }
                    }
                    else {
                        $scope.UsersList = null;
                    }

                });
            }


            debugger;
            //if ($scope.AccountData.length != 0) {
            //    debugger;
                if ($("#MainContent_MMContent_txtAccountName").val() != "")
                {
                    debugger;
                     $scope.GetWareHouseList();
                    $scope.GetSubscriptionListList();
                    debugger;
                    $scope.GetUserList();
                }               
           // }
            $scope.getWareHouseData = function (info) {
               
                //WH
                $scope.whlist = true;
                $scope.DisplayNewWareHouse = false;
                var data = info;
                $scope.getCurrency(data.CountryMasterID);
                $scope.getCities(data.StateID);
                $scope.getZipCode(data.CityID);
                
                //$window.open('../TPL/NewWarehouse.aspx?accountid=' + accountid + '&&WareHouseID='+ data.WarehouseID, '_blank');
                location.href = "../TPL/NewWarehouse.aspx?accountid=" + accountid + "&&WareHouseID="+ data.WarehouseID;




                $scope.warehousedata = new WareHouse(data.WarehouseID, data.WHName, data.WHCode, data.WarehouseGroupID, data.Location, data.RackingTypeID,
                    data.WarehouseTypeID, '', data.AddressLine1, data.FloorSpace, '', 0, '',
                    data.CountryMasterID, data.CurrencyID, data.InOutID, data.PCP_Name, data.PCP_Mobile, data.PCP_Email,
                    data.PCP_Address, data.SCP_Name, data.SCP_Mobile, data.SCP_Email, data.SCP_Address, data.StateID, data.CityID, data.ZipCodeID, data.Latitude
                    , data.Longitude, data.Lenght, data.Width, data.Height, data.GEN_MST_PreferenceOption_ID, data.AccountID);//added accountid
                $scope.warehousedata.Currency = data.CurrencyID;
                $scope.warehousedata.ZipCodeId = data.ZipCodeID;
                if (data.WarehouseID != 0) {
                    $('#btncreatewh').html('Save Warehouse');
                }
                   
                if ($scope.WarehouseListdata.length != 0 || $scope.WarehouseListdata.length != undefined) {
                    if ($scope.SubscriptionData[0].NumberOfWarehouses == $scope.WarehouseListdata.length) {
                       // $scope.CreateWareHouseShow = true;
                        $('#btncreatenew').prop("disabled", true);
                        $('#btncreatenew').attr("title", "Additional warehouses cannot be created as the Warehouse Limit is reached");

                       // $('.tool [data-toggle="tooltip"]').tooltip();
                       // $scope.CreateWareHouseShow = false;
                    }
                    else {
                        //$('#btncreatenew').css("display", "block");
                      //  $scope.CreateWareHouseShow = true;
                       // $("#btncreatenew").tooltip('hide');
                        $('#btncreatenew').removeAttr("title");
                    }
                }

            }

            $scope.deleteWareHouseData = function (Wid) {

                if (confirm("Are you sure do you want to delete ?")) {
                    var states = {
                        method: 'POST',
                        url: '../TPL/NewAccount.aspx/DeleteWarehouse',
                        headers: {
                            'Content-Type': 'application/json; charset=utf-8',
                            'dataType': 'json'
                        },
                        data: {
                            'WarehouseID': Wid
                        }
                    }
                    $http(states).success(function (response) {
                        ////debugger;
                        $scope.delres = response.d;
                        if ($scope.delres == "-1") {
                             showStickyToast(false, "Unable to delete the WH as it contains stock");
                        }
                        else if ($scope.delres == "success") {

                            $scope.GetWareHouseList();
                            $scope.GetSubscriptionListList();
                            $scope.GetUserList();
                            showStickyToast(true, "Successfully deleted");
                        }
                        else if ($scope.delres == "Mapped") {
                            showStickyToast(false, "Could not delete since Zone/Dock is mapped");
                            return false;
                        }
                        else {
                            showStickyToast(false, "Error while Delete");
                            return false;
                        }

                    });
                }
            }


            $scope.dispalynewwarehouse = function () {
                ////debugger;
                $scope.warehousedata = new WareHouse(0, '', '', 0, '', 0, 0, '', '', '', '', '',
                    '', 0, 0, 0, '', '', '', '', '', '', '', '', 0, 0, 0, '', '', 0, 0, 0, 0, parseInt(accountid));//added accountid
                $scope.whlist = false;
                $scope.DisplayNewWareHouse = true;
                //$scope.warehousedata.AccountId = accountid;
                if ($scope.WarehouseListdata.length != 0 || $scope.WarehouseListdata.length != undefined) {

                    if ($scope.SubscriptionData[0].NumberOfWarehouses == $scope.WarehouseListdata.length) {
                        //$('#btncreatenew').css("display", "none");
                      //  $scope.CreateWareHouseShow = true;
                        $('#btncreatenew').prop("disabled", true);
                        $('#btncreatenew').attr("title", "Additional warehouses cannot be created as the Warehouse Limit is reached");

                       // $('.tool [data-toggle="tooltip"]').tooltip();
                      //$scope.CreateWareHouseShow = false;
                    }
                    else {
                        //$('#btncreatenew').css("display", "block");
                     //   $scope.CreateWareHouseShow = true;
                       // $("#btncreatenew").tooltip('hide');
                        $('#btncreatenew').removeAttr("title");
                    }
                }

            }
            $scope.displaylist = function () {
                $scope.whlist = true;
                $scope.DisplayNewWareHouse = false;
                if ($scope.WarehouseListdata.length != 0 || $scope.WarehouseListdata.length != undefined) {
                    if ($scope.SubscriptionData[0].NumberOfWarehouses == $scope.WarehouseListdata.length) {
                        //$('#btncreatenew').css("display", "none");
                     //   $scope.CreateWareHouseShow = true;
                        $('#btncreatenew').prop("disabled", true);
                        $('#btncreatenew').attr("title", "Additional warehouses cannot be created as the Warehouse Limit is reached");

                       // $('.tool [data-toggle="tooltip"]').tooltip();
                        //$scope.CreateWareHouseShow = false;
                    }
                    else {
                        //$('#btncreatenew').css("display", "block");
                   //     $scope.CreateWareHouseShow = true;
                      //  $("#btncreatenew").tooltip('hide');
                        $('#btncreatenew').removeAttr("title");
                    }
                }
            }
            $scope.dispalynewwareUser = function () {
                debugger;
                if ($scope.UserRoleId.indexOf("5") == 0) {
                    $scope.UserData = new User(4, parseInt(accountid), 0, '', '', '', '', 0, '', '', '', '', '', true, '', '', 0, 0);

                    $scope.UserDisabled = true;
                    $scope.checktenats();
                }
                else {
                    $scope.UserData = new User(0, parseInt(accountid), 0, '', '', '', '', 0, '', '', '', '', '', true, '', '', 0, 0);
                }
                $scope.selectedWareHouses = null;
                $scope.selectedValues = null;
                $scope.DisplayNewUserDetails = true;
                $scope.UserList = false;
            }
            $scope.displayuserlist = function () {
                $scope.DisplayNewUserDetails = false;
                $scope.UserList = true;
            }
            $scope.CreateNewUser = function () { 
                debugger;
                var data = $scope.UserData;
                if ($scope.UserData.UserTypeID == undefined || $scope.UserData.UserTypeID == '' || $scope.UserData.UserTypeID == null) {
                    $scope.UserData.UserTypeID = 0;
                    showStickyToast(false, "Please check all mandatory fields ");
                    return false;
                }
                 if ($scope.UserData.UserTypeID != undefined || $scope.UserData.UserTypeID != '' || $scope.UserData.UserTypeID != null) {
                     if ($scope.UserData.UserTypeID == "3") {
                          if ($scope.UserData.TenantID == undefined || $scope.UserData.TenantID == '') {
                              $scope.UserData.TenantID = 0;
                              showStickyToast(false, "Please check all mandatory fields ");
                      $('.spTenant').css('display','block');
                    return false;
                    //showStickyToast(false, "Please Select Tenant ");
                    //return false;
                }
                     }
                     
                }
                if ($scope.UserData.TenantID == undefined || $scope.UserData.TenantID == '') {
                    $scope.UserData.TenantID = 0;
                    //showStickyToast(false, "Please Select Tenant ");
                    //return false;
                }
                if ($scope.UserData.FirstName == undefined || $scope.UserData.FirstName == '') {

                    showStickyToast(false, "Please check all mandatory fields ");
                    return false;
                }
                 if ($scope.UserData.EMPCode == undefined || $scope.UserData.EMPCode == '') {

                    showStickyToast(false, "Please check all mandatory fields ");
                    return false;
                }

                if ($scope.UserData.Email == undefined || $scope.UserData.Email == '') {

                    showStickyToast(false, "Please check all mandatory fields ");
                    return false;
                }
                var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
                var isemail;
                isemail = regex.test($scope.UserData.Email);
                if (!isemail) {
                    showStickyToast(false, "Please enter valid Email ");
                    return false;
                }
                 if ($scope.UserData.Password == undefined || $scope.UserData.Password == '') {

                    showStickyToast(false, "Please check all mandatory fields ");
                    return false;
                }

                //if ($scope.UserData.Mobile != null || $scope.UserData.Mobile != undefined) {

                //    if ($scope.UserData.Mobile.length < 10) {
                //        showStickyToast(false, "Please enter mobile number must be 10 digits");
                //        return false;
                //    }
                //}
                if ($scope.selectedValues == null && $scope.selectedValues == undefined) {
                  
                   
                        showStickyToast(false, "Please check all mandatory fields ");
                        return false;
                   
                }
                
                
                if ($scope.UserData.AltEmail1 != '' && $scope.UserData.AltEmail1 != undefined) {
                    isemail = regex.test($scope.UserData.AltEmail1);
                    if (!isemail) {
                        showStickyToast(false, "Please enter valid Alt. Email1 ");
                        return false;
                    }
                }
                else {
                    $scope.UserData.AltEmail1 = '';
                }
                if ($scope.UserData.AltEmail2 != '' && $scope.UserData.AltEmail2 != undefined) {
                    isemail = regex.test($scope.UserData.AltEmail2);
                    if (!isemail) {
                        showStickyToast(false, "Please enter valid Alt. Email2 ");
                        return false;
                    }
                }
                else {
                    $scope.UserData.AltEmail2 = '';
                }
                if ($scope.UserData.MiddleName == null) {
                    $scope.UserData.MiddleName = '';
                }
                if ($scope.UserData.LastNane == null) {
                    $scope.UserData.LastNane = '';
                }

                if ($scope.UserData.Mobile != "" ) {
                    var Mbllength = $scope.UserData.Mobile.length;
                    if (Mbllength < 10) {
                        showStickyToast(false, "Please enter  10 digits Mobile No.");
                        return false;
                    }
                }


                var warehouses = '';
                var roles = '';
                if ($scope.UserData.UserTypeID != 3) {
                     if ($scope.selectedWareHouses == undefined) {
                         showStickyToast(false, "Please check all mandatory fields");
                        return false;
                    }
                }
               
                if ($scope.selectedWareHouses != undefined) {
                    if ($scope.selectedWareHouses.length != 0) {
                        for (var i = 0; $scope.selectedWareHouses.length > i; i++) {
                            warehouses += $scope.selectedWareHouses[i] + ',';

                        }
                        $scope.UserData.warehouses = warehouses.substring(0, warehouses.length - 1);
                    }
                    else {
                        $scope.UserData.warehouses = '';
                    }
                    if ($scope.UserData.UserTypeID==4 && $scope.selectedWareHouses.length > 1)
                    {
                        showStickyToast(false, "Please select only one warehouse");
                        return false;
                    }
                    
                    

                }


                if ($scope.selectedValues != undefined || $scope.selectedValues != null) {
                    if ($scope.selectedValues != undefined || $scope.selectedValues.length != 0) {
                        for (var i = 0; $scope.selectedValues.length > i; i++) {
                            roles += $scope.selectedValues[i] + ',';

                        }
                        $scope.UserData.Roles = roles.substring(0, roles.length - 1);
                    }
                    else {
                        $scope.UserData.Roles = '';
                    }

                }



                data.SSOAccountID = $('#<%=this.hifssoaccid.ClientID%>').val();
                $scope.UserData.CompanyName = $('#<%=this.txtCompanyLegalName.ClientID%>').val();
                $scope.UserData.SSOUserID = $scope.ssouserid;
                debugger;
                var states = {
                    method: 'POST',
                    url: '../TPL/NewAccount.aspx/UpsertUserData',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'obj': $scope.UserData }
                }
                $http(states).success(function (response) {
                    debugger;

                    if (response.d == -111) {
                        showStickyToast(false, "Employee Code already existed ");
                        return false;
                    }
                    else if (response.d == -222) {
                        showStickyToast(false, "Email already existed ");
                        return false;
                    }
                    else if (response.d == 0) {
                        showStickyToast(false, "Error while creating ");
                        return false;
                    }
                    else if (response.d > 0) {
                        if (data.UserID != 0) {
                            showStickyToast(true, "Successfully saved ");
                        }
                        else {
                            showStickyToast(true, "Successfully created ");
                        }

                        $scope.GetUserList();
                        $scope.DisplayNewUserDetails = false;
                        $scope.UserList = true;

                    }
                    $scope.ssouserid = 0;

                });

            }
            $scope.getUserEditData = function (info) {
                debugger;

                $scope.selectedValues = null;
                $scope.selectedWareHouses = null;
                
                $scope.ssouserid = info.SSOUserID;
                var data = info;
                if (data.WarehouseIDs != '' && data.WarehouseIDs != undefined && data.WarehouseIDs != null) {
                    var states = {
                        method: 'POST',
                        url: '../TPL/NewAccount.aspx/setUserData',
                        headers: {
                            'Content-Type': 'application/json; charset=utf-8',
                            'dataType': 'json'
                        },
                        data: { 'obj': data.WarehouseIDs }
                    }
                    $http(states).success(function (response)
                    {
                        debugger;
                        $scope.selectedWareHouses = response.d;

                    });
                }
                var roles = {
                    method: 'POST',
                    url: '../TPL/NewAccount.aspx/GetRoleData',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { "usertypeid": data.UserTypeID }
                }
                $http(roles).success(function (response) {
                    $scope.Roles = response.d;
                    if (data.UserRoleIDs != '' && data.UserRoleIDs != undefined && data.UserRoleIDs != null) {
                        var states = {
                            method: 'POST',
                            url: '../TPL/NewAccount.aspx/setUserData',
                            headers: {
                                'Content-Type': 'application/json; charset=utf-8',
                                'dataType': 'json'
                            },
                            data: { 'obj': data.UserRoleIDs }
                        }
                        $http(states).success(function (response) {
                            debugger;
                            $scope.selectedValues = response.d;

                        });
                    }
                });
                //
                var isactive = false;
                if (data.IsActive == 1 || data.IsActive == 'YES') {
                    isactive = true;
                }
               
                if (data.UserTypeID == 2 || data.UserTypeID == 4) {
                    $scope.Disabled = true;
                    $scope.UserData.TenantID = 0;
                }//info.TenantID == null ? 0 : data.TenantID
                $scope.UserData = new User(data.UserTypeID, parseInt(accountid), data.TenantID, data.FirstName, data.LastName, data.MiddleName, data.EmployeeCode,
                    1, data.Email, data.AlternateEmail1, data.AlternateEmail2, data.Password, data.Mobile,
                    isactive, '', '', 0, data.UserID, data.SSOAccountID);
                $scope.DisplayNewUserDetails = true;
                $scope.UserList = false;

                if (data.UserID != 0) {
                    $('#btncreateUser').html('Save User');
                }


            }
            $scope.checktenats = function () {
               debugger;
                if ($scope.UserData.UserTypeID == undefined)
                    return false;
                if ($scope.UserData.UserTypeID == 2 || $scope.UserData.UserTypeID == 4) {
                    $scope.Disabled = true;
                    $scope.UserData.TenantID = 0;
                }
                else {          
                    $scope.Disabled = false;                
                    //$('#selectWH').find('span').remove();                 
                    $scope.UserData.TenantID = 0;
                }

                //if ($scope.UserData.UserTypeID == 3) {
                //    $('#selectedWareHouses').css("display", "none");
                //    //$('#btncreatenew').prop("disabled", true);
                //}

                var roles = {
                    method: 'POST',
                    url: '../TPL/NewAccount.aspx/GetRoleData',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { "usertypeid": $scope.UserData.UserTypeID }
                }
                $http(roles).success(function (response) {
                    $scope.Roles = response.d;
                });


            }
            $scope.searchUser = function () {
                ////debugger;
                if ($scope.tenant == undefined && $scope.tenant == null) {
                    $scope.tenant = 0;

                }
                var states = {
                    method: 'POST',
                    url: '../TPL/NewAccount.aspx/GetUserList',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: {
                        'Accountid': accountid, 'tenantid': 0,'UserId':$scope.tenant
                    }
                }
                $http(states).success(function (response) {
                    ////debugger;
                    $scope.UsersList = JSON.parse(response.d);


                });

            }
            $scope.GoToList = function(){
               
                //$window.open('../TPL/NewWarehouse.aspx?accountid=' + accountid, '_blank');
                location.href = "../TPL/NewWarehouse.aspx?accountid=" + accountid ;
            }
        });
        //$scope.clear = function () {
        //    debugger;
        //    $("#selecttenant").val("");
        //}


        function WareHouse(warehouseid, whname, whcode, whgroupcode, location, rackingtype, whtype, wharea, whaddress, floorspace, measurements, pin,
            geolocation, country, currency, Inout, pName, pMobile, pEmail, paddress, sname, smobile, semail, saddress,
            stateid, cityid, zipcode, latitude, langitude, length, width, height, Time, accountid) {
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

        }
        function User(usertypeid, accountid, tenantid, firstname, lastname, middlename, empcode, gender, email, altemail1, altemail2, password, mobile,
            isactive, roles, warehouses, printerid, userid, ssoaccountid) {
            this.UserID = userid;
            this.UserTypeID = usertypeid;
            this.AccountID = accountid;
            this.TenantID = tenantid;
            this.FirstName = firstname;
            this.LastNane = lastname;
            this.MiddleName = middlename;
            this.EMPCode = empcode;
            this.Gender = gender;
            this.Email = email;
            this.AltEmail1 = altemail1;
            this.AltEmail2 = altemail2;
            this.Password = password;
            this.Mobile = mobile;
            this.Isactive = isactive;
            this.Roles = roles;
            this.warehouses = warehouses;
            this.PrinterId = printerid;
            this.SSOAccountID = ssoaccountid;



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
    </script>

    <style>
        /*#MainContent_LinkButton3 {
    text-decoration: none;
    box-shadow: 0 3px 1px -2px rgba(0,0,0,.2), 0 2px 2px 0 rgba(0,0,0,.14), 0 1px 5px 0 rgba(0,0,0,.12);
    background: #73cadc;
    font-weight: normal;
    color: #fff;
    border-radius: 3px;
    margin-bottom: 9px;
    display: inline-block;
    background-image: linear-gradient(80deg, #00aeff, #73cadc);
}*/
        .flex__ div {
        }

        .flill input {
        }

        /*[type="file"] {
            font-size: 12px;
            border: 1px solid #999;
            padding: 5px;
            margin-bottom: 5px;
            width: 36% !important;
        }*/

        .row {
            margin: 0px !important;
        }

        select.GetPreferenceOptions {
            width: 23% !important;
            padding-right: 20px !important;

        }

        #divPreferenceHeader {
                font-size: 15px !important;
    border-radius: 0px !important;
    box-shadow: var(--z1);
    margin-bottom: 0px !important;
    border: 0px;
    font-weight: 400 !important;
    background-color: var(--sideNav-bg);
    color: #fff !important;
    padding: 9px 12px;
    /*border-top-left-radius: 5px !important;
    border-top-right-radius: 5px !important;*/
        }

        .ui-SubHeading {
            margin-top: 10px;
        }

        input[type="password"] {
            width: 90%;
        }
        .tool-tip {
  display: inline-block;
}

        .tool [data-tooltip]:hover:before,
        [data-tooltip]:hover:after
        {
            visibility:visible;
            opacity:1;
        }

      .tool span {
           position:absolute;
            right: 0px;
        }
        .tool span:first-child {
            display:none !important;
        }

        .tool [data-tooltip]:after {
            bottom:100%;
         }
        .tool [data-tooltip]:before {
            bottom:100%;
         }
    </style>
    <link href="tpl.css" rel="stylesheet" />

    <asp:UpdateProgress ID="uprgAccountList" runat="server" AssociatedUpdatePanelID="upnlPODetails">
        <ProgressTemplate>
            <div style="width: 100%; height: 100%; z-index: 999; position: fixed; top: 0; left: 0; right: 0; bottom: 0; align-items: center; display: flex; justify-content: center; background: #e0ddd8ba;">
                <%--<div class="spinner">
                <div class="bounce1"></div>
                <div class="bounce2"></div>
                <div class="bounce3"></div>
            </div>--%>
                <div style="align-self: center;">
                    <div class="spinner">
                        <div class="bounce1"></div>
                        <div class="bounce2"></div>
                        <div class="bounce3"></div>
                    </div>

                </div>

            </div>


        </ProgressTemplate>
    </asp:UpdateProgress>
    <div class="dashed"></div>

    <asp:HiddenField ID="hifssoaccid" runat="server" Value="0" />
    <div class="pagewidth">

    <asp:UpdatePanel ID="upnlPODetails" ChildrenAsTriggers="true" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="">
                <div>
                    <table border="0" cellpadding="0" cellspacing="0" align="center" class="" style="margin-bottom: 0px;">

                       
                        <tr>
                            <%-- <td colspan="2">
                        <br />
                        <asp:Literal ID="lblStatus" runat="server" />
                    </td>--%>
                            <tr class="btl">
                                <td align="right">
                                    <asp:LinkButton ID="LinkButton3" runat="server" CssClass="btn btn-sm btn-primary"  PostBackUrl="~/TPL/AccountList.aspx" style="float:right !important;"><i class="material-icons vl">arrow_back</i><%= GetGlobalResourceObject("Resource", "BacktoList")%></asp:LinkButton>

                                </td>
                            </tr>
                        </tr>
                        <tr class="">
                            <td colspan="">
                                
                                            <!-- Globalization Tag is added for multilingual  -->
                                <div class="angulardiv " ng-app="MyApp" ng-controller="NewUser">
                                    <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="dvPOHDHeader" style=""> <%= GetGlobalResourceObject("Resource", "AccountDetails")%> </div>
                                    <div class="ui-Customaccordion" id="dvPOHDBody">
                                        <table border="0" cellspacing="0" cellpadding="0" width="100%" style="padding: 10px;">
                                            <tr>
                                                <td colspan="3" align="center">
                                                    <asp:Panel runat="server" ID="pnlHeaderDetails">
                                                        <div>
                                                            <div class="row">
                                                                <div class="col m4 s4">
                                                                    <div class="flex">
                                                                        <div>


                                                                            <asp:TextBox runat="server"  ID="txtAccountName" required="" />
                                                                            <label>
                                                                                <asp:Literal runat="server" ID="ltAccount" Text= "<%$Resources:Resource,AccountName%>"  /></label>
                                                                            <span class="errorMsg"></span>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col m4 s4">
                                                                    <div class="flex">
                                                                        <div>
                                                                            <asp:TextBox runat="server" ID="txtzohoacccode" required="" readonly />
                                                                            <label>
                                                                                <asp:Literal runat="server" ID="ltZohoAcc" Text=  "<%$Resources:Resource,ZohoAccountCode%>" /></label>
                                                                            <span class="errorMsg">*</span>
                                                                            <%--<asp:TextBox runat="server" ID="txtCompanyLegalName" required=""/>--%>
                                                                        </div>
                                                                    </div>
                                                                </div>


                                                                <div class="col m4 s4">
                                                                    <div class="flex">
                                                                        <div>
                                                                             <asp:RequiredFieldValidator ID="rfvAccountCode" runat="server" ValidationGroup="save" ControlToValidate="txtAccountCode" Display="Dynamic" />
                                                               

                                                                            <asp:TextBox runat="server" ID="txtAccountCode" MaxLength="30" required="" />
                                                                            <label>
                                                                                <asp:Literal runat="server" ID="ltaccountCode" Text=  "<%$Resources:Resource,AccountCode%>" /></label>
                                                                            <span class="errorMsg">*</span>

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                </div>
                                                            <div class="row">
                                                                <div class="col m4 s4">
                                                                    <div class="flex">
                                                                        <div>
                                                                             <asp:RequiredFieldValidator ID="rfvcompany" runat="server" ValidationGroup="save" ControlToValidate="txtCompanyLegalName" Display="Dynamic" />
                                                               
                                                                            <asp:TextBox runat="server" ID="txtCompanyLegalName" required="" />
                                                                            <label>
                                                                                <asp:Literal runat="server" ID="ltLegal" Text= "<%$Resources:Resource,CompanyName%>"  /></label>
                                                                            <span class="errorMsg">*</span>
                                                                            <%--<asp:TextBox runat="server" ID="txtCompanyLegalName" required=""/>--%>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="col m4 s4">
                                                                    <div class="flex">
                                                                        <asp:Literal runat="server" ID="ltAccLogo" Text=  "<%$Resources:Resource,ChooseLogo%>" />

                                                                        <asp:FileUpload ID="AccountLogo" runat="server" EnableViewState="true" Text="Upload" style="width:100% !important;" />
                                                                        <span class="errorMsg">*</span>
                                                                    </div>
                                                                    <p style="font-size:11px !important;">Note: <span style="color:red;font-size:10px;">*</span><span style="color:slategray;font-size:9px !important;font-style:italic;font-weight:400;">Max File size: 2MB &emsp;&emsp;&emsp;<span style="color:red;font-size:10px;">*</span> FileTypes Allowed : .PNG, .JPG, .JPEG</span></p>
                                                                </div>
                                                               <div class="col m4 s4">
                                                                    <br />
                                                                    <asp:Image ID="UpdateImage" runat="server" AlternateText="No Logo Uploaded"/>
                                                                    <asp:Label ID="lblMessage" runat="server"></asp:Label></div>
                                                                <div align="right">

                                                                    <%--<asp:LinkButton ID="lnkUpdate" CssClass="btn btn-sm btn-primary" Text="Create" runat="server" OnClick="lnkUpdate_Click"></asp:LinkButton>--%>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>

                                        <div style="display: flex; justify-content: flex-end">
                                            <asp:LinkButton ID="lnkUpdate" CssClass="btn btn-sm btn-primary" Text="Create" runat="server" OnClick="lnkUpdate_Click"></asp:LinkButton>
                                        </div>
                                    </div>
                                    <!-- Subscription Details -->

                                    <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="dvScubScriptionHeader" style=""><%= GetGlobalResourceObject("Resource", "SubscriptionDetails")%></div>

                                            <div class="ui-Customaccordion" id="dvScuscriptionBody">
                                                <br />
                                                <table class="table-striped ">
                                                    <tr>
                                                        <th> <%= GetGlobalResourceObject("Resource", "Name")%></th>
                                                        <th> <%= GetGlobalResourceObject("Resource", "ValidFrom")%></th>
                                                        <th><%= GetGlobalResourceObject("Resource", "ValidTo")%></th>
                                                        <th> <%= GetGlobalResourceObject("Resource", "NoofConcurrentUsers")%></th>
                                                        <th> <%= GetGlobalResourceObject("Resource", "NoofWareHouses")%></th>
                                                        <th> <%= GetGlobalResourceObject("Resource", "ActivatedOn")%></th>
                                                        <th><%= GetGlobalResourceObject("Resource", "ExpiryDate")%></th>
                                                        <th> <%= GetGlobalResourceObject("Resource", "Status")%></th>
                                                        <th> <%= GetGlobalResourceObject("Resource", "Plan")%></th>
                                                        <th> <%= GetGlobalResourceObject("Resource", "Subscription")%></th>
                                                    </tr>


                                                    <tr ng-repeat="info in SubscriptionData">
                                                        <td>{{info.name}}</td>
                                                        <td align="center">{{info.SubscriptionValidFrom}}</td>
                                                        <td align="center">{{info.SubscriptionValidTo}}</td>
                                                        <td align="right">{{info.NumberOfConcurrentUsers}}</td>
                                                        <td align="right">{{info.NumberOfWarehouses}}</td>
                                                        <td align="center">{{info.ActivatedOn}}</td>
                                                        <td align="center">{{info.ExpiryDate}}</td>
                                                        <td>{{info.ZOHO_Status}}</td>
                                                        <td>{{info.ZOHO_Plan}}</td>
                                                        <td>{{info.ZOHO_Subscription}}</td>

                                                    </tr>



                                                </table>
                                                <br />
                                            </div>

                                    <!-- Subscription Details -->

                                <%--    <div class="angulardiv" ng-app="MyApp" ng-controller="NewUser">--%>
                                        <div ng-if="fromAccount">


                                            <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="dvTDHeader" style="">  <%= GetGlobalResourceObject("Resource", "WarehouseDetails")%> </div>

                                            <div class="ui-Customaccordion" id="dvTDBody">
                                                <%-- <div class="angulardiv" ng-app="MyApp" ng-controller="NewUser">--%>
                                                <table class="internalData" width="100%">
                                                    <tr>
                                                        <td>
                                                            <%--  <div class="angulardiv" ng-app="MyApp" ng-controller="NewUser">--%>
                                                            <div ng-cloak ng-show="DisplayNewWareHouse">
                                                                <div style="display: flex; justify-content: flex-end;">
                                                                    <button type="button" ng-click="displaylist()" class="addbuttonOutbound btn btn-primary"><i class="material-icons vl">arrow_back</i>Back To List</button>
                                                                </div>
                                                                <div class="ui-SubHeading ui-SubHeadingBar" id="PrimaryInformationHeader"> <%= GetGlobalResourceObject("Resource", "WarehouseCreation")%>  </div>

                                                                <div class="ui-Customaccordion" id="PrimaryInformationBody">
                                                                    <br />
                                                                    <div class="row">
                                                                        <div class="col m4 s4">
                                                                            <div class="flex">
                                                                                <div>
                                                                                    <%--<span class="requiredlabel" style="font-size:13pt"> <label>Tenant</label> </span>--%>
                                                                                </div>
                                                                                <div>
                                                                                    <select ng-model="warehousedata.AccountId" class="DropdownGH" ng-options="SC.ID as SC.Name for SC in AccountData" required="">
                                                                                        <%--  <option value=""></option>--%>
                                                                                    </select>
                                                                                    <span class="errorMsg"></span>
                                                                                    <label> <%= GetGlobalResourceObject("Resource", "SelectAccount")%>  </label>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col m4 s4">
                                                                            <div class="flex">
                                                                                <div>
                                                                                    <%--<span class="requiredlabel" style="font-size:13pt"> <label>Tenant</label> </span>--%>
                                                                                </div>
                                                                                <div>
                                                                                    <input type="text" id="Text17" ng-model="warehousedata.WHName" required="" />
                                                                                    <span class="errorMsg"></span>
                                                                                    <label> <%= GetGlobalResourceObject("Resource", "Warehouse")%> </label>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col m4 s4">
                                                                            <div class="flex">
                                                                                <div>
                                                                                    <%--<span class="requiredlabel" style="font-size:13pt"> Customer Name :</span>--%>
                                                                                </div>
                                                                                <div>
                                                                                    <input type="text" id="Text18" ng-model="warehousedata.WHCode" required="" maxlength="3" onkeypress="return blockSpecialChar(event)" />
                                                                                    <span class="errorMsg"></span>
                                                                                    <label>  <%= GetGlobalResourceObject("Resource", "WHCode")%></label>
                                                                                </div>
                                                                            </div>
                                                                        </div>



                                                                    </div>

                                                                    <div class="row">

                                                                        <%-- <div class="col m4 s4">
                                                        <div class="flex">
                                                            <div>
                                                                <span class="requiredlabel" style="font-size:13pt">Customer Code : </span><br /> 
                                                               
                                                            </div>
                                                            <div>
                                                                <select ng-model="warehousedata.WHGroupcode" class="DropdownGH" ng-options="SC.ID as SC.Name for SC in WareHouseCodes" required="">
                                                                    <option value=""></option>
                                                                </select>
                                                                 <label>Select WH Group Code.</label>
                                                            </div>
                                                        </div>
                                                    </div>--%>





                                                                        <div class="col m4 s4">
                                                                            <div class="flex">
                                                                                <div>
                                                                                    <%--<span class="requiredlabel" style="font-size:13pt"> Customer Name :</span>--%>
                                                                                </div>
                                                                                <div>
                                                                                    <select ng-model="warehousedata.RackingRType" class="DropdownGH" ng-options="SC.ID as SC.Name for SC in racktypes" required="">
                                                                                        <option value="" selected hidden />
                                                                                    </select>
                                                                                    <label> <%= GetGlobalResourceObject("Resource", "RackingType")%></label>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col m4 s4">
                                                                            <div class="flex">
                                                                                <div>
                                                                                    <%--<span class="requiredlabel" style="font-size:13pt">Customer Code : </span><br />--%>
                                                                                </div>
                                                                                <div>
                                                                                    <select ng-model="warehousedata.WHtype" class="DropdownGH" ng-options="SC.ID as SC.Name for SC in whtypes" required="">
                                                                                        <option value="" selected hidden />
                                                                                    </select>
                                                                                    <label>  <%= GetGlobalResourceObject("Resource", "WHType")%> </label>
                                                                                </div>
                                                                            </div>
                                                                        </div>


                                                                        <div class="col m4 s4">
                                                                            <div class="flex">
                                                                                <div>
                                                                                    <%--<span class="requiredlabel" style="font-size:13pt"> <label>Tenant</label> </span>--%>
                                                                                </div>
                                                                                <div>
                                                                                    <input type="text" ng-model="warehousedata.WHAddress" required="" />
                                                                                    <label>  <%= GetGlobalResourceObject("Resource", "WHAddress")%></label>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                    </div>

                                                                    <div class="row">


                                                                        <div class="col m4 s4">
                                                                            <div class="flex">
                                                                                <div>
                                                                                    <%--<span class="requiredlabel" style="font-size:13pt"> Customer Name :</span>--%>
                                                                                </div>
                                                                                <div>
                                                                                    <select ng-model="warehousedata.Inout" class="DropdownGH" ng-options="SC.ID as SC.Name for SC in inouts" required="">
                                                                                        <option value="" selected hidden />
                                                                                    </select>
                                                                                    <span class="errorMsg"></span>
                                                                                    <label> <%= GetGlobalResourceObject("Resource", "InOut")%></label>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col m4 s4">
                                                                            <div class="flex">
                                                                                <div>
                                                                                    <%--<span class="requiredlabel" style="font-size:13pt">Customer Code : </span><br />--%>
                                                                                </div>
                                                                                <div>
                                                                                    <input type="number" min="0" oninput="validity.valid||(value='');" id="Text21" ng-model="warehousedata.FloorSpace" required="" />
                                                                                    <label> <%= GetGlobalResourceObject("Resource", "FloorSpace")%> </label>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col m4 s4">
                                                                            <div class="flex">
                                                                                <div>
                                                                                    <%--<span class="requiredlabel" style="font-size:13pt"> <label>Tenant</label> </span>--%>
                                                                                </div>
                                                                                <div>
                                                                                    <input type="number" min="0" oninput="validity.valid||(value='');" id="Text22" ng-model="warehousedata.Length" required="" />
                                                                                    <label>  <%= GetGlobalResourceObject("Resource", "Length")%>  </label>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                    </div>

                                                                    <div class="row">


                                                                        <div class="col m4 s4">
                                                                            <div class="flex">
                                                                                <div>
                                                                                    <%--<span class="requiredlabel" style="font-size:13pt"> Customer Name :</span>--%>
                                                                                </div>
                                                                                <div>
                                                                                    <input type="number" min="0" oninput="validity.valid||(value='');" id="Text24" ng-model="warehousedata.Width" required="" />
                                                                                    <label> <%= GetGlobalResourceObject("Resource", "Width")%> </label>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col m4 s4">
                                                                            <div class="flex">
                                                                                <div>
                                                                                    <%--<span class="requiredlabel" style="font-size:13pt">Customer Code : </span><br />--%>
                                                                                </div>
                                                                                <div>
                                                                                    <input type="number" min="0" oninput="validity.valid||(value='');" id="Text23" ng-model="warehousedata.Height" required="" />
                                                                                    <label> <%= GetGlobalResourceObject("Resource", "Height")%> </label>
                                                                                </div>
                                                                            </div>
                                                                        </div>


                                                                        <div class="col m4 s4">
                                                                            <div class="flex">
                                                                                <div>
                                                                                    <%--<span class="requiredlabel" style="font-size:13pt"> <label>Tenant</label> </span>--%>
                                                                                </div>
                                                                                <div>
                                                                                    <select ng-model="warehousedata.Country" class="DropdownGH" ng-options="SC.ID as SC.Name for SC in countrynames" ng-change="getCurrency(0)" required="">
                                                                                        <option value="" selected hidden />
                                                                                    </select>
                                                                                    <span class="errorMsg"></span>
                                                                                    <label> <%= GetGlobalResourceObject("Resource", "Country")%>  </label>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                    </div>


                                                                    <div class="row">


                                                                        <div class="col m4 s4">
                                                                            <div class="flex">
                                                                                <div>
                                                                                    <%--<span class="requiredlabel" style="font-size:13pt"> Customer Name :</span>--%>
                                                                                </div>
                                                                                <div>
                                                                                    <select ng-model="warehousedata.StateId" class="DropdownGH" ng-options="SC.ID as SC.Name for SC in Statesdata" ng-change="getCities(0)" required="">
                                                                                        <option value="" selected hidden />
                                                                                    </select>
                                                                                    <span class="errorMsg"></span>
                                                                                    <label>  <%= GetGlobalResourceObject("Resource", "State")%> </label>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col m4 s4">
                                                                            <div class="flex">
                                                                                <div>
                                                                                    <%--<span class="requiredlabel" style="font-size:13pt">Customer Code : </span><br />--%>
                                                                                </div>
                                                                                <div>
                                                                                    <select ng-model="warehousedata.CityId" class="DropdownGH" ng-options="SC.ID as SC.Name for SC in CityData" ng-change="getZipCode(0)" required="">
                                                                                        <option value="" selected hidden />
                                                                                    </select>
                                                                                    <span class="errorMsg"></span>
                                                                                    <label> <%= GetGlobalResourceObject("Resource", "City")%> </label>
                                                                                </div>
                                                                            </div>
                                                                        </div>


                                                                        <div class="col m4 s4">
                                                                            <div class="flex">
                                                                                <div>
                                                                                    <%--<span class="requiredlabel" style="font-size:13pt"> Customer Name :</span>--%>
                                                                                </div>
                                                                                <div>
                                                                                    <select ng-model="warehousedata.ZipCodeId" class="DropdownGH" ng-options="SC.ID as SC.Name for SC in ZipCodeData" ng-change="getlatitudelongitude()" required="">
                                                                                        <option value="" selected hidden />
                                                                                    </select>
                                                                                    <span class="errorMsg"></span>
                                                                                    <label> <%= GetGlobalResourceObject("Resource", "ZipCode")%> </label>
                                                                                </div>

                                                                            </div>
                                                                        </div>

                                                                    </div>

                                                                    <div class="row">


                                                                        <div class="col m4 s4">
                                                                            <div class="flex">
                                                                                <div>
                                                                                    <%--<span class="requiredlabel" style="font-size:13pt"> Customer Name :</span>--%>
                                                                                </div>
                                                                                <div>
                                                                                    <input type="text" id="txtlatitude" disabled="true" ng-model="warehousedata.Langitude" readonly />
                                                                                    <label> <%= GetGlobalResourceObject("Resource", "Langitude")%></label>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="col m4 s4">
                                                                            <div class="flex">
                                                                                <div>
                                                                                    <%--<span class="requiredlabel" style="font-size:13pt"> <label>Tenant</label> </span>--%>
                                                                                </div>
                                                                                <div>
                                                                                    <input type="text" id="txtlangitude" disabled="true" ng-model="warehousedata.Latitude" readonly />
                                                                                    <label><%= GetGlobalResourceObject("Resource", "Latitude")%> </label>
                                                                                </div>
                                                                            </div>


                                                                        </div>
                                                                        <div class="col m4 s4">
                                                                            <div class="flex">
                                                                                <div>
                                                                                    <%--<span class="requiredlabel" style="font-size:13pt">Customer Code : </span><br />--%>
                                                                                </div>
                                                                                <div>
                                                                                    <select ng-model="warehousedata.Currency" class="DropdownGH" ng-options="SC.ID as SC.Name for SC in currencynames" required="">
                                                                                        <option value="" selected hidden />
                                                                                    </select>
                                                                                    <span class="errorMsg"></span>
                                                                                    <label>  <%= GetGlobalResourceObject("Resource", "Currency")%></label>
                                                                                </div>

                                                                            </div>

                                                                        </div>
                                                                    </div>
                                                                    <div class="row">

                                                                        <div class="col m4 s4">
                                                                            <div class="flex">
                                                                                <div>
                                                                                    <%--<span class="requiredlabel" style="font-size:13pt">Customer Code : </span><br />--%>
                                                                                </div>
                                                                                <div>
                                                                                    <select ng-model="warehousedata.Time" class="DropdownGH" ng-options="SC.ID as SC.Name for SC in timepreference" required="">
                                                                                        <option value="" selected hidden />
                                                                                    </select>
                                                                                    <label> <%= GetGlobalResourceObject("Resource", "TimeZone")%> </label>
                                                                                </div>

                                                                            </div>

                                                                        </div>
                                                                        <div class="col m4 s4">
                                                                            <div class="flex">
                                                                                <div>
                                                                                    <%--<span class="requiredlabel" style="font-size:13pt"> <label>Tenant</label> </span>--%>
                                                                                </div>
                                                                                <div>
                                                                                    <input type="text" id="Text19" ng-model="warehousedata.Location" required="">
                                                                                    <label> <%= GetGlobalResourceObject("Resource", "Location")%>  </label>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                    </div>


                                                                    <div class="ui-SubHeading ui-SubHeadingBar" id="PContactHeader"> <%= GetGlobalResourceObject("Resource", "PrimaryContactPerson")%>  </div>

                                                                    <div class="ui-Customaccordion" id="PContactBody">
                                                                        <div class="row">

                                                                            <div class="col-md-3">
                                                                                <div class="flex">
                                                                                    <div>
                                                                                        <%--<span class="requiredlabel" style="font-size:13pt"> <label>Tenant</label> </span>--%>
                                                                                    </div>
                                                                                    <div>
                                                                                        <input type="text" id="Text31" ng-model="warehousedata.pName" required="" />
                                                                                        <label> <%= GetGlobalResourceObject("Resource", "Name")%>  </label>
                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                            <div class="col-md-3">
                                                                                <div class="flex">
                                                                                    <div>
                                                                                        <%--<span class="requiredlabel" style="font-size:13pt"> Customer Name :</span>--%>
                                                                                    </div>
                                                                                    <div>
                                                                                        <%--<input  type="text"  min="0" oninput="validity.valid||(value='');" id="Text32" ng-model="warehousedata.Pmobile" required="" maxlength="10" allow-only-numbers  ng-minlength="10">--%>
                                                                                        <input type="text" onkeypress="return isNumber(event)" id="Text32" ng-model="warehousedata.Pmobile" required="" maxlength="10" />
                                                                                        <label> <%= GetGlobalResourceObject("Resource", "Mobile")%></label>
                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                            <div class="col-md-3">
                                                                                <div class="flex">
                                                                                    <div>
                                                                                        <%--<span class="requiredlabel" style="font-size:13pt">Customer Code : </span><br />--%>
                                                                                    </div>
                                                                                    <div>
                                                                                        <input type="text" id="Text33" ng-model="warehousedata.pEmail" required="">
                                                                                        <label> <%= GetGlobalResourceObject("Resource", "Email")%></label>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-3">
                                                                                <div class="flex">
                                                                                    <div>
                                                                                        <%--<span class="requiredlabel" style="font-size:13pt">Customer Code : </span><br />--%>
                                                                                    </div>
                                                                                    <div>
                                                                                        <input type="text" id="Text34" ng-model="warehousedata.PAddress" required="">
                                                                                        <label>  <%= GetGlobalResourceObject("Resource", "Address")%></label>
                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                        </div>

                                                                    </div>


                                                                    <div class="ui-SubHeading ui-SubHeadingBar" id="SContactHeader">  <%= GetGlobalResourceObject("Resource", "SecondaryContactPerson")%> </div>

                                                                    <div class="ui-Customaccordion" id="SContactBody">
                                                                        <div class="row">

                                                                            <div class="col-md-3">
                                                                                <div class="flex">
                                                                                    <div>
                                                                                        <%--<span class="requiredlabel" style="font-size:13pt"> <label>Tenant</label> </span>--%>
                                                                                    </div>
                                                                                    <div>
                                                                                        <input type="text" id="Text27" ng-model="warehousedata.sname" required="" />
                                                                                        <label>  <%= GetGlobalResourceObject("Resource", "Name")%> </label>
                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                            <div class="col-md-3">
                                                                                <div class="flex">
                                                                                    <div>
                                                                                        <%--<span class="requiredlabel" style="font-size:13pt"> Customer Name :</span>--%>
                                                                                    </div>
                                                                                    <div>
                                                                                        <%--<input  id="Text28" type="text"  min="0" oninput="validity.valid||(value='');" ng-model="warehousedata.SMobile" required="" maxlength="10" allow-only-numbers  ng-minlength="10">--%>
                                                                                        <input id="Text28" type="text" onkeypress="return isNumber(event)" ng-model="warehousedata.SMobile" required="" maxlength="10">
                                                                                        <label>  <%= GetGlobalResourceObject("Resource", "Mobile")%></label>
                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                            <div class="col-md-3">
                                                                                <div class="flex">
                                                                                    <div>
                                                                                        <%--<span class="requiredlabel" style="font-size:13pt">Customer Code : </span><br />--%>
                                                                                    </div>
                                                                                    <div>
                                                                                        <input type="text" id="Text29" ng-model="warehousedata.SEmail" required="">
                                                                                        <label>  <%= GetGlobalResourceObject("Resource", "Email")%></label>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="col-md-3">
                                                                                <div class="flex">
                                                                                    <div>
                                                                                        <%--<span class="requiredlabel" style="font-size:13pt">Customer Code : </span><br />--%>
                                                                                    </div>
                                                                                    <div>
                                                                                        <input type="text" id="Text30" ng-model="warehousedata.SAddress" required="">
                                                                                        <label>  <%= GetGlobalResourceObject("Resource", "Address")%></label>
                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                        </div>

                                                                    </div>

                                                                    <br />
                                                                    <div style="display: flex; justify-content: flex-end;">
                                                                        <button type="button" id="btncreatewh" ng-click="CreateNewWareHouse()" class="addbuttonOutbound btn btn-primary"> <%= GetGlobalResourceObject("Resource", "CreateWareHouse")%> <%=MRLWMSC21Common.CommonLogic.btnfaSave %> </button>
                                                                    </div>


                                                                </div>


                                                            </div>
                                                            <div ng-cloak ng-if="whlist">
                                                                <div style="float: right; margin-bottom: 5px;" class="tool">
                                                                    <%--<button type="button" id="btncreatenews" ng-click="GoToList()" class="btn btn-primary">Create <%=MRLWMSC21Common.CommonLogic.btnfaSave %></button>--%>
                                                                    <%--<button type="button" id="btncreatenew" ng-show="CreateWareHouseShow" ng-click="dispalynewwarehouse()" class="addbuttonOutbound btn btn-primary">Create New <%=MRLWMSC21Common.CommonLogic.btnfaSave %></button>--%>
                                                                  
                                                                        <button type="button" id="btncreatenew"  ng-click="GoToList()" class="addbuttonOutbound btn btn-primary"   Title=""> <%= GetGlobalResourceObject("Resource", "CreateNew")%> <%=MRLWMSC21Common.CommonLogic.btnfaSave %></button>
                                                                  
                                                                </div>

                                                                <table class="table-striped " width="100%">
                                                                    <thead>
                                                                        <tr>
                                                                            <th style="display: none">  <%= GetGlobalResourceObject("Resource", "WarehouseID")%></th>
                                                                            <th style="display: none"> <%= GetGlobalResourceObject("Resource", "Warehouse")%> </th>
                                                                            <th> <%= GetGlobalResourceObject("Resource", "Warehouse")%> </th>
                                                                            <%-- <th>WH Group Code</th>--%>
                                                                            <th>  <%= GetGlobalResourceObject("Resource", "WHType")%></th>
                                                                            <th>  <%= GetGlobalResourceObject("Resource", "Location")%> </th>
                                                                            <th> <%= GetGlobalResourceObject("Resource", "IsActive")%></th>
                                                                            <%--
                                                                            <th>Country</th>
                                                                            <th>State</th>
                                                                            <th>City</th>
                                                                            --%>
                                                                            <th>  <%= GetGlobalResourceObject("Resource", "Edit")%> </th>
                                                                            <th>  <%= GetGlobalResourceObject("Resource", "Delete")%></th>
                                                                        </tr>
                                                                    </thead>

                                                                    <tbody>
                                                                        <tr ng-repeat="info in WarehouseListdata">
                                                                            <th style="display: none">{{info.WarehouseID}}</th>
                                                                            <td style="display: none">{{info.WHName}}</td>
                                                                            <td><span Title="{{info.WHName}}">{{info.WHCode}}</span></td>
                                                                            <%--   <td>{{info.WarehouseGroupCode}}</td>--%>
                                                                            <td>{{info.WarehouseType}}</td>
                                                                            <td>{{info.Location}}</td>
                                                                            <td>{{info.IsActive==1?"Yes":"No"}}</td>
                                                                            <%--
                                                                             <td>{{info.country}}</td>
                                                                              <td>{{info.state}}</td>
                                                                            <td>{{info.city}}</td>
                                                                            --%>
                                                                            <td>
                                                                                <div ng-if="UserRoleId.indexOf('3')!=0 || UserRoleId.indexOf('1')!=0">
                                                                                    <%--   <div ng-if="UserRoleId.indexOf('5')==0 ">--%>

                                                                                    <div ng-if="role>=0">
                                                                                        <a ng-if="info.WarehouseID! = userWH" style="cursor: pointer; display: none !important;" ng-click="getWareHouseData(info)"><i class="material-icons ss">edit</i><em class="sugg-tooltis">Edit</em></a>
                                                                                        <a ng-if="info.WarehouseID == userWH" style="cursor: pointer;" ng-click="getWareHouseData(info)"><i class="material-icons ss">edit</i><em class="sugg-tooltis">Edit</em></a>
                                                                                    </div>
                                                                                </div>
                                                                                <div ng-if="UserRoleId.indexOf('3')==0 || UserRoleId.indexOf('1')==0">
                                                                                    <span style="cursor: pointer;" ng-click="getWareHouseData(info)"><i class="material-icons ss">edit</i><em class="sugg-tooltis">Edit</em></span>
                                                                                    <%-- <span ng-if="info.WarehouseID == userWH" style="cursor: pointer;" ng-click="getWareHouseData(info)"><i class="material-icons ss">edit</i></span>
                                                                                    --%>
                                                                                </div>
                                                                            </td>

                                                                            <td ng-show="whdelete">
                                                                                <div ng-if="UserRoleId.indexOf('3')!=0 || UserRoleId.indexOf('1')!=0">
                                                                                    <%-- <div ng-if="UserRoleId.indexOf('5')==0 ">--%>
                                                                                    <div ng-if="role>=0">
                                                                                        <a ng-if="info.WarehouseID! = userWH" style="cursor: pointer; display: none !important;" ng-click="deleteWareHouseData(info.WarehouseID)"><i class="material-icons ss">delete</i><em class="sugg-tooltis">Delete</em></a>
                                                                                        <a ng-if="info.WarehouseID == userWH" ng-click="deleteWareHouseData(info.WarehouseID)" style="cursor: pointer;"><i class="material-icons ss">delete</i><em class="sugg-tooltis">Delete</em></a>
                                                                                    </div>
                                                                                </div>
                                                                                <div ng-if="UserRoleId.indexOf('3')==0 || UserRoleId.indexOf('1')==0">
                                                                                    <span ng-click="deleteWareHouseData(info.WarehouseID)" style="cursor: pointer;"><i class="material-icons ss">delete</i><em class="sugg-tooltis">Delete</em></span>

                                                                                </div>

                                                                            </td>
                                                                        </tr>
                                                                </tbody>


                                                                </table>
                                                            </div>







                                                        </td>
                                                    </tr>



                                                </table>
                                            </div>



                                            <%-- Body end --%>
                                            <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="dvuserHeader" style=""> <%= GetGlobalResourceObject("Resource", "UserDetails")%></div>

                                            <div class="ui-Customaccordion" id="dvuserBody">

                                                <%-- <iframe id="userFrame" runat="server" style="width: 100%; height: 600px;">alternative content for browsers which do not support iframe.
                            </iframe>--%>
                                                <div ng-cloak ng-show="DisplayNewUserDetails">

                                                    <div class="ui-SubHeading ui-SubHeadingBar" id="UserCreationHeader"><%= GetGlobalResourceObject("Resource", "Usercreation")%>  </div>

                                                    <div class="ui-Customaccordion" id="UserCreationBody">
                                                        <br />
                                                        <div class="row">
                                                            <div class="col m4 s4">
                                                                <div class="flex">
                                                                    <div>
                                                                        <div>
                                                                            <select ng-model="UserData.UserTypeID" ng-disabled="UserDisabled"  class="DropdownGH" ng-options="SC.ID as SC.Name for SC in usertypes" ng-change="checktenats()" required="">
                                                                                <option value="" selected hidden />
                                                                            </select>
                                                                            <span class="errorMsg"></span>
                                                                            <label><%= GetGlobalResourceObject("Resource", "UserType")%></label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col m4 s4">
                                                                <div class="flex">
                                                                    <div>
                                                                        <div>
                                                                            <select ng-model="UserData.AccountID" class="DropdownGH" ng-options="SC.ID as SC.Name for SC in AccountData" required="">
                                                                                <option value="" selected hidden />
                                                                            </select>
                                                                            <label><%= GetGlobalResourceObject("Resource", "Account")%></label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="col m4 s4">
                                                                <div class="flex">

                                                                    <div>
                                                                        <select ng-model="UserData.TenantID" ng-disabled="Disabled" class="DropdownGH" ng-options="SC.ID as SC.Name for SC in TenantData" required="">
                                                                            <option value=""> <%= GetGlobalResourceObject("Resource", "Tenant")%></option>
                                                                        </select>
                                                                         <span class="spTenant  errorMsg "  style="display:none"></span>
                                                                    </div>
                                                                </div>

                                                            </div>



                                                        </div>

                                                        <div class="row">

                                                            <div class="col m4 s4">
                                                                <div class="flex">

                                                                    <div>
                                                                        <input type="text" ng-model="UserData.FirstName" required="" />
                                                                        <span class="errorMsg"></span>
                                                                        <label> <%= GetGlobalResourceObject("Resource", "FirstName")%></label>
                                                                    </div>
                                                                </div>
                                                            </div>





                                                            <div class="col m4 s4">
                                                                <div class="flex">

                                                                    <div>
                                                                        <input type="text" ng-model="UserData.MiddleName" required="" />
                                                                        <label> <%= GetGlobalResourceObject("Resource", "MiddleName")%></label>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="col m4 s4">
                                                                <div class="flex">

                                                                    <div>
                                                                        <input type="text" ng-model="UserData.LastNane" required="" />
                                                                        <label> <%= GetGlobalResourceObject("Resource", "LastName")%></label>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                        </div>

                                                        <div class="row">

                                                            <div class="col m4 s4">
                                                                <div class="flex">

                                                                    <div>
                                                                        <input type="text" ng-model="UserData.EMPCode" required="" />
                                                                        <span class="errorMsg"></span>
                                                                        <label> <%= GetGlobalResourceObject("Resource", "EmployeeCode")%></label>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="col m4 s4">
                                                                <div class="flex">

                                                                    <div>
                                                                        <div>
                                                                            <select ng-model="UserData.Gender" class="DropdownGH" ng-options="SC.ID as SC.Name for SC in Genderdata" required="">
                                                                                <option value="" selected hidden />

                                                                            </select>
                                                                            <label> <%= GetGlobalResourceObject("Resource", "Gender")%></label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="col m4 s4">
                                                                <div class="flex">

                                                                    <div>
                                                                        <input type="text" ng-model="UserData.Email" required="" />
                                                                        <span class="errorMsg"></span>
                                                                        <label>  <%= GetGlobalResourceObject("Resource", "Email")%> </label>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                        </div>

                                                        <div class="row">

                                                            <div class="col m4 s4">
                                                                <div class="flex">

                                                                    <div>
                                                                        <input type="text" ng-model="UserData.AltEmail1" required="" />
                                                                        <label>  <%= GetGlobalResourceObject("Resource", "AltEmail")%> </label>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="col m4 s4">
                                                                <div class="flex">

                                                                    <div>
                                                                        <input type="text" ng-model="UserData.AltEmail2" required="" />
                                                                        <label>  <%= GetGlobalResourceObject("Resource", "AltEmails")%> </label>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="col m4 s4">
                                                                <div class="flex">

                                                                    <div>
                                                                        <input type="password" ng-model="UserData.Password" required="" />
                                                                        <span class="errorMsg"></span>
                                                                        <label>  <%= GetGlobalResourceObject("Resource", "Password")%>  </label>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                        </div>


                                                        <div class="row">

                                                            <div class="col m4 s4">
                                                                <div class="flex">

                                                                    <div>
                                                                        <input type="text" ng-model="UserData.Mobile" required="" maxlength="10" onkeypress="filterDigits(event)"  />
                                                                         
                                                                        <label> <%= GetGlobalResourceObject("Resource", "Mobile")%> </label>
                                                                    </div>
                                                                </div>
                                                            </div>


                                                            <div style="display:none" class="col m4 s4">
                                                                <div class="flex">

                                                                    <div>
                                                                        <select ng-model="UserData.PrinterID" class="DropdownGH" ng-options="SC.ID as SC.Name for SC in PrinterData" required="">
                                                                            <option value="" selected hidden />
                                                                        </select>
                                                                        <label> <%= GetGlobalResourceObject("Resource", "Printer")%> </label>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col m4 s4">
                                                                <div class="">

                                                                    <div class="flex__">
                                                                        <input type="checkbox" id="aactive" ng-model="UserData.Isactive" />
                                                                        <label for="aactive">&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp; <%= GetGlobalResourceObject("Resource", "Active")%>  </label>
                                                                    </div>
                                                                </div>
                                                            </div>



                                                        </div>

                                                        <div class="row">

                                                            <div class="col m4 s4">
                                                                <div class="flex">

                                                                    <div>

                                                                        <select multiple ng-model="$parent.selectedValues" style="width: 60%;" size="7">
                                                                           <%-- default selected based on usertype selection commented  by Meena --%>
                                                                        <%--    <option ng-repeat="category in Roles" ng-if="Roles.length > 0" value="{{category.ID}}" ng-selected="{{selectedValues.indexOf(category.ID.toString())!=-1}}">{{category.Name}}</option>
                                                                     --%> 
                                                                                <option ng-repeat="category in Roles" ng-if="Roles.length > 0" value="{{category.ID}}" >{{category.Name}}</option>
                                                                     
                                                                        </select>
                                                                        <label> <%= GetGlobalResourceObject("Resource", "Roles")%></label>
                                                                        <span class="errorMsg">*</span>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col m4" id="selectWH">
                                                                <div class="flex">

                                                                    <div>

                                                                        <select multiple   ng-model="$parent.selectedWareHouses" style="width: 60%;" size="7">
<%--                                                                            <option ng-repeat="category in warehouses" ng-if="warehouses.length > 0" value="{{category.ID}}" ng-selected="{{selectedValues.indexOf(category.ID.toString())!=-1}}">{{category.Name}}</option>--%>
                                                                            <option ng-repeat="category in warehouses" ng-if="warehouses.length > 0" value="{{category.ID}}" ng-selected="{{selectedValues}}">{{category.Name}}</option>
                                                                            <option ng-if="warehouses.length == 0" selected disabled><%= GetGlobalResourceObject("Resource", "PleaseCreateWarehousetoConfigure")%> </option>
                                                                        </select>
                                                                        <label> <%= GetGlobalResourceObject("Resource", "Warehousess")%></label>
                                                                        <span class="errorMsg">*</span>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>


                                                        <flex end>
                                                                <button type="button" ng-click="displayuserlist()" class="addbuttonOutbound btn btn-primary"><i class="material-icons vl">arrow_back</i>  <%= GetGlobalResourceObject("Resource", "BackToList")%></button>
                                                                <button type="button" id="btncreateUser" Title="" ng-click="CreateNewUser()" class="addbuttonOutbound btn btn-primary"> <%= GetGlobalResourceObject("Resource", "CreateUser")%> <%=MRLWMSC21Common.CommonLogic.btnfaSave %></button>

                                                          
                                                        </flex>

                                                    </div>


                                                </div>

                                                <div ng-cloak ng-if="UserList">
                                                    <br />
                                                    <div class="flex__ right">
                                                        <div class="flex">
                                                        <%--    <select ng-model="$parent.$parent.tenant" id="selecttenant" style="width: 200px !important;" ng-options="SC.ID as SC.Name for SC in TenantData" required="">
                                                                <option value="">Select Tenant</option>
                                                            </select>--%>
                                                           <%-- </select>--%>
                                                               <select ng-model="$parent.$parent.tenant" id="selecttenant" style="width: 200px !important;" ng-options="SC.ID as SC.Name for SC in UsersData" required="">
                                                                <option value=""> <%= GetGlobalResourceObject("Resource", "UsersFor")%></option>
                                                            </select>

                                                        </div>
                                                        &nbsp;&nbsp;
                                                    <%--<button type="button" class="btn btn-primary" ng-click="clear()">Clear <i class="fa fa-ban" aria-hidden="true"></i></button>&nbsp;&nbsp;--%>
                                                        <button type="button" ng-click="searchUser()" class="addbuttonOutbound btn btn-primary">  <%= GetGlobalResourceObject("Resource", "Search")%> <i class="material-icons">search</i></button>
                                                        &nbsp;&nbsp;
                                                        <div class="tool" style="float: right; margin-bottom: 5px;">
                                                        <button type="button" id="btnCreateUser" ng-click="dispalynewwareUser()" Title="" class="addbuttonOutbound btn btn-primary">  <%= GetGlobalResourceObject("Resource", "CreateNew")%> <%=MRLWMSC21Common.CommonLogic.btnfaSave %></button>
                                                        </div>
                                                    </div>

                                                    <table class="table-striped ">
                                                        <tr>
                                                         <%--   <th> <%= GetGlobalResourceObject("Resource", "UserID")%> </th>--%>
                                                            <th> <%= GetGlobalResourceObject("Resource", "UserType")%> </th>
                                                            <th> <%= GetGlobalResourceObject("Resource", "Account")%></th>

                                                            <th> <%= GetGlobalResourceObject("Resource", "Tenant")%></th>
                                                            <th> <%= GetGlobalResourceObject("Resource", "FullName")%></th>
                                                            <th> <%= GetGlobalResourceObject("Resource", "EmpCode")%></th>
                                                              <th> <%= GetGlobalResourceObject("Resource", "USerRoles")%></th>
                                                            <th> <%= GetGlobalResourceObject("Resource", "WH")%></th>
                                                       <%--     <th></th>--%>
                                                            <%-- <th>Work Stations</th>--%>
                                                            <th>  <%= GetGlobalResourceObject("Resource", "Active")%></th>
                                                            <th>  <%= GetGlobalResourceObject("Resource", "Edit")%> </th>
                                                        </tr>


                                                        <tr dir-paginate="info in UsersList |itemsPerPage:5" pagination-id="nonAvaible" ng-show="UsersList.length > 0">                                                            
                                                            <%--<td>{{info.UserID}}</td>--%>
                                                            <td>{{info.UserType}}</td>
                                                            <td><span Title="{{info.Account}}">{{info.AccountCode}}</span></td>
                                                            <td>{{info.TenantName}}</td>
                                                            <td>{{info.FullName}}</td>
                                                            <td>{{info.EmployeeCode}}</td>
                                                            <td>{{info.UserRoles}}</td>
                                                            <td><span Title="{{info.WHName}}">{{info.WHCode}}</span></td>
                                                            <%-- <td>{{info.WorkCenters}}</td>--%>
                                                            <td>{{info.IsActive}}</td>

                                                            <td>
                                                                <div ng-if="UserRoleId.indexOf('3')!=0 || UserRoleId.indexOf('1')!=0  ">
                                                                    
                                                                   <%--  <div ng-if="UserRoleId.indexOf('5')==0 ">--%>
                                                                      <div ng-if="role>=0">
                                                                <div ng-if="info.UserID != UserId1" style="cursor: pointer;display:none !important;" ng-click="getUserEditData(info)"><i class="material-icons ss">edit {{UserId1}}</i><em class="sugg-tooltis">Edit</em></div>


                                                                 <div ng-if="info.UserID == UserId1" style="cursor: pointer;" ng-click="getUserEditData(info)"><i class="material-icons ss">edit{{UserId1}}</i><em class="sugg-tooltis">Edit</em></div>

</div>

                                                                </div>
                                                                <div ng-if="testedit==false">
                                                                 <div ng-if="UserRoleId.indexOf('3')==0 ||  UserRoleId.indexOf('1')==0 ">
                                                                 <div  style="cursor: pointer;" ng-click="getUserEditData(info)"><i class="material-icons ss">edit{{UserId1}}</i><em class="sugg-tooltis">Edit</em></div>
</div>
                                                                </div>

                                                                    </td>
                                                        </tr>
                                                      <%--  <tr ng-if="UsersList.length == 0">
                                                            <td colspan="10" style="text-align:center !important;">No Data Found</td>
                                                        </tr>--%>



                                                    </table>
                                                    <div style="float: right !important; font-family: Arial; font-size: small; margin-right: 1%;">
                                                        <dir-pagination-controls class="getPageId" direction-links="true" pagination-id="nonAvaible" boundary-links="true"> </dir-pagination-controls>
                                                    </div>
                                                </div>

                                            </div>

                                            

                                        </div>

                                    </div>

                                </div>
                            </td>
                        </tr>
                    </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lnkUpdate" />
        </Triggers>
    </asp:UpdatePanel></div>
    
    <div class="">
        <input type="hidden" value="0" id="GEN_TRN_Preference_ID" />
        <div id="divPreferences" style="display: none;"></div>
    </div>

    <script type="text/javascript">
        CustomAccordino($('#dvMDHeader'), $('#dvMDBody'));
        CustomAccordino($('#dvTDHeader'), $('#dvTDBody'));
        CustomAccordino($('#dvuserHeader'), $('#dvuserBody'));
        CustomAccordino($('#PrimaryInformationHeader'), $('#PrimaryInformationBody'));
        CustomAccordino($('#PContactHeader'), $('#PContactBody'));
        CustomAccordino($('#PContactHeader'), $('#SContactBody'));
        CustomAccordino($('#dvScubScriptionHeader'), $('#dvScuscriptionBody'));
        CustomAccordino($('#UserCreationHeader'), $('#UserCreationBody'));
        var ItemList = null;
        MasterID = new URL(window.location.href).searchParams.get("accountid");
        // alert(MasterID);
        if (MasterID != null) {
           // $("#divPreferences").css("display", "block");
        }
        function GetPreferencesList() {

            $.ajax({
                url: '<%=ResolveUrl("NewAccount.aspx/GetPreferences") %>',
                //data: "{ 'prefix': '" + request.term + "'}",
                data: "{}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (response) {

                    var dt = JSON.parse(response.d);
                    ItemList = dt;
                    GetPrefernces();
                    BindPreferences(MasterID);
                },
                error: function (response) {

                },
                failure: function (response) {

                }
            });
        }

        GetPreferencesList();
        var OptionListData = null;
        function GetPrefernces() {
            debugger;
            var pref = "Preferences";
            var displayid = "";
            displaypreferenceid = "";
            var PrerenceList = null;
            var PreferenceContainer = document.getElementById('divPreferences');
            var PreferenceContent = '';
            if (ItemList.Table != null && ItemList.Table.length > 0) {

                var GroupList = $.grep(ItemList.Table, function (a) { return a.GroupName == "Account" });

                //PreferenceContent += '<div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="divPreferenceHeader">' + GroupList[0].GroupName + '</div><div class="ui-Customaccordion" id="divPreferenceBody" style="text-align:left;">';
                PreferenceContent += '<div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="divPreferenceHeader">' + pref + '</div><div class="ui-Customaccordion" id="divPreferenceBody" style="text-align:left;">';

                if (GroupList != null && GroupList.length > 0) {
                    PrerenceList = $.grep(ItemList.Table1, function (a) { return a.GEN_MST_PreferenceGroup_ID == GroupList[0].GEN_MST_PreferenceGroup_ID });
                    PreferenceContent += '<div style="padding:10px;">';
                    // PreferenceContent += '<div style="padding:10px;width:96.2%;display:inline-block;text-align:left;vertical-align: top; -webkit-column-count: 4;-moz-column-count: 4;column-count: 4;-webkit-column-gap: 10px;-moz-column-gap: 10px;column-gap: 10px;">';
                    for (var i = 0; i < PrerenceList.length; i++) {
                        //PreferenceContent += '<div style="width:100%;padding:5px;">' + PrerenceList[i].PreferenceName + ' : <br/>';
                        //  PreferenceContent += '<div style="width:100%;padding:5px;">';
                        //if (PrerenceList[i].PreferenceName == "Is Strict Compliance for Putaway") {
                        //    var x = sss;
                        //}
                        PreferenceContent += '<div style=";"><h5>' + PrerenceList[i].PreferenceName + ' :</h5><div style="display: flex;">';

                        var OptionList = $.grep(ItemList.Table2, function (a) { return a.GEN_MST_Preference_ID == PrerenceList[i].GEN_MST_Preference_ID });

                        if (PrerenceList[i].UIControlType == "DropdownList") {
                            // OptionListData = OptionList[i];
                            //PreferenceContent += '<select class="GetPreferenceOptions" id="' + OptionList[j].GEN_MST_PreferenceOption_ID + '" name="' + PrerenceList[i].GEN_MST_Preference_ID + '" value="' + GroupList[0].GEN_MST_PreferenceGroup_ID + '"> +'<br>';
                            PreferenceContent += '<select class=" GetPreferenceOptions"  id="Pref' + PrerenceList[i].GEN_MST_Preference_ID + '" data-attr="0" name="' + PrerenceList[i].GEN_MST_Preference_ID + '" data-grpID="' + GroupList[0].GEN_MST_PreferenceGroup_ID + '"></select>';
                            //<select class="GetPreferenceOptions" id="' + PrerenceList[i].GEN_MST_Preference_ID + '" data-CNFMSP="' + item[i].MM_CNF_MSP_ID +'"></select>

                        }
                        else {

                            for (var j = 0; j < OptionList.length; j++) {
                                if (PrerenceList[i].UIControlType == "TextBox") {
                                    //PreferenceContent += '<div><b>' + OptionList[j].OptionCode + '</b></div>'

                                    PreferenceContent += OptionList[j].OptionLabel + ' :&emsp; <input type="text" class="GetPreferenceOptions" id="Pref' + OptionList[j].GEN_MST_PreferenceOption_ID + '" name="' + PrerenceList[i].GEN_MST_Preference_ID + '" data-grpID="' + GroupList[0].GEN_MST_PreferenceGroup_ID + '" data-attr="0" value="">';
                                }

                                else if (PrerenceList[i].UIControlType == "RadioButton") {
                                    //PreferenceContent += '<div><b>' + OptionList[j].OptionCode + '</b></div>';
                                    PreferenceContent += '<input type="radio" class="GetPreferenceOptions" id="Pref' + OptionList[j].GEN_MST_PreferenceOption_ID + '" name="' + PrerenceList[i].GEN_MST_Preference_ID + '" data-grpID="' + GroupList[0].GEN_MST_PreferenceGroup_ID + '" data-attr="0" value="' + GroupList[0].GEN_MST_PreferenceGroup_ID + '"> ' + OptionList[j].OptionLabel + '<br>';
                                }

                                else if (PrerenceList[i].UIControlType == "CheckBox") {
                                    PreferenceContent += '<div class="checkbox"><input type="checkbox" class="GetPreferenceOptions" id="Pref' + OptionList[j].GEN_MST_PreferenceOption_ID + '" name="' + PrerenceList[i].GEN_MST_Preference_ID + '" data-grpID="' + GroupList[0].GEN_MST_PreferenceGroup_ID + '" data-attr="0"  value="' + GroupList[0].GEN_MST_PreferenceGroup_ID + '"> <label for="Pref' + OptionList[j].GEN_MST_PreferenceOption_ID + '">' + OptionList[j].OptionLabel + '</label></div>';
                                }


                            }
                        }

                        PreferenceContent += '</div></div>&emsp;';

                        if (i % 1 == 0) {
                            PreferenceContent += '<p></p>';
                        }
                        //PreferenceContent += '</div>';
                    }
                    PreferenceContent += '</div>';
                }


                PreferenceContent += '<div style="text-align:right;padding:5px 23px; overflow:hidden; padding-right: 0;"><button type="button" class="btn btn-sm btn-primary" onclick="UpsertPreferences()">Update Preferences <i class="space fa fa-database"></i></button></div>';

                PreferenceContainer.innerHTML = PreferenceContent;

                for (var i = 0; i < PrerenceList.length; i++) {

                    var OptionList = $.grep(ItemList.Table2, function (a) { return a.GEN_MST_Preference_ID == PrerenceList[i].GEN_MST_Preference_ID });
                    for (var x = 0; x < OptionList.length; x++) {
                        if (PrerenceList[i].UIControlType == "DropdownList") {
                            BindInvDropdowns(OptionList, PrerenceList[i].GEN_MST_Preference_ID);
                        }
                        else {

                        }
                    }
                }

                CustomAccordino($('#divPreferenceHeader'), $('#divPreferenceBody'));
            }

            else {
                $(".PrefereModule").css("display", "none");
            }

            //for (var a = 0; a < displayid.split(',').length ; a++) {
            //    var divid = displayid.split(',')[a];
            //    $("#PanelBlockId" + divid).css("display", "none");
            //}

            //for (var a = 0; a < displaypreferenceid.split(',').length ; a++) {
            //    var divid = displaypreferenceid.split(',')[a];
            //    $(".Preference" + divid).css("display", "none");
            //}

            //DefaultPreferenceData(ItemList[44]);
        }

        function BindInvDropdowns(dt, attributeid)
        {
            //KeyText, KeyValue
            if (dt != null && dt != '') {
                for (var x = 0; x < dt.length; x++)
                {
                    if (x == 0) {
                        $('#Pref' + dt[x].GEN_MST_Preference_ID).empty();
                        $("#Pref" + dt[x].GEN_MST_Preference_ID).append($("<option></option>").val(0).html("Please Select"));
                    }
                    $("#Pref" + dt[x].GEN_MST_Preference_ID).append($("<option></option>").val(dt[x].GEN_MST_PreferenceOption_ID).html(dt[x].OptionCode));
                }
            }
            else {
                $('#Pref' + attributeid).empty();
                $("#Pref" + attributeid).append($("<option></option>").val(0).html("Please Select"));
            }
        }

        function BindPreferences(AccountID) {

            var PreferenceData = $.grep(ItemList.Table5, function (a) { return a.AccountID == AccountID });
            FillPreferenceData(PreferenceData);
        }

        function FillPreferenceData(obj) {
            if (obj != null && obj.length > 0) {
                for (var i = 0; i < obj.length; i++) {
                    if (obj[i].UIControlType == "TextBox") {
                        $('#Pref' + obj[i].GEN_MST_PreferenceOption_ID).val(obj[i].Value);
                        $('#Pref' + obj[i].GEN_MST_PreferenceOption_ID).attr("data-attr", obj[i].GEN_TRN_Preference_ID);
                    }
                    if (obj[i].UIControlType == "DropdownList") {
                        $('#Pref' + obj[i].GEN_MST_Preference_ID).val(obj[i].GEN_MST_PreferenceOption_ID);
                        $('#Pref' + obj[i].GEN_MST_Preference_ID).attr("data-attr", obj[i].GEN_TRN_Preference_ID);
                    }
                    else if (obj[i].UIControlType == "CheckBox") {
                        //$('#' + obj[i].GEN_MST_PreferenceOption_ID).attr("checked", "checked");
                        $('#Pref' + obj[i].GEN_MST_PreferenceOption_ID).prop("checked", true);
                        $('#Pref' + obj[i].GEN_MST_PreferenceOption_ID).attr("data-attr", obj[i].GEN_TRN_Preference_ID);
                    }
                    else {
                        $('#Pref' + obj[i].GEN_MST_PreferenceOption_ID).prop("checked", true);
                        $('#Pref' + obj[i].GEN_MST_PreferenceOption_ID).attr("data-attr", obj[i].GEN_TRN_Preference_ID);
                    }

                }
            }
        }

        function GetPrefernceFromData() {
            ////debugger;
            // var fieldDataOut = '{';
            MasterID = new URL(window.location.href).searchParams.get("accountid");
            var fieldData = '<root>';
            $(".GetPreferenceOptions").each(function () {
                var param = $(this).attr('id').replace("Pref", "");
                var val = $(this).val().trim();
                //data-pId
                var paramtype = $(this).attr('type');
                var optionvalue = $(this).find("option:selected").text();
                if (paramtype == "radio" || paramtype == "checkbox") {
                    val = $(this).prop('checked');
                    if (val == true) {
                        var GroupID = $(this).val().trim();
                        var PreferenceID = $(this)[0].name;
                        var OptionID = $(this)[0].id.replace("Pref", "");
                        var keyid = $(this).attr("data-attr");
                        var OrgEntityID = 5;
                        fieldData += '<data>';
                        fieldData += '<Value>1</Value>';
                        fieldData += '<GEN_MST_OrgEntity_ID>' + OrgEntityID + '</GEN_MST_OrgEntity_ID>';
                        fieldData += '<EntityID>' + MasterID + '</EntityID>';
                        fieldData += '<GEN_MST_PreferenceGroup_ID>' + GroupID + '</GEN_MST_PreferenceGroup_ID>';
                        fieldData += '<GEN_MST_Preference_ID>' + PreferenceID + '</GEN_MST_Preference_ID>';
                        fieldData += '<GEN_TRN_Preference_ID>' + keyid + '</GEN_TRN_Preference_ID>';
                        fieldData += '<GEN_MST_PreferenceOption_ID>' + OptionID + '</GEN_MST_PreferenceOption_ID> </data>';
                    }

                    else {
                        var GroupID = $(this).val().trim();
                        var PreferenceID = $(this)[0].name;
                        var OptionID = $(this)[0].id.replace("Pref", "");
                        var keyid = $(this).attr("data-attr");
                        var OrgEntityID = 5;
                        fieldData += '<data>';
                        fieldData += '<Value></Value>';
                        fieldData += '<GEN_MST_OrgEntity_ID>' + OrgEntityID + '</GEN_MST_OrgEntity_ID>';
                        fieldData += '<EntityID>' + MasterID + '</EntityID>';
                        fieldData += '<GEN_MST_PreferenceGroup_ID>' + GroupID + '</GEN_MST_PreferenceGroup_ID>';
                        fieldData += '<GEN_MST_Preference_ID>' + PreferenceID + '</GEN_MST_Preference_ID>';
                        fieldData += '<GEN_TRN_Preference_ID>' + keyid + '</GEN_TRN_Preference_ID>';
                        fieldData += '<GEN_MST_PreferenceOption_ID>' + OptionID + '</GEN_MST_PreferenceOption_ID> </data>';
                    }
                }
                else {

                    val = $(this).val();
                    if (val == null || val == "") {

                    }

                    else {

                        // var Value = $(this).val().trim();
                        //  var GroupID = $(this).attr("data-attr");
                        if (paramtype == undefined) {
                            optionvalue = optionvalue;
                            val = $(this).val();
                        }
                        else {
                            optionvalue = $(this).val().trim();
                            val = $(this)[0].id.replace("Pref", "");
                            //val = $(this).attr('id').replace("Pref", "");
                        }
                        var GroupID = $(this).attr("data-grpID");
                        //var PreferenceID = $(this).attr("data-attr");
                        var keyid = $(this).attr("data-attr");
                        var PreferenceID = $(this)[0].name;
                        var OrgEntityID = 5;
                        fieldData += '<data>';
                        fieldData += '<Value>' + optionvalue + '</Value>';
                        fieldData += '<GEN_MST_OrgEntity_ID>' + OrgEntityID + '</GEN_MST_OrgEntity_ID>';
                        fieldData += '<EntityID>' + MasterID + '</EntityID>';
                        fieldData += '<GEN_MST_PreferenceGroup_ID>' + GroupID + '</GEN_MST_PreferenceGroup_ID>';
                        fieldData += '<GEN_MST_Preference_ID>' + PreferenceID + '</GEN_MST_Preference_ID>';
                        fieldData += '<GEN_TRN_Preference_ID>' + keyid + '</GEN_TRN_Preference_ID>';
                        fieldData += '<GEN_MST_PreferenceOption_ID>' + val + '</GEN_MST_PreferenceOption_ID> </data>';
                    }
                }



            });
            fieldData = fieldData + '</root>';
            //fieldDataOut += '"' + String.fromCharCode(64) + 'inputDataXml' + '":"' + fieldData + '",' + '"' + String.fromCharCode(64) + 'LanguageType' + '":"' + 'en' + '",' + '"' + String.fromCharCode(64) + 'LoggedUserID' + '":"' + $("#hdnUpdatedBy").val() + '",';
            // fieldDataOut = fieldDataOut.substring(0, fieldDataOut.length - 1);
            // fieldDataOut += '}';
            //return fieldDataOut;
            return fieldData;
        }

        function InsertPreference() {
            var status = false, value;

            $(".GetPreferenceOptions").each(function () {
                var param = $(this).attr('id').replace("Pref", "");
                var paramtype = $(this).attr('type');
                if (paramtype == "radio" || paramtype == "checkbox") {
                    value = $(this).prop('checked');
                    if (value == true) {
                        status = true;
                    }
                }
                else {

                    value = $(this).val();
                    if (value != null) {
                        status = true;
                    }
                }
            });
            return status;
        }


        function UpsertPreferences() {

            var data = GetPrefernceFromData();
            var obj = {};
            obj.UserID = "<%=cp.UserID.ToString()%>";
             obj.Inxml = GetPrefernceFromData();
             $.ajax({
                 url: "NewAccount.aspx/SETPreferences",
                 dataType: 'json',
                 contentType: "application/json",
                 type: 'POST',
                 data: JSON.stringify(obj),
                 success: function (response) {

                     if (response.d == "success") {
                         showStickyToast(true, 'Saved successfully');
                         GetPreferencesList();
                         setTimeout(function () {
                             location.reload();
                         }, 1000);
                         //GetPreferencesList();
                     }

                 }
             });
         }
    </script>

</asp:Content>
