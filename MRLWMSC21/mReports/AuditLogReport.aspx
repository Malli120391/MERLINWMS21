<%@ Page Language="C#" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="AuditLogReport.aspx.cs" Inherits="MRLWMSC21.mReports.AuditLogReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
    <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
    <script src="AuditLogReport.js"></script>
    <link href="Scripts/Custom.css" rel="stylesheet" />
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

        .mytableReportBodyTR tr:nth-child(even) {
            background-color: #fff !important;
        }
    </style>

    <%-- <script type="text/javascript">
         $(document).ready(function () {
             $("#hdnTenantID").val(<%=cp.TenantID%>);
           });
    </script>--%>
    <div class="dashed"></div>
    <div ng-app="myApp" ng-controller="AuditLogReport" class="pagewidth" style="margin: auto;">
        <div class="divlineheight"></div>
        <div class="row">
            <div class="col m6">
                                                <div class="buttonrowstyle">
                                                    <button type="button" id="btnPdf" class="button button3" ng-click="exportPdf()">PDF &nbsp;<i class="fa fa-file-pdf-o" aria-hidden="true"></i></button>
                                                    <button type="button" id="btnExcel" class="button button1" ng-click="exportExcel()">Excel &nbsp;<i class="fa fa-file-excel-o" aria-hidden="true"></i></button>
                                                   <%-- <button type="button" id="btnTxt" class="button button4" ng-click="exportTxt()">Txt &nbsp;<i class="fa fa-file-text-o" aria-hidden="true"></i></button>--%>
                                                    <button type="button" id="btnWord" class="button button2" ng-click="exportWord()">Word &nbsp;<i class="fa fa-file-word-o" aria-hidden="true"></i></button>
                                                    <button type="button" id="btnCsv" class="button button1" ng-click="exportCsv()">CSV &nbsp;<i class="fa fa-file-excel-o" aria-hidden="true"></i></button>
                                                    <%--<button type="button" id="btnXML" class="button button6" ng-click="exportXml()">XML &nbsp;<i class="fa fa-file-text-o" aria-hidden="true"></i></button>--%>
                                                </div>
            </div>
            <div class="col m6">
                <div class="flex__ right">
                    <div>
                        <div class="flex">
                            <input type="text" id="txtTables" ngchange="getTables()" ng-click="getTables()" required="" />
                            <label>Entity</label>
                            <span class="errorMsg"></span>
                        </div>
                        <%--<input type="text" id="txtTenant" placeholder="Tenant" ngchange="getTenant()" ng-click="getTenant()"   class="TextboxInventoryAuto" />&nbsp;&nbsp;
                        <input type="text" id="txtPartnumber" placeholder="Part Number" ngchange="getpartnumber()" ng-click="getpartnumber()"  class="TextboxInventoryAuto" />&nbsp;&nbsp;
                        <input type="text" id="txtMaterialType" placeholder="Material Type" ngchange="getmtype()" ng-click="getmtype()"  class="TextboxInventoryAuto" />&nbsp;&nbsp; 
                           <input type="text" id="txtBatchNo" placeholder="Batch No." class="Textbox"/>&nbsp;&nbsp;
                             <input type="text" id="txtLocation" placeholder="Location" ngchange="getlocation()" ng-click="getlocation()"  class="TextboxInventoryAuto" />&nbsp;&nbsp;
                        <input type="text" id="txtKitId" placeholder="KitId" ngchange="getkitplannerid()" ng-click="getkitplannerid()"  class="TextboxInventoryAuto" />&nbsp;&nbsp;
                             <button type="button" ng-click="Getgedetails()"  style="width: 100px !important; background-color: #455b7c;" class="addbuttonOutbound">View Report <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>--%>
                    </div>&nbsp;&nbsp;
                    <div>
                        <button type="button" ng-click="Getgedetails()" class="btn btn-primary">View Report <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                    </div>
                </div>

            </div>

        </div>
        <div class="row" style="margin: 0;">
            <div class="col-sm-6 col-lg-6" style="margin: 0; padding: 0;">
                <div class="divmainwidth">
                    <table class="table table-striped" id="tbldatas">
                        <thead>
                            <tr class="mytableReportHeaderTR">
                                <th ng-click="sort('Client')">Sr. No<span class="glyphicon sort-icon" ng-show="sortKey=='Client'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('OBDNumber')">Transaction Id<span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('OBDNumber')">Entity Id<span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('OBDNumber')">Table Name<span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('OBDNumber')">Column Name<span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('OBDNumber')">Data Inserted<span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('OBDNumber')">Data Updated<span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('OBDNumber')">Data Deleted<span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('VLPDNumber')">Old Value<span class="glyphicon sort-icon" ng-show="sortKey=='VLPDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('MType')">New Value<span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('MType')">Modified By<span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th ng-click="sort('MType')">Modified On<span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>

                            </tr>
                        </thead>
                        <tbody class="mytableReportBodyTR">
                            <tr dir-paginate="BLR in BIllingReport|orderBy:sortKey:reverse|filter:search|itemsPerPage:10">
                                <td align="center">{{$index +1}}</td>
                                <td align="center">{{BLR.TransactionId}}</td>
                                <td>{{BLR.EntityId}}</td>
                                <td>{{BLR.TableName}}</td>
                                <td>{{BLR.ColumnName}}</td>
                                <td align="center">{{BLR.DataInserted}}</td>
                                <td align="center">{{BLR.DataUpdated}}</td>
                                <td align="center">{{BLR.DataDeletd}}</td>
                                <td align="center">{{BLR.OldValue}}</td>
                                <td align="center">{{BLR.NewValue}}</td>
                                <td align="center">{{BLR.ModifiedBy}}</td>
                                <td align="right">{{BLR.ModifiedOn}}</td>



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
    </div>
</asp:Content>
