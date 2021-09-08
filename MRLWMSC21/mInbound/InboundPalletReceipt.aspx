<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InboundPalletReceipt.aspx.cs" Inherits="MRLWMSC21.mInbound.InboundPalletReceipt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />
    <script src="../mInventory/Scripts/angular.min.js"></script>
    <script src="../mInventory/Scripts/dirPagination.js"></script>

    <style>
        .btn {
            cursor: pointer !important
        }
    </style>

    <script>
      
        var app = angular.module('InboundPalletReceipt', ['angularUtils.directives.dirPagination']);
        app.controller('InboundPalletReceiptController', function ($scope, $http) {

            //=================== Tenant dropdown ================================//
            debugger;
            $scope.tenantid = 0;
            $('#txtTenant').val("");
            var textfieldname = $("#txtTenant");
            DropdownFunction(textfieldname);
            $("#txtTenant").autocomplete({
                source: function (request, response) {
                    if ($("#txtTenant").val() == '') {
                        $scope.tenantid = 0;
                    }

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
                    txtTenant = i.item.label;
                    TenantID = i.item.val;
                
                },
                minLength: 0
            })
         
            //=================== Material Description dropdown ================================//               
            
            debugger
            $("#MDescription").val("");
            var TextFieldName = $("#MDescription");
            DropdownFunction(TextFieldName);
            $("#MDescription").autocomplete({
                source: function (request, response) {
                    if (TenantID == 0 || TenantID == "0" || TenantID == undefined || TenantID == null) {
                        showStickyToast(false, 'Please select Tenant');
                        return false;
                    }
                    debugger
                    $.ajax({
                        url: '../mWebServices/FalconWebService.asmx/LoadMDesData',
                        data: "{ 'prefix': '" + request.term + "','tenantid':" + TenantID + "}",
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
                    MaterialMasterID = i.item.val;
                },
                minLength: 0
            });

             $('#MFGDate').datepicker({
                dateFormat: "dd-MM-yy",
            });


            /////////// store InboundPalletReceiptList data in session ////////////////////
            
            debugger
          
            $scope.InboundPalletReceiptList = [];
            $scope.InboundPalletReceiptList_Session = JSON.parse(sessionStorage.getItem('InboundPalletReceiptList'));
            if ($scope.InboundPalletReceiptList_Session) {
                for (var i = 0; i < $scope.InboundPalletReceiptList_Session.length; i++) {
                    $scope.InboundPalletReceiptList.push(
                        {
                            TenantID: $scope.InboundPalletReceiptList_Session[i].TenantID,
                            txtTenant: $scope.InboundPalletReceiptList_Session[i].txtTenant,
                            MaterialMasterID: $scope.InboundPalletReceiptList_Session[i].MaterialMasterID,
                            MDescription: $scope.InboundPalletReceiptList_Session[i].MDescription,
                            PalletQuantity: $scope.InboundPalletReceiptList_Session[i].PalletQuantity,
                            MFGDate: $scope.InboundPalletReceiptList_Session[i].MFGDate,
                            NoOfPallets: $scope.InboundPalletReceiptList_Session[i].NoOfPallets,
                        }
                    )
                }
            }


            /////////// add sku data in InboundPalletReceiptList  ////////////////////

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
                var MDescription = $("#MDescription").val();
                $scope.MaterialMasterID = $("#hifWareHouse").val();
                if (MDescription == undefined || MDescription == null || MDescription == "") {
                    showStickyToast(false, "Select Material Description", false);
                    return false;
                }
                if ($scope.PalletQuantity == undefined || $scope.PalletQuantity == "" || $scope.PalletQuantity == null) {
                    showStickyToast(false, "Please Enter Pallet Quantity", false);
                    return false;
                }
                if ($scope.MFGDate == undefined || $scope.MFGDate == "" || $scope.MFGDate == null) {
                    showStickyToast(false, "Select MFG Date", false);
                    return false;
                }
                if ($scope.NoOfPallets == undefined || $scope.NoOfPallets == "" || $scope.NoOfPallets == null) {
                    showStickyToast(false, "Please Enter No Of Pallets", false);
                    return false;
                }
                debugger
                for (var i = 0; i < $scope.InboundPalletReceiptList.length; i++) {
                    if ($scope.TenantID == $scope.InboundPalletReceiptList[i].tenantid && $scope.MaterialMasterID == $scope.InboundPalletReceiptList[i].MaterialMasterID && $scope.PalletQuantity == $scope.InboundPalletReceiptList[i].PalletQuantity && $scope.MFGDate == $scope.InboundPalletReceiptList[i].MFGDate && $scope.NoOfPallets == $scope.InboundPalletReceiptList[i].NoOfPallets) {
                        showStickyToast(false, "Duplicate Record Found", false);
                        return false;
                    }
                }
                $scope.InboundPalletReceiptList.push(
                    { TenantID: TenantID, txtTenant: txtTenant, MDescription: MDescription, MaterialMasterID: MaterialMasterID, PalletQuantity: $scope.PalletQuantity, MFGDate: $scope.MFGDate, NoOfPallets: $scope.NoOfPallets }

                )
                debugger
                $scope.MDescription = $scope.PalletQuantity = $scope.MFGDate = $scope.NoOfPallets = "";

                sessionStorage.setItem('InboundPalletReceiptList', JSON.stringify($scope.InboundPalletReceiptList));
                //alert(sessionStorage.length);                

                $("#MDescription").val("");

                return false;
            }

            /////////// delete selected sku in  InboundPalletReceiptList  ////////////////////

            $scope.delete = function (index) {
                debugger
                sessionStorage.removeItem('InboundPalletReceiptList');
                for (var i = 0; i < $scope.InboundPalletReceiptList.length; i++) {
                    if (index == i) {
                        $scope.InboundPalletReceiptList.splice(i, 1);
                        break;
                    }
                }
                sessionStorage.setItem('InboundPalletReceiptList', JSON.stringify($scope.InboundPalletReceiptList));
            }

            /////////// delete all added sku's in  session  ////////////////////

            $scope.Clear = function () {
                //alert();
                sessionStorage.removeItem('InboundPalletReceiptList');
                location.reload();
            }

            /////////// send  sku data to database   ////////////////////

            $scope.GeneratePutaway = function () {
                //alert("hai");
                debugger;

                $scope.GeneratePutawaySuggestionList = [];
                for (var i = 0; i < $scope.InboundPalletReceiptList.length; i++) {
                    $scope.GeneratePutawaySuggestionList.push({
                        TenantID: $scope.InboundPalletReceiptList[i].TenantID,
                        txtTenant: $scope.InboundPalletReceiptList[i].txtTenant,
                        MaterialMasterID: $scope.InboundPalletReceiptList[i].MaterialMasterID,
                        MDescription: $scope.InboundPalletReceiptList[i].MDescription,
                        PalletQuantity: $scope.InboundPalletReceiptList[i].PalletQuantity,
                        MFGDate: $scope.InboundPalletReceiptList[i].MFGDate,
                        NoOfPallets: $scope.InboundPalletReceiptList[i].NoOfPallets                      
                    })
                }

                debugger
                var http = {
                    method: 'POST',
                    url: 'InboundPalletReceipt.aspx/GeneratePutawaySuggestionList',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { '_Obj': JSON.stringify($scope.GeneratePutawaySuggestionList) }
                }
                $http(http).success(function (response) {
                    debugger
                    if (response.d != 0) {
                        showStickyToast(true, "successfully Created" + " " + "StoreRefNo -" + response.d)
                        sessionStorage.removeItem('InboundPalletReceiptList');
                        $scope.InboundPalletReceiptList = "";
                    }
                    else {
                        alert("Failed Insertion");
                    }

                })
            }
        });
    </script>
    <div class="container" ng-app="InboundPalletReceipt" ng-controller="InboundPalletReceiptController">
        <div align="center" class="">
            <form autocomplete="off">
                <div class="row" style="margin-bottom: 0px !important">
                    <div class="col m3 s4">
                        <div class="flex">
                            <input type="text" id="txtTenant" required="" />
                            <%--<span class="errorMsg"></span>--%>
                            <label>Tenant</label>
                            <span class="errorMsg"></span>
                        </div>


                        &emsp;
                    </div>
                    <div class="col m3 s4">
                        <div class="flex">
                            <%--<asp:HiddenField runat="server" ID="hifWareHouse" Value="0" ClientIDMode="Static" />
                            <asp:DropDownList Style="display: none;" runat="server" CssClass="NoPrint" required="" />--%>
                            <%-- <input type="text" id="MDescription" required="" ng-model="MDescription"/>--%>
                            <%--<input type="text" id="MDescription" runat="server" skinid="txt_Hidden_Req_Auto" required="" clientidmode="Static"  />--%>
                            <input type="text" id="MDescription" required="">
                            <label>Material Description</label>
                            <span class="errorMsg"></span>

                        </div>
                        &emsp;
                    </div>
                    <div class="col m3 s4">
                        <div class="flex">
                            <input type="number" id="PalletQuantity" required="" ng-model="PalletQuantity" autocomplete="off" />
                            <label>Pallet Quantity</label>
                            <span class="errorMsg"></span>
                        </div>
                        &emsp;
                    </div>
                    <div class="col m3 s4">
                        <div class="flex">
                            <input type="text" id="MFGDate" required="" ng-model="MFGDate" autocomplete="off" />
                            <label>MFG Date</label>
                            <span class="errorMsg"></span>
                        </div>
                        &emsp;
                    </div>
                </div>
                <div class="row">
                    <div class="col m3 s4">
                        <div class="flex">
                            <input type="number" id="NoPallets" required="" ng-model="NoOfPallets" autocomplete="off" />
                            <label>No.of Pallets</label>
                            <span class="errorMsg"></span>
                        </div>
                        &emsp;
                    </div>
                    <div class="col m3 s4 offset-m6" style="padding-right: 0;">
                        <div class="flex" style="float: right">
                            <input type="button" class="btn btn-sm btn-primary" ng-click="AddSKU()" value="Add SKU" />
                        </div>
                    </div>
                </div>
            </form>
        </div>
        <div class="">
            <table class="table-striped">
                <thead>
                    <tr>
                        <th>S.No</th>
                        <th>Tenant</th>
                        <th>Material Description</th>
                        <th>Pallet Quantity</th>
                        <th>MFG Date</th>
                        <th>No.of Pallets</th>
                        <th>Action</th>
                    </tr>
                </thead>
                <tbody>
                    <tr ng-repeat="x in InboundPalletReceiptList">
                        <td>{{$index+1}}</td>
                        <td>{{x.txtTenant}}</td>
                        <td>{{x.MDescription}}</td>
                        <td>{{x.PalletQuantity}}</td>
                        <td>{{x.MFGDate}}</td>
                        <td>{{x.NoOfPallets}}</td>
                        <td><a><i class="material-icons vl" ng-click="delete($index)">delete</i></a></td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="row">
            <br />
            <div class="col m3 s4 offset-l9" style="padding-right: 0;">
                <div class="flex" style="float: right" ng-show="InboundPalletReceiptList.length">
                    <input type="button" id="clear" class="btn btn-sm btn-primary" ng-click="Clear()" value="Clear" />
                    <input type="button" id="GeneratePutaway" class="btn btn-sm btn-primary" ng-click="GeneratePutaway()" value="Generate Putaway Suggestion List" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>

