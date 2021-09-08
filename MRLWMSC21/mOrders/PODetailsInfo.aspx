<%@ Page Title="PO Details:." Language="C#" MasterPageFile="~/mOrders/OrdersMaster.master" AutoEventWireup="true" CodeBehind="PODetailsInfo.aspx.cs" Inherits="MRLWMSC21.mOrders.PODetailsInfo" MaintainScrollPositionOnPostback="true" EnableEventValidation="false"  %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="OrdersContent" runat="server">
    <asp:ScriptManager runat="server" EnablePartialRendering="true" ID="PODetailsScript" SupportsPartialRendering="true"></asp:ScriptManager>
    <script type="text/javascript" src="Scripts/CommonScripts.js"></script>

    <script src="../mInventory/Scripts/bootstrap.min.js"></script>
    <link href="../mInventory/Scripts/bootstrap.min.css" rel="stylesheet" />
   <script>
       $(document).ready(function () {
        //var tid=0;
               // var poid=0;
               // var url = window.location.pathname;
             //   if (window.location.href.indexOf("poid") >-1) {
                 //   var obj = location.href.split('?')[1].split('&');
                  //  if (obj.length > 0) {
                  //       poid = obj[0].split('=')[1];
                    //    tid = obj[1].split('=')[1];
                  //  }
              //  }
         //       if (location.href.split('?')[1] >0) {
                //    tid = url.searchParams.get("tid");
             //   poid = url.searchParams.get("poid"); 
          //      }
           //CustomAccordino($('#dvPOHDHeader'), $('#dvPOHDBody'));
           //CustomAccordino($('#dvMDHeader'), $('#dvMDBody'));
           //CustomAccordino($('#dvSIDHeader'), $('#dvSIDBody'));
           $('body').on('click', '.ui-SubHeading', function (){
               $(this).siblings('.ui-Customaccordion').slideToggle();
           });

          // Modal hide when click on NO in Modal Pop up delete

            $("#btnNoConfirmNo").click(function () {
                $("#modalConfirmDelete").modal('hide');     
                return false;     
           });
               $("#btnNoConfirmNo1").click(function () {
                $("#modalInvoiceConfirmDelete").modal('hide');     
                return false;     
           });
           $("#btnNoConfirmNo2").click(function () {
                $("#modalInvMaterialConfirmDelete").modal('hide');     
                return false;     
           });
           
       });
       
   </script>

    <style>
.ui-dialog-titlebar-close {
  visibility: hidden;
}

        .ui-widget input{
            font-family: Verdana,Arial,sans-serif;
        }

       .gvLightSteelBlueNew_DataCellGridEdit input[type="text"], .gvLightSteelBlueNew_DataCellGridEdit select {
    border: 0 !important;
    border-bottom: 1px solid var(--paper-grey-300) !important;
    width: 111px !important;
    margin-right: 5px !important;
    border-radius: 0px;
    padding: 3px !important;
    font-size: 13px !important;
}

#disputeDivSupplierItemList .ui-dialog {
            position:fixed !important;
            top:10vh !important;
        }
/*#disputeDivSupplierItemList .ui-dialog {top:0px !important;}*/

        .DateBoxCSS_small {
                   border-radius:3px;
       border:1px solid #848484;
       font-family:Calibri;
       position:relative;
       color: #A4A4A4; 
       font-size:13pt;
       width:200px;
        }
        .DateBoxCSS_small:focus {
            outline: none;
    color: #000000; 
    box-shadow: 0px 0px 5px #1a79cf;
    border:1px solid #1a79cf;
    border-radius: 4px;
        }
        img
        {  
            border-style: none;
        }
.icon-lock{
    text-indent:-99999px;
}

        #MainContent_OrdersContent_pnlHeaderDetails input[type="text"], textarea {
            width:100% !important;
                margin-bottom: 15px;
        }

        #MainContent_OrdersContent_FUImport {
    border: 1px solid var(--sideNav-bg) !important;
    padding: 0px !important;
        }

