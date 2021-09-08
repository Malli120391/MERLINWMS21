<%@ Page Title=" .: Item Master Request :. " Language="C#" MasterPageFile="~/mMaterialManagement/MaterialManagementMaster.master" AutoEventWireup="true" CodeBehind="MaterialMasterRequest.aspx.cs" Inherits="MRLWMSC21.mMaterialManagement.MaterialMasterRequest" MaintainScrollPositionOnPostback="true" EnableEventValidation="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MMContent" runat="server">

    <asp:ScriptManager ID="mySManager" runat="server" EnablePartialRendering="true" SupportsPartialRendering="true" />
    <script src="Scripts/jquery-ui-1.8.24.min.js"></script>
    <script type="text/javascript" src="Scripts/CommonScripts.js"></script>


    <style>
/* The container */
.container {
    display: block;
    position: relative;
    padding-left: 35px;
    margin-bottom: 12px;
    cursor: pointer;
    font-size: medium;
    -webkit-user-select: none;
    -moz-user-select: none;
    -ms-user-select: none;
    user-select: none;
   
}

/* Hide the browser's default checkbox */
.container input {
    position: absolute;
    opacity: 0;
     border-radius:4px;
}

/* Create a custom checkbox */
.checkmark {
    position: absolute;
    top: 0;
    left: 0;
    height: 20px;
    width: 20px;
    background-color: #FFF;
    border:1px solid #f7953c;

}

/* On mouse-over, add a grey background color */
.container:hover input ~ .checkmark {
    background-color: #f2f2f2;
}

/* When the checkbox is checked, add a blue background */
.container input:checked ~ .checkmark {
    background-color: #f7953c;
     border-radius:4px;
}

/* Create the checkmark/indicator (hidden when not checked) */
.checkmark:after {
    content: "";
    position: absolute;
    display: none;
}

/* Show the checkmark when checked */
.container input:checked ~ .checkmark:after {
    display: block;
}

/* Style the checkmark/indicator */
.container .checkmark:after {
    left: 7px;
    top: 3px;
    width: 4px;
    height: 8px;
    border: solid white;
    border-width: 0 3px 3px 0;
    -webkit-transform: rotate(45deg);
    -ms-transform: rotate(45deg);
    transform: rotate(45deg);
}

.ajax-upload-dragdrop {
border: 1px outset #fac18a !important;
    border-radius: 5px;
    color: #dadce3;
    text-align: left;
    vertical-align: middle;
    padding: 10px 10px 0 10px;
    width: 300px;
    padding: 1px !important;
    height: 31px;
    font-size: 12px;
}
#MainContent_MMContent_fuItemPicture {
        width: 210px !important;
}
.txt_small_Req ui-autocomplete-input {
    width: calc(200px - 15px );
}
        .FormLabels {    padding-bottom: 10px;
        }
textarea {
    width: 210px !important;
}
select {
    width:215px !important;
}

        .material-icons {
                vertical-align: middle;
    margin-right: 5px;    font-size: 18px;
        }
