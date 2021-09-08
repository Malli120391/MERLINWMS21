<%@ Page Title="Misc Transactions:." Language="C#" MasterPageFile="~/mInventory/InventoryMaster.master" AutoEventWireup="true" CodeBehind="miscTransactions.aspx.cs" Inherits="MRLWMSC21.mReports.miscTransactions" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="InvContent" runat="server">


    <script src="Scripts/jquery-ui-1.8.24.min.js"></script>
    <link href="../CSS/bootstrap.min.css" rel="stylesheet" />
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>

    <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
    <script src="CurrentStockReport.js"></script>
    <link href="Scripts/Custom.css" rel="stylesheet" />
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>

    <!--==================================== FOR Importaing Data Buttons ====================================== -->
    <script src="Scripts/FileSaver.min.js"></script>    
    <script src="Scripts/html2canvas.min.js"></script>
    <script src="Scripts/jspdf.min.js"></script>
    <script src="Scripts/jspdf.plugin.autotable.js"></script>
    <script src="Scripts/tableExport.min.js"></script>
    <script src="Scripts/xlsx.core.min.js"></script>
    <!--==================================== FOR Importaing Data Buttons ====================================== -->

        <script>
            //========================================  Created by M.D.Prasad ==================================//
            var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
            myApp.controller('miscTransactions', function ($scope, $http, $compile) {
                var RefTenant = "";
                var Refpartno = "";
                var Tenant = 0;
                var PartNo = 0;

                $('#txtTenant').val("");
                var textfieldname = $("#txtTenant");
                DropdownFunction(textfieldname);
                $("#txtTenant").autocomplete({
                    source: function (request, response) {
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
                        debugger;
                        RefTenant = i.item.val;
                        $("#txtPartnumber").val("");
                        Tenant = 0;
                        PartNo = 0;
                    },
                    minLength: 0
                });

                $('#txtPartnumber').val("");
                var textfieldname = $("#txtPartnumber");
                DropdownFunction(textfieldname);
                $("#txtPartnumber").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '../mWebServices/FalconWebService.asmx/LoadMaterialsForCurrentStock',
                            data: "{ 'prefix': '" + request.term + "','TenantID':'" + RefTenant + "'}",
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
                        Refpartno = i.item.val;
                    },
                    minLength: 0
                });

                $scope.clearData = function () {
                    $("#txtTenant").val("");
                    $("#txtPartnumber").val("");
                    RefTenant = "";
                    Refpartno = "";
                    Tenant = 0;
                    PartNo = 0;
                }

                $scope.getDetails = function () {
                    debugger;
                    Tenant = 0;
                    PartNo = 0;
                    if ($("#txtTenant").val() == "") {
                        Tenant = 0;
                        //showStickyToast(false, "Please Select Tenant", false);
                        //return false;
                    }
                    else {
                        Tenant = RefTenant;
                    }
                   
                    if ($("#txtPartnumber").val() == "") {
                        PartNo = 0;
                        //showStickyToast(false, "Please Select SKU", false);
                        //return false;
                    }
                    else {
                        PartNo = Refpartno;
                    }

                    var httpreq = $.ajax({
                        type: 'POST',
                        url: 'miscTransactions.aspx/getMiscTransactions',
                        headers: {
                            'Content-Type': 'application/json; charset=utf-8',
                            'dataType': 'json'
                        },
                        data: JSON.stringify({ Tenantid: Tenant, Mcode: PartNo }),
                        async: false,
                        success: function (data) {
                            debugger;
                            var dt = JSON.parse(data.d);
                            $scope.Data = dt.Table;
                        }
                    });
                };

                $scope.exportPdf = function () {
                    // 
                    $scope.export();
                    $("#tbldata").css('display', 'block');
                    $('#tbldata').tableExport({ type: 'pdf', jspdf: { orientation: 'l', margins: { right: 10, left: 10, top: 40, bottom: 40 }, autotable: { tableWidth: 'auto' } } });
                    $("#tbldata").css('display', 'none');
                    $scope.export();
                }
                $scope.exportExcel = function () {
                    //
                    $scope.export();
                    $("#tbldata").css('display', 'block');
                    $('#tbldata').tableExport({ type: 'excel' });
                    $("#tbldata").css('display', 'none');
                }
                $scope.exportCsv = function () {
                    //
                    $scope.export();
                    $("#tbldata").css('display', 'block');
                    $('#tbldata').tableExport({ type: 'csv', numbers: { html: { decimalMark: '.', thousandsSeparator: ',' }, output: { decimalMark: ',', thousandsSeparator: '' } } });
                    $("#tbldata").css('display', 'none');
                }

                $scope.exportXml = function () {
                    //
                    $scope.export();
                    $("#tbldata").css('display', 'block');
                    $('#tbldata').tableExport({ type: 'xml' });
                    $("#tbldata").css('display', 'none');
                }

                $scope.exportTxt = function () {
                    //
                    $scope.export();
                    $("#tbldata").css('display', 'block');
                    $('#tbldata').tableExport({ type: 'txt' });
                    $("#tbldata").css('display', 'none');
                }

                $scope.exportWord = function () {
                    //
                    $scope.export();
                    $("#tbldata").css('display', 'block');
                    $('#tbldata').tableExport({ type: 'doc' });
                    $("#tbldata").css('display', 'none');
                }

                $scope.export = function () {
                    debugger;
                    var table = $scope.Data;
                    $("#tbldata").empty();
                    $("#tbldata").append("<table><thead><tr><th>S. No.</th><th> SKU</th><th>Date</th><th>Store Ref. No.</th><th>Remarks</th><th>Reciept Quantity</th><th>Issued Quantity</th><th>Closing Quantity</th></tr></thead><tbody>");
                    for (var i = 0; i < table.length; i++) {
                        $("#tbldata").append("<tr><td>" + (i + 1) + " </td><td class='aligndate'>" + table[i].SKU + "</td><td class='aligndate'>" + table[i].Date + "</td><td class='aligndate'>" + table[i].RefNo + "</td><td class='aligndate'>" + table[i].Remarks + "</td><td class='aligndate'>" + table[i].RecieptQty + "</td><td class='aligndate'>" + table[i].IssueQty + "</td><td class='aligndate'>" + table[i].ClosingQty + "</td></tr>");
                    }
                    $("#tbldata").append("</tbody></table>");
                }

            });

         //========================================  Created by M.D.Prasad ==================================//

    </script>

    <style>
          .btht {
        background: #fff;
    border: 0;
    }
