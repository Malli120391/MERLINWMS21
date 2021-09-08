<%@ Page Title="Create Freight Company" Language="C#" MasterPageFile="~/mMaterialManagement/MaterialManagementMaster.master" AutoEventWireup="true" CodeBehind="CreateFreightCompany.aspx.cs" MaintainScrollPositionOnPostback="true"  Inherits="MRLWMSC21.mMaterialManagement.CreateFreightCompany" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
<script src="../Scripts/angular.min.js"></script>
<script src="CreateFreightCompany.js"></script>
      <script src="../mInbound/Scripts/dirPagination.js"></script>
            <script src="../mInventory/Scripts/bootstrap.min.js"></script>
    <link href="../mInventory/Scripts/bootstrap.min.css" rel="stylesheet" />

        <style>
                         .getPageId {
                    float: right;
                    margin-bottom:10px;
                    margin-top:10px;
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



     

        .alignright {
            float: right;
            margin-bottom: 45px;
        }

        .txt_slim {
            width:100% !important;
        }

            </style>
   
    <div class="module_yellow">
            <div class="ModuleHeader">
               <div> <a href="../Default.aspx">Home</a> <i class="material-icons">arrow_right</i> <span>Administration</span> <i class="material-icons">arrow_right</i> <span class="breadcrumbd" contenteditable="false">Create Freight Company </span></div>
                
           </div>
            
        </div>
    <div ng-app="MyApp" ng-controller="FrmCtrl" class="container">
                
            <div class="modal inmodal" id="Container" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static" data-keyboard="false" >
        <div class="modal-dialog" style="width: 50% !important;">
            <div class="modal-content animated fadeIn">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                     <h2 class="modal-title" style="display: inline !important; text-align: left"><%= GetGlobalResourceObject("Resource", "FreightCompanyDetails")%></h2>
                </div>

                <div class="modal-body">
                 <div class="row">
                    <div class="col m4">
                        <div class="flex">
                            <input type="text" class="txt_slim" id="txtAccount" data-ng-model="txtCAccount" required />
                            <%--   <span class="errorMsg"> * </span><label>  Account </label>--%>
                            <span class="errorMsg">* </span>
                            <label><%= GetGlobalResourceObject("Resource", "Account")%> </label>
                        </div>
                    </div>
                    <div class="col m4">
                        <div class="flex">
                            <input type="text" required="" ng-model="data.FreightCompanyCode" id="txtcode" style="width: 100%" />
                            <span class="errorMsg">* </span>
                            <%--   <label>Freight Company Code </label>--%>
                            <label><%= GetGlobalResourceObject("Resource", "FreightCompanyCode")%> </label>
                            <input type="hidden" id="stID" value="0" />
                        </div>
                    </div>
                    <div class="col m4">
                        <div class="flex">
                            <input type="text" required="" ng-model="data.FreightCompany" id="txtcompany" />
                            <span class="errorMsg"></span>
                            <%-- <label>Freight Company</label>--%>
                            <label><%= GetGlobalResourceObject("Resource", "FreightCompany")%> </label>
                        </div>
                    </div>
                    <div class="col m3 flex__">
                       


                        <%--  <button type="button" class="btn btn-primary" ng-click="Clear()">Cancel</button>--%>
                    </div>

                </div>
                </div>
                <div class="modal-footer">
                  <button type="button" class="btn btn-primary" data-dismiss="modal" ng-click="Clear()"><%= GetGlobalResourceObject("Resource", "Cancel")%></button>&nbsp;
                  <%--  <button type="button" id="btnCreate" class="btn btn-primary" ng-click="Submit(data)">Create</button>--%>
                        <button type="button" id="btnCreate" class="btn btn-primary"  ng-click="Submit(data)"><%= GetGlobalResourceObject("Resource", "Create")%> </button>
                </div>
            </div>
        </div>
    </div>

         <div ng-cloak >

             <div >
              <div >
               <div class="row">
                   <div>
                       <div>
                          <div class="flex__ right">
                           <div class="flex">
                               <input type="text" ng-model="search" required="">
                              <%-- <label class="lblFormItem">Search</label>--%>
                                <label class="lblFormItem"><%= GetGlobalResourceObject("Resource", "Search")%> </label>
                           </div>&nbsp;&nbsp;&nbsp;&nbsp;
                              <div>
                            <%-- <button type="button" id="btnevent" class="btn btn-primary"  ng-click="event()">Add <i class="material-icons">add</i></button></div>--%>
                                   <button type="button" id="btnevent" class="btn btn-primary"  ng-click="event()"> <%= GetGlobalResourceObject("Resource", "Add")%> <i class="material-icons">add</i></button></div>
                           </div> 
                       </div>
                       <div style="text-align:right">
                         
                       </div>
                   </div>
               </div>
           </div>
           </div>
        <table class="table-striped">
            <thead>
                <tr>
                  <%-- <th>Account</th>
                    <th>Freight Company Code</th>
                     <th>Freight Company</th>
                    <th>Action</th>--%>
                    <th>S. NO</th>
                     <th><%= GetGlobalResourceObject("Resource", "Account")%></th>
                    <th> <%= GetGlobalResourceObject("Resource", "FreightCompanyCode")%></th>
                     <th><%= GetGlobalResourceObject("Resource", "FreightCompany")%></th>
                    <th><%= GetGlobalResourceObject("Resource", "Action")%></th>
                </tr>
            </thead>
            <tbody>
              

                <tr dir-paginate="t in Listdata|filter:search|itemsPerPage:25" pagination-id="nonAvaible"">
                    <td>{{$index+1}}</td>
                   <td><span title={{t.AccountName}}>{{t.AccountName}}</span></td>
                    <td>{{t.FreightCompanyCode}}</td>
                     <td>{{t.FreightCompany}}</td>
                    <td>
                        <a ng-click="Edit(t)"><i class='material-icons ss'>mode_edit</i></a>
                        <a ng-click="Delete(t)"><i class='material-icons ss'>delete</i></a>
                    </td>
                </tr>

                  <tr>
                            <td ng-show="Listdata.length==0" colspan="9">
                                <div align="center" style="font-size:13px">No data Found. </div>                
                            </td>
                        </tr>
            </tbody>
        </table>
             <dir-pagination-controls class="getPageId" direction-links="true" pagination-id="nonAvaible" boundary-links="true"> </dir-pagination-controls>
    </div>
     <br />
     <br />

        </div>

</asp:Content>