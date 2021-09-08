<%@ Page Title="Add Gate Entry" Language="C#" MasterPageFile="~/mInbound/InboundMaster.master" AutoEventWireup="true" CodeBehind="GateEntry.aspx.cs" Inherits="MRLWMSC21.mInbound.GateEntry" %>
<asp:Content ID="Content1" ContentPlaceHolderID="IBContent" runat="server">
    <script src="../Scripts/angular.min.js"></script>

    <script src="../mReports/Scripts/dirPagination.js"></script>
    <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />
    <script src="GateEntry.js"></script>
      <script src="../Scripts/jquery-ui-1.8.24.js"></script>
    <link href="../CSS/bootstrap.min.css" rel="stylesheet" />
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>
    <link href="../mYardManagement/Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../mYardManagement/Scripts/toast/jquery.toastmessage.js"></script>
         <link href="../mYardManagement/Scripts/Datepicker/jquery.datetimepicker.css" rel="stylesheet" />
    <script src="../mYardManagement/Scripts/Datepicker/jquery.datetimepicker.full.js"></script>
    <link href="../Content/app.css" rel="stylesheet" />
        <script src="../Scripts/CommonWMS.js"></script>
     <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />
     <link href="../mInventory/Scripts/bootstrap.min.css" rel="stylesheet" />
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>
       


     <script type="text/javascript">
         $(document).ready(function () {
             $('#ESTDockInTime').datetimepicker({
                 formatTime: 'H:i',
                 formatDate: 'd.m.y',
                 //defaultDate:'8.12.1986', // it's my birthday
                 defaultDate: '+03-Jan-1970', // it's my birthday
                 defaultTime: '1:00',
                 step: 5,
                 timepickerScrollbar: false
             });
             $('#txtEstIBOperationTime').datetimepicker({
                 formatTime: 'H:i',
                 formatDate: 'd.m.y',
                 //defaultDate:'8.12.1986', // it's my birthday
                 defaultDate: '+03-Jan-1970', // it's my birthday
                 defaultTime: '1:00',
                 step: 5,
                 timepickerScrollbar: false
             });
             $('#txtGateOutTime').datetimepicker({
                 formatTime: 'H:i',
                 formatDate: 'd.m.y',
                 //defaultDate:'8.12.1986', // it's my birthday
                 defaultDate: '+03-Jan-1970', // it's my birthday
                 defaultTime: '1:00',
                 step: 5,
                 timepickerScrollbar: false
             });
           
         });

         </script>

    <style>
        .row {
            margin:0px !important;
        }
        .flex select {
            font-size: 14px;
            padding: 10px 0px 10px 0px !important;
            display: block;
            width: calc( 92% + 10px ) !important;
        }
    </style>

    <div class="dashed"></div>
    <div class="pagewidth">
        <div class="angulardiv" ng-app="MyApp" ng-controller="GateEntry">
             <div class="flex__ end" style="position: relative; z-index:998;     right: 10px;">
                <div class="flex ">
                   <%-- <button type="button" type="button"  class="btn btn-primary" ng-click="changemenulink()"><i class="material-icons vl">arrow_back</i>&nbsp;Back to List</button>--%>
                    <a style="text-decoration:none;"  href="../mInbound/gateentrylist.aspx"><button type="button" type="button"  class="btn btn-primary" ng-click="changemenulink()"><i class="material-icons vl">arrow_back</i>&nbsp;Back to List</button></a>

                </div>
            </div>
            <div class="ui-SubHeading ui-SubHeadingBar" id="PrimaryInformationHeader">Account & Vehicle Info <span class="ui-icon"></span></div>
        <div class="ui-Customaccordion" id="PrimaryInformationBody">
            <br />
             
                    <div class="FormLabels flex__ end" style="font-size:18pt;" >

                        <label ID="lblInboundStatus" ng-bind="GateEntryObj.Status"></label>
                        &nbsp; &nbsp; &nbsp; &nbsp;
                    </div>
            <div class="row">

                <div class="col m3">
                    <div class="flex">
                        <div>
                            <%--<span style="color:red">*</span>                        --%>
                            <select ng-model="GateEntryObj.AccountId" ng-options="tnt.ID as tnt.Value for tnt in AccountData" style="width: 280px;" >
                                <option value="">Select Account</option>
                            </select>
                            <span class="errorMsg"></span>
                        </div>
                    </div>
                  <%--  <div class="flex">
                       
                        <select ng-model="GateEntryObj.AccountId" ng-options="tnt.ID as tnt.Value for tnt in AccountData" style="width: 280px;" required="">
                                <option value=""></option>
                            </select>
                             <label>Account</label>
                            <span class="errorMsg"></span>
                        <label>Account</label>
                    </div>--%>
                </div>
                <div class="col m3">
                    <div class="flex">
                        <input type="text" id="txtTenant"  ng-model="GateEntryObj.Tenant" required="" />
                         <span class="errorMsg"></span>
                        <label>Tenant</label>
                    </div>
                </div>
                 <div class="col m3">
                    <div class="flex">
                         <select ng-model="GateEntryObj.WareHouseId" ng-options="wh.Id as wh.Name for wh in WareHouseData" style="width: 280px;" required="">
                                <option value="">Select Warehouse</option>
                            </select>
                        <span class="errorMsg"></span>
                            

                            
                        
                    
                    </div>
                </div>
                 <div class="col m3">
                    <div class="flex">
                        <input type="text" id="txtFreightCmpny"  required=""  />
                        <label>Transporter Company Name</label>
                    </div>
                </div>
                <%--<div class="col m3">
                    <div class="flex">
                        <input type="text" id="txtvehicle" ng-model="GateEntryObj.Vehicle" required="" ng-change="getwarehouseData()" />
                        <label>Vehicle</label>
                    </div>
                </div>--%>
               

            </div>
            <div class="row">
                <div class="col m3">
                    <div class="flex">
                       
                        <input type="text" id="txtvehicle" ng-model="GateEntryObj.Vehicle" required="" ng-change="getwarehouseData()" />
                        <span class="errorMsg"></span>
                        <label>Vehicle</label>
                    </div>
                </div>
                <div class="col m3">
                    <div class="flex">
                        <input type="text" id="txtPermitInfo"  ng-model="GateEntryObj.PermitInfo" required="" readonly="true" />
                       <%-- <label>Permit Info</label>--%>
                         <label>Storage Volume</label>
                    </div>
                </div>
                 <div class="col m3">
                    <div class="flex">
                      
                        <input type="text" id="txtCapacityInfo"  ng-model="GateEntryObj.CapacityInfo" required="" readonly="true"/>
                         <label>Max Storage Weight</label>
                       <%-- <label>Capacity Info</label>--%>
                        
                    
                    </div>
                </div>
                  <div class="col m3">
                    <div class="flex">
                        <input type="text" id="txtdriver" ng-model="GateEntryObj.InDriverName" required="" />
                         <span class="errorMsg"></span>
                        <label>Driver</label>
                    </div>
                </div>
               
            </div>
             <div class="row">
                 <div class="col m3">
                    <div class="flex">
                        <input type="text" id="txtdriverno" onkeypress="return isNumber(event)" maxlength="10" ng-model="GateEntryObj.InDriverNo" required="" />
                         <span class="errorMsg"></span>
                        <label> Driver Contact No.</label>
                    </div>
                </div>
                 </div>

                 <br />
            <div class="flex__ end">
                <div class="flex ">
                  
                   
                    
                        <button type="button" type="button" id="SaveInfo" ng-if="GateEntryObj.StatusId<1" class="btn btn-primary" ng-click="SaveGateEntry(1)">Save</button>
                   
                   
                    

                </div>
            </div>
            </div>

        <div ng-show="ShimentInfo">
            <div class="ui-SubHeading ui-SubHeadingBar" id="PrimaryShipmentHeader">Shipment Info <span class="ui-icon"></span></div>
        <div class="ui-Customaccordion" id="PrimaryShipmentBody">
            <br />
            <div class="row">

                <div class="col m3">
                    <div class="flex">

                        <select ng-model="GateEntryObj.GateEntryType" ng-change="clearShipmentInfo()" ng-options="sl.Id as sl.Name for sl in Types" style="width: 280px;" required="">
                            <option value="">Reporting For</option>
                        </select>
                       

                        <span class="errorMsg"></span>

                    </div>
                </div>
                <%-- <div class="col m3">
                    <div class="flex">
                        <input type="text" id="txtInboundId"  ng-model="GateEntryObj.InboundNo" required="" />
                        <label>Inbound Ref#</label>
                    </div>
                </div>--%>
                 <div class="col m3">
                    <div class="flex">
                        <input type="text" id="txtContainerSeal"  ng-model="GateEntryObj.ContainerSeal" required="" />
                        <label>Container Seal#</label>
                    </div>
                </div>
                 <div class="col m3" >
                    <div class="flex" ng-hide="GateEntryObj.GateEntryType==2">
                        <input type="text" id="txtArrivingFrom"  ng-model="GateEntryObj.ArrivingFrom" required="" />

                        <label>Arriving From Country</label>
                    </div>
                </div>
                 <div class="col m3" >
                    <div class="flex" ng-hide="GateEntryObj.GateEntryType==2">
                        <input type="text" id="txtArrivingSate"   required="" />
                        <label>Arriving From State</label>
                    </div>
                </div>
                <%--<div class="col m3">
                    <div class="flex">
                        <input type="text" id="txtLRNO" ng-model="GateEntryObj.LRNO" required="" />
                        <label>LR#</label>
                    </div>
                </div>--%>
            </div>

            <div class="row">
               
                  <div class="col m3" >
                    <div class="flex" ng-hide="GateEntryObj.GateEntryType==2">
                        <input type="text" id="txtArrivingCity"   required="" />
                        <label>Arriving From City</label>
                    </div>
                </div>
              
               <%-- <div class="col m3">
                    <div class="flex">

                        <input type="text" id="txtBOXQty"  ng-model="GateEntryObj.BOXQty" required="" />
                        <label>Box Qty.</label>

                        <span class="errorMsg"></span>

                    </div>
                </div>--%>
               <%--  <div class="col m3">
                    <div class="flex">
                        <input type="text" id="txtSKUQty"  ng-model="GateEntryObj.SKUQty" required="" />
                        <label>SKU Qty.</label>
                    </div>
                </div>--%>
                
                 <div class="col m3">
                    <%--<div class="flex">--%>
                     <br />
                     <div class="checkbox">
                        <input type="checkbox" id="txtIsRetutnLoad" ng-if="GateEntryObj.GateEntryType==1"  ng-model="GateEntryObj.IsRetutnLoad" required="" />
                        <label for="txtIsRetutnLoad" ng-if="GateEntryObj.GateEntryType==1">Return Load</label>
                         </div>
                    <%--</div>--%>
                </div>
                  <div class="col m6">
                       <br />
                 <div class="pull-right" >
                      <button type="button" type="button" id="SaveInfo" class="btn btn-primary" ng-click="SaveGateEntry(2)">Save</button>
                    
                     
                </div>
                       </div>
                <br />
                 </div>
            <br />
             <div class="flex__ end">
                <div class="flex " ng-if="GateEntryObj.GateEntryType>=2 || GateEntryObj.IsRetutnLoad==true">
                  
                    <button type="button" id="lnkAddPreferdLocation" class="btn btn-primary " ng-click="cleardataWhenOpenPreLocation(1)" data-toggle="modal">Add preferred Location <i class="fa fa-plus" aria-hidden="true"></i></button>
                    
                       <%-- <button type="button" type="button" id="SaveInfo" class="btn btn-primary" ng-click="SaveGateEntry(1)">Save</button>--%>
                   
                   
                    

                </div>
            </div>
                <div ng-show="DockInfo">
                    
                     <table align="center" class="mytableOutbound" cellpadding="0" cellspacing="0">
                    <thead>
                        <tr class="mytableOutboundHeaderTR">
                            <th>S. No.</th>
                            <th style="text-align: center;">preferred Country</th>
                            <th style="text-align: center;">preferred State </th>
                            <th style="text-align: center;">preferred city</th>
                            <th>Edit</th>
                            <th>Delete</th>
                          

                        </tr>
                    </thead>

                    <tbody>
                        <tr class="mytableOutboundBodyTR"  dir-paginate="bom in PreferedLocationdata  |itemsPerPage:25" pagination-id="main">
                            <td align="right">{{$index+1}}</td>
                            <td>{{bom.Country}}</td>
                            <td>{{bom.State}}</td>
                            <td>{{bom.City}}</td>
                            <%--<td><button type="button" type="button"  class="btn btn-primary" ng-click="updatePreferedLOCDetails(bom)">Update</button></td>
                            <td><button type="button" type="button"  class="btn btn-primary" ng-click="DeletePreferredLocDetails(bom)">Delete</button></td>--%>
                            <td><a  ng-click="updatePreferedLOCDetails(bom)"><i class="material-icons">mode_edit</i></a></td>
                            <td><a  ng-click="DeletePreferredLocDetails(bom)"><i class="material-icons">delete</i></a></td>
                            


                        </tr>
                    </tbody>
                </table>

                 <div style="float:right !important;font-family:Arial;font-size:small;margin-right:1%;">
          <dir-pagination-controls direction-links="true" pagination-id="main" boundary-links="true"> </dir-pagination-controls>            
        </div> 
                </div>
                  
                   
           
            <%--<div class="row">

                <div class="col m3">
                    <div class="flex">

                        <input type="text" id="txtIsPreferedLocation" ng-model="GateEntryObj.IsPreferedLocation" required="" />
                        <label>Preferred Location</label>

                        <span class="errorMsg"></span>

                    </div>
                </div>
            </div>--%>
            <br />
           <%-- <label>Material Info :</label>
            <div class="pull-right">
                    <button type="button" id="lnkAddMaterial" class="btn btn-primary " ng-click="cleardataWhenOpen()" data-toggle="modal">Add Items <i class="fa fa-plus" aria-hidden="true"></i></button>
                     
                </div>--%>
            </div>
        </div>

            <div ng-if="GateEntryObj.StatusId>1">
                 <div class="ui-SubHeading ui-SubHeadingBar" id="PrimaryMaterialHeader">Docking Info <span class="ui-icon"></span></div>
            <div class="ui-Customaccordion" id="PrimaryMaterialBody">
                <br />
                   <div class="pull-right">
                    <button type="button" id="lnkAddDock" class="btn btn-primary " ng-click="cleardataWhenOpenPreDock(1)" data-toggle="modal">Add Docks <i class="fa fa-plus" aria-hidden="true"></i></button>
                     
                </div>
                <br />
                 <div>
                    
                     <table align="center" class="mytableOutbound" cellpadding="0" cellspacing="0">
                    <thead>
                        <tr class="mytableOutboundHeaderTR">
                            <th>S. No.</th>
                            <th style="text-align: center;">Shipment No.</th>
                            <th style="text-align: center;">Dock </th>
                            <th style="text-align: center;">LR#</th>
                            <th style="text-align: center;">Est. Dock In Time</th>
                           <%-- <th style="text-align: center;">Est. Opr. Time(min)</th>--%>
                            <th style="text-align: center;">Est Dock Out Time</th>
                           <%-- <th style="text-align: center;">SKU Qty.</th>
                            <th style="text-align: center;">BOX Qty.</th>--%>
                             <th style="text-align: center;">Act. Dock In Time</th>
                           <%-- <th style="text-align: center;">Act. Opr. Time(min)</th>--%>
                            <th style="text-align: center;">Act. Dock Out Time</th>
                            <th>Edit</th>
                            <th>Delete</th>
                          

                        </tr>
                    </thead>

                    <tbody>
                        <tr class="mytableOutboundBodyTR"  dir-paginate="bom in DockData  |itemsPerPage:25" pagination-id="Dockmain">
                            <td align="right">{{$index+1}}</td>
                            <td>{{bom.ShipmentNo}}</td>
                            <td>{{bom.Dock}}</td>
                            <td>{{bom.LR}}</td>
                             <td>{{bom.ESTDockInTime}}</td>
                            <%-- <td>{{bom.ESTOperationTime}}</td>--%>
                             <td>{{bom.ESTDockoutTime}}</td>
                            <%-- <td>{{bom.SKUQty}}</td>
                             <td>{{bom.BOXQty}}</td>--%>
                              <td>{{bom.Actualdockintime}}</td>
                            <%-- <td>{{bom.Actualoperationtime}}</td>--%>
                             <td>{{bom.Actualdockouttime}}</td>
                            <td><a  ng-click="updateDock(bom)"><i class="material-icons">mode_edit</i></a></td>
                            <td><a  ng-click="DeleteDock(bom)"><i class="material-icons">delete</i></a></td>
                            


                        </tr>
                    </tbody>
                </table>

                 <div style="float:right !important;font-family:Arial;font-size:small;margin-right:1%;">
          <dir-pagination-controls direction-links="true" pagination-id="Dockmain" boundary-links="true"> </dir-pagination-controls>            
        </div> 
                </div>
                
            </div>
            </div>

            <div ng-if="GateEntryObj.StatusId>=4">
                     <div class="ui-SubHeading ui-SubHeadingBar" id="PrimaryUnloadingInfoHeader">Unloading/Loading Info <span class="ui-icon"></span></div>
            <div class="ui-Customaccordion" id="PrimaryUnloadingInfoBody">
                <br />
                     <div class="row">
                         <div class="col m2"><br /><div class="checkbox">
                             <input type="checkbox" id="txtIsUnload"  ng-model="IsUnload" required="" />
                             <label for="txtIsUnload" ng-if="GateEntryObj.GateEntryType==1">Is vehicle Unloaded</label>
                             <label for="txtIsUnload" ng-if="GateEntryObj.GateEntryType==2">Is vehicle Loaded</label>

                                                   </div></div>
                <div class="col m3">
                    <div class="flex" ng-if="IsUnload==true">
                         
                        <input type="text" id="txtUnloadedQty"   ng-model="GateEntryObj.UnloadedQty" required="" />
                        <label ng-if="GateEntryObj.GateEntryType==1">Quantity Unloaded</label>
                         <label ng-if="GateEntryObj.GateEntryType==2">Quantity Loaded</label>

                        <span class="errorMsg"></span>

                    </div>
                </div>
                         <div class="col m3" ng-if="IsUnload==true && GateEntryObj.StatusId<5">
                             <br />
                            <button type="button" type="button"  class="btn btn-primary" ng-click="SaveUnloadVehicleData()">Save</button>
                         </div>
                        
                  </div>
                </div>
            </div>
           
            <div ng-if="GateEntryObj.StatusId>=6">
                     <div class="ui-SubHeading ui-SubHeadingBar" id="PrimaryDocumentsHeader">Documents Info <span class="ui-icon"></span></div>
            <div class="ui-Customaccordion" id="PrimaryDocumentsBody">
                <br />
                 <div class="pull-right">
                    <button type="button" id="lnkAddMaterial" class="btn btn-primary " ng-click="cleardataWhenOpen()" data-toggle="modal">Add Documents <i class="fa fa-plus" aria-hidden="true"></i></button>
                     
                </div>
                <br />

                <div>
                   
                     <table align="center" class="mytableOutbound" cellpadding="0" cellspacing="0">
                    <thead>
                        <tr class="mytableOutboundHeaderTR">
                            <th>S. No.</th>
                            <th style="text-align: center;">Document Type</th>
                            <th style="text-align: center;">Document Name </th>
                            <th style="text-align: center;">File</th>
                           
                            
                            <th>Delete</th>
                          

                        </tr>
                    </thead>
                        
                    <tbody>
                        <tr class="mytableOutboundBodyTR"  dir-paginate="bom in Documnetdata  |itemsPerPage:25" pagination-id="Dockmain">
                            <td align="right">{{$index+1}}</td>
                            <td>{{bom.Document_Type}}</td>
                            <td>{{bom.DocumentName}}</td>
                            <td><a href="" ng-click="getSampleTemplate(bom.DocumentURI)">{{bom.DocumentName}}</a></td>
                            
                            <%--<td><button type="button" type="button"  class="btn btn-primary" ng-click="DeleteDocument(bom)">Delete</button></td>--%>
                             <td><a  ng-click="DeleteDocument(bom)"><i class="material-icons">delete</i></a></td>

                            


                        </tr>
                    </tbody>
                </table>

                 <div style="float:right !important;font-family:Arial;font-size:small;margin-right:1%;">
          <dir-pagination-controls direction-links="true" pagination-id="Dockmain" boundary-links="true"> </dir-pagination-controls>            
        </div> 
                </div>
            </div>
            </div>
            <div ng-show="showgateoutinfo">
                      <div class="ui-SubHeading ui-SubHeadingBar" id="PrimaryGateoutHeader">Gate Out Info <span class="ui-icon"></span></div>
            <div class="ui-Customaccordion" id="PrimaryGateoutBody">
                <br />
                <div class="row">

                    <div class="col m3">
                        <div class="flex">
                          <input type="text" id="txtGateOutTime" ng-model="GateEntryObj.OutdateTime" name="name" value="" required="" />
                            <%--<input type="text" id="txtOutdatetime" ng-model="GateEntryObj.OutdateTime" required="" />--%>
                            <label>Out DateTime</label>

                            <span class="errorMsg"></span>

                        </div>
                    </div>
                     
                </div>
                 
            <br />
            <div class="flex__ end">
                <div class="flex ">
                  
                    
                    
                        <button type="button" type="button" id="lnkSavePrimaryInfo" ng-if="GateEntryObj.StatusId<=7" class="btn btn-primary" ng-click="SaveGateOutTime()">Save</button>
                   
                   
                    

                </div>
            </div>
            </div>
            </div>
           
           

            <div id="SupModal" class="modal fade">
                    <div class="modal-dialog" role="document" style=" width: 710px !important;">
                        <div class="modal-content">
                            <div class="modal-header" style="background-color: var(--sideNav-bg) !important; color: #fff !important;">
                                <h4 class="modal-title" style="display: inline !important;">Add Documents</h4>
                                <button type="button" data-dismiss="modal" class="pull-right modalclose" onclick="myKitclear();" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <div class="modal-body" id="mySupForm">

                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="flex">
                                            <div>
                                               <%-- <asp:TextBox ID="txtlinepartno" runat="server" required="" MaxLength="30" />--%>
                                                <select ng-model="ShipmentDocuments.DocumentTypeID" ng-options="sl.Id as sl.Name for sl in DocumentTypes" style="width: 280px;" required="">
                                                    <option value="">Document Type</option>
                                                </select>
                                                

                                                <span class="errorMsg"></span>


                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="flex">
                                            <div>
                                               <select ng-model="ShipmentDocuments.MIMEID" ng-options="sl.Id as sl.Name for sl in MimeTypes" style="width: 280px;" required="">
                                                    <option value="">MIME Type</option>
                                                </select>
                                               

                                                <span class="errorMsg"></span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="flex">
                                            <div>
                                              
                                                <input type="text" id="txtRTUoM" ng-model="ShipmentDocuments.DocumentName" required=""/>
                                                <span class="errorMsg">*</span>                                               
                                                <label>Document Name </label>
                                            </div>
                                        </div>
                                    </div>
                                    
                                </div>
                                 <div class="row">
                                    <div class="col-md-6">
                                        <div class="flex">
                                            <div>
                                                <input type="file" id="Mimefile"/>
                                               <%-- <button type="button" ng-click="uploadFile()">Upload File</button>--%>

                                            </div>
                                        </div>
                                    </div>
                                     </div>
                                

                            </div>
                            <div class="modal-footer">
                                <input type="hidden" id="MMT_SUPPLIER_ID" />
                                <%--<asp:CheckBox runat="server" ID="chkBDDelete" Text="Delete" onclick="CheckIsDelted(this);" />--%>
                                <button type="button" class="btn btn-secondary" style="color: #fff !important;" ng-click="cleardata();">Clear</button>
                                <button type="button" class="btn btn-secondary" style="color: #fff !important;" data-dismiss="modal">Close</button>
                                <button type="button" type="button" ID="lnkSaveSecondaryInfo" class="btn btn-primary" ng-click="uploadFile()">Save</button>
                                     <%--   <asp:LinkButton runat="server" ID="lnkButUpdate" ValidationGroup="updateBOMItems"  CssClass="ui-btn ui-button-large">
                                                                Update <%=MRLWMSC21Common.CommonLogic.btnfaUpdate %>
                                        </asp:LinkButton>--%>
                                    

                            </div>
                        </div>
                    </div>
                </div>


