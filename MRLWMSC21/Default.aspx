<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MRLWMSC21.mReports.Default" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <link href="CSS/bootstrap.min.css" rel="stylesheet" />
    <link href="mReports/Charts/animate.css" rel="stylesheet" />
    <script src="mInventory/Scripts/angular.min.js"></script>
    <script src="mInventory/Scripts/dirPagination.js"></script>
    <script src="mMisc/Scripts/bootstrap.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.0/moment.min.js"></script>
    <script src="mReports/Charts/Chart.min.js"></script>

    <%-- <script src="Scripts/cookie.min.js"></script>--%>

    <%--    <script src="Charts/highcharts.js"></script>
    <script src="Charts/highcharts-3d.js"></script>
    <script src="Charts/exporting.js"></script>--%>
    <style>
        #lblWelcome {
            min-width: 160px;
            margin-left: -125px;
            background-color: #0e0e0e;
            color: #fff;
            text-align: center;
            border-radius: 2px;
            padding: 16px;
            position: fixed;
            z-index: 1;
            border-radius: 30px;
            left: calc(50% + 80px);
            bottom: -40px;
            font-size: 17px;
            animation: shimmy 2s cubic-bezier(1, 0.38, 1, 1);
            box-shadow: var(--z1);
        }

        .TextOrder {
            margin-bottom: 5px;
            margin-top: 0px;
        }

        @keyframes shimmy {
            0% {
                transform: translateY(0%);
            }

            30% {
                transform: translateY(-200%);
            }

            50% {
                transform: translateY(-200%);
            }

            80% {
                transform: translateY(-200%);
            }

            100% {
                transform: translateY(0%);
            }
        }

        #lblWelcome:empty {
            display: none !important;
        }
    </style>
    <%--  <script src="Scripts/inputValidation.js"></script>--%>

    <script>

        var app = angular.module('MyApp', []);

        app.controller('Dashboards', function ($scope, $http) {
            //alert('TP');
            $scope.dataList = [];
            Whid = 0;
            var TextFieldName = $("#txtWareHouse");
            DropdownFunction(TextFieldName);
            $("#txtWareHouse").autocomplete({
                source: function (request, response) {
                    //debugger;
                    $.ajax({
                   <%--     url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadWarehouseForCyccleCount") %>',--%>
                       <%-- url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadWarehouse") %>',
                        data: "{ 'prefix': '" + request.term + "','AccountID':'" + <%=this.cp.AccountID%> + "'}",--%>
                        url:  '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoaDWHListBasedonUser") %>',
                        data: "{ 'prefix': '" + request.term + "'  }",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            $scope.blockUI = false;
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split(',')[0],
                                    val: item.split(',')[1]
                                }
                            }))
                        }
                    });
                },
                select: function (e, i) {
                    debugger
                    $("#WarehouseID").val(i.item.val);
                    $scope.gePercentageCharts(i.item.val);

                },
                minLength: 0
            });

            $scope.GetWarehouse = function () {
                //debugger;
                var httpreq = {
                    method: 'POST',
                    url: 'Default.aspx/GetWarehouses',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'AccountID': <%=this.cp.AccountID%> },
                    async: false
                }
                $http(httpreq).success(function (response) {
                    //debugger;
                    $scope.blockUI = false;
                    var dt = JSON.parse(response.d).Table;
                    if (dt != null && dt.length > 0) {
                        var WHID = dt[0].WarehouseID;
                        $scope.gePercentageCharts(WHID);
                    }
                });
            }
            $scope.GetWarehouse();

            var stockChart;
            var locationChart;
            var inbEffChart;
            var obdEffChart;
            var outwardORDChart;

            $scope.gePercentageCharts = function (Whid) {

                var cityid = 0;
                var httpreq = {
                    method: 'POST',
                    url: 'Default.aspx/GetPercentageReportData',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'WareHouseID': Whid },
                    async: false
                }
                $http(httpreq).success(function (response) {
                    debugger;
                    $scope.blockUI = false;
                    $scope.dataList = JSON.parse(response.d);



                    //================== Counts ===================//

                    $scope.POCount = $scope.dataList.Table[0].PO_Count || 0;
                    $scope.SOCount = $scope.dataList.Table1[0].SO_COUNT || 0;

                    $scope.POCountOpen = $scope.dataList.Table2[0].PO_Count_Open || 0;
                    $scope.SOCountOpen = $scope.dataList.Table3[0].SO_COUNT_Open || 0;

                    $scope.Cus_Return = $scope.dataList.Table6[0].CUSTOMERRETURN_COUNT || 0;
                    $scope.Sup_Return = $scope.dataList.Table7[0].SUPPLIERRETURN_COUNT || 0;

                    $scope.Cancel_SO = $scope.dataList.Table8[0].CancelledSo_Count || 0;
                    $scope.ItemCount = $scope.dataList.Table9[0].ITEM_COUNT || 0;

                    var objs = $scope.dataList.Table11;

                    var WHWeightpieData = {
                        labels: ["Lost ", "OK", "Damage", "IBOH", "OBOH", "INSP"],
                        //labels: ["Lost ", "OK ","Exp.","IBOH","OBOH","QCOH"],
                        datasets: [{
                            //data: [objs[0].Lost, objs[0].OK, objs[0].Exp, objs[0].IBOH, objs[0].OBOH, objs[0].QCOH], commented by lalitha on 25/06/2019 to remove the QCOH
                            //  backgroundColor: ["#f99522", "#03c450", "#f43202", "#009ccc","#f9f509","#CCCCCC"],
                            data: [objs[0].LNF, objs[0].OK, objs[0].DAMAGE, objs[0].IBOH, objs[0].OBOH, objs[0].INSP],
                            backgroundColor: ["#f99522", "#03c450", "#f43202", "#009ccc", "#f9f509", "#CCC"],
                        }]
                    };

                    var WHWeightpieOptions = {
                        responsive: true
                    };

                    var ctxStock = document.getElementById("divPieChartStock").getContext("2d");
                    if (stockChart != undefined) {
                        stockChart.destroy();
                    }
                    stockChart = new Chart(ctxStock, {
                        type: 'doughnut', data: WHWeightpieData, options: {
                            legend: {
                                display: true,
                                position: 'right',
                                responsive: true,
                                fullWidth: false,
                                labels: {
                                    boxWidth: 15,
                                    fontColor: "black",
                                    fontFamily: "verdana",
                                    fontWeight: 'bold'
                                }
                            }
                        }
                    })



                    //==================== END ====================//


                    //================== Bin Status ================//
                    
                    var obj = $scope.dataList.Table12;
                    //var objs = $scope.dataList.Table4;
                    var BinData = {
                        labels: ["Empty ", "Occupied "],
                        datasets: [{
                            data: [obj[0].NoOfEmptyLocations, obj[0].NoOfOccupiedLocations],
                            backgroundColor: ["#f99522", "#009ccc"],
                        }]
                    };
                    var BinOptions = {
                        responsive: true
                    };
                    var ctxLocation = document.getElementById("TotalBinpiechart").getContext("2d");
                    if (locationChart != undefined) {
                        locationChart.destroy();
                    }
                    locationChart = new Chart(ctxLocation, {
                        type: 'doughnut', data: BinData, options: {
                            legend: {
                                display: true,
                                position: 'right',
                                responsive: true,
                                fullWidth: false,
                                labels: {
                                    boxWidth: 15,
                                    fontColor: "black",
                                    fontFamily: "verdana",
                                    fontWeight: 'bold'
                                }
                            }
                        }
                    });

                    //================== END =======================//

                    //================================= Line Chart FOR OTIF ======================================//

                    var InboundDays = [];
                    var InboundHours = [];

                    for (i = 0; i < $scope.dataList.Table4.length; i++) {
                        InboundDays.push($scope.dataList.Table4[i].DayInDATE);
                        InboundHours.push($scope.dataList.Table4[i].Inb_Count);
                    }

                    var OutboundDays = [];
                    var OutboundHours = [];

                    for (i = 0; i < $scope.dataList.Table5.length; i++) {
                        OutboundDays.push($scope.dataList.Table5[i].DayInDATE);
                        OutboundHours.push($scope.dataList.Table5[i].OBD_Count);
                    }

                    var OutwardOrderDays = [];
                    var OutwardOrderHours = [];
                    //for (i = 0; i < $scope.dataList.Table4.length; i++) {
                    //    OutwardOrderDays.push($scope.dataList.Table10[i].DayInDATE);
                    //    OutwardOrderHours.push($scope.dataList.Table10[i].Inb_Count);
                    //}
                    OutwardOrderDays = ["Yesterday Pending SO's", "Created SO's", "Delivered SO's", "Pending SO Till Date", "Total Pending SO's", "Today Pending Order"];
                    OutwardOrderHours = [$scope.dataList.Table10[0].YESTERDAY_PENDINGSO, $scope.dataList.Table10[0].CREATEDSO, $scope.dataList.Table10[0].DELIVEREDSO, $scope.dataList.Table10[0].PENDINGSOTILLDATE, $scope.dataList.Table10[0].TotalPendingSO, $scope.dataList.Table10[0].TODAY_PendingOrder]


                    var lineData = {
                        labels: eval(InboundDays),
                        datasets: [

                            {
                                label: "Days",
                                // backgroundColor:"#20cc95",
                                backgroundColor: 'rgba(26,179,148,0.5)',
                                borderColor: "rgba(26,179,148,0.7)",
                                pointBackgroundColor: "rgba(26,179,148,1)",
                                pointBorderColor: "rgba(26,179,148,1)",
                                data: InboundHours
                            }
                        ]
                    };

                    var lineOptions = {
                        responsive: true
                    };

                    var ctxINB = document.getElementById("LineChartForInbound").getContext("2d");
                    if (inbEffChart != undefined) {
                        inbEffChart.destroy();
                    }
                    inbEffChart = new Chart(ctxINB, {
                        type: 'line', data: lineData, options: {
                            legend: {
                                display: true,
                                position: 'bottom',
                                responsive: true,
                                fullWidth: false,
                                labels: {
                                    boxWidth: 15,
                                    fontColor: "black",
                                    fontFamily: "verdana",
                                    fontWeight: 'bold'
                                }
                            },
                            scales: {
                                yAxes: [{
                                    scaleLabel: {
                                        display: true,
                                        labelString: 'No. of Inbounds',
                                        fontColor: 'black',
                                        fontFamily: "verdana",
                                        fontWeight: 'bold',
                                        fontSize: 15
                                    },
                                    ticks: {
                                        beginAtZero: true,
                                        fontColor: 'black',
                                        fontFamily: "verdana",
                                        fontWeight: 'bold'
                                    },
                                }],
                                //xAxes: [{
                                //    scaleLabel: {
                                //        display: true,
                                //        labelString: 'Days',
                                //        fontColor: 'black',
                                //        fontFamily: "verdana",
                                //        fontWeight: 'bold',
                                //        fontSize: 18
                                //    },
                                //    ticks: {
                                //        fontColor: 'black',
                                //        fontFamily: "verdana",
                                //        fontWeight: 'bold'
                                //    },
                                //}]
                            }
                        }
                    });

                    var lineData = {
                        labels: eval(OutboundDays),
                        datasets: [

                            {
                                label: "Days",
                                // backgroundColor:"#20cc95",
                                backgroundColor: "rgba(249, 103, 12,0.5)", //'rgba(249, 152, 67,0.5)',
                                borderColor: "rgba(249, 103, 12,0.5)",
                                pointBackgroundColor: "rgba(249, 103, 12,0.5)",
                                pointBorderColor: "rgba(249, 103, 12,0.5)",
                                data: OutboundHours
                            }
                        ]
                    };

                    var lineOptions = {
                        responsive: true
                    };

                    var ctxOBD = document.getElementById("LineChartForOutbound").getContext("2d");
                    if (obdEffChart != undefined) {
                        obdEffChart.destroy();
                    }
                    obdEffChart = new Chart(ctxOBD, {
                        type: 'line', data: lineData, options: {
                            legend: {
                                display: true,
                                position: 'bottom',
                                responsive: true,
                                fullWidth: false,
                                labels: {
                                    boxWidth: 15,
                                    fontColor: "black",
                                    fontFamily: "verdana",
                                    fontWeight: 'bold'
                                }
                            },
                            scales: {
                                yAxes: [{
                                    scaleLabel: {
                                        display: true,
                                        labelString: 'No. of Outbounds',
                                        fontColor: 'black',
                                        fontFamily: "verdana",
                                        fontWeight: 'bold',
                                        fontSize: 15
                                    },
                                    ticks: {
                                        beginAtZero: true,
                                        fontColor: 'black',
                                        fontFamily: "verdana",
                                        fontWeight: 'bold'
                                    },
                                }],
                                //xAxes: [{
                                //    scaleLabel: {
                                //        display: true,
                                //        labelString: 'Days',
                                //        fontColor: 'black',
                                //        fontFamily: "verdana",
                                //        fontWeight: 'bold',
                                //        fontSize: 18
                                //    },
                                //    ticks: {
                                //        fontColor: 'black',
                                //        fontFamily: "verdana",
                                //        fontWeight: 'bold'
                                //    },
                                //}]
                            }
                        }
                    });

                    var lineData = {
                        labels: eval(OutwardOrderDays),
                        datasets: [

                            {
                                label: "Hours",
                                backgroundColor: '#6a91e6',
                                borderColor: "#4f91f9",
                                // backgroundColor:"#20cc95",
                                //backgroundColor: 'rgba(26,179,148,0.5)',
                                //borderColor: "rgba(26,179,148,0.7)",
                                //backgroundColor: [
                                //    "#7fccbc", "#fff5dd", "#ffe0e6", "#ffecd9", "d7ecfb",
                                //    "#7fccbc", "#fff5dd", "#ffe0e6", "#ffecd9", "d7ecfb",
                                //    "#7fccbc", "#fff5dd", "#ffe0e6", "#ffecd9", "d7ecfb",
                                //    "#7fccbc", "#fff5dd", "#ffe0e6", "#ffecd9", "d7ecfb",
                                //    "#7fccbc", "#fff5dd", "#ffe0e6", "#ffecd9", "d7ecfb",
                                //    "#7fccbc", "#fff5dd", "#ffe0e6", "#ffecd9", "d7ecfb"                                   
                                //],
                                //borderColor: [
                                //    "#1ab394", "#fbedca", "#f3b4c0", "#f5d7b9", "#b0d1e8",
                                //    "#1ab394", "#fbedca", "#f3b4c0", "#f5d7b9", "#b0d1e8",
                                //    "#1ab394", "#fbedca", "#f3b4c0", "#f5d7b9", "#b0d1e8",
                                //    "#1ab394", "#fbedca", "#f3b4c0", "#f5d7b9", "#b0d1e8",
                                //    "#1ab394", "#fbedca", "#f3b4c0", "#f5d7b9", "#b0d1e8",
                                //    "#1ab394", "#fbedca", "#f3b4c0", "#f5d7b9", "#b0d1e8"                                    
                                //],
                                pointBackgroundColor: "rgba(26,179,148,1)",
                                pointBorderColor: "rgba(26,179,148,1)",
                                data: OutwardOrderHours
                            }
                        ]
                    };

                    var lineOptions = {
                        responsive: true
                    };


                    $scope.OutwardList = $scope.dataList.Table13;
                    //var ctx = document.getElementById("LineChartForOrders").getContext("2d");
                    //new Chart(ctx, {
                    //    type: 'bar', data: lineData,
                    //    options: {
                    //        legend: {
                    //            display: true,
                    //            position: 'bottom',
                    //            responsive: true,
                    //            fullWidth: false,
                    //            labels: {
                    //                boxWidth: 15,
                    //                fontColor: "black",
                    //                fontFamily: "verdana",
                    //                fontWeight: 'bold'
                    //            }
                    //        },
                    //        scales: {
                    //            yAxes: [{
                    //                scaleLabel: {
                    //                    display: true,
                    //                    labelString: 'No. Of Orders',
                    //                    fontColor: 'black',
                    //                    fontFamily: "verdana",
                    //                    fontWeight: 'bold',
                    //                    fontSize: 15
                    //                },
                    //                ticks: {
                    //                    beginAtZero: true,
                    //                    fontColor: 'black',
                    //                    fontFamily: "verdana",
                    //                    fontWeight: 'bold'
                    //                },
                    //            }],
                    //            xAxes: [{
                    //                scaleLabel: {
                    //                    display: true,
                    //                    labelString: 'Hours',
                    //                    fontColor: 'black',
                    //                    fontFamily: "verdana",
                    //                    fontWeight: 'bold',
                    //                    fontSize: 15
                    //                },
                    //                ticks: {
                    //                    fontColor: 'black',
                    //                    fontFamily: "verdana",
                    //                    fontWeight: 'bold'
                    //                }
                    //            }]
                    //        }
                    //    }
                    //});


                    //================================= Line Chart ======================================//
                    //OutwardOrderDays = ["Yesterday Pending SO's", "Created SO's", "Delivered SO's", "Pending SO Till Date", "Total Pending SO's", "Today Pending Order"];
                    //OutwardOrderHours = [$scope.dataList.Table10[0].YESTERDAY_PENDINGSO, $scope.dataList.Table10[0].CREATEDSO, $scope.dataList.Table10[0].DELIVEREDSO, $scope.dataList.Table10[0].PENDINGSOTILLDATE, $scope.dataList.Table10[0].TotalPendingSO, $scope.dataList.Table10[0].TODAY_PendingOrder]
                    var config = {
                        type: 'line',
                        data: {
                            labels: ['01:00', '02:00', '03:00', '04:00', '05:00',
                                '06:00', '07:00', '08:00', '09:00', '10:00', '11:00',
                                '12:00', '13:00', '14:00', '15:00', '16:00', '17:00', '18:00','19:00','20:00','21:00','22:00', '23:00','24:00'],
                            datasets: [
                                //{
                                //    label: "My First dataset",
                                //    data: [$scope.dataList.Table10[0].YESTERDAY_PENDINGSO, $scope.dataList.Table10[0].CREATEDSO, $scope.dataList.Table10[0].DELIVEREDSO, $scope.dataList.Table10[0].PENDINGSOTILLDATE, $scope.dataList.Table10[0].TotalPendingSO, $scope.dataList.Table10[0].TODAY_PendingOrder]//[1, 3, 4, 2, 1, 4, 2],
                                //}
                                {
                                    label: "Number of SO's",
                                    backgroundColor: "rgba(249, 103, 12,0.5)", //'rgba(249, 152, 67,0.5)',
                                    borderColor: "rgba(249, 103, 12,0.5)",
                                    //data: [parseInt($scope.dataList.Table10[0].YESTERDAY_PENDINGSO),0,0,0,0,0]
                                    data: [$scope.OutwardList[0].SO_count,
                                        $scope.OutwardList[1].SO_count,
                                        $scope.OutwardList[2].SO_count,
                                        $scope.OutwardList[3].SO_count,
                                        $scope.OutwardList[4].SO_count,
                                        $scope.OutwardList[5].SO_count,
                                        $scope.OutwardList[6].SO_count,
                                        $scope.OutwardList[7].SO_count,
                                        $scope.OutwardList[8].SO_count,
                                        $scope.OutwardList[9].SO_count,
                                        $scope.OutwardList[10].SO_count,
                                        $scope.OutwardList[11].SO_count,
                                        $scope.OutwardList[12].SO_count,
                                        $scope.OutwardList[13].SO_count,
                                        $scope.OutwardList[14].SO_count, $scope.OutwardList[15].SO_count, $scope.OutwardList[16].SO_count,
                                        $scope.OutwardList[17].SO_count, $scope.OutwardList[18].SO_count, $scope.OutwardList[19].SO_count,
                                        $scope.OutwardList[20].SO_count, $scope.OutwardList[21].SO_count, $scope.OutwardList[22].SO_count,
                                        $scope.OutwardList[23].SO_count]
                                },
                                //{
                                //    label: "Inprogress SO's",
                                //    backgroundColor: 'rgba(26,179,148,0.5)',
                                //    borderColor: "rgba(26,179,148,0.7)",
                                //    //data: [0,parseInt($scope.dataList.Table10[0].CREATEDSO),0,0,0,0]
                                //    data: [30, 100, 20, 50, 80, 200, 30, 100,
                                //        20, 50, 80, 200, 30, 100, 20, 50, 80,
                                //        200, 30, 100, 20, 50, 80, 200]
                                //},
                                //{
                                //    label: "Delivered SO's",
                                //    backgroundColor: "rgba(0, 0, 0,0)",
                                //    //backgroundColor: 'rgba(79, 145, 249,0.5)',
                                //    borderColor: "#03c450",
                                //    //data: [0,0,parseInt($scope.dataList.Table10[0].DELIVEREDSO),0,0,0]
                                //    data: [10,50,40,0,80,100,10,50,40,0,80,100,10,50,40,0,80,100,10,50,40,0,80,100]
                                //}
                                //,
                                //{
                                //    label: "Pending SO Till Date",
                                //    //backgroundColor: 'rgba(79, 145, 249,0.5)',
                                //    backgroundColor: "rgba(0, 0, 0,0)",
                                //    borderColor: "#f9f509",
                                //    //data: [0,0,0,parseInt($scope.dataList.Table10[0].PENDINGSOTILLDATE),0,0]
                                //    data: [20,150,20,70,60,150,20,150,20,70,60,150,20,150,20,70,60,150,20,150,20,70,60,150]
                                //}
                                //,
                                //{
                                //    label: "Total Pending SO's",
                                //    //backgroundColor: 'rgba(79, 145, 249,0.5)',
                                //    backgroundColor: "rgba(0, 0, 0,0)",
                                //    borderColor: "#173caf",
                                //    //data: [0,0,0,0,parseInt($scope.dataList.Table10[0].TotalPendingSO),0]
                                //    data: [50,0,20,70,80,140,50,0,20,70,80,140,50,0,20,70,80,140,50,0,20,70,80,140]
                                //}
                                //,
                                //{
                                //    label: "Today Pending Order",
                                //    //backgroundColor: 'rgba(79, 145, 249,0.5)',
                                //    backgroundColor: "rgba(0, 0, 0,0)",
                                //    borderColor: "#f33408",
                                //    //data: [0,0,0,0,0,parseInt($scope.dataList.Table10[0].TODAY_PendingOrder)]
                                //    data: [150,10,0,50,130,90,150,10,0,50,130,90,150,10,0,50,130,90,150,10,0,50,130,90]
                                //}
                            ]
                        },
                        options: {
                            scales: {
                                yAxes: [{
                                    scaleLabel: {
                                        display: true,
                                        labelString: 'No. of Orders',
                                        fontColor: 'black',
                                        fontFamily: "verdana",
                                        fontWeight: 'bold',
                                        fontSize: 15
                                    },
                                    ticks: {
                                        beginAtZero: true,
                                        fontColor: 'black',
                                        fontFamily: "verdana",
                                        fontWeight: 'bold'
                                    },
                                }],
                                xAxes: [{
                                    scaleLabel: {
                                        display: true,
                                        labelString: 'Hours',
                                        fontColor: 'black',
                                        fontFamily: "verdana",
                                        fontWeight: 'bold',
                                        fontSize: 15
                                    },
                                    type: 'time',
                                    time: {
                                        format: "HH:mm",
                                        unit: 'hour',
                                        unitStepSize: 1,
                                        displayFormats: {
                                            'minute': 'HH:mm',
                                            'hour': 'HH:mm',
                                            min: '00:00',
                                            max: '23:59'
                                        },
                                    }
                                }],
                            },
                        }
                    };
                    var ctxOutward = document.getElementById("LineChartForOrders").getContext("2d");
                    if (outwardORDChart != undefined) {
                        outwardORDChart.destroy();
                    }
                    outwardORDChart = new Chart(ctxOutward,config );
                    //new Chart(ctx, {
                    //    type: 'line', data: data,
                    //    options: options
                    //    });
                    //================ END ==================//
                    
                });                   
            };
        });




    </script>



    <script>

        var app = angular.module('MyAppOLD', []);

        app.controller('DashboardsOLD', function ($scope, $http) {



            //alert('TP');
            $scope.dataList = [];
            Whid = 0;
            var TextFieldName = $("#txtWareHouse");
            DropdownFunction(TextFieldName);
            $("#txtWareHouse").autocomplete({
                source: function (request, response) {
                    $.ajax({
                   <%--     url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadWarehouseForCyccleCount") %>',--%>
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadWarehouse") %>',
                        data: "{ 'prefix': '" + request.term + "','AccountID':'" + <%=this.cp.AccountID%> + "'}",//<=cp.TenantID%>
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {

                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split(',')[0],
                                    val: item.split(',')[1]
                                }
                            }))
                        }
                    });
                },
                select: function (e, i) {
                    debugger
                    $("#WarehouseID").val(i.item.val);
                    $scope.gePercentageCharts(i.item.val);

                },
                minLength: 0
            });

            $scope.GetWarehouse = function () {
                //debugger;
                var httpreq = {
                    method: 'POST',
                    url: 'Default.aspx/GetWarehouses',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'AccountID': <%=this.cp.AccountID%> },
                    async: false
                }
                $http(httpreq).success(function (response) {
                    //debugger;
                    var dt = JSON.parse(response.d).Table;
                    if (dt != null && dt.length > 0) {
                        var WHID = dt[0].WarehouseID;
                        $scope.gePercentageCharts(WHID);
                    }
                });
            }
            $scope.GetWarehouse();

            $scope.gePercentageCharts = function (Whid) {

                var cityid = 0;
                var httpreq = {
                    method: 'POST',
                    url: 'Default.aspx/GetPercentageReportData',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'WareHouseID': Whid },
                    async: false
                }
                $http(httpreq).success(function (response) {
                    //debugger;
                    $scope.dataList = JSON.parse(response.d);




                    //================================= Line Chart FOR OTIF ======================================//

                    var OrderFillRateMonths = [];
                    var OrderFillRatePercent = [];

                    for (i = 0; i < $scope.dataList.Table.length; i++) {
                        OrderFillRateMonths.push($scope.dataList.Table[i].Monthname);
                        OrderFillRatePercent.push($scope.dataList.Table[i].Percentage);
                    }

                    var OntimeDelMonths = [];
                    var OntimeDelPercent = [];

                    for (i = 0; i < $scope.dataList.Table1.length; i++) {
                        OntimeDelMonths.push($scope.dataList.Table1[i].Monthname);
                        OntimeDelPercent.push($scope.dataList.Table1[i].Percentage);
                    }

                    var AvgCycleTimeMonth = [];
                    var AvgCycleTimeQty = [];
                    for (i = 0; i < $scope.dataList.Table2.length; i++) {
                        AvgCycleTimeMonth.push($scope.dataList.Table2[i].Monthname);
                        AvgCycleTimeQty.push($scope.dataList.Table2[i].Percentage);
                    }

                    var lineData = {
                        labels: eval(OrderFillRateMonths),
                        datasets: [

                            {
                                label: "OTIF",
                                // backgroundColor:"#20cc95",
                                backgroundColor: 'rgba(26,179,148,0.5)',
                                borderColor: "rgba(26,179,148,0.7)",
                                pointBackgroundColor: "rgba(26,179,148,1)",
                                pointBorderColor: "rgba(26,179,148,1)",
                                data: OrderFillRatePercent
                            },

                            {
                                label: "Order Accuracy",
                                //backgroundColor:"#43cbf9",
                                backgroundColor: 'rgba(79, 145, 249,0.5)',
                                borderColor: "rgba(79, 145, 249,0.7)",
                                pointBackgroundColor: "rgba(79, 145, 249,1)",
                                pointBorderColor: "rgba(79, 145, 249,1)",
                                data: OntimeDelPercent
                            },
                            {
                                label: "Perfect Order Completion",
                                // backgroundColor: "#f99843",
                                backgroundColor: "rgba(249, 103, 12,0.5)",//'rgba(249, 152, 67,0.5)',
                                borderColor: "rgba(249, 103, 12,0.5)",
                                pointBackgroundColor: "rgba(249, 103, 12,0.5)",
                                pointBorderColor: "rgba(249, 103, 12,0.5)",
                                data: AvgCycleTimeQty
                                //data: [100]
                            }
                        ]
                    };

                });
            };

    
