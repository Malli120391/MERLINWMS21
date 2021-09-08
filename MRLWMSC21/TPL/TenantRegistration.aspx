<%@ Page Title="Tenant Registration:." Language="C#" MasterPageFile="~/mMaterialManagement/MaterialManagementMaster.master" AutoEventWireup="true" CodeBehind="TenantRegistration.aspx.cs" Inherits="FalconAdmin.General.TenantRegistration" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
    <style>
        a.sm-btn{
            margin-bottom:1px !important;
        }
    </style>
    <script src="../Scripts/angular.min.js"></script>
    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyB-amPYw4EvJGyYfY16HzhF2lqpw--FcHM&libraries=places"></script>
    <asp:ScriptManager ID="smInboundDetails" runat="server" EnablePartialRendering="true" SupportsPartialRendering="true"></asp:ScriptManager>

    <!--Print Strart-->

    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=txtlatitude.ClientID%>").attr('readonly', 'readonly');
            $("#<%=txtlongitude.ClientID%>").attr('readonly', 'readonly');
            $("#<%=txtblatitude.ClientID%>").attr('readonly', 'readonly');
            $("#<%=txtblongitude.ClientID%>").attr('readonly', 'readonly');

            var TextFieldName = $("#txtWHCode");
            DropdownFunction(TextFieldName);
            $("#txtWHCode").autocomplete({
                source: function (request, response) {

                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadWarehouseData1") %>',
                        data: "{ 'prefix': '" + request.term + "','TenantID':'" + 0 + "'}",
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

                    $("#hifWarehouseID").val(i.item.val);
                },
                minLength: 0
            });

            var textfieldname = $('#<%=txtCountry.ClientID%>');
            DropdownFunction(textfieldname);
            $("#<%= this.txtCountry.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadCountry") %>',
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
                        },
                        error: function (response) {

                        },
                        failure: function (response) {

                        }
                    });
                },
                select: function (e, i) {
                    $("#<%=hdnCountry.ClientID %>").val(i.item.val);

                },
                minLength: 0,

            });//.val(accountid).data('autocomplete')._trigger('');

            var textfieldname = $('#<%=txtCurrency.ClientID%>');
            DropdownFunction(textfieldname);
            $("#<%= this.txtCurrency.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadCurrency") %>',
                        data: "{ 'prefix': '" + request.term + "','CountryId':" + $('#<%=this.hdnCountry.ClientID%>').val() + "}",//<=cp.TenantID%>
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
                        },
                        error: function (response) {

                        },
                        failure: function (response) {

                        }
                    });
                },
                select: function (e, i) {
                    $("#<%=hdnCUrrencyId.ClientID %>").val(i.item.val);

                },
                minLength: 0,

            });//.val(accountid).data('autocomplete')._trigger('');


            var textfieldname = $('#<%=txtState.ClientID%>');
            DropdownFunction(textfieldname);
            $("#<%= this.txtState.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadState") %>',
                        data: "{ 'prefix': '" + request.term + "','CountryId':" + $('#<%=this.hdnCountry.ClientID%>').val() + "}",//<=cp.TenantID%>
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
                        },
                        error: function (response) {

                        },
                        failure: function (response) {

                        }
                    });
                },
                select: function (e, i) {
                    $("#<%=hdnStateId.ClientID %>").val(i.item.val);

                },
                minLength: 0,

            });//.val(accountid).data('autocomplete')._trigger('');


            var textfieldname = $('#<%=txtCity.ClientID%>');
            DropdownFunction(textfieldname);
            $("#<%= this.txtCity.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadCity") %>',
                        data: "{ 'prefix': '" + request.term + "','StateId':" + $('#<%=this.hdnStateId.ClientID%>').val() + "}",//<=cp.TenantID%>
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
                        },
                        error: function (response) {

                        },
                        failure: function (response) {

                        }
                    });
                },
                select: function (e, i) {
                    $("#<%=hdnCity.ClientID %>").val(i.item.val);

                },
                minLength: 0,

            });//.val(accountid).data('autocomplete')._trigger('');

            var textfieldname = $('#<%=txtZip.ClientID%>');
            DropdownFunction(textfieldname);
            $("#<%= this.txtZip.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadZipCodes") %>',
                        data: "{ 'prefix': '" + request.term + "','CityId':" + $('#<%=this.hdnCity.ClientID%>').val() + "}",//<=cp.TenantID%>
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
                        },
                        error: function (response) {

                        },
                        failure: function (response) {

                        }
                    });
                },
                select: function (e, i) {
                    debugger;
                    $("#<%=hdnZip.ClientID %>").val(i.item.val);
                    $("#<%=txtZip.ClientID %>").val(i.item.label);
                    debugger;

                    var geocoder = new google.maps.Geocoder();
                    debugger;
                    var address = $("#<%=txtZip.ClientID %>").val();
                    geocoder.geocode({ 'address': address }, function (results, status)
                    {
                        if (status == google.maps.GeocoderStatus.OK) {
                            var latitude = results[0].geometry.location.lat();
                            var longitude = results[0].geometry.location.lng();
                            //alert("Latitude: " + latitude + "\nLongitude: " + longitude);
                            $('#<%=txtlatitude.ClientID %>').val(latitude);
                            $('#<%=this.txtlongitude.ClientID %>').val(longitude);
                        } else {
                            //alert("Request failed.")
                        }
                    });
                },
                minLength: 0,
            });//.val(accountid).data('autocomplete')._trigger('');



            //======================================  Billing Details Auto-Completes =======================================================//////////////////

            var textfieldname = $('#<%=txtBillingCountry.ClientID%>');
            DropdownFunction(textfieldname);
            $("#<%= this.txtBillingCountry.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadCountry") %>',
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
                        },
                        error: function (response) {

                        },
                        failure: function (response) {

                        }
                    });
                },
                select: function (e, i) {
                    $("#<%=hdnBillingCountry.ClientID %>").val(i.item.val);

                },
                minLength: 0,

            });//.val(accountid).data('autocomplete')._trigger('');


            var textfieldname = $('#<%=txtBillingCurrency.ClientID%>');
            DropdownFunction(textfieldname);
            $("#<%= this.txtBillingCurrency.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadCurrency") %>',
                        data: "{ 'prefix': '" + request.term + "','CountryId':" + $('#<%=this.hdnBillingCountry.ClientID%>').val() + "}",//<=cp.TenantID%>
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
                        },
                        error: function (response) {

                        },
                        failure: function (response) {

                        }
                    });
                },
                select: function (e, i) {
                    //alert($("#<%=hdnBillingCurrency.ClientID %>").val(i.item.val));
                    $("#<%=hdnBillingCurrency.ClientID %>").val(i.item.val);

                },
                minLength: 0,

            });//.val(accountid).data('autocomplete')._trigger('');


            var textfieldname = $('#<%=txtBillingState.ClientID%>');
            DropdownFunction(textfieldname);
            $("#<%= this.txtBillingState.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadState") %>',
                        data: "{ 'prefix': '" + request.term + "','CountryId':" + $('#<%=this.hdnBillingCountry.ClientID%>').val() + "}",//<=cp.TenantID%>
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
                        },
                        error: function (response) {

                        },
                        failure: function (response) {

                        }
                    });
                },
                select: function (e, i) {
                    $("#<%=hdnBillingState.ClientID %>").val(i.item.val);

                },
                minLength: 0,

            });//.val(accountid).data('autocomplete')._trigger('');

            var textfieldname = $('#<%=txtBillingCity.ClientID%>');
            DropdownFunction(textfieldname);
            $("#<%= this.txtBillingCity.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadCity") %>',
                        data: "{ 'prefix': '" + request.term + "','StateId':" + $('#<%=this.hdnBillingState.ClientID%>').val() + "}",//<=cp.TenantID%>
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
                        },
                        error: function (response) {

                        },
                        failure: function (response) {

                        }
                    });
                },
                select: function (e, i) {
                    $("#<%=hdnBillingCity.ClientID %>").val(i.item.val);

                },
                minLength: 0,

            });//.val(accountid).data('autocomplete')._trigger('');

            var textfieldname = $('#<%=txtBillingZip.ClientID%>');
            DropdownFunction(textfieldname);
            $("#<%= this.txtBillingZip.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadZipCodes") %>',
                        data: "{ 'prefix': '" + request.term + "','CityId':" + $('#<%=this.hdnBillingCity.ClientID%>').val() + "}",//<=cp.TenantID%>
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
                        },
                        error: function (response) {

                        },
                        failure: function (response) {

                        }
                    });
                },
                select: function (e, i) {
                    $("#<%=hdnBillingZip.ClientID %>").val(i.item.val);
                    $("#<%=txtBillingZip.ClientID %>").val(i.item.val);


                    var geocoder = new google.maps.Geocoder();
                    debugger;
                    var address1 = $("#<%=txtBillingZip.ClientID %>").val();
                    geocoder.geocode({ 'address': address1 }, function (results, status) {
                        if (status == google.maps.GeocoderStatus.OK) {
                            var blatitude = results[0].geometry.location.lat();
                            var blongitude = results[0].geometry.location.lng();
                            //alert("Latitude: " + blatitude + "\nLongitude: " + blongitude);
                            $('#<%=txtblatitude.ClientID %>').val(blatitude);
                            $('#<%=txtblongitude.ClientID %>').val(blongitude);
                        } else {
                            //alert("Request failed.")
                        }
                    });
                },
                minLength: 0,

            });//.val(accountid).data('autocomplete')._trigger('');

            //======================================  Billing Details Auto-Completes =======================================================//////////////////


            $("#divItemPrintData").dialog(
                {
                    autoOpen: false,
                    minHeight: 20,
                    height: '400',
                    width: '500',
                    modal: true,
                    resizable: false,
                    draggable: false,
                    overflow: "auto",
                    position: ["center top", 40],
                    open: function () {
                        $(".ui-dialog").hide().fadeIn(500);
                        $('body').css({ 'overflow': 'hidden' });
                        $('body').width($('body').width());
                        $(document).bind('scroll', function () {
                            window.scrollTo(0, 0);
                        });
                    },
                    close: function () {
                        $(".ui-dialog").fadeOut(500);
                        $(document).unbind('scroll');
                        $('body').css({ 'overflow': 'visible' });
                    }
                });

        });
        var tenantid = 0;

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
        function openDialog(title) {
            $("#divItemPrintData").dialog("option", "title", title);
            $("#divItemPrintData").dialog('open');
            NProgress.start();

            $("#divItemPrintData").block({
                message: '<img src="<%=ResolveUrl("~") %>Images/async_master.gif" />',
                css: { border: '0px' },
                fadeIn: 0,
                fadeOut: 0,
                overlayCSS: { backgroundColor: '#ffffff', opacity: 1 }
            });

            unblockDialog();
        }

        function unblockDialog() {
            $("#divItemPrintData").unblock();
            NProgress.done();
        }

        function Restrict() {
            var selected = document.getElementById('<%=ddlCountry.ClientID%>').value;
            }


        

    </script>

    <script>
            var app = angular.module('MyApp', []);
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
            app.controller('NewUser', function ($scope, $http, $timeout) {
                debugger;
                $scope.Genderdata = [{ Name: 'Male', ID: 1 }, { Name: 'FeMale', ID: 2 }];
                $scope.UserList = true;
                var tenatid = 0;
                var accountid = 0;
                if (location.href.split('?tid=')[1] > 0) {
                    debugger;
                    tenatid = location.href.split('?tid=')[1];
                    accountid = $('#<%=ddlaccount.ClientID %>').val();
                }
                else if (location.href.split('?tid=')[1].split('&')[0] > 0) {
                    tenatid = location.href.split('?tid=')[1].split('&')[0];
                }

                $scope.UserData = new User(0, parseInt(accountid), parseInt(tenatid), '', '', '', '', 0, '', '', '', '', '', true, '', '', 0, 0);
                $scope.selectedValues = [];
                //$scope.btntext = "ADD";
                $scope.$watch('selectedcategories ', function (nowSelected) {
                    debugger;
                    $scope.selectedValues = [];
                    if (!nowSelected) {
                        return;
                    }
                    angular.forEach(nowSelected, function (val) {
                        $scope.selectedValues.push(val.id.toString());
                    });
                });
                var Utypes = {
                    method: 'POST',
                    url: 'TenantRegistration.aspx/GetUserTypes',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: {}
                }
                $http(Utypes).success(function (response) {
                    debugger;
                    $scope.usertypes = response.d;
                    $scope.UserData.UserTypeID = $scope.usertypes[0].ID;



                });
                var accounts = {
                    method: 'POST',
                    url: 'TenantRegistration.aspx/GetAccount',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'tenantid': tenatid }
                }
                $http(accounts).success(function (response) {
                    $scope.AccountData = response.d;
                });



                var tenant = {
                    method: 'POST',
                    url: 'TenantRegistration.aspx/GetTenants',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'tenantid': tenatid }
                }
                $http(tenant).success(function (response) {
                    $scope.TenantData = response.d;


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
                    url: 'TenantRegistration.aspx/GetRoleData',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { "usertypeid": 3 }
                }
                $http(roles).success(function (response) {
                    $scope.Roles = response.d;
                });

                var warehouse = {
                    method: 'POST',
                    url: 'TenantRegistration.aspx/GetWareHouseData',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'tenantid': tenatid }
                }
                $http(warehouse).success(function (response) {
                    $scope.warehouses = response.d;
                });

                $scope.GetUserList = function () {
                    debugger;
                    var states = {
                        method: 'POST',
                        url: 'TenantRegistration.aspx/GetUserList',
                        headers: {
                            'Content-Type': 'application/json; charset=utf-8',
                            'dataType': 'json'
                        },
                        data: {
                            'Accountid': 0, 'tenantid': tenatid
                        }
                    }
                    $http(states).success(function (response) {
                        debugger;
                        $scope.UsersList = JSON.parse(response.d);


                    });
                }
                $scope.GetUserList();
                $scope.dispalynewwareUser = function () {
                    $scope.UserData = new User(0, parseInt(accountid), parseInt(tenatid), '', '', '', '', 0, '', '', '', '', '', true, '', '', 0, 0);
                    $scope.selectedWareHouses = null;
                    $scope.selectedValues = null;
                    $scope.DisplayNewUserDetails = true;
                    $scope.UserList = false;
                }

                $scope.CreateNewUser = function () {
                    debugger;
                    var data = $scope.UserData;
                    if ($scope.UserData.UserTypeID == undefined || $scope.UserData.UserTypeID == '' || $scope.UserData.UserTypeID == null) {
                        $scope.UserData.UserTypeID = 0;
                        showStickyToast(false, "Please select User Type ");
                        return false;
                    }

                    if ($scope.UserData.EMPCode == undefined || $scope.UserData.EMPCode == '' || $scope.UserData.EMPCode == null) {
                        showStickyToast(false, "Please Enter Employee Code ");
                        return false;
                    }

                    if ($scope.UserData.Mobile == undefined || $scope.UserData.Mobile == '' || $scope.UserData.Mobile == null) {
                        showStickyToast(false, "Please Enter Mobile No. ");
                        return false;
                    }

                    if ($scope.UserData.Password == undefined || $scope.UserData.Password == '' || $scope.UserData.Password == null) {
                        showStickyToast(false, "Please Enter Password ");
                        return false;
                    }

                    if ($scope.UserData.Gender == undefined || $scope.UserData.Gender == '' || $scope.UserData.Gender == null) {
                        showStickyToast(false, "Please select Gender ");
                        return false;
                    }

                    if ($scope.UserData.AccountID == undefined || $scope.UserData.AccountID == '' || $scope.UserData.AccountID == null) {
                        showStickyToast(false, "Please select Account ");
                        return false;
                    }

                    if ($scope.UserData.TenantID == undefined || $scope.UserData.TenantID == '') {
                        $scope.UserData.TenantID = 0;
                        //showStickyToast(false, "Please Select Tenant ");
                        //return false;
                    }
                    if ($scope.UserData.FirstName == undefined || $scope.UserData.FirstName == '') {

                        showStickyToast(false, "Please Enter First Name ");
                        return false;
                    }
                    if ($scope.UserData.Email == undefined || $scope.UserData.Email == '') {

                        showStickyToast(false, "Please Enter Email ");
                        return false;
                    }
                    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
                    var isemail;
                    isemail = regex.test($scope.UserData.Email);
                    if (!isemail) {
                        showStickyToast(false, "Please Enter valid Email ");
                        return false;
                    }

                    if ($scope.UserData.AltEmail1 != '' && $scope.UserData.AltEmail1 != undefined) {
                        isemail = regex.test($scope.UserData.AltEmail1);
                        if (!isemail) {
                            showStickyToast(false, "Please Enter valid Alt. Email1 ");
                            return false;
                        }
                    }
                    else {
                        $scope.UserData.AltEmail1 = '';
                    }
                    if ($scope.UserData.AltEmail2 != '' && $scope.UserData.AltEmail2 != undefined) {
                        isemail = regex.test($scope.UserData.AltEmail2);
                        if (!isemail) {
                            showStickyToast(false, "Please Enter valid Alt. Email2 ");
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
                    var warehouses = '';
                    var roles = '';
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

                    }


                    if ($scope.selectedValues != undefined) {
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
                    var states = {
                        method: 'POST',
                        url: 'TenantRegistration.aspx/UpsertUserData',
                        headers: {
                            'Content-Type': 'application/json; charset=utf-8',
                            'dataType': 'json'
                        },
                        data: { 'obj': $scope.UserData }
                    }
                    $http(states).success(function (response) {
                        debugger;
                        if (response.d == -111) {
                            showStickyToast(false, "Employee Code Already Exist ");
                            return false;
                        }
                        else if (response.d == -222) {
                            showStickyToast(false, "Email Already Exist ");
                            return false;
                        }
                        else if (response.d == 0) {
                            showStickyToast(false, "Error While Creating ");
                            return false;
                        }
                        else if (response.d > 0) {
                            if (response.d == 999) {
                                showStickyToast(true, "Successfully Updated ");
                            }
                            else {
                                showStickyToast(true, "Successfully Created ");
                            }
                            $scope.GetUserList();
                            $scope.DisplayNewUserDetails = false;
                            $scope.UserList = true;
                        }
                    });
                }
                $scope.getUserEditData = function (info) {
                    debugger;
                    $scope.selectedValues = null;
                    $scope.selectedWareHouses = null;
                    var data = info;
                    if (data.WarehouseIDs != '' && data.WarehouseIDs != undefined && data.WarehouseIDs != null) {
                        var states = {
                            method: 'POST',
                            url: 'TenantRegistration.aspx/setUserData',
                            headers: {
                                'Content-Type': 'application/json; charset=utf-8',
                                'dataType': 'json'
                            },
                            data: { 'obj': data.WarehouseIDs }
                        }
                        $http(states).success(function (response) {
                            $scope.selectedWareHouses = response.d;

                        });
                    }
                    var roles = {
                        method: 'POST',
                        url: 'TenantRegistration.aspx/GetRoleData',
                        headers: {
                            'Content-Type': 'application/json; charset=utf-8',
                            'dataType': 'json'
                        },
                        data: { "usertypeid": 3 }
                    }
                    $http(roles).success(function (response) {
                        $scope.Roles = response.d;
                        if (data.UserRoleIDs != '' && data.UserRoleIDs != undefined && data.UserRoleIDs != null) {
                            var states = {
                                method: 'POST',
                                url: 'TenantRegistration.aspx/setUserData',
                                headers: {
                                    'Content-Type': 'application/json; charset=utf-8',
                                    'dataType': 'json'
                                },
                                data: { 'obj': data.UserRoleIDs }
                            }
                            $http(states).success(function (response) {
                                $scope.selectedValues = response.d;

                            });
                        }
                    });

                    var isactive = false;
                    if (data.IsActive == 1) {
                        isactive = true;
                    }
                    $scope.UserData = new User(data.UserTypeID, data.AccountID, data.TenantID, data.FirstName, data.LastName, data.MiddleName, data.EmployeeCode,
                        1, data.Email, data.AlternateEmail1, data.AlternateEmail2, data.Password, data.Mobile,
                        isactive, '', '', 0, data.UserID);
                    $scope.DisplayNewUserDetails = true;
                    $scope.UserList = false;
                }
                $scope.displayuserlist = function () {
                    $scope.DisplayNewUserDetails = false;
                    $scope.UserList = true;
                }

            });
            function User(usertypeid, accountid, tenantid, firstname, lastname, middlename, empcode, gender, email, altemail1, altemail2, password, mobile,
                isactive, roles, warehouses, printerid, userid) {
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
            }
    </script>
  
    <div id="divItemPrintData">
        <div id="divItemPrintDataContainer" style="display: block; padding: 19px;">

            <asp:TreeView ID="trvmaterialattachment" Target="_blank" runat="server">
                <Nodes>
                    <asp:TreeNode Expanded="false"></asp:TreeNode>
                </Nodes>
            </asp:TreeView>
            <asp:Label ID="lblfileslist" runat="server"></asp:Label>

        </div>
    </div>

    <!--Print div end-->

    <script>

            function Checkall(flag) {
                //var frm = document.getElementsByClassName('deleteRecord');
                $("[id=cbkTenantContractID]").prop('checked', flag);
            }
            $(document).ready(function () {

                $('#cbkCheckall').click(function () {
                    if ($('#cbkCheckall').is(':checked')) {
                        Checkall(true);
                    } else
                        Checkall(false);
                });



                //======================== New Code Added by Meena ======================//

                $("#txtEffectiveFrom").datepicker({
                    dateFormat: "dd-M-yy",
                    //maxDate: new Date(),
                    onSelect: function (selected) {
                        var instance = $(this).data("datepicker");
                        var date = $.datepicker.parseDate(instance.settings.dateFormat || $.datepicker._defaults.dateFormat, selected, instance.settings);
                        date.setDate(date.getDate() + 1);
                        $("#txtEffectiveTo").datepicker("option", "minDate", date, { dateFormat: "dd-M-yy" })
                    }
                });

                $("#txtEffectiveTo").datepicker({
                    dateFormat: "dd-M-yy",
                    //maxDate: new Date()
                });


                $('#txtEffectiveFrom, #txtEffectiveTo').keypress(function () {
                    return false;
                });




                CustomAccordino($('#PrimaryInformationHeader'), $('#PrimaryInformationBody'));
                CustomAccordino($('#BillingInformationHeader'), $('#BillingInformationBody'));
                CustomAccordino($('#BankInformationHeader'), $('#BankInformationBody'));
                CustomAccordino($('#ContractInformationHeader'), $('#ContractInformationBody'));
                CustomAccordino($('#UserInformationHeader'), $('#UserInformationBody'));
                CustomAccordino($('#divFileUploadHeader'), $('#divFileUploadBody'));

            });

    </script>
    <%-- <script src="~/mMaterialManagement/FileUploader/jquery.uploadfile.min.js" type="text/javascript"></script>
    <link href="~/mMaterialManagement/FileUploader/uploadfile.min.css" rel="stylesheet" type="text/css" />--%>
    <script type="text/javascript">
            $(document).ready(function () {

            //$('#<%=this.txtlatitude.ClientID%>').attr("disabled", "disabled");
             //var fileextention;
             //if ($('#=ddldocumenttype.ClientID ').val() == 2)
             //            fileextention = "pdf";
             //        else
             //fileextention = "doc,docx,gif,jpg,png,xps,pdf";

            <%-- var uploadObj = $("#fileuploader").uploadFile({
                 url: "UploadTenantFiles.ashx",
                 multiple: true,
                 autoSubmit: false,
                 fileName: "FileNames",
                 allowedTypes: "doc,docx,pdf,jpg,gif,png,xps,txt",
                 dynamicFormData: function () {
                   
                     var parm = document.getElementById('<%=ddldocumenttype.ClientID %>');
                    DocType = parm.options[parm.selectedIndex].value;   
                    //alert(Tenantid);
                    var data = { Doctype: DocType, TenantID: '<%=ViewState["TenantID"].ToString()%>' }
                    return data;
                },
                showStatusAfterSuccess: true,
                abortStr: "Abort",
                cancelStr: "Cancel",
                doneStr: "Close",
                showDone: true,
                afterUploadAll: function () {
                    alert('All attached files are successfully uploaded');
                    location.reload();
                },
                onError: function (files, status, errMsg) {
                    alert('Error while uploading');
                },
                onSubmit: function (files) {
                },
                /*onSuccess: function (files, data, xhr) {
                    alert('One File Uploaded');

                },*/
                width: "5px"

            });
            //$('#fileuploader').fileupload({
            //    dataType: 'json',
            //    sequentialUploads: true,
            //    formData: [{ 'Doctype': 'PDF' }, {'mid': 1024 },{ 'sid': 25 }, {'ddldocumentvalue': 2 }]
            //});--%>


                function UploadFiles() {

                    //if (ValidateFileUploadData()) {
                    //if (totalAttachmentFileCount != 0) {
                    var MaterialImagesPath = '';
                    var imgurlPath = '';
                    var obj = Dropzone.forElement("#dropzoneForm");
                    var formData = new FormData();
                    for (var i = 0; i < obj.files.length; i++) {
                        var imgPath = "";
                        var fileData = obj.files[i];
                        formData.append("file", fileData);
                    }

                    $.ajax({
                        url: "../TPL/UploadTenantFiles.ashx", //"../Pages/HomePage.aspx/findUserTwitter",
                        type: 'POST',
                        data: formData,
                        processData: false,
                        contentType: false,
                        async: false,
                        success: function (result) {
                            imgurlPath = result;
                        },
                        error: function (errorData) {
                            alert("Error in uploading media, please try later.");
                        }
                    });

                    imgurlPath = imgurlPath.substring(0, imgurlPath.length - 1);
                    var tentid = new URL(window.location.href).searchParams.get("tid");
                    $.ajax({
                        url: "TenantRegistration.aspx/SetFile", //"../Pages/HomePage.aspx/findUserTwitter",
                        type: 'POST',
                        data: { '@TenantID': 'tentid', '@ResourcePath': imgurlPath },
                        processData: false,
                        contentType: false,
                        async: false,
                        success: function (result) {
                            imgurlPath = result;
                        },
                        error: function (errorData) {
                            alert("Error in uploading media, please try later.");
                        }
                    });


                    //    }
                    //}
                }

                $("#startUpload").click(function () {

                    DocType = parm.options[parm.selectedIndex].value;

                    var parm1 = Id = url.searchParams.get("tid");
                    //supplierID = parm1.options[parm1.selectedIndex].value;
                    //if (supplierID == 0) {
                    //    alert("Please select supplier");
                    //    return false;
                    //}
                    if (DocType == 0) {
                        alert("Please select attachment type");
                        return false;
                    }

                    var httpreqpl = {
                        method: 'POST',
                        url: 'TenantRegistration.aspx/SetFile',
                        headers: {
                            'Content-Type': 'application/json; charset=utf-8',
                            'dataType': 'json'
                        },
                        data: {
                            'Tenantid': 'parm1', 'filepath': ''
                        }
                    }
                    $http(httpreqpl).success(function (response) {

                        $scope.P1 = response.d;
                        $scope.P2 = response.d;

                    });
                    //  uploadObj.startUpload();


                    //var supplierID, DocType;

                    //                var parm = document.getElementById('ddldocumenttype.ClientID ');



                    //DocType = parm.options[parm.selectedIndex].value;

                    //              alert(DocType);
                    //            supplierID = parm1.options[parm1.selectedIndex].value;

                    //                alert(supplierID);

                    //$('#fileuploader').bind('fileuploadsubmit', function (e, data) {
                    //    data.formData = [{ 'Doctype': 'PDF' }, { 'mid': 1024 }, { 'sid': 25 }, { 'ddldocumentvalue': 2 }];
                    //});
                    //$('#fileuploader').fileupload({
                    //    formData: {
                    //        'Doctype': DocType,
                    //        'mid': 3,
                    //        'sid': supplierID,
                    //        'ddldocumentvalue': 2
                    //    }
                    //});

                });


            });
    </script>

    <script lang="javascript" type="text/javascript">
        //debugger;
        //alert(ValidateRegForm);
   <%--     function ValidateRegForm() {
            var email = document.getElementById("<%=txtEmail.ClientID%>");
        var filter = /^([a-zA-Z0-9_.-])+@(([a-zA-Z0-9-])+.)+([a-zA-Z0-9]{2,4})+$/;
        if (!filter.test(email.value)) {
            showStickyToast(false, 'Please provide a valid email address');
        
            email.focus;
            return false;
        }
        return true;
         }--%>



        //function validate(value) {
        //    var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
        //    if (reg.test(value) == false) {
        //        showStickyToast(false, 'Please provide a valid email address');
        //        return false;
        //    }
        //}
    </script>

    <script lang="javascript" type="text/javascript">
         function ClientMobileValidate(source, arguments) {
             arguments.IsValid = false;
             if (!(isNaN(arguments.Value))) {
                 arguments.IsValid = true;
             }
             if (arguments.Value.length == 10) {
                 arguments.IsValid = true;
             }
             else {
                 arguments.IsValid = false;
             }
         }

    </script>

    <link href="tpl.css" rel="stylesheet" />
    <style>
        td {
            font-size: 13pt !important;
        }

        /*.ui-SubHeadingBar {
            font-size: 16px !important;
            border-radius: 0px !important;
            box-shadow: var(--z1);
            margin-bottom: 3px;
            border: 0px;
            font-weight: 500 !important;
        }*/
        a.ButEmpty {
          
            background-color: transparent;                           
            text-decoration: none;
            border: 1px solid var(--sideNav-bg);
            color: #000;
            background: #fff;
            padding: 5px;
            border-radius: 3px;
            margin: 1px;
            transition: all 0.8s;
            display: inline-block;
            text-align: center;           
            font-family: inherit;
            font-size: small;
            line-height: inherit;
        }
    </style>
    <style>
        #MainContent_MMContent_trvmaterialattachmentn0Nodes table td {
            display: inline-table !important;
            width: fit-content !important;
            font-size: 13px !important;
        }

        #MainContent_MMContent_trvmaterialattachment table td {
            display: inline-table !important;
            width: fit-content !important;
            font-size: 13px !important;
        }

        .fixed-width {
            width: 230px;
            background-color: #73cadc !important;
            color: #fff !important;
            background-image: linear-gradient(80deg, #00aeff, #73cadc);
        }

        .FormSubHeading {
            font-size: 9pt !important;
            color: #ffffff;
            font-weight: normal !important;
            text-shadow: unset !important;
        }

        .litstyle {
            color: dimgrey;
            font-style: italic;
            font-size: 8pt !important;
        }

    

        #MainContent_LinkButton3 {
            text-decoration: none;
            box-shadow: 0 3px 1px -2px rgba(0,0,0,.2), 0 2px 2px 0 rgba(0,0,0,.14), 0 1px 5px 0 rgba(0,0,0,.12);
            background: #73cadc;
            font-weight: normal;
            color: #000;
            border-radius: 3px;
            margin-bottom: 9px;
            display: inline-block;
        }

  
        .ui-Customaccordion {
            margin-bottom: 15px;
        }

          
        /*.txt_Blue_Small{
            border: 1px solid #d4d0d0;
        }*/
        .oneafter label::after {
            display: none;
        }

        .oneafter label {
            width: 30%;
            word-wrap: break-word;
            font-size: 13px;
            padding-left: 10px;
        }


        .gvLightSeaBlue_DataCellGridEdit .md-select-underline {
            width: 100px !important;
        }

        #MainContent_MMContent_gvContract_lblsquareunits_0 {
            font-size: 13px;
        }

        #MainContent_MMContent_LinkButton3 {
            padding:5px;
            margin-bottom:5px !important;
            margin-top:5px;font-size:14px;
        }

        #MainContent_MMContent_LinkButton3:hover {
            color:#fff !important;
            font-size:14px;
        }
    </style>

    <%--<asp:UpdateProgress ID="uprdTenantRedg" runat="server" AssociatedUpdatePanelID="upnlTenantRedg">
            <ProgressTemplate>
                <div style="width:100%; height:100%; z-index:999; position:fixed; top:0; left:0; right:0; bottom:0; align-items:center; display:flex; justify-content:center; background: #e0ddd8ba;">
                
                <div style="align-self:center;" >
                        <div class="spinner">
                    <div class="bounce1"></div>
                    <div class="bounce2"></div>
                    <div class="bounce3"></div>
                </div>

                </div>
                                  
                </div>
                                
                                
            </ProgressTemplate>
            </asp:UpdateProgress>--%>
    <%--<asp:UpdatePanel ID="upnlTenantRedg" runat="server" RenderMode="Inline" ViewStateMode="Enabled" ClientIDMode="Inherit" UpdateMode="Conditional">
            <ContentTemplate>--%>



    <div class="pagewidth">
    <table class="" width="100%" style="table-layout: fixed;">
        <tr>
            <td></td>

        </tr>
        <tr class="btl">

            <td align="right">
                <div style="float: right;">
                    <!-- Globalization Tag is added for multilingual  -->
                    <asp:LinkButton ID="LinkButton3" runat="server" SkinID="lnkButEmpty" CssClass="btn btn-primary" PostBackUrl="~/TPL/TenantList.aspx"><i class="material-icons vl">arrow_back</i><%= GetGlobalResourceObject("Resource", "BacktoList")%>
