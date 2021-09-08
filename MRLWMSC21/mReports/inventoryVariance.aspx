<%@ Page Title="Inventory Variance:." Language="C#" MasterPageFile="~/mInventory/InventoryMaster.master" AutoEventWireup="true" CodeBehind="inventoryVariance.aspx.cs" Inherits="MRLWMSC21.mReports.inventoryVariance" %>

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
        myApp.controller('inventoryVariance', function ($scope, $http, $compile) {
            var RefTenant = "";
            var Refmtype = "0";
            var Refpartno = "";
            var Tenant = 0;
            var Warehouse = 0;
            var PartNo = 0;
            var MTypeId = 0;
            var TenantId = 0;
            var Warehouseid = 0;
          //  $('#txtTenant').val("");
            var textfieldname = $("#txtTenant");
            DropdownFunction(textfieldname);
            $("#txtTenant").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        //url: '../mWebServices/FalconWebService.asmx/GetTenantList',
                       // data: "{ 'prefix': '" + request.term + "'}",
                         url: '../mWebServices/FalconWebService.asmx/LoadTenantsByWH',  // Added by Ganesh @sep 25 2020-- Tenant drop down should be displayed by Warehouse
                        data: "{ 'prefix': '" + request.term + "','whid':'"+Warehouseid+"' }",                       
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
                    TenantId = i.item.val;
                    $("#txtPartnumber").val("");
                    $("#txtMaterialType").val("");
                   // $scope.LoadWareHouse();
                    Tenant = 0;
                    PartNo = 0;
                    MTypeId = 0;

                },
                minLength: 0
            });

          //  $scope.LoadWareHouse = function () {
                $("#txtWarehouse").val("");

                var textfieldname = $("#txtWarehouse");
                debugger;
                DropdownFunction(textfieldname);
                $("#txtWarehouse").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            //url: '../mWebServices/FalconWebService.asmx/LoadWHForWHList',
                           // data: "{ 'prefix': '" + request.term + "','TenantID':'" + TenantId + "'  }",
                            url: '../mWebServices/FalconWebService.asmx/LoaDWHListBasedonUser',  // Added by Ganesh @sep 25 2020-- WH drop down should be displayed by user based
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
                        Warehouseid = i.item.val;
                        $('#txtTenant').val("");
                        $("#txtPartnumber").val("");
                        $("#txtMaterialType").val("");
                       // $("#txtWarehouse").val("");

                    },
                    minLength: 0
                });
                //ending of warehouse dropdown
           // }

            $("#txtMaterialType").val("");
            var textfieldname = $("#txtMaterialType");
            DropdownFunction(textfieldname);
            $("#txtMaterialType").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '../mWebServices/FalconWebService.asmx/LoadMaterialTypesForCurrentStock',
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
                    Refmtype = i.item.val;
                    $("#txtPartnumber").val("");
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
                        url: '../mWebServices/FalconWebService.asmx/LoadMaterialsUnderMtype',
                        data: "{ 'prefix': '" + request.term + "','MTypeID':'" + parseInt(Refmtype) + "','tenantid':'"+RefTenant+"'}",
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
                $("#txtMaterialType").val("");
                Refmtype = "";
                RefTenant = "";
                Refpartno = "";
                Tenant = 0;
                PartNo = 0;
                MTypeId = 0;

            }

            $scope.getDetails = function () {
                debugger;
                Tenant = 0;
                PartNo = 0;
                MTypeId = 0;
                Warehouse = 0;

                if ($("#txtWarehouse").val() == "") {
                    Warehouse = 0;
                    showStickyToast(false, "Please select WareHouse", false);
                    return false;
                }
                //else {
                //    Tenant = RefTenant;
                //}
                if ($("#txtTenant").val() == "") {
                    Tenant = 0;
                    showStickyToast(false, "Please select Tenant", false);
                    return false;
                }
                else {
                    Tenant = RefTenant;
                }

                if ($("#txtMaterialType").val() == "") {
                    MTypeId = 0;
                }
                else {
                    MTypeId = Refmtype;
                }
                if ($("#txtPartnumber").val() == "") {
                    PartNo = 0;
                    showStickyToast(false, "Please select SKU", false);
                    return false;
                }
                else {
                    PartNo = Refpartno;
                }

                var httpreq = $.ajax({
                    type: 'POST',
                    url: 'inventoryVariance.aspx/getInventoryVarianceData',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: JSON.stringify({ Tenantid: Tenant, Mtypeid: MTypeId, Mcode: PartNo, WareHouseId: Warehouseid }),
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
                $("#tbldata").append("<table><thead><tr><th>S. No.</th><th> SKU</th><th>Inventory Dispersion</th><th>Net Variance </th><th>Spread Variance</th><th>Available Quantity</th></tr></thead><tbody>");
                for (var i = 0; i < table.length; i++) {
                    $("#tbldata").append("<tr><td>" + (i + 1) + " </td><td class='aligndate'>" + table[i].SKU + "</td><td class='aligndate'>" + table[i].Dispertion + "</td><td class='aligndate'>" + table[i].NetVariance + "</td><td class='aligndate'>" + table[i].SpreadVariance + "</td><td class='aligndate'>" + table[i].AQTY + "</td></tr>");
                }
                $("#tbldata").append("</tbody></table>");
            }
        });

         //========================================  Created by M.D.Prasad ==================================//

    </script>

  

