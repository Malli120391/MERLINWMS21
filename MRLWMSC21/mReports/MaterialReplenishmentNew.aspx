<%@ Page Language="C#" Title="Material Replenishment Report" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="MaterialReplenishmentNew.aspx.cs" Inherits="MRLWMSC21.mReports.MaterialReplenishmentNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
    <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
    <link href="Scripts/Custom.css" rel="stylesheet" />
    <script src="MaterialReplenishmentNew.js"></script>
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>

    <div ng-app="myApp" ng-controller="MaterialReplenishmentNew" class="container">
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
                           <%-- <span class="errorMsg"></span>--%>
                        </div>
                    </div>
                     
                    <div class="col m3">
                        <div class="flex">
                            <input type="text" id="txtPartNo" required="" />
                            <label><%= GetGlobalResourceObject("Resource", "PartNo")%></label>
                        </div>
                       
                    </div>
                    <div class="col m1">
                        <gap5></gap5>
                        <button type="button" ng-click="Getgedetails()" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Search")%> <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                    </div>

                </div>
            </div>
        </div>
        <div class="row" style="margin: 0;">
            <div class="" style="margin: 0; padding: 0;">
                <div class="">
                    <table class="table-striped" id="tbldatas">
                        <thead>
                            <tr class="">
                                <%-- <th>S. No.</th>--%>
                                <th><%= GetGlobalResourceObject("Resource", "SNo")%> </th>
                                <%-- <th>Part No.</th>--%>
                               <%-- <th>Location</th>--%>
                                <th><%= GetGlobalResourceObject("Resource", "PartNo")%></th>
                                    <%--<th>Description</th>--%>
                                <th><%= GetGlobalResourceObject("Resource", "Description")%> </th>
                                <%--<th>Tenant</th>--%>
                                <th><%= GetGlobalResourceObject("Resource", "Tenant")%></th>
                                <%--<th>Supplier</th>--%>
                                <th><%= GetGlobalResourceObject("Resource", "Supplier")%></th>
                                <%-- <th>Material Type</th>--%>
                                <th><%= GetGlobalResourceObject("Resource", "MaterialType")%> </th>
                            
                                <%--<th>Planned Delivery in Days</th>--%>
                                <%--<th number><%= GetGlobalResourceObject("Resource", "PlannedDeliveryinDays")%> </th>--%>
                                <%--    <th>Expected Unit Cost</th>--%>
                                <%--<th number><%= GetGlobalResourceObject("Resource", "ExpectedUnitCost")%></th>--%>
                                <%-- <th>Initial Order Qty.</th>--%>
                                <th number><%= GetGlobalResourceObject("Resource", "InitialOrderQty")%> </th>
                                <%--  <th>Available Qty.</th>--%>
                                <th number><%= GetGlobalResourceObject("Resource", "AvailableQty")%> </th>
                                <%--   <th>Reorder Point </th>--%>
                                <th number><%= GetGlobalResourceObject("Resource", "ReorderPoint")%>  </th>

                            </tr>
                        </thead>
                        <tbody class="">
                            <tr dir-paginate="BLR in BIllingReport|itemsPerPage:25">
                                <td align="center">{{$index +1}}</td>
                               <%-- <td align="center">{{BLR.Location}}</td>--%>
                                <td>{{BLR.PartNumber}}</td>
                                 <td>{{BLR.Description}}</td>
                                <td>{{BLR.Tenant}}</td>
                                <td>{{BLR.Supplier}}</td>
                                <td>{{BLR.MaterialType}}</td>                               
                                <%--<td number>{{BLR.PlannedDeliveryinDays}}</td>--%>
                                <%--<td number>{{BLR.ExpectedUnitCost}}</td>--%>
                                <td number>{{BLR.InitialOrderQty}}</td>
                                <td number>{{BLR.AvailableQty}}</td>
                                <td number>{{BLR.ReorderPoint}}</td>


                            </tr>
                        </tbody>
                        <tfoot>
                            <tr class="">
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
                <td class="lineheight"></td>
            </div>
        </div>
    </div>
</asp:Content>
