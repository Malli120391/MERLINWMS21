<%@ Page Title="Dispatch List" Language="C#" MasterPageFile="~/mOutbound/OutboundMaster.master" AutoEventWireup="true" CodeBehind="DispatchList.aspx.cs" Inherits="MRLWMSC21.mOutbound.DispatchList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="OBContent" runat="server">
    <script src="../mInventory/Scripts/angular.min.js"></script>
    <script src="../mInventory/Scripts/dirPagination.js"></script>
    <style>
        .inline_text {
            border: 1px solid #ccc;
            height: 20px;
            width: 30%;
            padding-left: 10px
        }

            .inline_text:focus {
                border: 1px solid #ccc !important;
            }

        .btn {
            min-width: unset !important
        }
    </style>
    <script>  
        var OutboundID = 0;
        if (location.href.indexOf('?obdid=') > 0) {
            OutboundID = location.href.split('?obdid=')[1];
        }

        $(document).ready(function () {
            debugger;
            var hid = new URL(window.location.href).searchParams.get("obdid");
            function bindDetails(hid) {
                debugger;

                if (hid != "0") {
                    $.ajax({
                        url: "DispatchList.aspx/GetMaterialDetailsData",
                        dataType: 'json',
                        contentType: "application/json",
                        type: 'POST',
                        data: "{ 'obdid': '" + hid + "'}",
                        success: function (response) {
                            debugger
                            var dt = JSON.parse(response.d);
                            debugger
                            console.log(dt);
                            $(window).ready(function () {
                                $("#txtTenant").val(dt.Table[0].TENANTNAME);
                                $("#txtCustomer").val(dt.Table[0].CUSTOMERNAME);
                                $("#IndentNumber").val(dt.Table[0].CustPONumber);
                                $("#BillingAddress").val(dt.Table[0].ShipmentAddress1);
                                $("#MDescription").val(dt.Table[0].MDescription);
                                $("#Quantity").val(dt.Table[0].Quantity);
                            })
                        }
                    });
                }
            }

            if (hid != null) {
                debugger;

                bindDetails(hid);

            }
            else {
                hid = "0";
            }

            if (location.href.indexOf('?obdid=') > 0) {
                $("#save").hide();
                $("#ADDSKU").hide();
            }
        });



        var app = angular.module('DispatchList', []);
        app.controller('DispatchListController', function ($scope, $http) {

            //=================== Tenant dropdown ================================//
            debugger;
            $scope.DispatchList = [];
            if (sessionStorage.getItem('FianlDispatchList') != null) {
                    $scope.DispatchList = JSON.parse(sessionStorage.getItem('FianlDispatchList'));
            }
            
            $scope.tenantid = 0;
            $('#txtTenant').val("");
            var textfieldname = $("#txtTenant");
            DropdownFunction(textfieldname);
            $("#txtTenant").autocomplete({
                source: function (request, response) {
                    if ($("#txtTenant").val() == '') {
                        $scope.tenantid = 0;
                    }
                    debugger;
                    $.ajax({
                        url: '../mWebServices/FalconWebService.asmx/LoadTenantDataFor3PL',
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
                    debugger
                    $scope.tenantid = i.item.val;
                },
                minLength: 0
            });

            //=================== Material Description dropdown ================================//               

            debugger
            $("#MDescription").val("");
            var TextFieldName = $("#MDescription");
            DropdownFunction(TextFieldName);
            $("#MDescription").autocomplete({
                source: function (request, response) {
                    if ($scope.tenantid == 0 || $scope.tenantid == "0" || $scope.tenantid == undefined || $scope.tenantid == null) {
                        showStickyToast(false, 'Please select Tenant');
                        return false;
                    }
                    debugger
                    $.ajax({
                        url: '../mWebServices/FalconWebService.asmx/LoadMDesData',
                        data: "{ 'prefix': '" + request.term + "','tenantid':" + $scope.tenantid + "}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            debugger
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
                    debugger
                    MDescription = i.item.label;
                    //MaterialMasterID = i.item.val;
                    $("#MaterialMasterID").val(i.item.val);
                },
                minLength: 0
            });


            var textfieldname = $("#txtCustomer");
            DropdownFunction(textfieldname);
            $("#txtCustomer").autocomplete({
                source: function (request, response) {
                    debugger;
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadCustomerNames") %>',
                        data: "{ 'prefix': '" + request.term + "','TenantID':'" + $scope.tenantid + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            if (data.d == '')
                                alert('No customer is available');

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

                    $("#hifCustomerName").val(i.item.val);
                },
                minLength: 0
            });


            /////////// store DispatchList data in session ////////////////////

            debugger
            
            if ($scope.DispatchList_Session) {
                for (var i = 0; i < $scope.DispatchList_Session.length; i++) {
                    $scope.DispatchList.push(
                        {
                            TenantID: $scope.DispatchList_Session[i].TenantID,
                            txtTenant: $scope.DispatchList_Session[i].txtTenant,
                            txtCustomer: $scope.DispatchList_Session[i].txtCustomer,
                            IndentNumber: $scope.DispatchList_Session[i].IndentNumber,
                            BillingAddress: $scope.DispatchList_Session[i].BillingAddress,
                            MaterialMasterID: $scope.DispatchList_Session[i].MaterialMasterID,
                            MDescription: $scope.DispatchList_Session[i].MDescription,
                            Quantity: $scope.DispatchList_Session[i].Quantity,
                            FreeQuantity: $scope.DispatchList_Session[i].FreeQuantity,
                            TotalQty: $scope.DispatchList_Session[i].TotalQty,
                        }
                    )
                }
            }

           
            $scope.AddSKU = function () {
                debugger
                var txtTenant = $("#txtTenant").val();
                if ($scope.tenantid == "" || $scope.tenantid == null || $scope.tenantid == undefined) {
                    $scope.tenantid = 0;
                }
                $scope.txtTenant = $("#txtTenant").val();
                if (txtTenant == undefined || txtTenant == null || txtTenant == "") {
                    showStickyToast(false, "Select Tenant", false);
                    return false;
                }

                $scope.txtCustomer = $("#txtCustomer").val();
                if (txtCustomer == undefined || txtCustomer == null || txtCustomer == "") {
                    showStickyToast(false, "Select Customer", false);
                    return false;
                }
                $scope.IndentNumber = $("#IndentNumber").val();
                if ($scope.IndentNumber == undefined || $scope.IndentNumber == null || $scope.IndentNumber == "") {
                    showStickyToast(false, "Select Indent Number", false);
                    return false;
                }
                $scope.BillingAddress = $("#BillingAddress").val();

                var MDescription = $("#MDescription").val();
                $scope.MaterialMasterID = $("#MaterialMasterID").val();
                if (MDescription == undefined || MDescription == null || MDescription == "") {
                    showStickyToast(false, "Select Material Description", false);
                    return false;
                }
                if ($scope.Quantity == undefined || $scope.Quantity == "" || $scope.Quantity == null) {
                    showStickyToast(false, "Please Enter Quantity", false);
                    return false;
                }              
                if ($scope.FreeQuantity == undefined || $scope.FreeQuantity == "" || $scope.FreeQuantity == null) {
                    $scope.FreeQuantity = 0;
                }
                $scope.TotalQty = $scope.Quantity + $scope.FreeQuantity;

                $scope.DispatchList.push(
                    {
                        TenantID: $scope.tenantid,
                        txtTenant: $scope.txtTenant,
                        txtCustomer: $scope.txtCustomer,
                        IndentNumber: $scope.IndentNumber,
                        BillingAddress: $scope.BillingAddress,
                        MDescription: MDescription,
                        MaterialMasterID: $scope.MaterialMasterID,
                        Quantity: $scope.Quantity,
                        FreeQuantity: $scope.FreeQuantity,  
                        TotalQty: $scope.TotalQty
                    }

                )
               
                $("#MDescription").val("");
                $("#Quantity").val("");
                $scope.FreeQuantity = "";

                return false;
            }


            $scope.DispatchPGI = function () {
                debugger;
                var txtTenant = $("#txtTenant").val();
                if ($scope.tenantid == "" || $scope.tenantid == null || $scope.tenantid == undefined) {
                    $scope.tenantid = 0;
                }
                $scope.txtTenant = $("#txtTenant").val();
                if (txtTenant == undefined || txtTenant == null || txtTenant == "") {
                    showStickyToast(false, "Select Tenant", false);
                    return false;
                }

                $scope.txtCustomer = $("#txtCustomer").val();
                if ($scope.txtCustomer == undefined || $scope.txtCustomer == null || $scope.txtCustomer == "") {
                    showStickyToast(false, "Select Customer", false);
                    return false;
                }
                $scope.IndentNumber = $("#IndentNumber").val();
                if ($scope.IndentNumber == undefined || $scope.IndentNumber == null || $scope.IndentNumber == "") {
                    showStickyToast(false, "Select Indent Number", false);
                    return false;
                }
                $scope.BillingAddress = $("#BillingAddress").val();
                //var MDescription = $("#MDescription").val();
                $scope.MaterialMasterID = $("#MaterialMasterID").val();
                //if (MDescription == undefined || MDescription == null || MDescription == "") {
                //    showStickyToast(false, "Select Material Description", false);
                //    return false;
                //}
                //if ($scope.Quantity == undefined || $scope.Quantity == "" || $scope.Quantity == null) {
                //    showStickyToast(false, "Please Enter Quantity", false);
                //    return false;
                //}
                //if ($scope.FreeQuantity == undefined || $scope.FreeQuantity == "" || $scope.FreeQuantity == null) {
                //    showStickyToast(false, "Please Enter Free Quantity", false);
                //    return false;
                //}
                //sessionStorage.setItem('DispatchList', JSON.stringify($scope.DispatchList));
                $scope.FianlDispatchList = [];
                for (var i = 0; i < $scope.DispatchList.length; i++) {
                    $scope.FianlDispatchList.push({
                        TenantID: $scope.DispatchList[i].TenantID,
                        txtTenant: $scope.DispatchList[i].txtTenant,
                        txtCustomer: $scope.DispatchList[i].txtCustomer,
                        IndentNumber: $scope.DispatchList[i].IndentNumber,
                        BillingAddress: $scope.DispatchList[i].BillingAddress,
                        MaterialMasterID: $scope.DispatchList[i].MaterialMasterID,
                        MDescription: $scope.DispatchList[i].MDescription,
                        Quantity: $scope.DispatchList[i].Quantity,
                        FreeQuantity: $scope.DispatchList[i].FreeQuantity,
                        TotalQty: $scope.DispatchList[i].TotalQty
                    })
                }
                sessionStorage.setItem('FianlDispatchList', JSON.stringify($scope.FianlDispatchList));
                var http = {
                    method: 'POST',
                    url: 'DispatchList.aspx/DispatchListAdd',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { '_Obj': JSON.stringify($scope.FianlDispatchList) }
                }
                $http(http).success(function (response) {
                    debugger
                    if (response.d != 0) {
                        $scope.Obd_Data = JSON.parse(response.d).Table;
                        OBDid = $scope.Obd_Data[0].OutboundID;
                        OBDNumber = $scope.Obd_Data[0].OBDNumber;
                        showStickyToast(true, "Successfully Created" + "" + "OBDNumber - " + OBDNumber);
                        //sessionStorage.removeItem("DispatchList");
                        $("#save").hide();
                        $("#ADDSKU").hide();
                        window.open("../mOutbound/DispatchList.aspx?obdid=" + OBDid, "_top");
                        


                    }
                    else {
                        alert("Failed Insertion");
                    }

                });

            }

            $scope.ReleaseDispatch = function () {
                debugger;
                //alert(OutboundID);
                var obdiddispatch = OutboundID;
                var billing = {
                    method: 'POST',
                    url: 'DispatchList.aspx/BulkRelease',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'outboundid': obdiddispatch, 'DockID': 3 },
                }
                $http(billing).success(function (response) {
                    debugger;
                    if (JSON.parse(response.d).Table[0].Status == 1) {
                        $("#ReleaseNpick").hide();
                        showStickyToast(true, 'Successfully Updated', false);
                    }
                    else {
                        showStickyToast(true, 'Error while releasing outbound', false);
                    }
                }).catch(function (reason) {
                    debugger;
                    showStickyToast(false, 'Unexpected error, please contact support');
                    console.log(reason);
                    $scope.blockUI = false;
                    $('inv-preloader').hide();
                });
            }

            $scope.PGIComplete = function () {
                debugger;
                //alert(OutboundID);
                var OBD = OutboundID;
                var billing = {
                    method: 'POST',
                    url: 'DispatchList.aspx/PGI',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'OutboundID': OBD },
                }
                $http(billing).success(function (response) {
                    debugger;
                    if (JSON.parse(response.d).Table[0].N == 1) {
                        $("#PGIDONE").hide();
                        showStickyToast(true, 'Successfully Updated', false);
                    }
                    else {
                        showStickyToast(true, 'Error while Proceeding PGI', false);
                    }
                }).catch(function (reason) {
                    debugger;
                    showStickyToast(false, 'Unexpected error, please contact support');
                    console.log(reason);
                    $scope.blockUI = false;
                    $('inv-preloader').hide();
                });
            }

            $scope.delete = function (index) {
                debugger;
                if (confirm("Are you sure do you want to Delete?")) {
                    sessionStorage.removeItem('DispatchList');
                    for (var i = 0; i < $scope.DispatchList.length; i++) {
                        if (index == i) {
                            $scope.DispatchList.splice(i, 1);
                            break;
                        }
                    }
                    //sessionStorage.setItem('DispatchList', JSON.stringify($scope.DispatchList));
                }
            }
            $scope.CreateOutbound = function () {
                debugger;
                if ($scope.obdbdata == undefined) {
                    showStickyToast(false, "Please upload Excel Sheet");
                    return false;
                }
                var http = {
                    method: 'POST',
                    url: '../mOutbound/ObdImports.aspx/GetOutwardImport',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'obdlst': $scope.obdbdata }
                }
                $http(http).success(function (response) {
                    //
                    debugger;
                    if (response.d.indexOf('Error') != -1) {
                        showStickyToast(false, response.d);

                        return false;
                    }
                    else {
                        showStickyToast(true, response.d);
                        $scope.obdbdata = null;
                        $scope.excel = null;
                        $("#fuExcel").val(null);
                        $("#filetype").val(null);
                        $scope.excel = null;
                        return false;
                    }
                });
            }

        });
    </script>
    <div class="container" ng-app="DispatchList" ng-controller="DispatchListController">
        <div class="panel-group" id="accordion">
            <div class="panel panel-default panelborder" id="panel1">
                <div class="panel-heading accordpanel">
                    <a class="accord collapsed" data-toggle="collapse" data-target="#collapseOne">Dispatch Form</a>
                </div>
                <div id="collapseOne" class="panel-collapse collapse in">
                    <div class="panel-body" style="border-top-color: #fac18a !important;">
                        <br />
                        <div class="row">
                            <div class="col m3 s4">
                                <div class="flex">
                                    <asp:HiddenField runat="server" ID="hifTenant" Value="0" ClientIDMode="Static" />
                                    <input type="text" id="txtTenant" required="" />
                                    <label>Tenant</label>
                                    <span class="errorMsg"></span>
                                </div>
                            </div>
                            <div class="col m3 s4">
                                <div class="flex">
                                    <asp:HiddenField runat="server" ID="hifCustomerName" Value="0" ClientIDMode="Static" />
                                    <asp:DropDownList Style="display: none;" runat="server" CssClass="NoPrint" required="" />
                                    <input type="text" id="txtCustomer" required="" ng-model="CustomerName" />
                                    <label>Customer Name</label>
                                    <span class="errorMsg"></span>

                                </div>
                            </div>
                            <div class="col m3 s4">
                                <div class="flex">
                                    <asp:HiddenField runat="server" ID="hifIndentumber" Value="0" ClientIDMode="Static" />
                                    <input type="text" id="IndentNumber" required="" ng-model="IndentNumber" />
                                    <label>Indent Number</label>
                                    <span class="errorMsg"></span>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col m6 s4">
                                <div class="flex">
                                    <asp:HiddenField runat="server" ID="hdnBillingAddress" Value="0" ClientIDMode="Static" />
                                    <input type="text" id="BillingAddress" required="" />
                                    <label>Billing address</label>
                                </div>
                            </div>
                            <div class="col m3 s4 offset-m3">
                                <div class="flex">
                                    <%--<input type="button" class="btn btn-sm btn-primary" style="float: right" ng-click="AddDispatch()" value="Add Dispatch" />--%>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <div class="panel-group" id="accordion">
            <div class="panel panel-default panelborder" id="panel2">
                <div class="panel-heading accordpanel">
                    <a class="accord collapsed" data-toggle="collapse" data-target="#collapseTwo">Dispatch List</a>
                </div>
                <div id="collapseTwo" class="panel-collapse collapse in">
                    <div class="panel-body" style="border-top-color: #fac18a !important;">
                        <br />
                        <div align="center" class="">
                            <form autocomplete="off">
                                <div class="row" style="margin-bottom: 0px !important">
                                    <div class="col m3 s4">
                                        <div class="flex">
                                            <asp:HiddenField runat="server" ID="MaterialMasterID" Value="0" ClientIDMode="Static" />
                                            <asp:DropDownList Style="display: none;" runat="server" CssClass="NoPrint" required="" />
                                            <input type="text" id="MDescription" runat="server" skinid="txt_Hidden_Req_Auto" required="" clientidmode="Static" />
                                            <label>Material Description</label>
                                            <span class="errorMsg"></span>
                                        </div>


                                        &emsp;
                                    </div>
                                    <div class="col m3 s4">
                                        <div class="flex">
                                            <asp:HiddenField runat="server" ID="hdnQuantity" Value="0" ClientIDMode="Static" />
                                            <input type="number" id="Quantity" required="" ng-model="Quantity" autocomplete="off" />
                                            <label>Quantity</label>
                                            <span class="errorMsg"></span>
                                        </div>
                                        &emsp;
                                    </div>
                                    <div class="col m3 s4">
                                        <div class="flex">
                                            <asp:HiddenField runat="server" ID="hdnFreeQuantity" Value="0" ClientIDMode="Static" />
                                            <input type="number" id="FreeQuantity" required="" ng-model="FreeQuantity" autocomplete="off" />
                                            <label>Free Quantity</label>
                                            <span class="errorMsg"></span>
                                        </div>
                                        &emsp;
                                    </div>
                                    <div class="col m3 s4">
                                        <div class="flex" style="float: right">
                                            <input type="button" id="ADDSKU" class="btn btn-sm btn-primary" ng-click="AddSKU()" value="Add SKU" />
                                        </div>
                                        &emsp;
                                    </div>
                                </div>
                            </form>
                        </div>
                        <div class="row">
                            <div class="col m12">
                                <table class="table-striped">
                                    <thead>
                                        <tr>
                                            <th>S.No</th>
                                            <th>Material Description</th>
                                            <th>Qty</th>
                                            <th>Free Qty</th>
                                            <th>Total Qty</th>
                                            <th>Action</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr ng-repeat="x in DispatchList">
                                            <td>{{$index+1}}</td>
                                            <td>{{x.MDescription}}</td>
                                            <td>{{x.Quantity}}</td>
                                            <td>{{x.FreeQuantity}}</td>
                                            <td>{{x.TotalQty}}</td>
                                            <td><a><i class="material-icons vl" ng-click="delete($index)">delete</i></a></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col m3 s4 offset-m9">
                                <div class="flex" style="text-align: right">
                                    <input type="button" id="save" class="btn btn-sm btn-primary" style="text-align: center" ng-click="DispatchPGI();" value="Save" />
                                    <input type="button" id="ReleaseNpick" class="btn btn-sm btn-primary" style="text-align: center" ng-click="ReleaseDispatch();" value="Release and Pick" />
                                    <input type="button" id="PGIDONE" class="btn btn-sm btn-primary" style="text-align: center" ng-click="PGIComplete()" value="PGI" />
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
