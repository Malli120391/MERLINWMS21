<%@ Page Title="Customer Returns:." Language="C#" MasterPageFile="~/mInventory/InventoryMaster.Master" AutoEventWireup="true" CodeBehind="CustomerReturn.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="MRLWMSC21.mInventory.CustomerReturn" %>
<asp:Content ID="Content1" ContentPlaceHolderID="InvContent" runat="server">
    <asp:ScriptManager runat="server" EnablePartialRendering="true" ID="smCustomerReturn" SupportsPartialRendering="true"></asp:ScriptManager>
     <script type="text/javascript" src="Scripts/CommonScripts.js"></script>

    <script>
        $(document).ready(function () {

            try{
                CheckQtys();
            } catch (err) {
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

                    },
                    minLength: 0
                });
            var textfieldname = $("#<%= this.atcOBDNumber.ClientID %>");
            DropdownFunction(textfieldname);
            $("#<%= this.atcOBDNumber.ClientID %>").autocomplete({
                source: function (request, response) {
                   
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadReturnOBDNumbersTenant") %>',
                        data: "{'prefix':'" + request.term + "','TenantID':'"+$("#<%=hifTenant.ClientID %>").val()+"' }",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            if (data.d == "") {
                                alert('No OBDnumber');
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

                    $("#<%=hifOBDNumber.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });

            textfieldname = $("#<%= this.atcInvoice.ClientID %>");
            DropdownFunction(textfieldname);
            $("#<%= this.atcInvoice.ClientID %>").autocomplete({
                source: function (request, response) {

                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadReturnInvoices") %>',
                        data: "{'prefix':'" + request.term + "','OutboundID':'" + document.getElementById('<%=this.hifOBDNumber.ClientID%>').value + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            if (data.d == "") {

                                alert('No Invoice');
                            }
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split(',')[1],
                                    val: item.split(',')[0]
                                }

                            }))
                        }

                    });
                },
                 select: function (e, i) {

                     $("#<%=hifInvoice.ClientID %>").val(i.item.val);
                },
                minLength: 0
            });

            textfieldname = $("#<%= this.atcStoreNo.ClientID %>");
            DropdownFunction(textfieldname);
            $("#<%= this.atcStoreNo.ClientID %>").autocomplete({
                source: function (request, response) {

                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadReturnStoreNumber") %>',
                        data: "{ 'OutboundID':'" + document.getElementById('<%=this.hifOBDNumber.ClientID%>').value + "'}",
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

                    $("#<%=hifStoreNo.ClientID %>").val(i.item.val);
                 },
                 minLength: 0
            });

        });



        function CheckIUoMQty(textBox) {
            var HiddenValues = textBox.parentNode.parentNode.getElementsByTagName('input')

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
            var Table = document.getElementById('<%=this.gvReturnsDetails.ClientID%>');

            var tableRows = Table.getElementsByTagName('tr');
            var Hiddenvalues;
            var MUoMQty;
            var BUoMQty;
            var ReturnQty;
            for (index = 1; index < tableRows.length - 1; index++) {
                Hiddenvalues = tableRows[index].getElementsByTagName('input');
                if (Hiddenvalues[5].checked && Hiddenvalues[6].value != "") {

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
            width:85% !important;
        }

        #MainContent_InvContent_pnlList {
            
            float: right;
        }
    </style>
    <link href="../Content/app.css" rel="stylesheet" />
    <div class="dashed"></div>
   

    <div class="pagewidth">
    <table >

        <tr>
            <td colspan="1" align="left">
                <br />

            </td>

        </tr>

        <tr>
            <td align="right" style="padding:10px;">
                <asp:Panel ID="pnlList" runat="server" DefaultButton="lnkGetData">
                    <table class="internalData" align="right">
                        <tr>

                            <td>
                                <div class="flex">
                                <asp:TextBox ID="txtTenant" runat="server" SkinID="txt_Hidden_Req" required=""></asp:TextBox>
                                    <label>Tenant</label>
                                <asp:HiddenField runat="server" ID="hifTenant"/>
                                </div>
                            </td>
                            <td>
                                <div class="flex">
                                    <asp:TextBox ID="atcOBDNumber" SkinID="txt_Auto" runat="server" required="" />
                                    <label>OBD Number</label>
                                    <span class="errorMsg"></span>
                                 </div>
                            </td>
                            <td>
                                <div class="flex">
                                    <asp:TextBox ID="atcInvoice" SkinID="txt_Auto" runat="server" required="" />
                                    <label>Invoice</label>
                                    <span class="errorMsg"></span>
                                </div>
                            </td>
                            <td>
                                <div class="flex">
                                    <asp:TextBox ID="atcStoreNo" SkinID="txt_Auto"  runat="server" required="" />
                                    <label>Store</label>
                                    <span class="errorMsg"></span>
                                 </div>
                            </td>
                            <td style="vertical-align:bottom">
                                <asp:LinkButton ID="lnkGetData" runat="server" OnClientClick="showAsynchronus();"  OnClick="lnkGetData_Click" CssClass="btn btn-primary">
                         Get Details <i class="material-icons vl">filter_list</i>
                        </asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                        
                      
                        
                        

                        
                        &nbsp;

                        <asp:HiddenField ID="hifOBDNumber" runat="server"  />
                        <asp:HiddenField ID="hifStoreNo" runat="server"  />
                        <asp:HiddenField ID="hifInvoice" runat="server" />
                </asp:Panel>
            </td>
        </tr>

        <tr height="300px" valign="top">
            <td>
              
                <asp:GridView SkinID="gvLightLimeGreenNew" ID="gvReturnsDetails" ShowHeader="true" ShowHeaderWhenEmpty="true" OnRowDataBound="gvReturnsDetails_RowDataBound" runat="server" AutoGenerateColumns="false">
                    <Columns>
                        <asp:TemplateField HeaderText="Line No." ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:HiddenField ID="hifMuomQty" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "MUoMQty").ToString() %>' />
                                <asp:HiddenField ID="hifConversion" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "CF").ToString() %>' />                         

                                <asp:HiddenField ID="hifBUoMQty" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "BUoMQty").ToString() %>' />
                                 <asp:HiddenField ID="hifConversionInMUoM" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "CFInMUoM").ToString() %>' />
                                <asp:HiddenField ID="hifMeasurementType" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "MeasurementTypeID").ToString() %>' />

                                <asp:Literal ID="ltLineNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LineNumber").ToString() %>' />
                                <asp:Literal ID="ltIsKitParent" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "IsKitParent").ToString() %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Part Number" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Literal ID="ltMaterialID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialMasterID").ToString() %>' />
                                <asp:Literal ID="ltMCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MCode").ToString() %>' />

                                 <br />
                           <span style="color:#1287a1;"> <nobr> <%# DataBinder.Eval(Container.DataItem, "OEMPartNo").ToString() %>  </nobr> </span>
                        

                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="BUoM/Qty." ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Literal ID="ltbuom_qty" runat="server" Text='<%# String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "BUoM").ToString(),DataBinder.Eval(Container.DataItem, "BUoMQty").ToString())  %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="MUoM/Qty." ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Literal ID="ltmuom_qty" runat="server" Text='<%# String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "MUoM").ToString(),DataBinder.Eval(Container.DataItem, "MUoMQty").ToString())  %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="IUoM/Qty." ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Literal ID="ltMaterialUomID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialMaster_SUoMID").ToString() %>' />
                                <asp:Literal ID="ltiuom_qty" runat="server" Text='<%# String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "SUoM").ToString(),DataBinder.Eval(Container.DataItem, "SUoMQty").ToString())  %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Picked Qty." ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Literal ID="ltPickedQuantity" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PickedQuantity").ToString() %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Returned Qty." ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Literal ID="ltReturnsQty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ReturnsQty").ToString() %>' />
                                <asp:Literal ID="ltRemainingQty" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "Remaining").ToString() %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Kit ID" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Literal ID="ltKitPlannerID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "KitPlannerID").ToString() %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                    </Columns>
                    <EmptyDataTemplate>
                        <div align="center">No Data Found</div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>

        <tr>
            <td align="right">
                <br />
                <asp:LinkButton ID="lnkCustomerReturn"  OnClientClick="showAsynchronus();return CheckQtys();"  OnClick="lnkCustomerReturn_Click" runat="server" CssClass="btn btn-primary" >

                    Return <i class="material-icons vl">keyboard_return</i>

                    </asp:LinkButton>


                <br /><br />
            </td>
        </tr>

    </table>
        </div>
 
</asp:Content>
