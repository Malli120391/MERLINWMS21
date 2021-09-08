var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('MaterialAgeingReportNew', function ($scope, $http) {
    //alert('Hi');
    var Tenantid = 0;
    var zoneid = 0;
    //$scope.loadwarehouse = function () {
    //    debugger;
    //    var httpreq = {
    //        method: 'POST',
    //        url: '../mReports/MaterialAgeingReportNew.aspx/GetWarehouse1',
    //        headers: {
    //            'Content-Type': 'application/json; charset=utf-8',
    //            'dataType': 'json'
    //        },
    //        data: { 'TenantID': Tenantid}
    //    }
    //    $http(httpreq).success(function (response) {
    //        debugger;
    //        $scope.WareHouses = response.d;
          
    //    });
    //}

  //  $scope.loadwarehouse();

    $scope.loadZones = function () {

        debugger;
        //WareHouseName = $("#Warehouse option:selected").text();

        //$("#txtWarehouse").val("");

        var textfieldname = $("#ddlZones");
        debugger;
        DropdownFunction(textfieldname);
        $("#ddlZones").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/GetLocationZonesByWHID',
                    data: "{ 'Prefix': '" + request.term + "','WarehouseID':'" + Warehouseid + "'}",                   
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
                zoneid = i.item.val;
                $("#ddlZones").val("");
                $('#txtTenant').val("");

            },
            minLength: 0
        });

    }

        //if ($scope.ddlwarehouse != undefined) {
        //    var WHId = $scope.ddlwarehouse;
        //}
        //WareHouse = WareHouseName == "Select Warehouse " ? 0 : WHId;          
        //var httpreq = {
        //    method: 'POST',
        //    url: '../mReports/MaterialAgeingReportNew.aspx/GetZones',
        //    headers: {
        //        'Content-Type': 'application/json; charset=utf-8',
        //        'dataType': 'json'
        //    },
        //    data: { WareHouseId: WareHouse }
        //}
        //$http(httpreq).success(function (response) {
        //    debugger;
        //    $scope.zones = response.d;
        //});

   // }
   // $scope.loadZones();


    $scope.changeSelect = function () {
        debugger
        $scope.loadZones();
        
    }
   

    var Refnumber = '';
    //var Tenantid = 0;
    $('#txtTenant').val("");
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
            debugger;
            Tenantid = i.item.val;
           // $scope.ddlZones = '';
          //  $scope.ddlwarehouse = "";
           // $("#txtWarehouse").val("");
          //  $("#ddlZones").val("");
            $("#txtPartnumber").val("");
            
        },
        minLength: 0
    });
   // $scope.getskus = function () {

        //alert('11');
        $('#txtrefdnumber').val("");
        var textfieldname1 = $("#txtPartnumber");
        DropdownFunction(textfieldname1);
        $("#txtPartnumber").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/GetReplenishedMaterialCode',
                    data: "{ 'prefix': '" + request.term + "','TenantID':'" + Tenantid + "'}",
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
                // alert(Refnumber);
            },
            minLength: 0
        });

    //}

  //  $scope.LoadWareHouse = function () {
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
                $scope.loadZones();
            },
            minLength: 0
        });
        //ending of warehouse dropdown
  //  }   

    $scope.Getgedetails = function (pageid) {

        debugger;

        //if ($("#txtTenant").val() == "0" || $("#txtTenant").val() == null || $("#txtTenant").val() == undefined || $("#txtTenant").val() == "") {

        //    showStickyToast(false, "Please select Tenant");
        //    return false;
        //}

        if ($("#txtWarehouse").val() == '' || $("#txtWarehouse").val() == null || $("#txtWarehouse").val() == undefined || $("#txtWarehouse").val() == "0") {

            showStickyToast(false, "Please select Warehouse");
            return false;
        }

        if ($("#ddlZones").val() == '' || $("#ddlZones").val() == null || $("#ddlZones").val() == undefined || $("#ddlZones").val() == 0) {

            //showStickyToast(false, "Please select Zone");
            //return false;
            textfieldname = "";
        }

        var ageindays = $scope.ddlAge;
        vtextfieldname = "";
        textfieldname = $("#ddlZones").val();
        var mcode = Refnumber;

        ageindays = ageindays == undefined ? "0" : ageindays;
        textfieldname = textfieldname == undefined ? "" : textfieldname;
        $("#tbldatas").addClass("tableLoader");
        document.querySelector('#tbldatas').classList.add("tableLoader");
        var httpreq = {
            method: 'POST',
            url: 'MaterialAgeingReportNew.aspx/getMaterialAgeingReport',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'AgeinDays': ageindays, 'ZoneCode': textfieldname, 'Mcode': mcode, 'TenantID': Tenantid, 'WarehouseID': Warehouseid, 'IsExport': 0, 'NoofRecords': 25, 'PageNo': pageid },
            async: false
        }
        $http(httpreq).success(function (response) {
            // $scope.blockUI = false;
            //
            //$scope.BIllingReport = response.d;
            //if ($scope.BIllingReport == undefined || $scope.BIllingReport == null || $scope.BIllingReport.length == 0)
            //    showStickyToast(false, "No Data found");

            var dt = JSON.parse(response.d).Table;
            if (dt == undefined || dt == null || dt.length == 0) {
                showStickyToast(false, "No Data Found", false);
                $scope.AgeingData = null;
                $scope.Totalrecords = 0;
                document.querySelector('#tbldatas').classList.remove("tableLoader");
                return false;
            }
            $scope.AgeingData = dt;
            $scope.Totalrecords = $scope.AgeingData[0].TotalRecords;
            document.querySelector('#tbldatas').classList.remove("tableLoader");
        })
    };

    $scope.exprotData = function () {
        //debugger
        if ($scope.AgeingData == null || $scope.AgeingData == undefined) {
            showStickyToast(false, "No Data Found", false);
            return false;
        }

        //if ($("#txtTenant").val() == "0" || $("#txtTenant").val() == null || $("#txtTenant").val() == undefined || $("#txtTenant").val() == "") {

        //    showStickyToast(false, "Please select Tenant");
        //    return false;
        //}

        if ($("#txtWarehouse").val() == '' || $("#txtWarehouse").val() == null || $("#txtWarehouse").val() == undefined || $("#txtWarehouse").val() == "0") {

            showStickyToast(false, "Please select Warehouse");
            return false;
        }

        if ($("#ddlZones").val() == '' || $("#ddlZones").val() == null || $("#ddlZones").val() == undefined || $("#ddlZones").val() == 0) {

            //showStickyToast(false, "Please select Zone");
            //return false;
            textfieldname = "";
        }

        var ageindays = $scope.ddlAge;
        vtextfieldname = "";
        textfieldname = $("#ddlZones").val();
        var mcode = Refnumber;

        ageindays = ageindays == undefined ? "0" : ageindays;
        textfieldname = textfieldname == undefined ? "" : textfieldname;
        $("#tbldatas").addClass("tableLoader");
        document.querySelector('#tbldatas').classList.add("tableLoader");
        var httpreq = {
            method: 'POST',
            url: 'MaterialAgeingReportNew.aspx/getMaterialAgeingReport_Export',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'AgeinDays': ageindays, 'ZoneCode': textfieldname, 'Mcode': mcode, 'TenantID': Tenantid, 'WarehouseID': Warehouseid, 'IsExport': 1, 'NoofRecords': 200000, 'PageNo': 1 },
            async: false
        }
        $http(httpreq).success(function (response) {
            if (response.d == "No Data Found") {
                showStickyToast(false, "No Data Found", false);
                document.querySelector('#tbldatas').classList.remove("tableLoader");
                return false;
            }
            else {
                window.open('../ExcelData/' + response.d + ".xlsx");
            }
            document.querySelector('#tbldatas').classList.remove("tableLoader");
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
debugger;
        var table = $scope.BIllingReport;
        $("#tbldata").empty();
        $("#tbldata").append("<table><thead><tr><td><img src='../Images/INV_TraceabilityAssured.png' style='height: 56px;' border='0'></td></tr><tr><th>Part Number</th><th>Part Description </th><th>Tenant</th><th>Location</th><th>UoM/Qty.</th><th>Available Qty.</th><th>Received Date</th><th>Age in Days</th></thead><tbody>");
   if (table != null && table.length > 0) {       
 for (var i = 0; i < table.length; i++) {
            $("#tbldata").append("<tr></td><td class='aligntext'>" + table[i].PartNumber + "</td><td class='aligntext'>" + table[i].PartDescription + "</td><td class='aligntext'>" + table[i].Tenant + "</td><td class='aligntext'>" + table[i].Location + "</td><td class='aligntext'>" + table[i].UoM + "</td><td class='aligntext'>" + table[i].AvailableQty + "</td><td class='aligntext'>" + table[i].ReceivedDate + "</td><td class='aligntext'>" + table[i].AgeinDays + "</td></tr>");
        }
        $("#tbldata").append("</tbody></table>");
    }
else {

        $("#tbldata").append("<tr><td colspan='9' class='aligndate' style='background-color: white'>No Data Found</td></tr>");
        $("#tbldata").append("</tbody>");
    }
}

});