<%@ Page Title="Gate EntryList" Language="C#" MasterPageFile="~/mInbound/InboundMaster.master" AutoEventWireup="true" CodeBehind="GateEntryList.aspx.cs" Inherits="MRLWMSC21.mInbound.GateEntryList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="IBContent" runat="server">
     <script src="../Scripts/angular.min.js"></script>
    <script src="../mReports/Scripts/dirPagination.js"></script>
    <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />
     <link href="../mInventory/Scripts/bootstrap.min.css" rel="stylesheet" />
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>
        <script src="GateEntryList.js"></script>
   

    <div class="angulardiv container" ng-app="myApp" ng-controller="GateList">
        
<div ng-show="blockUI">
  <div style="width:100%; height:100vh; z-index:999; position:fixed; top:0; left:0; right:0; bottom:0; align-items:center; display:flex; justify-content:center; background: rgba(255, 255, 255, 0.24); background: hsla(0, 0%, 100%, 0.72);">
                                
                                <div style="align-self:center;" >
                                       <img src="../Images/preloader.svg" width="60" />

                                </div>
                                  
                                </div>
  
</div>
           <div style="">
            <div class="row">
                <div class="col m2 s3 offset-m6">
                    <div class="flex">
                        <div>
                            <input type="text" id="txtvehicle"   required=""/>

                           <%-- <label>Vehicle No.</label>--%>
                             <label><%= GetGlobalResourceObject("Resource", "VehicleNo")%></label>


                        </div>
                    </div>
                </div>
                <div class="col s3 m2">
                    <div class="flex">
                        <div>
                           
                           <select ng-model="search.StatusId" ng-options="tnt.ID as tnt.Value for tnt in GateStatus" style="width: 280px;" required="">
                               <%-- <option value="">Select Status</option>--%>
                                <option value=""> <%= GetGlobalResourceObject("Resource", "SelectStatus")%></option>
                            </select>

                        </div>
                    </div>
                </div>
               
                <div class="col s3 m2">
                  <gap5></gap5>
                    <div class="">
                      
                        <div class=" ">
                                  <%-- <button type="button"  ng-click = "getGateEntryListData()" class="btn btn-primary obd">Search <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>&nbsp;&nbsp;--%>
                             <button type="button"  ng-click = "getGateEntryListData()" class="btn btn-primary obd"> <%= GetGlobalResourceObject("Resource", "Search")%> <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                           <%--  <a style="text-decoration:none;" target="_blank" href="../mInbound/gateentry.aspx"><button type="button" type="button"  class="btn btn-primary" ng-click="changemenulink()">Add New&nbsp;<i class="material-icons">add</i></button></a>--%>
                              <a style="text-decoration:none;" target="_blank" href="../mInbound/gateentry.aspx"><button type="button" type="button"  class="btn btn-primary" ng-click="changemenulink()"> <%= GetGlobalResourceObject("Resource", "Add")%><i class="material-icons">add</i></button></a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
       <%--<div>
            <div class="row">
                <div class="col m2 s3 offset-m4">
                    <div class="flex">
                        <div>
                            <input type="text" id="txtTenant"  ng-model="search.Tenant" required=""/>

                            <label>Tenant</label>


                        </div>
                    </div>
                </div>
                 <div class="col m2 s3">
                    <div class="flex">
                        <div>
                            <input type="text" id="txtBOMRefNo"  ng-model="search.BomRefNo" required=""/>
                           <label>BOM Ref. No.</label>
                        </div>
                    </div>
                </div>
                <div class="col m2 s3">
                    <div class="flex">
                        <div>
                           
                            <input type="text" id="txtJobRefNo"  ng-model="search.JOBRefNo" required=""/>
                             <label>Job Order Ref. No.</label>

                        </div>
                    </div>
                </div>
               
                <div class="col m2 s3">
                      <br />
                    <div class="">
                      
                        <div class="flex__ end">
                                   <button type="button"  ng-click = "getJOBHeaderData()" class="btn btn-primary obd">Search <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>&nbsp;&nbsp;
                            <button type="button" type="button"  class="btn btn-primary" ng-click="changemenulink()">Add New <i class="material-icons">add</i></button>
                        </div>
                    </div>
                </div>
            </div>
        </div>--%>
        
        <div ng-if="GateList!=undefined && GateList!=null && GateList.length!=0">
             <table class="table-striped">
                    <thead>
                        <tr class="">
                            <th>S. No.</th>
                               <th><%= GetGlobalResourceObject("Resource", "SNo")%></th>
                         <%--    <th>Tenant</th>--%>
                                <th><%= GetGlobalResourceObject("Resource", "Tenant")%> </th>
                           <%-- <th>Warehouse</th>--%>
                             <th><%= GetGlobalResourceObject("Resource", "Warehouse")%></th>
                            <%-- <th>Vehicle Reg. No.</th>--%>
                             <th><%= GetGlobalResourceObject("Resource", "VehicleRegNo")%></th>
                         <%--   <th>Driver</th>--%>
                               <th><%= GetGlobalResourceObject("Resource", "Driver")%></th>
                        <%--    <th>Driver Contact No.</th>--%>
                                <th><%= GetGlobalResourceObject("Resource", "DriverContactNo")%></th>
                          <%-- <th>Transporter</th>--%>
                             <th><%= GetGlobalResourceObject("Resource", "Transporter")%> </th>
                             <%--<th>Reporting For</th>--%>
                            <th><%= GetGlobalResourceObject("Resource", "ReportingFor")%> </th>
                           <%-- <th>Arriving From Country</th>--%>
                             <th><%= GetGlobalResourceObject("Resource", "ArrivingFromCountry")%> </th>
                           <%--  <th>Arriving From Sate</th>
                             <th>Arriving From City</th>--%>
                            <%-- <th>Created Date</th>--%>
                             <th><%= GetGlobalResourceObject("Resource", "CreatedDate")%> </th>
                           <%-- <th>Status</th>--%>
                             <th><%= GetGlobalResourceObject("Resource", "Status")%>  </th>
                           <%-- <th>Gate In/Out</th>--%>
                             <th><%= GetGlobalResourceObject("Resource", "GateInOut")%> </th>
                            <%--<th>Edit</th>--%>
                            <th><%= GetGlobalResourceObject("Resource", "Edit")%> </th>
                          

                        </tr>
                    </thead>

                    <tbody>
                        <tr class="" dir-paginate="gate in GateList  |itemsPerPage:25" pagination-id="main">
                            <td>{{$index+1}}</td>
                            <td align="right">{{gate.SNO}}</td>
                            <td>{{gate.Tenant}}</td>
                            <td>{{gate.WareHouse}}</td>
                            <td>{{gate.Vehicle}}</td>
                            <td>{{gate.InDriverName}}</td>
                             <td>{{gate.InDriverNo}}</td>
                             <td>{{gate.FrieghtCompany}}</td>
                              <%-- <td><div ng-if="gate.GateEntryType==1">Inbound</div><div ng-if="gate.GateEntryType==2">Outbound</div></td>--%>
                             <td><div ng-if="gate.GateEntryType==1"><%= GetGlobalResourceObject("Resource", "Inbound")%></div><div ng-if="gate.GateEntryType==2"><%= GetGlobalResourceObject("Resource", "Outbound")%></div></td>
                             <td>{{gate.CountryArrivingFrom}}</td>
                            <td>{{gate.CreatedDate}}</td>
                            <%-- <td>{{gate.ArrivingState}}</td>
                            <td>{{gate.ArrivingCity}}</td>--%>
                            <td>{{gate.Status}}</td>
                            <td><button type="button" type="button"  class="btn btn-primary" ng-if="gate.GateEntryCheck!=''" ng-click="UpdateGateDetails(gate)">{{gate.GateEntryCheck}}</button></td>
                            <td><a target="_blank" href="../mInbound/GateEntry.aspx?GID={{gate.GateEntryId}}"><i class="material-icons">launch</i></a></td>


                        </tr>
                    </tbody>
                </table>
         <div style="float:right !important;font-family:Arial;font-size:small;margin-right:1%;">
          <dir-pagination-controls direction-links="true" pagination-id="main" boundary-links="true"> </dir-pagination-controls>            
        </div> 
        </div>
       
    </div>
</asp:Content>
