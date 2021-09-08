<%@ Page Title="Search Inbound" Language="C#" MasterPageFile="~/mInbound/InboundMaster.master" AutoEventWireup="true" CodeBehind="NewInboundSearch.aspx.cs" Inherits="MRLWMSC21.mInbound.NewInboundSearch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="IBContent" runat="server">
    <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />

    <script type="text/javascript" src="Scripts/CommonScripts.js"></script>
    <script type="text/javascript" src="../Scripts/timeentry/jquery.timeentry.js"></script>

    <link href="../mInventory/Scripts/bootstrap.min.css" rel="stylesheet" />
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>
    
     <script src="../Scripts/angular.min.js"></script>
      <script src="../mReports/Scripts/dirPagination.js"></script>
    <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />
    <script src="NewInboundSearch.js"></script>
    
  
    <script src="../Scripts/xlsx.full.min.js"></script>
    <script src="../Scripts/xlsx-model.js"></script>
    <link href="../Scripts/mdtimepicker.css" rel="stylesheet" />
    <script src="../Scripts/mdtimepicker.js"></script>
    <script src="Scripts/InventraxAjax.js"></script>

    <script src="../mInventory/CycleCountScripts/jquery.dataTables.min.js"></script>
    <script src="../mInventory/CycleCountScripts/dataTables.bootstrap.min.js"></script>
    
 <script>
         $(document).ready(function () {
             debugger
      
                     
                $('#txtFromDate').datepicker({
                    dateFormat: "dd-M-yy",
                    maxDate: new Date(),
                    onSelect: function (selected) {
                        $("#txtFromDate").datepicker("option", "minDate", selected, { dateFormat: "dd-M-yy" })
                    }
                });
                 
                $('#txtToDate').datepicker({
                    dateFormat: "dd-M-yy",
                    maxDate: new Date(),
                    onSelect: function (selected) {
                        $("#txtToDate").datepicker("option", "minDate", selected, { dateFormat: "dd-M-yy" })
                    }
             });

             $(function () {
            $('.isvisibleNow').on('click', function () {
                $('.ishideNow').slideToggle();
            });
        });

            });
    </script>
     <style>
        .material-icons {
            line-height:0 !important
        }
                .ishideNow {
            display:none;
        }
                .tooltip {
    height: auto;
    color: #121212;
    padding-top: 10px;
    padding-bottom: 10px;
    padding-left: 6px;
    padding-right: 6px;
    font-weight: normal;
    display: block;
    position: absolute;
    margin: 0 0 12px 0;
    width: auto;
    text-align: left;
    border-radius: 0px !important;
    background: #fdf9f9;
    font-size: 12px !important;
    border: 1px solid #a6a5a5;
    box-shadow: var(--z1-1);
}
    </style>


  
    <div class="container" ng-app="myApp" ng-controller="GateList">
        
