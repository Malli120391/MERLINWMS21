<%@ Page Title="SO List :." Language="C#" MasterPageFile="~/mOrders/OrdersMaster.master" AutoEventWireup="true" CodeBehind="SOList.aspx.cs" Inherits="MRLWMSC21.mOrders.SOList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="OrdersContent" runat="server">
    <link rel="stylesheet" type="text/css" href="../App_Themes/Orders/Orders_Style.css" media="screen" />

    <script>

        $(document).ready(function () {
            $("#<%= this.txtTenant.ClientID %>").focusout(function () {
                if ($("#<%= this.txtTenant.ClientID %>").val() == '') {
                    $("#<%= this.txtTenant.ClientID %>").val('Tenant...');
                    $("#<%= this.hifTenant.ClientID %>").val('1');

                }
            })
            
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


            var textfielname = $('#<%=this.SoNumberSearch.ClientID%>');
            DropdownFunction(textfielname);
            $('#<%=this.SoNumberSearch.ClientID%>').autocomplete({

                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadSONumbersList") %>',
                            data: "{ 'prefix': '" + request.term + "','IsToolItem':'0','TenentID':'" +$('#<%=hifTenant.ClientID%>').val() +"'}",
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

            })

    </script>
    <style>
        .gvLightSkyBlueNew_pager span {
            background: var(--sideNav-bg) !important;
            color: #fff !important;
        }

        .gvLightSkyBlueNew_pager a {
            background: #fff !important;
            color: #0e0e0e !important;
        }
    </style>
     <!-- Globalization Tag is added for multilingual  -->
    <div class="container">
        <table align="center" class="" cellpadding="5" cellspacing="5">
            <tr class="ListHeaderRow">

                <td class="FormLabels" align="right">

                    <asp:Panel ID="pnlSOList" runat="server" DefaultButton="lnkGetData">
                        <div class="row">
                            <div class="col m3 s4">
                                <div class="flex">
                                    <asp:TextBox ID="txtTenant" runat="server" required="" />
                                    <asp:HiddenField runat="server" ID="hifTenant" Value="0" />
                                    <label><%= GetGlobalResourceObject("Resource", "Tenant")%> </label>
                                </div>
                            </div>
                            <div class="col m3 s4">
                                <div class="flex">
                                    <asp:DropDownList ID="ddlSelectStatus" runat="server" required="" />
                                    <label><%= GetGlobalResourceObject("Resource", "OutwardStatus")%></label>
                                </div>
                            </div>
                            <div class="col m3 s4">
                                <div class="flex">
                                    <asp:TextBox SkinID="txt_Hidden_Req_Auto" runat="server" ID="SoNumberSearch"  required=""></asp:TextBox>
                                    <label><%= GetGlobalResourceObject("Resource", "OutwardNumber")%></label>
                                </div>
                            </div>
                            <div class="col m3 s4 offset-s8 p0">
                                <gap5></gap5>
                                <flex><asp:LinkButton ID="lnkGetData" CssClass="btn btn-primary" runat="server" OnClick="lnkGetData_Click">
                                     <%= GetGlobalResourceObject("Resource", "Search")%>   <%=MRLWMSC21Common.CommonLogic.btnfaSearch %>
                                </asp:LinkButton>

                                <asp:LinkButton ID="lnkadd" runat="server" PostBackUrl="~/mOrders/SalesOrderInfo.aspx" CssClass="btn btn-primary">
                                                <%= GetGlobalResourceObject("Resource", "Add")%>  <%=MRLWMSC21Common.CommonLogic.btnfaNew %>
                                </asp:LinkButton>
                          
                                <asp:LinkButton ID="imgbtngvSOList" CssClass="btn btn-primary" runat="server" OnClick="imgbtngvSOList_Click"> <%= GetGlobalResourceObject("Resource", "Export")%> <%=MRLWMSC21Common.CommonLogic.btnfaExcel %></asp:LinkButton></flex>
                            </div>
                        </div>
                    </asp:Panel>

                </td>


            </tr>
            <tr class="ListDataRow">
                <td>
                    <asp:GridView SkinID="gvLightSkyBlueNew" ID="gvSOList" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" PagerSettings-Position="Bottom" AllowPaging="true" PageSize="25" OnPageIndexChanging="gvSOList_PageIndexChanging" AllowSorting="True" OnRowDeleting="gvSOList_RowDeleting" OnRowDataBound="gvSOList_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,OutwardNumber%>" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="140" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="SONumber" Text='<%# DataBinder.Eval(Container.DataItem, "SONumber").ToString() %>' />
                                    <asp:Literal runat="server" ID="Literal1" Text="[" />
                                    <asp:Literal runat="server" ID="Literal2" Text='<%# DataBinder.Eval(Container.DataItem, "LineItemCount").ToString() %>' />
                                    <asp:Literal runat="server" ID="Literal3" Text="]" />
                                    <asp:Literal runat="server" ID="SOHeaderID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "SOHeaderID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                               <asp:TemplateField HeaderText="<%$Resources:Resource,Account%>" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="140" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="ItAccName" ToolTip='<%# Bind("Account") %>' Text='<%# DataBinder.Eval(Container.DataItem, "AccountCode") %>' />
                                    <%--  <asp:Literal runat="server" ID="ltTenant" Text='<%# DataBinder.Eval(Container.DataItem, "TenantName").ToString() %>' />--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,Tenant%>" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="140" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="ItCompanyName" ToolTip='<%# Bind("TenantName") %>' Text='<%# DataBinder.Eval(Container.DataItem, "TenantCode") %>' />
                                    <%--  <asp:Literal runat="server" ID="ltTenant" Text='<%# DataBinder.Eval(Container.DataItem, "TenantName").ToString() %>' />--%>
                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="<%$Resources:Resource,Customer%>" ItemStyle-Width="130" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="ItCompanyName" ToolTip='<%# Bind("CustomerName") %>' Text='<%# DataBinder.Eval(Container.DataItem, "CustomerCode") %>' />
                                    <%-- <asp:Literal runat="server" ID="SOCustomerName" Text='<%# DataBinder.Eval(Container.DataItem, "CustomerName") %>' />--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,ProjectCode%>" ItemStyle-Width="130" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                <ItemTemplate>
                                    <!--swamy-->
                                    <asp:Literal runat="server" ID="ltProjectCode" Text='<%# DataBinder.Eval(Container.DataItem, "ProjectCode") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,OrderDate%>" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="90" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="ltSODate" Text='<%#DataBinder.Eval(Container.DataItem, "SODate","{0:dd-MMM-yyyy}") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="<%$Resources:Resource,OutwardType%>" ItemStyle-Width="90" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="SOType" Text='<%# DataBinder.Eval(Container.DataItem, "SOType") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,OBDNumbers%>">
                                <ItemStyle Width="" />
                                <ItemTemplate>
                                    <div style="width: 55px; word-wrap: break-word;">
                                        <asp:Literal ID="ltobdnumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "OBDNumbers") %>' /></div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,Status%>" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="150" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="SOStatus" Text='<%# DataBinder.Eval(Container.DataItem, "StatusName") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100" HeaderText="<%$Resources:Resource,Edit%>">
                                <ItemTemplate>
                                    <a style="text-decoration: none;" id='<%#"SODID_"+DataBinder.Eval(Container.DataItem, "SOHeaderID").ToString() %>' href='<%# String.Concat("SalesOrderInfo.aspx?soid=",DataBinder.Eval(Container.DataItem, "SOHeaderID").ToString()+"&tid="+DataBinder.Eval(Container.DataItem, "TenantID")) %>'>
                                        <i class="material-icons ss">mode_edit</i>
                                    </a>

                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                        <EmptyDataTemplate>
                            <div align="center">No Data Found</div>
                        </EmptyDataTemplate>

                    </asp:GridView>
                    <br />
                </td>
            </tr>

        </table>
    </div>
</asp:Content>
