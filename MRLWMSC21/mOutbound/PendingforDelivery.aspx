<%@ Page Title=" .: Pending for Delivery :. " Language="C#" MasterPageFile="~/mOutbound/OutboundMaster.master" AutoEventWireup="true" CodeBehind="PendingforDelivery.aspx.cs" Inherits="MRLWMSC21.mOutbound.PendingforDelivery" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="OBContent" runat="server">

    <script type="text/javascript" src="../Scripts/jQuery2/countdown/jquery.countdown.js"></script>

  <script  type="text/javascript" >
      $(function () {

          var activeIndex = parseInt($('#<%=hidAccordionIndex.ClientID %>').val());

            $("#accordion").accordion({
                autoHeight: false, clearStyle: true,
                active: activeIndex,
                change: function (event, ui) {
                    var index = $(this).children('h3').index(ui.newHeader);
                    $('#<%=hidAccordionIndex.ClientID %>').val(index);
                 }

             });
            $("#accordion").accordion({ header: 'h3', collapsible: true, autoHeight: false, clearStyle: true, navigation: true });
            $("#accordion").accordion("option", "icons", { 'header': 'defaultIcon', 'headerSelected': 'selectedIcon' });


        });
    </script>




    <style type="text/css">
        .ui-autocomplete-loading {
             background: white url('../Images/ui-anim_basic_16x16.gif') right center no-repeat;
         }
    </style>
    <div class="dashed"></div>
    <div class="dashed">
     <table border="0" cellspacing="4" cellpadding="0" align="center">

        <tr>
            <td>
                <br /><br />
            </td>
        </tr>
        <tr>
            <td align="center">

                 <div id="accordion" align="left">

                     <h3>Deliveries Pending   <span id="lblDelvPRecCount" runat="server" />   </h3>
                     <div>
                         <!-- Deliveries Pending -->

                         <table border="0" width="100%" cellpadding="0" cellspacing="0" align="center">

                             <tr>
                                 <td align="right">
                                     <br />
                                     Delv. Doc. Number:
                                     <asp:TextBox ID="txtDelvPOBDNumber" runat="server" Width="200" CssClass="txt_Blue_Small" ></asp:TextBox>
                                     <asp:LinkButton runat="server" Text="Search" SkinID="lnkButEmpty" ID="lnkDelvpSearch" OnClick="lnkDelvpSearch_Click"></asp:LinkButton>
                                                                          <asp:ImageButton ID="imgbtngvOBDReceived" runat="server"  ImageAlign="Right" ImageUrl="../Images/excel_icon.jpg" Width="20" OnClick="imgbtngvOBDReceived_Click" ToolTip="Export To Excel" />
                                     <br />
                                 </td>
                             </tr>
                             <tr>
                                 <td class="FormLabels">
                                     <br />
                                     <asp:Label ID="lblDelvPStatus" runat="server"></asp:Label>
                                 </td>
                             </tr>
                             
                             <tr>   
                              <td>

                                     <asp:GridView Width="100%" ShowFooter="true"   GridLines="None" CellPadding="1" CellSpacing="1"  ID="gvOBDReceived" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="true" PageSize="10" AllowSorting="True" SkinID="gvLightGreen" HorizontalAlign="Left" OnRowDataBound="gvOBDDeliveryPending_OnRowDataBound"   OnSorting="gvOBDDeliveryPending_Sorting" OnPageIndexChanging="gvOBDDeliveryPending_PageIndexChanging" >
    								  
                                            <Columns>
                                             <asp:TemplateField ItemStyle-Width="50"   ItemStyle-CssClass="gvOBDNumber" ItemStyle-HorizontalAlign="Right">
                                             
                                                                   <ItemTemplate>
                                                                    <asp:Literal ID="ltPriorityLevel" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PriorityLevel") %>'/>
                                                                    <asp:Literal ID="ltPriorityTime"   Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PriorityDateTime") %>'/>
                                                                    </ItemTemplate>                                   
                                                 </asp:TemplateField>
                                               
                                                <asp:TemplateField ItemStyle-Width="150" HeaderText="Delv.Doc.No." HeaderStyle-HorizontalAlign="Left"   ItemStyle-CssClass="gvOBDNumber">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="ltOBDNumber_Pending" Text='<%# DataBinder.Eval(Container.DataItem, "OBDNumber") %>'/>
                                                        <asp:Label runat="server" ID="lblOBDID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "OutboundID") %>'></asp:Label>
                                                        <asp:Label runat="server" ID="lblLineItemCount" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "LineCount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                  <asp:TemplateField ItemStyle-Width="150" HeaderText="Delv. Type" HeaderStyle-HorizontalAlign="Left"  ItemStyle-CssClass="home">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="ltDelvType" Text='<%# DataBinder.Eval(Container.DataItem, "DocumentType").ToString() %>'/>
                                                        <asp:Literal runat="server" ID="ltDocTypeID" Visible="false"  Text='<%# DataBinder.Eval(Container.DataItem, "DocumentTypeID") %>'/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                
                                                <asp:TemplateField ItemStyle-Width="150" HeaderText="Delv. Doc. Date" HeaderStyle-HorizontalAlign="Left"  ItemStyle-CssClass="home">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="ltOBDDate_Pending" Text='<%# DataBinder.Eval(Container.DataItem, "OBDDate","{0: dd/MM/yy}") %>'/>
                                                    </ItemTemplate>
                                                    
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField ItemStyle-Width="250" HeaderText="Customer Name" HeaderStyle-HorizontalAlign="Left"  ItemStyle-CssClass="home">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="ltCustomerName" Text='<%# DataBinder.Eval(Container.DataItem, "CustomerName") %>'/>
                                                    </ItemTemplate>
                                                    
                                                </asp:TemplateField> 
                                                    
                                                    <asp:TemplateField ItemStyle-Width="250" HeaderText="Requested By" HeaderStyle-HorizontalAlign="Left"  ItemStyle-CssClass="home">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="ltRequestedBy" Text='<%# DataBinder.Eval(Container.DataItem, "RequestedBy").ToString() %>'></asp:Literal>
                                                    </ItemTemplate>
                                                   </asp:TemplateField>  
                                                    <asp:TemplateField HeaderStyle-Height="10px" ItemStyle-Width="250" HeaderText="Sent to Delivery"  ItemStyle-CssClass="home">
                                                    <HeaderTemplate >
                                                        <table align="center" cellpadding="0" cellspacing="0" height="10px"  ><tr> <td colspan="2" align="center"><asp:LinkButton ID="lnkSortByPGIDate" Font-Underline="false" runat="server" CommandName="Sort" CommandArgument="PGIDone_DT" Text="Sent to Delivery"/></td></tr> 
                                                        <tr><td width="80px" align="center">Date</td><td width="80px" align="center" >Time</td></tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                     <ItemTemplate>
                                                     <table ><tr>
                                                     <td width="80px" align="center"><asp:Literal runat="server" ID="ltGTSDate" Text='<%# DataBinder.Eval(Container.DataItem,"PGIDoneOn","{0:dd/MM/yy}") %>'/></td>
                                                     <td width="80px" align="center"><asp:Literal runat="server" ID="ltOGTSTime" Text='<%# DataBinder.Eval(Container.DataItem,"PGIDoneOn","{0:hh:mm tt}") %>'/></td>
                                                     </tr></table>
                                                        
                                                    </ItemTemplate>
                                                    </asp:TemplateField>
                                                    
                                                    <asp:TemplateField ItemStyle-Width="200" HeaderText="Store(s)"  ItemStyle-CssClass="home">
                                                     <ItemTemplate>
                                                      
                                                      <asp:Literal runat="server" ID="ltStores" Text='<%# MRLWMSC21Common.CommonLogic.GetStoreNamesWithDeliveryStatus(DataBinder.Eval(Container.DataItem, "ReferedStores").ToString(),"<br/>",DataBinder.Eval(Container.DataItem, "OBDNumber").ToString(), DataBinder.Eval(Container.DataItem,"OutboundID").ToString() ) %>'/>
                                                    </ItemTemplate>
                                                    </asp:TemplateField>
                                                    
                                                       
                                                         <asp:TemplateField  ItemStyle-CssClass="home"  HeaderText="P.Note" ItemStyle-Width="300"  >
                                                     <ItemTemplate>
                                                      <asp:HyperLink runat="server" ID="lnkHyperLink" ToolTip="Pick Note | Pick note with barcoded material codes to pick items for delivery."  Text="Pick Note" Font-Underline="false" ForeColor="Red"></asp:HyperLink>
                                                           
                                                         <asp:Literal runat="server" ID="ltLineCount" Text='<%# String.Format("[{0}]",DataBinder.Eval(Container.DataItem, "LineCount"))%>'></asp:Literal>
                                                         <img src='../Images/redarrowright.gif' border='0' />
                                                    </ItemTemplate>
                                                    </asp:TemplateField>
                                                    
                                                    
                                                     <asp:TemplateField  ItemStyle-CssClass="home" ItemStyle-Width="80">
                                                     <ItemTemplate>
                                                      <asp:LinkButton ID="lnkReceivedelivery" runat="server" Visible="false" CssClass="GvLink" PostBackUrl ='<%# Eval("OutboundID", "OutboundDetails.aspx?obdid={0}&edittype=4") %>'   Text="Receive" ToolTip="Receive for PGI" />
                                                      

                                                         <asp:HyperLink ID="HyperLink1" Text="<nobr> Receive <img src='../Images/redarrowright.gif' border='0' /></nobr>" NavigateUrl='<%#   Eval("OutboundID", "OutboundDetails.aspx?obdid={0}&edittype=4")  %>' Font-Underline="false" runat="server" ></asp:HyperLink>
                                                    </ItemTemplate>
                                                   
                                                    </asp:TemplateField>
                                            </Columns>
                                    </asp:GridView>


                                 </td>
                             </tr>

                         </table>

                         <!-- Deliveries Pending -->
                     </div>


                      <h3>POD Pending   <span id="lblPODRecCount" runat="server" />   </h3>
                     <div>
                         <!-- POD Pending -->

                         <table border="0" width="100%" cellpadding="0" cellspacing="0" align="center">

                             <tr>
                                 <td align="right">
                                     <br />
                                     Delv. Doc. Number:
                                     <asp:TextBox ID="txtPODDelDocNo" runat="server" Width="200" CssClass="txt_Blue_Small" ></asp:TextBox>
                                     <asp:LinkButton runat="server" Text="Search" SkinID="lnkButEmpty" ID="lnlPODSearch" OnClick="lnlPODSearch_Click"></asp:LinkButton>
                                     <asp:ImageButton ID="imgbtngvDCRPending" runat="server"  ImageAlign="Right" ImageUrl="../Images/excel_icon.jpg" Width="20" OnClick="imgbtngvDCRPending_Click" ToolTip="Export To Excel" />
                                     <br />
                                 </td>
                             </tr>
                             <tr>
                                 <td class="FormLabels">
                                     <br />
                                     <asp:Label ID="lblPODStatus" runat="server"></asp:Label>
                                 </td>
                             </tr>
                             <tr>
                                 <td>

                                     <asp:GridView Width="100%" ShowFooter="true"   GridLines="None" CellPadding="1" CellSpacing="1" ID="gvDCRPending" runat="server" PagerStyle-HorizontalAlign="Right" PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" AllowPaging="true" PageSize="10" AllowSorting="True" SkinID="gvIBCyan" HorizontalAlign="Left"     OnSorting="gvDCRPending_Sorting" OnPageIndexChanging="gvDCRPending_PageIndexChanging" OnRowDataBound="gvDCRPending_OnRowDataBound" >
    								  
                                            <Columns>
                                            
                                               
                                                <asp:TemplateField ItemStyle-Width="150" HeaderText="Delv.Doc.No."  ItemStyle-CssClass="gvOBDNumber">
                                                    <ItemTemplate>
                                                    
                                                        <asp:Literal runat="server" ID="ltOBDNumber_Pending" Text='<%# DataBinder.Eval(Container.DataItem, "OBDNumber") %>'/>
                                                         <asp:Label runat="server" ID="lblOBDID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "OutboundID") %>'></asp:Label>
                                                        <asp:Label runat="server" ID="lblLineItemCount" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "LineCount") %>'></asp:Label>
                                                        
                                                        
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                  <asp:TemplateField ItemStyle-Width="150" HeaderText="Delv. Type"  ItemStyle-CssClass="home">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="ltDelvType" Text='<%# DataBinder.Eval(Container.DataItem, "DocumentType").ToString() %>'/>
                                                        <asp:Literal runat="server" ID="ltDocTypeID" Visible="false"  Text='<%# DataBinder.Eval(Container.DataItem, "DocumentTypeID") %>'/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                
                                                <asp:TemplateField ItemStyle-Width="150" HeaderText="Delv. Doc. Date"  ItemStyle-CssClass="home">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="ltOBDDate_Pending" Text='<%# DataBinder.Eval(Container.DataItem, "OBDDate","{0: dd/MM/yy}") %>'/>
                                                    </ItemTemplate>
                                                    
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField ItemStyle-Width="250" HeaderText="Customer Name"  ItemStyle-CssClass="home">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="ltCustomerName" Text='<%# DataBinder.Eval(Container.DataItem, "CustomerName") %>'/>
                                                    </ItemTemplate>
                                                    
                                                </asp:TemplateField> 
                                                    
                                                    <asp:TemplateField ItemStyle-Width="200" HeaderText="Requested By"  ItemStyle-CssClass="home">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="ltRequestedBy" Text='<%# DataBinder.Eval(Container.DataItem, "RequestedBy").ToString() %>'></asp:Literal>
                                                    </ItemTemplate>
                                                    
                                                   </asp:TemplateField>  
                                                   
                                                                                          
                                                   
                                                    <asp:TemplateField HeaderStyle-Height="10px" ItemStyle-Width="200"  ItemStyle-CssClass="home">
                                                    <HeaderTemplate >
                                                        <table align="center" cellpadding="0" cellspacing="0" height="10px"  ><tr> <td colspan="2" align="center" >Sent to Delivery </td></tr> 
                                                        <tr><td width="80px" align="center" >Date</td><td width="80px" align="center">Time</td></tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                     <ItemTemplate>
                                                     <table ><tr>
                                                     <td width="80px" align="center"><asp:Literal runat="server" ID="ltGTSDate" Text='<%# DataBinder.Eval(Container.DataItem,"PGIDoneOn","{0:dd/MM/yy}") %>'/></td>
                                                     <td width="80px" align="center"><asp:Literal runat="server" ID="ltOGTSTime" Text='<%# DataBinder.Eval(Container.DataItem,"PGIDoneOn","{0:hh:mm tt}") %>'/></td>
                                                     </tr></table>
                                                        
                                                    </ItemTemplate>
                                                    </asp:TemplateField>
                                                    
                                                    <asp:TemplateField ItemStyle-Width="200" HeaderText="Store(s)"  ItemStyle-CssClass="home">
                                                     <ItemTemplate>
                                                      
                                                      <asp:Literal runat="server" ID="ltStores" Text='<%# MRLWMSC21Common.CommonLogic.GetStoreNamesWithDeliveryStatus(DataBinder.Eval(Container.DataItem, "ReferedStores").ToString(),"<br/>",DataBinder.Eval(Container.DataItem, "OBDNumber").ToString(), DataBinder.Eval(Container.DataItem,"OutboundID").ToString() ) %>'/>
                                                    </ItemTemplate>
                                                    </asp:TemplateField>
                                                    
                                                      <asp:TemplateField ItemStyle-Width="200" HeaderText="POD Not Rcvd / Delv. Date /Driver" ItemStyle-CssClass="home">
                                                     <ItemTemplate>
                                                      <asp:Literal runat="server" ID="ltDeliveryStatus" Text='<%# MRLWMSC21Common.CommonLogic.DCRUpdate(DataBinder.Eval(Container.DataItem, "DocumentTypeID").ToString(),DataBinder.Eval(Container.DataItem, "DeliveryStatusID").ToString(), DataBinder.Eval(Container.DataItem, "IsPODReceived").ToString(),DataBinder.Eval(Container.DataItem, "DeliveryDate","{0: dd/MM/yy, hh:mm tt}").ToString(),DataBinder.Eval(Container.DataItem, "ReferedStores").ToString(),DataBinder.Eval(Container.DataItem, "OBDNumber").ToString(),DataBinder.Eval(Container.DataItem, "OutboundID").ToString(),DataBinder.Eval(Container.DataItem, "DriverName").ToString()) %>'></asp:Literal>
                                                      
                                                    </ItemTemplate>
                                                    </asp:TemplateField>
                                                    
                                                       
                                                        <asp:TemplateField  ItemStyle-CssClass="home"  HeaderText="P.Note" ItemStyle-Width="300"  >
                                                     <ItemTemplate>
                                                       <asp:HyperLink runat="server" ID="lnkHyperLink" ToolTip="Pick Note | Pick note with barcoded material codes to pick items for delivery."  Text="Pick Note" Font-Underline="false" ForeColor="Red"></asp:HyperLink>
                                                           
                                                         <asp:Literal runat="server" ID="ltLineCount" Text='<%# String.Format("[{0}]",DataBinder.Eval(Container.DataItem, "LineCount"))%>'></asp:Literal>
                                                         <img src='../Images/redarrowright.gif' border='0' />  
                                                    </ItemTemplate>
                                                    </asp:TemplateField>
                                                     
                                                     
                                                
                                                    
                                                    
                                                     <asp:TemplateField  ItemStyle-CssClass="home" ItemStyle-Width="80">
                                                     <ItemTemplate>
                                                      <asp:LinkButton ID="lnkReceivedelivery" runat="server" CssClass="GvLink" Visible="false" PostBackUrl ='<%# Eval("OutboundID", "OutboundDetails.aspx?obdid={0}&edittype=4") %>'   Text="Receive" ToolTip="Receive for PGI" />
                                                           
                                                         <asp:HyperLink ID="HyperLink1" Text="<nobr> Receive <img src='../Images/redarrowright.gif' border='0' /></nobr>" NavigateUrl='<%#   Eval("OutboundID", "OutboundDetails.aspx?obdid={0}&edittype=4")  %>' Font-Underline="false" runat="server" ></asp:HyperLink>
                                                      
                                                    </ItemTemplate>
                                                   
                                                    </asp:TemplateField>    
                                            </Columns>
                                            
                                    </asp:GridView>


                                 </td>
                             </tr>

                         </table>

                         <!-- POD Pending -->
                     </div>


                     
                 </div>

            </td>
        </tr>
    </table>
        </div>
    <asp:HiddenField ID="hidAccordionIndex" runat="server" Value="0" />
    <br /><br /><br /><br /><br /><br />

</asp:Content>
