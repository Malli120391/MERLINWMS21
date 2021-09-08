<%@ Page Title=" Inbound Details :." Language="C#" MasterPageFile="~/mInbound/InboundMaster.master" AutoEventWireup="true" CodeBehind="InboundDetails.aspx.cs" Inherits="MRLWMSC21.mInbound.InboundDetails" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="IBContent" runat="server">

    <asp:ScriptManager ID="smInboundDetails" runat="server" EnablePartialRendering="true" SupportsPartialRendering="true"></asp:ScriptManager>
    <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />

    <script type="text/javascript" src="Scripts/CommonScripts.js"></script>
    <script type="text/javascript" src="../Scripts/timeentry/jquery.timeentry.js"></script>

    <link href="../mInventory/Scripts/bootstrap.min.css" rel="stylesheet" />
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>
    
     <script src="../Scripts/angular.min.js"></script>
      <script src="../mReports/Scripts/dirPagination.js"></script>
    <link href="../mReports/Scripts/Custom.css" rel="stylesheet" />
    <script src="InboundDetails.js"></script>
  
    <script src="../Scripts/xlsx.full.min.js"></script>
    <script src="../Scripts/xlsx-model.js"></script>
    <link href="../Scripts/mdtimepicker.css" rel="stylesheet" />
    <script src="../Scripts/mdtimepicker.js"></script>
    <script src="Scripts/InventraxAjax.js"></script>

    <script src="../mInventory/CycleCountScripts/jquery.dataTables.min.js"></script>
    <script src="../mInventory/CycleCountScripts/dataTables.bootstrap.min.js"></script>

    <%--<script type="text/javascript">
        
        $(document).ready(function () {
            $('#<%= txtShipmentReceivedDate.ClientID %>').val('<%=(System.DateTime.Now).ToString("dd-MMM-yyyy")%>');
        });
