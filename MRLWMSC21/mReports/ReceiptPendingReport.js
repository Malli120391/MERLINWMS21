var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('ReceiptPendingReport', function ($scope, $http) {
    var Tenantid = 0;
    var Warehouseid = 0;
    $('#txtTenant').val("");
    var textfieldname = $("#txtTenant");
    DropdownFunction(textfieldname);
    $("#txtTenant").autocomplete({
        source: function (request, response) {
            $.ajax({
              //  url: '../mWebServices/FalconWebService.asmx/GetTenantList',
               // data: "{ 'prefix': '" + request.term + "'}",
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
            $("#txtStoreRefNo").val("");
        },
        minLength: 0
    });
        $("#txtWarehouse").val("");
        var textfieldname = $("#txtWarehouse");
        DropdownFunction(textfieldname);
        $("#txtWarehouse").autocomplete({
            source: function (request, response) {
                $.ajax({
                   // url: '../mWebServices/FalconWebService.asmx/LoadWHForWHList',
                   // data: "{ 'prefix': '" + request.term + "','TenantID':'" + Tenantid + "'  }",
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
                $("#txtStoreRefNo").val("");
                $('#txtTenant').val("");
            },
            minLength: 0
        });

    $("#txtStoreRefNo").val("");
    var textfieldname = $("#txtStoreRefNo");
    DropdownFunction(textfieldname);
    $("#txtStoreRefNo").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/LoadStoreRefNumbers',
                data: "{ 'prefix': '" + request.term + "','TenantId':'" + Tenantid + "','WarehouseId':'" + Warehouseid+"'}",
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
            //debugger;
            $("#hdnReceiptID").val(i.item.val);
        },
        minLength: 0
    });


    $scope.getReceiptPendingReport = function (pageid) {

        //debugger

        if ($("#txtTenant").val() == "" || Tenantid == 0) {
            Tenantid = 0;
            showStickyToast(false, " Please select Tenant");
            return false;
        }
        if ($("#txtWarehouse").val() == "" || Warehouseid == 0) {
            Warehouseid = 0;
            showStickyToast(false, " Please select Warehouse");
            return false;
        }
        if ($("#txtStoreRefNo").val() == "" || $("#hdnReceiptID").val() == "0") {
            $("#hdnReceiptID").val("0");
            showStickyToast(false, " Please select Receipt ID");
            return false;
        }
        $("#tbldatas").addClass("tableLoader");
        document.querySelector('#tbldatas').classList.add("tableLoader");
        var httpreq = {
            method: 'POST',
            url: 'ReceiptPendingReport.aspx/getReceiptPendingReport',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'TenantID': Tenantid, 'WHID': Warehouseid, 'InbID': $("#hdnReceiptID").val(), 'IsExport': 0, 'NoofRecords': 25, 'PageNo': pageid },
            async: false
        }
        $http(httpreq).success(function (response) {
            debugger
            //var dt = JSON.parse(response.d).Table;
            //if (dt != null && dt.length > 0) {
            //    $scope.ReceiptPending = dt;
            //    document.querySelector('#tbldatas').classList.remove("tableLoader");
            //}
            //else {
            //    $scope.ReceiptPending = null;
            //    showStickyToast(false, "No Data Found");
            //    document.querySelector('#tbldatas').classList.remove("tableLoader");
            //    return false;
            //}

            var dt = JSON.parse(response.d).Table;
            if (dt == undefined || dt == null || dt.length == 0) {
                showStickyToast(false, "No Data Found", false);
                $scope.ReceiptPending = null;
                $scope.Totalrecords = 0;
                document.querySelector('#tbldatas').classList.remove("tableLoader");
                return false;
            }
            $scope.ReceiptPending = dt;
            $scope.Totalrecords = $scope.ReceiptPending[0].TotalRecords;
            document.querySelector('#tbldatas').classList.remove("tableLoader");
        })
    }

    $scope.exportExcel = function () {
        //debugger
        if ($scope.ReceiptPending == null || $scope.ReceiptPending == undefined) {
            showStickyToast(false, "No Data Found", false);
            return false;
        }
        if ($("#txtTenant").val() == "" || Tenantid == 0) {
            Tenantid = 0;
            showStickyToast(false, " Please select Tenant");
            return false;
        }
        if ($("#txtWarehouse").val() == "" || Warehouseid == 0) {
            Warehouseid = 0;
            showStickyToast(false, " Please select Warehouse");
            return false;
        }
        if ($("#txtStoreRefNo").val() == "" || $("#hdnReceiptID").val() == "0") {
            $("#hdnReceiptID").val("0");
            showStickyToast(false, " Please select Receipt ID");
            return false;
        }
        $("#tbldatas").addClass("tableLoader");
        document.querySelector('#tbldatas').classList.add("tableLoader");
        var httpreq = {
            method: 'POST',
            url: 'ReceiptPendingReport.aspx/getReceiptPendingReport_Export',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'TenantID': Tenantid, 'WHID': Warehouseid, 'InbID': $("#hdnReceiptID").val(), 'IsExport': 1, 'NoofRecords': 200000, 'PageNo': 1 },
            async: false
        }
        $http(httpreq).success(function (response) {
            if (response.d == "No Data Found") {
                showStickyToast(false, "No Data Found", false);
                document.querySelector('#tbldatas').classList.remove("tableLoader");
                return false;
            }
            else {
                window.open('../ExcelData/' + response.d + ".xlsx");
            }
            document.querySelector('#tbldatas').classList.remove("tableLoader");
        })
    }


    //$scope.exportExcel = function () {
    //    if ($scope.ReceiptPending == null || $scope.ReceiptPending == undefined) {
    //        showStickyToast(false, "No Data Found", false);
    //        return false;
    //    }
    //    $scope.export();
    //    $("#tbldata").css('display', 'block');
    //    $('#tbldata').tableExport({ type: 'excel' });
    //    $("#tbldata").css('display', 'none');
    //}

    //$scope.export = function () {

    //    var table = $scope.ReceiptPending;

    //    $("#tbldata").empty();
    //    $("#tbldata").append("<table><thead><tr><td><img src='../Images/INV_TraceabilityAssured.png' style='height: 56px;' border='0'></td></tr><tr><th> Receipt Ref. No.</th><th>PO Number </th><th>Invoice Number</th><th>MCode</th><th>Line Number</th><th>Expected Qty.</th><th>Received Qty.</th><th>Pending Qty.</th></thead><tbody>");
    //    if (table != null && table.length > 0) {
    //        for (var i = 0; i < table.length; i++) {
    //            $("#tbldata").append("<tr></td><td class='aligntext'>" + table[i].StoreRefNo + "</td><td class='aligntext'>" + table[i].PONumber + "</td><td class='aligntext'>" + table[i].InvoiceNumber + "</td><td class='aligntext'>" + table[i].MCode + "</td><td class='aligntext'>" + table[i].LineNumber + "</td><td class='aligntext'>" + table[i].InvoiceQuantity + "</td><td class='aligntext'>" + table[i].ReceivedQty + "</td><td class='aligntext'>" + table[i].PendingQty + "</td></tr>");
    //        }
    //        $("#tbldata").append("</tbody></table>");
    //    }
    //    else {

    //        $("#tbldata").append("<tr><td colspan='6' class='aligndate' style='background-color: white'>No Data found</td></tr>");
    //        $("#tbldata").append("</tbody>");
    //    }
    //}

});