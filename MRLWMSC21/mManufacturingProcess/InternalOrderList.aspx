<%@ Page Title="NC Order List:." Language="C#" MasterPageFile="~/mManufacturingProcess/Manufacturing.master" AutoEventWireup="true" CodeBehind="InternalOrderList.aspx.cs" Inherits="MRLWMSC21.mManufacturingProcess.InternalOrderList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ManfContent" runat="server">
    <style>
        img
        {  
            border-style: none;
        }
    </style>

    <script>
        $(document).ready(function () {


            var textfieldname = $("#<%= this.txtIORefNoSearch.ClientID %>");
            DropdownFunction(textfieldname);
            $("#<%= this.txtIORefNoSearch.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadIORefNoList") %>',
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
            if (TextBox.value == "NC Ref. No...") {
                TextBox.value = "";
                TextBox.style.color = "#000000";
            }
        }
        function focuslost(TextBox) {
            if (TextBox.value == "") {
                TextBox.value = "NC Ref. No...";
                TextBox.style.color = "#A4A4A4";
            }
        }
    </script>



    <table align="center" width="90%"  class="ListTable">
        <tr class="ListHeaderRow">
                
            <td class="FormLabels" align="right">
                <asp:Panel ID="pnlNCList" runat="server" DefaultButton="lnkGetData">
                        <asp:TextBox runat="server" SkinID="txt_Auto" Text="NC Ref. No..." onblur="javascript:focuslost(this)" onfocus="ClearText(this)" ID="txtIORefNoSearch"></asp:TextBox>
                        <asp:LinkButton  ID="lnkGetData"  CssClass="ui-btn ui-button-large"  runat="server" OnClick="lnkGetData_Click" >Search<%=MRLWMSC21Common.CommonLogic.btnfaSearch %></asp:LinkButton>&nbsp;&nbsp;
                         <asp:ImageButton ID="imgbtngvInternalOrderList" runat="server"  ImageAlign="AbsMiddle" ImageUrl="../Images/excel_icon.jpg" Width="20" OnClick="imgbtngvInternalOrderList_Click" ToolTip="Export To Excel" />
                </asp:Panel>
            </td>
        </tr>
        
          <tr class="ListDataRow">
              <td>
                  <table >
                      <tr>
                          <td>
                                  <div id="JOD" style="width:20px;height:10px;background-color:#ef4a4a;"></div> 
                          </td>
                          <td class="FormLabels">
                                  <span class="SubHeading"> Indicates NC order has insufficient materials to release</span>
                          </td>
                      </tr>
                  </table>
                <asp:GridView SkinID="gvLightGrayNew" ID="gvInternalOrderList" OnRowDataBound="gvInternalOrderList_RowDataBound" runat="server" AutoGenerateColumns="false"  PagerSettings-Position="TopAndBottom"  AllowPaging="true" PageSize="10" OnPageIndexChanging="gvInternalOrderList_PageIndexChanging" AllowSorting="True" >
                        <Columns>
                            <asp:TemplateField HeaderText="NC Ref. No." ItemStyle-HorizontalAlign="Left" ItemStyle-Width="140" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>  
                                    <asp:Literal runat="server" ID="ltIORefNo" Text='<%# String.Format("{0}  [{1}]", DataBinder.Eval(Container.DataItem, "IORefNo").ToString(), DataBinder.Eval(Container.DataItem, "LineItemCount").ToString()) %>' />
                            
                                </ItemTemplate>
                            </asp:TemplateField>
          
                            <asp:TemplateField HeaderText="Job Order Ref. #" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="left">
                                <ItemTemplate>   
                                    <asp:Literal runat="server" ID="ltPRORefNo" Text='<%# DataBinder.Eval(Container.DataItem, "PRORefNo") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>

                     

                            <asp:TemplateField  HeaderText="Workstation" ItemStyle-Width="230" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="left">
                                <ItemTemplate>   
                                    <asp:Literal runat="server" ID="ltWorkCenter" Text='<%#DataBinder.Eval(Container.DataItem, "WorkCenter") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                                <asp:TemplateField HeaderText="Reason for Request" ItemStyle-Width="130" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>   
                                    <asp:Literal runat="server" ID="ltReasonForInternalOrderRequest" Text='<%# DataBinder.Eval(Container.DataItem, "ReasonForInternalOrderRequest") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" ItemStyle-Width="130" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>   
                                    <asp:Literal runat="server" ID="ltStatus" Text='<%# DataBinder.Eval(Container.DataItem, "InternalOrderStatus") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70">
                                <ItemTemplate>
                                    <a style="text-decoration:none;" href='<%# String.Concat("InternalOrder.aspx?ioid=",DataBinder.Eval(Container.DataItem, "InternalOrderHeaderID").ToString()) %>'>Edit  <image src="../Images/redarrowright.gif"></image></a>
                              
                                    </ItemTemplate>                      
                            </asp:TemplateField>
                    
                    </Columns>
                     <PagerSettings FirstPageText="&amp;lt;&amp;lt;First Page" LastPageText="Last Page&amp;gt;&amp;gt;" Mode="NumericFirstLast" PageButtonCount="15" />
                </asp:GridView>
                   <br />
              </td>
          </tr> 

   </table> 



</asp:Content>
