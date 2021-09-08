<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReplenishmentFormPickList.aspx.cs" Inherits="MRLWMSC21.mInventory.ReplenishmentFormPickList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />
    <script src="../mInventory/Scripts/angular.min.js"></script>
    <script src="../mInventory/Scripts/dirPagination.js"></script>
    <script>

        var app = angular.module('ReplenishmentFormPickList', []);
        app.controller('ReplenishmentFormPickListController', function ($scope, $http) {
            debugger
            $scope.ReplenishmentFormPickList_Session = [];
            $scope.ReplenishmentFormPickList_Session = JSON.parse(sessionStorage.getItem('FinalGeneratePickList'));
            debugger

            $scope.GetLocation = function (obj) {
                debugger;
                // $("#txtLocation").val("");
                var textfieldname = $(".txtLocation");
                DropdownFunction(textfieldname);
                $(".txtLocation").autocomplete({
                    source: function (request, response) {
                        $.ajax({

                            url: '../mWebServices/FalconWebService.asmx/LoadLocationsForRF',
                            data: "{'prefix': '" + request.term + "'}",
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                debugger;
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
                        debugger;

                        LocationID = i.item.val;
                        Location = i.item.label;
                        $scope.ReplenishmentFormPickList_Session[obj - 1].ActualPutLocation = Location;

                    },
                    minLength: 0
                });

            }



            $scope.Save = function () {
                debugger
                $scope.FinalSavePickList = [];
                for (var i = 0; i < $scope.ReplenishmentFormPickList_Session.length; i++) {
                    if ($scope.ReplenishmentFormPickList_Session[i].ActualPutLocation == null || $scope.ReplenishmentFormPickList_Session[i].ActualPutLocation == undefined) {
                        $scope.FinalSavePickList.push(
                            {
                                MDescription: $scope.ReplenishmentFormPickList_Session[i].MDescription,
                                MaterialMasterID: $scope.ReplenishmentFormPickList_Session[i].MaterialMasterID,
                                FinalLocationCode: $scope.ReplenishmentFormPickList_Session[i].Location,
                                Quantity: $scope.ReplenishmentFormPickList_Session[i].Quantity
                            }
                        )
                    }
                     else {
                        $scope.FinalSavePickList.push({

                            MDescription: $scope.ReplenishmentFormPickList_Session[i].MDescription,
                            MaterialMasterID: $scope.ReplenishmentFormPickList_Session[i].MaterialMasterID,
                            FinalLocationCode: $scope.ReplenishmentFormPickList_Session[i].ActualPutLocation,                            
                            Quantity: $scope.ReplenishmentFormPickList_Session[i].Quantity                            
                        })
                    }
                }
                debugger
                var http = {
                    method: 'POST',
                    url: 'ReplenishmentFormPickList.aspx/ReplenishmentFormSave',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { '_Obj': JSON.stringify($scope.FinalSavePickList) }
                }
                $http(http).success(function (response) {
                    debugger
                    $scope.result1 = JSON.parse(response.d).Table;
                    var result = response.d;
                    if (result != 0) {
                        sessionStorage.setItem('FinalSavePickList', JSON.stringify($scope.result1));
                        //sessionStorage.removeItem('FinalGeneratePickList');
                    }
                    else {
                        alert("Failed Insertion");
                    }

                });
            }


            $scope.ConfirmMovement = function () {                              
                debugger
                var http = {
                    method: 'POST',
                    url: 'ReplenishmentFormPickList.aspx/ReplenishmentFormConfirmMove',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: {  }
                }
                $http(http).success(function (response) {
                    debugger
                    $scope.result1 = JSON.parse(response.d).Table;
                    var result = response.d;
                    if (result != 0) {
                       alert(" Insertion Done");
                       
                    }
                    else {
                        alert("Failed Insertion");
                    }

                });
            }


        });
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
    <style>
        .inline_text1 {
            border: 1px solid #ccc;
            height: 20px;
            width: 60%;
            padding-left: 10px
        }

        .inline_text2 {
            border: 1px solid #ccc;
            height: 20px;
            width: 30%;
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
    <link href="../PrintStyle.css?v=4.0" type="text/css" rel="stylesheet" media="print" />
    <asp:Panel runat="server" ID="PrintPanel">
        <body ng-app="ReplenishmentFormPickList" ng-controller="ReplenishmentFormPickListController">
            <div class="container">
                <div class="loading" id="divLoading" style="display: none;"></div>
                <div id="printArea" class="PrintListcontainer">
                    <table border="0" cellpadding="0" cellspacing="0" align="center" width="100%" id="tdDDRPrintArea" visible="false">
                        <tr>
                            <td style="width: 38% !important;">
                                <%--<img src="../Images/inventrax.jpg" width="140" />--%>
                                <asp:Image runat="server" ID="imgLogo" Width="140" />
                            </td>
                            <td colspan="2">
                                <asp:Label runat="server" Text="ReplenishmentForm PickList" ID="lblHeader" CssClass="SubHeading3" Visible="false" />
                                <div class="divAlign">
                                    <div class="SubHeading3">REPLENISHMENTFORM PICKLIST</div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <hr style="height: 0.2px; color: #CCC; border-color: #CCC; background-color: #000;" />
                            </td>
                        </tr>
                    </table>


                    <div class="col m3 s4" style="padding-right: 0;">
                        <div class="flex" style="float: right">
                            <a href="" style="text-decoration: none;" onclick="javascript:printDiv('tdDDRPrintArea');" class="btn btn-primary NoPrint">Print<%=MRLWMSC21Common.CommonLogic.btnfaPrint %></a>
                        </div>
                    </div>
                </div>
                <br />
                &nbsp;
            <table class="table-striped" border="0" cellpadding="0" cellspacing="0" align="center" width="100%">
                <tr>
                    <th>S.No</th>
                    <th>Material Description</th>
                    <th>Suggested Location</th>
                    <th>Actual Location</th>
                    <th style="text-align: center">Quantity per Pallet</th>

                </tr>
                <br />
                <tbody border="0" cellpadding="0" cellspacing="0" align="center" width="100%">
                    <tr ng-repeat="M in ReplenishmentFormPickList_Session">
                        <td>{{$index+1}}</td>
                        <td>{{M.MDescription}}</td>
                        <td>{{M.Location}}</td>
                        <td>
                            <div class="flex">
                                <input type="text" id="txtLocation" class="txtLocation" ng-model="row.GetLocation" required="" ng-change="GetLocation($index+1,row)" ng-click="GetLocation($index+1,row)" />

                            </div>
                        </td>
                        <td style="text-align: center">{{M.Quantity}}</td>
                    </tr>
                </tbody>
            </table>
                <br />
                <div class="col m3 s4 p0  NoPrint" style="float: right">
                    <gap5> </gap5>
                    <flex class="NoPrint">
           <input type="button" Class="btn btn-primary"  ng-model="RFPicklist" ng-click="Save()" value="Save" />          
           <input type="button" Class="btn btn-primary"  ID="lnkConfirmMovement"  ng-model="RFPicklist" ng-click="ConfirmMovement()" value="Confirm Movement" CssClass="btn btn-primary NoPrint" />              
         </flex>
                </div>
            </div>

            <div style="display: table-footer-group; position: fixed; bottom: -5px; clear: both; right: 0; left: 0px; page-break-inside: avoid; page-break-before: always;">
                <table width="100%" class="tableSize">
                    <tr>
                        <td colspan="12">
                            <div style="display: flex; justify-content: space-between; align-items: center; padding: 0px 10px">
                                <span><small>Printed On : {{currentDate | date:'dd-MMM-yyyy hh:mm a'}}</small> </span>
                                <span>www.merlinwms.in</span>
                            </div>
                        </td>

                    </tr>
                </table>
            </div>
        </body>
    </asp:Panel>
</asp:Content>


