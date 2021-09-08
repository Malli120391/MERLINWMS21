<%@ Page Title="Goods Out:." Language="C#" MasterPageFile="~/mInventory/InventoryMaster.master" AutoEventWireup="true" CodeBehind="PickItem.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="MRLWMSC21.mInventory.PickItem" %>

<asp:Content ID="Content1" ContentPlaceHolderID="InvContent" runat="server">
    <script type="text/javascript" src="Scripts/CommonScripts.js"></script>
    <asp:ScriptManager ID="mySManager" runat="server" EnablePartialRendering="true" SupportsPartialRendering="true" />
    <style>
        .strikeout {
            text-decoration: line-through;
        }
    </style>
    
    <script type="text/javascript">

        $(document).ready(function () {
            $("#divPrintDlg").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 50,
                minWidth: 200,
                height: 'auto',
                width: 'auto',
                overflow: 'auto',
                resizable: false,
                draggable: false,
                position: {
                    my: "center middle",
                    at: "center middle",
                    of: window
                },
                open: function (event, ui) { $(this).parent().appendTo("#divPrintDlgContainer"); }
            });
        });

        function closePrintDialog() {
            //Could cause an infinite loop because of "on close handling"
            $("#divPrintDlg").dialog('close');
        }
        function openPrintDialog() {
            $("#divPrintDlg").dialog("option", "title", "Print Item Details");
            $("#divPrintDlg").dialog('open');
        }





    </script>

    <script type="text/javascript">

        function CheckSUoMQty() {
        //    if (document.getElementById('<=this.hifMeasurementType.ClientID%>').value == "0") {
        //    var textBox = document.getElementById('<=this.txtPickQty.ClientID%>');
        //    //var ReceivedQty = parseFloat(textBox.value) * 100;
        //    //var MUoMQty = parseFloat(document.getElementById('<=this.hifMUoMQty.ClientID%>').value) * 100;
        //    //var BUoMQty = parseFloat(document.getElementById('<=this.hifBUoMQty.ClientID%>').value) * 100;
        //    var ReceivedQty = parseFloat(document.getElementById('<=this.hifConversion.ClientID%>').value) * parseFloat(textBox.value) * 100;

        //    //if (ReceiveQty < MUoMQty) {
        //    //    showStickyToast(true, "'Pick Quantity' should be greater than or equal to MUoM Qty.");
        //    //    textBox.value = "";
        //    //    return;
        //    //}

        //    //if ((ReceiveQty % BUoMQty) != 0) {
        //    //    showStickyToast(true, "'Pick Quantity' should be multiple of BUoMQty.");
        //    //    textBox.value = "";
        //    //    return;
        //    //}

        //    //TotalConvesionvalue = parseFloat(document.getElementById('<=this.hifConversion.ClientID%>').value) * 100;
        //    MUoMConversionvalue = parseFloat(document.getElementById('<=this.hifconversionInMUoM.ClientID%>').value) * 100;


        //    if (ReceivedQty < MUoMConversionvalue) {
        //        showStickyToast(true, "'Quantity Received' should be greater than or equal to MUoM Qty.");
        //        textBox.value = "";
        //        return;
        //    }
        //    if ((ReceivedQty % 100) != 0) {
        //        showStickyToast(true, "'Quantity Received' should be multiple of BUoMQty.");
        //        textBox.value = "";
        //        return;
        //    }
        //}
        //else {
        var textBox = document.getElementById('<%=this.txtPickQty.ClientID%>');

        //var ConversionvalueInMUoM = parseFloat(document.getElementById('<=this.hifconversionInMUoM.ClientID%>').value);
        //var Quantity = parseFloat(textBox.value);
        //var ReceivedQtyInMUoM = parseInt(ConversionvalueInMUoM * Quantity * 1000);
        ////alert(ConversionvalueInMUoM);
        /////MUoMConversionvalue = parseFloat(document.getElementById('<=this.hifconversionInMUoM.ClientID%>').value) * 100;
        //var ModuloValue = ReceivedQtyInMUoM % 1000;
        ////alert((Math.abs(100 - ModuloValue)) );
               
        //if (ModuloValue!=0 && (Math.abs(1000 - ModuloValue)) > 15) {
        //    showStickyToast(true, "'Quantity Received' should be multiple of MUoM Qty. <br/> Suggested Quantity is " + parseInt(Math.ceil(ReceivedQtyInMUoM / 1000) / ConversionvalueInMUoM*1000)/1000);
        //    textBox.value = "";
        //    return;
        //}




        //if ((ReceivedQty % 100) != 0) {
        //    showStickyToast(true, "'Quantity Received' should be multiple of BUoMQty.");
        //    textBox.value = "";
        //    return;
        //}



        //}


            CheckDecimal(textBox);
        }
        function RadioCheck(rb) {

            var gv = document.getElementById("<%=gvPOQuantityList.ClientID%>");

        var rbs = gv.getElementsByTagName("input");

        var row = rb.parentNode.parentNode.parentNode;
        //alert(row);
        var coumn = row.getElementsByTagName('td');
        //alert(coumn.length);
        var text = coumn[0].innerHTML.toString();
        //alert('llll'+text);

        document.getElementById('<%=txtPickLocation.ClientID%>').value = text.replace(/^\s+/, '').replace(/\s+$/, '');

         for (var i = 0; i < rbs.length; i++) {
             // alert(rbs[i]);
             if (rbs[i].type == "radio") {

                 if (rbs[i].checked && rbs[i] != rb) {

                     rbs[i].checked = false;

                     break;

                 }


             }

         }
        //alert('sdsds');
        /*  var tdradioButton = rb.parentNode.parentNode.parentNode;
          var hiddenfied = tdradioButton.getElementsByTagName('input')[0];
          if (hiddenfied.value == '0') {
              alert('Selected item is not a required storage parameter');
          }
         // alert('hhhhhh'); */
     }

    </script>
    <style>
        .gvLightGrayNew_headerGrid th {
            white-space:nowrap;
        }

        select {
            width: 53% !important;
        }
        input[type="text"], textarea {
                width: 50%;
        }

        .flex__ div:first-of-type {
            
               width: 220px;
        }

    </style>
