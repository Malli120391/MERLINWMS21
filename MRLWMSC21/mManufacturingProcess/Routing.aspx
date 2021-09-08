<%@ Page Title=" Routing :." Language="C#" MasterPageFile="~/mManufacturingProcess/Manufacturing.master" AutoEventWireup="true" CodeBehind="Routing.aspx.cs" Inherits="MRLWMSC21.mManufacturingProcess.Routing" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ManfContent" runat="server">
    <asp:ScriptManager runat="server"  ID="ss"  EnablePartialRendering="true" SupportsPartialRendering="true"></asp:ScriptManager>

    <script type="text/javascript" src="Scripts/CommonScripts.js"></script>



    <script type="text/javascript">

        function checkCustomDec(textbox, evt) {



            evt = (evt) ? evt : window.event
            var charCode = (evt.which) ? evt.which : evt.keyCode

            

            if (charCode == 46 || charCode == 45) {
                
                if (textbox.value.indexOf(".") == "-" || textbox.value.indexOf(".") < 0)
                    return true;
                else
                    return false;
            }
            if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 46) {
                status = "This field accepts numbers only."
                return false
            }
            status = "";
            return true;
        }


    </script>

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
            position: fixed;
            height: 200px;
            overflow-y: scroll;
        }

        .pretextwrap {
            white-space: pre-wrap;
        }
    </style>
    <style type="text/css">
        .showonhover .hovertext {
            display: none;
        }

        .showonhover:hover .hovertext {
            display: inline;
        }

        div.viewdescription {
            color: #999;
        }

            div.viewdescription:hover {
                background-color: #999;
                color: White;
            }

        .hovertext {
            position: absolute;
            z-index: 1000;
            border: 2px solid #ffd971;
            background-color: #9cb70f;
            padding: 5px;
            width: 200px;
            font-size: 15px;
        }

        .rotate {
            -webkit-transform: rotate(90deg);
            -moz-transform: rotate(90deg);
            -ms-transform: rotate(90deg);
            -o-transform: rotate(90deg);
            transform: rotate(90deg);
            filter: progid:DXImageTransform.Microsoft.BasicImage(rotation=1);
            color: #ff6500;
            font-size: 40px;
            font-family: Calibri;
            position: fixed;
            font-weight: bold;
            letter-spacing: 2px;
            text-shadow: 0px 0px 5px #808080;
        }
    </style>
    <style>
        img {
            border-style: none;
        }

        .MCodePicker,.ParameterDataTypePicker {
            position: relative;
            font-family: Calibri;
            position: relative;
            color: #000000;
            font-size: 13pt;
            padding-right: 20px;
            background-image: url('../Images/Down-BFBFBF.png');
            background-repeat: no-repeat;
            background-position: right;
            cursor: pointer;
            width: 180px;
            border: 1px solid #848484;
            border-radius: 3px;
        }

            .MCodePicker:focus,.ParameterDataTypePicker:focus {
                outline: none;
                box-shadow: 0px 0px 5px #FFB133;
                border:1px solid #FFB133;
                border-radius: 5px;
                background-image: url('../Images/Down_Orange-FFB133.png') ;
                background-repeat: no-repeat;
                background-position: right;
            }

       
       

            
    </style>
    <script type="text/javascript">

        function Collapse() {
            var toggle = document.getElementsByClassName("toggle");
            var img;
            for (var i = 0; i < toggle.length; i++) {
                img = document.getElementById('img' + toggle[i].id);
                img.src = "../plus.gif";
                toggle[i].style.display = "none";
                //toggle[i].style.display = "block";
            }

        }

        function Expand() {
            var toggle = document.getElementsByClassName("toggle");
            var img;
            for (var i = 0; i < toggle.length; i++) {
                img = document.getElementById('img' + toggle[i].id);
                img.src = "../minus.gif";
                toggle[i].style.display = "block";
            }
            return false;
        }

        $(document).ready(function () {
            $("#divProdDefcList").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 400,
                minWidth: 500,
                width: 'auto',
                resizable: false,
                draggable: false,
                position: ["center top", 40],
                title: "Routing/BOM Deficiency List",
                open: function (event, ui) {
                    $("#divProdDefcList").hide().fadeIn(500);
                    $('body').css({ 'overflow': 'hidden' });
                    $('body').width($('body').width());
                    $(document).bind('scroll', function () {
                        window.scrollTo(0, 0);
                    });
                    $(this).parent().appendTo("#divProdDefcListContainer");
                },
                close: function () {
                    $("#divProdDefcList").fadeOut(500);
                    $(document).unbind('scroll');
                    $('body').css({ 'overflow': 'visible' });
                }
            });
        });
        function ProdBlockDialog() {

            //block it to clean out the data
          
        }
        function ProdCloseDialog() {

            //Could cause an infinite loop because of "on close handling"
            $("#divProdDefcList").dialog('close');

        }

        function ProdOpenPrintDialog() {
            $("#divProdDefcList").dialog({
                autoOpen: false,
            });
        }
        function ProdOpenDialog() {

            $("#divProdDefcList").dialog('open');

            NProgress.start();

            $("#divProdDefcList").block({
                message: '<img src="<%=ResolveUrl("~") %>Images/async_inb.gif" />',
                css: { border: '0px' },
                fadeIn: 0,
                fadeOut: 0,
                overlayCSS: { backgroundColor: '#ffffff', opacity: 1 }
            });
        }

        function unblockProdDialog() {
            $("#divProdDefcList").unblock();
            NProgress.done();
        }


    </script>
    
    <script>
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                fnMCodeAC();
            }
        }


        function fnMCodeAC() {
            $(document).ready(function () {

                $(".DateBoxCSS_small").datepicker({ dateFormat: "dd/mm/yy" });

                $("#<%= this.txtJRPReleseDate.ClientID %>").datepicker({ dateFormat: "dd/mm/yy" });
                $("#<%= this.txtICReleaseDate.ClientID %>").datepicker({ dateFormat: "dd/mm/yy" });
                $("#<%= this.txtECNDate.ClientID %>").datepicker({ dateFormat: "dd/mm/yy" });
                $("#<%= this.txtEffectedDate.ClientID %>").datepicker({ dateFormat: "dd/mm/yy" });



                var TextfieldName = $('#<%=txtRoutingRefNo.ClientID%>');
                DropdownFunction(TextfieldName);
                $('#<%=txtRoutingRefNo.ClientID%>').autocomplete({
                    source: function (request, response) {

                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadRoutingRevMcodes") %>',
                            data: "{ 'prefix': '" + request.term + "'}",
                            dataType: "json",
                            type: "POST",
                            async: true,
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {

                                if (data.d == "") {
                                    alert("No Materials are configured");
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

                            }


                        });
                    },
                    select: function (e, i) {

                        $("#<%=this.hifMMRvID.ClientID%>").val(i.item.val);
                    },
                    minLength: 0
                });


                var TextfieldName = $('#<%=txtFinishedMaterial.ClientID%>');
                DropdownFunction(TextfieldName);
                $('#<%=txtFinishedMaterial.ClientID%>').autocomplete({
                    source: function (request, response) {

                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadBOMFinishedMcodes") %>',
                            data: "{ 'prefix': '" + request.term + "','BOMHeaderRevID':'" + document.getElementById("<%=hifBoMHeaderRevID.ClientID %>").value + "','RoutingDocTypeID':'" + document.getElementById("<%=hifRoutingDocTypeID.ClientID %>").value + "'}",
                            dataType: "json",
                            type: "POST",
                            async: true,
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {

                                if (data.d == "") {
                                    alert("No Materials are configured");
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

                            }


                        });
                    },
                    select: function (e, i) {

                        $("#<%=this.hifFinishedMRvID.ClientID%>").val(i.item.val);
                    },
                    minLength: 0
                });



                var TextfieldName = $('#<%=txtBOMrefNo.ClientID%>');
                DropdownFunction(TextfieldName);
                $('#<%=txtBOMrefNo.ClientID%>').autocomplete({
                    source: function (request, response) {

                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadBoMRefWithRevsions") %>',
                            data: "{ 'prefix': '" + request.term + "','MMRevisionID':'" + document.getElementById("<%=hifMMRvID.ClientID %>").value + "'}",
                            dataType: "json",
                            type: "POST",
                            async: true,
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {

                                if (data.d == "") {
                                    alert("No BOM's are configured to the selected part number");
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

                            }


                        });
                    },
                    select: function (e, i) {

                        $("#<%=this.hifBoMHeaderRevID.ClientID%>").val(i.item.val);
                    },
                    minLength: 0
                });




                var TextfieldName = $('#<%=txtSourceActivity.ClientID%>');
                DropdownFunction(TextfieldName);
                $('#<%=txtSourceActivity.ClientID%>').autocomplete({
                    source: function (request, response) {

                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadSourceActivityList") %>',
                            data: "{ 'prefix': '" + request.term + "','RoutingHeaderID':'" + document.getElementById("<%=this.hifRoutingHeaderID.ClientID%>").value + "','RoutingDetailsActivityID':'" + document.getElementById("<%=this.hifSourceActivityRoutingDetailsActivityID.ClientID%>").value + "','DisplayNumber':'" + document.getElementById("<%=this.hifDisplayNumber.ClientID%>").value + "','DisplayOrder':'" + document.getElementById("<%=this.hifDisplayOrder.ClientID%>").value + "','ActivityID':'" + document.getElementById("<%=this.hifSActivityID.ClientID%>").value + "'}",
                            dataType: "json",
                            type: "POST",
                            async: true,
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {

                                if (data.d == "") {
                                    alert("No 'Source Activities' are configured");
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

                            }


                        });
                    },
                    select: function (e, i) {

                        $("#<%=this.hifSourceActivityID.ClientID%>").val(i.item.val);
                    },
                    minLength: 0
                });









                var TextfieldName = $('.NonConfirmityTypes');
                DropdownFunction(TextfieldName);
                $('.NonConfirmityTypes').autocomplete({
                    source: function (request, response) {

                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadNonConfirmityTypes") %>',
                            data: "{ 'prefix': '" + request.term + "'}",
                            dataType: "json",
                            type: "POST",
                            async: true,
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


                        $("#<%=this.hifNonConfirmityTypeID.ClientID%>").val(i.item.val);



                    },
                    minLength: 0
                });

                try {
                    var TextfieldName = $('.MCodePicker');
                    DropdownFunction(TextfieldName);
                    $('.MCodePicker').autocomplete({
                        source: function (request, response) {

                            $.ajax({
                                url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadBoMMCodesWithOEM") %>',
                                data: "{ 'prefix': '" + request.term + "','BoMHeaderID':'" + document.getElementById("<%=hifBoMHeaderID.ClientID %>").value + "','MaterialMasterRevisionID':'" + document.getElementById("<%=hifFinishedMRvID.ClientID %>").value + "','RoutingDocTypeID':'" + document.getElementById("<%=hifRoutingDocTypeID.ClientID %>").value + "'}",
                                dataType: "json",
                                type: "POST",
                                async: true,
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {

                                    if (data.d == "") {
                                        alert("No Materials are configured to this Routing");
                                        return;
                                    }
                                    else {

                                        response($.map(data.d, function (item) {
                                            return {

                                                label: item.split('~')[0].split('`')[0],
                                                description: item.split('~')[0].split('`')[1] == undefined ? "" : " <font color='#086A87'  >" + item.split('~')[0].split('`')[1] + "</font>",
                                                val: item.split('~')[1]
                                            }
                                        }))
                                    }

                                }


                            });
                        },
                        select: function (e, i) {

                        },
                        minLength: 0
                    }).data("autocomplete")._renderItem = function (ul, item) {
                        // Inside of _renderItem you can use any property that exists on each item that we built
                        // with $.map above */
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a>" + item.label + "" + item.description + "</a>")
                            .appendTo(ul)
                    };
                } catch (ex) { }

                var TextfieldName = $('.ScrapCodePicker');
                DropdownFunction(TextfieldName);
                $('.ScrapCodePicker').autocomplete({
                    source: function (request, response) {

                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadNonConfirmityCode") %>',
                            data: "{ 'prefix': '" + request.term + "','NonConfirmityTypeID':'" + document.getElementById("<%=hifNonConfirmityTypeID.ClientID %>").value + "'}",
                            dataType: "json",
                            type: "POST",
                            async: true,
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
                                alert(response.responseText);
                            },
                            failure: function (response) {
                                alert(response.responseText);
                            }

                        });
                    },
                    minLength: 0
                });


                var TextfieldName = $('#txtParameterDataType');
                DropdownFunction(TextfieldName);
                $('#txtParameterDataType').autocomplete({
                    source: function (request, response) {

                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadParameterDataTypes") %>',
                            data: "{ 'prefix': '" + request.term + "'}",
                            dataType: "json",
                            type: "POST",
                            async: true,
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
                        $("#<%=hifQCparameterID.ClientID %>").val(i.item.val);


                        if (document.getElementById('<%= hifQCparameterID.ClientID %>').value == "2") {
                            document.getElementById('txtMinValue').style.display = "inline";
                            document.getElementById('txtMaxValue').style.display = "inline";
                        } else {
                            document.getElementById('txtMinValue').style.display = "none";
                            document.getElementById('txtMaxValue').style.display = "none";
                        }

                    },
                    minLength: 0
                });



                var TextfieldName = $("#<%=this.txtWorkCenterGroup.ClientID%>");
                DropdownFunction(TextfieldName);
                $("#<%=this.txtWorkCenterGroup.ClientID%>").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadWorkCenterGroups") %>',
                            data: "{ 'prefix': '" + request.term + "','RoutingHeaderID':'0'}",
                            dataType: "json",
                            type: "POST",
                            async: true,
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

                    },
                    minLength: 0
                });

                var TextfieldName = $("#<%=this.txtRoutingDocumentType.ClientID%>");
                DropdownFunction(TextfieldName);
                $("#<%=this.txtRoutingDocumentType.ClientID%>").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadRoutingDocumentType") %>',
                            data: "{ 'prefix': '" + request.term + "'}",
                            dataType: "json",
                            type: "POST",
                            async: true,
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

                        $("#<%=hifRoutingDocTypeID.ClientID %>").val(i.item.val);

                    },
                    minLength: 0
                });

                var TextfieldName = $("#<%=this.txtCheckListType.ClientID%>");
                DropdownFunction(TextfieldName);
                $("#<%=this.txtCheckListType.ClientID%>").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadQCCheckListNames") %>',
                            data: "{ 'prefix': '" + request.term + "'}",
                            dataType: "json",
                            type: "POST",
                            async: true,
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

                        $("#<%=hifCheckListTypeID.ClientID %>").val(i.item.val);

                    },
                    minLength: 0
                });



                var TextfieldName = $("#<%=this.txtTERCaptureType.ClientID%>");
                DropdownFunction(TextfieldName);
                $("#<%=this.txtTERCaptureType.ClientID%>").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadTERCaptureType") %>',
                            data: "{ 'prefix': '" + request.term + "'}",
                            dataType: "json",
                            type: "POST",
                            async: true,
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

                        $("#<%=hifTERCaptureTypeID.ClientID %>").val(i.item.val);

                    },
                    minLength: 0
                });









                var TextfieldName = $("#<%=this.atcSupplier.ClientID%>");
                DropdownFunction(TextfieldName);
                $("#<%= this.atcSupplier.ClientID %>").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadServiceSupplierData") %>',
                            data: "{ 'prefix': '" + request.term + "','TenantID':'" +<%=cp.TenantID%> +"'}",
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

                        $("#<%=hifSupplier.ClientID %>").val(i.item.val);
                    },
                    minLength: 0
                });


                var TextfieldName = $("#<%=this.txtApprovedBy.ClientID%>");
                DropdownFunction(TextfieldName);
                $("#<%= this.txtApprovedBy.ClientID %>").autocomplete({
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
                        $("#<%=hifApprovedBy.ClientID %>").val(i.item.val);
                    },
                    minLength: 0
                });


                var TextfieldName = $("#<%=this.txtCheckedBy.ClientID%>");
                DropdownFunction(TextfieldName);
                $("#<%= this.txtCheckedBy.ClientID %>").autocomplete({
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



                var TextfieldName = $("#<%=this.txtPreparedBy.ClientID%>");
                DropdownFunction(TextfieldName);
                $("#<%= this.txtPreparedBy.ClientID %>").autocomplete({
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
                        $("#<%=hifPreparedBy.ClientID %>").val(i.item.val);
                    },
                    minLength: 0
                });




                var TextfieldName = $("#<%=this.txtJRPApprovedBy.ClientID%>");
                DropdownFunction(TextfieldName);
                $("#<%= this.txtJRPApprovedBy.ClientID %>").autocomplete({
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
                        $("#<%=hifJRPApprovedByID.ClientID %>").val(i.item.val);
                    },
                    minLength: 0
                });


                var TextfieldName = $("#<%=this.txtJRPReviewedBy.ClientID%>");
                DropdownFunction(TextfieldName);
                $("#<%= this.txtJRPReviewedBy.ClientID %>").autocomplete({
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
                        $("#<%=hifJRPReviewedByID.ClientID %>").val(i.item.val);
                    },
                    minLength: 0
                });

                var TextfieldName = $("#<%=this.txtJRPPreparedBy.ClientID%>");
                DropdownFunction(TextfieldName);
                $("#<%= this.txtJRPPreparedBy.ClientID %>").autocomplete({
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
                        $("#<%=hifJRPPreparedByID.ClientID %>").val(i.item.val);
                    },
                    minLength: 0
                });




            });
            }
            fnMCodeAC();

            function CheckIsDelted(checkBox) {
                if (checkBox.checked) {
                    alert('Are you sure want to delete routing?');
                }
            }

            function HideGridviewColumn() { }
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#divEditInvoiceDispute").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 200,
                minWidth: 500,
                height:600,
                width: 'auto',
                overflow: 'auto',
                resizable: true,
                draggable: true,
                position: ["center top", 40],
                title: "Add Non Confirmity Details",
                open: function (event, ui) {
                    $("#divEditInvoiceDispute").hide().fadeIn(500);
                    $('body').css({ 'overflow': 'hidden' });
                    $('body').width($('body').width());
                    $(document).bind('scroll', function () {
                        window.scrollTo(0, 0);
                    });
                    $(this).parent().appendTo("#divEditInvoiceDisputeDlgContainer");
                },
                close: function () {
                    $("#divEditInvoiceDispute").fadeOut(500);
                    $(document).unbind('scroll');
                    $('body').css({ 'overflow': 'visible' });
                }
            });
        });
       
        function closeDialog() {
            //Could cause an infinite loop because of "on close handling"
            $("#divEditInvoiceDispute").dialog('close');
        }
        function openDialog() {
            $("#divEditInvoiceDispute").dialog('open');
        }

        function openDialog(title, linkID) {

            $("#divEditInvoiceDispute").dialog("option", "title", "Add Non Confirmity Details");
            $("#divEditInvoiceDispute").dialog('open');
        }

        function openDialog(title) {

            $("#divEditInvoiceDispute").dialog("option", "title", "Add Non Confirmity Details");
            $("#divEditInvoiceDispute").dialog('open');
        }

        function openDialogAndBlock(title, linkID) {

            openDialog(title, linkID);

        }


    </script>

    <script type="text/javascript">
        $(document).ready(function () {

            $("#divEditQualityDispute").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 200,
                minWidth: 450,
                height: 650,
                width: 950,
                resizable: false,
                draggable: false,
                position: ["center top", 40],
                title: "Check Point Details",
                open: function (event, ui) {
                    $("#divEditQualityDispute").hide().fadeIn(500);
                    $('body').css({ 'overflow': 'hidden' });
                    $('body').width($('body').width());
                    $(document).bind('scroll', function () {
                        window.scrollTo(0, 0);
                    });
                    $(this).parent().appendTo("#divEditQualityDisputeDlgContainer");
                },
                close: function () {
                    $("#divEditQualityDispute").fadeOut(500);
                    $(document).unbind('scroll');
                    $('body').css({ 'overflow': 'visible' });
                }
            });
        });

        function closeDialog1() {
            //Could cause an infinite loop because of "on close handling"
            $("#divEditQualityDispute").dialog('close');
        }


        function openDialog1(title, linkID) {

            $("#divEditQualityDispute").dialog("option", "title", "Check Point Details");
            $("#divEditQualityDispute").dialog('open');
        }


        function openDialogAndBlock1(title, linkID) {

            openDialog1(title, linkID);

            NProgress.start();

            $("#divEditQualityDispute").block({
                message: '<img src="<%=ResolveUrl("~") %>Images/async_inb.gif" />',
                 css: { border: '0px' },
                 fadeIn: 0,
                 fadeOut: 0,
                 overlayCSS: { backgroundColor: '#ffffff', opacity: 1 }
             });

        }

        function unblockQualityDialog() {
            $("#divEditQualityDispute").unblock();

            NProgress.done();
        }




    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#divEditTERDispute").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 200,
                minWidth: 450,
                height: 600,
                width: 950,
                resizable: false,
                draggable: false,
                position: ["center top", 40],
                title: "TER Check Point Details",
                open: function (event, ui) {
                    $("#divEditTERDispute").hide().fadeIn(500);
                    $('body').css({ 'overflow': 'hidden' });
                    $('body').width($('body').width());
                    $(document).bind('scroll', function () {
                        window.scrollTo(0, 0);
                    });
                    $(this).parent().appendTo("#divEditTERDisputeDlgContainer");
                },
               
                close: function () {
                    $("#divEditTERDispute").fadeOut(500);
                    $(document).unbind('scroll');
                    $('body').css({ 'overflow': 'visible' });
                }
              
            });
        });

        function closeTERDialog1() {
            //Could cause an infinite loop because of "on close handling"
            $("#divEditTERDispute").dialog('close');
        }


        function openTERDialog1() {

            $("#divEditTERDispute").dialog("option", "title", "TER Check Point Details");
            $("#divEditTERDispute").dialog('open');
            NProgress.start();
            $("#divEditTERDispute").block({
                message: '<img src="<%=ResolveUrl("~") %>Images/async_inb.gif" />',
                css: { border: '0px' },
                fadeIn: 0,
                fadeOut: 0,
                overlayCSS: { backgroundColor: '#ffffff', opacity: 1 }
            });

            unblockTERDialog();
        }


        function unblockTERDialog() {
            $("#divEditTERDispute").unblock();
            NProgress.done();
        }




    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#divEditRoutActivityDispute").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 200,
                minWidth: 600,
                height: 670,
                width: 400,
                resizable: false,
                draggable: false,
                position: ["center top", 40],
                open: function (event, ui) {
                    $("#divEditRoutActivityDispute").hide().fadeIn(500);
                    $('body').css({ 'overflow': 'hidden' });
                    $('body').width($('body').width());
                    $(document).bind('scroll', function () {
                        window.scrollTo(0, 0);
                    });
                    $(this).parent().appendTo("#divEditRoutActivityDisputeDlgContainer");
                },
             
                close: function () {
                    $("#divEditRoutActivityDispute").fadeOut(500);
                    $(document).unbind('scroll');
                    $('body').css({ 'overflow': 'visible' });
                }
            });

            $("#divComponentsDispute").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 200,
                minWidth: 600,
                height:600,
                width: 800,
                resizable: false,
                draggable: false,
                position: ["center top", 40],
                title: "Component Details",
                open: function (event, ui) {
                    $("#divComponentsDispute").hide().fadeIn(500);
                    $('body').css({ 'overflow': 'hidden' });
                    $('body').width($('body').width());
                    $(document).bind('scroll', function () {
                        window.scrollTo(0, 0);
                    });
                    $(this).parent().appendTo("#divActivityLevelComponents");
                },
               
                close: function () {
                    $("#divComponentsDispute").fadeOut(500);
                    $(document).unbind('scroll');
                    $('body').css({ 'overflow': 'visible' });
                }
            });
        });


        function closeActivityDialog() {
            //Could cause an infinite loop because of "on close handling"
            $("#divEditRoutActivityDispute").dialog('close');
        }


        function openActivityDialog(title, linkID) {

            $("#divEditRoutActivityDispute").dialog("option", "title", "Add Activity");
            $("#divEditRoutActivityDispute").dialog('open');
            NProgress.start();
            $("#divEditRoutActivityDispute").block({
                message: '<img src="<%=ResolveUrl("~") %>Images/async_inb.gif" />',
                 css: { border: '0px' },
                 fadeIn: 0,
                 fadeOut: 0,
                 overlayCSS: { backgroundColor: '#ffffff', opacity: 1 }
             });

        }

        function unblockActivityDialog() {
            $("#divEditRoutActivityDispute").unblock();
            NProgress.done();
        }


        function openActivityDialogAndBlock(title, linkID) {

            openActivityDialog(title, linkID);
            unblockActivityDialog();
        }

        function openMcodeDialog(title) {
            //sssssssss

            $("#divComponentsDispute").dialog("option", "title", title);
            $("#divComponentsDispute").dialog('open');
            NProgress.start();
            $("#divComponentsDispute").block({
                message: '<img src="<%=ResolveUrl("~") %>Images/async_inb.gif" />',
                css: { border: '0px' },
                fadeIn: 0,
                fadeOut: 0,
                overlayCSS: { backgroundColor: '#ffffff', opacity: 1 }
            });
        }

        function CloseMcodeDialog() {
            $("#divComponentsDispute").dialog('close');
        }

        function unblockMcodeDialog() {
            $("#divComponentsDispute").unblock();
            NProgress.done();
        }




    </script>
    <script language="javascript" type="text/javascript">
        function expandcollapse(obj, row) {
            var div = document.getElementById(obj);
            var img = document.getElementById('img' + obj);

            if (div.style.display == "none") {
                div.style.display = "block";
                if (row == 'alt') {
                    img.src = "../minus.gif";
                }
                else {
                    img.src = "../minus.gif";
                }
                img.alt = "Close to view other Sequences";
            }
            else {
                div.style.display = "none";
                if (row == 'alt') {
                    img.src = "../plus.gif";
                }
                else {
                    img.src = "../plus.gif";
                }
                img.alt = "Expand to show Activities";
            }
        }

    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#divEditRevisionDispute").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                minWidth: 300,
                height: 350,
                width: 'auto',
                resizable: false,
                draggable: false,
                position: ["center top", 40],
                title: "Revision History",
                open: function (event, ui) {

                    $("#divEditRevisionDispute").hide().fadeIn(500);
                    $('body').css({ 'overflow': 'hidden' });
                    $('body').width($('body').width());
                    $(document).bind('scroll', function () {
                        window.scrollTo(0, 0);
                    });
                    $(this).parent().appendTo("#divEditRevisionDisputeDlgContainer");
                },
                close: function () {
                    $("#divEditRevisionDispute").fadeOut(500);
                    $(document).unbind('scroll');
                    $('body').css({ 'overflow': 'visible' });
                }
            });
        });

        function closeRevisionDialog() {
            //Could cause an infinite loop because of "on close handling"
            $("#divEditRevisionDispute").dialog('close');
        }


        function openRevisionDialog(title) {

            $("#divEditRevisionDispute").dialog("option", "title", "Revision History");
            $("#divEditRevisionDispute").dialog('open');
        }


        function openRevisionDialogAndBlock(title, linkID) {

            openRevisionDialog(title, linkID);
        }

    </script>

    <script type="text/javascript">

        function HideColumns(checkbox) {



            if (checkbox.checked) {


                document.getElementById('txtMinValue').style.display = "inline";
                document.getElementById('txtMaxValue').style.display = "inline";
                document.getElementById('txtParameterDataType').style.display = "inline";

            }
            else {

                document.getElementById('txtMinValue').style.display = "none";
                document.getElementById('txtMaxValue').style.display = "none";
                document.getElementById('txtParameterDataType').style.display = "none";


            }

        }

        function HideDefaultColumns() {
            document.getElementById('txtMinValue').style.display = "none";
            document.getElementById('txtMaxValue').style.display = "none";
            document.getElementById('txtParameterDataType').style.display = "none";
        }


        function CheckPoinOnFocus() {


            var checkPointText = document.getElementById("hifRoutingDetailsActivityQCID");



            $.ajax({
                url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/IsRecordTypeChecked") %>',
                data: "{  'RoutingHeaderID': '" + document.getElementById("<%=this.hifRoutingHeaderID.ClientID%>").value + "', 'RoutingDetailsActivityID': '" + checkPointText.value + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {

                    var parameterdata = data.d.split(',');

                    if (parameterdata[1] == "2") {

                        if (parameterdata[0] == "1") {
                            document.getElementById('txtMinValue').style.display = "inline";
                            document.getElementById('txtMaxValue').style.display = "inline";
                            document.getElementById('txtParameterDataType').style.display = "inline";
                        }
                        else {
                            document.getElementById('txtMinValue').style.display = "none";
                            document.getElementById('txtMaxValue').style.display = "none";
                            document.getElementById('txtParameterDataType').style.display = "none";
                        }
                    }
                    else {
                        if (parameterdata[0] == "1") {
                            document.getElementById('txtParameterDataType').style.display = "inline";
                            document.getElementById('txtMinValue').style.display = "none";
                            document.getElementById('txtMaxValue').style.display = "none";
                        }
                        else {
                            document.getElementById('txtMinValue').style.display = "none";
                            document.getElementById('txtMaxValue').style.display = "none";
                            document.getElementById('txtParameterDataType').style.display = "none";
                        }
                    }


                },
                error: function (response) {

                },
                failure: function (response) {

                }
            });




        }


    </script>
    
    <script>
        $(document).ready(function () {
            CustomAccordino($('#dvRoutingHeader'), $('#dvRoutingBody'));
            CustomAccordino($('#dvDocumentDtlsHeader'), $('#dvDocumentDtlsBody'));
            CustomAccordino($('#dvOSHeader'), $('#dvOSBody'));
        });


        function ValidateMins(TextBox) {

           

            if (TextBox.value == "") {
                return;
            }

            if (parseFloat(TextBox.value) > 59) {

                
                showStickyToast1("tue", "Minutes value cannot exceed 59", false);
                TextBox.value = "";
                TextBox.focus();
                return;

            }
        }


        function showStickyToast1(type, message, IsParmenent) {
            var val;
            var time;
            if (type == true)
                val = 'success';
            else
                val = 'error';
            $().toastmessage('showToast', {
                stayTime: 2600,
                text: message,
                sticky: IsParmenent,
                position: 'bottom-right',
                type: val,
                closeText: '',
                close: function () {
                },

            });

        }


    </script>

    <table border="0" cellpadding="0" cellspacing="0" width="1150px" align="center">

        <tr>
            <td >
                <table width="95%" align="center" cellspacing="3" cellpadding="3" class="pagewidth">


                    <tr>
                        <td colspan="2" align="left">
                            <span class="mandatory_field">Note: </span>
                            <asp:Label ID="lberrormsg" runat="server" CssClass="errorMsg" Text=" * " />
                            Indicates mandatory fields

                        </td>
                        <td align="right" valign="top" class="auto-style2" colspan="2"></td>
                    </tr>

                    <tr>
                        <td class="FormLabels" colspan="4" align="right">

                            <a class="ui-button-small" href="RoutingList.aspx">Routing List<%=MRLWMSC21Common.CommonLogic.btnfaList %></a>
                            &nbsp;&nbsp;
                <a class="ui-button-small" href="Routing.aspx">New Routing <%=MRLWMSC21Common.CommonLogic.btnfaNew %></a>

                        </td>
                    </tr>

                    <tr>
                        <td colspan="4">
                            <asp:Label CssClass="errorMsg" ID="lblStatus" runat="server" />
                        </td>
                    </tr>

                    <tr>
                        <td colspan="4">
                            <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="dvRoutingHeader">Routing Header</div>
                            <div class="ui-Customaccordion" id="dvRoutingBody">
                                <table width="100%" class="internalData">
                                    <tr>
                                        <td align="left" class="auto-style3">

                                            <asp:UpdatePanel ChildrenAsTriggers="true" ID="uppoNumber" UpdateMode="Always" runat="server">
                                                <ContentTemplate>
                                                    <asp:RequiredFieldValidator ID="rfvRoutingRefNo" runat="server" ValidationGroup="UpdateRouting" ControlToValidate="txtRoutingRefNo" Display="Dynamic" ErrorMessage=" * " />
                                                    <asp:Literal ID="lclRoutingRefNo" runat="server" Text="Routing Ref. No.:" /><br />
                                                    <asp:TextBox ID="txtRoutingRefNo" runat="server" SkinID="txt_Auto_Req" />

                                                </ContentTemplate>

                                            </asp:UpdatePanel>

                                        </td>
                                        <td class="auto-style1">
                                            <asp:RequiredFieldValidator ID="rfvtxtRevNo" runat="server" ValidationGroup="UpdateRouting" ControlToValidate="txtRevNo" Display="Dynamic" ErrorMessage=" * " />
                                            <asp:Literal ID="ltRevNo" runat="server" Text="Revision Number:" /><br />
                                            <asp:TextBox ID="txtRevNo" runat="server" Style="margin-left: 0px" />


                                        <td align="left" class="auto-style2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="UpdateRouting" ControlToValidate="txtRoutingName" Display="Dynamic" ErrorMessage=" * " />
                                            <asp:Literal ID="lclRoutingName" runat="server" Text="Routing Name:" /><br />
                                            <asp:TextBox ID="txtRoutingName" onKeypress="return checkSpecialChar(event)" runat="server"  />

                                        </td>
                                        <td align="left">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="UpdateRouting" ControlToValidate="atcSupplier" Display="Dynamic" ErrorMessage=" * " />
                                            <asp:Literal runat="server" ID="ltSupplier" Text="Supplier Name:" /><br />
                                            <asp:TextBox runat="server" ID="atcSupplier" SkinID="txt_Auto_Req" Style="margin-left: 0px" />
                                            <asp:HiddenField runat="server" ID="hifSupplier" />
                                        </td>

                                    </tr>


                                    <tr>

                                        <td class="FormLabels" valign="top">
                                            <asp:RequiredFieldValidator ID="rfvtxtRoutingDocumentType" runat="server" ValidationGroup="UpdateRouting" ControlToValidate="txtRoutingDocumentType" Display="Dynamic" ErrorMessage=" * " />
                                            Routing Document Type:
                            <br />

                                            <asp:TextBox runat="server" SkinID="txt_Auto_Req" ID="txtRoutingDocumentType"></asp:TextBox>
                                        </td>

                                        <td class="FormLabels" valign="top">
                                            <asp:RequiredFieldValidator ID="rfvtxtBOMrefNo" runat="server" ValidationGroup="UpdateRouting" ControlToValidate="txtBOMrefNo" Display="Dynamic" ErrorMessage=" * " />

                                            BOM Ref. #:
                            <br />

                                            <asp:TextBox runat="server" SkinID="txt_Auto_Req" ID="txtBOMrefNo"></asp:TextBox>

                                        </td>

                                        <td class="FormLabels" valign="top">

                                            <asp:RequiredFieldValidator ID="rfvtxtFinishedMaterial" runat="server" ValidationGroup="UpdateRouting" ControlToValidate="txtFinishedMaterial" Display="Dynamic" ErrorMessage=" * " />
                                            Finished Material:
                            <br />

                                            <asp:TextBox runat="server" SkinID="txt_Auto_Req" ID="txtFinishedMaterial"></asp:TextBox>
                                        </td>

                                        <td class="FormLabels" valign="top" style="visibility: hidden;">JAP Reference #:
                            <br />

                                            <asp:TextBox runat="server" Width="200" ID="txtJAPReferenceNo" TextMode="MultiLine" Rows="3" Columns="50"></asp:TextBox>

                                        </td>


                                    </tr>

                                    <tr>

                                        <td class="FormLabels" valign="top">JRP Release Date:
                            <br />
                                            <asp:TextBox runat="server" Width="200" ID="txtJRPReleseDate"></asp:TextBox>

                                        </td>
                                        <td class="FormLabels" valign="top">IC Rev.:
                            <br />

                                            <asp:TextBox runat="server" Width="200" ID="txtInspectionCheckListVer"></asp:TextBox>

                                        </td>



                                        <td class="FormLabels" valign="top" colspan="2">IC Release Date:
                            <br />

                                            <asp:TextBox runat="server" Width="200" ID="txtICReleaseDate"></asp:TextBox>

                                        </td>




                                    </tr>


                                    <tr>

                                        <td class="FormLabels" valign="top">ECN No.:  
                            <br />
                                            <asp:TextBox runat="server" Width="200" ID="txtECNNo"></asp:TextBox>
                                        </td>

                                        <td class="FormLabels" valign="top">ECN Date:
                            <br />
                                            <asp:TextBox runat="server" Width="200" ID="txtECNDate"></asp:TextBox>
                                        </td>
                                        <td class="FormLabels" valign="top">Effectivity:<br />
                                            <asp:TextBox runat="server" Width="200" ID="txtPreviousVer"></asp:TextBox>
                                        </td>
                                        <td class="FormLabels" valign="top">JAP Link:<br />
                                            <asp:TextBox runat="server" Width="200" ID="txtJAPLink"></asp:TextBox>
                                        </td>

                                    </tr>

                                    <tr>

                                        <td class="FormLabels">JAP Rev.:
                            <br />

                                            <asp:TextBox runat="server" ID="txtOtherReferences"></asp:TextBox>
                                        </td>

                                        <td class="FormLabels">JAP Ref.:
                            <br />
                                            <asp:TextBox runat="server" ID="txtJAPRev"></asp:TextBox>
                                        </td>

                                        <td class="FormLabels">BTP Ref.:
                            <br />
                                            <asp:TextBox runat="server" ID="txtBTPDocumentRefNo"></asp:TextBox>
                                        </td>

                                        <td class="FormLabels">BTP Rev.:
                            <br />
                                            <asp:TextBox runat="server" ID="txtBTPRev"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="FormLabels">CAG Drawing Ref.:
                            <br />
                                            <asp:TextBox runat="server" ID="txtCGMDrawingRefNo"></asp:TextBox>

                                        </td>
                                        <td class="FormLabels" colspan="3">CAG Drawing Rev.:
                            <br />
                                            <asp:TextBox runat="server" ID="txtCAGRev"></asp:TextBox>

                                        </td>

                                    </tr>
                                    <tr>

                                        <td class="FormLabels">JRP Prepared By:
                            <br />

                                            <asp:TextBox runat="server" ID="txtJRPPreparedBy" SkinID="txt_Auto_Req"></asp:TextBox>
                                        </td>


                                        <td class="FormLabels">JRP  Reviewed By:
                            <br />

                                            <asp:TextBox runat="server" ID="txtJRPReviewedBy" SkinID="txt_Auto_Req"></asp:TextBox>
                                        </td>

                                        <td class="FormLabels" colspan="2">JRP  Approved By:
                            <br />

                                            <asp:TextBox runat="server" ID="txtJRPApprovedBy" SkinID="txt_Auto_Req"></asp:TextBox>
                                        </td>

                                    </tr>
                                    <tr>

                                        <td class="FormLabels" valign="top">IC Prepared By:<br />
                                            <asp:TextBox runat="server" SkinID="txt_Auto_Req" ID="txtPreparedBy"></asp:TextBox>
                                        </td>

                                        <td class="FormLabels" valign="top">IC Reviewed By:
                            <br />
                                            <asp:TextBox runat="server" SkinID="txt_Auto_Req" ID="txtCheckedBy"></asp:TextBox>
                                        </td>

                                        <td class="FormLabels" valign="top" colspan="2">IC Approved By:<br />
                                            <asp:TextBox runat="server" SkinID="txt_Auto_Req" ID="txtApprovedBy"></asp:TextBox>
                                        </td>



                                    </tr>
                                    <tr>
                                        <td class="FormLabels" colspan="3">Remarks:
                            <br />

                                            <asp:TextBox runat="server" Width="500" ID="txtRoutRemarks" TextMode="MultiLine" Rows="5" Columns="100"></asp:TextBox>

                                        </td>
                                        <td class="FormLabels">Effective Date: &nbsp;&nbsp;&nbsp;

                 <asp:Label runat="server" ID="lblEffectiveDate"></asp:Label>

                                        </td>
                                    </tr>

                                    <tr>

                                        <td class="FormLabels" valign="top" colspan="4">
                                            <br />
                                            <asp:UpdatePanel runat="server" ID="upnlTercheckPointss" ChildrenAsTriggers="true" UpdateMode="Always">

                                                <ContentTemplate>

                                                    <asp:LinkButton Font-Underline="false" ID="lnkAddTERCheckPoints" Visible="false" runat="server" CssClass="RoutButEmpty" />

                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>

                                    </tr>

                                    <tr>

                                        <td align="right" colspan="4">

                                            <asp:CheckBox ID="chkIsActive" Text="Active" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="chkIsDeleted" onclick="CheckIsDelted(this);" Text="Delete" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;
                     
                                        </td>

                                    </tr>

                                    <tr>
                                        <td colspan="2">

                                            <asp:UpdatePanel ChildrenAsTriggers="true" ID="UpdatePanel1" UpdateMode="Always" runat="server">
                                                <ContentTemplate>
                                                    <asp:LinkButton runat="server" ID="lnkCopyRotDetails" CssClass="ui-button-small" OnClientClick="openRevisionDialog('Add New Revision');">Clone Routing<span class="space fa fa-copy" ></span> </asp:LinkButton>

                                                    &nbsp;&nbsp;&nbsp;

                                                 <asp:LinkButton runat="server" ID="lnkVewRoutingBOMDefc" Visible="false" SkinID="lnkButEmpty" Text="View Routing/BOM Deficiency" Font-Underline="false" OnClick="lnkVewRoutingBOMDefc_Click"></asp:LinkButton>

                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td colspan="2" align="right">

                                            <br />
                                            <asp:UpdatePanel ChildrenAsTriggers="true" ID="UpdatePanel2" UpdateMode="Always" runat="server">
                                                <ContentTemplate>

                                                    <asp:LinkButton ID="lnkClear" runat="server" CausesValidation="false" CssClass="ui-btn ui-button-large" OnClick="lnkClear_Click" >Cancel<%=MRLWMSC21Common.CommonLogic.btnfaClear %></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;

                                                    <asp:LinkButton ID="lnkUpdate" ValidationGroup="UpdateRouting" runat="server" CssClass="ui-btn ui-button-large" OnClick="lnkUpdate_Click" />

                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="4" id="tdRotBOMDefc" runat="server">
                                            <asp:UpdatePanel ChildrenAsTriggers="true" ID="upnlRoutDefc" UpdateMode="Always" runat="server">

                                                <ContentTemplate>


                                                    <asp:Label runat="server" ID="lblRoutDefciency" CssClass="SubHeadingBarRed" Text=" All BOM components related to this routing are not configured "></asp:Label>

                                                </ContentTemplate>

                                            </asp:UpdatePanel>

                                        </td>


                                    </tr>

                                </table>
                            </div>
                        </td>
                    </tr>
                    
                    <tr><td class="accordinoGap"></td></tr>
                    <tr>
                        <td colspan="4" id="tdDocRefDt" runat="server">
                            <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="dvDocumentDtlsHeader" >Document Ref. Details</div>
                            <div class="ui-Customaccordion" id="dvDocumentDtlsBody">
                                <table width="100%" class="pagewidth">
                                    <tr>
                                        <td align="right" colspan="4" style="padding:5px;">
                                            <asp:LinkButton ID="lnkAddDocRef" runat="server"  OnClick="lnkAddDocRef_Click" CssClass="ui-button-small" >Add Doc. Ref.<%=MRLWMSC21Common.CommonLogic.btnfaNew %></asp:LinkButton>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="4">

                                            <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" ID="upnlDocRefDetails" UpdateMode="Always">
                                                <ContentTemplate>

                                                    <asp:Label ID="lblDocRefStatus" runat="server" />


                                                    <asp:GridView Width="100%" ShowFooter="true" ID="gvRoutDocRef" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="true" PageSize="50" AllowSorting="True" SkinID="gvLightSteelBlueNew" HorizontalAlign="Left" OnPageIndexChanging="gvRoutDocRef_PageIndexChanging" OnRowDataBound="gvRoutDocRef_RowDataBound" OnRowEditing="gvRoutDocRef_RowEditing" OnRowUpdating="gvRoutDocRef_RowUpdating" OnRowCancelingEdit="gvRoutDocRef_RowCancelingEdit">
                                                        <Columns>

                                                            <asp:TemplateField ItemStyle-Width="100" HeaderText="Document Label" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltDocumentLabel" Text='<%# DataBinder.Eval(Container.DataItem, "DocumentLabel") %>' />
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:RequiredFieldValidator ID="rfvtxtDocumentLabel" ValidationGroup="UpdateRoutDocRef" runat="server" ControlToValidate="txtDocumentLabel" Display="Dynamic" ErrorMessage=" * " />
                                                                    <asp:TextBox runat="server" ID="txtDocumentLabel" Width="200" Text='<%# DataBinder.Eval(Container.DataItem, "DocumentLabel") %>' />
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-Width="80" HeaderText="Document Name" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal Visible="false" runat="server" ID="ltHidRoutingDocumentReferenceID" Text='<%# DataBinder.Eval(Container.DataItem, "RoutingDocumentReferenceID") %>' />
                                                                    <asp:Literal runat="server" ID="ltDocumentName" Text='<%# DataBinder.Eval(Container.DataItem, "DocumentName") %>' />

                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:Literal Visible="false" runat="server" ID="ltHidRoutingDocumentReferenceID" Text='<%# DataBinder.Eval(Container.DataItem, "RoutingDocumentReferenceID") %>' />
                                                                    <asp:TextBox ID="txtDocumentName" runat="server" Width="200" Text='<%# DataBinder.Eval(Container.DataItem, "DocumentName") %>' />
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>


                                                            <asp:TemplateField ItemStyle-Width="80" HeaderText="Remarks" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltFileLocation" Text='<%# DataBinder.Eval(Container.DataItem, "FileLocation") %>' />

                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="txtFileLocation" runat="server" Width="200" Text='<%# DataBinder.Eval(Container.DataItem, "FileLocation") %>' TextMode="MultiLine" Rows="5" Columns="50" />
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-Width="45" HeaderText="Delete" ItemStyle-CssClass="txtCenteralign" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkChildIsDelete" runat="server" />
                                                                </ItemTemplate>
                                                                <EditItemTemplate></EditItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:LinkButton Font-Underline="false" ID="lnkRotDocRedDelete" runat="server" Text="Delete" OnClick="lnkRotDocRedDelete_Click" OnClientClick="return confirm('Are you sure you want to delete the selected line items?')" />
                                                                    <img border="0" src="../Images/redarrowright.gif" alt="delete" />
                                                                </FooterTemplate>
                                                            </asp:TemplateField>

                                                            <asp:CommandField ItemStyle-HorizontalAlign="Center" ValidationGroup="UpdateRoutDocRef" CausesValidation="false" ControlStyle-Font-Underline="false" ItemStyle-Width="40" ButtonType="Link" CancelImageUrl="icons/cancel.gif" CancelText="Cancel" EditImageUrl="icons/edit.gif" EditText=" Edit <img src='../Images/redarrowright.gif' border='0' />" ShowEditButton="True" UpdateImageUrl="icons/update.gif" UpdateText="Update" />
                                                        </Columns>



                                                    </asp:GridView>



                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <br />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr><td class="accordinoGap"></td></tr>
                   
                     <tr>
                        <td id="tdlineitems" runat="server" colspan="4">
                            <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="dvOSHeader">Operation Sequences /Activities</div>
                            <div class="ui-Customaccordion" id="dvOSBody"> 
                                <table width="100%" class="pagewidth">
                                    <tr>
                                        <td align="right" colspan="4" style="padding:5px;">
                                            <asp:LinkButton ID="lnkAddNewLineItem" runat="server" OnClick="lnkAddNewLineItem_Click"  CssClass="ui-button-small" >Add Operation Seq.<%=MRLWMSC21Common.CommonLogic.btnfaNew %></asp:LinkButton>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="4">

                                            <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" ID="upnlRotDetails" UpdateMode="Always">
                                                <ContentTemplate>

                                                    <asp:Literal ID="ltGridStatus" runat="server" />
                                                    <asp:LinkButton runat="server" ID="LinkExpandgrid" CssClass="ui-button-small" OnClientClick="Expand(); return false" >Expand<span class="space fa fa-expand"></span></asp:LinkButton>
                                                    <asp:LinkButton runat="server" ID="lnkcolapsegrid" CssClass="ui-button-small"  OnClientClick="Collapse(); return false" >Collapse<span class="space fa  fa-compress"></span></asp:LinkButton>
                                                    <br /><br />

                                                    <asp:GridView ID="gvRoutingDetailsList" CssClass="gvRoutingDetails" runat="server" ShowFooter="true" PageSize="15" AutoGenerateColumns="false" OnPageIndexChanging="gvRoutingDetailsList_PageIndexChanging" OnRowEditing="gvRoutingDetailsList_RowEditing" OnRowUpdating="gvRoutingDetailsList_RowUpdating" OnRowCancelingEdit="gvRoutingDetailsList_RowCancelingEdit" OnRowCommand="gvRoutingDetailsList_RowCommand" OnRowDataBound="gvRoutingDetailsList_RowDataBound">
                                                        <Columns>

                                                            <asp:TemplateField ItemStyle-Width="10" ItemStyle-BackColor="#F5ECCE" HeaderStyle-BackColor="#BDBDBD">
                                                                <ItemTemplate>
                                                                    <a href="javascript:expandcollapse('div<%# DataBinder.Eval(Container.DataItem, "RoutingDetailsID").ToString() %>', 'one');">
                                                                        <img id='imgdiv<%# DataBinder.Eval(Container.DataItem, "RoutingDetailsID").ToString() %>' alt="Click to show/hide Activities <%# DataBinder.Eval(Container.DataItem, "RoutingDetailsID").ToString() %>" width="9px" border="0" src="../minus.gif" />
                                                                    </a>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Operation Sequence" ItemStyle-Width="79" ItemStyle-BackColor="#F5ECCE" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="Left" HeaderStyle-BackColor="#BDBDBD">

                                                                <HeaderTemplate>
                                                                    <asp:Label ID="Header" ToolTip="Operation Sequence/ Actvity No." runat="server" Text="Op.Seq./ Actv.No."></asp:Label>
                                                                </HeaderTemplate>

                                                                <ItemTemplate>
                                                                    <asp:Literal ID="ltSequenceNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "OperationNumber").ToString() %>' />
                                                                    <asp:Literal ID="ltRoutingDetailsID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "RoutingDetailsID").ToString() %>' />
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:RequiredFieldValidator ID="rfvtxtrfvName" ValidationGroup="UpdateGridItems" SetFocusOnError="true" ControlToValidate="ltSequenceNumber" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" />
                                                                    <asp:Literal ID="ltRoutingDetailsID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "RoutingDetailsID").ToString() %>' />
                                                                    <asp:TextBox ID="ltSequenceNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "OperationNumber").ToString() %>' />
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Routing /Activity" ItemStyle-Width="760" HeaderStyle-HorizontalAlign="left" ItemStyle-BackColor="#F5ECCE" HeaderStyle-BackColor="#BDBDBD">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="ltName" CssClass="SubHeading" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name").ToString() %>' />
                                                                </ItemTemplate>
                                                                <EditItemTemplate>

                                                                    <asp:RequiredFieldValidator ID="rfvtxtName" ValidationGroup="UpdateGridItems" SetFocusOnError="true" ControlToValidate="txtName" Display="Dynamic" EnableClientScript="true" CssClass="ErrorAlert2" ErrorMessage=" * " runat="server" />

                                                                    <asp:TextBox ID="txtName" runat="server" TextMode="MultiLine" Width="500" Height="30" ClientIDMode="Static" Text='<%# DataBinder.Eval(Container.DataItem, "Name").ToString() %>' />

                                                                </EditItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="D.O." ItemStyle-BackColor="#F5ECCE" HeaderStyle-BackColor="#BDBDBD" ItemStyle-Width="80">
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="DO" ToolTip="Display Order" runat="server" Text="D.O."></asp:Label>
                                                                </HeaderTemplate>


                                                                <ItemTemplate>
                                                                    <asp:Label ID="lttxtDisplayNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DisplayNumber").ToString() %>' />
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="txtDisplayNumber" onKeyPress="return checkNum(event)" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DisplayNumber").ToString() %>' />
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-Width="185" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="gvRoutingDetailsRowStyle" ItemStyle-BackColor="#F5ECCE" HeaderStyle-BackColor="#BDBDBD">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton Font-Underline="false" ID="lnkAddActivityDetails" runat="server" CssClass="RoutButEmpty" Text='<%# String.Format("Add Activity[{0}]", DataBinder.Eval(Container.DataItem, "ActivityCount").ToString()) %>' CommandName="EditChildItems" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "RoutingDetailsID") %> ' />
                                                                    <asp:Label ID="Label3" runat="server" Visible="false" Text='<%# String.Format("[{0}]", DataBinder.Eval(Container.DataItem, "ActivityCount").ToString()) %>'></asp:Label>

                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:LinkButton Font-Underline="false" ID="lnkAddActivityDetails" runat="server" Visible="false" Text="Activity Details" CommandName="EditChildItems" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "RoutingDetailsID") %> ' />
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Delete" ItemStyle-BackColor="#F5ECCE" ItemStyle-CssClass="gvRoutingDetailsRowStyle" FooterStyle-CssClass="gvRoutingDetailsRowStyle" ItemStyle-Width="90" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#BDBDBD">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkisdeleted" runat="server" />
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                </EditItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:LinkButton ID="lnkIsDeleted" runat="server" Text="Delete" OnClientClick="return confirm('Are you sure want to delete?')" Font-Underline="false" OnClick="lnkIsDeleted_Click" />
                                                                    <img src='../Images/redarrowright.gif' border='0' />
                                                                </FooterTemplate>
                                                            </asp:TemplateField>

                                                            <asp:CommandField HeaderStyle-BackColor="#BDBDBD" CausesValidation="false" ItemStyle-BackColor="#F5ECCE" ItemStyle-CssClass="gvRoutingDetailsRowStyle" ItemStyle-Font-Underline="false" ItemStyle-Width="35" ButtonType="Link" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint" ItemStyle-HorizontalAlign="left" CancelImageUrl="Images/cancel.gif" EditImageUrl="/Images/edit.gif" EditText=" Edit<img src='../Images/redarrowright.gif' border='0' />" ShowEditButton="True" UpdateImageUrl="/Images/save.gif" ControlStyle-Font-Underline="false" />

                                                            <asp:TemplateField HeaderStyle-BackColor="#BDBDBD">
                                                                <ItemTemplate>
                                                                    <tr>
                                                                        <td colspan="100%">

                                                                            <div class="toggle" id='div<%#  DataBinder.Eval(Container.DataItem, "RoutingDetailsID").ToString() %>' align="center" style="padding-left: 10px;">

                                                                                <asp:UpdatePanel ID="upnlRoutingActivity" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">

                                                                                    <ContentTemplate>

                                                                                        <asp:Panel ID="pnlgvRoutingChildActivity" runat="server">


                                                                                            <asp:GridView
                                                                                                ID="gvRoutingChildActivity"
                                                                                                Width="100%"
                                                                                                runat="server"
                                                                                                OnPageIndexChanging="gvRoutingChildActivity_PageIndexChanging"
                                                                                                OnRowCommand="gvRoutingChildActivity_RowCommand"
                                                                                                OnRowDataBound="gvRoutingChildActivity_RowDataBound"
                                                                                                OnRowEditing="gvRoutingChildActivity_RowEditing"
                                                                                                OnRowCancelingEdit="gvRoutingChildActivity_RowCancelingEdit"
                                                                                                OnRowUpdating="gvRoutingChildActivity_RowUpdating"
                                                                                                ShowHeader="false"
                                                                                                ShowFooter="false"
                                                                                                AutoGenerateColumns="false"
                                                                                                CellPadding="5"
                                                                                                CellSpacing="5"
                                                                                                CssClass="gvRoutingDetailsActivity">

                                                                                                <Columns>


                                                                                                    <asp:TemplateField ItemStyle-Width="60" HeaderText="Activity No." ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="Left" ItemStyle-BackColor="#EFF2FB">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Literal Visible="false" runat="server" ID="ltHidRoutingDetailsActivityID" Text='<%# DataBinder.Eval(Container.DataItem, "RoutingDetailsActivityID") %>' />
                                                                                                            <asp:Literal runat="server" ID="ltActivityCode" Text='<%# DataBinder.Eval(Container.DataItem, "ActivityCode") %>' />
                                                                                                            <asp:Literal Visible="false" runat="server" ID="lthidRoutingDetailsID" Text='<%# DataBinder.Eval(Container.DataItem, "RoutingDetailsID") %>' />
                                                                                                        </ItemTemplate>
                                                                                                        <EditItemTemplate>
                                                                                                            <asp:RequiredFieldValidator ID="rfvtxtScrapCode2" runat="server" ControlToValidate="txtActivityCode1" Display="Dynamic" ErrorMessage=" * " />
                                                                                                            <asp:Literal Visible="false" runat="server" ID="ltHidRoutingDetailsActivityID" Text='<%# DataBinder.Eval(Container.DataItem, "RoutingDetailsActivityID") %>' />
                                                                                                            <asp:TextBox ID="txtActivityCode1" EnableTheming="false" CssClass="ActivityCodePicker" runat="server" Width="160" Text='<%# DataBinder.Eval(Container.DataItem, "ActivityCode") %>' />
                                                                                                            <asp:Literal Visible="false" runat="server" ID="lthidRoutingDetailsID" Text='<%# DataBinder.Eval(Container.DataItem, "RoutingDetailsID") %>' />
                                                                                                        </EditItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField ItemStyle-Width="250" HeaderText="Instruction" ItemStyle-BackColor="#EFF2FB">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Literal runat="server" ID="ltMDescription" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>' />
                                                                                                        </ItemTemplate>
                                                                                                        <EditItemTemplate>
                                                                                                            <asp:TextBox runat="server" ID="txtDescription" TextMode="MultiLine" Width="150" Columns="5" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>' />
                                                                                                        </EditItemTemplate>
                                                                                                    </asp:TemplateField>

                                                                                                    <asp:TemplateField ItemStyle-Width="35" HeaderText="Workstation Type" ItemStyle-BackColor="#EFF2FB">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Literal runat="server" ID="ltWorkCenterGroup" Text='<%# DataBinder.Eval(Container.DataItem, "WorkCenterGroupCode") %>' />
                                                                                                        </ItemTemplate>
                                                                                                        <EditItemTemplate>
                                                                                                            <asp:RequiredFieldValidator ID="rfvtxtWorkCenterGroup1" runat="server" ControlToValidate="txtWorkCenterGroup" Display="Dynamic" ErrorMessage=" * " />
                                                                                                            <asp:TextBox runat="server" ID="txtWorkCenterGroup1" EnableTheming="false" CssClass="WorkGroupPicker" Text='<%# DataBinder.Eval(Container.DataItem, "WorkCenterGroup") %>' />
                                                                                                        </EditItemTemplate>
                                                                                                    </asp:TemplateField>

                                                                                                    <asp:TemplateField ItemStyle-Width="50" HeaderText="Operations" ItemStyle-BackColor="#EFF2FB">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Literal runat="server" ID="ltOperations" Text='<%# DataBinder.Eval(Container.DataItem, "Operations") %>' />
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>

                                                                                                    <asp:TemplateField ItemStyle-Width="25" HeaderText="Check List Type" ItemStyle-BackColor="#EFF2FB">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Literal runat="server" ID="ltCheckListName" Text='<%# DataBinder.Eval(Container.DataItem, "CheckListName") %>' />


                                                                                                            <asp:Literal runat="server" Visible="false" ID="ltQCCheckListConfigurationID" Text='<%# DataBinder.Eval(Container.DataItem, "QCCheckListConfigurationID") %>' />
                                                                                                        </ItemTemplate>





                                                                                                    </asp:TemplateField>

                                                                                                    <asp:TemplateField ItemStyle-Width="140" HeaderText="TER Capture Type" ItemStyle-BackColor="#EFF2FB">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Literal runat="server" ID="ltTERCaptureType" Text='<%# DataBinder.Eval(Container.DataItem, "TERCaptureType") %>' />
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>

                                                                                                    <asp:TemplateField ItemStyle-Width="30" HeaderText="Display Order" ItemStyle-BackColor="#EFF2FB">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Literal runat="server" ID="ltDisplayOrder" Text='<%# DataBinder.Eval(Container.DataItem, "DisplayOrder") %>' />
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>




                                                                                                    <asp:TemplateField ItemStyle-Width="130" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-BackColor="#EFF2FB">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:LinkButton Font-Underline="false" ID="lnkAddQCParameters" Visible="false" runat="server" Text='<%# String.Format("Check Points [{0}]", DataBinder.Eval(Container.DataItem, "QCLineItemCount").ToString()) %>' CssClass="RoutButEmpty" CommandName="EditQCItems" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "RoutingDetailsActivityID") %> ' />
                                                                                                            <asp:Label ID="lblQCLineItemCount" Visible="false" runat="server" Text='<%# String.Format("[{0}]", DataBinder.Eval(Container.DataItem, "QCLineItemCount").ToString()) %>'></asp:Label>

                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>

                                                                                                    <asp:TemplateField ItemStyle-Width="120" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-BackColor="#EFF2FB">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:LinkButton Font-Underline="false" ID="lnkAddComponents" runat="server" CssClass="RoutButEmpty" Text='<%# String.Format("Components[{0}]", DataBinder.Eval(Container.DataItem, "MMLineItemCount").ToString()) %>' CommandName="AddComponents" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "RoutingDetailsActivityID") %> ' />
                                                                                                            <asp:Label ID="lblMMLineItemCount" Visible="false" runat="server" Text='<%# String.Format("[{0}]", DataBinder.Eval(Container.DataItem, "MMLineItemCount").ToString()) %>'></asp:Label>

                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>



                                                                                                    <asp:TemplateField ItemStyle-Width="35" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-BackColor="#EFF2FB">

                                                                                                        <ItemTemplate>
                                                                                                            <asp:LinkButton Font-Underline="false" ID="lnkEditActivityItems" ToolTip="Edit Activity Details" runat="server" Text="Edit" CommandName="EditActivityItems" CommandArgument='<%# string.Format("{0},{1},{2}", DataBinder.Eval(Container.DataItem, "RoutingDetailsActivityID"),DataBinder.Eval(Container.DataItem, "RoutingDetailsID"),DataBinder.Eval(Container.DataItem, "DisplayOrder")) %> ' />
                                                                                                            <img src='../Images/redarrowright.gif' border='0' />
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>


                                                                                                </Columns>
                                                                                            </asp:GridView>

                                                                                        </asp:Panel>

                                                                                    </ContentTemplate>

                                                                                </asp:UpdatePanel>
                                                                            </div>

                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                        </Columns>
                                                    </asp:GridView>


                                                    <asp:HiddenField ID="hifSourceActivityRoutingDetailsActivityID" runat="server" />
                                                    <asp:HiddenField ID="hifDisplayNumber" runat="server" />
                                                    <asp:HiddenField ID="hifDisplayOrder" runat="server" />
                                                    <asp:HiddenField ID="hifSActivityID" runat="server"  />


                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr><td class="accordinoGap"></td></tr>

                </table>

            </td>

            <td valign="top" style="padding-top: 300px;">
                <span class="rotate">
                    <asp:Label runat="server" ID="lblRoutType" /></span>
            </td>

        </tr>

    </table>





    <!-- Start Routing Details Activity Items -->

    <div id="divEditRoutActivityDisputeDlgContainer">

        <div id="divEditRoutActivityDispute" style="display: none;">

            <asp:UpdatePanel ID="upnlRoutingActivity" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">

                <ContentTemplate>

                    <asp:Panel runat="server" >
                        <div class="dailoggap " style="height:545px">
                        <table border="0" cellpadding="3" cellspacing="3" width="100%">


                            <tr>
                                <td colspan="2">
                                    <asp:Label runat="server" ID="lblActivityStatus"></asp:Label>
                                </td>
                            </tr>

                            <tr>
                                <td class="FormLabels">
                                    <asp:RequiredFieldValidator ID="rfvtxtScrapCode" runat="server" ControlToValidate="txtActivityCode" Display="Dynamic" ErrorMessage=" * " />
                                    Activity Number:
                                </td>
                                <td class="FormLabels">


                                    <asp:TextBox ID="txtActivityCode" runat="server" Width="200" />

                                </td>
                            </tr>

                            <tr>
                                <td class="FormLabels">
                                    <asp:RequiredFieldValidator ID="rfvtxtDescription" runat="server" ControlToValidate="txtDescription" Display="Dynamic" ErrorMessage=" * " />
                                    Instruction:
                                </td>
                                <td class="FormLabels">

                                    <asp:TextBox runat="server" ID="txtDescription" TextMode="MultiLine" Width="300" Columns="5" Height="150" />
                                </td>
                            </tr>

                            <tr>
                                <td class="FormLabels">Operations:   
                                </td>
                                <td class="FormLabels">
                                    <asp:CheckBoxList ID="oprChkBoxList" runat="server" RepeatColumns="4" RepeatDirection="Horizontal" TextAlign="Right" CellSpacing="7">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>

                            <tr>
                                <td class="FormLabels">Source Activity: 
                                </td>
                                <td class="FormLabels">
                                    <asp:TextBox ID="txtSourceActivity" runat="server" SkinID="txt_Auto_Req" Width="180" />
                                </td>
                            </tr>


                            <tr>
                                <td class="FormLabels">
                                    <asp:RequiredFieldValidator ID="rfvtxtWorkCenterGroup" runat="server" ControlToValidate="txtWorkCenterGroup" Display="Dynamic" ErrorMessage=" * " />
                                    Workstation Type:
                                </td>
                                <td class="FormLabels">


                                    <asp:TextBox runat="server" ID="txtWorkCenterGroup" SkinID="txt_Auto_Req" Width="180" />

                                </td>
                            </tr>

                            <tr>
                                <td class="FormLabels">Check List Type:
                                </td>
                                <td class="FormLabels">

                                    <asp:TextBox runat="server" ID="txtCheckListType" SkinID="txt_Auto_Req" Width="180" />

                                </td>
                            </tr>


                            <tr>
                                <td class="FormLabels">TER Capture Type:
                                </td>
                                <td class="FormLabels">

                                    <asp:TextBox runat="server" ID="txtTERCaptureType" SkinID="txt_Auto_Req" Width="180" />

                                </td>
                            </tr>



                            <tr>
                                <td class="FormLabels">
                                    <asp:RequiredFieldValidator ID="rfvtxtCycleTimeInHours" runat="server" ControlToValidate="txtCycleTimeInHours" Display="Dynamic" ErrorMessage=" * " />
                                    Cycle Time In(Hrs:Mins):
                                </td>
                                <td class="FormLabels">
                                    
                                    <asp:TextBox runat="server" ID="txtCycleTimeInHours" onKeyPress="return checkNum(event)" MaxLength="4" Width="60" placeholder="Hours" />&nbsp;
                                    :&nbsp;
                                    <asp:TextBox runat="server" ID="txtCycleTimeInMins" onKeyPress="return checkNum(event)" MaxLength="2" Width="50" placeholder="Mins"  onblur="javascript:ValidateMins(this);" />
                                   

                                   


                                </td>
                            </tr>


                            <tr>
                                <td class="FormLabels">
                                    <asp:RequiredFieldValidator ID="rfvtxtDisplayOrder" runat="server" ControlToValidate="txtDisplayOrder" Display="Dynamic" ErrorMessage=" * " />
                                    Display Order:
                                </td>
                                <td class="FormLabels">

                                    <asp:TextBox runat="server" ID="txtDisplayOrder" onKeyPress="return checkNum(event)" />

                                </td>
                            </tr>

                        </table>   
                            </div>
                        <div class="ui-dailog-footer">
                            <div style="padding: 15px 13px 15px 5px;">
                            <asp:LinkButton ID="lnkRoutCancel" OnClick="lnkRoutCancel_Click" CausesValidation="false"  runat="server" CssClass="ui-btn ui-button-large">Close<%=MRLWMSC21Common.CommonLogic.btnfaClear %></asp:LinkButton>
                            <asp:LinkButton ID="lnkRoutDelete" OnClick="lnkRoutDelete_Click"  runat="server" CssClass="ui-btn ui-button-large">Delete<%=MRLWMSC21Common.CommonLogic.btnfaDelete %></asp:LinkButton>
                            <asp:LinkButton runat="server" ID="lnkRoutActivityUpdate" OnClick="lnkRoutActivityUpdate_Click" CssClass="ui-btn ui-button-large">Update<%=MRLWMSC21Common.CommonLogic.btnfaUpdate %></asp:LinkButton>
                            </div>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
    </div>





    <!-- End Routing Details Activity Items  -->




    <!-- Start Scrap Items -->

    <div id="divEditInvoiceDisputeDlgContainer">

        <div id="divEditInvoiceDispute" style="display: none;">

            <asp:UpdatePanel ID="upnlEditCustomer" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">

                <ContentTemplate>

                    <div class="dailoggap ui-dailog-body" style="height:560px">
                        <table cellpadding="3" cellspacing="3" border="0">
                        <tr>
                            <td class="SubHeading2"></td>
                            <td align="right">
                                <asp:LinkButton ID="lnkAddNew" runat="server" CssClass="ui-button-small" OnClick="lnkAddNewItem_Click" >Add Non Confirmity Codes<%=MRLWMSC21Common.CommonLogic.btnfaNew %></asp:LinkButton>

                            </td>
                        </tr>

                        <tr>
                            <td colspan="2">
                                <asp:Label runat="server" ID="lblRoutStatus" />

                                <asp:Panel ID="pnlGridContainer" runat="server" Width="800" ScrollBars="Auto">
                                    <asp:GridView Width="100%" ShowFooter="true" GridLines="Both" ID="gvRoutScrapCodes" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="true" PageSize="50" AllowSorting="True" SkinID="gvLightSteelBlueNew" HorizontalAlign="Left" OnPageIndexChanging="gvRoutScrapCodes_PageIndexChanging" OnRowDataBound="gvRoutScrapCodes_RowDataBound" OnRowEditing="gvRoutScrapCodes_RowEditing" OnRowUpdating="gvRoutScrapCodes_RowUpdating" OnRowCancelingEdit="gvRoutScrapCodes_RowCancelingEdit" OnRowCommand="gvRoutScrapCodes_RowCommand">
                                        <Columns>

                                            <asp:TemplateField ItemStyle-Width="100" HeaderText="Non Confirmity Type" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltNonConfirmityType" Text='<%# DataBinder.Eval(Container.DataItem, "NonConfirmityType") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:RequiredFieldValidator ID="rfvtxtNonConfirmityType" ValidationGroup="UpdateRoutScrapCodes" runat="server" ControlToValidate="txtNonConfirmityType" Display="Dynamic" ErrorMessage=" * " />
                                                    <asp:TextBox runat="server" ID="txtNonConfirmityType" EnableTheming="false" CssClass="NonConfirmityTypes" Text='<%# DataBinder.Eval(Container.DataItem, "NonConfirmityType") %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-Width="80" HeaderText="Non Confirmity Code" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal Visible="false" runat="server" ID="ltHidRoutingDetails_ScrapCodeID" Text='<%# DataBinder.Eval(Container.DataItem, "RoutingDetailsActivity_NonConfirmityID") %>' />
                                                    <asp:Literal runat="server" ID="ltScrapCode" Text='<%# DataBinder.Eval(Container.DataItem, "NonConfirmityCode") %>' />

                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:RequiredFieldValidator ID="rfvtxtScrapCode1" ValidationGroup="UpdateRoutScrapCodes" runat="server" ControlToValidate="txtScrapCode" Display="Dynamic" ErrorMessage=" * " />
                                                    <asp:Literal Visible="false" runat="server" ID="ltHidRoutingDetails_ScrapCodeID" Text='<%# DataBinder.Eval(Container.DataItem, "RoutingDetailsActivity_NonConfirmityID") %>' />
                                                    <asp:TextBox ID="txtScrapCode" EnableTheming="false" CssClass="ScrapCodePicker" runat="server" Width="160" Text='<%# DataBinder.Eval(Container.DataItem, "NonConfirmityCode") %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-Width="100" HeaderText="Description" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltMDescription" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtDescription" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-Width="45" HeaderText="Delete" ItemStyle-CssClass="txtCenteralign" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkChildIsDelete" runat="server" />
                                                </ItemTemplate>
                                                <EditItemTemplate></EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:LinkButton Font-Underline="false" ID="lnkRoutSCRDelete" runat="server" Text="Delete" OnClick="lnkRoutSCRDelete_Click" OnClientClick="return confirm('Are you sure you want to delete the selected items?')" />
                                                    <img border="0" src="../Images/redarrowright.gif" alt="delete" />
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField Visible="false" DataField="EditName" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                            <asp:CommandField ValidationGroup="UpdateRoutScrapCodes" CausesValidation="false" ItemStyle-HorizontalAlign="Center" ControlStyle-Font-Underline="false" ItemStyle-Width="40" ButtonType="Link" CancelImageUrl="icons/cancel.gif" CancelText="Cancel" EditImageUrl="icons/edit.gif" EditText=" Edit <img src='../Images/redarrowright.gif' border='0' />" ShowEditButton="True" UpdateImageUrl="icons/update.gif" UpdateText="Update" />
                                        </Columns>

                                        <PagerSettings FirstPageText="&amp;lt;&amp;lt;First Page" LastPageText="Last Page&amp;gt;&amp;gt;" Mode="NumericFirstLast" PageButtonCount="15" />

                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                        </tr>

                                
                    </table>
                    </div>
                    <div class="ui-dailog-footer">
                            <div style="padding: 15px 13px 15px 5px;">
                                <asp:LinkButton ID="btnCancel" OnClick="btnCancel_Click" CssClass="ui-btn ui-button-large" CausesValidation="false"  runat="server" >Close<%=MRLWMSC21Common.CommonLogic.btnfaClear %></asp:LinkButton>
                            </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>


    <!-- End Scrap Items -->



    <!-- Start Quality Parameters -->

    <div id="divEditQualityDisputeDlgContainer">

        <div id="divEditQualityDispute" style="display: none;">

            <asp:UpdatePanel ID="upnlQualityParameters" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">

                <ContentTemplate>

                    <div class="dailoggap ui-dailog-body" style="height:525px">
                    <table cellpadding="3" cellspacing="3" border="0" width="100%">
                        <tr>
                            <td class="FormLabels" style="position: fixed;" runat="server" id="tdCheckPointFormat">
                                <span style="font-weight: bold; font-size: 15pt; color: #ff6500;">Checkpoint Format:-</span> <span style="font-weight: bold; font-size: 12pt;">[ Print Data ] | Part Number | Reference | Dimension (Inches) | Quantity (Nos.)</span>
                            </td>
                            <td align="right">
                                <asp:LinkButton ID="lnkAddQualityParameter" runat="server" CssClass="ui-button-small"  OnClick="lnkAddQualityParameter_Click" >Add Check Point<%=MRLWMSC21Common.CommonLogic.btnfaNew %></asp:LinkButton>

                            </td>
                        </tr>

                        <tr>
                            <td colspan="2">
                                <asp:Label runat="server" ID="lblQualityStatus" />

                                <asp:Panel ID="pnlQualityParameters" runat="server">
                                    <asp:GridView Width="100%" ShowFooter="true" GridLines="Both" ID="gvQualityParameters" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="true" PageSize="50" AllowSorting="True" SkinID="gvLightSteelBlueNew" HorizontalAlign="Left" OnPageIndexChanging="gvQualityParameters_PageIndexChanging" OnRowDataBound="gvQualityParameters_RowDataBound" OnRowEditing="gvQualityParameters_RowEditing" OnRowUpdating="gvQualityParameters_RowUpdating" OnRowCancelingEdit="gvQualityParameters_RowCancelingEdit" OnRowCommand="gvQualityParameters_RowCommand">
                                        <Columns>




                                            <asp:TemplateField ItemStyle-Width="250" HeaderText="Check Point" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="ltCheckPointDescription" CssClass="pretextwrap" Text='<%#  DataBinder.Eval(Container.DataItem, "CheckPointDescription").ToString() %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:RequiredFieldValidator ID="rfvtxtCheckPointDescription" ValidationGroup="UpdateQualityParameters" runat="server" ControlToValidate="txtCheckPointDescription" Display="Dynamic" ErrorMessage=" * " />
                                                    <asp:TextBox runat="server" ID="txtCheckPointDescription" ClientIDMode="Static" CssClass="txtCheckPointDescription" onfocus="CheckPoinOnFocus()" TextMode="MultiLine" Rows="3" Width="250" Text='<%# DataBinder.Eval(Container.DataItem, "CheckPointDescription") %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>



                                            <asp:TemplateField ItemStyle-Width="10" HeaderText="Display Order" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal Visible="false" runat="server" ID="lthidRoutingDetails_QualityParameterID" Text='<%# DataBinder.Eval(Container.DataItem, "RoutingDetailsActivity_QualityParameterID") %>' />
                                                    <asp:Literal runat="server" ID="ltDisplayOrder" Text='<%# DataBinder.Eval(Container.DataItem, "DisplayOrder") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:RequiredFieldValidator ID="rfvtxtDisplayOrder" ValidationGroup="UpdateQualityParameters" runat="server" ControlToValidate="txtDisplayOrder" Display="Dynamic" ErrorMessage=" * " />
                                                    <asp:Literal Visible="false" runat="server" ID="lthidRoutingDetails_QualityParameterID" Text='<%# DataBinder.Eval(Container.DataItem, "RoutingDetailsActivity_QualityParameterID") %>' />

                                                    <asp:HiddenField ClientIDMode="Static" ID="hifRoutingDetailsActivityQCID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "RoutingDetailsActivity_QualityParameterID") %>' />

                                                    <asp:TextBox ID="txtDisplayOrder" runat="server" Width="40" Text='<%# DataBinder.Eval(Container.DataItem, "DisplayOrder") %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-Width="40" HeaderText="V" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="chkVisualType" runat="server" Text='<%#GetIsRequiredimage( DataBinder.Eval(Container.DataItem, "VisualType").ToString()) %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:CheckBox ID="chkVisualType" runat="server" onClick="HideDefaultColumns()" Checked='<%# Convert.ToBoolean( Convert.ToInt32(DataBinder.Eval(Container.DataItem, "VisualType"))) %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-Width="40" HeaderText="M" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="chkMeasureType" runat="server" Text='<%#GetIsRequiredimage( DataBinder.Eval(Container.DataItem, "MeasureType").ToString()) %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:CheckBox ID="chkMeasureType" ClientIDMode="Static" runat="server" onClick="HideDefaultColumns()" Checked='<%# Convert.ToBoolean( Convert.ToInt32(DataBinder.Eval(Container.DataItem, "MeasureType"))) %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-Width="40" HeaderText="R" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal ID="chkRecordedType1" runat="server" Text='<%#GetIsRequiredimage( DataBinder.Eval(Container.DataItem, "RecordedType").ToString()) %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:CheckBox ID="chkRecordedType" runat="server" onClick="HideColumns(this)" Checked='<%# Convert.ToBoolean( Convert.ToInt32(DataBinder.Eval(Container.DataItem, "RecordedType"))) %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-Width="100" HeaderText="Parameter Type" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>

                                                    <asp:Literal runat="server" ID="ltParameterDataType" Text='<%# DataBinder.Eval(Container.DataItem, "ParameterDataType") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>

                                                    <asp:TextBox ID="txtParameterDataType" ClientIDMode="Static" SkinID="txt_Auto_Req" runat="server" Width="110" Text='<%# DataBinder.Eval(Container.DataItem, "ParameterDataType") %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-Width="80" HeaderText="Min. Value" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltMinValue" Text='<%# DataBinder.Eval(Container.DataItem, "MinValue") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox runat="server" ClientIDMode="Static" ID="txtMinValue" Width="80" onKeyPress="return checkCustomDec(this,event)" Text='<%# DataBinder.Eval(Container.DataItem, "MinValue") %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-Width="80" HeaderText="Max. Value" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltMaxValue" Text='<%# DataBinder.Eval(Container.DataItem, "MaxValue") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox runat="server" ClientIDMode="Static" ID="txtMaxValue" Width="80" onKeyPress="return checkCustomDec(this,event)" Text='<%# DataBinder.Eval(Container.DataItem, "MaxValue") %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField ItemStyle-Width="150" HeaderText="Ref. Doc." HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltJAPReferenceText" Text='<%# DataBinder.Eval(Container.DataItem, "JAPReferenceText") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtJAPReferenceText" TextMode="MultiLine" Rows="3" Width="150" Text='<%# DataBinder.Eval(Container.DataItem, "JAPReferenceText") %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-Width="100" HeaderText="Delete" ItemStyle-CssClass="txtCenteralign" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkChildIsDelete" runat="server" />
                                                </ItemTemplate>
                                                <EditItemTemplate></EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:LinkButton Font-Underline="false" ID="lnkQCDelete" runat="server" Text="Delete" OnClick="lnkQCDelete_Click" OnClientClick="return confirm('Are you sure you want to delete the selected items?')" />
                                                    <img border="0" src="../Images/redarrowright.gif" alt="delete" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField Visible="false" DataField="EditName" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                            <asp:CommandField ValidationGroup="UpdateQualityParameters" CausesValidation="false" ItemStyle-HorizontalAlign="Center" ControlStyle-Font-Underline="false" ItemStyle-Width="55" ButtonType="Link" CancelImageUrl="icons/cancel.gif" CancelText="Cancel" EditImageUrl="icons/edit.gif" EditText=" Edit <img src='../Images/redarrowright.gif' border='0' />" ShowEditButton="True" UpdateImageUrl="icons/update.gif" UpdateText="Update" />
                                        </Columns>

                                        <PagerSettings FirstPageText="&amp;lt;&amp;lt;First Page" LastPageText="Last Page&amp;gt;&amp;gt;" Mode="NumericFirstLast" PageButtonCount="15" />

                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                        </tr>
                  
                                
                    </table>
                    </div>
                    <div class="ui-dailog-footer">
                            <div style="padding: 15px 13px 15px 5px;">
                                <asp:LinkButton ID="lnkQtCancel" OnClick="lnkQtCancel_Click" CssClass="ui-btn ui-button-large"  CausesValidation="false" runat="server" >Close<%=MRLWMSC21Common.CommonLogic.btnfaClear%></asp:LinkButton>
                            </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <!-- End Quality Parameters -->








    <!-- Start TER CheckPoints -->

    <div id="divEditTERDisputeDlgContainer">

        <div id="divEditTERDispute" style="display: none;">

            <asp:UpdatePanel ID="upnlTERCheckpoints" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">

                <ContentTemplate>

                    <div class="dailoggap ui-dailog-body" style="height:475px;">
                    <table cellpadding="3" cellspacing="3" border="0" width="100%" >
                        <tr>
                            <td align="right">
                                <asp:LinkButton ID="lnkAddTERCheckPoint" runat="server" CssClass="ui-button-small" OnClick="lnkAddTERCheckPoint_Click" >Add Check Point<%=MRLWMSC21Common.CommonLogic.btnfaNew %></asp:LinkButton>

                            </td>
                        </tr>

                        <tr>
                            <td colspan="2">
                                <asp:Label runat="server" ID="lblTERStatus" />

                                <asp:Panel ID="pnlTERCheckPoints" runat="server" Height="400" ScrollBars="None">
                                    <asp:GridView Width="500" ShowFooter="true" GridLines="Both" ID="gvTERCheckPoints" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="true" PageSize="50" AllowSorting="True" SkinID="gvLightSteelBlueNew" HorizontalAlign="Left" OnPageIndexChanging="gvTERCheckPoints_PageIndexChanging" OnRowDataBound="gvTERCheckPoints_RowDataBound" OnRowEditing="gvTERCheckPoints_RowEditing" OnRowUpdating="gvTERCheckPoints_RowUpdating" OnRowCancelingEdit="gvTERCheckPoints_RowCancelingEdit" OnRowCommand="gvTERCheckPoints_RowCommand">
                                        <Columns>

                                            <asp:TemplateField HeaderText="Wire ID" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltWireID" Text='<%# DataBinder.Eval(Container.DataItem, "WireID") %>' />
                                                    <asp:Literal runat="server" Visible="false" ID="ltRoutingDetailsActivity_TERID" Text='<%# DataBinder.Eval(Container.DataItem, "RoutingDetailsActivity_TERID") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:RequiredFieldValidator ID="rfvtxtWireID" ValidationGroup="UpdateTERCheckPoints" runat="server" ControlToValidate="txtWireID" Display="Dynamic" ErrorMessage=" * " />
                                                    <asp:TextBox runat="server" ID="txtWireID" ClientIDMode="Static" Text='<%# DataBinder.Eval(Container.DataItem, "WireID") %>' />


                                                    <asp:Literal runat="server" Visible="false" ID="ltRoutingDetailsActivity_TERID" Text='<%# DataBinder.Eval(Container.DataItem, "RoutingDetailsActivity_TERID") %>' />

                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Core ID" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltCoreID" Text='<%# DataBinder.Eval(Container.DataItem, "CoreID") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:RequiredFieldValidator ID="rfvtxtCoreID" ValidationGroup="UpdateTERCheckPoints" runat="server" ControlToValidate="txtCoreID" Display="Dynamic" ErrorMessage=" * " />
                                                    <asp:TextBox runat="server" ID="txtCoreID" ClientIDMode="Static" Text='<%# DataBinder.Eval(Container.DataItem, "CoreID") %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="Circle No." HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltCircleNo" Text='<%# DataBinder.Eval(Container.DataItem, "CircleNo") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:RequiredFieldValidator ID="rfvtxtCircleNo" ValidationGroup="UpdateTERCheckPoints" runat="server" ControlToValidate="txtCircleNo" Display="Dynamic" ErrorMessage=" * " />
                                                    <asp:TextBox runat="server" ID="txtCircleNo" ClientIDMode="Static" Text='<%# DataBinder.Eval(Container.DataItem, "CircleNo") %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="Designator" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltDesignator1" Text='<%# DataBinder.Eval(Container.DataItem, "Designator1") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:RequiredFieldValidator ID="rfvtxtDesignator1" ValidationGroup="UpdateTERCheckPoints" runat="server" ControlToValidate="txtDesignator1" Display="Dynamic" ErrorMessage=" * " />
                                                    <asp:TextBox runat="server" ID="txtDesignator1" ClientIDMode="Static" Text='<%# DataBinder.Eval(Container.DataItem, "Designator1") %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="Pin No." HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltPinNo1" Text='<%# DataBinder.Eval(Container.DataItem, "PinNo1") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:RequiredFieldValidator ID="rfvPinNo1" ValidationGroup="UpdateTERCheckPoints" runat="server" ControlToValidate="txtPinNo1" Display="Dynamic" ErrorMessage=" * " />
                                                    <asp:TextBox runat="server" ID="txtPinNo1" ClientIDMode="Static" Text='<%# DataBinder.Eval(Container.DataItem, "PinNo1") %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="Designator" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltDesignator2" Text='<%# DataBinder.Eval(Container.DataItem, "Designator2") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:RequiredFieldValidator ID="rfvtxtDesignator2" ValidationGroup="UpdateTERCheckPoints" runat="server" ControlToValidate="txtDesignator2" Display="Dynamic" ErrorMessage=" * " />
                                                    <asp:TextBox runat="server" ID="txtDesignator2" ClientIDMode="Static" Text='<%# DataBinder.Eval(Container.DataItem, "Designator2") %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="Pin No." HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltPinNo2" Text='<%# DataBinder.Eval(Container.DataItem, "PinNo2") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:RequiredFieldValidator ID="rfvtxtPinNo2" ValidationGroup="UpdateTERCheckPoints" runat="server" ControlToValidate="txtPinNo2" Display="Dynamic" ErrorMessage=" * " />
                                                    <asp:TextBox runat="server" ID="txtPinNo2" ClientIDMode="Static" Text='<%# DataBinder.Eval(Container.DataItem, "PinNo2") %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-Width="100" HeaderText="Parameter Type" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>

                                                    <asp:Literal runat="server" ID="ltParameterDataType" Text='<%# DataBinder.Eval(Container.DataItem, "ParameterDataType") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>

                                                    <asp:TextBox ID="txtParameterDataType" ClientIDMode="Static" SkinID="txt_Auto_Req" runat="server" Width="110" Text='<%# DataBinder.Eval(Container.DataItem, "ParameterDataType") %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-Width="80" HeaderText="Min. Value" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltMinValue" Text='<%# DataBinder.Eval(Container.DataItem, "MinValue") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox runat="server" ClientIDMode="Static" ID="txtMinValue" Width="80" onKeyPress="return checkDec(this,event)" Text='<%# DataBinder.Eval(Container.DataItem, "MinValue") %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-Width="80" HeaderText="Max. Value" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltMaxValue" Text='<%# DataBinder.Eval(Container.DataItem, "MaxValue") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox runat="server" ClientIDMode="Static" ID="txtMaxValue" Width="80" onKeyPress="return checkDec(this,event)" Text='<%# DataBinder.Eval(Container.DataItem, "MaxValue") %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField ItemStyle-Width="100" HeaderText="Delete" ItemStyle-CssClass="txtCenteralign" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkTERChildIsDelete" runat="server" />
                                                </ItemTemplate>
                                                <EditItemTemplate></EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:LinkButton Font-Underline="false" ID="lnkTERDelete" runat="server" Text="Delete" OnClick="lnkTERDelete_Click" SkinID="lnkButEmpty" OnClientClick="return confirm('Are you sure you want to delete the selected items?')" />

                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <asp:CommandField ValidationGroup="UpdateTERCheckPoints" CausesValidation="false" ItemStyle-HorizontalAlign="Center" ControlStyle-Font-Underline="false" ItemStyle-Width="165" ButtonType="Link" CancelImageUrl="icons/cancel.gif" CancelText="Cancel" EditImageUrl="icons/edit.gif" EditText="Edit <img src='../Images/redarrowright.gif' border='0' />" ShowEditButton="True" UpdateImageUrl="icons/update.gif" UpdateText="Update" />
                                        </Columns>

                                        <PagerSettings FirstPageText="&amp;lt;&amp;lt;First Page" LastPageText="Last Page&amp;gt;&amp;gt;" Mode="NumericFirstLast" PageButtonCount="15" />

                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                        </tr>
                                
                    </table>
                    </div>
                    <div class="ui-dailog-footer">
                            <div style="padding: 15px 13px 15px 5px;">
                                <asp:LinkButton ID="lnkTERCancel" OnClick="lnkTERCancel_Click" CssClass="ui-btn ui-button-large" CausesValidation="false" runat="server" >Close<%=MRLWMSC21Common.CommonLogic.btnfaClear %></asp:LinkButton>
                            </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <!-- End TER CheckPoints -->












    <!-- Start Activity Material -->

    <div id="divActivityLevelComponents">
        <div id="divComponentsDispute" style="display: none;">

            <asp:UpdatePanel ID="upnlComponents" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
                <ContentTemplate>

                    <div class="dailoggap ui-dailog-body " style="height:475px">

                    <table cellpadding="3" cellspacing="3" border="0" width="100%">
                        <tr>
                            <td class="SubHeading2"></td>
                            <td align="right">
                                <asp:LinkButton ID="lnkAddComponents" runat="server" CssClass="ui-button-small" OnClick="lnkAddComponents_Click" >Add Components<%=MRLWMSC21Common.CommonLogic.btnfaNew %></asp:LinkButton>

                            </td>
                        </tr>

                        <tr>
                            <td colspan="2">
                                <asp:Label runat="server" ID="McodeError" />

                                <asp:Panel ID="Panel1" runat="server" >
                                    <asp:GridView Width="100%" ShowFooter="true" GridLines="Both" ID="gvActivityMcode" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="true" PageSize="50" AllowSorting="True" SkinID="gvLightSteelBlueNew" HorizontalAlign="Left" OnRowDataBound="gvActivityMcode_RowDataBound" OnRowEditing="gvActivityMcode_RowEditing" OnRowUpdating="gvActivityMcode_RowUpdating" OnRowCancelingEdit="gvActivityMcode_RowCancelingEdit">
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="150" HeaderText="Part Number" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltMcode" Text='<%# DataBinder.Eval(Container.DataItem, "MCode") %>' />
                                                    <asp:Literal Visible="false" runat="server" ID="lthid_RoutingDetailsActivity_MaterilaMasterID" Text='<%# DataBinder.Eval(Container.DataItem, "RoutingDetailsActivity_MaterilaMasterID") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:RequiredFieldValidator ID="rfvTxtMaterilCode" ValidationGroup="UpdateActivityMcode" runat="server" ControlToValidate="txtMcode" Display="Dynamic" ErrorMessage=" * " />
                                                    <asp:Literal Visible="false" runat="server" ID="lthid_RoutingDetailsActivity_MaterilaMasterID" Text='<%# DataBinder.Eval(Container.DataItem, "RoutingDetailsActivity_MaterilaMasterID") %>' />
                                                    <asp:Literal Visible="false" runat="server" ID="lthid_RoutingDetailsActivityID" Text='<%# DataBinder.Eval(Container.DataItem, "RoutingDetailsActivityID") %>' />
                                                    <asp:TextBox ID="txtMcode" EnableTheming="false" CssClass="MCodePicker" runat="server" Width="200" Text='<%# DataBinder.Eval(Container.DataItem, "MCode") %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField ItemStyle-Width="150" HeaderText="OEM Part #" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltOEMPartNo" Text='<%# DataBinder.Eval(Container.DataItem, "OEMPartNo") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-Width="100" HeaderText="UoM / Qty." HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltMMUoM" Text='<%# DataBinder.Eval(Container.DataItem, "UoM") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField ItemStyle-Width="80" HeaderText="Quantity" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltQty" Text='<%# DataBinder.Eval(Container.DataItem, "Quantity") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:RequiredFieldValidator ID="rfvTxtQty" ValidationGroup="UpdateActivityMcode" runat="server" ControlToValidate="txtQty" Display="Dynamic" ErrorMessage=" * " />
                                                    <asp:TextBox ID="txtQty" EnableTheming="false" runat="server" onKeyPress="return checkDec(this,event)" Width="160" Text='<%# DataBinder.Eval(Container.DataItem, "Quantity") %>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>



                                            <asp:TemplateField ItemStyle-Width="45" HeaderText="Delete" ItemStyle-CssClass="txtCenteralign" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkChildIsDelete" runat="server" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:LinkButton Font-Underline="false" ID="lnkMcodeDelete" runat="server" Text="Delete" OnClick="lnkMcodeDelete_Click" OnClientClick="return confirm('Are you sure you want to delete the selected items?')" />
                                                    <img border="0" src="../Images/redarrowright.gif" alt="delete" />
                                                </FooterTemplate>
                                            </asp:TemplateField>

                                            <asp:CommandField ItemStyle-HorizontalAlign="Center" CausesValidation="false" ControlStyle-Font-Underline="false" ItemStyle-Width="40" ButtonType="Link" CancelImageUrl="icons/cancel.gif" CancelText="Cancel" EditImageUrl="icons/edit.gif" EditText="Edit <img src='../Images/redarrowright.gif' border='0' />" ShowEditButton="true" UpdateImageUrl="icons/update.gif" UpdateText="Update" />
                                        </Columns>

                                        <PagerSettings FirstPageText="&amp;lt;&amp;lt;First Page" LastPageText="Last Page&amp;gt;&amp;gt;" Mode="NumericFirstLast" PageButtonCount="15" />


                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                        </tr>
                        
                                
                              
                    </table>
                        </div>
                     <div class="ui-dailog-footer">
                            <div style="padding: 15px 13px 15px 5px;"> 
                                <asp:LinkButton ID="lnkMcodeClose" OnClick="lnkMcodeClose_Click" CssClass="ui-btn ui-button-large" Text="" runat="server" >Close<%=MRLWMSC21Common.CommonLogic.btnfaClear %></asp:LinkButton>
                                </div>
                                   </div>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <!-- END  Activity Material -->



    <!-- Routing Revision History  -->


    <div id="divEditRevisionDisputeDlgContainer">

        <div id="divEditRevisionDispute" style="display: none;">

            <asp:UpdatePanel ID="upnlRevision" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
                <ContentTemplate>
 <div class="dailoggap">
                    <table border="0" cellpadding="3" cellpadding="3">
                        <tr>
                            <td colspan="2" class="FormLabels">
                                <asp:Label runat="server" ID="lblRevisionStatus"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabels">
                                <asp:RequiredFieldValidator ID="rfvtxtNewRevision" runat="server" ValidationGroup="routingrvsion" ControlToValidate="txtNewRevision" Display="Dynamic" ErrorMessage=" * " />
                                Revision: 
                            </td>
                            <td>
                                <asp:TextBox runat="server" Width="150" ID="txtNewRevision"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td class="FormLabels">
                                <asp:RequiredFieldValidator ID="rfvtxtEffectedDate" runat="server" ValidationGroup="routingrvsion" ControlToValidate="txtEffectedDate" Display="Dynamic" ErrorMessage=" * " />
                                Effective Date: 
                            </td>
                            <td>
                                <asp:TextBox runat="server" Width="150" ID="txtEffectedDate"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td class="FormLabels">
                                <asp:RequiredFieldValidator ID="rfvtxtRevRemarks" runat="server" ValidationGroup="routingrvsion" ControlToValidate="txtRevRemarks" Display="Dynamic" ErrorMessage=" * " />
                                Remarks: 
                            </td>
                            <td>

                                <asp:TextBox ID="txtRevRemarks" onKeypress="return checkSpecialChar(event)" runat="server" Width="250" Height="110" TextMode="MultiLine" CssClass="txt_Blue_Small" />
                            </td>
                        </tr>

                        
                    </table>
