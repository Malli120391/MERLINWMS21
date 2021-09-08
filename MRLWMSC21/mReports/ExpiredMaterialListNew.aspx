<%@ Page Language="C#" Title="Expired Material Report" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="ExpiredMaterialListNew.aspx.cs" Inherits="MRLWMSC21.mReports.ExpiredMaterialListNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
   <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
    <script src="ExpiredMaterialListNew.js"></script>
    <link href="Scripts/Custom.css" rel="stylesheet" />
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>

   
    <div ng-app="myApp" ng-controller="ExpiredMaterialListNew" class="container">

        <div class="row">

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
                    <%--<span class="errorMsg"></span>--%>
                </div>
            </div>

           

            <div class="col m3">
                <div class="flex">
                    <input type="text" id="txtPartNo" required="" />
                    <label><%= GetGlobalResourceObject("Resource", "PartNo")%></label>
                </div>
            </div>

            <div class="col m2">
                <gap5></gap5>
                <button type="button" ng-click="getExpiryMaterialReport(1)" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Search")%>  <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                <button type="button" class="btn btn-primary" ng-click="exprotData()">Export <i class="fa fa-file-excel-o" aria-hidden="true"></i></button>
            </div>
        </div>
      
       <%-- -------------------------- Remove Unneccesary code from table by durga on 21/05/2018------------------------%>
        <div class="row">
            <div class="col m12">
                    <table class="table-striped" id="tbldatas">
                        <thead>
                            <tr>                               
                                <th>Part No</th>
                                <th>Tenant</th>
                                <th>Material Type</th>
                                <th>Location</th>
                                <th>Available Qty.</th>
                                <th>UoM/Qty. </th>
                                <th>Discrepancy</th>
                                <th>Expired On</th>                             
                            </tr>
                        </thead>
                        <tbody>
                            <tr dir-paginate="exp in expDataList|itemsPerPage:25" total-items="Totalrecords">
                                <%--<td align="center">{{$index +1}}</td>--%>
                                <td>{{exp.MCode}}</td>
                                <td>{{exp.TenantName}}</td>
                                <td>{{exp.MType}}</td>
                                <td>{{exp.Location}}</td>
                                <td>{{exp.AvailableQty}}</td>
                                <td>{{exp.UoM}}</td>
                                <td>{{exp.HasDiscrepancy}}</td>
                                <td align="right">{{exp.ExpiredOn}}</td>                               
                            </tr>
                        </tbody>
                         <tfoot>
                            <tr>
                                <td colspan="15">
                                    <div class="divpaginationstyle">
                                        <dir-pagination-controls direction-links="true" boundary-links="true" on-page-change="getExpiryMaterialReport(newPageNumber)"> </dir-pagination-controls>
                                    </div>
                                </td>
                            </tr>
                        </tfoot>

                    </table>

                <table id="tbldata"></table>
            </div>
            <div class="divlineheight"></div>

        </div>
</asp:Content>

