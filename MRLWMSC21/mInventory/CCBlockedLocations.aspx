<%@ Page Title="Cycle Count Blocked Locations" Language="C#" MasterPageFile="~/mInventory/InventoryMaster.master" AutoEventWireup="true" CodeBehind="CCBlockedLocations.aspx.cs" Inherits="MRLWMSC21.mInventory.CCBlockedLocations" %>

<asp:Content ID="Content1" ContentPlaceHolderID="InvContent" runat="server">

    <script src="Scripts/angular.min.js"></script>
    <script src="../mReports/Scripts/dirPagination.js"></script>
     <style>
         /* Absolute Center Spinner */
        .loading
        {
            position: fixed;
            z-index: 9999;
            height: 2em;
            width: 2em;
            overflow: show;
            margin: auto;
            top: 0;
            left: 0;
            bottom: 0;
            right: 0;
        }

            /* Transparent Overlay */
            .loading:before
            {
                content: '';
                display: block;
                position: fixed;
                top: 0;
                left: 0;
                width: 100%;
                height: 100%;
                background-color: rgba(0,0,0,0.3);
            }

            /* :not(:required) hides these rules from IE9 and below */
            .loading:not(:required)
            {
                /* hide "loading..." text */
                font: 0/0 a;
                color: transparent;
                text-shadow: none;
                background-color: transparent;
                border: 0;
            }

                .loading:not(:required):after
                {
                    content: '';
                    display: block;
                    font-size: 10px;
                    width: 1em;
                    height: 1em;
                    margin-top: -0.5em;
                    -webkit-animation: spinner 1500ms infinite linear;
                    -moz-animation: spinner 1500ms infinite linear;
                    -ms-animation: spinner 1500ms infinite linear;
                    -o-animation: spinner 1500ms infinite linear;
                    animation: spinner 1500ms infinite linear;
                    border-radius: 0.5em;
                    -webkit-box-shadow: rgba(0, 0, 0, 0.75) 1.5em 0 0 0, rgba(0, 0, 0, 0.75) 1.1em 1.1em 0 0, rgba(0, 0, 0, 0.75) 0 1.5em 0 0, rgba(0, 0, 0, 0.75) -1.1em 1.1em 0 0, rgba(0, 0, 0, 0.5) -1.5em 0 0 0, rgba(0, 0, 0, 0.5) -1.1em -1.1em 0 0, rgba(0, 0, 0, 0.75) 0 -1.5em 0 0, rgba(0, 0, 0, 0.75) 1.1em -1.1em 0 0;
                    box-shadow: rgba(0, 0, 0, 0.75) 1.5em 0 0 0, rgba(0, 0, 0, 0.75) 1.1em 1.1em 0 0, rgba(0, 0, 0, 0.75) 0 1.5em 0 0, rgba(0, 0, 0, 0.75) -1.1em 1.1em 0 0, rgba(0, 0, 0, 0.75) -1.5em 0 0 0, rgba(0, 0, 0, 0.75) -1.1em -1.1em 0 0, rgba(0, 0, 0, 0.75) 0 -1.5em 0 0, rgba(0, 0, 0, 0.75) 1.1em -1.1em 0 0;
                }

        /* Animation */

        @-webkit-keyframes spinner
        {
            0%
            {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100%
            {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        @-moz-keyframes spinner
        {
            0%
            {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100%
            {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        @-o-keyframes spinner
        {
            0%
            {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100%
            {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }

        @keyframes spinner
        {
            0%
            {
                -webkit-transform: rotate(0deg);
                -moz-transform: rotate(0deg);
                -ms-transform: rotate(0deg);
                -o-transform: rotate(0deg);
                transform: rotate(0deg);
            }

            100%
            {
                -webkit-transform: rotate(360deg);
                -moz-transform: rotate(360deg);
                -ms-transform: rotate(360deg);
                -o-transform: rotate(360deg);
                transform: rotate(360deg);
            }
        }
    </style>
   
    <script>
        var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
        myApp.controller('CCBlockedLocations', function ($scope, $http) {

            var CycleCountID = 0, CCTRNID = 0;
            CycleCountID = new URL(window.location.href).searchParams.get("param");
            CCTRNID = new URL(window.location.href).searchParams.get("param1");
            if (CycleCountID == 0 || CycleCountID == null) {
                CycleCountID = 0;
                CCTRNID = 0;
            }
            else {
                CycleCountID = CycleCountID;
                CCTRNID = CCTRNID;
            }
            $scope.getLocationData = function () {
                //debugger
                $("#divLoading").show();
                var httpreq = {
                    method: 'POST',
                    url: 'CCBlockedLocations.aspx/GetList',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'ACID': CycleCountID, 'CCTID': CCTRNID },
                }
                $http(httpreq).success(function (response) {
                    debugger
                    var dt = JSON.parse(response.d).Table;
                    if (dt != null && dt.length > 0) {
                        $scope.locationData = dt;
                        var headerData = JSON.parse(response.d).Table1[0];
                        $("#spnCycleCount").text(headerData.CycleCountName);
                        $("#spnCycleCountName").text(headerData.AccountCycleCountName);
                        $("#divLoading").hide();
                    }
                    else {
                        $scope.locationData = null;
                        $("#spnCycleCount").text("");
                        $("#spnCycleCountName").text("");
                        $("#divLoading").hide();
                        showStickyToast(false, "No Data Found");
                        return false;
                    }
                });
            };
            $scope.getLocationData();
        });
    </script>
    <div class="module_yellow">
        <div class="ModuleHeader" height="35px">
            <div><a href="../Default.aspx">Home</a> <i class="material-icons">arrow_right</i> <span>Orders</span> <i class="material-icons">arrow_right</i> <span class="breadcrumbd" contenteditable="false">Cycle Count Blocked Locations</span></div>
        </div>
    </div>
    <div ng-app="myApp" ng-controller="CCBlockedLocations" class="container">
        <div class="loading" id="divLoading" style="display: none;"></div> 
        <div class="row">
            <div class="col m3">
                <br />
                <b>Cycle Count : </b>&emsp;<span class="cycleData" id="spnCycleCount"></span>
            </div>
            <div class="col m4">
                <br />
                <b>Cycle Count Name : </b>&emsp;<span class="cycleData" id="spnCycleCountName"></span>
            </div>
            <div class="col m3">
                <div class="flex">
                    <input type="text" id="txtLocation" ng-model="serachData" required="" />
                    <label>Location</label>
                </div>
            </div>
            <div class="col m3" flex end hidden>
                <gap5></gap5>
                <button type="button" ng-click="getLocationData()" style="display:none !important;" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Search")%>  <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
            </div>

        </div>
        
        <div class="row">
            <div class="col m12">
                <table class="table-striped" id="tbldatas">
                    <thead>
                        <tr>
                            <th>Location</th>                            
                            <th center>CC Activity Time</th>
                            <th center>Scanned</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr dir-paginate="loc in locationData|filter : serachData |itemsPerPage:25">
                            <td>{{loc.Location}}</td>
                            <td style="text-align: center !important;">{{loc.CreatedOn}}</td>
                            <td style="text-align: center !important;color:#03bd1a !important;font-weight:bold !important;" ng-show="loc.IsScanned=='Yes'">Yes</td>        
                            <td style="text-align: center !important;color:red !important;font-weight:bold !important;" ng-show="loc.IsScanned=='No'">No</td>        
                        </tr>
                    </tbody>
                </table>
            </div>
        </div><p></p>
        <div class="row">
            <div class="col m12" flex end>
                <dir-pagination-controls direction-links="true" boundary-links="true"> </dir-pagination-controls>
            </div>
        </div>
        </div>
</asp:Content>
