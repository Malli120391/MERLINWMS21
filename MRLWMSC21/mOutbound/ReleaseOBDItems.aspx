<%@ Page Title="Release Outbound" Language="C#"  MasterPageFile="~/mOutbound/OutboundMaster.master" AutoEventWireup="true" CodeBehind="ReleaseOBDItems.aspx.cs" Inherits="MRLWMSC21.mOutbound.ReleaseOBDItems" %>
<asp:Content ID="Content1" ContentPlaceHolderID="OBContent" runat="server">
   
    <script src="../mInventory/Scripts/angular.min.js"></script>
  <script src="../mInventory/Scripts/dirPagination.js"></script>
    <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />
    <script src="ReleaseOBDItems.js"></script>
     <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
         <link href="../mInventory/Scripts/bootstrap.min.css" rel="stylesheet" />
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>

    <style>
        .checkbox {
            margin-top: 0px !important;
        }

        .mytablechildOutbound tr td {
            padding: 9px 15px 9px 0px !important;
        }

        .setwidth select, input {
            width: 100% !important;
        }

        .table-striped {
            white-space: nowrap;
        }

            .table-striped td input {
                width: 30px !important;
            }

            .flex select{
                padding:0px !important;
            }
            .btn{
                display:block !important;
                width:100% !important;
            }
     </style>

   
    <div class="dashed"></div>
    <div class="container">
     <div ng-app="Myapp" ng-controller="ReleaseOutboundItems" class="">
      
          <div ng-show="blockUI">
  <div style="width:100%; height:100vh; z-index:999; position:fixed; top:0; left:0; right:0; bottom:0; align-items:center; display:flex; justify-content:center; background: rgba(255, 255, 255, 0.24); background: hsla(0, 0%, 100%, 0.72);">

      <div style="align-self: center;">
          <img src="<%= ResolveUrl("~/Images/preloader.svg") %>" loader />

      </div>

  </div>
  
