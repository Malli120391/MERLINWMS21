<%@ Page Title=" Outbound Details :. " Language="C#" MasterPageFile="~/mOutbound/OutboundMaster.master" AutoEventWireup="true" CodeBehind="OutboundDetails.aspx.cs" Inherits="MRLWMSC21.mOutbound.OutboundDetails" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="OBContent" runat="server">

    <asp:ScriptManager runat="server" ID="scriptMngrROBD" EnablePartialRendering="true" SupportsPartialRendering="true"></asp:ScriptManager>


    <script type="text/javascript" src="Scripts/CommonScripts.js"></script>
    <script type="text/javascript" src="../Scripts/timeentry/jquery.timeentry.js"></script>
    <script type="text/javascript" src="Scripts/CommonScripts.js"></script>
    <link href="../CSS/bootstrap.min.css" rel="stylesheet" />
    <script src="../mInventory/Scripts/bootstrap.min.js"></script>

    <%--<script src="../Scripts/mdtimepicker.js"></script>
    <link href="../Scripts/mdtimepicker.css" rel="stylesheet" />--%>

    <script type="text/javascript">

        $(function () {

            var activeIndex = parseInt($('#<%=hidAccordionIndex.ClientID %>').val());

            $("#accordion").accordion({
                autoHeight: false, clearStyle: true,
                active: activeIndex,
                change: function (event, ui) {
                    var index = $(this).children('h3').index(ui.newHeader);
                    $('#<%=hidAccordionIndex.ClientID %>').val(index);
                }

            });
            $("#accordion").accordion({ header: 'h3', collapsible: true, autoHeight: false, clearStyle: true, navigation: true });



        });

    </script>
    <script type="text/javascript">
        var PackingSliptable = "";
        var ItemMasterList = "";
        var PackingItemList = "";
        var Mvolume = "";
        var MWeight = "";
        var TotpackedQty = "";
        function GetSelectedTextValue(txtPSNMaterial, txtPSNMaterialselectedText) {
            var selectedText = txtPSNMaterialselectedText;
            var selectedValue = txtPSNMaterial;
            if (selectedValue != 0) {
                //alert("Selected Text: " + selectedText + " Value: " + selectedValue);

                $.ajax({
                    url: '<%=ResolveUrl("~/mOutbound/OutboundDetails.aspx/GetPSNMaterial") %>',
                    data: "{'Mcode':'" + selectedText + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",

                    success: function (response) {
                        debugger;
                        // alert('123');
                        if (response.d.length > 0) {
                            var pickqty = JSON.parse(response.d);
                            $("#<%= txtPickedQty.ClientID %>").val(pickqty[0].PickedQty);
                            $("#<%= txtPackedQty.ClientID %>").val(pickqty[0].PickedQty);
                         <%-- $("#<%= txtPickedUOM.ClientID %>").val(pickqty[0].UOM);--%>
                            $("#txtPickedUOM").val(pickqty[0].UOM);
                            Mvolume = pickqty[0].MVolume;
                            MWeight = pickqty[0].MWeight;
                            TotpackedQty = pickqty[0].TotPackedQty;
                        }
                        //setTimeout(function () {
                        //    location.reload();
                        //    showStickyToast(true, "Packing Slip Information Saved Successfully", false);
                        //}, 3000);
                    }
                })
            }
        }
    </script>
    <style type="text/css">
        .ui-autocomplete-loading {
            background: white url('../Images/ui-anim_basic_16x16.gif') right center no-repeat;
        }

        .PrintListcontainer {
            background-color: #f5ec8a;
        }


        .ChargesStatus {
            color: Steelblue;
            font-weight: bold;
            font-size: 15px;
        }

        .ui-widget-content a {
            margin-left: 5px;
        }

    </style>

    <script type="text/javascript">
        $(function () {
            $("#<%= this.txtPriorityDate.ClientID%>").datepicker({ dateFormat: "dd-M-yy" });
            $("#<%= this.txtGDRDate.ClientID%>").datepicker({ dateFormat: "dd-M-yy" });
            $("#<%= this.txtInvoiceDate.ClientID%>").datepicker({ dateFormat: "dd-M-yy" });
            $("#<%= this.txtOBDRcvdDate.ClientID%>").datepicker({ dateFormat: "dd-M-yy" });
            $("#<%= this.txtPGIDate.ClientID%>").datepicker({ dateFormat: "dd-M-yy" });
            $("#<%= this.txtDocRcvdDate.ClientID%>").datepicker({ dateFormat: "dd-M-yy" });
            $("#<%= this.txtDeliveryDate.ClientID%>").datepicker({ dateFormat: "dd-M-yy" });
            $("#<%= this.txtPackedOn.ClientID%>").datepicker({ dateFormat: "dd-M-yy" });
            <%--  $("#<%= this.txtTimeEntry.ClientID%>","#<%= this.OBDRecvdTimeEntry.ClientID%>","#<%= this.txtPGITimeEntry.ClientID%>").mdtimepicker();
 --%>         $("#<%= this.txtTimeEntry.ClientID%>").timeEntry();
            $("#<%= this.OBDRecvdTimeEntry.ClientID%>").timeEntry();
            $("#<%= this.txtPGITimeEntry.ClientID%>").timeEntry();
            $("#<%= this.txtAutoPGITime.ClientID%>").timeEntry();
            $("#<%= this.txtReceivedDelTimeEntry.ClientID%>").timeEntry();
            $("#<%= this.txtPackedOnTime.ClientID%>").timeEntry();
            $("#<%= this.txtOBDDate.ClientID %>").datepicker({ minDate: 0, dateFormat: "dd-M-yy" });
            $("#<%= this.txtAutoPGIDate.ClientID%>").datepicker({ dateFormat: "dd-M-yy" });
            <%-- $("#<%= this.txtPGIDate.ClientID%>").keypress(function () {
                 return false;
             });--%>

            //$('#MainContent_IBContent_txtTimeEntry').mdtimepicker();
        });
    </script>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                fnLoadAutocompletes();
            }
        }

        function fnLoadAutocompletes() {
           <%-- $('#<%= this.txtOBDDate.ClientID%>').attr('readonly','readonly'); --%>
            $("#<%= this.txtOBDDate.ClientID%>").datepicker({ dateFormat: "dd-M-yy" });
       <%--     $('#<%= this.txtPriorityDate.ClientID%>').attr('readonly','readonly'); --%>
            $("#<%= this.txtPriorityDate.ClientID%>").datepicker({ dateFormat: "dd-M-yy" });
            $("#<%= this.txtGDRDate.ClientID%>").datepicker({ dateFormat: "dd-M-yy" });
            $("#<%= this.txtInvoiceDate.ClientID%>").datepicker({ dateFormat: "dd-M-yy" });
            $("#<%= this.txtOBDRcvdDate.ClientID%>").datepicker({ dateFormat: "dd-M-yy" });
            $("#<%= this.txtPGIDate.ClientID%>").datepicker({ dateFormat: "dd-M-yy" });
            $("#<%= this.txtAutoPGIDate.ClientID%>").datepicker({ dateFormat: "dd-M-yy" });
            $("#<%= this.txtDocRcvdDate.ClientID%>").datepicker({ dateFormat: "dd-M-yy" });
            $("#<%= this.txtDeliveryDate.ClientID%>").datepicker({ dateFormat: "dd-M-yy" });
          <%--  $('#<%= this.txtPackedOn.ClientID%>').attr('readonly','readonly'); --%>
        <%--    $("#<%= this.txtTimeEntry.ClientID%>").mdtimepicker();
            $("#<%= this.txtPackedOnTime.ClientID%>").mdtimepicker();
               $("#<%= this.txtPGITimeEntry.ClientID%>").mdtimepicker();
             $("#<%= this.OBDRecvdTimeEntry.ClientID%>").mdtimepicker();--%>
            $("#<%= this.txtPackedOn.ClientID%>").datepicker({ dateFormat: "dd-M-yy" });
            $("#<%= this.txtTimeEntry.ClientID%>").timeEntry();
            $("#<%= this.OBDRecvdTimeEntry.ClientID%>").timeEntry();
            $("#<%= this.txtPGITimeEntry.ClientID%>").timeEntry();
            $("#<%= this.txtAutoPGITime.ClientID%>").timeEntry();
            $("#<%= this.txtReceivedDelTimeEntry.ClientID%>").timeEntry();
            $("#<%= this.txtPackedOnTime.ClientID%>").timeEntry();
            $("#<%= this.OBDRecvdTimeEntry.ClientID%>").timeEntry();
            $("#<%= this.txtOBDDate.ClientID%>").keypress(function () {
                return false;
            });
            $("#<%= this.txtPriorityDate.ClientID%>").keypress(function () {
                return false;
            });
            $("#<%= this.txtPackedOn.ClientID%>").keypress(function () {
                return false;
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

                    $("#hifTenant").val(i.item.val);
                    //alert($("#hifTenant").val());
                    //LoadStoresAssociated($("#hifTenant").val());
                },
                minLength: 0
            });

            debugger;
            var OBDMaterialID = new URL(window.location.href).searchParams.get("obdid");
            var TextFieldMaterial = $("#<%= this.txtPSNMaterial.ClientID %>");
            DropdownFunction(TextFieldMaterial);
            $("#<%= this.txtPSNMaterial.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadPSNMaterialItems") %>',
                        data: "{ 'prefix': '" + request.term + "','OBDID': '" + OBDMaterialID + "'}",//<=cp.MaterialId%> 'MaterialID': '" + $('#hdnMaterial').val() + "',
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            if (data.d.length == 0) {
                                showStickyToast(false, "No Material found", false);
                                TextFieldMaterial.empty();
                            }
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
                    $("#hdnMaterial").val(i.item.val);
                    $("#<%= this.txtPackedUOM.ClientID %>").val("");
                    debugger;
                    GetSelectedTextValue(i.item.val, i.item.label);

                },
                minLength: 0
            });


            debugger;
            var TextFieldUOM = $("#<%= this.txtPackedUOM.ClientID %>");
            DropdownFunction(TextFieldUOM);
            $("#<%= this.txtPackedUOM.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadUOMs") %>',
                        data: "{'prefix':'" + request.term + "'}",//<=cp.MaterialId%> 'MaterialID': '" + $('#hdnMaterial').val() + "',
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            if (data.d.length == 0) {
                                showStickyToast(false, "No UOM found", false);
                                TextFieldUOM.empty();
                            }
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split(',')[0],
                                    val: item.split(',')[1]
                                }
                            }))
                        }
                    });
                },
                minLength: 0
            });


            var TextFieldPC = $("#<%= this.txtPkgCondition.ClientID %>");
            DropdownFunction(TextFieldPC);
            debugger;
            $("#<%= this.txtPkgCondition.ClientID %>").autocomplete({
                source: function (request, response) {
                    debugger;
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadPackageConditions") %>',
                        data: "{'prefix':'" + request.term + "'}",//<=cp.MaterialId%> 'MaterialID': '" + $('#hdnMaterial').val() + "',
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            if (data.d.length == 0) {
                                showStickyToast(false, "Package Conditions not found", false);
                                TextFieldPC.empty();
                            }
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split(',')[0],
                                    val: item.split(',')[1]
                                }
                            }))
                        }
                    });
                },
                minLength: 0
            });

            var TextFieldNameSA = $("#<%= this.txtStoresAssociated.ClientID %>");
            DropdownFunction(TextFieldNameSA);
            $("#<%= this.txtStoresAssociated.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadWarehousesDataFor3PL") %>',
                        data: "{ 'prefix': '" + request.term + "','TenantID': '" + $('#hifTenant').val() + "'}",//<=cp.TenantID%>
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            if (data.d.length == 0) {
                                showStickyToast(false, "No stores found", false);
                                TextFieldNameSA.empty();
                            }
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
                    $("#hdnStoresAssociated").val(i.item.val);
                },
                minLength: 0
            });



            var TextFieldName1 = $("#<%= this.atcRequestedBy.ClientID %>");
            DropdownFunction(TextFieldName1);
            $("#<%= this.atcRequestedBy.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadUsersData2") %>',
                        data: "{ 'prefix': '" + request.term + "', 'TenantID': '" + '<%= ViewState["TenantID"] %>' + "','WarehouseID':'"+$("#<%= this.ddlStores.ClientID %>").val()+"'}",
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
                        //error: function (response) {

                        //},
                        //failure: function (response) {

                        //}
                    });
                },
                select: function (e, i) {
                   // $("#<%=hifRequestedBy.ClientID %>").val(i.item.val);
                    $("#hifRequestedBy").val(i.item.val);
                },
                minLength: 0
            });
          <%--  var textfieldname = $("#<%= this.atcRequestedBy.ClientID %>");
            DropdownFunction(textfieldname);--%>



            $("#<%= this.txtPackedBy.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                         url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadUsersData2") %>',
                        data: "{ 'prefix': '" + request.term + "', 'TenantID': '" + '<%= ViewState["TenantID"] %>' + "','WarehouseID':'"+$("#<%= this.ddlStores.ClientID %>").val()+"'}",
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
                    $("#<%=hifPackedByID.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });
            var textfieldname = $("#<%= this.txtPackedBy.ClientID %>");
            DropdownFunction(textfieldname);



            $("#<%= this.atcPGIRequestedBy.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadUsersData2") %>',
                        data: "{ 'prefix': '" + request.term + "', 'TenantID': '" + '<%= ViewState["TenantID"] %>' + "','WarehouseID':'"+$("#<%= this.ddlStores.ClientID %>").val()+"'}",
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
                    $("#<%=HifPGIRequestedByID.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });
            var textfieldname = $("#<%= this.atcPGIRequestedBy.ClientID %>");
            DropdownFunction(textfieldname);

            $("#<%= this.atcDeliveredBy.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadUsersData2") %>',
                        data: "{ 'prefix': '" + request.term + "', 'TenantID': '" + '<%= ViewState["TenantID"] %>' + "','WarehouseID':'"+$("#<%= this.ddlStores.ClientID %>").val()+"'}",
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
                    $("#<%=hifDeliveredByID.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });
            var textfieldname = $("#<%= this.atcDeliveredBy.ClientID %>");
            DropdownFunction(textfieldname);



            $("#<%= this.atcPickedBy.ClientID %>").autocomplete({
                source: function (request, response) {
                    debugger;
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadUsersData2") %>',
                        data: "{ 'prefix': '" + request.term + "', 'TenantID': '" + '<%= ViewState["TenantID"] %>' + "','WarehouseID':'"+$("#<%= this.ddlStores.ClientID %>").val()+"'}",
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
                    $("#<%=hifPickedByID.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });
            var textfieldname = $("#<%= this.atcPickedBy.ClientID %>");
            DropdownFunction(textfieldname);

            $("#<%= this.atcCheckedBy.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadUsersData2") %>',
                        data: "{ 'prefix': '" + request.term + "', 'TenantID': '" + '<%= ViewState["TenantID"] %>' + "','WarehouseID':'"+$("#<%= this.ddlStores.ClientID %>").val()+"'}",
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
                    $("#<%=hifCheckedByID.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });
            var textfieldname = $("#<%= this.atcCheckedBy.ClientID %>");
            DropdownFunction(textfieldname);


            $("#<%= this.atcGDRRequestedBy.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                         url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadUsersData2") %>',
                        data: "{ 'prefix': '" + request.term + "', 'TenantID': '" + '<%= ViewState["TenantID"] %>' + "','WarehouseID':'"+$("#<%= this.ddlStores.ClientID %>").val()+"'}",
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
                    $("#<%=hifGDRRequestedByID.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });

            var textfieldname = $("#<%= this.atcGDRRequestedBy.ClientID %>");
            DropdownFunction(textfieldname);

            $("#<%= this.txtGDRApprovedBy.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadUsersData2") %>',
                        data: "{ 'prefix': '" + request.term + "', 'TenantID': '" + '<%= ViewState["TenantID"] %>' + "','WarehouseID':'"+$("#<%= this.ddlStores.ClientID %>").val()+"'}",
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
                    $("#<%=hifGDRApprovedByID.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });

            var textfieldname = $("#<%= this.txtGDRApprovedBy.ClientID %>");
            DropdownFunction(textfieldname);


            $("#<%= this.txtGDRPreparedBy.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                       url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadUsersData2") %>',
                        data: "{ 'prefix': '" + request.term + "', 'TenantID': '" + '<%= ViewState["TenantID"] %>' + "','WarehouseID':'"+$("#<%= this.ddlStores.ClientID %>").val()+"'}",
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
                    $("#<%=hifGDRPreparedByID.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });
            var textfieldname = $("#<%= this.txtGDRPreparedBy.ClientID %>");
            DropdownFunction(textfieldname);







            $("#<%= this.atcCustomerName.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadSOCustomerNames") %>',
                        data: "{ 'prefix': '" + request.term + "', 'TenantID': '" + $("#hifTenant").val() + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            if (data.d == "" || data.d == ",") {
                                alert("No customer is available");
                                document.getElementById('<%=atcCustomerName.ClientID %>').focus();
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
                    $("#<%=hifCustomerID.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });
            var textfieldname = $("#<%= this.atcCustomerName.ClientID %>");
            DropdownFunction(textfieldname);


            $("#<%= this.atcDepartment.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadDepartment") %>',
                        data: "{ 'prefix': '" + request.term + "','TenentID': '" + '<%=  ViewState["TenantID"] %>' + "'}",
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

                    $("#<%=hifDepartmentID.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });
            var textfieldname = $("#<%= this.atcDepartment.ClientID %>");
            DropdownFunction(textfieldname);

            $("#<%= this.atcDivision.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadDivision") %>',
                        data: "{ 'prefix': '" + request.term + "' ,'TenantID': '" + '<%=  ViewState["TenantID"] %>' + "' ,'DeptID': '" + document.getElementById('<%= hifDepartmentID.ClientID%>').value + "'  }",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {

                            if (data.d == "" || data.d == ",") {
                                alert("No Divisions are available");
                                document.getElementById('<%=atcDepartment.ClientID %>').focus();
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

                    $("#<%=hifDivisionID.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });
            var textfieldname = $("#<%= this.atcDivision.ClientID %>");
            DropdownFunction(textfieldname)


            var textfieldname = $('.DynaSONumber');
            DropdownFunction(textfieldname);

            $('.DynaSONumber').autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadSONumbers") %>',
                        data: "{ 'prefix': '" + request.term + "', 'TenentID': '" + document.getElementById('<%=this.hifTenant.ClientID%>').value + "',  'CustomerID': '" + document.getElementById("<%= hifCustomerID.ClientID %>").value + "'}",
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

                    $("#<%=hifSOHeaderID.ClientID %>").val(i.item.val);
                    $("#txtCustPONumber").val("");
                    $("#txtInvoiceNumber").val("");
                },
                minLength: 0
            });



            var textfieldname = $('.DynaProOrderNumber');
            DropdownFunction(textfieldname);
            $('.DynaProOrderNumber').autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadProOrderNumbers") %>',
                        data: "{ 'prefix': '" + request.term + "', 'TenantID': '" + '<%=  ViewState["TenantID"] %>' + "'}",
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

            var textfieldname = $('.DynaCusPONumber');
            DropdownFunction(textfieldname);
            $('.DynaCusPONumber').autocomplete({
                source: function (request, response) {
                   // alert(document.getElementById("<%= hifActualInvoice.ClientID %>").value);
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadCustomerPONumbers") %>',
                        data: "{ 'prefix': '" + request.term + "', 'SOHeaderID': '" + document.getElementById("<%= hifSOHeaderID.ClientID %>").value + "', 'InvoiceNo': '" + document.getElementById("<%=hifActualInvoice.ClientID%>").value + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {

                            if (data.d == "") {
                                alert("No Customer PO / Invoice is configured to this SO");
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
                }
                ,
                select: function (e, i) {

                    $("#vhifCustomerPOID").val(i.item.val);
                    $("#txtInvoiceNumber").val("");
                },
                minLength: 0
            });


            var textfieldname = $('.DynaInvoiceNumber');
            DropdownFunction(textfieldname);
            $('.DynaInvoiceNumber').autocomplete({
                source: function (request, response) {
                    if (document.getElementById("<%=hifSOHeaderID.ClientID%>").valueOflue == "0") {
                        alert("No invoice # is configured");
                        return;
                    }
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetCusPOInvoiceNoList") %>',
                        data: "{ 'prefix': '" + request.term + "', 'CustomerPOID': '" + document.getElementById('vhifCustomerPOID').value + "', 'InvoiceNo': '" + document.getElementById("<%=hifActualInvoice.ClientID%>").value + "','ActualCustomerPOID':'" + document.getElementById("<%=hifActualCustomerPOID.ClientID%>").value + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {

                            if (data.d == "") {
                                alert("No invoice # is configured");
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

                    $("#vhifInvoiceNumber").val(i.item.val);
                },
                minLength: 0
            });





            var textfieldname = $('#txtActivityRateType');
            DropdownFunction(textfieldname);
            $('#txtActivityRateType').autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadActivityRateType") %>',
                        data: "{ 'prefix': '" + request.term + "', 'InOut': '2,3'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {

                            if (data.d == "") {
                                alert("No RateType is available.");
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

                    $("#hifActivityRateTypeID").val(i.item.val);
                    $('#txtActivityRateName').val('');
                },
                minLength: 0
            });
            var OBDID = new URL(window.location.href).searchParams.get("obdid") 
            var textfieldname = $('#txtActivityRateName');
            DropdownFunction(textfieldname);
            $('#txtActivityRateName').autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadActivityRate") %>',
                        data: "{ 'prefix': '" + request.term + "', 'RateTypeID': '" + $("#hifActivityRateTypeID").val() + "','OutboundId':'"+OBDID+"'}",
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
                },
                minLength: 0
            });


        }

        // Load Mcode 
        function GetMCodes() {
            debugger;

            //alert($("#Account").val());
            $.ajax({
                url: '<%=ResolveUrl("~/mOutbound/OutboundDetails.aspx/LoadPSNMaterialItems") %>',
                data: "{}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (response) {

                    var dt = JSON.parse(response.d);
                    ItemList = dt;

                },
                error: function (response) {

                },
                failure: function (response) {

                }
            });
        }

        function GetpackingSlipData() {
            // ListPackedQty = 0;
            var OBDMaterialID = new URL(window.location.href).searchParams.get("obdid");
            debugger;
            $.ajax({
                url: '<%=ResolveUrl("~/mOutbound/OutboundDetails.aspx/GetPackingSlipData") %>',
                data: "{'OutboundID':"+OBDMaterialID+"}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                async: false,
                success: function (response) {
                    debugger;
                    PackingItemList = JSON.parse(response.d);
                }
            });
        }
        GetpackingSlipData();
        function GetpackingSlipDetails() {
            // ListPackedQty = 0;
            debugger;
            $.ajax({
                url: '<%=ResolveUrl("~/mOutbound/OutboundDetails.aspx/GetPackingSlipNumberData") %>',
                data: "{}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                async: false,
                success: function (response) {
                    debugger;
                    ItemMasterList = JSON.parse(response.d);
                }
            });
            var uomcheck = null;
            //Get ItemMasterDetails on Edit  <th>PackingSlipDetailsID</th>  <td>" + ItemMasterList[i].PackingSlipDetailsID + "</td>
            PackingSliptable = "<table class='table' id='PackingSliptable'><thead><tr><th>Packing Slip No.</th><th>HandlingType</th><th>Weight</th><th>volume</th><th >Delete</th><th >Add Item</th><th >Print</th></tr></thead><tbody>";
            if (ItemMasterList != null && ItemMasterList.length > 0) {
                //if (uomcheck > 0) {
                $('#lblPSN').text(ItemMasterList[0].PackingSlipNo);   
                for (var i = 0; i < ItemMasterList.length; i++) {
                    //debugger;
<%--                                 var TextFieldMaterial = $("#<%= this.txtPSNMaterial.ClientID %>");
                                if (ItemMasterList[i].MCode == TextFieldMaterial) {
                                    
                                    ListPackedQty += ItemMasterList[i].PackedQty;
                                    }--%>
                    PackingSliptable += "<tr id='uomrow-" + i + "'><td>" + ItemMasterList[i].PackingSlipNo + "</td><td>" + ItemMasterList[i].HandlingType + "</td><td>" + ItemMasterList[i].MaxWeight + "</td><td>" + ItemMasterList[i].MaxVolume + "</td><td><input type='checkbox' id='ChkConfigure" + i + "' class='i-checks' name='chdelete' data-attr='" + ItemMasterList[i].PackingSlipHeaderID + "'/></td><td><span style='cursor:pointer !important;' id='editbtnuom" + i + "' onclick=EditItem(" + ItemMasterList[i].PackingSlipHeaderID + "," + ItemMasterList[i].MaxWeight + "," + ItemMasterList[i].MaxVolume + "); ><i class='material-icons vl'>edit</i></span></td><td><span style='cursor:pointer !important;' id='printbtnuom" + i + "' onclick=PrintPackingSlipMaterialInfo(" + ItemMasterList[i].PackingSlipHeaderID + ");><i class='material-icons vl'>print</i></span></td>";
                    //PSNtable += "<td ><span id='editbtnuom" + i + "' style='cursor:pointer !important;' onclick=DeletePSNMaterialDetails(" + ItemMasterList.Table[i].MaterialMaster_UoMID + ");><i class='material-icons vl'>edit</i></span></td>";
                    PackingSliptable += "</tr>";

                    //Count = $.grep(newdata[2], function (a)
                    //{ return a.INB_MST_ConsignmentType_ID == ItemMasterList[i].PackingSlipHeaderID });
                    //        debugger;
                    //        if (Count.length != 0) {
                    //            $('#ChkConfigure' + i).prop('checked', true);
                    //        }

                }
                PackingSliptable += "<tr id='uomrow1-" + i + "'><td></td><td></td><td></td><td></td><td><span style='cursor:pointer !important;' id='delbtnuom" + i + "' onclick=DeletePSNDetails();><i class='material-icons vl'>delete</i></span></td><td></td><td></td>";
                PackingSliptable += "</tr>";
                // }

                //}
            }
            else {

                PackingSliptable += "<tr><td colspan='8' style='text-align:center'><b>No Data Found</b></td><tr>";
                //$("#ddlMeasure").val("0");            
                // $("#ddlMeasure").attr("disabled", false);
            }
            PackingSliptable += "</tbody></table>";
            $('#divPackingSliptable').html(PackingSliptable);
        }

        //Deleting Material Details
        function DeletePSNDetails() {
            debugger;
            //var chklength = $('input[type="checkbox"][name="chdelete"]:checked').length;
            var IDs = "";
            $('input[type="checkbox"][name="chdelete"]').each(function () {
                debugger;
                var val = this.checked;
                if (val == true) {
                    //alert($(this).attr("data-attr"));
                    IDs += $(this).attr("data-attr") + ',';
                }
            });


            //IDs = IDs.sub(0, IDs.length - 1);
            //for (var i = 0; i < chklength; i++) {
            //    IDs += $('#ChkConfigure' + i).attr('data-attr');
            //}
            //Tenant = $("#Tenant").val();
            //alert(IDs);
            debugger;
            var rfidID = IDs;
            $.ajax({
                url: '<%=ResolveUrl("~/mOutbound/OutboundDetails.aspx/DeletePSNDetails") %>',
                data: "{'IDs' : '" + IDs + "'}",
                dataType: "json",
                type: "POST",
                async: false,
                contentType: "application/json; charset=utf-8",
                beforeSend: function () {
                debugger;
                    var lblOutboundStatus=$("#<%= this.lblOutboundStatus.ClientID %>").text();
                    if(lblOutboundStatus=="Sent to Delivery"  || lblOutboundStatus=="Delivered")
                    {
                      showStickyToast(false, "Cannot delete, as this OBD is Sent to Delivery", false);
                      return false;
                    }
                    else
                    {
                       return confirm("Are you sure you want to delete?");
                    }
                },
                success: function (response) {
                    //alert(response.d);
                    var del = response.d;
                    if (del == "") {
                        //showStickyToast(false, "Cannot delete, as this UoM is used by some transactions", false);
                        showStickyToast(true, "Successfully deleted the selected records", false);
                        //var PSNHeaderId = $("#hdnPSNHeaderID").val();
                        GetpackingSlipDetails();
                    }
                    else {
                        //showStickyToast(true, "Successfully Deleted UoM", true);
                        showStickyToast(false, "Cannot delete, as this Material is used by some transactions", false);
                    }
                    //location.reload();
                }
            });
        }

        var ListPackedQty = 0;
        function GetPSNMaterialDetails(PSNHeaderId) {
            debugger;
            $.ajax({
                url: '<%=ResolveUrl("~/mOutbound/OutboundDetails.aspx/GetPSNMaterialDetails") %>',
                data: "{'PSNHeaderId' : '" + PSNHeaderId + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                async: false,
                success: function (response) {
                    debugger;
                    ItemMasterList = JSON.parse(response.d);
                    generateMaterials(ItemMasterList);
                    //$("#txtAccount").val(ItemMasterList.Table[0].Account);
                    //$("#Account").val(ItemMasterList.Table[0].AccountID);
                    //$('#txtAccount').attr("disabled", true);

                }
            });
            return ItemMasterList;
        }

        function generateMaterials(ItemMasterList) {
            // ListPackedQty = 0;
            debugger;
            var uomcheck = null;
            //Get ItemMasterDetails on Edit  <th>PackingSlipDetailsID</th>  <td>" + ItemMasterList[i].PackingSlipDetailsID + "</td>
            var PSNtable = "<table class='table' style='padding: 10px;' id='PSNtable'><thead><tr><th>Material</th><th>Picked UOM</th><th>Picked Qty.</th><th>packed UOM</th><th>Packed Qty.</th><th >Delete</th></tr></thead><tbody style='max-height: 210px;'>";
            if (ItemMasterList != null && ItemMasterList.length > 0) {
                //if (uomcheck > 0) {
                for (var i = 0; i < ItemMasterList.length; i++) {
                        //debugger;
<%--                                 var TextFieldMaterial = $("#<%= this.txtPSNMaterial.ClientID %>");
                                if (ItemMasterList[i].MCode == TextFieldMaterial) {
                                    
                                    ListPackedQty += ItemMasterList[i].PackedQty;
                                    }--%>
                    PSNtable += "<tr id='uomrow-" + i + "'><td>" + ItemMasterList[i].MCode + "</td><td>" + ItemMasterList[i].MaterialMaster_UoMID + "</td><td>" + ItemMasterList[i].PickedQuantity + "</td><td>" + ItemMasterList[i].PackedUOM + "</td><td>" + ItemMasterList[i].PackedQty + "</td>   <td><span style='cursor:pointer !important;' id='delbtnuom" + i + "' onclick=DeletePSNMaterialDetails(" + ItemMasterList[i].PackingSlipDetailsID + ");><i class='material-icons vl'>delete</i></span></td>";
                    //PSNtable += "<td ><span id='editbtnuom" + i + "' style='cursor:pointer !important;' onclick=DeletePSNMaterialDetails(" + ItemMasterList.Table[i].MaterialMaster_UoMID + ");><i class='material-icons vl'>edit</i></span></td>";
                    PSNtable += "</tr>";
                }
                //}
            }
            else {

                PSNtable += "<tr><td colspan='8' style='text-align:center'><b>No Data Found</b></td><tr>";
                //$("#ddlMeasure").val("0");            
                // $("#ddlMeasure").attr("disabled", false);
            }
            PSNtable += "</tbody></table>";
            $('#divPSNtable').html(PSNtable);
        }

        //Deleting Material Details
        function DeletePSNMaterialDetails(id) {
            debugger;
            //Tenant = $("#Tenant").val();
            var rfidID = id;
            $.ajax({
                url: '<%=ResolveUrl("~/mOutbound/OutboundDetails.aspx/DeletePSNMaterialDetails") %>',
                data: "{'detId' : '" + rfidID + "'}",
                dataType: "json",
                type: "POST",
                async: false,
                contentType: "application/json; charset=utf-8",
                beforeSend: function () {
                 debugger;
                    var lblOutboundStatus=$("#<%= this.lblOutboundStatus.ClientID %>").text();
                    if(lblOutboundStatus=="Sent to Delivery")
                    {
                      showStickyToast(false, "Cannot delete, as this OBD is Sent to Delivery", false);
                      return false;
                    }
                    else
                    {
                       return confirm("Are you sure you want to delete?");
                    }
                },
                success: function (response) {
                    //alert(response.d);
                    var del = response.d;
                    if (del == "") {
                        //showStickyToast(false, "Cannot delete, as this UoM is used by some transactions", false);
                        showStickyToast(true, "Successfully Deleted Material", false);
                        var PSNHeaderId = $("#hdnPSNHeaderID").val();
                        GetPSNMaterialDetails(PSNHeaderId);
                    }
                    else {
                        //showStickyToast(true, "Successfully Deleted UoM", true);
                        showStickyToast(false, "Cannot delete, as this Material is used by some transactions", false);
                    }
                    //location.reload();
                }
            });
        }
        //Saving Packing Slip Header Information
        function AddPackingSlipInfoSave() {
            debugger;
            var HandlingTypeId = $("#<%= this.ddlHHType.ClientID %>").val();
            var Maxweight = $("#<%= this.txtMaxCapacity.ClientID %>").val();
            var MaxLength = $("#<%= this.txtLength.ClientID %>").val();
            var MaxWidth = $("#<%= this.txtWidth.ClientID %>").val();
            var MaxHeight = $("#<%= this.txtHeight.ClientID %>").val();
            var Remarks = $("#<%= this.txtRemarks.ClientID %>").val();
            var PackageCondition = $("#<%= this.txtPkgCondition.ClientID %>").val();

            if (HandlingTypeId == "0") {
                showStickyToast(false, "Please select HandlingType", false);
                return false;
            }
            if (Maxweight == "" || Maxweight == undefined || Maxweight == null) {
                showStickyToast(false, "Please Enter Weight", false);
                return false;
            }
            if (MaxLength == "" || MaxLength == undefined || MaxLength == null) {
                showStickyToast(false, "Please Enter Length", false);
                return false;
            }
            if (MaxWidth == "" || MaxWidth == undefined || MaxWidth == null) {
                showStickyToast(false, "Please Enter Width", false);
                return false;
            }
            if (MaxHeight == "" || MaxHeight == undefined || MaxHeight == null) {
                showStickyToast(false, "Please Enter Height", false);
                return false;
            }
           
            debugger;
            $.ajax({
                url: '<%=ResolveUrl("~/mOutbound/OutboundDetails.aspx/UpsertPackingSlipAddlnInfo") %>',
                data: "{'HandlingTypeId':'" + HandlingTypeId + "','Maxweight':'" + Maxweight + "','MaxLength':'" + MaxLength + "','MaxWidth':'" + MaxWidth + "','MaxHeight':'" + MaxHeight + "','Remarks':'" + Remarks + "','PackageCondition':'" + PackageCondition + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (response) {
                    if (response.d == "1") {
                        setTimeout(function () {
                            showStickyToast(false, "Outbound Is Already Delivered", false);
                        }, 3000);
                    }
                    else {
                        showStickyToast(true, "Packing Slip Information Saved Successfully", false);
                        GetpackingSlipDetails();
                        PackingSlipNumberControlclear();
                        return false;
                    }

                }
            });
             GetpackingSlipDetails();
<%--            $.ajax({
                url: '<%=ResolveUrl("~/mOutbound/OutboundDetails.aspx/GetValidationInfo") %>',
                data: "{}",//<=cp.MaterialId%> 'MaterialID': '" + $('#hdnMaterial').val() + "',
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (response) {
                    if (response.d == "1") {
                        showStickyToast(false, "PGI not yet performed", false);
                        return false;
                    }
                    else if (response.d == "") {
                        showStickyToast(false, "PGI UserName is not Saved", false);
                        return false;
                    }
                    else if (response.d == "-2")
                    //showStickyToast(false, "Packing is already done", false);
                    //return false;
                    {

                        $.ajax({
                            url: '<%=ResolveUrl("~/mOutbound/OutboundDetails.aspx/LoadPSNMaterialItems") %>',
                            data: "{ 'OBDID': '" + OBDMaterialID + "'}",//<=cp.MaterialId%> 'MaterialID': '" + $('#hdnMaterial').val() + "',
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                if (data.d.length == 0) {
                                    debugger;
                                    showStickyToast(false, "Items are filled.", false);
                                    PackingSlipNumberControlclear();
                                    debugger;
                                      $('#<%= pnlReceivedDelivery.ClientID %>').show();
                                 //   $('<%= pnlReceivedDelivery.ClientID %>').style.visibility = 'visible';
                                  //  $('<%=pnlReceivedDelivery.ClientID%>').css('display','block');
                                    return false;
                                }
                                else {
                                    debugger;
                                    $.ajax({
                                        url: '<%=ResolveUrl("~/mOutbound/OutboundDetails.aspx/UpsertPackingSlipAddlnInfo") %>',
                                        data: "{'HandlingTypeId':'" + HandlingTypeId + "','Maxweight':'" + Maxweight + "','MaxLength':'" + MaxLength + "','MaxWidth':'" + MaxWidth + "','MaxHeight':'" + MaxHeight + "','Remarks':'" + Remarks + "','PackageCondition':'" + PackageCondition + "'}",
                                        dataType: "json",
                                        type: "POST",
                                        contentType: "application/json; charset=utf-8",
                                        success: function (response) {
                                            if (response.d == "1") {
                                                setTimeout(function () {
                                                    showStickyToast(false, "Outbound Is Already Delivered", false);
                                                }, 3000);
                                            }
                                            else {
                                                showStickyToast(true, "Packing Slip Information Saved Successfully", false);
                                                GetpackingSlipDetails();
                                                PackingSlipNumberControlclear();
                                                return false;
                                            }

                                        }
                                    })
                                }

                            }
                        });
                    }
                    GetpackingSlipDetails();
                }
            });--%>
        }


        //Update Packing Slip Information for status Change 
        function UpdatePackingSlipInfo() {
            debugger;
            GetpackingSlipData();
            GetPickMaterialData();
            var OBDMaterialID = new URL(window.location.href).searchParams.get("obdid");
            var lblOutboundStatus=$("#<%= this.lblOutboundStatus.ClientID %>").text();
                    if(lblOutboundStatus=="Delivered")
                    {
                      showStickyToast(false, "Outbound Is Already Delivered", false);
                      return false;
            }
           debugger;
            var OBDMaterialID = new URL(window.location.href).searchParams.get("obdid");
            if (PackingItemList.length == 0) {
                 showStickyToast(false, "Please Add Item details", false);
                return false;
            }
            if (PackingItemList.length != PickingItemList.length) {

                showStickyToast(false, "Please do packing for all picked Items", false);
                return false; 
            }
            $.ajax({
                url: '<%=ResolveUrl("~/mOutbound/OutboundDetails.aspx/UpdatePackingSlipInformation") %>',
                data: "{ }",//<=cp.MaterialId%> 'MaterialID': '" + $('#hdnMaterial').val() + "',
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (response) {
                    if (response.d == "1") {
                        setTimeout(function () {
                            showStickyToast(false, "Outbound Is Already Delivered", false);
                        }, 3000);
                    }
                    else if (response.d == "2") {
                        debugger;
                            $('#<%= lblOutboundStatus.ClientID%>').html('Sent to Delivery')
                            showStickyToast(true, "Sent to Delivery", false);
                            PackingSlipNumberControlclear();
                            $("#<%=pnlReceivedDelivery.ClientID%>").css("display", "block");
                            return false;
                    }
                    else if (response.d == "5") {
                         $('#<%= lblOutboundStatus.ClientID%>').html('Sent to Delivery')
                            showStickyToast(true, "Sent to Delivery", false);
                            PackingSlipNumberControlclear();
                            $("#<%=pnlReceivedDelivery.ClientID%>").css("display", "block");
                            return false;
                    }
                         else if (response.d == "6") {
                         $('#<%= lblOutboundStatus.ClientID%>').html('Sent to Delivery')
                            showStickyToast(false, "Packing is already updated", false);
                            PackingSlipNumberControlclear();
                            $("#<%=pnlReceivedDelivery.ClientID%>").css("display", "block");
                            return false;
                    }
                    else if (response.d == "0") {
                        showStickyToast(false, "PGI not yet performed", false);
                        return false;
                    }
                    else {
                        (response.d == "")                   
                        showStickyToast(false, "PackingSlip Is Already Generated", false);
                        return false;
                         
                         }
                    }
                })
            GetpackingSlipDetails();
            //document.getElementById('pnlReceivedDelivery').style.visibility='visible';
           
        }
        
        //Saving Packing Slip Header Material Information
        function AddPackingSlipMaterialInfoSave() {
            debugger;
            var selectedText = $('#MainContent_OBContent_txtPSNMaterial').val();
            $.ajax({
                url: '<%=ResolveUrl("~/mOutbound/OutboundDetails.aspx/GetPSNMaterial") %>',
                data: "{'Mcode':'" + selectedText + "'}",
                dataType: "json",
                type: "POST",
                async: false,
                contentType: "application/json; charset=utf-8",

                success: function (response) {
                    debugger;
                    // alert('123');
                    if (response.d.length > 0) {
                        var pickqty = JSON.parse(response.d);
                        TotpackedQty = pickqty[0].TotPackedQty;
                        Mvolume = pickqty[0].MVolume;
                        MWeight = pickqty[0].MWeight;
                    }
                }
            })

            var PSNMaterial = $("#<%= this.txtPSNMaterial.ClientID %>").val();
            var PickedQty = $("#<%= this.txtPickedQty.ClientID %>").val();
            var PackedQty = $("#<%= this.txtPackedQty.ClientID %>").val();
            var PSNHeaderId = $("#hdnPSNHeaderID").val();
            var PackedUOM = $("#<%= this.txtPackedUOM.ClientID %>").val();

            var dt = GetPSNMaterialDetails(PSNHeaderId);

            var items = $.grep(dt, function (a) { return a.MCode == PSNMaterial });
            var ListPackedQty = 0;
            for (var i = 0; i < items.length; i++) {
                ListPackedQty += items[i].PackedQty;
            }


            if (PSNMaterial == "0") {
                showStickyToast(false, "Please select Item", false);
                return;
            }
            if (PickedQty == "" || PickedQty == undefined || PickedQty == null) {
                showStickyToast(false, "Please Pick the Items", false);
                return;
                $("#SupModal").modal({ show: 'true' });
            }
            if (PackedUOM == "0" || PackedUOM == "") {
                showStickyToast(false, "Please select Packed UOM.", false);
                return;
                // $("#SupModal").modal({show: 'true' });
            }
            if (PackedQty == "" || PackedQty == undefined || PackedQty == null || PackedQty == 0) {
                showStickyToast(false, "Please Enter Packed Qty and it should be greater than zero.", false);
                return;
                // $("#SupModal").modal({show: 'true' });
            }
            debugger;
            var Maxweight = $("#hdnPSNMaxWeight").val();
            var Maxvolume = $("#hdnPSNMaxVol").val();
            if ((parseInt(TotpackedQty) + parseInt(PackedQty)) > PickedQty) {
                debugger;
                if ((PickedQty - TotpackedQty) > 0) {
                    showStickyToast(false, "Packed Qty to be limited upto '" + (PickedQty - TotpackedQty) + "'", false);
                    return;
                }
                else {
                    showStickyToast(false, "Picked Qty is filled", false);
                    return;
                }
            }
            else if (parseInt(PackedQty) > parseInt(PickedQty)) {
                debugger;
                showStickyToast(false, "Picked Qty should be greater than Packed Qty.", false);
                return;
            }
            else if (parseInt(PackedQty) > parseInt(PickedQty) && ListPackedQty < parseInt(PickedQty)) {
                debugger;
                if ((PickedQty - ListPackedQty) > 0) {
                    showStickyToast(false, "Packed Qty to be limited upto '" + (PickedQty - ListPackedQty) + "'", false);
                    return;
                }
                else {
                    showStickyToast(false, "Picked Qty is filled", false);
                    return;
                }
            }
            else if ((ListPackedQty + parseInt(PackedQty)) > parseInt(PickedQty)) {
                debugger;
                if (PickedQty - ListPackedQty > 0) {
                    showStickyToast(false, "Packed Qty to be limited upto '" + (PickedQty - ListPackedQty) + "'", false);
                    return;
                }
                else {
                    showStickyToast(false, "Picked Qty is filled", false);
                    return;
                }
            }
           
            else if (ListPackedQty == parseInt(PickedQty)) {
                showStickyToast(false, "Packed Qty is filled.", false);
                return;
            }

            debugger;

            //if ((PickedQty - ListPackedQty) < PackedQty) {
            //if ((ListPackedQty) < PickedQty) {
            //    showStickyToast(false, "Packed Qty to be given upto :'"+ (PickedQty - ListPackedQty)+"'.", false);
            //    return;
            //}

            // if (ListPackedQty >= PickedQty) {
            //        debugger;
            //        showStickyToast(false, "Picked Qty should be greater or equal to the Packed Qty.", false);
            //        return;
            //}
            $.ajax({
                url: '<%=ResolveUrl("~/mOutbound/OutboundDetails.aspx/UpsertPackingSlipAddMaterialInfo") %>',
                data: "{'PSNMaterial':'" + PSNMaterial + "','PickedQty':'" + PickedQty + "','PackedQty':'" + PackedQty + "','PSNHeaderId':'" + PSNHeaderId + "','PackedUOM':'" + PackedUOM + "','Itemvolume':'" + Mvolume + "','Itemweight':'" + MWeight + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                beforeSend: function () {
                    if (Mvolume > Maxvolume || MWeight > Maxweight) {
                        return confirm("Item Volume is exceeding the Handling Type Volume/Weight do you want to continue?");
                    }
                    //if (MWeight > Maxweight) {
                    //    return confirm("Item Weight is exceeding the Handling Type Weight do you want to continue?");
                    //}
                },

                success: function (response) {
                    debugger;
                    //setTimeout(function () {
                    //    location.reload();
                    showStickyToast(true, "Packing Slip Material Information Saved Successfully", false);
                    PSNMaterialControlclear();
                    //ListPackedQty = 0;
                    GetPSNMaterialDetails($("#hdnPSNHeaderID").val());
                    //}, 3000);
                }

            })
        }


        function PrintPackingSlipMaterialInfo(PackingSlipHeaderID) {
            debugger;
            // var OBDMaterialID = new URL(window.location.href).searchParams.get("obdid");
            var PSNHeaderID = PackingSlipHeaderID;
            if (PSNHeaderID == undefined || PSNHeaderID == "") {
                (location.href.indexOf('?OBDId=') > 0)
                var OBDId = location.href.split('?OBDId=')[1];
            }
            $.ajax({
                url: 'DeliveryPackSlip.aspx/btnPrintPackingSlipInfo',
                data: "{ 'PSNHeaderID': '" + PSNHeaderID + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    var obj = data.d;
                    debugger;
                    var link = document.createElement('a');
                    //window.open('UnloadPdf/' + obj);
                    link.href = '../mOutbound/PackingSlip/' + obj;
                    link.download = obj;
                    link.dispatchEvent(new MouseEvent('click'));
                    showStickyToast(true, 'PDF Generated Successfully');
                    //console.log(FormData);

                    //if (res.length == 1) {
                    //    if (res == "1") {
                    //var obj = data.d;
                    //        var link = document.createElement('a');
                    //        link.href = '../mOutbound/PackingSlip/' + res;

                    //        //window.open("../mOutbound/PackingSlip/PackingSlipMaterialInfo.pdf");
                    //    }
                    //    else {
                    //        showStickyToast(false, "No box found", false);
                    //    }
                    //}
                    //else {
                    //    showStickyToast(false, "Box filling is not completed yet", false);
                    //    $("#divBoxPendingDetails").html(res);
                    //    $('#divBlocker').show();
                    //}
                },
                error: function (response) {

                    showStickyToast(false, "Please check your network connection", false);
                },
                failure: function (response) {
                }
            });

            return false;

        }

        //Clear Supplier Form On Close Button
        function mySupclear() {
            $("#txtPickedQty, #UnitCost, #PlannedTime, #SupQuantity, #MMT_SUPPLIER_ID, #ddlSupplier, #Supname").val("");
            $("#ddlCurrency").val("Select PSN Material");

        }
        function LoadStoresAssociated(TenantID) {
            var data = "{ TenantID:  '" + TenantID + "'}";
            InventraxAjax.AjaxResultExecute("OutboundDetails.aspx/GetStoresAssociated", data, 'GetStoresAssociatedOnSuccess', 'GetStoresAssociatedOnError', null);
        }

        function GetStoresAssociatedOnSuccess(data) {
            var obj = JSON.parse(data.Result);
            console.log(obj);
            $('.ddlStoresAssociated').empty();
            $('.ddlStoresAssociated').append("<option value=0>--Select--</option>");
            for (var i = 0; i < obj.length; i++) {
                $('.ddlStoresAssociated').append("<option value=" + obj[i].WarehouseID + ">" + obj[i].WHCode + "</option>");
            }
        }


        function GetPickMaterialData() {
            var OBDMaterialID = new URL(window.location.href).searchParams.get("obdid");
            $.ajax({
                url: '<%=ResolveUrl("~/mOutbound/OutboundDetails.aspx/GetPickMaterial") %>',
                data: "{'OutboundID':"+OBDMaterialID+"}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                async: false,
                success: function (response) {
                    debugger;
                    PickingItemList = JSON.parse(response.d);
                }
            });
        }

        fnLoadAutocompletes();


    </script>
    <script src="../mInbound/Scripts/InventraxAjax.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            fnLoadAutocompletes();

        });

        function EditItem(PackingSlipHeaderID, MaxWeight, MaxVolume) {
            debugger;
            $("#hdnPSNHeaderID").val(PackingSlipHeaderID);
            $("#hdnPSNMaxVol").val(MaxVolume);
            $("#hdnPSNMaxWeight").val(MaxWeight);
            $("#SupModal").modal({
                show: 'true'
            });
            //GetMCodes();
            GetPSNMaterialDetails(PackingSlipHeaderID);
            PSNMaterialControlclear();
        }

        //Clear Packed Qty controls
        function PSNMaterialControlclear() {
            debugger;
            <%-- var PSNMaterial = $("#<%= txtPSNMaterial.ClientID %>").val();
            var PickedQty = $("#<%= txtPickedQty.ClientID %>").val();
            var PackedQty = $("#<%= txtPackedQty.ClientID %>").val();--%>

            $("#<%= txtPickedQty.ClientID %>, #<%= txtPackedQty.ClientID %>, #<%= txtPackedUOM.ClientID %>, #<%= txtPickedQty.ClientID %>").val("");
            $("#<%= txtPSNMaterial.ClientID %>").val("");

        }
        //Clear Packing Slip Number controls
        function PackingSlipNumberControlclear() {
            debugger;
            $("#<%= txtMaxCapacity.ClientID %>, #<%= txtLength.ClientID %>, #<%= txtWidth.ClientID %>, #<%= txtHeight.ClientID %>").val("");   //, #<%= txtRemarks.ClientID %>").val("");
            $("#<%= txtPkgCondition.ClientID %>").val("");
            $("#<%= ddlHHType.ClientID %>").val(0);
            $("#<%= txtRemarks.ClientID %>").val("");
        }

    </script>

    <script type="text/javascript">
        // Following script is to Handle scroll position in the Updatepanle While updating the Force Updates Forms in the Panel
        // It is important to place this JavaScript code after ScriptManager1

        var xPos, yPos;
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        function BeginRequestHandler(sender, args) {
            //GRN Pending
            if ($get('accordion') != null) {
                // Get X and Y positions of scrollbar before the partial postback
                xPos = $get('accordion').scrollLeft;
                yPos = $get('accordion').scrollTop;
            }


        }

        function EndRequestHandler(sender, args) {
            //GRN Pending
            if ($get('accordion') != null) {
                // Set X and Y positions back to the scrollbar
                // after partial postback
                $get('accordion').scrollLeft = xPos;
                $get('accordion').scrollTop = yPos;
            }

        }

        prm.add_beginRequest(BeginRequestHandler);
        prm.add_endRequest(EndRequestHandler);
    </script>

    <script type="text/javascript">
        function OpenImage(path) {
            window.open(path, 'Naresh', 'height=800,width=900');
        }
    </script>

    <script type="text/javascript">

        $(document).ready(function () {
            debugger;
            GetpackingSlipDetails();

            $("#divItemPrintData").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 200,
                height: 450,
                width: 700,
                overflow: 'auto',
                resizable: false,
                draggable: false,
                position: ["center top", 40],

                close: function () {

                    $(".ui-dialog").fadeOut(500);

                    $(document).unbind('scroll');

                    $('body').css({ 'overflow': 'visible' });

                },
                title: "Pending Goods-OUT List",
                open: function (event, ui) {
                    $(".ui-dialog").hide().fadeIn(500);

                    $('body').css({ 'overflow': 'hidden' });

                    //$('body').width($('body').width());

                    $(document).bind('scroll', function () {
                        window.scrollTo(0, 0);
                    });

                    $(this).parent().appendTo("#divItemPrintDataContainer");
                }
            });
        });
        function closeDialog() {

            //Could cause an infinite loop because of "on close handling"
            $("#divItemPrintData").dialog('close');

        }
        function openDialog(title, linkID) {

            $("#divItemPrintData").dialog('open');

            NProgress.start();

            $("#divItemPrintData").block({
                message: '<img src="<%=ResolveUrl("~") %>Images/async_obd.gif" />',
                css: { border: '0px' },
                fadeIn: 0,
                fadeOut: 0,
                overlayCSS: { backgroundColor: '#ffffff', opacity: 1 }
            });
        }

        function unblockDialog() {
            $("#divItemPrintData").unblock();
            NProgress.done();
        }


    </script>

    <script type="text/javascript" language="javascript">
        function confirmMsg() {
            var frm = document.forms[0];
            // loop through all elements
            for (i = 0; i < frm.length; i++) {
                // Look for our checkboxes only
                if (frm.elements[i].name.indexOf("deleteRec") != -1) {
                    // If any are checked then confirm alert, otherwise nothing happens
                    if (frm.elements[i].checked)
                        return confirm('Are you sure you want to delete the selected facility list data?')
                }
            }
            return false;
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
        }
    </script>

    <div class="dashed"></div>
    <div class="container">
        <table border="0" cellspacing="0" cellpadding="0" align="center" class="setCenter">

            <tr>
                <td align="center">
                    <status> <asp:Label ID="lblOutboundStatus" runat="server" CssClass="OutboundStatus"></asp:Label></status>
                    <div id="accordion" align="left">

                        <!-- Section 1 -->
                        <h3>1. Initiate Outbound Delivery</h3>
                        <div class="converttodiv">

                            <!--Start Initiate Outbound Delivery  -->

                            <asp:UpdatePanel ChildrenAsTriggers="true" ID="upnlInitiateOBD" runat="server" UpdateMode="Always">

                                <Triggers>
                                    <asp:PostBackTrigger ControlID="lnkSaveOBD" />

                                </Triggers>

                                <ContentTemplate>

                                    <table cellpadding="2" cellspacing="2" border="0" width="100%" align="center">

                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="lblStatusMessage" runat="server" CssClass="errorMsg" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td class="FormLabels" colspan="2">
                                                <%-- <span class="mandatory_field">Note: </span><span class="errorMsg">* </span>Indicates mandatory fields--%>
                                            </td>
                                            <td class="FormLabels" colspan="2">
                                                <%-- <span class="mandatory_field">Note: </span><span class="errorMsg">* </span>Indicates mandatory fields--%>
                                            </td>

                                            <%--<td class="FormLabels">&nbsp; &nbsp;
                                       <div style="float: right; font-weight: bold; font-size: 30px !important;">
                                           <asp:Label ID="lblOutboundStatus" runat="server" CssClass="OutboundStatus"></asp:Label>
                                       </div>

                                            </td>--%>

                                        </tr>

                                        <tr>
                                            <td>&nbsp; </td>
                                        </tr>

                                        <tr>
                                            <td class="FormLabels">
                                                <div class="flex">

                                                    <asp:DropDownList ID="ddlDeliveryDocType" CssClass="txt_Blue_Req" runat="server" required="" />
                                                    <label>Delivery Doc. Type</label>
                                                </div>
                                            </td>
                                            <td class="FormLabels">
                                                <div class="flex">
                                                    <asp:RequiredFieldValidator ID="rfvtxtOBDNumber" SetFocusOnError="true" ControlToValidate="txtOBDNumber" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" ValidationGroup="createOBD" Enabled="false" />
                                                    <asp:TextBox ID="txtOBDNumber" SkinID="txt_Req" runat="server" Enabled="false" required="" />
                                                    <asp:ImageButton ID="btnGetOBDGDR" ToolTip="Generate Delivery Doc. No." CssClass="" runat="server" OnClick="btnGetOBDGDR_OnClick" ImageUrl="../Images/icon_newID.gif" Visible="false" />
                                                    <label>Delivery Doc. Number</label>
                                                </div>
                                            </td>
                                            <td class="FormLabels">
                                                <div class="flex">
                                                    <div>
                                                        <asp:RequiredFieldValidator ID="rfvtxtOBDDate" SetFocusOnError="true" ControlToValidate="txtOBDDate" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" runat="server" ValidationGroup="createOBD" />
                                                    </div>
                                                    <div>
                                                        <asp:TextBox ID="txtOBDDate" SkinID="txt_Req" runat="server" required="" onpaste="return false" />
                                                        <label><span class="errorMsg">* </span>Delivery Doc. Date</label>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>


                                        <!-- Print Area -->

                                        <tr>
                                            <td colspan="3" id="tdDDRPrintArea" align="center">
                                                <div id="printArea" class="PrintListcontainer">
                                                    <link href="../PrintStyle.css" type="text/css" rel="stylesheet" media="print">
                                                    <asp:Panel runat="server" ID="pnlGDRInfo" CssClass="GDRPanel" Width="650px" Visible="false">
                                                        <table cellpadding="3" cellspacing="3" border="0" width="100%">
                                                            <thead></thead>
                                                            <tr>
                                                                <td colspan="3" align="center">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<font color="blue" size="5" weight="bold">PRIORITY DELIVERY NOTE </font> </td>
                                                                <td align="right">

                                                                    <asp:ImageButton runat="server" ID="btnGDRPrint" ImageUrl="../Images/blue_menu_icons/printer.png" CssClass="NoPrint" OnClick="btnGDRPrint_Click" Visible="true" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="FormLabels" colspan="2">PDN No:
                                                           <asp:Literal ID="ltGDRNumber" runat="server" />
                                                                </td>
                                                                <td class="FormLabels" colspan="2">Date:&nbsp;&nbsp;<asp:TextBox ID="txtGDRDate" Width="118" CssClass="txt_slim_green" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="FormLabels" colspan="2">Mr./Messrs
                                                           <br />
                                                                    <asp:TextBox ID="txtCompanyName" runat="server" CssClass="txt_slim_green" Width="400" />
                                                                </td>
                                                                <td class="FormLabels" colspan="2">Customer P.O.No:
                                                           <br />
                                                                    <asp:TextBox ID="txtPONumber" runat="server" CssClass="txt_slim_green" Width="150" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="FormLabels">Invoice No:
                                                                    <br />
                                                                    <asp:TextBox ID="txtInvoiceNo" Width="150" CssClass="txt_slim_green" runat="server" />
                                                                </td>
                                                                <td class="FormLabels">
                                                                    <asp:RequiredFieldValidator ID="rfvtxtInvoiceDate" SetFocusOnError="true" ControlToValidate="txtInvoiceDate" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" ValidationGroup="createOBD" />Invoice Date:
                                                                    <br />
                                                                    <asp:TextBox ID="txtInvoiceDate" Width="150" CssClass="txt_slim_green" runat="server" />
                                                                </td>
                                                                <td class="FormLabels" colspan="2">SAP Ref. No:
                                                                    <br />
                                                                    <asp:TextBox ID="txtGDRSAPRefNo" Width="150" CssClass="txt_slim_green" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="1" class="FormLabels">
                                                                    <div class="flex">
                                                                        <div>
                                                                            <label>Remarks</label>
                                                                        </div>
                                                                        <div>
                                                                            <asp:TextBox ID="txtPDNRemarks" TextMode="MultiLine" CssClass="txt_slim_green" runat="server" />
                                                                        </div>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:Literal ID="ltScript" runat="server"></asp:Literal>
                                                                    <asp:Literal ID="ltValid" runat="server"></asp:Literal>
                                                                    <span class="NoPrint">
                                                                        <asp:Label CssClass="ErrorAlert" ID="ltError" runat="server" Text="" />
                                                                    </span>
                                                                </td>
                                                                <td align="right">
                                                                    <b>
                                                                        <asp:LinkButton runat="server" CssClass="NoPrint" Text="Add New PDN Item" ID="lnkAddNew" OnClick="lnkAddNew_Click" Visible="false" /></b>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4" valign="top" class="PrintArea">
                                                                    <asp:GridView Width="100%" ShowFooter="true" AllowPaging="true" PageSize="30" GridLines="Both" ID="gvGDRRecords" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowSorting="True" CssClass="overallGrid" HorizontalAlign="Left" OnRowCancelingEdit="gvGDRRecords_RowCancelingEdit" OnRowCommand="gvGDRRecords_RowCommand" OnSorting="gvGDRRecords_Sorting" OnPageIndexChanging="gvGDRRecords_PageIndexChanging" OnRowUpdating="gvGDRRecords_RowUpdating" OnRowEditing="gvGDRRecords_RowEditing">
                                                                        <Columns>
                                                                            <asp:TemplateField ItemStyle-Width="60" HeaderText="S.No:" ItemStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:Literal runat="server" ID="ltItemSNo" Text='<%# DataBinder.Eval(Container.DataItem, "LineNumber") %>' />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-Width="100" HeaderText="ITEM CODE">
                                                                                <ItemTemplate>
                                                                                    <asp:Literal runat="server" ID="ltItemCode" Text='<%# DataBinder.Eval(Container.DataItem, "Mcode") %>' />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-Width="300" HeaderText="DESCRIPTION">
                                                                                <ItemTemplate>
                                                                                    <span style="overflow: hidden; word-wrap: break-word; max-width: 150px;">
                                                                                        <asp:Literal runat="server" ID="ltDescription" Text='<%# DataBinder.Eval(Container.DataItem, "MDescription").ToString() %>' />
                                                                                    </span>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-Width="50" HeaderText="UNIT" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:Literal runat="server" ID="ltUnit" Text='<%# DataBinder.Eval(Container.DataItem, "SUoM") %>' />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-Width="80" HeaderText="QTY" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:Literal runat="server" ID="ltQty" Text='<%# DataBinder.Eval(Container.DataItem, "Quantity") %>' />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-Width="200" HeaderText="REMARKS" ItemStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:Literal runat="server" ID="ltRemarks" Text='<%# DataBinder.Eval(Container.DataItem, "Remarks") %>' />
                                                                                </ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:TextBox ID="txtItemRemarks" runat="server" TextMode="MultiLine" Text='<%# DataBinder.Eval(Container.DataItem, "Remarks") %>' CssClass="" Width="150" Rows="4" />
                                                                                </EditItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4">
                                                                    <table border="0" cellpadding="3" cellspacing="3" width="100%" class="GDR_FooterTab">
                                                                        <tr>
                                                                            <td align="right" width="60">
                                                                                <asp:RequiredFieldValidator ID="rfvatcGDRRequestedBy" SetFocusOnError="true" ControlToValidate="atcGDRRequestedBy" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" ValidationGroup="createOBD" />
                                                                                <nobr>Requested By:</nobr>
                                                                            </td>
                                                                            <td width="200" class="cell_rightborder">
                                                                                <asp:TextBox ID="atcGDRRequestedBy" Width="150" CssClass="txt_slim_green" runat="server" />
                                                                                <asp:HiddenField runat="server" ID="hifGDRRequestedByID" />
                                                                            </td>
                                                                            <td colspan="2">
                                                                                <span class="small_Note">Goods received in good order & condition unless otherwise
                                                                                    <br />
                                                                                    specified in REMARKS column</span> </td>

                                                                        </tr>
                                                                        <tr>
                                                                            <td align="right">Driver Name:</td>
                                                                            <td class="cell_rightborder">
                                                                                <asp:TextBox ID="txtGDRDriver" CssClass="txt_slim_green" runat="server" Width="150" /></td>
                                                                            <td width="50">&nbsp;</td>
                                                                            <td width="320">&nbsp;</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="right">Prepared By:</td>
                                                                            <td class="cell_rightborder">
                                                                                <asp:TextBox ID="txtGDRPreparedBy" runat="server" CssClass="txt_slim_green" Width="150" />
                                                                                <asp:HiddenField runat="server" ID="hifGDRPreparedByID" />
                                                                            </td>
                                                                            <td align="right">Received Name:</td>
                                                                            <td class="cell_bottomborder">&nbsp;</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="right">&nbsp;</td>
                                                                            <td class="cell_rightborder">&nbsp;</td>
                                                                            <td align="right">Signature:</td>
                                                                            <td class="cell_bottomborder">&nbsp;</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="right">Approved By:</td>
                                                                            <td class="cell_rightborder">
                                                                                <asp:TextBox ID="txtGDRApprovedBy" CssClass="txt_slim_green" runat="server" Width="150" />
                                                                                <asp:HiddenField runat="server" ID="hifGDRApprovedByID" />
                                                                            </td>
                                                                            <td align="right">Date:</td>
                                                                            <td class="cell_bottomborder">&nbsp;</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="right">Sign:</td>
                                                                            <td class="cell_rightborder">&nbsp;</td>
                                                                            <td align="right">Stamp:</td>
                                                                            <td class="cell_bottomborder">&nbsp;</td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </div>
                                            </td>

                                        </tr>

                                        <!-- Print Area -->

                                        <tr>
                                            <td class="FormLabels">
                                                <div class="flex">
                                                    <div>
                                                        <asp:RequiredFieldValidator ID="rfvTenant" SetFocusOnError="true" ControlToValidate="txtTenant" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" runat="server" ValidationGroup="createOBD" />


                                                    </div>
                                                    <div>
                                                        <asp:TextBox ID="txtTenant" SkinID="txt_Auto" runat="server" required="" />
                                                        <span class="errorMsg">* </span>
                                                        <label>Tenant</label>
                                                        <asp:HiddenField runat="server" ClientIDMode="Static" ID="hifTenant" Value="0" />
                                                    </div>
                                                </div>
                                            </td>
                                            <td class="FormLabels" valign="top">
                                                <div class="flex">
                                                    <div>
                                                        <asp:RequiredFieldValidator ID="rfvatcCustomerName" SetFocusOnError="true" ControlToValidate="atcCustomerName" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" runat="server" ValidationGroup="createOBD" />
                                                    </div>
                                                    <div>
                                                        <asp:TextBox ID="atcCustomerName" SkinID="txt_Auto" runat="server" required="" />
                                                        <span class="errorMsg">* </span>
                                                        <label>Customer</label>
                                                        <asp:HiddenField runat="server" ID="hifCustomerID" />
                                                    </div>
                                                </div>
                                            </td>
                                            <td class="FormLabels" valign="top">
                                                <div class="flex">
                                                    <div>
                                                        <asp:RequiredFieldValidator ID="rfvatcRequestedBy" SetFocusOnError="true" ControlToValidate="atcRequestedBy" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" runat="server" ValidationGroup="createOBD" />
                                                    </div>
                                                    <div>
                                                        <asp:TextBox ID="atcRequestedBy" SkinID="txt_Auto" runat="server" required="" />
                                                        <span class="errorMsg">* </span>
                                                        <label>Requested By</label>
                                                        <asp:HiddenField ID="hifRequestedBy" runat="server" ClientIDMode="Static" Value="0" />
                                                    </div>
                                                </div>

                                            </td>
                                            <td class="FormLabels" style="display: none;">Department:
                                                <br />
                                                <asp:TextBox ID="atcDepartment" Width="200" runat="server" />
                                                <asp:HiddenField runat="server" ID="hifDepartmentID" />
                                                <br />
                                                Division:
                                                <br />
                                                <asp:TextBox ID="atcDivision" Width="200" runat="server" />
                                                <asp:HiddenField ID="hifDivisionID" runat="server" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td colspan="3">&nbsp;
                                            </td>
                                        </tr>

                                        <tr>
                                            <td colspan="" class="FormLabels">
                                                <div class="flex ">

                                                    <div class="noafter">
                                                        <asp:CheckBoxList ID="cblStoresAssociated" runat="server" Visible="false" Width="100%" CellPadding="4" CellSpacing="4" TextAlign="right" RepeatColumns="3" />
                                                        <asp:TextBox ID="txtStoresAssociated" SkinID="txt_Auto" runat="server" required="" />
                                                        <label>Store(s) Associated</label>
                                                        <asp:HiddenField runat="server" ClientIDMode="Static" ID="hdnStoresAssociated" Value="0" />
                                                    </div>
                                                </div>
                                            </td>
                                            <td colspan="" class="FormLabels">
                                                <div class="flex">
                                                    <div></div>
                                                    <div>
                                                        <asp:TextBox ID="txtRemarks_Ini" TextMode="MultiLine" runat="server" required="" /><label>Remarks</label>
                                                    </div>
                                                </div>
                                            </td>


                                            <td colspan="" class="FormLabels">
                                                <asp:UpdatePanel ChildrenAsTriggers="true" UpdateMode="Always" ID="upnlfileUpload" runat="server">
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="lnkSaveOBD" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <div class="flex">
                                                            <div>
                                                                <p style="margin: 0">Attach Delivery Doc.</p>
                                                            </div>
                                                            <div>
                                                                <asp:FileUpload ID="fucOBDDeliveryNote" runat="server" CssClass="txt_small_Req" />
                                                                <asp:Literal ID="ltDNLink" runat="server" />
                                                            </div>
                                                        </div>

                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>


                                        <tr>
                                            <%-- <td colspan="1" class="FormLabels"> 
                                       <asp:UpdatePanel ChildrenAsTriggers="true" UpdateMode="Always" ID="upnlfileUpload" runat="server">
                                           <Triggers>
                                               <asp:PostBackTrigger ControlID="lnkSaveOBD"/>
                                           </Triggers>
                                           <ContentTemplate>
                                        Attach Delivery Doc.:<br />
                                        <asp:FileUpload ID="fucOBDDeliveryNote" runat="server" CssClass="txt_small_Req" /> &nbsp;&nbsp;&nbsp;
                                       <br /><br />
                                        <asp:Literal ID="ltDNLink" runat="server"/>
                                               </ContentTemplate>
                                           </asp:UpdatePanel>
                                   </td>--%>
                                            <td class="FormLabels" style="width: 100% !important">
                                                <hr />
                                                <p>Priority (Set Level / Delivery Date & Time)</p>
                                                <br />
                                                <tr>
                                                    <td>
                                                        <div class="flex">
                                                            <div></div>
                                                            <div>
                                                                <asp:DropDownList ID="ddlPriorityLevel" runat="server" required="" /><label style="width: 50px;">Level</label>
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="flex">
                                                            <div></div>
                                                            <div>
                                                                <asp:TextBox ID="txtPriorityDate" Width="62%" runat="server" required="" onpaste="return false" /><label style="width: 50px;">Date</label>
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="flex">
                                                            <div></div>
                                                            <div>
                                                                <asp:TextBox ID="txtTimeEntry" Width="62%" CssClass="TimePicker" runat="server" placeholder="Time" />
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td colspan="3">&nbsp;
                                            </td>
                                        </tr>

                                        <tr>
                                            <td align="right" class="space_align" style="width: 100% !important;">

                                                <asp:LinkButton ID="lnkCloseOBD" Font-Underline="false" Visible="false" runat="server" Text="Close OBD from System" SkinID="lnkButCancel" OnClientClick="return confirm('Are you sure you want to close this Delv. Doc. from the System? All the line items associated with this Delv. Doc\'s. pick transcations will be cancled.Please ensure that the items are placed back in the respective location they are picked from with the help of \'Delivery Pick Note.\' ');" OnClick="lnkCloseOBD_Click" />

                                                <asp:LinkButton ID="lnkCancelfromSystem" Font-Underline="false" Visible="false" SkinID="lnkButCancel" runat="server" OnClientClick="return confirm('Are you sure you want to cancel this Delv. Doc. from the System? All the line items associated with this Delv. Doc\'s. pick transcations will be cancled.Please ensure that the items are placed back in the respective location they are picked from with the help of \'Delivery Pick Note.\' ');" Text="Cancel OBD from System" OnClick="lnkCancelfromSystem_Click" />

                                                <asp:LinkButton ID="lnkCancelOBD" runat="server" OnClick="lnkCancelOBD_Click" CssClass="btn btn-primary right">
                                        Cancel <%=MRLWMSC21Common.CommonLogic.btnfaClear %>
                                                </asp:LinkButton>



                                                <asp:LinkButton ID="lnkSaveOBD" CausesValidation="True" OnClientClick="return checkFields();Page_ValidationActive=true;" ValidationGroup="createOBD" CssClass="btn btn-primary right" runat="server" OnClick="lnkSaveOBD_Click" />

                                                <%-- <asp:ValidationSummary ID="valSummary" DisplayMode="List" runat="server" ShowMessageBox="true" EnableClientScript="true" ShowSummary="false" ValidationGroup="createOBD" ForeColor="red" Font-Bold="true" />--%>
                                                <br />
                                                <br />
                                            </td>
                                        </tr>

                                    </table>

                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <!--End Initiate Outbound Delivery  -->


                        </div>

                        <!-- Section 2 -->
                        <h3>2. Delivery Document Line Items</h3>
                        <div>

                            <!-- Start SO Line Items -->

                            <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" ID="upnlDelvDocLineItems" UpdateMode="Always">
                                <ContentTemplate>

                                    <asp:Panel runat="server" ID="pnlDelvDocLineItems" Visible="false">




                                        <table border="0" cellspacing="0" cellpadding="0" width="100%">

                                            <tr>
                                                <td align="right">
                                                    <asp:LinkButton runat="server" ID="lnkAddDelvDocLineItem" Font-Underline="false" OnClick="lnkAddDelvDocLineItem_Click" CssClass="btn btn-primary right">
                                                Add Sales Order <%=MRLWMSC21Common.CommonLogic.btnfaNew %>
                                                    </asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <br />
                                                    <asp:Label runat="server" ID="lblGvStatus"></asp:Label>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td align="center">

                                                    <asp:Panel ID="pnlSOLineItems" runat="server">

                                                        <asp:GridView Width="100%" ShowFooter="true" GridLines="Both" ID="gvSOLineItems" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="true" PageSize="25" AllowSorting="True" SkinID="gvLightSteelBlueNew" HorizontalAlign="Left" OnSorting="gvSOLineItems_Sorting" OnPageIndexChanging="gvSOLineItems_PageIndexChanging" OnRowDataBound="gvSOLineItems_RowDataBound" OnRowEditing="gvSOLineItems_RowEditing" OnRowUpdating="gvSOLineItems_RowUpdating" OnRowCancelingEdit="gvSOLineItems_RowCancelingEdit" OnRowCommand="gvSOLineItems_RowCommand">

                                                            <Columns>

                                                                <asp:TemplateField ItemStyle-Width="100" HeaderText="SO Number" ItemStyle-CssClass="home" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton Font-Underline="false" ID="lnkEditPOItem" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SONumber") %>' PostBackUrl='<%# String.Format("../mOrders/SalesOrderInfo.aspx?soid={0}",DataBinder.Eval(Container.DataItem, "SOHeaderID").ToString()) %>' />
                                                                        <img src="../Images/redarrowleft.gif" border="0" />
                                                                        <asp:Literal runat="server" ID="ltSOHeaderID" Text='<%# DataBinder.Eval(Container.DataItem, "SOHeaderID").ToString() %>' Visible="false"></asp:Literal>
                                                                        <asp:HyperLink ID="hypSOTMList" Text='<%# DataBinder.Eval(Container.DataItem, "SONumber") %>' NavigateUrl='<%# String.Format("../mOrders/SalesOrderInfo.aspx?soid={0}",DataBinder.Eval(Container.DataItem, "SOHeaderID").ToString()+"&tid="+DataBinder.Eval(Container.DataItem,"TenantID")) %>' Font-Underline="false" runat="server"></asp:HyperLink>

                                                                        <asp:Literal runat="server" Visible="false" ID="ltSONumber" Text='<%# DataBinder.Eval(Container.DataItem, "SONumber") %>' />
                                                                        <asp:Literal runat="server" ID="lthidOutbound_CustomerPOID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "Outbound_CustomerPOID") %>' />
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <%--<asp:RequiredFieldValidator ID="rfvtxtSONumber" SetFocusOnError="true" ControlToValidate="txtSONumber" Display="Dynamic" EnableClientScript="true"  CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" />--%>
                                                                        <div class="gridInput">
                                                                            <asp:Literal runat="server" ID="lthidOutbound_CustomerPOID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "Outbound_CustomerPOID") %>' />
                                                                            <span style="color: red; margin-left: -0.2em">*</span><asp:TextBox ID="txtSONumber" EnableTheming="false" runat="server" CssClass="DynaSONumber" Text='<%# DataBinder.Eval(Container.DataItem, "SONumber") %>' />
                                                                        </div>
                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField ItemStyle-Width="100" HeaderText="Customer PO #" ItemStyle-CssClass="home">
                                                                    <ItemTemplate>
                                                                        <asp:Literal runat="server" ID="ltCustPONumber" Text='<%# DataBinder.Eval(Container.DataItem, "CustPONumber") %>' />

                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <%--<asp:RequiredFieldValidator ID="rfvtxtCustPONumber" SetFocusOnError="true" ControlToValidate="txtCustPONumber" Display="Dynamic" EnableClientScript="true"  CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" />--%>
                                                                        <div class="gridInput">
                                                                            <span style="color: red; margin-left: -0.2em">*</span><asp:TextBox runat="server" ID="txtCustPONumber" EnableTheming="false" ClientIDMode="Static" CssClass="DynaCusPONumber" Text='<%# DataBinder.Eval(Container.DataItem, "CustPONumber") %>' />

                                                                            <asp:HiddenField ClientIDMode="Static" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "CustomerPOID").ToString() %>' ID="vhifCustomerPOID" />
                                                                        </div>


                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField ItemStyle-Width="100" HeaderText="Invoice No." ItemStyle-CssClass="home">
                                                                    <ItemTemplate>
                                                                        <asp:Literal runat="server" ID="ltInvoiceNumber" Text='<%# DataBinder.Eval(Container.DataItem, "InvoiceNo") %>' />
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <%--<asp:RequiredFieldValidator ID="rfvtxtInvoiceNumber" SetFocusOnError="true" ControlToValidate="txtInvoiceNumber" Display="Dynamic" EnableClientScript="true"  CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" />--%>
                                                                        <div class="gridInput">
                                                                            <span style="color: red; margin-left: -0.2em">*</span><asp:TextBox runat="server" ID="txtInvoiceNumber" EnableTheming="false" ClientIDMode="Static" CssClass="DynaInvoiceNumber" Text='<%# DataBinder.Eval(Container.DataItem, "InvoiceNo") %>' />

                                                                            <asp:HiddenField ClientIDMode="Static" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "InvoiceNo").ToString() %>' ID="vhifInvoiceNumber" />
                                                                        </div>

                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>



                                                                <asp:TemplateField ItemStyle-Width="50" HeaderText="SO Date" ItemStyle-CssClass="home" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Literal runat="server" ID="ltSODate" Text='<%#DataBinder.Eval(Container.DataItem, "SODate","{0: dd-MMM-yyyy}") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField ItemStyle-Width="50" HeaderText="Customer Name" ItemStyle-CssClass="home" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Literal runat="server" ID="ltCustomerName" Text='<%#DataBinder.Eval(Container.DataItem, "CustomerName") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField ItemStyle-Width="50" HeaderText="SO Currency" ItemStyle-CssClass="home" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Literal runat="server" ID="ltCurrencyCode" Text='<%#DataBinder.Eval(Container.DataItem, "Code") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField ItemStyle-Width="50" HeaderText="Line Items Count" ItemStyle-CssClass="home" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Literal runat="server" ID="ltLineItemsCount" Text='<%#DataBinder.Eval(Container.DataItem, "LineItemsCount") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Delete" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint">
                                                                    <HeaderTemplate>
                                                                        <nobr>Delete</nobr>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkIsDeleteRFItem" runat="server" />
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                    </EditItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:LinkButton ID="lnkDeleteRFItem" Font-Underline="false" CssClass="blueLink" runat="server" Text="<nobr><i class='material-icons vl'>delete</i></nobr>" OnClick="lnkDeleteRFItem_Click" OnClientClick="return confirm('Are you sure you want to delete the selected line items?')" />
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField ItemStyle-Font-Underline="false" Visible="false" DataField="EditName" ReadOnly="true" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint" />
                                                                <asp:CommandField ControlStyle-Font-Underline="false" ItemStyle-Width="30" ButtonType="Link" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint" ItemStyle-HorizontalAlign="left" CancelImageUrl="Images/cancel.gif" EditImageUrl="/Images/edit.gif" EditText="<nobr> <i class='material-icons vl'>edit</i</nobr>" ShowEditButton="true" UpdateImageUrl="/Images/save.gif" />


                                                            </Columns>

                                                        </asp:GridView>


                                                    </asp:Panel>


                                                </td>
                                            </tr>



                                        </table>






                                    </asp:Panel>


                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <!-- End SO Line Items -->


                            <!-- Start Production Order Line Items -->

                            <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" ID="upnlProdOrder" UpdateMode="Always">
                                <ContentTemplate>

                                    <asp:Panel runat="server" ID="pnlProdOrder" Visible="false">

                                        <table border="0" cellspacing="0" cellpadding="0" width="100%">

                                            <tr>
                                                <td align="right">
                                                    <asp:LinkButton runat="server" ID="lnkAddProdDelvDocLineItem" SkinID="lnkButEmpty" Font-Underline="false" Text="Add Line Item" OnClick="lnkAddProdDelvDocLineItem_Click"></asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="lblProdOrderStatus"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">

                                                    <asp:Panel ID="Panel2" runat="server">

                                                        <asp:GridView Width="100%" ShowFooter="true" GridLines="Both" ID="gvProdOrder" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="true" PageSize="25" AllowSorting="True" SkinID="gvLightCactus" HorizontalAlign="Left" OnSorting="gvProdOrderLineItems_Sorting" OnPageIndexChanging="gvProdOrderLineItems_PageIndexChanging" OnRowDataBound="gvProdOrderLineItems_RowDataBound" OnRowEditing="gvProdOrderLineItems_RowEditing" OnRowUpdating="gvProdOrderLineItems_RowUpdating" OnRowCancelingEdit="gvProdOrderLineItems_RowCancelingEdit" OnRowCommand="gvProdOrderLineItems_RowCommand">

                                                            <Columns>

                                                                <asp:TemplateField ItemStyle-Width="100" HeaderText="Prod. Order #" ItemStyle-CssClass="home" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton Font-Underline="false" ID="lnkEditProOItem" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PRORefNo") %>' PostBackUrl='<%# String.Format("../mManufacturingProcess/ProductionOrder.aspx?proid={0}",DataBinder.Eval(Container.DataItem, "ProductionOrderHeaderID").ToString()) %>' />

                                                                        <asp:HyperLink ID="HyperLink1" Text='<%# DataBinder.Eval(Container.DataItem, "PRORefNo") %>' NavigateUrl='<%# String.Format("../mManufacturingProcess/ProductionOrder.aspx?proid={0}",DataBinder.Eval(Container.DataItem, "ProductionOrderHeaderID").ToString()) %>' Font-Underline="false" runat="server"></asp:HyperLink>

                                                                        <asp:Literal runat="server" Visible="false" ID="ltSONumber" Text='<%# DataBinder.Eval(Container.DataItem, "PRORefNo") %>' />
                                                                        <asp:Literal runat="server" ID="lthidOutbound_ProductionOrderHeaderID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "Outbound_ProductionOrderHeaderID") %>' />
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <div class="gridInput">
                                                                            <asp:RequiredFieldValidator ID="rfvtxtSONumber" SetFocusOnError="true" ControlToValidate="txtPRONumber" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" />
                                                                            <asp:Literal runat="server" ID="lthidOutbound_ProductionOrderHeaderID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "Outbound_ProductionOrderHeaderID") %>' />
                                                                            <asp:TextBox ID="txtPRONumber" EnableTheming="false" runat="server" CssClass="DynaProOrderNumber" Text='<%# DataBinder.Eval(Container.DataItem, "PRORefNo") %>' />
                                                                        </div>
                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>


                                                                <asp:TemplateField ItemStyle-Width="50" HeaderText="Prod. Order Date" ItemStyle-CssClass="home" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Literal runat="server" ID="ltProOrderDate" Text='<%#DataBinder.Eval(Container.DataItem, "ProductionOrderDate","{0: dd/MM/yyyy}") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>


                                                                <asp:TemplateField ItemStyle-Width="50" HeaderText="Start Date" ItemStyle-CssClass="home" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Literal runat="server" ID="ltProOrderStartDate" Text='<%#DataBinder.Eval(Container.DataItem, "StartDate","{0: dd/MM/yyyy}") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField ItemStyle-Width="50" HeaderText="Due Date" ItemStyle-CssClass="home" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Literal runat="server" ID="ltProOrderEndDate" Text='<%#DataBinder.Eval(Container.DataItem, "DueDate","{0: dd/MM/yyyy}") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField ItemStyle-Width="50" HeaderText="Line Items Count" ItemStyle-CssClass="home" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Literal runat="server" ID="ltLineItemsCount" Text='<%#DataBinder.Eval(Container.DataItem, "LineItemsCount") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField ItemStyle-Width="50" HeaderText="Prod. Qty." ItemStyle-CssClass="home" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Literal runat="server" ID="ltProdOrderQty" Text='<%#DataBinder.Eval(Container.DataItem, "ProductionQuantity") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Delete" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint">
                                                                    <HeaderTemplate>
                                                                        <nobr>Delete</nobr>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkIsDeleteRFItem" runat="server" />
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                    </EditItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:LinkButton ID="lnkDeleteProdOrderItem" Font-Underline="false" CssClass="blueLink" runat="server" Text="<nobr> Delete <img src='../Images/redarrowright.gif' border='0' /></nobr>" OnClick="lnkDeleteProdOrderItem_Click" OnClientClick="return confirm('Are you sure you want to delete the selected line items?')" />
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField ItemStyle-Font-Underline="false" Visible="false" DataField="EditName" ReadOnly="true" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint" />
                                                                <asp:CommandField ControlStyle-Font-Underline="false" ItemStyle-Width="30" ButtonType="Link" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint" ItemStyle-HorizontalAlign="left" CancelImageUrl="Images/cancel.gif" EditImageUrl="/Images/edit.gif" EditText="<nobr> Edit <img src='../Images/redarrowright.gif' border='0' /></nobr>" ShowEditButton="true" UpdateImageUrl="/Images/save.gif" />


                                                            </Columns>

                                                        </asp:GridView>

                                                        <span>&nbsp;<br />
                                                        </span>
                                                    </asp:Panel>
                                                    <br />

                                                </td>
                                            </tr>



                                        </table>

                                    </asp:Panel>


                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <!-- End Production Line Items -->

                        </div>

                        <!-- Section 3 -->
                        <h3>3. Pick & Check and PGI</h3>
                        <div>

                            <!-- Start Pick N Check -->

                            <asp:UpdatePanel ChildrenAsTriggers="true" runat="server" ID="upnlPickandCheck" UpdateMode="Always" class="upnlClass">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlStores" EventName="SelectedIndexChanged" />
                                </Triggers>
                                <ContentTemplate>

                                    <asp:Panel runat="server" ID="pnlPickNCheck" Visible="false">

                                        <div class="">
                                            <div class="row">
                                                <div class="Col m12" colspan="4">
                                                   <div class="makeitboler"> <asp:HyperLink runat="server" ID="hylPickNote" Text="Pick Note" Font-Underline="false" CssClass="ButEmpty"></asp:HyperLink>

                                                     &nbsp;&nbsp;<asp:Label runat="server" ID="lblStoreName" CssClass=""></asp:Label></div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div>
                                                    <div class="">
                                                        <div cellpadding="2" cellspacing="2" border="0" width="100%">

                                                            <div class="row">
                                                                <div class="col m3 s3">
                                                                    <div class="flex">
                                                                        <asp:RequiredFieldValidator ID="rfvddlStores" SetFocusOnError="true" ControlToValidate="ddlStores" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage="*" runat="server" ValidationGroup="recieveOBD" />

                                                                        <div class="fit-wi">
                                                                            <asp:DropDownList ID="ddlStores" runat="server" SkinID="ddlRequired" AutoPostBack="true" OnSelectedIndexChanged="ddlStores_SelectedIndexChanged" Width="36%" required="" /><label> Pick Store</label>
                                                                            <span class="errorMsg"></span>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col m3">
                                                                    <div class="flex" style="display: flex;">
                                                                        <div>
                                                                            <asp:RequiredFieldValidator ID="rfvtxtOBDRcvdDate" SetFocusOnError="true" ControlToValidate="txtOBDRcvdDate" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage="*" runat="server" ValidationGroup="recieveOBD" /><br />
                                                                        </div>

                                                                        <div class="" style="display: flex;">
                                                                            <div>
                                                                                
                                                                                <asp:TextBox ID="txtOBDRcvdDate" Width="" SkinID="txt_Req" runat="server" Enabled="false" />
                                                                            </div>
                                                                            <div>
                                                                               
                                                                                <asp:TextBox runat="server" ID="OBDRecvdTimeEntry" CssClass="TimePicker" SkinID="txt_Req" Enabled="false" />
                                                                            </div>
                                                                        </div>
                                                                        <label style="position: absolute; top: -12px;">
                                                                            Delivery Doc. Received On
                                                                        </label>
                                                                    </div>
                                                                </div>
                                                                <div class="Col m3">
                                                                    <asp:LinkButton runat="server" ID="lnkViewPendingGoodsOutList" Visible="false" CssClass="lnkViewButton" Font-Underline="false" OnClick="lnkViewPendingGoodsOutList_Click">
                                                                            View Pending Goods-Out List  <span  class="space fa fa-external-link"></span>
                                                                    </asp:LinkButton>
                                                                </div>
                                                           

                                                                <div class="col m3">
                                                                    <div class="flex">
                                                                        <asp:RequiredFieldValidator ID="rfvatcPickedBy" SetFocusOnError="true" ControlToValidate="atcPickedBy" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage="*" runat="server" ValidationGroup="recieveOBD" />

                                                                        <div class="">

                                                                            <asp:TextBox ID="atcPickedBy" SkinID="txt_Auto" runat="server" required="" />
                                                                            <asp:HiddenField runat="server" ID="hifPickedByID" />
                                                                            <label>Picked By</label>
                                                                            <span class="errorMsg"></span>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col m3" colspan="3">
                                                                    <div class="flex">
                                                                        <div>
                                                                            <asp:RequiredFieldValidator ID="rfvatcCheckedBy" SetFocusOnError="true" ControlToValidate="atcCheckedBy" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage="*" runat="server" ValidationGroup="recieveOBD" />
                                                                        </div>
                                                                        <div>
                                                                            <asp:TextBox ID="atcCheckedBy" SkinID="txt_Auto" runat="server"  required="" />
                                                                            <label>Checked By</label>
                                                                            <span class="errorMsg"></span>
                                                                            <asp:HiddenField runat="server" ID="hifCheckedByID" />
                                                                        </div>
                                                                </div>
                                                            </div>
                                                                </div>
                                                            <div class="row">
                                                                <div class="col m3">
                                                                    <div class="flex">
                                                                        <div class="">
                                                                            <asp:TextBox ID="txtRemarks_StoreIncharge" TextMode="MultiLine" CssClass="txt_slim" runat="server" required="" /><label>Remarks</label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col m3">
                                                                    <div class="flex">
                                                                        <div class="">
                                                                            <asp:TextBox runat="server" ID="txtAutoPGIDate" SkinID="txt_Req" required="" ></asp:TextBox><label>PGI Done Date </label>
                                                                        <span class="errorMsg"></span>

                                                                        </div>

                                                                    </div>

                                                                </div>
                                                                <div class="col m3">
                                                                    <div class="flex">
                                                                        <div class="">
                                                                             <asp:TextBox runat="server" ID="txtAutoPGITime" CssClass="TimePicker" placeholder="PGI Done Time" SkinID="txt_Req" required="" /><%--<label>PGI Done  Time</label>--%> 
                                                                            <span class="errorMsg"></span>
                                                                        </div>

                                                                    </div>

                                                                </div>
                                                                <div class="col m3">
                                                                    <gap5></gap5>
                                                                    <div flex end style="margin-top: 5px; display:none !important;">
                                                                        <div>
                                                                            <p>Required Vehicle Details</p>
                                                                        </div>
                                                                        <div>
                                                                            <asp:LinkButton ID="lnkReqVehicle" Visible="true" OnClick="lnkReqVehicle_Click" runat="server" Font-Underline="false" CssClass="btn btn-primary right">
                                                                                                        Add Required Vehicle <%=MRLWMSC21Common.CommonLogic.btnfaNew %>
                                                                            </asp:LinkButton>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">

                                                                <div>

                                                                    <div>

                                                                        <div class="">

                                                                          

                                                                            <div class="">

                                                                                <!-- Start Required Equipment -->

                                                                                <div border="0" width="100%" style="vertical-align: top;">
                                                                                    <div class="">

                                                                                   
                                                                                    <div class="row">
                                                                                        <div align="left" colspan="2">
                                                                                            <asp:Panel runat="server" ID="pnlRequiredVehicleGrid" HorizontalAlign="left">
                                                                                                <asp:Label ID="lblReqGvStatus" runat="server"></asp:Label>
                                                                                              

                                                                                                <asp:GridView ID="gvRequiredVehicle" SkinID="gvLightSteelBlueNew" runat="server" Width="300px" CellPadding="4" AllowPaging="false" AllowSorting="false"
                                                                                                    OnRowDataBound="gvRequiredVehicle_RowDataBound"
                                                                                                    OnRowCommand="gvRequiredVehicle_RowCommand"
                                                                                                    OnRowUpdating="gvRequiredVehicle_RowUpdating"
                                                                                                    OnRowCancelingEdit="gvRequiredVehicle_RowCancelingEdit"
                                                                                                    OnRowEditing="gvRequiredVehicle_RowEditing"
                                                                                                    PagerStyle-HorizontalAlign="Right"
                                                                                                    CellSpacing="0">

                                                                                                    <Columns>
                                                                                                        <asp:TemplateField HeaderText="Required Vehicle">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Literal ID="ltRequiredVehicle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "EquipmentName") %>' />
                                                                                                                <asp:Literal ID="lthidRequiredVehicleID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RequiredEquipmentID") %>' />
                                                                                                            </ItemTemplate>
                                                                                                            <EditItemTemplate>
                                                                                                                <asp:RequiredFieldValidator ID="rfvddlEquipmentType" SetFocusOnError="true" ControlToValidate="ddlEquipmentType" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" InitialValue="0" />
                                                                                                                <asp:DropDownList ID="ddlEquipmentType" runat="server"></asp:DropDownList>
                                                                                                                <asp:Literal runat="server" Visible="false" ID="ltEquipmentID" Text='<%# DataBinder.Eval(Container.DataItem, "EquipmentID") %>'></asp:Literal>
                                                                                                                <asp:Literal ID="lthidRequiredVehicleID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RequiredEquipmentID") %>' />
                                                                                                            </EditItemTemplate>
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Required Value" ItemStyle-HorizontalAlign="Center">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Literal ID="ltRequiredValue" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RequiredValue") %>' />
                                                                                                            </ItemTemplate>
                                                                                                            <EditItemTemplate>
                                                                                                                <asp:RequiredFieldValidator ID="rfvtxtRequiredValue" SetFocusOnError="true" ControlToValidate="txtRequiredValue" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" />
                                                                                                                <asp:TextBox ID="txtRequiredValue" Width="80" onKeyPress="return checkNum(event)" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RequiredValue") %>'></asp:TextBox>
                                                                                                            </EditItemTemplate>
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint">
                                                                                                            <HeaderTemplate>
                                                                                                                <nobr>Delete</nobr>
                                                                                                            </HeaderTemplate>
                                                                                                            <ItemTemplate>
                                                                                                                <asp:CheckBox ID="chkIsDeleteReqVItem" runat="server" />
                                                                                                            </ItemTemplate>
                                                                                                            <EditItemTemplate></EditItemTemplate>
                                                                                                            <FooterTemplate>
                                                                                                                <asp:LinkButton ID="lnkDeleteReqVItem" Font-Underline="false" CssClass="blueLink" runat="server" Text="<nobr> Delete <img src='../Images/redarrowright.gif' border='0' /></nobr>" OnClick="lnkDeleteReqVItem_Click" OnClientClick="return Confirm('Are you sure ? want to delete selected items')" />
                                                                                                            </FooterTemplate>
                                                                                                        </asp:TemplateField>

                                                                                                        <asp:BoundField ItemStyle-Font-Underline="false" Visible="false" DataField="EditName" ReadOnly="true" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint" />

                                                                                                        <asp:CommandField ItemStyle-Font-Underline="false" ControlStyle-Font-Underline="false" ItemStyle-Width="30" ButtonType="Link" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint" ItemStyle-HorizontalAlign="left" CancelImageUrl="/Images/cancel.gif" EditImageUrl="/Images/edit.gif" EditText="<nobr> Edit <img src='../Images/redarrowright.gif' border='0' /></nobr>" ShowEditButton="True" UpdateImageUrl="/Images/save.gif" />
                                                                                                    </Columns>
                                                                                                </asp:GridView>

                                                                                            </asp:Panel>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>

                                                                                <!-- End Required Equipment -->


                                                                            </div>

                                                                        </div>

                                                                    </div>

                                                                </div>


                                                            </div>
                                                            <div class="row m0">
                                                                <div colspan="4" class="FormLabels">
                                                                    <asp:CheckBox ID="chkIsReservationDelivery" Visible="false" runat="server" Text="Is this reservation a delivery to the location?" />
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col m3">
                                                                  
                                                                    <div class="flex">
                                                                        <div>
                                                                            <p class="label2">Attach Pick & Check Sheet(s)</p>
                                                                        </div>
                                                                        <!-- For folowing Fileupload control if you change the class ="-multi-" it will upload multiple files (but currently not working in IE 8) wokring in Firefox  -->
                                                                        <div>
                                                                            <asp:UpdatePanel ChildrenAsTriggers="true" ID="upnlPNCFileUpload" runat="server" UpdateMode="Always">
                                                                                <Triggers>
                                                                                    <asp:PostBackTrigger ControlID="lnkSendToPGI" />
                                                                                </Triggers>
                                                                                <ContentTemplate>

                                                                                    <asp:FileUpload ID="fuPickNCheckNotes" runat="server" class="txt_slim_req" />


                                                                                    <asp:Literal ID="ltPickCheckAttachments" runat="server" />

                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </div>
                                                                </div>
                                                           </div>
                                                                <div class="col m9">  <gap></gap>  <gap></gap>  <gap></gap>
                                                                    <asp:LinkButton ID="lnkCancelSendToPGI" runat="server" OnClick="lnkCancelSendToPGI_Click" CssClass="btn btn-primary right">
