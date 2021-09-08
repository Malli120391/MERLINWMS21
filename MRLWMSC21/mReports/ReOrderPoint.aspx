<%@ Page Language="C#" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="ReOrderPoint.aspx.cs" Inherits="MRLWMSC21.mReports.ReOrderPoint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
    <script src="../Scripts/angular.min.js"></script>
    <script src="../mReports/Scripts/dirPagination.js"></script>
    <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
    <link href="../mInventory/Scripts/bootstrap.min.css" rel="stylesheet" />
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>

    <script src="ReOrderPoint.js"></script>

    <div class="dashed"></div>

    <div class="pagewidth">
        <div ng-app="MyApp" ng-controller="FormCtrl">
            <inv-preloader is="show" style="display: none"></inv-preloader>
            <div ng-show="blockUI">
                <div style="width: 100%; height: 100vh; z-index: 999; position: fixed; top: 0; left: 0; right: 0; bottom: 0; align-items: center; display: flex; justify-content: center; background: rgba(255, 255, 255, 0.24); background: hsla(0, 0%, 100%, 0.72);">

                    <div style="align-self: center;">
                        <div class="spinner">
                            <div class="bounce1"></div>
                            <div class="bounce2"></div>
                            <div class="bounce3"></div>
                        </div>

                    </div>

                </div>
            </div>

            <div class="row">
                <div class="col s4 m2">
                    <div class="flex">
                        <input type="text" id="txtWarehouse" required="" />
                        <label>Warehouse</label>
                        <span class="errorMsg"></span>
                    </div>
                </div>

                <div class="col s4 m2">
                    <div class="flex">
                        <input type="text" id="txtTenant" required="" />
                        <label>Tenant</label>
                        <span class="errorMsg"></span>
                    </div>
                </div>

                <div class="col s4 m2">
                    <div class="flex">
                        <input type="text" id="txtMaterialGroup" required="" />
                        <label>Material Group</label>
                    </div>
                </div>

                <div class="col s4 m2">
                    <div class="flex">
                        <input type="text" id="txtMType" required="" />
                        <label>Material Type</label>
                    </div>
                </div>

                <div class="col s4 m2">
                    <div class="flex">
                        <input type="text" id="txtMcode" required="" />
                        <label>Mcode</label>
                    </div>
                </div>

                <div class ="col s4 m2">
                    <div class ="flex">
                       <button type="button" id="btnSearch" ng-click="Getgedetails(1)" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Search")%>  <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                     <a class="btn btn-primary" ng-click="exportExcel()"><%= GetGlobalResourceObject("Resource", "Export")%> <i class="fa fa-file-excel-o" aria-hidden="true"></i></a>
                    </div>

                </div>

            </div>

            <div class="row">
                <table class="table-striped">
                    <thead>
                        <tr>
                            <th>S.No</th>
                            <th>Mcode</th>
                            <th>Current Stock Level</th>
                            <th>Lead Time(Days)</th>
                            <th>Max. Lead Time(Days)</th>
                            <th>Avg. Lead Time (Days)</th>
                            <th>Avg. Daily Usage</th>
                            <th>Safety Stock</th>
                            <th>Reorder Point</th>
                            <th>Reorder Status</th>
                        </tr>
                        
                    </thead>
                    <tbody>
                        <tr dir-paginate="row in BillingData  |itemsPerPage:noofrecords" total-items="Totalrecords"">
                            <td>{{$index+1}}</td>
                            <td>{{row.MCode}}</td>
                            <td>{{row.CurrentstockLevel}}</td>
                            <td>{{row.LeadTime}}</td>
                            <td>{{row.MAX_LeadTime}}</td>
                            <td>{{row.AVG_LeadTime}}</td>
                            <td>{{row.AVG_SOQty}}</td>
                            <td>{{row.SafetyStock}}</td>
                            <td>{{row.ReorderPoint}}</td>
                             <td style="text-align: center !important;color:#03bd1a !important;font-weight:bold !important;" ng-show="row.ReorderStatus=='No'">No</td>        
                            <td style="text-align: center !important;color:red !important;font-weight:bold !important;" ng-show="row.ReorderStatus=='Yes'">Yes</td> 

                        </tr>
                    </tbody>
                    <tfoot>
                        <tr ng-show="BillingData.length == 0">
                            <td colspan="11" style="text-align: center !important">No Data Found</td>
                        </tr>
                    </tfoot>
                </table>
                 <div style="float: right !important; font-family: Arial; font-size: small; margin-right: 1%;">
                <dir-pagination-controls direction-links="true" boundary-links="true" on-page-change="Getgedetails(newPageNumber)"> </dir-pagination-controls>

            </div>

        </div>
    </div>
</asp:Content>
