var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('ExpiredMaterialListNew', function ($scope, $http) {
  
    $scope.TenantId = "0";
    var TenantId = 0;
    $scope.MMID = "0";
    var Warehouseid = 0;
    var textfieldname = $("#txtTenant");
    DropdownFunction(textfieldname);
    $("#txtTenant").autocomplete({
        source: function (request, response) {
            if ($('#txtTenant').val() == '' || $('#txtTenant').val() == undefined) {
                // $scope.TenantId  = "0";
                TenantId=0
            }
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
            $scope.TenantId  = "0";
            $scope.TenantId = i.item.val;
            $("#hdnTenant").val(i.item.val);
           
            $('#txtPartNo').val(i.item.UOM);
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
                debugger;
                $.ajax({
                    //url: '../mWebServices/FalconWebService.asmx/LoadWHForWHList',
                    //data: "{ 'prefix': '" + request.term + "','TenantID':'" + TenantId + "'  }",
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
               // $("#txtWarehouse").val("");
                $("#hdnWarehouse").val(i.item.val);
                $("#txtTenant").val("");
                Tenantid = 0;
                $("#hdnTenant").val(0);

            },
            minLength: 0
        });
        //ending of warehouse dropdown
   // }   
    var textfieldname = $("#txtPartNo");
    DropdownFunction(textfieldname);
    $("#txtPartNo").autocomplete({
        source: function (request, response) {
            debugger;
            if ($('#txtTenant').val() == '' || $('#txtTenant').val() == undefined) {
                return false;
            }
            if ($('#txtPartNo').val() == '' || $('#txtPartNo').val() == undefined) {
                $scope.MMID = "0";
            }
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/getPartNOForJoblist',
                data: "{ 'prefix': '" + request.term + "','TenantID':'" + $scope.TenantId+ "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.split(',')[0],
                            val: item.split(',')[1],
                            UOM: item.split(',')[2],
                            UOMID: item.split(',')[3]
                        }

                        
                    }))
                    if (data.d.length == "0") {
                        showStickyToast(false, " No Material found ");
                        return false;
                    }
                }
            });
        },
        select: function (e, i) {
            $scope.MMID = "0";
            $scope.MMID = i.item.val;

          
        },
        minLength: 0
    });


    $scope.getExpiryMaterialReport = function (pageid) {

        debugger;
        //if ($("#txtTenant").val() == "0" || $("#txtTenant").val() == null || $("#txtTenant").val() == undefined || $("#txtTenant").val() == "") {

        //    showStickyToast(false, " Please select Tenant ");
        //    return false;
        //}
        if ($("#txtWarehouse").val() == "0" || $("#txtWarehouse").val() == null || $("#txtWarehouse").val() == undefined || $("#txtWarehouse").val() == "") {

            showStickyToast(false, " Please select Warehouse ");
            return false;
        }
        $("#tbldatas").addClass("tableLoader");
        document.querySelector('#tbldatas').classList.add("tableLoader");
        var httpreq = {
            method: 'POST',
            url: 'ExpiredMaterialListNew.aspx/GetExpMaterialList',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: {
                'TenantId': $scope.TenantId, 'MMId': $scope.MMID, 'WareHouseId': Warehouseid, 'IsExport': 0, 'NoofRecords': 25, 'PageNo': pageid
            },
            async: false
        }
        $http(httpreq).success(function (response) {
            var dt = JSON.parse(response.d).Table;
            if (dt == undefined || dt == null || dt.length == 0) {
                showStickyToast(false, "No Data Found", false);
                $scope.expDataList = null;
                $scope.Totalrecords = 0;
                document.querySelector('#tbldatas').classList.remove("tableLoader");
                return false;
            }
            $scope.expDataList = dt;
            $scope.Totalrecords = $scope.expDataList[0].TotalRecords;
            document.querySelector('#tbldatas').classList.remove("tableLoader");
        })
    };

    $scope.exprotData = function () {
        //debugger
        if ($scope.expDataList == null || $scope.expDataList == undefined) {
            showStickyToast(false, "No Data Found", false);
            return false;
        }
        //if ($("#txtTenant").val() == "0" || $("#txtTenant").val() == null || $("#txtTenant").val() == undefined || $("#txtTenant").val() == "") {

        //    showStickyToast(false, " Please select Tenant ");
        //    return false;
        //}
        if ($("#txtWarehouse").val() == "0" || $("#txtWarehouse").val() == null || $("#txtWarehouse").val() == undefined || $("#txtWarehouse").val() == "") {

            showStickyToast(false, " Please select Warehouse ");
            return false;
        }
        $("#tbldatas").addClass("tableLoader");
        document.querySelector('#tbldatas').classList.add("tableLoader");
        var httpreq = {
            method: 'POST',
            url: 'ExpiredMaterialListNew.aspx/GetExpMaterialList_Export',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: {
                'TenantId': $scope.TenantId, 'MMId': $scope.MMID, 'WareHouseId': Warehouseid, 'IsExport': 1, 'NoofRecords': 200000, 'PageNo': 1
            },
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

       
  //  $scope.getExpiryMaterialReport(1);

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
        $("#tbldata").append("<table><thead><tr><td><img src='../Images/INV_TraceabilityAssured.png' style='height: 56px;' border='0'></td></tr><tr><th> Part No</th><th>Tenant </th><th>Material Type</th><th>Location</th><th>Available Qty. </th><th>UoM/Qty.</th><th>Discrepancy</th><th>Expired On</th></thead><tbody>");
        if (table != null && table.length > 0) {
            for (var i = 0; i < table.length; i++) {
                $("#tbldata").append("<tr></td><td class='aligntext'>" + table[i].PartNo + "</td><td class='aligntext'>" + table[i].Tenant + "</td><td class='aligntext'>" + table[i].MaterialType + "</td><td class='aligntext'>" + table[i].Location + "</td><td class='aligntext'>" + table[i].AvailableQty + "</td><td class='aligntext'>" + table[i].UoM + "</td><td class='aligntext'>" + table[i].Discrepancy + "</td><td class='aligntext'>" + table[i].ExpiredOn + "</td></tr>");
            }
            $("#tbldata").append("</tbody></table>");
        }
        else {

            $("#tbldata").append("<tr><td colspan='6' class='aligndate' style='background-color: white'>No Data found</td></tr>");
            $("#tbldata").append("</tbody>");
        }
    }

});