Clear <%=MRLWMSC21Common.CommonLogic.btnfaClear %>
                                                                    </asp:LinkButton>

                                                                   
                                         <asp:LinkButton ID="lnkSendToPGI" CausesValidation="True" OnClientClick="Page_ValidationActive=true;" ValidationGroup="recieveOBD" runat="server" OnClick="lnkSendToPGI_Click" CssClass="btn btn-primary right">
Update & Send to PGI <%=MRLWMSC21Common.CommonLogic.btnfaSave %>
                                         </asp:LinkButton>


                                                                    <asp:ValidationSummary ID="ValidationSummary1" DisplayMode="List" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="recieveOBD" ForeColor="red" Font-Bold="true" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                       </div>
                                                    </div>
                                           
                                           

                                    </asp:Panel>
                                </ContentTemplate>

                            </asp:UpdatePanel>

                            <!-- End Pick N Check --->

                        </div>

                        <!-- Section 4 -->
                        <h3 style="display:none!important;">4. PGI Details</h3>
                        <div style="display:none!important;">

                            <!-- Start PGI Details --->

                            <asp:UpdatePanel ChildrenAsTriggers="true" runat="server" ID="upnlPGIDetails" UpdateMode="Always">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="lnkPGIUpdate" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:Panel runat="server" ID="pnlPGIDetails" Visible="false">


                                        <div>

                                            <div class="row m0">
                                                <div align="left">
                                                    <asp:Label ID="lblChargesMsg" runat="server" CssClass="ChargesStatus"></asp:Label>
                                                </div>
                                            </div>

                                            <div class="row m0">
                                                <div class="FormLabels" colspan="2">

                                                    <asp:Label runat="server" ID="lblPGIStatus"></asp:Label>
                                                    <br />
                                                </div>
                                            </div>
                                            <div>
                                                <div>
                                                    <div>

                                                        <div class="row">
                                                            <div class="col m3">

                                                                <div class="">
                                                                    <div class="flex erroralign">
                                                                        <asp:TextBox runat="server" ID="txtPGIDate" SkinID="txt_Req" required="" ReadOnly="true"></asp:TextBox><label>PGI Done Date </label>
                                                                        <span class="errorMsg"></span>
                                                                    </div>

                                                                </div>
                                                            </div>
                                                            <div class="col m3">
                                                                <div class="flex">
                                                                    <asp:TextBox runat="server" ID="txtPGITimeEntry" CssClass="TimePicker" placeholder="PGI Done Time" SkinID="txt_Req" required="" /><%--<label>PGI Done  Time</label>--%> <span class="errorMsg"></span>

                                                                </div>
                                                            </div>
                                                            <div class="col m3">
                                                                <div class="flex">

                                                                    <div>
                                                                        <asp:TextBox runat="server" ID="atcPGIRequestedBy" SkinID="txt_Auto"  required=""></asp:TextBox><label>Requested By</label><span class="errorMsg"></span>
                                                                       
                                                                        <asp:HiddenField runat="server" ID="HifPGIRequestedByID" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col m3">

                                                                <div class="flex">
                                                                    <asp:TextBox runat="server" TextMode="MultiLine"  ID="txtPGIRemarks" required=""></asp:TextBox><label>Remarks</label>
                                                                </div>
                                                                </div>
                                                            </div>

                                                        </div>

                                                    </div>

                                                </div>

                                            <div class="row">
                                                <div class="col m12 s12" flex end>
                                                    <asp:LinkButton runat="server" ID="lnkPGICancel" OnClick="lnkPGICancel_Click" CssClass="btn btn-primary right"> Clear <%=MRLWMSC21Common.CommonLogic.btnfaClear %> </asp:LinkButton>

                                            <asp:LinkButton runat="server" ID="lnkPGIUpdate" OnClick="lnkPGIUpdate_Click" CssClass="btn btn-primary right">
