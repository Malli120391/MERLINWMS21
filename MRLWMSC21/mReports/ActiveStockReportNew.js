var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('ActiveStockReportNew', function ($scope, $http) {
    //alert('Hi');
    //$scope.getskus();

    var Refnumber = '';
    $scope.getskus = function () {

        //alert('11');
        $('#txtrefdnumber').val("");

        var textfieldname = $("#txtPartnumber");
        DropdownFunction(textfieldname);
        $("#txtPartnumber").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/GetMCode',
                    //data: "{ 'prefix': '" + request.term + "'}",
                    data: "{ 'prefix': '" + request.term + "','TenantID':'" + $("#hdnTenantID").val() + "'}",
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

        var textfieldname = $("#txtMaterialType");
        DropdownFunction(textfieldname);
        $("#txtMaterialType").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/LoadMTypeDataForReports',
                    //data: "{ 'prefix': '" + request.term + "'}",
                    data: "{ 'prefix': '" + request.term + "','TenantID':'" + $("#hdnTenantID").val() + "'}",
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

    var Refno = '';
    $scope.getlocation = function () {

        
       // 
       // $('#txtrefdnumber').val("");

        var textfieldname = $("#txtLocation");
        DropdownFunction(textfieldname);
       // alert(textfieldname);
        $("#txtLocation").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/GetRPTLocationListINCC',
                    data: "{ 'Prefix': '" + request.term + "'}",
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
                Refno = i.item.val;
                // alert(Refnumber);
            },
            minLength: 0
        });

    }

    $scope.Getgedetails = function () {
        
        debugger;

       // var FromDate = $("#txtFromdate").val();
        //var textfieldname = $("#txttodate").val();
        //var mcode = $("#txtPartnumber").val();
        var mcode = Refnumber;
        //var materialtype = $("#txtMaterialType").val();
        var materialtype = Refnumber1;
        //var Loc = $("#txtLocation").val();
        var Loc = Refno;
        var httpreq = {
            method: 'POST',
            url: 'ActiveStockReportNew.aspx/GetBillingReportList',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'MCode': mcode, 'MaterialType': materialtype, 'Location': Loc },
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
        debugger;
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
        debugger;
        var table = $scope.BIllingReport;
        $("#tbldata").empty();
        $("#tbldata").append("<table><thead><tr><td><img src='../Images/INV_TraceabilityAssured.png' style='height: 56px;' border='0'></td></tr><tr><th> PartNo</th><th>Description </th><th>UoM</th><th>Location</th><th>KitID</th><th>InOH</th><th>ObOH</th><th>AvlQty</th></thead><tbody>");
        if (table != null && table.length > 0) {
        for (var i = 0; i < table.length; i++) {
            $("#tbldata").append("<tr></td><td class='aligntext'>" + table[i].PartNo + "</td><td class='aligntext'>" + table[i].Description + "</td><td class='aligntext'>" + table[i].UoM + "</td><td class='aligntext'>" + table[i].Location + "</td><td class='aligntext'>" + table[i].KitID + "</td><td class='aligntext'>" + table[i].InOH + "</td><td class='aligntext'>" + table[i].ObOH + "</td><td class='aligntext'>" + table[i].AvlQty + "</td></tr>");
            //<td class='aligntext'>" + table[i].BatchNo + "</td> <td class='aligntext'>" + table[i].ExpDate + "</td> <td class='aligntext'>" + table[i].MfgDate + "</td> <td class='aligntext'>" + table[i].Plant + "</td> <td class='aligntext'>" + table[i].SerialNO + "</td> <td class='aligntext'>" + table[i].StockType + "</td>
        }
        $("#tbldata").append("</tbody></table>");
        }
        else {

            $("#tbldata").append("<tr><td colspan='8' class='aligndate' style='background-color: white'>No Data Found</td></tr>");
            $("#tbldata").append("</tbody>");
        }
    }


});