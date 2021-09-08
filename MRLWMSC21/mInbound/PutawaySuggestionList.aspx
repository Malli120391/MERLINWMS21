<%@ Page Title="Putaway Suggestion List" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PutawaySuggestionList.aspx.cs" Inherits="MRLWMSC21.mInbound.PutawaySuggestionList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />
    <script src="../mInventory/Scripts/angular.min.js"></script>
    <script src="../mInventory/Scripts/dirPagination.js"></script>

    <style>
        .inline_text {
            border: 1px solid #ccc;
            height: 20px;
            width: 80%;
            padding-left: 10px
        }

        @media print {
            .hideFooter {
                display: block !important;
            }

            .table-striped th {
                border: none !important;
            }
        }

        .inline_text:focus {
            border: 1px solid #ccc !important;
        }
    </style>

    <script>
        var app = angular.module('PutawaySuggestionList', []);
        app.controller('PutawaySuggestionListController', function ($scope, $http) {

            $scope.CurrentDate = new Date();

            var WHID = 0;
            $scope.PutawaySuggestionList = [];
            $scope.List = function () {
                var http = {
                    method: 'POST',
                    //url:  'PutawaySuggestionList.aspx/getPutawaySuggestionList',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'PutawaySuggestionList': $scope.PutawaySuggestionList }
                }
                $http(http).success(function (response) {
                    debugger
                    $scope.PutawaySuggestionList = JSON.parse(response.d).Table;
                    if ($scope.count.length == 0) {
                        alert("Inserted Successfully");
                    }
                    else {
                        alert("Failed Insertion");
                    }

                });
            }

            //******Store Reference Number dropdown**********///// 

            $("#mDescription").val("");
            var textfieldname = $("#mDescription");
            DropdownFunction(textfieldname);
            $("#mDescription").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '../mWebServices/FalconWebService.asmx/LoadStoreRefNos',
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
                    //debugger;
                    $scope.Location = i.item.label;
                    $("#hdnDescription").val(i.item.val);

                },
                minLength: 0

            });


            $scope.ActualPutawayLocation = function (obj) {
                debugger;

                var textfieldname = $(".ActualPutLoc");
                DropdownFunction(textfieldname);
                $(".ActualPutLoc").autocomplete({
                    source: function (request, response) {
                        //debugger;
                        $.ajax({
                            url: '../mWebServices/FalconWebService.asmx/GetMMLocation',
                            data: "{ 'Prefix': '" + request.term + "','WarehouseID':'" + WHID + "'}",
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
                        Reflocno = i.item.val;
                        Location = i.item.label;
                        $scope.PutawaySuggestionList[obj - 1].ActualPutLocation = Location;
                    },
                    minLength: 0
                });
            }

            //*********Search in Create putawaysuggestionList*********//

            $scope.getPutawaysuggestionlist = function () {

                debugger;
                if ($("#mDescription").val() == "" || $("#hdnDescription").val() == "0") {
                    $("#hdnDescription").val("0");
                    showStickyToast(false, " Please select Store Reference");
                    return false;
                }
                var M = $scope.Location;
                var a = parseInt($("#hdnDescription").val());
                $scope.blockUI = true;
                var httpreq = {
                    method: 'POST',
                    url: 'PutawaySuggestionList.aspx/getPutawaySuggestionList',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'InbID': a },// $("#hdnDescription").val(); },
                    async: false
                }
                $http(httpreq).success(function (response) {
                    debugger
                    $scope.blockUI = false;
                    var dt = JSON.parse(response.d).Table;
                    if (dt == undefined || dt == null || dt.length == 0) {
                        showStickyToast(false, "No Data Found", false);
                        $scope.PutawaySuggestionList = null;
                        // document.querySelector('#tbldatas').classList.remove("tableLoader");
                        return false;
                    }
                    $scope.PutawaySuggestionList = dt;
                })
            }


            //***************** SavePutawaySuggestions*************//

            $scope.PutawaySuggestions = function () {
                debugger;
                $scope.SavePutawaySuggestions = [];
                for (var i = 0; i < $scope.PutawaySuggestionList.length; i++) {
                    if ($scope.PutawaySuggestionList[i].ActualPutLocation == null || $scope.PutawaySuggestionList[i].ActualPutLocation == undefined) {
                        $scope.SavePutawaySuggestions.push({

                            TenantID: $scope.PutawaySuggestionList[i].TenantID,
                            MaterialMasterID: $scope.PutawaySuggestionList[i].MaterialMasterID,
                            MDescription: $scope.PutawaySuggestionList[i].MDescription,
                            SuggestedQty: $scope.PutawaySuggestionList[i].SuggestedQty,
                            FinalLocationCode: $scope.PutawaySuggestionList[i].DisplayLocationCode,
                            InbID: parseInt($("#hdnDescription").val()),
                            SupplierInvoiceDetailsID: $scope.PutawaySuggestionList[i].SupplierInvoiceDetailsID

                        })
                    }
                    else {
                        $scope.SavePutawaySuggestions.push({


                            TenantID: $scope.PutawaySuggestionList[i].TenantID,
                            MaterialMasterID: $scope.PutawaySuggestionList[i].MaterialMasterID,
                            MDescription: $scope.PutawaySuggestionList[i].MDescription,
                            SuggestedQty: $scope.PutawaySuggestionList[i].SuggestedQty,
                            FinalLocationCode: $scope.PutawaySuggestionList[i].ActualPutLocation,
                            InbID: parseInt($("#hdnDescription").val()),
                            SupplierInvoiceDetailsID: $scope.PutawaySuggestionList[i].SupplierInvoiceDetailsID
                        })
                    }

                }
                debugger
                var http = {
                    method: 'POST',
                    url: 'PutawaySuggestionList.aspx/SavePutawaySuggestions',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { '_Obj': JSON.stringify($scope.SavePutawaySuggestions) },
                    async: false
                }
                $http(http).success(function (response) {
                    debugger
                    if (response.d != 0) {
                        showStickyToast(true, "successfully Created")
                        $scope.PutawaySuggestionList = "";
                    }
                    else {
                        alert("Failed Insertion");
                    }
                })
            }
        });





        //***************** Printer Code****************//

        function printDiv(divName) {
            debugger;
            var panel = document.getElementById("<%=PrintPanel.ClientID %>");
            //var printWindow = window.open('', '', 'height=400,width=800,scrollbars=1,location=1,status=1,');
            //var printWindow = window.open('', '', 'height=400,width=800,scrollbars=1,location=1,status=1,resizable=1');
            var printWindow = window.open('', '', 'location=0, status=0, resizable=1, scrollbars=1, width=800, height=400');
            printWindow.document.write('<html><head><title></title>');
            printWindow.document.write('<style type="text/css"> @page { size: landscape;margin-right:15px;margin-left:15px;;margin-top:10px;}  @media print { @page{ size: landscape;}}</style>');
            printWindow.document.write('</head><body >');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write('</body></html>');
            printWindow.document.write('<LINK href="../PrintStyle.css?v=4.0"  type="text/css" rel="stylesheet" media="print">');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
                printWindow.close();

            }, 0);


        }

    </script>

    <link href="../PrintStyle.css?v=4.0" type="text/css" rel="stylesheet" media="print" />

    <asp:Panel runat="server" ID="PrintPanel">

        <body ng-app="PutawaySuggestionList" ng-controller="PutawaySuggestionListController">

            <div class="container">
                <div ng-show="blockUI" class="NoPrint">
                    <div style="width: 100%; height: 100vh; z-index: 999; position: fixed; top: 0; left: 0; right: 0; bottom: 0; align-items: center; display: flex; justify-content: center; background: rgba(255, 255, 255, 0.24); background: hsla(0, 0%, 100%, 0.72);">
                        <div style="align-self: center;">
                            <img width="60" src="../Images/preloader.svg" />
                        </div>
                    </div>
                </div>

                <div class="loading" id="divLoading" style="display: none;"></div>
                <div id="printArea" class="PrintListcontainer">
                    <table border="0" cellpadding="0" cellspacing="0" align="center" width="100%" id="tdDDRPrintArea" visible="false" border="1">
                        <tr>
                            <td >
                                <img src="../Images/Swastiks_Logo.png" style="height:50px;width:180px"/>
                                <asp:Image runat="server" ID="imgLogo" Width="140" />
                            </td>
                            <td colspan="2">
                                <asp:Label runat="server" Text="Putaway Suggestion List" ID="lblHeader" CssClass="SubHeading3" Visible="false" align="center" />
                                <div class="divAlign" style="align:center">
                                    <div class="SubHeading3">PUTAWAY SUGGESTION LIST</div>

                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <hr style="height: 0.2px; color: #CCC; border-color: #CCC; background-color: #000;" />
                            </td>
                        </tr>
                    </table>

                    <div class="row" style="margin-bottom: 0px !important">
                        <div class="col m3 s4">
                            <div class="flex">
                                <input type="text" id="mDescription" required="" ng-model="mDescription" ng-value="{{$scope.Location}}"/>
                                <label>Store Reference No.</label>                             
                                <span class="errorMsg"></span>
                                <input type="hidden" id="hdnDescription" />
                            </div>
                            &emsp;
                        </div>
                        <div class="col m3 s4" hidden>

                            <div class="flex NoPrint">
                                <input type="text" id="txtWarehouse" required="" />
                                <label>Warehouse</label>
                                <span class="errorMsg"></span>
                                <input type="hidden" id="hdnWarehouse" />
                            </div>
                        </div>

                        <div class="col m3 s4 NoPrint">
                        </div>

                        <div class="col m3 s4" style="padding-right: 0;">
                            <div class="flex" style="float: right">
                                <button class="btn btn-primary NoPrint" ng-click="getPutawaysuggestionlist()" ng-model="Putawaylist">Search</button>
                                &nbsp;
                                <a href="#" style="text-decoration: none;" onclick="javascript:printDiv('tdDDRPrintArea');" class="btn btn-primary NoPrint">Print<%=MRLWMSC21Common.CommonLogic.btnfaPrint %></a>

                            </div>
                        </div>
                    </div>


                    <table class="table-striped" border="0" cellpadding="0" cellspacing="0" align="center" width="100%">

                        <tr>
                            <th>S.No</th>
                            <th>Material Description</th>
                            <th>Suggested Putaway Location </th>
                            <th>Actual Putaway Location</th>
                            <th>Quantity Per Pallet</th>
                        </tr>

                        <tbody border="0" cellpadding="0" cellspacing="0" align="center" width="100%">
                            <tr ng-repeat="x in PutawaySuggestionList">
                                <td>{{$index+1}}</td>
                                <td>{{x.MDescription}}</td>
                                <td>{{x.DisplayLocationCode}}</td>
                                <td>
                                    <div class="flex">
                                        <%--<input type="text" id="ActualPutLoc" class="inline_text" ng-model="ActualPutawayLocation" />--%>
                                        <input type="text" id="ActualPutLoc" class="ActualPutLoc" ng-model="row.AvailableLocation" required="" ng-change="ActualPutawayLocation($index+1,row)" ng-click="ActualPutawayLocation($index+1,row)" />

                                    </div>
                                </td>
                                <td>{{x.SuggestedQty}}</td>

                            </tr>
                        </tbody>

                    </table>
                    <br />
                    <div class="col m3 s4 p0 NoPrint" style="float: right" ng-show="PutawaySuggestionList.length">
                        <gap5></gap5>
                        <flex>
                     <input type="button" id="GeneratePutawaySuggestions" class="btn btn-sm btn-primary" ng-click="PutawaySuggestions()" value="Save&GRN" />                    
                </flex>
                    </div>
                </div>
            </div>
            <div style="display: table-footer-group; position: fixed; bottom: -5px; clear: both; right: 0; left: 0px; page-break-inside: avoid; page-break-before: always;">
                <table width="100%" class="tableSize">
                    <tr>
                        <td colspan="12">
                            <div style="display: flex; justify-content: space-between; align-items: center; padding: 0px 10px">
                                <span><small>Printed On : {{CurrentDate | date:'dd-MMM-yyyy hh:mm a'}}</small> </span>
                                <span>www.merlinwms.in</span>
                            </div>
                        </td>

                    </tr>
                </table>
            </div>
        </body>
    </asp:Panel>
</asp:Content>
