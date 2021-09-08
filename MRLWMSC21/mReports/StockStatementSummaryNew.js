var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('StockStatementSummaryNew', function ($scope, $http) {

    var Tenantid = 0;
    var mtypeid = 0;
    var Warehouseid = 0;
    $scope.StockSummary = [];
    $('#txtTenant').val("");
    debugger;
    //var textfieldname = $("#txtTenant");
    //DropdownFunction(textfieldname);
    //$("#txtTenant").autocomplete({
    //    source: function (request, response) {
    //        $.ajax({
    //            //url: '../mWebServices/FalconWebService.asmx/GetTenantList',
    //            //data: "{ 'prefix': '" + request.term + "'}",
    //            url: '../mWebServices/FalconWebService.asmx/GetWarehouseTenant',
    //            data: "{ 'prefix': '" + request.term + "','WHID':'" + Warehouseid + "'}",
    //            dataType: "json",
    //            type: "POST",
    //            contentType: "application/json; charset=utf-8",
    //            success: function (data) {
    //                response($.map(data.d, function (item) {
    //                    return {
    //                        label: item.split(',')[0],
    //                        val: item.split(',')[1]
    //                    }
    //                }))
    //            }
    //        });
    //    },
    //    select: function (e, i) {
    //        Tenantid = i.item.val;
    //      //  $scope.LoadWareHouse();
    //    },
    //    minLength: 0
    //});
    //$scope.LoadWareHouse = function () {
    //    $("#txtWarehouse").val("");

    //    var textfieldname = $("#txtWarehouse");
    //    debugger;
    //    DropdownFunction(textfieldname);
    //    $("#txtWarehouse").autocomplete({
    //        source: function (request, response) {
    //            $.ajax({
    //                //url: '../mWebServices/FalconWebService.asmx/LoadWHForWHList',
    //                //data: "{ 'prefix': '" + request.term + "','TenantID':'" + Tenantid + "'  }",
    //                url: '../mWebServices/FalconWebService.asmx/LoaDWHListBasedonUser',
    //                data: "{ 'prefix': '" + request.term + "'}",  
    //                dataType: "json",
    //                type: "POST",
    //                contentType: "application/json; charset=utf-8",
    //                success: function (data) {
    //                    response($.map(data.d, function (item) {
    //                        return {
    //                            label: item.split(',')[0],
    //                            val: item.split(',')[1]
    //                        }
    //                    }))
    //                }
    //            });
    //        },
    //        select: function (e, i) {
    //            Warehouseid = i.item.val;
    //            $("#txtWarehouse").val("");

    //        },
    //        minLength: 0
    //    });
    //    //ending of warehouse dropdown
    //}

    $('#txtTenant').val("");
    var textfieldname = $("#txtTenant");
    DropdownFunction(textfieldname);
    $("#txtTenant").autocomplete({
        source: function (request, response) {
            debugger;
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/GetWarehouseTenant',
                data: "{ 'prefix': '" + request.term + "','WHID':'" + Warehouseid + "'}",
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
        },
        minLength: 0
    });
    $("#txtWarehouse").val("");



    var textfieldname = $("#txtWarehouse");
    //debugger;

    DropdownFunction(textfieldname);
    $("#txtWarehouse").autocomplete({
        source: function (request, response) {
            debugger;
            $.ajax({
                //url: '../mWebServices/FalconWebService.asmx/LoadWHForWHList',
                //data: "{ 'prefix': '" + request.term + "','TenantID':'" + Tenantid + "'  }",
                url: '../mWebServices/FalconWebService.asmx/LoaDWHListBasedonUser',
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
            Warehouseid = i.item.val;
        },
        minLength: 0
    });


    var textfieldname = $("#txtSKU");
    DropdownFunction(textfieldname);
    $("#txtSKU").autocomplete({
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






    //alert('11');
    //$("#txtPartNo").val("");
    //var textfieldname = $("#txtPartNo");
    //DropdownFunction(textfieldname);
    //$("#txtPartNo").autocomplete({
    //    source: function (request, response) {
    //        $.ajax({
    //            url: '../mWebServices/FalconWebService.asmx/GetReplenishedMaterialCode',
    //            data: "{ 'prefix': '" + request.term + "','TenantID':'" + Tenantid + "'}",
    //            dataType: "json",
    //            type: "POST",
    //            contentType: "application/json; charset=utf-8",
    //            success: function (data) {
    //                response($.map(data.d, function (item) {
    //                    return {
    //                        label: item.split(',')[0],
    //                        val: item.split(',')[1]
    //                    }
    //                }))
    //            }
    //        });
    //    },
    //    select: function (e, i) {
    //        Refnumber = i.item.val;
    //        // alert(Refnumber);
    //    },
    //    minLength: 0
    //});



    var textfieldname = $("#txtMaterialType");
    DropdownFunction(textfieldname);
    $("#txtMaterialType").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/LoadMTypeDataForReports',
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
            mtypeid = i.item.val;
            // alert(Refnumber);
        },
        minLength: 0
    });
    //}
    $scope.Getgedetails = function (pageNo) {

        debugger;
        var Mcode = 0;
        var Mocdeid = $("#txtPartNo").val();
        var fromdate = $("#txtOpeningdate").val();
        var textfieldname = $("#txtClosingdate").val();
        //var mtypeid = $scope.ddlMaterialType;
        var mtype = $("#txtMaterialType").val();
        //var Mcode = $("#txtPartNo").val();
        if (Mocdeid != "") {
            Mcode = Refnumber;
        }
        else {
            Mcode = "0";
        }
        if (mtype != "") {
            mtypeid = mtypeid;
        }
        else {
            mtypeid = 0;
        }





        mtypeid = mtypeid == undefined ? "0" : mtypeid;
        Mcode = Mcode == "" ? "0" : Mcode;
        Mcode = Mcode == undefined ? "0" : Mcode;
        debugger
        $scope.blockUI = true;
        var httpreq = {

            method: 'POST',
            url: 'StockStatementSummaryNew.aspx/GetBillingReportList',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: {
                'FromDate': fromdate, 'ToDate': textfieldname, 'MTypeID': mtypeid, 'mcode': Mcode, 'tenantid': Tenantid, 'accountid': 2, 'Warehouseid': Warehouseid, 'PageNo': pageNo, 'PageSize': 25, 'IsExport': 0
            },
            async: false
        }
        $http(httpreq).success(function (response) {
            // $scope.blockUI = false;
            //
            //$scope.blockUI = false;
            //$scope.BIllingReport = response.d;
            //if ($scope.BIllingReport == undefined || $scope.BIllingReport == null || $scope.BIllingReport.length == 0)
            //    showStickyToast(false, "No Data found");

            var dt = response.d;
            if (dt == undefined || dt == null || dt.length == 0) {
                showStickyToast(false, "No Data Found", false);
                $scope.StockSummary = null;
                $scope.Totalrecords = 0;
                $scope.blockUI = false;
                return false;
            }
            $scope.StockSummary = dt;
            $scope.Totalrecords = $scope.StockSummary[0].TotalRecords;
            //console.log($scope.StockSummary)
            $scope.blockUI = false;
        //console.log($scope.StockSummary)
    });
    }

    var Refnumber = 0;
    $('#txtrefdnumber').val("");

    var textfieldname = $("#txtPartNo");
    DropdownFunction(textfieldname);
    $("#txtPartNo").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/GetReplenishedMaterialCode',
                data: "{ 'prefix': '" + request.term + "'}",
                //data: "{ 'prefix': '" + request.term + "','TenentID' : '" + $("#hdnTenantID").val() + "'}",
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
    $scope.exportPdf = function () {
        //
        $scope.export();
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ type: 'pdf', jspdf: { orientation: 'l', margins: { right: 10, left: 10, top: 40, bottom: 40 }, autotable: { tableWidth: 'auto' } } });
        $("#tbldata").css('display', 'none');
        $scope.export();
    }
    $scope.exportExcel = function () {
        debugger
        if ($scope.StockSummary.length == 0 || $scope.StockSummary == null) {
            showStickyToast(false, "No Data Found", false);
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
        //

        var table = $scope.StockSummary;
        $("#tbldata").empty();
        $("#tbldata").append("<table><thead><tr><td><img src='../Images/INV_TraceabilityAssured.png' style='height: 56px;' border='0'></td></tr><tr><th>PartNo</th><th>Description</th><th>UOM</th><th>OpeningStock</th><th>Inbound</th><th>Outbound</th><th>ClosingStock</th><th>StockDifference</th></thead><tbody>");
        if (table != null && table.length > 0) {
        for (var i = 0; i < table.length; i++) {
            $("#tbldata").append("<tr></td><td class='aligntext'>" + table[i].PartNo + "</td><td class='aligntext'>" + table[i].Description + "</td><td class='aligntext'>" + table[i].UOM + "</td><td class='aligntext'>" + table[i].OpeningStock + "</td><td class='aligntext'>" + table[i].Inbound + "</td><td class='aligntext'>" + table[i].Outbound + "</td><td class='aligntext'>" + table[i].ClosingStock + "</td><td class='aligntext'>" + table[i].StockDifference + "</td></tr>");
        }
        $("#tbldata").append("</tbody></table>");
    }
      else {

        $("#tbldata").append("<tr><td colspan='8' class='aligndate' style='background-color: white'>No Data Found</td></tr>");
        $("#tbldata").append("</tbody>");
    }
}


});