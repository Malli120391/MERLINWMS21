<%@ Page Title=" Outbound Tracking :." Language="C#" MasterPageFile="~/mOutbound/OutboundMaster.master" AutoEventWireup="true" CodeBehind="OutboundTracking.aspx.cs" Inherits="MRLWMSC21.mOutbound.OutboundTracking" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="OBContent" runat="server">



    <asp:ScriptManager runat="server" ID="spmngrOBDTracking" EnablePartialRendering="true" SupportsPartialRendering="true"></asp:ScriptManager>

    <script type="text/javascript" src="../Scripts/jQuery2/countdown/jquery.countdown.js"></script>


    <script type="text/javascript" src="Scripts/jquery-1.8.2.js"></script>
    <script type="text/javascript" src="Scripts/jquery-ui-1.8.24.js"></script>
    <script type="text/javascript" src="Scripts/modernizr-2.6.2.js"></script>

    <style>
        .gvLightBlueNew_pager span, .gvLightGrayNew_pager span{
            color:#fff !important
        }
    </style>

    <script type="text/javascript">
        jQuery.noConflict(); // you can use $ instead
        (function ($) {
            $.fn.blink = function (options) {
                var defaults = { delay: 800 };
                var options = $.extend(defaults, options);

                return this.each(function () {
                    var obj = $(this);
                    setInterval(function () {
                        if ($(obj).css("visibility") == "visible") {
                            $(obj).css('visibility', 'hidden');
                        }
                        else {
                            $(obj).css('visibility', 'visible');
                        }
                    }, options.delay);
                });
            }
        }(jQuery))
    </script>




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
          $("#accordion").accordion({ header: 'h3', collapsible: true, autoHeight: true, clearStyle: true, navigation: true });

      });

        var VLPWarehouseid = 0;
         function ClearText(TextBox) {
             TextBox.style.color = "#000000";
             if (TextBox.value == "Search Delv. Doc.#...")
                 TextBox.value = "";
         }

         function focuslost1(TextBox) {
             TextBox.style.color = "#A4A4A4";
             if (TextBox.value == "")
                 TextBox.value = "Search Delv. Doc.#...";
         }

         function ClearTextKitCode(TextBox) {
             TextBox.style.color = "#000000";
             if (TextBox.value == "Search Kit Code ...")
                 TextBox.value = "";
         }

         function focuslostKitCode(TextBox) {
             TextBox.style.color = "#A4A4A4";
             if (TextBox.value == "")
                 TextBox.value = "Search Kit Code ...";
         }

         function ClearTextTenant(TextBox) {
             if (TextBox.value == "Search Tenant...")
                 TextBox.value = "";

             TextBox.style.color = "#000000";
         }

         function focuslostTenant(TextBox) {
             if (TextBox.value == "")
                 TextBox.value = "Search Tenant...";

             TextBox.style.color = "#A4A4A4";
         }

         function CollapseAll() {
             $("#collapseAll").click(function () {
                 $(".ui-accordion-content").hide()
             });
         }

         function ExpandAll() {
             $("#expandAll").click(function () {
                 $(".ui-accordion-content").show()
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

                var textfieldname = $('#txtPlantDeliveryNote');
                DropdownFunction(textfieldname);
                $('#txtPlantDeliveryNote').autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadPNCDelvDocNo") %>',
                              data: "{ 'prefix': '" + request.term + "'}",
                              dataType: "json",
                              type: "POST",
                              contentType: "application/json; charset=utf-8",
                              success: function (data) {
                                  if (data.d == "" || data.d == "/") {
                                      //showStickyToast(false, 'No \'Delivery\' is available for \'Pick N Check (Plant Delivery) Pending\'');
                                      
                                      showStickyToast(false, '\'Delv. Doc\'s are not available to display');
                                  }
                                  else {
                                      response($.map(data.d, function (item) {
                                          return {
                                              label: item,
                                          }

                                      }))
                                  }
                              }
                          });
                      },
                      minLength: 0
                  });

                  var textfieldname = $('#txtDelDocNumber');
                  DropdownFunction(textfieldname);
                  $('#txtDelDocNumber').autocomplete({
                      source: function (request, response) {
                          $.ajax({
                              url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadPNCPendingDelvDocNo_TC") %>',
                              data: "{ 'prefix': '" + request.term + "','TenantID':'" + document.getElementById('<%= this.hifPNCpTenantID.ClientID %>').value + "','WHID':'"+$("#hdnWarehouse").val()+"'}",
                              dataType: "json",
                              type: "POST",
                              contentType: "application/json; charset=utf-8",
                              success: function (data) {
                                  if (data.d == "" || data.d == "/") {
                                      //showStickyToast(false, 'No \'Delivery\' is available for \'Pick N Check Pending  \'');
                                       showStickyToast(false, '\'Delv. Doc\'s are not available to display');
                                  }
                                  else {
                                      response($.map(data.d, function (item) {
                                          return {
                                              label: item,
                                          }

                                      }))
                                  }
                              }
                          });
                      },
                      minLength: 0
                  });

                  var textfieldname = $('#txtPGIOBDNumber');
                  DropdownFunction(textfieldname);
                  $('#txtPGIOBDNumber').autocomplete({
                      source: function (request, response) {
                          $.ajax({
                              url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadPGIPendingDelvDocNo") %>',
                              data: "{ 'prefix': '" + request.term + "','TenantID':'" + document.getElementById('<%= this.hifPGITenantID.ClientID %>').value + "'}",
                              dataType: "json",
                              type: "POST",
                              contentType: "application/json; charset=utf-8",
                              success: function (data) {
                                  if (data.d == "" || data.d == "/") {
                                      //showStickyToast(false, 'No \'Delivery\' is available for \'PGI Pending \'');
                                       showStickyToast(false, '\'Delv. Doc\'s are not available to display');
                                  }
                                  else {
                                      response($.map(data.d, function (item) {
                                          return {
                                              label: item,
                                          }

                                      }))
                                  }
                              }
                          });
                      },
                      minLength: 0
                  });

                  var textfieldname = $('#txtDelvPOBDNumber');
                  DropdownFunction(textfieldname);
                  $('#txtDelvPOBDNumber').autocomplete({
                      source: function (request, response) {
                          $.ajax({
                              url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadDelvPendDelvDocNo") %>',
                              data: "{ 'prefix': '" + request.term + "','TenantID':'" + document.getElementById('<%= this.hifDPTenantID.ClientID %>').value + "'}",
                              dataType: "json",
                              type: "POST",
                              contentType: "application/json; charset=utf-8",
                              success: function (data) {
                                  if (data.d == "" || data.d == "/") {
                                      //showStickyToast(false, 'No \'Delivery\' is available for \'Deliveries Pending \'');
                                       showStickyToast(false, '\'Delv. Doc\'s are not available to display');
                                  }
                                  else {
                                      response($.map(data.d, function (item) {
                                          return {
                                              label: item,
                                          }

                                      }))
                                  }
                              }
                          });
                      },
                      minLength: 0
                  });
                  var textfieldname = $('#txtPODDelDocNo');
                  DropdownFunction(textfieldname);
                  $('#txtPODDelDocNo').autocomplete({
                      source: function (request, response) {
                          $.ajax({
                              url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadPODDelvDocNo") %>',
                              data: "{ 'prefix': '" + request.term + "','TenantID':'" + document.getElementById('<%= this.hifPPTenantID.ClientID %>').value + "'}",
                              dataType: "json",
                              type: "POST",
                              contentType: "application/json; charset=utf-8",
                              success: function (data) {
                                  if (data.d == "" || data.d == "/") {
                                      //showStickyToast(false, 'No \'Delivery\' is available for \'POD Pending \'');
                                       showStickyToast(false, '\'Delv. Doc\'s are not available to display');
                                  }
                                  else {
                                      response($.map(data.d, function (item) {
                                          return {
                                              label: item,
                                          }

                                      }))
                                  }
                              }
                          });
                      },
                      minLength: 0
                  });
                var TextFieldName = $("#<%= this.txtPNCpTenant.ClientID %>");
                DropdownFunction(TextFieldName);
                <%--$("#<%= this.txtPNCpTenant.ClientID %>").focusout(function () {
                    if ($("#<%= this.txtPNCpTenant.ClientID %>").val() == '') {
                        $("#<%= this.hifPNCpTenantID.ClientID %>").val('0');
                        $("#<%= this.txtPNCpTenant.ClientID %>").val('Tenant');
                    }                  
                })
                --%>
                $("#<%= this.txtPNCpTenant.ClientID %>").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '../mWebServices/FalconWebService.asmx/GetWarehouseTenant',
                            data: "{ 'prefix': '" + request.term + "','WHID':'" + $("#<%=hdnWarehouse.ClientID %>").val() +"'}",
                           <%-- url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadTenantDataFor3PL") %>',
                            data: "{ 'prefix': '" + request.term + "'}",//<=cp.TenantID%>--%>
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

                        $("#<%=hifPNCpTenantID.ClientID %>").val(i.item.val);
                        //$("#txtWarehouse").val("");
                        //$("#hdnWarehouse").val("0");

                    },
                    minLength: 0
                });

                var textfieldname = $('#txtVLPDDocNumber');
                DropdownFunction(textfieldname);
                $('#txtVLPDDocNumber').autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadVLPDDelvDocNo") %>',
                              data: "{ 'prefix': '" + request.term + "','TenantID':'" + document.getElementById('<%= this.hifVLPDTenantID.ClientID %>').value + "'}",
                              dataType: "json",
                              type: "POST",
                              contentType: "application/json; charset=utf-8",
                              success: function (data) {
                                  if (data.d == "" || data.d == "/") {
                                      //showStickyToast(false, 'No \'Delivery\' is available for \'VLPD Pending  \'');
                                      showStickyToast(false, '\'Delv. Doc\'s are not available to display');
                                  }
                                  else {
                                      response($.map(data.d, function (item) {
                                          return {
                                              label: item,
                                          }

                                      }))
                                  }
                              }
                          });
                      },
                      minLength: 0
                  });

                //---------------------------- added by durga on 02/04/2018 ------------------------//

                
                var TextFieldName = $("#<%= this.txtVLPTenant.ClientID %>");
                DropdownFunction(TextFieldName);
                $("#<%= this.txtVLPTenant.ClientID %>").focusout(function () {
                    if ($("#<%= this.txtVLPTenant.ClientID %>").val() == '') {
                        $("#<%= this.hifVLPDTenantID.ClientID %>").val('0');
                        $("#<%= this.txtVLPTenant.ClientID %>").val('Tenant');
                    }
                })

                $("#<%= this.txtVLPTenant.ClientID %>").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                           <%-- url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadTenantDataFor3PL") %>',
                            data: "{ 'prefix': '" + request.term + "'}",//<=cp.TenantID%>--%>
                              url: '../mWebServices/FalconWebService.asmx/GetWarehouseTenant',
                             data: "{ 'prefix': '" + request.term + "','WHID':'" + VLPWarehouseid +"'}",

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

                        $("#<%=hifVLPDTenantID.ClientID %>").val(i.item.val);
                          $("#<%=txtVLPDDocNumber.ClientID%>").val("");
                    },
                    minLength: 0
                });





                //-------------------------- end ----------------------------------------------------//

               // -------added by Meena------------
                 var textfieldname = $("#txtVLPWarehouse");
    //debugger;
    
    DropdownFunction(textfieldname);
    $("#txtVLPWarehouse").autocomplete({
        source: function (request, response) {
            debugger;
            $.ajax({
                //url: '../mWebServices/FalconWebService.asmx/LoadWHForWHList',
                //data: "{ 'prefix': '" + request.term + "','TenantID':'" + Tenantid + "'  }",
                url: '../mWebServices/FalconWebService.asmx/LoaDWHListBasedonUser',
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
            VLPWarehouseid = i.item.val;
        },
        minLength: 0
    });
   
//-------End---------------------
                 var textfieldname = $('#txtPGIWarehouse');
                  DropdownFunction(textfieldname);
                  $('#txtPGIWarehouse').autocomplete({
                      source: function (request, response) {
                          //if ($("#txtPNCpTenant").val() == "Tenant" || $("#txtPNCpTenant").val() == "") {
                          //    $("#hifPNCpTenantID").val("0");
                          //    $('#txtWarehouse').val("");
                          //    $("#hdnWarehouse").val("0");
                          //}
                          $.ajax({
                              url: '../mWebServices/FalconWebService.asmx/LoaDWHListBasedonUser',
                              data: "{ 'prefix': '" + request.term + "'}",   
                             <%-- url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadWHForWHList_CurrentStock") %>',
                              data: "{ 'prefix': '" + request.term + "','TenantID':'" + document.getElementById('<%= this.hifPNCpTenantID.ClientID %>').value + "'}",
                             --%> 
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
                          $("#<%=hifPGIWarehouseID.ClientID %>").val(i.item.val);
                      },
                      minLength: 0
                  });

                var TextFieldName = $("#<%= this.txtPGITenant.ClientID %>");
                DropdownFunction(TextFieldName);
                $("#<%= this.txtPGITenant.ClientID %>").focusout(function () {
                    if ($("#<%= this.txtPGITenant.ClientID %>").val() == '') {
                        $("#<%= this.hifPGITenantID.ClientID %>").val('0');
                        $("#<%= this.txtPGITenant.ClientID %>").val('Tenant');
                    }
                })

                $("#<%= this.txtPGITenant.ClientID %>").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                             url: '../mWebServices/FalconWebService.asmx/GetWarehouseTenant',
                            data: "{ 'prefix': '" + request.term + "','WHID':'" +  $("#<%=hifPGIWarehouseID.ClientID %>").val() + "'}",

                           <%-- url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadTenantDataFor3PL") %>',
                            data: "{ 'prefix': '" + request.term + "'}",//<=cp.TenantID%>--%>
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

                        $("#<%=hifPGITenantID.ClientID %>").val(i.item.val);

                    },
                    minLength: 0
                });

                  var textfieldname = $('#txtDPWarehouse');
                  DropdownFunction(textfieldname);
                  $('#txtDPWarehouse').autocomplete({
                      source: function (request, response) {
                          //if ($("#txtPNCpTenant").val() == "Tenant" || $("#txtPNCpTenant").val() == "") {
                          //    $("#hifPNCpTenantID").val("0");
                          //    $('#txtWarehouse').val("");
                          //    $("#hdnWarehouse").val("0");
                          //}
                          $.ajax({
                              url: '../mWebServices/FalconWebService.asmx/LoaDWHListBasedonUser',
                              data: "{ 'prefix': '" + request.term + "'}",   
                             <%-- url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadWHForWHList_CurrentStock") %>',
                              data: "{ 'prefix': '" + request.term + "','TenantID':'" + document.getElementById('<%= this.hifPNCpTenantID.ClientID %>').value + "'}",
                             --%> 
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
                          $("#<%=hifDPWarehouseID.ClientID %>").val(i.item.val);
                      },
                      minLength: 0
                  });

                var TextFieldName = $("#<%= this.txtDPTenant.ClientID %>");
                DropdownFunction(TextFieldName);
                $("#<%= this.txtDPTenant.ClientID %>").focusout(function () {
                    if ($("#<%= this.txtDPTenant.ClientID %>").val() == '') {
                        $("#<%= this.hifDPTenantID.ClientID %>").val('0');
                        $("#<%= this.txtDPTenant.ClientID %>").val('Tenant');
                    }
                })

                $("#<%= this.txtDPTenant.ClientID %>").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                           <%-- url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadTenantDataFor3PL") %>',
                            data: "{ 'prefix': '" + request.term + "'}",//<=cp.TenantID%>--%>
                             url: '../mWebServices/FalconWebService.asmx/GetWarehouseTenant',
                            data: "{ 'prefix': '" + request.term + "','WHID':'" +  $("#<%=hifDPWarehouseID.ClientID %>").val() +"'}",
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

                        $("#<%=hifDPTenantID.ClientID %>").val(i.item.val);

                    },
                    minLength: 0
                });


                
                  var textfieldname = $('#txtPPWarehouse');
                  DropdownFunction(textfieldname);
                  $('#txtPPWarehouse').autocomplete({
                      source: function (request, response) {
                        
                          $.ajax({
                              url: '../mWebServices/FalconWebService.asmx/LoaDWHListBasedonUser',
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
                          $("#<%=hifPPWarehouseID.ClientID %>").val(i.item.val);
                      },
                      minLength: 0
                  });
                var TextFieldName = $("#<%= this.txtPPTenant.ClientID %>");
                DropdownFunction(TextFieldName);
                $("#<%= this.txtPPTenant.ClientID %>").focusout(function () {
                    if ($("#<%= this.txtPPTenant.ClientID %>").val() == '') {
                        $("#<%= this.hifPPTenantID.ClientID %>").val('0');
                        $("#<%= this.txtPPTenant.ClientID %>").val('Tenant');
                    }
                })

                $("#<%= this.txtPPTenant.ClientID %>").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                           <%-- url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadTenantDataFor3PL") %>',
                            data: "{ 'prefix': '" + request.term + "'}",//<=cp.TenantID%>--%>
                             url: '../mWebServices/FalconWebService.asmx/GetWarehouseTenant',
                             data: "{ 'prefix': '" + request.term + "','WHID':'" +  $("#<%=hifPPWarehouseID.ClientID %>").val() +"'}",
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

                        $("#<%=hifPPTenantID.ClientID %>").val(i.item.val);

                    },
                    minLength: 0
                });

                //$('#txtWarehouse').val("");
                  var textfieldname = $('#txtWarehouse');
                  DropdownFunction(textfieldname);
                  $('#txtWarehouse').autocomplete({
                      source: function (request, response) {
                          //if ($("#txtPNCpTenant").val() == "Tenant" || $("#txtPNCpTenant").val() == "") {
                          //    $("#hifPNCpTenantID").val("0");
                          //    $('#txtWarehouse').val("");
                          //    $("#hdnWarehouse").val("0");
                          //}
                          $.ajax({
                              url: '../mWebServices/FalconWebService.asmx/LoaDWHListBasedonUser',
                              data: "{ 'prefix': '" + request.term + "'}",   
                             <%-- url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadWHForWHList_CurrentStock") %>',
                              data: "{ 'prefix': '" + request.term + "','TenantID':'" + document.getElementById('<%= this.hifPNCpTenantID.ClientID %>').value + "'}",
                             --%> 
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
                          $("#<%=hdnWarehouse.ClientID %>").val(i.item.val);
                      },
                      minLength: 0
                  });

              });
          }
          fnMCodeAC();
    </script>

    <script type="text/javascript">
        jQuery(document).ready(function () {
            jQuery('.PNCblink').blink();
            jQuery('.PGIblink').blink();
            jQuery('.DPblink').blink();
            jQuery('.PODblink').blink();
        });
    </script>


    <style type="text/css">
        .pagewidth {
            min-width: 1050px;
        }

        .ui-autocomplete-loading {
            background: white url('../Images/ui-anim_basic_16x16.gif') right center no-repeat;
        }

        .PNCblink, .PGIblink, .DPblink, .PODblink {
            color: white;
            font-size: 10pt;
            font-weight: bold;
            font-family: Calibri;
            width: 100px;
            background-color: red;
            border-radius: 5px;
            -ms-border-radius: 5px;
        }
        /*#MainContent_OBContent_pnlpickandcheck {
             position: absolute;
             right: 30px;
        }*/
        .obd{
           position: relative;
    top: -9px;
    left: 0px;
    display: flex;
        }
        #MainContent_OBContent_pnlpodpending{
            /*position: absolute;
             right: 30px;
                 top: 10px;*/
        }

        .flex__ div {
                margin-left: 5px;
        }

        gvLightGreenNew_pager td table tr {
            display: flex; justify-content: flex-end;
        }

        .mdd-select-underline {
            bottom:unset !important;
        }

    </style>

    <div class="dashed"></div>
    <div class="pagewidth">
    <table border="0" cellspacing="2" cellpadding="5" align="center">

