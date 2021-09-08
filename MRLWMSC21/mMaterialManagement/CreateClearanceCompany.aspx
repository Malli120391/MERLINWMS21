<%@ Page Title="Create Clearance Company" Language="C#" MasterPageFile="~/mMaterialManagement/MaterialManagementMaster.master" AutoEventWireup="true" CodeBehind="CreateClearanceCompany.aspx.cs" Inherits="MRLWMSC21.mMaterialManagement.CreateClearanceCompany" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server" class="">

    <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
    <script type="text/javascript" src="Scripts/CommonScripts.js"></script>
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>
    <link href="../mInventory/Scripts/bootstrap.min.css" rel="stylesheet" />

    <script>
        var app = angular.module('clearanceCompany', ['angularUtils.directives.dirPagination']);
        app.controller('myCtrl', function ($scope, $http) {
            // debugger;
            $scope.button = "Save";
            var accountid = 0;

            //Account filters
            var textfieldname = $("#txtAccount");
            DropdownFunction(textfieldname);
            $("#txtAccount").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '../mWebServices/FalconWebService.asmx/LoadAccountDataFor3PL',
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
                    accountid = i.item.val;
                },
                minLength: 0
            });
            // alert($scope.id);
            $scope.insertClearance = function () {
                // alert($scope.Id);
                debugger;
                if ($("#txtAccount").val() == "") {

                    showStickyToast(false, 'Please Select Account', false);
                    return false;
                }
                if ($scope.txtClearanceName == "" || $scope.txtClearanceName == undefined) {
                    showStickyToast(false, "Clearance Company Name is required");
                    return;
                }
                if ($scope.txtClearanceCode == "" || $scope.txtClearanceCode == undefined) {
                    showStickyToast(false, "Clearance Company Code is required");
                    return;
                }
                else {
                    var clearance = { 'ClearanceId': $scope.Id, 'ClearanceName': $scope.txtClearanceName, 'ClearanceCode': $scope.txtClearanceCode, 'AccountID': accountid };
                    var insert = {
                        method: 'POST',
                        url: 'CreateClearanceCompany.aspx/SaveClearance',
                        header: {
                            'Content-Type': 'application/json; charset=utf-8',
                            'dataType': 'json'
                        },
                        data: clearance
                    }
                    $http(insert).success(function (response) {

                        if (response.d == "1") { showStickyToast(true, "Data Entered Successfully"); }
                        if (response.d == "0") { showStickyToast(false, "Clearance Code for this Company already exists"); }
                        $('#Container').modal('hide');

                    });
                    $scope.clearancelist();
                    $scope.cancelClearance();
                }

            }
            $scope.cancelClearance = function () {
                // $scope.InsertPanel = false;
                $scope.button = "Save";
                $scope.bindClearanceCode = "";
                $scope.bindClearanceName = "";
                $("#txtAccount").val("");
                accountid = 0;
            }
            $scope.editClearance = function (id, cname, code, AccCode, Accid) {
                debugger;
                $scope.button = "Update";
                //  alert(id + "" + cname);
                // $scope.InsertPanel = true;
                $("#Container").modal({
                    show: 'true'
                });
                $scope.Id = id;
                $scope.bindClearanceName = cname;
                $scope.txtClearanceName = $scope.bindClearanceName;
                $scope.bindClearanceCode = code;
                $scope.txtClearanceCode = $scope.bindClearanceCode;
                $("#txtAccount").val(AccCode);
                accountid = Accid;


                //var edit = {
                //    method: 'POST',
                //    url: 'CreateClearanceCompany.aspx/EditClearance',
                //    header: { 'Content-Type': 'application/json; charset=utf-8',
                //        'dataType': 'json'
                //    },
                //    data: {'Id':id}
                //}
                //$http(edit).success(function (response) {
                //    $scope.txtClearanceName = response.d[0].CompanyName;
                //    $scope.txtClearanceCode = response.d[0].ClearanceCode;
                //});

            }
            $scope.deleteClearance = function (id) {
                if (confirm("Are you sure do you want to delete ?")) {
                    var del = {
                        method: 'POST',
                        url: 'CreateClearanceCompany.aspx/DeleteClearance',
                        header: {
                            'Content-Type': 'application/json; charset=utf-8',
                            'dataType': 'json'
                        },
                        data: { 'Id': id }
                    }
                    $http(del).success(function (response) {
                        showStickyToast(true, response.d);
                    });
                }
                $scope.clearancelist();
            }
            $scope.clearancelist = function () {
                //  alert("table");
                var list = {
                    method: 'POST',
                    url: 'CreateClearanceCompany.aspx/ClearanceCompany_Data',
                    header: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: {}
                }
                $http(list).success(function (response) {
                    $scope.Clearancelist = response.d;
                });
            }
            $scope.addClearance = function () {
                // $scope.InsertPanel = true;
                $("#Container").modal({
                    show: 'true'
                });
                $scope.txtClearanceCode = "";
                $scope.txtClearanceName = "";
                $scope.Id = 0;
                $("#txtAccount").val('');
            }
        });
    </script>
    <style>
        /*#Container{
            position:fixed;
            background-color:rgba(0,0,0,0.5);
            top:-20px;bottom:-20px;left:0;right:0;
            z-index:999; text-align:center;
        }*/
        /*#divContent{
                  background-color: white;
    width: 50%;
    padding-bottom: 25px;
    padding: 20px;
    border-radius: 5px;
    position: absolute;
    left: calc(50% - 25%);
    top: calc(50% - 82px );
        }*/

        .alignright {
            float: right;
            margin-bottom: 45px;
        }

        .txt_slim {
            width: 100% !important;
        }

        .pagination {
            margin: 0px !important;
        }
    </style>
   <div class="module_yellow">
            <div class="ModuleHeader">
               <div> <a href="../Default.aspx">Home</a> <i class="material-icons">arrow_right</i> <span>Administration</span> <i class="material-icons">arrow_right</i> <span class="breadcrumbd" contenteditable="false">Create Clearance Company  </span></div>
                
           </div>
            
        </div>
    <div data-ng-app="clearanceCompany" data-ng-controller="myCtrl" data-ng-init="clearancelist();" class="pagewidth">



        <div class="modal inmodal" id="Container" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog" style="width: 50% !important;">
                <div class="modal-content animated fadeIn">

                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                        <h3 class="modal-title" style="display: inline !important; text-align: left"><%= GetGlobalResourceObject("Resource", "ClearanceCompanyDetails")%></h3>
                    </div>

                    <div class="modal-body">
                        <div class="row">
                            <div class="col m4">
                                <div class="flex">
                                    <input type="text" class="txt_slim" value="{{AccountName}}" id="txtAccount" data-ng-model="txtCAccount" required />
                                    <%-- <span class="errorMsg"> * </span><label>  Account </label>--%>
                                    <span class="errorMsg">* </span>
                                    <label><%= GetGlobalResourceObject("Resource", "Account")%> </label>
                                </div>
                            </div>
                            <div class="col m4">
                                <div class="flex">
                                    <input type="text" class="txt_slim" value="{{bindClearanceName}}" data-ng-model="txtClearanceName" required />
                                    <%-- <span class="errorMsg"> * </span><label>  Clearance Company Name </label>--%>
                                    <span class="errorMsg">* </span>
                                    <label><%= GetGlobalResourceObject("Resource", "ClearanceCompanyName")%> </label>
                                </div>
                            </div>
                            <div class="col m4">
                                <div class="flex">
                                    <input type="text" class="txt_slim" value="{{bindClearanceCode}}" data-ng-model="txtClearanceCode" required />
                                    <%--  <span class="errorMsg"> * </span><label>Clearance Company Code </label>--%>
                                    <span class="errorMsg">* </span>
                                    <label><%= GetGlobalResourceObject("Resource", "ClearanceCompanyCode")%> </label>
                                </div>
                            </div>

                        </div>
                    </div>
                    <div class="modal-footer">
                        <button id="btnCancel" class="btn btn-sm btn-primary" type="button" data-dismiss="modal" data-ng-click="cancelClearance()"><%= GetGlobalResourceObject("Resource", "Cancel")%></button>
                        &nbsp;&nbsp;
                      <button id="btnSave" class="btn btn-sm btn-primary" type="button" data-ng-click="insertClearance()">{{button}}</button>
                    </div>
                </div>
            </div>
        </div>

        <div>
            <div class="flex__ right">
                <%--  <div class="flex"><input type="text" data-ng-model="search" required="" /><label>Search</label></div>--%>
                <div class="flex">
                    <input type="text" data-ng-model="search" required="" /><label><%= GetGlobalResourceObject("Resource", "Search")%> </label>
                </div>
                <button type="button" data-ng-click="addClearance()" class="btn btn-primary btn-sm" style="float: right"><%= GetGlobalResourceObject("Resource", "Add")%> <i class="material-icons vl">add</i></button>
            </div>
            <div>
                <table class="table-striped">
                    <thead>
                        <tr>
                            <th>S. NO</th>
                            <%--<th>Serial No.</th>--%>
                            <%-- <th>Account</th><th>Company Name</th><th>Company Code</th><th>Action</th></tr>--%>
                            <th><%= GetGlobalResourceObject("Resource", "Account")%></th>
                            <th><%= GetGlobalResourceObject("Resource", "CompanyName")%></th>
                            <th><%= GetGlobalResourceObject("Resource", "CompanyCode")%></th>
                            <th><%= GetGlobalResourceObject("Resource", "Action")%></th>
                        </tr>
                    </thead>
                    <tbody>
                       

                        <tr dir-paginate="x in Clearancelist|filter:search|itemsPerPage:10">
                            <td>{{$index+1}}</td>
                            <td><span Title="{{x.AccountName}}">{{x.AccountCode}}</span></td>
                            <td>{{x.CompanyName}}</td>
                            <td>{{x.CompanyCode}}</td>
                            <td><a data-ng-click='editClearance(x.CompanyId,x.CompanyName,x.CompanyCode,x.AccountCode,x.AccountID);'><i class='material-icons ss'>mode_edit</i></a>
                                &nbsp;&nbsp;<a data-ng-click='deleteClearance(x.CompanyId);'><i class='material-icons ss'>delete</i></a>
                            </td>
                        </tr>

                         <tr>
                            <td ng-show="Clearancelist.length==0" colspan="9">
                                <div align="center" style="font-size:13px">No data Found. </div>                
                            </td>
                        </tr>
                    </tbody>
                </table>
                <dir-pagination-controls max-size="3" direction-links="true" boundary-links="true" class="alignright"> </dir-pagination-controls>
            </div>
        </div>
    </div>

</asp:Content>
