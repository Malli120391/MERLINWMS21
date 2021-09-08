<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/mInventory/InventoryMaster.master" CodeBehind="StorageLocations.aspx.cs" Inherits="MRLWMSC21.mInventory.StorageLocations"%>

<asp:Content ID="BodyContent" ContentPlaceHolderID="InvContent" runat="server">

    <asp:ScriptManager ID="mySManager"  EnablePageMethods="true" EnablePartialRendering="true" runat="server" SupportsPartialRendering="true">
    </asp:ScriptManager>
        <link href="Scripts/bootstrap.min.css" rel="stylesheet" />
        <script src="Scripts/angular.min.js"></script>
        <script src="Scripts/bootstrap.min.js"></script>
        <script src="Scripts/dirPagination.js"></script>
    <style>
  .pagination ul 
  {
  display: inline-block;
  padding: 0;
  margin: 0;
  }

.pagination li 
{
  display: inline;
}

.pagination li a 
{
  color: black;
  float: left;
  padding: 5px 11px;
  text-decoration: none;
  background-color:#CCCCCC;
  border-radius:5px;
  border:2px solid #FFFFFF;
  box-shadow: 1px 3px 5px #CCCCCC;
  border-spacing:1px;
}

.pagination li.active a
{
  background-color: #6B8C57 !important;
  color: white;
  border:2px solid #FFFFFF !important;
}

.pagination li:hover.active a 
{
  background-color: #f2f2f2;
}

