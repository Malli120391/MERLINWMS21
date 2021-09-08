<%@ Page Title=" Inbound Tracking :." Language="C#" MasterPageFile="~/mInbound/InboundMaster.master" AutoEventWireup="true" CodeBehind="InboundTracking.aspx.cs" Inherits="MRLWMSC21.mInbound.InboundTracking" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="IBContent" runat="server">
    <asp:ScriptManager runat="server" EnablePartialRendering="true" ID="inboundtracking" SupportsPartialRendering="true"></asp:ScriptManager>

       <script type="text/javascript" src="../Scripts/jQuery2/countdown/jquery.countdown.js"></script>

     <script type="text/javascript" src="Scripts/jquery-1.8.2.js"></script>
    <script type="text/javascript" src="Scripts/jquery-ui-1.8.24.js"></script>
    <script type="text/javascript" src="Scripts/modernizr-2.6.2.js"></script>


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
             $("#accordion").accordion({
                 header: 'h3', collapsible: true, autoHeight: false, clearStyle: true, navigation: true
             });
            

         });
    </script>



    <style type="text/css">

        .ui-autocomplete-loading {
            background: white url('../Images/ui-anim_basic_16x16.gif') right center no-repeat;
        }

        .gvLightBlueNew_pager span{
            color:#fff !important;
        }

        .gvLightGrayNew_footerGrid {
            display: none;
        }

        .gvLightGreenNew_pager table tr {
            display: flex;
                justify-content: flex-end;
        }
        .home {
            white-space:nowrap;
        }
    </style>
    <script type="text/javascript">
        function OpenImage(path) {
            window.open(path, 'Naresh', 'height=800,width=900');
        }



        function ClearText(TextBox) {
            if (TextBox.value == "Search Store Ref.# ...")
                TextBox.value = "";

            TextBox.style.color = "#000000";
        }

        function ClearTextTenant(TextBox) {
            if (TextBox.value == "Search Tenant...")
                TextBox.value = "";

            TextBox.style.color = "#000000";
        }

        function focuslost1(TextBox) {
            if (TextBox.value == "")
                TextBox.value = "Search Store Ref.# ...";

            TextBox.style.color = "#A4A4A4";
        }

        function focuslostTenant(TextBox) {
            if (TextBox.value == "")
                TextBox.value = "Search Tenant...";

            TextBox.style.color = "#A4A4A4";
        }


        function CollapseAll() {
            //$("#collapseAll").click(function () {
                $(".ui-accordion-content").hide()
            //});
        }
        function ExpandAll() {
            //$("#expandAll").click(function () {
            
                $(".ui-accordion-content").show()
            //});
        }
    </script>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                fnMCodeAC();
            }
        }

         function validate() {
            if ($('#<%=txtSITTenant.ClientID%>').val() != $("#hiSITTenantName").val()) {
                showStickyToast(false, 'Select Valid Tenant');
                return false;
            }
            return true;
        }
        function validate1() {
            debugger;
            if ($('#<%=txtSIETenant.ClientID%>').val() != $("#hiSIETenantName").val()) {
                showStickyToast(false, 'Select Valid Tenant');
                return false;
            }
            return true;
        }
        function validate2() {
            debugger;
            if ($('#<%=txtSIPTenant.ClientID%>').val() != $("#hiSIPTenantName").val()) {
                showStickyToast(false, 'Select Valid Tenant');
                return false;
            }
            return true;
         }
        function fnMCodeAC() {
            $(document).ready(function () {

                var textfieldname = $('#<%=txtSITStoreRefNo.ClientID%>');
                DropdownFunction(textfieldname);
                $('#<%=txtSITStoreRefNo.ClientID%>').autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadSITStoreRefNumbers") %>',
                            data: "{ 'prefix': '" + request.term + "','TenantID':'" + document.getElementById('<%= this.hifSITTenantID.ClientID %>').value + "'}",
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                if (data.d == "" || data.d == "/") {
                                    showStickyToast(false, 'No store ref.# is available for \'Shipments In Transit\'');
                                }
                                else {
                                    response($.map(data.d, function (item) {
                                        return {
                                            label: item,
                                        }
                                    }
                                    ))
                                }
                            }

                        });
                    },
                    minLength: 0
                });

                var textfieldname = $('#<%=txtSIEStoreRefNo.ClientID%>');
                DropdownFunction(textfieldname);
                $('#<%=txtSIEStoreRefNo.ClientID%>').autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadSIEStoreRefNumbers_TC") %>',
                            data: "{ 'prefix': '" + request.term + "','TenantID':'" + document.getElementById('<%= this.hifSIETenant.ClientID %>').value + "','WHID':'"+$("#hdnWarehouse").val()+"'}",
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                if (data.d == "" || data.d == "/") {
                                    showStickyToast(false, 'No \'Shipment\' is available for \'Shipment Expected\'');
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

                var textfieldname = $('#<%=txtSIPStoreRefNo.ClientID%>');
                DropdownFunction(textfieldname);
                $('#<%=txtSIPStoreRefNo.ClientID%>').autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadSIPStoreRefNumbers") %>',
                            data: "{ 'prefix': '" + request.term + "','TenantID':'" + document.getElementById('<%= this.hifSIPTenant.ClientID %>').value + "'}",
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                if (data.d == "" || data.d == "/") {
                                    showStickyToast(false, 'No store ref.#  is available for \'Shipments In Process\' ');
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
                var textfieldname = $('#<%=txtGRNStoreRefNumber.ClientID%>');
                DropdownFunction(textfieldname);
                $('#<%=txtGRNStoreRefNumber.ClientID%>').autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadGRNStoreRefNumbers") %>',
                            data: "{ 'prefix': '" + request.term + "','TenantID':'" + document.getElementById('<%= this.hifGRNTenant.ClientID %>').value + "'}",
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                if (data.d == "" || data.d == "/") {
                                    showStickyToast(false, 'No store ref.# is available for \'GRN Pending\' ');
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

                var textfieldname = $('#<%=txtDiscStoreRefNumber.ClientID%>');
                DropdownFunction(textfieldname);
                $('#<%=txtDiscStoreRefNumber.ClientID%>').autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadDISStoreRefNumbers") %>',
                            data: "{ 'prefix': '" + request.term + "','TenantID':'" + document.getElementById('<%= this.hifDiscTenant.ClientID %>').value + "'}",
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                if (data.d == "" || data.d == "/") {
                                    showStickyToast(false, 'No store ref.# is available for \'Discrepancy Shipments\' ');
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

                var textfieldname = $('#<%=txtSITTenant.ClientID%>');
                DropdownFunction(textfieldname);
                $('#<%=txtSITTenant.ClientID%>').autocomplete({
                    source: function (request, response) {
                        if ($('#<%=txtSITTenant.ClientID%>').val() == '') {
                            $("#<%=hifSITTenantID.ClientID %>").val(0);
                                $("#hiSITTenantName").val('');
                        }
                        $.ajax({
                           <%-- url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadTenantDataFor3PL") %>',
                            data: "{ 'prefix': '" + request.term + "'}",--%>
                               url: '../mWebServices/FalconWebService.asmx/GetWarehouseTenant',
                            data: "{ 'prefix': '" + request.term + "','WHID':'" + $("#<%=hdnSITWarehouse.ClientID %>").val() +"'}",
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
                        $("#<%=hifSITTenantID.ClientID %>").val(i.item.val);
                        $("#hiSITTenantName").val(i.item.label);
                    },
                    minLength: 0
                });
                    var textfieldname = $('#txtSITWarehouse');
                  DropdownFunction(textfieldname);
                  $('#txtSITWarehouse').autocomplete({
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
                          $("#<%=hdnSITWarehouse.ClientID %>").val(i.item.val);
                      },
                      minLength: 0
                  });

              
                var textfieldname = $('#<%=txtSIETenant.ClientID%>');
                DropdownFunction(textfieldname);
                $('#<%=txtSIETenant.ClientID%>').autocomplete({
                    source: function (request, response) {
                         if ($('#<%=txtSIETenant.ClientID%>').val() == '') {
                            $("#<%=hifSIETenant.ClientID %>").val(0);
                             $("#hiSIETenantName").val('');

                        }
                        $.ajax({
                           <%-- url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadTenantDataFor3PL") %>',
                            data: "{ 'prefix': '" + request.term + "'}",--%>
                            url: '../mWebServices/FalconWebService.asmx/GetWarehouseTenant',
                            data: "{ 'prefix': '" + request.term + "','WHID':'" +  $("#<%=hdnWarehouse.ClientID %>").val() + "'}",
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
                        $("#<%=hifSIETenant.ClientID %>").val(i.item.val);
                        $("#hiSIETenantName").val(i.item.label);

                        //$("#txtWarehouse").val("");
                        //$("#hdnWarehouse").val("0");
                    },
                    minLength: 0
                });

                //Shipment in process WH added by Meena
                   var textfieldname = $('#txtSIPWarehouse');
                  DropdownFunction(textfieldname);
                  $('#txtSIPWarehouse').autocomplete({
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
                          $("#<%=hdnSIPWarehouse.ClientID %>").val(i.item.val);
                      },
                      minLength: 0
                  });

                var textfieldname = $('#<%=txtSIPTenant.ClientID%>');
                DropdownFunction(textfieldname);
                $('#<%=txtSIPTenant.ClientID%>').autocomplete({
                    source: function (request, response) {
                         if ($('#<%=txtSIPTenant.ClientID%>').val() == '') {
                            $("#<%=hifSIPTenant.ClientID %>").val(0);
                                $("#hiSIPTenantName").val('');
                         }
                        $.ajax({
                          <%--  url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadTenantDataFor3PL") %>',
                            data: "{ 'prefix': '" + request.term + "'}",--%>
                            url: '../mWebServices/FalconWebService.asmx/GetWarehouseTenant',
                            data: "{ 'prefix': '" + request.term + "','WHID':'" +  $("#<%=hdnSIPWarehouse.ClientID %>").val() + "'}",
                         
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
                        $("#<%=hifSIPTenant.ClientID %>").val(i.item.val);
                         $("#hiSIPTenantName").val(i.item.label);
                    },
                    minLength: 0
                });

                var textfieldname = $('#<%=txtGRNTenant.ClientID%>');
                DropdownFunction(textfieldname);
                $('#<%=txtGRNTenant.ClientID%>').autocomplete({
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
                            },
                            error: function (response) {

                            },
                            failure: function (response) {

                            }
                        });
                    },
                    select: function (e, i) {
                        $("#<%=hifGRNTenant.ClientID %>").val(i.item.val);
                    },
                    minLength: 0
                });

                var textfieldname = $('#<%=txtDiscTenant.ClientID%>');
                DropdownFunction(textfieldname);
                $('#<%=txtDiscTenant.ClientID%>').autocomplete({
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
                            },
                            error: function (response) {

                            },
                            failure: function (response) {

                            }
                        });
                    },
                    select: function (e, i) {
                        $("#<%=hifDiscTenant.ClientID %>").val(i.item.val);
                    },
                    minLength: 0
                });

                 //$('#txtWarehouse').val("");
                  var textfieldname = $('#txtWarehouse');
                  DropdownFunction(textfieldname);
                  $('#txtWarehouse').autocomplete({
                      source: function (request, response) {
                          $.ajax({
                               url: '../mWebServices/FalconWebService.asmx/LoaDWHListBasedonUser',
                              data: "{ 'prefix': '" + request.term + "'}", 
                             <%-- url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadWHForWHList_CurrentStock") %>',
                              data: "{ 'prefix': '" + request.term + "','TenantID':'" + document.getElementById('<%= this.hifSIETenant.ClientID %>').value + "'}",--%>
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
         jQuery(document).ready(function () {
             jQuery('.SITblink').blink();
             jQuery('.SHEblink').blink();
             jQuery('.SIPblink').blink();
             jQuery('.GRNblink').blink();
             jQuery('.DISblink').blink();

           });

         
     </script>


    <style type="text/css">
        
        .SITblink, .SHEblink, .SIPblink, .GRNblink, .DISblink {
            color: white;
            font-size: 10pt;
            font-weight: bold;
            font-family: Calibri;
            width: 100px;
            background-color: red;
            border-radius: 5px;
            -ms-border-radius: 5px;
        }

        .mdd-select-underline {
            bottom: 0px !important;
        }

    </style>

    
<div class="container">
    <table border="0" cellspacing="2" cellpadding="5" align="center" class="">

   <%--     <tr>
            <td align="right">
                <br />
                <a id="collapseAll" onclick="CollapseAll()" href="#" class=" ui-button-small">Collapse All <span class="space fa fa-compress"></span></a> &nbsp; &nbsp; &nbsp;
                <a id="expandAll" onclick="ExpandAll()" href="#" class="ui-button-small">Expand All <span class="space fa fa-expand" ></span></a>
            </td>
        </tr>--%>
        <tr>
            <td align="center">

                <div id="accordion" align="left" class="accordion">
                    <h3>  <%= GetGlobalResourceObject("Resource", "ShipmentsInTransit")%>  <span id="lblSITRecordCount" runat="server"></span> <sup><span id="lbltodaySITRecCount" class="SITblink" runat="server"></span></sup></h3>
                    <div>
                        <!-- Shipments IN - Transit -->
                        <asp:UpdateProgress ID="uprgSIT" runat="server" AssociatedUpdatePanelID="upnlInTransit">
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
                        <asp:UpdatePanel ID="upnlInTransit" runat="server" RenderMode="Inline" ViewStateMode="Enabled" ClientIDMode="Inherit" UpdateMode="Conditional">
                             <Triggers>
                                <asp:PostBackTrigger ControlID="imgbtngvShipmentPending" />
                            </Triggers>
                            <ContentTemplate>
                            
                         <!-- Globalization Tag is added for multilingual  -->    
                        <table border="0" width="100%" cellpadding="5" cellspacing="5" align="center">

                            <tr class="absolute">
                                <td align="right" class="FormLabels">

                                    <asp:Panel ID="pnlspntintansit" runat="server" DefaultButton="lnkSITSearch">
                                        <div class="flex__end ">
                                               <div class="flex">
                                                            <asp:TextBox ID="txtSITWarehouse" runat="server" ClientIDMode="Static" SkinID="txt_Hidden_Req_Auto" required=""></asp:TextBox> 
                                                             <asp:HiddenField ID="hdnSITWarehouse" runat="server" Value="0" ClientIDMode="Static" />
                                                            <label>Warehouse</label>                                                            
                                                        
                                                        </div>  &nbsp;&nbsp;
                                            <div class="flex"><asp:TextBox ID="txtSITTenant" required=""  runat="server" SkinID="txt_Hidden_Req_Auto" ></asp:TextBox>
                                            <asp:HiddenField  ID="hifSITTenantID" runat="server" Value="0"/>
                                                <label>  <%= GetGlobalResourceObject("Resource", "SearchTenant")%></label>
                                                <input type="hidden" id="hiSITTenantName" />
                                            </div>&nbsp;&nbsp;
                                            <div class="flex" width="170">
                                             <asp:TextBox ID="txtSITStoreRefNo" runat="server" SkinID="txt_Hidden_Req_Auto" required="" ></asp:TextBox>
                                                <%--<label>Search Store Ref.#  </label>--%>
                                                <label><%= GetGlobalResourceObject("Resource", "SearchStoreRef")%>  </label>
                                            </div>&nbsp;&nbsp;
                                            <div>
                                            <%--<asp:LinkButton runat="server"  ID="lnkSITSearch" OnClick="lnkSITSearch_Click" CssClass="btn btn-sm btn-primary" > Search <span class="space fa fa-search"></span></asp:LinkButton>--%>
                                                <asp:LinkButton runat="server"  ID="lnkSITSearch"  OnClick="lnkSITSearch_Click" CssClass="btn btn-sm btn-primary" > <%= GetGlobalResourceObject("Resource", "Search")%> <span class="space fa fa-search"></span></asp:LinkButton>
                                            
                                            <%--<asp:ImageButton ID="imgbtngvShipmentPending" runat="server" ImageAlign="AbsMiddle" ImageUrl="../Images/excel_icon.jpg" Width="20" OnClick="imgbtngvShipmentPending_Click1" ToolTip="Export To Excel" />--%>
                                                    <%--<asp:LinkButton ID="imgbtngvShipmentPending" CssClass="btn btn-primary" runat="server" OnClick="imgbtngvShipmentPending_Click1">Export Excel <%=MRLWMSC21Common.CommonLogic.btnfaExcel %></asp:LinkButton>--%>
                                                <asp:LinkButton ID="imgbtngvShipmentPending" CssClass="btn btn-primary" runat="server" OnClick="imgbtngvShipmentPending_Click1"><%= GetGlobalResourceObject("Resource", "ExportExcel")%> <%=MRLWMSC21Common.CommonLogic.btnfaExcel %></asp:LinkButton>

                                            </div>
                                            </div>
                                    </asp:Panel>
                                   
                                </td>
                            </tr>
                           <%-- <tr>
                                <td class="FormLabels">
                                    <asp:Label ID="lblSITStatus" runat="server"></asp:Label>
                                </td>
                            </tr>--%>
                            <tr>
                                <td>
                                    <asp:GridView Width="100%" EnableViewState="false" ShowHeader="true" ShowHeaderWhenEmpty="true" ShowFooter="false" GridLines="None" ID="gvShipmentPending" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="true" PageSize="25" AllowSorting="True" SkinID="gvLightBlueNew" HorizontalAlign="Left" OnSorting="gvShipmentPending_Sorting" OnPageIndexChanging="gvShipmentPending_PageIndexChanging" OnRowDataBound="gvShipmentPending_RowDataBound">
                                        <Columns>
                                            
                                          <%--  <asp:TemplateField ItemStyle-Width="150" HeaderText="Store Ref. #" ItemStyle-CssClass="gvOBDNumber">--%>
                                              <asp:TemplateField ItemStyle-Width="150" HeaderText= "<%$Resources:Resource,StoreRef%>" ItemStyle-CssClass="gvOBDNumber">
                                                <ItemTemplate>

                                                    <asp:Literal runat="server" ID="ltStoreRefNo" Text='<%# GetStoreRefNoWithLink(DataBinder.Eval(Container.DataItem, "StoreRefNo").ToString(),DataBinder.Eval(Container.DataItem, "TenantID").ToString()) %>' />

                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField ItemStyle-Width="150" HeaderText="<%$Resources:Resource,ShipmentType%>"  ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltShipType" Text='<%# DataBinder.Eval(Container.DataItem, "ShipmentType").ToString() %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                          <%--  <asp:TemplateField ItemStyle-Width="150" HeaderText="Doc. Rcvd. Dt."  ItemStyle-CssClass="home">--%>
                                              <asp:TemplateField ItemStyle-Width="150" HeaderText="<%$Resources:Resource,DocRcvdDt%>"   ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltDocRdvDate" Text='<%# DataBinder.Eval(Container.DataItem, "DocReceivedDate","{0: dd-MMM-yyyy}") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                           <%-- <asp:TemplateField ItemStyle-Width="250" HeaderText="Tenant" ItemStyle-CssClass="home">--%>
                                             <asp:TemplateField ItemStyle-Width="250" HeaderText="<%$Resources:Resource,Tenant%>"  ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltSITTenant" Text='<%# DataBinder.Eval(Container.DataItem, "CompanyName") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                           <%-- <asp:TemplateField ItemStyle-Width="250" HeaderText="Supplier" ItemStyle-CssClass="home">--%>
                                             <asp:TemplateField ItemStyle-Width="250" HeaderText="<%$Resources:Resource,Supplier%>"  ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltSupplier" Text='<%# DataBinder.Eval(Container.DataItem, "SupplierName") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                            <asp:TemplateField ItemStyle-Width="250" HeaderText="Clearance Agent" ItemStyle-CssClass="home" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltCompanyName" Text='<%# DataBinder.Eval(Container.DataItem, "ClearanceCompany").ToString() %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                           <%-- <asp:TemplateField ItemStyle-Width="200" HeaderText="Warehouse" ItemStyle-CssClass="home">--%>
                                             <asp:TemplateField ItemStyle-Width="200" HeaderText="<%$Resources:Resource,Warehouse%>" ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltSoreAssociated" Text='<%# MRLWMSC21Common.CommonLogic.GetStoreNames(DataBinder.Eval(Container.DataItem, "ReferedStores").ToString(),"<br/>" ) %>' />
                                                    <asp:Literal runat="server" ID="ltShipmentLocation" Text='<%# DataBinder.Eval(Container.DataItem, "ShipmentLocation") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="RTR" HeaderStyle-Width="200" Visible="false" ItemStyle-Width="150">
                                                <ItemTemplate>
                                                    <a id="A1" class="helpWTitle" Title="Receiving Tally Report(RTR) | Receiving Tally Report with barcoded material codes to receive items for putaway." runat="server" visible="true" href='<%# String.Format("RTReport.aspx?ibdid={0}&lineitemcount={1}&TN={2}", DataBinder.Eval(Container.DataItem, "InboundID").ToString(),DataBinder.Eval(Container.DataItem, "LineCount").ToString(),DataBinder.Eval(Container.DataItem, "CompanyName").ToString()) %>'>RTR
                                                        <asp:Literal runat="server" ID="ltLineCount" Text='<%# String.Format("[{0}]",DataBinder.Eval(Container.DataItem, "LineCount"))%>'></asp:Literal>
                                                    </a>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                          <%--  <asp:TemplateField ItemStyle-CssClass="home" HeaderText="Change">--%>
                                              <asp:TemplateField ItemStyle-CssClass="home" HeaderText="<%$Resources:Resource,Change%>">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEditInbound" runat="server" CssClass="GvLink" Visible="false" PostBackUrl='<%# Eval("InboundID", "InboundDetails.aspx?ibdid={0}&edittype=2") %>' Text="<nobr> Change <img src='../Images/redarrowright.gif' border='0' /></nobr>"  />

                                                    <asp:HyperLink ID="HyperLink1" Text="<nobr> <i class='material-icons vl'>settings</i></nobr>" NavigateUrl='<%#  Eval("InboundID", "InboundDetails.aspx?ibdid={0}&edittype=2")  %>' Font-Underline="false" runat="server" ></asp:HyperLink>

                                                </ItemTemplate>

                                            </asp:TemplateField>


                                        </Columns>
                                        <EmptyDataTemplate>
	                                        <%--<div align="center">No Data Found</div>--%>
                                            <div align="center"> <%= GetGlobalResourceObject("Resource", "NoDataFound")%>
</div>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                                </ContentTemplate>
                          <%--  <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lnkSITSearch" EventName="lnkSITSearch_Click" />
                                <asp:AsyncPostBackTrigger ControlID="gvShipmentPending" EventName="gvShipmentPending_PageIndexChanging" />
                            </Triggers>--%>
                        </asp:UpdatePanel>
                        <!-- Shipments IN - Transit -->
                        
                    <br />
                    </div>



                    <%--<h3>Shipment Expected    <span id="lblSIERecordCount" runat="server" /><sup><span id="lbltodaySHERecCount" class="SHEblink" runat="server"></span></sup></h3>--%>
                    <h3> <%= GetGlobalResourceObject("Resource", "ShipmentExpected")%>  <span id="lblSIERecordCount" runat="server" /><sup><span id="lbltodaySHERecCount" class="SHEblink" runat="server"></span></sup></h3>
                    <div>
                        <!-- Shipments Expected -->
                           <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upnlInTransit">
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
                        <asp:UpdatePanel ID="upnlShipmentExpected" runat="server" RenderMode="Inline" ViewStateMode="Enabled" ClientIDMode="Inherit" UpdateMode="Conditional">
                             <Triggers>
                                <asp:PostBackTrigger ControlID="imgbtngvShipmentExpected" />
                            </Triggers>
                            
                            <ContentTemplate>
                        <table border="0" width="100%" cellpadding="5" cellspacing="5" align="center">

                            <tr class="">
                                <td align="right" class="FormLabels absolute">
                                   
                                    <asp:Panel runat="server" ID="pnlspntexp" DefaultButton="lnkSIESearch">
                                        <div class="flex__end">
                                              <div class="flex">
                                                 <asp:TextBox ID="txtWarehouse" runat="server" ClientIDMode="Static" SkinID="txt_Hidden_Req_Auto" required=""></asp:TextBox>
                                                 <asp:HiddenField ID="hdnWarehouse" runat="server" Value="0" ClientIDMode="Static" />
                                                 <label>Warehouse</label>
                                                
                                             </div>

                                             &nbsp;&nbsp;
                                        <div class="flex"><asp:TextBox ID="txtSIETenant" runat="server" SkinID="txt_Hidden_Req_Auto" required=""></asp:TextBox>
                                        <asp:HiddenField  ID="hifSIETenant" runat="server" Value="0"/>
                                     <%--   <label>Search Tenant</label>--%>
                                               <label> <%= GetGlobalResourceObject("Resource", "SearchTenant")%></label>
                                             <input type="hidden" id="hiSIETenantName" />
                                        </div>
                                        &nbsp;&nbsp;

                                           
                                        <div class="flex" width170>
                                        <asp:TextBox ID="txtSIEStoreRefNo" runat="server" SkinID="txt_Hidden_Req_Auto" required=""></asp:TextBox>
                                            <%--<label>Search Store Ref.#</label>--%>
                                            <label><%= GetGlobalResourceObject("Resource", "SearchStoreRef")%> </label>
                                        </div>&nbsp;&nbsp;
                                        <div>
                                            <%--<asp:LinkButton runat="server"  ID="lnkSIESearch" OnClientClick="return validate1();" OnClick="lnkSIESearch_Click" CssClass="btn btn-sm btn-primary" >Search<span class="space fa fa-search"></span></asp:LinkButton>--%>
                                            <asp:LinkButton runat="server"  ID="lnkSIESearch"  OnClick="lnkSIESearch_Click" CssClass="btn btn-sm btn-primary" >Search<span class="space fa fa-search"></span></asp:LinkButton>
                                       
                                        
                                         <%--<asp:ImageButton ID="imgbtngvShipmentExpected" runat="server" ImageAlign="AbsMiddle" ImageUrl="../Images/excel_icon.jpg" Width="20" OnClick="btngvShipmentExpected_Click" ToolTip="Export To Excel" />--%>
                                            <asp:LinkButton ID="imgbtngvShipmentExpected" CssClass="btn btn-primary" runat="server" OnClick="btngvShipmentExpected_Click">Export Excel <%=MRLWMSC21Common.CommonLogic.btnfaExcel %></asp:LinkButton>
                                         </div></div>
                                      </asp:Panel>
                                </td>
                            </tr>

                            <%--<tr>
                                <td class="FormLabels">
                                    <asp:Label ID="ltShipmentExpectedStatus" runat="server"></asp:Label>
                                </td>
                            </tr>--%>

                            <tr>
                                <td>

                                    <asp:GridView Width="100%" ShowHeader="true" ShowHeaderWhenEmpty="true"  EnableViewState="false" ShowFooter="true" GridLines="None"  ID="gvShipmentExpected" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="true" PageSize="25" AllowSorting="True" SkinID="gvLightGreenNew" HorizontalAlign="Left" OnRowDataBound="gvShipmentExpected_RowDataBound" OnSorting="gvShipmentExpected_Sorting" OnPageIndexChanging="gvShipmentExpected_PageIndexChanging">
                                        <Columns>
                                            
                                            <%--<asp:TemplateField ItemStyle-Width="150" HeaderText="Store Ref. #"  ItemStyle-CssClass="gvOBDNumber">--%>
                                            <asp:TemplateField ItemStyle-Width="150" HeaderText="<%$Resources:Resource,StoreRef%>"  ItemStyle-CssClass="gvOBDNumber">
                                                <ItemTemplate>

                                                    <asp:Literal runat="server" ID="ltStoreRefNo" Text='<%# GetStoreRefNoWithLink(DataBinder.Eval(Container.DataItem, "StoreRefNo").ToString(),DataBinder.Eval(Container.DataItem, "TenantID").ToString()) %>' />


                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%--<asp:TemplateField ItemStyle-Width="150" HeaderText="Shipment Type" ItemStyle-CssClass="home">--%>
                                            <asp:TemplateField ItemStyle-Width="150" HeaderText="<%$Resources:Resource,ShipmentType%>"  ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltShipType" Text='<%# DataBinder.Eval(Container.DataItem, "ShipmentType").ToString() %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                           <%-- <asp:TemplateField ItemStyle-Width="150" HeaderText="Doc. Rcvd. Dt." ItemStyle-CssClass="home">--%>
                                             <asp:TemplateField ItemStyle-Width="150" HeaderText="<%$Resources:Resource,DocRcvdDt%>" ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltDocRdvDate" Text='<%# DataBinder.Eval(Container.DataItem, "DocReceivedDate","{0: dd-MMM-yyyy}") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                           <%-- <asp:TemplateField ItemStyle-Width="150" HeaderText="Shipment Expected" ItemStyle-CssClass="home">--%>
                                             <asp:TemplateField ItemStyle-Width="150" HeaderText="<%$Resources:Resource,ShipmentExpected%>"  ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltShipmentExpectedDate" Text='<%# DataBinder.Eval(Container.DataItem, "ShipmentExpectedDate","{0: dd-MMM-yyyy}") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                           <%-- <asp:TemplateField ItemStyle-Width="250" HeaderText="Tenant" ItemStyle-CssClass="home">--%>
                                             <asp:TemplateField ItemStyle-Width="250" HeaderText="<%$Resources:Resource,Tenant%>"  ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltSETenant" Text='<%# DataBinder.Eval(Container.DataItem, "CompanyName") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                         <%--   <asp:TemplateField ItemStyle-Width="250" HeaderText="Supplier" ItemStyle-CssClass="home">--%>
                                               <asp:TemplateField ItemStyle-Width="250" HeaderText="<%$Resources:Resource,Supplier%>"  ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltSupplier" Text='<%# DataBinder.Eval(Container.DataItem, "SupplierName") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%--<asp:TemplateField ItemStyle-Width="200" HeaderText="Warehouse" ItemStyle-CssClass="home">--%>
                                            <asp:TemplateField ItemStyle-Width="200" HeaderText="<%$Resources:Resource,Warehouse%>"  ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltSoreAssociated" Text='<%# MRLWMSC21Common.CommonLogic.GetStoreNames(DataBinder.Eval(Container.DataItem, "ReferedStores").ToString(),"<br/>") %>' />
                                                    <asp:Literal runat="server" ID="ltShipmentLocation" Text='<%# DataBinder.Eval(Container.DataItem, "ShipmentLocation") %>' />
                                                </ItemTemplate>

                                            </asp:TemplateField>


                                            <%--<asp:TemplateField ItemStyle-Width="100" HeaderText="No. of Packages" ItemStyle-CssClass="home" ItemStyle-HorizontalAlign="Center">--%>
                                            <asp:TemplateField ItemStyle-Width="100" HeaderText="<%$Resources:Resource,NoofPackages%>"  ItemStyle-CssClass="home" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltNoofPackages" Text='<%# DataBinder.Eval(Container.DataItem, "NoofPackagesInDocument") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                          <%--  <asp:TemplateField ItemStyle-Width="100" HeaderText="Gross Weight" ItemStyle-CssClass="home" ItemStyle-HorizontalAlign="Right">--%>
                                              <asp:TemplateField ItemStyle-Width="100" HeaderText="<%$Resources:Resource,GrossWeight%>" ItemStyle-CssClass="home" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltGrossWeight" Text='<%# DataBinder.Eval(Container.DataItem, "GrossWeight") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                           <%-- <asp:TemplateField ItemStyle-Width="200" HeaderText="Projected Vehicles" ItemStyle-CssClass="home">--%>
                                             <asp:TemplateField ItemStyle-Width="200" HeaderText="<%$Resources:Resource,ProjectedVehicles%>"  ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltVehicleType" Text='<%# DataBinder.Eval(Container.DataItem, "ProjectedVehicleDetails").ToString() %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                           <%-- <asp:TemplateField ItemStyle-Width="250" HeaderText="Status" ItemStyle-CssClass="home">--%>
                                             <asp:TemplateField ItemStyle-Width="250" HeaderText="<%$Resources:Resource,Status%>"  ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <%--<asp:Literal runat="server" ID="ltShipmentStatus" Text='<%# MRLWMSC21Common.CommonLogic.GetShipmentStatus((DataBinder.Eval(Container.DataItem, "InBoundStatusID").ToString())) %>' />--%>
                                                    <asp:Literal runat="server" ID="ltShipmentStatus" Text='<%# DataBinder.Eval(Container.DataItem, "InboundStatus").ToString() %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="RTR" Visible="false" HeaderStyle-Width="200" ItemStyle-Width="150">
                                                <ItemTemplate>
                                                    <a id="A1" class="helpWTitle" Title="Receiving Tally Report(RTR) | Receiving Tally Report with barcoded material codes to receive items for putaway." runat="server" visible="true" href='<%# String.Format("RTReport.aspx?ibdid={0}&lineitemcount={1}&TN={2}", DataBinder.Eval(Container.DataItem, "InboundID").ToString(),DataBinder.Eval(Container.DataItem, "LineCount").ToString(),DataBinder.Eval(Container.DataItem, "CompanyName").ToString()) %>'>RTR
                                                        <asp:Literal runat="server" ID="ltLineCount" Text='<%# String.Format("[{0}]",DataBinder.Eval(Container.DataItem, "LineCount"))%>'></asp:Literal>
                                                    </a>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                           <%-- <asp:TemplateField ItemStyle-CssClass="home" HeaderText="Change">--%>
                                             <asp:TemplateField ItemStyle-CssClass="home" HeaderText="<%$Resources:Resource,Change%>">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEditInbound" runat="server" CssClass="GvLink" Visible="false" PostBackUrl='<%# Eval("InboundID", "InboundDetails.aspx?ibdid={0}&edittype=2") %>' Text="<nobr> Receive <img src='../Images/redarrowright.gif' border='0' /></nobr>"  />

                                                    <asp:HyperLink ID="HyperLink1" Text="<nobr> <i class='material-icons vl'>settings</i></nobr>" NavigateUrl='<%#  Eval("InboundID", "InboundDetails.aspx?ibdid={0}&edittype=2")  %>' Font-Underline="false" runat="server" ></asp:HyperLink>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                        <EmptyDataTemplate>
	                                      <%--  <div align="center">No Data Found</div>--%>
                                              <div align="center"> <%= GetGlobalResourceObject("Resource", "NoDataFound")%></div>
                                        </EmptyDataTemplate>
                                    </asp:GridView>



                                </td>
                            </tr>
                        </table>
                                </ContentTemplate>
                         
                        </asp:UpdatePanel>
                        <!-- Shipments Expected -->
                    </div>





                    <%--<h3>Shipments In Process <span id="lblSIPRecordCount" runat="server" /><sup><span id="lbltodaySIPRecCount" class="SIPblink" runat="server"></span></sup></h3>--%>
                    <h3><%= GetGlobalResourceObject("Resource", "ShipmentsInProcess")%>  <span id="lblSIPRecordCount" runat="server" /><sup><span id="lbltodaySIPRecCount" class="SIPblink" runat="server"></span></sup></h3>

                    <div>

                        <!-- Shipments IN - Process -->
                           <asp:UpdateProgress ID="uprogShipmentInProgress" runat="server" AssociatedUpdatePanelID="updpnlShipmentInProgress">
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
                        <asp:UpdatePanel ID="updpnlShipmentInProgress" runat="server" RenderMode="Inline" ViewStateMode="Enabled" ClientIDMode="Inherit" UpdateMode="Conditional">
                             <Triggers>
                                <asp:PostBackTrigger ControlID="btngvShipInProcess" />
                            </Triggers>
                            <ContentTemplate>
                        <table border="0" width="100%" cellpadding="5" cellspacing="5" align="center">

                            <tr class="absolute">
                                <td align="right" class="FormLabels">

                                    <asp:Panel ID="pnlsptinprocess" runat="server" DefaultButton="lnkSIpSearch">
                                        <div class="flex__end ">
                                                    <div class="flex">
                                                 <asp:TextBox ID="txtSIPWarehouse" runat="server" ClientIDMode="Static" SkinID="txt_Hidden_Req_Auto" required=""></asp:TextBox>
                                                 <asp:HiddenField ID="hdnSIPWarehouse" runat="server" Value="0" ClientIDMode="Static" />
                                                 <label>Warehouse</label>
                                                
                                             </div>

                                             &nbsp;&nbsp;
                                            <div class="flex">
                                        <asp:TextBox ID="txtSIPTenant" runat="server" SkinID="txt_Hidden_Req_Auto" required=""></asp:TextBox>
                                        <asp:HiddenField  ID="hifSIPTenant" runat="server" Value="0"/>
                                               <%-- <label>Search Tenant</label>--%>
                                                 <label><%= GetGlobalResourceObject("Resource", "SearchTenant")%></label>
                                                <input type="hidden" id="hiSIPTenantName" />
                                        </div>
                                        &nbsp;&nbsp;
                                            <div class="flex" width170>
                                        <asp:TextBox ID="txtSIPStoreRefNo" runat="server" SkinID="txt_Hidden_Req_Auto" required=""></asp:TextBox>
                                                <label> <%= GetGlobalResourceObject("Resource", "SearchStoreRef")%></label>
                                        </div>&nbsp;&nbsp;
                                            <div>
                                      <%--  <asp:LinkButton runat="server"  ID="lnkSIpSearch" OnClick="lnkSIpSearch_Click" CssClass="btn btn-sm btn-primary" >Search<span class="space fa fa-search"></span></asp:LinkButton>--%>
                                                 <%-- <asp:LinkButton runat="server"  ID="lnkSIpSearch" OnClientClick="return validate2();" OnClick="lnkSIpSearch_Click" CssClass="btn btn-sm btn-primary" > <%= GetGlobalResourceObject("Resource", "Search")%><span class="space fa fa-search"></span></asp:LinkButton>--%>
                                                 <asp:LinkButton runat="server"  ID="lnkSIpSearch"  OnClick="lnkSIpSearch_Click" CssClass="btn btn-sm btn-primary" > <%= GetGlobalResourceObject("Resource", "Search")%><span class="space fa fa-search"></span></asp:LinkButton>
                                        
                                        
                                       
                                          <%--<asp:ImageButton ID="btngvShipInProcess" runat="server" ImageAlign="AbsMiddle" ImageUrl="../Images/excel_icon.jpg" Width="20" OnClick="btngvShipInProcess_Click" ToolTip="Export To Excel" />--%>
                                               <%-- <asp:LinkButton ID="btngvShipInProcess" CssClass="btn btn-primary" runat="server" OnClick="btngvShipInProcess_Click">Export Excel <%=MRLWMSC21Common.CommonLogic.btnfaExcel %></asp:LinkButton>--%>
                                                 <asp:LinkButton ID="btngvShipInProcess" CssClass="btn btn-primary" runat="server" OnClick="btngvShipInProcess_Click"><%= GetGlobalResourceObject("Resource", "ExportExcel")%> <%=MRLWMSC21Common.CommonLogic.btnfaExcel %></asp:LinkButton>
                                   </div> </div></asp:Panel>
                                </td>
                            </tr>
                            <%--<tr>
                                <td class="FormLabels">
                                    <asp:Label ID="lblSIPStatusMsg" runat="server"></asp:Label>
                                </td>
                            </tr>--%>
                            <tr>
                                <td>
                                    <asp:GridView Width="100%" ShowHeader="true" ShowHeaderWhenEmpty="true"  EnableViewState="false" ShowFooter="false" GridLines="None"  ID="gvShipInProcess" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="true" PageSize="25" AllowSorting="True" SkinID="gvLightGrayNew" HorizontalAlign="Left" OnRowCommand="gvShipInProcess_RowCommand" OnRowDataBound="gvShipInProcess_RowDataBound" OnSorting="gvShipInProcess_Sorting" OnPageIndexChanging="gvShipInProcess_PageIndexChanging">

                                        <Columns>

                                         
                                            <asp:TemplateField ItemStyle-Width="150" HeaderText= "<%$Resources:Resource,StoreRef%>" ItemStyle-CssClass="gvOBDNumber">
                                                <ItemTemplate>

                                                    <asp:Literal runat="server" ID="ltStoreRefNo" Text='<%# GetStoreRefNoWithLink(DataBinder.Eval(Container.DataItem, "StoreRefNo").ToString(),DataBinder.Eval(Container.DataItem, "TenantID").ToString()) %>' />

                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%--<asp:TemplateField ItemStyle-Width="150" HeaderText="Shipment Type" ItemStyle-CssClass="home">--%>
                                            <asp:TemplateField ItemStyle-Width="150" HeaderText= "<%$Resources:Resource,ShipmentType%>" ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltShipType" Text='<%# DataBinder.Eval(Container.DataItem, "ShipmentType").ToString() %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%--<asp:TemplateField ItemStyle-Width="150" HeaderText="Doc. Rcvd. Dt." ItemStyle-CssClass="home">--%>
                                            <asp:TemplateField ItemStyle-Width="150" HeaderText="<%$Resources:Resource,DocRcvdDt%>"    ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltDocRdvDate" Text='<%# DataBinder.Eval(Container.DataItem, "DocReceivedDate","{0: dd-MMM-yyyy}") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                          <%--  <asp:TemplateField ItemStyle-Width="150" HeaderText="Shipment Exp. Dt." ItemStyle-CssClass="home">--%>
                                              <asp:TemplateField ItemStyle-Width="150" HeaderText= "<%$Resources:Resource,ShipmentExpDt%>"  ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltShipmentExpectedDate" Text='<%# DataBinder.Eval(Container.DataItem, "ShipmentExpectedDate","{0: dd-MMM-yyyy}") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                           <%-- <asp:TemplateField ItemStyle-Width="150" HeaderText="Ship. Rcvd. Dt." ItemStyle-CssClass="home">--%>
                                             <asp:TemplateField ItemStyle-Width="150" HeaderText="<%$Resources:Resource,ShipRcvdDt%>"  ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltShipmentRecivedDate" Text='<%# DataBinder.Eval(Container.DataItem, "ShipmentReceivedOn","{0: dd-MMM-yyyy}") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                           <%-- <asp:TemplateField ItemStyle-Width="250" HeaderText="Tenant" ItemStyle-CssClass="home">--%>
                                             <asp:TemplateField ItemStyle-Width="250" HeaderText="<%$Resources:Resource,Tenant%>"  ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltSIPTenant" Text='<%# DataBinder.Eval(Container.DataItem, "CompanyName") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%--<asp:TemplateField ItemStyle-Width="250" HeaderText="Supplier" ItemStyle-CssClass="home">--%>
                                            <asp:TemplateField ItemStyle-Width="250" HeaderText="<%$Resources:Resource,Supplier%>"  ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltSupplier" Text='<%# DataBinder.Eval(Container.DataItem, "SupplierName") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                           <%-- <asp:TemplateField ItemStyle-Width="200" HeaderText="Warehouse" ItemStyle-CssClass="home">--%>
                                             <asp:TemplateField ItemStyle-Width="200" HeaderText="<%$Resources:Resource,Warehouse%>" ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltSoreAssociated" Text='<%# MRLWMSC21Common.CommonLogic.GetSearchStoreNamesWithVerificationStatus(DataBinder.Eval(Container.DataItem, "ReferedStores").ToString(),"<br/>",DataBinder.Eval(Container.DataItem, "StoreRefNo").ToString(),DataBinder.Eval(Container.DataItem, "InboundID").ToString(),DataBinder.Eval(Container.DataItem,"TenantID").ToString() ) %>' />
                                                    <asp:Literal runat="server" ID="ltShipmentLocation" Text='<%# DataBinder.Eval(Container.DataItem, "ShipmentLocation") %>' />
                                                </ItemTemplate>

                                            </asp:TemplateField>

                                           <%-- <asp:TemplateField ItemStyle-Width="250" HeaderText="Status" ItemStyle-CssClass="home">--%>
                                             <asp:TemplateField ItemStyle-Width="250" HeaderText="<%$Resources:Resource,Status%>"  ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <%--<asp:Literal runat="server" ID="ltShipmentStatus" Text='<%# MRLWMSC21Common.CommonLogic.GetShipmentStatus((DataBinder.Eval(Container.DataItem, "InboundStatusID").ToString())) %>' />--%>
                                                    <asp:Literal runat="server" ID="ltShipmentStatus" Text='<%# DataBinder.Eval(Container.DataItem, "InboundStatus") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                           <%-- <asp:TemplateField HeaderText="RTR" HeaderStyle-Width="200" ItemStyle-Width="150">--%>
                                             <asp:TemplateField HeaderText="RTS"  HeaderStyle-Width="200" ItemStyle-Width="150">
                                                <ItemTemplate>
                                                    <a id="A1" class="helpWTitle" Title="Receiving Tally Sheet(RTS) | Receiving Tally Sheet with barcoded material codes to receive items for putaway." runat="server" visible="true" href='<%# String.Format("RTReport.aspx?ibdid={0}&lineitemcount={1}&TN={2}", DataBinder.Eval(Container.DataItem, "InboundID").ToString(),DataBinder.Eval(Container.DataItem, "LineCount").ToString(),DataBinder.Eval(Container.DataItem, "CompanyName").ToString()) %>'>RTR
                                                        <asp:Literal runat="server" ID="ltLineCount" Text='<%# String.Format("[{0}]",DataBinder.Eval(Container.DataItem, "LineCount"))%>'></asp:Literal>
                                                    </a>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="RCR" HeaderStyle-Width="200" ItemStyle-Width="150">
                                                <ItemTemplate>
                                                    <a id="A1" class="helpWTitle" title="Receiving Confirmation Report(RCR) | Receiving Tally Report with barcoded material codes to receive items for putaway." runat="server" visible="true" href='<%# String.Format("ReceiveConfirmationReport.aspx?ibdid={0}&lineitemcount={1}&TN={2}", DataBinder.Eval(Container.DataItem, "InboundID").ToString(),DataBinder.Eval(Container.DataItem, "LineCount").ToString(),DataBinder.Eval(Container.DataItem, "CompanyName").ToString()) %>'>RCR
                                                        <asp:Literal runat="server" ID="ltLineCount" Text='<%# String.Format("[{0}]",DataBinder.Eval(Container.DataItem, "LineCount"))%>'></asp:Literal>
                                                    </a>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                           <%-- <asp:TemplateField ItemStyle-CssClass="home" HeaderText="Change">--%>
                                             <asp:TemplateField ItemStyle-CssClass="home" HeaderText="<%$Resources:Resource,Change%>" >
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEditInbound" runat="server" CssClass="GvLink" PostBackUrl='<%# Eval("InboundID", "InboundDetails.aspx?ibdid={0}&edittype=3") %>' Text="<i class='material-icons vl'>settings</i></nobr>"  Visible="false" />

                                                    <asp:HyperLink ID="HyperLink1" Text="<nobr> <i class='material-icons vl'>settings</i></nobr>" NavigateUrl='<%#  Eval("InboundID", "InboundDetails.aspx?ibdid={0}&edittype=3")  %>' Font-Underline="false" runat="server" ></asp:HyperLink>

                                                </ItemTemplate>

                                            </asp:TemplateField>


                                        </Columns>
                                        <EmptyDataTemplate>
	                                     <%--   <div align="center">No Data Found</div>--%>
                                               <div align="center"> <%= GetGlobalResourceObject("Resource", "NoDataFound")%></div>
                                        </EmptyDataTemplate>
                                    </asp:GridView>

                                </td>
                            </tr>
                        </table>

                                </ContentTemplate>
                            </asp:UpdatePanel>
                        <!-- Shipments IN - Process -->


                    </div>

                    <%--<h3 style="display:none;">GRN Pending <span id="lblGRNRecCount" runat="server" /><sup><span id="lbltodayGRNRecCount" class="GRNblink" runat="server"></span></sup></h3>--%>
                    <h3 style="display:none;"><%= GetGlobalResourceObject("Resource", "GRNPending")%> <span id="lblGRNRecCount" runat="server" /><sup><span id="lbltodayGRNRecCount" class="GRNblink" runat="server"></span></sup></h3>

                    <div>

                        <!-- GRN Pending -->
                            <asp:UpdateProgress ID="uprogGRNPending" runat="server" AssociatedUpdatePanelID="uPnlGRNPending">
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
                        <asp:UpdatePanel ID="uPnlGRNPending" runat="server" RenderMode="Inline" ViewStateMode="Enabled" ClientIDMode="Inherit" UpdateMode="Conditional">
                               <Triggers>
                                <asp:PostBackTrigger ControlID="imgbtngvGRNPending" />
                            </Triggers>
                            <ContentTemplate>
                        <table border="0" width="100%" cellpadding="5" cellspacing="5" align="center">

                            <tr class="">
                                <td align="right" class="FormLabels absolute">
                                    <asp:Panel runat="server" ID="pnlgrnpending" DefaultButton="lnkGRNSearch">
                                        <div class="flex__end ">
                                           <div class="flex">
                                        <asp:TextBox ID="txtGRNTenant" runat="server" SkinID="txt_Hidden_Req_Auto" required=""></asp:TextBox>
                                        <asp:HiddenField  ID="hifGRNTenant" runat="server" Value="0"/>
                                             <%--  <label>Search Tenant</label>--%>
                                                 <label>Search Tenant <%= GetGlobalResourceObject("Resource", "SearchTenant")%></label>
                                        </div>
                                        &nbsp;&nbsp;
                                        <div class="flex">
                                        <asp:TextBox ID="txtGRNStoreRefNumber" runat="server" SkinID="txt_Hidden_Req_Auto" required=""></asp:TextBox>
                                           <%-- <label>Search Store Ref.#</label>--%>
                                             <label> <%= GetGlobalResourceObject("Resource", "SearchStoreRef")%></label>
                                        </div>
                                        &nbsp;&nbsp;
                                        <div>
                                        <asp:LinkButton runat="server" ID="lnkGRNSearch" OnClick="lnkGRNSearch_Click" CssClass="btn btn-sm btn-primary" >Search<span class="space fa fa-search"></span></asp:LinkButton>

                                       
                                        
                                         <%--<asp:ImageButton ID="imgbtngvGRNPending" runat="server" ImageAlign="AbsMiddle" ImageUrl="../Images/excel_icon.jpg" Width="20" OnClick="imgbtngvGRNPending_Click" ToolTip="Export To Excel" />--%>
                                            <asp:LinkButton ID="imgbtngvGRNPending" CssClass="btn btn-primary" runat="server" OnClick="imgbtngvGRNPending_Click">Export Excel <%=MRLWMSC21Common.CommonLogic.btnfaExcel %></asp:LinkButton>
                                    </div></div></asp:Panel>
                                </td>
                            </tr>
                            <%--<tr>
                                <td class="FormLabels">
                                    <asp:Label ID="lblGRNStatus" runat="server"></asp:Label>
                                </td>
                            </tr>--%>
                            <tr>
                                <td>
                                    <asp:GridView Width="100%" ShowHeader="true" ShowHeaderWhenEmpty="true" EnableViewState="false" ShowFooter="false" GridLines="None"  ID="gvGRNPending" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="true" PageSize="25" AllowSorting="True" SkinID="gvLightPurpleNew" HorizontalAlign="Left" OnRowDataBound="gvGRNPending_RowDataBound" OnSorting="gvGRNPending_Sorting" OnPageIndexChanging="gvGRNPending_PageIndexChanging">

                                        <Columns>
                                            
                                           <%-- <asp:TemplateField ItemStyle-Width="150" HeaderText="Store Ref. #" ItemStyle-CssClass="gvOBDNumber">--%>
                                             <asp:TemplateField ItemStyle-Width="150" HeaderText="<%$Resources:Resource,StoreRef%>" ItemStyle-CssClass="gvOBDNumber">
                                                <ItemTemplate>

                                                    <asp:Literal runat="server" ID="ltStoreRefNo" Text='<%# GetStoreRefNoWithLink(DataBinder.Eval(Container.DataItem, "StoreRefNo").ToString(),DataBinder.Eval(Container.DataItem, "TenantID").ToString()) %>' />

                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%--<asp:TemplateField ItemStyle-Width="150" HeaderText="Shipment Type" ItemStyle-CssClass="home">--%>
                                            <asp:TemplateField ItemStyle-Width="150" HeaderText="<%$Resources:Resource,ShipmentType%>"  ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltShipType" Text='<%# DataBinder.Eval(Container.DataItem, "ShipmentType").ToString() %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                          <%--  <asp:TemplateField ItemStyle-Width="150" HeaderText="Doc. Rcvd. Dt." ItemStyle-CssClass="home">--%>
                                              <asp:TemplateField ItemStyle-Width="150" HeaderText="<%$Resources:Resource,DocRcvdDt%>"  ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltDocRdvDate" Text='<%# DataBinder.Eval(Container.DataItem, "DocReceivedDate","{0: dd-MMM-yyyy}") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                           <%-- <asp:TemplateField ItemStyle-Width="150" HeaderText="Ship. Rcvd. Dt." ItemStyle-CssClass="home">--%>
                                             <asp:TemplateField ItemStyle-Width="150" HeaderText="<%$Resources:Resource,ShipRcvdDt%>"  ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltShipmentRecivedDate" Text='<%# DataBinder.Eval(Container.DataItem, "ShipmentReceivedOn","{0: dd-MMM-yyyy}") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%--<asp:TemplateField ItemStyle-Width="150" HeaderText="Ship. Verf. Dt." ItemStyle-CssClass="home">--%>
                                            <asp:TemplateField ItemStyle-Width="150" HeaderText="<%$Resources:Resource,ShipVerfDt%>"  ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltShipmentVerifiedDate" Text='<%# DataBinder.Eval(Container.DataItem, "ShipmentVerifiedOn","{0: dd-MMM-yyyy}") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%--<asp:TemplateField ItemStyle-Width="250" HeaderText="Tenant" ItemStyle-CssClass="home">--%>
                                            <asp:TemplateField ItemStyle-Width="250" HeaderText="<%$Resources:Resource,Tenant%>" ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltGRNTenant" Text='<%# DataBinder.Eval(Container.DataItem, "CompanyName") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%--<asp:TemplateField ItemStyle-Width="250" HeaderText="Supplier" ItemStyle-CssClass="home">--%>
                                            <asp:TemplateField ItemStyle-Width="250" HeaderText="<%$Resources:Resource,Supplier%>"  ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltSupplier" Text='<%# DataBinder.Eval(Container.DataItem, "SupplierName") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                           <%-- <asp:TemplateField ItemStyle-Width="200" HeaderText="Warehouse" ItemStyle-CssClass="home">--%>
                                             <asp:TemplateField ItemStyle-Width="200" HeaderText="<%$Resources:Resource,Warehouse%>"   ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltSoreAssociated" Text='<%# MRLWMSC21Common.CommonLogic.GetSearchStoreNamesWithVerificationStatus(DataBinder.Eval(Container.DataItem, "ReferedStores").ToString(),"<br/>",DataBinder.Eval(Container.DataItem, "StoreRefNo").ToString(),DataBinder.Eval(Container.DataItem, "InboundID").ToString(),DataBinder.Eval(Container.DataItem,"TenantID").ToString() ) %>' />
                                                    <asp:Literal runat="server" ID="ltShipmentLocation" Text='<%# DataBinder.Eval(Container.DataItem, "ShipmentLocation") %>' />
                                                </ItemTemplate>

                                            </asp:TemplateField>

                                           <%-- <asp:TemplateField ItemStyle-Width="250" HeaderText="Status" ItemStyle-CssClass="home">--%>
                                             <asp:TemplateField ItemStyle-Width="250" HeaderText="<%$Resources:Resource,Status%>"  ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <%--<asp:Literal runat="server" ID="ltShipmentStatus" Text='<%# MRLWMSC21Common.CommonLogic.GetShipmentStatus((DataBinder.Eval(Container.DataItem, "InBoundStatusID").ToString())) %>' />--%>
                                                    <asp:Literal runat="server" ID="ltShipmentStatus" Text='<%# DataBinder.Eval(Container.DataItem, "InboundStatus") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                          <%--  <asp:TemplateField HeaderText="RTR" HeaderStyle-Width="200" ItemStyle-Width="150">--%>
                                              <asp:TemplateField HeaderText="<%$Resources:Resource,RTR%>" HeaderStyle-Width="200" ItemStyle-Width="150">
                                                <ItemTemplate>
                                                    <a id="A1" class="helpWTitle" Title="Receiving Tally Report(RTR) | Receiving Tally Report with barcoded material codes to receive items for putaway." runat="server" visible="true" href='<%# String.Format("RTReport.aspx?ibdid={0}&lineitemcount={1}&TN={2}", DataBinder.Eval(Container.DataItem, "InboundID").ToString(),DataBinder.Eval(Container.DataItem, "LineCount").ToString(),DataBinder.Eval(Container.DataItem, "CompanyName").ToString()) %>'>RTR
                                                        <asp:Literal runat="server" ID="ltLineCount" Text='<%# String.Format("[{0}]",DataBinder.Eval(Container.DataItem, "LineCount"))%>'></asp:Literal>
                                                    </a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:TemplateField ItemStyle-CssClass="home" HeaderText="Change">--%>
                                            <asp:TemplateField ItemStyle-CssClass="home" HeaderText="<%$Resources:Resource,Change%>">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEditInbound" runat="server" CssClass="GvLink" Visible="false" PostBackUrl='<%# Eval("InboundID","InboundDetails.aspx?ibdid={0}&edittype=4") %>' Text="<i class='material-icons vl'>settings</i></nobr>"  />

                                                    <asp:HyperLink ID="HyperLink1" Text="<nobr> <i class='material-icons vl'>settings</i></nobr>" NavigateUrl='<%#  Eval("InboundID", "InboundDetails.aspx?ibdid={0}&edittype=4")  %>' Font-Underline="false" runat="server" ></asp:HyperLink>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                        </Columns>
                                        <EmptyDataTemplate>
	                                        <%--<div align="center">No Data Found</div>--%>
                                            <div align="center"><%= GetGlobalResourceObject("Resource", "NoDataFound")%></div>
                                        </EmptyDataTemplate>
                                    </asp:GridView>

                                </td>
                            </tr>
                        </table>
                                </ContentTemplate>
                                </asp:UpdatePanel>
                        <!-- GRN Pending -->


                    </div>


                   <%-- <h3 style="display:none;">Discrepancy Shipments <span id="lblDisRecCount" runat="server" /><sup><span id="lbltodayDISRecCount" class="DISblink" runat="server"></span></sup></h3>--%>
                     <h3 style="display:none;"><%= GetGlobalResourceObject("Resource", "DiscrepancyShipments")%> <span id="lblDisRecCount" runat="server" /><sup><span id="lbltodayDISRecCount" class="DISblink" runat="server"></span></sup></h3>

                    <div>

                        <!--  Discrepancy Shipments -->
                        
                             <asp:UpdateProgress ID="uprgDiscrepancyShipments" runat="server" AssociatedUpdatePanelID="upnlDiscrepancyShipments">
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
                        <asp:UpdatePanel ID="upnlDiscrepancyShipments" runat="server" RenderMode="Inline" ViewStateMode="Enabled" ClientIDMode="Inherit" UpdateMode="Conditional">
                             <Triggers>
                                <asp:PostBackTrigger ControlID="imgbtngvDiscrepancy" />
                            </Triggers>
                            <ContentTemplate>
                        <table border="0" width="100%" cellpadding="5" cellspacing="5" align="center">

                            <tr class="">
                                <td align="right" class="FormLabels">
                                    <asp:Panel runat="server" ID="pnldiscrepancyshpt" DefaultButton="lnkDiscSearch">
                                        <div class="flex__end"><div class="flex">
                                        <asp:TextBox ID="txtDiscTenant" required="" runat="server" SkinID="txt_Hidden_Req_Auto"></asp:TextBox>
                                        <asp:HiddenField  ID="hifDiscTenant" runat="server" Value="0"/>
                                           <%-- <label>Search Tenant</label>--%>
                                             <label> <%= GetGlobalResourceObject("Resource", "SearchTenant")%></label>
                                        </div>
                                        &nbsp;&nbsp;
                                            <div class="flex">
                                        <asp:TextBox ID="txtDiscStoreRefNumber"  runat="server" SkinID="txt_Hidden_Req_Auto" required=""></asp:TextBox>
                                                <%--<label>Search Store Ref.#</label>--%>
                                                <label> <%= GetGlobalResourceObject("Resource", "SearchStoreRef")%></label>
                                            </div>&nbsp;&nbsp;
                                            <div>
                                       <%-- <asp:LinkButton runat="server" ID="lnkDiscSearch" OnClick="lnkDiscSearch_Click" CssClass="btn btn-sm btn-primary" > Search <span class="space fa fa-search"></span></asp:LinkButton>--%>
                                                 <asp:LinkButton runat="server" ID="lnkDiscSearch" OnClick="lnkDiscSearch_Click" CssClass="btn btn-sm btn-primary" >  <%= GetGlobalResourceObject("Resource", "Search")%> <span class="space fa fa-search"></span></asp:LinkButton>
                                      
                                        
                                        <%--<asp:ImageButton ID="imgbtngvDiscrepancy" runat="server" ImageAlign="AbsMiddle" ImageUrl="../Images/excel_icon.jpg" Width="20" OnClick="imgbtngvDiscrepancy_Click" ToolTip="Export To Excel" />--%>
                                                <%--<asp:LinkButton ID="imgbtngvDiscrepancy" CssClass="btn btn-primary" runat="server" OnClick="imgbtngvDiscrepancy_Click">Export Excel <%=MRLWMSC21Common.CommonLogic.btnfaExcel %></asp:LinkButton>--%>
                                                <asp:LinkButton ID="imgbtngvDiscrepancy" CssClass="btn btn-primary" runat="server" OnClick="imgbtngvDiscrepancy_Click"><%= GetGlobalResourceObject("Resource", "ExportExcel")%>  <%=MRLWMSC21Common.CommonLogic.btnfaExcel %></asp:LinkButton>
                                        </div></div>
                                    </asp:Panel>
                                </td>
                            </tr>
                           <%-- <tr>
                                <td class="FormLabels">
                                    <asp:Label ID="lblDiscrStatus" runat="server"></asp:Label>
                                </td>
                            </tr>--%>
                            <tr>
                                <td>

                                    <asp:GridView Width="100%" ShowHeader="true" ShowHeaderWhenEmpty="true" EnableViewState="false" ShowFooter="false" GridLines="None"  ID="gvDiscrepancy" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="true" PageSize="25" AllowSorting="True" SkinID="gvLightLimeGreenNew" HorizontalAlign="Left" OnRowCommand="gvDiscrepancy_RowCommand" OnRowDataBound="gvDiscrepancy_RowDataBound" OnSorting="gvDiscrepancy_Sorting" OnPageIndexChanging="gvDiscrepancy_PageIndexChanging">

                                        <Columns>
                                            <%--<asp:TemplateField ItemStyle-Width="150" HeaderText="Store Ref. #" ItemStyle-CssClass="gvOBDNumber">--%>
                                            <asp:TemplateField ItemStyle-Width="150" HeaderText= "<%$Resources:Resource,StoreRef%>" ItemStyle-CssClass="gvOBDNumber">
                                                <ItemTemplate>

                                                    <asp:Literal runat="server" ID="ltStoreRefNo" Text='<%# GetStoreRefNoWithLink(DataBinder.Eval(Container.DataItem, "StoreRefNo").ToString(),DataBinder.Eval(Container.DataItem, "TenantID").ToString()) %>' />

                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%--<asp:TemplateField ItemStyle-Width="150" HeaderText="Shipment Type" ItemStyle-CssClass="home">--%>
                                            <asp:TemplateField ItemStyle-Width="150" HeaderText="<%$Resources:Resource,ShipmentType%>"  ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltShipType" Text='<%# DataBinder.Eval(Container.DataItem, "ShipmentType").ToString() %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>



                                            <%--<asp:TemplateField ItemStyle-Width="150" HeaderText="Doc. Rcvd. Dt." ItemStyle-CssClass="home">--%>
                                            <asp:TemplateField ItemStyle-Width="150" HeaderText="<%$Resources:Resource,DocRcvdDt%>" ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltDocRdvDate" Text='<%# DataBinder.Eval(Container.DataItem, "DocReceivedDate","{0: dd-MMM-yyyy}") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                           <%-- <asp:TemplateField ItemStyle-Width="150" HeaderText="Ship. Rcvd. Dt." ItemStyle-CssClass="home">--%>
                                             <asp:TemplateField ItemStyle-Width="150" HeaderText="<%$Resources:Resource,ShipRcvdDt%>"  ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltShipmentRecivedDate" Text='<%# DataBinder.Eval(Container.DataItem, "ShipmentReceivedOn","{0: dd-MMM-yyyy}") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                         <%--   <asp:TemplateField ItemStyle-Width="250" HeaderText="Tenant" ItemStyle-CssClass="home">--%>
                                               <asp:TemplateField ItemStyle-Width="250" HeaderText="<%$Resources:Resource,Tenant%>"  ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltDesTenant" Text='<%# DataBinder.Eval(Container.DataItem, "CompanyName") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%--<asp:TemplateField ItemStyle-Width="250" HeaderText="Supplier" ItemStyle-CssClass="home">--%>
                                            <asp:TemplateField ItemStyle-Width="250" HeaderText="<%$Resources:Resource,Supplier%>"   ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltSupplier" Text='<%# DataBinder.Eval(Container.DataItem, "SupplierName") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                           <%-- <asp:TemplateField ItemStyle-Width="200" HeaderText="Warehouse" ItemStyle-CssClass="home">--%>
                                             <asp:TemplateField ItemStyle-Width="200" HeaderText="<%$Resources:Resource,Warehouse%>"  ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <asp:Literal runat="server" ID="ltSoreAssociated" Text='<%# MRLWMSC21Common.CommonLogic.GetSearchStoreNamesWithVerificationStatus(DataBinder.Eval(Container.DataItem, "ReferedStores").ToString(),"<br/>",DataBinder.Eval(Container.DataItem, "StoreRefNo").ToString(),DataBinder.Eval(Container.DataItem, "InboundID").ToString(),DataBinder.Eval(Container.DataItem,"TenantID").ToString() ) %>' />
                                                    <asp:Literal runat="server" ID="ltShipmentLocation" Text='<%# DataBinder.Eval(Container.DataItem, "ShipmentLocation") %>' />
                                                </ItemTemplate>

                                            </asp:TemplateField>

                                          <%--  <asp:TemplateField ItemStyle-Width="250" HeaderText="Status" ItemStyle-CssClass="home">--%>
                                              <asp:TemplateField ItemStyle-Width="250" HeaderText=" <%$Resources:Resource,Status%>"  ItemStyle-CssClass="home">
                                                <ItemTemplate>
                                                    <%--<asp:Literal runat="server" ID="ltShipmentStatus" Text='<%# MRLWMSC21Common.CommonLogic.GetShipmentStatus((DataBinder.Eval(Container.DataItem, "InBoundStatusID").ToString())) %>' />--%>
                                                    <asp:Literal runat="server" ID="ltShipmentStatus" Text='<%# DataBinder.Eval(Container.DataItem, "InboundStatus") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                           <%-- <asp:TemplateField HeaderText="RTR" HeaderStyle-Width="200" ItemStyle-Width="150">--%>
                                             <asp:TemplateField HeaderText=" <%$Resources:Resource,RTR%>"  HeaderStyle-Width="200" ItemStyle-Width="150">
                                                <ItemTemplate>
                                                    <a id="A1" class="helpWTitle" Title="Receiving Tally Report(RTR) | Receiving Tally Report with barcoded material codes to receive items for putaway." runat="server" visible="true" href='<%# String.Format("RTReport.aspx?ibdid={0}&lineitemcount={1}&TN={2}", DataBinder.Eval(Container.DataItem, "InboundID").ToString(),DataBinder.Eval(Container.DataItem, "LineCount").ToString(),DataBinder.Eval(Container.DataItem, "CompanyName").ToString()) %>'>RTR
                                                        <asp:Literal runat="server" ID="ltLineCount" Text='<%# String.Format("[{0}]",DataBinder.Eval(Container.DataItem, "LineCount"))%>'></asp:Literal>
                                                    </a>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                           <%-- <asp:TemplateField ItemStyle-CssClass="home" HeaderText="Change">--%>
                                             <asp:TemplateField ItemStyle-CssClass="home" HeaderText=" <%$Resources:Resource,Change%>" >
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEditInbound" runat="server" Visible="false" CssClass="GvLink" PostBackUrl='<%# Eval("InboundID", "InboundDetails.aspx?ibdid={0}&edittype=3") %>' Text="<nobr> Update <img src='../Images/redarrowright.gif' border='0' /></nobr>"  />

                                                    <asp:HyperLink ID="HyperLink1" Text="<nobr> <i class='material-icons vl'>settings</i></nobr>" NavigateUrl='<%#  Eval("InboundID", "InboundDetails.aspx?ibdid={0}&edittype=3")  %>' Font-Underline="false" runat="server" ></asp:HyperLink>
                                                </ItemTemplate>

                                            </asp:TemplateField>
                                            

                                        </Columns>
                                        <EmptyDataTemplate>
	                                        <%--<div align="center">No Data Found </div>--%>
                                            <div align="center"> <%= GetGlobalResourceObject("Resource", "NoDataFound")%>

 </div>
                                        </EmptyDataTemplate>
                                    </asp:GridView>

                                </td>
                            </tr>
                        </table>
                            </ContentTemplate>
                            </asp:UpdatePanel>
                        <!--  Discrepancy Shipments -->


                    </div>






                </div>



            </td>
        </tr>
    </table>
</div>

    <asp:HiddenField ID="hidAccordionIndex" runat="server" Value="0" />
    

</asp:Content>
