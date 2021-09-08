<%@ Page Title="Lost And Found Report" Language="C#" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="LostAndFoundReport.aspx.cs" Inherits="MRLWMSC21.mReports.LostAndFoundReport" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MMContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#txtFromdate").datepicker({
                dateFormat: "dd-M-yy",
                maxDate: new Date(),
                onSelect: function (selected) {
                    $(this).focus();
                    $("#txttodate").datepicker("option", "minDate", selected, { dateFormate: "dd/mm/yy" })
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
        });
    </script>

    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/CommonWMS.js"></script>
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
    <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
    <link href="Scripts/Custom.css" rel="stylesheet" />
    <script src="LostAndFoundReport.js"></script>

    <script src="Scripts/FileSaver.min.js"></script>
    <script src="Scripts/html2canvas.min.js"></script>

    <script src="Scripts/jspdf.min.js"></script>
    <script src="Scripts/jspdf.plugin.autotable.js"></script>
    <script src="Scripts/tableExport.js"></script>
    <script src="Scripts/tableExport.min.js"></script>
    <script src="Scripts/xlsx.core.min.js"></script>


    <style>
  

        /*.mytableOutbound tr th {
            color: #fff !important;
        }*/
    </style>
    <div ng-app="MyApp" ng-controller="LostAndFoundReport" class="container">
        <div class="divlineheight"></div>
        <div class="">
            <div class="row mo">
                <div class="col-md-12">
                    <div class="">
                        <div class="row">
                            <div class="col m3">
                                <div class="flex">
                                    <input type="text" id="txtWarehouse" ngchange="getWarehouse()" ng-click="getWarehouse()" required="" />
                                    <label> <%= GetGlobalResourceObject("Resource", "Warehouse")%></label>
                                    <span class="errorMsg"></span>
                                </div>
                            </div>
                            <div class="col m3">
                                <div class="flex">
                                    <input type="text" id="txtFromdate" required="" />
                                      <label> <%= GetGlobalResourceObject("Resource", "StartDate")%></label>
                                    <span class="errorMsg"></span>
                                </div>
                            </div>
                            <div class="col m3">
                                <div class="flex">
                                    <input type="text" id="txttodate" required="" />
                                     <label><%= GetGlobalResourceObject("Resource", "EndDate")%></label>
                                </div>
                            </div>
                            <div class="col m3">
                                <div class="flex">
                                    <input type="text" id="txtTenant" ngchange="getTenant()" ng-click="getTenant()" required="" />
                                      <label><%= GetGlobalResourceObject("Resource", "Tenant")%> </label>
                                    <input type="hidden" id="txtTenantVal" />
                                    <span class="errorMsg"></span>
                                </div>
                            </div>
                        </div>
                        <div class="row m0">
                           <div class="col m3">
                                <div class="flex">
                                    <input type="text" id="txtSupplier" ngchange="getSupplier()" ng-click="getSupplier()" required="" />
                                     <label><%= GetGlobalResourceObject("Resource", "Supplier")%></label>
                                </div>
                            </div>
                            <div class="col m3">
                                <div class="flex">
                                    <input type="text" id="txtMaterial" ngchange="getMaterial()" ng-click="getMaterial()" required="" />
                                    <label><%= GetGlobalResourceObject("Resource", "Material")%> </label>
                                </div>
                            </div>
                            <div class="col m6">
                                <gap5></gap5>
                                <flex end><button type="button" ng-click="GetLFdetails()" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Search")%><%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                                <button type="button" ng-click="exportExcel()" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Excel")%><i class="fa fa-file-excel-o" aria-hidden="true"></i></button></flex>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <table class="table-striped" id="lostfoundtable">
                        <thead>                            
                            <tr class="">
                               <%-- <th>S.No</th>
                                <th>Date</th>
                                <th>Description</th>--%>
                                 <th><%= GetGlobalResourceObject("Resource", "SNo")%> </th>
                                <th> <%= GetGlobalResourceObject("Resource", "Date")%></th>
                                <th><%= GetGlobalResourceObject("Resource", "Description")%> </th>
                                <%--<th>Account Name</th>
                                <th>Type</th>
                                <th>Material</th>
                                <th>Location</th>--%>
                                <%--<th>Lost/Found</th>--%>
                              <%--  <th style="text-align: center !important;">Lost</th>--%>
                                  <th style="text-align: center !important;"> <%= GetGlobalResourceObject("Resource", "Lost")%></th>
                             <%--   <th style="text-align: right !important;">Found</th>--%>
                                   <th style="text-align: right !important;"> <%= GetGlobalResourceObject("Resource", "Found")%> </th>
                                <%--<th>Total</th>--%>
                              <%--  <th style="text-align: right !important;">Net Position</th>--%>
                                  <th style="text-align: right !important;"> <%= GetGlobalResourceObject("Resource", "NetPosition ")%></th>
                            </tr>
                            <tr ng-repeat="OBT in OBTable" ng-init="myVar = 0">
                                <td></td>
                                <td>{{OBT.StartDt}}</td>
                              <%--  <td>Opening Balance (OB)</td>--%>
                                  <td><%= GetGlobalResourceObject("Resource", "OpeningBalanceOB")%></td>
                                <%--<td></td>
                                <td></td>
                                <td></td>
                                <td></td>--%>
                                <td style="text-align: right"><span ng-if="OBT.LostORFound < myVar">{{OBT.LostORFound}}</span></td>
                                <td style="text-align: right"><span ng-if="OBT.LostORFound >= myVar">{{OBT.LostORFound}}</span></td>
                                <td style="text-align: right">{{OBT.LostORFound}}</td>
                            </tr>
                            <tr ng-repeat="Reports in MTable" ng-init="Type = 'VLPD'; Type1 = 'CycleCount'">
                                <td>{{$index +1}}</td>
                                <td>{{Reports.Date}}</td>
                                <td>
                                    <span ng-if="Reports.Type == Type"> 
                                        {{Reports.UserName}} Triggered Material ({{Reports.MCode}}) not found at Location {{Reports.Location}} while picking for [OBD/Internal Transfer/VLPD] ({{Reports.AccountName}}).
                                    </span>
                                    <span ng-if="Reports.Type == Type1" ng-init="myVar = 0"> 
                                        <span ng-if="Reports.LOST > myVar">{{Reports.MCode}} identified as <span style="color:red;">Lost</span> at {{Reports.Location}} during Cycle Count ({{Reports.AccountName}})</span>
                                        <span ng-if="Reports.FOUND > myVar">{{Reports.MCode}} <span style="color:#3cf33c;">Found</span> at {{Reports.Location}} during Cycle Count ({{Reports.AccountName}})</span>
                                    </span>
                                </td>
                               <%-- <td>{{Reports.AccountName}}</td>
                                <td>{{Reports.Type}}</td>
                                <td>{{Reports.MCode}}</td>
                                <td>{{Reports.Location}}</td>--%>
                                <%--<td>{{Reports.LostORFound}}</td>--%>
                                <td style="text-align: right">{{Reports.LOST}}</td>
                                <td style="text-align: right">{{Reports.FOUND}}</td>
                                <%--<td>{{Reports.Total}}</td>--%>
                                <td style="text-align: right">{{Reports.NetPosition}}</td>
                            </tr>
                            <tr ng-repeat="CBT in CBTable" ng-init="myVal = 0">
                                <td></td>
                                <td>{{CBT.CBEnd}}</td>
                                <%--<td></td>
                                <td></td>--%>
                                <td>Closing Balance (CB)</td>
                                <%--<td></td>
                                <td></td>--%>
                                <td style="text-align: right"><span ng-if="CBT.CB < myVal">{{CBT.CB}}</span></td>
                                <td style="text-align: right"><span ng-if="CBT.CB >= myVal">{{CBT.CB}}</span></td>
                                <td style="text-align: right">{{CBT.CB}}</td>
                            </tr>
                        </thead>
                    </table>
                </div>
            </div>
            <table id="tbldata"></table>
        </div>
    </div>
</asp:Content>
