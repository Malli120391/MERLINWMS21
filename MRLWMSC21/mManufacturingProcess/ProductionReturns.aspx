<%@ Page Title="Job Returns:." Language="C#" MasterPageFile="~/mManufacturingProcess/Manufacturing.master" AutoEventWireup="true" CodeBehind="ProductionReturns.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="MRLWMSC21.mManufacturingProcess.ProductionReturns" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ManfContent" runat="server">
     <script type="text/javascript" src="Scripts/CommonScripts.js"></script>
    <script>
        $(document).ready(function () {

            var textfieldname = $('#<%=atcKitCode.ClientID%>');
            DropdownFunction(textfieldname);
            $('#<%=atcKitCode.ClientID%>').autocomplete({
                source: function (request, response) {
                   
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetKitCodeList") %>',
                        data: "{ 'Prefix': '" + request.term + "','Type':'0'}",
                             dataType: "json",
                             type: "POST",
                             async: true,
                             contentType: "application/json; charset=utf-8",
                             success: function (data) {

                                 response($.map(data.d, function (item) {
                                     return {
                                         label: item.split(',')[0],
                                         val: item.split(',')[0]
                                     }
                                 }))

                             }

                         });
                     },
                select: function (e, i) {

                    $("#<%=this.hifKitCode.ClientID%>").val(i.item.val);

                     },
                     minLength: 0
            });

            var textfieldname = $('#<%=atcProductionOrder.ClientID%>');
            DropdownFunction(textfieldname);
            $("#<%= this.atcProductionOrder.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadPRONumberforReturns") %>',
                        data: "{ 'KitCode': '" + document.getElementById('<%=hifKitCode.ClientID%>').value + "' }",
                         dataType: "json",
                         type: "POST",
                         contentType: "application/json; charset=utf-8",
                         success: function (data) {
                             if (data.d == "") {
                                 alert('Please select kit code');
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

                    $("#<%=hifProductionOrder.ClientID %>").val(i.item.val);
                   
                },
                minLength: 0
            });
        });



        function CheckIUoMQty(textBox) {

           
            //var HiddenValues = textBox.parentNode.parentNode.getElementsByTagName('input')

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

            CheckDecimal(textBox);
        }
        

        function CheckQtys() {
            //var Table = document.getElementById('<=this.gvProductionOrderReturns.ClientID%>');

            //var tableRows = Table.getElementsByTagName('tr');

            //var Hiddenvalues;
            //var MUoMQty;
            //var BUoMQty;
            //var ReturnQty;
            //for (index = 1; index < tableRows.length - 1; index++) {
            //    Hiddenvalues = tableRows[index].getElementsByTagName('input');

            //    if (Hiddenvalues[3].checked && Hiddenvalues[4].value != "") {
            //        MUoMQty = parseFloat(Hiddenvalues[0].value) * 100;
            //        ReturnQty = parseFloat(Hiddenvalues[1].value) * parseFloat(Hiddenvalues[4].value) * 100;
            //        BUoMQty = parseFloat(Hiddenvalues[2].value) * 100;

            //        if (ReturnQty < MUoMQty) {

            //            showStickyToast(true, "'Entered Qty.' should be greater than or equal to MUoM Qty.");
            //            Hiddenvalues[3].value = "";
            //            return false;

            //        }
            //        if ((ReturnQty % BUoMQty) != 0) {
            //            showStickyToast(true, "'Entered Qty.' should be multiple of BUoMQty.");
            //            Hiddenvalues[3].value = "";
            //            return false;
            //        }
            //    }

            //}
            
            return true;

        }
