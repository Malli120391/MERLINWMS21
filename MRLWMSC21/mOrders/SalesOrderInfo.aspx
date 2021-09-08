<%@ Page Title="Sales Order:." Language="C#" MasterPageFile="~/mOrders/OrdersMaster.master" AutoEventWireup="true" CodeBehind="SalesOrderInfo.aspx.cs" EnableEventValidation="false" Inherits="MRLWMSC21.mOrders.SalesOrderInfo" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="OrdersContent" runat="server">
    <script type="text/javascript" src="Scripts/CommonScripts.js"></script>
      <script src="../mInventory/Scripts/bootstrap.min.js"></script>
    <link href="../mInventory/Scripts/bootstrap.min.css" rel="stylesheet" />
    <asp:ScriptManager runat="server" EnablePartialRendering="true" ID="ss" SupportsPartialRendering="true"></asp:ScriptManager>
    <script>
        $(document).ready(function () {

            CustomAccordino($('#dvWOHeader'), $('#dvWOBody'));
            CustomAccordino($('#dvSADHeader'), $('#dvSADBody'));
            CustomAccordino($('#dvCPODHeader'), $('#dvCPODBody'));
            CustomAccordino($('#dvMDHeader'), $('#dvMDBody'));
        });

    </script>

    <style>
        .ButnEmpty{

            padding:1px !important;
            /*width : 50px !important;*/
        }
    </style>
    <script>
        function SuccessMsg(soid, tid) {
            setTimeout(function () {
                location.reload();
                window.location.href = "SalesOrderInfo.aspx?soid=" + soid + "&tid=" + tid + "";
            }, 1000);
            //showStickyToast(false, "SO Canceled Successfully", true);
            //return false;  
        }

        function minmax(value, min, max) {
            debugger
                if (value == "")
                return value;
                else if (parseInt(value) < min || isNaN(parseInt(value)))
                    return min;
                else if (parseInt(value) > max)
                    return max;
                else return value;
            
        }
    </script>





    <script type="text/javascript">
        $(function () {
            $('.accordino_icon_Right').on('click', function () {
                $(this).children('.downarrow ').toggleClass('isRotateright');
            });
        });
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                javaScriptfunction();
            }
        }
        javaScriptfunction();
        function showConfirmAlert() {
            debugger;
            var $confirm = $("#modalConfirmYesNo");
            $confirm.modal('show');
            $("#lblTitleConfirmYesNo").html("<i class='fa fa-exclamation-circle' aria-hidden='true'></i> Confirmation");
            $("#lblMsgConfirmYesNo").html("<div class='row col-md-12'>Are you sure do you want to cancel?</div>");

            $("#btnYesConfirmYesNo").on('click').click(function () {
                CancelSO();
            });
            $("#btnNoConfirmYesNo").on('click').click(function () {
                $confirm.modal("hide");
            });
        }
        //function validateCurrency(amount) {
        //    var regex = /^[1-9]\d*(?:\.\d{0,2})?$/;
        //    return regex.test(amount);
        //}
        //function validCheck(e) {
        //    var keyCode = (e.which) ? e.which : e.keyCode;
        //    if ((keyCode >= 48 && keyCode <= 57) || (keyCode == 8))
        //        return true;
        //    else if (keyCode == 46) {
        //        var curVal = document.activeElement.value;
        //        if (curVal != null && curVal.trim().indexOf('.') == -1)
        //            return true;
        //        else
        //            return false;
        //    }
        //    else
        //        return false;
        //}

        //--------------------------------Added By meena-----------
        //For money validation
        function validateFloatKeyPress(el) {
            var v = parseFloat(el.value);
            el.value = (isNaN(v)) ? '' : v.toFixed(2);
        }
        //For Number validation
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }

        function lnkClear() {
            $("#<%= this.txtSOType.ClientID %>").val("");
            $("#<%= this.txtCustomname.ClientID %>").val("");          
            $("#<%= this.txtSoDate.ClientID %>").val("");
            $("#<%= this.txtTenant.ClientID %>").val("");
            $("#<%= this.txtDeliverydate.ClientID %>").val("");
            $("#<%= this.txtProjectCode.ClientID %>").val("");
            $("#<%= this.txtRequirementNumber.ClientID %>").val("");
            $("#<%= this.txtFreightCompany.ClientID %>").val("");
            $("#<%= this.txtShipmentCharges.ClientID %>").val("");
            $("#<%= this.txtGrossValue.ClientID %>").val("");
            $("#<%= this.txtNetValue.ClientID %>").val("");
            $("#<%= this.txtlCurrency.ClientID %>").val("");
            $("#<%= this.txtSOTax.ClientID %>").val("");
            $("#<%= this.txtDiscount.ClientID %>").val("");
            
        }
        //--------------------------------
        function CancelSO() {
            var $confirm = $("#modalConfirmYesNo");
            $.ajax({
                url: '<%=ResolveUrl("~/mOrders/SalesOrderInfo.aspx/CancelSO") %>',
                data: "{}",
                dataType: "json",
                type: "POST",
                async: true,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    debugger;
                    var poid = data.d;
                    if (poid == "-1") {
                        showStickyToast(false, "Cannot cancel, as the Sales order is configured in Outbound", false);
                        return false;
                    }
                    else if (poid == "1") {
                        showStickyToast(false, "Error while canceling", false);
                        return false;
                    }
                    else if (poid == "2") {
                     
                        $confirm.modal("hide");
                       debugger;
                        setTimeout(function () { 
                            location.reload();                                                 
                         window.location.href = "SalesOrderInfo.aspx?soid=" + soid + "&tid=" + tid + "";                          
                          
                        }, 400);
                        showStickyToast(false, "SO Canceled Successfully", true);  
                        return false;
                       
                        //
                       
                    }
                    else {
                      //  location.reload();
                    }
                }
                //,
                //error: function (response) {
                //    alert(response.text);
                //},
                //failure: function (response) {
                //    alert(response.text);
                //}
            });
        }
        function lnkDeleteItem_ClientClick() {
       
            debugger;
            var count = $(".chkDelete").length;
            // alert(count);
            if (count > 0) {

                return true;
            }
            else {
                showStickyToast(false, "Please Select Items", false);
                return false;
            }
        }
        function lnkDeleteItem1_ClientClick() {

            debugger;
            var count = $(".chkDelete1").length;
            // alert(count);
            if (count > 0) {

                return true;
            }
            else {
                showStickyToast(false, "Please Select Items", false);
                return false;
            }
        }
        function javaScriptfunction() {

            $(document).ready(function () {

                $("#txtmfgdate").datepicker({
                    dateFormat: "dd-M-yy",
                    maxDate: new Date(),
                    changeMonth: true,
                    changeYear: true,
                    onSelect: function (selected) {
                        var instance = $(this).data("datepicker");
                        var date = $.datepicker.parseDate(instance.settings.dateFormat || $.datepicker._defaults.dateFormat, selected, instance.settings);
                        date.setDate(date.getDate() + 1);
                        $("#txtexpdate").datepicker("option", "minDate", date, { dateFormat: "dd-M-yy" })
                        //$("#txtexpdate").datepicker("option", "minDate", selected, { dateFormat: "dd/mm/yy" })
                    }
                });

                $("#txtexpdate").datepicker({
                    dateFormat: "dd-M-yy",
                    changeMonth: true,
                    changeYear: true,
                    //maxDate: new Date()
                });

                //Added by kashyap on 21/08/2017  to reslove the server issue 
                $('#txtmfgdate, #txtexpdate').keypress(function () {
                    return false;
                });

                var textfieldname = $("#<%= this.txtSOType.ClientID %>");
                DropdownFunction(textfieldname);
                $("#<%= this.txtSOType.ClientID %>").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadSOTypes") %>',
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
                        $("#<%=hifSOType.ClientID %>").val(i.item.val);
                    },
                    minLength: 0
                });

                var textfieldname = $("#<%= this.txtCustomname.ClientID %>");
                DropdownFunction(textfieldname);
                $("#<%= this.txtCustomname.ClientID %>").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadCustomerNames") %>',
                            data: "{ 'prefix': '" + request.term + "','TenantID':'" + $('#<%=this.hifTenant.ClientID%>').val() + "'}",//<= cp.TenantID %>
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                if (data.d == '')
                                    alert('No customer is available');

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

                        $("#<%=hifCustomerName.ClientID %>").val(i.item.val);



                        getCustomerDetails(i.item.val);