</script>
    


    <style>
        .divblockstyle {
            transition: transform 500ms !important;
            transition-timing-function: ease-in;
            padding: 5px;
            background-color: #FFFFFF;
            min-height: 205px;
            /*border: 1px solid #ccc3c3;f7f8f9*/
        }

        .highcharts-button {
            display: none;
        }

        .module_login, body, .c-container-fluid {
            background-color: #fff !important;
        }

        .TextOrder {
            /*font-family: Verdana;
            font-size: 15px;
            letter-spacing: 2px;
            font-weight: bold;
            color: #7f8082;*/
            font-family: 'Roboto', sans-serif;
            letter-spacing: 0.5px;
            font-weight: bold;
            color: #7f8082;
            font-size: 15px;
        }

        .blockStyle:before {
            content: attr(tooltip);
            background: #FFFFFF;
            border: 2px solid #ccdffc;
            border-radius: 4px;
            padding: 5px 7px;
            margin-right: 10px;
            border-radius: 4px;
            color: #000;
            font: 500 13px Roboto;
            white-space: nowrap;
            position: absolute;
            /*bottom: -100%;*/
            margin-top: -35px;
            margin-left: -240%;
            z-index: 99999999;
            visibility: hidden;
            opacity: 0;
            transition: .3s;
        }

        [tooltip]:hover:before {
            visibility: visible;
            opacity: 1;
        }

        .col-md-6 {
            padding-right: 0px !important;
        }

        .tooltip {
            position: absolute;
            display: none;
            z-index: 1000;
            background-color: #BDA670;
            color: white;
            border: 1px solid black;
            width: 400px;
            height: 150px;
            padding: 10px;
            border-bottom-left-radius: 10px;
            border-bottom-right-radius: 10px;
            border-top-left-radius: 10px;
            border-top-right-radius: 10px;
            overflow: auto;
        }

        .blockStyle {
            height: 13px;
            background-color: red;
            padding: 6px;
            position: relative;
            border-radius: 100%;
            border: 1px solid #FFFFFF;
            box-shadow: 1px 2px 3px #CCCCCC;
        }

        .tooltip_ {
            position: relative;
        }

        .tooltip__ {
            position: absolute;
            /*background: var(--sideNav-bg);*/
            background-color: #f4f6f9;
            /*background-image: linear-gradient(-90deg, #cfcde200 -1%, #f4f6f9 150%);*/
            /*margin-top: -19px;*/
            /*padding: 5px 10px;*/
            padding: 10px;
            border: 3px solid #4f91f9;
            border-radius: 4px;
            color: #000;
            width: fit-content;
            margin-bottom: -70px;
            margin-left: -200px;
            opacity: 0;
            /*bottom:-70px;*/
            visibility: hidden;
            text-align: left;
            box-shadow: 1px 2px 3px #808080;
                left: 100px;
                cursor: pointer !important;
        }

        .tooltip_:hover .tooltip__ {
            visibility: visible;
            opacity: 1;
            z-index: 9999;
        }

        .dashedborder {
            border-top: 1px dashed #CCCCCC;
        }

        input {
            --md-arrow-width: 0.9em;
            --md-arrow-offset: -3%;
            -webkit-appearance: none;
            -webkit-padding-end: calc(var(--md-select-side-padding) + var(--md-arrow-offset) + var(--md-arrow-width) + 3px);
            -webkit-padding-start: var(--md-select-side-padding);
            background: url(Images/arrow_down.svg) calc(100% - var(--md-arrow-offset) - var(--md-select-side-padding)) center no-repeat;
            background-size: var(--md-arrow-width);
            cursor: pointer;
            font-family: inherit;
            font-size: inherit;
            outline: none;
            border-radius: 0px !important;
        }

        .card_view, .divblockstyle {
            position: relative;
            /*box-shadow: 0 2px 0px 0px rgba(0,0,0,0.14), 0 1px 0px 0px rgba(0,0,0,0.12), 0 1px 3px 0px rgba(0,0,0,0.2);*/
            border-radius: 4px;
            background: #fff;
            padding-top: 5px;
            box-shadow: 0 1px 4px 0 rgba(0,0,0,.14);
            box-shadow: rgba(53,64,82,.04) 0 2px 4px 0;
            background-color: #fff;
            background-clip: border-box;
            border: 1px solid rgb(110 117 130 / 37%);
            border-radius: 3px;
        }

        .pagewidth, .container, [container] {
            background: #f5f7fb !important;
                padding: 5px !important;
        }

        .innder__card {
            width: 60px;
            height: 60px;
            border-radius: 4px;
            box-shadow: 0 4px 6px 0 rgba(0,0,0,.14), 0 7px 7px -8px rgba(255,152,0,.4);
            transform: translate(5px, -5px);
        }

        .flex--card--between {
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .p10 {
            padding-top: 10px;
            padding-bottom: 1px;
        }


        .innder__card {
            text-align: center;
            font-size: 30px;
            line-height: 30px;
        }

            .innder__card span {
                font-size: 30px;
                line-height: 60px;
                font-weight: 500;
            }


        .card--count--card {
            font-size: 30px;
            font-weight: 600;
            text-align: right;
        }

        .card--title--card {
            text-align: right;
        }

        .inner_footer__card {
            border-top: 1px solid #cccccc75;
            padding: 5px 0px 0px 5px;
            font-weight: bold;
            font-size: 15px;
            color: #7f8082;
            /*font-family:verdana !important;*/
        }

        html {
            --left--bg--img: url("data:image/png;base64,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");
            --right--bg--img: url("data:image/png;base64,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");
        }

        .c1__c {
            background: var(--right--bg--img) no-repeat 100% 0,var(--left--bg--img) no-repeat 0 0 #5ab3fe;
            color: #fff;
        }

        .c2__c {
            background: var(--right--bg--img) no-repeat 100% 0,var(--left--bg--img) no-repeat 0 0 #173caf;
            color: #fff;
        }

        .c3__c {
            background: var(--right--bg--img) no-repeat 100% 0,var(--left--bg--img) no-repeat 0 0 rgba(244,67,54,.71);
            color: #fff;
        }

        .c4__c {
            background: var(--right--bg--img) no-repeat 100% 0,var(--left--bg--img) no-repeat 0 0 #4CAF50;
            color: #fff;
        }

        .search--bar-- {
            width: 250px;
            background: #fff;
            border-radius: 30px;
            padding: 0px 20px;
            position: fixed;
            top: 3px;
            z-index: 9999;
            right: 320px;
            border: 1px solid #ccc;
        }

            .search--bar-- .mdd-select-underline:before {
                display: none !important;
            }

            .search--bar-- .flex .mdd-select-underline, .flex input {
                border: 0px !important;
            }

                .flex input[type="text"], .flex input[type="number"], .flex input[type="password"], .flex input[type="password"] {
                    padding: 5px 5px 5px 0px !important;
                    display: block;
                    border: none !important;
                    border-bottom: px solid #b3adad !important;
                    outline: none !important;
                    color: #0e0e0e;
                    font-size: 12px;
                    width: 100%;
                }

            .search--bar-- .flex {
                width: 100%;
            }

        .row {
            margin-bottom: 0px !important;
        }

        .tooltip_legend {
            display: flex;
            font-family: verdana;
            font-size: small;
            font-weight: bold;
            width: 100%;
            justify-content: flex-end;
            position: absolute;
            bottom: 20px;
            right: 20px;
            text-align: right;
        }

        .mp {
            margin: 0px !important;
            padding: 0px !important;
        }

        .mb10 {
            margin-bottom: 5px !important;
        }
    </style>

    <style>
        .table-striped tr th {
            padding: 5px 10px 5px 5px !important;
            border-color: #5b9035;
            background-color: #5b9035;
            color: white;
            border-right: 1px solid #c8e8af !important;
        }

        .table-striped tr td {
            border-bottom: 1px solid #c8e8af !important;
            padding: 5px 10px 5px 5px !important;
            border-right: 1px solid #c8e8af !important;
        }

        .table-striped tbody tr:nth-child(even) {
            background-color: #f9fbf5;
        }

    </style>
   <%-- <div id="blocker" ng-show="blockUI">
            <div>
                loading... 
                <img src="Images/ajax-loader.gif" style="width: 50px;" />
            </div>
           <%-- <div class="breadCrumb">
                <div class="">Tripsheet List</div>
            </div>--%>
       
    <div class="module_yellow">
        <div class="ModuleHeader">
            <div>Dashboard</div>
            <%--        <a href="../MRLWMSC21_Core/Default.aspx">Dashboard</a> </div>--%>
        </div>

    </div>
    <div class="container" style="overflow: hidden">
        <div ng-app="MyApp" ng-controller="Dashboards">

            <div class="flex__ center search--bar--">
                <div class="flex" style="margin-right: 10px;">
                    <input type="text" id="txtWareHouse" placeholder="Select Warehouse" required="" />
                    <label></label>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6 mp ">

                    <div class="row mp">
                        <div class="col m6 mp">
                            <div class="card_view p10 mb10">
                                <div class="flex--card--between">
                                    <div class="innder__card c1__c setBg">
                                        <span class="material-icons">save_alt</span>
                                    </div>
                                    <div class="innder__count">
                                        <div class="card--title--card">MTD</div>
                                        <div class="card--count--card counter-value">{{POCount}}</div>
                                    </div>

                                </div>
                                <div class="inner_footer__card">Inbound</div>
                            </div>
                        </div>
                        <div class="col m6">
                            <div class="card_view p10 mb10">
                                <div class="flex--card--between">
                                    <div class="innder__card c2__c setBg">
                                        <span class="material-icons">local_shipping</span>
                                    </div>
                                    <div class="innder__count">
                                        <div class="card--title--card">MTD</div>
                                        <div class="card--count--card counter-value">{{SOCount}}</div>
                                    </div>

                                </div>
                                <div class="inner_footer__card">Outbound</div>
                            </div>
                        </div>
                        <div class="col m6 mp">
                            <div class="card_view p10 mb10">
                                <div class="flex--card--between">
                                    <div class="innder__card c3__c setBg">
                                        <span class="material-icons">refresh</span>
                                    </div>
                                    <div class="innder__count">
                                        <div class="card--title--card">TODAY</div>
                                        <div class="card--count--card counter-value">{{POCountOpen}}</div>
                                    </div>

                                </div>
                                <div class="inner_footer__card">Inward-Inprocess</div>
                            </div>
                        </div>
                        <div class="col m6">
                            <div class="card_view p10">
                                <div class="flex--card--between">
                                    <div class="innder__card c4__c setBg">
                                        <span class="material-icons">publish</span>
                                    </div>
                                    <div class="innder__count">
                                        <div class="card--title--card">TODAY</div>
                                        <div class="card--count--card counter-value">{{SOCountOpen}}</div>
                                    </div>

                                </div>
                                <div class="inner_footer__card">Outward-Inprocess</div>
                            </div>
                        </div>

                    </div>

                </div>
                <div class="col-md-6 mp">
                    <div class="row" style="margin: 0; padding-right:0px">
                        <div class="col m6 s6 l6" style="padding-right:0px; padding-left:0px">
                            <div class="divblockstyle">
                                <div class="tooltip_legend">
                                    <div class="tooltip_">
                                        <%--<div class="blockStyle" style="background-color: #3782fc"></div>--%>
                                        <div style="font-size: 15px; font-weight: bold; text-align: center; color: #7f8082;font-family:'Roboto',sans-serif;">WH Occupancy</div>
                                        <div class="tooltip__" style="margin-left: -150px;">
                                            <span style="font-weight: normal; font-size: 13px;">Locations Occupied VS Empty Locations</span>
                                        </div>
                                    </div>
                                </div>
                                <canvas id="TotalBinpiechart"></canvas>
                            </div>
                        </div>
                        <div class="col m3 s6 l3" hidden>
                            <div class="divblockstyle">

                                <div class="tooltip_legend">
                                    <div class="tooltip_">
                                        <%--<div class="blockStyle" style="background-color: #3782fc"></div>--%>
                                        <div style="font-size: 15px; font-weight: bold; text-align: center; color: #7f8082;">By Volume</div>
                                        <div class="tooltip__" style="margin-left: -150px;">
                                            <span style="font-weight: normal; font-size: 13px;">Volume Occupied .vs. Empty Volume</span>
                                        </div>
                                    </div>
                                </div>
                                <canvas id="ToalVolumepiechart"></canvas>
                            </div>
                        </div>

                        <div class="col m3 s6 l3" hidden>
                            <div class="divblockstyle">
                                <div class="tooltip_legend">
                                    <div class="tooltip_">
                                        <%--<div class="blockStyle" style="background-color: #3782fc"></div>--%>
                                        <div style="font-size: 15px; font-weight: bold; text-align: center; color: #7f8082;">By Weight</div>
                                        <div class="tooltip__" style="margin-left: -150px;">
                                            <span style="font-weight: normal; font-size: 13px;">Weight of Material Occupied .vs. Available Weight</span>
                                        </div>
                                    </div>
                                </div>
                                <canvas id="TotalWHWeightpiechart"></canvas>
                            </div>
                        </div>
                        <div class="col m6 s6 l6" style="    padding-right: 0;">
                            <div class="divblockstyle">
                                <div class="tooltip_legend">
                                    <div class="tooltip_">
                                        <%--<div class="blockStyle" style="background-color: #3782fc"></div>--%>
                                        <div style="font-size: 15px; font-weight: bold; text-align: center; color: #7f8082;font-family:'Roboto',sans-serif;">Stock State</div>
                                        <div class="tooltip__">
                                            <div style="text-align: center !important; font-size: 14px;">
                                                <u>Material State in the warehouse</u><br />
                                                <br />
                                            </div>
                                            <span style="color: #29328b; font-size: small !important; letter-spacing: 1px;">Formula :</span><br />
                                            <br />
                                            <span style="font-weight: normal; font-size: 13px;">Available (OK)<br />
                                                Inbound On Hold (IB-OH)<br />
                                                Outbound On Hold (OB-OH)<br />
                                                Expired Material (Exp)<br />
                                                Missing / Lost Material (Lost)<br />
                                                Quality On Hold (QC-OH)<br />
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <canvas id="divPieChartStock"></canvas>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            

            <div hidden>
                <div>
                    <div class="TextOrder" style="text-align: center;">Utilization</div>
                </div>
            </div>

            <div>
                <div hidden>
                    <div class="TextOrder" style="text-align: center;">Efficiency</div>

                </div>
                <div class="row" style="margin-bottom: 5px !important">
                    
                    <div class="col-md-6" style="padding-left: 0px;padding-right:7px !important;">
                        <div class="divblockstyle">
                             <div class="TextOrder" style="text-align: center;">Inbound - Efficiency</div>
                           <%-- <div style="display: flex; font-family: verdana; font-size: small; font-weight: bold; text-align: center; width: 100%; justify-content: center;">

                                <div class="tooltip_">
                                    <div class="blockStyle" style="background-color: #11d8ad;"></div>
                                    <div class="tooltip__">
                                        <div style="text-align: center !important; font-size: 14px;">
                                            <u>Average Pick Time </u>
                                            <br />
                                            <br />
                                        </div>
                                        <span style="font-weight: normal;">Average time taken to pick inventory</span><br />
                                        <br />
                                        <span style="color: #29328b; font-size: small !important; letter-spacing: 1px;">Formula :</span>
                                        <br />
                                        <br />
                                        <span style="font-weight: normal; font-size: 13px;">Time taken to pick orders / Total No of Orders * 100</span>
                                    </div>
                                </div>
                                &emsp;&emsp;

                         <div class="tooltip_">
                             <div class="blockStyle" style="background-color: #70b7f9;"></div>
                             <div class="tooltip__">
                                 <div style="text-align: center !important; font-size: 14px;">
                                     <u>Dock to Stock Time </u>
                                     <br />
                                     <br />
                                 </div>
                                 <span style="font-weight: normal;">Average time taken for inventory to be available from the time it is received at the receiving dock location</span><br />
                                 <br />
                                 <span style="color: #29328b; font-size: small !important; letter-spacing: 1px;">Formula :</span><br />
                                 <br />
                                 <span style="font-weight: normal; font-size: 13px;">Time taken from Receiving at Dock to GRN / Quantity * 100</span>
                             </div>
                         </div>
                                &emsp;&emsp;

                         <div class="tooltip_">
                             <div class="blockStyle" style="background-color: #f99b43;"></div>
                             <div class="tooltip__">
                                 <div style="text-align: center !important; font-size: 14px;">
                                     <u>Inventory Visibility</u><br />
                                     <br />
                                 </div>
                                 <span style="font-weight: normal;">Average time taken for inventory to be available from the time it is physically received into the warehouse</span><br />
                                 <br />
                                 <span style="color: #29328b; font-size: small !important; letter-spacing: 1px;">Formula :</span>
                                 <br />
                                 <br />
                                 <span style="font-weight: normal; font-size: 13px;">Time taken from first item receipt to GRN / Quantity * 100</span>
                             </div>
                         </div>
                            </div>--%>
                            <canvas id="LineChartForInbound" height="150"></canvas>
                        </div>
                    </div>
                    <div class="col-md-6" style="padding-right: 0px !important;padding-left: 0px !important;">
                        <div class="divblockstyle">
                            <div class="TextOrder" style="text-align: center;">Outbound - Efficiency</div>
                         <%--   <div style="display: flex; font-family: verdana; font-size: small; font-weight: bold; text-align: center; width: 100%; justify-content: center;">
                                <div class="tooltip_">
                                    <div class="blockStyle" style="background-color: #11d8ad;"></div>
                                    <div class="tooltip__">
                                        <div style="text-align: center !important; font-size: 14px;">
                                            <u>OTIF</u><br />
                                            <br />
                                        </div>
                                        <span style="font-weight: normal;">On Time, In Full</span><br />
                                        <br />
                                        <span style="color: #29328b; font-size: small !important; letter-spacing: 1px;">Formula :</span><br />
                                        <br />
                                        <span style="font-weight: normal; font-size: 13px;"># of orders on time and in full / Total # Orders * 100</span>
                                    </div>
                                </div>
                                &emsp;&emsp;
                        <div class="tooltip_">
                            <div class="blockStyle" style="background-color: #70b7f9;"></div>
                            <div class="tooltip__">
                                <div style="text-align: center !important; font-size: 14px;">
                                    <u>Order Accuracy </u>
                                    <br />
                                    <br />
                                </div>
                                <span style="font-weight: normal;">Orders picked in the right quantity without without line item corrections, Reverts</span>
                                <br />
                                <br />
                                <span style="color: #29328b; font-size: small !important; letter-spacing: 1px;">Formula :</span><br />
                                <br />
                                <span style="font-weight: normal; font-size: 13px;">((1 – Order with errors / Total Orders) * 100)</span>
                            </div>
                        </div>
                                &emsp;&emsp;

                        <div class="tooltip_">
                            <div class="blockStyle" style="background-color: #f99b43;"></div>
                            <div class="tooltip__">
                                <div style="text-align: center !important; font-size: 14px;">
                                    <u>Perfect Order Completion </u>
                                    <br />
                                    <br />
                                </div>
                                <span style="font-weight: normal;">No of orders without Changes, Damages, Returns / Total Orders</span><br />
                                <br />
                                <span style="color: #29328b; font-size: small !important; letter-spacing: 1px;">Formula :</span><br />
                                <br />
                                <span style="font-weight: normal; font-size: 13px;">No of Perfect Orders / Total Orders * 100</span>
                            </div>
                        </div>
                            </div>--%>

                            <canvas id="LineChartForOutbound" height="150">loading....
                            </canvas>
                        </div>
                    </div>
                </div>
                <div class="row" style="margin-bottom: 5px !important">
                    <div class="col-md-12" style="padding-left: 0px;    padding-right: 0px !important;">
                        <div class="divblockstyle">
                            <div class="TextOrder" style="text-align: center;">Outward Order Flow</div>
                            <%--<div style="display: flex; font-family: verdana; font-size: small; font-weight: bold; text-align: center; width: 100%; justify-content: center;">
                                <div class="tooltip_">
                                    <div class="blockStyle" style="background-color: #11d8ad;"></div>
                                    <div class="tooltip__">
                                        <div style="text-align: center !important; font-size: 14px;">
                                            <u>OTIF</u><br />
                                            <br />
                                        </div>
                                        <span style="font-weight: normal;">On Time, In Full</span><br />
                                        <br />
                                        <span style="color: #29328b; font-size: small !important; letter-spacing: 1px;">Formula :</span><br />
                                        <br />
                                        <span style="font-weight: normal; font-size: 13px;"># of orders on time and in full / Total # Orders * 100</span>
                                    </div>
                                </div>
                                &emsp;&emsp;
                        <div class="tooltip_">
                            <div class="blockStyle" style="background-color: #70b7f9;"></div>
                            <div class="tooltip__">
                                <div style="text-align: center !important; font-size: 14px;">
                                    <u>Order Accuracy </u>
                                    <br />
                                    <br />
                                </div>
                                <span style="font-weight: normal;">Orders picked in the right quantity without without line item corrections, Reverts</span>
                                <br />
                                <br />
                                <span style="color: #29328b; font-size: small !important; letter-spacing: 1px;">Formula :</span><br />
                                <br />
                                <span style="font-weight: normal; font-size: 13px;">((1 – Order with errors / Total Orders) * 100)</span>
                            </div>
                        </div>
                                &emsp;&emsp;

                        <div class="tooltip_">
                            <div class="blockStyle" style="background-color: #f99b43;"></div>
                            <div class="tooltip__">
                                <div style="text-align: center !important; font-size: 14px;">
                                    <u>Perfect Order Completion </u>
                                    <br />
                                    <br />
                                </div>
                                <span style="font-weight: normal;">No of orders without Changes, Damages, Returns / Total Orders</span><br />
                                <br />
                                <span style="color: #29328b; font-size: small !important; letter-spacing: 1px;">Formula :</span><br />
                                <br />
                                <span style="font-weight: normal; font-size: 13px;">No of Perfect Orders / Total Orders * 100</span>
                            </div>
                        </div>
                            </div>--%>

                            <canvas id="LineChartForOrders" height="100">
                            </canvas>

                            <%--<table class="table-striped">
                               <thead>
                                   <tr>
                                       <th center>Status (in Hrs.)</th>
                                       <th center>1</th>
                                       <th center>2</th>
                                       <th center>3</th>
                                       <th center>4</th>
                                       <th center>5</th>
                                       <th center>6</th>
                                       <th center>7</th>
                                       <th center>8</th>
                                       <th center>9</th>
                                       <th center>10</th>
                                       <th center>11</th>
                                       <th center>12</th>
                                       <th center>13</th>
                                       <th center>14</th>
                                       <th center>15</th>
                                       <th center>16</th>
                                       <th center>17</th>
                                       <th center>18</th>
                                       <th center>19</th>
                                       <th center>20</th>
                                       <th center>21</th>
                                       <th center>22</th>
                                       <th center>23</th>
                                       <th center>24</th>
                                   </tr>
                               </thead>
                                <tbody>
                                    <tr ng-repeat="ord in OutwardList">
                                        <td ng-show="ord.STATUS=='CREATED_OBD'" style="background:linear-gradient(to bottom, #3896f7 0%, #0722e2 100%);font-weight: bold;color: white;border-bottom: 1px solid #1037e5 !important;">Created SO's</td>                                        
                                        <td ng-show="ord.STATUS=='PENDING_OBD'" style="background:linear-gradient(to bottom, #fdec1c 0%, #f7a105 100%);font-weight: bold;color: black;border-bottom: 1px solid #f7ac08 !important;">Pending SO's</td>
                                        <td ng-show="ord.STATUS=='DELIVERED_OBD'" style="background:linear-gradient(to bottom, #63e208 0%, #049c25 100%);font-weight: bold;color: white;border-bottom: 1px solid #0ca222 !important;">Delivered SO's</td>
                                        <td ng-show="ord.STATUS=='TOTAL_OBD'" style="background:linear-gradient(to bottom, #aaada9 0%, #424c44 100%);font-weight: bold;color: white;border-bottom: 1px solid #4e5750 !important">Total SO's</td>
                                        <td style="text-align:center !important;">{{ord[0]}}</td>
                                        <td style="text-align:center !important;">{{ord[1]}}</td>
                                        <td style="text-align:center !important;">{{ord[2]}}</td>
                                        <td style="text-align:center !important;">{{ord[3]}}</td>
                                        <td style="text-align:center !important;">{{ord[4]}}</td>
                                        <td style="text-align:center !important;">{{ord[5]}}</td>
                                        <td style="text-align:center !important;">{{ord[6]}}</td>
                                        <td style="text-align:center !important;">{{ord[7]}}</td>
                                        <td style="text-align:center !important;">{{ord[8]}}</td>
                                        <td style="text-align:center !important;">{{ord[9]}}</td>
                                        <td style="text-align:center !important;">{{ord[10]}}</td>
                                        <td style="text-align:center !important;">{{ord[11]}}</td>
                                        <td style="text-align:center !important;">{{ord[12]}}</td>
                                        <td style="text-align:center !important;">{{ord[13]}}</td>
                                        <td style="text-align:center !important;">{{ord[14]}}</td>
                                        <td style="text-align:center !important;">{{ord[15]}}</td>
                                        <td style="text-align:center !important;">{{ord[16]}}</td>
                                        <td style="text-align:center !important;">{{ord[17]}}</td>
                                        <td style="text-align:center !important;">{{ord[18]}}</td>
                                        <td style="text-align:center !important;">{{ord[19]}}</td>
                                        <td style="text-align:center !important;">{{ord[20]}}</td>
                                        <td style="text-align:center !important;">{{ord[21]}}</td>
                                        <td style="text-align:center !important;">{{ord[22]}}</td>
                                        <td style="text-align:center !important;">{{ord[23]}}</td>
                                    </tr>
                                   
                                </tbody>
                            </table>--%>

                            <%-- <tr>
                                        <td style="background:linear-gradient(to bottom, #f78737 0%, #f14706 100%);font-weight: bold;color: white;border-bottom: 1px solid #f25712 !important;">Yesterday SO's</td>

                                        <td style="text-align:center !important;">10</td>
                                        <td style="text-align:center !important;">20</td>
                                        <td style="text-align:center !important;">10</td>
                                        <td style="text-align:center !important;">30</td>
                                        <td style="text-align:center !important;">35</td>
                                        <td style="text-align:center !important;">40</td>
                                        <td style="text-align:center !important;">30</td>
                                        <td style="text-align:center !important;">5</td>
                                        <td style="text-align:center !important;">10</td>
                                        <td style="text-align:center !important;">20</td>
                                        <td style="text-align:center !important;">10</td>
                                        <td style="text-align:center !important;">30</td>
                                        <td style="text-align:center !important;">35</td>
                                        <td style="text-align:center !important;">40</td>
                                        <td style="text-align:center !important;">30</td>
                                        <td style="text-align:center !important;">5</td>
                                        <td style="text-align:center !important;">10</td>
                                        <td style="text-align:center !important;">20</td>
                                        <td style="text-align:center !important;">10</td>
                                        <td style="text-align:center !important;">30</td>
                                        <td style="text-align:center !important;">35</td>
                                        <td style="text-align:center !important;">40</td>
                                        <td style="text-align:center !important;">30</td>
                                        <td style="text-align:center !important;">5</td>
                                    </tr>                                    
                                    <tr>
                                        <td style="background:linear-gradient(to bottom, #3896f7 0%, #0722e2 100%);font-weight: bold;color: white;border-bottom: 1px solid #1037e5 !important;">Created SO's</td>

                                        <td style="text-align:center !important;">10</td>
                                        <td style="text-align:center !important;">20</td>
                                        <td style="text-align:center !important;">10</td>
                                        <td style="text-align:center !important;">30</td>
                                        <td style="text-align:center !important;">35</td>
                                        <td style="text-align:center !important;">40</td>
                                        <td style="text-align:center !important;">30</td>
                                        <td style="text-align:center !important;">5</td>
                                        <td style="text-align:center !important;">10</td>
                                        <td style="text-align:center !important;">20</td>
                                        <td style="text-align:center !important;">10</td>
                                        <td style="text-align:center !important;">30</td>
                                        <td style="text-align:center !important;">35</td>
                                        <td style="text-align:center !important;">40</td>
                                        <td style="text-align:center !important;">30</td>
                                        <td style="text-align:center !important;">5</td>
                                        <td style="text-align:center !important;">10</td>
                                        <td style="text-align:center !important;">20</td>
                                        <td style="text-align:center !important;">10</td>
                                        <td style="text-align:center !important;">30</td>
                                        <td style="text-align:center !important;">35</td>
                                        <td style="text-align:center !important;">40</td>
                                        <td style="text-align:center !important;">30</td>
                                        <td style="text-align:center !important;">5</td>
                                    </tr>
                                    <tr>
                                        <td style="background:linear-gradient(to bottom, #fdec1c 0%, #f7a105 100%);font-weight: bold;color: black;border-bottom: 1px solid #f7ac08 !important;">Pending SO's</td>

                                        <td style="text-align:center !important;">10</td>
                                        <td style="text-align:center !important;">20</td>
                                        <td style="text-align:center !important;">10</td>
                                        <td style="text-align:center !important;">30</td>
                                        <td style="text-align:center !important;">35</td>
                                        <td style="text-align:center !important;">40</td>
                                        <td style="text-align:center !important;">30</td>
                                        <td style="text-align:center !important;">5</td>
                                        <td style="text-align:center !important;">10</td>
                                        <td style="text-align:center !important;">20</td>
                                        <td style="text-align:center !important;">10</td>
                                        <td style="text-align:center !important;">30</td>
                                        <td style="text-align:center !important;">35</td>
                                        <td style="text-align:center !important;">40</td>
                                        <td style="text-align:center !important;">30</td>
                                        <td style="text-align:center !important;">5</td>
                                        <td style="text-align:center !important;">10</td>
                                        <td style="text-align:center !important;">20</td>
                                        <td style="text-align:center !important;">10</td>
                                        <td style="text-align:center !important;">30</td>
                                        <td style="text-align:center !important;">35</td>
                                        <td style="text-align:center !important;">40</td>
                                        <td style="text-align:center !important;">30</td>
                                        <td style="text-align:center !important;">5</td>
                                    </tr>
                                    <tr>
                                        <td style="background:linear-gradient(to bottom, #63e208 0%, #049c25 100%);font-weight: bold;color: white;border-bottom: 1px solid #0ca222 !important;">Delivered SO's</td>

                                        <td style="text-align:center !important;">10</td>
                                        <td style="text-align:center !important;">20</td>
                                        <td style="text-align:center !important;">10</td>
                                        <td style="text-align:center !important;">30</td>
                                        <td style="text-align:center !important;">35</td>
                                        <td style="text-align:center !important;">40</td>
                                        <td style="text-align:center !important;">30</td>
                                        <td style="text-align:center !important;">5</td>
                                        <td style="text-align:center !important;">10</td>
                                        <td style="text-align:center !important;">20</td>
                                        <td style="text-align:center !important;">10</td>
                                        <td style="text-align:center !important;">30</td>
                                        <td style="text-align:center !important;">35</td>
                                        <td style="text-align:center !important;">40</td>
                                        <td style="text-align:center !important;">30</td>
                                        <td style="text-align:center !important;">5</td>
                                        <td style="text-align:center !important;">10</td>
                                        <td style="text-align:center !important;">20</td>
                                        <td style="text-align:center !important;">10</td>
                                        <td style="text-align:center !important;">30</td>
                                        <td style="text-align:center !important;">35</td>
                                        <td style="text-align:center !important;">40</td>
                                        <td style="text-align:center !important;">30</td>
                                        <td style="text-align:center !important;">5</td>
                                    </tr>
                                    <tr>
                                        <td style="background:linear-gradient(to bottom, #aaada9 0%, #424c44 100%);font-weight: bold;color: white;border-bottom: 1px solid #4e5750 !important">Total SO's</td>

                                        <td style="text-align:center !important;">10</td>
                                        <td style="text-align:center !important;">20</td>
                                        <td style="text-align:center !important;">10</td>
                                        <td style="text-align:center !important;">30</td>
                                        <td style="text-align:center !important;">35</td>
                                        <td style="text-align:center !important;">40</td>
                                        <td style="text-align:center !important;">30</td>
                                        <td style="text-align:center !important;">5</td>
                                        <td style="text-align:center !important;">10</td>
                                        <td style="text-align:center !important;">20</td>
                                        <td style="text-align:center !important;">10</td>
                                        <td style="text-align:center !important;">30</td>
                                        <td style="text-align:center !important;">35</td>
                                        <td style="text-align:center !important;">40</td>
                                        <td style="text-align:center !important;">30</td>
                                        <td style="text-align:center !important;">5</td>
                                        <td style="text-align:center !important;">10</td>
                                        <td style="text-align:center !important;">20</td>
                                        <td style="text-align:center !important;">10</td>
                                        <td style="text-align:center !important;">30</td>
                                        <td style="text-align:center !important;">35</td>
                                        <td style="text-align:center !important;">40</td>
                                        <td style="text-align:center !important;">30</td>
                                        <td style="text-align:center !important;">5</td>
                                    </tr>--%>
                            <br />

                        </div>
                    </div>
                  

                </div>
                
            </div>
            <div hidden>
                <div>

                    <div class="TextOrder" style="text-align: center;">Orders</div>

                </div>
                <div class="row" style="margin: 0;">
                    <div class="col-md-6">
                        <div class="divblockstyle">
                            <div style="display: flex; font-family: verdana; font-size: small; font-weight: bold; text-align: center; width: 100%; justify-content: center;">
                                <div class="tooltip_">
                                    <div class="blockStyle" style="background-color: #11d8ad;"></div>
                                    <div class="tooltip__">
                                        <div style="text-align: center !important; font-size: 14px;"><u>Orders per hour</u></div>
                                        <br />
                                        <span style="color: #29328b; font-size: small !important; letter-spacing: 1px;">Formula :</span><br />
                                        <br />
                                        <span style="font-weight: normal; font-size: 13px;">((# Orders / Total Time / No Persons) * 60)</span>
                                    </div>
                                </div>
                                &emsp;&emsp;

                         <div class="tooltip_">
                             <div class="blockStyle" style="background-color: #70b7f9;"></div>
                             <div class="tooltip__">
                                 <div style="text-align: center !important; font-size: 14px;"><u>Lines per hour</u></div>
                                 <br />
                                 <span style="color: #29328b; font-size: small !important; letter-spacing: 1px;">Formula :</span><br />
                                 <br />
                                 <span style="font-weight: normal; font-size: 13px;">((# Order Lines / Total Time / No Persons) * 60)</span>
                             </div>
                         </div>
                                &emsp;&emsp;

                         <div class="tooltip_">
                             <div class="blockStyle" style="background-color: #f99b43;"></div>
                             <div class="tooltip__">
                                 <div style="text-align: center !important; font-size: 14px;"><u>Items per hour</u></div>
                                 <br />
                                 <span style="color: #29328b; font-size: small !important; letter-spacing: 1px;">Formula :</span><br />
                                 <br />
                                 <br />
                                 <span style="font-weight: normal; font-size: 13px;">((# Order Items / Total Time / No Persons) * 60)</span>
                             </div>
                         </div>
                            </div>
                            <canvas id="LineChartForOrders1" height="150"></canvas>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="divblockstyle">
                            <div style="display: flex; font-family: verdana; font-size: small; font-weight: bold; text-align: center; width: 100%; justify-content: center;">
                                <div class="tooltip_">
                                    <div class="blockStyle" style="background-color: #11d8ad;"></div>
                                    <div class="tooltip__">
                                        <div style="text-align: center !important; font-size: 14px;"><u>Inventory Accuracy </u></div>
                                        <br />
                                        <span style="color: #29328b; font-size: small !important; letter-spacing: 1px;">Formula :</span>
                                        <br />
                                        <br />
                                        <span style="font-weight: normal; font-size: 13px;">Database Inventory Count  /  Physical Inventory Count * 100</span>
                                    </div>
                                </div>
                                &emsp;&emsp;

                         <div class="tooltip_">
                             <div class="blockStyle" style="background-color: #70b7f9;"></div>
                             <div class="tooltip__">
                                 <div style="text-align: center !important; font-size: 14px;"><u>Damaged Inventory</u></div>
                                 <br />
                                 <span style="color: #29328b; font-size: small !important; letter-spacing: 1px;">Formula :</span><br />
                                 <br />
                                 <span style="font-weight: normal; font-size: 13px;">Database Damaged Inventory Count  /  Physical Inventory Count * 100</span>
                             </div>
                         </div>
                            </div>
                            <canvas id="LineChartForInventory" height="150"></canvas>
                        </div>
                    </div>

                </div>
            </div>


            <div class="row mp">
                <div class="col m3" style="margin-left: 0; padding-left: 0px">
                    <div class="card_view p10 mb10">
                        <div class="flex--card--between">
                            <div class="innder__card c1__c setBg">
                                <span class="material-icons">bookmark_border</span>
                            </div>
                            <div class="innder__count">
                                <div class="card--title--card">&nbsp;</div>
                                <div class="card--count--card counter-value">{{ItemCount}}</div>
                            </div>

                        </div>
                        <div class="inner_footer__card">Active SKU's</div>
                    </div>
                </div>
                <div class="col m3">
                    <div class="card_view p10">
                        <div class="flex--card--between">
                            <div class="innder__card c3__c setBg">
                                <span class="material-icons">cancel_presentation</span>
                            </div>
                            <div class="innder__count">
                                <div class="card--title--card">MTD</div>
                                <div class="card--count--card counter-value">{{Cancel_SO}}</div>
                            </div>

                        </div>
                        <div class="inner_footer__card">SO's Cancelled</div>
                    </div>
                </div>
                <div class="col m3">
                    <div class="card_view p10">
                        <div class="flex--card--between">
                            <div class="innder__card c2__c setBg">
                                <span class="material-icons">assignment_return</span>
                            </div>
                            <div class="innder__count">
                                <div class="card--title--card">MTD</div>
                                <div class="card--count--card counter-value">{{Sup_Return}}</div>
                            </div>

                        </div>
                        <div class="inner_footer__card">Supplier Returns</div>
                    </div>
                </div>
                <div class="col m3" style="    padding-right: 0px;">
                    <div class="card_view p10">
                        <div class="flex--card--between">
                            <div class="innder__card c4__c setBg">
                                <span class="material-icons">assignment_returned</span>
                            </div>
                            <div class="innder__count">
                                <div class="card--title--card">MTD</div>
                                <div class="card--count--card counter-value">{{Cus_Return}}</div>
                            </div>

                        </div>
                        <div class="inner_footer__card">Customer Returns</div>
                    </div>
                </div>

            </div>

        </div>
    </div>

</asp:Content>
