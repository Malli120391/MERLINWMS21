<%@ Page Title="Material Type" Language="C#"  MasterPageFile="~/mMaterialManagement/MaterialManagementMaster.master"  MaintainScrollPositionOnPostback="true"  AutoEventWireup="true" CodeBehind="MaterialType.aspx.cs" Inherits="MRLWMSC21.mMaterialManagement.MaterialType" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
        <script src="Scripts/jquery-ui-1.8.24.min.js"></script>
    <script type="text/javascript" src="Scripts/CommonScripts.js"></script>
      <script src="../mInventory/Scripts/angular.min.js"></script>
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>
    <link href="../mInventory/Scripts/bootstrap.min.css" rel="stylesheet" />
<%--<!DOCTYPE html>
    <html>
        <head>--%>
   
          
            <style>

                .getPageId {
                    float: right;
                    margin-bottom:10px;
                }

                .c-container-fluid {
                    margin-bottom:0px !important;
                }
                .pagination ul {
                    display: inline-block;
                    padding: 0;
                    margin: 0;
                }

                .pagination li {
                    display: inline;
                }

                    .pagination li a {
                        position: relative;
                        padding: 6px 12px;
                        margin-left: -1px;
                        line-height: 1.42857143;
                        color: ;
                        text-decoration: none;
                        background-color: #fff;
                        border: 1px solid #ddd;
                        text-align: center;
                        text-decoration: none;
                        vertical-align: middle;
                        box-shadow: var(--z1);
                        padding: 0px;
                        display: inline-block !important;
                        /* border: 2px solid var(--sideNav-bg) !important; */
                        /* background-color: var(--sideNav-bg) !important; */
                        min-width: 23px !important;
                        height: 23px !important;
                        line-height: 20px;
                        /* color: #fff; */
                        border-radius: 3px;
                        margin: 2px;
                        padding: 1px 5px;
                        line-height: 25px;
                        border: 0;
                        font-weight: 500;
                    }

                    .pagination li.active a {
                         border-radius: 3px;
                        display: table-cell;
                        height: 20px;
                        width: 17px;
                        background-color: #cad5e0;
                        border-radius: 14%;
                        color: #fff;
                        font-weight: bold;
                        border: 1px solid #B0C4DE;
                        text-align: center;
                        text-decoration: none;
                        vertical-align: middle;
                        box-shadow: var(--z1);
                        padding: 0px;
                        display: inline-block !important;
                        border: 2px solid var(--sideNav-bg) !important;
                        background-color: var(--sideNav-bg) !important;
                        width: 20px !important;
                        height: 20px !important;
                        line-height: 20px;
                    }

               

                .flex input[type="text"], input[type="number"], textarea {
                    width:100% !important;
                }

            </style>
         
           <%--<script src="../mInbound/Scripts/angular.min.js"></script>--%>
        <script src="../Scripts/angular.min.js"></script>
           
            <script src="../mInbound/Scripts/dirPagination.js"></script>

            <script type="text/javascript">
                var app = angular.module('myApp', ['angularUtils.directives.dirPagination']);
                app.controller('formCtrl', function ($scope, $http) {

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

                    $scope.showList = true;
                    $scope.showForm = false;
                    $scope.GetList = function () {
                        var tenantid = "0";
                        var roleid = {
                            method: 'POST',
                            url: 'MaterialType.aspx/Getlistdata',
                            headers: {
                                'Content-Type': 'application/json; charset=utf-8',
                                'dataType': 'json'
                            },
                            data: {}
                        }
                        $http(roleid).success(function (response) {
                            $scope.listdata = response.d;
                        });
                    }
                    $scope.GetList();
                    $scope.tenantdata = new parameters(0, '', 0, '', '');
                    $scope.gettenant = function () {
                        if ($scope.tenantdata.tntname == undefined) {
                            $scope.tenantdata.tntname = '';
                        }
                        //alert(tntname);
                        var roleid = {

                            method: 'POST',
                            url: 'MaterialType.aspx/Getdata',
                            headers: {
                                'Content-Type': 'application/json; charset=utf-8',
                                'dataType': 'json'
                            },
                            data: { 'value': $scope.tenantdata.tntname }
                        }
                        $http(roleid).success(function (response) {
                            $scope.statedata = response.d;
                        });
                    }

                    $scope.gettenant();
                    $scope.Delete = function (obj) {
                        if (confirm("Are you sure do you want to delete?")) {
                            //  $scope.tenantdata = new parameters(obj.mtypeID,obj.tntname, obj.tenentId, obj.Mtype, obj.Desc);
                            var roleid = {
                                method: 'POST',
                                url: 'MaterialType.aspx/delete',
                                headers: {
                                    'Content-Type': 'application/json; charset=utf-8',
                                    'dataType': 'json'
                                },
                                data: { 'prsobj': obj }
                            }
                            $http(roleid).success(function (response) {

                                debugger;
                                if (response.d == "Exist") {
                                    showStickyToast(false, "Cannot delete Material Type as item is mapped to this.");
                                }
                                else {
                                    showStickyToast(true, 'Deleted Successfully', false);
                                    $scope.GetList();
                                    setTimeout(function () {
                                        location.reload();
                                    }, 1000);
                                }
                                
                            });
                        }
                        else
                            return false;
                    }
                    $scope.event = function () {
                        debugger;
                        $scope.Clear();
                        $("#btnCreate").html('Create');
                         $("#MTModal").modal({
                         show: 'true'
                        });
                      //$scope.showList = false;
                      //$scope.showForm = true;
                    };

                    $scope.Submit = function (objs) {
                        debugger;
                        if (objs != null) {
                            objs.tenentId = tenantid;
                        }
                       
                        if ($("#txtTenant").val() == "")
                        {
                            showStickyToast(false, 'Please select Tenant', false);
                            return false;
                        }

                        if ($("#txtMType").val() == "")
                        {
                            showStickyToast(false, 'Please enter Material Type', false);
                            return false;
                        }
                        if ($("#txtMType").val().trim() != "") {
                            var item = $scope.listdata, Count = 0;
                            debugger;
                            if ($('#MTypeID').val() != 0) {

                                item = $.grep($scope.listdata, function (data) {
                                    return data['mtypeID'] != $('#MTypeID').val();
                                });
                                Count = $.grep(item, function (data) {

                                    return data['Mtype'].toLowerCase() == $('#txtMType').val().toLowerCase().trim() && data['tenentId'] == objs.tenentId;
                                });

                                if (Count.length != 0) {
                                    IsValid = false;
                                    showStickyToast(false, 'Material Type already exists.', false);
                                    return false;
                                }
                            }
                            else {
                                 Count = $.grep($scope.listdata, function (data) {

                                    return data['Mtype'].toLowerCase() == $('#txtMType').val().toLowerCase().trim() && data['tenentId'] == objs.tenentId;
                                });

                                if (Count.length != 0) {
                                    IsValid = false;
                                    showStickyToast(false, 'Material Type already exists.', false);
                                    return false;
                                }
                            }

                           
                        }
                        //if (objs.mtypeID != "" && objs.mtypeID != null && objs.mtypeID != undefined) {
                        var roleid = {
                            method: 'POST',
                            url: 'MaterialType.aspx/insert',
                            headers: {
                                'Content-Type': 'application/json; charset=utf-8',
                                'dataType': 'json'
                            },
                            data: { 'prmtrs': $scope.tenantdata }
                        }
                        $http(roleid).success(function (response) {
                            //alert(response);
                            if (response.d == "1") {
                                showStickyToast(true, 'Material Type Saved Successfully. ', false);
                                $scope.GetList();
                                 setTimeout(function () {
                                    location.reload();
                                }, 2000);
                                return;
                            }
                             if (response.d == "0") {
                                 showStickyToast(false, 'Error While Updating ', false);
                                  setTimeout(function () {
                                    location.reload();
                                }, 2000);
                                return;
                            }
                            debugger;
                            if (response.d == "exist") {
                                showStickyToast(false, 'Could not update Material Type as item is mapped to this. ', false);
                                return;

                            }
                            if ($('#btnCreate').text() == "Update") {
                               
                                $scope.showForm = false;
                                $scope.showList = true;
                                showStickyToast(true, 'Material Type Saved Successfully', false);
                                $scope.GetList();
                                setTimeout(function () {
                                    location.reload();
                                }, 2000);
                              
                            }
                            else {
                                showStickyToast(true, 'Material Type Saved Successfully', false);
                                $scope.GetList();
                                setTimeout(function () {
                                    location.reload();
                                }, 2000);
                               
                            }
                        });
                    }

                    $scope.Clear = function () {
                        $scope.tenantdata = null;
                        $scope.showList = true;
                        $scope.showForm = false;
                        $("#txtTenant").val("");
                        
                    }

                    $scope.Edit = function (obj) {
                        // $scope.showList = false;
                        //$scope.showForm = true;
                        $("#MTModal").modal({
                        show: 'true'
                         });
                        debugger;
                        $("#btnCreate").html('Update');
                       
                        var value = obj.tntname;
                        $scope.tenantdata = new parameters(obj.mtypeID, obj.tntname, obj.tenentId, obj.Mtype, obj.Desc);
                        tenantid = obj.tenentId;
                        $("#MTypeID").val(obj.mtypeID);                        
                    }

                    $scope.locationsList = function () {
                        debugger;
                        var Prefix = $scope.tenantdata.tntname;
                        if (Prefix == undefined) {
                            Prefix = "";
                        }                       

                        var textfieldname = $("#txtTenant");
                        DropdownFunction(textfieldname);
                        $("#txtTenant").autocomplete({
                            source: function (request, response) {
                                $.ajax({
                                   // url: '../mWebServices/FalconWebService.asmx/LoadTenantDataFor3PL',
                                    url: '../mWebServices/FalconWebService.asmx/LoadTenantDataByUserWH', // Added By Ganesh @Sep 28-2020 --Tenant Drop down data should be displyed by UserWh
                                    data: "{'prefix': '" + request.term + "' }",
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
                                tenantid = i.item.val;

                                debugger;
                            },
                            minLength: 0
                        });
                    }

                    $scope.locationsList();
                });
                function parameters(mid, tname, TID, Mtype, Desc) {
                    this.mtypeID = mid;
                    this.tntname = tname;
                    this.tenentId = TID;
                    this.mtype = Mtype;
                    this.desc = Desc;
                }
            </script>
   <%--     </head>--%>
    <div class="module_yellow">
            <div class="ModuleHeader">
               <div> <a href="../Default.aspx">Home</a> <i class="material-icons">arrow_right</i> <span>Administration</span> <i class="material-icons">arrow_right</i> <span class="breadcrumbd" contenteditable="false">Material Type </span></div>
                
           </div>
            
        </div>
    <div ng-app="myApp" ng-controller="formCtrl" class="container">

<%--        <div ng-show="showForm" ng-cloak>--%>
          <div id="MTModal" class="modal">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header" style="background-color: var(--sideNav-bg) !important; color: #fff !important;">
                               <%-- <h4 class="modal-title" style="display: inline !important;">Add Material Type</h4>--%>
                                 <h4 class="modal-title" style="display: inline !important;"> <%= GetGlobalResourceObject("Resource", "AddMaterialType")%></h4>
                                <button type="button" data-dismiss="modal" class="pull-right modalclose" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <div class="modal-body" id="mySupForm">
            <div class="row">
                <div class="col m4">
                    <div class="flex">
                        <input type="text" id="txtTenant" required="" ng-model="tenantdata.tntname">
                        <span class="errorMsg"> </span>
                     <%--   <label>Tenant</label>--%>
                           <label> <%= GetGlobalResourceObject("Resource", "Tenant")%></label>
                    </div>

                </div>
                <div class="col m4">
                    <div class="flex">
                        <input type="text" required="" ng-model="tenantdata.mtype" maxlength="10" id="txtMType" />
                        <span class="errorMsg"></span>
                      <%--  <label>Material type </label>--%>
                          <label><%= GetGlobalResourceObject("Resource", "Materialtype")%>  </label>
                        <input type="hidden" id="MTypeID" value="0" />
                    </div>
                </div>
                <div class="col m4">
                    <div class="flex">
                        <input type="text" required="" maxlength="50" ng-model="tenantdata.desc" />
                    <%--    <label>Description </label>--%>
                            <label> <%= GetGlobalResourceObject("Resource", "Description")%> </label>
                    </div>

                </div>
       <%--         <div class="col m3">
                    <button type="button" id="btnCreate" class="btn btn-primary" ng-click="Submit(tenantdata)">Create</button>
                    &nbsp;&nbsp;
                         <button type="button" class="btn btn-primary" ng-click="Clear()">Cancel</button>
                </div>--%>
            </div>
                                </div>
                                                            <div class="modal-footer">
                                <input type="hidden" id="DockID" ng-model="materialids" />
                                <%--<input type="reset" value="reset" />--%>
                               <%-- <button type="button" class="btn btn-primary" style="color: #fff !important;" ng-click="Clear()">Clear</button>--%>
                                                                <button type="button" class="btn btn-primary" style="color: #fff !important;" ng-click="Clear()"> <%= GetGlobalResourceObject("Resource", "Clear")%></button>
                               <%-- <button type="button" class="btn btn-primary" style="color: #fff !important;" data-dismiss="modal">Close</button>--%>
                                                                <button type="button" class="btn btn-primary" style="color: #fff !important;" data-dismiss="modal"><%= GetGlobalResourceObject("Resource", "Close")%></button>
                                <%--<button type="button" class="btn btn-primary"  ng-click="Submit(tenantdata)">Save</button>--%>
                                                                <button type="button" class="btn btn-primary"  ng-click="Submit(tenantdata)"> <%= GetGlobalResourceObject("Resource", "Save")%></button>
                            </div>
                                </div>
           
              </div>
        </div>
     <%--   <div ng-show="showList" ng-cloak>--%>
            <div>
                <div class="row">
                    <div>
                        <div>
                            <div class="flex__ right">
                                <div class="flex">
                                    <input type="text" ng-model="search" required="">
                                  <%--  <label class="lblFormItem">Search</label>--%>
                                      <label class="lblFormItem"> <%= GetGlobalResourceObject("Resource", "Search")%></label>
                                </div>
                                <div>
                                   <%-- <button ng-if="UserRoleDataID != '4'" type="button" id="btnevent" class="btn btn-primary" ng-click="event()">Add <i class="material-icons">add</i></button>--%>
                                     <button ng-if="UserRoleDataID != '4'" type="button" id="btnevent" class="btn btn-primary" ng-click="event()"><%= GetGlobalResourceObject("Resource", "Add")%> <i class="material-icons">add</i></button>
                                </div>
                            </div>
                        </div>
                        <div style="text-align: right">
                        </div>
                    </div>
                </div>
            </div>
            <table class="table-striped">
                  <thead>  <tr>
                        <%--<th>Tenant 
                        </th>
                        <th>Material Type
                        </th>
                        <th>Description
                        </th>
                        <th><span ng-if="UserRoleDataID != '4'">
                            Action
                            </span>
                        </th>--%>
                      <th>S. NO</th>
                      <th> <%= GetGlobalResourceObject("Resource", "Tenant")%>
                        </th>
                        <th> <%= GetGlobalResourceObject("Resource", "MaterialType")%>
                        </th>
                        <th><%= GetGlobalResourceObject("Resource", "Description")%>
                        </th>
                        <th><span ng-if="UserRoleDataID != '4'">
                            <%= GetGlobalResourceObject("Resource", "Action")%>
                            </span>
                        </th>

                    </tr></thead>
                <tbody>
                   

                <tr dir-paginate="t in listdata |filter:search|itemsPerPage:25" pagination-id="nonAvaible">
                    <td>{{$index+1}}</td>
                    <td>{{t.tntname}}
                    </td>
                    <td>{{t.Mtype}}
                    </td>
                    <td>{{t.Desc}}
                    </td>
                    <td>

                        <a ng-if="UserRoleDataID != '4'" ng-click="Edit(t)"><i class='material-icons ss'>mode_edit</i></a>
                        <a ng-if="UserRoleDataID != '4'" ng-click="Delete(t)" onclick="ConfirmDelete()"><i class='material-icons ss'>delete</i></a>

                    </td>
                </tr>
                     <tr>
                    <td ng-show="listdata.length==0" colspan="9">
                                <div align="center" style="font-size:13px">No data Found. </div>                
                            </td>
                        </tr>
                </tbody>

            </table>
            <dir-pagination-controls class="getPageId" direction-links="true" pagination-id="nonAvaible" boundary-links="true"> </dir-pagination-controls>
      <%--  </div>--%>
    </div>
<%--    </html> --%> 

    <br />
    <br />
    <br />

   
     
</asp:Content>