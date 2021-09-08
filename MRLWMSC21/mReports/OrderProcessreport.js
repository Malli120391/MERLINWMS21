
var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('PickPerformance', function ($scope, $http) {
    var Tenantid = 0;
    var Warehouseid = 0;
    var mtypeid = 0;
    $('#txtTenant').val("");
    debugger;
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
          //  $scope.LoadWareHouse();

        },
        minLength: 0
    });


   // $scope.LoadWareHouse = function () {
        $("#txtWarehouse").val("");

        var textfieldname = $("#txtWarehouse");
        debugger;
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
                Warehouseid = i.item.val;
                //$("#txtWarehouse").val("");
                $('#txtTenant').val("");

            },
            minLength: 0
        });
        //ending of warehouse dropdown
    //}
        $scope.Getgedetails = function () {
        
        var fromdate = $("#txtFromdate").val();
        var textfieldname = $("#txtTodate").val();

            if ($("#txtWarehouse").val() == "" || $("#txtWarehouse").val() == undefined) {
                showStickyToast(false, "Please select WareHouse");
                return;
            }
            if ($("#txtTenant").val() == "" || $("#txtTenant").val() == undefined) {
                showStickyToast(false, "Please select Tenant");
                return;
            }
        var httpreq = {
            method: 'POST',
            url: 'OrderProcessreport.aspx/GetBillingReportList',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'FromDate': fromdate, 'ToDate': textfieldname, 'TenantId': Tenantid, 'WareHouse': Warehouseid},
            async: false
        }
        $http(httpreq).success(function (response) {
            // $scope.blockUI = false;
            debugger;
            $scope.list = response.d;
            if ($scope.list == undefined || $scope.list == null || $scope.list.length == 0)
                showStickyToast(false, "No Data found");
        })
    }



    $scope.exportPdf = function (TableName) {
        
        $scope.export();
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ type: 'pdf', jspdf: { orientation: 'l', margins: { right: 10, left: 10, top: 40, bottom: 40 }, autotable: { tableWidth: 'auto' } } });
        $("#tbldata").css('display', 'none');
        $scope.export();
    }

    $scope.exportExcel = function (TableName) {
        //
        $scope.export();
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ type: 'excel' });
        $("#tbldata").css('display', 'none');
    }
    $scope.exportCsv = function (TableName) {
        //
        $scope.export();
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ type: 'csv', numbers: { html: { decimalMark: '.', thousandsSeparator: ',' }, output: { decimalMark: ',', thousandsSeparator: '' } } });
        $("#tbldata").css('display', 'none');
    }
    $scope.exportXml = function (TableName) {
        //
        $scope.export();
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ type: 'xml' });
        $("#tbldata").css('display', 'none');
    }
    $scope.exportTxt = function (TableName) {
        //
        $scope.export();
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ type: 'txt' });
        $("#tbldata").css('display', 'none');
    }
    $scope.exportWord = function (TableName) {
        //
        $scope.export();
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ type: 'doc' });
        $("#tbldata").css('display', 'none');
    }

    $scope.export = function (TableData) {
        
        var table = $scope.list;
        $("#tbldata").empty();
        $("#tbldata").append("<table><thead><tr><th align='center'>S.No</th><th align='center'>Total Inward</th><th>Work Items Completed</th><th>Completed Percentage</th><th>Receipts To Do</th><th>To Do Perecentage</th></thead><tbody>");
 if (table != null && table.length > 0) {       
   for (var i = 0; i < table.length; i++) {
            $("#tbldata").append("<tr><td class='aligntext'>" + (i + 1) + "</td><td class='aligntext'>" + table[i].TotalInward + "</td><td class='aligntext'>" + table[i].WorkItemsCompleted + "</td><td class='aligntext'>" + table[i].CompletedPercent + "</td><td class='aligntext'>" + table[i].ReceiptsToDo + "</td><td class='aligntext'>" + table[i].ToDoPercent + "</td></tr>");
        }
        $("#tbldata").append("</tbody></table>");
    }
else {

        $("#tbldata").append("<tr><td colspan='6' class='aligndate' style='background-color: white'>No Data found</td></tr>");
        $("#tbldata").append("</tbody>");
    }
}

});


