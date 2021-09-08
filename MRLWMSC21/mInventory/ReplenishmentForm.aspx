<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReplenishmentForm.aspx.cs" Inherits="MRLWMSC21.mInventory.ReplenishmentForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../mInventory/Scripts/angular.min.js"></script>
    <script src="../mInventory/Scripts/dirPagination.js"></script>
    <style>
        .inline_text1 {
            border: 1px solid #ccc;
            height: 20px;
            width: 60%;
            padding-left:10px
        }
        .inline_text2 {
            border: 1px solid #ccc;
            height: 20px;
            width: 30%;
            padding-left:10px
        }
    </style>
    <script>
        //=================== Material Description dropdown ================================//
        $(document).ready(function () {
            debugger
            var TextFieldName = $("#MDescription");
             DropdownFunction(TextFieldName);
            $("#MDescription").autocomplete({
                
                source: function (request, response) {
                    if ($("#txtContainertype").val() == '') {
                        $("#hifContainertype").val('0');
                    }
                    $.ajax({


                        url: '../mWebServices/FalconWebService.asmx/LoadMaterialDesData',
                        data: "{ 'prefix': '" + request.term + "'  }",  

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

                    $("#hifWareHouse").val(i.item.val);

                },
                minLength: 0
            });
        });
        var app = angular.module('ReplenishmentForm', []);
        app.controller('ReplenishmentFormController', function ($scope, $http) {

            $scope.ReplenishmentFormAddSKU = [];
            $scope.AddSKU = function () {
                var MDescription = $("#MDescription").val();
                $scope.MaterialMasterID = $("#hifWareHouse").val();
                if (MDescription ==  undefined || MDescription == null || MDescription == "" ) {
                    showStickyToast(false, "Select Material Description", false);
                    return false;
                }
                if ($scope.NoOfPallets ==   undefined || $scope.NoOfPallets == "" || $scope.NoOfPallets == null) {
                    showStickyToast(false, "Please Enter No. Of Pallets", false);
                    return false;
                }
                $scope.ReplenishmentFormAddSKU.push(
                    { MDescription: MDescription, MaterialMasterID:$scope.MaterialMasterID, NoOfPallets: $scope.NoOfPallets }
                )
                $("#MDescription").val("");
                $("#NoOfPallets").val("");
            }
            /////////// send  sku data to database   ////////////////////

            $scope.GeneratePickList = function () {
                debugger
                $scope.FinalGeneratePickList = [];
                for (var i = 0; i < $scope.ReplenishmentFormAddSKU.length; i++) {
                    $scope.FinalGeneratePickList.push(
                        {
                            MDescription: $scope.ReplenishmentFormAddSKU[i].MDescription,
                            MaterialMasterID: $scope.ReplenishmentFormAddSKU[i].MaterialMasterID,
                            NoOfPallets: $scope.ReplenishmentFormAddSKU[i].NoOfPallets
                        }
                    )
                }
                debugger
                var http = {
                    method: 'POST',
                    url: 'ReplenishmentForm.aspx/ReplenishmentFormAdd',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { '_Obj': JSON.stringify($scope.FinalGeneratePickList) }
                }
                $http(http).success(function (response) {
                    debugger   
                    $scope.result1 = JSON.parse(response.d).Table;
                    var result = response.d;
                    if (result != 0) {
                        sessionStorage.setItem('FinalGeneratePickList', JSON.stringify($scope.result1));
                        window.location.href = "ReplenishmentFormPickList.aspx";
                        //sessionStorage.removeItem('FinalGeneratePickList');
                    }
                    else {
                        alert("Failed Insertion");
                    }
                    
                });
            }
        });
    </script>

    <body ng-app="ReplenishmentForm" ng-controller="ReplenishmentFormController">
        <div class="container">
            <table border="0" class="" align="center" cellpadding="5" cellspacing="5">
                <tr class="ListHeaderRow">
                    <td class="FormLabels" align="right">
                        <div class="row">
                            <div class="col m3 s4">
                                <div class="flex">
                                    <asp:HiddenField runat="server" ID="hifWareHouse" Value="0" ClientIDMode="Static" />
                                    <asp:DropDownList Style="display: none;" runat="server" CssClass="NoPrint" required="" />
                                    <input type="text" id="MDescription" runat="server" skinid="txt_Hidden_Req_Auto" required="" clientidmode="Static" />
                                    <label>Material Description</label>
                                    <span class="errorMsg"></span>
                                </div>
                                &emsp;
                            </div>
                            <div class="col m3 s4">
                                <div class="flex">
                                    <input type="number" id="NoOfPallets" required="" ng-model="NoOfPallets" autocomplete="off" />
                                    <label>No.of Pallets</label>
                                    <span class="errorMsg"></span>
                                </div>
                                &emsp;
                            </div>
                            <div class="col m3 s4 offset-m3" style="padding-right: 0;">
                                <div class="flex" style="float: right">
                                    <button class="btn btn-primary" ng-click="AddSKU()"  style="margin-right: 20px">Add SKU</button>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
            <table class="table-striped">
                <tr>
                    <th>S.No</th>
                    <th>Material Description</th>
                    <th>No.of Pallets </th>
                </tr>
                <tbody>
                    <tr ng-repeat="x in ReplenishmentFormAddSKU ">
                        <td>{{$index+1}}</td>
                        <td>{{x.MDescription}}</td>
                        <td>{{x.NoOfPallets}}</td>
                    </tr>
                </tbody>
            </table>
            <br />
            <div class="col m3 s4 p0 " style="float: right">
                <gap5></gap5>
                <flex>
            <input type="button" Class="btn btn-primary"  ng-model="Picklist" ng-click="GeneratePickList()" value="Generate Pick List" />  
        </flex>
            </div>
        </div>
    </body>

</asp:Content>
