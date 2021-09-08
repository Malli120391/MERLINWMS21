var rackList = null;
var levelList = null;
var colList = null;
var binListItems = null;

function UnBlockDialog() {
    $.unblockUI();
}

function BlockDialog() {

    $.blockUI({ message: '<h3> Just a moment...</h3>' });
}



$(document).ready(function () {


    LoadLocationRackData();
    LoadLocationLevelData();
    LoadLocationColumnData();
    LoadLocationBinData();

    $('#txtLevelAdd').attr('disabled', 'disabled').val('');
    $('#txtLevelMand').hide(0);
    $('#toggledbtns').on('click', function () {
        $('.toggleBlock').slideToggle();
    });
    $('#ddlColumnOrLevel').change(function () {
        if ($(this).val() == "1") {
            $('#txtColumnAdd').removeAttr('disabled').val('');
            $('#txtColumnMand').show(0);

            $('#txtLevelAdd').attr('disabled', 'disabled').val('');
            $('#txtLevelMand').hide(0);
        }
        else {
            $('#txtLevelAdd').removeAttr('disabled').val('');
            $('#txtLevelMand').show(0);

            $('#txtColumnAdd').attr('disabled', 'disabled').val('');
            $('#txtColumnMand').hide(0);
        }
    });

    $("#alertdialog").dialog({
        autoOpen: false,
        hide: "close",
        draggable: false,
        modal: true,
        width: 500,
        position: {
            my: "center",
            at: "center",
            of: window
        }
    });
    $("#locationcreatedialog").dialog({
        autoOpen: false,
        width: 800,
        height: 500,
        hide: "close",
        modal: true,
        draggable: true,
        resizable: false,
        position: {
            my: "center",
            at: "center",
            of: window
        }
    });
    $("#Addinglocationcreatedialog").dialog({
        autoOpen: false,
        width: 700,
        height: 450,
        hide: "close",
        modal: true,
        draggable: true,
        resizable: false,
        position: {
            my: "center",
            at: "center",
            of: window
        }
    });
    $("#printlocationdialog").dialog({
        autoOpen: false,
        width: 650,
        height: 420,
        hide: "close",
        modal: true,
        draggable: true,
        resizable: false,
        position: {
            my: "center",
            at: "center",
            of: window
        }
    });
    $("#beamdialog").dialog({
        autoOpen: false,
        width: 600,
        height: 330,
        hide: "close",
        modal: true,
        draggable: true,
        position: {
            my: "center",
            at: "center",
            of: window
        }
    });



});




$(document).ready(function () {

    $("#updatebindialog").dialog({
        autoOpen: false,
        hide: "close",
        draggable: true,
        width: 650,
        height: 490,
        modal: true,
        resizable: false
    });
    $("#updatebindialogBulk").dialog({
        autoOpen: false,
        hide: "close",
        draggable: true,
        width: 650,
        height: 500,
        modal: true,
        resizable: false
    });
    $("#createlocalert").dialog({
        autoOpen: false,
        hide: "close",
        draggable: true,
        width: 500,
        height: 150,
        modal: true,
        resizable: false,
        position: {
            my: "center",
            at: "center",
            of: window
        }
    });
    $("#Addinglocalert").dialog({
        autoOpen: false,
        hide: "close",
        draggable: true,
        width: 500,
        height: 150,
        modal: true,
        resizable: false,
        position: {
            my: "center",
            at: "center",
            of: window
        }
    });
    $("#deletealert").dialog({
        autoOpen: false,
        hide: "close",
        draggable: true,
        width: 500,
        height: 200,
        modal: true,
        resizable: false,
        position: {
            my: "center",
            at: "center",
            of: window
        }
    });
    $("#createbeamlocalert").dialog({
        autoOpen: false,
        hide: "close",
        draggable: true,
        width: 500,
        height: 200,
        modal: true,
        resizable: false,
        position: {
            my: "center",
            at: "center",
            of: window
        }
    });
    $("#warningalert").dialog({
        autoOpen: false,
        hide: "close",
        draggable: true,
        width: 500,
        height: 150,
        modal: true,
        resizable: false,
        position: {
            my: "center",
            at: "center",
            of: window
        }
    });

    var textfieldname = $("#txtSearch");
    DropdownFunction(textfieldname);

    textfieldname = $("#txtFixedmaterialcode");
    DropdownFunction(textfieldname);

    var textfieldname = $("#txtSelectTenant");
    DropdownFunction(textfieldname);

    var textfieldname = $("#txtSelectTenantAdd");
    DropdownFunction(textfieldname);

    var textfieldname = $("#txtBayTenant");
    DropdownFunction(textfieldname);

    var textfieldname = $("#txtModifyTenant");
    DropdownFunction(textfieldname);

    $('#txtSearch').bind('keydown', function (e) {
        if (e.keyCode == 13)
            document.getElementById("btnSearch").focus();

        //e.preventDefault();
    });

    $('#ddlRackPrint').append('<option value="0" selected="selected">--SelectRack--</option>');

});
//functionn for inserting locations into database

function createLocations_OLD() {


    var _iAisle = $('#txtAisle').val();
    var _iRack = $('#txtRack').val();
    var _iCol = $('#txtColumn').val();
    var _iLev = $('#txtLevel').val();
    var _iBin = $('#txtBin').val();

    var _iWidth = $('#txtWidthB').val();
    var _iLength = $('#txtLengthB').val();
    var _iHeight = $('#txtHeightB').val();
    var _iMaxWeight = $('#txtMaxWeightB').val();
    var LocType = $('.ddlLocTypeBulkCreate').val();

    var SelectedSuppliers = [];
    $('#txtSupplier :selected').each(function (i, selected) {
        SelectedSuppliers[i] = $(selected).text();
    });

    //           if (document.getElementById('txtSelectTenant').value == "" && SelectedSuppliers != "") {
    //    showStickyToast(false, "Please select the Tenant", false);
    //return false;
    //           }
    if (document.getElementById("txtSelectTenant").value == "") {
        SelectedSuppliers = "";
    }

    var boolIsFastMove;
    if ($('#cbxfastmove').prop('checked')) {
        boolIsFastMove = 1;
    }
    else {
        boolIsFastMove = 0;
    }

    if (ZoneID == null || ZoneID == 0) {
        showStickyToast(false, "Please select Zone", false);
        return false;
    }

    $('#btnCreateLocations, #btnCancelCreateLocations').hide();

    $.ajax({
        type: "POST",
        //url: "LocationHandlers/InsertLocationHandler.ashx?bayfrom=" + bayfrom + "&bayto=" + bayto + "&beamfrom=" + beamfrom + "&beamto=" + beamto + "&locfrom=" + locfrom + "&locto=" + locto + "&zone=" + Zonecode + "&Tenant=" + document.getElementById("txtSelectTenant").value + "&Supplier=" + SelectedSuppliers + "&IsFastMoving=" + boolIsFastMove + "&aisle=" + aisle,
        url: "LocationHandlers/InsertLocationHandler.ashx?aisle=" + _iAisle + "&rack=" + _iRack + "&column=" + _iCol + "&level=" + _iLev + "&bin=" + _iBin + "&zone=" + ZoneCode + "&Tenant=" + document.getElementById("txtSelectTenant").value + "&Supplier=" + SelectedSuppliers + "&IsFastMoving=" + boolIsFastMove + "&length=" + _iLength + "&width=" + _iWidth + "&height=" + _iHeight + "&maxWeight=" + _iMaxWeight + "&ZoneID=" + ZoneID + "&LocType=" + LocType,
        contentType: "text/html",
        success: function (response) {
            if (response != "1") {
                showStickyToast(false, "Error:" + response, false);
                CloseDialog(alertdialog);
                return false;
            } else {
                showStickyToast(true, "Location(s) Created Successfully", false);
                CloseDialog(printlocationdialog);
                CloseDialog(createlocalert);
                CloseDialog(locationcreatedialog);
                // getLocations();
                LoadMap();
            }

        },
        error: function (response) {
            showStickyToast(false, response, false);
        },
        complete: function () {
            $('#btnCreateLocations, #btnCancelCreateLocations').show();
        }
    });
}
function InsertLocations_OLD() {

    var _iAisle = $('#txtAisle').val();
    var _iRack = $('#txtRack').val();
    var _iCol = $('#txtColumn').val();
    var _iLev = $('#txtLevel').val();
    var _iBin = $('#txtBin').val();

    var _iWidth = $('#txtWidthB').val();
    var _iLength = $('#txtLengthB').val();
    var _iHeight = $('#txtHeightB').val();
    var _iMaxWeight = $('#txtMaxWeightB').val();
    var LocType = $('.ddlLocTypeBulkCreate').val();

    if (_iAisle == "" || _iAisle == 0 || _iRack == "" || _iRack == 0 || _iCol == "" || _iCol == "" || _iLev == 0 || _iBin == "" || _iBin == 0) {
        showStickyToast(false, "Please enter Mandatory fields", false);
        return false;
    }
    //if (document.getElementById('txtSelectTenant').value == "" && SelectedSuppliers != "") {
    //    showStickyToast(false, "Please select the Tenant", false);
    //    return false;
    //}

    if (_iWidth == "" || _iWidth == 0 || _iLength == "" || _iLength == 0 || _iHeight == "" || _iHeight == 0 || _iMaxWeight == "" || _iMaxWeight == 0) {
        showStickyToast(false, "Please enter Dimension fields", false);
        return false;
    }



    var SelectedSuppliers = [];
    $('#txtSupplier :selected').each(function (i, selected) {
        SelectedSuppliers[i] = $(selected).text();
    });




    //var numberofbays = bayto - bayfrom + 1;

    //var numberofbeams = beamto - beamfrom + 1;

    //var numberoflocations = locto - locfrom + 1;
    var totallocations;
    // totallocations = numberofbays * numberofbeams * numberoflocations;
    totallocations = _iAisle * _iRack * _iLev * _iCol * _iBin;

    document.getElementById("createlocalertspan").innerHTML = " Do you want to create a total of <b><font color='red'>" + totallocations + " </b></font> Location(s)?<br> ";
    $("#createlocalert").dialog("option", "title", "Confirmation");
    $("#createlocalert").dialog("open");
}




//============================ Added New Methods By M.D.Prasad ON 08-Nov-2019 ==============================//

function createLocations() {
    var zoneCode = $('#MainContent_MMContent_ddlLocationCode option:selected').text();
    var fromRackID = $('#fromRackID').val();
    var toRackID = $('#toRackID').val();
    var fromLevelID = $('#fromLevelID').val();
    var toLevelID = $('#toLevelID').val();

    var fromColID = $('#fromColumnID').val();
    var toColID = $('#toColumnID').val();
    var fromBinID = $('#fromBinID').val();
    var toBinID = $('#toBinID').val();

    var _iWidth = $('#txtWidthB').val();
    var _iLength = $('#txtLengthB').val();
    var _iHeight = $('#txtHeightB').val();
    var _iMaxWeight = $('#txtMaxWeightB').val();


    var LocType = $('.ddlLocTypeBulkCreate').val();

    var SelectedSuppliers = [];
    $('#txtSupplier :selected').each(function (i, selected) {
        SelectedSuppliers[i] = $(selected).text();
    });

    if (document.getElementById("txtSelectTenant").value == "") {
        SelectedSuppliers = "";
    }

    var boolIsFastMove;
    if ($('#cbxfastmove').prop('checked')) {
        boolIsFastMove = 1;
    }
    else {
        boolIsFastMove = 0;
    }

    if (ZoneID == null || ZoneID == 0) {
        showStickyToast(false, "Please select Zone", false);
        return false;
    }

    var locationFormatData = new LocationCreateRequest(null, zoneCode, fromRackID, toRackID, fromLevelID, toLevelID, fromColID, toColID, fromBinID, toBinID, SelectedSuppliers, $('#MainContent_MMContent_ddlWarehouse option:selected').text(), document.getElementById("hifTenant").value, boolIsFastMove, _iWidth, _iHeight, _iLength, _iMaxWeight, LocType, ZoneID, document.getElementById("txtSelectTenant").value, rackList, levelList, colList, binListItems);

    $('#btnCreateLocations, #btnCancelCreateLocations').hide();
    var dataObj = "{ data:  " + JSON.stringify(locationFormatData) + " }";
    $.ajax({
        url: "LocationManager.aspx/CreateLocation",
        dataType: 'json',
        contentType: "application/json",
        type: 'POST',
        data: dataObj,
        success: function (response) {
            debugger;
            if (response.d != "1") {
                showStickyToast(false, "Error:" + response.d, false);
                CloseDialog(alertdialog);
                return false;
            } else {
                showStickyToast(true, "Location(s) Created Successfully", false);
                CloseDialog(printlocationdialog);
                CloseDialog(createlocalert);
                CloseDialog(locationcreatedialog);
                // getLocations();
                LoadMap();
            }
        },
        error: function (response) {
            showStickyToast(false, response, false);
        },
        complete: function () {
            $('#btnCreateLocations, #btnCancelCreateLocations').show();
        }
    });
}