<div class="dashed"></div>
    <div class="pagewidth">
    <table border="0" cellpadding="4" cellspacing="4" width="100%" style="padding-left: 142px;">

        <tr>
            <td>&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="3" align="left">Note:
                <asp:Label ID="Label2" runat="server" CssClass="errorMsg" Text=" * " />
                Indicates mandatory fields 
                                    
            </td>

        </tr>
        <tr>
            <td colspan="3">

                <asp:UpdatePanel ID="upnlGoodsOut" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
                    <ContentTemplate>

                        <asp:Panel runat="server" HorizontalAlign="center" ID="pnlgoodsOut" Width="80%">

                            <fieldset style="border: 1px solid #808080; border-radius: 5px; width: 100%;" align="left">
                                <legend><b>Pick Details</b></legend>

                                <table border="0" cellpadding="4" cellspacing="8" align="left" width="100%">
                                    <tr>
                                        <td style="width: 50%" align="left">
                                            <div class="flex__">
                                                <div><asp:Label runat="server" ID="lbMaterialCode" CssClass="FormLabelsBlue" Text="Part Number:" /></div>
                                                &nbsp;&nbsp;&nbsp;<div><asp:Literal runat="server" ID="ltMaterialCode" /></div>
                                             </div>
                                        </td>
                                        <td align="left">
                                            <div class="felx_">
                                                <div><asp:Label runat="server" Visible="false" ID="lbMaterialGroup" CssClass="FormLabelsBlue" Text="Material Group :" /></div>
                                                &nbsp;&nbsp;&nbsp;<div><asp:Literal runat="server" Visible="false" ID="ltMaterialGroup" /></div>
                                            </div>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="left">
                                            <div class="flex__">
                                                <div><asp:Label runat="server" ID="lbBUoMQty" CssClass="FormLabelsBlue" Text="BUoM/Qty.:" /></div>
                                                &nbsp;&nbsp;&nbsp;<div><asp:Literal runat="server" ID="ltBUoMQty" />
                                                <asp:HiddenField ID="hifBUoMQty" runat="server" /></div>
                                            </div>
                                        </td>
                                        <td align="left">
                                            <div class="flex__">
                                            <div><asp:Label runat="server" ID="lbMUoMQty" CssClass="FormLabelsBlue" Text="MUoM/Qty.:" /></div>
                                            &nbsp;&nbsp;&nbsp;<div><asp:Literal runat="server" ID="ltMUoMQty" />
                                            <asp:HiddenField ID="hifMUoMQty" runat="server" /></div></div>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="left">
                                            <div class="flex__">
                                                <div><asp:Label runat="server" ID="lbDescription" CssClass="FormLabelsBlue" Text="Description:" /></div>
                                            &nbsp;&nbsp;&nbsp;<div><asp:Literal runat="server" ID="ltDescription" /></div></div>
                                        </td>
                                        <td align="left">
                                            <div class="flex__">
                                                <div><asp:Label runat="server" ID="lbKitID" CssClass="FormLabelsBlue" Text="Kit ID:" /></div>
                                                &nbsp;&nbsp;&nbsp;<div><asp:Literal runat="server" ID="ltKitID" /></div>
                                            </div>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="left">
                                            <div class="flex__">
                                                <div><asp:Label runat="server" ID="lbSUoMQty" CssClass="FormLabelsBlue" Text="SO UoM/Qty.:" /></div>
                                                &nbsp;&nbsp;&nbsp;<div><asp:Literal runat="server" ID="ltSUoMQty" />
                                                <asp:HiddenField ID="hifSuomQtyID" runat="server" /></div>
                                            </div>
                                        </td>
                                        <td align="left">
                                            <div class="flex__">
                                               <div><asp:Label runat="server" ID="lbDelvDocQty" CssClass="FormLabelsBlue" Text="Delv. Doc. Qty.:" /></div>
                                                &nbsp;&nbsp;&nbsp;<div><asp:Literal runat="server" ID="ltDelvDocQty" /></div></div>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="left">
                                            <div class="flex__">
                                                <div><asp:Label runat="server" ID="lbQtyperUoM" CssClass="FormLabelsBlue" Text="Conversion Factor to BUoM:" /></div>
                                                <div><asp:Literal runat="server" ID="ltQtyperuom" />
                                                <asp:HiddenField ID="hifConversion" runat="server" />
                                                <asp:HiddenField ID="hifconversionInMUoM" runat="server" />
                                                <asp:HiddenField ID="hifMeasurementType" runat="server" /></div>
                                            </div>
                                        </td>


                                        <td align="left">
                                            <div class="flex__">
                                                <div><asp:Label runat="server" ID="lbTotalQuantity" CssClass="FormLabelsBlue" Text="Total Quantity in BUoM:" /></div>
                                                <div><asp:Literal runat="server" ID="ltTotalQuantity" /></div></div>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="2" align="left">
                                            <asp:GridView ID="gvPOQuantityList" SkinID="gvLightGrayNew" PageSize="20" Width="100%" runat="server" AutoGenerateColumns="false" OnPageIndexChanging="gvPOQuantityList_PageIndexChanging" OnRowDataBound="gvPOQuantityList_RowDataBound">
                                                <Columns>

                                                    <asp:TemplateField HeaderStyle-Width="80" HeaderText="Location">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ltLocation" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Location").ToString() %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderStyle-Width="80" HeaderText="Container Code">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ltContainerCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CartonCode").ToString() %>' />
                                                            <asp:HiddenField ID="hifCartonID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "CartonID").ToString() %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="40" HeaderText="Is Dam.">
                                                        <ItemTemplate>
                                                            <asp:HiddenField ID="hifGoodsMovementDetailsID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "GoodsMovementDetailsID").ToString() %>' />
                                                            <asp:Image ID="imgDamage" runat="server" ImageAlign="Middle" ImageUrl='<%# Getimage(DataBinder.Eval(Container.DataItem, "IsDamaged").ToString()) %>' />
                                                            <asp:Literal ID="ltIsDamaged" runat="server" Visible="false" Text='<%# Getint(DataBinder.Eval(Container.DataItem, "IsDamaged").ToString()).ToString() %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="40" HeaderText="Has Disc.">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ltHasDiscrepancy" Visible="false" runat="server" Text='<%# Getint(DataBinder.Eval(Container.DataItem, "HasDiscrepancy").ToString()).ToString() %>' />
                                                            <asp:Image ID="imgDiscrepancy" runat="server" ImageAlign="Middle" ImageUrl='<%# Getimage(DataBinder.Eval(Container.DataItem, "HasDiscrepancy").ToString()) %>' />

                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="40" HeaderText="QC Non Conf.">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ltIsNonConfirmity" Visible="false" runat="server" Text='<%# Getint(DataBinder.Eval(Container.DataItem, "IsNonConfirmity").ToString()).ToString() %>' />
                                                            <asp:Image ID="imgIsNonConfirmity" runat="server" ImageAlign="Middle" ImageUrl='<%# Getimage(DataBinder.Eval(Container.DataItem, "IsNonConfirmity").ToString()) %>' />

                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="40" HeaderText="Positive Recall">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ltIsPositiveRecall" Visible="false" runat="server" Text='<%# Getint(DataBinder.Eval(Container.DataItem, "IsPositiveRecall").ToString()).ToString() %>' />
                                                            <asp:Image ID="imgIsPositiveRecall" runat="server" ImageAlign="Middle" ImageUrl='<%# Getimage(DataBinder.Eval(Container.DataItem, "IsPositiveRecall").ToString()) %>' />

                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="40" HeaderText="As Is">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ltAsIs" Visible="false" runat="server" Text='<%# Getint(DataBinder.Eval(Container.DataItem, "AsIs").ToString()).ToString() %>' />
                                                            <asp:Image ID="imgAsIs" runat="server" ImageAlign="Middle" ImageUrl='<%# Getimage(DataBinder.Eval(Container.DataItem, "AsIs").ToString()) %>' />

                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="60" HeaderText="Quantity" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ltQuantity" runat="server" Text='<%# GetQtyInIUoM( DataBinder.Eval(Container.DataItem, "AvailableQuantity").ToString()) %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                 <asp:TemplateField HeaderStyle-Width="60" HeaderText="Storage Location" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ltStorageLocationID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "StorageLocationID").ToString() %>' />
                                                            <asp:Literal ID="ltCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Code").ToString() %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                </Columns>
                                            </asp:GridView>

                                        </td>
                                    </tr>

                                    <tr>
                                        <td align="left">
                                         <%--   <asp:RequiredFieldValidator ID="rfvpickQty" runat="server" ControlToValidate="txtPickQty" ValidationGroup="Pickitems" Display="Dynamic" ErrorMessage=" * " />
                                            <asp:Literal ID="ltPickQty" runat="server" Text="Pick Quantity:" />--%>
                                            <div class="flex">
                                                 <asp:TextBox ID="txtPickQty" runat="server" onKeyPress="return checkDec(this,event)" onblur="CheckSUoMQty()" required=""/>
                                                  <asp:RequiredFieldValidator ID="rfvpickQty" runat="server" ControlToValidate="txtPickQty" ValidationGroup="Pickitems" Display="Dynamic" ErrorMessage=" * " />
                                                  <label><asp:Literal ID="ltPickQty" runat="server" Text="Pick Quantity" /></label></div>

                                        </td>
                                        <td align="left">
                                            <asp:RequiredFieldValidator ID="rfvLocation" Enabled="false" runat="server" ControlToValidate="txtPickLocation" ValidationGroup="Pickitems" Display="Dynamic" ErrorMessage=" * " />
                                            <asp:Literal ID="ltPickLocation" runat="server" Text="Pick Location:" />
                                            <asp:TextBox ID="txtPickLocation" Enabled="false" onKeyPress="return checkSpecialChar(event)" runat="server" />
                                        </td>
                                    </tr>
                                   <%-- <tr>
                                        <td colspan="2">
                                            &nbsp;&nbsp;
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td align="left">

                                            <asp:CheckBox runat="server" ID="chkIsPrintRequired" Text="Is Print Required" Checked="true" />

                                            <asp:DropDownList ID="ddlNetworkPrinter" runat="server" CssClass="NoPrint" />
                                        </td>
                                        
                                        <td align="left">
                                            <asp:Label ID="lblLabelSize" runat="server">Label Size:</asp:Label><br />
                                            <asp:DropDownList ID="ddlLabelSize" runat="server" CssClass="NoPrint"></asp:DropDownList>
                                            &nbsp;&nbsp;&nbsp;

                                            <asp:LinkButton ID="lnkPickItem" runat="server" CssClass="btn btn-primary right" OnClientClick="showAsynchronus();CheckSUoMQty();" OnClick="lnkPickItem_Click" ValidationGroup="Pickitems" >Pick Item<%=MRLWMSC21Common.CommonLogic.btnfaTransfer %></asp:LinkButton>

                                            <br />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td colspan="2" align="left">

                                            <asp:GridView ID="gvPickedItemList" SkinID="gvLightGreenNew" PageSize="20" runat="server" Width="100%" OnRowEditing="gvPickedItemList_RowEditing" OnRowUpdating="gvPickedItemList_RowUpdating" OnSelectedIndexChanging="gvPickedItemList_SelectedIndexChanging" AutoGenerateColumns="false" OnRowCancelingEdit="gvPickedItemList_RowCancelingEdit" OnPageIndexChanging="gvPickedItemList_PageIndexChanging" OnRowDataBound="gvPickedItemList_RowDataBound" OnRowCommand="gvPickedItemList_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField HeaderStyle-Width="80" HeaderText="Container Code">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ltContainerCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CartonCode").ToString() %>' />
                                                            <asp:HiddenField ID="hifCartonID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "CartonID").ToString() %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="100" HeaderText="Location">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ltGoodsMovementDetailsID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "GoodsMovementDetailsID").ToString() %>' />
                                                            <asp:Literal ID="ltLocation" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Location").ToString() %>' />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:RequiredFieldValidator ID="rfvLocation" runat="server" ValidationGroup="gridrequirequantity" ControlToValidate="txtLocation" Display="Dynamic" ErrorMessage=" * " />
                                                            <%--<asp:Literal ID="ltGoodsMovementDetailsID_Edit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "GoodsMovementDetailsID").ToString() %>' />--%>
                                                            <asp:TextBox ID="txtLocation" Width="70" runat="server" onKeypress="return checkSpecialChar(event)" Text='<%# DataBinder.Eval(Container.DataItem, "Location").ToString() %>' />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                      
                                                    <asp:TemplateField HeaderStyle-Width="60" HeaderText="Is Dam.">
                                                        <ItemTemplate>
                                                            <asp:Image ID="imgDamage" ImageAlign="Middle" runat="server" ImageUrl='<%# Getimage(DataBinder.Eval(Container.DataItem, "IsDamaged").ToString()) %>' />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:CheckBox ID="chkDamaged" Width="30" runat="server" Checked='<%# GetBool(DataBinder.Eval(Container.DataItem, "IsDamaged").ToString()) %>' />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="60" HeaderText="Has Disc.">
                                                        <ItemTemplate>
                                                            <asp:Image ID="imgDisc" ImageAlign="Middle" runat="server" ImageUrl='<%# Getimage(DataBinder.Eval(Container.DataItem, "HasDiscrepancy").ToString()) %>' />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:CheckBox ID="chkDisc" Width="30" runat="server" Checked='<%# GetBool(DataBinder.Eval(Container.DataItem, "HasDiscrepancy").ToString()) %>' />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderStyle-Width="60" HeaderText="QC Non Conf.">
                                                        <ItemTemplate>
                                                            <asp:Image ID="imgIsNC" ImageAlign="Middle" runat="server" ImageUrl='<%# Getimage(DataBinder.Eval(Container.DataItem, "IsNonConfirmity").ToString()) %>' />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:CheckBox ID="chkIsNC" Width="30" runat="server" Checked='<%# GetBool(DataBinder.Eval(Container.DataItem, "IsNonConfirmity").ToString()) %>' />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="40" HeaderText="Positive Recall">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ltIsPositiveRecall" Visible="false" runat="server" Text='<%# Getint(DataBinder.Eval(Container.DataItem, "IsPositiveRecall").ToString()).ToString() %>' />
                                                            <asp:Image ID="imgIsPositiveRecall" runat="server" ImageAlign="Middle" ImageUrl='<%# Getimage(DataBinder.Eval(Container.DataItem, "IsPositiveRecall").ToString()) %>' />

                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="60" HeaderText="As Is">
                                                        <ItemTemplate>
                                                            <asp:Image ID="imgAsIs" ImageAlign="Middle" runat="server" ImageUrl='<%# Getimage(DataBinder.Eval(Container.DataItem, "AsIs").ToString()) %>' />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:CheckBox ID="chkAsIs" Width="30" runat="server" Checked='<%# GetBool(DataBinder.Eval(Container.DataItem, "AsIs").ToString()) %>' />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderStyle-Width="100" HeaderText="Quantity" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ltQuantity" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DocQty").ToString() %>' />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ValidationGroup="gridrequirequantity" ControlToValidate="txtQuantity" Display="Dynamic" ErrorMessage=" * " />
                                                            <asp:TextBox ID="txtQuantity" Width="70" onKeyPress="return checkDec(event)" onblur="CheckDecimal(this)" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Quantity").ToString() %>' />
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Literal ID="ltQuantityCount" runat="server" />
                                                        </FooterTemplate>


                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>

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
    
    <!-- Input Dialog for Print Quantity -->
    
    <div id="divPrintDlgContainer">

        <div id="divPrintDlg" style="display: none;">

            <asp:UpdatePanel ID="upnlPrintDialog" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
                <ContentTemplate>

                    <table border="0" align="center" cellpadding="5" cellspacing="5">
                        <tr>
                            <td class="FormLabels">
                                <asp:RequiredFieldValidator ID="rfvtxtPrintQty" runat="server" Enabled="false" ControlToValidate="txtPrintQty" Display="Dynamic" ErrorMessage=" * " />
                                <asp:Label runat="server" ID="lblPrintQty" Text="Please enter quantity :" CssClass="FormLabelsBlue"></asp:Label>
                                &nbsp;&nbsp;&nbsp;

                                <asp:TextBox runat="server" ID="txtPrintQty" Text="1" Width="50" onKeyPress="return checkNum(event)"></asp:TextBox>
                            </td>

                        </tr>
                        <tr>
                            <td align="right">

                                <asp:LinkButton runat="server" ID="lnkPrintSubmit" OnClientClick="showAsynchronus();" OnClick="lnkPrintSubmit_Click" SkinID="lnkButEmpty" Text="Ok"></asp:LinkButton>

                            </td>

                        </tr>

                    </table>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    
    <!-- Input Dialog for Print Quantity -->
        </div>
</asp:Content>
