var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('OutboundTransactionsHistoryNew', function ($scope, $http) {
    //alert("11");
    var Tenantid = 0;

    var Warehouseid = 0;
  

    var mmid = 0;
    $('#txtTenant').val("");
    var textfieldname = $("#txtTenant");
    DropdownFunction(textfieldname);
    $("#txtTenant").autocomplete({
        
        source: function (request, response) {

            debugger;
            if ($("#txtTenant").val() || $("#txtTenant").val == null || $("#txtTenant").val() == undefined) {
               // $scope.search.tenantid = 0;
                tenantid = 0;
            }
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
            $("#hdnTenant").val(i.item.val);
           // $scope.LoadWareHouse();
            $("#txtPartnumber").val('');
            $("#hdnPartNo").val("0");
        },
        minLength: 0
    });

    //$scope.LoadWareHouse = function () {
        $("#txtWarehouse").val("");
        var textfieldname = $("#txtWarehouse");
        //debugger;
        DropdownFunction(textfieldname);
        $("#txtWarehouse").autocomplete({
            source: function (request, response) {
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
                debugger;
                Warehouseid = i.item.val;
                //$("#txtWarehouse").val("");
                $("#hdnWarehouse").val(i.item.val);
                $("#txtTenant").val("");
                Tenantid = 0;
                $("#hdnTenant").val(0);
            },
            minLength: 0
        });
        //ending of warehouse dropdown
   // }   

    $('#txtPartnumber').val("");
    var textfieldname = $("#txtPartnumber");
    DropdownFunction(textfieldname);
    $("#txtPartnumber").autocomplete({
        source: function (request, response) {
            if (Tenantid == 0 || Tenantid == "0" || Tenantid == undefined || Tenantid == null) {
                showStickyToast(false, 'Please select Tenant');
                return false;
            }
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/LoadMaterialsForCurrentStock',
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
            mmid = i.item.val;
            $("#hdnPartNo").val(i.item.val);
        },
        minLength: 0
    });
    $scope.GetReport = function (PaginationId) {
        debugger;

        //if (Tenantid == "0" || Tenantid == null || Tenantid == undefined || $("#txtTenant").val().trim() == "" || $("#txtTenant").val() == null || $("#txtTenant").val() == undefined) {
        //    showStickyToast(false, "Please select Tenant");
        //    return false;
        //}

        if (Warehouseid == "0" || Warehouseid == null || Warehouseid == undefined || $("#txtWarehouse").val() == "0" || $("#txtWarehouse").val() == "" || $("#txtWarehouse").val() == null || $("#txtWarehouse").val() == undefined) {
            showStickyToast(false, "Please select Warehouse");
            return false;
        }

        var fromdate = $("#txtFromDate").val();
        var ToDate = $("#txtToDate").val();
        var httpreq = {
            method: 'POST',
            url: 'OutboundTransactionsHistoryNew.aspx/getOBDTransactionList',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: {
                'TenantId': Tenantid, 'MMId': mmid, 'FromDate': fromdate, 'ToDate': ToDate, 'Warehouseid': Warehouseid, 'PageIndex': PaginationId,'PageSize':25 },
            async: false
        }
        $http(httpreq).success(function (response) {
            // $scope.blockUI = false;
            //debugger;
            var dt = JSON.parse(response.d).Table;
            if (dt.length > 0) {
                var dt = JSON.parse(response.d).Table;
                $scope.OBDTramsactionData = dt;
                $scope.TotalRecords = dt[0].TotalRecords;
            }
            else
            {
                $scope.OBDTramsactionData = [];
                $scope.TotalRecords = 0; 
                showStickyToast(false, "No Data found");
            }
            
            //if ($scope.BIllingReport == undefined || $scope.BIllingReport == null || $scope.BIllingReport.length == 0)
                
        })
    }

   // $scope.GetReport(1);

    $scope.exportPdf = function () {
        //
        $scope.export();
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ type: 'pdf', jspdf: { orientation: 'l', margins: { right: 10, left: 10, top: 40, bottom: 40 }, autotable: { tableWidth: 'auto' } } });
        $("#tbldata").css('display', 'none');
        $scope.export();
    }
    $scope.exportExcel = function () {
        debugger;
        if ($scope.BIllingReport == undefined || $scope.BIllingReport == null || $scope.BIllingReport == "") {

            showStickyToast(false, "No data found to Download Excel");
            return false;
        }
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
        debugger;
        var table = $scope.BIllingReport;
        
        $("#tbldata").empty();
        //$("#tbldata").append("<table><thead><tr><td><img src='../Images/INV_TraceabilityAssured.png' style='height: 56px;' border='0'></td></tr><tr><th> TenantName</th><th>StoreRefNo </th><th>SupplierName</th><th>PONumber</th><th>Receipt</th><th>Services</th><th>UoM</th><th>UnitCost</th><th>After Disc</th><th>Qty</th><th>TotalCost</th><th>TotalCostAfterDisc</th><th>DiscWithOutTax</th><th>Tax</th></thead><tbody>");
        $("#tbldata").append("<table><thead><tr><td><img src='../Images/INV_TraceabilityAssured.png' style='height: 56px;' border='0'></td></tr><tr><th> MaterialCode</th><th>YearofTrans </th><th>Jan</th><th>Feb</th><th>Mar</th><th>Apr</th><th>May</th><th>June</th><th>July</th><th>Aug</th><th>Sep</th><th>Oct</th><th>Nov</th><th>Dec</th></thead><tbody>");
        for (var i = 0; i < table.length; i++) {
            //$("#tbldata").append("<tr></td><td class='aligndate'>" + table[i].TenantName + "</td><td class='aligndate'>" + table[i].StoreRefNo + "</td><td class='aligndate'>" + table[i].SupplierName + "</td><td class='aligndate'>" + table[i].PONumber + "</td><td class='aligndate'>" + table[i].Receipt + "</td><td class='aligndate'>" + table[i].Services + "</td><td class='aligndate'>" + table[i].UoM + "</td><td class='aligndate'>" + table[i].UnitCost + "</td><td class='aligndate'>" + table[i].UnitCostAfterDisc + "</td><td class='aligndate'>" + table[i].Quantity + "</td><td class='aligndate'>" + table[i].TotalCost + "</td><td class='aligndate'>" + table[i].TotalCostAfterDisc + "</td><td class='aligndate'>" + table[i].TotalCostAfterDiscWithOutTax + "</td><td class='aligndate'>" + table[i].Tax + "</td></tr>");
            $("#tbldata").append("<tr></td><td class='aligntext'>" + table[i].MaterialCode + "</td><td class='aligntext'>" + table[i].YearofTrans + "</td><td class='aligntext'>" + table[i].Jan + "</td><td class='aligntext'>" + table[i].Feb + "</td><td class='aligntext'>" + table[i].Mar + "</td><td class='aligntext'>" + table[i].Apr + "</td><td class='aligntext'>" + table[i].May + "</td><td class='aligntext'>" + table[i].June + "</td><td class='aligndate'>" + table[i].July + "</td><td class='aligntext'>" + table[i].Aug + "</td><td class='aligntext'>" + table[i].Sep + "</td><td class='aligntext'>" + table[i].Oct + "</td><td class='aligntext'>" + table[i].Nov + "</td><td class='aligntext'>" + table[i].Dec + "</td></tr>");
        }
        $("#tbldata").append("</tbody></table>");
    }


});