function InsertLocations() {

    var _zoneCode = $('#MainContent_MMContent_ddlLocationCode option:selected').text();
    var fromRackID = $('#fromRackID').val();
    var toRackID = $('#toRackID').val();
    var fromLevelID = $('#fromLevelID').val();
    var toLevelID = $('#toLevelID').val();

    var fromColID = $('#fromColumnID').val();
    var toColID = $('#toColumnID').val();
    var fromBinID = $('#fromBinID').val();
    var toBinID = $('#toBinID').val();

    var _iWidth = $('#txtWidthB').val();
    var _iLength = $('#txtLengthB').val();
    var _iHeight = $('#txtHeightB').val();
    var _iMaxWeight = $('#txtMaxWeightB').val();
    var LocType = $('.ddlLocTypeBulkCreate').val();

    if (fromRackID == "0" || toRackID == "0" || fromLevelID == "0" || toLevelID == "0" || fromColID == "0" || toColID == "0" || fromBinID == "0" || toBinID == "0") {
        showStickyToast(false, "Please enter Mandatory fields", false);
        return false;
    }

    if (_iWidth == "" || _iWidth == 0 || _iLength == "" || _iLength == 0 || _iHeight == "" || _iHeight == 0 || _iMaxWeight == "" || _iMaxWeight == 0) {
        showStickyToast(false, "Please enter Dimension fields", false);
        return false;
    }



    var SelectedSuppliers = [];
    $('#txtSupplier :selected').each(function (i, selected) {
        SelectedSuppliers[i] = $(selected).text();
    });

    var totallocations = "";
    //totallocations = _iAisle * _iRack * _iLev * _iCol * _iBin;

    document.getElementById("createlocalertspan").innerHTML = " Do you want to create a total of <b><font color='red'>" + totallocations + " </b></font> Location(s)?<br> ";
    $("#createlocalert").dialog("option", "title", "Confirmation");
    $("#createlocalert").dialog("open");
}

function LocationCreateRequest(aisleName, zoneCode, fromRackID, toRackID, fromLevelID, toLevelID, fromColID, toColID, fromBinID, toBinID, supData, WhCode, tenantID, isFastMoving, width, height, length, weight, locType, zoneID, tenantName, rackList, levelList, colList, binListItems) {
    debugger
    this.PhaseName = zoneCode;
    this.AisleName = "";
    this.RackFrom = fromRackID;
    this.RackTo = toRackID;
    this.ColumnFrom = fromColID;
    this.ColumnTo = toColID;
    this.LevelFrom = fromLevelID;
    this.LevelTo = toLevelID;
    this.BinFrom = fromBinID;
    this.BinTo = toBinID;
    this.IsFastMoving = "" + isFastMoving;
    this.TenantID = tenantID;
    this.SupList = supData.length == 0 ? "" : supData;
    this.WhCode = WhCode;

    this.Width = width;
    this.Height = height;
    this.Length = length;
    this.Weight = weight;
    this.LocationType = locType;
    this.ZoneID = zoneID;
    this.TenantName = tenantName;

    this.RackList = rackList;
    this.ColumnList = colList;
    this.LevelList = levelList;
    this.BinList = binListItems;
}

// =========================== END ============================//




//Printing Alert
function InitatePrint() {

    debugger;

    var Totalrack = $('#ddlRackPrint option:selected').val();
    if (Totalrack == 0 || Totalrack == null || Totalrack == undefined) {
        showStickyToast(false, "Please select atleast one rack");
        return false;
    }

    totallocations = "Rack " + $('#ddlRackPrint option:selected').val();
    //document.getElementById("printlocalertspan").title = "Confirmation";
    //document.getElementById("printlocalertspan").innerHTML = " Do you want to Print <b><font color='red'>" + totallocations + " </b></font> Location(s)?<br> ";

    var $confirm = $("#modalConfirmYesNo");
    $confirm.modal('show');
    // $("#lblTitleConfirmYesNo").html("<i class='fa fa-exclamation-circle' aria-hidden='true'></i> Confirmation");
    $("#lblMsgConfirmYesNo").html("Do you want to Print <b><font color='red'>" + totallocations + " </b></font> Location(s)?<br>");

    $("#btnYesConfirmYesNo").on('click').click(function () {
        PrintBulkLocations();
    });
    $("#btnNoConfirmYesNo").on('click').click(function () {
        $confirm.modal("hide");
    });

    //$("#printlocaalert").dialog("open");   
    //$("#printlocaalert").attr("title", "Confirmation");
    //$("#printlocaalert").dialog({
    //    buttons: [
    //        {
    //            text: "OK",
    //            "click": function () {
    //                PrintBulkLocations();
    //            }
    //        },
    //        {
    //            text: "Cancel",
    //            "click": function () {
    //                ClosePrintDialog("printlocaalert");
    //            }
    //        }
    //    ],
    //    focus: function (i, j) {
    //        $('#printlocaalert').siblings('.ui-dialog-buttonpane').find('button.ui-widget').css('position', "initial");
    //    },
    //    create: function (i, j) {
    //        $('#printlocaalert').siblings('.ui-dialog-buttonpane').find('button.ui-widget').css('position', "initial");
    //    },
    //    close: function (i, j) {
    //        $("#printlocaalert").dialog("destroy");
    //    }
    //});

}

///Adding Locations



function AddingLocations() {

    debugger;

    var _iRack = $('.ddlRack option:selected').val();
    var _iCol = $('#txtColumnAdd').val();
    var _iLev = $('#txtLevelAdd').val();
    var _iBin = $('#txtBinAdd').val();

    var _iWidth = $('#txtWidthAdd').val();
    var _iLength = $('#txtLengthAdd').val();
    var _iHeight = $('#txtHeightAdd').val();
    var _iMaxWeight = $('#txtMaxWeightAdd').val();
    var _iLocType = $('.ddlLocTypeAdd').val();
    ZoneCode = $('#MainContent_MMContent_ddlLocationCode option:selected').text();
    var SelectedSuppliers = [];
    $('#txtSupplierAdd :selected').each(function (i, selected) {
        SelectedSuppliers[i] = $(selected).text();
    });

    if (document.getElementById('txtSelectTenantAdd').value == "" && SelectedSuppliers != "") {
        showStickyToast(false, "Please select the Tenant", false);
        return false;
    }
    if (document.getElementById("txtSelectTenantAdd").value == "") {
        SelectedSuppliers = "";
    }

    var boolIsFastMove;
    if ($('#cbxfastmoveAdd').prop('checked')) {
        boolIsFastMove = 1;
    }
    else {
        boolIsFastMove = 0;
    }

    $.ajax({
        type: "POST",

        //url: "LocationHandlers/AddingLocationHandler.ashx?rack=" + _iRack + "&column=" + _iCol + "&level=" + _iLev + "&bin=" + _iBin + "&zone=" + Zonecode + "&Tenant=" + document.getElementById("txtSelectTenantAdd").value + "&Supplier=" + SelectedSuppliers + "&IsFastMoving=" + boolIsFastMove + "&length=" + _iLength + "&width=" + _iWidth + "&height=" + _iHeight + "&maxWeight=" + _iMaxWeight,
        url: "LocationTree.aspx/AddingLocations",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: "{'rack': '" + _iRack + "' , 'column':  '" + _iCol + "', 'level': '" + _iLev + "', 'bin':  '" + _iBin + "', 'zone': '" + ZoneCode + "', 'Tenant':  '" + document.getElementById("txtSelectTenantAdd").value + "', 'Supplier': '" + SelectedSuppliers + "' ,'IsFastMoving':  " + boolIsFastMove + ", 'length': " + _iLength + " ,'width':  " + _iWidth + ", 'height': " + _iHeight + " ,'maxWeight':  " + _iMaxWeight + " ,'ZoneID':  " + ZoneID + ",'LocType':  " + _iLocType + "}",
        success: function (response) {
            debugger;
            if (response.d != "1") {
                // showStickyToast(false, "Location(s) already exists", false);
                showStickyToast(false, "Error:" + response, false);
                CloseDialog(alertdialog);
                return false;
            } else {
                CloseDialog(printlocationdialog);
                CloseDialog(Addinglocalert);
                CloseDialog(Addinglocationcreatedialog);
                // getLocations();
                LoadMap();
                showStickyToast(true, "Locations Added Successfully", false);

            }

        },
        error: function (response) {
            showStickyToast(false, "Error", false);
        }
    });
}

function AddingInsertLocations() {

    var _iRack = $('.ddlRack option:selected').val();
    var _iCol = $('#txtColumnAdd').val();
    var _iLev = $('#txtLevelAdd').val();
    var _iBin = $('#txtBinAdd').val();

    var _iWidth = $('#txtWidthAdd').val();
    var _iLength = $('#txtLengthAdd').val();
    var _iHeight = $('#txtHeightAdd').val();
    var _iMaxWeight = $('#txtMaxWeightAdd').val();

    var _iColorLev = $('#ddlColumnOrLevel').val();



    if (_iRack == "" || _iRack == 0) {
        showStickyToast(false, "Please select Rack", false);
        return false;
    }

    if (_iColorLev == 1) {
        if (_iCol == "" || _iCol == 0) {
            showStickyToast(false, "Please enter No.of Columns Required", false);
            return false;
        }
        if (_iLev == "") {
            _iLev = 0;
        }
    }
    if (_iColorLev == 2) {
        if (_iLev == "" || _iLev == 0) {
            showStickyToast(false, "Please enter No.of Levels Required", false);
            return false;
        }
        if (_iCol == '') {
            _iCol = 0;
        }
    }


    if (_iBin == "" || _iBin == 0) {
        showStickyToast(false, "Please enter No.of Locations Required in each Bin", false);
        return false;
    }

    //if (document.getElementById('txtSelectTenantAdd').value == "" && SelectedSuppliers != "") {
    //    showStickyToast(false, "Please select Tenant", false);
    //    return false;
    //}

    if (_iWidth == "" || _iWidth == 0 || _iLength == "" || _iLength == 0 || _iHeight == "" || _iHeight == 0 || _iMaxWeight == "" || _iMaxWeight == 0) {
        showStickyToast(false, "Please enter Dimension fields", false);
        return false;
    }

    var SelectedSuppliers = [];
    $('#txtSupplierAdd :selected').each(function (i, selected) {
        SelectedSuppliers[i] = $(selected).text();
    });





    var totallocations = _iLev * _iCol * _iBin;
    if (_iCol == 0)
        totallocations = _iLev + " Levels";
    else
        totallocations = _iCol + " Columns";

    document.getElementById("Addinglocalertspan").innerHTML = " Do you want to create a total of <b><font color='red'>" + totallocations + " </b></font> Location(s)?<br> ";
    $("#Addinglocalert").dialog("option", "title", "Confirmation");
    $("#Addinglocalert").dialog("open");
}


///END Adding Locations






//For Printing Bulk locations
//   function PrintBulkLocations() {

//            BlockDialog();
//        var Rack = $('#ddlRackPrint').val();
//       var Column = $('#ddlColumnPrint').val();
//       var Level = $('#ddlLevelPrint').val();
//       var Bin = $('#ddlBinPrint').val();
//       var printerIP = $('#MainContent_MMContent_ddlPrinter').val();

//    $.ajax({
//            type: "POST",
//        //url: "LocationHandlers/PrintLocationHandler.ashx?Zone=" + ZoneCode + "&Rack=" + Rack + "&Column=" + Column + "&Level=" + Level + "&Bin=" + Bin + "&PrinterIP=" + printerIP + "&aisle=" + aisle + "&ZoneID=" + ZoneID,
//        url: "LocationHandlers/PrintLocationHandler.ashx?Zone=" + ZoneCode + "&Rack=" + Rack + "&Column=" + Column + "&Level=" + Level + "&Bin=" + Bin + "&PrinterIP=" + printerIP + "&ZoneID=" + ZoneID,
//        contentType: "text/html",
//        success: function (response) {
//            debugger;
//            var dt = response.d;
//            if (response == "1") {
//            showStickyToast(true, "Successfully Printed");
//        //CloseDialog(alertdialog);
//        ClosePrintDialog("printlocaalert");
//                UnBlockDialog();
//                return false;
//            }
//        },
//        error: function (response) {
//            showStickyToast(false, "Error", false);
//        UnBlockDialog();
//        }
//    });
//}


function PrintBulkLocations() {
    debugger;
    BlockDialog();
    var Rack = $('#ddlRackPrint').val();
    var Column = $('#ddlColumnPrint').val();
    var Level = $('#ddlLevelPrint').val();
    var Bin = $('#ddlBinPrint').val();
    //var printerIP = $('#MainContent_MMContent_ddlPrinter').val();

    if ($('#pid').val() == "3") {
        if ($("#netPrinterHost").val() == "") {
            showStickyToast(false, "Please enter Printer IP Address", false);
            return false;
        }
        if ($("#netPrinterPort").val() == "") {
            showStickyToast(false, "Please enter Printer Port", false);
            return false;
        }
    }

    $.ajax({
        type: "POST",
        //  01 version 
        // url: "LocationHandlers/PrintLocationHandler.ashx?Zone=" + ZoneCode + "&Rack=" + Rack + "&Column=" + Column + "&Level=" + Level + "&Bin=" + Bin + "&PrinterIP=" + printerIP + "&aisle=" + aisle + "&ZoneID=" + ZoneID,
        url: "LocationHandlers/PrintLocationHandler.ashx?Zone=" + ZoneCode + "&Rack=" + Rack + "&Column=" + Column + "&Level=" + Level + "&Bin=" + Bin + "&PrinterIP=" + 0 + "&ZoneID=" + ZoneID,
        contentType: "text/html",
        success: function (response) {
            debugger;
            var dt = response;

            if (dt == "") {
                showStickyToast(false, "Error occured", false);
                ClosePrintDialog("printlocaalert");
                UnBlockDialog();
                return false;
            }
            else {
                $("#printerCommands").val("");
                $("#printerCommands").val(dt);
                javascript: doClientPrint();
                showStickyToast(true, "Successfully Printed", false);
                ClosePrintDialog("printlocaalert");
                UnBlockDialog();
            }



            //if (response != "") {
            //    /*//Commeneted by M.D Prasad on 2-1-2019 // $("#entered_name").val("");
            //    $("#entered_name").val(dt);
            //    sendData();*/


            //    showStickyToast(true, "Successfully Printed");
            //    //CloseDialog(alertdialog);
            //    ClosePrintDialog("printlocaalert");
            //    UnBlockDialog();
            //    return false;
            //}
            //else
            //{
            //    showStickyToast(false, "Error while printing", false);
            //    return false;
            //}
        },
        error: function (response) {
            debugger;
            showStickyToast(false, "Error", false);
            UnBlockDialog();
        }
    });
}















