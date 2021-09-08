<%@ Page Title="Stock Statement Summary" Language="C#" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="StockStatementSummaryNew.aspx.cs" Inherits="MRLWMSC21.mReports.StockStatementSummaryNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {


            $("#txtOpeningdate").datepicker({
                dateFormat: "dd/mm/yy",
                maxDate: new Date(),
                onSelect: function (selected) {
                    $(this).focus();
                    $("#txtClosingdate").datepicker("option", "minDate", selected, { dateFormate: "dd/mm/yy" })
                }
            });
            $("#txtClosingdate").datepicker({
                dateFormat: "dd/mm/yy",
                maxDate: new Date(),
                onSelect: function (selected) {
                    $(this).focus();
                }
            });

            $('#txtOpeningdate, #txtClosingdate').keypress(function () {
                return false;
            });
        });



    </script>

    <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
    <link href="Scripts/Custom.css" rel="stylesheet" />
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
    <script src="StockStatementSummaryNew.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#hdnTenantID").val(<%=cp.TenantID%>);

        });
    </script>
  
    <div ng-app="myApp" ng-controller="StockStatementSummaryNew" class="container">

        <div class="divlineheight"></div>
        <input type="hidden" ng-model="tenantid" id="hdnTenantID" />
        <div ng-show="blockUI">
            <div style="width: 100%; height: 100vh; z-index: 999; position: fixed; top: 0; left: 0; right: 0; bottom: 0; align-items: center; display: flex; justify-content: center;">

                <div style="align-self: center;">
                    <img src="<%= ResolveUrl("~/Images/preloader.svg") %>" loader/>

                </div>

            </div>

        </div>
        <div class="row">
             <div class="col m3">
                        <div class="flex">
                            <input type="text" id="txtWarehouse" required />
                            <label><%= GetGlobalResourceObject("Resource", "WareHouse")%></label>
                             <span class="errorMsg"></span>
                        </div>
                    </div>
                    <div class="col m3">
                        <div class="flex">
                            <input type="text" id="txtTenant" required/>
                            <label><%= GetGlobalResourceObject("Resource", "Tenant")%></label>
                            <span class="errorMsg"></span>
                        </div>
                    </div>
                     
                    <div class="col m3">
                        <div class="flex">
                            <input required type="text" id="txtSKU" />
                            <label>Part#</label>
                        </div>
                    </div>
                    <div class="col m3">
                        <div class="flex">
                            <input type="text" id="txtMaterialType" required/>
                            <label><%=GetGlobalResourceObject("Resource","Material Type") %></label>
                        </div>
                    </div>

        </div>

        <div class="row">
            <div class="col m3">
                <div class="flex">
                    <input type="text" required="" ng-model="Openingdate" id="txtOpeningdate" />
                    <label><%= GetGlobalResourceObject("Resource", "OpeningDate")%> </label>
                </div>
            </div>
            <div class="col m3">
                <div class="flex">
                    <input type="text" required="" ng-model="Closingdate" id="txtClosingdate" />
                    <label><%= GetGlobalResourceObject("Resource", "ClosingDate")%> </label>
                </div>
            </div>
            <div class="col m6">
                <gap5></gap5>
                <flex>  
                            <button type="button" ng-click="Getgedetails(1)" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Search")%> <%=MRLWMSC21Common.CommonLogic.btnfaSearch %></button>
                            <button type="button" class="btn btn-primary" ng-click="exportExcel()"><%= GetGlobalResourceObject("Resource", "Export")%><i class="fa fa-file-excel-o" aria-hidden="true"></i></button>
                        </flex>
            </div>
        </div>

        <div class="row" style="margin: 0;">
            <div class="col-sm-6 col-lg-6" style="margin: 0; padding: 0;">
                <div class="divmainwidth">
                    <table class=" table-striped" id="tbldatas">
                        <thead>
                            <tr class="">
                                <th>Part#</th>
                                <th>Material Description</th>
                                <th number>UoM/Qty.</th>
                                <th number>Opening Stock</th>
                                <th number>Inbound</th>
                                <th number>Outbound</th>
                                <th number>Closing Stock</th>
                                <th number>Stock Difference</th>
                            </tr>
                        </thead>
                        <tbody class="">
                            <tr dir-paginate="STM in StockSummary|itemsPerPage:25" total-items="Totalrecords">
                                <td>{{STM.PartNo}}</td>
                                <td>{{STM.Description}}</td>
                                <td number>{{STM.UOM}}</td>
                                <td number>{{STM.OpeningStock}}</td>
                                <td number>{{STM.Inbound}}</td>
                                <td number>{{STM.Outbound}}</td>
                                <td number>{{STM.ClosingStock}}</td>
                                <td number>{{STM.StockDifference}}</td>
                            </tr>
                        </tbody>
                        
                    </table>
                    <br />
                     <div flex end><dir-pagination-controls direction-links="true" boundary-links="true" on-page-change="Getgedetails(newPageNumber)"></dir-pagination-controls></div>
                    <br />
                </div>

                <table id="tbldata"></table>
                <td class="lineheight"></td>
            </div>
        </div>
    </div>
</asp:Content>
