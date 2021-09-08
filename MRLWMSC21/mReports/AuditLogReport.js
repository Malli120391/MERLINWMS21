var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('AuditLogReport', function ($scope, $http) {
   // alert("11");
   
    var Refobject = '';
    $scope.getTables = function () {

        //alert('11');
        //
        $('#txtrefdnumber').val("");

        var textfieldname = $("#txtTables");
        DropdownFunction(textfieldname);
        $("#txtTables").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/GetTablesbasedOnObjects',
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
                Refobject = i.item.val;
                // alert(Refnumber);
                //$scope.ngtenant = i.item.val;
            },
            minLength: 0
        });

    }
    
    $scope.Getgedetails = function () {
        
        var objectid = Refobject;
        //var fromdate = $("#txtFromdate").val();
        //var textfieldname = $("#txttodate").val();
        var httpreq = {
            method: 'POST',
            url: 'AuditLogReport.aspx/GetBillingReportList',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'ObjectID': objectid },
            async: false
        }
        $http(httpreq).success(function (response) {
            // $scope.blockUI = false;
            
            $scope.BIllingReport = response.d;
        })
    }
    //}
    //$http(httpreq).success(function (response) {
    //    // $scope.blockUI = false;
    //    //
    //    $scope.BIllingReport = response.d;
    //    if ($scope.BIllingReport == undefined || $scope.BIllingReport == null || $scope.BIllingReport.length == 0)
    //        showStickyToast(false, "No Data Found");
    //})


    $scope.exportPdf = function () {
        
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
        
        var table = $scope.BIllingReport;
        $("#tbldata").empty();
        $("#tbldata").append("<table><thead><tr><td><img src='../Images/INV_TraceabilityAssured.png' style='height: 56px;' border='0'></td></tr><tr><th> Transaction Id</th><th> Entity Id</th><th> Table Name</th><th>Column Name </th><th>Data Inserted</th><th>Data Updated</th><th>Data Deleted </th><th>Old Value</th><th>New Value</th><th>Modified By</th><th>Modified On</th></thead><tbody>");
        for (var i = 0; i < table.length; i++) {
            $("#tbldata").append("<tr></td><td class='aligndate'>" + table[i].TransactionId + "</td><td class='aligndate'>" + table[i].EntityId + "</td><td class='aligndate'>" + table[i].TableName + "</td><td class='aligndate'>" + table[i].ColumnName + "</td><td class='aligndate'>" + table[i].DataInserted + "</td><td class='aligndate'>" + table[i].DataUpdated + "</td><td class='aligndate'>" + table[i].DataDeletd + "</td><td class='aligndate'>" + table[i].OldValue + "</td><td class='aligndate'>" + table[i].NewValue + "</td><td class='aligndate'>" + table[i].ModifiedBy + "</td><td class='aligndate'>" + table[i].ModifiedOn + "</td></tr>");
        }
        $("#tbldata").append("</tbody></table>");
    }


});