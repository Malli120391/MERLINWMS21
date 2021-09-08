<%@ Page Language="C#" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="MaterialActivityReportNew.aspx.cs" Inherits="MRLWMSC21.mReports.MaterialActivityReportNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
    <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
    <script src="MaterialActivityReportNew.js"></script>

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
    <div ng-app="myApp" ng-controller="MaterialActivityReportNew" class="pagewidth">
        <div class="divlineheight"></div>
        <input type="hidden" id="hdnTenantID" />
        <div class="row">
            <div class="col m4">
            </div>
            <div class="col m8">
                <div class="flex__ right">
                   
                        <div class="flex">
                            <input type="text" id="txtPartNo" ngchange="getskus()" ng-click="getskus()" required="" />
                            <label>Part No</label>
                        </div>
                    &nbsp;&nbsp;
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
        <%------------------------- remove unnecessary code by durga on 21/05/2018 ---------------------------------%>
        <div class="row" style="margin: 0;">
            <div class="col-sm-6 col-lg-6" style="margin: 0; padding: 0;">
                <div class="divmainwidth">
                    <table class="mytableOutbound" id="tbldatas">
                        <thead>

                            <tr ng-if="inwardhide" class="mytableReportHeaderTR">
                                <th colspan="10">Inward Activity</th>

                            </tr>
                            <tr class="mytableReportHeaderTR" ng-if="inwardhide">

                                <th>Trans. Date</th>
                                <th>Supplier</th>
                                <th>PO Number</th>
                                <th>Store Ref. No.</th>
                                <th>UoM/Qty.</th>
                                <th>Rcvd. Quantity</th>
                                <th>Location </th>
                                <th>Mfg. Date</th>
                                <th>Serial No. </th>
                                <th>Batch No. </th>

                            </tr>

                        </thead>
                        <tbody class="mytableReportBodyTR" ng-if="inwardhide">
                            <tr dir-paginate="BLR in BIllingReportIn|orderBy:sortKey:reverse|filter:search|itemsPerPage:25" pagination-id="MainTable">

                                <td>{{BLR.TransDate}}</td>
                                <td>{{BLR.Supplier}}</td>
                                <td>{{BLR.PONumber}}</td>
                                <td>{{BLR.StoreRefNo}}</td>
                                <td>{{BLR.UoM}}</td>
                                <td>{{BLR.ReceivedQty}}</td>
                                <td>{{BLR.Location}}</td>
                                <td>{{BLR.MfgDate}}</td>
                                <td>{{BLR.SerialNo}}</td>
                                <td>{{BLR.BatchNo}}</td>


                            </tr>
                            <tr ng-if="inwardhide">
                                <td colspan="10">
                                    <div class="divpaginationstyle">
                                        <dir-pagination-controls direction-links="true" boundary-links="true" pagination-id="MainTable"> </dir-pagination-controls>
                                    </div>
                                </td>
                            </tr>

                        </tbody>


                        <tr ng-if="outwardhide" class="mytableReportHeaderTR">
                            <th colspan="10">Outward Activity</th>
                        </tr>
                        <tr class="mytableReportHeaderTR" ng-if="outwardhide">


                            <th ng-click="sort('OBDNumber')">Trans. Date<span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('VLPDNumber')">Customer<span class="glyphicon sort-icon" ng-show="sortKey=='VLPDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('MType')">SO Number<span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('MType')">OBD Number<span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('MType')">UoM/Qty.<span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('MType')">Picked Quantity<span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('ItemVolume')">Location <span class="glyphicon sort-icon" ng-show="sortKey=='ItemVolume'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('ItemVolume')">Exp. Date <span class="glyphicon sort-icon" ng-show="sortKey=='ItemVolume'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('ItemVolume')">Serial No. <span class="glyphicon sort-icon" ng-show="sortKey=='ItemVolume'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('ItemVolume')">Batch No. <span class="glyphicon sort-icon" ng-show="sortKey=='ItemVolume'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>

                        </tr>
                        <tbody class="mytableReportBodyTR" ng-if="outwardhide">
                            <tr dir-paginate="BLR in BIllingReportOut|orderBy:sortKey:reverse|filter:search|itemsPerPage:10" pagination-id="owdPagination">

                                <td>{{BLR.TransDate}}</td>
                                <td>{{BLR.Customer}}</td>
                                <td>{{BLR.SONumber}}</td>
                                <td>{{BLR.OBDNo}}</td>
                                <td>{{BLR.UoM}}</td>
                                <td>{{BLR.PickedQty}}</td>
                                <td>{{BLR.Location}}</td>
                                <td>{{BLR.ExpDate}}</td>
                                <td>{{BLR.SerialNo}}</td>
                                <td>{{BLR.BatchNo}}</td>

                            </tr>
                            <tr ng-if="outwardhide">
                                <td colspan="10">
                                    <div class="divpaginationstyle">
                                        <dir-pagination-controls direction-links="true" boundary-links="true" pagination-id="owdPagination"> </dir-pagination-controls>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                      <%--  <tfoot>
                            <tr class="mytableReportFooterTR">
                                <td colspan="15">
                                    <div class="divpaginationstyle">
                                        <dir-pagination-controls direction-links="true" boundary-links="true"> </dir-pagination-controls>
                                    </div>
                                </td>
                            </tr>
                        </tfoot>--%>

                    </table>
                </div>

                <table id="tbldataIn"></table>
                <div class="divlineheight"></div>
                <table id="tbldataOut"></table>
            </div>
        </div>
    </div>



</asp:Content>