.ButnEmpty {
    
    background-color: var(--sideNav-bg); 
    color: #fff;
   
    box-shadow:var(--z1) !important;
        margin: 0px 1px;
}
        td a:hover {
            color: #111 !important;
        }

    </style>
      
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                javaScriptfunction();
            }
        }
        javaScriptfunction();
        var role = [];
        
        function lnkDelete_ClientClick() {
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
        //Show Modal when click on Yes in Modal Pop up delete
        function myconfirmbox()
        {
            $("#modalConfirmDelete").modal({
                show: 'true'
            });
            return false;
        }
         //  Show Modal when click on Yes in Modal Pop up delete by Meena
          function myInvoiceconfirmbox()
        {
            $("#modalInvoiceConfirmDelete").modal({
                show: 'true'
            });
            return false;
        }
        function InvMaterrialconfirmbox() {
            debugger;
            $("#modalInvMaterialConfirmDelete").modal({
                show: 'true'
            });
            return false;
        }
       //End 
        function showConfirmAlert()
        {
            debugger;
            var $confirm = $("#modalConfirmYesNo");
            $confirm.modal('show');
            $("#lblTitleConfirmYesNo").html("<i class='fa fa-exclamation-circle' aria-hidden='true'></i> Confirmation");
            $("#lblMsgConfirmYesNo").html("<div class='row col-md-12'> Are you sure do you want to cancel? </div>");

            $("#btnYesConfirmYesNo").on('click').click(function () {
                CancelPO();
            });
            $("#btnNoConfirmYesNo").on('click').click(function () {
                $confirm.modal("hide");
            });
        }

        function CancelPO() {
                var $confirm = $("#modalConfirmYesNo");
                var tid=0;
                var poid=0;
                var url = window.location.pathname;
                if (window.location.href.indexOf("poid") >-1) {
                    var obj = location.href.split('?')[1].split('&');
                    if (obj.length > 0) {
                         poid = obj[0].split('=')[1];
                         tid = obj[1].split('=')[1];
                    }
                }
                if (location.href.split('?')[1] >0) {
                    tid = url.searchParams.get("tid");
                poid = url.searchParams.get("poid"); 
                }
            $.ajax({
                url: '<%=ResolveUrl("~/mOrders/PODetailsInfo.aspx/CancelPO") %>',
                data: "{}",
                dataType: "json",
                type: "POST",
                async: true,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    debugger;
                    var poid = data.d;
                    if (poid == "-1") {
                        showStickyToast(false, "Cannot cancel, as the Purchase order is configured in Inbound", false);
                        return false;
                    }
                    else if (poid == "1") {
                        showStickyToast(false, "Error while canceling", false);
                        return false;
                    }
                    else
                    {
                        $confirm.modal("hide");
                        // window.location.href = "PODetailsInfo.aspx?poid=" + poid + "&tid=" + tid + "&CancelStatus=true";
                        showStickyToast(true, "Successfully Cancelled", false);
                        setTimeout(function () {
                            
                            var s = true;
                            window.location.href = "PODetailsInfo.aspx?poid=" + poid + "&tid=" + tid + "&CancelStatus=true";
                            // location.reload();

                        }, 3000);

                        //location.reload();
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

        function lnkDeleteItem1_ClientClick() {
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
            debugger
            $(document).ready(function () {
                var tid=0;
                var poid=0;
                var url = window.location.pathname;
                if (window.location.href.indexOf("poid") >-1) {
                    var obj = location.href.split('?')[1].split('&');
                    if (obj.length > 0) {
                         poid = obj[0].split('=')[1];
                        // tid = obj[1].split('=')[1];
                    }
                }
                if (location.href.split('?')[1] >0) {
                    tid = url.searchParams.get("tid");
                poid = url.searchParams.get("poid"); 
                }
                $('#txtInvDate').datepicker({
                    dateFormat: 'dd-M-yy',              
                });
                //$('#txtInvDate').keydown(function () {
                //    return false;
                //});
                $("#<%= this.txtPODate.ClientID%>").datepicker({ dateFormat: 'dd-M-yy' });
              <%--  $("#<%= this.txtPODate.ClientID%>").keydown(function () {
                    return false;
                });--%>
       //   $("#txtMfgDate1").datepicker({ dateFormat: 'dd-M-yy' });

                  dateFormat: "dd-M-yy",
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
                $("#txtMfgDate1").datepicker({
                    dateFormat: "dd-M-yy",
                    maxDate: new Date(),
                    onSelect: function (selected) {
                        var instance = $(this).data("datepicker");
                        var date = $.datepicker.parseDate(instance.settings.dateFormat || $.datepicker._defaults.dateFormat, selected, instance.settings);
                        date.setDate(date.getDate() + 1);
                        $("#txtExpDate1").datepicker("option", "minDate", date, { dateFormat: "dd-M-yy" })
                    }
                });

                $("#txtExpDate1").datepicker({
                    dateFormat: "dd-M-yy",
                    //maxDate: new Date()
                });

                //Added by kashyap on 21/08/2017  to reslove the server issue 
                $('#txtMfgDate1, #txtExpDate1').keypress(function () {
                    return false;
                });  
                //$('#txtMfgDate1').keydown(function () {
                //    return false;
                //});
                //$('#txtExpDate1').keydown(function () {
                //    return false;
                //});

                try {
                    var TextFieldName = $("#<%= this.txtSearch.ClientID %>");
                    DropdownFunction(TextFieldName);
                    $('#<%=this.txtSearch.ClientID%>').autocomplete({
                    source: function (request, response) {
                        //alert('ffffffff');
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetMCodeBasedonSupplierWithOEMFor3PL") %>',
                            data: "{ 'prefix': '" + request.term + "','SuppleirID':'" +<%=this.hidSupplier.Value%> +"','TenantID':'" + document.getElementById('<%= this.hifTenant.ClientID %>').value + "','POHeaderID':'<%=ViewState["HeaderID"]%>'}",
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
                }).data("autocomplete")._renderItem = function (ul, item) {
                    // Inside of _renderItem you can use any property that exists on each item that we built
                    // with $.map above */
                    return $("<li></li>")
                        .data("item.autocomplete", item)
                        .append("<a>" + item.label + "" + item.description + "</a>")
                        .appendTo(ul)
                };
            } catch (Ex) { }

                var TextFieldName = $("#<%= this.atcPOType.ClientID %>");
                DropdownFunction(TextFieldName);
                $("#<%= this.atcPOType.ClientID %>").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadPOTypes") %>',
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
                        $("#<%=hifPOType.ClientID %>").val(i.item.val);
                    },
                    minLength: 0
                });
                var TextFieldName = $("#<%= this.atcSupplier.ClientID %>");
                DropdownFunction(TextFieldName);
                $("#<%= this.atcSupplier.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadSupplierDataFor3PL") %>',
                        data: "{ 'prefix': '" + request.term + "','TenantID':'" + document.getElementById('<%= this.hifTenant.ClientID %>').value + "','Type':'PO'}",//<=cp.TenantID%>
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

                    $("#<%=hidSupplier.ClientID %>").val(i.item.val);

            },
            minLength: 0
            });
                var TextFieldName = $("#<%= this.atcCurrency.ClientID %>");
                DropdownFunction(TextFieldName);
                $("#<%= this.atcCurrency.ClientID %>").autocomplete({
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

                    $("#<%=hidCurrency.ClientID %>").val(i.item.val);
            },
            minLength: 0
                });
                $("#<%= this.atcCurrency.ClientID %>").keypress(function (event) {
                    debugger;
                    var inputValue = event.charCode;
                    //alert(inputValue);
                    if (!((inputValue > 64 && inputValue < 91) || (inputValue > 96 && inputValue < 123) || (inputValue == 32) || (inputValue == 0))) {
                        event.preventDefault();
                    }
                });

                $("#<%=this.txtDateRequested.ClientID%>").datepicker({
                    dateFormat: "dd-M-yy",
                    minDate: 0,
                    onSelect: function (selected) {
                        $("#<%=this.txtDateDue.ClientID%>").datepicker("option", "minDate", selected, { dateFormat: "dd-M-yy" })
                    }
                });
                  $('.hasDatepicker').attr('readonly', 'true');
                var _minDate = new Date(1990, 1, 1, 0, 0, 0);
                if (document.getElementById('<%=this.txtDateRequested.ClientID%>').value != "") {
                     var date = document.getElementById('<%=this.txtDateRequested.ClientID%>').value;
                    _minDate = new Date(date.split('/')[2], parseInt(date.split('/')[1]) - 1, date.split('/')[0], 0, 0, 0, 0);
                }
                $("#<%=this.txtDateDue.ClientID%>").datepicker({
                    dateFormat: "dd-M-yy",
                    minDate: _minDate,
                    onSelect: function (selected) {
                        $("#<%=this.txtDateRequested.ClientID%>").datepicker("option", "maxDate", selected, { dateFormat: "dd-M-yy" })
                }
                });
                $('#<%=this.txtDateRequested.ClientID%>').keydown(function () {
                    return false;
                });
                $('#<%=this.txtDateDue.ClientID%>').keydown(function () {
                    return false;
                });
                try {
                    var TextFieldName = $('#atcPUoM');
                    DropdownFunction(TextFieldName);
                    $('#atcPUoM').autocomplete({
                        source: function (request, response) {
                            if (document.getElementById("hifMCode").value == "") {
                                showStickyToast(false, 'Select part number');
                                return false;
                            }
                            $.ajax({
                                url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadUoMWithQty") %>',
                                   data: "{ 'MaterialID': '" + document.getElementById("hifMCode").value + "'}",
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
                               $("#hifPUoM_Qty").val(i.item.val);
                               $("#hidUoMQty").val(i.item.val);
                           },
                           minLength: 0
                       });
                   } catch (ex) { }
                var TextFieldName = $('#atcKitPlanner');
                DropdownFunction(TextFieldName);
                $('#atcKitPlanner').autocomplete({
                    source: function (request, response) {
                        debugger;
                        if ($('#atcKitPlanner').val() == '') {
                            $("#hifKitPlanner").val("");
                        }
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadKitPlanner") %>',
                            data: "{ 'MaterialID': '" + document.getElementById("hifMCode").value + "','TenantID':'" + document.getElementById('<%= this.hifTenant.ClientID %>').value + "'}",
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
                                        debugger;
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
                           //alert(document.getElementById('hifKitPlanner').value);
                       },
                       minLength: 0
                });

                try {
                    var TextFieldName = $('#atcMCode');
                    DropdownFunction(TextFieldName);
                    $('#atcMCode').autocomplete({
                        source: function (request, response) {
                            $.ajax({
                                url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetMCodeBasedonSupplierWithOEMFor3PL") %>',
                            data: "{ 'prefix': '" + request.term + "','SuppleirID':'" +<%=this.hidSupplier.Value%> +"','TenantID':'" + document.getElementById('<%= this.hifTenant.ClientID %>').value +"','POHeaderID':'0'}",
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
                    select: function (e, i) {
                        $("#hifMCode").val(i.item.val);
                        try {
                            document.getElementById("atcPUoM").value = "";
                            document.getElementById("hifPUoM_Qty").value = "";
                            document.getElementById("atcKitPlanner").value = "";
                        } catch (err) { }

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
            } catch (err) { }

                ReplaceTableRowForLineItems();
                $(".DateBoxCSS_small").datepicker({ dateFormat: "dd/mm/yy" });
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

                        $("#hifInvCurrency").val(i.item.val);
                    },
                    minLength: 0
                });
                $("#atcInvCurrency").keypress(function (event) {
                    debugger;
                    var inputValue = event.charCode;
                    //alert(inputValue);
                    if (!((inputValue > 64 && inputValue < 91) || (inputValue > 96 && inputValue < 123) || (inputValue == 32) || (inputValue == 0))) {
                        event.preventDefault();
                    }
                });
                var TextFieldName = $('#atcInvCountryofOrigin');
                DropdownFunction(TextFieldName);
                $('#atcInvCountryofOrigin').autocomplete({
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
                        $("#hifInvCountryofOrigin").val(i.item.val);
                    },
                    minLength: 0
                });

                var TextFieldName = $('#atcMaterialMaster_InvUoM');
                DropdownFunction(TextFieldName);
                $('#atcMaterialMaster_InvUoM').autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadUoMWithQty") %>',
                            data: "{ 'MaterialID': '" + document.getElementById("<%=hifMaterialMasterId.ClientID%>").value + "'}",
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

                        $("#hifMaterialMaster_InvUoM").val(i.item.val);
                        $("#hidInvUoMQty").val(i.item.label.split('/')[1]);
                    },
                    minLength: 0
                });

                var TextFieldName = $('#atcInvoiceNumber');
                DropdownFunction(TextFieldName);
                $('#atcInvoiceNumber').autocomplete({
                    source: function (request, response) {

                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadInvNumbers") %>',
                            data: "{ 'POHeaderID': '" +<%=ViewState["HeaderID"].ToString()%> + "','PODetailsID':'" + document.getElementById('<%=this.hifPODetailsID.ClientID%>').value + "','SupplierInvoiceDetailsID':'" + document.getElementById('hifSupplierInvoiceID').value + "'}",
                            dataType: "json",
                            type: "POST",
                            async: true,
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                //alert(data.d)
                                if (data.d == '') {
                                    showStickyToast(false, 'No Invoice', false);
                                    $('#atcInvoiceNumber').value = '';
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

                        $("#hifSupplierInvoiceID").val(i.item.val);
                        
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

                        $("#<%=hifTenant.ClientID %>").val(i.item.val);
                        $("#<%=atcSupplier.ClientID %>").val("");
                    },
                    minLength: 0
                });

            });
        
        }
   
    </script>
  
    <script type="text/javascript">
        function closetooltip() {
            $('.tooltip').remove();
        }

        $(".DateTimecss").datepicker({ dateFormat: "dd-M-yy" });
        function checkvalid() {
            $('#chkDelete.ClientID%>').checked;
        }
        function ReplaceTableRowForLineItems() {

            var LineItemtable = document.getElementById('tbLineItemsDetails');
            //alert(LineItemtable);

            if (LineItemtable != null) {
                var IndexRow = LineItemtable.getElementsByClassName('gvLightSteelBlueNew_pager');

                if (!(IndexRow == null || IndexRow.length == 0)) {
                    LineItemtable.appendChild(IndexRow[0]);
                    //var hhhh = LineItemtable.insertRow(3);
                    //hhhh.appendChild(IndexRow[0]);
                    //LineItemtable.insertRow(3).ap(IndexRow[0], hhhh);
                    //hhhh.innerHTML = IndexRow[0].innerHTML;
                }
            }

            $('.accordino_icon_Right').on('click', function () {
                $(this).children('.downarrow ').toggleClass('isRotateright');
            });

        }
  </script>
    
    <script>
        function CheckPUoMQty(TextBox) {
           /* var UoMQty = parseFloat(document.getElementById('hidUoMQty').value) * 100;
            var RequireQty = parseFloat(TextBox.value) * 100;
            if (RequireQty % UoMQty != 0) {
                showStickyToast(true, "PO Qty. should be multiple of PUoMQty.");
                TextBox.value = "";
                return;
            }*/
            CheckDecimal(TextBox);
        }
    

        function CheckInvUoMQty(TextBox) {
           /* var UoMQty = parseFloat(document.getElementById('hidInvUoMQty').value) * 100;
            var RequireQty = parseFloat(TextBox.value) * 100;
            if (RequireQty % UoMQty != 0) {
                showStickyToast(true, "Inv. Qty. should be multiple of Inv.UoMQty.");
                TextBox.value = "";
                return;
            }*/
            CheckDecimal(TextBox);
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

        function CheckInvNum(textbox) {
           // alert('ssssss');
            $.ajax({
                url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadInvoiceNumbers") %>',
                 data: "{ 'prefix': '" +'' + "','POHeaderID': '" +<%=ViewState["HeaderID"].ToString()%> + "'}",
                 dataType: "json",
                 type: "POST",
                 async: true,
                 contentType: "application/json; charset=utf-8",
                 success: function (data) {
                     //alert(data.d);
                     var numbers = data.d.toString();
                    // alert(numbers);
                    // alert(numbers.split(','))
                     var invNumber = numbers.split(',');
                     //alert(textbox.value);
                     //textbox.value = "";
                    // alert(invNumber.length);
                     var check = false;
                     if (textbox.value != "") {

                         for (n = 0; n < invNumber.length; n = n + 2) {
                             //alert(invNumber[n]);
                             if (invNumber[n] == textbox.value) {
                                 check = true;
                                 break;
                             }
                         }
                     }
                     else
                         check = true;
                     //alert(check);
                     if (check == false) {
                         textbox.value = "";
                         textbox.focus();
                     }
                 }

             });


        }



        function MSPConfifure(name) {

            var mmid = 0;
            try
            {
                mmid = document.getElementById("hifMCode").value;
                if (mmid == ''||mmid==null) {
                    mmid = 0;
                }
            }
            catch (err) {
            }
            //alert(mmid);
            $.ajax({
                url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/MaterialConfigurationService") %>',
                data: "{ 'MaterialId': '" + mmid + "','TenantID':'" +<%=hifTenant.Value%> +"'}",
                dataType: "json",
                type: "POST",
                async: true,
                contentType: "application/json; charset=utf-8",
                success: function (data)
                {
                    //alert(data.d);
                    configure(name, data.d);
                   // response(data.d);
                }
            });

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
                         var ToolTip = 'configured kitIDs to this Part Number\n\n    ' + data.d;
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
                         //kitImage.setAttribute("title", ToolTip);
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
            
           
            for(var item=0;item<listOfparames.length;item++)
            {
                // alert(listOfparames[item]);
                try{
                    document.getElementById(listOfparames[item]).style.display = "none";
                }catch(err)
                {
                    }
            }
            listOfparames = paramNames[1].split(',');

            for (var item = 0; item < listOfparames.length; item++) {
                try{
               
                    document.getElementById(listOfparames[item]).style.display = "block";
                } catch (err) {
                }
            }
          

        }
    </script>

    <script >

        function getFileName(path) {
            debugger;
            return path.match(/[-_\w]+[.][\w]+$/i)[0];
        }

        function checkpo() {
            debugger;
            var ponumber = $("#<%=txtPONumber.ClientID %>").val();
            var uploadcontrol = $("#<%=FUImport.ClientID %>").val();
         
            if (uploadcontrol=="")
            {
                showStickyToast(false, 'Please Upload File');
                return false;
            }
            var j = (uploadcontrol.split(/[\\\/]/).pop());
            var filename = uploadcontrol.substring(uploadcontrol.lastIndexOf('/') + 1);
            var file = getFileName(uploadcontrol);

            filename = file.split('.')[0];
            var fileext = file.split('.')[1];
               if(fileext!='xlsx' && fileext!='xls' )
           {
             showStickyToast(false, 'file should be excel with extension .xlsx');
                return false;
           }
            //alert(filename);
            if (ponumber != filename.split('_')[4]) {
                showStickyToast(false, 'Filename Is not valid');

                return false;

            }


        }
         
         function CheckIsDelted(checkBox) {
             if (checkBox.checked) {
                 alert('Are you sure want to delete the Purchase Order?');
             }
         }

         function CheckInvoices() {
            // alert('sssssss');
             $.ajax({
                 url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/CheckInvoice") %>',
                 data: "{ 'POHeaderID': '" + <%=ViewState["HeaderID"].ToString()%> + "'}",
                 dataType: "json",
                 type: "POST",
                 async: true,
                 contentType: "application/json; charset=utf-8",
                 success: function (data) {
                     if (data.d == "false") {
                        // alert(data.d);
                         document.getElementById("atcInvUoM").style.display = "none";
                         document.getElementById("atcInvoiceNumber").style.display = "none";
                         document.getElementById("txtInvQty").style.display = "none";
                         document.getElementById("txtInvdiper").style.display = "none";
                         document.getElementById("txtInvVATCode").style.display = "none";
                     }
                     else {
                        // alert(data.d);
                         document.getElementById("atcInvUoM").style.display = "block";
                         document.getElementById("atcInvoiceNumber").style.display = "block";
                         document.getElementById("txtInvQty").style.display = "block";
                         document.getElementById("txtInvdiper").style.display = "block";
                         document.getElementById("txtInvVATCode").style.display = "block";
                     }
                 }
             });
         }


        function DisableAstricks(data) {
            //alert(data);
            debugger;
            var mfgDate = data.split(',')[0];
            var expDate = data.split(',')[1];
            var batchNo = data.split(',')[2];
            var projRefNo = data.split(',')[3];
            var SerialNo = data.split(',')[4];
            var MRP = data.split(',')[5];

            // alert($("#MainContent_OrdersContent_hdnmfgdate").val());
            if (mfgDate == "1") {
                $('.lblmfgdate').css('display', 'block');
                //alert('show');
            }
            else {
                $('.lblmfgdate').css('display', 'none');
                //alert('hide');
            }

            if (expDate == "1") {
                $('.lblexpdate').css('display', 'block');
                //alert('show');
            }
            else {
                $('.lblexpdate').css('display', 'none');
                // alert('hide');
            }
            // alert($("#MainContent_OrdersContent_hdnbatchno").val());
            if (batchNo == "1") {
                $('.lblBatchNo').css('display', 'block');
                //alert('show');
            }
            else {
                $('.lblBatchNo').css('display', 'none');
                //alert('hide');
            }

            if (projRefNo == "1") {
                $('.lblProjrefno').css('display', 'block');
                //alert('show');
            }
            else {
                $('.lblProjrefno').css('display', 'none');
                //alert('hide');
            }

            if (SerialNo == "1") {
                $('.lblSerialNo').css('display', 'block');
                //alert('show');
            }
            else {
                $('.lblSerialNo').css('display', 'none');
                //alert('hide');
            }
           
            if (MRP == "1") {
                $('.lblMRP').css('display', 'block');
                //alert('show');
            }
            else {
                $('.lblMRP').css('display', 'none');
                //alert('hide');
            }

        }
    </script>

    <script>
        
        $(document).ready(function () {
            
            $("#divSupplierItemList").dialog({
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
                    $(".ui-dialog").fadeOut(500);
                    $(document).unbind('scroll');
                    $('body').css({ 'overflow': 'visible' });
                },

                //position: 'center',
                open: function (event, ui) {
                    $(this).parent().appendTo("#disputeDivSupplierItemList");
                    $(".ui-dialog").hide().fadeIn(500);

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
            $("#divSupplierItemList").dialog('close');
            
        }

        function btnclose() {
             if ($("#<%=this.hdnIsKitParent.ClientID%>").val() == "0") {
                $("#btnInvoice").hide();
            }
        }

        function openDialog() {
         debugger;
            // alert("from");
            // BuildDialogBox(GoodsMovementID)
            
            //if ($('#lnkInvoiceIDs.ClientID%>').val() == "Add Invoice") {

            //}

           

            $("#divSupplierItemList").dialog("option", "title", "Supplier Invoice Details");
      
            $("#divSupplierItemList").dialog('open');

            NProgress.start();

            $("#divSupplierItemList").block({
                message: '<img src="<%=ResolveUrl("~") %>Images/async_order.gif" />',
                 css: { border: '0px' },
                 fadeIn: 0,
                 fadeOut: 0,
                 overlayCSS: { backgroundColor: '#ffffff', opacity: 1 }
             });
            
        }

        function unblockDialog() {
            $("#divSupplierItemList").unblock();
        }
      
    </script>

    


    <asp:HiddenField ID="hdnmfgdate" runat="server" Value="0" />
    <asp:HiddenField ID="hdnexpdate" runat="server" Value="0" />
    <asp:HiddenField ID="hdnbatchno" runat="server" Value="0" />
    <asp:HiddenField ID="hdnprojrefno" runat="server" Value="0"  />
     <asp:HiddenField ID="hdnSerialNo" runat="server" Value="0"  />
    <asp:HiddenField ID="hdnMRP" runat="server" Value="0" />
    <asp:UpdatePanel ID="upnlPODetails" ChildrenAsTriggers="true" runat="server" UpdateMode="Always">
        <ContentTemplate>

  
            <div class="container">
               <div flex between>
                    <inv-status><asp:Label ID="ltPOStatus" runat="server" CssClass="BigCapsHeading" /></inv-status>
                    <div><asp:LinkButton ID="lnkback" runat="server" CssClass="btn btn-primary backtolist mb0 bbttn" SkinID="lnkButEmpty" PostBackUrl="~/mOrders/POList.aspx"><i class="material-icons vl">arrow_back</i>Back to List</asp:LinkButton></div></div>
                <div class="gap5"></div>
                <table border="0" cellpadding="0" cellspacing="0" align="center">

         
                <tr>
                    <td colspan="3">
                      
                        <asp:Literal ID="lblStatus" runat="server" />
                    </td>
                </tr>

                <tr class="">
                    <td colspan="3">
                       <%-- <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="dvPOHDHeader" style="">Inward Header Details --%>
                         <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="dvPOHDHeader" style=""> <%= GetGlobalResourceObject("Resource", "InwardHeaderDetails")%>
                            <i class="material-icons downarrow right">keyboard_arrow_down</i>
                        </div>

                        <div class="ui-Customaccordion" id="dvPOHDBody">
                            <table border="0" cellspacing="0" cellpadding="0" width="100%" style="padding-top: 20px; padding-left: 20px;">


                                <tr>

                                    <td colspan="3" align="center">

                                        <asp:Panel runat="server" ID="pnlHeaderDetails" DefaultButton="lnkUpdate">
                                            <div class="converttodiv">
                                                <div border="0" cellspacing="0" cellpadding="3" align="left" width="100%">


                                                    <div class="row">

                                                        <div class="col m4 s4" align="left">

                                                            <div class="">
                                                                <asp:UpdatePanel ChildrenAsTriggers="true" ID="uppoNumber" UpdateMode="Conditional" runat="server">
                                                                    <Triggers>
                                                                        <asp:PostBackTrigger ControlID="IbutNew" />
                                                                    </Triggers>
                                                                    <ContentTemplate>
                                                                        <div class="flex">
                                                                            <asp:HiddenField ID="hdnPO" runat="server" />
                                                                            <asp:Label ID="lblPO" runat="server" Text="0" Visible="false"></asp:Label>
                                                                            <asp:RequiredFieldValidator ID="rfvPONumber" runat="server" Enabled="false" ValidationGroup="save" ControlToValidate="txtPONumber" Display="Dynamic" />




                                                                            <asp:TextBox ID="txtPONumber" runat="server" Enabled="false" />
                                                                            <asp:ImageButton ID="IbutNew" runat="server" OnClick="IbutNew_Click" ImageUrl="~/Images/blue_menu_icons/add_new.png" ToolTip="Generate New PO Number" />
                                                                            <label>

                                                                                <%--Inward Order Ref. No.--%>
                                                                                <%= GetGlobalResourceObject("Resource", "InwardOrderRefNo")%>
                                                                                <asp:Literal ID="lclPONumber" runat="server" />

                                                                            </label>


                                                                        </div>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </div>
                                                        </div>



                                                        <div class="col m4 s4">
                                                            <div class="flex">


                                                                <asp:TextBox runat="server" ID="atcPOType" required="" />
                                                                <asp:HiddenField runat="server" ID="hifPOType" />

                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ValidationGroup="save" ControlToValidate="atcPOType" Display="Dynamic" />
                                                                <%-- <label><span style="color:red;margin-left:-0.3em"></span>Order Type<asp:Literal  runat="server" ID="ltPOType"/></label> --%>
                                                                <label><span style="color: red; margin-left: -0.3em"></span><%= GetGlobalResourceObject("Resource", "OrderType")%><asp:Literal runat="server" ID="ltPOType" /></label>
                                                            </div>
                                                        </div>

                                                       <%-- <div class="col m4 s4">
                                                            <div class="right">
                                                                <asp:Label ID="ltPOStatus" runat="server" CssClass="BigCapsHeading" />
                                                            </div>
                                                        </div>--%>

                                                    </div>

                                                    <div class="row">
                                                        <div class="col m4 s4">
                                                            <div class="flex">

                                                                <asp:TextBox ID="txtPODate" runat="server" required="" />
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="save" ControlToValidate="txtPODate" Display="Dynamic" />
                                                             
                                                                <label><span style="color: red; margin-left: -0.3em"></span><%= GetGlobalResourceObject("Resource", "InwardDate")%><asp:Literal ID="lclPODate" runat="server" /></label>
                                                              
                                                            </div>
                                                        </div>
                                                        <div class="col m4 s4">


                                                            <%--         <asp:RequiredFieldValidator ID="rfvTenant" runat="server" ValidationGroup="save" ControlToValidate="txtTenant" Display="Dynamic" InitialValue="0" />
                                       <label> <span style="color:red">*</span> Tenant<asp:Literal ID="ltTenant" runat="server" /></label>--%>
                                                            <div class="flex">
                                                                <asp:TextBox ID="txtTenant" runat="server" required=""></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="rfvTenant" runat="server" ValidationGroup="save" ControlToValidate="txtTenant" Display="Dynamic" InitialValue="0" />
                                                                <%-- <label> Tenant<asp:Literal ID="ltTenant" runat="server" /></label> <span class="errorMsg" ></span>--%>
                                                                <label><%= GetGlobalResourceObject("Resource", "Tenant")%><asp:Literal ID="ltTenant" runat="server" /></label>
                                                                <span class="errorMsg"></span>
                                                                <asp:HiddenField runat="server" ID="hifTenant" />
                                                            </div>

                                                        </div>
                                                        <div class="col m4 s4">
                                                            <div class="flex">

                                                                <%--                                            <asp:RequiredFieldValidator ID="Suppliervalidate" runat="server" ValidationGroup="save" ControlToValidate="atcSupplier" Display="Dynamic"/>
                                           <label><span style="color:red;margin-left:-0.3em">*</span> Supplier<asp:Literal ID="lclSupplier" runat="server"  /></label>--%>


                                                                <asp:TextBox ID="atcSupplier" runat="server" required=""> </asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="Suppliervalidate" runat="server" ValidationGroup="save" ControlToValidate="atcSupplier" Display="Dynamic" />
                                                                <%--<label><span style="color:red;margin-left:-0.3em"></span> Supplier<asp:Literal ID="lclSupplier" runat="server"  /></label>--%>
                                                                <label><span style="color: red; margin-left: -0.3em"></span><%= GetGlobalResourceObject("Resource", "Supplier")%><asp:Literal ID="lclSupplier" runat="server" /></label>
                                                                <asp:HiddenField runat="server" ID="hidSupplier" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col m4 s4">
                                                            <div class="flex">

                                                                <%--<label>Currency <asp:Literal ID="lclCurrency" runat="server" /></label>--%>


                                                                <asp:TextBox ID="atcCurrency" runat="server" required=""></asp:TextBox>
                                                                <%-- <label>Currency <asp:Literal ID="lclCurrency" runat="server" /></label>--%>
                                                                <label><%= GetGlobalResourceObject("Resource", "Currency")%><asp:Literal ID="lclCurrency" runat="server" /></label>
                                                                <asp:HiddenField runat="server" ID="hidCurrency" />
                                                            </div>
                                                        </div>

                                                        <div class="col m4 s4" align="left">
                                                            <div class="flex">
                                                                <%--<label>Total Value<asp:Literal ID="lclTotalValue" runat="server" /></label>--%>
                                                                <asp:TextBox ID="txtTotalValue" runat="server" onKeyPress=" return checkDec(this,event)" onblur="CheckDecimal(this)" MaxLength="30" required="required" />
                                                                <%-- <label>Total Value<asp:Literal ID="lclTotalValue" runat="server" /></label>--%>
                                                                <label><%= GetGlobalResourceObject("Resource", "TotalValue")%><asp:Literal ID="lclTotalValue" runat="server" /></label>
                                                            </div>
                                                        </div>
                                                        <div class="col m4 s4" align="left">
                                                            <div class="flex">


                                                                <asp:TextBox ID="txtDateRequested" runat="server" onblur="" required="" />
                                                                <%-- <label>  Date Requested  <asp:Literal ID="lclDateRequested" runat="server"/></label>--%>
                                                                <label>
                                                                    <%= GetGlobalResourceObject("Resource", "DateRequested")%>
                                                                    <asp:Literal ID="lclDateRequested" runat="server" /></label>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="row">


                                                        <div class="col m4 s4" align="left">
                                                            <div class="flex">
                                                                <asp:TextBox ID="txtDateDue" runat="server" required="" />
                                                                <%-- <label>Date Due<asp:Literal ID="lclDateDue" runat="server"/></label>--%>
                                                                <label><%= GetGlobalResourceObject("Resource", "DateDue")%><asp:Literal ID="lclDateDue" runat="server" /></label>
                                                            </div>
                                                        </div>
                                                        <div class="col m4 s4" align="left" style="display: none">
                                                            <div class="flex">
                                                                <%-- <label> Exchange Rate<asp:Literal ID="lclExchangeRate" runat="server"  /></label>--%>
                                                                <asp:TextBox ID="txtExchangeRate" runat="server" onKeyPress=" return checkDec(this,event)" onblur="CheckDecimal(this)" MaxLength="30" required="" />
                                                                <label>
                                                                    <%= GetGlobalResourceObject("Resource", "ExchangeRate")%>
                                                                    <asp:Literal ID="lclExchangeRate" runat="server" /></label>
                                                            </div>
                                                        </div>
                                                        <div class="col m4 s4" align="left" style="display: none">
                                                            <div class="flex">
                                                                <asp:TextBox ID="txtPOTax" runat="server" onKeyPress=" return checkDec(this,event)" onblur="CheckDecimal(this)" required="" />
                                                                <%-- <label>PO Tax <asp:Literal ID="ltPOTax" runat="server" /> </label>--%>
                                                                <label>
                                                                    <%= GetGlobalResourceObject("Resource", "POTax")%>
                                                                    <asp:Literal ID="ltPOTax" runat="server" />
                                                                </label>
                                                            </div>
                                                        </div>

                                                        <div class="col m4 s4">
                                                            <div class="flex">

                                                                <div class="f-flex">
                                                                    <%--   <label>Instructions
                                                        <asp:Label ID="lbInstructions" runat="server"></asp:Label></label>--%>
                                                                </div>
                                                                <div class="f-flex form">
                                                                    <asp:TextBox ID="txtareaInstructions" runat="server" TextMode="MultiLine"  required=""></asp:TextBox>
                                                                    <%-- <label>Instructions--%>
                                                                    <label>
                                                                        <%= GetGlobalResourceObject("Resource", "Instructions")%>
                                                                        <asp:Label ID="lbInstructions" runat="server"></asp:Label></label>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="col m4 s4">
                                                            <div class="flex">
                                                                <div class="f-flex">
                                                                    <%--<label>Remarks
                                                        <asp:Label ID="Label3" runat="server"></asp:Label></label>--%>
                                                                </div>
                                                                <div class="f-flex form">
                                                                    <asp:TextBox ID="txtareaRemarks" runat="server" MaxLength="40" TextMode="MultiLine" Style="font-family: inherit"  required="" />
                                                                    <%--   <label>Remarks--%>
                                                                    <label>
                                                                        <%= GetGlobalResourceObject("Resource", "Remarks")%>
                                                                        <asp:Label ID="Label3" runat="server"></asp:Label></label>
                                                                </div>
                                                            </div>
                                                            <asp:Literal ID="ltheaderId" runat="server" Visible="false" />
                                                        </div>

                                                    </div>



                                                    <div class="row">
                                                        <div class="col m4 s4" colspan="4" align="right" style="padding-right: 19px">
                                                            <asp:CheckBox ID="chkIsActive" runat="server" Text="Active" Visible="false" />&nbsp;
                            <asp:CheckBox ID="chkIsDeleted" onclick="CheckIsDelted(this);" runat="server" Text="Delete" Visible="false" />
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                       
                                                        <div class="">
                                                            <div class="col m4 s4 offset-m8 offset-s8">
                                                                <div align="right" style="padding-right: 10px;">
                                                                    <%--   OnClientClick="return confirm('Are you sure want to close?');" return confirm('Are you sure want to cancel?'); OnClick="lnkCancel_Click"--%>
                                                                    <%-- <asp:LinkButton ID="lnkClose" CssClass="btn btn-primary" Visible="false" runat="server"  OnClick="inkClose_Click">
                                              Close<%=MRLWMSC21Common.CommonLogic.btnfaClear%></asp:LinkButton>&nbsp;&nbsp;&nbsp;--%>
                                                                    <asp:LinkButton ID="lnkClose" CssClass="btn btn-primary" Visible="false" runat="server" OnClick="inkClose_Click">
                                              <%= GetGlobalResourceObject("Resource", "Close")%><%=MRLWMSC21Common.CommonLogic.btnfaClear%></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                   <%--<asp:LinkButton ID="lnkCancel" CssClass="btn btn-primary" runat="server"  Text="PO Cancel" OnClientClick="showConfirmAlert();" >
                                       Cancel <%=MRLWMSC21Common.CommonLogic.btnfaClear %>
                                   </asp:LinkButton>--%>
                                                                    <asp:LinkButton ID="lnkCancel" CssClass="btn btn-primary" runat="server" Text="PO Cancel" OnClientClick="showConfirmAlert(); return false;">
                                        <%= GetGlobalResourceObject("Resource", "Cancel")%> <%=MRLWMSC21Common.CommonLogic.btnfaClear %>
                                                                    </asp:LinkButton>
                                                                    &nbsp;&nbsp;&nbsp;
                                                       <%--  <asp:LinkButton ID="lnkClear" CssClass="btn btn-primary" runat="server" Text="PO Clear" OnClick="lnkClear_Click">
                                       Cancel <%=MRLWMSC21Common.CommonLogic.btnfaClear %> &nbsp;&nbsp;&nbsp;
                                   </asp:LinkButton>--%>
                                                                    <asp:LinkButton ID="lnkClear" CssClass="btn btn-primary" runat="server" Text="PO Clear" OnClick="lnkClear_Click">
                                       <%= GetGlobalResourceObject("Resource", "Cancel")%> <%=MRLWMSC21Common.CommonLogic.btnfaClear %> &nbsp;&nbsp;&nbsp;
                                                                    </asp:LinkButton>
                                                                    <%--     <asp:LinkButton ID="lnkUpdate" CssClass="btn btn-primary" OnClientClick="showAsynchronus();" ValidationGroup="FleetCordValidation" OnClick="lnkUpdate_Click" runat="server">
                                      

                                  </asp:LinkButton>--%>
                                                                    <asp:LinkButton ID="lnkUpdate" CssClass="btn btn-primary" ValidationGroup="FleetCordValidation" OnClick="lnkUpdate_Click" runat="server">
                                      

                                                                    </asp:LinkButton>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>
                                        </asp:Panel>
                                    </td>

                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="accordinoGap"></td>
                </tr>
                <tr class="">

                    <td colspan="3" id="tdPODetails" runat="server">
                       <%-- <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="dvMDHeader" style="">Material Details--%>
                         <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="dvMDHeader" style=""><%= GetGlobalResourceObject("Resource", "MaterialDetails")%>
                             <i class="material-icons downarrow right">keyboard_arrow_down</i>
                        </div>
                        <div class="ui-Customaccordion" id="dvMDBody">
                            <table border="0" cellspacing="0" cellpadding="0" width="100%" style="padding-top: 10px; padding-left: 10px;" id="tbLineItemsDetails">

                            
                                <tr>
                                    <td colspan="3">
                                       <asp:Panel ID="pnlPodetails" runat="server" DefaultButton="Inksearch">
                                           <div class="flex__ right">
                                               
                                                   <div class="flex"><asp:TextBox runat="server" Text="Search Part Number..." ID="txtSearch" /></div>
                                                   <div class="flex__">
                                                       <asp:LinkButton runat="server" CssClass="btn btn-primary" ID="Inksearch" OnClick="Inksearch_Click"><%= GetGlobalResourceObject("Resource", "Search")%> <span class=" fa fa-search"></span></asp:LinkButton>

                                                       <asp:LinkButton CssClass="btn btn-primary" ID="lnkaddnewrow" runat="server" Visible="false" OnClick="btaddnewrow_Click1"><%= GetGlobalResourceObject("Resource", "AddMaterial")%> <%=MRLWMSC21Common.CommonLogic.btnfaNew %></asp:LinkButton>

                                                       <asp:LinkButton ID="ImgExport" CssClass="btn btn-primary" runat="server" OnClick="ImgExport_Click"> <%= GetGlobalResourceObject("Resource", "DownloadASN")%> <%=MRLWMSC21Common.CommonLogic.btnfaExcel %></asp:LinkButton>
                                                   </div>
                                           </div>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr class="gridpager">
                                    <td colspan="3" align="center">

                                        <asp:Panel ID="pnlgvPODetails" runat="server"  HorizontalAlign="center">
                                            <asp:Literal ID="ltgridstatus" runat="server"></asp:Literal>

                                             <asp:GridView ID="GVPODetails" SkinID="gvLightSteelBlueNew" runat="server" PagerStyle-HorizontalAlign="left" OnRowCommand="GVPODetails_RowCommand" AllowPaging="true" PageSize="10" OnPageIndexChanging="GVPODetails_PageIndexChanging" AutoGenerateColumns="False" Width="624px" ShowFooter="True" OnRowEditing="GVPODetails_RowEditing" OnRowDataBound="GVPODetails_RowDataBound" OnRowCancelingEdit="GVPODetails_RowCancelingEdit" OnRowUpdating="GVPODetails_RowUpdating">
                                                   <%--<PagerStyle cssClass="gridpager" HorizontalAlign="Right" />--%>
                                                <Columns>

                                                    <%--<asp:TemplateField ItemStyle-Width="100" HeaderText="Line No." >--%>
                                                    <asp:TemplateField  HeaderText="<%$Resources:Resource,LineNo%>" >
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltlinenumber" Text='<%#DataBinder.Eval(Container.DataItem, "LineNumber").ToString() %>' />
                                                            <asp:Literal runat="server" ID="ltpodetails" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "PODetailsID").ToString() %>' />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:Literal runat="server" ID="ltpodetailsex" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "PODetailsID").ToString() %>' />
                                                            <asp:Literal runat="server" ID="ltlinenumberEdit" Text='<%#DataBinder.Eval(Container.DataItem, "LineNumber").ToString() %>' />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField  Visible="false" HeaderText="<%$Resources:Resource,ReqNumber%>" >
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltreqnumber" Text='<%# DataBinder.Eval(Container.DataItem, "RequirementNumber").ToString() %>' />

                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox runat="server" ID="txtreqnumber" onfocus="javascript:MSPConfifure(this)" onKeypress="return checkSpecialChar(event)" Width="80" Text='<%# DataBinder.Eval(Container.DataItem, "RequirementNumber").ToString() %>' />

                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$Resources:Resource,Part%>" >
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltmaterialcode" Text='<%# DataBinder.Eval(Container.DataItem, "MCode").ToString() %>' />
                                                            <br />
                                                            <%--<asp:Label CssClass="BOMPartNoHead" runat="server" ID="lbOEMPartNumber" Text='<%# DataBinder.Eval(Container.DataItem, "oempartno").ToString() %>'></asp:Label>--%>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <div class="gridInput">
                                                            <asp:RequiredFieldValidator ID="rfvMCode" runat="server" ValidationGroup="UpdatePODetails" ControlToValidate="atcMCode" Display="Dynamic"/>
                                                          <span class="errorMsg"></span> <asp:TextBox ID="atcMCode" runat="server" SkinID="txt_Hidden_Req_Auto" onfocus="javascript:MSPConfifure(this)" Width="130" ClientIDMode="Static" onblur="javascript:MSPConfifure(this)" Text='<%# DataBinder.Eval(Container.DataItem, "MCode").ToString() %>' />
                                                            <asp:HiddenField ID="hifMCode" runat="server" ClientIDMode="Static" Value='<%# DataBinder.Eval(Container.DataItem, "MaterialMasterID").ToString() %>' />
</div>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>

                                                   <%-- <asp:TemplateField ItemStyle-Width="100" HeaderText="BUoM/ Qty." Visible="false">--%>
                                                     <asp:TemplateField HeaderText="<%$Resources:Resource,BUoMQty%>" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltBUoM" Text='<%# String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "BUoM").ToString(),DataBinder.Eval(Container.DataItem, "BUoMQty").ToString())  %>' />

                                                        </ItemTemplate>

                                                    </asp:TemplateField>
                                                   <%-- <asp:TemplateField ItemStyle-Width="100" HeaderText="UoM/ Qty." >--%>
                                                    <asp:TemplateField HeaderText="<%$Resources:Resource,UoMQty%>" >
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" Visible="false" ID="ltpuom" Text='<%#String.Format("{0}-{1}",DataBinder.Eval(Container.DataItem, "PUoM").ToString(),DataBinder.Eval(Container.DataItem, "PUoMQty").ToString())   %>' />
                                                            <asp:Literal runat="server" ID="ltpuom_qty" Text='<%# String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "PUoM").ToString(),DataBinder.Eval(Container.DataItem, "PUoMQty").ToString())  %>' />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <div class="gridInput">
                                                            <asp:RequiredFieldValidator ID="rfvPUoM" runat="server" ValidationGroup="UpdatePODetails" ControlToValidate="atcPUoM" Display="Dynamic"/>
                                                           <span class="errorMsg"></span><asp:TextBox ID="atcPUoM" SkinID="txt_Hidden_Req_Auto" runat="server" Width="80" ClientIDMode="Static" Text='<%# String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "PUoM").ToString(),DataBinder.Eval(Container.DataItem, "PUoMQty").ToString())=="/"?"":String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "PUoM").ToString(),DataBinder.Eval(Container.DataItem, "PUoMQty").ToString())  %>' />
                                                            <asp:HiddenField ID="hifPUoM_Qty" runat="server" ClientIDMode="Static" Value='<%#DataBinder.Eval(Container.DataItem, "MaterialMaster_PUoMID").ToString() %>' />
                                                            <input id="hidUoMQty" type="hidden" value='<%# DataBinder.Eval(Container.DataItem, "PUoMQty").ToString() %>' />
                                                       </div>
                                                                </EditItemTemplate>
                                                    </asp:TemplateField>
                                                  <%--  <asp:TemplateField ItemStyle-Width="100" HeaderText="Kit ID">--%>
                                                      <asp:TemplateField HeaderText="<%$Resources:Resource,KitID%>" >
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltKitPlanner" Text='<%#DataBinder.Eval(Container.DataItem, "KitCode").ToString() %>' />
                                                                  <asp:Literal runat="server" ID="ltIsKitParent" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "IsKitParent").ToString() %>' />
                                                    
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:Image ImageUrl="../Images/kit.gif" ClientIDMode="Static" Visible="true" ID="imgkit" runat="server" />
                                                            <asp:TextBox ID="atcKitPlanner" SkinID="txt_Hidden_Req_Auto" runat="server" ClientIDMode="Static" Width="60" Text='<%#DataBinder.Eval(Container.DataItem, "KitCode").ToString() %>' />
                                                            <asp:HiddenField ID="hifKitPlanner" runat="server" ClientIDMode="Static" Value='<%#DataBinder.Eval(Container.DataItem, "KitPlannerID").ToString() %>' />

                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <%-- <asp:TemplateField ItemStyle-Width="100" HeaderText="Kit.Qty" >--%>
                                                    <asp:TemplateField HeaderText="<%$Resources:Resource,KitQty%>" >
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltKitQty" Text='<%#DataBinder.Eval(Container.DataItem, "KitQty").ToString() %>' />
                                                        </ItemTemplate>

                                                    </asp:TemplateField>
                                                   <%--  <asp:TemplateField ItemStyle-Width="100" HeaderText="Quantity" >--%>
                                                    <asp:TemplateField HeaderText="<%$Resources:Resource,Quantity%>" >
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltTotalqty" Text='<%# DataBinder.Eval(Container.DataItem, "POQuantity").ToString() %>' />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <div class="gridInput">
                                                            <asp:RequiredFieldValidator ID="rfvTotalqty" runat="server" ValidationGroup="UpdatePODetails" ControlToValidate="txtTotalqty" Display="Dynamic"/>
                                                            <span class="errorMsg"></span><asp:TextBox runat="server" ClientIDMode="Static" ID="txtTotalqty" onKeyPress="return checkDec(this,event)" onblur="CheckPUoMQty(this)" Width="80" Text='<%# DataBinder.Eval(Container.DataItem, "POQuantity").ToString() %>' />
                                                        </div>
                                                                </EditItemTemplate>
                                                    </asp:TemplateField>
                                                     <%-- <asp:TemplateField HeaderText="Invoice #"  HeaderStyle-Width="100">--%>
                                                    <asp:TemplateField HeaderText="<%$Resources:Resource,Invoice%>" >
                                                        <ItemTemplate>
                                                            <nobr>
                                                                      <asp:LinkButton OnClientClick="openDialog();" ForeColor="Black" ID="lnkInvoiceIDs" Font-Underline="false" runat="server"   CommandName="InvoiceIDs" CommandArgument='<%# String.Format("{0},{1},{2},{3},{4},{5}",DataBinder.Eval(Container.DataItem, "PODetailsID").ToString(),DataBinder.Eval(Container.DataItem, "MaterialMasterID").ToString(),DataBinder.Eval(Container.DataItem, "LineNumber").ToString(),DataBinder.Eval(Container.DataItem, "MCode").ToString(),DataBinder.Eval(Container.DataItem, "KitPlannerID").ToString(),DataBinder.Eval(Container.DataItem, "IsKitParent").ToString()) %>' Text='<%# DataBinder.Eval(Container.DataItem, "InvoiceIDs").ToString()  %>'> </asp:LinkButton>
                                                                    <img width="12" src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAADcSURBVDhPYwCBso5ZFhXdc/6TgweJAeUdM+Wapiz58p9EQDsDfvz68//Cndco+OPXn1BZBMDrgmV7bv33q9oKx6dvvITKIABeAz5/+/U/vn0PeQaANBdOOQLWCDMErwHV3YsUl28+8AskiKwZRIP4IO/gNWDxxv0W+09e/odNMwyAAhYdoBiw48jFf7g04wJwA/oW73VL6dxNkmYQABsQUr9DKKh26w2Q5rTuvf8PnL75//jFW/+nLd0MVkAIM/hWbt0N0uxXufUsyDCwk0gBPlWbjECGkKWZgYEBAKlgsNdoilJDAAAAAElFTkSuQmCC">     
                                                                <%-- <asp:Image ID=""   src='../Images/redarrowright.gif' border='0' runat="server"></asp:Image>--%>
                                                                        <asp:Image ImageUrl="../Images/redarrowright.gif" ID="Imdarrow" runat="server" Visible="true"></asp:Image>
                                                                      </nobr>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>

<%--Add New columns--%>
                                                     <%-- <asp:TemplateField ItemStyle-Width="100" HeaderText="Mfg Date" Visible="false">--%>
                                                     <asp:TemplateField ItemStyle-Width="100" HeaderText="<%$Resources:Resource,MfgDate%>"  Visible="false">
                                                                <ItemTemplate>
                                                                         <asp:Literal runat="server" ID="ltmfgdate" Text='<%#DataBinder.Eval(Container.DataItem, "MfgDate_Static","{0:dd-MMM-yyyy}").ToString() %>' />
                                                                  
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                      <asp:TextBox ID="txtmfgdate" runat="server" ClientIDMode="Static" CssClass="DateBoxCSS_small" EnableTheming="false" Text='<%#DataBinder.Eval(Container.DataItem, "MfgDate_Static","{0:dd-MMM-yyyy}").ToString() %>' />                                                                        
                                                                    
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                                 <%--<asp:TemplateField ItemStyle-Width="100" HeaderText="Exp Date" Visible="false">--%>
                                                    <asp:TemplateField ItemStyle-Width="100" HeaderText= "<%$Resources:Resource,ExpDate%>"  Visible="false">
                                                                <ItemTemplate>
                                                                      <asp:Literal runat="server" ID="ltexpdate" Text='<%#DataBinder.Eval(Container.DataItem, "ExpDate_Static","{0:dd-MMM-yyyy}").ToString() %>' />                                                               
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                      <asp:TextBox ID="txtexpdate" runat="server" ClientIDMode="Static" CssClass="DateBoxCSS_small" EnableTheming="false" Text='<%#DataBinder.Eval(Container.DataItem, "ExpDate_Static","{0:dd-MMM-yyyy}").ToString() %>' />                                                                                                                                       
                                                              </EditItemTemplate>
                                                            </asp:TemplateField>
                                                                <%-- <asp:TemplateField ItemStyle-Width="100" HeaderText="Serial No." Visible="false">--%>
                                                     <asp:TemplateField ItemStyle-Width="100" HeaderText="<%$Resources:Resource,SerialNo%>" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltserialno" Text='<%#DataBinder.Eval(Container.DataItem, "SerialNo_Static").ToString() %>' />
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox runat="server" ID="txtserialno"  Width="100" onKeypress="return checkSpecialChar(event)" Text='<%#DataBinder.Eval(Container.DataItem, "SerialNo_Static").ToString() %>' />
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                                <%-- <asp:TemplateField ItemStyle-Width="100" HeaderText="Batch No." Visible="false">--%>
                                                    <asp:TemplateField ItemStyle-Width="100" HeaderText="<%$Resources:Resource,BatchNo%>" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltbatchno" Text='<%#DataBinder.Eval(Container.DataItem, "BatchNo_Static").ToString() %>' />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox runat="server" ID="txtbatchno" Width="100" onKeypress="return checkSpecialChar(event)" Text='<%#DataBinder.Eval(Container.DataItem, "BatchNo_Static").ToString() %>' />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                                 <%--<asp:TemplateField ItemStyle-Width="100" HeaderText="Project Ref No." Visible="false">--%>
                                                    <asp:TemplateField ItemStyle-Width="100" HeaderText="<%$Resources:Resource,BatchNo%>"  Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Literal runat="server" ID="ltrefno" Text='<%#DataBinder.Eval(Container.DataItem, "ProjectRefNo_Static").ToString() %>' />
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox runat="server" ID="txtrefno"  Width="100" onKeypress="return checkSpecialChar(event)" Text='<%#DataBinder.Eval(Container.DataItem, "ProjectRefNo_Static").ToString() %>' />
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                             <%--<asp:TemplateField ControlStyle-Width="60" HeaderText="Delete" >--%>
                                                    <asp:TemplateField ControlStyle-Width="60" HeaderText="<%$Resources:Resource,Delete%>" >
                                                                            <ItemStyle HorizontalAlign="right"></ItemStyle>
                                                                            <ItemTemplate >
                                                                                <asp:CheckBox ID="chkDele" CssClass="chkDelete1" runat="server" />
                                                                            </ItemTemplate>
                                                                            <EditItemTemplate>
                                                                            </EditItemTemplate>
                                                                            <FooterTemplate>
                                                                                <%--<asp:LinkButton ID="lntDeleteItems1" runat="server" Text="<i class='material-icons ss'>delete</i>" ForeColor="Blue" Font-Underline="false" OnClientClick="return confirm('Are you sure want to delete the selected line items?');" OnClick="DeleteItems" />--%>
                                                                                <asp:LinkButton ID="lntDeleteItems1" runat="server"  ForeColor="Blue" Font-Underline="false" OnClientClick="return myconfirmbox();"><%=MRLWMSC21Common.CommonLogic.btnfaDelete %></asp:LinkButton>
                                                                            </FooterTemplate>
                                                                        </asp:TemplateField>
                                                                       <%-- <asp:TemplateField ControlStyle-Width="80" HeaderText="Edit">--%>
                                                     <asp:TemplateField ControlStyle-Width="80" HeaderText="<%$Resources:Resource,Edit%>">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkCPOEdit" runat="server" Text="<nobr><i class='material-icons'>mode_edit</i></nobr>" CssClass="" OnClick="GVPODetails_RowEditing" ></asp:LinkButton>
                                                                                
                                                                            </ItemTemplate>
                                                                            <EditItemTemplate>
                                                                               <flex>
                                                                                   <asp:Button ID="lnkSOUpdate"  CssClass="ButnEmpty" runat="server" Text="Update"  OnClientClick="this.disabled = true; this.value = 'Submitting...';" UseSubmitBehavior="false" OnClick="GVPODetails_RowUpdating"  ></asp:Button>
                                                                               <asp:LinkButton ID="lnkCSOCancel" runat="server" Text="Cancel" CssClass="ButnEmpty" OnClick="GVPODetails_RowCancelingEdit"  ></asp:LinkButton>
                                                                              </flex>
                                                                             <%--   <asp:Button ID="lnkCSOCancel" CssClass="ButnEmpty" runat="server" Text="Cancel"  OnClick="GVPODetails_RowCancelingEdit"></asp:Button>
                                                                    --%>        </EditItemTemplate>
                                                                        </asp:TemplateField>   
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="accordinoGap"></td>
                </tr>
                <tr class="">
                    <td id="tdSupinvoice" runat="server">
                       <%-- <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="dvSIDHeader">Supplier Invoice Details --%>
                         <div class="ui-SubHeading ui-SubHeadingBar accordino_icon_Right" id="dvSIDHeader"> <%= GetGlobalResourceObject("Resource", "SupplierInvoiceDetails")%>
                             <i class="material-icons downarrow right">keyboard_arrow_down</i>
                        </div>

                        <div class="ui-Customaccordion" id="dvSIDBody">


                            <table width="100%" style="padding-top: 10px; padding-left: 10px;">

                                <tr>
                                         <div align="right">
                                    <td colspan="3" align="right" style="padding-bottom: 10px;">
                                    <div class="flex__ right">
                                        <div><asp:FileUpload ID="FUImport" runat="server" /></div>&nbsp;&nbsp;
                                        <div class="flex__"><%--<asp:LinkButton ID="lnkimport" runat="server" onClientClick="return checkpo()" CssClass="btn btn-primary" OnClick="lnkimport_Click" >
                                        Import ASN<%=MRLWMSC21Common.CommonLogic.btnfaExcel %>
                                        </asp:LinkButton>--%>
                                            <asp:LinkButton ID="lnkimport" runat="server" onClientClick="return checkpo()" CssClass="btn btn-primary" OnClick="lnkimport_Click" >
                                          <%= GetGlobalResourceObject("Resource", "ImportASN")%><%=MRLWMSC21Common.CommonLogic.btnfaExcel %>
                                        </asp:LinkButton>&nbsp;&nbsp;
                                       <%-- <asp:LinkButton ID="lnkNewInv" runat="server" CssClass="btn btn-primary" OnClick="lnkNewInv_Click">
                                        Add Invoice<%=MRLWMSC21Common.CommonLogic.btnfaNew %>
                                        </asp:LinkButton>--%>
                                            <asp:LinkButton ID="lnkNewInv" runat="server" CssClass="btn btn-primary" OnClick="lnkNewInv_Click">
                                        <%= GetGlobalResourceObject("Resource", "AddInvoice")%><%=MRLWMSC21Common.CommonLogic.btnfaNew %>
                                        </asp:LinkButton></div> 
                                    </div>
                                    </td>
                                      </div>
                                </tr>

                                <tr>
                                    <td colspan="3" align="center">
                                        <table cellspacing="0" cellpadding="0" border="0" align="center">
                                            <tr>
                                                <td>
                                                    <%--<asp:UpdatePanel ID="upnlinvoice" ChildrenAsTriggers="true" runat="server"  UpdateMode="Always">
                                        <ContentTemplate>--%>
                                                    <asp:Literal ID="ltinvStatus" runat="server"></asp:Literal>
                                                    <asp:Panel runat="server" ID="pnlInvoiceDetails" HorizontalAlign="Center">
                                                        <asp:GridView ID="gvInvoicDetails" AutoGenerateColumns="false" PageSize="10" runat="server" SkinID="gvLightSteelBlueNew" OnRowDataBound="gvInvoicDetails_RowDataBound" OnPageIndexChanging="gvInvoicDetails_PageIndexChanging">
                                                            <Columns>
                                                                <%--<asp:TemplateField HeaderStyle-Width="100" HeaderText="Invoice Number">--%>
                                                                <asp:TemplateField HeaderStyle-Width="100" HeaderText= "<%$Resources:Resource,InvoiceNumber%>">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="ltInvID" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "SupplierInvoiceID").ToString() %>'></asp:Literal>
                                                                        <asp:Literal ID="ltInvNumber" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "InvoiceNumber").ToString() %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <div class="gridInput">
                                                                            <asp:Literal ID="ltInvID_Edit" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "SupplierInvoiceID").ToString() %>'></asp:Literal>
                                                                            <asp:RequiredFieldValidator ID="rfvInvNumber" runat="server" ValidationGroup="UpdateInvoice" ControlToValidate="txtInvNumber" Display="Dynamic"/>
                                                                            <asp:TextBox ID="txtInvNumber" onKeypress="return checkSpecialChar(event)" runat="server" Width="90" Text='<%#DataBinder.Eval(Container.DataItem, "InvoiceNumber").ToString() %>'></asp:TextBox>
                                                                            <span class="errorMsg"></span></div>
                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>
                                                              <%--  <asp:TemplateField HeaderStyle-Width="80" HeaderText="Invoice Date">--%>
                                                                  <asp:TemplateField HeaderStyle-Width="80" HeaderText="<%$Resources:Resource,InvoiceDate%>" >
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="ltInvDate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "InvoiceDate", "{0:dd-MMM-yyyy}").ToString() %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <div class="gridInput">
                                                                         <asp:RequiredFieldValidator ID="rfvInvDate" runat="server" ValidationGroup="UpdateInvoice" ControlToValidate="txtInvDate" Display="Dynamic"/>
                                                                     
                                                                       <span class="errorMsg"></span><asp:TextBox ID="txtInvDate" ClientIDMode="Static" runat="server" Width="90" Text='<%#DataBinder.Eval(Container.DataItem, "InvoiceDate", "{0:dd-MMM-yyyy}").ToString() %>' />
                                                                   </div>
                                                                            </EditItemTemplate>
                                                                </asp:TemplateField>
                                                             <%--   <asp:TemplateField HeaderStyle-Width="80" HeaderText="Invoice Value">--%>
                                                                   <asp:TemplateField HeaderStyle-Width="80" HeaderText="<%$Resources:Resource,InvoiceValue%>">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="ltInvValue" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "InvoiceValue").ToString() %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtInvValue" runat="server" Width="90" onKeyPress="return checkDec(this,event)" onblur="CheckDecimal(this)" Text='<%#DataBinder.Eval(Container.DataItem, "InvoiceValue").ToString() %>'></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>
                                                               <%-- <asp:TemplateField HeaderStyle-Width="60" HeaderText="Currency">--%>
                                                                 <asp:TemplateField HeaderStyle-Width="60" HeaderText="<%$Resources:Resource,Currency%>">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="ltCurrency" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Code").ToString() %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>

                                                                        <asp:TextBox ID="atcInvCurrency" Width="90" SkinID="txt_Hidden_Req_Auto" runat="server" ClientIDMode="Static" Text='<%#DataBinder.Eval(Container.DataItem, "Code").ToString() %>'>    </asp:TextBox>
                                                                        <asp:HiddenField ID="hifInvCurrency" runat="server" ClientIDMode="Static" Value='<%#DataBinder.Eval(Container.DataItem, "CurrencyID").ToString() %>' />

                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>
                                                              <%--  <asp:TemplateField HeaderStyle-Width="80" HeaderText="Inv. Country of Origin">--%>
                                                                  <asp:TemplateField HeaderStyle-Width="80" HeaderText="<%$Resources:Resource,InvCountryofOrigin%>" >
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="ltInvCountryofOrigin" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "InvCountryofOrigin").ToString() %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="atcInvCountryofOrigin" Width="90" SkinID="txt_Hidden_Req_Auto" runat="server" ClientIDMode="Static" Text='<%#DataBinder.Eval(Container.DataItem, "InvCountryofOrigin").ToString() %>'>    </asp:TextBox>
                                                                        <asp:HiddenField ID="hifInvCountryofOrigin" runat="server" ClientIDMode="Static" Value='<%#DataBinder.Eval(Container.DataItem, "InvCountryofOriginID").ToString() %>' />
                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>

                                                           <%--     <asp:TemplateField HeaderStyle-Width="80" HeaderText="Inv. VAT Code">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="ltInvVATCode" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "InvVATCode").ToString() %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtInvVATCode" runat="server" onKeyPress="return checkSpecialChar(event)" Width="90" Text='<%#DataBinder.Eval(Container.DataItem, "InvVATCode").ToString() %>'></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderStyle-Width="80" HeaderText="Exchange">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="ltExchange" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ExchangeRate").ToString() %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtExchange" runat="server" onKeyPress="return checkDec(this,event)" onblur="CheckDecimal(this)" Width="90" Text='<%#DataBinder.Eval(Container.DataItem, "ExchangeRate").ToString() %>'></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>--%>
                                                              <%--  <asp:TemplateField HeaderStyle-Width="80" HeaderText="No. of Packages">--%>
                                                                  <asp:TemplateField HeaderStyle-Width="80" HeaderText= "<%$Resources:Resource,NoofPackages%>">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="ltNoofPackages" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "NoofPackages").ToString() %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtNoofPackages" Width="90" onKeyPress="return checkNum(event)" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "NoofPackages").ToString() %>'></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>
                                                                <%--<asp:TemplateField HeaderStyle-Width="80" HeaderText="Gross Weight">--%>
                                                                <asp:TemplateField HeaderStyle-Width="80" HeaderText="<%$Resources:Resource,GrossWeight%>">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="ltGrossWeight" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "GrossWeight").ToString() %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtGrossWeight" Width="90" runat="server" onKeyPress="return checkDec(this,event);" onblur="CheckDecimal(this)" Text='<%#DataBinder.Eval(Container.DataItem, "GrossWeight").ToString() %>'></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>
                                                                <%--<asp:TemplateField HeaderStyle-Width="80" HeaderText="Net Weight">--%>
                                                                <asp:TemplateField HeaderStyle-Width="80" HeaderText="<%$Resources:Resource,NetWeight%>" >
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="ltNetWeight" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "NetWeight").ToString() %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtNetWeight" Width="90" runat="server" onKeyPress="return checkDec(this,event)" onblur="CheckDecimal(this)" Text='<%#DataBinder.Eval(Container.DataItem, "NetWeight").ToString() %>'></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>
                                                             <%--   <asp:TemplateField HeaderStyle-Width="60" HeaderText="Delete">--%>
                                                                   <asp:TemplateField HeaderStyle-Width="60" HeaderText="<%$Resources:Resource,Delete%>" >
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkDelete" CssClass="chkDelete" runat="server" />
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                    </EditItemTemplate>
                                                                    <FooterTemplate>
                                                                      <%--  <asp:LinkButton ID="lnkDelete" runat="server" Font-Underline="false" ForeColor="Blue" OnClientClick="return lnkDelete_ClientClick();"   OnClick="lnkDelete_Click"><%=MRLWMSC21Common.CommonLogic.btnfaDelete %></asp:LinkButton>
                                                                     --%>  
                                                                        <asp:LinkButton ID="lnkDelete" runat="server"  ForeColor="Blue" Font-Underline="false" OnClientClick="return myInvoiceconfirmbox();"><%=MRLWMSC21Common.CommonLogic.btnfaDelete %></asp:LinkButton>
                                                                          
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                               <%-- <asp:TemplateField HeaderStyle-Width="60" HeaderText="Edit">--%>
                                                                 <asp:TemplateField HeaderStyle-Width="60" HeaderText="<%$Resources:Resource,Edit%>" >
                                                                    <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkCPOEdit" runat="server" Text="<nobr><i class='material-icons ss'>mode_edit</i></nobr>" CssClass="" OnClick="gvInvoicDetails_RowEditing" ></asp:LinkButton>
                                                                                 </ItemTemplate>
                                                                            <EditItemTemplate>
                                                                                <flex><asp:Button ID="lnkCPOUpdate" CssClass="ButnEmpty" runat="server" Text="Update"  OnClientClick="this.disabled = true; this.value = 'Submitting...';" UseSubmitBehavior="false" OnClick="gvInvoicDetails_RowUpdating" ></asp:Button>
                                                                               
                                                                                <asp:LinkButton ID="lnkCPOCancel" runat="server" Text="Cancel" CssClass="ButnEmpty" OnClick="gvInvoicDetails_RowCancelingEdit" ></asp:LinkButton>
                                                                            </flex>
                                                                             <%--   <asp:Button ID="lnkCPOCancel" CssClass="ButnEmpty" runat="server" Text="Cancel"  OnClick="gvInvoicDetails_RowCancelingEdit"></asp:Button>
                                                                        --%>    </EditItemTemplate>
                                                                </asp:TemplateField>

                                                                <%--<asp:CommandField ControlStyle-Font-Underline="false" CausesValidation="true" EditText="<nobr> Edit <img src='../Images/redarrowright.gif' border='0' /></nobr>" ShowEditButton="true" />--%>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                    <%--</ContentTemplate>
                                            </asp:UpdatePanel>--%>
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
                    <td class="accordinoGap"></td>
                </tr>
            </table></div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lnkUpdate" />
                   <asp:PostBackTrigger ControlID="lnkimport" />
        </Triggers>
    </asp:UpdatePanel>
