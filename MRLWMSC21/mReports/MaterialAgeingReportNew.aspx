<%@ Page Language="C#" Title="Material Ageing Report" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="MaterialAgeingReportNew.aspx.cs" Inherits="MRLWMSC21.mReports.MaterialAgeingReportNew" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
    <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
    <script src="MaterialAgeingReportNew.js"></script>
    <link href="Scripts/Custom.css" rel="stylesheet" />
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#hdnTenantID").val(<%=cp.TenantID%>);
        });
    </script>


    <div class="dashed"></div>
    <div ng-app="myApp" ng-controller="MaterialAgeingReportNew" class="container">
        <div class="divlineheight"></div>
        <input type="hidden" id="hdnTenantID" />
        <div class="row">

            <div class="">
                <div class="">

                    <div class="col m3 s3">
                        <div class="flex">

                             <input type="text" id="txtWarehouse" required=""/>

                          <%--  <select ng-change="changeSelect()" ng-model="ddlwarehouse" id="Warehouse" ng-focus="loadwarehouse()" ng-options="WareHouse.WarehouseID as WareHouse.WHCode for WareHouse in WareHouses" required="">

                                <option value=""><%= GetGlobalResourceObject("Resource", "SelectWarehouse")%> </option>

                            </select>--%>
                            <label>Warehouse</label>
                            <span class="errorMsg"></span>
                        </div>
                    </div>
                        <div class="col m3 s3">
                        <div class="flex">
                              <input type="text" id="ddlZones" required=""/>

                            <%--<select ng-model="ddlZones" id="zone" ng-options="zone.LocationZoneID as zone.LocationZoneCode for zone in zones" required="">

                                <option value=""><%= GetGlobalResourceObject("Resource", "SelectZone")%> </option>

                            </select>--%>
                            <label>Zone</label>
                           <%-- <span class="errorMsg"></span>--%>
                        </div>
                    </div>
                    <div class="col m3 s3">

                        <div class="flex">
                            <input type="text" id="txtTenant" required/>
                            <label><%= GetGlobalResourceObject("Resource", "Tenant")%></label>
                          <%--  <span class="errorMsg"></span>--%>
                        </div>
                    </div>
                   
                    

                     <div class="col m3 s3">
                        <div class="flex">
                            <input type="text" id="txtPartnumber" required />

                            <label><%= GetGlobalResourceObject("Resource", "PartNumber")%></label>
                        </div>
                    </div>
                    <div class="col m3 s3">
                        <div class="flex">
                            <select ng-model="ddlAge" ng-style="{'width': getMaxLength() + 'px'}">
                                <option value="">Select Age (In Days)</option>
                                <option value="1">0-30</option>
                                <option value="2">31-90</option>
                                <option value="3">91-180</option>
                                <option value="4">181-365</option>
                                <option value="5">Above 365</option>
                            </select>
                        </div>
                    </div>
                    <div class="col m9 s12">
                        <gap5></gap5>
                       <flex end> <button type="button" ng-click="Getgedetails(1)" class="btn btn-primary">Search <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                        <a ng-click="exprotData()" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "ExportTo")%><i class="fa fa-file-excel-o" aria-hidden="true"></i></a></flex>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col m12">
                    <table class="table-striped" id="tbldatas">
                        <thead>
                            <tr class="">
                                <th>S. No.</th>
                                <th>MCode</th>
                                <th>Material Desc.</th>
                                <th>Tenant</th>
                                <th>Location</th>
                                <th number>UoM/ Qty.</th>
                                <th number>Available Qty.</th>
                                <th middel>Received Date</th>
                                <th number>Age in Days</th>
                            </tr>
                        </thead>
                        <tbody class="">
                            <tr dir-paginate="age in AgeingData|itemsPerPage:25" total-items="Totalrecords">
                                <td sno>{{$index +1}}</td>
                                <td>{{age.PartNo}}</td>
                                <td>{{age.MDescription}}</td>
                                <td>{{age.TenantName}}</td>
                                <td>{{age.Location}}</td>
                                <%--<td  align="right">{{BLR.UnitPrice}}</td>--%>
                                <td number >{{age.UoM}}</td>
                                <td number>{{age.AvailableQTY}}</td>
                                <td middel>{{age.ReceivedDate}}</td>
                                <td number >{{age.Age}}</td>

                            </tr>
                        </tbody>
                        <tfoot>
                            <tr class="mytableReportFooterTR">
                                <td colspan="15">
                                    <div class="divpaginationstyle">
                                        <dir-pagination-controls direction-links="true" boundary-links="true" on-page-change="Getgedetails(newPageNumber)"> </dir-pagination-controls>
                                    </div>
                                </td>
                            </tr>
                        </tfoot>

                    </table>
                <table id="tbldata"></table>
                <div class="divlineheight"></div>
            </div>
        </div>

    </div>
</asp:Content>