</script>--%>
    <style type="text/css">
        .ui-autocomplete-loading 
        {
            background: white url('../Images/ui-anim_basic_16x16.gif') right center no-repeat;
        }

        .ButEmpty {
            font-size: 13pt;
            color: #0026ff;
            font-weight: bold;
        }

        .inpt input {
            width: 91% !important;
        }

        .accordion ~ br {
            display: none !important;
        }

        .row {
            margin-bottom: 15px;
        }

        input[type="text"] {
            background-color: #fff !important;
        }

            .flex input[type="text"]:focus ~ label, input[type="text"]:valid ~ label {
                top: -7px;
                font-size: 12px;
                color: var(--sideNav-bg);
            }

        /*label {
            width:150px;
        }

        .label {
            width:150px;
        }*/
        .flex_Error:focus {
            /*color:red !important;*/
            border-bottom: 1.5px solid red !important;
        }


        .hideUnderLine input[type="text"] {
            color: rgba(0,0,0,0.42);
            border-bottom: 1px dotted rgba(0,0,0,0.42) !important;
            padding: 10px 0px;
        }

        #MainContent_IBContent_uPanelStoreRefNo {
            position: relative
        }

            #MainContent_IBContent_uPanelStoreRefNo label {
                top: -7px;
                font-size: 12px;
                color: var(--sideNav-bg);
                font-weight: 500;
                position: absolute;
                display: block;
            }
    </style>


    <script type="text/javascript">
        var polineitmes = null;
        $(document).ready(function () {
            $("#Loading").hide();


            $("#divSupplierItemList1").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 50,
                minWidth: 300,
                height: 500,
                width: 850,
                resizable: false,
                draggable: false,
                closeOnEscape: false,
                position: ["center top", 40],
                close: function () {
                    $(".divSupplierItemList1").fadeOut(500);
                    $(document).unbind('scroll');
                    $('body').css({ 'overflow': 'visible' });
                },

                //position: 'center',
                open: function (event, ui) {
                    $(this).parent().appendTo("#disputeDivSupplierItemList1");
                    $(".divSupplierItemList1").hide().fadeIn(500);
                    $('body').css({ 'overflow': 'hidden' });
                    $('body').width($('body').width());
                    $(document).bind('scroll', function () {
                        window.scrollTo(0, 0);
                    });
                }
            });

        });

       
        function Openloader() {
            debugger;
            $("#Loading").show();
           // setTimeout(function () { $("#Loading").show(); }, 3000);
        }

        function closeDialog1() {
            //Could cause an infinite loop because of "on close handling"
            $("#divSupplierItemList1").dialog('close');
            $("#MainContent_IBContent_pnlGRNDetails").focus();
        }

        function disableControls(count) {
            //alert();
            //debugger;
            if (count != 0) {
                $("#atcSupplier").attr("disabled", true);
                $("#txtTenant").attr("disabled", true);
                $("#txtWarehouse").attr("disabled", true);
                $("#txtDocRcvDate").attr("disabled", true);
                $("#atcShipmentType").attr("disabled", true);
            }
            else {
                $("#atcSupplier").attr("disabled", false);
                $("#txtTenant").attr("disabled", false);
                $("#txtWarehouse").attr("disabled", false);
                $("#txtDocRcvDate").attr("disabled", false);
                $("#atcShipmentType").attr("disabled", false);
            }
        }

        function saveLineItems()
        {
            //alert();
            debugger;

             var fieldData = '{<root>';
            $(".chkLineItems").each(function () {
                //alert();
                debugger;

                var x = document.getElementById("tbl").rows.length;
                alert(x);
                if ($(this).prop("checked") == true) {
                    debugger;
                    alert($(this).attr("data-masterid"));
                    console.log(polineitmes);
                    //var dt = $.grep(polineitmes, function (a) { return a.MaterialMasterID == parseInt($(this).attr("data-masterid")) });
                    //console.log(dt);
                    fieldData += '<data>';
                    fieldData += '<MaterialMasterID>' + $(this).attr("data-masterid") + '</MaterialMasterID>';
                    fieldData += '<LineNumber>' + $(this).attr("data-lineno") + '</LineNumber>';
                    fieldData += '<Quantity>' +  $(this).attr("data-qty") + '</Quantity>';
                    fieldData += '<CreatedBy>' + <%=this.cp.UserID%> + '</CreatedBy>';
                    fieldData += '<data>';
                }
            });
            fieldData = fieldData + '</root>}';            
            //fieldDataOut += '}';

                 $("#hdnPOLines").val(fieldData);
            console.log(fieldData);


            $("#divSupplierItemList1").dialog('close');
            $("#MainContent_IBContent_pnlGRNDetails").focus();

       
         
        }
        //function closeDialog() {
        //    //Could cause an infinite loop because of "on close handling"
        //    $("#divItemPrintDataContainer").dialog('close');
        //}

        function GRNUpdateMessage(GrnRowCount) {
            debugger;

            showStickyToast(true, 'GRN details successfully updated', false);
            //location.reload();

            setTimeout(function () { location.reload(); }, 1500);
        }

        function PartialGRN(GrnRowCount) {
            debugger;

            if (GrnRowCount > 0) {
                $("#lnkInvclose").html("Close");

                for (var i = 0; i < GrnRowCount; i++) {
                    $("#MainContent_IBContent_GVPOLineItems_chkIsDelete_" + i).attr("Checked", true);
                    $("#MainContent_IBContent_GVPOLineItems_chkIsDelete_" + i).attr("disabled", true);
                }

            }
        }

        //function openDialog() {
        //    $("#divItemPrintDataContainer").dialog("option", "title", "GRN Pending Goods List");
        //    $("#divItemPrintDataContainer").dialog('open');
        //}

        function openDialog1() {
            debugger;
            var PoId = $("#<%=hifGRNPOHeaderID.ClientID %>").val();
             var InvoiceId = $("#<%=hifInvoiceId.ClientID %>").val();
            $("#divItemPrintDataContainer").hide();
            $("#divSupplierItemList1").dialog("option", "title", "Line Item Details");
            $("#divSupplierItemList1").dialog('open');

            //else
            //{
            //    alert("Please Select Po Number and Invoice #.");
            //    return false;
            //}

           <%-- NProgress.start();

            $("#divSupplierItemList1").block({
                message: '<img src="<%=ResolveUrl("~") %>Images/async_order.gif" />',
                 css: { border: '0px' },
                 fadeIn: 0,
                 fadeOut: 0,
                 overlayCSS: { backgroundColor: '#ffffff', opacity: 1 }
             });--%>            
        }

        function unblockDialog1() {
            $("#divSupplierItemList1").unblock();
        }

        //function unblockDialog() {
        //    $("#divItemPrintDataContainer").unblock();
        //}

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
            alert('The file extension ' + ext.toUpperCase() + ' is not allowed!');
            return false;
        }
   
        var TenantID = 0;
        $(function () {

            var activeIndex1 = parseInt($('#<%=hidAccordionIndex1.ClientID %>').val());
            var activeIndex2 = parseInt($('#<%=hidAccordionIndex2.ClientID %>').val());
            var activeIndex3 = parseInt($('#<%=hidAccordionIndex3.ClientID %>').val());
            var activeIndex4 = parseInt($('#<%=hidAccordionIndex4.ClientID %>').val());
            var activeIndex5 = parseInt($('#<%=hidAccordionIndex5.ClientID %>').val());
            var activeIndex6 = parseInt($('#<%=hidAccordionIndex6.ClientID %>').val());
            var activeIndex7 = parseInt($('#<%=hidAccordionIndex7.ClientID %>').val());
            var activeIndex8 = parseInt($('#<%=hidAccordionIndex8.ClientID %>').val());

            $("#accordion1").accordion({
                expandAll: false,
                alwaysOpen: false,
                autoHeight: false, clearStyle: true,
                active: activeIndex1,
                change: function (event, ui) {
                    var index = $(this).children('h3').index(ui.newHeader);
                    $('#<%=hidAccordionIndex1.ClientID %>').val(index);
                }
            });
            $("#accordion1").accordion({ header: 'h3', collapsible: true, autoHeight: false, clearStyle: true, navigation: true });


            $("#accordion2").accordion({
                alwaysOpen: false,
                autoHeight: false, clearStyle: true,
                active: activeIndex2,
                change: function (event, ui) {
                    var index = $(this).children('h3').index(ui.newHeader);
                    $('#<%=hidAccordionIndex2.ClientID %>').val(index);
                }
            });
            $("#accordion2").accordion({ header: 'h3', collapsible: true, autoHeight: false, clearStyle: true, navigation: true });


            $("#accordion3").accordion({
                alwaysOpen: false,
                autoHeight: false, clearStyle: true,
                active: activeIndex3,
                change: function (event, ui) {
                    var index = $(this).children('h3').index(ui.newHeader);
                    $('#<%=hidAccordionIndex3.ClientID %>').val(index);
                }
            });
            $("#accordion3").accordion({ header: 'h3', collapsible: true, autoHeight: false, clearStyle: true, navigation: true });


            $("#accordion4").accordion({
                alwaysOpen: false,
                autoHeight: false, clearStyle: true,
                active: activeIndex4,
                change: function (event, ui) {
                    var index = $(this).children('h3').index(ui.newHeader);
                    $('#<%=hidAccordionIndex4.ClientID %>').val(index);
                }
            });
            $("#accordion4").accordion({ header: 'h3', collapsible: true, autoHeight: false, clearStyle: true, navigation: true });


            $("#accordion5").accordion({
                collapseAll: false,
                alwaysOpen: false,
                autoHeight: false, clearStyle: true,
                active: activeIndex5,
                change: function (event, ui) {
                    var index = $(this).children('h3').index(ui.newHeader);
                    $('#<%=hidAccordionIndex5.ClientID %>').val(index);
                }
            });
            $("#accordion5").accordion({ header: 'h3', collapsible: true, autoHeight: false, clearStyle: true, navigation: true });

            $("#accordion12").accordion({
                collapseAll: false,
                alwaysOpen: false,
                autoHeight: false, clearStyle: true,
                active: activeIndex5,
                change: function (event, ui) {
                    var index = $(this).children('h3').index(ui.newHeader);
                    $('#<%=hidAccordionIndex5.ClientID %>').val(index);
                }
            });
            $("#accordion12").accordion({ header: 'h3', collapsible: true, autoHeight: false, clearStyle: true, navigation: true });

            $("#accordion8").accordion({
                collapseAll: false,
                alwaysOpen: false,
                autoHeight: false, clearStyle: true,
                active: activeIndex5,
                change: function (event, ui) {
                    var index = $(this).children('h3').index(ui.newHeader);
                    $('#<%=hidAccordionIndex5.ClientID %>').val(index);
                }
            });
            $("#accordion8").accordion({ header: 'h3', collapsible: true, autoHeight: false, clearStyle: true, navigation: true });


            $("#accordion6").accordion({
                collapseAll: false,
                alwaysOpen: false,
                autoHeight: false, clearStyle: true,
                active: activeIndex6,
                change: function (event, ui) {
                    var index = $(this).children('h3').index(ui.newHeader);
                    $('#<%=hidAccordionIndex6.ClientID %>').val(index);
                }
            });
            $("#accordion6").accordion({ header: 'h3', collapsible: true, autoHeight: false, clearStyle: true, navigation: true });

            $("#accordion7").accordion({
                collapseAll: false,
                alwaysOpen: false,
                autoHeight: false, clearStyle: true,
                active: activeIndex7,
                change: function (event, ui) {
                    var index = $(this).children('h3').index(ui.newHeader);
                    $('#<%=hidAccordionIndex7.ClientID %>').val(index);
                }
            });
            $("#accordion7").accordion({ header: 'h3', collapsible: true, autoHeight: false, clearStyle: true, navigation: true });


            $("#accordion10").accordion({
                alwaysOpen: false,
                autoHeight: false, clearStyle: true,
                active: activeIndex8,
                change: function (event, ui) {
                    var index = $(this).children('h3').index(ui.newHeader);
                    $('#<%=hidAccordionIndex8.ClientID %>').val(index);
                }
            });
            $("#accordion10").accordion({ header: 'h3', collapsible: true, autoHeight: false, clearStyle: true, navigation: true });

        });
        
        function OpenImage(path) {
            window.open(path, 'Naresh', 'height=800,width=900');
        }

        function printDiv() {

            // Print the DIV.
            $(".tdDDRPrintArea").print();
        }


        function check_uncheck(Val) {
            var ValChecked = Val.checked;
            var ValId = Val.id;
            var frm = document.forms[0];
            // Loop through all elements
            for (i = 0; i < frm.length; i++) {
                // Look for Header Template's Checkbox
                //As we have not other control other than checkbox we just check following statement

                if (this != null) {

                    if (ValId.indexOf('chkIsDeleteRFItemsAll') != -1) {
                        // Check if main checkbox is checked,
                        // then select or deselect datagrid checkboxes

                        if (frm.elements[i].type == 'checkbox' && frm.elements[i].name.indexOf('chkIsDeleteRFItem') != -1) {
                            if (ValChecked)
                                frm.elements[i].checked = true;
                            else
                                frm.elements[i].checked = false;
                        }
                    }
                    else if (ValId.indexOf('chkIsDeleteRFItem') != -1) {
                        // Check if any of the checkboxes are not checked, and then uncheck top select all checkbox
                        if (frm.elements[i].checked == false)
                            frm.elements[1].checked = false;
                    }

                }

            } // for
        } // function

        function confirmMsg() {

            //alert(frm.elements[i].name.indexOf("chkIsDelete"));
            var frm = document.forms[0];
            // loop through all elements
            for (i = 0; i < frm.length; i++) {
                // Look for our checkboxes only
                if (frm.elements[i].name.indexOf("chkIsDeleteRFItem") != -1) {
                    // If any are checked then confirm alert, otherwise nothing happens
                    if (frm.elements[i].checked)
                        return confirm('Are you sure you want to delete your selection(s)?')
                }

            }
        }


        function check_uncheck1(Val) {
            var ValChecked = Val.checked;
            var ValId = Val.id;
            var frm = document.forms[0];
            // Loop through all elements
            for (i = 0; i < frm.length; i++) {
                // Look for Header Template's Checkbox
                //As we have not other control other than checkbox we just check following statement

                if (this != null) {

                    if (ValId.indexOf('chkIsDeleteRFItemsAll1') != -1) {
                        // Check if main checkbox is checked,
                        // then select or deselect datagrid checkboxes

                        if (frm.elements[i].type == 'checkbox' && frm.elements[i].name.indexOf('chkIsDeleteRFItem1') != -1) {
                            if (ValChecked)
                                frm.elements[i].checked = true;
                            else
                                frm.elements[i].checked = false;
                        }
                    }
                    else if (ValId.indexOf('chkIsDeleteRFItem1') != -1) {
                        // Check if any of the checkboxes are not checked, and then uncheck top select all checkbox
                        if (frm.elements[i].checked == false)
                            frm.elements[1].checked = false;
                    }

                }

            } // for
        } // function

        function confirmMsg1() {

            //alert(frm.elements[i].name.indexOf("chkIsDelete"));
            var frm = document.forms[0];
            // loop through all elements
            for (i = 0; i < frm.length; i++) {
                // Look for our checkboxes only
                if (frm.elements[i].name.indexOf("chkIsDeleteRFItem1") != -1) {
                    // If any are checked then confirm alert, otherwise nothing happens
                    if (frm.elements[i].checked)
                        return confirm('Are you sure you want to delete your selection(s)?')
                }

            }
        }


        function check_uncheck2(Val) {
            var ValChecked = Val.checked;
            var ValId = Val.id;
            var frm = document.forms[0];
            // Loop through all elements
            for (i = 0; i < frm.length; i++) {
                // Look for Header Template's Checkbox
                //As we have not other control other than checkbox we just check following statement

                if (this != null) {

                    if (ValId.indexOf('chkIsDeletePOInvItemsAll') != -1) {
                        // Check if main checkbox is checked,
                        // then select or deselect datagrid checkboxes

                        if (frm.elements[i].type == 'checkbox' && frm.elements[i].name.indexOf('chkIsDeletePOInvItems') != -1) {
                            if (ValChecked)
                                frm.elements[i].checked = true;
                            else
                                frm.elements[i].checked = false;
                        }
                    }
                    else if (ValId.indexOf('chkIsDeletePOInvItems') != -1) {
                        // Check if any of the checkboxes are not checked, and then uncheck top select all checkbox
                        if (frm.elements[i].checked == false)
                            frm.elements[1].checked = false;
                    }

                }

            } // for
        } // function

        function confirmMsg2() {

            //alert(frm.elements[i].name.indexOf("chkIsDelete"));
            var frm = document.forms[0];
            // loop through all elements
            for (i = 0; i < frm.length; i++) {
                // Look for our checkboxes only
                if (frm.elements[i].name.indexOf("chkIsDeletePOInvItems") != -1) {
                    // If any are checked then confirm alert, otherwise nothing happens
                    if (frm.elements[i].checked)
                        return confirm('Are you sure you want to delete your selection(s)?')
                }

            }
        }


        function check_uncheck3(Val) {
            var ValChecked = Val.checked;
            var ValId = Val.id;
            var frm = document.forms[0];
            // Loop through all elements
            for (i = 0; i < frm.length; i++) {
                // Look for Header Template's Checkbox
                //As we have not other control other than checkbox we just check following statement

                if (this != null) {

                    if (ValId.indexOf('CheckAll') != -1) {
                        // Check if main checkbox is checked,
                        // then select or deselect datagrid checkboxes

                        if (frm.elements[i].type == 'checkbox' && frm.elements[i].name.indexOf('deleteRec') != -1) {
                            if (ValChecked)
                                frm.elements[i].checked = true;
                            else
                                frm.elements[i].checked = false;
                        }
                    }
                    else if (ValId.indexOf('deleteRec') != -1) {
                        // Check if any of the checkboxes are not checked, and then uncheck top select all checkbox
                        if (frm.elements[i].checked == false)
                            frm.elements[1].checked = false;
                    }

                }

            } // for
        } // function


        function check_uncheckTPL(Val) {
            var ValChecked = Val.checked;
            var ValId = Val.id;
            var frm = document.forms[0];
            // Loop through all elements
            for (i = 0; i < frm.length; i++) {
                // Look for Header Template's Checkbox
                //As we have not other control other than checkbox we just check following statement

                if (this != null) {

                    if (ValId.indexOf('CheckAllForTPLInboundAll') != -1) {
                        // Check if main checkbox is checked,
                        // then select or deselect datagrid checkboxes

                        if (frm.elements[i].type == 'checkbox' && frm.elements[i].name.indexOf('deleteTPLInboundCharge') != -1) {
                            if (ValChecked)
                                frm.elements[i].checked = true;
                            else
                                frm.elements[i].checked = false;
                        }
                    }
                    else if (ValId.indexOf('deleteTPLInboundCharge') != -1) {
                        // Check if any of the checkboxes are not checked, and then uncheck top select all checkbox
                        if (frm.elements[i].checked == false)
                            frm.elements[1].checked = false;
                    }
                }
            } // for
        }

        function check_uncheckTPLRcv(Val) {
            var ValChecked = Val.checked;
            var ValId = Val.id;
            var frm = document.forms[0];
            // Loop through all elements
            for (i = 0; i < frm.length; i++) {
                // Look for Header Template's Checkbox
                //As we have not other control other than checkbox we just check following statement

                if (this != null) {

                    if (ValId.indexOf('CheckTPLRcvAll') != -1) {

                        // Check if main checkbox is checked,
                        // then select or deselect datagrid checkboxes

                        if (frm.elements[i].type == 'checkbox' && frm.elements[i].name.indexOf('deleteTPLReceivingCharge') != -1) {
                            if (ValChecked)
                                frm.elements[i].checked = true;
                            else
                                frm.elements[i].checked = false;
                        }
                    }
                    else if (ValId.indexOf('deleteTPLReceivingCharge') != -1) {

                        // Check if any of the checkboxes are not checked, and then uncheck top select all checkbox
                        if (frm.elements[i].checked == false)
                            frm.elements[1].checked = false;
                    }
                } // if
            } // for
        }
        function selectAll() {
            debugger;
            
            console.log($("#MainContent_IBContent_GVPOLineItems_action").is(":checked"));
           // console.log($("#MainContent_IBContent_GVPOLineItems_action").prop("checked"));
            if ($("#MainContent_IBContent_GVPOLineItems_action").prop("checked")==true ) {
                $(".allcheck").prop("checked", true);
            }
            else {
                 $(".allcheck").prop("checked", false);
            }
           
        }

        $(document).ready(function () {
            $("#spanClose").click(function () {
                $("#divContainer").hide();
            });
            fnLoadMCode();
   
        });
        $(document).ready(function () {

            if (document.getElementById('<%= this.hifShipmentType.ClientID %>').value == "2" || document.getElementById('<%= this.hifShipmentType.ClientID %>').value == "6") {
                //document.getElementById("trConsignmentNoteType").style.display = "none !important";
                //document.getElementById("trClearanceCompany").style.display = "none !important";
                $("#MainContent_IBContent_UpdatePanel1").css("display", "none");
                $("#MainContent_IBContent_UpdatePanel2").css("display", "none");
                $("#MainContent_IBContent_UpdatePanel3").css("display", "none");
                $("#MainContent_IBContent_UpdatePanel5").css("display", "none");
            }
        });

        var UomResult;
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                fnLoadMCode();
            }
        }
        function fnLoadMCode() {
            $(".DynaGRNDate").datepicker({ dateFormat: "dd-M-yy", minDate: 0 });
            $("#<%= this.txtShipmentExpectedDate.ClientID %>").datepicker({ minDate: 0, dateFormat: "dd-M-yy" });
             $("#<%= this.txtOffLoadingTime.ClientID %>").timeEntry();
              <%--$("#<%= this.txtOffLoadingTime.ClientID %>").mdtimepicker();--%>
            $("#<%= this.txtConsignmentNoteTypeDate.ClientID %>").datepicker({
                dateFormat: "dd-M-yy", changeMonth: true,
                changeYear: true});
             $("#<%= this.txtCheckedDate.ClientID%>").datepicker({
                 dateFormat: "dd-M-yy",
                 minDate: new Date()
             });

             $("#<%= this.txtVerifiedDate.ClientID%>").datepicker({
                 dateFormat: "dd-M-yy",
                 minDate: new Date()
             });

             $('.DateBoxCSS_small').datepicker({ dateFormat: "dd/mm/yy" });
             var textfieldname = $('.DynaDisInvNumber');
             DropdownFunction(textfieldname);
             $('.DynaDisInvNumber').autocomplete({
                 source: function (request, response) {
                     $.ajax({
                         url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadDisInvoiceNumbers") %>',
                         data: "{ 'prefix': '" + request.term + "' , 'InboundID' : ' " + document.getElementById("<%=hifInboundID.ClientID %>").value + "'}",
                         dataType: "json",
                         type: "POST",
                         contentType: "application/json; charset=utf-8",
                         success: function (data) {

                             if (data.d == "") {
                                 alert("No Invoice No. is configured to this PO");
                                 return;
                             }
                             else {
                                 response($.map(data.d, function (item) {
                                     return {
                                         label: item.split(',')[0],
                                         val: item.split(',')[1]
                                     }
                                 }))
                             }
                         },
                         error: function (response) {

                         },
                         failure: function (response) {

                         }
                     });
                 },
                 select: function (e, i) {
                     $("#<%=hifDisInvNumberID.ClientID %>").val(i.item.val);

                 },
                 minLength: 0
             });

             var textfieldname = $('.DescMCodePicker');
             DropdownFunction(textfieldname);
             $('.DescMCodePicker').autocomplete({
                 source: function (request, response) {
                     $.ajax({
                         url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadPOMCodes") %>',
                             data: "{ 'prefix': '" + request.term + "' , 'InboundID' : ' " + document.getElementById("<%=hifInboundID.ClientID %>").value + "' , 'SupplierInvoiceID' : ' " + document.getElementById("<%=hifDisInvNumberID.ClientID %>").value + " '}",
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
                     $("#hifDescMCodeID").val(i.item.val);
                     $("#<%=hifDisMMID.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });


            var textfieldname = $('.DescPOLineNumber');
            DropdownFunction(textfieldname);
            $('.DescPOLineNumber').autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadPOLineNumbers") %>',
                             data: "{ 'prefix': '" + request.term + "' , 'InboundID' : ' " + document.getElementById("<%=hifInboundID.ClientID %>").value + "' , 'SupplierInvoiceID' : ' " + document.getElementById("<%=hifDisInvNumberID.ClientID %>").value + "' , 'MMID' : ' " + document.getElementById("<%=hifDisMMID.ClientID %>").value + "' , 'POHeaderID' : ' " + document.getElementById("<%=hifPONumberID.ClientID %>").value + " '}",
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
                minLength: 0
            });


            var textfieldname = $('.DynaDescPONumber');
            DropdownFunction(textfieldname);

            $('.DynaDescPONumber').autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadConfiguredIBPONumbers") %>',
                         data: "{ 'prefix': '" + request.term + "', 'TenentID': '" + document.getElementById("<%=hidTenantID.ClientID %>").value + "' , 'InboundID' : ' " + document.getElementById("<%=hifInboundID.ClientID %>").value + "' , 'SupplierInvoiceID' : ' " + document.getElementById("<%=hifDisInvNumberID.ClientID %>").value + " '}",
                         dataType: "json",
                         type: "POST",
                         contentType: "application/json; charset=utf-8",
                         success: function (data) {
                             if (data.d == "" || data.d == "/,") {
                                 alert("No PO Numbers are configured to this shipment");
                                 return;
                             }
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
                     $("#<%=hifPONumberID.ClientID %>").val(i.item.val);


                },
                minLength: 0
            });



            var textfieldname = $('.DescMUoM');
            DropdownFunction(textfieldname);
            $('.DescMUoM').autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/MaterialUoMList") %>',
                        data: "{ 'prefix': '" + document.getElementsByClassName("DescMUoM").value + "', 'MMID': '" + document.getElementById("hifDescMCodeID").value + "' }",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {

                            if (data.d == "/,") {
                                alert('No UoM\'s are configured to this Material');
                                return;
                            }
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
                    $("#hifDescMUoMID").val(i.item.val);
                },
                minLength: 0
            });



            var textfieldname = $('.DynaGRNUpdatedBy');
            DropdownFunction(textfieldname);

            $(".DynaGRNUpdatedBy").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadUsersData2") %>',
                                 data: "{ 'prefix': '" + request.term + "', 'TenantID': '" + '<%= ViewState["TenantID"] %>' + "',WarehouseID:'" + $("#<%=hdnWarehouse.ClientID %>").val() + "'}",
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
                             $("#<%=hifGRNDoneBy.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });



            var textfieldname = $('.DynaPONumber');
            DropdownFunction(textfieldname);

            $('.DynaPONumber').autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadPONumbers") %>',
                         data: "{ 'prefix': '" + request.term + "', 'TenentID': '" + document.getElementById("<%=hidTenantID.ClientID %>").value + "', 'SupplierID': '" + document.getElementById("<%=hifSupplier.ClientID %>").value + "'}",
                         dataType: "json",
                         type: "POST",
                         contentType: "application/json; charset=utf-8",
                         success: function (data) {
                             if (data.d == "" || data.d == "/,") {
                                 alert("No PO Numbers are configured to this Supplier");
                                 return;
                             }

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
                     alert("sss");
                     $("#<%=hifPONumberID.ClientID %>").val(i.item.val);


                },
                minLength: 0
            });

            var textfieldname = $('.vDynaPONumber');
            DropdownFunction(textfieldname);
            $(".vDynaPONumber").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadPONumbers") %>',
                             data: "{ 'prefix': '" + request.term + "', 'TenentID': '" + document.getElementById("<%=hidTenantID.ClientID %>").value + "', 'SupplierID': '" + document.getElementById("<%=hifSupplier.ClientID %>").value + "'}",
                             dataType: "json",
                             type: "POST",
                             contentType: "application/json; charset=utf-8",
                             success: function (data) {
                                 if (data.d == "" || data.d == "/,") {
                                     alert("No PO Numbers are configured to this Supplier");
                                     return;
                                 }

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

                         $("#<%=hifDynaPOHeaderID.ClientID %>").val(i.item.val);

                },
                minLength: 0
            });

            var textfieldname = $('.DynaGRNPONumber');
            DropdownFunction(textfieldname);
            $(".DynaGRNPONumber").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadConfiguredGRNIBPONumbers") %>',
                         data: "{ 'prefix': '" + request.term + "', 'TenentID': '" + document.getElementById("<%=hidTenantID.ClientID %>").value + "' , 'InboundID' : ' " + document.getElementById("<%=hifInboundID.ClientID %>").value + " '}",
                         dataType: "json",
                         type: "POST",
                         contentType: "application/json; charset=utf-8",
                         success: function (data) {
                             if (data.d == "" || data.d == "/,") {
                                 alert("No PO Numbers are configured to this Supplier");
                                 return;
                             }

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


                     $("#<%=hifGRNPOHeaderID.ClientID %>").val(i.item.val);
                },

                minLength: 0
            });



            var textfieldname = $('.DynaGRNInvNumber');
            DropdownFunction(textfieldname);

            $('.DynaGRNInvNumber').autocomplete({
                source: function (request, response) {

                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadGRNPOInvoiceNumbers") %>',
                              data: "{ 'prefix': '" + request.term + "' , 'POHeaderID' : ' " + document.getElementById("<%=hifGRNPOHeaderID.ClientID %>").value + "' , 'SupplierInvoiceID' : '" + document.getElementById('hifGRNSupplierInvoiceID').value + "' , 'InboundID' : '" + document.getElementById("<%=hifInboundID.ClientID %>").value + "'}",
                              dataType: "json",
                              type: "POST",
                              contentType: "application/json; charset=utf-8",
                              success: function (data) {

                                  if (data.d == "") {
                                      alert("No invoice no. is configured to this PO");
                                      return;
                                  }
                                  else {
                                      response($.map(data.d, function (item) {
                                          return {
                                              label: item.split(',')[0],
                                              val: item.split(',')[1]
                                          }
                                      }))
                                  }
                              },
                              error: function (response) {

                              },
                              failure: function (response) {

                              }
                          });
                      },
                      select: function (e, i) {
                          $("#<%=hifInvoiceId.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });

            var textfieldname = $('.DynaInvNumber');
            DropdownFunction(textfieldname);
            $('.DynaInvNumber').autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadInvoiceNumbers") %>',
                             data: "{ 'prefix': '" + request.term + "' , 'SupplierID' : ' " + document.getElementById("<%=hifSupplier.ClientID %>").value + "', 'POHeaderID': '" + document.getElementById("<%=hifDynaPOHeaderID.ClientID %>").value + "', 'SupplierInvoiceID': '" + document.getElementById('hifInvNumberID').value + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {

                            if (data.d == "") {
                                alert("No invoice no. is configured to this PO");
                                return;
                            }
                            else {
                                response($.map(data.d, function (item) {
                                    return {
                                        label: item.split(',')[0],
                                        val: item.split(',')[1]
                                    }
                                }))
                            }
                        },
                        error: function (response) {

                        },
                        failure: function (response) {

                        }
                    });
                },
                select: function (e, i) {
                    $("#hifInvNumberID").val(i.item.val);


                },
                minLength: 0
            });


            //for line number and material master

            var textfieldname = $('.DynalineNumber');
            DropdownFunction(textfieldname);

            $('.DynalineNumber').autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadPOLineNumbersforInbound") %>',
                         data: "{ 'prefix': '" + request.term + "' , 'SupplierID' : ' " + document.getElementById("<%=hifSupplier.ClientID %>").value + "', 'POHeaderID': '" + document.getElementById("<%=hifGRNPOHeaderID.ClientID %>").value + "', 'MaterialMasterID': '" + document.getElementById("<%=hifMaterialId.ClientID %>").value + "'}",
                         dataType: "json",
                         type: "POST",
                         contentType: "application/json; charset=utf-8",
                         success: function (data) {

                             if (data.d == "") {
                                 alert("No Line Number is configured to this Material.");
                                 return;
                             }
                             else {
                                 response($.map(data.d, function (item) {
                                     return {
                                         label: item.split(',')[0],
                                         val: item.split(',')[1]
                                     }
                                 }))
                             }
                         },
                         error: function (response) {

                         },
                         failure: function (response) {

                         }
                     });
                 },
                 select: function (e, i) {
                     //$("#hifMaterialId").val(i.item.val);
                     $("#<%=hifgrnpolinenumber.ClientID %>").val(i.item.label);

                },
                minLength: 0
            });




            var textfieldname = $('.DynaMaterialCode');
            DropdownFunction(textfieldname);

            $('.DynaMaterialCode').autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadMaterialCodesforInbound") %>',
                        data: "{ 'prefix': '" + request.term + "' , 'SupplierID' : ' " + document.getElementById("<%=hifSupplier.ClientID %>").value + "', 'POHeaderID': '" + document.getElementById("<%=hifGRNPOHeaderID.ClientID %>").value + "', 'SupplierInvoiceID': '" + document.getElementById("<%=hifInvoiceId.ClientID %>").value + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {

                            if (data.d == "") {
                                alert("No Material is configured to this Invoice.");
                                return;
                            }
                            else {
                                response($.map(data.d, function (item) {
                                    return {
                                        label: item.split(',')[0],
                                        val: item.split(',')[1]
                                    }
                                }))
                            }
                        },
                        error: function (response) {

                        },
                        failure: function (response) {

                        }
                    });
                },
                select: function (e, i) {

                    $("#<%=hifMaterialId.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });

            // End 

            var textfieldname = $("#<%= this.atcCheckedBy.ClientID %>");
            DropdownFunction(textfieldname);
            $("#<%= this.atcCheckedBy.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadUsersData") %>',
                         data: "{ 'prefix': '" + request.term + "', 'TenantID': '" + '<%= ViewState["TenantID"] %>' + "'}",
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
                     $("#<%=hifCheckedBy.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });
            var textfieldname = $("#<%= this.txtVerifiedBy.ClientID %>");
            DropdownFunction(textfieldname);
            $("#<%= this.txtVerifiedBy.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadUsersData") %>',
                         data: "{ 'prefix': '" + request.term + "', 'TenantID': '" + '<%= ViewState["TenantID"] %>' + "'}",
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
                     $("#<%=hifVerifiedBy.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });

            var textfieldname = $("#txtActivityRateType");
            DropdownFunction(textfieldname);
            $("#txtActivityRateType").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetDataForActivityrateType") %>',
                        data: "{ 'prefix': '" + request.term + "', 'ActivityRateGroupID': '3', 'ActivityRateTypeID':'2' }",
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
                    $("#hifActivityRateTypeID").val(i.item.val);
                    $("#txtActivityRateName").val("");
                    //alert($("#hifChargeDetailID").val());
                },
                minLength: 0
            });


            var textfieldname = $("#txtActivityRateName");
            DropdownFunction(textfieldname);
            $("#txtActivityRateName").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetDataForActivityName_Inbound") %>',
                        data: "{ 'prefix': '" + request.term + "', 'ActivityRateTypeID': '" + $("#hifActivityRateTypeID").val() + "','InboundId':'" + document.getElementById("<%=hifInboundID.ClientID %>").value + "' }",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {

                            if (data.d == "") {
                                alert("No Rates are configured.");
                                $("#txtActivityRateName").val("");
                                return;
                            }
                            else {

                                response($.map(data.d, function (item) {
                                    return {
                                        label: item.split(',')[0],
                                        val: item.split(',')[1]
                                    }
                                }))
                            }
                        },
                        error: function (response) {

                        },
                        failure: function (response) {

                        }
                    });
                },
                select: function (e, i) {
                    $("#hifActivityRateID").val(i.item.val);
                    //alert($("#hifChargeConfigurationID").val());
                },
                minLength: 0
            });

            var textfieldname = $("#txtRcvActivityRateType");
            DropdownFunction(textfieldname);
            $("#txtRcvActivityRateType").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetDataForActivityrateType") %>',
                        data: "{ 'prefix': '" + request.term + "', 'ActivityRateGroupID': '3', 'ActivityRateTypeID':'3' }",
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
                    $("#hifRcvActivityRateTypeID").val(i.item.val);
                    $("#txtRcvActivityRateName").val("");
                    //alert($("#hifChargeDetailID").val());
                },
                minLength: 0
            });

            var textfieldname = $("#txtRcvActivityRateName");
            DropdownFunction(textfieldname);
            $("#txtRcvActivityRateName").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetDataForActivityName_Inbound") %>',
                        data: "{ 'prefix': '" + request.term + "', 'ActivityRateTypeID': '" + $("#hifRcvActivityRateTypeID").val() + "', 'InboundId':'" + document.getElementById("<%=hifInboundID.ClientID %>").value + "' }",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {

                            if (data.d == "") {
                                alert("No Rates are configured.");
                                $("#txtRcvActivityRateName").val("");
                                return;
                            }
                            else {

                                response($.map(data.d, function (item) {
                                    return {
                                        label: item.split(',')[0],
                                        val: item.split(',')[1]
                                    }
                                }))
                            }
                        },
                        error: function (response) {

                        },
                        failure: function (response) {

                        }
                    });
                },
                select: function (e, i) {
                    $("#hifRcvActivityRateID").val(i.item.val);
                    //alert($("#hifChargeConfigurationID").val());
                },
                minLength: 0
            });



            //Transfer


            $("#<%= this.txtDocRcvDate.ClientID %>").datepicker({
                //dateFormat: "dd-M-yy", minDate: 0, changeMonth: true,
                dateFormat: "dd-M-yy", changeMonth: true,
                changeYear: true
            })
            $("#<%= this.txtConsignmentNoteTypeDate.ClientID %>").datepicker({
                dateFormat: "dd-M-yy", changeMonth: true,
                changeYear: true
            });
            $("#<%= this.txtClearenceInvoiceDate.ClientID %>").datepicker({
                dateFormat: "dd-M-yy", changeMonth: true,
                changeYear: true
            });
            $("#<%= this.txtPriorityDate.ClientID %>").datepicker({ dateFormat: "dd-M-yy", minDate: 0 });
            $("#<%= this.txtShipmentVerifiedDate.ClientID %>").datepicker({
                dateFormat: "dd-M-yy",
                minDate: new Date()

            });
            $("#<%= this.txtFreightInvoiceDate.ClientID%>").datepicker({
                dateFormat: "dd-M-yy", changeMonth: true,
                changeYear: true
            });
            $("#<%= this.txtShipmentReceivedDate.ClientID%>").datepicker({
                dateFormat: "dd-M-yy", changeMonth: true,
                changeYear: true
            });
            $(".DynaGRNDate").datepicker({
                dateFormat: "dd-M-yy", minDate: 0, changeMonth: true,
                changeYear: true
            });
            $("#<%= this.txtTimeEntry.ClientID %>").timeEntry();
            <%--$("#<%= this.txtTimeEntry.ClientID %>").mdtimepicker();--%>
            $("#<%= this.txtShipmentExpectedDate.ClientID %>").datepicker({
                minDate: 0, dateFormat: "dd-M-yy", changeMonth: true,
                changeYear: true
            });

            $("#<%= this.atcSupplier.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadSupplierDataFor3PL") %>',
                        data: "{ 'prefix': '" + request.term + "','TenantID':'" + $('#<%=this.hidTenantID.ClientID%>').val() + "','Type':'PO'}",//<=cp.TenantID%>
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
            var textfieldname = $("#<%= this.atcSupplier.ClientID %>");
            DropdownFunction(textfieldname);


            var accountid = '<%=this.cp.AccountID%>';

            var textfieldname = $('#<%=txtAccount.ClientID%>');
            DropdownFunction(textfieldname);
            $("#<%= this.txtAccount.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadAccountForWHList") %>',
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
                        },
                        error: function (response) {

                        },
                        failure: function (response) {

                        }
                    });
                },
                select: function (e, i) {
                    $("#<%=hdnAccount.ClientID %>").val(i.item.val);
                    TenantID = $("#<%=hdnAccount.ClientID %>").val();
                },
                minLength: 0,

            });//.val(accountid).data('autocomplete')._trigger('');





            var textfieldname = $('#<%=txtTenant.ClientID%>');
            DropdownFunction(textfieldname);
            $("#<%= this.txtTenant.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                       <%-- url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadTenantsForINBWHList") %>',--%>
                      <%--  data: "{ 'prefix': '" + request.term + "','AccountID':'" + $('#<%=this.hdnAccount.ClientID%>').val() + "'}",//<=cp.TenantID%>--%>
                        //data: "{ 'prefix': '" + request.term + "'}",
                        //<=cp.TenantID%>
                        url: '../mWebServices/FalconWebService.asmx/GetWarehouseTenant',
                        data: "{ 'prefix': '" + request.term + "','WHID':'" + $("#<%=hdnWarehouse.ClientID %>").val() +"'}",
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
                    $("#<%=hidTenantID.ClientID %>").val(i.item.val);
                    $("#<%=atcSupplier.ClientID %>").val("");

                    var TenantID = $("#<%=hidTenantID.ClientID %>").val();
                },
                minLength: 0,

            });

            var textfieldname = $("#<%= this.txtTenant.ClientID %>");
            DropdownFunction(textfieldname);


            var textfieldname = $('#<%=txtWarehouse.ClientID%>');
            DropdownFunction(textfieldname);
            $("#<%= this.txtWarehouse.ClientID %>").autocomplete({

                source: function (request, response) {
                    $.ajax({
                       <%-- url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadWHForWHListWithUserID") %>',
                        data: "{ 'prefix': '" + request.term + "','TenantID':'" + $('#<%=this.hidTenantID.ClientID%>').val() + "'}",--%>
                        url: '../mWebServices/FalconWebService.asmx/LoaDWHListBasedonUser',
                        data: "{ 'prefix': '" + request.term + "'}", 
                        //<=cp.TenantID%>
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
                    $("#<%=hdnWarehouse.ClientID %>").val(i.item.val);
                    var TenantID = $("#<%=hdnWarehouse.ClientID %>").val();
                },
                minLength: 0,

            });

            var textfieldname = $("#<%= this.txtWarehouse.ClientID %>");
            DropdownFunction(textfieldname);




            debugger;
            $("#<%= this.atcClearenceCompany.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadClearenceCompanyData") %>',
                        data: "{ 'prefix': '" + request.term + "', 'TenantID': '" + '<%= ViewState["TenantID"] %>' + "'}",
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
                        },
                        error: function (response) {

                        },
                        failure: function (response) {

                        }
                    });
                },
                select: function (e, i) {
                    $("#<%=hifClearenceCompany.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });
            var textfieldname = $("#<%= this.atcClearenceCompany.ClientID %>");
            DropdownFunction(textfieldname);






            var textfieldname = $("#<%= this.atcShipmentType.ClientID %>");
            DropdownFunction(textfieldname);

            $("#<%= this.atcShipmentType.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadShipmentTypeData") %>',
                        data: "{ 'prefix': '" + request.term + "', 'TenantID': '" + '<%= ViewState["TenantID"] %>' + "'}",
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
                    debugger;
                    $("#<%=hifShipmentType.ClientID %>").val(i.item.val);

                    if (document.getElementById('<%= this.hifShipmentType.ClientID %>').value == "2" || document.getElementById('<%= this.hifShipmentType.ClientID %>').value == "6") {
                        //document.getElementById("trConsignmentNoteType").style.display = "none !important";
                        //document.getElementById("trClearanceCompany").style.display = "none !important";
                        $("#MainContent_IBContent_UpdatePanel1").css("display", "none");
                        $("#MainContent_IBContent_UpdatePanel2").css("display", "none");
                        $("#MainContent_IBContent_UpdatePanel3").css("display", "none");
                        $("#MainContent_IBContent_UpdatePanel5").css("display", "none");
                        //$("#MainContent_IBContent_ddlConsignmentNoteType").val("0");
                        //$("#MainContent_IBContent_txtConsignmentNoteTypeValue").val("");
                        //$("#MainContent_IBContent_txtConsignmentNoteTypeDate").val("");
                        //$("#MainContent_IBContent_atcClearenceCompany").val("");
                        //$("#MainContent_IBContent_txtClearenceCompanyInvoice").val("");
                        //$("#MainContent_IBContent_txtClearenceInvoiceDate").val("");
                        //$("#MainContent_IBContent_txtClearenceCompanyAmount").val("");
                        //$("#MainContent_IBContent_hifClearenceCompany").val("");

                    }
                    else {
                        //document.getElementById("trConsignmentNoteType").style.display = "table-row !important";
                        //document.getElementById("trClearanceCompany").style.display = "table-row !important";
                        $("#MainContent_IBContent_UpdatePanel1").css("display", "block");
                        $("#MainContent_IBContent_UpdatePanel2").css("display", "block");
                        $("#MainContent_IBContent_UpdatePanel3").css("display", "block");
                        $("#MainContent_IBContent_UpdatePanel5").css("display", "block");
                        $("#MainContent_IBContent_ddlConsignmentNoteType").val("0");
                        $("#MainContent_IBContent_txtConsignmentNoteTypeValue").val("");
                        $("#MainContent_IBContent_txtConsignmentNoteTypeDate").val("");
                        $("#MainContent_IBContent_atcClearenceCompany").val("");
                        $("#MainContent_IBContent_txtClearenceCompanyInvoice").val("");
                        $("#MainContent_IBContent_txtClearenceInvoiceDate").val("");
                        $("#MainContent_IBContent_txtClearenceCompanyAmount").val("");
                        $("#MainContent_IBContent_hifClearenceCompany").val("");
                    }
                },
                minLength: 0
            });

            var textfieldname = $('#<%=txtdock.ClientID%>');
            DropdownFunction(textfieldname);
            $("#<%= this.txtdock.ClientID %>").autocomplete({
                source: function (request, response) {

                    var ControlWarehouseID = $('#<%=hdnWarehouse.ClientID%>');


                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadDockFor3PL") %>',
                        data: "{ 'prefix': '" + ControlWarehouseID.val() + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {

                            if (data.d == "") {
                                //  alert("No Dock is available");
                                showStickyToast(false, "No Docks available in the warehouse.");
                                return;
                            }
                            else {
                                response($.map(data.d, function (item) {
                                    return {
                                        label: item.split(',')[0],
                                        val: item.split(',')[1]
                                    }
                                }))
                            }

                        },
                        error: function (response) {

                        },
                        failure: function (response) {

                        }
                    });
                },
                select: function (e, i) {
                    $("#<%=hdfdock.ClientID %>").val(i.item.val);

                },
                minLength: 0,
            });

            //End Transfer






        }
        fnLoadMCode();

        $(document).ready(function () {
            //$("#divItemPrintData").dialog({
            //    autoOpen: false,
            //    modal: true,
            //    minHeight: 200,
            //    height: 450,
            //    width: 700,
            //    overflow: 'auto',
            //    resizable: false,
            //    draggable: false,
            //    position: ["center top", 40],

            //    close: function () {

            //        $(".ui-dialog").fadeOut(500);

            //        $(document).unbind('scroll');

            //        $('body').css({ 'overflow': 'visible' });

            //    },
            //    title: "Pending Goods-IN List",
            //    open: function (event, ui) {
            //        $(".ui-dialog").hide().fadeIn(500);

            //        $('body').css({ 'overflow': 'hidden' });

            //        //$('body').width($('body').width());

            //        $(document).bind('scroll', function () {
            //            window.scrollTo(0, 0);
            //        });

            //        $(this).parent().appendTo("#divItemPrintDataContainer");
            //    }
            //});

            $("#divItemPrintData").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 50,
                minWidth: 300,
                height: 500,
                width: 850,
                resizable: false,
                draggable: false,
                closeOnEscape: false,
                position: ["center top", 40],
                close: function () {
                    $(".divItemPrintData").fadeOut(500);
                    $(document).unbind('scroll');
                    $('body').css({ 'overflow': 'visible' });
                },
                title: "Pending Goods-IN List",
                //position: 'center',
                open: function (event, ui) {
                    $(this).parent().appendTo("#divItemPrintDataContainer");
                    $(".divItemPrintData").hide().fadeIn(500);
                    $('body').css({ 'overflow': 'hidden' });
                    $('body').width($('body').width());
                    $(document).bind('scroll', function () {
                        window.scrollTo(0, 0);
                    });
                }
            });

        });

        function closeDialog() {

            //Could cause an infinite loop because of "on close handling"
            $("#divItemPrintData").dialog('close');

        }

        function openDialog(title, linkID) {
            debugger;
            $("#divSupplierItemList1").hide();
            $("#divSupplierItemList1").css("display", "none");
            $("#divItemPrintData").dialog("option", "title", "Pending Goods-IN List");
            $("#divItemPrintData").dialog('open');
            NProgress.start();
            blockDialog();


        }

        function unblockDialog() {
            $("#divItemPrintData").unblock();

            NProgress.done();

        }

        function blockDialog() {

            //block it to clean out the data
            $("#divItemPrintData").block({
                message: '<img src="<%=ResolveUrl("~") %>Images/async_inb.gif" />',
                css: { border: '0px' },
                fadeIn: 0,
                fadeOut: 0,
                overlayCSS: { backgroundColor: '#ffffff', opacity: 1 }
            });
        }

        function OpenPoLineItems() {
            debugger;
            var PoId = $("#<%=hifGRNPOHeaderID.ClientID %>").val();
            var InvoiceId = $("#<%=hifInvoiceId.ClientID %>").val();
            if (PoId != "" && InvoiceId != "") {
                //$("#divContainer").show();

            }

            else {
                alert("Please Select Po Number and Invoice #.");
                return;
            }

        }

        function SetTableSettings(data) {
            //console.log(data);
            polineitmes = data.Table;
            debugger;
            //$(document).ready(function () {
            $('.dataTables-example').DataTable({
                pageLength: 2,
                //searching: false,
                //paging: false,
                retrieve: true,
                columnDefs: [
                    { orderable: false, targets: -1 }
                ],
                dom: '<"html5buttons"B>lTfgitp',
                buttons: [
                    { extend: 'copy' },
                    { extend: 'csv' },
                    { extend: 'excel', title: 'ExampleFile' },
                    { extend: 'pdf', title: 'ExampleFile' },

                    {
                        extend: 'print',
                        customize: function (win) {
                            $(win.document.body).addClass('white-bg');
                            $(win.document.body).css('font-size', '10px');

                            $(win.document.body).find('table')
                                .addClass('compact')
                                .css('font-size', 'inherit');
                        }
                    }
                ]

            });

            ///});
        }


    </script>

    <script>
        //======================= Added by M.D.Prasad for Discrepency =========================//
        $(document).ready(function () {


            var Inboundid = new URL(window.location.href).searchParams.get("ibdid");
            debugger;
            if (Inboundid != null) {
                var data = "{ IbdId:  '" + Inboundid + "' }";
                InventraxAjax.AjaxResultExecute("InboundDetails.aspx/getDiscrepency", data, 'GetListOnSuccess', 'GetListOnError', null);
            }

        });

        function GetListOnSuccess(data) {
            debugger;
            var check = JSON.parse(data.Result).Table[0].Flag;
            if (check == 1) {
                $(".DiscrepencyHeader").css("pointer-events", "none");
                $(".Discrepency").css("display", "none");
            }
            else {
                $(".Discrepency").css("display", "block");
                $(".DiscrepencyHeader").css("pointer-events", "unset");
            }
        }
        //======================= Added by M.D.Prasad for Discrepency =========================//

    </script>

    <style type="text/css">
        .ChargesStatus {
            color: Steelblue;
            font-weight: bold;
            font-size: 15px;
        }

        /*#gvLightGrayNew_DataCellGridEdit input[type="text"]:disabled, input[type="text"][readonly] {
            padding:5px;
                margin-top: 13px;

        }*/

        .gvLightGrayNew input[readonly] {
            padding: 5px 5px 8px 0px !important;
        }

        input[type="text"] {
            border: 1px solid transparent;
            border-bottom: 1px solid #b3adad;
            margin-right: 5px;
            padding: 5px;
        }

        #MainContent_IBContent_gvActualVehicle {
        }


        .fixayy input, select {
            width: 92% !important;
        }



        /*#MainContent_IBContent_pnlDiscInfo input, select {
               width: 92% !important;
        }*/

        #gvLightGrayNew_DataCellGridEdit input, select {
            width: 92% !important;
        }

        .hideUnderLine .mdd-select-underline {
            display: none !important;
        }
    </style>

    <div class="dashed"> </div>
    <div ng-app="MyApp" ng-controller="createinbound">
    <div class="pagewidth">
       
 <!-- Globalization Tag is added for multilingual  -->
        <div>
            <div>
                <div  id="Loading" style="width:100%; height:100%; z-index:999; position:fixed; top:0; left:0; right:0; bottom:0; align-items:center; display:none; justify-content:center; background: #e0ddd8ba; ">
                                        <div style="align-self:center;" >
                                            <div class="spinner">
                                                <div class="bounce1"></div>
                                                <div class="bounce2"></div>
                                                <div class="bounce3"></div>
                                            </div>

                                        </div>                                  
                                    </div>

                <asp:UpdatePanel ID="updatepnl" runat="server">  
                    
                    <ContentTemplate>
                 <status class=""><asp:Label ID="lblInboundStatus" runat="server" CssClass="InboundStatus" ></asp:Label></status>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                <!-- Section 1 -->
                <div id="accordion1"  class="accordion">
                    <h3>1. Basic Data</h3>
                    <div>                         
                        <!-- Start Initiate Inbound Received-->                        
                            <asp:UpdateProgress ID="uprgBasicDetails" runat="server" AssociatedUpdatePanelID="upnlBasicDetails">
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
                            </asp:UpdateProgress>
                            <asp:UpdatePanel ID="upnlBasicDetails" runat="server" RenderMode="Inline" ViewStateMode="Enabled" ClientIDMode="Inherit" UpdateMode="Conditional">
                                <ContentTemplate>

                                    <div class="inputRightAlign">
                                        <div>
                                            <div>
                                                <asp:UpdatePanel runat="server" ID="upnlIBStatusMsg" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="lnkSaveInbound" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <asp:Label ID="lblStatusMessage" runat="server" CssClass="errorMsg" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>

                                            </div>
                                        </div>

                                        <div>
                         
                                             <div class="FormLabels" >
                                     
                                               <%-- <asp:Label ID="lblInboundStatus" runat="server" CssClass="InboundStatus" ></asp:Label>--%>
                                    
                                            </div>

                                        </div>
                                        <!---------- Row  1  STORES --------->
                                        <div class="row">
                                            <div class="FormLabels">

                                                <asp:UpdatePanel runat="server" ID="uPanelWHList" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                                    <ContentTemplate>
                                                        <asp:Label runat="server" ID="lblStoreStatus" CssClass="errorMsg"></asp:Label>

                                                        <%--<span class="SubHeading"><span class="errorMsg">* </span>Store(s) Associated</span>--%>
                                                        <br />
                                                        <div runat="server" id="divWHCheckBoxList" class="appendcheck" style="display:none !important;">
                                                            <!----  WH CheckBox List   ----->
                                                        </div>

                                                    </ContentTemplate>
                                                </asp:UpdatePanel>

                                            </div>
                                        </div>
                                        <!----------- Row 2 Store RefNo: ----->
                                        <div class="row">
                                            <div class="col m4 s4">
                                                    <div class="FormLabels">
                                                        <div class="">
                                                            <asp:UpdatePanel ID="uPanelStoreRefNo" UpdateMode="Conditional" runat="server" ChildrenAsTriggers="true">
                                                                <ContentTemplate>
                                                                    <div class=" hideUnderLine"> 
                                                                        <asp:TextBox ID="txtStrRefNo" runat="server" Enabled="false" EnableTheming="true" required="required" Text="New" />
                                                                        <label><%= GetGlobalResourceObject("Resource", "StoreRefNo")%> </label>
                                                                    <asp:ImageButton ID="btnGetnewStrRefNo" ToolTip="Generate Store Ref. No." runat="server" Width="20" OnClick="btnGetnewStrRefNo_Click" ImageUrl="~/Images/icon_newID.gif" Visible="false" />
                                                
                                                                    <asp:RequiredFieldValidator ID="rfvtxtStrRefNo" SetFocusOnError="true" ControlToValidate="txtStrRefNo" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" ValidationGroup="InitiateShipment" Enabled="false" Visible="false" />
                                                                    <%-- <label>Store Ref. No.</label>--%>
                                                        
                                                                    <%-- <asp:TextBox ID="txtStrRefNo" Width="166" runat="server" Enabled="false" EnableTheming="true" />
                                                                    <asp:ImageButton ID="btnGetnewStrRefNo" ToolTip="Generate Store Ref. No." runat="server" Width="20" OnClick="btnGetnewStrRefNo_Click" ImageUrl="~/Images/icon_newID.gif" Visible="false" />
                                                            --%>        </div></ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>

                                                        <div class="">
                                                            <asp:UpdatePanel UpdateMode="Conditional" ID="uPanelTest" runat="server">
                                                                <ContentTemplate>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="btnGetnewStrRefNo" EventName="Click" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                            </div>
                                
                                            <div class="col m4 s4">
                                                    <div class="flex">
                                                            <asp:TextBox ID="txtAccount" runat="server" SkinID="txt_Auto"  required="required"/>
                                                            <asp:HiddenField ID="hdnAccount" runat="server" Value="0" />
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2"  ControlToValidate="txtAccount" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" runat="server" ValidationGroup="InitiateShipment" />
                                                             <span class="errorMsg"></span>
                                                            <label> <%= GetGlobalResourceObject("Resource", "Account")%> </label>   
                                                                  
                                                    </div>
                                            </div>    
                                              <div class="col m4 s4">
                                                        <div class="FormLabels flex">
                                                
                                                            <asp:TextBox ID="txtWarehouse" runat="server" SkinID="txt_Auto"  required="required" ClientIDMode="Static"/>
                                                            <asp:HiddenField ID="hdnWarehouse" runat="server" Value="0" />
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3"  ControlToValidate="txtWarehouse" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" runat="server" ValidationGroup="InitiateShipment" />
                                                            <label>  <%= GetGlobalResourceObject("Resource", "Warehouse")%> </label>

                                                </div>
                                            </div>
                                         
                                        </div>
                                        <!------------- Row 3 CHARGES -------->

                                        <div class="row">
                                              
                                               <div class="col m4 s4">
                                                        <div class="flex">

                                                            <asp:TextBox ID="txtTenant" runat="server" SkinID="txt_Auto"  required="required" ClientIDMode="Static"/>
                                                            <asp:HiddenField ID="hidTenantID" runat="server" Value="0" />                                            

                                                            <asp:RequiredFieldValidator ID="rfvTenant"  ControlToValidate="txtTenant" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" runat="server" ValidationGroup="InitiateShipment" />
                                                            <span class="errorMsg">* </span><label> <%= GetGlobalResourceObject("Resource", "Tenant")%> </label>




                                               
                                          
                                                            <%--<asp:TextBox ID="atcShipmentType" runat="server" SkinID="txt_Auto" required="required" ></asp:TextBox>
                                                            <asp:HiddenField runat="server" ID="hifShipmentType" />--%>
                                            
                                                    </div>
                                            </div>
                                                <div class="col m4 s4">
                                                        <div class="FormLabels flex">
                                                
                                                            <asp:TextBox ID="txtDocRcvDate" CssClass="flex_Error" runat="server" required="required" ClientIDMode="Static" />
                                                            <asp:RequiredFieldValidator ID="rfvtxtDocRcvDate" SetFocusOnError="true" ControlToValidate="txtDocRcvDate" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2 " ErrorMessage=" * " runat="server" ValidationGroup="InitiateShipment" />
                                                            <label>  <%= GetGlobalResourceObject("Resource", "DocumentReceivedDate")%></label>          

                                                </div>
                                            </div>

                                                <div class="col m4 s4">
                                                        <div class="FormLabels flex">
                                                
                                         

                                                </div>
                                            </div>
                                        </div>




                                        <div class="row">
                                            <div class="col m4 s4" style="display: none;">
                                                <div  class="FormLabels" style="display: none;">
                                                    <asp:Panel ID="Panel2" runat="server" CssClass="FormLabels" Width="65%" GroupingText="Charges">
                                                        <asp:RadioButtonList ID="rblChargesRequired" RepeatDirection="Horizontal" runat="server" CssClass="opt_slim_req" CellPadding="3" CellSpacing="2">
                                                            <asp:ListItem Text="Required" Value="1" Selected="True" />
                                                            <asp:ListItem Text="Not required" Value="2" />
                                                            <asp:ListItem Text="To be estimated" Value="3" />
                                                        </asp:RadioButtonList>
                                                    </asp:Panel>
                                                </div>
                                            </div>

                                            <div class="col m4 s4">
                                                        <div class="FormLabels flex">
                                                
                                                            <asp:TextBox ID="atcShipmentType" runat="server" SkinID="txt_Auto" required="required" ClientIDMode="Static"></asp:TextBox>
                                                            <asp:HiddenField runat="server" ID="hifShipmentType" />
                                                            <asp:RequiredFieldValidator ID="rfvatcShipmentType"  Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ControlToValidate="atcShipmentType" runat="server" InitialValue="" ValidationGroup="InitiateShipment" />
                                                            <span class="errorMsg">* </span> <label> <%= GetGlobalResourceObject("Resource", "ShipmentType")%></label>
                                                </div>
                                            </div>
                                
                                            <div class="col m4 s4">
                                                    <div class="FormLabels flex">

                                         
                                                            <%--   <span class="errorMsg">* </span> <label>Shipment Type</label>--%>

                                                        <%--     <asp:RequiredFieldValidator ID="rfvatcSupplier" SetFocusOnError="true" ControlToValidate="atcSupplier" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" runat="server" ValidationGroup="InitiateShipment" />
                                                            <span class="errorMsg">* </span><label>Supplier</label>--%>
                                           
                                                            <asp:TextBox ID="atcSupplier" runat="server" SkinID="txt_Auto" required="required" ClientIDMode="Static"/>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1"  Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ControlToValidate="atcSupplier" runat="server" InitialValue="" ValidationGroup="InitiateShipment" />
                                                            <span class="errorMsg">* </span><label>  <%= GetGlobalResourceObject("Resource", "Supplier")%> </label>
                                                            <asp:HiddenField ID="hifSupplier" runat="server" />
                                                            <asp:HiddenField ID="hifMaterialId" runat="server" />
                                                            <asp:HiddenField ID="hifPoDetailsid" runat="server" />
                                                            <asp:HiddenField ID="hifPoHeaderId" runat="server" />
                                                            <asp:HiddenField ID="hifInvoiceId" runat="server" />
                                                            <asp:HiddenField ID="hifgrnpolinenumber" runat="server" />
                                                            <%--<asp:RequiredFieldValidator ID="rfvatcSupplier" SetFocusOnError="true" ControlToValidate="atcSupplier" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" runat="server" ValidationGroup="InitiateShipment" require="required"/>--%>
                                               
                                                </div>
                                            </div>
                                        </div>
                                        <!------------- Row  5 Supplier  ----->
                                        <div id="trConsignmentNoteType" >
                                            <div class="row">
                                            <div class="col m4 s4">
                                                <div class="FormLabels">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                                        <ContentTemplate>
                                                            <div class="flex">
                                                       
                                                                    <asp:DropDownList ID="ddlConsignmentNoteType" runat="server" required="required"/>
                                                                <%-- <label>Consignment Note Type</label>--%>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <div class="col m4 s4">
                                                <div class="FormLabels">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                                        <ContentTemplate>
                                                            <div class="flex">
                                                                    <asp:TextBox ID="txtConsignmentNoteTypeValue" runat="server"  required="required"/>
                                                                    
                                                                <label id="lblConsignmentNoteType" runat="server"> Consignment Number</label>
                                               
                                                                    <%--<asp:TextBox ID="txtConsignmentNoteTypeValue" runat="server" />--%>
                                                 
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <div class="col m4 s4">
                                                <div class="FormLabels">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                                        <ContentTemplate>
                                                            <div class="flex">
                                                                    <asp:TextBox ID="txtConsignmentNoteTypeDate" runat="server" required="required"/>
                                                                   
                                                                <label id="lblConsignmentNoteTypeDate" runat="server"> Consignment Date</label>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div></div>

                                
                                        </div>
                                        <!------------ Row  6 BL ------------->
                                        <div class="row">
                                            <div class="col m4 s4">
                                                <div class="FormLabels flex">
                                                            <asp:TextBox ID="txtNoofPackages" MaxLength="4" onKeyPress="return checkNum(event)" runat="server" required="required"/>
                                                            <label><%= GetGlobalResourceObject("Resource", "NoofPackages")%></label>
                                                            <%--<asp:RequiredFieldValidator ID="rfvtxtNoofPackages" Enabled="false" SetFocusOnError="true" ControlToValidate="txtNoofPackages" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" ValidationGroup="InitiateShipment"  required="required"/>--%>
                                           
                                                </div>
                                            </div>
                                            <div class="col m4 s4">
                                                <div class="FormLabels flex">
                            <%--                                                <asp:RequiredFieldValidator ID="rfvtxtGrossWeight" SetFocusOnError="true" ControlToValidate="txtGrossWeight" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" ValidationGroup="InitiateShipment" Enabled="false" />
                                                            <label>Gross Weight (Kgs)</label> --%>
                                          
                                                           <%-- <asp:RegularExpressionValidator ID="revtxtGrossweight" runat="server" ValidationExpression="^\d*[0-9](|.\d*[0-9]|,\d*[0-9])?$" ControlToValidate="txtGrossWeight" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage="Invalid gross weight " ValidationGroup="InitiateShipment" />--%>
                                                            <asp:TextBox ID="txtGrossWeight" onKeyPress="return checkDec(this,event)" runat="server" ValidationGroup="InitiateShipment" required="required"/>
                                        
                                                           <%-- <asp:RequiredFieldValidator ID="rfvtxtGrossWeight" SetFocusOnError="true" ControlToValidate="txtGrossWeight" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" ValidationGroup="InitiateShipment" Enabled="false" />--%>
                                                            <label> <%= GetGlobalResourceObject("Resource", "GrossWeightKgs")%></label> </label> 
                                        
                                                </div>
                                            </div>
                                            <div class="col m4 s4">
                                                <div class="FormLabels flex">
                                                        <%--<asp:RegularExpressionValidator ID="revtxtCBM" runat="server" ValidationExpression="^\d*[0-9](|.\d*[0-9]|,\d*[0-9])?$" ControlToValidate="txtCBM" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage="Invalid CBM" ValidationGroup="InitiateShipment" />--%>
                                                        <asp:TextBox ID="txtCBM" onKeyPress="return checkDec(this,event)" runat="server" ValidationGroup="InitiateShipment" required="required"/>

                                                        <%--<asp:RequiredFieldValidator ID="rfvtxtCBM" Enabled="false" SetFocusOnError="true" ControlToValidate="txtCBM" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" ValidationGroup="InitiateShipment" />--%>
                                                        <label> <%= GetGlobalResourceObject("Resource", "CBM")%></label>
                                                </div>
                                            </div>
                                        </div>
                          
                                        <!------ Row  7 Clearance Details ---->
                                        <asp:UpdatePanel runat="server" ID="UpdatePanel5" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                            <ContentTemplate>
                                                 <div id="trClearanceCompany" class="row">
                                            <div class="col m4 s4">
                                                <div class="flex">
                                                        <asp:TextBox ID="atcClearenceCompany" SkinID="txt_Auto" runat="server" required="required"></asp:TextBox>
                                                        <asp:HiddenField ID="hifClearenceCompany" runat="server"/>
                                                        <label> <%= GetGlobalResourceObject("Resource", "ClearanceCompany")%> </label>
                                                </div>
                                            </div>
                                            <div class="col m4 s4">
                                                <div class="flex">
                                                        <asp:TextBox ID="txtClearenceCompanyInvoice" runat="server" required="required"/>
                                                    <label> <%= GetGlobalResourceObject("Resource", "ClearanceInvoice")%> </label>
                                                    </div>   
                                            </div>
                                            <div class="col m4 s4">
                                                <div class="flex">
                                                        <asp:TextBox ID="txtClearenceInvoiceDate" CssClass="" runat="server" required="required"/>
                                                        <label> <%= GetGlobalResourceObject("Resource", "InvoiceDate")%></label>
                                                    </div>
                                        
                                            </div>
                                            <div class="col m4 s4">
                                                <div class="flex">
                                                        <asp:TextBox ID="txtClearenceCompanyAmount" onKeyPress="return checkDec(event)" CssClass="txt_slim" runat="server" required="required"/>
                                                        <label> <%= GetGlobalResourceObject("Resource", "ClearanceAmount")%> (<asp:Literal runat="server" ID="ltClCurrency" />)</label>
                                                </div>
                                            </div>
                                        </div>
                                            </ContentTemplate>
                                            </asp:UpdatePanel>
                                       
                                        <!------ Row  8 FreightCompany Details ---->
                                        <div class="row">
                                            <div class="col m4 s4">
                                                <div class="FormLabels flex">
                                               
                                                    <%--   <label>Freight Company</label>--%>
                                                            <asp:DropDownList ID="ddlFreightCompany" runat="server" required="required"></asp:DropDownList>
                                        
                                        
                                                </div>
                                            </div>
                                            <div class="col m4 s4">
                                                            <div class="flex">
                                                                    <asp:TextBox ID="txtFreightInvoice" runat="server" required="required"/>
                                                                    <label> <%= GetGlobalResourceObject("Resource", "FreightInvoice")%> </label>
                                                            </div>
                                            </div>

                                                <div class="col m4">
                                                            <div class="FormLabels  flex">
                                                                <asp:TextBox ID="txtFreightInvoiceDate"  runat="server" required="required"/>
                                                                    <label> <%= GetGlobalResourceObject("Resource", "InvoiceDate")%></label>
                                                            </div>
                                                </div>

                                            <div class="col m4 s4">
                                                <div class="FormLabels flex">
                                                    <%--<label>Freight Amount(<asp:Literal runat="server" ID="ltFrCurrency" />)</label>--%>
                                                    <asp:TextBox ID="txtFreightAmount" onKeyPress="return checkDec(this,event)" CssClass="" runat="server" required="required"/>
                                                    <label> <%= GetGlobalResourceObject("Resource", "FreightAmount")%>(<asp:Literal runat="server" ID="ltFrCurrency" />)</label>
                                                </div>
                                            </div>
                                        </div>
                                        <!------------- Row  9 Remarks ------->
                                        <%--<div class="row">
                                            <div class="col m4 s4">
                                                <div  class="FormLabels flex">
                                                        <div class="f-flex"><label>Remarks</label></div>
                                                    <div class="f-flex form"><asp:TextBox ID="txtRemarks_Ini"   TextMode="MultiLine" CssClass="txt_slim" runat="server" /></div>
                                                </div>
                                                </div>
                                        </div>--%>
                                        <!------------- Row  10  Attachment--->
                                        <hr />
                                        <div class="">
                                            <div class="">
                                                    <div class="row">
                                                        <p><%= GetGlobalResourceObject("Resource", "PrioritySetLevelReceivedDateTime")%></p><br />
                                                            <div class="FormLabels">
                                                    
                                                                        <div class="col m4 s4">
                                                                                <div  class=" flex ">
                                                                                        <asp:DropDownList ID="ddlPriorityLevel" runat="server" CssClass="txt_slim" required="required"/> <label>Level</label>
                                                                                </div>
                                                                        </div>
                                                                        <div class="col m4 s4">
                                                                            <div  class=" flex">
                                                                                    <asp:TextBox ID="txtPriorityDate" CssClass="txt_slim" runat="server" required="required"/>
                                                                                    <label><%= GetGlobalResourceObject("Resource", "Date")%></label>
                                                                            </div>
                                                                            </div>
                                                                        <div class="col m4 s4">
                                                                
                                                                                <div class="flex">
                                                                                <asp:TextBox ID="txtTimeEntry" CssClass="txt_slim" runat="server" placeholder="Time"/>
                                                                                  
                                                                                </div>
                                                                        </div>
                                                
                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <div class="row">
                                        
                                                            <div class="col m4 s4">
                                                   
                                                                <div class="flex">
                                                                    <asp:UpdatePanel ChildrenAsTriggers="true" ID="upnlFileAttchments" UpdateMode="Conditional" Cssclass="flex__" runat="server">

                                                                        <Triggers>
                                                                            <asp:PostBackTrigger ControlID="lnkSaveInbound" />
                                                                        </Triggers>
                                                                        <ContentTemplate>
                                                                            <asp:FileUpload ID="fucInboundDeliveryNote" runat="server" Cssclass="file" required=""/>
                                                                            <div runat="server" id="divDNAtachment"></div>
                                                                                <span> <%= GetGlobalResourceObject("Resource", "AttachShipmentDoc")%> <asp:Literal ID="Literal3" runat="server"  Text='File Type ( .pdf )' /></span><br />
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                            </div>     
                                                                <div class="col m4 s4">
                                                                    <div  class=" flex">
                                                                        <asp:TextBox ID="txtRemarks_Ini"   TextMode="MultiLine" CssClass="txt_slim" runat="server"  required="required"/>
                                                                            <label> <%= GetGlobalResourceObject("Resource", "Remarks")%></label>
                                                                    </div>
                                                                    </div>
                                                    </div>
                                                    <div class="row mb0">
                                                            <div class="col m4 s4  offset-m8 offset-s8 p0">
                                                                <div class="right">                                                        
                                                                    <asp:UpdatePanel ChildrenAsTriggers="true" ID="UpdatePanel4" UpdateMode="Conditional" runat="server">
                                                                        <Triggers>
                                                                            <asp:PostBackTrigger ControlID="lnkSaveInbound" />
                                                                            <asp:PostBackTrigger ControlID="lnkCancel" />
                                                                        </Triggers>
                                                                        <ContentTemplate>
                                                                            <asp:LinkButton ID="lnkCancel" runat="server" OnClick="lnkCancel_Click" CssClass="btn btn-primary" />
                                                                            <asp:LinkButton ID="lnkSaveInbound" CausesValidation="True" ValidationGroup="InitiateShipment" runat="server" OnClick="lnkSaveInbound_Click" CssClass="btn btn-primary"> </asp:LinkButton>
                                                                                <%-- <asp:ValidationSummary ID="valSummary" DisplayMode="List" runat="server" ShowMessageBox="true" ShowSummary="false" EnableClientScript="true" ValidationGroup="InitiateShipment" ForeColor="red" Font-Bold="true" />--%>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                            </div>
                                                    </div>
                                
                                            </div>
                                </div>
                        
                            </div>

                                </ContentTemplate>
                            </asp:UpdatePanel>
                        <!-- End Initiate Inbound Received    -->                        
                    </div>
                </div>
                
                <br />
                <div id="accordion2" class="accordion">
                    <h3> <%= GetGlobalResourceObject("Resource", "InwardOrderInvoiceDetails")%> </h3>
                    <div>
                        <!-- Start PO / Invoice Grid  -->
                        <!---- //Here have to add "PODetails" grid with coloums are PONumber,Supplier Name,PODate,RequestedBy,Total No.of Line Items,Deapartment & Division //   ------->
                        
                            <asp:UpdateProgress ID="uprgPOInvDetails" runat="server" AssociatedUpdatePanelID="upnlPOInvDetails">
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
                            </asp:UpdateProgress>
                            <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" ID="upnlPOInvDetails" UpdateMode="Always">
                                <ContentTemplate>
                                    <asp:Panel runat="server" ID="pnlPOInvDetails" Visible="false" HorizontalAlign="Center">
                                        <div>
                                            <div class="row">
                                                <div class="right">
                                                    <asp:LinkButton runat="server" ID="lnkAddNewPoItems" Font-Underline="false" OnClick="lnkAddNewPoItems_Click" CssClass="btn btn-sm btn-primary">
                                                     <%= GetGlobalResourceObject("Resource", "AddOrderInvoice")%>  <%=MRLWMSC21Common.CommonLogic.btnfaNew %>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                            <div>
                                                <div class="FormLabels">
                                                    <asp:Label runat="server" ID="lblPoInvStatus"></asp:Label>
                                                </div>
                                            </div>
                                            <div>
                                                <div>
                                                    <br />
                                                    <asp:Panel runat="server" ID="pnlPoInvDetalis2" HorizontalAlign="Center">

                                                        <asp:GridView ID="gvPOInvoice" HorizontalAlign="Center" SkinID="gvLightGrayNew" runat="server" AllowPaging="true"
                                                            AllowSorting="false"
                                                            OnPageIndexChanging="gvPOInvoice_PageIndexChanging"
                                                            OnRowDataBound="gvPOInvoice_RowDataBound"
                                                            OnRowCommand="gvPOInvoice_RowCommand"
                                                            OnRowUpdating="gvPOInvoice_RowUpdating"
                                                            OnRowCancelingEdit="gvPOInvoice_RowCancelingEdit"
                                                            OnRowEditing="gvPOInvoice_RowEditing"
                                                            PagerStyle-HorizontalAlign="Right"
                                                            PageSize="10"
                                                            CellSpacing="2">

                                                            <Columns>


                                                                <asp:TemplateField HeaderText= "<%$Resources:Resource,OrderNumber%>" ItemStyle-Width="130">
                                                                    <ItemTemplate>
                                                                        <img src="../Images/redarrowleft.gif" />
                                                                        <asp:Literal ID="ltHidInbound_POID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "PONumber") %>' />
                                                                        <asp:HyperLink ID="HyperLink2" Text='<%# DataBinder.Eval(Container.DataItem, "PONumber") %>' NavigateUrl='<%# String.Format("../mOrders/PODetailsInfo.aspx?poid={0}",DataBinder.Eval(Container.DataItem, "POHeaderID").ToString()) %>' Font-Underline="false" runat="server"></asp:HyperLink>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <div class="gridInput">
                                                                           <%-- <asp:RequiredFieldValidator ID="rfvtxtPONumber" runat="server" ControlToValidate="txtPONumber" Display="Dynamic" ErrorMessage=" * " />--%>
                                                                        <asp:Literal ID="ltHidInbound_POID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "PONumber") %>' />
                                                                            <asp:TextBox ID="txtPONumber" runat="server" EnableTheming="false" CssClass="vDynaPONumber" Text='<%# DataBinder.Eval(Container.DataItem, "PONumber") %>' ></asp:TextBox>
                                                                            <span class="errorMsg">* </span>
                                                                        </div>
                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText= "<%$Resources:Resource,InvoiceNumber%>" ItemStyle-Width="130">
                                                                    <ItemTemplate>

                                                                        <asp:Literal ID="ltHidInbound_SupplierInvoiceID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Inbound_SupplierInvoiceID") %>' />

                                                                        <img src="../Images/redarrowleft.gif" />
                                                                        <asp:LinkButton Font-Underline="false" ID="lnkEditPOItem" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "InvoiceNumber") %>' PostBackUrl='<%# String.Format("../mOrders/PODetailsInfo.aspx?poid={0}",DataBinder.Eval(Container.DataItem, "POHeaderID").ToString()) %>' />
                                                                        <asp:HyperLink Text='<%# DataBinder.Eval(Container.DataItem, "InvoiceNumber") %>' NavigateUrl='<%# String.Format("../mOrders/PODetailsInfo.aspx?poid={0}",DataBinder.Eval(Container.DataItem, "POHeaderID").ToString()) %>' Font-Underline="false" runat="server"></asp:HyperLink>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                       <div class="gridInput">
