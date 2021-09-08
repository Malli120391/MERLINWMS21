<%@ Page Title="" Language="C#" MasterPageFile="~/mMaterialManagement/MaterialManagementMaster.master" AutoEventWireup="true" CodeBehind="KitOrdersList.aspx.cs" Inherits="MRLWMSC21.mMaterialManagement.KitOrdersList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MMContent" runat="server">
   <div class="dashed"></div>
    <div class="pagewidth">
        <div style="float:right;padding-bottom:20px; !important;">
            <asp:LinkButton ID="lnkAddNew" runat="server" PostBackUrl="~/mMaterialManagement/KitOrderDetails.aspx"  CssClass="btn btn-primary">Add New <%= MRLWMSC21Common.CommonLogic.btnfaNew %></asp:LinkButton>
        </div>
        <br /><br />
    <table>
        <tr>
            <td>
                <asp:GridView SkinID="gvLightSkyBlueNew"    ID="KitList" runat="server" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"  PagerSettings-Position="Bottom"  AllowPaging="true" PageSize="25" AllowSorting="True"  HorizontalAlign="Left"   OnSorting="JobListList_Sorting" OnPageIndexChanging="JobList_PageIndexChanging"  Width="899px" >
                                            <Columns>

                                               

                                                <asp:TemplateField ItemStyle-Width="140" HeaderText="Job Order Ref. No."  ItemStyle-HorizontalAlign="left" >
                                                    <ItemTemplate>
              
                                                        <asp:Literal runat="server" ID="ItRefNo" Text='<%# DataBinder.Eval(Container.DataItem, "KitJobOrderRefNo")  %>'/>
                                                        <asp:Literal runat="server" ID="ItHeaderID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "KitJobOrderHeaderID") %>'/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-Width="150"  HeaderText="Kit Type"  ItemStyle-HorizontalAlign="left">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="ltKitType" Text='<%# DataBinder.Eval(Container.DataItem, "KitType") %>'/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField ItemStyle-Width="150"  HeaderText="Status" ItemStyle-HorizontalAlign="left" >
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="ltPOtype" Text='<%# DataBinder.Eval(Container.DataItem, "KitJobOrderStatus") %>'/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                               
                                                <asp:TemplateField ItemStyle-Width="90" HeaderText="Created Date" Visible="false" >
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="ltCoordinator" Text='<%# DataBinder.Eval(Container.DataItem, "CreatedOn").ToString() %>'/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 
                                               
                                                <asp:TemplateField ItemStyle-Width="150" HeaderText="Created User" ItemStyle-HorizontalAlign="left">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="ltStatus" Text='<%# DataBinder.Eval(Container.DataItem, "FirstName") %>'/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                               
                                                    <asp:TemplateField ItemStyle-Width="80" HeaderText="Edit" ControlStyle-CssClass="txtCenteralign">
                                                    <ItemTemplate>
                                                        <a style="text-decoration:none;" id='<%#"PODID_"+DataBinder.Eval(Container.DataItem, "KitJobOrderHeaderID").ToString() %>' href='<%# String.Concat("KitOrderDetails.aspx?jobid=",DataBinder.Eval(Container.DataItem, "KitJobOrderHeaderID").ToString()) %>' > <i class="material-icons ss">mode_edit</i></a>

                                                       
                                                        
                                                    </ItemTemplate>
                            
                                                      
                                                </asp:TemplateField>
                                             
                                
                                            </Columns>
                                            <PagerSettings FirstPageText="&amp;lt;&amp;lt;First Page" LastPageText="&amp;gt;&amp;gt;"
                                                Mode="NumericFirstLast" PageButtonCount="15" />
                                     
                                           
                                    </asp:GridView>
            </td>
        </tr>
    </table>
        </div>
</asp:Content>
