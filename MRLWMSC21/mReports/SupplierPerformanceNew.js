var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('SupplierPerformanceNew', function ($scope, $http) {
    //alert('Hi');
    //$scope.getskus();
    var Tenantid = 0;
    var Warehouseid = 0;
    var mtypeid = 0;
    $('#txtTenant').val("");
    var PageIndex = 1;
    debugger;
    var textfieldname = $("#txtTenant");
    DropdownFunction(textfieldname);
    $("#txtTenant").autocomplete({
        source: function (request, response) {
            if (Warehouseid == 0) {
                showStickyToast(false, "Please Select Warehouse");
            }
            // this line added By lalitha on 23-12-2020  to Clear the TenantID when user de select the drop down value

            if ($('#txtTenant').val() == "" || $('#txtTenant').val() == undefined || $('#txtTenant').val() == null) {
                Tenantid = 0;
            }

            $.ajax({
               // url: '../mWebServices/FalconWebService.asmx/GetTenantList',
                //url: '../mWebServices/FalconWebService.asmx/LoadTenantDataByUserWH',
                //data: "{ 'prefix': '" + request.term + "'}",
                url: '../mWebServices/FalconWebService.asmx/LoadTenantDataByUserWH',
                data: "{ 'prefix': '" + request.term + "','whid': '" + Warehouseid + "' }",
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
            $("#txtSupplier").val('');
            $("#txtPartNo").val('');
            Tenantid = i.item.val;
          //  $scope.LoadWareHouse();

        },
        minLength: 0
    });


    $scope.LoadWareHouse = function () {
        $("#txtWarehouse").val("");

        var textfieldname = $("#txtWarehouse");
        debugger;
        DropdownFunction(textfieldname);
        $("#txtWarehouse").autocomplete({
            source: function (request, response) {
                if ($('#txtWarehouse').val() == "" || $('#txtWarehouse').val() == undefined || $('#txtWarehouse').val() == null) {
                    Warehouseid = 0;
                }

                $.ajax({
                    //url: '../mWebServices/FalconWebService.asmx/LoadWHForWHList',
                    //data: "{ 'prefix': '" + request.term + "','TenantID':'" + Tenantid + "'  }",
                    url: '../mWebServices/FalconWebService.asmx/LoaDWHListBasedonUser',
                    data: "{ 'prefix': '" + request.term + "' }",
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
                $("#txtWarehouse").val("");
                $("#txtPartNo").val("");
                Refnumber1 = "0";
            },
            minLength: 0
        });
        //ending of warehouse dropdown
    }
    $scope.LoadWareHouse();
    var Refnumber = '';
    $scope.getskus = function () {

        //alert('11');
        //
        $('#txtrefdnumber').val("");

        var textfieldname = $("#txtSupplier");
        DropdownFunction(textfieldname);
        $("#txtSupplier").autocomplete({
            source: function (request, response) {
                if (Tenantid == 0) {
                    showStickyToast(false, "Select Tenant");
                }
                if ($('#txtrefdnumber').val() == "" || $('#txtrefdnumber').val() == undefined || $('#txtrefdnumber').val() == null) {
                    Refnumber = 0;
                }

                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/LoadSupplierForSupplierPerformanceReport',
                    data: "{ 'prefix': '" + request.term + "','TenantID': '" + Tenantid + "'}",
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

    $scope.getskus();

    var Refnumber1 = '';
    $scope.getskus1 = function () {
        debugger;

        //alert('11');
        $('#txtrefdnumber').val("");

        var textfieldname = $("#txtPartNo");
        DropdownFunction(textfieldname);
        $("#txtPartNo").autocomplete({
            source: function (request, response) {

                if ($('#txtrefdnumber').val() == "" || $('#txtrefdnumber').val() == null || $('#txtrefdnumber').val() == undefined) {
                    Refnumber1 = 0;
                }

                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/GetMCode1',
                    //data: "{ 'prefix': '" + request.term + "'}",
                    data: "{ 'prefix': '" + request.term + "','SupplierID':" + Refnumber+"}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        debugger;
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

    $scope.Getgedetails = function (PageIndex) {
        
        debugger;
        //var supplier = $("#txtSupplier").val();
        var supplier = Refnumber;
        //var textfieldname = $("#txtPartNo").val();
        var textfieldname = Refnumber1;
        //alert(supplier);
        //alert(textfieldname);
        $scope.blockUI = true;
        var httpreq = {
            method: 'POST',
            url: 'SupplierPerformanceNew.aspx/GetBillingReportList',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: {
                'SupplierID': supplier, 'PartNo': textfieldname, 'TenantId': Tenantid, 'WareHouseID': Warehouseid, 'PageIndex': PageIndex
            },
            async: false
        }
        $http(httpreq).success(function (response) {
            $scope.blockUI = false;
            
            $scope.BIllingReport = response.d;
            if ($scope.BIllingReport == undefined || $scope.BIllingReport == null || $scope.BIllingReport.length == 0)
                showStickyToast(false, "No Data found");
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
        $("#tbldata").append("<table><thead><tr><td><img src='../Images/INV_TraceabilityAssured.png' style='height: 56px;' border='0'></td></tr><tr><th> Supplier</th><th>TotalPO </th><th>TotalPOLineNo</th><th>Line No's Received</th><th>TotalPOQty</th><th>ActualReceivedQty</th><th>AcceptedQty</th><th>DamagedQty</th><th>Discrepancy Qty.</th></thead><tbody>");
        if (table != null && table.length > 0) {
        for (var i = 0; i < table.length; i++) {
            $("#tbldata").append("<tr></td><td class='aligntext'>" + table[i].Supplier + "</td><td class='aligntext'>" + table[i].TotalPO + "</td><td class='aligntext'>" + table[i].TotalPOLineNo + "</td><td class='aligntext'>" + table[i].LineNo + "</td><td class='aligntext'>" + table[i].TotalPOQty + "</td><td class='aligntext'>" + table[i].ActualReceivedQty + "</td><td class='aligntext'>" + table[i].AcceptedQty + "</td><td class='aligntext'>" + table[i].DamagedQty + "</td><td class='aligntext'>" + table[i].DiscrepancyQty + "</td></tr>");
        }
        $("#tbldata").append("</tbody></table>");
    }
     else {

        $("#tbldata").append("<tr><td colspan='9' class='aligndate' style='background-color: white'>No Data found</td></tr>");
        $("#tbldata").append("</tbody>");
    }
}

});