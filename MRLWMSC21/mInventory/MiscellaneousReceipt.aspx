<%@ Page Title="" Language="C#" MasterPageFile="~/mInventory/InventoryMaster.master" AutoEventWireup="true" CodeBehind="MiscellaneousReceipt.aspx.cs" Inherits="MRLWMSC21.mInventory.MiscellaneousReceipt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="InvContent" runat="server">

    <asp:ScriptManager ID="mySManager" runat="server" EnablePartialRendering="true" SupportsPartialRendering="true" />
    <script type="text/javascript" src="Scripts/CommonScripts.js"></script>

    <script>
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                fnLoadAutocompletes();
            }
        }
        fnLoadAutocompletes();
        
                       

        function fnLoadAutocompletes() {
            $(document).ready(function () {
                $("#Loading").hide();
                $("#txtMfgDate").datepicker({
                    dateFormat: "dd-M-yy",
                    maxDate: new Date(),
                    onSelect: function (selected) {
                        var instance = $(this).data("datepicker");
                        var date = $.datepicker.parseDate(instance.settings.dateFormat || $.datepicker._defaults.dateFormat, selected, instance.settings);
                        date.setDate(date.getDate() + 1);
                        $("#txtexpdate").datepicker("option", "minDate", date, { dateFormat: "dd-M-yy" })
                    }
                });

                $("#txtexpdate").datepicker({
                    dateFormat: "dd-M-yy",
                    //maxDate: new Date()
                });

                $('#<%=this.txtQuantity.ClientID%>').focusout(function () {
                    if (parseFloat($('#<%=this.txtQuantity.ClientID%>').val()) == 0) {
                        $('#<%=this.txtQuantity.ClientID%>').val('');
                        showStickyToast(false, 'Receive quantity cannot be zero');
                    }
                })



                $('.DateBoxCSS_small').datepicker({ dateFormat: 'dd/mm/yy' });

                debugger;

                $('#<%=this.chkDamgae.ClientID%>').click(function () {
                    //alert('demo');
                    //if ($('#<%=this.chkDamgae.ClientID%>').is(":checked")) {
                    $('#<%=this.txtLocation.ClientID%>').val('');


                    //}

                });
                $('#<%=this.chkDiscrepancy.ClientID%>').click(function () {

                    //if ($('#<%=this.chkDiscrepancy.ClientID%>').is(":checked"))
                    $('#<%=this.txtLocation.ClientID%>').val('');


                });


                var textfieldname = $("#<%= this.txtLocation.ClientID %>");
                DropdownFunction(textfieldname);
                $('#<%=this.txtLocation.ClientID%>').autocomplete({

                    source: function (request, response) {

                        var Damage = 0;<%-- document.getElementById('<%=this.chkDamgae.ClientID%>').checked ? "1" : "0";;--%>
                    var Discrepancy = 0;<%--document.getElementById('<%=this.chkDiscrepancy.ClientID%>').checked ? "1" : "0";;--%>

                    if (Damage == 1 || Discrepancy == 1) {

                        $("#<%=hifIsQuarantine.ClientID%>").val("1");
                    }
                    else {
                        $("#<%=hifIsQuarantine.ClientID%>").val("0");
                    }

                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetAllLocationsUnderWarehouse") %>',
                       <%-- data: "{'Prefix': '" + request.term + "','materialID':'" + '<%=MerlinCommon.CommonLogic.QueryString("mid")%>' + "','TenantID':'" + $("#<%=hifTenant.ClientID%>").val() + "'}",--%>

                            data: "{'Prefix': '" + request.term + "','MMID':'" + $("#<%=hifMaterialId.ClientID%>").val() + "','WHID':'" + $("#<%=hifWarehouseId.ClientID%>").val() + "'}",
                            dataType: "json",
                            type: "POST",
                            async: true,
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                    debugger;
                               // response(data.d)

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
                            
                        var Location = $("#<%= this.txtLocation.ClientID %>").val();
                    // alert(Location);
                    var textfieldname = $("#<%= this.txtContainerCode.ClientID %>");
                    //if (Location == undefined || Location == "" || Location == null) {
                    //    showStickyToast(false, 'Please select location', false);
                    //    return;
                    //}
                    DropdownFunction(textfieldname);
                    $('#<%=this.txtContainerCode.ClientID%>').autocomplete({

                        source: function (request, response) {
                            $.ajax({
                                url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetContainers_Tenant") %>',
                                        data: "{'Prefix': '" + request.term + "', 'TenantID': '" + $("#<%=hifTenant.ClientID%>").val() + "', 'Location':'" + Location + "','WarehouseId':"+$("#<%=hifWarehouseId.ClientID %>").val()+"}",
                                            dataType: "json",
                                            type: "POST",
                                            async: true,
                                            contentType: "application/json; charset=utf-8",
                                            success: function (data) {
                                                if (data.d == "" || data.d == "/") {
                                                    showStickyToast(false, 'No Pallets');
                                                    return;
                                                }
                                                //else response(data.d)
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

                                        $("#<%=hifContainercode.ClientID %>").val(i.item.val);
                            },
                            minLength: 0
                        });

                    },

                    minLength: 0
                });





                var textfieldname = $("#<%= this.txtTenant.ClientID %>");
                DropdownFunction(textfieldname);
                $("#<%= this.txtTenant.ClientID %>").autocomplete({
                    source: function (request, response) {
                        debugger;
                        $.ajax({
                          //  url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadTenantDataFor3PL") %>',
                           // data: "{ 'prefix': '" + request.term + "'}",
                        url: '../mWebServices/FalconWebService.asmx/LoadTenantsByWH',  // added by Ganesh @Sep 29 2020 --- tenant drop down data should be displyed by UserWH
                        data: "{ 'prefix': '" + request.term + "','whid':'"+$("#<%=hifWarehouseId.ClientID %>").val()+"' }",
                      
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
                        debugger;

                        $("#<%=hifTenant.ClientID %>").val(i.item.val);
                    //alert($("#<%=hifTenant.ClientID %>").val());
                        $('#<%=this.atcMateialCode.ClientID%>').val("");
                        $("#<%=this.hifMaterialId.ClientID%>").val(0);
                       // $("#<%= this.txtWH.ClientID %>").val("");
                 //$("#<%=hifWarehouseId.ClientID %>").val(0);
                    },
                    minLength: 0
                });

                var textfieldname = $('#<%=this.atcMateialCode.ClientID%>')
                DropdownFunction(textfieldname);
                $('#<%=this.atcMateialCode.ClientID%>').autocomplete({
                    source: function (request, response) {
                        //alert($("#<%=hifTenant.ClientID %>").val());
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetMaterialForMiscReceipt") %>',
                                 //data: "{ 'prefix': '" + request.term + "','TenantID':'" +<%=cp.TenantID%> +"'}",
                            data: "{ 'prefix': '" + request.term + "','TenantID':'" + $("#<%=hifTenant.ClientID %>").val() + "'}",//<=cp.TenantID%>
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
                        debugger;
                        $("#<%=this.hifMaterialId.ClientID%>").val(i.item.val);
                             $("#<%=this.atcMateialCode.ClientID%>").val(i.item.label);
                            <%-- var btnID = i.item.val + '&mcode' + i.item.label;//'<%=lnkGetMaterialDetails.ClientID %>';
                             var params = 'materialDetails';
                             __doPostBack(btnID, params);--%>
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


                var TextFieldName = $("#<%= this.txtWH.ClientID %>");
                DropdownFunction(TextFieldName);
                $("#<%= this.txtWH.ClientID %>").autocomplete({
                    source: function (request, response) {
                        debugger;
                        $.ajax({
                           // url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadWarehouseData") %>',
                          //  data: "{ 'prefix': '" + request.term + "', 'TenantID':'"+ $("#<%=hifTenant.ClientID %>").val()+"'}",//<=cp.TenantID%>
                             url: '../mWebServices/FalconWebService.asmx/LoaDWHListBasedonUser',
                            data: "{ 'prefix': '" + request.term + "'  }",
                      
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
                        //debugger;
                         //Warehouseid = i.item.val;
                        //$("#hifWarehouseId").val(i.item.val);
                       $("#<%=hifWarehouseId.ClientID %>").val(i.item.val);
                      
                        $('#<%=this.atcMateialCode.ClientID%>').val("");
                        $("#<%= this.txtTenant.ClientID %>").val("");

                    },
                    minLength: 0
                });


            });


        }
  

        function getvalidate() {
            debugger;
            var materialcode = $('#<%=this.atcMateialCode.ClientID%>').val();
            var tenantgcode = $('#<%=this.txtTenant.ClientID%>').val();
            var whcode = $('#<%=this.txtWH.ClientID%>').val();
            if (whcode == "")
            {
                showStickyToast(false, 'Please Check For Mandatory Fields');
                return false;
            }
            if (materialcode == "" || tenantgcode == "") {
                showStickyToast(false, 'Please Check For Mandatory Fields');
               
                $('#<%=ltDescription.ClientID%>').val()="";
                return;
         }
        }
      function CheckDigit(event, textbox) {

            var x = event.which || event.keyCode;
            if (!((x == 190) || (x > 47 && x < 58)||(x >= 96 && x <= 106))) {
                textbox.value = "";
            } else {
                if (!(textbox.value.split('.').length <= 2 && textbox.value.split('.')[0] != '')) {
                    textbox.value = "";
                }


            }
            console.log(x);      
        }
        function Openloader() {
            debugger;
            $("#Loading").show();
        }
    </script>
   <style>

       .row .col.offset-m5 {
           margin-left: 15.666667%;
       }
   </style>


    <div  id="Loading" style="width:100%; height:100%; z-index:999; position:fixed; top:0; left:0; right:0; bottom:0; align-items:center; display:none;display:flex; justify-content:center; background: #e0ddd8ba; ">
                                        <div style="align-self:center;" >
                                            <div class="spinner">
                                                <div class="bounce1"></div>
                                                <div class="bounce2"></div>
                                                <div class="bounce3"></div>
                                            </div>

                                        </div>                                  
                                    </div>

    <asp:UpdatePanel ID="upnlReceipt" runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hifIsQuarantine" Value="0" />
            <div class="container">

                <div class="row">
                    <div>
                        <div class="col m3 offset-m5 offset-s5">
                             
                            <div class="flex">
                                <asp:TextBox ID="txtWH" runat="server" required="" Style="margin-bottom: 0px !important;"></asp:TextBox>                                
                                <asp:HiddenField ID="hifWarehouseId" runat="server" ClientIDMode="Static" />

                                <label><%= GetGlobalResourceObject("Resource", "WareHouse")%></label>
                                <span class="errorMsg"></span>
                            </div>
                        </div>
                        <div class="col m3 ">
                            <div class="flex">
                                <asp:TextBox ID="txtTenant" runat="server" required="" Style="margin-bottom: 0px !important;"></asp:TextBox>
                                <asp:HiddenField ID="hifTenant" runat="server" />
                                <label><%= GetGlobalResourceObject("Resource", "Tenant")%></label>
                                <span class="errorMsg"></span>
                            </div>
                        </div>
                       
                        <div class="col m3">
                            <div class="flex">
                                <asp:TextBox ID="atcMateialCode" runat="server" SkinID="txt_Auto" required="" />
                                <asp:RequiredFieldValidator ID="rfvMaterilas" runat="server" ControlToValidate="atcMateialCode" ErrorMessage="*" Display="Dynamic" ValidationGroup="receiveQuantity" />
                                <label><%= GetGlobalResourceObject("Resource", "Material")%> </label>
                                <asp:HiddenField ID="hifMaterialId" runat="server" />
                            </div>
                        </div>
                        <div class="col m1">
                            <gap5></gap5>
                            <asp:LinkButton ID="lnkGetMaterialDetails" runat="server" OnClientClick="Openloader(); getvalidate();" OnClick="lnkGetMaterialDetails_Click" CssClass="btn btn-primary">Search <i class="material-icons">search</i></asp:LinkButton>
                        </div>
                    </div>
                </div>
                <table class="" cellpadding="5" cellspacing="5">

                    <tr id="trDetails" runat="server" style="display: none">
                        <td>
                            <asp:Label CssClass="FormLabelsBlue" runat="server" ID="lblDescription" Text="Description :" />
                            <asp:Literal ID="ltDescription" runat="server" />
                        </td>
                        <td>
                            <asp:Label CssClass="FormLabelsBlue" runat="server" ID="Label1" Text="BUoM/Qty. :" />
                            <asp:Literal ID="ltBaseUOM" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table id="tbMaterialStorageparameter" runat="server" border="0" cellspacing="5" cellpadding="5" width="100%">
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">


                            <table id="tbdetail" runat="server" visible="false" border="0" cellspacing="5" cellpadding="5" width="100%">
                                <tr>
                                    <td>
                                        <div class="row">
                                            <div class="col m6">
                                                <div class="">
                                                    <div class="checkbox" style="display: none;">
                                                        <asp:CheckBox ID="chkDiscrepancy" runat="server" /><%--<label for="MainContent_InvContent_chkDiscrepancy">Has Discrepancy</label>--%>
                                                        <label for="MainContent_InvContent_chkDiscrepancy"><%= GetGlobalResourceObject("Resource", "HasDiscrepancy")%></label>
                                                    </div>
                                                    &nbsp;&nbsp;&nbsp;<div class="checkbox">
                                                        <asp:CheckBox ID="chkDamgae" Visible="false" runat="server" /><%--<label style="display:none;" for="MainContent_InvContent_chkDamgae">Is Damaged</label>--%>
                                                        <label style="display: none;" for="MainContent_InvContent_chkDamgae"><%= GetGlobalResourceObject("Resource", "IsDamaged")%> </label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col m3 s3">
                                                <div class="flex">
                                                    <asp:TextBox ID="txtQuantity" runat="server" onKeyPress="return checkDec(this,event)" required="" />
                                                    <%--  <label>Quantity Received</label>--%>
                                                    <label><%= GetGlobalResourceObject("Resource", "QuantityReceived")%></label>
                                                    <span class="errorMsg"></span>
                                                    <asp:RequiredFieldValidator ID="rfvquantity" runat="server" ControlToValidate="txtQuantity" ErrorMessage="*" Display="Dynamic" ValidationGroup="receiveQuantity" />
                                                </div>
                                            </div>


                                            <div class="col m3 s3">
                                                <div class="flex">
                                                    <asp:TextBox ID="txtLocation" SkinID="txt_Auto" onKeypress="return checkSpecialChar(event)" runat="server" required="" />
                                                    <asp:RequiredFieldValidator ID="rfvLocation" runat="server" ControlToValidate="txtLocation" ErrorMessage="*" Display="Dynamic" ValidationGroup="receiveQuantity" />
                                                    <%--   <label>Location</label>--%>
                                                    <label><%= GetGlobalResourceObject("Resource", "Location")%> </label>
                                                    <span class="errorMsg"></span>
                                                </div>
                                            </div>

                                            <div class="col m3 s3">

                                                <div class="flex">
                                                    <asp:TextBox ID="txtContainerCode" runat="server" onKeypress="return checkSpecialChar(event)" SkinID="txt_Auto" required="" />
                                                             <asp:RequiredFieldValidator  ID="rfvContainerCode" runat="server" ValidationGroup="receiveQuantity" ControlToValidate="txtContainerCode" Display="Dynamic" ErrorMessage=" * " />
                                                    <label>
                                                        <asp:Literal ID="ltContainerCode" runat="server" Text="Pallet Code" /></label>
                                                      <span class="errorMsg"></span>
                                                    <%--   <span class="errorMsg"></span>--%>
                                                    <asp:HiddenField runat="server" ID="hifContainercode" Value="0" />
                                                </div>


                                            </div>

                                            <div class="col m3 s3">
                                                <div>
                                                    <div class="flex">
                                                        <asp:DropDownList ID="ddlStorageLocationID" runat="server" Width="180px" required=""></asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlStorageLocationID" InitialValue="0" ErrorMessage="*" Display="Static" ValidationGroup="receiveQuantity" />

                                                        <%--<label>Storage Location</label>--%>
                                                        <label><%= GetGlobalResourceObject("Resource", "StorageLocation")%></label>
                                                        <span class="errorMsg"></span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>



                                        <div class="row">
                                            <div class="col m3 s3">
                                                <div class="flex">
                                                    <asp:TextBox ID="txtbatchno" runat="server" required="" />
                                                    <%-- <label>BatchNo</label>--%>
                                                    <label><%= GetGlobalResourceObject("Resource", "BatchNo")%></label>

                                                </div>
                                            </div>
                                            <div class="col m3 s3">

                                                <div class="flex">
                                                    <asp:TextBox ID="txtMfgDate" ClientIDMode="Static" runat="server" required="" />

                                                    <label>
                                                        <asp:Literal ID="Literal1" runat="server" Text="Mfg Date " /></label>
                                                </div>


                                            </div>

                                            <div class="col m3 s3">
                                                <div class="flex">
                                                    <asp:TextBox ID="txtexpdate" ClientIDMode="Static" runat="server" required="" />


                                                    <%--<label>Exp Date</label>--%>
                                                    <label><%= GetGlobalResourceObject("Resource", "ExpDate")%></label>
                                                </div>
                                            </div>

                                            <div class="col m3 s3">
                                                <div class="flex">
                                                    <asp:TextBox ID="txtserialNo" runat="server" required="" />


                                                    <%--  <label>Serial no</label>--%>
                                                    <label><%= GetGlobalResourceObject("Resource", "Serialno")%></label>
                                                </div>
                                            </div>
                                        </div>


                                        <div class="row">
                                            <div class="col m3 s3">
                                                <div class="flex">
                                                    <asp:TextBox ID="txtprojectref" runat="server" required="" />


                                                    <%-- <label>Project Ref No.</label>--%>
                                                    <label><%= GetGlobalResourceObject("Resource", "ProjectRefNo")%></label>
                                                </div>
                                            </div>
                                             <div class="col m3 s3">
                                                <div class="flex">
                                                    <asp:TextBox ID="txtMRP" runat="server" onkeyup="CheckDigit(event,this);"  required="" />                                                                                                      
                                                    <label><%= GetGlobalResourceObject("Resource", "MRP")%> </label>
                                                   
                                                </div>
                                            </div>
                                            <div class="col m3 s3">
                                                <div class="flex">
                                                    <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" required="" />
                                                    <asp:RequiredFieldValidator ID="rfvRemarks" runat="server" ControlToValidate="txtRemarks" ErrorMessage="*" Display="Dynamic" ValidationGroup="receiveQuantity" />
                                                    <%--<label>Remarks</label>--%>
                                                    <label><%= GetGlobalResourceObject("Resource", "Remarks")%> </label>
                                                    <span class="errorMsg"></span>
                                                </div>
                                            </div>
                                            <div class="col m3 s3">
                                                <br />
                                                <br />
                                                <br />
                                                <asp:Button ID="lnksave" Visible="false" runat="server" OnClientClick="Openloader(); this.disable=true; this.text='Updating...'" OnClick="lnksave_Click" UseSubmitBehavior="false" CssClass="btn btn-primary right" ValidationGroup="receiveQuantity" Text="Receive"  />
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>

                    <tr>
                        <%-- <td colspan="4" align="right">--%>
                        <%--<asp:LinkButton ID="lnksave"  runat="server" OnClick="lnksave_Click" CssClass="btn btn-primary" ValidationGroup="receiveQuantity">Receive<%=MerlinCommon.CommonLogic.btnfaSave %></asp:LinkButton>--%>
                        <%--<asp:Button ID="lnksave"  Visible="false"  runat="server" OnClientClick="this.disable=true; this.text='Updateing...'" OnClick="lnksave_Click" UseSubmitBehavior="false"  CssClass="btn btn-primary right" ValidationGroup="receiveQuantity" Text="Receive" />--%>
                        <%--</td>--%>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:GridView ID="gvReceivedQty" ShowHeaderWhenEmpty="true" runat="server" AutoGenerateColumns="false" SkinID="gvLightBlueNew"
                                OnRowDataBound="gvReceivedQty_RowDataBound"
                                OnPageIndexChanging="gvReceivedQty_PageIndexChanging">
                                <Columns>
                                    <%--   <asp:TemplateField HeaderText="Location">--%>
                                    <asp:TemplateField HeaderText="<%$Resources:Resource,Location%>">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltLocation" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Location") %>' />
                                            <asp:HiddenField ID="ltgoodsmovementDetailId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"GoodsMovementDetailsID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--   <asp:TemplateField HeaderText="Container Code">--%>
                                    <asp:TemplateField HeaderText="<%$Resources:Resource,PalletCode%>">
                                        <ItemTemplate>

                                            <asp:Literal ID="ltccode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ContainerCode").ToString() %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField Visible="false" HeaderText="Is Dam.">--%>
                                    <asp:TemplateField Visible="false" HeaderText="Is Dam.">
                                        <ItemTemplate>
                                            <%#((bool)DataBinder.Eval(Container.DataItem,"isDamgae")?"<img src=\"../Images/blue_menu_icons/check_mark.png\">":"" )%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false" HeaderText="Has Disc.">
                                        <ItemTemplate>
                                            <%#((bool)DataBinder.Eval(Container.DataItem,"hasDiscapency")?"<img src=\"../Images/blue_menu_icons/check_mark.png\">":"" )%>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%--<asp:TemplateField HeaderText="Quantity" >--%>
                                    <asp:TemplateField HeaderText="<%$Resources:Resource,Quantity%>">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltAVAILABLE" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"qty") %>' />
                                            <asp:Literal ID="ltDynamicMsps" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--     <asp:TemplateField HeaderText="Storage Location">--%>
                                    <asp:TemplateField HeaderText="<%$Resources:Resource,StorageLocation%>">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltStorageLocationID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "StorageLocationID").ToString() %>' />
                                            <asp:Literal ID="ltCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Code").ToString() %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%-- <asp:TemplateField HeaderText="Batch No.">--%>
                                    <asp:TemplateField HeaderText="<%$Resources:Resource,BatchNo%>">
                                        <ItemTemplate>

                                            <asp:Literal ID="ltbatchNo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BatchNo").ToString() %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="Mfg. Date">--%>
                                    <asp:TemplateField HeaderText="<%$Resources:Resource,MfgDate%>">
                                        <ItemTemplate>

                                            <asp:Literal ID="ltmfgdate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MfgDate").ToString() %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-- <asp:TemplateField HeaderText="Exp. Date">--%>
                                    <asp:TemplateField HeaderText="<%$Resources:Resource,ExpDate%>">
                                        <ItemTemplate>

                                            <asp:Literal ID="ltexpdate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ExpDate").ToString() %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-- <asp:TemplateField HeaderText="Serial No.">--%>
                                    <asp:TemplateField HeaderText="<%$Resources:Resource,SerialNo%>">
                                        <ItemTemplate>

                                            <asp:Literal ID="ltserialNo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SerialNo").ToString() %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%--          <asp:TemplateField HeaderText="Project Ref. No.">--%>
                                    <asp:TemplateField HeaderText="<%$Resources:Resource,ProjectRefNo%>">
                                        <ItemTemplate>

                                            <asp:Literal ID="ltprojectrefno" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ProjectREf").ToString() %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                      <asp:TemplateField HeaderText="MRP">
                                        <ItemTemplate>

                                            <asp:Literal ID="ltmrp" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MRP").ToString() %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="msps">
                                    <ItemTemplate>
                                        
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