</style>

    
    <script type="text/javascript">
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
    </script>



    <script type="text/javascript">

        function ShowConversionValue(ToDropDown) {
            document.getElementById('lbConversion').innerText = "Result : " + ToDropDown.value;
        }

        function changeToMeasurementindex() {
            //var index = dropdown.selectedIndex;
            //alert(index);
            document.getElementById('<%=this.ddlToMeasurement.ClientID%>').selectedIndex = 0;
            document.getElementById('lbConversion').innerText = "";
            //dropdown.selectedIndex = 0;
            //dropdown.selectedIndex = index;

        }

    </script>

    <script>
        
        $(document).ready(function () {
            CustomAccordino($('#dvBMDHeader'), $('#dvBMDBody'));
            CustomAccordino($('#divIndustryHeader'), $('#divIndustryBody')); //Added by kashyap on 29-01-2018
            CustomAccordino($('#divSDHeader'), $('#divSDBody'));
            CustomAccordino($('#divUOMHeader'), $('#divUOMBody'));
            CustomAccordino($('#divQCPDHeader'), $('#divQCPDBody'));
            CustomAccordino($('#divIDimensionHeader'), $('#divIDimensionBody'));
            //CustomAccordino($('#divSPviewHeader'), $('#divSPviewBody'));
            CustomAccordino($('#divBRPHeader'), $('#divBRPBody')); //Added by kashyap on 29-01-2018
            CustomAccordino($('#divMRPHeader'), $('#divMRPBody'));
            CustomAccordino($('#divGPDHeader'), $('#divGPDBody'));
            CustomAccordino($('#divAPviewHeader'), $('#divAPviewBody'));
            CustomAccordino($('#divIPictureHeader'), $('#divIPictureBody'));
            CustomAccordino($('#divSAHeader'), $('#divSABody'));
            CustomAccordino($('#divTDHeader'), $('#divTDBody'));
            CustomAccordino($('#divMspHeader'), $('#divMSpBody'));

            
            

        });

    </script>

    <style type="text/css">
        .btnSearch {
            padding-top: 1.3px;
            padding-bottom: 7.5px;
        }

        .startupload {
            background-color: #E67E22;
            -moz-border-radius: 4px;
            -webkit-border-radius: 4px;
            border-radius: 4px;
            margin: 0;
            padding: 0;
            display: inline-block;
            color: #fff;
            font-family: arial;
            font-size: 13px;
            font-weight: normal;
            padding: 4px 15px;
            text-decoration: none;
            cursor: pointer;
            text-shadow: 0 1px 0 #5b8a3c;
            vertical-align: top;
            margin-right: 5px;
            box-shadow: inset 0 39px 0 -24px #E67E22;
        }

        .SupplierPicker {
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

            .SupplierPicker:focus {
                outline: none;
                color: #000000;
                box-shadow: 0px 0px 5px #E67E22;
                border: 1px solid #E67E22;
                border-radius: 5px;
                background-image: url('../Images/Down_OrangeMD-E67E22.png');
                background-repeat: no-repeat;
                background-position: right;
            }

        .ui-autocomplete-loading {
            background: white url('../Images/ui-anim_basic_16x16.gif') right center no-repeat;
        }
        #MainContent_MMContent_lnkUpdateTenant {
    margin-right: -18px;
        }
        .ui-autocomplete {
            position: absolute;
            height: 200px;
            overflow-y: scroll;
        }

        .ui-state-hover {
            cursor: pointer;
        }

        .ui-btn {
            float: right;
            margin-left:5px;
        }
        .vl {font-size: 18px;
        }
        .internalData {
        }
        .ui-autocomplete-input {
                width: calc(200px - 15px ) !important;
        }
    </style>

    <script language="javascript" type="text/JavaScript">
        function MyJavascriptFunction() {
            alert("testing");
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

//Check/uncheck MSP's


        function check_uncheckMSP(Val) {
            
            var ValChecked = Val.checked;
            var ValId = Val.id;
            var frm = document.forms[0];
            // Loop through all elements
            for (i = 0; i < frm.length; i++) {
                // Look for Header Template's Checkbox
                //As we have not other control other than checkbox we just check following statement

                if (this != null) {

                    if (ValId.indexOf('chkIsRequiredItemsAll') != -1) {
                        // Check if main checkbox is checked,
                        // then select or deselect datagrid checkboxes

                        if (frm.elements[i].type == 'checkbox' && frm.elements[i].name.indexOf('chkMSPIsRequired') != -1) {
                            if (ValChecked)
                                frm.elements[i].checked = true;
                            else
                                frm.elements[i].checked = false;
                        }
                    }
                    else if (ValId.indexOf('chkMSPIsRequired') != -1) {
                        // Check if any of the checkboxes are not checked, and then uncheck top select all checkbox
                        if (frm.elements[i].checked == false)
                            frm.elements[1].checked = false;
                    }

                }

            } // for
        }


        //function ChangingEvent() {
        //    
        //    var Acount = 0;
        //    var Ccount = 0;
        //    $('.cbdeleteinner').each(function () {
        //        console.log($(this).children().attr('checked'));
        //        Acount = eval(Acount) + 1;
        //        if ($(this).children().attr('checked') == 'checked') {
        //            Ccount = eval(Ccount) + 1;
        //        }
        //    });

        //    if (Acount == Ccount) {
        //        $('.cbdeleteall').children().attr('checked', 'checked');
        //    }

        //    else {

        //        $('.cbdeleteall').children().removeAttr('checked');
        //    }

        //}


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

    </script>

   



    <script type="text/javascript">
        var select = function (dropdown, selectedValue) {
            var options = $(dropdown).find("option");
            var matches = $.grep(options,
                function (n) { return $(n).text() == selectedValue; });
            $(matches).attr("selected", "selected");
        };


        $(function () {
            function log(message) {
                $(" ").text(message).prependTo("#log");
                $("#log").attr("scrollTop", 0);
                alert(message);
                //fetchPartyInformation(message);
            }
        }
        )
    </script>

    <script type="text/javascript">

        $(document).ready(function () {

            $('#<%= this.txtMfgPartNo.ClientID %>').blur(function () {


                if (document.getElementById("<%=txtMfgPartNo.ClientID %>").value == "") {
                    document.getElementById("<%=txtMfgPartNo.ClientID %>").focus();
                    return;
                } else {
                    if (checkAlphaNumericWithDash(document.getElementById("<%=txtMfgPartNo.ClientID %>").value)) {


                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/CheckSupplierPartNo") %>',
                            data: "{  '_CatlogNumber': '" + document.getElementById("<%=this.txtMfgPartNo.ClientID%>").value + "', 'TenantID': '" + '<%= ViewState["TenantID"] %>' + "' }",
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {



                                if ('<%= ViewState["MaterialMasterID"] %>' == "0") {

                                    if (checkCatlogNoLength(document.getElementById("<%=this.txtMfgPartNo.ClientID%>").value) == false) {
                                        alert('Minimum length of part no. is 3');
                                        document.getElementById("<%=this.txtMfgPartNo.ClientID%>").focus();
                                        return;
                                    }
                                    if (CheckMCode(data.d) == "false") {

                                        document.getElementById("<%=this.txtMfgPartNo.ClientID%>").focus();
                                        return;

                                    }
                                }
                            },
                            error: function (response) {
                                alert(response.text);
                            },
                            failure: function (response) {
                                alert(response.text);
                            }
                        });
                    }
                }
            });
        });


        function CheckMCode(result) {
            if (result == "false") {
                document.getElementById("ltSupplierPartPicHolder").innerHTML = "<img border='0'  src='../Images/blue_menu_icons/check_mark.png' />";
                return "true";
            }
            else {
                document.getElementById("ltSupplierPartPicHolder").innerHTML = "<img border='0'  src='../Images/cancel.png' />";

                return "false";
            }

        }

        function checkCatlogNoLength(length1) {
            if (length1.length < 3 || length1.length > 17)
                return false;
        }

    </script>

    <!-- Scripts for autocompletes  -->

    <script type="text/javascript">

        function checkAlphaNumericWithDash(fieldname) {

            return true;

        }


        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                fnLoadBasicMMAutocompletes();
            }
        }

        function fnLoadBasicMMAutocompletes() {


            $(document).ready(function () {



                $("#<%= this.txtSearchMCode.ClientID %>").on("focus", function (e) {
                $(this).select();
            });

            $("#<%= this.atcStorageConditionID.ClientID %>").blur(function () {

                document.getElementById("<%=txtRefContainer.ClientID %>").value = document.getElementById("<%=atcStorageConditionID.ClientID %>").value;
            });

            try {

                var textfieldname = $("#<%= this.txtSearchMCode.ClientID %>");
                DropdownFunction(textfieldname);
                $("#<%= this.txtSearchMCode.ClientID %>").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadMCodeOEMDataFor3PL") %>',
                            data: "{ 'prefix': '" + request.term + "'}",
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                response($.map(data.d, function (item) {
                                    return {
                                        label: item.split('`')[0],
                                        description: item.split('`')[1] == undefined ? "" : " <font color='#086A87'>" + item.split('`')[1] + "</font>"
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
                }).data("autocomplete")._renderItem = function (ul, item) {
                    // Inside of _renderItem you can use any property that exists on each item that we built
                    // with $.map above */
                    return $("<li></li>")
                        .data("item.autocomplete", item)
                        .append("<a>" + item.label + "" + item.description + "</a>")
                        .appendTo(ul)
                };
            } catch (ex) {
            }

            var textfieldname = $("#<%= this.atcPlantID.ClientID %>");
            DropdownFunction(textfieldname);
            $("#<%= this.atcPlantID.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadPlantData") %>',
                        data: "{ 'prefix': '" + request.term + "', 'TenantID': '" + '<%= ViewState["TenantID"] %>' + "' }",
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
                    $("#<%=hifPlantID.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });


            var textfieldname = $("#<%= this.atcMTypeID.ClientID %>");
            DropdownFunction(textfieldname);
            $("#<%= this.atcMTypeID.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadMTypeData") %>',
                        data: "{ 'prefix': '" + request.term + "', 'TenantID': '" + '<%= ViewState["TenantID"] %>' + "' }",
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
                minLength: 0
            });

            var textfieldname = $("#<%= this.atcStorageConditionID.ClientID %>");
            DropdownFunction(textfieldname);
            $("#<%= this.atcStorageConditionID.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadStorageConditionData") %>',
                        data: "{ 'prefix': '" + request.term + "', 'TenantID': '" + '<%= ViewState["TenantID"] %>' + "' }",
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
                    $("#<%=hifStorageConditionID.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });

            var textfieldname = $("#<%= this.atcMaterialGroup.ClientID %>");
            DropdownFunction(textfieldname);
            $("#<%= this.atcMaterialGroup.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadMaterialGroupData") %>',
                        data: "{ 'prefix': '" + request.term + "', 'TenantID': '" + '<%= ViewState["TenantID"] %>' + "' }",
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
                    $("#<%=hifMaterialGroup.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });

            var textfieldname = $("#<%= this.atcProductCategories.ClientID %>");
            DropdownFunction(textfieldname);
            $("#<%= this.atcProductCategories.ClientID %>").autocomplete({
                source: function (request, response) {




                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadProductCategoriesData") %>',
                        data: "{ 'prefix': '" + request.term + "','MTypeID' : '" + document.getElementById("<%=hifMtypeIDp.ClientID %>").value + "', 'TenantID': '" + '<%= ViewState["TenantID"] %>' + "' }",
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
                    $("#<%=hifProductCategories.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });




            var textfieldname = $("#<%= this.atcSalesUoM.ClientID %>");
            DropdownFunction(textfieldname);
            $("#<%= this.atcSalesUoM.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadUoMData") %>',
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
                    $("#<%=hifSalesUoM.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });

            var textfieldname = $("#<%= this.txtlocation.ClientID %>");
            DropdownFunction(textfieldname);
            $("#<%= this.txtlocation.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadLoction_Auto") %>',
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
                    $("#<%=hiflocation.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });

            var textfieldname = $("#<%= this.atcPurchaseUom.ClientID %>");
            DropdownFunction(textfieldname);
            $("#<%= this.atcPurchaseUom.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadUoMData") %>',
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
                    $("#<%=hifPurchaseUom.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });


            var textfieldname = $("#<%= this.atcUoM.ClientID %>");
            DropdownFunction(textfieldname);
            $("#<%= this.atcUoM.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadUoMData") %>',
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
                    $("#<%=hifUoM.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });

            var textfieldname = $("#<%= this.txtPrUoM.ClientID %>");
            DropdownFunction(textfieldname);
            $("#<%= this. txtPrUoM.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadUoMData") %>',
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
                    $("#<%=hifPrUoMID.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });

            var textfieldname = $("#<%= this.txtTenant.ClientID %>");
            DropdownFunction(textfieldname);
            $("#<%= this.txtTenant.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadTenantDataFor3PL") %>',
                        data: "{ 'prefix': '" + request.term + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data)
                        {
                          
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
                select: function (e, i)
                {
                  
                   // $("#<%=hifTenant.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });

            var textfieldname = $("#<%= this.txtSpaceUtilization.ClientID %>");
            DropdownFunction(textfieldname);
            $("#<%= this.txtSpaceUtilization.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadSpaceutilization") %>',
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
                    $("#<%=hifSpaceUtilization.ClientID %>").val(i.item.val);
                    if ($("#<%=hifSpaceUtilization.ClientID %>").val() != '4') {

                        if (document.getElementById('<%=this.txtMLength.ClientID%>').value == "" || document.getElementById('<%=this.txtMHeight.ClientID%>').value == "" || document.getElementById('<%=this.txtMWidth.ClientID%>').value == "" || document.getElementById('<%=this.txtMWeight.ClientID%>').value == "") {

                            showStickyToast(true, 'Please enter \'Item Dimensions\' values, to add the \'Tenant\'')
                        }
                    }
                    else
                        $("#<%=hifSpaceUtilization.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });

            var TextFieldName = $('#atcInvCurrency');
            DropdownFunction(TextFieldName);
            $('#atcInvCurrency').autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadCurrencyData") %>',
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
                        }
                    });
                },
                select: function (e, i) {


                },
                minLength: 0
            });



            var TextFieldName = $('#txtMaterialShape');
            DropdownFunction(TextFieldName);
            $('#txtMaterialShape').autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadMaterialShape") %>',
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
                        }
                    });
                },
                select: function (e, i) {
                    $('#hifMaterialShapeID').val(i.item.val);
                },
                minLength: 0
            });

            });




        }

        fnLoadBasicMMAutocompletes();
    </script>

    <script type="text/javascript">


        var UomResult;

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                fnLoadQCPicker();
            }
        }

        function fnLoadQCPicker() {


            $(document).ready(function () {

                $(".DynaEffectiveDate").datepicker({ dateFormat: "dd/mm/yy" });


                var textfieldname = $('#txtQCParameter');
                DropdownFunction(textfieldname);
                $('#txtQCParameter').autocomplete({
                    source: function (request, response) {

                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadQualityParameters") %>',
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

                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetParameterType") %>',
                            data: "{ 'QCParameterID': '" + document.getElementById('<%= hifQCparameterID.ClientID %>').value + "'}",
                            dataType: "json",
                            type: "POST",
                            async: true,
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                if (data.d == "0") {
                                    document.getElementById('txtMinTolerance').style.display = "none";
                                    document.getElementById('txtMaxTolerance').style.display = "none";
                                } else {
                                    document.getElementById('txtMinTolerance').style.display = "inline";
                                    document.getElementById('txtMaxTolerance').style.display = "inline";
                                }
                            }

                        });



                    },
                    minLength: 0
                });



            });
        }

        fnLoadQCPicker();


    </script>

    <script type="text/javascript">


        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                fnLoadSupplierPicker();
            }
        }

        function fnLoadSupplierPicker() {


            $(document).ready(function () {
                var textfieldname = $('.SupplierPicker');
                DropdownFunction(textfieldname);
                $('.SupplierPicker').autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadMMSupplierDataFor3PL") %>',
                            data: "{ 'prefix': '" + request.term + "', 'TenantID': '" + document.getElementById('<%= this.hifTenant.ClientID %>').value + "' }",
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



                var TextFieldName = $('#atcInvCurrency');
                DropdownFunction(TextFieldName);
                $('#atcInvCurrency').autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadCurrencyData") %>',
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
                            }
                        });
                    },
                    select: function (e, i) {

                    },
                    minLength: 0
                });

                var TextFieldName = $('#txtCompanyName');
                DropdownFunction(TextFieldName);
                $("#txtCompanyName").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadTenantDataFor3PL") %>',
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
                            }
                        });
                    },
                    select: function (e, i) {
                        $("#hifTenantID").val(i.item.val);

                    },
                    minLength: 0
                });



                var TextFieldName = $('#txtSpaceUtilization');
                DropdownFunction(TextFieldName);
                $("#txtSpaceUtilization").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadSpaceutilization") %>',
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
                            }
                        });
                    },
                    select: function (e, i) {
                        $("#hifSpaceUtilizationID").val(i.item.val);
                        if ($("#hifSpaceUtilizationID").val() != '4') {

                            if (document.getElementById('<%=this.txtMLength.ClientID%>').value == "" || document.getElementById('<%=this.txtMHeight.ClientID%>').value == "" || document.getElementById('<%=this.txtMWidth.ClientID%>').value == "" || document.getElementById('<%=this.txtMWeight.ClientID%>').value == "") {

                                showStickyToast(true, 'Please enter \'Item Dimensions\' values, to add the \'Tenant\'')
                                //$("#hifSpaceUtilizationID").val('-1')='-1';
                            }
                        }
                        else
                            $("#hifSpaceUtilizationID").val(i.item.val);

                    },
                    minLength: 0
                });

            });
        }
        fnLoadSupplierPicker();
    </script>

    <!--Latest Uploadify Begin-->

    <script src="FileUploader/jquery.uploadfile.min.js" type="text/javascript"></script>
    <link href="FileUploader/uploadfile.min.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">



        $(document).ready(function () {

            
            //var fileextention;
            //if ($('#=ddldocumenttype.ClientID ').val() == 2)
            //            fileextention = "pdf";
            //        else
            //fileextention = "doc,docx,gif,jpg,png,xps,pdf";

            var uploadObj = $("#fileuploader").uploadFile({
                url: "UploadMultiFiles.ashx",
                multiple: true,
                autoSubmit: false,
                fileName: "FileNames",
                allowedTypes: "doc,docx,pdf,jpg,gif,png,xps,txt",
                dynamicFormData: function () {

                    var parm = document.getElementById('<%=ddldocumenttype.ClientID %>');
                    DocType = parm.options[parm.selectedIndex].value;
                    var parm1 = document.getElementById('<%=ddlsid.ClientID %>');
                    supplierID = parm1.options[parm1.selectedIndex].value;
                    var Tenantid = document.getElementById('<%=hifTenant.ClientID %>').value;
                    //alert(Tenantid);
                    var data = { Doctype: DocType, mid: '<%=ViewState["MaterialMasterID"].ToString()%>', sid: supplierID, Tid: Tenantid }
                    return data;
                },


               <%-- onSelect: function (files) {
                    var extension = files[0].name.split('.')[1];
                    var docType = document.getElementById('<%=ddldocumenttype.ClientID %>');
                    DocType = docType.options[docType.selectedIndex].value;
                    if (DocType == 1)
                    {

                    }
                    return true;
                },--%>

                showStatusAfterSuccess: true,
                abortStr: "Abort",
                cancelStr: "Cancel",
                doneStr: "Close",
                showDone: true,
                afterUploadAll: function () {
                    //alert('All attached files are successfully uploaded');
                    
                    showStickyToast(true, "All attached files are successfully uploaded");
                    location.reload();
                },
                onError: function (files, status, errMsg) {
                    //alert('Error while uploading');
                    showStickyToast(false, "Error while uploading");
                },
                onSubmit: function (files) {
                    alert();
                },
                /*onSuccess: function (files, data, xhr) {
                    alert('One File Uploaded');

                },*/
                width: "5px"

            });
            //$('#fileuploader').fileupload({
            //    dataType: 'json',
            //    sequentialUploads: true,
            //    formData: [{ 'Doctype': 'PDF' }, {'mid': 1024 },{ 'sid': 25 }, {'ddldocumentvalue': 2 }]
            //});

            $("#startUpload").click(function () {
                
                var parm = document.getElementById('<%=ddldocumenttype.ClientID %>');
                DocType = parm.options[parm.selectedIndex].value;

                var parm1 = document.getElementById('<%=ddlsid.ClientID %>');
                supplierID = parm1.options[parm1.selectedIndex].value;
                if (supplierID == 0) {
                    //alert("Please select supplier");
                    showStickyToast(false, "Please select supplier");
                    return false;
                }
                if (DocType == 0) {
                    //alert("Please select attachment type");
                    showStickyToast(false, "Please select attachment type");
                    return false;
                }

                uploadObj.startUpload();


                //var supplierID, DocType;

                //                var parm = document.getElementById('ddldocumenttype.ClientID ');



                //DocType = parm.options[parm.selectedIndex].value;

                //              alert(DocType);
                //            supplierID = parm1.options[parm1.selectedIndex].value;

                //                alert(supplierID);

                //$('#fileuploader').bind('fileuploadsubmit', function (e, data) {
                //    data.formData = [{ 'Doctype': 'PDF' }, { 'mid': 1024 }, { 'sid': 25 }, { 'ddldocumentvalue': 2 }];
                //});
                //$('#fileuploader').fileupload({
                //    formData: {
                //        'Doctype': DocType,
                //        'mid': 3,
                //        'sid': supplierID,
                //        'ddldocumentvalue': 2
                //    }
                //});

            });


        });
    </script>

    <!--Latest Uploadify End-->




    <!--Print Strart-->

    <script type="text/javascript">
        $(document).ready(function () {
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






        function ClearText(TextBox) {
            if (TextBox.value == "Search Part # ...")
                TextBox.value = "";

            TextBox.style.color = "#CCC";
        }


        function focuslost2(TextBox) {
            if (TextBox.value == "")
                TextBox.value = "Search Part # ...";
            TextBox.style.color = "#A4A4A4";
        }

       

    </script>


    <div id="divItemPrintData">
        <div id="divItemPrintDataContainer" style="display: block; padding: 19px;">

            <asp:TreeView ID="trvmaterialattachment" Target="_blank" runat="server">
                <Nodes>
                    <asp:TreeNode Expanded="false"></asp:TreeNode>
                </Nodes>
            </asp:TreeView>
            <asp:Label ID="lblfileslist" runat="server"></asp:Label>

        </div>
    </div>

    <!--Print div end-->

        <div class="dashed"></div>

    <div class="pagewidth">
    <table align="center" border="0" cellpadding="3" cellspacing="2" width="100%" >


        <tr>
            <td colspan="4" width="100%">&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2" class="FormLabels">



                <span class="mandatory_field">Note :  </span>
                <asp:Label ID="lblRedIndicator" runat="server" Font-Bold="true" ForeColor="Red" Text=" * " />
                <span class="FormLabels">Indicates mandatory fields </span></td>

            <td align="right" colspan="2">
                <asp:Panel runat="server" ID="pnlSearch" DefaultButton="lnkSearchMaterial">


                    <asp:TextBox ID="txtSearchMCode" runat="server" SkinID="txt_Req" MaxLength="15" Placeholder="Search Part # ..." Width="200" onfocus="ClearText(this)" onblur="javascript:focuslost2(this)" Visible="false" />
                    &nbsp;&nbsp;
                    <asp:LinkButton ID="lnkSearchMaterial" runat="server" CssClass="ui-btn ui-button-large" OnClick="lnkSearchMaterial_Click" Visible="false">Search<span class="space fa fa-search"></span></asp:LinkButton>




                </asp:Panel>

            </td>

        </tr>



        <tr>
            <td colspan="4">

                <asp:UpdatePanel ChildrenAsTriggers="true" ID="upnlStatus" UpdateMode="Always" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblStatus" runat="server" CssClass="errorMsg" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>



        <tr>
            <td colspan="4">

                <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="dvBMDHeader" style="">Basic Material Details </div>

                <div class="ui-Customaccordion" id="dvBMDBody">


                    <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" ID="upnlBasicMaterialData" UpdateMode="Always">
                        <ContentTemplate>


                            <table width="100%" style="padding-top: 10px; padding-left: 10px;">

                                <tr>

                                    <asp:Panel runat="server" ID="pnlMMCodeDetails" Visible="false">

                                        <td class="FormLabels" style="min-width: 264px;">
                                            <asp:UpdatePanel ChildrenAsTriggers="true" ID="upnlRvsnStatus" UpdateMode="Always" runat="server">
                                                <ContentTemplate>
                                                    <asp:RequiredFieldValidator ID="rfvCatalogNumber" ValidationGroup="UpdateMaterial" runat="server" ControlToValidate="txtMfgPartNo" Display="Dynamic"/>
                                                    <%--<asp:RequiredFieldValidator ID="rfvMaterialMasterCode" ValidationGroup="UpdateMaterial" runat="server" ControlToValidate="txtMCode" Display="Dynamic" ErrorMessage=" * " />--%>
                                                    <span style="color:red">*</span><asp:Literal ID="ltMCodeLabel" runat="server" Text="Part Number:&lt;br /&gt;" />
                                                    <%-- <asp:TextBox ID="txtMCode" runat="server" SkinID="txt_Req" />--%>






                                                    <asp:TextBox ID="txtMfgPartNo" runat="server" MaxLength="17" Width="200" />

                                                    <span id="ltSupplierPartPicHolder" style="padding: 0px; margin: 0px;"></span>



                                                    &nbsp;&nbsp;
                                    <asp:Label runat="server" ID="mmRevision" CssClass="SubHeading3"></asp:Label>

                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>

                                        <td class="FormLabels" style="min-width: 240px;">
                                            <asp:Literal ID="ltMCodeAlternativeLabel" runat="server" Text="Alternative Part# 1:&lt;br /&gt;" />
                                            <asp:TextBox ID="txtMCodeAlternative" runat="server" CssClass="txt_slim" Width="200" />
                                        </td>

                                        <td class="FormLabels">
                                            <asp:Literal ID="ltMCodeAlternativeLabel2" runat="server" Text="Alternative Part# 2:&lt;br /&gt;" />
                                            <asp:TextBox ID="txtMCodeAlternative2" runat="server" CssClass="txt_slim" Width="200" />


                                        </td>
                                        <td class="FormLabels" align="center" rowspan="3" style="vertical-align: top; padding-left: 14px; min-width: 200px">
                                            <asp:Literal ID="ltPicHolder" runat="server" />
                                            <br />
                                            <asp:Label ID="lblvieweattachment" runat="server"></asp:Label></>
                                        </td>

                                    </asp:Panel>

                                </tr>

                                <tr>

                                    <td class="FormLabels">
                                        <asp:RequiredFieldValidator ID="rfvatcPlantID" runat="server" ValidationGroup="UpdateMaterial" ControlToValidate="atcPlantID" Display="Dynamic" />
                                        <span style="color:red">*</span>Plant:<br />
                                        <asp:TextBox ID="atcPlantID" runat="server" SkinID="txt_Req"></asp:TextBox>
                                        <asp:HiddenField ID="hifPlantID" runat="server" />
                                    </td>




                                    <td class="FormLabels" valign="bottom" align="left">
                                        <asp:TextBox ID="txtCustomerPartNumber" Visible="false" runat="server" Width="200" />

                                        <asp:RequiredFieldValidator ID="rfvtxtOEMPartNo" Enabled="false" runat="server" ValidationGroup="UpdateMaterial" ControlToValidate="txtOEMPartNo" Display="Dynamic" ErrorMessage=" * " />
                                        OEM Part Number:<br />
                                        <asp:TextBox ID="txtOEMPartNo" runat="server" Width="200"></asp:TextBox>


                                    </td>
                                    <td valign="middle" class="FormLabels">

                                        <asp:CheckBox ID="chkIsApproved" runat="server" Text="Approved" Visible="false" />
                                        &nbsp;&nbsp;<asp:CheckBox ID="chkIsActive" runat="server" Text="Active" />

                                    </td>

                                </tr>

                                <tr>

                                    <td class="FormLabels">
                                        <asp:RequiredFieldValidator ID="rfvatcMTypeID" runat="server" ValidationGroup="UpdateMaterial" ControlToValidate="atcMTypeID" Display="Dynamic" ErrorMessage=" * " Visible="false" />
                                        <asp:TextBox ID="atcMTypeID" runat="server" Visible="false" Width="200" SkinID="txt_Req"></asp:TextBox>
                                        <asp:HiddenField ID="hifMTypeID" runat="server" Visible="false" />

                                        <asp:UpdatePanel ChildrenAsTriggers="true" ID="upnlMMType" UpdateMode="Always" runat="server">
                                            <ContentTemplate>
                                                <asp:RequiredFieldValidator ID="rfvddlMTypeID" ValidationGroup="UpdateMaterial" runat="server" ControlToValidate="ddlMTypeID" Display="Dynamic" InitialValue="0" />
                                                <span style="color:red">*</span> Material Type:<br />
                                               <asp:DropDownList ID="ddlMTypeID" runat="server" Width="200" AutoPostBack="true" OnSelectedIndexChanged="ddlMTypeID_SelectedIndexChanged" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                    </td>

                                    <td class="FormLabels">
                                        <asp:RequiredFieldValidator ID="rfvatcStorageConditionID" runat="server" ValidationGroup="UpdateMaterial" ControlToValidate="atcStorageConditionID" Display="Dynamic"/>
                                        <span style="color:red">*</span>Storage Condition:<br />
                                        <asp:TextBox ID="atcStorageConditionID" runat="server" SkinID="txt_Req" Width="180"></asp:TextBox>
                                        <asp:HiddenField ID="hifStorageConditionID" runat="server" />

                                    </td>

                                    <td class="FormLabels">
                                        <asp:RequiredFieldValidator ID="rfvatcProductCategories" runat="server" ValidationGroup="UpdateMaterial" ControlToValidate="atcProductCategories" Display="Dynamic" />
                                        <span style="color:red">*</span>Product Category:<br />
                                        <asp:TextBox ID="atcProductCategories" runat="server" SkinID="txt_Req"></asp:TextBox>
                                        <asp:HiddenField ID="hifProductCategories" runat="server" />

                                    </td>


                                </tr>

                                <tr>

                                

                                    <td class="FormLabels" valign="bottom" colspan="2">
                                        <asp:RequiredFieldValidator ID="rfvtxtDescriptionShort" ValidationGroup="UpdateMaterial" runat="server" ControlToValidate="txtDescription" Display="Dynamic"/>
                                        <span style="color:red">*</span>Item Description [Short]:<br />
                                        <asp:TextBox ID="txtDescription" runat="server" Rows="3" TextMode="MultiLine" Width="470" MaxLength="100" />
                                    </td>
                                    <td class="FormLabels" valign="bottom">Item Description [Long]:<br />
                                        <asp:TextBox ID="txtDescriptionLong" runat="server" Rows="3" TextMode="MultiLine" Width="375" MaxLength="100" />
                                    </td>

                                </tr>
                                <tr>
                                    <td>

                                    </td>
                                </tr>
                                <tr>

                                    <td class="FormLabels" colspan="2" valign="top">Remarks:
                                    <br />
                                        <asp:TextBox ID="txtRemarks" runat="server" Rows="3" TextMode="MultiLine" Width="470" />
                                    </td>
                                    <td>

                                        <asp:UpdatePanel ChildrenAsTriggers="true" ID="upnlRevisionHistory" UpdateMode="Always" runat="server">
                                            <ContentTemplate>

                                                <asp:Panel runat="server" ID="pnlRvsnHistory" Visible="false"  Width="400px">

                                                    <table border="0">

                                                        <tr>
                                                            <td>
                                                                <%--<h4>Revision History  </h4>--%>
                                                            </td>
                                                            <td class="FormLabels" align="right">
                                                                <asp:LinkButton runat="server" ID="lnkAddRvsnHistory" Visible="false" OnClick="lnkAddRvsnHistory_Click" SkinID="lnkButEmpty" Text="Add Revision"></asp:LinkButton>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">

                                                                <asp:GridView ID="gvRvsnHistory" SkinID="gvLightSteelBlueNew" runat="server" Width="100" CellPadding="4" CellSpacing="4"
                                                                    OnPageIndexChanging="gvRvsnHistory_PageIndexChanging"
                                                                    OnRowCancelingEdit="gvRvsnHistory_RowCancelingEdit"
                                                                    OnRowEditing="gvRvsnHistory_RowEditing"
                                                                    OnRowUpdating="gvRvsnHistory_RowUpdating"
                                                                    AllowPaging="true" Visible="false">

                                                                    <Columns>

                                                                        <asp:TemplateField HeaderText="Revision">
                                                                            <ItemTemplate>
                                                                                <asp:Literal ID="ltRevision" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Revision") %>' />
                                                                                <asp:Literal ID="lthidMaterialMasterRevisionID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialMasterRevisionID") %>' />
                                                                            </ItemTemplate>
                                                                            <EditItemTemplate>
                                                                                <asp:RequiredFieldValidator ID="rfvtxtRevision" ValidationGroup="UpdateRevision" runat="server" ControlToValidate="txtRevision" Display="Dynamic" ErrorMessage=" * " />
                                                                                <asp:TextBox ID="txtRevision" runat="server" Width="40" Text='<%# DataBinder.Eval(Container.DataItem,"Revision").ToString() %>'></asp:TextBox>
                                                                                <asp:Literal ID="lthidMaterialMasterRevisionID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialMasterRevisionID") %>' />
                                                                            </EditItemTemplate>
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="Effective Date">
                                                                            <ItemTemplate>
                                                                                <asp:Literal ID="ltEffectedDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "EffectiveDate","{0: dd/MM/yyyy}") %>' />
                                                                            </ItemTemplate>
                                                                            <EditItemTemplate>
                                                                                <asp:RequiredFieldValidator ID="rfvtxtEffectiveDate" runat="server" ControlToValidate="txtEffectiveDate" ValidationGroup="UpdateRevision" Display="Dynamic" ErrorMessage=" * " />
                                                                                <asp:TextBox ID="txtEffectiveDate" Width="80" runat="server" EnableTheming="false" CssClass="DynaEffectiveDate" Text='<%# DataBinder.Eval(Container.DataItem, "EffectiveDate","{0: dd/MM/yyyy}") %>'></asp:TextBox>
                                                                            </EditItemTemplate>
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="Description">
                                                                            <ItemTemplate>
                                                                                <asp:Literal ID="ltDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>' />
                                                                            </ItemTemplate>
                                                                            <EditItemTemplate>
                                                                                <asp:TextBox ID="txtDescription" Rows="3" TextMode="MultiLine" Width="100" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>' />
                                                                            </EditItemTemplate>
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField ItemStyle-Width="20" HeaderText="Delete" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint">

                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkIsDelete" runat="server" />
                                                                            </ItemTemplate>
                                                                            <EditItemTemplate></EditItemTemplate>

                                                                            <FooterTemplate>
                                                                                <asp:LinkButton ID="lnkRvsnDelete" Font-Underline="false" CssClass="blueLink" runat="server" Text="<nobr><i class='material-icons ss'>delete</i></nobr>" OnClick="lnkRvsnDelete_Click" OnClientClick="return confirm('Are you sure you want to delete the selected line items?')" />
                                                                            </FooterTemplate>
                                                                        </asp:TemplateField>

                                                                        <asp:CommandField ValidationGroup="UpdateRevision" ItemStyle-Font-Underline="false" ControlStyle-Font-Underline="false" ItemStyle-Width="30" ButtonType="Link" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint" ItemStyle-HorizontalAlign="left" CancelImageUrl="Images/cancel.gif" EditImageUrl="/Images/edit.gif" EditText="<nobr> <i class='material-icons ss'>mode_edit</i></nobr>" ShowEditButton="True" UpdateImageUrl="/Images/save.gif" />

                                                                    </Columns>

                                                                </asp:GridView>


                                                            </td>
                                                        </tr>

                                                    </table>

                                                </asp:Panel>

                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                    </td>

                                </tr>

                                <tr>
                                    <td colspan="12" align="right">
                                         <div id="divMaterialActions">
                                        <br />
                                        <asp:LinkButton runat="server" ID="lnkButCancel2" CssClass="ui-btn ui-button-large" OnClick="lnkButCancel_Click">Cancel&nbsp;<i class="material-icons vl">cancel</i></asp:LinkButton>

                                        

                                <asp:LinkButton ValidationGroup="UpdateMaterial" CssClass="ui-btn ui-button-large" CausesValidation="true" ID="lnkSendRequest2" runat="server" OnClick="lnkSendRequest_Click" OnClientClick="return  valid();">  </asp:LinkButton>
                                        &nbsp;&nbsp;&nbsp;
                                        </div>
                                    </td>
                                </tr>



                            </table>



                        </ContentTemplate>
                    </asp:UpdatePanel>







                </div>
            </td>
        </tr>


        <!-- Industries Data -->

        <tr>
            <td colspan="4">
                 <input type="hidden" value="0" id="Hidden1"/>
                <div id="divIndustryData" style="display: none;" class="divIndustryData">
                    <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="divIndustryHeader">Industrial Attributes</div>
                    <div class="ui-Customaccordion" id="divIndustryBody" style="padding:20px;">
                        Industry:<br />
                        <asp:TextBox ID="txtIndustry" runat="server" SkinID="txt_Req"></asp:TextBox>
                        <asp:HiddenField ID="hdnIndustry" runat="server" />

                        <div class="divGetIndustry"><br />
                        <div id="divIndustryContent">

                        </div>

                        </div>
                      
                            <br />
                            <asp:LinkButton runat="server" ID="lnkButCancel1" Visible="false" CssClass="ui-btn ui-button-large" OnClick="lnkButCancel_Click">Cancel&nbsp;<i class="material-icons vl">cancel</i></asp:LinkButton>

                            &nbsp;&nbsp;&nbsp;

                                <%--<asp:LinkButton ValidationGroup="UpdateMaterial" CssClass="ui-btn ui-button-large" CausesValidation="true" ID="lnkSendRequest2" runat="server" OnClick="lnkSendRequest_Click" OnClientClick="return  valid();">  </asp:LinkButton>--%>
                            <asp:LinkButton ValidationGroup="UpdateMaterial" CssClass="ui-btn ui-button-large" CausesValidation="true" ID="lnkSendRequest3" OnClick="lnkSendRequest_Click" runat="server" OnClientClick="UpsertIndustries();"></asp:LinkButton>
                            &nbsp;&nbsp;&nbsp;

                    </div>
                </div>
            </td>
        </tr>

        <!--  Industries Data -->

        <!-- Tenant Details-->


        <tr>
            <td colspan="4">

                <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="divTDHeader" style="">Tenant Details </div>

                <div class="ui-Customaccordion" id="divTDBody">


                    <table width="100%">

                        <tr>
                            <td colspan="4">

                                <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" ID="upnlTenant" UpdateMode="Always">

                                    <ContentTemplate>

                                        <table width="100%" class="" cellspacing="10">

                                            <tr>

                                                <td class="FormLabels">
                                                    <asp:RequiredFieldValidator ValidationGroup="UpdateTenant" ID="rfvTenant" runat="server" ControlToValidate="txtTenant" Display="Dynamic" ErrorMessage=" * " />
                                                    Tenant:<br />
                                                    <asp:TextBox ID="txtTenant" runat="server" CssClass="txt_slim" SkinID="txt_Hidden_Req" Width="180" />
                                                    <asp:HiddenField ID="hifTenant" runat="server" />
                                                </td>

                                                <td class="FormLabels">
                                                    <asp:RequiredFieldValidator ValidationGroup="UpdateTenant" ID="rfvSpaceUtilization" runat="server" ControlToValidate="txtSpaceUtilization" Display="Dynamic" ErrorMessage=" * " />
                                                    Space Utilization:<br />
                                                    <asp:TextBox ID="txtSpaceUtilization" runat="server" CssClass="txt_slim" SkinID="txt_Hidden_Req" Width="180" />
                                                    <asp:HiddenField ID="hifSpaceUtilization" runat="server" />
                                                </td>

                                                <td class="FormLabels" colspan="2" style="display:none">Tenant Part No:<br />
                                                    <asp:TextBox ID="txtTenantPartNo" runat="server" onKeyPress="return checkDec(this,event)" MaxLength="18" Width="200" />
                                                </td>
                                                
                                                <td class="FormLabels" colspan="2" style="visibility:hidden;">Tenant Part No:<br />
                                                   <input />
                                                </td>

                                            </tr>

                                            <tr>
                                                <td class="FormLabels" style="display:none">Price:<br />
                                                    <asp:RequiredFieldValidator ValidationGroup="UpdateTenant" ID="rfvPrice" runat="server" ControlToValidate="txtPrice" Display="Dynamic" ErrorMessage=" * " />
                                                    <asp:TextBox ID="txtPrice" runat="server" CssClass="txt_slim" MaxLength="19" onKeyPress="return checkDec(this,event)" Width="200" />
                                                </td>

                                                <td class="FormLabels" colspan="4" style="display:none">Insurance:<br />
                                                    <asp:CheckBox ID="chkIsInsurance" runat="server" />

                                                </td>

                                            </tr>
                                            <tr>
                                                <td colspan="4" align="right">
                                                    <asp:LinkButton ID="lnkUpdateTenant" ValidationGroup="UpdateTenant" CausesValidation="true" runat="server" OnClick="lnkUpdateTenant_Click" CssClass="ui-button-small">Update Tenant<%=MRLWMSC21Common.CommonLogic.btnfaUpdate %></asp:LinkButton>
                                                </td>
                                            </tr>



                                        </table>

                                        <table border="0" align="left" width="100%" style="min-width: 930px;">

                                            <tr>

                                                <td align="right" style="vertical-align: top;">

                                                    <asp:LinkButton ID="lnkAddTenant" OnClick="lnkAddTenant_Click" Visible="false" runat="server" CssClass="ui-button-small">Add Tenant<%=MRLWMSC21Common.CommonLogic.btnfaNew %></asp:LinkButton>

                                                </td>

                                            </tr>

                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="lblTenant"></asp:Label>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td align="center">
                                                    <asp:Panel runat="server" ID="pnlTenantList" ScrollBars="Auto" Visible="false">
                                                        <br />

                                                        <asp:GridView SkinID="gvLightSteelBlueNew" ID="gvTenantList" runat="server" PagerSettings-Position="TopAndBottom" AllowPaging="true" PageSize="10" AllowSorting="True"
                                                            HorizontalAlign="Left"
                                                            OnSorting="gvTenantList_Sorting"
                                                            OnPageIndexChanging="gvTenantList_PageIndexChanging"
                                                            OnRowDataBound="gvTenantList_RowDataBound"
                                                            OnRowEditing="gvTenantList_RowEditing"
                                                            OnRowCancelingEdit="gvTenantList_RowCancelingEdit"
                                                            OnRowUpdating="gvTenantList_RowUpdating">
                                                            <Columns>

                                                                <asp:TemplateField ItemStyle-Width="300" HeaderText="Tenant">
                                                                    <ItemTemplate>
                                                                        <asp:Literal runat="server" ID="ltTenant" Text='<%# DataBinder.Eval(Container.DataItem, "CompanyName") %>' />
                                                                        <asp:Literal ID="ltTenantMaterialMasterID" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TenantMaterialMasterID") %>' />
                                                                        <asp:Literal ID="ltTenantID" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TenantID") %>' />
                                                                    </ItemTemplate>

                                                                    <EditItemTemplate>

                                                                        <asp:Literal ID="ltTenantMaterialMasterID" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TenantMaterialMasterID") %>' />
                                                                        <asp:RequiredFieldValidator ID="rfvCompanyName" runat="server" Display="Dynamic" ControlToValidate="txtCompanyName" ValidationGroup="vRequiredTenant" ErrorMessage="*" />
                                                                        <asp:TextBox ID="txtCompanyName" ClientIDMode="Static" SkinID="txt_Hidden_Req" Width="150" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"CompanyName") %>' />
                                                                        <asp:HiddenField ID="hifTenantID" runat="server" ClientIDMode="Static" Value='<%# DataBinder.Eval(Container.DataItem, "TenantID").ToString() %>' />
                                                                        <asp:Literal ID="ltTenantID" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TenantID") %>' />

                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField ItemStyle-Width="100" HeaderText="Space Utilization">
                                                                    <ItemTemplate>
                                                                        <asp:Literal runat="server" ID="ltSpaceUtilizationID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "SpaceUtilizationID").ToString() %>' />
                                                                        <asp:Literal runat="server" ID="ltSpaceUtilization" Text='<%# DataBinder.Eval(Container.DataItem, "SpaceUtilization").ToString() %>' />

                                                                    </ItemTemplate>

                                                                    <EditItemTemplate>
                                                                        <asp:RequiredFieldValidator ID="rfvSpaceUtilization" runat="server" Display="Dynamic" ControlToValidate="txtSpaceUtilization" ValidationGroup="vRequiredTenant" ErrorMessage="*" />
                                                                        <asp:TextBox ID="txtSpaceUtilization" ClientIDMode="Static" SkinID="txt_Hidden_Req" Width="100" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"SpaceUtilization") %>' />
                                                                        <asp:HiddenField ID="hifSpaceUtilizationID" runat="server" ClientIDMode="Static" Value='<%# DataBinder.Eval(Container.DataItem, "SpaceUtilizationID").ToString() %>' />
                                                                        <asp:Literal ID="ltSpaceUtilizationID" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"SpaceUtilizationID") %>' />

                                                                    </EditItemTemplate>

                                                                </asp:TemplateField>

                                                                <%--<asp:TemplateField ItemStyle-Width="150" HeaderText="Material Shape"  >
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="ltMaterialShape" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialShape").ToString() %>'/>
                                                    </ItemTemplate>
                                                     <EditItemTemplate>
                                                         <asp:RequiredFieldValidator ID="rfvMaterialShapeID" runat="server" Display="Dynamic" ControlToValidate="txtMaterialShape" ValidationGroup="vRequiredTenant" ErrorMessage="*" />
                                                         <asp:TextBox ID="txtMaterialShape" ClientIDMode="Static" SkinID="txt_Hidden_Req" Width="140"  runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialShape").ToString() %>'/>
                                                         <asp:HiddenField ID="hifMaterialShapeID" runat="server" ClientIDMode="Static" value='<%# DataBinder.Eval(Container.DataItem, "MaterialShapeID").ToString() %>'/>
                                                     </EditItemTemplate>
                                                </asp:TemplateField>--%>

                                                                <asp:TemplateField ItemStyle-Width="250" HeaderText="Company Registration #">
                                                                    <ItemTemplate>
                                                                        <asp:Literal runat="server" ID="ltTenantRegistrationNo" Text='<%# DataBinder.Eval(Container.DataItem, "TenantRegistrationNo") %>' />

                                                                    </ItemTemplate>
                                                                </asp:TemplateField>



                                                                <asp:TemplateField ItemStyle-Width="160" HeaderText="Tenant Part #">
                                                                    <ItemTemplate>
                                                                        <asp:Literal runat="server" ID="ltTenantPartNo" Text='<%# DataBinder.Eval(Container.DataItem, "TenantPartNo").ToString() %>' />
                                                                    </ItemTemplate>

                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtTenantPartNo" ClientIDMode="Static" Width="100" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TenantPartNo") %>' />

                                                                    </EditItemTemplate>

                                                                </asp:TemplateField>

                                                                <asp:TemplateField ItemStyle-Width="80" HeaderText="Price">
                                                                    <ItemTemplate>
                                                                        <asp:Literal runat="server" ID="ltPrice" Text='<%# DataBinder.Eval(Container.DataItem, "Price").ToString() %>' />
                                                                    </ItemTemplate>

                                                                    <EditItemTemplate>
                                                                        <asp:RequiredFieldValidator ID="rfvtxtPrice" runat="server" Display="Dynamic" ControlToValidate="txtPrice" ValidationGroup="vRequiredTenant" ErrorMessage="*" />
                                                                        <asp:TextBox ID="txtPrice" ClientIDMode="Static" Width="80" runat="server" onKeyPress="return checkDec(this,event)" onblur="CheckPUoMQty(this)" Text='<%#DataBinder.Eval(Container.DataItem,"Price") %>' />

                                                                    </EditItemTemplate>

                                                                </asp:TemplateField>

                                                                <asp:TemplateField ItemStyle-Width="110" HeaderText="Is Insurance">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="chkIsInsurance" runat="server" Text='<%#Getimage( DataBinder.Eval(Container.DataItem, "IsInsurance").ToString()) %>' />
                                                                    </ItemTemplate>

                                                                    <EditItemTemplate>
                                                                        <asp:CheckBox ID="chkIsInsurance" runat="server" Checked='<%# Convert.ToBoolean( Convert.ToInt16(DataBinder.Eval(Container.DataItem, "IsInsurance"))) %>' />
                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField ItemStyle-Width="40" HeaderText="Delete" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint">

                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkIsDelete" runat="server" />
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate></EditItemTemplate>

                                                                    <FooterTemplate>
                                                                        <asp:LinkButton ID="lnkTenantDelete" Font-Underline="false" CssClass="blueLink" runat="server" Text="<nobr><i class='material-icons ss'>delete</i></nobr>" OnClick="lnkTenantDelete_Click" OnClientClick="return confirm('Are you sure you want to delete the selected line items?')" />
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>

                                                                <asp:CommandField ValidationGroup="vRequiredTenant" ItemStyle-Font-Underline="false" ControlStyle-Font-Underline="false" ItemStyle-Width="50" ButtonType="Link" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint" ItemStyle-HorizontalAlign="left" CancelImageUrl="Images/cancel.gif" EditImageUrl="/Images/edit.gif" EditText="<nobr><i class='material-icons ss'>mode_edit</i></nobr>" ShowEditButton="True" UpdateImageUrl="/Images/save.gif" />





                                                            </Columns>
                                                            <PagerSettings FirstPageText="&amp;lt;&amp;lt;First Page" LastPageText="Last Page&amp;gt;&amp;gt;"
                                                                Mode="NumericFirstLast" PageButtonCount="15" />

                                                        </asp:GridView>

                                                    </asp:Panel>

                                                </td>
                                            </tr>

                                        </table>

                                    </ContentTemplate>

                                </asp:UpdatePanel>




                            </td>

                        </tr>

                    </table>

                </div>
            </td>
        </tr>

        <!-- Tenant Details-->

        <!-- Supplier Details -->

        <tr>
            <td colspan="4">

                <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="divSDHeader" style="">Supplier Details </div>

                <div class="ui-Customaccordion" id="divSDBody">


                    <table width="100%" style="padding-top: 10px; padding-left: 10px;">

                        <tr>
                            <td colspan="4">

                                <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" ID="upnlSupplierDetails" UpdateMode="Always">

                                    <ContentTemplate>

                                        <table border="0" align="left" width="100%" style="min-width: 930px;">

                                            <tr>

                                                <td align="right" style="vertical-align: top;">

                                                    <asp:LinkButton ID="lnkAddSupplier" OnClick="lnkAddSupplier_Click" runat="server" CssClass="ui-button-small">Add Supplier<%=MRLWMSC21Common.CommonLogic.btnfaNew %></asp:LinkButton>

                                                </td>

                                            </tr>

                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="lblSupplierStatus"></asp:Label>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td align="center">
                                                    <asp:Panel runat="server" ID="pnlSupplier" ScrollBars="Auto">
                                                        <br />
                                                        <asp:GridView ID="gvSupplierDetails" SkinID="gvLightSteelBlueNew" runat="server" Width="100" CellPadding="4" CellSpacing="4"
                                                            OnPageIndexChanging="gvSupplierDetails_PageIndexChanging"
                                                            OnRowCancelingEdit="gvSupplierDetails_RowCancelingEdit"
                                                            OnRowCommand="gvSupplierDetails_RowCommand"
                                                            OnRowDataBound="gvSupplierDetails_RowDataBound"
                                                            OnRowEditing="gvSupplierDetails_RowEditing"
                                                            OnRowUpdating="gvSupplierDetails_RowUpdating"
                                                            AllowPaging="true">
                                                            <Columns>

                                                                <asp:TemplateField HeaderText="Supplier Name" ItemStyle-Width="180">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="ltSupplier" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SupplierName") %>' />
                                                                        <asp:Literal ID="lthidMMT_MaterialMaster_SupplierID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialMaster_SupplierID") %>' />
                                                                        <asp:Literal ID="ltSupplierID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SupplierID") %>' />

                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:RequiredFieldValidator ID="rfvddlSuplierName" runat="server" ControlToValidate="atcSupplierName" Display="Dynamic"/>
                                                                       <span style="color:red">*</span><asp:Literal ID="ltSupplierID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SupplierID").ToString()%>'></asp:Literal>
                                                                        <asp:TextBox ID="atcSupplierName" CssClass="SupplierPicker" EnableTheming="false" runat="server" Width="150" Text='<%# DataBinder.Eval(Container.DataItem,"SupplierName").ToString() %>'></asp:TextBox>
                                                                        <asp:Literal ID="lthidMMT_MaterialMaster_SupplierID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialMaster_SupplierID") %>' />

                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="EAN" ItemStyle-Width="150">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="ltSupplierPartNo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SupplierPartNumber") %>' />
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:RequiredFieldValidator ID="rfvdtxtSupplierPartNumber" runat="server" ControlToValidate="txtSupplierPartNumber" Display="Dynamic" ErrorMessage=" * " />
                                                                        <asp:TextBox ID="txtSupplierPartNumber" runat="server" Width="150" Text='<%# DataBinder.Eval(Container.DataItem,"SupplierPartNumber").ToString() %>'></asp:TextBox>

                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>


                                                                <asp:TemplateField HeaderText="Currency" ItemStyle-Width="120">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="ltCurrency" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Currency") %>' />
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>

                                                                        <asp:TextBox ID="atcInvCurrency" ClientIDMode="Static" EnableTheming="false" CssClass="txt_small_Req" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Currency") %>' />
                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Unit Cost">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="ltExpectedUnitCost" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ExpectedUnitCost") %>' />

                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>

                                                                        <asp:TextBox ID="txtUnitCost" onKeyPress="return checkDec(this,event)" Width="80" MaxLength="8" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ExpectedUnitCost") %>' />
                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>


                                                                <asp:TemplateField HeaderText="Planned Delivery Time(In Days )">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="ltPlannedDeliveryTime" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PlannedDeliveryTime") %>' />
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>

                                                                        <asp:TextBox ID="txtDeliveryTime" onKeyPress="return checkNum(event)" Width="80" MaxLength="8" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PlannedDeliveryTime") %>' />
                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Quantity">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="ltInitialOrdQty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "InitialOrderQuantity") %>' />

                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>

                                                                        <asp:TextBox ID="txtInitialOrdQty" onKeyPress="return checkDec(this,event)" Width="80" MaxLength="8" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "InitialOrderQuantity") %>' />
                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField ItemStyle-Width="80" HeaderText="Delete" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint">

                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkIsDelete" runat="server" />
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate></EditItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:LinkButton ID="lnkSupDelete" Font-Underline="false" CssClass="blueLink" runat="server" Text="<nobr><i class='material-icons ss'>delete</i></nobr>" OnClick="lnkSupDelete_Click" OnClientClick="return confirm('Are you sure you want to delete the selected line items?');" />
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>

                                                                <asp:CommandField ItemStyle-Font-Underline="false" ControlStyle-Font-Underline="false" ItemStyle-Width="30" ButtonType="Link" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint" ItemStyle-HorizontalAlign="left" CancelImageUrl="Images/cancel.gif" EditImageUrl="/Images/edit.gif" EditText="<nobr> Edit <img src='../Images/redarrowright.gif' border='0' /></nobr>" ShowEditButton="True" UpdateImageUrl="/Images/save.gif" />

                                                            </Columns>
                                                        </asp:GridView>
                                                    </asp:Panel>

                                                </td>
                                            </tr>

                                        </table>

                                    </ContentTemplate>

                                </asp:UpdatePanel>




                            </td>

                        </tr>

                    </table>

                </div>
            </td>
        </tr>

        <!-- Supplier Details -->


        <tr>
            <td colspan="4">

                <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="divUOMHeader">

                    <table border="0" width="100%">

                        <tr>

                            <td width="60%">Unit of Measurement(UoM) Configuration  &nbsp;&nbsp;
            

                            </td>

                            <td style="display: none">Data Capture Requirements</td>
                        </tr>


                    </table>

                </div>

                <div class="ui-Customaccordion" id="divUOMBody">

                    <table width="100%" style="padding-top: 10px; padding-left: 10px;">




                        <tr>

                            <td colspan="2" align="left" style="vertical-align: top; width: 58%;">

                                <asp:UpdatePanel ChildrenAsTriggers="true" ID="upUoM" UpdateMode="Always" runat="server">

                                    <ContentTemplate>

                                        <table border="0" width="100%" style="vertical-align: top;">

                                            <tr>
                                                <td colspan="2">





                                                    <asp:Panel runat="server" ID="pnlStandardUoM">

                                                        <table border="0" cellpadding="3" width="100%" cellspacing="3">
                                                            <tr>
                                                                <td colspan="2" align="left">

                                                                    <asp:Label runat="server" ID="ltgvUoMStatus" CssClass="SubHeading4"></asp:Label>
                                                                </td>
                                                            </tr>

                                                            <tr>
                                                                <td class="FormLabelsBlue" rowspan="2">Measurement Type:</td>
                                                                <td colspan="2" align="right">

                                                                    <asp:LinkButton runat="server" ID="lnkUoMConversion" OnClick="lnkUoMConversion_Click" Font-Underline="false">

                    UoM Conversion <span class="space fa fa-cog"></span>

                                                                    </asp:LinkButton>

                                                                </td>
                                                            </tr>
                                                            <tr>

                                                                <td visible="false" style="background-color: #e6f2f8; color: black; font-weight: bold; text-align: center;" align="left" class="FormLabelsBlue" rowspan="2" id="tdConversion" runat="server">
                                                                    <%--<label id="lbConversion" style="font-size: 20px;"></label>--%>
                                                                    <asp:Label ID="lbConversion" runat="server" ClientIDMode="Static" Font-Size="20px"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" class="FormLabels">

                                                                    <asp:DropDownList ID="ddlMeadureType" OnSelectedIndexChanged="ddlMeadureType_SelectedIndexChanged" runat="server" AutoPostBack="true">
                                                                        <asp:ListItem Text="Length" Value="1"></asp:ListItem>
                                                                        <%--<asp:ListItem Text="Area" Value="2"></asp:ListItem>--%>
                                                                        <%--<asp:ListItem Text="Volume" Value="3"></asp:ListItem>--%>
                                                                        <asp:ListItem Text="Weight" Value="4"></asp:ListItem>
                                                                        <asp:ListItem Text="Liquid Measurements" Value="5"></asp:ListItem>
                                                                        <%--<asp:ListItem Text="Temperature" Value="6"></asp:ListItem>--%>
                                                                        <asp:ListItem Text="Other" Value="0" Selected="True"></asp:ListItem>
                                                                    </asp:DropDownList>

                                                                </td>

                                                            </tr>
                                                            <tr id="trMeasurements" runat="server" visible="false">
                                                                <td class="FormLabels">From:<br />
                                                                    <asp:DropDownList ID="ddlFromMeasurement" OnSelectedIndexChanged="ddlFromMeasurement_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                                                </td>
                                                                <td class="FormLabels">To:<br />
                                                                    <asp:DropDownList ID="ddlToMeasurement" runat="server" OnSelectedIndexChanged="ddlToMeasurement_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                                </td>

                                                            </tr>


                                                        </table>





                                                    </asp:Panel>



                                                    </div>

                              
   

                                                </td>

                                            </tr>

                                            <tr>
                                                <td colspan="2">&nbsp;</td>
                                            </tr>

                                            <tr>
                                                <td align="left">&nbsp;
                                                </td>
                                                <td align="right" style="vertical-align: top;">
                                                    <asp:LinkButton ID="lnkUoMConfig" Visible="false" OnClick="lnkUoMConfig_Click" runat="server" CssClass="ui-button-small">
                                Add UoM <%=MRLWMSC21Common.CommonLogic.btnfaNew %>
                                                    </asp:LinkButton>


                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="2">

                                                    <div style="max-height: 170px; overflow-x: hidden;">
                                                        <%--<asp:Panel runat="server" ID="pnlUoMMeasurementGrid" Width="500px" Height="300px" HorizontalAlign="left" ScrollBars="Auto">--%>

                                                        <asp:GridView ID="gvUoM" SkinID="gvLightSteelBlueNew" runat="server" Width="498px" CellPadding="4"
                                                            AllowPaging="false"
                                                            AllowSorting="false"
                                                            OnPageIndexChanging="gvUoM_PageIndexChanging"
                                                            OnRowDataBound="gvUoM_RowDataBound"
                                                            OnRowCommand="gvUoM_RowCommand"
                                                            OnRowUpdating="gvUoM_RowUpdating"
                                                            OnRowCancelingEdit="gvUoM_RowCancelingEdit"
                                                            OnRowEditing="gvUoM_RowEditing"
                                                            PagerStyle-HorizontalAlign="Right"
                                                            CellSpacing="2">

                                                            <Columns>

                                                                <asp:TemplateField HeaderText="UoM Type" ItemStyle-Width="30%">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="ltUoMType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "UoMType") %>' />
                                                                        <asp:Literal ID="lthidMMT_MaterialMaster_UoMID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialMaster_UoMID") %>' />
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:RequiredFieldValidator ID="rfvddlvUoMType" runat="server" ControlToValidate="ddlvUoMType" Display="Dynamic" ErrorMessage=" * " InitialValue="0" ValidationGroup="vlgUoM" />
                                                                        <asp:DropDownList runat="server" ID="ddlvUoMType" Width="80" />
                                                                        <asp:Literal ID="lthidUoMTypeID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "UoMTypeID") %>' />
                                                                        <asp:Literal ID="lthidMMT_MaterialMaster_UoMID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialMaster_UoMID") %>' />
                                                                        <asp:Literal ID="ltUoMType" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "UoMType") %>' />
                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="UoM" ItemStyle-Width="30%">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="ltUoM" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "UoM") %>' />

                                                                        <asp:Literal ID="lthidUoMD" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "UoMID") %>' />
                                                                        <asp:HiddenField ID="hifMeasuretypeID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "MeasurementTypeID") %>' />
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:RequiredFieldValidator ID="rfvddlUoMConfig" runat="server" ControlToValidate="ddlUoMConfig" Display="Dynamic" ErrorMessage=" * " InitialValue="0" ValidationGroup="vlgUoM" />
                                                                        <asp:DropDownList runat="server" ID="ddlUoMConfig" Width="80" />
                                                                        <asp:Literal ID="lthidUoMD" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "UoMID") %>' />
                                                                        <asp:HiddenField ID="hifMeasuretypeID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "MeasurementTypeID") %>' />
                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Qty. Per UoM" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="25%">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="ltUoMQty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "UoMQty") %>' />
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:RequiredFieldValidator ID="rfvtxtUoMQty" runat="server" ControlToValidate="txtUoMQty" Display="Dynamic" ErrorMessage=" * " ValidationGroup="vlgUoM" />
                                                                        <asp:TextBox ID="txtUoMQty" Width="80" MaxLength="10" onKeyPress="return checkDec(this,event)" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "UoMQty") %>' />
                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField ItemStyle-Width="5%" HeaderText="Delete" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint">
                                                                    <HeaderTemplate>
                                                                        <nobr><asp:CheckBox ID="chkIsDeleteRFItemsAll" onclick="return check_uncheck (this );"  runat="server" /> Delete</nobr>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkIsDeleteRFItem" runat="server" />
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                    </EditItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:LinkButton ID="lnkDeleteRFItem" Font-Underline="false" CssClass="blueLink" runat="server" Text="<nobr> <i class='material-icons ss'>delete</i></nobr>" OnClick="lnkDeleteRFItem_Click" OnClientClick="return confirm('Are you sure you want to delete the selected line items?')" />
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>

                                                                <%--<asp:BoundField Visible="false" DataField="EditName" ItemStyle-Font-Underline="false" ReadOnly="true" ItemStyle-CssClass="NoPrint"  ControlStyle-CssClass ="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint" ControlStyle-Font-Underline="false"/>--%>

                                                                <asp:CommandField ValidationGroup="vlgUoM" ItemStyle-Font-Underline="false" ControlStyle-Font-Underline="false" ButtonType="Link" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint" ItemStyle-HorizontalAlign="left" CancelImageUrl="Images/cancel.gif" EditImageUrl="/Images/edit.gif" EditText="<nobr> <i class='material-icons ss'>mode_edit</i></nobr>" ShowEditButton="True" UpdateImageUrl="/Images/save.gif" />


                                                            </Columns>


                                                        </asp:GridView>

                                                        <%--</asp:panel>--%>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>

                                    </ContentTemplate>

                                </asp:UpdatePanel>

                            </td>

                            <td colspan="2" valign="top" align="center" style="padding-left: 50px; display: none">

                                <!-- MSP List Box -->
                                <asp:UpdatePanel ChildrenAsTriggers="true" ID="upnlMSP" UpdateMode="Always" runat="server">
                                    <ContentTemplate>

                                        <asp:Label runat="server" ID="mspStatusLabel"></asp:Label>
                                        <br />

                                        <asp:Panel ID="pnlMsp" runat="server" HorizontalAlign="center">

                                            <fieldset style="width: 320px; border-radius: 5px; border: 1px solid #045FB4;">
                                                <legend align="left">Material Storage Parameter</legend>
                                                <table border="0" cellpadding="2" align="center">
                                                    <tr>
                                                        <td align="right">
                                                            <asp:CheckBoxList ID="mspChkBoxList" CssClass="mspCheckBox" runat="server" RepeatColumns="1" RepeatDirection="Horizontal" TextAlign="left">
                                                            </asp:CheckBoxList>
                                                        </td>

                                                        <td>
                                                            <asp:CheckBoxList ID="mspIsRequiredCheckBoxList" runat="server" RepeatColumns="1" RepeatDirection="Horizontal" TextAlign="Right">
                                                            </asp:CheckBoxList>
                                                        </td>
                                                    </tr>
                                                </table>

                                                <table border="0" align="right">
                                                    <tr>
                                                        <td>
                                                            <asp:LinkButton runat="server" ID="lnkUpdateMSPs" CssClass="ui-button-small" OnClick="lnkUpdateMSPs_Click">Update MSPs <%=MRLWMSC21Common.CommonLogic.btnfaUpdate %></asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>


                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                            </td>

                        </tr>



                    </table>

                </div>
            </td>
        </tr>


        <%--Msp Configuration Begins--%>

        <tr>
            <td colspan="4">

                <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="divMspHeader" style="">MSP Configuration</div>

                <div class="ui-Customaccordion" id="divMSpBody">


                    <table width="100%" style="padding-top: 10px; padding-left: 10px;">

                        <tr>
                            <td colspan="4">

                                <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" ID="UpdatePanel2" UpdateMode="Always">

                                    <ContentTemplate>

                                        <table border="0" align="left" width="100%" style="min-width: 930px;">

                                            <tr>

                                                <td align="right" style="vertical-align: top;">

                                                </td>

                                            </tr>

                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="Label1"></asp:Label>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td align="center">
                                                    <asp:Panel runat="server" ID="Panel1" ScrollBars="Auto" Width="100%">
                                                        <br />
                                                        <asp:GridView ID="gvMsp" SkinID="gvLightSteelBlueNew" runat="server" Width="10%" CellPadding="4" CellSpacing="4"  AllowPaging="true" >                                                   
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Material Storage Parameter" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="ltMSpName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ParameterName") %>' />
                                                                        <asp:Literal ID="ltMaterialStorageParameterID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialStorageParameterID") %>' />
                                                                    </ItemTemplate>
                                                                   
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-Width="50" HeaderStyle-HorizontalAlign="Center" HeaderText="Is Required" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint">
                                                                    <HeaderTemplate>
                                                                       <%-- <nobr>--%>
                                                                            Is Required<br />
                                                                            <asp:CheckBox ID="chkIsRequiredItemsAll" onclick="return check_uncheckMSP(this);"  runat="server" />

                                                                        <%-- </nobr>--%>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkMSPIsRequired" onclick="return check_uncheckMSP(this);" runat="server" Checked='<%#Convert.ToBoolean(Eval("IsRequired"))%>' />
                                                                    </ItemTemplate>
                                                                    </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                        <br />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                                    </asp:Panel>

                                                </td>
                                            </tr>

                                            <tr>
                                                <td align="right">
                                                    <asp:LinkButton ID="linkmspsave" runat="server" CssClass="ui-button-small" OnClick="linkmspsave_Click">Save<%=MRLWMSC21Common.CommonLogic.btnfaUpdate %></asp:LinkButton>
                                                    </td>
                                            </tr>

                                        </table>

                                    </ContentTemplate>

                                </asp:UpdatePanel>




                            </td>

                        </tr>

                    </table>

                </div>
            </td>
        </tr>
        <%--Msp Configuration End--%>



        <!-- Start QC Parameters -->


        <tr>
            <td colspan="4">

                <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="divQCPDHeader">Inward QC Parameters Details </div>

                <div class="ui-Customaccordion" id="divQCPDBody">


                    <table width="100%" class="internalData">


                        <tr>
                            <td colspan="4" align="center">

                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">

                                    <ContentTemplate>

                                        <asp:Panel runat="server" ID="pnlMMQCParameters">

                                            <table border="0" cellpadding="0" cellspacing="0" align="left" width="100%" align="center" style="min-width: 925px;">

                                                <tr>
                                                    <td>

                                                        <asp:Label ID="lblQCStatus" runat="server"></asp:Label>

                                                    </td>
                                                    <td align="right">
                                                        <asp:LinkButton runat="server" ID="lnkAddQCParameters" CssClass="ui-button-small" OnClick="lnkAddQCParameters_Click">Add QC Parameters<%=MRLWMSC21Common.CommonLogic.btnfaNew %></span></asp:LinkButton>

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="left">
                                                        <br />

                                                        <asp:GridView Width="100%" ShowFooter="true" GridLines="Both" ID="gvQCParameters" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="true" PageSize="50" AllowSorting="True" SkinID="gvLightSteelBlueNew" HorizontalAlign="Left" OnPageIndexChanging="gvQCParameters_PageIndexChanging" OnRowDataBound="gvQCParameters_RowDataBound" OnRowEditing="gvQCParameters_RowEditing" OnRowUpdating="gvQCParameters_RowUpdating" OnRowCancelingEdit="gvQCParameters_RowCancelingEdit" OnRowCommand="gvQCParameters_RowCommand">

                                                            <Columns>

                                                                <asp:TemplateField ItemStyle-Width="80" HeaderText="Parameter Name" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal Visible="false" runat="server" ID="lthidMaterialMaster_QualityParameterID" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialMaster_QualityParameterID") %>' />
                                                                        <asp:Literal runat="server" ID="ltQCParameterName" Text='<%# DataBinder.Eval(Container.DataItem, "ParameterName") %>' />

                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:RequiredFieldValidator ID="rfvtxtQCParameter" runat="server" ControlToValidate="txtQCParameter" Display="Dynamic" ErrorMessage=" * " />
                                                                        <asp:Literal Visible="false" runat="server" ID="lthidMaterialMaster_QualityParameterID" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialMaster_QualityParameterID") %>' />
                                                                        <asp:TextBox ID="txtQCParameter" ClientIDMode="Static" SkinID="txt_Req" runat="server" Width="200" Text='<%# DataBinder.Eval(Container.DataItem, "ParameterName") %>' />
                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Min. Tolerance" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="ltMinTolerance" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MinTolerance") %>' />
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>

                                                                        <asp:TextBox ID="txtMinTolerance" ClientIDMode="Static" Width="80" MaxLength="10" onKeyPress="return checkDec(this,event)" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MinTolerance") %>' />
                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Max. Tolerance" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="ltMaxTolerance" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MaxTolerance") %>' />
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>

                                                                        <asp:TextBox ID="txtMaxTolerance" ClientIDMode="Static" Width="80" MaxLength="10" onKeyPress="return checkDec(this,event)" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MaxTolerance") %>' />
                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>



                                                                <asp:TemplateField ItemStyle-Width="100" HeaderText="Is Required" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="chkIsRequired" runat="server" Text='<%#Getimage( DataBinder.Eval(Container.DataItem, "IsRequired").ToString()) %>' />
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:CheckBox ID="chkIsRequired" runat="server" Checked='<%# Convert.ToBoolean( Convert.ToInt16(DataBinder.Eval(Container.DataItem, "IsRequired"))) %>' />
                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField ItemStyle-Width="45" HeaderText="Delete" ItemStyle-CssClass="txtCenteralign" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkChildIsDelete" runat="server" />
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate></EditItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:LinkButton Font-Underline="false" ID="lnkQCDelete" runat="server" Text="<i class='material-icons ss'>delete</i>" OnClick="lnkQCDelete_Click" OnClientClick="return confirm('Are you sure you want to delete the selected items?')" />
                                                                        <img border="0" src="../Images/redarrowright.gif" alt="delete" />
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>

                                                                <asp:BoundField Visible="false" DataField="EditName" ReadOnly="true" ItemStyle-HorizontalAlign="Right" />
                                                                <asp:CommandField ItemStyle-HorizontalAlign="Center" ControlStyle-Font-Underline="false" ItemStyle-Width="40" ButtonType="Link" CancelImageUrl="icons/cancel.gif" CancelText="Cancel" EditImageUrl="icons/edit.gif" EditText="<nobr> <i class='material-icons ss'>mode_edit</i></nobr>" ShowEditButton="True" UpdateImageUrl="icons/update.gif" UpdateText="Update" />
                                                            </Columns>

                                                        </asp:GridView>


                                                    </td>
                                                </tr>


                                            </table>

                                        </asp:Panel>
                                    </ContentTemplate>

                                </asp:UpdatePanel>


                            </td>
                        </tr>


                    </table>

                </div>
            </td>
        </tr>

        <!-- End QC Parameters -->



        <tr>
            <td colspan="4">

                <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="divIDimensionHeader">Item Dimensions </div>

                <div class="ui-Customaccordion" id="divIDimensionBody">

                    <asp:UpdatePanel ID="upnlItemDimention" runat="server" UpdateMode="Always">
                        <ContentTemplate>

                            <table width="100%" class="internalData" cellspacing="10" style="padding-left:0px;">


                                <tr>

                                    <td class="FormLabels">Length (cm):<br />

                                        <asp:TextBox ID="txtMLength" runat="server" CssClass="txt_slim" MaxLength="15" onKeyPress="return checkNum(event)" Width="200" />
                                    </td>

                                    <td class="FormLabels">Height (cm):<br />

                                        <asp:TextBox ID="txtMHeight" runat="server" CssClass="txt_slim" MaxLength="15" onKeyPress="return checkNum(event)" Width="200" />
                                    </td>

                                    <td class="FormLabels">Width (cm):<br />

                                        <asp:TextBox ID="txtMWidth" runat="server" CssClass="txt_slim" MaxLength="15" onKeyPress="return checkNum(event)" Width="200" />
                                    </td>

                                    <td class="FormLabels">Weight (kgs):<br />

                                        <asp:TextBox ID="txtMWeight" runat="server" CssClass="txt_slim" MaxLength="19" onKeyPress="return checkDec(this,event)" Width="200" />
                                    </td>

                                </tr>
                                <tr>
                                    <td class="FormLabels" colspan="4">Capacity Per Bin:<br />
                                        <%--<asp:RequiredFieldValidator ID="rfvCapcityperbin" runat="server" ControlToValidate="txtCapacityperBin" ErrorMessage="*" ValidationGroup="UpdateMaterial" />--%>
                                        <asp:TextBox ID="txtCapacityperBin" runat="server" onKeyPress="return checkDec(this,event)" />

                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="right">
                                        <asp:LinkButton ID="lnkupdateDimention" runat="server" OnClick="lnkupdateDimention_Click" CssClass="ui-button-small">Update <i class='material-icons vl'>update</i></asp:LinkButton>
                                    </td>
                                </tr>



                            </table>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </td>
        </tr>



        <tr>
            <td colspan="4" style="display: none">

                <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="divSPviewHeader">Sales View  / Purchasing View</div>

                <div class="ui-Customaccordion" id="divSPviewBody">


                    <table width="100%" class="internalData">


                        <tr>

                            <td class="FormLabels">
                                <asp:RequiredFieldValidator ID="rfvatcSalesUoM" Enabled="false" runat="server" ValidationGroup="UpdateMaterial" ControlToValidate="atcSalesUoM" Display="Dynamic" ErrorMessage=" * " />
                                Sales UoM:<br />
                                <asp:TextBox ID="atcSalesUoM" runat="server" SkinID="txt_Req"></asp:TextBox>
                                <asp:HiddenField ID="hifSalesUoM" runat="server" />

                            </td>
                            <td class="FormLabels">
                                <asp:RequiredFieldValidator ValidationGroup="UpdateMaterial" Enabled="false" ID="rfvConversionFactor" runat="server" ControlToValidate="txtSalesConvsFactor" Display="Dynamic" ErrorMessage=" * " />
                                Sales UoM Qty.:<br />
                                <asp:TextBox ID="txtSalesConvsFactor" runat="server" Width="200" MaxLength="18" onKeyPress="return checkDec(this,event)" />
                            </td>

                            <td class="FormLabels">
                                <asp:RequiredFieldValidator ID="rfvatcPurchaseUom" Enabled="false" runat="server" ValidationGroup="UpdateMaterial" ControlToValidate="atcPurchaseUom" Display="Dynamic" ErrorMessage=" * " InitialValue="" />
                                Purchasing UoM:<br />
                                <asp:TextBox ID="atcPurchaseUom" runat="server" SkinID="txt_Req"></asp:TextBox>
                                <asp:HiddenField ID="hifPurchaseUom" runat="server" />
                            </td>

                            <td class="FormLabels" style="padding-left: 20px;">
                                <asp:RequiredFieldValidator ValidationGroup="UpdateMaterial" Enabled="false" ID="rfvtxtPurchaseConvsFactor" runat="server" ControlToValidate="txtPurchaseConvsFactor" Display="Dynamic" ErrorMessage=" * " />
                                Purchasing UoM Qty.:<br />
                                <asp:TextBox ID="txtPurchaseConvsFactor" runat="server" MaxLength="18" Width="200" onKeyPress="return checkDec(this,event)" />
                            </td>

                        </tr>



                    </table>

                </div>
            </td>
        </tr>



        <tr>
            <td colspan="4">

                <%--   <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="divMRPHeader">Material Requirement Planning (MRP)</div>--%>
                <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="divBRPHeader">Bin Replenishment</div>

                <div class="ui-Customaccordion" id="divBRPBody">


                    <table width="100%" class="internalData">


                        <tr>

                            <td class="FormLabels">
                                <asp:RequiredFieldValidator
                                    ValidationGroup="UpdateMaterial" Visible="false" Enabled="false" ID="rfvtxtReorderPoint" runat="server" ControlToValidate="txtReorderPoint" Display="Dynamic" ErrorMessage=" * " />
                                <%--  Reorder Point:<br />--%>
                                <asp:TextBox ID="txtReorderPoint" runat="server" Visible="false" onKeyPress="return checkDec(this,event)" MaxLength="18" Width="200" />
                            </td>

                            <td class="FormLabels">
                                <asp:RequiredFieldValidator ValidationGroup="UpdateMaterial" Visible="false" Enabled="false" ID="rfvtxtReorderQtyMin" runat="server" ControlToValidate="txtReorderQtyMin" Display="Dynamic" ErrorMessage=" * " />
                                <%--  Reorder Qty. Min.:<br />--%>
                                <asp:TextBox ID="txtReorderQtyMin" runat="server" Visible="false" MaxLength="5" Width="200" onKeyPress="return checkDec(this,event)" />
                            </td>

                            <td class="FormLabels" colspan="2">
                                <asp:RequiredFieldValidator ValidationGroup="UpdateMaterial" Visible="false" Enabled="false" ID="rfvReorderQtyMax" runat="server" ControlToValidate="txtReorderQtyMax" Display="Dynamic" ErrorMessage=" * " />
                                <%--Reorder Qty. Max.:<br />--%>
                                <asp:TextBox ID="txtReorderQtyMax" runat="server" Visible="false" onKeyPress="return checkDec(this,event)" MaxLength="18" Width="200" />
                            </td>






                        </tr>

                        <tr>
                            <td colspan="3">
                                <table style="width:100%;">
                                    <tr> 
                                        <td class="FormLabels" style="width:20%"><span style="color:red">*</span>Location<br />
                                            <asp:RequiredFieldValidator ValidationGroup="UpdateMaterial" Enabled="false" ID="rfvLocation" runat="server" ControlToValidate="txtlocation" Display="Dynamic"/>
                                            <asp:TextBox ID="txtlocation" runat="server" SkinID="txt_Req" Width="200" MaxLength="18" />
                                            <asp:HiddenField ID="hiflocation" runat="server" />
                                            <asp:HiddenField ID="hifbinid" runat="server" />
                                            <asp:HiddenField ID="hifexstingloc" runat="server" />
                                        </td>
                                        <td style="width:5%">  &nbsp; </td>
                                        <td class="FormLabels" style="width:20%">
                                            <asp:RequiredFieldValidator ValidationGroup="UpdateMaterial" Enabled="false" ID="rfvMinimumStockLevel" runat="server" ControlToValidate="txtMinimumStockLevel" Display="Dynamic"/>
                                            <span style="color:red">*</span>Minimum Stock Level:<br />
                                            <asp:TextBox ID="txtMinimumStockLevel" runat="server" onKeyPress="return checkDec(this,event)" MaxLength="18" Width="200" />

                                        </td>
                                        <td style="width:5%"> &nbsp; </td>
                                        <td class="FormLabels" style="width:20%">
                                            <asp:RequiredFieldValidator ValidationGroup="UpdateMaterial" Enabled="false" ID="rfvMaximumStockLevel" runat="server" ControlToValidate="txtMaximumStockLevel" Display="Dynamic"/>
                                            <span style="color:red">*</span>Maximum Stock Level:<br />
                                            <asp:TextBox ID="txtMaximumStockLevel" runat="server" onKeyPress="return checkDec(this,event)" MaxLength="18" Width="200" />
                                        </td>
                                       <td style="width:5%"> &nbsp;  </td>
                                        <td class="FormLabels"  style="width:25%">
                                            <br />
                                            <asp:LinkButton runat="server" ID="lnkbtnsave" CssClass="ui-btn ui-button-large" Visible="false" OnClick="lnkbtnsave_Click">Save <%= MRLWMSC21Common.CommonLogic.btnfaSave  %> </asp:LinkButton>
                                        </td>

                                    </tr>
                                    <tr> 
                                        <td colspan="7">
                                                <br />
                                            
                                            <asp:GridView ID="gvbinreplishment" runat="server" SkinID="gvLightSteelBlueNew" runat="server" Width="100%" CellPadding="4"
                                                AllowPaging="false"
                                                AllowSorting="false"
                                                OnPageIndexChanging="gvbinreplishment_PageIndexChanging"
                                                            OnRowDataBound="gvbinreplishment_RowDataBound"
                                                            OnRowCommand="gvbinreplishment_RowCommand"
                                                            OnRowUpdating="gvbinreplishment_RowUpdating1"
                                                            OnRowCancelingEdit="gvbinreplishment_RowCancelingEdit"
                                                            OnRowEditing="gvbinreplishment_RowEditing" >
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-Width="23%" HeaderText="Location" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="ltlocationheader" Text='<%# DataBinder.Eval(Container.DataItem, "Location") %>' />
                                                            <asp:Label runat="server" Visible="false" ID="lblMMID" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialMasterID") %>' />
                                                            <asp:Label runat="server" Visible="false" ID="lbllocation" Text='<%# DataBinder.Eval(Container.DataItem, "LocationID") %>' />
                                                             <asp:Label runat="server" Visible="false" ID="lblBinRepid" Text='<%# DataBinder.Eval(Container.DataItem, "BinReplenishmentId") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-Width="23%" HeaderText="Minimum Stock Level" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="ltminimumstocklevel" Text='<%# DataBinder.Eval(Container.DataItem, "MinimumStockLevel") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-Width="23%" HeaderText="Maximum Stock Level" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="ltmaximumstocklevel" Text='<%# DataBinder.Eval(Container.DataItem, "MaximumStockLevel") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-Width="23%" HeaderText="Created By" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="ltcreatedby" Text='<%# DataBinder.Eval(Container.DataItem, "UserName") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                         <ItemTemplate>
                                                              <asp:LinkButton ID="lintedit" runat="server" Text="Edit" OnClick="lintedit_Click" CssClass="ButnEmpty"  ></asp:LinkButton>                 
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            
                                        </td>
                                    </tr>
                                </table>
                            </td>




                        </tr>




                    </table>

                </div>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="divMRPHeader">Material Replenishment</div>
                <div class="ui-Customaccordion" id="divMRPBody">
                    <table width="100%" class="internalData">
                        <tr>
                            
                            <td class="FormLabels">
                                <asp:RequiredFieldValidator
                                    ValidationGroup="UpdateMaterial" Visible="false" Enabled="false" ID="rfvtxtreordermin" runat="server" ControlToValidate="txtReorderPoint" Display="Dynamic" ErrorMessage=" * " />
                                   Reorder Qty. Min.:<br />
                                <asp:TextBox ID="txtreordermin" runat="server"  onKeyPress="return checkDec(this,event)" MaxLength="18" Width="200" />
                            </td>
                            <td class="FormLabels">
                                  Reorder Qty. Max.:<br />
                                <asp:TextBox ID="txtreordermax" runat="server"  MaxLength="5" Width="200" onKeyPress="return checkDec(this,event)" />
                            </td>
                            <td style="visibility:hidden"><input type="text"  ></td> <td><input type="text"  style="visibility:hidden"></td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>



        <tr>
            <td colspan="4">

                <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="divGPDHeader">General Plant Data / Storage</div>

                <div class="ui-Customaccordion" id="divGPDBody">


                    <table width="100%" class="internalData">

                        <tr>

                            <td class="FormLabels">
                                <asp:RequiredFieldValidator ID="rfvtxtRefContainer" runat="server" Enabled="false" ControlToValidate="txtRefContainer" Display="Dynamic" ErrorMessage=" * " />
                                Storage Condition:<br />
                                <asp:TextBox ID="txtRefContainer" runat="server" Enabled="false" SkinID="txt_Req"></asp:TextBox>

                            </td>

                            <td class="FormLabels"> &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;Min. Shelf Life(Days):
                <br />
                               &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; <asp:TextBox ID="txtMinShelfLife" runat="server" CssClass="txt_slim_rew" MaxLength="5" onKeyPress="return checkNum(event)" Width="200" />
                            </td>

                            <td class="FormLabels" width="50%">Total Shelf Life(Days):<br />
                                <asp:TextBox ID="txtTotalShelfLife" runat="server" MaxLength="5" onKeyPress="return checkNum(event)" Width="200" />
                            </td>



                        </tr>


                    </table>

                </div>
            </td>
        </tr>



        <tr>
            <td colspan="4" style="display: none">

                <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="divAPviewHeader">

                    <table widt="100%">

                        <tr>
                            <td style="min-width: 509px;">Accounting View</td>
                            <td>Production View</td>

                        </tr>

                    </table>


                </div>

                <div class="ui-Customaccordion" id="divAPviewBody">


                    <table width="100%" class="internalData">





                        <tr>


                            <td class="FormLabels">
                                <asp:RequiredFieldValidator ID="rfvatcUoM" runat="server" Enabled="false" ValidationGroup="UpdateMaterial" ControlToValidate="atcUoM" Display="Dynamic" ErrorMessage=" * " />
                                UoM:<br />
                                <asp:TextBox ID="atcUoM" runat="server" SkinID="txt_Req" Width="180"></asp:TextBox>
                                <asp:HiddenField ID="hifUoM" runat="server" />


                            </td>

                            <td class="FormLabels">Standard Price
                <asp:Literal ID="ltLoggedUserCurrency" runat="server" />:<br />
                                <asp:TextBox ID="txtStandardPrice" runat="server" CssClass="txt_slim" MaxLength="20" Width="200" onKeyPress="return checkDec(this,event)" />
                            </td>


                            <td class="FormLabels">
                                <asp:RequiredFieldValidator ID="rfvtxtPrUoM" Enabled="false" runat="server" ValidationGroup="UpdateMaterial" ControlToValidate="txtPrUoM" Display="Dynamic" ErrorMessage=" * " />
                                Production UoM:<br />
                                <asp:TextBox ID="txtPrUoM" runat="server" SkinID="txt_Req" Width="180"></asp:TextBox>
                                <asp:HiddenField ID="hifPrUoMID" runat="server" />


                            </td>


                            <td class="FormLabels" style="padding-left: 20px;">
                                <asp:RequiredFieldValidator ID="rfvtxtPrUoMQty" Enabled="false" runat="server" ValidationGroup="UpdateMaterial" ControlToValidate="txtPrUoMQty" Display="Dynamic" ErrorMessage=" * " />
                                Production UoM Qty.:
                <br />

                                <asp:TextBox ID="txtPrUoMQty" runat="server" MaxLength="20" Width="200" onKeyPress="return checkDec(this,event)" />
                            </td>

                        </tr>


                    </table>

                </div>
            </td>
        </tr>




        <!-- -------------Item Picture Attachement--------------------------->



        <tr>
            <td colspan="4">

                <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="divIPictureHeader">Item Pictures</div>

                <div class="ui-Customaccordion" id="divIPictureBody">


                    <table width="100%" class="internalData">

                        <tr>

                            <td class="FormLabels" colspan="2">Attach Picture File:
                 
                <br />


                                <asp:FileUpload ID="fuItemPicture" AllowMultiple="true" onchange="return checkFileExtension(this);" runat="server" Width="180" />



                            </td>

                            <td align="right" class="FormLabels" colspan="2">


                                <br />
                                <br />
                            </td>

                        </tr>

                    </table>

                </div>
            </td>
        </tr>







        <!--This is for multiple attachment starting -->


        <tr>
            <td colspan="4">

                <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="divSAHeader">Supplier Attachments</div>

                <div class="ui-Customaccordion" id="divSABody">


                    <table width="100%" class="internalData">


                        <tr>

                            <td colspan="4">


                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upsupplierattachment" RenderMode="Inline">

                                    <Triggers>

                                        <asp:PostBackTrigger ControlID="ddldocumenttype" />

                                    </Triggers>

                                    <ContentTemplate>


                                        <div runat="server" id="Atachment">

                                            <table border="0" width="100%" cellpadding="2" cellspacing="2">

                                                <tr>

                                                    <td class="FormLabels" valign="top"><span style="color:red">*</span>Supplier:
                                        <br />

                                                        <asp:UpdatePanel runat="server" UpdateMode="Always" ChildrenAsTriggers="true">
                                                            <ContentTemplate>

                                                                <asp:DropDownList ID="ddlsid" runat="server"></asp:DropDownList>

                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>




                                                    </td>

                                                    <td class="FormLabels" valign="top"><span style="color:red">*</span>Attachment Type:<br />

                                                        <asp:DropDownList ID="ddldocumenttype" runat="server" AutoPostBack="false"></asp:DropDownList>

                                                    </td>


                                                    <td class="FormLabels" valign="top">
                                                        <label>&nbsp;</label>
                                                        <div id="fileuploader">Upload</div>

                                                       
                                                        

                                                    </td>
                                                    <td><label>&nbsp;</label><br /><input type="button" id="startUpload" class="ui-btn ui-button-large" style="padding-right: 35px; margin-left: 0px" value="Start Upload" /></td>



                                                </tr>

                                            </table>


                                        </div>


                                    </ContentTemplate>
                                </asp:UpdatePanel>

                            </td>
                        </tr>

                    </table>

                </div>
            </td>
        </tr>

        <!--This is for multiple attachment Ending -->


        <tr>
            <td colspan="4">
                 <input type="hidden" value="0" id="GEN_TRN_Preference_ID"/>
                <div id="divPreferences" style="display:none;"></div>
               <%-- <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="divPreferenceHeader">Preferences</div>
                <div class="ui-Customaccordion" id="divPreferenceBody">
                    <table width="100%" class="internalData">
                        <tr>
                            <td class="FormLabels" colspan="2">
                                <label class="container">
                                    SLED
                                    <input type="checkbox" checked="checked" id="chkSLED" class="Preference">
                                    <span class="checkmark"></span><input type="text" />
                                </label>

                                 <label class="container">
                                    FEFO
                                    <input type="checkbox" id="chkFEFO" class="Preference">
                                    <span class="checkmark"></span>
                                </label>

                                 <label class="container">
                                    FIFO
                                    <input type="checkbox" id="chkFIFO" class="Preference">
                                    <span class="checkmark"></span>
                                </label>

                                 <label class="container">
                                    LIFO
                                    <input type="checkbox" id="chkLIFO" class="Preference">
                                    <span class="checkmark"></span>
                                </label>

                                 <label class="container">
                                    Packing Strategy
                                    <input type="checkbox" id="chkPacking" class="Preference">
                                    <span class="checkmark"></span>
                                </label>

                            </td>
                            <td align="right" class="FormLabels" colspan="2">
                                <br />
                                <br />
                            </td>
                        </tr>
                    </table>
                </div>--%>
            </td>
        </tr>

          <tr>

            <td class="FormLabels" colspan="4">
                <asp:Literal ID="lthidIsFirstEdit" runat="server" Visible="false" />
            </td>
        </tr>

        <tr>
            <td class="FormLabels" colspan="4" align="right">

                <asp:LinkButton runat="server" ID="lnkButCancel" CssClass="ui-btn ui-button-large" OnClick="lnkButCancel_Click"> Cancel <i class='material-icons'>cancel</i>  </asp:LinkButton>
                &nbsp;&nbsp;&nbsp;

                <asp:LinkButton ValidationGroup="UpdateMaterial" CssClass="ui-btn ui-button-large" CausesValidation="true" ID="lnkSendRequest" runat="server" OnClick="lnkSendRequest_Click" OnClientClick="return  valid();">  </asp:LinkButton>

            </td>
        </tr>


    </table>
