<%@ Page Title="Dashboards" Language="C#" MasterPageFile="~/Site.Master"  AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MRLWMSC21.mReports.Default"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <link href="../CSS/bootstrap.min.css" rel="stylesheet" />
    <link href="Charts/animate.css" rel="stylesheet" />
    <script src="../mInventory/Scripts/angular.min.js"></script>  
    <script src="../mInventory/Scripts/dirPagination.js"></script>
    <script src="../mMisc/Scripts/bootstrap.min.js"></script>  
    <script src="Charts/Chart.min.js"></script>
    <%--<script src="Dashboards.js"></script>--%>


    <script src="Charts/highcharts.js"></script>
    <script src="Charts/highcharts-3d.js"></script>
    <script src="Charts/exporting.js"></script>
    
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
                    debugger;
                    $.ajax({
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

                    $("#WarehouseID").val(i.item.val);
                    $scope.gePercentageCharts(i.item.val);

                },
                minLength: 0
            });

            $scope.GetWarehouse = function ()
            {
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
                    var WHID = dt[0].WarehouseID;
                    $scope.gePercentageCharts(WHID);
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
                    data: { 'WareHouseID': Whid},
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
                                label: "OTIF ",
                                // backgroundColor:"#20cc95",
                                backgroundColor: 'rgba(26,179,148,0.5)',
                                borderColor: "rgba(26,179,148,0.7)",
                                pointBackgroundColor: "rgba(26,179,148,1)",
                                pointBorderColor: "rgba(26,179,148,1)",
                                data: OrderFillRatePercent
                            },

                            {
                                label: "Order Accuracy ",
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

                    var lineOptions = {
                        responsive: true
                    };

                    var ctx = document.getElementById("LineChartPercentage").getContext("2d");
                    new Chart(ctx, {
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
                                        labelString: 'Percentage',
                                        fontColor: 'black',
                                        fontFamily: "verdana",
                                        fontWeight: 'bold',
                                        fontSize: 18
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
                                        labelString: 'Months',
                                        fontColor: 'black',
                                        fontFamily: "verdana",
                                        fontWeight: 'bold',
                                        fontSize: 18
                                    },
                                    ticks: {
                                        fontColor: 'black',
                                        fontFamily: "verdana",
                                        fontWeight: 'bold'
                                    },
                                }]
                            }
                        }
                    });


                    //================================= Line Chart ======================================//

                    //================================= Pie Chart Method1======================================//
                    //        debugger; 
                    //        var obj = $scope.dataList.Table3;
                    //        var dt = Object.keys(obj[0]);
                    //        var divContentID6 = '';
                    //        var divContainerID6 = document.getElementById("divPieChartStock");
                    //        divContentID6 += '<canvas id="PieChartStock" height="150"></canvas>';
                    //        divContainerID6.innerHTML = divContentID6;

                    //        var HeaderData = [];
                    //        var Columndata = [];

                    //        for (var i = 0; i < dt.length; i++) {
                    //            HeaderData.push(dt[i].replace("_"," ") + " ");
                    //            Columndata.push(obj[0][dt[i]]);
                    //        }

                    //        var BinData = { labels: ["OK Stock ","Damage Stock ","Lost and Found Qty. "], datasets: [{ data: [obj[0].OK_Stock,obj[0].Dam_Stock,obj[0].LFQty ], backgroundColor: ["#f78359", "#f7f583", "#23efa5"], options: { title: { display: true } } }] };
                    //        var canvas = document.getElementById("PieChartStock").getContext("2d");
                    //        new Chart(canvas, {
                    //            type: 'pie', data: BinData, options: {
                    //                legend: {
                    //                    display: true,
                    //                    position: 'bottom',
                    //                    responsive: true,
                    //                    fullWidth:false,
                    //                    labels: {
                    //                        boxWidth: 15,
                    //                    }
                    //                }
                    //            },
                    //    options3d: {
                    //    enabled: true,
                    //    alpha: 45,
                    //    beta: 0
                    //}
                    //        });

                    // ================================= Pie Chart Method1======================================//
                    var obj = $scope.dataList.Table3;
                    var objs = $scope.dataList.Table4;
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
                    var ctx4 = document.getElementById("TotalBinpiechart").getContext("2d");
                    new Chart(ctx4, {
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

                    //============================================= END Bin Locations =========================================//

                    //============================================= Bin Volume =========================================//
                    var VolumepieData = {
                        labels: ["Empty ", "Occupied "],
                        datasets: [{
                            data: [obj[0].EmptyVolume, obj[0].InventoryVolumeOccupied],
                            backgroundColor: ["#f99522", "#009ccc"],
                        }]
                    };

                    var VolumepieOptions = {
                        responsive: true
                    };

                    var ctx4 = document.getElementById("ToalVolumepiechart").getContext("2d");
                    new Chart(ctx4, {
                        type: 'doughnut', data: VolumepieData, options: {
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

                    //============================================= END Bin Volume =========================================//

                    //============================================= WH Capacity =========================================//

                    var WHWeightpieData = {
                        labels: ["Empty ", "Occupied "],
                        datasets: [{
                            data: [obj[0].EmptyCapacityByWeight, obj[0].WightOfAvailableStock],
                            backgroundColor: ["#f99522", "#009ccc"],
                        }]
                    };

                    var WHWeightpieOptions = {
                        responsive: true
                    };

                    var ctx4 = document.getElementById("TotalWHWeightpiechart").getContext("2d");
                    new Chart(ctx4, {
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
                    });


                    var WHWeightpieData = {
                        labels: ["Lost ", "OK ","Exp.","IBOH","OBOH","QCOH"],
                        datasets: [{
                            data: [objs[0].Lost, objs[0].OK, objs[0].Exp, objs[0].IBOH, objs[0].OBOH, objs[0].QCOH],
                            backgroundColor: ["#f99522", "#03c450", "#f43202", "#009ccc","#f9f509","#CCCCCC"],
                        }]
                    };

                    var WHWeightpieOptions = {
                        responsive: true
                    };

                    var ctx4 = document.getElementById("divPieChartStock").getContext("2d");
                    new Chart(ctx4, {
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
                    });


                    //============================= Multi line Chart for Picking Time =============================//

                    var PickingMonths = [];
                    var PickingHours = [];

                    for (i = 0; i < $scope.dataList.Table5.length; i++) {
                        PickingMonths.push($scope.dataList.Table5[i].Monthname);
                        PickingHours.push($scope.dataList.Table5[i].AvgPickTimeInHours);
                    }

                    var DocStocklMonths = [];
                    var DocStockPercent = [];

                    for (i = 0; i < $scope.dataList.Table6.length; i++) {
                        DocStocklMonths.push($scope.dataList.Table6[i].Monthname);
                        DocStockPercent.push($scope.dataList.Table6[i].DockToStockTime);
                    }

                    var InvvisblMonth = [];
                    var InvVisblHours = [];
                    for (i = 0; i < $scope.dataList.Table7.length; i++) {
                        InvvisblMonth.push($scope.dataList.Table7[i].Monthname);
                        InvVisblHours.push($scope.dataList.Table7[i].InventoryVisibility);
                    }

                    var lineData = {
                        labels: eval(InvvisblMonth),
                        datasets: [

                            {
                                label: "Average Picking Time ",
                                // backgroundColor:"#20cc95",
                                backgroundColor: 'rgba(26,179,148,0.5)',
                                borderColor: "rgba(26,179,148,0.7)",
                                pointBackgroundColor: "rgba(26,179,148,1)",
                                pointBorderColor: "rgba(26,179,148,1)",
                                data: PickingHours
                            },
                            {
                                label: "Dock to Stock ",
                                //backgroundColor:"#43cbf9",
                                backgroundColor: 'rgba(79, 145, 249,0.5)',
                                borderColor: "rgba(79, 145, 249,0.7)",
                                pointBackgroundColor: "rgba(79, 145, 249,1)",
                                pointBorderColor: "rgba(79, 145, 249,1)",
                                data: DocStockPercent
                            },
                            {
                                label: "Inventory Visibility",
                                // backgroundColor: "#f99843",
                                backgroundColor: "rgba(249, 103, 12,0.5)",//'rgba(249, 152, 67,0.5)',
                                borderColor: "rgba(249, 103, 12,0.5)",
                                pointBackgroundColor: "rgba(249, 103, 12,0.5)",
                                pointBorderColor: "rgba(249, 103, 12,0.5)",
                                data: InvVisblHours
                                //data: [100]
                            }
                        ]
                    };

                    var lineOptions = {
                        responsive: true
                    };

                    var ctx = document.getElementById("LineChartForPicking").getContext("2d");
                    new Chart(ctx, {
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
                                        labelString: 'Hours',
                                        fontColor: 'black',
                                        fontFamily: "verdana",
                                        fontWeight: 'bold',
                                        fontSize: 18
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
                                        labelString: 'Months',
                                        fontColor: 'black',
                                        fontFamily: "verdana",
                                        fontWeight: 'bold',
                                        fontSize: 18
                                    },
                                    ticks: {
                                        fontColor: 'black',
                                        fontFamily: "verdana",
                                        fontWeight: 'bold'
                                    },
                                }]
                            }
                        }
                    });

                    //============================= Multi line Chart for Picking Time =============================//






                    //============================= Multi line Chart for Picking Time =============================//

                    var InvAccMonths = [];
                    var InvAccHours = [];

                    for (i = 0; i < $scope.dataList.Table8.length; i++) {
                        InvAccMonths.push($scope.dataList.Table8[i].Monthname);
                        InvAccHours.push($scope.dataList.Table8[i].InventoryAccuracy);
                    }

                    var DamMonths = [];
                    var DamPercent = [];

                    for (i = 0; i < $scope.dataList.Table9.length; i++) {
                        DamMonths.push($scope.dataList.Table9[i].Monthname);
                        DamPercent.push($scope.dataList.Table9[i].DamageAccuracy);
                    }

                    //var InvAccMonth = [];
                    //var InvAccHours = [];
                    //for (i = 0; i < $scope.dataList.Table7.length; i++) {
                    //    InvAccMonth.push($scope.dataList.Table7[i].Monthname);
                    //    InvAccHours.push($scope.dataList.Table7[i].InventoryVisibility);
                    //}

                    var lineData = {
                        labels: eval(InvAccMonths),
                        datasets: [

                            {
                                label: "Inventory Accuracy ",
                                // backgroundColor:"#20cc95",
                                backgroundColor: 'rgba(26,179,148,0.5)',
                                borderColor: "rgba(26,179,148,0.7)",
                                pointBackgroundColor: "rgba(26,179,148,1)",
                                pointBorderColor: "rgba(26,179,148,1)",
                                data: InvAccHours
                            },
                            {
                                label: "Damage Inventory ",
                                //backgroundColor:"#43cbf9",
                                backgroundColor: 'rgba(79, 145, 249,0.5)',
                                borderColor: "rgba(79, 145, 249,0.7)",
                                pointBackgroundColor: "rgba(79, 145, 249,1)",
                                pointBorderColor: "rgba(79, 145, 249,1)",
                                data: DamPercent
                            }
                            //,
                            //{
                            //    label: "Inventory Visibility",
                            //    // backgroundColor: "#f99843",
                            //    backgroundColor: "rgba(249, 103, 12,0.5)",//'rgba(249, 152, 67,0.5)',
                            //    borderColor: "rgba(249, 103, 12,0.5)",
                            //    pointBackgroundColor: "rgba(249, 103, 12,0.5)",
                            //    pointBorderColor: "rgba(249, 103, 12,0.5)",
                            //    data: InvAccHours
                            //    //data: [100]
                            //}
                        ]
                    };

                    var lineOptions = {
                        responsive: true
                    };

                    var ctx = document.getElementById("LineChartForInventory").getContext("2d");
                    new Chart(ctx, {
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
                                        labelString: 'Percentage',
                                        fontColor: 'black',
                                        fontFamily: "verdana",
                                        fontWeight: 'bold',
                                        fontSize: 18
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
                                        labelString: 'Months',
                                        fontColor: 'black',
                                        fontFamily: "verdana",
                                        fontWeight: 'bold',
                                        fontSize: 18
                                    },
                                    ticks: {
                                        fontColor: 'black',
                                        fontFamily: "verdana",
                                        fontWeight: 'bold'
                                    },
                                }]
                            }
                        }
                    });

                    //============================= Multi line Chart for Picking Time =============================//



                    //============================= Multi line Chart for Picking Time =============================//

                   
                    //var InvAccMonth = [];
                    //var InvAccHours = [];
                    //for (i = 0; i < $scope.dataList.Table7.length; i++) {
                    //    InvAccMonth.push($scope.dataList.Table7[i].Monthname);
                    //    InvAccHours.push($scope.dataList.Table7[i].InventoryVisibility);
                    //}

                    var lineData = {
                        labels: eval(InvAccMonths),
                        datasets: [

                            {
                                label: "OPH ",
                                // backgroundColor:"#20cc95",
                                backgroundColor: 'rgba(26,179,148,0.5)',
                                borderColor: "rgba(26,179,148,0.7)",
                                pointBackgroundColor: "rgba(26,179,148,1)",
                                pointBorderColor: "rgba(26,179,148,1)",
                                data: [0.00,3.50,4.00,2.00,0.00,0.00]
                            },
                            {
                                label: "LPH ",
                                //backgroundColor:"#43cbf9",
                                backgroundColor: 'rgba(79, 145, 249,0.5)',
                                borderColor: "rgba(79, 145, 249,0.7)",
                                pointBackgroundColor: "rgba(79, 145, 249,1)",
                                pointBorderColor: "rgba(79, 145, 249,1)",
                                data: [2.50,0.00,0.00,5.50,0.00,6.50]
                            },
                            {
                                label: "IPH",
                                // backgroundColor: "#f99843",
                                backgroundColor: "rgba(249, 103, 12,0.5)",//'rgba(249, 152, 67,0.5)',
                                borderColor: "rgba(249, 103, 12,0.5)",
                                pointBackgroundColor: "rgba(249, 103, 12,0.5)",
                                pointBorderColor: "rgba(249, 103, 12,0.5)",
                                data: [9.50,0.00,2.50,7.00,0.00,0.00]
                                //data: [100]
                            }
                        ]
                    };

                    var lineOptions = {
                        responsive: true
                    };

                    var ctx = document.getElementById("LineChartForOrders").getContext("2d");
                    new Chart(ctx, {
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
                                        labelString: 'Units',
                                        fontColor: 'black',
                                        fontFamily: "verdana",
                                        fontWeight: 'bold',
                                        fontSize: 18
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
                                        labelString: 'Months',
                                        fontColor: 'black',
                                        fontFamily: "verdana",
                                        fontWeight: 'bold',
                                        fontSize: 18
                                    },
                                    ticks: {
                                        fontColor: 'black',
                                        fontFamily: "verdana",
                                        fontWeight: 'bold'
                                    },
                                }]
                            }
                        }
                    });

                    //============================= Multi line Chart for Picking Time =============================//


































                    // <div id="divPieChartWarehouse"></div>
                    //var obj = $scope.dataList.Table3;

                    //Highcharts.chart('divPieChartLocations', {
                    //    chart: {
                    //        type: 'pie',
                    //        width: 550,
                    //        options3d: {
                    //            enabled: true,
                    //            alpha: 45,
                    //            beta: 0
                    //        },
                    //        backgroundColor: "none",
                    //    },
                    //    title: {
                    //        text: ''
                    //    },
                    //    tooltip: {
                    //    },
                    //    plotOptions: {
                    //        pie: {
                    //            allowPointSelect: true,
                    //            cursor: 'pointer',
                    //            depth: 35,
                    //            dataLabels: {
                    //                enabled: true,
                    //            },
                    //            showInLegend: true
                    //        }
                    //    },
                    //    series: [{
                    //        type: 'pie',
                    //        name: 'Loc Report',
                    //        data: [
                    //        ['No. Of Occupied Locations', obj[0].NoOfEmptyLocations], 
                    //        ['No.Of Empty Locations', obj[0].NoOfOccupiedLocations],
                    //        ]
                    //    }]
                    //});

                    //Highcharts.chart('divPieChartVolumes', {
                    //    chart: {
                    //        type: 'pie',
                    //        width: 450,
                    //        options3d: {
                    //            enabled: true,
                    //            alpha: 45,
                    //            beta: 0
                    //        },
                    //        backgroundColor: "none",
                    //    },
                    //    title: {
                    //        text: ''
                    //    },
                    //    tooltip: {
                    //    },
                    //    plotOptions: {
                    //        pie: {
                    //            allowPointSelect: true,
                    //            cursor: 'pointer',
                    //            depth: 35,
                    //            dataLabels: {
                    //                enabled: true,
                    //            },
                    //            showInLegend: true
                    //        }
                    //    },
                    //    series: [{
                    //        type: 'pie',
                    //        name: 'Loc Report',
                    //        data: [
                    //            ['Occupied Volume', obj[0].EmptyVolume],
                    //            ['Empty Volume', obj[0].InventoryVolumeOccupied],
                    //        ]
                    //    }]
                    //});

                    //Highcharts.chart('divPieChartWarehouse', {
                    //    chart: {
                    //        type: 'pie',
                    //        width: 450,
                    //        options3d: {
                    //            enabled: true,
                    //            alpha: 45,
                    //            beta: 0
                    //        },
                    //        backgroundColor: "none",
                    //    },
                    //    title: {
                    //        text: ''
                    //    },
                    //    tooltip: {
                    //    },
                    //    plotOptions: {
                    //        pie: {
                    //            allowPointSelect: true,
                    //            cursor: 'pointer',
                    //            depth: 35,
                    //            dataLabels: {
                    //                enabled: true,
                    //            },
                    //            showInLegend: true
                    //        }
                    //    },
                    //    series: [{
                    //        type: 'pie',
                    //        name: 'Warehouse Capacity',
                    //        data: [
                    //            ['Available Stock', obj[0].EmptyCapacityByWeight],
                    //            ['Empty Capacity', obj[0].WightOfAvailableStock],
                    //        ]                   
                    //    }]
                    //});


                    //debugger;
                    //var objs = $scope.dataList.Table4;
                    //Highcharts.chart('divPieChartStock', {
                    //    chart: {
                    //        type: 'pie',
                    //        width: 450,
                    //        options3d: {
                    //            enabled: true,
                    //            alpha: 45,
                    //            beta: 0
                    //        },
                    //        backgroundColor: "none",
                    //    },
                    //    title: {
                    //        text: ''
                    //    },
                    //    tooltip: {
                    //    },
                    //    plotOptions: {
                    //        pie: {
                    //            allowPointSelect: true,
                    //            cursor: 'pointer',
                    //            depth: 35,
                    //            dataLabels: {
                    //                enabled: true,
                    //            },
                    //            showInLegend: true
                    //        }
                    //    },
                    //    series: [{
                    //        type: 'pie',
                    //        name: 'Stock Report',
                    //        data: [
                    //            ['OK Stock', objs[0].OK_Stock],
                    //            ['Damage Stock', objs[0].Dam_Stock],
                    //            ['Lost and Found Qty.', objs[0].LFQty]
                    //        ],
                    //        colors: ["#7cb5ec", "#434348","#fc520f"]
                    //    }]
                    //});

                });
            };

            //$scope.gePercentageCharts(Whid);
        });
    </script>


    <style>
         .divblockstyle {
            transition: transform 500ms !important;
            transition-timing-function: ease-in;
            box-shadow:2px 2px 3px #dadada;
            border-radius: 0px;
            padding: 10px;
            background-color:#FFFFFF;
            /*border: 1px solid #ccc3c3;f7f8f9*/
        }

        .highcharts-button {
            display:none;
        }

        .module_login,body,.c-container-fluid {
            background-color:#fff !important;
        }

        .TextOrder {
            /*writing-mode: vertical-rl;
            text-orientation: upright;*/
            font-family:Verdana;
            font-size:large;
            letter-spacing:2px;
            font-weight:bold;
            color:#7f8082;
        }
        .blockStyle:before {
    content: attr(tooltip);
    background: #FFFFFF;
    border:2px solid #ccdffc;
    border-radius:4px;
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
    
    z-index:99999999;
    visibility: hidden;
    opacity: 0;
    transition: .3s;
}