Update <%=MRLWMSC21Common.CommonLogic.btnfaUpdate %>
                                            </asp:LinkButton>

                                                </div>
                                            </div>

                                        </div>



                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <!-- End PGI Details --->




                        </div>




                        <%--<h3>5. 3PL Outbound Capture Details</h3>--%>
                        <h3>4. 3PL Activity Details</h3>
                        <div>
                            <asp:UpdatePanel ID="upnl3pl" runat="server" Visible="false" UpdateMode="Always">
                                <ContentTemplate>
                                    <table border="0" cellspacing="0" cellpadding="0" width="100%">
                                        <tr>
                                            <td align="right">
                                                <asp:LinkButton ID="lnkadd3pl" runat="server" OnClick="lnkadd3pl_Click" CssClass="btn btn-primary right">Add New <%=MRLWMSC21Common.CommonLogic.btnfaNew %></asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gv3pl_list" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="10" SkinID="gvLightBlueNew"
                                                    OnRowDataBound="gv3pl_RowDataBound"
                                                    OnRowEditing="gv3pl_RowEditing"
                                                    OnRowUpdating="gv3pl_RowUpdating"
                                                    OnRowCancelingEdit="gv3pl_RowCancelingEdit"
                                                    OnPageIndexChanging="gv3pl_PageIndexChanging">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Tariff Sub-Group" ItemStyle-Width="100">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="ltActivityRateType" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ActivityRateType") %>' />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <div class="gridInput">
                                                                    <asp:HiddenField ID="hifTenantTransactionAccessorialCaptureID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"TenantTransactionAccessorialCaptureID") %>' />

                                                                    <asp:RequiredFieldValidator ID="rfvActivityRateType" runat="server" Display="Dynamic" ControlToValidate="txtActivityRateType" ValidationGroup="vgRequired3pl" ErrorMessage="*" />
                                                                    <asp:TextBox ID="txtActivityRateType" ClientIDMode="Static" SkinID="txt_Auto" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ActivityRateType") %>' />
                                                                    <asp:HiddenField ID="hifActivityRateTypeID" ClientIDMode="Static" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"ActivityRateTypeID") %>' />
                                                                </div>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Tariff " ItemStyle-Width="170">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="ltHandlingService" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ActivityRateName") %>' />

                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <div class="gridInput">
                                                                    <asp:RequiredFieldValidator ID="rfvActivityRateName" runat="server" ControlToValidate="txtActivityRateName" ValidationGroup="vgRequired3pl" Display="Dynamic" ErrorMessage="*" />
                                                                    <asp:TextBox ID="txtActivityRateName" ClientIDMode="Static" SkinID="txt_Auto" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ActivityRateName") %>' />
                                                                    <asp:HiddenField ID="hifActivityRateID" ClientIDMode="Static" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"ActivityRateID") %>' />
                                                                </div>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="UoM" ItemStyle-Width="30">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="ltUOM" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"UoM") %>' />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <%--<asp:TextBox ID="txtUoM" ClientIDMode="Static"  Width="80" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"UoM") %>' />
                                        <asp:HiddenField ID="ltUomID" ClientIDMode="Static" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"UoMID") %>' />--%>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Unit Cost after Discount" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <%--<asp:Literal ID="ltUnitCost" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"UnitCost") %>' />--%>
                                                                <asp:Literal ID="ltUnitCostAfterDiscount" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"UnitCostAfterDiscount") %>' />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <%--<asp:TextBox ID="txtUnitCost" Width="50" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"UnitCost") %>' />--%>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Quantity" ItemStyle-Width="60" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="ltQuantity" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Quantity") %>' />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <div class="gridInput">
                                                                    <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ControlToValidate="txtQuantity" ValidationGroup="vgRequired3pl" Display="Dynamic" ErrorMessage="*" />
                                                                    <asp:TextBox ID="txtQuantity" Width="50" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Quantity") %>' onKeyPress="return checkDec(event)" />
                                                                </div>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Total Cost after Discount" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <%--<asp:Literal ID="ltTotalCost" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TotalCost") %>' />--%>
                                                                <asp:Literal ID="ltTotalCostAfterDiscount" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TotalCostAfterDiscount") %>' />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <%--<asp:TextBox ID="txtTotalCost" Width="50" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TotalCost") %>' />--%>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Description" ItemStyle-Width="200">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="ltActivityRateDescription" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ActivityRateDescription") %>' />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <%--<asp:TextBox ID="txtDescription" Width="120" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Description") %>' />--%>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>


                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%" FooterText="Delete">
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="CheckAll" onclick="return check_uncheck (this);" runat="server" />
                                                                <asp:Label ID="lblOBDCancel" CssClass="smlText" runat="server" Text="Delete"></asp:Label>
                                                            </HeaderTemplate>
                                                            <FooterTemplate>
                                                                <asp:LinkButton ID="btnDelete3pl" Font-Underline="false" runat="server" OnClick="btnDelete3pl_Click" OnClientClick="return confirm('Are you sure want to delete?');">Delete<%=MRLWMSC21Common.CommonLogic.btnfaDelete %></asp:LinkButton>
                                                            </FooterTemplate>

                                                            <ItemTemplate>
                                                                <asp:Label ID="RecID" Visible="false" Text='<%# DataBinder.Eval (Container.DataItem, "TenantTransactionAccessorialCaptureID") %>' runat="server" />
                                                                <asp:CheckBox ID="deleteRec" runat="server"></asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:CommandField ValidationGroup="vgRequired3pl" ItemStyle-Font-Underline="false" ControlStyle-Font-Underline="false" ItemStyle-Width="30" ButtonType="Link" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint" ItemStyle-HorizontalAlign="left" CancelImageUrl="/Images/cancel.gif" EditImageUrl="/Images/edit.gif" EditText="<nobr> Edit <img src='../Images/redarrowright.gif' border='0' /></nobr>" ShowEditButton="True" UpdateImageUrl="/Images/save.gif" />



                                                    </Columns>

                                                </asp:GridView>

                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </div>


                        <%--<h3>6. Delivery Pack Slip </h3>--%>
                        <h3>5. Delivery Pack Slip </h3>
                        <div>
                            <%--      <asp:UpdateProgress ID="uprgasnimport" runat="server" AssociatedUpdatePanelID="upnlASNDetails">
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
                            <asp:UpdatePanel ChildrenAsTriggers="true" runat="server" ID="upnlPackagingDetails" UpdateMode="Always">
                                <%-- <Triggers>
                                          <asp:PostBackTrigger ControlID="lnkPackingupdate"/>
                    </Triggers>--%>
                                <ContentTemplate>
                                    <asp:Panel runat="server" ID="pnlPackagingdetails" HorizontalAlign="Center" Visible="false">
                                        <div>
                                            <div class="row">
                                                <div class="row">

                                                    <div class="col m4 s12">
                                                        <div class="FormLabels flex">
                                                            <asp:DropDownList ID="ddlHHType" runat="server" required=""></asp:DropDownList><%-- OnSelectedIndexChanged="ddlHHType_SelectedIndexChanged"--%>
                                                            <asp:RequiredFieldValidator ID="rfvddlHHTypes" SetFocusOnError="true" ValidationGroup="NewPackingDetails" ControlToValidate="ddlHHType" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" InitialValue="0" />
                                                            <label>HandlingType</label>
                                                            <span class="errorMsg"></span>
                                                            <%--<asp:DropDownList ID="ddlPackingStores" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="ddlPackingStores_SelectedIndexChanged" ></asp:DropDownList>--%>
                                                        </div>
                                                    </div>
                                                    <div class="col m4 s12">
                                                        <div class="FormLabels flex">
                                                            <asp:TextBox runat="server" ID="txtMaxCapacity" placeholder="Weight" onKeyPress="return checkNum(event)"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvtxtMaxCapacity" SetFocusOnError="true" ValidationGroup="NewPackingDetails" ControlToValidate="txtMaxCapacity" Display="Dynamic" EnableClientScript="true" ErrorMessage=" * " runat="server" />
                                                            <label>Weight(Kg)</label>
                                                            <span class="errorMsg"></span>
                                                            <%--<asp:TextBox runat="server" ID="txtPackedOn" width="120" ></asp:TextBox>--%>
                                                        </div>
                                                    </div>
                                                    <div class="col m4 s12">
                                                        <div class="FormLabels flex">
                                                            <%--<asp:TextBox runat="server" ID="txtVolume" placeholder="Volume" onKeyPress="return checkNum(event)"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvVolume" SetFocusOnError="true" ValidationGroup="NewPackingDetails" ControlToValidate="txtVolume" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" />
                                                            <label>Volume</label>
                                                            <span class="errorMsg"></span>--%>
                                                            <asp:TextBox runat="server" ID="txtLength" placeholder="Length" onKeyPress="return checkNum(event)"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvLength" SetFocusOnError="true" ValidationGroup="NewPackingDetails" ControlToValidate="txtLength" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" />
                                                            <label>Length(cm)</label>
                                                            <span class="errorMsg"></span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col m4 s12">
                                                        <div class="FormLabels flex">
                                                            <asp:TextBox runat="server" ID="txtWidth" placeholder="Width" onKeyPress="return checkNum(event)"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvWidth" SetFocusOnError="true" ValidationGroup="NewPackingDetails" ControlToValidate="txtWidth" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" />
                                                            <label>Width(cm)</label>
                                                            <span class="errorMsg"></span>
                                                        </div>
                                                    </div>
                                                    <div class="col m4 s12">
                                                        <div class="FormLabels flex">
                                                            <asp:TextBox runat="server" ID="txtHeight" placeholder="Height" onKeyPress="return checkNum(event)"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvHeight" SetFocusOnError="true" ValidationGroup="NewPackingDetails" ControlToValidate="txtHeight" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" />
                                                            <label>Height(cm)   </label>
                                                            <span class="errorMsg"></span>
                                                        </div>
                                                    </div>
                                                    <div class="col m4 s12">
                                                        <div class="FormLabels flex">
                                                            <asp:TextBox runat="server" ID="txtPkgCondition" CssClass="txt_slim"></asp:TextBox>
                                                            <asp:HiddenField ID="hdnPCondition" runat="server" />
                                                            <label>Package Condition</label>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col m4 s12">
                                                        <div class="FormLabels flex">
                                                            <asp:TextBox runat="server" ID="txtRemarks" placeholder="Remarks"></asp:TextBox>
                                                            <label>Remarks</label>
                                                        </div>
                                                    </div>

                                                    <div class="col m4 s12" style="float: right">
                                                        <div>
                                                            <br />
                                                            <button type="button" id="lnkPackingupdate" onclick="AddPackingSlipInfoSave();" class="btn btn-primary right">
                                                                Save <%=MRLWMSC21Common.CommonLogic.btnfaSave %>
                                                            </button>
                                                        </div>

                                                    </div>
                                                   
                                                </div>
                                            </div>


                                        </div>
                                      
                                        <div>
                                            <div class="row" style="margin-top: 10% !important; margin: auto !important;" id="divPackingSliptable"></div>

                                            <div>
                                            <div class="row">

                                                <div class="col m4 s12" style="float: right">
                                                    <div class="FormLabels flex">
                                                         <button type="button" id="btnPackingSlipUpdate" class="btn btn-primary right" onclick="UpdatePackingSlipInfo();">Update </button>
