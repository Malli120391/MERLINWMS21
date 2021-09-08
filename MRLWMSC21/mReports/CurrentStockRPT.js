var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('CurrentStockRPT', function ($scope, $http) {
  
    var Tenantid = 0;
    var Warehouseid = 0;
    var fromLocID = 0;
    var toLocID = 0;
    //$scope.UserId1 ='<%=UserId %>';
    //alert($scope.UserId1);
    var userID = sessionStorage.getItem('UserID');
  //  alert(userID);
    $('#txtTenant').val("");
    var textfieldname = $("#txtTenant");
    DropdownFunction(textfieldname);
    $("#txtTenant").autocomplete({
        source: function (request, response) {
            debugger;
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/GetWarehouseTenant',
                data: "{ 'prefix': '" + request.term + "','WHID':'" + Warehouseid +"'}",
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
        //ending of warehouse dropdown


    $("#txtFromLocation").val("");
    var textfieldname = $("#txtFromLocation");
    DropdownFunction(textfieldname);
    $("#txtFromLocation").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/GetLocationListINCC',
                data: "{ 'Prefix': '" + request.term + "'}",
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
            fromLocID = i.item.val;
        },
        minLength: 0
    });

    $("#txtToLocation").val("");
    var textfieldname = $("#txtToLocation");
    DropdownFunction(textfieldname);
    $("#txtToLocation").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/GetLocationListINCC',
                data: "{ 'Prefix': '" + request.term + "'}",
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
            toLocID = i.item.val;
        },
        minLength: 0
    });

    $scope.getStockData = function (pageid) {
        //debugger
        if ($("#txtTenant").val() == "0" || $("#txtTenant").val() == "") {
            Tenantid = 0;
        }
        if ($("#txtWarehouse").val() == "0" || $("#txtWarehouse").val() == "") {
            Warehouseid = 0;
        }
        if ($("#txtFromLocation").val() == "0" || $("#txtFromLocation").val() == "") {
            fromLocID = 0;
        }
        if ($("#txtToLocation").val() == "0" || $("#txtToLocation").val() == "") {
            toLocID = 0;
        }
        $("#tbldatas").addClass("tableLoader");
        document.querySelector('#tbldatas').classList.add("tableLoader");
        var httpreq = {
            method: 'POST',
            url: 'CurrentStockRPT.aspx/GetStockData',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'TenantID': Tenantid, 'WHID': Warehouseid, 'fromLocID': fromLocID, 'toLocID': toLocID, 'IsExport': 0, 'NoofRecords': 25, 'PageNo': pageid},
            async: false
        }
        $http(httpreq).success(function (response) {
           // debugger
            var dt = JSON.parse(response.d).Table;
            if (dt == undefined || dt == null || dt.length == 0) {
                showStickyToast(false, "No Data Found", false);
                $scope.stockData = null;
                $scope.Totalrecords = 0;
                document.querySelector('#tbldatas').classList.remove("tableLoader");
                return false;
            }
            $scope.stockData = dt;
            $scope.Totalrecords = $scope.stockData[0].TotalRecords;
            document.querySelector('#tbldatas').classList.remove("tableLoader");
        })
    }


    $scope.exprotData = function () {
        //debugger
        if ($scope.stockData == null || $scope.stockData == undefined) {
            showStickyToast(false, "No Data Found", false);
            return false;
        }
        if ($("#txtTenant").val() == "0" || $("#txtTenant").val() == "") {
            Tenantid = 0;
        }
        if ($("#txtWarehouse").val() == "0" || $("#txtWarehouse").val() == "") {
            Warehouseid = 0;
        }
        if ($("#txtFromLocation").val() == "0" || $("#txtFromLocation").val() == "") {
            fromLocID = 0;
        }
        if ($("#txtToLocation").val() == "0" || $("#txtToLocation").val() == "") {
            toLocID = 0;
        }
        $("#tbldatas").addClass("tableLoader");
        document.querySelector('#tbldatas').classList.add("tableLoader");
        var httpreq = {
            method: 'POST',
            url: 'CurrentStockRPT.aspx/GetStockData_Export',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'TenantID': Tenantid, 'WHID': Warehouseid, 'fromLocID': fromLocID, 'toLocID': toLocID, 'IsExport': 1, 'NoofRecords': 200000, 'PageNo': 1 },
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

    $scope.exportExcel = function () {
        //
        if ($scope.stockData == null || $scope.stockData == undefined) {
            showStickyToast(false, "No Data Found", false);
            return false;
        }
        //document.querySelector('#tbldatas').classList.add("tableLoader");
        $scope.export();
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ type: 'excel' });
        $("#tbldata").css('display', 'none');
        //document.querySelector('#tbldatas').classList.remove("tableLoader");
    }
    
    $scope.export = function () {
    
        var table = $scope.stockData;

        $("#tbldata").empty();
        $("#tbldata").append("<table><thead><tr><td><img src='../Images/INV_TraceabilityAssured.png' style='height: 56px;' border='0'></td></tr><tr><th> Part No.</th><th>Description </th><th>Category</th><th>Company</th><th>Warehouse</th><th>Location</th><th>Pallet</th><th>Available Qty.</th><th>UoM</th></thead><tbody>");
        if (table != null && table.length > 0) {
            for (var i = 0; i < table.length; i++) {
                $("#tbldata").append("<tr></td><td class='aligntext'>" + table[i].MCode + "</td><td class='aligntext'>" + table[i].MDescription + "</td><td class='aligntext'>" + table[i].ProductCategory + "</td><td class='aligntext'>" + table[i].TenantName + "</td><td class='aligntext'>" + table[i].WHCode + "</td><td class='aligntext'>" + table[i].Location + "</td><td class='aligntext'>" + table[i].CartonCode + "</td><td class='aligntext'>" + table[i].AvaliableQuantity + "</td><td class='aligntext'>" + table[i].UoM + "</td></tr>");
            }
            $("#tbldata").append("</tbody></table>");
        }
        else {

            $("#tbldata").append("<tr><td colspan='6' class='aligndate' style='background-color: white'>No Data found</td></tr>");
            $("#tbldata").append("</tbody>");
        }
    }

});