<%--                                                                           <asp:RequiredFieldValidator ID="rfvtxtInvoiceNumber" runat="server" ControlToValidate="txtInvoiceNumber" Display="Dynamic" ErrorMessage=" * " />--%>
                                                                        <asp:Literal ID="ltHidInbound_SupplierInvoiceID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Inbound_SupplierInvoiceID") %>' />
                                                                        <asp:TextBox ID="txtInvoiceNumber" runat="server" EnableTheming="false" CssClass="DynaInvNumber" Text='<%# DataBinder.Eval(Container.DataItem, "InvoiceNumber") %>' ></asp:TextBox>
                                                                            <span class="errorMsg">* </span>
                                                                        <asp:HiddenField ClientIDMode="Static" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "SupplierInvoiceID").ToString() %>' ID="hifInvNumberID" /></div>

                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>




                                                                <asp:TemplateField HeaderText= "<%$Resources:Resource,OrderDate%>">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="ltPODate" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "PODate","{0: dd-MMM-yyyy}") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText= "<%$Resources:Resource,InvoiceValue%>" >
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="ltInvoiceValue" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "InvoiceValue") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText= "<%$Resources:Resource,OrderValue%>">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="ltPOValue" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "POValue") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText= "<%$Resources:Resource,InvCurrency%>">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="ltInvCurrency" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Currency") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText= "<%$Resources:Resource,RequestedBy%>" >
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="ltRequestedBy" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RequestedBy") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText= "<%$Resources:Resource,LineItemsCount%>" >
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="ltLineItemsCount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LineItemsCount") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="<%$Resources:Resource,Delete%>"  ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint">
                                                                    <HeaderTemplate>
                                                                        <nobr>Delete</nobr>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <%--<div class="checkbox"><asp:CheckBox ID="chkIsDeletePOInvItems" runat="server" /><label for=""></label></div>--%>
                                                                        <asp:CheckBox ID="chkIsDeletePOInvItems" runat="server" />

                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                    </EditItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:LinkButton ID="lnkDeletePOInvItem" Font-Underline="false" CssClass="blueLink" runat="server" Text="<nobr><inv-icon is='delete' position='left' help='Delete'></inv-icon></nobr>" OnClick="lnkDeletePOInvItem_Click" OnClientClick="return confirm('Are you sure want to delete?')" />
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField ItemStyle-Font-Underline="false" ControlStyle-Font-Underline="false" Visible="false" DataField="EditName" ReadOnly="true" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint" />
                                                                <asp:CommandField ItemStyle-Font-Underline="false" ItemStyle-Width="30" ButtonType="Link" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint" ItemStyle-HorizontalAlign="left" CancelImageUrl="Images/cancel.gif" EditImageUrl="/Images/edit.gif" EditText="<nobr><i class='material-icons ss'>mode_edit</i></nobr>" ShowEditButton="True" UpdateImageUrl="/Images/save.gif" ControlStyle-Font-Underline="false" />
                                                            </Columns>
                                                        </asp:GridView>

                                                        <br />
                                                    </asp:Panel>
                                                </div>
                                            </div>
                                        </div>




                                    </asp:Panel>

                                    <asp:HiddenField runat="server" ID="hifPONumberID" />

                                </ContentTemplate>
                            </asp:UpdatePanel>

                        <!-- End PO / Invoice Grid  -->
                    </div>

                </div>
                
                <br />
                <div id="accordion3" class="accordion">

                    <h3>   <%= GetGlobalResourceObject("Resource", "ShipmentExpectedDetails")%> </h3>
                    <div>

                        <!-- Start Shipment Expected Details -->

                        
                            <asp:UpdateProgress ID="uprgShipmentDetails" runat="server" AssociatedUpdatePanelID="upnlShipmentDetails">
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
                            </asp:UpdateProgress>
                            <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" ID="upnlShipmentDetails" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlShipmentDetails" runat="server" Visible="false" >

                                        <div >

                                            <div>
                                                <div >
                                                    <asp:Label ID="lblShipmentDetailsStatus" runat="server" CssClass="ErrorMsg" />
                                        
                                                </div>
                                            </div>

                                            <div class="flex_between_column">
                                                <div class="flex__" style="justify-content: space-between;">
                                                <div class="FormLabels flex__" style="    width: 400px;">
                                   
                                                    <div class="flex upl">
                                                    <asp:RequiredFieldValidator ID="rfvtxtShipmentExpectedDate" SetFocusOnError="true" ControlToValidate="txtShipmentExpectedDate" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" ValidationGroup="ShipmentDetails" />
                                       
                                      
                                                    <asp:TextBox ID="txtShipmentExpectedDate" Width="200" runat="server" required="required"/>
                                                    <label>  <%= GetGlobalResourceObject("Resource", "ShipmentExpectedDate")%></label>
                                                         <span class="errorMsg">* </span>
                                                    </div>
                                                   <div>
                                                        <div><asp:LinkButton ID="lnkUpdateShipmentDetails" CausesValidation="True" ValidationGroup="ShipmentDetails" runat="server" OnClientClick="Panel_ValidationActive=true;" OnClick="lnkUpdateShipmentDetails_Click" CssClass="btn btn-sm btn-primary">
                                                          <%= GetGlobalResourceObject("Resource", "Update")%>  <%=MRLWMSC21Common.CommonLogic.btnfaUpdate %>
                                                        </asp:LinkButton></div>
                                                    </div>
                                                </div>

                                                 <div class="flex__ top-right" style="width:fit-content;display:none">
                                                  
                                                               <p style="width:200px;">   <%= GetGlobalResourceObject("Resource", "ProjectedVehicleDetails")%> </p>
                                                               &nbsp;&nbsp;&nbsp;
                                                                <asp:LinkButton ID="lnkPrjVehicle" Visible="true" OnClick="lnkPrjVehicle_Click" runat="server" CssClass="btn btn-sm btn-primary">
                                                               <%= GetGlobalResourceObject("Resource", "AddProjectedVehicle")%> <%=MRLWMSC21Common.CommonLogic.btnfaNew %>
                                                                </asp:LinkButton>
                                                                <br />
                                                            </div>
                                                  </div>
                                 
                                                <div class="FormLabels flex-grow">
                                                    <!-- Start Projected Vehicle Grid -->


                                                    <div>

                                                        <div>
                                                           <%-- <div class="flex__ top-right" style="width:fit-content;">
                                                  
                                                               <p style="width:200px;"> Projected  Vehicle  Details</p>
                                                               &nbsp;&nbsp;&nbsp;
                                                                <asp:LinkButton ID="lnkPrjVehicle" Visible="true" OnClick="lnkPrjVehicle_Click" runat="server" CssClass="btn btn-sm btn-primary">
                                                            Add Projected Vehicle <%=MRLWMSC21Common.CommonLogic.btnfaNew %>
                                                                </asp:LinkButton>
                                                                <br />
                                                            </div>--%>
                                                        </div>
                                                        <div>
                                                            <div >
                                                                <asp:Panel runat="server" ID="pnlProjectedVehicleGrid" HorizontalAlign="left">
                                                                    <asp:Label runat="server" ID="ltgvProjectedVehicleStatus"></asp:Label>
                                                                    <asp:Label ID="lblPVStatus" runat="server"></asp:Label>
                                                                    <br />
                                                                    <asp:GridView ID="gvProjectedVehicle" SkinID="gvLightGrayNew" runat="server" Width="300px" CellPadding="4"
                                                                        AllowPaging="false"
                                                                        AllowSorting="false"
                                                                        OnRowDataBound="gvProjectedVehicle_RowDataBound"
                                                                        OnRowCommand="gvProjectedVehicle_RowCommand"
                                                                        OnRowUpdating="gvProjectedVehicle_RowUpdating"
                                                                        OnRowCancelingEdit="gvProjectedVehicle_RowCancelingEdit"
                                                                        OnRowEditing="gvProjectedVehicle_RowEditing"
                                                                        PagerStyle-HorizontalAlign="Right"
                                                                        CellSpacing="0">

                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText= "<%$Resources:Resource,ProjectedVehicle%>" >
                                                                                <ItemTemplate>
                                                                                    <asp:Literal ID="ltProjectedVehicle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "EquipmentName") %>' />
                                                                                    <asp:Literal ID="lthidProjectedVehicleID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ProjectedEquipmentID") %>' />
                                                                                </ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:RequiredFieldValidator ID="rfvddlEquipmentType" SetFocusOnError="true" ControlToValidate="ddlEquipmentType" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" InitialValue="0" />
                                                                                    <asp:DropDownList ID="ddlEquipmentType" Width="100" runat="server"></asp:DropDownList>
                                                                                    <asp:Literal runat="server" Visible="false" ID="ltEquipmentID" Text='<%# DataBinder.Eval(Container.DataItem, "EquipmentID") %>'></asp:Literal>
                                                                                    <asp:Literal ID="lthidProjectedVehicleID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ProjectedEquipmentID") %>' />
                                                                                </EditItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText= "<%$Resources:Resource,ProjectedValue%>" ItemStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:Literal ID="ltProjectedValue" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ProjectedValue") %>' />
                                                                                </ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:RequiredFieldValidator ID="rfvtxtProjectedValue" SetFocusOnError="true" ControlToValidate="txtProjectedValue" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" />
                                                                                    <asp:TextBox ID="txtProjectedValue" Width="80" onKeyPress="return checkNum(event)" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ProjectedValue") %>'></asp:TextBox>
                                                                                </EditItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText= "<%$Resources:Resource,Delete%>" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint">
                                                                                <HeaderTemplate>
                                                                                    <nobr> <%= GetGlobalResourceObject("Resource", "Delete")%> </nobr>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="chkIsDeleteRFItem" runat="server" />
                                                                                </ItemTemplate>
                                                                                <EditItemTemplate></EditItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:LinkButton ID="lnkDeleteRFItem" Font-Underline="false" CssClass="blueLink" runat="server" Text="<nobr> <i class='material-icons ss'>delete</i></nobr>" OnClick="lnkDeleteRFItem_Click" OnClientClick="return confirm('Are you sure want to delete selected items ?')" />
                                                                                </FooterTemplate>
                                                                            </asp:TemplateField>

                                                                            <asp:BoundField ItemStyle-Font-Underline="false" Visible="false" DataField="EditName" ReadOnly="true" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint" />

                                                                            <asp:CommandField ItemStyle-Font-Underline="false" ControlStyle-Font-Underline="false" ItemStyle-Width="30" ButtonType="Link" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint" ItemStyle-HorizontalAlign="left" CancelImageUrl="/Images/cancel.gif" EditImageUrl="/Images/edit.gif" EditText="<nobr> <i class='material-icons ss'>mode_edit</i></nobr>" ShowEditButton="True" UpdateImageUrl="/Images/save.gif" />
                                                                        </Columns>

                                                                    </asp:GridView>
                                                                </asp:Panel>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <!-- End Projected Vehicle Grid -->
                                                </div>
                                            </div>
                                        </div>

                                    </asp:Panel>
                                </ContentTemplate>

                                 <%-- Triggers added by lalitha on 28/02/2019--%>
                                 <Triggers>
                                         <asp:PostBackTrigger ControlID="lnkUpdateShipmentDetails" />
                                     <%--<asp:AsyncPostBackTrigger ControlID="lnkUpdateShipmentDetails" EventName="Click" />--%>
                                    </Triggers>
                            </asp:UpdatePanel>

                        <!-- End Shipment Expected Details -->
                    </div>

                </div>
                
                <br />
                <div id="accordion10" class="accordion">

                    <h3> <%= GetGlobalResourceObject("Resource", "ReceivingDockManagement")%> </h3>
                    <div>
                        
                            <asp:UpdateProgress ID="uprgRecvDockMgmt" runat="server" AssociatedUpdatePanelID="upnlRcvDockMgmt">
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
                            </asp:UpdateProgress>
                            <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" ID="upnlRcvDockMgmt" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <!-- Start New Panel -->


                                    <asp:Panel ID="NewPanelVisible" runat="server" Visible="false" >

                                        <div >

                                            <div>
                                                <div >
                                                    <%--<asp:Label ID="Label1" runat="server" CssClass="ErrorMsg" />--%>
                                        
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col m3">
                                                  <%--  <div class=" FormLabels flex">--%>
                                         
                                                              <%--  <asp:RequiredFieldValidator ID="rfvdock" SetFocusOnError="true" ControlToValidate="txtdock" Display="Dynamic" EnableClientScript="true" ValidationGroup="dockDetails"  CssClass="ErrorAlert2" runat="server" />
                                               
                                                            <%-- <label>Dock</label> --%>
                                          
