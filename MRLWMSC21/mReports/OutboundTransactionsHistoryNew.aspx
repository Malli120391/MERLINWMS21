<%@ Page Language="C#" Title="Outbound Transactions History" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="OutboundTransactionsHistoryNew.aspx.cs" Inherits="MRLWMSC21.mReports.OutboundTransactionsHistoryNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
    <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
    <script src="OutboundTransactionsHistoryNew.js"></script>
    <link href="Scripts/Custom.css" rel="stylesheet" />
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
     <script type="text/javascript">
        $(document).ready(function () {
            
            $(document).ready(function () {
                $("#txtFromDate").datepicker({
                    dateFormat: "dd-M-yy",
                    maxDate: new Date(),
                    onSelect: function (selected) {
                        $(this).focus();
                        $("#txtToDate").datepicker("option", "minDate", selected, { dateFormate: "dd-M-yy" })
                    }
                });
                $("#txtToDate").datepicker({
                    dateFormat: "dd-M-yy",
                    maxDate: new Date(),
                    onSelect: function (selected) {
                        $(this).focus();
                    }
                });

                $('#txtFromdate, #txtToDate').keypress(function () {
                    return false;
                });
            });
        });


    </script>
    <style>
        .monthborder {
            background-color: #f7f7f7 !important;
            color: #171414;
            text-align: center !important;
        }

        .table {
            white-space: nowrap;
        }
    </style>
    <div ng-app="myApp" ng-controller="OutboundTransactionsHistoryNew" class="pagewidth">
        <div class="divlineheight"></div>
        <div>
            <table class="Headertablewidth">
                <tr>
                    <td align="right">
                        <%-- <button type="button" ng-click="Getgedetails()"  style="background-color: #455b7c;" class="addbuttonOutbound">Search <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>--%>
                    </td>

                </tr>

            </table>

        </div>
        <div class="row">
            <div class="">
                <div class="col m3">
                        <div class="flex">
                            <%--<input type="text" id="txtWarehouse" required="" />--%>
                             <asp:TextBox runat="server" ID="txtWarehouse" ClientIDMode="Static" required=""></asp:TextBox>
                            <label><%= GetGlobalResourceObject("Resource", "WareHouse")%></label>
                            <asp:HiddenField runat="server" ID="hdnWarehouse" ClientIDMode="Static" Value="0" />
                            <span class="errorMsg" ></span>
                        </div>
                    </div>
                <div class="col m3">
                    
                    <div class="flex">
                       <%-- <input type="text" id="txtTenant" ngchange="getskus()" ng-click="getskus()" class="TextboxInventoryAuto" required="" />--%>
                         <asp:TextBox runat="server" ID="txtTenant" ClientIDMode="Static" required=""></asp:TextBox>
                        <label><%= GetGlobalResourceObject("Resource", "Tenant")%></label>
                        <asp:HiddenField runat="server" ID="hdnTenant" ClientIDMode="Static" Value="0" />
                       <%-- <span class="errorMsg"></span>--%>
                    </div>
                </div>
                 
                <div class="col m3">
                    <div class="flex">
                        <%--<input type="text" id="txtPartnumber" ngchange="getskus()" ng-click="getskus()" class="TextboxInventoryAuto" required="" />--%>
                         <asp:TextBox runat="server" ID="txtPartnumber" ClientIDMode="Static" required=""></asp:TextBox>
                        <asp:HiddenField runat="server" ID="hdnPartNo" ClientIDMode="Static" Value="0" />
                        <label><%= GetGlobalResourceObject("Resource", "PartNumber")%></label>
                    </div>
                </div>
              
                <div class="col m3">
                    <div class="flex">
                        <%--<input type="text" ng-model="fromdate" id="txtFromDate" onpaste="return false" required="" />       --%>
                        <asp:TextBox runat="server" ID="txtFromDate" ClientIDMode="Static" onpaste="return false"  required=""></asp:TextBox>      
                        <label>From Date</label>
                    </div>
                </div>
               
                 <div class="col m3">
                    <div class="flex">
                        <%--<input type="text" ng-model="fromdate" id="txtToDate" onpaste="return false" required="" />          --%>
                        <asp:TextBox runat="server" ID="txtToDate" ClientIDMode="Static" onpaste="return false" required=""></asp:TextBox>
                        <label>To Date</label>
                    </div>
                </div>
              
                <div class="col m12">
                    <flex end><button type="button" ng-click="GetReport(1)" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Search")%> <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                        <asp:LinkButton runat="server" ID="lnkExportData" CssClass="btn btn-primary" OnClick="lnkExportData_Click">Export <i class="fa fa-file-excel-o" aria-hidden="true"></i></asp:LinkButton>
                    </flex>
                    <%--<button type="button" class="btn btn-primary" ng-click="exportExcel()"><%= GetGlobalResourceObject("Resource", "Excel")%><i class="fa fa-file-excel-o" aria-hidden="true"></i></button></flex>--%>

                </div>
            </div>
        </div>
        <div class="row" style="margin: 0;">
            <div class="col-md-12" style="margin: 0; padding: 0;">
                <div class="divmainwidth">
                 
                    <div colspan="12" style="text-align: center !important; font-weight: 600; font-size: 14px; color: #484444; margin-left: 23%;">
                        <%= GetGlobalResourceObject("Resource", "TotalIssuedQuantityandNoofOutboundTransactions")%>
                    </div>
                    <table class="table-striped" id="tbldatas">
                        <thead>
                            <tr>                                
                                <th>Material Code</th>
                                <th>Tenant Code</th>
                                <th>Year of Trans.</th>
                                <th class="monthborder">Jan</th>
                                <th class="monthborder">Feb</th>
                                <th class="monthborder">Mar</th>
                                <th class="monthborder">Apr</th>
                                <th class="monthborder">May</th>
                                <th class="monthborder">June</th>
                                <th class="monthborder">July</th>
                                <th class="monthborder">Aug</th>
                                <th class="monthborder">Sep</th>
                                <th class="monthborder">Oct</th>
                                <th class="monthborder">Nov</th>
                                <th class="monthborder">Dec</th>
                            </tr>
                        </thead>
                        <tbody class="">
                            <tr dir-paginate="OBDT in OBDTramsactionData|itemsPerPage:25" total-items="TotalRecords">
                                <td>{{OBDT.MCode}}</td>
                                <td>{{OBDT.TenantCode}}</td>
                                <td>{{OBDT.TrYear}}</td>
                                <td class="monthborder">{{OBDT['1']}}</td>
                                <td class="monthborder">{{OBDT['2']}}</td>
                                <td class="monthborder">{{OBDT['3']}}</td>
                                <td class="monthborder">{{OBDT['4']}}</td>
                                <td class="monthborder">{{OBDT['5']}}</td>
                                <td class="monthborder">{{OBDT['6']}}</td>
                                <td class="monthborder">{{OBDT['7']}}</td>
                                <td class="monthborder">{{OBDT['8']}}</td>
                                <td class="monthborder">{{OBDT['9']}}</td>
                                <td class="monthborder">{{OBDT['10']}}</td>
                                <td class="monthborder">{{OBDT['11']}}</td>
                                <td class="monthborder">{{OBDT['12']}}</td>



<%--                                <td class="monthborder">{{BLR.Jan}}</td>
                                <td class="monthborder">{{BLR.Feb}}</td>
                                <td class="monthborder">{{BLR.Mar}}</td>
                                <td class="monthborder">{{BLR.Apr}}</td>
                                <td class="monthborder">{{BLR.May}}</td>
                                <td class="monthborder">{{BLR.June}}</td>
                                <td class="monthborder">{{BLR.July}}</td>
                                <td class="monthborder">{{BLR.Aug}}</td>
                                <td class="monthborder">{{BLR.Sep}}</td>
                                <td class="monthborder">{{BLR.Oct}}</td>
                                <td class="monthborder">{{BLR.Nov}}</td>
                                <td class="monthborder">{{BLR.Dec}}</td>--%>
                            </tr>
                        </tbody>
                        <tfoot>
                            <tr class="mytableReportFooterTR">
                                <td colspan="15">
                                    <div class="divpaginationstyle">
                                        <dir-pagination-controls direction-links="true" boundary-links="true" on-page-change="GetReport(newPageNumber)"> </dir-pagination-controls>
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

