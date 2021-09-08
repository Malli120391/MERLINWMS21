<%@ Page Language="C#" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="RateCardReport.aspx.cs" Inherits="MRLWMSC21.mReports.RateCardReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
     <script type="text/javascript">
        $(document).ready(function () {
            $("#txtFromdate").datepicker({
                dateFormat: "dd/mm/yy",
                maxDate: new Date(),
                onSelect: function (selected) {
                    $(this).focus();
                    $("#txtTodate").datepicker("option", "minDate", selected, { dateFormate: "dd/mm/yy" })
                }
            });
            $("#txtTodate").datepicker({
                dateFormat: "dd/mm/yy",
                maxDate: new Date(),
                onSelect: function (selected) {
                    $(this).focus();
                }
            });

            $('#txtFromdate, #txtTodate').keypress(function () {
                return false;
            });
        });

    </script>

    <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
    <script src="RateCardReport.js"></script>
    <link href="Scripts/Custom.css" rel="stylesheet" />
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>

     <%-- <script type="text/javascript">
         $(document).ready(function () {
             $("#hdnTenantID").val(<%=cp.TenantID%>);
           });
    </script>--%>
    <div class="dashed"></div>
<div class="pagewidth">
     <div ng-app="myApp" ng-controller="RateCard">
         <div class="divlineheight"></div>
         <div>
            <table class="Headertablewidth" style="width: 99%;">
                <tr>
                    <td> <b>Tenant:</b><br />
                        <input type="text" id="txtTenant" placeholder="Tenant" ngchange="getTables()" ng-click="getTables()"   class="TextboxInventoryAuto" />&nbsp;&nbsp;
                       </td>
                    <td>
                        <b>From Date :</b><br />
                        <input type="text" placeholder="From Date" class="TextboxInventory"  ng-model="fromdate" id="txtFromdate" />&nbsp;&nbsp;
                    </td>
                    <td> 
                        <b>To Date :</b><br />
                        <input type="text" placeholder="To Date" class="TextboxInventory"  ng-model="fromdate" id="txtTodate" />&nbsp;&nbsp;</td>
                    <td>
                         <button type="button" ng-click="Getgedetails()"  style="width: 100px !important; background-color: #455b7c;" class="addbuttonOutbound">View Report <%=MRLWMSC21Common.CommonLogic.btnfaSearch %>
                    </td>
                        
                    </tr>
                
            </table>   
                
         </div>
         <div class="divlineheight"></div>
          <div class="row" style="margin: 0;">
            <div class="col-sm-6 col-lg-6" style="margin: 0; padding: 0;" >
                <div class="divmainwidth" >
                <table class="mytableOutbound" id="tbldatas" >
                    <thead>
                        <tr class="mytableReportItemsHeaderTR">
                            <th  colspan="15" class="thalign">
                                <table class="Headertablewidth">
                                     <tr>
                                        <td>
                                            <div class="buttonrowstyle">
                                                <button type="button" id="btnPdf" class="button button3" ng-click="exportPdf()">PDF &nbsp;<i class="fa fa-file-pdf-o" aria-hidden="true"></i></button>
                                                <button type="button" id="btnExcel" class="button button1" ng-click="exportExcel()">Excel &nbsp;<i class="fa fa-file-excel-o" aria-hidden="true"></i></button>
                                                <button type="button" id="btnTxt" class="button button4" ng-click="exportTxt()">Txt &nbsp;<i class="fa fa-file-text-o" aria-hidden="true"></i></button>
                                                <button type="button" id="btnWord" class="button button2" ng-click="exportWord()">Word &nbsp;<i class="fa fa-file-word-o" aria-hidden="true"></i></button>
                                                <button type="button" id="btnCsv" class="button button1" ng-click="exportCsv()">CSV &nbsp;<i class="fa fa-file-excel-o" aria-hidden="true"></i></button>
                                                <button type="button" id="btnXML" class="button button6" ng-click="exportXml()">XML &nbsp;<i class="fa fa-file-text-o" aria-hidden="true"></i></button>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </th>
                        </tr>
                </table>
                     <div class="divlineheight"></div>
                     <table class="Headertablewidth" style="padding:15px;">
                            <thead>
                                <tr class="mytableReportHeaderTR">
                                    <th colspan="5">
                                       Tenant Details
                                    </th>
                                </tr>
                                <tr class="mytableReportHeaderTR">
                            <th ng-click="sort('Client')">Sr. No<span class="glyphicon sort-icon" ng-show="sortKey=='Client'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('OBDNumber')">Tenant Name<span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('OBDNumber')">Activity Rate Type<span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('OBDNumber')">Activity Rate Name<span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('OBDNumber')">Cost Price<span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            
                        </tr>
                            </thead>
                            <tbody class="mytableReportBodyTR">
                            <tr dir-paginate="BLR in BIllingReport|orderBy:sortKey:reverse|filter:search|itemsPerPage:10">
                            <td style="text-align:center;" >{{$index +1}}</td>
                            <td style="text-align:center;" >{{BLR.TenantName}}</td>
                            <td style="text-align:center;">{{BLR.ActivityRateType}}</td>
                            <td style="text-align:center;">{{BLR.ActivityRateName}}</td>
                            <td style="text-align:center;">{{BLR.CostPrice}}</td>
                            
                        </tr>
                    </tbody>
                        </table>
                     <div class="divlineheight"></div>      
                    <table class="Headertablewidth" style="padding:15px;">
                        <thead>
                            <tr class="mytableReportHeaderTR">
                                <th colspan="6">
                                    Inbound Activity
                                </th>
                            </tr>
                                <tr class="mytableReportHeaderTR">
                            <th ng-click="sort('Client')">S No.<span class="glyphicon sort-icon" ng-show="sortKey=='Client'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('OBDNumber')">Tenant Name<span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('OBDNumber')">Store Ref. No.<span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('OBDNumber')">Activity Rate Name<span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('OBDNumber')">Quantity<span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('OBDNumber')">Cost Price<span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            
                        </tr>
                            </thead>
                            <tbody class="mytableReportBodyTR">
                            <tr dir-paginate="INB in InboundReport|orderBy:sortKey:reverse|filter:search|itemsPerPage:10">
                            <td style="text-align:center;" >{{$index +1}}</td>
                            <td style="text-align:center;" >{{INB.TenantName}}</td>
                            <td style="text-align:center;">{{INB.StoreRefNo}}</td>
                            <td style="text-align:center;">{{INB.ActivityRateName}}</td>
                            <td style="text-align:center;">{{INB.Quantity}}</td>
                            <td style="text-align:center;">{{INB.CostPrice}}</td>
                            
                        </tr>
                    </tbody>
                    </table>
                     <div class="divlineheight"></div>
                    <table class="Headertablewidth" style="padding:15px;">
                        <thead>
                            <tr class="mytableReportHeaderTR">
                                <th colspan="6">
                                    Outbound Activity
                                </th>
                            </tr>
                                <tr class="mytableReportHeaderTR">
                            <th ng-click="sort('Client')">S No.<span class="glyphicon sort-icon" ng-show="sortKey=='Client'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('OBDNumber')">Tenant Name<span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('OBDNumber')">OBD No.<span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('OBDNumber')">Activity Rate Name<span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('OBDNumber')">Quantity<span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('OBDNumber')">Cost Price<span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            
                        </tr>
                            </thead>
                            <tbody class="mytableReportBodyTR">
                            <tr dir-paginate="OBD in OutboundReport|orderBy:sortKey:reverse|filter:search|itemsPerPage:10">
                            <td style="text-align:center;" >{{$index +1}}</td>
                            <td style="text-align:center;" >{{OBD.TenantName}}</td>
                            <td style="text-align:center;">{{OBD.OBDNumber}}</td>
                            <td style="text-align:center;">{{OBD.ActivityRateName}}</td>
                            <td style="text-align:center;">{{OBD.Quantity}}</td>
                            <td style="text-align:center;">{{OBD.CostPrice}}</td>
                            
                        </tr>
                    </tbody>
                    </table>
                    <div class="divlineheight"></div>
                </div>

                <table id="tbldata"></table>
            </div>
        <div class="divlineheight"></div>
              
          </div>     
     </div>
    </div>
</asp:Content>