<div class="">
    <div id="disputeDivSupplierItemList">
     <div id="divSupplierItemList"  >
         <asp:UpdatePanel ID="upnlSupplierItemListDialog" ChildrenAsTriggers="true" runat="server"  UpdateMode="Always">
                    <ContentTemplate>
        <div class="ui-dailog-body" style="height:395px;width: 835px;overflow: auto;">

        <table Width="800px" align="center">
            
            <tr>
                <td align="left">
                    <%--<span style="color:#1a25f7;font-size:16px;">Line Number: </span ><asp:Literal ID="ltLineNo" runat="server" />--%>
                    <span style="color:#1a25f7;font-size:16px;"><%= GetGlobalResourceObject("Resource", "LineNumber")%> </span ><asp:Literal ID="ltLineNo" runat="server" />
                    <br />
                    <%--<span style="color:#1a25f7;font-size:16px;">Part Number: </span><asp:Literal ID="ltMaterialCode" runat="server" />--%>
                    <span style="color:#1a25f7;font-size:16px;"><%= GetGlobalResourceObject("Resource", "PartNumber")%>  </span><asp:Literal ID="ltMaterialCode" runat="server" />
                </td>
                <td align="right">
                    <div align="right" id="btnInvoice" style="margin:10px;">
                    <%--<asp:LinkButton ID="lnkNewinvoiceItemDetials" runat="server" CssClass="btn btn-primary" OnClick="lnkNewinvoiceItemDetials_Click" >
                            Add Invoice Details<%=MRLWMSC21Common.CommonLogic.btnfaNew %>
                        </asp:LinkButton>--%>
                        <asp:LinkButton ID="lnkNewinvoiceItemDetials"  runat="server" CssClass="btn btn-primary" OnClick="lnkNewinvoiceItemDetials_Click" >
                            <%= GetGlobalResourceObject("Resource", "AddInvoiceDetails")%>  <%=MRLWMSC21Common.CommonLogic.btnfaNew %>
                        </asp:LinkButton>
                    </div>
                        <asp:HiddenField ID="hifMaterialMasterId" runat="server" />
                        <asp:HiddenField ID="hifPODetailsID" runat="server" />
                    <asp:HiddenField ID="hdnkitid" runat="server" />
                     <asp:HiddenField ID="hdnIsKitParent" runat="server" />
                </td>
            </tr>
             
                <tr>
                    <td align="left" colspan="2">
                        <asp:Literal ID="ltSupplierDetailsStatus" runat="server"></asp:Literal>
                        <asp:GridView ID="gvSupplerInvoiceMaterialList" SkinID="gvLightSteelBlueNew" AutoGenerateColumns="False" runat="server" OnRowDataBound="gvSupplerInvoiceMaterialList_RowDataBound" OnPageIndexChanging="gvSupplerInvoiceMaterialList_PageIndexChanging" >
                            <Columns>
                                <%--<asp:TemplateField HeaderText="Invoice No." HeaderStyle-Width="100">--%>
                                <asp:TemplateField HeaderText= "<%$Resources:Resource,InvoiceNo%>" HeaderStyle-Width="100">
                                    <ItemTemplate >
                                        <asp:Literal ID="ltInvoiceNumber" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "InvoiceNumber").ToString() %>'/>
                                        <asp:Literal ID="ltSupplierInvoiceDetailsID" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "SupplierInvoiceDetailsID").ToString() %>'/>
                                        <asp:Literal ID="ltinvoiceID" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "SupplierInvoiceID").ToString() %>'/>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <div class="gridInput">
                                        <asp:RequiredFieldValidator ID="rfvInvoiceNumber" runat="server" ValidationGroup="UpdateInvoiceDetails" ControlToValidate="atcInvoiceNumber" Display="Dynamic"/>
                                        <span class="errorMsg"></span><asp:Literal ID="ltSupplierInvoiceDetailsID" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "SupplierInvoiceDetailsID").ToString() %>'/><asp:TextBox ID="atcInvoiceNumber" Width="100" SkinID="txt_Hidden_Req_Auto" runat="server" ClientIDMode="Static" Text='<%#DataBinder.Eval(Container.DataItem, "InvoiceNumber").ToString() %>'/>
                                        <asp:HiddenField ID="hifSupplierInvoiceID" ClientIDMode="Static" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "SupplierInvoiceID").ToString() %>'/>
                                         
                                 
                                   </div>
                                            </EditItemTemplate>
                                    <HeaderStyle Width="100px" />
                                </asp:TemplateField>

                                
                                <%--<asp:TemplateField HeaderText="Inv. UoM/ Qty." HeaderStyle-Width="80">--%>
                                <asp:TemplateField HeaderText= "<%$Resources:Resource,InvUoMQty%>" HeaderStyle-Width="80">
                                    <ItemTemplate >
                                        <asp:Literal ID="ltMaterialMaster_InvUoM" runat="server" Text='<%#String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "InvUoM").ToString(),DataBinder.Eval(Container.DataItem, "InvUoMQty").ToString())  %>'/>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <div class="gridInput">
                                        <asp:RequiredFieldValidator ID="rfvMaterialMaster_InvUoM" runat="server" ValidationGroup="UpdateInvoiceDetails" ControlToValidate="atcMaterialMaster_InvUoM" Display="Dynamic"/>
                                        <span class="errorMsg"></span><asp:TextBox ID="atcMaterialMaster_InvUoM" Width="80" SkinID="txt_Hidden_Req_Auto" runat="server" ClientIDMode="Static" Text='<%#  String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "InvUoM").ToString(),DataBinder.Eval(Container.DataItem, "InvUoMQty").ToString())=="/"?"":String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "InvUoM").ToString(),DataBinder.Eval(Container.DataItem, "InvUoMQty").ToString())  %>'/>
                                        <asp:HiddenField ID="hifMaterialMaster_InvUoM" ClientIDMode="Static" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "MaterialMaster_InvUoMID").ToString() %>'/>
                                        <input id="hidInvUoMQty" type="hidden" value='<%# DataBinder.Eval(Container.DataItem, "InvUoMQty").ToString() %>'/>
                                  </div>
                                            </EditItemTemplate>
                                    <HeaderStyle Width="80px" />
                                </asp:TemplateField>

                                <%--<asp:TemplateField HeaderText="Invoice Qty." HeaderStyle-Width="60">--%>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,InvoiceQty%>" HeaderStyle-Width="60">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltInvoiceQuantity" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "InvoiceQuantity").ToString() %>'/>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <div class="gridInput">
                                        <asp:RequiredFieldValidator ID="rfvInvoiceQuantity" runat="server" ValidationGroup="UpdateInvoiceDetails" ControlToValidate="txtInvoiceQuantity" Display="Dynamic"/>
                                       <span class="errorMsg"></span><asp:TextBox ID="txtInvoiceQuantity" Width="80" onKeyPress="return checkDec(this,event)" onblur="CheckInvUoMQty(this)" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "InvoiceQuantity").ToString() %>'/>
                                   </div>
                                            </EditItemTemplate>
                                    <HeaderStyle Width="60px" />
                                </asp:TemplateField>

                               <%-- <asp:TemplateField HeaderText="Inv. Discount In Percentage" HeaderStyle-Width="80">--%>
                                 <asp:TemplateField HeaderText="<%$Resources:Resource,InvDiscountInPercentage%>"  HeaderStyle-Width="80">
                                    <ItemTemplate>
                                        <asp:Literal ID="ltInvDiscountInPercentage" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "InvDiscountInPercentage").ToString() %>'/>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtInvDiscountInPercentage" Width="80" onKeyPress="return checkDec(this,event)" onblur="CheckDecimal(this)" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "InvDiscountInPercentage").ToString() %>'/>
                                        
                                    </EditItemTemplate>
                                    <HeaderStyle Width="80px" />
                                </asp:TemplateField>

                               <%-- <asp:TemplateField HeaderText="Unit Price" HeaderStyle-Width="70">--%>
                                 <asp:TemplateField HeaderText= "<%$Resources:Resource,UnitPrice%>"   HeaderStyle-Width="70">
                                    <ItemTemplate >
                                        <asp:Literal ID="ltUnitPrice" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "UnitPrice").ToString() %>'/>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtUnitPrice" runat="server" onKeyPress="return checkDec(this,event)" onblur="CheckDecimal(this)" Width="70" Text='<%#DataBinder.Eval(Container.DataItem, "UnitPrice").ToString() %>'/>
                                        
                                    </EditItemTemplate>
                                    <HeaderStyle Width="70px" />
                                </asp:TemplateField>

                               <%-- <asp:TemplateField HeaderText="Tax" HeaderStyle-Width="60">--%>
                                 <asp:TemplateField HeaderText= "<%$Resources:Resource,Tax%>"  HeaderStyle-Width="60">
                                    <ItemTemplate >
                                        <asp:Literal ID="ltTax" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Tax").ToString() %>'/>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtTax" runat="server" Width="60" onKeyPress="return checkDec(this,event)" onblur="CheckDecimal(this)" Text='<%#DataBinder.Eval(Container.DataItem, "Tax").ToString() %>'/>                                    
                                    </EditItemTemplate>
                                    <HeaderStyle Width="60px" />
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="Store Ref. #">--%>
                                <asp:TemplateField HeaderText= "<%$Resources:Resource,StoreRef%>">
                                    <ItemTemplate>
                                        <a href="../mInbound/InboundDetails.aspx?ibdid=<%#DataBinder.Eval(Container.DataItem, "InboundID").ToString() %>" style="font-size:11pt;color:#6387fc;text-decoration:none"><%#DataBinder.Eval(Container.DataItem, "StoreRefNo").ToString() %></a>
                                    </ItemTemplate>
                                    <EditItemTemplate>

                                    </EditItemTemplate>
                                </asp:TemplateField>
                                   <%-- <asp:TemplateField HeaderText="Mfg. Date" HeaderStyle-Width="150">--%>
                                 <asp:TemplateField HeaderText="<%$Resources:Resource,MfgDate%>"  HeaderStyle-Width="150">
                                          <ItemTemplate>
                                                                        <asp:Literal ID="ltmfgdate1" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "MfgDate", "{0:dd-MMM-yyyy}").ToString() %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <div class="gridInput">
                                                                        <asp:Label ID="lblmfgdate" CssClass="lblmfgdate errorMsg" style="color:red;display:none" runat="server">*</asp:Label><asp:TextBox ID="txtMfgDate1" ClientIDMode="Static"  runat="server" Width="150" Text='<%#DataBinder.Eval(Container.DataItem, "MfgDate", "{0:dd-MMM-yyyy}").ToString() %>'  />
                                                                    </div>
                                                                            </EditItemTemplate>
                                 
                                          <HeaderStyle Width="150px" />
                                 
                                </asp:TemplateField>
                                   <%--<asp:TemplateField HeaderText="Exp. Date" HeaderStyle-Width="150">--%>
                                   <asp:TemplateField HeaderText="<%$Resources:Resource,ExpDate%>" HeaderStyle-Width="150">
                                         
                                    <ItemTemplate >
                                      
                                               <asp:Literal ID="ltexpdate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ExpDate", "{0:dd-MMM-yyyy}").ToString() %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <div class="gridInput">
                                                                       <asp:Label ID="lblexpdate" CssClass="lblexpdate errorMsg" style="color:red;display:none"  runat="server">*</asp:Label><asp:TextBox ID="txtExpDate1" ClientIDMode="Static" runat="server" Width="90" Text='<%#DataBinder.Eval(Container.DataItem, "ExpDate", "{0:dd-MMM-yyyy}").ToString() %>' />
                                                                   </div>
                                                                            </EditItemTemplate>
                                       <HeaderStyle Width="150px" />
                                     
                               </asp:TemplateField>
                                   <%--<asp:TemplateField HeaderText="Batch No." HeaderStyle-Width="60">--%>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,BatchNo%>"  HeaderStyle-Width="60">
                                    <ItemTemplate >
                                        <asp:Literal ID="ltBatchNo" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "BatchNo").ToString() %>'/>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                         <div class="gridInput">
                                       <asp:Label ID="lblBatchNo" CssClass="lblBatchNo errorMsg" style="color:red;display:none"  runat="server">*</asp:Label><asp:TextBox ID="txtBatchNo" runat="server" Width="60"  Text='<%#DataBinder.Eval(Container.DataItem, "BatchNo").ToString() %>'/>                                    
                                   </div>
                                             </EditItemTemplate>

                                       <HeaderStyle Width="60px" />
                                </asp:TemplateField>                              
                                  <%--  <asp:TemplateField HeaderText="Project Ref No." HeaderStyle-Width="60">--%>
                                        <%-- <asp:TemplateField HeaderText="MRP" HeaderStyle-Width="60">--%>
                                 <asp:TemplateField HeaderText="<%$Resources:Resource,MRP%>"  HeaderStyle-Width="60">
                                    <ItemTemplate >
                                        
                                        <asp:Literal ID="ltMRP" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "MRP").ToString() %>'/>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                          <div class="gridInput">
                                       <asp:Label ID="lblMRP" CssClass="lblMRP errorMsg" style="color:red;display:none" runat="server">*</asp:Label><asp:TextBox ID="txtMRP" runat="server" Width="60" onKeyPress="return checkDec(this,event)" Text='<%#DataBinder.Eval(Container.DataItem, "MRP").ToString() %>'/>                                    
                                   </div>
                                              </EditItemTemplate>
                                        <HeaderStyle Width="60px" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Project RefNo"  HeaderStyle-Width="60">
                                    <ItemTemplate >
                                        
                                        <asp:Literal ID="ltProjectrefno" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ProjectRefNo").ToString() %>'/>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                          <div class="gridInput">
                                       <asp:Label ID="lblProjrefno" CssClass="lblProjrefno errorMsg" style="color:red;display:none" runat="server">*</asp:Label><asp:TextBox ID="txtProjectrefno" runat="server" Width="60"  Text='<%#DataBinder.Eval(Container.DataItem, "ProjectRefNo").ToString() %>'/>                                    
                                   </div>
                                              </EditItemTemplate>
                                        <HeaderStyle Width="60px" />
                                </asp:TemplateField>
                                          <%--<asp:TemplateField HeaderText="Serial No." HeaderStyle-Width="60">--%>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,SerialNo%>"  HeaderStyle-Width="60">
                                    <ItemTemplate >
                                        
                                        <asp:Literal ID="ltSerialNo" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "SerialNo").ToString() %>'/>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                          <div class="gridInput">
                                       <asp:Label ID="lblSerialNo" CssClass="lblSerialNo errorMsg" style="color:red;display:none" runat="server">*</asp:Label><asp:TextBox ID="txtSerialNo" runat="server" Width="60"  Text='<%#DataBinder.Eval(Container.DataItem, "SerialNo").ToString() %>'/>                                    
                                 </div>
                                              </EditItemTemplate>
                                        <HeaderStyle Width="60px" />
                                </asp:TemplateField>
                               <%-- <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="60">--%>
                                 <asp:TemplateField HeaderText="<%$Resources:Resource,Delete%>"  HeaderStyle-Width="60">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkDelete2" runat="server" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                      <%--  <asp:LinkButton ID="lnkinvDelete" runat="server" ForeColor="Blue" OnClientClick="return confirm('Are you sure want to delete the selected line items?');" OnClick="lnkinvDelete_Click" Text="Delete" Font-Underline="false"></asp:LinkButton>
                               --%>   
                                       <%--   <asp:LinkButton ID="lnkinvDelete" runat="server" ForeColor="Blue"  OnClick="lnkinvDelete_Click" Text="<nobar><i class='material-icons'>delete</i></nobar>" Font-Underline="false"></asp:LinkButton>
                               --%>       <asp:LinkButton ID="lnkDelete" runat="server"  ForeColor="Blue" Font-Underline="false" OnClientClick="return InvMaterrialconfirmbox();"><%=MRLWMSC21Common.CommonLogic.btnfaDelete %></asp:LinkButton>
                                                                      
                                    </FooterTemplate>
                                    <HeaderStyle Width="60px" />
                                </asp:TemplateField>       
                                <asp:TemplateField ItemStyle-Width="150">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkCPOEdit" runat="server"  CssClass="ButnEmpty"  border='0'  OnClick="gvSupplerInvoiceMaterialList_RowEditing" >Edit</asp:LinkButton>
                                                                                
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <flex>
                                        <asp:Button ID="lnkCPOUpdate" CssClass="ButnEmpty" runat="server" Text="Update"  OnClientClick="this.disabled = true; this.value = 'Submitting...';" UseSubmitBehavior="false" OnClick="gvSupplerInvoiceMaterialList_RowUpdating"></asp:Button>
                                        <asp:LinkButton ID="lnkCPOCancel" runat="server" Text="Cancel" CssClass="ButnEmpty" OnClick="gvSupplerInvoiceMaterialList_RowCancelingEdit" ></asp:LinkButton>
                                            </flex>                                
                                        <%--  <asp:Button ID="lnkCPOCancel" CssClass="ButnEmpty" runat="server" Text="Cancel"  OnClick="gvSupplerInvoiceMaterialList_RowCancelingEdit"></asp:Button>
                                 --%>   </EditItemTemplate>
                                    <ItemStyle Width="100px" />
                                </asp:TemplateField>                        
                               <%--<asp:CommandField ControlStyle-Font-Underline ="false" ItemStyle-Width="100" CausesValidation="true" EditText=" Edit <img src='../Images/redarrowright.gif' border='0' />" ShowEditButton="true"/>--%>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
           
        </table>
            </div>
                        <div class="ui-dailog-footer">
                          <%--  <div style="padding: 15px 13px 15px 5px;">--%>
                            <div style="float:right;margin:5px">
                                <%--<asp:LinkButton ID="lnkInvclose" CssClass="btn btn-primary" OnClick="lnkInvclose_Click"  runat="server" >
                                    Close<%=MRLWMSC21Common.CommonLogic.btnfaClear %>

                                </asp:LinkButton>--%>
                                <asp:LinkButton ID="lnkInvclose" CssClass="btn btn-primary" OnClick="lnkInvclose_Click"  runat="server" >
                                   <%= GetGlobalResourceObject("Resource", "Close")%><%=MRLWMSC21Common.CommonLogic.btnfaClear %>

                                </asp:LinkButton>
                            </div>
                        </div>    
                        </ContentTemplate>
        
             </asp:UpdatePanel>
         </div>
        </div>
        <div id="modalConfirmYesNo" class="modal" aria-hidden="true" data-backdrop="static" data-keyboard="false">
    <div class="modal-dialog" style="height:100px;width: 350px;">
        <div class="modal-content">
            <div class="modal-header" >
                <button type="button" 

                class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            
                <h4 id="lblTitleConfirmYesNo" class="modal-title"><%= GetGlobalResourceObject("Resource", "Confirmation")%></h4>
            </div>
            <div class="modal-body text-left" style="height:50px;">
                <span style="font-size:larger;"> <%= GetGlobalResourceObject("Resource", "AreyousuredoyouwanttoDelete")%></span>
             
            </div>
            <div class="modal-footer">               
                  <flex end><button id="btnYesConfirmYesNo"  type="button" class="btn btn-primary"> <%= GetGlobalResourceObject("Resource", "Yes")%><i class="fa fa-check" aria-hidden="true"></i></button>
                 <button id="btnNoConfirmYesNo" type="button" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "No")%>  <i class="fa fa-remove" aria-hidden="true"></i></button></flex>
            </div>
        </div>
    </div>
