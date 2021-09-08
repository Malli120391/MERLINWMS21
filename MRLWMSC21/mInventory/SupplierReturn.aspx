<%@ Page Title="Supplier Returns" Language="C#" MasterPageFile="~/mInventory/InventoryMaster.master" AutoEventWireup="true" CodeBehind="SupplierReturn.aspx.cs" Inherits="MRLWMSC21.mInventory.SupplierReturn" %>
<asp:Content ID="Content1" ContentPlaceHolderID="InvContent" runat="server">
     <script src="Scripts/angular.min.js"></script>
    <script src="SupplierReturn.js"></script>
     <script src="../mReports/Scripts/dirPagination.js"></script>
    <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />
     <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>

     <script type="text/javascript">
        function isNumber(evt) {
            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
                return false;
            return true;
        }
    </script>
     

    <div ng-app="MyApp" ng-controller="Sutomerretun" class="container">
        <div ng-show="blockUI">
  <div ng-show="blockUI">
            <div style="width: 100%; height: 100vh; z-index: 999; position: fixed; top: 0; left: 0; right: 0; bottom: 0; align-items: center; display: flex; justify-content: center; background: rgba(255, 255, 255, 0.24); background: hsla(0, 0%, 100%, 0.72);">

                <div style="align-self: center;">
                   
                    <img width="60" src="../Images/Preloader.svg"/>
                </div>

            </div>

        </div>
  
