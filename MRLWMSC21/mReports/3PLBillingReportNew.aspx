<%@ Page Title="" Language="C#" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="3PLBillingReportNew.aspx.cs" Inherits="MRLWMSC21.mReports._3PLBillingReportNew" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#txtFromdate").datepicker({
                dateFormat: "dd/mm/yy",
                maxDate: new Date(),
                onSelect: function (selected) {
                    $(this).focus();
                    $("#txttodate").datepicker("option", "minDate", selected, { dateFormate: "dd/mm/yy" })
                 }
            });
            $("#txttodate").datepicker({
                dateFormat: "dd/mm/yy",
                maxDate: new Date(),
                onSelect: function (selected) {
                    $(this).focus();
                }
            });
        });

    </script>
    <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
     <script src="3PLBillingReportNew.js"></script>
    <link href="Scripts/Custom.css" rel="stylesheet" />
    
<div class="dashed"></div>
<div class="pagewidth">
    <div ng-app="myApp" ng-controller="3PLBillingReport" style="overflow:auto; margin: 0; padding: 0;">
         <div class="divlineheight"></div>
         <div>
            <table class="Headertablewidth" style="width: 99%;">
                <tr>
                    <td align="right">
                             <div class="flex"><input type="text" id="txtTanent" ngchange="getskus()" ng-click="getskus()" required="required"  class="" /><label>Tanent</label></div>  </td>
                             <td align="right"> <div class="flex"><input type="text" class="" ng-model="fromdate" id="txtFromdate" required="required" /><label>From Date</label></div>  </td>
                            <td align="right">  <div class="flex"><input type="text" class="" ng-model="todate" id="txttodate" required="required" /> <label>To Date</label> 
                               </div> </td>
                            <td align="right">  <button type="button" ng-click="Getgedetails()"  class="btn btn-primary">Search <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>  </td>
                  
                </tr>
            </table>
        </div>
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
                        <tr class="mytableReportHeaderTR">
                            <th ng-click="sort('Client')">Sr. No<span class="glyphicon sort-icon" ng-show="sortKey=='Client'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('OBDNumber')">TatentName<span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('OBDNumber')">StoreRefNo<span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('OBDNumber')">SupplierName<span class="glyphicon sort-icon" ng-show="sortKey=='OBDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('VLPDNumber')">PONumber <span class="glyphicon sort-icon" ng-show="sortKey=='VLPDNumber'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('MType')">Receipt<span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('MType')">Services<span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('MType')">UoM <span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('MType')">UnitCost <span class="glyphicon sort-icon" ng-show="sortKey=='MType'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('ItemVolume')">After Disc <span class="glyphicon sort-icon" ng-show="sortKey=='ItemVolume'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('ItemWeight')"> Qty <span class="glyphicon sort-icon" ng-show="sortKey=='ItemWeight'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('ItemWeight')"> TotalCost <span class="glyphicon sort-icon" ng-show="sortKey=='ItemWeight'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('ItemWeight')"> TotalCostAfterDisc <span class="glyphicon sort-icon" ng-show="sortKey=='ItemWeight'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('ItemWeight')"> DiscWithOutTax <span class="glyphicon sort-icon" ng-show="sortKey=='ItemWeight'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                            <th ng-click="sort('ItemWeight')"> Tax <span class="glyphicon sort-icon" ng-show="sortKey=='ItemWeight'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                          
                        </tr>
                    </thead>
                    <tbody class="mytableReportBodyTR">
                        <tr dir-paginate="BLR in BIllingReport|orderBy:sortKey:reverse|filter:search|itemsPerPage:10">
                            <td align="center">{{$index +1}}</td>
                            <td >{{BLR.TatentName}}</td>
                            <td>{{BLR.StoreRefNo}}</td>
                            <td >{{BLR.SupplierName}}</td>
                            <td >{{BLR.PONumber}}</td>
                            <td >{{BLR.Receipt}}</td>
                            <td >{{BLR.Services}}</td>
                            <td>{{BLR.UoM}}</td>
                             <td  align="right">{{BLR.UnitCost}}</td>
                            <td  align="right">{{BLR.UnitCostAfterDisc}}</td>
                            <td  align="right">{{BLR.Quantity}}</td>
                            <td  align="right">{{BLR.TotalCost}}</td>
                            <td  align="right">{{BLR.TotalCostAfterDisc}}</td>
                            <td  align="right">{{BLR.TotalCostAfterDiscWithOutTax}}</td>
                            <td  align="right">{{BLR.Tax}}</td>
                          
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
            </div>
        </div>
    </div>
    </div>
</asp:Content>
