<%@ Page Title=" Functional Configuration :." Language="C#" MasterPageFile="~/mMaterialManagement/MaterialManagementMaster.master" AutoEventWireup="true" CodeBehind="FunctionalConfiguration.aspx.cs" Inherits="MRLWMSC21.mMaterialManagement.FunctionalConfiguration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
    <script src="Scripts/CommonScripts.js"></script>
    <script>
        $(document).ready(function () {

            var textfieldname = $("#<%= this.txtFunctionalExecutiveCode.ClientID %>");
            DropdownFunction(textfieldname);
            $("#<%= this.txtFunctionalExecutiveCode.ClientID %>").autocomplete({
                    source: function (request, response) {

                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/GetFuncExecCode") %>',
                            data: "{ 'Prefix': '" + request.term + "'}",
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                response($.map(data.d, function (item) {
                                    return {
                                        label: item.split(',')[0],
                                        val: item.split(',')[1]
                                    }
                                }
                                ))
                            },
                            error: function (response) {

                            },
                            failure: function (response) {

                            }
                        });
                    },
                    minLength: 0
                });


            $('#txtValue').focusout(function () {

                if (document.getElementById('<%= this.hifDataType.ClientID %>').value=="1")
                    {
                        if (parseInt($('#txtValue').val()) > parseInt('<%=ViewState["value"].ToString()%>')) {
                            $('#txtValue').val('');
                            showStickyToast(true, '<%=ViewState["Error"].ToString()%>');
                            }
                    }
            });

        });
    </script>
    <style>
        
        .gvLightGrayNew_DataCellGrid td {
            font-size:14px;
            padding:7px 5px;
        }
        
        .gvLightGrayNew_DataCellGridAlt td {
            font-size:14px;
            padding:7px 5px;
        }

        .gvLightGrayNew_footerGrid {
            display: none;
        }
    </style>
    <div class="dashed"></div>
    <div class="pagewidth">
    <table style="width:100%;overflow:hidden !important;" class="internalData" cellpadding="5" cellspacing="5">
        <tr>
            <td align="right">
                <table>
                    <tr>
                        <td><div class="flex__ right">
                            <div class="flex">
                               
                                <asp:TextBox ID="txtFunctionalExecutiveCode" runat="server" required="" SkinID="txt_Req" />
                                    <label>Func. Exe. Code</label>
                               
                            </div>&nbsp;&nbsp;
                            <div><asp:LinkButton ID="lnkSearch" runat="server" OnClick="lnkSearch_Click" CssClass="btn btn-primary">Search<%=MRLWMSC21Common.CommonLogic.btnfaSearch %></asp:LinkButton></div>
                        </div></td>
                    </tr>
                </table>
                
                
            </td>
        </tr>
        <tr>
            <td>

                <asp:GridView ID="gvFunctionalStrategy" runat="server" SkinID="gvLightGrayNew" 
                    OnRowEditing="gvFunctionalStrategy_RowEditing"
                    OnRowCancelingEdit="gvFunctionalStrategy_RowCancelingEdit"
                    OnRowUpdating="gvFunctionalStrategy_RowUpdating"
                    OnPageIndexChanging="gvFunctionalStrategy_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="Func. Exe. Code">
                            <ItemTemplate>
                                <asp:Literal ID="ltFuncExecCode" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"FuncExecCode") %>' />
                                <asp:HiddenField ID="hifConfigurableFunctionID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"ConfigurableFunctionID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                                <asp:Literal ID="ltDescription" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Description") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Strategy Code">
                            <ItemTemplate>
                                <asp:Literal ID="ltDisplayStrategyCode" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"StrategyCode") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Literal ID="ltStrategyCode" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"StrategyCode") %>' />
                                <asp:HiddenField ID="hifFunctionalStrategyID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem,"FunctionalStrategyID") %>' />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                                <asp:Literal ID="ltDescription" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Description") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DataType" Visible="false">
                            <ItemTemplate>
                                <asp:Literal ID="ltDataType" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"DataType") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Value">
                            <ItemTemplate>
                                <asp:Literal ID="ltValue" runat="server" Text='<%# displayValue(DataBinder.Eval(Container.DataItem,"Value").ToString() ,DataBinder.Eval(Container.DataItem,"DataType").ToString())%>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:RequiredFieldValidator ID="rfvValue" runat="server" ControlToValidate="txtValue" Display="Dynamic" ErrorMessage="*" ValidationGroup="vgFunctionalStrategy" />
                                <asp:TextBox ID="txtValue" ClientIDMode="Static" Width="100" onkeypress="return checkNum(event)" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Value") %>' />
                                <asp:CheckBox ID="chkValue" runat="server" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField UpdateText="Update" EditText="<i class='material-icons ss'>mode_edit</i>" CancelText="Cancel" ControlStyle-CssClass="ButEmpty"  ShowEditButton="true" ValidationGroup="vgFunctionalStrategy" />
                    </Columns>
                </asp:GridView>

            </td>
        </tr>
    </table>
    </div>
    <asp:HiddenField runat="server" ID="hifDataType" Value="0" />

</asp:Content>
