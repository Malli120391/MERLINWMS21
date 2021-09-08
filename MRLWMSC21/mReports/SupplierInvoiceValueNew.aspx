<%@ Page Title=" Supplier Invoice Value Report :. " Language="C#" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="SupplierInvoiceValueNew.aspx.cs" Inherits="MRLWMSC21.mReports.SupplierInvoiceValueNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
    <script type="text/javascript">
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
        });

    </script>
    <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
    <link href="Scripts/Custom.css" rel="stylesheet" />
    <script src="SupplierInvoiceValueNew.js"></script>
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
    <style>
        .mytableReportHeaderTR {
            color: #000 !important;
            background-color: #fff !important;
            text-align: justify;
        }

        .module_Green {
            border-right: none !important;
            border-left: none !important;
        }
        .mytableReportBodyTR tr:nth-child(even){
            background-color : #fff !important;
        }
    </style>
    <div ng-app="myApp" ng-controller="SupplierInvoiceValueNew" class="container">
     <div class="divlineheight"></div>
        <div class="row">

            <div class="">
                <div class=" ">
                    <div class="col m3 offset-m3">
                        <div class="flex">
                            <input type="text" ng-model="fromdate" onpaste="return false" id="txtFromdate" required="" />

                            <label><%= GetGlobalResourceObject("Resource", "FromDate")%></label>
                        </div>
                    </div>
                    <div class="col m3">
                        <div class="flex">
                            <input type="text" ng-model="todate" onpaste="return false" id="txttodate" required="" />

                            <label><%= GetGlobalResourceObject("Resource", "ToDate")%> </label>
                        </div>
                    </div>
                    <div class="col m3">
                        <gap5></gap5>
                        <button type="button" ng-click="Getgedetails()" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "ViewReport")%> <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                        <a class="btn btn-primary" ng-click="exportExcel()"><%= GetGlobalResourceObject("Resource", "ExportTo")%><i class="fa fa-file-excel-o" aria-hidden="true"></i></a>
                    </div>
                </div>
            </div>
        </div>    
    <div class="row" style="margin: 0;">
            <div class="col-sm-6 col-lg-6" style="margin: 0; padding: 0;" >
                <div class="divmainwidth" >
                <table class="mytableOutbound table-striped" id="tbldatas" >
                    <thead>
                        <tr class="mytableReportHeaderTR">
                            <%--<th ng-click="sort('Client')">Sr. No<span class="glyphicon sort-icon" ng-show="sortKey=='Client'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>--%>
                            <th ng-click="sort('OBDNumber')"> <%= GetGlobalResourceObject("Resource", "StoreRefNo")%><span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('VLPDNumber')"><%= GetGlobalResourceObject("Resource", "ShipmentRcvdOn")%> <span class="glyphicon sort-icon" ng-show="sortKey=='VLPDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('MType')"><%= GetGlobalResourceObject("Resource", "Tenant")%> <span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('MType')"><%= GetGlobalResourceObject("Resource", "Supplier")%> <span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('MType')"><%= GetGlobalResourceObject("Resource", "PONumber")%> <span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('ItemVolume')"><%= GetGlobalResourceObject("Resource", "InvoiceNumber")%> <span class="glyphicon sort-icon" ng-show="sortKey=='ItemVolume'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('ItemWeight')"> <%= GetGlobalResourceObject("Resource", "InvoiceDate")%> <span class="glyphicon sort-icon" ng-show="sortKey=='ItemWeight'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th number ng-click="sort('ItemWeight')">  <%= GetGlobalResourceObject("Resource", "InvoiceValue")%>  <span class="glyphicon sort-icon" ng-show="sortKey=='ItemWeight'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th number ng-click="sort('ItemWeight')">   <%= GetGlobalResourceObject("Resource", "Currency")%><span class="glyphicon sort-icon" ng-show="sortKey=='ItemWeight'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                           <%-- <th ng-click="sort('ItemWeight')"> Ex. Rate <span class="glyphicon sort-icon" ng-show="sortKey=='ItemWeight'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>--%>
                            
                          
                        </tr>
                    </thead>
                    <tbody class="mytableReportBodyTR">
                        <tr dir-paginate="BLR in BIllingReport|orderBy:sortKey:reverse|filter:search|itemsPerPage:25">
                            <td >{{BLR.StoreRefNo}}</td>
                            <td>{{BLR.ShipmentRcvdOn}}</td>
                            <td >{{BLR.Tenant}}</td>
                            <td >{{BLR.Supplier}}</td>
                            <td >{{BLR.PONumber}}</td>
                            <td>{{BLR.InvoiceNo}}</td>
                             <td  align="right">{{BLR.InvoiceDate}}</td>
                            <td  number align="right">{{BLR.Invoicevalue}}</td>
                            <td  number align="right">{{BLR.Currency}}</td>
                            <%--<td  align="right">{{BLR.ExRate}}</td>--%>
                           <%-- <td  align="right">{{BLR.TotalCostAfterDisc}}</td>
                            <td  align="right">{{BLR.TotalCostAfterDiscWithOutTax}}</td>
                            <td  align="right">{{BLR.Tax}}</td>--%>
                          
                        </tr>
                    </tbody>
                    <tfoot>
                        <tr class="mytableReportFooterTR">
                            <td colspan="15">
                                <div class="divpaginationstyle">
                                    <dir-pagination-controls direction-links="true" boundary-links="true"> </dir-pagination-controls>
                                </div>
                            </td>
                        </tr>
                    </tfoot>

                </table></div>

                <table id="tbldata"></table>
                <div class="divlineheight"></div>
            </div>
        </div>
    </div>
</asp:Content>
