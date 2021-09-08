
var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('MaterialTrackingReportNew', function ($scope, $http) {
    var Warehouseid = 0;
    var TenantId = 0;
  
    var MID = 0;


    var textfieldname = $("#txtWarehouse");
    debugger;
    DropdownFunction(textfieldname);
    $("#txtWarehouse").autocomplete({
        source: function (request, response) {
            $.ajax({
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
        },
        minLength: 0
    });


    var textfieldname = $("#txtTenant");
    DropdownFunction(textfieldname);
    
    $("#txtTenant").autocomplete({
        source: function (request, response) {
            if ($('#txtTenant').val() == '' || $('#txtTenant').val() == undefined) {
                $scope.TenantId = "0";
            }
           
            $.ajax({
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

            TenantId = i.item.val;
            $("#hdnTenant").val(i.item.val);
            //$scope.MMID = "0";
            ///$('#txtPartNo').val(i.item.UOM);
            //$scope.LoadWareHouse();
        },
        minLength: 0
    });


   // $("#txtMaterial").val("");
    var textfieldname = $("#txtMaterial");
    debugger;
    DropdownFunction(textfieldname);
    $("#txtMaterial").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/LoadMaterialsData',
                data: "{ 'prefix': '" + request.term + "','TenantID':'" + TenantId + "'}",
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
            MID = i.item.val;
            $("#hdnMaterial").val(i.item.val);
        },
        minLength: 0
    });

   // $scope.LoadWareHouse = function () {
        //$("#txtWarehouse").val("");

      
        //ending of warehouse dropdown
    //}   
    //alert("11");
    $scope.inwardhide = false;
    $scope.outwardhide = false;
    $scope.Getgedetails = function () {
        
        //var serialno = $("#txtserailno").val();
        //var batchno = $("#txtbatchno").val();

        //if (serialno == "" && batchno == "") {
        //    showStickyToast(false, "Please Enter Serial No./Batch No.");
        //    return;
        //}
        if ($("#txtTenant").val() == "") { TenantId = 0; } else { TenantId = TenantId; }
        if ($("#txtWarehouse").val() == "") { Warehouseid = 0; } else { Warehouseid = Warehouseid; }
        if ($("#txtMaterial").val() == "" || $("#hdnMaterial").val() == "0")
        {
            MID = 0; 
            showStickyToast(false, "Please select Material");
            return;
        }
        else { MID = MID;}

        var httpreq = {
            method: 'POST',
            url: 'MaterialTrackingReportNew.aspx/GetBillingReportList',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: {
                //'Serialno': serialno, 'Batchno': batchno, 'TenantId': TenantId, 'WareHouseId': Warehouseid, 'MaterialID': MID
                'TenantId': TenantId, 'WareHouseId': Warehouseid, 'MaterialID': MID
            },
            async: false
        }
        $http(httpreq).success(function (response) {
            debugger;
            if (response.d != null) {
                $scope.BIllingReportIn = response.d.one;
                $scope.BIllingReportOut = response.d.two;
            }
        })
    }

    $scope.exportPdf = function () {
        
        $scope.export();
        $("#tbldataIn").css('display', 'block');
        $('#tbldataIn').tableExport({ type: 'pdf', jspdf: { orientation: 'l', margins: { right: 10, left: 10, top: 40, bottom: 40 }, autotable: { tableWidth: 'auto' } } });
        $("#tbldataIn").css('display', 'none');
        $scope.export();

        $scope.export();
        $("#tbldataOut").css('display', 'block');
        $('#tbldataOut').tableExport({ type: 'pdf', jspdf: { orientation: 'l', margins: { right: 10, left: 10, top: 40, bottom: 40 }, autotable: { tableWidth: 'auto' } } });
        $("#tbldataOut").css('display', 'none');
        $scope.export();
    }
    $scope.exportExcel = function () {
        //
        $scope.export();
        $("#tbldataIn").css('display', 'block');
        $('#tbldataIn').tableExport({ type: 'excel' });
        $("#tbldataIn").css('display', 'none');

        $scope.export();
        $("#tbldataOut").css('display', 'block');
        $('#tbldataOut').tableExport({ type: 'excel' });
        $("#tbldataOut").css('display', 'none');
    }
    $scope.exportCsv = function () {
        //
        $scope.export();
        $("#tbldataIn").css('display', 'block');
        $('#tbldataIn').tableExport({ type: 'csv', numbers: { html: { decimalMark: '.', thousandsSeparator: ',' }, output: { decimalMark: ',', thousandsSeparator: '' } } });
        $("#tbldataIn").css('display', 'none');

        $scope.export();
        $("#tbldataOut").css('display', 'block');
        $('#tbldataOut').tableExport({ type: 'csv', numbers: { html: { decimalMark: '.', thousandsSeparator: ',' }, output: { decimalMark: ',', thousandsSeparator: '' } } });
        $("#tbldataOut").css('display', 'none');
    }

    $scope.exportXml = function () {
        //
        $scope.export();
        $("#tbldataIn").css('display', 'block');
        $('#tbldataIn').tableExport({ type: 'xml' });
        $("#tbldataIn").css('display', 'none');

        $scope.export();
        $("#tbldataOut").css('display', 'block');
        $('#tbldataOut').tableExport({ type: 'xml' });
        $("#tbldataOut").css('display', 'none');
    }

    $scope.exportTxt = function () {
        //
        $scope.export();
        $("#tbldataIn").css('display', 'block');
        $('#tbldataIn').tableExport({ type: 'txt' });
        $("#tbldataIn").css('display', 'none');

        $scope.export();
        $("#tbldataOut").css('display', 'block');
        $('#tbldataOut').tableExport({ type: 'txt' });
        $("#tbldataOut").css('display', 'none');
    }

    $scope.exportWord = function () {
        //
        $scope.export();
        $("#tbldataIn").css('display', 'block');
        $('#tbldataIn').tableExport({ type: 'doc' });
        $("#tbldataIn").css('display', 'none');

        $scope.export();
        $("#tbldataOut").css('display', 'block');
        $('#tbldataOut').tableExport({ type: 'doc' });
        $("#tbldataOut").css('display', 'none');
    }

    $scope.export = function () {
        
        var table = $scope.BIllingReportIn;
        $("#tbldataIn").empty();
        $("#tbldataIn").append("<table><thead><tr><td><img src='../Images/INV_TraceabilityAssured.png' style='height: 56px;' border='0'></td></tr><tr><th> PONumber</th><th>InvoiceNumber </th><th>Tenant</th><th>Supplier</th><th>StoreRefNo</th><th>PartNo</th><th>ReceivedQty</th></thead><tbody>");
        for (var i = 0; i < table.length; i++) {
            $("#tbldataIn").append("<tr></td><td class='aligndate'>" + table[i].PONumber + "</td><td class='aligndate'>" + table[i].InvoiceNumber + "</td><td class='aligndate'>" + table[i].Tenant + "</td><td class='aligndate'>" + table[i].Supplier + "</td><td class='aligndate'>" + table[i].StoreRefNo + "</td><td class='aligndate'>" + table[i].PartNo + "</td><td class='aligndate'>" + table[i].ReceivedQty + "</td></tr>");
        }
        $("#tbldataIn").append("</tbody></table>");


        table = $scope.BIllingReportOut;
        $("#tbldataOut").empty();
        $("#tbldataOut").append("<table><thead><tr><td><img src='../Images/INV_TraceabilityAssured.png' style='height: 56px;' border='0'></td></tr><tr><th> SONumber</th><th>SODate </th><th>CustomerPONo</th><th>Customer</th><th>PartNo</th><th>PickedQty</th></thead><tbody>");
        for (var i = 0; i < table.length; i++) {
            $("#tbldataOut").append("<tr></td><td class='aligndate'>" + table[i].SONumber + "</td><td class='aligndate'>" + table[i].SODate + "</td><td class='aligndate'>" + table[i].CustomerPONo + "</td><td class='aligndate'>" + table[i].Customer + "</td><td class='aligndate'>" + table[i].PartNo + "</td><td class='aligndate'>" + table[i].PickedQty + "</td></tr>");
        }
        $("#tbldataOut").append("</tbody></table>");
    }


});