function InsertBeamLocations() {

    var SelectedSuppliers = [];
    $('#txtBaySupplier :selected').each(function (i, selected) {
        SelectedSuppliers[i] = $(selected).text();
    });

    if (document.getElementById('txtBayTenant').value == "" && SelectedSuppliers != "") {
        alert('Please select the Tenant');
        return false;
    }
    beam = beamcode;
    var totallocations = _iAisle * _iRack * _iLev * _iRack;// beamlocto - beamlocfrom + 1;
    document.getElementById("createbeamlocalertspan").innerHTML = " Do you want to create a total of <b><font color='red'>" + totallocations + " </b></font> Location(s) in Beam " + beam + "?<br> ";
    $("#createbeamlocalert").dialog("option", "title", "Confirmation");
    /*$("#alertdialog").dialog({
                buttons: [
            {
                text: "Cancel", click: function () {
                CloseDialog(alertdialog);

            }
        }, {
                text: "Ok", click: function () {
                $.ajax({
                    type: "POST",
                    url: "LocationHandlers/InsertLocationHandler.ashx?beamcode=" + beam + "&beamlocfrom=" + beamlocfrom + "&beamlocto=" + beamlocto,
                    contentType: "text/html",
                    success: function (response) {
                        if (response != "1") {
                            alert("Locations Already Existed");
                            CloseDialog(alertdialog);
                            return false;
                        } else {
                            CloseDialog(beamdialog);
                            getLocations();
                            CloseDialog(alertdialog);
                        }

                    },
                    error: function (response) {
                        alert('error');
                    }
                });
            }
        }]
    });*/
    $("#createbeamlocalert").dialog("open");
}
function CreateBeamLocations() {
    var SelectedSuppliers = [];
    $('#txtBaySupplier :selected').each(function (i, selected) {
        SelectedSuppliers[i] = $(selected).text();
    });

    if (document.getElementById('txtBayTenant').value == "" && SelectedSuppliers != "") {
        alert('Please select the Tenant');
        return false;
    }

    if (document.getElementById("txtBayTenant").value == "") {
        SelectedSuppliers = "";
    }

    $.ajax({
        type: "POST",
        url: "LocationHandlers/InsertLocationHandler.ashx?beamcode=" + beam + "&beamlocfrom=" + beamlocfrom + "&beamlocto=" + beamlocto + "&Tenant=" + document.getElementById("txtBayTenant").value + "&Supplier=" + SelectedSuppliers + "&aisle=" + aisle + "&ZoneID=" + ZoneID,
        contentType: "text/html",
        success: function (response) {
            if (response == "0") {
                //alert("Locations already exists");
                alert("Error while Inserting the Locations");
                CloseDialog(alertdialog);
                return false;
            } else {
                CloseDialog(beamdialog);
                //getLocations();
                //GetLocations();
                LoadMap();
                CloseDialog(createbeamlocalert);
            }

        },
        error: function (response) {
            alert('error');
        }
    });
}

function LoadRacks(ZoneID) {

    // $('#ddlRackPrint').append("<option value='0'>--SelectRack--</option>");

    $.ajax(
        {
            url: "LocationTree.aspx/GetRacks",
            dataType: "json",
            type: "POST",
            data: "{'ZoneID':'" + ZoneID + "'}",
            async: true,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                debugger;
                $('.ddlRack, .ddlRackBulk').empty();
                var obj = JSON.parse(data.d);
                $('.ddlRackBulk').append("<option value='0'>--Select--</option>");

                $('#ddlRack').append("<option value='0'>--Select--</option>");
                for (var i = 0; i < obj.length; i++) {
                    $('.ddlRack, .ddlRackBulk').append("<option value=" + obj[i].Rack + ">" + obj[i].Rack + "</option>");
                }

            },
            error: function (response) {
                showStickyToast(false, "Error, Please contact 'Inventrax Admin'. " + response.responseText, false);
            },
            failure: function (response) {
                showStickyToast(false, "Error, Please contact 'Inventrax Admin'. " + response.responseText, false);
            }

        });
}


function ddlRackPrint_Change() {
    $('#ddlLevelPrint,#ddlColumnPrint,#ddlBinPrint').empty();
    $('#ddlLevelPrint,#ddlColumnPrint,#ddlBinPrint').append("<option value='0'>All</option>");

    var RackCode = $('#ddlRackPrint').val();

    $.ajax({
        url: "LocationTree.aspx/GetColumnAndLevel",
        dataType: "json",
        type: "POST",
        data: "{'ZoneID':'" + ZoneID + "','rackCode':'" + RackCode + "'}",
        async: true,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var obj = JSON.parse(data.d);
            for (var i = 0; i < obj.Table.length; i++) {
                $('#ddlColumnPrint').append("<option value=" + obj.Table[i].Column + ">" + obj.Table[i].Column + "</option>");
            }


            for (var i = 0; i < obj.Table1.length; i++) {
                $('#ddlLevelPrint').append("<option value=" + obj.Table1[i].Level + ">" + obj.Table1[i].Level + "</option>");
            }

        },
        error: function (response) {
            showStickyToast(false, "Error, Please contact 'Inventrax Admin'. " + response.responseText, false);
        },
        failure: function (response) {
            showStickyToast(false, "Error, Please contact 'Inventrax Admin'. " + response.responseText, false);
        }



    });
}

function ddlColLevPrint_Change() {
    var RackCode = $('#ddlRackPrint').val();
    var ColCode = $('#ddlColumnPrint').val();
    var LevCode = $('#ddlLevelPrint').val();

    $.ajax({
        url: "LocationTree.aspx/GetBins",
        dataType: "json",
        type: "POST",
        data: "{'ZoneID':'" + ZoneID + "','rackCode':'" + RackCode + "','ColCode':'" + ColCode + "','LevCode':'" + LevCode + "'}",
        async: false,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var obj = JSON.parse(data.d);
            $('#ddlBinPrint').empty();
            $('#ddlBinPrint').append("<option value='0'>All</option>");
            for (var i = 0; i < obj.length; i++) {
                $('#ddlBinPrint').append("<option value=" + obj[i].Bin + ">" + obj[i].Bin + "</option>");
            }
        },
        error: function (response) {
            showStickyToast(false, "Error, Please contact 'Inventrax Admin'. " + response.responseText, false);
        },
        failure: function (response) {
            showStickyToast(false, "Error, Please contact 'Inventrax Admin'. " + response.responseText, false);
        }
    });
}
function LoadRacksForPrint(ZoneID) {
    $.ajax({
        url: "LocationTree.aspx/GetRacks",
        dataType: "json",
        type: "POST",
        data: "{'ZoneID':'" + ZoneID + "'}",
        async: true,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            debugger;
            $('.ddlRackPrint').empty();
            var obj = JSON.parse(data.d);
            $('#ddlRackPrint').append("<option value='0'>--Select--</option>");
            for (var i = 0; i < obj.length; i++) {
                $('#ddlRackPrint').append("<option value=" + obj[i].Rack + ">" + obj[i].Rack + "</option>");
            }

        },
        error: function (response) {
            showStickyToast(false, "Error, Please contact 'Inventrax Admin'. " + response.responseText, false);
        },
        failure: function (response) {
            showStickyToast(false, "Error, Please contact 'Inventrax Admin'. " + response.responseText, false);
        }

    });
}

function ddlRackBulk_Change() {
    $('#ddlLevelBulk,#ddlColumnBulk,#ddlBinBulk').empty();
    $('#ddlLevelBulk,#ddlColumnBulk,#ddlBinBulk').append("<option value='0'>All</option>");
    var RackCode = $('#ddlRackBulk').val();





    $.ajax({
        url: "LocationTree.aspx/GetColumnAndLevel",
        dataType: "json",
        type: "POST",
        data: "{'ZoneID':'" + ZoneID + "','rackCode':'" + RackCode + "'}",
        async: true,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var obj = JSON.parse(data.d);
            for (var i = 0; i < obj.Table.length; i++) {
                $('#ddlColumnBulk').append("<option value=" + obj.Table[i].Column + ">" + obj.Table[i].Column + "</option>");
            }

            for (var i = 0; i < obj.Table1.length; i++) {
                $('#ddlLevelBulk').append("<option value=" + obj.Table1[i].Level + ">" + obj.Table1[i].Level + "</option>");
            }

        },
        error: function (response) {
            showStickyToast(false, "Error, Please contact 'Inventrax Admin'. " + response.responseText, false);
        },
        failure: function (response) {
            showStickyToast(false, "Error, Please contact 'Inventrax Admin'. " + response.responseText, false);
        }



    });
}




//autocompleting material data in searchbox
function getMaterialAutoComplete(vTenantID) {

    try {

        $('.mcodepicker').autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "LocationHandlers/GetMMCodehandler.ashx?prefix=" + request.term + "&choice=mcode" + "&Tenant=" + vTenantID,
                    dataType: "json",
                    type: "POST",
                    async: true,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data, function (item) {
                            return {
                                label: item.split('~')[0].split('`')[0],
                                description: item.split('~')[0].split('`')[1] == undefined ? "" : " <font color='#f47f4a'  >" + item.split('~')[0].split('`')[1] + "</font>",
                                val: item.split('~')[1]
                            }
                        }))

                    },
                    error: function (response) {
                        showStickyToast(false, "Error, Please contact 'Inventrax Admin'. " + response.responseText, false);
                    },
                    failure: function (response) {
                        showStickyToast(false, "Error, Please contact 'Inventrax Admin'. " + response.responseText, false);
                    }

                });
            },
            select: function (e, i) {
                //alert();
                document.getElementById("hidTenantID").value = i.item.val;
            },
            minLength: 0,

        }).data("autocomplete")._renderItem = function (ul, item) {
            // Inside of _renderItem you can use any property that exists on each item that we built
            // with $.map above */
            return $("<li></li>")
                .data("item.autocomplete", item)
                .append("<a>" + item.label + "" + (item.description == null ? '' : item.description) + "</a>")
                .appendTo(ul)
        };
    } catch (ex) { }

}
//auto completing deptdata in searchbox
function getTenantAutoComplete(vTenantID, vWarehouseID) {
    debugger;
    $('.mcodepicker').autocomplete({
        source: function (request, response) {

            $.ajax({
                url: "LocationHandlers/GetMMCodehandler.ashx?prefix=" + request.term + "&choice=tenentdata" + "&Tenant=" + vTenantID + "&WarehouseID=" + vWarehouseID,
                dataType: "json",
                type: "POST",
                async: true,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.split(',')[0],
                            val: item.split(',')[1]
                        }
                    }))

                },
                error: function (response) {
                    showStickyToast(false, "Error, Please contact 'Inventrax Admin'. " + response.responseText, false);
                },
                failure: function (response) {
                    showStickyToast(false, "Error, Please contact 'Inventrax Admin'. " + response.responseText, false);
                }

            });
        },
        select: function (e, i) {
            //alert();
            document.getElementById("hidTenantID").value = i.item.val;

        },
        minLength: 0
    });
}

function getTenantForMultipleSupplier(value) {

    $('.TenantListboxPicker').autocomplete({

        source: function (request, response) {
            debugger;
            var WHID = $("#MainContent_MMContent_ddlWarehouse").attr("value");
            $.ajax({
                url: "LocationHandlers/GetMMCodehandler.ashx?prefix=" + request.term + "&choice=tenentdata" + "&Tenant=0&WarehouseID=" + WHID + "",
                dataType: "json",
                type: "POST",
                async: true,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.split(',')[0],
                            val: item.split(',')[1]

                        }
                    }))

                },
                error: function (response) {
                    showStickyToast(false, "Error, Please contact 'Inventrax Admin'. " + response.responseText, false);
                },
                failure: function (response) {
                    showStickyToast(false, "Error, Please contact 'Inventrax Admin'. " + response.responseText, false);
                }

            });
        },
        select: function (e, i) {

            debugger;
            $("#hifTenant").val(i.item.val);
            $("#hifTenantAdd").val(i.item.val);
            $("#hidTenantIDBulk").val(i.item.val);

            if (value == 'Create') {
                $("#txtSelectTenant").val(i.item.label);
                document.getElementById("trSupplier").style.display = "table-row";
            }
            else if (value == '+') {
                $("#txtSelectTenantAdd").val(i.item.label);
                document.getElementById("trSupplierAdd").style.display = "table-row";
            }

            else if (value == 'Modify') {
                $("#txtModifyTenant").val(i.item.label);

            }
            else if (value == 'ModifyBulk') {
                $("#txtModifyTenantBulk").val(i.item.label);

            }
            else if (value == 'Bay') {
                $("#txtBayTenant").val(i.item.label);
                document.getElementById("trBaySupplier").style.display = "table-row";
            }
            $('#txtModifySupplier').empty();
            $('#txtModifySupplierBulk').empty();
            $('#txtSupplier').empty();
            $('#txtBaySupplier').empty();

            getListboxList(value);
        },
        minLength: 0
    });
}




