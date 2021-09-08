var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('WHStockInfoReport', function ($scope, $http) {
    $scope.TenantID = 0;
    var Tenantid = 0;
    var Warehouseid = 0;
    var mid = 0;
    $(".hideContent").hide();
    $('#txtTenant').val("");
    var textfieldname = $("#txtTenant");
    DropdownFunction(textfieldname);
    $("#txtTenant").autocomplete({
        source: function (request, response) {
            if ($("#txtTenant").val() == '') {
                TenantID = 0;
            }
            $.ajax({
               // url: '../mWebServices/FalconWebService.asmx/GetTenantList',
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
            $scope.TenantID = i.item.val;
            $("#hdnTenant").val(i.item.val);
            $("#txtItem").val("");
            $("#txtDate").val("");
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
            $("#hdnWarehouse").val(i.item.val);
            $('#txtTenant').val("");
            $("#txtItem").val("");
            $("#txtDate").val("");
        },
        minLength: 0
    });

    debugger;
    $("#txtItem").val("");
    var textfieldname = $("#txtItem");
    DropdownFunction(textfieldname);
    $("#txtItem").autocomplete({

        
        source: function (request, response) {
            debugger;
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/GetMCodeForWHStock',
                data: "{ 'prefix': '" + request.term + "','TenantID':'" + Tenantid + "'  }",
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
            mid = i.item.val;
            $("#hdnItem").val(i.item.val);
        },
        minLength: 0
    });

    $scope.GetWHStockDataReport = function () {
        //debugger
        if ($("#txtTenant").val() == "0" || $("#txtTenant").val() == "") {
            Tenantid = 0;
            showStickyToast(false, "Please select Tenant ");
            return false;
        }
        if ($("#txtWarehouse").val() == "0" || $("#txtWarehouse").val() == "") {
            Warehouseid = 0;
            showStickyToast(false, "Please select Warehouse ");
            return false;
        }
        if ($("#txtDate").val() == "") {
            showStickyToast(false, "Please select Date ");
            return false;
        }
        if ($("#txtItem").val() == "") {
            mid = 0;
        }
        $(".hideContent").show();
        $scope.warehouse = Warehouseid;
        $("#tbldatas").addClass("tableLoader");
        document.querySelector('#tbldatas').classList.add("tableLoader");
        var httpreq = {
            method: 'POST',
            url: 'WHStockInfoReport.aspx/GetWHData',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: {
                'TenantID': Tenantid, 'WHID': Warehouseid, 'date': $("#txtDate").val(),'MID':mid
            },
            async: false
        }
        $http(httpreq).success(function (response) {
            debugger
            var dt = JSON.parse(response.d).Table;
            $scope.TenantData = JSON.parse(response.d).Table1[0];
            if (dt != null && dt.length > 0) {
                $scope.WHStockData = dt;
                $("#hdnCount").val(dt.length);
                document.querySelector('#tbldatas').classList.remove("tableLoader");
            }
            else {
                $scope.WHStockData = null;
                showStickyToast(false, "No Data Found");
                document.querySelector('#tbldatas').classList.remove("tableLoader");
                return false;
            }
        })
    }

    $scope.exportExcelData = function () {
        debugger
        if ($("#txtTenant").val() == "0" || $("#txtTenant").val() == "") {
            Tenantid = 0;
            showStickyToast(false, "Please select Tenant ");
            return false;
        }
        if ($("#txtWarehouse").val() == "0" || $("#txtWarehouse").val() == "") {
            Warehouseid = 0;
            showStickyToast(false, "Please select Warehouse ");
            return false;
        }
        if ($("#txtDate").val() == "") {
            showStickyToast(false, "Please select Date ");
            return false;
        }
        if ($("#txtItem").val() == "") {
            mid = 0;
        }
        $("#tbldatas").addClass("tableLoader");
        document.querySelector('#tbldatas').classList.add("tableLoader");
        var httpreq = {
            method: 'POST',
            url: 'WHStockInfoReport.aspx/GetWHData_Export',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: {
                'TenantID': Tenantid, 'WHID': Warehouseid, 'date': $("#txtDate").val(), 'MID': mid
            },
            async: false
        }
        $http(httpreq).success(function (response) {
            debugger
            var dt = JSON.parse(response.d).Table;
            $scope.TenantData = JSON.parse(response.d).Table1[0];
            if (dt != null && dt.length > 0) {
                $scope.WHStockData = dt;
                document.querySelector('#tbldatas').classList.remove("tableLoader");
            }
            else {
                $scope.WHStockData = null;
                showStickyToast(false, "No Data Found");
                document.querySelector('#tbldatas').classList.remove("tableLoader");
                return false;
            }
        })
    }

    $scope.exportExcel = function () {
        if ($scope.WHStockData == null || $scope.WHStockData == undefined) {
            showStickyToast(false, "No Data Found", false);
            return false;
        }
        $scope.export();
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ type: 'excel' });
        $("#tbldata").css('display', 'none');
    }

    $scope.export = function () {

        var table = $scope.WHStockData;
        var tableFormat = "";
        $("#tbldata").empty();
        tableFormat += "<table><thead><tr><th>Item Code</th>";
        if ($scope.TenantID == 58) {
            tableFormat += "<th>Size</th><th>Collection</th><th>Description</th><th>Category</th><th>UoM/Qty.</th><th>Mfg Date.</th><th>Exp. Date</th><th>Batch No.</th><th>Onhand Qty.</th><th>Allocated Qty.</th><th>Good Qty.</th><th>Damaged Qty.</th><th>Volume(CBM)</th></thead><tbody>";
        }
        else {
            tableFormat += "<th>Serial No.</th><th>PO Number</th><th>Airway Bill No.</th><th>Category</th><th>UoM/Qty.</th><th>Mfg Date.</th><th>Exp. Date</th><th>Batch No.</th><th>Onhand Qty.</th><th>Allocated Qty.</th><th>Good Qty.</th><th>Damaged Qty.</th><th>Volume(CBM)</th></thead><tbody>";
        }

        if (table != null && table.length > 0) {
            for (var i = 0; i < table.length; i++) {
                tableFormat += "<tr></td><td class='aligntext'>" + table[i].MCode + "</td><td class='aligntext'>" + table[i].MCodeAlternative1 + "</td><td class='aligntext'>" + table[i].MCodeAlternative2 + "</td><td class='aligntext'>" + table[i].MDescription + "</td><td class='aligntext'>" + table[i].MType + "</td><td class='aligntext'>" + table[i].UoMQty + "</td><td class='aligntext'>" + table[i].MfgDate + "</td><td class='aligntext'>" + table[i].ExpiryDate + "</td><td class='aligntext'>" + table[i].BatchNo + "</td><td class='aligntext'>" + table[i].ClosingStockQty + "</td><td class='aligntext'>" + table[i].SOQuantity + "</td><td class='aligntext'>" + table[i].GoodQty + "</td><td class='aligntext'>" + table[i].DamagedQty + "</td><td class='aligntext'>" + table[i].ItemVolume + "</td></tr>";
            }
            tableFormat += "</tbody></table>";
            $("#tbldata").append(tableFormat);
        }
        else {
            tableFormat += "<tr><td colspan='6' class='aligndate' style='background-color: white'>No Data found</td></tr></tbody></table>";
            $("#tbldata").append(tableFormat);
        }





        //var table = $scope.WHStockData;

        //$("#tbldata").empty();
        //$("#tbldata").append("<table><thead><tr><td><img src='../Images/INV_TraceabilityAssured.png' style='height: 56px;' border='0'></td></tr><tr><th>Item Code</th><th>Airway Bill No.</th><th>Category</th><th>UoM/Qty.</th><th>Mfg Date.</th><th>Exp. Date</th><th>Batch No.</th><th>Onhand Qty.</th><th>Allocated Qty.</th><th>Good Qty.</th><th>Damaged Qty.</th><th>Volume(CBM)</th></thead><tbody>");
        //if (table != null && table.length > 0) {
        //    for (var i = 0; i < table.length; i++) {
        //        $("#tbldata").append("<tr></td><td class='aligntext'>" + table[i].MCode + "</td><td class='aligntext'>" + table[i].MDescription + "</td><td class='aligntext'>" + table[i].MType + "</td><td class='aligntext'>" + table[i].UoMQty + "</td><td class='aligntext'>" + table[i].MfgDate + "</td><td class='aligntext'>" + table[i].ExpiryDate + "</td><td class='aligntext'>" + table[i].BatchNo + "</td><td class='aligntext'>" + table[i].ClosingStockQty + "</td><td class='aligntext'>" + table[i].SOQuantity + "</td><td class='aligntext'>" + table[i].GoodQty + "</td><td class='aligntext'>" + table[i].DamagedQty + "</td><td class='aligntext'>" + table[i].ItemVolume + "</td></tr>");
        //    }
        //    $("#tbldata").append("</tbody></table>");
        //}
        //else {

        //    $("#tbldata").append("<tr><td colspan='6' class='aligndate' style='background-color: white'>No Data found</td></tr>");
        //    $("#tbldata").append("</tbody>");
        //}
    }

    $scope.GetWHDataReportPDF = function () {
        debugger;
        if ($("#txtTenant").val() == "0" || $("#txtTenant").val() == "") {
            Tenantid = 0;
            showStickyToast(false, "Please select Tenant ");
            return false;
        }
        if ($("#txtWarehouse").val() == "0" || $("#txtWarehouse").val() == "") {
            Warehouseid = 0;
            showStickyToast(false, "Please select Warehouse ");
            return false;
        }
        if ($("#txtDate").val() == "") {
            showStickyToast(false, "Please select Date ");
            return false;
        }
        if ($("#txtItem").val() == "") {
            mid = 0;
        }
        $scope.blockUI = true;
        var httpreq = {
            method: 'POST',
            url: 'WHStockInfoReport.aspx/genratePDF',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'TenantID': Tenantid, 'WHID': Warehouseid, 'date': $("#txtDate").val(), 'MID': mid },
            async: false
        }
        $http(httpreq).success(function (response) {
            debugger;
            if (response.d != "No Data Found") {
                var obj = response.d;
                showStickyToast(true, "PDF Generated Successfully ", false);
                $scope.blockUI = false;
                window.open('../mOutbound/PackingSlip/' + obj);
                return false;

            }
            else {
                showStickyToast(false, "No Data Found ", false);
                $scope.blockUI = false;
                return false;
            }
        })
    }
});