</div>
         <%-- <div style="text-align:left;color:var(--sideNav-bg) !important;font-family:Arial;font-size:11pt;font-weight:bold;">OBD Item Details</div>--%>
          <div style="text-align:left;color:var(--sideNav-bg) !important;font-family:Arial;font-size:11pt;font-weight:bold;"> <%= GetGlobalResourceObject("Resource", "OBDNumber")%> : {{OBDNumber}}</div>
                                                             <div class="lineheight"></div>
                                                            <br />
                                                            <table>
                                                                <tr><td width="33%">
                                                                   <%-- <b>Total Volume (CBM) :</b> {{MainVolume}}--%>
                                                                     <b> <%= GetGlobalResourceObject("Resource", "TotalVolumeCBM")%> :</b> {{MainVolume}}
                                                                    </td>
                                                                    <td width="1%">&nbsp;</td>
                                                                   <%-- <td width="32%"><b>Total Weight (KG) :</b> {{MainWeight}}</td>--%>
                                                                     <td width="32%"><b> <%= GetGlobalResourceObject("Resource", "TotalWeightKG")%> :</b> {{MainWeight}}</td>
                                                                     <td width="1%">&nbsp;</td>
                                                                   <%-- <td style="color:red" width="33%">Suggestion: {{AvailableContainer}}</td>--%>
                                                                     <td style="color:red" width="33%"><%= GetGlobalResourceObject("Resource", "Suggestion")%> : {{AvailableContainer}}</td>
                                                                </tr>
                                                            </table>

                                                             <div>

                                                                 <table class="table-striped">
                                                                     <thead>
                                                                         <tr class="">
                                                                             <th>
                                                                                 <div class="checkbox">
                                                                                    <%-- <input id="checkAll" ng-model="selectall" type="checkbox" />--%>
                                                                                     <input type="checkbox" id="allselect" class="allselect" ng-change="selectAll()" ng-model="myValue" />
                                                                                     <label for="checkAll"></label>
                                                                                     
                                                                                 </div>
                                                                             </th>

                                                                             <th><%= GetGlobalResourceObject("Resource", "SONumber")%> </th>
                                                                             <th><%= GetGlobalResourceObject("Resource", "LineNumber")%></th>

                                                                             <th ><%= GetGlobalResourceObject("Resource", "PartNo")%> </th>
                                                                             <th number><%= GetGlobalResourceObject("Resource", "SOQty")%></th>
                                                                              <th number>Available Qty.</th>

                                                                             <th number><%= GetGlobalResourceObject("Resource", "PendingQty")%> </th>

                                                                             <th number><%= GetGlobalResourceObject("Resource", "DeliveryQty")%>  </th>

                                                                             <th><%= GetGlobalResourceObject("Resource", "UnitVolumeCBM")%> </th>

                                                                             <th><%= GetGlobalResourceObject("Resource", "UnitWeightKG")%> </th>

                                                                             <th><%= GetGlobalResourceObject("Resource", "TotalVolumeCBM")%> </th>

                                                                             <th><%= GetGlobalResourceObject("Resource", "TotalWeightKG")%></th>

                                                                           <%--  <th number><%= GetGlobalResourceObject("Resource", "RevertQty")%> </th>--%>
                                                                             <th ng-if="!DisplayReleaseData"></th>
                                                                         </tr>
                                                                     </thead>
                                                                      <tbody class="mytableOutboundBodyTR">
                                                                                     <tr ng-repeat="OBD in OBDwiseItems" class="mytableOutboundBodyTR">
                                                                                     <%-- <tr class="mytableOutboundBodyTR">--%>
                                                                                           <td style="text-align: center;">
                                                                                           <%--<div class="checkbox"><input type="checkbox" id="chk{{$index +1 }}" ng-model="OBD.IsSelected" class="cbkSelected" data-DetailID="{{OBD.SODetailsID}}"  id="ng-change-example1{{$index +1 }}" data-index="{{$index +1 }}" ng-change="calcualtedimension()"/><label for="ng-change-example{{$index +1 }}"> </label></div></td>--%>
                                                                                               <div class="checkbox"><input type="checkbox" id="chk{{$index +1 }}" ng-model="OBD.IsSelected" class="cbkSelected" data-DetailID="{{OBD.SODetailsID}}" data-index="{{$index +1 }}" ng-change="calcualtedimension()"/><label for="ng-change-example{{$index +1 }}"> </label></div></td>
                                                                                           <td class="aligntext">{{OBD.SONumber}}</td>
                                                                                           <td style="text-align:center;">{{OBD.LineNumber}}</td> 
                                                                                           <td class="aligntext">{{OBD.MCode}}</td> 
                                                                                           <td number>{{OBD.SOQty}}</td>   
                                                                                           <td number>{{OBD.AvailableQty}}</td>   
                                                                                           <td number>{{OBD.PendingQty}}</td>  
                                                                                           <td number"><input type="text" ng-model="OBD.DeliveryQty" ng-Keyup="checkQty(OBD)" class="DeliveryQty{{$index +1 }}" style="text-align:right; border:0px; border-bottom:1px solid; border-color: var(--paper-grey-300) !important;width:70px;" /></td>  
                                                                                           <td number>{{OBD.VolumeinCBM}}</td>
                                                                                           <td number>{{OBD.MWeight}}</td>
                                                                                           <td number>{{OBD.TotalVolume}}</td>
                                                                                           <td number>{{OBD.TotalWeight}}</td>
                                                                                         <%--<td style="text-align:center;"><input type="text" ng-model="OBD.RevertQty" onkeypress="return isNumber(event)" ng-Keyup="checkRevertQty(OBD)" style="text-align:right; border:0px; border-bottom:1px solid; border-color: var(--paper-grey-300) !important;width:60px;" /></td>--%>  
                                                                                         <%--<td ng-if="!DisplayReleaseData;"><button type="button" id="btnRevert" ng-if="OBD.RevertQty>0" class="btn btn-primary"  ng-click="revertLineItmes(OBD)">Revert <%=MRLWMSC21Common.CommonLogic.btnfaTransfer %></button>--%>
                                                                                    </tr>
                                                        
                                                                                </tbody>

                                                                 </table>
                                                                
                                                             </div>
                                                            <br />


                                                            <%--<table>
                                                                <tr>
                                                                    <td>
                                                                        
                                                                    </td>
                                                                    <td colspan="5" style="display: flex;float: right;">
                                                                        <div class="flex">
                                                                            <div>
                                                                                <select ng-model="DockId" class="DropdownGH" ng-options="Doc.Id as Doc.Name for Doc in Docks" ng-change="LoadDock()" ng-show="DisplayReleaseData" required="">
                                                                                    <option value=""><%= GetGlobalResourceObject("Resource", "SelectDock")%></option>
                                                                                </select>
                                                                                <span class="errorMsg"></span>
                                                                            </div>

                                                                        </div>
                                                                        
                                                                        <div style="padding-left: 41px;padding-top: 16px;">
                                                                            
                                                                        
                                                                            <button type="button" ng-show="DisplayReleaseData" class="btn btn-primary" ng-click="ReleaseOBDwithDock()" id="btnCreate">Release OBD  <%=MRLWMSC21Common.CommonLogic.btnfaTransfer %></button>
                                                                            

                                                                            <button type="button" ng-hide="DisplayReleaseData" title="Regenerate Suggestions" id="btnRelease2" class="btn btn-primary" ng-click="ReleaseOBDwithDock()"><%= GetGlobalResourceObject("Resource", "RegenarateSuggetion")%>  <%=MRLWMSC21Common.CommonLogic.btnfaTransfer %></button>

                                                                        </div>
                                                                    </td>

                                                                </tr>
                                                            </table>--%>
                                                     <div class="row">
                                                         <div class="col m3 s4 offset-m7">
                                                             <div class="flex">
                                                                 <div ng-show="DisplayReleaseData">
                                                                     <select ng-model="DockId" class="DropdownGH" ng-options="Doc.Id as Doc.Name for Doc in Docks" ng-change="LoadDock()"  required="">
                                                                         <option value=""><%= GetGlobalResourceObject("Resource", "SelectDock")%></option>
                                                                     </select>
                                                                     <span class="errorMsg"></span>
                                                                 </div>

                                                             </div>

                                                         </div> 
                                                         <div class="col m2 s4" style="float:right">
                                                             <div class="flex" style="text-align:right; width:100%;">
                                                                 <div>
                                                                     <button type="button" ng-show="DisplayReleaseData" class="btn btn-primary" ng-click="ReleaseOBDwithDock()" id="btnCreate">Release OBD  <%=MRLWMSC21Common.CommonLogic.btnfaTransfer %></button>
                                                                     <button style="height:32px;width:175px;" type="button" ng-hide="DisplayReleaseData"  id="btnRelease2" class="btn btn-primary" ng-click="ReleaseOBDwithDock()"><%= GetGlobalResourceObject("Resource", "RegenarateSuggetion")%>  <%=MRLWMSC21Common.CommonLogic.btnfaTransfer %></button>

                                                                 </div>

                                                             </div>

                                                         </div>

                                                     </div>

         <br /><br />   
         <div ng-if="resultantassigndata!=undefined && resultantassigndata!=null && resultantassigndata.length!=0">
                                                      <table class="mytableOutbound" style="width: 100% !important; margin-left: 0 !important;">
                                                            <thead class="mytableOutboundHeaderTR">
                                                                <tr style="height: 30px;">
                                                                    <th> </th>
                                                                   <%-- <th>SKU</th>--%>
                                                                     <th> <%= GetGlobalResourceObject("Resource", "SKU")%></th>
                                                                    <%--<th>SONumber  </th>--%>
                                                                    <th><%= GetGlobalResourceObject("Resource", "SONumber")%>  </th>
                                                                   <%-- <th>Status</th>--%>
                                                                     <th><%= GetGlobalResourceObject("Resource", "Status")%></th>
                                                                  
                                                                    
                                                                </tr>
                                                            </thead>
                                                            <tbody ng-repeat="DS in resultantassigndata"  align="center" class="mytableOutboundBodyTR">
                                                                <tr>
                                                                    <td><input  type="checkbox" ng-model="DS.Isselected" ng-onclick="";  ng-change="GetInfo(DS.RowID)" id="Checkbox1"  ng-if="DS.ChildErrorCodes.length>0"/></td>
                                                                    <td>{{DS.MCode}}</td>
                                                                    <td>{{DS.SONumber}}</td>
                                                                    <td>{{DS.ErrorCodeMessage}}</td>
                                                                </tr>
                                                               
                                                                <tr class="trPO1" ng-if="DS.Isselected">
                                                                    <td colspan="4" align="center">
                                                                        <table class="mytableOutbound" style="width: 98% !important;">
                                                                            <thead>
                                                                                <tr class="mytableOutboundchildHeaderTR">
                                                                                    <%--<th>Batch No.</th>--%>
                                                                                    <th><%= GetGlobalResourceObject("Resource", "BatchNo")%> </th>
                                                                                    <%--<th>Mfg. Date</th>--%>
                                                                                    <th> <%= GetGlobalResourceObject("Resource", "MfgDate")%> </th>
                                                                                    <%--<th>Exp. Date</th>--%>
                                                                                    <th> <%= GetGlobalResourceObject("Resource", "ExpDate")%> </th>
                                                                                   <%-- <th>Serial No.</th>--%>
                                                                                     <th><%= GetGlobalResourceObject("Resource", "SerialNo")%></th>
                                                                                    <%--<th>MRP No.</th>--%>
                                                                                    
                                                                                    <th><%= GetGlobalResourceObject("Resource", "MRPNo")%> </th>

                                                                                   <%-- <th>Error Message</th>--%>
                                                                                 <%--    <th>Error Message</th>--%>
                                                                                        <th> <%= GetGlobalResourceObject("Resource", "ErrorMessage")%> </th>

                                                                                </tr>
                                                                            </thead>
                                                                            <tbody>
                                                                                <tr  ng-repeat="CEC in DS.ChildErrorCodes" class="mytableOutboundBodyTR">
                                                                                    <td >{{ CEC.BatchNo }}</td>
                                                                                    <td>{{CEC.MfgDate}}</td>
                                                                                    <td >{{CEC.ExpDate}}</td>
                                                                                    <td >{{CEC.SerialNo}}</td>
                                                                                    <td >{{CEC.ProjectRefBo}}</td>
                                                                                    <td >{{CEC.ErrorCodeMessage}} </td>
                                                                                </tr>

                                                                            </tbody>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                            </div>



         <div class="modal inmodal" id="showColOptions" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog" style="width:65% !important;">
            <div class="modal-content animated fadeIn">
                <div class="modal-header">
                   <%-- <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>--%>
                     <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only"> <%= GetGlobalResourceObject("Resource", "Close")%></span></button>
                    <%--<h4 class="modal-title">Dock Information</h4>--%>
                    <h4 class="modal-title"><%= GetGlobalResourceObject("Resource", "DockInformation")%></h4>
                </div>
                <div class="modal-body setwidth">
                    <div id="divValidationCycleCountMessages" class="text-danger" style="color:red !important;"></div>
                    <p></p>
                    <div class="row">
                        <div class="col-lg-3">
                            <div class="flex">
                                <div>
                                    <%--<span style="color:red">*</span>--%>
                                    <select ng-model="DockId" ng-options="Doc.Id as Doc.Name for Doc in Docks" ng-change="LoadDock()">
                                        <%--<option value="">Select Dock</option>--%>
                                        <option value=""> <%= GetGlobalResourceObject("Resource", "SelectDock")%></option>
                                    </select>
                                    <span class="errorMsg"></span>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3">
                            <div class="flex">
                                <div>

                                    <%--<span style="color:red">*</span>--%>
                                    <select ng-model="VehicleTypeId" ng-options="VT.Id as VT.Name for VT in VehicleTypes">
                                      <%--  <option value="">Select Vehicle Type</option>--%>
                                          <option value=""> <%= GetGlobalResourceObject("Resource", "SelectVehicleType")%> </option>
                                    </select>
                                    <%--<span class="errorMsg"></span>--%>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3">
                            <div class="flex">
                                <div>
                                    <%--<span style="color:red">*</span>--%>
                                    <input type="text" placeholder="Vehicle Number" maxlength="12"  ng-model="Vehicleno" id="txtVehicleno" />
                                    <%--<span class="errorMsg"></span>--%>
                                </div>
                            </div>

                        </div>
                        <div class="col-lg-3">
                            <div class="flex">
                                <div>
                                    <%--<span style="color:red">*</span>    --%>
                                    <input type="text" placeholder="Driver Name" ng-model="DriverName" id="txtDriver" />
                                   <%-- <span class="errorMsg"></span>--%>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-3">
                             <div class="flex">
                                                    <div>                              
                                          <%--<span style="color:red">*</span>--%>
                                           <input type="text" placeholder=" Mobile Number" onkeypress="return isNumber(event)" maxlength="10"   ng-model="Mobile"  id="txtMobile" ng-pattern="onlyNumbers" maxlength="10"  ng-minlength="10" oninput="validity.valid||(value='');" input-restrictor//>
                                                       <%-- <span class="errorMsg"></span>--%>
                                                    </div>
                                                    </div>
                        </div>
                    </div>
                  
                     
                </div>
                <div class="modal-footer">
                    <input type="hidden" value="0" id="CCM_TRN_CycleCount_ID" class="fieldToGet" />
                   <%-- <button type="button" class="btn btn-primary" data-dismiss="modal" style="color:#fff !important;">Close</button>--%>
                     <button type="button" class="btn btn-primary" data-dismiss="modal" style="color:#fff !important;"> <%= GetGlobalResourceObject("Resource", "Close")%></button>
                   <%-- <button type="button" class="btn btn-primary" ng-click="ReleaseOBDwithDock()" id="btnCreate" onclick="return UpsertData();">Release OBD</button>--%>
                     <%--<button type="button" class="btn btn-primary" ng-click="ReleaseOBDwithDock()" id="btnCreate"> <%= GetGlobalResourceObject("Resource", "ReleaseOBD")%></button> Commented By M.D.Prasad On 26-Dec-2019 --%>
                </div>
            </div>
        </div>
    </div>

         </div>
        </div>
</asp:Content>
