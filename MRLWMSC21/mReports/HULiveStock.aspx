<%@ Page Title="HU Live Stock" Language="C#" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="HULiveStock.aspx.cs" Inherits="MRLWMSC21.mReports.HULiveStock" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {

            $("#txtMfgDate").datepicker({
                dateFormat: "dd-M-yy",
                maxDate: new Date(),
                onSelect: function (selected) {
                    $(this).focus();
                    $("#txtExpDate").datepicker("option", "minDate", selected, { dateFormate: "dd-M-yy" })
                }
            });
            $("#txtExpDate").datepicker({
                dateFormat: "dd-M-yy",
            });

            $('#txtMfgDate, #txtExpDate').keypress(function () {
                return false;
            });
        });

    </script>

    <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
    <link href="Scripts/Custom.css" rel="stylesheet" />
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
    <script src="HULiveStock.js"></script>

    <div ng-app="myApp" ng-controller="HULiveStock" class="container">

        <div class="divlineheight"></div>
        <input type="hidden" ng-model="tenantid" id="hdnTenantID" />
        <div ng-show="blockUI">
            <div style="width: 100%; height: 100vh; z-index: 999; position: fixed; top: 0; left: 0; right: 0; bottom: 0; align-items: center; display: flex; justify-content: center; background: rgba(255, 255, 255, 0.24); background: hsla(0, 0%, 100%, 0.72);">
                <div style="align-self: center;">
                    <img width="60" src="../Images/preloader.svg" />
                </div>
            </div>
        </div>
        <div class="row">
             <div class="col m3">
                        <div class="flex">
                            <input type="text" id="txtWarehouse" required />
                            <label>Warehouse</label>
                             <span class="errorMsg"></span>
                        </div>
                    </div>
                    <div class="col m3">
                        <div class="flex">
                            <input type="text" id="txtTenant" required/>
                            <label>Tenant</label>
                            <span class="errorMsg"></span>
                        </div>
                    </div>
                     
                    <div class="col m3">
                        <div class="flex">
                            <input required type="text" id="txtPartNo" />
                            <label>MCode</label>
                        </div>
                    </div>
                    <div class="col m3">
                        <div class="flex">
                            <input type="text" id="txtBatchNo" required/>
                            <label>Batch No.</label>
                        </div>
                    </div>

        </div>

        <div class="row">

             <div class="col m3">
                        <div class="flex">
                            <input type="text" id="txtSerialNo" required/>
                            <label>Serial No.</label>
                        </div>
                    </div>
            <div class="col m3">
                <div class="flex">
                    <input type="text" required="" id="txtMfgDate" />
                    <label>Mfg Date</label>
                </div>
            </div>
             <div class="col m3">
                <div class="flex">
                    <input type="text" required="" id="txtExpDate" />
                    <label>Exp Date</label>
                </div>
            </div>   
             <div class="col m3">
                        <div class="flex">
                            <input type="text" id="txtProjectRefNo" required/>
                            <label>Project Ref. No.</label>
                        </div>
                    </div>                 

            
        </div>

        <div class="row">
             <div class="col m3">
                        <div class="flex">
                            <input type="text" id="txtMRP" required/>
                            <label>MRP</label>
                        </div>
                    </div>
             <div class="col m3">
                        <div class="flex">
                            <select id="selReportType">
                                <option value="1">Consolidated</option>
                                <option value="2" selected>HUSize</option>
                            </select>
                            <label>Report Type</label>
                        </div>
                    </div>

            <div class="col m3">
                <gap5></gap5>
                <flex>  
                    <button type="button" ng-click="getDetails(1)" class="btn btn-primary">Search <i class="fa fa-search" aria-hidden="true"></i></button>
                    <button type="button" class="btn btn-primary" ng-click="getDetailsExport()">Export <i class="fa fa-file-excel-o" aria-hidden="true"></i></button>
                </flex>
            </div>
        </div>

        <div class="row">
            <div class="col m12">
                 <table class=" table-striped">
                        <thead>
                            <tr>
                                <th>Warehouse</th>
                                <th>Tenant</th>
                                <th>MCode</th>
                                <th number>HU Size</th>
                                <th number>HU NO</th>
                                <th>Batch No.</th>
                                <th>Serial No.</th>
                                <th>Mfg. Date</th>
                                <th>Exp. Date</th>
                                <th>Project Ref. No.</th>
                                <th>MRP</th>
                                <th ng-show="ReportType == '2'">Location</th>
                                <th ng-show="ReportType == '2'">Container</th>
                                <th ng-show="ReportType == '2'">S. Loc</th>
                                <th number>Qty.</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr dir-paginate="HU in HUStock|itemsPerPage:25" total-items="Totalrecords">
                                <td>{{HU.WHCode}}</td>
                                <td>{{HU.TenantCode}}</td>
                                <td>{{HU.MCode}}</td>
                                <td number>{{HU.HUSize}}</td>
                                <td number>{{HU.HUNO}}</td>
                                <td>{{HU.BatchNo}}</td>
                                <td>{{HU.SerialNo}}</td>
                                <td>{{HU.MfgDate}}</td>
                                <td>{{HU.ExpDate}}</td>
                                <td>{{HU.ProjectRefNo}}</td>
                                <td>{{HU.MRP}}</td>
                                <td ng-show="ReportType == '2'">{{HU.Location}}</td>
                                <td ng-show="ReportType == '2'">{{HU.CartonCode}}</td>
                                <td ng-show="ReportType == '2'">{{HU.StorageLocation}}</td>
                                <td number>{{HU.Qty}}</td>
                            </tr>
                        </tbody>
                        
                    </table>
            </div>
        </div>
        <div class="row">
            <div class="col m12">
                 <div flex end><dir-pagination-controls direction-links="true" boundary-links="true" on-page-change="getDetails(newPageNumber)"></dir-pagination-controls></div>
            </div>
        </div>
    </div>
</asp:Content>
