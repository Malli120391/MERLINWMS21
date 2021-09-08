var myApp = angular.module('MyApp', ['angularUtils.directives.dirPagination']);
myApp.controller('LostAndFoundReport', function ($scope, $http) {
    var WarehouseLoc = '';
    var TenantName = '';
    var SupplierID = '';
    var MaterialID = '';
    var sFileName = 'LostAndFoundReport';
    $scope.date = new Date();
    $scope.getWarehouse = function () {
        //debugger;
        var textfieldname = $("#txtWarehouse");
        DropdownFunction(textfieldname);
        $("#txtWarehouse").autocomplete({
            source: function (request, response) {
                $.ajax({
                    
                    //url: '../mWebServices/FalconWebService.asmx/LoadWarehouseByLoc',
                    //data: "{ 'prefix': '" + request.term + "'}",
                    url: '../mWebServices/FalconWebService.asmx/LoadWarehouseBasedonUser',
                    data: "{ 'prefix': '" + request.term + "'}",//<=cp.TenantID%>
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
                WarehouseLoc = i.item.val;
            },
            minLength: 0
        });
    }
    $scope.getTenant = function () {
        var textfieldname = $("#txtTenant");
        DropdownFunction(textfieldname);
        $("#txtTenant").autocomplete({
            source: function (request, response) {
                $.ajax({
                    //url: '../mWebServices/FalconWebService.asmx/LoadTenantDataFor3PL',
                    //data: "{ 'prefix': '" + request.term + "'}",
                    url: '../mWebServices/FalconWebService.asmx/GetWarehouseTenant',
                    data: "{ 'prefix': '" + request.term + "','WHID':'" + WarehouseLoc + "'}",

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
                TenantName = i.item.val;
            },
            minLength: 0
        });
    }
    $scope.getSupplier = function () {
        var textfieldname = $("#txtSupplier");
        DropdownFunction(textfieldname);
        //debugger;
        $("#txtSupplier").autocomplete({            
            source: function (request, response) {
                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/LoadSpplierDataBasedTenant',
                    data: "{ 'prefix': '" + request.term + "','TenantID': '" + TenantName + "'}",
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
                SupplierID = i.item.val;
            },
            minLength: 0
        });
    }
    $scope.getMaterial = function () {
        var textfieldname = $("#txtMaterial");
        DropdownFunction(textfieldname);
        //debugger;
        $("#txtMaterial").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '../mWebServices/FalconWebService.asmx/LoadMaterialDataBasedSupplier',
                    data: "{ 'prefix': '" + request.term + "','SupplierID': '" + SupplierID + "'}",
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
                MaterialID = i.item.val;
            },
            minLength: 0
        });
    }
    $scope.GetLFdetails = function () {
        //debugger;
        if ($("#txtWarehouse").val() == "")
        {
            showStickyToast(false, "Please select Warehouse", false);
            return;
        }
        if ($("#txtFromdate").val() == "") {
            showStickyToast(false, "Please select Start Date", false);
            return;
        }
        $scope.LFReport = null;
        var warehouseid = WarehouseLoc;
        var startdate = $("#txtFromdate").val();
        var enddate = $("#txttodate").val();
        var tenantid  ="";
        var supplierid="";
        var materialid = "";
        if ($("#txtTenant").val() == "") {
            tenantid = "";
        }
        else
        {
            tenantid = TenantName;
        }

        if ($("#txtSupplier").val() == "") {
            supplierid = "";
        }
        else {
            supplierid = SupplierID;
        }

        if ($("#txtMaterial").val() == "") {
            materialid = "";
        }
        else {
            materialid = MaterialID;
        }

        var httpreq = {
            method: 'POST',
            url: 'LostAndFoundReport.aspx/GetLostFoundReportList',
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
                'dataType': 'json'
            },
            //data: "{ 'WarehouseID': '" + warehouseid + "','StartDate': '" + startdate + "','EndDate': '" + enddate + "','TenantID': '" + tenantid + "','SupplierID': '" + supplierid + "','MaterialID': '" + materialid + "' }",
            data: "{ 'WarehouseID': '" + warehouseid + "','StartDate': '" + startdate + "','EndDate': '" + enddate + "','TenantID': '" + tenantid + "','SupplierID': '" + supplierid + "','MaterialID': '" + materialid + "'}",
            async: false
        }
        $http(httpreq).success(function (response) {
            debugger;
            $scope.LAFReport = response.d;            
            if ($scope.LAFReport == undefined || $scope.LAFReport == null || $scope.LAFReport.length == 0)
                showStickyToast(false, "No Data found");
            $scope.OBTable = $scope.LAFReport[0].objOBTable;
            $scope.MTable = $scope.LAFReport[0].objMainTable;
            $scope.CBTable = $scope.LAFReport[0].objCBTable;
        })
    }

    $scope.exportPdf = function () {
        //debugger;
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
        $('#tbldata').tableExport({
            fileName: sFileName, type: 'csv', headers: true,
            //footers: true,
            //formats: ['xlsx', 'csv', 'txt'],
            //filename: 'id',
            //bootstrap: false,
            //exportButtons: true,
            //position: 'bottom',
            //ignoreRows: null,
            //ignoreCols: null,
            //trimWhitespace: true,
            //consoleLog: false,
            //csvEnclosure: '"',
            //csvSeparator: ',',
            //csvUseBOM: true,
            //displayTableName: false,
            //escape: false,
            //excelRTL: false,
            //excelstyles: [],
            //excelFileFormat: 'xlshtml',
            //exportHiddenCells: false,
            //fileName: 'tableExport',
            //htmlContent: false,
            //ignoreColumn: [],
            //ignoreRow: [],
            //jsonScope: 'all',
            numbers: {
                html: { decimalMark: '.', thousandsSeparator: ',' }, output: false
            },
           

            //numbers: {
            //    html: { decimalMark: '.', thousandsSeparator: ',' }, output: { decimalMark: ',', thousandsSeparator: '', output: false }
            //},
            onCellData: null,
            onCellHtmlData: null,
            onIgnoreRow: null,
            onMsoNumberFormat: null
        });
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
        //debugger;
        var table = $scope.LAFReport[0].objMainTable;
        var table1 = $scope.LAFReport[0].objOBTable;
        var table2 = $scope.LAFReport[0].objCBTable;
        var now = new Date();        
        $("#tbldata").empty();
        $("#tbldata").append("<table><thead><tr><td><img src='../Images/inventrax.png' style='height: 56px;' border='0'></td></tr><tr><th>Date</th><th>Description</th><th>Lost</th><th>Found</th><th>Net Position</th></thead><tbody>");
        
if(table1!=null && table1.length > 0)
{
for (var j = 0; j < table1.length; j++) {
            
            $("#tbldata").append("<tr style='text-align:center;'></td><td>" + table1[j].StartDt + "</td><td>Opening Balance (OB)</td><td><span id='oblost'>" + table1[j].LostORFound + "</span></td><td><span id='obfound'><label>" + table1[j].LostORFound + "</span></td><td>" + table1[j].LostORFound + "</td></tr > ");
        }
}
if(table!=null && table.length > 0)
{
        for (var j = 0; j < table.length; j++) {
            var Desc = "";
            if (table[j].Type == "VLPD") {
                Desc = table[j].UserName + " Triggered Material (" + table[j].MCode + ") not found at Location " + table[j].Location + " while picking for [OBD / Internal Transfer/ VLPD](" + table[j].AccountName + ")";
            }
            else {
                if (table[j].LOST > 0)
                {
                    Desc = table[j].MCode + " identified as <span style='color:red !important;'>Lost</span> at " + table[j].Location + " during Cycle Count (" + table[j].AccountName + ")";
                }
                if (table[j].FOUND > 0)
                {
                    Desc = table[j].MCode + " <span style='color:#3cf33c !important;'>Found</span > at " + table[j].Location + " during Cycle Count (" + table[j].AccountName + ")";
                }
            }
            $("#tbldata").append("<tr style='text-align:center;'></td><td>" + table[j].Date + "</td><td> " + Desc + "</td><td>" + table[j].LOST + "</td><td>" + table[j].FOUND + "</td><td>" + table[j].NetPosition + "</td></tr > ");
        }
}
if(table2!=null && table2.length > 0)
{
  
        for (var j = 0; j < table2.length; j++) {            
            $("#tbldata").append("<tr style='text-align:center;'></td><td>" + table2[j].CBEnd + "</td><td>Closing Balance (CB)</td><td><span id='cblost'>" + table2[j].CB + "</span></td><td><span id='cbfound'>" + table2[j].CB + "</span></td><td>" + table2[j].CB + "</td></tr > ");
        }
}
        $("#tbldata").append("</tbody></table>");
        var clost = $("#cblost").text();
        if (clost > 0) {
            $("#cblost").hide();
            $("#cbfound").show();
        }
        else {
            $("#cbfound").hide();
            $("#cblost").show();
        }
        var lost = $("#oblost").text();
        if (lost > 0) {
            $("#oblost").hide();
            $("#obfound").show();
        }
        else {
            $("#obfound").hide();
            $("#oblost").css("display", "block");
        }
    }
});