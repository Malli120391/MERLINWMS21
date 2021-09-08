<%@ Page Language="C#" MasterPageFile="~/mReports/mReport.master" AutoEventWireup="true" CodeBehind="DemandForecastingReport.aspx.cs" Inherits="MRLWMSC21.mReports.DemandForecastingReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
    <script src="../Scripts/angular.min.js"></script>
    <script src="Scripts/dirPagination.js"></script>
    <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
    <link href="../mInventory/Scripts/bootstrap.min.css" rel="stylesheet" />
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>


    <script src="DemandForecastingReport.js"></script>

    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <link rel="stylesheet" href="/resources/demos/style.css">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script src="../Scripts/inputValidation.js"></script>
    <link href="Charts/animate.css" rel="stylesheet" />

    <script src="Charts/Chart.min.js"></script>

    <style>
        .ui-widget-header {
            padding-top: unset !important;
            min-height: unset !important;
        }

        #slider-range-min {
            width: 10%;
        }
    </style>
    <div ng-app="MyApp" ng-controller="FormCtrl">
        <div class="container">
            <div class="row">
                <div class="col s4 m2">
                    <div class="flex">
                        <input type="text" id="txtWarehouse" required="" />
                        <label>Warehouse</label>

                    </div>
                </div>

                <div class="col s4 m2">
                    <div class="flex">
                        <input type="text" id="txtTenant" required="" />
                        <label>Tenant</label>

                    </div>
                </div>
                <div class="col m2">
                    <div class="flex">
                        <input type="text" id="txtMcode" required="" />
                        <label>MCode</label>

                    </div>


                </div>
            </div>
            <div class="row">
                <div class="col m12">
                    <label for="amount">Forecast For (Year):</label>
                    <input type="text" id="amount" readonly style="border: 0; color: #f6931f; font-weight: bold;"><br />
                    <br />
                    <div id="slider-range-min"></div>
                </div>


            </div>
            <div class="row">
                <div class="col m9">
                        <div class="divblockstyle" >

                            <canvas id="LineChartPercentage" height="150"></canvas>
                        </div>
                    </div>
                <div class="col m3">
                    <table class="table-striped">
                    <thead>
                        <tr>
                            <th>S.No</th>
                            <th>SKU</th>
                            <th>Predicted Date</th>
                            <th>Qty.</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr dir-paginate="row in BillingData|itemsPerPage:25">
                            <td>{{$index+1}}</td>
                            <td>{{row.SKU}}</td>
                            <td>{{row['Expression.$TIME'] | date : 'dd-MMM-yyyy'}}</td>
                            <td>{{row['Expression.QUANTITY']}}</td>

                        </tr>
                    </tbody>

                </table>
                <div style="float: right !important; font-family: Arial; font-size: small; margin-right: 1%;">
                <dir-pagination-controls direction-links="true" boundary-links="true" > </dir-pagination-controls>
                </div>


            </div>

            <div class="row">
                <%--<table class="table-striped">
                    <thead>
                        <tr>
                            <th>S.No</th>
                            <th>SKU</th>
                            <th>Predicted Date</th>
                            <th>Qty.</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr dir-paginate="row in BillingData|itemsPerPage:25">
                            <td>{{$index+1}}</td>
                            <td>{{row.SKU}}</td>
                            <td>{{row['Expression.$TIME'] | date : 'dd-MMM-yyyy'}}</td>
                            <td>{{row['Expression.QUANTITY']}}</td>

                        </tr>
                    </tbody>

                </table>
                <div style="float: right !important; font-family: Arial; font-size: small; margin-right: 1%;">
                <dir-pagination-controls direction-links="true" boundary-links="true" > </dir-pagination-controls>--%>
            </div>
        </div>



    </div>

</asp:Content>