function getListboxList(value) {

    $('#txtModifySupplier, #txtModifySupplierBulk').empty();
    $('#txtSupplier').empty();
    $('#txtBaySupplier').empty();

    if (value == 'Modify') {
        Tenant = document.getElementById('txtModifyTenant').value;
    }

    else if (value == 'Create') {
        Tenant = document.getElementById('txtSelectTenant').value;
    }

    else if (value == '+') {
        Tenant = document.getElementById('txtSelectTenantAdd').value;
    }

    else if (value == 'Bay') {
        Tenant = document.getElementById('txtBayTenant').value;
    }
    else if (value == 'ModifyBulk') {
        Tenant = document.getElementById('txtModifyTenantBulk').value;
    }

    $.ajax({
        url: "LocationHandlers/GetMMCodehandler.ashx?prefix=0&choice=SupplierData" + "&Tenant=" + Tenant,
        dataType: "json",
        type: "POST",
        async: true,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $.map(data, function (item) {
                if (value == 'Modify') {
                    $('#txtModifySupplier').append(
                        $('<option></option>').html(item.split(',')[0])
                    );
                }
                else if (value == 'Create') {
                    $('#txtSupplier').append(
                        $('<option></option>').html(item.split(',')[0])
                    );
                }
                else if (value == '+') {
                    $('#txtSupplierAdd').append(
                        $('<option></option>').html(item.split(',')[0])
                    );
                }
                else if (value == 'Bay') {
                    $('#txtBaySupplier').append(
                        $('<option></option>').html(item.split(',')[0])
                    );
                }
                else if (value == 'ModifyBulk') {
                    $('#txtModifySupplierBulk').append(
                        $('<option></option>').html(item.split(',')[0])
                    );
                }

            });
        },
        error: function (request, error) {
            showStickyToast(false, "Request: " + JSON.stringify(request), false);
        }
    });
}
function getTenantList() {


    $("#hifTenant").val("");
    $('.MMCodepicker').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "LocationHandlers/GetMMCodehandler.ashx?prefix=" + request.term + "&choice=tenentdata" + "&Tenant=0",
                dataType: "json",
                type: "POST",
                async: true,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.split(',')[0],
                            val: item.split(',')[1]
                        }
                    }))

                },
                error: function (response) {
                    showStickyToast(false, "Error, Please contact 'Inventrax Admin'. " + response.responseText, false);
                },
                failure: function (response) {
                    showStickyToast(false, "Error, Please contact 'Inventrax Admin'. " + response.responseText, false);
                }

            });
        },
        select: function (e, i) {

            $('#hifTenant').val(i.item.val);
            $("#txtSupplier").val("");
            $("#txtModifySupplier,#txtModifySupplierBulk").val("");
            $("#txtBaySupplier").val("");

        },
        minLength: 0
    });
}


function getSupplierList(Val) {


    $("#hifSupplierID").val("");
    $('.SupplierPicker').autocomplete({
        source: function (request, response) {

            var Tenant = "";
            if (Val == 1) {
                if (document.getElementById('txtSelectTenant').value == "") {
                    showStickyToast(false, "Please select Tenant ", false);
                    return false;
                }
                Tenant = document.getElementById('txtSelectTenant').value;
            }

            else if (Val == 2) {
                if (document.getElementById('txtModifyTenant').value == "") {
                    showStickyToast(false, "Please select Tenant ", false);
                    return false;
                }
                Tenant = document.getElementById('txtModifyTenant').value;
            }

            else if (Val == 3) {
                if (document.getElementById('txtBayTenant').value == "") {
                    showStickyToast(false, "Please select Tenant ", false);
                    return false;
                }
                Tenant = document.getElementById('txtBayTenant').value;
            }

            $.ajax({
                url: "LocationHandlers/GetMMCodehandler.ashx?prefix=" + request.term + "&choice=SupplierData" + "&Tenant=" + Tenant.toString(),
                dataType: "json",
                type: "POST",
                async: true,
                contentType: "application/json; charset=utf-8",
                success: function (data) {

                    response($.map(data, function (item) {
                        debugger
                        return {
                            label: item.split(',')[0],
                            val: item.split(',')[1]
                        }
                    }))


                },
                error: function (response) {
                    alert("Error, Please contact 'Inventrax Admin'");
                    alert(response.responseText);
                },
                failure: function (response) {
                    alert("Error, Please contact 'Inventrax Admin'");
                    alert(response.responseText);
                }

            });
        },
        select: function (e, i) {

            $("#hifSupplierID").val(i.item.val);

        },
        minLength: 0
    });
}

function getMMCodeList() {

    try {
        if ($("#txtModifyTenant").val() == "") { $("#hifTenant").val("0") }
        $('#txtFixedmaterialcode').val("");
        var textfieldname = $("#txtFixedmaterialcode");
        DropdownFunction(textfieldname);
        $("#txtFixedmaterialcode").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "LocationHandlers/GetMMCodehandler.ashx?prefix=" + request.term + "&choice=mcode" + "&Tenant=" + $("#hifTenant").val(),
                    dataType: "json",
                    type: "POST",
                    async: true,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        debugger;
                        response($.map(data, function (item) {
                            return {
                                label: item.split('~')[0].split('`')[0],
                                description: item.split('~')[0].split('`')[1] == undefined ? "" : " <font color='#f47f4a'>" + item.split('~')[0].split('`')[1] + "</font>",
                                val: item.split('~')[1]
                            }
                        }))

                    },
                    error: function (response) {
                        alert("Error, Please contact 'Inventrax Admin' ");
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert("Error, Please contact 'Inventrax Admin' ");
                        alert(response.responseText);
                    }

                });
            },
            minLength: 0,

        }).data("autocomplete")._renderItem = function (ul, item) {
            // Inside of _renderItem you can use any property that exists on each item that we built
            // with $.map above */
            return $("<li></li>")
                .data("item.autocomplete", item)
                .append("<a>" + item.label + "" + (item.description == null ? '' : item.description) + "</a>")
                .appendTo(ul)
        };
    } catch (ex) { }
}

function getMMCodeListBulk() {

    try {

        if ($("#txtModifyTenantBulk").val() == "") { $("#hidTenantIDBulk").val("0") }

        $('#txtFixedmaterialcodeBulk').val("");
        var textfieldname = $("#txtFixedmaterialcodeBulk");
        DropdownFunction(textfieldname);
        $("#txtFixedmaterialcodeBulk").autocomplete({
            source: function (request, response) {
                $.ajax({
                    //url: "LocationHandlers/GetMMCodehandler.ashx?prefix=" + request.term + "&choice=mcode" + "&Tenant=0",
                    url: "LocationHandlers/GetMMCodehandler.ashx?prefix=" + request.term + "&choice=mcode" + "&Tenant=" + $("#hidTenantIDBulk").val(),
                    dataType: "json",
                    type: "POST",
                    async: true,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        debugger;
                        response($.map(data, function (item) {
                            return {
                                label: item.split('~')[0].split('`')[0],
                                description: item.split('~')[0].split('`')[1] == undefined ? "" : " <font color='#f47f4a'>" + item.split('~')[0].split('`')[1] + "</font>",
                                val: item.split('~')[1]
                            }
                        }))

                    },
                    error: function (response) {
                        alert("Error, Please contact 'Inventrax Admin' ");
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert("Error, Please contact 'Inventrax Admin' ");
                        alert(response.responseText);
                    }

                });
            },
            minLength: 0,

        }).data("autocomplete")._renderItem = function (ul, item) {
            // Inside of _renderItem you can use any property that exists on each item that we built
            // with $.map above */
            return $("<li></li>")
                .data("item.autocomplete", item)
                .append("<a>" + item.label + "" + (item.description == null ? '' : item.description) + "</a>")
                .appendTo(ul)
        };
    } catch (ex) { }
}
//getMMCodeListBulk();
function getselectMixedMaterial() {

    var IsM = $('#selectIsMMA').attr('checked') == undefined ? 0 : 1;

    if (IsM == "1") {
        document.getElementById("trFixedMaterial").style.display = "none";
        $('#txtFixedmaterialcode').empty();
    }
    else {
        document.getElementById("trFixedMaterial").style.display = "block";
        getMcodeListboxList();
    }

    //Bulk
    var IsMB = $('#selectIsMMABulk').attr('checked') == undefined ? 0 : 1;
    if (IsMB == "1") {
        document.getElementById("trFixedMaterialBulk").style.display = "none";
        $('#txtFixedmaterialcodeBulk').empty();
    }
    else {
        document.getElementById("trFixedMaterialBulk").style.display = "block";
        getMcodeListboxListBulk();
    }
}