<%--                                                            <asp:TextBox ID="txtdock" runat="server" SkinID="txt_Auto"   Value="Dock001" required="required"/>
                                                            <label>  <%= GetGlobalResourceObject("Resource", "Dock")%></label> 
                                                            <%--<asp:Literal ID="ltDock" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Dock") %>' />--%>
                                                           <%-- <span class="errorMsg"> </span>
                                                            <asp:HiddenField ID="hdfdock" runat="server" Value="0" />--%>
                                                        <div class="flex">
                                                                <asp:TextBox runat="server"  ID="txtdock" required=""  />
                                                                <asp:HiddenField runat="server" ID="hdfdock"  />

                                                                <asp:RequiredFieldValidator ID="rfvdock" runat="server" ValidationGroup="save" ControlToValidate="txtdock" Display="Dynamic" />
                                                                <%-- <label><span style="color:red;margin-left:-0.3em"></span>Order Type<asp:Literal  runat="server" ID="ltPOType"/></label> --%>
                                                                <label><span style="color: red; margin-left: -0.3em"></span><%= GetGlobalResourceObject("Resource", "Dock")%><asp:Literal runat="server" ID="ltDockType" /></label>
                                                            </div>
                                                    <%--</div>--%>
                                                </div>
                                                <div class="col m3">
                                                    <div class="FormLabels  flex">
                                             <asp:RequiredFieldValidator ID="rfvVehicle" SetFocusOnError="true" ControlToValidate="txtVehicleNo" Display="Dynamic" EnableClientScript="true" ValidationGroup="dockDetails"  CssClass="ErrorAlert2" runat="server" />
                                                            <asp:TextBox ID="txtVehicleNo" runat="server"  Value="Dummy" required="required"/>
                                                         
                                                            <label>  <%= GetGlobalResourceObject("Resource", "VehicleRegNo")%>  </label>
                                                        <span class="errorMsg"> </span>
                                                    </div>
                                                </div>
                                                <div class="col m3">
                                                    <div class="FormLabels flex">
                                                         <asp:RequiredFieldValidator ID="rfvDriverName" SetFocusOnError="true" ControlToValidate="txtdriver" Display="Dynamic" EnableClientScript="true" ValidationGroup="dockDetails"  CssClass="ErrorAlert2" runat="server" />
                                                            <asp:TextBox ID="txtdriver" runat="server" Value="Dummy"  required="required"/>
                                                            <label><%= GetGlobalResourceObject("Resource", "DriverName")%> </label>
                                                         <span class="errorMsg"> </span>
                                                    </div>
                                                </div>

                                               
                                                    <div class="col m3">
                                                        <gap></gap>
                                                        <flex end><asp:LinkButton ID="lnkdockupdate" runat="server" CausesValidation="True" CssClass="btn btn-sm right btn-primary" OnClick="lnkdockupdate_Click" ValidationGroup="dockdtails">
                                                             <%= GetGlobalResourceObject("Resource", "Save")%>   <%=MRLWMSC21Common.CommonLogic.btnfaUpdate %>
                                                        </asp:LinkButton></flex>
                                                        </div>
                                                </div>
                                                <div class="row">
                                                    <asp:GridView ID="GridDockVeh" runat="server" AutoGenerateColumns="False"   
                                                        BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px"   
                                                        CellPadding="4" >   
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="DockID" Visible ="false">  
                                                               
                                                                <ItemTemplate>  
                                                                    <asp:Label ID="LabelDOCKID" runat="server" Text='<%# Eval("DockID") %>' Visible ="false"></asp:Label>  
                                                                </ItemTemplate>  
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Dock">  
                                                               
                                                                <ItemTemplate>  
                                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("DockName") %>'></asp:Label>  
                                                                </ItemTemplate>  
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Dock Location">  
                                                               
                                                                <ItemTemplate>  
                                                                    <asp:Label ID="lblDOCLOC" runat="server" Text='<%# Eval("Location") %>'></asp:Label>  
                                                                </ItemTemplate>  
                                                            </asp:TemplateField> 
                                                            <asp:TemplateField HeaderText="Vehicle No.">  
                                                               <ItemTemplate>  
                                                                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("VehicleRegNo") %>'></asp:Label>  
                                                                </ItemTemplate>  
                                                            </asp:TemplateField>  
                                                            <asp:TemplateField HeaderText="Created By">  
                                                               
                                                                <ItemTemplate>  
                                                                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>  
                                                                </ItemTemplate>  
                                                            </asp:TemplateField>  
                                                            <asp:TemplateField HeaderText="Created On">  
                                                               <ItemTemplate>  
                                                                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("CreatedOn") %>'></asp:Label>  
                                                                </ItemTemplate>  
                                                            </asp:TemplateField>
                                                             <asp:TemplateField HeaderText="Edit">
                                                                 <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkEditDock" runat="server" OnClick="lnkEditDock_Click" CommandArgument='<%# Eval("DockID", "{0}") %>'><nobr><i class='material-icons ss'>mode_edit</i></nobr></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Delete">  
                                                               <ItemTemplate>  
                                                                    <asp:LinkButton ID ="lnkDeleteDock" runat="server"  OnClick="lnkDeleteDock_Click" OnClientClick="return  window.confirm('Are you sure do you want to delete?');" CommandArgument='<%# Eval("DockID","{0}") %>'><%=MRLWMSC21Common.CommonLogic.btnfaDelete %></asp:LinkButton>
                                                                </ItemTemplate>  
                                                            </asp:TemplateField>
                                                           
                                                        </Columns>  
                                                        
                                                    </asp:GridView> 
                                                </div>
                                                </div>
                           

                                    </asp:Panel>

                                    <!-- New -->

                                </ContentTemplate>
                            </asp:UpdatePanel>

                    </div>

                </div>
                
                <br />
            <div id="accordion6" class="accordion" >
                    <h3>  <%= GetGlobalResourceObject("Resource", "PLShipmentInitiationDetails")%></h3>

                    <div >
                        
                            <asp:UpdateProgress ID="uprgTPLInboundCharge" runat="server" AssociatedUpdatePanelID="upnlTPLInboundCharge">
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
                            </asp:UpdateProgress>
                            <asp:UpdatePanel ID="upnlTPLInboundCharge" ChildrenAsTriggers="true" runat="server"  UpdateMode="Conditional" >
                                <ContentTemplate>
                                    
                                    <div>

                                        <div class="row">
                                            <div class="right">
                                                <asp:LinkButton ID="lnkAddNewTPLActivity" runat="server" OnClick="lnkAddNewTPLActivity_Click" CssClass="btn btn-sm btn-primary"> <%= GetGlobalResourceObject("Resource", "AddNew")%> <%=MRLWMSC21Common.CommonLogic.btnfaNew %></asp:LinkButton>
                                            </div>
                                        </div>
                                        
                                        <div>
                                            <div>
                                                <asp:GridView ID="gvTPLInboundCharges" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="10" SkinID="gvLightBlueNew"
                                                    OnRowDataBound="gvTPLInboundCharges_RowDataBound"
                                                    OnRowEditing="gvTPLInboundCharges_RowEditing"
                                                    OnRowUpdating="gvTPLInboundCharges_RowUpdating"
                                                    OnRowCancelingEdit="gvTPLInboundCharges_RowCancelingEdit"
                                                    OnPageIndexChanging="gvTPLInboundCharges_PageIndexChanging" Visible="true">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText= "<%$Resources:Resource,TariffSubGroup%>" ItemStyle-Width="100">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="ltActivityRateType" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ActivityRateType") %>' />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <div class="gridInput"><asp:Literal ID="ltTenantTransactionAccessorialCaptureID" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TenantTransactionAccessorialCaptureID") %>' />
                                                                <asp:HiddenField ID="hifActivityRateTypeID" ClientIDMode="Static" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"ActivityRateTypeID") %>' />
                                                                <asp:RequiredFieldValidator ID="rfvActivityRateType" runat="server" Display="Dynamic" ControlToValidate="txtActivityRateType" ValidationGroup="vgRequired3pl" />
                                                                <span class="errorMsg">* </span><asp:TextBox ID="txtActivityRateType" ClientIDMode="Static" SkinID="txt_Auto_Req" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ActivityRateType") %>' /></div>

                                                            </EditItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText= "<%$Resources:Resource,Tariff%>"  ItemStyle-Width="170">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="ltActivityRateName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ActivityRateName") %>' />

                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <div class="gridInput"><asp:RequiredFieldValidator ID="rfvActivityRateName" runat="server" ControlToValidate="txtActivityRateName" ValidationGroup="vgRequired3pl" Display="Dynamic" />
                                                                <span class="errorMsg">* </span><asp:TextBox ID="txtActivityRateName" ClientIDMode="Static" SkinID="txt_Auto_Req" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ActivityRateName") %>' />
                                                                <asp:HiddenField ID="hifActivityRateID" ClientIDMode="Static" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"ActivityRateID") %>' /></div>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText= "<%$Resources:Resource,UoM%>"   ItemStyle-Width="30">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="ltUOM" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"UoM") %>' />
                                                            </ItemTemplate>
                                                       
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText= "<%$Resources:Resource,CostPrice%>"  ItemStyle-Width="100" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="ltcostprice" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"CostPrice") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText= "<%$Resources:Resource,UnitCostafterDiscount%>"   ItemStyle-Width="100" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <%--<asp:Literal ID="ltUnitCost" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"UnitCost") %>' />--%>
                                                                <asp:Literal ID="ltUnitCostAfterDiscount" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"UnitCostAfterDiscount") %>' />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <%--<asp:TextBox ID="txtUnitCost" Width="50" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"UnitCost") %>' />--%>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText= "<%$Resources:Resource,Quantity%>"  ItemStyle-Width="60" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="ltQuantity" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Quantity") %>' />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <div class="gridInput"><asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ControlToValidate="txtQuantity" ValidationGroup="vgRequired3pl" Display="Dynamic" /><span class="errorMsg">*</span><asp:TextBox ID="txtQuantity" Width="50" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Quantity") %>' onKeyPress="return checkDec(event)" /></div>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText= "<%$Resources:Resource,TotalCostafterDiscount%>" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <%--<asp:Literal ID="ltTotalCost" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TotalCost") %>' />--%>
                                                                 <asp:Literal ID="ltTotalCostAfterDiscount" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TotalCostAfterDiscount") %>' />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <%--<asp:TextBox ID="txtTotalCost" Width="50" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TotalCost") %>' />--%>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText= "<%$Resources:Resource,Description%>" ItemStyle-Width="200">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="ltDescription" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ActivityRateDescription") %>' />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <%--<asp:TextBox ID="txtDescription" Width="120" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Description") %>' />--%>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>


                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%" FooterText="Delete">
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="CheckAllForTPLInboundAll" onclick="return check_uncheckTPL (this);" runat="server" /><br>
                                                                <asp:Label ID="lblOBDCancel" CssClass="smlText" runat="server" Text="Delete"></asp:Label>
                                                            </HeaderTemplate>
                                                            <FooterTemplate>
                                                                <asp:LinkButton ID="btnDeleteTPLInboundCharge" Font-Underline="false" runat="server" OnClick="btnDeleteTPLInboundCharge_Click" OnClientClick="return confirm('Are you sure, you want to delete the selected items?')"><nobr> <i class='material-icons ss'>delete</i></nobr></asp:LinkButton>
                                                            </FooterTemplate>
                                                            <EditItemTemplate>
                                                                  <asp:Label ID="RecID" Visible="false" Text='<%# DataBinder.Eval (Container.DataItem, "TenantTransactionAccessorialCaptureID") %>' runat="server" />
                                                                <asp:CheckBox ID="deleteTPLInboundCharge" runat="server"></asp:CheckBox>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="RecID" Visible="false" Text='<%# DataBinder.Eval (Container.DataItem, "TenantTransactionAccessorialCaptureID") %>' runat="server" />
                                                                <asp:CheckBox ID="deleteTPLInboundCharge" runat="server"></asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:CommandField ValidationGroup="vgRequired3pl" ItemStyle-Font-Underline="false" ControlStyle-Font-Underline="false" ItemStyle-Width="30" ButtonType="Link" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint" ItemStyle-HorizontalAlign="left" CancelImageUrl="/Images/cancel.gif" EditImageUrl="/Images/edit.gif" EditText="<nobr> <i class='material-icons ss'>mode_edit</i></nobr>" ShowEditButton="True" UpdateImageUrl="/Images/save.gif" />



                                                    </Columns>

                                                </asp:GridView>

                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        
                    </div>

                </div>
                     
                <br />          
                <div id="accordion4" class="accordion border">

                    <h3> <%= GetGlobalResourceObject("Resource", "ShipmentReceivedDetails")%>
                    </h3>
                    <div>
                        <!-- Start Shipment Received & Verification Details -->
                        
                            <asp:UpdateProgress ID="uprgRcv" runat="server" AssociatedUpdatePanelID="upnlRcv">
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
                            </asp:UpdateProgress>
                            <asp:UpdatePanel ID="upnlRcv" runat="server"  UpdateMode="Conditional" >
                                    <ContentTemplate>               
            
                                        <asp:Panel runat="server" ID="pnlShipmentReceived" Visible="false" HorizontalAlign="Left">


                                            <div class="">

                                                <div>
                                                    <div>
                                                         <asp:Label ID="lblChargesMsg" runat="server" CssClass="ChargesStatus" ></asp:Label>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col m6">
                                                        <span class="FormLabelsBlue"><p> <%= GetGlobalResourceObject("Resource", "ReceivedDetails")%></p></span>
                                                    </div>
                                                     <div class="col m6">
                                                         <div class="right">
                                                             <asp:HyperLink runat="server" ID="hlnkRTR" Font-Underline="false"></asp:HyperLink>
                                                             <asp:Label runat="server" ID="lblShipmentReceivedWH" CssClass="SubHeading3"></asp:Label>
                                                        </div>
                                                     </div>
                                                </div>
                                            </div>



                                            <div>
                                                <div>

                                                    <div class="FormLabels">
                                                        <div>
                                                            <br />

                                                            <div class="row underlineani">
                                                                <div>
                                                                    <div>

                                                                        <asp:Label ID="lblLocalerror" runat="server" />
                                                                        <a name="page_msg"></a>
                                                                        <asp:Label ID="lblShipmentReceivedStatusMessage" runat="server" CssClass="ErrorMsg" />
                                                                    </div>
                                                                </div>

                                                                <div class="col m4">
                                                           <%--         <div class=" flex">
                                                                            <asp:RequiredFieldValidator ID="rfvddlStores" SetFocusOnError="true" ControlToValidate="ddlStores" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2"  runat="server" ValidationGroup="updateShipmentReceived"  />
                                                                            <asp:DropDownList ID="ddlStores"  Enabled="true" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlStores_SelectedIndexChanged" required="required"/>
                                                                                <span class="errorMsg">* </span>
                                                                                <label>Receiving Store</label>
                                                                     </div>--%>
                                                                </div>
                                                            </div>
                                                            <div>
                                                                <div>
                                                                    <a name="shipment_rcvd"></a>

                                                                    <asp:Panel ID="pnlShipmentReceivedDetails" runat="server" CssClass="pnlBoxAlt">
                                                                        <div>

                                                                            <%--<div class="input-full-with">--%>
                                                                            <div class="">
                                                                                <div class="FormLabels row">
                                                                                        <div class="col m3">
                                                                                            <div class=" flex">
                                                                                            <asp:RequiredFieldValidator ID="rfvddlStores" SetFocusOnError="true" ControlToValidate="ddlStores" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2"  runat="server" ValidationGroup="updateShipmentReceived"  />
                                                                                            <asp:DropDownList ID="ddlStores"  Enabled="true"  runat="server"  required="required"/>
                                                                                            <span class="errorMsg">* </span>
                                                                                            <label> <%= GetGlobalResourceObject("Resource", "ReceivingStore")%></label></div>
                                                                                        </div>
                                                                                        <div class="col m3">
                                                                                            <div class="FormLabels flex ">
                                                                                                <%--<asp:RequiredFieldValidator ID="rfvtxtShipmentReceivedDate" SetFocusOnError="true" ControlToValidate="txtShipmentReceivedDate" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2"  runat="server" ValidationGroup="updateShipmentReceived" />--%>
                                                                                                <asp:TextBox ID="txtShipmentReceivedDate" Enabled="false" runat="server" required="required"/>
                                                                                                  <label> <%= GetGlobalResourceObject("Resource", "ShipmentReceivedDate")%></label>
                                                                                                 <span class="errorMsg">* </span>
                                                                                                 <%--<label>Shipment Received DateM</label>--%>
                                                                                           </div> 
                                                                                        </div>
                                                                                        <div class="col m3">
                                                                                            <div class=" FormLabels" >
                                                                                               <div class="flex l-lab">
                                                                                                <asp:RequiredFieldValidator ID="rfvtxtOffLoadingTime" SetFocusOnError="true" ControlToValidate="txtOffLoadingTime" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2"  runat="server" ValidationGroup="updateShipmentReceived" />
                                                                                                <asp:TextBox ID="txtOffLoadingTime" CssClass="timeEntry" AutoCompleteType="Disabled" runat="server" placeholder="Offloading Time" required="required" />
                                                                                                <span class="errorMsg">* </span>
                                                                                                </div>
                                                                                           </div> 
                                                                                        </div>
                                                                                        <div class="col m3  p0">
                                                                                            <br />
                                                                                            <div class="right">
                                                                                                <asp:LinkButton ID="lnkCancelReceived" runat="server" OnClick="lnkCancelReceived_Click" CssClass="btn btn-sm btn-primary">
                                                                                                 <%= GetGlobalResourceObject("Resource", "Clear")%> <%=MRLWMSC21Common.CommonLogic.btnfaClear %>
                                                                                                </asp:LinkButton>
                                                                                                <asp:LinkButton ID="lnkShipmentReceived" CausesValidation="True" OnClientClick=" Openloader(); Panel_ValidationActive=true; " ValidationGroup="updateShipmentReceived" runat="server" OnClick="lnkShipmentReceived_Click" CssClass="btn btn-sm btn-primary">
                                                                                                               <%= GetGlobalResourceObject("Resource", "Receive")%> <%=MRLWMSC21Common.CommonLogic.btnfaSave %>
                                                                                                </asp:LinkButton>
                                                                                                <%--<asp:ValidationSummary ID="ValidationSummary2"  runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="updateShipmentReceived" ForeColor="red" Font-Bold="true" />--%>
                                                                                            </div>
                                                                                        </div>
                                                                                </div>
                                                                             </div>

                                                                        </div>
                                                                    </asp:Panel>

                

                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                     <div class="FormLabels">

                                                        <div>

                                                            <div>

                                                                <div class="FormLabels">

                                                                    <div>
                                                                        <div>
                                                                            <div>
                                                                                <!-- Start Actual Vehicle Grid -->

                                                                                               <br />
                                                                                <div>
                                                                   
                                                                                    <div class="row" style="display:none">
                                                                                        <div class="FormLabelsBlue col m5">

                                                                                            <div class="">
                                                                                                <label style="    margin: 0;">  <%= GetGlobalResourceObject("Resource", "ActualVehicleDetails")%>  </label>
                                                                                                <asp:LinkButton ID="lnkAddAVNew" Visible="true" OnClick="lnkAddAVNew_Click" runat="server" CssClass="btn btn-sm btn-primary">
                                                                                                     <%= GetGlobalResourceObject("Resource", "AddActualVehicle")%> <%=MRLWMSC21Common.CommonLogic.btnfaNew %>
                                                                                                </asp:LinkButton>
                                                                                                </div>
                                                                                        </div>
                                                                                    </div>

                                                                                    <div>
                                                                                        <div style="width:50%;">
                                                                                            <asp:Panel runat="server" ID="pnlAVDetails" HorizontalAlign="left">
                                                                                                <asp:Label runat="server" ID="lblAVStatus"></asp:Label>
                                                                                               
                                                                                                <asp:GridView ID="gvActualVehicle" SkinID="gvLightGrayNew" runat="server" Width="300px" CellPadding="4"
                                                                                                    AllowPaging="false"
                                                                                                    AllowSorting="false"
                                                                                                    OnRowDataBound="gvActualVehicle_RowDataBound"
                                                                                                    OnRowCommand="gvActualVehicle_RowCommand"
                                                                                                    OnRowUpdating="gvActualVehicle_RowUpdating"
                                                                                                    OnRowCancelingEdit="gvActualVehicle_RowCancelingEdit"
                                                                                                    OnRowEditing="gvActualVehicle_RowEditing"
                                                                                                    PagerStyle-HorizontalAlign="Right"
                                                                                                    CellSpacing="0">
                                                                                                    <Columns>
                                                                                                        <asp:TemplateField HeaderText= "<%$Resources:Resource,ActualVehicle%>"  >
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Literal ID="ltActualVehicle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "EquipmentName") %>' />
                                                                                                                <asp:Literal ID="lthidActualEquipmentID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ActualEquipmentID") %>' />
                                                                                                            </ItemTemplate>
                                                                                                            <EditItemTemplate>
                                                                                                                <asp:RequiredFieldValidator ID="rfvddlActualEquipment" SetFocusOnError="true" ControlToValidate="ddlActualEquipment" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage="*" runat="server" InitialValue="0" />
                                                                                                                <asp:DropDownList runat="server" ID="ddlActualEquipment" ></asp:DropDownList>
                                                                                                                <asp:Literal runat="server" Visible="false" ID="ltEquipmentID" Text='<%# DataBinder.Eval(Container.DataItem, "EquipmentID") %>'></asp:Literal>
                                                                                                                <asp:Literal ID="lthidActualEquipmentID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ActualEquipmentID") %>' />
                                                                                                            </EditItemTemplate>
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText= "<%$Resources:Resource,ActualValue%>"  ItemStyle-HorizontalAlign="Center">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Literal ID="ltActualValue" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ActualValue") %>' />
                                                                                                            </ItemTemplate>
                                                                                                            <EditItemTemplate>
                                                                                                                <asp:RequiredFieldValidator ID="rfvtxtActualValue" SetFocusOnError="true" ControlToValidate="txtActualValue" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" />
                                                                                                                <asp:TextBox ID="txtActualValue" onKeyPress="return checkNum(event)" Width="80" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ActualValue") %>' ></asp:TextBox>
                                                                                                            </EditItemTemplate>
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText= "<%$Resources:Resource,Delete%>"  ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint">
                                                                                                            <HeaderTemplate>
                                                                                                                <nobr>"<%= GetGlobalResourceObject("Resource", "Delete")%></nobr>
                                                                                                            </HeaderTemplate>
                                                                                                            <ItemTemplate>
                                                                                                                <asp:CheckBox ID="chkIsDeleteRFItem1" runat="server" />
                                                                                                            </ItemTemplate>
                                                                                                            <EditItemTemplate></EditItemTemplate>
                                                                                                            <FooterTemplate>
                                                                                                                <asp:LinkButton ID="lnkDeleteRFItem1" Font-Underline="false" CssClass="blueLink" runat="server" Text="<nobr> <i class='material-icons ss'>delete</i></nobr>" OnClick="lnkDeleteAVItem_Click" OnClientClick="return confirm('Are you sure want to delete selected items ?')" />
                                                                                                            </FooterTemplate>
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:BoundField ItemStyle-Font-Underline="false" Visible="false" DataField="EditName" ReadOnly="true" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint" />
                                                                                                        <asp:CommandField ItemStyle-Font-Underline="false" ControlStyle-Font-Underline="false" ItemStyle-Width="30" ButtonType="Link" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint" ItemStyle-HorizontalAlign="left" CancelImageUrl="/Images/cancel.gif" EditImageUrl="/Images/edit.gif" EditText="<nobr> <i class='material-icons ss'>mode_edit</i></nobr>" ShowEditButton="True" UpdateImageUrl="/Images/save.gif" />
                                                                                                    </Columns>
                                                                                                </asp:GridView>
                                                                                            </asp:Panel>

                                                                                        </div>
                                                                                    </div>

                                                                                </div>
                                                                                <!--  End Actual Vehicle Grid -->
                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                </div>
                                                            </div>
                                                        </div>

                                                    </div>
                                        </div>
                                    </div>

                                       </asp:Panel>

                                        <!-- End Shipment Received Details -->


                                        <!-- Start Shipment Verification Details -->
                          
                                    </ContentTemplate>
                                    <Triggers>
                                         <asp:PostBackTrigger ControlID="lnkShipmentReceived" />
                                    </Triggers>
                            </asp:UpdatePanel>

                        <br />

                        <!-- End Shipment  Verification Details ---->

                        <!-- End Shipment Received & Verification Details ---->
                    </div>


                </div>
                
                <br />
                <div id="accordion7" class="accordion">
                    <h3> <%= GetGlobalResourceObject("Resource", "PLShipmentReceivingDetails")%></h3>

                    <div >
                        
                            <asp:UpdateProgress ID="uprgTPLReceivingCharge" runat="server" AssociatedUpdatePanelID="upnlTPLReceivingCharge">
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
                            </asp:UpdateProgress>
                            <asp:UpdatePanel ID="upnlTPLReceivingCharge" runat="server" Visible="false" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div>
                                        <div>
                                            <div>
                                                <asp:LinkButton ID="lnkADDNewReceivingActivity" runat="server" OnClick="lnkADDNewReceivingActivity_Click" CssClass="btn btn-sm btn-primary right">Add New <%=MRLWMSC21Common.CommonLogic.btnfaNew %></asp:LinkButton>
                                           <br /> </div>
                                        </div>
                                        <br />
                                        <div>
                                            <div>
                                                &nbsp;
                                            </div>
                                        </div>
                                        <div>
                                            <div>
                                                <asp:GridView ID="gvTPLReceivingCharges" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="10" SkinID="gvLightBlueNew"
                                                    OnRowDataBound="gvTPLReceivingCharges_RowDataBound"
                                                    OnRowEditing="gvTPLReceivingCharges_RowEditing"
                                                    OnRowUpdating="gvTPLReceivingCharges_RowUpdating"
                                                    OnRowCancelingEdit="gvTPLReceivingCharges_RowCancelingEdit"
                                                    OnPageIndexChanging="gvTPLReceivingCharges_PageIndexChanging">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText= "<%$Resources:Resource,TariffSubGroup%>" ItemStyle-Width="100">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="ltRcvActivityRateType" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ActivityRateType") %>' />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:Literal ID="ltRcvTenantTransactionAccessorialCaptureID" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TenantTransactionAccessorialCaptureID") %>' />
                                                                <asp:HiddenField ID="hifRcvActivityRateTypeID" ClientIDMode="Static" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"ActivityRateTypeID") %>' />
                                                                <asp:RequiredFieldValidator ID="rfvRcvActivityRateType" runat="server" Display="Dynamic" ControlToValidate="txtRcvActivityRateType" ValidationGroup="vgRequiredReceiving" ErrorMessage="*" />
                                                                <asp:TextBox ID="txtRcvActivityRateType" ClientIDMode="Static" SkinID="txt_Auto_Req" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ActivityRateType") %>' />

                                                            </EditItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText= "<%$Resources:Resource,Tariff%>"  ItemStyle-Width="170">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="ltRcvActivityRateName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ActivityRateName") %>' />

                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:RequiredFieldValidator ID="rfvRcvActivityRateName" runat="server" ControlToValidate="txtRcvActivityRateName" ValidationGroup="vgRequiredReceiving" Display="Dynamic" ErrorMessage="*" />
                                                                <asp:TextBox ID="txtRcvActivityRateName" ClientIDMode="Static" SkinID="txt_Auto_Req" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ActivityRateName") %>' />
                                                                <asp:HiddenField ID="hifRcvActivityRateID" ClientIDMode="Static" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"ActivityRateID") %>' />
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText= "<%$Resources:Resource,UoM%>"  ItemStyle-Width="30">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="ltRcvUOM" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"UoM") %>' />
                                                            </ItemTemplate>
                                                       
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText= "<%$Resources:Resource,UnitCostafterDiscount%>"  ItemStyle-Width="100" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <%--<asp:Literal ID="ltUnitCost" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"UnitCost") %>' />--%>
                                                                <asp:Literal ID="ltRcvUnitCostAfterDiscount" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"UnitCostAfterDiscount") %>' />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <%--<asp:TextBox ID="txtUnitCost" Width="50" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"UnitCost") %>' />--%>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText= "<%$Resources:Resource,Quantity%>"  ItemStyle-Width="60" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="ltRcvQuantity" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Quantity") %>' />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ControlToValidate="txtRcvQuantity" ValidationGroup="vgRequiredReceiving" Display="Dynamic" ErrorMessage="*" />
                                                                <asp:TextBox ID="txtRcvQuantity" Width="50" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Quantity") %>' onKeyPress="return checkDec(event)" />
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText= "<%$Resources:Resource,TotalCostafterDiscount%>"  ItemStyle-Width="100" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <%--<asp:Literal ID="ltTotalCost" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TotalCost") %>' />--%>
                                                                 <asp:Literal ID="ltRcvTotalCostAfterDiscount" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TotalCostAfterDiscount") %>' />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <%--<asp:TextBox ID="txtTotalCost" Width="50" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TotalCost") %>' />--%>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText=  "<%$Resources:Resource,Description%>"  ItemStyle-Width="200">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="ltRcvDescription" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ActivityRateDescription") %>' />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <%--<asp:TextBox ID="txtDescription" Width="120" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Description") %>' />--%>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>


                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%" FooterText="Delete">
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="CheckTPLRcvAll" onclick="return check_uncheckTPLRcv (this);" runat="server" /><br>
                                                                <asp:Label ID="lblOBDCancelRcv" CssClass="smlText" runat="server" Text="Delete"></asp:Label>
                                                            </HeaderTemplate>
                                                            <FooterTemplate>
                                                                <asp:LinkButton ID="btnDeleteTPLRecvCharge" Font-Underline="false" runat="server" OnClick="btnDeleteTPLRecvCharge_Click" OnClientClick="return confirm('Are you sure ? want to delete selected items')"><i class='material-icons ss'>delete</i></asp:LinkButton>
                                                            </FooterTemplate>
                                                            <EditItemTemplate>

                                                                <asp:Label ID="RecRecvID" Visible="false" Text='<%# DataBinder.Eval (Container.DataItem, "TenantTransactionAccessorialCaptureID") %>' runat="server" />
                                                                <asp:CheckBox ID="deleteTPLReceivingCharge" runat="server"></asp:CheckBox>
                                                            </EditItemTemplate>

                                                            <ItemTemplate>
                                                                <asp:Label ID="RecRecvID" Visible="false" Text='<%# DataBinder.Eval (Container.DataItem, "TenantTransactionAccessorialCaptureID") %>' runat="server" />
                                                                <asp:CheckBox ID="deleteTPLReceivingCharge" runat="server"></asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:CommandField ValidationGroup="vgRequiredReceiving" ItemStyle-Font-Underline="false" ControlStyle-Font-Underline="false" ItemStyle-Width="30" ButtonType="Link" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint" ItemStyle-HorizontalAlign="left" CancelImageUrl="/Images/cancel.gif" EditImageUrl="/Images/edit.gif" EditText="<nobr> <i class='material-icons ss'>mode_edit</i></nobr>" ShowEditButton="True" UpdateImageUrl="/Images/save.gif" />



                                                    </Columns>

                                                </asp:GridView>

                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        
                    </div>

                </div>
                <br />
                <div id="accordion5" class="accordion">
                    <h3> <%= GetGlobalResourceObject("Resource", "GRNDetails")%>
                    </h3>
                    <div>                        

                            <asp:UpdateProgress ID="uprgGRNDetails" runat="server" AssociatedUpdatePanelID="upnlGRNDetails">
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
                            </asp:UpdateProgress>
                            <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" ID="upnlGRNDetails" UpdateMode="Conditional">
                                <ContentTemplate>

                                    <asp:Panel ID="pnlGRNDetails" runat="server" Visible="false">

                                        <div>                                     
                                            <div>
                                                <div>
                                                
                                                    <asp:Label runat="server" ID="lblgvGRNStatus"></asp:Label>
                                                </div>
                                                <div class="row">
                                                    <div class="right">
                                                      <button type="button" id="inkADDGRN" class="btn btn-primary " ng-click="OpenGRNPopUP()" data-toggle="modal">Add GRN<i class="material-icons">add</i></button>
                                                    </div>
                                                </div>
                                            </div>
                                            <div>
                                                <div >
                                                 

                                                    <div ng-show="GRNData!=null && GRNData!=undefined && GRNData.length!=0">

                                                        <table align="center" class="table-striped" cellpadding="0" cellspacing="0">
                                                            <thead style="white-space: nowrap">
                                                                <tr>
                                                                    <th>PO Number</th>
                                                                    <th>Invoice No.</th>
                                                                    <th>Updated By</th>
                                                                    <th>GRN Date</th>
                                                                    <th>GRN No.</th>                                                                    
                                                                  

                                                                </tr>
                                                            </thead>

                                                            <tbody>
                                                                <tr  ng-repeat="PO in GRNData" class="mytableOutboundBodyTR">

                                                                    <td>{{PO.PONumber}}</td>
                                                                    <td>{{PO.InvoiceNumber}}</td>
                                                                    <td>{{PO.GRNUpdatedBy}}</td>
                                                                    <td>{{PO.GRNDate}}</td>
                                                                    <td>{{PO.GRNNumber}}</td>
                                                                    
                      
                                                                </tr>
                                                            </tbody>
                                                        </table>

                                                        <div style="float: right !important; font-family: Arial; font-size: small; margin-right: 1%;">
                                                            
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>

                                    </asp:Panel>

                                </ContentTemplate>                            
                            </asp:UpdatePanel>





                        <div id="AddGRN" class="modal fade" data-backdrop="static" data-keyboard="false">
                            <div class="modal-dialog" role="document" style="width: 800px !important;">
                                <div class="modal-content">
                                    <div class="modal-header" style="background-color: var(--sideNav-bg) !important; color: #fff !important;">
                                        <h4 class="modal-title" style="display: inline !important;">Add GRN Details</h4>
                                        <button type="button" data-dismiss="modal" class="pull-right modalclose" aria-label="Close">
                                            <span aria-hidden="true">&times;</span>
                                        </button>
                                    </div>
                                    <div class="modal-body" id="GrnPending">

                                        <table>
                                            <tr>
                                                <td>

                                                    <div class="modal-body" id="GrnPendingDetailsPOPUP" style="padding: 5px !important;">

                                                        <div class="">
                                                            <div class="row">

                                                                <div class="col m3">
                                                                    <div class="flex">
                                                                        <input type="text" id="txtPONO" required="" />
                                                                        <label>PO No.</label>
                                                                        <span class="errorMsg"></span>
                                                                    </div>
                                                                </div>
                                                                <div class="col m3">
                                                                    <div class="flex">
                                                                        <input type="text" id="txtInvoiceNo" required="" />
                                                                        <label>Invoice No.</label>
                                                                        <span class="errorMsg"></span>
                                                                    </div>
                                                                </div>
                                                              
                                                                <div class="col m1">
                                                                    <gap></gap>
                                                                    <button type="button" class="btn btn-primary" ng-click="FetchGRNDataForInbound()">Fetch</button>
                                                                </div>

                                                            </div>
                                                            <div class="row" ng-if="GRNProcessingData!=undefined && GRNProcessingData!=null && GRNProcessingData!='' && GRNProcessingData.length!=0">
                                                                <table class="table-striped">
                                                                    <thead style="white-space: nowrap">
                                                                        <tr>
                                                                            <th>Rcvd. Dt.</th>
                                                                            <th>PO No.</th>
                                                                            <th>Invoice No.</th>
                                                                            <th>SKU</th>
                                                                            <th>Description</th>
                                                                            <th>Received Qty.</th>

                                                                        </tr>
                                                                    </thead>

                                                                    <tbody >
                                                                      
                                                                        <tr  ng-repeat="PO in GRNProcessingData" class="mytableOutboundBodyTR">

                                                                            <td>{{PO.ReceivedDate}}</td>
                                                                            <td>{{PO.PONumber}}</td>
                                                                            <td>{{PO.InvoiceNumber}}</td>
                                                                            <td>{{PO.MCode}}</td>
                                                                            <td>{{PO.MDescription}}</td>
                                                                            <td>{{PO.Quantity}}</td>

                                                                        </tr>
                                                                    </tbody>

                                                                </table>
                                                                <div style="float: right !important; font-family: Arial; font-size: small; margin-right: 1%;">
                                                         <%--           <dir-pagination-controls direction-links="true" pagination-id="GRNDetails" boundary-links="true"> </dir-pagination-controls>--%>
                                                                </div>
                                                            </div>
                                                            <div class="row" ng-if="GRNProcessingData!=undefined && GRNProcessingData!=null && GRNProcessingData!='' && GRNProcessingData.length!=0">
                                                                <div class="col m6 offset-m4">
                                                                    <div class="flex">
                                                                        <input type="text" required="required" ng-model="GRNHeader.Remarks"/>
                                                                        <label>Remarks.</label>
                                                                        <span class="errorMsg"></span>
                                                                        <div class="mdd-select-underline"></div>
                                                                    </div>
                                                                </div>
                                                                <div class="col m1">