<%--                                                        <asp:LinkButton runat="server" ID="lnkPackingSlipUpdate" OnClick="lnkPackingSlipUpdate_Click" CssClass="btn btn-primary right"> Update <%=MRLWMSC21Common.CommonLogic.btnfaUpdate %> </asp:LinkButton>--%>

                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                            <%--    <asp:Panel runat="server" ID="pnlPackaging" HorizontalAlign="Center">--%>

                                            <%-- <asp:GridView ID="grdPackaging" AutoGenerateColumns="false" PageSize="10" runat="server" SkinID="gvLightSteelBlueNew">
                                                <Columns>
                                                    <asp:TemplateField HeaderStyle-Width="100" HeaderText="Packing Slip No.">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ltHeaderID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PackingSlipHeaderID") %>' Visible="false" />
                                                            <asp:Literal ID="ltPackingSlipNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PackingSlipNo") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="80" HeaderText="HandlingType">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ltHandlingType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "HandlingType") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="80" HeaderText="Max. Weight">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ltMaxWeight" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "MaxWeight") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="60" HeaderText="Max. Volume">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ltMaxVolume" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "MaxVolume") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="60" HeaderText="Delete">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkDelete" CssClass="chkDelete" runat="server"/>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:LinkButton ID="lnkDelete" runat="server" ForeColor="Blue" Font-Underline="false" 
                                                                OnClick="btnDeletePSN_Click" OnClientClick="return confirm('Are you sure want to delete?');">
                                                                <%=MRLWMSC21Common.CommonLogic.btnfaDelete %></asp:LinkButton>

                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="60" HeaderText="Add Item">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkPSNEdit" runat="server" data-toggle="modal" data-target="#SupModal" OnClientClick='<%# "f" %>' Text="<nobr><Modify class='material-icons ss'>mode_edit</i></nobr>" CssClass=""></asp:LinkButton>
                                                           
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="60" HeaderText="Print">
                                                        <ItemTemplate>
                                                            <button type='button' class='fa fa-print btn btn-warning' onclick='<%# "PrintPackingSlipMaterialInfo(" +Eval("PackingSlipHeaderID") + ");"%>'></button>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>--%>

                                            <%--  </asp:Panel>--%>
                                        </div>

                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>

                        <%--  <h3 > 7. Delivery Challan</h3>--%>
                        <%--   <div  >--%>

                        <!-- Start Packing Details --->


                        <asp:UpdatePanel ChildrenAsTriggers="true" ID="upnlPackingDetails" runat="server" UpdateMode="Always" Visible="false">

                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlPackingStores" EventName="SelectedIndexChanged" />
                            </Triggers>

                            <ContentTemplate>

                                <asp:Panel runat="server" ID="pnlPackingDetails" Visible="false">


                                    <table border="0" cellpadding="2" cellspacing="2" width="100%">

                                        <tr>
                                            <td class="FormLabels">
                                                <div class="flex">
                                                    <asp:DropDownList ID="ddlPackingStores" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPackingStores_SelectedIndexChanged" required=""></asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvddlPackingStores" SetFocusOnError="true" ValidationGroup="PackingDetails" ControlToValidate="ddlPackingStores" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" InitialValue="0" />
                                                    <label>Packing Store</label>
                                                    <span class="errorMsg"></span>
                                                    <%--<asp:DropDownList ID="ddlPackingStores" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="ddlPackingStores_SelectedIndexChanged" ></asp:DropDownList>--%>
                                                </div>
                                            </td>
                                            <td class="FormLabels">
                                                <div class="flex">
                                                    <asp:TextBox runat="server" ID="txtPackedOn" required="" onpaste="return false"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvtxtPackedOn" SetFocusOnError="true" ValidationGroup="PackingDetails" ControlToValidate="txtPackedOn" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" />
                                                    <label>PackedOn Date</label>
                                                    <%--<asp:TextBox runat="server" ID="txtPackedOn" width="120" ></asp:TextBox>--%>
                                                </div>
                                            </td>
                                            <td class="FormLabels">
                                                <div class="flex">
                                                    <asp:TextBox runat="server" ID="txtPackedOnTime" placeholder="Time"></asp:TextBox>

                                                    <span class="errorMsg"></span>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="FormLabels">
                                                <div class="flex">
                                                    <asp:TextBox runat="server" ID="txtPackedBy" SkinID="txt_Hidden_Req_Auto" required=""></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvtxtPackedBy" SetFocusOnError="true" ValidationGroup="PackingDetails" ControlToValidate="txtPackedBy" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" />
                                                    <label>Packed By</label>
                                                    <span class="errorMsg"></span>
                                                    <%--<asp:TextBox runat="server" ID="txtPackedBy" SkinID="txt_Hidden_Req_Auto" ></asp:TextBox>--%>
                                                </div>
                                            </td>




                                            <td class="FormLabels">
                                                <div class="flex">
                                                    <div>
                                                        <asp:TextBox runat="server" ID="txtPackingRemarks" TextMode="MultiLine" required=""></asp:TextBox><label>Remarks</label>
                                                    </div>
                                                </div>

                                            </td>

                                            <td>

                                                <br />
                                                <asp:LinkButton runat="server" ID="lnkPakingSlip" OnClick="lnkPakingSlip_Click" CssClass="lnkViewButton">
                                                View Delivery Pack Slip  <span  class="space fa fa-external-link"></span>
                                                </asp:LinkButton>

                                                &nbsp;&nbsp;&nbsp;
                                            <asp:LinkButton runat="server" ID="lnkPackButCancel" OnClick="lnkPackButCancel_Click" CssClass="btn btn-primary right">
