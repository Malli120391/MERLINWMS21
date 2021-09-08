var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('StockMovementReportNew', function ($scope, $http) {
    //alert("11");
    $scope.Getgedetails = function () {
        
        var fromdate = $("#txtFromdate").val();
        var textfieldname = $("#txttodate").val();
        var httpreq = {
            method: 'POST',
            url: 'StockMovementReportNew.aspx/GetBillingReportList',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'FromDate': fromdate, 'ToDate': textfieldname },
            async: false
        }
        $http(httpreq).success(function (response) {
            // $scope.blockUI = false;
            //
            $scope.BIllingReport = response.d;
            if ($scope.BIllingReport == undefined || $scope.BIllingReport == null || $scope.BIllingReport.length == 0)
                showStickyToast(false, "No Data Found");
        })
    }

    $scope.exportPdf = function () {
        //
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
        //
        var table = $scope.BIllingReport;
       
            $("#tbldata").empty();
            $("#tbldata").append("<table><thead><tr><td><img src='../Images/INV_TraceabilityAssured.png' style='height: 56px;' border='0'></td></tr><tr><th> PartNo</th><th>Description </th><th>TransactionDate</th><th>UOM</th><th>GoodsInQty</th><th>GoodsOutQty</th><th>AvailableQty</th></thead><tbody>");
            if (table != null && table.length > 0) {
            for (var i = 0; i < table.length; i++) {
                $("#tbldata").append("<tr></td><td class='aligntext'>" + table[i].PartNo + "</td><td class='aligntext'>" + table[i].Description + "</td><td class='aligntext'>" + table[i].TransactionDate + "</td><td class='aligntext'>" + table[i].UOM + "</td><td class='aligntext'>" + table[i].GoodsInQty + "</td><td class='aligntext'>" + table[i].GoodsOutQty + "</td><td class='aligntext'>" + table[i].AvailableQty + "</td></tr>");
            }
            $("#tbldata").append("</tbody></table>");
        }
        else {
                      
            $("#tbldata").append("<tr><td colspan='6' class='aligndate' style='background-color: white'>No Data</td></tr>");
                $("#tbldata").append("</tbody>");
            }
        }



});