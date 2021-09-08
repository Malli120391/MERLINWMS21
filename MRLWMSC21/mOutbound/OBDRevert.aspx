<%@ Page Title="" Language="C#" MasterPageFile="~/mOutbound/OutboundMaster.master" AutoEventWireup="true" CodeBehind="OBDRevert.aspx.cs" Inherits="MRLWMSC21.mOutbound.OBDRevert" %>
<asp:Content ID="Content1" ContentPlaceHolderID="OBContent" runat="server">
     <script src="../Scripts/angular.min.js"></script>
    <script src="OBDRevert.js"></script>
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
    <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />
    <script src="../mReports/Scripts/dirPagination.js"></script>
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>
    <link href="../mInventory/Scripts/bootstrap.min.css" rel="stylesheet" />
    <style>
        .btn-icon {
            background: none !important;
            color: #333 !important;
            border: none !important;
            box-shadow: none !important;
        }
        .btn-icon:hover {
            color:#333 !important
        }
        @media (min-width: 768px){
            .modal-dialog {
                width: 600px;
                margin: 30px auto auto 220px !important;
            }
        }
        .doc__file {
            color:blue !important;
        }
        .head__doc{

        }
    </style>
    <div ng-app="myApp" ng-controller="OBDRevert" class="container">
        <div class="row">
            <div class="col m3">
                <div class="flex">
                    <input type="text" id="txtWarehouse" required="" />
                    <label><%= GetGlobalResourceObject("Resource", "WareHouse")%></label>
                    <span class="errorMsg"></span>
                </div>
            </div>

            <div class="col m3">
                <div class="flex">
                    <input type="text" id="txtTenant" required="" />
                    <label><%= GetGlobalResourceObject("Resource", "Tenant")%></label>
                    <span class="errorMsg"></span>
                </div>
            </div>
            <div class="col m3">
                <div class="flex">
                    <input type="text" id="txtFromDate" placeholder="From Date" class="" datepicker />
                </div>
            </div>
            <div class="col m3">
                <div class="flex">
                    <input type="text" id="txtToDate" placeholder="To Date" class="" datepicker />
                </div>
            </div>
            
        </div>
        <div class="row">
            <div class="col m3">
                 <div class="flex">
                    <select id="txtprpo" name="txtprpo" class="form-control">
                        <option value="0" selected></option>
                        <option value="1">Sent to Store</option>
                        <option value="2">On Hold</option>
                        <option value="5">Sent for PGI</option>
                        <option value="4">Delivered</option>                      
                    </select>
                   <label>Delivery Status</label>
                </div>
            </div>
            <div class="col m3">
                 <div class="flex">
                    <select id="txtCategory" name="txtCategory" class="form-control">
                        <option value="0" selected></option>
                        <option value="1">SO Number</option>
                        <option value="2">OBD Number</option>
                    </select>
                   <label>Select Category</label>
                </div>
            </div>
             <div class="col m3">
                <div class="flex">
                    <input type="text" id="txtSearch" placeholder="Search" class="" />
                </div>
            </div>
            
            

            <div class="col m2">
                <gap5></gap5>
                <button type="button" ng-click="getOBDlist(1)" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Search")%>  <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
            </div>
        </div>


        <div class="row">
            <div class="col m12">
                    <table class="table-striped" id="tbldatas">
                        <thead>
                            <tr>                               
                                <th>Delv. Doc. #</th>
                                <th>Tenant</th>
                                <th>Customer</th>
                                <th>Warehouse</th>
                                <th>Delv. Doc. Date</th>
                                <th>SO #</th>
                                <th>Status</th>
                                <th align="right">SO Qty.</th> 
                                <th>OBD Qty.</th> 
                                <th>Picked Qty.</th>
                                <th>Packed Qty.</th>
                                <th>Loaded Qty.</th>
                                <th>PGI Qty.</th>
                                <th>Action</th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr dir-paginate="exp in OBDList|itemsPerPage:25" total-items="Totalrecords">
                                <%--<td align="center">{{$index +1}}</td>--%>
                                <td>{{exp.OBDNumber}}</td>
                                <td>{{exp.Tenant}}</td>
                                <td>{{exp.Customer}}</td>
                                <td>{{exp.Warehouse}}</td>
                                <td>{{exp.DeliveryDocDate}}</td>
                                <td>{{exp.SONumber}}</td>
                                <td>{{exp.Status}}</td>
                                <td align="right">{{exp.SOQty}}</td>   
                                <td align="right">{{exp.OBDQty}}</td> 
                                <td align="right">{{exp.PickedQty}}</td>
                                <td align="right">{{exp.PackedQty}}</td> 
                                <td align="right">{{exp.LoadedQty}}</td> 
                                <td align="right">{{exp.PGIQty}}</td>                     
                                <td> <button type="button" id="btnRevert{{exp.OutboundID}}" class="btn btn-primary" ng-if="exp.Status!='Cancelled'"  ng-click ="RevertOBD(exp.OBDNumber, exp.DeliveryTypeID);">{{exp.RevertStatus}}</button></td>
                                <td><button type='button' id="btn{{exp.OutboundID}}" ng-if="exp.Status == 'Picking In Process'"  class='btn btn-primary btn-icon' data-toggle='modal' data-target='#AddEntityToCreate' ng-click='GetOBDDetails(exp.OutboundID);'><i class='fa fa-eye' aria-hidden='true'></i></button></td>
                                <%--<td><button type="button" id="btn{{exp.OutboundID}}" class="btn btn-primary" ng-click =" GetDisplayColumns();" data-target="#showColOptions" data-toggle="modal"><%= GetGlobalResourceObject("Resource", "ColumnOptions")%>  <i class="material-icons">list</i></button></td>--%>
                            </tr>
                        </tbody>
                         <tfoot>
                            <tr>
                                <td colspan="15">
                                    <div class="divpaginationstyle">
                                        <dir-pagination-controls direction-links="true" boundary-links="true" on-page-change="getOBDlist(newPageNumber)"> </dir-pagination-controls>
                                    </div>
                                </td>
                            </tr>
                        </tfoot>

                    </table>

                <table id="tbldata"></table>
            </div>
            <div class="divlineheight"></div>

        </div>
        <!-- ========================= Modal Popup For Cycle Count Entity Details ========================================== -->
    <div class="modal inmodal" id="AddEntityToCreate" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog" style="width: 80% !important;">
            <div class="modal-content animated fadeIn">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title">Picking Revert</h4>
                </div>

                <div class="modal-body">
                    <div id="divValidationCycleCountEntityMessages" class="text-danger"></div>
                    <p></p>
                    <p></p>
                    <div class="row">
                        <div class="col m6">
                            <span>Delv. Doc. #:</span><span class="doc__file">{{OBDDetails.length>0 ? OBDDetails[0].OBDNumber:""}}</span>
                        </div>
                        <div class="col m6" style="text-align: right;">
                             <span>Customer: &nbsp</span><span class="doc__file">{{OBDDetails.length>0 ? OBDDetails[0].CustomerCode:"" }}</span>
                        </div>
                       
                    </div>
                    <div class="row">
                       
                        <div class="col m6">
                            <span>Warehouse: &nbsp</span><span class="doc__file">{{OBDDetails.length>0 ? OBDDetails[0].WHCode:"" }}</span>
                        </div>
                        
                    </div>
                   
                    <br />
                    <div class="row">
                        <div class="col m12">
                            <table class="table-striped" id="tblModel" style="overflow: auto !important;">
                                <thead>
                                    <tr>                               
                                        <th>Line#</th>
                                        <th>Part Number</th>
                                        <th>SO#</th>
                                        <th>UOM</th>
                                        <th>Order Qty.</th>
                                        <th>Location</th>                                    
                                        <th>Pallet</th> 
                                        <th>Assigned Qty.</th>
                                        <th>Batch No.</th>
                                        <th>Mfg. Date</th>
                                        <th>Exp. Date</th>
                                        <th>MRP</th>
                                        <th>Serial No</th>
                                        <th>Project Ref No.</th>
                                        <th>Picked Qty</th>
                                        <th>Reason</th>
                                        <th>Revert Qty.</th>
                                        <th></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr ng-repeat="exp in OBDDetails" >            
                                        <td>{{exp.LineNumber}}</td>
                                        <td>{{exp.MCode}}</td>
                                        <td>{{exp.SONumber}}</td>
                                        <td>{{exp.UoM}}</td>
                                        <td>{{exp.SOQuantity}}</td>                       
                                        <td>{{exp.DisplayLocationCode}}</td>                            
                                        <td >{{exp.CartonCode}}</td>   
                                        <td align="right">{{exp.AssignedQuantity}}</td> 
                                        <td>{{exp.BatchNo}}</td>
                                        <td>{{exp.MfgDate}}</td> 
                                        <td>{{exp.ExpDate}}</td> 
                                        <td>{{exp.MRP}}</td>
                                        <td>{{exp.SerialNo}}</td>
                                        <td>{{exp.ProjectRefNo}}</td>
                                        <td>{{exp.PickedQty}}</td>               
                                        <td>
                                            <div class="flex">
                                                <select id="selrevert{{exp.OutboundID}}" class="EntityfieldToGet" ng-model ="exp.ReasonID">
                                                     <%--<option value="0">Select Reason</option>--%>
                                                     <option value="1">Is Damaged</option>
                                                     <option value="2">Is OBD Revert</option>
                                                     <option value="3">Is Release Revert </option>
                                                </select>
                                                <label>Select Reason</label>
                                                <span class="errormsg"></span>
                                            </div>

                                        </td>
                                        <td class="flex"><input type="number" id="txtrevert{{exp.OutboundID}}" ng-model ="exp.RevertQty"/></td>
                                        <td><button type='button' id="btn{{exp.OutboundID}}"  class='btn btn-primary btn-icon'  ng-click='RevertPickItem(exp, this);'> <i class='fa fa-refresh' aria-hidden='true'></i></button></td>
                                        <%--<td><button type="button" id="btn{{exp.OutboundID}}" class="btn btn-primary" ng-click =" GetDisplayColumns();" data-target="#showColOptions" data-toggle="modal"><%= GetGlobalResourceObject("Resource", "ColumnOptions")%>  <i class="material-icons">list</i></button></td>--%>
                                    </tr>
                                </tbody>
                                 
                            </table>
                    </div>
                 </div>
                <div class="modal-footer">
                    <input type="hidden" value="0" id="CCM_CNF_AccountCycleCountDetail_ID" class="EntityfieldToGet" />
                    <button type="button" class="btn btn-white" data-dismiss="modal" style="color: white !important;">Close</button>
                    <%--<button type="button" class="btn btn-primary" id="btnEntityCreate" onclick="return UpsertEntityData();">Create</button>--%>
                </div>
            </div>
        </div>
    </div>
    <!-- ========================= END Modal Popup For Cycle Count Entity Details ========================================== -->
</asp:Content>
