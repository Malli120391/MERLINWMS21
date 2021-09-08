<%@ Page Title="Tool Return:." Language="C#" MasterPageFile="~/mOrders/OrdersMaster.master" AutoEventWireup="true" CodeBehind="ToolReturn.aspx.cs" Inherits="MRLWMSC21.mInventory.ToolReturn" %>
<asp:Content ID="Content1" ContentPlaceHolderID="OrdersContent" runat="server">
    <script type="text/javascript" src="Scripts/CommonScripts.js"></script>
    <script>
        $(document).ready(function () {
            var textfieldname = $("#<%= this.atcEmployee.ClientID %>");
            DropdownFunction(textfieldname);
            $("#<%= this.atcEmployee.ClientID %>").autocomplete({
                source: function (request, response) {

                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetEmployeeListInTMO") %>',
                        data: "{'prefix':'" + request.term + "' }",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            if (data.d == "") {
                                alert('No Employees');
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

                    $("#<%=hifEmployee.ClientID %>").val(i.item.val);
                    document.getElementById('<%=this.atcSONumber.ClientID%>').value = "";
                    document.getElementById('<%=this.atcStoreNo.ClientID%>').value = "";
                },
                minLength: 0
            });

            var textfieldname = $("#<%= this.atcSONumber.ClientID %>");
            DropdownFunction(textfieldname);
            $("#<%= this.atcSONumber.ClientID %>").autocomplete({
                source: function (request, response) {

                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetTMOListEmployee") %>',
                        data: "{ 'UserID':'" + document.getElementById('<%=this.hifEmployee.ClientID%>').value + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            if (data.d == "") {
                                alert('Employe not took Tools');
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

                    $("#<%=hifSONumber.ClientID %>").val(i.item.val);
                  },
                  minLength: 0
            });

            var textfieldname = $("#<%= this.atcStoreNo.ClientID %>");
            DropdownFunction(textfieldname);
            $("#<%= this.atcStoreNo.ClientID %>").autocomplete({
                source: function (request, response) {
                   
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetStoreForEmployee") %>',
                        data: "{ 'SOHeaderID':'" + document.getElementById('<%=this.hifSONumber.ClientID%>').value + "'}",
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
        })



        function CheckIUoMQty(textBox) {
            var HiddenValues = textBox.parentNode.parentNode.getElementsByTagName('input')

            var MUoMQty = parseFloat(HiddenValues[0].value) * 100;
            var BUoMQty = parseFloat(HiddenValues[2].value) * 100;
            var ReceiveQty = parseFloat(HiddenValues[1].value) * parseFloat(textBox.value) * 100;

            if (ReceiveQty < MUoMQty) {
                showStickyToast(true, "'Entered Qty.' should be greater than or equal to MUoM Qty.");
                textBox.value = "";
                return;
            }
            if ((ReceiveQty % BUoMQty) != 0) {
                showStickyToast(true, "'Entered Qty.' should be multiple of BUoMQty.");
                textBox.value = "";
                return;
            }

            CheckDecimal(textBox);
        }

        function CheckQtys() {
            var Table = document.getElementById('<%=this.gvToolReturn.ClientID%>');

            var tableRows = Table.getElementsByTagName('tr');
            var Hiddenvalues;
            var MUoMQty;
            var BUoMQty;
            var ReturnQty;
            for (index = 1; index < tableRows.length - 1; index++) {
                Hiddenvalues = tableRows[index].getElementsByTagName('input');
                if (Hiddenvalues[3].checked && Hiddenvalues[4].value != "") {
                    MUoMQty = parseFloat(Hiddenvalues[0].value) * 100;
                    BUoMQty = parseFloat(Hiddenvalues[2].value) * 100;
                    ReturnQty = parseFloat(Hiddenvalues[1].value) * parseFloat(Hiddenvalues[4].value) * 100;

                    if (ReturnQty < MUoMQty) {
                        showStickyToast(true, "'Entered Qty.' should be greater than or equal to MUoM Qty.");
                        Hiddenvalues[3].value = "";
                        return false;
                    }
                    if ((ReturnQty % BUoMQty) != 0) {
                        showStickyToast(true, "'Entered Qty.' should be multiple of BUoMQty.");
                        Hiddenvalues[3].value = "";
                        return false;
                    }
                }

            }
            return true;
        }






    </script>
<div class="dashed"></div>
<div class="pagewidth">
    <table width="100%" cellpadding="3" cellspacing="3">
        <tr>
            <td align="right">
                <br />
                <asp:Panel ID="pnlList" runat="server" DefaultButton="lnkGetData">
                        Employee:       
                        <asp:TextBox ID="atcEmployee" Width="180" SkinID="txt_Auto" runat="server" />
                        <asp:HiddenField ID="hifEmployee" runat="server" />
                        Issue No.:
                        <asp:TextBox ID="atcSONumber" Width="120" SkinID="txt_Auto" runat="server" />
                        Store: 
                        <asp:TextBox ID="atcStoreNo" Width="150" SkinID="txt_Auto" runat="server" />
                        <asp:HiddenField ID="hifStoreNo" runat="server"  />
                        <asp:HiddenField ID="hifSONumber" runat="server" />
                        <asp:LinkButton ID="lnkGetData" runat="server" CssClass="ui-btn ui-button-large" OnClientClick="showAsynchronus();" OnClick="lnkGetData_Click" >
                            Get Details<%=MRLWMSC21Common.CommonLogic.btnfaFilter %>
                        </asp:LinkButton>
                </asp:Panel>
            </td>
        </tr>

        <tr>
            <td height="300px" valign="top">
                <asp:GridView ID="gvToolReturn" runat="server" OnRowDataBound="gvToolReturn_RowDataBound" SkinID="gvLightGrayNew" AutoGenerateColumns="false" >
                    <Columns>
                        <asp:TemplateField HeaderText="Line No." ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                
                                <asp:Literal ID="ltLineNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LineNumber").ToString() %>' />
                                <asp:Literal ID="ltIsKitParent" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "IsKitParent").ToString() %>' />
                                
                                <asp:HiddenField ID="hifMUoMQty"  runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "MUoMQty").ToString() %>' />
                                <asp:HiddenField ID="hifConversion" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "CF").ToString() %>' />
                                <asp:HiddenField ID="hifBUoMQty" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "BUoMQty").ToString() %>' />

                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Part Number" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Literal ID="ltMaterialID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialMasterID").ToString() %>' />
                                <asp:Literal ID="ltMCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MCode").ToString() %>' />
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
                        <asp:TemplateField HeaderText="Received Qty." ItemStyle-HorizontalAlign="Right">
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
                </asp:GridView>
            </td>
        </tr>

        <tr>
            <td align="right">
                <br />
                <br />
                <asp:LinkButton ID="lnkReturn" runat="server" OnClientClick="showAsynchronus(); return CheckQtys();" CssClass="ui-btn ui-button-large" Text="Return" OnClick="lnkReturn_Click">
                    Return <%=MRLWMSC21Common.CommonLogic.btnfaSignOut %>
                </asp:LinkButton>
                <br />
                <br />
            </td>
        </tr>
    </table>
    </div>
</asp:Content>
