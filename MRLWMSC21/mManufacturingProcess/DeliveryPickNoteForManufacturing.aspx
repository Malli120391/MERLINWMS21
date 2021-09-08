<%@ Page Title="" Language="C#" MasterPageFile="~/mManufacturingProcess/Manufacturing.master" AutoEventWireup="true" CodeBehind="DeliveryPickNoteForManufacturing.aspx.cs" Inherits="MRLWMSC21.mManufacturingProcess.DeliveryPickNoteForManufacturing" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ManfContent" runat="server">

    <script>

        $(document).ready(function () {

            $('#<%=atcProductionOrderNo.ClientID%>').autocomplete({
                source: function (request, response) {

                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadPRORefNo") %>',
                    data: "{ 'prefix': '" + request.term + "'}",
                    dataType: "json",
                    type: "POST",
                    async: true,
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
        function ClearText(TextBox) {
            if (TextBox.value == "Production Order RefNo...")
                TextBox.value = "";
        }
        function focuslost(TextBox) {
            if (TextBox.value == "")
                TextBox.value = "Production Order RefNo...";
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
                <asp:TextBox ID="atcProductionOrderNo" onblur="javascript:focuslost(this)" onfocus="ClearText(this)" Text="Production Order RefNo..." runat="server"></asp:TextBox>
                <asp:LinkButton ID="lnkProductionorder" Text="Get" runat="server" SkinID="lnkButEmpty" OnClick="lnkProductionorder_Click" />
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="gvDeliveryPickNote" SkinID="gvLightGreen" runat="server" PageSize="20"  AutoGenerateColumns="false" OnPageIndexChanging="gvDeliveryPickNote_PageIndexChanging" >
                    <Columns>
                        <asp:TemplateField HeaderText="SO Number" ItemStyle-Width="100">
                            <ItemTemplate>
                                <a style="text-decoration:none;" href="../mOrders/SalesOrderInfo.aspx?soid=<%#DataBinder.Eval(Container.DataItem, "SOHeaderID").ToString() %>"><%# DataBinder.Eval(Container.DataItem, "SONumber").ToString()%></a>
                                
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="WorkCenter Group" ItemStyle-Width="200">
                            <ItemTemplate>
                                <asp:Literal ID="ltWorkCenterGroup" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "WorkCenterGroup").ToString() %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Material Code" ItemStyle-Width="150">
                            <ItemTemplate>
                                <asp:Literal ID="ltMCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MCode").ToString() %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="WorkGroup Qty" ItemStyle-Width="150">
                            <ItemTemplate>
                                <asp:Literal ID="ltWorkGroupQty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "WorkGroupQty").ToString() %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="UoM/Qty" ItemStyle-Width="150">
                            <ItemTemplate>
                                <asp:Literal ID="ltMCode" runat="server" Text='<%# string.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "UoM").ToString(),DataBinder.Eval(Container.DataItem, "UoMQty").ToString())  %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Received Qty" ItemStyle-Width="150">
                            <ItemTemplate>
                                <asp:Literal ID="ltReceivedQty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ReceivedQty").ToString() %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Consumed Qty" ItemStyle-Width="150">
                            <ItemTemplate>
                                <asp:Literal ID="ltConsumedQty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ConsumedQty").ToString() %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
            </td>
        </tr>
    </table>



</asp:Content>