.button {
    background-color: #4CAF50; /* Green */
    border: none;
    color: white;
    padding: 4px 12px;
    text-align: center;
    text-decoration: none;
    display: inline-block;
    font-size: 9pt;
    margin: 4px 2px;
    -webkit-transition-duration: 0.4s; /* Safari */
    transition-duration: 0.4s;
    cursor: pointer;
    width:75px;
    border-radius:4px;
}

.button1 {
    background-color: white; 
    color: black; 
    border: 1px solid #4CAF50;
}

.button1:hover {
    background-color: #4CAF50;
    color: white;
}

.button2 {
    background-color: white; 
    color: black; 
    border: 1px solid #008CBA;
}

.button2:hover {
    background-color: #008CBA;
    color: white;
}

.button3 {
    background-color: white; 
    color: black; 
    border: 1px solid #f44336;
}

.button3:hover {
    background-color: #f44336;
    color: white;
}

.button4 {
    background-color: white;
    color: black;
    border: 1px solid #455b7c;
}

.button4:hover {background-color: #455b7c;color:white;}

.button5 {
    background-color: white;
    color: black;
    border: 1px solid #555555;
}

.button5:hover {
    background-color: #555555;
    color: white;
}

.button6 {
    background-color: white;
    color: black;
    border: 1px solid #6996e0;
}

.button6:hover {
    background-color: #6996e0;
    color: white;
}
    </style>
    <div class="dashed"></div>
     <table class="tbsty">
        <tbody>
            <tr class="module_yellow">
                <td class="ModuleHeader fixed-width exModule">
                    <div><a href="../Default.aspx">Home</a> / Reports / <span class=""><b>Misc Transactions</b></span></div>
                </td>
            </tr>
        </tbody>
    </table>
    <div ng-app="myApp" ng-controller="miscTransactions" class="pagewidth">
        <div class="loaderforCurrentStock" style="display: none;">
            <div style="width: 100%; height: 100vh; z-index: 999; position: fixed; top: 0; left: 0; right: 0; bottom: 0; align-items: center; display: flex; justify-content: center; background: rgba(255, 255, 255, 0.24); background: hsla(0, 0%, 100%, 0.72);">

                <div style="align-self: center;">
                    <div class="spinner">
                        <div class="bounce1"></div>
                        <div class="bounce2"></div>
                        <div class="bounce3"></div>
                    </div>

                </div>

            </div>

        </div>
        <div class="divlineheight"></div>
        <div class="row">

              
            <div class="col m3">
                <div class="flex">
                    <input type="text" id="txtTenant" class="TextboxInventoryAuto" required="" />
                    <label>Tenant </label>
                    <span class="errorMsg"></span>
                </div>
            </div>          
            
            <div class="col m3">
                <div class="flex">
                    <input type="text" id="txtPartnumber" class="TextboxInventoryAuto" required="" />
                    <label id="lblType">SKU</label>
                     <span class="errorMsg"></span>
                </div>
            </div>
            <div class="col m3" style="margin-bottom: 0px;"><br />
                    <div class="flex__ left">
                        <button type="button" id="btnSearch" ng-click="getDetails()"  class="btn btn-primary">Search <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>   &nbsp;                          
                        <button type="button" id="btnClear" ng-click="clearData()"  class="btn btn-primary">Clear <%=MRLWMSC21Common.CommonLogic.btnfaClear %></button>   &nbsp; 
                        <div class="exportto"> <a href="#" class="btn btn-primary">Export To &nbsp;<i class="material-icons">cloud_download</i></a>
                                    <ul class="export-menu">
                                               <li><span  id="btnPdf" class="buttons button3" ng-click="exportPdf()"><i class="fa fa-file-pdf-o" aria-hidden="true"></i> &nbsp;&nbsp;PDF</span></li>
                                                <li><span id="btnExcel" class="buttons button1" ng-click="exportExcel()"><i class="fa fa-file-excel-o" aria-hidden="true"></i>&nbsp;&nbsp;Excel</span></li>
                                                <li><span  id="btnTxt" class="buttons button4 hidden" ng-click="exportTxt()"><i class="fa fa-file-text-o" aria-hidden="true"></i>&nbsp;&nbsp;Txt</span></li>
                                                <li><span id="btnWord" class="buttons button2 hidden" ng-click="exportWord()"><i class="fa fa-file-word-o" aria-hidden="true"></i>&nbsp;&nbsp;Word</span></li>
                                                <li><span  id="btnCsv" class="buttons button1 hidden" ng-click="exportCsv()"><i class="fa fa-file-excel-o" aria-hidden="true"></i>&nbsp;&nbsp;CSV</span></li>
                                                <li><span id="btnXML" class="buttons button6 hidden" ng-click="exportXml()"><i class="fa fa-file-text-o" aria-hidden="true"></i>&nbsp;&nbsp;XML</span></li>
                                        </ul>
                            </div>
                            
                    </div></div>
        </div>
        <div class="row" style="margin: 0;">
            <div class="col-sm-12 col-lg-12"  style="padding: 0px 10px;">
                <div class="divmainwidth" >
                    <table class="mytableOutbound" id="tbldatas">
                        <thead>
                            <tr class="mytableReportHeaderTR">
                                <th>S. No.</th>
                                <th>SKU</th>
                                <th>Date</th>
                                <th>Store Ref. No.</th>
                                <th>Remarks</th>
                                <th>Reciept Quantity</th>
                                <th>Issued Quantity</th>
                                <th>Closing Quantity</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr dir-paginate="row in Data | itemsPerPage : 10">
                                 <td>{{$index+1}}</td>
                                 <td>{{row.SKU}}</td>
                                 <td>{{row.Date}}</td>
                                 <td>{{row.RefNo}}</td>
                                 <td>{{row.Remarks}}</td>
                                 <td>{{row.RecieptQty}}</td>
                                 <td>{{row.IssueQty}}</td>
                                 <td>{{row.ClosingQty}}</td>
                            </tr>
                            <tr ng-show="Data.length == 0">
                                <td colspan="9" style="text-align: center !important;">No Data Found</td>
                            </tr>
                        </tbody>
                        <tfoot>
                            <tr class="mytableReportFooterTR" ng-show="Data.length != 0">
                                <td colspan="9" style="padding-right: 2.4% !important; border: none;">
                                    <p></p>
                                    <div class="divpaginationstyle">
                                        <dir-pagination-controls direction-links="true" boundary-links="true"></dir-pagination-controls>
                                    </div>
                                </td>
                            </tr>
                        </tfoot>
                    </table>
                </div><br />
                <table id="tbldata"></table>
            </div>
        <div class="divlineheight"></div>
              
          </div>

    </div>
</asp:Content>
