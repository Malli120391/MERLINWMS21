var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('RTReport', function ($scope, $http) {
    var ibdid = new URL(window.location.href).searchParams.get("ibdid");
    $scope.currentDate = new Date();
    var Partno = "";
    var httpreq = {
        method: 'POST',
        url: '../mReports/CurrentStockReport.aspx/GetLabels',
        headers: {
            'Content-Type': 'application/json; charset=utf-8',
            'dataType': 'json'
        },
        data: {}
    }
    $http(httpreq).success(function (response) {
        $scope.labels = response.d;
    });

    $scope.GetRTR = function () {        // debugger;

        var mid = 0;
        $("#divLoading").show();
        var httpreq = {
            method: 'POST',
            url: 'RTReport.aspx/getReceivingDetails',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'InbID': ibdid, 'MID': Partno },
            async: false
        }
        $http(httpreq).success(function (response) {
            debugger;
            var dt = JSON.parse(response.d).Table;
            if (dt === undefined || dt === null || dt.length === 0) {
                $("#divLoading").hide();
                $scope.RTRData = null;
                showStickyToast(false, "No Data Found", false);
                return false;
            }
            else {
                $scope.RTRData = dt;
                $("#divLoading").hide();
                //$("#hdnCount").val(dt.length);
            }
        });
    };
    $scope.isChecked = 0;
    $scope.selectAllCheckBoxs = function () {
        $scope.isChecked = !$scope.isChecked;
        $('.checkselectall').attr('checked', $scope.isChecked);
    };

    $scope.GetRTR();


    $scope.getPrintObjects = function () {
        debugger;
        $scope.PrintData = [];

        $(".checkedone").each(function () {
            if ($(this).is(":checked") === true) {
                var dt = "";
                dt = JSON.parse(JSON.stringify($(this).attr("data-obj")));
                $scope.PrintData.push(JSON.parse(dt));
            }
        });
        var labelid = $scope.ddlPrintLabel;
        if ($('#pid').val() == "3") {
            if ($("#netPrinterHost").val() == "") {
                showStickyToast(false, "Please enter Printer IP Address", false);
                return false;
            }
            if ($("#netPrinterPort").val() == "") {
                showStickyToast(false, "Please enter Printer Port", false);
                return false;
            }
        }
        if (labelid == "" || labelid == undefined) {
            showStickyToast(false, "Please Select Label", false);
            return false;
        }

        var length = $scope.PrintData.length;
        if (length == 0) {

            showStickyToast(false, "Please select at least one Item", false);
            return false;
        }
        var printerName = $("#installedPrinterName option:selected").text();
        var printerID = 0;
        if (printerName == "Microsoft XPS Document Writer") {
            printerID = 2;
        }
        else if (printerName == "Microsoft Print to PDF") { printerID = 2; }
        else { printerID = 0; }

        var httpreq = {
            method: 'POST',
            url: 'RTReport.aspx/GetPrint',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'printobj': $scope.PrintData, 'Printerid': printerID, 'LabelID': labelid, 'PrinterIP': $("#netPrinterHost").val(), 'PortID': $("#netPrinterPort").val() },
            async: false,
        }
        $http(httpreq).success(function (response) {
            debugger;
            var dt = response.d;
            if (dt == "") {
                //document.querySelector('#tbldatas').classList.remove("tableLoader");
                showStickyToast(false, "Error occured", false);
                return false;
            }
            else if (dt == "Success") {
                showStickyToast(true, "Successfully Printed", false);
            }
            else if (response.d == "Error") {
                showStickyToast(false, "Exception while generating PDF ", false);
                $("#divLoading").hide();
                return false;
            }
            else if (response.d == "No Data Found") {
                showStickyToast(false, "No Data Found ", false);
                $("#divLoading").hide();
                return false;
            }
            else if (response.d == "200") {
                //var obj = response.d;
                //showStickyToast(true, "PDF Generated Successfully ", false);
                //$("#divLoading").hide();
                //window.open('../mOutbound/PackingSlip/QRBarcode_PDFData.pdf');
                //return false;
            }
            else {
                $("#printerCommands").val("");
                $("#printerCommands").val(dt);
                javascript: doClientPrint();
                // document.querySelector('#tbldatas').classList.remove("tableLoader");
                showStickyToast(true, "Successfully Printed", false);
            }
        });
    };

    $("#txtMcode").val("");
    var textfieldname = $("#txtMcode");
    DropdownFunction(textfieldname);
    $("#txtMcode").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/LoadRTRMCodes',
                data: "{ 'prefix': '" + request.term + "','InboundID':'" + ibdid + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item,
                            //val: item.split(',')[1]
                        }
                    }))
                }
            });
        },
        select: function (e, i) {
            // debugger;
            Partno = "";
            Partno = i.item.label;
        },
        minLength: 0
    });


    //=================== RTR Print Functionality ==================//
    $scope.printData = function (divName) {
        debugger
        var Gcount = $scope.RTRData.length;
        var time = 100;
        if (Gcount > 100) { time = 75 } else
            if (Gcount > 500) { time = 50 };

        var panel = document.getElementById("PrintPanel");
        //var printWindow = window.open('', '', 'height=400,width=800,scrollbars=1,location=1,status=1,');
        var printWindow = window.open('', '', 'height=400,width=800,scrollbars=1,location=0,status=0,resizable=1');
        printWindow.document.write('<html><head><title>Receive Tally Sheet</title>');
        printWindow.document.write('</head><body >');
        printWindow.document.write(panel.innerHTML);
        printWindow.document.write('</body></html>');
        printWindow.document.write('<LINK href="../PrintStyle.css"  type="text/css" rel="stylesheet" media="print">');
        printWindow.document.close();
        setTimeout(function () {
            printWindow.print();
            printWindow.close();

        }, time * Gcount);
    };
});         