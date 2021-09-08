var app = angular.module('MyApp', ['angularUtils.directives.dirPagination']);
app.controller('FormCtrl', function ($scope, $http) {

    var RefTenant = 1;

    $('#txtTenant').val("");
    var textfieldname = $("#txtTenant");
    DropdownFunction(textfieldname);
    $("#txtTenant").autocomplete({
        source: function (request, response) {
            $.ajax({
                //url: '../mWebServices/FalconWebService.asmx/GetTenantList',
                // data: "{ 'prefix': '" + request.term + "'}",
                url: '../mWebServices/FalconWebService.asmx/LoadTenantsByWH',
                data: "{ 'prefix': '" + request.term + "','whid':'" + WarehouseID + "' }",
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
            //Tenantid = i.item.val;
            RefTenant = i.item.val;
        },
        minLength: 0
    });


    var textfieldname = $("#txtWarehouse");
    DropdownFunction(textfieldname);
    $("#txtWarehouse").autocomplete({
        source: function (request, response) {
            //if ($("#txtTenant").val() == '') {
            //   showStickyToast(false, "Please select Tenant");
            // return false;
            //}
            $.ajax({
                // url: '../mWebServices/FalconWebService.asmx/LoadWarehousesBasedonTenant',
                //  data: JSON.stringify({ 'prefix': request.term, 'tenantID': RefTenant }),
                url: '../mWebServices/FalconWebService.asmx/LoaDWHListBasedonUser',
                data: "{ 'prefix': '" + request.term + "'  }",
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
            WarehouseID = i.item.val
            $('#txtTenant').val("");
        },
        minLength: 0
    });

    $('#txtMcode').val("");
    var textfieldname = $("#txtMcode");
    DropdownFunction(textfieldname);
    $("#txtMcode").autocomplete({
        source: function (request, response) {
            debugger;
            if (RefTenant == 0 || RefTenant == "0" || RefTenant == undefined || RefTenant == null) {
                showStickyToast(false, 'Please select Tenant');
                return false;
            }
            if ($("#txtMcode").val() == '') {
                Refpartno = 0;
            }
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/LoadMaterialsForForecasting',
                data: "{ 'prefix': '" + request.term + "','TenantID':'" + RefTenant + "'}",
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
            Refpartno = i.item.label;
        },
        minLength: 0
    });
 
    debugger;
    $scope.GetPredictions = function (val) {
        debugger;
        //alert($("#slider-range-min").slider("value"));
        var billing = {
            method: 'POST',
            url: 'DemandForecastingReport.aspx/GetTimeSeries',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'slidervalue': val }
        }
        $http(billing).success(function (response) {
            debugger;

            var dt = JSON.parse(response.d);
            $scope.BillingData = dt;
             $scope.GenerateChart(dt);

           
        });
    };

    $scope.GetPredictions(1);

   
    $scope.GenerateChart = function (obj) {
        //================================= Line Chart FOR OTIF ======================================//

        var OrderFillRateMonths = [];
        var OrderFillRatePercent = [];

        for (i = 0; i < $scope.BillingData.length; i++) {
            OrderFillRateMonths.push($scope.BillingData[i]['Expression.$TIME'].split("T")[0]);
            OrderFillRatePercent.push($scope.BillingData[i]['Expression.QUANTITY']);
        }



        var lineData = {
            //labels: ['2021-10-10', '2022-10-10', '2023-10-10', '2024-10-10', '2025-10-10', '2026-10-10'],//eval(OrderFillRateMonths),
            labels: eval(OrderFillRateMonths),
            datasets: [

                {
                    label: "OTIF",
                    // backgroundColor:"#20cc95",
                    backgroundColor: 'rgba(26,179,148,0.5)',
                    borderColor: "rgba(26,179,148,0.7)",
                    pointBackgroundColor: "rgba(26,179,148,1)",
                    pointBorderColor: "rgba(26,179,148,1)",
                    //data: [1, 10, 90, 87, 75, 98]
                    data: OrderFillRatePercent
                },
                {
                    label: "Perfect Order Completion",
                    // backgroundColor: "#f99843",
                    backgroundColor: "rgba(249, 103, 12,0.5)",//'rgba(249, 152, 67,0.5)',
                    borderColor: "rgba(249, 103, 12,0.5)",
                    pointBackgroundColor: "rgba(249, 103, 12,0.5)",
                    pointBorderColor: "rgba(249, 103, 12,0.5)",
                    //data: [1, 10, 90, 87, 75, 98]
                    data: OrderFillRatePercent
                    //data: AvgCycleTimeQty
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
                            labelString: 'Quantity',
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
    };
    $("#slider-range-min").slider({
        range: "min",
        value: 1,
        min: 1,
        max: 30,
        slide: function (event, ui) {
            //alert();
            $("#amount").val(ui.value);
            $scope.GetPredictions(ui.value);
        }
    });
    $("#amount").val($("#slider-range-min").slider("value"));


});