<%--        <tr>
            <td align="right">
                <br />
                <a id="collapseAll" onclick="CollapseAll()" href="#" class=" ui-button-small">Collapse All <span class="space fa fa-compress"></span></a>&nbsp; &nbsp; 
                <a id="expandAll" onclick="ExpandAll()" href="#" class="ui-button-small">Expand All <span class="space fa fa-expand"></span></a>

            </td>
        </tr>--%>
        <tr>
            <td align="center">

                <div id="accordion" align="left">
                    
                     <!-- Globalization Tag is added for multilingual  -->







                    <%-- <h3>Pending OBD's For VLPD Creation   <span id="spnvlpdcount" runat="server" /><sup><span id="Span2" class="PNCblink" runat="server"></span></sup></h3>--%>
                     <h3><%= GetGlobalResourceObject("Resource", "PendingOBDsForVLPDCreation")%>   <span id="spnvlpdcount" runat="server" /><sup><span id="Span2" class="PNCblink" runat="server"></span></sup></h3>
                    <div>
                        <!--  Delivery's In Process  -->
                        
                            <asp:UpdateProgress ID="uprVLPDInProgress" runat="server" AssociatedUpdatePanelID="upnVLPDInProgress">
                            <ProgressTemplate>
                              <div style="width:100%; height:100%; z-index:999; position:fixed; top:0; left:0; right:0; bottom:0; align-items:center; display:flex; justify-content:center; background: #e0ddd8ba;">
                                <%--<div class="spinner">
                                  <div class="bounce1"></div>
                                  <div class="bounce2"></div>
                                  <div class="bounce3"></div>
                                </div>--%>
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
                        <asp:UpdatePanel ID="upnVLPDInProgress" runat="server" RenderMode="Inline" ViewStateMode="Enabled" ClientIDMode="Inherit" UpdateMode="Conditional">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="LinkButton2" />
                            </Triggers>
                            <ContentTemplate>
                                <table border="0" width="100%" cellpadding="5" cellspacing="5" align="center" >

                                    <tr>
                                        <td align="right">
                                            <asp:Panel runat="server" ID="pnlvlpdpending" DefaultButton="vlpdpending">
                                                <div style="float:right;">
                                                    <div class="flex__">
                                                        
                                                        <div class="flex">
                                                            <input type="text" id="txtVLPWarehouse" required="" />
                                                            <label>Warehouse</label>
                                                             &nbsp;&nbsp;
                                                            </div>
                                                        <div class="flex">
                                                            <asp:TextBox ID="txtVLPTenant" runat="server" SkinID="txt_Hidden_Req_Auto" ClientIDMode="Static" required=""></asp:TextBox>
                                                            <asp:HiddenField ID="hifVLPDTenantID" runat="server" value="0"/>
                                                          <%--  <label>Search Tenant</label>--%>
                                                              <label> <%= GetGlobalResourceObject("Resource", "SearchTenant")%></label>
                                                &nbsp;&nbsp;
                                                            </div>
                                                        <div class="flex">
                                                <asp:TextBox ID="txtVLPDDocNumber" runat="server" ClientIDMode="Static" SkinID="txt_Hidden_Req_Auto" required=""></asp:TextBox>
                                                            <%--<label>Search Delv. Doc.#</label>--%>
                                                            <label> <%= GetGlobalResourceObject("Resource", "SearchDelvDoc")%> </label>
                                                &nbsp;&nbsp;

                                                <asp:TextBox ID="TextBox3" runat="server" ClientIDMode="Static" Text="Search Kit Code ..." Visible="false" onfocus="ClearTextKitCode(this)" onblur="javascript:focuslostKitCode(this)"></asp:TextBox>
                                                &nbsp;&nbsp;
                                                            </div>
                                                        <div class="obd">
                                                            &nbsp;&nbsp;
                                                <%-- <asp:LinkButton runat="server" ID="vlpdpending" OnClick="lnnkVLPDPendingSearch_Click" CssClass="btn btn-primary"> Search<span class="space fa fa-search"></span></asp:LinkButton>--%>
                                                             <asp:LinkButton runat="server" ID="vlpdpending" OnClick="lnnkVLPDPendingSearch_Click" CssClass="btn btn-primary"> <%= GetGlobalResourceObject("Resource", "Search")%> <span class="space fa fa-search"></span></asp:LinkButton>
                                                &nbsp;&nbsp;
                                        
                                                  <%--<asp:ImageButton ID="imgbtngvOBDInProcess" runat="server" ImageAlign="AbsMiddle" ImageUrl="../Images/excel_icon.jpg" Width="20" OnClick="imgbtngvOBDInProcess_Click" ToolTip="Export To Excel" />--%>
                                                          <%--  <asp:LinkButton ID="LinkButton2" CssClass="btn btn-primary" runat="server" OnClick="imgbtngvOBDInProcess_Click">Export Excel <%=MRLWMSC21Common.CommonLogic.btnfaExcel %></asp:LinkButton>
                                                        --%>       <asp:LinkButton ID="LinkButton2" CssClass="btn btn-primary" runat="server" OnClick="imgbtngvOBDInVLPD_Click"><%= GetGlobalResourceObject("Resource", "ExportExcel")%>  <%=MRLWMSC21Common.CommonLogic.btnfaExcel %></asp:LinkButton>
                                                     
                                                               </div>
                                                        </div>
                                                    </div>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <%--<tr>
                                        <td class="FormLabels">
                                            <br />
                                            <asp:Label ID="lblDelPStatus" runat="server"></asp:Label>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td>

                                            <asp:GridView Width="100%" ShowFooter="true" ShowHeader="true" ShowHeaderWhenEmpty="true" GridLines="None" CellPadding="1" CellSpacing="1" ID="gvVLPDPending" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="true" PageSize="25" AllowSorting="True" SkinID="gvLightGrayNew" HorizontalAlign="Left" OnRowCommand="gvOBDInProcess_RowCommand" OnRowDataBound="gvVLPDPending_RowDataBound" OnSorting="gvOBDInProcess_Sorting" OnPageIndexChanging="gvOBDInProcess_PageIndexChanging">
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-Width="50" ItemStyle-CssClass="gvOBDNumber" ItemStyle-HorizontalAlign="Right" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ltPriorityLevel" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PriorityLevel") %>' />
                                                            <asp:Literal ID="ltPriorityTime" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PriorityDateTime") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                   <%-- <asp:TemplateField ItemStyle-Width="150" HeaderText="Delv. Doc. No." ItemStyle-CssClass="gvOBDNumber">--%>
                                                     <asp:TemplateField ItemStyle-Width="150" HeaderText="<%$Resources:Resource,DelvDocNo%>" ItemStyle-CssClass="gvOBDNumber">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="ltOBDNumber_Pending" Text='<%# Eval("OBDNumberRefNo") %>' />
                                                            <asp:Label runat="server" ID="lblOBDID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "OutboundID") %>'></asp:Label>
                                                            <asp:Label runat="server" ID="lblLineItemCount" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "LineCount") %>'></asp:Label>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                   <%-- <asp:TemplateField ItemStyle-Width="150" HeaderText="Delv. Doc. Type" ItemStyle-CssClass="home">--%>
                                                     <asp:TemplateField ItemStyle-Width="150" HeaderText="<%$Resources:Resource,DelvDocType%>" ItemStyle-CssClass="home">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltDelvType" Text='<%# DataBinder.Eval(Container.DataItem, "DocumentType").ToString() %>' />
                                                            <asp:Literal runat="server" ID="ltDocTypeID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "DocumentTypeID") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                   <%-- <asp:TemplateField ItemStyle-Width="150" HeaderText="Delv. Doc. Dt." ItemStyle-CssClass="home">--%>
                                                     <asp:TemplateField ItemStyle-Width="150" HeaderText="<%$Resources:Resource,DelvDocDt%>"  ItemStyle-CssClass="home">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltOBDDate_Pending" Text='<%# DataBinder.Eval(Container.DataItem, "OBDDate","{0: dd/MM/yy}") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <%--<asp:TemplateField ItemStyle-Width="250" HeaderText="Tenant">--%>
                                                    <asp:TemplateField ItemStyle-Width="250" HeaderText="<%$Resources:Resource,Tenant%>" >
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltTenantName" Text='<%# DataBinder.Eval(Container.DataItem, "TenantName") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                <%--    <asp:TemplateField ItemStyle-Width="250" HeaderText="Customer" ItemStyle-CssClass="home">--%>
                                                        <asp:TemplateField ItemStyle-Width="250" HeaderText="<%$Resources:Resource,Customer%>"  ItemStyle-CssClass="home">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltCustomerName" Text='<%# DataBinder.Eval(Container.DataItem, "CustomerName") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <%--<asp:TemplateField ItemStyle-Width="250" HeaderText="Requested By" ItemStyle-CssClass="home">--%>
                                                    <asp:TemplateField ItemStyle-Width="250" HeaderText="<%$Resources:Resource,RequestedBy%>"  ItemStyle-CssClass="home">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltRequestedBy" Text='<%# DataBinder.Eval(Container.DataItem, "RequestedBy").ToString() %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>



                                                    <%--<asp:TemplateField ItemStyle-Width="250" HeaderText="Warehouse" ItemStyle-CssClass="home">--%>
                                                    <asp:TemplateField ItemStyle-Width="250" HeaderText="<%$Resources:Resource,Warehouse%>" ItemStyle-CssClass="home">
                                                        <ItemTemplate>

                                                            <asp:Literal runat="server" ID="ltOBDDate" Text='<%# MRLWMSC21Common.CommonLogic.GetStoreNames(DataBinder.Eval(Container.DataItem, "ReferedStores").ToString(),"<br/>" ) %>' />
                                                        </ItemTemplate>

                                                    </asp:TemplateField>

                                                   <%-- <asp:TemplateField ItemStyle-CssClass="home" HeaderText="P.Note" ItemStyle-Width="300">--%>
                                                     <asp:TemplateField ItemStyle-CssClass="home" HeaderText="<%$Resources:Resource,PNote%>" ItemStyle-Width="300">
                                                        <ItemTemplate>
                                                            <asp:HyperLink runat="server" ID="lnkHyperLink" ToolTip="Pick Note | Pick note with barcoded material codes to pick items for delivery." Text="Pick Note" Font-Underline="false" ForeColor="Red"></asp:HyperLink>

                                                            <asp:Literal runat="server" ID="ltLineCount" Text='<%# String.Format("[{0}]",DataBinder.Eval(Container.DataItem, "LineCount"))%>'></asp:Literal>
                                                            <img src='../Images/redarrowright.gif' border='0' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <asp:TemplateField ItemStyle-CssClass="home" HeaderText="Del. Pick Note" ItemStyle-Width="300">
                                                        <ItemTemplate>
                                                            <asp:HyperLink runat="server" ID="lnkHyperLinkDel" ToolTip="Delivery Pick Note" Text="Del. Pick Note" Font-Underline="false" ForeColor="Red"></asp:HyperLink>

                                                            <asp:Literal runat="server" ID="ltLineCountDel" Text='<%# String.Format("[{0}]",DataBinder.Eval(Container.DataItem, "LineCount"))%>'></asp:Literal>
                                                            <img src='../Images/redarrowright.gif' border='0' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                  <%--  <asp:TemplateField ItemStyle-CssClass="home"  HeaderText="Change">--%>
                                                      <asp:TemplateField ItemStyle-CssClass="home"  HeaderText="<%$Resources:Resource,Change%>" >
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkRecievePGI" runat="server" CssClass="GvLink" PostBackUrl='<%# Eval("OutboundID", "OutboundDetails.aspx?obdid={0}&edittype=2") %>' Visible="false" Text="<nobr> Edit <img src='../Images/redarrowright.gif' border='0' /></nobr>" ToolTip="Recieve for PGI" />

                                                            <asp:HyperLink ID="HyperLink1" Text="<nobr><i class='material-icons'>settings</i></nobr>" NavigateUrl='<%#   Eval("OutboundID", "OutboundDetails.aspx?obdid={0}&edittype=2")  %>' Font-Underline="false" runat="server"></asp:HyperLink>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EmptyDataTemplate>
				                                  <%--  <div align="center">No Data Found</div>--%>
                                                      <div align="center"><%= GetGlobalResourceObject("Resource", "NoDataFound ")%></div>
			                                    </EmptyDataTemplate>
                                            </asp:GridView>


                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <!--  Delivery's In Process  -->
                    </div>

                    <%--<h3>Pick N Check Pending    <span id="lblDeliveryRecCount" runat="server" /><sup><span id="lbltodayRecCount" class="PNCblink" runat="server"></span></sup></h3>--%>
                    <h3> <%= GetGlobalResourceObject("Resource", "PickNCheckPending")%>    <span id="lblDeliveryRecCount" runat="server" /><sup><span id="lbltodayRecCount" class="PNCblink" runat="server"></span></sup></h3>
                    <div>
                        <!--  Delivery's In Process  -->
                        
                            <asp:UpdateProgress ID="uprgDelInProgress" runat="server" AssociatedUpdatePanelID="upnlDelInProgress">
                            <ProgressTemplate>
                              <div style="width:100%; height:100%; z-index:999; position:fixed; top:0; left:0; right:0; bottom:0; align-items:center; display:flex; justify-content:center; background: #e0ddd8ba;">
                                <%--<div class="spinner">
                                  <div class="bounce1"></div>
                                  <div class="bounce2"></div>
                                  <div class="bounce3"></div>
                                </div>--%>
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
                        <asp:UpdatePanel ID="upnlDelInProgress" runat="server" RenderMode="Inline" ViewStateMode="Enabled" ClientIDMode="Inherit" UpdateMode="Conditional">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="imgbtngvOBDInProcess" />
                            </Triggers>
                            <ContentTemplate>
                                <table border="0" width="100%" cellpadding="5" cellspacing="5" align="center" >

                                    <tr>
                                        <td align="right">
                                            <asp:Panel runat="server" ID="pnlpickandcheck" DefaultButton="lnkDelSearch">
                                                <div style="float:right;">
                                                    <div class="flex__">
                                                        
                                                         <div class="flex">
                                                            <asp:TextBox ID="txtWarehouse" runat="server" ClientIDMode="Static" SkinID="txt_Hidden_Req_Auto" required=""></asp:TextBox> 
                                                             <asp:HiddenField ID="hdnWarehouse" runat="server" Value="0" ClientIDMode="Static" />
                                                            <label>Warehouse</label>                                                            
                                                          &nbsp;&nbsp;
                                                        </div>
                                                        <div class="flex">
                                                            <asp:TextBox ID="txtPNCpTenant" runat="server" SkinID="txt_Hidden_Req_Auto" ClientIDMode="Static" required=""></asp:TextBox>
                                                            <asp:HiddenField ID="hifPNCpTenantID" runat="server" Value="0" ClientIDMode="Static" />
                                                           <%-- <label>Search Tenant</label>--%>
                                                             <label> <%= GetGlobalResourceObject("Resource", "SearchTenant")%></label>
                                                            &nbsp;&nbsp;
                                                        </div>


                                                        <div class="flex">
                                                            <asp:TextBox ID="txtDelDocNumber" runat="server" ClientIDMode="Static" SkinID="txt_Hidden_Req_Auto" required=""></asp:TextBox>
                                                           
                                                            <%--<label>Search Delv. Doc.#</label>--%>
                                                            <label> <%= GetGlobalResourceObject("Resource", "SearchDelvDoc")%></label>
                                                            <asp:TextBox ID="txtDIPKitCode" runat="server" ClientIDMode="Static" Text="Search Kit Code ..." Visible="false" onfocus="ClearTextKitCode(this)" onblur="javascript:focuslostKitCode(this)"></asp:TextBox>
                                                          &nbsp;&nbsp;
                                                        </div>
                                                        <div class="obd">
                                                            &nbsp;&nbsp;
                                                <%-- <asp:LinkButton runat="server" ID="lnkDelSearch" OnClick="lnkDelSearch_Click" CssClass="btn btn-primary"> Search<span class="space fa fa-search"></span></asp:LinkButton>--%>
                                                             <asp:LinkButton runat="server" ID="lnkDelSearch" OnClick="lnkDelSearch_Click" CssClass="btn btn-primary">  <%= GetGlobalResourceObject("Resource", "Search")%><span class="space fa fa-search"></span></asp:LinkButton>
                                                &nbsp;&nbsp;
                                        
                                                  <%--<asp:ImageButton ID="imgbtngvOBDInProcess" runat="server" ImageAlign="AbsMiddle" ImageUrl="../Images/excel_icon.jpg" Width="20" OnClick="imgbtngvOBDInProcess_Click" ToolTip="Export To Excel" />--%>
                                                           <%-- <asp:LinkButton ID="imgbtngvOBDInProcess" CssClass="btn btn-primary" runat="server" OnClick="imgbtngvOBDInProcess_Click">Export Excel <%=MRLWMSC21Common.CommonLogic.btnfaExcel %></asp:LinkButton>--%>
                                                             <asp:LinkButton ID="imgbtngvOBDInProcess" CssClass="btn btn-primary" runat="server" OnClick="imgbtngvOBDInProcess_Click"><%= GetGlobalResourceObject("Resource", "ExportExcel")%>  <%=MRLWMSC21Common.CommonLogic.btnfaExcel %></asp:LinkButton>
                                                            </div>
                                                        </div>
                                                    </div>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <%--<tr>
                                        <td class="FormLabels">
                                            <br />
                                            <asp:Label ID="lblDelPStatus" runat="server"></asp:Label>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td>

                                            <asp:GridView Width="100%" ShowFooter="true" ShowHeader="true" ShowHeaderWhenEmpty="true" GridLines="None" CellPadding="1" CellSpacing="1" ID="gvOBDInProcess" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="true" PageSize="25" AllowSorting="True" SkinID="gvLightGrayNew" HorizontalAlign="Left" OnRowCommand="gvOBDInProcess_RowCommand" OnRowDataBound="gvOBDInProcess_RowDataBound" OnSorting="gvOBDInProcess_Sorting" OnPageIndexChanging="gvOBDInProcess_PageIndexChanging">
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-Width="50" ItemStyle-CssClass="gvOBDNumber" HeaderText="<%$Resources:Resource,OBDNumber%>" Visible="false" ItemStyle-HorizontalAlign="Right">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ltPriorityLevel" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PriorityLevel") %>' />
                                                            <asp:Literal ID="ltPriorityTime" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PriorityDateTime") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:TemplateField ItemStyle-Width="150" HeaderText="Delv. Doc. No." ItemStyle-CssClass="gvOBDNumber">--%>
                                                    <asp:TemplateField ItemStyle-Width="150" HeaderText="<%$Resources:Resource,DelvDocNo%>" ItemStyle-CssClass="gvOBDNumber">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="ltOBDNumber_Pending" Text='<%# Eval("OBDNumberRefNo") %>' />
                                                            <asp:Label runat="server" ID="lblOBDID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "OutboundID") %>'></asp:Label>
                                                            <asp:Label runat="server" ID="lblLineItemCount" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "LineCount") %>'></asp:Label>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                   <%-- <asp:TemplateField ItemStyle-Width="150" HeaderText="Delv. Doc. Type" ItemStyle-CssClass="home">--%>
                                                     <asp:TemplateField ItemStyle-Width="150" HeaderText="<%$Resources:Resource,DelvDocType%>"  ItemStyle-CssClass="home">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltDelvType" Text='<%# DataBinder.Eval(Container.DataItem, "DocumentType").ToString() %>' />
                                                            <asp:Literal runat="server" ID="ltDocTypeID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "DocumentTypeID") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                              <%--      <asp:TemplateField ItemStyle-Width="150" HeaderText="Delv. Doc. Dt." ItemStyle-CssClass="home">--%>
                                                          <asp:TemplateField ItemStyle-Width="150" HeaderText="<%$Resources:Resource,DelvDocDt%>"  ItemStyle-CssClass="home">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltOBDDate_Pending" Text='<%# DataBinder.Eval(Container.DataItem, "OBDDate","{0: dd/MM/yy}") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <%--<asp:TemplateField ItemStyle-Width="250" HeaderText="Tenant">--%>
                                                    <asp:TemplateField ItemStyle-Width="250" HeaderText="<%$Resources:Resource,Tenant%>" >
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltTenantName" Text='<%# DataBinder.Eval(Container.DataItem, "TenantName") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <%--<asp:TemplateField ItemStyle-Width="250" HeaderText="Customer" ItemStyle-CssClass="home">--%>
                                                    <asp:TemplateField ItemStyle-Width="250" HeaderText="<%$Resources:Resource,Customer%>"   ItemStyle-CssClass="home">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltCustomerName" Text='<%# DataBinder.Eval(Container.DataItem, "CustomerName") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <%--<asp:TemplateField ItemStyle-Width="250" HeaderText="Requested By" ItemStyle-CssClass="home">--%>
                                                    <asp:TemplateField ItemStyle-Width="250" HeaderText="<%$Resources:Resource,RequestedBy%>"   ItemStyle-CssClass="home">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltRequestedBy" Text='<%# DataBinder.Eval(Container.DataItem, "RequestedBy").ToString() %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>



                                                    <%--<asp:TemplateField ItemStyle-Width="250" HeaderText="Warehouse" ItemStyle-CssClass="home">--%>
                                                    <asp:TemplateField ItemStyle-Width="250" HeaderText="<%$Resources:Resource,Warehouse%>" ItemStyle-CssClass="home">
                                                        <ItemTemplate>

                                                            <asp:Literal runat="server" ID="ltOBDDate" Text='<%# MRLWMSC21Common.CommonLogic.GetStoreNames(DataBinder.Eval(Container.DataItem, "ReferedStores").ToString(),"<br/>" ) %>' />
                                                        </ItemTemplate>

                                                    </asp:TemplateField>

                                                    <%--<asp:TemplateField ItemStyle-CssClass="home" HeaderText="P.Note" ItemStyle-Width="300">--%>
                                                    <asp:TemplateField ItemStyle-CssClass="home" HeaderText="<%$Resources:Resource,PNote%>"  ItemStyle-Width="300">
                                                        <ItemTemplate>
                                                            <asp:HyperLink runat="server" ID="lnkHyperLink" ToolTip="Pick Note | Pick note with barcoded material codes to pick items for delivery." Text="Pick Note" Font-Underline="false" ForeColor="Red"></asp:HyperLink>

                                                            <asp:Literal runat="server" ID="ltLineCount" Text='<%# String.Format("[{0}]",DataBinder.Eval(Container.DataItem, "LineCount"))%>'></asp:Literal>
                                                            <img src='../Images/redarrowright.gif' border='0' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    
                                                    <asp:TemplateField ItemStyle-CssClass="home" HeaderText="Del. Pick Note" ItemStyle-Width="300">
                                                        <ItemTemplate>
                                                            <asp:HyperLink runat="server" ID="lnkHyperLinkDel" ToolTip="Delivery Pick Note" Text="Del. Pick Note" Font-Underline="false" ForeColor="Red"></asp:HyperLink>

                                                            <asp:Literal runat="server" ID="ltLineCountDel" Text='<%# String.Format("[{0}]",DataBinder.Eval(Container.DataItem, "LineCount"))%>'></asp:Literal>
                                                            <img src='../Images/redarrowright.gif' border='0' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <%--<asp:TemplateField ItemStyle-CssClass="home"  HeaderText="Change">--%>
                                                    <asp:TemplateField ItemStyle-CssClass="home"  HeaderText="<%$Resources:Resource,Change%>"  >
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkRecievePGI" runat="server" CssClass="GvLink" PostBackUrl='<%# Eval("OutboundID", "OutboundDetails.aspx?obdid={0}&edittype=2") %>' Visible="false" Text="<nobr> Edit <img src='../Images/redarrowright.gif' border='0' /></nobr>" ToolTip="Recieve for PGI" />

                                                            <asp:HyperLink ID="HyperLink1" Text="<nobr><i class='material-icons'>settings</i></nobr>" NavigateUrl='<%#   Eval("OutboundID", "OutboundDetails.aspx?obdid={0}&edittype=2")  %>' Font-Underline="false" runat="server"></asp:HyperLink>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EmptyDataTemplate>
				                                   <%-- <div align="center">No Data Found</div>--%>
                                                     <div align="center"> <%= GetGlobalResourceObject("Resource", "NoDataFound")%></div>
			                                    </EmptyDataTemplate>
                                            </asp:GridView>


                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <!--  Delivery's In Process  -->
                    </div>




                    <%--<h3>PGI Pending   <span id="lblPGIRecordCount" runat="server" /><sup><span id="lbltodayPGIRecCount" class="PGIblink" runat="server"></span></sup></h3>--%>
                    <h3> <%= GetGlobalResourceObject("Resource", "PGIPending")%> <span id="lblPGIRecordCount" runat="server" /><sup><span id="lbltodayPGIRecCount" class="PGIblink" runat="server"></span></sup></h3>
                    <div>
                        <!-- PGI Pending -->
                        
                            <asp:UpdateProgress ID="uprgPGIPending" runat="server" AssociatedUpdatePanelID="upnlPGIPending">
                            <ProgressTemplate>
                              <div style="width:100%; height:100%; z-index:999; position:fixed; top:0; left:0; right:0; bottom:0; align-items:center; display:flex; justify-content:center; background: #e0ddd8ba;">
                                <%--<div class="spinner">
                                  <div class="bounce1"></div>
                                  <div class="bounce2"></div>
                                  <div class="bounce3"></div>
                                </div>--%>
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
                        <asp:UpdatePanel ID="upnlPGIPending" runat="server" RenderMode="Inline" ViewStateMode="Enabled" ClientIDMode="Inherit" UpdateMode="Conditional">
                             <Triggers>
                                <asp:PostBackTrigger ControlID="imgbtngvOBDPGIPending" />
                            </Triggers>
                            <ContentTemplate>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0" align="center">

                                    <tr>
                                        <td align="right">
                                            <asp:Panel ID="pnlpgipending" runat="server" DefaultButton="lnklPGISearch">
                                                <div style="float: right;">
                                                    <div class="flex__">
                                                         <div class="flex">
                                                         <input type="text" id="txtPGIWarehouse" required="" />
                                                         <label>Warehouse</label>
                                                             &nbsp;&nbsp;
                                                              <asp:HiddenField ID="hifPGIWarehouseID" runat="server" Value="0" />
                                                         </div>
                                                        <div class="flex">
                                                            <asp:TextBox ID="txtPGITenant" runat="server" SkinID="txt_Hidden_Req_Auto" ClientIDMode="Static" required=""></asp:TextBox>
                                                            <%--<label>Search Tenant</label>--%>
                                                            <label><%= GetGlobalResourceObject("Resource", "SearchTenant")%></label>
                                                            <asp:HiddenField ID="hifPGITenantID" runat="server" Value="0" />
                                                            &nbsp;&nbsp;
                                                        </div>
                                                        <div class="flex">
                                                            <asp:TextBox ID="txtPGIOBDNumber" runat="server" ClientIDMode="Static" SkinID="txt_Hidden_Req_Auto" required=""></asp:TextBox>
                                                          <%--  <label>Search Delv. Doc.#</label>--%>
                                                              <label> <%= GetGlobalResourceObject("Resource", "SearchDelvDoc")%></label>
                                                            &nbsp;&nbsp;

                                                            <asp:TextBox ID="txtPGIKitCode" runat="server" ClientIDMode="Static" Text="Search Kit Code ..." Visible="false" onfocus="ClearTextKitCode(this)" onblur="javascript:focuslostKitCode(this)"></asp:TextBox>
                                                            &nbsp;&nbsp;
                                                        </div>
                                                        <div class="obd">
                                                            &nbsp;&nbsp;
                                                            <asp:LinkButton runat="server" ID="lnklPGISearch" OnClick="lnklPGISearch_Click" CssClass="btn btn-primary"> Search <span class="space fa fa-search"></span></asp:LinkButton>
                                                            &nbsp;&nbsp;
                                       
                                         
                                                   <%--<asp:ImageButton ID="imgbtngvOBDPGIPending" runat="server" ImageAlign="AbsMiddle" ImageUrl="../Images/excel_icon.jpg" Width="20" OnClick="imgbtngvOBDPGIPending_Click" ToolTip="Export To Excel" />--%>
                                                           <%-- <asp:LinkButton ID="imgbtngvOBDPGIPending" CssClass="btn btn-primary" runat="server" OnClick="imgbtngvOBDPGIPending_Click">Export Excel <%=MRLWMSC21Common.CommonLogic.btnfaExcel %></asp:LinkButton>--%>
                                                             <asp:LinkButton ID="imgbtngvOBDPGIPending" CssClass="btn btn-primary" runat="server" OnClick="imgbtngvOBDPGIPending_Click"><%= GetGlobalResourceObject("Resource", "ExportExcel")%> <%=MRLWMSC21Common.CommonLogic.btnfaExcel %></asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                   <%-- <tr>
                                        <td class="FormLabels">
                                            <br />
                                            <asp:Label ID="lblPGIStatus" runat="server"></asp:Label>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td>

                                            <asp:GridView Width="100%" ShowFooter="true" ShowHeader="true" ShowHeaderWhenEmpty="true" GridLines="None" CellPadding="1" CellSpacing="1" ID="gvOBDPGIPending" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="true" PageSize="25" AllowSorting="True" SkinID="gvLightGreenNew" HorizontalAlign="Left" OnSorting="gvOBDPGIPending_Sorting" OnPageIndexChanging="gvOBDPGIPending_PageIndexChanging" OnRowDataBound="gvOBDPGIPending_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-Width="50" ItemStyle-CssClass="gvOBDNumber" ItemStyle-HorizontalAlign="Right" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ltPriorityLevel" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PriorityLevel") %>' />
                                                            <asp:Literal ID="ltPriorityTime" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PriorityDateTime") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:TemplateField ItemStyle-Width="150" HeaderText="Delv. Doc. No." ItemStyle-CssClass="gvOBDNumber">--%>
                                                    <asp:TemplateField ItemStyle-Width="150" HeaderText="<%$Resources:Resource,DelvDocNo%>" ItemStyle-CssClass="gvOBDNumber">
                                                        <ItemTemplate>

                                                            <asp:Literal runat="server" ID="ltOBDNumber_Pending" Text='<%# DataBinder.Eval(Container.DataItem, "OBDNumberRefNo") %>' />
                                                            <asp:Label runat="server" ID="lblOBDID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "OutboundID") %>'></asp:Label>
                                                            <asp:Label runat="server" ID="lblLineItemCount" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "LineCount") %>'></asp:Label>


                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <%--<asp:TemplateField ItemStyle-Width="180" HeaderText="Delv. Doc. Type" ItemStyle-CssClass="home">--%>
                                                    <asp:TemplateField ItemStyle-Width="180" HeaderText="<%$Resources:Resource,DelvDocType%>" ItemStyle-CssClass="home">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltDelvType" Text='<%# DataBinder.Eval(Container.DataItem, "DocumentType").ToString() %>' />
                                                            <asp:Literal runat="server" ID="ltDocTypeID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "DocumentTypeID") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                   <%-- <asp:TemplateField ItemStyle-Width="150" HeaderText="Delv. Doc. Dt." ItemStyle-CssClass="home">--%>
                                                     <asp:TemplateField ItemStyle-Width="150" HeaderText="<%$Resources:Resource,DelvDocDt%>"  ItemStyle-CssClass="home">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltOBDDate_Pending" Text='<%# DataBinder.Eval(Container.DataItem, "OBDDate","{0: dd/MM/yy}") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <%--<asp:TemplateField ItemStyle-Width="250" HeaderText="Tenant">--%>
                                                    <asp:TemplateField ItemStyle-Width="250" HeaderText="<%$Resources:Resource,Tenant%>" >
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltTenantName" Text='<%# DataBinder.Eval(Container.DataItem, "TenantName") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                   <%-- <asp:TemplateField ItemStyle-Width="250" HeaderText="Customer" ItemStyle-CssClass="home">--%>
                                                     <asp:TemplateField ItemStyle-Width="250" HeaderText="<%$Resources:Resource,Customer%>"  ItemStyle-CssClass="home">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltCustomerName" Text='<%# DataBinder.Eval(Container.DataItem, "CustomerName") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <%--<asp:TemplateField ItemStyle-Width="250" HeaderText="Requested By" ItemStyle-CssClass="home">--%>
                                                    <asp:TemplateField ItemStyle-Width="250" HeaderText="<%$Resources:Resource,RequestedBy%>"   ItemStyle-CssClass="home">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltRequestedBy" Text='<%# DataBinder.Eval(Container.DataItem, "RequestedBy").ToString() %>'></asp:Literal>
                                                        </ItemTemplate>

                                                    </asp:TemplateField>



                                                    <asp:TemplateField HeaderStyle-Height="10px" ItemStyle-Width="200" ItemStyle-CssClass="home" HeaderText="Received for PGI">
                                                        <HeaderTemplate>
                                                            <table align="center" cellpadding="0" cellspacing="0" height="10px">
                                                                <tr>
                                                                  <%--  <td colspan="2" align="center">Received for PGI </td>--%>
                                                                      <td colspan="2" align="center"> <%= GetGlobalResourceObject("Resource", "ReceivedforPGI")%> </td>
                                                                </tr>
                                                                <tr>
                                                                    <%--<td width="80px" align="center">Date</td>--%>
                                                                    <td width="80px" align="center"> <%= GetGlobalResourceObject("Resource", "Date")%></td>
                                                                    <%--<td width="80px" align="center">Time</td>--%>
                                                                    <td width="80px" align="center">  <%= GetGlobalResourceObject("Resource", "Time")%> </td>
                                                                </tr>
                                                            </table>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td width="70px" align="center">
                                                                        <asp:Literal runat="server" ID="ltGTSDate" Text='<%# DataBinder.Eval(Container.DataItem, "SentForPGIOn","{0: dd-MMM-yyyy}") %>' /></td>
                                                                    <td width="70px" align="center">
                                                                        <asp:Literal runat="server" ID="ltOGTSTime" Text='<%# DataBinder.Eval(Container.DataItem, "SentForPGIOn","{0: hh :mm tt}") %>' /></td>
                                                                </tr>
                                                            </table>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                   <%-- <asp:TemplateField ItemStyle-Width="180" HeaderText="Warehouse" ItemStyle-CssClass="home">--%>
                                                     <asp:TemplateField ItemStyle-Width="180" HeaderText="<%$Resources:Resource,Warehouse%>" ItemStyle-CssClass="home">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltOBDDate" Text='<%# MRLWMSC21Common.CommonLogic.GetStoreNamesWithPNCStatus(DataBinder.Eval(Container.DataItem, "ReferedStores").ToString(),"<br>", DataBinder.Eval(Container.DataItem,"OBDNumber").ToString() , DataBinder.Eval(Container.DataItem,"OutboundID").ToString() ,DataBinder.Eval(Container.DataItem,"TenantID").ToString()) %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                  <%--  <asp:TemplateField ItemStyle-CssClass="home" HeaderText="P.Note" ItemStyle-Width="300">--%>
                                                      <asp:TemplateField ItemStyle-CssClass="home" HeaderText="<%$Resources:Resource,PNote%>"  ItemStyle-Width="300">
                                                        <ItemTemplate>
                                                            <asp:HyperLink runat="server" ID="lnkHyperLink" ToolTip="Pick Note | Pick note with barcoded material codes to pick items for delivery." Text="Pick Note" Font-Underline="false" ForeColor="Red"></asp:HyperLink>

                                                            <asp:Literal runat="server" ID="ltLineCount" Text='<%# String.Format("[{0}]",DataBinder.Eval(Container.DataItem, "LineCount"))%>'></asp:Literal>
                                                            <img src='../Images/redarrowright.gif' border='0' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                     <asp:TemplateField ItemStyle-CssClass="home" HeaderText="Del. Pick Note" ItemStyle-Width="300">
                                                        <ItemTemplate>
                                                            <asp:HyperLink runat="server" ID="lnkHyperLinkDel" ToolTip="Delivery Pick Note" Text="Del. Pick Note" Font-Underline="false" ForeColor="Red"></asp:HyperLink>

                                                            <asp:Literal runat="server" ID="ltLineCountDel" Text='<%# String.Format("[{0}]",DataBinder.Eval(Container.DataItem, "LineCount"))%>'></asp:Literal>
                                                            <img src='../Images/redarrowright.gif' border='0' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <%--<asp:TemplateField ItemStyle-CssClass="home" HeaderText="Change">--%>
                                                    <asp:TemplateField ItemStyle-CssClass="home" HeaderText="<%$Resources:Resource,Change%>" >
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkRecievePGI" runat="server" Visible="false" CssClass="GvLink" PostBackUrl='<%# Eval("OutboundID", "OutboundDetails.aspx?obdid={0}&edittype=3") %>' Text="<nobr> Change <img src='../Images/redarrowright.gif' border='0' /></nobr>" ToolTip="Recieve for PGI" />

                                                            <asp:HyperLink ID="HyperLink1" Text="<nobr> <i class='material-icons'>settings</i></nobr>" NavigateUrl='<%#   Eval("OutboundID", "OutboundDetails.aspx?obdid={0}&edittype=3")  %>' Font-Underline="false" runat="server"></asp:HyperLink>
                                                        </ItemTemplate>

                                                    </asp:TemplateField>

                                                </Columns>
                                                <EmptyDataTemplate>
										           <%-- <div align="center">No Data Found</div>--%>
                                                     <div align="center"> <%= GetGlobalResourceObject("Resource", "NoDataFound")%></div>
									            </EmptyDataTemplate>
                                            </asp:GridView>

                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <!-- PGI Pending -->
                    </div>


                    <%--<h3>Deliveries Pending   <span id="lblDelvPRecCount" runat="server" /><sup><span id="lbltodayDPRecCount" class="DPblink" runat="server"></span></sup></h3>--%>
                    <h3><%= GetGlobalResourceObject("Resource", "DeliveriesPending")%>  <span id="lblDelvPRecCount" runat="server" /><sup><span id="lbltodayDPRecCount" class="DPblink" runat="server"></span></sup></h3>

                    <div>
                        <!-- Deliveries Pending -->
                        
                            <asp:UpdateProgress ID="unrgDeliveryPending" runat="server" AssociatedUpdatePanelID="unplDeliveryPending">
                            <ProgressTemplate>
                              <div style="width:100%; height:100%; z-index:999; position:fixed; top:0; left:0; right:0; bottom:0; align-items:center; display:flex; justify-content:center; background: #e0ddd8ba;">
                                <%--<div class="spinner">
                                  <div class="bounce1"></div>
                                  <div class="bounce2"></div>
                                  <div class="bounce3"></div>
                                </div>--%>
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
                        <asp:UpdatePanel ID="unplDeliveryPending" runat="server" RenderMode="Inline" ViewStateMode="Enabled" ClientIDMode="Inherit" UpdateMode="Conditional">
                             <Triggers>
                                <asp:PostBackTrigger ControlID="imgbtngvOBDReceived" />
                            </Triggers>
                            <ContentTemplate>
                                <table border="0" width="100%" cellpadding="5" cellspacing="5" align="center" >

                                    <tr>
                                        <td align="right">
                                            <asp:Panel runat="server" ID="pnldeliverypending" DefaultButton="lnkDelvpSearch">
                                                <div style="float: right;">
                                                    <div class="flex__">
                                                         <div class="flex">
                                                         <input type="text" id="txtDPWarehouse" required="" />
                                                         <label>Warehouse</label>
                                                             &nbsp;&nbsp;
                                                              <asp:HiddenField ID="hifDPWarehouseID" runat="server" Value="0" />
                                                         </div>
                                                        <div class="flex">
                                                            <asp:TextBox ID="txtDPTenant" runat="server" SkinID="txt_Hidden_Req_Auto"  ClientIDMode="Static" required=""></asp:TextBox>
                                                           <%-- <label>Search Tenant</label>--%>
                                                             <label> <%= GetGlobalResourceObject("Resource", "SearchTenant")%></label>
                                                            <asp:HiddenField ID="hifDPTenantID" runat="server" Value="0" />
                                                            &nbsp;&nbsp;
                                                        </div>
                                                        <div class="flex">
                                                            <asp:TextBox ID="txtDelvPOBDNumber" ClientIDMode="Static" runat="server" SkinID="txt_Hidden_Req_Auto" required=""></asp:TextBox>
                                                         <%--   <label>Search Delv. Doc.#</label>--%>
                                                              
                                                            &nbsp;&nbsp;
                                                            <asp:TextBox ID="txtDelvPKitCode" runat="server" ClientIDMode="Static" Text="Search Kit Code ..." Visible="false" onfocus="ClearTextKitCode(this)" onblur="javascript:focuslostKitCode(this)"></asp:TextBox>
                                                            &nbsp;&nbsp;
                                                        </div>
                                                        <div class="obd">
                                                            &nbsp;&nbsp;
                                                          <%--  <asp:LinkButton runat="server" Text="Search" ID="lnkDelvpSearch" OnClick="lnkDelvpSearch_Click" CssClass="btn btn-primary"> Search <span class="space fa fa-search"></span></asp:LinkButton>--%>
                                                              <asp:LinkButton runat="server" Text="Search" ID="lnkDelvpSearch" OnClick="lnkDelvpSearch_Click" CssClass="btn btn-primary"> <%= GetGlobalResourceObject("Resource", "Search")%> <span class="space fa fa-search"></span></asp:LinkButton>
                                                            &nbsp;&nbsp;
                                         
                                                 <%--<asp:ImageButton ID="imgbtngvOBDReceived" runat="server" ImageAlign="AbsMiddle" ImageUrl="../Images/excel_icon.jpg" Width="20" OnClick="imgbtngvOBDReceived_Click" ToolTip="Export To Excel" />--%>
                                                          <%--  <asp:LinkButton ID="imgbtngvOBDReceived" CssClass="btn btn-primary" runat="server" OnClick="imgbtngvOBDReceived_Click">Export Excel <%=MRLWMSC21Common.CommonLogic.btnfaExcel %></asp:LinkButton>--%>
                                                              <asp:LinkButton ID="imgbtngvOBDReceived" CssClass="btn btn-primary" runat="server" OnClick="imgbtngvOBDReceived_Click"><%= GetGlobalResourceObject("Resource", "ExportExcel")%> <%=MRLWMSC21Common.CommonLogic.btnfaExcel %></asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </td>

                                    </tr>
                                  <%--  <tr>
                                        <td class="FormLabels">
                                            <br />
                                            <asp:Label ID="lblDelvPStatus" runat="server"></asp:Label>
                                        </td>
                                    </tr>--%>

                                    <tr>
                                        <td>

                                            <asp:GridView Width="100%" ShowHeader="true" ShowHeaderWhenEmpty="true" ShowFooter="true" GridLines="None" CellPadding="1" CellSpacing="1" ID="gvOBDReceived" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="true" PageSize="25" AllowSorting="True" SkinID="gvLightOrangeNew" HorizontalAlign="Left" OnRowDataBound="gvOBDDeliveryPending_OnRowDataBound" OnSorting="gvOBDDeliveryPending_Sorting" OnPageIndexChanging="gvOBDDeliveryPending_PageIndexChanging">

                                                <Columns>

                                                    <%--<asp:TemplateField ItemStyle-Width="150" HeaderText="Delv. Doc. No." HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="gvOBDNumber">--%>
                                                    <asp:TemplateField ItemStyle-Width="150" HeaderText="<%$Resources:Resource,DelvDocNo%>" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="gvOBDNumber" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltOBDNumber_Pending" Text='<%# DataBinder.Eval(Container.DataItem, "OBDNumberRefNo") %>' />
                                                            <asp:Label runat="server" ID="lblOBDID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "OutboundID") %>'></asp:Label>
                                                            <asp:Label runat="server" ID="lblLineItemCount" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "LineCount") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <%--<asp:TemplateField ItemStyle-Width="150" HeaderText="Delv. Doc. Type" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home">--%>
                                                    <asp:TemplateField ItemStyle-Width="150" HeaderText="<%$Resources:Resource,DelvDocType%>"  HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltDelvType" Text='<%# DataBinder.Eval(Container.DataItem, "DocumentType").ToString() %>' />
                                                            <asp:Literal runat="server" ID="ltDocTypeID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "DocumentTypeID") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <%--<asp:TemplateField ItemStyle-Width="150" HeaderText="Delv. Doc. Dt." HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home">--%>
                                                    <asp:TemplateField ItemStyle-Width="150" HeaderText="<%$Resources:Resource,DelvDocDt%>"  HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltOBDDate_Pending" Text='<%# DataBinder.Eval(Container.DataItem, "OBDDate","{0: dd/MM/yy}") %>' />
                                                        </ItemTemplate>

                                                    </asp:TemplateField>

                                                    <%--<asp:TemplateField ItemStyle-Width="250" HeaderText="Tenant">--%>
                                                    <asp:TemplateField ItemStyle-Width="250" HeaderText="<%$Resources:Resource,Tenant%>">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltTenantName" Text='<%# DataBinder.Eval(Container.DataItem, "TenantName") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <%--<asp:TemplateField ItemStyle-Width="250" HeaderText="Customer" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home">--%>
                                                    <asp:TemplateField ItemStyle-Width="250" HeaderText="<%$Resources:Resource,Customer%>" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltCustomerName" Text='<%# DataBinder.Eval(Container.DataItem, "CustomerName") %>' />
                                                        </ItemTemplate>

                                                    </asp:TemplateField>

                                                    <%--<asp:TemplateField ItemStyle-Width="250" HeaderText="Requested By" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home">--%>
                                                    <asp:TemplateField ItemStyle-Width="250" HeaderText="<%$Resources:Resource,RequestedBy%>"  HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="home">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltRequestedBy" Text='<%# DataBinder.Eval(Container.DataItem, "RequestedBy").ToString() %>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:TemplateField HeaderStyle-Height="10px" ItemStyle-Width="250" HeaderText="Sent to Delivery" ItemStyle-CssClass="home">--%>
                                                    <asp:TemplateField HeaderStyle-Height="10px" ItemStyle-Width="250" HeaderText="<%$Resources:Resource,SenttoDelivery%>"  ItemStyle-CssClass="home">
                                                        <HeaderTemplate>
                                                            <table align="center" cellpadding="0" cellspacing="0" height="10px">
                                                                <tr>
                                                                    <td colspan="2" align="center">
                                                                        <asp:LinkButton ID="lnkSortByPGIDate" Font-Underline="false" runat="server" CommandName="Sort" CommandArgument="PGIDone_DT" Text="Sent to Delivery" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <%--<td width="80px" align="center">Date</td>--%>
                                                                    <td width="80px" align="center">Date </td>
                                                                    <td width="80px" align="center">Time</td>
                                                                </tr>
                                                            </table>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td width="80px" align="center">
                                                                        <asp:Literal runat="server" ID="ltGTSDate" Text='<%# DataBinder.Eval(Container.DataItem,"PGIDoneOn","{0:dd-MMM-yyyy}") %>' /></td>
                                                                    <td width="80px" align="center">
                                                                        <asp:Literal runat="server" ID="ltOGTSTime" Text='<%# DataBinder.Eval(Container.DataItem,"PackedOn","{0:hh:mm tt}") %>' /></td>
                                                                </tr>
                                                            </table>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <%--<asp:TemplateField ItemStyle-Width="200" HeaderText="Warehouse" ItemStyle-CssClass="home">--%>
                                                    <asp:TemplateField ItemStyle-Width="200" HeaderText="<%$Resources:Resource,Warehouse%>" ItemStyle-CssClass="home">
                                                        <ItemTemplate>

                                                            <asp:Literal runat="server" ID="ltStores" Text='<%# MRLWMSC21Common.CommonLogic.GetStoreNamesWithDeliveryStatus(DataBinder.Eval(Container.DataItem, "ReferedStores").ToString(),"<br/>",DataBinder.Eval(Container.DataItem, "OBDNumber").ToString(), DataBinder.Eval(Container.DataItem,"OutboundID").ToString() ) %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                   <%-- <asp:TemplateField ItemStyle-CssClass="home" HeaderText="P.Note" ItemStyle-Width="300">--%>
                                                     <asp:TemplateField ItemStyle-CssClass="home" HeaderText="<%$Resources:Resource,PNote%>" ItemStyle-Width="300">
                                                        <ItemTemplate>
                                                            <asp:HyperLink runat="server" ID="lnkHyperLink" ToolTip="Pick Note | Pick note with barcoded material codes to pick items for delivery." Text="Pick Note" Font-Underline="false" ForeColor="Red"></asp:HyperLink>

                                                            <asp:Literal runat="server" ID="ltLineCount" Text='<%# String.Format("[{0}]",DataBinder.Eval(Container.DataItem, "LineCount"))%>'></asp:Literal>
                                                            <img src='../Images/redarrowright.gif' border='0' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                     <asp:TemplateField ItemStyle-CssClass="home" HeaderText="Del. Pick Note" ItemStyle-Width="300">
                                                        <ItemTemplate>
                                                            <asp:HyperLink runat="server" ID="lnkHyperLinkDel" ToolTip="Delivery Pick Note" Text="Del. Pick Note" Font-Underline="false" ForeColor="Red"></asp:HyperLink>

                                                            <asp:Literal runat="server" ID="ltLineCountDel" Text='<%# String.Format("[{0}]",DataBinder.Eval(Container.DataItem, "LineCount"))%>'></asp:Literal>
                                                            <img src='../Images/redarrowright.gif' border='0' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                   <%-- <asp:TemplateField ItemStyle-CssClass="home" ItemStyle-Width="80" HeaderText="Change">--%>
                                                     <asp:TemplateField ItemStyle-CssClass="home" ItemStyle-Width="80" HeaderText="<%$Resources:Resource,Change%>" >
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkReceivedelivery" runat="server" Visible="false" CssClass="GvLink" PostBackUrl='<%# Eval("OutboundID", "OutboundDetails.aspx?obdid={0}&edittype=4") %>' Text="Change" ToolTip="Receive for PGI" />


                                                            <asp:HyperLink ID="HyperLink1" Text="<nobr> <i class='material-icons'>settings</i></nobr>" NavigateUrl='<%#   Eval("OutboundID", "OutboundDetails.aspx?obdid={0}&edittype=4")  %>' Font-Underline="false" runat="server"></asp:HyperLink>
                                                        </ItemTemplate>

                                                    </asp:TemplateField>
                                                </Columns>
                                                <EmptyDataTemplate>
													<%--<div align="center">No Data Found</div>--%>
                                                    	<div align="center"> <%= GetGlobalResourceObject("Resource", "NoDataFound")%></div>
												</EmptyDataTemplate>
                                            </asp:GridView>

                                        </td>
                                    </tr>

                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <!-- Deliveries Pending -->
                    </div>


                   <%-- <h3>POD Pending   <span id="lblPODRecCount" runat="server" /><sup><span id="lbltodayPODRecCount" class="PODblink" runat="server"></span></sup></h3>--%>
                     <h3> <%= GetGlobalResourceObject("Resource", "PODPending")%>   <span id="lblPODRecCount" runat="server" /><sup><span id="lbltodayPODRecCount" class="PODblink" runat="server"></span></sup></h3>

                    <div>
                        <!-- POD Pending -->
                        
                            <asp:UpdateProgress ID="uprgPODPending" runat="server" AssociatedUpdatePanelID="upnlPODPending">
                            <ProgressTemplate>
                              <div style="width:100%; height:100%; z-index:999; position:fixed; top:0; left:0; right:0; bottom:0; align-items:center; display:flex; justify-content:center; background: #e0ddd8ba;">
                                <%--<div class="spinner">
                                  <div class="bounce1"></div>
                                  <div class="bounce2"></div>
                                  <div class="bounce3"></div>
                                </div>--%>
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
                        <asp:UpdatePanel ID="upnlPODPending" runat="server" RenderMode="Inline" ViewStateMode="Enabled" ClientIDMode="Inherit" UpdateMode="Conditional">
                             <Triggers>
                                <asp:PostBackTrigger ControlID="imgbtngvDCRPending" />
                            </Triggers>
                            <ContentTemplate>
                                <table border="0" width="100%" cellpadding="5" cellspacing="5" align="center" >

                                    <tr>
                                        <td align="right">
                                            <asp:Panel runat="server" ID="pnlpodpending" DefaultButton="lnlPODSearch">
                                                <div style="float:right;">
                                                    <div class="flex__">
                                                         <div class="flex">
                                                                 &nbsp;&nbsp;&nbsp;
                                                         <input type="text" id="txtPPWarehouse" required="" />
                                                         <label>Warehouse</label>
                                                         
                                                              <asp:HiddenField ID="hifPPWarehouseID" runat="server" Value="0" />
                                                         </div>
                                                        <div class="flex">

                                                <asp:TextBox ID="txtPPTenant" runat="server" SkinID="txt_Hidden_Req_Auto" Text="Search Tenant..." ClientIDMode="Static" onfocus="ClearTextTenant(this)" onblur="javascript:focuslostTenant(this)"></asp:TextBox>
                                                <asp:HiddenField ID="hifPPTenantID" runat="server" Value="0" />
                                               
                                                            </div> 
                                                        <div style="margin-left:12px" class="flex">
                                                <asp:TextBox ID="txtPODDelDocNo" ClientIDMode="Static" runat="server" Text="Search Delv. Doc.#..." onfocus="ClearText(this)" SkinID="txt_Hidden_Req_Auto" onblur="javascript:focuslost1(this)"></asp:TextBox>
                                           
                                                            </div>
                                                        <div class="obd">
                                                            &nbsp;&nbsp;
                                                <%-- <asp:LinkButton runat="server" ID="lnlPODSearch" OnClick="lnlPODSearch_Click" CssClass="btn btn-primary"> Search <span class="space fa fa-search"></span></asp:LinkButton>--%>
                                                             <asp:LinkButton runat="server" ID="lnlPODSearch" OnClick="lnlPODSearch_Click" CssClass="btn btn-primary"> <%= GetGlobalResourceObject("Resource", "Search")%>  <span class="space fa fa-search"></span></asp:LinkButton>
                                                &nbsp;&nbsp;
                                        
                                                  <%--<asp:ImageButton ID="imgbtngvDCRPending" runat="server" ImageAlign="AbsMiddle" ImageUrl="../Images/excel_icon.jpg" Width="20" OnClick="imgbtngvDCRPending_Click" ToolTip="Export To Excel" />--%>
                                                          <%--  <asp:LinkButton ID="imgbtngvDCRPending" CssClass="btn btn-primary"  runat="server" OnClick="imgbtngvDCRPending_Click">Export Excel <%=MRLWMSC21Common.CommonLogic.btnfaExcel %></asp:LinkButton>--%>
                                                              <asp:LinkButton ID="imgbtngvDCRPending" CssClass="btn btn-primary"  runat="server" OnClick="imgbtngvDCRPending_Click"><%= GetGlobalResourceObject("Resource", "ExportExcel")%> <%=MRLWMSC21Common.CommonLogic.btnfaExcel %></asp:LinkButton>
                                                            </div>
                                                        </div>
                                                    </div>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                   <%-- <tr>
                                        <td class="FormLabels">
                                            <br />
                                            <asp:Label ID="lblPODStatus" runat="server"></asp:Label>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td>
                                            <asp:GridView Width="100%" ShowHeader="true" ShowHeaderWhenEmpty="true" ShowFooter="true" GridLines="None" CellPadding="1" CellSpacing="1" ID="gvDCRPending" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="true" PageSize="25" AllowSorting="True" SkinID="gvLightBlueNew" HorizontalAlign="Left" OnSorting="gvDCRPending_Sorting" OnPageIndexChanging="gvDCRPending_PageIndexChanging" OnRowDataBound="gvDCRPending_OnRowDataBound">

                                                <Columns>


                                                    <%--<asp:TemplateField ItemStyle-Width="150" HeaderText="Delv. Doc. No." ItemStyle-CssClass="gvOBDNumber">--%>
                                                    <asp:TemplateField ItemStyle-Width="150" HeaderText="<%$Resources:Resource,DelvDocNo%>" ItemStyle-CssClass="gvOBDNumber" Visible="false">
                                                        <ItemTemplate>

                                                            <asp:Literal runat="server" ID="ltOBDNumber_Pending" Text='<%# DataBinder.Eval(Container.DataItem, "OBDNumber") %>' />
                                                            <asp:Label runat="server" ID="lblOBDID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "OutboundID") %>'></asp:Label>
                                                            <asp:Label runat="server" ID="lblLineItemCount" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "LineCount") %>'></asp:Label>


                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                   <%-- <asp:TemplateField ItemStyle-Width="150" HeaderText="Delv. Doc. Type" ItemStyle-CssClass="home">--%>
                                                     <asp:TemplateField ItemStyle-Width="150" HeaderText="<%$Resources:Resource,DelvDocType%>"  ItemStyle-CssClass="home">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltDelvType" Text='<%# DataBinder.Eval(Container.DataItem, "DocumentType").ToString() %>' />
                                                            <asp:Literal runat="server" ID="ltDocTypeID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "DocumentTypeID") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <%--<asp:TemplateField ItemStyle-Width="150" HeaderText="Delv. Doc. Dt." ItemStyle-CssClass="home">--%>
                                                    <asp:TemplateField ItemStyle-Width="150" HeaderText="<%$Resources:Resource,DelvDocDt%>"  ItemStyle-CssClass="home">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltOBDDate_Pending" Text='<%# DataBinder.Eval(Container.DataItem, "OBDDate","{0: dd/MM/yy}") %>' />
                                                        </ItemTemplate>

                                                    </asp:TemplateField>


                                                   <%-- <asp:TemplateField ItemStyle-Width="250" HeaderText="Tenant">--%>
                                                     <asp:TemplateField ItemStyle-Width="250" HeaderText="<%$Resources:Resource,Tenant%>" >
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltTenantName" Text='<%# DataBinder.Eval(Container.DataItem, "TenantName") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                   <%-- <asp:TemplateField ItemStyle-Width="250" HeaderText="Customer" ItemStyle-CssClass="home">--%>
                                                     <asp:TemplateField ItemStyle-Width="250" HeaderText="<%$Resources:Resource,Customer%>"  ItemStyle-CssClass="home">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltCustomerName" Text='<%# DataBinder.Eval(Container.DataItem, "CustomerName") %>' />
                                                        </ItemTemplate>

                                                    </asp:TemplateField>

                                                   <%-- <asp:TemplateField ItemStyle-Width="200" HeaderText="Requested By" ItemStyle-CssClass="home">--%>
                                                     <asp:TemplateField ItemStyle-Width="200" HeaderText="<%$Resources:Resource,RequestedBy%>"  ItemStyle-CssClass="home">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltRequestedBy" Text='<%# DataBinder.Eval(Container.DataItem, "RequestedBy").ToString() %>'></asp:Literal>
                                                        </ItemTemplate>

                                                    </asp:TemplateField>



                                                    <asp:TemplateField HeaderStyle-Height="10px" ItemStyle-Width="200" ItemStyle-CssClass="home" HeaderText="Sent to Delivery">
                                                        <HeaderTemplate>
                                                            <table align="center" cellpadding="0" cellspacing="0" height="10px">
                                                                <tr>
                                                                    <%--<td colspan="2" align="center">Delivery </td>--%>
                                                                    <td colspan="2" align="center"> <%= GetGlobalResourceObject("Resource", "Delivery")%> </td>
                                                                </tr>
                                                                <tr>
                                                                    <%--<td width="80px" align="center">Date</td>--%>
                                                                    <td width="80px" align="center"><%= GetGlobalResourceObject("Resource", "Date")%> </td>
                                                                   <%-- <td width="80px" align="center">Time</td>--%>
                                                                     <td width="80px" align="center"> <%= GetGlobalResourceObject("Resource", "Time")%> </td>
                                                                </tr>
                                                            </table>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td width="80px" align="center">
                                                                        <asp:Literal runat="server" ID="ltGTSDate" Text='<%# DataBinder.Eval(Container.DataItem,"PGIDoneOn","{0:dd-MMM-yyyy}") %>' /></td>
                                                                    <td width="80px" align="center">
                                                                        <asp:Literal runat="server" ID="ltOGTSTime" Text='<%# DataBinder.Eval(Container.DataItem,"DeliveryDate","{0:hh:mm tt}") %>' /></td>
                                                                </tr>
                                                            </table>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <%--<asp:TemplateField ItemStyle-Width="200" HeaderText="Warehouse" ItemStyle-CssClass="home">--%>
                                                    <asp:TemplateField ItemStyle-Width="200" HeaderText="<%$Resources:Resource,Warehouse%>" ItemStyle-CssClass="home">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltStores" Text='<%# MRLWMSC21Common.CommonLogic.GetStoreNames(DataBinder.Eval(Container.DataItem, "ReferedStores").ToString(),"<br/>" ) %>' />

                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                   <%-- <asp:TemplateField ItemStyle-Width="200" HeaderText="POD Not Rcvd. / Delv. Dt. /Driver" ItemStyle-CssClass="home">--%>
                                                     <asp:TemplateField ItemStyle-Width="200" HeaderText="<%$Resources:Resource,PODNotRcvdDelvDtDriver%>" ItemStyle-CssClass="home">
                                                        <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltDeliveryStatus" Text='<%# MRLWMSC21Common.CommonLogic.DCRUpdate(DataBinder.Eval(Container.DataItem, "DocumentTypeID").ToString(),DataBinder.Eval(Container.DataItem, "DeliveryStatusID").ToString(), DataBinder.Eval(Container.DataItem, "IsPODReceived").ToString(),DataBinder.Eval(Container.DataItem, "DeliveryDate","{0: dd-MMM-yyyy, hh:mm tt}").ToString(),DataBinder.Eval(Container.DataItem, "ReferedStores").ToString(),DataBinder.Eval(Container.DataItem, "OBDNumber").ToString(),DataBinder.Eval(Container.DataItem, "OutboundID").ToString(),DataBinder.Eval(Container.DataItem, "DriverName").ToString()) %>'></asp:Literal>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <%--<asp:TemplateField ItemStyle-CssClass="home" HeaderText="P.Note" ItemStyle-Width="300">--%>
                                                    <asp:TemplateField ItemStyle-CssClass="home" HeaderText="<%$Resources:Resource,PNote%>" ItemStyle-Width="300">
                                                        <ItemTemplate>
                                                            <asp:HyperLink runat="server" ID="lnkHyperLink" ToolTip="Pick Note | Pick note with barcoded material codes to pick items for delivery." Text="Pick Note" Font-Underline="false" ForeColor="Red"></asp:HyperLink>

                                                            <asp:Literal runat="server" ID="ltLineCount" Text='<%# String.Format("[{0}]",DataBinder.Eval(Container.DataItem, "LineCount"))%>'></asp:Literal>
                                                            <img src='../Images/redarrowright.gif' border='0' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                     <asp:TemplateField ItemStyle-CssClass="home" HeaderText="Del. Pick Note" ItemStyle-Width="300">
                                                        <ItemTemplate>
                                                            <asp:HyperLink runat="server" ID="lnkHyperLinkDel" ToolTip="Delivery Pick Note" Text="Del. Pick Note" Font-Underline="false" ForeColor="Red"></asp:HyperLink>

                                                            <asp:Literal runat="server" ID="ltLineCountDel" Text='<%# String.Format("[{0}]",DataBinder.Eval(Container.DataItem, "LineCount"))%>'></asp:Literal>
                                                            <img src='../Images/redarrowright.gif' border='0' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <%--<asp:TemplateField ItemStyle-CssClass="home" ItemStyle-Width="80" HeaderText="Change">--%>
                                                    <asp:TemplateField ItemStyle-CssClass="home" ItemStyle-Width="80" HeaderText="<%$Resources:Resource,Change%>" >
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkReceivedelivery" runat="server" CssClass="GvLink" Visible="false" PostBackUrl='<%# Eval("OutboundID", "OutboundDetails.aspx?obdid={0}&edittype=4") %>' Text="Change" ToolTip="Receive for PGI" />

                                                            <asp:HyperLink ID="HyperLink1" Text="<nobr> <i class='material-icons'>settings</i></nobr>" NavigateUrl='<%#   Eval("OutboundID", "OutboundDetails.aspx?obdid={0}&edittype=4")  %>' Font-Underline="false" runat="server"></asp:HyperLink>

                                                        </ItemTemplate>

                                                    </asp:TemplateField>
                                                </Columns>
                                                <EmptyDataTemplate>
													<%--<div align="center">No Data Found</div>--%>
                                                    <div align="center"> <%= GetGlobalResourceObject("Resource", "NoDataFound")%></div>
												</EmptyDataTemplate>
                                            </asp:GridView>

                                        </td>
                                    </tr>

                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <!-- POD Pending -->
                    </div>






                </div>



            </td>
        </tr>
    </table>

</div>



    <asp:HiddenField ID="hidAccordionIndex" runat="server" Value="0" />

</asp:Content>
