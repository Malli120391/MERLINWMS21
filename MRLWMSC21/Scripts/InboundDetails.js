
    $(document).ready(function () {

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

        function closeDialog1() {
        //Could cause an infinite loop because of "on close handling"
        $("#divSupplierItemList1").dialog('close');
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
            $("#divSupplierItemList1").dialog("option", "title", "PO Line Details");
            $("#divSupplierItemList1").dialog('open');

            //else
            //{
            //    alert("Please Select Po Number and Invoice #.");
            //    return false;
            //}

           <%--NProgress.start();

            $("#divSupplierItemList1").block({
                message: '<img src="<%=ResolveUrl("~") %>Images/async_order.gif" />',
                css: { border: '0px' },
                fadeIn: 0,
                fadeOut: 0,
                overlayCSS: { backgroundColor: '#ffffff', opacity: 1 }
            }); --%>            
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
            $("#accordion1").accordion({header: 'h3', collapsible: true, autoHeight: false, clearStyle: true, navigation: true });


            $("#accordion2").accordion({
                alwaysOpen: false,
                autoHeight: false, clearStyle: true,
                active: activeIndex2,
                change: function (event, ui) {
                    var index = $(this).children('h3').index(ui.newHeader);
                    $('#<%=hidAccordionIndex2.ClientID %>').val(index);
                }
            });
            $("#accordion2").accordion({header: 'h3', collapsible: true, autoHeight: false, clearStyle: true, navigation: true });


            $("#accordion3").accordion({
                alwaysOpen: false,
                autoHeight: false, clearStyle: true,
                active: activeIndex3,
                change: function (event, ui) {
                    var index = $(this).children('h3').index(ui.newHeader);
                    $('#<%=hidAccordionIndex3.ClientID %>').val(index);
                }
            });
            $("#accordion3").accordion({header: 'h3', collapsible: true, autoHeight: false, clearStyle: true, navigation: true });


            $("#accordion4").accordion({
                alwaysOpen: false,
                autoHeight: false, clearStyle: true,
                active: activeIndex4,
                change: function (event, ui) {
                    var index = $(this).children('h3').index(ui.newHeader);
                    $('#<%=hidAccordionIndex4.ClientID %>').val(index);
                }
            });
            $("#accordion4").accordion({header: 'h3', collapsible: true, autoHeight: false, clearStyle: true, navigation: true });


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
            $("#accordion5").accordion({header: 'h3', collapsible: true, autoHeight: false, clearStyle: true, navigation: true });


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
            $("#accordion8").accordion({header: 'h3', collapsible: true, autoHeight: false, clearStyle: true, navigation: true });


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
            $("#accordion6").accordion({header: 'h3', collapsible: true, autoHeight: false, clearStyle: true, navigation: true });

            $("#accordion7").accordion({
                collapseAll: false,
                alwaysOpen: false,
                autoHeight: false, clearStyle: true,
                active: activeIndex6,
                change: function (event, ui) {
                    var index = $(this).children('h3').index(ui.newHeader);
                    $('#<%=hidAccordionIndex7.ClientID %>').val(index);
                }
            });
            $("#accordion7").accordion({header: 'h3', collapsible: true, autoHeight: false, clearStyle: true, navigation: true });


            $("#accordion10").accordion({
                alwaysOpen: false,
                autoHeight: false, clearStyle: true,
                active: activeIndex8,
                change: function (event, ui) {
                    var index = $(this).children('h3').index(ui.newHeader);
                    $('#<%=hidAccordionIndex8.ClientID %>').val(index);
                }
            });
            $("#accordion10").accordion({header: 'h3', collapsible: true, autoHeight: false, clearStyle: true, navigation: true });

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

        $(document).ready(function () {
            $("#spanClose").click(function () {
                $("#divContainer").hide();
            });
            fnLoadMCode();



            $("#<%= this.txtDocRcvDate.ClientID %>").datepicker({ dateFormat: "dd/mm/yy", minDate: 0 })
            $("#<%= this.txtConsignmentNoteTypeDate.ClientID %>").datepicker({ dateFormat: "dd/mm/yy" });
            $("#<%= this.txtClearenceInvoiceDate.ClientID %>").datepicker({ dateFormat: "dd/mm/yy" });
            $("#<%= this.txtPriorityDate.ClientID %>").datepicker({ dateFormat: "dd/mm/yy", minDate: 0 });
            $("#<%= this.txtShipmentVerifiedDate.ClientID %>").datepicker({
                dateFormat: "dd/mm/yy",
                minDate: new Date()

            });
            $("#<%= this.txtFreightInvoiceDate.ClientID%>").datepicker({ dateFormat: "dd/mm/yy" });
            $("#<%= this.txtShipmentReceivedDate.ClientID%>").datepicker({
                dateFormat: "dd/mm/yy"
            });
            $(".DynaGRNDate").datepicker({ dateFormat: "dd/mm/yy", minDate: 0 });
            $("#<%= this.txtTimeEntry.ClientID %>").timeEntry();

            $("#<%= this.txtShipmentExpectedDate.ClientID %>").datepicker({ minDate: 0, dateFormat: "dd/mm/yy" });

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
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadTenantsForWHList") %>',
                        data: "{ 'prefix': '" + request.term + "','AccountID':'" + $('#<%=this.hdnAccount.ClientID%>').val() + "'}",//<=cp.TenantID%>
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
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadWHForWHList") %>',
                        data: "{ 'prefix': '" + request.term + "','TenantID':'" + $('#<%=this.hidTenantID.ClientID%>').val() + "'}",//<=cp.TenantID%>
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
                    }
                    else {
                        //document.getElementById("trConsignmentNoteType").style.display = "table-row !important";
                        //document.getElementById("trClearanceCompany").style.display = "table-row !important";
                        $("#MainContent_IBContent_UpdatePanel1").css("display", "block");
                        $("#MainContent_IBContent_UpdatePanel2").css("display", "block");
                        $("#MainContent_IBContent_UpdatePanel3").css("display", "block");
                    }
                },
                minLength: 0
            });

            var textfieldname = $('#<%=txtdock.ClientID%>');
            DropdownFunction(textfieldname);
            $("#<%= this.txtdock.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadDockFor3PL") %>',
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
        });
        $(document).ready(function () {

            if (document.getElementById('<%= this.hifShipmentType.ClientID %>').value == "2" || document.getElementById('<%= this.hifShipmentType.ClientID %>').value == "6") {
                document.getElementById("trConsignmentNoteType").style.display = "none !important";
                document.getElementById("trClearanceCompany").style.display = "none !important";
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
            $(".DynaGRNDate").datepicker({ dateFormat: "dd/mm/yy", minDate: 0 });
            $("#<%= this.txtShipmentExpectedDate.ClientID %>").datepicker({ minDate: 0, dateFormat: "dd/mm/yy" });
            $("#<%= this.txtOffLoadingTime.ClientID %>").timeEntry();
            $("#<%= this.txtConsignmentNoteTypeDate.ClientID %>").datepicker({ dateFormat: "dd/mm/yy" });
            $("#<%= this.txtCheckedDate.ClientID%>").datepicker({
                dateFormat: "dd/mm/yy",
                minDate: new Date()
            });

            $("#<%= this.txtVerifiedDate.ClientID%>").datepicker({
                dateFormat: "dd/mm/yy",
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
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetDataForActivityName") %>',
                        data: "{ 'prefix': '" + request.term + "', 'ActivityRateTypeID': '" + $("#hifActivityRateTypeID").val() + "' }",
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
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetDataForActivityName") %>',
                        data: "{ 'prefix': '" + request.term + "', 'ActivityRateTypeID': '" + $("#hifRcvActivityRateTypeID").val() + "' }",
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
