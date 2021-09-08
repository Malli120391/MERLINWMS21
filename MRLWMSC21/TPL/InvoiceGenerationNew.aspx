<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InvoiceGenerationNew.aspx.cs" Inherits="MRLWMSC21.TPL.InvoiceGenerationNew" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="dashed"></div>
<div class="pagewidth">
    <table width="100%" class="">
        <tr>
            <td>
                <span class="SubHeading"> TPL Billing Queue</span>
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="gvInvoiceNew" runat="server" SkinID="gvLightBlueNew" AllowPaging="true" PageSize="15" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
                    OnPageIndexChanging="gvInvoiceNew_PageIndexChanging"
                    OnRowCommand="gvInvoiceNew_RowCommand" >
                    <Columns>

                        <asp:TemplateField HeaderText="Customer Name" ItemStyle-Width="150" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal ID="ltTenantID" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem,"TenantID") %>' />
                                <asp:Literal ID="ltTenantName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TenantName") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        
                        <asp:TemplateField HeaderText="Company" ItemStyle-Width="120" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal ID="ltCompanyDBA" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"CompanyDBA") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Billing Type" ItemStyle-Width="100" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal ID="ltBillingType" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"BillName") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        

                        <asp:TemplateField HeaderText="Currency" ItemStyle-Width="150" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal ID="ltCurrency" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Curreny") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Invoice Period" ItemStyle-Width="180" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal ID="ltPeriodTxt" runat="server" Text=' <%#DataBinder.Eval(Container.DataItem,"InvPeriod") %>  '/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        
                        <asp:TemplateField HeaderText="Generate Invoice" Visible="false" ItemStyle-Width="50" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkinvoice" runat="server" CssClass="ButEmpty" CommandName="InvoiceGeneration" CommandArgument='<%# String.Format("{0}`{1}`{2}",DataBinder.Eval(Container.DataItem, "TenantID"),DataBinder.Eval(Container.DataItem,"EffectiveFrom","{0:yyyy-MM-dd}"),DataBinder.Eval(Container.DataItem,"EffectiveTo","{0:yyyy-MM-dd}"))%>'  Text="View" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="View Bill" ItemStyle-Width="80" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBill" runat="server" CssClass="ButEmpty" CommandName="ViewBill" CommandArgument='<%# String.Format("{0}`{1}`{2}",DataBinder.Eval(Container.DataItem, "TenantID"),DataBinder.Eval(Container.DataItem,"EffectiveFrom","{0:yyyy-MM-dd}"),DataBinder.Eval(Container.DataItem,"EffectiveTo","{0:yyyy-MM-dd}"))%>'  Text="View Bill" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
    </div>
</asp:Content>
