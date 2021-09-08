<%@ Page Title="Space Utilization Report" Language="C#" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="SpaceUtilizationReportNew.aspx.cs" Inherits="MRLWMSC21.mReports.SpaceUtilizationReportNew" %>

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
    <script src="SpaceUtilizationReportNew.js"></script>


    <div ng-app="myApp" ng-controller="SpaceUtilizationReportNew" class="container">

        <div class="row">

            <div class="">
                <div class="">
                    <div class="col m3 offset-m1 s3">
                        <div class="flex">
                            <select ng-model="ddlWarehouse" ng-options="warehouse.WarehouseID as warehouse.WHCode for warehouse in warehouses">
                                <option value="">Select</option>
                            </select>
                        </div>
                    </div>
                    <div class="col m3 s3">
                        <div class="flex">
                            <input type="text" ng-model="fromdate" id="txtFromdate" onpaste="return false" required="" />

                            <label><%= GetGlobalResourceObject("Resource", "FromDate")%></label>
                        </div>
                    </div>
                    <div class="col m3 s3">
                        <div class="flex">
                            <input type="text" ng-model="todate" id="txttodate" required="" />

                            <label><%= GetGlobalResourceObject("Resource", "ToDate")%></label>
                        </div>
                    </div>
                    <div class="col m2 s3">
                        <gap5></gap5>
                        <button type="button" ng-click="Getgedetails()" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Search")%>  <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                        <a class="btn btn-primary" ng-click="exportExcel()"><%= GetGlobalResourceObject("Resource", "Export")%><i class="fa fa-file-excel-o" aria-hidden="true"></i></a>
                    </div>
                </div>
            </div>
        </div>
        <div class="row" style="margin: 0;">
            <div class="col-sm-12 col-lg-6" style="margin: 0; padding: 0;">
                <div class="divmainwidth">
                    <table class="mytableOutbound table-striped" id="tbldatas">
                        <thead>
                            <%--<tr class="mytableReportHeaderTR">
                         
                           <th ng-repeat = "result in BIllingReport">
                        </tr>
                    </thead>
                    <tbody class="mytableReportBodyTR">
                        <tr ng-repeat = "result in BIllingReport">
                            
                           <%-- <td align="center" >{{BLR.Zone}}</td>
                            <td align="center">{{BLR.Date}}</td>
                            <td align="center" >{{BLR.UsedSpace}}</td>--%>
                            <%--<td> {{result.Key }} </td>
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
                    </tfoot>--%>
                            <tr>
                                <td>
                                    <div id="div12">
                                    </div>
                                </td>
                            </tr>
                    </table>
                </div>

                <table id="tbldata"></table>
                <div class="divlineheight"></div>
            </div>
        </div>
    </div>
</asp:Content>
