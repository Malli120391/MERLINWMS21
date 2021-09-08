var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('SalesOrderNew', function ($scope, $http) {
    //alert('Hi');
    //$scope.getskus();

    var Refnumber = '';
    $scope.getskus = function () {

        //alert('11');
        $('#txtrefdnumber').val("");

        var textfieldname = $("#txtSoNo");
        DropdownFunction(textfieldname);
        $("#txtSoNo").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/LoadSONumbersForSalesOrderReport',
                    //data: "{ 'prefix': '" + request.term + "'}",
                    data: "{ 'prefix': '" + request.term + "','CustomerID':'0','TenentID':'" + $("#hdnTenantID").val() + "'}",
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
                Refnumber = i.item.val;
                // alert(Refnumber);
            },
            minLength: 0
        });

    }

    var Refnumber1 = '';
    $scope.getskus1 = function () {

        //alert('11');
        $('#txtrefdnumber').val("");

        var textfieldname = $("#txtSOStatus");
        DropdownFunction(textfieldname);
        $("#txtSOStatus").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/LoadSOStatus',
                    data: "{ 'prefix': '" + request.term + "'}",
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
                Refnumber1 = i.item.val;
                // alert(Refnumber);
            },
            minLength: 0
        });

    }

    var Refnumber2 = '';
    $scope.getskus2 = function () {

        //alert('11');
        $('#txtrefdnumber').val("");

        var textfieldname = $("#txtSOType");
        DropdownFunction(textfieldname);
        $("#txtSOType").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/LoadSOTypes',
                    data: "{ 'prefix': '" + request.term + "'}",
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
                Refnumber2 = i.item.val;
                // alert(Refnumber);
            },
            minLength: 0
        });

    }

   

    $scope.Getgedetails = function () {
        debugger;
        var sono = $("#txtSoNo").val();
        var SoDate = $("#txtSODate").val();
        var sostatus = Refnumber1;
        var type =Refnumber2;
        var FromDate = $("#txtFromdate").val();
        var textfieldname = $("#txttodate").val();
        var httpreq = {
            method: 'POST',
            url: 'SalesOrderNew.aspx/GetBillingReportList',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'SONumber': sono, 'SODate': SoDate, 'StatusId': Refnumber1, 'TypeID': Refnumber2, 'FromDate': FromDate, 'ToDate': textfieldname },
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
        $('#tbldata').tableExport({ type: 'pdf', jspdf: { orientation: 'p', margins: { right: 10, left: 10, top: 40, bottom: 40 }, autotable: { tableWidth: 'auto' } } });
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
        $("#tbldata").append("<table><thead><tr><td><img src='../Images/INV_TraceabilityAssured.png' style='height: 56px;' border='0'></td></tr><tr><th> SONumber</th><th>SODate </th><th>Customer</th><th>SOType</th><th>Currency</th><th>GrossValue</th><th>NetValue</th><th>Tax</th><th>Status</th></thead><tbody>");
        if (table != null && table.length > 0) {
  for (var i = 0; i < table.length; i++) {
            $("#tbldata").append("<tr></td><td class='aligntext'>" + table[i].SONumber + "</td><td class='aligntext'>" + table[i].SODate + "</td><td class='aligntext'>" + table[i].Customer + "</td><td class='aligntext'>" + table[i].SOType + "</td><td class='aligntext'>" + table[i].Currency + "</td><td class='aligntext'>" + table[i].GrossValue + "</td><td class='aligntext'>" + table[i].NetValue + "</td><td class='aligntext'>" + table[i].Tax + "</td><td class='aligntext'>" + table[i].Status + "</td></tr>");
        }
        $("#tbldata").append("</tbody></table>");
    }
 else {

            $("#tbldata").append("<tr><td colspan='9' class='aligndate' style='background-color: white'>No Data Found</td></tr>");
            $("#tbldata").append("</tbody>");
        }
    }

});