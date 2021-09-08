<%@ Page Title="BillingReport" Language="C#" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="BillingReport.aspx.cs" Inherits="MRLWMSC21.mReports.BillingReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
    <script src="BillingReport.js"></script>
    <link href="../Scripts/bootstrap/bootstrap.min.css" rel="stylesheet" />
      <script src="Scripts/jquery-ui-1.8.24.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <%--<script src="Scripts/docraptor-1.0.0.js"></script>--%>
    <script src="Scripts/html2canvas.min.js"></script>
    <script src="Scripts/pdfmake.min.js"></script>
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
      <script>
       //   document.getElementById('pdfdiv').classList.add("temp"); 
          var downloadPDF = function () {
              debugger;
              var pdfElement = document.getElementById('pdfdiv');
              pdfElement.innerHTML = "";
              pdfElement.innerHTML += "<div style='display:flex;justify-content:center'>";
              pdfElement.innerHTML +="  <img id='Image1' width='166' height='50' src='/MRLWMSC21_SL/Images/inventrax.png'> </div>";
              var viewElement = document.getElementById('Datapdf');
              pdfElement.innerHTML += viewElement.innerHTML;
              pdfElement.innerHTML += " <div>Powered By Inventrax </div>";
              pdfElement.classList.remove("temp"); 
             // pdfElement.className += " pdf";
              html2canvas(pdfElement, {
                  scale: window.devicePixelRatio,
                  useCORS: true,
                    onrendered: function (canvas) {
                        var data = canvas.toDataURL();    
                        var docDefinition = {
                            content: [{
                                image: data,
                                width:500,
                                pageSize:'A4'
                            }]
                        };
                        pdfMake.createPdf(docDefinition).download("Billing Report.pdf");
                       
                    }

              });
              pdfElement.className += " temp";

          }
  </script>
     <script type="text/javascript">
         var RefTenant = '';
         var WarehouseID = '';
         $(document).ready(function () {
         
             $("#txtFromdate").datepicker({
                 dateFormat: "dd-M-yy",
                 maxDate: new Date(),
                 onSelect: function (selected) {
                     $(this).focus();
                     $("#txttodate").datepicker("option", "minDate", selected, { dateFormate: "dd-M-yy" })
                 }
             });
             $("#txttodate").datepicker({
                 dateFormat: "dd-M-yy",
                 maxDate: new Date(),
                 onSelect: function (selected) {
                     $(this).focus();
                 }
             });

             $('#txtFromdate, #txttodate').keypress(function () {
                 return false;
             });

             
             // $scope.getTenant = function () {

             //alert('11');
             //  debugger;
             $('#txtTenant').val("");

             var textfieldname = $("#txtTenant");
             DropdownFunction(textfieldname);
             $("#txtTenant").autocomplete({
                 source: function (request, response) {
                     $.ajax({
                         url: '../mWebServices/FalconWebService.asmx/LoadTenantsForReports',
                         data: "{ 'prefix': '" + request.term + "'}",
                         dataType: "json",
                         type: "POST",
                         contentType: "application/json; charset=utf-8",
                         success: function (data) {

                             debugger;
                            // $scope.Tenantdata = data;
                             response($.map(data.d, function (item) {
                                 return {
                                     label: item.split(',')[1],
                                     val: item.split(',')[0]
                                 }
                             }))
                         }
                     });
                 },
                 select: function (e, i) {
                     RefTenant = i.item.val;
                     // alert(Refnumber);
                    // $scope.ngtenant = i.item.val;
                 },
                 minLength: 0
             });

              var textfieldname = $("#txtWarehouse");
                DropdownFunction(textfieldname);

                $("#txtWarehouse").autocomplete({
                    source: function (request, response) {
                        debugger;
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadWarehousesBasedonTenant") %>',
                            data: JSON.stringify({ 'prefix': request.term, 'tenantID': RefTenant }),
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {

                                response($.map(data.d, function (item) {
                                    return {
                                        label: item.split(',')[0],
                                        val: item.split(',')[1]
                                    }
                                }))
                            }
                        });
                    },
                    select: function (e, i) {

                       // $("#hifwarehouse").val(i.item.val);
                        //alert($("#hifToteTypeID").val());
                        WarehouseID=i.item.val
                    },
                    minLength: 0
                });
         });
        
    </script>

    <style>
        .set{float:right;}
        .pdf{
           font-size:20px !important;   
        }
        .pdf table thead tr th{font-size:20px !important;}
        .pdf table tbody tr td{font-size:20px !important;}
        .pdf p {font-size:20px !important}
        .pdf h4 {font-size:25px !important}
        #BillTo{
                width: 40%;
    border: 1px solid #e5e5e5;
    margin-right: 10px;
    padding: 0px 10px;
        }
        .heading { text-align:center}
        .display{
            display:flex; justify-content:space-between;
        }
        .v-table {
                        border: 1px solid #ddd;
                        width:60%;
                    }
         .v-table td {
                        position: relative;
                        border-bottom: 1px solid #f5f5f5;
                        empty-cells: show;
                        padding: 9px 0 9px 12px;
                        vertical-align: top;
                        font-weight: normal;
                        text-align: left;
                        font-size:  11.5px !important;
                        border-right: 1px solid #e5e5e5;
                    }
        .table-striped ~ p {text-align:right;
        font-weight:bold;
        }
        .temp{display:none}
    </style>
    <div ng-app="myApp" ng-controller="BillingReport" class="pagewidth">
         <div ng-show="blockUI">
            <div style="width: 100%; height: 100vh; z-index: 999; position: fixed; top: 0; left: 0; right: 0; bottom: 0; align-items: center; display: flex; justify-content: center;">

                <div style="align-self: center;">
                    <img src="<%= ResolveUrl("~/Images/preloader.svg") %>" loader/>

                </div>

            </div>

        </div>
        <div class="divlineheight"></div>
        <div>
            <div class="divlineheight"></div>
            <div class="row">
                <div class="">
                    <div class="">
                            <div class="col m2">
                            <div class="flex">
                                <input type="text" id="txtTenant"  ngchange="getTenant()" ng-click="getTenant()" class="TextboxInventoryAuto" style="margin-bottom: 0px !important;" required="required" /><span class="errorMsg"></span>
                                <label>Tenant</label>
                            </div>
                       </div>
                          <div class="col m2">
                            <div class="flex">
                                <input type="text" id="txtWarehouse" class="TextboxInventoryAuto" style="margin-bottom: 0px !important;" required="required" /><span class="errorMsg"></span>
                                <label>Warehouse</label>
                            </div>
                       </div>
                        
                        <div class="col m2">
                            <div class="flex">
                                <input type="text" class=""  onpaste="return false;" ng-model="fromdate" id="txtFromdate" required="" />
                                <label><%= GetGlobalResourceObject("Resource", "FromDate")%> </label>
                                <span class="errorMsg"></span>
                            </div>
                        </div>
                        <div class="col m2">
                            <div class="flex">
                                <input type="text" class=""  onpaste="return false;" ng-model="todate" id="txttodate" required="" /><label><%= GetGlobalResourceObject("Resource", "ToDate")%> </label>
                                <span class="errorMsg"></span>
                            </div>
                        </div>
                        <div class="col m4">
                            <gap></gap>
                            <button type="button" ng-click="ViewReport()" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "ViewReport")%> <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                             <button type="button" style="display:none;" class="btn btn-primary"><span id="btnPdf" onclick="downloadPDF()"><i class="fa fa-file-pdf-o" aria-hidden="true"></i>&nbsp;&nbsp;<%= GetGlobalResourceObject("Resource", "DowndloadPDF")%> </span></button>
                            <button type="button" class="btn btn-primary" ng-click="generatePDF()">Export PDF<i class="fa fa-file-pdf-o" aria-hidden="true"></i></button>
                        </div>
                    </div>

                </div>

            </div>

        </div>
        <div id="Datapdf" ng-show="billDetails">
            <div class="display">
                <div id="BillTo" >
       <%--     <h4>Bill To</h4>--%>
                         <h4> <%= GetGlobalResourceObject("Resource", "BillTo")%></h4>
                    {{Data.Table5[0].TenantName}}<br />
                    {{Data.Table5[0].Address1}}<br />
                    {{Data.Table5[0].Address2}}<br />
                    {{Data.Table5[0].City}}<br />
                    {{Data.Table5[0].State}}<br />
                     {{Data.Table5[0].ZIP}}
        </div>

                <div class="v-table">
                    <table>
                        <tbody>
                            <tr>
                                <td rowspan="2">Inbound Charges</td>
                                <%-- <td rowspan="2">InboundCharges <%= GetGlobalResourceObject("Resource", "ProjectedVehicleDetails")%></td>--%>
                                <%--<td>Unloading & Handling Charges</td>--%>
                                <td><%= GetGlobalResourceObject("Resource", "UnloadingHandlingCharges")%> </td>
                                <td>{{Total1}}</td>
                            </tr>
                            <tr>
                               <%-- <td>Receiving and Putaway Charges</td>--%>
                                 <td><%= GetGlobalResourceObject("Resource", "ReceivingandPutawayCharges")%></td>
                                <td>{{Total2}}</td>
                            </tr>
                            <tr>
                            <%--    <td rowspan="2">Outbound Charges</td>--%>
                                    <td rowspan="2"><%= GetGlobalResourceObject("Resource", "OutboundCharges")%> </td>
                               <%-- <td>Picking and Packing Charges</td>--%>
                                 <td><%= GetGlobalResourceObject("Resource", "PickingandPackingCharges")%></td>
                                <td>{{Total3}}</td>
                            </tr>
                             <tr>
                               <%-- <td>Loading Charges</td>--%>
                                  <td> <%= GetGlobalResourceObject("Resource", "LoadingCharges")%></td>
                                <td>{{Total4}}</td>
                            </tr>
                            <tr>
                               <%-- <td colspan="2">Storage Charges</td>--%>
                                 <td colspan="2"> <%= GetGlobalResourceObject("Resource", "StorageCharges")%></td>
                                <td>{{Total5}}</td>
                            </tr>
                                <tr>
                               <%-- <td colspan="2">Grand Total (KWD)</td>--%>
                                    <%-- <td colspan="2"> <%= GetGlobalResourceObject("Resource", "GrandTotalKWD")%></td>--%>  <%--commented by lalitha on 20/02/2019--%>
                                    <td colspan="2" >{{TotalCur.S}}</td>
                                <td>{{Total5+Total4+Total3+Total2+Total1}}</td>
                            </tr>
                        </tbody>
                    </table>

                </div>
            </div>
        

        <div id="inbBill" >
          <%--  <h4 class="heading"> Inbound Billing Report </h4>--%>
              <h4 class="heading"> <%= GetGlobalResourceObject("Resource", "InboundBillingReport")%> </h4>
            <div>
           <table class="table-striped">
               <thead>
                   <tr>
                     <%--  <th colspan="10" style="text-align:center !important">Unloading & Handling Charges</th>--%>
                         <th colspan="10" style="text-align:center !important"><%= GetGlobalResourceObject("Resource", "UnloadingHandlingCharges")%></th>
                   </tr>
                   <tr>
                      <%-- <th>Store Ref. No.</th>--%>
                        <th><%= GetGlobalResourceObject("Resource", "StoreRefNo")%> </th>
                      <%-- <th>Supplier</th>--%>
                        <th><%= GetGlobalResourceObject("Resource", "Supplier")%></th>
                      <%-- <th>PO Number </th>--%>
                        <th> <%= GetGlobalResourceObject("Resource", "PONumber")%> </th>
                      <%-- <th>Receipt Date</th>--%>
                        <th> <%= GetGlobalResourceObject("Resource", "ReceiptDate")%></th>
                      <%-- <th>Service/Material</th>--%>
                        <th> <%= GetGlobalResourceObject("Resource", "ServiceMaterial")%></th>
                      <%-- <th>UoM</th>--%>
                        <th> <%= GetGlobalResourceObject("Resource", "UoM")%></th>
                      <%-- <th>Quantity</th>--%>
                        <th><%= GetGlobalResourceObject("Resource", "Quantity")%></th>
                      <%-- <th>Unit Cost (KWD)</th>--%>
                     <%--   <th><%= GetGlobalResourceObject("Resource", "UnitCostKWD")%></th>--%>  <%--commented by lalitha on 20/02/2019 --%>
                       <th>{{UnitCur.S}}</th>
                    <%--   <th> Tax</th>--%>
                          <th> <%= GetGlobalResourceObject("Resource", "Tax")%> </th>
                      <%-- <th>Total Cost (KWD)</th>--%>
                       <%-- <th><%= GetGlobalResourceObject("Resource", "TotalCostKWD")%></th>--%>
                       <%--<th >{{ Currency.S }}</th>--%>
                       <th>Total</th>
                   </tr>
               </thead>
               <tbody>
                   <tr ng-repeat="t in  Data.Table">
                       <td>{{t.StoreRefNo}}</td>
                        <td>{{t.SupplierName}}</td>
                        <td>{{t.PONumber}}</td>
                        <td>{{t.Receipt}}</td>
                          <td>{{t.ServiceMaterial}}</td>
                        <td>{{t.UoM}}</td>
                        <td>{{t.Quantity}}</td>
                       <td>{{t.UnitCost}}</td>
                       <td>{{t.Tax}}</td>
                       <td>{{t.TotalCost}}</td>

                   </tr>
               </tbody>
           </table>
            <%--    <p>Total Cost (KWD): {{Total1}}</p>--%>
                    <%--<p><%= GetGlobalResourceObject("Resource", "TotalCostKWD")%>: {{Total1}}</p>--%>
               <%-- <p><%= GetGlobalResourceObject("Resource", "TotalCostKWD")%>: {{Currency}}</p>--%>
               <%-- <p>{{Currency.S}}: {{Total1}}</p>--%>
                 <p>Total: {{Total1}}</p>

            </div>
            <div>
                <br /><br />
 <table class="table-striped">
               <thead>
                   <tr>
                     <%--  <th colspan="10" style="text-align:center !important">Receiving & Putaway Charges</th>--%>
                         <th colspan="10" style="text-align:center !important"><%= GetGlobalResourceObject("Resource", "ReceivingPutawayCharges")%></th>
                   </tr>
                   <tr>
                       <%--<th>Store Ref. No.</th>--%>
                           <th><%= GetGlobalResourceObject("Resource", "StoreRefNo")%> </th>
                     <%--  <th>Supplier</th>--%>
                        <th><%= GetGlobalResourceObject("Resource", "Supplier")%></th>
                       <%--<th>PO Number R</th>--%>
                       <th><%= GetGlobalResourceObject("Resource", "PONumberR")%></th>
                      <%-- <th>Receipt Date</th>--%>
                        <th> <%= GetGlobalResourceObject("Resource", "ReceiptDate")%></th>
                    <%--   <th>Service/Material</th>--%>
                         <th> <%= GetGlobalResourceObject("Resource", "ServiceMaterial")%></th>
                     <%--  <th>UoM</th>--%>
                           <th> <%= GetGlobalResourceObject("Resource", "UoM")%></th>
                       <%--<th>Quantity</th>--%>
                            <th><%= GetGlobalResourceObject("Resource", "Quantity")%></th>
                      <%-- <th>Unit Cost (KWD)</th>--%>
                       <%--<th><%= GetGlobalResourceObject("Resource", "UnitCostKWD")%></th>--%>
                        <th>{{UnitCur.S}}</th>
                    <%--   <th> Tax</th>--%>
                         <th> <%= GetGlobalResourceObject("Resource", "Tax")%> </th>
                     <%--  <th>Total Cost (KWD)</th>--%>
                       <%-- <th><%= GetGlobalResourceObject("Resource", "TotalCostKWD")%></th>--%>
                        <%--<th >{{ Currency.S }}</th>--%>
                        <th>Total</th>
                   </tr>
               </thead>
                    <tbody>
                   <tr ng-repeat="t in  Data.Table1">
                       <td>{{t.StoreRefNo}}</td>
                        <td>{{t.SupplierName}}</td>
                        <td>{{t.PONumber}}</td>
                        <td>{{t.Receipt}}</td>
                          <td>{{t.ServiceMaterial}}</td>
                        <td>{{t.UoM}}</td>
                        <td>{{t.Quantity}}</td>
                       <td>{{t.UnitCost}}</td>
                       <td>{{t.Tax}}</td>
                       <td>{{t.TotalCost}}</td>

                   </tr>
               </tbody>
           </table>
               <%-- <p>Total Cost (KWD): {{Total2}}</p>--%>
              <%--   <p><%= GetGlobalResourceObject("Resource", "TotalCostKWD")%> : {{Total2}}</p>--%>
               <%-- <p>{{Currency.S}}: {{Total2}}</p>--%>
                 <p>Total : {{Total2}}</p>
            </div>
        </div>
            <br />
        <div id="ObdBill">
        <%--    <h4 class="heading">Outbound Billing Report</h4>--%>
                <h4 class="heading"> <%= GetGlobalResourceObject("Resource", "OutboundBillingReport")%></h4>
            <div>
                <table class="table-striped">
               <thead>
                   <tr>
                       <%--<th colspan="10" style="text-align:center !important">Picking & Packing Charges</th>--%>
                       <th colspan="10" style="text-align:center !important"><%= GetGlobalResourceObject("Resource", "PickingPackingCharges")%> </th>
                   </tr>
                   <tr>
                      <%-- <th>OBD Number</th>--%>
                        <th><%= GetGlobalResourceObject("Resource", "OBDNumber")%> </th>
                     <%--  <th>Customer</th>--%>
                         <th><%= GetGlobalResourceObject("Resource", "Customer")%></th>
                       <%--<th>SO Number R</th>--%>
                       <th> <%= GetGlobalResourceObject("Resource", "SONumberR")%> </th>
                      <%-- <th>Delivery Date</th>--%>
                        <th> <%= GetGlobalResourceObject("Resource", "DeliveryDate")%> </th>
                      <%-- <th>Service/Material</th>--%>
                      <th> <%= GetGlobalResourceObject("Resource", "ServiceMaterial")%></th>
                      <%-- <th>UoM</th>--%>
                        <th><%= GetGlobalResourceObject("Resource", "UoM")%> </th>
                       <%--<th>Quantity</th>--%>
                       <th><%= GetGlobalResourceObject("Resource", "Quantity")%> </th>
                  <%--     <th>Unit Cost (KWD)</th>--%>
                          <%--  <th> <%= GetGlobalResourceObject("Resource", "UnitCostKWD")%></th>--%>
                        <th>{{UnitCur.S}}</th>
                      <%-- <th> Tax</th>--%>
                        <th> <%= GetGlobalResourceObject("Resource", "Tax")%> </th>
                       <%--<th>Total Cost (KWD)</th>--%>
                      <%-- <th><%= GetGlobalResourceObject("Resource", "TotalCostKWD")%> </th>--%>                      
                             <%--<th >{{ Currency.S }}</th>--%>
                        <th>Total</th>
                   </tr>
               </thead>
                     <tbody>
                   <tr ng-repeat="t in  Data.Table2">
                       <td>{{t.OBDNumber}}</td>
                        <td>{{t.CustomerName}}</td>
                        <td>{{t.SONumber}}</td>
                        <td>{{t.Delivery}}</td>
                        <td>{{t.ServiceMaterial}}</td>
                        <td>{{t.UoM}}</td>
                        <td>{{t.Quantity}}</td>
                       <td>{{t.UnitCost}}</td>
                       <td>{{t.Tax}}</td>
                       <td>{{t.TotalCost}}</td>

                   </tr>
               </tbody>
           </table>
                <%--<p>Total Cost (KWD): {{Total3}}</p>--%>
               <%-- <p><%= GetGlobalResourceObject("Resource", "TotalCostKWD")%>: {{Total3}}</p>--%>                
                   <%--<p>{{Currency.S}}: {{Total3}}</p>--%>
                <p>Total : {{Total3}}</p>
            </div>
            <br /><br />
            <div>
 <table class="table-striped">
               <thead>
                   <tr>
                       <%--<th colspan="10" style="text-align:center !important">Loading Charges</th>--%>
                       <th colspan="10" style="text-align:center !important"><%= GetGlobalResourceObject("Resource", "LoadingCharges")%> </th>
                   </tr>
                   <tr>
                      <%-- <th>OBD Number</th>--%>
                        <th><%= GetGlobalResourceObject("Resource", "OBDNumber")%> </th>
                   <%--    <th>Customer</th>--%>
                           <th><%= GetGlobalResourceObject("Resource", "Customer")%> </th>
                     <%--  <th>SO Number R</th>--%>
                        <th> <%= GetGlobalResourceObject("Resource", "SONumberR")%> </th>
                      <%-- <th>Delivery Date</th>--%>
                      <th> <%= GetGlobalResourceObject("Resource", "DeliveryDate")%> </th>
                   <%--    <th>Service/Material</th>--%>
                         <th> <%= GetGlobalResourceObject("Resource", "ServiceMaterial")%></th>
                     <%--  <th>UoM</th>--%>
                       <th><%= GetGlobalResourceObject("Resource", "UoM")%> </th>
                     <%--  <th>Quantity</th>--%>
                         <th><%= GetGlobalResourceObject("Resource", "Quantity")%> </th>
                   <%--    <th>Unit Cost (KWD)</th>--%>
                         <%-- <th> <%= GetGlobalResourceObject("Resource", "UnitCostKWD")%></th>--%>
                        <th>{{UnitCur.S}}</th>
                     <%--  <th> Tax</th>--%>
                        <th> <%= GetGlobalResourceObject("Resource", "Tax")%> </th>
                      <%-- <th>Total Cost (KWD)</th>--%>
                            <%--<th><%= GetGlobalResourceObject("Resource", "TotalCostKWD")%> </th>--%>
                            <%-- <th >{{ Currency.S }}</th>--%>
                        <th>Total</th>
                   </tr>
               </thead>
                         <tbody>
                   <tr ng-repeat="t in  Data.Table3">
                       <td>{{t.OBDNumber}}</td>
                        <td>{{t.CustomerName}}</td>
                        <td>{{t.SONumber}}</td>
                        <td>{{t.Delivery}}</td>
                          <td>{{t.ServiceMaterial}}</td>
                        <td>{{t.UoM}}</td>
                        <td>{{t.Quantity}}</td>
                       <td>{{t.UnitCost}}</td>
                       <td>{{t.Tax}}</td>
                       <td>{{t.TotalCost}}</td>

                   </tr>
               </tbody>
           </table>
               <%-- <p>Total Cost (KWD): {{Total4}}</p>--%>
                <%-- <p><%= GetGlobalResourceObject("Resource", "TotalCostKWD")%>: {{Total4}}</p>--%>
                <%--<p>{{Currency.S}}: {{Total4}}</p>--%>
                <p>Total : {{Total4}}</p>
            </div>
        </div>