[tooltip]:hover:before {
    visibility: visible;
    opacity: 1;
}

        .col-md-6 {
            padding-right:0px !important;
        }

.tooltip
        {
            position:absolute;
            display:none;
            z-index:1000;
             background-color:#BDA670;
            color:white;
            border: 1px solid black;
            width:400px;
            height:150px;
            padding:10px;
            border-bottom-left-radius:10px;
            border-bottom-right-radius:10px;
            border-top-left-radius:10px;
            border-top-right-radius:10px;
            overflow:auto;
           
        }
        .blockStyle {
            height: 13px;background-color:red;padding:6px;position:relative;border-radius:100%;border:1px solid #FFFFFF;box-shadow:1px 2px 3px #CCCCCC;
        }

        .tooltip_ {
            position: relative;
        }

        .tooltip__ {
            position: absolute;
            /*background: var(--sideNav-bg);*/
            background-color:#f4f6f9;
             /*background-image: linear-gradient(-90deg, #cfcde200 -1%, #f4f6f9 150%);*/
            /*margin-top: -19px;*/
            /*padding: 5px 10px;*/
            padding:10px;
            border:3px solid #4f91f9;
            border-radius: 4px;
            color: #000;
            width: 27vw;
            margin-bottom: -70px;
            margin-left: -200px;
            opacity: 0;
            /*bottom:-70px;*/
            visibility: hidden;
            text-align:left;
            box-shadow:1px 2px 3px #808080;
        }

        .tooltip_:hover .tooltip__ {
            visibility:visible;
            opacity:1;
            z-index:9999;
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
    background: url(../Images/arrow_down.svg) calc(100% - var(--md-arrow-offset) - var(--md-select-side-padding)) center no-repeat;
    background-size: var(--md-arrow-width);
    cursor: pointer;
    font-family: inherit;
    font-size: inherit;
    outline: none;
    border-radius: 0px !important;
        }
        #txtWareHouse {
            width: 160px !important;
        }
    </style>
