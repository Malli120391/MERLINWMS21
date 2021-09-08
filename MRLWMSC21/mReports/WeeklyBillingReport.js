var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('WeeklyBillingReport', function ($scope, $http) {
    // alert('Hi');

    //var Tenantid;
    //$scope.GetAllTenants = function () {
    //    debugger;
    //    Tenantid = $("#dpo option[value='" + $('#txtTenantName').val() + "']").attr('id');

    //    //alert(abc);

    //    //alert($scope.test);
    //    //alert($("#txtTenantName").attr(""));
    //    var tenant = $("#txtTenantName").val();

    //    if (tenant == "") {
    //        tenant = "";
    //    }
    //    else {
    //        tenant = tenant;
    //    }
    //    var httpreq2 = {
    //        method: 'POST',
    //        url: '../mReports/WeeklyBillingReport.aspx/GetTenants',
    //        headers: {
    //            'Content-Type': 'application/json; charset=utf-8',
    //            'dataType': 'json'
    //        },
    //        data: { 'tenant': tenant }
    //    }
    //    $http(httpreq2).success(function (response) {
    //        // debugger;
    //        $scope.Tenants = response.d;

    //    });

    //};
    var RefTenant = '';
    $scope.getTenant = function () {

        //alert('11');
        debugger;
        $('#txtTenant').val("");

        var textfieldname = $("#txtTenant");
        DropdownFunction(textfieldname);
        $("#txtTenant").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/LoadTenantsForReports',
                    data: "{ 'prefix': '" + request.term + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {

                        debugger;
                        $scope.Tenantdata = data;
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
                RefTenant = i.item.val;
                // alert(Refnumber);
                //$scope.ngtenant = i.item.val;
            },
            minLength: 0
        });

    }

    $scope.Getgedetails = function () {
        debugger;


        //alert($scope.ngtenant);
        var Tenant = RefTenant;
        var fromdate = $("#txtFromdate").val();
        var textfieldname = $("#txttodate").val();

        if (Tenant == "" || Tenant == undefined && fromdate == "" || fromdate == undefined && textfieldname == "" || textfieldname == undefined) {
            showStickyToast(false, "Please Select Tenant, From Date, End Date");
        }
        else {
            var obj = {};
            obj.TenantId = Tenant;
            obj.FromDate = fromdate;
            obj.ToDate = textfieldname;


            var httpreq = $.ajax({
                type: 'POST',
                url: 'WeeklyBillingReport.aspx/GetBillingReportList',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'dataType': 'json'
                },
                //data: {},
                //data: JSON.stringify({ Tenantid: Tenant, FromDate: fromdate, ToDate: textfieldname }),
                data: JSON.stringify(obj),
                async: false,
                success: function (data) {

                    debugger;

                    var hh = data.d.one;
                    var hh1 = data.d.two;
                    var hh2 = data.d.three;
                    var hh3 = data.d.four;
                    var hh4 = data.d.five;

                    var totalcharge = data.d.TotalCharge;
                    var columnTotal = data.d.ColumnTotal;
                    var InboundColumn = data.d.Column;

                    $scope.InboundReport = hh;
                    $scope.StorageReport = hh1;
                    $scope.InventoryReport = hh2;
                    $scope.GrandTotal = totalcharge;
                    $scope.column = columnTotal;
                    $scope.ICoulmn = InboundColumn;
                    $scope.Address = hh3;
                    $scope.AccAddress = hh4;

                    $(".divBillTo").css("display","flex");

                }
            });
        }
    }

    $scope.exportPdf = function () {
        debugger;
        $scope.export();
        $("#tbldataInb").css('display', 'block');
        $('#tbldataInb').tableExport({ type: 'pdf', jspdf: { orientation: 'l', margins: { right: 35, left: 35, top: 40, bottom: 40 }, autotable: { tableWidth: 'auto' } } });
        //$('#tbldataInb').tableExport({ type: 'pdf', jspdf: { orientation: 'p', autotable: { tableWidth: 'auto' } } });
        $("#tbldataInb").css('display', 'none');




    }
    $scope.exportExcel = function () {
        //debugger;
        $scope.export();
        $("#tbldataInb").css('display', 'block');
        $('#tbldataInb').tableExport({ type: 'excel' });
        $("#tbldataInb").css('display', 'none');


    }
    $scope.exportCsv = function () {
        //debugger;
        $scope.export();
        $("#tbldataInb").css('display', 'block');
        $('#tbldataInb').tableExport({ type: 'csv', numbers: { html: { decimalMark: '.', thousandsSeparator: ',' }, output: { decimalMark: ',', thousandsSeparator: '' } } });
        $("#tbldataInb").css('display', 'none');


    }

    $scope.exportXml = function () {
        //debugger;
        $scope.export();
        $("#tbldataInb").css('display', 'block');
        $('#tbldataInb').tableExport({ type: 'xml' });
        $("#tbldataInb").css('display', 'none');


    }

    $scope.exportTxt = function () {
        //debugger;
        $scope.export();
        $("#tbldataInb").css('display', 'block');
        $('#tbldataInb').tableExport({ type: 'txt' });
        $("#tbldataInb").css('display', 'none');


    }

    $scope.exportWord = function () {
        //debugger;
        $scope.export();
        $("#tbldataInb").css('display', 'block');
        $('#tbldataInb').tableExport({ type: 'doc' });
        $("#tbldataInb").css('display', 'none');


    }

    $scope.export = function () {
        debugger;

        var table = $scope.Address;
        $("#tbldataInb").empty();
        $("#tbldataInb").append("<table width='100%'><tr ><td style='text-align:center !important;font-size:30px;' ><b>BILLING REPORT</b></td><td></td></tr>");
        $("#tbldataInb").append("<tr><td style='text-align:left !important;'><b><div>From Date: " + $("#txtFromdate").val() + "<br/>" + " To Date:       "    + $("#txttodate").val() +"</div></b></td></tr>");
        
        var table = $scope.Address;
        $("#tbldataInb").append("<tr ><td ><b> Bill To:</b></td><td></td></tr>");
        for (var i = 0; i < table.length; i++) {
            $("#tbldataInb").append("<tr><td colspan='2' valign='top'><div style='height:500px;display:inline-block;border:2px;vertical-align:top;'>" + table[i].TenantName + "," + " <br/>" + table[i].Address1 + "," + " <br/>" + table[i].Address2 + "," + " <br/>" + table[i].City + "," + " <br/>" + table[i].State + "-" + table[i].ZIP + "." + " </div></td><td></td></tr>");
        }

        var table = $scope.AccAddress;
        $("#tbldataInb").append("<tr ><td ><b> Bill From:</b></td><td></td></tr>");
       
        //$("#tbldataInb").append("<tr><td><div> TRANSCRATE INTERNATIONAL LOGISTICS,<br/> 540 SAFAT MIRQAB, <br/> KUWAIT-13006</div></td><td></td></tr>");
        for (var i = 0; i < table.length; i++) {
            $("#tbldataInb").append("<tr><td colspan='2' valign='top'><div style='height:500px;display:inline-block;border:2px;vertical-align:top;'>" + table[i].Account + "," + " <br/>" + table[i].ComapanyName+"."+"</div></td><td></td></tr>");
        }
        

        var table = $scope.InboundReport;

        $("#tbldataInb").append("<tr><td ><b> Inbound & Outbound Charges</b></td><td style='background-color:red !important;'>&nbsp;</td></tr>");
        for (var i = 0; i < table.length; i++) {
            $("#tbldataInb").append("<tr><td class='aligndate' style='text-align:left !important;'>" + table[i].Week + "</td><td class='aligndate'style='text-align:right !important;'>" + table[i].Charge + "</td></tr>");
        }
        table = $scope.ICoulmn;
        $("#tbldataInb").append("<tr><td><b> Total</b></td><td class='aligndate'style='text-align:right !important;'>" + $scope.ICoulmn + "</td></tr>")


        $("#tbldataInb").append("<tr><td>&nbsp; </td><td>&nbsp;</td></tr>");


        table = $scope.StorageReport;
        $("#tbldataInb").append("<tr><td><b> Storage Charges</b></td><td>&nbsp;</td></tr>");
        for (var i = 0; i < table.length; i++) {
            $("#tbldataInb").append("<tr><td class='aligndate' style='text-align:left !important;'>" + table[i].Week + "</td><td class='aligndate'style='text-align:right !important;'>" + table[i].Charge + "</td></tr>");
        }
        table = $scope.column;
        $("#tbldataInb").append("<tr><td><b> Total</b></td><td class='aligndate' style='text-align:right !important;'>" + $scope.column + "</td></tr>");


        $("#tbldataInb").append("<tr><td>&nbsp; </td><td>&nbsp;</td></tr>");



        table = $scope.InventoryReport;
        $("#tbldataInb").append("<tr><td style='background-color:red;'><b> Inventory Charges</b></td><td>&nbsp;</td></tr>");
        for (var i = 0; i < table.length; i++) {
            $("#tbldataInb").append("<tr><td class='aligndate' style='text-align:left !important;'>" + table[i].Week + "</td><td class='aligndate' style='text-align:right !important;'>" + table[i].Charge + "</td></tr>");
        }

       



        table = $scope.GrandTotal;
        //$("#tbldataInb").append("<tr><td> Grand Total in KWD</td><td>&nbsp;</td></tr>");
        $("#tbldataInb").append("<tr><td> <b> Grand Total </b></td><td class='aligndate' style='text-align:right !important;'><b>" + $scope.GrandTotal + "</b></td></tr>");
        $("#tbldataInb").append("</table>");
    }
});