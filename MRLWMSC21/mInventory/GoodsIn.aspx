<%@ Page Title="Goods Receipt (IN)" Language="C#" MasterPageFile="~/mInventory/InventoryMaster.master" AutoEventWireup="true" CodeBehind="GoodsIn.aspx.cs" Inherits="MRLWMSC21.mInventory.GoodsIn" %>
<asp:Content ID="Content1" ContentPlaceHolderID="InvContent" runat="server">
   
  
    <script src="Scripts/angular.min.js"></script>
    <script src="GoodsIn.js?v1.1"></script>
    <script src="../mReports/Scripts/dirPagination.js"></script>
    <%--<script src="../mReports/Scripts/dirPagination.js"></script>--%>

    <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />
     <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
<%--    <script src="../Scripts/xlsx-model.js"></script>
<%--    <script src="../Scripts/xlsx.full.min.js"></script>--%>
   <%-- <script src="../Scripts/xlsx.full.min.js"></script>--%>

    <script type="text/javascript">
        function isNumber(evt) {
            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
                return false;
            return true;
        }

        //function selectAll() {
        //    debugger;

        //    console.log($("#Deleteall").is(":checked"));
        //    // console.log($("#MainContent_IBContent_GVPOLineItems_action").prop("checked"));
        //    if ($("#Deleteall").prop("checked") == true) {
        //        $(".alldel").prop("checked", true);
        //        for (var i = 0; i < $scope.GMDInfo.length; i++) {
                                           
        //          $scope.GMDInfo[i].IsSelected = true;                                                             
        //        }
        //    }
        //    else {
        //        $(".alldel").prop("checked", false);
        //        for (var i = 0; i < $scope.GMDInfo.length; i++) {
        //            $scope.GMDInfo[i].IsSelected = false;

        //        }
        //    }

        //}

    </script>

    <script>
        $(document).ready(function () {
            debugger;
            CustomAccordino($('#dvPOHDHeader'), $('#dvPOHDBody'));
            //CustomAccordino($('#divCusAddress'), $('#dvCusAddressBody'));

            $("#txtmfgdate").datepicker({
                dateFormat: "dd-M-yy",
                maxDate: new Date(),
                onSelect: function (selected) {
                    $("#txtexpdate").datepicker("option", "minDate", selected, { dateFormat: "dd-M-yy" })
                }
            });

            $("#txtexpdate").datepicker({
                dateFormat: "dd-M-yy",
                //maxDate: new Date()
            });

            //Added by kashyap on 21/08/2017  to reslove the server issue 
            $('#txtmfgdate, #txtexpdate').keypress(function () {
                return false;
            });
            
        });
        
    </script>

    
    <div ng-app="MyApp" ng-controller="stockin" class="container">
       <div ng-show="blockUI">
            <div style="width: 100%; height: 100vh; z-index: 999; position: fixed; top: 0; left: 0; right: 0; bottom: 0; align-items: center; display: flex; justify-content: center; background: rgba(255, 255, 255, 0.24); background: hsla(0, 0%, 100%, 0.72);">
                <div style="align-self: center;">
                    <img width="60" src="../Images/preloader.svg" />
                </div>
            </div>
        </div>
        <div >

            <div>
                <div class="row">

                    <div class="col m3">
                        <div class="flex">

                            <input type="text" list="ddtentcode" id="txttenat" class="dr" ng-model="txttenateID" class="form-control" ng-keyup="GetStorefeNum()" ng-change="GetStorefeNum()" required="">
                            <label>Tenant </label>
                            <span class="errorMsg"></span>
                            <datalist id="ddtentcode">
                                <select>
                                    <option ng-repeat="tn in tenants" value="{{tn.Name}}"></option>
                                </select>
                            </datalist>
                        </div>

                    </div>

                    <div class="col m3">
                        <div class="flex">
                            <input type="text" list="ddstore" id="txtstoreid" class="dr" ng-model="txtstore" class="form-control" ng-keyup="GetSKUList()" ng-change="GetSKUList()" required="">
                            <label>Store Ref. No. </label>
                            <span class="errorMsg"></span>
                            <datalist id="ddstore">
                                <select>
                                    <option ng-repeat="st in StoreIf" value="{{st.Name}}"></option>
                                </select>
                            </datalist>
                        </div>

                    </div>
                    <div class="col m3">
                        <div class="flex">
                            <input type="text" list="ddsku" id="txtsku" class="dr" ng-model="txtskuid" class="form-control" required="">
                            <label>SKU</label>
                            <span class="errorMsg"></span>
                            <datalist id="ddsku">
                                <select>
                                    <option ng-repeat="SKU in SkuInfo" value="{{SKU.Name}}"></option>
                                </select>
                            </datalist>
                        </div>
                    </div>
                    <div class="col m3">
                        <gap5></gap5>
                       <%-- <button id="btnimport" type="button" ng-click="GetDetails()" class="btn btn-primary ">Get Details<i class="fa fa-folder-open" aria-hidden="true"></i></button>--%>
                   
                            <button type="button" class="btn btn-primary" ng-click="backTo()">Back to List <%=MRLWMSC21Common.CommonLogic.btnfaList %></button>
                       
                    </div>
                </div>
            </div>

            <table class="table-striped" style="" ng-if="hide">
                <thead>
                     <tr>
                         <th>S.NO</th>
                         <th>PO Number</th>
                         <th>Supplier Invoice</th>
                         <th>Suggested Location</th>
                         <th>Invoice Qty.</th>
                         <th>Received Qty.</th>
                         <th>Batch No.</th>
                         <th>Mfg. Date  </th>
                         <th>Exp. Date</th>
                          <th>Project Ref# No.</th>
                         <th>MRP</th>
                          <th>Serial No.</th>
                         <th>HU No.</th>
                         <th>HU Size</th>
                         <th>Select</th>
                     </tr>
                 </thead>
                 <tbody>
                     <tr ng-repeat="itemInfo in SuggestedInfo">
                         <td >{{$index+1}}</td>
                         <td>{{ itemInfo.Ponumber }}</td>
                         <td>{{ itemInfo.SupplierInvoice }}</td>
                         <td>{{ itemInfo.Location }}</td>
                         <td >{{ itemInfo.SuggestedQty }}</td>
                         <td >{{ itemInfo.ReceivedQty }}</td>
                         <td>{{ itemInfo.BatchNo }}</td>
                         <td>{{ itemInfo.MfgDate }}</td>
                         <td>{{ itemInfo.ExpDate }}</td>
                          <td>{{ itemInfo.ProjectRef }}</td>
                          <td>{{ itemInfo.MRP }}</td>
                         <td>{{ itemInfo.SerialNumber }}</td>
                          <td>{{itemInfo.HUNo}}</td>
                         <td>{{itemInfo.HUSize}}</td>
              
                         <td >
                             
                              <div class="md-radio"><input type="radio" id="{{$index + 1}}" value="{{itemInfo.Isselected}}"  ng-model="itemInfo.Isselected"   name="radioSelect" ng-click="Getputawaylist(itemInfo,itemInfo.SupplierInvoice,itemInfo.SuggestedPutawayID)"/><label for="{{$index + 1}}"></label></div></td>
                     </tr>
                 </tbody>
          </table>
   
            <br />

            <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="dvPOHDHeader" style="font-size: 13pt">MSPs</div>
            <div class="ui-Customaccordion" id="dvPOHDBody">
                <gap></gap>
                <div>
                    <div class="row">
                        <div class="col m3">
                            <div class="flex">
                                <input type="text" id="txtbatch" ng-model="goodsindata.BatchNo" ng-readonly="noneditableBatch" required="">
                                <label>Batch No.</label>
                            </div>
                        </div>
                        <div class="col m3">
                            <div class="flex">
                                <input type="text" id="txtmfgdate" onpaste="return false;" ng-model="goodsindata.MfgDate" required="">
                                <label>Mfg. Date</label>
                            </div>
                        </div>
                        <div class="col m3">
                            <div class="flex">
                                <input type="text" id="txtexpdate" ng-model="goodsindata.ExpDate" ng-readonly="noneditableExp" required="">
                                <label>Exp. Date</label>
                            </div>
                        </div>
                   
                        <div class="col m3">
                            <div class="flex">
                                <input type="text" id="txtMRP" ng-model="goodsindata.MRP" required="">
                                <label>MRP</label>
                            </div>
                        </div> 

                    </div>
                    <div class="row">
                        <div class="col m3">
                            <div class="flex">
                                <input type="text" id="txtserialno" ng-model="goodsindata.SerialNo" ng-readonly="noneditableSer" required="">
                                <label>Serial No.</label>
                            </div>
                        </div>
                        <div class="col m3">
                            <div class="flex">
                                <input type="text" id="txtProjectrefNo" ng-readonly="noneditableProj" ng-model="goodsindata.ProjectRefNo"  required="" ng-model="goodsindata.MRP">
                                <label>Project Ref. No.</label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
           
            <br />

            <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="dvPutAwayHeader" style="font-size: 13pt">Putaway list</div>
            <div class="ui-Customaccordion" id="dvPutAwayBody">
                <div class="" ng-if="hide1">
                    <div class="row">
                        <%-- <td> Suggested  Qty :{{iteminfo.Quantity}}</td>--%>

                        <div class="col m4">
                            <div class="flex__" style="padding-bottom: 30px; padding-top: 10px">
                                <b>Suggested  Qty :</b>
                                <label>{{SuggestedQty}}</label>
                            </div>
                        </div>


                        <div class="col m4">
                            <div class="flex__" style="padding-bottom: 30px; padding-top: 10px"><b>Quantity Receive in BUoM :</b><label>{{BUoMQty}}</label></div>
                        </div>


                        <div class="col m4">
                            <div class="flex__" style="padding-bottom: 30px; padding-top: 10px">
                                <b>Suggested  Pending Qty :</b><label>{{SuggestedPendingQty}}</label>
                            </div>
                        </div>

                    </div>

                    <div class="row" style="display: none">
                        <div class="col m3" style="display: none">
                            <div class="flex">
                                Is Damaged:
                                <input type="checkbox" id="cbisdamage" ng-model="goodsindata.IsDamaged" class="form-control" />
                            </div>
                        </div>
                        <div class="col m3" style="display: none">
                            <div class="flex">
                                Has Discrepancy:
                                <input type="checkbox" id="cbisdescreb" ng-model="goodsindata.hasDiscrepancy" class="form-control" />
                            </div>
                        </div>
                    </div>

                  <%--  <tr>
                        <td>
                            <br />
                        </td>
                    </tr>--%>
                    <%--       <tr>
                    <td> 
                        <div class="flex">
                         <input type="text" id="txtbatch" class="dr" ng-model="goodsindata.BatchNo" ng-readonly="noneditableBatch" required="">
                         <label>Batch No</label></div>
                    </td>
                    <td> 
                        <div class="flex">
                         <input type="text" id="txtmfgdate" class="dr" ng-model="goodsindata.MfgDate" ng-readonly="noneditableMfg" required="">
                        <label>Mfg Date</label>
                            </div>
                    </td>
                    <td>
                        <div class="flex">
                         <input type="text" id="txtexpdate" class="dr" ng-model="goodsindata.ExpDate" ng-readonly="noneditableExp" required="">
                            <label>Exp Date</label>
                            </div>
                    </td>
                    <td> 
                        <div class="flex">
                         <input type="text" id="txtprojectref"  ng-model="goodsindata.ProjectRefNo" class="dr" required="">
                        <label>Project Ref</label>
                            </div>
                    </td>
                    <td>
                        <div class="flex">
                         <input type="text" id="txtserialno" ng-model="goodsindata.SerialNo" class="dr" required="">
                        <label>Serial no</label></div>
                    </td>
                </tr>--%>
                 <%--   <tr>
                        <td>
                            <br />
                        </td>
                    </tr>--%>
                    <div class="row">
                        <div class="col m4">
                            <div class="flex">
                                <input type="text" id="txtlocation" class="dr" ng-model="goodsindata.Location" ng-click="getalllocations()"  ng-focus="getalllocations()" ng-keyup="getalllocations()" required="">
                                <span class="errorMsg"></span>
                                <label>Location</label>
                                <%-- <datalist id="ddlLocation">
                                <select>
                                <option   ng-repeat="LOC in Locations" value="{{LOC.Name}}"></option>
                                </select>
                                </datalist>--%>
                            </div>
                        </div>
                        <div class="col m4">
                            <div class="flex">
                                <input type="text" list="ddlcontainer" id="txtcantainer" class="dr" ng-model="goodsindata.Carton" ng-keyup="getallcartons(goodsindata.Location)" class="form-control" ng-change="GetcontainerDetails()" ng-click="GetcontainerDetails()" required="">
                                <span class="errorMsg"></span>
                                <label>Pallet</label>
                                <%--<datalist id="ddlcontainer">
                            <select>
                            <option   ng-repeat="CON in Containers" value="{{CON.Name}}"></option>
                            </select>
                            </datalist>--%>
                            </div>
                        </div>
                        <div class="col m4">
                            <div class="flex">

                                <input type="text" list="ddlStorage" id="txtStorage"  ng-model="txtStorageLocation" ng-focus="LoadSL()"  required="">
                                <span class="errorMsg"></span>
                                <label>Storage Location</label>
                                <%-- <datalist id="ddlStorage">
                            <select>
                            <option   ng-repeat="SL in SLOC" value="{{SL.Name}}"></option>
                            </select>
                            </datalist>--%>
                            </div>
                        </div>
                    </div>
                    <div class="row" style="display: none">
                        <div class="col m3">
                            <div class="flex">
                                <select ng-model="ddlprinter" class="" ng-options="pt.Id as pt.Name for pt in Printers" required="">
                                    <option value=""></option>
                                </select>
                                <label>Select Printer</label>
                            </div>
                        </div>
                        <div class="col m3">
                            <div class="flex">
                                <select ng-model="ddllabel" class="" ng-options="lb.Id as lb.Name for lb in labels" required="">
                                    <option value=""></option>
                                </select>
                                <label>Label Size</label>
                            </div>
                        </div>


                    </div>
                   <%-- <tr>
                        <td>
                            <br />
                        </td>
                    </tr>--%>
                    <div class="row">
                        <div class="col m4">

                            <div class="flex">

                                <input type="number" onkeypress="return isNumber(event)" min="0" id="txtQty" class="dr" ng-model="goodsindata.DocQty" required="">

                                <span class="errorMsg"></span>
                                <label>Quantity</label>
                            </div>
                        </div>

                        <div class="col m4">
                            <div class="flex">
                                <input type="text" id="txtRemarks" ng-model="goodsindata.remarks" required="">
                                <label>Remarks</label>
                            </div>
                        </div>
                        <div class="col m4">
                            <div class="flex">
                                <select ng-model="ddskipreason" id="ddskipid" ng-options="tnt.Id as tnt.Name for tnt in Skiplist" class="" name="D1" required="">
                                    <option value=""></option>
                                </select>
                                <span ng-if="skiplist" class="errorMsg"></span>
                                <label>Select Skip Reason</label>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <%--<div class="col m6">
                            <div class="flex">
                                <input type="text" style="visibility: hidden" />
                            </div>
                        </div>--%>
                        <div class="col m4">
                            
                            <div class="flex">
                                <input type="text" id="txtvehicled" ng-focus="getvehiclelist()" class="dr"  required="">
                                <span class="errorMsg"></span>
                                <label>Vehicle</label>
                            </div>
                        
                        </div>
                        <div class="col m6">
                            <div class="" flex end>
                                <button id="btnreceive" type="button" ng-click="GetReceive()" class="btn btn-primary">Receive<i class="fa fa-folder-open" aria-hidden="true"></i></button>
                            </div>
                        </div>
                    </div>
                    <%--<tr>
                        <td>
                            <br />
                        </td>
                    </tr>--%>
                </div>
            </div>
            <br />

           
            <table class="table-striped">
                <thead>
                    <tr class='mytableOutboundHeaderTR'>
                        <th>Container</th>
                        <th>Location</th>
                        <th>Storage Location</th>
                        <th>Quantity</th>
                        <th>Mfg. Date</th>
                        <th>Exp. Date</th>
                        <th>Batch#</th>
                        <th>Serial#</th>


                        <th>
                            <input type="checkbox" id="Deleteall" class="deleteall" ng-change="selectAll()" ng-model="myValue" />
                            <%--Delete--%>
                        </th>
                        <%-- <th style="text-align: center">Print</th>--%>
                       <%-- <th>Select</th>--%>
                    </tr>
                </thead>
                <tbody>           
                     <tr dir-paginate="QC in GMDInfo|itemsPerPage:10" class='mytableOutboundBodyTR' >   
                     <td>{{ QC.CartonCode }}</td>
                     <td>{{ QC.Location }}</td>
                     <td>{{ QC.StorageLocations }}</td>
                     <td align="center">{{ QC.Quantity }}</td>
                     <td align="center">{{ QC.MfgDate }}</td>
                     <td align="center">{{ QC.ExpDate }}</td>
                     <td align="center">{{ QC.BatchNo }}</td>
                     <td align="center">{{ QC.SerialNo }}</td>
                     <td><input type="checkbox"  id="Checkdelete" class="alldel" ng-model="QC.IsSelected"  /></td>         
                    </tr>
                    <tr >
                       <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                         <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                         <td>
                            <div ng-if="GMDInfo.length!=0">
                              <span  ng-click="deleteQCitem()" class="relative"> <i class="material-icons">delete</i><em class="sugg-tooltis"  style="left: 32px;">Delete</em>
                              </span>
                             </div>
                         </td>
                    </tr>
                  
                 
                </tbody>
            </table>
            <br />


           <%-- <div class="row">

                <div class="col m10">
                    <div class="" flex end>
                        <button id="btnRevertReceipt" type="button" ng-click="RevertReceiving()" class="btn btn-primary">Revert<i class="material-icons">delete</i></button>
                    </div>
                </div>
            </div>--%>

            <dir-pagination-controls max-size="3" direction-links="true"  boundary-links="true" style="float:right"> </dir-pagination-controls>
            <br />

            <div ng-if="hideerrorcode">
                <table>
                     <thead>
                    <tr class='mytableOutboundHeaderTR'>
                        <th style="text-align: center">Sl.No.</th>
                        <th style="text-align: center">Error Code</th>
                        <th style="text-align: center">Description</th>
                    </tr>
                </thead>
                    <tbody>
                    <tr ng-repeat="ec in Suggestederrorcode" class='mytableOutboundBodyTR'>
                        <td>{{$index+1}}</td>
                        <td>{{ ec.errorCode }}</td>
                        <td align="center">{{ ec.errordescription }}</td>
                    </tr>
                </table>
            </div>

        </div>




          <div id="divContainer" class="PopupContainerInbound" style="display:none" style="height:300px;width:1020px">
            <div id="divInner" class="PopupInnerOutbound" style="height:300px;width:820px">
                <div class="PopupHeadertextOutbound">QC Parameters Capture</div>
                    <span id="spanClose" class="fa fa-times PopupSpanCloseOutbound" aria-hidden="true"></span>&emsp;
                        <div class="PopupPaddingOutbound">
                            <div class="PopupSpaceOutbound">
                                <br />
                             
                             <div style="text-align:left;color:Green;margin-left:1%;font-family:Arial;font-size:11pt;font-weight:bold;"></div>
                                 <div class="lineheight"></div>

                                <table runat="server" id="tbGoodsMovementDetails" class="table-striped" style="width: 40%; border: 1px solid; border-color: var(--paper-grey-300); padding: 0px 6px; margin: auto;">
                                    <tr>
                                        <td>

                                            <label><strong>Location</strong></label></td>
                                        <td>{{QCLocation}}</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label><strong>Is Damaged</strong></label></td>
                                        <td>{{QCIsDamaged}}</td>
                                    </tr>
                                    <tr>

                                        <td>

                                            <label><strong>Has Discrepancy</strong></label></td>
                                        <td>{{QCHasDiscrepancy}}</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label><strong>Is Non Confirmity</strong></label></td>
                                        <td>{{QCIsNonConfirmity}}</td>

                                    </tr>
                                    <tr>

                                        <td>
                                            <label><strong>Mfg Date</strong></label></td>
                                        <td>{{QCMfgDate}}</td>

                                    </tr>
                                    <tr>

                                        <td>
                                            <label><strong>Exp Date</strong></label></td>
                                        <td>{{QCExpDate}}</td>

                                    </tr>
                                    <tr>

                                        <td>
                                            <label><strong>Batch No.</strong></label></td>
                                        <td>{{QCBatchNo}}</td>

                                    </tr>
                                    <tr>
                                        <td>
                                            <label><strong>Serial No. </strong></label>
                                        </td>
                                        <td>{{QCSerialNo}}</td>
                                    </tr>

                                </table>
                                <br />
                             <%--   <table ng-if="QChide">
                                    <tr>
                                        <td>
 <input type="checkbox" ng-model="ASIs"  id="ng-change-example2" ng-true-value="1" ng-false-value="0" />Is As<br />
                                           
                                        </td>
                                        <td><span style="color:red">*</span>Remarks:
   <textarea id="textarea1" cols="80" rows="10" ng-model="$parent.QCRemarks"></textarea>
                                        </td>
                                    </tr>
                                </table>--%>
                                <br />

                                <div style="text-align:center;">
                                 <table class="table-striped" >
                                                    <thead>
                                                        <tr class="mytableOutboundHeaderTR"> 
                                                            <th></th>                                                           
                                                              <th>QC Serial No.</th>  
                                                            <th>QC Quantity</th>
                                                             
                                                            <th></th>
                                                        </tr>
                                                    </thead>
                                                    <tbody class="mytableOutboundBodyTR">
                                                         <tr ng-repeat="QCList in QCUpdatedList" class="mytableOutboundBodyTR">                                                     
                                                          
                                                              
                                                           <td >
                                                               <%--<input type="radio" ng-model="QCdetailID" name="radioSelect1" value="false" ng-click="GetbulkQty(QCList.Quantity,QCList.QCSerialNo)"/>--%>
                                                               <input type="radio" name="radio-primary" ng-value="true" ng-model="QCList.Isselect" ng-checked="QCList.Isselect" ng-click="UpdatePrimary(QCList,$index)" />
                                                               
                                                             <%--  <input type="radio" name="groupName" ng-model="QCList.Isselect" />--%>

                                                           </td>
                                                           <td>{{QCList.QCSerialNo}}</td>
                                                           <td>{{QCList.Quantity}}</td>
                                                              <td><button type="button" ng-if="QCList.QCParamCaptureID>0" id="btndelete"  class="btn btn-primary" style="width:60px;" ng-click="deleteQCCaptureitem(QCList)">Delete</button></td>
                                                    <%--         <td style="text-align: left;">
                                                             <input type="checkbox" ng-model="AsIs" id="ngASIS"  ng-change="disablenonconfirm()"/></td>--%>
                                                             </tr>
                                                      
                                                      
                                                        
                                                    </tbody>
                                                </table>
                                    </div>
                                <br />



                                <table>
                                    <tr style="display: flex; justify-content: space-between;">
                                        
                                        <td>
                                            <div class="flex__" style="padding-bottom:5px"><label><strong>QC Completed Qty</strong></label>: {{CompletedQty}} </div></td>
                                        <td><div class="flex__"><label><strong>Total Qty</strong></label> :&nbsp;&nbsp; {{TotalQty}}</div></td>
                                      
                                       <td> <div class="flex__"><label><strong>QC Ref. No.</strong></label>:&nbsp;&nbsp; {{QCRefNo}}</div></td>
                                    </tr>
                                   
                                    <tr>
                                        <td>
                                            <div class="row">
                                                <div class="col m4 p0">
                                                    <div class="flex">
                                                        <input type="text"  id="" onkeypress="return isNumber(event)" ng-model="QCQuantity" required="" />
                                                        <label>Bulk QC Quantity </label>
                                                    </div>
                                                    </div></div>
                                        </td>
                                        <td><input type="hidden"  id=""  ng-model="GoodsMID" style="width:200px !important"/></td>
                                    </tr>
                                    <tr>
                                 <%--       <td>
                                             <input type="checkbox" ng-model="NonConfirmity"  id="txtNonConfirmity" ng-true-value="1" ng-false-value="0" ng-click="bindlocs()"/>Is Non Confirmity<br />
                                           
                                              <input list="ddlNMLocation" id="txtNMLocation" class="dr" ng-model="NormalLocation" ng-keyup="GetNMLocs()"   class="form-control"  />
                            <datalist id="ddlNMLocation">
                            <select>
                            <option   ng-repeat="NM in NMLOC" id="{{NM.Id}}" value="{{NM.Name}}"></option>
                            </select>
                            </datalist>
                                        </td>--%>

                                    <%--     <td><span style="color:red">*</span>Remarks:
   <textarea id="textarea1" cols="80" rows="10" ng-model="$parent.QCRemarks"></textarea>
                                        </td>--%>
                                    </tr>
                                <tr>
                                                            <td colspan="5" style="text-align:right"></td>
                                                        </tr></table>
                                <br />
                               <br/>
                               <%--  <div id="divParameters"></div>--%>
                               
                                <table>
                                    <tr>
                                        <td>
                                            <div class="row">
                                            <div  ng-repeat="QC in QCParams">
                                                <div class="col m4 p0">
                                                    <div class="flex">
                                                        <input type="text" ng-model="QC.value" onkeypress="return isNumber(event)"  required="" />
                                                        <label>{{QC.ParameterName}} ( {{QC.MinTolerance}} - {{QC.MaxTolerance}} )</label>
                                                    </div>
                                                </div>
                                              </div>
                                                </div>
                                        </td>
                                    </tr>
                                </table>
                                  <div style="float:right;padding:10px">
                                       <button type="button" id="btnClose" class="btn btn-primary" style="width:86px;">Close <%=MRLWMSC21Common.CommonLogic.btnfaClear %></button>&nbsp;
                                      <button type="button" id="btnRelease" class="btn btn-primary"  ng-click="saveBulkQCItemsParameters()">Capture QC Parameters <%=MRLWMSC21Common.CommonLogic.btnfaSave %></button>
                                     </div>
                      
                            </div>

                            </div>
                    </div>                
            </div>
     
    </div>
     <br /><br />  






         


</asp:Content>
