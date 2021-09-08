<%@ Page Title="Item Master List " Language="C#" MasterPageFile="~/mMaterialManagement/MaterialManagementMaster.master" AutoEventWireup="true" CodeBehind="MaterialMasterList.aspx.cs" Inherits="MRLWMSC21.mMaterialManagement.MaterialMasterList" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MMContent" runat="server">

    <asp:ScriptManager ID="mySManager" runat="server" EnablePartialRendering="true" SupportsPartialRendering="true" />

    <script type="text/javascript" src="Scripts/CommonScripts.js"></script>
    <link href="../mInventory/Scripts/bootstrap.min.css" rel="stylesheet" />
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>


    <style type="text/css">
        #Processing {
            background: white url('../Images/ui-anim_basic_16x16.gif') center center no-repeat;
        }

        .ui-widget {
            position: fixed;
            font-size: 11px !important;
        }


        .ui-state-error-text {
            margin-left: 10px;
        }

        .ui-widget-overlay {
            position: fixed;
        }

        .ui-autocomplete-loading {
            background: white url('../Images/ui-anim_basic_16x16.gif') right center no-repeat;
        }

        .ui-autocomplete {
            position: absolute;
            height: 200px;
            overflow-y: scroll;
        }

        .ui-state-hover {
            cursor: pointer;
        }

        .home a {
            position: absolute;
            opacity: 0;
        }

        .btnSearchSmall {
            font-size: 20px;
            color: #0e0e0e !important;
        }



        .custom-file-input {
            color: transparent;
        }

        .txt_Blue_Small::-webkit-file-upload-button {
            visibility: hidden;
        }

        .txt_Blue_Small::before {
            content: 'Choose Excel';
            color: #fff;
            display: inline-block;
            background: var(--sideNav-bg);
            border: 1px solid var(--sideNav-bg);
            border-radius: 0px;
            padding: 7px 4px;
            outline: none;
            white-space: nowrap;
            -webkit-user-select: none;
            cursor: pointer;
            /* text-shadow: 1px 1px #fff; */
            font-weight: 500;
            font-size: 8pt;
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

        select {
            width: 72 !important
        }

        #MainContent_MMContent_trvmaterialattachmentn0Nodes table td {
            display: inline-table !important;
            width: fit-content !important;
        }

        #MainContent_MMContent_trvmaterialattachment table td {
            display: inline-table !important;
            width: fit-content !important;
        }

        @media(max-width: 850px) {
            [type="file"] {
                width: 94% !important;
                border: 1px solid #999;
                padding: 5px;
                margin-bottom: 5px;
                height: 20px;
                position: relative;
                top: 2px;
                margin-left: 0px !important;
                left: -16px !important;
            }
        }

        #divItemPrintData {
            height: 460px !important;
        }

        .input-full-with input, input[type="text"]:disabled, select {
            width: 100% !important;
        }

        .aspNetDisabled.page-numbers {
            background: var(--sideNav-bg) !important;
            border-color: var(--sideNav-bg) !important;
            color: #fff !important;
        }
        .home a {opacity:unset;bottom:5px;
        }
    </style>

    <!-- Following Line to block the page and show 'Processing'    --->

    <script type="text/javascript">


        $(document).ready(function () {
            
            $("#<%= this.lnkMMListSearch.ClientID%>").click(function () {
                $.blockUI({
                    message: $('#displayBox'),
                    css:
                    {
                        border: 'none',
                        padding: '15px',
                        backgroundColor: '#fff',
                        '-webkit-border-radius': '10px',
                        '-moz-border-radius': '10px',
                        opacity: .5,
                        color: '#333',
                        '-ms-filter': 'progid:DXImageTransform.Microsoft.Alpha(Opacity=50)',
                        filter: 'progid:DXImageTransform.Microsoft.Alpha(Opacity=50)',
                        '-moz-opacity': '.70',
                        opacity: .70

                    }
                });
            });
            LoadSuppliers();
            LoadMType();
        });
    </script>

    <!-- Following Line to open print dialog box    --->

    <script type="text/javascript">

        $(document).ready(function () {
            $("#divItemPrintData").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 20,
                height: 630,
                width: 600,
                resizable: false,
                draggable: false,
                position: ["center top", 40],
                open: function () {

                    $(".ui-dialog").hide().fadeIn(500);

                    $('body').css({ 'overflow': 'hidden' });

                    //$('body').width($('body').width());

                    $(document).bind('scroll', function () {
                        window.scrollTo(0, 0);
                    });
                },
                close: function () {

                    $(".ui-dialog").fadeOut(500);

                    $(document).unbind('scroll');

                    $('body').css({ 'overflow': 'visible' });

                },

                title: "Print Labels",
                open: function (event, ui) {
                    $(this).parent().appendTo("#divViewAttachmentContainer");


                }
            });
       $("#txtmfgdate").datepicker({
                dateFormat: "dd-M-yy",
                maxDate: new Date(),
                onSelect: function (selected) {
                    $("#txtexpdate").datepicker("option", "minDate", selected, { dateFormat: "dd-M-yy" })
                }
            });

            $("#txtexpdate").datepicker({
                dateFormat: "dd-M-yy",
                //maxDate: new Date()
            });

            //Added by kashyap on 21/08/2017  to reslove the server issue 
            $('#txtmfgdate, #txtexpdate').keypress(function () {
                return false;
            });
        });

        function CheckDigit(event, textbox) {

            var x = event.which || event.keyCode;
            if (!((x == 190) || (x > 47 && x < 58)||(x >= 96 && x <= 106))) {
                textbox.value = "";
            } else {
                if (!(textbox.value.split('.').length <= 2 && textbox.value.split('.')[0] != '')) {
                    textbox.value = "";
                }


            }
            console.log(x);
            //var num = parseFloat(text.value).toFixed(2);
            //console.log(num);
            //if (num == 'NaN')
            //{
            //    text.value = "";
            //} else
            //{
            //    text.value = num.split('.')[0];
            //}



        }

        function blockDialog() {

            //block it to clean out the data
            $("#divItemPrintData").block({
                message: '<img src="<%=ResolveUrl("~") %>Images/async_master.gif" />',
                css: { border: '0px' },
                fadeIn: 0,
                fadeOut: 0,
                overlayCSS: { backgroundColor: '#ffffff', opacity: 1 }
            });
        }
        function closeDialog() {
            debugger;
            //Could cause an infinite loop because of "on close handling"
            $("#divItemPrintData").dialog('close');
           
            $('#PrintItems').modal('hide');
            $('#<%=txtBatchNo.ClientID%>').val("");
            $('#<%=txtSerialNo.ClientID%>').val("");
            $('#<%=txtStrRefNo.ClientID%>').val("");
            $('#<%=txtMRP.ClientID%>').val("");
            $('#<%=txtQuantity.ClientID%>').val("") ;
            $('#<%=ddlWarehousePrinter.ClientID%>').val(0);
            $('#<%=ddlLabelSize.ClientID%>').val(0);
            return false;
        }

        function openPrintDialog() {
            $("#divItemPrintData").dialog({
                autoOpen: false,
            });
        }
        function openDialog(title, linkID) {

            $("#divItemPrintData").dialog('open');

        }
        function openDialogAndBlock(title, prmMaterialData) {
            var MatData = new Array();
            MatData = prmMaterialData.toString().split("|");
            eval(document.getElementById("<%=txtMCode.ClientID%>")).value = MatData[0];

            eval(document.getElementById("<%=txtItemDesc.ClientID%>")).value = MatData[3];
            linkID = MatData[0];
            openDialog(title, linkID);
            //block it to clean out the data
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
        }
    </script>

    <script type="text/javascript">

        function txtOnKeyPress() {
            if ($("#<%=this.txtTenant.ClientID %>").val("")) {
                showStickyToast(false, 'Please select \'Tenant\'');
            }
        }


        function confirmMsg() {
            return confirm("Are you sure?");
        }

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                fnLoadMCode();
            }
        }

        //function focuslost1(TextBox) {
        //    if (TextBox.value == "")
        //        TextBox.value = "Search Supplier...";

        //    TextBox.style.color = "#A4A4A4";
        //}

        //function focuslost2(TextBox) {
        //    if (TextBox.value == "")
        //        TextBox.value = "Search Part Number ...";
        //    TextBox.style.color = "#A4A4A4";
        //}

        //function focuslost3(TextBox) {
        //    if (TextBox.value == "")
        //        TextBox.value = "Search OEM # ...";

        //    TextBox.style.color = "#A4A4A4";

        //} 

        //function focuslost4(TextBox) {
        //    if (TextBox.value == "")
        //        TextBox.value = "Search Material Type ...";

        //    TextBox.style.color = "#A4A4A4";

        //} 

        //function focuslostTenant(TextBox) {
        //    if (TextBox.value == "")
        //        TextBox.value = "Search Tenant...";

        //    TextBox.style.color = "#A4A4A4";

        //}

        //function ClearText(TextBox) {
        //    if (TextBox.value == "Search Supplier...")
        //        TextBox.value = "";

        //    TextBox.style.color = "#000000";
        //}
        //function ClearText1(TextBox) {
        //    if (TextBox.value == "Search Part Number ...")
        //        TextBox.value = "";

        //    TextBox.style.color = "#000000";
        //}
        //function ClearText2(TextBox) {
        //    if (TextBox.value == "Search OEM # ...")
        //        TextBox.value = "";

        //    TextBox.style.color = "#000000";
        //}
        //function ClearText3(TextBox) {
        //    if (TextBox.value == "Search Material Type ...")
        //        TextBox.value = "";

        //    TextBox.style.color = "#000000";
        //}

        //function ClearTextTenant(TextBox) {
        //    if (TextBox.value == "Search Tenant...")
        //        TextBox.value = "";

        //    TextBox.style.color = "#000000";
        //}

        function fnLoadMCode() {

            $(document).ready(function () {


                var textfieldname = $("#<%= this.txtMMListItemCode.ClientID %>");
                DropdownFunction(textfieldname);
                $("#<%= this.txtMMListItemCode.ClientID %>").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadMCodeDataFor3PLSupplier") %>',
                            data: "{ 'prefix': '" + request.term + "','TenantID':'" + document.getElementById('<%= this.hifTenant.ClientID %>').value + "','SupplierID':" + $('#<%=this.hifSupplier.ClientID%>').val() + "}",//<=cp.TenantID%>
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                <%--if (data.d == "" || data.d == "/") {
                                    if (document.getElementById('<%= this.hifTenant.ClientID %>').value == 0) {
                                        showStickyToast(false, 'Please select \'Tenant\'');
                                    } else {
                                        showStickyToast(false, 'No Material is available for this \'Tenant\'');
                                    }
                                }
                                else {--%>
                                response($.map(data.d, function (item) {
                                    return {
                                        label: item.split(',')[0],
                                        val: item.split(',')[1]
                                    }
                                }
                                ))
                                //}
                            },
                            error: function (response) {

                            },
                            failure: function (response) {

                            }
                        });
                    },
                    minLength: 0
                });

                var textfieldname = $("#<%= this.txtOEMSearch.ClientID %>");
                DropdownFunction(textfieldname);
                $("#<%= this.txtOEMSearch.ClientID %>").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadOEMPartNo") %>',
                            data: "{ 'prefix': '" + request.term + "' }",
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
                                alert(response.text);
                            },
                            failure: function (response) {
                                alert(response.text);
                            }
                        });
                    },
                    minLength: 0
                });

                var TextFieldName = $("#<%= this.txtTenant.ClientID %>");
                DropdownFunction(TextFieldName);
                $("#<%= this.txtTenant.ClientID %>").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadTenantDataFor3PL") %>',
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
                        debugger;
                        $("#<%=hifTenant.ClientID %>").val(i.item.val);
                         <%-- $("#<%= txtSupplier.ClientID %>").val('Search Supplier...');
                        $("#<%= txtMMListItemCode.ClientID %>").val('Search Part Number ...');
                        $("#<%= txtMType.ClientID %>").val('Search Material Type ...');
                        $("#<%=hifMTypeID.ClientID %>").val(0);--%>
                        LoadSuppliers();
                        LoadMType();
                    },
                    minLength: 0
                });

            });


        }
        function LoadSuppliers() {
            
            var textfieldnames = $('#<%=txtSupplier.ClientID%>');
            DropdownFunction(textfieldnames);
            $("#<%= this.txtSupplier.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadSupplierDataFor3PL") %>',
                        data: "{ 'prefix': '" + request.term + "','TenantID':'" + $('#<%=this.hifTenant.ClientID%>').val() + "','Type':'Supplier'}",//<=cp.TenantID%>
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
                    $("#<%=hifSupplier.ClientID %>").val(i.item.val);
                },
                minLength: 0,

            });
        }

        function LoadMType() {
            
            var textfieldnames = $('#<%=txtMType.ClientID%>');
            DropdownFunction(textfieldnames);
            $("#<%= this.txtMType.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadMTypeDataItem") %>',
                        data: "{ 'prefix': '" + request.term + "','TenantID':'" + $('#<%=this.hifTenant.ClientID%>').val() + "'}",
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
                    $("#<%=hifMTypeID.ClientID %>").val(i.item.val);
                },
                minLength: 0,
            });
        }
        LoadMType();

        LoadSuppliers();

        fnLoadMCode();

    </script>


    <script type="text/javascript">
        function checkFileExtension(elem) {
            var filePath = elem.value;

            if (filePath.indexOf('.') == -1)
                return false;

            var validExtensions = new Array();
            var ext = filePath.substring(filePath.lastIndexOf('.') + 1).toLowerCase();

            validExtensions[0] = 'xls';
            validExtensions[1] = 'xlsx';


            for (var i = 0; i < validExtensions.length; i++) {
                if (ext == validExtensions[i])
                    return true;
            }

            elem.value = "";
            alert('The file extension ' + ext.toUpperCase() + ' is not allowed!');
            return false;
        }
    </script>


    <style>
        .ui-dialog-titlebar-close {
            visibility: hidden;
        }

        .pager {
            margin-top: 10px;
            margin-bottom: 10px;
        }

        .page-numbers {
        }

        #MainContent_MMContent_dlPagerupper td {
            padding: 2px;
            display: none;
        }

        #MainContent_MMContent_dlPager td {
            padding: 2px;
        }

        /*.page-numbers.current, .aspNetDisabled {*/
        .page-numbers.current {
            vertical-align: middle;
            box-shadow: var(--z1);
            padding: 0px;
            display: inline-block !important;
            border: 2px solid var(--sideNav-bg) !important;
            background-color: var(--sideNav-bg) !important;
            width: 20px !important;
            height: 20px !important;
            line-height: 20px;
            color: #fff;
        }

        .page-numbers.next, .page-numbers.prev {
            border: 1px solid white;
        }

        .page-numbers.desc {
            border: none;
            margin-bottom: 10px;
        }

        .gvLightGrayNew_footerGrid {
            display: none;
        }


        .btntxtclr {
            /*color: var(--sideNav-bg) !important;*/
            color: cornflowerblue !important;
        }
    </style>

    <!-- This is for Attachments-->
    <script type="text/javascript">
        $(document).ready(function () {
            $("#divViewAttachment").dialog({
                //bgiframe:false,
                autoOpen: false,
                modal: true,
                minHeight: 20,
                height: '400',
                width: '350',
                resizable: false,
                draggable: false,
                position: ["center top", 40],

                open: function (event, ui) {
                    $("#divViewAttachment", ui.dialog || ui).hide();
                    $("#divViewAttachment").hide().fadeIn(500);
                    $('body').css({ 'overflow': 'hidden' });
                    $('body').width($('body').width());
                    $(document).bind('scroll', function () {
                        window.scrollTo(0, 0);
                    });
                },
                close: function () {
                    $("#divViewAttachment").fadeOut(500);
                    $(document).unbind('scroll');
                    $('body').css({ 'overflow': 'visible' });
                },
                //open: function (event, ui) { $(this).parent().appendTo("#divViewAttachmentContainer"); }
            });
            $("#Processing").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 20,
                height: '400',
                width: '350',
                resizable: false,
                draggable: false,
                dialogClass: 'no-close',
                position: ["center top", 40],
                open: function (event, ui) {
                    $(this).parent().appendTo("#divViewAttachmentContainer");
                    $("#Processing").hide().fadeIn(500);
                    $('body').css({ 'overflow': 'hidden' });
                    $('body').width($('body').width());
                    $(document).bind('scroll', function () {
                        window.scrollTo(0, 0);
                    });
                },
                close: function () {
                    $("#Processing").fadeOut(500);
                    $(document).unbind('scroll');
                    $('body').css({ 'overflow': 'visible' });
                },
            });
        });
        function closeAttachment() {
            //Could cause an infinite loop because of "on close handling"
            $("#divViewAttachment").dialog('close');
        }
        function OpenAttachment(title) {
            $("#divViewAttachment").dialog("option", "title", title);
            $("#divViewAttachment").dialog('open');
        }
        function showProcessing() {
            $("#Processing").dialog("option", "title", 'Processing...');
            $("#Processing").dialog('open');
        }
        function closeProcessing() {
            $("#Processing").dialog('close');
            $("#divViewAttachment").dialog("option", "title", 'View Attachments');
            $("#divViewAttachment").dialog('open');
        }
    </script>


    <script type="text/javascript">

        function unblockAycDialog() {
            $.unblockUI();
        }

        function showAycBlock() {

            $.blockUI({ message: '<h3> Just a moment...</h3>' });
        }

    </script>

    
        <script type="text/javascript">


        //var wcppGetPrintersDelay_ms = 0;
        var wcppGetPrintersTimeout_ms = 10000; //10 sec
        var wcppGetPrintersTimeoutStep_ms = 500; //0.5 sec
        // var jsWebClientPrint=(function(){var setA=function(){var e_id='id_'+new Date().getTime();if(window.chrome){$('body').append('<a id="'+e_id+'"></a>');$('#'+e_id).attr('href','webclientprintiv:'+arguments[0]);var a=$('a#'+e_id)[0];var evObj=document.createEvent('MouseEvents');evObj.initEvent('click',true,true);a.dispatchEvent(evObj)}else{$('body').append('<iframe name="'+e_id+'" id="'+e_id+'" width="1" height="1" style="visibility:hidden;position:absolute" />');$('#'+e_id).attr('src','webclientprintiv:'+arguments[0])}setTimeout(function(){$('#'+e_id).remove()},5000)};return{print:function(){setA('http://192.168.1.24/ITWMS/DemoPrintCommandsHandler.ashx?clientPrint'+(arguments.length==1?'&'+arguments[0]:''))},getPrinters:function(){setA('-getPrinters:http://192.168.1.24/ITWMS/WebClientPrintAPI.ashx?sid='+'3pufxe55b5dtxg453ywcy055');var delay_ms=(typeof wcppGetPrintersDelay_ms==='undefined')?0:wcppGetPrintersDelay_ms;if(delay_ms>0){setTimeout(function(){$.get('http://localhost/WebClientPrintAPI.ashx?getPrinters&sid='+'3pufxe55b5dtxg453ywcy055',function(data){if(data.length>0){wcpGetPrintersOnSuccess(data)}else{wcpGetPrintersOnFailure()}})},delay_ms)}else{var fncGetPrinters=setInterval(getClientPrinters,wcppGetPrintersTimeoutStep_ms);var wcpp_count=0;function getClientPrinters(){if(wcpp_count<=wcppGetPrintersTimeout_ms){$.get('http://192.168.1.24/ITWMS/WebClientPrintAPI.ashx?getPrinters&sid='+'3pufxe55b5dtxg453ywcy055',{'_':$.now()},function(data){if(data.length>0){clearInterval(fncGetPrinters);wcpGetPrintersOnSuccess(data)}});wcpp_count+=wcppGetPrintersTimeoutStep_ms}else{clearInterval(fncGetPrinters);wcpGetPrintersOnFailure()}}}},getPrintersInfo:function(){setA('-getPrintersInfo:http://192.168.1.24/ITWMS/WebClientPrintAPI.ashx?sid='+'3pufxe55b5dtxg453ywcy055');var delay_ms=(typeof wcppGetPrintersDelay_ms==='undefined')?0:wcppGetPrintersDelay_ms;if(delay_ms>0){setTimeout(function(){$.get('http://192.168.1.24/ITWMS/WebClientPrintAPI.ashx?getPrintersInfo&sid='+'3pufxe55b5dtxg453ywcy055',function(data){if(data.length>0){wcpGetPrintersOnSuccess(data)}else{wcpGetPrintersOnFailure()}})},delay_ms)}else{var fncGetPrintersInfo=setInterval(getClientPrintersInfo,wcppGetPrintersTimeoutStep_ms);var wcpp_count=0;function getClientPrintersInfo(){if(wcpp_count<=wcppGetPrintersTimeout_ms){$.get('http://192.168.1.24/ITWMS/WebClientPrintAPI.ashx?getPrintersInfo&sid='+'3pufxe55b5dtxg453ywcy055',{'_':$.now()},function(data){if(data.length>0){clearInterval(fncGetPrintersInfo);wcpGetPrintersOnSuccess(data)}});wcpp_count+=wcppGetPrintersTimeoutStep_ms}else{clearInterval(fncGetPrintersInfo);wcpGetPrintersOnFailure()}}}},getWcppVer:function(){setA('-getWcppVersion:http://192.168.1.24/ITWMS/WebClientPrintAPI.ashx?sid='+'3pufxe55b5dtxg453ywcy055');var delay_ms=(typeof wcppGetVerDelay_ms==='undefined')?0:wcppGetVerDelay_ms;if(delay_ms>0){setTimeout(function(){$.get('http://192.168.1.24/ITWMS/WebClientPrintAPI.ashx?getWcppVersion&sid='+'3pufxe55b5dtxg453ywcy055',function(data){if(data.length>0){wcpGetWcppVerOnSuccess(data)}else{wcpGetWcppVerOnFailure()}})},delay_ms)}else{var fncWCPP=setInterval(getClientVer,wcppGetVerTimeoutStep_ms);var wcpp_count=0;function getClientVer(){if(wcpp_count<=wcppGetVerTimeout_ms){$.get('http://192.168.1.24/ITWMS/WebClientPrintAPI.ashx?getWcppVersion&sid='+'3pufxe55b5dtxg453ywcy055',{'_':$.now()},function(data){if(data.length>0){clearInterval(fncWCPP);wcpGetWcppVerOnSuccess(data)}});wcpp_count+=wcppGetVerTimeoutStep_ms}else{clearInterval(fncWCPP);wcpGetWcppVerOnFailure()}}}},send:function(){setA.apply(this,arguments)}}})();
        function wcpGetPrintersOnSuccess() {
            debugger;
            // Display client installed printers
            if (arguments[0].length > 0) {
                var p = arguments[0].split("|");
                var options = '';
                for (var i = 0; i < p.length; i++) {
                    options += '<option>' + p[i] + '</option>';
                }
                $('#installedPrinterName').html(options);
                $('#installedPrinterName').focus();
                $('#loadPrinters').hide();
            } else {
                // alert("No printers are installed in your system.");
                showStickyToast(false, "Please install Web Client Print Software in your PC", false);
                return false;
            }
        }

        function wcpGetPrintersOnFailure() {
            // Do something if printers cannot be got from the client 
            //alert("No printers are installed in your system.");
            showStickyToast(false, "No printers are installed in your system", false);
            return false;
        }
    </script>
    <script type="text/javascript">

        function doClientPrint() {
            debugger;
            //collect printer settings and raw commands
            var printerSettings = $("#myForm :input").serialize();

            //store printer settings in the server cache...
            $.post('ItemMasterWebClientPrintDemo.ashx',
                printerSettings
            );

            // Launch WCPP at the client side for printing...
            var sessionId = $("#sid").val();
            jsWebClientPrint.print('sid=' + sessionId);

        }


        $(document).ready(function () {

            //jQuery-based Wizard
            $("#myForm").formToWizard();

            //change printer options based on user selection
            $("#pid").change(function () {
                debugger;
                var printerId = $("select#pid").val();

                displayInfo(printerId);
                hidePrinters();
                if (printerId == 2) {
                    $("#installedPrinter").show();
                    // $("#installedPrinterName").removeAttr("disabled");
                    javascript: jsWebClientPrint.getPrinters();


                }
                else if (printerId == 3) {
                    $("#installedPrinter").hide();
                    $("#netPrinter").show();
                }
                else if (printerId == 4) {
                    $("#installedPrinter").hide();
                    $("#parallelPrinter").show();
                }
                else if (printerId == 5) {
                    $("#installedPrinter").hide();
                    $("#serialPrinter").show();
                }
            });

            hidePrinters();
            displayInfo(0);


        });

        function displayInfo(i) {
            if (i == 0)
                $("#info").html('This will make the WCPP to send the commands to the printer installed in your machine as "Default Printer" without displaying any dialog!');
            else if (i == 1)
                $("#info").html('This will make the WCPP to display the Printer dialog so you can select which printer you want to use.');
            else if (i == 2)
                $("#info").html('Please specify the <b>Printer\'s Name</b> as it figures installed under your system.');
            else if (i == 3)
                $("#info").html('Please specify the Network Printer info.<br /><strong>On Linux &amp; Mac</strong> it\'s recommended you install the printer through <strong>CUPS</strong> and set the assigned printer name to the <strong>"Use an installed Printer"</strong> option on this demo.');
            else if (i == 4)
                $("#info").html('Please specify the Parallel Port which your printer is connected to.<br /><strong>On Linux &amp; Mac</strong> you must install the printer through <strong>CUPS</strong> and set the assigned printer name to the <strong>"Use an installed Printer"</strong> option on this demo.');
            else if (i == 5)
                $("#info").html('Please specify the Serial RS232 Port info which your printer does support.<br /><strong>On Linux &amp; Mac</strong> you must install the printer through <strong>CUPS</strong> and set the assigned printer name to the <strong>"Use an installed Printer"</strong> option on this demo.');
        }

        function hidePrinters() {
            $("#installedPrinter").hide();
            $("#netPrinter").hide();
            $("#parallelPrinter").hide();
            $("#serialPrinter").hide();
        }




        /* FORM to WIZARD */
        /* Created by jankoatwarpspeed.com */

        (function ($) {
            $.fn.formToWizard = function () {

                var element = this;

                var steps = $(element).find("fieldset");
                var count = steps.size();


                // 2
                $(element).before("<ul id='steps'></ul>");

                steps.each(function (i) {
                    $(this).wrap("<div id='step" + i + "'></div>");
                    $(this).append("<p id='step" + i + "commands'></p>");

                    // 2
                    var name = $(this).find("legend").html();
                    $("#steps").append("<li id='stepDesc" + i + "'>Step " + (i + 1) + "<span>" + name + "</span></li>");

                    if (i == 0) {
                        createNextButton(i);
                        selectStep(i);
                    }
                    else if (i == count - 1) {
                        $("#step" + i).hide();
                        createPrevButton(i);
                    }
                    else {
                        $("#step" + i).hide();
                        createPrevButton(i);
                        createNextButton(i);
                    }
                });

                function createPrevButton(i) {
                    var stepName = "step" + i;
                    $("#" + stepName + "commands").append("<a href='#' id='" + stepName + "Prev' class='prev btn btn-info'>< Back</a>");

                    $("#" + stepName + "Prev").bind("click", function (e) {
                        $("#" + stepName).hide();
                        $("#step" + (i - 1)).show();

                        selectStep(i - 1);
                    });
                }

                function createNextButton(i) {
                    var stepName = "step" + i;
                    $("#" + stepName + "commands").append("<a href='#' id='" + stepName + "Next' class='next btn btn-info'>Next ></a>");

                    $("#" + stepName + "Next").bind("click", function (e) {
                        $("#" + stepName).hide();
                        $("#step" + (i + 1)).show();

                        selectStep(i + 1);
                    });
                }

                function selectStep(i) {
                    $("#steps li").removeClass("current");
                    $("#stepDesc" + i).addClass("current");
                }

            }
        })(jQuery);


        function setItemData(mcode,mcodealt1,oem,mdesc)
        {
            $("#txtMCode").val(mcode);
            $("#txtItemDesc").val(mdesc);
        }
        function printItems()
        {
            debugger;
            if ($("#txtMCode").val() == "") { showStickyToast(false, "Please enter Part Number", false); return false; }
            if ($("#txtItemDesc").val() == "") { showStickyToast(false, "Please enter Item Desc.", false); return false; }
           
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
              if ($("#ddlLabelSize").val() == "0") { showStickyToast(false, "Please select Label", false); return false; }
         
              if ($("#txtQuantity").val() == "") { showStickyToast(false, "Please enter Quantity", false); return false; }
        
            if ($("#txtQuantity").val() <= 0) {
                 showStickyToast(false, "Quantity should be greater than zero", false);
                    return false;
            }


            $.ajax({
                url: 'MaterialMasterList.aspx/GetPrint',
                data: "{ 'MCode': '" + $("#txtMCode").val() + "','MDesc': '" + $("#txtItemDesc").val() + "','SerialNo':'" + $("#txtSerialNo").val() + "','BatchNo':'" + $("#txtBatchNo").val() + "','MfgDate':'" + $("#txtmfgdate").val() + "','ExpDate':'" + $("#txtexpdate").val() + "','ProjectRefNo':'" + $("#txtStrRefNo").val() + "','LabelID':'" + $("#ddlLabelSize").val() + "','PrintQty':'" + $("#txtQuantity").val() + "','MRP':'" + $("#txtMRP").val() + "','HUSize':'"+$("#txtHUSize").val()+"','HUNo':'"+$("#txtHUNo").val()+"'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    debugger;
                    var dt = data.d;
                    if (dt == "") {
                        showStickyToast(false, "Error occured", false);
                        return false;
                    }
                    else {
                        $("#printerCommands").val("");
                        $("#printerCommands").val(dt);
                        javascript: doClientPrint();
                        showStickyToast(true, "Successfully Printed", false);
                        setTimeout(function () {
                            location.reload();
                         }, 1000);
                    }
                },
                error: function (response) {

                },
                failure: function (response) {

                }
            });

        }
        function clearData()
        {
            $("#txtMCode").val();
            $("#txtItemDesc").val();
            $("#txtQuantity").val();
            $("#ddlLabelSize").val();
            $('#pid').val();
            $("#netPrinterHost").val();

            $("#netPrinterPort").val();
               
            
        }
    </script>
    


    <div class="loaderforCurrentStock" style="display: none;">
        <div style="width: 100%; height: 100vh; z-index: 999; position: fixed; top: 0; left: 0; right: 0; bottom: 0; align-items: center; display: flex; justify-content: center; background: rgba(255, 255, 255, 0.24); background: hsla(0, 0%, 100%, 0.72);">

            <div style="align-self: center;">
                <div class="spinner">
                    <div class="bounce1"></div>
                    <div class="bounce2"></div>
                    <div class="bounce3"></div>
                </div>

            </div>

        </div>

    </div>

    <div id="Processing"></div>

    <div id="divViewAttachmentContainer">
        <div id="divViewAttachment" style="display: block;">
            <asp:UpdatePanel ID="upviewattachment" runat="server" UpdateMode="Always" ChildrenAsTriggers="true">
                <Triggers>
                    <asp:PostBackTrigger ControlID="lnkattchemntclose" />
                </Triggers>
                <ContentTemplate>
                    <div class="ui-dailog-body" style="height: 275px; padding: 10px;">
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:TreeView ID="trvmaterialattachment" Target="_blank" runat="server">
                                        <Nodes>
                                            <asp:TreeNode Expanded="false"></asp:TreeNode>
                                        </Nodes>
                                    </asp:TreeView>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="ui-dailog-footer">
                        <div style="padding: 15px 13px 15px 5px;">

                            <asp:LinkButton runat="server" ID="lnkattchemntclose" OnClick="lnkattchemntclose_Click" OnClientClick="closeAttachment(); return false;" CssClass="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Close")%> <%=MRLWMSC21Common.CommonLogic.btnfaClear %></asp:LinkButton>

                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <!-- This is for Attachments End-->

     <!-- Print Dialog Data  -->
    <div id="divItemPrintDataContainer">
        <div id="divItemPrintData">

            <div class="alin-gn-input-fields">
                <br />

            </div>


            <div class="ui-dailog-footer">
                <div style="padding: 15px 13px 15px 5px;">

                  
                </div>

            </div>


        </div>
    </div>
                <asp:UpdatePanel ID="upnlUpdateReason" runat="server" Visible="false">
                <ContentTemplate>
                    <asp:PlaceHolder ID="phrUpdateReason" runat="server" Visible="false">



                    </asp:PlaceHolder>
                </ContentTemplate>
            </asp:UpdatePanel>


    <%--  <asp:UpdateProgress ID="uprgSearchOutbound" runat="server" AssociatedUpdatePanelID="upnlSearchOutbound">
            <ProgressTemplate>
                <div style="width:100%; height:100%; z-index:999; position:fixed; top:0; left:0; right:0; bottom:0; align-items:center; display:flex; justify-content:center; background: #e0ddd8ba;">
                
                <div style="align-self:center;" >
                        <div class="spinner">
                    <div class="bounce1"></div>
                    <div class="bounce2"></div>
                    <div class="bounce3"></div>
                </div>

                </div>
                                  
                </div>
                                
                                
            </ProgressTemplate>
            </asp:UpdateProgress>--%>
    <%--<asp:UpdatePanel ID="upnlSearchOutbound" runat="server" RenderMode="Inline" ViewStateMode="Enabled" ClientIDMode="Inherit" UpdateMode="Conditional">
            <ContentTemplate>--%>


    <!-- start Material List Grid -->
    <div class="pagewidth">
        <table cellspacing="3" cellpadding="3" border="0" align="center">


            <tr>
                <td>

                    <div id="divAdvanceSearch">

                        <asp:Panel ID="pnlStrRefShipmentPendingSort" runat="server" CssClass="SearchPanel" HorizontalAlign="right" DefaultButton="lnkMMListSearch">

                            <div>

                                <div class="row">


                                    <div class="col m2 s3">
                                        <div class="flex">
                                            <asp:TextBox ID="txtTenant" SkinID="txt_Hidden_Req" runat="server" required="" />
                                            <label><%= GetGlobalResourceObject("Resource", "SearchTenant")%></label>
                                            <asp:HiddenField runat="server" ID="hifTenant" Value="0" />
                                        </div>
                                    </div>

                                    <div class="col m2 s3" style="display: none !important;">
                                        <div class="flex">
                                            <asp:DropDownList runat="server" ID="ddlMMAdminApproved" CssClass="txt_slim_small" Width="170"></asp:DropDownList>
                                        </div>

                                    </div>
                                    <div class="col m2 s3">
                                        <div class="flex">
                                            <asp:TextBox ID="txtSupplier" runat="server" SkinID="txt_Hidden_Req" required="" />
                                            <asp:HiddenField runat="server" ID="hifSupplier" Value="0" />
                                            <label><%= GetGlobalResourceObject("Resource", "SearchSupplier")%></label>
                                        </div>
                                    </div>
                                    <div class="col m2 s3">
                                        <div class="flex">
                                            <asp:TextBox ID="txtMMListItemCode" SkinID="txt_Hidden_Req" runat="server" required="" />
                                            <label><%= GetGlobalResourceObject("Resource", "SearchPartNumber")%></label>
                                        </div>
                                    </div>
                                    <div class="col m2">
                                        <div class="flex">
                                            <%-- <asp:DropDownList ID="ddlMTypeID" runat="server" CssClass="txt_slim_small" required=""/>
                                        <label>Material Type</label>--%>
                                            <asp:TextBox ID="txtMType" SkinID="txt_Hidden_Req" runat="server" required="" />
                                            <label>Search Material Type</label>
                                            <asp:HiddenField runat="server" ID="hifMTypeID" Value="0" />
                                        </div>
                                    </div>
                                     <div class="col m3">
                                        <div class="flex">
                                            <%-- <asp:DropDownList ID="ddlMTypeID" runat="server" CssClass="txt_slim_small" required=""/>
                                        <label>Material Type</label>--%>
                                            <asp:TextBox ID="txtDescription" SkinID="txt_Hidden_Req" runat="server" required="" />
                                            <label>Search Description</label>
                                            <asp:HiddenField runat="server" ID="hifDes" Value="0" />
                                        </div>
                                    </div>
                                    <div class="col m3 s3" style="display: none;">
                                        <div class="flex">
                                            <asp:TextBox ID="txtOEMSearch" SkinID="txt_Hidden_Req" runat="server" Visible="false" Text="" onfocus="ClearText2(this)" onblur="javascript:focuslost3(this)" />&nbsp;
                                        </div>
                                    </div>
                                    <div class="col m1 p0" style="padding-right: 0px !important;">
                                        <div class="flex">
                                            <asp:LinkButton ID="lnkMMListSearch" runat="server" CssClass="btn btn-primary" OnClick="lnkMMListSearch_Click"><%= GetGlobalResourceObject("Resource", "Search")%><i class="material-icons">search</i></asp:LinkButton>
                                        </div>

                                    </div>

                                </div>

                            </div>


                        </asp:Panel>

                    </div>

                </td>
            </tr>

            <tr>


                <td style="display: flex; justify-content: space-between;">

                    <table style="width: 30%;">
                        <tr>
                            <td style="width: 80%">
                                <table>
                                    <tr class="nobotm">
                                        <td align="left">

                                            <asp:UpdatePanel runat="server" ID="upnlRecordCout" ChildrenAsTriggers="true" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <span style="font-family: Calibri; font-weight: bold; font-size: 11pt;">Total Materials&nbsp;&nbsp;<asp:Literal runat="server" ID="ltSITRecordCount" /></span>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td align="left">
                                            <div class="flex__">
                                                <div class="flex" style="vertical-align: middle !important;">
                                                    <asp:Literal runat="server" ID="lblstringperpage" Text="Display " />
                                                </div>
                                                &nbsp;&nbsp;
                                        <asp:DropDownList runat="server" Width="70px" AutoPostBack="true" OnSelectedIndexChanged="drppagesize_SelectedIndexChanged" ID="drppagesize">
                                            <asp:ListItem Text="25" Value="25" Selected="True" />
                                            <%--<asp:ListItem Text="30" Value="30" />--%>
                                            <asp:ListItem Text="50" Value="50" />
                                            <%--<asp:ListItem Text="80" Value="80" />--%>
                                            <asp:ListItem Text="75" Value="75" />
                                            <asp:ListItem Text="100" Value="100" />
                                        </asp:DropDownList>
                                            </div>
                                        </td>
                                    
                                    </tr>
                                </table>
                            </td>
                            <td align="right" width="60%">
                                <table>
                                    <tr>
                                        <td align="right">
                                            <asp:DataList CellPadding="10" RepeatDirection="Horizontal" runat="server" ID="dlPagerupper" OnItemCommand="dlPagerupper_ItemCommand" Visible="false">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" class="page-numbers" Font-Underline="false" ID="lnkPageNo" Enabled='<%#Eval("Enabled") %>' Text='<%#Eval("Text") %>' CommandArgument='<%#Eval("Value") %>' CommandName="PageNo" />
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </td>

                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>

                    <table border="0" style="width: unset;">
                        <tr>
                            <%--<td>
                            <asp:DropDownList ID="DropDownList1" runat="server"></asp:DropDownList>
                        </td>--%>
                            <td>
                                <div class="flex">
                                    <asp:FileUpload runat="server" ID="flupldImportExcel" CssClass="custom-file-input pull-right" onchange="return checkFileExtension(this);" Style="margin-left: 2% !important;" />
                                </div>
                            </td>
                            <td>
                                <div style="top: 0px !important;">
                                    <asp:LinkButton runat="server" ID="lnkflupldImportExcel" OnClick="lnkflupldImportExcel_Click" CssClass="btn btn-primary">  <%= GetGlobalResourceObject("Resource", "ImportExel")%><%=MRLWMSC21Common.CommonLogic.btnfaExcel %></asp:LinkButton><br />
                                    <%--(<a href="SampleTemplateForMaterial/MMT_ItemMasterFromExcel.xlsx"><%=FalconCommon.CommonLogic.btnfaExcel %> Sample Template</a>)--%>
                                </div>
                            </td>
                            &nbsp;
                                     <td>
                                         <div>

                                             <asp:LinkButton ID="imgbtngvMMList" runat="server" CssClass="btn btn-primary" OnClick="imgbtngvMMList_Click"><%= GetGlobalResourceObject("Resource", "ExportExel")%> <%=MRLWMSC21Common.CommonLogic.btnfaExcel %></asp:LinkButton>

                                         </div>
                                     </td>
                            <td>
                                <%--href="SampleTemplateForMaterial/MMT_ItemMasterFromExcel.xlsx"--%>
                                <div class="flex">
                                    <asp:LinkButton ID="btnSampleTemplate"  OnClick="btnSampleTemplate_Click" runat="server" CssClass="btnGetTemplate btn btn-sm btn-primary" > <%= GetGlobalResourceObject("Resource", "SampleTemplate")%> <i class="material-icons vl">file_download</i></asp:LinkButton>
                                </div>
                            </td>
                            <td>
                                <div>
                                    <asp:LinkButton PostBackUrl="~/mMaterialManagement/ItemMasterRequest.aspx" CssClass="btn btn-primary" ID="lnkAddMaterial" runat="server">  <%= GetGlobalResourceObject("Resource", "AddMaterial")%> <%=MRLWMSC21Common.CommonLogic.btnfaNew %></asp:LinkButton>
                                </div>
                            </td>
                        </tr>

                    </table>

                </td>
            </tr>


            <tr>
                <td></td>

            </tr>

            <tr>
                <td>

                    <asp:UpdatePanel runat="server" ID="upMMTList" ChildrenAsTriggers="true" UpdateMode="Always">
                        <ContentTemplate>

                            <asp:Panel runat="server" ID="pnlMatMasList">


                                <div id="divMaterialMasterList">

                                    <asp:Label runat="server" ID="lblStatus" />

                                    <asp:Label ID="lblGroupLabelStatus" runat="server" CssClass="ErrorMsg" />

                                    <asp:GridView Width="100%" ID="gvMMList" runat="server" ShowHeader="true" ShowHeaderWhenEmpty="true" Font-Underline="false" PagerSettings-Position="TopAndBottom" AllowPaging="true" PageSize="100" AllowSorting="True" SkinID="gvLightGrayNew" HorizontalAlign="Left" OnSorting="gvMMList_Sorting" OnPageIndexChanging="gvMMList_PageIndexChanging" OnRowDataBound="gvMMList_RowDataBound" OnRowCommand="gvMMList_RowCommand">

                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="170" HeaderText="Material" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <%--<asp:LinkButton runat="server" Font-Bold="false" Font-Underline="false" ID="viewattachment" CommandName="viewpics" CommandArgument='<%# String.Format("{0},{1},{2}",DataBinder.Eval(Container.DataItem, "MaterialMasterID"),DataBinder.Eval(Container.DataItem,"MCode"),DataBinder.Eval(Container.DataItem, "TenantID").ToString() )%>' OnClientClick="showProcessing();">
                                                    <asp:Literal runat="server" ID="ltMCode1" Text='<%# String.Format("{0}",DataBinder.Eval(Container.DataItem, "MCode").ToString()) %>' />
                                                </asp:LinkButton><asp:Literal runat="server" ID="ltMMItemID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialMasterID") %>' />
                                                 <div id="msgDiv" runat="server" >
                                              
                                                
                                                 
                      
                                                </div>--%>
                                                    <asp:Label runat="server" ID="ltItemMCode" Text='<%# DataBinder.Eval(Container.DataItem, "MCode") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-Width="150" HeaderText="<%$Resources:Resource,OEMPartNumber%>" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="true" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="ltOEMPartNo" Text='<%# DataBinder.Eval(Container.DataItem, "OEMPartNo") %>' />

                                                    <asp:Literal runat="server" ID="ltMCode" Text='<%# DisplayMCodes(DataBinder.Eval(Container.DataItem, "MCode").ToString(),DataBinder.Eval(Container.DataItem, "MCodeAlternative1").ToString(),DataBinder.Eval(Container.DataItem, "MCodeAlternative2").ToString()) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-Width="320" HeaderText="<%$Resources:Resource,Description%>" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="ltItemDesc" Text='<%# DataBinder.Eval(Container.DataItem, "MDescription") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField ItemStyle-Width="150" HeaderText="<%$Resources:Resource,MaterialType%>" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="ltMType" ToolTip='<%#Bind("MType") %>' Text='<%# DataBinder.Eval(Container.DataItem, "Type") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-Width="150" HeaderText="<%$Resources:Resource,MaterialGroup%>" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="ltMgroup" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialGroup") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-Width="250" HeaderText="<%$Resources:Resource,Tenant%>" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="ltTenantName" ToolTip='<%#Bind("TenantName") %>' Text='<%# DataBinder.Eval(Container.DataItem, "TenantCode") %>' />
                                                    <asp:Literal runat="server" ID="ltTenantID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "TenantID") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-Width="200" HeaderText="<%$Resources:Resource,Supplier%>" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="ltSupplier" ToolTip='<%# Bind("SupplierName") %>' Text='<%# DataBinder.Eval(Container.DataItem, "SupplierCode") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="<%$Resources:Resource,Print%>" ItemStyle-CssClass="home" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="ltPrintDialogLink" runat="server" Visible="false" Text='<%# GetPrintDialog(DataBinder.Eval(Container.DataItem, "MCode").ToString(),DataBinder.Eval(Container.DataItem, "MCodeAlternative1").ToString(),DataBinder.Eval(Container.DataItem, "OEMPartNo").ToString(), DataBinder.Eval(Container.DataItem, "MDescription").ToString())%>' />
                                                    <a data-target="#PrintItems" data-toggle="modal" onclick="setItemData('<%# DataBinder.Eval(Container.DataItem, "MCode").ToString()%>','<%# DataBinder.Eval(Container.DataItem, "MCodeAlternative1").ToString()%>','<%# DataBinder.Eval(Container.DataItem, "OEMPartNo").ToString()%>', '<%# DataBinder.Eval(Container.DataItem, "MDescription").ToString()%>')"><i class="material-icons">print</i></a>
                                                </ItemTemplate>
                                            </asp:TemplateField>

