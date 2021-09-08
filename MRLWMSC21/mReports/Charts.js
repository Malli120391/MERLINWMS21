//  ====================== Created by M.D.Prasad =========================//
var app = angular.module('MyApp', []);

app.controller('Charts', function ($scope, $http) {
  
    $scope.blockUI = true;
    $(".divapp").css('height', '600px');
    $(".divContainer").hide();    
    $scope.LoadInboundData = function (time)
    {
        
        var data = time;
        $('#spanTab1').html(data);
        $('#divTab1').mousemove(function () {
            
                $('.divTable').hide();
                var left = event.pageX;
                var top = event.pageY;
                top = top - 80;
                left = left - (-20);
                $('#divTable1').show().css({ 'left': left + 'px', 'top': top + 'px' });
                $("#divchildtable1").show();

            }).mouseout(function () {
                $('.divTable').hide();
            });
       
    }

    $scope.LoadOutboundData = function (time) {
        
        var data = time;
        $('#spanTab2').html(data);
        $('#divTab2').mousemove(function () {
            
            $('.divTable').hide();
            var left = event.pageX;
            var top = event.pageY;
            top = top - 80;
            left = left - (-20);
            $('#divTable2').show().css({ 'left': left + 'px', 'top': top + 'px' });
            $("#divchildtable2").show();

        }).mouseout(function () {
            $('.divTable').hide();
        });

    }

    $scope.LoadBaysData = function (bay) {
        
        var data = bay;
        $('#spanTab3').html(data);
        //$('#divTab3').mousemove(function () {
        //    
        //    $('.divTable').hide();
        //    var left = event.pageX;
        //    var top = event.pageY;
        //    top = top - 80;
        //    left = left - (-20);
        //    $('#divTable3').show().css({ 'left': left + 'px', 'top': top + 'px' });
        //    //$("#divchildtable3").show();

        //}).mouseout(function () {
        //    $('.divTable').hide();
        //});

    }
    
    $scope.TotalBinData = [];
    $scope.getBindata = function () {
        
        var cityid = 0;
        var httpreq = {
            method: 'POST',
            url: 'InfoGraphics.aspx/GETAllBinsData',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: {},
            async: false
        }
        $http(httpreq).success(function (response) {
            $scope.TotalBinData = response.d;
          
            
            $scope.blockUI = false;
            $(".divapp").css('height', 'auto');
            $(".divContainer").show();

            //============================================= Bin Locations =========================================//
            var BinData = {
                labels: ["Empty Loc. ", "Occupied Loc. "],
                datasets: [{
                    data: [$scope.TotalBinData.objTotalBins.NoOfEmptyLocations, $scope.TotalBinData.objTotalBins.NoOfOccupiedLocations],
                    backgroundColor: ["#f99522", "#009ccc"],
                }]
            };
            var BinOptions = {
                responsive: true               
            };
            var ctx4 = document.getElementById("TotalBinpiechart").getContext("2d");
            new Chart(ctx4, { type: 'pie', data: BinData, options: BinOptions });

            $("#lblTotal").text("Total Loc. :");
            $("#lblTotalLoc").text($scope.TotalBinData.objTotalBins.TotalBinLocations);

            //============================================= END Bin Locations =========================================//

            //============================================= Bin Volume =========================================//
            var VolumepieData = {
                labels: ["Empty Vol. ", "Occupied Vol. "],
                datasets: [{
                    data: [$scope.TotalBinData.objTotalBins.EmptyVolume, $scope.TotalBinData.objTotalBins.InventoryVolumeOccupied],
                    backgroundColor: ["#f99522", "#009ccc"],
                }]
            };

            var VolumepieOptions = {
                responsive: true
            };

            var ctx4 = document.getElementById("ToalVolumepiechart").getContext("2d");
            new Chart(ctx4, { type: 'pie', data: VolumepieData, options: VolumepieOptions });

            $("#lblTotalVolText").text("Total Vol. :");
            $("#lblTotalVolData").text($scope.TotalBinData.objTotalBins.TotalWarehouseBinVolume);

            //============================================= END Bin Volume =========================================//

            //============================================= WH Capacity =========================================//

            var WHWeightpieData = {
                labels: ["Empty Capacity ", "Available Stock "],
                datasets: [{
                    data: [$scope.TotalBinData.objTotalBins.EmptyCapacityByWeight, $scope.TotalBinData.objTotalBins.WightOfAvailableStock],
                    backgroundColor: ["#f99522", "#009ccc"],
                }]
            };

            var WHWeightpieOptions = {
                responsive: true
            };

            var ctx4 = document.getElementById("TotalWHWeightpiechart").getContext("2d");
            new Chart(ctx4, { type: 'pie', data: WHWeightpieData, options: WHWeightpieOptions });

            $("#lblTotalWeightText").text("Total WH Capacity :");
            $("#lblTotalWeightData").text($scope.TotalBinData.objTotalBins.TotalWHCapacityByWeight);

            //============================================= END WH Capacity =========================================//

            //============================================= Receipts =========================================//

            var InwardReceiptsData = {
                            labels: ["Receipts To Do ", "Completed Items "],
                            datasets: [{
                                data: [$scope.TotalBinData.objTotalInwardReceipts.ReceiptsToDo, $scope.TotalBinData.objTotalInwardReceipts.WorkItemsCompleted],
                                backgroundColor: ["#f99522", "#009ccc"],
                                doughnutHoleSize: 5.5
                            }]
                        };
                        var InwardReceiptsOptions = {
                            responsive: true
                        };

                        var ctx4 = document.getElementById("doughnutReceipts").getContext("2d");
                        new Chart(ctx4, { type: 'doughnut', data: InwardReceiptsData, options: InwardReceiptsOptions });

                        $("#lblTotalInwardText").text("Total Inward :");
                        $("#lblTotalInwardData").text($scope.TotalBinData.objTotalInwardReceipts.TotalInward);

            //============================================= END Receipts =========================================//

            //============================================= Putaways =========================================//

                        var InwardPutawayData = {
                            labels: ["Pending Items ", "Completed Items "],
                            datasets: [{
                                data: [$scope.TotalBinData.objTotalInwardPutaways.WorkItemsPanding, $scope.TotalBinData.objTotalInwardPutaways.WorkItemsCompleted],
                                backgroundColor: ["#f99522", "#009ccc"],
                                doughnutHoleSize: 5.5
                            }]
                        };
                        var InwardPutawayOptions = {
                            responsive: true
                        };

                        var ctx4 = document.getElementById("doughnutPutaways").getContext("2d");
                        new Chart(ctx4, { type: 'doughnut', data: InwardPutawayData, options: InwardPutawayOptions });

                        $("#lblTotalWorkText").text("Total Work Items :");
                        $("#lblTotalWorkData").text($scope.TotalBinData.objTotalInwardPutaways.TotalInwardWorkLines);

            //============================================= END Putaways =========================================//

            //============================================= END Picking =========================================//

                        var InwardPickingData = {
                            labels: ["Pending Lines ", "Completed Lines "],
                            datasets: [{
                                data: [$scope.TotalBinData.objTotalInwardPicking.PendingLines, $scope.TotalBinData.objTotalInwardPicking.CompletedLines],
                                backgroundColor: ["#f99522", "#009ccc"],
                                doughnutHoleSize: 5.5
                            }]
                        };
                        var InwardPickingOptions = {
                            responsive: true
                        };

                        var ctx4 = document.getElementById("doughnutPicking").getContext("2d");
                        new Chart(ctx4, { type: 'doughnut', data: InwardPickingData, options: InwardPickingOptions });

                        $("#lblTotalPickText").text("Total Picking Lines :");
                        $("#lblTotalPickData").text($scope.TotalBinData.objTotalInwardPicking.PickingLines);

            //============================================= END Picking =========================================//

            //============================================= Inbound =========================================//
                        
                        var time = $scope.TotalBinData.objTotalInboundData.NormalizedTime;
                        $scope.LoadInboundData(time);
                        $("#divchildtable1").append("<div><b>Time Taken For Completion : </b>" + $scope.TotalBinData.objTotalInboundData.TimeTakenForCompletion + "</div>");

            //============================================= END Inbound =========================================//

            //============================================= Outbound =========================================//
                        
                        var time = $scope.TotalBinData.objTotalOutBoundData.NormalizedTime;
                        $scope.LoadOutboundData(time);
                        $("#divchildtable2").append("<div><b>Time Taken For Completion : </b>" +  $scope.TotalBinData.objTotalOutBoundData.TimeTakenForCompletion + "</div>");

            //============================================= END Outbound =========================================//

            //============================================= END Bays =========================================//
                        
                        //var bay = $scope.TotalBinData.objTotalBays.BaysCurrentlyUsed + " [ " + $scope.TotalBinData.objTotalBays.TotalLoadingBays + " ]";
                        //$scope.LoadBaysData(bay); 

            //============== unused for Active Bays =============== //
                        //var num = "Active Bays : " + $scope.TotalBinData.objTotalBays.BaysCurrentlyUsed;
            //$("#divchildtable3").append("<div>" + num + "</div>");

            //============================================= END Bays =========================================//

            //============================================= Inventory Ageing =========================================//
                        
                        var barData = {
                            labels: ["Vol. Age 6Months", "Vol. Age 12Months", "Vol. Age 24Months", "SKUs 6Months", "SKUs 12Months", "SKUs 24Months"],
                            datasets: [
                                {
                                    label: "Volume ",
                                    backgroundColor: 'rgba(220, 220, 220, 0.5)',
                                    pointBorderColor: "#fff",
                                    data: [$scope.TotalBinData.objInventoryAgeing.VolumeAgeGT6Months, $scope.TotalBinData.objInventoryAgeing.VolumeAgeGT12Months, $scope.TotalBinData.objInventoryAgeing.VolumeAgeGT24Months]
                                },
                                {
                                    label: "No. Of SKUs ",
                                    backgroundColor: 'rgba(26,179,148,0.5)',
                                    borderColor: "rgba(26,179,148,0.7)",
                                    pointBackgroundColor: "rgba(26,179,148,1)",
                                    pointBorderColor: "#fff",
                                    data: [$scope.TotalBinData.objInventoryAgeing.NoSKUsGT6Months, $scope.TotalBinData.objInventoryAgeing.NoSKUsGT12Months, $scope.TotalBinData.objInventoryAgeing.NoSKUsGT24Months]
                                }
                            ]
                        };

                        var barOptions = {
                            responsive: true
                        };

                        var ctx2 = document.getElementById("barChart").getContext("2d");
                        new Chart(ctx2, { type: 'bar', data: barData, options: barOptions });

            //============================================= END Inventory Ageing =========================================//

            //============================================= Inbound Ageing =========================================//

                        
                        var Inbounddata = [];
                        var InboundQty = [];
                        Inbounddata += "[";
                        InboundQty += "[";

                        for (i = 0 ; i < $scope.TotalBinData.objItemLinesAgeing.length ; i++) {
                            if ($scope.TotalBinData.objItemLinesAgeing[i].GoodsMovementType == "Inbound") {
                                Inbounddata += "'" + $scope.TotalBinData.objItemLinesAgeing[i].MonthName + "',";
                                InboundQty += $scope.TotalBinData.objItemLinesAgeing[i].Quantity + ",";
                            }

                        }
                        Inbounddata = Inbounddata.substring(0, Inbounddata.length - 1);
                        Inbounddata += "]";

                        InboundQty = InboundQty.substring(0, InboundQty.length - 1);
                        InboundQty += "]";

                        var lineData = {
                            labels : eval(Inbounddata),
                            datasets: [

                                {
                                    label: "Quantity ",
                                    backgroundColor: 'rgba(26,179,148,0.5)',
                                    borderColor: "rgba(26,179,148,0.7)",
                                    pointBackgroundColor: "rgba(26,179,148,1)",
                                    pointBorderColor: "#fff",
                                    data: eval(InboundQty)
                                }
                            ]
                        };

                        var lineOptions = {
                            responsive: true
                        };

                        var ctx = document.getElementById("inboundLineChart").getContext("2d");
                        new Chart(ctx, { type: 'line', data: lineData, options: lineOptions });

            //============================================= END Inbound Ageing =========================================//

            //============================================= Outbound Ageing =========================================//
                        
                        var Outbounddata = [];
                        var OutboundQty = [];
                        Outbounddata += "[";
                        OutboundQty += "[";

                        for (i = 0 ; i < $scope.TotalBinData.objItemLinesAgeing.length ; i++)
                        {
                            if ($scope.TotalBinData.objItemLinesAgeing[i].GoodsMovementType == "Outbound")
                            {
                                Outbounddata += "'" + $scope.TotalBinData.objItemLinesAgeing[i].MonthName + "',";
                                OutboundQty += $scope.TotalBinData.objItemLinesAgeing[i].Quantity + ",";
                            }

                        }
                        Outbounddata = Outbounddata.substring(0, Outbounddata.length - 1);
                        Outbounddata += "]";

                        OutboundQty = OutboundQty.substring(0, OutboundQty.length - 1);
                        OutboundQty += "]";            

                        var lineData = {
                            labels: eval(Outbounddata),
                            datasets: [

                                {
                                    label: "Quantity ",
                                    backgroundColor: 'rgba(26,179,148,0.5)',
                                    borderColor: "rgba(26,179,148,0.7)",
                                    pointBackgroundColor: "rgba(26,179,148,1)",
                                    pointBorderColor: "#fff",
                                    data: eval(OutboundQty)
                                }
                            ]
                        };

                        var lineOptions = {
                            responsive: true
                        };

                        var ctx = document.getElementById("outboundLineChart").getContext("2d");
                        new Chart(ctx, { type: 'line', data: lineData, options: lineOptions });

            //============================================= END Outbound Ageing =========================================//
        });
    }
    $scope.getBindata();

});