<%@ Page Title="Issue List" Language="C#" MasterPageFile="~/mOrders/OrdersMaster.master" AutoEventWireup="true" CodeBehind="ToolManagementList.aspx.cs" Inherits="MRLWMSC21.mOrders.ToolManagementList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="OrdersContent" runat="server">
    <link rel="stylesheet" type="text/css" href="../App_Themes/Orders/Orders_Style.css" media="screen" />
    <style>
        img
        {  
            border-style: none;
        }
    </style>
    <script>

        $(document).ready(function () {
            var textfieldname = $('#<%=this.SoNumberSearch.ClientID%>');
            DropdownFunction(textfieldname);
            $('#<%=this.SoNumberSearch.ClientID%>').autocomplete({

                source: function (request, response) {
                    // alert('ffffffff');
                    $.ajax({
                        url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadSONumbersList") %>',
                        data: "{ 'prefix': '" + request.term + "','IsToolItem':'1','TenentID':'" +<%=cp.TenantID%> +"'}",
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

    <script>
        function ClearText(TextBox) {
            if (TextBox.value == "Issue Ref.#...") {
                TextBox.value = "";
                TextBox.style.color = "#000000";
            }
        }
        function focuslost(TextBox) {
            if (TextBox.value == "") {
                TextBox.value = "Issue Ref.#...";
                TextBox.style.color = "#A4A4A4";
            }
        }
    </script>
    <div class="dashed"></div>
    <div class="pagewidth">
        <table align="center" class="ListTable">
         
        <tr class="ListHeaderRow">
                
            <td class="ListHeaderRow" align="right">
                <asp:Panel ID="pnltoolmgtlist" runat="server" DefaultButton="lnkGetData">
                Select Status:
                <asp:DropDownList ID="ddlSelectStatus" runat="server" Height="34" Width="100" /> &nbsp;&nbsp;
                <asp:TextBox runat="server" Text="Issue Ref.#..." SkinID="txt_Hidden_Req_Auto"  onblur="javascript:focuslost(this)" onfocus="ClearText(this)" ID="SoNumberSearch"></asp:TextBox>
                <asp:LinkButton  ID="lnkGetData" CssClass="ui-btn ui-button-large"  runat="server" OnClick="lnkGetData_Click" >
                   Search <%=MRLWMSC21Common.CommonLogic.btnfaSearch%>
                </asp:LinkButton>&nbsp;&nbsp;
                <asp:ImageButton ID="imgbtngvToolmgtlist" runat="server" ImageAlign="AbsMiddle" ImageUrl="../Images/excel_icon.jpg" Width="20" OnClick="imgbtngvToolmgtlist_Click" ToolTip="Export To Excel" />
                </asp:Panel>
            </td>
        </tr>
        
        <tr class="ListDataRow">
              <td class="FormLabels" align="center" valign="top" width="85%" height="300px">

                    <asp:GridView SkinID="gvLightSeaBlueNew" ID="gvTMOList" runat="server" AutoGenerateColumns="false" OnPageIndexChanging="gvTMOList_PageIndexChanging"  PagerSettings-Position="TopAndBottom"  AllowPaging="true" PageSize="10"  AllowSorting="True">
                <Columns>
                    <asp:TemplateField HeaderText="Issue Ref. #" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="140" HeaderStyle-HorizontalAlign="Center">
                      <ItemTemplate>  
                            <asp:Literal runat="server" ID="SONumber" Text='<%# DataBinder.Eval(Container.DataItem, "SONumber").ToString() %>' />
                            <asp:Literal runat="server" ID="Literal1" Text="["/>
                             <asp:Literal runat="server" ID="Literal2" Text='<%# DataBinder.Eval(Container.DataItem, "LineItemCount").ToString() %>'/>
                            <asp:Literal runat="server" ID="Literal3" Text="]"/>
                            <asp:Literal runat="server" ID="SOHeaderID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "SOHeaderID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
          
                    <asp:TemplateField HeaderText="Employee" ItemStyle-Width="130" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>   
                            <asp:Literal runat="server" ID="UserName" Text='<%# DataBinder.Eval(Container.DataItem, "UserName") %>'/>
                        </ItemTemplate>
                    </asp:TemplateField>
                   
                     <asp:TemplateField HeaderText="Issue Date" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="90" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>   
                            <asp:Literal runat="server" ID="ltSODate" Text='<%#DataBinder.Eval(Container.DataItem, "SODate","{0:dd/MM/yyyy}") %>'/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="OBD Number's" ItemStyle-Width="90">
                        <ItemTemplate>
                            <asp:Literal ID="ltobdnumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "OBDNumbers") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>   
                            <asp:Literal runat="server" ID="SOStatus" Text='<%# DataBinder.Eval(Container.DataItem, "StatusName") %>'/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70">
                        <ItemTemplate>
                            <a style="text-decoration:none;" href='<%# String.Concat("ToolManagement.aspx?tmoid=",DataBinder.Eval(Container.DataItem, "SOHeaderID").ToString()) %>'>Edit  <image src="../Images/redarrowright.gif"></image></a>
                              
                            </ItemTemplate>                      
                    </asp:TemplateField>
                    
          </Columns>
            
        </asp:GridView>
                  <br />
                  <br />
              </td>
          </tr> 

   </table> 
        </div>


</asp:Content>
