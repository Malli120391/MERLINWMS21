<%@ Page Title="GRN Pending:." Language="C#" MasterPageFile="~/mInventory/InventoryMaster.master" AutoEventWireup="true" CodeBehind="grnPending.aspx.cs" Inherits="MRLWMSC21.mReports.grnPending" %>

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
        myApp.controller('grnPending', function ($scope, $http, $compile) {
            var RefTenant = "";
            var Refpartno = "";
            var Tenant = 0;
            var PartNo = 0;
            var TenantId = 0;
            var Warehouseid = 0;
            $scope.typeValue = "1";
            var tvalue = "1";
            debugger;
            $scope.typeWise = [{ type: "Doc. Wise", val: 1 }, { type: "SKU Wise", val: 2 }];
            $scope.form = { value: $scope.typeWise[0].val };

            $('#txtTenant').val("");
            var textfieldname = $("#txtTenant");
            DropdownFunction(textfieldname);
            $("#txtTenant").autocomplete({
                source: function (request, response) {
                    debugger;
                    TenantId = 0;
                    RefTenant = 0;
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
                    $("#txtPartnumber").val("");
                    TenantId = i.item.val;
                    Tenant = 0;
                    PartNo = 0;
                  //  $scope.LoadWareHouse();
                },
                minLength: 0
            });

          //  $scope.LoadWareHouse = function () {
                $("#txtWarehouse").val("");

                var textfieldname = $("#txtWarehouse");
                //debugger;

                DropdownFunction(textfieldname);
                $("#txtWarehouse").autocomplete({
                    source: function (request, response) {
                        debugger;
                        $.ajax({
                            //url: '../mWebServices/FalconWebService.asmx/LoadWHForWHList',
                            //data: "{ 'prefix': '" + request.term + "','TenantID':'" + Tenantid + "'  }",
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
                         $("#txtTenant").val("");
                    },
                    minLength: 0
                });

       //     }   
            $('#txtPartnumber').val("");
            var textfieldname = $("#txtPartnumber");
            DropdownFunction(textfieldname);
            $("#txtPartnumber").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '../mWebServices/FalconWebService.asmx/LoadFilters',
                        data: "{ 'prefix': '" + request.term + "','TenantID':'" + RefTenant + "','Type':'" + tvalue +"'}",
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

            $scope.changeType = function ()
            {
                //$scope.typeValue = $("#selType").val().split(":")[1];
                tvalue = $("#selType").val().split(":")[1];
                debugger;
                if (tvalue == "1") {
                    $("#lblType").text("");
                    $("#lblType").text("Invoice Number");
                   // $("#txtTenant").val("");
                   // $("#txtWarehouse").val("");
                    $("#txtPartnumber").val("");
                    Tenant = 0;
                    PartNo = 0;
                }
                else {
                    $("#lblType").text("");
                    $("#lblType").text("SKU");
                  //  $("#txtTenant").val("");
                  //   $("#txtWarehouse").val("");
                    $("#txtPartnumber").val("");
                    Tenant = 0;
                    PartNo = 0;
                }
            }
            $scope.clearData= function()
            {
                $("#txtTenant").val("");
                 $("#txtWarehouse").val("");
                $("#txtPartnumber").val("");
                Warehouseid = 0;
                RefTenant = "";
                Refpartno = "";
                Tenant = 0;
                PartNo = 0;
                $scope.form = { value: $scope.typeWise[0].val };
                $("#lblType").text("");
                $("#lblType").text("Invoice Number");
                tvalue = "1";
            }



            $scope.getDetails = function (pageid) {
                debugger;
                Tenant = 0;
                PartNo = 0;
                MTypeId = 0;


                if (Warehouseid == "0" || $("#txtWarehouse").val() == "" || $("#txtWarehouse").val() == null || $("#txtWarehouse").val() == undefined) {

                    showStickyToast(false, "Please select Warehouse");
                    return false;
                }

                if ($("#txtTenant").val() == "" ||RefTenant == "0" ) {
                    Tenant = 0;
                    showStickyToast(false, "Please Select Tenant", false);
                    return false;
                }
                else {
                    Tenant = RefTenant;
                }
                
                if ($("#txtPartnumber").val() == "") {
                    PartNo = 0;
                    var errmsg = "";
                    errmsg = tvalue == "1" ? "Please Select Invoice Number" : "Please Select SKU";
                    showStickyToast(false, errmsg, false);
                    return false;
                }
                else {
                    PartNo = Refpartno;
                }

                if (tvalue == "1") {
                    PartNo = "0";
                    supInvID = $("#txtPartnumber").val();
                    $scope.typeValue = tvalue;
                }
                else
                {
                    PartNo = Refpartno;
                    supInvID = "";
                   // supInvID = $("#txtPartnumber").val();
                    $scope.typeValue = tvalue;
                }

               
                $("#tbldatas").addClass("tableLoader");
                document.querySelector('#tbldatas').classList.add("tableLoader");
                var httpreq = $.ajax({
                    type: 'POST',
                    url: 'grnPending.aspx/getGRNPending',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: JSON.stringify({ Tenantid: Tenant, Mcode: PartNo, SupplierInvoiceID: supInvID, Warehouseid: Warehouseid,IsExport: 0, NoofRecords: 25, PageNo: pageid }),
                    async: false,
                    success: function (response) {
                        //debugger;
                        var dt = JSON.parse(response.d).Table;
                        if (dt == undefined || dt == null || dt.length == 0) {
                            showStickyToast(false, "No Data Found", false);
                            $scope.Data = null;
                            $scope.Totalrecords = 0;
                            document.querySelector('#tbldatas').classList.remove("tableLoader");
                            return false;
                        }
                        $scope.Data = dt;
                        $scope.Totalrecords = $scope.Data[0].TotalRecords;
                        document.querySelector('#tbldatas').classList.remove("tableLoader");
                    }
                });
            };

            $scope.exprotData = function () {
                debugger
                 Tenant = 0;
                PartNo = 0;
                MTypeId = 0;
                if ($scope.Data == null || $scope.Data == undefined) {
                    showStickyToast(false, "No Data Found", false);
                    return false;
                }

                if ($("#txtTenant").val() == "" ||RefTenant == "0" ) {
                    Tenant = 0;
                    showStickyToast(false, "Please Select Tenant", false);
                    return false;
                }
                else {
                    Tenant = RefTenant;
                }
                
                if ($("#txtPartnumber").val() == "") {
                    PartNo = 0;
                    var errmsg = "";
                    errmsg = tvalue == "1" ? "Please Select Invoice Number" : "Please Select SKU";
                    showStickyToast(false, errmsg, false);
                    return false;
                }
                else {
                    PartNo = Refpartno;
                }

                if (tvalue == "1") {
                    PartNo = "0";
                    supInvID = $("#txtPartnumber").val();
                    $scope.typeValue = tvalue;
                }
                else
                {
                    PartNo = Refpartno;
                    supInvID = "";
                    $scope.typeValue = tvalue;
                }

                if (Warehouseid == "0" || $("#txtWarehouse").val() == "" || $("#txtWarehouse").val() == null || $("#txtWarehouse").val() == undefined) {

                    showStickyToast(false, "Please select Warehouse");
                    return false;
                }
                $("#tbldatas").addClass("tableLoader");
                document.querySelector('#tbldatas').classList.add("tableLoader");

                var httpreq = {
                    method: 'POST',
                    url: 'grnPending.aspx/getGRNPending_Export',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'Tenantid': Tenant, 'Mcode': PartNo, 'SupplierInvoiceID': supInvID, 'Warehouseid': Warehouseid, 'IsExport': 1, 'NoofRecords': 200000, 'PageNo': 1 },
                    async: false
                }
                $http(httpreq).success(function (response) {
                    if (response.d == "No Data Found") {
                        showStickyToast(false, "No Data Found", false);
                        document.querySelector('#tbldatas').classList.remove("tableLoader");
                        return false;
                    }
                    else {
                        window.open('../ExcelData/' + response.d + ".xlsx");
                    }
                    document.querySelector('#tbldatas').classList.remove("tableLoader");
                })



                //var httpreq = $.ajax({
                //    type: 'POST',
                //    url: 'grnPending.aspx/getGRNPending_Export',
                //    headers: {
                //        'Content-Type': 'application/json; charset=utf-8',
                //        'dataType': 'json'
                //    },
                //    data: JSON.stringify({ Tenantid: Tenant, Mcode: PartNo, SupplierInvoiceID: supInvID, Warehouseid: Warehouseid, IsExport: 1, NoofRecords: 200000, PageNo: 1 }),
                //    async: false,
                //    success: function (response) {
                //        if (response.d == "No Data Found") {
                //            showStickyToast(false, "No Data Found", false);
                //            document.querySelector('#tbldatas').classList.remove("tableLoader");
                //            return false;
                //        }
                //        else {
                //            document.querySelector('#tbldatas').classList.remove("tableLoader");
                //            window.open('../ExcelData/' + response.d + ".xlsx");
                //        }
                //    }
                //});
            }



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
                var htype = "";
                var btype = "";
                var tableContent = "";
                $("#tbldata").empty();
                tableContent += "<table><thead><tr><th>S. No.</th>";
                if (tvalue == "1") { htype = "Doc. No."; } else { htype = "SKU"; }
                tableContent += "<th> " + htype+ "</th><th>Vehicle No.</th><th>Unloading Date </th><th>Inventory Location</th><th>Quantity</th><th>Putaway Status</th><th>Received Date</th><th>Store Ref. No.</th></tr></thead><tbody>";
                for (var i = 0; i < table.length; i++) {
                    tableContent += "<tr><td>" + (i + 1) + " </td>";
                    if (tvalue == "1") { btype = table[i].InvoiceNumber; } else { btype = table[i].MCode; }
                    if (table[i].ReceivedDate == null) { table[i].ReceivedDate = ""; } else { table[i].ReceivedDate = table[i].ReceivedDate; }
                    if (table[i].Unloadingdate == null) { table[i].Unloadingdate = ""; } else { table[i].Unloadingdate = table[i].Unloadingdate; }
                    if (table[i].VehicleNo == null) { table[i].VehicleNo = ""; } else { table[i].VehicleNo = table[i].VehicleNo; }
                    tableContent += "<td class='aligndate'>" + btype + "</td><td class='aligndate'>" + table[i].VehicleNo + "</td><td class='aligndate'>" + table[i].Unloadingdate + "</td><td class='aligndate'>" + table[i].Location + "</td><td class='aligndate'>" + table[i].Quantity + "</td><td class='aligndate'>" + table[i].PutawayStatus + "</td><td class='aligndate'>" + table[i].ReceivedDate + "</td><td class='aligndate'>" + table[i].StoreRefNo + "</td></tr>";
                }
                tableContent += "</tbody></table>";
                $("#tbldata").append(tableContent);
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
                 <span class="breadcrumbd" contenteditable="false">Grn Pending</span>
            </div>
        </div>
    </div>
<!--ends-breadcrumb-->  
   
    <div ng-app="myApp" ng-controller="grnPending" class="container">
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
        
        <div class="row m0" style="margin-left:0px; margin-right:0px;">

              
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
                     <label> <%= GetGlobalResourceObject("Resource", "Tenant")%></label>
                    <span class="errorMsg"></span>
                </div>
            </div>   
            <div class="col m3">
                <div class="flex">
                    <select id="selType" class="" required="" ng-change="changeType();"  ng-model="form.value" ng-options="t.val as t.type for t in typeWise">
                    </select>
                      <label><%= GetGlobalResourceObject("Resource", "SelectType")%></label>
                    <span class="errorMsg"></span>
                </div>
            </div>
              
            <div class="col m3">
                <div class="flex">
                    <input type="text" id="txtPartnumber" class="TextboxInventoryAuto" required="" />
                     <label id="lblType"><%= GetGlobalResourceObject("Resource", "InvoiceNumber")%> </label>
                     <span class="errorMsg"></span>
                </div>
            </div>            
        </div>

        <div class="row">
            <div class="col m12" flex end>
                <gap5></gap5>
                <button type="button" id="btnSearch" ng-click="getDetails(1)" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Search")%> <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                <a class="btn btn-primary" ng-click="exprotData()"><%= GetGlobalResourceObject("Resource", "ExportTo")%><i class="fa fa-file-excel-o" aria-hidden="true"></i></a>
                 <button type="button" id="btnClear" ng-click="clearData()" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Clear")%>  <%=MRLWMSC21Common.CommonLogic.btnfaClear %></button>               
            </div>
        </div>

        <div class="row" style="margin: 0;">
            <div class="col-sm-12 col-lg-12"  style="padding: 0px 10px;">
                <div class="divmainwidth" >
                    <table class="table-striped" id="tbldatas">
                        <thead>
                            <tr class="">
                                <th>S. No.</th>
                                <th ng-show="typeValue=='1'">Doc No.</th>
                                <th ng-show="typeValue=='2'">SKU</th>
                                <th>Vehicle No.</th>
                                <th>Unloading Date</th>
                                <th>Inventory Location</th>
                                <th number>Quantity</th>
                                <th>Putaway Status</th>
                                <th>Received Date</th>
                                <th number>Store Ref. No.</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr dir-paginate="row in Data | itemsPerPage : 10">
                                 <td>{{$index+1}}</td>
                                 <td ng-show="typeValue=='1'">{{row.InvoiceNumber}}</td>
                                 <td ng-show="typeValue=='2'">{{row.MCode}}</td>
                                 <td>{{row.VehicleNo}}</td>
                                 <td>{{row.Unloadingdate}}</td>
                                 <td>{{row.Location}}</td>
                                 <td number>{{row.Quantity}}</td>
                                 <td>{{row.PutawayStatus}}</td>
                                <%-- <td>{{row.ReceivedDate}}</td>--%>
                                <td>{{row.Unloadingdate}}</td>
                                
                                 <td number>{{row.StoreRefNo}}</td>
                            </tr>
                            <tr ng-show="Data.length == 0">
                             <%--   <td colspan="9" style="text-align: center !important;">No Data Found</td>--%>
                                   <td colspan="9" style="text-align: center !important;"><%= GetGlobalResourceObject("Resource", "NoDataFound")%> </td>
                            </tr>
                        </tbody>
                        <tfoot>
                            <tr class="" ng-show="Data.length != 0">
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
