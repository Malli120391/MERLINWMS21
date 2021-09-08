<%@ Page Language="C#" Title="Current Stock Report" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="CurrentStockRPT.aspx.cs" Inherits="MRLWMSC21.mReports.CurrentStockRPT" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
   <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
    <script src="CurrentStockRPT.js"></script>
    <link href="Scripts/Custom.css" rel="stylesheet" />
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
    <style>
        .tableLoader::before {
            height: 392px;
        }
    </style>
    <script>
        $(document).ready(function () {
            sessionStorage.removeItem('UserID');
            sessionStorage.setItem('UserID', <%=cp.UserID%>);
        });
    </script>
   
    <div ng-app="myApp" ng-controller="CurrentStockRPT" class="container">

        <div class="row">
            <div class="col m3">
                <div class="flex">
                    <input type="text" id="txtWarehouse" required="" />
                    <label>Warehouse</label>
                </div>
            </div>
            <div class="col m3">
                <div class="flex">
                    <input type="text" id="txtTenant" required="" />
                    <label>Tenant</label>
                </div>
            </div>
            
            <div class="col m3">
                <div class="flex">
                    <input type="text" id="txtFromLocation" required="" />
                    <label>From Location</label>
                </div>
            </div>
            <div class="col m3">
                <div class="flex">
                    <input type="text" id="txtToLocation" required="" />
                    <label>To Location</label>
                </div>
            </div>

        </div>

        <div class="row">
            <div class="col m12" flex end>
                <gap5></gap5>
                <button type="button" ng-click="getStockData(1)" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Search")%>  <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                <button type="button" class="btn btn-primary" ng-click="exprotData()">Export <i class="fa fa-file-excel-o" aria-hidden="true"></i></button>
            </div>
        </div>

        <%-- -------------------------- Remove Unneccesary code from table by durga on 21/05/2018------------------------%>
        <div class="row" style="margin: 0;">
            <div class="col-sm-6 col-lg-6" style="margin: 0; padding: 0;">
                <div class="divmainwidth">
                    <table class="table-striped" id="tbldatas">
                        <thead>
                            <tr>
                                <th>Part No.</th>
                                <th>Description</th>
                                <th>Category</th>
                                <th>Company</th>
                                <th>Warehouse</th>
                                <th>Location</th>
                                <th>Pallet</th>
                                <th>Available Qty.</th>
                                <th>UoM</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr dir-paginate="std in stockData|itemsPerPage:25" total-items="Totalrecords">
                                <td>{{std.MCode}}</td>
                                <td>{{std.MDescription}}</td>
                                <td>{{std.ProductCategory}}</td>
                                <td>{{std.TenantName}}</td>
                                <td>{{std.WHCode}}</td>
                                <td>{{std.Location}}</td>
                                <td>{{std.CartonCode}}</td>
                                <td>{{std.AvaliableQuantity}}</td>
                                <td>{{std.UoM}}</td>                               
                            </tr>
                        </tbody>
                         <tfoot>
                            <tr>
                                <td colspan="15">
                                    <div class="divpaginationstyle">
                                        <dir-pagination-controls direction-links="true" boundary-links="true" on-page-change="getStockData(newPageNumber)"> </dir-pagination-controls>
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
</asp:Content>

