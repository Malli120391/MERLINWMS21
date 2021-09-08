<%@ Page Title=" Routing List :." Language="C#" MasterPageFile="~/mManufacturingProcess/Manufacturing.master" AutoEventWireup="true" CodeBehind="RoutingList.aspx.cs" Inherits="MRLWMSC21.mManufacturingProcess.RoutingList" MaintainScrollPositionOnPostback="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ManfContent" runat="server">
   
    
     <style>
        img
        {  
            border-style: none;
        }
          .btnSearch {
            padding-top: 1.3px;
            padding-bottom: 7.5px;
        }
    </style>

     <script>
         $(document).ready(function () {
             $('.module_login').css({
                 "border-color": "#1a79cf",
                 "border-width": "0px",
                 "border-style": "solid"
             });
             var texfieldname = $("#<%= this.txtRoutingRefNoSearch.ClientID %>");
             DropdownFunction(texfieldname);
             $("#<%= this.txtRoutingRefNoSearch.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadRoutingSearchList") %>',
                        data: "{ 'prefix': '" + request.term + "' }",
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
                minLength: 0
            });
        });

        function ClearText(TextBox) {
            if (TextBox.value == "Routing Ref. No...")
                TextBox.value = "";
            TextBox.style.color = "#000000";
        }
        function focuslost(TextBox) {
            if (TextBox.value == "")
                TextBox.value = "Routing Ref. No...";
            TextBox.style.color = "#A4A4A4";
        }

    </script>

     <table align="center" width="100%" cellpadding="5" cellspacing="3">
        
        <tr>
                
            <td width="25%" valign="middle">
                <br />
                 <asp:Label ID="lblTotalRoutings" CssClass="SubHeading3" runat="server"></asp:Label>
                
             </td>
            <td class="FormLabels" align="right"  width="74%">
                <br />
                <asp:Panel ID="pnlroutinglist" runat="server" DefaultButton="lnkGetData">
                <asp:DropDownList runat="server" Width="150px" ID="ddlRoutingDocType" CssClass="txt_Light_Small" ></asp:DropDownList>
                &nbsp;&nbsp;

                <asp:TextBox runat="server" Text="Routing Ref. No..." Width="200" onblur="javascript:focuslost(this)" onfocus="ClearText(this)" ID="txtRoutingRefNoSearch" SkinID="txt_Hidden_Req_Auto" ></asp:TextBox>
                    &nbsp;&nbsp;
                <asp:LinkButton  ID="lnkGetData"  runat="server"  OnClick="lnkGetData_Click" CssClass="ui-btn ui-button-large" >Search<span class="space fa fa-search"></span></asp:LinkButton>

                </asp:Panel>
            </td>
             <td class="FormLabels" >
                 <br />
                 <asp:ImageButton ID="imgbtngvRoutingList" runat="server"  ImageAlign="AbsMiddle" ImageUrl="../Images/excel_icon.jpg" Width="20" OnClick="imgbtngvRoutingList_Click" ToolTip="Export To Excel" />
             </td>
        </tr>
      
         
          <tr>
              <td class="FormLabels" align="center" colspan="3">
                   
                    <asp:GridView SkinID="gvLightBlueNew" ID="gvRoutingList" runat="server" AutoGenerateColumns="false"  PagerSettings-Position="TopAndBottom"  AllowPaging="true" PageSize="30" OnPageIndexChanging="gvRoutingList_PageIndexChanging" AllowSorting="True" >
                <Columns>
                    <asp:TemplateField HeaderText="Routing Ref. #" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="350" HeaderStyle-HorizontalAlign="Left">
                      <ItemTemplate>  
                            <asp:Literal runat="server" ID="ltRoutingRefNo" Visible="false" Text='<%# String.Format("{0}[{1}]", DataBinder.Eval(Container.DataItem, "MCode").ToString(), DataBinder.Eval(Container.DataItem, "LineItemCount").ToString()) %>' />
                           
                            <asp:HyperLink runat="server" CssClass="HyperLinkStyle" ID="hypRoutRefNo" Text='<%# String.Format("{0}[{1}]", DataBinder.Eval(Container.DataItem, "MCode").ToString(), DataBinder.Eval(Container.DataItem, "LineItemCount").ToString()) %>'  NavigateUrl='<%# String.Format("~/mManufacturingProcess/Routing.aspx?routid={0}",DataBinder.Eval(Container.DataItem, "RoutingHeaderID").ToString() )  %>' ></asp:HyperLink>
                                                      
                        </ItemTemplate>
                    </asp:TemplateField>
          
                    <asp:TemplateField HeaderText="Routing Type" ItemStyle-Width="150" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="left">
                        <ItemTemplate>   
                            <asp:Literal runat="server" ID="ltRoutingDocumentType" Text='<%# DataBinder.Eval(Container.DataItem, "RoutingDocumentType") %>'/>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="BOM Ref. #" ItemStyle-Width="250" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="left">
                        <ItemTemplate>   
                            <asp:Literal runat="server" ID="ltRoutingName" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "BOMRefNo") %>'/>

                            <asp:HyperLink runat="server" ID="hypBOMRefNo" CssClass="HyperLinkStyle" Text='<%# DataBinder.Eval(Container.DataItem, "BOMRefNo") %>' NavigateUrl='<%# String.Format("~/mManufacturingProcess/BillofMaterial.aspx?bomid={0}",DataBinder.Eval(Container.DataItem, "BOMHeaderID")) %>' ></asp:HyperLink>

                        </ItemTemplate>
                    </asp:TemplateField>

                                                     

                    <asp:TemplateField  HeaderText="Supplier Name" ItemStyle-Width="230" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="left">
                        <ItemTemplate>   
                            <asp:Literal runat="server" ID="ltSupplierName" Text='<%#DataBinder.Eval(Container.DataItem, "SupplierName") %>'/>
                        </ItemTemplate>
                    </asp:TemplateField>

                    
                    
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70">
                        <ItemTemplate>
                            <a style="text-decoration:none;" href='<%# String.Concat("Routing.aspx?routid=",DataBinder.Eval(Container.DataItem, "RoutingHeaderID").ToString()) %>'>Edit  <image src="../Images/redarrowright.gif"></image></a>
                              
                            </ItemTemplate>                      
                    </asp:TemplateField>
                    
          </Columns>
            
        </asp:GridView>
                  <br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
              </td>
          </tr> 

   </table> 


    <br /><br /><br /><br /><br /><br />
</asp:Content>
