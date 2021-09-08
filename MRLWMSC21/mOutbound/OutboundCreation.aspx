<%@ Page Title="Create Outbound.. :." Language="C#" MasterPageFile="~/mOutbound/OutboundMaster.master" AutoEventWireup="true" CodeBehind="OutboundCreation.aspx.cs" Inherits="MRLWMSC21.mOutbound.OutboundCreation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="OBContent" runat="server">
    <script src="../Scripts/angular.min.js"></script>
    <script src="../mReports/Scripts/dirPagination.js"></script>
    <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />
    <script src="OutboundCreation.js"></script>
     <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
    <script src="../Scripts/xlsx-model.js"></script>
    <script src="../Scripts/xlsx.full.min.js"></script>
    <script src="http://goessner.net/download/prj/jsonxml/json2xml.js"></script>
    <style>
        select, input[type=text] {
            width:250px !important; 
                margin: 0;
        }

    </style>
     <div class="dashed"></div>
    <div ng-app="myApp" ng-controller="outboundcreate" class="pagewidth">

                <div ng-show="blockUI">
            <div style="width: 100%; height: 100vh; z-index: 999; position: fixed; top: 0; left: 0; right: 0; bottom: 0; align-items: center; display: flex; justify-content: center; background: rgba(255, 255, 255, 0.24); background: hsla(0, 0%, 100%, 0.72);">

                <div style="align-self: center;">
                  <img width="60" src="../Images/preloader.svg" />

                </div>

            </div>

        </div>

        <div class="row">
            <div class="col m12" flex end>
                 <div class="flex">
                    <input type="text" id="txtWarehouse"  required="" />
                    <label>Warehouse</label>
                      <span class="errorMsg"></span>  
                </div>&emsp;
                 <div class="flex">
                    <input type="text" id="txtTenant" required="" />
                    <label>Tenant</label>
                     <span class="errorMsg"></span>  
                </div>&emsp;
                 <div class="flex">
                    <button id="btnimport" type="button" ng-click="GetData()" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "GetSOs")%> <i class="material-icons">call_missed_outgoing</i></button>&nbsp; 
                                              <button type="reset" class="btn btn-primary" ng-click="cleardata()" style="margin-top: 2px;"><%= GetGlobalResourceObject("Resource", "Clear")%> <i class="material-icons">clear_all</i></button>
                 </div>
            </div>
        </div>




