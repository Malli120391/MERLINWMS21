<%@ Page Title="PO List:." Language="C#" MasterPageFile="~/mOrders/OrdersMaster.master" AutoEventWireup="true" CodeBehind="POList.aspx.cs" Inherits="MRLWMSC21.mOrders.POList"   EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="OrdersContent" runat="server">
    <link rel="stylesheet" type="text/css" href="../App_Themes/Orders/Orders_Style.css" media="screen" />
   <style>
        img
        {  
            border-style: none;
        }

       .gvLightSkyBlueNew_footerGrid {
            display: none;
       }


             .gvLightSkyBlueNew_pager span {
    background: var(--sideNav-bg) !important;
    color: #fff !important;
}
     
       .gvLightSkyBlueNew_pager a {
    background: #fff !important;
    color: #0e0e0e !important;
}
    </style>
    <script>
       
            $(document).ready(function () {
                var textfieldname = $('#<%=this.txtPONumber.ClientID%>');
                DropdownFunction(textfieldname);

                $('#<%=this.txtPONumber.ClientID%>').autocomplete({
                       
                    source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadPONumbersfor3PL") %>',
                            data: "{ 'prefix': '" + request.term + "','StatusID':'" + document.getElementById('<%=this.ddlSelectStatus.ClientID%>').options[document.getElementById('<%=this.ddlSelectStatus.ClientID%>').selectedIndex].value + "','TenantID':'" + document.getElementById('<%= this.hifTenant.ClientID %>').value + "'}",
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

                var textfieldname = $('#<%=this.txtTenant.ClientID%>');
                DropdownFunction(textfieldname);

                $('#<%=this.txtTenant.ClientID%>').autocomplete({

                    source: function (request, response) {
                        // alert(request.term);
                        $.ajax({
                            url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadTenantDataFor3PLSO") %>',
                            data: "{ 'prefix': '" + request.term + "'}",//<=cp.TenantID%>
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
                    select: function (e, i) {

                        $("#<%=hifTenant.ClientID %>").val(i.item.val);
                        $("#hiTenantName").val(i.item.label);
                    },
                    minLength: 0
                });
            })

    </script>
<div class="container">
    <table border="0" class="" align="center" cellpadding="5" cellspacing="5">

                    <tr class="ListHeaderRow">
                            <td class ="FormLabels" align="right">
                                    <asp:Panel ID="pnlHeaderRow" runat="server">
                                        <div class="row">
                                            <div class="col m3 s4">
                                                <div class="flex">
                                                    <asp:TextBox ID="txtTenant" SkinID="txt_Hidden_Req_Auto" runat="server"  required="" />
                                                    <label><%= GetGlobalResourceObject("Resource", "Tenant")%></label>
                                                    <asp:HiddenField runat="server" ID="hifTenant" Value="0" />
                                                </div>
                                            </div>
                                            <div class="col m3 s4">
                                                <div class="flex">
                                                    <div>
                                                        <asp:DropDownList ID="ddlSelectStatus" runat="server" required=""  />
                                                        <label><%= GetGlobalResourceObject("Resource", "InwardStatus")%></label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col m3 s4">
                                                <div class="flex">
                                                    <asp:TextBox ID="txtPONumber" required="" SkinID="txt_Hidden_Req_Auto" runat="server" />
                                                    <label><%= GetGlobalResourceObject("Resource", "InwardNumber")%></label>
                                                </div>
                                            </div>
                                            <div class="col m3 s4 p0">
                                                <gap5></gap5>
                                              <flex>  <asp:LinkButton ID="lnkGetData" runat="server" CssClass="btn btn-primary" OnClick="lnkGetData_Click">
                                                <%= GetGlobalResourceObject("Resource", "Search")%>  <%=MRLWMSC21Common.CommonLogic.btnfaSearch %>
                                                </asp:LinkButton>
                                                  <asp:LinkButton ID="LinkButton1" runat="server" PostBackUrl="~/mOrders/PODetailsInfo.aspx" CssClass="btn btn-primary">
                                                       <%= GetGlobalResourceObject("Resource", "Add")%>  <%=MRLWMSC21Common.CommonLogic.btnfaNew %>
                                                  </asp:LinkButton>
                                                <asp:LinkButton ID="imgbtngvPOList" CssClass="btn btn-primary" runat="server" OnClick="imgbtngvPOList_Click"><%= GetGlobalResourceObject("Resource", "Export")%> <%=MRLWMSC21Common.CommonLogic.btnfaExcel %></asp:LinkButton></flex>
                                            </div>
                                        </div>

                                    </asp:Panel>
                            </td>
                       </tr>
       
                    <tr class="ListDataRow">
                        <td>	
    								    <asp:GridView SkinID="gvLightSkyBlueNew"    ID="gvPOList" runat="server" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"  PagerSettings-Position="Bottom" OnRowDeleting="gvPOList_RowDeleting" AllowPaging="true" PageSize="25" AllowSorting="True"  HorizontalAlign="Left"   OnSorting="gvPOList_Sorting" OnPageIndexChanging="gvPOList_PageIndexChanging" OnRowDataBound="gvPOList_RowDataBound" Width="899px" >
                                            <Columns>

                                                <asp:TemplateField ItemStyle-Width="130" HeaderText= "<%$Resources:Resource,OrderRefNo%>" ItemStyle-HorizontalAlign="left" >
                                                    <ItemTemplate>
              
                                                        <asp:Literal runat="server" ID="ItPONumber" Text='<%# String.Format("{0} [{1}]",DataBinder.Eval(Container.DataItem, "PONumber"),DataBinder.Eval(Container.DataItem, "LineItemCount"))  %>'/>
         
                                                        <asp:Literal runat="server" ID="ItPOHeaderID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "POHeaderID") %>'/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="<%$Resources:Resource,Account%>" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="140" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="ItAccName" ToolTip='<%# Bind("Account") %>' Text='<%# DataBinder.Eval(Container.DataItem, "AccountCode") %>' />
                                    <%--  <asp:Literal runat="server" ID="ltTenant" Text='<%# DataBinder.Eval(Container.DataItem, "TenantName").ToString() %>' />--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="140" HeaderText="<%$Resources:Resource,Tenant%>"  ItemStyle-HorizontalAlign="left" >
                                                    <ItemTemplate>
                                                   <asp:Label runat="server" ID="ItCompanyName" ToolTip='<%# Bind("TenantName") %>' Text='<%# DataBinder.Eval(Container.DataItem, "CompanyName") %>' />
                                                       <%-- <asp:Literal runat="server" ID="ItCompanyName" Text='<%# DataBinder.Eval(Container.DataItem, "CompanyName")  %>'/>--%>
                                                        <asp:Literal runat="server" ID="ItTenantID" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "TenantID") %>'/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-Width="150"  HeaderText="<%$Resources:Resource,Supplier%>"  ItemStyle-HorizontalAlign="left">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="ItCompanyName" ToolTip='<%# Bind("SupplierName") %>' Text='<%# DataBinder.Eval(Container.DataItem, "SupplierCode") %>' />
                                                       <%-- <asp:Literal runat="server" ID="ltSupplier" Text='<%# DataBinder.Eval(Container.DataItem, "SupplierCode") %>'/>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField ItemStyle-Width="150"  HeaderText= "<%$Resources:Resource,OrderType%>" ItemStyle-HorizontalAlign="left" >
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="ltPOtype" Text='<%# DataBinder.Eval(Container.DataItem, "POType") %>'/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="230" Visible="false" HeaderText= "<%$Resources:Resource,DepartmentDivision%>"  >
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="ltDepartMent_division" Text='<%# String.Format("{0} / {1}",DataBinder.Eval(Container.DataItem, "Department"),DataBinder.Eval(Container.DataItem, "Division")) %>'/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="90" HeaderText="Requested By" Visible="false" >
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="ltCoordinator" Text='<%# DataBinder.Eval(Container.DataItem, "Coordinator").ToString() %>'/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 
                                                <asp:TemplateField ItemStyle-Width="90" HeaderText= "<%$Resources:Resource,OrderDate%>"  ItemStyle-HorizontalAlign="left">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="ltPODate" Text='<%# DataBinder.Eval(Container.DataItem, "PODate","{0:dd-MMM-yyyy}") %>'/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                               
                                                <asp:TemplateField ItemStyle-Width="90" HeaderText="Due Date" Visible="false" >
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="ltDueDate" Text='<%# DataBinder.Eval(Container.DataItem, "DateDue","{0:dd-MMM-yyyy}") %>'/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="150" HeaderText= "<%$Resources:Resource,InShipRefNo%>" ItemStyle-HorizontalAlign="left">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="ltStatus" Text='<%# DataBinder.Eval(Container.DataItem, "StoreRefNos") %>'/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="90" HeaderText= "<%$Resources:Resource,Status%>" ItemStyle-HorizontalAlign="left">
                                                    <ItemTemplate>
                                                        <asp:Literal runat="server" ID="ltStatus" Text='<%# DataBinder.Eval(Container.DataItem, "StatusName") %>'/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 
                                                    <asp:TemplateField ItemStyle-Width="80" HeaderText= "<%$Resources:Resource,Edit%>" ControlStyle-CssClass="txtCenteralign">
                                                    <ItemTemplate>
                                                        <a style="text-decoration:none;" id='<%#"PODID_"+DataBinder.Eval(Container.DataItem, "POHeaderID").ToString() %>' href='<%# String.Concat("PODetailsInfo.aspx?poid=",DataBinder.Eval(Container.DataItem, "POHeaderID").ToString()+"&tid="+DataBinder.Eval(Container.DataItem, "TenantID")) %>' > <i class="material-icons ss">mode_edit</i></a>

                                                       
                                                        
                                                    </ItemTemplate>
                            
                                                      
                                                </asp:TemplateField>
                                             
                                
                                            </Columns>
                                            <EmptyDataTemplate>
                                        <div align="center">No Data Found</div>
                                    </EmptyDataTemplate>
                                            <PagerSettings FirstPageText="&amp;lt;&amp;lt;First Page" LastPageText="&amp;gt;&amp;gt;"
                                                Mode="NumericFirstLast" PageButtonCount="15" />                  
                                    </asp:GridView>
                            <br/>
						</td>
							
                    </tr>

    </table>

    </div>


</asp:Content>
