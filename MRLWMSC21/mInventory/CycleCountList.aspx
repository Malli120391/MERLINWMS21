<%@ Page Title="Cycle Count List" Language="C#" MasterPageFile="~/mInventory/InventoryMaster.master" AutoEventWireup="true" CodeBehind="CycleCountList.aspx.cs" Inherits="MRLWMSC21.mInventory.CycleCountList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="InvContent" runat="server">
    <script src="Scripts/angular.min.js"></script>
    <script src="../mReports/Scripts/dirPagination.js"></script>
    <script src="../mInbound/Scripts/InventraxAjax.js"></script>
    <script src="../Scripts/CommonWMS.js"></script>
    <script src="CycleCountScripts/jquery.cookie.min.js"></script>
    <input type="hidden" value='' id="hdnViewName" />
    <input type="hidden" value='' id="hdnSp_Get" />
    <input type="hidden" value='' id="hdnSp_Set" />
    <input type="hidden" value='' id="hdnJSONMaster" />
    <input type="hidden" value='0' id="hdnCId" />

    <input type="hidden" value='1' id="hdnCreatedBy" />
    <input type="hidden" value='2018-01-04' id="hdnUpdatedOn" />       
    <input type="hidden" value='1' id="hdnUpdatedBy" />

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
        var myApp = angular.module("myApp", ['angularUtils.directives.dirPagination']);
        myApp.controller("CCList", function ($scope, $http, $window) {
            $scope.getCCListData = function () {
                var httpreq = {
                    method: 'POST',
                    url: 'CycleCountList.aspx/GetList',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'Id': 0 },
                    async: false
                }
                $http(httpreq).success(function (response) {
                    debugger
                    var dt = JSON.parse(response.d).Table;
                    $scope.CCList = dt;
                });
            };
            $scope.getCCListData();
            $scope.EditItem = function (id) {
                debugger
                window.location.href = "CycleCountDetails.aspx?parm=" + id;
            };

            $scope.create = function () {
                window.location.href = "CycleCountDetails.aspx?parm=0";
            };

            $scope.DeleteItem = function (id) {
                debugger
                if (confirm("Are you sure do you want to Delete?")) {
                    $("#divLoading").show();
                    debugger
                    var httpreq = {
                        method: 'POST',
                        url: 'CycleCountList.aspx/GetCounts',
                        headers: {
                            'Content-Type': 'application/json; charset=utf-8',
                            'dataType': 'json'
                        },
                        data: { 'CCID': id },
                        //async: false
                    }
                    $http(httpreq).success(function (response) {
                        debugger
                        var dt = response.d;
                        var InCount = eval(dt.split(',')[0]);
                        var CompCount = eval(dt.split(',')[1]);
                        if (InCount > 0 || CompCount > 0) {
                            showStickyToast(false, "Cannot delete this Cycle Count, as it is already Initiated", false);
                            $("#divLoading").hide();
                            return false;
                        }
                        else {
                            $scope.DeleteEntity(id);
                            setTimeout(function () {
                                location.reload();
                            }, 2000);
                        }
                    });
                }
            };

            $scope.DeleteEntity = function (id) {
                debugger
                 var httpreq = {
                    method: 'POST',
                    url: 'CycleCountList.aspx/DeleteItemsById',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'ID': id },
                }
                $http(httpreq).success(function (response) {
                    debugger
                    var dt = response.d;
                    if (dt == "Success") {
                        showStickyToast(true, "Deleted Successfully.", false);
                        setTimeout(function () {
                            location.reload();
                        }, 2500);
                    }
                    else {
                        $("#divLoading").hide();
                        showStickyToast(true, dt, true);
                        return false;
                    }
                });
            };
        });
    </script>
    <div class="module_yellow">
        <div class="ModuleHeader" height="35px">
            <div><a href="../Default.aspx">Home</a> <i class="material-icons">arrow_right</i> <span>Orders</span> <i class="material-icons">arrow_right</i> <span class="breadcrumbd" contenteditable="false">Cycle Count List </span></div>
        </div>
    </div>
    <div class="container" ng-app="myApp" ng-controller="CCList">
        <div class="loading" id="divLoading" style="display: none;"></div>
        <div class="row">
            <div class="col m8"></div>
            <div class="col m3">
                <div class="flex">
                    <input type="text" ng-model="searchData" required />
                    <label>Search</label>
                </div>
            </div>
            <div class="col m1">
                <div flex end>
                    <button type='button' class='btn btn-primary' ng-click='create();'>Add <i class='material-icons'>add</i></button>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col m12">
                <table class="table-striped">
                    <thead>
                        <tr>
                            <th>S.No.</th>
                            <th>Warehouse</th>
                            <th>Cycle Count Type</th>
                            <th>Cycle Count Name</th>
                            <th>User</th>
                            <th>Valid From</th>
                            <th>ValidTo</th>
                            <th>Frequency</th>
                            <th>Active</th>
                            <th>Action</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr dir-paginate="ccl in CCList|filter:searchData|itemsPerPage:25">
                            <td>{{$index+1}}</td>
                            <td>{{ccl.WHCode}}</td>
                            <td>{{ccl.CycleCountName}}</td>
                            <td>{{ccl.AccountCycleCountName}}</td>
                            <td>{{ccl.UserName}}</td>
                            <td>{{ccl.ValidFrom}}</td>
                            <td>{{ccl.ValidThru}}</td>
                            <td>{{ccl.FreequencyName}}</td>
                            <td>{{ccl.IsActive}}</td>
                            <td>
                                <i class='material-icons' ng-click="EditItem(ccl.CCM_CNF_AccountCycleCount_ID)">edit</i>&emsp;
                                <i class='material-icons' ng-click="DeleteItem(ccl.CCM_CNF_AccountCycleCount_ID)">delete</i>
                            </td>
                        </tr>
                    </tbody>
                </table>
               <p></p>
                <div class="row">
                    <div class="col m12" flex end>
                         <dir-pagination-controls direction-links="true" boundary-links="true"> </dir-pagination-controls>
                    </div>                   
                </div>
            </div>
        </div>
    </div>
    
</asp:Content>