<%--       
   <div class="flex__ right" style="margin-bottom:10px;">
                          <div class="flex">
                                                          <select ng-model="searchdata.TenantId" ng-readonly="tenantedit" style="padding-right: 30px !important; margin-right: 30px !important;" required=""  ng-options="tnt.Id as tnt.Name for tnt in tenants">
                                <option value="">Select Tenant</option>
                            </select><span class="errorMsg">*</span>  
                           
                              </div>&nbsp;

                          <div class="flex" >
                          
                            
                                <button id="btnimport" type="button" ng-click="GetData()" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "GetSOs")%> <i class="material-icons">call_missed_outgoing</i></button>&nbsp;
                             
                              <button type="reset" class="btn btn-primary" ng-click="cleardata()" style="margin-top: 2px;"><%= GetGlobalResourceObject("Resource", "Clear")%> <i class="material-icons">clear_all</i></button>
                          </div>

   </div> --%>
      
      
        <div ng-if="SOInfo!=undefined && SOInfo!=null && SOInfo.length!=0">
            <table class="table">
            <thead>
                <tr class="mytableOutboundHeaderTR">
                  
                        <th><div   class="checkbox"  ><input ng-click="SelectAll(checked)" type="checkbox" id="allchecked"  ng-checked="v"  ng-model="checked"/>
                            <label for="allchecked"></label>
                            </div>
                            <br />
                        </th>
                      <%--  <th  ng-click="sort('SONumber')">SO Number <span class="glyphicon sort-icon" ng-show="sortKey=='SONumber'" ng-class="{'glyphicon-chevron-up':reverse,'glyphicon-chevron-down':!reverse}"></span></th>--%>
                      <th  ng-click="sort('SONumber')"> <%= GetGlobalResourceObject("Resource", "SONumber")%> <span class="glyphicon sort-icon" ng-show="sortKey=='SONumber'" ng-class="{'glyphicon-chevron-up':reverse,'glyphicon-chevron-down':!reverse}"></span></th>
                      <%--  <th  ng-click="sort('CustomerName')">Customer<span class="glyphicon sort-icon" ng-show="sortKey=='CustomerName'" ng-click = "getCustomrs()" ng-class="{'glyphicon-chevron-up':reverse,'glyphicon-chevron-down':!reverse}"></span></th>--%>
                      <th  ng-click="sort('CustomerName')"> <%= GetGlobalResourceObject("Resource", "Customer")%> <span class="glyphicon sort-icon" ng-show="sortKey=='CustomerName'" ng-click = "getCustomrs()" ng-class="{'glyphicon-chevron-up':reverse,'glyphicon-chevron-down':!reverse}"></span></th>
                        <%--<th   ng-click="sort('SODate')">SO Date<span class="glyphicon sort-icon" ng-show="sortKey=='SODate'" ng-class="{'glyphicon-chevron-up':reverse,'glyphicon-chevron-down':!reverse}"></span></th>--%>
                    <th   ng-click="sort('SODate')"><%= GetGlobalResourceObject("Resource", "SODate")%> <span class="glyphicon sort-icon" ng-show="sortKey=='SODate'" ng-class="{'glyphicon-chevron-up':reverse,'glyphicon-chevron-down':!reverse}"></span></th>
                       <%-- <th   ng-click="sort('SOType')">SO Type<span class="glyphicon sort-icon" ng-show="sortKey=='SOType'" ng-class="{'glyphicon-chevron-up':reverse,'glyphicon-chevron-down':!reverse}"></span></th> --%>
                     <th   ng-click="sort('SOType')"><%= GetGlobalResourceObject("Resource", "SOType")%> <span class="glyphicon sort-icon" ng-show="sortKey=='SOType'" ng-class="{'glyphicon-chevron-up':reverse,'glyphicon-chevron-down':!reverse}"></span></th> 
                        <%--<th >Status</th>--%>
                    <th > <%= GetGlobalResourceObject("Resource", "Status")%> </th>
                        <th ></th>                        
                     </tr>
            </thead>
            <tbody >

                <tr class="mytableOutboundBodyTR" dir-paginate="SO in SOInfo|orderBy:sortKey:reverse|filter:search|itemsPerPage:100" align="center">
                    <td>
                        <div class="checkbox"><input  type="checkbox" ng-model="SO.IsSelected" ng-onclick=""; id="Checkbox{{$index + 1}}" ng-click="uncheckedparent()"  /><label for=""></label></div></td>
                    <td class="aligntext">{{ SO.SOnumber }}</td>
                    <td class="aligntext">{{ SO.CustomerName }}</td>
                    <td class="aligntext">{{ SO.SODate }}</td>
                    <td >{{ SO.SOType }}</td>
                    <td >{{ SO.SOSatus }}</td>
                    <td></td>
                </tr>
            </tbody>
        </table>
            <br />
        <div class="flex__  right">
               
                       <div class="flex" style="display:none">
                        
                           
                       <select ng-model="searchdata.WareHouseId" ng-readonly="warehouseedit" style="padding-right:30px !important; margin-right:30px !important;"  
                           required="" ng-options="wh.Id as wh.Name for wh in WareHouseData">
                           <%-- <option value="">Select Warehouse</option>--%>
                            <option value=""> <%= GetGlobalResourceObject("Resource", "SelectWarehouse")%></option>
                        </select>
                           <span class="errorMsg">*</span>  
                           
                       </div>&nbsp;
                  
                        <div class="flex">
                        
                           
                       <select  style="padding-right:30px !important; margin-right:30px !important;"  required="" id="dropPriority">
                           
                            <option value=""> Select OBD Priority</option>
                            <option value="1"> Normal</option>
                           <option value="2"> High Priority</option>
                        </select>
                           <span class="errorMsg">*</span>  
                           
                       </div>&nbsp;
                
                      <div class="flex"> 
                    
                            <select ng-model="searchdata.DeliveryTypeId" ng-readonly="obdtypeedit"  style=" margin-right:30px !important;" required="" ng-options="obt.Id as obt.Name for obt in OBDTypesData">
                           
                        </select>   
                          
                     </div> &nbsp;
                 
            <div><%--<button id="btncreate" ng-if="show" type="button" ng-click="Create()" class="btn btn-primary">Create Outbound <i class="fa fa-folder-open" aria-hidden="true"></i></button>--%>
                <button id="btncreate" ng-if="show" type="button" ng-click="Create()" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "CreateOutbound")%> <i class="fa fa-folder-open" aria-hidden="true"></i></button>
            </div>

        </div>
        </div>
        
       
        <div ng-if="ResultOutinfon!=null && ResultOutinfon!=undefined && ResultOutinfon.length!=0" class="row" style="margin: 0;padding: 0px 10px;"  >
            <div class="">
                <table class="table-striped" style="">
                    <thead>
                        <tr>
                            <%--<th>S. No.</th>--%>
                            <th> <%= GetGlobalResourceObject("Resource", "SNo")%></th>
                            <%--<th>OBD Number</th>--%>
                            <th> <%= GetGlobalResourceObject("Resource", "OBDNumber")%> </th>
                            <%--<th>Customer</th>--%>
                            <th> <%= GetGlobalResourceObject("Resource", "Customer")%>  </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr dir-paginate="ROI in ResultOutinfon|filter:search|itemsPerPage:100" pagination-id="main">
                            <td align="right">{{$index+1}}</td>
                            <td>{{ROI.OBDRefNo}}</td>
                            <td>{{ROI.CustomerName}}</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
     
    </div>
     <br />
     <br />
     <br />
     <br />



</asp:Content>