function getMcodeListboxList() {

    $('#txtFixedmaterialcode').empty();
    $.ajax({
        //url: "LocationHandlers/GetMMCodehandler.ashx?prefix=MAN&choice=SupplierData" + "&TenantID=" + document.getElementById('txtModifyTenant').value,
        url: "LocationHandlers/GetMMCodehandler.ashx?prefix=MAN&choice=mcode" + "&Tenant=0",
        dataType: "json",
        type: "POST",
        async: true,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $.map(data, function (item) {
                $('#txtFixedmaterialcode').append(
                    $('<option></option>').html(item.split(',')[0])
                );
            });
        },
        error: function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}


function getMcodeListboxListBulk() {

    $('#txtFixedmaterialcodeBulk').empty();
    $.ajax({
        //url: "LocationHandlers/GetMMCodehandler.ashx?prefix=MAN&choice=SupplierData" + "&TenantID=" + document.getElementById('txtModifyTenant').value,
        url: "LocationHandlers/GetMMCodehandler.ashx?prefix=MAN&choice=mcode" + "&Tenant=0",
        dataType: "json",
        type: "POST",
        async: true,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $.map(data, function (item) {
                $('#txtFixedmaterialcodeBulk').append(
                    $('<option></option>').html(item.split(',')[0])
                );
            });
        },
        error: function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}


$(document).ready(function () {
    $("#canwidth .ui-slider").css('background', '#003366');
    $("#MainContent_MMContent_ddlWarehouse").change(function () {
        debugger;
        $("#MainContent_MMContent_ddlLocationCode").html("");

        var DivisionID = $("#MainContent_MMContent_ddlWarehouse").attr("value");
        if (DivisionID != 0) {

            $.ajax({
                type: "POST",
                url: "LocationHandlers/Warehouse_LocationCode.ashx?whid=" + DivisionID,
                contentType: "application/json",
                async: false,
                success: function (locationcodes) {

                    $("#MainContent_MMContent_ddlLocationCode").append($("<option></option>").val('0').html('Select Zone'));
                    $.each(locationcodes, function () {

                        if (this['ID'] != undefined) {
                            $("#MainContent_MMContent_ddlLocationCode").append($("<option></option>").val(this['ID']).html(this['LocationCode']));
                        }
                    });
                },
                error: function (response) {
                    showStickyToast(false, "No zones available", false);
                    return false;
                }
            });
        }
    });


});
//close update dialog
function CloseDialog(id) {

    $("#txtBayTenant").val("");
    $("#txtBaySupplier").val("");
    $("#txtModifyTenant").val("");
    $("#txtModifySupplier, #txtModifySupplierBulk").val("");
    $("#txtSelectTenant").val("");
    $("#txtSupplier").val("");

    $(id).dialog("close");
}
function ClosePrintDialog(id) {



    $('#' + id + '').dialog("close");
}
function SearchCategory() {
    debugger;

    var searchid = document.getElementById("selSearchCategory").value;
    var vTenantID = $('#MainContent_MMContent_hdnTenantID').val();
    var DivisionID = $("#MainContent_MMContent_ddlWarehouse").attr("value");

    if (searchid == 1) {

        getTenantAutoComplete(vTenantID, DivisionID);

        // $('.mcodepicker').autocomplete('search', '');
    } else if (searchid == 2) {
        getMaterialAutoComplete(vTenantID);
        //$('.mcodepicker').autocomplete('search', '');
    } else {
        document.getElementById("selSearchCategory").focus();
        //document.getElementById("txtSearch").value = "Search Part /Tenant";
        //$("#lblSearch").text("Search Part /Tenant");
        GetSearchCategory();
    }
}

function GetSearchCategory() {

    $('.mcodepicker').autocomplete({
        source: function (request, response) {
            return false;
        },
    });
}

//Searching for Materilawiselocationns or department wise locations
function doSearch() {
    var SearchElement = document.getElementById("selSearchCategory").value;

    if (SearchElement == 1 || SearchElement == 2) {

        SerachTenantData(SearchElement);
    }
    else {
        showStickyToast(false, "Select search category", false);
    }
}

function SerachTenantData(SearchElement) {

    BlockDialog();
    if (SearchElement == 2) {
        //here iam going to search for material wise locations

        var searchelement = document.getElementById("txtSearch").value.trim();

        searchelement = searchelement.toUpperCase();
        if (searchelement.length >= 3) {
            drawPrevoiusData();
            mtlocationarray.splice(0);
            TenantArray.splice(0);

            locdetailsarray.forEach(function (loc) {

                if (loc.name.indexOf(searchelement) != -1) {
                    mtlocationarray.push({
                        name: loc.name,
                        label: loc.label,
                        xaxis: loc.xaxis,
                        yaxis: loc.yaxis,
                        width: loc.width,
                        height: loc.height,
                        color: loc.color
                    });

                }
            });
            drawSearchData();
            UnBlockDialog();
        } else {

            showStickyToast(false, "Part number must be more than 3 characters", false);
            UnBlockDialog();
        }
    }
    else if (SearchElement == 1) {
        //here iam going to search for department wise locations
        var searchelement = document.getElementById('hidTenantID').value;

        drawPrevoiusData();
        mtlocationarray.splice(0);
        TenantArray.splice(0);

        locdetailsarray.forEach(function (loc) {
            //var LocationSplit = loc.name.split('@');
            var LocationSplice = loc.name.split('@')[0].toString();//.splice(0, 1).toString();

            var TenantMaterialList = LocationSplice.split('|');
            var TenantList = TenantMaterialList.splice(0, 1).toString();
            var LocationList = TenantList;
            var MaterialList = TenantMaterialList.toString();

            var TenantData = TenantList.split('!');
            var TenantID = TenantData.splice(1, 1).toString();
            //var TenantID = TenantData;

            //if (loc.name.indexOf(searchelement) != -1)
            if (MaterialList.length != 0 && TenantID == searchelement) {
                mtlocationarray.push({
                    name: loc.name,
                    label: loc.label,
                    xaxis: loc.xaxis,
                    yaxis: loc.yaxis,
                    width: loc.width,
                    height: loc.height,
                    color: loc.color
                });
            }

            if (LocationList.length != 0 && TenantID == searchelement) {
                TenantArray.push({
                    name: loc.name,
                    label: loc.label,
                    xaxis: loc.xaxis,
                    yaxis: loc.yaxis,
                    width: loc.width,
                    height: loc.height,
                    color: loc.color,
                });
            }


        });

        drawSearchData();
        //document.getElementById("hidTenantID").value = 0;

    }
}

function drawPrevoiusData() {
    var searchelement = document.getElementById("txtSearch").value.trim();

    //searchelement = "$" + searchelement;
    mtlocationarray.forEach(function (mdata) {
        var elem = document.getElementById('myCanvas');
        var canvasContext = elem.getContext("2d");
        canvasContext.fillStyle = mdata.color;
        canvasContext.fillRect(mdata.xaxis, mdata.yaxis, mdata.width, mdata.height);
        canvasContext.fillStyle = "black";
        canvasContext.fillText(mdata.label, mdata.xaxis + 6, mdata.yaxis + 8);
    });

    TenantArray.forEach(function (mdata) {
        var elem = document.getElementById('myCanvas');
        var canvasContext = elem.getContext("2d");
        canvasContext.fillStyle = mdata.color;
        canvasContext.fillRect(mdata.xaxis, mdata.yaxis, mdata.width, mdata.height);
        canvasContext.fillStyle = "black";
        canvasContext.fillText(mdata.label, mdata.xaxis + 6, mdata.yaxis + 8);
    });
}
//drawing data on canvas after searching
function drawSearchData() {
    locinformation.innerHTML = '';
    if (TenantArray.length != 0) {
        TenantArray.forEach(function (Tenantloc) {
            var elem = document.getElementById('myCanvas');
            var canvasContext = elem.getContext("2d");
            var grd = canvasContext.createRadialGradient(Tenantloc.xaxis - 2, Tenantloc.yaxis - 2, 5, Tenantloc.xaxis + 6, Tenantloc.yaxis + 6, 20);
            grd.addColorStop(0, "#ff4dff");

            canvasContext.fillStyle = grd;
            canvasContext.fillRect(Tenantloc.xaxis, Tenantloc.yaxis, Tenantloc.width, Tenantloc.height);
            canvasContext.fillStyle = "black";
            canvasContext.fillText(Tenantloc.label, Tenantloc.xaxis + 6, Tenantloc.yaxis + 8);

        });
    }

    if (mtlocationarray.length != 0) {

        mtlocationarray.forEach(function (mtloc) {
            var elem = document.getElementById('myCanvas');
            var canvasContext = elem.getContext("2d");
            //alert(mtloc.name.substr(0,7));
            // canvasContext.fillStyle = "#CCFF00";
            //canvasContext.fillStyle = fillStyle.
            var grd = canvasContext.createRadialGradient(mtloc.xaxis - 2, mtloc.yaxis - 2, 5, mtloc.xaxis + 6, mtloc.yaxis + 8, 20);
            grd.addColorStop(0, "#58ACFA");
            grd.addColorStop(1, "#0066FF");

            canvasContext.fillStyle = grd;
            canvasContext.fillRect(mtloc.xaxis, mtloc.yaxis, mtloc.width, mtloc.height);
            canvasContext.fillStyle = "black";

            canvasContext.fillText(mtloc.label, mtloc.xaxis + 6, mtloc.yaxis + 8);
            locinformation.innerHTML += mtloc.name.substring(0, 9) + "&nbsp&nbsp&nbsp";
        });
    }

    else {
        showStickyToast(false, "No materials are available", false);
    }
    UnBlockDialog();
}
//prasad
function checkNum(evt) {

    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;

    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        status = "This field accepts numbers only."
        return false;
    }
    status = "";
    return true;
}

function checkNumInclZero(evt) {

    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;

    //if (charCode == 48) {
    //    return false;
    //}
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        status = "This field accepts numbers only."
        return false;
    }
    status = "";
    return true;
}

function ClearText1(TextBox) {

    if (TextBox.value = "Search RT Part # ...") {
        TextBox.value = "";
        TextBox.style.color = "#000000";
    }
    function FillText(TextBox) {
        TextBox.value = "Search RT Part # ...";
    }

}

function ClearText(TextBox) {

    if (document.getElementById('txtSearch').value = "Search Part /Tenant") {
        document.getElementById('txtSearch').value = "";
        document.getElementById('txtSearch').style.color = "#000000";

    }
}

function FillDefaultText() {

    if (document.getElementById('txtSearch').value.trim() == "") {

        document.getElementById('txtSearch').value = "Search Part /Tenant";
    }
}

function GetSelectedData() {
    if (document.getElementById("selSearchCategory").value == 0) {
        document.getElementById("txtSearch").value = "";
        $("#lblSearch").text("Search Part /Tenant");
        document.getElementById("hdnTenantID").value = "0";
    }
    else if (document.getElementById("selSearchCategory").value == 1) {
        document.getElementById("txtSearch").value = "";
        $("#lblSearch").text("Search Tenant");
        document.getElementById("hdnTenantID").value = "0";
    }
    else if (document.getElementById("selSearchCategory").value == 2) {
        document.getElementById("txtSearch").value = "";
        $("#lblSearch").text("Search Part Number");
        document.getElementById("hdnTenantID").value = "0";
    }
}




/////////////////////////New Rendering Logic/////////////////////////////

var ZoneID = 0;
var ZoneCode = "";
$(document).ready(function () {
    $('#divMap').draggable();
    $('#MainContent_MMContent_divMap1').draggable();

    var AccID = GetQueryString('ACCID');
    var WHID = GetQueryString('WHId');
    var ZoneID = GetQueryString('ZNId');

    if (WHID != null && ZoneID != null) {
        $('#MainContent_MMContent_ddlWarehouse').val(WHID);
        $('#MainContent_MMContent_ddlWarehouse').trigger('change');

        $('#MainContent_MMContent_ddlLocationCode').val(ZoneID);
        LoadMap();  // Stops server side loading
    }

    $('#MainContent_MMContent_ddlLocationCode').change(function () {
        ZoneID = $('#MainContent_MMContent_ddlLocationCode').val();
        $('.hdnSelectedZone').val(ZoneID);
    });

    RoleBasedButtons();
    var TextFieldName = $("#txtModifyTenantBulk");
    DropdownFunction(TextFieldName);
    $('#txtModifyTenantBulk').autocomplete({
        source: function (request, response) {
            debugger;
            var WHID = $("#MainContent_MMContent_ddlWarehouse").attr("value");
            $.ajax({
                url: "LocationHandlers/GetMMCodehandler.ashx?prefix=" + request.term + "&choice=tenentdata" + "&Tenant=0 &WarehouseID=" + WHID + "",
                dataType: "json",
                type: "POST",
                async: true,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    debugger;
                    response($.map(data, function (item) {
                        return {
                            label: item.split(',')[0],
                            val: item.split(',')[1]

                        }
                    }))

                },
                error: function (response) {
                    showStickyToast(false, "Error, Please contact 'Inventrax Admin'. " + response.responseText, false);
                },
                failure: function (response) {
                    showStickyToast(false, "Error, Please contact 'Inventrax Admin'. " + response.responseText, false);
                }

            });
        },
        select: function (e, i) {
            $("#hidTenantIDBulk").val(i.item.val);
            $("#txtModifyTenantBulk").val(i.item.label);

            getListboxList('ModifyBulk');
        },
        minLength: 0
    });



});

function RoleBasedButtons() {
    if ($('#MainContent_MMContent_hdnRoles').val() == "1") {
        $('#btnCreate, #btnLocationdelete, #btnUpdatelocation, #btnUpdatelocationBulk, #btnAdding').show();
    }
    else {
        $('#btnCreate, #btnLocationdelete, #btnUpdatelocation, #btnUpdatelocationBulk, #btnAdding').hide();
    }

}

function LoadMap() {
    debugger;
    ZoneID = $('#MainContent_MMContent_ddlLocationCode').val();
    WareHouseID = $('#MainContent_MMContent_ddlWarehouse').val();
    $('#txtSupplierAdd').empty();
    if (WareHouseID == 0) {
        debugger;
        showStickyToast(false, "Please select Warehouse", false);
        return false;
    }
    if (ZoneID == null || ZoneID == 0) {
        showStickyToast(false, "Please select Zone", false);
        return false;
    }
    BlockDialog();
    ZoneID = $("#MainContent_MMContent_ddlLocationCode").val();
    ZoneCode = $('#MainContent_MMContent_ddlLocationCode option:selected').text();
    var data = "{ 'zoneID':'" + ZoneID + "' }";
    InventraxAjax.AjaxResultExecute("LocationManager.aspx/GetLocations", data, 'GetLocationsOnSuccess', 'GetLocationsOnError', null);
}

var selectedBins = [];
var selectedBinIDs = [];
function GetLocationsOnSuccess(data) {
    console.log(data);


    debugger;
    $('#divMap').empty();
    selectedBins = [];
    selectedBinIDs = [];
    var obj = data.Result;
    if (obj.length <= 0) {
        showStickyToast(false, "No Locations Found", false);
        UnBlockDialog();
        return false;
    }
    var rack = "";
    var bindataClass = "";
    var divMapWidth = 1000, rackHeight = 100, rackWidth = 25, columnWidth = 100, levelHeight = 20;
    for (var i = 0; i < obj.length; i++) {
        rackHeight = GetRackHeight_Div(obj[i]);
        rackWidth = GetRackWidth_Div(obj[i]);
        divMapWidth = divMapWidth < rackWidth ? rackWidth : divMapWidth;
        $('#divMap').css({ 'width': divMapWidth + 'px' });
        rack = "<div class='divRackContainer' style='width:" + rackWidth + "px;'>";
        //rack += "<div class='divRackName' style='height:" + rackHeight + "px;line-height:" + rackHeight + "px;'>" + obj[i].RackName + "</div><div class='divColumnContainer'>";
        rack += "<div class='divRackName'><div style='align-self:center;'>" + obj[i].RackName + "</div></div><div class='divColumnContainer'>";

        for (var j = 0; j < obj[i].ColumnList.length; j++) {
            rack += "<div class='divColumn' >";

            for (var k = 0; k < obj[i].ColumnList[j].LevelList.length; k++) {
                rack += "<div class='divLevelBox'";
                rack += "<div> ";
                for (var l = 0; l < obj[i].ColumnList[j].LevelList[k].binList.length; l++) {
                    var bindata = obj[i].ColumnList[j].LevelList[k].binList[l].bindata;
                    if (bindata != "")
                        bindataClass = "bindataClass";
                    else
                        bindataClass = "";

                    rack += "<span class='spanBin " + bindataClass + "' data-locationID='" + obj[i].ColumnList[j].LevelList[k].binList[l].LocationID + "' data-fulllocation='" + obj[i].ColumnList[j].LevelList[k].binList[l].FullLocation + "' data-Tenant='" + obj[i].ColumnList[j].LevelList[k].binList[l].Tenant + "' data-TenantID='" + obj[i].ColumnList[j].LevelList[k].binList[l].TenantID + "'   data-bindata='" + bindata + "' data-binrepdata='" + obj[i].ColumnList[j].LevelList[k].binList[l].binRepdata + "' data-suppliers='" + obj[i].ColumnList[j].LevelList[k].binList[l].Suppliers + "'   data-isQ='" + obj[i].ColumnList[j].LevelList[k].binList[l].IsQuarantine + "' data-isF='" + obj[i].ColumnList[j].LevelList[k].binList[l].IsFastMoving + "' data-isM='" + obj[i].ColumnList[j].LevelList[k].binList[l].IsMixedMaterialOK + "'>" + obj[i].ColumnList[j].LevelList[k].binList[l].BinName + "</span>";
                }
                rack += "</div> ";
                //rack += "<div class='divLevel'  style='height:" + levelHeight + "px;'>" + obj[i].ColumnList[j].ColumnName + "-" + obj[i].ColumnList[j].LevelList[k].LevelName + "</div>";
            }
            rack += " </div>";
        }
        rack += "</div>";

        rack += "</div>";
        $('#divMap').append(rack);
    }
    LoadHandlers();

    UnBlockDialog();


}
function GetLocationsOnError(data) {

    UnBlockDialog();
}