</div>

      <!--- PO material  delete Alert  By Meena--------->

            <div id="modalConfirmDelete" class="modal" aria-hidden="true" data-backdrop="static" data-keyboard="false">
    <div class="modal-dialog" style="height:100px;width: 350px;">
        <div class="modal-content">
            <div class="modal-header"  style="background-color: var(--sideNav-bg)">
                <button type="button" 

                class="close" data-dismiss="modal" aria-label="Close" style="color:#fff;opacity:1;">
                    <span aria-hidden="true">&times;</span>
                </button>
               <%-- <h4 id="lblTitleConfirmDelete" class="modal-title">Confirmation</h4>--%>
                 <h4 id="lblTitleConfirmDelete" style="color:#fff" class="modal-title"> <%= GetGlobalResourceObject("Resource", "Confirmation")%></h4>
            </div>
            <div class="modal-body text-left" style="height:50px;">
              
                <%--<span style="font-size:larger;"><p id="lblMsgConfirmYesNo"></p></span>--%>
               <%-- <span style="font-size:larger;">Are you sure do you want to Delete?</span>--%>
                 <span style="font-size:larger;"><%= GetGlobalResourceObject("Resource", "AreyousuredoyouwanttoDelete")%> </span>
              
            </div>
            <div class="modal-footer" flex end> 
                <asp:LinkButton ID="btnConfirmDeleteYes" runat="server" CssClass="btn btn-primary"  Font-Underline="false" OnClick="DeleteItems">Yes <i class="fa fa-check" aria-hidden="true"></i></asp:LinkButton>
                
                   <button id="btnNoConfirmNo"    type="button" class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "No")%> <i class="fa fa-remove" aria-hidden="true"></i></button>
            </div>
        </div>
    </div>
