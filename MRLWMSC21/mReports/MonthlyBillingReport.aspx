<%@ Page Title="" Language="C#" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="MonthlyBillingReport.aspx.cs" Inherits="MRLWMSC21.mReports.MonthlyBillingReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
    <script src="MonthlyBillingReport.js"></script>
    <link href="../Scripts/bootstrap/bootstrap.min.css" rel="stylesheet" />
    <script src="Scripts/jquery-ui-1.8.24.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <%--<script src="Scripts/docraptor-1.0.0.js"></script>--%>
    <script src="Scripts/html2canvas.min.js"></script>
    <script src="Scripts/pdfmake.min.js"></script>
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>

    <script type="text/javascript">
        var RefTenant = '';
        var WarehouseID = '';
        $(document).ready(function () {

            $("#txtFromdate").datepicker({
                dateFormat: "dd-M-yy",
                maxDate: new Date(),
                onSelect: function (selected) {
                    $(this).focus();
                    $("#txttodate").datepicker("option", "minDate", selected, { dateFormate: "dd-M-yy" })
                }
            });
            $("#txttodate").datepicker({
                dateFormat: "dd-M-yy",
                maxDate: new Date(),
                onSelect: function (selected) {
                    $(this).focus();
                }
            });

            $('#txtFromdate, #txttodate').keypress(function () {
                return false;
            });


            // $scope.getTenant = function () {

            //alert('11');
              debugger;
            $('#txtTenant').val("");

            var textfieldname = $("#txtTenant");
            DropdownFunction(textfieldname);
            $("#txtTenant").autocomplete({
                source: function (request, response) {
                    $.ajax({
                       // url: '../mWebServices/FalconWebService.asmx/LoadTenantsForReports',
                       // data: "{ 'prefix': '" + request.term + "'}",
                        url: '../mWebServices/FalconWebService.asmx/LoadTenantsByWH',
                        data: "{ 'prefix': '" + request.term + "','whid':'"+WarehouseID+"' }",
                        
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {

                            debugger;
                            // $scope.Tenantdata = data;
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
                    RefTenant = i.item.val;
                    // alert(Refnumber);
                    // $scope.ngtenant = i.item.val;
                },
                minLength: 0
            });

            var textfieldname = $("#txtWarehouse");
            DropdownFunction(textfieldname);

            $("#txtWarehouse").autocomplete({
                source: function (request, response) {
                    debugger;
                    $.ajax({
                       // url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadWarehousesBasedonTenant") %>',
                        //data: JSON.stringify({ 'prefix': request.term, 'tenantID': RefTenant }),
                         url: '../mWebServices/FalconWebService.asmx/LoaDWHListBasedonUser',
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

                    // $("#hifwarehouse").val(i.item.val);
                    //alert($("#hifToteTypeID").val());
                    WarehouseID = i.item.val
                    $('#txtTenant').val("");
                    $('#txttodate').val("");
                    $('#txtFromdate').val("");
                },
                minLength: 0
            });
        });

    </script>

    <style>
        .set {
            float: right;
        }

        .pdf {
            font-size: 20px !important;
        }

            .pdf table thead tr th {
                font-size: 20px !important;
            }

            .pdf table tbody tr td {
                font-size: 20px !important;
            }

            .pdf p {
                font-size: 20px !important
            }

            .pdf h4 {
                font-size: 25px !important
            }

        #BillTo {
            width: 40%;
            border: 1px solid #e5e5e5;
            margin-right: 10px;
            padding: 0px 10px;
        }

        .heading {
            text-align: center
        }

        .display {
            display: flex;
            justify-content: space-between;
        }

        .v-table {
            border: 1px solid #ddd;
            width: 60%;
        }

            .v-table td {
                position: relative;
                border-bottom: 1px solid #f5f5f5;
                empty-cells: show;
                padding: 9px 0 9px 12px;
                vertical-align: top;
                font-weight: normal;
                text-align: left;
                font-size: 11.5px !important;
                border-right: 1px solid #e5e5e5;
            }

        .table-striped ~ p {
            text-align: right;
            font-weight: bold;
        }

        .temp {
            display: none
        }
    </style>
    <div ng-app="myApp" ng-controller="BillingReport" class="pagewidth">
        <div ng-show="blockUI">
            <div style="width: 100%; height: 100vh; z-index: 999; position: fixed; top: 0; left: 0; right: 0; bottom: 0; align-items: center; display: flex; justify-content: center;">

                <div style="align-self: center;">
                    <img src="<%= ResolveUrl("~/Images/preloader.svg") %>" loader />

                </div>

            </div>

        </div>
        <div class="divlineheight"></div>
        <div>
            <div class="divlineheight"></div>
            <div class="row">
                <div class="">
                    <div class="">
                         <div class="col m2">
                            <div class="flex">
                                <input type="text" id="txtWarehouse" class="TextboxInventoryAuto" style="margin-bottom: 0px !important;" required="required" /><span class="errorMsg"></span>
                                <label>Warehouse</label>
                            </div>
                        </div>

                        <div class="col m2">
                            <div class="flex">
                                <input type="text" id="txtTenant" ngchange="getTenant()" ng-click="getTenant()" class="TextboxInventoryAuto" style="margin-bottom: 0px !important;" required="required" /><span class="errorMsg"></span>
                                <label>Tenant</label>
                            </div>
                        </div>
                       

                        <div class="col m2">
                            <div class="flex">
                                <input type="text" class="" onpaste="return false;" ng-model="fromdate" id="txtFromdate" required="" />
                                <label><%= GetGlobalResourceObject("Resource", "FromDate")%> </label>
                                <span class="errorMsg"></span>
                            </div>
                        </div>
                        <div class="col m2">
                            <div class="flex">
                                <input type="text" class="" onpaste="return false;" ng-model="todate" id="txttodate" required="" /><label><%= GetGlobalResourceObject("Resource", "ToDate")%> </label>
                                <span class="errorMsg"></span>
                            </div>
                        </div>
                        <div class="col m4">
                            <gap></gap>
                            <button type="button" ng-click="ViewReport()" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "ViewReport")%> <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                            <button type="button" style="display: none;" class="btn btn-primary"><span id="btnPdf" onclick="downloadPDF()"><i class="fa fa-file-pdf-o" aria-hidden="true"></i>&nbsp;&nbsp;<%= GetGlobalResourceObject("Resource", "DowndloadPDF")%> </span></button>
                            <button type="button" class="btn btn-primary" ng-click="generatePDF()">Export PDF<i class="fa fa-file-pdf-o" aria-hidden="true"></i></button>
                        </div>
                    </div>

                </div>

            </div>

        </div>
        <div id="Datapdf" ng-show="billDetails">
            <div class="display">
                <div id="BillTo">
                    <%--     <h4>Bill To</h4>--%>
                    <h4><%= GetGlobalResourceObject("Resource", "BillTo")%></h4>
                    {{Data.Table5[0].TenantName}}<br />
                    {{Data.Table5[0].Address1}}<br />
                    {{Data.Table5[0].Address2}}<br />
                    {{Data.Table5[0].City}}<br />
                    {{Data.Table5[0].State}}<br />
                    {{Data.Table5[0].ZIP}}
                </div>

                <div class="v-table">
                 <%--   <h4><%= GetGlobalResourceObject("Resource", "BillTo")%></h4>
                    {{Data.Table5[0].TenantName}}<br />
                    {{Data.Table5[0].Address1}}<br />
                    {{Data.Table5[0].Address2}}<br />
                    {{Data.Table5[0].City}}<br />
                    {{Data.Table5[0].State}}<br />
                    {{Data.Table5[0].ZIP}}--%>
                </div>
            </div>


            <div id="inbBill">
                <h4 class="heading"><%= GetGlobalResourceObject("Resource", "InboundBillingReport")%> </h4>
                <div>
                    <table class="table-striped">
                        <thead>
                            <tr>
                                <th>ItemName </th>
                                <th>Price</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr ng-repeat="t in  Data.Table">
                                <td>{{t.RateName}}</td>
                                <td style="text-align:right">{{t.Rate}}</td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align:right"> <p>Total: {{Total1}}</p></td>
                            </tr>
                        </tbody>
                    </table>
                      
                </div>
            </div>
            <br />
            <div id="ObdBill">
                <h4 class="heading"><%= GetGlobalResourceObject("Resource", "OutboundBillingReport")%></h4>
                <div>
                    <table class="table-striped">
                        <thead>
                            <tr>
                                <th>ItemName </th>
                                <th>Price</th>
                            </tr>

                        </thead>
                        <tbody>
                            <tr ng-repeat="t in  Data.Table1">
                                 <td>{{t.RateName}}</td>
                                <td style="text-align:right">{{t.Rate}}</td>
                            </tr>
                             <tr>
                                <td colspan="2" style="text-align:right"> <p>Total: {{Total2}}</p></td>
                            </tr>

                        </tbody>
                    </table>
                       
                </div>
                <br />
                <br />
                <div>
                </div>
                <br />
             
            </div>
               <div id="StBill">
                    <h4 class="heading"><%= GetGlobalResourceObject("Resource", "StorageBillingReport")%> </h4>
                    <table class="table-striped">
                        <thead>
                            <tr>
                                <th>ItemName </th>
                                <th>Price</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr ng-repeat="t in  Data.Table2">
                                  <td>{{t.RateName}}</td>
                                <td style="text-align:right">{{t.Rate}}</td>
                            </tr>
                             <tr>
                                <td colspan="2" style="text-align:right"> <p>Total: {{Total3}}</p></td>
                            </tr>
                        </tbody>
                    </table>
                       
                </div>
                  <div id="othBill">
                    <h4 class="heading">Other Charges</h4>
                    <table class="table-striped">
                        <thead>
                            <tr>
                                <th>ItemName </th>
                                <th>Price</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr ng-repeat="t in  Data.Table3">
                                  <td>{{t.RateName}}</td>
                                <td style="text-align:right">{{t.Rate}}</td>
                            </tr>
                              <tr>
                                <td colspan="2" style="text-align:right"> <p>Total: {{Total4}}</p></td>
                            </tr>
                        </tbody>
                    </table>
                        
                </div>
            <div style="text-align:center">
                  <h1><b>Grand Total : {{GrandTotal}}</b>
              </h1>

            </div>
                
            <div id="pdfdiv" class="pdf temp">
            </div>
</asp:Content>
