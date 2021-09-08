<%@ Page Title="Tariff Creation" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="TariffCreation.aspx.cs" Inherits="FalconAdmin._3PLBilling.TariffCreation" MaintainScrollPositionOnPostback="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

      <asp:ScriptManager ID="mySManager" runat="server" EnablePartialRendering="true" SupportsPartialRendering="true" />
       <script src="../Scripts/toast/jquery.toastmessage.js"></script>
    <link href="Scripts/toast/jquery.toastmessage.css" rel="stylesheet" />
     <script type="text/javascript" src="../Scripts/CommonScripts.js"></script>
    <link href="tpl.css" rel="stylesheet" />
    <style>
        #MainContent_pnlActivityRate div{
            width:100%;
            overflow:auto;
        }


        .gvLightSeaBlue_DataCellGridEdit input[type="text"],  
        .gvLightSeaBlue_DataCellGridEdit select{
            width: 100px !important;
        }

        ul.tabs {
            padding: 7px 0;
            margin: 0;
            list-style-type: none;
            text-align: left;
            display: flex;
            flex-wrap: wrap;
            height: fit-content;
        }

        .gvLightSeaBlue_DataCellGridEdit td{
            padding-left:0px !important;
        }

            ul.tabs li {
             
                margin: 0;
                margin-right: 3px; /*distance between tabs*/
            }

                ul.tabs li a {
                    font: normal 12px Verdana;
                    text-decoration: none;
                    position: relative;
                    padding: 7px 16px;
                    border: 1px solid #CCC;
                    border-bottom-color: #B7B7B7;
                    color: #000;
                    background: #F0F0F0 url(tabbg.gif) 0 0 repeat-x;
                    border-radius: 3px 3px 0 0;
                    outline: none;
                    cursor: pointer;
                }

                    ul.tabs li a:visited {
                        color: #000;
                    }

                    ul.tabs li a:hover {
                        border: 1px solid #B7B7B7;
                        background: #F0F0F0 url(tabbg.gif) 0 -36px repeat-x;
                    }

                ul.tabs li.selected a, ul.tabs li.selected a:hover {
                    position: relative;
                    top: 0px;
                    font-weight: bold;
                    background: white;
                    border: 1px solid #B7B7B7;
                    border-bottom-color: white;
                }


                    ul.tabs li.selected a:hover {
                        text-decoration: none;
                    }


                div.tabcontents {
                    border: 1px solid #B7B7B7;
                    padding: 30px;
                    background-color: #FFF;
                    border-radius: 0 3px 3px 3px;
                }

                .DynamicTabs {
                    color: #fff;
                }

                .gvLightSeaBlue_pager table tr{
                    display: flex;
                    justify-content: flex-end;
                }
    </style>

    <script type="text/javascript">
        $(function () {

            var activeIndex1 = parseInt($('#<%=hidAccordionIndex1.ClientID%>').val());
             var activeIndex2 = parseInt($('#<%=hidAccordionIndex2.ClientID%>').val());
             var activeIndex3 = parseInt($('#<%=hidAccordionIndex3.ClientID%>').val());

             $("#accordion1").accordion({
                 expandAll: false,
                 alwaysOpen: false,
                 autoHeight: false, clearStyle: true,
                 active: activeIndex1,
                 change: function (event, ui) {
                     var index = $(this).children('h3').index(ui.newHeader);
                     $('#<%=hidAccordionIndex1.ClientID%>').val(index);
                }
            });
             $("#accordion1").accordion({ header: 'h3', collapsible: true, autoHeight: false, clearStyle: true, navigation: true });


             $("#accordion2").accordion({
                 alwaysOpen: false,
                 autoHeight: false, clearStyle: true,
                 active: activeIndex2,
                 change: function (event, ui) {
                     var index = $(this).children('h3').index(ui.newHeader);
                     $('#<%=hidAccordionIndex2.ClientID%>').val(index);
                }
            });
             $("#accordion2").accordion({ header: 'h3', collapsible: true, autoHeight: false, clearStyle: true, navigation: true });


             $("#accordion3").accordion({
                 alwaysOpen: false,
                 autoHeight: false, clearStyle: true,
                 active: activeIndex3,
                 change: function (event, ui) {
                     var index = $(this).children('h3').index(ui.newHeader);
                     $('#<%=hidAccordionIndex3.ClientID%>').val(index);
                }
            });
             $("#accordion3").accordion({ header: 'h3', collapsible: true, autoHeight: false, clearStyle: true, navigation: true });

         });

    </script>

    <script>
        function SelecteTab(index, type) {
            //alert(type == 'Activity');
            if (type == 'Activity') {

                $('#hifSelectedTabID').val(index);
                __doPostBack('GoEvent', 'ChangeActivityRateTab');
            }
            else {
                $('#hifTabsActivityRateType').val(index);
                __doPostBack('GoEvent', 'ChangeActivityRateTypeTab');
            }
        }
    </script>


    <script type="text/javascript">

         
           
       
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
                //alert('load Autocomplete');
                //$('#txtEffectiveFrom').datepicker({
                //    dateFormat: 'dd/mm/yy',
                //    onSelect: function (selected) {
                //        var datepart = selected.split('/');
                //        var dat = new Date(datepart[2], (datepart[1] - 1), datepart[0]);
                //        dat.setDate(dat.getDate() + 1);
                //        $("#txtEffectiveTo").datepicker("option", "minDate", dat, { dateFormat: "dd/mm/yy" })
                //    }
                //});
                //$('#txtEffectiveTo').datepicker({
                //    dateFormat: 'dd/mm/yy',
                //});

                var EffectiveDate = $("#hifEffectiveDate").val();

                $('#txtEffectiveFrom').datepicker({
                    dateFormat: "dd/mm/yy",
                    onSelect: function (selected) {
                        $("#txtEffectiveTo").datepicker("option", "minDate", selected, { dateFormat: "dd/mm/yy" })
                    }
                });

                var _minDate = new Date(1990, 1, 1, 0, 0, 0);
                if (EffectiveDate != undefined) {
                    var date = EffectiveDate;
                    _minDate = new Date(date.split('/')[2], parseInt(date.split('/')[1]) - 1, date.split('/')[0], 0, 0, 0, 0);
                }

                $('#txtEffectiveTo').datepicker({
                    dateFormat: "dd/mm/yy",
                    minDate: _minDate,
                    onSelect: function (selected) {
                        $("#txtEffectiveFrom").datepicker("option", "maxDate", selected, { dateFormat: "dd/mm/yy" })
                    }
                });
                var textfieldname = $("#txtActivityRateType");//Added by kashyap on 28/08/2017 for dropdown fucntion
                DropdownFunction(textfieldname);//Added by kashyap on 28/08/2017 for dropdown fucntion

                $("#txtActivityRateType").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetActivityTariffType") %>',
                            data: "{ 'prefix': '" + request.term + "', 'ActivityRateGroupID': '" + document.getElementById('<%=this.hifTabsActivityRateType.ClientID%>').value + "' }",
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

                        $("#hifARTActivityRateGroup").val(i.item.val);
                        //alert($("#hifToteTypeID").val());
                    },
                    minLength: 0
                });

                var textfieldname = $("#txtServiceType"); //Added by kashyap on 28/08/2017 for dropdown fucntion
                DropdownFunction(textfieldname); //Added by kashyap on 28/08/2017 for dropdown fucntion

                $("#txtServiceType").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetServiceTypeForTPL") %>',
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
                             }
                         });
                     },
                     select: function (e, i) {

                         $("#hifServiceType").val(i.item.val);
                         //alert($("#hifToteTypeID").val());
                     },
                     minLength: 0
                });

                var textfieldname = $("#txtInOutType"); //Added by kashyap on 28/08/2017 for dropdown fucntion
                DropdownFunction(textfieldname); //Added by kashyap on 28/08/2017 for dropdown fucntion

                $("#txtInOutType").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetInOutForTPL") %>',
                             data: "{ 'prefix': '" + request.term + "', 'ActivityRateGroupID': '" + document.getElementById('<%=this.hifTabsActivityRateType.ClientID%>').value + "' }",
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

                         $("#hifInOut").val(i.item.val);
                         //alert($("#hifToteTypeID").val());
                     },
                     minLength: 0
                });

                var textfieldname = $("#txtARActivityRateType"); //Added by kashyap on 28/08/2017 for dropdown fucntion
                DropdownFunction(textfieldname); //Added by kashyap on 28/08/2017 for dropdown fucntion

                $("#txtARActivityRateType").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetActivityTariffType") %>',
                             data: "{ 'prefix': '" + request.term + "', 'ActivityRateGroupID': '" + document.getElementById('<%=this.hifSelectedTabID.ClientID%>').value + "' }",
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

                         $("#hifARActivityRateType").val(i.item.val);
                         //alert($("#hifToteTypeID").val());
                     },
                     minLength: 0
                });

                var textfieldname = $("#txtWareHouse");
                DropdownFunction(textfieldname);

                $("#txtWareHouse").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                           // url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadWarehouses") %>',
                              url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoaDWHListBasedonUser") %>', // added by Ganesh --Wh Drop should be displayed by User
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
                            }
                        });
                    },
                    select: function (e, i) {

                        $("#hifwarehouse").val(i.item.val);
                        //alert($("#hifToteTypeID").val());
                    },
                    minLength: 0
                });

                var textfieldname = $("#txtUoM"); //Added by kashyap on 28/08/2017 for dropdown fucntion
                DropdownFunction(textfieldname); //Added by kashyap on 28/08/2017 for dropdown fucntion

                $("#txtUoM").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetUoMForTPL") %>',
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
                             }
                         });
                     },
                     select: function (e, i) {

                         $("#hifUoMID").val(i.item.val);
                         //alert($("#hifToteTypeID").val());
                     },
                     minLength: 0
                });

                var textfieldname = $("#txtCurrency"); //Added by kashyap on 28/08/2017 for dropdown fucntion
                DropdownFunction(textfieldname); //Added by kashyap on 28/08/2017 for dropdown fucntion
                
                $("#txtCurrency").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetCurrencyForTPL") %>',
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
                             }
                         });
                     },
                     select: function (e, i) {

                         $("#hifCurrency").val(i.item.val);
                         //alert($("#hifToteTypeID").val());
                     },
                     minLength: 0
                 });

                var textfieldname = $("#<%= this.txtActivitytariffGroup1.ClientID %>");
                DropdownFunction(textfieldname);

                $("#<%= this.txtActivitytariffGroup1.ClientID %>").autocomplete({
                     source: function (request, response) {
                         $.ajax({
                             url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetTariffGroupList") %>',
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


                         });
                     },
                     select: function (e, i) {
                         //$("#<=hifTarrifGrouid.ClientID %>").val(i.item.val);
                         $('#hifSelectedTabID').val(i.item.val);
                         $('#hifTabsActivityRateType').val(i.item.val);
                         //alert($('#hifSelectedTabID').val() + 'dd' + $('#hifTabsActivityRateType').val());
                     },
                     minLength: 0
                 });
                var textfieldname = $("#<%= this.txtActivityTariffType2.ClientID %>");
                DropdownFunction(textfieldname);

                $("#<%= this.txtActivityTariffType2.ClientID %>").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetActivityTariffType") %>',
                             data: "{ 'prefix': '" + request.term + "', 'ActivityRateGroupID': '" + document.getElementById('<%=this.hifTabsActivityRateType.ClientID%>').value + "' }",
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
                    minLength: 0
                });


                var textfieldname = $("#<%= this.txtActivityTariffType3.ClientID %>");
                DropdownFunction(textfieldname);
                $("#<%= this.txtActivityTariffType3.ClientID %>").autocomplete({
                     source: function (request, response) {
                         $.ajax({
                             url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetActivityTariffType") %>',
                             data: "{ 'prefix': '" + request.term + "', 'ActivityRateGroupID': '" + document.getElementById('<%=this.hifSelectedTabID.ClientID%>').value + "' }",
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

                         $("#<%=hidActivityTariffType3.ClientID %>").val(i.item.val);
                          $("#<%= this.txtActivityTariff3.ClientID %>").val(''); // added by Ganesh @Sep 25 2020
                     },
                     minLength: 0
                });


                var textfieldname = $("#<%= this.txtWareHouse_Auto.ClientID %>");
                DropdownFunction(textfieldname);
                $("#<%= this.txtWareHouse_Auto.ClientID %>").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoaDWHListBasedonUser") %>', // added by Ganesh , WH displayed by User
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
                             }
                         });
                     },
                    select: function (e, i) {

                        $("#<%=hidWareHouseID.ClientID %>").val(i.item.val);
                        $("#<%= this.txtActivityTariffType3.ClientID %>").val('');  // added Ganesh @Sep 25 2020
                        $("#<%= this.txtActivityTariff3.ClientID %>").val('');

                     },
                     minLength: 0
                });
                
               
                var textfieldname = $("#<%= this.txtActivityTariff3.ClientID %>");
                DropdownFunction(textfieldname);

                $("#<%= this.txtActivityTariff3.ClientID %>").autocomplete({
                     source: function (request, response) {
                         $.ajax({
                             url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetActivityTariff") %>',
                             data: "{ 'prefix': '" + request.term + "', 'ActivityRateGroupID': '" + document.getElementById('<%=this.hifSelectedTabID.ClientID%>').value + "', 'ActivityRateTypeID': '" + document.getElementById('<%=this.hidActivityTariffType3.ClientID%>').value + "'}",
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
                     minLength: 0
                 });
            });
         }
         fnMCodeAC();
    </script>

    <script type="text/javascript">

        $(document).ready(function () {
            $("#divEditInvoiceDispute").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 50,
                minWidth: 300,
                height: 550,
                width: 550,
                overflow: 'auto',
                resizable: false,
                position: ["center top", 40],
                close: function () {

                    $(".ui-dialog").fadeOut(500);
                    $(document).unbind('scroll');
                    $('body').css({ 'overflow': 'visible' });

                },

                open: function (event, ui) {
                    $(".ui-dialog").hide().fadeIn(500);

                    $('body').css({ 'overflow': 'hidden' });
                    $('body').width($('body').width());

                    $(document).bind('scroll', function () {
                        window.scrollTo(0, 0);
                    });

                    $(this).parent().appendTo("#divEditInvoiceDisputeDlgContainer");
                }
            });
        });

        function closeDialog() {
            //Could cause an infinite loop because of "on close handling"
            $("#divEditInvoiceDispute").dialog('close');
        }


        function openDialog(title, linkID) {

            $("#divEditInvoiceDispute").dialog("option", "title", "Tenant Tariff");
            $("#divEditInvoiceDispute").dialog('open');
            NProgress.start();
            $("#divEditInvoiceDispute").block({
                message: '<img src="<%=ResolveUrl("~") %>Images/async_master.gif" />',
                  css: { border: '0px' },
                  fadeIn: 0,
                  fadeOut: 0,
                  overlayCSS: { backgroundColor: '#ffffff', opacity: 1 }
              });
          }

          function unblockDialog() {
              $("#divEditInvoiceDispute").unblock();
              NProgress.done();
          }

          function ClearTextRateGroup(TextBox) {
              if (TextBox.value == "Tariff Group...");
              TextBox.value = "";
          }

          function focuslostRateGroup(TextBox) {
              if (TextBox.value == "")
                  TextBox.value = "Tariff Group...";
          }

          function ClearTextRateType(TextBox) {
              if (TextBox.value == "Tariff Sub-Group...");
              TextBox.value = "";
          }
          function ClearTextRateType_Ware(TextBox) {
              if (TextBox.value == "Ware House...");
              TextBox.value = "";
          }
          function focuslostRateType(TextBox) {
              if (TextBox.value == "")
                  TextBox.value = "Tariff Sub-Group...";
          }
          function focuslostRateType_WareHouse(TextBox) {
              if (TextBox.value == "")
                  TextBox.value = "WareHouse...";
          }
          function ClearTextRate(TextBox) {
              if (TextBox.value == "Tariff...");
              TextBox.value = "";
          }
          function focuslostRate(TextBox) {
              if (TextBox.value == "")
                  TextBox.value = "Tariff...";
          }

          function CollapseAll() {
              //$("#collapseAll").click(function () {
              $(".ui-accordion-content").hide();
              $('.ui-icon').toggleClass('ui-icon-triangle-1-s');
              //});
          }
          function ExpandAll() {
              //$("#expandAll").click(function () {

              $(".ui-accordion-content").show();

              $('.ui-icon').toggleClass('ui-icon-triangle-1-s');
              //});
          }

    </script>

    <style>
        .modal {
            position: fixed;
            z-index: 999;
            height: 100%;
            width: 100%;
            top: 0;
            left: 0;
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
            -moz-opacity: 0.8;
        }

        .center {
            z-index: 1001;
            margin: 50% auto;
            /*
            padding: 10px;
            border-radius: 10px;*/
            width: 130px;
            background-color: red;
            opacity: 1;
            -moz-opacity: 1;
        }

        table tr td {
        white-space: initial !important;
        }
    </style>
   <div class="dashed"></div>
    <%--  <table class="tbsty" style="table-layout:fixed">
        <tbody>
            <tr class="module_yellow">
               <td class="ModuleHeader fixed-width">
                    
  
                   <div><a href="../Default.aspx">Home</a> / 3PL / <span class="breadcrumbd">Tariff Creation</span></div>
                </td>
             </tr>
        </tbody>
    </table>--%>
      <div class="module_yellow">
            <div class="ModuleHeader" height="35px">
                <div><a href="../Default.aspx">Home</a> <i class="material-icons">arrow_right</i> <a href="#">3PL</a> <i class="material-icons">arrow_right</i> <span class="breadcrumbd" contenteditable="false">Tariff Creation<asp:Literal ID="ltFormSubHeading" runat="server"/> </span></div>
                <%--<div class="mandatory"><b>Note:</b> <span style="color:red"> __ </span>Indicates mandatory fields</div>--%>
            </div>

        </div>
    <div class="container">

    <div id="divEditInvoiceDisputeDlgContainer">  

      <div id="divEditInvoiceDispute" >  

        <asp:UpdatePanel ID="upnlEditCustomer" runat="server" ChildrenAsTriggers="true" UpdateMode="Always" >
           <ContentTemplate>
                
              <div class="ui-dailog-body" style="height: 445px; padding-left: 10px; padding-right: 10px;">
                 <table class="" width="100%" colspan="1">
       
                    <tr>
                      <td class="minHeight">
                            <asp:Label ID="lblStatus" runat="server" /><br />
                            <asp:GridView ID="gvTenantTariffAllocation" runat="server" SkinID="gvLightSeaBlue" AllowPaging="true" ShowHeader="true" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" PageSize="25"
                                OnRowDataBound="gvTenantTariffAllocation_RowDataBound"
                                OnPageIndexChanging="gvTenantTariffAllocation_PageIndexChanging">
                                 <Columns>

                                     
                       <%--     <asp:TemplateField HeaderText="Tenant" ItemStyle-Width="100">--%>
                                          <asp:TemplateField HeaderText="<%$Resources:Resource,Tenant%>" ItemStyle-Width="100">
                                <ItemTemplate>
                                    <asp:Literal ID="ltTenantName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TenantName") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                       
                            <%--<asp:TemplateField HeaderText="Unit Cost" ItemStyle-Width="80">
                                <ItemTemplate>
                                    <asp:Literal ID="ltUnitCost" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"UnitCost") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>

                                </EditItemTemplate>
                            </asp:TemplateField>--%>
                      
