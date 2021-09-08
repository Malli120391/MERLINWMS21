<%@ Page Title="" Language="C#" MasterPageFile="~/mInventory/InventoryMaster.master" AutoEventWireup="true" CodeBehind="ContainerTransfer.aspx.cs" Inherits="MRLWMSC21.mInventory.ContainerTransfer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="InvContent" runat="server">
     <asp:ScriptManager runat="server" ID="ssss" SupportsPartialRendering="true" EnablePartialRendering="true"></asp:ScriptManager>
    <script type="text/javascript" src="Scripts/CommonScripts.js"></script>
    <link href="css/InventoryMaster.css" rel="stylesheet" />
    <script>
        $(document).ready(function () {
            var TextFieldName = $("#<%= this.txtCarton.ClientID %>");
            DropdownFunction(TextFieldName);
            $("#<%= this.txtCarton.ClientID %>").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetContainerList") %>',
                            data: "{ 'Prefix': '" + request.term + "'}",//<=cp.TenantID%>
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

                        $("#<%=hifcontainer.ClientID %>").val(i.item.val);

                    },
                    minLength: 0
            });



            var textfieldname = $('#<%=this.txtLocation.ClientID%>');
            DropdownFunction(textfieldname);
            $('#<%=this.txtLocation.ClientID%>').autocomplete({

                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetAllLocations") %>',
                      
                        data: "{'Prefix': '" + request.term + "','ProductCategory':'PSDD'}",
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
                    $("#<%=this.hiflocationID.ClientID%>").val(i.item.val);

                },
                minLength: 0
            });

        });
    </script>
    <div class="dashed"></div>
    <div class="pagewidth">
    <table style="width: 100%;">
        <tr>
            <td style="width: 20%;"></td>
            <td style="width: 20%;"></td>
            <td style="width: 20%;"></td>
            <td style="width: 20%;"></td>
            <td style="width: 20%;"></td>
        </tr>
        <tr>

            <td class="FormLabels " colspan="6">
                <div class="flex__ " style="float:right !important;">
                <div class="flex">
                
                <asp:TextBox ID="txtCarton" SkinID="txt_Auto" runat="server" required="" />
                    <label> Container Code:</label>
                    <span class="errorMsg"></span>
                <asp:HiddenField ID="hifcontainer" runat="server" />   
                     </div>
                    <div>
                 <asp:LinkButton ID="lnkGet" runat="server" OnClientClick="showAsynchronus();" OnClick="lnkGet_Click" CssClass="btn btn-primary">
                Get Details <%=MRLWMSC21Common.CommonLogic.btnfaFilter %>
                </asp:LinkButton>
                        </div>
                    </div>
            </td>
        </tr>
        <tr>
            <td colspan="5">
                <asp:GridView ID="gvcartonlist" runat="server" SkinID="gvLightBlueNew">
                    <Columns>
                        <asp:TemplateField HeaderText="Carton Code">
                            <ItemTemplate>
                                <asp:Literal ID="ltcartoncode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CartonCode").ToString() %>' />
                                <asp:Literal ID="ltGoodsMovementDetailsIDs" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "GoodsMovementDetailsID").ToString() %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Part Number">
                            <ItemTemplate>
                                <asp:Literal ID="ltMcode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MCode").ToString() %>' />
                                <asp:Literal ID="ltMMID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialMasterID").ToString() %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Part Description">
                            <ItemTemplate>
                                <asp:Literal ID="ltpartdescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MDescription").ToString() %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Location">
                            <ItemTemplate>
                                <asp:Literal ID="ltLocation" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Location").ToString() %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mtype">
                            <ItemTemplate>
                                <asp:Literal ID="ltMtype" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MType").ToString() %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Available_Qty">
                            <ItemTemplate>
                                <asp:Literal ID="ltAvailableQty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Available_Qty").ToString() %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="INOH">
                            <ItemTemplate>
                                <asp:Literal ID="ltINOH" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "INOH").ToString() %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="OBOH">
                            <ItemTemplate>
                                <asp:Literal ID="ltOBOH" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "OBOH").ToString() %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                </asp:GridView>
            </td>
        </tr>
        <tr>
            
            <td  colspan="6" >
                <div class="flex__" style="float:right !important;">
                    <div class="flex">
                    <asp:TextBox ID="txtLocation" onKeypress="return checkSpecialChar(event)" runat="server" SkinID="txt_Auto" required="" />
                    <label>  Location:</label>
                    <span class="errorMsg"></span>
                <asp:HiddenField runat="server" ID="hiflocationID" Value="0" />
                    </div>
                    <div>
                <asp:LinkButton ID="lnkTransfer" runat="server" OnClientClick="showAsynchronus();" OnClick="lnkTransfer_Click" CssClass="btn btn-primary">
                     Transfer <span class="space fa fa-exchange"></span>
                </asp:LinkButton>
                        </div>
                    </div>
            </td>
        </tr>
    </table>
        </div>
</asp:Content>
