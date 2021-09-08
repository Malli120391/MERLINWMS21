var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('3PLBillingReport', function ($scope, $http) {
    //alert('Hi');
    //$scope.getskus();

    var Refnumber = '';
    $scope.getskus = function () {
        
        //alert('11');
        //$('#txtrefdnumber').val("");
      
        //var textfieldname = $("#txtTanent");
        //DropdownFunction(textfieldname);
        //$("#txtTanent").autocomplete({
        //    source: function (request, response) {
        //        $.ajax({
        //            url: '../mWebServices/FalconWebService.asmx/LoadTenantsForReports',
        //            data: "{ 'prefix': '" + request.term + "'}",
        //            dataType: "json",
        //            type: "POST",
        //            contentType: "application/json; charset=utf-8",
        //            success: function (data) {
        //                response($.map(data.d, function (item) {
        //                    return {
        //                        label: item.split(',')[1],
        //                        val: item.split(',')[0]
        //                    }
        //                }))
        //            }
        //        });
        //    },
        //    select: function (e, i) {
        //        Refnumber = i.item.val;
        //        // alert(Refnumber);
        //    },
        //    minLength: 0
        //});

    }

    $scope.Getgedetails = function () {
        debugger;

        var FromDate = $("#txttodate").val();
        var textfieldname = $("#txttodate").val();
        var httpreq = {
            method: 'POST',
            url: '3PLBillingReportNew.aspx/GetBillingReportList',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'TanentID': Refnumber, 'FromDate': FromDate, 'ToDate': textfieldname },
            async: false
        }
        $http(httpreq).success(function (response) {
           // $scope.blockUI = false;
            
            $scope.BIllingReport = response.d;
        })
    }

    $('#txtrefdnumber').val("");

    var textfieldname = $("#txtTanent");
    DropdownFunction(textfieldname);
    $("#txtTanent").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/LoadTenantsForReports',
                data: "{ 'prefix': '" + request.term + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.split(',')[1],
                            val: item.split(',')[0]
                        }
                    }))
                }
            });
        },
        select: function (e, i) {
            Refnumber = i.item.val;
            // alert(Refnumber);
        },
        minLength: 0
    });

    $scope.exportPdf = function () {
        
        $scope.export();
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ type: 'pdf', jspdf: { orientation: 'l', margins: { right: 10, left: 10, top: 40, bottom: 40 }, autotable: { tableWidth: 'auto' } } });
        $("#tbldata").css('display', 'none');
        $scope.export();
    }
    $scope.exportExcel = function () {
        
        $scope.export();
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ type: 'excel' });
        $("#tbldata").css('display', 'none');
    }
    $scope.exportCsv = function () {
        
        $scope.export();
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ type: 'csv', numbers: { html: { decimalMark: '.', thousandsSeparator: ',' }, output: { decimalMark: ',', thousandsSeparator: '' } } });
        $("#tbldata").css('display', 'none');
    }

    $scope.exportXml = function () {
        
        $scope.export();
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ type: 'xml' });
        $("#tbldata").css('display', 'none');
    }

    $scope.exportTxt = function () {
        
        $scope.export();
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ type: 'txt' });
        $("#tbldata").css('display', 'none');
    }

    $scope.exportWord = function () {
        
        $scope.export();
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ type: 'doc' });
        $("#tbldata").css('display', 'none');
    }

    $scope.export = function () {
        
        var table = $scope.BIllingReport;
        $("#tbldata").empty();
        $("#tbldata").append("<table><thead><tr><td><img src='../Images/INV_TraceabilityAssured.png' style='height: 56px;' border='0'></td></tr><tr><th> TatentName</th><th>StoreRefNo </th><th>SupplierName</th><th>PONumber</th><th>Receipt</th><th>Services</th><th>UoM</th><th>UnitCost</th><th>After Disc</th><th>Qty</th><th>TotalCost</th><th>TotalCostAfterDisc</th><th>DiscWithOutTax</th><th>Tax</th></thead><tbody>");
        for (var i = 0; i < table.length; i++) {
            $("#tbldata").append("<tr></td><td class='aligndate'>" + table[i].TatentName + "</td><td class='aligndate'>" + table[i].StoreRefNo + "</td><td class='aligndate'>" + table[i].SupplierName + "</td><td class='aligndate'>" + table[i].PONumber + "</td><td class='aligndate'>" + table[i].Receipt + "</td><td class='aligndate'>" + table[i].Services + "</td><td class='aligndate'>" + table[i].UoM + "</td><td class='aligndate'>" + table[i].UnitCost + "</td><td class='aligndate'>" + table[i].UnitCostAfterDisc + "</td><td class='aligndate'>" + table[i].Quantity + "</td><td class='aligndate'>" + table[i].TotalCost + "</td><td class='aligndate'>" + table[i].TotalCostAfterDisc + "</td><td class='aligndate'>" + table[i].TotalCostAfterDiscWithOutTax + "</td><td class='aligndate'>" + table[i].Tax + "</td></tr>");
        }
        $("#tbldata").append("</tbody></table>");
    }


});