<%--                                                                    <div class="gap10"></div>
                                                                    <br />--%>
                                                                    <img src="../Images/bx_loader.gif" id="imgGRNLLoadingSAP" style="width: 60px; display: none;" />
                                                                    <button type="button" id="btnaddGRNDetails" ng-click="AddGRNDetails()" class="btnGetTemplate btn btn-primary">Create</button>
                                                                    
                                                                </div>

                                                            </div>
                                                        </div>


                                                    </div>


                                                </td>
                                            </tr>
                                        </table>



                                    </div>

                                </div>
                            </div>
                        </div>


                    </div>
                </div>
                <br />
                <div id="accordion12" class="accordion DiscrepencyHeader">
                    <h3>  <%= GetGlobalResourceObject("Resource", "Discrepancy")%></h3>
                    <div class="Discrepency">
                       
                        <div class="FormLabels">

                            <asp:Panel runat="server" ID="pnlDiscInfo" Visible="false" CssClass="GDRPanel" ClientIDMode="Static">
                                <div class="checkbox">

                                    <asp:CheckBox ID="chkHasDiscrepancy" runat="server" AutoPostBack="true" Text="" OnCheckedChanged="chkHasDiscrepancy_CheckedChanged" />
                                    <label for="">  <%= GetGlobalResourceObject("Resource", "HasDiscrepancy")%> </label>

                                </div>
                                <div id="tdDDRPrintArea">

                                    <div>
                                        <div>
                                            <div>
                                                <div>
                                                    <div>
                                                        <%--style="visibility:hidden;"--%>
                                                        <img id="imgCompanyLogo" runat="server" enableviewstate="false" src="~/Images/INV_icon.png" width="40" alt="">
                                                    </div>
                                                    <div>
                                                        <font class="fontss">INBOUND DISCREPANCY FORM </font>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div></div>
                                    </div>

                                    <div>
                                        <div></div>
                                    </div>

                                    <div>
                                        <div>
                                            <asp:Literal ID="ltScript" runat="server"></asp:Literal>
                                            <asp:Literal ID="ltValid" runat="server"></asp:Literal>

                                        </div>

                                    </div>
                                    <div>
                                        <div>
                                            &nbsp;
                                        </div>
                                    </div>
