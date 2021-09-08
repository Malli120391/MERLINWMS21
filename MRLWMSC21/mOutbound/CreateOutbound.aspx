<%@ Page Title="Create Outbound.. :." Language="C#" MasterPageFile="~/mOutbound/OutboundMaster.master" AutoEventWireup="true" CodeBehind="CreateOutbound.aspx.cs" Inherits="MRLWMSC21.mOutbound.CreateOutbound" %>
<asp:Content ID="Content1" ContentPlaceHolderID="OBContent" runat="server">
     <script src="../Scripts/angular.min.js"></script>
    <script src="../mReports/Scripts/dirPagination.js"></script>
    <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />
    <script src="CreateOutbound.js"></script>
     <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
    <script src="../Scripts/xlsx-model.js"></script>
    <script src="../Scripts/xlsx.full.min.js"></script>
    <script src="http://goessner.net/download/prj/jsonxml/json2xml.js"></script>
    <div  ng-app="myApp" ng-controller="createoutbound" >
        <style>
            select {
                width:200px !important;
                margin-right:10px;
            }

            #filetype {
                width: 70% !important;
                border: 1px solid #999 !important;
                padding: 5px !important;
                margin-bottom: 5px;
            }
                
        </style>
       <%-- <div id="blocker" ng-show="blockUI">
            <div>
                loading... 
                 <img src="../Images/ajax-loader.gif" style="width: 50px;" />
            </div>

        </div>--%>
        
       <div class="dashed"></div><br />   
        <div class="pagewidth">
         <table class=" " >
              <tr>
                 
                  <td>
                      <div class="flex">
                      <div>
                          
                        <select ng-model="searchdata.TenantId"  ng-options="tnt.Id as tnt.Name for tnt in tenants">
                            <option value="">Select Tenant</option>
                        </select><span class="errorMsg">*</span>  
                              </div>
                          </div>
                   </td>
                   <td>
                       <div class="flex">
                       <div>
                           
                       <select ng-model="searchdata.WareHouseId"  ng-options="wh.Id as wh.Name for wh in WareHouseData">
                            <option value="">Select Warehouse</option>
                        </select>
                           <span class="errorMsg">*</span>  
                               </div>
                       </div>
                   </td>
                  <td>
                      <div class="flex"> 
                        <div>
                          
                            <select ng-model="searchdata.DeliveryTypeId" ng-options="obt.Id as obt.Name for obt in OBDTypesData" readonly="true">
                            
                       
                             </div>
                     </div>
                  </td>
                  <td>
                      <div class="flex">
                         
                            <input id="filetype" type="file"  Title="Import To Excel" name="upload"   xlsx-model="excel" multiple >
                             
                              <p class="label2">
                             Import SO  <a class="" href="../Template/sonumbes.xls"="../Template/sonumbes.xls">Get Template <i class="material-icons vl">file_download</i></a>
                              </p>
                          
                          </div>
                   </td>
                  <td>
                      <div class="flex" style="text-align: right;">
                          
                          <button id="btnimport" type="button" ng-click="ImportData(excel)" class="btn btn-primary">Create Outbound <i class="fa fa-folder-open" aria-hidden="true"></i></button>
                          <button type="reset" class="btn btn-primary" ng-click="cleardata()" style="margin-top: 2px;">Clear <i class="space fa fa-ban" aria-hidden="true"></i></button>
                      </div>
                  </td>

              </tr>
          </table> 
       <br />
        <div class="row" style="margin: 0;padding: 0px 10px;"  >
            <div class="">
                <table class="mytableOutbound" style="">
                    <thead class="mytableOutboundHeaderTR">
                        <tr style="height: 30px;">
                            <th>Outbound ID</th>
                            <th>Outbound No</th>
                            <th>Customer Name</th>
                        </tr>
                    </thead>
                    <tbody class="mytableOutboundBodyTR">
                        <tr dir-paginate="ROI in ResultOutinfon|filter:search|itemsPerPage:100" pagination-id="main">
                            <td align="right">{{ROI.OutboundID}}</td>
                            <td>{{ROI.OBDRefNo}}</td>
                            <td>{{ROI.CustomerName}}</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <div class="row" style="margin: 0;" style="width: 80%;" ng-if="hide" >
            <div class="col-sm-6 col-lg-6" style="width: 1000px; overflow: auto; margin: 0; padding: 0;">
              <%--  <div class="divmainwidth" id="div12" style="width: 80%;">
                </div>--%>
               <%-- <div id="div12">

                </div>--%>
                <table class="mytableOutbound" style="width:140% !important;margin-left:0 !important;">
               <thead class="mytableOutboundHeaderTR">
                <tr style="height:30px;">
                    <th>Select</th>
                    <th>Line Number</th>
                    <th>SO Number</th>
                    <th>Mcode</th>
                    <th>Description</th>
                    <th>SO Quantity</th>
                    <th>PendingQty</th>
                    <th>AvailableQty</th>
                    <th>InProcessQty</th>
                    <th>BatchNo</th>
                    <th>SerialNo</th>
                    <th>ProjectRefNo</th>
                    <th>ExpDate</th>
                    <th>MfgDate</th>
                </tr>
                   </thead>
               <tbody  class="mytableOutboundBodyTR">
                   <tr dir-paginate="GSL in GETSOList|filter:search|itemsPerPage:100" pagination-id="main">
                   <td><input type="checkbox" id="ng-change-example1" ng-model="GSL.Isselected" /></td>
                       <td align="right">{{GSL.LineNumber}}</td>
                   <td>{{GSL.SONumber}}</td>
                   <td>{{GSL.Mcode}}</td>
                        <td>{{GSL.MDescription}}</td>
                   <td align="right">{{GSL.SoQuantity}}</td>
                    <td>{{GSL.PendingQty}}</td>
                    <td>{{GSL.AvailableQty}}</td>
                   <td align="right">{{GSL.InProcessQty}}</td>
                       <td>

                       </td>
                   <td align="right">{{GSL.BatchNo}}</td>
                   <td align="right">{{GSL.SerialNo}}</td>
                   <td align="right">{{GSL.ProjectRefNo}}</td>
                   <td align="right">{{GSL.ExpDate}}</td>
                   <td align="right">{{GSL.MfgDate}}</td>
               </tr>
                   </tbody>
            </table>
            </div>
        </div>
          <br />
        
        <div ng-if="hide" align="right" style="margin-right:1%;">
            <br />  
                      
                <img src="../Images/bx_loader.gif" id="imgLLoadingSAP" style="width:60px;display:none;" />
            <button type="button" id="btnclick"   ng-click="CreateOutbound();" class="addbuttonOutbound" style="width:130px;" >Create Delivery <%=MRLWMSC21Common.CommonLogic.btnfaSave %></button>
        </div>
    </div>
     <br /><br />  
</asp:Content>
