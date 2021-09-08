<%@ Page Title="Tenant List" Language="C#" MasterPageFile="~/mMaterialManagement/MaterialManagementMaster.master" AutoEventWireup="true" CodeBehind="TenantList.aspx.cs" Inherits="MRLWMSC21.TPL.TenantList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
  <asp:ScriptManager ID="mySManager"  EnablePageMethods="true" EnablePartialRendering="true" runat="server" SupportsPartialRendering="true">
    </asp:ScriptManager>
    <link href="../mInventory/Scripts/bootstrap.min.css" rel="stylesheet" />
    <script src="../mInventory/Scripts/angular.min.js"></script>
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>
    <script src="../mInventory/Scripts/dirPagination.js"></script>
    <link href="tpl.css" rel="stylesheet" />
    <style>
        .pagination ul {
            display: inline-block;
            padding: 0;
            margin: 0;
        }

        .pagination li {
            display: inline;
        }

            .pagination li a {
                color: black;
                margin: 0px 2px;
                box-shadow: var(--z1);
                display: inline-block !important;
                width: 20px !important;
                height: 20px !important;
                text-align: center !important;
                background: #fff;
                border-radius: 2px;
                padding: 1px;
                line-height: 20px;
                text-decoration: none;
                border: 0px !important;
            }

        .ui-button-small {
            text-decoration: none;
            font-size: 14px;
            font-weight: 600;
            color: #ffffff;
            background-color: #45b9d1;
            border: solid 1px #2CA8C2;
            padding: 3px 5px 3px 5px;
            border-radius: 4px;
            border-radius: 100%;
            color: #fff !important;
            padding: 5px 5px !important;
            text-align: center;
        }


        .pagination li.active a {
            color: white;
            display: block !important;
            border: 2px solid var(--sideNav-bg) !important;
            background-color: var(--sideNav-bg) !important;
            line-height: 15px;
            height: fit-content !important;
            position: relative;
        }

        .pagination li:hover.active a {
            background-color: #f2f2f2;
        }

        .pagination li:hover:not(.active) a {
            background-color: #f2f2f2 !important;
            color: black;
        }

        #btnEdit, #btnDelete {
            cursor: pointer;
        }

        .tooltip {
            position: absolute;
            display: none;
            z-index: 1000;
            background-color: #BDA670;
            color: white;
            border: 1px solid black;
            width: 400px;
            height: 150px;
            padding: 10px;
            border-bottom-left-radius: 10px;
            border-bottom-right-radius: 10px;
            border-top-left-radius: 10px;
            border-top-right-radius: 10px;
            overflow: auto;
        }

        .intd [tooltip]:before {
            content: attr(tooltip);
            background: #585858;
            padding: 5px 7px;
            margin-right: 10px;
            border-radius: 2px;
            color: #FFF;
            font: 500 12px Roboto;
            white-space: nowrap;
            position: absolute;
            bottom: -21%;
            right: 100%;
            visibility: hidden;
            opacity: 0;
            transition: .3s;
        }


        .intd [tooltip]:hover:before {
            visibility: visible;
            opacity: 1;
        }

        .infotip {
            display: block;
            width: 100%;
            height: 16px;
            background: transparent;
            position: relative;
        }
    </style>

      
      

                   
                    <div ng-app="TenantList" ng-controller="TenantList" class="container">
                        <div ng-show="blockUI">
                            <div style="width: 100%; height: 100vh; z-index: 999; position: fixed; top: 0; left: 0; right: 0; bottom: 0; align-items: center; display: flex; justify-content: center; background: rgba(255, 255, 255, 0.24); background: hsla(0, 0%, 100%, 0.72);">

                                <img  src="<%= ResolveUrl("~/Images/preloader.svg") %>" loader/>

                            </div>

                        </div>

                        
                        <gap5></gap5>
                   

                        <div align="text-right">
                            <div flex end>
                                <div class="flex">
                                    <input type="text" class="TextboxReport" ng-model="search" required="required" />
                                    <label>Search</label>
                                </div>
                                <div>
                                   <%-- <button type="button" class="btn btn-primary"  ng-click="GetTenantList()"> Search<i class="material-icons">search</i></button>--%>
                                    <button type="button" class="btn btn-primary" id="btnCustomer" ng-click="AddTenant()"><%= GetGlobalResourceObject("Resource", "Add")%> <i class="material-icons vl">add</i></button>
                                    
                                </div>
                            </div>
                        </div>
                     
                <table class="table-striped" id="tblTenantList" style="width:100% !important;">
                    <thead>
                        <tr class="">
                            <th><%= GetGlobalResourceObject("Resource", "TenantName")%></th>
                            <th ng-click="OrderData('CompanyDBA')"><%= GetGlobalResourceObject("Resource", "TenantCode")%><span class="fa fa-sort-amount-desc" ng-show="sortKey=='CompanyDBA'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="OrderData('CustomerSince')"><%= GetGlobalResourceObject("Resource", "CustomerSince")%><span class="fa fa-sort-amount-desc" ng-show="sortKey=='CustomerSince'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="OrderData('WHCode')"><%= GetGlobalResourceObject("Resource", "WHMapped")%> <span class="fa fa-sort-amount-desc" ng-show="sortKey=='WarehousesMapped'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="OrderData('ContractExpiredOn')"><%= GetGlobalResourceObject("Resource", "ContractExpiryOn")%> <span class="fa fa-sort-amount-desc" ng-show="sortKey=='WarehousesMapped'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="OrderData('ActiveContracts')"><%= GetGlobalResourceObject("Resource", "ActiveContracts")%>  <span class="fa fa-sort-amount-desc" ng-show="sortKey==''" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="OrderData('ContractExpires')"><%= GetGlobalResourceObject("Resource", "ContractExpiresInDays")%> <span class="fa fa-sort-amount-desc" ng-show="sortKey=='TenantRegistrationNo'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>

                            <th>
                                <span ng-show="UserRoleDataID != '4'"><%= GetGlobalResourceObject("Resource", "Edit")%></span>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        

                        
                        <tr dir-paginate ="objstr in TenantList|filter: search|itemsPerPage:25">
                            <td><span Title="{{objstr.TenantName}}">{{objstr.TenantName}}</td>
                            <td><span Title="{{objstr.CompanyDBA}}">{{objstr.CompanyDBA}}</span></td>
                            <td>{{objstr.CustomerSince}}</td>
                            <td ng-if="objstr.WHCode!=null"><span >{{objstr.WHCode}}</span></td>
                            <td class="intd" ng-if="objstr.WHCode==null"><span class="infotip" tooltip="Contract information has not been updated">{{objstr.WHCode}}</span></td>
                            <td>{{objstr.ContractExpiredOn}}</td>
                            <td>{{objstr.ActiveContracts}}</td>
                            <td><span ng-if="objstr.ContractExpires>0"> {{objstr.ContractExpires}}</span><span ng-if="objstr.ContractExpires<0"><span style="color:red">{{objstr.ContractExpires}} (Contract Expired)</span></span></td>
                           

                          
                            <td>
                                <a ng-show="UserRoleDataID != '4'" ng-click="EditData(objstr)" id="btnEdit" ng-href="" ><i class="material-icons ss">edit</i></a>
                            </td>
                           
                       </tr>

                      
            
                    </tbody>
                    <tfoot ng-show="TenantList.length==0">
                        <tr>
                            <td colspan="7">
                                <div class="text-center" style="font-size: 13px !important;">No data Found. </div>
                            </td>
                        </tr>
                    </tfoot>
                </table>
    


                    <div style="text-align:right;">
                        <dir-pagination-controls  boundary-links="true"> </dir-pagination-controls>
                    </div>
                </div>
                   
               

    <script>
        var app = angular.module('TenantList', ['angularUtils.directives.dirPagination']);
        app.controller('TenantList', function ($scope, $http) {
            //======================= Added By M.D.Prasad For View Only Condition ======================//
            $scope.UserRoleDataID = "";
            var role = '';
            debugger;
            role = '<%=UserRoledat%>';
            role = role.substring(0, role.length - 1);
            role = role.split(',');
            for (var i = 0; i < role.length; i++) {
                if ('<%=UserTypeID%>' == '3' && role[i] == '4') {
                    $scope.UserRoleDataID = role[i];
                }
            }
            //======================= Added By M.D.Prasad For View Only Condition ======================//
            $scope.OrderData = function (val) {
                debugger;
                 if ($scope.search == undefined) {
                    $scope.search = '';
                }
                if ($scope.reverse == undefined) {
                    $scope.reverse = false;
                }
                $scope.orderby = 0;
                if ($scope.reverse == true) {
                    $scope.orderby = 1;
                }
                $scope.TenantList = [];
                 $scope.blockUI = true;
                var accounts = {
                    method: 'POST',
                    url: 'TenantList.aspx/GetTenantList',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: {'Search': $scope.search,'orderbytext':val,'orderby':  $scope.orderby}
                }
                $http(accounts).success(function (response) {
                    debugger;
                     $scope.blockUI = false;
                     $scope.TenantList = null;
                    var data = JSON.parse(response.d);
                    $scope.TenantList = data.Table;

                      //$scope.sortKey = val;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa

                });
                //var AjaxCall =
                //    {
                //        method: 'Post',
                //        datatype: 'json',
                //        url: 'TenantList.aspx/GetTenantList',
                //        data: { 'Search': $scope.search,'orderbytext':val,'orderby':  $scope.orderby },
                //        headers: 'application-json; charset=utf-8'
                //    }


                //$http(AjaxCall).then(function (response) {
                //    $scope.TenantList = null;
                //    var data = JSON.parse(response.data.d);
                //    $scope.TenantList = data.Table;

                //      //$scope.sortKey = val;   //set the sortKey to the param passed
                //$scope.reverse = !$scope.reverse; //if true make it false and vice versa
                   

                //});
            }
            $scope.GetTenantList = function () {
                debugger;
                if ($scope.search == undefined) {
                    $scope.search = '';
                }
                if ($scope.reverse == undefined) {
                    $scope.reverse = false;
                }
                $scope.orderby = 0;
                if ($scope.reverse == true) {
                    $scope.orderby = 1;
                }
                $scope.TenantList = [];
                $scope.blockUI = true;
                var accounts = {
                    method: 'POST',
                    url: 'TenantList.aspx/GetTenantList',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: {'Search': $scope.search,'orderbytext':'','orderby':  $scope.orderby}
                }
                $http(accounts).success(function (response) {
        
                     $scope.blockUI = false;
                     var data = JSON.parse(response.d);
                    $scope.TenantList = data.Table;
                    //if ($scope.TenantList == undefined || $scope.TenantList == '') {
                    //     showStickyToast(false, 'No data found for given search criteria ');
                    //}
                    //else if ($scope.TenantList.length == 0) {
                    //    showStickyToast(false, 'No data found for given search criteria ');
                    //}

                });



                //var AjaxCall =
                //    {
                //        method: 'Post',
                //        datatype: 'json',
                //        url: 'TenantList.aspx/GetTenantList',
                //        data: { 'Search': $scope.search,'orderbytext':'','orderby':  $scope.orderby},
                //        headers: 'application-json; charset=utf-8'
                //    }


                //$http(AjaxCall).then(function (response) {

                //    var data = JSON.parse(response.data.d);
                //    $scope.TenantList = data.Table;
                   

                //});

            }


            $scope.EditData = function (obj) {
                window.location.href = "TenantRegistration.aspx?tid=" + obj.TenantID;
            }

            $scope.DeleteData = function (obj) {

                var AjaxCall =
                    {
                        method: 'Post',
                        datatype: 'json',
                        url: 'TenantList.aspx/DeleteTenant',
                        data: { 'StrId': obj.CustomerID },
                        headers: 'application-json; charset=utf-8'
                    }
                $http(AjaxCall).then(function (response) {

                    if (response.data.d == "success") {
                        showStickyToast(true, 'Deleted Successfully');
                        $scope.TenantList();
                    }

                })
            }

            $scope.AddTenant = function () {
                window.location.href = "TenantRegistration.aspx";
            }



            $scope.GetTenantList();
            $scope.sort = function (keyname) {
                debugger;

                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

        });



    </script>
    

</asp:Content>