function GetRackHeight_Div(oRack) {
    var rackHeight = 0;
    for (var i = 0; i < oRack.ColumnList.length; i++) {
        if (oRack.ColumnList[i].LevelList.length > rackHeight)
            rackHeight = oRack.ColumnList[i].LevelList.length;
    }
    return rackHeight * 50;
}

function GetRackWidth_Div(oRack) {
    debugger;
    return oRack.ColumnList.length * 200;
}



function LoadHandlers() {
    $(document).ready(function () {
        var selectClass = '';
        $('.spanBin').click(function () {
            debugger;
            selectClass = $(this).attr('class');
            if (selectClass.indexOf('binSelect') > -1) {
                $(this).removeClass('binSelect');
                for (var i = selectedBins.length - 1; i >= 0; i--) {
                    if (selectedBins[i] === $(this).attr('data-fulllocation')) {
                        selectedBins.splice(i, 1);

                        if (selectedBinIDs[i] === $(this).attr('data-locationid')) {
                            selectedBinIDs.splice(i, 1);
                        }

                    }
                }
            }
            else {
                $(this).addClass('binSelect');
                selectedBins.push($(this).attr('data-fulllocation'));
                selectedBinIDs.push($(this).attr('data-locationid'));
            }
        });

        $('.spanBin').mouseover(function (event) {

            var MenuBarWidth = $('.swipe-area').css('width').replace('px', '');

            var fullLocation = $(this).attr('data-fulllocation');
            var Tenant = $(this).attr('data-tenant');
            Tenant = Tenant == '' ? '' : '[' + $(this).attr('data-tenant') + ']';
            var matData = $(this).attr('data-bindata');
            var binRepData = $(this).attr('data-binrepdata');

            var isQ = $(this).attr('data-isq');
            isQ = isQ == 1 ? '<i class="iconStyle" Title="FWMSC21® - Is Quarantine"><img src="../Images/do-not-drop-filled-active.png" width="30"></i>' : '<i class="iconStyle" Title="FWMSC21® - Is Quarantine"><img src="../Images/do-not-drop-filled-inactive.png" style="filter:grayscale(100%;);" width="30"></i>';

            var isF = $(this).attr('data-isf');
            isF = isF == 'true' ? '<i class="iconStyle" Title="FWMSC21® - Fast Moving"><img src="../Images/exercise-filled-active.png" width="30"> </i>' : '<i class="iconStyle" Title="FWMSC21® - Fast Moving"><img src="../Images/exercise-filled-inactive.png" style="filter:grayscale(100%;);" width="30"></i>';

            var isM = $(this).attr('data-ism');
            isM = isM == 1 ? '<i class="iconStyle" Title="FWMSC21® - Is Mixed Material"><img src="../Images/open-box-filled-active.png" width="30"></i>' : '<i class="iconStyle" Title="FWMSC21® - Is Mixed Material"><img src="../Images/open-box-filled-inactive.png" style="filter:grayscale(100%;);" width="30"></i>';

            var sup = $(this).attr('data-suppliers');

            var w = window.innerWidth;
            var x = event.pageX - MenuBarWidth;
            var y = event.pageY - 10;
            if (MenuBarWidth < 200) {
                if (w - x < 580) {
                    x = x - 550;
                }
            }
            else {
                if (w - x < 750) {
                    x = x - 550;
                }
            }

            var s = '';
            s += '<div class="flex__"><div class="iconblock"><span class="spanIndiCators">' + isF + ' ' + isQ + ' ' + isM + '</span></div> <div class="table_block"><div ><h5 class="tooltipHeader">' + fullLocation + ' ' + Tenant + '</h5>  </div><table class="table-striped"></div>';
            s += '<tr><th style="width: 20px;">#<th>Part #</th><th>SLoc</th><th>Avl. Qty.</th><th>In-OnHold</th><th>Out-OnHold</th></tr>';
            s += GetTooltipData(matData);
            s += GetBinRepData(binRepData);
            s += GetSuppliersData(sup);
            // s += '       <tr><td>1</td><td>PartNo1RS</td><td>Minerva sup</td><td>5.00</td><td></td><td>3.00</td></tr>';
            s += '  </table></div>';

            $('#divTooltip').html(s);
            $('#divTooltip').css({ 'top': y + "px", 'left': x + "px" }).show();
        });

        $('#divTooltip').mouseleave(function () { $('#divTooltip').hide(); });
        $('body').click(function () {
            $('#divTooltip').hide();
        });

        //Zoom               

        var currentZoom = 1.0;

        $(".zoomIn").click(function () {
            $('#MainContent_MMContent_divpnlChart').animate({ 'zoom': currentZoom += .1 }, 'slow');

        });
        $(".zoomOff").click(function () {
            currentZoom = 1.0
            $('#MainContent_MMContent_divpnlChart').animate({ 'zoom': 1 }, 'slow');
        });
        $(".zoomOut").click(function () {
            $('#MainContent_MMContent_divpnlChart').animate({ 'zoom': currentZoom -= .1 }, 'slow');
        });



        //Fast moving changing event
        $('#selectIsFMA').change(function () {
            if ($('#selectIsFMA').attr('checked') == undefined) {
                if (!window.confirm("Bin Replenishment plans will be deleted?")) {
                    $('#selectIsFMA').attr('checked', 'checked');
                }
            }
        });

        $('#selectIsFMABulk').change(function () {
            if ($('#selectIsFMABulk').attr('checked') == undefined) {
                if (!window.confirm("Bin Replenishment plans will be deleted?")) {
                    $('#selectIsFMABulk').attr('checked', 'checked');
                }
            }
        });

    });
}


function GetTooltipData(matData) {

    var s = '';
    if (matData != "") {
        var obj = matData.split('#');
        for (var i = 0; i < obj.length; i++) {
            //HUL14529á [inventrax] á [100.00] áá áá
            var items = obj[i].split('@');
            s += '<tr><td>' + (i + 1) + '</td><td class="partNumber" style="width:150px !important;">' + items[0] + '<br/><span>' + items[8] + '</span> </td><td>' + items[7] + '</td><td>' + items[2] + '</td><td>' + items[3] + '</td><td>' + items[4] + '</td></tr>'
        }
    }
    else {
        s += '<tr><td colspan="6" style="text-align:center;">No Material Found</td></tr>';
    }

    return s;
}


function GetBinRepData(repData) {
    var s = '<tr><td align="center" colspan="6" style="text-align:center;color:#336699;"><b>Bin Replenishment Data</b></td></tr><tr><td colspan="6"><table><tr><th style="width: 20px;">#<th>Part #</th><th>Min.</th><th>Max.</th><th>Vol.%</th><th>Weight%</th>';
    if (repData != "") {
        var obj = repData.split('#');
        for (var i = 0; i < obj.length; i++) {
            //HUL14529á [inventrax] á [100.00] áá áá
            var items = obj[i].split('@');
            s += '<tr><td>' + (i + 1) + '</td><td>' + items[0] + '</td><td>' + items[2] + '</td><td>' + items[3] + '</td><td>' + items[4] + '</td></tr>'
        }
    }
    else {
        s += '<tr><td colspan="6" style="text-align:center;">No Replenishments Found</td></tr>';
    }

    s += '</tr></table></td></tr>';

    return s;
}
function GetSuppliersData(sup) {
    var suppliers = "";
    var s = '<tr><td align="center" colspan="6" style="text-align:center;color:#336699;"><b>Suppliers</b></td></tr>';
    if (sup != "") {
        sup = sup.split(',');
        for (var i = 0; i < sup.length; i++) {
            suppliers += "<span class='supLabel'>" + sup[i] + "</span>&nbsp;";
        }
        s += '<tr><td colspan="6">' + suppliers + '</td></tr>';
    }
    else {
        s += '<tr><td colspan="6" style="text-align:center;">No Suppliers</td></tr>';
    }
    return s;
}

//Delete Selected Locations
function deleteLocations() {
    if (selectedBins.length == 0) {
        showStickyToast(false, "Please select a Bin to delete", false);
    }


    var dellocations = selectedBins.join(',');


    if (dellocations != "") {
        document.getElementById("deletealertspan").innerHTML = "<b>The following location will be deleted permanently<br>" + dellocations;
        $("#deletealert").dialog("option", "title", "Delete Location(s)");
        $("#deletealert").dialog('open');
    }
}

function doDelete() {
    debugger;
    $.ajax({
        type: "POST",
        url: "LocationHandlers/DeleteLocationHandler.ashx?dellocations=" + selectedBinIDs.join(','),//selectedBins.join(',')
        contentType: "text/html",
        success: function (response) {
            if (response == "success") {
                LoadMap();
                CloseDialog(deletealert);
                showStickyToast(true, "Deleted successfully", false);
            } else {
                CloseDialog(deletealert);
                showStickyToast(false, "Unable to delete the selected bin, it contains items", false);
            }
        },
        error: function (response) {
            showStickyToast(false, "Error", false);
        }
    });
}


//Update Selected Locations

function GetUpdateDialog() {
    debugger;
    selectedlocation = "";
    document.getElementById("UpdateError").innerHTML = "";
    if (selectedBins.length == 0) {
        showStickyToast(false, "Please select a bin to update", false);
        return false;
    }
    else if (selectedBins.length > 1) {
        showStickyToast(false, "Please select only one bin to update", false);
        return false;

    }

    else if (selectedBins.length == 1) {
        selectedlocation = selectedBins[0];

        var mats = $('[data-fulllocation=' + selectedBins[0] + ']').attr('data-bindata');
        if (mats.length > 0) {
            $('#selectIsActive').attr('disabled', 'disabled');
        }
        else {
            $('#selectIsActive').removeAttr('disabled');
        }

        $.ajax({
            type: "GET",
            url: "LocationHandlers/GetLocationDetailsHandler.ashx?location=" + selectedBinIDs[0],
            contentType: "application/json; charset=utf-8",
            success: function (jsonList) {
                prepareUpdateDialog(jsonList);
            },
            error: function (e) {
                showStickyToast(false, "Error please contact 'Inventrax Admin' ", false);
            }
        });

    }
}

function prepareUpdateDialog(jsonList) {
    debugger;

    $('#txtModifySupplier').empty();
    txtwidth.value = jsonList[0].width;
    txtHeight.value = jsonList[1].height;
    txtLength.value = jsonList[2].length;
    txtMaxweight.value = jsonList[3].maxweight;

    var IsMMok = jsonList[4].isMMok;
    var Location = jsonList[16].Location;
    var FixedMCode = jsonList[5].fixedMcode;
    var IsActive = jsonList[6].isactive;
    var IsQuarantine = jsonList[7].isQuarantine;
    var Tenant = jsonList[8].Tenant;
    var Supplier = jsonList[9].Supplier;
    var TenantID = jsonList[10].TenantID;
    var SupplierID = jsonList[11].SupplierID;
    var CBM = jsonList[13].cbm;
    var IsFastMoving = jsonList[14].isFastMoving;
    $('#MainContent_MMContent_ddlLocType').val(jsonList[15].LocType);

    if (IsMMok == 0)
        $('#selectIsMMA').removeAttr('checked');
    else
        $('#selectIsMMA').attr('checked', 'checked');

    if (IsActive == 0)
        $('#selectIsActive').removeAttr('checked');
    else
        $('#selectIsActive').attr('checked', 'checked');

    if (IsQuarantine == 0)
        $('#selectIsQuarantine').removeAttr('checked');
    else
        $('#selectIsQuarantine').attr('checked', 'checked');

    if (IsFastMoving == 0)
        $('#selectIsFMA').removeAttr('checked');
    else
        $('#selectIsFMA').attr('checked', 'checked');

    txtModifyTenant.value = Tenant;
    $("#hifTenant").val(TenantID);
    divCbm.innerText = CBM;
    txtFixedmaterialcode.value = FixedMCode;

    $.ajax({
        url: "LocationHandlers/GetMMCodehandler.ashx?prefix=0&choice=SupplierData" + "&Tenant=" + jsonList[8].Tenant,
        dataType: "json",
        type: "POST",
        async: true,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $.map(data, function (item) {
                if (jsonList[8].Tenant != "") {
                    $('#txtModifySupplier').append(
                        $('<option></option>').html(item.split(',')[0]) // Append all Suppliers
                    );
                }
            });

            var TotalSuppliers = document.getElementById('txtModifySupplier');
            var SelectedSupplier = Supplier.split(',');
            var Count = 0;
            for (var i = 0; i < SelectedSupplier.length; i++) {
                for (var j = 0; j < TotalSuppliers.options.length; j++) {
                    if (SelectedSupplier[i].trim() == TotalSuppliers.options[j].value.trim()) {
                        TotalSuppliers.options[j].selected = true;
                    }
                }
            }

        },
        error: function (request, error) {
            showStickyToast(false, "Request: " + JSON.stringify(request), false);
        }
    });

    document.getElementById("updatebindialog").style.visibility = "visible";
    $("#updatebindialog").dialog('option', 'title', Location);
    $("#updatebindialog").dialog('option', 'position', [300, 40]);
    $("#updatebindialog").dialog('open');

    if (IsMMok == "0") {
        document.getElementById("trFixedMaterial").style.display = "block";
    }
    else {
        document.getElementById("trFixedMaterial").style.display = "none";
    }
}

