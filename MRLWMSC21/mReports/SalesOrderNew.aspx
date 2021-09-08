<%@ Page Title="  Sales Order Report :. " Language="C#" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="SalesOrderNew.aspx.cs" Inherits="MRLWMSC21.mReports.SalesOrderNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
     <script type="text/javascript">
         $(document).ready(function () {
             
             $("#txtSODate").datepicker({
                 dateFormat: "dd-M-yy",
                 maxDate: new Date(),
                 onSelect: function (selected) {
                     $(this).focus();
                     $("#txttodate").datepicker("option", "minDate", selected, { dateFormate: "dd-M-yy" })
                 }
             });
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

                 $('#txtSODate,#txtFromdate, #txttodate').keypress(function () {
                     return false;
                 });
             });
         });


    </script>
    
<script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>     
    <link href="Scripts/Custom.css" rel="stylesheet" />
    <script src="SalesOrderNew.js"></script>
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

        
        .flex input[type="text"], input[type="number"], textarea {
            width:100% !important;
        }
    </style>

    <div ng-app="myApp" ng-controller="SalesOrderNew" class="pagewidth">

         <div class="divlineheight"></div><input type="hidden" id="hdnTenantID" />
         <div>
            <div class="row mo">
                    <div class="col m3">
                        <div class="flex">
                            <input type="text" id="txtSoNo" ngchange="getskus()" ng-click="getskus()"  required="" />    
                           <%--     <label>SO Number</label>--%>
                                 <label> <%= GetGlobalResourceObject("Resource", "SONumber")%></label>
                        </div>
                    </div>
                
                    <div class="col m3">
                        <div>
                            <div class="flex">
                            <input type="text"   ng-model="fromdate" onpaste="return false" id="txtSODate" required=""/>
                           <%-- <label>SO Date</label>--%>
                                 <label><%= GetGlobalResourceObject("Resource", "SODate")%></label>
                            </div>
                        </div>
                    </div>
                
                    <div class="col m3">
                            <div class="flex">
                            <input type="text" id="txtSOStatus" ngchange="getskus1()" ng-click="getskus1()" required="" />
                          <%--  <label>SO Status</label>--%>
                                  <label><%= GetGlobalResourceObject("Resource", "SOStatus")%> </label>
                            </div>
                        </div>

                     <div class="col m3">
                            <div class="flex">
                            <input type="text" id="txtSOType" ngchange="getskus2()" ng-click="getskus2()" required="" />
                          <%--  <label>SO Type</label>--%>
                                  <label><%= GetGlobalResourceObject("Resource", "SOType")%></label>
                            </div>
                        </div>
               </div>
             <div class="row m0">
                 <div class="col m3">
                     <div class="flex">
                         <input type="text" ng-model="fromdate" id="txtFromdate" onpaste="return false" required="" />
                        <%-- <label>From Date</label>--%>
                          <label><%= GetGlobalResourceObject("Resource", "FromDate")%> </label>
                     </div>
                 </div>

                 <div class="col m3">
                     <div class="flex">
                         <input type="text" ng-model="todate" id="txttodate" onpaste="return false" required="" />
                       <%--  <label>To Date</label>--%>
                           <label><%= GetGlobalResourceObject("Resource", "ToDate")%> </label>
                     </div>
                 </div><div class="col m3"></div>
                  <div class="col m3">
                      <br />
                      <div class="flex__ end">
                    <%-- <button type="button" ng-click="Getgedetails()" class="btn btn-primary">View Report <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>--%>
                           <button type="button" ng-click="Getgedetails()" class="btn btn-primary"> <%= GetGlobalResourceObject("Resource", "ViewReport")%> <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>&nbsp;&nbsp;
                                <div class="exportto">
                                  <%--  <a href="#" class="btn btn-primary">Export To &nbsp;<i class="material-icons">cloud_download</i></a>--%>
                                      <a href="#" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "ExportTo")%>&nbsp;<i class="material-icons">cloud_download</i></a>
                                    <ul class="export-menu">
                                         <%-- <li><span id="btnPdf" class="buttons button3" ng-click="exportPdf()"><i class="fa fa-file-pdf-o" aria-hidden="true"></i>&nbsp;&nbsp;PDF</span></li>--%>
                                     <%--<li><span id="btnPdf" class="buttons button3" ng-click="exportPdf()"><i class="fa fa-file-pdf-o" aria-hidden="true"></i>&nbsp;&nbsp;<%= GetGlobalResourceObject("Resource", "PDF")%></span></li>--%>
                                   <%-- <li><span id="btnExcel" class="buttons button1" ng-click="exportExcel()"><i class="fa fa-file-excel-o" aria-hidden="true"></i>&nbsp;&nbsp;Excel</span></li>--%>
                                     <li><span id="btnExcel" class="buttons button1" ng-click="exportExcel()"><i class="fa fa-file-excel-o" aria-hidden="true"></i>&nbsp;&nbsp;<%= GetGlobalResourceObject("Resource", "Excel")%> </span></li>
                                  
                                    </ul>
                                </div></div>
                      </div>

             </div>
        <%--             <div class="row">
                                            <div class="buttonrowstyle right">
                                                <button type="button" id="btnPdf" class="button button3" ng-click="exportPdf()">PDF &nbsp;<i class="fa fa-file-pdf-o" aria-hidden="true"></i></button>
                                                <button type="button" id="btnExcel" class="button button1" ng-click="exportExcel()">Excel &nbsp;<i class="fa fa-file-excel-o" aria-hidden="true"></i></button>
                                               <button type="button" id="btnTxt" class="button button4" ng-click="exportTxt()">Txt &nbsp;<i class="fa fa-file-text-o" aria-hidden="true"></i></button>
                                                <button type="button" id="btnWord" class="button button2" ng-click="exportWord()">Word &nbsp;<i class="fa fa-file-word-o" aria-hidden="true"></i></button>
                                                <button type="button" id="btnCsv" class="button button1" ng-click="exportCsv()">CSV &nbsp;<i class="fa fa-file-excel-o" aria-hidden="true"></i></button>
                                               <button type="button" id="btnXML" class="button button6" ng-click="exportXml()">XML &nbsp;<i class="fa fa-file-text-o" aria-hidden="true"></i></button>
                                            </div>
            </div>--%>
        </div>
           <div class="row" style="margin: 0;">
            <div class="col-sm-6 col-lg-6" style="margin: 0; padding: 0;" >
                <div class="divmainwidth" >
                <table class=" table-striped" id="tbldatas" >
                    <thead>
                        <tr class="">
                            <th ng-click="sort('Client')"><%= GetGlobalResourceObject("Resource", "SONumber")%><span class="glyphicon sort-icon" ng-show="sortKey=='Client'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('Client')"><%= GetGlobalResourceObject("Resource", "SODate")%><span class="glyphicon sort-icon" ng-show="sortKey=='Client'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('OBDNumber')"><%= GetGlobalResourceObject("Resource", "Customer")%> <span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('OBDNumber')"><%= GetGlobalResourceObject("Resource", "SOType")%> <span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                           <%-- <th ng-click="sort('MType')"><%= GetGlobalResourceObject("Resource", "Currency")%> <span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('VLPDNumber')"><%= GetGlobalResourceObject("Resource", "GrossValue")%>  <span class="glyphicon sort-icon" ng-show="sortKey=='VLPDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th number ng-click="sort('MType')"><%= GetGlobalResourceObject("Resource", "NetValue")%> <span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('MType')"><%= GetGlobalResourceObject("Resource", "Tax")%><span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>--%>
                            <th ng-click="sort('MType')"><%= GetGlobalResourceObject("Resource", "Status")%> <span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            
                            
                          
                        </tr>
                    </thead>
                    <tbody class="">
                        <tr dir-paginate="BLR in BIllingReport|orderBy:sortKey:reverse|filter:search|itemsPerPage:25">
                             <td>{{BLR.SONumber}}</td>
                            <td >{{BLR.SODate}}</td>
                            <td>{{BLR.Customer}}</td>
                            <td >{{BLR.SOType}}</td>
                           <%-- <td >{{BLR.Currency}}</td>
                            <td >{{BLR.GrossValue}}</td>                            
                            <td number>{{BLR.NetValue}}</td>
                            <td>{{BLR.Tax}}</td>--%>
                             <td>{{BLR.Status}}</td>
                            
                            
                          
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
               <%-- <td class="lineheight"></td>--%>
            </div>
        </div>
    </div>
</asp:Content>
