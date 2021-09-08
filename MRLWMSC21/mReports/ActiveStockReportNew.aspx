<%@ Page Language="C#" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="ActiveStockReportNew.aspx.cs" Inherits="MRLWMSC21.mReports.ActiveStockReportNew" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">

    <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
    <script src="ActiveStockReportNew.js"></script>
    <link href="Scripts/Custom.css" rel="stylesheet" />
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#hdnTenantID").val(<%=cp.TenantID%>);
        });
    </script>

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

    <div ng-app="myApp" ng-controller="ActiveStockReportNew" class="pagewidth" style="margin: auto;">
        <div class="divlineheight"></div>
        <input type="hidden" id="hdnTenantID" />
        <div class="row">
            <div class="col m4">
            </div>
            <div class="col m8">
                <div class="flex__ right">
                    <div>
                        <div class="flex">
                            <input type="text" id="txtPartnumber" ngchange="getskus()" ng-click="getskus()" required="" />
                            <label>Part Number</label>
                        </div>
                        <%--<input type="text" placeholder="Material Type" class="TextboxInventoryAuto" style="width:120PX;"  ng-model="fromdate" id="txtMaterialType  " />
                             <input type="text" placeholder="Location" class="TextboxInventoryAuto" style="width:120PX;"  ng-model="todate" id="txtLocation" /> &nbsp;&nbsp;--%>
                    </div>&nbsp;&nbsp;
                    <div>
                        <div class="flex">
                            <input type="text" id="txtMaterialType" ngchange="getskus1()" ng-click="getskus1()" required="" />
                            <label>Material Type</label>
                        </div>
                    </div>&nbsp;&nbsp;
                    <div>
                        <div class="flex">
                            <input type="text" id="txtLocation" ngchange="getlocation()" ng-click="getlocation()" required="" />
                            <label>Location</label>
                        </div>
                    </div>&nbsp;&nbsp;
                    <div>
                        <button type="button" ng-click="Getgedetails()" class="btn btn-primary">View Report <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                    </div>&nbsp;
                    
                            <div class="exportto">
                                <a href="#" class="btn btn-primary">Export To &nbsp;<i class="material-icons">cloud_download</i></a>
                                <ul class="export-menu">
                                    <li><span id="btnPdf" class="buttons button3" ng-click="exportPdf()"><i class="fa fa-file-pdf-o" aria-hidden="true"></i>&nbsp;&nbsp;PDF</span></li>
                                    <li><span id="btnExcel" class="buttons button1" ng-click="exportExcel()"><i class="fa fa-file-excel-o" aria-hidden="true"></i>&nbsp;&nbsp;Excel</span></li>
                                    <li><span id="btnTxt" class="buttons button4" ng-click="exportTxt()"><i class="fa fa-file-text-o" aria-hidden="true"></i>&nbsp;&nbsp;Txt</span></li>
                                    <li><span id="btnWord" class="buttons button2 hidden" ng-click="exportWord()"><i class="fa fa-file-word-o" aria-hidden="true"></i>&nbsp;&nbsp;Word</span></li>
                                    <li><span id="btnCsv" class="buttons button1" ng-click="exportCsv()"><i class="fa fa-file-excel-o" aria-hidden="true"></i>&nbsp;&nbsp;CSV</span></li>
                                    <li><span id="btnXML" class="buttons button6" ng-click="exportXml()"><i class="fa fa-file-text-o" aria-hidden="true"></i>&nbsp;&nbsp;XML</span></li>
                                </ul>
                            </div>
                </div>
            </div>
        </div>
        <div class="row" style="margin: 0;">
            <div class="col-sm-6 col-lg-6" style="margin: 0; padding: 0;">
                <div class="divmainwidth">
                    <table class="table" id="tbldatas">
                        <thead>
                            <tr class="mytableReportHeaderTR">
                                <th ng-click="sort('Client')">Part No.<span class="glyphicon sort-icon" ng-show="sortKey=='Client'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('OBDNumber')">Description<span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('OBDNumber')">UoM/Qty.<span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('OBDNumber')">Location<span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('VLPDNumber')">Kit ID<span class="glyphicon sort-icon" ng-show="sortKey=='VLPDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('MType')">In-OH<span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('MType')">Ob-OH<span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('MType')">Avl. Qty.<span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                               <%-- <th ng-click="sort('MType')">Batch No.<span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('MType')">Exp. Date<span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('MType')">Mfg. Date<span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('MType')">Plant<span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('MType')">Serial No.<span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('MType')">Stock Type<span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>--%>

                            </tr>
                        </thead>
                        <tbody class="mytableReportBodyTR">
                            <tr dir-paginate="BLR in BIllingReport|orderBy:sortKey:reverse|filter:search|itemsPerPage:25">
                                <td align="center">{{BLR.PartNo}}</td>
                                <td>{{BLR.Description}}</td>
                                <td>{{BLR.UoM}}</td>
                                <td>{{BLR.Location}}</td>
                                <td>{{BLR.KitID}}</td>
                                <td>{{BLR.InOH}}</td>
                                <td>{{BLR.ObOH}}</td>
                                <td>{{BLR.AvlQty}}</td>
                               <%-- <td>{{BLR.BatchNo}}</td>
                                <td>{{BLR.ExpDate}}</td>
                                <td>{{BLR.MfgDate}}</td>
                                <td>{{BLR.Plant}}</td>
                                <td>{{BLR.SerialNO}}</td>
                                <td>{{BLR.StockType}}</td>--%>

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
            </div>
        </div>
    </div>
</asp:Content>
