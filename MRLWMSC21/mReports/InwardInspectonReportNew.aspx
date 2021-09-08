<%@ Page Language="C#" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="InwardInspectonReportNew.aspx.cs" Inherits="MRLWMSC21.mReports.InwardInspectonReportNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
    <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
    <script src="InwardInspectonReportNew.js"></script>
    <script type="text/javascript" src="Scripts/CommonScripts.js"></script>
    <script type="text/javascript" src="Scripts/jquery.blockUI.js"></script>

    <script src="../Scripts/jquery-1.8.2.min.js"></script>
    <script src="../Scripts/jquery-ui-1.8.24.js"></script>
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>

    <link href="Scripts/Custom.css" rel="stylesheet" />
    <style>
        .module_Green {
             border-width: 3px !important;
        }

        .strtt {margin-bottom: 10px;
                           background-color: #f2fbff !important;
    border-radius: 2px !important;
        }

        .Headertablewidth tr td{
            font-size:14px;
                padding: 5px;
        }
        .green_td {
            border-color: #476089;
            background-color: #fff !important;
        }

        .mytableReportHeaderTR th {
            font-weight: normal;
            font-size: 17px;
            padding: 10px;
        }

        .mytableReportBodyTR td {
            font-size:14.5px;
            text-align: center;
        }
        .mytableReportHeaderTR {
            color: #000 !important;
            background-color: #fff !important;
            text-align: justify;
        }
        .module_Green {
            border-right:none !important;
            border-left:none !important;
        }

        .mytableReportBodyTR tr:nth-child(even) {
            background-color: #506fa14f;
            height: 25px;
        }

        .mytableReportBodyTR tr:nth-child(odd) {
            background-color: #ffffff;
            height: 25px;
        }
        .table-striped th table tbody tr td {
                 border-bottom: 1px solid !important;
    border-right: 1px solid !important;
    border-color: #2d2b2b !important;
    padding: 7px !important;
        }



    </style>
    <div class="dashed"></div>
    <div ng-app="myApp" ng-controller="InwardInspectonReportNew" class="pagewidth">
        <div class="divlineheight"></div>
        <div class="pull-right">
            <table class="Headertablewidth">
                <tr>
                    <td align="right">
                        <%-- Inspection Report No. : --%>
                        <div class="flex__">
                        <div class="flex">
                        <input type="text" id="txtstoreRefNo" ngchange="getStoreRefNo()" ng-click="getStoreRefNo()" required="" /> 
                        <label>Store Reference No.</label>
                            </div>
                         &nbsp;&nbsp;&nbsp;&nbsp;<button type="button" ng-click="Getgedetails()" class="btn btn-primary">Search <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                            
                            </div>
                    </td>

                </tr>

            </table>

        </div>
        <div class="row" style="margin: 0;">
            <div class="col-sm-6 col-lg-6" style="margin: 0; padding: 0;">
                <div class="divmainwidth">


                    <table class="table" id="tbldatas">
                        <thead>
                            <tr class="mytableReportItemsHeaderTR">
                                <th colspan="15" class="thalign">
                                    <table class="Headertablewidth">
                                        <tr>
                                            <td>
                                                <%--<div style="padding-top:0px" align="left"><img id="Img3" runat="server" enableviewstate="false" src="~/Images/Logo_Header_falcon2.png" visible="true" border="0" alt=""></div>--%>
                                                <table class="Headertablewidth strtt" border="1" id="tbldata1">
                                                    <tr>
                                                        <div align="left">
                                                            <td align="left" colspan="10">RT/QC/T/IIR ver 1.0 Dt :   {{today | date :  "dd.MM.y"}}
                                                            
                                                            </td>

                                                        </div>

                                                    </tr>
                                                    <tr>
                                                        <td align="left">Inspection report No:

                                                        </td>
                                                        <td align="center">
                                                            <label id="lblStorerefenceNo"></label>
                                                        </td>

                                                        <td>Date
                                                        </td>
                                                        <td style="width:100px;"></td>
                                                        <td>MRN No:

                                                        </td>
                                                        <%--<td></td>--%>
                                                        <td colspan="1" style="width:100px;"></td>

                                                        <td style="width:100px;"></td>

                                                        <td align="right">Date
                                                        </td>
                                                        <td style="width:100px;" colspan="2"></td>
                                                    </tr>

                                                    <tr>
                                                        <td align="left">Material Type
                         
                                                        </td>
                                                        <td align="center">Purchased/Customer supplied

                                                        </td><td colspan="2" rowspan="4"></td>
                                                        <td colspan="5">Supplier/Customer name

                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">COC
                                                        </td>
                                                        <td align="center">Received/Not applicable

                                                        </td><%--<td colspan="2"></td>--%>
                                                        <td colspan="5">Supplier Status

                                                        </td>
                                                        <td align="center">Approved/Not Approved

                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td align="left">Test Certificate

                                                        </td>
                                                        <td align="center">Received/Not applicable

                                                        </td><%--<td colspan="2"></td>--%>
                                                        <td colspan="5">Other reports if any (specify)

                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">Product under positive recall

                                                        </td>
                                                        <td align="center">Yes/No

                                                        </td><%--<td colspan="2"></td>--%>
                                                        <td colspan="5">If Yes, Report No:

                                                        </td>
                                                        <td></td>
                                                    </tr>




                                                </table>
                                                <div class="buttonrowstyle pull-right">
                                                    <button type="button" id="btnPdf" class="button button3" ng-click="exportPdf()">PDF &nbsp;<i class="fa fa-file-pdf-o" aria-hidden="true"></i></button>
                                                    <button type="button" id="btnExcel" class="button button1" ng-click="exportExcel()">Excel &nbsp;<i class="fa fa-file-excel-o" aria-hidden="true"></i></button>
                                                   <%-- <button type="button" id="btnTxt" class="button button4" ng-click="exportTxt()">Txt &nbsp;<i class="fa fa-file-text-o" aria-hidden="true"></i></button>--%>
                                                    <button type="button" id="btnWord" class="button button2" ng-click="exportWord()">Word &nbsp;<i class="fa fa-file-word-o" aria-hidden="true"></i></button>
                                                    <button type="button" id="btnCsv" class="button button1" ng-click="exportCsv()">CSV &nbsp;<i class="fa fa-file-excel-o" aria-hidden="true"></i></button>
                                                    <%--<button type="button" id="btnXML" class="button button6" ng-click="exportXml()">XML &nbsp;<i class="fa fa-file-text-o" aria-hidden="true"></i></button>--%>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </th>
                            </tr>
                            <tr class="mytableReportHeaderTR">
                                <th ng-click="sort('Client')">Sr. No<span class="glyphicon sort-icon" ng-show="sortKey=='Client'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('OBDNumber')">Part No.<span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('VLPDNumber')">Description<span class="glyphicon sort-icon" ng-show="sortKey=='VLPDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('MType')">Received Qty.<span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('MType')">Accepted Qty.<span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('MType')">Rejected Qty. <span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('MType')">Sample Size <span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('ItemVolume')">Parameter Checked <span class="glyphicon sort-icon" ng-show="sortKey=='ItemVolume'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('ItemWeight')">Test Method<span class="glyphicon sort-icon" ng-show="sortKey=='ItemWeight'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('ItemWeight')">UoM <span class="glyphicon sort-icon" ng-show="sortKey=='ItemWeight'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('ItemWeight')">Specification with Tolerance <span class="glyphicon sort-icon" ng-show="sortKey=='ItemWeight'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('ItemWeight')">Observation <span class="glyphicon sort-icon" ng-show="sortKey=='ItemWeight'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('ItemWeight')">Remarks <span class="glyphicon sort-icon" ng-show="sortKey=='ItemWeight'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <%-- <th ng-click="sort('ItemWeight')"> Nov <span class="glyphicon sort-icon" ng-show="sortKey=='ItemWeight'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('ItemWeight')"> Dec <span class="glyphicon sort-icon" ng-show="sortKey=='ItemWeight'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>--%>
                            </tr>
                        </thead>
                        <tbody class="mytableReportBodyTR">
                            <tr dir-paginate="BLR in BIllingReport|orderBy:sortKey:reverse|filter:search|itemsPerPage:10">
                                <td align="center">{{$index +1}}</td>
                                <td>{{BLR.PartNo}}</td>
                                <td>{{BLR.Description}}</td>
                                <td>{{BLR.ReceivedQty}}</td>
                                <td>{{BLR.AcceptedQty}}</td>
                                <td>{{BLR.RejectedQty}}</td>
                                <td>{{BLR.SampleSize}}</td>
                                <td>{{BLR.Parameter}}</td>
                                <td>{{BLR.Test}}</td>
                                <td>{{BLR.UoM}}</td>
                                <td>{{BLR.Specification}}</td>
                                <td>{{BLR.Observation}}</td>
                                <td>{{BLR.Remarks}}</td>
                                <%--<td  align="right">{{BLR.Dec}}</td>--%>
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
            <div class="divlineheight"></div>

        </div>
</asp:Content>
