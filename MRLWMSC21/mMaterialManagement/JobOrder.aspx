<%@ Page Title="JOB Order Creation" Language="C#" MasterPageFile="~/mMaterialManagement/MaterialManagementMaster.master" AutoEventWireup="true" CodeBehind="JobOrder.aspx.cs" Inherits="MRLWMSC21.mMaterialManagement.JobOrder" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
         <script src="../Scripts/angular.min.js"></script>

    <script src="../mReports/Scripts/dirPagination.js"></script>
    <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />
     <link href="../mInventory/Scripts/bootstrap.min.css" rel="stylesheet" />
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>
        <script src="JobOrder.js"></script>
    <style>

        .z-998 {
            position: relative;
            z-index:998;
        }
        </style>
    <div class="angulardiv container" ng-app="MyApp" ng-controller="JOBOrder">
           
        <div ng-show="blockUI">
            <div style="width: 100%; height: 100vh; z-index: 999; position: fixed; top: 0; left: 0; right: 0; bottom: 0; align-items: center; display: flex; justify-content: center; background: rgba(255, 255, 255, 0.24); background: hsla(0, 0%, 100%, 0.72);">

                <div style="align-self: center;">
                   
                    <img width="60" src="../Images/Preloader.svg"/>
                </div>

            </div>

        </div>
        <div class="row">
            <div flex between>
                 <inv-status><label id="lblInboundStatus" ng-bind="JOBHeader.Status"></label></inv-status>
                <button type="button" type="button" class="btn btn-primary" ng-click="changemenulink()"><i class="material-icons vl">arrow_back</i>&nbsp;Back to List</button>
            </div>
        </div>
       
        <div class="ui-SubHeading ui-SubHeadingBar" id="PrimaryInformationHeader">Job Order Header <span class="ui-icon"></span></div>
        <div class="ui-Customaccordion" id="PrimaryInformationBody">

         <%--   <div class="FormLabels flex__ end" style="font-size: 18pt;">

                <label id="lblInboundStatus" ng-bind="JOBHeader.Status"></label>
               
            </div>--%>
            <gap></gap>
            <div class="row">
                <div class="col m3 s3">
                    <div class="flex">
                        <div>
                            <%--  <asp:TextBox ID="txtrefno" runat="server" required="" Enabled="false"></asp:TextBox>--%>
                            <input type="text" ng-model="JOBHeader.JOBRefNo" readonly="true" required="" />

                            <label>Job Order Ref. No.</label>


                        </div>
                    </div>
                </div>
                <div class="col m3 s3">
                    <div class="flex">
                        <div>
                            <%-- <asp:DropDownList runat="server" ID="ddlaccount" runat="server" required="" />--%>
                            <select ng-model="JOBHeader.AccountId" ng-options="tnt.ID as tnt.Value for tnt in AccountData" style="width: 280px;" required="">
                                <option value=""></option>
                            </select>
                            <label>Account</label>

                            <span class="errorMsg"></span>


                        </div>
                    </div>
                </div>
                <div class="col m3 s3">
                    <div class="flex">
                        <div>
                            <input type="text" id="txtTenant" ng-model="JOBHeader.Tenant" required="" />

                            <label>Tenant</label>
                            <span class="errorMsg"></span>
                        </div>
                    </div>

                </div>
                <div class="col m3 s3">
                    <div class="flex">
                        <div>
                            <input type="text" id="txtbomrefno" ng-model="JOBHeader.BOMRefNo" required="" />
                            <span class="errorMsg"></span>
                            <label>BOM Ref. No.</label>

                        </div>
                    </div>
                </div>
            </div>

            <div class="row">

                <div class="col m3 s3">
                    <div class="flex">
                        <div>

                            <input type="text" id="txtUOM" required="" ng-model="JOBHeader.Quantity" onkeypress="return isNumber(event)" required="" />
                            <label>Quantity </label>
                            <span class="errorMsg"></span>
                        </div>
                    </div>
                </div>
                <div class="col m3 s3">
                    <div class="flex">
                        <div>
                            <%-- <input type="text" id="txtremarks" required="" ng-model="JOBHeader.JobOrderType" />--%>
                            <select ng-model="JOBHeader.JobOrderTypeId" ng-options="tnt.ID as tnt.Value for tnt in JoborderTypes" style="width: 280px;" required="">
                                <option value=""></option>
                            </select>
                            <label>JOB Order Type</label>
                            <span class="errorMsg"></span>
                        </div>
                    </div>
                </div>
                <div class="col m3 s3">
                    <div class="flex">
                        <div>
                            <%-- <input type="text" id="txtremarks" required="" ng-model="JOBHeader.JobOrderType" />--%>
                            <select ng-model="JOBHeader.WareHouseId" ng-options="tnt.Id as tnt.Name for tnt in WareHouseData" style="width: 280px;" required="">
                                <option value="">Select WareHouse</option>

                            </select>
                            <span class="errorMsg"></span>
                        </div>
                    </div>
                </div>
            </div>

            <br />
            <div class="flex__ end">
                <div class="flex ">



                    <button type="button" type="button" id="lnkSavePrimaryInfo" class="btn btn-primary" ng-if="JOBHeader.StatusId<=1" ng-click="saveJOBHeader()">Save</button>




                </div>
            </div>
        </div>
        <div ng-show="hidedata">
            <div class="ui-SubHeading ui-SubHeadingBar" id="PrimaryInformationHeader1">Outward Details<span class="ui-icon"></span></div>
            <div class="ui-Customaccordion" id="PrimaryInformationBody1">
                <div class="">
                    <div class=" ">
                       <gap5></gap5>
                        <div flex end>
                            <div ng-if="JOBHeader.StatusId>=2">
                                <strong>SO Number : </strong><a target="_blank" href="../mOrders/SalesOrderInfo.aspx?soid={{releasedata.soheaderid}}">{{releasedata.SONumber}}</a>&nbsp; <strong>Outbound Number : </strong><a target="_blank" href="../mOutbound/OutboundDetails.aspx?obdid={{releasedata.outboundid}}">{{releasedata.OBDNumber}}</a> &nbsp; 
                           <%-- <strong>PO Number : </strong> <a href=""> {{releasedata.PONumber}}</a>--%>
                            </div>
                            <div ng-if="JOBHeader.StatusId<2">
                                <button type="button" type="button" id="lnitiateInward" class="btn btn-primary" ng-click="InitiateOutward()">Release Job Order</button>
                            </div>
                        </div>
                        <gap5></gap5>
                        <div ng-if="resultantassigndata!=undefined && resultantassigndata!=null && resultantassigndata.length!=0">
                            <table id="Table2" class="mytablechildOutbound">
                                <thead>
                                    <tr class="mytableOutboundchildHeaderTR">
                                        <th>SO No.</th>
                                        <th>Part No.</th>
                                        <th>Mfg. Date</th>
                                        <th>Exp. Date</th>
                                        <th>Serial No.</th>
                                        <th>Batch No.</th>
                                        <th>Proj. Ref. No.</th>
                                        <th>Location</th>
                                        <th>Qty.</th>
                                        <th>Status</th>


                                    </tr>
                                </thead>
                                <tbody class="mytableOutboundBodyTR">
                                    <tr ng-repeat="row in resultantassigndata " class="mytableOutboundBodyTR">
                                        <td>{{row.SONumber}}</td>
                                        <td>{{row.MCode}}</td>
                                        <td>{{row.MfgDate}}</td>
                                        <td>{{row.ExpDate}}</td>
                                        <td>{{row.SerialNo}}</td>
                                        <td>{{row.BatchNo}}</td>
                                        <td>{{row.ProjectRefNo}}</td>
                                        <td>{{row.LocationCode}}</td>
                                        <td>{{row.Quantity}}</td>
                                        <td>{{row.ErrorMessage}}</td>

                                    </tr>

                                                                    </tbody>
                                                                </table>
                                                            </div>
                    <div ng-if="PendingQuantities!=undefined && PendingQuantities!=null && PendingQuantities.length!=0">
                                                                <table id="table3" class="mytablechildOutbound">
                                                                    <thead>
                                                                        <tr class="mytableOutboundchildHeaderTR">
                                                                           
                                                                             <th>Part No.</th>
                                                                            <th >Available Qty.</th>
                                                                            <th >Job Qty.</th>
                                                                            <th >Pending Qty.</th>
                                                                            <th>Status</th>
                                                                            
                                                                         

                                                                        </tr>
                                                                    </thead>
                                                                    <tbody  class="mytableOutboundBodyTR">
                                                                        <tr ng-repeat="row in PendingQuantities "  class="mytableOutboundBodyTR">
                                                                            <td>{{row.mcode}}</td> 
                                                                              <td style="padding-left: 55px !important;">{{row.AvailbleQty}}</td>   
                                                                              <td style="padding-left: 30px !important;">{{row.BOMQuantity}}</td>   
                                                                              <td style="padding-left: 55px !important;">{{row.PendingQty}}</td>   
                                                                              <td>{{row.Status}}</td>   
                                                                              
                                                                        </tr>

                                </tbody>
                            </table>
                        </div>

                    </div>
                </div>
            </div>
        </div>

        <div ng-if="JOBHeader.StatusId>2">
            <div class="ui-SubHeading ui-SubHeadingBar" id="PrimaryInformationHeader2">Inward Details<span class="ui-icon"></span></div>
            <div class="ui-Customaccordion" id="PrimaryInformationBody2">
                <div class="">
                    <div class=" ">
                        <br />
                        <div class="flex__ flex__between">
                            <div ng-if="JOBHeader.StatusId>3">
                                <strong>PO Number : </strong><a target="_blank" href="../mOrders/PODetailsInfo.aspx?poid={{releasedata.poheaderid}}">{{releasedata.PONumber}}</a>&nbsp; <strong>Inbound Number : </strong><a target="_blank" href="../mInbound/InboundDetails.aspx?ibdid={{releasedata.inboundid}}">{{releasedata.StoreRefNo}}</a> &nbsp; 
                           <%-- <strong>PO Number : </strong> <a href=""> {{releasedata.PONumber}}</a>--%>
                            </div>
                            <div ng-if="JOBHeader.StatusId==3">
                                <button type="button" type="button" id="lnitiateoutward" class="btn btn-primary" ng-click="InitiateInward()">Initiate Inward</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
