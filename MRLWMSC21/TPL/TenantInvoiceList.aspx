<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TenantInvoiceList.aspx.cs" Inherits="MRLWMSC21.TPL.TenantInvoiceList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
      <style>
          .gvLightBlueNew {
            border: 0px !important;
            border-radius: 0px !important;
            background-color: #b2bbc8 !important;
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
              border-radius: 30px;
              margin: auto;
              font-size: 14px;
              text-decoration: none;
              padding: 8px;
              text-align: center;
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
    <div class="dashed"></div> 
    <table width="100%" class="internalData">
        <tr>
             <td>
                <asp:GridView ID="gvInvoice" runat="server" SkinID="gvLightBlueNew" AllowPaging="true" PageSize="20" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" OnPageIndexChanging="gvInvoice_PageIndexChanging">
                    <Columns>

                        <asp:TemplateField HeaderText="Tenant Name" ItemStyle-Width="150" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                
                                <asp:Literal ID="ltTenantName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"CompanyName") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        
                        <asp:TemplateField HeaderText="Invoice Date" ItemStyle-Width="120" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal ID="ltCompanyDBA" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"CreatedOn") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Invoice number" ItemStyle-Width="100" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                 <%-- <asp:HyperLink ID="HyperLink1" Text='<%#DataBinder.Eval(Container.DataItem,"InvoiceNumber") %>'  NavigateUrl='<%#  String.Format("DisplayInvoice.aspx?fid=",DataBinder.Eval(Container.DataItem, "FileURL").ToString())  %>' Font-Underline="false" runat="server"></asp:HyperLink>--%>
                                 <a style="text-decoration:none;"   href='<%# String.Concat("DisplayInvoice.aspx?fid=",DataBinder.Eval(Container.DataItem, "FileURL").ToString()) %>'><%#DataBinder.Eval(Container.DataItem,"InvoiceNumber") %></a>
                                
                            </ItemTemplate>
                        </asp:TemplateField>
                        

                        <asp:TemplateField HeaderText="Invoice period From" ItemStyle-Width="150" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal ID="ltCurrency" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"FromDate") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Invoice Period To" ItemStyle-Width="180" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal ID="ltPeriodTxt" runat="server" Text=' <%#DataBinder.Eval(Container.DataItem,"Todate") %>  '/>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>
