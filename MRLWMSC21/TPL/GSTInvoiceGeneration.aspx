<%@ Page Title="GST Invoice Generation" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GSTInvoiceGeneration.aspx.cs" Inherits="MRLWMSC21.TPL.GSTInvoiceGeneration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .gvLightBlueNew {
            border: 0px !important;
            border-radius: 0px !important;
          
        }

        .gvLightBlueNew_headerGrid {
            background-color: #fff !important;
            text-align: left;
            color: #000 !important;
        }

        .gvLightBlueNew_DataCellGridAlt:hover, .gvLightBlueNew_DataCellGrid:hover, .gvLightBlueNew_DataCellGridAlt {
            background-color: #fff !important;
        }

        .gvLightBlueNew_pager span {
            border: 2px solid var(--sideNav-bg) !important;
            background-color: var(--sideNav-bg) !important;
            color: #fff !important;
        }

        .internalData td, th {
            padding: 7px
        }

        a.ButEmpty {
            font-size: 13pt;
            font-weight: bold;
            padding-left: 5px;
            padding-right: 10px;
            padding-top: 5px;
            padding-bottom: 5px;
            /*box-shadow: var(--z1);*/
            margin: 1px;
            display: inline-block;
            background: var(--sideNav-bg);
            font-weight: normal;
            margin: auto;
            font-size: 14px;
            text-decoration: none;
            padding: 8px;
            text-align: center;
            color: #fff !important;
        }
    </style>
<!--breadcrumb-->
    <div class="module_yellow">
        <div class="ModuleHeader">
            <div>
                <a href="../Default.aspx">Home</a> <i class="material-icons">arrow_right</i> 
                 <span>Reports</span> <i class="material-icons">arrow_right</i> 
                 <span class="breadcrumbd" contenteditable="false">GSTInvoiceGeneration</span>
            </div>
        </div>
    </div>
<!--ends-breadcrumb-->    
 <!-- Globalization Tag is added for multilingual  -->
<div class="container">
    <table width="100%" class="">

        <tr>
            <td>
                <asp:GridView ID="gvInvoice" runat="server" SkinID="gvLightBlueNew" AllowPaging="true" PageSize="25" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
                    OnPageIndexChanging="gvInvoice_PageIndexChanging"
                    OnRowCommand="gvInvoice_RowCommand">
                    <Columns>

                      <%--  <asp:TemplateField HeaderText="Customer Name" ItemStyle-Width="150" HeaderStyle-HorizontalAlign="Center">--%>
                          <asp:TemplateField HeaderText="<%$Resources:Resource,CustomerName%>" ItemStyle-Width="150" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal ID="ltTenantID" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem,"TenantID") %>' />
                                <asp:Literal ID="ltTenantName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TenantName") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>


                      <%--  <asp:TemplateField HeaderText="Company" ItemStyle-Width="120" HeaderStyle-HorizontalAlign="Center">--%>
                          <asp:TemplateField HeaderText="<%$Resources:Resource,Company%>"  ItemStyle-Width="120" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal ID="ltCompanyDBA" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"CompanyDBA") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                      <%--  <asp:TemplateField HeaderText="Billing Type" ItemStyle-Width="100" HeaderStyle-HorizontalAlign="Center">--%>
                          <asp:TemplateField HeaderText="<%$Resources:Resource,BillingType%>" ItemStyle-Width="100" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal ID="ltBillingType" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"BillName") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>


                     <%--   <asp:TemplateField HeaderText="Currency" ItemStyle-Width="150" HeaderStyle-HorizontalAlign="Center">--%>
                           <asp:TemplateField HeaderText="<%$Resources:Resource,Currency%>" ItemStyle-Width="150" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal ID="ltCurrency" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Curreny") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                       <%-- <asp:TemplateField HeaderText="Invoice Period" ItemStyle-Width="180" HeaderStyle-HorizontalAlign="Center">--%>
                         <asp:TemplateField HeaderText="<%$Resources:Resource,InvoicePeriod%>" ItemStyle-Width="180" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal ID="ltPeriodTxt" runat="server" Text=' <%#DataBinder.Eval(Container.DataItem,"InvPeriod") %>  ' />
                            </ItemTemplate>
                        </asp:TemplateField>


                       <%-- <asp:TemplateField HeaderText="Generate Invoice" Visible="false" ItemStyle-Width="50" HeaderStyle-HorizontalAlign="Center">--%>
                         <asp:TemplateField HeaderText="<%$Resources:Resource,GenerateInvoice%>" Visible="false" ItemStyle-Width="50" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkinvoice" runat="server" CssClass="ButEmpty" CommandName="InvoiceGeneration" CommandArgument='<%# String.Format("{0}`{1}`{2}",DataBinder.Eval(Container.DataItem, "TenantID"),DataBinder.Eval(Container.DataItem,"EffectiveFrom","{0:yyyy-MM-dd}"),DataBinder.Eval(Container.DataItem,"EffectiveTo","{0:yyyy-MM-dd}"))%>' Text="View" />
                            </ItemTemplate>
                        </asp:TemplateField>

                       <%-- <asp:TemplateField HeaderText="View Bill" ItemStyle-Width="80" HeaderStyle-HorizontalAlign="Center">--%>
                         <asp:TemplateField HeaderText="<%$Resources:Resource,ViewBill%>"  ItemStyle-Width="80" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBill" runat="server" CssClass="" CommandName="ViewBill" CommandArgument='<%# String.Format("{0}`{1}`{2}",DataBinder.Eval(Container.DataItem, "TenantID"),DataBinder.Eval(Container.DataItem,"EffectiveFrom","{0:yyyy-MM-dd}"),DataBinder.Eval(Container.DataItem,"EffectiveTo","{0:yyyy-MM-dd}"))%>' Text="<nobar><img style='vertical-align: middle; width: 22px; margin-left: 5px;' title='view bill' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACgAAAAoCAYAAACM/rhtAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAGkSURBVFhH7Zi7SsRAGIUDguANEe0EsR5IIrloCIqPIAsbEGux8y1EX8BG8AEEbbWRrUQrwVIsLHwAWQtNY6Fn4B9YQy6TTMwIzoFDdv7rt2Fhs2sZGbWkOI5nbNueKzJjbJxKu5fv+2fwV5k9z0uDINiklm4FgGEWqMCH1NKtcHc2sPwYPhFG7ALXH4CIHVGLXoVhOA+Yhz8JmIXD66dGgGjoo+ES13sFD1zXXaSReXfuDmdbnKUBUbwlmhT8hoVrNDIXDp6Fl0RMGhCF59TwCT838CNcCcdzuDYCvKGGFwo1VhkcF17rA6yC48J5EjUp5fcpXK42AGXghBBfRe1ukiRjFCqXKmAVHP9KQ96jY32pAErCfSC2TqH6agpYBYf8CsHxXLeAMp855HdG8t0BysBxaQNE3dXI4lw4Lp2At7S0EI5LGyBqF1Dbi6JogkK50gYoKwOoKgOoKgOoqn8FeE2Ar/Beiz4VgHjsimldfWHQgRj0G8b8lP9hROvqizE2jUGD7OCW/I67t02r1IQf3ct4t35b5g+sjuNM0XgjIyN1WdY3JVkrPNoFVBgAAAAASUVORK5CYII='></nobar>" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
						<%--<div align="center">No Data Found</div>--%>
                        <div align="center"> <%= GetGlobalResourceObject("Resource", "NoDataFound")%></div>
					</EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
    </table>
    </div>
</asp:Content>
