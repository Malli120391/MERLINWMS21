var app = angular.module('MyApp', []);

app.controller('Dashboards', function ($scope, $http) {
    
   

    //alert('TP');
    $scope.dataList = [];

    var TextFieldName = $("#txtWareHouse");
    DropdownFunction(TextFieldName);
    $("#txtWareHouse").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/LoadWarehouseForCyccleCount"',
                data: "{ 'prefix': '" + request.term + "','AccountID':'" + $("#AM_MST_Account_ID").val() + "'}",//<=cp.TenantID%>
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

        },
        minLength: 0
    });

    $scope.gePercentageCharts = function () {

        var cityid = 0;
        var httpreq = {
            method: 'POST',
            url: 'Dashboards.aspx/GetPercentageReportData',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: {},
            async: false
        }
        $http(httpreq).success(function (response) {
            debugger;
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
                        position: 'top',
                        responsive: true,
                        fullWidth: false,
                        labels: {
                            boxWidth: 15,
                            fontColor: "black",
                            fontFamily: "Verdana",
                            fontWeight: 'bold'
                        }
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                fontColor: 'black',                               
                                fontFamily: "Verdana",
                                fontWeight: 'bold'
                            },
                        }],
                        xAxes: [{
                            ticks: {
                                fontColor: 'black',
                                fontFamily: "Verdana",
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
                labels: ["Emp. Loc. ", "Occupied Loc. "],
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
                            fontFamily: "Verdana",
                            fontWeight: 'bold'
                        }
                    }
                }
            });

            //============================================= END Bin Locations =========================================//

            //============================================= Bin Volume =========================================//
            var VolumepieData = {
                labels: ["Emp. Vol. ", "Occupied Vol. "],
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
                            fontFamily: "Verdana",
                            fontWeight: 'bold'
                        }
                    }
                }
            });

            //============================================= END Bin Volume =========================================//

            //============================================= WH Capacity =========================================//

            var WHWeightpieData = {
                labels: ["Emp. Capacity ", "Avbl. Stock "],
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
                            fontFamily: "Verdana",
                            fontWeight: 'bold'
                        }
                    }
                }
            });


            var WHWeightpieData = {
                labels: ["Damage Stock ", "OK Stock "],
                datasets: [{
                    data: [objs[0].Dam_Stock, objs[0].OK_Stock],
                    backgroundColor: ["#f99522", "#009ccc"],
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
                            fontFamily: "Verdana",
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
                        position: 'top',
                        responsive: true,
                        fullWidth: false,
                        labels: {
                            boxWidth: 15,
                            fontColor: "black",
                            fontFamily: "Verdana",
                            fontWeight: 'bold'
                        }
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                fontColor: 'black',
                                fontFamily: "Verdana",
                                fontWeight: 'bold'
                            },
                        }],
                        xAxes: [{
                            ticks: {
                                fontColor: 'black',
                                fontFamily: "Verdana",
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
                        position: 'top',
                        responsive: true,
                        fullWidth: false,
                        labels: {
                            boxWidth: 15,
                            fontColor: "black",
                            fontFamily: "Verdana",
                            fontWeight: 'bold'
                        }
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                fontColor: 'black',
                                fontFamily: "Verdana",
                                fontWeight: 'bold'
                            },
                        }],
                        xAxes: [{
                            ticks: {
                                fontColor: 'black',
                                fontFamily: "Verdana",
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

    $scope.gePercentageCharts();
    
});