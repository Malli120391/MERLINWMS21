<%@ Page Language="C#" Title="Putaway Performance Report" AutoEventWireup="true" MasterPageFile="~/mReports/mReport.master" CodeBehind="PutawayPerforamcneReprot.aspx.cs" Inherits="MRLWMSC21.mReports.PutawayPerforamcneReprot" %>


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
    <script src="PutawayPerforamcneReprot.js"></script>

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
                           <%-- <span class="errorMsg"></span>--%>
                            <label><%= GetGlobalResourceObject("Resource", "Tenant")%></label>
                        </div>
                    </div>
                     
                    <div class="col m3">
                        <div class="flex">
                            <input type="text" required="" onpaste="return false" ng-model="fromdate" id="txtFromdate" />
                              <label> <%= GetGlobalResourceObject("Resource", "FromDate")%></label>
                        </div>
                    </div>
                    <div class="col m3" >
                        <div class="flex">
                            <input type="text" required="" onpaste="return false" ng-model="fromdate" id="txtTodate" />                   
                             <label><%= GetGlobalResourceObject("Resource", "ToDate")%> </label>
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
            <div class="" style="margin: 0; padding: 0;">
                <div class="divmainwidth">
                    <table class=" table-striped" id="tbldatas">
                        <thead>
                         
                            <tr class="">
                                 <th ng-click="sort('Client')"><%= GetGlobalResourceObject("Resource", "SNo")%><span class="glyphicon sort-icon" ng-show="sortKey=='Client'" ng-class="{'fa fa-sort-amount-desc':reverse,'fa fa-sort-amount-asc':!reverse}"></span></th>
                                <th number ng-click="sort('OBDNumber')"> <%= GetGlobalResourceObject("Resource", "TotalInwardWorkLines")%></th>
                                <th number ng-click="sort('OBDNumber')"><%= GetGlobalResourceObject("Resource", "WorkItemsCompleted")%></th>
                                <th number ng-click="sort('OBDNumber')"> <%= GetGlobalResourceObject("Resource", "WorkCompletedPercent")%></th>
                                <th number ng-click="sort('OBDNumber')"> <%= GetGlobalResourceObject("Resource", "WorkItemsPending")%></th>
                                <th number ng-click="sort('OBDNumber')"><%= GetGlobalResourceObject("Resource", "WorkPendingPercent")%> </th>
                            </tr>
                        </thead>
                        <tbody class="">
                            <tr dir-paginate="BLR in list | filter:searchKeyword |itemsPerPage:25">
                                <td>{{$index +1}}</td>
                                <td number>{{BLR.TotalInwardWorkLines}}</td>
                                <td number>{{BLR.WorkItemsCompleted}}</td>
                                <td number>{{BLR.WorkCompletedPercent}}</td>
                                <td number>{{BLR.WorkItemsPanding}}</td>
                                <td number>{{BLR.WorkPendingPercent}}</td>
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