Clear <%=MRLWMSC21Common.CommonLogic.btnfaClear %>
                                            </asp:LinkButton>
                                                &nbsp;&nbsp;&nbsp;
                                            <asp:LinkButton runat="server" ID="lnkPackButSave" OnClick="lnkPackButSave_Click" ValidationGroup="PackingDetails" CssClass="btn btn-primary right">
Update <%=MRLWMSC21Common.CommonLogic.btnfaUpdate %>
                                            </asp:LinkButton>
                                            </td>
                                        </tr>

                                    </table>


                                </asp:Panel>




                            </ContentTemplate>

                        </asp:UpdatePanel>



                        <!-- End Packing Details --->

                        <%--</div>--%>







                        <!-- Section 5 -->
                        <%--<h3>7. Delivery Details</h3>--%>
                        <h3>6. Delivery Details</h3>
                        <div>

                            <!-- Start Received Delivery Details --->

                            <asp:UpdatePanel ChildrenAsTriggers="true" runat="server" ID="upnlReceivedDelivery" UpdateMode="Always">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlStores" EventName="SelectedIndexChanged" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:Panel runat="server" ID="pnlReceivedDelivery" style="display:none;">


                                        <table border="0" cellpadding="0" cellspacing="0" align="left" width="100%">

                                            <tr>
                                                <td class="SubHeading3" colspan="2" align="right">
                                                    <asp:Label runat="server" ID="lblDeliveryStore"></asp:Label>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="FormLabels" colspan="2">

                                                    <asp:Label runat="server" ID="lblReceivedDeliveryStatus"></asp:Label>

                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="FormLabels" colspan="2">
                                                    <div class="row">
                                                        <div class="col m4" style="padding: 20px;">
                                                            <div class="flex" style="">
                                                                <div class="fix-select__">
                                                                    <asp:DropDownList ID="ddlReceivingStore" SkinID="ddlRequired"  OnSelectedIndexChanged="ddlReceivingStore_SelectedIndexChanged" runat="server" required=""></asp:DropDownList>
                                                                    <label>Received From</label>
                                                                    <span class="errorMsg"></span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td colspan="2">

                                                    <table cellpadding="4" cellspacing="4" border="0" width="100%">

                                                        <tr>
                                                            <td class="FormLabels">
                                                                <div class="flex">
                                                                    <asp:DropDownList ID="ddlInstructionMode" SkinID="ddlRequired" runat="server" />
                                                                    <asp:RequiredFieldValidator ID="rfvddlInstructionMode" SetFocusOnError="true" ControlToValidate="ddlInstructionMode" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage="*" runat="server" ValidationGroup="Receivedelivery" />
                                                                    <label>
                                                                        <asp:Literal ID="ltInstructionMode" Text="Instruction Mode" runat="server" /></label>

                                                                    <%--<asp:DropDownList ID="ddlInstructionMode" SkinID="ddlRequired" runat="server" /> --%>
                                                                </div>
                                                            </td>
                                                            <td class="FormLabels">
                                                                <div class="flex">
                                                                    <asp:TextBox ID="txtRequester" SkinID="txt_Req" runat="server" required="" />
                                                                    <label>
                                                                        <asp:Literal ID="ltRequester" Text="Requester" runat="server" /></label>
                                                                    <%--<asp:TextBox ID="txtRequester" SkinID="txt_Req" runat="server" />--%>
                                                                </div>
                                                            </td>
                                                            <td class="FormLabels">
                                                                <div class="flex">
                                                                    <asp:TextBox ID="txtDocNumber" SkinID="txt_Req" runat="server" required="" />
                                                                    <label>
                                                                        <asp:Literal ID="ltDocumentNo" Text="Document No." runat="server" /></label>
                                                                    <%-- <asp:TextBox ID="txtDocNumber" SkinID="txt_Req" runat="server" required=""/>--%>
                                                                </div>
                                                            </td>
                                                            <td class="FormLabels">
                                                                <div class="flex">
                                                                    <asp:TextBox ID="txtDocRcvdDate" SkinID="txt_Req" runat="server" required="" />
                                                                    <asp:RequiredFieldValidator ID="rfvtxtDocRcvdDate" SetFocusOnError="true" ControlToValidate="txtDocRcvdDate" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage="*" runat="server" ValidationGroup="Receivedelivery" />
                                                                    <label>
                                                                        <asp:Literal ID="ltDocRcvdDate" Text="Doc. Rcvd. Date:" runat="server" /></label>
                                                                    <%-- <asp:TextBox ID="txtDocRcvdDate" SkinID="txt_Req" runat="server" />--%>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="FormLabels">
                                                                <div class="flex">
                                                                    <asp:TextBox ID="atcDeliveredBy" SkinID="txt_Auto" runat="server" required="" />
                                                                    <asp:HiddenField runat="server" ID="hifDeliveredByID" />
                                                                    <label>
                                                                        <asp:RequiredFieldValidator ID="rfvatcDeliveredBy" SetFocusOnError="true" ControlToValidate="atcDeliveredBy" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage="*" runat="server" ValidationGroup="Receivedelivery" />
                                                                        <asp:Literal ID="ltDeliveredBy" Text="Delivered By:" runat="server" /></label>
                                                                    <span class="errorMsg"></span>
                                                                    <%--   <asp:TextBox ID="atcDeliveredBy" SkinID="txt_Auto" runat="server"  />
                                                        <asp:HiddenField runat="server" ID="hifDeliveredByID" />--%>
                                                                </div>
                                                            </td>
                                                            <td class="FormLabels">
                                                                <div class="flex">
                                                                    <asp:TextBox ID="txtDriverName" SkinID="txt_Req" runat="server" required="" />
                                                                    <label>
                                                                        <asp:Literal ID="ltDriverName" Text="Driver Name" runat="server" /></label>
                                                                    <%--<asp:TextBox ID="txtDriverName" SkinID="txt_Req" runat="server" />--%>
                                                                </div>
                                                            </td>
                                                            <td class="FormLabels">
                                                                <div class="flex">
                                                                    <p class="label2">
                                                                        <asp:Literal ID="ltDeliveryDate" Text="Delivery Date:" runat="server" />
                                                                    </p>
                                                                    <asp:TextBox ID="txtDeliveryDate" SkinID="txt_Req" runat="server" Enabled="false" required="" />
                                                                    <asp:RequiredFieldValidator ID="rfvtxtDeliveryDate" SetFocusOnError="true" ControlToValidate="txtDeliveryDate" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage=" *" runat="server" ValidationGroup="Receivedelivery" />
                                                                    <%-- <p class="label2"><asp:Literal ID="ltDeliveryDate"  Text="Delivery Date:" runat="server" /></p>--%>
                                                                    <%--<asp:TextBox ID="txtDeliveryDate" SkinID="txt_Req" runat="server" Enabled="false" />--%>
                                                                </div>
                                                            </td>
                                                            <td class="FormLabels">
                                                                <div class="flex">
                                                                    <p class="label2">
                                                                        <asp:Literal ID="ltTime" Text="Time:" runat="server" />
                                                                    </p>
                                                                    <asp:TextBox ID="txtReceivedDelTimeEntry" SkinID="txt_Req" CssClass="timeEntry" runat="server" required="" />
                                                                    <%--  <label><asp:Literal ID="ltTime"  Text="Time:" runat="server" /></label>--%>
                                                                    <%--<asp:TextBox ID="txtReceivedDelTimeEntry" SkinID="txt_Req" CssClass="timeEntry" runat="server" /> --%>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="FormLabels">
                                                                <div class="flex">
                                                                    <asp:TextBox ID="txtRcvdBy" SkinID="txt_Req" runat="server" required="" />
                                                                    <label>
                                                                        <asp:Literal ID="ltReceivedBy" Text="Received By" runat="server" /></label>
                                                                    <%--<asp:TextBox ID="txtRcvdBy" SkinID="txt_Req" runat="server" required=""/>--%>
                                                                </div>
                                                            </td>
                                                            <td class="FormLabelsBlue">
                                                                <div class="flex evenlyy">
                                                                    <div>
                                                                        <label style="width: auto;">Used Vehicle Details</label>
                                                                    </div>
                                                                    <div>
                                                                        <asp:LinkButton runat="server" ID="lnkAddNewUsedVehicle" OnClick="lnkAddNewUsedVehicle_Click" Font-Underline="false"
                                                                            CssClass="btn btn-primary right">Add Vehicle <%=MRLWMSC21Common.CommonLogic.btnfaNew %></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                                    </div>
                                                                </div>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="FormLabels" valign="top">
                                                                <div class="flex">
                                                                    <asp:TextBox ID="txtRemBy_DeliveryIncharge" TextMode="MultiLine" CssClass="txt_slim_req" runat="server" required="" />
                                                                    <label>
                                                                        <asp:Literal ID="ltRemarksOnDelivery" Text="Remarks On Delivery" runat="server" /></label>
                                                                    <%--<asp:TextBox ID="txtRemBy_DeliveryIncharge" TextMode="MultiLine" CssClass="txt_slim_req" runat="server" />--%>
                                                                </div>
                                                            </td>
                                                            <td colspan="2" align="right" valign="top">

                                                                <!-- Used Vehicle Details -->



                                                                <asp:Panel runat="server" ID="pnlUsedVehicleGrid" HorizontalAlign="left">
                                                                    <asp:Label ID="lblUsedVehicleStatus" runat="server"></asp:Label>
                                                                    <br />

                                                                    <asp:GridView ID="gvUsedVehicle" SkinID="gvLightSteelBlueNew" runat="server" Width="300px" CellPadding="4" AllowPaging="false" AllowSorting="false"
                                                                        OnRowDataBound="gvUsedVehicle_RowDataBound"
                                                                        OnRowCommand="gvUsedVehicle_RowCommand"
                                                                        OnRowUpdating="gvUsedVehicle_RowUpdating"
                                                                        OnRowCancelingEdit="gvUsedVehicle_RowCancelingEdit"
                                                                        OnRowEditing="gvUsedVehicle_RowEditing"
                                                                        PagerStyle-HorizontalAlign="Right"
                                                                        CellSpacing="0">

                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="Used Vehicle">
                                                                                <ItemTemplate>
                                                                                    <asp:Literal ID="ltUsedVehicle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "EquipmentName") %>' />
                                                                                    <asp:Literal ID="lthidUsedVehicleID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "UsedEquipmentID") %>' />
                                                                                </ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                    <div class="gridInput">
                                                                                        <asp:RequiredFieldValidator ID="rfvddlEquipmentType" SetFocusOnError="true" ControlToValidate="ddlEquipmentType" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" InitialValue="0" />
                                                                                        <asp:DropDownList ID="ddlEquipmentType" runat="server"></asp:DropDownList>
                                                                                        <asp:Literal runat="server" Visible="false" ID="ltEquipmentID" Text='<%# DataBinder.Eval(Container.DataItem, "EquipmentID") %>'></asp:Literal>
                                                                                        <asp:Literal ID="lthidUsedVehicleID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "UsedEquipmentID") %>' />
                                                                                    </div>
                                                                                </EditItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Used Value" ItemStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:Literal ID="ltUsedValue" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "UsedValue") %>' />
                                                                                </ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                    <div class="gridInput">
                                                                                        <asp:RequiredFieldValidator ID="rfvtxtUsedValue" SetFocusOnError="true" ControlToValidate="txtUsedValue" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" />
                                                                                        <asp:TextBox ID="txtUsedValue" Width="80" onKeyPress="return checkNum(event)" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "UsedValue") %>'></asp:TextBox>
                                                                                    </div>
                                                                                </EditItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint">
                                                                                <HeaderTemplate>
                                                                                    <nobr>Delete</nobr>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="chkIsDeleteUsedVItem" runat="server" />
                                                                                </ItemTemplate>
                                                                                <EditItemTemplate></EditItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:LinkButton ID="lnkDeleteUsedVItem" Font-Underline="false" CssClass="blueLink" runat="server" Text="<nobr> Delete <img src='../Images/redarrowright.gif' border='0' /></nobr>" OnClick="lnkDeleteUsedVItem_Click" OnClientClick="return Confirm('Are you sure ? want to delete selected items')" />
                                                                                </FooterTemplate>
                                                                            </asp:TemplateField>

                                                                            <asp:BoundField ItemStyle-Font-Underline="false" Visible="false" DataField="EditName" ReadOnly="true" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint" />

                                                                            <asp:CommandField ItemStyle-Font-Underline="false" ControlStyle-Font-Underline="false" ItemStyle-Width="30" ButtonType="Link" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint" ItemStyle-HorizontalAlign="left" CancelImageUrl="/Images/cancel.gif" EditImageUrl="/Images/edit.gif" EditText="<nobr> Edit <img src='../Images/redarrowright.gif' border='0' /></nobr>" ShowEditButton="True" UpdateImageUrl="/Images/save.gif" />
                                                                        </Columns>
                                                                    </asp:GridView>

                                                                </asp:Panel>




                                                                <!-- Used Vehicle Details -->
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="FormLabels">
                                                                <div class="flex">
                                                                    <div>
                                                                        <p class="label2" style="width: auto !important">
                                                                            <asp:Literal ID="ltPOD" Text="Attach Proof of Delivery (POD)" runat="server" />
                                                                        </p>
                                                                    </div>

                                                                    <div>



                                                                        <asp:UpdatePanel ChildrenAsTriggers="true" ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                                                            <Triggers>
                                                                                <asp:PostBackTrigger ControlID="lnkDelivery" />
                                                                            </Triggers>
                                                                            <ContentTemplate>

                                                                                <asp:FileUpload ID="fucODeliveryConfirmReciept" runat="server" CssClass="txt_slim_req" />
                                                                                <p style="font-size: 11px !important;">
                                                                                    Note: <span style="color: red; font-size: 10px;">*</span><span style="color: slategray; font-size: 9px !important; font-style: italic; font-weight: 400;">Max Filesize: 5MB
                                                                                    <br />
                                                                                        &emsp;&emsp;&emsp;<span style="color: red; font-size: 10px;">*</span> FileTypes Allowed : PDF</span>
                                                                                </p>
                                                                                <asp:Literal ID="ltPODAttacthment" runat="server" />

                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </div>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="">
                                                                    <div class="checkbox">
                                                                        <asp:CheckBox ID="IsDCRReceived" Text="Is Proof of Delivery (POD) received from the customer?" CssClass="txt_slim flex__ slipmi__" runat="server" required="" />

                                                                        <%--<label>Is Proof of Delivery (POD) received from the customer?</label>--%>
                                                                    </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" colspan="4">

                                                                <asp:LinkButton ID="lnkCancelDelivery" runat="server" OnClick="lnkCancelDelivery_Click" CssClass="btn btn-primary right">