</div>

    <!--- Invoice delete Alert  By Meena--------->


    <div id="modalInvoiceConfirmDelete" class="modal" aria-hidden="true" data-backdrop="static" data-keyboard="false">
    <div class="modal-dialog" style="height:100px;width: 350px;">
        <div class="modal-content">
            <div class="modal-header" style="background-color: var(--sideNav-bg)" >
                <button type="button" 

                class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
               <%-- <h4 id="lblConfirmDelete" class="modal-title">Confirmation</h4>--%>
                 <h4 id="lblConfirmDelete" style="background:; color:white;" class="modal-title"> <%= GetGlobalResourceObject("Resource", "Confirmation")%></h4>
            </div>
            <div class="modal-body text-left" style="height:50px;">
              
                <span style="font-size:larger;"> <%= GetGlobalResourceObject("Resource", "AreyousuredoyouwanttoDelete")%></span>
                
            </div>
            <div class="modal-footer"> 
                <asp:LinkButton ID="lnkinvoicedelete" runat="server" CssClass="btn btn-primary"  Font-Underline="false" OnClick="lnkDelete_Click">Yes <i class="fa fa-check" aria-hidden="true"></i></asp:LinkButton>
               
                <button id="btnNoConfirmNo1" type="button"  class="btn btn-primary"> <%= GetGlobalResourceObject("Resource", "No")%> <i class="fa fa-remove" aria-hidden="true"></i></button>
            </div>
        </div>
    </div>