<!--breadcrumb-->
    <div class="module_yellow">
        <div class="ModuleHeader">
            <div>
                <a href="../Default.aspx">Home</a> <i class="material-icons">arrow_right</i> 
                 <span>Reports</span> <i class="material-icons">arrow_right</i> 
                 <span>Inventory</span> <i class="material-icons">arrow_right</i> 
                 <span class="breadcrumbd" contenteditable="false">Inventory Variance</span>
            </div>
        </div>
    </div>
<!--ends-breadcrumb-->  
    <div ng-app="myApp" ng-controller="inventoryVariance" class="container">
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
                            <input type="text" id="txtWarehouse" required="" />
                            <label><%= GetGlobalResourceObject("Resource", "WareHouse")%></label>
                            <span class="errorMsg"></span>
                        </div>
                    </div>

            <div class="col m3">
                <div class="flex">
                    <input type="text" id="txtTenant" class="TextboxInventoryAuto" required="" />
                    <label><%= GetGlobalResourceObject("Resource", "Tenant")%> </label>
                    <span class="errorMsg"></span>
                    
                </div>
            </div>
              
            <div class="col m3">
                <div class="flex">
                    <input type="text" id="txtMaterialType" class="TextboxInventoryAuto" required="" />
                    <label><%= GetGlobalResourceObject("Resource", "MaterialType")%> </label>
                </div>
            </div>

            <div class="col m3">
                <div class="flex">
                    <input type="text" id="txtPartnumber" class="TextboxInventoryAuto" required="" />
                    <label><%= GetGlobalResourceObject("Resource", "SKU")%></label>
                    <span class="errorMsg"></span>
                </div>
            </div>
            
            </div>

        <div class="row">
            <div class="col m12">
                <gap5></gap5>
                <div flex end>
                    <button type="button" id="btnSearch" ng-click="getDetails()" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Search")%> <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                     <a ng-click="exportExcel()" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Export")%><i class="fa fa-file-excel-o" aria-hidden="true"></i></a>                    
                    <button type="button" id="btnClear" ng-click="clearData()" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Clear")%> <%=MRLWMSC21Common.CommonLogic.btnfaClear %></button>                   
                     
                </div>
                </div>
        </div>

        <div class="row" style="margin: 0;">
            <div class="col-sm-12 col-lg-12"  style="padding: 0px 10px;">
                <div class="divmainwidth" >
                    <table class="table-striped" id="tbldatas">
                        <thead>
                            <tr class="mytableReportHeaderTR">
                              <%--  <th sno>S. No.</th>--%>
                                  <th sno><%= GetGlobalResourceObject("Resource", "SNo")%></th>
                                <%--<th>SKU</th>--%>
                                <th><%= GetGlobalResourceObject("Resource", "SKU")%> </th>
                             <%--   <th>Inventory Dispersion</th>--%>
                                   <th><%= GetGlobalResourceObject("Resource", "InventoryDispersion")%> </th>
                       <%--         <th>Net Variance</th>--%>
                                         <th><%= GetGlobalResourceObject("Resource", "NetVariance")%> </th>
                            <%--    <th>Spread Variance</th>--%>
                                    <th><%= GetGlobalResourceObject("Resource", "SpreadVariance")%> </th>
                              <%--  <th number>Available Quantity</th>--%>
                                  <th number><%= GetGlobalResourceObject("Resource", "AvailableQuantity")%> </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr dir-paginate="row in Data | itemsPerPage : 10">
                                 <td sno>{{$index+1}}</td>
                                 <td>{{row.SKU}}</td>
                                 <td>{{row.Dispertion}}</td>
                                 <td>{{row.NetVariance}}</td>
                                 <td>{{row.SpreadVariance}}</td>
                                 <td number>{{row.AQTY}}</td>
                            </tr>
                            <tr ng-show="Data.length == 0">
                          <%--      <td colspan="6" style="text-align: center !important;">No Data Found</td>--%>
                                      <td colspan="6" style="text-align: center !important;"><%= GetGlobalResourceObject("Resource", "NoDataFound")%> </td>
                            </tr>
                        </tbody>
                        <tfoot>
                            <tr class="mytableReportFooterTR" ng-show="Data.length != 0">
                                <td colspan="6" style="padding-right: 2.4% !important; border: none;">
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
