<%@ Page Title=" Bin Replenishment Report :." Language="C#" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="BinReplenishmentReport.aspx.cs" Inherits="MRLWMSC21.mReports.BinReplenishmentReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
    <style>
        .mytableReportBodyTR td {
            text-align: center;
            vertical-align: middle;
        }
    </style>

    <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
    <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />
    <script src="BinReplenishment.js"></script>
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
    
    <div ng-app="myApp" ng-controller="BinReplenishment" class="container">

        <div class="">
            <div class="row">
                 <div class="col m3">
                        <div class="flex">
                            <input type="text" id="txtWarehouse" required="" />
                            <label><%= GetGlobalResourceObject("Resource", "WareHouse")%></label>
                          <%--  <span class="errorMsg"></span>--%>
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
                        <input type="text" id="txtItemNo" ngchange="getskus()" ng-focus="getskus()" required="" />
                        <label><%= GetGlobalResourceObject("Resource", "PartNumber")%></label>
                    </div>
                </div>
                <div class="col m2 s3">
                    <gap5></gap5>
                    <button type="button" ng-click="Getbindetails()" class="btn btn-primary" ng-click="exportExcel()"><%= GetGlobalResourceObject("Resource", "Search")%>  <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>

                    <a ng-click="exportExcel()" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Export")%><i class="fa fa-file-excel-o" aria-hidden="true"></i></a>
                </div>
            </div>
        </div>

        <div class="row" style="margin: 0;">
            <div class="col-sm-12 col-lg-12" style="margin: 0; padding: 0;">
                <div class="divmainwidth">

                    <table class=" table-striped" id="tbldatas">
                        <thead>
                            <tr class="">
                                <%--   <th style="width:150px;">Item No</th>--%>
                                <th style="width: 150px;"><%= GetGlobalResourceObject("Resource", "PartNumber")%> </th>
                                <th align="center">
                                    <table class=" table-striped borderless">
                                        <thead>
                                            <tr>
                                                <%-- <th style="width:15%;">Suggested Location</th>--%>
                                                <th style="width: 15%;"><%= GetGlobalResourceObject("Resource", "SuggestedLocation")%> </th>
                                                <%--     <th style="width:15%;">Min Qty.</th>   --%>
                                                <th style="width: 10%;"><%= GetGlobalResourceObject("Resource", "MinQty")%></th>
                                                <%--  <th style="width:10%;">Max Qty.</th>  --%>
                                                <th style="width: 8%;"><%= GetGlobalResourceObject("Resource", "MaxQty")%> </th>
                                                <%--<th style="width:10%;">Available Qty.</th>--%>
                                                <th style="width: 13%;"><%= GetGlobalResourceObject("Resource", "AvailableQty")%> </th>

                                            </tr>
                                        </thead>
                                    </table>
                                </th>
                            </tr>
                        </thead>
                        <tbody class="mytableReportBodyTR table borderless">
                            <tr ng-repeat="BDR in BinDetailsReport" class="trPO1 mytableOutboundBodyTR">
                                <td align="center"><a style="text-decoration: none; color: blue !important;" id="{{ BDR.MaterialMasterID }}" ng-click="openDialog(BDR.MaterialmasterId)" href="">{{ BDR.ItemNo }} </a></td>
                                <td align="center">
                                    <table class="mytablechildOutbound" style="width: 100%; vertical-align: middle; margin-top: 5px; margin-left: 0% !important;">
                                        <tbody>
                                            <tr style="background-color: #a0c1f1 !important; height: 27px !important;" ng-repeat="BinInfo in BDR.oBinDetailsListlst">
                                                <td style="width: 15%;">{{ BinInfo.SuggLoc }}</td>
                                                <td style="width: 10%;">{{ BinInfo.MinQty }}</td>
                                                <td style="width: 8%;">{{ BinInfo.MaxQty }}</td>
                                                <td style="width: 13%;">{{ BinInfo.BinRepQty }}</td>

                                            </tr>
                                        </tbody>
                                    </table>
                                    <div class="lineheight"></div>
                                </td>


                            </tr>

                        </tbody>

                    </table>



                </div>
                <table id="tbldata"></table>
            </div>
        </div>

        <div id="divContainer" class="PopupContainerInbound">
            <div id="divInner" class="PopupInnerOutbound">
                <%--  <div class="PopupHeadertextOutbound">Bin Details</div>--%>
                <div class="PopupHeadertextOutbound"><%= GetGlobalResourceObject("Resource", "BinDetails")%></div>
                <span id="spanClose" class="fa fa-times PopupSpanCloseOutbound" aria-hidden="true"></span>&emsp;
                        <div class="PopupPaddingOutbound">
                            <div class="PopupSpaceOutbound">
                                <table class="tablestyle mytableOutbound" style="text-align: center;">
                                    <thead class="mytableReportItemsHeaderTR">
                                        <tr class="mytableReportHeaderTR">
                                            <%--<th>Storage Location</th>--%>
                                            <th><%= GetGlobalResourceObject("Resource", "StorageLocation")%> </th>
                                            <%-- <th>Bin Location</th>--%>
                                            <th><%= GetGlobalResourceObject("Resource", "BinLocation")%> </th>
                                            <%--<th>Qty.</th>--%>
                                            <th><%= GetGlobalResourceObject("Resource", "Qty")%> </th>
                                            <%-- <th>UoM</th>--%>
                                            <th><%= GetGlobalResourceObject("Resource", "UoMQty")%></th>
                                        </tr>
                                    </thead>
                                    <tbody class="mytableReportBodyTR">
                                      <%--  <tr ng-repeat=" BPL in BinPopUpDetails ">--%>
                                            <tr dir-paginate="BPL in BinPopUpDetails|itemsPerPage:10">
                                            <td>{{ BPL.StorageLocation }}</td>
                                            <td>{{ BPL.Location }}</td>
                                            <td>{{ BPL.Quantity }}</td>
                                            <td style="text-align: center !important;">{{ BPL.UoM }}</td>
                                        </tr>
                                          <tr >
                                           <td colspan="4" style="color: red;text-align:center; font-weight: bold" ng-show="BinPopUpDetails=='undefined' ||BinPopUpDetails=='null' || BinPopUpDetails.length==0 ">
                                            No Records Found.                
                                           </td>
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
                        </div>
            </div>
        </div>
    </div>

    </div>
</asp:Content>
