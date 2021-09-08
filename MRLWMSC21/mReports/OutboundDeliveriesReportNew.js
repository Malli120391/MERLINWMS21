var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('OutboundDeliveriesReportNew', function ($scope, $http) {
   // alert('Hi');
    var Warehouseid = 0;
    var TenantId = 0;
    var textfieldname = $("#txtTenant");
    DropdownFunction(textfieldname);
    $("#txtTenant").autocomplete({
        source: function (request, response) {
            if ($('#txtTenant').val() == '' || $('#txtTenant').val() == undefined) {
                $scope.TenantId = "0";
            }
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
            $scope.TenantId = "0";
            $scope.TenantId = i.item.val;
            TenantId = i.item.val;
            $scope.MMID = "0";
            $('#txtPartNo').val(i.item.UOM);
            $scope.LoadWareHouse();
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
                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/LoadWHForWHList',
                    data: "{ 'prefix': '" + request.term + "','TenantID':'" + TenantId + "'  }",
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

            },
            minLength: 0
        });
        //ending of warehouse dropdown
    }   
    var httpreq = {
        method: 'POST',
        url: '../mReports/OutboundDeliveriesReportNew.aspx/GetDocuments',
        headers: {
            'Content-Type': 'application/json; charset=utf-8',
            'dataType': 'json'
        },
        data: {}
    }
    $http(httpreq).success(function (response) {
        $scope.documents = response.d;
    });

    var httpreq = {
        method: 'POST',
        url: '../mReports/OutboundDeliveriesReportNew.aspx/GetDelivery',
        headers: {
            'Content-Type': 'application/json; charset=utf-8',
            'dataType': 'json'
        },
        data: {}
    }
    $http(httpreq).success(function (response) {
        $scope.deliveries = response.d;
    });

    var Refnumber = '';
    $scope.getskus = function () {

        //alert('11');
        $('#txtrefdnumber').val("");

        var textfieldname = $("#txtPartnumber");
        DropdownFunction(textfieldname);
        $("#txtPartnumber").autocomplete({
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
                                label: item.split(',')[1],
                                val: item.split(',')[0]
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

    }

    $scope.Getgedetails = function () {
        
        var documnettype = $scope.ddlDocumentType;
        var deliverystatus = $scope.ddlDeliveryStatus;
        var fromdate = $("#txtFromdate").val();
        var textfieldname = $("#txttodate").val();

        documnettype = documnettype == undefined ? "0" : documnettype;
        deliverystatus = deliverystatus == undefined ? "0" : deliverystatus;
        var httpreq = {
            method: 'POST',
            url: 'OutboundDeliveriesReportNew.aspx/GetBillingReportList',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'DocumenttypeID': documnettype, 'DeliverystatusID': deliverystatus, 'FromDate': fromdate, 'ToDate': textfieldname, 'TenantId': TenantId, 'Warehouseid': Warehouseid },
            async: false
        }
        $http(httpreq).success(function (response) {
            // $scope.blockUI = false;
            //
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
        $("#tbldata").append("<table><thead><tr><td><img src='../Images/INV_TraceabilityAssured.png' style='height: 56px;' border='0'></td></tr><tr><th> DeliveryDoc. No.</th><th>Document Type </th><th>Delivery Doc. Date</th><th>Customer</th><th>Stores</th><th> Date /Done By</th><th>Delivery Date</th><th>Status</th><th>Line Items</th></thead><tbody>");
        if (table != null && table.length > 0) {
        for (var i = 0; i < table.length; i++) {
            $("#tbldata").append("<tr></td><td class='aligntext'>" + table[i].DeliveryDocNo + "</td><td class='aligntext'>" + table[i].DocumentType + "</td><td class='aligntext'>" + table[i].DeliveryDocDate + "</td><td class='aligntext'>" + table[i].Customer + "</td><td class='aligntext'>" + table[i].Stores + "</td><td class='aligntext'>" + table[i].PGI + "</td><td class='aligntext'>" + table[i].DeliveryDate + "</td><td class='aligntext'>" + table[i].Status + "</td><td class='aligntext'>" + table[i].LineItems + "</td></tr>");
        }
        $("#tbldata").append("</tbody></table>");
    }
        else {

        $("#tbldata").append("<tr><td colspan='8' class='aligndate' style='background-color: white'>No Data Found</td></tr>");
        $("#tbldata").append("</tbody>");
    }
    }

    $scope.sort = function (keyname) {
        $scope.sortKey = keyname;
        $scope.reverse = !$scope.reverse;
    } 
    
});