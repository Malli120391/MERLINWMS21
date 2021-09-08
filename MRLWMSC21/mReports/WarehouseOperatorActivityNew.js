var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('WarehouseOperatorActivityNew', function ($scope, $http) {
    //alert("11");

    var Tenantid = 0;
    var Warehouseid = 0;
    var UserID = 0;
    var mtypeid = 0;
    $('#txtTenant').val("");
    debugger;
    var textfieldname = $("#txtTenant");
    DropdownFunction(textfieldname);
    $("#txtTenant").autocomplete({
        source: function (request, response) {
            $("#txtoperator").val("");
            UserID = 0;
            if ($('#txtTenant').val() == "") {
                Tenantid = 0;
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
           // $scope.LoadWareHouse();

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
               // $("#txtWarehouse").val("");
                //Warehouseid = 0;
                //if (Warehouseid == 0) {
                //    showStickyToast(false, "Select WareHouse");
                //}
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
                $('#txtTenant').val("");
             //   $("#txtWarehouse").val("");

            },
            minLength: 0
        });
        //ending of warehouse dropdown
    }
   // $('#txtoperator').val("");
    debugger;
    var textfieldname1 = $("#txtoperator");
    DropdownFunction(textfieldname1);
    $("#txtoperator").autocomplete({
        source: function (request, response) {
            debugger;
           // $("#txtoperator").val("");
           // Warehouseid = 0;
            if (Tenantid == 0) {
                showStickyToast(false, "Select Tenant");
            }
            $.ajax({
               
                url: '../mWebServices/FalconWebService.asmx/LoadUsersData1',
                data: "{ 'prefix': '" + request.term + "','TenantID':'" + Tenantid + "','WarehouseID':'" + Warehouseid+"'}",
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
            UserID = i.item.val;
            //   $("#txtWarehouse").val("");

        },
        minLength: 0
    });
    //ending of warehouse dropdown

    
    $scope.LoadWareHouse();

    $scope.Getgedetails = function () {
        debugger;
        //if ($("#txtTenant").val() == "" || $("#txtTenant").val() == null || $("#txtTenant").val() == undefined || $("#txtTenant").val() == "0") {

        //    showStickyToast(false, "Please select Tenant");
        //    return false;
        //}
        if ($("#txtWarehouse").val() == "" || $("#txtWarehouse").val() == null || $("#txtWarehouse").val() == undefined || $("#txtWarehouse").val() == "0") {

            showStickyToast(false, "Please select WareHouse");
            return false;
        }
        
        var fromdate = $("#txtFromdate").val();
        var textfieldname = $("#txttodate").val();
        var httpreq = {
            method: 'POST',
            url: 'WarehouseOperatorActivityNew.aspx/GetBillingReportList',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'FromDate': fromdate, 'ToDate': textfieldname, 'TenantId': Tenantid, 'WareHouse': Warehouseid, 'UserID': UserID },
            async: false
        }
        $http(httpreq).success(function (response) {
            // $scope.blockUI = false;
            //
            debugger;
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
        $("#tbldata").append("<table><thead><tr><td><img src='../Images/INV_TraceabilityAssured.png' style='height: 56px;' border='0'></td></tr><tr><th>Operator</th><th>Role Assigned</th><th>Warehouse</th><th>Total LineItems</th><th>Total Qty.</th><th>Goods In LineItems</th><th>Goods In Qty.</th><th>Goods Out LineItems</th><th>Goods Out Qty.</th></thead><tbody>");
        if (table != null && table.length > 0) {
        for (var i = 0; i < table.length; i++) {
            $("#tbldata").append("<tr></td><td class='aligndate'>" + table[i].Operator + "</td><td class='aligndate'>" + table[i].RoleAssigned + "</td><td class='aligndate'>" + table[i].Warehouse + "</td><td class='aligndate'>" + table[i].TotalLineItems + "</td><td class='aligndate'>" + table[i].TotalQuantity + "</td><td class='aligndate'>" + table[i].GoodsInLineItems + "</td><td class='aligndate'>" + table[i].GoodsInQuantity + "</td><td class='aligndate'>" + table[i].GoodsOutLineItems + "</td><td class='aligndate'>" + table[i].GoodsOutQuantity + "</td></tr>");
       }
        $("#tbldata").append("</tbody></table>");
    }
         else {

        $("#tbldata").append("<tr><td colspan='9' class='aligndate' style='background-color: white'>No Data found</td></tr>");
        $("#tbldata").append("</tbody>");
    }
}

});