</div>
                    <div class="ui-dailog-footer">
                            <div style="padding: 15px 13px 15px 5px;"> 
                                <asp:LinkButton runat="server" ID="lnkCreateNewRevision" ValidationGroup="routingrvsion"  OnClick="lnkCreateNewRevision_Click" CssClass="ui-btn ui-button-large">Create New Revision<%=MRLWMSC21Common.CommonLogic.btnfaSave%></asp:LinkButton>
                                </div>
                        </div>
                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
    </div>


    <!-- Routing Revision History  -->





    <asp:HiddenField ClientIDMode="Static" ID="hifNonConfirmityTypeID" runat="server" />

    <asp:HiddenField ID="hifRoutingDetailsctivityID" runat="server" Value="0" />

    

    <asp:HiddenField ID="hifMMRvID" runat="server" />

    <asp:HiddenField ID="hifBoMHeaderID" runat="server" />

    <asp:HiddenField ID="hifQCparameterID" runat="server" />

    <asp:HiddenField ID="hifRoutingDetailsID" runat="server" />

    <asp:HiddenField ID="hifParentRDActivityID" runat="server" />

    <asp:HiddenField ID="hifFinishedMRvID" runat="server" />
    <asp:HiddenField ID="hifBoMHeaderRevID" runat="server" />

    <asp:Label ID="lblRoutDetID" runat="server"></asp:Label>

    <asp:HiddenField ID="hifRoutingDocTypeID" runat="server" />

    <asp:HiddenField ID="hifApprovedBy" runat="server" />
    <asp:HiddenField ID="hifCheckedBy" runat="server" />
    <asp:HiddenField ID="hifPreparedBy" runat="server" />

    <asp:HiddenField ID="hifCheckListTypeID" runat="server" />
    <asp:HiddenField ID="hifTERCaptureTypeID" runat="server" />


    <asp:HiddenField ID="hifJRPApprovedByID" runat="server" />
    <asp:HiddenField ID="hifJRPReviewedByID" runat="server" />
    <asp:HiddenField ID="hifJRPPreparedByID" runat="server" />
    <asp:HiddenField ID="hifJRPQCApprovedByID" runat="server" />
    <asp:HiddenField ID="hifSourceActivityID" runat="server" />








    <!--
                                            <span class="showonhover">
                                                <div> <h1>test</h1></div>
                                                <span class="hovertext">
                                                    Available Quantity:  <br/>
			                                        Replenishment Quantity:  
                                                </span>
                                            </span> -->



    <!-- Routing BOM Deficiency List Dialog   Developed by Naresh 05/03/2014-->


    <div id="divProdDefcListContainer">
        <div id="divProdDefcList" style="display: block; padding: 35px;">
            <asp:UpdatePanel ID="upnlProdDefcList" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
                <ContentTemplate>

                    <div class="dailoggap">
                    <asp:Panel ID="pnlProdDefcList" runat="server" Width="600px" Height="500px" HorizontalAlign="Center" ScrollBars="Auto" >

                        <asp:GridView Width="100%" ShowFooter="true" ID="gvRoutingBOMDefcList" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="true" PageSize="25" SkinID="gvLightSteelBlueNew" HorizontalAlign="Left" OnPageIndexChanging="gvRoutingBOMDefcList_PageIndexChanging" OnRowDataBound="gvRoutingBOMDefcList_RowDataBound">
                            <Columns>

                                <asp:TemplateField ItemStyle-Width="150" HeaderText="Part #">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="ltMCode" Text='<%# DataBinder.Eval(Container.DataItem, "MCode") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="150" HeaderText="BOM Qty.">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="ltBOMQty" Text='<%# DataBinder.Eval(Container.DataItem, "BOMQty") %>' />
                                    </ItemTemplate>

                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="150" HeaderText="Activity Mapped Qty.">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="ltOperActivityQty" Text='<%# DataBinder.Eval(Container.DataItem, "OperActivityQty") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField ItemStyle-Width="150" HeaderText="Qty. to Map">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="ltDeficienct" Text='<%# DataBinder.Eval(Container.DataItem, "Deficiency") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>



                            </Columns>
                        </asp:GridView>

                        <br />

                    </asp:Panel>
                        </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <asp:HiddenField runat="server" ID="hifRoutingHeaderID" />



  
  
    <!-- Routing BOM Deficiency List Dialog   Developed by Naresh 05/03/2014   -->

    
</asp:Content>

