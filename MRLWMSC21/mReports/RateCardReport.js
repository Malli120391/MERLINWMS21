var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('RateCard', function ($scope, $http) {
   //  alert("11");

    var Refobject = '';
    $scope.getTables = function () {

        //alert('11');
        //
        $('#txtrefdnumber').val("");

        var textfieldname = $("#txtTenant");
        DropdownFunction(textfieldname);
        $("#txtTenant").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/GetTenantList',
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
                Refobject = i.item.val;
                // alert(Refnumber);
                //$scope.ngtenant = i.item.val;
            },
            minLength: 0
        });

    }

    $scope.Getgedetails = function () {
        
        var tenantid = Refobject;
        var fromdate = $("#txtFromdate").val();
        var textfieldname = $("#txtTodate").val();

        if (tenantid == undefined || tenantid == "" && fromdate == undefined || fromdate == "" && textfieldname == undefined || textfieldname == "") {
            showStickyToast(false, "Please Select Tenant, From Date, End Date");
        }
        else {
        
        var httpreq = {
            method: 'POST',
            url: 'RateCardReport.aspx/GetBillingReportList',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'TenantID': tenantid, 'Fromdate': fromdate, 'Todate': textfieldname },
            async: false
        }
        $http(httpreq).success(function (response) {
            // $scope.blockUI = false;
            
            $scope.BIllingReport = response.d.one;
            $scope.InboundReport = response.d.two;
            $scope.OutboundReport = response.d.three;

            if ($scope.BIllingReport == undefined || $scope.BIllingReport == null || $scope.BIllingReport.length == 0)
            {
                showStickyToast(false, "No Data Found For Tenant Details");
            }


            if ($scope.InboundReport == undefined || $scope.InboundReport == null || $scope.InboundReport.length == 0) {
                showStickyToast(false, "No Data Found For Inbound Activity");
            }


            if ($scope.OutboundReport == undefined || $scope.OutboundReport == null || $scope.OutboundReport.length == 0) {
                showStickyToast(false, "No Data Found For Outbound Activity");
            }
        })
        }
    }
    //}
    //$http(httpreq).success(function (response) {
    //    // $scope.blockUI = false;
    //    //
    //    $scope.BIllingReport = response.d;
    //    if ($scope.BIllingReport == undefined || $scope.BIllingReport == null || $scope.BIllingReport.length == 0)
    //        showStickyToast(false, "No Data Found");
    //})


    $scope.exportPdf = function () {
        
        $scope.export();
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ type: 'pdf', jspdf: { orientation: 'p', margins: { right: 35, left: 35, top: 40, bottom: 40 }, autotable: { tableWidth: 'auto' } } });
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
        $("#tbldata").append("<table width='100%'><tr ><td colspan='5' style='text-align:center !important;font-size:30px;' ><b> Rate Card Report</b></td></tr>");
        $("#tbldata").append("<tr><td colspan='5' style='text-align:left !important;'><b>From Date: " + $("#txtFromdate").val() + "</b></td></tr>");
        $("#tbldata").append("<tr><td colspan='5' style='text-align:left !important;'><b>To Date: " + $("#txtTodate").val() + "</b></td></tr>");
        
        var table = $scope.BIllingReport;

        $("#tbldata").append("<tr><td colspan='5' style='text-align:center !important;font-size:30px;'><b>Tenant Details</b></td></tr>");
        $("#tbldata").append("<tr><td><b>Tenant Name</b></td><td style='text-align:center !important;font-size:30px;'><b>Activity Rate Type</b></td><td style='text-align:center !important;font-size:30px;'><b>Activity Rate Name</b></td><td><b>Cost Price</b></td></tr>");
        for (var i = 0; i < table.length; i++) {
            $("#tbldata").append("<tr><td class='aligndate' style='text-align:left !important;'>" + table[i].TenantName + "</td><td class='aligndate'>" + table[i].ActivityRateType + "</td><td class='aligndate'>" + table[i].ActivityRateName + "</td><td class='aligndate'>" + table[i].CostPrice + "</td></tr>");
        }

        //$("#tbldata").append("<tr><td>&nbsp; </td><td>&nbsp;</td></tr>");


        table = $scope.InboundReport;
        $("#tbldata").append("<tr><td colspan='5' style='text-align:center !important;font-size:30px;'><b>Inbound Activity</b></td></tr>");
        $("#tbldata").append("<tr><td><b>Tenant Name</b></td><td style='text-align:center !important;font-size:30px;'><b>Store Ref. No.</b></td><td style='text-align:center !important;font-size:30px;'><b>Activity Rate Name</b></td><td><b>Quantity</b></td><td><b>Cost Price</b></td></tr>");
        for (var i = 0; i < table.length; i++) {
            $("#tbldata").append("<tr><td class='aligndate' style='text-align:left !important;'>" + table[i].TenantName + "</td><td class='aligndate'>" + table[i].StoreRefNo + "</td><td class='aligndate'>" + table[i].ActivityRateName + "</td><td class='aligndate'>" + table[i].Quantity + "</td><td class='aligndate'>" + table[i].CostPrice + "</td></tr>");
        }


        //$("#tbldata").append("<tr><td>&nbsp; </td><td>&nbsp;</td></tr>");



        table = $scope.OutboundReport;
        $("#tbldata").append("<tr><td colspan='5' style='text-align:center !important;font-size:30px;'><b>Outbound Activity</b></td><td style='background-color:red !important;'>&nbsp;</td></tr>");
        $("#tbldata").append("<tr><td><b>Tenant Name</b></td><td style='text-align:center !important;font-size:30px;'><b>OBD No.</b></td><td style='text-align:center !important;font-size:30px;'><b>Activity Rate Name</b></td><td><b>Quantity</b></td><td><b>Cost Price</b></td></tr>");
        for (var i = 0; i < table.length; i++) {
            $("#tbldata").append("<tr><td class='aligndate' style='text-align:left !important;'>" + table[i].TenantName + "</td><td class='aligndate'>" + table[i].OBDNumber + "</td><td class='aligndate'>" + table[i].ActivityRateName + "</td><td class='aligndate'>" + table[i].Quantity + "</td><td class='aligndate'>" + table[i].CostPrice + "</td></tr>");
        }

        
        $("#tbldata").append("</table>");
    }


});