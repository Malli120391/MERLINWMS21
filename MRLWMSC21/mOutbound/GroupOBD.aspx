<%@ Page Title="GroupOBD:." Language="C#" MasterPageFile="~/mOutbound/OutboundMaster.master" AutoEventWireup="true" CodeBehind="GroupOBD.aspx.cs" Inherits="MRLWMSC21.mOutbound.GroupOBD" %>
<asp:Content ID="Content1" ContentPlaceHolderID="OBContent" runat="server">

    <asp:ScriptManager runat="server" ID="spmngrGroupOBD" EnablePartialRendering="true" SupportsPartialRendering="true"></asp:ScriptManager>

    <script src="../Scripts/angular.min.js"></script>

    <script src="../mReports/Scripts/dirPagination.js"></script>
    <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />
    <script src="GroupOBD.js"></script>
     <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
    <script src="../Scripts/xlsx-model.js"></script>
<%--    <script src="../Scripts/xlsx.full.min.js"></script>--%>
    <script src="../Scripts/xlsx.full.min.js"></script>
    <script>

       
        $(document).ready(function () {
                //$("#txtFromdate").attr('readonly','readonly'); 
            $("#txtFromdate").datepicker({ dateFormat: "dd-M-yy" });
             //$("#txtTodate").attr('readonly','readonly'); 
            $("#txtTodate").datepicker({ dateFormat: "dd-M-yy" });
               $('#txtFromdate, #txtTodate').keypress(function () {
                return false;
            });
        });
    </script>


    <div class="container">
     <div  ng-app="myApp" ng-controller="GroupOutbound" >

         <div ng-show="blockUI" >
  <div style="width:100%; height:100%; z-index:999; position:fixed; top:0; left:0; right:0; bottom:0; align-items:center; display:flex; justify-content:center; background: rgba(255, 255, 255, 0.8);">
                                
                                <div style="align-self:center;" >
                                        <div class="spinner">
                                  <div class="bounce1"></div>
                                  <div class="bounce2"></div>
                                  <div class="bounce3"></div>
                                </div>

                                </div>
                                  
                                </div>
  