</div>




    <asp:HiddenField ID="hifSupplierID" runat="server" />

    <asp:HiddenField ID="hifMMID" runat="server" />
    <asp:HiddenField ID="hifMtypeIDp" runat="server" />
    <asp:HiddenField ID="hifQCparameterID" runat="server" />



    <asp:Panel runat="server" ID="pnlDataCaptureReq" Visible="false">


        <asp:RequiredFieldValidator ID="rfvatcMaterialGroup" Enabled="false" runat="server" ValidationGroup="UpdateMaterial" ControlToValidate="atcMaterialGroup" Display="Dynamic" ErrorMessage=" * " />
        Material Group:<br />
        <asp:TextBox ID="atcMaterialGroup" runat="server" Width="200" SkinID="txt_Req"></asp:TextBox>
        <asp:HiddenField ID="hifMaterialGroup" runat="server" />



    </asp:Panel>


    <br />
    <br />
    <br />

    <script type="text/javascript">
        var ItemList = null;
        MasterID = new URL(window.location.href).searchParams.get("mid");
        // alert(MasterID);
        if (MasterID != null)
        {
            
            $("#divPreferences").css("display", "block");
        }
        function GetPreferencesList() {
            $.ajax({
                url: '<%=ResolveUrl("~/mMaterialManagement/MaterialMasterRequest.aspx/GetPreferences") %>',
                //data: "{ 'prefix': '" + request.term + "'}",
                data: "{}",
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

        GetPreferencesList();

        function GetPrefernces() {
            var displayid = "";
            displaypreferenceid = "";
            var PreferenceContainer = document.getElementById('divPreferences');
            var PreferenceContent = '';
            if (ItemList.Table != null && ItemList.Table.length > 0) {

                var GroupList = $.grep(ItemList.Table, function (a) { return a.GroupName == "Material Group" });

                PreferenceContent += '<div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="divPreferenceHeader">' + GroupList[0].GroupName + '</div><div class="ui-Customaccordion" id="divPreferenceBody" style="text-align:left;">';

                if (GroupList != null && GroupList.length > 0)
                {
                    var PrerenceList = $.grep(ItemList.Table1, function (a) { return a.GEN_MST_PreferenceGroup_ID == GroupList[0].GEN_MST_PreferenceGroup_ID });

                    PreferenceContent += '<div style="width:100%;padding:10px;">';
                    for (var i = 0; i < PrerenceList.length; i++)
                    {
                        PreferenceContent += '<div style="padding:10px;border:1px solid #fac18a;border-radius:4px;box-shadow:1px 3px 5px #CCCCCC;width:96.2%;display:inline-block;text-align:left;vertical-align: top;"><b>' + PrerenceList[i].PreferenceName + ' :</b><p></p><div style="-webkit-column-count: 3;-moz-column-count: 3;column-count: 3;">';

                        var OptionList = $.grep(ItemList.Table2, function (a) { return a.GEN_MST_Preference_ID == PrerenceList[i].GEN_MST_Preference_ID });

                        for (var j = 0; j < OptionList.length; j++)
                        {
                            if (PrerenceList[i].UIControlType == "TextBox")
                            {
                                //PreferenceContent += '<div><b>' + OptionList[j].OptionCode + '</b></div>';
                              
                                PreferenceContent += OptionList[j].OptionLabel + ' : <br/><input type="text" class="GetPreferenceOptions txt_Blue_Small" id="' + OptionList[j].GEN_MST_PreferenceOption_ID + '" name="' + PrerenceList[i].GEN_MST_Preference_ID + '" data-attr="' + PrerenceList[i].GEN_MST_PreferenceGroup_ID + '" value=""><p></P><p></P>';
                            }

                            else if (PrerenceList[i].UIControlType == "RadioButton") {
                                //PreferenceContent += '<div><b>' + OptionList[j].OptionCode + '</b></div>';
                                PreferenceContent += '<input type="radio" class="GetPreferenceOptions" id="' + OptionList[j].GEN_MST_PreferenceOption_ID + '" name="' + PrerenceList[i].GEN_MST_Preference_ID + '" value="' + GroupList[0].GEN_MST_PreferenceGroup_ID + '"> ' + OptionList[j].OptionLabel + '<br>';
                            }
                            else
                            {
                                PreferenceContent += '<input type="checkbox" class="GetPreferenceOptions" id="' + OptionList[j].GEN_MST_PreferenceOption_ID + '" name="' + PrerenceList[i].GEN_MST_Preference_ID + '" value="' + GroupList[0].GEN_MST_PreferenceGroup_ID + '"> ' + OptionList[j].OptionLabel + '<br>';
                            }

                          
                        }

                        PreferenceContent += '</div></div>&emsp;';
                        
                        if (i % 1 == 0) {
                            PreferenceContent += '<p></p>';
                        }

                    }
                    PreferenceContent += '</div>';
                }

             
                PreferenceContent += '<div style="text-align:right;padding:5px 23px; overflow:hidden; padding-right: 0;"><button type="button" class="ui-btn ui-button-large" onclick="UpsertPreferences()">Update Preferences <i class="space fa fa-database"></i></button></div>';

                PreferenceContainer.innerHTML = PreferenceContent;

                CustomAccordino($('#divPreferenceHeader'), $('#divPreferenceBody'));
            }           

            else {
                $(".PrefereModule").css("display", "none");
            }

            //for (var a = 0; a < displayid.split(',').length ; a++) {
            //    var divid = displayid.split(',')[a];
            //    $("#PanelBlockId" + divid).css("display", "none");
            //}

            //for (var a = 0; a < displaypreferenceid.split(',').length ; a++) {
            //    var divid = displaypreferenceid.split(',')[a];
            //    $(".Preference" + divid).css("display", "none");
            //}

            //DefaultPreferenceData(ItemList[44]);
        }

        function BindPreferences(MaterialId) {
           
            var PreferenceData = $.grep(ItemList.Table3, function (a) { return a.MaterialMasterID == MaterialId });
            FillPreferenceData(PreferenceData);
        }

        function FillPreferenceData(obj) {
            if (obj != null && obj.length > 0) {
                for (var i = 0; i < obj.length; i++) {
                    if (obj[i].UIControlType == "TextBox") {
                        $('#' + obj[i].GEN_MST_PreferenceOption_ID).val(obj[i].Value);
                    }
                    else if (obj[i].UIControlType == "CheckBox") {
                        //$('#' + obj[i].GEN_MST_PreferenceOption_ID).attr("checked", "checked");
                        $('#' + obj[i].GEN_MST_PreferenceOption_ID).prop("checked", true);
                    }
                    else
                    {
                        $('#' + obj[i].GEN_MST_PreferenceOption_ID).prop("checked", true);
                    }

                }
            }
        }

        function GetPrefernceFromData() {
           
           // var fieldDataOut = '{';
            var fieldData = '<root>';
            $(".GetPreferenceOptions").each(function () {
                var param = $(this).attr('id');
                var val = $(this).val().trim();
                var paramtype = $(this).attr('type');
                if (paramtype == "radio" || paramtype == "checkbox") {
                    val = $(this).prop('checked');
                    if (val == true) {
                        var GroupID = $(this).val().trim();
                        var PreferenceID = $(this)[0].name;
                        var OptionID = $(this)[0].id;
                        var OrgEntityID = 3;
                        fieldData += '<data>';
                        fieldData += '<GEN_MST_OrgEntity_ID>' + OrgEntityID + '</GEN_MST_OrgEntity_ID>';
                        fieldData += '<EntityID>' + MasterID + '</EntityID>';
                        fieldData += '<GEN_MST_PreferenceGroup_ID>' + GroupID + '</GEN_MST_PreferenceGroup_ID>';
                        fieldData += '<GEN_MST_Preference_ID>' + PreferenceID + '</GEN_MST_Preference_ID>';
                        fieldData += '<GEN_MST_PreferenceOption_ID>' + OptionID + '</GEN_MST_PreferenceOption_ID> </data>';
                    }
                }
                else
                {
                   
                    val = $(this).val();
                    if (val == null || val == "") {

                    }

                    else
                    {
                        var Value = $(this).val().trim();
                        var GroupID = $(this).attr("data-attr");
                        var PreferenceID = $(this)[0].name;
                        var OptionID = $(this)[0].id;
                        var OrgEntityID = 3;
                        fieldData += '<data>';
                        fieldData += '<Value>' + Value + '</Value>';
                        fieldData += '<GEN_MST_OrgEntity_ID>' + OrgEntityID + '</GEN_MST_OrgEntity_ID>';
                        fieldData += '<EntityID>' + MasterID + '</EntityID>';
                        fieldData += '<GEN_MST_PreferenceGroup_ID>' + GroupID + '</GEN_MST_PreferenceGroup_ID>';
                        fieldData += '<GEN_MST_Preference_ID>' + PreferenceID + '</GEN_MST_Preference_ID>';
                        fieldData += '<GEN_MST_PreferenceOption_ID>' + OptionID + '</GEN_MST_PreferenceOption_ID> </data>';
                    }
                }
            });
            fieldData = fieldData + '</root>';
            //fieldDataOut += '"' + String.fromCharCode(64) + 'inputDataXml' + '":"' + fieldData + '",' + '"' + String.fromCharCode(64) + 'LanguageType' + '":"' + 'en' + '",' + '"' + String.fromCharCode(64) + 'LoggedUserID' + '":"' + $("#hdnUpdatedBy").val() + '",';
           // fieldDataOut = fieldDataOut.substring(0, fieldDataOut.length - 1);
           // fieldDataOut += '}';
            //return fieldDataOut;
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
                else
                {
                    value = $(this).val();
                    if (value != null) {
                        status = true;
                    }
                }
            });
            return status;
        }

        function UpsertPreferences() {
            //if (InsertPreference()) 
            //{ 
            //    getallpreferences(); 
            //}
           
            var data = GetPrefernceFromData();

            var obj = {};
            obj.UserID = "<%=cp.UserID.ToString()%>";
            obj.Inxml = GetPrefernceFromData();
            $.ajax({
                url: "MaterialMasterRequest.aspx/SETPreferences",

                dataType: 'json',
                contentType: "application/json",
                type: 'POST',
                data: JSON.stringify(obj),
                success: function (response) {
                    if (response.d == "success") {
                     showStickyToast(true, 'Preferences Saved Successfully ');                        
                        //alert("Saved Successfully");
                        //location.reload();
                        //GetPreferencesList();
                    }
                }
            });
        }


        // ====================== Load Industries ====================================

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
                    if (MasterID != null) {
                        $("#<%=lnkSendRequest3.ClientID %>").html('Update <i class="fa fa-database" aria-hidden="true"></i>');
                    }
                    else {
                        $("#<%=lnkSendRequest3.ClientID %>").html('Send Request <i class="fa fa-floppy-o" aria-hidden="true"></i>');
                    }
                    getIndustryFromid(i.item.val);
                },
                minLength: 0
            });

            // ====================== Load Industries ====================================

            function GetIndustries() {
                $.ajax({
                    url: '<%=ResolveUrl("~/mMaterialManagement/MaterialMasterRequest.aspx/GetIndustries") %>',
                    data: "{}",
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
                            
                            GetIndustryID(MasterID);
                        }
                    },
                    error: function (response) {

                    },
                    failure: function (response) {

                    }
                });
            }

            GetIndustries();


           
        //todo
            function GetIndustryAttributes(industryID) {
                
                if (industryID != 0) {
                    var item = $.grep(IndItemList.Table, function (a) { return a.GEN_MST_Industry_ID == industryID });
                    if (item != null && item.length > 0) {
                        //$(".divIndustryData").css("display", "block");
                        $(".divGetIndustry").css("display", "block");
                        var container = document.getElementById('divIndustryContent');
                        var IndustryContent = '';
                        IndustryContent += '<div style="-webkit-column-count: 3;-moz-column-count: 3;column-count: 3;">';
                        //var m = 0;
                        for (var i = 0 ; i < item.length; i++) {
                            if (item[i].UIControlType == "DatePicker") {
                                IndustryContent += '<div class="col-md-4"><label class="lblFormItem">' + item[i].UILabelText + ' :</label><br /><input type="text" class="txt_Blue_Small DueDate IndustryfieldToGet" id="' + item[i].MM_MST_Attribute_ID + '" readonly="true"/></div>';
                                // m = m + 1;
                            }
                            else if (item[i].UIControlType == "DropdownList") {
                                IndustryContent += '<div class="col-md-4"><label class="lblFormItem">' + item[i].UILabelText + ' :</label><br /><select style="width:214px !important" class="txt_Blue_Small IndustryfieldToGet" id="' + item[i].MM_MST_Attribute_ID + '" onchange="getchildattributelist(this);"></select></div>';
                                //m = m + 1;
                            }
                            else if (item[i].UIControlType == "TextBox") {
                                IndustryContent += '<div class="col-md-4"><label class="lblFormItem">' + item[i].UILabelText + ' :</label><br /><input type="text" class="txt_Blue_Small IndustryfieldToGet" id="' + item[i].MM_MST_Attribute_ID + '" onclick="checkNegativeValue(this);"/></div>';
                                //todo



                                // m = m + 1;
                            }
                            //if (i == 2) {
                            //    IndustryContent += '<br /><br /><br /><br />';
                            //}
                        }

                        IndustryContent += '</div>';

                        container.innerHTML = IndustryContent;
                        // getIndustryData();
                        for (var i = 0 ; i < item.length; i++) {
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
                    for (var x = 0; x < dt.length ; x++) {
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

          function GetIndustryID(MID)
            {
                
                if (MasterID != null)
                {
                    $(".divIndustryData").css("display", "block");
                    $("#divMaterialActions").css("display", "none");
                    if (MasterID != null) {
                        $("#<%=lnkSendRequest3.ClientID %>").html('Update <i class="fa fa-database" aria-hidden="true"></i>');
                    }
                    else {
                        $("#<%=lnkSendRequest3.ClientID %>").html('Send Request <i class="fa fa-floppy-o" aria-hidden="true"></i>');
                    }

                    var ind = $("#<%=hdnIndustry.ClientID %>").val();
                    getIndustryFromid(ind);
                }
            }
       

      

            function getIndustryFromid(id) {
               
                
                GetIndustryAttributes(id);
                var item = $.grep(IndItemList.Table3, function (a) { return a.GEN_MST_Industry_ID == id });
                GetAttributes(item);
            }

            function GetAttributes(obj) {
                BindGetINDAttributes(obj);
            }
            var LookupData = null;
            function BindGetINDAttributes(obj) {
                //KeyText, KeyValue
                var dt = $.grep(obj, function (a) { return a.MM_MST_Material_ID == MasterID });
                LookupData = dt;
                if (dt != null && dt != '') {
                    for (var x = 0; x < dt.length ; x++) {
                        if (dt[x].KeyValue == 0) {
                            $("#" + dt[x].MM_MST_Attribute_ID).val(dt[x].AttributeValue);
                        }
                        else {
                            var Indobj = $.grep(IndItemList.Table3, function (a) { return a.MM_MST_Attribute_ID == dt[x].MM_MST_Attribute_ID && a.KeyValue == dt[x].KeyValue });
                            if (Indobj[0].LookupFilterValue == null) {
                                $("#" + dt[x].MM_MST_Attribute_ID).val(dt[x].KeyValue);
                            }
                            else {
                                BinLookupData(Indobj);
                            }
                        }
                    }
                }
            }

            function BinLookupData(obj) {
                var dt = $.grep(IndItemList.Table3, function (a) { return a.LookupFilterValue == obj[0].LookupFilterValue });
                if (dt != null && dt != '') {
                    for (var x = 0; x < dt.length ; x++) {
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

           
            function getchildattributelist(filterValue)
            {
                if (filterValue != null)
                {
                    var attributeid = filterValue.id;
                    if (filterValue.value != 0)
                    {
                        var attrlist = $.grep(IndItemList.Table1, function (a) { return a.LookupFilterValue == filterValue.value });
                        if (attrlist != null && attrlist.length > 0)
                        {
                            BindInvDropdowns(attrlist, attrlist[0].MM_MST_Attribute_ID);
                        }
                        else
                        {
                            var filetrlist = $.grep(IndItemList.Table2, function (a) { return a.MM_MST_DependsOnAttribute_ID == attributeid });
                            EmptyChildAttributes(filetrlist, attributeid);
                        }
                    }
                    else
                    {
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
          

        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //function EndRequestHandler(sender, args) {
        //    if (args.get_error() == undefined) {
        //        fnLoadBasicMMAutocompletes();
                function getIndustryData() {

                   // alert($('.IndustryfieldToGet').length);
                    
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
                    });

                    AttrfieldData = AttrfieldData + '</root>';
                    AttrfieldDataOut = AttrfieldData;
                    // AttrfieldDataOut += '"' + String.fromCharCode(64) + 'AinputDataXml' + '":"' + AttrfieldData + '",' + '"' + String.fromCharCode(64) + 'LanguageType' + '":"' + 'en' + '",' + '"' + String.fromCharCode(64) + 'MM_MST_Material_ID' + '":"' + MasterID + '",' + '"' + String.fromCharCode(64) + 'LoggedUserID' + '":"' + <%=cp.UserID.ToString()%> + '",';
                    // AttrfieldDataOut = AttrfieldDataOut.substring(0, AttrfieldDataOut.length - 1);
                    //AttrfieldDataOut += '}';
                    return AttrfieldDataOut;
                }


                function UpsertIndustries() {
                    
                   // MasterID = Result;
                    if (MasterID != null) {
                        MasterID = MasterID;
                    }
                    else {
                        MasterID = 0;
                    }

                    //var data = GetPrefernceFromData();

                    var obj = {};
                    obj.UserID = "<%=cp.UserID.ToString()%>";
                    obj.Inxml = getIndustryData();
                    obj.MM_MST_Material_ID = MasterID;
                    $.ajax({
                        url: "MaterialMasterRequest.aspx/SETIndustries",

                        dataType: 'json',
                        contentType: "application/json",
                        type: 'POST',
                        data: JSON.stringify(obj),
                        success: function (response) {
                            if (response.d == "success") {
                                //alert("Saved Successfully");
                                //location.reload();
                                //GetPreferencesList();
                            }
                        }
                    });
                }
            //}
       // }
        
    </script>
</asp:Content>

