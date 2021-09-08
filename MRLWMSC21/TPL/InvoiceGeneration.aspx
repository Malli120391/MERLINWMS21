<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InvoiceGeneration.aspx.cs" Inherits="MRLWMSC21.TPL.InvoiceGeneration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .gvLightBlueNew {
            border: 0px !important;
            border-radius: 0px !important;
        }
        .gvLightBlueNew_headerGrid {
            background-color: #455b7c;
            text-align: left;
            color: #ffffff;
        }

        .internalData td, th {
            padding:7px
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
              margin: auto;
              font-size: 14px;
              text-decoration: none;
              padding: 8px;
              text-align: center;
              font-weight: normal;
          }
          .gvLightBlueNew_DataCellGridAlt:hover, .gvLightBlueNew_DataCellGrid:hover
          {
              background-color: #fff !important;
          }
          .gvLightBlueNew_headerGrid{
              background: #fff;
          }
          .gvLightBlueNew_DataCellGridAlt{
              background-color:#fff !important;
          }
          .gvLightBlueNew_headerGrid{
              color: #000 !important;
          }
          .gvLightBlueNew_pager span {
            border: 2px solid var(--sideNav-bg) !important;
            background-color: var(--sideNav-bg) !important;
            color: #fff !important;
        }
    </style>
   <table class="tbsty">
        <tbody>
            <tr class="module_yellow">
               <td class="ModuleHeader fixed-width">
                    Invoice Generation
                </td>
             </tr>
        </tbody>
    </table>
    <bread-crumb parent="TPL" child="Invoice Generation"></bread-crumb>
    <div class="dashed"></div>
<%--<div class="pagewidth">
    <table width="100%" class="">
      
        <tr>
            <td>
                <asp:GridView ID="gvInvoice" runat="server" SkinID="gvLightBlueNew" AllowPaging="true" PageSize="15" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
                    OnPageIndexChanging="gvInvoice_PageIndexChanging"
                    OnRowCommand="gvInvoice_RowCommand" >
                    <Columns>
                        <asp:TemplateField HeaderText="Company Name">
                            <ItemTemplate>
                                <asp:Literal ID="ltTenantID" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem,"TenantID") %>' />
                                <asp:Literal ID="ltCompanyDBA" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TenantName") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="From Date">
                            <ItemTemplate>
                                <asp:Literal ID="ltFromDate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"EffectiveFrom","{0:dd/MM/yyyy}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="To Date">
                            <ItemTemplate>
                                <asp:Literal ID="ltToDate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"EffectiveTo","{0:dd/MM/yyyy}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Generate Invoice">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkinvoice" runat="server" CssClass="ButEmpty btn btn-primary btn-sm" CommandName="InvoiceGeneration" CommandArgument='<%# String.Format("{0}`{1}`{2}",DataBinder.Eval(Container.DataItem, "TenantID"),DataBinder.Eval(Container.DataItem,"EffectiveFrom","{0:yyyy-MM-dd}"),DataBinder.Eval(Container.DataItem,"EffectiveTo","{0:yyyy-MM-dd}"))%>'  Text="Generate" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
    </div>--%>

    <div class="container">
    <div class="row">
        <div class="col m12">
             <asp:GridView ID="gvInvoice" runat="server" SkinID="gvLightBlueNew" AllowPaging="true" PageSize="15" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
                    OnPageIndexChanging="gvInvoice_PageIndexChanging"
                    OnRowCommand="gvInvoice_RowCommand" >
                    <Columns>
                        <asp:TemplateField HeaderText="Company Name">
                            <ItemTemplate>
                                <asp:Literal ID="ltTenantID" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem,"TenantID") %>' />
                                <asp:Literal ID="ltCompanyDBA" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"TenantName") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Billing Type">
                            <ItemTemplate>
                                <asp:Literal ID="ltBillType" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"BillName") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Currency">
                            <ItemTemplate>
                                <asp:Literal ID="ltCurrency" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Currency") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Invoice Period">
                            <ItemTemplate>
                                <asp:Literal ID="ltInvoicePeriod" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"InvPeriod")%>'/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="View Bill" ItemStyle-Width="80" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton style="color:white !important;font-size:12px;" ID="lnkBill" runat="server" CssClass="ButEmpty" CommandName="ViewBill" CommandArgument='<%# String.Format("{0}`{1}`{2}",DataBinder.Eval(Container.DataItem, "TenantID"),DataBinder.Eval(Container.DataItem,"EffectiveFrom","{0:yyyy-MM-dd}"),DataBinder.Eval(Container.DataItem,"EffectiveTo","{0:yyyy-MM-dd}"))%>'  Text="View Bill" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
        </div>
    </div>
    </div>
</asp:Content>
