<%@ Page Language="C#" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="StockMovementReportNew.aspx.cs" Inherits="MRLWMSC21.mReports.StockMovementReportNew" %>

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
    <script src="StockMovementReportNew.js"></script>
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>

    <link href="Scripts/Custom.css" rel="stylesheet" />
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

        .mytableReportBodyTR tr:nth-child(even) {
            background-color: #fff !important;
        }
    </style>
   
    <div ng-app="myApp" ng-controller="StockMovementReportNew" class="pagewidth">
        <div class="divlineheight"></div>
        <div class="row">
            <div class="col s1 m4">
            </div>
            <div class="col s11 m8">
                <div class="flex__ right">
                    <div>
                        <div class="flex">
                            <input type="text" ng-model="fromdate" onpaste="return false" id="txtFromdate" required="" />
                           <%-- <label>From Date</label>--%>
                             <label> <%= GetGlobalResourceObject("Resource", "FromDate")%></label>
                        </div>
                    </div>&nbsp;&nbsp;
                    <div>
                        <div class="flex">
                            <input type="text" ng-model="todate" onpaste="return false" id="txttodate" required="" />
                            <%--<label>To Date</label>--%>
                            <label><%= GetGlobalResourceObject("Resource", "ToDate")%> </label>
                        </div>
                    </div>&nbsp;&nbsp;
                    <div>
                        <%--<button type="button" ng-click="Getgedetails()" class="btn btn-primary">View Report <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>--%>
                        <button type="button" ng-click="Getgedetails()" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "ViewReport")%>  <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                    </div>
                    &nbsp;
                            <div class="exportto">
                              <%--  <a href="#" class="btn btn-primary">Export To &nbsp;<i class="material-icons">cloud_download</i></a>--%>
                                  <a href="#" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "ExportTo ")%> &nbsp;<i class="material-icons">cloud_download</i></a>
                                <ul class="export-menu">
                                       <%-- <li><span id="btnPdf" class="buttons button3" ng-click="exportPdf()"><i class="fa fa-file-pdf-o" aria-hidden="true"></i>&nbsp;&nbsp;PDF</span></li>--%>
                                     <%--<li><span id="btnPdf" class="buttons button3" ng-click="exportPdf()"><i class="fa fa-file-pdf-o" aria-hidden="true"></i>&nbsp;&nbsp;<%= GetGlobalResourceObject("Resource", "PDF")%></span></li>--%>
                                   <%-- <li><span id="btnExcel" class="buttons button1" ng-click="exportExcel()"><i class="fa fa-file-excel-o" aria-hidden="true"></i>&nbsp;&nbsp;Excel</span></li>--%>
                                     <li><span id="btnExcel" class="buttons button1" ng-click="exportExcel()"><i class="fa fa-file-excel-o" aria-hidden="true"></i>&nbsp;&nbsp;<%= GetGlobalResourceObject("Resource", "Excel")%> </span></li>
                                  
                                </ul>
                            </div>
                </div>

            </div>

        </div>
        <div class="row" style="margin: 0;">
            <div class="col-sm-6 col-lg-6" style="margin: 0; padding: 0;">
                <div class="divmainwidth">
                    <table class="mytableOutbound table" id="tbldatas">
                        <thead>
                            <tr class="mytableReportHeaderTR">
                                <%--<th ng-click="sort('Client')">Sr. No<span class="glyphicon sort-icon" ng-show="sortKey=='Client'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>--%>
                                <th ng-click="sort('OBDNumber')"><%= GetGlobalResourceObject("Resource", "PartNo")%><span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('VLPDNumber')"><%= GetGlobalResourceObject("Resource", "Description")%> <span class="glyphicon sort-icon" ng-show="sortKey=='VLPDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('MType')"><%= GetGlobalResourceObject("Resource", "TransactionDate")%> <span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('MType')"><%= GetGlobalResourceObject("Resource", "UOM")%><span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th number ng-click="sort('MType')"> <%= GetGlobalResourceObject("Resource", "GoodsInQty")%>  <span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th number ng-click="sort('MType')"><%= GetGlobalResourceObject("Resource", "GoodsOutQty")%>  <span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th number ng-click="sort('ItemVolume')"><%= GetGlobalResourceObject("Resource", "AvailableQty")%> <span class="glyphicon sort-icon" ng-show="sortKey=='ItemVolume'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>



                            </tr>
                        </thead>
                        <tbody class="mytableReportBodyTR">
                            <tr dir-paginate="BLR in BIllingReport|orderBy:sortKey:reverse|filter:search|itemsPerPage:25">
                                <%--<td align="center">{{$index +1}}</td>--%>
                                <td>{{BLR.PartNo}}</td>
                                <td>{{BLR.Description}}</td>
                                <td>{{BLR.TransactionDate}}</td>
                                <td>{{BLR.UOM}}</td>
                                <td number>{{BLR.GoodsInQty}}</td>
                                <td number>{{BLR.GoodsOutQty}}</td>
                                <td number>{{BLR.AvailableQty}}</td>

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

                    </table>
                </div>

                <table id="tbldata"></table>
                <div class="divlineheight"></div>
            </div>
        </div>
    </div>
</asp:Content>