<%--                            <asp:TemplateField HeaderText="Eff. From" ItemStyle-Width="50">--%>
                                     <asp:TemplateField HeaderText="<%$Resources:Resource,EffFrom%>" ItemStyle-Width="50">
                                <ItemTemplate>
                                    <asp:Literal ID="ltEffectiveFrom" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"EffectiveFrom","{0:dd/MM/yyyy}") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        
                           <%-- <asp:TemplateField HeaderText="Eff. To" ItemStyle-Width="50">--%>
                                      <asp:TemplateField HeaderText="<%$Resources:Resource,EffTo%>"  ItemStyle-Width="50">
                                <ItemTemplate>
                                    <asp:Literal ID="ltEffectiveTo" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"EffectiveTo","{0:dd/MM/yyyy}") %>' />
                                </ItemTemplate>
                            
                            </asp:TemplateField>
                      
                        </Columns>
                                <EmptyDataTemplate>
								<%--	<div align="center">No Data Found</div>--%>
                                    	<div align="center"><%= GetGlobalResourceObject("Resource", "NoDataFound ")%></div>
								</EmptyDataTemplate>
                             </asp:GridView>
                       </td>
                    </tr>
                  </table>

            </div>

             <div class="ui-dailog-footer">
                <div style="padding: 15px 13px 15px 5px;">

                  <%-- <asp:LinkButton ID="btnCancel" onclick="btnCancel_Click"  CssClass="ui-btn ui-button-large" CausesValidation="false"  runat="server" >
                        Close <%= MRLWMSC21Common.CommonLogic.btnfaClear %>
                    </asp:LinkButton>--%>
                     <asp:LinkButton ID="btnCancel" onclick="btnCancel_Click"  CssClass="ui-btn ui-button-large" CausesValidation="false"  runat="server" >
                         <%= GetGlobalResourceObject("Resource", "Close")%> <%= MRLWMSC21Common.CommonLogic.btnfaClear %>
                    </asp:LinkButton>

                </div>
              </div>

               

           </ContentTemplate>
        </asp:UpdatePanel>

      </div>
    </div>

 
   <div>