<div class="dashed"></div>
    <div class="pagewidth">
    <div ng-app="MyApp" ng-controller="Dashboards"><br />
        <div align="right">
         <div class="flex" style="width:160px;">
        <input type="text" id="txtWareHouse" required="" />  
             <label>Select Warehouse</label>
             </div>
            </div>

        <div>
        <div>
             <div class="TextOrder" style="text-align:center;margin-top:-30px;">Efficiency</div><br /><p></p>
        </div>
        <div class="row" style="margin:0;">
            <div class="col-md-6">
                <div class="divblockstyle">
                    <div style="display: flex; font-family: verdana; font-size: small; font-weight: bold; text-align: center; width: 100%; justify-content: center;">
                        <div class="tooltip_">
                            <div class="blockStyle" style="background-color: #11d8ad;"></div>
                            <div class="tooltip__">
                                <div style="text-align:center !important;font-size:14px;"><u>OTIF</u><br /><br /></div>
                                <span style="font-weight:normal;">On Time, In Full</span><br /><br />
                                <span style="color:#29328b;font-size:small !important;letter-spacing:1px;"> Formula :</span><br /><br /><span style="font-weight:normal;font-size:13px;"> # of orders on time and in full / Total # Orders * 100</span></div>
                        </div>
                        &emsp;&emsp;
                        <div class="tooltip_">
                            <div class="blockStyle" style="background-color: #70b7f9;"></div>
                            <div class="tooltip__">
                                <div style="text-align:center !important;font-size:14px;"><u>Order Accuracy </u><br /><br /></div>
                                <span style="font-weight:normal;"> Orders picked in the right quantity without line item corrections, Reverts</span>
                                <br /><br />
                                <span style="color:#29328b;font-size:small !important;letter-spacing:1px;"> Formula :</span><br /><br /><span style="font-weight:normal;font-size:13px;">  ((1 – Order with errors / Total Orders) * 100)</span></div>
                        </div>
                        &emsp;&emsp;

                        <div class="tooltip_">
                            <div class="blockStyle" style="background-color: #f99b43;"></div>
                            <div class="tooltip__">
                                <div style="text-align:center !important;font-size:14px;"><u>Perfect Order Completion </u><br /><br /></div>
                                   <span style="font-weight:normal;"> No of orders without Changes, Damages, Returns / Total Orders</span><br /><br />
                                <span style="color:#29328b;font-size:small !important;letter-spacing:1px;"> Formula :</span><br /><br /> <span style="font-weight:normal;font-size:13px;"> No of Perfect Orders / Total Orders * 100</span></div>
                        </div>
                    </div>

                    <canvas id="LineChartPercentage" height="150"></canvas>
                </div>
            </div>
            <div class="col-md-6">
                <div class="divblockstyle">
                    <div style="display: flex; font-family: verdana; font-size: small; font-weight: bold; text-align: center; width: 100%; justify-content: center;">

                         <div class="tooltip_">
                            <div class="blockStyle" style="background-color: #11d8ad;"></div>
                            <div class="tooltip__">
                                <div style="text-align:center !important;font-size:14px;"><u>Average Pick Time </u><br /><br /></div>
                                    <span style="font-weight:normal;">Average time taken to pick inventory</span><br /><br />
                                 <span style="color:#29328b;font-size:small !important;letter-spacing:1px;"> Formula :</span> <br /><br /><span style="font-weight:normal;font-size:13px;">Time taken to pick orders / Total No of Orders * 100</span></div>
                        </div>
                        &emsp;&emsp;

                         <div class="tooltip_">
                            <div class="blockStyle" style="background-color: #70b7f9;"></div>
                            <div class="tooltip__">
                                <div style="text-align:center !important;font-size:14px;"><u>Dock to Stock Time </u><br /><br /></div>
                               <span style="font-weight:normal;"> Average time taken for inventory to be available from the time it is received at the receiving dock location</span><br /><br />
                                <span style="color:#29328b;font-size:small !important;letter-spacing:1px;">  Formula :</span><br /><br /> <span style="font-weight:normal;font-size:13px;">Time taken from Receiving at Dock to GRN / Quantity * 100</span></div>
                        </div>
                        &emsp;&emsp;

                         <div class="tooltip_">
                            <div class="blockStyle" style="background-color: #f99b43;"></div>
                            <div class="tooltip__">
                                <div style="text-align:center !important;font-size:14px;"><u>Inventory Visibility</u><br /><br /></div>
                                <span style="font-weight:normal;">
                                Average time taken for inventory to be available from the time it is physically received into the warehouse</span><br /><br />
                                <span style="color:#29328b;font-size:small !important;letter-spacing:1px;"> Formula :</span> <br /><br /><span style="font-weight:normal;font-size:13px;">Time taken from first item receipt to GRN / Quantity * 100</span></div>
                        </div>
                    </div>
                    <canvas id="LineChartForPicking" height="150"></canvas>
                </div>
            </div>

        </div>
            </div>
        <br />
        <br />
        <div class="dashedborder"></div>

        <div><br />
            <div>
                <div class="TextOrder" style="text-align:center;">Utilization</div><br /><p></p>
            </div>

            <div class="row" style="margin:0;">                     
            <div class="col-lg-3">
                <div class="divblockstyle">                    
                    <div style="display: flex; font-family: verdana; font-size: small; font-weight: bold; text-align: center; width: 100%; justify-content: center;">                       
                    <div class="tooltip_">
                            <%--<div class="blockStyle" style="background-color: #3782fc"></div>--%>
                        <div style="font-size:medium;font-weight:bold;text-align:center;color:#7f8082;"> By Location</div> 
                            <div class="tooltip__" style="margin-left: -150px;">
                                <span style="font-weight:normal;font-size:13px;">
                                Locations Occupied .vs. Empty Locations</span>
                        </div>
                        </div>
                    </div>   <br />           
                    <canvas id="TotalBinpiechart"></canvas>
                </div>
            </div>
            <div class="col-lg-3">
                <div class="divblockstyle">
                    
                    <div style="display: flex; font-family: verdana; font-size: small; font-weight: bold; text-align: center; width: 100%; justify-content: center;">
                    <div class="tooltip_">
                            <%--<div class="blockStyle" style="background-color: #3782fc"></div>--%>
                        <div style="font-size:medium;font-weight:bold;text-align:center;color:#7f8082;"> By Volume</div> 
                            <div class="tooltip__" style="margin-left: -150px;"> <span style="font-weight:normal;font-size:13px;">Volume Occupied .vs. Empty Volume</span>
                        </div>
                        </div>
                    </div><br />     
                    <canvas id="ToalVolumepiechart"></canvas>
                </div>
            </div>
           
                  <div class="col-lg-3">
                <div class="divblockstyle">                    
                    <div style="display: flex; font-family: verdana; font-size: small; font-weight: bold; text-align: center; width: 100%; justify-content: center;">
                    <div class="tooltip_">
                            <%--<div class="blockStyle" style="background-color: #3782fc"></div>--%>
                         <div style="font-size:medium;font-weight:bold;text-align:center;color:#7f8082;"> By Weight</div> 
                            <div class="tooltip__" style="margin-left: -150px;"> <span style="font-weight:normal;font-size:13px;">Weight of Material Occupied .vs. Available Weight</span>
                        </div>
                        </div>
                    </div><br />     
                    <canvas id="TotalWHWeightpiechart"></canvas>
                </div>
            </div>
            <div class="col-lg-3">
                <div class="divblockstyle">                      
                    <div style="display: flex; font-family: verdana; font-size: small; font-weight: bold; text-align: center; width: 100%; justify-content: center;">                        
                    <div class="tooltip_">
                            <%--<div class="blockStyle" style="background-color: #3782fc"></div>--%>
                        <div style="font-size:medium;font-weight:bold;text-align:center;color:#7f8082;"> Stock State</div>
                            <div class="tooltip__">
                                <div style="text-align:center !important;font-size:14px;"><u>Material State in the warehouse</u><br /><br /></div>
                                <span style="color:#29328b;font-size:small !important;letter-spacing:1px;"> Formula :</span><br /><br /><span style="font-weight:normal;font-size:13px;"> Available (OK)<br />Inbound On Hold (IB-OH)<br />Outbound On Hold (OB-OH)<br />Expired Material (Exp)<br />Missing / Lost Material (Lost)<br />Quality On Hold (QC-OH)<br /> </span></div>
                        </div>
                    </div><br />
                    <canvas id="divPieChartStock"></canvas>
                </div>
            </div>
        </div>


        </div>
        <br />
        <br />
        <div class="dashedborder"></div>

        <div>
        <div><br /><p></p>
             <div class="TextOrder" style="text-align:center;">Orders</div><br /><p></p>
        </div>
        <div class="row" style="margin:0;">
            <div class="col-md-6">
                <div class="divblockstyle">
                     <div style="display: flex; font-family: verdana; font-size: small; font-weight: bold; text-align: center; width: 100%; justify-content: center;">
                        <div class="tooltip_">
                            <div class="blockStyle" style="background-color: #11d8ad;"></div>
                            <div class="tooltip__">
                                <div style="text-align:center !important;font-size:14px;"><u>Orders per hour</u></div><br />
                                <span style="color:#29328b;font-size:small !important;letter-spacing:1px;"> Formula :</span><br /><br /> <span style="font-weight:normal;font-size:13px;">((# Orders / Total Time / No Persons) * 60)</span></div>
                        </div>
                        &emsp;&emsp;

                         <div class="tooltip_">
                            <div class="blockStyle" style="background-color: #70b7f9;"></div>
                            <div class="tooltip__">
                                <div style="text-align:center !important;font-size:14px;"><u>Lines per hour</u></div><br />
                                <span style="color:#29328b;font-size:small !important;letter-spacing:1px;"> Formula :</span><br /><br /><span style="font-weight:normal;font-size:13px;"> ((# Order Lines / Total Time / No Persons) * 60)</span></div>
                        </div>
                        &emsp;&emsp;

                         <div class="tooltip_">
                            <div class="blockStyle" style="background-color: #f99b43;"></div>
                            <div class="tooltip__">
                                 <div style="text-align:center !important;font-size:14px;"><u>Items per hour</u></div><br />
                                <span style="color:#29328b;font-size:small !important;letter-spacing:1px;"> Formula :</span><br /> <br /><br /><span style="font-weight:normal;font-size:13px;">((# Order Items / Total Time / No Persons) * 60)</span></div>
                        </div>
                    </div>
                    <canvas id="LineChartForOrders" height="150"></canvas>
                </div>
            </div>
            <div class="col-md-6">
                <div class="divblockstyle">
                     <div style="display: flex; font-family: verdana; font-size: small; font-weight: bold; text-align: center; width: 100%; justify-content: center;">
                        <div class="tooltip_">
                            <div class="blockStyle" style="background-color: #11d8ad;"></div>
                            <div class="tooltip__">
                                <div style="text-align:center !important;font-size:14px;"><u>Inventory Accuracy </u></div><br />
                                <span style="color:#29328b;font-size:small !important;letter-spacing:1px;"> Formula :</span> <br /><br /><span style="font-weight:normal;font-size:13px;">Database Inventory Count  /  Physical Inventory Count * 100</span></div>
                        </div>
                        &emsp;&emsp;

                         <div class="tooltip_">
                            <div class="blockStyle" style="background-color: #70b7f9;"></div>
                            <div class="tooltip__">
                                <div style="text-align:center !important;font-size:14px;"><u>Damaged Inventory</u></div>
                                <br />
                                <span style="color:#29328b;font-size:small !important;letter-spacing:1px;"> Formula :</span><br /><br /> <span style="font-weight:normal;font-size:13px;">Database Damaged Inventory Count  /  Physical Inventory Count * 100</span></div>
                        </div>
                    </div>
                    <canvas id="LineChartForInventory" height="150"></canvas>
                </div>
            </div>

        </div>
            </div>
        
    </div>
        </div>
</asp:Content>
