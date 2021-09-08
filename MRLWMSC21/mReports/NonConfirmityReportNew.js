var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('NonConfirmityReportNew', function ($scope, $http) {
    //alert('Hi');
    var Refnumber = '';
    $scope.getskus = function () {

        //alert('11');
        $('#txtrefdnumber').val("");

        var textfieldname = $("#txtPONumber");
        DropdownFunction(textfieldname);
        $("#txtPONumber").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/LoadPONumbersForInwardQCReport',
                    data: "{ 'prefix': '" + request.term + "','TenentID' : '" + $("#hdnTenantID").val() + "','SupplierID':'0'}",
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

    }

    $scope.Getgedetails = function () {
        //alert('Hi');
        
       
        //var textfieldname = $("#txtPONumber").val();
        var textfieldname = Refnumber;
        

        var httpreq = {
            method: 'POST',
            url: 'NonConfirmityReportNew.aspx/GetBillingReportList',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'POHeaderID': textfieldname },
            async: false
            
        }
        $http(httpreq).success(function (response) {
            // $scope.blockUI = false;
            //
            $scope.BIllingReport = response.d;
            if ($scope.BIllingReport == undefined || $scope.BIllingReport == null || $scope.BIllingReport.length == 0)
                showStickyToast(false, "No Data Found");
        })
    }

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
        $("#tbldata").append("<table><thead><tr><td><img src='../Images/INV_TraceabilityAssured.png' style='height: 56px;' border='0'></td></tr><tr><th> LineNo</th><th>PartNumber </th><th>Quantity</th><th>Status</th><th>QCParameters</th></thead><tbody>");
        for (var i = 0; i < table.length; i++) {
            $("#tbldata").append("<tr></td><td class='aligndate'>" + table[i].LineNo + "</td><td class='aligndate'>" + table[i].PartNumber + "</td><td class='aligndate'>" + table[i].Quantity + "</td><td class='aligndate'>" + table[i].Status + "</td><td class='aligndate'>" + table[i].QCParameters + "</td></tr>");
        }
        $("#tbldata").append("</tbody></table>");
    }
});