Clear <%=MRLWMSC21Common.CommonLogic.btnfaClear %>
                                                                </asp:LinkButton>
                                                                &nbsp; &nbsp; 
                                                        
                                                        <asp:LinkButton ID="lnkCancelRecvfromSystem" Visible="false" Font-Underline="false" runat="server" Text="Cancel from System" OnClientClick="return confirm('Are you sure you want to cancel this Delv. Doc. from the System? All the line items associated with this Delv. Doc\'s. pick transcations will be cancled.Please ensure that the items are placed back in the respective location they are picked from with the help of \'Delivery Pick Note.\' ');" OnClick="lnkCancelRecvfromSystem_Click" SkinID="lnkButCancel" />
                                                                &nbsp; &nbsp 
                                                        
                                                        <asp:LinkButton ID="lnkDelivery" CausesValidation="True" OnClientClick="return checkDelvVehTypeMand();Page_ValidationActive=true;" ValidationGroup="Receivedelivery" runat="server" OnClick="lnkDelivery_Click" CssClass="btn btn-primary right">
Update <%=MRLWMSC21Common.CommonLogic.btnfaUpdate %>
                                                        </asp:LinkButton>

                                                                <asp:ValidationSummary ID="ValidationSummary2" DisplayMode="List" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="Receivedelivery" ForeColor="red" Font-Bold="true" />
                                                            </td>
                                                        </tr>

                                                    </table>


                                                </td>
                                            </tr>
                                        </table>

                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>


                            <!-- End Received Delivery Details --->


                        </div>

                    </div>
                    <asp:HiddenField ID="hidAccordionIndex" runat="server" />
                </td>
            </tr>




        </table>



        <!-- Goods-Out Pending List Dialog   -->
        <div id="divItemPrintDataContainer">
            <div id="divItemPrintData" style="display: block;">
                <asp:UpdatePanel ID="upnlGoodsIN" runat="server">
                    <ContentTemplate>


                        <asp:Panel ID="pnlPendingGoodsOutList" runat="server" ScrollBars="Auto">


                            <div style="padding-left: 10px; padding-right: 10px;">
                                <br />
                                <asp:GridView Width="100%" ShowFooter="true" ID="gvPendingGoodsOutList" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="true" PageSize="20" SkinID="gvLightSteelBlueNew" HorizontalAlign="Left" OnPageIndexChanging="gvPendingGoodsOutList_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="150" HeaderText="SO Line#">
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="ltLineNumber" Text='<%# DataBinder.Eval(Container.DataItem, "LineNumber") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="250" HeaderText="Part Number">
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="ltMCode" Text='<%# DataBinder.Eval(Container.DataItem, "MCode") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-Width="150" HeaderText="SO Number">
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="ltPONumber" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "SONumber") %>' />
                                                <img src="../Images/redarrowleft.gif" />
                                                <asp:HyperLink ID="HyperLink1" Text='<%# DataBinder.Eval(Container.DataItem, "SONumber") %>' NavigateUrl='<%# String.Format("../mOrders/SalesOrderInfo.aspx?soid={0}",DataBinder.Eval(Container.DataItem, "SOHeaderID").ToString()) %>' Font-Underline="false" runat="server"></asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-Width="150" HeaderText="SO Qty.">
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="ltInvoiveQty" Text='<%# DataBinder.Eval(Container.DataItem, "SOQuantity") %>' />
                                            </ItemTemplate>

                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="150" HeaderText="Picked Qty.">
                                            <ItemTemplate>
                                                <asp:Literal runat="server" ID="ltReceivedQty" Text='<%# DataBinder.Eval(Container.DataItem, "PickedQty") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>

                                <br />
                                <br />
                            </div>

                        </asp:Panel>

                        <br />


                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
  

    <asp:HiddenField ID="hifSOHeaderID" runat="server" />
    <asp:HiddenField ID="hifPackedByID" runat="server" />
    <asp:HiddenField ID="hifActualCustomerPOID" runat="server" />
    <asp:HiddenField ID="hifActualInvoice" runat="server" />

    <style>
       

        .home span {
            font-size: 0pt;
            display: block;
            position: absolute;
            left: 8px;
            width: 7%;
            height: 2px;
            background: red;
            bottom: 0px;
            left: 0px;
            margin-bottom: 0px;
            border-radius: 5px;
            color: #fff !important;
        }



        @media (min-width: 768px) {
            .modal-dialog {
                width: 60vw;
                margin: 30px auto;
            }
        }
    </style>
    <!-- Modal -->

    <div id="SupModal" class="modal fade" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="height: 450px;">
                <div class="modal-header" style="background-color: var(--sideNav-bg) !important; color: #fff !important;">
                    <h4 class="modal-title" style="display: inline !important;">&nbsp;&nbsp;<label id="lblPSN"></label></h4>
                    <button type="button" data-dismiss="modal" class="pull-right modalclose" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>

                <div class="modal-body" id="mySupForm">
                    <div class="row">
                        <div class="col-md-4">
                            <div class="flex">
                                <div>
                                    <asp:TextBox ID="txtPSNMaterial" Width="200" CssClass="txt_slim" runat="server" />
                                    <asp:HiddenField ID="hdnMaterial" runat="server" />

                                    <%--                                    <asp:DropDownList ID="ddlPSNMaterial" runat="server" required="" onchange="GetSelectedTextValue(this)"></asp:DropDownList> OnSelectedIndexChanged="ddlHHType_SelectedIndexChanged"--%>
                                    <asp:RequiredFieldValidator ID="RFMaterial" SetFocusOnError="true" ValidationGroup="PackingDetailsPop" ControlToValidate="txtPSNMaterial" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" InitialValue="0" />
                                    <label>Item</label>
                                    <span class="errorMsg"></span>
                                </div>

                                <input type="hidden" id="hdnPSNHeaderID" />
                                <input type="hidden" id="hdnPSNMaxWeight" />
                                <input type="hidden" id="hdnPSNMaxVol" />
                                <input type="hidden" id="txtPickedUOM" />
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="flex">
                                <div>
                                    <asp:TextBox ID="txtPickedQty" runat="server" required="required" ReadOnly="true"></asp:TextBox>
                                    <%--   <asp:Label runat="server" ID="txtPickedQty" placeholder="Picked qty."></asp:Label>--%>
                                    <%--<input type="text" name="name"  value="" />--%>
                                    <label>Picked Qty.</label>
                                </div>
                            </div>
                        </div>
                        <%-- <div class="col-md-0" style="display: none">
                            <div class="flex">
                                <div>
                                    <asp:TextBox ID="txtPickedUOM" runat="server" required="required" ReadOnly="true"></asp:TextBox>
                                    <label>Picked UOM.</label>
                                </div>
                            </div>
                        </div>--%>
                        <div class="col-md-4">
                            <div class="flex">
                                <div>
                                    <asp:TextBox ID="txtPackedUOM" Width="200" CssClass="txt_slim" runat="server" />
                                    <asp:HiddenField ID="hdnPackedUOM" runat="server" />
                                    <asp:RequiredFieldValidator ID="RFVtxtPackedUOM" SetFocusOnError="true" ValidationGroup="PackingDetailsPop" ControlToValidate="txtPackedUOM" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" InitialValue="0" />
                                    <label>UOM</label>
                                    <span class="errorMsg"></span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="flex">
                                <div>
                                    <asp:TextBox runat="server" ID="txtPackedQty" placeholder="Packed Qty." onKeyPress="return checkNum(event)"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RFVPackedQty" SetFocusOnError="true" ValidationGroup="PackingDetailsPop" ControlToValidate="txtPackedQty" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" />
                                    <label>Packed Qty.</label>
                                    <span class="errorMsg"></span>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="flex">
                                <div>
                                    <%--<asp:LinkButton runat="server" ID="lnkPSNItemdetails" OnClientClick="" ValidationGroup="PackingDetailsPop" CssClass="btn btn-primary right">Save <%=MRLWMSC21Common.CommonLogic.btnfaSave %>
                                    </asp:LinkButton>--%>
                                    <button type="button" id="btnSave" class="btn btn-primary right" onclick="AddPackingSlipMaterialInfoSave();">Save </button>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <div class="">
                                    <div class="row" style="margin-top: 8% !important; margin: auto !important;" id="divPSNtable"></div>
                                </div>
                            </div>

                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
    <!-- Modal -->

</asp:Content>
