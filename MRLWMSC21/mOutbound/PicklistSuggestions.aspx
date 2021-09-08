<%@ Page Title="" Language="C#" MasterPageFile="~/mOutbound/OutboundMaster.master" AutoEventWireup="true" CodeBehind="PickListSuggestions.aspx.cs" Inherits="MRLWMSC21.mOutbound.PickListSuggestions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="OBContent" runat="server">
    <script src="../Scripts/angular.min.js"></script>
    <script src="../mReports/Scripts/dirPagination.js"></script>
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>

    <script src="Scripts/jquery-ui-1.8.24.min.js"></script>
    <link href="../CSS/bootstrap.min.css" rel="stylesheet" />
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>
    <style>
        .PDF:hover {
            color: #c60909;
        }

        .table-striped thead th {
            box-shadow: none !important;
        }

        table table thead tr {
            height: 0px !important;
        }

        table table tbody {
            transform: translateY(-16PX);
        }

        .secondTble {
            opacity: 0;
        }

            .secondTble th {
                padding-top: 0px !important;
                padding-bottom: 0px !important;
            }

        :root {
            /*--indesder: #fdfdfd;*/
            --insdeborder: #0e0e0e;
        }

        .table-striped th:last-of-type {
            background: var(--indesder) !important;
        }

        .table-striped td:last-of-type {
            background: var(--indesder) !important;
        }

        .table-striped table tr {
            background: transparent !important;
        }

        .table-striped table td {
            background: transparent !important;
            padding: 9px 10px 9px 9px !important;
        }

        .table-striped table th {
            background: transparent !important;
            padding: 9px 10px 9px 9px !important;
        }
    </style>
    <script>
        var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
        myApp.controller('PickNote', function ($scope, $http, $compile) {
            var OBDID = 0;
            var PickrefNo = 0;

            $scope.getDetails = function () {
                debugger;
                if (window.location.href.indexOf("OBDNO") > -1)
                {
                    var obj = location.href.split('?')[1].split('&');
                    if (obj.length > 0) {
                        PickrefNo = obj[0].split('=')[1];
                        OBDID = obj[1].split('=')[1];
                    }
                }


                var BPRData = {
                    method: 'POST',
                    url: 'PickListSuggestions.aspx/GetPickNoteData',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'OBDID': OBDID },
                }
                $http(BPRData).success(function (response) {

                    $scope.Picklist = response.d;
                    console.log($scope.Picklist)

                    $scope.PickrefNo = PickrefNo;
                    if ($scope.Picklist.length == 0) {
                        showStickyToast(false, 'No data Found', false);
                    }


                });
            };

            $scope.getDetails();


       

            $scope.downloadExcel = function () {


               if (window.location.href.indexOf("OBDNO") > -1) {
                    var obj = location.href.split('?')[1].split('&');
                    if (obj.length > 0) {
                        PickrefNo = obj[0].split('=')[1];
                        OBDID = obj[1].split('=')[1];
                    }
                }





                var BPRData = {
                    method: 'POST',
                    url: 'PickListSuggestions.aspx/DownloadExcel',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'OBDID': OBDID, 'OBDNumber': PickrefNo },
                }
                $http(BPRData).success(function (response) {
                    var filename = response.d;
                    window.open('../ExcelData/' + filename + '.xlsx');
                });

            }
            $scope.PickListWisePrint = function () {
                debugger;
                var printer = $("#ddlWarehousePrinter").val();
                var Printdata = {
                    method: 'POST',
                    url: 'PickListSuggestions.aspx/PicklistwisePrint',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'pickHeaderID': picklistHeaderID, 'PrinterIP': printer },
                }
                $http(Printdata).success(function (response) {
                    if (response.d == "success") {
                        $scope.blockUI = false;
                        //showStickyToast("Print Successfully", false);
                        showStickyToast(true, 'Print Successfully', false);
                    }
                });
            }
        });

      


       


       

    </script>
    <div ng-app="myApp" ng-controller="PickNote" class="pagewidth">
        <br />
        <%-- <div flex id="dvprinter">
            <asp:DropDownList ID="ddlWarehousePrinter" ClientIDMode="Static" runat="server" CssClass="ddl_slim" />
            <a id="btnPrintLabel" ng-click="PickListWisePrint()">
                <i class="material-icons">print</i><em class="sugg-tooltis">Print</em></a>
        </div>--%>

        <div class="flex__ end">
            <div>
                <a style='button' ng-click="downloadExcel()">
                    <img height='20' style='filter: grayscale(100%) !important;' width='20' src='../Images/excel_icon1.jpg'></a>
            </div>

        </div>
        <div style="margin-left: 1.6%; font-family: Arial; font-size: small; font-weight: bold;">

            <br />
            <table>
                <tr>
                    <td>
                        <label>OBD Number</label>: {{PickrefNo}}
                    </td>
                </tr>
            </table>
            <table class="table-striped">
                <thead>
                    <tr class="mytableOutboundHeaderTR">
                        <th>Line #</th>
                        <th>Part#</th>
                        <th>SO#</th>
                        <th>UoM</th>
                        <th>Total Qty.</th>
                        <th align="center" style="border-right: 1px solid #e6e6e6; border-left: 1px solid #e6e6e6;">
                            <table>
                                <thead>
                                    <tr>
                                        <th style="width: 150px">Location</th>
                                        <th style="width: 150px">Carton Code</th>
                                        <th style="width: 150px">Assigned Qty.</th>
                                        <th style="width: 150px">MfgDate</th>
                                        <th style="width: 150px">ExpDate</th>
                                        <th style="width: 150px">Picked Qty</th>
                                        <th style="width: 150px">Total Vol.(m³)</th>
                                       
                                    </tr>
                                </thead>
                            </table>
                        </th>

                    </tr>
                </thead>

                <tbody>
                    <tr ng-repeat="Pick in Picklist">
                        <td class="alignnumbers">{{Pick.LineNumber}}  </td>

                        <td>
                            <b>SKU &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</b>&nbsp;&nbsp;&nbsp;{{ Pick.Mcode }}<br />
                            <b>Description &nbsp;:</b>&nbsp;&nbsp; {{ Pick.MDescription }}<br />

                        </td>
                        <td>{{Pick.CustomerName}}</td>
                        <td>{{Pick.MUoMQty}}</td>

                        <td>{{ Pick.ReqQty }}</td>
                        <td align="center" style="border-right: 1px solid #e6e6e6; border-left: 1px solid #e6e6e6;">
                            <table ng-if="Pick.objAssignedList.length!=0">

                                <thead class="secondTble">
                                    <tr>
                                        <th style="height: 0px !important; padding-top: 0px !important; padding: 0px !important; width: 150px">Location</th>
                                        <th style="height: 0px !important; padding-top: 0px !important; padding: 0px !important; width: 150px">Carton Code</th>
                                        <th style="height: 0px !important; padding-top: 0px !important; padding: 0px !important; width: 150px">Assigned Qty.</th>
                                        <th style="height: 0px !important; padding-top: 0px !important; padding: 0px !important; width: 150px">MfgDate</th>
                                        <th style="height: 0px !important; padding-top: 0px !important; padding: 0px !important; width: 150px">ExpDate</th>
                                        <th style="height: 0px !important; padding-top: 0px !important; padding: 0px !important; width: 150px">Picked Qty</th>
                                        <th style="height: 0px !important; padding-top: 0px !important; padding: 0px !important; width: 150px">Total Vol.(m³)</th>
                                        
                                    </tr>

                                </thead>

                                <tbody>
                                    <tr ng-repeat="MATERIALINFO in Pick.objAssignedList">
                                        <td>{{ MATERIALINFO.Location }}</td>
                                        <td>{{ MATERIALINFO.CartonCode }}</td>
                                        <td>{{ MATERIALINFO.AssignedQty }}</td>
                                        <td>{{MATERIALINFO.MfgDate}}</td>
                                        <td>{{MATERIALINFO.ExpDate}}</td>
                                        <td>{{MATERIALINFO.PickedQty}}</td>
                                        <td>{{MATERIALINFO.TotalVolume}}</td>
                                        


                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
            </table>
            <br />
        </div>
        <br />
       
    </div>


</asp:Content>