<div ng-show="blockUI">
            <div style="width: 100%; height: 100vh; z-index: 999; position: fixed; top: 0; left: 0; right: 0; bottom: 0; align-items: center; display: flex; justify-content: center; background: rgba(255, 255, 255, 0.24); background: hsla(0, 0%, 100%, 0.72);">

                <div style="align-self: center;">
                    <img width="60" src="../Images/preloader.svg" />

                </div>

            </div>

        </div>

       
        <div class="row">
            
                 <div class="col s3 m3">
                    <div class="flex">
                        <div>
                           
                            <input type="text" id="txtWarehouse"  required=""  />
                           <label>Warehouse</label>
                            <span class="errorMsg"></span>

                        </div>
                    </div>
                </div>
                 <div class="col s3 m3">
                    <div class="flex">
                        <div>
                            <input type="text" id="txttenant"  required=""/>
                           <label>Tenant</label>
                        </div>
                    </div>
                </div>
                 <div class="col m3 s3">
                    <div class="flex">
                        <div>
                            <input type="text" id="txtFromDate"   required=""/>

                            <label>From Date</label>


                        </div>
                    </div>
                </div>
                 <div class="col m3 s3">
                    <div class="flex">
                        <div>
                            <input type="text" id="txtToDate"   required=""/>

                            <label>To Date</label>


                        </div>
                    </div>
                </div>
         </div>
         <div class="row">
           
               
              <div class="col m3 s3">
                    <div class="flex">
                        <div>
                            <input type="text" id="txtstatus"   required=""/>

                            <label>Inbound Status</label>


                        </div>
                    </div>
                </div>
             <div class="ishideNow">
              <div class="col m4 s4">
                    <div class="flex">


                        <select name="Select Category" id="selCategory">
                            <option value="0" selected>Select Category</option>
                            <option value="1">Store Ref. Number</option>
                            <option value="3">Invoice Number</option>
                            <option value="8">GRN #</option>
                            <option value="11">Supplier</option>
                            <option value="13">PO Number</option>

                        </select>
                    </div>
                </div>
              <div class="col m4 s4">
                    <div class="flex">                          
                            <input type="text" id="txtSearch" required=""/>
                            <label>Search</label>
                    </div>
                </div>

             <div class="col m4 s4">
                    <div class="flex">                        
                            <input type="text" id="txtShipment" required=""/>
                            <label>Shipment Type</label>                        
                    </div>
                </div>
    
            </div>
    </div>
        <div class="row">
            <div class="col m2">
                <b>Total : {{Totalrecords}}</b>
            </div>
            <div class="col m10" flex end>
                <button type="button"  ng-click = "getAdvancedData()" class="btn btn-primary obd isvisibleNow">Advanced Search <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>&nbsp;
                <button type="button"  ng-click = "getSearchData(1)" class="btn btn-primary obd">Search <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>&nbsp;
                <button type="button"  ng-click = "getExportData()" class="btn btn-primary obd">Export Excel <%=MRLWMSC21Common.CommonLogic.btnfaExcel %></button>
            </div>
        </div>
        
            <%--<div class="row">
                <div class="col m2 s3 offset-m6">
                    <div class="flex">
                        <div>
                           <button type="button"  ng-click = "getAdvancedData()" class="btn btn-primary obd">Advanced Search <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>&nbsp;&nbsp;

                        </div>
                    </div>
                </div>
                <div class="col s3 m2">
                    <div class="flex">
                        <div>
                            <button type="button"  ng-click = "getSearchData(1)" class="btn btn-primary obd">Search <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>&nbsp;&nbsp;

                        </div>
                    </div>
                </div>
               
                <div class="col s3 m2">
                   
                    
                      
                        <div class="flex">
                                 <button type="button"  ng-click = "getExportData()" class="btn btn-primary obd">Export Excel <%=MRLWMSC21Common.CommonLogic.btnfaExcel %></button>&nbsp;&nbsp;
                        </div>
                    
                </div>
            </div>--%>
   
       
     
        
              <div ng-if="InboundList!=undefined && InboundList!=null && InboundList.length!=0">

            

             <table class="table-striped">
                    <thead>
                        <tr>
                            <th>Store Ref.#</th>
                             <th>Shipment Type</th>
                             <th>Doc. Rcvd. Date</th>
                            
                            <th>Ship. Rcvd. Dt. / Offload Time</th>
                             <th>Tenant</th>
                             <th>Supplier</th>
                            <th>Store(s)</th>
                           <th>PO Number</th>
                            <th>Invoice</th>
                            <th>GRN</th>
                             <th>GRN Done By</th>
                            
                             <th>Verified Date</th>
                            <th>Status</th>
                            <th>RTS</th>
                          
                            <th>RCR</th>
                             <th>Modify</th>
                            
                           <%-- <th>Action</th>--%>
                          

                        </tr>
                    </thead>

                    <tbody>
                        <tr dir-paginate="Obj in InboundList  |itemsPerPage:25" pagination-id="main" total-items="Totalrecords"> 
                            <td>{{Obj.StoreRefNo}}</td>
                            <td>{{Obj.ShipmentType}}</td>
                           
                            <%-- <td>{{Obj.DocReceivedDate}}</td> --%>
                         <td>{{Obj.DocReceivedDate |date:'dd-MM-yy'}}</td> 
                           
                            <td>{{Obj.ShipmentReceivedOn |date:'dd-MM-yy'}} {{Obj.Offloadtime}}</td>
                             <td>{{Obj.CompanyName}}</td>
                            <td>{{Obj.SupplierName}} </td>
                           
                             <td>{{Obj.Warehouse}}
                                <%-- <span style="color:red" ng-if="Obj.InboundStatusID!=1 && Obj.InboundStatusID!=2" >[Received]</span>--%>
                             </td>
                             <td>{{Obj.PONumber}}</td> 
                             <td>{{Obj.InvoiceNumber}}</td> 
                             <td number>{{Obj.GRNNumber}}</td>
                            <td number>{{Obj.GRNDoneBy}}</td>
                            <td>{{Obj.ShipmentVerifiedOn |date:'dd-MM-yy'}}</td>
                             <td>{{Obj.InboundStatus}}</td>
                            <td style="width:70px !important;"><a target="_blank" class="helpWTitle vip" Title="FalconWMS® - Receiving Tally Sheet(RTS) | Receiving Tally Sheet with barcoded material codes to receive items for putaway." href="../mInbound/RTReport.aspx?ibdid={{Obj.InboundID}}&&lineitemcount ={{Obj.RowNum}}&&TN={{Obj.CompanyName}}">RTR{{[Obj.LineCount]}}<i style="color:#ce2827 !important" class="material-icons">arrow_right</i></a></td>
                            <td style="width:70px !important;"><a target="_blank" class="helpWTitle vip" title="FalconWMS® - Receipt Confirmation Report(RCR)" href="../mReports/ReceiptConfirmationReport.aspx?ibdid={{Obj.InboundID}}&&lineitemcount ={{Obj.RowNum}}&&TN={{Obj.CompanyName}}">RCR<i style="color:#ce2827 !important" class="material-icons">arrow_right</i></a></td>
                           
                            <td>
                                <div><a target="_blank"  ng-click="Edit(Obj.InboundID)"><i class="material-icons ss Edit">mode_edit</i><em class="sugg-tooltis">Edit</em></a></div>
                            </td>
                            
                         
                        </tr>
                    </tbody>
                </table> 
         <div style="float:right !important;font-family:Arial;font-size:small;margin-right:1%;">
          <dir-pagination-controls direction-links="true" boundary-links="true" pagination-id="main" on-page-change="getSearchData(newPageNumber)"> </dir-pagination-controls>  
             
        </div> 
        </div>


    </div>
</asp:Content>
