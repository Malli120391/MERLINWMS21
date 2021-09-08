<%@ Page Title="" Language="C#" MasterPageFile="~/mManufacturingProcess/Manufacturing.master" AutoEventWireup="true" CodeBehind="EngineeringChangeOrders.aspx.cs" Inherits="MRLWMSC21.mManufacturingProcess.EngineeringChangeOrders" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ManfContent" runat="server">

    <asp:ScriptManager runat="server" EnablePartialRendering="true" ID="PODetailsScript" SupportsPartialRendering="true"></asp:ScriptManager>
    <style>
        .ShowDate {
            font-size:10pt;
            color:#0099FF;
        }
        .ShowHeaderDetails {
            font-size:14pt;
            color:#2581e5;
        }
        .ShowNewRevision {
            color:orange;
        }
        .ShowOldRevision {
            color:green
        }
    </style>

    <script type="text/javascript">
       
            $(document).ready(function () {
           var TextFieldName = $("#<%= this.atcPartNumber.ClientID %>");
           DropdownFunction(TextFieldName);
            
            $("#<%= this.atcPartNumber.ClientID %>").autocomplete({

                source: function (request, response) {
                $.ajax({
                    url: '<%=ResolveUrl("~/mWebServices/FalconWebService.asmx/LoadMCodeForECO") %>',
                    data: "{ 'prefix': '" + request.term + "','TenantID':'" + <%=cp.TenantID%> + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            if (data.d == "") {
                                alert('No Clone Material');
                            }
                            response($.map(data.d, function (item) {
                                return {

                                    label: item.split('~')[0].split('`')[0],
                                    description: item.split('~')[0].split('`')[1] == undefined ? "" : " <font color='#086A87'  >" + item.split('~')[0].split('`')[1] + "</font>",
                                    val: item.split('~')[1]
                                }
                            }))
                        }
                    });
                },
            select: function (e, i) {

                $("#<%=hifPartNumber.ClientID %>").val(i.item.val);

                },
                minLength: 0
            }).data("autocomplete")._renderItem = function (ul, item) {
                // Inside of _renderItem you can use any property that exists on each item that we built
                // with $.map above */
                return $("<li></li>")
                    .data("item.autocomplete", item)
                    .append("<a>" + item.label + "" + item.description + "</a>")
                    .appendTo(ul)
            };         
            
                
        });

      
        
    </script>
    
    <table width="80%" align="center" height="360">
    <tr height="60px">
        <td align="right">
            Part Number:
            <asp:TextBox ID="atcPartNumber" runat="server" SkinID="txt_Auto" ></asp:TextBox>
            <asp:HiddenField ID="hifPartNumber" runat="server" />
            
            <asp:DropDownList ID="ddlRoutingDocumentType" runat="server" Height="34">
                <asp:ListItem Text="Select RoutingType" Value="0"></asp:ListItem>
                <asp:ListItem Text="Sleeve Printing" Value="1"></asp:ListItem>
                <asp:ListItem Text="Main Assembly" Value="2"></asp:ListItem>
                <asp:ListItem Text="Wire Marking" Value="3"></asp:ListItem>
            </asp:DropDownList>
            <asp:LinkButton ID="lnkGetDetails" runat="server" OnClientClick="showAsynchronus();" OnClick="lnkGetDetails_Click" CssClass="ui-btn ui-button-large">Get Details<%=MRLWMSC21Common.CommonLogic.btnfaFilter %></asp:LinkButton>
        </td>
              
    </tr>
    
    <tr>
        <td colspan="3" valign="top">
            <asp:Label ID="lbMessage" Visible="false" runat="server" class="ShowHeaderDetails" Text="Job Order History for transfer transactions to New Routing Revision"></asp:Label><br />
            <asp:GridView ID="gvECO" runat="server" OnRowDataBound="gvECO_RowDataBound" SkinID="gvLightSteelBlueNew" AutoGenerateColumns="false" OnRowCommand="gvECO_RowCommand">
                <Columns>
                   <asp:TemplateField HeaderText="Material Revision" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle">
                        <ItemTemplate>
                            <asp:Label runat="server" CssClass="ShowNewRevision" ID="lbNewMaterialMasterRevision" Text='<%# DataBinder.Eval(Container.DataItem, "NewMaterialRevision").ToString() %>' />
                             <asp:Label CssClass="ShowDate"  runat="server" ID="lbMaterialMasterRevisionEffectiveDate" Text='<%# String.Format("[{0}]",DataBinder.Eval(Container.DataItem, "MaterialRevisionEffectiveDate").ToString())  %>' />
                            <img src="../Images/Question-12.png" Title="<%# DataBinder.Eval(Container.DataItem, "Description").ToString() %>" />
                            <br />
                            
                            <asp:Label runat="server" CssClass="ShowOldRevision" Visible=<%#(DataBinder.Eval(Container.DataItem, "MaterialMasterRevision").ToString()==DataBinder.Eval(Container.DataItem, "NewMaterialRevision").ToString()?false:true)%> ID="lbMatrialMasterRevision" Text='<%#"<img src=\"../Images/Uparrow-12.png\"/><br />"+ DataBinder.Eval(Container.DataItem, "MaterialMasterRevision").ToString() %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                     
                    <asp:TemplateField HeaderText="BOM Revision" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle">
                        <ItemTemplate>
                            <asp:Label runat="server" CssClass="ShowNewRevision" ID="lbCloneBoMRevision" Text='<%# DataBinder.Eval(Container.DataItem, "NewBoMRevision").ToString() %>' />
                            <asp:Label CssClass="ShowDate"  runat="server" ID="lbBoMEffectiveDate" Text='<%# String.Format("[{0}]",DataBinder.Eval(Container.DataItem, "BoMRevisionEffectiveDate").ToString())  %>' />
                            <img src="../Images/Question-12.png" Title="<%# DataBinder.Eval(Container.DataItem, "BoMRemarks").ToString() %>"/>
                            <br />
                            <asp:Label runat="server" CssClass="ShowOldRevision" ID="lbBoMRevision" Text='<%#"<img src=\"../Images/Uparrow-12.png\"/><br />"+ DataBinder.Eval(Container.DataItem, "BoMRevision").ToString() %>' Visible=<%#(DataBinder.Eval(Container.DataItem, "NewBoMRevision").ToString()==DataBinder.Eval(Container.DataItem, "BoMRevision").ToString()?false:true)%> />
                        </ItemTemplate>
                    </asp:TemplateField>
                   
                    <asp:TemplateField HeaderText="Routing Revision" ItemStyle-HorizontalAlign="left" ItemStyle-VerticalAlign="Middle">
                        <ItemTemplate>
                            <asp:Label runat="server" CssClass="ShowNewRevision" ID="lbCloneRoutingRevision" Text='<%# DataBinder.Eval(Container.DataItem, "NewRoutingRevision").ToString() %>' />
                             <asp:Label CssClass="ShowDate"  runat="server" ID="lbRoutingRevisionEffectiveDate" Text='<%# String.Format("[{0}]",DataBinder.Eval(Container.DataItem, "RoutingRevisionEffectiveDate").ToString())  %>' />
                            <img src="../Images/Question-12.png" Title="<%# DataBinder.Eval(Container.DataItem, "RoutingRemarks").ToString() %>"/>
                            <br />
                            <asp:Label runat="server" CssClass="ShowOldRevision" ID="lbRoutingRevision" Text='<%#"<img src=\"../Images/Uparrow-12.png\"/><br />"+ DataBinder.Eval(Container.DataItem, "RoutingRevision").ToString() %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Routing Type" Visible="false">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="ltRoutingType" Text='<%# DataBinder.Eval(Container.DataItem, "RoutingDocumentType").ToString() %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Kit Code list">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="ltJobList" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "JobList").ToString() %>' />
                            <asp:Panel ID="pnlJobList" runat="server"></asp:Panel>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
        </td>
    </tr>   
                
    </table>
</asp:Content>
