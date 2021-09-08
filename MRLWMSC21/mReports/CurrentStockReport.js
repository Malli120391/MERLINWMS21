//========================================  Modified by M.D.Prasad ==================================//


var myApp = angular.module('myApp', ['angularUtils.directives.dirPagination']);
myApp.controller('CurrentStockReport', function ($scope, $http, $compile) {
 
    var IndItemList = null;
    var Tenantid;
    var WHID = 0;
    var Tenant = 0;
    var PartNo = 0;
    var MTypeId = 0;
    var WareHouseID = 0;
    var LocId = 0;
    var KitID = 0;
    var Batchno = "";
    var OemNo = "";
    var mtypeid; 
    var Mcodeid; 
    var locid = 0;    
    var kit;
    var kitid;
    var containerID = 0;
    var pagenumber = 1;
    $scope.noofrecords = 25;
    $scope.Totalrecords = 0;

    $scope.MspItems = new Msp('', '', '', '', '', '', '', '', '');
    var httpreq1 = {

        method: 'POST',
        url: '../mReports/CurrentStockReport.aspx/GetPrinters',
        headers: {
            'Content-Type': 'application/json; charset=utf-8',
            'dataType': 'json'
        },
        data: {}
    }
    $http(httpreq1).success(function (response) {
        $scope.printers = response.d;
    });

    document.querySelector('#tbldatas').classList.remove("tableLoader");

    var httpreq = {
        method: 'POST',
        url: '../mReports/CurrentStockReport.aspx/GetLabels',
        headers: {
            'Content-Type': 'application/json; charset=utf-8',
            'dataType': 'json'
        },
        data: {}
    }
    $http(httpreq).success(function (response) {
        $scope.labels = response.d;
    });



    //alert("11");
    var RefTenant = '';   
    var Refpartno = '';   
    var Reflocno = '';  
    var Refkpid = '';  

    $scope.checkAll = function () {
        debugger;       
        if ($scope.selectedAll == true) {
            $scope.selectedAll = true;
        }
        else {
            $scope.selectedAll = false;
        }
        angular.forEach($scope.DynamicColumns, function (row) {
            row.selected = $scope.selectedAll;
        });
    };

    $scope.removeRow = function () {
        var newDataList = [];
        $scope.selectedAll = false;
        angular.forEach($scope.DynamicColumns, function (selected) {
            if (!selected.selected) {
                newDataList.push(selected);
            }
        });
        $scope.DynamicColumns = newDataList;
    };


    $scope.PrintTenantIds = [];
    $scope.print = function (BLR) {
        debugger;
        var dataattr = [];
        var printid = $scope.ddlPrinterType;
        var labelid = $scope.ddlPrintLabel;

        if (printid == "" || printid == undefined) {
            showStickyToast(false, "Please Select Printer");
            return false;
        }

        if (labelid == "" || labelid == undefined) {
            showStickyToast(false, "Please Select Label");
            return false;
        }
        $scope.PrintTenantIds = [];
        for (var i = 0; i < BLR.length; i++) {

            if (BLR[i].selected == true) {
                
                $scope.PrintTenantIds.push(BLR[i]);

            }
        }
        var ddd = 'hhh';
        var httpreq = $.ajax({
            type: 'POST',
            url: 'CurrentStockReport.aspx/GetPrint',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: JSON.stringify({ printobj: JSON.stringify($scope.PrintTenantIds), Printerid: printid, LabelID: labelid }),
            async: false,
            success: function (data) {
                debugger;
                var hh = data.d;
                $scope.BIllingReport = hh;

                if (data.d == "Printed Successfully") {
                    showStickyToast(true, "Printed Successfully");
                    setTimeout(function () {
                        location.reload();
                    }, 2000);
                }

                else {
                    showStickyToast(false, "Error While Printing");
                }
            }
        });
    };

//============================================== For Search Functionality ==============================================//



    var tid = 0;
    $scope.Getgedetails = function (PaginationId) {
        debugger;
       
        Tenant = 0;
        PartNo = 0;
        MTypeId = 0;
        WareHouseID = 0;
        containerDataID = 0;
        LocId = 0;
        KitID = 0;
        Batchno = "";
        OemNo = "";
        if ($("#txtTenant").val() == "") {
            Tenant = 0;
        }
        else {
            Tenant = RefTenant;
        }
        if ($("#txtPartnumber").val() == "") {
            PartNo = 0;
        }
        else {
            PartNo = Refpartno;
        }

        if ($("#txtMaterialType").val() == "") {
            MTypeId = 0;
        }
        else {
            MTypeId = Refmtype;
        }
        if ($("#txtWarehouse").val() == "") {
            showStickyToast(false, 'Please select WareHouse');
            return false;
        }
        else {
            WareHouseID = WHID;
        }

        if ($("#txtContainer").val() == "") {
            containerDataID = 0;
        }
        else {
            containerDataID = containerID;
        }

        if ($("#txtLocation").val() == "") {
            LocId = 0;
        }
        else {
            LocId = Reflocno;
        }

        if ($("#txtKitId").val() == "") {
            KitID = 0;
        }
        else {
            KitID = Refkpid;
        }
        if ($("#txtBatchNo").val() == undefined || $("#txtBatchNo").val() == "") {
            Batchno = "";
        }
        else {
            Batchno = $("#txtBatchNo").val();
        }

        if ($("#txtOEMpartNum").val() == undefined || $("#txtOEMpartNum").val() == "") {
            OemNo = "";
        }
        else {
            OemNo = $("#txtOEMpartNum").val();
        }
        if ($scope.TypeID != 0 || $scope.TypeID != undefined) {
            if ($("#txtTypeText").text == "") {
                var i = 0;
            }
            if ($("#txtTypeText").val() == undefined || $("#txtTypeText").val() == null || $("#txtTypeText").val() == 0) {
                Type = ""
            }
            else {
                Type = $("#txtTypeText").val();
            }
        }


        var FormAttributesData = $scope.FormAttributes();
        if (FormAttributesData != "<root></root>") {
            FormAttributesData = FormAttributesData;
        }
        else {
            FormAttributesData = null;
        }
       // $(".loaderforCurrentStock").show();
        //document.querySelector('#tbldatas').classList.add("tableLoader");
        $('inv-preloader').show();
        var httpreq = {
            method: 'POST',
            url: 'CurrentStockReport.aspx/GetCurrentStockDynamicData',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'typeid': $scope.TypeID, 'typetext': Type, 'Tenantid': Tenant, 'Mcode': PartNo, 'Mtypeid': MTypeId, 'BatchNo': Batchno, 'Location': LocId, 'kitId': KitID, 'JSON': FormAttributesData, 'IndustryID': parseInt($("#IndustryID").val()), 'Warehouseid': WareHouseID, 'OEMNum': OemNo, 'PageId': PaginationId, 'PageSize': $scope.noofrecords, 'containerID': containerDataID },
            async: false,
        }
        $http(httpreq).success(function (response) {
            debugger;
            var dt = JSON.parse(response.d);
            if (dt != "0") {

                $scope.Data = dt.Table;
                if (dt.Table != null && dt.Table.length > 0) {
                    $scope.DynamicHeaders = dt.Table1;
                    $scope.DynamicColumns = dt.Table;
                    $scope.Totalrecords = dt.Table2[0].NoOfRecords;
                    $scope.cols = Object.keys($scope.DynamicColumns[0]);
                    $("#tbldatas").css("display", "inline-table");
                    //$(".loaderforCurrentStock").hide();
                    //document.querySelector('#tbldatas').classList.remove("tableLoader");
                    $('inv-preloader').hide();
                }
                if ($scope.Data.length == 0) {
                    $scope.DynamicColumns = [];
                    $scope.DynamicHeaders = dt.Table1;
                    showStickyToast(false, "No Data Found", false);
                    $("#tbldatas").css("display", "inline-table");
                    // $(".loaderforCurrentStock").hide();
                    //document.querySelector('#tbldatas').classList.remove("tableLoader");
                    $('inv-preloader').hide();
                }
            }
            else {
                location.href = '../Login.aspx?FP=1&SE=1'; 
            }
        });



         
        //var httpreq = $.ajax({
        //    type: 'POST',
        //    url: 'CurrentStockReport.aspx/GetCurrentStockDynamicData',
        //    headers: {
        //        'Content-Type': 'application/json; charset=utf-8',
        //        'dataType': 'json'
        //    },
        //    data: JSON.stringify({ Tenantid: Tenant, Mcode: PartNo, Mtypeid: MTypeId, BatchNo: Batchno, Location: LocId, kitId: KitID, JSON: FormAttributesData, IndustryID: parseInt($("#IndustryID").val()), Warehouseid: WareHouseID, OEMNum: OemNo, PageId: PaginationId, PageSize: $scope.noofrecords }),
        //    async: false,
        //    success: function (data) {
        //        debugger;
        //        var dt = JSON.parse(data.d);
        //        $scope.Data = dt.Table;
        //        if (dt.Table != null && dt.Table.length > 0) {
        //            $scope.DynamicHeaders = dt.Table1;
        //            $scope.DynamicColumns = dt.Table;
        //            $scope.Totalrecords = dt.Table2[0].NoOfRecords;
        //            $scope.cols = Object.keys($scope.DynamicColumns[0]);
        //            $("#tbldatas").css("display", "inline-table"); 
        //            $(".loaderforCurrentStock").hide();
        //        }
        //        if ($scope.Data.length == 0) {
        //            $scope.DynamicColumns = [];
        //            $scope.DynamicHeaders = dt.Table1;
        //            showStickyToast(false, "No Data Found", false);
        //            $("#tbldatas").css("display", "inline-table");
        //            $(".loaderforCurrentStock").hide();
        //        }
        //    }
        //});
    }
//============================================== For Search Functionality ==============================================//

    $scope.clear = function () {
        $("#txtTenant").val("");
        $("#txtPartnumber").val("");
        $("#txtMaterialType").val("");
        $("#txtWarehouse").val("");
        $("#txtLocation").val("");
        $("#txtKitId").val("");

        $("#txtContainer").val("");

        $("#txtBatchNo").val("");
        $("#txtOEMpartNum").val("");
        $("#txtIndustry").val("");
        $("#selPrinterType").val("");
        $("#selPrintLabel").val("");
        $("#divIndustryContent").empty();
        //$("#divFirstAttibute").empty();
        //$("#divFirstAttibute1").empty();
        //$("#divFirstAttibute2").empty();
        $("#IndustryID").val("0");

        Tenant = 0;
        PartNo = 0;
        MTypeId = 0;
        WareHouseID = 0;
        containerID = 0;
        LocId = 0;
        KitID = 0;
        Batchno = "";
        OemNo = "";
    }

    $scope.FormAttributes = function () {
        debugger;
        var AttrfieldDataOut = '';
        var AttrfieldData = '<root>';

        $('.IndustryfieldToGet').each(function () {
            var param = $(this).attr('id').replace("Ind", "");
            var val = $(this).val() == null ? "" : $(this).val().trim();
            var paramtype = $(this).attr('type');
            AttrfieldData += '<data>';
            AttrfieldData += '<MM_MST_Attribute_ID>' + param + '</MM_MST_Attribute_ID>';
            AttrfieldData += '<GEN_MST_Industry_ID>' + $("#IndustryID").val() + '</GEN_MST_Industry_ID>';
            if (paramtype == undefined) {

                AttrfieldData += '<MM_MST_AttributeLookup_ID>' + val + '</MM_MST_AttributeLookup_ID>';
            }
            else {
                AttrfieldData += '<AttributeValue>' + val + '</AttributeValue>';
                AttrfieldData += '<MM_MST_AttributeLookup_ID>' + 0 + '</MM_MST_AttributeLookup_ID>';
            }
            AttrfieldData += '</data>';
        });

        AttrfieldData = AttrfieldData + '</root>';
        AttrfieldDataOut += AttrfieldData;
        return AttrfieldDataOut;
    }

    $scope.exportPdf = function () {
        // 
        $scope.export();
        $("#tbldata").css('display', 'block');
        $('#tbldata').tableExport({ type: 'pdf', jspdf: { orientation: 'l', margins: { right: 10, left: 10, top: 40, bottom: 40 }, autotable: { tableWidth: 'auto' } } });
        $("#tbldata").css('display', 'none');
        $scope.export();
    }
    //$scope.exportExcel = function () {
    //    //
    //    $scope.export();
    //    $("#tbldata").css('display', 'block');
    //    $('#tbldata').tableExport({ type: 'excel' });
    //    $("#tbldata").css('display', 'none');
    //}

    $scope.exportExcel = function () {
        debugger;
        if ($scope.Data == null || $scope.Data == undefined || $scope.Data.length == 0)
        {
            showStickyToast(false, 'No Data Found');
            return false;
        }
        var FormAttributesData = $scope.FormAttributes();
        if (FormAttributesData != "<root></root>") {
            FormAttributesData = FormAttributesData;
        }
        else {
            FormAttributesData = null;
        }
        var httpreq = {
            method: 'POST',
            url: 'CurrentStockReport.aspx/GetExcelData',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            data: { 'typeid': $scope.TypeID, 'typetext': $("#txtTypeText").val(), 'Tenantid': Tenant, 'Mcode': PartNo, 'Mtypeid': MTypeId, 'BatchNo': Batchno, 'Location': LocId, 'kitId': KitID, 'JSON': FormAttributesData, 'IndustryID': parseInt($("#IndustryID").val()), 'Warehouseid': WareHouseID, 'OEMNum': OemNo, 'PageId': 1, 'PageSize': 200000, 'containerID': containerDataID},
            async: false,
        }
        $http(httpreq).success(function (response) {
            debugger;
            window.open('../ExcelData/' + response.d + '.xlsx');
        });

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
    //debugger;       
        var dt1=  $scope.DynamicColumns;
        var dt2 =  $scope.DynamicHeaders;
                   
        $("#tbldata").empty();
        var tableData = '';
        tableData += ' <table id="tblStockData" class="table-striped dataTables-example"><thead><tr>';
        for (var i = 0; i < dt2.length; i++)
        {
            if(dt2[i].DynamicColumn != "InAct" && dt2[i].DynamicColumn != "OutAct")
            {
                tableData += '<th>' + dt2[i].DynamicColumn + '</th>';  
            }          
        }
        tableData += '</tr></thead><tbody>';
        for (var j = 0; j < dt1.length; j++)
        {
            tableData += '<tr>';
            var rows = Object.keys(dt1[0]);            
            for (var k = 0; k < rows.length-1; k++) {
                if (rows[k] != "TenantID" && rows[k] != "MaterialMasterID" && rows[k] != "RID") {
                    if (rows[k] != "InAct" && rows[k] != "OutAct") {
                        if (dt1[j][rows[k]] != null) {
                            tableData += '<td>' + dt1[j][rows[k]] + '</td>';
                        }
                        else {
                            tableData += '<td></td>';
                        }
                    }       
                }                
            }    
            tableData += '</tr>';
        }
        tableData += '</tbody></table>';
        $("#tbldata").append(tableData);
    }


    //========================================  For Auto Complete Textboxes ==================================//
    $('#txtTenant').val("");
    var textfieldname = $("#txtTenant");
    DropdownFunction(textfieldname);
    $("#txtTenant").autocomplete({
        source: function (request, response) {
            if ($("#txtTenant").val() == '') {
                RefTenant = 0;
            }
            if (WHID == 0 || WHID == "0" || WHID == undefined || WHID == null) {
                showStickyToast(false, 'Please select WareHouse');
                return false;
            }
            $.ajax({
               // url: '../mWebServices/FalconWebService.asmx/GetTenantList',                
               // data: "{ 'prefix': '" + request.term + "'}",
                url: '../mWebServices/FalconWebService.asmx/LoadTenantsByWH',
                data: "{ 'prefix': '" + request.term + "','whid':'" + WHID + "' }",
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
            RefTenant = i.item.val;
            $("#hdnTenantID").val(i.item.val);
            $("#txtPartnumber").val("");
            $("#txtMaterialType").val("");
           // $("#txtWarehouse").val("");
            $("#txtLocation").val("");
            $("#txtKitId").val("");
            $("#txtBatchNo").val("");
            $("#txtOEMpartNum").val("");
            $("#txtIndustry").val("");
            $("#selPrinterType").val("");
            $("#selPrintLabel").val("");
            $("#divIndustryContent").empty();
            //$("#divFirstAttibute").empty();
            //$("#divFirstAttibute1").empty();
            //$("#divFirstAttibute2").empty();
            $("#IndustryID").val("0");
            $("#txtContainer").val("");

            Tenant = 0;
            PartNo = 0;
            MTypeId = 0;
            WareHouseID = 0;
            containerID = 0;
            LocId = 0;
            KitID = 0;
            Batchno = "";
            OemNo = "";
        },
        minLength: 0
    });


    $('#txtPartnumber').val("");
    var textfieldname = $("#txtPartnumber");
    DropdownFunction(textfieldname);
    $("#txtPartnumber").autocomplete({
        source: function (request, response) {
            debugger;
            if (RefTenant == 0 || RefTenant == "0" || RefTenant == undefined || RefTenant == null) {
                showStickyToast(false, 'Please select Tenant');
                return false;
            }
            if ($("#txtPartnumber").val() == '') {
                Refpartno = 0;
            }
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/LoadMaterialsForCurrentStock',
                data: "{ 'prefix': '" + request.term + "','TenantID':'" + RefTenant + "'}",
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
            Refpartno = i.item.val;
        },
        minLength: 0
    });

    $("#txtMaterialType").val("");
    var textfieldname = $("#txtMaterialType");
    DropdownFunction(textfieldname);
    $("#txtMaterialType").autocomplete({
        source: function (request, response) {
            if (RefTenant == 0 || RefTenant == "0" || RefTenant == undefined || RefTenant == null) {
                showStickyToast(false, 'Please select Tenant');
                return false;
            }
            if ($("#txtMaterialType").val() == '') {
                Refmtype = 0;
            }
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/LoadMaterialTypesForCurrentStock',
                data: "{ 'prefix': '" + request.term + "','TenantID':'" + RefTenant + "'}",
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
            Refmtype = i.item.val;
        },
        minLength: 0
    });

    $("#txtWarehouse").val("");
    var textfieldname = $("#txtWarehouse");
    DropdownFunction(textfieldname);
    $("#txtWarehouse").autocomplete({
        source: function (request, response) {
            //if (RefTenant == 0 || RefTenant == "0" || RefTenant == undefined || RefTenant == null) {
            //    showStickyToast(false, 'Please select Tenant');
            //    return false;
            //}
            //if ($("#txtWarehouse").val() == '') {
            //    WHID = 0;
            //}
            $.ajax({
               // url: '../mWebServices/FalconWebService.asmx/LoadWHForWHList_CurrentStock',
                //data: "{ 'prefix': '" + request.term + "','TenantID':'" + RefTenant + "'}",
               // data: "{ 'prefix': '" + request.term + "','TenantID':'" + 0 + "'}",
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
            WHID = i.item.val;
            $("#txtPartnumber").val("");
            $("#txtMaterialType").val("");
            $("#txtTenant").val("");
            $("#txtLocation").val("");
            $("#txtKitId").val("");
            $("#txtBatchNo").val("");
            $("#txtOEMpartNum").val("");
            $("#txtIndustry").val("");
            $("#selPrinterType").val("");
            $("#selPrintLabel").val("");

        },
        minLength: 0
    });

    $("#txtLocation").val("");
    var textfieldname = $("#txtLocation");
    DropdownFunction(textfieldname);
    $("#txtLocation").autocomplete({
        source: function (request, response) {
            //debugger;
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/LoadLocationsForCurrentStock',
                data: "{ 'prefix': '" + request.term + "','WarehouseID':'" + WHID + "'}",
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
            Reflocno = i.item.val;
        },
        minLength: 0
    });

    $("#txtKitId").val("");
    var textfieldname = $("#txtKitId");
    DropdownFunction(textfieldname);
    $("#txtKitId").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/GetKitPlannerId',
                data: "{ 'prefix': '" + request.term + "','TenantID':'" + RefTenant + "'}",
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
            Refkpid = i.item.val;
        },
        minLength: 0
    });


    $("#txtContainer").val("");
    var textfieldname = $("#txtContainer");
    DropdownFunction(textfieldname);
    $("#txtContainer").autocomplete({
        source: function (request, response) {
            //debugger;
            if ($("#txtWarehouse").val() == "" || WHID == 0 || WHID == undefined || WHID == null)
            {
                showStickyToast(false, "Please Select Warehouse");
                return false;
            }
            if ($("#txtContainer").val() == "") {
                containerID = 0;
            }
            $.ajax({
                url: '../mWebServices/FalconWebService.asmx/getContainers',
                data: "{ 'prefix': '" + request.term + "','WarehouseID':'" + WHID + "'}",
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
            containerID = i.item.val;
        },
        minLength: 0
    });


    //======================================== END ======================================================//






    //===============================================  Added by M.D.Prasad  Industry Attributes ===========================//

    var Industryfield = $('#txtIndustry');
    DropdownFunction(Industryfield);
 
    $("#txtIndustry").autocomplete({
       
        source: function (request, response) {
            if ($("#txtIndustry").val() == "") {
                $("#IndustryID").val(0);
            }
            $.ajax({
                url: "../mWebServices/FalconWebService.asmx/LoadIndustries_Auto",
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
                },
                error: function (response) {

                },
                failure: function (response) {

                }
            });
        },
        select: function (e, i) {
            $("#IndustryID").val(i.item.val);
            $scope.GetIndustries();
            $scope.getIndustryFromid(i.item.val);
        },
        minLength: 0
    });
    $scope.TypeID = 0;
    var Industryfield = $('#txttype');
    DropdownFunction(Industryfield);
    $("#txttype").autocomplete({
        source: function (request, response) {
            if ($('#txttype').val() == '') {
                $scope.TypeID = 0;
            }
            $.ajax({
                url: "../mWebServices/FalconWebService.asmx/GettingTypesDataforCurrentStock",
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
                },
                error: function (response) {

                },
                failure: function (response) {

                }
            });
        },
        select: function (e, i) {
            $scope.TypeID = i.item.val;
            //$("#IndustryID").val(i.item.val);
            //$scope.getIndustryFromid(i.item.val);
        },
        minLength: 0
    });

    $scope.GetIndustries = function () {
        var MasterID = 0;
        var TenantId = 0;
        if (RefTenant != null && RefTenant != undefined && RefTenant != "") {
            TenantId = RefTenant;
        }
        if (Refpartno != null && Refpartno != undefined && Refpartno != "") {
            MasterID = Refpartno;
        }
        $.ajax({
            url: '../mMaterialManagement/ItemMasterRequest.aspx/GetIndustries',
            //data: "{'MaterialMasterID' : '" + MasterID + "'}",
            data: "{'MaterialMasterID' : '" + MasterID + "','AccountID' : '" + $("#Account").val() + "',TenantId:'" + TenantId + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (response) {
                var dt = JSON.parse(response.d);
                IndItemList = dt;
            },
            error: function (response) {

            },
            failure: function (response) {

            }
        });
    }
    $scope.GetIndustries();

    $scope.GetIndustryAttributes = function (industryID) {
        if (industryID != 0) {
            var item = $.grep(IndItemList.Table, function (a) { return a.GEN_MST_Industry_ID == industryID });
            if (item != null && item.length > 0) {
                var container = null;
                container = document.getElementById('divIndustryContent');
                containerOne = document.getElementById('divFirstAttibute');
                containerTwo = document.getElementById('divFirstAttibute1');
                containerThree = document.getElementById('divFirstAttibute2');
                var IndustryContent = '';
                var IndustryContentOne = '';
                var IndustryContentTwo = '';
                var IndustryContentThree = '';
                //IndustryContent += '<div class="row">';      commented by lalitha on 09/04/2019
                for (var i = 0; i < item.length; i++) {
                    debugger;
                    if (item[i].UIControlType == "DatePicker") {
                        if (i == 0) {
                            IndustryContentOne += '<input  type="text" class="txt_Blue_Small DueDate IndustryfieldToGet" id="Ind' + item[i].MM_MST_Attribute_ID + '" readonly="true"/><label>' + item[i].UILabelText + '</label>';
                        }
                        else if (i == 1) { IndustryContentTwo += '<input  type="text" class="txt_Blue_Small DueDate IndustryfieldToGet" id="Ind' + item[i].MM_MST_Attribute_ID + '" readonly="true"/><label>' + item[i].UILabelText + '</label>'; }
                        else if (i == 2) { IndustryContentThree += '<input  type="text" class="txt_Blue_Small DueDate IndustryfieldToGet" id="Ind' + item[i].MM_MST_Attribute_ID + '" readonly="true"/><label>' + item[i].UILabelText + '</label>'; }
                        else {
                            IndustryContent += '<div class="col-md-3"> <div class="flex"> <input  type="text" class="txt_Blue_Small DueDate IndustryfieldToGet" id="Ind' + item[i].MM_MST_Attribute_ID + '" readonly="true"/><label>' + item[i].UILabelText + '</label></div></div>';
                        }
                    }
                    else if (item[i].UIControlType == "DropdownList") {

                        if (i == 0) {
                            IndustryContentOne += '<select  style="    width: 103% !important;" class="txt_Blue_Small IndustryfieldToGet" id="Ind' + item[i].MM_MST_Attribute_ID + '" onchange="getchildattributelist(this);"></select><label>' + item[i].UILabelText + '</label>';
                        }
                        else if (i == 1) { IndustryContentTwo += '<select  style="    width: 103% !important;" class="txt_Blue_Small IndustryfieldToGet" id="Ind' + item[i].MM_MST_Attribute_ID + '" onchange="getchildattributelist(this);"></select><label>' + item[i].UILabelText + '</label>'; }
                        else if (i == 2) { IndustryContentThree += '<select  style="    width: 505% !important;" class="txt_Blue_Small IndustryfieldToGet" id="Ind' + item[i].MM_MST_Attribute_ID + '" onchange="getchildattributelist(this);"></select><label>' + item[i].UILabelText + '</label>'; }

                        else {
                            IndustryContent += '<div class="col-md-3"> <div class="flex"> <select  style="    width: 700% !important;" class="txt_Blue_Small IndustryfieldToGet" id="Ind' + item[i].MM_MST_Attribute_ID + '" onchange="getchildattributelist(this);"></select><label>' + item[i].UILabelText + '</label></div></div>';
                        }
                    }
                    else if (item[i].UIControlType == "TextBox") {
                        if (i == 0) {
                            IndustryContentOne += '<input required="" type="text" class="txt_Blue_Small IndustryfieldToGet" id="Ind' + item[i].MM_MST_Attribute_ID + '" onclick="checkNegativeValue(this);"/><label>' + item[i].UILabelText + '</label>';
                        }
                        else if (i == 1) { IndustryContentTwo += '<input required="" type="text" class="txt_Blue_Small IndustryfieldToGet" id="Ind' + item[i].MM_MST_Attribute_ID + '" onclick="checkNegativeValue(this);"/><label>' + item[i].UILabelText + '</label>'; }
                        else if (i == 2) { IndustryContentThree += '<input required="" type="text" class="txt_Blue_Small IndustryfieldToGet" id="Ind' + item[i].MM_MST_Attribute_ID + '" onclick="checkNegativeValue(this);"/><label>' + item[i].UILabelText + '</label>'; }
                        else {
                            //IndustryContent += '<div class="col-md-3"> <div class="flex"><input required="" type="text" class="txt_Blue_Small IndustryfieldToGet" id="Ind' + item[i].MM_MST_Attribute_ID + '" onclick="checkNegativeValue(this);"/><label>' + item[i].UILabelText + '</label></div></div>';
                            IndustryContent += '<input required="" type="text" class="txt_Blue_Small IndustryfieldToGet" id="Ind' + item[i].MM_MST_Attribute_ID + '" onclick="checkNegativeValue(this);"/><label>' + item[i].UILabelText + '</label>';

                        }
                    }
                }


                //IndustryContent += '</div>';
                containerOne.innerHTML = IndustryContentOne;
                containerTwo.innerHTML = IndustryContentTwo;
                containerThree.innerHTML = IndustryContentThree;
                container.innerHTML = IndustryContent;

                for (var i = 0; i < item.length; i++) {
                    var attrlist = $.grep(IndItemList.Table1, function (a) { return a.GEN_MST_Industry_ID == item[i].GEN_MST_Industry_ID });
                    var attrlistwithselectdata = $.grep(attrlist, function (a) { return a.MM_MST_Attribute_ID == item[i].MM_MST_Attribute_ID });
                    if (item[i].UIControlType == "DropdownList") {
                        $scope.BindInvDropdowns(attrlistwithselectdata, item[i].MM_MST_Attribute_ID);
                    }
                    else if (item[i].UIControlType == "DatePicker") {
                        $scope.datepicker();
                    }
                }
            }
            else {

                $("#divIndustryContent").empty();
                //$("#divFirstAttibute").empty();
                //$("#divFirstAttibute1").empty();
                //$("#divFirstAttibute2").empty();
            }
        }
        else {
        }
    }

    $scope.BindInvDropdowns = function (dt, attributeid) {
        //KeyText, KeyValue
        if (dt != null && dt != '') {
            for (var x = 0; x < dt.length; x++) {
                if (x == 0) {
                    $('#Ind' + dt[x].MM_MST_Attribute_ID).empty();
                    $("#Ind" + dt[x].MM_MST_Attribute_ID).append($("<option></option>").val(0).html("Please Select"));
                }
                $("#Ind" + dt[x].MM_MST_Attribute_ID).append($("<option></option>").val(dt[x].KeyValue).html(dt[x].KeyText));
            }
        }
        else {
            $('#Ind' + attributeid).empty();
            $("#Ind" + attributeid).append($("<option></option>").val(0).html("Please Select"));
        }
    }


    $scope.checkNegativeValue = function () {
        var value = parseFloat(document.getElementById("TextBox").value);
        if (value < 0) {
            showStickyToast(false, "Negative Value is not allowed");

            return false;
        }
    }

    $scope.getIndustryFromid = function (id) {
        $scope.GetIndustryAttributes(id);
    }
    $scope.datepicker = function () {
        $('.DueDate').datepicker({
            singleDatePicker: true,
            showDropdowns: true,
            autoclose: true,
            dateFormat: "dd-M-yy",
            forceParse: false,
            viewMode: "days",
            minViewMode: "days",
            minDate: 0,
            endDate: "today"
        });
    }


    //================================== END ===========================================//
    
    //================================== FOR Column Options ============================//


            $scope.GetColumnOptionData = function() {
            debugger;
            var pos = 0;
            var data = [];
            var slData = [];
            var slType = [];
            var sort = 0;
            var $el = $("#selSelected");
            var sortVal = 0;
            $el.find('option').each(function () {
                data.push({ value: $(this).val(), text: $(this).text() });
            });

            var $sl = $("#selSort");
            var sortVal = 0;
            $sl.find('option').each(function () {
                pos = pos + 1;
                slData.push({ value: $(this).val(), text: $(this).text().split("-")[0].trim(), position: pos, sortType: $(this).text() });
            });

            var fieldData = '<root>';
            var sortseq = $("#selSort option").length;
            for (var i = 0; i < data.length; i++) {
                fieldData += '<data>';
                fieldData += '<GEN_MST_DynamicColumn_ID>' + data[i].value + '</GEN_MST_DynamicColumn_ID>';
                fieldData += '<IsDisplay>1</IsDisplay>';
                fieldData += '<DisplaySequence>' + (i + 1) + '</DisplaySequence>';

                var d = $.grep(slData, function (a) { return a.text == data[i].text });
                if(d!=null && d.length>0)
                {
                    if(d[0].text == data[i].text){
                    var item = $.grep(slData, function (a) { return a.text == data[i].text });
                        if (item != null && item.length > 0) {
                            fieldData += '<SortSequence>' + item[0].position + '</SortSequence>';
                            var srtType = item[0].sortType.split("-")[1].trim();
                            if (srtType == "[ Asc ]") { sortVal = 0; } else { sortVal = 1; }
                            fieldData += '<SortType>' + sortVal + '</SortType>';
                            fieldData += '<IsSort>1</IsSort></data>';
                        }
                        else {
                            fieldData += '<IsSort></IsSort>';
                            fieldData += '<SortSequence></SortSequence>';
                            fieldData += '<SortType></SortType></data>';
                        }
                    }
                    else 
                    {
                        fieldData += '<IsSort></IsSort>';
                        fieldData += '<SortSequence></SortSequence>';
                        fieldData += '<SortType></SortType></data>';
                    }
                }

            else {
                    fieldData += '<IsSort></IsSort>';
                    fieldData += '<SortSequence></SortSequence>';
                    fieldData += '<SortType></SortType></data>';
                }
            }
            fieldData = fieldData + '</root>';            
            return fieldData;
        }
        
            $scope.UpsertData = function() 
            {
                debugger;
                var MenuID = $(".activeMenu a").attr("class").split("MenuID")[1];
                var obj = {};
                obj.MenuID = MenuID;
                obj.DynamicXML = $scope.GetColumnOptionData();
                var httpreq = $.ajax({
                            type: 'POST',
                            url: 'CurrentStockReport.aspx/SET_CurrentStockColumnOptionData',
                            headers: {
                                'Content-Type': 'application/json; charset=utf-8',
                                'dataType': 'json'
                            },
                            data: JSON.stringify(obj),
                            async: false,
                            success: function (data) {
                            debugger;
                            $scope.Getgedetails(1);
                            $('#showColOptions').modal('hide');
                            showStickyToast(true, 'Created Successfully', false);
                            }
                        });
            }     

    //$scope.GetMspData = function (MID) {
    //    debugger;
    //    var httpreq = $.ajax({
    //        type: 'POST',
    //        url: 'CurrentStockReport.aspx/GetMspItems',
    //        headers: {
    //            'Content-Type': 'application/json; charset=utf-8',
    //            'dataType': 'json'
    //        },
    //        data: JSON.stringify({ MaterialMasterID: MID }),
    //        async: false,
    //        success: function (data) {
    //            debugger;
    //            $scope.MspItems = JSON.parse(data.d).Table;
    //        }
    //    });
    //}

    $scope.GetMspData1 = function (obj) {
        debugger;
        $scope.ItemDetails = obj;
        $scope.MspItems.MCode = $scope.ItemDetails["Part#"];
        $scope.MspItems.BatchNo = $scope.ItemDetails["Batch#"];
        $scope.MspItems.SerialNo = $scope.ItemDetails["Serial#"];
        $scope.MspItems.MfgDate = $scope.ItemDetails["Mfg.Dt."];
        $scope.MspItems.ExpDate = $scope.ItemDetails["Exp.Dt."];
        $scope.MspItems.ProjectRefNo = $scope.ItemDetails["ProjectRefNo"];
        $scope.MspItems.MRP = $scope.ItemDetails.MRP;
        $scope.MspItems.Quantity = $scope.ItemDetails.QoH;
        $scope.MspItems.GRNNumber = $scope.ItemDetails["GRN Number"];

    }


            $scope.GetPrintData = function (obj) {
                debugger;
                var dataattr = [];
                var printid = $scope.ddlPrinterType;
                var labelid = $scope.ddlPrintLabel;
                var copies = obj.NoOfCopies;

                //if (printid == "" || printid == undefined) {
                //    showStickyToast(false, "Please Select Printer");
                //    return false;
                //}
                if ($('#pid').val() == "3")
                {
                    if ($("#netPrinterHost").val() == "")
                    {
                        showStickyToast(false, "Please enter Printer IP Address", false);
                        return false;
                    }
                    if ($("#netPrinterPort").val() == "")
                    {
                        showStickyToast(false, "Please enter Printer Port", false);
                        return false;
                    }
                }

                if (labelid == "" || labelid == undefined) {
                    showStickyToast(false, "Please Select Label", false);
                    return false;
                }

                if (copies == "" || copies == undefined) {
                    showStickyToast(false, "Please Enter No Of Copies.", false);
                    return false;
                }
                $scope.PrintData = [];
                $scope.PrintData.push(obj);

                var ddd = 'hhh';

               // $(".loaderforCurrentStock").show();
                document.querySelector('#tbldatas').classList.add("tableLoader");
                var httpreq = {
                    method: 'POST',
                    url: 'CurrentStockReport.aspx/GetPrint',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8',
                        'dataType': 'json'
                    },
                    data: { 'printobj': $scope.PrintData, 'Printerid': 0, 'LabelID': labelid },
                    async: false,
                }
                $http(httpreq).success(function (response) {
                    debugger;
                    var dt = response.d;
                    if (dt == "") {
                        //$("#entered_name").val("");
                        //$("#entered_name").val(dt);
                        //sendData();
                        //$(".loaderforCurrentStock").hide();
                        document.querySelector('#tbldatas').classList.remove("tableLoader");
                        showStickyToast(false, "Error occured", false);
                      //  return false;
                    }
                    else {
                        //$(".loaderforCurrentStock").hide();
                        $("#printerCommands").val("");
                        $("#printerCommands").val(dt);
                        javascript: doClientPrint();
                        document.querySelector('#tbldatas').classList.remove("tableLoader");
                        showStickyToast(true, "Successfully Printed", false);
                        //showStickyToast(false, "Error while printing", false);
                       // return false;
                    }
                });






                //var httpreq = $.ajax({
                //    type: 'POST',
                //    url: 'CurrentStockReport.aspx/GetPrint',
                //    headers: {
                //        'Content-Type': 'application/json; charset=utf-8',
                //        'dataType': 'json'
                //    },
                //    data: JSON.stringify({ printobj: $scope.PrintData, Printerid: printid, LabelID: labelid }),
                //    async: false,
                //    success: function (data) {
                //        debugger;
                //        var hh = data.d;
                //        $scope.BIllingReport = hh;

                //        if (data.d == "Printed Successfully") {
                //            showStickyToast(true, "Printed Successfully");
                //            setTimeout(function () {
                //            }, 2000);
                //        }

                //        else {
                //            showStickyToast(false, "Error While Printing");
                //        }
                //});
    };


    function Msp(MCode, BatchNo, SerialNo, MfgDate, ExpDate, ProjectRefNo, MRP, Quantity, GRNNumber) {
        
        this.MCode = MCode;
        this.BatchNo = BatchNo;
        this.SerialNo = SerialNo;
        this.MfgDate = MfgDate;
        this.ExpDate = ExpDate;
        this.ProjectRefNo = ProjectRefNo;
        this.MRP = MRP;
        this.Quantity = Quantity;
        this.GRNNumber = GRNNumber;
    }

});