<%--        <tr>

            <td>
                <table border="0" width="97%"  cellpadding="2" cellspacing="1">
                    <tr> 
                        <td align="left">
                       </td>
                        <td align="right">
                            <a id="collapseAll" onclick="CollapseAll()" href="#" class="btn btn-primary">Collapse All <span class="space fa fa-compress"></span></a> &nbsp; &nbsp; &nbsp;
                            <a id="expandAll" onclick="ExpandAll()" href="#" class="btn btn-primary">Expand All <span class="space fa fa-expand" ></span></a>
                        </td>
                    </tr>
                </table>
            </td>

        </tr>--%>
     
        <div class="row">
           <div align="center">

             <div id="accordion1" align="left" class="accordion">
           <%--   <h3>1. Tariff Groups </nobr></h3>--%>
                <h3> <%= GetGlobalResourceObject("Resource", "TariffGroups")%></h3>
                 <div align="center">

                   <div border="0" cellpadding="1" cellspacing="1"  align="center">
         
                    <div class="row">
                        <div >
                            <div class="row">
                                <div class="col m2 offset-m9">
                                    <div class="flex">
                                        <asp:TextBox runat="server" required="" ID="txtActivitytariffGroup1" SkinID="txt_Hidden_Req_Auto" />
                                        <label>Tariff Group...</label>
                                        <asp:HiddenField ID="hifTarrifGrouid" runat="server" />
                                    </div>
                                </div>
                                <div class="col m1 p0">
                                    <gap5></gap5>
                                    <asp:LinkButton runat="server" ID="lnkTariffGroupSearch" CssClass="btn btn-primary" OnClick="lnkTariffGroupSearch_Click"> <%= GetGlobalResourceObject("Resource", "Search")%> <%= MRLWMSC21Common.CommonLogic.btnfaSearch %> </asp:LinkButton>

                                    <asp:LinkButton Visible="false" ID="lnkbtntariffgroup" runat="server" Font-Underline="False" CssClass="btn btn-primary" OnClick="lnkbtntariffgroup_Click"> <%= GetGlobalResourceObject("Resource", "AddNew")%><%=MRLWMSC21Common.CommonLogic.btnfaNew%></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
       
                    <div>
                        <div align="right" class="auto-style3"> 
                          <%--  <asp:LinkButton ID="lnkAddNewTariffGroup" runat="server" Visible="false"   OnClick="lnkAddNewTariffGroup_Click" CssClass="btn btn-primary">Add New <%=MRLWMSC21Common.CommonLogic.btnfaNew %></asp:LinkButton>--%>
                              <asp:LinkButton ID="lnkAddNewTariffGroup" runat="server" Visible="false"   OnClick="lnkAddNewTariffGroup_Click" CssClass="btn btn-primary"><%= GetGlobalResourceObject("Resource", "AddNew")%> <%=MRLWMSC21Common.CommonLogic.btnfaNew %></asp:LinkButton>
         
                        </div> 
                    </div>

                 </div>
                      <div class="ListDataRow">

                          <div >
                              <%--<asp:UpdateProgress ID="uppgActivityGroup" runat="server" AssociatedUpdatePanelID="upnlActivityGroups"  >
                                  <ProgressTemplate>
                                      <div class="ui-widget-overlay">
                                          <div class="center">
                                              <img alt="" src="../Images/ui-anim_basic_16x16.gif" />
                                          </div>
                                      </div>
                                  </ProgressTemplate>
                              </asp:UpdateProgress>--%>
                                 <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" ID="upnlActivityGroups" UpdateMode="Always">
                                        <ContentTemplate>
                              <asp:Label runat="server" ID="lblTariffGroup" CssClass="errorMsg" ForeColor="DarkGreen"></asp:Label> 

                              <asp:Panel runat="server" ID="pnlTPLTariffGroups">
                                    <asp:GridView ID="gvTPLTariffGroups" runat="server" ShowHeader="true" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" AllowPaging="true" PageSize="25" SkinID="gvLightSeaBlue" CellPadding="4" Width="80%"
                                    OnRowDataBound="gvTPLTariffGroups_RowDataBound"
                                    OnRowEditing="gvTPLTariffGroups_RowEditing"
                                    OnRowUpdating="gvTPLTariffGroups_RowUpdating"
                                    OnRowCancelingEdit="gvTPLTariffGroups_RowCancelingEdit"
                                    OnPageIndexChanging="gvTPLTariffGroups_PageIndexChanging">
                                    <Columns>
                                        <%--<asp:TemplateField HeaderText="Tariff Group" ItemStyle-Width="200" HeaderStyle-HorizontalAlign="Center">--%>
                                        <asp:TemplateField HeaderText="<%$Resources:Resource,TariffGroup%>" ItemStyle-Width="200" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="ltActivityRateGroup" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ActivityRateGroup") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <div class="gridInput">
                                                <asp:Literal ID="ltActivityRateGroupID" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ActivityRateGroupID") %>' />
                                                <asp:RequiredFieldValidator ID="rfvActivityRateGroup" runat="server" Display="Dynamic" ControlToValidate="txtActivityRateGroup" ValidationGroup="vRequiredTariffGroup" ErrorMessage="*" />
                                                <asp:TextBox ID="txtActivityRateGroup" ClientIDMode="Static" SkinID="txt_Auto_Req" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ActivityRateGroup") %>' />
                                    <span class="errorMsg"></span>     
                                    </div>
                                            </EditItemTemplate>
                                        </asp:TemplateField>

                                       <%-- <asp:TemplateField HeaderText="Description" ItemStyle-Width="250" HeaderStyle-HorizontalAlign="Center">--%>
                                         <asp:TemplateField HeaderText="<%$Resources:Resource,Description%>" ItemStyle-Width="250" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Literal ID="ltRateGroupDescription" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"RateGroupDescription") %>' />

                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                    <div class="gridInput">
                                                <asp:TextBox ID="txtRateGroupDescription" ClientIDMode="Static" SkinID="txt_Auto_Req" Width="300" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"RateGroupDescription") %>' />
                                            </div>
                                                        </EditItemTemplate>
                                        </asp:TemplateField>

                                      <%-- <asp:TemplateField HeaderText="Active" ItemStyle-Width="50" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">--%>
                                         <asp:TemplateField Visible="false" HeaderText="<%$Resources:Resource,Active%>" ItemStyle-Width="50" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                             <ItemTemplate>
                                                  <asp:Literal runat="server" ID="ltIsActive" Text='<%# GetCheckValueOrDeleted(DataBinder.Eval(Container.DataItem, "IsActive").ToString()) %>'/>
                                              </ItemTemplate>
                                                           
                                             <EditItemTemplate>
                                                   <div class="pure-material-checkbox"><asp:CheckBox ID="chkActive" runat="server" Checked='<%# Convert.ToBoolean( Convert.ToInt16(DataBinder.Eval(Container.DataItem, "IsActive"))) %>' /><span></span></div>
                                             </EditItemTemplate>
                                       </asp:TemplateField>

                                        <%--<asp:TemplateField ItemStyle-Width="50" HeaderText="Delete" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint">

                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkIsDelete" runat="server" />
                                                                </ItemTemplate>
                                                                <EditItemTemplate></EditItemTemplate>

                                                                <FooterTemplate>
                                                                    <asp:LinkButton ID="lnkDeleteActivityTariffGroup" Font-Underline="false" CssClass="blueLink" runat="server" Text="<nobr> Delete <img src='../Images/redarrowright.gif' border='0' /></nobr>" OnClick="lnkDeleteActivityTariffGroup_Click" OnClientClick="return confirm('Are you sure you want to delete the selected line items?')" />
                                                                </FooterTemplate>
                                                            </asp:TemplateField>--%>
            
                                    <asp:CommandField ValidationGroup="UpdateRevision" Visible="false" ItemStyle-Font-Underline="false" ControlStyle-Font-Underline="false" ItemStyle-Width="30" ButtonType="Link" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint" ItemStyle-HorizontalAlign="left" CancelImageUrl="Images/cancel.gif" EditImageUrl="/Images/edit.gif" EditText="<nobr> <i class='material-icons ss'>mode_edit</i></nobr>" ShowEditButton="True" UpdateImageUrl="/Images/save.gif" />


                                    </Columns>
                                    <EmptyDataTemplate>
										<%--<div align="center">No Data Found</div>--%>
                                        <div align="center"> <%= GetGlobalResourceObject("Resource", "NoDataFound")%></div>
									</EmptyDataTemplate>
                                </asp:GridView>
                                  </asp:Panel>
                   
                                                 </ContentTemplate>
                                    </asp:UpdatePanel>                    
                          </div>
                      </div> 
          
       
               </div>
             </div>


               <div id="accordion2" align="left" class="accordion">
                   <h3><%= GetGlobalResourceObject("Resource", "TariffSubGroups")%></h3>
                   <div align="center">
                       <table border="0" cellpadding="1" cellspacing="1" align="center">
                           <tr>
                               <td colspan="2">
                                   <asp:Literal ID="ltDynamicTabsActivityRateType" runat="server" />
                                   <asp:HiddenField ID="hifTabsActivityRateType" Value="1" runat="server" ClientIDMode="Static" />
                               </td>
                           </tr>

                           <tr>
                               <td align="right" valign="top" colspan="1">
                                   <div class="row">
                                       <div class="col m2 offset-m9">
                                           <div class="flex">
                                               <asp:TextBox required="" runat="server" ID="txtActivityTariffType2" SkinID="txt_Hidden_Req_Auto" />
                                               <label>Tariff Sub-Group...</label>
                                           </div>
                                       </div>
                                      <div class="col m1 p0">
                                          <gap5></gap5>
                                           <asp:LinkButton runat="server" ID="lnkActivityRateTypeSearch" CssClass="btn btn-primary" OnClick="lnkActivityRateTypeSearch_Click">  <%= GetGlobalResourceObject("Resource", "Search")%> <%= MRLWMSC21Common.CommonLogic.btnfaSearch %> </asp:LinkButton>
                                           <asp:HiddenField runat="server" ID="hidActivityTariffGroup2" />
                                           <asp:LinkButton Visible="false" ID="lnkbtntariffsubgroup" runat="server" Font-Underline="False" CssClass="btn btn-primary" OnClick="lnkbtntariffsubgroup_Click"> <%= GetGlobalResourceObject("Resource", "AddNew")%><%=MRLWMSC21Common.CommonLogic.btnfaNew%></asp:LinkButton>
                                       </div>
                                   </div>
                               </td>
                           </tr>

                           <tr>
                               <td align="right" class="auto-style3">
                                   <%-- <asp:LinkButton ID="lnkAddnewActivityRateType" Visible="false" runat="server" Font-Underline="False"  OnClick="lnkAddnewActivityRateType_Click" SkinID="lnkButEmpty">Add New</asp:LinkButton>--%>
                                   <asp:LinkButton ID="lnkAddnewActivityRateType" Visible="false" runat="server" Font-Underline="False" OnClick="lnkAddnewActivityRateType_Click" SkinID="lnkButEmpty"><%= GetGlobalResourceObject("Resource", "AddNew")%></asp:LinkButton>

                               </td>
                           </tr>

                           <%-- <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" ID="upnlActivityRateType" UpdateMode="Always">
                            <ContentTemplate>--%>
                           <tr class="ListDataRow">

                               <td>

                                   <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" ID="upnlActivityRateType" UpdateMode="Always">
                                       <ContentTemplate>
                                           <asp:Label runat="server" ID="lblActivityrateType" CssClass="errorMsg" ForeColor="DarkGreen"></asp:Label>
                                     
                                           <asp:Panel runat="server" ID="pnlActivityRateType" HorizontalAlign="left">
                                               <asp:GridView ID="gvActivityRateType" runat="server" AutoGenerateColumns="false" ShowHeader="true" ShowHeaderWhenEmpty="true" AllowPaging="true" PageSize="25" SkinID="gvLightSeaBlue" CellPadding="4" Width="70%"
                                                   OnRowDataBound="gvActivityRateType_RowDataBound"
                                                   OnRowEditing="gvActivityRateType_RowEditing"
                                                   OnRowUpdating="gvActivityRateType_RowUpdating"
                                                   OnRowCancelingEdit="gvActivityRateType_RowCancelingEdit"
                                                   OnPageIndexChanging="gvActivityRateType_PageIndexChanging">
                                                   <Columns>

                                                       <%--<asp:TemplateField HeaderText="Tariff Sub-Group" ItemStyle-Width="150" HeaderStyle-HorizontalAlign="Center">--%>
                                                       <asp:TemplateField HeaderText="<%$Resources:Resource,TariffSubGroup%>" ItemStyle-Width="150" HeaderStyle-HorizontalAlign="Center">
                                                           <ItemTemplate>
                                                               <asp:Literal ID="ltActivityRateType" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ActivityRateType") %>' />
                                                           </ItemTemplate>
                                                           <EditItemTemplate>
                                                               <div class="gridInput">
                                                                   <asp:Literal ID="ltActivityRateTypeID" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ActivityRateTypeID") %>' />
                                                                   <asp:RequiredFieldValidator ID="rfvActivityRateType" runat="server" Display="Dynamic" ControlToValidate="txtActivityRateType" ValidationGroup="vRequiredActivityRateType" ErrorMessage="*" />
                                                                   <asp:TextBox ID="txtActivityRateType" ClientIDMode="Static" SkinID="txt_Auto_Req" Width="200" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ActivityRateType") %>' />
                                                                   <span class="errorMsg"></span>
                                                               </div>
                                                           </EditItemTemplate>
                                                       </asp:TemplateField>

                                                       <%-- <asp:TemplateField HeaderText="Service/Material" ItemStyle-Width="170" HeaderStyle-HorizontalAlign="Center">--%>
                                                       <asp:TemplateField HeaderText="<%$Resources:Resource,ServiceMaterial%>" ItemStyle-Width="170" HeaderStyle-HorizontalAlign="Center">
                                                           <ItemTemplate>
                                                               <asp:Literal ID="ltServiceType" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ServiceType") %>' />

                                                           </ItemTemplate>
                                                           <EditItemTemplate>
                                                               <div class="gridInput">
                                                                   <asp:Literal ID="ltServiceTypeID" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ServiceTypeID") %>' />
                                                                   <asp:RequiredFieldValidator ID="rfvServiceType" runat="server" Display="Dynamic" ControlToValidate="txtServiceType" ValidationGroup="vRequiredActivityRateType" ErrorMessage="*" />
                                                                   <asp:TextBox ID="txtServiceType" ClientIDMode="Static" SkinID="txt_Hidden_Req_Auto" Width="110" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ServiceType") %>' />
                                                                   <asp:HiddenField runat="server" ID="hifServiceType" ClientIDMode="Static" Value='<%# DataBinder.Eval(Container.DataItem, "ServiceTypeID").ToString() %>' />
                                                                   <span class="errorMsg"></span>
                                                               </div>
                                                           </EditItemTemplate>
                                                       </asp:TemplateField>

                                                       <%--<asp:TemplateField HeaderText="In-Out" ItemStyle-Width="100" HeaderStyle-HorizontalAlign="Center">--%>
                                                       <asp:TemplateField HeaderText="<%$Resources:Resource,InOut%>" ItemStyle-Width="100" HeaderStyle-HorizontalAlign="Center">
                                                           <ItemTemplate>
                                                               <asp:Literal ID="ltInOutType" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"InOutType") %>' />

                                                           </ItemTemplate>
                                                           <EditItemTemplate>
                                                               <div class="gridInput">
                                                                   <asp:Literal ID="ltInOutID" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"InOutID") %>' />
                                                                   <asp:RequiredFieldValidator ID="rfvInOutType" runat="server" Display="Dynamic" ControlToValidate="txtInOutType" ValidationGroup="vRequiredActivityRateType" ErrorMessage="*" />
                                                                   <asp:TextBox ID="txtInOutType" ClientIDMode="Static" SkinID="txt_Hidden_Req_Auto" Width="100" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"InOutType") %>' />
                                                                   <asp:HiddenField runat="server" ID="hifInOut" ClientIDMode="Static" Value='<%# DataBinder.Eval(Container.DataItem, "InOutID").ToString() %>' />
                                                                   <span class="errorMsg"></span>
                                                               </div>
                                                           </EditItemTemplate>
                                                       </asp:TemplateField>

                                                       <%--<asp:TemplateField HeaderText="Active" ItemStyle-Width="50" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">--%>
                                                       <asp:TemplateField Visible="false" HeaderText="<%$Resources:Resource,Active%>" ItemStyle-Width="50" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                           <ItemTemplate>
                                                               <asp:Literal runat="server" ID="ltIsActive" Text='<%# GetCheckValueOrDeleted(DataBinder.Eval(Container.DataItem, "IsActive").ToString()) %>' />
                                                           </ItemTemplate>

                                                           <EditItemTemplate>
                                                               <div class="pure-material-checkbox"><asp:CheckBox ID="chkActive" runat="server" Checked='<%# Convert.ToBoolean( Convert.ToInt16(DataBinder.Eval(Container.DataItem, "IsActive"))) %>' /><span></span></div>
                                                           </EditItemTemplate>
                                                       </asp:TemplateField>

                                                       <%--   <asp:TemplateField ItemStyle-Width="50" HeaderText="Delete" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint">

                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsDelete" runat="server" />
                                                </ItemTemplate>
                                                <EditItemTemplate></EditItemTemplate>

                                                <FooterTemplate>
                                                    <asp:LinkButton ID="lnkDeleteActivityTariffType" Font-Underline="false" CssClass="blueLink" runat="server" Text="<nobr> Delete <img src='../Images/redarrowright.gif' border='0' /></nobr>" OnClick="lnkDeleteActivityTariffType_Click" OnClientClick="return confirm('Are you sure you want to delete the selected line items?')" />
                                                </FooterTemplate>
                                            </asp:TemplateField>--%>

                                                       <asp:CommandField Visible="false" ValidationGroup="UpdateRevision" ItemStyle-Font-Underline="false" ControlStyle-Font-Underline="false" ItemStyle-Width="30" ButtonType="Link" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint" ItemStyle-HorizontalAlign="left" CancelImageUrl="Images/cancel.gif" EditImageUrl="/Images/edit.gif" EditText="<nobr><i class='material-icons ss'>mode_edit</i></nobr>" ShowEditButton="True" UpdateImageUrl="/Images/save.gif" />


                                                   </Columns>
                                                   <EmptyDataTemplate>
                                                       <%--	<div align="center">No Data Found</div>--%>
                                                       <div align="center"><%= GetGlobalResourceObject("Resource", "NoDataFound")%></div>
                                                   </EmptyDataTemplate>

                                               </asp:GridView>
                                           </asp:Panel>

                                       </ContentTemplate>
                                   </asp:UpdatePanel>
                               </td>
                           </tr>



                       </table>

                   </div>
               </div>
               
       <div id="accordion3" align="left" class="accordion">

        <%-- <h3>3. Tariffs </h3>--%>
            <h3><%= GetGlobalResourceObject("Resource", "Tariffs")%> </h3>
           <div align="center">

                       <asp:UpdatePanel runat="server" ChildrenAsTriggers="true" ID="upnlActivityRate" UpdateMode="Always">
                           <Triggers>
                               <asp:PostBackTrigger ControlID="lnkActivityRateSearch" />
                           </Triggers>
                           <ContentTemplate>
                               <div>

                                   

                                   <div class="row">
                                       <div class="col m12" colspan="1">
                                           <asp:Literal ID="ltActivityGroups" runat="server" Text="sample" />
                                           <asp:HiddenField ID="hifSelectedTabID" Value="1" runat="server" ClientIDMode="Static" />
                                           <%--<asp:LinkButton ID="lnkChangeTabs" runat="server" OnClick="lnkChangeTabs_Click" Visible="false" >Go</asp:LinkButton>--%>
                                           <asp:LinkButton ID="lnkChangeTabs" runat="server" OnClick="lnkChangeTabs_Click" Visible="false" > <%= GetGlobalResourceObject("Resource", "Go")%></asp:LinkButton>
                                       </div>
                                   </div>
                                   <div class="row">
                                       <div align="left" valign="top" colspan="12">
                                           <div class="">
                                               <div class="col m3">
                                                   <div class="flex"><asp:TextBox required="" runat="server" ID="txtWareHouse_Auto" SkinID="txt_Hidden_Req_Auto" />
                                                       <label>Ware House</label>
                                                   </div>
                                              </div>
                                               <div class="col m3">
                                                   <div class="flex">
                                                       <asp:TextBox runat="server" required="" ID="txtActivityTariffType3" SkinID="txt_Hidden_Req_Auto" />
                                                       <label>Tariff Sub-Group</label>
                                                   </div>
                                                   </div>
                                               <div class="col m3">
                                                   <div class="flex">
                                                       <asp:TextBox runat="server" required="" ID="txtActivityTariff3" SkinID="txt_Hidden_Req_Auto" />
                                                       <label>Tariff</label>
                                                   </div>
                                               </div>
                                               <div class="col m2">
                                                   <gap5></gap5>
                                                   <asp:LinkButton runat="server" ID="lnkActivityRateSearch" CssClass="btn btn-primary" OnClick="lnkActivityRateSearch_Click"><%= GetGlobalResourceObject("Resource", "Search")%> <%= MRLWMSC21Common.CommonLogic.btnfaSearch %> </asp:LinkButton>
                                                   <asp:HiddenField runat="server" ID="hidActivityTariffType3" />
                                                   <asp:HiddenField runat="server" ID="hidWareHouseID" />
                                                   <asp:LinkButton ID="lnkAddNewActivityRate" runat="server" OnClick="lnkAddNewActivityRate_Click" CssClass="btn btn-primary"> <%= GetGlobalResourceObject("Resource", "Add")%>  <%=MRLWMSC21Common.CommonLogic.btnfaNew%></asp:LinkButton>
                                               </div>
                                           </div>
                                       </div>
                                   </div>
                                 
                                   <div>
                                       <div align="right" colspan="3">
                                          

                                       </div>
                                   </div>
                                   
                                  
                                   

                                   <div>
                                       <div>
                                           <asp:Label runat="server" ID="lblActivityRate" CssClass="errorMsg" ForeColor="DarkGreen"></asp:Label>
                                       </div>
                                   </div>



                                   <div class="ListDataRow row">

                                       <div>

                                           <asp:Panel runat="server" ID="pnlActivityRate" HorizontalAlign="left" Width="" >
                                               <asp:GridView ID="gvActivityRate" runat="server" ShowHeader="true" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" AllowPaging="true" PageSize="25" SkinID="gvLightSeaBlue" CellPadding="4" Width="60%"
                                                   OnRowDataBound="gvActivityRate_RowDataBound"
                                                   OnRowEditing="gvActivityRate_RowEditing"
                                                   OnRowUpdating="gvActivityRate_RowUpdating"
                                                   OnRowCancelingEdit="gvActivityRate_RowCancelingEdit"
                                                   OnPageIndexChanging="gvActivityRate_PageIndexChanging"
                                                   OnRowCommand="gvActivityRate_RowCommand">
                                                   <Columns>

                                                       
                                                     <%--   <asp:TemplateField HeaderText="Ware House" ItemStyle-Width="80" HeaderStyle-HorizontalAlign="Center">--%>
                                                          <asp:TemplateField HeaderText="<%$Resources:Resource,WareHouse%>"  HeaderStyle-HorizontalAlign="Center">
                                                           <ItemTemplate>
                                                               <asp:Literal ID="ltWareHouse" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "WareHouse")%>' />
                                                           </ItemTemplate>
                                                           <EditItemTemplate>
                                                               <div class="gridInput">
                                                               <asp:RequiredFieldValidator ID="rfvwarehouse" runat="server" Display="Dynamic" ControlToValidate="txtWareHouse" ValidationGroup="vRequiredActivityRateType" ErrorMessage="*" />
                                                               <asp:TextBox ID="txtWareHouse" ClientIDMode="Static" SkinID="txt_Hidden_Req_Auto" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "WareHouse")%>' />
                                                               <asp:HiddenField runat="server" ID="hifwarehouse" ClientIDMode="Static" Value='<%# DataBinder.Eval(Container.DataItem, "WarehouseID").ToString()%>' />
                                                         <span class="errorMsg"></span>
                                                                   </div>
                                                                   </EditItemTemplate>
                                                       </asp:TemplateField>



                                                      <%-- <asp:TemplateField HeaderText="Tariff Sub-Group" ItemStyle-Width="80" HeaderStyle-HorizontalAlign="Center">--%>
                                                        <asp:TemplateField HeaderText="<%$Resources:Resource,TariffSubGroup%>"  HeaderStyle-HorizontalAlign="Center">
                                                           <ItemTemplate>
                                                               <asp:Literal ID="ltARActivityRateType" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ActivityRateType")%>' />
                                                           </ItemTemplate>
                                                           <EditItemTemplate>
                                                               <div class="gridInput">
                                                               <asp:RequiredFieldValidator ID="rfvActivityRateType" runat="server" Display="Dynamic" ControlToValidate="txtARActivityRateType" ValidationGroup="vRequiredActivityRateType" ErrorMessage="*" />
                                                               <asp:TextBox ID="txtARActivityRateType" ClientIDMode="Static" SkinID="txt_Hidden_Req_Auto" Width="190" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ActivityRateType")%>' />
                                                               <asp:HiddenField runat="server" ID="hifARActivityRateType" ClientIDMode="Static" Value='<%# DataBinder.Eval(Container.DataItem, "ActivityRateTypeID").ToString()%>' />
                                                           <span class="errorMsg"></span>
                                                                   </div>
                                                                   </EditItemTemplate>
                                                       </asp:TemplateField>

                                                     <%--  <asp:TemplateField HeaderText="Tariff " ItemStyle-Width="80" HeaderStyle-HorizontalAlign="Center">--%>
                                                         <asp:TemplateField HeaderText="<%$Resources:Resource,Tariff%>"  HeaderStyle-HorizontalAlign="Center">
                                                           <ItemTemplate>
                                                               <asp:Literal ID="ltActivityRateName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ActivityRateName")%>' />
                                                               <asp:Literal ID="ltActivityRateID" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ActivityRateID")%>' />
                                                           </ItemTemplate>
                                                           <EditItemTemplate>
                                                                <div class="gridInput">
                                                               <asp:Literal ID="ltActivityRateID" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ActivityRateID")%>' />
                                                               <asp:RequiredFieldValidator ID="rfvActivityRateName" runat="server" Display="Dynamic" ControlToValidate="txtActivityRateName" ValidationGroup="vRequiredActivityRate" ErrorMessage="*" />
                                                               <asp:TextBox ID="txtActivityRateName" ClientIDMode="Static" SkinID="txt_Auto_Req" runat="server"  Text='<%#DataBinder.Eval(Container.DataItem, "ActivityRateName")%>' />
                                                            <span class="errorMsg"></span>
                                                                   </div>
                                                           </EditItemTemplate>
                                                       </asp:TemplateField>


                                                       <%--<asp:TemplateField HeaderText="UoM" ItemStyle-Width="40" HeaderStyle-HorizontalAlign="Center">--%>
                                                       <asp:TemplateField HeaderText="<%$Resources:Resource,UoM%>"   HeaderStyle-HorizontalAlign="Center">
                                                           <ItemTemplate>
                                                               <asp:Literal ID="ltUoM" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "UoM")%>' />

                                                           </ItemTemplate>
                                                           <EditItemTemplate>
                                                                <div class="gridInput">
                                                               <asp:RequiredFieldValidator ID="rfvUoM" runat="server" Display="Dynamic" ControlToValidate="txtUoM" ValidationGroup="vRequiredActivityRate" ErrorMessage="*" />
                                                               <asp:TextBox ID="txtUoM" ClientIDMode="Static" SkinID="txt_Hidden_Req_Auto" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "UoM")%>' />
                                                               <asp:HiddenField runat="server" ID="hifUoMID" ClientIDMode="Static" Value='<%# DataBinder.Eval(Container.DataItem, "UoMID").ToString()%>' />
                                                           <span class="errorMsg"></span>
                                                                   </div>
                                                                    </EditItemTemplate>
                                                       </asp:TemplateField>

                                                     <%--  <asp:TemplateField HeaderText="Unit Cost" ItemStyle-Width="50" HeaderStyle-HorizontalAlign="Center">--%>
                                                         <asp:TemplateField HeaderText="<%$Resources:Resource,UnitCost%>" HeaderStyle-HorizontalAlign="Center">
                                                           <ItemTemplate>
                                                               <asp:Literal ID="ltUnitCost" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "UnitCost")%>' />

                                                           </ItemTemplate>
                                                           <EditItemTemplate>
                                                                <div class="gridInput">
                                                               <asp:RequiredFieldValidator ID="rfvUnitCost" runat="server" Display="Dynamic" ControlToValidate="txtUnitCost" ValidationGroup="vRequiredActivityRate" ErrorMessage="*" />
                                                               <asp:TextBox ID="txtUnitCost" ClientIDMode="Static" SkinID="txt_Hidden_Req_Auto" onKeyPress="return checkDec(this,event)" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "UnitCost")%>' />
                                                            <span class="errorMsg"></span>
                                                                   </div>
                                                           </EditItemTemplate>
                                                       </asp:TemplateField>

                                                     <%--  <asp:TemplateField HeaderText="Cost Price" ItemStyle-Width="50" HeaderStyle-HorizontalAlign="Center">--%>
                                                         <asp:TemplateField Visible="false" HeaderText="<%$Resources:Resource,CostPrice%>"  ItemStyle-Width="50" HeaderStyle-HorizontalAlign="Center">
                                                           <ItemTemplate>
                                                               <asp:Literal ID="ltCostPrice" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "CostPrice")%>' />

                                                           </ItemTemplate>
                                                           <EditItemTemplate>
                                                                <div class="gridInput">
                                                               <asp:RequiredFieldValidator ID="rfvCostPrice" runat="server" Display="Dynamic" ControlToValidate="txtCostPrice" ValidationGroup="vRequiredActivityRate" ErrorMessage="*" />
                                                               <asp:TextBox ID="txtCostPrice" ClientIDMode="Static" SkinID="txt_Hidden_Req_Auto" onKeyPress="return checkDec(this,event)" runat="server" Text="0" />
                                                             <span class="errorMsg"></span>
                                                                   </div>
                                                                    </EditItemTemplate>
                                                       </asp:TemplateField>
                                                     <%--  <asp:TemplateField HeaderText="HSN/SAC Code" ItemStyle-Width="50" HeaderStyle-HorizontalAlign="Center">--%>
                                                         <asp:TemplateField HeaderText="<%$Resources:Resource,HSNSACCode%>" ItemStyle-Width="50" HeaderStyle-HorizontalAlign="Center">
                                                           <ItemTemplate>
                                                               <asp:Literal ID="lthsnsaccode" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "HSNSACCode")%>' />

                                                           </ItemTemplate>
                                                           <EditItemTemplate>
                                                               <div class="gridInput">
                                                               <%--<asp:RequiredFieldValidator ID="rfvCostPrice" runat="server" Display="Dynamic" ControlToValidate="txtCostPrice" ValidationGroup="vRequiredActivityRate" ErrorMessage="*" />--%>
                                                               <asp:TextBox ID="txthsnsaccode" ClientIDMode="Static" SkinID="txt_Hidden_Req_Auto"  runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "HSNSACCode")%>' />
                                                           </div>
                                                                   </EditItemTemplate>
                                                       </asp:TemplateField>

                                                     <%--  <asp:TemplateField HeaderText="CGST" ItemStyle-Width="50" HeaderStyle-HorizontalAlign="Center">--%>
                                                         <asp:TemplateField HeaderText="<%$Resources:Resource,CGST%>" HeaderStyle-HorizontalAlign="Center">
                                                           <ItemTemplate>
                                                               <asp:Literal ID="ltCGST" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "CGST")%>' />

                                                           </ItemTemplate>
                                                           <EditItemTemplate>
                                                               <div class="gridInput">
                                                              <%-- <asp:RequiredFieldValidator ID="rfvCostPrice" runat="server" Display="Dynamic" ControlToValidate="txtCGST" ValidationGroup="vRequiredActivityRate" ErrorMessage="*" />--%>
                                                               <asp:TextBox ID="txtCGST" ClientIDMode="Static" SkinID="txt_Hidden_Req_Auto" onKeyPress="return checkDec(this,event)" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "CGST")%>' />
                                                          </div>
                                                                   </EditItemTemplate>
                                                       </asp:TemplateField>

                                                      <%-- <asp:TemplateField HeaderText="SGST" ItemStyle-Width="50" HeaderStyle-HorizontalAlign="Center">--%>
                                                        <asp:TemplateField HeaderText="<%$Resources:Resource,SGST%>" HeaderStyle-HorizontalAlign="Center">
                                                           <ItemTemplate>
                                                               <asp:Literal ID="ltSGST" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "SGST")%>' />

                                                           </ItemTemplate>
                                                           <EditItemTemplate>
                                                               <div class="gridInput">
                                                              <%-- <asp:RequiredFieldValidator ID="rfvCostPrice" runat="server" Display="Dynamic" ControlToValidate="txtCGST" ValidationGroup="vRequiredActivityRate" ErrorMessage="*" />--%>
                                                               <asp:TextBox ID="txtSGST" ClientIDMode="Static" SkinID="txt_Hidden_Req_Auto" onKeyPress="return checkDec(this,event)" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "SGST")%>' />
                                                          </div>
                                                                   </EditItemTemplate>
                                                       </asp:TemplateField>

                                                     <%--   <asp:TemplateField HeaderText="IGST" ItemStyle-Width="50" HeaderStyle-HorizontalAlign="Center">--%>
                                                          <asp:TemplateField HeaderText="<%$Resources:Resource,IGST%>" ItemStyle-Width="50" HeaderStyle-HorizontalAlign="Center">
                                                           <ItemTemplate>
                                                               <asp:Literal ID="ltIGST" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "IGST")%>' />

                                                           </ItemTemplate>
                                                           <EditItemTemplate>
                                                               <div class="gridInput">
                                                              <%-- <asp:RequiredFieldValidator ID="rfvCostPrice" runat="server" Display="Dynamic" ControlToValidate="txtCGST" ValidationGroup="vRequiredActivityRate" ErrorMessage="*" />--%>
                                                               <asp:TextBox ID="txtIGST" ClientIDMode="Static" SkinID="txt_Hidden_Req_Auto" onKeyPress="return checkDec(this,event)" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "IGST")%>' />
                                                          </div>
                                                                   </EditItemTemplate>
                                                       </asp:TemplateField>


                                                       <%--   <asp:TemplateField HeaderText="CESS" ItemStyle-Width="50" HeaderStyle-HorizontalAlign="Center">--%>
                                                          <asp:TemplateField HeaderText="<%$Resources:Resource,CESS%>"  HeaderStyle-HorizontalAlign="Center">
                                                           <ItemTemplate>
                                                               <asp:Literal ID="ltCess" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Cess")%>' />

                                                           </ItemTemplate>
                                                           <EditItemTemplate>
                                                               <div class="gridInput">
                                                              <%-- <asp:RequiredFieldValidator ID="rfvCostPrice" runat="server" Display="Dynamic" ControlToValidate="txtCGST" ValidationGroup="vRequiredActivityRate" ErrorMessage="*" />--%>
                                                               <asp:TextBox ID="txtCess" ClientIDMode="Static" SkinID="txt_Hidden_Req_Auto" onKeyPress="return checkDec(this,event)" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Cess")%>' />
                                                          </div>
                                                                   </EditItemTemplate>
                                                       </asp:TemplateField>


                                                       <%--<asp:TemplateField HeaderText="Currency" ItemStyle-Width="80" HeaderStyle-HorizontalAlign="Center">--%>
                                                       <asp:TemplateField HeaderText="<%$Resources:Resource,Currency%>"  HeaderStyle-HorizontalAlign="Center">
                                                           <ItemTemplate>
                                                               <asp:Literal ID="ltCurrency" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Code")%>' />

                                                           </ItemTemplate>
                                                           <EditItemTemplate>
                                                               <div class="gridInput">
                                                               <asp:RequiredFieldValidator ID="rfvCurrency" runat="server" Display="Dynamic" ControlToValidate="txtCurrency" ValidationGroup="vRequiredActivityRate" ErrorMessage="*" />
                                                               <asp:TextBox ID="txtCurrency" ClientIDMode="Static" SkinID="txt_Hidden_Req_Auto" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Code")%>' />
                                                               <asp:HiddenField runat="server" ID="hifCurrency" ClientIDMode="Static" Value='<%# DataBinder.Eval(Container.DataItem, "CurrencyID").ToString()%>' />
                                                          <span class="errorMsg"></span>
                                                                   </div>
                                                                   </EditItemTemplate>

                                                       </asp:TemplateField>

                                                       <%--<asp:TemplateField HeaderText="Description" ItemStyle-Width="350" HeaderStyle-HorizontalAlign="Center">--%>
                                                       <asp:TemplateField HeaderText="<%$Resources:Resource,Description%>" HeaderStyle-HorizontalAlign="Center">
                                                           <ItemTemplate>
                                                               <asp:Literal ID="ltActivityRateDescription" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ActivityRateDescription")%>' />

                                                           </ItemTemplate>
                                                           <EditItemTemplate>
                                                               <div class="gridInput">
                                                               <asp:TextBox ID="txtActivityRateDescription" ClientIDMode="Static" SkinID="txt_Auto_Req" runat="server" Width="110" Text='<%#DataBinder.Eval(Container.DataItem, "ActivityRateDescription")%>' />
                                                          </div>
                                                                   </EditItemTemplate>

                                                       </asp:TemplateField>

                                                      <%-- <asp:TemplateField HeaderText="Effective From" ItemStyle-Width="100" HeaderStyle-HorizontalAlign="Center">--%>
                                                        <asp:TemplateField HeaderText="<%$Resources:Resource,EffectiveFrom%>"  HeaderStyle-HorizontalAlign="Center">
                                                           <ItemTemplate>
                                                               <asp:Literal ID="ltServiceType" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "EffectiveFrom", "{0:dd/MM/yyyy}").ToString()%>'></asp:Literal>

                                                           </ItemTemplate>
                                                           <EditItemTemplate>
                                                               <div class="gridInput">
                                                               <asp:RequiredFieldValidator ID="rfvEffectiveFrom" runat="server" Display="Dynamic" ControlToValidate="txtEffectiveFrom" ValidationGroup="vRequiredActivityRate" ErrorMessage="*" />
                                                               <asp:TextBox ID="txtEffectiveFrom" ClientIDMode="Static" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "EffectiveFrom", "{0:dd/MM/yyyy}").ToString()%>' />
                                                               <asp:HiddenField runat="server" ID="hifEffectiveDate" ClientIDMode="Static" Value='<%# DataBinder.Eval(Container.DataItem, "EffectiveFrom", "{0:dd/MM/yyyy}").ToString()%>' />
                                                         
                                                                   <span class="errorMsg"></span>
                                                                   </div>

                                                           </EditItemTemplate>

                                                       </asp:TemplateField>

                                                     <%--  <asp:TemplateField HeaderText="Effective To" ItemStyle-Width="100" HeaderStyle-HorizontalAlign="Center">
                                                           <ItemTemplate>
                                                               <asp:Literal ID="ltEffectiveTo" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "EffectiveTo", "{0:dd/MM/yyyy}").ToString()%>'></asp:Literal>

                                                           </ItemTemplate>
                                                           <EditItemTemplate>
                                                               <asp:RequiredFieldValidator ID="rfvEffectiveTo" runat="server" Display="Dynamic" ControlToValidate="txtEffectiveTo" ValidationGroup="vRequiredActivityRate" ErrorMessage="*" />
                                                               <asp:TextBox ID="txtEffectiveTo" ClientIDMode="Static" runat="server" Width="90" Text='<%#DataBinder.Eval(Container.DataItem, "EffectiveTo", "{0:dd/MM/yyyy}").ToString()%>' />
                                                           </EditItemTemplate>

                                                       </asp:TemplateField>--%>
                                                      
                                                      <%-- <asp:TemplateField HeaderText="Is Default Rate" ItemStyle-Width="30" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">--%>
                                                        <asp:TemplateField HeaderText="<%$Resources:Resource,IsDefaultRate%>"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                     <ItemTemplate>
                                                            <asp:Literal runat="server" ID="lnkIsDefaultRate" Text='<%# GetCheckValueOrDeleted(DataBinder.Eval(Container.DataItem, "IsDefaultRate").ToString()) %>'/>
                                                        </ItemTemplate>
                                                       
                                                        <EditItemTemplate>
                                                           <asp:CheckBox ID="chkIsDefaultRate" runat="server" Checked='<%# Convert.ToBoolean( Convert.ToInt16(DataBinder.Eval(Container.DataItem, "IsDefaultRate")))%>' />
                                                        </EditItemTemplate>
                                                   </asp:TemplateField>

                                                     <%-- <asp:TemplateField HeaderText="Active" ItemStyle-Width="30" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">--%>
                                                        <asp:TemplateField HeaderText="<%$Resources:Resource,Active%>" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                     <ItemTemplate>
                                                            <asp:Literal runat="server" ID="ltIsActive" Text='<%# GetCheckValueOrDeleted(DataBinder.Eval(Container.DataItem, "IsActive").ToString()) %>'/>
                                                        </ItemTemplate>
                                                           <EditItemTemplate>
                                                           <div class="pure-material-checkbox"><asp:CheckBox ID="chkActive" runat="server" Checked='<%# Convert.ToBoolean( Convert.ToInt16(DataBinder.Eval(Container.DataItem, "IsActive"))) %>' /><span></span></div>
                                                        </EditItemTemplate>
                                                   </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center">

                                                            <ItemTemplate>
                                                                <label class="pure-material-checkbox"><asp:CheckBox ID="chkIsDelete" runat="server"  /><span></span></label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate></EditItemTemplate>

                                                            <FooterTemplate>
                                                                <asp:LinkButton ID="lnkDeleteActivityRate" Font-Underline="false" CssClass="blueLink" runat="server" Text="<nobr><i class='material-icons'>delete</nobr>" OnClick="lnkDeleteActivityRate_Click" OnClientClick="return confirm('Are you sure you want to delete the selected line items?')" />
                                                            </FooterTemplate>
                                                        </asp:TemplateField>

                                                       <asp:TemplateField  ItemStyle-HorizontalAlign="Center">
                                                     <ItemTemplate>

                                                         <asp:LinkButton Font-Underline="false" ID="lnkTenantTariff" runat="server"  Text="Tenant Info"   CommandName="TenantTariff" CommandArgument='<%#string.Format("{0}",DataBinder.Eval(Container.DataItem, "ActivityRateID")) %> '  />
                                                         <img src='../Images/redarrowright.gif' border='0' />
                                                         
                                                     </ItemTemplate>

                                                      <EditItemTemplate>
                                                          <asp:LinkButton Font-Underline="false" ID="lnkTenantTariff" runat="server"  Visible="false"  Text="Tenant Info"  CommandName="TenantTariff" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ActivityRateID") %> ' />
                                                         <img src='../Images/redarrowright.gif' border='0' style="display:none;" />
                                                      </EditItemTemplate>

                                                  </asp:TemplateField> 
                                                       <asp:TemplateField>
                                                           <ItemTemplate>
                                                               <asp:LinkButton ID="lnkActivityRateEdit" ValidationGroup="UpdateRevision" runat="server" OnClick="lnkActivityRateEdit_Click" Font-Underline="false" Text="<i class='material-icons ss'>mode_edit</i> " />
                                                           </ItemTemplate>
                                                           <EditItemTemplate>
                                                               <nobar class="sm-btn"><asp:LinkButton ID="lnkActivityRateUpdate" Font-Underline="false" runat="server" OnClick="lnkActivityRateUpdate_Click"  Text="Update"  ValidationGroup="UpdateRevision" /><p></p>
                                                               <asp:LinkButton ID="lnkActivityRateCancel" Font-Underline="false" runat="server" OnClick="lnkActivityRateCancel_Click"  Text="Cancel" /></nobar>
                                                           </EditItemTemplate>
                                                       </asp:TemplateField>
                                                       
                                                       <%--<asp:CommandField ValidationGroup="UpdateRevision" ItemStyle-Font-Underline="false" ControlStyle-Font-Underline="false" ItemStyle-Width="30" ButtonType="Link" ItemStyle-CssClass="NoPrint" ControlStyle-CssClass="NoPrint" HeaderStyle-CssClass="NoPrint" FooterStyle-CssClass="NoPrint" ItemStyle-HorizontalAlign="left" CancelImageUrl="Images/cancel.gif" EditImageUrl="/Images/edit.gif" EditText="<nobr> Edit <img src='../Images/redarrowright.gif' border='0' /></nobr>" ShowEditButton="True" UpdateImageUrl="/Images/save.gif" />--%>


                                                   </Columns>
                                                   <EmptyDataTemplate>
													<%--<div align="center">No Data Found</div>--%>
                                                       <div align="center"> <%= GetGlobalResourceObject("Resource", "NoDataFound")%></div>
												</EmptyDataTemplate>
                                               </asp:GridView>
                                           </asp:Panel>

                                       </div>
                                   </div>



                               </div>
                           </ContentTemplate>
                       </asp:UpdatePanel>
                   </div>
                    </div>
                </div>
             </div>

        </div>
    </div>
    <asp:HiddenField ID="hidAccordionIndex1" runat="server" />
    <asp:HiddenField ID="hidAccordionIndex2" runat="server" />
    <asp:HiddenField ID="hidAccordionIndex3" runat="server" />

     <asp:PlaceHolder ID="dontCare" runat="server">

        <script type="text/javascript" language="javascript">
            $(function () {

                $("#txtEffectiveFrom").datepicker({ dateFormat: 'dd/mm/yy' });
                $("#txtEffectiveTo").datepicker({ dateFormat: 'dd/mm/yy' });

            });



        </script>
    </asp:PlaceHolder>

     </div>

</asp:Content>
