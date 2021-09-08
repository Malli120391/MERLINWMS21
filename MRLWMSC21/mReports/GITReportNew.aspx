<%@ Page Title="GIT Report" Language="C#" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="GITReportNew.aspx.cs" Inherits="MRLWMSC21.mReports.GITReportNew" %>

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
    <%--<script src="3PLBillingReportNew.js"></script>--%>
    <link href="Scripts/Custom.css" rel="stylesheet" />
    <script src="GITReportNew.js"></script>
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
    <style>
        #tbldatas {
            width: 95%;
            border: 0px !important;
            margin: 15px auto;
            border-radius: 0px !important;
            background-color: #d1dced !important;
        }

        .thalign {
            text-align: right !important;
            background: #fff;
        }

            .thalign table {
                float: right;
                margin-right: 7px;
            }

        .mytableReportHeaderTR {
            color: #000 !important;
            background-color: #fff !important;
            text-align: justify;
        }

        .module_Green {
            border-right: none !important;
            border-left: none !important;
        }
        .flex input[type="text"]:focus ~ label, input[type="text"]:valid ~ label
        {
            top: -1px !important;

        }
    </style>
    <link href="../Content/app.css" rel="stylesheet" />
    <table class="tbsty" >
        <tbody>
            <tr class="module_yellow">
               <td class="ModuleHeader fixed-width">
                        <div><a href="../Default.aspx">Home</a> / Reports / <span class="FormSubHeading">Inbound / GIT Report</span></div>
                </td>
             </tr>
        </tbody>
    </table>
    <div ng-app="myApp" ng-controller="GITReportNew" class="pagewidth">
        <gap></gap>
        <div class="row m0">
            <div class="col m6">
              
            </div>
            <div class="col m6">
                <div class="flex__ right">
                            <div class="flex">

                                <!-- Globalization Tag is added for multilingual  -->
                                <input type="text" class="" ng-model="fromdate" onpaste="return false" id="txtFromdate" required="" />
                            <%--    <label>From Date</label>--%>
                                    <label> <%= GetGlobalResourceObject("Resource", "FromDate")%></label>
                            </div>&nbsp;&nbsp;
                            <div class="flex">
                                <input type="text" class="" ng-model="todate" onpaste="return false" id="txttodate" required="" />
                                <label>To Date</label>
                            </div>&nbsp;&nbsp;
                           <%-- <button type="button" ng-click="Getgedetails()" class="btn btn-primary">View Report <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>--%>
                     <div><button type="button" ng-click="Getgedetails()" class="btn btn-primary"> <%= GetGlobalResourceObject("Resource", "ViewReport")%> <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button></div>
                    &nbsp;
                            <div class="exportto">
                               <%-- <a href="#" class="btn btn-primary">Export To &nbsp;<i class="material-icons">cloud_download</i></a>--%>
                                 <a href="#" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "ExportTo")%>  &nbsp;<i class="material-icons">cloud_download</i></a>
                                <ul class="export-menu">
                                   <%-- <li><span id="btnPdf" class="buttons button3" ng-click="exportPdf()"><i class="fa fa-file-pdf-o" aria-hidden="true"></i>&nbsp;&nbsp;PDF</span></li>--%>
                                     <%--<li><span id="btnPdf" class="buttons button3" ng-click="exportPdf()"><i class="fa fa-file-pdf-o" aria-hidden="true"></i>&nbsp;&nbsp; <%= GetGlobalResourceObject("Resource", "PDF")%></span></li>--%>
                                    <%--<li><span id="btnExcel" class="buttons button1" ng-click="exportExcel()"><i class="fa fa-file-excel-o" aria-hidden="true"></i>&nbsp;&nbsp;Excel</span></li>--%>
                                    <li><span id="btnExcel" class="buttons button1" ng-click="exportExcel()"><i class="fa fa-file-excel-o" aria-hidden="true"></i>&nbsp;&nbsp;<%= GetGlobalResourceObject("Resource", "Excel")%> </span></li>
                                  <%--  <li><span id="btnTxt" class="buttons button4" ng-click="exportTxt()"><i class="fa fa-file-text-o" aria-hidden="true"></i>&nbsp;&nbsp;Txt</span></li>--%>
                                  <%--  <li><span id="btnWord" class="buttons button2 hidden" ng-click="exportWord()"><i class="fa fa-file-word-o" aria-hidden="true"></i>&nbsp;&nbsp;Word</span></li>--%>
                                      <%--<li><span id="btnWord" class="buttons button2 hidden" ng-click="exportWord()"><i class="fa fa-file-word-o" aria-hidden="true"></i>&nbsp;&nbsp;<%= GetGlobalResourceObject("Resource", "Word")%> </span></li>--%>
                                   <%-- <li><span id="btnCsv" class="buttons button1" ng-click="exportCsv()"><i class="fa fa-file-excel-o" aria-hidden="true"></i>&nbsp;&nbsp;CSV</span></li>--%>
                                    <%-- <li><span id="btnCsv" class="buttons button1" ng-click="exportCsv()"><i class="fa fa-file-excel-o" aria-hidden="true"></i>&nbsp;&nbsp; <%= GetGlobalResourceObject("Resource", "CSV")%></span></li>--%>
                                   <%-- <li><span id="btnXML" class="buttons button6" ng-click="exportXml()"><i class="fa fa-file-text-o" aria-hidden="true"></i>&nbsp;&nbsp;XML</span></li>--%>
                                </ul>
                            </div>
               </div>
            </div>
        </div>
        <div class="row" style="margin: 0;">
            <div class="col-sm-12 col-lg-12" style="margin: 0; padding: 0;">
                <div class="divmainwidth">
                    <table class="table-striped" id="">
                        <thead>
                            <tr class="">
                                <th ng-click="sort('Client')"> <%= GetGlobalResourceObject("Resource", "SrNo")%><span class="glyphicon sort-icon" ng-show="sortKey=='Client'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('OBDNumber')"> <%= GetGlobalResourceObject("Resource", "StoreRefNo")%><span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('VLPDNumber')">  <%= GetGlobalResourceObject("Resource", "DocRcvdDate")%> <span class="glyphicon sort-icon" ng-show="sortKey=='VLPDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('MType')"> <%= GetGlobalResourceObject("Resource", "Tenant")%> <span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('MType')"><%= GetGlobalResourceObject("Resource", "Supplier")%> <span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('MType')"><%= GetGlobalResourceObject("Resource", "BLorAWBNo")%>  <span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('MType')"><%= GetGlobalResourceObject("Resource", "BLorAWBDate")%>  <span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th number ng-click="sort('ItemVolume')"><%= GetGlobalResourceObject("Resource", "InvoiceNo")%>   <span class="glyphicon sort-icon" ng-show="sortKey=='ItemVolume'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('ItemWeight')"><%= GetGlobalResourceObject("Resource", "InvoiceDate")%>  <span class="glyphicon sort-icon" ng-show="sortKey=='ItemWeight'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th number ng-click="sort('ItemWeight')"><%= GetGlobalResourceObject("Resource", "Invoicevalue")%> <span class="glyphicon sort-icon" ng-show="sortKey=='ItemWeight'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('ItemWeight')"><%= GetGlobalResourceObject("Resource", "Currency")%> <span class="glyphicon sort-icon" ng-show="sortKey=='ItemWeight'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                              <%--  <th ng-click="sort('ItemWeight')">Ex. Rate <span class="glyphicon sort-icon" ng-show="sortKey=='ItemWeight'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>--%>


                            </tr>
                        </thead>
                        <tbody class="">
                            <tr dir-paginate="BLR in BIllingReport|orderBy:sortKey:reverse|filter:search|itemsPerPage:25">
                                <td align="center">{{$index +1}}</td>
                                <td>{{BLR.StoreRefNo}}</td>
                                <td>{{BLR.DocRcvdDate}}</td>
                                <td>{{BLR.Tenant}}</td>
                                <td>{{BLR.Supplier}}</td>
                                <td>{{BLR.BLorAWBNO}}</td>
                                <td>{{BLR.BLorAWBDate}}</td>
                                <td number>{{BLR.InvoiceNO}}</td>
                                <td align="right">{{BLR.InvoiceDate}}</td>
                                <td number>{{BLR.Invoicevalue}}</td>
                                <td align="right">{{BLR.Currency}}</td>
                               <%-- <td align="right">{{BLR.ExRate}}</td>--%>
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

                    </table>
                </div>

                <table id="tbldata"></table>
                <div class="divlineheight"></div>
            </div>
        </div>
    </div>
</asp:Content>
