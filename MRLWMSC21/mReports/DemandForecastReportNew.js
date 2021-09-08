var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('DemandForecastReportNew', function ($scope, $http) {
    // alert("11");

    var months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];   
    var today = new Date();   
    var mm = today.getMonth()+1; //January is 0!
    
    //alert(mm);
    var m1 = mm + 1;
    m1 = m1 > 12 ? m1 - 12 : m1;
    $scope.Month1 = months[m1 - 1];

    var m2 = mm + 2;
    m2 = m2 > 12 ? m2 - 12 : m2;
    $scope.Month2 = months[m2 - 1];

    var m3 = mm + 3;
    m3 = m3 > 12 ? m3 - 12 : m3;
    $scope.Month3 = months[m3 - 1];

    var m4 = mm + 4;
    m4 = m4 > 12 ? m4 - 12 : m4;
    $scope.Month4 = months[m4 - 1];


   

    
    //var m1 = mm > 12 ? (mm + 1 - 11) : (mm + 1);

    //if (m1 > 12) {

    //    m1 = m1 - 12;
    //}
    //else {
       
    //    alert(m1);
    //    $scope.Month1 = months[m1-1];
    //}

    //var m2 = mm + 1 > 11 ? (mm + 2 - 11) : (mm + 2);
    //if (m2 > 11) {

    //    m2 = 0;
    //}
    //else {
        
    //    alert(m2);
    //    $scope.Month2 = months[m2];
    //}

    //var m3 = mm + 2 > 11 ? (mm + 3 - 11) : (mm + 3);
    //if (m3 > 11) {

    //    m3 = 0;
    //}

    //else {
        
    //    alert(m3);
    //    $scope.Month3 = months[m3];
    //}

    //var m4 = mm + 3 > 11 ? (mm + 4 - 11) : (mm + 4);
    //if (m4 > 11) {

    //    m4 = 0;
    //    alert(m4);
    //}

    //else {
        
    //    alert(m4);
    //    $scope.Month4 = months[m4];
    //}
    



    
        
        //var fromdate = $("#txtFromdate").val();
        //var textfieldname = $("#txttodate").val();
        var httpreq = {
            method: 'POST',
            url: 'DemandForecastReportNew.aspx/GetBillingReportList',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: {},
            async: false
        }
        $http(httpreq).success(function (response) {
            // $scope.blockUI = false;
            //
            $scope.BIllingReport = response.d;
            if ($scope.BIllingReport == undefined || $scope.BIllingReport == null || $scope.BIllingReport.length == 0)
                showStickyToast(false, "No Data Found");
        })
    

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
        
        var table = $scope.BIllingReport;
        $("#tbldata").empty();
        $("#tbldata").append("<table><thead><tr><td><img src='../Images/INV_TraceabilityAssured.png' style='height: 56px;' border='0'></td></tr><tr><th> MaterialCode</th><th>AvailableQty</th><th>ReorderPoint</th><th>PlannedDeliveryTime</th><th>Month1</th><th>Month2</th><th>Month3</th><th>Month4</th></thead><tbody>");
        for (var i = 0; i < table.length; i++) {
            $("#tbldata").append("<tr></td><td class='aligndate'>" + table[i].MaterialCode + "</td><td class='aligndate'>" + table[i].AvailableQty + "</td><td class='aligndate'>" + table[i].ReorderPoint + "</td><td class='aligndate'>" + table[i].PlannedDeliveryTime + "</td><td class='aligndate'>" + table[i].M1 + "</td><td class='aligndate'>" + table[i].M2 + "</td><td class='aligndate'>" + table[i].M3 + "</td><td class='aligndate'>" + table[i].M4 + "</td></tr>");
        }
        $("#tbldata").append("</tbody></table>");
    }


});