function UpdateLocationDetails() {

    var IsMMok = $('#selectIsMMA').attr('checked');
    IsMMok = IsMMok == undefined ? 0 : 1;

    var mmcode = "";
    if (IsMMok == "0" && document.getElementById("txtFixedmaterialcode").value.trim() == "") {

        document.getElementById("UpdateError").innerHTML = "<b>'Material Code' cannot be blank when the bin is configured to a fixed  material<br>";
        //document.getElementById("warningalertspan").innerHTML = "<b>Material Code' cannot be blank when the bin is configured to a fixed  material<br>";
        //$("#warningalert").dialog("option", "title", "Warning");
        //document.getElementById("warningalert").style.background = "red";
        //$("#warningalert").dialog('open');
    }
    if (IsMMok == "0" && document.getElementById("txtFixedmaterialcode").value.trim() != "") {
        $.ajax({
            type: "POST",
            url: "LocationHandlers/CheckMMcodehandler.ashx?mmcode=" + document.getElementById("txtFixedmaterialcode").value,
            contentType: "text/html",
            success: function (response) {
                mmcode = response;
                if (mmcode == "0") {
                    document.getElementById("warningalertspan").innerHTML = "<b>Please provide valid material code";
                    $("#warningalert").dialog("option", "title", "Warning");
                    document.getElementById("warningalert").style.background = "red";
                    $("#warningalert").dialog('open');
                } else if (mmcode != "0") {
                    UpdateLocationData(document.getElementById("txtFixedmaterialcode").value);
                    CloseDialog(updatebindialog);
                }
            },
            error: function (response) {
                showStickyToast(false, "Error", false);
                return false;
            }
        });
    }
    if (IsMMok == "1") {
        mmcode = "null";
        if (UpdateLocationData(mmcode) == false) {
        }
        else {
            CloseDialog(updatebindialog);
        }

    }
}
function UpdateLocationData(mmcode) {

    var Width = document.getElementById("txtwidth").value;
    var Height = document.getElementById("txtHeight").value;
    var Length = document.getElementById("txtLength").value;
    var Weight = document.getElementById("txtMaxweight").value;

    var IsMMOk = $('#selectIsMMA').attr('checked') == undefined ? 0 : 1;
    var IsFM = $('#selectIsFMA').attr('checked') == undefined ? 0 : 1;
    var IsActive = $('#selectIsActive').attr('checked') == undefined ? 0 : 1;
    var IsQ = $('#selectIsQuarantine').attr('checked') == undefined ? 0 : 1;
    var Tenant = document.getElementById("txtModifyTenant").value;

    //if (document.getElementById('txtModifyTenant').value == "" && SelectedSuppliers != "") {
    //    showStickyToast(false, "Please select the Tenant", false);
    //    return false;
    //}

    if (Width == "" || Width == 0 || Height == "" || Height == 0 || Length == "" || Length == 0 || Weight == "" || Weight == 0) {

        showStickyToast(false, "Please enter Dimension details", false);
        return false;
    }
    var SelectedSuppliers = [];
    $('#txtModifySupplier :selected').each(function (i, selected) {
        SelectedSuppliers[i] = $(selected).text();
    });



    selectedlocation = "";

    selectedlocation = selectedBinIDs.join(',');//selectedBins.join(',');

    //if (SelectedSuppliers != "" && document.getElementById('txtModifyTenant').value == "") {
    //    showStickyToast(false, "Please select the Tenant", false);
    //    return false;
    //}
    if (document.getElementById("txtModifyTenant").value == "") {
        SelectedSuppliers = "";
    }

    $.ajax({
        type: "GET",
        url: "LocationHandlers/UpdateLocDataHandler.ashx?width=" + Width + "&height=" + Height + "&length=" + Length + "&maxweight=" + Weight + "&ismmok=" + IsMMOk + "&isFM=" + IsFM + "&isactive=" + IsActive + "&MCode=" + mmcode + "&locid=" + selectedlocation + "&IsQuarantine=" + IsQ + "&Tenant=" + document.getElementById("txtModifyTenant").value + "&Supplier=" + SelectedSuppliers + "&ZoneID=" + ZoneID + "&LocType=" + $('#MainContent_MMContent_ddlLocType').val(),

        contentType: "text/html",
        success: function (response) {
            LoadMap();
        },
        error: function (response) {
            showStickyToast(false, "Sorry some error occured", false);
        }
    });
}

//Bulk Modify 
function GetUpdateDialogBulk() {
    debugger;
    var zone = $('#MainContent_MMContent_ddlLocationCode').val();
    if (zone == null || zone == 0) {
        showStickyToast(false, "Please select Zone", false);
        return false;
    }



    //document.getElementById("UpdateErrorBulk").innerHTML = "";

    if (selectedBins.length == 1) {
        selectedlocation = selectedBinIDs[0];//selectedBins[0];
        $.ajax({
            type: "GET",
            url: "LocationHandlers/GetLocationDetailsHandler.ashx?location=" + selectedlocation,
            contentType: "application/json; charset=utf-8",
            success: function (jsonList) {
                prepareUpdateDialog(jsonList);
            },
            error: function (e) {
                showStickyToast(false, "Error please contact 'Inventrax Admin' ", false);
            }
        });

    }

    else if (selectedBins.length > 0) {
        showStickyToast(false, "Please deselect a bin", false);
        return false;
    }

    else {


        $('#selectIsActiveBulk').attr('disabled', 'disabled');


        LoadRacks(zone);
        $('#txtModifySupplierBulk').empty();
        txtwidthBulk.value = "";
        txtHeightBulk.value = "";
        txtLengthBulk.value = "";
        txtMaxweightBulk.value = "";
        txtFixedmaterialcodeBulk.value = "";
        // selectIsMMABulk.value = 1;               
        // selectIsActiveBulk.value = 1;
        // selectIsQuarantineBulk.value = 0;
        //selectIsFMABulk.value = 0;
        $('#selectIsFMABulk').removeAttr('checked');
        $('#selectIsMMABulk').attr('checked', 'checked');
        $('#selectIsQuarantineBulk').removeAttr('checked');
        $('#selectIsActiveBulk').attr('checked', 'checked');

        txtModifyTenantBulk.value = "";

        $("#hifTenant").val("");
        $("#hifSupplierID").val("");
        divCbmBulk.innerText = "0";



        document.getElementById("updatebindialogBulk").style.visibility = "visible";
        $("#updatebindialogBulk").dialog('option', 'title', 'Bulk Modify');
        $("#updatebindialogBulk").dialog('option', 'position', [300, 40]);
        $("#updatebindialogBulk").dialog('open');

        if (document.getElementById("selectIsMMABulk").value == "0") {
            document.getElementById("trFixedMaterialBulk").style.display = "block";
        }
        else {
            document.getElementById("trFixedMaterialBulk").style.display = "none";
        }

    }
}

function UpdateLocationDetailsBulk() {
    debugger;
    var mmcode = "";
    var isF = $('#selectIsFMABulk').attr('checked') == undefined ? 0 : 1;
    var isM = $('#selectIsMMABulk').attr('checked') == undefined ? 0 : 1;

    //$('#selectIsMMABulk').attr('checked', 'checked');
    //$('#selectIsQuarantineBulk').removeAttr('checked');
    //$('#selectIsActiveBulk').attr('checked', 'checked');

    if (isM == "0" && document.getElementById("txtFixedmaterialcodeBulk").value.trim() == "") {

        document.getElementById("UpdateErrorBulk").innerHTML = "<b>'Material Code' cannot be blank when the bin is configured to a fixed  material<br>";

    }
    if (isM == "0" && document.getElementById("txtFixedmaterialcodeBulk").value.trim() != "") {
        $.ajax({
            type: "POST",
            url: "LocationHandlers/CheckMMcodehandler.ashx?mmcode=" + document.getElementById("txtFixedmaterialcodeBulk").value,
            contentType: "text/html",
            success: function (response) {
                mmcode = response;
                if (mmcode == "0") {
                    document.getElementById("warningalertspan").innerHTML = "<b>Please provide valid material code";
                    $("#warningalert").dialog("option", "title", "Warning");
                    document.getElementById("warningalert").style.background = "red";
                    $("#warningalert").dialog('open');
                } else if (mmcode != "0") {
                    UpdateLocationDataBulk(document.getElementById("txtFixedmaterialcodeBulk").value);
                    CloseDialog(updatebindialogBulk);
                }
            },
            error: function (response) {
                showStickyToast(false, "Error", false);
                return false;
            }
        });
    }
    if (isM == "1") {
        mmcode = "null";
        if (UpdateLocationDataBulk(mmcode) == false) {
        }
        else {
            CloseDialog(updatebindialogBulk);
        }

    }
}

function UpdateLocationDataBulk(mmcode) {
    debugger;
    var Width = document.getElementById("txtwidthBulk").value;
    var Height = document.getElementById("txtHeightBulk").value;
    var Length = document.getElementById("txtLengthBulk").value;
    var Weight = document.getElementById("txtMaxweightBulk").value;

    var IsMMOk = $('#selectIsMMABulk').attr('checked') == undefined ? 0 : 1;
    var IsFM = $('#selectIsFMABulk').attr('checked') == undefined ? 0 : 1;
    var IsActive = $('#selectIsActiveBulk').attr('checked') == undefined ? 0 : 1;
    var IsQ = $('#selectIsQuarantineBulk').attr('checked') == undefined ? 0 : 1;
    var Tenant = document.getElementById("txtModifyTenantBulk").value;

    var zone = $('#MainContent_MMContent_ddlLocationCode').val();
    var rack = $('#ddlRackBulk').val();
    var col = $('#ddlColumnBulk').val();
    var lev = $('#ddlLevelBulk').val();
    var LocType = $('#MainContent_MMContent_ddlLocTypeBulk').val();

    if (zone == null || zone == 0) {
        showStickyToast(false, "Please select Zone", false);
        return false;
    }
    if (rack == null || rack == 0) {
        showStickyToast(false, "Please select Rack", false);
        return false;
    }

    //if (document.getElementById('txtModifyTenantBulk').value == "" && SelectedSuppliers != "") {
    //    showStickyToast(false, "Please select the Tenant", false);
    //    return false;
    //}

    if (Width == "" || Width == 0 || Height == "" || Height == 0 || Length == "" || Length == 0 || Weight == "" || Weight == 0) {

        showStickyToast(false, "Please enter Dimensions", false);
        return false;
    }
    var SelectedSuppliers = [];
    $('#txtModifySupplierBulk :selected').each(function (i, selected) {
        SelectedSuppliers[i] = $(selected).text();
    });

    //if (document.getElementById('txtModifyTenantBulk').value == "" && SelectedSuppliers != "") {
    //    showStickyToast(false, "Please select the Tenant", false);
    //    return false;
    //}

    selectedlocation = "";

    // selectedlocation = selectedBins.join(',');

    //if (SelectedSuppliers != "" && document.getElementById('txtModifyTenantBulk').value == "") {
    //    showStickyToast(false, "Please select the Tenant", false);
    //    return false;
    //}
    if (document.getElementById("txtModifyTenantBulk").value == "") {
        SelectedSuppliers = "";
    }

    $.ajax({
        type: "GET",
        url: "LocationHandlers/UpdateLocDataHandler.ashx?width=" + Width + "&height=" + Height + "&length=" + Length + "&maxweight=" + (Weight == "" ? null : Weight) + "&ismmok=" + IsMMOk + "&isFM=" + IsFM + "&isactive=" + IsActive + "&MCode=" + mmcode + "&locid=" + selectedlocation + "&IsQuarantine=" + IsQ + "&Tenant=" + Tenant + "&Supplier=" + SelectedSuppliers + "&rack=" + rack + "&col=" + col + "&lev=" + lev + "&zone=" + zone + "&ZoneID=" + ZoneID + "&LocType=" + LocType,

        contentType: "text/html",
        success: function (response) {
            LoadMap();
        },
        error: function (response) {
            showStickyToast(false, "Sorry some error occured", false);
        }
    });
}




