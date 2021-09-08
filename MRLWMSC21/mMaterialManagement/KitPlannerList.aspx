<%@ Page Title=" Kit Planner List :." Language="C#" MasterPageFile="~/mMaterialManagement/MaterialManagementMaster.master" AutoEventWireup="true" CodeBehind="KitPlannerList.aspx.cs" Inherits="MRLWMSC21.mMaterialManagement.KitPlannerList" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MMContent" runat="server">

   <%-- <asp:ScriptManager ID="mySManager" runat="server" EnablePartialRendering="true" SupportsPartialRendering="true" />--%>
    <script src="../Scripts/angular.min.js"></script>
    <script src="../mReports/Scripts/dirPagination.js"></script>
    <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />
     <link href="../mInventory/Scripts/bootstrap.min.css" rel="stylesheet" />
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>
    <style>
        .table-striped tr{
            height:unset !important;
        }
    </style>
    <script>
        var app = angular.module('KitListMyApp', ['angularUtils.directives.dirPagination']);


        app.controller('Kitlist', function ($scope, $http, $timeout) {
            $scope.changemenulink = function () {
                window.location.href = '../mMaterialManagement/KitPlannerRequest.aspx';
            }
            $scope.search = new KitListSearch('0', '', '', '0', '', '0');


            var textfieldname = $("#txtTenant");
            DropdownFunction(textfieldname);
            $("#txtTenant").autocomplete({
                source: function (request, response) {
                    debugger;
                    if ($scope.search.Tenant == '' || $scope.search.Tenant == undefined) {
                        $scope.search.TenantId = 0;
                    }
                    $scope.search.TenantId = 0;
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
                    $scope.search.TenantId = 0;
                    $scope.search.TenantId = i.item.val;
                    // alert(Refnumber);
                    //$scope.ngtenant = i.item.val;
                },
                minLength: 0
            });

            var textfieldname = $("#txtPartNo");
            DropdownFunction(textfieldname);
            $("#txtPartNo").autocomplete({
                source: function (request, response) {
                    if ($scope.search.TenantId == '' || $scope.search.TenantId == undefined) {
                        showStickyToast(false, "Please select Tenant ");
                        return false;
                    }
                    if ($scope.search.PartNo == '' || $scope.search.PartNo == undefined) {
                        $scope.search.MMID = 0;
                    }
                     $scope.search.MMID = 0;
                    $.ajax({
                        url: '../mWebServices/FalconWebService.asmx/getPartNOForKits',
                        data: "{ 'prefix': '" + request.term + "','TenantID':'" + $scope.search.TenantId + "'}",
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
                    $scope.search.MMID = 0;
                    $scope.search.MMID = i.item.val;

                },
                minLength: 0
            });



          






            $scope.getKitHeaderData = function () {
                debugger;

                if ($('#txtTenant').val() != "") {
                    if ($scope.search.TenantId == 0) {

                        showStickyToast(false, "Please select Tenant", false);
                        return false;
                    }
                }
                if ($('#txtPartNo').val() != "") {
                    if ($scope.search.MMID == 0) {

                        showStickyToast(false, "Please select Part No.", false);
                        return false;
                    }
                }
                var data = $scope.search;
                var accounts = {
                    method: 'POST',
                    url: '../mMaterialManagement/KitPlannerList.aspx/GetKitPlannerList',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'obj': data }
                }
                $http(accounts).success(function (response) {
                    debugger;

                    $scope.KitList = response.d;
                    //if ($scope.KitList == undefined || $scope.KitList == null || $scope.KitList == "" || $scope.KitList.length == 0) {
                    //    showStickyToast(false, "No Data found for given search criteria");
                    //    return false;
                    //}
                    console.log($scope.KitList);

                });

            }
            $scope.getKitHeaderData();
        });

        function KitListSearch(tenantid, tenant, partno, mmid, Kitcode, kitid) {
            this.TenantId = tenantid;
            this.Tenant = tenant;
            this.PartNo = partno;
            this.MMID = mmid;
            this.KitCode = Kitcode;
            this.KitId = kitid;
        }
    </script>

    <div class="dashed"></div>

   <div class="container">
    <div class="angulardiv" ng-app="KitListMyApp" ng-controller="Kitlist" style="overflow: hidden;">
       <div>
           
 <!-- Globalization Tag is added for multilingual  -->
            <div class="">
                <div class="row  ">
                    <div class="col m3 offset-m4 s3"> 
                        <div class="flex">
                            <div>
                                <input type="text" id="txtTenant"  ng-model="search.Tenant" required=""/>
                               <%-- <label>Tenant</label>--%>
                                 <label> <%= GetGlobalResourceObject("Resource", "Tenant")%></label>
                            </div>
                        </div>
                    </div>

                     <div class="col m3 s3">
                        <div class="flex">
                            <div>
                                <input type="text" id="txtPartNo"  ng-model="search.PartNo" required=""/>
                              <%-- <label>Part No.</label>--%>
                                 <label> <%= GetGlobalResourceObject("Resource", "PartNo")%></label>
                            </div>
                        </div>
                    </div>

                   <%-- <div class="col m2 s3">
                        <div class="flex">
                            <div>
                                <input type="text" id="txtKitCode"  ng-model="search.KitCode" required=""/>
                                 <label>Kit Code</label>
                            </div>
                        </div>
                    </div>--%>

                    <div class="col m2 s3">
                        <gap></gap>
                        <button type="button" ng-click="getKitHeaderData()" class="btn btn-primary obd"><%= GetGlobalResourceObject("Resource", "Search")%> <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>&nbsp;&nbsp;
                        <button type="button" type="button" class="btn btn-primary" ng-click="changemenulink()"><%= GetGlobalResourceObject("Resource", "Add")%>  <i class="material-icons">add</i></button>
                    </div>
                </div>
            </div>
        </div>
        
        <div >
             <table class="table-striped">
                    <thead>
                       <%-- <tr class="mytableOutboundHeaderTR">
                            <th>S. No.</th>
                            <th>Tenant</th>
                             <th>Kit Code</th>
                            <th>Kit Part No.</th>
                           <th>UOM</th>
                           <th>Kit Type</th>
                            
                            <th>Created Date</th>
                            <th>Created User</th>
                            <th></th>
                          

                        </tr>--%>
                         <tr class="mytableOutboundHeaderTR">
                            <th><%= GetGlobalResourceObject("Resource", "SNo")%></th>
                            <th><%= GetGlobalResourceObject("Resource", "Tenant")%></th>
                             <th> <%= GetGlobalResourceObject("Resource", "KitCode")%></th>
                            <th><%= GetGlobalResourceObject("Resource", "KitPartNo")%> </th>
                           <th> <%= GetGlobalResourceObject("Resource", "UOM")%></th>
                           <th> <%= GetGlobalResourceObject("Resource", "KitType")%> </th>
                            
                            <th><%= GetGlobalResourceObject("Resource", "CreatedDate")%> </th>
                            <th><%= GetGlobalResourceObject("Resource", "CreatedUser")%> </th>
                            <th></th>
                          

                        </tr>
                    </thead>

                    <tbody>
                        <tr>
                            <td ng-show="KitList.length==0" colspan="9">
                                <div align="center" style="font-size:13px">No data Found. </div>                
                            </td>
                        </tr>

                        <tr class="" dir-paginate="job in KitList  |itemsPerPage:25" pagination-id="main">
                           
                            <td align="right">{{job.SNO}}</td>
                            <td>{{job.tenant}}</td>
                            <td>{{job.KitCode}}</td>
                            <td>{{job.KitPartNo}}</td>
                            
                             <td>{{job.UOM}}</td>
                             <td>{{job.KitType}}</td>
                             
                            <td>{{job.CreatedDate}}</td>
                            <td>{{job.CreatedUser}}</td>
                            <td><a target="_blank" href="../mMaterialManagement/KitPlannerRequest.aspx?kpid={{job.KItId}}"><i class="material-icons">mode_edit</i></a></td>


                        </tr>
                    </tbody>
                </table>
         <div style="float:right !important;font-family:Arial;font-size:small;margin-right:1%;">
          <dir-pagination-controls direction-links="true" pagination-id="main" boundary-links="true"> </dir-pagination-controls>            
        </div> 
        </div>
       
    </div>
       </div>
</asp:Content>
