var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('MaterialActivityReportNew', function ($scope, $http) {
    //alert("11");
    $scope.inwardhide = false;
    $scope.outwardhide = false;
    var Refnumber = '';
    $scope.getskus = function () {

        //alert('11');
        $('#txtrefdnumber').val("");

        var textfieldname = $("#txtPartNo");
        DropdownFunction(textfieldname);
        $("#txtPartNo").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/GetReplenishedMaterialCode',
                    data: "{ 'prefix': '" + request.term + "'}",
                    //data: "{ 'prefix': '" + request.term + "','TenentID' : '" + $("#hdnTenantID").val() + "'}",
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

                 //alert(Refnumber);
            },
            minLength: 0
        });

    }

    $scope.Getgedetails = function () {
        
        // var partno = $("#txtPartNo").val();
        var partno = Refnumber;

        //var batchno = $("#txtbatchno").val();
        var httpreq = {
            method: 'POST',
            url: 'MaterialActivityReportNew.aspx/GetBillingReportList',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'MCode': partno },
            async: false
        }
        $http(httpreq).success(function (response) {
            // $scope.blockUI = false;
            //
          
            $scope.BIllingReportIn = response.d.one;
            $scope.BIllingReportOut = response.d.two;

            if ($scope.BIllingReportIn == undefined || $scope.BIllingReportIn == null || $scope.BIllingReportIn.length == 0) {
                $scope.inwardhide = false;
                $scope.outwardhide = false;
                showStickyToast(false, "No Data Found for inbound activity");
            }
            else {
                $scope.inwardhide = true;

            }
            if ($scope.BIllingReportOut == undefined || $scope.BIllingReportOut == null || $scope.BIllingReportOut.length == 0) {

                $scope.inwardhide = false;
                $scope.outwardhide = false;
                showStickyToast(false, "No Data Found for outbound activity");
            }
            else {
                $scope.outwardhide = true;
            }
        })
    }

    $scope.exportPdf = function () {
      
        $scope.export();
        $("#tbldataIn").css('display', 'block');
        $('#tbldataIn').tableExport({ type: 'pdf', jspdf: { orientation: 'l', margins: { right: 10, left: 10, top: 40, bottom: 40 }, autotable: { tableWidth: 'auto' } } });
        $("#tbldataIn").css('display', 'none');
        $scope.export();

        $scope.export();
        $("#tbldataOut").css('display', 'block');
        $('#tbldataOut').tableExport({ type: 'pdf', jspdf: { orientation: 'l', margins: { right: 10, left: 10, top: 40, bottom: 40 }, autotable: { tableWidth: 'auto' } } });
        $("#tbldataOut").css('display', 'none');
        $scope.export();
    }
    $scope.exportExcel = function () {
        //
        $scope.export();
        $("#tbldataIn").css('display', 'block');
        $('#tbldataIn').tableExport({ type: 'excel' });
        $("#tbldataIn").css('display', 'none');

        $scope.export();
        $("#tbldataOut").css('display', 'block');
        $('#tbldataOut').tableExport({ type: 'excel' });
        $("#tbldataOut").css('display', 'none');
    }
    $scope.exportCsv = function () {
        //
        $scope.export();
        $("#tbldataIn").css('display', 'block');
        $('#tbldataIn').tableExport({ type: 'csv', numbers: { html: { decimalMark: '.', thousandsSeparator: ',' }, output: { decimalMark: ',', thousandsSeparator: '' } } });
        $("#tbldataIn").css('display', 'none');

        $scope.export();
        $("#tbldataOut").css('display', 'block');
        $('#tbldataOut').tableExport({ type: 'csv', numbers: { html: { decimalMark: '.', thousandsSeparator: ',' }, output: { decimalMark: ',', thousandsSeparator: '' } } });
        $("#tbldataOut").css('display', 'none');
    }

    $scope.exportXml = function () {
        //
        $scope.export();
        $("#tbldataIn").css('display', 'block');
        $('#tbldataIn').tableExport({ type: 'xml' });
        $("#tbldataIn").css('display', 'none');

        $scope.export();
        $("#tbldataOut").css('display', 'block');
        $('#tbldataOut').tableExport({ type: 'xml' });
        $("#tbldataOut").css('display', 'none');
    }

    $scope.exportTxt = function () {
        //
        $scope.export();
        $("#tbldataIn").css('display', 'block');
        $('#tbldataIn').tableExport({ type: 'txt' });
        $("#tbldataIn").css('display', 'none');

        $scope.export();
        $("#tbldataOut").css('display', 'block');
        $('#tbldataOut').tableExport({ type: 'txt' });
        $("#tbldataOut").css('display', 'none');
    }

    $scope.exportWord = function () {
        //
        $scope.export();
        $("#tbldataIn").css('display', 'block');
        $('#tbldataIn').tableExport({ type: 'doc' });
        $("#tbldataIn").css('display', 'none');

        $scope.export();
        $("#tbldataOut").css('display', 'block');
        $('#tbldataOut').tableExport({ type: 'doc' });
        $("#tbldataOut").css('display', 'none');
    }

    $scope.export = function () {
      
        var table = $scope.BIllingReportIn;
        $("#tbldataIn").empty();
        $("#tbldataIn").append("<table><thead><tr><td><img src='../Images/INV_TraceabilityAssured.png' style='height: 56px;' border='0'></td></tr><tr><th> TransDate</th><th>Supplier </th><th>PONumber</th><th>StoreRefNo</th><th>ReceivedQty</th><th>UoM</th><th>Location</th><th>MfgDate</th><th>SerialNo</th><th>BatchNo</th></thead><tbody>");
        for (var i = 0; i < table.length; i++) {
            $("#tbldataIn").append("<tr></td><td class='aligndate'>" + table[i].TransDate + "</td><td class='aligndate'>" + table[i].Supplier + "</td><td class='aligndate'>" + table[i].PONumber + "</td><td class='aligndate'>" + table[i].StoreRefNo + "</td><td class='aligndate'>" + table[i].UoM + "</td><td class='aligndate'>" + table[i].ReceivedQty + "</td><td class='aligndate'>" + table[i].Location + "</td><td class='aligndate'>" + table[i].MfgDate + "</td><td class='aligndate'>" + table[i].SerialNo + "</td><td class='aligndate'>" + table[i].BatchNo + "</td></tr>");
        }
        $("#tbldataIn").append("</tbody></table>");


        table = $scope.BIllingReportOut;
        $("#tbldataOut").empty();
        $("#tbldataOut").append("<table><thead><tr><td><img src='../Images/INV_TraceabilityAssured.png' style='height: 56px;' border='0'></td></tr><tr><th> TransDate</th><th>Customer</th><th>SONumber</th><th>OBDNo</th><th>UoM</th><th>PickedQty</th><th>Location</th><th>ExpDate</th><th>SerialNo</th><th>BatchNo</th></thead><tbody>");
        for (var i = 0; i < table.length; i++) {
            $("#tbldataOut").append("<tr></td><td class='aligndate'>" + table[i].TransDate + "</td><td class='aligndate'>" + table[i].Customer + "</td><td class='aligndate'>" + table[i].SONumber + "</td><td class='aligndate'>" + table[i].OBDNo + "</td><td class='aligndate'>" + table[i].UoM + "</td><td class='aligndate'>" + table[i].PickedQty + "</td><td class='aligndate'>" + table[i].Location + "</td><td class='aligndate'>" + table[i].ExpDate + "</td><td class='aligndate'>" + table[i].SerialNo + "</td><td class='aligndate'>" + table[i].BatchNo + "</td></tr>");
        }
        $("#tbldataOut").append("</tbody></table>");
    }


});