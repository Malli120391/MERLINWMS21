<%@ Page Title="Transfer Request" Language="C#" MasterPageFile="~/mInventory/InventoryMaster.master" AutoEventWireup="true" CodeBehind="InternalTransferRequest.aspx.cs" Inherits="MRLWMSC21.mInventory.InternalTransferRequest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="InvContent" runat="server">
    <script src="../Scripts/angular.min.js"></script>
    <script src="InternalTransferRequest.js"></script>
    <script src="../mReports/Scripts/dirPagination.js"></script>
    <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
    <script src="../Scripts/xlsx-model.js"></script>
    <%--    <script src="../Scripts/xlsx.full.min.js"></script>--%>
    <script src="../Scripts/xlsx.full.min.js"></script>


        <script type="text/javascript">
        function isNumber(evt) {
            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
                return false;
            return true;
        }

    </script>
    <style >

        .table-striped tr td {
        text-align : center !important;
        
        }
    </style>
    <div class="module_yellow">
        <div class="ModuleHeader" height="35px">
            <div><a href="../Default.aspx">Home</a> <i class="material-icons">arrow_right</i> <span>Orders</span> <i class="material-icons">arrow_right</i><span>House Keeping</span> <i class="material-icons">arrow_right</i> <span class="breadcrumbd" contenteditable="false">Transfer Request</span></div>

        </div>

    </div>
    <div class="container">
        <div ng-app="MyApp" ng-controller="InternalTransfer">

            <div ng-show="blockUI">
                <div style="width: 100%; height: 100vh; z-index: 999; position: fixed; top: 0; left: 0; right: 0; bottom: 0; align-items: center; display: flex; justify-content: center;">

                    <div style="align-self: center;">
                        <img src="<%= ResolveUrl("~/Images/preloader.svg") %>" loader />

                    </div>

                </div>

            </div>
            <div>
                <%-- <div class="dashedborder"></div>--%>
                <!-- Globalization Tag is added for multilingual  -->
                <div class="row">
                    <label id="Tranferstatus" style=" font-size: 27px; font-weight: bolder;"><b>{{TRNstatus}}</b></label>

                    <button type="button" id="btnList" ng-click="GoToList()" class="btn btn-primary pull-right"><i class="material-icons vl">arrow_back</i> <%= GetGlobalResourceObject("Resource", "BackToList")%></button>
                </div>
                <div class="">

                    <div class="ui-SubHeading ui-SubHeadingBar" id="PrimaryInformationHeader"><%= GetGlobalResourceObject("Resource", "TransferHeader")%> <span class="ui-icon"></span></div>
                    <div class="ui-Customaccordion" id="PrimaryInformationBody">
                        <br />
                        <div class="row">

                            <div class="col m3 s3">
                                <div class="flex">
                                    <input class="" placeholder="" type="text" ng-model="txtRefNumber" id="RefNumber" disabled="disabled" />
                                    <label><%= GetGlobalResourceObject("Resource", "RefNumber")%></label>
                                </div>
                            </div>
                              <div class="col m3 s3">
                                <div class="flex">
                                    <%--<select ng-model="ddlWHId"  ng-options="wh.Id as wh.Name for wh in warehouses"  ng-click="" ng-disabled="txtRefNumber != undefind">
                                <option value="">Select Warehouse</option>
                            </select>--%>
                                    <input type="text" required="required" id="txtWH" ng-model="ddlWHId" ng-disabled="txtRefNumber != undefind" />
                                    <label>Select Warehouse</label>
                                    <span class="errorMsg"></span>
                                    <input type="hidden" id="hifWarehouseId" />
                                </div>
                            </div>
                            <div class="col m3 s3">
                                <div class="flex">
                                    <%-- <select ng-model="ddlTenantId"  ng-options="tnt.Id as tnt.Name for tnt in tenants"  ng-disabled="txtRefNumber != undefind">
                                <option value="">Select Tenant</option>
                            </select>--%>
                                    <input type="text" required="required" id="txtTenant" ng-model="ddlTenantId" ng-disabled="txtRefNumber != undefind" />
                                    <label>Select Tenant</label>
                                    <span class="errorMsg"></span>
                                    <input type="hidden" id="hifTenant" />
                                </div>
                            </div>
                          

                            <div class="col m3 s3">

                                <div class="flex">
                                    <select ng-model="ddtransfertype" ng-options="tnt.Id as tnt.Name for tnt in transfertype" name="D1" ng-disabled="txtRefNumber != undefind">
                                        <option value=""><%= GetGlobalResourceObject("Resource", "SelectTransferType")%></option>
                                    </select>
                                    <span class="errorMsg"></span>
                                </div>
                            </div>
                            <div class="col m3 s3">
                                <div class="flex">
                                    <textarea ng-disabled="txtRefNumber != undefind" ng-model="txtremarks" id="txtremarksid" required=""></textarea>
                                    <label><%= GetGlobalResourceObject("Resource", "Remarks")%></label>
                                </div>
                            </div>
                        </div>

                        <div class="row">

                            <div class="col-md-5 offset-md-7">
                                <div class=" " flex end>
                                    <div class=" " ng-if="ddtransfertype != 5">
                                        <input type="checkbox" id="cnissuggestedreg" class="mspCheckBox" style="display: none !important; width: 13px !important;" ng-model="cbIsReg" ng-disabled="txtRefNumber != undefind" />
                                        <label for="cnissuggestedreg" style="position: static !important; display: none">&nbsp;  <%= GetGlobalResourceObject("Resource", "IsSuggestedRequired")%>    </label>
                                    </div>
                                    &nbsp;&nbsp;&nbsp;
                             <div style="vertical-align: top;" align="right" colspan="6" ng-if="hidesave">
                                 <img src="../Images/bx_loader.gif" id="img1" style="width: 60px; display: none;" />
                                 <button type="button" id="btnsearch" ng-click="GetSave()" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Save")%></button>
                             </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <br />
            <div ng-show="IsheaderCreated">
                <div class="ui-SubHeading ui-SubHeadingBar" id="PrimaryInformationHeader1"><%= GetGlobalResourceObject("Resource", "TransferDetails")%> <span class="ui-icon"></span></div>

                <div class="ui-Customaccordion" id="PrimaryInformationBody1">

                    <b class="tablestyle"><span ng-if="hide" style="font-weight: bold;"><%= GetGlobalResourceObject("Resource", "Details")%> </span></b>
                    <gap5></gap5>

                   <%-- <div class="row">
                        <b>Available Qty.</b> : <label id="AvalQty"> {{Qty}}</label>
                      
                    </div>--%>
                    <div class="" align="center" ng-if="hide">
                        <div class="row">
                            <div class="col m3 s3">
                                <div class="flex">
                                    <%-- <input type="text" list="ddmcode" id="txtmcode" class="TextboxInbound" ng-model="txtmcode" class="form-control" ng-keyup="Getiteminfo()"  ng-change="Getiteminfo()" required="required"/>--%>
                                    <%--<datalist id="ddmcode">
                            <select>
                            <option   ng-repeat="cust in Iteminfo" value="{{cust.Name}}"></option>
                            </select>
                            </datalist> --%>
                                    <input type="text" required id="txtPartNumber" ng-model="txtmcode" ng-focus="Getiteminfo()" />
                                    <label><%= GetGlobalResourceObject("Resource", "PartNumber")%></label>
                                    <span class="errorMsg"></span>
                                    <input type="hidden" id="hifMaterialMaster" />
                                </div>
                            </div>
                            <div class="col m3 s3">
                                <div class="flex">
                                    <%--<input type="text" list="location" id="txtlocationId" class="TextboxInbound" ng-model="txtlocation" class="form-control" ng-keyup="location()" ng-change="location()" required="required" />
                              <span class="errorMsg"></span>--%>
                                    <%--<datalist id="location" >
                            <select>
                            <option ng-repeat="Loc in Locinfo" value="{{Loc.Name}}"></option>
                            </select>
                            </datalist> --%>
                                    <input type="text" required id="txtLocation" ng-model="Location1" ng-focus="GetLocinfo()" />
                                    <label><%= GetGlobalResourceObject("Resource", "Location")%></label>
                                    <span class="errorMsg"></span>
                                    <input type="hidden" id="hdnLocation" />
                                    <%-- <span class="astrickicon"></span>--%>
                                </div>
                            </div>
                            <div class="col m3 s3">
                                <div class="flex">
                                    <%--<input type="text" list="Carton" id="txtCartonId" class="TextboxInbound" ng-model="txtCarton" class="form-control" ng-keyup="Carton()" ng-change="Carton()" required="" />
                            <datalist id="Carton" >
                            <select>
                            <option   ng-repeat="Car in Cartoninfo" value="{{Car.Name}}"></option>
                            </select>
                            </datalist> --%>
                                    <input type="text" required id="txtCartonId" ng-model="txtCarton" ng-focus="Carton()" />
                                    <label><%= GetGlobalResourceObject("Resource", "Pallet")%> </label>
                                    <input type="hidden" id="hdnCarton" />
                                    <%-- <span class="astrickicon"></span>--%>
                                </div>
                            </div>

                            <div class="col m3 s3">
                                <div class="flex">
                                    <input type="text" required id="txtBatchNo" ng-model="BatchNo" ng-focus="getBatchNo()" />
                                    <label><%= GetGlobalResourceObject("Resource", "BatchNo")%></label>
                                    <input type="hidden" id="hdnBatch" />
                                </div>
                            </div>
                           </div>

                            <div class="row">

                            <div class="col m3 s3" ng-if="cbIsReg == false && ddtransfertype != 5">

                                <div class="flex">
                                    <%-- <input type="text" required="" list="ddltoloc" id="txttoloca"   class="TextboxInbound" ng-model="txttolocation"  class="form-control" ng-keyup="GetLocinfo()" ng-change="GetLocinfo()" />
                            <datalist id="ddltoloc">
                            <select>
                            <option   ng-repeat="cust in toloclist" value="{{cust.Name}}"></option>
                            </select>
                            </datalist> --%>
                                    <input type="text" required id="txttoloca" ng-model="txttolocation" ng-focus="getToLocation()" />
                                    <label ng-if="cbIsReg == false && ddtransfertype != 5"><%= GetGlobalResourceObject("Resource", "ToLocation")%> </label>
                                    <span class="errorMsg"></span>
                                    <input type="hidden" id="hdnToLoc" />
                                </div>
                            </div>
                            <div class="col m3 s3" ng-if="ddtransfertype == 5">

                                <div class="flex">
                                    <%--<input type="text" list="ddfromsl" id="txtfromsl" ng-model="txtfromsl" class="form-control" ng-keyup="getCustomerData()" ng-change="getCustomerData()" required="required" />

                                    <datalist id="ddfromsl">
                                        <select>
                                            <option ng-repeat="cust in slinfo" value="{{cust.Name}}"></option>
                                        </select>
                                    </datalist>--%>
                                    <input type="text" required  id="txtfromsl" ng-model="fromsl"   ng-focus="getFromStorageData()"  />
                                    <label ng-if="ddtransfertype == 5"><%= GetGlobalResourceObject("Resource", "FromSL")%>  </label>
                                    <span class="errorMsg"></span>
                                      <input type="hidden" id="hdnFromSL" />
                                </div>
                            </div>
                            <div class="col m3 s3" ng-if="ddtransfertype == 5">

                                <div class="flex">
                                   <%-- <input type="text" list="ddtosl" id="tosl" ng-model="tosl" class="" ng-keyup="getCustomerData()" ng-change="getCustomerData()" required="required" />--%>
                                    <input type="text" required id="txttosl" ng-model="tosl" ng-focus="getToStorageData()" />
                                    <label ng-if="ddtransfertype == 5"><%= GetGlobalResourceObject("Resource", "ToSL")%> </label>
                                  <%--  <datalist id="ddtosl">
                                        <select>
                                            <option ng-repeat="cust in slinfo" value="{{cust.Name}}"></option>
                                        </select>
                                    </datalist>--%>
                                    <input type="hidden" id="hdnToSL" />
                                    <span class="errorMsg"></span>
                                </div>
                            </div>

                            <div class="col m3 s3"> 
                                <div class="flex">
                                    <input type="text" required id="AvalQty1" disabled/>
                                    <label>Available Quantity</label>
                                </div>
                            </div>

                             <div class="col m3 s3">
                                <div class="flex">
                                    <input type="text" required class="" placeholder="" type="text" ng-model="txtQty" id="txtQtyid" onkeypress="return isNumber(event)" required="required" />
                                    <%-- <label><%= GetGlobalResourceObject("Resource", "Quantity")%> </label>--%>
                                    <label>Tranfer Quantity</label>
                                    <span class="errorMsg"></span>
                                </div>
                          
                     </div>
                        <div class="col m3 s3" ng-if="hidesavebtn">
                            <br />
                            <div >

                                <flex end> <button type="button"  ng-model="hidesavedetails"  id="btnsearch" ng-click="SaveTRD()" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "SaveDetails")%> <%=MRLWMSC21Common.CommonLogic.btnfaNew %></button></flex>

                            </div>
                        </div>
                    </div>
                    <br />
                    <table class='table-striped' id="tbldatasonumbers" ng-if="hide">
                        <thead>
                            <tr class=''>
                                <th style="text-align: center"><%= GetGlobalResourceObject("Resource", "PartNumber")%></th>
                                <th style="text-align: center"><%= GetGlobalResourceObject("Resource", "FromLocation")%> </th>
                                <th style="text-align: center"><%= GetGlobalResourceObject("Resource", "Quantity")%> </th>
                                <th style="text-align: center"><%= GetGlobalResourceObject("Resource", "BatchNo")%> </th>
                                <th style="text-align: center"><%= GetGlobalResourceObject("Resource", "FromSL")%>  </th>
                                <th style="text-align: center"><%= GetGlobalResourceObject("Resource", "TOSL")%></th>
                                <th style="text-align: center"><%= GetGlobalResourceObject("Resource", "ToLocation")%></th>
                                <th style="text-align: center" ng-if="ddtransfertype != 5"><%= GetGlobalResourceObject("Resource", "ToContainer")%> </th>
                                <th ng-if="ddtransfertype != 5"></th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr ng-repeat="itemInfo in TRDList" class=''>
                                <td>{{ itemInfo.Mcode }}</td>
                                <td>{{ itemInfo.Location }}</td>
                                <td>{{ itemInfo.Quantiy }}</td>
                                <td>{{ itemInfo.BatchNo }}</td>
                                <td>{{ itemInfo.FromSL }}</td>
                                <td>{{ itemInfo.ToSL }}</td>
                                <td>{{ itemInfo.Tolocation }}</td>
                                <td ng-if="ddtransfertype != 5">
                                    <div class="flex">
                                       <%-- <input required="required" type="text" list="Cartonlist" id="txtcart" class="TextboxInbound" ng-model="itemInfo.ToCarton" ng-click="Cartonlist(itemInfo)" ng-keyup="Cartonlist(itemInfo)" ng-change="Cartonlist(itemInfo)" />--%>

                                         <input required="required" type="text"  id="txtcart"  ng-model="ToCarton" ng-focus="Cartonlist1()"/>
                                        <input type="hidden" id="hdnTocart " />
                                    </div>                                 
                                </td>
                                <td ng-if="ddtransfertype != 5">
                                    <div>
                                        <button type="button" ng-if="itemInfo.IsTransferDone==1" ng-click="Transfer(itemInfo)" class="btn btn-primary" style="width: 100px;"><%= GetGlobalResourceObject("Resource", "Transfer")%> <%=MRLWMSC21Common.CommonLogic.btnfaTransfer %></i></button>

                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