<%--            -------------------------- Adding Docks---------------------------------%>

            <div id="DockModal" class="modal fade">
                    <div class="modal-dialog" role="document" style=" width: 710px !important;">
                        <div class="modal-content">
                            <div class="modal-header" style="background-color: var(--sideNav-bg) !important; color: #fff !important;">
                                <h4 class="modal-title" style="display: inline !important;">Add Docks</h4>
                                <button type="button" data-dismiss="modal" class="pull-right modalclose" onclick="myKitclear();" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <div class="modal-body" id="mySupForm">

                                <div class="row">
                                    <div class="col m4">
                                        <div class="flex">
                                            <input type="text" id="txtInboundId"  required="" />
                                            <label>Shipment Ref#</label>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="flex">
                                            <div>
                                                <input type="text" id="txtDock"  required="" />
                                                <label>Dock#</label>

                                                <span class="errorMsg"></span>


                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="flex">
                                            <div>                                              
                                                <input type="text" id="txtLR" ng-model="DocksData.LR" required=""/>
                                                <span class="errorMsg">*</span>                                               
                                                <label>LR# </label>
                                            </div>
                                        </div>
                                    </div>
                                     
                                    
                                </div>
                                
                                <div class="row">
                                     <div class="col-md-4">
                                        <div class="flex">
                                            <div> 
                                                  <input type="text" id="txtEstIBOperationTime" ng-model="DocksData.ESTDockInTime" name="name" value="" required="" />
                                               <%-- <input type="text" id="txtESTDockInTime" ng-model="DocksData.ESTDockInTime" required=""/>--%>
                                                <span class="errorMsg">*</span>                                               
                                                <label>Est. Dock In Time </label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="flex">
                                            <div>
                                                <input type="text" id="txtESTOperationTime" onkeypress="return isNumber(event)" ng-model="DocksData.ESTOperationTime"  required="" />
                                                <label>Est. Opr. Time (min.)</label>

                                                <span class="errorMsg"></span>


                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="flex">
                                            <div>                                              
                                                <input type="text" id="txtESTDockoutTime" ng-model="DocksData.ESTDockoutTime" readonly="true" required=""/>
                                               <%-- <span class="errorMsg">*</span> --%>                                              
                                                <label>Est. Dock Out Time </label>
                                            </div>
                                        </div>
                                    </div>
                                     
                                    
                                </div>


                                <div class="row">
                                     <div class="col-md-4">
                                        <div class="flex">
                                            <div>                                               
                                                <input type="text" id="txtSKUQty" onkeypress="return isNumber(event)" ng-model="DocksData.SKUQty" required=""/>
                                                <span class="errorMsg">*</span>                                               
                                                <label>SKU Qty.</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="flex">
                                            <div>
                                                <input type="text" id="txtBOXQty" onkeypress="return isNumber(event)" ng-model="DocksData.BOXQty"  required="" />
                                                 <span class="errorMsg"></span>
                                                <label>Box Qty.</label>

                                               


                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="flex">
                                            <div>                                              
                                                <input type="text" id="txtActualdockintime" ng-model="DocksData.Actualdockintime"  readonly="true" required=""/>
                                              <%--  <span class="errorMsg">*</span>            --%>                                   
                                                <label>Act. Dock In Time </label>
                                            </div>
                                        </div>
                                    </div>
                                      
                                    
                                </div>

                                 <div class="row">
                                     <div class="col-md-4">
                                        <div class="flex">
                                            <div>                                               
                                                <input type="text" id="txtActualoperationtime" ng-model="DocksData.Actualoperationtime"  readonly="true" required=""/>
                                               <%-- <span class="errorMsg">*</span>      --%>                                         
                                                <label>Act. Opr. Time (min)</label>
                                            </div>
                                        </div>
                                    </div>
                                      <div class="col-md-4">
                                        <div class="flex">
                                            <div>                                               
                                                <input type="text" id="ActualDockOuttime" ng-model="DocksData.Actualdockouttime"  readonly="true" required=""/>
                                              <%--  <span class="errorMsg">*</span> --%>                                              
                                                <label>Act. Dock. Out Time</label>
                                            </div>
                                        </div>
                                    </div>
                                   
                                     </div>




                            </div>
                            <div class="modal-footer">
                                <input type="hidden" id="MMT_SUPPLIER_ID" />
                                <%--<asp:CheckBox runat="server" ID="chkBDDelete" Text="Delete" onclick="CheckIsDelted(this);" />--%>
                                <button type="button" class="btn btn-secondary" style="color: #fff !important;" ng-click="cleardataWhenOpenPreDock(0);">Clear</button>
                                <button type="button" class="btn btn-secondary" style="color: #fff !important;" data-dismiss="modal">Close</button>
                                <button type="button" type="button" ID="lnkSaveSecondaryInfo" class="btn btn-primary" ng-click="saveDockDetails()">Save</button>
                                     <%--   <asp:LinkButton runat="server" ID="lnkButUpdate" ValidationGroup="updateBOMItems"  CssClass="ui-btn ui-button-large">
                                                                Update <%=MRLWMSC21Common.CommonLogic.btnfaUpdate %>
                                        </asp:LinkButton>--%>
                                    

                            </div>
                        </div>
                    </div>
                </div>


            <div id="PreferdLocation" class="modal fade">
                    <div class="modal-dialog" role="document" style=" width: 710px !important;">
                        <div class="modal-content">
                            <div class="modal-header" style="background-color: var(--sideNav-bg) !important; color: #fff !important;">
                                <h4 class="modal-title" style="display: inline !important;">Add Preferred Location</h4>
                                <button type="button" data-dismiss="modal" class="pull-right modalclose" onclick="cleardataWhenOpenPreLocation(0);" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <div class="modal-body" id="mySupForm">

                                <div class="row">

                                    <div class="col m4">
                                        <div class="flex">
                                            <input type="text" id="txtPreCountry"  required="" />
                                            <span class="errorMsg">*</span>         
                                            <label>Prefered Country</label>
                                        </div>
                                    </div>
                                    <div class="col m4">
                                        <div class="flex">
                                            <input type="text" id="txtPreSate" required="" />
                                            <label>Preferred State</label>
                                        </div>
                                    </div>
                                    <div class="col m4">
                                        <div class="flex">
                                            <input type="text" id="txtPreCity" required="" />
                                            <label>Preferred City</label>
                                        </div>
                                    </div>

                                </div>                         










                            </div>
                            <div class="modal-footer">
                                <input type="hidden" id="MMT_SUPPLIER_ID" />
                                <%--<asp:CheckBox runat="server" ID="chkBDDelete" Text="Delete" onclick="CheckIsDelted(this);" />--%>
                                <button type="button" class="btn btn-secondary" style="color: #fff !important;" ng-click="cleardataWhenOpenPreLocation(0);">Clear</button>
                                <button type="button" class="btn btn-secondary" style="color: #fff !important;" data-dismiss="modal">Close</button>
                                <button type="button" type="button" ID="lnkSaveSecondaryInfo" class="btn btn-primary" ng-click="savePreferedLocation()">Save</button>
                                     <%--   <asp:LinkButton runat="server" ID="lnkButUpdate" ValidationGroup="updateBOMItems"  CssClass="ui-btn ui-button-large">
                                                                Update <%=MRLWMSC21Common.CommonLogic.btnfaUpdate %>
                                        </asp:LinkButton>--%>
                                    

                            </div>
                        </div>
                    </div>
                </div>
        </div>
    
    </div>   <br />
        <br />
        <br />   <br />
        <br />
        <br />

</asp:Content>