<div class="row">
    
                                    <div class="col m12">
                                        <div class="PrintArea">
                                            <div>
                                                <div class="inbtt">
                                                    <div class='FormLabels'> <%= GetGlobalResourceObject("Resource", "Supplier")%></div>
                                                    <div>
                                                        <asp:Literal ID="ltSupplierVal" runat="server" />
                                                    </div>
                                                </div>
                                                <div  class="inbtt">
                                                    <div class='FormLabels'> <%= GetGlobalResourceObject("Resource", "StoreRefNo")%> </div>
                                                    <div>
                                                        <asp:Literal ID="ltStrrefNoVal" runat="server" />
                                                    </div>
                                                </div>
                                                <div  class="inbtt">
                                                    <div class='FormLabels'> <%= GetGlobalResourceObject("Resource", "Warehouse")%></div>
                                                    <div>
                                                        <asp:Literal ID="ltStoreVal" runat="server" />
                                                    </div>
                                                </div>
                                                <div  class="inbtt">
                                                    <div class='FormLabels'>  <%= GetGlobalResourceObject("Resource", "AWBBLNo")%> </div>
                                                    <div>
                                                        <asp:Literal ID="ltAWBVal" runat="server" />
                                                    </div>
                                                </div>
                                                <div style="display:none;" class="inbtt">
                                                    <div class='FormLabels'><%= GetGlobalResourceObject("Resource", "NoofPackages")%> </div>
                                                    <div>
                                                        <asp:Literal ID="ltNoofPackagesInDocumentVal" runat="server" />
                                                    </div>
                                                </div>
                                                <div style="display:none;" class="inbtt">
                                                    <div class='FormLabels'>
                                                       <%-- <asp:RequiredFieldValidator ID="rfvtxtPackPhyRcvd" runat="server" ValidationGroup="updateShipmentVerified1" ControlToValidate="txtPackPhyRcvd" Display="Dynamic" ErrorMessage=" * " CssClass="errorMsg" />--%>
                                                        Packages Physically Received:
                                                    </div>
                                                    <div class="flex inpt">
                                                        <asp:TextBox ID="txtPackPhyRcvd" runat="server" onKeyPress="return checkNum(event)" />
                                                    </div>
                                                </div>
                                                <div class="inbtt">
                                                    <div class='FormLabels'><%= GetGlobalResourceObject("Resource", "ClearanceAgent")%> </div>
                                                    <div>
                                                        <asp:Literal ID="ltCCVal" runat="server" />
                                                    </div>
                                                    </div>
                                                <div class="inbtt">
                                                    <div class='FormLabels'> <%= GetGlobalResourceObject("Resource", "DateReceivedinStore")%> </div>
                                                    <div>
                                                        <asp:Literal ID="ltShipmentRcvdVal" runat="server" />
                                                    </div>
                                                </div>
                                                <div  class="inbtt">

                                                    <div><span class="FormLabelsBlue">  <%= GetGlobalResourceObject("Resource", "DiscrepancyRecords")%>  </span></div>
                                                    <div>
                                                       
                                                        <asp:LinkButton runat="server" ID="lnkAddNewDiscrepancy" OnClick="lnkAddNewDiscrepancy_Click" CssClass="btn btn-sm btn-primary">
                                                                 <%= GetGlobalResourceObject("Resource", "AddDiscrepancy")%> <%=MRLWMSC21Common.CommonLogic.btnfaNew %>
                                                        </asp:LinkButton>
                                                    </div>

                                                </div>
                                            </div>

                                            <asp:Label ID="ltgvDiscrepancyError" runat="server" CssClass="NoPrint" Text="" />



                                            <asp:Panel ID="pnlDiscrepancyRecords" runat="server">
                                                <br />

                                                <asp:GridView Width="100%" ShowFooter="true" ID="gvDiscpreancyRecords" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="true" PageSize="10" SkinID="gvLightGrayNew" HorizontalAlign="Left" OnRowCancelingEdit="gvDiscpreancyRecords_RowCancelingEdit" OnPageIndexChanging="gvDiscpreancyRecords_PageIndexChanging" OnRowUpdating="gvDiscpreancyRecords_RowUpdating" OnRowDataBound="gvDiscpreancyRecords_RowDataBound" OnRowEditing="gvDiscpreancyRecords_RowEditing">
                                                    <Columns>

                                                        <asp:TemplateField ItemStyle-Width="90" HeaderText="<%$Resources:Resource,Invoice%>" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal runat="server" ID="ltInvoiceNumber" Text='<%# DataBinder.Eval(Container.DataItem, "InvoiceNumber") %>' />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:RequiredFieldValidator ID="rfvtxtDInvoiceNumber" runat="server" ControlToValidate="txtDInvoiceNumber" Display="Dynamic" ErrorMessage=" * " />
                                                                <asp:TextBox ID="txtDInvoiceNumber" runat="server" EnableTheming="false" CssClass="DynaDisInvNumber" Text='<%# DataBinder.Eval(Container.DataItem, "InvoiceNumber") %>' Width="150" />

                                                            </EditItemTemplate>
                                                        </asp:TemplateField>


                                                        <asp:TemplateField ItemStyle-Width="90" HeaderText= "<%$Resources:Resource,PONumber%>"  ItemStyle-HorizontalAlign="Center">

                                                            <ItemTemplate>
                                                                <asp:Literal runat="server" ID="ltPONumber" Text='<%# DataBinder.Eval(Container.DataItem, "PONumber") %>' />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:RequiredFieldValidator ID="rfvtxtDescPONumber" runat="server" ControlToValidate="txtDescPONumber" Display="Dynamic" ErrorMessage=" * " />
                                                                <asp:TextBox ID="txtDescPONumber" runat="server" EnableTheming="false" CssClass="DynaDescPONumber" Text='<%# DataBinder.Eval(Container.DataItem, "PONumber") %>' Width="150" />

                                                            </EditItemTemplate>


                                                        </asp:TemplateField>


                                                        <asp:TemplateField ItemStyle-Width="90" HeaderText=  "<%$Resources:Resource,PartNumber%>">
                                                            <ItemTemplate>
                                                                <asp:Literal runat="server" ID="ltMCode" Text='<%# DataBinder.Eval(Container.DataItem, "MCode") %>' />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:RequiredFieldValidator ID="rfvtxtMCode" runat="server" ControlToValidate="txtDMCode" Display="Dynamic" ErrorMessage=" * " />
                                                                <asp:TextBox ID="txtDMCode" EnableTheming="false" CssClass="DescMCodePicker" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MCode") %>' Width="150" />
                                                                <asp:HiddenField ID="hifDescMCodeID" ClientIDMode="Static" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "MaterialMasterID") %>' />
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField ItemStyle-Width="50" HeaderText=  "<%$Resources:Resource,POLine%>"  ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Literal runat="server" ID="ltLineNumber" Text='<%# DataBinder.Eval(Container.DataItem, "LineNumber") %>' />
                                                                <asp:Literal runat="server" Visible="false" ID="ltDiscrepancyID" Text='<%# DataBinder.Eval(Container.DataItem, "DiscrepancyID") %>' />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:Literal runat="server" Visible="false" ID="ltLineNumber" Text='<%# DataBinder.Eval(Container.DataItem, "LineNumber") %>' />
                                                                <asp:Literal runat="server" Visible="false" ID="ltDiscrepancyID" Text='<%# DataBinder.Eval(Container.DataItem, "DiscrepancyID") %>' />
                                                                <asp:TextBox runat="server" ID="txtLineNumber" EnableTheming="false" CssClass="DescPOLineNumber" Text='<%# DataBinder.Eval(Container.DataItem, "LineNumber") %>' Width="85" />
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>







                                                        <asp:TemplateField ItemStyle-Width="70" HeaderText= "<%$Resources:Resource,InvUoM%>">
                                                            <ItemTemplate>
                                                                <asp:Literal runat="server" ID="ltUoM" Text='<%# DataBinder.Eval(Container.DataItem, "UoM") %>' />
                                                            </ItemTemplate>

                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-Width="100" HeaderText= "<%$Resources:Resource,InvoiceQty%>" >
                                                            <ItemTemplate>
                                                                <asp:Literal runat="server" ID="ltInvoiveQty" Text='<%# DataBinder.Eval(Container.DataItem, "InvoiceQuantity") %>' />
                                                            </ItemTemplate>

                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-Width="100" HeaderText= "<%$Resources:Resource,ExcessStorageQty%>" >
                                                            <ItemTemplate>
                                                                <asp:Literal runat="server" ID="ltReceivedQty" Text='<%# DataBinder.Eval(Container.DataItem, "ReceivedQuantity") %>' />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:RequiredFieldValidator ID="rfvtxtDReceivedQty" runat="server" ControlToValidate="txtDReceivedQty" Display="Dynamic" ErrorMessage=" * " />
                                                                <asp:TextBox ID="txtDReceivedQty" runat="server" onKeyPress="return checkDec(this,event)" Text='<%# DataBinder.Eval(Container.DataItem, "ReceivedQuantity") %>' Width="40" />
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-Width="100" HeaderText="<%$Resources:Resource,Description%>" >
                                                            <ItemTemplate>
                                                                <asp:Literal runat="server" ID="ltDamage" Text='<%# DataBinder.Eval(Container.DataItem, "DiscrepancyDescription") %>' />
                                                                <br />
                                                                <asp:Literal ID="ltDiscAtatchment" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                 Description
                                                                                        <br />
                                                                <asp:TextBox ID="txtDamage" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DiscrepancyDescription") %>' Width="150" />
                                                                <br />
                                                                Attach Picture<br />
                                                                <asp:FileUpload ID="descfile_upload" runat="server" onchange="return checkFileExtension(this);" class="" />
                                                                <br />

                                                                <asp:Literal ID="ltDiscAtatchment" runat="server"></asp:Literal>

                                                            </EditItemTemplate>
                                                        </asp:TemplateField>


                                                        <asp:TemplateField ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint" HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90" FooterText="asd">
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblGDRItemDelete" CssClass="smlText" runat="server" Text="Delete"></asp:Label>
                                                            </HeaderTemplate>
                                                            <FooterTemplate>
                                                                <asp:LinkButton ID="btnDelete" Font-Underline="false" CssClass="GvLink" Text="<nobr> <i class='material-icons ss'>delete</i></nobr>" runat="server" OnClick="btnDelete_Click" OnClientClick="return confirm('Are you sure you want to delete the selected?');"></asp:LinkButton>
                                                            </FooterTemplate>
                                                            <EditItemTemplate>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="deleteRec" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField Visible="false" DataField="EditName" ItemStyle-Font-Underline="false" ItemStyle-HorizontalAlign="Center" ControlStyle-Font-Underline="false" ReadOnly="true" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint" />
                                                        <asp:CommandField ItemStyle-Font-Underline="false" ControlStyle-Font-Underline="false" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint" ItemStyle-Width="60" ButtonType="Link" ItemStyle-HorizontalAlign="left" CancelImageUrl="icons/cancel.gif" CancelText="Cancel" EditImageUrl="icons/edit.gif" EditText="<nobr> <i class='material-icons ss'>mode_edit</i></nobr>" ShowEditButton="True" UpdateImageUrl="icons/update.gif" UpdateText="Update" />
                                                    </Columns>
                                                </asp:GridView>

                                                <br />
                                            </asp:Panel>



                                            

                                        </div>
                                    </div>

                                   <div class="col m12">
                                        <div>

                                            <div class="input-new-style_______">
                                                <div class="row">
                                                    <div class="col m3">
                                                        <div class="flex">
                                                            <asp:RequiredFieldValidator ID="rfvatcCheckedBy" runat="server" ValidationGroup="updateShipmentVerified1" ControlToValidate="atcCheckedBy" Display="Dynamic" ErrorMessage=" * " CssClass="errorMsg" />

                                                            <asp:TextBox ID="atcCheckedBy" SkinID="txt_Auto" runat="server" required="required" /><asp:HiddenField ID="hifCheckedBy" runat="server" />
                                                             <span class="errorMsg"></span>
                                                            <label> <%= GetGlobalResourceObject("Resource", "CheckedBy")%></label>
                                                        </div>
                                                    </div>
                                                    <div class="col m3">
                                                        <div class="flex">
                                                            <asp:RequiredFieldValidator ID="rfvtxtCheckedDate" runat="server" ValidationGroup="updateShipmentVerified1" ControlToValidate="txtCheckedDate" Display="Dynamic" ErrorMessage=" * " CssClass="errorMsg" />

                                                            <asp:TextBox ID="txtCheckedDate" runat="server" required="required" />
                                                            <span class="errorMsg"></span>
                                                            <label> <%= GetGlobalResourceObject("Resource", "Date")%> </label>
                                                        </div>
                                                    </div>
                                                    <div style="display:none;">
                                                        <div class="FormLabels">
                                                            <p> <%= GetGlobalResourceObject("Resource", "Sign")%> </p>
                                                        </div>
                                                        <div height="50">&nbsp;</div>
                                                    </div>
                                                    <div class="col m3">
                                                        <div class="flex">
                                                            <asp:RequiredFieldValidator ID="rfvtxtVerifiedBy" SkinID="txt_Auto" runat="server" ValidationGroup="updateShipmentVerified1" ControlToValidate="txtVerifiedBy" Display="Dynamic" ErrorMessage=" * " CssClass="errorMsg" />

                                                            <asp:TextBox ID="txtVerifiedBy" runat="server" required="required" /><asp:HiddenField ID="hifVerifiedBy" runat="server" />
                                                             <span class="errorMsg"></span>
                                                            <label> <%= GetGlobalResourceObject("Resource", "VerifiedBy")%> </label>
                                                        </div>
                                                    </div>
                                                    
                                                    <div class="col m3">
                                                        <div class="flex">
                                                            <asp:RequiredFieldValidator ID="rfvtxtVerifiedDate" runat="server" ValidationGroup="updateShipmentVerified1" ControlToValidate="txtVerifiedDate" Display="Dynamic" ErrorMessage=" * " CssClass="errorMsg" />

                                                            <asp:TextBox ID="txtVerifiedDate" runat="server" required="required" />
                                                             <span class="errorMsg"></span>
                                                            <label>  <%= GetGlobalResourceObject("Resource", "Date")%></label>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col m3">
                                                        <div class="flex">
                                                            <asp:TextBox ID="txtDiscRemarks"  TextMode="MultiLine" CssClass="txt_slim_green" runat="server" required="required" />
                                                            <label>  <%= GetGlobalResourceObject("Resource", "Remarks")%> </label>
                                                        </div>
                                                    </div>
                                                    <div class="col m9">
                                                        
                                                   
                                                        <div flex end>
                                                            <asp:LinkButton runat="server" ID="lnkdescsubmit" OnClick="lnkdescsubmit_Click" ValidationGroup="updateShipmentVerified1"  CssClass="btn btn-sm btn-primary">
                                                          <%= GetGlobalResourceObject("Resource", "Save")%>   <%=MRLWMSC21Common.CommonLogic.btnfaSave %>
                                                            </asp:LinkButton>
                                                      </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                                    </div>
                            </asp:Panel>

                        </div>
                    </div>
                </div>
                <div id="accordion8" class="accordion">
                    <h3> <%= GetGlobalResourceObject("Resource", "ShipmentVerification")%> </h3>
                    <div class="Verification">  

                            <asp:UpdateProgress ID="uprgShipmentVerification" runat="server" AssociatedUpdatePanelID="upnlShipmentVerification">
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
                            </asp:UpdateProgress>
                            <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" ID="upnlShipmentVerification" UpdateMode="Conditional">
                                <ContentTemplate>

                                    <asp:Panel ID="pnlshipmentVerification" runat="server" CssClass="FormLabels" ClientIDMode="Static" Visible="false" ScrollBars="Auto" HorizontalAlign="left">


                                        <div>

                                            <div class="FormLabelsBlue flex">
                                                <div style="display:none;" class="">
                                                     <p><b> <%= GetGlobalResourceObject("Resource", "DiscrepancyDetails")%></b></p> 
                                                </div>
                                   

                                                <div class="f-flex form right">
                                                    <asp:Label runat="server" ID="lblVerfShipmentReceivedWH" CssClass="SubHeading3"></asp:Label>
                                                </div>
                                            </div> <br /><br />
                                        </div>
                                         <div style="display:none;" >
                                                <div class="divlineheight"></div>
                                                <div class="row" style="margin: 0;">
                                                    <div class="col-sm-6 col-lg-6" style="overflow: auto; margin: 15px; padding: 0;">
                                                        <table class="mytableOutbound" style="width: 100% !important; margin-left: 0 !important;">
                                                            <thead class="mytableOutboundHeaderTR">
                                                                <tr style="height: 30px;">
                                                                    <th> </th>
                                                                    <th> <%= GetGlobalResourceObject("Resource", "PONumber")%> </th>
                                                                    <th> <%= GetGlobalResourceObject("Resource", "TotalLineItems")%></th>
                                                                    <th><%= GetGlobalResourceObject("Resource", "DISCREPANCYStatus")%></th>
                                                                    <th> <%= GetGlobalResourceObject("Resource", "TotalInvoiceQty")%></th>
                                                                    <th> <%= GetGlobalResourceObject("Resource", "TotalPendingQty")%></th>
                                                                    
                                                                </tr>
                                                            </thead>
                                                            <tbody ng-repeat="DS in descrepancy"  align="center" class="mytableOutboundBodyTR">
                                                                <tr>
                                                                    <td><input  type="checkbox" ng-model="DS.Isselected" ng-onclick="";  ng-change="GetInfo(DS.PoHeaderID,DS.Isselected,DS.InboundiD)" id="Checkbox1"  /></td>
                                                                    <td>{{DS.PONumber}}</td>
                                                                    <td>{{DS.LinenumberCount}}</td>
                                                                    <td>{{DS.Discrepancystatus}}</td>
                                                                    <td>{{DS.TotalInvoiceQty}}</td>
                                                                    <td>{{DS.TotalPendingQty}}
                                                                      
                                                                       
                                                                    </td>
                                                                    
                                                                </tr>
                                                               
                                                                <tr class="trPO1" ng-if="DS.Isselected">
                                                                    <td colspan="4" align="center">
                                                                        <table class="mytableOutbound" style="width: 98% !important;">
                                                                            <thead>
                                                                                <tr class="mytableOutboundchildHeaderTR">
                                                                                    <th> <%= GetGlobalResourceObject("Resource", "InvoiceNumber")%></th>
                                                                                    <th> <%= GetGlobalResourceObject("Resource", "PartNumber")%></th>
                                                                                    <th><%= GetGlobalResourceObject("Resource", "LineNumber")%></th>
                                                                                    <th><%= GetGlobalResourceObject("Resource", "Invoice Qty")%></th>
                                                                                    <th><%= GetGlobalResourceObject("Resource", "Receivedqty")%></th>
                                                                                    <th><%= GetGlobalResourceObject("Resource", "Pendingqty")%></th>

                                                                                </tr>
                                                                            </thead>
                                                                            <tbody>
                                                                                <tr  ng-repeat="PO in DS.discrepancylineitem" class="mytableOutboundBodyTR">
                                                                                    <td >{{ PO.SupplierInvoice }}</td>
                                                                                    <td>{{PO.MCode}}</td>
                                                                                    <td >{{PO.LineNumber}}</td>
                                                                                    <td >{{PO.InvoiceQty}}</td>
                                                                                    <td >{{PO.ReceivedQty}}</td>
                                                                                    <td >{{PO.Pendingqty}} </td>
                                                                                </tr>

                                                                            </tbody>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                     <div class="divlineheight"></div>
                                                </div>
                                            </div>
                                        <div>

                                            <div>
                                                <div  class="FormLabels">

                                                    <asp:Label ID="lblLocalerror2" runat="server" CssClass="ErrorMsg" />

                                                </div>
                                            </div>
                                            <div class="row">
                                               <div class="col m3" style="display: none;">
                                                   

                                                    <div>

                                                        <%--<asp:UpdatePanel ID="UpdatePanelShipmentReceived" UpdateMode="Conditional" runat="server" ChildrenAsTriggers="true">


                                                            <ContentTemplate>

                                                                <asp:LinkButton runat="server" ID="lnkViewPendingGoodsINList" SkinID="lnkButEmpty" Text="View Pending Goods-IN List" Font-Underline="false" OnClick="lnkViewPendingGoodsINList_Click"></asp:LinkButton>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>--%>

                                                        <asp:UpdatePanel ID="UpdatePanelShipmentReceived" UpdateMode="Conditional" runat="server" ChildrenAsTriggers="true">


                                                            <ContentTemplate>

                                                                <asp:LinkButton runat="server" ID="lnkViewPendingGoodsINList" SkinID="lnkButEmpty" Visible="false" Text="View Pending Goods-IN List" Font-Underline="false" OnClick="lnkViewPendingGoodsINList_Click">
                                                                    <asp:Image ID="Image1" runat="server" ImageUrl="../Images/redarrowright.gif" style="margin-left: 3px;" />
                                                                </asp:LinkButton>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                        
                                                        <br />
                                                    </div>
                                                </div>

                                                <div class="col m3">
                                                    
                                                    <div class=" flex">
                                           
                                                            <asp:RequiredFieldValidator ID="rfvtxtShipmentVerifiedDate" SetFocusOnError="true" ControlToValidate="txtShipmentVerifiedDate" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage="Shipment verification date is required" runat="server" ValidationGroup="updateShipmentVerified" />
                                                            <span class="errorMsg">* </span>
                                                            <asp:TextBox ID="txtShipmentVerifiedDate"  runat="server" required=""/><label>Shipment Verification Date</label>
                                                    </div>
                                                </div>

                                                <div class="col m3">
                                                    
                                                    <div class="FormLabels flex">
                                        
                                                        <div class="f-flex form"><asp:UpdatePanel ChildrenAsTriggers="true" ID="upnlVerificationSheet" runat="server" UpdateMode="Conditional">

                                                            <Triggers>
                                                                <asp:PostBackTrigger ControlID="lnkUpdateShipmentVerified" />
                                                            </Triggers>
                                                            <ContentTemplate>
                                                                 
                                                                <asp:FileUpload runat="server" ID="fileupShipverificationNote" CssClass="" />
                                                               
                                                                <div id="divShpVeriNote" runat="server"></div>
                                                               <label style="top: -17px;"><%= GetGlobalResourceObject("Resource", "AttachVerificationSheet")%></label>

                                                            </ContentTemplate>
                                                        </asp:UpdatePanel></div>



                                                    </div>
                                                </div>

                                                <div class="col m3">
                                                    <div  class="FormLabels flex">
                                                       <div class="f-flex"> </div>
                                                        <div class="flex form"> <asp:TextBox ID="txtVerificationRemarks" Width="100%" required=""  TextMode="MultiLine" CssClass="txt_slim" runat="server"/>
                                                           <label > <%= GetGlobalResourceObject("Resource", "Remarks")%></label>
                                                            </div>
                                                    </div>
                                                </div>
                                            
                                                <div class="col m3">
                                                    <div class="right flex__">
                                                      <div style="display:none;" class="checkbox">
                                                        <input id="cbisdesc" runat="server" visible="false" type="checkbox" />  <label for="cbisdesc">&nbsp;&nbsp;Is Discrepancy</label>
                                                        </div>&nbsp;&nbsp;&nbsp;&nbsp;
                                                  <div class="flex__">  <asp:LinkButton ID="lnkCancelVerified" runat="server" OnClick="lnkCancelVerified_Click" CssClass="btn btn-sm btn-primary">
                                                          <%= GetGlobalResourceObject("Resource", "Clear")%> <%=MRLWMSC21Common.CommonLogic.btnfaClear %>
                                                    </asp:LinkButton>
                                                      &nbsp;&nbsp;
                                                        <asp:LinkButton ID="lnkUpdateShipmentVerified" CausesValidation="True" OnClientClick="Panel_ValidationActive=true;" ValidationGroup="updateShipmentVerified" runat="server" OnClick="lnkUpdateShipmentVerified_Click"
                                                            CssClass="btn btn-sm btn-primary">
                                                             <%= GetGlobalResourceObject("Resource", "Verify")%>  <%=MRLWMSC21Common.CommonLogic.btnfaSave %>
                                                        </asp:LinkButton></di>
                                                    </div>

                                                    <%-- <asp:ValidationSummary ID="ValidationSummary1" DisplayMode="List" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="updateShipmentVerified" ForeColor="red" Font-Bold="true" />--%>
                                        
                                                </div>
                                            </div>

                                        </div>


                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            
                    </div>
                </div>
            </div>
        </div>

    </div>
    
                <%--GRN pop up Begins--%>

    <div id="disputeDivSupplierItemList1">
         <div id="divSupplierItemList1"  >
             
                <asp:UpdateProgress ID="uprgSupplierItemListDialog" runat="server" AssociatedUpdatePanelID="upnlSupplierItemListDialog">
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
                </asp:UpdateProgress>
                <asp:UpdatePanel ID="upnlSupplierItemListDialog" ChildrenAsTriggers="true" runat="server"  UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="ui-dailog-body" style="height:395px;overflow:auto;">
                            <div>

                                <div>
                                    <div>

                                        <%--<div id="polineContainer" runat="server"></div>--%>


                                            <%--<asp:GridView  Width="100%" ShowFooter="true" ID="GVPOLineItems" DataKeyNames="LineNumber" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="Bottom" AutoGenerateColumns="False" AllowPaging="true" PageSize="3" SkinID="gvLightGrayNew" HorizontalAlign="Left" OnPageIndexChanging="GVPOLineItems_PageIndexChanging">--%>
                                             <asp:GridView  Width="100%" ShowFooter="true" ID="GVPOLineItems" DataKeyNames="LineNumber" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="Bottom" AutoGenerateColumns="False" SkinID="gvLightGrayNew" HorizontalAlign="Left">   
                                        <Columns>

                                                    <asp:TemplateField ItemStyle-Width="150" HeaderText= "<%$Resources:Resource,PartNo%>" ItemStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Center">

                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltMcode" Text='<%# DataBinder.Eval(Container.DataItem, "MCode") %>' />
                                                            <asp:Literal runat="server" Visible="false" ID="ltMaterialMasterID" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialMasterID") %>' />
                                        
                                                        </ItemTemplate>

                                                    </asp:TemplateField>

                                                    <asp:TemplateField ItemStyle-Width="150" HeaderText= "<%$Resources:Resource,LineNumber%>"  ItemStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Center">

                                                        <ItemTemplate>
                                                            <asp:Literal runat="server"  ID="ltLineNumber" Text='<%# DataBinder.Eval(Container.DataItem, "LineNumber") %>' />
                                                        </ItemTemplate>

                                                    </asp:TemplateField>

                                                    <asp:TemplateField ItemStyle-Width="150" HeaderText= "<%$Resources:Resource,Quantity%>"  ItemStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Center">

                                                        <ItemTemplate>
                                                            <asp:Literal runat="server"  ID="ltQuantity" Text='<%# DataBinder.Eval(Container.DataItem, "Quantity") %>' />
                                                        </ItemTemplate>

                                                    </asp:TemplateField>

                                                    <asp:TemplateField ItemStyle-Width="50"  ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint"  FooterStyle-CssClass="NoPrint">
                                                      <HeaderTemplate>
                                                          <%--<asp:CheckBox ID="selectAll" runat="server" OnCheckedChanged="selectAll_CheckedChanged" />--%>
                                                          <input type="checkbox" id="action" onchange="selectAll()"  runat="server"/>
                                                      </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <%--<asp:CheckBox ID="chkIsDelete" runat="server" Checked='<%#Convert.ToBoolean(Eval("Active"))%>' />--%>
                                                           <%-- <asp:CheckBox ID="chkIsDelete" runat="server" EnableViewState="true" EnablePersistedSelection="true" class="allcheck"/>--%>
                                                            <input type="checkbox" id="chkIsDelete" class="allcheck" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                </Columns>
                                            </asp:GridView>

                                    </div>
                                </div>

                            </div>
                        </div>
                        <div class="ui-dailog-footer">
                            <%--  // ========== Commented by M.D.Prasad ================= //<div style="padding: 15px 13px 15px 5px;">
                                1//<asp:LinkButton ID="lnkInvclose"  ClientIDMode="Static" CssClass="btn btn-sm btn-primary"  runat="server" OnClientClick="closeDialog1();">
                                    Save<%=MRLWMSC21Common.CommonLogic.btnfaSave %>
                                </asp:LinkButton>
                                2//<button type="button" class="btn btn-sm btn-primary" id="lnkInvclose" onclick="closeDialog1();">Save</button>
                             // ========== Commented by M.D.Prasad ================= //</div>--%>

                              <div style="padding: 15px 13px 15px 5px;">



                                  <button type="button" class="btn btn-primary" id="lnkInvclose" onclick="closeDialog1()">Save</button>


