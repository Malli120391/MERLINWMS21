var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('LogAuditReportNew', function ($scope, $http) {
    //alert('Hi');
    //$scope.getskus();

    var Refnumber = '';
    $scope.getskus = function () {

        //alert('11');
        $('#txtrefdnumber').val("");

        var textfieldname = $("#txtEmployee");
        DropdownFunction(textfieldname);
        $("#txtEmployee").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/LoadUsersDataForOperatorPerformanceReport',
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

    }

    $scope.Getgedetails = function () {
        //


        var FromDate = $("#txtFromdate").val();
        var textfieldname = $("#txttodate").val();
        var httpreq = {
            method: 'POST',
            url: 'LogAuditReportNew.aspx/GetBillingReportList',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'UserID': Refnumber, 'FromDate':FromDate, 'ToDate': textfieldname },
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
        $("#tbldata").append("<table><thead><tr><td><img src='../Images/INV_TraceabilityAssured.png' style='height: 56px;' border='0'></td></tr><tr><th> EmployeeCode</th><th>EmployeeName </th><th>IPAddress</th><th>PONumber</th><th>LoginTime</th><th>LogoutTime</th><th>SessionTime</th></thead><tbody>");
          if (table != null && table.length > 0) {
        for (var i = 0; i < table.length; i++) {
            $("#tbldata").append("<tr></td><td class='aligndate'>" + table[i].EmployeeCode + "</td><td class='aligndate'>" + table[i].EmployeeName + "</td><td class='aligndate'>" + table[i].IPAddress + "</td><td class='aligndate'>" + table[i].LoginTime + "</td><td class='aligndate'>" + table[i].LogoutTime + "</td><td class='aligndate'>" + table[i].SessionTime + "</td></tr>");
        }
        $("#tbldata").append("</tbody></table>");
    }
      else {

        $("#tbldata").append("<tr><td colspan='9' class='aligndate' style='background-color: white'>No Data found</td></tr>");
        $("#tbldata").append("</tbody>");
    }
}


});