</div>

        <div>
            <table align="right">
                 <!-- Globalization Tag is added for multilingual  -->
               
                <tr>
                    <td align="right" style="padding: 10px;">

                            <div class="" align="right">
                                <div class="row">
                                    <div class="col m3 s3">
                                        <div class="flex">
                                            <input  type="text"  ID="atcStore"  class="ui-autocomplete-input" SkinID="txt_Auto"  required="" />
                                            <span class="errorMsg"></span>
                                            <label><%= GetGlobalResourceObject("Resource", "Store")%></label>
                                        </div>
                                    </div>
                                    <div class="col m3 s3">
                                        <div class="flex">
                                            <input ID="txtTenant" type="text" SkinID="txt_Hidden_Req"  class="ui-autocomplete-input" required=""/>
                                            <label><%= GetGlobalResourceObject("Resource", "Tenant")%></label>
                                            <span class="errorMsg"></span>
                                            <asp:HiddenField runat="server" ID="hifTenant" />
                                        </div>
                                    </div>
                                    <div class="col m3 s3">
                                        <div class="flex">
                                            <input  type="text" ID="atcPONumber"  class="ui-autocomplete-input" SkinID="txt_Auto"  required="" />
                                            <label> <%= GetGlobalResourceObject("Resource", "PONumber")%></label>
                                            <span class="errorMsg"></span>
                                        </div>
                                    </div>
                                    <div class="col m3 s3">
                                        <div class="flex">
                                            <input  type="text"  ID="atcSupplierNumber"  class="ui-autocomplete-input" SkinID="txt_Auto"  required="" />
                                            <label><%= GetGlobalResourceObject("Resource", "Invoice")%></label>
                                            <span class="errorMsg"></span>
                                        </div>
                                    </div>
                                    
                                    <div class="col m12 s12">
                                        <gap5></gap5>
                                        <button id="btnimport" type="button" ng-click="GetDetails()" class="btn btn-primary right"><%= GetGlobalResourceObject("Resource", "GetDetails")%><i class="fa fa-folder-open" aria-hidden="true"></i></button>
                                    </div>
                                </div>
                            </div>
                    </td>
                </tr>
            </table>

            <table class="table-striped" style="">
                <thead>
                     <tr>
                         <th><%= GetGlobalResourceObject("Resource", "Select")%></th>
                         <th> <%= GetGlobalResourceObject("Resource", "LineNo")%></th>
                         <th><%= GetGlobalResourceObject("Resource", "Part")%></th>
                         <th><%= GetGlobalResourceObject("Resource", "BatchNo")%></th>
                         <th><%= GetGlobalResourceObject("Resource", "MfgDate")%></th>
                         <th><%= GetGlobalResourceObject("Resource", "ExpDate")%></th>
                         <th><%= GetGlobalResourceObject("Resource", "SerialNo")%></th>
                         <th><%= GetGlobalResourceObject("Resource", "ProjectRefNo")%> </th>
                          <th><%= GetGlobalResourceObject("Resource", "MRP")%> </th>                 <%--MRP is added by lalitha in 06/03/2019--%>
                         <th><%= GetGlobalResourceObject("Resource", "Location")%> </th>
                         <th>CartonCode </th>
                         <th>StorageLocation </th>
                         <th number><%= GetGlobalResourceObject("Resource", "AvailableQty")%></th>
                         <%--<th number><%= GetGlobalResourceObject("Resource", "PendingReturnedQty")%></th>--%>
                         <th> <%= GetGlobalResourceObject("Resource", "ReturnedQty")%></th>
                        
                     </tr>
                 </thead>
                 <tbody>
                     <tr ng-repeat="Sup in Supplierreturns">
                         <td align="center">  <input type="checkbox" ng-model="Sup.Isselected"   id="ng-change-example1" ng-change="calcualtedimension()"/></td>
                         <td>{{ Sup.Line }}</td>
                         <td>{{ Sup.MCode }}</td>
                         <td>{{ Sup.BatchNo }}</td>
                         <td >{{ Sup.MfgDate }}</td>
                         <td >{{ Sup.ExpDate }}</td>
                         <td>{{ Sup.SerialNo }}</td>
                         <td>{{ Sup.ProjectRefNo }}</td>
                         <td>{{ Sup.MRP }}</td>       <%--added by lalitha on 06/03/2019--%>
                         <td>{{ Sup.Location }}</td>
                         <td>{{ Sup.CartonCode }}</td>
                          <td>{{ Sup.StorageLocation }}</td>
                         <td number>{{ Sup.PickedQty }}</td>
                          <%--<td number>{{ Sup.PendingReturnQty }}</td>--%>
                         <td>
                            <%-- <label ng-show="Sup.IsKitParent==0">{{Sup.ReturnQty}}</label>--%>
                             <input type="text"  ng-model="Sup.ReturnQty" onkeypress="return isNumber(event)" ng-Keyup="checkreturnqty(Sup)" style="text-align:right; border:0px; border-bottom:1px solid; border-color: var(--paper-grey-300) !important;" /></td>
                         
                     </tr>
                 </tbody>
          </table>
            <div class="modal-body setwidth">
                    <div id="divValidationCycleCountMessages" class="text-danger" style="color:red !important;"></div>
                    <p></p>
                    <div class="row" ng-if="hide">
                        <div class="col m3 s3">
                            <div class="flex">
                                <div>
                                    <%--<span style="color:red">*</span>--%>
                                    <select ng-model="DockId" ng-options="Doc.Id as Doc.Name for Doc in Docks" id="dockid" ng-change="LoadDock()">
                                        <option value="">Select Dock</option>
                                    </select>
                                    <span class="errorMsg"></span>
                                </div>
                            </div>
                        </div>
                        <div class="col m3 s3">
                            <div class="flex">
                                <div>

                                    <%--<span style="color:red">*</span>--%>
                                    <select ng-model="VehicleTypeId" ng-options="VT.Id as VT.Name for VT in VehicleTypes" id="vehicletypeid" >
                                        <option value="">Select Vehicle Type</option>
                                    </select>
                                    <span class="errorMsg"></span>
                                </div>
                            </div>
                        </div>
                        <div class="col m3 s3">
                            <div class="flex">
                                <div>
                                    <%--<span style="color:red">*</span>--%>
                                    <input type="text" placeholder="Vehicle Number" ng-model="Vehicleno" maxlength="10" id="txtVehicleno" /><span class="errorMsg"></span>
                                </div>
                            </div>

                        </div>
                        <div class="col m3 s3">
                            <div class="flex">
                                <div>
                                    <%--<span style="color:red">*</span>    --%>
                                    <input type="text" placeholder="Driver Name" ng-model="DriverName" maxlength="50" id="txtDriver" /><span class="errorMsg"></span>
                                </div>
                            </div>
                        </div>
                         <div class="col m3 s3">
                             <div class="flex">
                                                    <div>                              
                                          <%--<span style="color:red">*</span>--%>
                                           <input type="text" placeholder=" Mobile Number" onkeypress="return isNumber(event)" maxlength="10"   ng-model="Mobile"  id="txtMobile" ng-pattern="onlyNumbers" maxlength="10"  ng-minlength="10"   oninput="validity.valid||(value='');" input-restrictor//>
                                                        <span class="errorMsg"></span>
                                                    </div>
                                                    </div>
                        </div>
                         <div class="col m3 s3">
                             <br />
                             <button  id="btntransfer" type="button" ng-click="Transfer()" ng-if="hide"  class="btn btn-primary right"> Return <i class="material-icons vl">keyboard_return</i></button> 
                         
                         </div>
                    </div>
                       
                  
                     
                </div>
        </div>
    </div>
</asp:Content>
