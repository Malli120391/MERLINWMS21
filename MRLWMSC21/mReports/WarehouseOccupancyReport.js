var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);

myApp.controller('WarehouseOccupancyReport', function ($scope, $http) {
    //alert("11");
  
    
    var RefWH = '';
    
    $("#txtDate").datepicker({
                 todayBtn: 1,
                 singleDatePicker: true,
                 showDropdowns: true,
                 autoclose: true,
                 forceParse: false,
                 format: "yyyy-mm-dd",
                 startDate: "today",
            
             });
   

  
    //$http(httpreq).success(function (response) {
    //    // $scope.blockUI = false;
    //    //
    //    $scope.BIllingReport = response.d;
    //    if ($scope.BIllingReport == undefined || $scope.BIllingReport == null || $scope.BIllingReport.length == 0)
    //        showStickyToast(false, "No Data Found");
    //})
    $scope.invoiceCount = 0;
    $scope.invoiceTotal = 0;
 
    $scope.getWarehouse = function () {

        //alert('11');  
    
        
        $('#txtWH').val("");

        var textfieldname = $("#txtWH");
        DropdownFunction(textfieldname);
        $("#txtWH").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/LoadWarehouseData',
                    data: "{ 'prefix': '" + request.term + "','TenantID':'"+null+"'}",
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
                
                RefWH = i.item.val;
                // alert(Refnumber);
                //$scope.ngtenant = i.item.val;
            },
            minLength: 0
        });

    }
    $scope.GetTotals = function (whid, date) {
        
        var Warehouse = RefWH;
        var Date = $("#txtDate").val();
        async: false;
        var httpitemInfo =
            {
                method: 'POST',
                url: '../mReports/WarehouseOccupancyReport.aspx/GetTotalOccupancyList',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'dataType': 'json',
                    async: false,
                },
                data: { 'WHID': whid, 'date': date }
            }
        $http(httpitemInfo).success(function (response) {
            
            $scope.TotalsList = response.d;

        });

    }
    $scope.Getdetails = function () {
        
        var Warehouse = RefWH;
        var Date = $("#txtDate").val();
        async: false;
        var httpitemInfo1 =
            {
                method: 'POST',
                url: '../mReports/WarehouseOccupancyReport.aspx/GetWarehouseOccupancyList',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'dataType': 'json',
                    async: false,
                },
                data: { 'WHID': 1, 'date': Date }
            }
        $http(httpitemInfo1).success(function (response) {
            
            $scope.WHOccupReport = response.d;
            $scope.setTotals();
            $scope.GetTotals(1, Date);
            
        });
        //$scope.GetTotals(1, Date);
        
    }
    var item = $scope.WHOccupReport;
    $scope.TotalAvailableQty = 0;
    $scope.TTTotalVolume = 0;
    $scope.TotalOccupancy = 0;
    $scope.setTotals = function () {
        
        for (var i = 0; i < $scope.WHOccupReport.length; i++) {
            $scope.TotalAvailableQty += parseFloat($scope.WHOccupReport[i].AvailableQty);
            $scope.TTTotalVolume += parseFloat($scope.WHOccupReport[i].TotalVolume);
            $scope.TotalOccupancy += parseFloat($scope.WHOccupReport[i].Occupancy);
       //total += (product.price * product.quantity);
     }
    }

    $scope.exportPdf = function () {
        
        $scope.export();
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ type: 'pdf', jspdf: { orientation: 'l', margins: { right: 10, left: 10, top: 40, bottom: 40 }, autotable: { tableWidth: 'auto' } } });
        $("#tbldata").css('display', 'none');
        $scope.export();
    }
    $scope.exportExcel = function () {
        //
        $scope.export();
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ type: 'excel' });
        $("#tbldata").css('display', 'none');
    }
    $scope.exportCsv = function () {
        //
        $scope.export();
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ type: 'csv', numbers: { html: { decimalMark: '.', thousandsSeparator: ',' }, output: { decimalMark: ',', thousandsSeparator: '' } } });
        $("#tbldata").css('display', 'none');
    }

    $scope.exportXml = function () {
        //
        $scope.export();
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ type: 'xml' });
        $("#tbldata").css('display', 'none');
    }

    $scope.exportTxt = function () {
        //
        $scope.export();
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ type: 'txt' });
        $("#tbldata").css('display', 'none');
    }

    $scope.exportWord = function () {
        //
        $scope.export();
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ type: 'doc' });
        $("#tbldata").css('display', 'none');
    }

    $scope.export = function () {
        
        var table = $scope.WHOccupReport;
        $("#tbldata").empty();
        $("#tbldata").append("<table><thead><tr><td><img src='../Images/INV_TraceabilityAssured.png' style='height: 56px;' border='0'></td></tr><tr><th> Tenant Name</th><th>Available Qty.</th><th>Total Volume</th><th>Occupancy</th></thead><tbody>");
        for (var i = 0; i < table.length; i++) {
            $("#tbldata").append("<tr></td><td class='aligndate'>" + table[i].Tenant + "</td><td class='aligndate'>" + table[i].AvailableQty + "</td><td class='aligndate'>" + table[i].TotalVolume + "</td><td class='aligndate'>" + table[i].Occupancy + "</td></tr>");
        }
        $("#tbldata").append("</tbody></table>");
    }


});