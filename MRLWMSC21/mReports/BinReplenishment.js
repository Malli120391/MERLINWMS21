var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('BinReplenishment', function ($scope, $http) {

    var itemnumber = '';
    var sFileName = 'BinReplenishmentReport'
    var Tenantid = 0;
    var mtypeid = 0;
    var Warehouseid = 0;
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
            //$scope.LoadWareHouse();
           
          

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
                debugger;
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
                $("#txtTenant").val("");

            },
            minLength: 0
        });
        //ending of warehouse dropdown
   // }   
    //$scope.getskus = function () {
        


        var textfieldname = $("#txtItemNo");
        DropdownFunction(textfieldname);
        $("#txtItemNo").autocomplete({
            source: function (request, response) {
                debugger;
                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/LoadItemNumbersForBinReplenishmentReportNew',
                    //data: "{ 'prefix': '" + request.term + "'}",
                    data: "{ 'prefix': '" + request.term + "','TenantId':'" + Tenantid + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        debugger;
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
                itemnumber = i.item.val;
               // alert(itemnumber);
            },
            minLength: 0
        });
    //}

    

    $scope.Getbindetails = function () {
        //if ($("#txtTenant").val() == null || $("#txtTenant").val() == undefined || $("#txtTenant").val() == "") {
        //    showStickyToast(false,"Select Tenant");
        //    return;
        //}
        var itemno = itemnumber;
        itemno = itemno == "" ? 0 : itemno;
        var httpreq = {
            method: 'POST',
            url: 'BinReplenishmentReport.aspx/GetBinReportList',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: {
                'ItemNumber': itemno, 'TenantId': Tenantid, 'WareHouseId': Warehouseid
            },
            async: false
        }
        $http(httpreq).success(function (response) {

            $scope.BinDetailsReport = response.d;
            // scope.BinDetailsReport.datainfo = response.d;
            if ($scope.BinDetailsReport == undefined || $scope.BinDetailsReport == null || $scope.BinDetailsReport.length == 0)
                showStickyToast(false, "No Data found");
        });
    }

    $scope.exportPdf = function () {
        //
        $scope.export();
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ fileName: sFileName, type: 'pdf', jspdf: { orientation: 'l', margins: { right: 10, left: 10, top: 40, bottom: 40 }, autotable: { tableWidth: 'auto' } } });
        $("#tbldata").css('display', 'none');
        $scope.export();
    }
    $scope.exportExcel = function () {
        //
        $scope.export();
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ fileName: sFileName, type: 'excel' });
        $("#tbldata").css('display', 'none');
    }
    $scope.exportCsv = function () {
        //
        $scope.export();
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ fileName: sFileName, type: 'csv', numbers: { html: { decimalMark: '.', thousandsSeparator: ',' }, output: { decimalMark: ',', thousandsSeparator: '' } } });
        $("#tbldata").css('display', 'none');
    }

    $scope.exportXml = function () {
        //
        $scope.export();
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ fileName: sFileName, type: 'xml' });
        $("#tbldata").css('display', 'none');
    }

    $scope.exportTxt = function () {
        //
        $scope.export();
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ fileName: sFileName, type: 'txt' });
        $("#tbldata").css('display', 'none');
    }

    $scope.exportWord = function () {
        //
        $scope.export();
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ fileName: sFileName, type: 'doc' });
        $("#tbldata").css('display', 'none');
    }

    $scope.export = function () {
        
        var table = $scope.BinDetailsReport;
        
        $("#tbldata").empty();
        $("#tbldata").append("<table><thead><tr><td><img src='../Images/inventrax.png' style='height: 56px;' border='0'></td></tr><tr><th>Item</th><th>Suggested Location</th><th>Min. Qty</th><th>Max. Qty</th><th>Available Qty</th></thead><tbody>");
        for (var i = 0; i < table.length; i++) {
           // $("#tbldata").append("<tr></td><td>" + table[i].ItemNo + "</td>");
            var childdata = table[i].oBinDetailsListlst;
            for (var j = 0; j < childdata.length; j++)
            {
                $("#tbldata").append("<tr style='text-align:center;'></td><td>" + table[i].ItemNo + "</td><td> " + childdata[j].SuggLoc + "</td> <td>" + childdata[j].MinQty + "</td> <td>" + childdata[j].MaxQty + "</td> <td>" + childdata[j].BinRepQty + "</td></tr > ");
            }
        }
        $("#tbldata").append("</tbody></table>");
    }

    //----------For Dialog open-----------------//

    $scope.openDialog = function (MID) {
        //
        //alert(MID);
        $('#divContainer').show();
        //alert(btnId);
        var httpreq = {
            method: 'POST',
            url: 'BinReplenishmentReport.aspx/GetBinPopUpList',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'MID': MID },
            async: false
        }
        $http(httpreq).success(function (response) {
            
            $scope.BinPopUpDetails = response.d;
            // scope.BinDetailsReport.datainfo = response.d;
            //if ($scope.BinPopUpDetails == undefined || $scope.BinPopUpDetails == null || $scope.BinPopUpDetails.length == 0)
            //    showStickyToast(false, "No Data found");
        })
    };   

    //----------For Popup Close-----------------//

    $('#spanClose').click(function (event) {
        $('#divContainer').hide();
    });

    $('#btncloseDialog').click(function (event) {
        $('#divContainer').hide();
    });

    $scope.closeData = function () {
        $('#divContainer').hide();
    }  

});