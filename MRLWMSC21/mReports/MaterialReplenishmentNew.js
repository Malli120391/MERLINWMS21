var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('MaterialReplenishmentNew', function ($scope, $http) {
    //alert('Hi');
    //$scope.getskus();

    var Refnumber = '';
    var Tenantid = 0;
    var Warehouseid = 0;
    $('#txtTenant').val("");
    var textfieldname = $("#txtTenant");
    DropdownFunction(textfieldname);
    $("#txtTenant").autocomplete({
        source: function (request, response) {
            $.ajax({
                //url: '../mWebServices/FalconWebService.asmx/GetTenantList',
                //data: "{ 'prefix': '" + request.term + "'}",
                url: '../mWebServices/FalconWebService.asmx/LoadTenantsByWH',
                data: "{ 'prefix': '" + request.term + "','whid':'" + Warehouseid + "' }",
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
            Tenantid = i.item.val;
           // $scope.LoadWareHouse();

        },
        minLength: 0
    });
    //$scope.LoadWareHouse = function () {
        $("#txtWarehouse").val("");

        var textfieldname = $("#txtWarehouse");
        debugger;
        DropdownFunction(textfieldname);
        $("#txtWarehouse").autocomplete({
            source: function (request, response) {
                debugger;
                $.ajax({
                    //url: '../mWebServices/FalconWebService.asmx/LoadWHForWHList',
                    //data: "{ 'prefix': '" + request.term + "','TenantID':'" + Tenantid + "'  }",
                    url: '../mWebServices/FalconWebService.asmx/LoaDWHListBasedonUser',
                    data: "{ 'prefix': '" + request.term + "'  }",
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
                Warehouseid = i.item.val;
               // $("#txtWarehouse").val("");
                $("#hdnWarehouse").val(i.item.val);
                $("#txtTenant").val("");
                Tenantid = 0;
                $("#hdnTenant").val(0);

            },
            minLength: 0
        });
        //ending of warehouse dropdown
   // }  

    //$scope.getskus = function () {

        //alert('11');
        $('#txtrefdnumber').val("");

        var textfieldname = $("#txtPartNo");
        DropdownFunction(textfieldname);
        $("#txtPartNo").autocomplete({
            source: function (request, response) {
                debugger;
                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/GetReplenishedMaterialCode',
                    data: "{ 'prefix': '" + request.term + "','TenantID':'" + Tenantid + "'}",
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

   // }

    $scope.Getgedetails = function () {
        // 

        //if ($("#txtTenant").val() == "0" || $("#txtTenant").val() == null || $("#txtTenant").val() == undefined || $("#txtTenant").val() == "") {

        //    showStickyToast(false, " Please select Tenant ");
        //    return false;
        //}
        if ($("#txtWarehouse").val() == "0" || $("#txtWarehouse").val() == null || $("#txtWarehouse").val() == undefined || $("#txtWarehouse").val() == "") {

            showStickyToast(false, " Please select Warehouse ");
            return false;
        }

        var textfieldname = $("#txtPartNo").val();
        var httpreq = {
            method: 'POST',
            url: 'MaterialReplenishmentNew.aspx/GetBillingReportList',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: {
                'PartNo': textfieldname, 'TenantId': Tenantid, 'WareHouseId': Warehouseid
            },
            async: false
        }
        $http(httpreq).success(function (response) {
            // $scope.blockUI = false;
            //
            $scope.BIllingReport = response.d;
            if ($scope.BIllingReport == undefined || $scope.BIllingReport == null || $scope.BIllingReport.length == 0)
                showStickyToast(false, "No data found");
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
        $("#tbldata").append("<table><thead><tr><td><img src='../Images/INV_TraceabilityAssured.png' style='height: 56px;' border='0'></td></tr><tr><th> TenantName</th><th>StoreRefNo </th><th>SupplierName</th><th>PONumber</th><th>Receipt</th><th>Services</th><th>UoM</th><th>UnitCost</th><th>After Disc</th><th>Qty</th><th>TotalCost</th><th>TotalCostAfterDisc</th><th>DiscWithOutTax</th><th>Tax</th></thead><tbody>");
        if (table != null && table.length > 0) {
        for (var i = 0; i < table.length; i++) {
            $("#tbldata").append("<tr></td><td class='aligntext'>" + table[i].StoreRefNo + "</td><td class='aligntext'>" + table[i].DocReceivedDate + "</td><td class='aligntext'>" + table[i].TenantName + "</td><td class='aligntext'>" + table[i].SupplierName + "</td><td class='aligntext'>" + table[i].B / LorAirwayBillNo + "</td><td class='aligntext'>" + table[i].B / LorAirwayBillDate + "</td><td class='aligntext'>" + table[i].InvoiceNumber + "</td><td class='aligntext'>" + table[i].InvoiceDate + "</td><td class='aligntext'>" + table[i].InvoiceValue + "</td><td class='aligntext'>" + table[i].Code + "</td><td class='aligntext'>" + table[i].ExchangeRate + "</td></tr>");
        }
        $("#tbldata").append("</tbody></table>");
        }
        else {

            $("#tbldata").append("<tr><td colspan='14' class='aligndate' style='background-color: white'>No Data</td></tr>");
            $("#tbldata").append("</tbody>");
        }
    }


});