</div>
     <!--- Invoice mapping material  delete Alert  By Meena--------->
    
     <div id="modalInvMaterialConfirmDelete" class="modal fade" aria-hidden="true" data-backdrop="static" data-keyboard="false">
    <div class="modal-dialog" style="height:100px;width: 350px;">
        <div class="modal-content">
            <div class="modal-header"  >
                <button type="button" 

                class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
               <%-- <h4 id="lblMatConfirmDelete" class="modal-title">Confirmation</h4>--%>
                 <h4 id="lblMatConfirmDelete" class="modal-title"> <%= GetGlobalResourceObject("Resource", "Confirmation")%></h4>
            </div>
            <div class="modal-body text-left" style="height:50px;">
              
                <%--<span style="font-size:larger;"><p id="lblMsgConfirmYesNo"></p></span>--%>
                <%--<span style="font-size:larger;">Are you sure do you want to Delete?</span>--%>
                <span style="font-size:larger;"> <%= GetGlobalResourceObject("Resource", "AreyousuredoyouwanttoDelete")%></span>
                <br /><br />
                <br /><br />
            </div>
            <div class="modal-footer"> 
                <asp:LinkButton ID="lnkinvMaterial" runat="server" CssClass="btn btn-primary"  Font-Underline="false" OnClick="lnkinvDelete_Click">Yes <i class="fa fa-check" aria-hidden="true"></i></asp:LinkButton>
                
                <%--<button id="btnNoConfirmNo2" 

                type="button" class="btn btn-primary">No <i class="fa fa-remove" aria-hidden="true"></i></button>--%>
                <button id="btnNoConfirmNo2" type="button"  class="btn btn-primary"><%= GetGlobalResourceObject("Resource", "No")%> <i class="fa fa-remove" aria-hidden="true"></i></button>
            </div>
        </div>
    </div>
</div>
    </div>
</asp:Content>
 



 