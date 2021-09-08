var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('InwardInspectonReportNew', function ($scope, $http) {
   // alert('Hi');
   // 
    //$scope.getskus();
    
    $scope.today = new Date();
    var StoreRefNo = '';
    $scope.getStoreRefNo = function () {
       // alert('11');
        //
        
        var textfieldname = $("#txtstoreRefNo");
        DropdownFunction(textfieldname);
        
        $("#txtstoreRefNo").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/LoadStoreRefNumbers',
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
                StoreRefNo = i.item.val;
                //alert(StoreRefNo);
            },
            minLength: 0
        });

    }

    $scope.Getgedetails = function () {
        

        $scope.BIllingReport = null;
        //var mcode = $("#txtstoreRefNo").val();
        var mcode = StoreRefNo;
        var httpreq = {
            method: 'POST',
            url: 'InwardInspectonReportNew.aspx/GetBillingReportList',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'storerefno': mcode },
            async: false
        }
        $http(httpreq).success(function (response) {
            // $scope.blockUI = false;
            
            $scope.BIllingReport = response.d;
            if ($scope.BIllingReport == undefined || $scope.BIllingReport == null || $scope.BIllingReport.length == 0)
                showStickyToast(false,"No Data Found");
        })

        var storerefNo = $("#txtstoreRefNo").val();
        $("#lblStorerefenceNo").text("");       
        $("#lblStorerefenceNo").text(storerefNo);
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
        $("#tbldata").append("<table><thead><tr><td><img src='../Images/Logo_Header_falcon2.png' style='height: 56px;' border='0'></td></tr><tr><th> PartNo</th><th>Description </th><th>ReceivedQty</th><th>AcceptedQty</th><th>RejectedQty</th><th>SampleSize</th><th>Parameter</th><th>Test</th><th>UoM</th><th>Specification</th><th>Observation</th><th>Remarks</th></thead><tbody>");
        for (var i = 0; i < table.length; i++) {
            $("#tbldata").append("<tr></td><td class='aligndate'>" + table[i].PartNo + "</td><td class='aligndate'>" + table[i].Description + "</td><td class='aligndate'>" + table[i].ReceivedQty + "</td><td class='aligndate'>" + table[i].AcceptedQty + "</td><td class='aligndate'>" + table[i].RejectedQty + "</td><td class='aligndate'>" + table[i].SampleSize + "</td><td class='aligndate'>" + table[i].Parameter + "</td><td class='aligndate'>" + table[i].Test + "</td><td class='aligndate'>" + table[i].UoM + "</td><td class='aligndate'>" + table[i].Specification + "</td><td class='aligndate'>" + table[i].Observation + "</td><td class='aligndate'>" + table[i].Remarks + "</td></tr>");
        }
        $("#tbldata").append("</tbody></table>");
    }


});