</div>
        
                    <asp:UpdateProgress ID="uprgGroupOBD" runat="server" AssociatedUpdatePanelID="upnlGroupOBD">
                    <ProgressTemplate>
                        <div style="width:100%; height:100%; z-index:999; position:fixed; top:0; left:0; right:0; bottom:0; align-items:center; display:flex; justify-content:center; background: #e0ddd8ba;">
                     
                        <div style="align-self:center;" >
                                <div class="spinner">
                            <div class="bounce1"></div>
                            <div class="bounce2"></div>
                            <div class="bounce3"></div>
                        </div>

                        </div>
                                  
                        </div>
                                
                                
                    </ProgressTemplate>
                    </asp:UpdateProgress>
                    <asp:UpdatePanel ID="upnlGroupOBD" runat="server" RenderMode="Inline" ViewStateMode="Enabled" ClientIDMode="Inherit" UpdateMode="Conditional">
                            <ContentTemplate>
         
         
                                 <div class="" >
                                      <div class="row">
                 
                                          <div class="col m2">
                                              <div class="flex">

                                               <div>
                                           
                                                <select ng-model="searchdata.TenantId"  ng-options="tnt.Id as tnt.Name for tnt in tenants" style="width: 280px;" ng-click="gettenant()" ng-change="getCustomerData()">
                                              
                                                    <option value=""> <%= GetGlobalResourceObject("Resource", "SelectTenant")%></option>
                                                </select>
                                                      <span class="errorMsg"></span>
                                                      </div>
                                                  </div>
                                           </div>
                                           <div class="col m2">
                                               <div class="flex">
                                                   <div>                          
                                                <select ng-model="searchdata.WareHouseId" ng-options="wh.Id as wh.Name for wh in WareHouseData" ng-change="LoadDock()">
                                                
                                                    <option value=""><%= GetGlobalResourceObject("Resource", "SelectWarehouse")%> </option>
                                                </select>
                                                       <span class="errorMsg"></span>
                                                       </div>
                                               </div>
                                           </div>
                                          <div class="col m2">
                                              <div class="flex">
                                                  <div>
                                                   <input type="text" list="cust" required="" id="txtcust"  ng-model="searchdata.CustomerName"  ng-keyup="getCustomerData()" ng-click="getCustomerData()" ng-change="getCustomerData()" />
                                                
                                                       <label> <%= GetGlobalResourceObject("Resource", "Customer")%>  </label>
                                                      <datalist id="cust" >
                                                    <select>
                                                    <option   ng-repeat="cust in customerdata" value="{{cust.Name}}"></option>
                                                    </select>
                                                    </datalist>
                                                       </div>
                                                  </div>
                                            </div>
                                                                <div class="col m2">
                                                                    <div class="flex">
                                                                        <div>                                               
                                                                  <input type="text" required=""   ng-model="searchdata.FromDate"  id="txtFromdate" onpaste="return false" />
                                                                    
                                                                             <label> <%= GetGlobalResourceObject("Resource", "FromDate")%> </label>
                                                                          </div> 
                                                              </div>
                                                                        </div>

                                                              <div class="col m2">
                                                                  <div class="flex">
                                                                      <div>
                                                                  <input type="text" required="required"  ng-model="searchdata.ToDate"  id="txtTodate" onpaste="return false"/>
                                                             
                                                                           <label><%= GetGlobalResourceObject("Resource", "ToDate")%>  </label>
                                                                          </div>
                                                                      </div>
                                                                                   </div>
                                                           <div class="col m2">
                                                                                       <gap5></gap5>
                                                                <%-- <button type="button"  ng-click = "GetOBDList()" class="btn btn-primary obd">Search <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>--%>
                                                                <button type="button"  ng-click = "GetOBDList()" class="btn btn-primary obd"> <%= GetGlobalResourceObject("Resource", "Search")%> <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                                                                                                 </div>
                                      </div>
           
                                  </div> 
                                 <gap5></gap5>
                                   <table style="margin-left:0.6em">
                                                            <tr><td>
                                                               <%-- <b>Total Volume (CBM):</b> {{MainVolume}}--%>
                                                                 <b> <%= GetGlobalResourceObject("Resource", "TotalVolumeCBM")%></b> {{MainVolume}}
                                                                </td>
                                                                <%--<td><b>Total Weight (KG):</b> {{MainWeight}}</td>--%>
                                                                <td><b><%= GetGlobalResourceObject("Resource", "TotalWeightKG")%> </b> {{MainWeight}}</td>
                                                            </tr>
                                                        </table>
                                 <br />
                                 <table class='table-striped' id="tbldatasonumbers" >
                                     <thead>
                                          <tr class=''>
                                             
                                               <th style="text-align: center"><div class="checkbox">
                                                      
                                                      <input type="checkbox" id="allselect" class="allselect" ng-change="selectAll()" ng-model="myValue" />
                                                      <label for="checkAll" ></label>
                                                  </div></th>
                                              <%--  <th style="text-align: center">Tenant</th>--%>
                                                <th style="text-align: center"> <%= GetGlobalResourceObject("Resource", "Tenant")%> </th>
                                               <%-- <th style="text-align: center">Warehouse</th>--%>
                                               <th style="text-align: center"> <%= GetGlobalResourceObject("Resource", "Warehouse")%> </th>

                                                <%--<th style="text-align: center">Delivery Doc. #</th>--%>
                                              <th style="text-align: center"><%= GetGlobalResourceObject("Resource", "DeliveryDoc")%> </th>
                                                <%--<th style="text-align: center">Doc. Date</th>--%>
                                              <th style="text-align: center"><%= GetGlobalResourceObject("Resource", "DocDate")%>  </th>
                                               <%-- <th style="text-align: center">Customer  </th>--%>
                                               <th style="text-align: center"><%= GetGlobalResourceObject("Resource", "Customer")%>   </th>
                                               <%--<th style="text-align: center">Volume</th>--%>
                                              <th style="text-align: center"> <%= GetGlobalResourceObject("Resource", "Volume")%></th>
                                                <%--<th style="text-align: center">Weight  </th>--%>
                                              <th style="text-align: center"> <%= GetGlobalResourceObject("Resource", "Weight")%>  </th>
                        
                                            </tr>
                                     </thead>
                   
                                     <tbody>
                                        <tr ng-if="obddata.length > 0" ng-repeat="itemInfo in obddata" class='mytableOutboundBodyTR'>
                                            <td style="text-align:center !important;"> <div class="checkbox"> <input type="checkbox" ng-model="itemInfo.Isselected"  id="ng-change-example{{$index+1}} " ng-change="calcualtedimension()"/><label for="ng-change-example{{$index+1}}"> </div></td>
                                            <td style="text-align:center !important;" >{{ itemInfo.tenantname }}</td>
                                            <td style="text-align:center !important;">{{ itemInfo.WHCode }}</td>
                                                <td style="text-align:center !important;">{{ itemInfo.OBDNumber }}</td>
                                                <td style="text-align:center !important;">{{ itemInfo.OBDDate | date : 'dd-MMM-yyyy' }}</td>
                                 
                                                <td style="text-align:center !important;">{{ itemInfo.CustomerName }}</td>
                                                <td style="text-align:center !important;">{{ itemInfo.Volume }}</td>
                                                <td style="text-align:center !important;">{{ itemInfo.Weight }}</td>
                                   
                                        </tr>
                                         <tr ng-if="obddata.length == 0">
										  <%--  <td colspan="8" style="text-align:center !important;">No Data Found</td>--%>
                                               <td colspan="8" style="text-align:center !important;"><%= GetGlobalResourceObject("Resource", "NoDataFound")%></td>
									    </tr>
                                    </tbody>
                  
                                        </table>
                               <gap5></gap5>
                               <div>   
                                   <div class="row">
                                        <div class="col m2">
                                            <div class="flex">
                                                <div>                           
                                          <%--<span style="color:red">*</span>--%>                                                    
                                           <select ng-model="DockId" ng-options="Doc.Id as Doc.Name for Doc in Docks">
                                                   <%-- <option value="">Select Dock</option>--%>
                                                <option value=""> <%= GetGlobalResourceObject("Resource", "SelectDock")%> </option>
                                                </select>
                                                     <span class="errorMsg"></span>
                                                    </div>
                                                </div>
                                           </div>
                                       <div class="col m2">
                                           <div class="flex">
                                               <div>
                           
                                          <%--<span style="color:red">*</span>--%>                                  
                                           <select ng-model="VehicleTypeId"  ng-options="VT.Id as VT.Name for VT in VehicleTypes">
                                                   <%-- <option value="">Select Vehicle Type</option>--%>
                                                <option value=""><%= GetGlobalResourceObject("Resource", "SelectVehicleType")%> </option>
                                                </select>
                                                   <span class="errorMsg"></span>
                                                   </div>
                                               </div>
                                           </div>
                                       <div class="col m2">
                                           <div class="flex">
                                               <div>
                                          <%--<span style="color:red">*</span>--%> 
                                           <input type="text" required="required" ng-model="Vehicleno" maxlength="12"  id="txtVehicleno"/><span class="errorMsg"></span>
                                                  <%-- <label>Vehicle Number</label>--%>
                                                    <label><%= GetGlobalResourceObject("Resource", "VehicleNumber")%> </label>
                                                   </div>
                                               </div>
                                       </div>
                                       <div class="col m2">
                                           <div class="flex">
                                               <div>
                                           <%--<span style="color:red">*</span>    --%>
                                           <input type="text" required="required" ng-model="DriverName"  id="txtDriver"/><span class="errorMsg"></span>
                                                  <%-- <label>Driver Name</label>--%>
                                                    <label><%= GetGlobalResourceObject("Resource", "DriverName")%> </label>
                                                   </div>
                                               </div>
                                       </div>
                                            <div class="col m2">
                                                <div class="flex">
                                                    <div>                              
                                          <%--<span style="color:red">*</span>--%>
                                           <input type="text" required="required" onkeypress="return isNumber(event)" maxlength="10"   ng-model="Mobile"  id="txtMobile" ng-pattern="onlyNumbers" maxlength="10"  ng-minlength="10" oninput="validity.valid||(value='');" input-restrictor//>
                                                        <span class="errorMsg"></span>
                                                        <%--<label>Mobile Number</label>--%>
                                                        <label><%= GetGlobalResourceObject("Resource", "MobileNumber")%> </label>
                                                    </div>
                                                    </div>
                                       </div>
                                       <div class="col m2">
                                           <div style="float:right;padding-right:20px;padding-top:20px;">
                                    <%--  <button type="button" id="btnclick" ng-if="obddata!=undefined && obddata!=null && obddata.length!=0"  ng-click = "CreateGroupOBD()" class="btn btn-primary obd" style="width:160px;">Create Group OBD <%=MRLWMSC21Common.CommonLogic.btnfaSave %></button>--%>
                                                 <button type="button" id="btnclick" ng-if="obddata!=undefined && obddata!=null && obddata.length!=0"  ng-click = "CreateGroupOBD()" class="btn btn-primary obd" style="width:160px;"> <%= GetGlobalResourceObject("Resource", "CreateGroupOBD")%> <%=MRLWMSC21Common.CommonLogic.btnfaSave %></button>
                                 </div>
                                       </div>
                                 <%--      <td>
                                            <input type="text" id="txtVLPD"  placeholder="VLPD"  ng-click="getVehicleData()"  class="TextboxInventoryAuto" />&nbsp;&nbsp;
                                       </td>--%>
                                       </div>
                                           </div>

                                 
     
        

                            </div></div>
                             <br /><br />  
                </ContentTemplate>
        </asp:UpdatePanel>
                        
</asp:Content>
