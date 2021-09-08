<%@ Page Language="C#" Title="Receipt Pending Report" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="ReceiptPendingReport.aspx.cs" Inherits="MRLWMSC21.mReports.ReceiptPendingReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
   <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
    <link href="Scripts/Custom.css" rel="stylesheet" />
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
    <script src="ReceiptPendingReport.js"></script>

    <style>
        .tableLoader::before {
            height: 392px;
        }
    </style>

    <div ng-app="myApp" ng-controller="ReceiptPendingReport" class="container">

        <div class="row">
            <div class="col m3">
                <div class="flex">
                    <input type="text" id="txtWarehouse" required="" />
                    <label>Warehouse</label>
                     <span class="errorMsg"></span>
                </div>
            </div>
            <div class="col m3">
                <div class="flex">
                    <input type="text" id="txtTenant" required="" />
                    <label>Tenant</label>
                    <span class="errorMsg"></span>
                </div>
            </div>
            
            <div class="col m3">
                <div class="flex">
                    <input type="text" id="txtStoreRefNo" required="" />
                    <label>Receipt Ref. No.</label>
                    <span class="errorMsg"></span>
                    <input type="hidden" id="hdnReceiptID" />
                </div>
            </div>
            <div class="col m3">
                <gap5></gap5>
                <button type="button" ng-click="getReceiptPendingReport(1)" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Search")%>  <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                <button type="button" class="btn btn-primary" ng-click="exportExcel()">Export <i class="fa fa-file-excel-o" aria-hidden="true"></i></button>
            </div>

        </div>
      
       <%-- -------------------------- Remove Unneccesary code from table by durga on 21/05/2018------------------------%>
        <div class="row">
            <div class="col m12">
                <div class="divmainwidth">
                    <table class="table-striped" id="tbldatas">
                        <thead>

                            <tr>
                                <th>Receipt Ref. No.</th>                                
                                <th>PO Number</th>
                                <th>Invoice Number</th>
                                <th>MCode</th>
                                <th>Line Number</th>
                                <th>Expected Qty.</th>                                
                                <th>Received Qty.</th>
                                <th>Pending Qty.</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr dir-paginate="RPR in ReceiptPending|itemsPerPage:25" total-items="Totalrecords">
                                <td>{{RPR.StoreRefNo}}</td>
                                <td>{{RPR.PONumber}}</td>
                                <td>{{RPR.InvoiceNumber}}</td>
                                <td>{{RPR.MCode}}</td>
                                <td>{{RPR.LineNumber}}</td>
                                <td>{{RPR.InvoiceQuantity}}</td>
                                <td>{{RPR.ReceivedQty}}</td>
                                <td>{{RPR.PendingQty}}</td>                               
                            </tr>
                        </tbody>
                         <tfoot>
                            <tr>
                                <td colspan="15">
                                    <div class="divpaginationstyle">
                                        <dir-pagination-controls direction-links="true" boundary-links="true" on-page-change="getReceiptPendingReport(newPageNumber)"> </dir-pagination-controls>
                                    </div>
                                </td>
                            </tr>
                        </tfoot>

                    </table>
                </div>

                <table id="tbldata"></table>
            </div>
            <div class="divlineheight"></div>

        </div>
</asp:Content>

