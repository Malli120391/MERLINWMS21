<%@ Page Title=".: Weekly Warehouse Occupancy Report :." Language="C#" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="WeeklyWHOccupancyReport.aspx.cs" Inherits="MRLWMSC21.mReports.WeeklyWHOccupancyReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
    <script type="text/javascript">
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
        });

    </script>
    <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
    <link href="Scripts/Custom.css" rel="stylesheet" />
    <script src="WeeklyWHOccupancyReport.js"></script>
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
    <style>
        #tbldatas {
            width: 95%;
            border: 0px !important;
            margin: 15px auto;
            border-radius: 0px !important;
            background-color: #d1dced !important;
        }

        .thalign {
            text-align: right !important;
            background: #fff;
        }

            .thalign table {
                float: right;
                margin-right: 7px;
            }

        .mytableReportHeaderTR {
            color: #000 !important;
            background-color: #fff !important;
            text-align: justify;
        }

        .module_Green {
            border-right: none !important;
            border-left: none !important;
        }

        .flex input[type="text"]:focus ~ label, input[type="text"]:valid ~ label {
            top: -1px !important;
        }

        .table-striped tr th {
            padding: 5px 10px 5px 5px !important;
            border-color: #3c3a75;
            background-color: #3c3a75;
            color: white;
            border-right: 1px solid #b5b0b0 !important;
        }

        .table-striped tr td {
            border-bottom: 1px solid #c8e8af !important;
            padding: 5px 10px 5px 5px !important;
            border-right: 1px solid #e4e3e6 !important;
        }

        .table-striped tbody tr:nth-child(even) {
            background-color: #f9fbf5;
        }

        .scrollable {
            overflow: auto;
            overflow-x: scroll;
        }

        /*table {
            white-space: nowrap;
        }*/

        .table-striped tr th {
            border-bottom: 0px !important;
        }

        .table-striped tr th {
            padding: 5px 10px 5px 5px !important;
        }

            .table-striped tr th:first-of-type, td:first-of-type {
                border-left: 1px solid #c8e8af !important;
            }

        .tblScroll::-webkit-scrollbar {
            height: 10px;
            width: 4px;
            background: #e8e7fb;
            border-radius: 10px;
        }

        .tblScroll::-webkit-scrollbar-thumb:horizontal {
            background: #3c3a75;
            border-radius: 10px;
        }
            
    </style>
    <link href="../Content/app.css" rel="stylesheet" />

    <div ng-app="myApp" ng-controller="WeeklyWHOccupancyReport" class="pagewidth">
        <gap></gap>
         <div ng-show="blockUI">
            <div style="width: 100%; height: 100vh; z-index: 999; position: fixed; top: 0; left: 0; right: 0; bottom: 0; align-items: center; display: flex; justify-content: center;">
                <div style="align-self: center;">
                    <img src="<%= ResolveUrl("~/Images/preloader.svg") %>" loader/>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col m12">
                <div class="col m3">
                    <div class="flex">
                        <input type="text" id="txtTenant" ngchange="getTenant()" ng-click="getTenant()" class="TextboxInventoryAuto" style="margin-bottom: 0px !important;" required="required" />
                        <label>Tenant</label>
                    </div>
                </div>
                <div class="col m3">
                    <div class="flex">
                        <input type="text" id="txtWarehouse" class="TextboxInventoryAuto" style="margin-bottom: 0px !important;" required="required" />
                        <label>Warehouse</label>
                    </div>
                </div>

                <div class="col m3">
                    <div class="flex">
                        <input type="text" class="" onpaste="return false;" ng-model="fromdate" id="txtFromdate" required="" />
                        <label><%= GetGlobalResourceObject("Resource", "FromDate")%> </label>
                        <span class="errorMsg"></span>
                    </div>
                </div>
                <div class="col m3">
                    <div class="flex">
                        <input type="text" class="" onpaste="return false;" ng-model="todate" id="txttodate" required="" /><label><%= GetGlobalResourceObject("Resource", "ToDate")%> </label>
                        <span class="errorMsg"></span>
                    </div>
                </div>               
            </div>
        </div>

        <div class="row">
             <div class="col m12" flex end>
                    <button type="button" ng-click="Getgedetails()" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "ViewReport")%> <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                </div>
        </div>

        
        <div class="row">
            <div class="col m12">
                <div style="width: 100%;overflow: auto;" class="tblScroll">
                    <table class="table-striped">
                        <thead>
                            <tr>
                                <th style="padding-left: 5px !important;" rowspan="2">Company Name</th>
                                <th style="padding-left: 5px !important;" colspan="3">Week-1</th>
                                <th style="padding-left: 5px !important;" colspan="3">Week-2</th>
                                <th style="padding-left: 5px !important;" colspan="3">Week-3</th>
                                <th style="padding-left: 5px !important;" colspan="3">Week-4</th>
                                <th style="padding-left: 5px !important;" colspan="3">Week-5</th>
                                <th style="padding-left: 5px !important;" rowspan="2">Total Occupied %</th>
                                <th style="padding-left: 5px !important;" rowspan="2">Total VAS Charges</th>
                                <th style="padding-left: 5px !important;" rowspan="2">Total Charges</th>
                            </tr>
                            <tr>
                                <th style="padding-left: 5px !important; background-color: #e8e7fb; color: black;">Total CBM</th>
                                <th style="padding-left: 5px !important; background-color: #e8e7fb; color: black;">Charges in KD</th>
                                <th style="padding-left: 5px !important; background-color: #e8e7fb; color: black;">Occupied %</th>

                                <th style="padding-left: 5px !important;">Total CBM</th>
                                <th style="padding-left: 5px !important;">Charges in KD</th>
                                <th style="padding-left: 5px !important;">Occupied %</th>

                                <th style="padding-left: 5px !important; background-color: #e8e7fb; color: black;">Total CBM</th>
                                <th style="padding-left: 5px !important; background-color: #e8e7fb; color: black;">Charges in KD</th>
                                <th style="padding-left: 5px !important; background-color: #e8e7fb; color: black;">Occupied %</th>

                                <th style="padding-left: 5px !important;">Total CBM</th>
                                <th style="padding-left: 5px !important;">Charges in KD</th>
                                <th style="padding-left: 5px !important;">Occupied %</th>

                                <th style="padding-left: 5px !important; background-color: #e8e7fb; color: black;">Total CBM</th>
                                <th style="padding-left: 5px !important; background-color: #e8e7fb; color: black;">Charges in KD</th>
                                <th style="padding-left: 5px !important; background-color: #e8e7fb; color: black;">Occupied %</th>
                            </tr>
                        </thead>

                        <tbody>
                            <tr dir-paginate="dt in TenantData | itemsPerPage : 10">
                                <td style="padding-left: 5px !important;">{{dt.TenantName}}</td>
                                <td style="padding-left: 5px !important;">{{getData('WEEK-1',dt.TenantID,'Charge') | number : 3}}</td>
                                <td style="padding-left: 5px !important;">{{getData('WEEK-1',dt.TenantID,'OccupiedCBM') | number : 4}}</td>
                                <td style="padding-left: 5px !important;">{{getData('WEEK-1',dt.TenantID,'OccupiedCBMPercentage') | number : 6}}</td>

                                <td style="padding-left: 5px !important;">{{getData('WEEK-2',dt.TenantID,'Charge') | number : 3}}</td>
                                <td style="padding-left: 5px !important;">{{getData('WEEK-2',dt.TenantID,'OccupiedCBM') | number : 4}}</td>
                                <td style="padding-left: 5px !important;">{{getData('WEEK-2',dt.TenantID,'OccupiedCBMPercentage') | number : 6}}</td>

                                <td style="padding-left: 5px !important;">{{getData('WEEK-3',dt.TenantID,'Charge') | number : 3}}</td>
                                <td style="padding-left: 5px !important;">{{getData('WEEK-3',dt.TenantID,'OccupiedCBM') | number : 4}}</td>
                                <td style="padding-left: 5px !important;">{{getData('WEEK-3',dt.TenantID,'OccupiedCBMPercentage') | number : 6}}</td>

                                <td style="padding-left: 5px !important;">{{getData('WEEK-4',dt.TenantID,'Charge') | number : 3}}</td>
                                <td style="padding-left: 5px !important;">{{getData('WEEK-4',dt.TenantID,'OccupiedCBM') | number : 4}}</td>
                                <td style="padding-left: 5px !important;">{{getData('WEEK-4',dt.TenantID,'OccupiedCBMPercentage') | number : 6}}</td>

                                <td style="padding-left: 5px !important;">{{getData('WEEK-5',dt.TenantID,'Charge') | number : 3}}</td>
                                <td style="padding-left: 5px !important;">{{getData('WEEK-5',dt.TenantID,'OccupiedCBM') | number : 4}}</td>
                                <td style="padding-left: 5px !important;">{{getData('WEEK-5',dt.TenantID,'OccupiedCBMPercentage') | number : 6}}</td>

                                <td style="padding-left: 5px !important;">{{getOccPercentage(dt.TenantID,'OccupiedCBMPercentage') | number : 6}}</td>
                                <td style="padding-left: 5px !important;">{{getData('Tariffs',dt.TenantID,'Charge') | number : 3}}</td>
                                <td style="padding-left: 5px !important;">{{getTotalCharges(dt.TenantID,'Charge') | number : 3}}</td>

                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <div flex end>
            <dir-pagination-controls direction-links="true" boundary-links="true"></dir-pagination-controls>
        </div>
    </div>
</asp:Content>