<br />
        <div id="StBill">
           <%-- <h4 class="heading"> Storage Billing Report </h4>--%>
             <h4 class="heading">  <%= GetGlobalResourceObject("Resource", "StorageBillingReport")%> </h4>
            <table class="table-striped">
                <thead>
                    <tr>
                        <%--<th>Part Number</th>--%>
                        <th> <%= GetGlobalResourceObject("Resource", "PartNumber")%> </th>
                    <%--    <th>Description </th>--%>
                            <th> <%= GetGlobalResourceObject("Resource", "Description")%> </th>
                     <%--   <th>UoM</th>--%>
                           <th><%= GetGlobalResourceObject("Resource", "UoM")%></th>
                       <%-- <th>Date</th>--%>
                         <th><%= GetGlobalResourceObject("Resource", "Date")%> </th>
                    <%--    <th>Avaliable Qty.</th>--%>
                            <th><%= GetGlobalResourceObject("Resource", "AvaliableQty")%> </th>
                       <%-- <th>Unit Cost </th>--%>
                        <%-- <th> <%= GetGlobalResourceObject("Resource", "UnitCost")%> </th>--%>
                         <th>{{UnitCur.S}}</th>
                         <%--<th>Total Qty.</th>--%>
                        <%--<th> <%= GetGlobalResourceObject("Resource", "TotalQty")%> </th>--%>
                        <%--<th >{{ Currency.S }}</th>--%>
                         <th>Total</th>
                    </tr>
                </thead>
                <tbody>
                    <tr dir-paginate="t in Data.Table4|filter:search|itemsPerPage:10">
                        <td>{{t.MCode}}</td>
                        <td>{{t.MDescription}}</td>
                        <td>{{t.UoM}}</td>
                        <td>{{t.date}}</td>
                        <td>{{t.AvailableQty}}</td>
                        <td>{{t.UnitCost}}</td>
                        <td>{{t.TotalCost}}</td>

                    </tr>
                </tbody>
            </table>
         <%--   <p>Total Cost (KWD): {{Total5}}</p>--%>
               <%--<p><%= GetGlobalResourceObject("Resource", "TotalCostKWD")%>: {{Total5}}</p>
               <p><%= GetGlobalResourceObject("Resource", "TotalCostKWD")%>: {{Total5}}</p>--%>

                        <%-- <p>{{Currency.S}}: {{Total5}}</p>--%>
            <p>Total : {{Total5}}</p>
                        <%-- <p>{{Currency.S}}: {{Total5}}</p>--%>
            <dir-pagination-controls max-size="3" direction-links="true"  boundary-links="true" class="alignright"> </dir-pagination-controls>
        </div>
            </div>
        
        <div id="pdfdiv" class="pdf temp">
     
        </div>
</asp:Content>

