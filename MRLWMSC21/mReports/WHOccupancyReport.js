var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('WHOccupancyReport', function ($scope, $http) {
  
    var Tenantid = 0;
    var Warehouseid = 0;
    $('#txtTenant').val("");
    var textfieldname = $("#txtTenant");
    $(".hideContent").hide();
    DropdownFunction(textfieldname);
    $("#txtTenant").autocomplete({
        source: function (request, response) {
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
            Tenantid = i.item.val;
            $("#hdnTenant").val(i.item.val);
        },
        minLength: 0
    });

    //$("#txtWarehouse").val("");
    var textfieldname = $("#txtWarehouse");
    DropdownFunction(textfieldname);
    $("#txtWarehouse").autocomplete({
        source: function (request, response) {
            debugger
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/LoadWHForWHListWithoutTenant',
                data: "{ 'prefix': '" + request.term + "','TenantID':'" + Tenantid + "'  }",
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
   
    $scope.GetWHDataReport = function () {
        //debugger
        if ($("#txtTenant").val() == "0" || $("#txtTenant").val() == "") {
            Tenantid = 0;
            //showStickyToast(false, "Please select Tenant ");
            //return false;
        }
        if ($("#txtWarehouse").val() == "0" || $("#txtWarehouse").val() == "") {
            Warehouseid = 0;
            showStickyToast(false, "Please select Warehouse ");
            return false;
        }
        if ($("#txtDate").val() == "") {
            showStickyToast(false, "Please select Date ");
            return false;
        }
        $(".hideContent").show();
        $scope.warehouse = Warehouseid;
        $("#tbldatas").addClass("tableLoader");
        document.querySelector('#tbldatas').classList.add("tableLoader");
        var httpreq = {
            method: 'POST',
            url: 'WHOccupancyReport.aspx/GetWHData',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: {
                'TenantID': Tenantid, 'WHID': Warehouseid,'date':$("#txtDate").val() },
            async: false
        }
        $http(httpreq).success(function (response) {
            //debugger
            var dt = JSON.parse(response.d).Table;
            if (dt != null && dt.length > 0) {
                $scope.WHData = dt;
                $scope.WHDataTotal = JSON.parse(response.d).Table1;
                document.querySelector('#tbldatas').classList.remove("tableLoader");
            }
            else {
                $scope.WHData = null;
                showStickyToast(false, "No Data Found");
                document.querySelector('#tbldatas').classList.remove("tableLoader");
                return false;
            }
        })
    }

    $scope.exportExcel = function () {
        if ($scope.WHData == null || $scope.WHData == undefined) {
            showStickyToast(false, "No Data Found", false);
            return false;
        }
        $scope.export();
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ type: 'excel' });
        $("#tbldata").css('display', 'none');
    }

    $scope.export = function () {
    
        var table = $scope.WHData;
        var table2 = $scope.WHDataTotal;
        $("#tbldata").empty();
        $("#tbldata").append("<table><thead><tr><th>Company </th><th>Available Qty.</th><th>Total Volume (CBM)</th><th>Occupied Percentage %</th></thead><tbody>");
        if (table != null && table.length > 0) {
            for (var i = 0; i < table.length; i++) {
                $("#tbldata").append("<tr><td class='aligntext'>" + table[i].TenantName + "</td><td class='aligntext'>" + table[i].AvailableQty + "</td><td class='aligntext'>" + table[i].TotalVolume + "</td><td class='aligntext'>" + table[i].Occupancy + "</td></tr>");
            }
            $("#tbldata").append("<tr><td class='aligntext'></td><td class='aligntext'></td><td class='aligntext'></td><td class='aligntext'></td></tr>");
            $("#tbldata").append("<tr><td class='aligntext'></td><td class='aligntext'></td><td class='aligntext'></td><td class='aligntext'></td></tr>");
            $("#tbldata").append("<tr><td class='aligntext'></td><td class='aligntext'><b>Total Warehouse Volume (CBM)</b></td><td class='aligntext'><b>Total Occupied Volume (CBM)</b></td><td class='aligntext'><b>Total Available Volume (CBM)</b></td></tr>");
            $("#tbldata").append("<tr><td class='aligntext'></td><td class='aligntext'>" + table2[0].WarehouseVolume + "</td><td class='aligntext'>" + table2[0].OccupiedVolume + "</td><td class='aligntext'>" + table2[0].AvailableVolume + "</td></tr>");
            $("#tbldata").append("</tbody></table>");
        }
        else {

            $("#tbldata").append("<tr><td colspan='6' class='aligndate' style='background-color: white'>No Data found</td></tr>");
            $("#tbldata").append("</tbody>");
        }
    }

    $scope.GetWHDataReportPDF = function () {
        debugger;
        if ($("#txtTenant").val() == "0" || $("#txtTenant").val() == "") {
            Tenantid = 0;
            //showStickyToast(false, "Please select Tenant ");
           // return false;
        }
        if ($("#txtWarehouse").val() == "0" || $("#txtWarehouse").val() == "") {
            Warehouseid = 0;
            showStickyToast(false, "Please select Warehouse ");
            return false;
        }
        if ($("#txtDate").val() == "") {
            showStickyToast(false, "Please select Date ");
            return false;
        }
        var httpreq = {
            method: 'POST',
            url: 'WHOccupancyReport.aspx/genratePDF',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: {'TenantID': Tenantid, 'WHID': Warehouseid, 'date': $("#txtDate").val()},
            async: false
        }
        $http(httpreq).success(function (response) {
            debugger;
            if (response.d != null) {
                var obj = response.d;
                showStickyToast(true, "PDF Generated Successfully ", false);
                window.open('../mOutbound/PackingSlip/' + obj );
                $("#divLoading").hide();
                return false;

            }
            else {
                showStickyToast(false, "No Data Found ", false);
                $("#divLoading").hide();
                return false;
            }
        })
    }

});