<%--                        var btnID = '<%=lnkGetCustomerDetails.ClientID %>';                     
                        var params = 'customerDetails';
                        __doPostBack(btnID, params);--%>



                           //ddddd("<= this.txtCustomname.ClientID %>");
                        //BuildUserAddress();
                        //return true;
                    },
                    minLength: 0
                });


                function getCustomerDetails(customerID) {
                    debugger;
                    //alert();
                    $.ajax({
                        url: 'SalesOrderInfo.aspx/getCustomerData',
                        data: "{'CustomerID':'" + customerID + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            debugger;
                            if (JSON.parse(data.d).Table.length > 0) {
                                var dt = JSON.parse(data.d).Table[0];
                                $("#txtBCustName").val(dt.CustomerName);
                                $("#txtBAddress1").val(dt.AddressLine1);
                                $("#txtBAddress2").val(dt.AddressLine2);
                                $("#txtBCity").val(dt.CityName);
                                $("#txtBState").val(dt.StateName);
                                $("#txtBCountyMaster").val(dt.CountryName);
                                $("#hifCountryMaster").val(dt.CountryMasterID);
                                $("#txtBZip").val(dt.ZipCode);
                                $("#txtBPhoneNo").val(dt.Phone1);
                            }
                            else {
                                $("#txtBCustName").val("");
                                $("#txtBAddress1").val("");
                                $("#txtBAddress2").val("");
                                $("#txtBCity").val("");
                                $("#txtBState").val("");
                                $("#txtBCountyMaster").val("");
                                $("#hifCountryMaster").val("0");
                                $("#txtBZip").val("");
                                $("#txtBPhoneNo").val("");
                            }
                            //CountryMasterID
                        }
                    });
                }



                 //======================== New Code Added by Meena ======================//
                $("#<%=this.txtDeliverydate.ClientID%>").datepicker({
                    dateFormat: "dd-M-yy",
                    minDate: 0,
                    changeMonth: true,
                    changeYear: true,

                });
                //$('.hasDatepicker').keydown(function () {
                //    return false;
                //});

                $('.hasDatepicker').prop('readonly', true);


                //For addresstype

                var textfieldname1 = $("#<%= this.txtaddresstype.ClientID %>");
                DropdownFunction(textfieldname1);
                $("#<%= this.txtaddresstype.ClientID %>").autocomplete({
                    source: function (request, response) {
                        debugger;
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadAddressTypes") %>',
                            data: "{ 'prefix': '" + request.term + "','CustomerID':'" + $('#<%=this.hifCustomerName.ClientID%>').val() + "'}",//<= cp.TenantID %>
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                if (data.d == '')
                                    showStickyToast(false, "No Address Type is available for this customer ");
                                    //alert('No addresstypes is available for this customer');

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
                        $("#<%=hdnaddresstypeid.ClientID %>").val(i.item.val);
                       
                        var btnID = '<%=lnkGetCustomerDetails.ClientID %>';
                        //ddddd("<= this.txtCustomname.ClientID %>");
                        var params = 'customerDetails';
                        __doPostBack(btnID, params);

                    },
                    minLength: 0
                });
                 //========================End ======================//

                var textfieldname = $("#<%= this.txtFreightCompany.ClientID %>");
                DropdownFunction(textfieldname);
                $("#<%= this.txtFreightCompany.ClientID %>").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadFreightCompanies") %>',
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
                        $("#<%=hifFreightCompany.ClientID %>").val(i.item.val);
                    },
                    minLength: 0
                });

                var textfieldname = $("#<%= this.txtlCurrency.ClientID %>");
                DropdownFunction(textfieldname);
                $("#<%= this.txtlCurrency.ClientID %>").autocomplete({
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
                        $("#<%=hifCurrency.ClientID %>").val(i.item.val);
                       

                    },
                    minLength: 0
                });


                 var textfieldname = $("#<%= this.txtBCountyMaster.ClientID %>");
                DropdownFunction(textfieldname);
                $("#<%= this.txtBCountyMaster.ClientID %>").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadCountries") %>',
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
                        $("#<%=hifCountryMaster.ClientID %>").val(i.item.val);
                       

                    },
                    minLength: 0
                });



               // var textfieldname = <%--$("#<%= this.txtCountyMaster.ClientID %>");--%>
                //DropdownFunction(textfieldname);
                <%--$("#<%= this.txtCountyMaster.ClientID %>").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadCountries") %>',
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
                        $("#<%=hifCountryMaster.ClientID %>").val(i.item.val);
                    },
                    minLength: 0
                });--%>
          
                $("#<%= this.txtlCurrency.ClientID %>").keypress(function (event) {
                    var inputValue = event.charCode;
                    //alert(inputValue);
                    if (!((inputValue > 64 && inputValue < 91) || (inputValue > 96 && inputValue < 123) || (inputValue == 32) || (inputValue == 0))) {
                        event.preventDefault();
                    }
                });
                $(".POCurrrencyPicker").keypress(function (event) {
                    var inputValue = event.charCode;
                    //alert(inputValue);
                    if (!((inputValue > 64 && inputValue < 91) || (inputValue > 96 && inputValue < 123) || (inputValue == 32) || (inputValue == 0))) {
                        event.preventDefault();
                    }
                });
                
                
                $("#<%= this.txtSoDate.ClientID %>").datepicker({
                    dateFormat: "dd-M-yy", changeMonth: true,
                    changeYear: true, });
              <%--  $('#<%= this.txtSoDate.ClientID %>').keydown(function () {
                    return false;
                });--%>
                $("#txtCustPODate").datepicker({
                    dateFormat: "dd-M-yy",
                    changeMonth: true,
                    changeYear: true, });
                $('.hasDatepicker').attr('readonly', 'true');
                //$("#my_txtbox").attr( 'readOnly' , 'true' );
                CheckCustomPO();

                //BuildUserAddress();
                //MSPConfifure(1);
            });
        }

    </script>

    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                fnMCodeAC();
            }
        }


        function fnMCodeAC() {
            $(document).ready(function () {
                debugger;
                var tId=0;
                var soid=0;
                var url = window.location.pathname;
                if (window.location.href.indexOf("soid") >-1) {
                    var obj = location.href.split('?')[1].split('&');
                    if (obj.length > 0) {
                        var soid = obj[0].split('=')[1];
                        var tid = obj[1].split('=')[1];
                    }
                }
                if (location.href.split('?')[1] >0) {
                    tId = url.searchParams.get("tid");
                soid = url.searchParams.get("soid"); 
                }
                $('.DateBoxCSS_small').datepicker({
                    dateFormat: 'dd-M-yy',
                    changeMonth: true,
                    changeYear: true, });
                //$('.DateBoxCSS_small').keydown(function () {
                //    return false;
                //});


                try {

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

                                $("#<%=hifTenant.ClientID %>").val(i.item.val);
                                $("#<%=txtCustomname.ClientID %>").val('');
                            },
                            minLength: 0
                        });

                        var textfieldname = $('#txtSearch');
                        DropdownFunction(textfieldname);
                        $('#txtSearch').autocomplete({

                            source: function (request, response) {

                                $.ajax({
                                    url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadMCodeForSaleOrderWithOEM") %>',
                                    data: "{ 'prefix': '" + request.term + "','TenantID':'" +<%=cp.TenantID%> +"','SOheaderID':'<%=ViewState["HeaderID"]%>'}",
                                    dataType: "json",
                                    type: "POST",
                                    async: true,
                                    contentType: "application/json; charset=utf-8",
                                    success: function (data) {
                                        response($.map(data.d, function (item) {
                                            return {

                                                label: item.split('~')[0].split('`')[0],
                                                description: item.split('~')[0].split('`')[1] == undefined ? "" : " <font color='#086A87'  >" + item.split('~')[0].split('`')[1] + "</font>",
                                                val: item.split('~')[1]
                                            }
                                        }))
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

                } catch (ex) { }


                var textfieldname = $('.POCurrrencyPicker');
                DropdownFunction(textfieldname);
                $('.POCurrrencyPicker').autocomplete({
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

                            $("#hifPOCurrency").val(i.item.val);
                        },
                        minLength: 0
                    });

                    var textfieldname = $('.CountrofOriginPicker');
                    DropdownFunction(textfieldname);
                    $('.CountrofOriginPicker').autocomplete({
                        source: function (request, response) {
                            $.ajax({
                                url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadCountries") %>',
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
                            $("#hifCountrofOriginID").val(i.item.val);
                        },
                        minLength: 0
                    });

                    try {
                        var textfieldname = $('.MCodePicker');
                        DropdownFunction(textfieldname);
                        $('.MCodePicker').autocomplete({
                            source: function (request, response) {
                                $.ajax({
                                    url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadTenantMDescForSaleOrder") %>',
                                    data: "{ 'prefix': '" + request.term + "','TenantID':'" + $('#<%=hifTenant.ClientID%>').val() + "'}",
                                    dataType: "json",
                                    type: "POST",
                                    async: true,
                                    contentType: "application/json; charset=utf-8",
                                    success: function (data) {
                                        if (data.d == '')
                                            alert('No part number is available');
                                        response($.map(data.d, function (item) {
                                            //alert();
                                            debugger
                                            return {

                                                label: item.split('~')[0],
                                                val: item.split('~')[1],
                                                text: item.split('~')[2]
                                                //label: item.split('~')[0].split('`')[0],
                                                //description: item.split('~')[0].split('`')[1] == undefined ? "" : " <font color='#086A87'  >" + item.split('~')[0].split('`')[1] + "</font>",
                                                //val: item.split('~')[1]
                                            }
                                        }))
                                    }

                                });
                            },
                            select: function (e, i) {
                                debugger
                                $("#hifmmid").val(i.item.val);

                                $("#atcMCode").val(i.item.text);

                                document.getElementById("atcSUoMID").value = "";


                                document.getElementById("atcKitPlanner").value = "";


                                document.getElementById("atcCustPOUoM").value = "";
                                //kitPlannerToolTip();

                            },
                            minLength: 0
                            //}).data("autocomplete")._renderItem = function (ul, item) {
                            //    return $("<li></li>")
                            //        .data("item.autocomplete", item)
                            //        .append("<a>" + item.label + "" + item.description + "</a>")
                            //        .appendTo(ul)
                            //};
                        });
                    }
                    catch (err) { }

                    var textfieldname = $('.SUoMPicker');
                    DropdownFunction(textfieldname);
                    $('.SUoMPicker').autocomplete({
                        source: function (request, response) {
                            $.ajax({
                                url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadUoMWithQty") %>',
                                data: "{ 'MaterialID': '" + +document.getElementById("hifmmid").value + "'}",
                                dataType: "json",
                                type: "POST",
                                async: true,
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    if (data.d == "" || data.d == "/") {
                                        showStickyToast(false, 'No UoM\'s are configured to this Part Number');

                                        document.getElementById("atcPUoM").value = "";

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

                            $("#hifUoMid").val(i.item.val);
                            $("#hidUoMQty").val(i.item.label.split('/')[1]);
                        },
                        minLength: 0
                    });

                    var textfieldname = $('.custUoMPicker');
                    DropdownFunction(textfieldname);
                    $('.custUoMPicker').autocomplete({
                        source: function (request, response) {
                            $.ajax({
                                url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadUoMWithQty") %>',
                                data: "{ 'MaterialID': '" + +document.getElementById("hifmmid").value + "'}",
                                dataType: "json",
                                type: "POST",
                                async: true,
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    if (data.d == "" || data.d == "/") {
                                        showStickyToast(false, 'No UoM\'s are configured to this Part Number');
                                        document.getElementById("atcInvUoM").value = "";
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

                            $("#hifcustPoUoM").val(i.item.val);
                        },
                        minLength: 0
                    });

                    var textfieldname = $('#atcCustPONumber');
                    DropdownFunction(textfieldname);
                    $('#atcCustPONumber').autocomplete({
                        source: function (request, response) {
                            $.ajax({
                                url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadCustomPONumbers") %>',
                                data: "{ 'SOHeaderID': '" +<%=ViewState["HeaderID"].ToString()%> + "'}",
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

                            $("#hifCustomPOId").val(i.item.val);
                        },
                        minLength: 0
                    });

                    var textfieldname = $('.KitPlannerPicker');
                    DropdownFunction(textfieldname);
                    $('.KitPlannerPicker').autocomplete({
                        source: function (request, response) {
                            $.ajax({
                                url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadKitPlanner") %>',
                                data: "{ 'MaterialID': '" + +document.getElementById("hifmmid").value + "','TenantID':'" + document.getElementById('<%=this.hifTenant.ClientID%>').value + "'}",
                                dataType: "json",
                                type: "POST",
                                async: true,
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    if (data.d == "") {
                                        showStickyToast(false, 'No Kit\'s are configured to this Part Number');

                                        document.getElementById("atcKitPlanner").value = "";
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
                            $("#hifKitPlanner").val(i.item.val);

                        },
                        minLength: 0
                    });

            });


        }
        fnMCodeAC();
    </script>

    <script>

            function CheckSUoMQty(TextBox) {
                /* var UoMQty = parseFloat(document.getElementById('hidUoMQty').value) * 100;
                var RequireQty = parseFloat(TextBox.value) * 100;
                if (RequireQty % UoMQty != 0) {
                    showStickyToast(true, "WO Qty. should be multiple of WO UoMQty.");
                    TextBox.value = "";
                    return;
                }*/
                CheckDecimal(TextBox);
            }

            function CheckIsDelted(checkBox) {
                if (checkBox.checked) {
                    alert('Are you sure want to delete the Sales Order?');
                }
            }

            function ClearText(TextBox) {
                if (TextBox.value == "Search Part Number...") {
                    TextBox.value = "";
                    TextBox.style.color = "#000000";
                }
            }
            function focuslost(TextBox) {
                if (TextBox.value == "") {
                    TextBox.value = "Search Part Number...";
                    TextBox.style.color = "#A4A4A4";
                }
            }

            function CheckPONumber(textbox) {

                $.ajax({
                    url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadCustomPONumbers") %>',
                    data: "{ 'SOHeaderID': '" +<%=ViewState["HeaderID"].ToString()%> + "'}",
                    dataType: "json",
                    type: "POST",
                    async: true,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {

                        var numbers = data.d.toString();

                        var PONumber = numbers.split(',');

                        var check = false;
                        if (textbox.value != "") {

                            for (n = 0; n < PONumber.length; n = n + 2) {

                                if (PONumber[n] == textbox.value) {
                                    check = true;
                                    break;
                                }
                            }
                        }
                        else
                            check = true;

                        if (check == false) {
                            textbox.value = "";
                            textbox.focus();
                        }

                    }

                });
            }



            function MSPConfifure(name) {
                var mmid = 0;
                try {

                    mmid = document.getElementById("hifmmid").value;
                    if (mmid == '' || mmid == null)
                        mmid = 0;
                } catch (err) {
                }
                //$(mmid);
                // alert(document.getElementById("hifmmid").value);

                $.ajax({
                    url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/MaterialConfigurationService") %>',
                data: "{ 'MaterialId': '" + mmid + "','TenantID':'" +<%=hifTenant.Value%> +"'}",
                dataType: "json",
                type: "POST",
                async: true,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    //alert(data);
                    configure(name, data.d);
                    //response(data.d);
                }
            });
            //alert('dsdsdsdsdsds');
            $.ajax({
                url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadKitPlanner") %>',
                data: "{ 'MaterialID': '" + mmid + "','TenantID':'" +<%=hifTenant.Value%> +"'}",
                dataType: "json",
                type: "POST",
                async: true,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.d != "") {
                        var kitImage = document.getElementById("imgkit");
                        kitImage.style.display = "block";
                        var ToolTip = 'configured kitIDs to this Part Number\n\n' + data.d;
                        kitImage.setAttribute("title", ToolTip);
                        return;
                    }
                    else {
                        var kitImage = document.getElementById("imgkit");
                        //alert(kitImage);
                        kitImage.style.display = "none";
                        // kitImage.style.display = "block";
                        // alert(data.d.split(',')[0]);
                        // var ToolTip = 'configure kitIDs to this Material\n\n kit Material IDs are ' + data.d;
                        // kitImage.setAttribute("title", ToolTip);
                        return;
                        //kitImage.setAttribute("title", "Live Chat is currently ONLINE");
                        //alert('aaaaaa');
                    }
                }
            });

            }
            function configure(textbox, text) {
                //alert(text);
                var paramNames = text.split('|');
                var listOfparames = paramNames[0].split(',');

                try {
                    for (var item = 0; item < listOfparames.length; item++) {
                        // alert(listOfparames[item]);

                        document.getElementById(listOfparames[item]).style.display = "none";


                    }
                    listOfparames = paramNames[1].split(',');

                    for (var item = 0; item < listOfparames.length; item++) {


                        document.getElementById(listOfparames[item]).style.display = "block";

                    }
                } catch (err) {
                }
            }

          

        //function BuildUserAddress() {
        //   // 

        //         $.ajax({
        //             url: '<=ResolveUrl("~/mWebServices/FalconWebService.asmx/CustomerDetails") %>',
        //             data: "{ 'CustomerID': '" +document.getElementById ("<=this.hifCustomerName.ClientID%>").value + "','TenantID':'" +<cp.TenantID%> +"'}",
        //              dataType: "json",
        //              type: "POST",
        //              async: true,
        //              contentType: "application/json; charset=utf-8",
        //              success: function (data) {
        //                 // alert(data.d);

        //                  document.getElementById("<=this.txtCustomerAddress.ClientID%>").value = data.d;
        //                  //response(data.d);
        //              }
        //          });
        //     }

         

    </script>

    <script>
        function CheckCustomPO() {
            return;
            //alert('ssssss');
            $.ajax({
                url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/CheckCustomPO") %>',
                data: "{ 'SOHeaderID': '" +<%=ViewState["HeaderID"].ToString()%> + "'}",
                dataType: "json",
                type: "POST",
                async: true,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    try {
                        if (data.d == "false") {

                            document.getElementById("atcCustPONumber").style.display = "none";
                            document.getElementById("atcCustPOUoM").style.display = "none";
                            document.getElementById("txtCustPOQuantity").style.display = "none";
                            document.getElementById("txtSODiscountInPercentage").style.display = "none";
                            document.getElementById("txtVATCode").style.display = "none";
                        }
                        else {
                            document.getElementById("atcCustPONumber").style.display = "block";
                            document.getElementById("atcCustPOUoM").style.display = "block";
                            document.getElementById("txtCustPOQuantity").style.display = "block";
                            document.getElementById("txtSODiscountInPercentage").style.display = "block";
                            document.getElementById("txtVATCode").style.display = "block";
                        }
                    } catch (err) {
                    }
                }
            });
        }
        
    </script>

    <asp:UpdatePanel ID="upnwoContent" runat="server">

        <ContentTemplate>
       

             <!-- Globalization Tag is added for multilingual  -->
            <div class="container">
                  <div flex between>
                       <inv-status><asp:Label ID="ltPOStatus" runat="server" CssClass="BigCapsHeading"  /></inv-status>
                      <div><asp:LinkButton ID="lnkback" runat="server" CssClass="btn btn-primary backtolist" SkinID="lnkButEmpty" PostBackUrl="~/mOrders/SOList.aspx"><i class="material-icons vl">arrow_back</i><%= GetGlobalResourceObject("Resource", "BacktoList")%> </asp:LinkButton></div></div>
                <gap5></gap5>
            <div>


                <div class="">
                    <div>
                         <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="dvWOHeader" style="">
                            <%= GetGlobalResourceObject("Resource", "OutwardOrderHeader")%>
                            <i class="material-icons downarrow right">keyboard_arrow_down</i>
                        </div>

                        <div class="ui-Customaccordion" id="dvWOBody">
                         
                           <gap></gap>
                            <div>
                                <div class="p10">
                                    <div class="row">
                                        <div class="col m3 s3">
                                            <div class="flex">
                                               
                                                <div class="flex">
                                                    <asp:TextBox ID="txtSOCode" runat="server" Enabled="false" />
                                                    <asp:ImageButton runat="server" ID="IbtnNew" OnClick="IbtnNew_Click" ImageUrl="../Images/blue_menu_icons/add_new.png" ToolTip="Generate New SO Number" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="savecustomerdetails" Enabled="false" CssClass="errorMsg" ControlToValidate="txtSOCode" Display="Dynamic" ErrorMessage=" * " />

                                                    <label>
                                                        <%= GetGlobalResourceObject("Resource", "OutwardRefNo")%><asp:Literal ID="ltSOCode" runat="server" />

                                                        <asp:Literal ID="ltSoID" Visible="false" runat="server"></asp:Literal>
                                                    </label>
                                                </div>

                                            </div>
                                        </div>
                                        <div class="col m3 s3">
                                            <div class="">
                                                
                                                <div class=" flex">
                                                    <asp:TextBox ID="txtSOType" runat="server" SkinID="txt_Auto" required=""></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" ValidationGroup="savecustomerdetails" runat="server" CssClass="errorMsg" ControlToValidate="txtSOType" Display="Dynamic" />
                                                    <label><span style="color: red; margin-left: -0.3em"></span><%= GetGlobalResourceObject("Resource", "OutwardType")%><asp:Literal ID="Literal7" runat="server" /></label>
                                                    <asp:HiddenField runat="server" ID="hifSOType" />
                                                </div>

                                            </div>
                                        </div>
                                        <div class="col m3 s3">
                                            <div class="flex">
                                                <div class="">
                                                    <asp:TextBox ID="txtSoDate" runat="server" required=""></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator16" ValidationGroup="savecustomerdetails" runat="server" CssClass="errorMsg" ControlToValidate="txtSoDate" Display="Dynamic" />

                                                    <label><span style="color: red; margin-left: -0.3em"></span><%= GetGlobalResourceObject("Resource", "OutwardDate")%><asp:Literal ID="Literal9" runat="server" /></label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col m3 s3">
                                            <div class="flex">
                                                <div class="">
                                                    <asp:TextBox runat="server" ID="txtTenant" SkinID="txt_Auto" required=""></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvTenant" ValidationGroup="savecustomerdetails" runat="server" CssClass="errorMsg" ControlToValidate="txtTenant" Display="Dynamic" />
                                                    <label><span style="color: red; margin-left: -0.3em"></span><%= GetGlobalResourceObject("Resource", "Tenant")%><asp:Literal ID="ltTenant" runat="server" /></label>
                                                    <asp:HiddenField ID="hifTenant" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col m3 s3">
                                            <div class="flex">
                                                <div class="">
                                                    <asp:TextBox runat="server" ID="txtCustomname" SkinID="txt_Auto" required=""></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvCustomerName" ValidationGroup="savecustomerdetails" runat="server" CssClass="errorMsg" ControlToValidate="txtCustomname" Display="Dynamic" />

                                                    <label>
                                                        <span style="color: red; margin-left: -0.3em"></span><%= GetGlobalResourceObject("Resource", "Customer")%><asp:Literal ID="lclCustomerName" runat="server" />

                                                        <asp:HiddenField ID="hifCustomerName" runat="server" Value="0" />
                                                </div>
                                            </div>
                                            <div style="display: none">
                                                <asp:LinkButton ID="lnkGetCustomerDetails" runat="server" OnClick="lnkGetCustomerDetails_Click" />
                                            </div>

                                        </div>
                                        <div class="col m3 s3">
                                            <div class="flex">
                                                <div class="f-flex">
                                                    </label>
                                                </div>
                                                <div class="f-flex form">
                                                    <asp:TextBox ID="txtProjectCode" runat="server" onKeyPress=" return checkSpecialChar(event)" MaxLength="30" required="" />

                                                    <label>
                                                        <%= GetGlobalResourceObject("Resource", "ProjectCode")%><asp:Literal ID="ltProjectCode" runat="server" />
                                                </div>
                                            </div>
                                        </div>


                                        <div class="col m3 s3">
                                            <div class="flex">
                                                <div class="f-flex">
                                                </div>
                                                <div class="f-flex form">
                                                    <asp:TextBox ID="txtDeliverydate" runat="server" required=""></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RfvDeliveryDate" ValidationGroup="savecustomerdetails" runat="server" CssClass="errorMsg" ControlToValidate="txtDeliverydate" ErrorMessage="" />

                                                    <label><%= GetGlobalResourceObject("Resource", "DeliveryDate")%><asp:Literal ID="ltDeliverydate" runat="server" /></label>

                                                </div>
                                            </div>

                                        </div>
                                    
                                        <div class="col m3 s3" style="display:none;">
                                            <div class="flex">
                                                <div class="f-flex form">

                                                    <asp:TextBox runat="server" ID="txtaddresstype" SkinID="txt_Auto" required=""></asp:TextBox>

                                                    <label>
                                                        <asp:Literal ID="Literal1" runat="server" />
                                                        <%= GetGlobalResourceObject("Resource", "AddressType")%></label>
                                                    <asp:HiddenField ID="hdnaddresstypeid" runat="server" Value="0" />
                                                </div>
                                            </div>
                                        </div>

                                         <div class="col m3 s3">
                                            <div class="flex">
                                               
                                                <div class="">

                                                    <asp:TextBox ID="txtRemarks" runat="server" MaxLength="40" TextMode="MultiLine"  required="" />


                                                    <label>
                                                        <asp:Literal ID="ltremarks" runat="server" Text="Area Sales Manager" />
                                                        <%--<%= GetGlobalResourceObject("Resource", "Remarks")%></label>--%>

                                                </div>
                                            </div>
                                        </div>

                                        
                                        <div class="col m3 s3" >
                                            <div class="flex">
                                               
                                                <div class="">

                                                    <asp:TextBox ID="txtDiscount" runat="server" MaxLength="40" required="" />
                                                    <asp:RequiredFieldValidator ID="RFVDiscount" ValidationGroup="savecustomerdetails" runat="server" CssClass="errorMsg" ControlToValidate="txtDiscount" ErrorMessage="" />


                                                    <label>
                                                        <asp:Literal ID="ltrDiscount" runat="server" />
                                                        <%= GetGlobalResourceObject("Resource", "Discount")%></label>

                                                </div>
                                            </div>
                                        </div>

                                    </div>



                                    <div class="row">
                                       
                                        <div class="col m12 s12" style="display: none">

                                    </div>
                                        </div>
                                        <div style="display: none">
                                            <div class="flex">
                                             
                                                <div class="f-flex form">
                                                    <asp:TextBox ID="txtRequirementNumber" onKeypress="return checkSpecialChar(event)" runat="server" MaxLength="25" required=""></asp:TextBox>
                                                    <label>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" Enabled="false" ValidationGroup="savecustomerdetails" runat="server" CssClass="errorMsg" ControlToValidate="txtRequirementNumber" Display="Dynamic" ErrorMessage=" * " />

                                                        <asp:Literal ID="Literal8" runat="server" />
                                                        <%= GetGlobalResourceObject("Resource", "IssueNo")%></label>
                                                </div>
                                            </div>
                                        </div>
                                        <div style="display: none">
                                            <div class="flex">
                                                <div class="f-flex">
                                                </div>
                                                <div class="f-flex form">
                                                    <asp:TextBox ID="txtFreightCompany" runat="server" SkinID="txt_Auto" required=""></asp:TextBox>
                                                    <label>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" ValidationGroup="savecustomerdetails" runat="server" Enabled="false" CssClass="errorMsg" ControlToValidate="txtFreightCompany" Display="Dynamic" ErrorMessage=" * " />
                                                        <asp:Literal ID="lclFreightCompany" runat="server" Text="Freight Company:" /></label>
                                                    <asp:HiddenField ID="hifFreightCompany" runat="server" />
                                                </div>
                                            </div>
                                        </div>






                                    </div>


                                    <div class="row" style="display: none">
                                        <div class="col m4 s4" colspan="1" >
                                            <div class="flex">
                                                <div class="f-flex">
                                                </div>
                                                <div class="f-flex form">
                                                    <asp:TextBox ID="txtShipmentCharges" runat="server" onKeyPress=" return checkDec(this,event)" onblur="CheckDecimal(this)" MaxLength="30" required="" />
                                                    <label>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ValidationGroup="savecustomerdetails" Enabled="false" CssClass="errorMsg" ControlToValidate="txtShipmentCharges" Display="Dynamic" ErrorMessage=" * " />

                                                        <asp:Literal ID="lclShipmentCharges" runat="server" /><%= GetGlobalResourceObject("Resource", "ShipmentCharges")%></label>
                                                </div>
                                            </div>
                                        </div>


                                        <div class="col m4 s4" colspan="1" style="display: none">
                                            <div class="flex">
                                                <div class="f-flex">
                                                </div>
                                                <div class="f-flex form">
                                                    <asp:TextBox ID="txtGrossValue" runat="server" onchange="return validateFloatKeyPress(this);" onblur="CheckDecimal(this)" MaxLength="8" required="" />
                                                    <label>
                                                        <asp:Literal ID="ltGrossValue" runat="server" /><%= GetGlobalResourceObject("Resource", "GrossValue")%></label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col m4 s4" colspan="1" style="display: none">
                                            <div class="flex">
                                                <div class="f-flex">
                                                </div>
                                                <div class="f-flex form">
                                                    <asp:TextBox ID="txtNetValue" runat="server" onchange="return validateFloatKeyPress(this);" onblur="CheckDecimal(this)" MaxLength="8" required="" />
                                                    <label>
                                                        <asp:Literal ID="ltnetValue" runat="server" Text="Net Value:" /></label>
                                                </div>
                                            </div>
                                        </div>


                                        <div class="col m4 s4" colspan="2" align="right" valign="bottom" style="padding-right: 31px;">
                                            <asp:CheckBox ID="chkIsActive" runat="server" Text="Active" Visible="false" />&nbsp;&nbsp;&nbsp;
                                                    <asp:CheckBox ID="chkIsDeleted" onclick="CheckIsDelted(this);" runat="server" Text="Delete" Visible="false" />
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        </div>
                                    </div>
                                    <div class="row" style="display: none">
                                        <div style="display: none">
                                            <div class="flex">
                                                <div class="f-flex">
                                                </div>
                                                <div class="f-flex form">
                                                    <asp:TextBox ID="txtlCurrency" runat="server" SkinID="txt_Auto" required=""></asp:TextBox>
                                                    <label>
                                                        <asp:Literal ID="lclCurrency" runat="server" />
                                                        <%= GetGlobalResourceObject("Resource", "Currency")%></label>
                                                    <asp:HiddenField runat="server" ID="hifCurrency" />
                                                </div>
                                            </div>
                                        </div>
                                        <div style="display: none">
                                            <div class="flex">
                                                <div class="f-flex">
                                                </div>
                                                <div class="f-flex form">
                                                    <asp:TextBox ID="txtSOTax" runat="server" onchange="return validateFloatKeyPress(this);" onblur="CheckDecimal(this)" MaxLength="8" required="" />
                                                    <label>
                                                        <asp:Literal ID="ltSOTax" runat="server" />
                                                        <%= GetGlobalResourceObject("Resource", "Tax")%></label>
                                                </div>
                                            </div>
                                        </div>


                                    </div>

                                </div>
 <%--                               <div class="row">
                           <div style="display: flex; justify-content: flex-end; margin-right: 10px; padding-bottom: 10px;">
                                        <div class="flex__">
                                            <div colspan="3" align="right">
                                                <asp:LinkButton ID="lnkClose" Visible="false" runat="server" CssClass="btn btn-primary" OnClick="lnkClose_Click" OnClientClick="return confirm('Are you sure want to Close?')">Close <%=MRLWMSC21Common.CommonLogic.btnfaClear %></asp:LinkButton>
                                               
                                                <asp:LinkButton ID="lnkCancel" CssClass="btn btn-primary" runat="server" Text="SO Cancel" OnClientClick="showConfirmAlert();">
                                            <%= GetGlobalResourceObject("Resource", "Cancel")%><%=MRLWMSC21Common.CommonLogic.btnfaClear %>
                                                </asp:LinkButton>
                                              
                                                <asp:LinkButton ID="lnkClear" CssClass="btn btn-primary" runat="server" OnClientClick="lnkClear()"><%= GetGlobalResourceObject("Resource", "Cancel")%><%=MRLWMSC21Common.CommonLogic.btnfaClear %>
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="lnkUpdate" CssClass="btn btn-primary" ValidationGroup="savecustomerdetails" OnClick="lnkUpdate_Click" runat="server">
                                                </asp:LinkButton>

                                            </div>
                                        </div>
                                    </div>
                                </div>--%>

                            </div>
                        </div>
                    </div>
                </div>

                <%--<tr>
                    <td class="accordinoGap"></td>
                </tr>
                <tr>
                    <td colspan="3" class="pagewidth">
                        <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="dvSADHeader" style="">Shipment Address Details </div>

                        <div class="ui-Customaccordion" id="dvSADBody">
                            <table border="0" cellspacing="0" cellpadding="0" width="100%" style="padding-top: 10px; padding-left: 10px;">
                                <tr>
                                    <td valign="top" width="50%" align="center">Customer Address:<br />
                                        <asp:TextBox ID="txtCustomerAddress" Visible="false" runat="server" Width="340" Rows="16" Height="340" Enabled="false" TextMode="MultiLine"></asp:TextBox>
                                        <asp:Label ID="lblCustomerAddress" runat="server"></asp:Label>

                                    </td>

                                    <td align="left">

                                        <asp:UpdatePanel ChildrenAsTriggers="true" ID="upCustomerdetails" UpdateMode="Always" runat="server">
                                             <Triggers>
                                        <asp:PostBackTrigger ControlID="lnkUpdate" />
                                    </Triggers>
                                            <ContentTemplate>
                                                <table cellspacing="3" cellpadding="2" border="0">
                                                    <tr>
                                                        <td colspan="2">

                                                            <asp:CheckBox ID="chkaddress" runat="server" AutoPostBack="true" CssClass="chk_slim_req" OnCheckedChanged="ckaddress_CheckedChanged" Text="Same as Customer Address" />

                                                            &nbsp; &nbsp;
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ValidationGroup="savecustomerdetails" Enabled="false" Visible="false" CssClass="errorMsg" ControlToValidate="txtAddress1" Display="Dynamic" ErrorMessage=" * " />
                                                            <asp:Literal ID="ltAddress1" runat="server" Text="Address1:" /><br />
                                                            <asp:TextBox ID="txtAddress1" runat="server" />
                                                        </td>
                                                        <td>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ValidationGroup="savecustomerdetails" CssClass="errorMsg" Visible="false" ControlToValidate="txtAddress2" Enabled="false" Display="Dynamic" ErrorMessage=" * " />
                                                            <asp:Literal ID="ltAddress2" runat="server" Text="Address2:" /><br />
                                                            <asp:TextBox ID="txtAddress2" runat="server" />
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ValidationGroup="savecustomerdetails" CssClass="errorMsg" Enabled="false" Visible="false" ControlToValidate="txtCity" Display="Dynamic" ErrorMessage=" * " />
                                                            <asp:Literal ID="ltCity" runat="server" Text="City:" /><br />
                                                            <asp:TextBox ID="txtCity" runat="server" />
                                                        </td>
                                                        <td>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ValidationGroup="savecustomerdetails" CssClass="errorMsg" Enabled="false" ControlToValidate="txtState" Visible="false" Display="Dynamic" ErrorMessage=" * " />
                                                            <asp:Literal ID="ltState" runat="server" Text="State:" /><br />
                                                            <asp:TextBox ID="txtState" runat="server" MaxLength="30" />
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="savecustomerdetails" CssClass="errorMsg" Enabled="false" ControlToValidate="txtCountyMaster" Visible="false" Display="Dynamic" ErrorMessage=" * " />
                                                            <asp:Literal ID="lclCountry" runat="server" Text="Country:" /><br />
                                                            <asp:TextBox ID="txtCountyMaster" SkinID="txt_Auto" runat="server" />
                                                            <asp:HiddenField runat="server" ID="hifCountryMaster" />
                                                        </td>
                                                        <td>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" Visible="false" ValidationGroup="savecustomerdetails" CssClass="errorMsg" Enabled="false" ControlToValidate="txtSOCode" Display="Dynamic" ErrorMessage=" * " />
                                                            <asp:Literal ID="lclZip" runat="server" Text="Zip:" /><br />
                                                            <asp:TextBox ID="txtZip" runat="server" MaxLength="30" />
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" Visible="false" ValidationGroup="savecustomerdetails" CssClass="errorMsg" Enabled="false" ControlToValidate="txtSOCode" Display="Dynamic" ErrorMessage=" * " />
                                                            <asp:Literal ID="Literal5" runat="server" Text="Phone No.:" /><br />
                                                            <asp:TextBox ID="txtPhoneNo" runat="server" MaxLength="30" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="accordinoGap"></td>
                                                    </tr>
                                                    <tr>

                                                        <td colspan="2" align="right">
                                                            <asp:LinkButton ID="lnkClose" Visible="false" runat="server" CssClass="btn btn-primary" OnClick="lnkClose_Click" OnClientClick="return confirm('Are you sure want to Close?')">Close <%=MRLWMSC21Common.CommonLogic.btnfaClear %></asp:LinkButton>
                                                            <asp:LinkButton ID="lnkCancel" CssClass="btn btn-primary" OnClientClick="return confirm('Are you sure want to cancel?')" runat="server" OnClick="lnkCancel_Click">
                                                                        Cancel <%=MRLWMSC21Common.CommonLogic.btnfaClear %>
                                                            </asp:LinkButton>

                                                            <asp:LinkButton ID="lnkUpdate" CssClass="btn btn-primary" ValidationGroup="savecustomerdetails" OnClick="lnkUpdate_Click" runat="server">
                                                            </asp:LinkButton>
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
                </tr>--%>
                <tr>
                    <td class="accordinoGap"></td>
                </tr>
                <tr>
                    <td colspan="3" class="">
                       <gap></gap>
                        <%--<div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="dvSADHeader" style="">Customer Billing Address Details</div>--%>
                        <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="dvSADHeader" style="">Shipment Address Details 
                      <%--  <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="dvSADHeader" style=""> <%= GetGlobalResourceObject("Resource", "CustomerBillingAddress")%>--%>
                             <i class="material-icons downarrow right">keyboard_arrow_down</i>
                        </div>
                        <div class="ui-Customaccordion" id="dvSADBody">
                            <asp:UpdatePanel ChildrenAsTriggers="true" ID="upCustomerdetails" UpdateMode="Always" runat="server">
                                <ContentTemplate>
                                    <br />
                                    <div>
                                        <div class="row">
                                            <div class="col m3 s3" style="display:none;">
                                                <div class="flex">
                                                   
                                                    <div>
                                                        <asp:TextBox ID="txtBCustName" ClientIDMode="Static" Enabled="true" runat="server" required />
                                                        <label>
                                                            <asp:Literal ID="ltBCustName" runat="server" Text="Customer Name" /></label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col m3 s3">
                                                <div class="flex">
                                                  
                                                    <div>
                                                        <asp:TextBox ID="txtBAddress1" ClientIDMode="Static" Enabled="true" runat="server" required/>
                                                        <label>
                                                            <asp:Literal ID="ltBAddress1" runat="server" Text="Address1" /></label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col m3 s3">
                                                <div class="flex">
                                                    <div>
                                                        <asp:TextBox ID="txtBAddress2" ClientIDMode="Static" Enabled="true" runat="server" required/>
                                                        <label>
                                                            <asp:Literal ID="ltBAddress2" runat="server" Text="Address2" /></label>
                                                    </div>
                                                </div>
                                            </div>
                                      
                                            <div class="col m3 s3">
                                                <div class="flex">
                                                    <div>
                                                        <asp:TextBox ID="txtBCity" ClientIDMode="Static" Enabled="true" runat="server" required/>
                                                        <label>
                                                            <asp:Literal ID="ltBCity" runat="server" Text="City" /></label>
                                                    </div>
                                                </div>
                                            </div> 

                                            <div class="col m3 s3">
                                                <div class="flex">
                                                    <div>
                                                        <asp:TextBox ID="txtBState" ClientIDMode="Static" Enabled="true" runat="server" MaxLength="30" required/><label><asp:Literal ID="ltBState" runat="server" Text="State" /></label>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>

                                        <div class="row">
                                            
                                            <div class="col m3 s3">
                                                <div class="flex">
                                                    <div>
                                                        <asp:TextBox ID="txtBCountyMaster" ClientIDMode="Static" Enabled="true" SkinID="txt_Auto" runat="server" required/>
                                                        <label>
                                                            <asp:Literal ID="ltBCountry" runat="server" Text="Country" /></label>
                                                        <asp:HiddenField runat="server" ID="hifCountryMaster" Value="0" ClientIDMode="Static" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col m3 s3">
                                                <div class="flex">
                                                    <div>
                                                        <asp:TextBox ID="txtBPhoneNo" runat="server" MaxLength="30" Enabled="true" SkinID="txt_Auto" required ClientIDMode="Static"/>
                                                        <label>
                                                        <asp:Literal ID="ltBPhone" runat="server" Text="Phone No." />
                                                        </label>
                                                    </div>
                                                </div>
                                            </div>
                                      
                                            <div class="col m3 s3">
                                                <div class="flex">
                                                    <div>
                                                        <asp:TextBox ID="txtBZip" ClientIDMode="Static" Enabled="true" runat="server" MaxLength="30" required/>
                                                        <label>
                                                            <asp:Literal ID="ltBZip" runat="server" Text="Zip" /></label>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col m3">
                                                <gap></gap>
                                                <div style="display: flex; justify-content: flex-end; margin-right: 10px;">
                                                    <div class="flex__">
                                                        <div colspan="3" align="right">
                                                            <asp:LinkButton ID="lnkClose" Visible="false" runat="server" CssClass="btn btn-primary" OnClick="lnkClose_Click" OnClientClick="return confirm('Are you sure want to Close?')">Close <%=MRLWMSC21Common.CommonLogic.btnfaClear %></asp:LinkButton>

                                                            <asp:LinkButton ID="lnkCancel" CssClass="btn btn-primary" runat="server" Text="SO Cancel" OnClientClick="showConfirmAlert();">
                                            <%= GetGlobalResourceObject("Resource", "Cancel")%><%=MRLWMSC21Common.CommonLogic.btnfaClear %>
                                                            </asp:LinkButton>

                                                            <asp:LinkButton ID="lnkClear" CssClass="btn btn-primary" runat="server" OnClientClick="lnkClear()"><%= GetGlobalResourceObject("Resource", "Cancel")%><%=MRLWMSC21Common.CommonLogic.btnfaClear %>
                                                            </asp:LinkButton>
                                                            <asp:LinkButton ID="lnkUpdate" CssClass="btn btn-primary" ValidationGroup="savecustomerdetails" OnClick="lnkUpdate_Click" runat="server">
                                                            </asp:LinkButton>

                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        <div class="col m3 s3">
                                            <td class="accordinoGap"></td>
                                        </div>

                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="accordinoGap"></td>
                </tr>

                <tr class="">
                    <td runat="server" id="tdCusPODet" colspan="3">
                        <gap></gap>
                       <%-- <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="dvCPODHeader">Customer PO Details --%>
                         <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="dvCPODHeader"><%= GetGlobalResourceObject("Resource", "CustomerPODetails")%>
                             <i class="material-icons downarrow right">keyboard_arrow_down</i>
                        </div>

                        <div class="ui-Customaccordion" id="dvCPODBody" >
                            <table border="0" cellspacing="0" cellpadding="0" width="100%" style="padding-top: 10px; padding-left: 10px;overflow:hidden">

                                <tr>

                                    <td colspan="3">&nbsp;
                                    </td>
                                </tr>

                                <tr>
                                    
                                    <td align="right" colspan="3" style="padding-bottom: 10px; padding-right: 12px;">
                                        <div style="float:right;">
                                            <div class="flex__">
                                        <%--<asp:LinkButton CssClass="btn btn-primary" runat="server" OnClick="lnkButAddNewd_Click" ID="lnkAddNewCusPO" Visible="false">
                                    Add Customer PO <%=MRLWMSC21Common.CommonLogic.btnfaNew %>
                                        </asp:LinkButton>--%>
                                                <asp:LinkButton CssClass="btn btn-primary" runat="server" OnClick="lnkButAddNewd_Click" ID="lnkAddNewCusPO" Visible="false">
                                    <%= GetGlobalResourceObject("Resource", "AddCustomerPO")%><%=MRLWMSC21Common.CommonLogic.btnfaNew %>
                                        </asp:LinkButton>
                                                </div>
                                            </div>
                                    </td>
                                </tr>

                                <tr>
                                    <td colspan="3" align="center">
                                        <asp:UpdatePanel ChildrenAsTriggers="true" ID="upCustomerPO" UpdateMode="Always" runat="server">
                                            <ContentTemplate>
                                                <table border="0" cellspacing="0" cellpadding="0" width="100%" align="center">

                                                    <tr>
                                                        <td>
                                                            <asp:Panel runat="server" ID="pnlGVCustPODetails" >
                                                                <asp:GridView ID="gvCustPODetails" SkinID="gvLightSteelBlueNew" AutoGenerateColumns="false" runat="server" OnRowCancelingEdit="gvCustPODetails_RowCancelingEdit" OnRowEditing="gvCustPODetails_RowEditing" OnRowUpdating="gvCustPODetails_RowUpdating" OnPageIndexChanging="gvCustPODetails_PageIndexChanging" OnRowDataBound="gvCustPODetails_RowDataBound">
                                                                    <Columns>
                                                                       <%-- <asp:TemplateField ControlStyle-Width="100" HeaderText="Customer PO Number" >--%>
                                                                         <asp:TemplateField ControlStyle-Width="100" HeaderText="<%$Resources:Resource,CustomerPONumber%>" >
                                                                            <ItemTemplate>
                                                                                <asp:Literal ID="ltcustomPoID" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "CustomerPOID").ToString() %>' />
                                                                                <asp:Literal runat="server" ID="ltCustomerPONumber" Text='<%#DataBinder.Eval(Container.DataItem, "CustPONumber").ToString() %>' />
                                                                            </ItemTemplate>
                                                                            <EditItemTemplate>
                                                                                <div class="gridInput">
                                                                                <asp:RequiredFieldValidator ID="rfvCustomerPONumber" runat="server" ValidationGroup="UpdateCustomer" ControlToValidate="txtCustomerPONumber" Display="Dynamic" style="width:auto !important" />
                                                                                <asp:Literal ID="ltcustomPoID_Edit" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "CustomerPOID").ToString() %>' />
                                                                               <span class="errorMsg"></span> <asp:TextBox ID="txtCustomerPONumber" runat="server" onKeypress="return checkSpecialChar(event)" MaxLength="20" Text='<%#DataBinder.Eval(Container.DataItem, "CustPONumber").ToString() %>' />
                                                                          </div>
                                                                                    </EditItemTemplate>
                                                                        </asp:TemplateField>

                                                                      <%--  <asp:TemplateField ControlStyle-Width="100" HeaderText="Customer PO Date" >--%>
                                                                        <asp:TemplateField ControlStyle-Width="100" HeaderText="<%$Resources:Resource,CustomerPODate%>" >
                                                                            <ItemTemplate>
                                                                                <asp:Literal runat="server" ID="ltCustPODate" Text='<%#DataBinder.Eval(Container.DataItem, "CustPODate","{0:dd-MMM-yyyy}").ToString() %>' />
                                                                            </ItemTemplate>
                                                                            <EditItemTemplate>
                                                                                <div class="gridInput">
                                                                                <asp:RequiredFieldValidator ID="rfvCustPODate" runat="server" ValidationGroup="UpdateCustomer" ControlToValidate="txtCustPODate" Display="Dynamic" style="width:auto !important" />
                                                                               <asp:TextBox ID="txtCustPODate" runat="server" ClientIDMode="Static" CssClass="DateBoxCSS_small" EnableTheming="true" Text='<%#DataBinder.Eval(Container.DataItem, "CustPODate","{0:dd-MMM-yyyy}").ToString() %>' />
                                                                             <span class="errorMsg"></span>
                                                                                </div>
                                                                                    </EditItemTemplate>
                                                                        </asp:TemplateField>

                                                                        <%-- <asp:TemplateField ControlStyle-Width="100" HeaderText="Invoice No.">
                                                                            <ItemTemplate>
                                                                                <asp:Literal runat="server" ID="ltInvoiceNumber" Text='<%#DataBinder.Eval(Container.DataItem, "InvoiceNumber").ToString() %>' />
                                                                            </ItemTemplate>

                                                                            <EditItemTemplate>
                                                                                <asp:RequiredFieldValidator ID="rfvInvoiceNumber" runat="server" ValidationGroup="UpdateCustomer" CssClass="errorMsg" ControlToValidate="txtInvoiceNumber" Display="Dynamic" ErrorMessage=" * " />
                                                                                <asp:TextBox ID="txtInvoiceNumber" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "InvoiceNumber").ToString() %>' />
                                                                            </EditItemTemplate>
                                                                        </asp:TemplateField>--%>

                                                                        <%--<asp:TemplateField ControlStyle-Width="100" HeaderText="Currency">--%>
                                                                        <asp:TemplateField Visible="false" ControlStyle-Width="100" HeaderText="<%$Resources:Resource,Currency%>">
                                                                            <ItemTemplate>
                                                                                <asp:Literal runat="server" ID="ltCurrency" Text='<%#DataBinder.Eval(Container.DataItem, "Code").ToString() %>' />
                                                                            </ItemTemplate>
                                                                            <EditItemTemplate>
                                                                                <asp:TextBox ID="atcCurrency" runat="server" CssClass="POCurrrencyPicker" EnableTheming="false" Text='<%#DataBinder.Eval(Container.DataItem, "Code").ToString() %>' />
                                                                                <asp:HiddenField ID="hifPOCurrency" runat="server" ClientIDMode="Static" Value='<%#DataBinder.Eval(Container.DataItem, "CurrencyID").ToString() %>' />
                                                                            </EditItemTemplate>
                                                                        </asp:TemplateField>
                                                                       <%-- <asp:TemplateField ControlStyle-Width="100" HeaderText="Exchange Rate">--%>
                                                                         <asp:TemplateField ControlStyle-Width="100" Visible="false" HeaderText="<%$Resources:Resource,ExchangeRate%>">
                                                                            <ItemTemplate>
                                                                                <asp:Literal runat="server" ID="ltExchangeRate" Text='<%#DataBinder.Eval(Container.DataItem, "ExchangeRate").ToString() %>' />
                                                                            </ItemTemplate>
                                                                            <EditItemTemplate>
                                                                                <asp:TextBox ID="txtExchangeRate" runat="server" onchange="return validateFloatKeyPress(this);" onblur="CheckDecimal(this)" MaxLength="8" Text='<%#DataBinder.Eval(Container.DataItem, "ExchangeRate").ToString() %>' />

                                                                            </EditItemTemplate>
                                                                        </asp:TemplateField>
                                                                       <%-- <asp:TemplateField ControlStyle-Width="100" HeaderText="Customer PO Value">--%>
                                                                         <asp:TemplateField Visible="false" ControlStyle-Width="100" HeaderText= "<%$Resources:Resource,CustomerPOValue%>">
                                                                            <ItemTemplate>
                                                                                <asp:Literal runat="server" ID="ltCustPOValue" Text='<%#DataBinder.Eval(Container.DataItem, "CustPOValue").ToString() %>' />
                                                                            </ItemTemplate>
                                                                            <EditItemTemplate>
                                                                                <asp:TextBox ID="txtCustPOValue" runat="server" onchange="return validateFloatKeyPress(this);" onblur="CheckDecimal(this)" MaxLength="8" Text='<%#DataBinder.Eval(Container.DataItem, "CustPOValue").ToString() %>' />

                                                                            </EditItemTemplate>
                                                                        </asp:TemplateField>

                                                                        <%--<asp:TemplateField ControlStyle-Width="60" HeaderText="Delete">--%>
                                                                        <asp:TemplateField ControlStyle-Width="60" Visible="false" HeaderText="<%$Resources:Resource,Delete%>">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkDelete" Visible="false" CssClass="chkDelete" runat="server" />
                                                                            </ItemTemplate>
                                                                            <EditItemTemplate>
                                                                            </EditItemTemplate>
                                                                            <FooterTemplate>
                                                                                  <asp:LinkButton ID="lntDeleteItems" runat="server" Visible="false"  ForeColor="Blue" OnClientClick="return lnkDeleteItem_ClientClick();" Font-Underline="false" OnClick="lntDeleteItems_Click" ><%=MRLWMSC21Common.CommonLogic.btnfaDelete %></asp:LinkButton>
                                                                      
                                                                               <%-- <asp:LinkButton ID="lntDeleteItems" runat="server" Text="<i class='material-icons ss'>delete</i>" ForeColor="Blue" Font-Underline="false" OnClientClick="return confirm('Are you sure want to delete the selected line items?');" OnClick="lntDeleteItems_Click" />
                                                                  --%>          </FooterTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkCPOEdit" runat="server" Text="<nobr> <i class='material-icons'>mode_edit</i></nobr>" CssClass="" OnClick="gvCustPODetails_RowEditing"></asp:LinkButton>

                                                                            </ItemTemplate>
                                                                            <EditItemTemplate>
                                                                                <asp:Button ID="lnkCPOUpdate" CssClass="ButnEmpty" runat="server" Text="Update" OnClientClick="this.disabled = true; this.value = 'Submitting...';" UseSubmitBehavior="false" OnClick="gvCustPODetails_RowUpdating"></asp:Button>
                                                                                 <asp:LinkButton ID="lnkCPOCancel" runat="server" Text="Cancel" CssClass="ButnEmpty" OnClick="gvCustPODetails_RowCancelingEdit" ></asp:LinkButton>
                                                                                
                                                                           <%--     <asp:Button ID="lnkCPOCancel" CssClass="ButnEmpty" runat="server" Text="Cancel" OnClick="gvCustPODetails_RowCancelingEdit"></asp:Button>
                                                                      --%>      </EditItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <%-- <asp:CommandField ControlStyle-Font-Underline="false" EditText="<nobr> Edit <img src='../Images/redarrowright.gif' border='0' /></nobr>" ShowEditButton="true" CausesValidation="false" ButtonType="Link" />--%>
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
                <tr>
                    <td class="accordinoGap"></td>
                </tr>

                <tr>
                    <td runat="server" id="tdSOLineItems" colspan="3">
                        <gap></gap>
                        <%--<div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="dvMDHeader" style="">Material Details--%>
                        <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="dvMDHeader" style=""><%= GetGlobalResourceObject("Resource", "MaterialDetails")%>

                             <i class="material-icons downarrow right">keyboard_arrow_down</i>
                        </div>

                        <div class="ui-Customaccordion" id="dvMDBody" style="">
                            <div>
                           

                                <div class="row">

                                    <div class="">
                                        <asp:Panel ID="pnlSOSearch" runat="server" DefaultButton="lnksearch">
                                            <div>
                                                <div class="">
                                                    <div class="col m3 offset-m7" >
                                                        <div class="flex" runat="server" ID="dvSearch">
                                                            <asp:TextBox runat="server" ID="txtSearch" ClientIDMode="Static" SkinID="txt_Hidden_Req_Auto"  required="" />
                                                            <label><%= GetGlobalResourceObject("Resource", "SearchPartNumber")%></label>
                                                        </div>
                                                    </div>
                                                    <div class="col m2">
                                                        <gap></gap>
                                                      <flex>  <asp:LinkButton runat="server" ID="lnksearch" CssClass="btn btn-primary" OnClick="lnksearch_Click">Search<%=MRLWMSC21Common.CommonLogic.btnfaSearch %></asp:LinkButton>
                                                   
                                                        <asp:LinkButton ID="lbutNewItem" CssClass="btn btn-primary" OnClick="butNewItem_Click" runat="server">
                                                                <%= GetGlobalResourceObject("Resource", "Add")%> <%=MRLWMSC21Common.CommonLogic.btnfaNew %>
                                                        </asp:LinkButton></flex>
                                                    </div>

                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>

                                </div>
                            <div border="0" cellspacing="0" cellpadding="0" width="100%" style="padding-top: 10px; padding-left: 10px;">                              
                                <div>
                                    <div colspan="3" align="left">
                                        <scroll></scroll>
                                        <asp:UpdatePanel ID="upnlSODetails" ChildrenAsTriggers="true" runat="server" UpdateMode="Always">
                                            <ContentTemplate>
                                                <asp:Panel ID="pnlgvSoDetails" runat="server"  >
                                                    <asp:Literal runat="server" ID="ltgridstatus" />
                                                    <asp:GridView ID="gvSODetails" SkinID="gvLightSteelBlueNew" runat="server" Width="100%" CellPadding="4" GridLines="None" OnRowCancelingEdit="gvSODetails_RowCancelingEdit" OnRowEditing="gvSODetails_RowEditing" OnRowUpdating="gvSODetails_RowUpdating" OnPageIndexChanging="gvSODetails_PageIndexChanging" OnRowDataBound="gvSODetails_RowDataBound" OnDataBinding="gvSODetails_DataBinding" AutoGenerateColumns="false" AllowPaging="true" PageSize="10" ShowHeader="true">
                                                        <AlternatingRowStyle BackColor="White" />
                                                        <Columns>
                                                            <%--<asp:TemplateField ItemStyle-Width="100" HeaderText="Line No.">--%>
                                                            <asp:TemplateField ItemStyle-Width="100" HeaderText="<%$Resources:Resource,LineNo%>">
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltlinenumber" Text='<%#DataBinder.Eval(Container.DataItem, "LineNumber").ToString() %>' />
                                                                    <asp:Literal runat="server" Visible="false" ID="ltSODetailsID" Text='<%#DataBinder.Eval(Container.DataItem, "SODetailsID").ToString() %>' />
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltlinenumberEdit" Text='<%#DataBinder.Eval(Container.DataItem, "LineNumber").ToString() %>' />
                                                                    <asp:Literal runat="server" Visible="false" ID="ltSODetailsIDEdit" Text='<%#DataBinder.Eval(Container.DataItem, "SODetailsID").ToString() %>' />

                                                                </EditItemTemplate>
                                                            </asp:TemplateField>

                                                           <%-- <asp:TemplateField ItemStyle-Width="100" HeaderText="Part #">--%>
                                                            <asp:TemplateField ItemStyle-Width="100" HeaderText="Material Desc.">
                                                                <ItemTemplate>

                                                                    <asp:Literal runat="server" ID="ltMMID" Text='<%#DataBinder.Eval(Container.DataItem, "MDescription").ToString() %>' />

                                                                    <br />
                                                                    <%--   <asp:Label CssClass="BOMPartNoHead" runat="server" ID="lbOEMPartNumber" Text='<%# DataBinder.Eval(Container.DataItem, "oempartno").ToString() %>'></asp:Label>
                                                                    --%>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <div class="gridInput">
                                                                        <asp:RequiredFieldValidator ID="rfvMCode" runat="server" ValidationGroup="UpdateSODetails" CssClass="errorMsg" ControlToValidate="atcEditMCode" Display="Dynamic" />
                                                                        <span class="errorMsg"></span>
                                                                        <asp:TextBox ID="atcEditMCode" style="width:250px !important;" onfocus="javascript:MSPConfifure(this);" runat="server" onblur="javascript:MSPConfifure(this);" Visible="true" EnableTheming="false" CssClass="MCodePicker" Width="130" Text='<%#DataBinder.Eval(Container.DataItem, "MDescription").ToString() %>' />
                                                                        <%--<asp:TextBox ID="atcEditMCode" style="width:250px !important;" runat="server" EnableTheming="false" CssClass="MCodePicker" Text='<%#DataBinder.Eval(Container.DataItem, "MDescription").ToString() %>' />--%>
                                                                        <asp:HiddenField runat="server" ID="hifmmid" ClientIDMode="Static" Value='<%#DataBinder.Eval(Container.DataItem, "MaterialMasterID").ToString() %>' />
                                                                    </div>

                                                                </EditItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="MCode">
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltmaterialcode" Text='<%# DataBinder.Eval(Container.DataItem, "MCode").ToString() %>' />
                                                                    <br />                                                                 
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <div class="gridInput">
                                                                    <%--    <asp:RequiredFieldValidator ID="rfvMCode" runat="server" ValidationGroup="UpdateSODetails" ControlToValidate="atcMCode" Display="Dynamic" />--%>
                                                                        <span class="errorMsg"></span>
                                                                        <asp:TextBox ID="atcMCode" runat="server" SkinID="txt_Hidden_Req_Auto" Enabled="false" onfocus="javascript:MSPConfifure(this)" Width="130" ClientIDMode="Static" onblur="javascript:MSPConfifure(this)" Text='<%# DataBinder.Eval(Container.DataItem, "MCode").ToString() %>' />
                                                                        <asp:HiddenField ID="hifMCode" runat="server" ClientIDMode="Static" Value='<%# DataBinder.Eval(Container.DataItem, "MaterialMasterID").ToString() %>' />
                                                                    </div>
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>



                                                           <%-- <asp:TemplateField ItemStyle-Width="100" HeaderText="BUoM/ Qty.">--%>
                                                             <asp:TemplateField ItemStyle-Width="100" HeaderText="<%$Resources:Resource,BUoMQty%>">
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltBuomQtyID" Text='<%#String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "BUoM").ToString(),DataBinder.Eval(Container.DataItem, "BUoMQty").ToString())  %>' />

                                                                </ItemTemplate>

                                                            </asp:TemplateField>

                                                            <%--<asp:TemplateField ItemStyle-Width="100" HeaderText="SO UoM/ Qty.">--%>
                                                            <asp:TemplateField ItemStyle-Width="100" HeaderText="<%$Resources:Resource,SOUoMQty%>" >
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltSUoMQtyID" Text='<%#String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "SUoM").ToString(),DataBinder.Eval(Container.DataItem, "SUoMQty").ToString())  %>' />

                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                     <div class="gridInput">
                                                                    <asp:RequiredFieldValidator ID="rfvSUoMID" runat="server" ValidationGroup="UpdateSODetails" CssClass="errorMsg" ControlToValidate="atcSUoMID" Display="Dynamic" />
                                                                         <span class="errorMsg"></span><asp:TextBox runat="server" ID="atcSUoMID" ClientIDMode="Static" EnableTheming="false" CssClass="SUoMPicker" Width="80" Text='<%#String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "SUoM").ToString(),DataBinder.Eval(Container.DataItem, "SUoMQty").ToString())=="/"?"":String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "SUoM").ToString(),DataBinder.Eval(Container.DataItem, "SUoMQty").ToString())  %>' />
                                                                    <asp:HiddenField runat="server" ID="hifUoMid" ClientIDMode="Static" Value='<%#DataBinder.Eval(Container.DataItem, "MaterialMaster_SUoMID").ToString() %>' />
                                                                    <input id="hidUoMQty" type="hidden" value='<%# DataBinder.Eval(Container.DataItem, "SUoMQty").ToString() %>' />
                                                              </div>
                                                                         </EditItemTemplate>
                                                            </asp:TemplateField>
                                                           <%-- <asp:TemplateField ItemStyle-Width="100" HeaderText="Kit ID">--%>
                                                             <asp:TemplateField ItemStyle-Width="100" HeaderText="<%$Resources:Resource,KitID%>" >
                                                                <ItemTemplate>

                                                                    <asp:Literal runat="server" ID="ltplannet" Text='<%#DataBinder.Eval(Container.DataItem, "KitCode").ToString() %>' />

                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:Image ImageUrl="../Images/kit.gif" ClientIDMode="Static" Visible="true" ID="imgkit" runat="server" />

                                                                    <asp:TextBox runat="server" ID="atcKitPlanner" ClientIDMode="Static" EnableTheming="false" CssClass="KitPlannerPicker" Width="60" Text='<%#DataBinder.Eval(Container.DataItem, "KitPlannerID").ToString() %>' />
                                                                    <asp:HiddenField ID="hifKitPlanner" runat="server" ClientIDMode="Static" Value='<%#DataBinder.Eval(Container.DataItem, "KitPlannerID").ToString() %>' />

                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                           <%-- <asp:TemplateField ItemStyle-Width="60" HeaderText="Kit Qty.">--%>
                                                             <asp:TemplateField ItemStyle-Width="60" HeaderText="<%$Resources:Resource,KitQty%>" >
                                                                <ItemTemplate>

                                                                    <asp:Literal runat="server" ID="ltkitQty" Text='<%#DataBinder.Eval(Container.DataItem, "KitQty").ToString() %>' />

                                                                </ItemTemplate>

                                                            </asp:TemplateField>
                                                           <%-- <asp:TemplateField ItemStyle-Width="100" HeaderText="SO Qty.">--%>
                                                             <asp:TemplateField ItemStyle-Width="100" HeaderText="<%$Resources:Resource,SOQty%>" >
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltQuantity" Text='<%#DataBinder.Eval(Container.DataItem, "SOQuantity").ToString() %>' />

                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <div class="gridInput">
                                                                    <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ValidationGroup="UpdateSODetails" CssClass="errorMsg" ControlToValidate="txtQuantity" Display="Dynamic" />
                                                                   <span class="errorMsg"></span>
                                                                        <asp:TextBox runat="server" ID="txtQuantity" onchange="return validateFloatKeyPress(this);"  MaxLength="8" onblur="CheckSUoMQty(this)" Width="80" Text='<%#DataBinder.Eval(Container.DataItem, "SOQuantity").ToString() %>' />
                                                                </div>
                                                                        </EditItemTemplate>
                                                            </asp:TemplateField>
                                                          <%--  <asp:TemplateField ItemStyle-Width="100" HeaderText="Unit Price">--%>
                                                              <asp:TemplateField ItemStyle-Width="100" HeaderText="<%$Resources:Resource,UnitPrice%>"  >
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltPrice" Text='<%#DataBinder.Eval(Container.DataItem, "UnitPrice").ToString() %>' />

                                                                </ItemTemplate>
                                                                <EditItemTemplate>

                                                                    <asp:TextBox runat="server" ID="txtPrice" onchange="return validateFloatKeyPress(this);"  MaxLength="8" Width="80" Text='<%#DataBinder.Eval(Container.DataItem, "UnitPrice").ToString() %>' />
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                            <%--<asp:TemplateField ItemStyle-Width="100" HeaderText="Customer PO">--%>
                                                            <asp:TemplateField ItemStyle-Width="100" HeaderText="<%$Resources:Resource,CustomerPO%>" >
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltCustPONumber" Text='<%#DataBinder.Eval(Container.DataItem, "CustPONumber").ToString() %>' />

                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <div class="gridInput">
                                                                    <asp:RequiredFieldValidator ID="rfvCustomerPO" runat="server" ValidationGroup="UpdateSODetails" CssClass="errorMsg" ControlToValidate="atcCustPONumber" Display="Dynamic" />
                                                                   <span class="errorMsg"></span><asp:TextBox runat="server" ID="atcCustPONumber" onblur="javascript:CheckPONumber(this);" ClientIDMode="Static" SkinID="txt_Auto" onKeypress="return checkSpecialChar(event)" Width="80" Text='<%#DataBinder.Eval(Container.DataItem, "CustPONumber").ToString() %>' />
                                                                    <asp:HiddenField ID="hifCustomPOId" runat="server" ClientIDMode="Static" Value='<%#DataBinder.Eval(Container.DataItem, "CustomerPOID").ToString() %>' />
                                                               </div>
                                                                        </EditItemTemplate>
                                                            </asp:TemplateField>
                                                           <%-- <asp:TemplateField ItemStyle-Width="100" HeaderText="Customer PO UoM/Qty.">--%>
                                                             <asp:TemplateField ItemStyle-Width="100" HeaderText="<%$Resources:Resource,CustomerPOUoMQty%>" >
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltCustPOUoM" Text='<%#String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "CustPOUoM").ToString(),DataBinder.Eval(Container.DataItem, "CustPOUoMQty").ToString())=="/"?"":String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "CustPOUoM").ToString(),DataBinder.Eval(Container.DataItem, "CustPOUoMQty").ToString())%>' />

                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                     <div class="gridInput">
                                                                    <asp:RequiredFieldValidator ID="rfvCustomerUOM" runat="server" ValidationGroup="UpdateSODetails" CssClass="errorMsg" ControlToValidate="atcCustPOUoM" Display="Dynamic" />
                                                                     <span class="errorMsg"></span><asp:TextBox runat="server" CssClass="custUoMPicker" EnableTheming="false" ClientIDMode="Static" ID="atcCustPOUoM" Width="80" Text='<%#String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "CustPOUoM").ToString(),DataBinder.Eval(Container.DataItem, "CustPOUoMQty").ToString())=="/"?"":String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "CustPOUoM").ToString(),DataBinder.Eval(Container.DataItem, "CustPOUoMQty").ToString())%>' />
                                                                    <asp:HiddenField ID="hifcustPoUoM" runat="server" ClientIDMode="Static" Value='<%#DataBinder.Eval(Container.DataItem, "MaterialMaster_CustPOUoMID").ToString() %>' />
                                                              </div>
                                                                         </EditItemTemplate>
                                                            </asp:TemplateField>
                                                           <%-- <asp:TemplateField ItemStyle-Width="100" HeaderText="Customer PO Qty.">--%>
                                                             <asp:TemplateField ItemStyle-Width="100" HeaderText= "<%$Resources:Resource,CustomerPOQty%>" >
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltCustPOQuantity" Text='<%#DataBinder.Eval(Container.DataItem, "CustPOQuantity").ToString() %>' />

                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <div class="gridInput">
                                                                    <asp:RequiredFieldValidator ID="rfvCustomerQty" runat="server" ValidationGroup="UpdateSODetails" ControlToValidate="txtCustPOQuantity" Display="Dynamic" />
                                                                     <span class="errorMsg"></span><asp:TextBox runat="server" ID="txtCustPOQuantity" ClientIDMode="Static" onchange="return validateFloatKeyPress(this);" onblur="CheckDecimal(this)" MaxLength="8" Width="80" Text='<%#DataBinder.Eval(Container.DataItem, "CustPOQuantity").ToString() %>' />
                                                                </div>
                                                                        </EditItemTemplate>
                                                            </asp:TemplateField>
                                                          
                                                            <%-- <asp:TemplateField ItemStyle-Width="100" HeaderText="Discount" Visible="false">--%>
                                                             <asp:TemplateField ItemStyle-Width="100" HeaderText="<%$Resources:Resource,Discount%>"  Visible="true">
                                                                
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltSODiscountInPercentage" Text='<%#DataBinder.Eval(Container.DataItem, "SODiscountInPercentage").ToString() %>' />
                                                                </ItemTemplate>
                                                                <EditItemTemplate>

                                                                    <asp:TextBox runat="server" ID="txtSODiscountInPercentage" ClientIDMode="Static" onchange="return validateFloatKeyPress(this);" onblur="CheckDecimal(this)" onkeyup="this.value = minmax(this.value, 1, 12)"  Width="80" Text='<%#DataBinder.Eval(Container.DataItem, "SODiscountInPercentage").ToString() %>' />
                                                                   
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                           
                                                            <%--<asp:TemplateField ItemStyle-Width="100" HeaderText="VAT Code" Visible="false">--%>
                                                            <asp:TemplateField ItemStyle-Width="100" HeaderText="<%$Resources:Resource,VATCode%>"  Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltVATCode" Text='<%#DataBinder.Eval(Container.DataItem, "VATCode").ToString() %>' />

                                                                </ItemTemplate>
                                                                <EditItemTemplate>

                                                                    <asp:TextBox runat="server" ID="txtVATCode" ClientIDMode="Static" Width="80" MaxLength="18" onKeypress="return checkSpecialChar(event)" Text='<%#DataBinder.Eval(Container.DataItem, "VATCode").ToString() %>' />
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                           <%-- <asp:TemplateField ItemStyle-Width="100" HeaderText="Invoice No.">--%>
                                                             <asp:TemplateField ItemStyle-Width="100" HeaderText="<%$Resources:Resource,InvoiceNo%>" >
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltInvoiceNo" Text='<%#DataBinder.Eval(Container.DataItem, "InvoiceNo").ToString() %>' />

                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <div class="gridInput">
                                                                 <%-- <span class="errorMsg"></span> --%>
                                                                        <asp:TextBox runat="server" ID="txtInvoiceNo" Width="100" onKeypress="return checkSpecialChar(event)" MaxLength="15" Text='<%#DataBinder.Eval(Container.DataItem, "InvoiceNo").ToString() %>' />
                                                               </div>
                                                                        </EditItemTemplate>
                                                            </asp:TemplateField>

                                                            <%--<asp:TemplateField ItemStyle-Width="100" HeaderText="Invoice Date">--%>
                                                            <asp:TemplateField ItemStyle-Width="100" HeaderText="<%$Resources:Resource,InvoiceDate%>" >
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltinvoiceDate" Text='<%#DataBinder.Eval(Container.DataItem, "InvoiceDate","{0:dd-MMM-yyyy}").ToString() %>' />

                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                  <div class="gridInput">
                                                                    <asp:RequiredFieldValidator ID="rfvInvoiceDt" runat="server" ValidationGroup="UpdateSODetails" ControlToValidate="txtinvoiceDate" Display="Dynamic" />
                                                                     
                                                                    <asp:TextBox runat="server" ID="txtinvoiceDate" CssClass="DateBoxCSS_small" EnableTheming="false" Width="100" Text='<%#DataBinder.Eval(Container.DataItem, "InvoiceDate","{0:dd-MMM-yyyy}").ToString() %>' />
                                                            <span class="errorMsg"></span>
                                                                      </div>
                                                                      </EditItemTemplate>
                                                            </asp:TemplateField>

                                                          <%--  <asp:TemplateField ItemStyle-Width="100" HeaderText="Storage Location">--%>
                                                              <asp:TemplateField ItemStyle-Width="100" HeaderText="<%$Resources:Resource,StorageLocation%>" >
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltStorageLocationID" Text='<%#DataBinder.Eval(Container.DataItem,"Code").ToString() %>' />
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <div class="gridInput">
                                                                    <asp:RequiredFieldValidator ID="rfvddlStoragelocation" runat="server" ControlToValidate="ddlStorageLocationID" Display="Dynamic" />
                                                                    <asp:Literal runat="server" Visible="false" ID="ltStorageLocationID" Text='<%#DataBinder.Eval(Container.DataItem,"StorageLocationID").ToString() %>' />
                                                                    <span class="errorMsg"></span> <asp:DropDownList ID="ddlStorageLocationID" runat="server"></asp:DropDownList>
                                                                        </div>
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>

                                                           <%-- <asp:TemplateField ItemStyle-Width="100" HeaderText="Delivery Site">--%>
                                                             <asp:TemplateField ItemStyle-Width="100" HeaderText="<%$Resources:Resource,DeliverySite%>" >
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltDeliveryPoint" Text='<%#DataBinder.Eval(Container.DataItem,"DeliveryPoint").ToString() %>' />
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <%--  <asp:RequiredFieldValidator ID="rfvddlDeliverypoint" runat="server" ControlToValidate="ddlDeliveryPoint" Display="Dynamic" ErrorMessage=" * " />
                                                                    --%>
                                                                    <asp:Literal runat="server" Visible="false" ID="ltDeliveryPoint" Text='<%#DataBinder.Eval(Container.DataItem,"GEN_MST_Address_ID").ToString() %>' />
                                                                    <asp:DropDownList ID="ddlDeliveryPoint" runat="server" style="padding:3px !important;"></asp:DropDownList>
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>


                                                            <%--   Add New Columns--%>
                                                           <%-- <asp:TemplateField ItemStyle-Width="100" HeaderText="Mfg. Date">--%>
                                                             <asp:TemplateField ItemStyle-Width="100" HeaderText="<%$Resources:Resource,MfgDate%>" >
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltmfgdate" Text='<%#DataBinder.Eval(Container.DataItem, "MfgDate_Static","{0:dd-MMM-yyyy}").ToString() %>' />

                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="txtmfgdate" runat="server" ClientIDMode="Static" CssClass="DateBoxCSS_small" EnableTheming="false" Text='<%#DataBinder.Eval(Container.DataItem, "MfgDate_Static","{0:dd-MMM-yyyy}").ToString() %>' />


                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                           <%-- <asp:TemplateField ItemStyle-Width="100" HeaderText="Exp. Date">--%>
                                                             <asp:TemplateField ItemStyle-Width="100" HeaderText="<%$Resources:Resource,ExpDate%>" >
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltexpdate" Text='<%#DataBinder.Eval(Container.DataItem, "ExpDate_Static","{0:dd-MMM-yyyy}").ToString() %>' />

                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="txtexpdate" runat="server" ClientIDMode="Static" CssClass="DateBoxCSS_small" EnableTheming="false" Text='<%#DataBinder.Eval(Container.DataItem, "ExpDate_Static","{0:dd-MMM-yyyy}").ToString() %>' />

                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                            <%--<asp:TemplateField ItemStyle-Width="100" HeaderText="Serial No.">--%>
                                                            <asp:TemplateField ItemStyle-Width="100" HeaderText="<%$Resources:Resource,SerialNo%>" >
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltserialno" Text='<%#DataBinder.Eval(Container.DataItem, "SerialNo_Static").ToString() %>' />
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox runat="server" ID="txtserialno" Width="100" MaxLength="20" onKeypress="return checkSpecialChar(event)" Text='<%#DataBinder.Eval(Container.DataItem, "SerialNo_Static").ToString() %>' />
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                            <%--<asp:TemplateField ItemStyle-Width="100" HeaderText="Batch No.">--%>
                                                            <asp:TemplateField ItemStyle-Width="100" HeaderText="<%$Resources:Resource,BatchNo%>">
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltbatchno" Text='<%#DataBinder.Eval(Container.DataItem, "BatchNo_Static").ToString() %>' />
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox runat="server" ID="txtbatchno" Width="100" MaxLength="20" onKeypress="return checkSpecialChar(event)" Text='<%#DataBinder.Eval(Container.DataItem, "BatchNo_Static").ToString() %>' />
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                           <%-- <asp:TemplateField ItemStyle-Width="100" HeaderText="Project Ref. No.">--%>
                                                            <%-- <asp:TemplateField ItemStyle-Width="100" HeaderText="MRP">--%>
                                                             <asp:TemplateField ItemStyle-Width="100" HeaderText="ProjectRefNo" >
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltrefno" Text='<%#DataBinder.Eval(Container.DataItem, "ProjectRefNo_Static").ToString() %>' />
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox runat="server" ID="txtrefno" Width="100" MaxLength="20" onKeypress="return checkSpecialChar(event)" Text='<%#DataBinder.Eval(Container.DataItem, "ProjectRefNo_Static").ToString() %>' />
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                             <asp:TemplateField ItemStyle-Width="100" HeaderText="<%$Resources:Resource,MRP%>" >
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltMRP" Text='<%#DataBinder.Eval(Container.DataItem, "MRP_Static").ToString() %>' />
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox runat="server" ID="txtMRP" Width="100" MaxLength="20" onKeypress="return checkSpecialChar(event)" Text='<%#DataBinder.Eval(Container.DataItem, "MRP_Static").ToString() %>' />
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>


                                                           <%-- <asp:TemplateField ControlStyle-Width="60" HeaderText="Delete">--%>
                                                             <asp:TemplateField ControlStyle-Width="60" HeaderText="<%$Resources:Resource,Delete%>" >
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkDelete1" CssClass="chkDelete1" runat="server" />
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                </EditItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:LinkButton ID="lntDeleteItems1" runat="server" ForeColor="Blue" Font-Underline="false" OnClientClick="return lnkDeleteItem1_ClientClick()" OnClick="DeleteItems" ><%=MRLWMSC21Common.CommonLogic.btnfaDelete %></asp:LinkButton>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                          <%--  <asp:TemplateField ControlStyle-Width="" HeaderText="Edit">--%>
                                                              <asp:TemplateField ControlStyle-Width="" HeaderText="<%$Resources:Resource,Edit%>" >


                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkCPOEdit" runat="server" Text="<nobr> <i class='material-icons'>mode_edit</i></nobr>" CssClass="" OnClick="gvSODetails_RowEditing"></asp:LinkButton>

                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                   <flex> <asp:Button ID="lnkSOUpdate" CssClass="sm-btn" runat="server" Text="Update"  UseSubmitBehavior="false" OnClick="gvSODetails_RowUpdating"></asp:Button>&nbsp;
                                                                   <asp:LinkButton ID="lnkCSOCancel" runat="server" Text="Cancel" CssClass="sm-btn" OnClick="gvSODetails_RowCancelingEdit" ></asp:LinkButton></flex>
                                                                           
                                                                <%--    <asp:Button ID="lnkCSOCancel" CssClass="ButnEmpty" runat="server" Text="Cancel" OnClick="gvSODetails_RowCancelingEdit"></asp:Button>
                                                             --%>   </EditItemTemplate>
                                                            </asp:TemplateField>
                                                            <%-- <asp:TemplateField ItemStyle-Width="100" HeaderText="Country of Origin">
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltCountryofOrigin" Text='<%#DataBinder.Eval(Container.DataItem, "CountryofOrigin").ToString() %>' />

                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:HiddenField ID="hifCountrofOriginID" ClientIDMode="Static" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "CountryofOriginID").ToString() %>' />
                                                                    <asp:TextBox runat="server" CssClass="CountrofOriginPicker" EnableTheming="false" ID="atcCountrofOriginID" Width="80" Text='<%#DataBinder.Eval(Container.DataItem, "CountryofOrigin").ToString() %>' />
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>--%>
                                                        </Columns>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="accordinoGap"></td>
                </tr>

                <%--<tr>
    <td>
        <asp:Button OnClientClick="testMe('param1');" ClientIDMode="Static"  ID="MyButton" runat="server" Text="Ok" ></asp:Button>
    </td>
</tr>--%>
            </div>
                </div>
       
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="modalConfirmYesNo" class="modal fade" aria-hidden="true" data-backdrop="static" data-keyboard="false">
    <div class="modal-dialog" style="height:300px;width: 400px;">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" 

                class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
              <%--  <h4 id="lblTitleConfirmYesNo" class="modal-title">Confirmation</h4>--%>
                  <h4 id="lblTitleConfirmYesNo" class="modal-title"> <%= GetGlobalResourceObject("Resource", "Confirmation")%></h4>
            </div>
            <div class="modal-body text-left" style="height:50px;">              
                <span style="font-size:larger;"><p id="lblMsgConfirmYesNo"></p></span>
                <br /><br />
                <br /><br />
            </div>
            <div class="modal-footer">               
                <%--<button id="btnYesConfirmYesNo" 

                type="button" class="btn btn-primary">Yes <i class="fa fa-check" aria-hidden="true"></i></button>--%>
                <button id="btnYesConfirmYesNo" 

                type="button" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "Yes")%> <i class="fa fa-check" aria-hidden="true"></i></button>
                <%--<button id="btnNoConfirmYesNo" 

                type="button" class="btn btn-primary">No <i class="fa fa-remove" aria-hidden="true"></i></button>--%>
                <button id="btnNoConfirmYesNo" 

                type="button" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "No")%> <i class="fa fa-remove" aria-hidden="true"></i></button>
            </div>
        </div>
    </div>
</div>
</asp:Content>