function getSliderDialog() {
    debugger;
    $('#txtAisle, #txtRack, #txtColumn, #txtLevel, #txtBin, #txtSelectTenant, #txtSupplier').val('');
    $('#txtWidthB, #txtLengthB, #txtHeightB, #txtMaxWeightB').val(0);

    document.getElementById("txtSelectTenant").value = "";
    $('#cbxfastmove').prop('checked', false);
    //document.getElementById("txtSupplier").value = "";
    $('#txtSupplier').empty();
    ZoneID = $('#MainContent_MMContent_ddlLocationCode').val();
    if (ZoneID == null || ZoneID == 0) {
        showStickyToast(false, "Please select 'Zone'", false);
        return false;
    }
    if (selectedBins.length == 0) {
        // $("#locationcreatedialog").dialog('option', 'position', [300, 40]);
        document.getElementById("locationcreatedialog").style.visibility = "visible";
        $("#locationcreatedialog").dialog('open');
        //$("#locationcreatedialog").css("background-color", "lightblue");
        $("#locationcreatedialog").dialog('option', 'title', "Create New Locations");
        // document.getElementById("trSupplier").style.display = "none";
    } else {
        showStickyToast(false, "Deselect the locations first", false);
    }

}

function GetAddingDialog() {
    debugger;
    document.getElementById("txtSelectTenantAdd").value = "";
    $('#cbxfastmoveAdd').prop('checked', false);
    //document.getElementById("txtSupplier").value = "";
    WareHouseID = $('#MainContent_MMContent_ddlWarehouse').val();
    ZoneID = $('#MainContent_MMContent_ddlLocationCode').val();
    $('#txtSupplierAdd').empty();
    if (WareHouseID == 0) {
        debugger;
        showStickyToast(false, "Please select 'Warehouse'", false);
        return false;
    }
    if (ZoneID == 0 || ZoneID == null || ZoneID == undefined || ZoneID == "") {
        debugger;
        showStickyToast(false, "Please select 'Zone'", false);
        return false;
    }

    $('#txtColumnAdd').val('');
    $('#txtLevelAdd').val('');
    $('#txtBinAdd').val('');
    $('#txtWidthAdd').val('0');
    $('#txtLengthAdd').val('0');
    $('#txtHeightAdd').val('0');
    $('#txtSelectTenantAdd, #txtSupplierAdd').val('');
    $('#txtMaxWeightAdd').val('0');




    if (selectedBins.length == 0) {
        LoadRacks(ZoneID);
        // $("#locationcreatedialog").dialog('option', 'position', [300, 40]);
        document.getElementById("Addinglocationcreatedialog").style.visibility = "visible";
        $("#Addinglocationcreatedialog").dialog('open');
        //$("#Addinglocationcreatedialog").css("background-color", "lightblue");
        $("#Addinglocationcreatedialog").dialog('option', 'title', "Adding New Locations(Levels or Columns)");
        // document.getElementById("trSupplierAdd").style.display = "none";
    } else {
        showStickyToast(false, "Deselect the locations first", false);
    }
}



//function PrintData() {
//    debugger;

//    var printLocations = "";

//    printLocations = selectedBins.join(',');


//    if (printLocations == "") {
//        showStickyToast(false, "Please select location you want to print", false);
//        return;
//    }

//    var printerIP = $('#MainContent_MMContent_ddlPrinter').val();

//    if (printerIP == 0 || printerIP == null) {
//        showStickyToast(false, "Please select Printer", false);
//        return;
//    }
//    $.ajax({

//        url: "../mWebServices/FalconWebService.asmx/Print_Location",
//        data: "{  'location': '" + printLocations + "','printerIP':'" + printerIP + "'}",
//        dataType: "json",
//        type: "POST",
//        contentType: "application/json; charset=utf-8",
//        success: function (data) {


//        },
//        error: function (response) {
//            showStickyToast(false, response.text, false);

//        },
//        failure: function (response) {
//            showStickyToast(false, response.text, false);

//        }
//    });
//}

//For BulkPrint Dialog

function getSliderDialogForBulkPrint() {
    debugger;
    document.getElementById("txtSelectTenant").value = "";
    //document.getElementById("txtSupplier").value = "";
    $('#txtSupplier').empty();

    WareHouseID = $('#MainContent_MMContent_ddlWarehouse').val();
    ZoneID = $('#MainContent_MMContent_ddlLocationCode').val();
    if (WareHouseID == "0" || WareHouseID == null || WareHouseID == undefined || WareHouseID == "") {
        showStickyToast(false, " Please select Warehouse", false);
        return false;
    }

    if (ZoneID == "0" || ZoneID == null || ZoneID == undefined || ZoneID == "") {
        showStickyToast(false, "Please select Zone ", false);
        return false;
    }



    //var printerIPforbulkprint = $('#MainContent_MMContent_ddlPrinter').val();

    //if (printerIPforbulkprint == 0 || printerIPforbulkprint == null) {
    //    showStickyToast(false, "Please select Printer ", false);
    //    return;
    //}
    if (selectedBins.length == 0) {
        LoadRacksForPrint(ZoneID);
        // $("#locationcreatedialog").dialog('option', 'position', [300, 40]);
        //document.getElementById("printlocationdialog").style.visibility = "visible";
        //$("#printlocationdialog").dialog('open');
        ////$("#locationcreatedialog").css("background-color", "lightblue");
        //$("#printlocationdialog").dialog('option', 'title', "Print New Labels");
        // document.getElementById("trSupplier").style.display = "none";
    } else {
        showStickyToast(false, "Deselect the locations first", false);
    }

}



//======================= ZPL Printing Added By M.D.Prasad ===========================//

function PrintLocations() {
    debugger;

    var printLocations = "";

    printLocations = selectedBins.join(',');

    //var printerIPforbulkprint = $('#MainContent_MMContent_ddlPrinter').val();

    //if (printerIPforbulkprint == 0 || printerIPforbulkprint == null) {
    //    showStickyToast(false, "Please select 'Printer' ", false);
    //    return;
    //}

    if ($('#pid').val() == "3") {
        if ($("#netPrinterHost").val() == "") {
            showStickyToast(false, "Please enter Printer IP Address", false);
            return false;
        }
        if ($("#netPrinterPort").val() == "") {
            showStickyToast(false, "Please enter Printer Port", false);
            return false;
        }
    }
    if (printLocations == "") {
        showStickyToast(false, "Please select location you want to print", false);
        return;
    }

    $.ajax({
        url: "../mWebServices/FalconWebService.asmx/Print_Location_ZPL",
        //url: "../mWebServices/FalconWebService.asmx/Print_Location",  //Commeneted by M.D Prasad on 2-1-2019 //
        data: "{  'location': '" + printLocations + "','printerIP':'" + "" + "'}",
        //data: "{  'location': '" + printLocations + "','printerIP':'" + printerIPforbulkprint + "'}",//Commeneted by M.D Prasad on 2-1-2019 //
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            debugger;
            if (data.d == "") {
                //document.querySelector('#tbldatas').classList.remove("tableLoader");
                showStickyToast(false, "Error occured", false);
                return false;
            }
            else {
                $("#printerCommands").val("");
                $("#printerCommands").val(data.d);
                javascript: doClientPrint();
                // document.querySelector('#tbldatas').classList.remove("tableLoader");
                showStickyToast(true, "Successfully Printed", false);
            }

            //var dt = data.d;
            //if (dt != "") {
            //    //Commeneted by M.D Prasad on 2-1-2019 //$("#entered_name").val("");
            //    $("#entered_name").val(dt);
            //    sendData();
            //    showStickyToast(true, "Successfully printed", false);
            //    return false;
            //}
            //else
            //{
            //    showStickyToast(false, "Error while printing", false);
            //    return false;                
            //}
        },
        error: function (response) {
            showStickyToast(false, response.text, false);

        },
        failure: function (response) {
            showStickyToast(false, response.text, false);

        }
    });
}

//======================= END ====================================//


//Searching for Materilawiselocationns or department wise locations
function doSearch1() {
    debugger;
    var SearchElement = document.getElementById("selSearchCategory").value;

    if (SearchElement == 1 || SearchElement == 2) {
        LoadMap_Search(SearchElement);
    }
    else {
        showStickyToast(false, "Select search Category", false);
    }
}

function LoadMap_Search(type) {
    debugger;
    $('.spanBin').removeClass('TenantClass');
    $('.spanBin').removeClass('mCodeClass');
    if (type == 1) {
        var TenantID = document.getElementById('hidTenantID').value;
        if (TenantID == "") {
            showStickyToast(false, "Please select valid Tenant", false);
            return false;
        }
        $('.spanBin').each(function () {
            if ($(this).attr('data-tenantid') == TenantID) {
                $(this).addClass('TenantClass');
            }
            else {
                $(this).removeClass('TenantClass');
            }

        });
    }
    else if (type == 2) {
        //HUL14529á [inventrax] á [100.00] áá áá
        var MID = document.getElementById('hidTenantID').value;
        if (MID == "") {
            showStickyToast(false, "Please select valid MCode", false);
            return false;
        }
        var sMCode = document.getElementById('txtSearch').value.trim();
        $('.spanBin').each(function () {
            var bindata = $(this).attr('data-bindata');
            if (bindata != "") {

                var itemsArray = bindata.split('#');
                for (var i = 0; i < itemsArray.length; i++) {

                    if (itemsArray[i].split('@')[0].trim() == sMCode) {
                        $(this).addClass('mCodeClass');
                        //return false;
                    }
                    //else {
                    //    $(this).removeClass('mCodeClass');ur
                    //}
                }
            }
        });

    }
}

function GetQueryString(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}









//add by swamy on 08 nov 2019


function LoadLocationRackData() {
    var data = "{}";
    InventraxAjax.AjaxResultExecute("LocationManager.aspx/GetRackList", data, 'LoadLocationRackDataOnSuccess', 'LoadLocationRackDataOnError', null);
}

var selectedBins = [];
var selectedBinIDs = [];
function LoadLocationRackDataOnSuccess(data) {
    //debugger;
    var dt = data.Result;
    rackList = dt;
    $("#fromRackID").empty();
    for (var x = 0; x < dt.length; x++) {
        $("#fromRackID").append($("<option></option>").val(dt[x].ID).html(dt[x].Value));
    }

    $("#toRackID").empty();
    for (var i = 0; i < dt.length; i++) {
        $("#toRackID").append($("<option></option>").val(dt[i].ID).html(dt[i].Value));
    }
    UnBlockDialog();

}
function LoadLocationRackDataOnError(data) {
    alert("Unable to load Rack Data");
}


function LoadLocationLevelData() {
    var data = "{}";
    InventraxAjax.AjaxResultExecute("LocationManager.aspx/GetBayList", data, 'LoadLocationLevelDataOnSuccess', 'LoadLocationLevelDataOnError', null);
}

var selectedBins = [];
var selectedBinIDs = [];
function LoadLocationLevelDataOnSuccess(data) {
    //debugger;
    var dt = data.Result;
    levelList = dt;
    $("#fromLevelID").empty();
    for (var x = 0; x < dt.length; x++) {
        $("#fromLevelID").append($("<option></option>").val(dt[x].ID).html(dt[x].Value));
    }

    $("#toLevelID").empty();
    for (var i = 0; i < dt.length; i++) {
        $("#toLevelID").append($("<option></option>").val(dt[i].ID).html(dt[i].Value));
    }

    UnBlockDialog();

}
function LoadLocationLevelDataOnError(data) {
    alert("Unable to load Level Data");
}


function LoadLocationColumnData() {
    var data = "{}";
    InventraxAjax.AjaxResultExecute("LocationManager.aspx/GetColumnList", data, 'LoadLocationColumnDataOnSuccess', 'LoadLocationColumnDataOnError', null);
}

var selectedBins = [];
var selectedBinIDs = [];
function LoadLocationColumnDataOnSuccess(data) {
    //debugger;
    var dt = data.Result;
    colList = dt;

    $("#fromColumnID").empty();
    for (var x = 0; x < dt.length; x++) {
        $("#fromColumnID").append($("<option></option>").val(dt[x].ID).html(dt[x].Value));
    }

    $("#toColumnID").empty();
    for (var i = 0; i < dt.length; i++) {
        $("#toColumnID").append($("<option></option>").val(dt[i].ID).html(dt[i].Value));
    }
    UnBlockDialog();

}
function LoadLocationColumnDataOnError(data) {
    alert("Unable to load Level Data");
}



function LoadLocationBinData() {
    var data = "{}";
    InventraxAjax.AjaxResultExecute("LocationManager.aspx/GetBinList", data, 'LoadLocationBinDataOnSuccess', 'LoadLocationBinDataOnError', null);
}

var selectedBins = [];
var selectedBinIDs = [];
function LoadLocationBinDataOnSuccess(data) {
    //debugger;
    var dt = data.Result;
    binListItems = dt;
    $("#fromBinID").empty();
    for (var x = 0; x < dt.length; x++) {
        $("#fromBinID").append($("<option></option>").val(dt[x].ID).html(dt[x].Value));
    }

    $("#toBinID").empty();
    for (var i = 0; i < dt.length; i++) {
        $("#toBinID").append($("<option></option>").val(dt[i].ID).html(dt[i].Value));
    }
    UnBlockDialog();

}
function LoadLocationBinDataOnError(data) {
    alert("Unable to load Level Data");
}



//add by swamy on 08 nov 2019




