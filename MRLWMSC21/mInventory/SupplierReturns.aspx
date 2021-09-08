<%@ Page Title="Supplier Returns:." Language="C#" MasterPageFile="~/mInventory/InventoryMaster.master" AutoEventWireup="true" CodeBehind="SupplierReturns.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="MRLWMSC21.mInventory.SupplierReturns" %>
<asp:Content ID="Content1" ContentPlaceHolderID="InvContent" runat="server">
    <asp:ScriptManager ID="sMngrIbSearch" runat="server" EnablePartialRendering="true" SupportsPartialRendering="true"></asp:ScriptManager>
    <script type="text/javascript" src="Scripts/CommonScripts.js"></script>
    <script>
        $(document).ready(function () {
            try{
                CheckQtys();
            }catch(err){
            }
            //$("#<%= this.txtTenant.ClientID %>").focusout(function () {
              //  if ($("#<%= this.txtTenant.ClientID %>").val() == '')
                //    $("#<%= this.txtTenant.ClientID %>").val('Tenant');

            //});

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
                        $("#<%=atcPONumber.ClientID %>").val('');
                    },
                    minLength: 0
            });
            var textfieldname = $("#<%= this.atcPONumber.ClientID %>");
            DropdownFunction(textfieldname);
            $("#<%= this.atcPONumber.ClientID %>").autocomplete({
                source: function (request, response) {

                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetPOHeaderListTenant") %>',
                        data: "{'prefix':'" + request.term + "' ,'TenantID':'" + $("#<%=hifTenant.ClientID %>").val() + "'}",
                          dataType: "json",
                          type: "POST",
                          contentType: "application/json; charset=utf-8",
                          success: function (data) {
                              if (data.d == "") {
                                  alert('No Suppliers');
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

                    $("#<%=hifPONumber.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });

            var textfieldname = $("#<%= this.atcSupplierNumber.ClientID %>");
            DropdownFunction(textfieldname);
            $("#<%= this.atcSupplierNumber.ClientID %>").autocomplete({
                source: function (request, response) {

                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetInvoiceListForPONumber") %>',
                          data: "{ 'POHeaderID':'" + document.getElementById('<%=this.hifPONumber.ClientID%>').value + "'}",
                          dataType: "json",
                          type: "POST",
                          contentType: "application/json; charset=utf-8",
                          success: function (data) {
                              if (data.d == "") {
                                  alert('No PONumbers to this Supplier');
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

                      $("#<%=hifSupplierNumber.ClientID %>").val(i.item.val);
                },
                minLength: 0
              });

            var textfieldname = $("#<%= this.atcStore.ClientID %>");
            DropdownFunction(textfieldname);
            $("#<%= this.atcStore.ClientID %>").autocomplete({
                source: function (request, response) {

                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetStoreForPONumber") %>',
                        data: "{ 'POHeaderID':'" + document.getElementById('<%=this.hifPONumber.ClientID%>').value + "','InvoiceNumber':'" + document.getElementById('<%=this.hifSupplierNumber.ClientID%>').value + "'}",
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

                    $("#<%=hifStore.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });

        });


        function CheckIUoMQty(textBox) {
            var HiddenValues = textBox.parentNode.parentNode.getElementsByTagName('input')
                       
            //var MUoMQty = parseFloat(HiddenValues[0].value) * 100;
            //var ReceiveQty = parseFloat(HiddenValues[1].value) * parseFloat(textBox.value) * 100;
            //var BUoMQty = parseFloat(HiddenValues[2].value) * 100;

            //if (ReceiveQty < MUoMQty) {
            //    showStickyToast(true, "'Entered Qty.' should be greater than or equal to MUoM Qty.");
            //    textBox.value = "";
            //    return;
            //}
            //if ((ReceiveQty % BUoMQty) != 0) {
            //    showStickyToast(true, "'Entered Qty.' should be multiple of BUoMQty.");
            //    textBox.value = "";
            //    return;
            //}
            //if (HiddenValues[4].value == "") {

            //    var ReceivedQty = parseFloat(HiddenValues[1].value) * parseFloat(textBox.value) * 100;
            //    MUoMConversionvalue = parseFloat(HiddenValues[3].value) * 100;

            //    if (ReceivedQty < MUoMConversionvalue) {
            //        showStickyToast(true, "'Entered Qty.' should be greater than or equal to MUoM Qty.");
            //        textBox.value = "";
            //        return;
            //    }
            //    if ((ReceivedQty % 100) != 0) {
            //        showStickyToast(true, "'Entered Qty.' should be multiple of BUoMQty.");
            //        textBox.value = "";
            //        return;
            //    }
            //}
            //else {




                //var ConversionvalueInMUoM = parseFloat(HiddenValues[3].value);
                //var Quantity = parseFloat(textBox.value);
                //var ReceivedQtyInMUoM = parseInt(ConversionvalueInMUoM * Quantity * 100);
                //var ModuloValue = ReceivedQtyInMUoM % 100;

                //if (ModuloValue != 0 && (Math.abs(100 - ModuloValue)) > 15) {
                //    showStickyToast(true, "'Entered Qty.' should be multiple of MUoM Qty. <br/> Suggested Quantity is " + parseInt(Math.ceil(ReceivedQtyInMUoM / 100) / ConversionvalueInMUoM * 1000) / 1000);
                //    textBox.value = "";
                //    return;
                //}




            //}

            CheckDecimal(textBox);
        }

        function CheckQtys() {
            var Table = document.getElementById('<%=this.gvSupplierReturns.ClientID%>');

            var tableRows = Table.getElementsByTagName('tr');
            var Hiddenvalues;
            var MUoMQty;
            var BUoMQty;
            var ReturnQty;
            for (index = 1; index < tableRows.length - 1; index++) {
                Hiddenvalues = tableRows[index].getElementsByTagName('input');
                if (Hiddenvalues[3].checked && Hiddenvalues[4].value != "") {
                    //MUoMQty = parseFloat(Hiddenvalues[0].value) * 100;
                    //ReturnQty = parseFloat(Hiddenvalues[1].value) * parseFloat(Hiddenvalues[4].value) * 100;
                    //BUoMQty = parseFloat(Hiddenvalues[2].value) * 100;

                    //if (ReturnQty < MUoMQty) {
                    //    showStickyToast(true, "'Entered Qty.' should be greater than or equal to MUoM Qty.");
                    //    Hiddenvalues[3].value = "";
                    //    return false;
                    //}
                    //if ((ReturnQty % BUoMQty) != 0) {
                    //    showStickyToast(true, "'Entered Qty.' should be multiple of BUoMQty.");
                    //    Hiddenvalues[3].value = "";
                    //    return false;
                    //}


                    //if (HiddenValues[4].value == "") {

                    //    var ReceivedQty = parseFloat(HiddenValues[1].value) * parseFloat(Hiddenvalues[6].value) * 100;
                    //    MUoMConversionvalue = parseFloat(HiddenValues[3].value) * 100;

                    //    if (ReceivedQty < MUoMConversionvalue) {
                    //        Hiddenvalues[6].value = "";
                    //    }
                    //    if ((ReceivedQty % 100) != 0) {
                    //        Hiddenvalues[6].value = "";
                    //    }
                    //}
                    //else {

                        var ConversionvalueInMUoM = parseFloat(HiddenValues[3].value);
                        var Quantity = parseFloat(Hiddenvalues[6].value);
                        var ReceivedQtyInMUoM = parseInt(ConversionvalueInMUoM * Quantity * 100);
                        var ModuloValue = ReceivedQtyInMUoM % 100;

                        if (ModuloValue != 0 && (Math.abs(100 - ModuloValue)) > 15) {
                            Hiddenvalues[6].value = "";
                        }
                    //}

                }

            }
            return true;
        }



    </script>

    <style>
        .flex input[type="text"], input[type="number"], textarea {
                width: 90%;
        }

        #MainContent_InvContent_pnlList {
            float: right
        }


        .gvLightSeaBlueNew_headerGrid th {
            white-space:nowrap;
        }

        .isinscroll {
                width: calc(92vw - 122px) !important;
        }
    </style>

    <link href="../Content/app.css" rel="stylesheet" />
    <div class="dashed"></div>
    <table class="tbsty">
        <tbody>
            <tr class="module_yellow">
               <td class="ModuleHeader fixed-width">
                        <div><a href="../Default.aspx">Home</a> / Orders / <span class="FormSubHeading">Returns / Supplier Returns</span></div>
                </td>
             </tr>
        </tbody>
    </table>
    <div class="pagewidth">
    <table>
        <tr>
                     <td colspan="1" align="left" >
                         <br />
                     </td>
                    
        </tr>
        <tr>
            <td align="right" style="padding:10px;">
               <asp:Panel ID="pnlList" runat="server" DefaultButton="lnkGetDetails">
                   <table class="internalData"  align="right" >
                       <tr>
                           <td>
                               <div class="flex">
                                   <asp:TextBox ID="txtTenant" runat="server" SkinID="txt_Hidden_Req" required=""></asp:TextBox>
                                   <label>Tenant</label>
                                   <asp:HiddenField runat="server" ID="hifTenant" />
                               </div>
                           </td>
                           <td>
                               <div class="flex">
                                   <asp:TextBox ID="atcPONumber" SkinID="txt_Auto" runat="server" required="" />
                                   <label>PO Number</label>
                                   <span class="errorMsg"></span>
                               </div>
                           </td>
                           <td>
                               <div class="flex">
                                   <asp:TextBox ID="atcSupplierNumber" SkinID="txt_Auto" runat="server" required="" />
                                   <label>Invoice No.</label>
                                   <span class="errorMsg"></span>
                                </div>
                           </td>
                           <td>
                               <div class="flex">
                                   <asp:TextBox  ID="atcStore" SkinID="txt_Auto" runat="server" required="" />
                                   <span class="errorMsg"></span>
                                   <label>Store</label></div>
                           </td>
                           <td class="vertivle-align-middel">
                               <asp:LinkButton ID="lnkGetDetails" runat="server" OnClick="lnkGetDetails_Click" OnClientClick="showAsynchronus();" CssClass="btn btn-primary">
                                Get Details <i class="material-icons vl">filter_list</i>
                               </asp:LinkButton>
                           </td>
                       </tr>
                   </table>
                        <asp:HiddenField ID="hifPONumber" runat="server" value="0"/>
                        <asp:HiddenField ID="hifSupplierNumber" runat="server" value="0"/>
                        <asp:HiddenField ID="hifStore" runat="server" value="0"/>
                </asp:Panel>
            </td>
        </tr>
       
        <tr>
            <td valign="top" >
                <div class="inscroll">
                    <asp:GridView ID="gvSupplierReturns" runat="server" ShowHeader="true" ShowHeaderWhenEmpty="true" SkinID="gvLightSeaBlueNew" OnRowDataBound="gvSupplierReturns_RowDataBound" AutoGenerateColumns="false" width="100%">
                <Columns>
                    <asp:TemplateField HeaderText="Line No.">
                        <ItemTemplate>
                            <asp:Literal ID="ltLineNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LineNumber").ToString() %>' />
                            <asp:HiddenField ID="hifMUoMQty"  runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "MUoMQty").ToString() %>' />
                            <asp:HiddenField ID="hifConversion" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "CF").ToString() %>' />
                            <asp:HiddenField ID="hifBUoMQty" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "BUoMQty").ToString() %>' />

                            <asp:HiddenField ID="hifConversionInMUoM" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "CFInMUoM").ToString() %>' />
                            <asp:HiddenField ID="hifMeasurementType" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "MeasurementTypeID").ToString() %>' />


                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Part Number">
                        <ItemTemplate>
                            <asp:Literal ID="ltMCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MCode").ToString() %>' />
                            <asp:Literal ID="ltMaterialMasterID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialMasterID").ToString() %>' />
                            <br />
                            <span style="color:#1287a1;"> <nobr> <%# DataBinder.Eval(Container.DataItem, "OEMPartNo").ToString() %>  </nobr> </span>

                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Kit ID">
                        <ItemTemplate>
                            <asp:Literal ID="ltIsKitParent" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "IsKitParent").ToString() %>' />
                            <asp:Literal ID="ltKitPlannerID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "KitPlannerID").ToString() %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    
                    <asp:TemplateField HeaderText="BUoM/ Qty.">
                        <ItemTemplate>
                            <asp:Literal ID="ltbuomqty" runat="server" Text='<%#String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "BUoM"),DataBinder.Eval(Container.DataItem, "BUoMQty"))  %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="MUoM/ Qty.">
                        <ItemTemplate>
                            <asp:Literal ID="ltmuomqty" runat="server" Text='<%#String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "MUoM"),DataBinder.Eval(Container.DataItem, "MUoMQty"))  %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="IUoM/ Qty.">
                        <ItemTemplate>
                            <asp:Literal ID="ltIUOMQTY" runat="server" Text='<%#String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "IUoM"),DataBinder.Eval(Container.DataItem, "IUoMQty"))  %>' />
                            <asp:Literal ID="ltMaterial_iuomid" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "MaterialMaster_InvUoMID")  %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Available Qty.">
                        <ItemTemplate>
                            <asp:Literal ID="ltAvailableQty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "AvailableDocQty").ToString() %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Returned Qty.">
                        <ItemTemplate>
                            <asp:Literal ID="ltReturnedQty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SOQuantity").ToString() %>' />
                            <asp:Literal ID="ltRemainingQty" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RemainingQty").ToString() %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Picked Qty.">
                        <ItemTemplate>
                            <asp:Literal ID="ltPickedQty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PickedQty").ToString() %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Is Dam.">
                        <ItemTemplate>
                            <asp:Literal ID="ltIsDamaged" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "IsDamaged").ToString() %>' />
                            <asp:Image ID="imgIsDamaged"  ImageAlign="Middle" runat="server" ImageUrl='<%# Getimage(DataBinder.Eval(Container.DataItem, "IsDamaged").ToString()) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Has Disc.">
                        <ItemTemplate>
                            <asp:Literal ID="ltHasDiscrepancy" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "HasDiscrepancy").ToString() %>' />
                            <asp:Image ID="imgHasDiscrepancy"  ImageAlign="Middle" runat="server" ImageUrl='<%# Getimage(DataBinder.Eval(Container.DataItem, "HasDiscrepancy").ToString()) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="QC Non Conf.">
                        <ItemTemplate>
                            <asp:Literal ID="ltIsNonConfirmity" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "IsNonConfirmity").ToString() %>' />
                            <asp:Image ID="imgIsNonConfirmity"  ImageAlign="Middle" runat="server" ImageUrl='<%# Getimage(DataBinder.Eval(Container.DataItem, "IsNonConfirmity").ToString()) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="As Is">
                        <ItemTemplate>
                            <asp:Literal ID="ltAsIs" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "AsIs").ToString() %>' />
                            <asp:Image ID="imgAsIs"  ImageAlign="Middle" runat="server" ImageUrl='<%# Getimage(DataBinder.Eval(Container.DataItem, "AsIs").ToString()) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
	                <div align="center">No Data Found</div>
                </EmptyDataTemplate>
            </asp:GridView>
                    </div>

                </td>
        </tr>
        <tr>
            
            <td align="right">
             <br />
                <asp:LinkButton ID="lnkSupplierReturns" OnClientClick="showAsynchronus(); return CheckQtys();" runat="server"  OnClick="lnkSupplierReturns_Click" CssClass="btn btn-primary" style="float: right;">

                    Return <i class="material-icons vl">keyboard_return</i>

                    </asp:LinkButton>
              
            </td>
        </tr>
    </table></div>
</asp:Content>
