<%@ Page Title=" BOM List :." Language="C#" MasterPageFile="~/mManufacturingProcess/Manufacturing.master" AutoEventWireup="true" CodeBehind="BillofMaterialList.aspx.cs" Inherits="MRLWMSC21.mManufacturingProcess.BillofMaterialList" %>
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

            var textfieldname = $("#<%= this.txtBOMRefNumberSearch.ClientID %>");
            DropdownFunction(textfieldname);
            $("#<%= this.txtBOMRefNumberSearch.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadBOMSearchList") %>',
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
            if (TextBox.value == "BOM Ref. No. ...")
                TextBox.value = "";
            TextBox.style.color = "#000000";
        }
        function focuslost(TextBox) {
            if (TextBox.value == "")
                TextBox.value = "BOM Ref. No. ...";
            TextBox.style.color = "#A4A4A4";
        }
    </script>

    <div style="padding-left:10px;padding-right:10px;">

    
   <table align="center" width="100%" >

       <tr>
           <td colspan="3">
               
               &nbsp;

           </td>
       </tr>
        
        <tr>
            
             <td valign="middle" width="25%">
               <asp:Label runat="server" ID="lblToatalBOMs"  CssClass="SubHeading3"></asp:Label>
              
           </td>
    

            <td class="FormLabels" align="right" width="74%">
                
                <asp:Panel runat="server" ID="pnlbillofmaterial" DefaultButton="lnkGetData">



                <asp:TextBox runat="server" Text="BOM Ref. # ..." onblur="javascript:focuslost(this)" Width="200" onfocus="ClearText(this)" ID="txtBOMRefNumberSearch"  SkinID="txt_Hidden_Req_Auto" />
                    &nbsp;&nbsp;
                <asp:LinkButton  ID="lnkGetData"   runat="server" OnClick="lnkGetData_Click" CssClass="ui-btn ui-button-large" >Search<span class="space fa fa-search"></span></asp:LinkButton>

                </asp:Panel>
            </td>

             <td class="FormLabels" align="right" style="padding-left:10px;" >
                 <asp:ImageButton ID="imgbtngvBoMList" runat="server"  ImageAlign="AbsMiddle" ImageUrl="../Images/excel_icon.jpg" Width="20" OnClick="imgbtngvBoMList_Click" ToolTip="Export To Excel" />
             </td>

        </tr>
       
       
       <tr>
           <td colspan="3">
               
               &nbsp;

           </td>
       </tr>
      
          <tr>
              <td class="FormLabels" align="center" colspan="3">

                  

                    <asp:GridView SkinID="gvLightGrayNew" ID="gvBoMList" runat="server" AutoGenerateColumns="false"  PagerSettings-Position="TopAndBottom"  AllowPaging="true" PageSize="25" OnPageIndexChanging="gvPROList_PageIndexChanging" AllowSorting="True" >
                <Columns>
                    <asp:TemplateField HeaderText="BOM Ref. #" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="250" HeaderStyle-HorizontalAlign="Left">
                      <ItemTemplate>  
                            <asp:Literal runat="server" ID="ltBOMRefNumber" Visible="false" Text='<%# String.Format("{0}  [{1}]", DataBinder.Eval(Container.DataItem, "BOMREV").ToString(), DataBinder.Eval(Container.DataItem, "LineItemCount").ToString()) %>' />

                          <asp:HyperLink runat="server" CssClass="HyperLinkStyle" ID="hypBOMRef" Text='<%# String.Format("{0}[{1}]", DataBinder.Eval(Container.DataItem, "BOMREV").ToString(), DataBinder.Eval(Container.DataItem, "LineItemCount").ToString())%>' NavigateUrl='<%# String.Format("~/mManufacturingProcess/BillofMaterial.aspx?bomid={0}",DataBinder.Eval(Container.DataItem, "BOMHeaderID").ToString() )%>'></asp:HyperLink>
                            
                        </ItemTemplate>
                    </asp:TemplateField>
          
                    <asp:TemplateField HeaderText="BOM Type" ItemStyle-Width="150" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="left">
                        <ItemTemplate>   
                            <asp:Literal runat="server" ID="ltBOMType" Text='<%# DataBinder.Eval(Container.DataItem, "BOMType") %>'/>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="UoM/Qty." ItemStyle-Width="100" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="left">
                        <ItemTemplate>   
                            <asp:Literal runat="server" ID="ltBOMuomqty" Text='<%# String.Format("{0}/{1}",DataBinder.Eval(Container.DataItem, "UoM"),DataBinder.Eval(Container.DataItem, "UoMQty"))  %>'/>
                        </ItemTemplate>
                    </asp:TemplateField>

                       <asp:TemplateField HeaderText="Product Name" ItemStyle-Width="250"  HeaderStyle-HorizontalAlign="left">
                        <ItemTemplate>   
                            <asp:Literal runat="server" ID="ltProductName" Text='<%# DataBinder.Eval(Container.DataItem, "ProductName") %>'/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70">
                        <ItemTemplate>
                            <a style="text-decoration:none;" href='<%# String.Concat("BillofMaterial.aspx?bomid=",DataBinder.Eval(Container.DataItem, "BOMHeaderID").ToString()) %>'>Edit  <image src="../Images/redarrowright.gif"></image></a>
                              
                            </ItemTemplate>                      
                    </asp:TemplateField>
                    
          </Columns>
            
        </asp:GridView>

              </td>
          </tr> 

   </table> 

        </div>

    <br /><br /><br /><br /><br /><br /><br /><br /><br />

</asp:Content>
