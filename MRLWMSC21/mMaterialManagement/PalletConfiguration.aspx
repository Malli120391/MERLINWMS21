<%@ Page Title=" Pallet Configuration :." Language="C#" MasterPageFile="~/mMaterialManagement/MaterialManagementMaster.master" AutoEventWireup="true" CodeBehind="PalletConfiguration.aspx.cs" Inherits="MRLWMSC21.mMaterialManagement.PalletConfiguration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">

    <asp:ScriptManager ID="mySManager" runat="server" EnablePartialRendering="true" SupportsPartialRendering="true" />

    <style>
        .module_login {
            border-right: 0px solid #1a79cf;
            border-left: 0px solid ;
        }
        a.ButEmpty {
            background-image: unset;
        }

        .gvLightSeaBlue_DataCellGrid {
            background: #fff;
        }

        .gvLightSeaBlue {
            border: 0px !important;
            border-radius: 0px !important;
            background-color: #e67f2247 !important;
        }
        /*.gvLightSeaBlue_DataCellGridAlt {
            background-color: #fff8ee !important;
        }*/

        .gvLightSeaBlue_pager tr {
            display: flex;
            justify-content:flex-end;
        }

        .gvLightSeaBlue_headerGrid{
                background-color: #ffe4b8;
        }

        .gvLightSeaBlue_pager {
            background-color: #fff;
        }
        .gvLightSeaBlue_footerGrid {
            display: none;
        }

        .gvLightSeaBlue_DataCellGridEdit .ButEmpty a{
                
                font-weight: 400;
                text-align: center;
                white-space: nowrap;
                vertical-align: middle;
                -webkit-user-select: none;
                -moz-user-select: none;
                -ms-user-select: none;
                user-select: none;
                border: 1px solid transparent;
                padding: 0.375rem 0.75rem;
                font-size: 1.3rem;
                line-height: 1.5;
                border-radius: 0.25rem;
                transition: background-color 0.15s ease-in-out, border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out;
                box-shadow: var(--z1);
                border-radius: 2px;
                position: relative;
                top: -4px;
                padding: 1px 4px;
                color: #0e0e0e;
                font-size: 12px;
                font-weight: bold;
                margin-bottom: 3px;
                background: #fff;
                width: 80%;
        }

        .gvLightSeaBlue_DataCellGrid td {
            font-size:14px;
        }
        
        .gvLightSeaBlue_DataCellGridAlt td {
            font-size:14px;
        }
    </style>

    <script type="text/javascript" src="Scripts/CommonScripts.js"></script>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                fnLoadMCode();
            }
        }

        function fnLoadMCode() {

            $(document).ready(function () {

            var textfieldname = $("#txtWhcode");
            DropdownFunction(textfieldname);
            $("#txtWhcode").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadWarehouseData") %>',
                        data: "{ 'prefix': '" + request.term + "', 'TenantID':'0' }",
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
                        error: function (response) {

                        },
                        failure: function (response) {

                        }
                    });
                },
                select: function (e, i) {
                    $("#hifWarehouseID").val(i.item.val);
                    $("#txtLocationZoneCode").val("");
                },
                minLength: 0
            });

                var textfieldname = $("#txtLocationZoneCode");
                DropdownFunction(textfieldname);
                $("#txtLocationZoneCode").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetLocationZones") %>',
                            data: "{ 'Prefix': '" + request.term + "', 'WarehouseID': '" + $("#hifWarehouseID").val() + "' }",
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
                        error: function (response) {

                        },
                        failure: function (response) {

                        }
                    });
                },
                    select: function (e, i) {
                    $("#hifLocationZoneID").val(i.item.val);
                },
                minLength: 0
            });
            });
        }
    fnLoadMCode();

    </script>
    <div class="dashed"></div>
    <div class="pagewidth">
    <table width="100%" class="internalData" cellpadding="5" cellspacing="5">
        <tr>
            <td>

            </td>
            <td align="right">
                <asp:LinkButton ID="lnkAddNewPallet" runat="server" OnClick="lnkAddNewPallet_Click" CssClass="btn btn-primary right">Add New <%=MRLWMSC21Common.CommonLogic.btnfaNew %></asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:GridView ID="gvPalletAllocation" runat="server" SkinID="gvLightSeaBlue" AllowPaging="true" AutoGenerateColumns="false" 
                    OnRowDataBound="gvPalletAllocation_RowDataBound"
                    OnRowEditing="gvPalletAllocation_RowEditing"
                    OnRowCancelingEdit="gvPalletAllocation_RowCancelingEdit"
                    OnRowUpdating="gvPalletAllocation_RowUpdating"
                    OnPageIndexChanging="gvPalletAllocation_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="Pallet Code" ItemStyle-Width="120">
                            <ItemTemplate>
                                <asp:Literal ID="ltPalletCode" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"PalletCode") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:RequiredFieldValidator ID="rfvPalletCode" runat="server" ControlToValidate="txtPalletCode" ValidationGroup="vgPallet" ErrorMessage="*" />
                                <asp:TextBox ID="txtPalletCode" ClientIDMode="Static" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"PalletCode") %>'/>
                                <asp:HiddenField ID="hifPalletID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"PalletID") %>'/>
                                
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Reusable" ItemStyle-Width="40">
                            <ItemTemplate>
                                <asp:Literal ID="ltIsReusable" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"IsReusable").ToString()=="1" %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkIsReusable" runat="server" Checked='<%#DataBinder.Eval(Container.DataItem,"IsReusable").ToString()=="1" %>' />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Length" ItemStyle-Width="80">
                            <ItemTemplate>
                                <asp:Literal ID="ltLength" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Length") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:RequiredFieldValidator ID="rfvLength" runat="server" ControlToValidate="txtLength" ValidationGroup="vgPallet" ErrorMessage="*" />
                                <asp:TextBox ID="txtLength" Width="80" onkeypress="return checkNum(event)" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Length") %>'/>                                
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Width" ItemStyle-Width="80">
                            <ItemTemplate>
                                <asp:Literal ID="ltWidth" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Width") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:RequiredFieldValidator ID="rfvWidth" runat="server" ControlToValidate="txtWidth" ValidationGroup="vgPallet" ErrorMessage="*" />
                                <asp:TextBox ID="txtWidth" runat="server" onkeypress="return checkNum(event)" Width="80" Text='<%#DataBinder.Eval(Container.DataItem,"Width") %>'/>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Height" ItemStyle-Width="80">
                            <ItemTemplate>
                                <asp:Literal ID="ltHeight" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Height") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:RequiredFieldValidator ID="rfHeight" runat="server" ControlToValidate="txtHeight" ValidationGroup="vgPallet" ErrorMessage="*" />
                                <asp:TextBox ID="txtHeight" Width="80" onkeypress="return checkNum(event)" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Height") %>'/>
                            </EditItemTemplate>
                        </asp:TemplateField>
                     
                        <asp:TemplateField HeaderText="Warehouse" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="120">
                            <ItemTemplate>
                                <asp:Literal ID="ltWHCode" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"WHCode") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:RequiredFieldValidator ID="rfWhcode"  runat="server" ControlToValidate="txtWhcode" ValidationGroup="vgPallet" ErrorMessage="*" />
                                <%--<asp:DropDownList ID="ddlWHCode"  runat="server" />--%>
                                <asp:TextBox ID="txtWhcode" ClientIDMode="Static" Width="160" SkinID="txt_Hidden_Req" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"WHCode") %>' />
                                <asp:HiddenField ID="hifWarehouseID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"WarehouseID") %>' ClientIDMode="Static" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        
                         <asp:TemplateField HeaderText="Zone" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="120">
                            <ItemTemplate>
                                <asp:Literal ID="ltLocationZoneCode" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"LocationZoneCode") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:RequiredFieldValidator ID="rfvLocationZoneCode" runat="server" ControlToValidate="txtLocationZoneCode" ValidationGroup="vgPallet" ErrorMessage="*" />
                                <%--<asp:DropDownList ID="ddlLocationZoneCode" runat="server" />--%>
                                <asp:TextBox ID="txtLocationZoneCode" Width="120" ClientIDMode="Static" SkinID="txt_Hidden_Req" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"LocationZoneCode") %>' />
                                <asp:HiddenField ID="hifLocationZoneID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"LocationZoneID") %>' ClientIDMode="Static" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:CommandField ValidationGroup="vgPallet" ControlStyle-CssClass="ButEmpty" ButtonType="Link" ItemStyle-CssClass="ButEmpty"  ItemStyle-HorizontalAlign="Right" EditText="<i class='material-icons ss'>mode_edit</i>" ShowEditButton="True" ItemStyle-Width="20" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
        </div>
</asp:Content>
