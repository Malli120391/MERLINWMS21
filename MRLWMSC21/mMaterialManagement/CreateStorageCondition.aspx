<%@ Page Title="Storage Condition" Language="C#" AutoEventWireup="true" CodeBehind="CreateStorageCondition.aspx.cs"  MasterPageFile="~/mMaterialManagement/MaterialManagementMaster.master" MaintainScrollPositionOnPostback="true"  Inherits="MRLWMSC21.mMaterialManagement.CreateStorageCondition" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server"  >
    <%--<script src="../mInbound/Scripts/angular.min.js"></script>--%>
    <script src="../Scripts/angular.min.js"></script>
          <script src="Scripts/jquery-ui-1.8.24.min.js"></script>
    <script type="text/javascript" src="Scripts/CommonScripts.js"></script>
 <%--     <script src="../mInventory/Scripts/angular.min.js"></script>--%>
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>
    <link href="../mInventory/Scripts/bootstrap.min.css" rel="stylesheet" />
     <script src="../mInbound/Scripts/dirPagination.js"></script>
    <script src="CreateStorageCondition.js"></script>
     <style>
                         .getPageId {
                    float: right;
                    margin-bottom:10px;
                }


                .pagination ul {
                    display: inline-block;
                    padding: 0;
                    margin: 0;
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



            </style>
    <div class="module_yellow">
            <div class="ModuleHeader">
               <div> <a href="../Default.aspx">Home</a> <i class="material-icons">arrow_right</i> <span>Administration</span> <i class="material-icons">arrow_right</i> <span class="breadcrumbd" contenteditable="false">Storage Condition </span></div>
                
           </div>
            
        </div>
    <div  ng-app="MyApp" ng-controller="FrmCtrl" class="container" >       
      
     <%--   <div ng-show="showForm" ng-cloak>--%>
                   <div id="STCModal" class="modal">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header" style="background-color: var(--sideNav-bg) !important; color: #fff !important;">
                              <%--  <h4 class="modal-title" style="display: inline !important;">Add Storage Condition</h4>--%>
                                  <h4 class="modal-title" style="display: inline !important;"> <%= GetGlobalResourceObject("Resource", "AddStorageCondition")%></h4>
                                <button type="button" data-dismiss="modal" class="pull-right modalclose" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <div class="modal-body" id="myStCondForm">
            <div class="row">
                <div class="col m4 s4">
                    <div class="flex">
                        <input type="text" id="txtTenant" required="" ng-model="tenantdata.TenantName">
                        <span class="errorMsg"></span>
                   <%--     <label>Tenant</label>--%>
                             <label> <%= GetGlobalResourceObject("Resource", "Tenant")%></label>
                    </div>
                </div>            
                <div class="col m4 s4">
                    <div class="flex">
                        <input type="text" required="" ng-model="tenantdata.StorageCondition" id="txtcon" />
                       <%-- <label>Storage Condition</label>--%>
                         <label><%= GetGlobalResourceObject("Resource", "StorageCondition")%> </label>
                    </div>
                </div>
                    <div class="col m4 s4">
                    <div class="flex">
                        <input type="text" required="" ng-model="tenantdata.StorageConditionCode" id="txtcode" maxlength="10" />
                        <span class="errorMsg"> </span>
                       <%-- <label>Storage Condition Code </label>--%>
                         <label> <%= GetGlobalResourceObject("Resource", "StorageConditionCode")%></label>
                        <input type="hidden" id="stID" value="0" />
                    </div>
                </div>
             <%--   <div class="col m3">
                    <button type="button" id="btnCreate" class="btn btn-primary" ng-click="Submit(tenantdata)">Create</button>
                    &nbsp;&nbsp;
                         <button type="button" class="btn btn-primary" ng-click="Clear()">Cancel</button>
                </div>--%>
            </div>
</div>
                       
                          <div class="modal-footer">
                                <input type="hidden" id="DockID" ng-model="storagecondids" />
                                <%--<input type="reset" value="reset" />--%>
                              <%--  <button type="button" class="btn btn-primary" style="color: #fff !important;" ng-click="Clear()">Clear</button>--%>
                                <button type="button" class="btn btn-primary" style="color: #fff !important;" ng-click="Clear()"><%= GetGlobalResourceObject("Resource", "Clear")%></button>
                               <%-- <button type="button" class="btn btn-primary" style="color: #fff !important;" data-dismiss="modal">Close</button>--%>
                               <button type="button" class="btn btn-primary" style="color: #fff !important;" data-dismiss="modal"><%= GetGlobalResourceObject("Resource", "Close")%></button>
                           <%--     <button type="button" class="btn btn-primary" ng-click="Submit(tenantdata)" >Save</button>--%>
                                   <button type="button" class="btn btn-primary" ng-click="Submit(tenantdata)" > <%= GetGlobalResourceObject("Resource", "Save")%></button>
                            </div>
                                </div>
           
              </div>

         </div>
   <%--     <div ng-show="showList" ng-cloak>--%>

            <div>
                <div class="row">
                    <div>
                        <div>
                            <div class="flex__ right">
                                <div class="flex">
                                    <input type="text" ng-model="search" required="">
                                <%--    <label class="lblFormItem">Search</label>--%>
                                        <label class="lblFormItem"> <%= GetGlobalResourceObject("Resource", "Search")%> </label>
                                    
                                </div>
                                &nbsp;&nbsp &nbsp;&nbsp; 
                                <div>
                                    <%--<button type="button" id="btnevent" class="btn btn-primary" ng-click="event()">Add <i class="material-icons">add</i></button>--%>
                                    <button type="button" id="btnevent" class="btn btn-primary" ng-click="event()"><%= GetGlobalResourceObject("Resource", "Add")%> <i class="material-icons">add</i></button>
                                </div>
                            </div>
                        </div>
                        <div style="text-align: right">
                        </div>
                    </div>
                </div>
            </div>
            <table class="table-striped">
                <thead>
                    <tr>
                       <%-- <th>Tenant</th>
                        <th>Storage Condition</th>
                        <th>Code</th>                  
                        <th>Action</th>--%>
                        <th>S. NO</th>
                         <th> <%= GetGlobalResourceObject("Resource", "Tenant")%></th>
                        <th><%= GetGlobalResourceObject("Resource", "StorageCondition")%> </th>
                        <th><%= GetGlobalResourceObject("Resource", "Code")%> </th>                  
                        <th><%= GetGlobalResourceObject("Resource", "Action")%></th>
                    </tr>
                </thead>
                <tbody>
                     
                    <tr dir-paginate="t in listdata|filter:search|itemsPerPage:25" pagination-id="nonAvaible">
                        <td>{{$index+1}}</td>
                        <td>{{t.TenantName}}  </td>                   
                        <td>{{t.StorageCondition}} </td>
                        <td>{{t.StorageConditionCode}} </td>
                        <td>
                            <a ng-click="Edit(t)"><i class='material-icons ss'>mode_edit</i></a>
                            <a ng-click="Delete(t)"><i class='material-icons ss'>delete</i></a>
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
</asp:Content>