<%--                                            <asp:TemplateField ItemStyle-Width="60" HeaderText="<%$Resources:Resource,Print%>" ItemStyle-CssClass="home" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="ltPrintDialogLink" runat="server" Text='<%# GetPrintDialog(DataBinder.Eval(Container.DataItem, "MCode").ToString(),DataBinder.Eval(Container.DataItem, "MCodeAlternative1").ToString(),DataBinder.Eval(Container.DataItem, "OEMPartNo").ToString(), DataBinder.Eval(Container.DataItem, "MDescription").ToString())%>' />
                                                    <nobr style=""><svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24">
                                                    <path d="M19 8H5c-1.66 0-3 1.34-3 3v6h4v4h12v-4h4v-6c0-1.66-1.34-3-3-3zm-3 11H8v-5h8v5zm3-7c-.55 0-1-.45-1-1s.45-1 1-1 1 .45 1 1-.45 1-1 1zm-1-9H6v4h12V3z"/>
                                                    <path d="M0 0h24v24H0z" fill="none"/>
                                                </svg>
                                                  
                                                </nobr>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>

                                            <asp:TemplateField ItemStyle-Width="55" ItemStyle-HorizontalAlign="Center" HeaderText="<%$Resources:Resource,Edit%>">
                                                <ItemTemplate>
                                                    <asp:Literal ID="lthidRequestedBy" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RequestedBy") %>' />
                                                    <asp:LinkButton Font-Underline="false" ID="lnkEditItem" runat="server" Visible="false" Text="Edit" PostBackUrl='<%# String.Format("ItemMasterRequest.aspx?mid={0}",DataBinder.Eval(Container.DataItem, "MaterialMasterID").ToString()) %>' />

                                                    <asp:HyperLink ID="HyperLink11" Text="<nobr> <i class='material-icons ss'>mode_edit</i><em class='sugg-tooltis'>Edit</em></nobr>" NavigateUrl='<%#  String.Format("ItemMasterRequest.aspx?mid={0}",DataBinder.Eval(Container.DataItem, "MaterialMasterID").ToString())  %>' Font-Underline="false" runat="server"></asp:HyperLink>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="55" ItemStyle-HorizontalAlign="Center" HeaderText="<%$Resources:Resource,Copy%>">
                                                <ItemTemplate>

                                                    <asp:LinkButton Font-Underline="false" ID="lnkCopyItem" runat="server" Visible="false" Text="Copy" PostBackUrl='<%# String.Format("ItemMasterRequest.aspx?mid={0}&edittype=copy",DataBinder.Eval(Container.DataItem, "MaterialMasterID").ToString()) %>' />

                                                    <asp:HyperLink ID="HyperLink1" Text="<nobr> <i class='material-icons ss'>content_copy</i><em class='sugg-tooltis'>Copy</em></nobr>" NavigateUrl='<%#  String.Format("ItemMasterRequest.aspx?mid={0}&edittype=copy",DataBinder.Eval(Container.DataItem, "MaterialMasterID").ToString())  %>' Font-Underline="false" runat="server"></asp:HyperLink>

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>

                                            <div style="text-align: center; font-size: 13px !important;">No Data Found</div>

                                        </EmptyDataTemplate>
                                        <PagerSettings FirstPageText="&amp;lt;&amp;lt;First Page" LastPageText="Last Page&amp;gt;&amp;gt;" Mode="NumericFirstLast" PageButtonCount="15" />

                                    </asp:GridView>

                                </div>

                            </asp:Panel>

                        </ContentTemplate>
                    </asp:UpdatePanel>


                </td>
            </tr>

            <tr>
                <td align="right">
                    <asp:DataList CellPadding="10" RepeatDirection="Horizontal" runat="server" ID="dlPager" OnItemCommand="dlPagerupper_ItemCommand">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" CssClass="page-numbers" Font-Underline="false" ID="lnkPageNo" Enabled='<%#Eval("Enabled") %>' Text='<%#Eval("Text") %>' CommandArgument='<%#Eval("Value") %>' CommandName="PageNo" />
                        </ItemTemplate>
                    </asp:DataList>
                </td>
            </tr>

        </table>
    </div>
    <br />
    <br />
    <br />

    <!-- End Material List Grid -->

    <asp:UpdatePanel ID="upnlJsRunner" UpdateMode="Always" runat="server">
        <ContentTemplate>
            <asp:PlaceHolder ID="phrJsRunner" runat="server"></asp:PlaceHolder>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:HiddenField runat="server" ID="hfpageId" />
    <%--</ContentTemplate>
        </asp:UpdatePanel>--%>


            <!-- ========================= Modal Popup For Bulk Print ========================================== -->
    <div class="modal inmodal" id="PrintItems" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog" style="width: 50% !important;">
            <div class="modal-content animated fadeIn">

                <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title">Print New Labels</h4>
                </div>

                <div class="modal-body">
                      <div class="row">
                                <div colspan="2">
                                    <asp:Label ID="lblPrintStatus" runat="server" CssClass="errorMsg" />
                                </div>
                            </div>
                             <!-- Globalization Tag is added for multilingual  -->
                            <div class="row">

                                <div class="col m6 s6">
                                    <div class="flex">
                                        <asp:TextBox ID="txtMCode" runat="server" required="" ClientIDMode="Static"></asp:TextBox> 
                                        <span class="errorMsg"></span>
                                        <label> <%= GetGlobalResourceObject("Resource", "PartNumber")%></label>
                                    </div>
                                </div>
                                <div class="col m6 s6">
                                    <div class="flex">
                                        <asp:TextBox ID="txtItemDesc" runat="server" required="" MaxLength="30" ClientIDMode="Static" />
                                        <span class="errorMsg"></span>
                                      <%--  <label>Item Description</label>--%>
                                          <label><%= GetGlobalResourceObject("Resource", "ItemDescription")%> </label>
                                    </div>
                                </div>
                            </div>
                          
                            <div class="row">
                                <div class="col m4 s4">
                                    <div class="flex">
                                        <input type="text" id="txtmfgdate" onpaste="return false;" required="">
                                        <label><%= GetGlobalResourceObject("Resource", "MfgDate")%></label>
                                    </div>
                                </div>
                                <div class="col m4 s4">
                                    <div class="flex">
                                        <input type="text" id="txtexpdate" required="" onpaste="return false;">
                                        <label><%= GetGlobalResourceObject("Resource", "ExpDate")%> </label>
                                    </div>
                                </div>
                                   <div class="col m4 s4">
                                        <div class="flex">
                                            <asp:TextBox ID="txtBatchNo" runat="server" required="" MaxLength="20" ClientIDMode="Static"/>
                                           <%-- <label>Batch No.</label>--%>
                                             <label> <%= GetGlobalResourceObject("Resource", "BatchNo")%></label>
                                        </div>
                                    </div>
                             
                            <div class="row">
                                 
                                    <div class="col m4 s4">
                                        <div class="flex">
                                            <asp:TextBox ID="txtSerialNo" runat="server" required="" MaxLength="20" ClientIDMode="Static"/>
                                           <%-- <label>Serial No.</label>--%>
                                             <label> <%= GetGlobalResourceObject("Resource", "SerialNo")%> </label>
                                        </div>
                                    </div>
                                    <div class="col m4 s4">
                                        <div class="flex">
                                            <asp:TextBox ID="txtStrRefNo" runat="server" required="" MaxLength="20" ClientIDMode="Static"/>
                                            <label>Project Ref. No.</label>
                                             <%--<label><%= GetGlobalResourceObject("Resource", "StoreRefNo")%> </label>--%>
                                        </div>
                                    </div>

                                <div class="col m4 s4">
                                    <div class="flex">
                                        <asp:TextBox ID="txtMRP" runat="server" required="" onkeyup="CheckDigit(event,this);" ClientIDMode="Static"/>
                                       <%-- <label>MRP</label>--%>
                                         <label> <%= GetGlobalResourceObject("Resource", "MRP")%></label>
                                    </div>
                                </div>

                            </div>
                            

                            <div class="row" id="myForm">
                                <div class="col m4 s4">
                                    <div class="flex">
                                        <input type="hidden" id="sid" name="sid" value="itemMaster" />
                                        <select id="pid" name="pid" class="form-control">
                                            <option selected="selected" value="0">Use Default Printer</option>
                                            <option value="2">Use an installed Printer</option>
                                            <option value="3">Use an IP/Ethernet Printer</option>
                                        </select>
                                        <label><%= GetGlobalResourceObject("Resource", "Printer")%></label>
                                        <span class="errorMsg"></span>
                                    </div>
                                </div>
                                <div class="col m4 s4" id="installedPrinter">
                                    <div class="flex">
                                        <select name="installedPrinterName" id="installedPrinterName" class="form-control"></select>
                                        <label for="installedPrinterName">Select an installed Printer:</label>
                                        <span class="errorMsg"></span>
                                    </div>
                                </div>
                                <div class="col m8 s6" id="netPrinter">
                                    <div class="col m6 p0">
                                        <div class="flex">
                                            <input type="text" name="netPrinterHost" id="netPrinterHost" class="form-control" required="" />
                                            <label for="netPrinterHost">Printer's IP Address:</label>
                                            <span class="errorMsg"></span>
                                        </div>
                                    </div>
                                    <div class="col m6">
                                        <div class="flex">
                                            <input type="text" name="netPrinterPort" id="netPrinterPort" class="form-control" required="" />
                                            <label for="netPrinterPort">Printer's Port:</label>
                                            <span class="errorMsg"></span>
                                        </div>
                                    </div>
                                </div>
                                <div class="col m12" hidden>
                                    <div class="flex">
                                        <textarea id="printerCommands" name="printerCommands" rows="10" cols="80" class="form-control" style="min-width: 100%"></textarea>
                                    </div>
                                </div>
                             
                            </div>

                            <div class="row">    
                                <div class="col m4 s4">
                                    <div class="flex">
                                  
                                   <asp:DropDownList ID="ddlLabelSize" style="padding-right:20px !important" runat="server" CssClass="ddl_slim" ClientIDMode="Static"></asp:DropDownList>
                                    <span class="errorMsg"></span></div>
                                </div>

                                <div class="col m4">
                                    <div class=" flex">
                                        <input type="text" id="txtHUSize" onkeypress="return checkNum(event)" required=""/>
                                        <label>HU Size</label>
                                    </div>
                                </div>
                                 <div class="col m4">
                                    <div class=" flex">
                                        <input type="text" id="txtHUNo" onkeypress="return checkNum(event)" required=""/>
                                        <label>HU No</label>
                                    </div>
                                </div>
                                
                                <div class="col m4 s4">
                                    <div class="flex">
                                        <asp:TextBox ID="txtQuantity" runat="server" onKeyPress="return checkNum(event)" required="" ClientIDMode="Static"/>
                                        <span class="errorMsg">*</span>
                                        <label>Print Quantity</label>
                                          <%--<label> <%= GetGlobalResourceObject("Resource", "PrintQuantity")%> </label>--%>
                                    </div>
                                </div>

                                <div class="col m4" hidden>
                                    <div class=" flex">
                                        <asp:DropDownList ID="ddlWarehousePrinter" runat="server" CssClass="ddl_slim" />
                                        <span class="errorMsg"></span>
                                    </div>
                                </div>

                            </div>

                            <div class="row" hidden>
                                
                                <div class="col m8">
                                    <br />
                                    <div class="FormLabels" colspan="2">
                                        <div class="checkbox"><asp:CheckBox ID="chkNeedBoxLabel" runat="server" Checked="false" /><label>Need 1 additional label for the packing</label></div>
                                    </div>
                                </div>
                            </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton CssClass="btn btn-primary" ID="btnCancel" OnClick="btnCancel_Click" OnClientClick="return closeDialog()" CausesValidation="false" runat="server">
                                                     <%= GetGlobalResourceObject("Resource", "Cancel")%> <%= MRLWMSC21Common.CommonLogic.btnfaClear %>
                    </asp:LinkButton>
                    &nbsp;&nbsp;
                    <button type="button" class="btn btn-primary" onclick="printItems();"><%= GetGlobalResourceObject("Resource", "Print")%> <%= MRLWMSC21Common.CommonLogic.btnfaPrint %></button>

                </div>
            </div>
        </div>
    </div>
        </div>
    <%=Neodynamic.SDK.Web.WebClientPrint.CreateScript(HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + HttpContext.Current.Request.ApplicationPath + "/mMaterialManagement/ItemMasterWebClientPrint.ashx", HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + HttpContext.Current.Request.ApplicationPath + "/mMaterialManagement/ItemMasterWebClientPrintDemo.ashx", "itemMaster")%>
       <%-- <%=Neodynamic.SDK.Web.WebClientPrint.CreateScript(HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + "/FalconWMS_SL/mMaterialManagement/ItemMasterWebClientPrint.ashx", HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host +  "/FalconWMS_SL/mMaterialManagement/ItemMasterWebClientPrintDemo.ashx", "itemMaster")%>--%>
</asp:Content>