.pagination li:hover:not(.active) a 
{
  background-color: #f2f2f2 !important;
  color: black;
}

        .rnd {
            
        }
    </style>
    <div class="dashed"></div>
    <div class="pagewidth">
    <div ng-app="StorageList" ng-controller="StorageList">

        <div class="dashedborder"></div>
        <div style="padding:10px;">

        <div style="color:red !important; display:none;" id="divErrors" ></div>
        <br />

    <%--<div class="row">
           <div class="col-md-1">
         </div>

        <div class="col-md-2">
            <label><literal style="color:red">*</literal> &nbsp;Location Code:</label> <br />
            <input type="text" ng-model="Code" id="Code" class="form-Control savefield" />
        </div>

         <div class="col-md-2">
            <label><literal style="color:red">*</literal> &nbsp; Description:</label> <br />
            <input type="text" ng-model="Description" id="Description" class="form-Control savefield" />
        </div>

         <div class="col-md-2">
            <label>Remarks:</label> <br />
            <input type="text" ng-model="Remarks" id="Remarks" class="form-Control savefield" />
        </div>
        
        <div class="col-md-2">
        <button type="button" ng-click="SaveLocationData()" id="btnsave" class=" btn btn-md  btn-success form-Control" style="margin-top:15% !important;">Save</button>    
        <input type="hidden" id="StrLocid" class="savefield" value="0" ng-model="StrLocid" />
        </div>

    </div>--%>

        <div>
            <table style="width:42%;float:right;">
                <tr>
                    <td>
                        <div class="flex">
                            <input type="text" ng-model="Code" id="Code" required=""/>
                            <span class="errorMsg" ></span> <label>Location Code:</label> 
                            </div>
                    </td>
                    <td>
                        <div class="flex">
                          
                        <input type="text" ng-model="Description" id="Description"  required=""/>
                             <span class="errorMsg" ></span><label> Description:</label>
                            </div>
                    </td>
                    <td>
                     <div class="flex">
                      <input type="text" ng-model="Remarks" id="Remarks" required="" /> 
                     <span class="errorMsg" ></span>    <label>Remarks:</label> 
                         </div>
                    </td>
                    <td>
                        <p></p>
                   <button type="button" ng-click="SaveLocationData()" id="btnsave" class=" btn btn-primary  btn-success form-Control" style="margin-top:15% !important; ">Save &nbsp; <i class="material-icons vl">save</i></button>    
                    <input type="hidden" id="StrLocid" class="savefield" value="0" ng-model="StrLocid" />
                    </td>
                </tr>
            </table>
        </div>
        <br />

    <table class="table table-striped table-bordered table-hover table-condensed" id="tblstoragelist" style="width:100% !important;">
        <thead class="mytableBlueHeaderTR" style="background-color:#6B8C57 !important; height:35%">
            <tr>
            <th class="text-center">Code</th>
            <th class="text-center">Description</th>
            <th class="text-center">Remarks</th>
            <th class="text-center">Edit</th>
            <th class="text-center">Delete</th>
                </tr>
        </thead>
        <tbody>
            <tr dir-paginate ="objstr in storagelist   | itemsPerPage:10">
                <td class="text-left">{{objstr.Code}}</td>
                <td class="text-left">{{objstr.Description}}</td>
                <td class="text-left">{{objstr.Remarks}}</td>
                <td class="text-center"><a  ng-click="EditData(objstr)" id="btnEdit" class="rnd"> <i class="material-icons ss">mode_edit</i></a></td>
                <td class="text-center"><a  ng-click="DeleteData(objstr)" id="btnDelete" class="rnd"><i class="material-icons ss">delete</i></a></td>
            </tr>
            
        </tbody>
    </table>
    


     <div style="text-align:right;">
            <dir-pagination-controls  boundary-links="true"> </dir-pagination-controls>
        </div>
    </div>
        </div>

    <script>
        var app = angular.module('StorageList', ['angularUtils.directives.dirPagination']);
        app.controller('StorageList', function ($scope, $http) {

            $scope.SaveLocationData = function ()
            {
               
                $scope.SaveData();
            }

            $scope.GetStorageList=function()
            {
            var AjaxCall =
             {
                 method: 'Post',
                 datatype: 'json',
                 url: 'StorageLocations.aspx/GetStorageList',
                 data: {},
                 headers: 'application-json; charset=utf-8'
             }


            $http(AjaxCall).then(function (response) {

                var data = JSON.parse(response.data.d);
                $scope.storagelist = data.Table;

            });

            }


            $scope.EditData = function (obj) {
                $('#StrLocid').val(obj.Id);
                $scope.StrLocid = obj.Id;
                $scope.Code = obj.Code;
                $scope.Description = obj.Description;
                $scope.Remarks = obj.Remarks;
            }

            $scope.DeleteData = function (obj)
            {
                var AjaxCall =
                           {
                               method: 'Post',
                               datatype: 'json',
                               url: 'StorageLocations.aspx/DeleteStorageLocation',
                               data: { 'StrId': obj.Id },
                               headers: 'application-json; charset=utf-8'
                           }
                                $http(AjaxCall).then(function (response)
                                {
                                showStickyToast(true, 'Deleted Successfully');
                                $scope.ClearFields();
                                var data = JSON.parse(response.data.d);
                                })
            }

            $scope.validateFields = function ()
            {
                var NoErrors = true ;
                $("#divErrors").empty();
                $("#divErrors").css('display', 'block');

                if ($scope.Code==undefined || $scope.Code=="" ||  $scope.Code.trim().length == 0)
                {
                    $("#divErrors").append("<li style='margin-top: 0.3em;'>" + 'Please Enter Location Code' + "</li>");
                    NoErrors = false;
                }

                if ($scope.Description == undefined || $scope.Description == "" || $scope.Description.trim().length == 0)
                {
                    $("#divErrors").append("<li style='margin-top: 0.3em;'>" + 'Please Enter Description' + "</li>");
                    NoErrors = false;
                }

                if ($scope.Code != undefined && $scope.Code != "" && $scope.Code.trim().length != 0)
                {
                    
                    var retval = CheckDuplicate($scope.storagelist, "Code", $('#Code').val().trim(), "StrLocid");
                    if (retval == false) {
                        NoErrors = false;
                        $("#divErrors").append("<li style='margin-top: 0.3em;'>" + 'Code  already exists' + "</li>");

                    }
                }
                return NoErrors;
            }

            $scope.SaveData = function ()
            {
                if ($scope.validateFields())
             {
                var xmlString = '';
                xmlString += '<Root><Data>';
                $('.savefield').each(function ()
                {

                    var Id = $(this).attr('id');
                    var value = $(this).val().trim();
                    xmlString += '<' + Id + '>' + value + '</' + Id + '>';
                });

                xmlString += '</Data></Root>';
                
                var AjaxCall =        
                            {
                            method: 'Post',
                            datatype: 'json',
                            url: 'StorageLocations.aspx/SetStorageLocations',
                            data: { 'StrId': $scope.StrLocid, 'Inxml': xmlString },
                            
                            headers: 'application-json; charset=utf-8'
                            }

                  $http(AjaxCall).then(function (response) {
                      
                      showStickyToast(true, 'Saved Successfully');
                      $scope.ClearFields();
                    //var data = JSON.parse(response.data.d);
                    
                })

                }
            }

            $scope.ClearFields = function ()
            {
                $('#StrLocid').val(0);
                $scope.Code = " ";
                $scope.Description = " ";
                $scope.Remarks = " ";
                $scope.StrLocid = 0;
                $scope.GetStorageList();
            }

            $scope.GetStorageList();

        });

        function CheckDuplicate(obj, field, value, PK) {
            
            var status = true;
            var item = obj, Count = 0;

            if ($('#' + PK).val() != 0) {
                item = $.grep(obj, function (data) {
                    return data['Id'] != $('#' + PK).val();
                });
            }
            Count = $.grep(item, function (data) {
                return data[field] == value || data[field].toUpperCase() == value.toUpperCase() || data[field].toLowerCase() == value.toLowerCase();
            });

            if (Count.length != 0) {
                status = false;
            }
            return status;
        }
        
    </script>
    <div class="dashedborder"></div>
        </div>
    </asp:Content>