<%--                                  <asp:LinkButton ID="lnkInvclose"  ClientIDMode="Static" CssClass="btn btn-sm btn-primary"  runat="server" OnClientClick="closeDialog1();">
                                    Save<%=MRLWMSC21Common.CommonLogic.btnfaSave %>
                                </asp:LinkButton>--%>

                               <%-- <asp:LinkButton ID="lnkInvclose"  ClientIDMode="Static" CssClass="btn btn-sm btn-primary"  runat="server" OnClientClick="closeDialog1();">
                                    Save<%=MRLWMSC21Common.CommonLogic.btnfaSave %>
                                </asp:LinkButton>--%>
                                 <%--<button type="button" class="btn btn-primary" id="btnclose" onclick="closeDialog1()">Close <%= MRLWMSC21Common.CommonLogic.btnfaClear %></button>--%>
                            </div>


                        </div>    
                    </ContentTemplate>        
                </asp:UpdatePanel>
         </div>
    </div>

       </div>         <%--GRN pop up Ends--%>


  

    <!-- Goods-IN Pending List Dialog   -->

    <div id="divItemPrintDataContainer">
        <div id="divItemPrintData" style="display: block;">
            
                <asp:UpdateProgress ID="uprgGoodsIN" runat="server" AssociatedUpdatePanelID="upnlGoodsIN">
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
                </asp:UpdateProgress>
                <asp:UpdatePanel ID="upnlGoodsIN" runat="server">
                    <ContentTemplate>
                    
                        <asp:Panel ID="pnlPendingGoodsINList" runat="server" ScrollBars="Auto">

                            <br />
                            <div style="padding-left: 10px; padding-right: 10px;">

                                <asp:GridView Width="100%" ShowFooter="true" ID="gvPendingGoodsINList" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="true" PageSize="10" SkinID="gvLightGrayNew" HorizontalAlign="Left" OnRowDataBound="gvPendingGoodsINList_RowDataBound" OnPageIndexChanging="gvPendingGoodsINList_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="150" HeaderText= "<%$Resources:Resource,POLine%>">
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="ltLineNumber" Text='<%# DataBinder.Eval(Container.DataItem, "LineNumber") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="250" HeaderText= "<%$Resources:Resource,PartNumber%>" >
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="ltMCode" Text='<%# DataBinder.Eval(Container.DataItem, "MCode") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="150" HeaderText= "<%$Resources:Resource,Invoice%>" >
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="ltInvoiceNumber" Text='<%# DataBinder.Eval(Container.DataItem, "InvoiceNumber") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-Width="150" HeaderText= "<%$Resources:Resource,PONumber%>" >
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="ltPONumber" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "PONumber") %>' />

                                                <img src="../Images/redarrowleft.gif" />

                                                <asp:HyperLink ID="HyperLink1" Text='<%# DataBinder.Eval(Container.DataItem, "PONumber") %>' NavigateUrl='<%# String.Format("../mOrders/PODetailsInfo.aspx?poid={0}",DataBinder.Eval(Container.DataItem, "POHeaderID").ToString()) %>' Font-Underline="false" runat="server"></asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-Width="150" HeaderText= "<%$Resources:Resource,InvoiceQty%>" >
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="ltInvoiveQty" Text='<%# DataBinder.Eval(Container.DataItem, "InvoiceQuantity") %>' />
                                            </ItemTemplate>

                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="150" HeaderText=  "<%$Resources:Resource,PendingQty%>"  >
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="ltReceivedQty" Text='<%# DataBinder.Eval(Container.DataItem, "PendingQty") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>

                            </div>
                      
                        </asp:Panel>

                    </ContentTemplate>
                </asp:UpdatePanel>

        </div>
    </div>


    <!-- Goods-IN Pending List Dialog   -->

    <asp:HiddenField ID="hifGRNDoneBy" runat="server" />

    
    <asp:HiddenField ID="hifInboundID" runat="server" />
    <asp:HiddenField ID="hifDynaPOHeaderID" runat="server" />
    <asp:HiddenField ID="hifGRNPOHeaderID" runat="server" />
    <asp:HiddenField ID="hidAccordionIndex1" runat="server" />
    <asp:HiddenField ID="hidAccordionIndex2" runat="server" />
    <asp:HiddenField ID="hidAccordionIndex3" runat="server" />
    <asp:HiddenField ID="hidAccordionIndex4" runat="server" />
    <asp:HiddenField ID="hidAccordionIndex5" runat="server" />
    <asp:HiddenField ID="hidAccordionIndex6" runat="server" />
    <asp:HiddenField ID="hidAccordionIndex7" runat="server" />
     <asp:HiddenField ID="hidAccordionIndex8" runat="server" />
    <asp:HiddenField ID="hifDisInvNumberID" runat="server" />
    <asp:HiddenField ID="hifDisMMID" runat="server" />

    <input type="hidden" runat="server" id="hdnPOLines" />

</asp:Content>
