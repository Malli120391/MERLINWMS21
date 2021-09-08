<%@ Page Title="Inward NC Report:." Language="C#" MasterPageFile="~/mInventory/InventoryMaster.master" AutoEventWireup="true" CodeBehind="NCReportForGoodsIn.aspx.cs" Inherits="MRLWMSC21.mInventory.NCReportForGoodsIn" %>
<asp:Content ID="Content1" ContentPlaceHolderID="InvContent" runat="server">

    <script>
        $(document).ready(function () {

            $('#<%=this.atcPOHeader.ClientID%>').autocomplete({

                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetPONumberForNCReport") %>',
                        data: "{ 'Prefix': '" + request.term + "'}",
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
                select: function (e, i) {

                    $("#<%=hifPOHeader.ClientID %>").val(i.item.val);
                 },
                minLength: 0
            });

        });
    </script>
    <div class="dashed"></div>
    <div class="pagewidth">
    <table align="center" height="360px">
        <tr valign="top">
            <td align="right" valign="top">
                <br />
                <div style="position:relative" align="bottom">
                
                PO Number:&nbsp;&nbsp;
                <asp:TextBox ID="atcPOHeader" Width="100" runat="server" />&nbsp;&nbsp;
                QC Ref. No.&nbsp;&nbsp;
                <asp:TextBox ID="txtRefNo" Width="100" runat="server" />
                <asp:LinkButton ID="lnkGet" runat="server" OnClick="lnkGet_Click" Text="Get" SkinID="lnkButEmpty" />
                <asp:HiddenField ID="hifPOHeader" runat="server" />
                    </div>
            </td>
        </tr>
        <tr valign="top">
            <td align="center">
                <asp:GridView ID="gvNCReport" runat="server" Width="100%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" OnRowDataBound="gvNCReport_RowDataBound" OnPageIndexChanging="gvNCReport_PageIndexChanging" AutoGenerateColumns="false" PageSize="15">
                    <Columns>
                        <asp:TemplateField HeaderText="Line No."  HeaderStyle-Width="30">
                            <ItemTemplate >
                                <asp:Literal ID="ltLineNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LineNumber").ToString() %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Part Number" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100">
                            <ItemTemplate >
                                <asp:Literal ID="ltMcode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MCode").ToString() %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="QC Result" HeaderStyle-Width="400">
                            <ItemTemplate >
                                <asp:Literal ID="ltQCResult" runat="server"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <br />
            </td>
        </tr>
    </table>  
        </div>
</asp:Content>
