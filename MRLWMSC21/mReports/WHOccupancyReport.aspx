<%@ Page Language="C#" Title="Warehouse Occupancy Report" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="WHOccupancyReport.aspx.cs" Inherits="MRLWMSC21.mReports.WHOccupancyReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
   <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
    <script src="WHOccupancyReport.js"></script>
    <link href="Scripts/Custom.css" rel="stylesheet" />
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {

            $("#txtDate").datepicker({
                dateFormat: "dd-M-yy",
                maxDate: new Date(),
            });

        });
    </script>

    <style>
        .tableLoader::before {
            height: 392px;
        }
    </style>
    <div ng-app="myApp" ng-controller="WHOccupancyReport" class="container">

        <div class="row">


            <div class="col m3">
                <div class="flex">
                     <asp:TextBox runat="server" ID="txtTenant" ClientIDMode="Static" required=""></asp:TextBox>
                     <label>Tenant</label>
                     <asp:HiddenField runat="server" ID="hdnTenant" ClientIDMode="Static" Value="0" />
                </div>
            </div>

            <div class="col m3">
                <div class="flex">
                    <asp:TextBox runat="server" ID="txtWarehouse" ClientIDMode="Static" required=""></asp:TextBox>
                    <label>Warehouse</label>
                     <span class="errorMsg"></span>
                    <asp:HiddenField runat="server" ID="hdnWarehouse" ClientIDMode="Static" Value="0" />
                </div>
            </div>
            <div class="col m3">
                <div class="flex">
                    <asp:TextBox runat="server" ID="txtDate" ClientIDMode="Static" required=""></asp:TextBox>
                    <label>Date</label>
                     <span class="errorMsg"></span>
                </div>
            </div>

            <%-- <div class="col m3">
                <div class="flex">
                    <input type="text" id="txtTenant" required="" />
                    <label>Tenant</label>
                </div>
            </div>

            <div class="col m3">
                <div class="flex">
                    <input type="text" id="txtWarehouse" required="" />
                    <label>Warehouse</label>
                    <span class="errorMsg"></span>
                </div>
            </div>
            
            <div class="col m3">
                <div class="flex">
                    <input type="text" id="txtDate" required="" />
                    <label>Date</label>
                    <span class="errorMsg"></span>
                </div>
            </div>--%>
            <div class="col m3">
                <gap5></gap5>
                <button type="button" ng-click="GetWHDataReport()" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Search")%>  <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                 <asp:LinkButton runat="server" ID="lnkExportData" CssClass="btn btn-primary" OnClick="lnkExportData_Click">Export <i class="fa fa-file-excel-o" aria-hidden="true"></i></asp:LinkButton>
                <%--<button type="button" class="btn btn-primary" style="background-color:#108e05 !important;" ng-click="exportExcel()">Export <i class="fa fa-file-excel-o" aria-hidden="true"></i></button>--%>
                <button type="button" class="btn btn-primary" style="background-color:red !important;" ng-click="GetWHDataReportPDF()">Export <i class="fa fa-file-pdf-o" aria-hidden="true"></i></button>
            </div>
        </div>
      
       <%-- -------------------------- Remove Unneccesary code from table by durga on 21/05/2018------------------------%>

        <div class="row hideContent">
            <div class="col m12" style="text-align:center;font-size:15px;font-weight:500;">
                <span>Transcrate International Logistics</span><br />
                    <span ng-show="warehouse == 1">3PL Warehouse-01, Amghara</span>
                    <span ng-show="warehouse == 4027">3PL Warehouse-01, Sharq</span>
                    <span ng-show="warehouse != 1 && warehouse != 4027">3PL Warehouse-02, Amghara</span>
            </div>
        </div>


        <div class="row">
            <div class="col m12">
                    <table class="table-striped" id="tbldatas">
                        <thead>

                            <tr>
                                <th>S. No.</th>
                                <th>Company</th>
                                <th>Total Volume (CBM)</th>
                                <th>Occupied Percentage</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr dir-paginate="WH in WHData | itemsPerPage:25">                              
                                <td>
                                    <span ng-show="WH.TenantName != 'TOTAL'">{{$index+1}}</span>
                                    <span ng-show="WH.TenantName == 'TOTAL'"></span>
                                </td>
                                <td>{{WH.TenantName}}</td>
                                <td>{{WH.TotalVolume}}</td>
                                <td>{{WH.Occupancy}}</td>                               
                            </tr>
                        </tbody>
                         <tfoot>
                            <tr>
                                <td colspan="15">
                                    <div class="divpaginationstyle">
                                        <dir-pagination-controls direction-links="true" boundary-links="true"> </dir-pagination-controls>
                                    </div>
                                </td>
                            </tr>
                        </tfoot>

                    </table>

                <table id="tbldata"></table>
            </div>
            <div class="divlineheight"></div>

        </div>
        <br />
        <div class="row">
            <div class="col m12">
                <table class="table-striped">
                        <thead>
                            <tr>
                                <th>Total Warehouse Volume (CBM)</th>
                                <th>Total Occupied Volume (CBM)</th>
                                <th>Total Available Volume (CBM)</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr ng-repeat="WHT in WHDataTotal">
                                <td>{{WHT.WarehouseVolume}}</td>
                                <td>{{WHT.OccupiedVolume}}</td>
                                <td>{{WHT.AvailableVolume}}</td>                            
                            </tr>
                        </tbody>
                    </table>
            </div>
        </div>
</asp:Content>

