<%@ Page Language="C#" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="NonConfirmityReportNew.aspx.cs" Inherits="MRLWMSC21.mReports.NonConfirmityReportNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">

    <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
    <script src="NonConfirmityReportNew.js"></script>
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
    </style>
    <div class="pagewidth" ng-app="myApp" ng-controller="NonConfirmityReportNew">
        <div class="divlineheight"></div>
        <input type="hidden" id="hdnTenantID" />
        <div class="row">
            <div class="col m6">
            </div>
            <div class="col m6">
                <div class="Headertablewidth ">
                    <div class="flex__ right">
                        <div>
                            <div class="flex">
                                <input type="text" id="txtPONumber" ngchange="getskus()" ng-click="getskus()" required="" />
                                <label>PO Number</label>
                            </div>
                        </div>
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
        </div>
        <div class="row" style="margin:auto;">
            <div class="col-sm-6 col-lg-6" style="margin: 0; padding: 0;">
                <div class="divmainwidth">
                    <table class="mytableOutbound table" id="tbldatas">
                        <thead>
                            <tr class="mytableReportHeaderTR">
                                <th ng-click="sort('OBDNumber')">Line No.<span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('OBDNumber')">Part Number<span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('OBDNumber')">Quantity<span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('VLPDNumber')">Status <span class="glyphicon sort-icon" ng-show="sortKey=='VLPDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('MType')">QC Parameters<span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('MType')">Actual QC Parameters<span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>

                            </tr>
                        </thead>
                        <tbody class="mytableReportBodyTR">
                            <tr dir-paginate="BLR in BIllingReport|orderBy:sortKey:reverse|filter:search|itemsPerPage:25">
                                <td>{{BLR.LineNo}}</td>
                                <td>{{BLR.PartNumber}}</td>
                                <td>{{BLR.Quantity}}</td>
                                <td>{{BLR.Status}}</td>
                                <td>{{BLR.QCParameters}}</td>
                                <td>{{BLR.ActualQCParameters}}</td>


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
                <div class="lineheight"></div>

                <table id="tbldata"></table>
            </div>
        </div>
    </div>




    </div>
</asp:Content>
