<%@ Page Language="C#" Title="Receipt Confirmation Report" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="ReceiptConfirmationReport.aspx.cs" Inherits="MRLWMSC21.mReports.ReceiptConfirmationReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
   <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
    <script src="ReceiptConfirmationReport.js"></script>
    <link href="Scripts/Custom.css" rel="stylesheet" />

    <script src="Scripts/html2canvas.min.js"></script>

    <script src="Scripts/pdfmake.min.js"></script>


    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>



    <style>
        
        @CHARSET "UTF-8";

    .page-break {
      page-break-after: always;
      page-break-inside: avoid;
      clear:both;
      background-color:#FFF;
    }
    .page-break-before {
      page-break-before: always;
      page-break-inside: avoid;
      clear:both;      background-color:#FFF;
    }
    #html-2-pdfwrapper{
      position: absolute; 
      left: 20px; 
      top: 50px; 
      bottom: 0; 
      overflow: auto; 
      width: 600px;      background-color:#FFF;
    }
        .tableLoader::before {
            height: 392px;
        }
        .set{float:right;}
        .pdf{
           font-size:17px !important;   
        }
        .pdf table thead tr th{font-size:17px !important;border:1px solid #CCC;padding:3px;}
        .pdf table tbody tr td{font-size:17px !important;border:1px solid #CCC;padding:3px;}
        .pdf p {font-size:17px !important}
        .pdf h4 {font-size:17px !important}
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
    <script>
  
    </script>

    <div ng-app="myApp" ng-controller="ReceiptConfirmationReport" class="container">
        <div ng-show="blockUI">
            <div style="width: 100%; height: 100vh; z-index: 999; position: fixed; top: 0; left: 0; right: 0; bottom: 0; align-items: center; display: flex; justify-content: center;">

                <div style="align-self: center;">
                    <img src="<%= ResolveUrl("~/Images/preloader.svg") %>" loader />

                </div>

            </div>

        </div>
        <asp:HiddenField runat="server" ID="hdnTenant" ClientIDMode="Static" Value="0" />
        <asp:HiddenField runat="server" ID="hdnInboundID" ClientIDMode="Static" Value="0" />
        <div class="row">
            <div class="col m4">
                <b>Receipt ID&emsp;&emsp;&emsp;: </b> &emsp;{{HeaderData.StoreRefNo}} <br /><br />
                <b>Document Date&emsp;: </b> &emsp;{{HeaderData.ShipmentReceivedOn}}
            </div>
            <div class="col m3">

            </div>
            <div class="col m5">
                <b>Company Name&emsp;&emsp;: </b> &emsp;{{HeaderData.TenantName}}<br /><br />
                <b>Address&emsp;&emsp;&emsp;&emsp;&emsp;: </b>&emsp;{{HeaderData.Address1}}<br /><br />
                <b>Shipment Type&emsp;&emsp;: </b>&emsp;{{HeaderData.ShipmentType}}<br /><br />
                <b>Truck No./TrallerID&emsp;:</b>&emsp;{{HeaderData.VehicleRegistrationNo}} 
            </div>
        </div>


        <div class="row">
            <div class="col m12" flex end>
                 <asp:LinkButton runat="server" ID="lnkExportData" CssClass="btn btn-primary" OnClick="lnkExportData_Click">Export <i class="fa fa-file-excel-o" aria-hidden="true"></i></asp:LinkButton>
                <button type="button" class="btn btn-primary" style="background-color:#108e05 !important;display:none !important;" ng-click="exportExcel()">Export <i class="fa fa-file-excel-o" aria-hidden="true"></i></button>
                <button type="button" class="btn btn-primary" style="background-color:red !important;" ng-click="exportRCRPDF()">Export <i class="fa fa-file-pdf-o" aria-hidden="true"></i></button>               
            </div>
        </div>
        <div class="row">
            <div class="col m12">
                <table class="table-striped" id="tbldatas">
                    <thead>
                        <tr>
                            <th>Item Code</th>
                            <th ng-show="TenantID == 52">Serial No.</th>
                            <th ng-show="TenantID == 52">PO Number</th>
                            <th ng-show="TenantID == 52">Airway Bill No.</th>

                            <th ng-show="TenantID == 58">Size</th>
                            <th ng-show="TenantID == 58">Collection</th>
                            <th ng-show="TenantID == 58">Description</th>

                            <th ng-show="TenantID != 58 && TenantID != 52">Description</th>
                            <th>UoM</th>
                            <th ng-show="TenantID != 58 && TenantID != 52">Mfg. Date</th>
                            <th ng-show="TenantID != 58 && TenantID != 52">Exp. Date</th>
                            <th>Expected Qty.</th>
                            <th>Received Qty.</th>
                            <th>Good Qty.</th>
                            <th>Damaged Qty.</th>
                            <th>Excess Qty.</th>
                            <th>Short Qty.</th>
                            <th>Volume (CBM)</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr dir-paginate="RPC in ReceiptConfirm | itemsPerPage:25">
                            <td>{{RPC.MCode}}</td>
                            <td ng-show="TenantID == 58 || TenantID == 52">{{RPC.MCodeAlternative1}}</td>
                            <td ng-show="TenantID == 58 || TenantID == 52">{{RPC.MCodeAlternative2}}</td>
                            <td ng-show="TenantID == 58 || TenantID == 52">{{RPC.MDescription}}</td>
                            <td ng-show="TenantID != 58 && TenantID != 52">{{RPC.MDescription}}</td>
                            <td>{{RPC.BUoM}}</td>
                            <td ng-show="TenantID != 58 && TenantID != 52">{{RPC.MfgDate}}</td>
                            <td ng-show="TenantID != 58 && TenantID != 52">{{RPC.ExpDate}}</td>
                            <td>{{RPC.InvoiceQuantity}}</td>
                            <td>{{RPC.ReceivedQty}}</td>
                            <td>{{RPC.GoodQty}}</td>
                            <td>{{RPC.DamagedQty}}</td>
                            <td>{{RPC.ExcessQty}}</td>
                            <td>{{RPC.DiscrepancyQty}}</td>
                            <td>{{RPC.MVolume | number : 6}}</td>
                        </tr>
                    </tbody>
                    <tfoot>
                        <tr>
                            <td colspan="15">
                                <div class="divpaginationstyle">
                                    <dir-pagination-controls direction-links="true" boundary-links="true"> </dir-pagination-controls>
                                </div>
                            </td>
                        </tr>
                    </tfoot>

                </table>
                <div id="divFormat" class="pdf temp">
                
                    </div>
                <table id="tbldata"></table>
            </div>
            <div class="divlineheight"></div>
        </div>
</asp:Content>

