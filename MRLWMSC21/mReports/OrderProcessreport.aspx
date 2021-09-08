<%@ Page Title="Order Process report" Language="C#" AutoEventWireup="true" MasterPageFile="~/mReports/mReport.master" CodeBehind="OrderProcessreport.aspx.cs" Inherits="MRLWMSC21.mReports.OrderProcessreport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#txtFromdate").datepicker({
                dateFormat: "dd-M-yy",
                maxDate: new Date(),
                onSelect: function (selected) {
                    $(this).focus();
                    $("#txtTodate").datepicker("option", "minDate", selected, { dateFormate: "dd-M-yy" })
                }
            });
            $("#txtTodate").datepicker({
                dateFormat: "dd-M-yy",
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

    <link href="Scripts/Custom.css" rel="stylesheet" />
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
    <script src="OrderProcessreport.js"></script>

    <div ng-app="myApp" ng-controller="PickPerformance" class="container">

        <div class="divlineheight"></div>
        <div class="row">
          
            <div class="">
                <div class="">
                    <div class="col m3">
                        <div class="flex">
                            <input type="text" id="txtWarehouse" required="" />
                            <label><%= GetGlobalResourceObject("Resource", "WareHouse")%></label>
                             <span class="errorMsg"></span>
                        </div>
                    </div>

                    <div class="col m3">
                        <div class="flex">
                            <input type="text" id="txtTenant" required="" />
                            <label><%= GetGlobalResourceObject("Resource", "Tenant")%></label>
                            <span class="errorMsg"></span>
                        </div>
                    </div>
                     
                    <div class="col m3">
                        <div class="flex">
                            <input type="text" required="" ng-model="fromdate" id="txtFromdate" onpaste="return false" />
                            <label> <%= GetGlobalResourceObject("Resource", "FromDate")%></label>
                        </div>
                    </div>
                    <div class="col m3">
                        <div class="flex">
                            <input type="text" required="" ng-model="fromdate" id="txtTodate" onpaste="return false" />                    
                              <label> <%= GetGlobalResourceObject("Resource", "ToDate")%></label>
                        </div>
                    </div>
                    <div class="col m12">
                        <gap5></gap5>
                        <flex end> <button type="button" ng-click="Getgedetails()" class="btn btn-primary"> <%= GetGlobalResourceObject("Resource", "Search")%> <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                         <button type="button" ng-click="exportExcel()" class="btn btn-primary"> <%= GetGlobalResourceObject("Resource", "Export")%> <i class="fa fa-file-excel-o" aria-hidden="true"></i></button></flex>
                    </div>
                          
                </div>
            </div>

        </div>

        <div class="row" style="margin: 0;">
            <div>
                <div class="divmainwidth">
                    <table class=" table-striped" id="tbldatas">
                        <thead>
                            <tr class="mytableReportHeaderTR">
                            <%--    <th ng-click="sort('Client')">S.No<span class="glyphicon sort-icon" ng-show="sortKey=='Client'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th number ng-click="sort('OBDNumber')">Total Inward</th>
                                <th number ng-click="sort('OBDNumber')">Work Items Completed</th>
                                <th number ng-click="sort('OBDNumber')">Completed Percentage</th>
                                <th number ng-click="sort('OBDNumber')">Receipts To Do</th>
                                <th number ng-click="sort('OBDNumber')">To Do Perecentage</th>--%>
                                    <th ng-click="sort('Client')"> <%= GetGlobalResourceObject("Resource", "SNo")%><span class="glyphicon sort-icon" ng-show="sortKey=='Client'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th number ng-click="sort('OBDNumber')"><%= GetGlobalResourceObject("Resource", "TotalInward")%> </th>
                                <th number ng-click="sort('OBDNumber')"><%= GetGlobalResourceObject("Resource", "WorkItemsCompleted")%></th>
                                <th number ng-click="sort('OBDNumber')"><%= GetGlobalResourceObject("Resource", "CompletedPercentage")%></th>
                                <th number ng-click="sort('OBDNumber')"><%= GetGlobalResourceObject("Resource", "ReceiptsToDo")%> </th>
                                <th number ng-click="sort('OBDNumber')"><%= GetGlobalResourceObject("Resource", "ToDoPerecentage")%> </th>
                            </tr>
                        </thead>
                        <tbody class="mytableReportBodyTR">
                            <tr dir-paginate="BLR in list | filter:searchKeyword |itemsPerPage:25">
                                <td>{{$index +1}}</td>
                                <td number>{{BLR.TotalInward}}</td>
                                <td number>{{BLR.WorkItemsCompleted}}</td>
                                <td number>{{BLR.CompletedPercent}}</td>
                                <td number>{{BLR.ReceiptsToDo}}</td>
                                <td number>{{BLR.ToDoPercent}}</td>
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
