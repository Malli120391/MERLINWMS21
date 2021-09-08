<%@ Page Title="Item Master" Language="C#" MasterPageFile="~/mMaterialManagement/MaterialManagementMaster.master" AutoEventWireup="true" CodeBehind="ItemMasterRequest.aspx.cs" Inherits="MRLWMSC21.mMaterialManagement.ItemMasterRequest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
    <asp:ScriptManager ID="mySManager" runat="server" EnablePartialRendering="true" SupportsPartialRendering="true" />
    <script src="Scripts/jquery-ui-1.8.24.min.js"></script>
    <script type="text/javascript" src="Scripts/CommonScripts.js"></script>
    <link href="../CSS/bootstrap.min.css" rel="stylesheet" />
    <%--<script src="Scripts/bootstrap.js"></script>--%>
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>
    <link href="../Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
    <script src="../Scripts/toast/jquery.toastmessage.js"></script>
    <script src="FileUploader/jquery.uploadfile.min.js" type="text/javascript"></script>
    <link href="FileUploader/uploadfile.min.css" rel="stylesheet" type="text/css" />
    <%--<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-cookie/1.4.1/jquery.cookie.min.js" type="text/javascript"></script>--%>
    <script src="../Scripts/CommonWMS.js"></script>
    <script>
        var MaterialMasterID = 0;
        //var BasicData = "";
        var BasicObj = {};
        var Tenant;
        var ItemMasterList = null;
        var ItemMasterDrop = null;
        var MaterialData = null;
        //var BinLocations = null;
        var BinData = null;







        $(document).ready(function ()
        {

            //debugger;
            $('#collapseTwo, #collapseThree, #collapseFour, #collapseFive, #collapseSix, #collapseSeven, #collapseEight, #collapseNine, #collapseTen').addClass('ui-state-disabled');
            var copymid = new URL(window.location.href).searchParams.get("edittype");
            if (copymid == "copy") {
                $('#collapseTwo, #collapseThree, #collapseFour, #collapseFive, #collapseSix, #collapseSeven, #collapseEight, #collapseNine, #collapseTen').addClass('ui-state-disabled');
            }
            MaterialMasterID = new URL(window.location.href).searchParams.get("mid");
            if (MaterialMasterID != null && (copymid != "copy" || copymid == null)) {
                $('#collapseTwo, #collapseThree, #collapseFour, #collapseFive, #collapseSix, #collapseSeven, #collapseEight, #collapseNine, #collapseTen').collapse({ 'toggle': true });
                // $('.accordrot').addClass('rotate');
                $('#collapseTwo, #collapseThree, #collapseFour, #collapseFive, #collapseSix, #collapseSeven, #collapseEight, #collapseNine, #collapseTen').removeClass('ui-state-disabled');
            }
            GetItemMasterDrop();
            //if (MaterialMasterID != null) {
            GetItemMasterDetails(MaterialMasterID);
            //}
            


            $("#divItemPrintData").dialog(
                {
                    autoOpen: false,
                    minHeight: 20,
                    height: '400',
                    width: '500',
                    modal: true,
                    resizable: false,
                    draggable: false,
                    overflow: "auto",
                    position: ["center top", 40],
                    open: function () {
                        $(".ui-dialog").hide().fadeIn(500);

                        $('body').css({ 'overflow': 'hidden' });
                        $('body').width($('body').width());

                        $(document).bind('scroll', function () {
                            window.scrollTo(0, 0);
                        });
                    },
                    close: function () {

                        $(".ui-dialog").fadeOut(500);
                        $(document).unbind('scroll');
                        $('body').css({ 'overflow': 'visible' });

                    }


                });

        });

        //OnKeyPress disable 0 and -ve values
        function myValidate() {
            if (event.which == 45 || event.which == 48) {
                event.preventDefault();
            }
        }

        //OnKeyPress disable only -ve values
        function myNegValidate() {
            if (event.which == 45) {
                event.preventDefault();
            }
        }

        //OnKeyPress disable decimal
        function myDecimal() {
            if (event.which == 110) {
                event.preventDefault();
            }
        }        

        <%--function fnScroll() {
            // alert("hi");
            //$(window).scrollTop(1200);
            document.getElementById('<%=this.ddlMeadureType.ClientID %>').scrollTop = 100;
        }--%>

        //OnChange Material Type check Checkboxes && IsRequired check Checkboxes
        $(function () {
            $("#MTypeID").on('change', function () {
                debugger;

                $('#msp1').prop('checked', false);
                $('#msp2').prop('checked', false);
                $("#minshel, #maxshel").hide()

                var val = $("#MTypeID option:selected").text();

                var fields = val.split('-');

                var a1 = fields[0];
                var a2 = fields[1].trim();

                if (a2 == "Perishable")
                {
                            $('#msp1').prop('checked', true);
                            $('#msp2').prop('checked', true);
                            $("#minshel, #maxshel").show()
                }
 
                else
                {
                    $('#msp1').prop('checked', false);
                    $('#msp2').prop('checked', false);
                    $("#minshel, #maxshel").hide();
                }


            });

            $("#suppartnum, #MCode, #OEMPartNo, #MCodeAlternative1, #MCodeAlternative2").keypress(function (e) {
                var regex = new RegExp("^[a-zA-Z0-9]+$");
                var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
                if (regex.test(str)) {
                    return true;
                }

                e.preventDefault();
                return false;
            });

            forceNumber($("#minTol"));
            forceNumber($("#maxTol"));

            $("#WarehouseID").on('change', function () {
                $(".matstock").show();
            });


            //$('#Account').on('change', function () {
            //    //debugger;
            //    var AccountID = $('#Account').val();
            //    if (AccountID != "0") {
            //        var obj = $.grep(ItemMasterDrop.Table, function (a) { return a.AccountID == AccountID });
            //    }
            //    else {
            //        var obj = ItemMasterDrop.Table;
            //    }
            //    $("#Tenant").empty().append('<option selected="selected" value="PS">select</option>');
            //    for (var i = 0; i < obj.length; i++) {
            //        $("#Tenant").append($("<option></option>").val(obj[i].TenantID).html(obj[i].TenantName));
            //    }
            //});

            $("#<%=fuItemPicture.ClientID %>").change(function () {
                //alert();
                var fileExtension = ['jpeg', 'jpg', 'png', 'gif', 'bmp'];
                if ($.inArray($(this).val().split('.').pop().toLowerCase(), fileExtension) == -1) {
                    showStickyToast(false, "Only formats are allowed : " + fileExtension.join(', '), false);
                }
            });

            $("#ddlUoM").change(function () {
                var ddlUoMvalue = $(this).val().trim();
                if (ddlUoMvalue == "1") {
                    $("#QtyPerUoM").val("1");
                }
            });

            $("#MCode").blur(function () {
                //debugger;
                var MCodeNumber = $(this).val().trim();
                var Tenantid = $("#Tenant").val().trim();
               // if (MaterialMasterID == null || MaterialMasterID == 0) {
                    //debugger;
                if (MCodeNumber != "") {

                    $.ajax({
                        url: '<%=ResolveUrl("~/mMaterialManagement/ItemMasterRequest.aspx/PartCheck") %>',
                        data: "{'TenantID' : '" + Tenantid + "','MCode' : '" + MCodeNumber + "'}",
                        dataType: "json",
                        type: "POST",
                        async: false,
                        contentType: "application/json; charset=utf-8",
                        success: function (response) {
                            //alert(response.d);
                            var check = response.d;
                            if (check == "Exists") {
                                $('#partno').show();
                                $('#partyes').hide();
                                showStickyToast(false, "Part Number Already Exists", false);
                                return false;
                            }
                            else {
                                $('#partyes').show();
                                $('#partno').hide();
                            }
                        }
                    });
                    //    var CheckPartNum = $.grep(ItemMasterDrop.Table12, function (a) { return a.MCode == MCodeNumber && a.TenantID == Tenantid });
                    //    if (CheckPartNum.length != 0) {
                    //        $('#partno').show();
                    //        $('#partyes').hide();
                    //        showStickyToast(false, "Part Number Already Exists", false);
                    //        return false;
                    //    }
                    //    else {
                    //        $('#partyes').show();
                    //        $('#partno').hide();
                    //    }
                    //}
                    //else {
                        //showStickyToast(false, "Please Enter Part Number To Check", false);
                    //}
                }
            });


            //$("#isrequired").click(function () {
            //    //alert();
            //    $('.allcheck input:checkbox').not(this).prop('checked', this.checked);
            //});
            ////debugger;
            //$('#SupModal').on('hidden.bs.modal', '.modal', function () {
            //    $(this).removeData('bs.modal');
            //});

            //Tenants Auto Dropdown
            //var tenantfield = $('#Tenantss');
            //DropdownFunction(tenantfield);
            //$('#Tenantss').autocomplete({
            //    source: function (request, response) {

            //        var treg = $('#Tenantss').val().trim();
            //        var tData = $.grep(Tenants, function (a) { return (a.TenantName.toLowerCase()).indexOf(treg.toLowerCase()) > -1 });
            //        //alert(reg);
            //        response($.map(tData, function (item) {
            //            //debugger;
            //            return {
            //                label: item.TenantName,
            //                val: item.TenantID
            //            }
            //        }))
            //    },
            //    select: function (e, i) {
            //        $("#Tenantss").val(i.item.label);
            //        $("#Tenant").val(i.item.val);                    
            //    },
            //    minLength: 0
            //});

            //Bin Location Auto Dropdown

            var binlocfield = $('#LocationText');
            DropdownFunction(binlocfield);
            $("#LocationText").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadLoction_AutoForBin") %>',
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
                    $("#LocationID").val(i.item.val);
                    $("#LocationText").val(i.item.label);
                    $(".binstock").show();
                },
                minLength: 0
            });

            //$('#LocationText').autocomplete({
            //    source: function (request, response) {
            //        console.log(BinLocations);
            //        var blreg = $('#LocationText').val().trim();
            //        var blData = $.grep(BinLocations, function (a) { return (a.Location.toLowerCase()).indexOf(blreg.toLowerCase()) > -1 });
            //        //alert(reg);
            //        response($.map(blData, function (item) {
            //            //debugger;
            //            return {
            //                label: item.Location,
            //                val: item.LocationID
            //            }
            //        }))
            //    },
            //    select: function (e, i) {
            //        $("#LocationID").val(i.item.val);
            //        $("#LocationText").val(i.item.label);
            //    },
            //    minLength: 0
            //});            

            //Account Autocomplete Dropdown
            var accountfield = $('#txtAccount');            
            DropdownFunction(accountfield);
            $("#txtAccount").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadAccountForCyccleCount") %>',
                        data: "{ 'prefix': '" + request.term + "', 'AccountID': '" +  <%=this.cp.AccountID%> + "'}",
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
                    //debugger
                    $("#Account").val(i.item.val);
                    $("#txtAccount").val(i.item.label);

                    var AccountIDT = $("#Account").val();
                    LoadTenantBasedAcc(AccountIDT);
                    //var obj = [];

                    //if ($("#Account").val() == "0") {
                    //    obj = ItemMasterDrop.Table;
                    //}
                    //else {
                    //    obj = $.grep(ItemMasterDrop.Table, function (a) { return a.AccountID == $("#Account").val() });
                    //}

                    //$("#Tenant").empty().append('<option selected="selected" value="PS">Select</option>');
                    //for (var i = 0; i < obj.length; i++) {
                    //    $("#Tenant").append($("<option></option>").val(obj[i].TenantID).html(obj[i].TenantName));
                    //}


                    <%--var AccountID = <%=this.cp.AccountID%>;
                    $("#Tenant").empty().append('<option selected="selected" value="PS">Select</option>');
                    if (AccountID != "0") {
                        var obj = $.grep(ItemMasterDrop.Table, function (a) { return a.AccountID == $("#Account").val() });
                    }
                    else {
                        var obj = ItemMasterDrop.Table;
                    }
                    for (var i = 0; i < obj.length; i++) {
                        $("#Tenant").append($("<option></option>").val(obj[i].TenantID).html(obj[i].TenantName));
                    }
                    if (Tenantsids != 0) {
                        $("#Tenant").val(<%=this.cp.TenantID%>);
                        $('#Tenant').attr("disabled", true);
                        $("#Tenant").css("background-color", "#ebebe4");
                    }--%>

                },
                minLength: 0
            });

           // On Change To Measurement
            $('#ddlFromMeasure').on('change', function () {
                debugger;
                UoMConvert();
            });


            //On Change Select in Measurement Type
            $('#ddlMeasure').on('change', function () {
                debugger;
                var MeasurementID = $(this).val();
                //if (MeasurementID != "0") {
                $.ajax({
                    url: '<%=ResolveUrl("~/mMaterialManagement/ItemMasterRequest.aspx/Build_FromMeasurement") %>',
                    data: "{'MeasurementID' : '" + MeasurementID + "'}",
                    dataType: "json",
                    type: "POST",
                    async: false,
                    contentType: "application/json; charset=utf-8",
                    success: function (response) {
                        //alert(response.d);
                        debugger;
                        var MeasuresDrop = JSON.parse(response.d);
                        $("#ddlFromMeasure, #ddlToMeasure").empty().append('<option selected="selected" value="PS">select</option>');
                        for (var i = 0; i < MeasuresDrop.Table.length; i++) {
                            $("#ddlFromMeasure, #ddlToMeasure").append($("<option></option>").val(MeasuresDrop.Table[i].MeasurementID).html(MeasuresDrop.Table[i].Measurement));
                        }
                        $(".measures").hide();
                        $(".uomdiv").hide();
                    }
                });
                //}
                // else {
                //$(".measures").hide();
                //}   
                if (MeasurementID == "0") {
                    $("#UoMConversion").attr('disabled', true);
                    $("#UoMConversion").css("cursor", "no-drop");
                }
                else {
                    $("#UoMConversion").attr('disabled', false);
                    $("#UoMConversion").css("cursor", "pointer");
                }
            })

            // On Change To Measurement
            $('#ddlToMeasure').on('change', function () {
                UoMConvert();
            });

            $("#SupModal").on('show.bs.modal', function () {
                mySupclear();
            });
            $("#UoMModal").on('show.bs.modal', function () {
                //debugger;
                myUoMclear();
                var MeasurementID = $("#ddlMeasure").val();
                //Get UoM Dropdown
                var objUoM = null;
                if (MeasurementID != 0) {
                    objUoM = $.grep(ItemMasterDrop.Table8, function (a) { return a.MeasurementTypeID == MeasurementID });
                }
                else {
                    objUoM = $.grep(ItemMasterDrop.Table8, function (a) { return a.MeasurementTypeID == null });
                }

                $("#UoM").empty().append('<option selected="selected" value="PS">select</option>');
                for (var j = 0; j < objUoM.length; j++) {
                    $("#UoM").append($("<option></option>").val(objUoM[j].UoMID).html(objUoM[j].UoM));
                }

            });
            $("#QcModal").on('show.bs.modal', function () {
                myQCclear();
            });



        });

        function LoadTenantBasedAcc(id) {
            var tenantfield = $('#txtTenant');
            DropdownFunction(tenantfield);
            $("#txtTenant").autocomplete({
                source: function (request, response) {
                    debugger;
                    if ($("#txtTenant").val() == '') {
                        $("#Tenant").val(0) ;
                    }
                    $("#Tenant").val(0) ;
                
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadTenantsForWHList") %>',
                        data: "{ 'prefix': '" + request.term + "', 'AccountID': '" + id + "'}",
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
                    //debugger;
                    $("#Tenant").val(i.item.val);
                    $("#txtTenant").val(i.item.label);
                    var TntID = $("#Tenant").val();
                    LoadMGroupBasedTenant(TntID);
                    LoadMaterialTypeBasedTenant(TntID);
                    LoadStorageCBasedTenant(TntID);
                    //var TenantIDs = $("#Tenant").val();
                    //LoadSupplier(TenantIDs);
                },
                minLength: 0
            });
        }

        //MGroup Dropdown Based on Tenant Selected
        function LoadMGroupBasedTenant(id)
        {
            var mgroupfield = $('#MGroup');
            DropdownFunction(mgroupfield);
            $("#MGroup").autocomplete({

                source: function (request, response) {
                    debugger;
                    if ($("#MGroup").val() == '') {
                        $("#MGroupID").val(0);
                    }
                      $("#MGroupID").val(0);
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadMaterialGroupDataItem") %>',
                        data: "{ 'prefix': '" + request.term + "', 'TenantID' : '" + id + "'}",
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
                    $("#MGroupID").val(i.item.val);
                    $("#MGroup").val(i.item.label);
                },
                minLength: 0
            });
        }

        //Accept Only one '-' and '.' with only 2 digits after decimal
        function forceNumber(element) {
            //debugger;
            element
                .data("oldValue", '')
                .bind("paste", function (e) {
                    var validNumber = /^[-]?\d+(\.\d{1,2})?$/;
                    element.data('oldValue', element.val())
                    setTimeout(function () {
                        if (!validNumber.test(element.val()))
                            element.val(element.data('oldValue'));
                    }, 0);
                });
            element
                .keypress(function (event) {
                    var text = $(this).val();
                    if ((event.which != 46 || text.indexOf('.') != -1) && //if the keypress is not a . or there is already a decimal point
                        ((event.which < 48 || event.which > 57) && //and you try to enter something that isn't a number
                            (event.which != 45 || (element[0].selectionStart != 0 || text.indexOf('-') != -1)) && //and the keypress is not a -, or the cursor is not at the beginning, or there is already a -
                            (event.which != 0 && event.which != 8))) { //and the keypress is not a backspace or arrow key (in FF)
                        event.preventDefault(); //cancel the keypress
                    }

                    if ((text.indexOf('.') != -1) && (text.substring(text.indexOf('.')).length > 2) && //if there is a decimal point, and there are more than two digits after the decimal point
                        ((element[0].selectionStart - element[0].selectionEnd) == 0) && //and no part of the input is selected
                        (element[0].selectionStart >= element.val().length - 2) && //and the cursor is to the right of the decimal point
                        (event.which != 45 || (element[0].selectionStart != 0 || text.indexOf('-') != -1)) && //and the keypress is not a -, or the cursor is not at the beginning, or there is already a -
                        (event.which != 0 && event.which != 8)) { //and the keypress is not a backspace or arrow key (in FF)
                        event.preventDefault(); //cancel the keypress
                    }
                });
        }



        function LoadSupplier(TenantID) {
            //debugger;
            //if (TenantID != null) {
            //    var obj = $.grep(ItemMasterDrop.Table19, function (a) { return a.TenantID == TenantID });
            //    if (obj != null && obj.length > 0) {
            //        $("#ddlSupplier").empty().append('<option selected="selected" value="PS">Select</option>');
            //        for (var j = 0; j < obj.length; j++) {
            //            $("#ddlSupplier").append($("<option></option>").val(obj[j].SupplierID).html(obj[j].SupplierName));
            //        }
            //    }
            //}

            //debugger;
            var supfield = $('#Supname');
            DropdownFunction(supfield);
            $("#Supname").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadSupplierDataBasedTenant") %>',
                        data: "{ 'prefix': '" + request.term + "', 'TenantID': '" + $("#Tenant").val() + "'}",
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
                    $("#ddlSupplier").val(i.item.val);
                    $("#Supname").val(i.item.label);
                },
                minLength: 0
            });

        }

        function LoadWarehouse(TenantID) {
            //debugger;
            if (TenantID != null) {
                var obj = $.grep(ItemMasterDrop.Table5, function (a) { return a.TenantID == TenantID });
                $("#WarehouseID").empty().append('<option selected="selected" value="PS">Select</option>');
                for (var j = 0; j < obj.length; j++) {
                    $("#WarehouseID").append($("<option></option>").val(obj[j].WarehouseID).html(obj[j].WarehouseLoc));
                }
            }
        }

        function LoadMaterialTypeBasedTenant(id)
        {
            //debugger;
            if (id != null) {
                var obj = $.grep(ItemMasterDrop.Table, function (a) { return a.TenantID == id });
                if (obj != null && obj.length > 0) {
                    $("#MTypeID").empty().append('<option selected="selected" value="PS">Select</option>');
                    for (var j = 0; j < obj.length; j++) {
                        $("#MTypeID").append($("<option></option>").val(obj[j].MTypeID).html(obj[j].MTypeDesc));
                    }
                }
            }
        }

        function LoadStorageCBasedTenant(id) {
            //debugger;
            if (id != null) {
                var obj = $.grep(ItemMasterDrop.Table1, function (a) { return a.TenantID == id });
                if (obj != null && obj.length > 0) {
                    $("#StorageConditionID").empty().append('<option selected="selected" value="PS">Select</option>');
                    for (var j = 0; j < obj.length; j++) {
                        $("#StorageConditionID").append($("<option></option>").val(obj[j].StorageConditionID).html(obj[j].StorageCondition));
                    }
                }
            }
        }

        //function LoadSupplierAttach() {
        //    var obj = $.grep(ItemMasterDrop.Table20, function (a) { return a.MaterialMasterID == MaterialMasterID });
        //    if (obj != null && obj.length > 0) {
        //        $("#ddlSupplierID").empty().append('<option selected="selected" value="PS">Please select</option>');
        //        for (var j = 0; j < obj.length; j++) {
        //            $("#ddlSupplierID").append($("<option></option>").val(obj[j].SupplierID).html(obj[j].SupplierName));
        //        }
        //    }
        //}

        function LoadSupplierAttach() {
            var obj = $.grep(ItemMasterList.Table3, function (a) { return a.MaterialMasterID == MaterialMasterID });
            if (obj != null && obj.length > 0) {
                $("#ddlSupplierID").empty().append('<option selected="selected" value="PS">Select</option>');
                for (var j = 0; j < obj.length; j++) {
                    $("#ddlSupplierID").append($("<option></option>").val(obj[j].SupplierID).html(obj[j].SupplierName));
                }
            }
        }

        //Get Item Master Dropdowns 
        var Tenants = null;
        var Plants = null;

        function GetItemMasterDrop()
        {
            debugger;

            $.ajax({
                url: '<%=ResolveUrl("~/mMaterialManagement/ItemMasterRequest.aspx/GetItemMasterLoad") %>',
                data: "{'TenantID' : '" +<%=this.cp.TenantID%>+"','AccountID':'" +<%=this.cp.AccountID%>+"'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                async: false,
                success: function (response)
                {
                    ItemMasterDrop = JSON.parse(response.d);
                    //Get Account Dropdown
                    //debugger;
                    //$("#Account").empty().append('<option selected="selected" value="PS">Select</option>');
                    //for (var i = 0; i < ItemMasterDrop.Table21.length; i++) {
                    //    $("#Account").append($("<option></option>").val(ItemMasterDrop.Table21[i].AccountID).html(ItemMasterDrop.Table21[i].Account));
                    //}
                    //$("#Account").val(<%=this.cp.AccountID%>);

                    if (<%=this.cp.AccountID%> == 0 || <%=this.cp.AccountID%> == null) {
                        $('#txtAccount').attr("disabled", false);
                    }
                    else {
                        //debugger;
                        $('#txtAccount').attr("disabled", true);
                        $("#txtAccount").css("background-color", "#ebebe4");
                        $("#txtAccount").val("<%=this.cp.Account%>");
                        $("#Account").val(<%=this.cp.AccountID%>);
                        var acid = $("#Account").val();
                        LoadTenantBasedAcc(acid);
                    }

                    var AccountID = <%=this.cp.AccountID%>; 
                    var Tenantsids = <%=this.cp.TenantID%>; 



                    //Get Tenant Dropdown
                    //debugger;

                   <%--====== 26/03/2018 $("#Tenant").empty().append('<option selected="selected" value="PS">Select</option>');
                    if (AccountID != "0") {
                        var obj = $.grep(ItemMasterDrop.Table, function (a) { return a.AccountID == AccountID });
                    }
                    else {
                        var obj = ItemMasterDrop.Table;
                    }
                    for (var i = 0; i < obj.length; i++) {
                        $("#Tenant").append($("<option></option>").val(obj[i].TenantID).html(obj[i].TenantName));
                    }
                    if (Tenantsids != 0) {
                        $("#Tenant").val(<%=this.cp.TenantID%>);
                        $('#Tenant').attr("disabled", true);
                        $("#Tenant").css("background-color", "#ebebe4");
                    }--%>
                    //Tenants = ItemMasterDrop.Table;

                    //Get Plant Dropdown
                    //$("#MPlantID").empty().append('<option selected="selected" value="PS">Select</option>');
                    //for (var j = 0; j < ItemMasterDrop.Table.length; j++) {
                    //    $("#MPlantID").append($("<option></option>").val(ItemMasterDrop.Table[j].MPlantID).html(ItemMasterDrop.Table[j].Plant));
                    //}
                    //Plants = ItemMasterDrop.Table1;

                    //Get Material Group Autocomplete Dropdown
                    //Autocomplete Dropdown for Material Group


                    //Get Material Type Dropdown
                    //$("#MTypeID").empty().append('<option selected="selected" value="PS">Select</option>');
                    //for (var j = 0; j < ItemMasterDrop.Table.length; j++) {
                    //    $("#MTypeID").append($("<option></option>").val(ItemMasterDrop.Table[j].MTypeID).html(ItemMasterDrop.Table[j].MTypeDesc));
                    //}
                    //Get Storage Dropdown
                    //$("#StorageConditionID").empty().append('<option selected="selected" value="PS">Select</option>');
                    //for (var j = 0; j < ItemMasterDrop.Table1.length; j++) {
                    //    $("#StorageConditionID").append($("<option></option>").val(ItemMasterDrop.Table1[j].StorageConditionID).html(ItemMasterDrop.Table1[j].StorageCondition));
                    //}
                    //Get SpaceUtilization Dropdown
                    //$("#MaterialSpaceUtilizationID").empty().append('<option selected="selected" value="PS">Select</option>');
                    //for (var j = 0; j < ItemMasterDrop.Table4.length; j++) {
                    //    $("#MaterialSpaceUtilizationID").append($("<option></option>").val(ItemMasterDrop.Table4[j].SpaceUtilizationID).html(ItemMasterDrop.Table4[j].SpaceUtilization));
                    //}
                    //Get Product Categories Dropdown
                    $("#ProductCategoryID").empty().append('<option selected="selected" value="">Select Category</option>');
                    for (var j = 0; j < ItemMasterDrop.Table2.length; j++) {
                        $("#ProductCategoryID").append($("<option></option>").val(ItemMasterDrop.Table2[j].ProductCategoryID).html(ItemMasterDrop.Table2[j].ProductCategory));
                    }

                    //Get Measurement Dropdown
                    $("#ddlMeasureType").empty().append('<option selected="selected" value="PS">Select</option>');
                    for (var j = 0; j < ItemMasterDrop.Table3.length; j++) {
                        $("#ddlMeasureType").append($("<option></option>").val(ItemMasterDrop.Table3[j].MeasurementTypeID).html(ItemMasterDrop.Table3[j].MeasurementTypeName));
                    }

                    //Get Supplier Dropdown
                    //$("#ddlSupplierID").empty().append('<option selected="selected" value="PS">Select</option>');
                    //for (var j = 0; j < ItemMasterDrop.Table7.length; j++) {
                    //    $("#ddlSupplierID").append($("<option></option>").val(ItemMasterDrop.Table7[j].SupplierID).html(ItemMasterDrop.Table7[j].SupplierName));
                    //}

                    //Get Supplier Dropdown PopUp


                    //Get Attachment type Dropdown
                    $("#ddlAttachType").empty().append('<option selected="selected" value="PS">Select</option>');
                    for (var j = 0; j < ItemMasterDrop.Table4.length; j++) {
                        $("#ddlAttachType").append($("<option></option>").val(ItemMasterDrop.Table4[j].FileTypeID).html(ItemMasterDrop.Table4[j].FileType));
                    }

                    //Get Industry Dropdown
                    $("#ddlIndustry").empty().append('<option selected="selected" value="PS">Select</option>');
                    for (var j = 0; j < ItemMasterDrop.Table9.length; j++) {
                        $("#ddlIndustry").append($("<option></option>").val(ItemMasterDrop.Table9[j].IndustryID).html(ItemMasterDrop.Table9[j].IndustryName));
                    }

                    //Get Warehouse Dropdown                    
                    //$("#WarehouseID").empty().append('<option selected="selected" value="PS">Select</option>');
                    //for (var j = 0; j < ItemMasterDrop.Table10.length; j++) {
                    //    $("#WarehouseID").append($("<option></option>").val(ItemMasterDrop.Table10[j].WarehouseID).html(ItemMasterDrop.Table10[j].WarehouseLoc));
                    //}

                    //Get Currency Dropdown
                    $("#ddlCurrency").empty().append('<option selected="selected" value="PS">Select</option>');
                    for (var j = 0; j < ItemMasterDrop.Table6.length; j++) {
                        $("#ddlCurrency").append($("<option></option>").val(ItemMasterDrop.Table6[j].CurrencyID).html(ItemMasterDrop.Table6[j].Currency));
                    }

                    //Get UoMType Dropdown
                    //if (ItemMasterList.Table5 != null && ItemMasterList.Table5.length > 0) {
                    //    $("#ddlUoM").empty().append('<option selected="selected" value="PS">Select</option>');
                    //    for (var j = 0; j < ItemMasterDrop.Table12.length; j++) {
                    //        $("#ddlUoM").append($("<option></option>").val(ItemMasterDrop.Table12[j].UoMTypeID).html(ItemMasterDrop.Table12[j].UoMType));
                    //    }
                    //}
                    //else
                    //{
                    //    $("#ddlUoM").empty().append('<option selected="selected" value="PS">Select</option>');
                    //    for (var j = 0; j < 1; j++) {
                    //        $("#ddlUoM").append($("<option></option>").val(ItemMasterDrop.Table12[j].UoMTypeID).html(ItemMasterDrop.Table12[j].UoMType));
                    //    }
                    //}
                    //var obj = $.grep(ItemMasterList.Table18, function (a) { return a.MaterialMasterID == MaterialMasterID });

                    //Get UoM Dropdown
                    //$("#UoM").empty().append('<option selected="selected" value="PS">Select</option>');
                    //for (var j = 0; j < ItemMasterDrop.Table13.length; j++) {
                    //    $("#UoM").append($("<option></option>").val(ItemMasterDrop.Table13[j].UoMID).html(ItemMasterDrop.Table13[j].UoM));
                    //}

                    //Get Inward Parameters Dropdown
                    $("#ddlParameter").empty().append('<option selected="selected" value="PS">Select</option>');
                    for (var j = 0; j < ItemMasterDrop.Table9.length; j++) {
                        $("#ddlParameter").append($("<option></option>").val(ItemMasterDrop.Table9[j].QualityParameterID).html(ItemMasterDrop.Table9[j].ParameterName));
                    }

                    //Get Bin Location Dropdown
                    //$("#LocationID").empty().append('<option selected="selected" value="PS">Select</option>');
                    //for (var j = 0; j < ItemMasterDrop.Table15.length; j++) {
                    //    $("#LocationID").append($("<option></option>").val(ItemMasterDrop.Table15[j].LocationID).html(ItemMasterDrop.Table15[j].Location));
                    //}
                    //debugger;
                    //BinLocations = ItemMasterDrop.Table10;
                    //console.log(BinLocations);

                    //MSP Data Bind
                    mspContent = '';
                    mspContainer = document.getElementById("divMsps");
                    for (var i = 0; i < ItemMasterDrop.Table11.length; i++) {
                        mspContent += '<div class="checkbox"><input type="checkbox" class="fieldtogetMsps" id="msp' + ItemMasterDrop.Table11[i].MaterialStorageParameterID + '" data-attr-id="0" data-attr-isactive="1" data-attr-isdeleted="0"/><label for="msp' + ItemMasterDrop.Table11[i].MaterialStorageParameterID + '">' + ItemMasterDrop.Table11[i].ParameterName + '</label></div>';
                        mspContent += '&emsp;'
                    }
                    mspContainer.innerHTML = mspContent;

                }
            });
        }



        var uomcheck = null;
        //Get ItemMasterDetails on Edit
        function GetItemMasterDetails(MaterialMasterID) {
            if (MaterialMasterID != null)
            {
                debugger;
                $.ajax({
                    url: '<%=ResolveUrl("~/mMaterialManagement/ItemMasterRequest.aspx/GetItemMasterDetails") %>',
                    data: "{'MMID' : '" + MaterialMasterID + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: false,
                    success: function (response) {
                        debugger;
                        ItemMasterList = JSON.parse(response.d);
                        //console.log(ItemMasterList);
                        //Basic Material Details
                        $("#MCode").val(ItemMasterList.Table[0].MCode);
                    <%--$("#<%= this.hdnPartNum.ClientID %>").val(ItemMasterList.Table[0].MCode);--%>
                        $("#txtAccount").val(ItemMasterList.Table[0].Account);
                        $("#Account").val(ItemMasterList.Table[0].AccountID);
                        $('#txtAccount').attr("disabled", true);
                        //$("#Tenant").empty().append('<option selected="selected" value="PS">Select</option>');
                        //if ($("#Account").val() != "0") {
                        //    var obj = $.grep(ItemMasterDrop.Table, function (a) { return a.AccountID == $("#Account").val() });
                        //}
                        //else {
                        //    var obj = ItemMasterDrop.Table;
                        //}

                        //for (var i = 0; i < obj.length; i++) {
                        //    $("#Tenant").append($("<option></option>").val(obj[i].TenantID).html(obj[i].TenantName));
                        //}
                        var Accidt = $("#Account").val();
                        LoadTenantBasedAcc(Accidt)
                        $("#txtTenant").val(ItemMasterList.Table[0].TenantName);
                        $("#Tenant").val(ItemMasterList.Table[0].TenantID);
                        $('#txtTenant').attr("disabled", true);
                        $("#txtTenant").css("background-color", "#ebebe4");
                    <%--$("#<%= this.hifTenant.ClientID %>").val(ItemMasterList.Table[0].TenantID);--%>
                        //$("#MPlantID").val(ItemMasterList.Table[0].MPlantID);
                        var tntids = $("#Tenant").val();
                        LoadMGroupBasedTenant(tntids);
                        $("#MGroupID").val(ItemMasterList.Table[0].MGroupID);
                        $("#MGroup").val(ItemMasterList.Table[0].MaterialGroup);
                        $("#MDescription").val(ItemMasterList.Table[0].MDescription);
                        LoadMaterialTypeBasedTenant(tntids);
                        $("#MTypeID").val(ItemMasterList.Table[0].MTypeID);
                        if (ItemMasterList.Table[0].MTypeID == 4) {
                            $("#minshel, #maxshel").show();
                        }
                        else {
                            $("#minshel, #maxshel").hide();
                        }
                        LoadStorageCBasedTenant(tntids);
                        $("#StorageConditionID").val(ItemMasterList.Table[0].StorageConditionID);
                        //$('#StorageConditionID').attr("disabled", true);
                        //$("#StorageConditionID").css("background-color", "#ebebe4");
                        $("#MinShelfLifeinDays").val(ItemMasterList.Table[0].MinShelfLifeinDays);
                        $("#TotalShelfLifeinDays").val(ItemMasterList.Table[0].TotalShelfLifeinDays);
                        //$("#MaterialSpaceUtilizationID").val(ItemMasterList.Table[0].MaterialSpaceUtilizationID);
                        //$('#MaterialSpaceUtilizationID').attr("disabled", true);
                        //$("#MaterialSpaceUtilizationID").css("background-color", "#ebebe4");
                        $("#MLength").val(ItemMasterList.Table[0].MLength == 0 ? "" : ItemMasterList.Table[0].MLength);
                        $("#MHeight").val(ItemMasterList.Table[0].MHeight == 0 ? "" : ItemMasterList.Table[0].MHeight);
                        $("#MWidth").val(ItemMasterList.Table[0].MWidth == 0 ? "" : ItemMasterList.Table[0].MWidth);
                        $("#MWeight").val(ItemMasterList.Table[0].MWeight == 0 ? "" : ItemMasterList.Table[0].MWeight);
                        $("#CapacityPerBin").val(ItemMasterList.Table[0].CapacityPerBin == 0 ? "" : ItemMasterList.Table[0].CapacityPerBin);
                        //Addln Information
                        $("#OEMPartNo").val(ItemMasterList.Table[0].OEMPartNo);
                        $("#MCodeAlternative1").val(ItemMasterList.Table[0].MCodeAlternative1);
                        $("#MCodeAlternative2").val(ItemMasterList.Table[0].MCodeAlternative2);
                        $("#ProductCategoryID").val(ItemMasterList.Table[0].ProductCategoryID);
                        $("#MDescriptionLong").val(ItemMasterList.Table[0].MDescriptionLong);
                        $("#<%=hdnIndustry.ClientID %>").val(ItemMasterList.Table[0].IndustryID);
                        $("#<%=txtIndustry.ClientID %>").val(ItemMasterList.Table[0].IndustryName);
                        //$("#<%=hdnPartNum.ClientID %>").val(ItemMasterList.Table[0].MCode);

                        MaterialMasterID = ItemMasterList.Table[0].MaterialMasterID;
                        var TenantIdst = ItemMasterList.Table[0].TenantID
                        LoadSupplier(TenantIdst);
                        LoadWarehouse(ItemMasterList.Table[0].TenantID);
                        LoadSupplierAttach();
                        GetIndustries();

                        //Get UoMType Dropdown
                        if (ItemMasterList.Table5 != null && ItemMasterList.Table5.length > 0) {
                            $("#ddlUoM").empty().append('<option selected="selected" value="PS">Select</option>');
                            for (var j = 0; j < ItemMasterDrop.Table7.length; j++) {
                                $("#ddlUoM").append($("<option></option>").val(ItemMasterDrop.Table7[j].UoMTypeID).html(ItemMasterDrop.Table7[j].UoMType));
                            }
                        }
                        else {
                            $("#ddlUoM").empty().append('<option selected="selected" value="PS">Select</option>');
                            for (var j = 0; j < 1; j++) {
                                $("#ddlUoM").append($("<option></option>").val(ItemMasterDrop.Table13[j].UoMTypeID).html(ItemMasterDrop.Table13[j].UoMType));
                            }
                        }
                        // var obj = $.grep(ItemMasterList.Table18, function (a) { return a.MaterialMasterID == MaterialMasterID });

                        //Supplier Details Table Append
                        debugger;
                        var suptable = "<table class='table table-striped' style='text-align:center';><thead><tr><th>Supplier</th><th style='width:250px'>EAN</th><th>Currency</th><th number>Expected Unit Cost</th><th>Lead Time</th><th>Min. Order Qty</th><th style='text-align:center !important;'>Delete</th><th style='text-align:center !important;'>Actions</th></tr></thead><tbody>";
                        if (ItemMasterList.Table3 != null && ItemMasterList.Table3.length > 0) {
                            for (var i = 0; i < ItemMasterList.Table3.length; i++) {
                                //debugger;
                                var supcurrency = ItemMasterList.Table3[i].Currency == null ? "" : ItemMasterList.Table3[i].Currency;
                                var supexpunitcost = (ItemMasterList.Table3[i].ExpectedUnitCost == 0 || ItemMasterList.Table3[i].ExpectedUnitCost == null) ? "0.00" : ItemMasterList.Table3[i].ExpectedUnitCost;
                                var supdelivtime = ItemMasterList.Table3[i].PlannedDeliveryTime == null ? "" : ItemMasterList.Table3[i].PlannedDeliveryTime;
                                var supiniorderqty = ItemMasterList.Table3[i].InitialOrderQuantity == null ? "" : ItemMasterList.Table3[i].InitialOrderQuantity;
                                suptable += "<tr><td>" + ItemMasterList.Table3[i].SupplierName + "</td><td style='width:250px'>" + ItemMasterList.Table3[i].SupplierPartNumber + "</td><td>" + supcurrency + "</td><td number>" + supexpunitcost + "</td><td>" + supdelivtime + "</td><td>" + supiniorderqty + "</td><td style='text-align:center !important;'><span style='cursor:pointer !important;' onclick=DeleteDetails(" + ItemMasterList.Table3[i].MaterialMaster_SupplierID + ",'Sup');><i class='material-icons vl'>delete</i></span></td><td style='text-align:center !important;'><span style='cursor:pointer !important;' onclick=EditSupdetails(" + ItemMasterList.Table3[i].MaterialMaster_SupplierID + ");><i class='material-icons vl'>edit</i></span></td></tr>";
                            }
                        }
                        else {
                            suptable += "<tr><td colspan='8' style='text-align:center'><b>No Data Found</b></td><tr>";
                        }
                        suptable += "</tbody></table>";
                        $('#SupDetailsTable').html(suptable);

                        //UoM Configuration

                        var uomtable = "<table class='table table-striped' id='uomtables'><thead><tr><th>UoM Type</th><th>UoM</th><th style='' number>Qty. Per UoM</th><th center>Delete</th><th>Actions</th></tr></thead><tbody>";
                        if (ItemMasterList.Table5 != null && ItemMasterList.Table5.length > 0) {
                            $("#ddlMeasure").val(ItemMasterList.Table5[0].MeasurementTypeID == null ? 0 : ItemMasterList.Table5[0].MeasurementTypeID);
                            $("#ddlMeasure").attr("disabled", true);
                            for (var i = 0; i < ItemMasterList.Table5.length; i++) {
                                //debugger;
                                var uomqtys = ItemMasterList.Table5[i].UoMQty == null ? "" : ItemMasterList.Table5[i].UoMQty;
                                uomtable += "<tr id='uomrow-" + i + "'><td class='UoMRowId'>" + ItemMasterList.Table5[i].UoMType + "</td><td>" + ItemMasterList.Table5[i].UoM + "</td><td style='' number>" + uomqtys + "</td><td center><span style='cursor:pointer !important; width:22px; display:block; margin:auto;' id='delbtnuom" + i + "' onclick=DeleteDetails(" + ItemMasterList.Table5[i].MaterialMaster_UoMID + ",'UoM');><i class='material-icons vl'>delete</i></span></td>";
                                uomtable += "<td ><span id='editbtnuom" + i + "' style='cursor:pointer !important;' onclick=EditUoMdetails(" + ItemMasterList.Table5[i].MaterialMaster_UoMID + ");><i class='material-icons vl'>edit</i></span></td>";
                                uomtable += "</tr>";
                            }
                        }
                        else {

                            uomtable += "<tr><td colspan='8' style='text-align:center'><b>No Data Found</b></td><tr>";
                            //$("#ddlMeasure").val("0");            
                            $("#ddlMeasure").attr("disabled", false);
                        }
                        uomtable += "</tbody></table>";

                        $('#UoMDetailsTable').html(uomtable);
                        var muomcheck = $.grep(ItemMasterList.Table5, function (a) { return a.UoMType == "MUoM" });
                        //debugger;
                        var checkUoMList = ItemMasterList.Table5;
                        uomcheck = ItemMasterList.Table5;
                        ////debugger;
                        var uomStatus = inArray(uomcheck);

                        if (checkUoMList.length != 0) {
                            if (uomStatus == false) {
                                $("#uomtables tr:eq(1) td:eq(4)").find("#editbtnuom0").show();
                                $("#uomtables tr:eq(2) td:eq(4)").find("#editbtnuom1").show();
                                if (muomcheck.length != 0) {
                                    $("#uomtables tr:eq(1) td:eq(3)").find("#delbtnuom0").hide();
                                }
                                else {
                                    $("#uomtables tr:eq(1) td:eq(3)").find("#delbtnuom0").show();
                                }
                                $("#uomtables tr:eq(2) td:eq(3)").find("#delbtnuom1").show();
                            }
                            else {

                                $('.UoMRowId').each(function () {
                                    var param = $(this).text();
                                    if (param == "BUoM") {
                                        var rowid = $(this).parent().attr("id");
                                        var tdid = rowid.split('-')[1];
                                        $('#editbtnuom' + tdid).hide();
                                        $('#delbtnuom' + tdid).hide();
                                    }
                                    if (param == "MUoM") {
                                        var rowid = $(this).parent().attr("id");
                                        var tdid = rowid.split('-')[1];
                                        $('#editbtnuom' + tdid).hide();
                                        $('#delbtnuom' + tdid).hide();
                                    }

                                });
                            }
                        }


                        //Inward QC Inspection
                        var qctable = "<table class='table table-striped'><thead><tr><th>Checkpoint</th><th number>Min.Threshold</th><th number>Max.Threshold</th><th center>Is Required</th><th style='text-align:center !important;'>Delete</th><th style='text-align:center !important;'>Actions</th></tr></thead><tbody>";

                        if (ItemMasterList.Table9 != null && ItemMasterList.Table9.length > 0) {
                            for (var i = 0; i < ItemMasterList.Table9.length; i++) {
                                //debugger;
                                var isrequired;
                                if (ItemMasterList.Table9[i].IsRequired == 1)
                                { isrequired = "Yes" }
                                else { isrequired = "No" }
                                var qcmintolerance = ItemMasterList.Table9[i].MinTolerance == null ? "" : ItemMasterList.Table9[i].MinTolerance;
                                var qcmaxtolerance = ItemMasterList.Table9[i].MaxTolerance == null ? "" : ItemMasterList.Table9[i].MaxTolerance;
                                qctable += "<tr><td>" + ItemMasterList.Table9[i].ParameterName + "</td><td number>" + qcmintolerance + "</td><td number>" + qcmaxtolerance + "</td><td style='text-align: center;'>" + isrequired + "</td>";
                                qctable += "<td style='text-align:center !important;'><span style='cursor:pointer !important;' onclick=DeleteDetails(" + ItemMasterList.Table9[i].MaterialMaster_QualityParameterID + ",'QC');><i class='material-icons vl'>delete</i></span></td>";
                                qctable += "<td style='text-align:center !important;'><span style='cursor:pointer !important;' onclick=EditQcdetails(" + ItemMasterList.Table9[i].MaterialMaster_QualityParameterID + ");><i class='material-icons vl'>edit</i></span></td></tr > ";
                            }
                        }
                        else {
                            qctable += "<tr><td colspan='8' style='text-align:center'><b>No Data Found</b></td><tr>";
                        }
                        qctable += "</tbody></table>";
                        $('#QcTable').html(qctable);

                        //Replenishment
                        var BinTable = "<table class='table table-striped' style='text-align:center !important;width: 97.5%;white-space:nowrap;margin: auto;'><thead><tr><th>Location</th><th>Min. Stock Level</th><th>Max. Stock Level</th><th>Created By</th><th>Actions</th></tr></thead><tbody>";
                        if (ItemMasterList.Table2 != null && ItemMasterList.Table2.length > 0) {
                            for (var i = 0; i < ItemMasterList.Table2.length; i++) {
                                //debugger;
                                BinTable += "<tr><td>" + ItemMasterList.Table2[i].Location + "</td><td>" + ItemMasterList.Table2[i].MinimumStockLevel + "</td><td>" + ItemMasterList.Table2[i].MaximumStockLevel + "</td><td>" + ItemMasterList.Table2[i].CreatedName + "</td><td style='text-align:center !important;'><span style='cursor:pointer !important;' onclick=EditBindetails(" + ItemMasterList.Table2[i].BinReplenishmentId + ");><i class='material-icons vl'>edit</i></span></td></tr>";
                            }
                        }
                        else {
                            BinTable += "<tr><td colspan='8' style='text-align:center'><b>No Data Found</b></td><tr>";
                        }
                        BinTable += "</tbody></table>";
                        $('#BinTable').html(BinTable);

                        var MaterialTable = "<table class='table table-striped' style='text-align:centerwidth: 97.5%;white-space:nowrap;margin: auto;'><thead><tr><th>Warehouse</th><th number>Reorder Min. Qty</th><th number>Reorder Max. Qty</th><th>Created By</th><th>Actions</th></tr></thead><tbody>";
                        if (ItemMasterList.Table2 != null && ItemMasterList.Table10.length > 0) {
                            for (var i = 0; i < ItemMasterList.Table10.length; i++) {
                                //debugger;
                                MaterialTable += "<tr><td>" + ItemMasterList.Table10[i].WarehouseLoc + "</td><td style='text-align: right;'> " + ItemMasterList.Table10[i].ReorderQtyMin + "</td><td style='text-align: right;'>" + ItemMasterList.Table10[i].ReorderQtyMax + "</td><td>" + ItemMasterList.Table10[i].CreatedName + "</td><td style='text-align:center !important;'><span style='cursor:pointer !important;' onclick=EditMaterialdetails(" + ItemMasterList.Table10[i].RequirementPlanningID + ");><i class='material-icons vl'>edit</i></span></td></tr>";
                            }
                        }
                        else {
                            MaterialTable += "<tr><td colspan='8' style='text-align:center'><b>No Data Found</b></td><tr>";
                        }
                        MaterialTable += "</tbody></table>";
                        $('#MaterialTable').html(MaterialTable);

                        //Get MSP Details
                        //var obj = $.grep(ItemMasterList.Table11, function (a) { return a.MaterialMasterID == MaterialMasterID });
                        var obj = ItemMasterList.Table11;
                        if (obj != null && obj.length > 0) {
                            for (var i = 0; i < obj.length; i++) {

                                $('#msp' + obj[i].MaterialStorageParameterID).prop("checked", true);
                                $('#msp' + obj[i].MaterialStorageParameterID).attr("data-attr-id", obj[i].MaterialMaster_MaterialStorageParameterID);
                            }
                        }
                    }
                });

            }
        }

        function inArray(stringArray) {
            //debugger;
            var uomStatus = false;
            for (var i = 0; i < stringArray.length; i++) {
                var UT = stringArray[i].UoMType;
                UT = UT.substring(0, UT.length - 1);
                if (UT == "AltUoM") {
                    return uomStatus = true;
                }
            }
            return uomStatus;
        }

        /* function inArray(needle, haystack) {

             var length = haystack.length;
             for(var i = 0; i < length; i++) {
                 if(haystack[i] == needle)
                     return true;
             }
             return false;
         }*/

        //Deleting Dupplier, Qc, UoM
        function DeleteDetails(id, name) {
            //debugger;
            $("#DelModal").modal({
                show: 'true'
            });
            $("#yesdel").click(function () {
                $('#DelModal').modal('hide');
                //$("#DelModal").modal({
                //    show: 'false'
                //});
                if (name == "Sup") {
                    DeleteSupDetails(id);
                }
                if (name == "UoM") {
                    DeleteUoMDetails(id);
                }
                if (name == "QC") {
                    DeleteQcDetails(id);
                }
            })
        }

        //Setting Supplier Details Onclick Edit
        function EditSupdetails(id) {
            //debugger;
            var supplierData = $.grep(ItemMasterList.Table3, function (a) { return a.MaterialMaster_SupplierID == id });
            $("#SupModal").modal({
                show: 'true'
            });
            var ltMMT_SupID = $("#MMT_SUPPLIER_ID").val(supplierData[0].MaterialMaster_SupplierID);
            Tenant = $("#Tenant").val();
            $("#Supname").val(supplierData[0].SupplierName);
            var vSUPID = $("#ddlSupplier").val(supplierData[0].SupplierID);
            var txtSupplierPartNumber = $("#suppartnum").val(supplierData[0].SupplierPartNumber);
            var CurrencyID = $("#ddlCurrency").val(supplierData[0].CurrencyID);
            var vUnitCost = $("#UnitCost").val(supplierData[0].ExpectedUnitCost == 0 ? "" : supplierData[0].ExpectedUnitCost);
            var vDeliveryTime = $("#PlannedTime").val(supplierData[0].PlannedDeliveryTime == 0 ? "" : supplierData[0].PlannedDeliveryTime);
            var vInitialOrderQty = $("#SupQuantity").val(supplierData[0].InitialOrderQuantity == 0 ? "" : supplierData[0].InitialOrderQuantity);
        }

        //Deleting Supplier Details
        function DeleteSupDetails(id) {
            Tenant = $("#Tenant").val();
            var rfidIDs = id;
            var supplierData = $.grep(ItemMasterList.Table3, function (a) { return a.MaterialMaster_SupplierID == id });
            var SupplierIDs = supplierData[0].SupplierID;
            $.ajax({
                url: '<%=ResolveUrl("~/mMaterialManagement/ItemMasterRequest.aspx/DeleteSupDetails") %>',
                data: "{'MaterialMasterID' : '" + MaterialMasterID + "','rfidIDs' : '" + rfidIDs + "','TenantID' : '" + Tenant + "','SupplierIDs' : '" + SupplierIDs + "'}",
                dataType: "json",
                type: "POST",
                async: false,
                contentType: "application/json; charset=utf-8",
                success: function (response) {
                    //alert(response.d);
                    var del = response.d;
                    if (del == "") {

                        showStickyToast(true, "Successfully Deleted Supplier", false);
                    }
                    else {
                        showStickyToast(false, "Cannot delete, as this Supplier is configured in PO", false);
                    }
                    //location.reload();
                    GetItemMasterDetails(MaterialMasterID);
                }
            });
        }

        //Setting UoM Details Onclick Edit
        function EditUoMdetails(id) {
            var UoMData = $.grep(ItemMasterList.Table5, function (a) { return a.MaterialMaster_UoMID == id });
            $("#UoMModal").modal({
                show: 'true'
            });
            var ltMMT_GUoMID = $("#UOM_TYPE_ID").val(UoMData[0].MaterialMaster_UoMID);
            Tenant = $("#Tenant").val();
            var UoMTypeID = $("#ddlUoM").val(UoMData[0].UoMTypeID);
            var UoMID = $("#UoM").val(UoMData[0].UoMID);
            var UoMQty = $("#QtyPerUoM").val(UoMData[0].UoMQty);
        }

        //Deleting UoM Details
        function DeleteUoMDetails(id) {
            //debugger;
            Tenant = $("#Tenant").val();
            var rfidID = id;
            $.ajax({
                url: '<%=ResolveUrl("~/mMaterialManagement/ItemMasterRequest.aspx/DeleteUoMDetails") %>',
                data: "{'rfidIDs' : '" + rfidID + "'}",
                dataType: "json",
                type: "POST",
                async: false,
                contentType: "application/json; charset=utf-8",
                success: function (response) {
                    //alert(response.d);
                    var del = response.d;
                    if (del == "") {
                        //showStickyToast(false, "Cannot delete, as this UoM is used by some transactions", false);
                        showStickyToast(true, "Successfully Deleted UoM", false);
                    }
                    else {
                        //showStickyToast(true, "Successfully Deleted UoM", true);
                        showStickyToast(false, "Cannot delete, as this UoM is used by some transactions", false);
                    }
                    //location.reload();
                    GetItemMasterDetails(MaterialMasterID);
                }
            });
        }

        //Setting Qc Details OnClick Edit
        function EditQcdetails(id) {
            var QcData = $.grep(ItemMasterList.Table9, function (a) { return a.MaterialMaster_QualityParameterID == id });
            $("#QcModal").modal({
                show: 'true'
            });
            var vMMT_QualityParameterID = $("#vMMT_QualityParameterID").val(QcData[0].MaterialMaster_QualityParameterID);
            Tenant = $("#Tenant").val();
            var vQualityParameterID = $("#ddlParameter").val(QcData[0].QualityParameterID);
            var vIsRequired = "";
            if (QcData[0].IsRequired == 1) {
                $('#QCReq').prop("checked", true);
            }
            else {
                $('#QCReq').prop("checked", false);
            }
            var vtxtMinValue = $("#minTol").val(QcData[0].MinTolerance);
            var vtxtMaxValue = $("#maxTol").val(QcData[0].MaxTolerance);
        }

        //Deleting QC Details
        function DeleteQcDetails(id) {
            Tenant = $("#Tenant").val();
            var rfidID = id;
            $.ajax({
                url: '<%=ResolveUrl("~/mMaterialManagement/ItemMasterRequest.aspx/DeleteQCDetails") %>',
                data: "{'MaterialMasterID' : '" + MaterialMasterID + "','rfidIDs' : '" + rfidID + "','TenantID' : '" + Tenant + "'}",
                dataType: "json",
                type: "POST",
                async: false,
                contentType: "application/json; charset=utf-8",
                success: function (response) {
                    //alert(response.d);
                    var del = response.d;
                    if (del == "") {
                        //showStickyToast(false, "Cannot delete, as this QC Parameter is configured in PO", false);
                        showStickyToast(true, "Successfully Deleted QC Parameter", false);
                    }
                    else {
                        //showStickyToast(true, "Successfully Deleted QC Parameter", true);
                        showStickyToast(false, "Cannot delete, as this QC Parameter is configured in PO", false);
                    }
                    //location.reload();
                    GetItemMasterDetails(MaterialMasterID);
                }
            });
        }

        //Setting Bin Details on Edit
        function EditBindetails(id) {
            //debugger;
            BinData = $.grep(ItemMasterList.Table2, function (a) { return a.BinReplenishmentId == id });
            $("#LocationID").val(BinData[0].LocationID);
            $("#LocationText").val(BinData[0].Location);
            $('#LocationText').attr("disabled", true);
            $("#LocationText").css("background-color", "#ebebe4");
            $("#MinimumStockLevel").val(BinData[0].MinimumStockLevel);
            $("#MaximumStockLevel").val(BinData[0].MaximumStockLevel);
        }

        //Setting Material Details OnClick Edit
        function EditMaterialdetails(id) {
            MaterialData = $.grep(ItemMasterList.Table10, function (a) { return a.RequirementPlanningID == id });
            $("#WarehouseID").val(MaterialData[0].WarehouseID);
            $('#WarehouseID').attr("disabled", true);
            $("#WarehouseID").css("background-color", "#ebebe4");
            $("#ReorderQtyMin").val(MaterialData[0].ReorderQtyMin);
            $("#ReorderQtyMax").val(MaterialData[0].ReorderQtyMax);
        }

        // Saving of Basic Details
        function InsertBasic()
        {
            //alert();
            debugger;
            Tenant = $("#Tenant").val();
            var AccountID = $("#Account").val();
            var PartNum = $("#MCode").val().trim();
            var Plant = $("#MPlantID").val();
            var MGroup = $("#MGroupID").val();
            var SDesc = $("#MDescription").val().trim();
            var MType = $("#MTypeID").val();
            var StorageCondition = $("#StorageConditionID").val();
            var MinShelfLife = $("#MinShelfLifeinDays").val().trim();
            var TotShelfLife = $("#TotalShelfLifeinDays").val().trim();
            //var SpaceUtilizationID = $("#MaterialSpaceUtilizationID").val();
            var MLength = $("#MLength").val().trim();
            var MHeight = $("#MHeight").val().trim();
            var MWidth = $("#MWidth").val().trim();
            var MWeight = $("#MWeight").val().trim();
            var CPerBin = $("#CapacityPerBin").val().trim();



            if (MaterialMasterID == null || MaterialMasterID == 0) {
                //debugger;
                var CheckPartNum = $.grep(ItemMasterDrop.Table12, function (a) { return a.MCode == PartNum && a.TenantID == Tenant });
                if (CheckPartNum.length != 0) {
                    showStickyToast(false, "Part Number Already Exists", false);
                    $('#partno').show();
                    $('#partyes').hide();
                    return false;
                }
            }

            if (Tenant == '' || Tenant == "PS" || Tenant==0) {
                showStickyToast(false, "Please select Tenant", false);
                return false;
            }
            if (PartNum == '') {
                showStickyToast(false, "Please enter Part Number", false);
                return false;
            }
            if ($("#MGroup").val() == '' || MGroup == 0 || MGroup== null || MGroup == undefined ) {
                showStickyToast(false, "Please select Material Group", false);
                return false;
            }
            if (SDesc == '') {
                showStickyToast(false, "Please enter Description", false);
                return false;
            }
            if (MType == '' || MType == "PS") {
                showStickyToast(false, "Please select Material type", false);
                return false;
            }
            if (StorageCondition == '' || StorageCondition == "PS") {
                showStickyToast(false, "Please select Storage Condition", false);
                return false;
            }
            
            //if (MType == "4") {
            //    if (MinShelfLife == '') {
            //        showStickyToast(false, "Please enter Min. Shelf Life", false);
            //        return false;
            //    }
            //    if (TotShelfLife == '') {
            //        showStickyToast(false, "Please enter Total Shelf Life", false);
            //        return false;
            //    }
            //}
            if (MinShelfLife != '') {
                if (TotShelfLife == '') {
                    showStickyToast(false, "Please enter Total Shelf Life", false);
                    return false;
                }
            }
            if (TotShelfLife != '') {
                if (MinShelfLife == '') {
                    showStickyToast(false, "Please enter Min. Shelf Life", false);
                    return false;
                }
            }
            // if (MLength == '' || MLength == "0") {
            //    showStickyToast(false, "Please Enter Length", false);
            //    return false;
            //}
            //if (MHeight == '' || MHeight == "0") {
            //    showStickyToast(false, "Please Enter Height", false);
            //    return false;
            //}
            // if (MWidth == '' || MWidth == "0") {
            //    showStickyToast(false, "Please Enter width", false);
            //    return false;
            //}
            // if (MWeight == '' || MWeight == "0") {
            //    showStickyToast(false, "Please Enter Weight", false);
            //    return false;
            //}
              if (CPerBin == '' || CPerBin == "0") {
                showStickyToast(false, "Please Enter  Capacity Per Bin ", false);
                return false;
            }
            //if (SpaceUtilizationID == '' || SpaceUtilizationID == "PS") {
            //    showStickyToast(false, "Please select Space Utilization", false);
            //    return false;
            //}
            //if (MLength == '') {
            //    showStickyToast(false, "Please enter Length", false);
            //    return false;
            //}
            //if (MHeight == '') {
            //    showStickyToast(false, "Please enter Height", false);
            //    return false;
            //}
            //if (MWidth == '') {
            //    showStickyToast(false, "Please enter Width", false);
            //    return false;
            //}
            //if (MWeight == '') {
            //    showStickyToast(false, "Please enter Weight", false);
            //    return false;
            //}
            //if (CPerBin == '') {
            //    showStickyToast(false, "Please enter Capacity Per. Bin", false);
            //    return false;
            //}
            if (parseInt(MinShelfLife) > parseInt(TotShelfLife)) {
                showStickyToast(false, "Min. Shelf Life should be less than Total Shelf Life", false);
                return false;
            }

            $('.p1save').each(function () {
                var paramname = $(this).attr('id');
                var paramvalue = $(this).val();
                if (paramname == "MLength" || paramname == "MHeight" || paramname == "MWidth" || paramname == "MWeight" || paramname == "CapacityPerBin") {
                    paramvalue = paramvalue == "" ? "0" : paramvalue;
                }
                BasicObj[paramname] = paramvalue;
            });
            

            
            var copymid = new URL(window.location.href).searchParams.get("edittype");
            if (copymid == "copy") {
                MaterialMasterID = 0;
                BasicObj['MaterialMasterID'] = MaterialMasterID;
                var BasicData = JSON.stringify(BasicObj);
                var prevMaterialMasterID = new URL(window.location.href).searchParams.get("mid");    
                $.ajax({
                    url: '<%=ResolveUrl("~/mMaterialManagement/ItemMasterRequest.aspx/UpsertCopyBasicDetails") %>',
                    data: "{'BasicData' : '" + BasicData + "','AccountID' : '" + AccountID + "','TenantID' : '" + Tenant + "','LoggedInUserID' : '" + <%=this.cp.UserID%> + "','MaterialMasterID' : '" + MaterialMasterID + "','prevMaterialMasterID' : '" + prevMaterialMasterID + "' ,'Mcode':'" + PartNum + "'}",
                    dataType: "json",
                    type: "POST",
                    async: false,
                    contentType: "application/json; charset=utf-8",
                    success: function (response) {
                        //alert(response.d);
                        debugger;  
                        if (response.d == "Failed") {
                            showStickyToast(false, "Item Code Already Exists", false);
                        }
                        var basicmaterialid = JSON.parse(response.d);
                        MaterialMasterID = basicmaterialid.Table[0].MaterialID;
                        //debugger;
                        //Upsert MSP Call
                        UpsertMsp(MaterialMasterID);
                        showStickyToast(true, "Basic Details Saved Successfully", false);
                        setTimeout(function () {                            
                            window.location.replace("ItemMasterRequest.aspx?mid=" + MaterialMasterID);
                        },1500);                          
                    }
                });
            }
            else {
                MaterialMasterID = MaterialMasterID == null ? 0 : MaterialMasterID;
                BasicObj['MaterialMasterID'] = MaterialMasterID;
                var BasicData = JSON.stringify(BasicObj);
                $.ajax({
                    url: '<%=ResolveUrl("~/mMaterialManagement/ItemMasterRequest.aspx/UpsertBasicDetails") %>',
                    data: "{'BasicData' : '" + BasicData + "','AccountID' : '" + AccountID + "','TenantID' : '" + Tenant + "','LoggedInUserID' : '" + <%=this.cp.UserID%> + "','MaterialMasterID' : '" + MaterialMasterID + "', 'Mcode':'" + PartNum + "'}",
                    dataType: "json",
                    type: "POST",
                    async: false,
                    contentType: "application/json; charset=utf-8",
                    success: function (response) {
                        //alert(response.d);
                        //debugger;
                        if (response.d == "Failed")
                        {
                            showStickyToast(false, "Item Code Already Exists", false);
                        }
                        var POmapped = response.d;
                        if (POmapped == "mappedpo") {
                            showStickyToast(false, "Cannot save as this Item is mapped to PO.", false);
                            return false;
                        }
                        else {
                            var basicmaterialid = JSON.parse(response.d);
                            MaterialMasterID = basicmaterialid.Table[0].MaterialID;
                            //debugger;
                            //Upsert MSP Call
                            UpsertMsp(MaterialMasterID);
                            showStickyToast(true, "Basic Details Saved Successfully", false);
                            setTimeout(function () {
                                window.location.replace("ItemMasterRequest.aspx?mid=" + MaterialMasterID);
                            }, 1500); 
                            //$("#collapseNine").focus();
                            //document.getElementById("collapseNine").focus();
                            //window.location = "ItemMasterRequest.aspx?mid=" + MaterialMasterID;
                            //GetItemMasterDetails(MaterialMasterID);
                            //return false;
                        }
                    }
                });
            }
            
        }

        //Get MSP Data
        function GetMSPData() {
            var fieldData = '<root>';
            $(".fieldtogetMsps").each(function () {
                var param = $(this).attr('id').replace("msp", "");
                var val = $(this).val().trim();
                var MMMSPID = $(this).attr("data-attr-id");
                var copymid = new URL(window.location.href).searchParams.get("edittype");
                if (copymid == "copy") {
                    MMMSPID = 0;
                }
                else {
                    MMMSPID = $(this).attr("data-attr-id");
                }
                var IsActive = $(this).attr("data-attr-isactive");
                var IsDeleted = $(this).attr("data-attr-isdeleted");
                var paramtype = $(this).attr('type');
                if (paramtype == "checkbox") {
                    val = $(this).prop('checked');
                    if (val == true) {

                        //var MSPID = $(this)[0].id;

                        fieldData += '<data>';
                        fieldData += '<IsRequired>1</IsRequired>';
                        fieldData += '<IsActive>' + IsActive + '</IsActive>';
                        fieldData += '<IsDeleted>' + IsDeleted + '</IsDeleted>';
                        fieldData += '<MaterialMasterID>' + MaterialMasterID + '</MaterialMasterID>';
                        fieldData += '<MaterialStorageParameterID>' + param + '</MaterialStorageParameterID>';
                        fieldData += '<MaterialMaster_MaterialStorageParameterID>' + MMMSPID + '</MaterialMaster_MaterialStorageParameterID></data>';
                    }

                    else {
                        //var MSPID = $(this)[0].id;

                        fieldData += '<data>';
                        fieldData += '<IsRequired>0</IsRequired>';
                        fieldData += '<IsActive>0</IsActive>';
                        fieldData += '<IsDeleted>1</IsDeleted>';
                        fieldData += '<MaterialMasterID>' + MaterialMasterID + '</MaterialMasterID>';
                        fieldData += '<MaterialStorageParameterID>' + param + '</MaterialStorageParameterID>';
                        fieldData += '<MaterialMaster_MaterialStorageParameterID>' + MMMSPID + '</MaterialMaster_MaterialStorageParameterID></data>';
                    }
                }
            });
            fieldData = fieldData + '</root>';
            return fieldData;
        }

        //Save MSP Data
        function UpsertMsp(mid) {
            var obj = {};
            obj.UserID = "<%=cp.UserID.ToString()%>";
            obj.Inxml = GetMSPData();
            //obj.MM_MST_Material_ID = mid;
            $.ajax({
                url: "ItemMasterRequest.aspx/SETMsps",

                dataType: 'json',
                contentType: "application/json",
                type: 'POST',
                data: JSON.stringify(obj),
                async: false,
                success: function (response) {
                    if (response.d == "success") {
                        //alert("Saved Successfully");
                        //location.reload();
                        //GetPreferencesList();
                    }
                }
            });
        }

        //Saving Additional Information
        function AddlnInfoSave() {
            //debugger;
            Tenant = $("#Tenant").val();
            var OEMPartNo = $("#OEMPartNo").val();
            var AltPartNum1 = $("#MCodeAlternative1").val();
            var AltpartNum2 = $("#MCodeAlternative2").val();
            var ProductCategoryID = $("#ProductCategoryID").val();
            var MDescriptionLong = $("#MDescriptionLong").val();
            if (ProductCategoryID == "PS") {
                showStickyToast(false, "Please select Product Category", false);
                return false;
            }

            $('.p1save').each(function () {
                BasicObj[$(this).attr('id')] = $(this).val();
            });
            BasicObj['MaterialMasterID'] = MaterialMasterID;
            var AddlnData = JSON.stringify(BasicObj);
            $.ajax({
                url: '<%=ResolveUrl("~/mMaterialManagement/ItemMasterRequest.aspx/UpsertAddlnInfo") %>',
                data: "{'AddlnData' : '" + AddlnData + "','TenantID' : '" + Tenant + "','LoggedInUserID' : '" + <%=this.cp.UserID%> + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (response) {
                    showStickyToast(true, "Additional Information Saved Successfully", false);
                    //location.reload();
                    //document.getElementById("panel3").focus();
                }
            });
        }

        //Clear Supplier Form On Close Button
        function mySupclear() {
            $("#suppartnum, #UnitCost, #PlannedTime, #SupQuantity, #MMT_SUPPLIER_ID, #ddlSupplier, #Supname").val("");
            $("#ddlCurrency").val("PS");

        }

        //Clear UoM Form On Close Button
        function myUoMclear() {
            //debugger;
            $("#QtyPerUoM, #UOM_TYPE_ID").val("");
            $("#ddlUoM, #UoM").val("PS");

        }

        //Clear QC Form On Close Button
        function myQCclear() {
            $("#minTol, #maxTol, #vMMT_QualityParameterID").val("");
            $("#QCReq").prop("checked", false);
            $("#ddlParameter").val("PS");

        }

        //Clear Bin and Material Replenishment
        function RepClear() {
            //debugger;
            $("#LocationText").val("");
            $("#LocationID, #MaximumStockLevel, #MinimumStockLevel, #ReorderQtyMin, #ReorderQtyMax").val("");
            $("#WarehouseID").val("PS");
        }

        //Saving Add Supplier Details
        function AddSupSave() {
            //debugger;
            var ltMMT_SupID = $("#MMT_SUPPLIER_ID").val().trim();
            ltMMT_SupID = ltMMT_SupID == "" ? "0" : ltMMT_SupID;
            Tenant = $("#Tenant").val();
            var vSUPID = $("#ddlSupplier").val();
            var txtSupplierPartNumber = $("#suppartnum").val().trim();
            var curr = $("#ddlCurrency").val();
            //var CurrencyID = $("#ddlCurrency").val();
            var CurrencyID = curr == "PS" ? 0 : curr;
            var vunitcost = $("#UnitCost").val().trim();
            //var vUnitCost = $("#UnitCost").val();
            var vUnitCost = vunitcost == "" ? "0" : vunitcost;
            vdeltime = $("#PlannedTime").val().trim();
            //var vDeliveryTime = $("#PlannedTime").val();
            var vDeliveryTime = vdeltime == "" ? "0" : vdeltime;
            var viniordtime = $("#SupQuantity").val().trim();
            //var vInitialOrderQty = $("#SupQuantity").val();
            var vInitialOrderQty = viniordtime == "" ? "0" : viniordtime;
            if (vSUPID == '' || vSUPID == "PS") {
                showStickyToast(false, "Please select Supplier", false);
                return false;
            }
            if (txtSupplierPartNumber == "") {
                showStickyToast(false, "Please enter EAN", false);
                return false;
            }




            //debugger;
            if (ltMMT_SupID != 0) {
                var validateeditsup = $.grep(ItemMasterList.Table3, function (a) { return (a.MaterialMasterID == MaterialMasterID && a.MaterialMaster_SupplierID != eval(ltMMT_SupID)) && (a.SupplierID == eval(vSUPID) || a.SupplierPartNumber == txtSupplierPartNumber) });
                if (validateeditsup.length == 0) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mMaterialManagement/ItemMasterRequest.aspx/UpsertSupDetailsInfo") %>',
                        data: "{'ltMMT_SupID' : '" + ltMMT_SupID + "','MaterialMasterID' : '" + MaterialMasterID + "','vSUPID' : '" + vSUPID + "','TenantID' : '" + Tenant + "','vUnitCost' : '" + vUnitCost + "','vDeliveryTime' : '" + vDeliveryTime + "','CurrencyID' : '" + CurrencyID + "','txtSupplierPartNumber' : '" + txtSupplierPartNumber + "','vInitialOrderQty' : '" + vInitialOrderQty + "','CreatedBy' : '" + <%=this.cp.UserID%> + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (response) {
                            var save = response.d;
                            if (save == "") {
                                showStickyToast(true, "Supplier Details Saved Successfully", false);
                                GetItemMasterDetails(MaterialMasterID);
                                mySupclear();
                                $("#SupModal").modal('hide');
                            }
                            else {
                                showStickyToast(false, "Cannot Save, as this Supplier is configured in PO", false);
                                $("#SupModal").modal('hide');
                                mySupclear();
                            }

                        }
                    });

                }
                else {
                    var ValidateSup = $.grep(ItemMasterList.Table3, function (a) { return a.SupplierID == vSUPID && a.MaterialMasterID == MaterialMasterID && a.MaterialMaster_SupplierID != eval(ltMMT_SupID) });
                    if (ValidateSup.length != 0) {
                        showStickyToast(false, "Supplier Already Exists", false);
                        return false;
                    }
                    var ValidateSupPart = $.grep(ItemMasterList.Table3, function (a) { return a.SupplierPartNumber == txtSupplierPartNumber && a.MaterialMasterID == MaterialMasterID && a.MaterialMaster_SupplierID != eval(ltMMT_SupID) });
                    if (ValidateSupPart.length != 0) {
                        showStickyToast(false, "EAN Already Exists", false);
                        return false;
                    }
                }
            }
            else {
                var ValidateSup = $.grep(ItemMasterList.Table3, function (a) { return a.SupplierID == vSUPID });
                if (ValidateSup.length != 0) {
                    showStickyToast(false, "Supplier Already Exists", false);
                    return false;
                }
                var ValidateSupPart = $.grep(ItemMasterList.Table3, function (a) { return a.SupplierPartNumber == txtSupplierPartNumber });
                if (ValidateSupPart.length != 0) {
                    showStickyToast(false, "EAN Already Exists", false);
                    return false;
                }
                $.ajax({
                    url: '<%=ResolveUrl("~/mMaterialManagement/ItemMasterRequest.aspx/UpsertSupDetailsInfo") %>',
                     data: "{'ltMMT_SupID' : '" + ltMMT_SupID + "','MaterialMasterID' : '" + MaterialMasterID + "','vSUPID' : '" + vSUPID + "','TenantID' : '" + Tenant + "','vUnitCost' : '" + vUnitCost + "','vDeliveryTime' : '" + vDeliveryTime + "','CurrencyID' : '" + CurrencyID + "','txtSupplierPartNumber' : '" + txtSupplierPartNumber + "','vInitialOrderQty' : '" + vInitialOrderQty + "','CreatedBy' : '" + <%=this.cp.UserID%> + "'}",
                     dataType: "json",
                     type: "POST",
                     contentType: "application/json; charset=utf-8",
                     success: function (response) {
                         //debugger;
                         var save = response.d;
                         if (save == "") {
                             showStickyToast(true, "Supplier Details Saved Successfully", false);
                             GetItemMasterDetails(MaterialMasterID);
                             mySupclear();
                             $("#SupModal").modal('hide');
                         }
                         else {
                             showStickyToast(false, "Cannot Save, as this Supplier is configured in PO", false);
                             $("#SupModal").modal('hide');
                             mySupclear();
                         }
                     }
                 });
            }

        }

        //Saving Add UoM Details
        function UoMSave() {
            debugger;
            var ltMMT_GUoMID = $("#UOM_TYPE_ID").val();
            ltMMT_GUoMID = ltMMT_GUoMID == "" ? "0" : ltMMT_GUoMID;
            Tenant = $("#Tenant").val();
            var UoMTypeID = $("#ddlUoM").val();
            var UoMTypeText = $("#ddlUoM option:selected").text();
            var UoMID = $("#UoM").val();
            var UoMQty = $("#QtyPerUoM").val().trim();
            //var UoMQty = $("#QtyPerUoM").val();
            //var UoMQty = uomqty == "" ? "0" : uomqty;
            if (UoMTypeID == '' || UoMTypeID == "PS") {
                showStickyToast(false, "Please select UoM Type", false);
                return false;
            }
            if (UoMID == '' || UoMID == "PS") {
                showStickyToast(false, "Please select UoM", false);
                return false;
            }
            if (UoMQty == '') {
                showStickyToast(false, "Please enter UoM Qty.", false);
                return false;
            }
            if (UoMQty == 0) {
                showStickyToast(false, "UoM Qty. cannot be 0", false);
                return false;
            }
            if (UoMTypeID == "1"){
                if (UoMQty != 1) {
                    showStickyToast(false, "UoM Qty. should Be 1 For BUoM", false);
                    return false;
                }
            }
            if (ltMMT_GUoMID != 0) {
                var validateeditsup = $.grep(ItemMasterList.Table5, function (a) { return (a.MaterialMasterID == MaterialMasterID && a.MaterialMaster_UoMID != eval(ltMMT_GUoMID)) && (a.UoMTypeID == eval(UoMTypeID) || a.UoMID == UoMID) });
                if (validateeditsup.length == 0) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mMaterialManagement/ItemMasterRequest.aspx/UpsertUoMInfo") %>',
                         data: "{'ltMMT_GUoMID' : '" + ltMMT_GUoMID + "','MaterialMasterID' : '" + MaterialMasterID + "','UoMTypeID' : '" + UoMTypeID + "','TenantID' : '" + Tenant + "','UoMID' : '" + UoMID + "','UoMQty' : '" + UoMQty + "','CreatedBy' : '" + <%=this.cp.UserID%> + "'}",
                         dataType: "json",
                         type: "POST",
                         contentType: "application/json; charset=utf-8",
                         success: function (response) {
                             debugger;
                             var save = response.d;
                             if (save == "") {
                                 showStickyToast(true, "UoM Details Saved Successfully", false);
                                 GetItemMasterDetails(MaterialMasterID);
                                 myUoMclear();
                                 $("#UoMModal").modal('hide');
                             }
                             else {
                                 showStickyToast(false, "Cannot Save, as this UoM is configured in PO", false);
                                 $("#UoMModal").modal('hide');
                                 myUoMclear();
                             }
                         }
                     });
                 }
                 else {
                     //debugger;
                     var ValidateUoMID = $.grep(ItemMasterList.Table5, function (a) { return a.UoMTypeID == UoMTypeID && a.MaterialMasterID == MaterialMasterID && a.MaterialMaster_UoMID != eval(ltMMT_GUoMID) });
                     if (ValidateUoMID.length != 0) {
                         showStickyToast(false, "UoM Type Already Exists", false);
                         return false;
                     }
                     var ValidateUoM = $.grep(ItemMasterList.Table5, function (a) { return a.UoMID == UoMID && (a.UoMType == "MUoM" || a.UoMType == "BUoM") && a.MaterialMasterID == MaterialMasterID && a.MaterialMaster_UoMID != eval(ltMMT_GUoMID) });
                     if (ValidateUoM.length != 0) {
                         //if(ValidateUoM.UoMType != "MUoM" && ValidateUoM.UoMType != "BUoM"){
                         if (UoMTypeText != "MUoM" && UoMTypeText != "BUoM") {
                             showStickyToast(false, "UoM Already Exists in Base Measurements", false);
                             return false;
                         }
                         //}

                     }

                     var ValidateUoM = $.grep(ItemMasterList.Table5, function (a) { return a.UoMID == UoMID && a.UoMQty == UoMQty && a.MaterialMasterID == MaterialMasterID && a.MaterialMaster_UoMID != eval(ltMMT_GUoMID) });
                     //debugger;
                     if (UoMTypeText == "MUoM" || UoMTypeText == "BUoM") {
                         $.ajax({
                             url: '<%=ResolveUrl("~/mMaterialManagement/ItemMasterRequest.aspx/UpsertUoMInfo") %>',
                            data: "{'ltMMT_GUoMID' : '" + ltMMT_GUoMID + "','MaterialMasterID' : '" + MaterialMasterID + "','UoMTypeID' : '" + UoMTypeID + "','TenantID' : '" + Tenant + "','UoMID' : '" + UoMID + "','UoMQty' : '" + UoMQty + "','CreatedBy' : '" + <%=this.cp.UserID%> + "'}",
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            success: function (response) {
                                debugger;
                                var save = response.d;
                                if (save == "") {
                                    showStickyToast(true, "UoM Details Saved Successfully", false);
                                    GetItemMasterDetails(MaterialMasterID);
                                    myUoMclear();
                                    $("#UoMModal").modal('hide');
                                }
                                else {
                                    showStickyToast(false, "Cannot Save, as this UoM is configured in PO", false);
                                    $("#UoMModal").modal('hide');
                                    myUoMclear();
                                }
                            }
                        });
                    }
                    else {
                        if (ValidateUoM.length != 0) {
                            showStickyToast(false, "UoM and UoMQty. Already Exists", false);
                            return false;
                        }
                        else {
                            $.ajax({
                                url: '<%=ResolveUrl("~/mMaterialManagement/ItemMasterRequest.aspx/UpsertUoMInfo") %>',
                                    data: "{'ltMMT_GUoMID' : '" + ltMMT_GUoMID + "','MaterialMasterID' : '" + MaterialMasterID + "','UoMTypeID' : '" + UoMTypeID + "','TenantID' : '" + Tenant + "','UoMID' : '" + UoMID + "','UoMQty' : '" + UoMQty + "','CreatedBy' : '" + <%=this.cp.UserID%> + "'}",
                                    dataType: "json",
                                    type: "POST",
                                    contentType: "application/json; charset=utf-8",
                                    success: function (response) {
                                        debugger;
                                        var save = response.d;
                                        if (save == "") {
                                            showStickyToast(true, "UoM Details Saved Successfully", false);
                                            GetItemMasterDetails(MaterialMasterID);
                                            myUoMclear();
                                            $("#UoMModal").modal('hide');
                                        }
                                        else {
                                            showStickyToast(false, "Cannot Save, as this UoM is configured in PO", false);
                                            $("#UoMModal").modal('hide');
                                            myUoMclear();
                                        }
                                    }
                            });
                        }
                    }
                 }
             }
             else {
                 //debugger;
                 var ValidateUoMID = $.grep(ItemMasterList.Table5, function (a) { return a.UoMTypeID == UoMTypeID });
                 if (ValidateUoMID.length != 0) {
                     showStickyToast(false, "UoM Type Already Exists", false);
                     return false;
                 }
                 var ValidateUoM = $.grep(ItemMasterList.Table5, function (a) { return a.UoMID == UoMID && (a.UoMType == "MUoM" || a.UoMType == "BUoM") });
                 if (ValidateUoM.length != 0) {
                     if (UoMTypeText != "MUoM" && UoMTypeText != "BUoM") {
                         showStickyToast(false, "UoM Already Exists in Base Measurements", false);
                         return false;
                     }
                 }
                 var ValidateUoM = $.grep(ItemMasterList.Table5, function (a) { return a.UoMID == UoMID && a.UoMQty == UoMQty && (a.UoMType != "MUoM" && a.UoMType != "BUoM") });
                 if (ValidateUoM.length != 0) {
                     showStickyToast(false, "UoM and UoMQty. Already Exists", false);
                     return false;
                 }
                 $.ajax({
                     url: '<%=ResolveUrl("~/mMaterialManagement/ItemMasterRequest.aspx/UpsertUoMInfo") %>',
                    data: "{'ltMMT_GUoMID' : '" + ltMMT_GUoMID + "','MaterialMasterID' : '" + MaterialMasterID + "','UoMTypeID' : '" + UoMTypeID + "','TenantID' : '" + Tenant + "','UoMID' : '" + UoMID + "','UoMQty' : '" + UoMQty + "','CreatedBy' : '" + <%=this.cp.UserID%> + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (response) {
                        debugger;
                        var save = response.d;
                        if (save == "") {
                            showStickyToast(true, "UoM Details Saved Successfully", false);
                            GetItemMasterDetails(MaterialMasterID);
                            myUoMclear();
                            $("#UoMModal").modal('hide');
                        }
                        else {
                            showStickyToast(false, "Cannot Save, as this UoM is configured in PO", false);
                            $("#UoMModal").modal('hide');
                            myUoMclear();
                        }
                    }
                });
             }
        }

        //Click UoM Conversion
        function UoMShow() {
            var MeasurementID = $("#ddlMeasure").val();
            if (MeasurementID != "0") {
                $(".measures").show();
            }
            else {
                $(".measures").hide();
            }
        }

        //UoM Conversion
        function UoMConvert() {
            //debugger;

            if ($("#ddlFromMeasure").val() == "PS") {
                showStickyToast(false, "Please select From Measurement", false);
                return false;
            }
            if ($("#ddlToMeasure").val() == "PS") {
                showStickyToast(false, "Please select To Measurement", false);
                return false;
            }
            var MeasurementTypeID = $("#ddlMeasure").val();
            var FromMeasureData = $("#ddlFromMeasure").val().split(',');
            var ToMeasureData = $("#ddlToMeasure").val().split(',');
            var FromMeasurements0 = FromMeasureData[0];
            var FromMeasurements1 = FromMeasureData[1];
            var ToMeasurements0 = ToMeasureData[0];
            var ToMeasurements1 = ToMeasureData[1];
            $.ajax({
                url: '<%=ResolveUrl("~/mMaterialManagement/ItemMasterRequest.aspx/UoMConversion") %>',
                 data: "{'MeasurementTypeID' : '" + MeasurementTypeID + "','FromMeasurements0' : '" + FromMeasurements0 + "','FromMeasurements1' : '" + FromMeasurements1 + "','ToMeasurements0' : '" + ToMeasurements0 + "','ToMeasurements1' : '" + ToMeasurements1 + "'}",
                 dataType: "json",
                 type: "POST",
                 contentType: "application/json; charset=utf-8",
                 success: function (response) {
                     //debugger;
                     var UoMResult = response.d;
                     $("#UomResult").html(UoMResult);
                     $(".uomdiv").show();
                 }
             });
         }

         //Saving QC Parameter Details
         function QcSave() {
             //debugger;
             var vMMT_QualityParameterID = $("#vMMT_QualityParameterID").val();
             vMMT_QualityParameterID = vMMT_QualityParameterID == "" ? "0" : vMMT_QualityParameterID;
             Tenant = $("#Tenant").val();
             var vQualityParameterID = $("#ddlParameter").val();
             var vIsRequired = "";
             if ($('#QCReq').is(':checked')) {
                 vIsRequired = 1;
             } else {
                 vIsRequired = 0;
             }
             var vtxtMinValues = $("#minTol").val().trim();
             var vtxtMaxValue = $("#maxTol").val().trim();
             var vtxtMinValue = "";
             //var vtxtMaxValue = "";
             vtxtMinValue = vtxtMinValues == "" ? "0" : vtxtMinValues;
             //vtxtMaxValue = vtxtMaxValues == "" ? "0" : vtxtMaxValues;
             if (vQualityParameterID == '' || vQualityParameterID == "PS") {
                 showStickyToast(false, "Please select Parameter Name", false);
                 return false;
             }
             if (vtxtMinValue == '') {
                 showStickyToast(false, "Please enter Min. Threshold", false);
                 return false;
             }
             if (vtxtMaxValue == '') {
                 showStickyToast(false, "Please enter Max. Threshold", false);
                 return false;
             }
             if (vtxtMaxValue == 0) {
                 showStickyToast(false, "Max. Threshold cannot be 0", false);
                 return false;
             }
             if (parseInt(vtxtMinValue) > parseInt(vtxtMaxValue)) {
                 showStickyToast(false, "Min. Threshold should be less than Max. Threshold", false);
                 return false;
             }
             if (vMMT_QualityParameterID != 0) {
                 var validateeditsup = $.grep(ItemMasterList.Table9, function (a) { return (a.MaterialMasterID == MaterialMasterID && a.MaterialMaster_QualityParameterID != eval(vMMT_QualityParameterID)) && (a.QualityParameterID == eval(vQualityParameterID)) });
                 if (validateeditsup.length == 0) {
                     $.ajax({
                         url: '<%=ResolveUrl("~/mMaterialManagement/ItemMasterRequest.aspx/UpsertQCInfo") %>',
                        data: "{'vMMT_QualityParameterID' : '" + vMMT_QualityParameterID + "','MaterialMasterID' : '" + MaterialMasterID + "','vQualityParameterID' : '" + vQualityParameterID + "','TenantID' : '" + Tenant + "','vIsRequired' : '" + vIsRequired + "','vtxtMinValue' : '" + vtxtMinValue + "','vtxtMaxValue' : '" + vtxtMaxValue + "','CreatedBy' : '" + <%=this.cp.UserID%> + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (response) {
                            //debugger;
                            var save = response.d;
                            if (save == "") {
                                //debugger;
                                showStickyToast(true, "Inward Inspection Details Saved Successfully", false);
                                GetItemMasterDetails(MaterialMasterID);
                                myQCclear();
                                $("#QcModal").modal('hide');
                            }
                            else {
                                showStickyToast(false, "Transactions against this material have been initiated, and hence the material cannot be edited.", false);
                                $("#QcModal").modal('hide')
                                myQCclear();
                            }

                            //location.reload();
                            //document.getElementById("panel6").focus();
                        }
                    });
                }
                else {
                    var ValidateParameter = $.grep(ItemMasterList.Table9, function (a) { return a.QualityParameterID == vQualityParameterID && a.MaterialMasterID == MaterialMasterID && a.MaterialMaster_QualityParameterID != eval(vMMT_QualityParameterID) });
                    if (ValidateParameter.length != 0) {
                        showStickyToast(false, "Parameter Name Already Exists", false);
                        return false;
                    }
                }
            }
            else {
                var ValidateParameter = $.grep(ItemMasterList.Table9, function (a) { return a.QualityParameterID == vQualityParameterID });
                if (ValidateParameter.length != 0) {
                    showStickyToast(false, "Parameter Name Already Exists", false);
                    return false;
                }
                $.ajax({
                    url: '<%=ResolveUrl("~/mMaterialManagement/ItemMasterRequest.aspx/UpsertQCInfo") %>',
                        data: "{'vMMT_QualityParameterID' : '" + vMMT_QualityParameterID + "','MaterialMasterID' : '" + MaterialMasterID + "','vQualityParameterID' : '" + vQualityParameterID + "','TenantID' : '" + Tenant + "','vIsRequired' : '" + vIsRequired + "','vtxtMinValue' : '" + vtxtMinValue + "','vtxtMaxValue' : '" + vtxtMaxValue + "','CreatedBy' : '" + <%=this.cp.UserID%> + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (response) {
                            //debugger;
                            var save = response.d;
                            if (save == "") {
                                //debugger;
                                showStickyToast(true, "Inward Inspection Details Saved Successfully", false);
                                GetItemMasterDetails(MaterialMasterID);
                                myQCclear();
                                $("#QcModal").modal('hide');
                            }
                            else {
                                showStickyToast(false, "Transactions against this material have been initiated, and hence the material cannot be edited.", false);
                                $("#QcModal").modal('hide')
                                myQCclear();
                            }
                        }
                 });
             }

         }

         //Saving Replenishment 
         function RepSave() {
             //debugger;
             Tenant = $("#Tenant").val();
             var LocationID = $("#LocationID").val();
             var MinimumStockLevel = $("#MinimumStockLevel").val();
             var MaximumStockLevel = $("#MaximumStockLevel").val();
             var WarehouseIDs = $("#WarehouseID").val();
             var WarehouseID = WarehouseIDs == "PS" ? "0" : WarehouseIDs;
             var ReorderQtyMin = $("#ReorderQtyMin").val();
             var ReorderQtyMax = $("#ReorderQtyMax").val();
             var CPerBins = $("#CapacityPerBin").val();
             var binobj = "";
             if (LocationID != "") {
                 $.ajax({
                     url: '<%=ResolveUrl("~/mMaterialManagement/ItemMasterRequest.aspx/GetBinVolume") %>',
                     data: "{'LocationID' : '" + LocationID + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (response) {
                        debugger;
                        var binobjs = JSON.parse(response.d);
                        binobj = binobjs.Table[0];
                    }
                 });
                 //var binobj = $.grep(ItemMasterDrop.Table14, function (a) { return a.LocationID == LocationID });
                 if (binobj != 0 || binobj != null) {
                     var BinLength = binobj.Length;
                     var BinWidth = binobj.Width;
                     var BinHeight = binobj.Height;
                     var BinWeight = binobj.MaxWeight;
                 }
                 var BinVolume = BinLength * BinWidth * BinHeight;
                 var IMLength = $("#MLength").val().trim();
                 var IMHeight = $("#MHeight").val().trim();
                 var IMWidth = $("#MWidth").val().trim();
                 if (IMLength != '' && IMHeight != '' && IMWidth != '') {
                     var ItemVolume = IMLength * IMWidth * IMHeight;
                     var UnitVolItem = ItemVolume * MaximumStockLevel;
                 }
                 var ItemMWeight = $("#MWeight").val().trim();
                 var UnitWeight = ItemMWeight * MaximumStockLevel;
                 if (MinimumStockLevel == '') {
                     showStickyToast(false, "Please enter Min. Stock Level", false);
                     return false;
                 }
                 if (MaximumStockLevel == '') {
                     showStickyToast(false, "Please enter Max. Stock Level", false);
                     return false;
                 }
                 if (parseInt(MinimumStockLevel) > parseInt(MaximumStockLevel)) {
                     showStickyToast(false, "Min. StockLevel should be less than Max. StockLevel", false);
                     return false;
                 }
                 if (CPerBins != '') {
                     if (parseInt(MaximumStockLevel) > parseInt(CPerBins)) {
                         showStickyToast(false, "Max. StockLevel should be less than Capacity Per. Bin", false);
                         return false;
                     }
                 }
                 if (parseInt(UnitVolItem) > parseInt(BinVolume)) {
                     showStickyToast(false, "Item Unit Volume cannot be more than Bin Volume", false);
                     return false;
                 }
                 if (parseInt(UnitWeight) > parseInt(BinWeight)) {
                     showStickyToast(false, "Item Unit Weight cannot be more than Bin Weight", false);
                     return false;
                 }
             }
             if (WarehouseIDs != "PS") {
                 if (ReorderQtyMin == '') {
                     showStickyToast(false, "Please enter Reorder Min. Qty", false);
                     return false;
                 }
                 if (ReorderQtyMax == '') {
                     showStickyToast(false, "Please enter Reorder Max. Qty", false);
                     return false;
                 }
                 if (parseInt(ReorderQtyMin) > parseInt(ReorderQtyMax)) {
                     showStickyToast(false, "Reorder Min. Qty should be less than Reorder Max. Qty", false);
                     return false;
                 }
             }

             //if (LocationID == '' || LocationID == "PS") {
             //    showStickyToast(false, "Please select Bin Location", false);
             //    return false;
             //}

             //if (WarehouseID == '' || WarehouseID == "PS") {
             //    showStickyToast(false, "Please select Warehouse", false);
             //    return false;
             //}
             //var BinRepsData = $.grep(ItemMasterList.Table2, function (a) { return a.BinReplenishmentId == id });

             var RepObj = new Object();
             RepObj.MaterialMasterID = MaterialMasterID;
             RepObj.LocationID = LocationID;
             RepObj.MinimumStockLevel = MinimumStockLevel;
             RepObj.MaximumStockLevel = MaximumStockLevel;
             RepObj.WarehouseID = WarehouseID;
             RepObj.ReorderQtyMin = ReorderQtyMin;
             RepObj.ReorderQtyMax = ReorderQtyMax;
             var RepData = JSON.stringify(RepObj);
             //debugger;
             //BinData = $.grep(ItemMasterList.Table2, function (a) { return a.BinReplenishmentId == id });
             //MaterialData = $.grep(ItemMasterList.Table10, function (a) { return a.RequirementPlanningID == id });
             var validateeditbin = null;
             var validateeditmat = null;
             if (BinData != null || MaterialData != null) {
                 if (BinData != null) {
                     validateeditbin = $.grep(ItemMasterList.Table2, function (a) { return (a.MaterialMasterID == MaterialMasterID && a.BinReplenishmentId != eval(BinData[0].BinReplenishmentId)) && (a.LocationID == eval(LocationID)) });
                 }
                 else if (MaterialData != null) {
                     validateeditmat = $.grep(ItemMasterList.Table10, function (a) { return (a.MaterialMasterID == MaterialMasterID && a.RequirementPlanningID != eval(MaterialData[0].RequirementPlanningID)) && (a.WarehouseID == eval(WarehouseID)) });
                 }
                 else if (BinData != null && MaterialData != null) {
                     validateeditmat = $.grep(ItemMasterList.Table10, function (a) { return (a.MaterialMasterID == MaterialMasterID && a.RequirementPlanningID != eval(MaterialData[0].RequirementPlanningID)) && (a.WarehouseID == eval(WarehouseID)) });
                     validateeditbin = $.grep(ItemMasterList.Table2, function (a) { return (a.MaterialMasterID == MaterialMasterID && a.BinReplenishmentId != eval(BinData[0].BinReplenishmentId)) && (a.LocationID == eval(LocationID)) });
                 }

                 if (validateeditmat == null || validateeditbin == null) {
                     $.ajax({
                         url: '<%=ResolveUrl("~/mMaterialManagement/ItemMasterRequest.aspx/UpsertRepInfo") %>',
                        data: "{'RepData' : '" + RepData + "','TenantID' : '" + Tenant + "','LoggedInUserID' : '" + <%=this.cp.UserID%> + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (response) {
                            showStickyToast(true, "Replenishment Details Saved Successfully", false);

                            GetItemMasterDetails(MaterialMasterID);
                            RepClear();
                            //location.reload();
                        }
                    });
                }
                else {
                    var ValidateParameter = $.grep(ItemMasterList.Table2, function (a) { return a.LocationID == eval(LocationID) && a.MaterialMasterID == MaterialMasterID && a.BinReplenishmentId != eval(BinData[0].BinReplenishmentId) });
                    if (ValidateParameter.length != 0) {
                        showStickyToast(false, "Bin Location Already Exists", false);
                        return false;
                    }

                    var ValidateMat = $.grep(ItemMasterList.Table10, function (a) { return a.WarehouseID == eval(WarehouseID) && a.MaterialMasterID == MaterialMasterID && a.RequirementPlanningID != eval(MaterialData[0].RequirementPlanningID) });
                    if (ValidateMat.length != 0) {
                        showStickyToast(false, "Warehouse Location Already Exists", false);
                        return false;
                    }
                }
            }
            else {
                var ValidateBinLoc = $.grep(ItemMasterList.Table2, function (a) { return a.LocationID == LocationID });
                if (ValidateBinLoc.length != 0) {
                    showStickyToast(false, "Bin Location Already Exists", false);
                    return false;
                }
                var ValidateWarehouse = $.grep(ItemMasterList.Table10, function (a) { return a.WarehouseID == WarehouseID });
                if (ValidateWarehouse.length != 0) {
                    showStickyToast(false, "Warehouse Location Already Exists", false);
                    return false;
                }

                $.ajax({
                    url: '<%=ResolveUrl("~/mMaterialManagement/ItemMasterRequest.aspx/UpsertRepInfo") %>',
                        data: "{'RepData' : '" + RepData + "','TenantID' : '" + Tenant + "','LoggedInUserID' : '" + <%=this.cp.UserID%> + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (response) {
                            showStickyToast(true, "Replenishment Details Saved Successfully", false);

                            GetItemMasterDetails(MaterialMasterID);
                            RepClear();
                            //location.reload();
                            //document.getElementById("panel7").focus();
                        }
                    });
            }


        }

        //Item Pictures
        function checkFileExtension(elem) {
            var filePath = elem.value;
            if (filePath.indexOf('.') == -1)
                return false;
            var validExtensions = new Array();
            var ext = filePath.substring(filePath.lastIndexOf('.') + 1).toLowerCase();
            validExtensions[0] = 'jpg';
            validExtensions[1] = 'jpeg';
            validExtensions[2] = 'png';
            validExtensions[3] = 'gif';
            for (var i = 0; i < validExtensions.length; i++) {
                if (ext == validExtensions[i])
                    return true;
            }
            elem.value = "";
            showStickyToast(false, 'The file extension ' + ext.toUpperCase() + ' is not allowed!', false);
            return false;
        }

        //Supplier Attachements Save
        $(function () {
            //Supplier Attachments
            var uploadObj = $("#fileuploader").uploadFile({
                url: "UploadMultiFiles.ashx",
                multiple: true,
                autoSubmit: false,
                fileName: "FileNames",
                allowedTypes: "doc,docx,pdf,jpg,jpeg,gif,png,xps,txt,PNG,JOG,GIF,JPEG",
                dynamicFormData: function () {
                    //debugger;
                    DocType = $("#ddlAttachType").val();
                    supplierID = $("#ddlSupplierID").val();
                    var Tenantid = $("#Tenant").val();
                    //alert(Tenantid);
                    var data = { Doctype: DocType, mid: MaterialMasterID, sid: supplierID, Tid: Tenantid }
                    return data;
                },
                showStatusAfterSuccess: true,
                abortStr: "Abort",
                cancelStr: "Cancel",
                doneStr: "Close",
                showDone: true,
                afterUploadAll: function () {
                    showStickyToast(true, "All attached files are successfully uploaded");
                    setTimeout(function () {
                        location.reload();
                    }, 1500);                   
                    //GetItemMasterDetails(MaterialMasterID);
                    
                    //document.getElementById("panel8").focus();
                },
                onSelect: function (files) {
                    //debugger;
                    var extension = files[0].name.split('.')[1];
                    var DocsType = $("#ddlAttachType").val();
                    if (DocsType == 1) {
                        if (extension == "png" || extension == "PNG" || extension == "jpg" || extension == "JPG" || extension == "gif" || extension == "GIF" || extension == "jpeg" || extension == "JPEG") {
                            return true;
                        }
                        else {
                            showStickyToast(false, "Please select Image file");
                            return false;
                        }
                    }
                    if (DocsType == 2) {
                        if (extension == ('pdf')) {
                            return true;
                        }
                        else {
                            showStickyToast(false, "Please select PDF file");
                            return false;
                        }
                    }
                },
                onError: function (files, status, errMsg) {
                    showStickyToast(false, "Error while uploading");
                },
                width: "5px"
            });
            $("#startUpload").click(function () {
                //debugger;
                DocType = $("#ddlAttachType").val();
                supplierID = $("#ddlSupplierID").val();
                if (supplierID == '' || supplierID == "PS") {
                    showStickyToast(false, "Please select supplier");
                    return false;
                }
                if (DocType == '' || DocType == "PS") {
                    showStickyToast(false, "Please select attachment type");
                    return false;
                }

                uploadObj.startUpload();

            });
        })

        function openDialog(title) {
            $("#divItemPrintData").dialog("option", "title", title);
            $("#divItemPrintData").dialog('open');

            NProgress.start();

            $("#divItemPrintData").block({
                message: '<img src="<%=ResolveUrl("~") %>Images/async_master.gif" />',
                    css: { border: '0px' },
                    fadeIn: 0,
                    fadeOut: 0,
                    overlayCSS: { backgroundColor: '#ffffff', opacity: 1 }
            });

                unblockDialog();
        }

        function unblockDialog() {
            $("#divItemPrintData").unblock();
            NProgress.done();
        }

        function successitemsave() {
            debugger;
            showStickyToast(true, "Successfully Uploaded Item Picture", false);
            setTimeout(function () {
                window.location.href='ItemMasterRequest.aspx?mid=' + MaterialMasterID;
                //location.reload();
            }, 1500);
        }
    </script>
    <style>
        #MainContent_MMContent_trvmaterialattachmentn0Nodes table td {
            display: inline-table !important;
            width: fit-content !important;
        }

        #MainContent_MMContent_trvmaterialattachment table td {
            display: inline-table !important;
            width: fit-content !important;
        }

        /*.panel-heading a:after {
            font-family: FontAwesome;
            content: "\f078";
            float: right;
            color: black;
        }*/

        /*.collapsed::after {
            transform:rotate(90deg) !important;
        }*/

        /*.panel-heading a.collapsed:after {
            content: "\f054";
        }*/

        .accord {
            color: #313030;
            font-weight: 400;
            cursor: pointer;
            font-family: Calibri,Verdana,Geneva,sans-serif !important;
            font-size: 13pt !important;
            display: block;
            width: 100%;
        }

        /*a:hover {
            text-decoration: none !important;
            color: #000 !important;
        }*/

        .txt_Blue_Small {
            border-bottom: 0px;
        }

        [type="file"] {
            border: 1px solid !important;
            border-color: #737373 !important;
            border-radius: 3px;
        }



   

        .field {
            border: 1px solid silver !important;
            margin: 0 2px !important;
            padding: .35em .625em .75em !important;
        }

        .lege {
            width: inherit;
            padding: 0 10px;
            border-bottom: none;
            font-size: 11pt !important;
        }

        .borderless td, .borderless th {
            border: none;
        }

        .borderless tr > td {
            border-top: none !important;
        }

        .btncolor {
            color: #000 !important;
        }

        .radio, .checkbox {
            display: inline-block;
        }

        .table-striped > tbody > tr:nth-child(odd) > th {
            background-color: #ffe4b8;
        }

        .ajax-upload-dragdrop {
            width: 100% !important;
            border: 2px outset var(--sideNav-bg) !important;
        }

        .ajax-upload-dragdrop {
            border: 2px outset #fac18a;
            border-radius: 5px;
            color: #dadce3;
            text-align: left;
            vertical-align: middle;
            padding: 1px 0px 0 1px;
            width: 300px;
            height: 38px;
        }

        .ajax-file-upload:hover {
            background: var(--sideNav-bg);
        }

        .ajax-file-upload {
            height: 30px !important;
            background: var(--sideNav-bg);
            box-shadow: 0 2px 0 0 #2b579a !important;
        }

        .ajax-file-upload-statusbar {
            width: 400px !important;
        }

        .ui-widget {
            width: auto !important;
        }

        .radio label, .checkbox label {
            vertical-align: middle !important;
        }

        select:not([size]):not([multiple]) {
            height: auto !important;
        }

        /*.field select,input,textarea{
                width: 150px;
                font-size: 14px;
        }*/

        /*.field .md-select-underline {
            width: 150px;
        }*/

        .panel-title {
            width: 100%;
        }

        /*.rotate::after {
            transform: rotate(90deg);
        }*/

        .ui-dialog {
            width: 500px !important;
            left: 535px !important;
        }

        .ui-autocomplete {
            width: 235px !important;
        }


        .panel-body .field .table tr td {
            border-bottom: 0px;
            padding: 0;
            font-size: 13px !important;
        }

        .btn-primary {
            margin-bottom: 10px;
        }
        /*i{
            color:#337ab7 !important;
        }*/

        .flex input[type="text"], input[type="number"], textarea {
            min-width: 100%;
        }

        .custom-file-input {
            color: transparent;
        }

        .txt_Blue_Small::-webkit-file-upload-button {
            visibility: hidden;
        }

        .txt_Blue_Small::before {
            content: 'Choose picture';
            color: #fff;
            display: inline-block;
            background: var(--sideNav-bg);
            border: 1px solid var(--sideNav-bg);
            border-radius: 3px;
            padding: 5px 6px;
            outline: none;
            white-space: nowrap;
            -webkit-user-select: none;
            cursor: pointer;
            /* text-shadow: 1px 1px #fff; */
            font-weight: 500;
            font-size: 9pt;
        }

        [type="file"] {
            width: 70% !important;
            border: 1px solid #fac18a !important;
            padding: 5px;
            margin-bottom: 5px;
        }

        .ctxt_Blue_Small:hover::before {
            border-color: black;
        }

        .txt_Blue_Small:active {
            outline: 0;
        }

            .txt_Blue_Small:active::before {
                background: -webkit-linear-gradient(top, #e3e3e3, #f9f9f9);
            }

        .btn-light {
            background-color: #fff !important;
            padding: 6px;
            border: 1px solid #f2f2f2;
            border-radius: 4px;
            box-shadow: var(--z1);
        }

        .partcheck {
            float: right;
            vertical-align: -webkit-baseline-middle;
            position: absolute;
            right: 0%;
            top: 50%;
        }

        select {
            width: 100% !important;
        }

     

        .flex {
            margin-bottom: 15px;
        }

        .modalclose {
            font-size: medium !important;
            background-color: #29328b;
            border: 0px solid #fff;
            cursor: pointer !important;
        }


        /*.accord:after {
            content: "\f054";
            width: 16px;
            color: transparent !important;
            height: 16px;
            background-image: url(../Images/downarrow.png) !important;
            background-size: contain;
        }*/

        .ui-state-disabled {
            display: none !important;
        }

        /*.collapsed:after {
            font-family: FontAwesome;
            content: "\f078";
            float: right;
            color: black;
            content: "\f054";
            width: 16px;
            color: transparent !important;
            height: 16px;
            background-image: url(../Images/downarrow.png) !important;
            background-size: contain;
        }*/

        .collapsed::after {
            transform: rotate(-90deg) !important;
        }



        @media (max-width:800px) {
           

            .excessds {
                height: 32px !important;
            }

            .w280 {
                width: 100% !important;
            }
        }

        .ui-autocomplete-input {
            --md-arrow-width: 1.39em !important;
            background: url(data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIyNCIgaGVpZ2h0PSIyNCIgdmlld0JveD0iMCAwIDI0IDI0Ij4KICAgIDxwYXRoIGQ9Ik0xNS41IDE0aC0uNzlsLS4yOC0uMjdDMTUuNDEgMTIuNTkgMTYgMTEuMTEgMTYgOS41IDE2IDUuOTEgMTMuMDkgMyA5LjUgM1MzIDUuOTEgMyA5LjUgNS45MSAxNiA5LjUgMTZjMS42MSAwIDMuMDktLjU5IDQuMjMtMS41N2wuMjcuMjh2Ljc5bDUgNC45OUwyMC40OSAxOWwtNC45OS01em0tNiAwQzcuMDEgMTQgNSAxMS45OSA1IDkuNVM3LjAxIDUgOS41IDUgMTQgNy4wMSAxNCA5LjUgMTEuOTkgMTQgOS41IDE0eiIvPgogICAgPHBhdGggZD0iTTAgMGgyNHYyNEgweiIgZmlsbD0ibm9uZSIvPgo8L3N2Zz4K) calc(100% - var(--md-arrow-offset) - var(--md-select-side-padding)) center no-repeat !important;
            background-size: var(--md-arrow-width) !important;
        }

        .row {
            margin:0px !important;
        }

        .ModuleHeader {
            margin-left:0px;
            width: calc(100% - 220px ) !important;
                padding: 0 !important;
        }

        .fixed-width {
            /* width: 100% !important; */
            width: calc(100% - 69px ) !important;
            margin-left: 8px !important;
        }
        textarea {
            padding: 0px 2px !important;
        }

.flex {
    position: relative;
    margin:5px 0px;
}
.panel{
    margin-bottom:0px !important
}
.panel-group .panel-heading{
    border-radius:0px !important;
}
    </style>

    <div class="container">
            <div class="flex__ end"><button type="button" id="btnList" onclick="GoToList()" class="btn btn-primary"><i class="material-icons vl">arrow_back</i> <%= GetGlobalResourceObject("Resource", "BackToList")%></button></div>
        <gap></gap>
        <div class="">
            <div class="row">
                <div class="col m6 s6">
                </div>
                <div class="col m6 s6">
                  
                </div>
            </div>
            <div>
                 <!-- Globalization Tag is added for multilingual  -->
                <div class="row">
                    <div class="panel-group" id="accordion">

                        <%----------------------------------------- Panel 1 ---------------------------------------------%>
                        <div class="panel panel-default panelborder" id="panel1">
                            <div class="panel-heading accordpanel">
                               <%-- <a class="accord collapsed" data-toggle="collapse" data-target="#collapseOne"> Basic Material Details </a>--%>
                                <a class="accord collapsed" data-toggle="collapse" data-target="#collapseOne">  <%= GetGlobalResourceObject("Resource", "BasicMaterialDetails")%></a>
                            </div>
                            <div id="collapseOne" class="panel-collapse collapse in">
                                <div class="panel-body" style="border-top-color: #fac18a !important;">
                                    <div class="">
                                        <div class="row">
                                            <div class="col m3 s3">
                                                <div class="flex">
                                                    <div>
                                                        <input type="text" id="txtAccount" class="ui-autocomplete-input" required="">
                                                          <%--<label> Account </label>--%>
                                                        <label> <%= GetGlobalResourceObject("Resource", "Account")%> </label>
                                                        <span class="errorMsg"></span> 
                                                        <input type="hidden" class="p1save" id="Account" />
                                                        <%--<select id="Account" required=""></select>--%>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col m3 s3">
                                                <div class="flex">
                                                    <div>
                                                        <input type="text" id="txtTenant" class="ui-autocomplete-input" required="">
                                                        <input type="hidden" class="p1save" id="Tenant" />
                                                        <%--<select id="Tenant" required=""></select>--%>
                                                        <%-- <label>Tenant </label>--%>
                                                        <label> <%= GetGlobalResourceObject("Resource", "Tenant")%></label>
                                                        <span class="errorMsg">*</span> 
                                                    </div>
                                                </div>
                                                <asp:HiddenField ID="hifTenant" runat="server" />
                                            </div>
                                            <div class="col m3 s3">
                                                <div class="flex">
                                                   
                                                    <div class="flex">
                                                        <input type="text" class="p1save" id="MCode" maxlength="20" onkeypress="return RestrictSpace()" required="">
                                                       <%-- <label>PartNumber  </label>--%>
                                                        <label>  <%= GetGlobalResourceObject("Resource", "PartNumber")%></label>
                                                        <span class="errorMsg">*</span> 
                                                        <span class="partcheck"><i class="fa fa-check" style="color: green !important; display: none;" id="partyes"></i><i class="fa fa-times" style="color: red !important; display: none;" id="partno"></i></span>
                                                    </div>
                                                </div>
                                                <asp:HiddenField ID="hdnPartNum" runat="server" />
                                                <asp:TextBox ID="txtMfgPartNo" runat="server" MaxLength="17" Width="200" Visible="false" />
                                            </div>
                                               <div class="col m3 s3">
                                                <div class="flex">
                                                    <div>
                                                        <input type="text" id="MGroup" class="ui-autocomplete-input" required="">
                                                     
                                                        <label>  <%= GetGlobalResourceObject("Resource", "MaterialGroup")%></label>
                                                        <span class="errorMsg">*</span> 
                                                        <input type="hidden" class="p1save" id="MGroupID" />
                                                    </div>
                                                </div>
                                            </div>
                                   </div>
                                    </div>
                                    <div class="row b15">
                                        <div class="" style="margin-top:15px !important;">
                                       <%--     <div class="col m3 s3">
                                                <div class="flex">
                                                    <div>
                                                        <input type="text" id="MGroup" class="ui-autocomplete-input" required="">
                                                     
                                                        <label>  <%= GetGlobalResourceObject("Resource", "MaterialGroup")%></label>
                                                        <span class="errorMsg">*</span> 
                                                        <input type="hidden" class="p1save" id="MGroupID" />
                                                    </div>
                                                </div>
                                            </div>--%>
                                            <div class="col m3 s3">
                                                <div class="flex">
                                                    <div>
                                                       
                                                        <%--<textarea class="p1save excessds" maxlength="400" id="MDescription" required=""></textarea>--%>
                                                          <input type="text" class="p1save excessds" id="MDescription" required="" maxlength="50"/>
                                                       <%-- <label>Item Description Short  </label>--%>
                                                        <label>  <%= GetGlobalResourceObject("Resource", "ItemDescriptionShort")%></label>
                                                        <span class="errorMsg">*</span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <br />

                                    <div class="row">
                                        <div class="col-md-12">
                                            <fieldset class="field flexlimit90">
                                                <%-- <legend class="lege">Material type  </legend>--%>
                                                <legend class="lege">  <%= GetGlobalResourceObject("Resource", "Materialtype")%></legend>
                                                <div class="col m4 s4">
                                                    <div class="flex">
                                                        <div>
                                                            
                                                        </div>
                                                        <div>
                                                            <select class="p1save" id="MTypeID" required="" style="width:100% !important;"></select>
                                                           <%--  <label> MaterialType </label>--%>
                                                            <label> <%= GetGlobalResourceObject("Resource", "MaterialType ")%> </label>
                                                            <span class="errorMsg"></span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-1"></div>
                                                <div class="col-md-7">
                                                    <div id="divMsps">
                                                    </div>
                                                </div>


                                            </fieldset>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col m4 s4">
                                            <fieldset class="field flexlimit80">
                                              <%--  <legend class="lege"> Storage Condition </legend>--%>
                                                <legend class="lege">  <%= GetGlobalResourceObject("Resource", "StorageCondition")%></legend>
                                                <div class="">
                                                    <div class="row">
                                                        <div class="col m12">
                                                            <div class="flex">
                                                                <div >
                                                                    <select class="p1save" id="StorageConditionID" required="" style="width: 100% !important;"></select>
                                                                     <%--<label>Storage Condition </label>--%>
                                                                    <label> <%= GetGlobalResourceObject("Resource", "StorageCondition")%></label>
                                                                    <span class="errorMsg"></span> 
                                                                    <asp:HiddenField ID="hifStorageConditionID" runat="server" />
                                                                </div>
                                                            </div>


                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col m12">
                                                            <div class="flex"> 
                                                                    <input type="text" class="p1save" id="MinShelfLifeinDays" maxlength="4" onkeypress="return isNumberKeyEvent(event)" required="">
                                                               <%-- <label> Min. Shelf Life(Days) </label>--%>
                                                                    <label> <%= GetGlobalResourceObject("Resource", "MinShelfLifeDays")%> </label>
                                                                    <span class="errorMsg" style="display: none" id="minshel"></span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col m12">
                                                            <div class="flex">
                                                                <div>
                                                                    <input type="text" class="p1save" id="TotalShelfLifeinDays" maxlength="4" onkeypress="return isNumberKeyEvent(event)" required="">
                                                                     <%--<label>Total Shelf Life(Days)</label>--%>
                                                                    <label><%= GetGlobalResourceObject("Resource", "TotalShelfLifeDays")%> </label>
                                                                    <span class="errorMsg" style="display: none;" id="maxshel"></span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </fieldset>
                                        </div>
                                        <div class="col-md-8">
                                            <fieldset class="field flexlimit80">
                                               <%-- <legend class="lege">Dimensions</legend>--%>
                                                 <legend class="lege"><%= GetGlobalResourceObject("Resource", "Dimensions")%></legend>
                                                <div class="">
                                                    <div class="row">
                                                        <%--<td class="FormLabels">
                                                            <div class="flex">
                                                                <div>
                                                                    <select class="p1save" id="MaterialSpaceUtilizationID" required=""></select>
                                                                    <label>Space Utilization</label>
                                                                    <span class="errorMsg">*</span> 
                                                                </div>
                                                            </div>
                                                        </td>--%>
                                                        <div class="col m6">
                                                            <div class="flex">
                                                                <div>
                                                                    <input type="text" class="p1save" maxlength="6" id="MLength" onkeypress="return checkDec(this,event)" required="">
                                                                   <%-- <label>Length (cm)</label>--%>
                                                                     <label><%= GetGlobalResourceObject("Resource", "Lengthcm")%> </label>
                                                                   <%-- <span class="errorMsg">*</span>--%>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col m6">
                                                            <div class="flex">
                                                                
                                                                <div>
                                                                    <input type="text" class="p1save" maxlength="6" id="MHeight" onkeypress="return checkDec(this,event)" required="">
                                                                   <%-- <label>Height (cm)</label>--%>
                                                                     <label><%= GetGlobalResourceObject("Resource", "Heightcm")%></label>
                                                                  <%--  <span class="errorMsg">*</span>--%>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        
                                                        <div class="col m6">
                                                            <div class="flex">
                                                                <div>
                                                                    <input type="text" class="p1save" maxlength="6" id="MWidth" onkeypress="return checkDec(this,event)" required="">
                                                                    <%--<label>Width (cm)</label>--%>
                                                                    <label> <%= GetGlobalResourceObject("Resource", "Widthcm")%> </label>
                                                                  <%--  <span class="errorMsg">*</span>--%>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col m6">
                                                            <div class="flex">
                                                                <div>
                                                                    <input type="text" class="p1save" maxlength="9" id="MWeight" onkeypress="return checkDec(this,event)" required="">
                                                                 <%--   <label>Weight (kgs)  </label>--%>
                                                                    <label> <%= GetGlobalResourceObject("Resource", "Weightkgs")%> </label>
                                                                   <%-- <span class="errorMsg">*</span>--%>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <%--<td class="FormLabels">
                                                    <div class="flex">
                                                        <div>
                                                            <label><span style="color: red">*</span> Weight (kgs)</label>
                                                        </div>
                                                        <div>
                                                            <input type="text" class="p1save" id="MWeight" onkeypress="return checkDec(this,event)" />
                                                        </div>
                                                    </div>
                                                </td>--%>
                                                    </div>
                                                    <div class="row">
                                                        
                                                        <div class="col m6">
                                                            <div class="flex">
                                                                <div>
                                                                    <input type="text" class="p1save" maxlength="4" id="CapacityPerBin" onkeypress="return isNumberKeyEvent(event)" required="">
                                                                   <%-- <label>Capacity Per Bin</label>--%>
                                                                     <label> <%= GetGlobalResourceObject("Resource", "CapacityPerBin")%> </label>
                                                                    <span class="errorMsg">*</span>

                                                                   
                                                                </div>
                                                            </div>

                                                        </div>
                                                        <div class="col m6">
                                                            <div class="flex">
                                                                <div>
                                                                   

                                                                     <select class="p1save" id="ProductCategoryID" required="" style="width:100% !important;"></select>
                                                               <%-- <label>Product Category</label>--%>
                                                                <asp:HiddenField ID="HiddenField2" runat="server" />
                                                                </div>
                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>
                                            </fieldset>
                                        </div>
                                    </div>
                                    <br />
                                    <div style="display: flex; justify-content:flex-end">
                                       <%-- <button type="button" id="savebasic" class="btn btn-primary " onclick="InsertBasic();">Save <i class='fa fa-floppy-o'></i></button>--%>
                                        <button type="button" id="savebasic" class="btn btn-primary " onclick="InsertBasic();">  <%= GetGlobalResourceObject("Resource", "Save")%>   <i class='fa fa-floppy-o'></i></button>
                                        <%--<button type="button" id="canbasic" class="btn btn-primary ">Cancel&nbsp;<i class="material-icons vl">cancel</i></button>--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%----------------------------------------- Panel 1 ---------------------------------------------%>

                        <%----------------------------------------- Panel 9 ---------------------------------------------%>
                        <div class="panel panel-default panelborder divIndustryData" id="divIndustryData" style="display: none;">
                            <div class="panel-heading accordpanel">
                                <h4 class="panel-title">
                                   <%-- <a class="accord collapsed" data-toggle="collapse" data-target="#collapseNine">Extended Attributes</a>--%>
                                     <a class="accord collapsed" data-toggle="collapse" data-target="#collapseNine"> <%= GetGlobalResourceObject("Resource", "ExtendedAttributes")%></a>
                                </h4>
                            </div>
                            <div id="collapseNine" class="panel-collapse collapse">
                                <div class="panel-body" id="divIndustryBody">
                                    <div class="row">
                                        <div class="col m3 s3">
                                            <div class="">
                                                <div class="flex">
                                                    <div></div>
                                                    <div>
                                                        <div class="flex">
                                                            <asp:TextBox ID="txtIndustry" runat="server" SkinID="txt_Req" required=""></asp:TextBox>
                                                            <%-- <label>Industry</label>--%>
                                                             <label> <%= GetGlobalResourceObject("Resource", "Industry")%></label>
                                                            <asp:HiddenField ID="hdnIndustry" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    
                                        <div class="">
                                            <div class="divGetIndustry">
                                              
                                                <div id="divIndustryContent">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="pull-right">
                                        <%--<button type="button" id="btnCreate" class="btn btn-primary " onclick="UpsertIndustry();">Save <i class='fa fa-floppy-o'></i></button>--%>
                                        <button type="button" id="btnCreate" class="btn btn-primary " onclick="UpsertIndustry();"> <%= GetGlobalResourceObject("Resource", "Save")%> <i class='fa fa-floppy-o'></i></button>
                                        <%--<button type="button" id="canindustry" class="btn btn-primary ">Cancel&nbsp;<i class="material-icons vl">cancel</i></button>--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%----------------------------------------- Panel 9 ---------------------------------------------%>

                        <%----------------------------------------- Panel 2 ---------------------------------------------%>
                        <div class="panel panel-default panelborder" id="panel2">
                            <div class="panel-heading accordpanel">
                                <h4 class="panel-title">
                                    <%--<a class="accord collapsed" data-toggle="collapse" data-target="#collapseTwo">Additional Information</a>--%>
                                    <a class="accord collapsed" data-toggle="collapse" data-target="#collapseTwo">  <%= GetGlobalResourceObject("Resource", "AdditionalInformation")%> </a>
                                </h4>
                            </div>
                            <div id="collapseTwo" class="panel-collapse collapse">
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col m3 s3">
                                            <div class="flex">
                                                <div>
                                                    <input type="text" class="p1save" maxlength="30" id="OEMPartNo" required="" onkeypress="return RestrictSpace()">
                                                   <%-- <label>OEM Part Number</label>--%>
                                                     <label> <%= GetGlobalResourceObject("Resource", "OEMPartNumber")%> </label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col m3 s3">
                                            <div class="flex">
                                                <div>
                                                    <input type="text" class="p1save" maxlength="30" id="MCodeAlternative1" required="" onkeypress="return RestrictSpace()">
                                                    <%-- <label>Alternative Part# 1</label>--%>
                                                     <label><%= GetGlobalResourceObject("Resource", "AlternativePart")%></label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col m3 s3">
                                            <div class="flex">
                                                <div>
                                                    <input type="text" class="p1save" maxlength="30" id="MCodeAlternative2" required="" onkeypress="return RestrictSpace()">
                                                    <%--<label>Alternative Part# 2</label>--%>
                                                    <label> <%= GetGlobalResourceObject("Resource", "AlternativeParts")%> </label>
                                                </div>
                                            </div>
                                        </div>
                                   
                                        <div class="col m3 s3" style=" display: none">
                                            <div class="flex">
                                                <div>
                                                    <select class="p1save" id="ProductCategoryID1" required="" style="width:100% !important;"></select>
                                                   <%-- <label>Product Category</label>--%>
                                                    <asp:HiddenField ID="hifProductCategories" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col m3 s3">
                                            <div class="flex">
                                                <div>
                                                    <textarea class="p1save" id="MDescriptionLong" maxlength="500"  required=""></textarea>
                                                   <%-- <label>Item Description [Long]</label>--%>
                                                     <label>  <%= GetGlobalResourceObject("Resource", "ItemDescriptionLong")%> </label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                    <div flex end>
                                       <%-- <button type="button" id="saveadd" class="btn btn-primary " onclick="AddlnInfoSave();">Save <i class='fa fa-floppy-o'></i></button>--%>
                                         <button type="button" id="saveadd" class="btn btn-primary " onclick="AddlnInfoSave();"> <%= GetGlobalResourceObject("Resource", "Save")%>  <i class='fa fa-floppy-o'></i></button>
                                        <%--<button type="button" id="canadd" class="btn btn-primary ">Cancel&nbsp;<i class="material-icons vl">cancel</i></button>--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%----------------------------------------- Panel 2 ---------------------------------------------%>

                        <%----------------------------------------- Panel 3 ---------------------------------------------%>
                        <div class="panel panel-default panelborder" id="panel3">
                            <div class="panel-heading accordpanel">
                                <h4 class="panel-title">
                                  <%--  <a class="accord collapsed" data-toggle="collapse" data-target="#collapseThree">Supplier Details </a>--%>
                                      <a class="accord collapsed" data-toggle="collapse" data-target="#collapseThree"> <%= GetGlobalResourceObject("Resource", "SupplierDetails")%> </a>
                                </h4>
                            </div>
                            <div id="collapseThree" class="panel-collapse collapse">
                                <div class="panel-body">
                                    <div class="" flex end>
                                        <%--<button type="button" id="lnkAddSupplier" class="btn btn-primary " data-toggle="modal" data-target="#SupModal">Add Supplier <i class="material-icons">add</i></button>--%>
                                        <button type="button" id="lnkAddSupplier" class="btn btn-primary " data-toggle="modal" data-target="#SupModal"> <%= GetGlobalResourceObject("Resource", "AddSupplier")%>  <i class="material-icons">add</i></button>
                                        <gap5></gap5>
                                    </div>
                                    <gap5></gap5>
                                    <!-- Modal -->
                                    <div id="SupModal" class="modal">
                                        <div class="modal-dialog" role="document">
                                            <div class="modal-content">
                                                <div class="modal-header" style="background-color: var(--sideNav-bg) !important; color:#fff !important;">
                                                   <%-- <h4 class="modal-title" style="display: inline !important;">Add Supplier</h4>--%>
                                                     <h4 class="modal-title" style="display: inline !important;"><%= GetGlobalResourceObject("Resource", "AddSupplier")%> </h4>
                                                    <button type="button"  data-dismiss="modal" class="pull-right modalclose" aria-label="Close">
                                                        <span aria-hidden="true">&times;</span>
                                                    </button>
                                                </div>
                                                <div class="modal-body" id="mySupForm">
                                                    <div class="row">
                                                        <div class="col m6 s6">
                                                            <div class="flex">
                                                                <div>
                                                                    <input type="text" id="Supname" class="ui-autocomplete-input" required=""/>
                                                                    <input type="hidden" id="ddlSupplier" />
                                                                    <%--<select id="ddlSupplier" required=""></select>--%>
                                                                    <%--<label>Supplier</label>--%>
                                                                    <label> <%= GetGlobalResourceObject("Resource", "Supplier")%> </label>
                                                                    <span class="errorMsg">*</span> 
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col m6 s6">
                                                            <div class="flex">
                                                                <div>
                                                                    <input type="text" id="suppartnum" maxlength="30" required="" onkeypress="return RestrictSpace()">
                                                                    <%--<label>EAN</label>--%>
                                                                    <label> <%= GetGlobalResourceObject("Resource", "EAN")%> </label>
                                                                    <span class="errorMsg">*</span> 
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col m6 s6">
                                                            <div class="flex">
                                                                <div>
                                                                    <select id="ddlCurrency" required=""></select>
                                                                   <%-- <label>Currency</label>--%>
                                                                     <label> <%= GetGlobalResourceObject("Resource", "Currency")%> </label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col m6 s6">
                                                            <div class="flex">
                                                                <div>
                                                                    <input type="text" id="UnitCost" maxlength="6" onkeypress="return isNumberKeyEvent(event)" required="">
                                                                    <%-- <label>Expected Unit Cost</label>--%>
                                                                     <label> <%= GetGlobalResourceObject("Resource", "ExpectedUnitCost")%> </label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col m6 s6">
                                                            <div class="flex">
                                                                <div>
                                                                    <input type="text" id="PlannedTime" maxlength="3" onkeypress="return isNumberKeyEvent(event)" required="">
                                                                    <%--<label>Planned Delivery Time(Days)</label>--%>
                                                                    <label><%= GetGlobalResourceObject("Resource", "PlannedDeliveryTimeDays")%> </label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col m6 s6">
                                                            <div class="flex">
                                                                <div>
                                                                    <input type="text" id="SupQuantity" maxlength="6" onkeypress="return isNumberKeyEvent(event)" required="">
                                                                  <%--  <label>Min. Order Qty</label>--%>
                                                                      <label><%= GetGlobalResourceObject("Resource", "MinOrderQty")%> </label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="modal-footer">
                                                    <input type="hidden" id="MMT_SUPPLIER_ID" />
                                                    <%--<input type="reset" value="reset" />--%>
                                                   <%-- <button type="button" class="btn btn-primary" style="color:#fff !important;" onclick="mySupclear();">Clear</button>--%>
                                                     <button type="button" class="btn btn-primary" style="color:#fff !important;" onclick="mySupclear();"><%= GetGlobalResourceObject("Resource", "Clear")%> </button>
                                                   <%-- <button type="button" class="btn btn-primary" style="color:#fff !important;" data-dismiss="modal">Close</button>--%>
                                                     <button type="button" class="btn btn-primary" style="color:#fff !important;" data-dismiss="modal"><%= GetGlobalResourceObject("Resource", "Close")%> </button>
                                                   <%-- <button type="button" class="btn btn-primary" onclick="AddSupSave();">Save</button>--%>
                                                     <button type="button" class="btn btn-primary" onclick="AddSupSave();"> <%= GetGlobalResourceObject("Resource", "Save")%> </button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- Modal -->
                                    <div class="row" style="margin-top: 8% !important; margin: auto !important;" id="SupDetailsTable">
                                        <%--Supplier Details Table Append--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%----------------------------------------- Panel 3 ---------------------------------------------%>

                        <%----------------------------------------- Panel 4 ---------------------------------------------%>
                        <div class="panel panel-default panelborder" id="panel4">
                            <div class="panel-heading accordpanel">
                                <h4 class="panel-title">
                                   <%-- <a class="accord collapsed" data-toggle="collapse" data-target="#collapseFour">Unit of Measurement(UoM) Configuration</a>--%>
                                     <a class="accord collapsed" data-toggle="collapse" data-target="#collapseFour">  <%= GetGlobalResourceObject("Resource", "UnitofMeasurementUoMConfiguration")%></a>
                                </h4>
                            </div>
                            <div id="collapseFour" class="panel-collapse collapse">
                                <div class="panel-body">
                                    <table class=" borderless">
                                        <tr>
                                            <td colspan="2" class="FormLabels">
                                                <div class="flex__">
                                                    
                                                    <div class="flex">
                                                        <select id="ddlMeasure">
                                                             <option value="1"><%= GetGlobalResourceObject("Resource", "Length")%> </option>                                                
                                                             <option value="4"><%= GetGlobalResourceObject("Resource", "Weight")%> </option>                                                         
                                                             <option value="5"> <%= GetGlobalResourceObject("Resource", "LiquidMeasurements")%></option>
                                                            <option value="0" selected="selected"><%= GetGlobalResourceObject("Resource", "Other")%> </option>
                                                        </select>
                                                         <label> <%= GetGlobalResourceObject("Resource", "MeasurementType")%> </label>
                                                    </div>
                                                </div>
                                            </td>
                                            <td class="pull-right">
                                              
                                                 <button type="button" id="UoMConversion" class="btn-light" style="margin-top:4px !important;cursor:no-drop;" onclick="UoMShow();"> <%= GetGlobalResourceObject("Resource", "UoMConversion")%> <span class="space fa fa-cog"></span></button>
                                             </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="measures" style="display:none;">
                                                    <div class="flex">                                                        
                                                        <select id="ddlFromMeasure" required=""></select>
                                                   
                                                         <label> <%= GetGlobalResourceObject("Resource", "From")%>  </label>
                                                        <span class="errorMsg"></span>
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="measures" style="display:none;">
                                                    <div class="flex">  
                                                        <div>
                                                            <select id="ddlToMeasure" required=""></select>
                                                            <%--<label>To </label>--%>
                                                            <label> <%= GetGlobalResourceObject("Resource", "To")%> </label>
                                                            <span class="errorMsg"></span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </td>
                                            <td class="pull-right">
                                                <div class="uomdiv" style="display:none;">
                                                    <div style="background-color: #e6f2f8; color: black; font-weight: bold; text-align: center; padding:10px;vertical-align:middle !important;">
                                                        <label id="UomResult"></label>
                                                    </div>    
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <%--<asp:UpdatePanel ChildrenAsTriggers="true" ID="upUoM" UpdateMode="Always" runat="server">

                                        <ContentTemplate>
                                            <table class="table borderless">
                                                <tr>
                                                    <%--<td class="FormLabels">Measurement Type:
                                                <br />
                                                <select class="txt_slim txt_Blue_Small" id="ddlMeasureType"></select>
                                            </td>--%>
                                                <%--<tr>
                                                    <td colspan="2" class="FormLabels">
                                                        <div class="flex__">
                                                            <div>
                                                                <label style="width: 160px;">Measurement Type</label>
                                                            </div>
                                                            <div>
                                                                <asp:DropDownList ID="ddlMeadureType" OnSelectedIndexChanged="ddlMeadureType_SelectedIndexChanged" runat="server" AutoPostBack="true">
                                                                    <asp:ListItem Text="Length" Value="1"></asp:ListItem>--%>
                                                                    <%--<asp:ListItem Text="Area" Value="2"></asp:ListItem>--%>
                                                                    <%--<asp:ListItem Text="Volume" Value="3"></asp:ListItem>--%>
                                                                    <%--<asp:ListItem Text="Weight" Value="4"></asp:ListItem>
                                                                    <asp:ListItem Text="Liquid Measurements" Value="5"></asp:ListItem>--%>
                                                                    <%--<asp:ListItem Text="Temperature" Value="6"></asp:ListItem>--%>
                                                                    <%--<asp:ListItem Text="Other" Value="0" Selected="True"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <br />
                                                        <asp:LinkButton runat="server" ID="lnkUoMConversion" Font-Underline="false" OnClick="lnkUoMConversion_Click">UoM Conversion <span class="space fa fa-cog"></span></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr id="trMeasurements" runat="server" visible="false">
                                                    <td class="FormLabels">From:<br />
                                                        <asp:DropDownList ID="ddlFromMeasurement" OnSelectedIndexChanged="ddlFromMeasurement_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                                    </td>
                                                    <td class="FormLabels">To:<br />
                                                        <asp:DropDownList ID="ddlToMeasurement" runat="server" OnSelectedIndexChanged="ddlToMeasurement_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                    </td>
                                                    <td visible="false"  align="left" class="FormLabelsBlue" id="tdConversion" runat="server">
                                                        <br />
                                                        <div style="background-color: #e6f2f8; color: black; font-weight: bold; text-align: center; width:170px;padding:10px;">
                                                            <asp:Label ID="lbConversion" runat="server" ClientIDMode="Static" Font-Size="15px"></asp:Label>
                                                        </div>
                                                        
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>

                                    </asp:UpdatePanel>--%>
                                    <div class="" flex end>
                                       <%-- <button type="button" id="UOMAdd" class="btn btn-primary " data-toggle="modal" data-target="#UoMModal" onclick="myUoMclear()">Add UoM <i class="material-icons">add</i></button>--%>
                                         <button type="button" id="UOMAdd" class="btn btn-primary " data-toggle="modal" data-target="#UoMModal" onclick="myUoMclear()"> <%= GetGlobalResourceObject("Resource", "AddUoM")%> <i class="material-icons">add</i></button>
                                        <gap5></gap5>
                                    </div>
                                    <gap></gap>
                                    <!-- Modal -->
                                    <div id="UoMModal" class="modal">
                                        <div class="modal-dialog" role="document">
                                            <div class="modal-content">
                                                <div class="modal-header" style="background-color: var(--sideNav-bg) !important; color:#fff !important;">
                                                    <%--<h4 class="modal-title" style="display: inline !important;">Add UoM</h4>--%>
                                                    <h4 class="modal-title" style="display: inline !important;"> <%= GetGlobalResourceObject("Resource", "AddUoM")%></h4>
                                                    <button type="button"  data-dismiss="modal" class="pull-right modalclose" aria-label="Close">
                                                        <span aria-hidden="true">&times;</span>
                                                    </button>
                                                </div>
                                                <div class="modal-body">
                                                    <div class="row">
                                                        <div class="col m4 s4">
                                                            <div class="flex">
                                                                <div>
                                                                    <select id="ddlUoM" required=""></select>
                                                                   <%-- <label>UoM Type</label><span class="errorMsg">*</span>--%>
                                                                     <label> <%= GetGlobalResourceObject("Resource", "UoMType")%></label><span class="errorMsg">*</span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col m4 s4">
                                                            <div class="flex">
                                                                <div>
                                                                    <select id="UoM" required=""></select>
                                                                    <%--<label>UoM</label><span class="errorMsg">*</span>--%>
                                                                    <label>  <%= GetGlobalResourceObject("Resource", "UoM")%> </label><span class="errorMsg">*</span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col m4 s4">
                                                            <div class="flex">
                                                                <div>
                                                                    <input type="text" id="QtyPerUoM" maxlength="3" onkeypress="return isNumberKeyEvent(event)" required="">
                                                                    <%--<label>Qty.Per UoM</label>--%>
                                                                    <label> <%= GetGlobalResourceObject("Resource", "QtyPerUoM")%></label>
                                                                    <span class="errorMsg">*</span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="modal-footer">
                                                    <input type="hidden" id="UOM_TYPE_ID" />
                                                   <%-- <button type="button" class="btn btn-primary" style="color:#fff !important;" onclick="myUoMclear();">Clear</button>--%>
                                                     <button type="button" class="btn btn-primary" style="color:#fff !important;" onclick="myUoMclear();"><%= GetGlobalResourceObject("Resource", "Clear")%> </button>
                                                   <%-- <button type="button" class="btn btn-primary" style="color:#fff !important;" data-dismiss="modal">Close</button>--%>
                                                     <button type="button" class="btn btn-primary" style="color:#fff !important;" data-dismiss="modal"><%= GetGlobalResourceObject("Resource", "Close")%>  </button>
                                                    <%--<button type="button" class="btn btn-primary" onclick="UoMSave();">Save</button>--%>
                                                    <button type="button" class="btn btn-primary" onclick="UoMSave();"> <%= GetGlobalResourceObject("Resource", "Save")%></button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- Modal -->
                                    <div class="row" style="margin-top: 8% !important; margin: auto !important;" id="UoMDetailsTable">
                                        <%--UoM Details Table Append--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%----------------------------------------- Panel 4 ---------------------------------------------%>

                        <%----------------------------------------- Panel 5 ---------------------------------------------%>
                        <div class="panel panel-default panelborder" id="panel5">
                            <div class="panel-heading accordpanel">
                                <h4 class="panel-title">
                                   <%-- <a class="accord collapsed" data-toggle="collapse" data-target="#collapseFive">Inward Inspection Checkpoint</a>--%>
                                     <a class="accord collapsed" data-toggle="collapse" data-target="#collapseFive"> <%= GetGlobalResourceObject("Resource", "InwardInspectionCheckpoint")%></a>
                                </h4>
                            </div>
                            <div id="collapseFive" class="panel-collapse collapse">
                                <div class="panel-body">
                                    <div class="" flex end>
                                        <%--<button type="button" id="QcAdd" class="btn btn-primary " data-toggle="modal" data-target="#QcModal">Add QC Parameters <i class="material-icons">add</i></button>--%>
                                        <button type="button" id="QcAdd" class="btn btn-primary " data-toggle="modal" data-target="#QcModal"><%= GetGlobalResourceObject("Resource", "AddQCParameters")%> <i class="material-icons">add</i></button>
                                        <gap5></gap5>
                                    </div>
                                    <gap></gap>
                                    <!-- Modal -->
                                    <div id="QcModal" class="modal">
                                        <div class="modal-dialog" role="document">
                                            <div class="modal-content">
                                                <div class="modal-header" style="background-color: var(--sideNav-bg) !important; color:#fff !important;">
                                                    <%--<h4 class="modal-title" style="display: inline !important;">Add QC Parameters</h4>--%>
                                                    <h4 class="modal-title" style="display: inline !important;"><%= GetGlobalResourceObject("Resource", "AddQCParameters")%> </h4>
                                                    <button type="button"  data-dismiss="modal" class="pull-right modalclose" aria-label="Close">
                                                        <span aria-hidden="true">&times;</span>
                                                    </button>
                                                </div>
                                                <div class="modal-body">
                                                    <div class="row">
                                                        <div class="col m6 s6">
                                                            <div class="flex">
                                                                <div>
                                                                    <select id="ddlParameter" required="" style="    width: 100% !important;"></select>
                                                                    <%--<label>Parameter Name</label>--%>
                                                                    <label><%= GetGlobalResourceObject("Resource", "ParameterName")%> </label>
                                                                    <span class="errorMsg">*</span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col m6 s6">
                                                            <div class="flex">
                                                                <div>
                                                                    <input type="text" maxlength="6" id="minTol" required="">
                                                                   <%-- <label>Min. Threshold</label>--%>
                                                                     <label> <%= GetGlobalResourceObject("Resource", "MinThreshold")%> </label>
                                                                    <span class="errorMsg">*</span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col m6 s6">
                                                            <div class="flex">
                                                                <div>
                                                                    <input type="text" maxlength="6" id="maxTol" required="">
                                                                   <%-- <label>Max. Threshold</label>--%>
                                                                     <label> <%= GetGlobalResourceObject("Resource", "MaxThreshold")%> </label>
                                                                    <span class="errorMsg">*</span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col m6 s6">
                                                            <div class="">
                                                                <br />
                                                                <div class="checkbox">
                                                                    <input type="checkbox" id="QCReq" required="">
                                                                   <%-- <label>Is Required</label>--%>
                                                                     <label> <%= GetGlobalResourceObject("Resource", "IsRequired")%></label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="modal-footer">
                                                    <input type="hidden" id="vMMT_QualityParameterID" />
                                                   <%-- <button type="button" class="btn btn-primary" style="color:#fff !important;" onclick="myQCclear();">Clear</button>--%>
                                                     <button type="button" class="btn btn-primary" style="color:#fff !important;" onclick="myQCclear();"> <%= GetGlobalResourceObject("Resource", "Clear")%> </button>
                                                    <%--<button type="button" class="btn btn-primary" style="color:#fff !important;" data-dismiss="modal">Close</button>--%>
                                                    <button type="button" class="btn btn-primary" style="color:#fff !important;" data-dismiss="modal"><%= GetGlobalResourceObject("Resource", "Close")%></button>
                                                   <%-- <button type="button" class="btn btn-primary" onclick="QcSave();">Save</button>--%>
                                                     <button type="button" class="btn btn-primary" onclick="QcSave();">  <%= GetGlobalResourceObject("Resource", "Save")%></button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- Modal -->
                                    <div class="row" style="margin-top: 8% !important; margin: auto !important;" id="QcTable">
                                        <%--QC Details Table Append--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%----------------------------------------- Panel 5 ---------------------------------------------%>

                        <%----------------------------------------- Panel 6 ---------------------------------------------%>
                        <div class="panel panel-default panelborder" id="panel6">
                            <div class="panel-heading accordpanel">
                                <h4 class="panel-title">
                                   <%-- <a class="accord collapsed" data-toggle="collapse" data-target="#collapseSix">Replenishment</a>--%>
                                     <a class="accord collapsed" data-toggle="collapse" data-target="#collapseSix"> <%= GetGlobalResourceObject("Resource", "Replenishment")%></a>
                                </h4>
                            </div>
                            <div id="collapseSix" class="panel-collapse collapse">
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col m6 s6">
                                            <fieldset class="field flexlimit80">
                                               <%-- <legend class="lege">Bin Replenishment:</legend>--%>
                                                 <legend class="lege"> <%= GetGlobalResourceObject("Resource", "BinReplenishment")%></legend>
                                                <div class="row">
                                                    <div class="col m6 s6">
                                                        <div class="flex">
                                                            <div>
                                                                <%--<select id="LocationID"></select>--%>
                                                                <input type="text" id="LocationText" class="ui-autocomplete-input"  required="">
                                                                <%--<label>Bin</label>--%>
                                                                <label><%= GetGlobalResourceObject("Resource", "Bin")%> </label>
                                                                <input type="hidden" id="LocationID" />
                                                                <asp:HiddenField ID="hiflocation" runat="server" />
                                                                <asp:HiddenField ID="hifbinid" runat="server" />
                                                                <asp:HiddenField ID="hifexstingloc" runat="server" />
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col m6 s6">
                                                        <div class="flex">
                                                            <div>
                                                                <input type="text" id="MinimumStockLevel" maxlength="6" onkeypress="return isNumberKeyEvent(event)" required="">
                                                                <%--<label>Minimum Stock Level</label>--%>
                                                                <label><%= GetGlobalResourceObject("Resource", "MinimumStockLevel")%> </label>
                                                                <span class="errorMsg binstock" style="display:none;">*</span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col m6 s6">
                                                        <div class="flex">
                                                            <div>
                                                                <input type="text" id="MaximumStockLevel" maxlength="6" onkeypress="return isNumberKeyEvent(event)" required="">
                                                                 <%--<label>Maximum Stock Level</label>--%>
                                                                <label><%= GetGlobalResourceObject("Resource", "MaximumStockLevel")%> </label>
                                                                 <span class="errorMsg binstock" style="display:none;">*</span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row" id="BinTable">
                                                    <%--Bin Replenishment Table Append--%>
                                                </div>
                                            </fieldset>
                                        </div>
                                        <div class="col m6 s6">
                                            <fieldset class="field flexlimit80" >
                                                <%--<legend class="lege">Material Replenishment:</legend>--%>
                                                <legend class="lege"><%= GetGlobalResourceObject("Resource", "MaterialReplenishment")%> </legend>
                                                <div class="row">
                                                    <div class="col m6 s6">
                                                        <div class="flex">
                                                            <div>
                                                                <select id="WarehouseID" required=""></select>
                                                                <%--<label>Warehouse</label>--%>
                                                                <label><%= GetGlobalResourceObject("Resource", "Warehouse")%></label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col m6 s6">
                                                        <div class="flex">
                                                            <div>
                                                                <input type="text" id="ReorderQtyMin" maxlength="9" onkeypress="return isNumberKeyEvent(event)" required="">
                                                                <%--<label>Reorder Qty.Min. </label>--%>
                                                                <label><%= GetGlobalResourceObject("Resource", "ReorderQtyMin")%> </label>
                                                                <span class="errorMsg matstock" style="display:none;">*</span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col m6 s6">
                                                        <div class="flex">
                                                            <div>
                                                                <input type="text" id="ReorderQtyMax" maxlength="9" onkeypress="return isNumberKeyEvent(event)" required="">
                                                                <%--<label>Reorder Qty. Max.</label>--%>
                                                                <label><%= GetGlobalResourceObject("Resource", "ReorderQtyMax")%></label>
                                                                <span class="errorMsg matstock" style="display:none;">*</span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row" id="MaterialTable">
                                                    <%--Material Replenishment Table Append--%>
                                                </div>
                                            </fieldset>
                                        </div>
                                    </div>
                                    <gap5></gap5>
                                    <div class="" flex end>
                                        <%--<button type="button" id="saverep" class="btn btn-primary " onclick="RepSave();">Save <i class='fa fa-floppy-o'></i></button>--%>
                                        <button type="button" id="saverep" class="btn btn-primary " onclick="RepSave();"><%= GetGlobalResourceObject("Resource", "Save")%> <i class='fa fa-floppy-o'></i></button>
                                        <%--<button type="button" id="canrep" class="btn btn-primary ">Cancel&nbsp;<i class="material-icons vl">cancel</i></button>--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%----------------------------------------- Panel 6 ---------------------------------------------%>

                        <%----------------------------------------- Panel 7 ---------------------------------------------%>
                        <div class="panel panel-default panelborder" id="panel7">
                            <div class="panel-heading accordpanel">
                                <h4 class="panel-title">
                                    <%--<a class="accord collapsed" data-toggle="collapse" data-target="#collapseSeven">Item Pictures</a>--%>
                                    <a class="accord collapsed" data-toggle="collapse" data-target="#collapseSeven"><%= GetGlobalResourceObject("Resource", "ItemPictures")%></a>
                                </h4>
                            </div>
                            <div id="collapseSeven" class="panel-collapse collapse">
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col m4 s6">
                                            <div class="">
                                                <div>
                                                    <%--<label>Attach Picture File</label>--%>
                                                    <label><%= GetGlobalResourceObject("Resource", "AttachPictureFile")%></label>
                                                </div>
                                                <div>
                                                    <asp:FileUpload ID="fuItemPicture" AllowMultiple="true" CssClass="custom-file-input" onchange="return checkFileExtension(this);" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col m6 s6">
                                            <div class="flex">
                                                <div>
                                                    <asp:Literal ID="ltPicHolder" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="pull-right">
                                       <%-- <asp:LinkButton ID="ItemPicSave" runat="server" OnClick="ItemPicSave_Click" CssClass="btn btn-primary ">Save <i class='fa fa-floppy-o'></i></asp:LinkButton>--%>
                                         <asp:LinkButton ID="ItemPicSave" runat="server" OnClick="ItemPicSave_Click" CssClass="btn btn-primary "><%= GetGlobalResourceObject("Resource", "Save")%> <i class='fa fa-floppy-o'></i></asp:LinkButton>
                                        <%--<button type="button" id="canpic" class="btn btn-primary ">Cancel&nbsp;<i class="material-icons vl">cancel</i></button>--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%----------------------------------------- Panel 7 ---------------------------------------------%>

                        <%----------------------------------------- Panel 8 ---------------------------------------------%>
                        <div class="panel panel-default panelborder" id="panel8">
                            <div class="panel-heading accordpanel">
                                <h4 class="panel-title">
                                    <%--<a class="accord collapsed" data-toggle="collapse" data-target="#collapseEight">Supplier Attachments</a>--%>
                                    <a class="accord collapsed" data-toggle="collapse" data-target="#collapseEight"><%= GetGlobalResourceObject("Resource", "SupplierAttachments")%></a>
                                </h4>
                            </div>
                            <div id="collapseEight" class="panel-collapse collapse">
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col m3 s3">
                                            <div class="flex">
                                                <div>
                                                    <select id="ddlSupplierID" required=""></select>
                                                     <%--<label>Supplier</label>--%>
                                                    <label> <%= GetGlobalResourceObject("Resource", "Supplier")%></label>
                                                    <span class="errorMsg">*</span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col m3 s3">
                                            <div class="flex">
                                                <div>
                                                    <select class="txt_slim txt_Blue_Small" id="ddlAttachType" required=""></select>
                                                   
                                                    <label>  <%= GetGlobalResourceObject("Resource", "AttachmentType")%></label>
                                                    <span class="errorMsg">*</span>
                                                </div>
                                            </div>
                                        </div> 
                                        <div class="col m3 s3">
                                            <div class="flex">
                                              
                                                <div id="fileuploader"><%= GetGlobalResourceObject("Resource", "Upload")%></div>
                                            </div>
                                        </div>  
                                        <div class="col m3 s3">
                                           
                                            <div>
                                                <div flex end>
                                                    <asp:Label ID="lblvieweattachment" runat="server"></asp:Label></>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row"> 
                                        <gap></gap>
                                        <div class="col m1 offset-m11">
                                             <flex end><button type="button" id="startUpload" class="btn btn-primary "><%= GetGlobalResourceObject("Resource", "Save")%> <i class='fa fa-floppy-o'></i></button></flex>
                                        </div>
                                    </div>                                    
                                  
                                </div>
                            </div>
                        </div>
                        <%----------------------------------------- Panel 8 ---------------------------------------------%>


                        <%----------------------------------------- Panel 10 ---------------------------------------------%>
                        <div class="panel panel-default panelborder" id="panel10">
                            <div class="panel-heading accordpanel">
                                <h4 class="panel-title">
                                    <%--<a class="accord collapsed" data-toggle="collapse" data-target="#collapseTen">Material Preferences</a>--%>
                                    <a class="accord collapsed" data-toggle="collapse" data-target="#collapseTen"><%= GetGlobalResourceObject("Resource", "MaterialPreferences")%></a>
                                </h4>
                            </div>
                            <div id="collapseTen" class="panel-collapse collapse">
                                <div class="panel-body">
                                    <input type="hidden" value="0" id="GEN_TRN_Preference_ID" />
                                    <div id="divPreferences" style="display: none;"></div>
                                </div>
                            </div>
                        </div>
                        <%----------------------------------------- Panel 10 ---------------------------------------------%>

                        <!-- Modal For Delete -->
                                    <div id="DelModal" class="modal">
                                              <div class="modal-dialog" role="document" style="height:100px;width:350px">
                                            <div class="modal-content">
                                                <div class="modal-header" style="background-color: var(--sideNav-bg) !important; color:#fff !important;">
                                                    <%--<h4 class="modal-title" style="display: inline !important;">Delete Record</h4>--%>
                                                    <h4 class="modal-title" style="display: inline !important;"><%= GetGlobalResourceObject("Resource", "Confirmation")%></h4>
                                                    <button type="button"  data-dismiss="modal" class="pull-right modalclose" aria-label="Close">
                                                        <span aria-hidden="true">&times;</span>
                                                    </button>
                                                </div>
                                                <div class="modal-body">
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="flex">
                                                                <div>
                                                                   <%-- <p>Are you sure to delete ?</p>--%>
                                                                      <span style="font-size:larger;"><%= GetGlobalResourceObject("Resource", "Areyousuretodelete")%></span>
                                                                </div>
                                                            </div>
                                                        </div>                                                        
                                                    </div>                                                   
                                                </div>
                                                <div class="modal-footer">
                                                    <button type="button" class="btn btn-primary" id="yesdel"><%= GetGlobalResourceObject("Resource", "Yes")%></button>
                                                    <button type="button" class="btn btn-primary" style="color:#fff !important;" data-dismiss="modal"><%= GetGlobalResourceObject("Resource", "No")%> </button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- Modal -->

                        <div id="divItemPrintData">
                            <div id="divItemPrintDataContainer" style="display: block;float:left !important; padding:10px !important;">

                                <asp:TreeView ID="trvmaterialattachment" Target="_blank" runat="server">
                                    <Nodes>
                                        <asp:TreeNode Expanded="false"></asp:TreeNode>
                                    </Nodes>
                                </asp:TreeView>
                                <asp:Label ID="lblfileslist" runat="server"></asp:Label>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var ItemList = null;
        MasterID = new URL(window.location.href).searchParams.get("mid");
        // alert(MasterID);
        if (MasterID != null) {
            //debugger;
            $("#divPreferences").css("display", "block");
        }

        //Get Material Preferences

        function GoToList() {
            window.location.href = "MaterialMasterList.aspx";
        }
        function GetPreferencesList() {


            if (MasterID != null)
            {
                $.ajax({
                    url: '<%=ResolveUrl("~/mMaterialManagement/ItemMasterRequest.aspx/GetPreferences") %>',
                    //data: "{ 'prefix': '" + request.term + "'}",
                    data: "{'MaterialMasterID' : '" + MasterID + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (response) {
                        var dt = JSON.parse(response.d);
                        ItemList = dt;
                        GetPrefernces();
                        BindPreferences(MasterID);
                    },
                    error: function (response) {

                    },
                    failure: function (response) {

                    }
                });
            }


            //debugger;
            
        }

        GetPreferencesList();

        function isNumberKeyEvent(e) {
            //debugger;
            if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                return false;
            }
        }

        function RestrictSpace() {
            if (event.keyCode == 32) {
                return false;
            }

        }

        //=============================== Added by MD.Prasad 01-03-2018 for disable control on LIFO AND FIFO ======================//
        function CheckFIFOandLIFO() {
            //debugger;
            $(".LFCheck").each(function () {
                var prefid = $(this).attr("id");
                var LFCheck = $(this).attr("data-attr-LF");
                var val = $(this).val();
                if (LFCheck == "FIFO") {
                    if (val != "") {
                        $(".LFIFIDLO").attr("disabled", true);
                    }
                    else {
                        $(".LFIFIDLO").attr("disabled", false);
                    }
                }

                if (LFCheck == "LIFO") {
                    if (val != "") {
                        $(".LFIFIDFO").attr("disabled", true);
                    }
                    else {
                        $(".LFIFIDFO").attr("disabled", false);
                    }
                }
                //alert(prefid);
            });
        }
        //=============================== Added by MD.Prasad 01-03-2018 for disable control on LIFO AND FIFO ======================//

        function GetPrefernces() {
            var displayid = "";
            displaypreferenceid = "";
            var PreferenceContainer = document.getElementById('divPreferences');
            var PreferenceContent = '';
            if (ItemList.Table != null && ItemList.Table.length > 0) {

                var GroupList = $.grep(ItemList.Table, function (a) { return a.GroupName == "Material Group" });

                //PreferenceContent += '<div class="panel panel-default panelborder" id="panel10">< div class="panel-heading accordpanel" ><h4 class="panel-title"><a class="accord collapsed" data-toggle="collapse" data-target="#collapseTen">' + GroupList[0].GroupName + '</a></h4></div ><div id="collapseTen" class="panel-collapse collapse">';
                //PreferenceContent += '<div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="divPreferenceHeader">' + GroupList[0].GroupName + '</div><div class="ui-Customaccordion" id="divPreferenceBody" style="text-align:left;">';
                PreferenceContent += '<div id="divPreferenceHeader"></div><div id="divPreferenceBody" style="text-align:left;">';

                if (GroupList != null && GroupList.length > 0) {
                    var PrerenceList = $.grep(ItemList.Table1, function (a) { return a.GEN_MST_PreferenceGroup_ID == GroupList[0].GEN_MST_PreferenceGroup_ID });
                    PreferenceContent += '<div style="width:100%;padding:10px;">';
                    for (var i = 0; i < PrerenceList.length; i++) {
                        PreferenceContent += '<div style=""><b>' + PrerenceList[i].PreferenceName + ' :</b><p></p><p></p><p></p><br/><div class="row">';
                        var OptionList = $.grep(ItemList.Table2, function (a) { return a.GEN_MST_Preference_ID == PrerenceList[i].GEN_MST_Preference_ID });
                        for (var j = 0; j < OptionList.length; j++) {
                            if (PrerenceList[i].UIControlType == "TextBox") {
                                //PreferenceContent += '<div><b>' + OptionList[j].OptionCode + '</b></div>';

                                //=============================== Added by MD.Prasad 01-03-2018 for disable control on LIFO AND FIFO ======================//
                                var LF = "";
                                var LFID = "";
                                if (OptionList[j].OptionLabel == "FIFO") {
                                    LF = OptionList[j].OptionLabel;
                                    LFID = "FO";
                                }
                                else if (OptionList[j].OptionLabel == "LIFO") {
                                    LF = OptionList[j].OptionLabel;
                                    LFID = "LO";
                                }
                                else {
                                    LF = "";
                                    LFID = "";
                                }
                                if (OptionList[j].OptionLabel == "FIFO" || OptionList[j].OptionLabel == "LIFO") {
                                    PreferenceContent += '<div class="col m3 s3"><div class="flex"><input type="text" required="" class="GetPreferenceOptions seqcheck LFCheck LFIFID' + LFID + '" id="pref' + OptionList[j].GEN_MST_PreferenceOption_ID + '" name="' + PrerenceList[i].GEN_MST_Preference_ID + '" data-attr="' + PrerenceList[i].GEN_MST_PreferenceGroup_ID + '" value="" data-attr-id="0" data-attr-LF="' + LF + '" onkeypress="return isNumberKeyEvent(event)" onchange="CheckFIFOandLIFO()" style="display:inline !important;"><label>' + OptionList[j].OptionLabel + '</label></div></div>';
                                }
                                //=============================== Added by MD.Prasad 01-03-2018 for disable control on LIFO AND FIFO ======================//
                                else {

                                    PreferenceContent += '<div class="col m3 s3"><div class="flex"><input type="text" required="" class="GetPreferenceOptions seqcheck" id="pref' + OptionList[j].GEN_MST_PreferenceOption_ID + '" name="' + PrerenceList[i].GEN_MST_Preference_ID + '" data-attr="' + PrerenceList[i].GEN_MST_PreferenceGroup_ID + '" value="" data-attr-id="0" onkeypress="return isNumberKeyEvent(event)" style="display:inline !important;"><label>' + OptionList[j].OptionLabel + '</label></div></div>';
                                }
                            }//onkeypress="return checkDec(this,event);myValidate();myDecimal();"
                            else if (PrerenceList[i].UIControlType == "RadioButton") {
                                //PreferenceContent += '<div><b>' + OptionList[j].OptionCode + '</b></div>';
                                PreferenceContent += '<div><input type="radio" class="GetPreferenceOptions" id="' + OptionList[j].GEN_MST_PreferenceOption_ID + '" name="' + PrerenceList[i].GEN_MST_Preference_ID + '" value="' + GroupList[0].GEN_MST_PreferenceGroup_ID + '" data-attr-id="0"><lable for="' + OptionList[j].GEN_MST_PreferenceOption_ID + '"> ' + OptionList[j].OptionLabel + '</lable></div><br>';
                            }
                            else {
                                PreferenceContent += '<div class=""><input type="checkbox" class="GetPreferenceOptions" id="' + OptionList[j].GEN_MST_PreferenceOption_ID + '" name="' + PrerenceList[i].GEN_MST_Preference_ID + '" value="' + GroupList[0].GEN_MST_PreferenceGroup_ID + '" data-attr-id="0"><lable for="' + OptionList[j].GEN_MST_PreferenceOption_ID + '"> ' + OptionList[j].OptionLabel + '</lable></div><br>';
                            }
                        }
                        PreferenceContent += '</div></div>&emsp;';
                        if (i % 1 == 0) {
                            PreferenceContent += '<p></p>';
                        }
                    }
                    PreferenceContent += '</div>';
                }
                PreferenceContent += '<div style="text-align:right;padding:5px 23px; overflow:hidden; padding-right: 0;"><button type="button" class="btn btn-primary" onclick="UpsertPreferences()">Save <i class="fa fa-floppy-o"></i></button>';
                //PreferenceContent += '&nbsp;<button type="button" id="canmatpref" class="btn btn-primary ">Cancel <i class="material-icons vl">cancel</i></button></div>';
                PreferenceContainer.innerHTML = PreferenceContent;
                CustomAccordino($('#divPreferenceHeader'), $('#divPreferenceBody'));
            }
            else {
                $(".PrefereModule").css("display", "none");
            }
        }

        //Bind Material Preferences
        function BindPreferences(MaterialId) {
            var PreferenceData = $.grep(ItemList.Table3, function (a) { return a.MaterialMasterID == MaterialId });
            FillPreferenceData(PreferenceData);
        }

        //Fill Preferences when Get
        function FillPreferenceData(obj) {
            //debugger;
            if (obj != null && obj.length > 0) {
                for (var i = 0; i < obj.length; i++) {
                    if (obj[i].UIControlType == "TextBox") {
                        var $id = $('#pref' + obj[i].GEN_MST_PreferenceOption_ID);
                        $id.val(obj[i].Value);
                        $id.attr("data-attr-id", obj[i].GEN_TRN_Preference_ID);


                        //=============================== Added by MD.Prasad 01-03-2018 for disable control on LIFO AND FIFO ======================//
                        if (obj[i].OptionLabel == "FIFO") {
                            if (obj[i].Value != "" || obj[i].Value != null) {
                                $('.LFIFIDLO').attr("disabled", true);
                            }
                            else {
                                $('.LFIFIDLO').attr("disabled", false);
                            }
                        }

                        if (obj[i].OptionLabel == "LIFO") {
                            if (obj[i].Value != "" || obj[i].Value != null) {
                                $('.LFIFIDFO').attr("disabled", true);
                            }
                            else {
                                $('.LFIFIDFO').attr("disabled", false);
                            }
                        }
                        //=============================== Added by MD.Prasad 01-03-2018 for disable control on LIFO AND FIFO ======================//
                    }
                    else if (obj[i].UIControlType == "CheckBox") {
                        $('#' + obj[i].GEN_MST_PreferenceOption_ID).prop("checked", true);
                        $('#' + obj[i].GEN_MST_PreferenceOption_ID).attr("data-attr-id", obj[i].GEN_TRN_Preference_ID);
                    }
                    else {
                        $('#' + obj[i].GEN_MST_PreferenceOption_ID).prop("checked", true);
                        $('#' + obj[i].GEN_MST_PreferenceOption_ID).attr("data-attr-id", obj[i].GEN_TRN_Preference_ID);
                    }
                }
            }
        }

        //XML for Material Preferences
        function GetPrefernceFromData() {
            // var fieldDataOut = '{';
            var fieldData = '<root>';
            $(".GetPreferenceOptions").each(function () {
                var param = $(this).attr('id');
                var val = $(this).val().trim();
                var trnid = $(this).attr("data-attr-id");
                //var trnid = trnsid == "undefined" ? 0 : trnsid;
                var paramtype = $(this).attr('type');
                if (paramtype == "radio" || paramtype == "checkbox") {
                    val = $(this).prop('checked');
                    if (val == true) {
                        var GroupID = $(this).val().trim();
                        var PreferenceID = $(this)[0].name;
                        var OptionID = ($(this)[0].id).replace("pref", "");
                        var OrgEntityID = 3;
                        fieldData += '<data>';
                        fieldData += '<Value>1</Value>';
                        fieldData += '<GEN_MST_OrgEntity_ID>' + OrgEntityID + '</GEN_MST_OrgEntity_ID>';
                        fieldData += '<EntityID>' + MasterID + '</EntityID>';
                        fieldData += '<GEN_MST_PreferenceGroup_ID>' + GroupID + '</GEN_MST_PreferenceGroup_ID>';
                        fieldData += '<GEN_MST_Preference_ID>' + PreferenceID + '</GEN_MST_Preference_ID>';
                        fieldData += '<GEN_MST_PreferenceOption_ID>' + OptionID + '</GEN_MST_PreferenceOption_ID>';
                        fieldData += '<GEN_TRN_Preference_ID>' + trnid + '</GEN_TRN_Preference_ID></data>';
                    }

                    else {
                        var GroupID = $(this).val().trim();
                        var PreferenceID = $(this)[0].name;
                        var OptionID = ($(this)[0].id).replace("pref", "");
                        var OrgEntityID = 3;
                        fieldData += '<data>';
                        fieldData += '<Value></Value>';
                        fieldData += '<GEN_MST_OrgEntity_ID>' + OrgEntityID + '</GEN_MST_OrgEntity_ID>';
                        fieldData += '<EntityID>' + MasterID + '</EntityID>';
                        fieldData += '<GEN_MST_PreferenceGroup_ID>' + GroupID + '</GEN_MST_PreferenceGroup_ID>';
                        fieldData += '<GEN_MST_Preference_ID>' + PreferenceID + '</GEN_MST_Preference_ID>';
                        fieldData += '<GEN_MST_PreferenceOption_ID>' + OptionID + '</GEN_MST_PreferenceOption_ID>';
                        fieldData += '<GEN_TRN_Preference_ID>' + trnid + '</GEN_TRN_Preference_ID></data>';
                    }
                }
                else {

                    val = $(this).val();
                    if (val == null || val == "") {
                        var Value = $(this).val().trim();
                        var GroupID = $(this).attr("data-attr");
                        var PreferenceID = $(this)[0].name;
                        var OptionID = ($(this)[0].id).replace("pref", "");
                        var OrgEntityID = 3;
                        fieldData += '<data>';
                        fieldData += '<Value></Value>';
                        fieldData += '<GEN_MST_OrgEntity_ID>' + OrgEntityID + '</GEN_MST_OrgEntity_ID>';
                        fieldData += '<EntityID>' + MasterID + '</EntityID>';
                        fieldData += '<GEN_MST_PreferenceGroup_ID>' + GroupID + '</GEN_MST_PreferenceGroup_ID>';
                        fieldData += '<GEN_MST_Preference_ID>' + PreferenceID + '</GEN_MST_Preference_ID>';
                        fieldData += '<GEN_MST_PreferenceOption_ID>' + OptionID + '</GEN_MST_PreferenceOption_ID>';
                        fieldData += '<GEN_TRN_Preference_ID>' + trnid + '</GEN_TRN_Preference_ID></data>';
                    }

                    else {
                        var Value = $(this).val().trim();
                        var GroupID = $(this).attr("data-attr");
                        var PreferenceID = $(this)[0].name;
                        var OptionID = ($(this)[0].id).replace("pref", "");
                        var OrgEntityID = 3;
                        fieldData += '<data>';
                        fieldData += '<Value>' + Value + '</Value>';
                        fieldData += '<GEN_MST_OrgEntity_ID>' + OrgEntityID + '</GEN_MST_OrgEntity_ID>';
                        fieldData += '<EntityID>' + MasterID + '</EntityID>';
                        fieldData += '<GEN_MST_PreferenceGroup_ID>' + GroupID + '</GEN_MST_PreferenceGroup_ID>';
                        fieldData += '<GEN_MST_Preference_ID>' + PreferenceID + '</GEN_MST_Preference_ID>';
                        fieldData += '<GEN_MST_PreferenceOption_ID>' + OptionID + '</GEN_MST_PreferenceOption_ID>';
                        fieldData += '<GEN_TRN_Preference_ID>' + trnid + '</GEN_TRN_Preference_ID></data>';
                    }
                }
            });
            fieldData = fieldData + '</root>';
            return fieldData;
        }

        function InsertPreference() {
            var status = false, value;
            $(".GetPreferenceOptions").each(function () {
                var param = $(this).attr('id');
                var paramtype = $(this).attr('type');
                if (paramtype == "radio" || paramtype == "checkbox") {
                    value = $(this).prop('checked');
                    if (value == true) {
                        status = true;
                    }
                }
                else {
                    value = $(this).val();
                    if (value != null) {
                        status = true;
                    }
                }
            });
            return status;
        }

        function UpsertPreferences() {
            //debugger;

            if (checkZeroValidation() == false) {
                showStickyToast(false, "Preference cannot be Zero", false);
                return false;
            }

            if (SeqCheck()) {
                showStickyToast(false, "Sequence Number Already Exists !", false);
                return false;
            }
            else {
                var data = GetPrefernceFromData();
                var obj = {};
                obj.UserID = "<%=cp.UserID.ToString()%>";
                obj.Inxml = GetPrefernceFromData();
                $.ajax({
                    url: "ItemMasterRequest.aspx/SETPreferences",
                    dataType: 'json',
                    contentType: "application/json",
                    type: 'POST',
                    data: JSON.stringify(obj),
                    success: function (response) {
                        if (response.d == "success") {
                            showStickyToast(true, 'Preferences Saved Successfully ', false);
                            //alert("Saved Successfully");
                            GetItemMasterDetails(MaterialMasterID);
                            //location.reload();
                            //GetPreferencesList();
                        }
                    }
                });
            }
        }


        function checkZeroValidation() {
            //debugger;
            var status = true;
            $('.seqcheck').each(function () {
                var checkzero = $(this).val();
                if (checkzero == "0") {
                    status = false;
                    return status;
                }
            });
            return status;
        }

        // Load Industries 
        var textfieldname = $("#<%= this.txtIndustry.ClientID %>");
        DropdownFunction(textfieldname);
        $("#<%= this.txtIndustry.ClientID %>").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadIndustries_Auto") %>',
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
                $("#<%=hdnIndustry.ClientID %>").val(i.item.val);
                $(".divIndustryData").css("display", "block");
                $("#divMaterialActions").css("display", "none");
                //if (MasterID != null) {
                //    $("#btnCreate").html('Update <i class="fa fa-database" aria-hidden="true"></i>');
                //}
                //else {
                //    $("#btnCreate").html('Send Request <i class="fa fa-floppy-o" aria-hidden="true"></i>');
                //}
                getIndustryFromid(i.item.val);
            },
            minLength: 0
        });

        // Load Industries 
        function GetIndustries()
        {
            if (MasterID != null)
            {
                //alert($("#Account").val());
                $.ajax({
                    url: '<%=ResolveUrl("~/mMaterialManagement/ItemMasterRequest.aspx/GetIndustries") %>',
                    data: "{'MaterialMasterID' : '" + MasterID + "','AccountID' : '" + $("#Account").val() + "','TenantId': '" + $("#Tenant").val() + "'}",
                     dataType: "json",
                     type: "POST",
                     contentType: "application/json; charset=utf-8",
                     success: function (response) {

                         var dt = JSON.parse(response.d);
                         IndItemList = dt;
                         //GetPrefernces();
                         //BindPreferences(MasterID);
                         //LoadDropDowns(dt);

                         if (MasterID != null) {
                             //debugger;
                             GetIndustryID(MasterID);
                         }
                     },
                     error: function (response) {

                     },
                     failure: function (response) {

                     }
                 });
            }        
        }
        //GetIndustries();

        //Get Industry Attributes
        function GetIndustryAttributes(industryID) {
            debugger;
            if (industryID != 0) {
                var item = $.grep(IndItemList.Table, function (a) { return a.GEN_MST_Industry_ID == industryID });
                if (item != null && item.length > 0) {
                    //$(".divIndustryData").css("display", "block");
                    $(".divGetIndustry").css("display", "block");
                    var container = document.getElementById('divIndustryContent');
                    var IndustryContent = '';
                    IndustryContent += '<div>';
                    //var m = 0;
                    for (var i = 0; i < item.length; i++) {
                        debugger;
                        if (item[i].UIControlType == "DatePicker") {
                            if (item[i].IsMandatory == true) {
                                IndustryContent += '<div class="col m3 s3"> <div class="flex"> <div><input type="text" class="txt_Blue_Small DueDate IndustryfieldToGet IsMandatory" id="' + item[i].MM_MST_Attribute_ID + '" data-attr="' + item[i].UILabelText +'" readonly="true"/><label class="">' + item[i].UILabelText + '</label><span class="errorMsg"></span></div></div></div>';
                            }
                            else {
                                IndustryContent += '<div class="col m3 s3"> <div class="flex"> <div><input type="text" class="txt_Blue_Small DueDate IndustryfieldToGet" id="' + item[i].MM_MST_Attribute_ID + '" data-attr="' + item[i].UILabelText +'" readonly="true"/><label class="">' + item[i].UILabelText + '</label></div></div></div>';
                            }
                            // m = m + 1;
                        }
                       
                        else if (item[i].UIControlType == "DropdownList") {
                            debugger;
                            if (item[i].HasDependency == 1 && (item[i].MM_MST_DependsOnAttribute_ID != null || item[i].MM_MST_DependsOnAttribute_ID != 0)) {
                                if (item[i].IsMandatory == true) {
                                    IndustryContent += '<div class="col m3 s3"> <div class="flex"><div> <select class="width214 txt_Blue_Small IndustryfieldToGet IsMandatory" id="' + item[i].MM_MST_Attribute_ID + '" data-attr="' + item[i].UILabelText + '"></select><label class="">' + item[i].UILabelText + '</label><span class="errorMsg"></span></div></div></div>';
                                }
                                else {
                                    IndustryContent += '<div class="col m3 s3"> <div class="flex"><div> <select class="width214 txt_Blue_Small IndustryfieldToGet" id="' + item[i].MM_MST_Attribute_ID + '" data-attr="' + item[i].UILabelText + '"></select><label class="">' + item[i].UILabelText + '</label></div></div></div>';
                                }
                            }
                            else {
                                if (item[i].IsMandatory == true) {
                                    IndustryContent += '<div class="col m3 s3"> <div class="flex"><div> <select class="width214 txt_Blue_Small IndustryfieldToGet IsMandatory" id="' + item[i].MM_MST_Attribute_ID + '" data-attr="' + item[i].UILabelText + '" onchange="getchildattributelist(this);"></select><label class="">' + item[i].UILabelText + '</label><span class="errorMsg"></span></div></div></div>';
                                }
                                else {
                                    IndustryContent += '<div class="col m3 s3"> <div class="flex"><div> <select class="width214 txt_Blue_Small IndustryfieldToGet" id="' + item[i].MM_MST_Attribute_ID + '" data-attr="' + item[i].UILabelText + '" onchange="getchildattributelist(this);"></select><label class="">' + item[i].UILabelText + '</label></div></div></div>';
                                }
                            }
                        }
                        else if (item[i].UIControlType == "TextBox") {
                            if (item[i].IsMandatory == true)
                            {
                                if (item[i].ValidationCode == "Number") {
                                    IndustryContent += '<div class="col m3 s3"> <div class="flex"> <div><input type="text" class="txt_Blue_Small IndustryfieldToGet IsMandatory" id="' + item[i].MM_MST_Attribute_ID + '" data-attr="' + item[i].UILabelText +'" onkeypress="return isNumberKeyEvent(event)"  required=""/><label class="">' + item[i].UILabelText + '</label><span class="errorMsg"></span></div></div></div>';
                                }
                                else if (item[i].ValidationCode == "Decimal") {
                                    IndustryContent += '<div class="col m3 s3"> <div class="flex"> <div><input type="text" class="txt_Blue_Small IndustryfieldToGet IsMandatory zerocheckIndustry" id="' + item[i].MM_MST_Attribute_ID + '" data-attr="' + item[i].UILabelText +'"  required="" onkeypress="return isNumberKeyEvent(event)"/><label class="">' + item[i].UILabelText + '</label><span class="errorMsg"></span></div></div></div>';
                                }
                                else {
                                    IndustryContent += '<div class="col m3 s3"> <div class="flex"> <div><input type="text" class="txt_Blue_Small IndustryfieldToGet IsMandatory zerocheckIndustry" id="' + item[i].MM_MST_Attribute_ID + '" data-attr="' + item[i].UILabelText +'"  required=""/><label class="">' + item[i].UILabelText + '</label><span class="errorMsg"></span></div></div></div>';
                                }
                            }
                            else
                            {
                                if (item[i].ValidationCode == "Number") {
                                    IndustryContent += '<div class="col m3 s3"> <div class="flex"> <div><input type="text" class="txt_Blue_Small IndustryfieldToGet" id="' + item[i].MM_MST_Attribute_ID + '" data-attr="' + item[i].UILabelText +'" onkeypress="return isNumberKeyEvent(event)"  required=""/><label class="">' + item[i].UILabelText + '</label></div></div></div>';
                                }
                                else if (item[i].ValidationCode == "Decimal") {
                                    IndustryContent += '<div class="col m3 s3"> <div class="flex"> <div><input type="text" class="txt_Blue_Small IndustryfieldToGet zerocheckIndustry" id="' + item[i].MM_MST_Attribute_ID + '" data-attr="' + item[i].UILabelText +'"  required="" onkeypress="return isNumberKeyEvent(event)"/><label class="">' + item[i].UILabelText + '</label></div></div></div>';
                                }
                                else {
                                    IndustryContent += '<div class="col m3 s3"> <div class="flex"> <div><input type="text" class="txt_Blue_Small IndustryfieldToGet zerocheckIndustry" id="' + item[i].MM_MST_Attribute_ID + '" data-attr="' + item[i].UILabelText +'"  required=""/><label class="">' + item[i].UILabelText + '</label></div></div></div>';
                                   // IndustryContent += '<div class="col m3 s3"> <div class="flex"> <div><input type="text" class="txt_Blue_Small IndustryfieldToGet zerocheckIndustry" id="' + item[i].MM_MST_Attribute_ID + '" data-attr="' + item[i].UILabelText +'"  required=""/><label class="">' + item[i].UILabelText + '</label></div></div></div>';
                                }
                            }
                        }
                    }

                    IndustryContent += '</div>';
                    container.innerHTML = IndustryContent;
                    // getIndustryData();
                    for (var i = 0; i < item.length; i++) {
                        var attrlist = $.grep(IndItemList.Table1, function (a) { return a.GEN_MST_Industry_ID == item[i].GEN_MST_Industry_ID });
                        var attrlistwithselectdata = $.grep(attrlist, function (a) { return a.MM_MST_Attribute_ID == item[i].MM_MST_Attribute_ID });
                        if (item[i].UIControlType == "DropdownList") {
                            BindInvDropdowns(attrlistwithselectdata, item[i].MM_MST_Attribute_ID);
                        }
                        else if (item[i].UIControlType == "DatePicker") {
                            //$("#" + item[i].MM_MST_Attribute_ID).val("");
                            datepicker();
                        }
                    }
                }
                else {
                    $(".divIndustryData").css("display", "block");
                    $(".divGetIndustry").css("display", "none");
                    //$("#divMaterialActions").css("display", "block");
                }
            }
            else {
                $(".divGetIndustry").css("display", "none");
                // $("#divMaterialActions").css("display", "block");
                $(".divIndustryData").css("display", "block");
            }
        }

        function BindInvDropdowns(dt, attributeid) {
            //KeyText, KeyValue
            if (dt != null && dt != '') {
                for (var x = 0; x < dt.length; x++) {
                    if (x == 0) {
                        $('#' + dt[x].MM_MST_Attribute_ID).empty();
                        $("#" + dt[x].MM_MST_Attribute_ID).append($("<option></option>").val(0).html("Please Select"));
                    }
                    $("#" + dt[x].MM_MST_Attribute_ID).append($("<option></option>").val(dt[x].KeyValue).html(dt[x].KeyText));
                }
            }
            else {
                $('#' + attributeid).empty();
                $("#" + attributeid).append($("<option></option>").val(0).html("Please Select"));
            }
        }


        function checkNegativeValue() {
            var value = parseFloat(document.getElementById("TextBox").value);
            if (value < 0) {
                showStickyToast(false, "Negative Value is not allowed");

                return false;
            }
        }

        function GetIndustryID(MID) {
            //debugger;
            if (MasterID != null) {
                $(".divIndustryData").css("display", "block");
                $("#divMaterialActions").css("display", "none");
                //if (MasterID != null) {
                //    $("#btnCreate").html('Update <i class="fa fa-database" aria-hidden="true"></i>');
                //}
                //else {
                //    $("#btnCreate").html('Send Request <i class="fa fa-floppy-o" aria-hidden="true"></i>');
                //}
                var indusid = $("#<%=hdnIndustry.ClientID %>").val();
                //var ind = $("#<%=hdnIndustry.ClientID %>").val();
                //var ind = indusid == '' ? 0 : indusid;
                getIndustryFromid(indusid);
            }
        }


        function getIndustryFromid(id) {
            debugger;
            GetIndustryAttributes(id);
            var item = $.grep(IndItemList.Table3, function (a) { return a.GEN_MST_Industry_ID == id });
            GetAttributes(item);
        }

        function GetAttributes(obj) {
            BindGetINDAttributes(obj);
        }

        var LookupData = null;

        function BindGetINDAttributes(obj) {
            debugger;
            //KeyText, KeyValue
            var dt = $.grep(obj, function (a) { return a.MM_MST_Material_ID == MasterID });
            LookupData = dt;
            if (dt != null && dt != '') {
                for (var x = 0; x < dt.length; x++) {
                    if (dt[x].KeyValue == 0) {
                        $("#" + dt[x].MM_MST_Attribute_ID).val(dt[x].AttributeValue);
                    }
                    else {
                        var Indobj = $.grep(IndItemList.Table3, function (a) { return a.MM_MST_Attribute_ID == dt[x].MM_MST_Attribute_ID && a.KeyValue == dt[x].KeyValue });
                        if (Indobj[0].LookupFilterValue == 0) {
                            $("#" + dt[x].MM_MST_Attribute_ID).val(dt[x].KeyValue);
                        }
                        else {
                            var filterObj = $.grep(IndItemList.Table1, function (a) { return a.LookupFilterValue == Indobj[0].LookupFilterValue });
                            //BinLookupData(Indobj);
                            BindInvDropdowns(filterObj);
                           $("#" + Indobj[0].MM_MST_Attribute_ID).val(dt[x].KeyValue);
                        }
                    }
                }
            }
        }

        function BinLookupData(obj) {
            var dt = $.grep(IndItemList.Table3, function (a) { return a.LookupFilterValue == obj[0].LookupFilterValue });
            if (dt != null && dt != '') {
                for (var x = 0; x < dt.length; x++) {
                    if (x == 0) {
                        $('#' + dt[x].MM_MST_Attribute_ID).empty();
                        $("#" + dt[x].MM_MST_Attribute_ID).append($("<option></option>").val(0).html("Please Select"));
                    }
                    $("#" + dt[x].MM_MST_Attribute_ID).append($("<option></option>").val(dt[x].KeyValue).html(dt[x].KeyText));
                }
                var Lookupid = $.grep(LookupData, function (a) { return a.MM_MST_Attribute_ID == dt[0].MM_MST_Attribute_ID });
                $("#" + Lookupid[0].MM_MST_Attribute_ID).val(Lookupid[0].KeyValue);
            }
        }

        function datepicker() {
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

        function getchildattributelist(filterValue) {
            debugger;
            if (filterValue != null) {
                var attributeid = filterValue.id;
                if (filterValue.value != 0) {
                    var attrlist = $.grep(IndItemList.Table1, function (a) { return a.LookupFilterValue == filterValue.value });
                    if (attrlist != null && attrlist.length > 0) {
                        BindInvDropdowns(attrlist, attrlist[0].MM_MST_Attribute_ID);
                    }
                    else {
                        var filetrlist = $.grep(IndItemList.Table2, function (a) { return a.MM_MST_DependsOnAttribute_ID == attributeid });
                        EmptyChildAttributes(filetrlist, attributeid);
                    }
                }
                else {
                    var filetrlist = $.grep(IndItemList.Table2, function (a) { return a.MM_MST_DependsOnAttribute_ID == attributeid });
                    EmptyChildAttributes(filetrlist, attributeid);
                }
            }
        }

        function EmptyChildAttributes(dataList, attributeid) {
            for (var x = 0; x < dataList.length; x++) {
                $('#' + dataList[x].MM_MST_Attribute_ID).empty();
                $("#" + dataList[x].MM_MST_Attribute_ID).append($("<option></option>").val(0).html("Please Select"));
            }
        }



        function getIndustryData() {

            // alert($('.IndustryfieldToGet').length);
            //debugger;
            var AttrfieldDataOut = '{';
            var AttrfieldData = '<root>';
            if (MasterID != null) {
                MasterID = MasterID;
            }
            else {
                MasterID = 0;
            }



            $('.IndustryfieldToGet').each(function () {
                var date = new Date();
                var param = $(this).attr('id');

                var val = $(this).val() == null ? "" : $(this).val().trim();
                var paramtype = $(this).attr('type');
                if (val == "0" || val == "") {
                } else {
                    AttrfieldData += '<data>';
                    AttrfieldData += '<MM_MST_Material_ID>' + MasterID + '</MM_MST_Material_ID>';
                    if (paramtype == undefined) {

                        AttrfieldData += '<MM_MST_AttributeLookup_ID>' + val + '</MM_MST_AttributeLookup_ID>';
                    }
                    else {
                        AttrfieldData += '<AttributeValue>' + val + '</AttributeValue>';
                        AttrfieldData += '<MM_MST_AttributeLookup_ID>' + 0 + '</MM_MST_AttributeLookup_ID>';
                    }

                    AttrfieldData += '<MM_MST_Attribute_ID>' + param + '</MM_MST_Attribute_ID>';
                    AttrfieldData += '<CreatedBy>' + <%=cp.UserID.ToString()%> + '</CreatedBy>' + '<UpdatedBy>' + <%=cp.UserID.ToString()%> + '</UpdatedBy>' + '<CreatedOn>' + date + '</CreatedOn>' + '<UpdatedOn>' + date + '</UpdatedOn>' + '<NewMM_MST_Material_ID>' + 0 + '</NewMM_MST_Material_ID></data>';
                }
             });

            AttrfieldData = AttrfieldData + '</root>';
            AttrfieldDataOut = AttrfieldData;
            return AttrfieldDataOut;
        }

        function UpsertIndustries() {
            //debugger;
            //UpsertIndustry();
            if (MasterID != null) {
                MasterID = MasterID;
            }
            else {
                MasterID = 0;
            }

            if (checkZeroValidationForIndustry() == false) {
                showStickyToast(false, "Industry Attributes cannot be Zero", false);
                return false;
            }

            var checkmand = checkIsmandatory();

            if (checkmand == false) {
                SetIndustryAttributes();
            }
        }


        function checkIsmandatory() {
            var status = false;
            $(".IsMandatory").each(function () {
                debugger;
                var param = $(this).attr('id');
                var val = $(this).val() == null ? "" : $(this).val().trim();
                var paramtype = $(this).attr('type');
                var lableText = $(this).attr('data-attr');
                if (paramtype == undefined) {
                    if (val == "0") {
                        status = true;
                        showStickyToast(false, "Please Select " + lableText + " .", false);
                        return false;
                    }
                }
                else {
                    if (val == "") {
                        status = true;
                        showStickyToast(false, "Please Enter " + lableText + " .", false);
                        return false;
                    }
                }
            });
            return status;
        }

        function SetIndustryAttributes() {
            Tenant = $("#Tenant").val();
            var obj = {};
            obj.UserID = "<%=cp.UserID.ToString()%>";
            obj.Inxml = getIndustryData();
            obj.MM_MST_Material_ID = MasterID;
            obj.TenantiD = Tenant;
            $.ajax({
                url: "ItemMasterRequest.aspx/SETIndustries",

                dataType: 'json',
                contentType: "application/json",
                type: 'POST',
                data: JSON.stringify(obj),
                async: false,
                success: function (response) {
                    if (response.d == "POMapped") {
                        showStickyToast(false, "Cannot save as this Item is mapped to PO. ", false);
                        return false;
                    }
                    if (response.d == "success") {
                        showStickyToast(true, "Industry Attributes Saved successfully", false);
                        GetItemMasterDetails(MaterialMasterID);
                    }
                }
            });
        }

        function checkZeroValidationForIndustry() {
            //debugger;
            var status = true;
            $('.IndustryfieldToGet').each(function () {
                debugger;
                var paramtype = $(this).attr('type');
                if (paramtype == "text")
                    var checkzero = $(this).val();
                if (checkzero == "0") {
                    status = false;
                    return status;
                }
            });
            return status;
        }

        function UpsertIndustry() {
            var obj = {};
            Tenant = $("#Tenant").val();
            obj.UserID = "<%=cp.UserID.ToString()%>";
            obj.IndustryID = $("#<%=hdnIndustry.ClientID %>").val();
            obj.MM_MST_Material_ID = MasterID;
            obj.TenantiD = Tenant;
            $.ajax({
                url: "ItemMasterRequest.aspx/SETIndustryID",

                dataType: 'json',
                contentType: "application/json",
                type: 'POST',
                data: JSON.stringify(obj),
                async: false,
                success: function (response) {
                    if (response.d == "POMapped") {
                        showStickyToast(false, "Cannot save as this Item is mapped to PO. ", false);
                        return false;
                    }
                    if (response.d == "success") {
                        //showStickyToast(true, "Industry Attributes Saved successfully", false);
                        UpsertIndustries();
                    }
                }
            });
        }

        //Sequence Number check in Material Preferences
        function SeqCheck() {
            // get all input elements
            var $elems = $('.seqcheck');
            // store the inputs value in array
            var values = [];
            // return this
            var isDuplicated = false;
            // loop through elements
            $elems.each(function () {
                //If value is empty then move to the next iteration.
                if (!this.value) return true;
                //If the stored array has this value, break from the each method
                if (values.indexOf(this.value) !== -1) {
                    isDuplicated = true;
                    return false;
                }
                // store the value
                values.push(this.value);
            });
            return isDuplicated;
        }
    </script>

</asp:Content>