</asp:LinkButton>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div class="ui-SubHeading ui-SubHeadingBar" id="PrimaryInformationHeader">  <%= GetGlobalResourceObject("Resource", "TenantInformation")%><span class="ui-icon"></span></div>

                <div class="ui-Customaccordion" id="PrimaryInformationBody">
                    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional" Visible="true" RenderMode="Inline">
                        <ContentTemplate>
                            <gap></gap>
                            <div class="converttodiv">
                            <div>
                                <div class="row">
                                    <div class="col m3 s3">
                                        <div class="flex">

                                            <asp:DropDownList ID="ddlaccount" runat="server" required="" />

                                            <asp:RequiredFieldValidator ID="rfvAccount" runat="server" ControlToValidate="ddlaccount" Display="Dynamic" ValidationGroup="vgTenantRegistration" />
                                            <span id="span1" class="errormsg"></span>
                                            <label>
                                                 <%= GetGlobalResourceObject("Resource", "Account")%>
                                            </label>

                                        </div>
                                    </div>
                                    <div class="col m3 s3" width="">
                                        <div class="flex">

                                            <asp:TextBox ID="txtCompanyName" runat="server" required="" MaxLength="50" />
                                            <asp:RequiredFieldValidator ID="rfvCompanyName" runat="server" ControlToValidate="txtCompanyName" Display="Dynamic" ValidationGroup="vgTenantRegistration" />
                                            <span class="errorMsg">*</span>
                                            <label>  <%= GetGlobalResourceObject("Resource", "TenantName")%></label>

                                        </div>
                                    </div>
                                    <div class="col m3 s3" width="">
                                        <div class="flex">

                                            <asp:TextBox ID="txtCompanyDBA" runat="server" MaxLength="6" required="" />
                                            <asp:RequiredFieldValidator ID="rfvCompanyDBA" runat="server" ControlToValidate="txtCompanyDBA" Display="Dynamic" ValidationGroup="vgTenantRegistration" />
                                            <span class="errorMsg">*</span><label> <%= GetGlobalResourceObject("Resource", "TenantCode")%></label>

                                        </div>
                                    </div>
                                    <div class="col m3 s3">
                                        <div class="flex">
                                            <asp:TextBox ID="txtCompanyRegistration" runat="server" required="" MaxLength="30" />
                                            <asp:RequiredFieldValidator ID="rfvtCompanyRegistration" runat="server" ControlToValidate="txtCompanyRegistration" Display="Dynamic" ValidationGroup="vgTenantRegistration" />
                                            <span class="errorMsg">*</span><label> <%= GetGlobalResourceObject("Resource", "TenantRegNo")%></label>

                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col m3 s3">
                                        <div class="flex">
                                            <asp:TextBox ID="txtWebsite" runat="server" required="" />
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="txtWebsite"  ValidationExpression="(([\w]+:)?\/\/)?(([\d\w]|%[a-fA-f\d]{2,2})+(:([\d\w]|%[a-fA-f\d]{2,2})+)?@)?([\d\w][-\d\w]{0,253}[\d\w]\.)+[\w]{2,4}(:[\d]+)?(\/([-+_~.\d\w]|%[a-fA-f\d]{2,2})*)*(\?(&?([-+_~.\d\w]|%[a-fA-f\d]{2,2})=?)*)?(#([-+_~.\d\w]|%[a-fA-f\d]{2,2})*)?$" runat="server" ValidationGroup="validWebsite" />
                                            <label><%= GetGlobalResourceObject("Resource", "Website")%> </label>
                                        </div>
                                    </div>

                                    <div class="col m3 s3">
                                        <div class="flex">

                                            <asp:TextBox ID="txtCI_email" runat="server" onchange="validate(this.value)" required="" MaxLength="64" />
                                            <span class="errorMsg"></span>
                                            <asp:RequiredFieldValidator Enabled="false" ID="rfvCI_email" runat="server" ControlToValidate="txtCI_email" Display="Dynamic" ValidationGroup="vgTenantRegistration" />
                                            <label> <%= GetGlobalResourceObject("Resource", "CompanyEmail")%>  </label>

                                        </div>
                                    </div>

                              
                                    <div class="col m3 s3">
                                        <div class="flex">

                                            <asp:TextBox ID="txtFirstName" runat="server" required="" />
                                            <label> <%= GetGlobalResourceObject("Resource", "FirstName")%> </label>

                                        </div>
                                    </div>
                                    <div class="col m3 s3">
                                        <div class="flex">

                                            <asp:TextBox ID="txtLastName" runat="server" required="" />
                                            <label><%= GetGlobalResourceObject("Resource", "LastName")%></label>

                                        </div>
                                    </div>  
                                </div>
                                <div class="row">
                                    <div class="col m3 s3">
                                        <div class="flex">

                                            <asp:TextBox ID="txtEmail" runat="server" onchange="validate(this.value)" required="" MaxLength="64" />
                                            <label> <%= GetGlobalResourceObject("Resource", "PCPEmail")%> </label>

                                        </div>

                                    </div>                             
                                    <div class="col m3 s3">
                                        <div class="flex">

                                            <asp:DropDownList ID="ddlBusinessType" runat="server" Width="140" required="" />
                                            <span class="errormsg"></span>
                                            <asp:RequiredFieldValidator ID="rfvBusinessType" runat="server" ControlToValidate="ddlBusinessType" InitialValue="0" Display="Dynamic" ValidationGroup="vgTenantRegistration" />
                                            <label> <%= GetGlobalResourceObject("Resource", "BusinessType")%> </label>

                                        </div>
                                    </div>
                                    <div class="col m3 s3">
                                        <div class="flex">
                                            <asp:TextBox ID="txtCountry" runat="server" SkinID="txt_Auto" required=""></asp:TextBox>
                                            <asp:HiddenField ID="hdnCountry" runat="server" Value="0" />
                                            <asp:DropDownList ID="ddlCountry" runat="server" Width="140" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" required="" Visible="false" />
                                            <span class="errorMsg"></span>
                                            <asp:RequiredFieldValidator ID="rfvCountry" runat="server" ControlToValidate="ddlCountry" InitialValue="0" Display="Dynamic" ValidationGroup="vgTenantRegistration" />
                                            <label> <%= GetGlobalResourceObject("Resource", "Country")%></label>

                                        </div>
                                    </div>
                                    <div class="col m3 s3">
                                        <div class="flex">
                                            <asp:TextBox ID="txtCurrency" runat="server" required=""></asp:TextBox>
                                            <asp:HiddenField ID="hdnCUrrencyId" runat="server" Value="0" />
                                            <asp:DropDownList ID="ddlCurrency" runat="server" Width="140" required="" Visible="false">
                                              
                                            </asp:DropDownList>
                                           <span class="errorMsg"></span>
                                            <asp:RequiredFieldValidator ID="rfvCurrency" runat="server" ControlToValidate="ddlCurrency" InitialValue="0" Display="Dynamic" ValidationGroup="vgTenantRegistration" />
                                            <label>  <%= GetGlobalResourceObject("Resource", "Currency")%></label>

                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col m3 s3">
                                        <div class="flex">
                                            <asp:TextBox ID="txtState" runat="server" required="" />
                                            <asp:HiddenField ID="hdnStateId" runat="server" Value="0" />
                                            <span class="errorMsg"></span>
                                            <asp:RequiredFieldValidator ID="rfvstate" runat="server" ControlToValidate="txtState" Display="Dynamic" ValidationGroup="vgTenantRegistration" />
                                            <label> <%= GetGlobalResourceObject("Resource", "State")%></label>
                                        </div>
                                    </div>
                                    <div class="col m3 s3">
                                        <div class="flex">
                                            <asp:TextBox ID="txtCity" runat="server" required="" />
                                            <asp:HiddenField ID="hdnCity" runat="server" Value="0" />
                                            <span class="errorMsg"></span>
                                            <asp:RequiredFieldValidator ID="rfvcity" runat="server" ControlToValidate="txtCity" Display="Dynamic" ValidationGroup="vgTenantRegistration" />
                                            <label> <%= GetGlobalResourceObject("Resource", "City")%></label>

                                        </div>
                                    </div>
                                    <div class="col m3 s3">
                                        <div class="flex">

                                            <asp:TextBox ID="txtZip" runat="server" onkeypress="return isNumberKey(event)" MaxLength="6" required="" />
                                            <asp:HiddenField ID="hdnZip" runat="server" Value="0" />
                                            <span class="errorMsg"></span>
                                            <asp:RequiredFieldValidator ID="rfvZip" runat="server" ControlToValidate="txtZip" Display="Dynamic" ValidationGroup="vgTenantRegistration" />
                                            <label> <%= GetGlobalResourceObject("Resource", "ZipCode")%></label>

                                        </div>
                                    </div>
                                    <div class="col m3 s3">
                                        <div class="flex">
                                            <asp:TextBox ID="txtMobile" runat="server" onblur="CheckDecimal(this)" onkeypress="return isNumberKey(event)" MaxLength="10" required="" allow-only-numbers />
                                            <span class="errormsg"></span>
                                            <asp:RequiredFieldValidator ID="rfvMobile" runat="server" ControlToValidate="txtMobile" Display="Dynamic" ValidationGroup="vgTenantRegistration" />
                                            <label><%= GetGlobalResourceObject("Resource", "Mobile")%></label>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">

                                    <div class="col m3 s3">
                                        <div class="flex">
                                            <asp:TextBox ID="txtPhoneno1" runat="server" onkeypress="return isNumberKey(event)" onblur="CheckDecimal(this)" MaxLength="15" required="" />
                                            <label> <%= GetGlobalResourceObject("Resource", "Phone")%></label>
                                        </div>
                                    </div>
                                    <div class="col m3 s3">
                                        <div class="flex">
                                            <asp:TextBox ID="txtPhoneno2" runat="server" onkeypress="return isNumberKey(event)" onblur="CheckDecimal(this)" MaxLength="15" required="" />
                                            <label><%= GetGlobalResourceObject("Resource", "Phones")%></label>
                                        </div>
                                    </div>
                                    <div class="col m3 s3">
                                        <div class="flex">

                                            <asp:TextBox ID="txtAddressLine1" runat="server" required=""  TextMode="MultiLine"  MaxLength="200" />
                                            <span class="errormsg"></span>
                                            <asp:RequiredFieldValidator ID="rfvAddressLine1" runat="server" ControlToValidate="txtAddressLine1" Display="Dynamic" ValidationGroup="vgTenantRegistration" />
                                            <label>  <%= GetGlobalResourceObject("Resource", "AddressLine")%></label>

                                        </div>

                                    </div>
                                    <div class="col m3 s3">
                                        <div class="flex">

                                            <asp:TextBox ID="txtAddressLine2" runat="server" required=""  TextMode="MultiLine"  MaxLength="200" />
                                            <label> <%= GetGlobalResourceObject("Resource", "AddressLines")%> </label>

                                        </div>
                                    </div>
                                    
                                </div>
                                <div class="row">
                                    <div class="col m3 s3">
                                        <div class="flex">
                                            <asp:TextBox ID="txtGstNumber" runat="server" required="" MaxLength="15" />
                                            <label><%= GetGlobalResourceObject("Resource", "TaxNumber")%></label>
                                        </div>
                                    </div>
                                    <div class="col m3 s3">
                                        <div class="flex">
                                            <asp:TextBox ID="txtlatitude" runat="server" CssClass="AddressfieldToGet" required="" ></asp:TextBox>
                                            <label>  <%= GetGlobalResourceObject("Resource", "Latitude")%></label>
                                        </div>
                                    </div>
                                    <div class="col m3 s3">
                                        <div class="flex">
                                            <asp:TextBox ID="txtlongitude" runat="server" CssClass="AddressfieldToGet" required="" ></asp:TextBox>
                                            <label> <%= GetGlobalResourceObject("Resource", "Longitude")%></label>
                                        </div>
                                    </div>
                               
                                    <div class="col m3 s3">
                                        <div class="checkbox"><asp:CheckBox ID="cbxIsInsurence" Visible="false" runat="server" Text="Is Insurance" required="" /></div>
                                    </div>
                                </div>
                            </div>
                                </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlCountry" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div class="ui-SubHeading ui-SubHeadingBar" id="BillingInformationHeader"> <%= GetGlobalResourceObject("Resource", "BillingInformation")%> <span class="ui-icon"></span></div>

                <div class="ui-Customaccordion" id="BillingInformationBody">
                    <br />
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" Visible="true" RenderMode="Inline">
                        <ContentTemplate>
                             <div class="">
                            <div>
                                <div class="row">
                                    <div class="col m12 s12">
                                        <div class="checkbox">
                                            <asp:CheckBox ID="cbxSameAddress" OnCheckedChanged="cbxSameAddress_CheckedChanged"  AutoPostBack="true" runat="server" />
                                            <label><%= GetGlobalResourceObject("Resource", "SameasContactAddress")%></label>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col m3 s3">
                                        <div class="flex">
                                            <asp:DropDownList ID="ddlBilltype" runat="server" required="" />
                                            <span class="errorMsg"></span>
                                            <asp:RequiredFieldValidator ID="rfvBilltype" runat="server" ControlToValidate="ddlBilltype" InitialValue="0" Display="Dynamic" ValidationGroup="vgTenantRegistration" />
                                            <label>
                                                <%= GetGlobalResourceObject("Resource", "BillType")%></label>

                                        </div>
                                    </div>

                                    <div class="col m3 s3">
                                        <div class="flex">
                                            <asp:TextBox ID="txtBillingCountry" runat="server"></asp:TextBox>
                                            <asp:HiddenField ID="hdnBillingCountry" runat="server" Value="0" />
                                            <asp:DropDownList ID="ddlBillingCountry" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlBillingCountry_SelectedIndexChanged" required="" Visible="false" />
                                            <span class="errorMsg"></span>
                                            <asp:RequiredFieldValidator ID="rfvBillingCountry" runat="server" ControlToValidate="ddlBillingCountry" InitialValue="0" Display="Dynamic" ValidationGroup="vgTenantRegistration" />
                                            <label><%= GetGlobalResourceObject("Resource", "Country")%> </label>

                                        </div>
                                    </div>
                                    <div class="col m3 s3">

                                        <div class="flex">

                                            <asp:TextBox ID="txtBillingCurrency" runat="server"></asp:TextBox>
                                            <asp:HiddenField ID="hdnBillingCurrency" runat="server" Value="0" />
                                            <asp:DropDownList ID="ddlBillingCurrency" runat="server" Width="140" required="" Visible="false" />
                                            <span class="errorMsg"></span>
                                            <asp:RequiredFieldValidator ID="rfvBillingCurrency" runat="server" ControlToValidate="ddlBillingCurrency" InitialValue="0" Display="Dynamic" ValidationGroup="vgTenantRegistration" />
                                            <label><%= GetGlobalResourceObject("Resource", "Currency")%> </label>

                                        </div>
                                    </div>
                                    <div class="col m3 s3">
                                        <div class="flex">
                                            <asp:TextBox ID="txtBillingState" runat="server" required="" />
                                            <asp:HiddenField ID="hdnBillingState" runat="server" Value="0" />
                                            <span class="errorMsg"></span>
                                            <asp:RequiredFieldValidator ID="rfvbillingstate" runat="server" ControlToValidate="txtBillingState" Display="Dynamic" ValidationGroup="vgTenantRegistration" />
                                            <label><%= GetGlobalResourceObject("Resource", "State")%>  </label>

                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col m3 s3">
                                        <div class="flex">
                                            <asp:TextBox ID="txtBillingCity" runat="server" required="" />
                                            <asp:HiddenField ID="hdnBillingCity" runat="server" Value="0" />
                                            <span class="errorMsg"></span>
                                            <asp:RequiredFieldValidator ID="rfvbillingcity" runat="server" ControlToValidate="txtBillingCity" Display="Dynamic" ValidationGroup="vgTenantRegistration" />
                                            <label><%= GetGlobalResourceObject("Resource", "City")%>  </label>

                                        </div>
                                    </div>
                                    <div class="col m3 s3">

                                        <div class="flex">

                                            <asp:TextBox ID="txtBillingZip" runat="server" MaxLength="6" onkeypress="return isNumberKey(event)" onblur="CheckDecimal(this)" required="" />
                                            <asp:HiddenField ID="hdnBillingZip" runat="server" Value="0" />
                                            <span class="errorMsg"></span>
                                            <asp:RequiredFieldValidator ID="rfvBillingZip" runat="server" ControlToValidate="txtBillingZip" Display="Dynamic" ValidationGroup="vgTenantRegistration" />
                                            <label><%= GetGlobalResourceObject("Resource", "ZipCode")%>  </label>

                                        </div>
                                    </div>

                                    <div class="col m3 s3">

                                        <div class="flex">

                                            <asp:TextBox ID="txtBillEmail" runat="server" onchange="validate(this.value)" required="" MaxLength="64" />
                                            <span class="errorMsg"></span>
                                            <asp:RequiredFieldValidator ID="rfvBillEmail" runat="server" ControlToValidate="txtBillEmail" Display="Dynamic" ValidationGroup="vgTenantRegistration" />
                                            <label><%= GetGlobalResourceObject("Resource", "Email")%> </label>

                                        </div>
                                    </div>
                                    <div class="col m3 s3">

                                        <div class="flex">

                                            <asp:TextBox ID="txtBillPhoneno1" runat="server" onkeypress="return isNumberKey(event)" onblur="CheckDecimal(this)" MaxLength="15" required="" />
                                            <label><%= GetGlobalResourceObject("Resource", "Phone")%>   </label>

                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col m3 s3">

                                        <div class="flex">

                                            <asp:TextBox ID="txtBillPhoneno2" runat="server" onkeypress="return isNumberKey(event)" onblur="CheckDecimal(this)" MaxLength="15" required="" />
                                            <label><%= GetGlobalResourceObject("Resource", "Phones")%>   </label>

                                        </div>
                                    </div>
                               
                                    <div class="col m3 s3">

                                        <div class="flex">

                                            <asp:TextBox ID="txtBillingMobile" runat="server" onkeypress="return isNumberKey(event)" onblur="CheckDecimal(this)" MaxLength="10" required="" />
                                            <span class="errorMsg"></span>
                                            <asp:RequiredFieldValidator ID="rfvBillingMobile" runat="server" ControlToValidate="txtBillingMobile" Display="Dynamic" ValidationGroup="vgTenantRegistration" />
                                            <label><%= GetGlobalResourceObject("Resource", "Mobile")%>   </label>

                                        </div>
                                    </div>
                                    <div class="col m3 s3">
                                        <div class="flex">
                                            <asp:TextBox ID="txtblatitude" runat="server" CssClass="AddressfieldToGet" required=""></asp:TextBox>
                                            <label style="margin-bottom: 2px;"><%= GetGlobalResourceObject("Resource", "Latitude")%>   </label>
                                        </div>
                                    </div>
                                    <div class="col m3 s3">
                                        <div class="flex">
                                            <asp:TextBox ID="txtblongitude" runat="server" CssClass="AddressfieldToGet" required=""></asp:TextBox>
                                            <label><%= GetGlobalResourceObject("Resource", "Longitude")%>   </label>
                                        </div>
                                    </div>

                                </div>

                                <div class="row">

                                    <div class="col m3 s3">
                                        <div class="flex">
                                            <asp:TextBox ID="txtBillingAddressLine1" runat="server" required=""  TextMode="MultiLine"  MaxLength="200" />
                                            <span class="errorMsg"></span>
                                            <asp:RequiredFieldValidator ID="rfvBillingAddressLine1" runat="server" ControlToValidate="txtBillingAddressLine1" Display="Dynamic" ValidationGroup="vgTenantRegistration" />
                                            <label><%= GetGlobalResourceObject("Resource", "AddressLine")%>  </label>
                                        </div>
                                    </div>
                                    <div class="col m3 s3">
                                        <div class="flex">
                                            <asp:TextBox ID="txtBillingAddressLine2" runat="server" required="" TextMode="MultiLine"  MaxLength="200" />
                                            <label><%= GetGlobalResourceObject("Resource", "AddressLines")%> </label>
                                        </div>
                                    </div>

                                    <div class="col m3 s12">

                                        <div class="checkbox">

                                            <asp:CheckBox ID="cbxIsLock" runat="server" Text="" />
                                            <label><%= GetGlobalResourceObject("Resource", "Areyouwillingtoaccepttheinvoicingfromthenextbusinesshours")%></label>

                                            <asp:HiddenField ID="hifTenantActivityRateID" runat="server" Value="0" />
                                        </div>
                                    </div>
                                    <div class="col m3 s12">

                                        <div class="flex__ checkbox chkIsActive">
                                            <asp:CheckBox ID="cbtaxapplicable" runat="server" />
                                            <label><%= GetGlobalResourceObject("Resource", "TaxApplicable")%></label>
                                        </div>
                                    </div>

                                </div>
                                <div class="row">
                                    <div class="col m12">
                                        <asp:Label Text="Type" ID="lbltype" Visible="false" runat="server"></asp:Label><br />
                                        <asp:DropDownList ID="ddltaxtype" Visible="false" runat="server" Width="140" />
                                    </div>
                                </div>
                            </div>
                                 </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlBillingCountry" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </td>
        </tr>
        <%--  <tr>
                        <td>
                            <div class="ui-SubHeading ui-SubHeadingBar" id="BankInformationHeader">Bank Information </div>

                            <div class="ui-Customaccordion" id="BankInformationBody">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:RequiredFieldValidator ID="rfvBankName" runat="server" ControlToValidate="txtBankName" Display="Dynamic" ValidationGroup="vgTenantRegistration" />
                                           <span style="color:red">*</span> Bank Name:<br />
                                            <asp:TextBox ID="txtBankName" runat="server" />
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="rfvAccountNo" runat="server" ControlToValidate="txtAccountNo" Display="Dynamic" ValidationGroup="vgTenantRegistration" />
                                            <span style="color:red">*</span>Account No. :<br />
                                            <asp:TextBox ID="txtAccountNo" runat="server" />
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="rfvIBAN" runat="server" ControlToValidate="txtIBAN" Display="Dynamic" ValidationGroup="vgTenantRegistration" />
                                           <span style="color:red">*</span> IBAN:<br />
                                            <asp:TextBox ID="txtIBAN" runat="server" />
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="rfvBIC" runat="server" ControlToValidate="txtBIC" ErrorMessage=" * " Display="Dynamic" ValidationGroup="vgTenantRegistration" />
                                            Swift Code:<br />
                                            <asp:TextBox ID="txtBIC" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                         <td>
                                            <asp:RequiredFieldValidator ID="rfvBankAddress" runat="server" ControlToValidate="txtBankAddress" ErrorMessage=" * " Display="Dynamic" ValidationGroup="vgTenantRegistration" />
                                            Bank Address :<br />
                                            <asp:TextBox ID="txtBankAddress" runat="server" TextMode="MultiLine" />
                                        </td>
                            
                                        <td>
                                            <asp:RequiredFieldValidator ID="rfvBankCity" runat="server" ControlToValidate="txtBankCity" ErrorMessage=" * " Display="Dynamic" ValidationGroup="vgTenantRegistration" />
                                            City :<br />
                                            <asp:TextBox ID="txtBankCity" runat="server" Width="140" />
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="rfvBankCountry" runat="server" ControlToValidate="ddlBankCountry" InitialValue="0" ErrorMessage=" * " Display="Dynamic" ValidationGroup="vgTenantRegistration" />
                                            Country :<br />
                                            <asp:DropDownList ID="ddlBankCountry" runat="server" Width="140" />
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="rfvBankCurrency" runat="server" ControlToValidate="ddlBankCurrency" InitialValue="0" ErrorMessage=" * " Display="Dynamic" ValidationGroup="vgTenantRegistration" />
                                            Currency :<br />
                                            <asp:DropDownList ID="ddlBankCurrency" runat="server" Width="140" />
                                        </td>
                           
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:CheckBox ID="cbxDirectDebit" runat="server" Text="Is Direct Debit" />
                                        </td>
                                    </tr>
                              </table>
                            </div>
                        </td>
                    </tr>--%>
        <tr id="trContractInformation" runat="server" visible="false">
            <td>
                <div class="ui-SubHeading ui-SubHeadingBar" id="ContractInformationHeader"><%= GetGlobalResourceObject("Resource", "ContractInformation")%> <span class="ui-icon"></span></div>

                <div class="ui-Customaccordion" id="ContractInformationBody" style="overflow-x: auto;">
                    <%--<table width="100%">
                                    <tr>
                                        <td>
                                            <asp:RequiredFieldValidator ID="rfvContract" runat="server" ControlToValidate="txtContract" ErrorMessage=" * " Display="Dynamic" ValidationGroup="vgTenantRegistration" />
                                            Contract Name:<br />
                                            <asp:TextBox ID="txtContract" runat="server" />
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="rfvContractFromDate" runat="server" ControlToValidate="txtContractFromDate" ErrorMessage=" * " Display="Dynamic" ValidationGroup="vgTenantRegistration" />
                                            From Date:<br />
                                            <asp:TextBox ID="txtContractFromDate" runat="server" ClientIDMode="Static" />
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="rfvContractToDate" runat="server" ControlToValidate="txtContractToDate" ErrorMessage=" * " Display="Dynamic" ValidationGroup="vgTenantRegistration" />
                                            To Date:<br />
                                            <asp:TextBox ID="txtContractToDate" runat="server" ClientIDMode="Static" />
                                        </td>
                                        <td>Remarks:<br />
                                            <asp:TextBox ID="txtContractRemarks" runat="server" TextMode="MultiLine" />
                                        </td>
                                    </tr>
                                </table>--%>
                    <div>


                        <div class="row">
                            <div class="" align="right">
                                <gap5></gap5>
                                <div flex end>
                                    <asp:LinkButton ID="lnkAddContract" runat="server" OnClick="lnkAddContract_Click" CssClass="btn btn-primary btn-sm"> <%= GetGlobalResourceObject("Resource", "AddNew")%> <i class="material-icons vl">&#xE145;</i></asp:LinkButton>
                                </div>
                            </div>
                           
                        </div>
                        <div class="row">
                            <div class="col m12">

                                <asp:Literal ID="ltContracterrorMsg" runat="server" /><br />
                                <div class="inscroll" style="width: 100% !important; overflow: auto !important;">
                                    <asp:GridView ID="gvContract" runat="server" SkinID="gvLightSteelBlueNew" AllowPaging="true" AutoGenerateColumns="false" Visible="false"
                                        OnRowDataBound="gvContract_RowDataBound"
                                        OnRowEditing="gvContract_RowEditing"
                                        OnRowCancelingEdit="gvContract_RowCancelingEdit"
                                        OnRowUpdating="gvContract_RowUpdating"
                                        OnPageIndexChanging="gvContract_PageIndexChanging">
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$Resources:Resource,ContractName%>">
                                                <ItemTemplate>
                                                    <asp:Literal ID="ltContractName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TenantContract") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <div class="gridInput">
                                                        <asp:RequiredFieldValidator ID="rfvContractName" runat="server" ControlToValidate="txtContractName" ValidationGroup="vgContract" Display="Dynamic" />
                                                        <span class="errorMsg"></span>
                                                        <asp:TextBox ID="txtContractName" ClientIDMode="Static" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TenantContract") %>' MaxLength="40" />
                                                        <asp:HiddenField ID="hifTenantContractID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"TenantContractID") %>' />
                                                    </div>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$Resources:Resource,Warehouse%>">
                                                <ItemTemplate>
                                                    <div title='<%#Eval("WHName") %>'><%#Eval("WHCode") %>   </div>
                                                    <%--  <asp:Literal ID="ltWHCode" runat="server"  Text='<%#DataBinder.Eval(Container.DataItem,"WHCode") %>' />--%>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <div class="gridInput">
                                                        <asp:RequiredFieldValidator ID="rfvWHCode" runat="server" ControlToValidate="txtWHCode" ValidationGroup="vgContract" Display="Dynamic" />
                                                        <span class="errorMsg"></span>
                                                        <asp:TextBox ID="txtWHCode" ClientIDMode="Static" Height="10%" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"WHCode") %>' />
                                                        <asp:HiddenField ID="hifWarehouseID" runat="server" ClientIDMode="Static" Value='<%#DataBinder.Eval(Container.DataItem,"WarehouseID") %>' />
                                                    </div>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$Resources:Resource,EffectiveFrom%>">
                                                <ItemTemplate>
                                                    <asp:Literal ID="ltActivityRateGroup" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"EffectiveFrom","{0:dd-MMM-yyyy}") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <div class="gridInput">
                                                        <asp:RequiredFieldValidator ID="rfvEffectiveFrom" runat="server" ControlToValidate="txtEffectiveFrom" ValidationGroup="vgContract" Display="Dynamic" />
                                                        <span class="errorMsg"></span>
                                                        <asp:TextBox ID="txtEffectiveFrom" ClientIDMode="Static" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"EffectiveFrom","{0:dd-MMM-yyyy}") %>' />
                                                    </div>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="<%$Resources:Resource,EffectiveTo%>">
                                                <ItemTemplate>
                                                    <asp:Literal ID="ltActivityRateType" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"EffectiveTo","{0:dd-MMM-yyyy}") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <div class="gridInput">
                                                        <asp:RequiredFieldValidator ID="rfvEffectiveTo" runat="server" ControlToValidate="txtEffectiveTo" ValidationGroup="vgContract" Display="Dynamic" />
                                                        <span class="errorMsg"></span>
                                                        <asp:TextBox ID="txtEffectiveTo" ClientIDMode="Static" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"EffectiveTo","{0:dd-MMM-yyyy}") %>' />
                                                    </div>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$Resources:Resource,SquareUnits%>" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Literal ID="ltSquareUnits" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Value") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <div class=" " style="display: flex; align-items: center;">
                                                        <label style="width: 130px; font-size: 12px;">
                                                            <asp:CheckBox ID="cbSpaceRental" OnCheckedChanged="cbSpaceRental_CheckedChanged" AutoPostBack="true" runat="server" />
                                                           <%= GetGlobalResourceObject("Resource", "SpaceRental")%></label>
                                                    
                                                        <div class="flex">
                                                            <asp:TextBox ID="txtSquareUnits" required="" runat="server" Visible="false" MaxLength="10" onkeypress="return isNumberKey(event)" onblur="CheckDecimal(this)" Text='<%#DataBinder.Eval(Container.DataItem,"Value") %>' />
                                                            <label><asp:Label ID="lblsquareunits" runat="server" Text="Square Feets" Visible="false" /></label>
                                                        </div>
                                                    </div>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$Resources:Resource,Remarks%>">
                                                <ItemTemplate>
                                                    <asp:Literal ID="ltRemarks" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Remarks") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <div class="gridInput">
                                                        <asp:RequiredFieldValidator Enabled="false" ID="rfvActivityRateName" runat="server" ControlToValidate="txtRemarks" ValidationGroup="vgContract" />
                                                        <asp:TextBox ID="txtRemarks" ClientIDMode="Static" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Remarks") %>' MaxLength="100" />
                                                    </div>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Contract Agreement File">
                                                <ItemTemplate>
                                                    <asp:Literal ID="ltpath" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ResourcePath") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>

                                                    <asp:FileUpload ID="FUattachment" runat="server" accept=".pdf,.docx,.doc" />
                                                    <div>
                                                        <span class="litstyle"><span class="aststyle">*</span><asp:Literal ID="Literal3" runat="server" Text='File Types : .pdf, .doc, .docx,' /></span>
                                                        <span class="litstyle"><span class="aststyle">*</span><asp:Literal ID="Literal4" runat="server" Text='Max File Size : 5MB' /></span>
                                                    </div>
                                                    <p style="font-size: 13px !important; color: green !important;">
                                                        <asp:Literal ID="ltpath1" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ResourcePath") %>' />
                                                    </p>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$Resources:Resource,Active%>" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                              
                                                    <asp:Literal ID="ltIsActive" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"IsActive").ToString()=="1"?"Yes":"No" %>'></asp:Literal>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:RequiredFieldValidator Enabled="false" ID="rfvIsActive" runat="server" ControlToValidate="txtRemarks" ValidationGroup="vgContract" />
                                                    <asp:CheckBox ID="cbkactive" runat="server" Checked='<%#GetBool(DataBinder.Eval(Container.DataItem, "IsActive").ToString())%>' ClientIDMode="Static"  />
                                                    <%--<asp:TextBox ID="txtRemarks" ClientIDMode="Static" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Remarks") %>'/>--%>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <div class="">
                                                        
                                                        <asp:CheckBox ID="cbkCheckall" runat="server" ClientIDMode="Static" />
                                                        <label for="" style="text-indent: 20px;" class="p0"><%= GetGlobalResourceObject("Resource", "Delete")%></label>

                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <label class="">
                                                        <asp:CheckBox ID="cbkTenantContractID" runat="server" ClientIDMode="Static" />
                                                        <span></span>
                                                    </label>
                                                    <asp:HiddenField ID="hifDeleteTenantContractID" Value='<%#DataBinder.Eval(Container.DataItem,"TenantContractID") %>' runat="server" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                   <%-- <asp:LinkButton ID="lnkDelete" runat="server" OnClick="lnkDelete_Click" OnClientClick="return window.confirm('Are you sure do you want to delete?');" Font-Underline="false"><%=MRLWMSC21Common.CommonLogic.btnfaDelete %></asp:LinkButton>--%>

                                                     <asp:LinkButton ID="lnkDelete" runat="server" OnClick="lnkDelete_Click" OnClientClick="return  window.confirm('Are you sure do you want to delete?');" Font-Underline="false"><%=MRLWMSC21Common.CommonLogic.btnfaDelete %></asp:LinkButton>
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                          <asp:CommandField ControlStyle-CssClass="sm-btn" ButtonType="Link" ItemStyle-CssClass=""  EditText="Edit" ShowEditButton="True" />
                                        </Columns>
                                    </asp:GridView>
                                </div>

                            </div>
                        </div>

                        <div class="row">


                            <div class="col m12">
                                <div align="right">
                                    <asp:Literal ID="ltPicHolder" runat="server" />
                                    <br />
                                    <asp:Label ID="lblvieweattachment" runat="server"  style="font-size: 12px;"></asp:Label></>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </td>
        </tr>

        <tr id="trUserInformation" runat="server" visible="false">
            <td>
                <div class="ui-SubHeading ui-SubHeadingBar" id="UserInformationHeader">  <%= GetGlobalResourceObject("Resource", "UserInformation")%> <span class="ui-icon"></span></div>

                <div class="ui-Customaccordion" id="UserInformationBody">
                    <div class="angulardiv" ng-app="MyApp" ng-controller="NewUser">
                        <br />
                        <div ng-cloak ng-show="DisplayNewUserDetails">
                            <div class="ui-SubHeading ui-SubHeadingBar" id="UserCreationHeader"> " <%= GetGlobalResourceObject("Resource", "Usercreation")%>  </div>

                            <div class="ui-Customaccordion" id="UserCreationBody">
                                <br />
                                <div class="row">

                                    <div class="flex__ right">

                                        <button type="button" ng-click="displayuserlist()" class="addbuttonOutbound btn btn-primary"><i class="material-icons">arrow_back</i>&nbsp;&nbsp;<%= GetGlobalResourceObject("Resource", "BackToList")%></button>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col m3 s3">
                                        <div class="flex">

                                            <div>
                                                <select ng-model="UserData.UserTypeID" class="DropdownGH" ng-options="SC.ID as SC.Name for SC in usertypes" ng-change="checktenats()">
                                                    <option value="" selected hidden />
                                                </select>
                                                <span class="errorMsg"></span>
                                                <label> <%= GetGlobalResourceObject("Resource", "SelectUserType")%></label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col m3 s3">
                                        <div class="flex">

                                            <div>
                                                <select ng-model="UserData.AccountID" class="DropdownGH" ng-options="SC.ID as SC.Name for SC in AccountData" required="">
                                                    <option value="" selected hidden />
                                                </select>
                                                <span class="errorMsg"></span>
                                                <label>  <%= GetGlobalResourceObject("Resource", "SelectAccount")%></label>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col m3 s3">
                                        <div class="flex">

                                            <div>
                                                <select ng-model="UserData.TenantID" ng-disabled="Disabled" class="DropdownGH" ng-options="SC.ID as SC.Name for SC in TenantData">
                                                    <option value="" selected hidden />
                                                </select>
                                                <span class="errorMsg"></span>
                                                <label>  <%= GetGlobalResourceObject("Resource", "SelectTenant")%></label>
                                            </div>
                                        </div>

                                    </div>



                                </div>

                                <div class="row">

                                    <div class="col m3 s3">
                                        <div class="flex">

                                            <div>
                                                <input type="text" ng-model="UserData.FirstName" required="" />
                                                <span class="errorMsg"></span>
                                                <label>  <%= GetGlobalResourceObject("Resource", "FirstName")%></label>
                                            </div>
                                        </div>
                                    </div>





                                    <div class="col m3 s3">
                                        <div class="flex">

                                            <div>
                                                <input type="text" ng-model="UserData.MiddleName" required="" />
                                                <label> <%= GetGlobalResourceObject("Resource", "MiddleName")%></label>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col m3 s3">
                                        <div class="flex">

                                            <div>
                                                <input type="text" ng-model="UserData.LastNane" required="" />
                                                <label> <%= GetGlobalResourceObject("Resource", "LastName")%> </label>
                                            </div>
                                        </div>
                                    </div>

                                </div>

                                <div class="row">

                                    <div class="col m3 s3">
                                        <div class="flex">

                                            <div>
                                                <input type="text" ng-model="UserData.EMPCode" required="" />
                                                <span class="errorMsg"></span>
                                                <label>  <%= GetGlobalResourceObject("Resource", "EmployeeCode")%></label>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col m3 s3">
                                        <div class="flex">

                                            <div>
                                                <div>
                                                    <select ng-model="UserData.Gender" class="DropdownGH" ng-options="SC.ID as SC.Name for SC in Genderdata" required="">
                                                        <option value="" selected hidden />

                                                    </select>
                                                    <span class="errorMsg"></span>
                                                    <label>  <%= GetGlobalResourceObject("Resource", "SelectGender")%> </label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col m3 s3">
                                        <div class="flex">

                                            <div>
                                                <input type="text" ng-model="UserData.Email" required="" maxlength="64" />
                                                <span class="errorMsg"></span>
                                                <label>  <%= GetGlobalResourceObject("Resource", "Email")%> </label>
                                            </div>
                                        </div>
                                    </div>

                                </div>

                                <div class="row">

                                    <div class="col m3 s3">
                                        <div class="flex">

                                            <div>
                                                <input type="text" ng-model="UserData.AltEmail1" required="" maxlength="64" />
                                                <label> <%= GetGlobalResourceObject("Resource", "AltEmail")%>   </label>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col m3 s3">
                                        <div class="flex">

                                            <div>
                                                <input type="text" ng-model="UserData.AltEmail2" required="" maxlength="64" />
                                                <label> <%= GetGlobalResourceObject("Resource", "AltEmails")%>  </label>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col m3 s3">
                                        <div class="flex">

                                            <div>
                                                <input type="password" ng-model="UserData.Password" required="" />
                                                <span class="errorMsg"></span>
                                                <label> <%= GetGlobalResourceObject("Resource", "Password")%> </label>
                                            </div>
                                        </div>
                                    </div>

                                </div>


                                <div class="row">

                                    <div class="col m3 s3">
                                        <div class="flex">

                                            <div>
                                                <input type="text" ng-model="UserData.Mobile" required="" />
                                                <span class="errorMsg"></span>
                                                <label> <%= GetGlobalResourceObject("Resource", "Mobile")%> </label>
                                            </div>
                                        </div>
                                    </div>


                                    <div class="col m3 s3">
                                        <div class="flex">

                                            <div>
                                                <select ng-model="UserData.PrinterID" class="DropdownGH" ng-options="SC.ID as SC.Name for SC in PrinterData" required="">
                                                    <option value="" selected hidden />
                                                </select>
                                                <label> <%= GetGlobalResourceObject("Resource", "SelectPrinter")%></label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col m3 s3">
                                        <div class="flex">

                                            <div class="checkbox chkIsActive">
                                                <input type="checkbox" ng-model="UserData.Isactive" />
                                                <label>  <%= GetGlobalResourceObject("Resource", "Active")%></label>
                                            </div>
                                        </div>
                                    </div>



                                </div>

                                <div class="row">

                                    <div class="col m3 s3">
                                        <div class="flex">

                                            <div>

                                                <select multiple ng-model="selectedValues" style="width: 60%;" size="7">
                                                    <option ng-repeat="category in Roles" value="{{category.ID}}" ng-selected="{{selectedValues.indexOf(category.ID.toString())!=-1}}">{{category.Name}}</option>
                                                </select>
                                                <label>  <%= GetGlobalResourceObject("Resource", "Roles")%> </label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col m3 s3">
                                        <div class="flex">

                                            <div>

                                                <select multiple ng-model="selectedWareHouses" style="width: 60%;" size="7">
                                                    <option ng-repeat="category in warehouses" value="{{category.ID}}" ng-selected="{{selectedValues.indexOf(category.ID.toString())!=-1}}">{{category.Name}}</option>
                                                </select>
                                                <label> <%= GetGlobalResourceObject("Resource", "WareHouses")%></label>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <br />


                            </div>
                            <div style="float: right;">
                                <button type="button" ng-click="CreateNewUser()" class="addbuttonOutbound btn btn-primary">   <%= GetGlobalResourceObject("Resource", "CreateUser")%> <%=MRLWMSC21Common.CommonLogic.btnfaNew %></button>
                            </div>


                        </div>
                        <%-- ng-cloak ng-if="UserList"--%>
                        <div ng-cloak ng-if="UserList">
                            <br />
                            <%--    <div class="flex__ right">
                                                   
                                                             &nbsp;&nbsp;<button type="button"  ng-click="dispalynewwareUser()"   class="addbuttonOutbound btn btn-primary">Create New <%=MRLWMSC21Common.CommonLogic.btnfaNew %></button>
                                                 
                                                            </div>--%>

                            <table class="table-striped ">
                                <tr>
                                    <th> <%= GetGlobalResourceObject("Resource", "UserId")%></th>
                                    <th>  <%= GetGlobalResourceObject("Resource", "UserType")%>  </th>
                                    <th><%= GetGlobalResourceObject("Resource", "Account")%> </th>
                                    <th><%= GetGlobalResourceObject("Resource", "Tenant")%> </th>
                                    <th> <%= GetGlobalResourceObject("Resource", "FullName")%> </th>
                                    <th> <%= GetGlobalResourceObject("Resource", "EmployeeCode")%> </th>
                                    <th>  <%= GetGlobalResourceObject("Resource", "UserRoles")%> </th>
                                    <th> <%= GetGlobalResourceObject("Resource", "WHCode")%> </th>
                                    <%-- <th>Work Stations</th>--%>
                                    <th> <%= GetGlobalResourceObject("Resource", "Active")%> </th>
                                    <%--  <th>Edit</th>--%>
                                </tr>


                                <tr dir-paginate="info in UsersList |itemsPerPage:10" pagination-id="nonAvaible">
                                    <td>{{info.UserID}}</td>
                                    <td>{{info.UserType}}</td>
                                    <td>{{info.Account}}</td>
                                    <td>{{info.TenantName}}</td>
                                    <td>{{info.FullName}}</td>
                                    <td>{{info.EmployeeCode}}</td>
                                    <td>{{info.UserRoles}}</td>
                                    <td>{{info.WHCode}}</td>
                                    <%-- <td>{{info.WorkCenters}}</td>--%>
                                    <td>{{info.IsActive}}</td>

                                    <%--  <td><div style="cursor:pointer;" ng-click="getUserEditData(info)">Edit</div></a> </td>--%>
                                </tr>



                            </table>
                            <div style="float: right !important; font-family: Arial; font-size: small; margin-right: 1%;">
                                <dir-pagination-controls class="getPageId" direction-links="true" pagination-id="nonAvaible" boundary-links="true"> </dir-pagination-controls>
                            </div>
                            <br />
                            <br />
                        </div>

                        <br />
                        <br />
                    </div>
                    <%--<iframe id="userFrame" runat="server"  style="width:100%;height:400px;">
                                                        alternative content for browsers which do not support iframe.
                                                </iframe>--%>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <input type="hidden" value="0" id="TenantWarehouseID" />
                <div id="divPreferences" style="display: none;"></div>
            </td>
        </tr>
        <tr>
            <td style="display: none">
                <div class="ui-SubHeading ui-SubHeadingBar" id="TenantWHHeader">  <%= GetGlobalResourceObject("Resource", "TenantWarehouses")%> <span class="ui-icon"></span></div>

                <div class="ui-Customaccordion" id="TenantWHBody">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" Visible="true" RenderMode="Inline">
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="WHChkBoxList" CssClass="mspCheckBox" runat="server" RepeatColumns="1" RepeatDirection="Horizontal">
                                        </asp:CheckBoxList>
                                    </td>

                                </tr>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="lnksave" runat="server" CssClass="btn btn-primary" OnClick="lnksave_Click" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <%-- <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlBillingCountry" EventName="SelectedIndexChanged" />
                            </Triggers>--%>
                    </asp:UpdatePanel>
                </div>
            </td>
        </tr>


        <tr style="display: none">
            <td colspan="4">

                <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="divFileUploadHeader">  <%= GetGlobalResourceObject("Resource", "Attachments")%></div>

                <div class="ui-Customaccordion" id="divFileUploadBody">


                    <table width="100%" class="">


                        <tr>

                            <td colspan="4">
                                <%--    <div class="row">
                                            <div class="col-lg-12">
                                                <form action="#" class="dropzone" id="dropzoneForm">
                                                    <div class="fallback">
                                                        <input name="file" type="file" multiple />
                                                    </div>
                                                </form>
                                            </div>
                                        </div>


                                        <div class="row">
                                            <br />
                                            <div class="col-lg-4 text-left">
                                           <input type="hidden" id="GEN_MST_EntityAttachment_ID" value="0" />
                                                <button type="button" class="btn btn-primary" id="getFiles" onclick="UploadFiles();">Start Upload</button>
                                                 <button type="button" class="btn btn-primary" id="btnReset" onclick="clearDropZone()">Clear</button>
                                            </div>                               
                                            </div>--%>




                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upTenantattachment" RenderMode="Inline">
                                    <%--   <Triggers>
                                    <asp:PostBackTrigger ControlID="ddldocumenttype"  />
            <asp:PostBackTrigger ControlID="btnupload"></asp:PostBackTrigger>
                                </Triggers>--%>
                                    <ContentTemplate>
                                        <div runat="server" id="Atachment">

                                            <table border="0" width="100%" cellpadding="2" cellspacing="2">

                                                <tr>

                                                    <%--   <td class="FormLabels" valign="top">Attachment Type:<br />
                                                    <asp:DropDownList ID="ddldocumenttype" runat="server" AutoPostBack="false"></asp:DropDownList>
                                                </td>--%>
                                                    <td class="FormLabels" valign="top" width="48%" style="display: none">
                                                        <%-- <div id="fileuploader">Upload</div>  --%>
                                                        <br />
                                                        <asp:FileUpload ID="FUAttachment" runat="server" Width="100" />
                                                        <asp:TextBox runat="server" ID="txtimage" Visible="false"></asp:TextBox>
                                                        <%--  <input type="button" id="startUpload" class="btn btn-primary" style="padding-right:35px;margin-left:0px" value="Start Upload" /><span style="margin-left: -29px;color:white" class="fa fa-upload"></span>
                                                        --%>
                                                        <asp:Button ID="btnupload" runat="server" Text="Start Upload" class="btn btn-primary" Style="padding-right: 35px; margin-left: 0px" OnClick="btnupload_Click1" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnupload" />
                                    </Triggers>
                                </asp:UpdatePanel>


                            </td>
                        </tr>

                    </table>

                </div>
            </td>
        </tr>



        <tr>
            <td>
                <div class="flex__ checkbox">
                    <asp:CheckBox ID="chkIsActive" runat="server" Checked />
                    <label>&nbsp;&nbsp;&nbsp;  <%= GetGlobalResourceObject("Resource", "Active")%></label>
                </div>
            </td>
        </tr>
        <tr>
            <td align="right">
                <div align="right">
                    <asp:LinkButton ID="lnkCancel" runat="server" OnClick="lnkCancel_Click" CssClass="btn btn-primary">   <%= GetGlobalResourceObject("Resource", "Cancel")%><%=MRLWMSC21Common.CommonLogic.btnfaClear %></asp:LinkButton>
                    <asp:LinkButton ID="lnkSavePrimaryInfo" runat="server" CssClass="btn btn-primary" OnClick="lnkSavePrimaryInfo_Click" ValidationGroup="vgTenantRegistration" />

                </div>
            </td>
        </tr>
    </table>
        </div>

    <asp:HiddenField ID="hifTenantID" runat="server" />

    <%--            </ContentTemplate>
        </asp:UpdatePanel>--%>

    <script type="text/javascript">
        var WHList = null;
        var TenantWHID = 0;
        MasterID = new URL(window.location.href).searchParams.get("tid");
        // alert(MasterID);
        if (MasterID != null) {

            $("#divPreferences").css("display", "block");
        }
        function GetWHList() {

            $.ajax({
                url: "TenantRegistration.aspx/GetWarehouse",
              //  url: '<%=ResolveUrl("TenantRegistration.aspx/GetWarehouse") %>',
                //data: "{ 'prefix': '" + request.term + "'}",
                data: "{}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (response) {
                    var dt = JSON.parse(response.d);
                    WHList = dt;
                    GetWarehouses();

                    BindWH(MasterID);
                },
                error: function (response) {

                },
                failure: function (response) {

                }
            });
        }

        // GetWHList();

        function GetWarehouses() {
            var displayid = "";
            displaypreferenceid = "";
            var PreferenceContainer = document.getElementById('divPreferences');
            var PreferenceContent = '';
            var tenantWHID = 0;
            if (WHList.Table != null && WHList.Table.length > 0) {

                //var GroupList = $.grep(WHList.Table, function (a) { return a.GroupName == "Material Group" });
                var warehouse = "Tenant Warehouses";

                PreferenceContent += '<div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="divPreferenceHeader">' + warehouse + '</div><div class="ui-Customaccordion" id="divPreferenceBody" style="text-align:left;">';
                var whs = WHList.Table;
                for (var j = 0; j < whs.length; j++) {
                    PreferenceContent += '<input type="checkbox" class="GetWHOptions" id="' + whs[j].WarehouseID + '" data-attr="0"/> ' + whs[j].WHCode + '<br>';
                    // PreferenceContent += '<input type="hidden" class="GetWHOptions" id="ChkConfigure'+ j +'" value="0"/>';
                }


            }


            PreferenceContent += '<div style="text-align:right;padding:5px 23px; overflow:hidden; padding-right: 0;"><button type="button" class="ui-btn ui-button-large" onclick="UpsertWHS()">Update<i class="space fa fa-database"></i></button></div>';

            PreferenceContainer.innerHTML = PreferenceContent;

            CustomAccordino($('#divPreferenceHeader'));
        }


        function BindWH(Tenantid) {

            var WHData = $.grep(WHList.Table1, function (a) { return a.TenantID == Tenantid });
            FillWHData(WHData);
        }

        function FillWHData(obj) {


            if (obj != null && obj.length > 0) {
                for (var i = 0; i < obj.length; i++) {

                    $('#' + obj[i].WarehouseID).prop("checked", true);
                    $('#' + obj[i].WarehouseID).attr("data-attr", obj[i].TenantWarehouseID);
                    // $('#ChkConfigure' + i).text(obj[i].TenantWarehouseID);
                }
            }
        }

        function GetWHData() {

            // var fieldDataOut = '{';
            var fieldData = '<root>';
            $(".GetWHOptions").each(function () {

                var param = $(this).attr('id');
                var val = $(this).val().trim();
                var keyid = $(this).attr("data-attr");
                var paramtype = $(this).attr('type');
                if (paramtype == "checkbox") {
                    val = $(this).prop('checked');
                    if (val == true) {
                        // var warehouseid = $('#' + obj[i].WarehouseID).attr('id');
                        fieldData += '<data>';
                        fieldData += '<TenantID>' + MasterID + '</TenantID>';
                        fieldData += '<WarehouseID>' + param + '</WarehouseID>';
                        fieldData += '<IsActive>' + 1 + '</IsActive>';
                        fieldData += '<IsDeleted>' + 0 + '</IsDeleted>';
                        fieldData += '<TenantWarehouseID>' + keyid + '</TenantWarehouseID></data>';

                    }
                    else {
                        fieldData += '<data>';
                        fieldData += '<TenantID>' + MasterID + '</TenantID>';
                        fieldData += '<WarehouseID>' + param + '</WarehouseID>';
                        fieldData += '<IsActive>' + 0 + '</IsActive>';
                        fieldData += '<IsDeleted>' + 1 + '</IsDeleted>';
                        fieldData += '<TenantWarehouseID>' + keyid + '</TenantWarehouseID></data>';
                    }

                }





                //var obj = WHList.Table;
                //for (var i = 0; i < obj.length; i++) {
                //  
                //    var tenatWHID = $('#ChkConfigure' + i).text();
                //    if (tenatWHID == "") {
                //        tenatWHID = 0;
                //    }
                //    if ($('#' + obj[i].WarehouseID).prop('checked') == true) {

                //        var warehouseid = $('#' + obj[i].WarehouseID).attr('id');

                //        fieldData += '<data>';
                //        fieldData += '<TenantID>' + MasterID + '</TenantID>';
                //        fieldData += '<WarehouseID>' + warehouseid + '</WarehouseID>';
                //        fieldData += '<TenantWarehouseID>' + tenatWHID + '</TenantWarehouseID></data>';
                //    }

                //    else {
                //        var warehouseid = $('#' + obj[i].WarehouseID).attr('id');

                //        fieldData += '<data>';
                //        fieldData += '<TenantID>' + MasterID + '</TenantID>';
                //        fieldData += '<WarehouseID>' + warehouseid + '</WarehouseID>';
                //        fieldData += '<TenantWarehouseID>' + tenatWHID + '</TenantWarehouseID></data>';
                //    }

                //}


                //fieldDataOut += '"' + String.fromCharCode(64) + 'inputDataXml' + '":"' + fieldData + '",' + '"' + String.fromCharCode(64) + 'LanguageType' + '":"' + 'en' + '",' + '"' + String.fromCharCode(64) + 'LoggedUserID' + '":"' + $("#hdnUpdatedBy").val() + '",';
                // fieldDataOut = fieldDataOut.substring(0, fieldDataOut.length - 1);
                // fieldDataOut += '}';
                //return fieldDataOut;

            });
            fieldData = fieldData + '</root>';
            return fieldData;
        }

        function InsertPreference() {
            var status = false, value;
            $(".GetWHOptions").each(function () {
                var param = $(this).attr('id');
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

        function UpsertWHS() {
            var data = GetWHData();
            var obj = {};
            obj.UserID = "<%=cp.UserID.ToString()%>";
            obj.Inxml = GetWHData();
            $.ajax({
                url: "TenantRegistration.aspx/SETWarehouses",
                dataType: 'json',
                contentType: "application/json",
                type: 'POST',
                data: JSON.stringify(obj),
                success: function (response) {
                    if (response.d == "success") {
                        showStickyToast(true, 'Tenant Warehouses Saved Successfully ');
                        //alert("Saved Successfully");
                        //location.reload();
                        //GetPreferencesList();
                    }
                }
            });
        }
    </script>

    <style>
        .ui-autocomplete-input {
            --md-arrow-width: 1em;
            background: url(../Images/magnifier.svg) calc(100% - var(--md-arrow-offset) - var(--md-select-side-padding)) center no-repeat !important;
            background-size: var(--md-arrow-width) !important;
        }

        .gridInput .errorMsg{
            bottom:5px;
        }

        .excessshow label span {
            display:inline-block !important;
        }

        .gvLightSteelBlueNew_DataCellGridEdit a.ButEmpty{
            box-shadow:var(--z1);
            font-size:12px;
            background-color:var(--sideNav-bg);
            color:#fff;
            padding:3px 5px;
        }
    </style>
    <br />
    <br />
</asp:Content>
