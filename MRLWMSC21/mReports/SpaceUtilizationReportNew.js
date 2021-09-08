var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('SpaceUtilizationReportNew', function ($scope, $http) {
    //alert('Hi');
    
    //$scope.today = new Date();
    //$scope.today1 = new Date();
    $scope.BIllingReport = [];

    
    var httpreq = {
        method: 'POST',
        url: '../mReports/SpaceUtilizationReportNew.aspx/GetWarehouses',
        headers: {
            'Content-Type': 'application/json; charset=utf-8',
            'dataType': 'json'
        },
        data: {}
    }
    $http(httpreq).success(function (response) {
        $scope.warehouses = response.d;
        if ($scope.warehouses == undefined || $scope.warehouses == null || $scope.warehouses.length == 0)
            showStickyToast(false, "No Data Found");
    });

    

    $scope.Getgedetails = function () {
        



        debugger;

        var fromdate = $("#txtFromdate").val();
        var textfieldname = $("#txttodate").val();
        var warehouse = $scope.ddlWarehouse;
      
        warehouse = warehouse == undefined ? "0" : warehouse;
        var httpreq = {
            method: 'POST',
            url: 'SpaceUtilizationReportNew.aspx/GetSpaceList',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'warehouseid': warehouse, 'FromDate': fromdate, 'ToDate': textfieldname },
            async: false
        }
        $http(httpreq).success(function (response) {
            // $scope.blockUI = false;
            debugger;
            $scope.BIllingReport = [];
            $scope.BIllingReport = JSON.parse(response.d);

            if ($scope.BIllingReport == "" || $scope.BIllingReport == null || $scope.BIllingReport.length == 0)
                showStickyToast(false, "No Data Found");
           
            $('#div12').empty();
            var table = "<table class='Headertablewidth'><tr class='mytableReportHeaderTR'>";
            $.each($scope.BIllingReport[0], function (key, value) {
                table += "<th style='width:auto;'>" + key + "</th>";
            });
            table += "</tr>";
            
            for (var i = 0; i < $scope.BIllingReport.length; i++) {
                    
                    table += "<tr class='mytableReportItemsHeaderTR'>";
                    $.each($scope.BIllingReport[i], function (key, value) {
                        table += "<td style='width:auto;'>" + value + "</td>";
                    });
                    
                    table += "</tr>";
            }
            
            table += "</table>";

            $("#div12").append(table);
           
            $scope.makeTable($scope.BIllingReport);
            
            //$scope.BIllingReport.push({ rows: response.d, cols: Object.keys(response.d) });
        })
    }

    $scope.makeTable = function (mydata) {
        
        var table = $('<table border=1>');
        var tblHeader = "<tr>";
        for (var k in mydata[0]) tblHeader += "<th>" + k + "</th>";
        tblHeader += "</tr>";
        $(tblHeader).appendTo(table);
        $.each(mydata, function (index, value) {
            var TableRow = "<tr>";
            $.each(value, function (key, val) {
                TableRow += "<td>" + val + "</td>";
            });
            TableRow += "</tr>";
            $(table).append(TableRow);
        });
        return ($(table));
    };

    // dynamic table binding for exporting files done by Meena 06/03/2018//
    $scope.loadgrid = function (mydata) {
        debugger;
        var tableHeaders = '';
        var count = 0;
        //var col = [];
        for (var k in mydata[0]) {
            tableHeaders += "<th>" + k + "</th>";
            count++;
        }        
        console.log(tableHeaders);     
            $("#tbldata").empty();
            $("#tbldata").append('<thead><tr>' + tableHeaders + '</tr></thead>');
            //$("#tblMISCList").append("<tr><td>tddata</td>leQuantity + "</td><td class='text-right'>" + MappingList[i].QCHoldQuantity + "</td><td class='text-right'>" + MappingList[i].OnHoldQuantity + "</td><td class='text-center ActionForHide'><input type='radio' id='IsMapped' name='spr' class='i-checks' onchange='EditStockMovement(" + MappingList[i].INV_TRN_StorageLocationInventory_ID + ");'/></td></tr>");
            $.each(mydata, function (index, value) {
                var tbody = '';
                $.each(value, function (key, val) {
                    tbody += "<td>" + val + "</td>";
                });
                $("#tbldata").append('<tbody><tr>' + tbody + '</tr></tbody>');

            });
         }
  
    $scope.exportPdf = function () {
        //
        debugger;
        $scope.loadgrid($scope.BIllingReport);
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ type: 'pdf', jspdf: { orientation: 'l', margins: { right: 10, left: 10, top: 40, bottom: 40 }, autotable: { tableWidth: 'auto' } } });
        $("#tbldata").css('display', 'none');
     //   $scope.export();
    }
    $scope.exportExcel = function () {
        //
        $scope.loadgrid($scope.BIllingReport);
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ type: 'excel' });
        $("#tbldata").css('display', 'none');
    }
    $scope.exportCsv = function () {
        //
        $scope.loadgrid($scope.BIllingReport);
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ type: 'csv', numbers: { html: { decimalMark: '.', thousandsSeparator: ',' }, output: { decimalMark: ',', thousandsSeparator: '' } } });
        $("#tbldata").css('display', 'none');
    }

    $scope.exportXml = function () {
        //
        $scope.loadgrid($scope.BIllingReport);
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ type: 'xml' });
        $("#tbldata").css('display', 'none');
    }

    $scope.exportTxt = function () {
        //
        $scope.loadgrid($scope.BIllingReport);
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ type: 'txt' });
        $("#tbldata").css('display', 'none');
    }

    $scope.exportWord = function () {
        //
        $scope.loadgrid($scope.BIllingReport);
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ type: 'doc' });
        $("#tbldata").css('display', 'none');
    }

    $scope.export = function () {
        
        var table = $scope.BIllingReport;
        $("#tbldata").empty();
        

        $("#tbldata").append("<table><thead></thead><tbody>");
        for (var i = 0; i < $scope.BIllingReport.length; i++) {
            
            table += "<tr class='mytableReportItemsHeaderTR'>";
            $.each($scope.BIllingReport[i], function (key, value) {
                table += "<td style='width:auto;'>" + value + "</td>";
            });

            table += "</tr>";

        } for (var i = 0; i < $scope.BIllingReport.length; i++) {
            
            table += "<tr class='mytableReportItemsHeaderTR'>";
            $.each($scope.BIllingReport[i], function (key, value) {
                table += "<td style='width:auto;'>" + value + "</td>";
            });

            table += "</tr>";
        }
        $("#tbldata").append("</tbody></table>");


    }

});