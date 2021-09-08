var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('PurchaseOrderListNew', function ($scope, $http) {
    //alert('Hi');
    //$scope.getskus();
    var TenantID = "";
    var TextFieldName = $("#txtTenant");
    DropdownFunction(TextFieldName);
    $("#txtTenant").autocomplete({
        
        source: function (request, response) {
            debugger;
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/LoadTenantDataFor3PL',
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
            debugger;
            TenantID=i.item.val;
            $("#txtPartNo").val("");
            $("#txtSupplier").val("");
        },
        minLength: 0
    });

    var Refponumber = '';
    $scope.getskus = function () {
        debugger;
        //alert('11');
        $('#txtrefdnumber').val("");

        var textfieldname = $("#txtPartNo");
        DropdownFunction(textfieldname);
        $("#txtPartNo").autocomplete({
            source: function (request, response) {

                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/LoadPONumbersForPurchaseOrderReport',
                    //data: "{ 'prefix': '" + request.term + "'}",
                    data: "{ 'prefix': '" + request.term + "','SupplierID':'0','TenentID':'" + TenantID + "'}",
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
                Refponumber = i.item.val;
                //alert(Refponumber);
            },
            minLength: 0
        });

    }

    var Refstatusnumber = '';
    $scope.getskus1 = function () {

        //alert('11');
        $('#txtrefdnumber').val("");

        var textfieldname = $("#txtPOStatus");
        DropdownFunction(textfieldname);
        $("#txtPOStatus").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/LoadPOStatus',
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
                Refstatusnumber = i.item.val;
                // alert(Refstatusnumber);
            },
            minLength: 0
        });

    }

    var Reftypenumber = '';
    $scope.getskus2 = function () {

        //alert('11');
        $('#txtrefdnumber').val("");

        var textfieldname = $("#txtPOType");
        DropdownFunction(textfieldname);
        $("#txtPOType").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/LoadPOTypes',
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
                Reftypenumber = i.item.val;
                // alert(Reftypenumber);
            },
            minLength: 0
        });

    }

    var Refsupplier = '';
    $scope.getskus3 = function () {

        //alert('11');
        $('#txtrefdnumber').val("");

        var textfieldname = $("#txtSupplier");
        DropdownFunction(textfieldname);
        $("#txtSupplier").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/LoadSupplierData',
                    data: "{ 'prefix': '" + request.term + "','TenantID':'" + TenantID + "'}",
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
                Refsupplier = i.item.val;
                // alert(Refsupplier);
            },
            minLength: 0
        });

    }

    $scope.Getgedetails = function () {
        debugger;
        //var pono = Refponumber;
        var pono = $("#txtPartNo").val();
        var PoDate = $("#txtPODate").val();
        var status = Refstatusnumber;
        var type = Reftypenumber;
        var supplier = Refsupplier;
        var FromDate = $("#txtFromdate").val();
        var textfieldname = $("#txttodate").val();
        if (TenantID == null || TenantID == undefined || TenantID=="") {
            TenantID = 0;
        }
        $scope.blockUI = true;
        var httpreq = {
            method: 'POST',
            url: 'PurchaseOrderListNew.aspx/GetBillingReportList',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'TenantID': TenantID, 'PONumber': pono, 'PODate': PoDate, 'StatusId': status, 'TypeID': type, 'SupplierId': supplier, 'FromDate': FromDate, 'ToDate': textfieldname },
            async: false
        }
        $http(httpreq).success(function (response) {
            $scope.blockUI = false;
            //
            debugger;
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
        $('#tbldata').tableExport({ type: 'csv'  });
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
        $("#tbldata").append("<table><thead><tr><td><img src='../Images/INV_TraceabilityAssured.png' style='height: 56px;' border='0'></td></tr><tr><th class='aligntext'> PONumber</th><th class='aligntext'>PODate </th><th class='aligntext'>Tenant</th><th class='aligntext'>Supplier</th><th class='aligntext'>POType</th><th class='aligntext'>TotalValue</th><th class='aligntext'>Currency</th><th class='aligntext'>Status</th></thead><tbody>");
        if (table != null && table.length > 0) {
            for (var i = 0; i < table.length; i++) {
                $("#tbldata").append("<tr></td><td class='aligntext'>" + table[i].PONumber + "</td><td class='aligntext'>" + table[i].PODate + "</td><td class='aligntext'>" + table[i].Tenant + "</td><td class='aligntext'>" + table[i].Supplier + "</td><td class='aligntext'>" + table[i].POType + "</td><td class='aligntext'>" + table[i].TotalValue + "</td><td class='aligntext'>" + table[i].Currency + "</td><td class='aligntext'>" + table[i].Status + "</td></tr>");
            }
            $("#tbldata").append("</tbody></table>");
        }
        else {

            $("#tbldata").append("<tr><td colspan='9' class='aligndate' style='background-color: white'>No Data found</td></tr>");
            $("#tbldata").append("</tbody>");
        }
    }

});