</script>

    <table width="100%">
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Panel ID="pnlList" runat="server" DefaultButton="lnkGet">
                        Kit Code:
                        <asp:TextBox ID="atcKitCode" SkinID="txt_Auto" Width="160" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                        <asp:HiddenField ID="hifKitCode" runat="server" />
                        Job Order Ref. No.:
                        <asp:TextBox ID="atcProductionOrder" SkinID="txt_Auto" Width="180" runat="server" />
                        <asp:HiddenField ID="hifProductionOrder" runat="server" /> 
                        <asp:LinkButton ID="lnkGet" runat="server" CssClass="ui-btn ui-button-large" OnClientClick="showAsynchronus();" OnClick="lnkGet_Click" > Get Details<%=MRLWMSC21Common.CommonLogic.btnfaFilter %></asp:LinkButton>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td height="300px" valign="top">
                <asp:GridView ID="gvProductionOrderReturns" SkinID="gvLightGrayNew" OnRowDataBound="gvProductionOrderReturns_RowDataBound" AutoGenerateColumns="false" runat="server" >
                    <Columns>
                        <asp:TemplateField HeaderText="Line No." ItemStyle-Width="50">
                            <ItemTemplate>
                               
                                <asp:Literal ID="ltLineNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LineNumber") %>' />
                            <asp:HiddenField ID="hifMUoMQty"  runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "MUoMQty").ToString() %>' />
                            <asp:HiddenField ID="hifConversion" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "CF").ToString() %>' />
                            <asp:HiddenField ID="hifBUoMQty" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "BUoMQty").ToString() %>' />

                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Part Number" ItemStyle-Width="100">
                            <ItemTemplate>
                                <asp:Literal ID="ltMCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MCode") %>' />
                                <asp:Literal ID="ltMaterialMasterID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialMasterID") %>' />
                                <br />
                                <span style="color:#086A87;"> <nobr> <%# DataBinder.Eval(Container.DataItem, "OEMPartNo") %> </nobr></span>
                            </ItemTemplate>
                        </asp:TemplateField>

                         <asp:TemplateField HeaderText="BUoM/ Qty." ItemStyle-Width="70">
                            <ItemTemplate>
                                <asp:Literal ID="ltBMaterialMaster_UoMID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem,"MaterialMaster_IUoMID").ToString() %>' />
                                <asp:Literal ID="ltBUom_Qty" runat="server" Text='<%#String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "BUoM"),DataBinder.Eval(Container.DataItem, "BUoMQty"))  %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                         <asp:TemplateField HeaderText="MUoM/ Qty." ItemStyle-Width="70">
                            <ItemTemplate>
                                <asp:Literal ID="ltMMaterialMaster_UoMID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem,"MaterialMaster_IUoMID").ToString() %>' />
                                <asp:Literal ID="ltMUom_Qty" runat="server" Text='<%#String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "MUoM"),DataBinder.Eval(Container.DataItem, "MUoMQty"))  %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="IUoM/ Qty." ItemStyle-Width="70">
                            <ItemTemplate>
                                <asp:Literal ID="ltIMaterialMaster_UoMID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem,"MaterialMaster_IUoMID").ToString() %>' />
                                <asp:Literal ID="ltIUom_Qty" runat="server" Text='<%#String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "IUoM"),DataBinder.Eval(Container.DataItem, "IUoMQty"))  %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Kit ID" ItemStyle-Width="100">
                            <ItemTemplate>
                                <asp:Literal ID="ltIsKitParent" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "IsKitParent") %>' />
                                <asp:Literal ID="ltKitPlannerID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "KitPlannerID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Quantity" ItemStyle-Width="100">
                            <ItemTemplate>
                                <asp:Literal ID="ltDocQty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DocQty") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Returned Qty." ItemStyle-Width="100">
                            <ItemTemplate>
                                <asp:Literal ID="ltRemainingDocQty" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RemainingDocQty") %>' />
                                <asp:Literal ID="ltPOQuantity" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "POQuantity") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td align="right">
                <br />
                <asp:LinkButton ID="lnkReturn"  OnClientClick="showAsynchronus(); return CheckQtys();" runat="server"  OnClick="lnkReturn_Click" CssClass="ui-btn ui-button-large">Return<%=MRLWMSC21Common.CommonLogic.btnfaSignOut %></asp:LinkButton>
                <br />
                <br />
            </td>
        </tr>
    </table>
</asp:Content>
